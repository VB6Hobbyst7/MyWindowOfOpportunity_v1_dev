<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SocialMediaEntry.ascx.vb" Inherits="UserControls_SocialMediaEntry" %>
<%@ Register Src="TextBox_Animated.ascx" TagName="TextBox_Animated" TagPrefix="uc" %>


<div class="row socialMediaEntry hide">
    <asp:Panel runat="server" ID="pnlAddress" CssClass="">
        <div class="small-24 medium-18 large-18 columns">
            <uc:TextBox_Animated runat="server" ID="txbAddress" />
        </div>
        <div class="small-12 medium-3 large-3 columns end">
            <asp:LinkButton runat="server" ID="lbtnSave" Text="&#10003;" CssClass="button radius secondary validationCheck" />
        </div>
        <div class="small-12 medium-3 large-3 columns end">
            <asp:LinkButton runat="server" ID="lbtnCancel" Text="&#10005;" CssClass="button radius validationCheck alert cancel" OnClientClick="return false;" /><%--secondary--%>
        </div>

        <div class="small-24 columns show-for-small-down">
            <br />
            <br />
            <br />
        </div>
        

        <div class="hide hiddenFields">
            <asp:HiddenField runat="server" ID="hfldNodeId" />
            <asp:HiddenField runat="server" ID="hfldMediaType" />
        </div>
    </asp:Panel>
</div>