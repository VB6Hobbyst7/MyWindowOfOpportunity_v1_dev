<%@ Control Language="VB" AutoEventWireup="false" CodeFile="progressBar.ascx.vb" Inherits="UserControls_progressBar" %>

<div class="summaryProgressbar">
    <asp:Panel runat="server" ID="pnlMarker" CssClass="pnlMarker" Visible="false">
        <div class="percentageMarker arrow_box">
            <div class="score">0%</div>
        </div>
    </asp:Panel>
    <asp:Panel runat="server" ID="outerPanel" CssClass="outerPanel">
        <asp:Panel runat="server" ID="innerPanel" CssClass="innerPanel " />
    </asp:Panel>
    <asp:HiddenField runat="server" ID="hfldPercentage" Value="" />
</div>
