<%@ Control Language="VB" AutoEventWireup="false" CodeFile="LetsGetStarted.ascx.vb" Inherits="UserControls_Home_LetsGetStarted" %>

<div id="letsGetStartedPnl" class="row shadow wbackgroundColorwhite" data-equalizer>
    <div class="columns small-text-center large-text-left">
        <br />
        <h2>TIME IS MONEY- <br class="show-for-small-down" />SO LET'S GET STARTED!</h2>
        <br class="show-for-medium-only" />
    </div>
    <div class="small-22 small-push-1 medium-8 medium-push-0 columns text-center">
        <asp:HyperLink runat="server" ID="hlnkCreateCampaign" CssClass="hrefGetStarted nth01">
            <div data-equalizer-watch>
                <h4>Create a<br />Campaign</h4>
                <br />
                <div class="row">
                    <div class="columns">
                        <img src="/Images/Icons/CreateACampaign.png" />
                    </div>
                </div>
            </div>
        </asp:HyperLink>
    </div>
    <div class="columns show-for-small-down">&nbsp;</div>
    <div class="small-22 small-push-1 medium-8 medium-push-0 columns text-center">
        <asp:HyperLink runat="server" ID="hlnkInvestment" CssClass="hrefGetStarted nth02">        
            <div data-equalizer-watch>
                <h4>Investment<br />Opportunities</h4>
                <br />
                <div class="row">
                    <div class="columns">
                        <img src="/Images/Icons/InvestmentOpportunities.png" />
                    </div>
                </div>
            </div>
        </asp:HyperLink>
    </div>
    <div class="columns show-for-small-down">&nbsp;</div>
    <div class="small-22 small-push-1 medium-8 medium-push-0 columns text-center">
        <asp:HyperLink runat="server" ID="hlnkProduction" CssClass="hrefGetStarted nth03">
            <div data-equalizer-watch>

                <asp:Panel runat="server" ID="pnlPreviewMode" CssClass="comingSoonRibbon">
                    <div class="ribbon">Coming Soon</div>
                </asp:Panel>

<style>
</style>

                <h4>Production &<br />Marketing</h4>
                <br />
                <div class="row">
                    <div class="columns">
                        <img src="/Images/Icons/ProductionAndMarketing.png" />
                    </div>
                </div>
            </div>
        </asp:HyperLink>
    </div>
    <div class="columns">&nbsp;</div>
</div>
