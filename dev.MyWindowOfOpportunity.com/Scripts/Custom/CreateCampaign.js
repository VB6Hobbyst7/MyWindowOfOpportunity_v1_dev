$(function (e) {
    try {
        function jscreateACampaign() {
            //Instantiate variables
            let alertMsg_Error = $('.alertBoxRow.alert');
            let alertMsg_Error_InitialMsg = $('.alertBoxRow.alert .initialText');
            let alertMsg_Error_AdditionalMsg = $('.alertBoxRow.alert .additionalText');
            let createCampaign = $('#btnCreateCampaign');
            //let countLi = $("#lstviewTeamAccts li");
            let CampaignName = document.getElementById('txb');
            let txbTeamName = document.getElementById('txbNewTeamName');
            let lbnCreateCampaign = document.getElementById("lbtnCreateCampaign");
            let hfldAltEmail = $('#hfldAltEmail');
            let txbAltEmail = $('.txbAltEmail input[type=email]');
            let hfldShowAlert = $('.hfldShowAlert').eq(0);


            function showErrorMsg(visible, msg, xtraMsg) {
                if (visible.toString() === 'true') {
                    alertMsg_Error.removeClass('hide');
                    //alertMsg_Error_InitialMsg.text(msg);
                  if (is.not.empty(xtraMsg)) {alertMsg_Error_InitialMsg.text(msg.trim());}
                  if (is.not.empty(xtraMsg)) {alertMsg_Error_AdditionalMsg.text(xtraMsg.trim());}
                }
                else { alertMsg_Error.addClass('hide'); }
            }
            createCampaign.on('click', function (e) {
                if ((CampaignName.value === '' || CampaignName.value === undefined) && (txbTeamName.value === '' && txbTeamName.disabled === false)) {
                    showErrorMsg('true', 'Campaign and Team name required to create a new campaign.', '');
                }
                else if (CampaignName.value === '' || CampaignName.value === undefined) {
                    showErrorMsg('true', 'Campaign name required.', '');
                }
                else if (txbTeamName.value === '' && txbTeamName.disabled === false) {
                    showErrorMsg('true', 'Please add a team name', '');
                }
                else if (hfldAltEmail.val() == 'true' && is.email(txbAltEmail.val()) === false) {
                    showErrorMsg('true', 'Please add an alternate email address', '');
                }
                else {
                    showErrorMsg('false');
                    lbnCreateCampaign.click();
                }
            });

            //
            if (is.truthy(hfldShowAlert.val())) {
                showErrorMsg('true');
            }
            else {
                showErrorMsg('false');
            }
        }


        //Run only if element exists
        if ($('#createACampaign').length > 0) { jscreateACampaign(); }
    }
    catch (err) {
        console.log('ERROR: [createACampaign] ' + err.message + ' | ' + err);
    }
});