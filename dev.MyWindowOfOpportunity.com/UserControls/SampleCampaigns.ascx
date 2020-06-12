<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SampleCampaigns.ascx.vb" Inherits="UserControls_Home_SampleCampaigns" %>
<%@ Register Src="CampaignPanel.ascx" TagName="CampaignPanel" TagPrefix="uc" %>


<div class="trending">
    <h2 class="small-text-center large-text-left">OUR TOP-PICKED CAMPAIGNS</h2>
    <%--<h2 class="small-text-center large-text-left">SAMPLE CAMPAIGNS</h2>
    <h2 class="small-text-center large-text-left">TRENDING NOW</h2>--%>
    <br class="show-for-medium-only" />
    <asp:ListView runat="server" ID="lstviewCampaignPanels">
        <LayoutTemplate>
            <div class="row" data-equalizer="cheer">
                <div data-equalizer="title">
                    <div data-equalizer="statistics">
                        <ul class="small-block-grid-1 medium-block-grid-3 large-block-grid-3 slickSlider" data-equalizer="description">
                            <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                        </ul>
                    </div>
                </div>
            </div>
        </LayoutTemplate>
        <ItemTemplate>
            <li>
                <uc:CampaignPanel runat="server" ID="ucCampaignPanel04" campaignSummary='<%# Container.DataItem %>' />
            </li>
        </ItemTemplate>
        <EmptyDataTemplate></EmptyDataTemplate>
    </asp:ListView>
    <br />
</div>
