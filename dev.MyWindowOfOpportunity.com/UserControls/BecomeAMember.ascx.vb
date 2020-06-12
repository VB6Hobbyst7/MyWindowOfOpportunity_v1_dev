Imports Common
Imports umbraco
Imports umbraco.Core
Imports ASPSnippets.FaceBookAPI
Imports umbraco.Web
Imports System.Net
Imports umbraco.Core.Models

Partial Class UserControls_BecomeAMember
    Inherits baseClass


#Region "Properties"
    Private campaignInvitation As CampaignInvitation
    Private blMembers As blMembers
    Private blTeams As blTeams
    Private blEmails As blEmails
    Private blPreAcctData As blPreAcctData
    Private returnObject As New BusinessReturn()
    Dim _uHelper As Uhelper = New Uhelper()

    Private Enum Views
        vBecomeAMember = 0
        vEmailSent = 1
        vAccountCreated = 2
        vAccountCreated_AddedToCampaign = 3
        vAccountCreated_NotAddedToCampaign = 4
        vInvalid = 5
    End Enum
#End Region

#Region "Handles"
    Private Sub UserControls_BecomeAMember_Init(sender As Object, e As EventArgs) Handles Me.Init
        Try
            If IsPostBack Then
                'Display page after account has been created
                mvBecomeAMember.ActiveViewIndex = Views.vAccountCreated
                If Session(queryParameters.newMember) = True Then Session(queryParameters.newMember) = Nothing
            Else
                'Instantiate variables
                returnObject = New BusinessReturn()
                FaceBookConnect.API_Key = System.Configuration.ConfigurationManager.AppSettings("FBAPI_Key").ToString()
                FaceBookConnect.API_Secret = System.Configuration.ConfigurationManager.AppSettings("FBAPI_Secret").ToString()

                'saveErrorMessage(getLoggedInMember, "Init Request.QueryString(queryParameters.preAcctId)", Request.QueryString(queryParameters.preAcctId))

                'Determine if we need to create submitted account or start a new one.
                If String.IsNullOrEmpty(Request.QueryString(queryParameters.preAcctId)) Then
                    'Show page to create account
                    mvBecomeAMember.ActiveViewIndex = Views.vBecomeAMember
                    'See if new member was being invited to a campaign
                    If Not String.IsNullOrEmpty(Request.QueryString(queryParameters.params_campaignInvitation)) Then
                        'Import json data from querystring into class
                        Dim params As String
                        campaignInvitation = New CampaignInvitation
                        params = Context.Request.QueryString(queryParameters.params_campaignInvitation)
                        Newtonsoft.Json.JsonConvert.PopulateObject(WebUtility.UrlDecode(params), campaignInvitation)

                        'Populate fields with imported data from querystring.
                        txbEmail.Text = campaignInvitation.email
                    End If
                Else
                    'Try to create account
                    returnObject = createAccount()

                    'saveErrorMessage(-3, "createAccount()", returnObject.isValid & " | " & returnObject.ExceptionMessage & " | " & returnObject.ReturnMessage)

                    If returnObject.isValid Then
                        If String.IsNullOrEmpty(Request.QueryString(queryParameters.params_campaignInvitation)) Then
                            'Account has been created successfully.  If logged in, transfer to edit page.
                            blMembers = New blMembers
                            If blMembers.isMemberLoggedIn() Then
                                'Go to edit page with success msg.
                                Response.Redirect(_uHelper.Get_IPublishedContentByID(siteNodes.EditAccount).Url & "?" & queryParameters.newMember & "=" & True.ToString, False)
                                HttpContext.Current.ApplicationInstance.CompleteRequest()
                            Else
                                'Display Page after account has been created
                                mvBecomeAMember.ActiveViewIndex = Views.vAccountCreated
                            End If
                        Else
                            'Attempt to add user to campaign
                            Dim returnObject = addUserToCampaign()
                            If returnObject.isValid Then
                                'Show successfully created and added to campaign
                                mvBecomeAMember.ActiveViewIndex = Views.vAccountCreated_AddedToCampaign
                            Else
                                'Show successfully created, but was not added to the campaign.
                                mvBecomeAMember.ActiveViewIndex = Views.vAccountCreated_NotAddedToCampaign
                                'Response.Write("Error Msg: " & returnObject.ExceptionMessage)
                            End If
                        End If
                    Else
                        'Display page after account has been created
                        mvBecomeAMember.ActiveViewIndex = Views.vInvalid
                        'Response.Write(returnObject.ExceptionMessage)
                    End If
                End If
            End If

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\BecomeAMember.ascx.vb : UserControls_BecomeAMember_Init()")

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Response.Write("Error: " & ex.ToString)
        End Try

    End Sub
    Private Sub btnCreateAcct_Click(sender As Object, e As EventArgs) Handles btnCreateAcct.Click
        Try
            If Page.IsValid Then
                'Instantiate variables
                Dim returnObject As New BusinessReturn()
                Dim url As String
                blMembers = New blMembers
                blPreAcctData = New blPreAcctData

                'Validate data
                returnObject = blMembers.IsUserUnique(txbEmail.Text.Trim.ToLower, txbPassword.Text.Trim)

                If (returnObject.isValid) Then
                    'Instantiate variable
                    returnObject = New BusinessReturn()

                    'submit acct data and obtaining id
                    returnObject = blPreAcctData.addData(txbFirstName.Text, txbLastName.Text, txbEmail.Text, txbPassword.Text, txbDateOfBirth.Value.Trim)

                    If returnObject.isValid Then
                        'Build url
                        If Not String.IsNullOrEmpty(Request.QueryString(queryParameters.params_campaignInvitation)) Then
                            'Create link to handler with params.
                            'url = ConfigurationManager.AppSettings(Miscellaneous.siteUrl) & Miscellaneous.handler_CampaignInvitation &
                            '    "?" & queryParameters.params_campaignInvitation & "=" & WebUtility.UrlEncode(Context.Request.QueryString(queryParameters.params_campaignInvitation)) &
                            '    "&" & queryParameters.createAcct & "=" & True.ToString &
                            '    "&" & queryParameters.preAcctId & "=" & returnObject.ReturnMessage


                            'Create link to handler with params.
                            url = ConfigurationManager.AppSettings(Miscellaneous.siteUrl) & Miscellaneous.handler_CampaignInvitation &
                                "?" & queryParameters.createAcct & "=" & True.ToString &
                                "&" & queryParameters.preAcctId & "=" & returnObject.ReturnMessage &
                                "&" & queryParameters.params_campaignInvitation & "=" & WebUtility.UrlEncode(Context.Request.QueryString(queryParameters.params_campaignInvitation))


                        Else
                            'Create return link with params.
                            url = _uHelper.Get_IPublishedContentByID(siteNodes.BecomeAMember).UrlAbsolute & "?" & queryParameters.preAcctId & "=" & returnObject.ReturnMessage
                            'url = UmbracoContext.Current.PublishedContentRequest.PublishedContent.Url & "?" & queryParameters.preAcctId & "=" & returnObject.ReturnMessage
                        End If

                        'Send email
                        blEmails = New blEmails
                        blEmails.sendVerifyEmailToCreateAcct(url,
                              txbFirstName.Text.Trim.ToLower.ToFirstUpper + " " + txbLastName.Text.Trim.ToLower.ToFirstUpper,
                              txbEmail.Text.Trim.ToLower)

                        'Show thankyou page with instructions
                        mvBecomeAMember.ActiveViewIndex = Views.vEmailSent

                        hfldErrorMsg.Value = "is valid... submitted"
                    Else
                        'Display any validation errors
                        'DisplayValidationErrors(returnObject, ValidationSummary1.ValidationGroup)
                        hfldErrorMsg.Value = "Error 1: " & returnObject.ExceptionMessage
                    End If
                Else
                    If Not String.IsNullOrEmpty(returnObject.ExceptionMessage) Then
                        hfldErrorMsg.Value = returnObject.ExceptionMessage
                    Else
                        hfldErrorMsg.Value = returnObject.ValidationMessages(0).ErrorMessage
                        If returnObject.ValidationMessages.Count > 1 Then
                            hfld2ndErrorMsg.Value = returnObject.ValidationMessages(1).ErrorMessage
                        End If
                    End If


                End If
            Else
                hfldErrorMsg.Value = "Page is not valid"
            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\BecomeAMember.ascx.vb : btnCreateAcct_Click()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
    End Sub
    Private Sub lnkFblogint_Click(sender As Object, e As EventArgs) Handles lnkFblogin.Click
        'Send user to facebook to log in.
        blMembers = New blMembers
        blMembers.logMemberIntoFacebook()
    End Sub
    Protected Sub lnkTwitterLogin_Click(sender As Object, e As EventArgs) Handles lnkTwitterLogin.Click
        'Send user to twitter to log in.
        blMembers = New blMembers
        blMembers.logMemberIntoTwitter()
    End Sub
    Protected Sub lnkLinkedInLogin_Click(sender As Object, e As EventArgs) Handles lnkLinkedInLogin.Click
        'Send user to LinkedIn to log in.
        blMembers = New blMembers
        blMembers.logMemberIntoLinkedIn()
    End Sub
#End Region

#Region "Methods"
    Private Function createAccount() As BusinessReturn
        Try
            'Dim sb As New StringBuilder()

            'Instantiate variables
            Dim brIsUserUnique As New BusinessReturn()
            Dim brPreAcctData As New BusinessReturn()
            blMembers = New blMembers
            blPreAcctData = New blPreAcctData
            Dim preAcctId As Integer
            Dim result_preAccount As PreAccountmembers = New PreAccountmembers

            'saveErrorMessage(getLoggedInMember, "Request.QueryString(queryParameters.preAcctId)", Request.QueryString(queryParameters.preAcctId))

            'Proceed if query parameter is numeric
            If IsNumeric(Request.QueryString(queryParameters.preAcctId)) Then
                'Obtain Id
                preAcctId = CInt(Request.QueryString(queryParameters.preAcctId))

                'Obtain pre-acct data
                brPreAcctData = blPreAcctData.selectRecord_byId(preAcctId)
                If brPreAcctData.DataContainer.Count > 0 Then result_preAccount = brPreAcctData.DataContainer(0)

                If Not String.IsNullOrEmpty(result_preAccount.email) Then
                    'Delete record from db
                    blPreAcctData.deleteRecord(result_preAccount.preAcctId)

                    'Validate key values to ensure account was not already created.
                    If Not String.IsNullOrEmpty(result_preAccount.password) AndAlso Not String.IsNullOrEmpty(result_preAccount.email) Then
                        'Validate data
                        brIsUserUnique = blMembers.IsUserUnique(result_preAccount.email, result_preAccount.password)

                        'If valid credentials, create account
                        If (brIsUserUnique.isValid) Then
                            Return blMembers.Insert_byPreAcct(result_preAccount)
                        Else
                            Return brIsUserUnique
                        End If
                    Else
                        brIsUserUnique.ExceptionMessage = "Invalid email or password"
                        Return brIsUserUnique
                    End If
                Else
                    brIsUserUnique.ExceptionMessage = "Account has already been created.  Please log in."
                    Return brIsUserUnique
                End If
            Else
                'sb.AppendLine("Invalid Id")
                'saveErrorMessage(getLoggedInMember, sb.ToString, sb.ToString())

                brIsUserUnique.ExceptionMessage = "Invalid Id"
                Return brIsUserUnique
            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\BecomeAMember.ascx.vb : createAccount()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

            '  saveErrorMessage("", ex.ToString, "")
            returnObject.ExceptionMessage = ex.ToString()
            Return returnObject
        End Try
    End Function
    Private Function addUserToCampaign() As BusinessReturn
        Try
            campaignInvitation = New CampaignInvitation
            blMembers = New blMembers
            blTeams = New blTeams
            Dim params As String
            'returnObject = New BusinessReturn

            'Import json data from querystring into class
            params = Context.Request.QueryString(queryParameters.params_campaignInvitation)
            Newtonsoft.Json.JsonConvert.PopulateObject(WebUtility.UrlDecode(params), campaignInvitation)


            '           Add member to campaign's team with role/description:
            '               If Team Admin:
            If campaignInvitation.roleValue = memberRole.TeamAdministrator Then
                '                   Add member id to team IPublishedContent if it does not exist
                '                   Remove member from all child campaigns/team member folders if exists
                'returnObject = blTeams.AddTeamAdministrator_byMemberId(blMembers.getMemberId_byEmail(campaignInvitation.email), campaignInvitation.campaignId)
                returnObject = blTeams.AddTeamAdministratorToCampaign_byMemberId(blMembers.getMemberId_byEmail(campaignInvitation.email), campaignInvitation.campaignId)


            Else
                returnObject = blTeams.AddCampaignMember_byMemberId(blMembers.getMemberId_byEmail(campaignInvitation.email), campaignInvitation.campaignId, campaignInvitation.roleValue)
            End If

            Return returnObject
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\UserControls_BecomeAMember.ascx.vb : addUserToCampaign()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return Nothing
        End Try
        '           Create New user
        '           Add parameters to email verification for adding member to campaign.
        '           Once verified, add member to campaign with role/description.:
        '               If Team Admin:
        '                   Add member id to team IPublishedContent
        '               Else:
        '                   Add member to campaign/team member folder 
        '                   Add role/description


    End Function
#End Region

End Class





'Private Sub UserControls_BecomeAMember_Init(sender As Object, e As EventArgs) Handles Me.Init
'    Try
'        'Session(queryParameters.newMember) = Nothing
'        If Session(queryParameters.newMember) = True Then
'            Session(queryParameters.newMember) = Nothing
'            'Response.Redirect(New IPublishedContent(siteNodes.EditAccount).NiceUrl & "?" & queryParameters.newMember & "=" & True.ToString)
'            Response.Redirect(_uHelper.Get_IPublishedContentByID(siteNodes.EditAccount).Url & "?" & queryParameters.newMember & "=" & True.ToString, False)
'            'mvBecomeAMember.ActiveViewIndex = Views.vAccountCreated '=====================DELETE WHEN WORKS
'        Else
'            'Display page after account has been created
'            mvBecomeAMember.ActiveViewIndex = Views.vAccountCreated
'        End If


'        If Not IsPostBack Then
'            'Instantiate variables
'            returnObject = New BusinessReturn()
'            FaceBookConnect.API_Key = System.Configuration.ConfigurationManager.AppSettings("FBAPI_Key").ToString()
'            FaceBookConnect.API_Secret = System.Configuration.ConfigurationManager.AppSettings("FBAPI_Secret").ToString()

'            saveErrorMessage(getLoggedInMember, "Init Request.QueryString(queryParameters.preAcctId)", Request.QueryString(queryParameters.preAcctId))

'            'Determine if we need to create submitted account or start a new one.
'            If String.IsNullOrEmpty(Request.QueryString(queryParameters.preAcctId)) Then
'                'Show page to create account
'                mvBecomeAMember.ActiveViewIndex = Views.vBecomeAMember
'                'See if new member was being invited to a campaign
'                If Not String.IsNullOrEmpty(Request.QueryString(queryParameters.params_campaignInvitation)) Then
'                    'Import json data from querystring into class
'                    Dim params As String
'                    CampaignInvitation = New CampaignInvitation
'                    params = Context.Request.QueryString(queryParameters.params_campaignInvitation)
'                    Newtonsoft.Json.JsonConvert.PopulateObject(WebUtility.UrlDecode(params), CampaignInvitation)

'                    'Populate fields with imported data from querystring.
'                    txbEmail.Text = CampaignInvitation.email
'                End If
'            Else
'                'Try to create account
'                returnObject = createAccount()

'                saveErrorMessage(-3, "createAccount()", returnObject.isValid & " | " & returnObject.ExceptionMessage & " | " & returnObject.ReturnMessage)

'                If returnObject.isValid Then
'                    If String.IsNullOrEmpty(Request.QueryString(queryParameters.params_campaignInvitation)) Then
'                        'Account has been created successfully.  If logged in, transfer to edit page.
'                        blMembers = New blMembers
'                        If blMembers.isMemberLoggedIn() Then
'                            'Go to edit page with success msg.
'                            Response.Redirect(_uHelper.Get_IPublishedContentByID(siteNodes.EditAccount).Url & "?" & queryParameters.newMember & "=" & True.ToString, False)
'                            HttpContext.Current.ApplicationInstance.CompleteRequest()
'                        Else
'                            'Display Page after account has been created
'                            mvBecomeAMember.ActiveViewIndex = Views.vAccountCreated
'                        End If
'                    Else
'                        'Attempt to add user to campaign
'                        Dim returnObject = addUserToCampaign()
'                        If returnObject.isValid Then
'                            'Show successfully created and added to campaign
'                            mvBecomeAMember.ActiveViewIndex = Views.vAccountCreated_AddedToCampaign
'                        Else
'                            'Show successfully created, but was not added to the campaign.
'                            mvBecomeAMember.ActiveViewIndex = Views.vAccountCreated_NotAddedToCampaign
'                            'Response.Write("Error Msg: " & returnObject.ExceptionMessage)
'                        End If
'                    End If
'                Else
'                    'Display page after account has been created
'                    mvBecomeAMember.ActiveViewIndex = Views.vInvalid
'                    'Response.Write(returnObject.ExceptionMessage)
'                End If
'            End If
'        End If

'    Catch ex As Exception
'        Dim sb As New StringBuilder()
'        sb.AppendLine("\UserControls\BecomeAMember.ascx.vb : UserControls_BecomeAMember_Init()")

'        saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
'        'Response.Write("Error: " & ex.ToString)
'    End Try

'End Sub