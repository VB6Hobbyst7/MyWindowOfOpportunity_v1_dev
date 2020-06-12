<%@ Control Language="VB" AutoEventWireup="false" CodeFile="UserImageMngr.ascx.vb" Inherits="UserControls_UserImageMngr" %>
<%@ Register Src="/UserControls/ImageCropper.ascx" TagName="ImageCropper" TagPrefix="uc" %>



<a name="topOfImgManagement" class="hiddenAnchor"></a>
<div class="pnlImageManagement">
    <div class="imageListAndUploadPnl">
        <div class="row">
            <div class="small-24 medium-16 large-16 columns">
                <div class="row pnlUpload profileImg dropzone">
                    <br />
                    <div class="small-24 medium-12 large-8 small-push-0 medium-push-6 large-push-8 columns end text-center pnlUploadImage">
                        <img alt="Upload Image icon" title="Upload Image" src="/Images/FileManagement/upload.gif" />
                        <h3>Drag & Drop Image Here</h3>
                    </div>
                    <div class="small-24 medium-24 large-24 columns text-center pnlUploadLink">
                        <h5>- or click here to upload -</h5>
                    </div>
                </div>
            </div>
            <div class="small-24 medium-8 large-8 columns">
                <div class="row">
                    <asp:ListView runat="server" ID="lstviewImageLibrary">
                        <LayoutTemplate>
                            <br />
                            <br />
                            <ul class="large-block-grid-2" data-equalizer>
                                <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                            </ul>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <li data-equalizer-watch>
                                <i></i>
                                <img src='<%#Eval("imgUrl")%>' class="imgFeatured" alt='<%#Eval("imgName")%>' title='<%#Eval("imgName")%>' />
                                <div class="btnSelectImg" mediaid='<%#Eval("mediaId")%>'><%#Eval("imgName")%></div>
                                <img src="/Images/FileManagement/trashCan_red.gif" class="trashcan" mediaid='<%#Eval("mediaId")%>' />
                            </li>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            <div>No images have been added to your library yet.</div>
                        </EmptyDataTemplate>
                    </asp:ListView>

                    <%--Hidden buttons and fields--%>
                    <div class="hide hiddenFields">
                        <asp:HiddenField runat="server" ID="hfldTabName" />
                        <asp:HiddenField runat="server" ID="hfldThisNodeId" />
                        <asp:HiddenField runat="server" ID="hfldMediaIdToDelete" ClientIDMode="Static" />
                        <asp:HiddenField runat="server" ID="hfldMediaFolder" ClientIDMode="Static" />
                        <asp:HiddenField runat="server" ID="hfldCurrentMediaId" ClientIDMode="Static" Value="-1" />
                        <%--<asp:Button runat="server" ID="btnDeleteImage" ClientIDMode="Static" CssClass="hide" />
                        <asp:Button runat="server" ID="btnRefreshPage" ClientIDMode="Static" CssClass="hide" />--%>
                        <input type="button" runat="server" id="btnDeleteImage" class="btnDeleteImage" />
                        <input type="button" runat="server" id="btnRefreshPage" class="btnRefreshPage" />
                    </div>
                    
                </div>
            </div>
        </div>
    </div>
    <asp:ListView runat="server" ID="lstViewImageCroppers">
        <LayoutTemplate>
            <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
        </LayoutTemplate>
        <ItemTemplate>
            <uc:ImageCropper runat="server" mediaId='<%#Eval("mediaId")%>' cropTypes='<%#Eval("cropTypes")%>' />
        </ItemTemplate>
        <EmptyDataTemplate></EmptyDataTemplate>
    </asp:ListView>
</div>
