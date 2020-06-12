<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ulTeamAccts.ascx.vb" Inherits="UserControls_ulTeamAccts" %>

<ul class="ulTeamAccts">
    <li>
        <h6 class="teamName">
            <asp:HyperLink runat="server" ID="hlnkTeamName" />
        </h6>
        <asp:PlaceHolder runat="server" ID="phTeamList" />
    </li>
</ul>
