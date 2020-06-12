<%@ Control Language="VB" AutoEventWireup="false" CodeFile="FeaturedImageSelector.ascx.vb" Inherits="UserControls_FeaturedImageSelector" %>
<%@ Reference Control="AlertMsg.ascx" %>


<div class="featuredImg_TopBanner small-text-center medium-text-left">
<asp:MultiView runat="server" ID="mvFeaturedImageSelector" ActiveViewIndex="0">
    <asp:View runat="server" ID="vShowImg">
        <div class="row">
            <div class="small-6 small-push-9 medium-6 medium-push-0 large-6 columns">
                <div class="selectImageBtn left">+</div>
                <input type="hidden" runat="server" value="" id="hfldPropertyName" class="hfldPropertyName" />
            </div>  
                
            <div class="small-24 show-for-small-down columns">
                <br />
            </div>

            <div class="small-24 medium-18 large-18 columns">
                <asp:Panel runat="server" ID="pnlSelectedImage" CssClass="pnlSelectedImage">
                    <asp:Image runat="server" ID="selectedImage" CssClass="selectedImage" />
                    <asp:Label runat="server" ID="lblTitle" CssClass="title" />
                    <asp:Label runat="server" ID="lblDelete" CssClass="delete" Text="X" />
                    <input type="hidden" runat="server" value="false" id="hfldRemoveSelectedImg" class="hfldRemoveSelectedImg" />
                </asp:Panel>
            </div>            
        </div>

        <div class="row collapse">
            <div class="columns">
                <asp:PlaceHolder runat="server" ID="phAlertMsg" />
            </div>  
        </div>
    </asp:View>

    <asp:View runat="server" ID="vSelectImg"> 
        
        <div class="row">
            <div class="columns">
                <h2>Content Management</h2>
                <h6 runat="server" id="h6Campaign">Select which image you want to use as the banner for your campaign.</h6>
                <h6 runat="server" id="h6Team" visible="false" >Select which image you want to use as your team's image.</h6>
            </div>
        </div>
   
        <br />
        <div class="row">
            <div class="small-24 columns">

                <asp:ListView runat="server" ID="lstviewImageLibrary">
                    <LayoutTemplate>
                        <ul class="large-block-grid-4 bannerOptions">
                            <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                        </ul>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <li  mediaid='<%#Eval("mediaId")%>'>                        
                            <img class="bannerOption" src='<%#Eval("imgUrl")%>' alt='<%#Eval("imgName")%>' title='<%#Eval("imgName")%>' />
                            <i class="button__icon fi-check"></i>
                        </li>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        <div>No images have been added to your library yet.</div>
                    </EmptyDataTemplate>
                </asp:ListView>

            </div>
        </div>       
        <div class="row">
            <div class="large-4 large-push-20 columns">
                <a class="shikobaButton nextPg button--shikoba green small button--round-s button--border-thin cancelBtn">
                    <i class="button__icon fi-previous size-24"></i>
                    <span>Cancel</span>
                </a>
            </div>
        </div>


        
        <div class="hide hiddenFields">
            <asp:Button runat="server" ID="btnSelectBanner" CssClass="hide btnSelectBanner" />
            <input type="hidden" runat="server" value="-1" id="hfldSelectedMediaId" class="hfldSelectedMediaId" />
            <input type="hidden" runat="server" value="-1" id="hfldCurrentNodeId" class="hfldCurrentNodeId" />
            <input type="hidden" runat="server" value="-1" id="hfldSelectedPropertyName" class="hfldSelectedPropertyName" />
        </div>


    </asp:View>
</asp:MultiView>
</div>
