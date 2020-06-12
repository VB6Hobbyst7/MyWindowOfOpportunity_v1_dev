$(function () {
    try {
        function jsAlertBox() {
            //Instantiate variables
            var $alertBoxRow = $('.alertBoxRow');


            //Loop thru 
            $alertBoxRow.each(function () {
                //
                var alertBox = $(this);
                var closeBtn = alertBox.find('.close');

                //
                closeBtn.click(function () {
                    alertBox.hide();
                });
            });
        }


        //Run only if element exists
        if ($('.alertBoxRow').length > 0) { jsAlertBox(); }
    }
    catch (err) {
        console.log('ERROR: [jsAlertBox] ' + err.message + ' | ' + err);
    }
});