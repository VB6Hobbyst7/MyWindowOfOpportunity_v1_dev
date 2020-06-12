<%@ Control Language="VB" AutoEventWireup="false" CodeFile="TeamImageMngr.ascx.vb" Inherits="UserControls_TeamImageMngr" %>
<%@ Register Src="/UserControls/ImageCropper.ascx" TagName="ImageCropper" TagPrefix="uc" %>


<a name="topOfImgManagement" class="hiddenAnchor"></a>
<div class="pnlImageManagement">
    <asp:HiddenField runat="server" ID="hfldTabName" />
    <asp:HiddenField runat="server" ID="hfldThisNodeId" />

    <div class="row">
        <div class="columns">
            <h3>Image Management</h3>
            <p>Store your team's images by dragging & dropping them here.</p>            
        </div>
    </div>

    <div class="imageListAndUploadPnl">
        <div class="row pnlUpload dropzone">
            <br />
            <div class="medium-12 medium-push-6 large-8 large-push-8 columns end text-center pnlUploadImage">
                <img alt="Upload Image icon" title="Upload Image" src="/Images/FileManagement/upload.gif" />
                <h3>Drag & Drop Image Here</h3>
            </div>
            <div class="large-24 columns text-center pnlUploadLink">
                <h5>- or click here to upload -</h5>
            </div>
        </div>

        <%--<div class="row">
            <div class="small-24 columns">
                <hr />
            </div>
        </div>--%>
        <br />
        <div class="row">
            <div class="small-24 columns">
                <asp:ListView runat="server" ID="lstviewImageLibrary">
                    <LayoutTemplate>
                        <ul class="small-block-grid-1 medium-block-grid-4 large-block-grid-6" data-equalizer>
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
                    <asp:Button runat="server" ID="btnDeleteImage" ClientIDMode="Static" CssClass="hide" />
                    <asp:Button runat="server" ID="btnRefreshPage" ClientIDMode="Static" CssClass="hide" />
                    <asp:HiddenField runat="server" ID="hfldMediaIdToDelete" ClientIDMode="Static" />
                    <asp:HiddenField runat="server" ID="hfldMediaFolder" ClientIDMode="Static" />
                    <asp:HiddenField runat="server" ID="hfldCurrentMediaId" ClientIDMode="Static" Value="-1" />
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


    <asp:GridView runat="server" ID="gv" />
</div>
