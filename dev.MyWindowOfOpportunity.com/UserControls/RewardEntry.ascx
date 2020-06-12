<%@ Control Language="VB" AutoEventWireup="false" CodeFile="RewardEntry.ascx.vb" Inherits="UserControls_RewardEntry" EnableViewState="true" %>
<%--<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="uc" %>--%>
<%@ Register Src="TextBox_Animated.ascx" TagName="TextBox_Animated" TagPrefix="uc" %>
<%@ Register Src="MinorImageSelector_Rewards.ascx" TagName="MinorImageSelector_Rewards" TagPrefix="uc" %>
<%@ Reference Control="AlertMsg.ascx" %>


<asp:Panel runat="server" ID="pnl" type="rewardEntry" CssClass="hide">

    <h6>Reward Info</h6>
    <uc:TextBox_Animated runat="server" Title="Heading" ID="txbHeading" />
    <uc:TextBox_Animated runat="server" Title="Brief Summary" isMultiLine="true" ID="txbBriefSummary" />


    <div class="row">
        <div class="small-24 medium-8 large-8 columns">
            <uc:TextBox_Animated runat="server" Title="Contribution Amount" ID="txbContributionAmount" CausesValidation="true" ErrorMessage="*" isCurrency="true" />
        </div>
        <div class="small-24 medium-8 large-8 columns">
            <uc:TextBox_Animated runat="server" Title="Quantity Available" ID="txbQuantityAvailable" CausesValidation="true" ErrorMessage="*" isNumeric="true" />
        </div>
        <div class="small-24 medium-8 large-8 columns">
            <uc:TextBox_Animated runat="server" Title="# claimed" ID="txbNumberClaimed" CausesValidation="true" ErrorMessage="*" isNumeric="true" />
        </div>
    </div>

    <div class="row">
        <div class="small-24 medium-24 large-12 columns">
            <h6 class="secondary">Featured Image</h6>
            <p>Select the featured image that is to be used for your reward.<br />
            <span class="smaller">Edit your image's cropping within the "IMAGES" tab.</span></p>
        </div>
        <div class="small-24 medium-24 large-12 columns text-center">
            <uc:MinorImageSelector_Rewards runat="server" ID="ucMinorImage" activeView="DisplayImg" thisTab="Rewards" />
        </div>
    </div>

    <div class="row">
        <div class="small-24 medium-24 large-8 columns">
            <br class="show-for-medium-down" />
            <h6 class="secondary">Approximate Delivery</h6>
        </div>
        <div class="small-24 medium-12 large-8 columns">
            <asp:DropDownList runat="server" ID="ddlMonth" CssClass="radius" AppendDataBoundItems="false" />
        </div>
        <div class="small-24 medium-12 large-8 columns end">
            <asp:DropDownList runat="server" ID="ddlYear" CssClass="radius" AppendDataBoundItems="false" />
        </div>
    </div>

    <div class="row">
        <br />
        <div class="small-24 medium-8 large-12 columns">
            <h6 class="displayInline secondary">Show on reward list </h6>            
        </div>
        <div class="small-24 medium-16 large-12 columns">
            <div class="switch round">
                <asp:CheckBox runat="server" ID="cbShowOnTimeline" />
                <label runat="server" id="lbl">
                    <span class="switch-on">Yes</span>
                    <span class="switch-off">No</span>
                </label>
            </div>
        </div>
    </div>

    <asp:HiddenField runat="server" ID="hfldNodeId" />

</asp:Panel>
