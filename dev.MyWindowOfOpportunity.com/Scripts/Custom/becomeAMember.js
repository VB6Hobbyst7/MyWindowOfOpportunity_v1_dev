$(function () {
    try {
        function jsBecomingAMember() {
            //Instantiate variables
            var pnlBecomingAMember = $('.pnlBecomingAMember');
            var cbAgreeToTerms = $('#cbAgreeToTerms');
            var $links = pnlBecomingAMember.find('a');

            //Disable controls
            function disableControls() {
                $links.attr('onclick', 'return false')
                pnlBecomingAMember.addClass('disabled');
                pnlBecomingAMember.attr('disabled', 'disabled')
            }
            //Enable controls
            function enableControls() {
                $links.removeAttr('onclick')
                pnlBecomingAMember.removeClass('disabled');
                pnlBecomingAMember.removeAttr('disabled')
            }

            //Set default status
            disableControls();

            //On agreement to terms...
            cbAgreeToTerms.click(function () {
                if ($(this).prop('checked')) {
                    enableControls();
                } else {
                    disableControls();
                }
            })
        }


        //Run only if element exists
        if ($('.pnlBecomingAMember').length > 0) { jsBecomingAMember(); }
    }
    catch (err) {
        console.log('ERROR: [jsBecomingAMember] ' + err.message + ' | ' + err);
    }
});