<%@ Control Language="VB" AutoEventWireup="false" CodeFile="CampaignPhase.ascx.vb" Inherits="UserControls_CampaignPhase" %>
<%@ Register Src="progressBar.ascx" TagName="progressBar" TagPrefix="uc" %>

<asp:Panel runat="server" ID="pnlPhase" CssClass="row phase">
    <div class="small-24 columns">
        <div class="summaryPanel "><%--hide--%>
            <div class="row">
                <h3 class="phaseNo">PHASE<asp:Literal runat="server" ID="ltrlPhaseNo" />:</h3>
                <h4 class="title">&nbsp;&nbsp;<asp:Literal runat="server" ID="ltrlTitle" /></h4>
                <h4 runat="server" id="h4LearnMore" class="right phaseLearnMore">Learn More</h4>
                <br />

                <div class="phaseDescription row">
                    <div class="small-24 columns">
                        <hr />
                        <asp:Literal runat="server" ID="ltrlPhaseDescription" />
                        <hr />
                    </div>
                </div>

                <div class="medium-14 columns">
                    <div>PLEDGED:</div>
                    <div class="goal">
                        <asp:Literal runat="server" ID="ltrlPledgedDollarAmt" />.<sup><asp:Literal runat="server" ID="ltrlPledgedCents" /></sup></div>
                    <asp:Panel runat="server" ID="pnlPhaseMsg" CssClass="phaseMsg radius text-center" Visible="false">
                        Phase fully funded
                    </asp:Panel>
                </div>
                <div class="medium-10 columns">
                    <div class="">GOAL:&nbsp;&nbsp;<strong><asp:Literal runat="server" ID="ltrlGoalDollarAmt" />.<sup><asp:Literal runat="server" ID="ltrlGoalCents" /></sup></strong></div>
                    <div class="">CONTRIBUTORS:&nbsp;&nbsp;<strong><asp:Literal runat="server" ID="ltrlPledges" /></strong></div>
                    
                    <asp:Panel runat="server" ID="pnlDaysRemaining" Visible="false">
                        DAYS REMAINING:&nbsp;&nbsp;
                        <strong><asp:Literal runat="server" ID="ltrlDaysRemaining" /></strong>
                    </asp:Panel>
                    
                    <asp:Panel runat="server" ID="pnlFundThisPhase" CssClass="" Visible="false">
                        <br />
                        <asp:HyperLink runat="server" ID="hlnkFundThisCampaign" CssClass="button expand radius secondary tiny" Text="FUND THIS PHASE" />
                    </asp:Panel>
                </div>
            </div>
            <asp:Panel runat="server" ID="pnlProgressBar" CssClass="row">
                <div class="columns">
                    <br class="show-for-small-down" />
                    <br />
                    <uc:progressBar runat="server" ID="ucProgressBar" percentage="0" fullSize="true" />
                </div>
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlComplete" CssClass="row pnlComplete" Visible="false">
                <div class="columns small-text-center medium-text-right">
                    <h3>Phase Completed Successfully!</h3>
                </div>
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlPendingPhase" CssClass="row pnlPendingPhase" Visible="false">
                <div class="columns small-text-center medium-text-right">
                    <h3>Phase Pending</h3>
                </div>
            </asp:Panel>
        </div>
    </div>
</asp:Panel>