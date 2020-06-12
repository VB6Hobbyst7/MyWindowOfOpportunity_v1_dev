<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Reward.ascx.vb" Inherits="UserControls_Reward" %>


<div class="rewardPanel">
    <h4>
        <asp:Literal runat="server" ID="ltrlContributionAmount" />
    </h4>
    <h5>
        <asp:Literal runat="server" ID="ltrlHeading" />
    </h5>
    <asp:Image runat="server" ID="imgFeatured" />

    <p><asp:Literal runat="server" ID="ltrlShortDescription" /></p>

    <p><strong>Available: </strong><asp:Literal runat="server" ID="ltrlAvailable" /></p>
    <p><strong>Claimed: </strong><asp:Literal runat="server" ID="ltrlClaimed" /></p>
    <p><strong>Estimated Shipping Date: </strong><asp:Literal runat="server" ID="ltrlShippingDate" /></p>
    <hr />
</div>
