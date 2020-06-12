<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ShipTo.ascx.vb" Inherits="UserControls_ShipTo" %>
<%--<%@ Register Src="/UserControls/AlertMsg.ascx" TagName="AlertMsg" TagPrefix="uc" %>--%>


<fieldset class="outlinePanel secondary shipTo" runat="server" id="pnlFieldset">
    <div class="row"">

        <div class="small-24 medium-12 large-5 columns">
            <h6>Shipping Address</h6>
            <br />
            <asp:Label runat="server" ID="lblShippingInfo" />
        </div>

        <div class="small-24 medium-12 large-3 columns">
            <h6>Fulfilled</h6>
            <br />
            <div class="switch radius">
                <input type="checkbox" id="cbFulfilled" runat="server" class="cbFulfilled" />
                <label runat="server" id="lblFulfilled">
                    <span class="switch-on">Yes</span>
                    <span class="switch-off">No</span>
                </label>
            </div>
            <div>  
                <br />              
                <asp:HyperLink runat="server" ID="hlnkMailTo" Visible="false" CssClass="button" NavigateUrl="mailto:" Text="Email Pledger" />
            </div>
        </div>
        
        <div class="hide-for-medium-down medium- large-1 columns">
            &nbsp;
        </div>

        <div class="small-24 medium-12 large-6 columns">
            <h6>Selected Reward</h6>
            <br />
            <asp:Label runat="server" ID="lblReward" />
            <br />
            <h6>Tracking No</h6>
            <br />
            <asp:TextBox runat="server" ID="txbTrackingNo" CssClass="txbTrackingNo" placeholder="*Optional - For your records only" />
        </div>

        <div class="small-24 medium-12 large-6 columns">
            <h6>Your Notes</h6>
            <br />
            <asp:TextBox runat="server" ID="txbNotes" CssClass="txbNotes" Rows="4" TextMode="multiline" placeholder="*Optional - For your records only" />
        </div>

        <div class="small-12 medium-8 large-3 columns text-right">
            <div>
                <br />
                <asp:HyperLink runat="server" ID="hlnkSave" CssClass="button saveShipToBtn" Text="Save" />
            </div>
        </div>
        


        <div class="close">
            <img alt="close" src="/Images/Icons/square_invalid_Inactive.png" />
            <img alt="close" src="/Images/Icons/square_invalid.png" />
        </div>

        <%--<uc:AlertMsg runat="server" ID="ucAlertMsg_Success" isHidden="true" MessageType="Success" AlertMsg="Saved Successfully" />--%>
        
    </div>
</fieldset>
