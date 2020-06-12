$(function () {
    try {
        function jsCCValidation() {
            //Instantiate variables
            var form = $('form');
            var spinner = $('.spinner');
            var nameOnCard;
            var cardNo;
            var expMonth;
            var expYear;
            var cvc;
            var postalCode;
            var pnlSubmit = $('#pnlSubmit');
            var pnlUpdate = $('#pnlUpdate');

            var btnSubmit = $('#btnSubmit');
            var btnUpdate = $('#btnUpdate');
            var btnDelete = $('#btnDelete');
            var ltrPhaseNode = $("#ltrPhaseNode");

            var publicKey = $('#hfldPublicKey').val();
            var stripeUserId = $('#hfldStripeUserId').val();
            var campaignId = $('#hfldCampaignId').val();
            var hfldPublicKey = $('#hfldPublicKey');
            var hfldCurrentUserId = $('#hfldCurrentUserId');
            var hfldCustomerId = $('#hfldCustomerId');
            var hfldCardId = $('#hfldCardId');
            var hfldCardToken = $('#hfldCardToken');
            var hfldName = $('#hfldName');
            var hfldBrand = $('#hfldBrand');
            var hfldPostalCode = $('#hfldPostalCode');
            var hfldLast4 = $('#hfldLast4');
            var hfldExpirationMonth = $('#hfldExpirationMonth');
            var hfldExpirationYear = $('#hfldExpirationYear');
            var hfldShowAlertMsg_Success = $('#hfldShowAlertMsg_Success');
            var hfldShowAlertMsg_Info = $('#hfldShowAlertMsg_Info');
            var hfldShowAlertMsg_Error = $('#hfldShowAlertMsg_Error');
            var hfldPledgeamt = $("#hfldPledgeamt");

            var alertMsg_Success = $('.alertBoxRow.success');
            var alertMsg_Info = $('.alertBoxRow.info');
            var alertMsg_Error = $('.alertBoxRow.alert');

            var alertMsg_Success_InitialMsg = $('.alertBoxRow.success .initialText');
            var alertMsg_Info_InitialMsg = $('.alertBoxRow.info .initialText');
            var alertMsg_Error_InitialMsg = $('.alertBoxRow.alert .initialText');

            var alertMsg_Success_AdditionalMsg = $('.alertBoxRow.success .additionalText');
            var alertMsg_Info_AdditionalMsg = $('.alertBoxRow.info .additionalText');
            var alertMsg_Error_AdditionalMsg = $('.alertBoxRow.alert .additionalText');

            var txtBillingAddress1 = $('.txtBillingAddress1 input');
            var txtBillingAddress2 = $('.txtBillingAddress2 input');
            var txtBillingCity = $('.txtBillingCity input');
            var txtBillingState = $('.txtBillingState input');
            var txtBillingZip = $('.txtBillingZip input');

            var txtShippingAddress1 = $('.txtShippingAddress1 input');
            var txtShippingAddress2 = $('.txtShippingAddress2 input');
            var txtShippingCity = $('.txtShippingCity input');
            var txtShippingState = $('.txtShippingState input');
            var txtShippingZip = $('.txtShippingZip input');

            var txbAltEmail = $('.txbAltEmail input[type=email]');
            var hfldUseAltEmail = $('.hfldUseAltEmail');
            var txbNameOnCard = $('#txbNameOnCard');
            var txbCreditCard = $('#txbCreditCard');
            var txbCCExpirationMonth = $('#txbCCExpirationMonth');
            var txbCCExpirationYear = $('#txbCCExpirationYear');
            var txbCvcCode = $('#txbCvcCode');
            var txbPostalCode = $('#txbPostalCode');

            var spanTxbCvcCode = $('.txbCvcCode');
            var spanTxbCCExpirationYear = $('.txbCCExpirationYear');
            var spanTxbCCExpirationMonth = $('.txbCCExpirationMonth');
            var spanTxbNameOnCard = $('.txbNameOnCard');
            var spanTxbPostalCode = $('.txbPostalCode');
            var spanTxbCreditCard = $('span.txbCreditCard');
            var spanTxbCreditCardSlash = $('span.txbCreditCard span.slash-show-hide');
            var $spanTxbCreditCardInputs = $('span.txbCreditCard input');
            var cardImg = $('span.txbCreditCard img#card_imag');
            var cbShowAsAnonymous = $("#cbShowAsAnonymous");

            var chargeWait = $(".ChargeWait");
            var chargeFail = $(".ChargeFail");
            var chargeMade = $(".ChargeMade");
            var errorMsg = $('#hfldAlertMsg_Error').val();

            var smartwizard = $("#smartwizard");
            var txbCustomAmount = $("#txbCustomAmount");
            var swBtnPrev = $(".sw-btn-prev");
            var swToolbar = $(".sw-toolbar");

            function getCardName(num) {
                if (num != '') {
                    var cardname = getCardType(num);
                    if (cardname == 'jcb') {
                        cardImg.attr('src', '/Images/Cards/jcb_card_payment.svg');
                    }
                    if (cardname == 'amex') {
                        cardImg.attr('src', '/Images/Cards/american_express.svg');
                    }
                    if (cardname == 'diners_club') {
                        cardImg.attr('src', '/Images/Cards/diners_club.svg');
                    }
                    if (cardname == 'visa') {
                        cardImg.attr('src', '/Images/Cards/card_visa.svg');
                    }
                    if (cardname == 'mastercard') {
                        cardImg.attr('src', '/Images/Cards/Mastercard.svg');
                    }
                    if (cardname == 'discover') {
                        cardImg.attr('src', '/Images/Cards/discover_network_card.svg');
                    }
                    if (cardname == 'maestro') {
                        cardImg.attr('src', '/Images/Cards/Maestro.svg');
                    }
                    if (cardname == 'unknown') {
                        cardImg.attr('src', '/Images/Cards/invalid-Credit_Card.svg');
                    }
                }
                else {
                    cardImg.attr('src', '/Images/Cards/icon-card.svg');
                }
            }
            function getCardimage(name) {
                if (name != '') {
                    if (name.toLowerCase() == 'jcb') {
                        cardImg.attr('src', '/Images/Cards/jcb_card_payment.svg');
                    }
                    if (name.toLowerCase() == 'american express') {
                        cardImg.attr('src', '/Images/Cards/american_express.svg');
                    }
                    if (name.toLowerCase() == 'diners club') {
                        cardImg.attr('src', '/Images/Cards/diners_club.svg');
                    }
                    if (name.toLowerCase() == 'visa') {
                        cardImg.attr('src', '/Images/Cards/card_visa.svg');
                    }
                    if (name.toLowerCase() == 'mastercard') {
                        cardImg.attr('src', '/Images/Cards/Mastercard.svg');
                    }
                    if (name.toLowerCase() == 'discover') {
                        cardImg.attr('src', '/Images/Cards/discover_network_card.svg');
                    }
                    if (name.toLowerCase() == 'maestro') {
                        cardImg.attr('src', '/Images/Cards/Maestro.svg');
                    }
                    if (name.toLowerCase() == 'unknown') {
                        cardImg.attr('src', '/Images/Cards/invalid-Credit_Card.svg');
                    }
                }
                else {
                    cardImg.attr('src', '/Images/Cards/icon-card.svg');
                }
            }

            getCardName(txbCreditCard.val());
            txbCreditCard.mask('0000 0000 0000 0000');
            txbCreditCard.keypress(function () {
                if ($(this).val().length > 2) {
                    getCardName($(this).val());
                }
                else {
                    cardImg.attr('src', "/Images/Cards/icon-card.svg");
                }
            })

            spanTxbCreditCard.focusin(function () {
                $spanTxbCreditCardInputs.each(function () {
                    if ($(this).hasClass('hide-text-box')) {
                        $(this).css('display', 'block');
                    }
                })
                cardImg.css('display', 'block');
                spanTxbCreditCardSlash.css('display', 'block');
            })

            //Added script for adding leading Zeros
            txbCCExpirationMonth.blur(function () {
                var month_num = $(this).val();
                txbCCExpirationMonth.val(addLeadingZeros(month_num, 2));
            })

            txbCCExpirationYear.blur(function () {
                var year_num = $(this).val();
                txbCCExpirationYear.val(addLeadingZeros(year_num, 2));
            })

            //Added script for creating card panel 
            spanTxbCreditCard.focusout(function () {
                if (txbCreditCard.val() != '' || txbCCExpirationMonth.val() != '' || txbCCExpirationYear.val() != '' || txbCvcCode.val() != '') {

                    spanTxbCreditCard.addClass('input--filled--new');
                    cardImg.css('display', 'block');
                    spanTxbCreditCardSlash.css('display', 'block');
                    getCardName(txbCreditCard.val());
                    //cardImg.attr('src', "/Images/Cards/invalid-Credit_Card.svg");
                }
                else {
                    spanTxbCreditCard.removeClass('input--filled--new');
                    cardImg.css('display', 'none');
                    getCardName(txbCreditCard.val());
                    spanTxbCreditCardSlash.css('display', 'none');
                }
            })




            //****************************************
            // MANAGE ALERT MESSAGES
            //**************************************** 
            function showProperBtn() {
                if (hfldCardId.val()) {
                    pnlSubmit.hide();
                    pnlUpdate.show();
                }
                else {
                    pnlSubmit.show();
                    pnlUpdate.hide();
                }
            }




            //****************************************
            // SET VALUES FROM EXISTING CREDIT CARD
            //****************************************
            populateFields();
            function populateFields() {
                getCardimage(hfldBrand.val());
                if (hfldName.val()) {
                    txbNameOnCard.val(hfldName.val());
                    spanTxbNameOnCard.addClass('input--filled');
                }
                if (hfldPostalCode.val()) {
                    txbPostalCode.val(hfldPostalCode.val());
                    spanTxbPostalCode.addClass('input--filled');
                }

                if (hfldLast4.val()) {
                    txbCreditCard.val(hfldLast4.val());
                    spanTxbCreditCard.addClass('input--filled');

                    spanTxbCreditCard.trigger('focusin', (function () {
                        $spanTxbCreditCardInputs.each(function () {
                            if ($(this).hasClass('hide-text-box')) {
                                $(this).css('display', 'block');
                            }
                        })
                        spanTxbCreditCardSlash.css('display', 'block');
                    }));
                }
                if (hfldExpirationMonth.val()) {
                    txbCCExpirationMonth.val(addLeadingZeros(hfldExpirationMonth.val(), 2));
                    spanTxbCCExpirationMonth.addClass('input--filled');
                }
                if (hfldExpirationYear.val()) {
                    txbCCExpirationYear.val(hfldExpirationYear.val());
                    spanTxbCCExpirationYear.addClass('input--filled');
                }
            }
            function clearFields() {
                txbNameOnCard.val('');
                spanTxbNameOnCard.removeClass('input--filled');

                txbCreditCard.val('');
                spanTxbCreditCard.removeClass('input--filled');

                txbCCExpirationMonth.val('');
                spanTxbCCExpirationMonth.removeClass('input--filled');

                txbCCExpirationYear.val('');
                spanTxbCCExpirationYear.removeClass('input--filled');

                txbPostalCode.val('');
                spanTxbPostalCode.removeClass('input--filled');

                txbCvcCode.val('');
                spanTxbCvcCode.removeClass('input--filled');

                cardImg.attr('src', '/Images/Cards/icon-card.svg');
                cardImg.css('display', 'none');
                spanTxbCreditCard.removeClass('input--filled input--filled--new');
                spanTxbCreditCardSlash.css('display', 'none');
            }




            //****************************************
            // MANAGE ALERT MESSAGES
            //****************************************
            showSuccessMsg(hfldShowAlertMsg_Success.val().toString().toLowerCase());
            showInfoMsg(hfldShowAlertMsg_Info.val().toString().toLowerCase());
            showErrorMsg(hfldShowAlertMsg_Error.val().toString().toLowerCase(), errorMsg);

            function showSuccessMsg(visible, msg, xtraMsg) {
                if (visible.toString() == 'true') {
                    alertMsg_Success.show();
                    //alertMsg_Success.removeClass('hide');
                    alertMsg_Success_InitialMsg.text(msg);
                    alertMsg_Success_AdditionalMsg.text(xtraMsg);
                }
                else {
                    alertMsg_Success.hide();
                    //alertMsg_Success.addClass('hide');
                }
            }
            function showInfoMsg(visible, msg, xtraMsg) {
                if (visible.toString() == 'true') {
                    alertMsg_Info.show();
                    //alertMsg_Info.removeClass('hide');
                    alertMsg_Info_InitialMsg.text(msg);
                    alertMsg_Info_AdditionalMsg.text(xtraMsg);
                }
                else {
                    alertMsg_Info.hide();
                    //alertMsg_Info.addClass('hide');
                }
            }
            function showErrorMsg(visible, msg, xtraMsg) {
                if (visible.toString() == 'true') {
                    alertMsg_Error.show();
                    //alertMsg_Error.removeClass('hide');
                    alertMsg_Error_InitialMsg.text(msg);
                    alertMsg_Error_AdditionalMsg.text(xtraMsg);
                }
                else {
                    alertMsg_Error.hide();
                    //alertMsg_Error.addClass('hide');
                }
            }
            function hideAllMsgs() {
                showSuccessMsg(false, '', '');
                showInfoMsg(false, '', '');
                showErrorMsg(false, '', '');
            }


            //*************************************
            // SUBMIT CREDIT CARD INFO
            //*************************************
            function submitCreditcard(e) {
                //form.find('.errors').text('');
                hideAllMsgs();
                try {
                    if ($.trim(txbNameOnCard.val()) === '') {
                        showErrorMsg("Name on Card field is required");
                        e.preventDefault();
                        //Hide spinner
                        //console.log('a');
                        TweenMax.set(spinner, { autoAlpha: 0 });
                        return false;
                    }
                    else {
                        // Disable the submit button to prevent repeated clicks:
                        form.find('.submit').prop('disabled', true);
                        //If form is valid, submit cc
                        if (form[0].checkValidity() == true) {
                            //Show Spinner
                            TweenMax.set(spinner, { autoAlpha: 1 });

                            //Submit token to umbraco
                            TweenMax.delayedCall(0, createCCToken());

                            function createCCToken() {
                                //Hide all messages
                                hideAllMsgs();

                                //Instantiate variables
                                nameOnCard = txbNameOnCard.val();
                                cardNo = txbCreditCard.val();
                                expMonth = txbCCExpirationMonth.val();
                                expYear = txbCCExpirationYear.val();
                                cvc = txbCvcCode.val();
                                postalCode = txbPostalCode.val();

                                //Set the publishable key
                                Stripe.setPublishableKey(publicKey);

                                // Request a token from Stripe:
                                Stripe.card.createToken({
                                    name: nameOnCard,
                                    number: cardNo,
                                    cvc: cvc,
                                    exp_month: expMonth,
                                    exp_year: expYear,
                                    address_zip: postalCode
                                    //address_line1: billing address line 1.
                                    //address_line2: billing address line 2.
                                    //address_city: billing address city.
                                    //address_state: billing address state.
                                    //address_zip: billing zip as a string, e.g. '94301'.
                                    //address_country: billing address country.
                                }, stripeResponseHandler);
                                function stripeResponseHandler(status, response) {
                                    if (response.error) { // Problem!
                                        //Hide spinner
                                        //console.log('b');
                                        TweenMax.set(spinner, { autoAlpha: 0 });
                                        // Show the errors on the form:
                                        showErrorMsg(true, response.error.message);
                                        //Re-enable submission
                                        form.find('.submit').prop('disabled', false);

                                    } else { // Token was created!
                                        //console.log(response);
                                        //Add token to hidden field and submit to umbraco.
                                        hfldCardId.val(response.id);

                                        //Submit token to umbraco
                                        saveTokenToUmbraco(response.id);
                                    }
                                };
                                function saveTokenToUmbraco(token) {
                                    //Instantiate variables
                                    var Type = "POST";
                                    var Url = "/Services/FinancialHandler.asmx/AddCCTokenToCustomer";
                                    var Data;
                                    var ContentType = "application/json; charset=utf-8";
                                    var DataType = "json";
                                    var ProcessData = true;

                                    //Proceed if token exists
                                    if (token.length > 0) {
                                        //Retrieve data from account and display it.
                                        CreateParameters();
                                        CallService();
                                    }

                                    function CreateParameters() {
                                        //Instantiate an array of parameters to pass to handler
                                        var myData = {};
                                        myData.currentUserId = $.trim(hfldCurrentUserId.val());
                                        myData.customerId = $.trim(hfldCustomerId.val());
                                        myData.ccToken = token;

                                        //Set array as json for use in ajax call
                                        Data = JSON.stringify(myData);
                                    }
                                    function CallService() {
                                        $.ajax({
                                            type: Type, //GET or POST or PUT or DELETE verb
                                            url: Url, // Location of the service
                                            data: Data, //Data sent to server
                                            contentType: ContentType, // content type sent to server
                                            dataType: DataType, //Expected data format from server
                                            processdata: ProcessData, //True or False
                                            success: function (msg) { ServiceSucceeded(msg); },
                                            error: function (msg) { ServiceFailed(msg); }
                                        });
                                    }

                                    function ServiceFailed(result) {
                                        //Show the alert box.
                                        showErrorMsg(true, 'Service call failed: ' + result.status + ' ' + result.statusText + ' ' + result.responseText);
                                        Type = null;
                                        varUrl = null;
                                        Data = null;
                                        ContentType = null;
                                        DataType = null;
                                        ProcessData = null;

                                        //Re-enable submission
                                        form.find('.submit').prop('disabled', false);

                                        //Hide spinner
                                        //console.log('c');
                                        TweenMax.set(spinner, { autoAlpha: 0 });
                                    }
                                    function ServiceSucceeded(result) {
                                        //console.log(result.d);

                                        if (result.d.errorMsg.length > 0) {
                                            showErrorMsg(true, result.d.errorMsg);
                                            //console.log(result.d.errorMsg);
                                        }
                                        else {
                                            //Save field IDs
                                            hfldCardToken.val(result.d.StripeIDs.creditCardToken);
                                            hfldCardId.val(result.d.StripeIDs.creditCardId);

                                            //Show the success alert box.
                                            showSuccessMsg(true, 'Creditcard has been added to your account successfully.');
                                        }

                                        //Determine which buttons to show.
                                        showProperBtn();

                                        //Re-enable submission
                                        form.find('.submit').prop('disabled', false);

                                        //Hide spinner
                                        //console.log('d');
                                        TweenMax.set(spinner, { autoAlpha: 0 });
                                    }
                                };
                            }
                        }
                        else {
                            //Re-enable submission
                            form.find('.submit').prop('disabled', false);
                        };
                    }


                }

                catch (err) {
                    //Show the alert box.
                    showErrorMsg('Error: ' + err.message + ' | ' + err);


                    if (err.message != null) {
                        //Hide spinner
                        //console.log('e');
                        TweenMax.set(spinner, { autoAlpha: 0 });
                        //Re-enable submission
                        form.find('.submit').prop('disabled', false);
                    }
                }
            };
            btnSubmit.click(function (e) {
                submitCreditcard(e)
            });
            btnUpdate.click(function (e) {
                submitCreditcard(e)
            });
            //****************************************






            //*************************************
            // Validate CREDIT CARD Fields of Smart Wizard
            // Checking input box of payment step and validatiing.
            //*************************************/
            ValidateStep = function (e, stepDirection) {
                var myBool = false;

                if (stepDirection === "forward") {

                    // Feteching the TxtCard input box value 
                    var txtCard = $.trim(txbCreditCard.val());

                    //  Convert the value in number
                    var check = parseInt(txtCard, 10);

                    //Checking the name on card input box , it should not empty 
                    if ($.trim(txbNameOnCard.val()) === '') {
                        showErrorMsg(true, "Name on Card field is required");
                        myBool = true;
                    }
                    // checking for postal code
                    if ($.trim(txbPostalCode.val()) === '') {
                        showErrorMsg(true, "Postal code field is required");
                        myBool = true;
                    }

                    // Checking txtcard value make sure it should not empty and after that check txt card should be a numeric value
                    if (txtCard === '') {
                        showErrorMsg(true, "Card number field is required");
                        myBool = true;
                    }
                    else if (Stripe.card.validateCardNumber(txtCard) === false && txbCreditCard.val().trim() != hfldLast4.val().trim()) {
                        showErrorMsg(true, "Please correct your card number");
                        myBool = true;
                        // Pushing back to previous step
                        // $('.sw-btn-prev').click();
                    }

                    if ($.trim(txbCvcCode.val()) === '') {
                        showErrorMsg(true, "Card CVC field is required");
                        myBool = true;
                    }

                    if ($.trim(txbCCExpirationMonth.val()) === '') {
                        showErrorMsg(true, "Expiration month field is required");
                        myBool = true;
                    }

                    if ($.trim(txbCCExpirationYear.val()) === '') {
                        showErrorMsg(true, "Expiration year field is required");
                        myBool = true;
                    }

                    if ($.trim(txbNameOnCard.val()) === '' && $.trim(txbPostalCode.val()) === '' && txtCard === '' && $.trim(txbCvcCode.val()) === '' && $.trim(txbCCExpirationMonth.val()) === '' && $.trim(txbCCExpirationYear.val()) === '') {
                        showErrorMsg(true, "All fields need to be properly filled out");
                        //  txbNameOnCard.css("border-color", "red");

                        myBool = true;
                    }

                    if (myBool) {
                        //Hide spinner
                        TweenMax.set(spinner, { autoAlpha: 0 });
                    }

                }
                else if (stepDirection === "backward") {
                    myBool = true;
                }
                //console.log(myBool);
                return myBool;
            }






            //*************************************
            // SUBMIT CREDIT CARD Via Smart Wizard
            //*************************************
            function submitCreditcard_Checkout(e) {
                try {

                    if (ValidateStep(e)) {
                        e.stopPropagation();
                        //console.log('returned true');

                        //Re-enable submission
                        //form.find('.sw-submit').prop('disabled', false);

                    }
                    else {
                        //console.log('returned false');
                        // Disable the submit button to prevent repeated clicks:
                        form.find('.sw-submit').prop('disabled', true);

                        //If form is valid, submit cc
                        if (form[0].checkValidity() == true) {

                            //Show Spinner
                            TweenMax.set(spinner, { autoAlpha: 1 });

                            //Submit token to umbraco
                            TweenMax.delayedCall(0, createCCToken());

                            function createCCToken() {
                                //Hide all messages
                                hideAllMsgs();

                                //Instantiate variables
                                nameOnCard = txbNameOnCard.val();
                                cardNo = txbCreditCard.val();
                                expMonth = txbCCExpirationMonth.val();
                                expYear = txbCCExpirationYear.val();
                                cvc = txbCvcCode.val();
                                postalCode = txbPostalCode.val();

                                //Set the publishable key
                                Stripe.setPublishableKey(publicKey);

                                // Request a token from Stripe:
                                Stripe.card.createToken({
                                    name: nameOnCard,
                                    number: cardNo,
                                    cvc: cvc,
                                    exp_month: expMonth,
                                    exp_year: expYear,
                                    address_zip: postalCode
                                }, stripeResponseHandler);

                                function stripeResponseHandler(status, response) {
                                    //console.log('stripeResponseHandler.status: ' + status);
                                    //console.log('stripeResponseHandler.response: ' + response.error.message);
                                    if (response.error) { // Problem!
                                        // Show the errors on the form:
                                        //console.log('g');
                                        TweenMax.set(spinner, { autoAlpha: 0 });
                                        if (response.error.message != undefined) {
                                            showErrorMsg(true, response.error.message);
                                        }
                                        //Re-enable submission
                                        form.find('.sw-submit').prop('disabled', false);

                                        //Pushing back to previous step
                                        $('.sw-btn-prev').click();

                                    }
                                    else {
                                        // Token was created!
                                        //Add token to hidden field and submit to umbraco.
                                        //hfldCardId.val(response.id);
                                        hfldCardToken.val(response.id);

                                        //Submit token to umbraco
                                        saveTokenToUmbraco(response.id);
                                    }
                                }

                                function saveTokenToUmbraco(token) {
                                    //Instantiate variables
                                    var Type = "POST";
                                    var Url = "/Services/FinancialHandler.asmx/AddCCTokenToCustomer";
                                    var Data;
                                    var ContentType = "application/json; charset=utf-8";
                                    var DataType = "json";
                                    var ProcessData = true;
                                    var msg; // just a variable for message to stop unexpected errors.

                                    //Proceed if token exists
                                    if (token.length > 0) {
                                        //Retrieve data from account and display it.
                                        CreateParameters();
                                        CallService();
                                    }

                                    function CreateParameters() {
                                        //Instantiate an array of parameters to pass to handler
                                        var myData = {};
                                        myData.currentUserId = $.trim(hfldCurrentUserId.val());
                                        myData.customerId = $.trim(hfldCustomerId.val());
                                        myData.ccToken = token;

                                        //Set array as json for use in ajax call
                                        //console.log('Create Parameters');
                                        //console.log(JSON.stringify(myData));
                                        Data = JSON.stringify(myData);
                                    }

                                    function CallService() {
                                        $.ajax({
                                            type: Type, //GET or POST or PUT or DELETE verb
                                            url: Url, // Location of the service
                                            data: Data, //Data sent to server
                                            contentType: ContentType, // content type sent to server
                                            dataType: DataType, //Expected data format from server
                                            processdata: ProcessData, //True or False
                                            success: function (msg) { ServiceSucceeded(msg); }, // msg not defined error
                                            error: function (msg) { ServiceFailed(msg); }
                                        });
                                    }

                                    function ServiceSucceeded(result) {
                                        if (result.d.errorMsg.length > 0 && result.d.errorMsg != undefined) {
                                            //Move to previous step
                                            smartwizard.smartWizard("prev");

                                            //Determine which buttons to show.
                                            showProperBtn();

                                            //Re-enable submission
                                            form.find('.sw-submit').prop('disabled', false);

                                            //Hide spinner
                                            //console.log('h');
                                            TweenMax.set(spinner, { autoAlpha: 0 });

                                            //Just used for debug please remove it later on
                                            console.log(result.d);

                                            //Displaying a failer message to user so user able to know there is something bad happen and payment not possible atm!
                                            //chargeFail.removeClass("hide");

                                            //Hide : Displaying a message to user while we are processing the functions
                                            chargeWait.addClass("hide");
                                            
                                            showErrorMsg(true, result.d.errorMsg);
                                            console.log('Service call submitted but returned failed: ' + result.d.errorMsg);

                                        }
                                        else {
                                            //Just used for debug please remove it later on
                                            console.log(result.d);
                                            console.log("CC added to customer's acct.");

                                            //Save field IDs
                                            hfldCardToken.val(result.d.StripeIDs.creditCardToken);
                                            hfldCardId.val(result.d.StripeIDs.creditCardId);

                                            //Calling the ChargeCreditCard Method to Charge the card for pledge amount.
                                            console.log("Charging creditcard.");
                                            chargeCreditCard(e);


                                        }
                                    }

                                    function ServiceFailed(result) {
                                        //Move to previous step
                                        smartwizard.smartWizard("prev");

                                        //Show the alert box.
                                        showErrorMsg(true, 'Service call failed: ' + result.status + ' ' + result.statusText + ' ' + result.responseText);
                                        Type = null;
                                        varUrl = null;
                                        Data = null;
                                        ContentType = null;
                                        DataType = null;
                                        ProcessData = null;

                                        //Re-enable submission
                                        form.find('.sw-submit').prop('disabled', false);

                                        //Hide : Displaying a failer message to user so user able to know there is something bad happen and payment not possible atm!
                                        chargeFail.removeClass("hide");
                                        console.log('Service call failed: ' + result.status + ' ' + result.statusText + ' ' + result.responseText);

                                        //Hide spinner
                                        //console.log('i');
                                        TweenMax.set(spinner, { autoAlpha: 0 });
                                    }
                                }
                            }
                        }
                        else {
                            //Re-enable submission
                            form.find('.sw-submit').prop('disabled', false);

                            //HIde : Displaying a message to user while we are processing the funcations
                            chargeWait.addClass("hide");
                            //TweenMax.set(spinner, { autoAlpha: 0 });

                        }
                    }
                }
                catch (err) {

                    if (err.message != null) {

                        //Show the alert box.
                        showErrorMsg(true, 'Error: ' + err.message + ' | ' + err);

                        //Hide spinner
                        //console.log('j');
                        TweenMax.set(spinner, { autoAlpha: 0 });

                        //Re-enable submission
                        form.find('.sw-submit').prop('disabled', false);

                        //HIde : Displaying a message to user while we are processing the funcations
                        chargeWait.addClass("hide");

                    }
                }


            };





            //*************************************
            // CHARGE CREDIT CARD Via Smart Wizard
            //*************************************
            function chargeCreditCard(e) {
                try {
                    if (ValidateStep(e)) {
                        e.stopPropagation();
                        //console.log('***** e.stopPropagation()');
                    }
                    else {
                        // Disable the submit button to prevent repeated clicks:
                        form.find('.sw-submit').prop('disabled', true);

                        //If form is valid, submit cc
                        if (form[0].checkValidity() == true) {

                            // Hiding error message if there is any from last fucntions
                            showErrorMsg(false, '', '');
                            //Instantiate variables
                            var Type = "POST";
                            var Url = "/Services/FinancialHandler.asmx/SubmitPledge";
                            var Data;
                            var ContentType = "application/json; charset=utf-8";
                            var DataType = "json";
                            var ProcessData = true;
                            var msg;

                            //METHODS
                            function CreateParameters() {
                                //Instantiate an array of parameters to pass to handler
                                var myData = {};
                                myData.currentUserId = $.trim(hfldCurrentUserId.val());
                                myData.CustomerId = $.trim(hfldCustomerId.val());
                                myData.creditcardId = $.trim(hfldCardId.val());
                                myData.activePhaseNode = ltrPhaseNode.val();
                                myData.SelectedReward = $("input[name='rbtn2']:checked").val();
                                myData.PledgeAmount = hfldPledgeamt.val();
                                myData.ShowAsAnonymous = "False";
                                myData.campaignId = campaignId;
                                myData.stripeUserId = stripeUserId;

                                if (cbShowAsAnonymous.is(':checked')) {
                                    myData.ShowAsAnonymous = "True";
                                }

                                myData.BillingAdd1 = txtBillingAddress1.val();
                                myData.BillingAdd2 = txtBillingAddress2.val();
                                myData.BillingCity = txtBillingCity.val();
                                myData.BillingState = txtBillingState.val();
                                myData.BillingZip = txtBillingZip.val();

                                myData.ShippingAdd1 = txtShippingAddress1.val();
                                myData.ShippingAdd2 = txtShippingAddress2.val();
                                myData.ShippingCity = txtShippingCity.val();
                                myData.ShippingState = txtShippingState.val();
                                myData.ShippingZip = txtShippingZip.val();

                                if (is.truthy(hfldUseAltEmail.val())) {
                                    myData.alternativeEmail = txbAltEmail.val();
                                }
                                else {
                                    myData.alternativeEmail = '';
                                }

                                //Goal Boolean, field for keep track that PhaseGoal is 100% completed or not, if 100% complete then some fee applied on Pledge
                                //myData.Goal = $('#ltrGoalBool').val();

                                //Set array as json for use in ajax call
                                Data = JSON.stringify(myData);

                                //Just used for debug please remove it later on
                                //console.log(Data);
                            }
                            function CallService() {
                                $.ajax({
                                    type: Type, //GET or POST or PUT or DELETE verb
                                    url: Url, // Location of the service
                                    data: Data, //Data sent to server
                                    contentType: ContentType, // content type sent to server
                                    dataType: DataType, //Expected data format from server
                                    processdata: ProcessData, //True or False
                                    success: function (msg) { ServiceSucceeded(msg); },
                                    error: function (msg) { ServiceFailed(msg); }
                                });
                            }
                            function ServiceFailed(result) {
                                console.log('***** ServiceFailed() - Must go back to step 3 and display error msg');
                                console.log(result.status);
                                console.log(result.statusText);
                                console.log(result.responseText);

                                //Move to previous step
                                smartwizard.smartWizard("prev");

                                //Show the alert box.
                                showErrorMsg(true, 'Service call failed: <br />' + result.status + '<br />' + result.statusText + '<br />' + result.responseText);
                                Type = null;
                                varUrl = null;
                                Data = null;
                                ContentType = null;
                                DataType = null;
                                ProcessData = null;

                                //Re-enable submission
                                form.find('.sw-submit').prop('disabled', false);

                                //Hide spinner
                                TweenMax.set(spinner, { autoAlpha: 0 });
                            }
                            function ServiceSucceeded(result) {
                                if (result.d.Failed != undefined && result.d.Failed.length > 0) {
                                    console.log('***** ServiceSucceeded() returned an error msg from our server - Must go back to step 3 and display error msg');
                                    console.log(result.d.Failed);

                                    //Move to previous step
                                    smartwizard.smartWizard("prev");

                                    //Show error message
                                    showErrorMsg(true, result.d.Failed);

                                    //Re-enable submission
                                    form.find('.sw-submit').prop('disabled', false);

                                    //Hide spinner
                                    TweenMax.set(spinner, { autoAlpha: 0 });

                                    //HIde : Displaying a message to user while we are processing the funcations
                                    chargeWait.addClass("hide");
                                }
                                else {
                                    //Show the success alert box.
                                    console.log('***** service succeeded.');
                                    console.log(result.d);

                                    //Show message
                                    showSuccessMsg(true, 'Success');

                                    //return true;
                                    //document.getElementById("json").innerHTML = JSON.stringify(JSON.parse(result.d), undefined, 2);

                                    //Displaying message on successful of payment 
                                    chargeMade.removeClass("hide");

                                    //HIde : Displaying a message to user while we are processing the funcations
                                    chargeWait.addClass("hide");


                                    //#Region for JSON OUTPUT ------ Output Json for Development, Comment it out 
                                    //function output(inp) {
                                    //    //document.body.appendChild(document.createElement('pre')).innerHTML = inp;
                                    //    document.getElementById("json").innerHTML = inp;
                                    //}

                                    //function syntaxHighlight(json) {
                                    //    json = json.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;');
                                    //    return json.replace(/("(\\u[a-zA-Z0-9]{4}|\\[^u]|[^\\"])*"(\s*:)?|\b(true|false|null)\b|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?)/g, function (match) {
                                    //        var cls = 'number';
                                    //        if (/^"/.test(match)) {
                                    //            if (/:$/.test(match)) {
                                    //                cls = 'key';
                                    //            } else {
                                    //                cls = 'string';
                                    //            }
                                    //        } else if (/true|false/.test(match)) {
                                    //            cls = 'boolean';
                                    //        } else if (/null/.test(match)) {
                                    //            cls = 'null';
                                    //        }
                                    //        return '<span class="' + cls + '">' + match + '</span>';
                                    //    });
                                    //}
                                    //output(syntaxHighlight(JSON.stringify(JSON.parse(result.d.GetJson), undefined, 4)));
                                    // # End Region for JSON OUTPUT ------------------------------------------------------------



                                    //Hide spinner
                                    //console.log('m');
                                    TweenMax.set(spinner, { autoAlpha: 0 });
                                }
                            }


                            //Just used for debug please remove it later on
                            //console.log('***** call function CreateParameters()');
                            CreateParameters();

                            //Just used for debug please remove it later on
                            //console.log('***** end function CreateParameters()');
                            CallService();
                        }
                        else {
                            //Re-enable submission
                            form.find('.sw-submit').prop('disabled', false);

                            //HIde : Displaying a message to user while we are processing the funcations
                            chargeWait.addClass("hide");
                            //TweenMax.set(spinner, { autoAlpha: 0 });
                            //console.log('***** form[0].checkValidity() == false');
                        }
                    }


                }
                catch (err) {
                    //console.log('***** try/catch error');
                    if (err.message != null) {
                        //Show the alert box.
                        showErrorMsg(true, 'Error: ' + err.message + ' | ' + err);
                        //Hide spinner
                        //console.log('n *****  ' + err.message + ' | ' + err);
                        TweenMax.set(spinner, { autoAlpha: 0 });

                        //Re-enable submission
                        form.find('.sw-submit').prop('disabled', false);

                        //HIde : Displaying a message to user while we are processing the funcations
                        chargeWait.addClass("hide");
                    }
                }
            };





            //****************************************
            // SMART WIZARD- SUBMIT TRANSACTION     http://techlaboratory.net/smartwizard/documentation
            //****************************************
            smartwizard.on("leaveStep", function (e, anchorObject, stepNumber, stepDirection) {
                //console.log('Step No:  ' + stepNumber);
                //console.log('stepDirection:  ' + stepDirection);
                try {
                    //Instantiate variables
                    var isStepValid = true; //false;
                    var txtval = txbCustomAmount.val();
                    var txtLength = txtval.length;
                    var rbtn_one = $("input[name='rbtnAmount']:checked").val();
                    swToolbar = $(".sw-toolbar");
                    var alertPnl = $('.alertBoxRow.alert');
                    var alertPnlText = $('.alertBoxRow.alert .initialText');
                    var errorMsgs = "";

                    //Clear additional text portion of alert box
                    $('.alertBoxRow.alert .additionalText').text('');

                    //Firing function for validate the custom text and radio selected pledge amount
                    txtEvent();

                    bindReview();

                    if (stepNumber === 0) {
                        //Is custom amount selected but empty?
                        if (txtLength < 1 && rbtn_one === "Custom" || txtval === "0" && rbtn_one === "Custom") {
                            // Display the alert Message 
                            alertPnl.show(); //removeClass('hide');
                            alertPnlText.text('You must fill custom amount greater than zero in pledge section');
                            //showErrorMsg(true, 'You must fill custom amount greater than zero in pledge section');

                            txbCustomAmount.focus();
                            isStepValid = false;
                            swToolbar.hide(); //console.log('leaveStep | hide 0');
                        }
                        else {
                            hideAllMsgs();
                            alertPnl.hide(); //addClass('hide');
                            isStepValid = true;
                            swToolbar.show(); //console.log('leaveStep | show 0');
                        }
                    }
                    else if (stepNumber === 1) {
                        //Reset alert msgs
                        alertPnl.hide();
                        errorMsgs = "";

                        if (stepDirection === 'backward') {
                            //Going back.  ignore validation
                            isStepValid = true;
                        }
                        else {
                            //Instantiate variables
                            var rbtnSelectedReward = $("input[name='rbtn2']:checked").data("ramt");

                            //If alt email is being used, validate
                            if (is.truthy(hfldUseAltEmail.val())) {
                                if (is.not.email(txbAltEmail.val())) {
                                    var msg = "*Please enter a valid alternate email.";
                                    if (is.empty(errorMsgs)) { errorMsgs = msg } else { errorMsgs = errorMsgs + '\n' + msg; }
                                }
                            }

                            //Validate billing address
                            if (is.empty(txtBillingAddress1.val())) {
                                var msg = "*Please enter a billing address.";
                                if (is.empty(errorMsgs)) { errorMsgs = msg } else { errorMsgs = errorMsgs + '\n' + msg; }
                            }
                            if (is.empty(txtBillingCity.val())) {
                                var msg = "*Please enter a billing city.";
                                if (is.empty(errorMsgs)) { errorMsgs = msg } else { errorMsgs = errorMsgs + '\n' + msg; }
                            }
                            if (is.empty(txtBillingState.val())) {
                                var msg = "*Please enter a billing state.";
                                if (is.empty(errorMsgs)) { errorMsgs = msg } else { errorMsgs = errorMsgs + '\n' + msg; }
                            }
                            if (is.empty(txtBillingZip.val())) {
                                var msg = "*Please enter a billing postal code";
                                if (is.empty(errorMsgs)) { errorMsgs = msg } else { errorMsgs = errorMsgs + '\n' + msg; }
                            }


                            //If a reward was selected, validate shipping fields
                            if (rbtnSelectedReward !== 0) {
                                if (is.empty(txtShippingAddress1.val())) {
                                    var msg = "*Please enter a shipping address.";
                                    if (is.empty(errorMsgs)) { errorMsgs = msg } else { errorMsgs = errorMsgs + '\n' + msg; }
                                }
                                if (is.empty(txtShippingCity.val())) {
                                    var msg = "*Please enter a shipping city.";
                                    if (is.empty(errorMsgs)) { errorMsgs = msg } else { errorMsgs = errorMsgs + '\n' + msg; }
                                }
                                if (is.empty(txtShippingState.val())) {
                                    var msg = "*Please enter a shipping state.";
                                    if (is.empty(errorMsgs)) { errorMsgs = msg } else { errorMsgs = errorMsgs + '\n' + msg; }
                                }
                                if (is.empty(txtShippingZip.val())) {
                                    var msg = "*Please enter a shipping postal code.";
                                    if (is.empty(errorMsgs)) { errorMsgs = msg } else { errorMsgs = errorMsgs + '\n' + msg; }
                                }
                            }

                            //Show error msgs if present
                            if (is.not.empty(errorMsgs)) {
                                alertPnlText.text(errorMsgs);
                                alertPnl.show();
                                isStepValid = false;
                                swToolbar.show(); //console.log('leaveStep | show 1');
                            };
                        }


                    }
                    else if (stepNumber === 2) {
                        //Reset alert msgs
                        alertPnl.hide();
                        errorMsgs = "";

                        if (stepDirection === 'backward') {
                            //Going back.  ignore validation
                            isStepValid = true;
                        }
                        else {
                            //Instantiate variables
                            //var txtCard = $.trim(txbCreditCard.val());
                            //var check = parseInt(txtCard, 10);





                            //console.log('txbNameOnCard: ' + txbNameOnCard.val().trim());
                            //console.log('txbPostalCode: ' + txbPostalCode.val().trim());
                            //console.log('txbCreditCard: ' + txbCreditCard.val().trim());
                            //console.log('txbCCExpirationYear: ' + txbCCExpirationYear.val().trim());
                            //console.log('txbCvcCode: ' + txbCvcCode.val().trim());

                            //console.log('txbCCExpirationMonth: ' + txbCCExpirationMonth.val().trim());
                            //console.log('txbCCExpirationMonth t/f: ' + is.not.number(txbCCExpirationMonth.val().trim()));
                            //var mo2 = txbCCExpirationMonth.val().trim();
                            //console.log('mo2: ' + mo2);
                            //console.log('mo2 t/f: ' + is.not.number(mo2));


                            //Did user accept MWoO terms and conditions?
                            if (is.not.truthy(acceptTerms())) {
                                var msg = "*Please read and accept the 'My Window of Opportunity' terms and conditions.";
                                if (is.empty(errorMsgs)) { errorMsgs = msg } else { errorMsgs = errorMsgs + '\n' + msg; }
                            }

                            //Validate step fields
                            if (is.empty(txbNameOnCard.val().trim())) {
                                var msg = "*Enter the person's name printed on the credit card.";
                                if (is.empty(errorMsgs)) { errorMsgs = msg } else { errorMsgs = errorMsgs + '\n' + msg; }
                            }

                            if (is.not.usZipCode(txbPostalCode.val().trim()) && is.not.caPostalCode($.trim(txbPostalCode.val()))) {
                                var msg = "*Enter the postal code associated with the credit card.";
                                if (is.empty(errorMsgs)) { errorMsgs = msg } else { errorMsgs = errorMsgs + '\n' + msg; }
                            }

                            if (Stripe.card.validateCardNumber(txbCreditCard.val().trim()) === false && txbCreditCard.val().trim() != hfldLast4.val().trim()) {
                                var msg = "*Enter a valid credit card number.";
                                if (is.empty(errorMsgs)) { errorMsgs = msg } else { errorMsgs = errorMsgs + '\n' + msg; }
                            }


                            var mo = txbCCExpirationMonth.val().trim();
                            if (is.empty(mo)) {
                                var msg = "*Enter your credit card's expiration month a.";
                                if (is.empty(errorMsgs)) { errorMsgs = msg } else { errorMsgs = errorMsgs + '\n' + msg; }
                            }
                            else if (is.not.number(parseInt(mo))) {
                                var msg = "*Enter your credit card's expiration month b.";
                                if (is.empty(errorMsgs)) { errorMsgs = msg } else { errorMsgs = errorMsgs + '\n' + msg; }
                            }
                            else if (is.not.within(parseInt(mo), 0, 13)) {
                                var msg = "*Enter your credit card's expiration month c.";
                                if (is.empty(errorMsgs)) { errorMsgs = msg } else { errorMsgs = errorMsgs + '\n' + msg; }
                            }


                            var yr = txbCCExpirationYear.val().trim();
                            if (is.empty(yr)) {
                                var msg = "*Enter your credit card's expiration year a.";
                                if (is.empty(errorMsgs)) { errorMsgs = msg } else { errorMsgs = errorMsgs + '\n' + msg; }
                            }
                            else if (is.not.number(parseInt(yr))) {
                                var msg = "*Enter your credit card's expiration year b.";
                                if (is.empty(errorMsgs)) { errorMsgs = msg } else { errorMsgs = errorMsgs + '\n' + msg; }
                            }
                            else if (is.not.within(parseInt(mo), 0, 99)) {
                                var msg = "*Enter your credit card's expiration year c.";
                                if (is.empty(errorMsgs)) { errorMsgs = msg } else { errorMsgs = errorMsgs + '\n' + msg; }
                            }


                            if (is.empty(txbCvcCode.val().trim())) {
                                var msg = "*Enter your credit card's cvc code .";
                                if (is.empty(errorMsgs)) { errorMsgs = msg } else { errorMsgs = errorMsgs + '\n' + msg; }
                            }


                            //Show error msgs if present
                            if (is.not.empty(errorMsgs)) {
                                alertPnlText.text(errorMsgs);
                                alertPnl.show();
                                isStepValid = false;
                                swToolbar.show(); //console.log('leaveStep | show 1');
                            }
                            else {
                                //Valid data.  Proceed with submitting
                                swToolbar.hide();
                                SubmitTransaction(e);
                            }
                        }
                    }

                    return isStepValid;
                }
                catch (err) {
                    console.log('Error: ' + err.message + ' | ' + err);
                    return false;
                }
            });





            /********************/
            // SMART WIZARD- Submit Transaction Method Call 
            /**********************/
            SubmitTransaction = function (e) {
                //$(document).ready(function (e) {
                $(document).on("click", ".sw-submit", function (e) {

                    e.preventDefault();
                    //console.log('submitting... show spinner');
                    //Show spinner
                    TweenMax.set(spinner, { autoAlpha: 1 });

                    //STEPS NEEDED FOR SUBMITTING TRANSACTION
                    //=========================================================================
                    //Does user have cc token?
                    //	Yes
                    //		Was credit card information updated?
                    //			yes:
                    //				Update token
                    //				    save to umbraco
                    //				submit token with payment
                    //					obtain result.  valid?
                    //						Yes:
                    //							save pledge/result info in umbraco
                    //						No:
                    //							display error message.
                    //			No:
                    //				submit token with payment
                    //				obtain result.  valid?
                    //					Yes:
                    //						save pledge/result info in umbraco
                    //					No:
                    //						display error message.
                    //	No
                    //		Create token
                    //			save to umbraco
                    //		submit token with payment
                    //			obtain result.  valid?
                    //				Yes:
                    //					save pledge/result info in umbraco
                    //				No:
                    //					display error message.

                    var successful = false;
                    var updated = false;
                    var changedValues = "";
                    //hfldCardToken.val('');



                    //Does the user have a creditcard token?
                    if (hfldCardToken.val().trim()) {
                        //User has a credit card token
                        //Was credit card information updated?
                        if (txbNameOnCard.val().trim() != hfldName.val().trim()) { updated = true; changedValues = changedValues + " hfldName" };
                        if (txbCreditCard.val().trim() != hfldLast4.val().trim()) { updated = true; changedValues = changedValues + " hfldLast4" };
                        if (txbCCExpirationMonth.val().trim().replace(/^0+/, '') != hfldExpirationMonth.val().trim().replace(/^0+/, '')) { updated = true; changedValues = changedValues + " hfldExpirationMonth" };
                        if (txbCCExpirationYear.val().trim().replace(/^0+/, '') != hfldExpirationYear.val().trim().replace(/^0+/, '')) { updated = true; changedValues = changedValues + " hfldExpirationYear" };
                        if (txbPostalCode.val().trim().replace(/^0+/, '') != hfldPostalCode.val().trim().replace(/^0+/, '')) { updated = true; changedValues = changedValues + " hfldPostalCode" };


                        //console.log('Updated: ' + updated);
                        if (updated) {
                            //Credit card was updated.
                            //				Update token
                            //				    save to umbraco

                            // Displaying a message to user while we are processing the funcations
                            chargeWait.removeClass("hide");
                            //showInfoMsg(true, "Please Wait, We are processing your pledge amount");

                            //Submit Credit Card details for stripe payment and Call Tokens
                            submitCreditcard_Checkout(e);

                            //HIde : Displaying a message to user while we are processing the funcations


                            ////				submit token with payment
                            //if (successful == true) {
                            //    //					obtain result.  valid?
                            //    //						Yes:
                            //    //							save pledge/result info in umbraco
                            //    //						No:
                            //    //							display error message.
                            //    chargeCreditCard(e);

                            //}

                        }
                        else {
                            // Displaying a message to user while we are processing the funcations
                            chargeWait.removeClass("hide");

                            //Credit card was not updated

                            //				submit token with payment
                            //				obtain result.  valid?
                            //					Yes:
                            //						save pledge/result info in umbraco
                            //					No:
                            //						display error message.
                            chargeCreditCard(e);
                        }
                    }
                    else {

                        //		Create token
                        //			save to umbraco
                        //successful = submitCreditcard_Checkout(e);
                        ////		submit token with payment
                        //if (successful == true) {
                        //    //			obtain result.  valid?
                        //    //				Yes:
                        //    //					save pledge/result info in umbraco
                        //    //				No:
                        //    //					display error message.
                        //    //    



                        submitCreditcard_Checkout(e);


                        //else
                        //{
                        //    // Here need to add a user friendly error, what should be reutrn either return some regrading the stripe error or just message
                        //    chargeFail.removeClass("hide");

                        //}
                    }

                });
                //});
            }




            //****************************************
            // DELETE CREDIT CARD
            //****************************************
            btnDelete.click(function () {
                try {
                    //Verify that the user wants to delete their card //Zebra Dialgo | http://stefangabos.ro/jquery/zebra-dialog/
                    $.Zebra_Dialog('Are you sure that you wish to delete your credit card?<br><br><strong>Confirm deletion.</strong>', {
                        'type': 'warning', //confirmation, information, warning, error and question
                        'title': 'Delete Creditcard?',
                        'buttons': [
                            {
                                caption: 'Yes', callback: function () {
                                    //Show Spinner
                                    TweenMax.set(spinner, { autoAlpha: 1 });
                                    //Call function
                                    TweenMax.delayedCall(0, submitDeletionRequest);
                                }
                            },
                            { caption: 'No' }
                        ]
                    });

                    function submitDeletionRequest() {
                        //Hide all messages
                        hideAllMsgs();

                        //Instantiate variables
                        var Type = "POST";
                        var Url = "/Services/FinancialHandler.asmx/DeleteCreditcard";
                        var Data;
                        var ContentType = "application/json; charset=utf-8";
                        var DataType = "json";
                        var ProcessData = true;

                        //Retrieve data from account and display it.
                        CreateParameters();
                        CallService();
                        //disableControls();

                        function CreateParameters() {
                            //Instantiate an array of parameters to pass to handler
                            var myData = {};
                            myData.currentUserId = $.trim(hfldCurrentUserId.val());
                            myData.customerId = $.trim(hfldCustomerId.val());
                            myData.cardId = $.trim(hfldCardId.val());

                            //Set array as json for use in ajax call
                            Data = JSON.stringify(myData);
                        }
                        function CallService() {
                            $.ajax({
                                type: Type, //GET or POST or PUT or DELETE verb
                                url: Url, // Location of the service
                                data: Data, //Data sent to server
                                contentType: ContentType, // content type sent to server
                                dataType: DataType, //Expected data format from server
                                processdata: ProcessData, //True or False
                                success: function (msg) { ServiceSucceeded(msg); },
                                error: function (msg) { ServiceFailed(msg); }
                            });
                        }
                        function ServiceFailed(result) {
                            //Show the alert box.
                            showErrorMsg('Service call failed: ' + result.status + ' ' + result.statusText + ' ' + result.responseText);
                            Type = null;
                            varUrl = null;
                            Data = null;
                            ContentType = null;
                            DataType = null;
                            ProcessData = null;

                            //Hide spinner
                            //console.log('o');
                            TweenMax.set(spinner, { autoAlpha: 0 });
                        }
                        function ServiceSucceeded(result) {
                            //console.log(result.d);

                            //Save field IDs
                            hfldCardToken.val('');
                            hfldCardId.val('');

                            //Set values from query to screen
                            clearFields();

                            //Determine which buttons to show.
                            showProperBtn();

                            //Show the alert box.
                            showSuccessMsg(true, 'Credit card has been deleted successfully.', '');

                            //Hide spinner
                            //console.log('p');
                            TweenMax.set(spinner, { autoAlpha: 0 });
                        }
                    }
                }
                catch (err) {
                    //Show the alert box.
                    showErrorMsg('Error: ' + err.message + ' | ' + err);

                    //Hide spinner
                    if (err.message != null) {
                        //console.log('q');
                        TweenMax.set(spinner, { autoAlpha: 0 });
                    }
                }
            });


        }


        //Run only if element exists
        if ($('#txbCreditCard').length > 0) { jsCCValidation(); }
    }
    catch (err) {
        console.log('ERROR: [jsCCValidation] ' + err.message + ' | ' + err);
    }
});
