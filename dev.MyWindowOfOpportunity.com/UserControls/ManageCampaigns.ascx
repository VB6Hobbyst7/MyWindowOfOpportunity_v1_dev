<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ManageCampaigns.ascx.vb" Inherits="UserControls_ManageCampaigns" %>
<%@ Register Src="ManageCampaign_List.ascx" TagName="ManageCampaign_List" TagPrefix="uc" %>


<%--<asp:GridView runat="server" ID="gv" />
<asp:GridView runat="server" ID="gv2" />
<asp:GridView runat="server" ID="gv3" />--%>


<div id="manageCampaigns">
    <div class="row">
        <div class="small-24 medium-14 large-15 columns campaignList">
            <p>Being able to manage you projects is key to building a successful campaign. Below you can view and edit the teams and campaigns you are associated with.</p>
            <h6 class="hide">You are currently not associated with any campaigns.</h6>

            <asp:ListView runat="server" ID="lstvCampaigns">
                <LayoutTemplate>
                    <ul class="small-block-grid-1 medium-block-grid-2 large-block-grid-3" data-equalizer="description">
                        <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                    </ul>
                </LayoutTemplate>
                <ItemTemplate>
                    <li data-campaignid='<%#Eval("campaignId")%>'>
                        <div class="CampaignPanel">
                            <div class="round border">
                                <img class="round topOnly campaignImg" src='<%#Eval("imageUrl")%>'>

                                <div class="titleSection text-center" data-equalizer-watch="title">
                                    <div class="title">
                                        <%#Eval("title")%>
                                    </div>
                                    <div class="author">
                                        by <%#Eval("team")%>
                                    </div>
                                </div>
                                <div class="description round bottomOnly" data-equalizer-watch="description">
                                    <div class="row">
                                        <div class="small-11 small-push-1 large-10 large-push-1 columns">
                                            <a class="button tiny radius " href='<%#Eval("campaignUrl")%>'>View</a>
                                        </div>
                                        <div class="small-11 small-pull-1 large-10 large-pull-1 columns">
                                            <a class="button tiny radius secondary" href='<%#Eval("campaignEditUrl")%>'>Edit</a>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </li>
                </ItemTemplate>
                <EmptyDataTemplate>
                    <div>No campaigns have been created yet. Let's begin one now. </div>
                </EmptyDataTemplate>
            </asp:ListView>

            <asp:ListView runat="server" ID="lstviewCampaignList">
                <LayoutTemplate>
                        <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                </LayoutTemplate>
                <ItemTemplate>
                    <uc:ManageCampaign_List runat="server" nodeId='<%#Eval("nodeId")%>' isAdmin='<%#Eval("Team")%>' />                    
                </ItemTemplate>
                <EmptyDataTemplate>
                    <div>No campaigns have been created yet. Let's begin one now. </div>
                </EmptyDataTemplate>
            </asp:ListView>
            <br />
        </div>
        <div class="hide-for-medium-down large-1 columns">&nbsp;</div>
        <div class="small-24 medium-10 large-8 columns">              

            <fieldset class="secondary outlinePanel teamList" data-equalizer-watch="">
                <legend>Teams</legend>
                <br />
                
                <asp:ListView runat="server" ID="lstvTeams">
                    <LayoutTemplate>
                            <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                    </LayoutTemplate>
                    <ItemTemplate> 
                        <div class="row" data-teamid='<%#Eval("teamId")%>'>
                            <div class="small- medium-24 large-12 columns">
                                <strong><%#Eval("name")%></strong>
                            </div>
                                      
                            <div class="small-12 medium-12 large-6 columns">
                                <a class="button tiny radius " href='<%#Eval("teamUrl")%>'>View</a>
                            </div>
                            <div class="small-12 medium-12 large-6 columns">
                                <a class="button tiny radius secondary" href='<%#Eval("teamEditUrl")%>'>Edit</a>
                            </div>                       
                        </div>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        <div>No teams have been created yet.</div>
                    </EmptyDataTemplate>
                    <ItemSeparatorTemplate>
                        <hr />
                    </ItemSeparatorTemplate>
                </asp:ListView>
                
            </fieldset>
            
            <br />
            <br />

            <fieldset class="outlinePanel">
                <legend>Create A Campaign</legend>
                <br />
                <div class="row">
                    <div class="small-24 medium-24 large-24 columns text-center">
                        <%--<p>Have an idea?<br />
                           &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Want to bring it to life?<br />
                           &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Let us help.</p>--%>
                        <p>Have an idea?  Want to bring it to life?<br /><strong class="larger">Start Here</strong></p>
                  
                    </div>
                </div>
                <div class="row">
                    <div class="large-18 large-push-3 columns end">
                        <asp:HyperLink runat="server" ID="hlnkCreateCampaign" CssClass="blue shikobaButton nextPg button--shikoba small button--round-s button--border-thin">
                            <i class="button__icon fi-next size-24"></i>
                            <span>Create a Campaign</span>
                        </asp:HyperLink>
                    </div>
                </div>
                <div class="row">
                    <div class="small-24 medium-24 large-24 columns text-center">
                        <br />
                        <h6 class="">Together we can create<br />YOUR Window of Opportunity</h6>
                    </div>
                </div>
            </fieldset>     
            
        </div>
    </div>
</div>
