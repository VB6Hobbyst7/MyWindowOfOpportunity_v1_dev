<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PledgeListItem_Campaign.ascx.vb" Inherits="UserControls_PledgeListItem" %>

<%--<asp:GridView runat="server" ID="gv" />--%>

<div class="row pledgeList" data-equalizer>
    <%--VIEW FOR CAMPAIGNS--%>
    <div class="hide-for-small-down medium-2 large-1 columns" data-equalizer-watch>
        <asp:Image runat="server" ID="memberImgUrl" CssClass="imgFeatured" />
    </div>

    <div class="small-16 medium-9 medium-push-1 large-10 large-push-1 columns centerVertically" data-equalizer-watch>
        <asp:Literal runat="server" ID="ltrlDonatedBy_Campaign" />
    </div>

    <div class="small-8 medium-6 medium-push-1 large-6 large-push-1 columns small-text-right medium-text-left centerVertically" data-equalizer-watch>
        <asp:Literal runat="server" ID="ltrlPledged_Campaign" />
    </div>

    <div class="hide-for-small-down medium-6 medium-push-1 large-6 large-push-1 columns centerVertically" data-equalizer-watch>
        <asp:Literal runat="server" ID="ltrlDate_Campaign" />
    </div>

</div>

