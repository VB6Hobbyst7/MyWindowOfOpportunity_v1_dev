<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PhaseMeter.ascx.vb" Inherits="UserControls_PhaseMeter" %>


<asp:PlaceHolder runat="server" ID="phPhaseMeter">
    <asp:Panel runat="server" ID="pnlPhaseMeter" CssClass="text-center phaseMeter ">
        <span class="title">PHASE&nbsp;&nbsp;&nbsp;</span>

        <asp:Image runat="server" ID="imgPhase01" ImageUrl="~/Images/transparent.gif" CssClass="phaseIcon inactive" Visible="false" />
        <asp:Image runat="server" ID="imgPhase02" ImageUrl="~/Images/transparent.gif" CssClass="phaseIcon inactive" Visible="false" />
        <asp:Image runat="server" ID="imgPhase03" ImageUrl="~/Images/transparent.gif" CssClass="phaseIcon inactive" Visible="false" />
        |
        <asp:Image runat="server" ID="imgComplete" ImageUrl="~/Images/transparent.gif" CssClass="phaseIcon inactive" />
    </asp:Panel>
    
    <asp:HiddenField runat="server" ID="hfldPhaseCount" />
    <asp:HiddenField runat="server" ID="hfldPhaseStatus" />
</asp:PlaceHolder>