$(function () {
    try {
        function jsCampaignAnimations() {

            //Lear More opening slider
            var $LearnMore = $("#Summaries .phaseLearnMore");
            
            //If only 1 phase exists, open phase.
            if ($LearnMore.length == 1) {
                var clickedBtn = $LearnMore.eq(0);
                var phaseDescription = clickedBtn.siblings(".phaseDescription");
                phaseDescription.slideToggle(500, function () {
                    //execute this after slideToggle is done
                    //change text of header based on visibility of content div
                    clickedBtn.text(function () {
                        //change text based on condition
                        return phaseDescription.is(":visible") ? "Collapse" : "Learn More"
                    });
                });
            }

            $LearnMore.click(function () {
                var clickedBtn = $(this);
                var phaseDescription = $(this).siblings(".phaseDescription");
                phaseDescription.slideToggle(500, function () {
                    //execute this after slideToggle is done
                    //change text of header based on visibility of content div
                    clickedBtn.text(function () {
                        //change text based on condition
                        return phaseDescription.is(":visible") ? "Collapse" : "Learn More"
                    });
                });
            });
            $('.open-popup-link').magnificPopup({
                type: 'inline',
                midClick: true// Allow opening popup on middle mouse click. Always set it to true if you don't provide alternative source in href.
            });


            
            pieChart();
            function pieChart() {
                //Obtain percentage to be displayed in piechart
                var percentFunded = parseInt($("#hfldPercentFunded").val());
                var pieData = [{
                    value: (100 - percentFunded),
                    color: "#8cc63f",
                    label: '% Needed',
                    labelColor: 'black',
                    labelFontSize: '20'
                }, {
                    value: percentFunded,
                    color: "#0071bc",
                    label: '% Pledged',
                    labelColor: 'white',
                    labelFontSize: '20'
                }
                ];
                if ($('#chart-area').length > 0) {
                    var ctx = document.getElementById("chart-area").getContext("2d");
                    var pieOptions = {
                        segmentShowStroke: false,
                        responsive: true,
                        maintainAspectRatio: true,
                        scaleShowLabels: true,
                        scaleShowLabelBackdrop: true,
                        labelAlign: 'center',
                        onAnimationComplete: function () {
                            this.showTooltip(this.segments, true);
                        },
                        tooltipEvents: [],
                        showTooltips: true
                    }
                    var myChart = new Chart(ctx).Pie(pieData, pieOptions);
                }
            };

            //Obtain percentage to be displayed in piechart
            var percentFunded = parseInt($("#hfldPercentFunded").val());
            if (percentFunded > 100) { percentFunded = 100 };

            //Instantiate data for pie chart
            var pieData = [
                {
                    value: percentFunded,
                    color: "#8cc63f"
                },
                {
                    value: 100 - percentFunded,
                    color: "#316dbc"
                }
            ];

            var pieOptions = {
                segmentShowStroke: true,
                segmentStrokeColor: "#fff",
                segmentStrokeWidth: 2,
                percentageInnerCutout: 70,
                animation: true,
                animationSteps: 100,
                animationEasing: "easeOutBounce",
                animateRotate: true,
                animateScale: false,
                onAnimationComplete: null,
                labelFontFamily: "Vegur",
                labelFontStyle: "normal",
                labelFontSize: 30,
                labelFontColor: "#666"
            };

            //Create pie chart
            var myPie = new Chart(document.getElementById("canvas").getContext("2d")).Doughnut(pieData, pieOptions); //{ percentageInnerCutout: 75, animateScale: true });
        }


        //Run only if element exists
        if ($('#campaignPg').length > 0) { jsCampaignAnimations(); }
    }
    catch (err) {
        console.log('ERROR: [jsCampaignAnimations] ' + err.message + ' | ' + err);
    }
});