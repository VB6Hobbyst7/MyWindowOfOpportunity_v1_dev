<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ManageCreditcard.ascx.vb" Inherits="UserControls_ManageCreditcard" %>
<%@ Register TagPrefix="uc"     Src="/UserControls/TextBox_Animated.ascx"   TagName="TextBox_Animated" %>
<%@ Register TagPrefix="uc"     Src="/UserControls/AlertMsg.ascx"           TagName="AlertMsg" %>



<style>
@media only screen and (max-width: 40em) {.FinancialManagement .input {width: 100%;}}
</style>

<%--<div style="overflow:scroll;">
        <asp:GridView runat="server" ID="gv" />
    </div>--%>

<br />
<div class="FinancialManagement">
    <div class="row">
        <div class="small- medium- large-24 columns alertMsgs">
            <uc:alertmsg runat="server" clientidmode="Static" ishidden="true" id="ucAlertMsg_Success" messagetype="Success" alertmsg="" additionaltext="" additionalClass="cc" />
            <uc:alertmsg runat="server" clientidmode="Static" ishidden="true" id="ucAlertMsg_Error" messagetype="Alert" alertmsg="" additionaltext="" additionalClass="cc" />
            <uc:alertmsg runat="server" clientidmode="Static" ishidden="true" id="ucAlertMsg_Info" messagetype="Info" alertmsg="A creditcard is assigned to this account." additionaltext="" additionalClass="cc" />
            <%--<span class="errors"></span>--%>
        </div>
    </div>

    <div class="row collapseInput statusPnl">
        <div class="small-24 medium-10 large-12 columns text-center">
            <br class="show-for-medium-up" />
            <p class="larger">Want to donate to a campaign?  Great!</p>
            <p>With <strong>My Window of Opportunity</strong>, you can use any of the following cards:</p>
            <br class="show-for-medium-up" />
            <div class="row">
                <div class="small- medium- large- columns">
                    <br />
                    <img src="/Images/Icons/stripeCheckout.png" />
                    <h5>Credit, Debit, Gift and Prepaid cards accepted</h5>
                </div>
            </div>

            <%--<br />
            <br />
            <p>Explanations/Instructions to go here.</p>
            <br class="show-for-small-down" />--%>
        </div>
        <div class="small-24 medium-14 large-12 columns">
            <fieldset class="summaryPanel">
                <legend>Card Information</legend>
                <div class="row">
                    <div class="small- medium- large-20 large-push-2 columns">
                        <br />
                        <div class="row collapse">
                            <div class="small- medium- large-12 columns text-center">
                                <span class="input input--manami txbNameOnCard">
                                    <input type="text" id="txbNameOnCard" class="input__field input__field--manami" title="Name on Card">
                                    <label class="input__label input__label--manami">
                                        <span class="input__label-content input__label-content--manami">Name on Card		                                   
                                                <span class="errorMsg">*</span>
                                        </span>
                                    </label>
                                </span>
                            </div>
                            <div class="small- medium- large-12 columns text-center">
                                <span class="input input--manami txbPostalCode">
                                    <input type="text" id="txbPostalCode" class="input__field input__field--manami" title="Postal Code" required onkeypress="return isNumberKey(event)">
                                    <label class="input__label input__label--manami">
                                        <span class="input__label-content input__label-content--manami">Postal Code			                                   
                                                <span class="errorMsg">*</span>
                                        </span>
                                    </label>
                                </span>
                            </div>

                        </div>
                        <br />
                        <div class="row collapse">
                            <div class="columns text-center">
                                <span class="input input--manami txbCreditCard relative">                                    
                                    <image id="card_imag" class="icon-show-hide" src="/Images/Cards/icon-card.svg" alt=""></image>
                                    <%--   <i class="fa fa-credit-card-alt icon-show-hide" aria-hidden="true"></i>--%>
                                    <input type="text" id="txbCreditCard" class="input__field input__field--manami input-1" title="Credit Card" placeholder="4242 4242 4242 4242" size="16" required>
                                    <input type="text" id="txbCCExpirationMonth" class="input__field input__field--manami hide-text-box input-2" placeholder="MM" title="Expiration Month" size="2" pattern="[0-9]{2}" required maxlength="2" onkeypress="return isNumberKey(event)">
                                    <%--<span class="slash-show-hide">/</span>--%>
                                    <input type="text" id="txbCCExpirationYear" class="input__field input__field--manami hide-text-box  input-3" title="Expiration Year" placeholder="YY" size="2" pattern="[0-9]{2}" required maxlength="2" onkeypress="return isNumberKey(event)">
                                    <input type="text" id="txbCvcCode" class="input__field input__field--manami hide-text-box  input-4" title="CVC Code" placeholder="CVC" size="4" pattern="[0-9]{3,4}" required maxlength="4" onkeypress="return isNumberKey(event)">
                                    <label class="input__label input__label--manami">
                                        <span class="input__label-content input__label-content--manami">Card	                                   
                                            <span class="errorMsg">*</span>
                                        </span>
                                    </label>
                                </span>
                            </div>
                        </div>



                        <asp:Panel runat="server" ID="pnlSubmit" CssClass="row" ClientIDMode="Static">
                            <div class="columns text-right">
                                <br />
                                <br />
                                <button type="submit" id="btnSubmit" class="button radius submit">Submit</button>
                            </div>
                        </asp:Panel>
                        <asp:Panel runat="server" ID="pnlUpdate" CssClass="row" ClientIDMode="Static">
                            <div class="small- medium- large-24 columns">
                                <br />
                                <br />
                                <button type="submit" id="btnUpdate" class="button radius submit left mblFullWidth">Replace</button>
                                <br class="show-for-small-down" />
                                <button type="button" id="btnDelete" class="button radius alert submit right mblFullWidth">Delete Existing Card</button>
                            </div>
                        </asp:Panel>

                    </div>
                </div>
            </fieldset>
        </div>
    </div>

    <div class="hide hiddenFields">
        <asp:HiddenField runat="server" ID="hfldPublicKey" ClientIDMode="Static" />
        <%--<asp:HiddenField runat="server" ID="hfldCurrentUserId" ClientIDMode="Static" />--%>
        <asp:HiddenField runat="server" ID="hfldCustomerId" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hfldCardId" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hfldCardToken" ClientIDMode="Static" />

        <asp:HiddenField runat="server" ID="hfldName" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hfldBrand" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hfldPostalCode" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hfldLast4" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hfldExpirationMonth" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hfldExpirationYear" ClientIDMode="Static" />

        <asp:HiddenField runat="server" ID="hfldShowAlertMsg_Success" ClientIDMode="Static" Value="False" />
        <asp:HiddenField runat="server" ID="hfldShowAlertMsg_Info" ClientIDMode="Static" Value="False" />
        <asp:HiddenField runat="server" ID="hfldShowAlertMsg_Error" ClientIDMode="Static" Value="False" />
        <asp:HiddenField runat="server" ID="hfldAlertMsg_Error" ClientIDMode="Static" />
    </div>






    <div class="row hide">
        <div class="columns">
            <fieldset class="displaysampleCredit">
                <legend class="wbackgroundColorRed">TEST CARDS</legend>
                <div class="row">
                    <div class="columns">
                        <strong>4242424242424242</strong> = Visa 
                            <br />
                        <strong>4000056655665556</strong> = Visa debit
                            <br />
                        <strong>4000001240000000</strong> = Visa canada
                            <br />
                        <strong>4000000000000077</strong> = Charge succeeds and funds will be added directly to your available balance (bypassing your pending balance).
                            <br />
                        <strong>4000000000000010</strong> = The address_line1_check and address_zip_check verifications fail. If your account is blocking payments that fail ZIP code validation, the charge is declined.
                            <br />
                        <strong>4000000000000028</strong> = Charge succeeds but the address_line1_check verification fails.
                            <br />
                        <strong>4000000000000036</strong> = The address_zip_check verification fails. If your account is blocking payments that fail ZIP code validation, the charge is declined.
                            <br />
                        <strong>4000000000000044</strong> = Charge succeeds but the address_zip_check and address_line1_check verifications are both unavailable.
                            <br />
                        <strong>4000000000000101</strong> = If a CVC number is provided, the cvc_check fails. If your account is blocking payments that fail CVC code validation, the charge is declined.
                            <br />
                        <strong>4000000000000341</strong> = Attaching this card to a Customer object succeeds, but attempts to charge the customer fail.
                            <br />
                        <strong>4000000000009235</strong> = Charge succeeds with a risk_level of elevated and placed into review.
                            <br />
                        <strong>4000000000000002</strong> = Charge is declined with a card_declined code.
                            <br />
                        <strong>4100000000000019</strong> = Charge is declined with a card_declined code and a fraudulent reason.
                            <br />
                        <strong>4000000000000127</strong> = Charge is declined with an incorrect_cvc code.
                            <br />
                        <strong>4000000000000069</strong> = Charge is declined with an expired_card code.
                            <br />
                        <strong>4000000000000119</strong> = Charge is declined with a processing_error code.
                            <br />
                        <strong>4242424242424241</strong> = Charge is declined with an incorrect_number code as the card number fails the Luhn check.
                            <br />
                        <strong>4000000000000259</strong> = Charge succeeds, only to be disputed.
                    </div>
                </div>
            </fieldset>
        </div>
    </div>




</div>
