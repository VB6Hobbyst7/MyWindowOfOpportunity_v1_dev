$(function (e) {
    try {
        function jsCheckout() {
            //Instantiate variables
            var $LearnMore = $("#CheckoutPage input:radio.rbtn2");
            var $NextPage = $("#CheckoutPage .nextPg");
            var smartwizard = $('#smartwizard');
            var lbtnContinue = $(".lbtnContinue");
            var phaseLearnMore = $(".phaseLearnMore");
            var phaseDescription = $(".phaseDescription");
            var rbtnAmount = $("input[name='rbtnAmount']");
            var txbCustomAmount = $("#txbCustomAmount");
            var $organicTabs = $(".organicTabs");


            //On radiobutton clicked, show/hide buttons.
            $LearnMore.on('change', function () {
                //Hide all buttons
                $NextPage.each(function () {
                    $(this).hide();
                });
                //
                $NextPage.eq($LearnMore.index(this)).show();
            });

            //Activate organic tabs
            $organicTabs.organicTabs({ "speed": 200 });


            //Firing function on page load           
            rbtnEvent();
            rbtntwoEvent();
            bindReview();

            // Toolbar extra buttons
            var btnFinish = $('<button></button>').text('Finish')
                .addClass('btn btn-info')
                .on('click', function () { alert('Finish Clicked'); });

            var btnCancel = $('<button></button>').text('Cancel')
                .addClass('btn btn-danger')
                .on('click', function () { smartwizard.smartWizard("reset"); });

            smartwizard.smartWizard({
                selected: 0,
                theme: 'arrows',
                transitionEffect: 'fade',
                showStepURLhash: false
            });
            


            // Step leave method moved to Campaign Credit Card Validation Js

            // Additional Setting for Reward Panel at Runtime
            txbCustomAmount.on("keypress keyup blur", function (event) {
                $(this).val($(this).val().replace(/[^\d].+/, ""));
                if ((event.which < 48 || event.which > 57)) {
                    event.preventDefault();
                }
            });
            txbCustomAmount.change(function (event) {
                txtEvent();
            });
            rbtnAmount.change(function (event) {
                rbtntwoEvent()
                rbtnEvent();
            });

            //Button click on reward panel.
            lbtnContinue.on('click', function (event) {
                //Validate custom amount
                smartwizard.smartWizard("next");
                return false;
            });
            phaseLearnMore.on('click', function (e) {
                phaseDescription.toggle();
            });

        }


        //Run only if element exists
        if ($('#CheckoutPage').length > 0) { jsCheckout(); }
    }
    catch (err) {
        console.log('ERROR: [jsCheckout] ' + err.message + ' | ' + err);
    }
});