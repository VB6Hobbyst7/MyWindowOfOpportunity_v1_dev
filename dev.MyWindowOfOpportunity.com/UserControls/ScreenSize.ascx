<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ScreenSize.ascx.vb" Inherits="UserControls_ScreenSize" %>

<div id="DeleteWhenLive" class=""><%--hide--%>
    <div class="text-center show-for-small-down" style="color: red; background-color: rgba(255, 0, 0, 0.15);">
        <span class="show-for-landscape">MOBILE — LANDSCAPE</span><span class="show-for-portrait">MOBILE — PORTRAIT</span>
    </div>
    <div class="text-center show-for-medium-only" style="color: green; background-color: rgba(0, 255, 0, 0.15);">
        <span class="show-for-landscape">TABLET — LANDSCAPE</span><span class="show-for-portrait">TABLET — PORTRAIT</span>
    </div>
    <div class="text-center show-for-large-up" style="color: blue; background-color: rgba(0, 0, 255, 0.15);">
        <span class="show-for-landscape">DESKTOP — LANDSCAPE</span><span class="show-for-portrait">DESKTOP — PORTRAIT</span>
    </div>
</div>



<asp:panel runat="server" ID="pnlNotes" cssclass="row collapse" Visible="false" >
    <div class="small-24 medium-12 columns" style="color:red;">
        <h6 style="color:red;">Notes: Front End</h6>
        <hr style="margin: 0;" />
        <asp:Literal runat="server" ID="ltrlNotesFrontEnd">None</asp:Literal>
    </div>
    <div class="small-24 medium-12 columns" style="color:red;">
        <h6 style="color:red;">Notes: Back End</h6>
        <hr style="margin: 0;" />
        <asp:Literal runat="server" ID="ltrlNotesBackEnd">None</asp:Literal>
    </div>
</asp:panel>