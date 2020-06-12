<%@ Control Language="VB" AutoEventWireup="false" CodeFile="DateDropDowns.ascx.vb" Inherits="UserControls_DateDropDowns" %>


<%--<div class="example text-left">
    <div>&nbsp;Date of Birth</div>
    <div class="dobDateDropdowns"></div>
</div>--%>


<asp:UpdatePanel runat="server" ID="upCreateAcct" UpdateMode="Conditional" ChildrenAsTriggers="true" >
    <ContentTemplate>
        <div class="row">
            <div class="small-23 small-push-1 columns bold">Date of Birth
                <asp:RequiredFieldValidator ID="rfvMonth" runat="server" ControlToValidate="ddlMonth" Display="Dynamic" ForeColor="Red" Enabled="false" InitialValue="" ErrorMessage="DOB - Month is required.">*</asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator ID="rfvDay" runat="server" ControlToValidate="ddlDay" Display="Dynamic" ForeColor="Red" Enabled="false" InitialValue="" ErrorMessage="DOB - Day is required.">*</asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator ID="rfvYear" runat="server" ControlToValidate="ddlYear" Display="Dynamic" ForeColor="Red" Enabled="false" InitialValue="" ErrorMessage="DOB - Year is required.">*</asp:RequiredFieldValidator>
            </div>

            <div class="small-10 small-push-1 columns">
                <asp:DropDownList runat="server" ID="ddlMonth" CssClass="radius" AutoPostBack="true" AppendDataBoundItems="true">
                    <asp:ListItem Value="" Text="Month" />
                </asp:DropDownList>
            </div>
            <div class="small-6 small-push-1 columns">
                <asp:DropDownList runat="server" ID="ddlDay" CssClass="radius" AutoPostBack="true" Enabled="false" AppendDataBoundItems="true">
                    <asp:ListItem Value="" Text="Day" />
                </asp:DropDownList>
            </div>
            <div class="small-6 small-push-1 columns end">
                <asp:DropDownList runat="server" ID="ddlYear" CssClass="radius" Enabled="false" AppendDataBoundItems="true">
                    <asp:ListItem Value="" Text="Year" />
                </asp:DropDownList>
            </div>

        </div>
    </ContentTemplate>
</asp:UpdatePanel>