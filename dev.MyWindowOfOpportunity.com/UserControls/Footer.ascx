<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Footer.ascx.vb" Inherits="UserControls_TopLevel_Footer" %>


<%--<asp:GridView runat="server" ID="gv" />--%>
<%--<style>
    /*.socialIcons a img{
        border:1px solid #fff !important;
    }*/
</style>--%>
<br />
<div id="footerPnl">
    <br />
    <div class="row">
        <div class="medium-8 columns">
            <asp:PlaceHolder runat="server" ID="phColumn01" />


            <div class="row hide-for-small-down">
                <div class="columns seeABug">
                    <asp:HyperLink runat="server" ID="hlnkSeeABug">
                        Report a &nbsp;<img alt="bug" src="/Images/Icons/bug.png" />
                    </asp:HyperLink>
                </div>
                <br />
                <br class="show-for-medium-only" />
            </div>

        </div>
        <div class="medium-8 columns">
            <asp:PlaceHolder runat="server" ID="phColumn02" />
        </div>
        <div class="medium-8 columns">
            <asp:PlaceHolder runat="server" ID="phColumn03" />

            <div class="row show-for-small-down">
                <div class="columns seeABug">
                    <asp:HyperLink runat="server" ID="hlnkSeeABug_sm">
                        Report a &nbsp;<img alt="bug" src="/Images/Icons/bug.png" />
                    </asp:HyperLink>
                </div>
                <br />
                <br />
            </div>

            <div class="small-text-center">
                <br class="show-for-small-down" />
                <a href="/">
                    <img src="/Images/Logo/logo_white.png" />
                </a>
            </div>
            <div class="small-text-center medium-text-right">
                <a target="_blank" href="http://fifthlabs.com/">A Fifth Labs Company</a>
            </div>
        </div>
    </div>


    <div class="row hide-for-small-down">
        <div class="medium-8 columns">
            <span id="siteseal">
                <script async type="text/javascript" src="https://seal.godaddy.com/getSeal?sealID=aWaCDS69xGfBodMCyfHJWYHy4eGUtPsIUjdq5n2hOItJEvMr83W70o4csArE"></script>
            </span>
        </div>
        <div class="medium-8 columns">
            Copyright ©<asp:Literal runat="server" ID="ltrlCopyrightYear" /><br class="show-for-medium-only" />
            <asp:Literal runat="server" ID="ltrlCopyrightTitle" />
        </div>
        <div class="medium-8 columns socialIcons text-right">
            <asp:HyperLink runat="server" ID="hlnkFacebook">
                <asp:Image runat="server" ID="imgFacebook" CssClass="FacebookIcon" />
            </asp:HyperLink>
            <asp:HyperLink runat="server" ID="hlnkTwitter">
                <asp:Image runat="server" ID="imgTwitter" CssClass="TwitterIcon" />
            </asp:HyperLink>
            <asp:HyperLink runat="server" ID="hlnkLinkedIn">
                <asp:Image runat="server" ID="imgLinkedIn" CssClass="LinkedInIcon" />
            </asp:HyperLink>
            <asp:HyperLink runat="server" ID="hlnkEmailUs" NavigateUrl="mailto:">
                <asp:Image runat="server" ID="imgEmailUs" CssClass="EmailIcon" />
            </asp:HyperLink>

            
            <a class="ScriptureIconBtn">
                <asp:Image runat="server" CssClass="ScriptureIcon" />
                <div class="scrVs">
                    For God so loved the world that He gave His only begotten Son, that whoever believes in Him should not perish but have everlasting life. <b>~John 3:16</b>
                </div>
            </a>

        </div>
    </div>

    <div class="row collapse show-for-small-down">
        <br />
        <div class="small-24 columns socialIcons text-center">
            <asp:HyperLink runat="server" ID="hlnkFacebook_sm" Target="_blank">
                <asp:Image runat="server" ID="imgFacebook_sm" CssClass="FacebookIcon" />
            </asp:HyperLink>
            <asp:HyperLink runat="server" ID="hlnkTwitter_sm" Target="_blank">
                <asp:Image runat="server" ID="imgTwitter_sm" CssClass="TwitterIcon" />
            </asp:HyperLink>
            <asp:HyperLink runat="server" ID="hlnkLinkedIn_sm" Target="_blank">
                <asp:Image runat="server" ID="imgLinkedIn_sm" CssClass="LinkedInIcon" />
            </asp:HyperLink>
            <asp:HyperLink runat="server" ID="hlnkEmailUs_sm" NavigateUrl="mailto:">
                <asp:Image runat="server" ID="imgEmailUs_sm" CssClass="EmailIcon" />
            </asp:HyperLink>
            
            <a class="ScriptureIconBtn">
                <asp:Image runat="server" CssClass="ScriptureIcon" />
                <div class="scrVs">
                    For God so loved the world that He gave His only begotten Son, that whoever believes in Him should not perish but have everlasting life. <b>~John 3:16</b>
                </div>
            </a>

        </div>
        <div class="columns text-center">
            <br />
            Copyright ©<asp:Literal runat="server" ID="ltrlCopyrightYear_sm" />
            <asp:Literal runat="server" ID="ltrlCopyrightTitle_sm" />
        </div>
    </div>
    <br />
</div>