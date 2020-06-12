$(function () {
    try {
        function jsTrendingNow() {
            TweenMax.staggerFrom(".CampaignPanel", 1, { scale: 0.5, opacity: 0, delay: 0.5, ease: Elastic.easeOut.config(1, 0.5), force3D: true }, 0.15);
        }


        //Run only if element exists
        if ($('.trending').length > 0) { jsTrendingNow(); }
    }
    catch (err) {
        console.log('ERROR: [jsTrendingNow] ' + err.message + ' | ' + err);
    }
});