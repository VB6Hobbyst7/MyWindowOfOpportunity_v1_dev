//Manage categories and subcategories
//===================================

//$(window).load(function () {
$(function () {
    try {
        function jsManageSubcategories() {
            //Instantiate variables
            var $categoryBtns = $("ul.categoryFilter input[type=radio]");
            var $subcategoryPnls = $("div[type=Subcategory]");


            //When category button is clicked, show subcategories
            $categoryBtns.click(function () {
                //Hide all subcategory panels
                hideAll();

                //Show matching subcategory panel
                var activePnl = $subcategoryPnls.filter("[category='" + $(this).val() + "']");
                activePnl.show('fade', {
                    duration: 500,
                    easing: 'easeInOutQuart',
                });
            });


            //Hide all subcategory panels
            function hideAll() {
                $subcategoryPnls.each(function () {
                    $(this).hide();
                });
            };
        }

        //Run only if element exists
        if ($('ul.categoryFilter').length > 0) { jsManageSubcategories(); }
    }
    catch (err) {
        console.log('ERROR: [jsManageSubcategories] ' + err.message + ' | ' + err);
    }
});