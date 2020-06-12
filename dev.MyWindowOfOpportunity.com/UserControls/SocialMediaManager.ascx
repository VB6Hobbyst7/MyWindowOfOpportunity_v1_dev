<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SocialMediaManager.ascx.vb" Inherits="UserControls_SocialMediaManager" %>
<%@ Register Src="SocialMediaEntry.ascx" TagName="SocialMediaEntry" TagPrefix="uc" %>
<%@ Register Src="AlertMsg.ascx" TagName="AlertMsg" TagPrefix="uc" %>
<%@ Reference Control="AlertMsg.ascx" %>

<div class="socialMediaManager">
    <div class="row">
        <div class="columns">
            <uc:AlertMsg runat="server" Visible="false" MessageType="Alert" ID="ucAlertMsg_Invalid" AlertMsg="Invalid Entry" AdditionalText="Please use proper url format. Ex: http://url.com" />
            <uc:AlertMsg runat="server" Visible="false" MessageType="Success" ID="ucAlertMsg_Success" AlertMsg="Saved Successfully" />
        </div>
    </div>
    <div class="row">
        <div class="small-24 medium-12 large-24 columns">
            <ul class="small-block-grid-4 medium-block-grid-4 large-block-grid-7 socialIcons text-right">
                <li>
                    <asp:HyperLink runat="server" ID="hlnkFacebook" CssClass="button callToAction socialIcon " mediaType="Facebook">
                        <img class="FacebookIcon" alt="" title="Facebook" />
                    </asp:HyperLink>
                </li>
                <li>
                    <asp:HyperLink runat="server" ID="HyperLink1" CssClass="button callToAction socialIcon " mediaType="Twitter">
                        <img class="TwitterIcon" alt="" title="Twitter" />
                    </asp:HyperLink>
                </li>
                 <li>
                    <asp:HyperLink runat="server" ID="HyperLink2" CssClass="button callToAction socialIcon " mediaType="LinkedIn">
                        <img class="LinkedInIcon" alt="" title="LinkedIn" />
                    </asp:HyperLink>
                </li>
                <li>
                    <asp:HyperLink runat="server" ID="HyperLink3" CssClass="button callToAction socialIcon " mediaType="SupportEmail">
                        <img class="EmailIcon" alt="" title="Support Email" />
                    </asp:HyperLink>
                </li>
            </ul>
        </div>
        
    
    </div>

    <div class="socialMediaEntries">
        <uc:SocialMediaEntry runat="server" ID="ucSocialMediaEntry_Facebook" socialMedia="Facebook"  />
        <uc:SocialMediaEntry runat="server" ID="ucSocialMediaEntry_Twitter" socialMedia="Twitter" />
        <uc:SocialMediaEntry runat="server" ID="ucSocialMediaEntry_LinkedIn" socialMedia="LinkedIn" />
        <uc:SocialMediaEntry runat="server" ID="ucSocialMediaEntry_SupportEmail" socialMedia="SupportEmail" />
    </div>
    
</div>