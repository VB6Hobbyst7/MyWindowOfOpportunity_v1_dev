//Set the 'Slick' scrolling functionality. [http://kenwheeler.github.io/slick/]
$(function () {
    try {
        function jsSlickTrending() {
            //Set to slick if not set already.
            slickIt($('.trending').eq(0));

            //Force equalizer to refresh itself.
            $(document).foundation('equalizer', 'reflow');

            function slickIt(trendingLst) {
                trendingLst.find('.slickSlider').not('.slick-initialized').slick({
                    dots: false,
                    infinite: false,
                    speed: 300,
                    slidesToShow: 5,
                    slidesToScroll: 5,
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

        }

        
        //Run only if element exists
        //if ($('.trending').length > 0) { jsSlickTrending(); }
    }
    catch (err) {
        console.log('ERROR: [jsSlickTrending] ' + err.message + ' | ' + err);
    }
});