<%@ Control Language="VB" AutoEventWireup="false" CodeFile="NavMain.ascx.vb" Inherits="UserControls_TopLevel_NavMain" EnableViewState="true" %>


<header id="navPnl">
    <div class="nav-container contain-to-grid">
        
        <%--DESKTOP NAVIGATION--%>
        <nav class="top-bar hide-for-medium-down" data-topbar="" data-options="is_hover: true; custom_back_text: false">
            <section class="top-bar-section main-nav">
                <div class="row collapse">
                    <div class="large-12 large-push-12 columns">
                        <ul class="small-block-grid-3 text-center">
                            <li class="has-dropdown">
                                <a data-dropdown="mega5" data-options="is_hover:true" href="#">CAMPAIGN</a>
                            </li>
                            <li class="has-dropdown">
                                <a data-dropdown="mega6" data-options="is_hover:true" href="#">INVEST</a>
                            </li>
                            <li class="has-dropdown">
                                <a data-dropdown="mega7" data-options="is_hover:true" href="#">MARKET</a>
                            </li>
                        </ul>
                    </div>
                </div>
            </section>
            <section class="top-bar-mega">
                <div id="mega5" data-dropdown-content class="f-dropdown content row">
                    <div class="small-9 columns subjectHeading">
                        <h2><asp:HyperLink runat="server" ID="hlnkCampaign1" /></h2>
                        <div><asp:Literal runat="server" ID="hlnkCampaignDesc1" /></div>
                    </div>
                    <asp:Panel runat="server" ID="pnlCampaignLinks_1" CssClass="small-5 columns" />
                    <asp:Panel runat="server" ID="pnlCampaignLinks_2" CssClass="small-5 columns" />
                    <asp:Panel runat="server" ID="pnlCampaignLinks_3" CssClass="small-5 columns" />
                </div>

                <div id="mega6" data-dropdown-content class="f-dropdown content row">
                    <div class="small-9 columns subjectHeading">
                        <h2><asp:HyperLink runat="server" ID="hlnkInvest1" /></h2>
                        <div><asp:Literal runat="server" ID="ltrlInvestDesc1" /></div>
                    </div>
                    <asp:Panel runat="server" ID="pnlInvestLinks_1" CssClass="small-5 columns" />
                    <asp:Panel runat="server" ID="pnlInvestLinks_2" CssClass="small-5 columns" />
                    <asp:Panel runat="server" ID="pnlInvestLinks_3" CssClass="small-5 columns" />
                </div>

                <div id="mega7" data-dropdown-content class="f-dropdown content row">
                    <div class="small-9 columns subjectHeading">
                        <h2><asp:HyperLink runat="server" ID="hlnkMarket1" /></h2>
                        <div><asp:Literal runat="server" ID="ltrlMarketDesc1" /></div>
                        <h6>Coming Soon...</h6>
                    </div>
                    <asp:Panel runat="server" ID="pnlMarketLinks_1" CssClass="small-5 columns" />
                    <asp:Panel runat="server" ID="pnlMarketLinks_2" CssClass="small-5 columns" />
                    <asp:Panel runat="server" ID="pnlMarketLinks_3" CssClass="small-5 columns" />                
                </div>
            </section>
        </nav>

        <%--TABLET NAVIGATION--%>
        <aside class="left-off-canvas-menu show-for-medium-only">
            <asp:PlaceHolder runat="server" ID="phTableMenu" />
        </aside>

        <%--MOBILE NAVIGATION--%>
        <aside class="right-off-canvas-menu show-for-small-down">            
            <asp:PlaceHolder runat="server" ID="phMobileMenu" />
            <%--<ul class="off-canvas-list">
                <li><label>Main Menu [If Logged Out]</label></li>
                <li><a href="/">Login</a></li>
                <li><a href="/">Become A Member</a></li>
                <li><a href="/">Contact Us</a></li>

                <li><label>Main Menu [If Logged In]</label></li>
                <li><a href="/">Logout</a></li>
                <li><a href="/">Account</a></li>
                <li><a href="/">New Campaign</a></li>
                <li><a href="/">Manage Campaigns</a></li>
                <li><a href="/">Contact Us</a></li>

                <li><label>Learn More</label></li>
                <li class="has-submenu"><a href="/">Campaign</a>
                    <ul class="right-submenu">
                        <li class="back"><a href="#">Back</a></li>
                        <li><a href="/">Link 1</a></li>
                        <li><a href="/">Link 2</a></li>
                        <li><a href="/">Link 3</a></li>
                    </ul>
                </li>

                <li class="has-submenu"><a href="/">Invest</a>
                    <ul class="right-submenu">
                        <li class="back"><a href="#">Back</a></li>
                        <li><a href="/">Link 1</a></li>
                        <li><a href="/">Link 2</a></li>
                        <li><a href="/">Link 3</a></li>
                    </ul>
                </li>

                <li class="has-submenu"><a href="/">Market</a>
                    <ul class="right-submenu">
                        <li class="back"><a href="#">Back</a></li>
                        <li><a href="/">Link 1</a></li>
                        <li><a href="/">Link 2</a></li>
                        <li><a href="/">Link 3</a></li>
                    </ul>
                </li>


            </ul>--%>
        </aside>

    </div>
</header>



<%--<asp:GridView runat="server" ID="gv" />--%>
