﻿$(function () {
    try {
        function jsEditAcctRatings() {
            $(".rating").each(function (i, v) {
                var $ratingDiv = $(v);

                $ratingDiv.find("div[id*='rateYo']").rateYo({
                    "rating": parseFloat($ratingDiv.find("input[type='hidden'][id*='hdnMemberRating']").val() || 0.0),
                    readOnly: true,
                    fullStar: true,
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
        if ($('#editAccount').length > 0) { jsEditAcctRatings(); }
    }
    catch (err) {
        console.log('ERROR: [jsEditAcctRatings] ' + err.message + ' | ' + err);
    }
});