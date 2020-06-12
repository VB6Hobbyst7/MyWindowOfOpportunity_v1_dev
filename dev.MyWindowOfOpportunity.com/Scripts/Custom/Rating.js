function RatingValidation() {
    try {
        //Instantiate variables
        var isLoggedIn = $('#hdnIsMemberLoggedIn').val();
        var ratingValue = $('#hdnRating').val(); //Obtain rating value from stars

        //Is member logged in?
        if (isLoggedIn === "True") {
            //Determine if a value was selected or not
            if (ratingValue == 0) {
                //No value was selected.  Show message to user
                $('#pnlRate1st').show();
            }
            else {
                //Hide message
                $('#pnlRate1st').hide();
                //Save rating value in hidden field
                $('#hdnRating').val(ratingValue);
                //Trigger button to submit review
                var btnSubRating = $('#btnSubRating');
                btnSubRating.trigger("click");
            }
        }
        else {
            //Inform user to log in first
            $('#pnlLogin').show();
        }
    }
    catch (err) {
        console.log('ERROR: [RatingValidation] ' + err.message + ' | ' + err);
    }

}


$(function () {
    try {
        function jsRateThisCampaign() {
            var isReadOnly = $('#hdnIsMemberAlreadyGivenRating').val();
            var isRatingReadOnly = false;
            if (isReadOnly.trim().toLowerCase() == "false") {
                isRatingReadOnly = false;
            }
            else {
                isRatingReadOnly = true;
            }

            //Show message stating need to log in if not already.
            if ($('#hdnIsMemberLoggedIn').val() === "False") { $('#pnlLogin').show(); }

            $("#rateYo").rateYo({
                "rating": $("#hdnRating").val(),
                readOnly: isRatingReadOnly,
                fullStar: true,
                multiColor: {
                    "startColor": "#ff0000",
                    "endColor": "#f2d340"
                },
                "starSvg": "<svg xmlns=http://www.w3.org/2000/svg width='24' height='24' viewBox='0 0 24 24'>" +
                "<path d='M12 6.76l1.379 4.246h4.465l-3.612 2.625 1.379 4.246-3.611-2.625-3.612 2.625 1.379-4.246-3.612-2.625h4.465l1.38-4.246zm0-6.472l-2.833 8.718h-9.167l7.416 5.389-2.833 8.718 7.417-5.388 7.416 5.388-2.833-8.718 7.417-5.389h-9.167l-2.833-8.718z'></path>" +
                "</svg>",
                onInit: function (rating, rateYoInstance) {
                    $("#hdnRating").val(rating);
                },
                onSet: function (rating, rateYoInstance) {
                    $("#hdnRating").val(rating);
                },
            });

            $('#h4LearnMore').click(function () {
                if ($(".rating-descriptiondiv").css('display') == 'block') {
                    $('.rating-descriptiondiv').slideUp("slow");
                    $('#h4LearnMore').text('Learn More');
                }
                else {
                    $('#h4LearnMore').text('Collapse');
                    $('.rating-descriptiondiv').slideDown("slow");
                }
            });
        }


        //Run only if element exists
        if ($('#rateYo').length > 0) { jsRateThisCampaign(); }
    }
    catch (err) {
        console.log('ERROR: [jsRateThisCampaign] ' + err.message + ' | ' + err);
    }
});