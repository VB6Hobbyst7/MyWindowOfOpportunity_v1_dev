//==============================================================
//      CONTENT IMAGE SELECTOR
//==============================================================
$(function () {
    try {
        function jsContentImgSelector() {
            //Instantiate variables
            var $bannerOptions_Content = $('.bannerOptions li');
            var selectImageBtn_Content = $('.selectImageBtn');
            var propertyName;
            var $hfldSelectedPropertyName = $('.hfldSelectedPropertyName');
            var cancelBtn_Content = $(' .pnlSelectFeaturedImage .cancelBtn');
            var $deleteBtns = $('.pnlContentManagement .pnlSelectedImage .delete');
            var pnlSelectedImage_Content = $('.pnlContentManagement .pnlSelectedImage');
            //var hfldRemoveSelectedImg_Content = $('.pnlContentManagement .pnlSelectedImage .hfldRemoveSelectedImg');

            //Toggle panels
            selectImageBtn_Content.click(function () {
                //Obtain the clicked btn's property name
                propertyName = $(this).next('.hfldPropertyName').val();
                //Set property name to all the following hidden fields for the server-side clicked control to obtain
                $hfldSelectedPropertyName.val(propertyName);
                //Show featured images
                toggleFeaturedImgPanels();
            });
            cancelBtn_Content.click(function () {
                toggleFeaturedImgPanels();
            });
            function toggleFeaturedImgPanels() {
                try {
                    //Instantiate variables
                    var pnlSelectFeaturedImage = $('.pnlSelectFeaturedImage');
                    var pnlContentManagement = $('.pnlContentManagement');

                    //
                    pnlContentManagement.toggle();
                    pnlSelectFeaturedImage.toggle();
                }
                catch (err) {
                    console.log('ERROR: ' + err.message + ' | ' + err);
                }
            };

            //When a thumbnail is clicked, save id to hidden field and trigger button click to server.
            $bannerOptions_Content.click(function () {
                try {
                    //console.log('$bannerOptions_Content.click: ' + propertyName);
                    //Instantiate variables
                    var hfldSelectedMediaId = $('.hiddenFields .hfldSelectedMediaId');
                    var btnSelectBanner = $('.hiddenFields .btnSelectBanner');

                    //Assign clicked mediaId to hidden field
                    hfldSelectedMediaId.val($(this).attr('mediaid'));

                    //Click button to trigger server function.
                    btnSelectBanner.trigger("click");
                }
                catch (err) {
                    console.log('ERROR: ' + err.message + ' | ' + err);
                }

            });

            //
            $deleteBtns.click(function () {
                //Set value for hidden field for deletion if/when saved.
                $(this).next('.hfldRemoveSelectedImg').val('true');

                //Hide img panel
                $(this).parent().toggle();
            });
            //console.log($deleteBtns.length);
            //console.log(pnlSelectedImage_Content.length);
        }


        //Run only if element exists
        if ($('.selectImageBtn').length > 0) { jsContentImgSelector(); }
    }
    catch (err) {
        console.log('ERROR: [jsContentImgSelector] ' + err.message + ' | ' + err);
    }
});



//==============================================================
//      REWARD IMAGE SELECTOR
//==============================================================
$(function () {
    try {
        function jsRewardImgSelector() {
            //Instantiate variables
            var minorImageSelector_Rewards_SelectPnl = $('.rewards .imgSelectPnl');
            var minorImageSelector_Rewards_EntryPnls = $('.rewards .entryPnls');
            var $minorImageSelector_Rewards_DisplayPnl = $('.rewards .minorImageSelector .displayPnl');
            var selectImageBtn_Reward = $('.rewards .selectImageBtn');
            var hfldActiveRewardNodeId = $('.hfldActiveRewardNodeId');
            var cancelBtn_Reward = $('.rewards .imgSelectPnl .cancelBtn');
            var pnlSelectedImage_Reward;
            var selectedImage_Reward;
            var deleteBtn_Reward;
            var hfldMediaId_Reward;
            var $rewardBtns = $("ul.rewardFilter input[type=radio]");
            var $rewardPnls = $("div[type=rewardEntry]");
            var $imagesToSelectFrom = minorImageSelector_Rewards_SelectPnl.find('.imagesToSelectFrom li');

            //When category button is clicked, update variables
            $rewardBtns.click(function () {
                //Obtain active panel
                var activePnl = $rewardPnls.filter("[nodeId='" + $(this).val() + "']");

                //Get clicked button's sibling image, btn and hidden field
                hfldMediaId_Reward = activePnl.find(".hfldMediaId");
                pnlSelectedImage_Reward = activePnl.find(".pnlSelectedImage");
                selectedImage_Reward = pnlSelectedImage_Reward.find(".selectedImage");
                deleteBtn_Reward = pnlSelectedImage_Reward.find(".delete");


                //Set deletion button's functionality
                deleteBtn_Reward.click(function () {
                    //Hide img panel
                    pnlSelectedImage_Reward.toggle();

                    //Set value for hidden field for deletion when saved.
                    hfldMediaId_Reward.val('');
                });


            });
            selectImageBtn_Reward.click(function () {
                //Toggle panels
                toggleRewardImgPanels();
            });
            cancelBtn_Reward.click(function () {
                //Clear variables
                selectedImage_Reward = null;
                deleteBtn_Reward = null;
                hfldMediaId_Reward = null;

                //Toggle panels
                toggleRewardImgPanels();
            });
            $imagesToSelectFrom.click(function () {
                //When a thumbnail is clicked, save id to hidden field and show in called panel.
                try {
                    //Assign clicked mediaId to hidden field and src to img
                    hfldMediaId_Reward.val($(this).attr('mediaid'));
                    selectedImage_Reward.attr('src', $(this).find('img').prop('src'));

                    //Toggle panels
                    toggleRewardImgPanels();

                    //Ensure image panel is being displayed.
                    pnlSelectedImage_Reward.show();
                }
                catch (err) {
                    console.log('ERROR: ' + err.message + ' | ' + err);
                }
            });

            //Toggle panels
            function toggleRewardImgPanels() {
                try {
                    //Toggle panels.
                    minorImageSelector_Rewards_SelectPnl.toggle();
                    minorImageSelector_Rewards_EntryPnls.toggle();
                }
                catch (err) {
                    console.log('ERROR: ' + err.message + ' | ' + err);
                }
            };
        }


        //Run only if element exists
        if ($('.rewards').length > 0) { jsRewardImgSelector(); }
    }
    catch (err) {
        console.log('ERROR: [jsRewardImgSelector] ' + err.message + ' | ' + err);
    }
});