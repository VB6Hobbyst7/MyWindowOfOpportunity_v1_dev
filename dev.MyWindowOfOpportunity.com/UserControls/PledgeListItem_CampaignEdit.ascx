<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PledgeListItem_CampaignEdit.ascx.vb" Inherits="UserControls_PledgeListItem" %>
<%@ Register Src="/UserControls/ShipTo.ascx" TagName="ShipTo" TagPrefix="uc" %>
<%@ Register Src="/UserControls/AlertMsg.ascx" TagName="AlertMsg" TagPrefix="uc" %>


<div class="pledgeList">
    <%--<asp:GridView runat="server" ID="gv" />
    <asp:GridView runat="server" ID="gv2" />
    <asp:GridView runat="server" ID="gv3" />
    <hr />--%>

    <asp:Panel runat="server" ID="pnlPledge" CssClass="row ">

        <div class="small-16 medium-8 large-4 columns">
            <asp:Literal runat="server" ID="ltrlDate" />&nbsp;
        </div>
        <div class="small-8 medium-16 large-4 columns small-text-right medium-text-left">
            <asp:Literal runat="server" ID="ltrlPledged" />&nbsp;
        </div>
        <div class="hide-for-medium-down large-4 columns">
            <asp:Literal runat="server" ID="ltrlDonatedBy" />&nbsp;
        </div>



        <div class="small-5 medium-2 large-1 columns iconFulfilled">
            <asp:PlaceHolder runat="server" ID="phShipmentDetails">
                <asp:Image runat="server" ID="imgShipmentDetails" ImageUrl="~/Images/Icons/shippingIcon.png" ToolTip="Shipping Details" CssClass="active shipmentDetails" />
            </asp:PlaceHolder>
			&nbsp;
        </div>
        <div class="small-19 medium-14 large-7 columns">
            <asp:PlaceHolder runat="server" ID="phShipmentStatus">
                <div>
                    <asp:Label runat="server" ID="lblStatus" CssClass="lblStatus" />
                </div>
            </asp:PlaceHolder>
			&nbsp;
        </div>



        <div class="small-4  medium-2 large-1 columns iconFulfilled">
            <asp:Image runat="server" ImageUrl="~/Images/Icons/square_valid_Inactive.png" ToolTip="Fulfilled" CssClass="inactive" />
            <asp:Image runat="server" ImageUrl="~/Images/Icons/square_valid.png" ToolTip="Fulfilled" CssClass="active" />
        </div>
        <div class="small-4 medium-2 large-1 columns iconCanceled">
            <asp:Image runat="server" ImageUrl="~/Images/Icons/square_invalid_Inactive.png" ToolTip="Canceled" CssClass="inactive" />
            <asp:Image runat="server" ImageUrl="~/Images/Icons/square_invalid.png" ToolTip="Canceled" CssClass="active" />
        </div>
        <div class="small-4 medium-2 large-1 columns iconDeclined">
            <asp:Image runat="server" ImageUrl="~/Images/Icons/square_warning_Inactive.png" ToolTip="Declined" CssClass="inactive" />
            <asp:Image runat="server" ImageUrl="~/Images/Icons/square_warning.png" ToolTip="Declined" CssClass="active" />
        </div>
        <div class="small-4 medium-2 large-1 columns iconReimbursed">
            <asp:Image runat="server" ImageUrl="~/Images/Icons/reimbursed_Inactive.png" ToolTip="Reimbursed" CssClass="inactive" />
            <asp:Image runat="server" ImageUrl="~/Images/Icons/reimbursed.png" ToolTip="Reimbursed" CssClass="active" />
        </div>
        <div class="small-1 show-for-small-down">
            &nbsp;
        </div>

    </asp:Panel>
        
    <uc:ShipTo runat="server" ID="ucShipTo" />

    <div class="row show-for-medium-down">
        <div class="columns">
            <hr />
        </div>
    </div>
</div>
