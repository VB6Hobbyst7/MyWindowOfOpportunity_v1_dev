<%@ Control Language="VB" AutoEventWireup="false" CodeFile="EndorsementPhase.ascx.vb" Inherits="UserControls_EndorsementPhase" %>
<%@ Register Src="progressBar.ascx" TagName="progressBar" TagPrefix="uc" %>

<asp:Panel runat="server" ID="pnlPhase" CssClass="row phase">
    <div class="small-24 columns">
            <div class="summaryPanel">
                <div class="row">
                    <%--<h6><asp:Label runat="server" ID="lblTemp" /></h6>--%>
                    <h3>ENDORSEMENTS:</h3>
                    <h4 class="left">Please endorse this campaign</h4>

                    <h4 class="right phaseLearnMore">Learn More</h4>

                    <div class="phaseDescription row">
                        <div class="small-24 columns">
                            <hr />
                            <asp:Literal runat="server" ID="ltrlPhaseDescription" />Explanation as to why endorsements are needed to go here.
                            <hr />
                        </div>                        
                    </div>

                    <div class="small-14 columns">
                        <div>CURRENT ENDORSEMENTS:</div>
                        <div class="goal">
                            <asp:Literal runat="server" ID="ltrlCurrentEndorsements" />
                        </div>   
                    </div>
                    <div class="small-10 columns">
                        <div class="">GOAL:&nbsp;&nbsp;
                            <strong><asp:Literal runat="server" ID="ltrlEndorsementGoal" /></strong>
                        </div>
                        <asp:Panel runat="server" ID="pnlFundThisPhase" CssClass="endorseBtnPnl" Visible="true">
                            <br />
                            <a href="#endorsementPopup" class="open-popup-link">
                                <div class="button expand radius secondary tiny">ENDORSE THIS CAMPAIGN</div>
                            </a>
                        </asp:Panel>                         
                    </div>
                </div>
                <asp:Panel runat="server" ID="pnlProgressBar" CssClass="row">
                    <div class="columns">
                        <br />
                        <uc:progressBar runat="server" ID="ucProgressBar" fullSize="true" />
                    </div>
                </asp:Panel>
            </div>     
        </div>     
</asp:Panel>