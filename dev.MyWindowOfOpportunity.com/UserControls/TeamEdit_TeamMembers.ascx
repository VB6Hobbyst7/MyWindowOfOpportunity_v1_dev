<%@ Control Language="VB" AutoEventWireup="false" CodeFile="TeamEdit_TeamMembers.ascx.vb" Inherits="UserControls_TeamEdit_TeamMembers" %>
<%--<%@ Register TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>--%>
<%@ Register Src="TeamMemberEntry.ascx" TagName="TeamMemberEntry" TagPrefix="uc" %>
<%@ Register Src="AlertMsg.ascx" TagName="AlertMsg" TagPrefix="uc" %>
<%@ Reference Control="TeamMemberEntry.ascx" %>
<%@ Reference Control="AlertMsg.ascx" %>


<div class="row">

    <div class="columns">
        <h2>Team Member Management</h2>
        <p> <%--class="twoColumns"--%>
            Managing who has the ability to edit your campaign is very critical to a successful campaign.  Team Administrators have the ability to edit all campaigns under their team and can also add Campaign Administrators and Members.  
            Campaign Administrators and members can only be added from within a campaign edit page.   Click the <strong>'Add New Team Administrator'</strong> to begin building your campaign’s team.
        </p>
    </div>

    <%--<div class="columns imgSelectPnl hide">
        <uc:MinorImageSelector runat="server" ID="ucMinorImageSelector" activeView="SelectImg" thisTab="TeamMembers" />
    </div>--%>
    <div class="entryPnls">
        <div class="small-24 medium-8 large-7 columns">
            <h6>Entries</h6>

            <ul class="teamMemberFilter" runat="server" id="ucAddNewTeamMember">
                <li>
                    <input type="radio" value="-1" id='rbl_0_teamMember' name="rblTeamMemberBtn" />
                    <label for='rbl_0_teamMember'>
                        <div>+ Add New Team Administrator</div>
                    </label>
                </li>
            </ul>

            <fieldset class="teamMemberList">
                <legend></legend>

                <h6>Team Administrators</h6>
                <hr />
                <asp:ListView runat="server" ID="lvTeamMemberBtns_TeamAdmins">
                    <LayoutTemplate>
                        <ul class="teamMemberFilter">
                            <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                        </ul>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <li>
                            <input type="radio" value='<%#Eval("Id")%>' id='rbl_<%#Eval("Id")%>' name="rblTeamMemberBtn" />
                            <label for='rbl_<%#Eval("Id")%>'>
                                <div><strong><%#Eval("Name")%></strong></div>
                                <div><%#Eval("Role")%></div>
                            </label>
                        </li>
                    </ItemTemplate>
                    <EmptyDataTemplate></EmptyDataTemplate>
                </asp:ListView>

                <br />

            </fieldset>
        </div>
        <div class="hide-for-medium-down large-1 columns">&nbsp;</div>
        <div class="small-24 medium-16 large-16 columns">

            <asp:Panel runat="server" ID="pnlAlertMsg_AddedSuccessfully" Visible="false">
                <uc:AlertMsg runat="server" MessageType="Success" ID="ucAlertMsg_AddedSuccessfully" AlertMsg="An invitatiuon has been submitted to the email address provided.<br />The user will have 48 hours to accept the invitation.<br />Once accepted, the user will be a member of your team." />
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlAlertMsg_ErrorSaving" Visible="false">
                <uc:AlertMsg runat="server" MessageType="Alert" ID="ucAlertMsg_ErrorSaving" AlertMsg="We apologize, but we were unable to send an email to the submitted member." />
            </asp:Panel>
            <asp:PlaceHolder runat="server" ID="phAlertMsgs" />

            <uc:TeamMemberEntry runat="server" ID="ucTeamMemberEntry_New" showTeamAdminOnly="true" />

            <asp:ListView runat="server" ID="lviewTeamAdminEntries">
                <LayoutTemplate>
                    <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                </LayoutTemplate>
                <ItemTemplate>
                    <uc:TeamMemberEntry runat="server" ID="TeamMemberEntry_Edit" thisNodeId='<%#Eval("Id")%>' parentNodeId='<%#Eval("parentNodeId")%>' currentMembersRole='<%#Eval("currentMembersRole")%>' thisMembersRole='<%#Eval("thisMembersRole")%>' showTeamAdminOnly="true" />
                </ItemTemplate>
                <EmptyDataTemplate></EmptyDataTemplate>
            </asp:ListView>
            <asp:ListView runat="server" ID="lviewCampaignAdminEntries">
                <LayoutTemplate>
                    <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                </LayoutTemplate>
                <ItemTemplate>
                    <uc:TeamMemberEntry runat="server" ID="TeamMemberEntry_Edit" thisNodeId='<%#Eval("Id")%>' parentNodeId='<%#Eval("parentNodeId")%>' currentMembersRole='<%#Eval("currentMembersRole")%>' thisMembersRole='<%#Eval("thisMembersRole")%>' showTeamAdminOnly="true" />
                </ItemTemplate>
                <EmptyDataTemplate></EmptyDataTemplate>
            </asp:ListView>
            <asp:ListView runat="server" ID="lviewTeamMemberEntries">
                <LayoutTemplate>
                    <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                </LayoutTemplate>
                <ItemTemplate>
                    <uc:TeamMemberEntry runat="server" ID="TeamMemberEntry_Edit" thisNodeId='<%#Eval("Id")%>' parentNodeId='<%#Eval("parentNodeId")%>' currentMembersRole='<%#Eval("currentMembersRole")%>' thisMembersRole='<%#Eval("thisMembersRole")%>' showTeamAdminOnly="true" />
                </ItemTemplate>
                <EmptyDataTemplate></EmptyDataTemplate>
            </asp:ListView>


        </div>
    </div>
    <br />
    <div class="hide hiddenFields">
        <asp:HiddenField runat="server" ID="hfldTabName" />
        <asp:HiddenField runat="server" ID="hfldThisNodeId" />
        <input type="hidden" value="0" id="hfldActiveTeamMemberNodeId" class="hfldActiveTeamMemberNodeId" />
        <input type="hidden" runat="server" value="false" id="hfldNewTeamMemberEntry" class="hfldNewTeamMemberEntry" />
    </div>
</div>
