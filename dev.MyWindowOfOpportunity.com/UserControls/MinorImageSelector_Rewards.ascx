<%@ Control Language="VB" AutoEventWireup="false" CodeFile="MinorImageSelector_Rewards.ascx.vb" Inherits="UserControls_MinorImageSelector_Rewards" %>

<asp:Panel runat="server" ID="pnl" CssClass="minorImageSelector">
    <asp:MultiView runat="server" ID="mvFeaturedImageSelector" ActiveViewIndex="0">


            <asp:View runat="server" ID="vDisplayImg">
                <div class="displayPnl">
                    <div class="row collapse">
                        <div class="columns">
                            <asp:PlaceHolder runat="server" ID="phAlertMsg" />
                        </div>
                    </div>

                    <div class="selectImageBtn left">+</div>

                    <asp:Panel runat="server" ID="pnlSelectedImage" CssClass="pnlSelectedImage left">
                        <asp:Image runat="server" ID="selectedImage" CssClass="selectedImage" />
                        <asp:Label runat="server" ID="lblDelete" CssClass="delete" Text="X" />
                    </asp:Panel>
                </div>
                <br />
                <br />
                
                <div class="hide hiddenFields">
                    <input type="hidden" runat="server" value="-1" id="hfldSelectedMediaId" class="hfldSelectedMediaId" />
                    <input type="hidden" runat="server" value="-1" id="hfldCurrentRewardId" class="hfldCurrentRewardId" />
                    <input type="hidden" runat="server" value="0" id="hfldMediaId" class="hfldMediaId" />
                </div>

            </asp:View>



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
                        <%--<asp:Button runat="server" ID="btnSelectBanner" CssClass="hide btnSelectBanner" />
                        <input type="hidden" runat="server" value="-1" id="hfldSelectedMediaId" class="hfldSelectedMediaId" />--%>
                        <input type="hidden" runat="server" value="-1" id="hfldCurrentCampaignId" class="hfldCurrentCampaignId" />
                    </div>
            

                </div>
            </asp:View>


        </asp:MultiView>
</asp:Panel>
