$(function () {
    try {
        function jsDaysRemaining() {
            //Instantiate array of controls
            var $daysRemainingControl = $('.daysRemainingControl');

            //Loop thru array
            $daysRemainingControl.each(function () {
                //Obtain percent value stored in hidden field
                var daysRemainingPercent = $(this).find('.hfldDaysRemainingPercent').val();

                //Parse percentage into an int [Note: an empty circl = 36 & a full circl = 255.  the following adjusts for discrepency.]
                var overflow = 36;
                var fullCircle = 255;
                var diff = fullCircle - overflow;
                var diffPercent = diff / 100;
                var equation = diffPercent * daysRemainingPercent;
                var progress = parseInt(equation + overflow);

                //Adjust progress circle's strokeDashoffset
                var progressCircle = $(this).find('.progress-circle');
                progressCircle.css('strokeDashoffset', progress);

                //Add text value to screen
                var daysRemaining = $(this).find('.hfldDaysRemaining').val();
                var txtDaysRemaining = $(this).find('.daysRemaining');
                txtDaysRemaining.text(daysRemaining);
            });
        }


        //Run only if element exists
        if ($('.daysRemainingControl').length > 0) { jsDaysRemaining(); }
    }
    catch (err) {
        console.log('ERROR: [jsDaysRemaining] ' + err.message + ' | ' + err);
    }
});