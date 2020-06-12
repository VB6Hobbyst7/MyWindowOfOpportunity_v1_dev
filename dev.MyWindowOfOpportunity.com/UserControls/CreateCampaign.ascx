<%@ Control Language="VB" AutoEventWireup="false" CodeFile="CreateCampaign.ascx.vb" Inherits="UserControls_CreateCampaign" ViewStateMode="Enabled" %>
<%@ Register Src="/UserControls/TextBox_Animated.ascx" TagName="TextBox_Animated" TagPrefix="uc" %>
<%@ Register Src="/UserControls/ulMemberAccts.ascx" TagName="ulMemberAccts" TagPrefix="uc" %>
<%@ Register Src="/UserControls/ulTeamAccts.ascx" TagName="ulTeamAccts" TagPrefix="uc" %>
<%@ Reference Control="~/UserControls/ulMemberAccts.ascx" %>
<%@ Reference Control="~/UserControls/ulTeamAccts.ascx" %>
<%@ Register Src="ManageCampaign_ListItem.ascx" TagName="ManageCampaign_ListItem" TagPrefix="uc" %>
<%@ Register Src="/UserControls/AlertMsg.ascx" TagName="AlertMsg" TagPrefix="uc" %>


<div id="createACampaign" class="createACampaign">
    <br />
    <div class="row">
        <uc:AlertMsg ID="AlertMsg" runat="server" isHidden="true" MessageType="Alert" /> 
        <input type="hidden" id="hfldShowAlert" runat="server" value="" class="hfldShowAlert" />

       <div id="ErrorMsgs" runat="server" data-alert class="alert-box alert" visible="false">
           <a href="#" class="close"></a>
       </div>
        <div class="small-24 medium-24 large-16 columns">
            <div class="row">
                <div class="columns ">
                    <fieldset class="outlinePanel">
                        <legend>Let's Get Started!</legend>
                        <p>First things first!  All campaigns need a name.  Be creative but also discriptive.</p>
                        <br class="hide-for-small-down" />
                        <div class="row">
                            <div class="small-24 medium-20 medium-push-2 large-16 large-push-4 columns">
                                <uc:TextBox_Animated runat="server" ID="ucTxbCampaignName" ClientIDMode="Static"  isEnabled="true" isRequired="true" Title="Campaign Name" />
                            </div>
                        </div>                        
                        <br />
                        <br class="hide-for-small-down" />
                    </fieldset>
                </div>
            </div>
            <br />
            <br class="hide-for-small-down" />
            <div class="row">
                <div class="columns ">

                    <fieldset id="fstTeamAccts" class="filterSet outlinePanel">
                        <legend id="parentFieldsetLegend">Give Your Campaign a Home</legend>
                        <p>Every campaign is owned and managed by a team.  Select a team to place your campaign under, or create a new one.</p>
                        <h6>SELECT A TEAM</h6>

                        <div id="divNewTeamAcct" class="row" data-equalizer>
                            <div class="small-10 medium-8 columns">
                                <asp:RadioButton runat="server" ID="rbCreateNew" Text="Create New" Checked="true" />
                            </div>
                            <div class="small-16 medium-16 columns">
                                <asp:TextBox runat="server" ID="txbNewTeamName" ClientIDMode="Static" CssClass="radius txbNew" data-equalizer-watch/>
                                 <asp:Panel runat="server" ID="pnlTeamErrorMsg" Visible="false" CssClass="errorMsg">
                                    *<asp:Literal runat="server" ID="ltrlTeamErrorMsg" />
                                </asp:Panel>

                            </div>
                        </div>
                        <asp:ListView runat="server" ID="lstviewTeamAccts" ClientIDMode="Predictable" ClientIDRowSuffix="nodeId">
                            <LayoutTemplate>
                                <ul class="small-block-grid-1 medium-block-grid-3 option-set">
                                    <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                                </ul>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <li>
                                    <asp:RadioButton ID='rbtnTeamName' CssClass="rbtnteams" runat="server" Text='<%#Eval("Name")%>' value='<%#Eval("nodeId")%>' GroupName="rbtnTeamName" />
                                    
                                    <%--<span class="rbtnteams">
                                        <input id='<%# "rbtnteams" & Eval("nodeId")%>' type="radio" name='<%# "rbtnteams" & Eval("nodeId")%>' value='<%#Eval("nodeId")%>'>
                                        <label for='<%# "rbtnteams" & Eval("nodeId")%>'><%#Eval("Name")%></label>
                                    </span>--%>

                                    <%--<span class="rbtnteams">
                                        <input id='<%#String.Concat("rbtnteams", Eval("nodeId"))%>' type="radio" name='<%# "rbtnteams" & Eval("nodeId")%>' value='<%#Eval("nodeId")%>'>
                                        <label for='<%#String.Concat("rbtnteams", Eval("nodeId"))%>'><%#Eval("Name")%></label>
                                    </span>--%>
                                </li>
                            </ItemTemplate>
                            <EmptyDataTemplate>
                                <p>You currently do not manage any teams.  Let's create one now.</p>
                            </EmptyDataTemplate>
                        </asp:ListView>
                    </fieldset>
                </div>
            </div>
            <br />
            <br class="hide-for-small-down" />
            
            <asp:PlaceHolder runat="server" ID="phAltEmail" Visible="false">
                <div class="row">
                    <div class="columns">
                        <fieldset class="outlinePanel">
                            <legend>Alternate Email</legend>
                            <p>Because you are logging is using a social media account, you will also need to provide an alternative email address to ensure you are receiving any notifications from your campaign.</p>
                            <br class="hide-for-small-down" />
                            <div class="row">
                                <div class="small-24 medium-20 medium-push-2 large-16 large-push-4 columns">
                                    <uc:TextBox_Animated runat="server" ID="txbAltEmail" isRequired="true" Title="Alt Email" TextMode="_Email" additionalClass="txbAltEmail" />
                                </div>
                            </div>                        
                            <br />
                            <br class="hide-for-small-down" />
                        </fieldset>
                    </div>
                </div>
                <br />
                <br class="hide-for-small-down" />
                <input type="hidden" value="true" id="hfldAltEmail" />
            </asp:PlaceHolder>
            
            <div class="row">
                <div class="small-24 medium-12 medium-push-6 large-8 large-push-0 columns">

                     <button id="btnCreateCampaign" Class="shikobaButton nextPg button--shikoba green small button--round-s button--border-thin">
                        <i class="button__icon fi-next size-24"></i>
                        <span>Create Campaign</span>
                    </button>

                    <asp:LinkButton runat="server" ID="lbtnCreateCampaign" ClientIDMode="Static" CssClass="hide">
                        <i class="button__icon fi-next size-24"></i>
                        <span>Create Campaign</span>
                    </asp:LinkButton>
                </div>
            </div>
        </div>

        <div class="hide-for-medium-down large-7 columns">
            <div class="row">
                <div class="small-24 columns">
                    <fieldset class="outlinePanel secondary currentCampaigns">
                        <legend>Current Campaigns</legend>
                        <br />
                        <p>The following is a list of all teams and campaigns that you currently manage.</p>
                        <h5 class="whiteTextShadow text-center">Administrative Accounts</h5>
                        <hr />
                        <asp:PlaceHolder runat="server" ID="phTeamAccts" />
                        <asp:Panel runat="server" ID="pnlTeamAccts">
                            <p class="text-center">No records exist yet.</p>
                        </asp:Panel>
                        <br />
                        <h5 class="whiteTextShadow text-center">Membership Accounts</h5>
                        <hr />
                        <asp:PlaceHolder runat="server" ID="phMemberAccts" />
                        <asp:Panel runat="server" ID="pnlMemberAccts">
                            <p class="text-center">No records exist yet.</p>
                        </asp:Panel>
                    </fieldset>
                </div>
            </div>
        </div>
    </div>
</div>
