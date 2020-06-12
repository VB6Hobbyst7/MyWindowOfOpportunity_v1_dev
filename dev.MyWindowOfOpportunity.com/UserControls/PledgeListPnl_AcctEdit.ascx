<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PledgeListPnl_AcctEdit.ascx.vb" Inherits="UserControls_PledgeListPhasePnl" %>
<%@ Register Src="PledgeListItem_AcctEdit.ascx" TagName="PledgeListItem_AcctEdit" TagPrefix="uc" %>

<div class="columns">
    <div class="row ">
        <div class="small-12 medium-4 large-5 columns">
            <strong>Date</strong>
        </div>
        <div class="small-12 medium-4 large-5 columns small-text-right medium-text-left">
            <strong>Pledged</strong>
        </div>
        <div class="hide-for-small-down medium-8 large-10 columns">
            <strong>Campaign</strong>
        </div>
        
        <div class="hide-for-small-down medium-2 large-1 columns">&nbsp;</div>
        <div class="hide-for-small-down medium-2 large-1 columns">&nbsp;</div>
        <div class="hide-for-small-down medium-2 large-1 columns">&nbsp;</div>
        <div class="hide-for-small-down medium-2 large-1 columns">&nbsp;</div>
    </div>
       
    <asp:ListView runat="server" ID="lstPledgeItems">
        <LayoutTemplate>
            <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
        </LayoutTemplate>
        <ItemTemplate>
            <uc:PledgeListItem_AcctEdit runat="server" campaignPledge="<%# Container.DataItem %>" />
        </ItemTemplate>
        <EmptyDataTemplate></EmptyDataTemplate>
    </asp:ListView>

</div>