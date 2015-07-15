// jQuery plugin to do a simple window.open()
(function($)
{
  $.popup = function(options, context)
  {
    var defaults = { url: (context ? $(context).attr('href') : null), width: null, height: null, name: '', immediate: false };
    options = $.extend({}, defaults, options);
    
    function showWindow()
    {
      var win = $(window), w = win.width(), h = win.height(),
        left = Math.max((w - (options.width || w)) / 2, 0), top = Math.max((h - (options.height || h)) / 2, 0);
      
      window.open(options.url, options.name,
        'toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=no,resizable=no' +
        ',left=' + left + ',top=' + top + ',screenX=' + left + ',screenY=' + top +
        (options.width ? (',width=' + options.width) : '') + (options.height ? (',height=' + options.height) : ''));
    }

    if ($(context).is('a') && !options.immediate)
    {
      $(context).click(
        function(e)
        {
          e.preventDefault();
          
          showWindow();
        }
      );
    }
    else
    {
      showWindow();
    }
  }
  
  $.fn.popup = function(options)
  {
    $.popup(options, $(this));
    return $(this);
  };
})(jQuery);