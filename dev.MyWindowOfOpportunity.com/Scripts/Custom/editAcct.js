$(function () {
    try {
        function jsEditAcct() {
            //Instantiate variables
            var $tabs = $('#editAccount .organicTabs .nav li');
            var lbtnUpdateAcct = $('.lbtnUpdateAcct');
            var $navs = $('#editAccount ul.nav');
            var $1stPnl = $('#editAccount .list-wrap ul').first();

            //Show/hide link button
            $tabs.click(function () {
                if ($(this).data("hideupdatebtn")) {
                    lbtnUpdateAcct.hide();
                }
                else {
                    lbtnUpdateAcct.show();
                }
            });
                        
            //Ensure the 1st tabs and panels are active.
            $navs.each(function () {
                var tab = $(this).find("li a").first();
                tab.addClass("current");
            });
            $1stPnl.show();
            $1stPnl.removeClass('hide');            
        }
        
        //Run only if element exists
        if ($('#editAccount').length > 0) { jsEditAcct(); }
    }
    catch (err) {
        console.log('ERROR: [jsEditAcct] ' + err.message + ' | ' + err);
    }
});