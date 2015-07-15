/* processRetweetLinks()
--------------------------------------------------------------------------------*/
function processRetweetLinks(context) {
    (context || holder).find('a.retweet').click(
        function(e) {
            e.preventDefault();

            var categoryId = 'Everything';

            var suggestTweepUrl = '/People/{0}/SuggestSomeone'.replace('{0}', categoryId);

            STLTweets.retweetCurrent = function() {
                STLTweets.retweet(retweetUrl);
            }

            // TODO: Cookie is not being expired, so once the cookie is set, we'll never get the "connect to twitter" popup.
            // STLTweets.getUser returns the data from the login cookie.

            if (!STLTweets.getUser()) {
                STLTweets.connectToTwitter('retweetCurrent');
            }
            else {
                STLTweets.retweetCurrent();
            }
        }
      );

        /* **************************************************************************************************** */
        /* **************************************************************************************************** */
        /* **************************************************************************************************** */


        //------------------------------------------------------------------------------
        // MAIN EXECUTION BLOCK
        //------------------------------------------------------------------------------
        
        
}