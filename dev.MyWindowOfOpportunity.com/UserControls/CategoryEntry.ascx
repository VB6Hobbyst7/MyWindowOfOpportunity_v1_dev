<%@ Control Language="VB" AutoEventWireup="false" CodeFile="CategoryEntry.ascx.vb" Inherits="UserControls_CategoryEntry" %>


<asp:Panel runat="server" ID="pnl">
    <asp:CheckBoxList runat="server" ID="cbl" />

    <asp:Placeholder runat="server" ID="phCharityInstructions" Visible="false">
        <hr />
        <div class="phCharityInstructions">
            <strong>Note: Charity campaigns opperate the same as a normal campaign but with the following exceptions:</strong>
            <br>
            <blockquote>
                ◈ Charity campaigns will be reviewed for authenticity.  Please review our <asp:HyperLink runat="server" ID="hlnkRules" Text="Rules & Guidelines" /> for further instructions.
                <br>◈ No discovery phases are provided.
                <br>◈ Only 1 phase is provided.
                <br>◈ My Window of Opportunity does not charge any fees for Charities, although credit card charges and processing fees will be applied just the same. 
                <br>◈ All donations are transfered to the campaign administrator regardless if the goal was met or not.
            </blockquote>
        </div>


<style>
.phCharityInstructions {font-size: 15px;font-family: 'RobotoCondensed-Bold';color: #29abe2;font-style: italic;text-transform: none;margin-left: 18px;}
.organicTabs li .phCharityInstructions a {font-size: 15px;font-family: 'RobotoCondensed-Bold';font-style: italic;text-transform: none;display: inline;border-color: #29abe2;padding: 0;}
.organicTabs li .phCharityInstructions a:hover {/*font-size: 15px;font-family: 'RobotoCondensed-Bold';font-style: italic;text-transform: none;display: inline;border-color: #29abe2;padding: 0;*/color: #29abe2;background-color: transparent;}
</style>

    </asp:Placeholder>

</asp:Panel>

