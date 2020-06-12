<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ManageCampaign_ListItem.ascx.vb" Inherits="UserControls_ManageCampaign_ListItem" %>


<div class="row">

    <div class="small-12 show-for-small-down columns end">
        <asp:MultiView runat="server" ID="mvItemType_Mbl" ActiveViewIndex="0">
            <asp:View runat="server" id="vTeamName_Mbl">
                <h5><asp:Literal runat="server" ID="ltrlTeamName_Mbl" /></h5>
            </asp:View>             
            <asp:View runat="server" id="vCampaignName_Mbl">
                <h6><asp:Literal runat="server" ID="ltrlCampaignName_Mbl" /></h6>                           
            </asp:View>
        </asp:MultiView>
    </div>



    <div class="small-6 medium-5 large-3 text-right columns">
        <asp:HyperLink runat="server" ID="hlnkView" CssClass="button tiny radius " Text="View" />
    </div>
    <div class="small-6 medium-4 large-2 columns">
        <asp:HyperLink runat="server" ID="hlnkEdit" CssClass="button tiny radius " Text="Edit" />
    </div>



    <div class="show-for-medium-up medium-15 large-19 columns end">
        <asp:MultiView runat="server" ID="mvItemType" ActiveViewIndex="0">
            <asp:View runat="server" id="vTeamName">
                <h5><asp:Literal runat="server" ID="ltrlTeamName" /></h5>
            </asp:View>             
            <asp:View runat="server" id="vCampaignName">
                <h6><asp:Literal runat="server" ID="ltrlCampaignName" /></h6>                           
            </asp:View>
        </asp:MultiView>
    </div>
</div>