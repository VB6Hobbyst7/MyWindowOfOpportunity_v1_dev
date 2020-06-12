<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ManageCampaign_List.ascx.vb" Inherits="UserControls_ManageCampaign_List" %>
<%@ Register Src="ManageCampaign_ListItem.ascx" TagName="ManageCampaign_ListItem" TagPrefix="uc" %>



<asp:ListView runat="server" ID="lstview">
    <LayoutTemplate>
        <fieldset>
            <legend></legend>
            <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
        </fieldset>
    </LayoutTemplate>
    <ItemTemplate>
        <uc:ManageCampaign_ListItem runat="server" nodeId='<%#Eval("nodeId")%>' isTeamName='<%#Eval("isTeamName")%>' isAdmin='<%#Eval("isAdmin")%>' />                    
    </ItemTemplate>
    <ItemSeparatorTemplate><hr class="show-for-small-down" /></ItemSeparatorTemplate>
</asp:ListView>

