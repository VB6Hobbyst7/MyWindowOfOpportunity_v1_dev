<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ImageCropper.ascx.vb" Inherits="UserControls_ImageCropper" %>
<%@ Register Src="ImageCropperButton.ascx" TagName="ImageCropperButton" TagPrefix="uc" %>
<%@ Register Src="ImageCropPanel.ascx" TagName="ImageCropPanel" TagPrefix="uc" %>
<%@ Register Src="/UserControls/AlertMsg.ascx" TagName="AlertMsg" TagPrefix="uc" %>


<asp:Panel runat="server" ID="pnlImageCropper" CssClass="pnlImageCropper hide">

    <%--Data Row--%>
    <fieldset class="outlinePanel imgStats">
        <legend>Image Stats</legend>
        <div class="row">
            <div class="small-24 medium-22 medium-push-1 large-22 large-push-1 columns">
                <div class="row">
                    <div class="small-12 medium-4 large-2 columns bold">
                        Width
                    </div>
                    <div class="small-12 medium-4 medium-push-8 large-2 large-push-10 columns bold">
                        Size
                    </div>

                    <div class="small-12 medium-8 medium-pull-4 large-6 large-pull-2 columns ">
                        <asp:Label runat="server" ID="lblWidth" />
                        pixels
                    </div>
                    <div class="small-12 medium-8 large-6 large-push-4 columns end">
                        <asp:Label runat="server" ID="lblSize" />
                        kb
                    </div>

                </div>
                <hr />
                <div class="row">
                    <div class="small-12 medium-4 large-2 columns bold">
                        Height
                    </div>
                    <div class="small-12 medium-4 medium-push-8 large-2 large-push-10 columns bold">
                        Type
                    </div>

                    <div class="small-12 medium-8 medium-pull-4 large-6 large-pull-2 columns">
                        <asp:Label runat="server" ID="lblHeight" />
                        pixels
                    </div>
                    <div class="small-12 medium-8 large-6 large-push-4 columns end">
                        <asp:Label runat="server" ID="lblType" />
                    </div>
                </div>                
            </div>
        </div>
    </fieldset>
    <br />

    <%--Crop Image Panels--%>
    <div class="row">
        <div class="small-24 medium-24 large-4 columns">
            <h5>Editable Image</h5>
        </div>
        <div class="small-24 medium-24 large-10 columns">
            <div class="show-for-small-down">
                <uc:AlertMsg runat="server" MessageType="Alert" AlertMsg="Be aware that updating crops on a mobile device is not fully functional as of yet.  This will be corrected in the weeks to come." AdditionalText="<hr />Use at your own risk." />
            </div>            

            <%--Primary Image--%>
            <div class="cropPnl fullImg active">
                <div class="cropper-gravity" src="imageSrc" center="model.value.focalPoint">
                    <div class="gravity-container">
                        <div class="viewport">
                            <asp:Image runat="server" ID="imgMain" CssClass="mainImg" />
                        </div>
                    </div>
                </div>
            </div>

            <asp:ListView runat="server" ID="lstviewImageCropPanels">
                <LayoutTemplate>
                    <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                </LayoutTemplate>
                <ItemTemplate>
                    <uc:ImageCropPanel runat="server" ID="ImageCropPanel" mediaProperties='<%#Eval("mediaProperties")%>' cropAlias='<%#Eval("cropAlias")%>' cropWidth='<%#Eval("cropWidth")%>' cropHeight='<%#Eval("cropHeight")%>' />
                </ItemTemplate>
                <EmptyDataTemplate>
                </EmptyDataTemplate>
            </asp:ListView>

        </div>


        <%--Crop Buttons--%>
        <div class="small-24 medium-24 large-10 columns">
            <h5>Crops</h5>
            <p>Your images can be used in many places throughout your campaign.  Some sections require a specific image size.  Select to edit your image’s crop sizes for each section it will be used.</p>
            <asp:ListView runat="server" ID="lstviewImageCropperButtons">
                <LayoutTemplate>
                    <ul class="small-block-grid-1 medium-block-grid-1 large-block-grid-3 cropBtnList">
                        <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                    </ul>
                </LayoutTemplate>
                <ItemTemplate>
                    <uc:ImageCropperButton runat="server" mediaProperties='<%#Eval("mediaProperties")%>' cropAlias='<%#Eval("cropAlias")%>' cropWidth='<%#Eval("cropWidth")%>' cropHeight='<%#Eval("cropHeight")%>' />
                </ItemTemplate>
                <EmptyDataTemplate>
                </EmptyDataTemplate>
            </asp:ListView>
        </div>
    </div>
    <br />
    <br />

    <%--Delete, Cancel & Save--%>
    <div class="row">
        <div class="small-24 medium-4 large-10 large-push-4 columns ">
            <a href="#" class="deleteImage">
                <img src="/Images/FileManagement/trashCan_gray.gif" alt="Delete Image" />
                Delete
            </a>
        </div>
        <div class="small-24 medium-20 large-10 large-push-1 columns small-text-center medium-text-right noMargin end">
            <a class="shikobaButton nextPg button--shikoba green small button--round-s button--border-thin cancelBtn alert">
                <i class="button__icon fi-x size-24"></i>
                <span>Cancel</span>
            </a>
            <asp:LinkButton runat="server" ID="lbtnSave" ValidationGroup="vg01" CssClass="shikobaButton nextPg button--shikoba green small button--round-s button--border-thin saveBtn">
                <i class="button__icon fi-next size-24"></i>
                <span>Save</span>
            </asp:LinkButton>
        </div>
    </div>


    <input type="hidden" runat="server" value="0" id="hfldMediaId" class="hfldMediaId" />
    <input type="hidden" runat="server" value="" id="hfldSrc" class="hfldSrc" />
    <input type="hidden" runat="server" value="0" id="hfldImageWidth" class="hfldImageWidth" />
    <input type="hidden" runat="server" value="0" id="hfldImageHeight" class="hfldImageHeight" />
    <input type="hidden" runat="server" value="" id="hfldcropTypes" class="hfldcropTypes" />
    <input type="hidden" runat="server" value="" id="hfldFromDb" class="hfldFromDb" />

</asp:Panel>
