$(function () {
    try {
        function jsTransitionLogo() {
            var logoImg01 = $("#LogoPanel .logoImg01");
            var logoImg02 = $("#LogoPanel .logoImg02");
            var logoImg03 = $("#LogoPanel .logoImg03");
            var logoImg04 = $("#LogoPanel .logoImg04");
            var logoImg05 = $("#LogoPanel .logoImg05");

            TweenLite.set(logoImg01, { autoAlpha: "1" });

            var tl = new TimelineMax({ repeat: -1, repeatDelay: 3 }); //delay:1, 
            tl.to(logoImg02, .25, { autoAlpha: "1" }, 0);

            tl.to(logoImg01, .25, { autoAlpha: "0" }, .25);
            tl.to(logoImg03, .25, { autoAlpha: "1" }, .25);

            tl.to(logoImg02, .25, { autoAlpha: "0" }, .5);
            tl.to(logoImg04, .25, { autoAlpha: "1" }, .5);

            tl.to(logoImg03, .25, { autoAlpha: "0" }, .75);
            tl.to(logoImg05, .25, { autoAlpha: "1" }, .75);

            tl.to(logoImg04, .25, { autoAlpha: "0" }, 1.0);
            tl.to(logoImg01, .25, { autoAlpha: "1" }, 1.0);

            tl.to(logoImg05, .25, { autoAlpha: "0" }, 1.25);
        }


        //Run only if element exists
        if ($('#LogoPanel').length > 0) { jsTransitionLogo(); }
    }
    catch (err) {
        console.log('ERROR: [jsTransitionLogo] ' + err.message + ' | ' + err);
    }
});