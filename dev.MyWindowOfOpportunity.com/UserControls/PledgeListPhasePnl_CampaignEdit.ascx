<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PledgeListPhasePnl_CampaignEdit.ascx.vb" Inherits="UserControls_PledgeListPhasePnl" %>
<%@ Register Src="PledgeListItem_CampaignEdit.ascx" TagName="PledgeListItem_CampaignEdit" TagPrefix="uc" %>

<asp:Panel runat="server" ID="phasePnl" CssClass="phasePnl">

    <%--    <asp:GridView runat="server" ID="gv" />
    <hr />--%>


    <div class="row">
        <div class="columns">
            <h2>Phase
                <asp:Literal runat="server" ID="ltrlPhaseNo" /></h2>
        </div>
    </div>
    
    <div class="row ">
        <div class="small-12 medium-8 large-4 columns">
            <strong>Date</strong>
        </div>
        <div class="small-12 medium-8 large-4 columns small-text-right medium-text-left">
            <strong>Pledged</strong>
        </div>
        <div class="hide-for-medium-down large-4 columns">
            <strong>Donated By</strong>
        </div>        
        <div class="hide-for-medium-down large-1 columns">&nbsp;</div>
        <div class="hide-for-medium-down large-1 columns">&nbsp;</div>
        <div class="hide-for-medium-down large-1 columns">&nbsp;</div>
        <div class="hide-for-medium-down large-1 columns">&nbsp;</div>
    </div>


    <asp:ListView runat="server" ID="lstPledgeItems">
        <LayoutTemplate>
            <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
        </LayoutTemplate>
        <ItemTemplate>
            <uc:PledgeListItem_CampaignEdit runat="server" campaignPledges="<%# Container.DataItem %>" />
        </ItemTemplate>
        <EmptyDataTemplate></EmptyDataTemplate>
    </asp:ListView>


</asp:Panel>

