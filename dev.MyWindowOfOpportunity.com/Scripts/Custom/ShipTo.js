$(function () {
    try {
        function jsShipTo() {
            //Instantiate variables
            var $shipmentDetailBtns = $('.shipmentDetails');
            var $shipToPnl = $('fieldset.shipTo');
            var $closeBtn = $('fieldset.shipTo .close');
            var $saveBtn = $('fieldset.shipTo .saveShipToBtn');
            var $lblStatus = $('.pledgeList .lblStatus');
                        
            //Open proper details tab
            $shipmentDetailBtns.click(function () {
                //Close all
                $shipToPnl.hide('slow');

                //Open pnl with matching id
                var pledgeid = $(this).data("pledgeid");                
                $shipToPnl.filter('[data-pledgeid=' + pledgeid + ']').show('slow');
            })

            //Close all detail tabs
            $closeBtn.click(function () {
                $shipToPnl.hide('slow');
            })

            //
            $saveBtn.click(function () {
                //
                var rewardId = $(this).data("rewardid");
                var pledgeid = $(this).data("pledgeid");
                
                //
                var shipToPnl = $shipToPnl.data("pledgeid", pledgeid);

                //
                if (shipToPnl.length > 0) {
                    //Instantiate variables
                    var cbFulfilled = shipToPnl.find('.cbFulfilled');
                    var txbTrackingNo = shipToPnl.find('.txbTrackingNo');
                    var txbNotes = shipToPnl.find('.txbNotes');
                    
                    var Type = "POST";
                    var Url = "/Services/FinancialHandler.asmx/SaveShippingDetails";
                    var Data = CreateParameters();
                    var ContentType = "application/json; charset=utf-8";
                    var DataType = "json";
                    var ProcessData = true;
                    
                    //console.log(Data);
                    CallService()
                                        
                    function CreateParameters() {
                        //Instantiate an array of parameters to pass to handler
                        var myData = {};
                        myData.pledgeId = pledgeid;
                        myData.rewardShipped = cbFulfilled.is(':checked');
                        myData.trackingNo = $.trim(txbTrackingNo.val());
                        myData.campaignMngrNotes = $.trim(txbNotes.val());

                        //Set array as json for use in ajax call
                        return JSON.stringify(myData);
                    }
                    function CallService() {
                        $.ajax({
                            type: Type,
                            url: Url,
                            data: Data,
                            contentType: ContentType,
                            dataType: DataType,
                            processdata: ProcessData,
                            success: function (msg) { ServiceSucceeded(msg); },
                            error: function (msg) { ServiceFailed(msg); }
                        });
                    }
                    function ServiceFailed(result) {
                        //Show alert box.

                        console.log('ERROR');
                        console.log(result.d);
                        console.log(result);
                        console.log(result.status);
                        console.log(result.statusText);
                        console.log(result.responseText);
                        //showErrorMsg('Service call failed: ' + result + ' ||| ' + result.status + ' ||| ' + result.statusText + ' ||| ' + result.responseText);
                        Type = null;
                        varUrl = null;
                        Data = null;
                        ContentType = null;
                        DataType = null;
                        ProcessData = null;

                    }
                    function ServiceSucceeded(result) {                        
                        if (result.d.isValid == true) {
                            //Update status text
                            var lblStatus = $lblStatus.data("pledgeid", pledgeid);

                            if (cbFulfilled.is(':checked') == true) {
                                lblStatus.text('Reward Shipped');
                            }
                            else {
                                lblStatus.text('Reward Pending');
                            }
                        }
                    }
                }
                
                //
                $shipToPnl.hide('slow');
                
            })

        }


        //Run only if element exists
        if ($('fieldset.shipTo').length > 0) { jsShipTo(); }
    }
    catch (err) {
        console.log('ERROR: [jsShipTo] ' + err.message + ' | ' + err);
    }
});