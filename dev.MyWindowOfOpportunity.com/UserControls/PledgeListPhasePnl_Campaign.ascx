<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PledgeListPhasePnl_Campaign.ascx.vb" Inherits="UserControls_PledgeListPhasePnl" %>
<%@ Register Src="PledgeListItem_Campaign.ascx" TagName="PledgeListItem_Campaign" TagPrefix="uc" %>

    <%--<asp:GridView runat="server" ID="gv" />--%>

<asp:Panel runat="server" ID="phasePnl" CssClass="phasePnl">

    <div class="row">
        <div class="columns">
            <h2>Phase <asp:Literal runat="server" ID="ltrlPhaseNo" /></h2>
        </div>
    </div>
     
    <%--VIEW FOR CAMPAIGNS--%>
    <div class="row ">
        <div class="hide-for-small-down medium-2 large-1 columns">&nbsp;</div>

        <div class="small-16 medium-9 medium-push-1 large-10 large-push-1 columns">
            <strong>Donated By</strong>
        </div>

        <div class="small-8 medium-6 medium-push-1 large-6 large-push-1 columns small-text-right medium-text-left">
            <strong>Pledged</strong>
        </div>

        <div class="hide-for-small-down show-for-medium-up medium-6 medium-push-1 large-6 large-push-1 columns">
            <strong>Date</strong>
        </div>
    </div>
         
    <asp:ListView runat="server" ID="lstPledgeItems">
        <LayoutTemplate>
            <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
        </LayoutTemplate>
        <ItemTemplate>
            <uc:PledgeListItem_Campaign ID="uc" runat="server" campaignPledge="<%# Container.DataItem %>" />
        </ItemTemplate>
        <EmptyDataTemplate></EmptyDataTemplate>
    </asp:ListView>

</asp:Panel>