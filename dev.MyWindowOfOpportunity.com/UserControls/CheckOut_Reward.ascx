<%@ Control Language="VB" AutoEventWireup="false" CodeFile="CheckOut_Reward.ascx.vb" Inherits="UserControls_CheckOut_Reward" %>








<%--<div id="divRewardStat" class="row radius panel callout inactive">

    <div class="small-1 columns hide">--%>

        <%--<input type="radio" id="RBTAuto" name="SelectReward"  runat="server" />
       <asp:HiddenField ID="HdRewardID" runat="server" />--%>
        <%--<asp:Literal ID="ltrRadio" runat="server" />
    </div>

    <div class="small-23 columns" style="color: #316dbc;">

        <div class="row">

            <div class="small-18 columns">
                <h3 class="ltrlamount" style="color: white; text-shadow: 1px 1px 1px #316dbc;">
                    <asp:Literal ID="ltrlContributionAmount" runat="server"></asp:Literal>

                </h3>
                <h4 style="color: white; text-shadow: 1px 1px 1px #316dbc; font-size: 20px; margin: -10px 0 0 0;">
                    <asp:Literal runat="server" ID="ltrlHeading" />

                </h4>
            </div>
            <div class="small-6 columns text-right">
                <asp:LinkButton ID="LinkButton1" CssClass="shikobaButton nextPg button--shikoba small button--round-s button--border-thin right" runat="server" ValidationGroup="vgEndorsement01" OnClientClick="CheckContinue()">
                 <i class="button__icon fi-next size-24"></i>
                  <span>Continue</span>
                </asp:LinkButton>
            </div>
        </div>

        <p style="line-height: 18px; font-size: 14px; margin: 12px 0; color: #316dbc;">
            <asp:Image runat="server" ID="imgFeatured" CssClass="small-6 right" />
            <asp:Literal runat="server" ID="ltrlShortDescription" />
        </p>

        <div class="row" style="font-size: 14px; font-weight: bold;" id="Additional" runat="server">
            <div class="small-6 columns">
                Available: &nbsp;<span style="font-weight: normal;"><asp:Literal runat="server" ID="ltrlAvailable" /></span>
            </div>
            <div class="small-6 columns">
                Claimed: &nbsp;<span style="font-weight: normal;"><asp:Literal runat="server" ID="ltrlClaimed" /></span>
            </div>
            <div class="small-12 columns">
                Estimated Shipping Date: &nbsp;<span style="font-weight: normal;"><asp:Literal runat="server" ID="ltrlShippingDate" /></span>
            </div>
        </div>

    </div>
</div>
<br />--%>
