<%@ Control Language="VB" AutoEventWireup="false" CodeFile="MinorImageSelector_TeamMembers.ascx.vb" Inherits="UserControls_MinorImageSelector_TeamMembers" %>


<asp:Panel runat="server" ID="pnl" CssClass="minorImageSelector">
    <%--<asp:MultiView runat="server" ID="mvFeaturedImageSelector" ActiveViewIndex="0">


            <asp:View runat="server" ID="vDisplayImg">--%>
    <div class="displayPnl">
        <div class="row collapse">
            <div class="columns">
                <asp:PlaceHolder runat="server" ID="phAlertMsg" />
            </div>
        </div>



        <div class="selectImageBtn left dropzone_member">+</div>

        
        
        <asp:Panel runat="server" ID="pnlSelectedImage" CssClass="pnlSelectedImage left">
            <asp:Image runat="server" ID="selectedImage" CssClass="selectedImage" />
            <asp:Label runat="server" ID="lblDelete" CssClass="delete" Text="X" />
        </asp:Panel>

        <asp:Label runat="server" ID="lblImgMsg" CssClass="lblImgMsg" />
    </div>
    <br />
    <br />

    <div class="hide hiddenFields">
        <input type="hidden" runat="server" value="-1" id="hfldTeamMemberId" class="hfldTeamMemberId" />
        <input type="hidden" runat="server" value="0" id="hfldPhotoId" class="hfldPhotoId" />
        <input type="hidden" runat="server" value="false" id="hfldRemoveSelectedImg" class="hfldRemoveSelectedImg" />
        <input type="hidden" runat="server" value="false" id="hfldUpdateSelectedImg" class="hfldUpdateSelectedImg" />
    </div>

    <%--</asp:View>



            <asp:View runat="server" ID="vSelectImg">
                <div class="selectPnl">
                    <div class="row">
                        <div class="columns">
                            <h2>Image Selector</h2>
                            <h6>Select which image you want to use:</h6>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="small-24 columns">

                            <asp:ListView runat="server" ID="lstviewImageLibrary">
                                <LayoutTemplate>
                                    <ul class="large-block-grid-4 imagesToSelectFrom">
                                        <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                                    </ul>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <li mediaid='<%#Eval("mediaId")%>'>
                                        <img class="imgOption" src='<%#Eval("imgUrl")%>' alt='<%#Eval("imgName")%>' title='<%#Eval("imgName")%>' />
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
                        <div class="large-6 large-push-18 columns">
                            <a class="shikobaButton nextPg button--shikoba green small button--round-s button--border-thin cancelBtn">
                                <i class="button__icon fi-previous size-24"></i>
                                <span>Cancel</span>
                            </a>
                        </div>
                    </div>



                    <div class="hide hiddenFields">
                        <input type="hidden" runat="server" value="-1" id="hfldCurrentCampaignId" class="hfldCurrentCampaignId" />
                    </div>
            

                </div>
            </asp:View>


        </asp:MultiView>--%>

</asp:Panel>


