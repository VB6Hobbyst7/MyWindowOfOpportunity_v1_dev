$(function () {
    try {
        function jsContactUs() {

            var alertMsg_Warning = $('.alertBoxRow.warning');
            var alertMsg_Warning_InitialMsg = $('.alertBoxRow.warning .initialText');
            var alertMsg_Warning_AdditionalMsg = $('.alertBoxRow.warning .additionalText');
            var txbFullName = $('.txbFullName input');
            var txbEmail = $('.txbEmail input');
            var txbPhoneNumber = $('.txbPhoneNumber input');
            var txtCaptcha = $('.txtCaptcha input');
            var txbSubject = $('.txbSubject input');
            var txbMessage = $(".txbMessage textarea");

            var btnContactUsPageSubmit = $('#btnContactUsPageSubmit');
            var btnSubmitMessage = $('#btnSubmitMessage');
            //****************************************
            // CONTINUE BUTTON CLICKED Contact Page
            //****************************************

            btnContactUsPageSubmit.click(function () {
                try {
                    if (txbFullName.val() == '' || txbEmail.val() == '' || txbPhoneNumber.val() == '' || txtCaptcha.val() == '' || txbSubject.val() == '' || txbMessage.val() == '') {
                        return;
                    }
                    else {
                        submitValid = areEmailsValid();
                        if (!submitValid) {
                            return;
                        }
                        //If page is valid, submit.
                        if (submitValid == true) {
                            //validateCaptcha();
                            TweenMax.set(spinner, { autoAlpha: 1 });
                            btnSubmitMessage.click();
                            return false;
                        }
                    }
                }
                catch (err) {
                    if (err.message != null) {
                        //Show the alert box.
                        showErrorMsg('Error: ' + err.message + ' | ' + err);
                    }
                }
            });

            function areEmailsValid() {
                try {
                    if (isEmailAddressValid(txbEmail.val())) {
                        //Valid email addresses
                        return true;
                    }
                    else {
                        //Invalid email addresses
                        showWarningMsg(true, 'Please provide a valid email address');
                        return false;
                    }
                }
                catch (err) {
                    if (err.message != null) {
                        //Show the alert box.
                        showErrorMsg('Error: ' + err.message + ' | ' + err);
                        return false;
                    }
                }
            }

            function showWarningMsg(visible, msg, xtraMsg) {
                if (visible.toString() == 'true') {
                    alertMsg_Warning.removeClass('hide');
                    alertMsg_Warning_InitialMsg.text(msg);
                    alertMsg_Warning_AdditionalMsg.text(xtraMsg);
                }
                else { alertMsg_Warning.addClass('hide'); }
            }

            $("#imgcaptcha").click(function () {
                $("input[type='text'].input__field").each(function () {
                    $(this).prop("required", false);
                })
                $("textarea.input__field").each(function () {
                    $(this).prop("required", false);
                })
                $("#cphMainContent_ImageButton1").click();
            })


            //function validateCaptcha() {
            //    var captcha_response = grecaptcha.getResponse();
            //    console.log(captcha_response)
            //    if (captcha_response.length == 0) {
            //        // Captcha is failed
            //        $("div.recaptcha-error-message").css("display", "block");
            //        return false;
            //    }
            //    else {
            //        // Captcha is Passed
            //        $("div.recaptcha-error-message").css("display", "none");
            //        TweenMax.set(spinner, { autoAlpha: 1 });
            //        btnSubmitMessage.click();
            //    }
            //}
        }


        //Run only if element exists
        if ($('#contactUsPg').length > 0) { jsContactUs(); }
    }
    catch (err) {
        console.log('ERROR: [jsContactUs] ' + err.message + ' | ' + err);
    }
});