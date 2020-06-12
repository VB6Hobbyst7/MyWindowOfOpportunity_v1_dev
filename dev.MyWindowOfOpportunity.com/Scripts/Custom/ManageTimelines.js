//Manage timelines
//=================

//$(window).load(function () {
$(function () {
    try {
        function jsManageTimelines() {
            //Instantiate variables
            var $timelineBtns = $("ul.timelineFilter input[type=radio]");
            var $timelinePnls = $("div[type=timelineEntry]");

            //When button is clicked, show panel
            $timelineBtns.click(function () {
                //
                var hfld = $('.hfldNewTimelineEntry');
                //Hide all subcategory panels
                hideAll();

                //Show the save button
                var btnSave = $('.timeline .btnSave');
                btnSave.show();

                //Show matching panel
                var activePnl = $timelinePnls.filter("[data-nodeId='" + $(this).val() + "']");
                activePnl.show('fade', {
                    duration: 500,
                    easing: 'easeInOutQuart',
                });

                //If button's value = -1, set hidden field's value as 'true' for new entry
                if ($(this).val() == '-1') { hfld.val(true); }
                    else {hfld.val(false)};
            });

            //Auto select first item when page load
            //var rewardFirst = document.getElementsByName("rblTimelineBtn");
            //if (rewardFirst[1] != null) {
            //    rewardFirst[1].click();
            //}


            //Hide all panels
            function hideAll() {
                $timelinePnls.each(function () {
                    $(this).hide();
                });
            };
        }


        //Run only if element exists
        if ($('div[type=timelineEntry]').length > 0) { jsManageTimelines(); }
    }
    catch (err) {
        console.log('ERROR: [jsManageTimelines] ' + err.message + ' | ' + err);
    }
});