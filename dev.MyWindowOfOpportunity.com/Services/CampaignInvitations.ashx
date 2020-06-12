<%@ WebHandler Language="VB" Class="CampaignInvitations" %>

Imports Common
Imports umbraco.Web
Imports System.Net


Public Class CampaignInvitations : Implements IHttpHandler


#Region "Properties"
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

    Private campaignInvitation As CampaignInvitation
    Private teamInvitation As TeamInvitation
    Private BusinessReturn As BusinessReturn

    Private blMembers As blMembers
    Private blTeams As blTeams
    Private params As String
    Dim _uHelper As Uhelper = New Uhelper()
#End Region

#Region "Handles"
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Try

            'Instantiate variables
            BusinessReturn = New BusinessReturn()
            blMembers = New blMembers
            blTeams = New blTeams


            'saveErrorMessage(-1, "context.Request.QueryString(queryParameters.createAcct)", context.Request.QueryString(queryParameters.createAcct))
            'saveErrorMessage(-1, "context.Request.QueryString(queryParameters.acctData)", context.Request.QueryString(queryParameters.acctData))

            'Verify if we need to create the account first.
            If Not String.IsNullOrEmpty(context.Request.QueryString(queryParameters.createAcct)) Then
                If context.Request.QueryString(queryParameters.createAcct) = True.ToString Then
                    'Ensure that the new acct info is provided
                    If Not String.IsNullOrEmpty(context.Request.QueryString(queryParameters.acctData)) Then
                        'Try to create account
                        BusinessReturn = createAccount(context)
                        'saveErrorMessage(-1, "BusinessReturn.isValid", BusinessReturn.isValid & " | " & BusinessReturn.ExceptionMessage & " | " & BusinessReturn.ReturnMessage)
                    End If
                End If
            End If



            'Will only be invalid if a new account failed to be created.
            If BusinessReturn.isValid Then
                'Determine if data is a campaign or team member invitation.
                If Not String.IsNullOrEmpty(context.Request.QueryString(queryParameters.params_campaignInvitation)) Then
                    'Import json data from querystring into class
                    campaignInvitation = New CampaignInvitation
                    params = context.Request.QueryString(queryParameters.params_campaignInvitation)
                    Newtonsoft.Json.JsonConvert.PopulateObject(WebUtility.UrlDecode(params), campaignInvitation)

                    'Determine if email validation has expired.
                    If DateTime.Now <= campaignInvitation.dateSubmitted.AddDays(2) Then
                        'Email is valid.  Continue.
                        '==========================================
                        'Determine if user has been created already
                        If blMembers.doesMemberExist_byEmail(campaignInvitation.email) Then
                            'Member exists
                            '           Add member to campaign's team with role/description:
                            '               If Team Admin:
                            If campaignInvitation.roleValue = memberRole.TeamAdministrator Then
                                '                   Add member id to team IPublishedContent if it does not exist
                                '                   Remove member from all child campaigns/team member folders if exists
                                BusinessReturn = blTeams.AddTeamAdministratorToCampaign_byMemberId(blMembers.getMemberId_byEmail(campaignInvitation.email), campaignInvitation.campaignId)

                            Else
                                '                   Add member to campaign/team member folder if it does not exist
                                '                   Add role/description
                                '                   Remove member from campaign's team if exists
                                BusinessReturn = blTeams.AddCampaignMember_byMemberId(blMembers.getMemberId_byEmail(
                                                                                      campaignInvitation.email),
                                                                                      campaignInvitation.campaignId,
                                                                                      campaignInvitation.roleValue)



                                saveErrorMessage(-1, WebUtility.UrlDecode(params), BusinessReturn.isValid & " | " & BusinessReturn.ExceptionMessage & " | " & BusinessReturn.ReturnMessage)



                            End If

                            'Navigate to member's edit account page and show success alert msg
                            context.Response.Redirect(_uHelper.Get_IPublishedContentByID(siteNodes.EditAccount).Url & "?" & queryParameters.userId & "=" & blMembers.getMemberId_byEmail(campaignInvitation.email) &
                                                      "&" & queryParameters.updatedSuccessfully & "=" & True.ToString, False)
                            HttpContext.Current.ApplicationInstance.CompleteRequest()

                        Else
                            ''Member does not exist.  Create it.
                            ''           navigate to 'become-a-member' [pass data via querystring for further processing.]
                            'Dim redirectUrl As String = _uHelper.Get_IPublishedContentByID(siteNodes.BecomeAMember).UrlAbsolute & "?" &
                            '        queryParameters.params_campaignInvitation & "=" & context.Request.QueryString(queryParameters.params_campaignInvitation)



                            'navigate to 'become-a-member' [pass data via querystring for further processing.]
                            Dim redirectUrl As String = _uHelper.Get_IPublishedContentByID(siteNodes.BecomeAMember).UrlAbsolute & "?" &
                                queryParameters.params_campaignInvitation & "=" & context.Request.QueryString(queryParameters.params_campaignInvitation) &
                                "&" & queryParameters.createAcct & "=" & context.Request.QueryString(queryParameters.createAcct) &
                                "&" & queryParameters.preAcctId & "=" & context.Request.QueryString(queryParameters.preAcctId)


                            context.Response.Redirect(redirectUrl, False)
                            HttpContext.Current.ApplicationInstance.CompleteRequest()
                        End If
                    Else
                        ''Email is no longer active.  Navigate to message page
                        context.Response.Redirect(_uHelper.Get_IPublishedContentByID(siteNodes.ExpiredInvitation).Url, False)
                        HttpContext.Current.ApplicationInstance.CompleteRequest()
                    End If











                ElseIf Not String.IsNullOrEmpty(context.Request.QueryString(queryParameters.params_teamInvitation)) Then
                    'Import json data from querystring into class
                    teamInvitation = New TeamInvitation
                    params = context.Request.QueryString(queryParameters.params_teamInvitation)
                    Newtonsoft.Json.JsonConvert.PopulateObject(WebUtility.UrlDecode(params), teamInvitation)

                    'Determine if email validation has expired.
                    If DateTime.Now <= teamInvitation.dateSubmitted.AddDays(2) Then
                        'Email is valid.  Continue.
                        '==========================================
                        'Determine if user has been created already
                        If blMembers.doesMemberExist_byEmail(teamInvitation.email) Then
                            'Member exists
                            '           Add member to campaign's team with role/description:
                            '               If Team Admin:
                            '                   Add member id to team IPublishedContent if it does not exist
                            BusinessReturn = blTeams.AddTeamAdministratorToTeam_byMemberId(blMembers.getMemberId_byEmail(teamInvitation.email), teamInvitation.teamId)

                            'Navigate to member's edit account page and show success alert msg
                            context.Response.Redirect(_uHelper.Get_IPublishedContentByID(siteNodes.EditAccount).Url & "?" & queryParameters.userId & "=" & blMembers.getMemberId_byEmail(teamInvitation.email) &
                                                      "&" & queryParameters.updatedSuccessfully & "=" & True.ToString, False)
                            HttpContext.Current.ApplicationInstance.CompleteRequest()

                        Else
                            ''Member does not exist.  Create it.
                            ''           navigate to 'become-a-member' [pass data via querystring for further processing.]
                            Dim redirectUrl As String = _uHelper.Get_IPublishedContentByID(siteNodes.BecomeAMember).Url & "?" & queryParameters.params_teamInvitation & "=" & params
                            context.Response.Redirect(redirectUrl, False)
                            HttpContext.Current.ApplicationInstance.CompleteRequest()
                        End If
                    Else
                        ''Email is no longer active.  Navigate to message page
                        context.Response.Redirect(_uHelper.Get_IPublishedContentByID(siteNodes.ExpiredInvitation).Url, False)
                        HttpContext.Current.ApplicationInstance.CompleteRequest()
                    End If



                End If
            Else
                ''Cannot create account
                context.Response.Redirect(_uHelper.Get_IPublishedContentByID(siteNodes.CannotCreateAcct).Url, False)
                HttpContext.Current.ApplicationInstance.CompleteRequest()
            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\Handlers\CampaignInvitations.ashx : ProcessRequest()")
            sb.AppendLine("context:" & context.ToString())

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            context.Response.ContentType = "text/plain"
            context.Response.Write("<br />Error: " & ex.ToString)
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
                    'Return New BusinessReturn
                    Return returnResult
                End If
            Else
                returnResult.ExceptionMessage = "Invalid id/email values"
                Return returnResult
            End If

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\Handlers\CampaignInvitations.ashx : createAccount()")
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