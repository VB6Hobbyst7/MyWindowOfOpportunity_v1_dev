$(function () {
    try {
        function jsManageUserPhoto() {
            //Instantiate variables
            var pnlImageManagement = $('.pnlImageManagement');
            var imageListAndUploadPnl = pnlImageManagement.find('.imageListAndUploadPnl');
            var $btnSelectImg = pnlImageManagement.find('.btnSelectImg');
            var $pnlImageCropper = pnlImageManagement.find('.pnlImageCropper');
            var pnlImageCropper;
            var $closeCropPnl;
            var $cropBtns;
            var cancelBtn;
            var originalImagePnl;
            var $cropPnl;
            var activeCrop;
            var activeCropPnl;
            var activeImage;
            var activeViewport;
            var $viewports; // = $('.cropPnl .viewport');
            var slider;


            //Hidden fields
            var hfldCurrentMediaId = pnlImageManagement.find('#hfldCurrentMediaId');
            var hfldViewportWidth;
            var hfldViewportHeight;
            var hfldLeft;
            var hfldTop;
            var hfldImgWidth;
            var hfldImgHeight;
            var hfldCropWidthy;
            var hfldCropHeight;
            var hfldX1;
            var hfldY1;
            var hfldX2;
            var hfldY2;
            //var lblCropWidth;
            //var lblCropHeight;
            //var lblX1;
            //var lblY1;
            //var lblX2;
            //var lblY2;


            //TEMPORARY: DISPLAY VALUES IN HIDDEN FIELDS
            //var lblViewportWidth;
            //var lblViewportHeight;
            //var lblLeft;
            //var lblTop;
            //var lblImgWidth;
            //var lblImgHeight;



            //When a thumbnail is clicked, open the related cropping panel.
            $btnSelectImg.click(function () {
                try {
                    //Assign clicked mediaId to hidden field
                    hfldCurrentMediaId.val($(this).attr('mediaid'));

                    //Hide all ImageCropper panels, image lists and upload panels
                    imageListAndUploadPnl.toggle();
                    $pnlImageCropper.addClass('hide');

                    //Loop thru cropper panels and activate the panel that has a matching mediaId
                    $pnlImageCropper.each(function () {
                        //
                        if ($(this).attr("mediaid") == hfldCurrentMediaId.val()) {
                            //Found correct panel to activate. 
                            //Obtain all child properties that will be needed
                            pnlImageCropper = $(this);
                            $closeCropPnl = pnlImageCropper.find('i.closeCropPnl');
                            $cropBtns = pnlImageCropper.find('ul.cropBtnList li.crop');
                            cancelBtn = pnlImageCropper.find('.cancelBtn');
                            originalImagePnl = pnlImageCropper.find('.cropPnl.fullImg');
                            $cropPnl = pnlImageCropper.find('.cropPnl:not(.fullImg)');
                            $viewports = $cropPnl.find('.viewport');

                            //Make full image active
                            originalImagePnl.addClass('active');

                            //Make visible
                            pnlImageCropper.toggle();
                            //Scroll to anchor at top of page.
                            ScrollToAnchor('topOfImgManagement');

                            //====ADD DATABASE VALUES HERE???========//
                            //Set panel's image croppers to default values (or database values if present)
                            $cropPnl.each(function () {
                                try {
                                    //Instantiate variables
                                    var viewport = $(this).find('.viewport');
                                    var mainImg = viewport.find('.mainImg');
                                    slider = $(this).find('.range-slider');
                                    //var thisSlider = $(this).find('.range-slider');

                                    //Adjust image size and location to defaults
                                    mainImg.width(viewport.width());
                                    mainImg.css({ 'height': 'auto', 'top': '0', 'left': '0' });
                                    //mainImg.width(viewport.width() - 40);
                                    //mainImg.css({ 'height': 'auto', 'top': '20px', 'left': '20px' });

                                    //Set the starting point for the slider
                                    var new_value = 0;
                                    slider.foundation('slider', 'set_value', new_value);

                                    //Set slider to adjust image when moved.
                                    slider.on('change.fndtn.slider', function () {
                                        //Only run if parent crop panel is active
                                        var parentPnl = $(this).parents('.cropPnl');
                                        if (parentPnl.hasClass('active')) {
                                            //Instantiate variables
                                            var thisImg = $(this).parents('.cropper.editor').find('.mainImg'); //get image associated with slider
                                            var thisViewport = $(this).parents('.cropper.editor').find('.viewport'); //get viewport associated with slider

                                            //Get viewport width
                                            //var thisImgWidth = thisViewport.width() - 40;
                                            var thisImgWidth = thisViewport.width();

                                            //Adjust image width accordion to slider value.
                                            thisImg.width(thisImgWidth + (thisImgWidth * $(this).attr('data-slider')) + "px");

                                            //Can also Try:
                                            //http://greensock.com/forums/topic/8480-scale-rotate-and-opacity/
                                            //https://ihatetomatoes.net/wp-content/uploads/2015/08/GreenSock-Cheatsheet-2.pdf
                                        }
                                    });

                                    ////Do initial update to image to reflect data from database.
                                    //InitialUpdate();
                                }
                                catch (err) {
                                    console.log('Error: ' + err.message + ' | ' + err);
                                }
                            });

                            //When a crop button is clicked, open related crop panel
                            $cropBtns.click(function () {
                                try {
                                    //Remove active class from all buttons and panels
                                    $cropBtns.removeClass('active');
                                    $cropPnl.removeClass('active');
                                    originalImagePnl.removeClass('active');

                                    //Make clicked crop button and associated panel active
                                    $(this).addClass('active');
                                    activeCrop = $(this).attr('cropAlias');
                                    activeCropPnl = pnlImageCropper.find(".cropPnl[cropAlias='" + activeCrop + "']");
                                    activeCropPnl.addClass('active');

                                    //Obtain active viewport and image
                                    activeViewport = activeCropPnl.find('.viewport');
                                    activeImage = activeViewport.find('.mainImg');

                                    //Make crop's slider active
                                    slider = activeCropPnl.find('.range-slider');
                                    slider.foundation();
                                    //var thisSlider = activeCropPnl.find('.range-slider');
                                    //thisSlider.foundation();

                                    //Obtain hidden fields
                                    hfldViewportWidth = activeCropPnl.find('.hfldViewportWidth');
                                    hfldViewportHeight = activeCropPnl.find('.hfldViewportHeight');
                                    hfldLeft = activeCropPnl.find('.hfldLeft');
                                    hfldTop = activeCropPnl.find('.hfldTop');
                                    hfldImgWidth = activeCropPnl.find('.hfldImgWidth');
                                    hfldImgHeight = activeCropPnl.find('.hfldImgHeight');
                                    hfldCropWidthy = activeCropPnl.find('.hfldCropWidthy');
                                    hfldCropHeight = activeCropPnl.find('.hfldCropHeight');
                                    hfldX1 = activeCropPnl.find('.hfldX1');
                                    hfldY1 = activeCropPnl.find('.hfldY1');
                                    hfldX2 = activeCropPnl.find('.hfldX2');
                                    hfldY2 = activeCropPnl.find('.hfldY2');

                                    //
                                    hfldCropWidthy.val($(this).attr('cropwidth'));
                                    hfldCropHeight.val($(this).attr('cropheight'));

                                    //Do initial update to image to reflect data from database.
                                    InitialUpdate();

                                    ////TEMP: DISPLAY VALUES IN HIDDEN FIELDS
                                    //lblViewportWidth = activeCropPnl.find('.lblViewportWidth');
                                    //lblViewportHeight = activeCropPnl.find('.lblViewportHeight');
                                    //lblLeft = activeCropPnl.find('.lblLeft');
                                    //lblTop = activeCropPnl.find('.lblTop');
                                    //lblImgWidth = activeCropPnl.find('.lblImgWidth');
                                    //lblImgHeight = activeCropPnl.find('.lblImgHeight');
                                    //lblCropWidth = activeCropPnl.find('.lblCropWidth');
                                    //lblCropHeight = activeCropPnl.find('.lblCropHeight');
                                    //lblX1 = activeCropPnl.find('.lblX1');
                                    //lblY1 = activeCropPnl.find('.lblY1');
                                    //lblX2 = activeCropPnl.find('.lblX2');
                                    //lblY2 = activeCropPnl.find('.lblY2');

                                    //Do first initial update of crop panel data
                                    updateData()
                                }
                                catch (err) {
                                    console.log('ERROR: ' + err.message + ' | ' + err);
                                }
                            });

                            //Add function to close panels by its close button
                            $closeCropPnl.click(function () {
                                //Remove active class from all elements
                                $cropBtns.removeClass('active');
                                $cropPnl.removeClass('active');
                                //Make original image active
                                originalImagePnl.addClass('active');
                            });

                            //Cancel changes and close cropping panel.
                            cancelBtn.click(function () {
                                //Remove active class from this panel and show list of images
                                $cropBtns.removeClass('active');
                                $cropPnl.removeClass('active');
                                originalImagePnl.removeClass('active');
                                pnlImageCropper.toggle();
                                imageListAndUploadPnl.toggle();

                                //Remove id from hidden field
                                hfldCurrentMediaId.val('');

                                //Scroll to anchor
                                ScrollToAnchor('topOfImgManagement');
                            });

                            //
                            $viewports.each(function () {
                                var thisViewport = $(this);
                                var thisImg = thisViewport.find('.mainImg');

                                var draggables = Draggable.create(thisImg, {
                                    bounds: thisViewport,
                                    type: "top,left",
                                    trigger: thisViewport
                                });
                                //to get the first Draggable instance use
                                var myDraggable = draggables[0];
                                myDraggable.applyBounds(thisViewport);
                                //
                                myDraggable.addEventListener("release", updateData);





                                //TEMP... delete
                                //myDraggable.addEventListener("release", InitialUpdate);





                                //
                                //myDraggable.addEventListener("release", updateData);
                                //http://greensock.com/docs/#/HTML5/GSAP/Utils/Draggable/
                            });
                        }
                    });
                }
                catch (err) {
                    console.log('ERROR: ' + err.message + ' | ' + err);
                }
            })

            function InitialUpdate() {

                try {
                    //
                    hfldViewportWidth.val(activeViewport.width());
                    hfldViewportHeight.val(activeViewport.height());

                    hfldImgWidth.val(fromDatabase_ImgWidth(hfldViewportWidth.val(), hfldX1.val(), hfldX2.val()));
                    //hfldImgHeight.val(fromDatabase_ImgHeight(hfldViewportHeight.val(), hfldY1.val(), hfldY2.val()));


                    //Set the starting point for the slider
                    var new_value = ((hfldImgWidth.val() / hfldViewportWidth.val()) - 1);
                    slider.foundation('slider', 'set_value', new_value);


                    hfldLeft.val(fromDatabase_left(hfldViewportWidth.val(), hfldX1.val(), hfldX2.val()));
                    hfldTop.val(fromDatabase_top(hfldViewportHeight.val(), hfldY1.val(), hfldY2.val()));

                    activeImage.css("left", hfldLeft.val() + "px");
                    activeImage.css("top", hfldTop.val() + "px");

                    //console.log('-------' + activeCrop + '-------');
                    //console.log('hfldViewportWidth: ' + hfldViewportWidth.val());
                    //console.log('hfldViewportHeight: ' + hfldViewportHeight.val());
                    //console.log('hfldLeft: ' + hfldLeft.val());
                    //console.log('hfldTop: ' + hfldTop.val());
                    //console.log('hfldImgWidth: ' + hfldImgWidth.val());
                    //console.log('hfldImgHeight: ' + hfldImgHeight.val());
                    //console.log('hfldCropWidthy: ' + hfldCropWidthy.val());
                    //console.log('hfldCropHeight: ' + hfldCropHeight.val());
                    //console.log('hfldX1: ' + hfldX1.val());
                    //console.log('hfldY1: ' + hfldY1.val());
                    //console.log('hfldX2: ' + hfldX2.val());
                    //console.log('hfldY2: ' + hfldY2.val());
                    //console.log('slider: ' + new_value);
                    //console.log('-----------------------');
                }
                catch (err) {
                    console.log('ERROR: [InitialUpdate()] ' + err.message + ' | ' + err);
                }
            };

            function updateData() {
                try {
                    //Populate hidden fields with updated image stats
                    hfldImgWidth.val(activeImage.width());
                    hfldImgHeight.val(activeImage.height());
                    hfldViewportWidth.val(activeViewport.width());
                    hfldViewportHeight.val(activeViewport.height());
                    hfldLeft.val(activeImage.css('left'));
                    hfldTop.val(activeImage.css('top'));

                    //Calculate values for database from image stats.
                    hfldX1.val(toDatabase_X1(hfldLeft.val(), hfldImgWidth.val()));
                    hfldY1.val(toDatabase_Y1(hfldTop.val(), hfldImgHeight.val()));
                    hfldX2.val(toDatabase_X2(hfldLeft.val(), hfldImgWidth.val(), hfldViewportWidth.val()));
                    hfldY2.val(toDatabase_Y2(hfldTop.val(), hfldImgHeight.val(), hfldViewportHeight.val()));

                    ////
                    //toDatabase_lblCropWidth.text(cropPnl.attr('cropWidth'));
                    //toDatabase_lblCropHeight.text(cropPnl.attr('cropHeight'));
                    //toDatabase_lblX1.text(toDatabase_X1(fromBrowser_lblLeft.text(), fromBrowser_lblImgWidth.text()));
                    //toDatabase_lblY1.text(toDatabase_Y1(fromBrowser_lblTop.text(), fromBrowser_lblImgHeight.text()));
                    //toDatabase_lblX2.text(toDatabase_X2(fromBrowser_lblLeft.text(), fromBrowser_lblImgWidth.text(), fromBrowser_lblViewportWidth.text()));
                    //toDatabase_lblY2.text(toDatabase_Y2(fromBrowser_lblTop.text(), fromBrowser_lblImgHeight.text(), fromBrowser_lblViewportHeight.text()));

                    ////
                    //fromDatabase_lblViewportWidth.text(fromBrowser_lblViewportWidth.text());
                    //fromDatabase_lblViewportHeight.text(fromBrowser_lblViewportHeight.text());
                    //fromDatabase_lblLeft.text(fromDatabase_left(fromBrowser_lblViewportWidth.text(), toDatabase_lblX1.text(), toDatabase_lblX2.text()));
                    //fromDatabase_lblTop.text(fromDatabase_top(fromBrowser_lblViewportHeight.text(), toDatabase_lblY1.text(), toDatabase_lblY2.text()));
                    //fromDatabase_lblImgWidth.text(fromDatabase_ImgWidth(fromBrowser_lblViewportWidth.text(), toDatabase_lblX1.text(), toDatabase_lblX2.text()));
                    //fromDatabase_lblImgHeight.text(fromDatabase_ImgHeight(fromDatabase_lblViewportHeight.text(), toDatabase_lblY1.text(), toDatabase_lblY2.text()));

                    ////TEMP: Display values in hidden fields
                    //lblViewportWidth.text(hfldViewportWidth.val());
                    //lblViewportHeight.text(hfldViewportHeight.val());
                    //lblLeft.text(hfldLeft.val());
                    //lblTop.text(hfldTop.val());
                    //lblImgWidth.text(hfldImgWidth.val());
                    //lblImgHeight.text(hfldImgHeight.val());

                    //lblCropWidth.text(hfldCropWidthy.val());
                    //lblCropHeight.text(hfldCropHeight.val());
                    //lblX1.text(hfldX1.val());
                    //lblY1.text(hfldY1.val());
                    //lblX2.text(hfldX2.val());
                    //lblY2.text(hfldY2.val());

                }
                catch (err) {
                    console.log('ERROR: index ' + err.message + ' | ' + err);
                }
            };

            function toDatabase_X1(left, width) { //       (20 - SL)/SW  [or  ((SL – 20) * -1) / SW]
                //Instantiate variables
                //var _padding = 0;
                var _left = parseFloat(left);
                var _width = parseInt(width);
                //Calculate value
                //return ((_padding - _left) / _width);
                return ((_left * -1) / _width);
            };
            function toDatabase_Y1(top, height) { //       (20 - ST)/SH  [or  ((ST – 20)  * -1) / SH]
                //Instantiate variables
                //var _padding = 0;
                var _top = parseFloat(top);
                var _height = parseInt(height);
                //Calculate value
                //return ((_padding - _top) / _height);
                return ((_top * -1) / _height);
            };
            function toDatabase_X2(left, imgWidth, viewportWidth) { //       (SW - VW + SL - 20) / SW  [or  (SW – VW - ((SL – 20)  * -1)) / SW]
                //Instantiate variables
                //var _padding = 0;
                var _left = parseFloat(left);
                var _imgWidth = parseInt(imgWidth);
                var _viewportWidth = parseInt(viewportWidth);
                //Calculate value
                //return ((_imgWidth - _viewportWidth + _left - _padding) / _imgWidth);
                return ((_imgWidth - _viewportWidth + _left) / _imgWidth);
            };
            function toDatabase_Y2(top, imgHeight, viewportHeight) { //       (SH - VH + ST - 20) / SH  [or  (SH – VH - ((ST – 20)  * -1)) / SH]
                //Instantiate variables
                //var _padding = 0;
                var _top = parseFloat(top);
                var _imgHeight = parseInt(imgHeight);
                var _viewportHeight = parseInt(viewportHeight);
                //Calculate value
                //return ((_imgHeight - _viewportHeight + _top - _padding) / _imgHeight);
                return ((_imgHeight - _viewportHeight + _top) / _imgHeight);
            };


            function fromDatabase_left(viewportWidth, X1, X2) { //       -(((-VW*X1)-(20*X1)-(20*X2)+20)/(X1+X2-1))
                //Instantiate variables
                //var _padding = 0;
                var _viewportWidth = parseInt(viewportWidth);
                var _X1 = parseFloat(X1);
                var _X2 = parseFloat(X2);
                //Calculate value
                //return -(((-_viewportWidth * _X1) - (_padding * _X1) - (_padding * _X2) + _padding) / (_X1 + _X2 - 1));
                return -((-_viewportWidth * _X1) / (_X1 + _X2 - 1));
            };
            function fromDatabase_top(viewportHeight, Y1, Y2) { //       (VH*Y1)/(Y1 + Y2 - 1)
                //Instantiate variables
                //var _padding = 0;
                var _viewportHeight = parseInt(viewportHeight);
                var _Y1 = parseFloat(Y1);
                var _Y2 = parseFloat(Y2);
                //Calculate value
                //return (_viewportHeight * _Y1) / (_Y1 + _Y2 - 1);
                //return -(((-_viewportHeight * _Y1) - (_padding * _Y1) - (_padding * _Y2) + _padding) / (_Y1 + _Y2 - 1));
                return -((-_viewportHeight * _Y1) / (_Y1 + _Y2 - 1));
            };
            function fromDatabase_ImgWidth(viewportWidth, X1, X2) { //       -(VW/(X1+X2-1))
                //Instantiate variables
                //var _padding = 20;
                var _viewportWidth = parseInt(viewportWidth);
                var _X1 = parseFloat(X1);
                var _X2 = parseFloat(X2);
                //Calculate value
                return -(_viewportWidth / (_X1 + _X2 - 1));
            };
            function fromDatabase_ImgHeight(viewportHeight, Y1, Y2) { //       -(VH/(Y1+Y2-1))
                //Instantiate variables
                //var padding = 20;
                var _viewportHeight = parseInt(viewportHeight);
                var _Y1 = parseFloat(Y1);
                var _Y2 = parseFloat(Y2);
                //Calculate value
                return -(_viewportHeight / (_Y1 + _Y2 - 1));
            };



            //$(document).ready(function () {


            //UPLOAD FUNCTIONALITY
            //$(function () {
            //    try {
            Dropzone.autoDiscover = false;

            var myDropzone = $("div.profileImg.dropzone");
            var btnRefreshPage = $(".btnRefreshPage");
            var hfldMediaFolder = $("#hfldMediaFolder");
            var hfldCurrentUserId = $('.hfldCurrentUserId');
            var membersFolder = 1637;



            //Upload image using Dropzone.js
            myDropzone.dropzone({ //url: '/Services/ImageUploadHandler.ashx?mediaId=1637' + '&currentUserId=' + hfldCurrentUserId.val(),
                url: '/Services/ImageUploadHandler.ashx?mediaId=' + membersFolder + '&currentUserId=' + hfldCurrentUserId.val(), //url: '/Services/ImageUploadHandler.ashx?mediaId=' + hfldMediaFolder.val() + '&=currentUserId' + hfldCurrentUserId.val(),
                dictDefaultMessage: '',
                previewContainer: '#preview', //this will change preview to element selected
                maxFiles: 1,
                maxfilesexceeded: function (file) {
                    this.removeAllFiles();
                    this.addFile(file);
                },
                sending: function (file) {
                    //console.log('sending');
                },
                error: function (file, errormessage, xhr) {
                    console.log('error');
                    if (xhr) {
                        var response = JSON.parse(xhr.responseText);
                        //console.log(response.message);
                    }
                },
                queuecomplete: function (file) {
                    //alert('uploaded');
                    //console.log('Finished.  Used following veriables-- membersFolder: ' + membersFolder + ' | Current User Id: ' + hfldCurrentUserId.val());
                    btnRefreshPage.trigger("click");
                }
                //,
                //success: function (file) {
                //    console.log('success');
                //},
                //complete: function (file) {
                //    console.log('complete');
                //}
            });

            //    }
            //    catch (err) {
            //        console.log('ERROR: ' + err.message + ' | ' + err);
            //    }
            //});

            //CALLS DIALOG BOX BEFORE DELETING IMAGE
            var $trashcans = $(".pnlImageManagement .trashcan");
            $trashcans.click(function () {
                //Obtain media Id from clicked trashcan.
                var mediaId = $(this).attr("mediaId");

                //Zebra Dialgo | http://stefangabos.ro/jquery/zebra-dialog/
                $.Zebra_Dialog('The following action will permanently delete this image from your campaign.  <strong>Confirm Deletion.</strong>', {
                    'type': 'warning',
                    'title': 'Are You Sure?',
                    'buttons': [
                        { caption: 'Yes', callback: function () { callDeleteImageFunction(mediaId); } },
                        { caption: 'No' }
                    ]
                });
            });

            //TRIGGERS HIDDEN BUTTON THAT DELETES IMAGE
            function callDeleteImageFunction(mediaId) {
                //Instantiate variables
                var btnDeleteImage = $('.btnDeleteImage');
                var hfldMediaIdToDelete = $('#hfldMediaIdToDelete');

                //Save mediaId in hidden field
                hfldMediaIdToDelete.val(mediaId);

                //Trigger button to delete image
                btnDeleteImage.trigger("click");
                //console.log('btnDeleteImage: ' + btnDeleteImage.length);
                //console.log('Yes clicked. Deleting media Id: ' + mediaId);
            }
            //});
        }


        //Run only if element exists
        if ($('#editAccount').length > 0) { jsManageUserPhoto(); }
    }
    catch (err) {
        console.log('ERROR: [jsManageUserPhoto] ' + err.message + ' | ' + err);
    }
});