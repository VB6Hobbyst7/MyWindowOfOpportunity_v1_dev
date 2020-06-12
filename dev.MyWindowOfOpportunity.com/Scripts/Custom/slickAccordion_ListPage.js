//Set the 'Slick' scrolling functionality. [http://kenwheeler.github.io/slick/]
$(function () {
    try {
        function jsSlickAccordion() {
            $('.accordion').on('toggled', function (event, accordion) {
                //On toggle, set accordion to slick if not set already.
                slickIt($(this));

                //Force equalizer to refresh itself.
                $(document).foundation('equalizer', 'reflow');

                function slickIt(accordion) {
                    accordion.find('.regular').not('.slick-initialized').slick({
                        dots: false,
                        infinite: false,
                        speed: 300,
                        slidesToShow: 6,
                        slidesToScroll: 6,
                        responsive: [
                            {
                                breakpoint: 1440,
                                settings: {
                                    slidesToShow: 5,
                                    slidesToScroll: 5,
                                    infinite: false,
                                    dots: true
                                }
                            },
                            {
                                breakpoint: 1200,
                                settings: {
                                    slidesToShow: 4,
                                    slidesToScroll: 4,
                                    infinite: false,
                                    dots: true
                                }
                            },
                            {
                                breakpoint: 1025,
                                settings: {
                                    slidesToShow: 3,
                                    slidesToScroll: 3,
                                    infinite: false,
                                    dots: true
                                }
                            },
                            {
                                breakpoint: 850,
                                settings: {
                                    slidesToShow: 2,
                                    slidesToScroll: 2
                                }
                            },
                            {
                                breakpoint: 640,
                                settings: {
                                    slidesToShow: 1,
                                    slidesToScroll: 1
                                }
                            }
                            // You can unslick at a given breakpoint now by adding:
                            // settings: "unslick"
                            // instead of a settings object
                        ]
                    });
                };
            });
        }

        function jsAccordion() {
            var speed = 400;
            //var $accordionPnls = $('li.accordion-navigation');
            var $accordionTabs = $('li.accordion-navigation > a');
            var $accordionContents = $('.accordion .content');

            $accordionTabs.click(function (event) {
                var li = $(this).prev('li');
                var content = $(this).find('div.content');
                var activeContent = $(this).find('div.content.active');

                if (li.hasClass('active')) {
                    //close clicked accordion.
                    content.slideToggle(speed);
                }
                else {
                    //close any open accordions and toggle clicked accordion
                    //$(".accordion li div.content:visible").slideToggle(speed);
                    $(".accordion .content.active").slideToggle(speed);
                    $accordionContents.removeClass('active');
                    content.slideToggle(speed);
                }

                //
                $accordionContents.removeAttr("style");
            })
        }


        //function jsAccordion() {
        //    console.log('jsAccordion');
        //    var speed = 400;
        //    var $accordionPnls = $('li.accordion-navigation');
        //    //var $accordionTabs = $('li.accordion-navigation > a');
        //    //var $accordionPnlLinks = $('li.accordion-navigation > a.handle');
        //    //var $accordionContent = $('li.accordion-navigation > div.content');

        //    $accordionPnls.click(function (event) {
        //        console.log('$accordionPnls.click');
        //        var li = $(this);
        //        var content = $(this).find('div.content');

        //        if (li.hasClass('active')) {
        //            //close clicked accordion.
        //            content.slideToggle(speed);
        //        }
        //        else {
        //            //close any open accordions and toggle clicked accordion
        //            $(".accordion li div.content:visible").slideToggle(speed);
        //            content.slideToggle(speed);
        //        }
        //    })
        //}

        function showCampaign(ratingDivId) {
            console.log('showCampaign');
            $('#' + ratingDivId).find('.rating-border').show();
        }
        function hideCampaign(ratingDivId) {
            console.log('hideCampaign');
            $('#' + ratingDivId).find('.rating-border').hide();
        }



        //Run only if element exists
        if ($('.accordion').length > 0) { jsAccordion(); }
        if ($('.listPg').length > 0) { jsSlickAccordion(); }
    }
    catch (err) {
        console.log('ERROR: [jsSlickAccordion] ' + err.message + ' | ' + err);
    }
});