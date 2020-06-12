<%@ Control Language="VB" AutoEventWireup="false" CodeFile="editTab_Content.ascx.vb" Inherits="UserControls_editTab_Content" %>
<%--<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="uc" %>--%>
<%@ Register Src="TextBox_Animated.ascx" TagName="TextBox_Animated" TagPrefix="uc" %>
<%@ Register Src="FeaturedImageSelector.ascx" TagName="FeaturedImageSelector" TagPrefix="uc" %>
<%@ Register Src="SocialMediaManager.ascx" TagName="SocialMediaManager" TagPrefix="uc" %>
<%@ Reference Control="AlertMsg.ascx" %>

<div class="hide hiddenFields">
    <input type="hidden" runat="server" value="" id="hdfdTemplateContent" class="hdfdTemplateContent" />
</div>

<div class="pnlContent">
    <asp:HiddenField runat="server" ID="hfldTabName" />
    <asp:HiddenField runat="server" ID="hfldThisNodeId" />

    <div class="pnlSelectFeaturedImage hide">
        <uc:FeaturedImageSelector runat="server" activeView="SelectImg" ID="ucSelectFeaturedImage" />
    </div>

    <div class="pnlContentManagement">
        <div class="row">

            <div class="small-24 medium-24 large-24 columns">
                <asp:PlaceHolder runat="server" ID="phAlertMsg" />
            </div>


            <div class="small-12 small-push-6 medium-5 medium-push-19 large-3 large-push-21 columns">
                <button runat="server" type="button" id="btnSaveContent" name="btnSaveContent" class="shikobaButton nextPg button--shikoba green button--round-s button--border-thin">
                    <i class="button__icon fi-next size-24"></i>
                    <span>Save</span>
                </button>
                <asp:Button runat="server" ID="lbtnSaveContent" CssClass="hide" ClientIDMode="Static" />
            </div>


            <div class="small-24 medium-24 large-20 large-pull-4 columns">
                <h2>Content Management</h2>
                <p>Now let’s bring this campaign from the drawing board to the web.  Here you can add your campaign’s story, description, and featured image.</p>
            </div>

        </div>

        <br />
        <div class="row">
            <div class="small-24 medium-24 large-12 columns">
                <h6>Brief Summary</h6>
                <p>A brief summary should consist of a single paragraph that briefly describes your campaign.  This discription will be displayed on the campaign page as well as the campaign summary on the team page.</p>
                <uc:TextBox_Animated runat="server" ID="txbBriefSummary" Title="Brief Summary" isMultiLine="true" />
            </div>
            <div class="small-24 medium-24 large-12 columns">
                <div class="row">
                    <div class="small-24 medium-24 large-12 columns">
                        <h6>Banner Image</h6>
                        <p>Select the image that is to be used for your campaign's top banner.</p>
                        <uc:FeaturedImageSelector runat="server" activeView="ShowImg" ID="ucSelectedBannerImg" propertyName="topBannerImage" />
                    </div>
                    <div class="small-24 medium-24 large-12 columns">
                        <h6>Panel Image</h6>
                        <p>Select the  image that is to be used for your campaign's summary panel.</p>
                        <uc:FeaturedImageSelector runat="server" activeView="ShowImg" ID="ucSelectedPanelImg" propertyName="featuredImage"  />
                    </div>
                </div>
                
                <br />
                <br class="hide-for-medium-down" />
                <h6>Social Media Links</h6>
                <p>Select the social medias you will be publicizing your campaign on.</p>
                <uc:SocialMediaManager runat="server" ID="ucSocialMediaManager" />
                <br class="show-for-medium-down" />
            </div>
        </div>
        <div class="row">
            <br />
            <div class="small-24 medium-24 large-16 columns">
                <h6>Full Summary</h6>
                <textarea id="TemplateContent" name="TemplateContent" class="TemplateContent"></textarea>
            </div>
            <div class="small-24 medium-24 large-8 columns">
                <br class="show-for-medium-down" />
                <br class="show-for-medium-down" />
                <h6>Custom CSS</h6>
                <p>These styles will be applied on the campaign page only.  To encorporate custom css for your full summary, ensure that you include the root element Id <strong>#campaign</strong><br />
                <span class="smaller">Example: <strong>#campaign p {color:blue;}</strong></span></p>
                <uc:TextBox_Animated runat="server" ID="txbCustomCss" Title="Custom CSS" isMultiLine="true" Text="#campaign {}" additionalClass="cssPnl" />

                <h6>Additional Class Libraries</h6>
                <p>The following class libraries are available for you to incorporate in your campaign as well:</p>

                
                <ul class="classLibraries">
                    <li>
                        <strong>Animate.css</strong>
                        <ul>
                            <li>
                                A "just-add-water" css animation toolset.  To learn how to use this library go to 
                                <a href="https://daneden.github.io/animate.css/" target="_blank">Animate.css</a>.
                            </li>
                        </ul>
                    </li>
                    <li>
                        <strong>Zurb Foundation</strong>
                        <ul>
                            <li>
                                This site is built using Zurb Foundation 5 responsive framework.  To learn how to incorporate the zurb library in your campaign go to 
                                <a href="https://foundation.zurb.com/sites/docs/v/5.5.3/" target="_blank">Zurb Foundation</a>.
                            </li>
                        </ul>
                    </li>
                </ul>


            </div>
        </div>
    </div>
</div>

