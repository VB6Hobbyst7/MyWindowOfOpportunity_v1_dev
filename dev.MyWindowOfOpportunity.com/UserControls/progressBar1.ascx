<%@ Control Language="VB" AutoEventWireup="false" CodeFile="progressBar1.ascx.vb" Inherits="UserControls_progressBar" %>

<div class="summaryProgressbar">
    <asp:Panel runat="server" ID="pnlMarker" CssClass="pnlMarker" Visible="false">
        <div class="percentageMarker arrow_box">
            <div class="score">0%</div>
        </div>
    </asp:Panel>
    <asp:Panel runat="server" ID="outerPanel" CssClass="outerPanel">
        <asp:Panel runat="server" ID="innerPanel" CssClass="innerPanel"/>
          <asp:HiddenField runat="server" ID="HiddenField2"  Value="" />
            </asp:Panel>
    <asp:HiddenField runat="server" ID="hfldPercentage" Value="" />
    
</div>
<%--<script>
    $(document).ready(function () {
        var txtDonation = $('#<%=HiddenField2.ClientID %>');
        alert(txtDonation.val());
       // $("#hfldPercentage").css('width','hfldPercentage');
       
        });
    
</script>--%>