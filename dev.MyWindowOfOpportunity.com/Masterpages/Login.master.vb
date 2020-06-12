Imports umbraco
Imports umbraco.NodeFactory
Imports Common
Imports ASPSnippets.FaceBookAPI
Imports System.Web.Script.Serialization
Imports Newtonsoft.Json.Linq
Imports System.Net.Http

Partial Class Masterpages_Standard_Page
    Inherits System.Web.UI.MasterPage

#Region "Properties"
    Private blMembers As blMembers

    Private blPreAcctData As blPreAcctData
    Private returnObject As New BusinessReturn()
    Dim _uHelper As Uhelper = New Uhelper()
#End Region

#Region "Handles"
    Private Sub Masterpages_Standard_Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                'Forgot Password Url
                ForgotPassword.NavigateUrl = _uHelper.Get_IPublishedContentByID(Common.siteNodes.ResetPassword).Url

                'Create link to become a member
                hlnkBecomeAMember.NavigateUrl = _uHelper.Get_IPublishedContentByID(siteNodes.BecomeAMember).Url
                hlnkBecomeAMember_Mbl.NavigateUrl = hlnkBecomeAMember.NavigateUrl

                'If access denied show alert msg
                If Request.QueryString(queryParameters.error_) = "access_denied" Then
                    Dim alert As ASP.usercontrols_alertmsg_ascx = New ASP.usercontrols_alertmsg_ascx
                    alert.AlertMsg = "User has denied access.."
                    alert.MessageType = UserControls_AlertMsg.msgType.Alert

                    'Display alert msg
                    phAlerts.Controls.Add(alert)
                    Return
                End If

                'Is user acct updated?
                If Request.QueryString("loginupdated") = "true" Then
                    Dim alert As ASP.usercontrols_alertmsg_ascx = New ASP.usercontrols_alertmsg_ascx
                    alert.AlertMsg = "User account updated successfully."
                    alert.MessageType = UserControls_AlertMsg.msgType.Success

                    'Display alert msg
                    phAlerts.Controls.Add(alert)
                End If

                'Obtain data from querystring
                Dim code As String = Request.QueryString("code")
                Dim state As String = Request.QueryString("state")


                If Not String.IsNullOrEmpty(state) AndAlso LinkedInUtil.LinkedInState = state Then
                    If Not String.IsNullOrEmpty(code) Then
                        '============   LINKEDIN LOGIN   ===========
                        '===========================================
                        'Instantiate variables
                        blMembers = New blMembers
                        Dim bReturn As New BusinessReturn

                        'Attempt to log facebook user in
                        bReturn = blMembers.logMemberIn_byLinkedIn(code)
                        resolveLogin(bReturn)

                    End If
                ElseIf Not String.IsNullOrEmpty(code) Then
                    '============   FACEBOOK LOGIN   ===========
                    '===========================================
                    'Instantiate variables
                    blMembers = New blMembers
                    Dim bReturn As New BusinessReturn

                    'Attempt to log facebook user in
                    bReturn = blMembers.logMemberIn_byFacebook(code)
                    resolveLogin(bReturn)
                End If



                '============   TWITTER LOGIN   ===========
                '==========================================
                If Request("oauth_token") IsNot Nothing AndAlso Request("oauth_verifier") IsNot Nothing Then
                    'Instantiate variables
                    blMembers = New blMembers
                    Dim bReturn As New BusinessReturn

                    'Attempt to log twitter user in
                    bReturn = blMembers.logMemberIn_byTwitter(Request("oauth_token"), Request("oauth_verifier"))
                    resolveLogin(bReturn)
                End If
            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("Login.master.vb : Load()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
    End Sub
    Private Sub lnkFblogin_Click(sender As Object, e As EventArgs) Handles lnkFblogin.Click
        'Send user to facebook to log in.
        blMembers = New blMembers
        blMembers.logMemberIntoFacebook()
    End Sub
    Private Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        Try
            'Instantiate variables
            blMembers = New blMembers

            'Determine what kind of member is logging in (reg user, facebook user, etc)
            Dim memberLoginType As mediaType_Values = blMembers.getMembersLoginType(ucTxbEmail.Text.Trim)

            Select Case memberLoginType
                Case mediaType_Values.Facebook
                    'Display alert msg
                    Dim msg As ASP.usercontrols_alertmsg_ascx = New ASP.usercontrols_alertmsg_ascx
                    msg.AlertMsg = "This account uses a social login. &nbsp;&nbsp; Sign in with Facebook."
                    msg.MessageType = UserControls_AlertMsg.msgType.Info
                    phAlerts.Controls.Add(msg)

                Case mediaType_Values.LinkedIn
                    'Display alert msg
                    Dim msg As ASP.usercontrols_alertmsg_ascx = New ASP.usercontrols_alertmsg_ascx
                    msg.AlertMsg = "This account uses a social login. &nbsp;&nbsp; Sign in with LinkedIn."
                    msg.MessageType = UserControls_AlertMsg.msgType.Info
                    phAlerts.Controls.Add(msg)

                Case mediaType_Values.Twitter
                    'Display alert msg
                    Dim msg As ASP.usercontrols_alertmsg_ascx = New ASP.usercontrols_alertmsg_ascx
                    msg.AlertMsg = "This account uses a social login. &nbsp;&nbsp; Sign in with Twitter."
                    msg.MessageType = UserControls_AlertMsg.msgType.Info
                    phAlerts.Controls.Add(msg)

                Case mediaType_Values.none
                    'Attempt to log member in
                    If blMembers.logMemberIn(ucTxbEmail.Text.Trim, ucTxbPassword.Text.Trim) Then
                        If IsNothing(Session(Sessions.previousPage)) Then
                            'Redirect home
                            Response.Redirect(_uHelper.Get_IPublishedContentByID(siteNodes.Home).Url, False)
                            HttpContext.Current.ApplicationInstance.CompleteRequest()
                        Else
                            'Obtain previous page from session
                            Dim previousPage As String = Session(Sessions.previousPage)
                            'Clear session
                            Session(Sessions.previousPage) = Nothing
                            'redirect to previous page
                            Response.Redirect(previousPage, False)
                            HttpContext.Current.ApplicationInstance.CompleteRequest()
                        End If
                    Else
                        'Show message
                        Dim alert As ASP.usercontrols_alertmsg_ascx = New ASP.usercontrols_alertmsg_ascx With {
                            .AlertMsg = "Your username and/or password are invalid.",
                            .MessageType = UserControls_AlertMsg.msgType.Alert
                        }

                        'Display alert msg
                        phAlerts.Controls.Add(alert)

                    End If
            End Select












        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\Masterpages\Login.master.vb : btnSubmit_Click()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'create alert msg
            Dim alert As ASP.usercontrols_alertmsg_ascx = New ASP.usercontrols_alertmsg_ascx With {
                .AlertMsg = "An error occured attempting to log you in.  Please try again.",
                .AdditionalText = ex.ToString,
                .MessageType = UserControls_AlertMsg.msgType.Alert
            }

            'Display alert msg
            phAlerts.Controls.Add(alert)
        End Try
    End Sub
    Protected Sub lnkTwitterLogin_Click(sender As Object, e As EventArgs) Handles LinkButton1.Click
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
    Public Sub resolveLogin(ByRef bReturn As BusinessReturn)
        If bReturn.isValid Then
            'Successful login.  return to previous/home page
            If IsNothing(Session(Sessions.previousPage)) Then
                'Redirect home
                Response.Redirect(_uHelper.Get_IPublishedContentByID(siteNodes.Home).Url, False)
                HttpContext.Current.ApplicationInstance.CompleteRequest()
            Else
                'Obtain previous page from session
                Dim previousPage As String = Session(Sessions.previousPage)
                Session(Sessions.previousPage) = Nothing
                'redirect to previous page
                Response.Redirect(previousPage, False)
                HttpContext.Current.ApplicationInstance.CompleteRequest()
            End If

        Else
            'create message
            Dim alert As ASP.usercontrols_alertmsg_ascx = New ASP.usercontrols_alertmsg_ascx With {
                .AlertMsg = bReturn.ExceptionMessage.ToString,
                .MessageType = UserControls_AlertMsg.msgType.Alert
            }
            'Display alert msg
            phAlerts.Controls.Add(alert)
        End If
    End Sub
    Private Function createMemberAccount(result_preAccount As PreAccountmembers) As BusinessReturn
        'Instantiate variables
        Dim brIsUserUnique As New BusinessReturn()
        blMembers = New blMembers

        Try

            If Not String.IsNullOrEmpty(result_preAccount.password) AndAlso Not String.IsNullOrEmpty(result_preAccount.email) Then
                'Validate data
                brIsUserUnique = blMembers.IsUserUnique(result_preAccount.email, result_preAccount.password)
                'If valid credentials, create account
                If (brIsUserUnique.isValid) Then
                    'Create new member
                    'Return blMembers.Insert_byTblPreAccountData(result_preAccount)
                    Return blMembers.Insert_byPreAcct(result_preAccount)
                Else
                    Return brIsUserUnique
                End If
            Else
                brIsUserUnique.ExceptionMessage = "Invalid email or password"
                Return brIsUserUnique
            End If

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\Login.master.vb : createAccount()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            returnObject.ExceptionMessage = ex.ToString()
            Return returnObject
        End Try
    End Function
#End Region

End Class

