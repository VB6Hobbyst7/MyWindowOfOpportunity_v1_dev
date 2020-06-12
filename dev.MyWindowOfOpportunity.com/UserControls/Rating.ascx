<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Rating.ascx.vb" Inherits="UserControls_Rating" %>
<%@ Register Src="progressBar.ascx" TagName="progressBar" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/RatingSummary.ascx" TagName="RatingSummary" TagPrefix="uc" %>


<div>
    <h2>RATE THIS CAMPAIGN</h2>
    <asp:Panel runat="server" ID="rateYo" ClientIDMode="Static" />
        
    <textarea id="txtComment" runat="server" rows="7" cols="20" class="txt" placeholder="Share your opinion about this campaign..."></textarea>
    <div class="opt"><span>*optional</span></div>

    <div class="oppotunity-box">
        
        <asp:Panel runat="server" ID="pnlThankyou" CssClass="alert-box success radius btn-sz" Visible="false" data-alert>
            <i class="fa fa-check left" aria-hidden="true"></i>
            <p>Thank you for votting!!!</p>
            <a href="#" class="close right" id="anchor_ThankYou" onclick="return CloseThisDiv(this.id)">&times;</a>
        </asp:Panel>
        
        <asp:Panel runat="server" ID="pnlError" CssClass="alert-box alert radius btn-sz" data-alert Visible="false">
            <i class="fa fa-times left" aria-hidden="true"></i>
            <p>Sorry, but there was an error saving your review. Please try again.</p>
            <a href="#" class="close right" id="anchor_Error" onclick="return CloseThisDiv(this.id)">&times;</a>
        </asp:Panel>
        
        <asp:Panel runat="server" ID="pnlLogin" ClientIDMode="Static" CssClass="alert-box warning radius btn-sz" data-alert style="display: none;">
            <i class="fa fa-exclamation-triangle left" aria-hidden="true"></i>
            <p>Before submitting a review please login or become a member.</p>
            <a href="#" class="close right" id="anchor_Register" onclick="return CloseThisDiv(this.id)">&times;</a>
        </asp:Panel>
                
        <asp:Panel runat="server" ID="pnlRate1st" ClientIDMode="Static" CssClass="alert-box warning radius btn-sz" data-alert style="display: none;">
            <i class="fa fa-exclamation-triangle left" aria-hidden="true"></i>
            <p>Please rate before submit</p>
            <a href="#" class="close right" id="anchor_Warning" onclick="return CloseThisDiv(this.id)">&times;</a>
        </asp:Panel>
        
    </div>
    
    <asp:HyperLink runat="server" ID="hlnkSubmit" CssClass="button expand radius secondary custom-btn" onclick="RatingValidation();" Text="SUBMIT" />
    
    <div class="hiddenFields hide">        
        <asp:Button ID="btnSubRating" runat="server" OnClick="btnSubRating_Click" ClientIDMode="Static" />
        <%-- <asp:Button ID="btnSubRating" CssClass="button small expand radius secondary custom-btn hide" runat="server" Text="SUBMIT" OnClick="btnSubRating_Click" OnClientClick="return CheckIfRatingGivenOrNot();" /> --%>
        <asp:HiddenField runat="server" ID="hdnRating" ClientIDMode="Static" Value="0" />
        <asp:HiddenField runat="server" ID="hdnIsMemberAlreadyGivenRating" ClientIDMode="Static" Value="false" />
        <asp:HiddenField runat="server" ID="hdnIsMemberLoggedIn" ClientIDMode="Static" />
        <%--<asp:HiddenField runat="server" ID="hdnAvgRatingCampaign" ClientIDMode="Static" Value="0" />--%>
    </div>
</div>

<div id="home-campaign-rating">
    <%--Summaries--%>
    <br />
    <uc:RatingSummary runat="server" ID="ucRatingSummary" />
    <div class="opt-learn">
        <h4 runat="server" id="h4LearnMore" class="phaseLearnMore" ClientIDMode="Static">Learn More</h4>
        <div class="clearfix"></div>
        <div class="phaseDescription row rating-descriptiondiv" style="display: none;">
            <div class="small-24 columns">
                The Discovery Phase provides an opportunity for the public to share their opinion about the campaign and its goal.  This information can provide valuable guidance the campaign can use to ensure it is successful.  
            </div>
        </div>
    </div>
    <br />
</div>

