<%@ Control Language="VB" AutoEventWireup="false" CodeFile="editTab_Phases.ascx.vb" Inherits="UserControls_editTab_Phases" %>
<%@ Register Src="TextBox_Animated.ascx" TagName="TextBox_Animated" TagPrefix="uc" %>
<%@ Reference Control="PhaseEntry.ascx" %>
<%@ Reference Control="AlertMsg.ascx" %>


<div>
    <asp:HiddenField runat="server" ID="hfldTabName" />
    <asp:HiddenField runat="server" ID="hfldThisNodeId" />

    <div class="row">
        <div class="small-24 medium-24 large-24 columns">
            <h2>Campaign Phases</h2>
            <asp:Panel runat="server" ID="pnlInstructions" CssClass="twoColumns">
                <p><strong class="larger">Phases: </strong>A campaign can consist of up to 3 different phases, allowing you to divide your campaign's needs into more manageable segments. Our multi-phase campaign platform allows you to run an active phase while working on the next one. Additionally, by dividing your project into segments you can set smaller goals that are easier to achieve.</p>
                <p><strong class="larger">Discovery: </strong>Not sure if there is enough interest to support your idea?  The discovery phase will allow you to gauge the public's interest and help you determine if your network is large enough to fund your ideas.  After publishing your campaign simply click the option to launch a discovery phase.</p>
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlCharityInstructions" CssClass="row" Visible="false">
                <blockquote>
                    <div>Please note that charity campaigns function different than a normal campaign.  These differences consist of: </div>
                    <blockquote>
                        <ul class="diamondDisc">
                            <li>Charity campaigns will be reviewed for authenticity. Please review our <asp:HyperLink runat="server" ID="HyperLink1">Rules & Guidelines</asp:HyperLink> for further instructions. </li>
                            <%--<li>A single phase lasting up to 45 days.</li>--%>
                            <li>All donations are transfered to the campaign administrator regardless if the goal was met or not.</li>
                            <li>No discovery phases are provided.</li>
                            <li>My Window of Opportunity does not charge any fees for Charities, although credit card charges and processing fees will be applied just the same.</li>
                        </ul>
                    </blockquote>
                </blockquote>
            </asp:Panel>
            <br />
        </div>

        <div class="small-24 medium-7 large-5 columns">
            <h6>Phases</h6>
            <asp:RadioButtonList runat="server" ID="rbl" CssClass="phaseFilter" RepeatLayout="UnorderedList" />
        </div>
        <div class="hide-for-small-down medium-1 large-1 columns">&nbsp;</div>
        <asp:Panel runat="server" ID="pnlPhaseEntries" CssClass="small-24 medium-16 large-18 columns">
            <br class="show-for-small-down" />
            <h6>Phase Description</h6>
            <asp:PlaceHolder runat="server" ID="phAlertMsg" />
            <asp:PlaceHolder runat="server" ID="phPhases" />
        </asp:Panel>
    </div>

    <div class="row">
        <div class="small-24 medium-16 medium-push-8 large-4 large-push-20 columns">
            <asp:LinkButton runat="server" ID="lbtnSave" ValidationGroup="vg01" CssClass="lbtnSavePhase shikobaButton nextPg button--shikoba green small button--round-s button--border-thin">
                <i class="button__icon fi-next size-24"></i>
                <span>Save</span>
            </asp:LinkButton>
        </div>
    </div>
</div>
