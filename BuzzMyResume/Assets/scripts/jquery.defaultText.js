//----------------------------------------------------------------------------------
// Plugin for jQuery 1.2.x to allow setting default text for text form fields when
// not focused on the fields and no custom text has been entered
//
// Title text acts as default text for text form fields
// - To enable this feature for a text field, add the class "default-text"
//
// Neil Monroe (neil.monroe@gmail.com)
//
//
// Change Log:
//
// 1.1 / 08.26.2009
// - New validation method for the validation jQuery plugin (if present)
// - Helper function hasDefaultText() added to enable check anytime
//
// 1.0 / 02.06.2009
// - Initial Release
//----------------------------------------------------------------------------------

(function($) {

	$.extend({
		defaultText: {
			defaults: {
				selector: ".default-text",
				blurClass: "blur"
			},
			init: function() {
				$(this.defaults.selector)
					.focus(this.focusText)
					.blur(this.blurText)
					.trigger("focus")
					.trigger("blur"); // reset text boxes initially
			},
			focusText: function(e) {
				$(this).removeClass($.defaultText.defaults.blurClass);
				if ($(this).hasDefaultText()) this.value = "";
			},
			blurText: function(e) {
				if ($(this).hasDefaultText()) {
					$(this).addClass($.defaultText.defaults.blurClass);
					this.value = this.title;
				}
			}
		}
	});
	
	// Check if the current element matches default text (case-insensitive)
	$.fn.extend({
		hasDefaultText: function() {
			return this.is(":text, textarea") && (!this.val().match(/\S/g) || this.val().toLowerCase() == this.attr("title").toLowerCase());
		}
	});
	
	// If using the validation plugin, add an additional method to take the
	// default text into account when checking required state.
	// http://bassistance.de/jquery-plugins/jquery-plugin-validation/
	$(function() {
		if ($.validator) {
			$.validator.addMethod("default-text", function(value, element) {
				return this.optional(element) || !$(element).hasDefaultText();
			}, $.validator.messages.required);
		}
	});
	
	// Automatically initialize this plugin when the DOM is ready
	$(function(){
		$.defaultText.init();
	});

})(jQuery);
