//Manage timelines
//==================================

//$(window).load(function () {
$(function () {
    try {
        function jsManagePhases() {
            //Instantiate variables
            var $phaseBtns = $("ul.phaseFilter input[type=radio]");
            var $phasePnls = $("div[type=phaseEntry]");

            //Hide all subcategory panels
            hideAll();
            $phasePnls.eq(0).show();

            //When category button is clicked, show subcategories
            $phaseBtns.click(function () {
                //Hide all subcategory panels
                hideAll();

                //Show matching subcategory panel
                var activePnl = $phasePnls.filter("[nodeId='" + $(this).val() + "']");
                activePnl.show('fade', {
                    duration: 500,
                    easing: 'easeInOutQuart',
                });
            });


            //Hide all subcategory panels
            function hideAll() {
                $phasePnls.each(function () {
                    $(this).hide();
                });
            };
        }


        //Run only if element exists
        if ($('div[type=phaseEntry]').length > 0) { jsManagePhases(); }
    }
    catch (err) {
        console.log('ERROR: [jsManagePhases] ' + err.message + ' | ' + err);
    }
});