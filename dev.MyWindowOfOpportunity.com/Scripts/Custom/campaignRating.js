$(function () {
    try {
        function jsCampaignRating() {
            $(".home-campaign-rating").each(function (i, v) {
                var $ratingDiv = $(v);
                var ratringArray = $ratingDiv.find("input[type='hidden'][id*='hdnCampaignRatingArray']").val().split(',').map(function (item) {
                    return parseInt(item, 10);
                });
                var count = ratringArray.length;
                var sum = 0;
                for (var j = 0; j < ratringArray.length; j++) {
                    sum += ratringArray[j] << 0;
                }
                var avg_Rating = (sum / count).toFixed(1);
                $ratingDiv.find("div[id*='avgrateCampaignYo']").rateYo({
                    "rating": avg_Rating,
                    starWidth: "24px",
                    starHeight: "auto",
                    readOnly: true,
                    fullStar: true,
                    spacing: "0px",
                    multiColor: {
                        "startColor": "#ff0000",
                        "endColor": "#f2d340"
                    },
                    "starSvg": "<svg xmlns=http://www.w3.org/2000/svg width='24' height='24' viewBox='0 0 24 24'>" +
                                 "<path d='M12 6.76l1.379 4.246h4.465l-3.612 2.625 1.379 4.246-3.611-2.625-3.612 2.625 1.379-4.246-3.612-2.625h4.465l1.38-4.246zm0-6.472l-2.833 8.718h-9.167l7.416 5.389-2.833 8.718 7.417-5.388 7.416 5.388-2.833-8.718 7.417-5.389h-9.167l-2.833-8.718z'></path>" +
                                 "</svg>"

                });

            });
        }


        //Run only if element exists
        if ($('.home-campaign-rating').length > 0) { jsCampaignRating(); }
    }
    catch (err) {
        console.log('ERROR: [jsCampaignRating] ' + err.message + ' | ' + err);
    }
});