<%@ Control Language="VB" AutoEventWireup="false" CodeFile="CampaignPanel.ascx.vb" Inherits="UserControls_CampaignPanel" %>
<%@ Register Src="progressBar.ascx" TagPrefix="uc" TagName="progressBar" %>
<%@ Register Src="progressBar1.ascx" TagPrefix="uc" TagName="progressBar1" %>
<%@ Register Src="DaysRemaining.ascx" TagPrefix="uc" TagName="DaysRemaining" %>
<%@ Register Src="RatingSummary.ascx" TagPrefix="uc" TagName="RatingSummary" %>
<%@ Register Src="PhaseMeter.ascx" TagPrefix="uc" TagName="PhaseMeter" %>


<div class=" CampaignPanel campaignsItemEntry">
    <div class="columns">
        <asp:Panel runat="server" ID="pnlCampaign" CssClass="round border ">
            <asp:HyperLink runat="server" ID="hlnkCampaign">
                <asp:Image runat="server" ID="imgCampaign" CssClass="round topOnly campaignImg" />
                <div data-equalizer-watch="cheer">
                    <asp:Panel runat="server" ID="pnlCheer" CssClass="row collapse pnlCheer" Visible="true">
                        <div class="cheerBox">
                            <asp:Literal runat="server" ID="ltrlCheer" />
                        </div>
                    </asp:Panel>
                </div>
                <div class="titleSection" data-equalizer-watch="title">
                    <div class="title">
                        <asp:Literal runat="server" ID="ltrlTitle"></asp:Literal>
                    </div>
                    <div class="author">
                        by
                        <asp:Literal runat="server" ID="ltrlAuthor"></asp:Literal>
                    </div>
                </div>
            </asp:HyperLink>
            <div id="divCampaignRatingReview" class="home-campaign-rating statistics" runat="server" data-equalizer-watch="statistics">
                <div id="avgrateCampaignOuter" runat="server" class="avgRateCampaignOuter">
                    <uc:RatingSummary runat="server" ID="ucRatingSummary" condensedView="true" />
                    <div class="row pledgePnl">
                        <div class="small-15 small-push-1 medium-15 medium-push-1 large-15 large-push-0 columns">
                            <div id="avgrateCampaignYo" runat="server"></div>
                        </div>
                        <div class="small-9 medium-8 large-9 columns">
                            <div id="totalReviews" runat="server" class="pledgedPercentage cus-review text-center">
                                <div class="ReviewCount">
                                    <asp:Literal runat="server" ID="ltrlReviewCount" Text="0" />
                                </div>
                                <div>Reviews</div>
                            </div>
                        </div>
                        <div class="large-24 columns">
                            <uc:PhaseMeter runat="server" ID="ucPhaseMeter02" />
                        </div>
                    </div>
                </div>
            </div>
            <div id="divCampaignStats" runat="server">
                <div class="row collapse statistics" data-equalizer-watch="statistics">
                    <div class="small- medium- large-24 columns progressBar">
                        <uc:progressBar1 runat="server" ID="ucProgressBar" />
                    </div>

                    <div>
                        <asp:PlaceHolder runat="server" ID="phActiveCampaign" Visible="false">
                            <div class="small-5 medium-4 medium-push-1 large-5 large-push-1 columns">
                                <uc:DaysRemaining runat="server" ID="ucDaysRemaining" />
                            </div>

                            <div class="small-12 medium-12 medium-push-2 large-10 large-push-2 columns text-center">
                                <div class="pledgedAmount">
                                    <asp:Literal runat="server" ID="ltrlPledged"></asp:Literal>
                                </div>
                                <div class="pledgedAction">
                                    Pledged
                                </div>
                            </div>

                            <div class="small-7 medium-5 medium-push-3 large-6 large-push-3 columns end pledgedPercentage text-center">
                                <asp:Literal runat="server" ID="ltrlPercentage"></asp:Literal>
                            </div>
                        </asp:PlaceHolder>

                    </div>

                    <div>
                        <asp:PlaceHolder runat="server" ID="phCompleteCampaign" Visible="false">
                            <div class="small-14 columns text-center">
                                <div class="pledgedAmount">
                                    <asp:Literal runat="server" ID="ltrlPledged_Complete"></asp:Literal>
                                </div>
                                <div class="pledgedAction">
                                    Pledged
                                </div>
                            </div>

                            <div class="small-10 columns end pledgedPercentage text-center">
                                <asp:Literal runat="server" ID="ltrlPercentage_Complete"></asp:Literal>
                            </div>
                        </asp:PlaceHolder>
                        <br />
                    </div>

                    <div class="large-24 columns">
                        <uc:PhaseMeter runat="server" ID="ucPhaseMeter01" />
                    </div>

                </div>
            </div>
        </asp:Panel>

        <asp:Panel runat="server" ID="pnlViewMore" CssClass="round border " Visible="false">
            <asp:HyperLink runat="server" ID="hlnkViewMore">
                <asp:Image runat="server" ID="imgViewMore" CssClass="round topOnly campaignImg" />
                <div data-equalizer-watch="cheer">&nbsp;</div>
                <div class="titleSection" data-equalizer-watch="title">&nbsp;
                    <%--<div class="title">View More</div>
                    <div class="author">
                        by
                        <asp:Literal runat="server" ID="ltrlViewMoreTitle"></asp:Literal>
                    </div>--%>
                </div>
                <div class="row collapse statistics text-center" data-equalizer-watch="statistics">
                    <br />
                    <h4 style="color:white;">VIEW MORE</h4>
                    <asp:PlaceHolder runat="server" ID="phSubcategory" Visible="false">
                        <h6 style="color:white;">by Subcategory</h6>
                    </asp:PlaceHolder>                    
                </div>
            </asp:HyperLink>
        </asp:Panel>

    </div>
</div>
