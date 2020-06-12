//****************************************
// CHECK IF EMAILS ARE VALID
//****************************************
function isEmailAddressValid(emailAddress) {
    var pattern = /^([a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+(\.[a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+)*|"((([ \t]*\r\n)?[ \t]+)?([\x01-\x08\x0b\x0c\x0e-\x1f\x7f\x21\x23-\x5b\x5d-\x7e\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|\\[\x01-\x09\x0b\x0c\x0d-\x7f\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))*(([ \t]*\r\n)?[ \t]+)?")@(([a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.)+([a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.?$/i;
    return pattern.test(emailAddress);
};

function scrollToAnchor(aid) {
    var aTag = $("a[id='" + aid + "']");
    if (aTag.length) {
        $('html,body').animate({ scrollTop: aTag.offset().top }, 'fast');
    }
}

//Used on the campaign page to close the alert messages after submitting a review.
function CloseThisDiv(anchorId) {
    $('#' + anchorId).click(function () {
        $(this).parent().closest('.alert-box').hide();
    });
}

// Global function accessable out side this file
function addLeadingZeros(n, length) {
    var str = (n > 0 ? n : -n) + "";
    var zeros = "";
    for (var i = length - str.length; i > 0; i--)
        zeros += "0";
    zeros += str;
    return n >= 0 ? zeros : "-" + zeros;
}

function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
    return true;
}

function getCardType(cur_val) {
    var sel_brand;
    // the regular expressions check for possible matches as you type, hence the OR operators based on the number of chars
    // regexp string length {0} provided for soonest detection of beginning of the card numbers this way it could be used for BIN CODE detection also

    //JCB
    jcb_regex = new RegExp('^(?:2131|1800|35)[0-9]{0,}$'); //2131, 1800, 35 (3528-3589)
    // American Express
    amex_regex = new RegExp('^3[47][0-9]{0,}$'); //34, 37
    // Diners Club
    diners_regex = new RegExp('^3(?:0[0-59]{1}|[689])[0-9]{0,}$'); //300-305, 309, 36, 38-39
    // Visa
    visa_regex = new RegExp('^4[0-9]{0,}$'); //4
    // MasterCard
    mastercard_regex = new RegExp('^(5[1-5]|222[1-9]|22[3-9]|2[3-6]|27[01]|2720)[0-9]{0,}$'); //2221-2720, 51-55
    maestro_regex = new RegExp('^(5[06789]|6)[0-9]{0,}$'); //always growing in the range: 60-69, started with / not something else, but starting 5 must be encoded as mastercard anyway
    //Discover
    discover_regex = new RegExp('^(6011|65|64[4-9]|62212[6-9]|6221[3-9]|622[2-8]|6229[01]|62292[0-5])[0-9]{0,}$');
    ////6011, 622126-622925, 644-649, 65



    // get rid of anything but numbers
    cur_val = cur_val.replace(/\D/g, '');

    // checks per each, as their could be multiple hits
    //fix: ordering matter in detection, otherwise can give false results in rare cases
    if (cur_val.match(jcb_regex)) {
        sel_brand = "jcb";
    } else if (cur_val.match(amex_regex)) {
        sel_brand = "amex";
    } else if (cur_val.match(diners_regex)) {
        sel_brand = "diners_club";
    } else if (cur_val.match(visa_regex)) {
        sel_brand = "visa";
    } else if (cur_val.match(mastercard_regex)) {
        sel_brand = "mastercard";
    } else if (cur_val.match(discover_regex)) {
        sel_brand = "discover";
    } else if (cur_val.match(maestro_regex)) {
        if (cur_val[0] == '5') { //started 5 must be mastercard
            sel_brand = "mastercard";
        } else {
            sel_brand = "maestro"; //maestro is all 60-69 which is not something else, thats why this condition in the end
        }
    } else {
        sel_brand = "unknown";
    }

    return sel_brand;
}

//Check etheir term accepted or not by user 
function acceptTerms() {
    var rcb = $("#cbAcceptTerms");
    if (rcb.prop("checked") === true) {
        return true;
    } else if (rcb.prop("checked") === false) {
        rcb.focus();
        return false;
    }
}

// Initailize a Radio button click event on page load
function rbtntwoEvent() {
    $("input[name^='rbtn2']")[0].click();
}

// Validating the Radio button event on click
function rbtnEvent() {
    var btnValue = $("input[name='rbtnAmount']:checked").val();
    var divElm = $("div[id^='pnlRewardItem']");
    var PledgeA = 0;
    // $("#btnRadio1").data("amtPledge",btnValue);
    for (var i = 0; i < divElm.length; i++) {
        var contamt = $(divElm[i]).data("amt");
        var actStat = $(divElm[i]).data("act");
        var actH3Value = Number(contamt);

        if (btnValue === "Custom" && actStat === "True") {

            PledgeA = Number($("#txbCustomAmount").val());

            txtEvent();

            if (PledgeA > actH3Value || PledgeA === actH3Value) {
                $(divElm[i]).removeClass("inactive");

            }
            else if (actH3Value > PledgeA) {
                $(divElm[i]).addClass("inactive");
            }
        }
        else if (actStat === "True") {
            PledgeA = Number(btnValue);
            if (PledgeA > actH3Value || PledgeA === actH3Value) {
                $(divElm[i]).removeClass("inactive");

            }
            else if (actH3Value > PledgeA) {
                $(divElm[i]).addClass("inactive");
                //$(divElm[i]).children().eq(0).addClass("hide");
            }

        }
    }
}

//Binding data for review 
function bindReview() {

    // Review Panel Pledge and Reward : Pledge Amount
    $("#RevPledgeAmount").text(function () {
        var btnnValue = $("input[name='rbtnAmount']:checked").val();
        var PledgeAa = $("#txbCustomAmount").val();
        if (btnnValue === "Custom") {
            $("#hfldPledgeamt").val(PledgeAa);
            return "$" + PledgeAa;
        } else {
            $("#hfldPledgeamt").val(btnnValue);
            return "$" + btnnValue;
        }
    });

    //Review Panel Pledge and Reward :  Reward Amount
    $("#RevRewardAmount").text(function () {
        var rbtnValue = $("input[name='rbtn2']:checked").data("ramt");
        var RevRewardTitle = $("input[name='rbtn2']:checked").data("rtit");
        //console.log('[$("#RevRewardAmount").text(function ()]  rbtnValue: ' + rbtnValue);
        if (rbtnValue === 0) {
            return "None, I just want to help.";
        } else {
            return RevRewardTitle + "  " + rbtnValue;
        }
    });


    //Billing Address for review panel of Shipping and billing address
    $("#BillingAddress").text(function () {
        var txtval = $("#cphMainContent_txtBillingAddress1_txb").val();
        txtval += "," + $.trim($("#cphMainContent_txtBillingAddress2_txb").val());
        txtval += "," + $.trim($("#cphMainContent_txtBillingCity_txb").val());
        txtval += "," + $.trim($("#cphMainContent_txtBillingState_txb").val());
        txtval += "," + $.trim($("#cphMainContent_txtBillingZip_txb").val());

        return $.trim(txtval);
    });

    //Shipping Address for review panel of Shipping and billing address
    $("#ShippingAddress").text(function () {
        var txtval = $("#cphMainContent_txtShippingAddress1_txb").val();
        txtval += "," + $.trim($("#cphMainContent_txtShippingAddress2_txb").val());
        txtval += "," + $.trim($("#cphMainContent_txtShippingCity_txb").val());
        txtval += "," + $.trim($("#cphMainContent_txtShippingState_txb").val());
        txtval += "," + $.trim($("#cphMainContent_txtShippingZip_txb").val());

        return $.trim(txtval);
    });
}

//Vailidating Pledge amount for custom text 
function txtEvent() {
    var btnValue = $("input[name='rbtnAmount']:checked").val();
    var divElm = $("div[id^='pnlRewardItem']");
    var PledgeA = Number($("#txbCustomAmount").val());
    
    for (var i = 0; i < divElm.length; i++) {

        var contamt = $(divElm[i]).data("amt");
        var actStat = $(divElm[i]).data("act");
        var actH3Value = Number(contamt);

        if (btnValue === "Custom" && actStat === "True") {

            if (PledgeA > actH3Value || PledgeA === actH3Value) {
                $(divElm[i]).removeClass("inactive");

            }
            else if (actH3Value > PledgeA) {
                $(divElm[i]).addClass("inactive");

            }
        }
    }
}

