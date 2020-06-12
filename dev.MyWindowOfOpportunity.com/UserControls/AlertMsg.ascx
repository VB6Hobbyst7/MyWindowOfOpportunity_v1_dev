<%@ Control Language="VB" AutoEventWireup="false" CodeFile="AlertMsg.ascx.vb" Inherits="UserControls_AlertMsg" %>



<asp:Panel runat="server" ID="pnlOuterRow" CssClass="row alertBoxRow">
    <div class="columns small-centered">
        <asp:Panel runat="server" ID="pnlAlertBox" data-alert CssClass="alert-box row collapse">
            <asp:Panel runat="server" id="pnlIcon" CssClass="hide-for-medium-down medium-2 large-2 columns">
                <i id="iFi" runat="server"></i>
            </asp:Panel>      
            <div class="small-20 small-push-2 medium-20 medium-push-0 large-20 columns">
                <div class="initialText"><asp:Literal runat="server" ID="ltrlMsg" /></div>            
                <div class="additionalText"><asp:Literal runat="server" ID="ltrlAdditionalText" /></div>
            </div>
            <div class="small-2 medium-2 large-2 columns text-right">
                <a href="#" class="close">&CircleTimes;</a>
            </div>
        </asp:Panel>
    </div>
    <br />
</asp:Panel>

