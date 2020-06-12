$(function () {
    try {
        function jsManageTeam() {
            //Instantiate variables
            var $accordion = $('.accordion');
            var firstAccordion = $accordion.first();
            var firstEntry = firstAccordion.find('li').first();
            var href = firstEntry.children('a').first();
            var content = firstEntry.find('.content');

            //Set first panel as active.
            content.slideToggle();
            firstEntry.addClass('active');
            href.attr('aria-expanded', 'true');
            content.addClass('active');



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


            $("#btnContentSave").click(function () {
                SaveContentdata();
            });
            $("#btnSummary").click(function () {
                if (window.location.href.indexOf("editMode") > -1) {
                    var url = window.location.href.split('?')[0];
                    window.location.href = url;
                }
                else {
                    window.location.href = window.location.href;
                }
            });


            function removeURLParam(url, param) {
                var urlparts = url.split('?');
                if (urlparts.length >= 2) {
                    var prefix = encodeURIComponent(param) + '=';
                    var pars = urlparts[1].split(/[&;]/g);
                    for (var i = pars.length; i-- > 0;)
                        if (pars[i].indexOf(prefix, 0) == 0)
                            pars.splice(i, 1);
                    if (pars.length > 0)
                        return urlparts[0] + '?' + pars.join('&');
                    else
                        return urlparts[0];
                }
                else
                    return url;
            }
            function SaveContentdata() {
                var value = CKEDITOR.instances['TemplateContent'].getData()
                $("#cphMainContent_hdfdTemplateContent").attr("value", value);
                lbtnSaveContent.click();
            }


            if ($('#TemplateContent').length > 0) {
                //
                $('textarea#TemplateContent').ckeditor();
                CKEDITOR.instances['TemplateContent'].setData($(".hdfdTemplateContent").val());           
                CKEDITOR.plugins.registered['save'] = {
                    init: function (editor) {
                        var command = editor.addCommand('save',
                        {
                            modes: { wysiwyg: 1, source: 1 },
                            exec: function (editor) { // Add here custom function for the save button
                                SaveContentdata();
                            }
                        });
                        editor.ui.addButton('Save', { label: 'Save', command: 'save' });
                    }
                }
            }

        }

        //Run only if element exists
        if ($('#teamPg').length > 0) { jsManageTeam(); }
    }
    catch (err) {
        console.log('ERROR: [jsManageTeam] ' + err.message + ' | ' + err);
    }
});