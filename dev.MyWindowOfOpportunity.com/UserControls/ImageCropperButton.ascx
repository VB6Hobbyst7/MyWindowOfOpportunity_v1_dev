<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ImageCropperButton.ascx.vb" Inherits="UserControls_ImageCropperButton" %>


<li class="crop " runat="server" id="li"> 
    <div>
        <span class="crop-name crop-text ng-binding"><asp:Literal runat="server" ID="ltrlCropName" /></span><br />
        <span class="crop-size crop-text ng-binding"><asp:Literal runat="server" ID="ltrlCropSize" /></span>
    </div>
    <div class="crop-thumbnail-container">
        <asp:Image runat="server" ID="imgCrop" />
    </div>

    
    <input type="hidden" runat="server" value="0" id="hfldCropWidth" class="hfldCropWidth" />
    <input type="hidden" runat="server" value="0" id="hfldCropHeight" class="hfldCropHeight" />
    <input type="hidden" runat="server" value="" id="hfldCropAlias" class="hfldCropAlias" />
</li>