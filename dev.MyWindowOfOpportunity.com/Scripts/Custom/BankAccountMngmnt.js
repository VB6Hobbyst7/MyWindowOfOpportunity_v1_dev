$(function () {
    try {
        function jsFlatpickr() {
            //==============================
            //=========Set dob calendar
            //==============================
                //Calculate youngest permitted date of birth (18 years old)
                var eighteenYearsAgo = new Date();
                eighteenYearsAgo.setDate(eighteenYearsAgo.getDate() - 6575);

                //Build dob calendar
                $(".flatpickr").flatpickr({
                    enableTime: false,
                    altInput: true,
                    altFormat: "F j, Y",
                    minDate: "1917-10-13",
                    maxDate: eighteenYearsAgo
                });

                //Set calendar's attributes.
                $('input.txbBankInfo_DateOfBirth.flatpickr-input.form-control').attr('required', 'true').removeAttr('readonly');
        }

        //Run only if element exists
        if ($('.flatpickr').length > 0) { jsFlatpickr(); }
    }
    catch (err) {
        console.log('ERROR: [jsFlatpickr] ' + err.message + ' | ' + err);
    }
});



//Submit Bank Account- Currently not being used.
//---------------------------------
$(function () {
    try {
        function jsSubmitBankInfo() {
            //Instantiate variables
            var spinner = $('.spinner');
            var btnCopyFromAcct = $('#btnCopyFromAcct');
            var btnSubmitBankInfo = $('#btnSubmitBankInfo');
            var hfldCurrentUserId = $("#hfldCurrentUserId");
            var hfldPublicKey = $("#hfldPublicKey");
            var hfldCurrentUserId_ParamName = $("#hfldCurrentUserId_ParamName");
            var hfldCampaignAcctId = $('#hfldCampaignAcctId');
            var hfldCampaignAcctId_ParamName = $("#hfldCampaignAcctId_ParamName");
            var hfldBankAcctToken = $('#hfldBankAcctToken');
            var hfldBankAcctId = $('#hfldBankAcctId');
            var hfldBankAcctId_ParamName = $("#hfldBankAcctId_ParamName");
            var hfldFileUploadId = $('#hfldFileUploadId');
            var myDropzone;
            var rblBankInfo_Country = $("#rblBankInfo_Country");
            var txbBankInfo_RoutingNo = $('#txbBankInfo_RoutingNo');
            var txbBankInfo_AcctNo = $('#txbBankInfo_AcctNo');
            var txbBankInfo_SSN = $('#txbBankInfo_SSN');
            var $country;
            var alertMsgs_Success = $('.alertMsgs .alertBoxRow.success');
            var alertMsgs_Info = $('.alertMsgs .alertBoxRow.info');
            var alertMsgs_Error = $('.alertMsgs .alertBoxRow.alert');

            //Ensure form does not submit a postback.
            $('form').attr('onsubmit', 'return false');


            //===============================
            //=========If acct exists, gather data
            //===============================
            isAcctCreated(true);
            function isAcctCreated(isLoad) {
                if (hfldCampaignAcctId.val() !== "") {
                    //Show Spinner
                    TweenMax.set(spinner, { autoAlpha: 1 });
                    //Call function
                    TweenMax.delayedCall(0, retrieveAcctData);

                    function retrieveAcctData() {
                        try {
                            //Instantiate variables
                            var Type = "POST";
                            var Url = "../Services/FinancialHandler.asmx/RetrieveAcctData_byId";
                            var Data;
                            var ContentType = "application/json; charset=utf-8";
                            var DataType = "json";
                            var ProcessData = true;

                            //Proceed if current user id exists
                            if (hfldCurrentUserId.val().length > 0) {
                                if (isLoad) {
                                    //Retrieve data from account and display it.
                                    CreateParameters();
                                    CallService();
                                    disableControls();
                                }

                                //Hide submit button
                                $('.btnSubmitPnl').hide();

                                //Hide spinner
                                TweenMax.set(spinner, { autoAlpha: 0 });
                            }

                            function CreateParameters() {
                                //Instantiate an array of parameters to pass to handler
                                var myData = { campaignAcctId: hfldCampaignAcctId.val() };

                                //Set array as json for use in ajax call
                                Data = JSON.stringify(myData);
                            }
                            function CallService() {
                                $.ajax({
                                    type: Type, //GET or POST or PUT or DELETE verb
                                    url: Url, // Location of the service
                                    data: Data, //Data sent to server
                                    contentType: ContentType, // content type sent to server
                                    dataType: DataType, //Expected data format from server
                                    processdata: ProcessData, //True or False
                                    success: function (msg) {//On Successfull service call
                                        ServiceSucceeded(msg);
                                    },
                                    error: function (msg) {
                                        ServiceFailed(msg);
                                    }
                                    //error: ServiceFailed
                                });
                            }
                            function ServiceFailed(result) {
                                //Show the alert box.
                                showErrorMsg('Service call failed: ' + result.status + ' ' + result.statusText + ' ' + result.responseText);
                                Type = null;
                                varUrl = null;
                                Data = null;
                                ContentType = null;
                                DataType = null;
                                ProcessData = null;

                                //Hide spinner
                                TweenMax.set(spinner, { autoAlpha: 0 });
                            }
                            function ServiceSucceeded(result) {
                                console.log('Account is already created.');
                                console.log(result.d);

                                //Set values from query to screen
                                if (result.d.Demographics.firstName) {
                                    $('#txbBankInfo_FirstName').val(result.d.Demographics.firstName);
                                    $('.txbBankInfo_FirstName').addClass('input--filled');
                                }
                                if (result.d.Demographics.lastName) {
                                    $('#txbBankInfo_LastName').val(result.d.Demographics.lastName);
                                    $('.txbBankInfo_LastName').addClass('input--filled');
                                }
                                if (result.d.MembershipProperties.email) {
                                    $('#txbBankInfo_Email').val(result.d.MembershipProperties.email);
                                    $('.txbBankInfo_Email').addClass('input--filled');
                                }
                                if (result.d.Demographics.dateOfBirth) {
                                    //Set date of birth value
                                    setDOBFlatpickr(result.d.Demographics.dateOfBirth);
                                }
                                if (result.d.BillingInfo.address01) {
                                    $('#txbBankInfo_Address1').val(result.d.BillingInfo.address01);
                                    $('.txbBankInfo_Address1').addClass('input--filled');
                                }
                                if (result.d.BillingInfo.address02) {
                                    $('#txbBankInfo_Address2').val(result.d.BillingInfo.address02);
                                    $('.txbBankInfo_Address2').addClass('input--filled');
                                }
                                if (result.d.BillingInfo.city) {
                                    $('#txbBankInfo_City').val(result.d.BillingInfo.city);
                                    $('.txbBankInfo_City').addClass('input--filled');
                                }
                                if (result.d.BillingInfo.stateProvidence) {
                                    $('#txbBankInfo_State').val(result.d.BillingInfo.stateProvidence);
                                    $('.txbBankInfo_State').addClass('input--filled');
                                }
                                if (result.d.BillingInfo.postalCode) {
                                    $('#txbBankInfo_Postal').val(result.d.BillingInfo.postalCode);
                                    $('.txbBankInfo_Postal').addClass('input--filled');
                                }

                                //Uncheck account's country location
                                $country = rblBankInfo_Country.find("input");
                                if ($country.length > 1) {
                                    //If only 1 entry, than value will always be US
                                    $country.prop('checked', false);
                                }

                                //Set checkbox to true
                                $('#cbAcceptTerms').prop("checked", true);

                                //Show the alert box.
                                alertMsgs_Info.toggle();
                            }
                            function disableControls() {
                                //Disable all controls on page.
                                myDropzone.disable();
                                $(".FinancialManagement").addClass('disabled');
                                $(".FinancialManagement input").prop("disabled", true);
                                $(".FinancialManagement button").prop("disabled", true);
                                $("div.pnlUpload.dropzone").addClass('disabled');
                            }
                        }
                        catch (err) {
                            //Show the alert box.
                            showErrorMsg('Error Copying Acct: ' + err.message + ' | ' + err);

                            //Hide spinner
                            TweenMax.set(spinner, { autoAlpha: 0 });
                        }
                    };
                }
            }



            //===============================
            //=========Dropzone Setup
            //===============================
            DropzoneSetup();
            function DropzoneSetup() {
                try {
                    //Prevents error with dropzone being assigned before it loads... i think.
                    Dropzone.autoDiscover = false;

                    //Instantiate dropzone
                    myDropzone = new Dropzone('#dropzone', {
                        url: "../Services/FinancialHandler.asmx/UploadFile",
                        autoProcessQueue: false,
                        previewContainer: null, //'#preview', //this will change preview to element selected
                        maxFiles: 1,
                        acceptedFiles: "image/*",
                        maxfilesexceeded: function (file) {
                            this.removeAllFiles();
                            this.addFile(file);
                        }
                    });

                    //Append variables before sending image
                    myDropzone.on('sending', function (file, xhr, formData) {
                        //Send parameters along with image.
                        formData.append(hfldCampaignAcctId_ParamName.val(), hfldCampaignAcctId.val());
                        formData.append(hfldCurrentUserId_ParamName.val(), hfldCurrentUserId.val());

                        console.log(hfldCampaignAcctId_ParamName.val() + ' | ' + hfldCampaignAcctId.val());
                        console.log(hfldCurrentUserId_ParamName.val() + ' | ' + hfldCurrentUserId.val());
                    });
                }
                catch (err) {
                    //Show the alert box.
                    showErrorMsg('Error instantiating dropzone: ' + err.message + ' | ' + err);
                }
            }



            //===============================
            //=========Button: Copy from Acct
            //===============================
            btnCopyFromAcct.click(function () {
                //Show Spinner
                TweenMax.set(spinner, { autoAlpha: 1 });
                //Call function
                TweenMax.delayedCall(0, copyFromAcct);

                function copyFromAcct() {
                    try {
                        //Instantiate variables
                        var Type = "POST";
                        var Url = "../Services/FinancialHandler.asmx/GetAcctData";
                        var Data;
                        var ContentType = "application/json; charset=utf-8";
                        var DataType = "json";
                        var ProcessData = true;

                        //Proceed if current user id exists
                        if (hfldCurrentUserId.val().length > 0) {
                            //Retrieve data from account and display it.
                            CreateParameters();
                            CallService();
                        }

                        function CreateParameters() {
                            //Instantiate an array of parameters to pass to handler
                            var myData = { userId: hfldCurrentUserId.val() }; //"{Id": "' + userid + '"}";

                            //Set array as json for use in ajax call
                            Data = JSON.stringify(myData);
                        }
                        function CallService() {
                            $.ajax({
                                type: Type, //GET or POST or PUT or DELETE verb
                                url: Url, // Location of the service
                                data: Data, //Data sent to server
                                contentType: ContentType, // content type sent to server
                                dataType: DataType, //Expected data format from server
                                processdata: ProcessData, //True or False
                                success: function (msg) {//On Successfull service call
                                    ServiceSucceeded(msg);
                                },
                                error: function (msg) {
                                    ServiceFailed(msg);
                                }
                                //error: ServiceFailed
                            });
                        }
                        function ServiceFailed(result) {
                            //Show alert box.
                            showErrorMsg('Service call failed: ' + result.status + ' ' + result.statusText + ' ' + result.responseText);
                            Type = null;
                            varUrl = null;
                            Data = null;
                            ContentType = null;
                            DataType = null;
                            ProcessData = null;

                            //Hide spinner
                            TweenMax.set(spinner, { autoAlpha: 0 });
                        }
                        function ServiceSucceeded(result) {
                            console.log(result.d);

                            //Set values from query to screen
                            if (result.d.Demographics.firstName) {
                                $('#txbBankInfo_FirstName').val(result.d.Demographics.firstName);
                                $('.txbBankInfo_FirstName').addClass('input--filled');
                            }
                            if (result.d.Demographics.lastName) {
                                $('#txbBankInfo_LastName').val(result.d.Demographics.lastName);
                                $('.txbBankInfo_LastName').addClass('input--filled');
                            }
                            if (result.d.MembershipProperties.email) {
                                $('#txbBankInfo_Email').val(result.d.MembershipProperties.email);
                                $('.txbBankInfo_Email').addClass('input--filled');
                            }



                            if (result.d.BillingInfo.address01) {
                                $('#txbBankInfo_Address1').val(result.d.BillingInfo.address01);
                                $('.txbBankInfo_Address1').addClass('input--filled');
                            }
                            if (result.d.BillingInfo.address02) {
                                $('#txbBankInfo_Address2').val(result.d.BillingInfo.address02);
                                $('.txbBankInfo_Address2').addClass('input--filled');
                            }
                            if (result.d.BillingInfo.city) {
                                $('#txbBankInfo_City').val(result.d.BillingInfo.city);
                                $('.txbBankInfo_City').addClass('input--filled');
                            }
                            if (result.d.BillingInfo.stateProvidence) {
                                $('#txbBankInfo_State').val(result.d.BillingInfo.stateProvidence);
                                $('.txbBankInfo_State').addClass('input--filled');
                            }
                            if (result.d.BillingInfo.postalCode) {
                                $('#txbBankInfo_Postal').val(result.d.BillingInfo.postalCode);
                                $('.txbBankInfo_Postal').addClass('input--filled');
                            }

                            //Hide spinner
                            TweenMax.set(spinner, { autoAlpha: 0 });
                        }
                    }
                    catch (err) {
                        //Show alert box.
                        showErrorMsg('Error Copying Acct: ' + err.message + ' | ' + err);

                        //Hide spinner
                        TweenMax.set(spinner, { autoAlpha: 0 });
                    }
                };
            });


            //===============================
            //=========Button: Submit Bank Acct Info
            //===============================
            btnSubmitBankInfo.click(function () {
                if ($('form')[0].checkValidity() == true) {

                    if (myDropzone.getQueuedFiles().length > 0) {
                        //Zebra Dialgo | http://stefangabos.ro/jquery/zebra-dialog/
                        $.Zebra_Dialog('The following action will register your bank account to receive monetary donations for your campaigns.<br /><br />Once verified, you will not be able to edit your account information online, but will need to contact us to make edits.<br><br><strong>Confirm that all is correct.</strong>', {
                            'type': 'confirmation', //confirmation, information, warning, error and question
                            'title': 'Is All Information Correct?',
                            'buttons': [
                                            {
                                                caption: 'Yes', callback: function () {
                                                    //Show Spinner
                                                    TweenMax.set(spinner, { autoAlpha: 1 });
                                                    //Call function
                                                    TweenMax.delayedCall(0, submitBankInfo);
                                                }
                                            },
                                            { caption: 'No' }
                            ]
                        });

                    }
                    else {
                        //Image is required.  add class to show it's required.
                        $('.pnlUpload').addClass('required');

                        //Hide spinner
                        TweenMax.set(spinner, { autoAlpha: 0 });
                    }

                }
            });
            function submitBankInfo() {
                try {
                    //Remove requirement class for panel
                    $('.pnlUpload').removeClass('required');

                    //Obtain selected country information
                    $country = rblBankInfo_Country.find("input:checked").val().split('|');

                    //Instantiate variables
                    var Type = "POST";
                    var Url = "../Services/FinancialHandler.asmx/CreateStripeAcct";
                    var Data;
                    var ContentType = "application/json; charset=utf-8";
                    var DataType = "json";
                    var ProcessData = true;

                    //Proceed if current user id exists
                    if (hfldCurrentUserId.val().length > 0) {
                        //Retrieve data from account and display it.
                        CreateParameters();
                        CallService();
                    }

                    function CreateParameters() {
                        //Instantiate an array of parameters to pass to handler
                        var myData = {};
                        myData.currentUserId = $.trim(hfldCurrentUserId.val());
                        myData.firstName = $.trim($('#txbBankInfo_FirstName').val());
                        myData.lastName = $.trim($('#txbBankInfo_LastName').val());
                        myData.email = $.trim($('#txbBankInfo_Email').val());
                        myData.dob = $.trim($('#txbBankInfo_DateOfBirth').val());
                        myData.address01 = $.trim($('#txbBankInfo_Address1').val());
                        myData.address02 = $.trim($('#txbBankInfo_Address2').val());
                        myData.city = $.trim($('#txbBankInfo_City').val());
                        myData.state = $.trim($('#txbBankInfo_State').val());
                        myData.postalCode = $.trim($('#txbBankInfo_Postal').val());
                        myData.country = $country[0];
                        myData.campaignAcctId = $.trim(hfldCampaignAcctId.val());
                        myData.bankAcctToken = $.trim(hfldBankAcctToken.val());
                        myData.bankAcctId = $.trim(hfldBankAcctId.val());
                        myData.fileUploadId = $.trim(hfldFileUploadId.val());
                        myData.ssn = $.trim(txbBankInfo_SSN.val());

                        //Set array as json for use in ajax call
                        Data = JSON.stringify(myData);
                    }
                    function CallService() {
                        $.ajax({
                            type: Type, //GET or POST or PUT or DELETE verb
                            url: Url, // Location of the service
                            data: Data, //Data sent to server
                            contentType: ContentType, // content type sent to server
                            dataType: DataType, //Expected data format from server
                            processdata: ProcessData, //True or False
                            success: function (msg) {//On Successfull service call
                                ServiceSucceeded(msg);
                            },
                            error: function (msg) {
                                ServiceFailed(msg);
                            }
                            //error: ServiceFailed
                        });
                    }
                    function ServiceFailed(result) {
                        //Show alert box.
                        showErrorMsg('Service call failed: ' + result.status + ' ' + result.statusText + ' ' + result.responseText);
                        Type = null;
                        varUrl = null;
                        Data = null;
                        ContentType = null;
                        DataType = null;
                        ProcessData = null;

                        //Hide spinner
                        TweenMax.set(spinner, { autoAlpha: 0 });
                    }
                    function ServiceSucceeded(result) {
                        //Check if any error messages has occured.
                        if (result.d.errorMsg !== "") {
                            //Show alert box.
                            showErrorMsg(result.d.errorMsg);

                            //Hide spinner
                            TweenMax.set(spinner, { autoAlpha: 0 });
                        }
                        else {
                            //Set values from query to screen
                            if (result.d.StripeIDs.campaignAcctId !== "") {
                                hfldCampaignAcctId.val(result.d.StripeIDs.campaignAcctId);
                                //Determine which button is to be displayed.
                                console.log('submitBankInfo() | ServiceSucceeded');
                                isAcctCreated(false);
                            }

                            //Upload photo id
                            UploadPhotoId();
                        }
                    }


                    //==============================
                    //=========Build Dropzone functionality
                    //=========Attach ID to account
                    //==============================
                    //Upload Photo Id
                    function UploadPhotoId() {
                        //Submit queued images to server
                        myDropzone.processQueue();
                    };


                    ////Append variables before sending image
                    //myDropzone.on('sending', function (file, xhr, formData) {
                    //    //Send parameters along with image.
                    //    formData.append(hfldCampaignAcctId_ParamName.val(), hfldCampaignAcctId.val());
                    //    formData.append(hfldCurrentUserId_ParamName.val(), hfldCurrentUserId.val());

                    //    console.log(hfldCampaignAcctId_ParamName.val() + ' | ' + hfldCampaignAcctId.val());
                    //    console.log(hfldCurrentUserId_ParamName.val() + ' | ' + hfldCurrentUserId.val());
                    //});

                    //If uploading image fails, show error in console.
                    myDropzone.on('error', function (file, response) {
                        console.log('Error Uploading Image:');
                        console.log(file);
                        console.log(response);
                        showErrorMsg('Error Uploading Image.  ' + response);
                        //Hide spinner
                        TweenMax.set(spinner, { autoAlpha: 0 });
                    });

                    //If successfull, save rest of data.
                    myDropzone.on('success', function (file, response) {
                        //Convert xml to json
                        var x2js = new X2JS();
                        var jsonObj = x2js.xml_str2json(response);
                        console.log('Photo uploaded successfully:');
                        console.log(jsonObj);

                        //Check if any error messages has occured.
                        if (jsonObj.Member.errorMsg !== "") {
                            //Show alert box.
                            showErrorMsg(jsonObj.Member.errorMsg);

                            //Hide spinner
                            TweenMax.set(spinner, { autoAlpha: 0 });
                        }
                        else {
                            //Set values from query to screen
                            if (jsonObj.Member.StripeIDs.fileUploadId !== "") {
                                hfldFileUploadId.val(jsonObj.Member.StripeIDs.fileUploadId);
                            }

                            //Submit bank acct information
                            SubmitBankingInfo();
                        }
                    });


                    //==============================
                    //=========Submit bank data to account
                    //==============================
                    function SubmitBankingInfo() {
                        //Set the publishable key
                        Stripe.setPublishableKey(hfldPublicKey.val());

                        //Disable button to prevent double-payments.
                        //btnSubmitFinancials.prop('disabled', true);

                        //Creaet bank account token
                        Stripe.bankAccount.createToken({
                            country: $country[0],  //GET FROM DROPDOWN
                            currency: $country[1],  //GET FROM DROPDOWN
                            routing_number: $.trim(txbBankInfo_RoutingNo.val()),
                            account_number: $.trim(txbBankInfo_AcctNo.val()),
                            account_holder_name: $.trim($('#txbBankInfo_FirstName').val()) + ' ' + $.trim($('#txbBankInfo_LastName').val()),
                            account_holder_type: 'individual'
                        }, stripeBankAcctResponseHandler);
                    };
                    function stripeBankAcctResponseHandler(status, response) {
                        console.log('status: ' + status);

                        if (response.error) {
                            // Show the errors on the form
                            showErrorMsg('Error creating bank acct: ' + response.error.message);

                            //Hide spinner
                            TweenMax.set(spinner, { autoAlpha: 0 });
                        } else {
                            // response contains id and card, which contains additional card details
                            var token = response.id;
                            // Insert the token into the form so it gets submitted to the server
                            hfldBankAcctToken.val(token);

                            //Server call to convert token to a bank account id.
                            ConvertTokenToBankAcctId();
                        }
                    }


                    //==============================
                    //=========Submit bank data to account
                    //==============================
                    function ConvertTokenToBankAcctId() {
                        //Instantiate variables
                        var Type = "POST";
                        var Url = "../Services/FinancialHandler.asmx/ConvertTokenToBankAcctId";
                        var Data;
                        var ContentType = "application/json; charset=utf-8";
                        var DataType = "json";
                        var ProcessData = true;

                        //
                        console.log('Create Params');
                        CreateParameters();
                        console.log('Call Service');
                        CallService();
                        console.log('Complete');

                        function CreateParameters() {
                            //Instantiate an array of parameters to pass to handler
                            var myData = {};
                            myData.currentUserId = hfldCurrentUserId.val();
                            myData.campaignAcctId = hfldCampaignAcctId.val();
                            myData.bankAcctToken = hfldBankAcctToken.val();

                            //Set array as json for use in ajax call
                            Data = JSON.stringify(myData);
                        }
                        function CallService() {
                            $.ajax({
                                type: Type, //GET or POST or PUT or DELETE verb
                                url: Url, // Location of the service
                                data: Data, //Data sent to server
                                contentType: ContentType, // content type sent to server
                                dataType: DataType, //Expected data format from server
                                processdata: ProcessData, //True or False
                                success: function (msg) {//On Successfull service call
                                    ServiceSucceeded(msg);
                                },
                                error: function (msg) {
                                    ServiceFailed(msg);
                                }
                            });
                        }
                        function ServiceFailed(result) {
                            showErrorMsg('Service call failed: ' + result.status + ' ' + result.statusText + ' ' + result.responseText);
                            Type = null;
                            varUrl = null;
                            Data = null;
                            ContentType = null;
                            DataType = null;
                            ProcessData = null;

                            //Hide spinner
                            TweenMax.set(spinner, { autoAlpha: 0 });
                        }
                        function ServiceSucceeded(result) {
                            //Check if any error messages has occured.
                            if (result.d.errorMsg !== "") {
                                //Show alert box.
                                showErrorMsg(result.d.errorMsg);
                            }
                            else {
                                //Set values from query to screen
                                if (result.d.StripeIDs.bankAcctToken !== "") {
                                    hfldBankAcctToken.val(result.d.StripeIDs.bankAcctToken);
                                }
                                if (result.d.StripeIDs.bankAcctId !== "") {
                                    hfldBankAcctId.val(result.d.StripeIDs.bankAcctId);
                                }
                                //Show the success alert box.
                                alertMsgs_Success.toggle();
                            }

                            //Hide spinner
                            TweenMax.set(spinner, { autoAlpha: 0 });
                        }
                    };
                }
                catch (err) {
                    //Show alert box.
                    showErrorMsg(err.message);

                    //Hide spinner
                    TweenMax.set(spinner, { autoAlpha: 0 });
                }
            };




            function parseJsonDate(jsonDateString) {
                //Convert json date to jquery date
                var date = new Date(parseInt(jsonDateString.replace('/Date(', '')));
                return date;

                ////Other examples
                //var formatted = date.getFullYear() + "-" + ("0" + (date.getMonth() + 1)).slice(-2) + "-" + ("0" + date.getDate()).slice(-2);
                //var formatted = date.format("MMMM d, yyyy");
                //var formatted = date.format("yyyy-mm-dd");
            }
            function setDOBFlatpickr(jsonDateString) {
                //Calculate youngest permitted date of birth (18 years old)
                var eighteenYearsAgo = new Date();
                eighteenYearsAgo.setDate(eighteenYearsAgo.getDate() - 6575);

                //Build dob calendar
                $(".flatpickr").flatpickr({
                    clickOpens: false,
                    disable: [],
                    enableTime: false,
                    altInput: true,
                    altFormat: "F j, Y",
                    minDate: "1917-10-13",
                    maxDate: eighteenYearsAgo,
                    defaultDate: parseJsonDate(jsonDateString)
                });
                //Set sliding panel as active
                $('.pnlBankInfo_DateOfBirth').addClass('input--filled');
                //Set calendar's attributes.
                $('input.txbBankInfo_DateOfBirth.flatpickr-input.form-control').attr('required', 'true').removeAttr('readonly');
                //Make calendar appear as disabled
                $('input.flatpickr.form-control').css("background-color", "#ddd");
                $('input.flatpickr.form-control').prop("disabled", true);
            }
            function showErrorMsg(msg) {
                //Show alert box.
                alertMsgs_Error.toggle();
                alertMsgs_Error.find('.additionalText').html(msg)
            }
        }


        //Run only if element exists
        if ($('#btnSubmitBankInfo').length > 0) { jsSubmitBankInfo(); }
    }
    catch (err) {
        console.log('ERROR: [jsSubmitBankInfo] ' + err.message + ' | ' + err);
    }
});










//$(function () {
    
//});
