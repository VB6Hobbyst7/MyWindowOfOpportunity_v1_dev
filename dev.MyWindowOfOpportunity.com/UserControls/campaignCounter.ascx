<%@ Control Language="VB" AutoEventWireup="false" CodeFile="campaignCounter.ascx.vb" Inherits="UserControls_campaignCounter" %>


<h2>Campaign Counter</h2>
<h4>Total: <asp:Literal runat="server" ID="ltrlCount" /> Campaigns</h4>
<div class="CampaignCounter">
<asp:GridView runat="server" ID="gv" CssClass="Grid"  CellPadding="3" GridLines="Horizontal" >
    <AlternatingRowStyle CssClass="AlternatingRowStyle" />
    <FooterStyle CssClass="FooterStyle"  />
    <HeaderStyle CssClass="HeaderStyle"  />
    <PagerStyle CssClass="RowStyle"  HorizontalAlign="Right" />
    <RowStyle CssClass="RowStyle"  />
    <SelectedRowStyle CssClass="SelectedRowStyle" />
    <SortedAscendingCellStyle CssClass="SortedAscendingCellStyle"  />
    <SortedAscendingHeaderStyle CssClass="SortedAscendingHeaderStyle" />
    <SortedDescendingCellStyle CssClass="SortedDescendingCellStyle"  />
    <SortedDescendingHeaderStyle CssClass="SortedDescendingHeaderStyle"  />
</asp:GridView>
    </div> 