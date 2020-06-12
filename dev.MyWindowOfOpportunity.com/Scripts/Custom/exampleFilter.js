$(function () {
    try {
        function jsFilterAnimation() {
            //Instantiate variables
            var filterIcon = $('#imgFilterIcon');
            var filterBtn = $('#parentFieldsetLegend');
            var parentFieldset = $('#fstParent');
            var fieldsetContainer = $('#fieldsetContainer');
            var channelFieldset = $('#fstChannel');
            var industryFieldset = $('#fstIndustry');
            var filterOpen = false;
            var filterHeight = parentFieldset.height();
            var fieldsetHeightAdjuster = 40;

            //Set filter as closed
            TweenMax.set(parentFieldset, { height: "0" });

            //image clicked.
            filterBtn.click(function () {
                //
                if (filterOpen == false) {
                    //Open the filter panel
                    TweenMax.to(filterIcon, 1, { rotation: "+=405_cw" });
                    TweenMax.to(parentFieldset, 1, { height: (filterHeight + fieldsetHeightAdjuster) + "px", borderColor: "#ddd" });
                    TweenMax.to(fieldsetContainer, .75, { autoAlpha: "1", delay: .25 });
                    TweenMax.set(parentFieldset, { height: "inherit", delay: 1 }); //force height to exact accurate size
                    filterOpen = true;
                }
                else {
                    //Close the filter panel
                    filterHeight = parentFieldset.height();
                    TweenMax.fromTo(parentFieldset, 1, { height: filterHeight + fieldsetHeightAdjuster + "px" }, { height: 0 });
                    TweenMax.to(parentFieldset, 1, { borderColor: "#fff" });
                    TweenMax.to(filterIcon, 1, { rotation: "-=405_ccw" });
                    TweenMax.to(fieldsetContainer, 1, { autoAlpha: "0" });
                    filterOpen = false;
                };
            });
        }


        //Run only if element exists
        if ($('#parentFieldsetLegend').length > 0) { jsFilterAnimation(); }
    }
    catch (err) {
        console.log('ERROR: [jsFilterAnimation] ' + err.message + ' | ' + err);
    }
});

$(function () {
    try {
        function jsPortfolioFiltering() {

            //Instantiate variables
            var filters = {};
            var optionSets = $(".option-set");

            //Add classes to first of each checkbox in filters
            optionSets.each(function () {
                var $cb = $(this).find('input');
                $cb.first().addClass("all");
            });

            //Temp function: add hardcoded values to checkboxes
            //tempFunction_AddValuesToCb()
            //function tempFunction_AddValuesToCb() {
            //    //Obtain all checkboxes
            //    var $cb0 = optionSets.eq(0).find('input');
            //    var $cb1 = optionSets.eq(1).find('input');

            //    //Add values to checkboxes: Channels
            //    $cb0.eq(0).val('');
            //    $cb0.eq(1).val('.Strategic');
            //    $cb0.eq(2).val('.Advertising');
            //    $cb0.eq(3).val('.IntegratedCampaigns');
            //    $cb0.eq(4).val('.SearchEngineMarketing');
            //    $cb0.eq(5).val('.SocialPR');
            //    $cb0.eq(6).val('.CARS');
            //    $cb0.eq(7).val('.Digital');
            //    $cb0.eq(8).val('.Video');

            //    //Add values to checkboxes: Industry
            //    $cb1.eq(0).val('');
            //    $cb1.eq(1).val('.Financial');
            //    $cb1.eq(2).val('.Energy');
            //    $cb1.eq(3).val('.Healthcare');
            //    $cb1.eq(4).val('.Manufacturing');
            //    $cb1.eq(5).val('.PetAndGarden');

            //};

            //Add classes to first of each checkbox in filters
            $("#cblChannels input:eq(0)").addClass("all");
            $("#cblIndustry input:eq(0)").addClass("all");


            //$("#cbSelectAll_Channel").addClass("all");
            //$("#cbSelectAll_Industry").addClass("all");



            $(function () {
                //Set isotope container and elements
                var $container = $('.isotope').isotope({
                    itemSelector: '.element-item',
                    layoutMode: 'fitRows'//,
                    //getSortData: {
                    //    name: '.name',
                    //    symbol: '.symbol',
                    //    number: '.number parseInt',
                    //    category: '[data-category]',
                    //    weight: function (itemElem) {
                    //        var weight = $(itemElem).find('.weight').text();
                    //        return parseFloat(weight.replace(/[\(\)]/g, ''));
                    //    }
                    //}
                });

                // do stuff when checkbox change
                $('#pnlFilters').on('change', function (jQEvent) {
                    //Obtain clicked checkbox
                    var $checkbox = $(jQEvent.target);
                    //console.log($checkbox.attr('id'));
                    manageCheckbox($checkbox);
                    //
                    var comboFilter = getComboFilter(filters);
                    //
                    $container.isotope({
                        filter: comboFilter
                    });
                    //console.log({ filter: comboFilter });
                });
            });

            function getComboFilter(filters) {
                var i = 0;
                var comboFilters = [];
                var message = [];

                for (var prop in filters) {
                    message.push(filters[prop].join(' '));
                    var filterGroup = filters[prop];
                    // skip to next filter group if it doesn't have any values
                    if (!filterGroup.length) {
                        continue;
                    }
                    if (i === 0) {
                        // copy to new array
                        comboFilters = filterGroup.slice(0);
                    } else {
                        var filterSelectors = [];
                        // copy to fresh array
                        var groupCombo = comboFilters.slice(0); // [ A, B ]
                        // merge filter Groups
                        for (var k = 0, len3 = filterGroup.length; k < len3; k++) {
                            for (var j = 0, len2 = groupCombo.length; j < len2; j++) {
                                filterSelectors.push(groupCombo[j] + filterGroup[k]); // [ 1, 2 ]
                            }

                        }
                        // apply filter selectors to combo filters for next group
                        comboFilters = filterSelectors;
                    }
                    i++;
                }

                var comboFilter = comboFilters.join(', ');
                return comboFilter;
            }

            function manageCheckbox($checkbox) {
                var checkbox = $checkbox[0];
                var parentContainer = $checkbox.parents('.option-set');
                var group = $checkbox.parents('.option-set').attr('data-group');
                var $checkboxList_ShowAll = parentContainer.find('input.all');
                var $checkboxList_NotShowAll = parentContainer.find('input:not(.all)');

                // create array for filter group, if not there yet
                var filterGroup = filters[group];
                if (!filterGroup) {
                    filterGroup = filters[group] = [];
                }

                var isAll = $checkbox.hasClass('all');
                // reset filter group if the all box was checked
                //console.log(isAll);
                if (isAll) {
                    //console.log(group + ' | isAll | ' + isAll);
                    delete filters[group];
                    if (!checkbox.checked) {
                        checkbox.checked = 'checked';
                    }
                }

                // index of
                var index = $.inArray(checkbox.value, filterGroup);

                if (checkbox.checked) {
                    var selector = isAll ? 'input' : 'input.all';
                    //get all cb within container
                    var $checkboxList = parentContainer.find(selector);
                    $checkboxList.each(function () {

                        $(this).removeAttr('checked');

                        if (isAll) {
                            if ($(this).hasClass('all')) {
                                $(this).attr('checked', 'checked');
                            };
                        };



                    });


                    if (!isAll && index === -1) {
                        // add filter to group
                        filters[group].push(checkbox.value);
                    }
                } else if (!isAll) {
                    // remove filter from group
                    filters[group].splice(index, 1);

                    // if unchecked the last box, check the all
                    if (!$checkbox.siblings('[checked]').length) { //============================================= error is here?
                        $checkbox.siblings('input.all').attr('checked', 'checked'); //============================================= error is here?
                    }

                    //
                    if ($checkboxList_NotShowAll.not(':checked').length == $checkboxList_NotShowAll.length) {
                        $checkboxList_ShowAll.first().attr('checked', 'checked');
                    }

                }

            }
        }


        //Run only if element exists
        if ($('.option-set').length > 0) { jsPortfolioFiltering(); }
    }
    catch (err) {
        console.log('ERROR: [jsPortfolioFiltering] ' + err.message + ' | ' + err);
    }
});