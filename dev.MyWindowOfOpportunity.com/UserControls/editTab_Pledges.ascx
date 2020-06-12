<%@ Control Language="VB" AutoEventWireup="false" CodeFile="editTab_Pledges.ascx.vb" Inherits="UserControls_editTab_Pledges" %>
<%@ Register Src="PledgeListPhasePnl_CampaignEdit.ascx" TagName="PledgeListPhasePnl_CampaignEdit" TagPrefix="uc" %>


<fieldset class="outlinePanel">
    <legend>Pledge History</legend>
    <div class="row">
        <div class="columns">
            <uc:PledgeListPhasePnl_CampaignEdit runat="server" phaseNo="3" ID="ucPhase03" />
            <uc:PledgeListPhasePnl_CampaignEdit runat="server" phaseNo="2" ID="ucPhase02" />
            <uc:PledgeListPhasePnl_CampaignEdit runat="server" phaseNo="1" ID="ucPhase01" />
            
            <asp:Panel runat="server" ID="pnlNoPledges" CssClass="text-center" Visible="false">
                <br />
                <h5>No pledges have been submitted yet.</h5>
                <br />
            </asp:Panel> 
        </div>
    </div>
</fieldset>
