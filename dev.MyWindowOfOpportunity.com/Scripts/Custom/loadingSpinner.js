$(function () {
    try {
        function jsSpinner() {
            //Instantiate variables
            var spinner = $('.spinner');
            TweenMax.set(spinner, { autoAlpha: 1 }); 
            
            //Hide spinner after page is fully loaded.
            function BeginRequestHandler(sender, args) {
                TweenMax.set(spinner, { autoAlpha: 1 }); //spinner.show();
            }
            function EndRequestHandler(sender, args) {
                TweenMax.set(spinner, { autoAlpha: 0 }); //spinner.hide();
            }
            Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
            $(window).bind('beforeunload', function () {
                TweenMax.set(spinner, { autoAlpha: 1 }); //spinner.show();
            });
            $(window).unload(function () {
                TweenMax.set(spinner, { autoAlpha: 1 }); //spinner.show();
            });
            $(window).load(function () {
                TweenMax.set(spinner, { autoAlpha: 0 }); 
            });
        }
        
        //Run only if element exists
        if ($('.spinner').length > 0) { jsSpinner(); }
    }
    catch (err) {
        console.log('ERROR: [jsSpinner] ' + err.message + ' | ' + err);
    }
});