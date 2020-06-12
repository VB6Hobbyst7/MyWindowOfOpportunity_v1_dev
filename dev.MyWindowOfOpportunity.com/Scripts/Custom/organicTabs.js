$(function () {
    try {
        function jsOrganicTabs() {
            //
            $(".organicTabs").organicTabs({ "speed": 200 });
            $(".accordion.defaultClosed").accordion({ collapsible: true, active: false });


            $(".organicTabs .nav a").click(function () {
                //Instantiate variables
                var duration = 1000;
                var listWrap = $(".list-wrap");

                //CUSTOM CODE ADDED 2016-04-01 BY JF
                //Delay removing style from element.
                setTimeout(function () { //this code will run after the duration elaspes
                    listWrap.css("height", ""); //Remove the height style after size animation of organic tab is complete. (This will allow resizing by other controls inside panel.)
                    $(document).foundation('equalizer', 'reflow');
                }, duration);

                //Obtain hidden field for active tab and assign clicked tab name to it.
                var hfldActiveTab = $('.hiddenFields .hfldActiveTab');
                hfldActiveTab.val(($(this).attr('href')).replace('#', ''));
            });
        }

        //Run only if element exists
        if ($('.organicTabs').length > 0) { jsOrganicTabs(); }
    }
    catch (err) {
        console.log('ERROR: [jsOrganicTabs] ' + err.message + ' | ' + err);
    }
});