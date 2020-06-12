$(function () {
    try {
        function jsStandardPg() {
            //==============================
            //=========Set dob calendar
            //==============================
            //Calculate youngest permitted date of birth (18 years old)
            var eighteenYearsAgo = new Date();
            eighteenYearsAgo.setDate(eighteenYearsAgo.getDate() - 6575);

            //Build dob calendar
            $(".flatpickr").flatpickr({
                enableTime: false,
                altInput: true,
                altFormat: "F j, Y",
                minDate: "1917-10-13",
                maxDate: eighteenYearsAgo
            });

            //Set calendar's attributes.
            $('input.txbDateOfBirth.flatpickr-input.form-control').attr('required', 'true').removeAttr('readonly');

            $(function () {
                //Instantiate variables
                var $form = $('form');
                var submitValid = false;
                var btnContinue = $('#btnContinue');
                var btnCreateAcct = $('#btnCreateAcct');
                var txbFirstName = $('.txbFirstName input');
                var txbLastName = $('.txbLastName input');
                var txbDateOfBirth = $('.txbDateOfBirth');
                var txbEmail = $('.txbEmail input');
                var txbReEnterEmail = $('.txbReEnterEmail input');
                var txbPassword = $('.txbPassword input');
                var txbPasswordReenter = $('.txbPasswordReenter input');
                var $pwValidCheck = $('.passwordValidations img.valid');
                var hfldErrorMsg = $('#hfldErrorMsg');
                var hfld2ndErrorMsg = $('#hfld2ndErrorMsg');

                var alertMsg_Success = $('.alertBoxRow.warning');
                var alertMsg_Success_InitialMsg = $('.alertBoxRow.warning .initialText');
                var alertMsg_Success_AdditionalMsg = $('.alertBoxRow.warning .additionalText');

                var alertMsg_Info = $('.alertBoxRow.info');
                var alertMsg_Info_InitialMsg = $('.alertBoxRow.info .initialText');
                var alertMsg_Info_AdditionalMsg = $('.alertBoxRow.info .additionalText');

                var alertMsg_Error = $('.alertBoxRow.alert');
                var alertMsg_Error_InitialMsg = $('.alertBoxRow.alert .initialText');
                var alertMsg_Error_AdditionalMsg = $('.alertBoxRow.alert .additionalText');

                var alertMsg_Warning = $('.alertBoxRow.warning');
                var alertMsg_Warning_InitialMsg = $('.alertBoxRow.warning .initialText');
                var alertMsg_Warning_AdditionalMsg = $('.alertBoxRow.warning .additionalText');


                //****************************************
                // MANAGE ALERT MESSAGES
                //****************************************
                function showSuccessMsg(visible, msg, xtraMsg) {
                    if (visible.toString() == 'true') {
                        alertMsg_Success.removeClass('hide');
                        alertMsg_Success_InitialMsg.text(msg);
                        alertMsg_Success_AdditionalMsg.text(xtraMsg);
                    }
                    else { alertMsg_Success.addClass('hide'); }
                }
                function showInfoMsg(visible, msg, xtraMsg) {
                    if (visible.toString() == 'true') {
                        alertMsg_Info.removeClass('hide');
                        alertMsg_Info_InitialMsg.text(msg);
                        alertMsg_Info_AdditionalMsg.text(xtraMsg);
                    }
                    else { alertMsg_Info.addClass('hide'); }
                }
                function showErrorMsg(visible, msg, xtraMsg) {
                    if (visible.toString() == 'true') {
                        alertMsg_Error.removeClass('hide');
                        alertMsg_Error_InitialMsg.text(msg);
                        alertMsg_Error_AdditionalMsg.text(xtraMsg);
                    }
                    else { alertMsg_Error.addClass('hide'); }
                }
                function showWarningMsg(visible, msg, xtraMsg) {
                    if (visible.toString() == 'true') {
                        alertMsg_Warning.removeClass('hide');
                        alertMsg_Warning_InitialMsg.text(msg);
                        alertMsg_Warning_AdditionalMsg.text(xtraMsg);
                    }
                    else { alertMsg_Warning.addClass('hide'); }
                }
                function hideAllMsgs() {
                    //showSuccessMsg(false, '', '');
                    //showInfoMsg(false, '', '');
                    //showErrorMsg(false, '', '');
                    showWarningMsg(false, '', '');
                }

                //Ensure all messages are hidden on load.
                hideAllMsgs();

                //If error message exists, display it.
                if (hfldErrorMsg.val()) { showErrorMsg(true, hfldErrorMsg.val(), hfld2ndErrorMsg.val()); };


                //****************************************
                // IS PASSWORDS VALID
                //****************************************
                function arePasswordsValid() {
                    try {
                        //Instantiate variables
                        var isValid = true;

                        //Ensure that all validations are valid.
                        $pwValidCheck.each(function () {
                            if ($(this).is(":hidden")) { isValid = false }
                        });

                        //
                        if (!isValid) { showWarningMsg(true, 'Invalid Password'); };

                        //Return results
                        return isValid;
                    }
                    catch (err) {

                        if (err.message != null) {
                            //Show the alert box.
                            showErrorMsg('Error: ' + err.message + ' | ' + err);

                            return false;
                        }
                    }
                }

                //****************************************
                // ARE EMAILS VALID
                //****************************************
                function areEmailsValid() {
                    try {
                        //Validate emails
                        if (txbEmail.val().toUpperCase() !== txbReEnterEmail.val().toUpperCase()) {
                            showWarningMsg(true, 'Both emails must match');
                            return false;
                        }

                        //
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

                //****************************************
                // CONTINUE BUTTON CLICKED
                //****************************************
                btnContinue.click(function () {
                    try {
                        //Hide all messages
                        hideAllMsgs();

                        if ($form[0].checkValidity()) {
                            //Validate emails
                            submitValid = areEmailsValid();
                            if (!submitValid) { return }

                            //Validate Passwords
                            submitValid = arePasswordsValid();
                            if (!submitValid) { return }

                            //If page is valid, submit.
                            if (submitValid == true) {
                                //Show spinner
                                TweenMax.set(spinner, { autoAlpha: 1 });

                                //Submit
                                btnCreateAcct.click();
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


                //****************************************
                // INSURE FORM ONLY SUBMITS WHEN VALID
                //****************************************
                $form.submit(function () {
                    if (submitValid == true) {
                        return true
                    }
                    else {
                        return false;
                    }
                });
            });

            function fPass() {
                console.log('fpass Started');
                var btnResetSubmit = $('#cphMainContent_ctl00_ResetPassword_1_btnPassWord');
                btnResetSubmit.trigger("click");
                console.log('fpass Complete');
            }
            function fReset() {
                console.log('fReset started');
                var btnSubmitResetRequest = $('#cphMainContent_ctl00_ResetPassword_1_btnReset');
                btnSubmitResetRequest.trigger("click");
                console.log('fReset complete');
            }
        }


        //Run only if element exists
        if ($('#standardPg').length > 0) { jsStandardPg(); }
    }
    catch (err) {
        console.log('ERROR: [jsStandardPg] ' + err.message + ' | ' + err);
    }
});