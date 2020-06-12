$(function () {
    try {
        function jsLoginPg() {
            //==============================
            //=========Button: Login
            //==============================        

            //Instantiate variables
            var btnLogin = $('#btnLogin');
            var btnSubmit = $('#btnSubmit');

            //Login btn clicked.  validate page and submit request.
            btnLogin.click(function (e) {
                if ($('form')[0].checkValidity() == true) {
                    //Prevent default submit
                    e.preventDefault();
                    //
                    var spinner = $('.spinner');
                    TweenMax.set(spinner, { autoAlpha: 1 });
                    //Click button to trigger server function.
                    btnSubmit.trigger("click");
                }
            });

            //var loginupdated = getParameterByName("loginupdated");
            //if (loginupdated) {
            //    alert('Updated Successfully!');
            //};

            //});
        }
        //Run only if element exists
        if ($('#loginPg').length > 0) { jsLoginPg(); }


        function jsLoginPanel() {
            TweenMax.set($("#loginPanel"), { zIndex: "0", display: "none", opacity: 0, delay: 0.1, scale: 1, ease: Power2.easeOut });
            $("#hlnkLogin").click(function () {
                //console.log("clicked");
                if ($("#loginPanel").css('opacity') == 0) {
                    TweenMax.to($("#loginPanel"), 0.3, { zIndex: "10", display: "block", opacity: 100, delay: 0.1, scale: 1, ease: Power2.easeOut });
                    $("#ucNavMinor_txbEmail").focus();
                }
                else {
                    TweenMax.to($("#loginPanel"), 0.3, { zIndex: "0", display: "none", opacity: 0, delay: 0.1, scale: 1, ease: Power2.easeOut });
                }
            });

            $("#ucNavMinor_txbEmail").bind('keypress', function (event) {
                if (event.keyCode == 13) {
                    javascript: __doPostBack('ctl00$ctl00$ucNavMinor$lbtnLogin', '');
                }
            });
            $("#ucNavMinor_txbPassword").bind('keypress', function (event) {
                if (event.keyCode == 13) {
                    javascript: __doPostBack('ctl00$ctl00$ucNavMinor$lbtnLogin', '');
                }
            });
        }
        //Run only if element exists
        if ($('#loginPanel').length > 0) { jsLoginPanel(); }
    }
    catch (err) {
        console.log('ERROR: [jsLogin] ' + err.message + ' | ' + err);
    }
});