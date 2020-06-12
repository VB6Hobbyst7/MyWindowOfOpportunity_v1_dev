<%@ Control Language="VB" AutoEventWireup="false" CodeFile="editTab_Images.ascx.vb" Inherits="UserControls_editTab_Images" %>
<%@ Register Src="/UserControls/ImageCropper.ascx" TagName="ImageCropper" TagPrefix="uc" %>


<a name="topOfImgManagement" class="hiddenAnchor"></a>
<div class="pnlImageManagement">
    <asp:HiddenField runat="server" ID="hfldTabName" />
    <asp:HiddenField runat="server" ID="hfldThisNodeId" />

    <div class="row">
        <div class="columns">
            <h2>Image Management</h2>
            <p class="twoColumns hide-for-small-down">What is the first thing people look at when viewing a web page?  <strong>IMAGES!</strong>  You can have the greatest idea ever, but if you do not have the proper imagery to help others see your dream than you will quickly lose your funding to those who do.
                <br /><strong>To use your image library:</strong>
                <br />&#9672; Drag & drop your images onto the screen to add your images to your library.
                <br />&#9672; Click on the image to edit your image's crops as needed throughout your campaign.</p>
            
            <p class="show-for-small-down">What is the first thing people look at when viewing a web page?  <strong>IMAGES!</strong>  You can have the greatest idea ever, but if you do not have the proper imagery to help others see your dream than you will quickly lose your funding to those who do.
                <br /><br /><strong>To use your image library:</strong>
                <br />&#9672; Drag & drop your images onto the screen to add your images to your library.
                <br />&#9672; Click on the image to edit your image's crops as needed throughout your campaign.</p>
            
            <div><span class="smaller">Important: An image must be uploaded before it can be used in your campaign.</span></div>
        </div>
    </div>

    <div class="imageListAndUploadPnl">
        <div class="row pnlUpload dropzone">
            <br />
            <div class="small-24 medium-12 medium-push-6 large-8 large-push-8 columns end text-center pnlUploadImage">
                <img alt="Upload Image icon" title="Upload Image" src="/Images/FileManagement/upload.gif" />
                <h3>Drag & Drop Image Here</h3>
            </div>
            <div class="large-24 columns text-center pnlUploadLink">
                <h5>- or click here to upload -</h5>
            </div>
        </div>
        <%--<asp:GridView runat="server" ID="gv" />--%>
        <br />
        <div class="row">
            <div class="small-24 columns">
                <asp:ListView runat="server" ID="lstviewImageLibrary">
                    <LayoutTemplate>
                        <ul class="small-block-grid-1 medium-block-grid-3 large-block-grid-6" data-equalizer>
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

</div>
