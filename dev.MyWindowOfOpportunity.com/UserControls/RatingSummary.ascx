<%@ Control Language="VB" AutoEventWireup="false" CodeFile="RatingSummary.ascx.vb" Inherits="UserControls_RatingSummary" %>
<%@ Register Src="~/UserControls/progressBar.ascx" TagName="progressBar" TagPrefix="uc" %>


<asp:HiddenField runat="server" ID="hdnCampaignRatingArray" />
<asp:HiddenField runat="server" ID="hfldReviewCount" />

<%--<asp:GridView runat="server" ID="gv" />--%>

<asp:Panel runat="server" ID="pnlRatingBorder" CssClass="rating-border ">
    <asp:MultiView runat="server" ID="mvRatingSummaries" ActiveViewIndex="0">
        <asp:View runat="server" ID="vCondensed">

            <div class="row collapse rate">
                <div class="small- medium- large-8 columns">
                    <div class="stars">5 Stars</div>
                </div>
                <div class="small- medium- large-11 columns summaryProgressbar">
                    <uc:progressBar runat="server" ID="ucProgressBar_5Stars" fullSize="true" percentage="0" showMarker="false" colorOverride="bgcolor_50percent" />
                </div>
                <div class="small- medium- large-4 columns">
                    <div class="star-per">
                        <asp:Literal runat="server" ID="ltrl_5Stars" Text="0" />%
                    </div>
                </div>
            </div>

            <div class="row collapse rate">
                <div class="small- medium- large-8 columns">
                    <div class="stars">4 Stars</div>
                </div>
                <div class="small- medium- large-11 columns summaryProgressbar">
                    <uc:progressBar runat="server" ID="ucProgressBar_4Stars" fullSize="true" percentage="0" showMarker="false" colorOverride="bgcolor_60percent" />
                </div>
                <div class="small- medium- large-4 columns">
                    <div class="star-per">
                        <asp:Literal runat="server" ID="ltrl_4Stars" Text="0" />%
                    </div>
                </div>
            </div>

            <div class="row collapse rate">
                <div class="small- medium- large-8 columns">
                    <div class="stars">3 Stars</div>
                </div>
                <div class="small- medium- large-11 columns summaryProgressbar">
                    <uc:progressBar runat="server" ID="ucProgressBar_3Stars" fullSize="true" percentage="0" showMarker="false" colorOverride="bgcolor_80percent" />
                </div>
                <div class="small- medium- large-4 columns">
                    <div class="star-per">
                        <asp:Literal runat="server" ID="ltrl_3Stars" Text="0" />%
                    </div>
                </div>
            </div>

            <div class="row collapse rate">
                <div class="small- medium- large-8 columns">
                    <div class="stars">2 Stars</div>
                </div>
                <div class="small- medium- large-11 columns summaryProgressbar">
                    <uc:progressBar runat="server" ID="ucProgressBar_2Stars" fullSize="true" percentage="0" showMarker="false" colorOverride="bgcolor_90percent" />
                </div>
                <div class="small- medium- large-4 columns">
                    <div class="star-per">
                        <asp:Literal runat="server" ID="ltrl_2Stars" Text="0" />%
                    </div>
                </div>
            </div>

            <div class="row collapse rate">
                <div class="small- medium- large-8 columns">
                    <div class="stars">1 Star</div>
                </div>
                <div class="small- medium- large-11 columns summaryProgressbar">
                    <uc:progressBar runat="server" ID="ucProgressBar_1Star" fullSize="true" percentage="0" showMarker="false" colorOverride="bgcolor_100percent" />
                </div>
                <div class="small- medium- large-4 columns">
                    <div class="star-per">
                        <asp:Literal runat="server" ID="ltrl_1Star" Text="0" />%
                    </div>
                </div>
            </div>

            <div class="out-star">
                <asp:Literal runat="server" ID="ltrlAvgStars" Text="0" />
                out of 5 stars
            </div>

        </asp:View>
        <asp:View runat="server" ID="vFullWidth">
            <div class="fullWidthSummary">

                <div class="row collapse rate">
                    <div class="small-5 medium-4 large-5 columns">
                        <div class="stars">5 Stars</div>
                    </div>
                    <div class="small-15 medium-16 large-15 columns summaryProgressbar">
                        <uc:progressBar runat="server" ID="ucProgressBar_5Stars_full" fullSize="true" percentage="0" showMarker="false" colorOverride="bgcolor_50percent" />
                    </div>
                    <div class="small-4 medium-4 large-4 columns">
                        <div class="star-per">
                            <asp:Literal runat="server" ID="ltrl_5Stars_full" Text="0" />%
                        </div>
                    </div>
                </div>

                <div class="row collapse rate">
                    <div class="small-5 medium-4 large-5 columns">
                        <div class="stars">4 Stars</div>
                    </div>
                    <div class="small-15 medium-16 large-15 columns summaryProgressbar">
                        <uc:progressBar runat="server" ID="ucProgressBar_4Stars_full" fullSize="true" percentage="0" showMarker="false" colorOverride="bgcolor_60percent" />
                    </div>
                    <div class="small-4 medium-4 large-4 columns">
                        <div class="star-per">
                            <asp:Literal runat="server" ID="ltrl_4Stars_full" Text="0" />%
                        </div>
                    </div>
                </div>

                <div class="row collapse rate">
                    <div class="small-5 medium-4 large-5 columns">
                        <div class="stars">3 Stars</div>
                    </div>
                    <div class="small-15 medium-16 large-15 columns summaryProgressbar">
                        <uc:progressBar runat="server" ID="ucProgressBar_3Stars_full" fullSize="true" percentage="0" showMarker="false" colorOverride="bgcolor_80percent" />
                    </div>
                    <div class="small-4 medium-4 large-4 columns">
                        <div class="star-per">
                            <asp:Literal runat="server" ID="ltrl_3Stars_full" Text="0" />%
                        </div>
                    </div>
                </div>

                <div class="row collapse rate">
                    <div class="small-5 medium-4 large-5 columns">
                        <div class="stars">2 Stars</div>
                    </div>
                    <div class="small-15 medium-16 large-15 columns summaryProgressbar">
                        <uc:progressBar runat="server" ID="ucProgressBar_2Stars_full" fullSize="true" percentage="0" showMarker="false" colorOverride="bgcolor_90percent" />
                    </div>
                    <div class="small-4 medium-4 large-4 columns">
                        <div class="star-per">
                            <asp:Literal runat="server" ID="ltrl_2Stars_full" Text="0" />%
                        </div>
                    </div>
                </div>

                <div class="row collapse rate">
                    <div class="small-5 medium-4 large-5 columns">
                        <div class="stars">1 Star</div>
                    </div>
                    <div class="small-15 medium-16 large-15 columns summaryProgressbar">
                        <uc:progressBar runat="server" ID="ucProgressBar_1Star_full" fullSize="true" percentage="0" showMarker="false" colorOverride="bgcolor_100percent" />
                    </div>
                    <div class="small-4 medium-4 large-4 columns">
                        <div class="star-per">
                            <asp:Literal runat="server" ID="ltrl_1Star_full" Text="0" />%
                        </div>
                    </div>
                </div>

                <div class="out-star">
                    <asp:Literal runat="server" ID="ltrlAvgStars_full" Text="0" />
                    out of 5 stars
                </div>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Panel>
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