//Manage rewards
//=================


//$(window).load(function () {
$(function () {
    try {
        function jsManageTeamMembers() {
            //Instantiate variables
            var $teamMemberBtns = $("ul.teamMemberFilter:not(.roles) input[type=radio]");
            var $teamMemberPnls = $("div[type=teamMemberEntry]");
            var lbtnRemoveMember = $('.lbtnRemoveMember');
            var txbRoleDescription;
            var txbRoleDescriptionInput;
            var rblRole;


            //
            lbtnRemoveMember.click(function (e) {
                e.preventDefault();

                //Zebra Dialgo | http://stefangabos.ro/jquery/zebra-dialog/
                $.Zebra_Dialog('Are you sure you wish to remove this member?  <strong>Confirm Deletion.</strong>', {
                    'type': 'warning',
                    'title': 'Are You Sure?',
                    'buttons': [
                                    {
                                        caption: 'Yes', callback: function () {
                                            TweenMax.set(spinner, { autoAlpha: 1 });
                                            $('.btnRemoveMember').trigger('click');
                                        }
                                    },
                                    { caption: 'No' }
                    ]
                });
            });


            //When category button is clicked, show subcategories
            $teamMemberBtns.click(function () {
                //Hide all subcategory panels
                hideAll();

                //Show matching subcategory panel
                var activePnl = $teamMemberPnls.filter("[nodeId='" + $(this).val() + "']");

                activePnl.show('fade', {
                    duration: 500,
                    easing: 'easeInOutQuart'
                });
                
                //If button's value = -1, set hidden field's value as 'true' for new entry
                if ($(this).val() === '-1') {
                    //Instantiate variables
                    var email;
                    var txbEmail = activePnl.find('.txbNewEmailAddress input[type="text"]');
                    var btnValidationCheck = activePnl.find('.button.validationCheck');
                    var $alerts = activePnl.find('.alertMsgs .alertBoxRow');
                    var alertAvailable = activePnl.find('.alertMsgs .alertBoxRow.success');
                    var alertExists = activePnl.find('.alertMsgs .alertBoxRow.warning');
                    var alertInvalid = activePnl.find('.alertMsgs .alertBoxRow.alert');
                    var pnlNewUser = activePnl.find('.emailValidated_NewUser');

                    var btnDecline = activePnl.find('.button.decline');
                    var hfldIsNewMember = activePnl.find('.hfldIsNewMember');
                    rblRole = activePnl.find('.rblRole_NewMember input');

                    txbRoleDescription = activePnl.find('.txbRoleDescription_NewMember');
                    txbRoleDescriptionInput = activePnl.find('.txbRoleDescription_NewMember input');

                    //Set default role value if an administrator role is selected
                    if (activePnl.find('.rblRole input:checked').val() == 'Team Administrator') {
                        txbRoleDescriptionInput.prop("disabled", true);
                        txbRoleDescriptionInput.val('Team Administrator');
                        txbRoleDescription.addClass('input--filled');
                    }

                    //Clear email field
                    txbEmail.val('');

                    //Assign value to new team member hidden field.
                    var hfldNewTeamMemberEntry = activePnl.find('.hfldNewTeamMemberEntry');
                    hfldNewTeamMemberEntry.val(true);

                    //If decline button is clicked:
                    btnDecline.click(function () {
                        //Clear and hide fields and controls.
                        $alerts.hide();
                        //emailBtnOptions.hide();
                        pnlNewUser.hide();
                        txbEmail.val('');
                        email = '';
                    });
                                        
                    //When validation button is clicked, validate email and show accordingly.
                    btnValidationCheck.click(function () {
                        //Show Spinner
                        TweenMax.set(spinner, { autoAlpha: 1 });
                        //Call function
                        TweenMax.delayedCall(0, validateEmail);

                        function validateEmail() {
                            //Instantiate variables
                            email = txbEmail.val().trim();

                            //Hide alerts and buttons
                            $alerts.hide();
                            //emailBtnOptions.hide();
                            pnlNewUser.hide();

                            //If email field is editted, hide controls till a new validation occurs.
                            txbEmail.on('input', function () {
                                $alerts.hide();
                                //emailBtnOptions.hide();
                                pnlNewUser.hide();
                            });

                            //Validate email
                            //function isEmail(email) {
                            //    var regex = '/^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/';
                            //    return regex.test(email);
                            //}
                            //if (!isEmail(email)) {

                            //Determine if email is valid
                            if (is.not.email(email)) {
                                //Invalid. Show alert msg.
                                alertInvalid.show();

                                //Hide spinner
                                TweenMax.set(spinner, { autoAlpha: 0 });
                            }
                            else {
                                //Valid email address. Instantiate variables
                                var urlPath = window.location.protocol + '//' + window.location.host;
                                var ashxUrl = urlPath + '/Services/MemberExist.ashx?email=' + email
                                var data = "Key:Value";

                                //Call service to determine how to use submitted email address.
                                $.ajax({
                                    url: ashxUrl,
                                    type: 'GET',
                                    data: data,
                                    dataType: 'json',
                                    success: function (data) {
                                        //
                                        if (data == true) {
                                            //Email exists in db.  Show proper elements
                                            alertExists.show();
                                            //emailBtnOptions.show();
                                            pnlNewUser.show();
                                            hfldIsNewMember.val(false);
                                        }
                                        else {
                                            //Email is unique.  Show proper elements
                                            alertAvailable.show();
                                            //emailBtnOptions.show();
                                            pnlNewUser.show();
                                            hfldIsNewMember.val(true);
                                        }

                                        //Hide spinner
                                        TweenMax.set(spinner, { autoAlpha: 0 });
                                    },
                                    error: function (request, error) {
                                        //Error occured.  Show error in console.
                                        console.log("Request error: " + request + ' - ' + error);
                                        alertInvalid.show();

                                        //Hide spinner
                                        TweenMax.set(spinner, { autoAlpha: 0 });
                                    }
                                });
                            }
                        }
                    });
                    
                }
                else {
                    //Obtain child elements
                    rblRole = activePnl.find('.rblRole input');
                    txbRoleDescription = activePnl.find('.txbRoleDescription');
                    txbRoleDescriptionInput = activePnl.find('.txbRoleDescription input');

                    //Set default role value if an administrator role is selected
                    if (activePnl.find('.rblRole input:checked').val() == 'Team Administrator') {
                        txbRoleDescriptionInput.prop("disabled", true);
                        txbRoleDescriptionInput.val('Team Administrator');
                        txbRoleDescription.addClass('input--filled');
                    }
                };

                //Update role value when role is changed.
                rblRole.change(function () {
                    if ($(this).val() == 'Team Administrator') {
                        txbRoleDescriptionInput.prop("disabled", true);
                        txbRoleDescriptionInput.val('Team Administrator');
                        txbRoleDescription.addClass('input--filled');
                    }
                    else {
                        if (txbRoleDescriptionInput.val() == 'Team Administrator') {
                            txbRoleDescriptionInput.prop("disabled", false);
                            txbRoleDescriptionInput.val('');
                            txbRoleDescription.removeClass('input--filled');
                            console.log('cleared');
                        }
                    }
                });

            });


            //Hide all subcategory panels
            function hideAll() {
                $teamMemberPnls.each(function () {
                    $(this).hide();
                });
            };
        }


        //Run only if element exists
        if ($('.lbtnRemoveMember').length > 0) {
            jsManageTeamMembers();
        }
        else if ($('ul.teamMemberFilter').length > 0) { 
            jsManageTeamMembers();
        }
    }
    catch (err) {
        console.log('ERROR: [jsManageTeamMembers] ' + err.message + ' | ' + err);
    }
});
