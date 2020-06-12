<%@ Control Language="VB" AutoEventWireup="false" CodeFile="NavMinor.ascx.vb" Inherits="UserControls_TopLevel_NavMinor" %>
<%--<%@ Register Src="SearchPanel.ascx" TagName="SearchPanel" TagPrefix="uc" %>--%>


<div id="minorNavPnl" class="">
    <div class="row">
        <div class="small-7 large-5 columns visible-for-large-up">
            <%--<asp:HyperLink runat="server" ID="hlnkContactUs" />--%>
            <asp:Label runat="server" ID="lblUserName" CssClass="welcomeBack" Visible="false" />
        </div>

        <div class="small-24 medium-24 large-19 columns text-right nopadding">
            <div class="left show-for-medium-only navMenu">
                <section class="left-medium">
                    <%--Note: the side menu is compiled in NavMain.ascx--%>
                    <a class="left-off-canvas-toggle menu-icon" href="#"><span></span></a>
                </section>
            </div>

            <%--<div class="hide-for-small-down">
                <uc:SearchPanel runat="server" ID="ucSearchPanel" panelSize="Micro" />
            </div>--%>

            <asp:Panel runat="server" ID="pnlLoggedOut" CssClass="right" ClientIDMode="Static">
                <div class="left LoginLink show-for-medium-up">
                    &nbsp;&nbsp;
                    <asp:HyperLink runat="server" ID="hlnkLogin" ClientIDMode="Static" Text="Login" />
                    <div id="loginPanel">
                        <div class="row">
                            <div class="small-24 columns">
                                Email: 
                                <asp:TextBox runat="server" ID="txbEmail" autofocus />
                            </div>
                            <div class="small-24 columns">
                                Password 
                                <asp:TextBox runat="server" ID="txbPassword" TextMode="Password" />
                            </div>


                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <div class="small-24 columns">
                                        <div class="left">
                                            <asp:LinkButton runat="server" ID="lbtnLogin" Text="Login" CssClass="loginButton" />
                                        </div>
                                        <div class="right">
                                            <asp:HyperLink ID="ForgotPassword" runat="server" CssClass="forgotPw">Forgot Password?</asp:HyperLink>
                                        </div>
                                    </div>

                                    <asp:Panel runat="server" ID="pnlInvalidCredentials" CssClass="small-24 columns invalidCredentials" Visible="false">
                                        <br />
                                        * * * Invalid Credentials * * *
                                    </asp:Panel>
                                    
                                    <asp:Panel runat="server" ID="pnlFacebookUser" CssClass="small-24 columns invalidCredentials" Visible="false">
                                        <br />This is a social account.
                                        <br />Sign in with Facebook
                                    </asp:Panel>
                                    <asp:Panel runat="server" ID="pnlTwitterUser" CssClass="small-24 columns invalidCredentials" Visible="false">
                                        <br />This is a social account.
                                        <br />Sign in with Twitter
                                    </asp:Panel>
                                    <asp:Panel runat="server" ID="pnlLinkedInUser" CssClass="small-24 columns invalidCredentials" Visible="false">
                                        <br />This is a social account.
                                        <br />Sign in with LinkedIn
                                    </asp:Panel>

                                </ContentTemplate>
                            </asp:UpdatePanel>

                            <div class="small-24 columns">
                                <hr />
                            </div>
                        </div>


                        <div class="row divRow">
                            <div class="small-24 columns">
                                <asp:LinkButton ID="lnkFblogin" runat="server" CssClass="socialLogin facebook">
                                    <i class="fa fa-facebook fa-facebookS"></i> 
                                    Sign in with Facebook                                    
                                </asp:LinkButton>
                            </div>
                        </div>
                        <div class="row divRow">
                            <div class="small-24 columns">
                                <asp:LinkButton ID="lnkTwitterLogin" runat="server" CssClass="socialLogin twitter">                                    
                                    <i class="fa fa-twitter fa-twitterS"></i> 
                                    Sign in with Twitter                                    
                                </asp:LinkButton>
                            </div>
                        </div>
                        <div class="row divRow_2">
                            <div class="small-24 columns">
                                <asp:LinkButton runat="server" ID="lnkLinkedInLogin" CssClass="socialLogin linkedIn">                                   
                                    <i class="fa fa-linkedin fa-linkedinS"></i> 
                                    Sign in with LinkedIn
                                </asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="left hide-for-small-down">&nbsp;&nbsp;|&nbsp;&nbsp;</div>
                <div class="left hide-for-small-down">
                    <asp:HyperLink runat="server" ID="hlnkBecomeAMember" />&nbsp;&nbsp;&nbsp;
                </div>

                <nav class="tab-bar show-for-small-down">
                    <section class="right-small">
                        <%--Note: the side menu is compiled in NavMain.ascx--%>
                        <a class="right-off-canvas-toggle menu-icon" href="#"><span></span></a>
                    </section>
                </nav>
            </asp:Panel>

            <asp:Panel runat="server" ID="pnlLoggedIn" CssClass="right loggedIn ">
                <nav class="top-bar hide-for-small-down" data-topbar role="navigation">
                    <section class="top-bar-section">
                        <ul class="right">

                            <li><asp:HyperLink runat="server" ID="hlnkCreateCampaign" Text="Create" /></li>
                            <li class="nav-alter hide-for-small-only">|</li>
                            
                            <li><asp:HyperLink runat="server" ID="hlnkManageCampaigns" Text="Manage" /></li>
                            <li class="nav-alter hide-for-small-only">|</li>

                            <li><asp:HyperLink runat="server" ID="hlnkEditAcct" Text="Account" /></li>
                            <li class="nav-alter hide-for-small-only">|</li>
                            
                            <li><asp:LinkButton runat="server" ID="lbtnLogout" OnClick="lbtnLogout_Click" Text="Logout" Visible="true" /></li>

                        </ul>
                    </section>
                </nav>
                <nav class="tab-bar show-for-small-down">
                    <section class="right-small">
                        <%--Note: the side menu is compiled in NavMain.ascx--%>
                        <a class="right-off-canvas-toggle menu-icon" href="#"><span></span></a>
                    </section>
                </nav>
            </asp:Panel>
            
        </div>
    </div>
</div>