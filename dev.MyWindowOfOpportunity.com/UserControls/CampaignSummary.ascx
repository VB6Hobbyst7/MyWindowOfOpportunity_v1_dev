<%@ Control Language="VB" AutoEventWireup="false" CodeFile="CampaignSummary.ascx.vb" Inherits="UserControls_CampaignSummary" %>


<div class="campaignSummary">
    <fieldset class="outlinePanel secondary">
        <legend>Campaign Summary</legend>

        <ul class="small-block-grid-1 medium-block-grid-3 large-block-grid-1 small-text-center medium-text-left">
            <li>
                <div class="bold">PERCENT FUNDED:</div>
                <div class="show-for-medium-down">
                    <asp:Literal runat="server" ID="ltrlPercentFunded" />%
                </div>
                <div class="text-center hide-for-medium-down">
                    <canvas id="canvas" height="200" width="200"></canvas>
                </div>
            </li>
            <li>
                <div class="bold">TOTAL PLEDGED:</div>
                <div class="goal">
                    <asp:Literal runat="server" ID="ltrlPledgedDollarAmt" />.<sup><asp:Literal runat="server" ID="ltrlPledgedCents" /></sup>
                </div>
            </li>
            <li>
                <div class="bold">CAMPAIGN GOAL:</div>
                <div class="">
                    <asp:Literal runat="server" ID="ltrlGoalDollarAmt" />.<sup><asp:Literal runat="server" ID="ltrlGoalCents" /></sup>
                </div>
            </li>
            <li>
                <div class="bold">TOTAL CONTRIBUTORS:</div>
                <div class="">
                    <asp:Literal runat="server" ID="ltrlTotalContributors" />
                </div>
            </li>
            <asp:PlaceHolder runat="server" ID="phCompletionDate">
                <li>
                    <div class="bold">COMPLETED ON:</div>
                    <div class="">
                        <asp:Literal runat="server" ID="ltrlCompletionDate" />
                    </div>
                </li>
            </asp:PlaceHolder>
        </ul>

    </fieldset>
</div>
<script>
    $(function () {
        try {
            function jsPhaseGuages() {
                $(".summaryProgressbar").each(function () {
                    //Instantiate variables
                    var game = { score: 0 };

                    //$("#Phase1 .summaryPanel").each(function () {
                    //Instantiate variables
                    var marker = $(this).find('.pnlMarker');
                    var innerPanel = $(this).find('.innerPanel');
                    var innerPanelWidth = $(this).find("input[type=hidden]").val();
                    var scoreDisplay = $(this).find('.score');

                    if (innerPanelWidth > 100) {
                        TweenMax.fromTo(innerPanel, 1, { css: { width: "0px" } }, { css: { width: "100%" }, delay: .5 });
                        TweenMax.to(marker, 1, { delay: .5, left: "100%" }); //slide marker into position
                    } else {
                        TweenMax.fromTo(innerPanel, 1, { css: { width: "0px" } }, { css: { width: innerPanelWidth + "%" }, delay: .5 });
                        TweenMax.to(marker, 1, { delay: .5, left: innerPanelWidth + "%" }); //slide marker into position
                    }


                    if (innerPanelWidth > 0) {
                        countUp(innerPanelWidth, scoreDisplay);
                    };




                    function countUp(innerPanelWidth, scoreDisplay) {
                        game.score = 0;
                        TweenMax.to(game, 1, {
                            score: "+=" + innerPanelWidth,
                            roundProps: "score",
                            onUpdate: updateHandler,
                            onUpdateParams: [scoreDisplay],
                            ease: Linear.easeNone,
                            delay: .5
                        });
                    }
                    function updateHandler(scoreDisplay) {
                        scoreDisplay.html(game.score + '%');
                    }
                });
            };


            //Run only if element exists
            if ($('.summaryProgressbar').length > 0) { jsPhaseGuages(); }
        }
        catch (err) {
            console.log('ERROR: [jsPhaseGuages] ' + err.message);
        }
    });
</script>
