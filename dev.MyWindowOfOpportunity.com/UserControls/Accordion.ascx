<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Accordion.ascx.vb" Inherits="UserControls_Accordion" ClassName="Accordion" %>
<%@ Register Src="AccordionItem_Campaign.ascx" TagName="AccordionItem_Campaign" TagPrefix="uc" %>
<%@ Register Src="AccordionItem_TeamMember.ascx" TagName="AccordionItem_TeamMember" TagPrefix="uc" %>
<%@ Register Src="CampaignPanel.ascx" TagName="CampaignPanel" TagPrefix="uc" %>


<asp:MultiView runat="server" ID="mvAccordionType" ActiveViewIndex="0">
    <asp:View runat="server" ID="vTeamMembers">
        <%--Team Members--%>
        <asp:ListView runat="server" ID="lstviewTeamMembers">
            <LayoutTemplate>
                <ul runat="server" id="ulteamMembers" class="accordion memberData " data-view="vTeamMembers">
                    <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                </ul>
            </LayoutTemplate>
            <ItemTemplate>
                <li class="accordion-navigation">
                    <uc:AccordionItem_TeamMember runat="server" ID="ucAccTeamMember" nodeId='<%#Eval("nodeId")%>' TeamAdministrator='<%#Eval("TeamAdministrator")%>' CampaignAdministrator='<%#Eval("CampaignAdministrator")%>' CampaignMember='<%#Eval("CampaignMember")%>' />
                </li>
            </ItemTemplate>
            <EmptyDataTemplate></EmptyDataTemplate>
        </asp:ListView>
    </asp:View>

    <asp:View runat="server" ID="vCampaigns">
        <%--Campaigns--%>
        <asp:ListView runat="server" ID="lstviewCampaigns">
            <LayoutTemplate>
                <ul runat="server" id="ulAccordion" class="accordion campaignData " data-view="vCampaigns">
                    <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                </ul>
            </LayoutTemplate>
            <ItemTemplate>
                <li class="accordion-navigation">
                    <uc:AccordionItem_Campaign runat="server" ID="ucAccCampaign" nodeId='<%# Eval("Key") %>' isActive='<%# Eval("Value") %>' />
                </li>
            </ItemTemplate>
            <EmptyDataTemplate></EmptyDataTemplate>
        </asp:ListView>
    </asp:View>

    <asp:View runat="server" ID="vCampaignCategory">
        <ul runat="server" id="ulCampaignCategory" class="accordion campaignData " data-view="vCampaignCategory">
            <li class="accordion-navigation">
                <asp:HyperLink runat="server" ID="hlnkHandle_Category">
                    <div class="row campaign">
                        <div class="small-24 columns accTopBanner">
                            &nbsp;
                        </div>
                        <div class="small-22 medium-14 large-7 columns accName">
                            <%--<asp:Image runat="server" ID="img" ImageUrl="~/Images/Icons/arts.png" />--%>
                            <asp:Literal runat="server" ID="ltrlCategoryName" />
                        </div>
                        <div class="small-2 medium-2 large-1 columns accClosed end text-right">
                            ▲
                        </div>
                        <div class="small-2 medium-2 large-1 columns accOpen end text-right">
                            ▼
                        </div>
                    </div>
                </asp:HyperLink>
                <asp:Panel runat="server" ID="pnlContent_" CssClass="content" ClientIDMode="Static">
                    <div class="row campaign">
                        <div class="small-4 medium-2 large-1 columns">
                            &nbsp;
                        </div>
                        <div class="small-16 medium-20 large-22 columns CampaignPanel">

                            <asp:ListView runat="server" ID="lstviewCampaignPanels">
                                <LayoutTemplate>
                                    <div class="row" data-equalizer="cheer">
                                        <div data-equalizer="title">
                                            <div data-equalizer="statistics">
                                                <section class="regular slider" data-equalizer="description">
                                                    <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                                                </section>
                                            </div>
                                        </div>
                                    </div>

                                </LayoutTemplate>
                                <ItemTemplate>
                                    <uc:CampaignPanel runat="server" ID="ucCampaignPanel04" campaignSummary='<%# Container.DataItem %>' />
                                </ItemTemplate>
                                <EmptyDataTemplate></EmptyDataTemplate>
                            </asp:ListView>

                        </div>
                        <div class="small-4 medium-2 large-1 columns">
                            &nbsp;
                        </div>
                    </div>
                </asp:Panel>
            </li>
        </ul>
    </asp:View>
</asp:MultiView>

<%--<asp:GridView runat="server" ID="gv" />  --%>  