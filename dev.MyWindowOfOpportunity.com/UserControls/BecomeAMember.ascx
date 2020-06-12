<%@ Control Language="VB" AutoEventWireup="false" CodeFile="BecomeAMember.ascx.vb" Inherits="UserControls_BecomeAMember" %>

<%@ Register TagPrefix="uc" Src="TextBox_Animated.ascx" TagName="TextBox_Animated" %>
<%@ Register TagPrefix="uc" Src="DateDropDowns.ascx" TagName="DateDropDowns" %>
<%@ Register TagPrefix="uc" Src="/UserControls/AlertMsg.ascx" TagName="AlertMsg" %>


<div class="hiddenFields">
    <asp:HiddenField runat="server" ID="hfldErrorMsg" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hfld2ndErrorMsg" ClientIDMode="Static" />
</div>

<asp:MultiView runat="server" ID="mvBecomeAMember" ActiveViewIndex="0">
    <asp:View runat="server" ID="vBecomeAMember">
        <div class="row">
            <div class="large-24 columns">
                <h3>Why should you create an account?</h3>
                <p>Whether you want to help a campaign or create your own, this is where it all starts! So let's get started!</p>
            </div>
        </div>

        <br />
        <fieldset class="outlinePanel phaseStatus">
            <legend>Agreements</legend>

            <div class="row">
                <div class="small- medium- large- columns">
                    <%----%>
                    
                    <div class="switch radius">
                        <input type="checkbox" id="cbAgreeToTerms" required />
                        &nbsp;
                        <label runat="server" id="lblAgreeToTerms">
                            <span class="switch-on">Yes</span>
                            <span class="switch-off">No</span>
                        </label>
                    </div>
                    &nbsp;&nbsp;&nbsp;
                    I agree to the MWoO
                    <strong><a target="_blank" href="/legal/#termsandconditions">Terms & Conditions</a></strong>,
                    <strong><a target="_blank" href="/legal/#privacypolicy">Privacy Policy</a></strong>, and 
                    <strong><a target="_blank" href="/legal/#disclaimer">Disclaimer</a></strong>.
                    <br />
                </div>
            </div>
        </fieldset>
        <br />

        <div class="row">
            <div class="columns alertMsgs">
                <uc:AlertMsg runat="server" ClientIDMode="Static" isHidden="true" ID="ucAlertMsg_Success" MessageType="Success" AlertMsg="" AdditionalText="" />
                <uc:AlertMsg runat="server" ClientIDMode="Static" isHidden="true" ID="ucAlertMsg_Info" MessageType="Info" AlertMsg="" AdditionalText="" />
                <uc:AlertMsg runat="server" ClientIDMode="Static" isHidden="true" ID="ucAlertMsg_Error" MessageType="Alert" AlertMsg="" AdditionalText="" />
                <uc:AlertMsg runat="server" ClientIDMode="Static" isHidden="true" ID="ucAlertMsg_Warning" MessageType="Warning" AlertMsg="" AdditionalText="" />
                <span class="errors"></span>
            </div>
        </div>

        <fieldset class="pnlBecomingAMember">
            <div class="row">
                <div class="large-5 columns">
                    <div class="row">
                        <div class="large-24 columns">
                            <br />
                            <h2 class="text-center">Signup by social media</h2>
                            <div class="row socialLinks">
                                <div class="small-24 medium-8 large-24 columns text-center">
                                    <br />
                                    <asp:LinkButton ID="lnkFblogin" runat="server" CssClass="socialLogin facebook">
                                        <div class="row collapse">
                                            <div class="small-3 medium- large-3 columns text-right">
                                                <i class="fa fa-facebook fa-facebookS"></i> 
                                            </div>
                                            <div class="small-3 medium- large-3 columns text-center">
                                                |
                                            </div>
                                            <div class="small-18 medium- large-18 columns text-left">
                                                Sign up with Facebook
                                            </div>
                                        </div>               
                                    </asp:LinkButton>
                                </div>
                                <div class="small-24 medium-8 large-24 columns text-center">
                                    <br />
                                    <asp:LinkButton ID="lnkTwitterLogin" runat="server" CssClass="socialLogin twitter"> 
                                    <div class="row collapse">
                                        <div class="small-3 columns text-right">
                                            <i class="fa fa-twitter fa-twitterS"></i> 
                                        </div>
                                        <div class="small-3 columns text-center">
                                            |
                                        </div>
                                        <div class="small-18 columns text-left">
                                            Sign up with Twitter
                                        </div>
                                    </div>                                    
                                    <%--<i class="fa fa-twitter fa-twitterS"></i> 
                                    &nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;Sign up with Twitter--%>                        
                                    </asp:LinkButton>
                                </div>
                                <div class="small-24 medium-8 large-24 columns text-center end">
                                    <br />
                                    <asp:LinkButton runat="server" ID="lnkLinkedInLogin" CssClass="socialLogin linkedIn"> 
                                    <div class="row collapse">
                                        <div class="small-3 columns text-right">
                                            <i class="fa fa-linkedin fa-linkedinS"></i> 
                                        </div>
                                        <div class="small-3 columns text-center">
                                            |
                                        </div>
                                        <div class="small-18 columns text-left">
                                            Sign up with LinkedIn
                                        </div>
                                    </div>                                                                      
                                    <%--<i class="fa fa-linkedin fa-linkedinS"></i> 
                                    &nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;Sign up with LinkedIn--%>
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="large-9 large-push-1 columns">
                    <br />
                    <div class="row">
                        <h2 class="text-center ">Or create an account</h2>
                        <div class="medium-9 large-8 columns">
                            <uc:TextBox_Animated runat="server" ID="txbFirstName" Title="First Name" additionalClass="txbFirstName" isRequired="true" />
                        </div>
                        <div class="medium-9 large-8 columns">
                            <uc:TextBox_Animated runat="server" ID="txbLastName" Title="Last Name" additionalClass="txbLastName" isRequired="true" />
                        </div>
                        <div class="medium-6 large-8 columns">
                            <span class="input input--manami pnl_DateOfBirth">
                                <input type="date" id="txbDateOfBirth" runat="server" class="flatpickr input__field input__field--manami txbDateOfBirth" required>
                                <label class="input__label input__label--manami">
                                    <span class="input__label-content input__label-content--manami">Date of Birth 
                                    <span class="errorMsg">*</span>
                                    </span>
                                </label>
                            </span>
                        </div>
                        <div class="medium-12 large-12 columns">
                            <uc:TextBox_Animated runat="server" ID="txbEmail" Title="Email" TextMode="_Email" additionalClass="txbEmail" isRequired="true" />
                        </div>
                        <div class="medium-12 large-12 columns">
                            <uc:TextBox_Animated runat="server" ID="txbReEnterEmail" Title="Re-enter Email" TextMode="_Email" additionalClass="txbReEnterEmail" isRequired="true" />
                        </div>
                    </div>
                </div>
                <div class="large-9 columns">
                    <fieldset class="passwordPnl">
                        <legend></legend>
                        <div class="changePasswordPnl">
                            <div class="row">
                                <div class="medium-12 columns">
                                    <uc:TextBox_Animated runat="server" ID="txbPassword" Title="Password" additionalClass="txbPassword" isRequired="True" isPassword="true" />
                                </div>
                                <div class="medium-12 columns">
                                    <uc:TextBox_Animated runat="server" ID="txbPasswordReenter" Title="Re-enter Password" additionalClass="txbPasswordReenter" isRequired="True" isPassword="true" />
                                </div>
                            </div>

                            <div class="passwordValidations">
                                <div class="row parameter01">
                                    <div class="small-4 medium-2 medium-push-2 large-2 large-push-2 columns pwd1">
                                        <img alt="valid" src="/Images/Icons/validation_circle_valid.png" class="valid hide" />
                                        <img alt="invalid" src="/Images/Icons/validation_circle_invalid.png" class="invalid" />
                                    </div>
                                    <div class="small-16 medium-16 large-16 medium-push-2 columns text-center">
                                        6+ characters
                                    </div>
                                    <div class="small-4 medium-2 medium-push-2 large-2 large-push-2 columns end pwd2 text-right">
                                        <img alt="valid" src="/Images/Icons/validation_circle_valid.png" class="valid hide" />
                                        <img alt="invalid" src="/Images/Icons/validation_circle_invalid.png" class="invalid" />
                                    </div>
                                </div>

                                <div class="row parameter02">
                                    <div class="small-4 medium-2 medium-push-2 large-2 large-push-2 columns pwd1">
                                        <img alt="valid" src="/Images/Icons/validation_circle_valid.png" class="valid hide" />
                                        <img alt="invalid" src="/Images/Icons/validation_circle_invalid.png" class="invalid" />
                                    </div>
                                    <div class="small-16 medium-16 large-16 medium-push-2 columns text-center">
                                        At least 1 uppercase letter
                                    </div>
                                    <div class="small-4 medium-2 medium-push-2 large-2 large-push-2 columns end pwd2 text-right">
                                        <img alt="valid" src="/Images/Icons/validation_circle_valid.png" class="valid hide" />
                                        <img alt="invalid" src="/Images/Icons/validation_circle_invalid.png" class="invalid" />
                                    </div>
                                </div>

                                <div class="row parameter03">
                                    <div class="small-4 medium-2 medium-push-2 large-2 large-push-2 columns pwd1">
                                        <img alt="valid" src="/Images/Icons/validation_circle_valid.png" class="valid hide" />
                                        <img alt="invalid" src="/Images/Icons/validation_circle_invalid.png" class="invalid" />
                                    </div>
                                    <div class="small-16 medium-16 large-16 medium-push-2 columns text-center">
                                        At least 1 lowercase letter
                                    </div>
                                    <div class="small-4 medium-2 medium-push-2 large-2 large-push-2 columns end pwd2 text-right">
                                        <img alt="valid" src="/Images/Icons/validation_circle_valid.png" class="valid hide" />
                                        <img alt="invalid" src="/Images/Icons/validation_circle_invalid.png" class="invalid" />
                                    </div>
                                </div>

                                <div class="row parameter04">
                                    <div class="small-4 medium-2 medium-push-2 large-2 large-push-2 columns pwd1">
                                        <img alt="valid" src="/Images/Icons/validation_circle_valid.png" class="valid hide" />
                                        <img alt="invalid" src="/Images/Icons/validation_circle_invalid.png" class="invalid" />
                                    </div>
                                    <div class="small-16 medium-16 large-16 medium-push-2 columns text-center">
                                        At least 1 number
                                    </div>
                                    <div class="small-4 medium-2 medium-push-2 large-2 large-push-2 columns end pwd2 text-right">
                                        <img alt="valid" src="/Images/Icons/validation_circle_valid.png" class="valid hide" />
                                        <img alt="invalid" src="/Images/Icons/validation_circle_invalid.png" class="invalid" />
                                    </div>
                                </div>

                                <div class="row parameter05">
                                    <div class="small-4 medium-2 medium-push-2 large-2 large-push-2 columns pwd1">
                                        <img alt="valid" src="/Images/Icons/validation_circle_valid.png" class="valid hide" />
                                        <img alt="invalid" src="/Images/Icons/validation_circle_invalid.png" class="invalid" />
                                    </div>
                                    <div class="small-16 medium-16 large-16 medium-push-2 columns text-center">
                                        Both password match
                                    </div>
                                    <div class="small-4 medium-2 medium-push-2 large-2 large-push-2 columns end pwd2 text-right">
                                        <img alt="valid" src="/Images/Icons/validation_circle_valid.png" class="valid hide" />
                                        <img alt="invalid" src="/Images/Icons/validation_circle_invalid.png" class="invalid" />
                                    </div>
                                </div>

                                <div class="row parameter06">
                                    <div class="small-4 medium-2 medium-push-2 large-2 large-push-2 columns pwd1">
                                        <img alt="valid" src="/Images/Icons/validation_circle_valid.png" class="valid hide" />
                                        <img alt="invalid" src="/Images/Icons/validation_circle_invalid.png" class="invalid" />
                                    </div>
                                    <div class="small-16 medium-16 large-16 medium-push-2 columns text-center">
                                        Alpha-Numeric Only
                                    </div>
                                    <div class="small-4 medium-2 medium-push-2 large-2 large-push-2 columns end pwd2 text-right">
                                        <img alt="valid" src="/Images/Icons/validation_circle_valid.png" class="valid hide" />
                                        <img alt="invalid" src="/Images/Icons/validation_circle_invalid.png" class="invalid" />
                                    </div>
                                </div>

                            </div>
                        </div>
                    </fieldset>
                </div>
            </div>
            <div class="row">
                <div class="small- medium- large- columns">
                    <button type="submit" id="btnContinue" name="btnContinue" class="shikobaButton nextPg button--shikoba green button--round-s button--border-thin right submit">
                        <i class="button__icon fi-next size-24"></i>
                        <span>Continue</span>
                    </button>
                    <asp:Button runat="server" ID="btnCreateAcct" CssClass="hide" ClientIDMode="Static" />
                </div>
            </div>
        </fieldset>
        
    </asp:View>

    <asp:View runat="server" ID="vEmailSent">
        <div class="row">
            <div class="columns">
                <h2>Verification Email Sent</h2>
                <uc:AlertMsg runat="server" MessageType="Success" AlertMsg="A verification email has been sent to the provided email address.  <br class='show-for-medium-up' />Simply click the 'Validate' link provided in the email to activate your account." AdditionalText="" />
            </div>
        </div>

    </asp:View>

    <asp:View runat="server" ID="vAccountCreated">
        <div class="row">
            <div class="columns">
                <h3>Account Created Successfully.</h3>
                <br />
                <uc:AlertMsg runat="server" MessageType="Success" AlertMsg="Congratulations, and welcome to the My Window of Opportunity family!!!" AdditionalText="" />                
                <div class="exclaimLogIn">
                    <br>
                    <br>
                    <h5>So what are you waiting for?<br>Login and get creating already!!!</h5>
                    <br>
                    <br>
                    <a href="/" class="button secondary radius large">Start Now</a>
                    <br>
                    <br>
                </div>
            </div>
        </div>
    </asp:View>

    <asp:View runat="server" ID="vAccountCreated_AddedToCampaign">
        <div class="row">
            <div class="columns">
                <h3>Account Created Successfully.</h3>
                <br />
                <uc:AlertMsg runat="server" MessageType="Success" AlertMsg="Congratulations, and welcome to the My Window of Opportunity family!!!" AdditionalText="" />
                <div class="exclaimLogIn">
                    <br>
                    <br>
                    <h5>So what are you waiting for?<br>Login and get creating already!!!</h5>
                    <br>
                    <br>
                </div>
            </div>
        </div>
    </asp:View>

    <asp:View runat="server" ID="vAccountCreated_NotAddedToCampaign">
        <div class="row">
            <div class="columns">
                <h2>Account Created Successfully.</h2>
                <h3>But... there was an error adding you to the campaign.</h3>
                <br />
                <p>Congratulations, and welcome to the My Window of Opportunity family!!!</p>
                <p>To get assistance with being assigned to the proper campaign, please contact your team administrator.</p>
            </div>
        </div>
    </asp:View>

    <asp:View runat="server" ID="vInvalid">
        <div class="row">
            <div class="columns">
                <h2>Invalid submission.  Please try again.</h2>
                <br />
                <uc:AlertMsg runat="server" MessageType="Alert" AlertMsg="Invalid submission.  Please try again." AdditionalText="" />
            </div>
        </div>
    </asp:View>

</asp:MultiView>
<br />
