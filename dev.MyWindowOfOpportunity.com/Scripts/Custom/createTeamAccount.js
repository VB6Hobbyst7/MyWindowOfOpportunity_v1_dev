$(function () {
    try {
        function jsCreateTeamAcct() {
            //Instantiate variables
            var $cbCreateNew = $("#fstTeamAccts input[type=radio]");
            var txbNewTeamName = $("#fstTeamAccts input[type=text]");
            //var lbtnNewTeamName = $("#fstTeamAccts .button");

            //Perform the initial team radiobutton management.
            manageTeamField();

            //If radiobutton is clicked.
            $cbCreateNew.click(function () {
                //uncheck all cb's except for clicked cb.
                $.each($cbCreateNew, function (i) {
                    //
                    $(this).prop('checked', false);
                });
                $(this).prop('checked', true);

                //manage textbox enable/disable
                manageTeamField();
            });

            //
            function manageTeamField() {
                //Determine if 'create new' cb is checked or not.  disable txb if unchecked.
                if ($cbCreateNew.eq(0).is(":checked")) {
                    //Enable text area
                    txbNewTeamName.prop("disabled", false);
                    //lbtnNewTeamName.unbind('click', disableLink);
                    //lbtnNewTeamName.removeClass("disabled");
                }
                else {
                    //Disable text area
                    txbNewTeamName.prop("disabled", true);
                    //lbtnNewTeamName.bind('click', disableLink);
                    //lbtnNewTeamName.addClass("disabled");
                }
            };

            //function disableLink(e) {
            //    // cancels the event
            //    e.preventDefault();

            //    return false;
            //}
        }


        //Run only if element exists
        if ($('#createACampaign').length > 0) { jsCreateTeamAcct(); }
    }
    catch (err) {
        console.log('ERROR: [jsCreateTeamAcct] ' + err.message + ' | ' + err);
    }
});