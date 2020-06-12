$(function () {
    try {
        function jsAutoNumeric_Currency() {            
            //
            var $isCurrencyTxb = $('input[type="text"].isCurrency');
            $isCurrencyTxb.each(function () {
                //console.log('start');
                //
                $(this).val($(this).val().replace("$", ""));
                $(this).val($(this).val().replace(",", ""));

                //
                var options = { aSep: ',', aDec: '.', aSign: '$' };
                $(this).autoNumeric('init', options);
                //console.log('done');
            });
        }
        function jsAutoNumeric_Numeric() {
            //
            var $isNumericTxb = $('input[type="text"].isNumeric');
            $isNumericTxb.each(function () {
                //console.log('start');
                //
                //$(this).val($(this).val().replace("$", ""));
                //$(this).val($(this).val().replace(",", ""));

                //
                var options = { aSep: ',', aDec: null, aSign: null, wEmpty: 'zero', anDefault: '0', vMin: '0', mDec: '0' };
                $(this).autoNumeric('init', options);
                //console.log('done');
            });
        }


        //Run only if element exists
        if ($('.isCurrency').length > 0) { jsAutoNumeric_Currency(); }
        if ($('.isNumeric').length > 0) { jsAutoNumeric_Numeric(); }
    }
    catch (err) {
        console.log('ERROR: [jsAutoNumeric] ' + err.message + ' | ' + err);
    }
});