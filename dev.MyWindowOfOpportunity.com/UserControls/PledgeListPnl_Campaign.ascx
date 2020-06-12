<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PledgeListPnl_Campaign.ascx.vb" Inherits="UserControls_Pledges_Listing" %>
<%@ Register Src="PledgeListPhasePnl_Campaign.ascx" TagName="PledgeListPhasePnl_Campaign" TagPrefix="uc" %>


<%--<h1>CAMPAIGN PAGE</h1>--%>
<%--<asp:GridView runat="server" ID="gv" /><hr />
<asp:GridView runat="server" ID="GridView1" /><hr />
<asp:GridView runat="server" ID="GridView2" /><hr />
<asp:GridView runat="server" ID="GridView3" />--%>


<fieldset>
    <legend>Pledge History</legend>
    <div class="row">
        <div class="columns">
            <uc:PledgeListPhasePnl_Campaign runat="server" phaseNo="3" ID="ucPhase03" />
            <uc:PledgeListPhasePnl_Campaign runat="server" phaseNo="2" ID="ucPhase02" />
            <uc:PledgeListPhasePnl_Campaign runat="server" phaseNo="1" ID="ucPhase01"  />
            
            <asp:Panel runat="server" ID="pnlNoPledges" CssClass="text-center" Visible="false">
                <br />
                <h5>No pledges have been submitted yet.</h5>
                <br />
            </asp:Panel>            
        </div>
    </div>
</fieldset>      