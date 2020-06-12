$(function () {
    try {
        function jsImageCropper() {
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
            var $viewports; 
            var slider;
            var prevWidth = 0;
            var prevHeight = 0;

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
                        if ($(this).attr("mediaid") == hfldCurrentMediaId.val()) { //Found correct panel to activate. 
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
                                            thisImg.width(Math.ceil(thisImgWidth + (thisImgWidth * $(this).attr('data-slider'))));
                                            thisImg.css('height', 'auto');
                                                                                        
                                            //Obtain objects
                                            activeCropPnl = pnlImageCropper.find(".cropPnl[cropAlias='" + activeCrop + "']");
                                            hfldViewportWidth = activeCropPnl.find('.hfldViewportWidth');
                                            hfldViewportHeight = activeCropPnl.find('.hfldViewportHeight');

                                            //Get image's positions (t/l)
                                            var pos = thisImg.position();
                                            
                                            //Ensure movement does not exceed the top position.
                                            if (pos.top > 0) { thisImg.css({ top: 0 }); }
                                            if (pos.left > 0) { thisImg.css({ left: 0 }); }
                                            
                                            //Adjust image's left position if shrinking image.
                                            if (prevWidth > thisImg.width()) {
                                                thisImg.css({ left: Math.ceil(hfldViewportWidth.val() - thisImg.width()) });
                                            }

                                            //Adjust image's top position if shrinking image.
                                            if (prevHeight > thisImg.height()) {
                                                thisImg.css({ top: Math.ceil(hfldViewportHeight.val() - thisImg.height()) });
                                            }
                                            
                                            //Capture current w/h for next loop's comparison
                                            prevWidth = thisImg.width();
                                            prevHeight = thisImg.height();                                            
                                        }
                                    });
                                }
                                catch (err) {
                                    console.log('Error: [$cropPnl.each] ' + err.message + ' | ' + err);
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
                    
                    //Set the starting point for the slider
                    var new_value = ((hfldImgWidth.val() / hfldViewportWidth.val()) - 1);
                    slider.foundation('slider', 'set_value', new_value);
                    
                    //ADJUST IMG TO SMALLEST SIZE.
                    if (activeImage.height() < parseInt(hfldViewportHeight.val())) {
                        activeImage.css('width', 'auto');
                        activeImage.height(parseInt(hfldViewportHeight.val()));
                    }

                    if (activeImage.width() < parseInt(hfldViewportWidth.val())) {
                        activeImage.css('height', 'auto');
                        activeImage.width(parseInt(hfldViewportWidth.val()));
                    }
                                        
                    hfldLeft.val(fromDatabase_left(hfldViewportWidth.val(), hfldX1.val(), hfldX2.val()));
                    hfldTop.val(fromDatabase_top(hfldViewportHeight.val(), hfldY1.val(), hfldY2.val()));

                    activeImage.css("left", hfldLeft.val() + "px");
                    activeImage.css("top", hfldTop.val() + "px");
                    
                    //Set previous values
                    prevWidth = activeImage.width();
                    prevHeight = activeImage.height();

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
                    
                    //Ensure movement does not exceed the top position.
                    var pos = activeImage.position();
                    if (pos.top > 0) { activeImage.css({ top: 0 }); }
                    if (pos.left > 0) { activeImage.css({ left: 0 }); }
                    
                    hfldLeft.val(Math.ceil(pos.left));
                    hfldTop.val(Math.ceil(pos.top));
                    
                    activeImage.css({ left: Math.ceil(pos.left) });
                    activeImage.css({ top: Math.ceil(pos.top) });
                    
                    //Calculate values for database from image stats.
                    hfldX1.val(toDatabase_X1(hfldLeft.val(), hfldImgWidth.val()));
                    hfldY1.val(toDatabase_Y1(hfldTop.val(), hfldImgHeight.val()));
                    hfldX2.val(toDatabase_X2(hfldLeft.val(), hfldImgWidth.val(), hfldViewportWidth.val()));
                    hfldY2.val(toDatabase_Y2(hfldTop.val(), hfldImgHeight.val(), hfldViewportHeight.val()));
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
            
            //UPLOAD FUNCTIONALITY
            $(function () {
                Dropzone.autoDiscover = false;

                var myDropzone = $("div.dropzone");
                var hfldMediaFolder = $("#hfldMediaFolder");
                var btnRefreshPage = $("#btnRefreshPage");

                //Upload image using Dropzone.js
                myDropzone.dropzone({
                    url: '/Services/ImageUploadHandler.ashx?mediaId=' + hfldMediaFolder.val(),
                    dictDefaultMessage: '',
                    previewContainer: '#preview', //this will change preview to element selected
                    maxFiles: 1,
                    maxfilesexceeded: function (file) {
                        this.removeAllFiles();
                        this.addFile(file);
                    },
                    queuecomplete: function (file) {
                        btnRefreshPage.trigger("click");
                    }
                });
            });

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
                var btnDeleteImage = $('#btnDeleteImage');
                var hfldMediaIdToDelete = $('#hfldMediaIdToDelete');

                //Save mediaId in hidden field
                hfldMediaIdToDelete.val(mediaId);

                //Trigger button to delete image
                btnDeleteImage.trigger("click");

                //console.log('Yes clicked. Deleting media Id: ' + mediaId);
            }

        }


        //Run only if element exists
        if ($('#editCampaign').length > 0) { jsImageCropper(); }
        else if ($('#teamPg').length > 0) { jsImageCropper(); }
    }
    catch (err) {
        console.log('ERROR: [jsImageCropper] ' + err.message + ' | ' + err);
    }
});
