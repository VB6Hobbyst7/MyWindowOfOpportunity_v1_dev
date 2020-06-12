<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PhaseEntry.ascx.vb" Inherits="UserControls_PhaseEntry" %>
<%@ Register Src="~/UserControls/progressBar.ascx" TagName="progressBar" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/TextBox_Animated.ascx" TagName="TextBox_Animated" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/RatingSummary.ascx" TagName="RatingSummary" TagPrefix="uc" %>
<%@ Reference Control="AlertMsg.ascx" %>


<%--<asp:GridView runat="server" ID="gv" />--%>

<div class="phaseEntry">
    <asp:Panel runat="server" ID="pnlPhaseEntry" type="phaseEntry" CssClass="row hide pnlPhaseEntry">
        <div class="small-24 medium-24 large-10 columns">
            <div id="divPhase" runat="server">
                <uc:TextBox_Animated runat="server" ID="txbTitle" Title="Phase Title" />
                <uc:TextBox_Animated runat="server" ID="txbGoal" Title="Phase Goal" isCurrency="true" />
                
                <div class="row">
                    <div class="small-22 small-push-1 columns small-text-center large-text-right minorNote">
                        Important- Phase goals cannot be edited once the phase is active.  So ensure you have the correct goal beforehand.
                    </div>
                </div>                
            </div>

            <div id="divDiscovery" runat="server" visible="false" class="campaign-rating">
                <input type="hidden" runat="server" id="hdnRatingArray" class="hdnRatingArray" />
                <span class="campaign-title">Summary</span>
                <div id="Summaries">
                    <uc:RatingSummary runat="server" ID="ucRatingSummary" />
                </div>
            </div>
        </div>
        <div class="small- medium- large-14 columns">
            <asp:Panel runat="server" ID="pnlStatusFields" CssClass="row">
                <div class="small-24 medium- medium-push- large-11 large-push-1 columns publishStatus small-text-center large-text-left">
                    <asp:PlaceHolder runat="server" ID="phPublished">Published: 
                        <div class="switch radius">
                            <asp:CheckBox runat="server" ID="cbPublished" />
                            &nbsp;
                            <label runat="server" id="lblPublished">
                                <span class="switch-on">Yes</span>
                                <span class="switch-off">No</span>
                            </label>
                        </div>
                        <asp:Panel runat="server" ID="pnlNotes" CssClass="notes" Visible="false">
                            Select only if the previous phase is published.
                        </asp:Panel>
                        
                        <br class="show-for-medium-down" />
                        <br class="show-for-medium-down">
                    </asp:PlaceHolder>
                    <asp:PlaceHolder runat="server" ID="phPrimaryNotes" Visible="false">
                        <p class="secondary small-text-center large-text-left"><strong>This phase is always published.</strong></p>
                    </asp:PlaceHolder>
                </div>
                <div class="small-24 medium- medium-push- large-11 large-push-1 columns text-center statusPnl">
                    <fieldset class="summaryPanel">
                        <strong>Status: </strong>
                        <asp:Literal runat="server" ID="ltrlStatus" />
                    </fieldset>

                </div>
                <br />
                <br />
            </asp:Panel>

            <asp:Panel runat="server" ID="pnlPhaseDescription">
                <div class="row">
                    <div class="columns">
                        <uc:TextBox_Animated runat="server" Title="Short Description" isMultiLine="true" ID="txbShortDescription" />
                    </div>
                </div>
            </asp:Panel>

            <div id="divDiscoveryDescription" runat="server" class="campaign-rating-description">
                <div class="row">
                    <div class="large-22 large-push-1 columns">
                        <div class="campaign-title">Comments</div>
                        <br />

                        <asp:ListView runat="server" ID="lvRatingComments">
                            <LayoutTemplate>
                                <div class="row">
                                    <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                                </div>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <div class="columns member-campaign-rating">
                                    <div id="avgrateCampaignYo" class="starRating"></div>
                                    <p><%#Eval("RatingDate")%></p>
                                    <p><%#Eval("ReviewDetail")%></p>
                                    <asp:HiddenField ID="hdnGivernCampaignRating" runat="server" Value='<%#Eval("Rating")%>' />
                                </div>
                            </ItemTemplate>
                            <EmptyDataTemplate></EmptyDataTemplate>
                            <ItemSeparatorTemplate>
                                <div class="columns">
                                    <br />
                                    <hr />
                                    <br />
                                </div>
                            </ItemSeparatorTemplate>
                        </asp:ListView>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
</div>
