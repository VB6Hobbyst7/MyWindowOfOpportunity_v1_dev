<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ImageCropPanel.ascx.vb" Inherits="UserControls_ImageCropPanel" %>


<asp:Panel runat="server" ID="pnlCrop" CssClass="cropPnl">
    <div class="ImageCropPanel">

        <i class="fa fa-times-circle-o fa-2x closeCropPnl" aria-hidden="true"></i>

        <div>
            <div class="cropper editor">
                <div class="crop-container">
                    <asp:Panel runat="server" ID="pnlViewport" CssClass="viewport" Width="468">
                        <asp:Image runat="server" ID="imgCropped" CssClass="mainImg" />
                    </asp:Panel>
                </div>

                <div class="row">
                    <div class="columns">
                        <div class="range-slider" data-slider data-options="start: 0; end: 2; step: 0.01;">
                            <span class="range-slider-handle" role="slider" tabindex="0"></span>
                            <span class="range-slider-active-segment"></span>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <%--<a class="btn btn-link red resetLink alink">Reset</a>--%>
    </div>
    
    <br />
    <div class="row cropDataPnl">    
        <input type="hidden" runat="server" value="0" id="hfldCropAlias" class="hfldCropAlias" />
        <input type="hidden" runat="server" value="0" id="hfldViewportWidth" class="hfldViewportWidth" />
        <input type="hidden" runat="server" value="0" id="hfldViewportHeight" class="hfldViewportHeight" />
        <input type="hidden" runat="server" value="0" id="hfldLeft" class="hfldLeft" />
        <input type="hidden" runat="server" value="0" id="hfldTop" class="hfldTop" />
        <input type="hidden" runat="server" value="0" id="hfldImgWidth" class="hfldImgWidth" />
        <input type="hidden" runat="server" value="0" id="hfldImgHeight" class="hfldImgHeight" />    
        <input type="hidden" runat="server" value="0" id="hfldCropWidthy" class="hfldCropWidthy" />
        <input type="hidden" runat="server" value="0" id="hfldCropHeight" class="hfldCropHeight" />
        <input type="hidden" runat="server" value="0" id="hfldX1" class="hfldX1" />
        <input type="hidden" runat="server" value="0" id="hfldY1" class="hfldY1" />
        <input type="hidden" runat="server" value="0" id="hfldX2" class="hfldX2" />
        <input type="hidden" runat="server" value="0" id="hfldY2" class="hfldY2" />
    </div>
</asp:Panel>