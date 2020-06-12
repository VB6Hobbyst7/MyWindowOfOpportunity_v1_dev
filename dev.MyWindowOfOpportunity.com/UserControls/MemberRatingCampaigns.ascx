<%@ Control Language="VB" AutoEventWireup="false" CodeFile="MemberRatingCampaigns.ascx.vb" Inherits="UserControls_MemberRatingCampaigns" %>




<asp:Repeater ID="rptRatingCampaign" runat="server">
    <HeaderTemplate>
        <br />
        <div class="row hide-for-small-down">
            <div class="small-4 medium-6 large-3 columns">
                <strong>Date</strong>
            </div>
            <div class="small-4 medium-8 large-4 columns">
                <strong>Your Rating</strong>
            </div>
            <div class="small-6 medium-10 large-6 columns">
                <strong>Campaign</strong>
            </div>
            <div class="hide-for-medium-down large-11 columns">
                <strong>Your Comment</strong>
            </div>
        </div>
    </HeaderTemplate>
    <ItemTemplate>
        <div class="row data">
            <div class="hide-for-small-down medium-6 large-3 columns">
                <%#Eval("RatingDate")%>
            </div>
            <div class="small-24 medium-8 large-4 columns rating" data-title="Your Rating">
                <asp:HiddenField ID="hdnMemberRating" runat="server" Value='<%#Eval("Rating")%>' />
                <div id="rateYo" runat="server"></div>
            </div>
            <div class="small-24 medium-10 large-6 columns">
                <asp:HyperLink runat="server" ID="hlnkCampaign" NavigateUrl='<%#Eval("NavigateUrl")%>' CssClass="hlnk"><%#Eval("CampaignName")%></asp:HyperLink>
            </div>
            <div class="hide-for-medium-down large-11 columns">
                <%#Eval("ReviewDetail")%>
            </div>
        </div>

    </ItemTemplate>
</asp:Repeater>
