//$(window).load(function () {
$(function () {
    try {
        function jsSearchboxFocus() {
            //When searchbox has focus, clear text and set color.
            $(".searchTextbox").focus(function () {
                if ($(this).val().trim() == 'Search Campaigns') {
                    $(this).val('');
                    $(this).css('color', '#484848');
                }
            });
            //When searchbox looses focus, adjust text as needed.
            $(".searchTextbox").blur(function () {
                if ($(this).val().trim() == '') {
                    $(this).val('Search Campaigns');
                    $(this).css('color', 'rgb(181, 181, 181);');
                }
            });


            //When searchbox has focus, clear text and set color.
            $("#txbStayConnectedEmail").focus(function () {
                if ($(this).val().trim() == 'Enter Email Address') {
                    $(this).val('');
                    $(this).css('font-weight', 'bold');
                }
            });
            //When searchbox looses focus, adjust text as needed.
            $("#txbStayConnectedEmail").blur(function () {
                if ($(this).val().trim() == '') {
                    $(this).val('Enter Email Address');
                    $(this).css('font-weight', 'normal');
                }
            });
        }


        //Run only if element exists
        if ($('.searchTextbox').length > 0) { jsSearchboxFocus(); }
    }
    catch (err) {
        console.log('ERROR: [jsSearchboxFocus] ' + err.message + ' | ' + err);
    }
});