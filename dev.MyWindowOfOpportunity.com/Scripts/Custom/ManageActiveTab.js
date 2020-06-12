$(window).load(function () {
    //$(function () {
    try {
        function jsManageActiveTab() {
            //Obtain hidden field for active tab.
            var hfldActiveTab = $('.hiddenFields .hfldActiveTab');

            //If cookie exists, save value to hfld and delete cookie.  [TAB OVERRIDE]
            var tabCookie = Cookies.get('tab');
            if (typeof tabCookie !== "undefined") {
                //Split value
                var $activeTab = tabCookie.split("=")
                //Add value to hidden field
                hfldActiveTab.val($activeTab[1]);
                //Remove cookie
                Cookies.remove('tab');
            };

            //Obtain tab querystring if exists.
            var tab = GetQueryStringParams('tab');

            //If hidden field for tab has value then use it if no querystring tab override is used.
            if (typeof tab === "undefined") {
                if (hfldActiveTab.val()) { tab = hfldActiveTab.val(); };
            };

        

            //Set active tab.
            if (typeof tab !== "undefined") {
                //Instantiate variables
                var $tabs = $('.nav a');
                var currentTab = $('a[href="#' + tab + '"]');
                var $pnl = $('.list-wrap > ul');
                var currentPnl = $('#' + tab);

                //Clear all tabs and panels
                $tabs.removeClass('current');
                $pnl.hide();

                //Set active panel and tab
                currentTab.addClass('current');
                currentPnl.show();

                //Set value in hidden field
                hfldActiveTab.val(tab);



                //Open tab            
                //var tabParams = $('a[href="#' + tab + '"]');    //$('a[href="#phases"]');
                //tabParams.click();

                ////Obtain which entry to display
                //var entryId = GetQueryStringParams('entryId');
                ////Obtain selected panel
                //var pnl = $("#" + tab);
                ////Find all child checkboxes, if any exist, and select the one with the proper entryId and click it.
                //var cb = pnl.find("input[value='" + entryId + "']");
                //cb.prop("checked", true);
                //cb.click();                
            };




            //Obtains parameter from querystring in url.
            function GetQueryStringParams(sParam) {
                var sPageURL = window.location.search.substring(1);
                var sURLVariables = sPageURL.split('&');
                for (var i = 0; i < sURLVariables.length; i++) {
                    var sParameterName = sURLVariables[i].split('=');
                    if (sParameterName[0] == sParam) {
                        return sParameterName[1];
                    }
                }
            };
        }


        //Run only if element exists
        if ($('.hfldActiveTab').length > 0) { jsManageActiveTab(); }
    }
    catch (err) {
        console.log('ERROR: [jsManageActiveTab] ' + err.message + ' | ' + err);
    }
});