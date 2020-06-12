<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Banner.ascx.vb" Inherits="UserControls_TopLevel_Banner" %>

<div id="bannerPnl">
    <asp:MultiView runat="server" ID="mvBanners" ActiveViewIndex="0">
        <asp:View runat="server" ID="vHomeBanner">
            <div class="row collapse shadow" data-homebanner>
                <div class="columns">
                    <asp:Image runat="server" ID="imgHomeBanner" CssClass="" />
                    <div class="row bannerOverImg">
                        <div class="columns">

                            <div class="row">
                                <div class="small-16 medium-12 large-10 large-push-1 columns">
                                    <asp:HyperLink runat="server" ID="hlnkMainLogo_HomePg">
                                        <div id="LogoPanel">
	                                        <img alt="" class="logoImg01" src="/Images/Logo/mainLogo01.png" />
	                                        <img alt="" class="logoImg02" src="/Images/Logo/mainLogo02.png" />
	                                        <img alt="" class="logoImg03" src="/Images/Logo/mainLogo03.png" />
	                                        <img alt="" class="logoImg04" src="/Images/Logo/mainLogo04.png" />
	                                        <img alt="" class="logoImg05" src="/Images/Logo/mainLogo05.png" />
                                        </div> 
                                    </asp:HyperLink>
                                </div>
                                <div class="hide-for-small-down medium-7 large-4 columns text-center">
                                    <div class="row collapse">

                                        <ul class="small-block-grid-5 socialIcons">
                                            <li>
                                                <asp:HyperLink runat="server" ID="hlnkFacebook" CssClass="button callToAction socialIcon" Target="_blank">
                                                    <asp:Image runat="server" ID="imgFacebook" CssClass="FacebookIcon" />
                                                </asp:HyperLink>
                                            </li>
                                            <li>
                                                <asp:HyperLink runat="server" ID="hlnkTwitter" CssClass="button callToAction socialIcon" Target="_blank">
                                                    <asp:Image runat="server" ID="imgTwitter" CssClass="TwitterIcon" />
                                                </asp:HyperLink>
                                            </li>
                                            <li>
                                                <asp:HyperLink runat="server" ID="hlnkLinkedIn" CssClass="button callToAction socialIcon" Target="_blank">
                                                    <asp:Image runat="server" ID="imgLinkedIn" CssClass="LinkedInIcon" />
                                                </asp:HyperLink>
                                            </li>
                                            <li>
                                                <asp:HyperLink runat="server" ID="hlnkEmail" CssClass="button callToAction socialIcon">
                                                    <asp:Image runat="server" ID="imgEmail" CssClass="EmailIcon" />
                                                </asp:HyperLink>
                                            </li>
                                            <li>
                                                <a class="button callToAction socialIcon ScriptureIconBtn">
                                                    <asp:Image runat="server" CssClass="ScriptureIcon" />
                                                    <div class="scrVs">
                                                        I can do all things through Christ who strengthens me.  <b>~Philippians 4:13</b>
                                                    </div>
                                                </a>
                                            </li>
                                        </ul>
                                        
                                    </div>
                                </div>
                            </div>
                            <div class="row collapse hide-for-medium-down">
                                <div class="large-8 large-push-4 columns callToAction majorText text-center">
                                    <span class="bold">Change</span> your future
                                </div>
                            </div>
                            <div class="row collapse hide-for-medium-down">
                                <div class="large-8 large-push-4 columns">
                                    <br class="show-for-medium-only" />
                                    <br class="show-for-medium-only" />
                                    <br class="show-for-medium-only" />
                                    <br class="show-for-medium-only" />
                                    <asp:HyperLink runat="server" ID="hlnkStartFreeCampaign" CssClass="button callToAction radius">
                                        START YOUR <strong>FREE<sup>*</sup></strong> CAMPAIGN TODAY!
                                    </asp:HyperLink>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
                <div class="small-24 medium-20 medium-push-2 space-below hide-for-large-up">
                    <a href="/" class="button callToAction radius startToday">START YOUR <strong>FREE<sup>*</sup></strong> CAMPAIGN TODAY!</a>
                </div>
            </div>
        </asp:View>
        <asp:View runat="server" ID="vStandardBanner">
            <div class="row collapse shadow standardBanner" data-standardbanner>
                <div class="columns">
                    <img src="/Images/Banners/backgroundBanner2_sm.jpg" id="imgBgBanner_Standard" />
                    <div class="row">
                        <div class="small-8 small-push-1 large-8 large-push-1 columns">
                            <asp:HyperLink runat="server" ID="hlnkMainLogo_StandardPg">
                                <asp:Image runat="server" ID="Image1" ImageUrl="~/Images/Logo/mainLogo.png" />
                            </asp:HyperLink>
                        </div>
                        <div class="hide-for-small-down medium-7 large-4 columns text-center socialIconPnl">
                            <div class="row collapse">
                                <ul class="small-block-grid-5 socialIcons">
                                    <li>
                                        <asp:HyperLink runat="server" ID="hlnkFacebook_sm" CssClass="button callToAction socialIcon" Target="_blank">
                                            <asp:Image runat="server" ID="imgFacebook_sm" CssClass="FacebookIcon" />
                                        </asp:HyperLink>
                                    </li>
                                    <li>
                                        <asp:HyperLink runat="server" ID="hlnkTwitter_sm" CssClass="button callToAction socialIcon" Target="_blank">
                                            <asp:Image runat="server" ID="imgTwitter_sm" CssClass="TwitterIcon" />
                                        </asp:HyperLink>
                                    </li>
                                    <li>
                                        <asp:HyperLink runat="server" ID="hlnkLinkedIn_sm" CssClass="button callToAction socialIcon" Target="_blank">
                                            <asp:Image runat="server" ID="imgLinkedIn_sm" CssClass="LinkedInIcon" />
                                        </asp:HyperLink>
                                    </li>
                                    <li>
                                        <asp:HyperLink runat="server" ID="hlnkEmail_sm" CssClass="button callToAction socialIcon" NavigateUrl="mailto:">
                                            <asp:Image runat="server" ID="imgEmail_sm" CssClass="EmailIcon" />
                                        </asp:HyperLink>
                                    </li>
                                    <li>
                                        <a class="button callToAction socialIcon ScriptureIconBtn">
                                            <div class="scrVs">
                                                I can do all things through Christ who strengthens me.  <b>~Philippians 4:13</b>
                                            </div>
                                            <asp:Image runat="server" CssClass="ScriptureIcon" />
                                        </a>
                                    </li>
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:View>
        <asp:View runat="server" ID="vNarrowBanner">
            <div class="row collapse shadow" data-campaignbanner>
                <div class="columns">
                    <img src="/Images/Banners/backgroundBanner3_sm.jpg" id="imgBgBanner_Campaign" />
                    <div class="row">
                        <div class="small-10 small-push-1 medium-8 large-8 large-push-1 columns">
                            <asp:HyperLink runat="server" ID="hlnkMainLogo_Smaller">
                                <asp:Image runat="server" ID="Image2" ImageUrl="~/Images/Logo/mainLogo_small.png" />
                            </asp:HyperLink>
                        </div>
                    </div>
                </div>
            </div>
        </asp:View>
    </asp:MultiView>
</div>
