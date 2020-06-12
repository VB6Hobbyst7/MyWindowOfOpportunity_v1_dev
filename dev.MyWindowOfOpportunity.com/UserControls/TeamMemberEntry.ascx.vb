Imports Common
Imports umbraco
Imports umbraco.Core.Models
Imports umbraco.Web

Partial Class UserControls_TeamMemberEntry
    Inherits System.Web.UI.UserControl


#Region "Properties"
    Private _parentNodeId As Integer = 0
    Private _currentUserId As Integer = 0
    Private blMembers As blMembers
    Private blTeams As blTeams
    Dim _uHelper As Uhelper = New Uhelper()

    Private Enum _view
        NewMember = 0
        EditMember = 1
    End Enum

    Public Property thisNodeId() As Integer
        Get
            Return CInt(hfldNodeId.Value)
        End Get
        Set(value As Integer)
            hfldNodeId.Value = value.ToString
        End Set
    End Property
    Public Property parentNodeId() As Integer
        Get
            Return _parentNodeId
        End Get
        Set(value As Integer)
            _parentNodeId = value
        End Set
    End Property
    Public Property currentUserId() As Integer
        Get
            Return _currentUserId
        End Get
        Set(value As Integer)
            _currentUserId = value
        End Set
    End Property
    Public Property currentMembersRole As String
        Get
            Return hfldCurrentMembersRole.Value
        End Get
        Set(value As String)
            hfldCurrentMembersRole.Value = value
        End Set
    End Property
    Public Property thisMembersRole As String
        Get
            Return hfldThisMembersRole.Value
        End Get
        Set(value As String)
            hfldThisMembersRole.Value = value
        End Set
    End Property
    Public Property showTeamAdminOnly() As Boolean
        Get
            Return CBool(hfldShowTeamAdminOnly.Value)
        End Get
        Set(value As Boolean)
            hfldShowTeamAdminOnly.Value = value.ToString
        End Set
    End Property
#End Region

#Region "Handles"
    Private Sub UserControls_TeamMemberEntry_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Context.Request.QueryString(queryParameters.newMember) IsNot Nothing Then
                If Context.Request.QueryString(queryParameters.newMember) = True Then
                    Session(Sessions.successMsg) = "Member has been added successfully"
                End If
            End If

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\TeamMemberEntry.ascx.vb : UserControls_TeamMemberEntry_Load()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
    End Sub
    Private Sub UserControls_TeamMemberEntry_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        Try
            If thisNodeId <> 0 Then
                'Add IPublishedContent Id to panel as an attribute
                pnl.Attributes.Add("nodeId", thisNodeId)

                If thisNodeId = -1 Then
                    'Set panel to show options to create a new member.
                    mvTeamMemberEntry.ActiveViewIndex = _view.NewMember

                    'Populate radiobuttonlist with member roles
                    populateMemberRoles(rblRole_NewMember)

                    'Show/Hide role section
                    phRoleInThisCampaign_NewUser.Visible = Not showTeamAdminOnly
                Else
                    'Set panel to show edit mode
                    mvTeamMemberEntry.ActiveViewIndex = _view.EditMember

                    'Populate radiobuttonlist with member roles
                    populateMemberRoles(rblRole)

                    'Show/Hide role section
                    phRoleInThisCampaign.Visible = Not showTeamAdminOnly
                    lbtnSaveMember.Visible = Not showTeamAdminOnly

                    'Instantiate variables
                    blMembers = New blMembers
                    Dim thisNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(thisNodeId)
                    Dim parentNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(parentNodeId)
                    Dim businessReturn As BusinessReturn = blMembers.getMemberDemographics_byId(thisNodeId, True, True, True, True)

                    If businessReturn.DataContainer.Count > 0 Then
                        Dim member As Member = businessReturn.DataContainer(0)

                        'Obtain data to display
                        'lblEmail.Text = member.MembershipProperties.email
                        hlnkEmail.Text = member.MembershipProperties.email
                        hlnkEmail.NavigateUrl = "mailto:" & member.MembershipProperties.email
                        lblFirstName.Text = member.Demographics.firstName
                        lblLastName.Text = member.Demographics.lastName

                        'Set role & description
                        Select Case parentNode.DocumentTypeAlias
                            Case docTypes.Team
                                rblRole.SelectedValue = memberRole.TeamAdministrator
                            Case docTypes.campaignMember
                                If parentNode.GetPropertyValue(Of Boolean)(nodeProperties.campaignManager) Then
                                    rblRole.SelectedValue = memberRole.CampaignAdministrator
                                Else
                                    rblRole.SelectedValue = memberRole.CampaignMember
                                End If
                        End Select

                        'Obtain image's src.
                        If Not String.IsNullOrWhiteSpace(member.Demographics.photoUrl) Then
                            imgTeamMember.ImageUrl = member.Demographics.photoUrl
                            imgTeamMember.AlternateText = member.Demographics.firstName & " " & member.Demographics.lastName
                            imgTeamMember.Attributes.Add("title", member.Demographics.firstName & " " & member.Demographics.lastName)
                            imgTeamMember.Visible = True
                        Else
                            imgTeamMember.Visible = False
                        End If

                    End If

                End If

                'Adjust elements depending on current user's role.
                displayElementsByRole()
            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\TeamMemberEntry.ascx.vb : UserControls_TeamMemberEntry_PreRender()")
            sb.AppendLine("thisNodeId: " & thisNodeId)
            sb.AppendLine("parentNodeId: " & parentNodeId)
            sb.AppendLine("currentUserId: " & currentUserId)
            sb.AppendLine("currentMembersRole: " & currentMembersRole)
            sb.AppendLine("thisMembersRole: " & thisMembersRole)
            sb.AppendLine("showTeamAdminOnly: " & showTeamAdminOnly)


            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Response.Write("<br />Error: " & ex.ToString & "<br />")
        End Try
    End Sub

    Private Sub btnCreateNewMember_Click(sender As Object, e As EventArgs) Handles btnCreateNewMember.Click
        Try
            'Set the tab to open
            setTabCookie(tabNames.teamMembers)

            'Send email
            Dim blEmails As New blEmails
            If blEmails.sendEmail_ConfirmMembership(txbNewEmailAddress.Text.Trim,
                                                    rblRole_NewMember.SelectedValue,
                                                    ConfigurationManager.AppSettings(Miscellaneous.siteUrl),
                                                    Request.QueryString("nodeId")) Then

                'Save session that a new member has been added successfully
                Session(Sessions.newTeamMemberAdded) = True
            Else
                'Error submitting 
                Session(Sessions.newTeamMemberAdded) = False
            End If

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\TeamMemberEntry.ascx.vb : btnCreateNewMember_Click()")


            ''returnObject.ExceptionMessage = ex.ToString()
            'Response.Write("Error: " & ex.ToString)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            ''Error submitting 
            'Response.Write("<br />Error: new member submission unsuccessfull.")
            Session(Sessions.newTeamMemberAdded) = False
            Session(Sessions.errorMsg) = ex.ToString
        End Try
    End Sub
    Private Sub lbtnSaveMember_Click(sender As Object, e As EventArgs) Handles lbtnSaveMember.Click
        'Instantiate variables
        Try
            blMembers = New blMembers
            blTeams = New blTeams
            Dim result As BusinessReturn = New BusinessReturn
            Dim campaignId As Integer = CInt(Request.QueryString("nodeId"))

            'Set the tab to open
            setTabCookie(tabNames.teamMembers)

            'Determine the logged-in member's role
            If hfldCurrentMembersRole.Value = memberRole.TeamAdministrator Then
                'Determine if user is editting thier own account
                If hfldNodeId.Value = blMembers.GetCurrentMemberId() Then
                    If hfldThisMembersRole.Value = rblRole.SelectedValue Then
                        'Allow only the editting of the member's role description.
                        result = blTeams.UpdateTeamMember(hfldNodeId.Value, campaignId, rblRole.SelectedValue)
                    Else
                        'Show a warning that a member cannot change their own role.  Member must request an updated via another team administrator
                        Session(Sessions.warningMsg) = "Members are not permitted to update their own role within a campaign.  To have your role updated, please request the change via another team administrator."

                    End If

                Else 'User Is editing someone else' acct.

                    If hfldThisMembersRole.Value = rblRole.SelectedValue Then
                        'No role change. Only description change
                        result = blTeams.UpdateTeamMember(hfldNodeId.Value, campaignId, rblRole.SelectedValue)

                    ElseIf (rblRole.SelectedValue = memberRole.TeamAdministrator) Or (hfldThisMembersRole.Value = memberRole.TeamAdministrator) Then
                        If (rblRole.SelectedValue = memberRole.TeamAdministrator) Then
                            'Upgrade to Team Admin
                            result = blTeams.AddTeamAdministratorToCampaign_byMemberId(hfldNodeId.Value, campaignId)
                        Else
                            'Remove as Team Admin
                            result = blTeams.AddCampaignMember_byMemberId(hfldNodeId.Value, campaignId, rblRole.SelectedValue)
                        End If

                    Else

                        'Update member
                        result = blTeams.UpdateTeamMember(hfldNodeId.Value, campaignId, rblRole.SelectedValue)

                    End If
                End If






            Else '(hfldCurrentMembersRole = 'Campaign Administrators')	

                If hfldNodeId.Value = blMembers.GetCurrentMemberId() Then '[user Is editing their own acct]
                    '			If hfldThisMembersRole = rblRole Then [only description change]	
                    '				Save role description
                    '            Else
                    '               Warning:        cannot change your own role.  Request update to your role via another member

                    If hfldThisMembersRole.Value = rblRole.SelectedValue Then                    'Response.Write("<br />2A1")
                        'Allow only the editting of the member's role description.
                        result = blTeams.UpdateTeamMember(hfldNodeId.Value, campaignId, rblRole.SelectedValue)

                    Else
                        'Show a warning that a member cannot change their own role.  Member must request an updated via another team administrator
                        Session(Sessions.warningMsg) = "Members are not permitted to update their own role within a campaign.  To have your role updated, please request the change via another team administrator."

                    End If


                Else '[user Is editing someone else' acct.]		
                    '			If hfldThisMembersRole = rblRole Then [only description change]	
                    '				Save role description
                    '            Else
                    '				set campaignManager boolean
                    result = blTeams.UpdateTeamMember(hfldNodeId.Value, campaignId, rblRole.SelectedValue)


                End If
            End If

            If IsNothing(Session(Sessions.warningMsg)) Then

                If result.isValid Then
                    Session(Sessions.successMsg) = "Member has been updated successfully"
                Else
                    Session(Sessions.errorMsg) = "We apologize, but we were unable to complete the requested task.  Error: " & result.ExceptionMessage
                End If

            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\UserControls_TeamMemberEntry.ascx.vb : lbtnSaveMember_Click()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try

    End Sub
    Private Sub btnRemoveMember_Click(sender As Object, e As EventArgs) Handles btnRemoveMember.Click
        'Instantiate variables
        Try
            blMembers = New blMembers

            'Set the tab to open
            setTabCookie(tabNames.teamMembers)

            'Attempt to submit update
            If hfldNodeId.Value = blMembers.GetCurrentMemberId() Then
                'Warning: Member cannot update their own role within a campaign.
                Session(Sessions.warningMsg) = "Members are not permitted to update their own role within a campaign.  To have your role updated, please request the change via another team administrator."
            Else
                'Instantiate variables
                Dim BusinessReturn As BusinessReturn = New BusinessReturn
                blTeams = New blTeams

                If showTeamAdminOnly Then
                    'Submit update request for team page
                    BusinessReturn = blTeams.RemoveFromTeam_byMemberId(hfldNodeId.Value, UmbracoContext.Current.PageId, hfldThisMembersRole.Value)
                Else
                    'Submit update request for campaign page.
                    Dim campaignId As Integer = CInt(Request.QueryString("nodeId"))
                    BusinessReturn = blTeams.RemoveFromCampaign_byMemberId(hfldNodeId.Value, campaignId, hfldThisMembersRole.Value)
                End If

                'Display result message.
                If BusinessReturn.isValid Then
                    Session(Sessions.successMsg) = "Member has been removed successfully"
                Else
                    Session(Sessions.errorMsg) = "We apologize, but we were unable to remove the member from this campaign.  Error: " & BusinessReturn.ExceptionMessage
                End If
            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\UserControls_TeamMemberEntry.ascx.vb : btnRemoveMember_Click()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
    End Sub
#End Region

#Region "Methods"
    Private Sub populateMemberRoles(ByRef rbl As RadioButtonList)
        'Instantiate variables
        Dim lstItem As ListItem = New ListItem

        'Ensure all former items are removed before proceeding.
        rbl.Items.Clear()

        'Create list item and add to radio button list
        lstItem.Text = memberRole.TeamAdministrator
        lstItem.Value = memberRole.TeamAdministrator
        rbl.Items.Add(lstItem)

        lstItem = New ListItem With {
            .Text = memberRole.CampaignAdministrator,
            .Value = memberRole.CampaignAdministrator
        }
        rbl.Items.Add(lstItem)

        lstItem = New ListItem With {
            .Text = memberRole.CampaignMember,
            .Value = memberRole.CampaignMember,
            .Selected = True
        }
        rbl.Items.Add(lstItem)
    End Sub
    Private Sub displayElementsByRole()
        'lbl.Text = currentMembersRole
        Try
            'adjust controls depending on member role and panel type.
            If thisNodeId = -1 Then
                'New member panel
                Select Case currentMembersRole
                    Case memberRole.TeamAdministrator
                    'Show everyting
                    Case memberRole.CampaignAdministrator
                        'disable 'team member' from rbl
                        For Each item As ListItem In rblRole_NewMember.Items
                            If item.Value = memberRole.TeamAdministrator Then item.Enabled = False
                        Next

                    Case memberRole.CampaignMember
                        'hide Team Members save & remove buttons
                        mvTeamMemberEntry.Visible = False
                End Select
            Else
                'Existing member panel
                Select Case currentMembersRole
                    Case memberRole.TeamAdministrator
                    'Show everyting
                    Case memberRole.CampaignAdministrator
                        If thisMembersRole = memberRole.TeamAdministrator Then
                            'If edit Then panel = 'team admin'
                            '   hide Team Members save & remove buttons
                            pnlSubmitButtons.Visible = False
                            '   disable Roles
                            rblRole.Enabled = False

                        Else
                            'disable 'team member' from rbl
                            For Each item As ListItem In rblRole.Items
                                If item.Value = memberRole.TeamAdministrator Then item.Enabled = False
                            Next
                        End If

                    Case memberRole.CampaignMember
                        'hide Team Members save & remove buttons
                        pnlSubmitButtons.Visible = False
                        'disable Roles
                        rblRole.Enabled = False
                End Select
            End If

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\UserControls_TeamMemberEntry.ascx.vb : displayElementsByRole()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
    End Sub
#End Region
End Class

