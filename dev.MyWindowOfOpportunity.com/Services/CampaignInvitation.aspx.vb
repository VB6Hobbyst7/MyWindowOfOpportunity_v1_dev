Imports Common
Imports umbraco
Imports umbraco.NodeFactory




Partial Class Handlers_CampaignInvitation
    Inherits System.Web.UI.Page


#Region "Properties"
    'Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
    '    Get
    '        Return False
    '    End Get
    'End Property

    Private campaignInvitation As CampaignInvitation
    Private teamInvitation As TeamInvitation
    Private BusinessReturn As BusinessReturn

    Private blMembers As blMembers
    Private blTeams As blTeams
    Private params As String

    'Private campaignInvitation As CampaignInvitation
    'Private BusinessReturn As BusinessReturn

    'Private blMembers As blMembers
    'Private blTeams As blTeams
#End Region

#Region "Handles"
    Private Sub Handlers_CampaignInvitation_Load(sender As Object, e As EventArgs) Handles Me.Load

        Try

            'Instantiate variables
            BusinessReturn = New BusinessReturn()
            blMembers = New blMembers
            blTeams = New blTeams

            'Verify if we need to create the account first.
            If Not String.IsNullOrEmpty(Context.Request.QueryString(queryParameters.createAcct)) Then
                If Context.Request.QueryString(queryParameters.createAcct) = True.ToString Then
                    'Ensure that the new acct info is provided
                    If Not String.IsNullOrEmpty(Context.Request.QueryString(queryParameters.acctData)) Then
                        'Try to create account
                        BusinessReturn = createAccount(Context)
                    End If
                End If
            End If

            'Will only be invalid if a new account failed to be created.
            If BusinessReturn.isValid Then
                'Determine if data is a campaign or team member invitation.
                If Not String.IsNullOrEmpty(Context.Request.QueryString(queryParameters.params_campaignInvitation)) Then
                    'Import json data from querystring into class
                    campaignInvitation = New CampaignInvitation
                    params = Context.Request.QueryString(queryParameters.params_campaignInvitation)
                    Newtonsoft.Json.JsonConvert.PopulateObject(params, campaignInvitation)

                    Response.Write("params_campaignInvitation exists<br />")

                    'Determine if email validation has expired.
                    If DateTime.Now <= campaignInvitation.dateSubmitted.AddDays(2) Then
                        'Email is valid.  Continue.
                        '==========================================
                        'Determine if user has been created already
                        If blMembers.doesMemberExist_byEmail(campaignInvitation.email) Then
                            Response.Write("member exists<br />")
                            'Member exists
                            '           Add member to campaign's team with role/description:
                            '               If Team Admin:
                            If campaignInvitation.roleValue = memberRole.TeamAdministrator Then
                                '                   Add member id to team node if it does not exist
                                '                   Remove member from all child campaigns/team member folders if exists
                                BusinessReturn = blTeams.AddTeamAdministratorToCampaign_byMemberId(blMembers.getMemberId_byEmail(campaignInvitation.email), campaignInvitation.campaignId)
                                Response.Write("roleValue = memberRole.TeamAdministrator<br />")

                            Else
                                '                   Add member to campaign/team member folder if it does not exist
                                '                   Add role/description
                                '                   Remove member from campaign's team if exists
                                BusinessReturn = blTeams.AddCampaignMember_byMemberId(blMembers.getMemberId_byEmail(
                                                                                      campaignInvitation.email), campaignInvitation.campaignId,
                                                                                      campaignInvitation.roleDescription, campaignInvitation.roleValue)
                                Response.Write("roleValue <> memberRole.TeamAdministrator<br />")
                                Dim sb As New StringBuilder
                                sb.AppendLine(campaignInvitation.email & "<br />")
                                sb.AppendLine(campaignInvitation.campaignId & "<br />")
                                sb.AppendLine(campaignInvitation.roleDescription & "<br />")
                                sb.AppendLine(campaignInvitation.roleValue & "<br />")
                                Response.Write(sb.ToString & "<br />")

                            End If

                            Response.Write("isValid: " & BusinessReturn.isValid & "<br>")
                            Response.Write("ExceptionMessage: " & BusinessReturn.ExceptionMessage & "<br>")
                            ''Navigate to campaign edit page and show success alert msg
                            'Context.Response.Redirect(New Node(siteNodes.EditCampaign).NiceUrl & "?" & queryParameters.tab & "=" & tabNames.teamMembers &
                            '                  "&" & queryParameters.newMember & "=" & True.ToString & "&" & queryParameters.nodeId & "=" & campaignInvitation.campaignId)

                        Else
                            Response.Write("member does not exists<br />")

                            ''Member does not exist.  Create it.
                            ''           navigate to 'become-a-member' [pass data via querystring for further processing.]
                            'Dim redirectUrl As String = New Node(siteNodes.BecomeAMember).NiceUrl & "?" & queryParameters.params_campaignInvitation & "=" & params
                            'Context.Response.Redirect(redirectUrl)
                        End If
                    Else
                        ''Email is no longer active.  Navigate to message page
                        'Context.Response.Redirect(New Node(siteNodes.ExpiredInvitation).NiceUrl, True)

                        Response.Write("email inactive.  to old<br />")
                    End If












                ElseIf Not String.IsNullOrEmpty(Context.Request.QueryString(queryParameters.params_teamInvitation)) Then
                    'Import json data from querystring into class
                    teamInvitation = New TeamInvitation
                    params = Context.Request.QueryString(queryParameters.params_teamInvitation)
                    Newtonsoft.Json.JsonConvert.PopulateObject(params, teamInvitation)

                    Response.Write("params_teamInvitation exists<br />")

                    'Determine if email validation has expired.
                    If DateTime.Now <= teamInvitation.dateSubmitted.AddDays(2) Then
                        'Email is valid.  Continue.
                        '==========================================
                        'Determine if user has been created already

                        Response.Write("email valid<br />")


                        If blMembers.doesMemberExist_byEmail(teamInvitation.email) Then
                            'Member exists
                            '           Add member to campaign's team with role/description:
                            '               If Team Admin:
                            '                   Add member id to team node if it does not exist
                            BusinessReturn = blTeams.AddTeamAdministratorToTeam_byMemberId(blMembers.getMemberId_byEmail(teamInvitation.email), teamInvitation.teamId)


                            'Navigate to team edit page and show success alert msg
                            Context.Response.Redirect(New Node(teamInvitation.teamId).NiceUrl & "?" & queryParameters.tab & "=" & tabNames.teamMembers &
                                          "&" & queryParameters.newMember & "=" & True.ToString & "&" & queryParameters.nodeId & "=" & teamInvitation.teamId, False)
                            HttpContext.Current.ApplicationInstance.CompleteRequest()

                            'context.Response.Write("Error? " & BusinessReturn.ExceptionMessage)
                        Else
                            ''Member does not exist.  Create it.
                            ''           navigate to 'become-a-member' [pass data via querystring for further processing.]
                            Dim redirectUrl As String = New Node(siteNodes.BecomeAMember).NiceUrl & "?" & queryParameters.params_teamInvitation & "=" & params
                            Context.Response.Redirect(redirectUrl, False)
                            HttpContext.Current.ApplicationInstance.CompleteRequest()
                        End If
                    Else
                        ''Email is no longer active.  Navigate to message page
                        'Context.Response.Redirect(New Node(siteNodes.ExpiredInvitation).NiceUrl, True)

                        Response.Write("email not valid<br />")
                    End If










                End If
            Else
                ''Cannot create account
                'Context.Response.Redirect(New Node(siteNodes.CannotCreateAcct).NiceUrl, True)


                Response.Write("businessreturn invalid<br />")
            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\Handlers\CampaignInvitation.aspx.vb : Handlers_CampaignInvitation_Load()")

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Context.Response.ContentType = "text/plain"
            Context.Response.Write("<br />Error: " & ex.ToString)
        End Try


    End Sub
#End Region

#Region "Methods"
    Private Function createAccount(ByRef context As HttpContext) As BusinessReturn
        '
        blMembers = New blMembers
        Dim returnResult As BusinessReturn = New BusinessReturn

        Try
            'Instantiate variables
            Dim valueString As String = context.Request.QueryString(queryParameters.acctData)
            Dim values As String() = valueString.Split("&")
            Dim valueDictionary = New Dictionary(Of String, String)

            '
            For Each value As String In values
                Dim dictionaryEntry As String() = value.Split("=")
                valueDictionary.Add(dictionaryEntry(0).Trim, dictionaryEntry(1).Trim)
            Next

            '
            If valueDictionary.ContainsKey(queryParameters.password) AndAlso valueDictionary.ContainsKey(queryParameters.email) Then
                'Validate key values to ensure account was not already created.
                returnResult = blMembers.IsUserUnique(valueDictionary.Item(queryParameters.email), valueDictionary.Item(queryParameters.password))

                'If unique, create account
                If (returnResult.isValid) Then
                    'Create new member
                    returnResult.ReturnMessage += " | result is valid."
                    returnResult = blMembers.Insert(valueDictionary)
                    'Log member in
                    logMemberIn(valueDictionary.Item(queryParameters.email), valueDictionary.Item(queryParameters.password), returnResult.ReturnMessage)

                    Return returnResult
                Else
                    'Log member in
                    logMemberIn(valueDictionary.Item(queryParameters.email), valueDictionary.Item(queryParameters.password), blMembers.getMemberId_byEmail(valueDictionary.Item(queryParameters.email)))

                    'if email is not unique, return without an error so to continue with adding member to campaign.
                    returnResult.ReturnMessage += " | result is invalid."
                    Return New BusinessReturn
                End If
            Else
                returnResult.ExceptionMessage = "Invalid id/email values"
                Return returnResult
            End If

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\Handlers\CampaignInvitation.aspx.vb : createAccount()")
            sb.AppendLine("context:" & context.ToString())

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            returnResult.ExceptionMessage = ex.ToString()
            Return returnResult
        End Try
    End Function
    Private Sub logMemberIn(ByVal _email As String, ByVal _password As String, ByVal _memberId As Integer)
        'Log member into MWoO
        If blMembers.isMemberLoggedIn Then
            If blMembers.GetCurrentMemberId() <> _memberId Then
                blMembers.logMemberOut()
                blMembers.logMemberIn(_email, _password)
            End If
        Else
            blMembers.logMemberIn(_email, _password)
        End If
    End Sub
#End Region
End Class





'Private Function createAccount() As BusinessReturn
'    '
'    blMembers = New blMembers
'    Dim returnResult As BusinessReturn = New BusinessReturn

'    Try
'        'Instantiate variables
'        Dim valueString As String = Request.QueryString(queryParameters.acctData)
'        Dim values As String() = valueString.Split("&")
'        Dim valueDictionary = New Dictionary(Of String, String)

'        '
'        For Each value As String In values
'            Dim dictionaryEntry As String() = value.Split("=")
'            valueDictionary.Add(dictionaryEntry(0).Trim, dictionaryEntry(1).Trim)
'        Next

'        '
'        If valueDictionary.ContainsKey(queryParameters.password) AndAlso valueDictionary.ContainsKey(queryParameters.email) Then
'            'Validate key values to ensure account was not already created.
'            returnResult = blMembers.IsUserUnique(valueDictionary.Item(queryParameters.email), valueDictionary.Item(queryParameters.password))

'            'If unique, create account
'            If (returnResult.isValid) Then
'                'Create new member
'                returnResult.ReturnMessage += " | result is valid."
'                returnResult = blMembers.Insert(valueDictionary)
'                'Log member in
'                logMemberIn(valueDictionary.Item(queryParameters.email), valueDictionary.Item(queryParameters.password), returnResult.ReturnMessage)

'                Return returnResult
'            Else
'                'Log member in
'                logMemberIn(valueDictionary.Item(queryParameters.email), valueDictionary.Item(queryParameters.password), blMembers.getMemberId_byEmail(valueDictionary.Item(queryParameters.email)))

'                'if email is not unique, return without an error so to continue with adding member to campaign.
'                returnResult.ReturnMessage += " | result is invalid."
'                Return New BusinessReturn
'            End If
'        Else
'            returnResult.ExceptionMessage = "Invalid id/email values"
'            Return returnResult
'        End If

'    Catch ex As Exception
'        returnResult.ExceptionMessage = ex.ToString()
'        Return returnResult
'    End Try
'End Function
'Private Sub logMemberIn(ByVal _email As String, ByVal _password As String, ByVal _memberId As Integer)
'    'Log member into MWoO
'    If blMembers.isMemberLoggedIn Then
'        If blMembers.GetCurrentMemberId() <> _memberId Then
'            blMembers.logMemberOut()
'            blMembers.logMemberIn(_email, _password)
'        End If
'    Else
'        blMembers.logMemberIn(_email, _password)
'    End If
'End Sub