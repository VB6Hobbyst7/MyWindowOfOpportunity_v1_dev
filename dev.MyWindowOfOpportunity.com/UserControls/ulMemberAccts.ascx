<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ulMemberAccts.ascx.vb" Inherits="UserControls_ulMemberAccts" %>

<%--<h5 class="whiteTextShadow text-left">Member Accounts</h5>--%>
<%--<ul class="ulMemberAccts">
    <li>
        <h6 class="campaignName">
            <asp:HyperLink runat="server" ID="hlnkCampaignName" />
        </h6>
    </li>
</ul>--%>

<ul class="ulMemberAccts">
    <li>
        <h6 class="teamName">
            <asp:HyperLink runat="server" ID="hlnkTeamName" />
        </h6>
        <ul>
            <li>
                <h6 class="campaignName">
                    <asp:HyperLink runat="server" ID="hlnkCampaignName" />
                </h6>
            </li>
        </ul>
    </li>
</ul>