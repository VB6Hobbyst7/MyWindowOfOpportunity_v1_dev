<%@ Control Language="VB" AutoEventWireup="false" CodeFile="TextBox_Animated.ascx.vb" Inherits="UserControls_TextBox_Animated" %>



<span class="input input--manami" runat="server" id="inputOuterPnl">
    <asp:TextBox runat="server" ID="txb" CssClass="input__field input__field--manami" AutoCompleteType="None" />
    <label class="input__label input__label--manami">
        <span class="input__label-content input__label-content--manami">
            <asp:Literal runat="server" ID="ltrlTitle" /> 
            <asp:RequiredFieldValidator ID="rfv" runat="server" ControlToValidate="txb" Display="Dynamic" ForeColor="Red" Enabled="false" CssClass="errorMsg">*</asp:RequiredFieldValidator>
            <asp:Label runat="server" ID="lblCustomErrorMgs" CssClass="errorMsg" Text="*" Visible="false" />
        </span>
    </label>
</span>

