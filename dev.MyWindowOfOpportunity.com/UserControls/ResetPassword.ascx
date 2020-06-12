<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ResetPassword.ascx.vb" Inherits="UserControls_ResetPassword" %>
<%@ Register Src="/UserControls/TextBox_Animated.ascx" TagName="TextBox_Animated" TagPrefix="uc" %>

<%@ Register TagPrefix="uc" Src="DateDropDowns.ascx" TagName="DateDropDowns" %>
<%@ Register TagPrefix="uc" Src="/UserControls/AlertMsg.ascx" TagName="AlertMsg" %>

<asp:MultiView ID="SetupResetPassowrd" runat="server" ActiveViewIndex="0">

    <asp:View ID="vDefault" runat="server">
        <div class="row">
            <div class="small-24 columns text-center">
                <br />
                <br />
                <h3>To reset your password, please provide your login credentials.</h3>
                <br />
                <br />
            </div>
        </div>

        <div class="row">
            <div class="small-24 medium-16 large-6 large-push-6 columns">
                <uc:TextBox_Animated runat="server" ID="ucTxbEmail" Title="Email" TextMode="_Email" isRequired="true" />
            </div>
            <div class="small-24 medium-8 large-6 large-push-6 columns end">
                <div class="">
                    <button type="submit" id="btnResetPass" onclick="fPass()" class="shikobaButton nextPg button--shikoba green button--round-s button--border-thin right submit mblFullWidth tblFullWidth">
                        <i class="button__icon fi-next size-24"></i>
                        <span>Validate Email Address</span>
                    </button>

                </div>
                <asp:Label ID="InvaildEmailMessage" runat="server" Text="Email does not exist." CssClass="initialText" Visible="false"></asp:Label>
            
            </div>
        </div>

        <br />
        <br />
        <br />

    </asp:View>

    <asp:View ID="vInfo" runat="server">
        <div class="row">
            <div class="large-24 columns text-center">
                <br />
                <br />
                <br />
                <h3>An email has been sent to the provided address. Once verified, you will be able to reset the password.</h3>
                <br />
                <br />
                <br />
            </div>
        </div>

    </asp:View>

    <asp:View ID="vResetpassword" runat="server">
        <div class="row">
            <div class="columns text-center">
                <br />
                <h3>Please provide a new password for your account.</h3>
                <br />
            </div>
        </div>

        <div class="row">
            <div class="medium-12 medium-push-6 columns end">
                <fieldset class="passwordPnl">
                    <legend></legend>
                    <div class="changePasswordPnl">
                        <div class="row">
                            <div class="large-12 columns">
                                <uc:TextBox_Animated runat="server" ID="txbPassword" Title="Password" additionalClass="txbPassword" isRequired="True" />
                            </div>
                            <div class="large-12 columns">
                                <uc:TextBox_Animated runat="server" ID="txbPasswordReenter" Title="Re-enter Password" additionalClass="txbPasswordReenter" isRequired="True" />
                            </div>
                        </div>

                        <div class="passwordValidations">
                            <div class="row parameter01">
                                <div class="large-2 large-push-4 columns pwd1">
                                    <img alt="valid" src="/Images/Icons/validation_circle_valid.png" class="valid hide" />
                                    <img alt="invalid" src="/Images/Icons/validation_circle_invalid.png" class="invalid" />
                                </div>
                                <div class="large-12 large-push-4 columns text-center">
                                    6+ characters
                                </div>
                                <div class="large-2 large-push-4 columns end pwd2">
                                    <img alt="valid" src="/Images/Icons/validation_circle_valid.png" class="valid hide" />
                                    <img alt="invalid" src="/Images/Icons/validation_circle_invalid.png" class="invalid" />
                                </div>
                            </div>

                            <div class="row parameter02">
                                <div class="large-2 large-push-4 columns pwd1">
                                    <img alt="valid" src="/Images/Icons/validation_circle_valid.png" class="valid hide" />
                                    <img alt="invalid" src="/Images/Icons/validation_circle_invalid.png" class="invalid" />
                                </div>
                                <div class="large-12 large-push-4 columns text-center">
                                    At least 1 uppercase letter
                                </div>
                                <div class="large-2 large-push-4 columns end pwd2">
                                    <img alt="valid" src="/Images/Icons/validation_circle_valid.png" class="valid hide" />
                                    <img alt="invalid" src="/Images/Icons/validation_circle_invalid.png" class="invalid" />
                                </div>
                            </div>

                            <div class="row parameter03">
                                <div class="large-2 large-push-4 columns pwd1">
                                    <img alt="valid" src="/Images/Icons/validation_circle_valid.png" class="valid hide" />
                                    <img alt="invalid" src="/Images/Icons/validation_circle_invalid.png" class="invalid" />
                                </div>
                                <div class="large-12 large-push-4 columns text-center">
                                    At least 1 lowercase letter
                                </div>
                                <div class="large-2 large-push-4 columns end pwd2">
                                    <img alt="valid" src="/Images/Icons/validation_circle_valid.png" class="valid hide" />
                                    <img alt="invalid" src="/Images/Icons/validation_circle_invalid.png" class="invalid" />
                                </div>
                            </div>

                            <div class="row parameter04">
                                <div class="large-2 large-push-4 columns pwd1">
                                    <img alt="valid" src="/Images/Icons/validation_circle_valid.png" class="valid hide" />
                                    <img alt="invalid" src="/Images/Icons/validation_circle_invalid.png" class="invalid" />
                                </div>
                                <div class="large-12 large-push-4 columns text-center">
                                    At least 1 number
                                </div>
                                <div class="large-2 large-push-4 columns end pwd2">
                                    <img alt="valid" src="/Images/Icons/validation_circle_valid.png" class="valid hide" />
                                    <img alt="invalid" src="/Images/Icons/validation_circle_invalid.png" class="invalid" />
                                </div>
                            </div>

                            <div class="row parameter05">
                                <div class="large-2 large-push-4 columns pwd1">
                                    <img alt="valid" src="/Images/Icons/validation_circle_valid.png" class="valid hide" />
                                    <img alt="invalid" src="/Images/Icons/validation_circle_invalid.png" class="invalid" />
                                </div>
                                <div class="large-12 large-push-4 columns text-center">
                                    Both password match
                                </div>
                                <div class="large-2 large-push-4 columns end pwd2">
                                    <img alt="valid" src="/Images/Icons/validation_circle_valid.png" class="valid hide" />
                                    <img alt="invalid" src="/Images/Icons/validation_circle_invalid.png" class="invalid" />
                                </div>
                            </div>

                            <div class="row parameter06">
                                <div class="large-2 large-push-4 columns pwd1">
                                    <img alt="valid" src="/Images/Icons/validation_circle_valid.png" class="valid hide" />
                                    <img alt="invalid" src="/Images/Icons/validation_circle_invalid.png" class="invalid" />
                                </div>
                                <div class="large-12 large-push-4 columns text-center">
                                    Alpha-Numeric Only
                                </div>
                                <div class="large-2 large-push-4 columns end pwd2">
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
            <div class="medium-12 medium-push-6 columns end">
                <div class="right">
                    <button type="button" id="btnSubmitReset" onclick="fReset()" class="shikobaButton nextPg button--shikoba green button--round-s button--border-thin right submit">
                        <i class="button__icon fi-next size-24"></i>
                        <span>Reset Password</span>
                    </button>
                </div>
            </div>
        </div>
    </asp:View>

    <asp:View ID="vSuccess" runat="server">

        <div class="row collapse">
            <div class="columns text-center">
                <br />
                <h3>Congratulations, you again have access to your account.</h3>
                <br />
                <asp:HyperLink ID="LoginUrl" runat="server" CssClass="button radius">Login</asp:HyperLink>
                <br />
                <br />
                <br />
            </div>
        </div>

    </asp:View>

</asp:MultiView>

<input id="btnPassWord" type="button" runat="server" class="hide" />
<input id="btnReset" type="button" runat="server" class="hide" />
<script>
    function fPass() {
        console.log('fpass Started');
        var btnResetSubmit = $('#cphMainContent_ctl00_ResetPassword_1_btnPassWord');
        btnResetSubmit.trigger("click");
        console.log('fpass Complete');
    }
    function fReset() {
        console.log('fReset started');
        var btnSubmitResetRequest = $('#cphMainContent_ctl00_ResetPassword_1_btnReset');
        btnSubmitResetRequest.trigger("click");
        console.log('fReset complete');
    }
</script>
<%--<asp:Button ID="btnPassWord" runat="server" ClientIDMode="Static" CssClass="hide" />
 <asp:Button ID="btnReset" runat="server" ClientIDMode="Static" CssClass="hide" /> 
<input id="testme" type="button" runat="server" name="test" value="LOL" />
<asp:Label ID="Label1" Text="text" runat="server" />--%>