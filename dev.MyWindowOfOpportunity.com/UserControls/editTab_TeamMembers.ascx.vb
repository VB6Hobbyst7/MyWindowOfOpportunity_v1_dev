Imports umbraco
Imports Common
Imports System.Data
Imports umbraco.Core.Models
Imports umbraco.Web

Partial Class UserControls_editTab_TeamMembers
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
    Public Property campaignComplete As Boolean = False
#End Region

#Region "Handles"
    Private Sub UserControls_editTab_TeamMembers_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        If Not String.IsNullOrEmpty(thisNodeId) Then
            Try
                'Instantiate variables
                Dim thisNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(CInt(thisNodeId))
                blMembers = New blMembers
                _currentMembersRole = blMembers.GetCurrentMemberRole_byCampaignId(thisNode.Id)

                Dim dtTeamAdmins As DataTable = createDatatable()
                Dim dtCampaignAdmins As DataTable = createDatatable()
                Dim dtMembers As DataTable = createDatatable()

                'Add data to tables
                dtTeamAdmins = getTeamAdministrators(dtTeamAdmins)
                dtCampaignAdmins = getCampaignMembers(dtCampaignAdmins, True)
                dtMembers = getCampaignMembers(dtMembers)

                ''TEMP
                'gv1.DataSource = dtTeamAdmins
                'gv2.DataSource = dtCampaignAdmins
                'gv3.DataSource = dtMembers
                'gv1.DataBind()
                'gv2.DataBind()
                'gv3.DataBind()


                'Create button lists
                lvTeamMemberBtns_TeamAdmins.DataSource = dtTeamAdmins
                lvTeamMemberBtns_TeamAdmins.DataBind()
                lvTeamMemberBtns_CampaignAdmins.DataSource = dtCampaignAdmins
                lvTeamMemberBtns_CampaignAdmins.DataBind()
                lvTeamMemberBtns_Members.DataSource = dtMembers
                lvTeamMemberBtns_Members.DataBind()

                'Create entries
                lviewTeamAdminEntries.DataSource = dtTeamAdmins
                lviewTeamAdminEntries.DataBind()
                lviewCampaignAdminEntries.DataSource = dtCampaignAdmins
                lviewCampaignAdminEntries.DataBind()
                lviewTeamMemberEntries.DataSource = dtMembers
                lviewTeamMemberEntries.DataBind()

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
                sb.AppendLine("\UserControls\editTab_TeamMembers.ascx.vb : UserControls_editTab_TeamMembers_PreRender()")
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
        'dt.Columns.Add(dtColumns.Role, GetType(String))
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

            If thisNode.Parent.HasProperty(nodeProperties.teamAdministrators) AndAlso thisNode.Parent.HasValue(nodeProperties.teamAdministrators) Then
                Dim lstTeamAdministrators As List(Of Integer) = thisNode.Parent.GetPropertyValue(Of String)(nodeProperties.teamAdministrators).Split(",").ToList.ConvertAll(Function(str) Int32.Parse(str))

                'Obtain data for each admin id
                For Each teamAdminId As Integer In lstTeamAdministrators
                    Dim dr As DataRow = dt.NewRow
                    dr(dtColumns.Id) = teamAdminId
                    dr(dtColumns.Name) = blMembers.getMemberName_byId(getIdFromGuid_byType(teamAdminId, UmbracoObjectTypes.Member))
                    'dr(dtColumns.Role) = memberRole.TeamAdministrator
                    dr(dtColumns.ParentNodeId) = thisNode.Parent.Id
                    dr(dtColumns.currentMembersRole) = _currentMembersRole
                    dr(dtColumns.thisMembersRole) = blMembers.GetCampaignMembersRole_byMemberId(teamAdminId, thisNode.Id)
                    dt.Rows.Add(dr)
                Next

            End If

            Return dt
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("editTab_TeamMembers.ascx.vb : getTeamAdministrators()")
            sb.AppendLine("dt:" & dt.ToString())

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return dt
        End Try
    End Function
    Private Function getCampaignMembers(ByVal dt As DataTable, Optional ByVal isAdministrator As Boolean = False) As DataTable
        Try
            Dim campaignMemberNode As IPublishedContent = GetCampaignMemberFolder()

            If campaignMemberNode.DocumentTypeAlias <> docTypes.editCampaign Then
                'Loop thru each member IPublishedContent
                For Each childNode As IPublishedContent In campaignMemberNode.Children
                    If childNode.GetPropertyValue(Of Boolean)(nodeProperties.campaignManager) = isAdministrator Then
                        If childNode.HasProperty(nodeProperties.campaignMember) AndAlso childNode.GetPropertyValue(Of String)(nodeProperties.campaignMember) > 0 Then
                            Dim dr As DataRow = dt.NewRow
                            dr(dtColumns.Id) = childNode.GetPropertyValue(Of String)(nodeProperties.campaignMember)
                            dr(dtColumns.Name) = blMembers.getMemberName_byId(childNode.GetPropertyValue(Of String)(nodeProperties.campaignMember))
                            dr(dtColumns.ParentNodeId) = childNode.Id
                            dr(dtColumns.currentMembersRole) = _currentMembersRole
                            dr(dtColumns.thisMembersRole) = blMembers.GetCampaignMembersRole_byMemberId(childNode.GetPropertyValue(Of String)(nodeProperties.campaignMember), thisNodeId)
                            dt.Rows.Add(dr)
                        End If
                    End If
                Next
            End If

            Return dt
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\editTab_TeamMembers.ascx.vb : getCampaignMembers()")
            sb.AppendLine("dt:" & dt.ToString())
            sb.AppendLine("isAdministrator:" & isAdministrator)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return dt
        End Try
    End Function
    Private Function GetCampaignMemberFolder() As IPublishedContent

        Dim campaignMemberNode As IPublishedContent

        Try 'Instantiate variables
            Dim thisNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(CInt(thisNodeId))

            'Loop thru child nodes, obtain campaignMember folder.
            For Each childNode As IPublishedContent In thisNode.Children
                If childNode.DocumentTypeAlias = docTypes.campaignMembers Then
                    campaignMemberNode = childNode
                    Exit For
                End If
            Next

            If IsNothing(campaignMemberNode) Then
                'Create folder
                Dim blMembers As New blMembers
                Dim folderId As Integer = blMembers.CreateCampaignMembersFolder(thisNode.Id)
                campaignMemberNode = _uHelper.Get_IPublishedContentByID(folderId)
            End If

            Return campaignMemberNode
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("editTab_TeamMembers.ascx.vb : GetCampaignMemberFolder()")
            sb.AppendLine("campaignMemberNode:" & JsonHelper.ToJson(campaignMemberNode))

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return Nothing
        End Try
    End Function
#End Region

End Class