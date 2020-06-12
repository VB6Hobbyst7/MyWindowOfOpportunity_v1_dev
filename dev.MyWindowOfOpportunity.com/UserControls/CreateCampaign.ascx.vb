'Imports System.Xml.XPath
Imports umbraco
Imports Common
Imports System.Data
Imports umbraco.Core.Models
Imports umbraco.Web
Imports System.Threading

Partial Class UserControls_CreateCampaign
    Inherits System.Web.UI.UserControl

#Region "Properties"
    Private _uHelper As Uhelper = New Uhelper()
    Private userId As Integer?
    Private blMembers As blMembers
    Private blMedia As blMedia
    Private blCampaigns As blCampaigns
    Private blTeams As blTeams
    Private dtAssociatedNodes As DataTable
#End Region

#Region "Handles"
    Private Sub UserControls_CreateCampaign_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            'Instantiate variables
            Dim loginStatus As Models.LoginStatusModel
            blMembers = New blMembers

            'Is current user logged in?
            loginStatus = blMembers.getCurrentLoginStatus()
            If loginStatus.IsLoggedIn Then
                'build list of campaigns/teams that user is associated with.
                buildCampaignList()

                'Is user logged in using social media?
                If blMembers.isUserLoggedIn_bySocialMedia() Then
                    'Display alt email panel for socail users
                    phAltEmail.Visible = True

                    'Obtain alt email if it exists
                    txbAltEmail.Text = blMembers.getCurrentUsersAltEmail()
                End If
            Else
                Response.Redirect(_uHelper.Get_IPublishedContentByID(siteNodes.Login).Url, False)
                HttpContext.Current.Response.Flush() 'Sends all currently buffered output To the client.
                HttpContext.Current.Response.SuppressContent = True 'Gets Or sets a value indicating whether To send HTTP content To the client.
                HttpContext.Current.ApplicationInstance.CompleteRequest() 'Causes ASP.NET To bypass all events And filtering In the HTTP pipeline chain Of execution And directly execute the EndRequest Event.
                HttpContext.Current.Response.End()
            End If

        Catch abortEx As ThreadAbortException
            'Do NOTHING!!!
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\CreateCampaign.ascx.vb : UserControls_CreateCampaign_Load()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
    End Sub
    Private Sub lbtnCreateCampaign_Click(sender As Object, e As EventArgs) Handles lbtnCreateCampaign.Click
        'Instantiate variables
        Dim loginStatus As Models.LoginStatusModel
        blMembers = New blMembers
        blMedia = New blMedia
        blTeams = New blTeams
        blCampaigns = New blCampaigns
        Dim newTeamName As String
        Dim firstChar As Char
        Dim results As BusinessReturn = New BusinessReturn

        Try
            'Empty alert field
            hfldShowAlert.Value = String.Empty

            'Is current user logged in?
            loginStatus = blMembers.getCurrentLoginStatus()

            If loginStatus.IsLoggedIn Then
                'Instantiate variables
                Dim teamId As Integer = -1

                'validate campaign name and alt email
                If phAltEmail.Visible Then
                    results = blCampaigns.Validate(results, ucTxbCampaignName.Text.Trim, txbAltEmail.Text.Trim)
                Else
                    results = blCampaigns.Validate(results, ucTxbCampaignName.Text.Trim)
                End If

                '
                If Not results.isValid Then
                    AlertMsg.AlertMsg = results.ExceptionMessage.ToString
                    hfldShowAlert.Value = True
                Else
                    If rbCreateNew.Checked Then
                        'Validate the new team name
                        results = blTeams.Validate(results, txbNewTeamName.Text.Trim)
                        If Not results.isValid Then
                            AlertMsg.AlertMsg = results.ExceptionMessage.ToString
                            hfldShowAlert.Value = True
                        End If
                    End If
                End If

                'If page values are valid, submit information.
                If results.isValid Then
                    'Create new team name and get ID
                    If rbCreateNew.Checked Then
                        'Get values from client-side
                        newTeamName = txbNewTeamName.Text.Trim
                        firstChar = newTeamName.Substring(0, 1)

                        'Get alpha folder id
                        Dim alphaFolderId As Integer = blTeams.getRootFolder_byName(firstChar)

                        'Create team and get id
                        teamId = blTeams.CreateTeam(alphaFolderId, newTeamName, blMembers.GetCurrentMemberId)

                    Else 'Get ID of checked team name
                        For Each myItem As ListViewDataItem In lstviewTeamAccts.Items
                            For Each myControl As Control In myItem.Controls
                                Try
                                    If (TypeOf myControl Is RadioButton) Then
                                        Dim rdoTemp As RadioButton = DirectCast(myControl, RadioButton)
                                        If rdoTemp.Checked Then
                                            teamId = rdoTemp.Attributes("value")
                                            Exit For
                                        End If
                                    End If
                                Catch ex As Exception
                                    Dim sb As New StringBuilder()
                                    sb.AppendLine("\UserControls\CreateCampaign.ascx.vb : lbtnCreateCampaign_Click()")
                                    saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
                                End Try
                            Next
                        Next
                    End If

                    'create new campaign under team
                    Dim campaignId As Integer = blCampaigns.CreateCampaign(teamId, ucTxbCampaignName.Text.Trim)

                    'Create Media folder for campaign
                    results = blMedia.createCampaignsMediaFolder(campaignId)

                    'Save alt email
                    Dim br As BusinessReturn = blMembers.UpdateAltEmail(blMembers.GetCurrentMemberId, txbAltEmail.Text.Trim)

                    'navigate to campagain edit with new campaign id as querystring.
                    Response.Redirect(_uHelper.Get_IPublishedContentByID(siteNodes.EditCampaign).Url & "?" & queryParameters.nodeId & "=" & campaignId & "&" & queryParameters.stepByStep & "=true", False)
                    HttpContext.Current.ApplicationInstance.CompleteRequest()
                Else
                    'Display error messages
                    If results.ValidationMessages.Count > 0 Then
                        Dim sbErrors As New StringBuilder
                        For Each validationMsg As ValidationContainer In results.ValidationMessages
                            sbErrors.AppendLine(validationMsg.ErrorMessage)
                        Next
                        AlertMsg.AlertMsg = sbErrors.ToString
                        hfldShowAlert.Value = True

                        'ErrorMsgs.Visible = True
                        'ErrorMsgs.InnerText = sbErrors.ToString
                    End If

                End If

            Else
                'Save current page and go to login page
                Session(Sessions.previousPage) = HttpContext.Current.Request.Url.AbsoluteUri
                Response.Redirect(_uHelper.Get_IPublishedContentByID(siteNodes.Login).Url, False)
                HttpContext.Current.ApplicationInstance.CompleteRequest()

                'Dim sb As New StringBuilder()
                'sb.AppendLine("CreateCampaign.ascx.vb : lbtnCreateCampaign_Click() Error")
                'sb.AppendLine("Result: " & results.isValid.ToString)
                'sb.AppendLine("Ex Msg: " & results.ExceptionMessage)
                'sb.AppendLine("List of Errors: " & Newtonsoft.Json.JsonConvert.SerializeObject(results.ValidationMessages))
                'saveErrorMessage(getLoggedInMember, sb.ToString, sb.ToString())
            End If


        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\CreateCampaign.ascx.vb : lbtnCreateCampaign_Click()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
    End Sub
#End Region

#Region "Methods"
    Private Sub buildCampaignList()
        'Obtain current user id
        Try
            userId = blMembers.GetCurrentMemberId

            'Obtain list of campaigns that user has access to.
            createTableStructure()
            userMemberOfCampaign(_uHelper.Get_IPublishedContentByID(siteNodes.Campaigns))

            'Build the current campaign list the user has access to.
            buildCurrentCampaignsList()

            'Create list of all teams that the user is an administrator with.
            CreateTeamList()
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\CreateCampaign.ascx.vb : buildCampaignList()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
    End Sub
    Private Sub createTableStructure()
        Try 'Instantiate new datatable
            dtAssociatedNodes = New DataTable
            'Add collumns to datatable
            dtAssociatedNodes.Columns.Add(Miscellaneous.nodeId, GetType(Integer))
            'dtAssociatedNodes.Columns.Add(Miscellaneous.parentId, GetType(Integer))
            'dtAssociatedNodes.Columns.Add(docTypes.CorporateEntity, GetType(Boolean)).DefaultValue = False
            dtAssociatedNodes.Columns.Add(docTypes.Team, GetType(Boolean)).DefaultValue = False
            dtAssociatedNodes.Columns.Add(docTypes.Campaign, GetType(Boolean)).DefaultValue = False
            dtAssociatedNodes.Columns.Add(nodeProperties.Name, GetType(String))
            dtAssociatedNodes.AcceptChanges()
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\CreateCampaign.ascx.vb : createTableStructure()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
    End Sub
    Private Sub userMemberOfCampaign(ByVal thisNode As IPublishedContent)
        'Is member associated to IPublishedContent
        Try
            Select Case thisNode.DocumentTypeAlias
                Case docTypes.Team

                    Dim members As String() = thisNode.GetPropertyValue(Of String)(nodeProperties.teamAdministrators).Split(",")
                    If members.Contains(userId.ToString) Then
                        addToDt(thisNode.Id, docTypes.Team)
                        'isMemberOfParent = True
                    End If
                    'End If

                Case docTypes.campaignMember
                    'Determine if user is a member of campaign
                    If thisNode.HasProperty(nodeProperties.campaignMember) Then
                        If thisNode.GetPropertyValue(Of Integer)(nodeProperties.campaignMember) = userId Then
                            addToDt(thisNode.Parent.Parent.Id, docTypes.Campaign)
                        End If
                    End If

            End Select
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\CreateCampaign.ascx.vb : userMemberOfCampaign()")
            sb.AppendLine("thisNode:" & thisNode.ToString())

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Response.Write(ex.ToString & "<br />")
        End Try

        For Each childNode As IPublishedContent In thisNode.Children
            userMemberOfCampaign(childNode)
        Next
    End Sub
    Private Sub addToDt(ByVal thisNodeId As Integer, ByVal doctype As String, Optional ByVal parentNodeId As Integer = -1)
        Try
            Dim dr As DataRow = dtAssociatedNodes.NewRow
            dr(Miscellaneous.nodeId) = thisNodeId
            dr(doctype) = True
            dr(nodeProperties.Name) = _uHelper.Get_IPublishedContentByID(thisNodeId).Name
            'If parentNodeId <> -1 Then dr(Miscellaneous.parentId) = parentNodeId
            dtAssociatedNodes.Rows.Add(dr)
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\CreateCampaign.ascx.vb : addToDt()")
            sb.AppendLine("thisNodeId" & thisNodeId)
            sb.AppendLine("doctype" & doctype)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
    End Sub
    Private Sub buildCurrentCampaignsList()
        'Loop thru each row and build list of current campaigns panel
        phTeamAccts.Controls.Clear()
        phMemberAccts.Controls.Clear()

        Try
            For Each dr As DataRow In dtAssociatedNodes.Rows
                Select Case True
                    Case dr(docTypes.Team)
                        pnlTeamAccts.Visible = False
                        Dim uc As ASP.usercontrols_ulteamaccts_ascx = New ASP.usercontrols_ulteamaccts_ascx With {
                            .nodeId = dr(Miscellaneous.nodeId)
                        }
                        phTeamAccts.Controls.Add(uc)

                    Case dr(docTypes.Campaign)
                        pnlMemberAccts.Visible = False
                        Dim uc As ASP.usercontrols_ulmemberaccts_ascx = New ASP.usercontrols_ulmemberaccts_ascx With {
                            .nodeId = dr(Miscellaneous.nodeId)
                        }
                        phMemberAccts.Controls.Add(uc)

                End Select
            Next

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\CreateCampaign.ascx.vb : buildCurrentCampaignsList()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
    End Sub
    Private Sub UserControls_CreateCampaign_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        'rbCreateNew.Attributes.Add("name", "test")
    End Sub
    Private Sub UserControls_CreateCampaign_DataBinding(sender As Object, e As EventArgs) Handles Me.DataBinding
        'rbCreateNew.Attributes.Add("name", "test")
    End Sub
    Private Sub UserControls_CreateCampaign_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
        'rbCreateNew.Attributes.Add("name", "test")
    End Sub
    Private Sub CreateTeamList()
        Try  'Instantiate variables
            Dim dtTeamList As DataTable = dtAssociatedNodes.Clone

            'Create list of all team names/IDs
            For Each dr As DataRow In dtAssociatedNodes.Rows
                If dr(docTypes.Team) Then
                    'teamList.Add(dr(nodeProperties.Name))
                    Dim newDr As DataRow = dtTeamList.NewRow
                    newDr(Miscellaneous.nodeId) = dr(Miscellaneous.nodeId)
                    newDr(nodeProperties.Name) = dr(nodeProperties.Name)
                    dtTeamList.Rows.Add(newDr)
                End If
            Next

            'Add list to listview
            lstviewTeamAccts.DataSource = dtTeamList
            lstviewTeamAccts.DataBind()
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\CreateCampaign.ascx.vb : CreateTeamList()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
    End Sub
#End Region

End Class