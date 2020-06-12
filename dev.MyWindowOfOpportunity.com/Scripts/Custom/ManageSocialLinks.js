$(function () {
    try {
        function jsManageSocialLinks() {
            //Instantiate variables
            var socialMediaManager = $('.socialMediaManager');
            var $socialIcons = socialMediaManager.find('.socialIcons .socialIcon');
            var $socialMediaEntry = socialMediaManager.find('.socialMediaEntries .socialMediaEntry');
            var $cancelBtns = $socialMediaEntry.find('a.button.cancel');
            var mediaType;
            var $txb = $socialMediaEntry.find('input[type="text"]');



            //
            $txb.each(function () {
                //Get social media type
                mediaType = $(this).parents().eq(2).attr('class');

                //Get button that matches media type
                var btn = socialMediaManager.find('.socialIcons .socialIcon[mediaType=' + mediaType + ']');

                if (jQuery.trim($(this).val()).length == 0) {
                    btn.addClass("inactive");
                }
                else {
                    btn.removeClass("inactive");
                };
            });


            //When button is clicked, show related panel
            $socialIcons.click(function () {
                //Hide all social media entry panels
                hideAll();

                //Obtain social media type
                mediaType = $(this).attr('mediaType');
                console.log(mediaType);

                //Locate and show matching fields
                $socialMediaEntry.each(function () {
                    if ($(this).children().hasClass(mediaType)) {
                        $(this).toggle();
                    };
                });
            });


            //When cancel button is clicked, hide all panels
            $cancelBtns.click(function () {
                //Hide all social media entry panels
                hideAll();
            });


            //Hide all social media entry panels
            function hideAll() {
                $socialMediaEntry.each(function () {
                    $(this).hide();
                });
            };
        }


        //Run only if element exists
        if ($('.socialMediaManager').length > 0) { jsManageSocialLinks(); }
    }
    catch (err) {
        console.log('ERROR: [jsManageSocialLinks] ' + err.message + ' | ' + err);
    }
});