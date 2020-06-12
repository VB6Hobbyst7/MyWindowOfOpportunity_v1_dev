<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PledgeListItem_AcctEdit.ascx.vb" Inherits="UserControls_PledgeListItem" %>


<div class="pledgeList">

    <%--<asp:GridView runat="server" ID="gv" />--%>

    <asp:Panel runat="server" ID="pnlPledge" CssClass="row ">

        <div class="small-16 medium-4 large-5 columns">
            <asp:Literal runat="server" ID="ltrlDate" />&nbsp;
        </div>
        <div class="small-8 medium-4 large-5 columns small-text-right medium-text-left">
            <asp:Literal runat="server" ID="ltrlPledged" />&nbsp;
        </div>
        <div class="hide-for-small-down medium-8 large-10 columns">
            <asp:Literal runat="server" ID="ltrlCampaign" />
            <asp:HyperLink runat="server" ID="hlnkCampaign" CssClass="tabLink hlnk" />
            &nbsp;
        </div>

        <div class="small-4  medium-2 large-1 small-push-4 medium-push-0 columns iconFulfilled">
            <asp:Image runat="server" ImageUrl="~/Images/Icons/square_valid_Inactive.png" ToolTip="Fulfilled" CssClass="inactive" />
            <asp:Image runat="server" ImageUrl="~/Images/Icons/square_valid.png" ToolTip="Fulfilled" CssClass="active" />
        </div>
        <div class="small-4 medium-2 large-1 small-push-4 medium-push-0 columns iconCanceled">
            <asp:Image runat="server" ImageUrl="~/Images/Icons/square_invalid_Inactive.png" ToolTip="Canceled" CssClass="inactive" />
            <asp:Image runat="server" ImageUrl="~/Images/Icons/square_invalid.png" ToolTip="Canceled" CssClass="active" />
        </div>
        <div class="small-4 medium-2 large-1 small-push-4 medium-push-0 columns iconDeclined">
            <asp:Image runat="server" ImageUrl="~/Images/Icons/square_warning_Inactive.png" ToolTip="Declined" CssClass="inactive" />
            <asp:Image runat="server" ImageUrl="~/Images/Icons/square_warning.png" ToolTip="Declined" CssClass="active" />
        </div>
        <div class="small-4 medium-2 large-1 small-push-4 medium-push-0 columns iconReimbursed">
            <asp:Image runat="server" ImageUrl="~/Images/Icons/reimbursed_Inactive.png" ToolTip="Reimbursed" CssClass="inactive" />
            <asp:Image runat="server" ImageUrl="~/Images/Icons/reimbursed.png" ToolTip="Reimbursed" CssClass="active" />
        </div>
        <div class="small-1 show-for-small-down">
            <br />
            <br />
            <br />
        </div>
        
    </asp:Panel>

</div>