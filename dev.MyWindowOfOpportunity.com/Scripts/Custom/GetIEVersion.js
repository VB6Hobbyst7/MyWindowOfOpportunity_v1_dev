$(function () {
    try {

        function jsGetIEVersion() {
            var sAgent = window.navigator.userAgent;
            var Idx = sAgent.indexOf("MSIE");
            //   var ie10andbelow = navigator.userAgent.indexOf('MSIE') != -1
            // If IE, return version number.
            if (Idx > 0)
                return parseInt(sAgent.substring(Idx + 5, sAgent.indexOf(".", Idx)));

            // If IE 11 then look for Updated user agent string.
            //else if (!!navigator.userAgent.match(/Trident\/7\./))
            //    return 11;

            //else
            //    return 0; //It is not IE
        }


        if (jsGetIEVersion() < 9 || jsGetIEVersion() == 9)
            alert('Your browser version will not work properly with this site.  Please upgrade to a newer browser.')
    }
    catch (err) {
        console.log('ERROR: [jsGetIEVersion] ' + err.message + ' | ' + err);
    }
});