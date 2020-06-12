//==================================================
//  Pagination  
//==================================================
$(function () {
    try {
        function jsCampaignPagination() {
            // Instantiate variables
            var items = $(".uCampaigns_container li");
            var numItems = items.length;
            var perPage = 24;

            // Only show the first 2 (or first `per_page`) items initially.
            items.slice(perPage).hide();

            // Now setup the pagination using the `.pagination-page` div.
            $(".pagination-page").pagination({
                items: numItems,
                itemsOnPage: perPage,
                cssStyle: "light-theme",

                // This is the actual page changing functionality.
                onPageClick: function (pageNumber) {
                    // We need to show and hide `tr`s appropriately.
                    var showFrom = perPage * (pageNumber - 1);
                    var showTo = showFrom + perPage;

                    // We'll first hide everything...
                    items.hide()
                         // ... and then only show the appropriate rows.
                         .slice(showFrom, showTo).show();
                }
            });

            // EDIT: Let's cover URL fragments (i.e. #page-3 in the URL).
            // More thoroughly explained (including the regular expression) in: 
            // https://github.com/bilalakil/bin/tree/master/simplepagination/page-fragment/index.html

            // We'll create a function to check the URL fragment
            // and trigger a change of page accordingly.
            function checkFragment() {
                // If there's no hash, treat it like page 1.
                var hash = window.location.hash || "#page-1";

                // We'll use a regular expression to check the hash string.
                hash = hash.match(/^#page-(\d+)$/);

                if (hash) {
                    // The `selectPage` function is described in the documentation.
                    // We've captured the page number in a regex group: `(\d+)`.
                    $(".pagination-page").pagination("selectPage", parseInt(hash[1]));
                }
            };

            // We'll call this function whenever back/forward is pressed...
            $(window).bind("popstate", checkFragment);

            // ... and we'll also call it when the page has loaded
            // (which is right now).
            checkFragment();
        }


        //Run only if element exists
        if ($('.pagination-page').length > 0) { jsCampaignPagination(); }
    }
    catch (err) {
        console.log('ERROR: [jsCampaignPagination] ' + err.message + ' | ' + err);
    }
});



//jQuery(function ($) {});