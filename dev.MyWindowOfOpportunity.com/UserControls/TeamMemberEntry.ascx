<%@ Control Language="VB" AutoEventWireup="false" CodeFile="TeamMemberEntry.ascx.vb" Inherits="UserControls_TeamMemberEntry" %>
<%@ Register Src="TextBox_Animated.ascx" TagName="TextBox_Animated" TagPrefix="uc" %>
<%@ Register Src="AlertMsg.ascx" TagName="AlertMsg" TagPrefix="uc" %>


<asp:Panel runat="server" ID="pnl" type="teamMemberEntry" CssClass="hide">

    <asp:MultiView runat="server" ID="mvTeamMemberEntry" ActiveViewIndex="0">
        <asp:View runat="server" ID="vNewMember">

            <asp:PlaceHolder runat="server" ID="PlaceHolder1">
                <br />

                <fieldset class="outlinePanel">
                    <legend>Invite New Member</legend>
                    <p>
                        Let's start by entering a valid email address.  
                        If a member already exists with this email address, you will be given an option to associate this member to your campaign.  
                        Otherwise a new member account will be created for the user.
                    </p>

                    <div class="row">
                        <div class="small-24 medium-20 large-22 columns">
                            <uc:TextBox_Animated runat="server" Title="Email Address" ID="txbNewEmailAddress" additionalClass="txbNewEmailAddress" />
                        </div>
                        <div class="small-24 medium-4 large-2 columns end">
                            <a class="button radius secondary validationCheck">&#10004;</a>
                        </div>
                    </div>
                </fieldset>

                <br />

                <div class="row">
                    <div class="small- medium- large-24 columns alertMsgs">
                        <uc:AlertMsg runat="server" isHidden="true" ID="ucAlertMsg1" AlertMsg="Available. Invite new member?" MessageType="Success" />
                        <uc:AlertMsg runat="server" isHidden="true" ID="AlertMsg2" AlertMsg="Please enter a valid email." MessageType="Alert" />
                        <uc:AlertMsg runat="server" isHidden="true" ID="AlertMsg1" AlertMsg="Member exists.  Continue to invite member?" MessageType="Warning" />
                    </div>
                </div>

                <div class="emailValidated_NewUser hide">
                    <asp:PlaceHolder runat="server" ID="phRoleInThisCampaign_NewUser">
                        <fieldset class="secondary outlinePanel">
                            <legend>Role in this campaign</legend>
                            <div class="row">
                                <div class="small- medium-12 large-16 columns">
                                    <div>
                                        Select the role this user is to have.
                                        <ul class="normalUl">
                                            <li><strong>Campaign members</strong> can only manage the campaign they are assigned to. </li>
                                            <li><strong>Campaign administrators</strong> have the additional role of adding and removing members in their assigned campaign. </li>
                                            <li><strong>Team administrators</strong> have full control of all members, administrators and campaigns within their team.</li>
                                        </ul>                                         
                                    </div>
                                </div>
                                <div class="small- medium-12 large-8 columns text-center">
                                    <asp:RadioButtonList runat="server" ID="rblRole_NewMember" CssClass="teamMemberFilter rblRole_NewMember roles small-block-grid-1 medium-block-grid-2 large-block-grid-1 text-left" RepeatLayout="UnorderedList" AutoPostBack="false" />
                                </div>
                            </div>
                        </fieldset>
                    </asp:PlaceHolder>

                    <br />

                    <div class="row">
                        <div class="small- medium- large- small-text-center medium-text-right columns emailValidated ">
                            <div class="button-group" data-grouptype="OR">
                                <asp:Button runat="server" ID="btnCreateNewMember" CssClass="small button accept primary radius" Text="Invite" />
                                <button type="button" class="small button decline radius">Cancel</button>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:PlaceHolder>
        </asp:View>

        <asp:View runat="server" ID="vEditMember">
            <br class="hide-for-small-down" />
            <br class="hide-for-small-down" />
            <br />

            <fieldset class="outlinePanel">
                <legend>Member Information</legend>
                <div id="cphMainContent_ctl02_pnlPhase" class="row ">
                    <div class="summaryPanel ">

                        <div class="hide-for-small-down medium-4 large-3 columns large-text-right">
                            <asp:Image runat="server" ID="imgTeamMember" CssClass="teamMemberImg" />
                        </div>

                        <div class="small-24 medium-20 large-21 columns">
                            <h6 class="inlineBlock">Name:&nbsp;<span class="hide-for-small-down">&nbsp;&nbsp;&nbsp;</span></h6>
                            <strong>
                                <asp:Label runat="server" ID="lblFirstName" CssClass="inlineBlock" />&nbsp;
                                <asp:Label runat="server" ID="lblLastName" CssClass="inlineBlock" />
                            </strong>

                            <br />

                            <h6 class="inlineBlock">Email:&nbsp;<span class="hide-for-small-down">&nbsp;&nbsp;&nbsp;</span></h6>
                            <strong>
                                <asp:HyperLink runat="server" ID="hlnkEmail" CssClass="inlineBlock default" />
                            </strong>
                        </div>

                    </div>
                </div>
            </fieldset>

            <br />
            <asp:PlaceHolder runat="server" ID="phRoleInThisCampaign">
                <fieldset class="secondary outlinePanel">
                    <legend>Role in this campaign</legend>
                    <div class="row">
                        <div class="small-24 medium-12 large-16 columns">
                            <div>
                                Select the role this user is to have.
                                <ul class="normalUl">
                                    <li><strong>Campaign members</strong> can only manage the campaign they are assigned to. </li>
                                    <li><strong>Campaign administrators</strong> have the additional role of adding and removing members in their assigned campaign. </li>
                                    <li><strong>Team administrators</strong> have full control of all members, administrators and campaigns within their team.</li>
                                </ul>   
                            </div>
                        </div>
                        <div class="small-24 medium-12 large-8 columns text-center">
                            <asp:RadioButtonList runat="server" ID="rblRole" CssClass="teamMemberFilter roles rblRole small-block-grid-1 medium-block-grid-2 large-block-grid-1 text-left" RepeatLayout="UnorderedList" AutoPostBack="false" />
                        </div>
                    </div>
                </fieldset>
            </asp:PlaceHolder>
            <br />
            <asp:Panel runat="server" ID="pnlSubmitButtons" CssClass="row">
                <div class="small-24 medium-12 large-6 columns">
                    <asp:LinkButton runat="server" ID="lbtnRemoveMember" ValidationGroup="vg01" CssClass="lbtnRemoveMember shikobaButton nextPg button--shikoba green small button--round-s button--border-thin alert">
                        <i class="button__icon fi-next size-24"></i>
                        <span>Remove</span>
                    </asp:LinkButton>
                    <asp:Button runat="server" ID="btnRemoveMember" Text="" CssClass="btnRemoveMember hide" />
                </div>
                <div class="small-24 medium-12 large-6 columns">
                    <asp:LinkButton runat="server" ID="lbtnSaveMember" ValidationGroup="vg01" CssClass="lbtnSaveMember shikobaButton nextPg button--shikoba green small button--round-s button--border-thin">
                        <i class="button__icon fi-next size-24"></i>
                        <span>Save</span>
                    </asp:LinkButton>
                </div>
            </asp:Panel>

        </asp:View>
    </asp:MultiView>


    <div class="hide hiddenFields">
        <asp:HiddenField runat="server" ID="hfldThisMembersRole" />
        <asp:HiddenField runat="server" ID="hfldCurrentMembersRole" />
        <asp:HiddenField runat="server" ID="hfldShowTeamAdminOnly" Value="false" />
        <input type="hidden" runat="server" id="hfldNodeId" class="hfldNodeId" value="0" />
        <input type="hidden" runat="server" id="hfldIsNewMember" class="hfldIsNewMember" value="false" />
    </div>
</asp:Panel>



