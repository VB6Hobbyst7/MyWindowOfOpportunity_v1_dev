Imports Common
Imports System.Data
Imports umbraco.Core.Models
Imports umbraco.Web

Partial Class UserControls_TeamEdit_TeamMembers
    Inherits System.Web.UI.UserControl


#Region "Properties"
    Private rewardListNode As IPublishedContent
    Private newRewardId As UInt32 = 0
    Private blMembers As blMembers
    Private _currentMembersRole As String = String.Empty
    Dim _uHelper As Uhelper = New Uhelper()

    'Private _thisNode As IPublishedContent
    'Public Property thisNode() As IPublishedContent
    '    Get
    '        Return _thisNode
    '    End Get
    '    Set(value As IPublishedContent)
    '        _thisNode = value
    '    End Set
    'End Property

    Public Property thisNodeId() As String
        Get
            Return hfldThisNodeId.Value
        End Get
        Set(value As String)
            hfldThisNodeId.Value = value.ToString
        End Set
    End Property

    Private Structure dtColumns
        Const Id As String = "Id"
        Const Name As String = "Name"
        Const Role As String = "Role"
        Const ParentNodeId As String = "parentNodeId"
        Const currentMembersRole As String = "currentMembersRole"
        Const thisMembersRole As String = "thisMembersRole"
    End Structure
#End Region

#Region "Handles"
    Private Sub UserControls_editTab_TeamMembers_Load(sender As Object, e As EventArgs) Handles Me.Load
    End Sub
    Private Sub UserControls_editTab_TeamMembers_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        If Not String.IsNullOrEmpty(thisNodeId) Then
            Try
                'If IsPostBack Then
                '    'Refresh the IPublishedContent to ensure updated data is retrieved
                '    thisNode = New IPublishedContent(thisNode.Id)
                'End If

                'Instantiate variables
                Dim thisNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(CInt(thisNodeId))
                blMembers = New blMembers
                _currentMembersRole = blMembers.GetCurrentMemberRole_byCampaignId(thisNode.Id)

                ''DELETE WHEN FINISHED- OVERRIDES MEMBER ROLE.
                '_currentMembersRole = memberRole.campaignAdministrator

                Dim dtTeamAdmins As DataTable = createDatatable()
                'Dim dtCampaignAdmins As DataTable = createDatatable()
                'Dim dtMembers As DataTable = createDatatable()

                'Add data to tables
                dtTeamAdmins = getTeamAdministrators(dtTeamAdmins)
                'dtCampaignAdmins = getCampaignMembers(dtCampaignAdmins, True)
                'dtMembers = getCampaignMembers(dtMembers)

                'Create button lists
                lvTeamMemberBtns_TeamAdmins.DataSource = dtTeamAdmins
                lvTeamMemberBtns_TeamAdmins.DataBind()
                'lvTeamMemberBtns_CampaignAdmins.DataSource = dtCampaignAdmins
                'lvTeamMemberBtns_CampaignAdmins.DataBind()
                'lvTeamMemberBtns_Members.DataSource = dtMembers
                'lvTeamMemberBtns_Members.DataBind()

                'Create entries
                lviewTeamAdminEntries.DataSource = dtTeamAdmins
                lviewTeamAdminEntries.DataBind()
                'lviewCampaignAdminEntries.DataSource = dtCampaignAdmins
                'lviewCampaignAdminEntries.DataBind()
                'lviewTeamMemberEntries.DataSource = dtMembers
                'lviewTeamMemberEntries.DataBind()

                'Assign default values to new teammember entry.
                ucTeamMemberEntry_New.currentMembersRole = _currentMembersRole
                ucTeamMemberEntry_New.thisNodeId = -1

                'If current user is only a member, hide adding new team members.
                If _currentMembersRole = memberRole.CampaignMember Then
                    ucAddNewTeamMember.Visible = False
                End If

                'If session exists from submitting new member
                If Not IsNothing(Session(Sessions.newTeamMemberAdded)) Then
                    If Session(Sessions.newTeamMemberAdded) = True Then
                        'Response.Write("<br />Sessions.newTeamMemberAdded) = True")
                        pnlAlertMsg_AddedSuccessfully.Visible = True
                    Else
                        'Response.Write("<br />Sessions.newTeamMemberAdded) = false")
                        pnlAlertMsg_ErrorSaving.Visible = True
                        If Not IsNothing(Session(Sessions.errorMsg)) Then
                            'Response.Write(Session(Sessions.errorMsg).ToString)
                            Session(Sessions.errorMsg) = Nothing
                        End If
                    End If

                    'Clear session
                    Session(Sessions.newTeamMemberAdded) = Nothing
                Else
                    'Response.Write("<br />Session(Sessions.newTeamMemberAdded) is nothing")
                    'Check if any other messages exist in session and display them
                    If Not IsNothing(Session(Sessions.errorMsg)) Then
                        'Response.Write("<br />Sessions.errorMsg")
                        Dim alert As New ASP.usercontrols_alertmsg_ascx With {
                            .MessageType = UserControls_AlertMsg.msgType.Alert,
                            .AlertMsg = Session(Sessions.errorMsg)
                        }
                        phAlertMsgs.Controls.Add(alert)
                        Session(Sessions.errorMsg) = Nothing
                    End If
                    If Not IsNothing(Session(Sessions.warningMsg)) Then
                        'Response.Write("<br />Sessions.warningMsg")
                        Dim alert As New ASP.usercontrols_alertmsg_ascx With {
                            .MessageType = UserControls_AlertMsg.msgType.Warning,
                            .AlertMsg = Session(Sessions.warningMsg)
                        }
                        phAlertMsgs.Controls.Add(alert)
                        Session(Sessions.warningMsg) = Nothing
                    End If
                    If Not IsNothing(Session(Sessions.successMsg)) Then
                        'Response.Write("<br />Sessions.successMsg")
                        Dim alert As New ASP.usercontrols_alertmsg_ascx With {
                            .MessageType = UserControls_AlertMsg.msgType.Success,
                            .AlertMsg = Session(Sessions.successMsg)
                        }
                        phAlertMsgs.Controls.Add(alert)
                        Session(Sessions.successMsg) = Nothing
                    End If
                End If

            Catch ex As Exception
                Dim sb As New StringBuilder()
                sb.AppendLine("\UserControls\TeamEdit_TeamMembers.ascx.vb : UserControls_editTab_TeamMembers_PreRender()")


                saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
                'Response.Write("<br />Error: " & ex.ToString)
            End Try
        End If

    End Sub
#End Region

#Region "Methods"
    Private Function createDatatable() As DataTable
        'Instantiate variables
        Dim dt As DataTable = New DataTable

        'Add columns to table
        dt.Columns.Add(dtColumns.Id, GetType(UInt32))
        dt.Columns.Add(dtColumns.Name, GetType(String))
        dt.Columns.Add(dtColumns.Role, GetType(String))
        dt.Columns.Add(dtColumns.ParentNodeId, GetType(UInt32))
        dt.Columns.Add(dtColumns.currentMembersRole, GetType(String))
        dt.Columns.Add(dtColumns.thisMembersRole, GetType(String))
        dt.AcceptChanges()

        'Return datatable
        Return dt
    End Function

    Private Function getTeamAdministrators(ByVal dt As DataTable) As DataTable
        Try
            'Instantiate variables
            Dim thisNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(CInt(thisNodeId))
            Dim lstTeamAdministrators As New List(Of String)


            'Obtain id list of team administrators
            If thisNode.HasValue(nodeProperties.teamAdministrators) Then
                For Each teamAdmin In thisNode.GetPropertyValue(Of String)(nodeProperties.teamAdministrators).Split(",").ToList
                    lstTeamAdministrators.Add(teamAdmin)
                Next
            End If


            'Obtain data for each admin id
            For Each teamAdminId As String In lstTeamAdministrators
                Dim dr As DataRow = dt.NewRow
                dr(dtColumns.Id) = teamAdminId
                dr(dtColumns.Name) = blMembers.getMemberName_byId(teamAdminId)
                dr(dtColumns.Role) = memberRole.TeamAdministrator
                dr(dtColumns.ParentNodeId) = thisNode.Parent.Id
                dr(dtColumns.currentMembersRole) = _currentMembersRole
                dr(dtColumns.thisMembersRole) = memberRole.TeamAdministrator 'blMembers.GetCampaignMembersRole_byMemberId(teamAdminId, thisNode.Id)
                dt.Rows.Add(dr)
            Next

            Return dt
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\TeamEdit_TeamMembers.ascx.vb : getTeamAdministrators()")
            sb.AppendLine("dt:" & dt.ToString())

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Response.Write("Error: " & ex.ToString)
            Return dt
        End Try
    End Function

#End Region

End Class

