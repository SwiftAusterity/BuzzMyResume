(function($) {
    $.fn.twitterize = function(params, options) {
        // Default parameters to send to the ajax call
        var defaultParams = {
            batchSize: 100
        };

        // Default options for the streaming tweets behavior
        var defaultOptions = {
            stream: true,
            source: 'http://search.twitter.com/search.json',
            callback: '?',
            tweetSelector: '.tweetWrap',
            template: '<div id="twt-{%i}" class="tweetWrap"><div class="tweet group"><a href="{%p}" title="{%un}"> <img alt="{%un}" width="48" height="48" src="{%a}" onerror="avatarOnError(this, \'normal\', \'fail\');" onload="avatarOnError(this, \'normal\', \'load\');" title="{%u}" class="tw-user" data-tw-user="{%ui}" /></a> <div class="{%tc}"><p> <strong><a href="{%p}">{%u}</a> </strong>: {%t} </p> <a href="{%dlink}"><em>{%d}</em> </a>{%promote} <a href="{%spfx}/Tweets/Retweet/{%i}" class="retweet" target="_blank" rel="nofollow">retweet</a> <a class="thumb" title="{%ln}" href="{%spfx}{%ll}"> <img height="48" width="48" src="{%lt}" alt="{%ln}" title="{%tlt}" onerror="linkOnError(this, \'fail\');" onload="linkOnError(this, \'load\');" /> </a> </div></div></div>',
            max: 20,
            moreSize: 20,
            lastId: -1
        };

        params = $.extend({}, defaultParams, params);
        options = $.extend({}, defaultOptions, options);

        var url = options.source + '?callback=' + options.callback;

        var holder = this;

        var controls = {
            playBtn: $('#play-btn'),
            pauseBtn: $('#pause-btn'),
            moreBtn: $('#more-btn'),
            status: $('#tweet-status')
        }

        STLTweets.tweetMeta = {
            tweetIds: [],
            updateId: -1,
            firstRenderedId: -1,
            firstVisibleId: -1,
            lastVisibleId: options.lastId,
            missedCount: 0
        };

        var paused = false;

        var preloaded = false;

        /* getTweets()
        --------------------------------------------------------------------------------*/
        function getTweets(update) {
            var _params = $.extend({}, params);

            // If it's an update call, then include the 'sinceId'
            if (update) {
                $.extend(_params, { sinceId: (!paused ? STLTweets.tweetMeta.firstVisibleId : STLTweets.tweetMeta.firstRenderedId) });
            } else {
                // Otherwise it's the initial load, so pull a small batch
                $.extend(_params, { batchSize: 20 });
            }

            // Execute the ajax call to get more tweets
            $.getJSON(url, _params,
        function(data) {
            getNextTweet(data.results.reverse(), 0, data.feedSpeed, !update, data.resultTime, data.categoryId);

            // If resuming play, show the Pause button again
            if (!paused) {
                $('#busy-loading, #busy-play').remove();
                if (options.stream) {
                    controls.pauseBtn.showControlButton();
                }
            }

            // Things to do on initial load
            if (!update) {
                preloaded = true;
                if (!paused) {
                    if (options.stream) {
                        controls.pauseBtn.showControlButton();
                    }
                    controls.moreBtn.showControlButton();
                }

                // No results on initial load
                if (!data.count) {
                    logBefore('There are no tweets currently available for this category/search.');
                }
            }

            var serverTime = new Date(data.resultTime);
            updateRelativeDates(serverTime, data.categoryId);
        }
      );
        }

        /* getNextTweet()
        --------------------------------------------------------------------------------*/
        function getNextTweet(tweetList, index, feedSpeed, immediateDraw, resultTime, categoryId) {
            if (index >= tweetList.length && options.stream) {
                if (immediateDraw) {
                    STLTweets.tweetMeta.updateId = getTweets(true);
                }
                else {
                    STLTweets.tweetMeta.updateId = setTimeout(function() { getTweets(true); }, feedSpeed);
                }
            }
            else {
                //Min of .5 seconds derived from a total max of 10 seconds
                var adjustedFeedSpeed = Math.min(feedSpeed, Math.max(500, 20000 / Math.max(1, (tweetList.length - index) / 5)));

                var item = tweetList[index];
                var serverTime = new Date(resultTime);

                STLTweets.tweetMeta.firstRenderedId = item.id;
                if (!paused) {
                    STLTweets.tweetMeta.firstVisibleId = item.id;
                    renderBefore(item, serverTime, categoryId, { animate: !immediateDraw });
                }
                else {
                    STLTweets.tweetMeta.missedCount++;
                    updatePausedStatus();
                }

                if (immediateDraw) {
                    getNextTweet(tweetList, index + 1, adjustedFeedSpeed, immediateDraw, resultTime, categoryId);
                }

                // Schedule the remaining tweets to display later to make it look like
                // there is consistent activity on this data stream
                else {
                    var tweetTimeoutId =
                    setTimeout(
                        function() {
                            //Schedule a new tweetGrab or schedule the next tweet
                            STLTweets.tweetMeta.tweetIds.shift();
                            getNextTweet(tweetList, index + 1, adjustedFeedSpeed, immediateDraw, resultTime, categoryId);
                        }
                        , feedSpeed
                    );

                    STLTweets.tweetMeta.tweetIds.push({ id: item.id, delay: adjustedFeedSpeed, timeoutId: tweetTimeoutId });
                }
            }
        }

        /* getOlderTweets()
        --------------------------------------------------------------------------------*/
        function getOlderTweets() {
            var _params = $.extend({}, params, { batchSize: options.moreSize });

            $.extend(_params, { untilId: STLTweets.tweetMeta.lastVisibleId })

            // Execute the ajax call to get more tweets
            $.getJSON(url, _params,
        function(data) {
            options.max += data.count; // increase total display max
            var serverTime = new Date(data.resultTime);

            $.each(data.results,
            function(i, item) {
                // Display tweets at the end of the list
                renderAfter(item, serverTime, data.categoryId, { animate: false });
            }
          );

            $('#busy-more, #busy-play').remove();
            controls.moreBtn.showControlButton();
            controls.playBtn.showControlButton();

            // No results. Set the feed to refresh
            if (!data.count) {
                logAfter('No more tweets to show.');
                controls.moreBtn.hide();
            }

            updateRelativeDates(serverTime, data.categoryId);
        }
      );
        }

        /* render()
        renderBefore()
        renderAfter()
        --------------------------------------------------------------------------------*/
        function render(item, now, categoryId, renderOptions) {
            var renderDefaults = { animate: true, method: 'prepend' };
            renderOptions = $.extend({}, renderDefaults, renderOptions);

            var promoteLink = '';
            
            if (item.canPromote)
                promoteLink = '&nbsp;<a class="promoter" href="/Tweets/Promote/{%i}">promote</a>';

            var userName = escape(item.from_userName || item.from_user || '');
            var thumbClass = (item.hasThumb == "true") ? 'withThumb' : 'noThumb';

            var tweetHtml = options.template
              .replace(/\{%promote\}/g, promoteLink)
              .replace(/\{%tc\}/g, thumbClass)
              .replace(/\{%u\}/g, item.from_user)
              .replace(/\{%un\}/g, userName)
              .replace(/\{%ui\}/g, item.from_user_id)
              .replace(/\{%a\}/g, unescape(item.profile_image_url))
              .replace(/\{%p\}/g, '/People/' + categoryId + '/Detail/' + item.from_user)
              .replace(/\{%d\}/g, new Date(item.created_at).toRelativeDateString(now))
              .replace(/\{%dlink\}/g, '/Tweets/' + categoryId + '/Detail/' + item.id)
              .replace(/\{%i\}/g, item.id)
              .replace(/\{%t\}/g, item.text.toTweetString(categoryId))
              .replace(/\{%ll\}/g, escape(item.linkLink || '/'))
              .replace(/\{%ln\}/g, escape(item.linkTitle || 'Home'))
              .replace(/\{%lt\}/g, item.linkThumbnail || '/favicon.ico')
              .replace(/\{%tlt\}/g, item.linkType || 'Other')
              .replace(/\{%spfx\}/g, '');
              
            var fragment = $(tweetHtml).data('tweet', item).hide();

            holder[renderOptions.method](fragment);

            if (renderOptions.animate) {
                transitions.blindFadeIn.call(fragment, checkDisplayLength);
                transitions.blindFadeOut.call(holder.children(':last'), checkDisplayLength);
            }
            else {
                fragment.show();
                checkDisplayLength();
            }

            STLTweets.processRetweetLinks(fragment, options);
            STLTweets.processPromoteTweets(fragment, options);
            updateRelativeDates(now, categoryId);
        }

        function renderBefore(item, now, categoryId, renderOptions) {
            render(item, now, categoryId, $.extend(renderOptions, { method: 'prepend' }));
        }

        function renderAfter(item, now, categoryId, renderOptions) {
            render(item, now, categoryId, $.extend(renderOptions, { method: 'append' }));
        }

        /* updateRelativeDates()
        --------------------------------------------------------------------------------*/
        function updateRelativeDates(now, categoryId) {
            holder
        .children(options.tweetSelector)
        .each(
          function() {
              var item = $(this),
              data = item.data('tweet');

              if (data) {
                  item.find('a').find('em').html(
                  new Date(data.created_at).toRelativeDateString(now)
              );
              }
          }
        );
        }

        /* checkDisplayLength()
        --------------------------------------------------------------------------------*/
        function checkDisplayLength() {
            while (holder.children(options.tweetSelector).length > options.max) {
                // Remove the last item in the display
                holder.children(':last').remove();
            }

            // Recalculate the last tweet id
            var lastTweet = getLastTweet();
            if (lastTweet) {
                STLTweets.tweetMeta.lastVisibleId = lastTweet.id;
            }
        }

        /* $.fn.showControlButton()
        --------------------------------------------------------------------------------*/
        $.fn.showControlButton = function() {
            this.show().css({ display: 'inline-block' });
        }

        /* initializeDisplay()
        --------------------------------------------------------------------------------*/
        function initializeDisplay() {
            var stream = {
                play: function() {
                    paused = false;

                    clearPendingUpdates();
                    getTweets(true); // Pause button is shown after the data is loaded

                    controls.playBtn.hide().after(STLTweets.Ajax.getLoadingIndicator('busy-play'));

                    updatePausedStatus();
                },
                pause: function() {
                    paused = true;

                    controls.pauseBtn.hide();
                    controls.playBtn.showControlButton();

                    updatePausedStatus();
                },
                more: function() {
                    if (options.stream) {
                        stream.pause();
                    }

                    $('#busy-more, #busy-play').remove();
                    controls.moreBtn.hide().after(STLTweets.Ajax.getLoadingIndicator('busy-more'));
                    controls.playBtn.hide().after(STLTweets.Ajax.getLoadingIndicator('busy-play'));

                    getOlderTweets();
                }
            };

            // Set up 'more' button
            controls.moreBtn.click(function(e) { STLTweets.UI.cancelDefault(e); stream.more(); }).hide();

            if (options.stream) {
                // Set up 'play' button
                controls.playBtn.click(function(e) { STLTweets.UI.cancelDefault(e); stream.play(); }).hide();

                // Set up 'pause' button
                controls.pauseBtn.click(function(e) { STLTweets.UI.cancelDefault(e); stream.pause(); }).hide();

                // Set up status indicator
                controls.status.hide();

                // Pause the tweet stream if the user scrolls from the top of the page or the page
                // is reloaded at a scroll position other than the top of the window
                $(window).scroll(function(e) {
                    if (preloaded && $(this).scrollTop() > 0) {
                        stream.pause();

                        // This only works the first time after the page is loaded. After
                        // that the user must play/pause as they see fit.
                        $(this).unbind('scroll');
                    }
                });
            }
            else {
                controls.moreBtn.showControlButton();
            }
        }

        /* updatePausedStatus()
        --------------------------------------------------------------------------------*/
        function updatePausedStatus() {
            if (paused) {
                controls.status.html('PAUSED' + (STLTweets.tweetMeta.missedCount > 0 ? ' (' + STLTweets.tweetMeta.missedCount + ' new)' : '')).showControlButton();
            }
            else {
                controls.status.empty().hide();
            }
        }

        /* clearPendingUpdates()
        --------------------------------------------------------------------------------*/
        function clearPendingUpdates() {
            STLTweets.tweetMeta.missedCount = 0;
            while (STLTweets.tweetMeta.tweetIds[0]) {
                var cachedTweetId = STLTweets.tweetMeta.tweetIds.shift();
                if (cachedTweetId) {
                    clearTimeout(cachedTweetId.timeoutId);
                }
            }
            clearTimeout(STLTweets.tweetMeta.updateId);
        }

        /* Custom transitions
        --------------------------------------------------------------------------------*/
        var transitions = {
            blindFadeIn:
        function(callback) {
            var $this = $(this);
            setTimeout(function() {
                $this
              .css({ visibility: 'visible', opacity: 0 })
              .animate({ opacity: 1 }, { queue: false, duration: 500 });
            }, 300);
            $this
            .css({ visibility: 'hidden' })
            .show('blind', { callback: (callback || function() { }) }, 1000);
        },
            blindFadeOut:
        function(callback) {
            var $this = $(this);
            $this
            .hide('blind', { callback: (callback || function() { }) }, 1000);
        }
        }

        /* getFirstTweet()
        --------------------------------------------------------------------------------*/
        function getFirstTweet() {
            return holder.children(options.tweetSelector + ':first').data('tweet');
        };

        /* getLastTweet()
        --------------------------------------------------------------------------------*/
        function getLastTweet() {
            return holder.children(options.tweetSelector + ':last').data('tweet');
        };

        /* log()
        --------------------------------------------------------------------------------*/
        function log(message, useAlt, method) {
            holder[method || 'prepend']('<div class="tweetLogWrap' + (useAlt ? ' alt' : '') + '"><div class="tweetLog group">' + message + '</div></div>');
        }

        function logBefore(message, useAlt) {
            log(message, useAlt, 'prepend');
        }

        function logAfter(message, useAlt) {
            log(message, useAlt, 'append');
        }

        //------------------------------------------------------------------------------
        // MAIN EXECUTION BLOCK
        //------------------------------------------------------------------------------

        //holder.prepend('<div id="busy-loading"><img alt="" src="/Assets/images/indicator_medium.gif"/></div>'); // already have server tweets

        initializeDisplay();

        if (options.stream) {
            getTweets();
        }
        else {
            STLTweets.processRetweetLinks($('#tweets'));
            STLTweets.processPromoteTweets($('#tweets'));
        }
    };

})(jQuery);