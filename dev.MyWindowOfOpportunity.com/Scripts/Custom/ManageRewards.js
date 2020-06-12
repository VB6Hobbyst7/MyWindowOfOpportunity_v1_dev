//Manage rewards
//==================================


//$(window).load(function () {
$(function () {
    try {
        function jsManageRewards() {
            //Instantiate variables
            var $rewardBtns = $("ul.rewardFilter input[type=radio]");
            var $rewardPnls = $("div[type=rewardEntry]");
            var hfldActiveRewardNodeId = $('.hfldActiveRewardNodeId');

            //When category button is clicked, show subcategories
            $rewardBtns.click(function () {
                //Hide all subcategory panels
                hideAll();

                //Show the save button
                var btnSave = $('.rewards .btnSave');
                btnSave.show();

                //Show matching subcategory panel
                var activePnl = $rewardPnls.filter("[nodeId='" + $(this).val() + "']");
                activePnl.show('fade', {
                    duration: 500,
                    easing: 'easeInOutQuart',
                });

                //If button's value = -1, set hidden field's value as 'true' for new entry
                if ($(this).val() == '-1') {
                    //console.log('set to true');
                    var hfld = $('.hfldNewRewardEntry');
                    hfld.val(true);
                };

                //Store active node id value for use by other functions.
                hfldActiveRewardNodeId.val($(this).val());
            });

            //Auto select first item when page load
            var rewardFirst = document.getElementsByName("rblRewardBtn");
            if (rewardFirst[1] != null) {
                rewardFirst[1].click();
            }

            //Hide all subcategory panels
            function hideAll() {
                $rewardPnls.each(function () {
                    $(this).hide();
                });
            };
        }


        //Run only if element exists
        if ($('div[type=rewardEntry]').length > 0) { jsManageRewards(); }
    }
    catch (err) {
        console.log('ERROR: [jsManageRewards] ' + err.message + ' | ' + err);
    }
});