$(function () {
    try {
        function jsPhaseGuages() {
            $(".summaryProgressbar").each(function () {
                //Instantiate variables
                var game = { score: 0 };

                //$("#Phase1 .summaryPanel").each(function () {
                //Instantiate variables
                var marker = $(this).find('.pnlMarker');
                var innerPanel = $(this).find('.innerPanel');
                var innerPanelWidth = $(this).find("input[type=hidden]").val();
                var scoreDisplay = $(this).find('.score');

                if (innerPanelWidth > 100) {
                    TweenMax.fromTo(innerPanel, 1, { css: { width: "0px" } }, { css: { width: "100%" }, delay: .5 });
                    TweenMax.to(marker, 1, { delay: .5, left: "100%" }); //slide marker into position
                } else {
                    TweenMax.fromTo(innerPanel, 1, { css: { width: "0px" } }, { css: { width: innerPanelWidth + "%" }, delay: .5 });
                    TweenMax.to(marker, 1, { delay: .5, left: innerPanelWidth + "%" }); //slide marker into position
                }


                if (innerPanelWidth > 0) {
                    countUp(innerPanelWidth, scoreDisplay);
                };




                function countUp(innerPanelWidth, scoreDisplay) {
                    game.score = 0;
                    TweenMax.to(game, 1, {
                        score: "+=" + innerPanelWidth,
                        roundProps: "score",
                        onUpdate: updateHandler,
                        onUpdateParams: [scoreDisplay],
                        ease: Linear.easeNone,
                        delay: .5
                    });
                }
                function updateHandler(scoreDisplay) {
                    scoreDisplay.html(game.score + '%');
                }
            });
        };


        //Run only if element exists
        if ($('.summaryProgressbar').length > 0) { jsPhaseGuages(); }
    }
    catch (err) {
        console.log('ERROR: [jsPhaseGuages] ' + err.message + ' | ' + err);
    }
});