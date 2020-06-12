Imports Common
Imports ASPSnippets.FaceBookAPI
Imports umbraco.Web
Imports Microsoft.Owin.Security

Partial Class UserControls_TopLevel_NavMinor
    Inherits System.Web.UI.UserControl

#Region "Property"
    Private blMembers As blMembers = New blMembers
    Dim _uHelper As Uhelper = New Uhelper()
#End Region

#Region "Handles"
    Private Sub UserControls_TopLevel_NavMinor_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            FaceBookConnect.API_Key = System.Configuration.ConfigurationManager.AppSettings("FBAPI_Key").ToString()
            FaceBookConnect.API_Secret = System.Configuration.ConfigurationManager.AppSettings("FBAPI_Secret").ToString()

            'Obtain links to controls
            'hlnkContactUs.NavigateUrl = New IPublishedContent(siteNodes.ContactUs).NiceUrl
            'hlnkContactUs.Text = New IPublishedContent(siteNodes.ContactUs).Name

            hlnkBecomeAMember.NavigateUrl = _uHelper.Get_IPublishedContentByID(siteNodes.BecomeAMember).Url
            hlnkBecomeAMember.Text = _uHelper.Get_IPublishedContentByID(siteNodes.BecomeAMember).Name

            hlnkEditAcct.NavigateUrl = _uHelper.Get_IPublishedContentByID(siteNodes.EditAccount).Url

            hlnkCreateCampaign.NavigateUrl = _uHelper.Get_IPublishedContentByID(siteNodes.CreateACampaign).Url
            hlnkManageCampaigns.NavigateUrl = _uHelper.Get_IPublishedContentByID(siteNodes.ManageCampaigns).Url

            ForgotPassword.NavigateUrl = _uHelper.Get_IPublishedContentByID(siteNodes.ResetPassword).Url

            'hlnkLoginLnk.NavigateUrl = New IPublishedContent(siteNodes.Login).NiceUrl
        End If

        'Determine what status to display for the login panel.
        setLoginStatus()
    End Sub
    Private Sub lbtnLogin_Click(sender As Object, e As EventArgs) Handles lbtnLogin.Click
        Try
            '
            pnlInvalidCredentials.Visible = False
            pnlFacebookUser.Visible = False
            pnlTwitterUser.Visible = False
            pnlLinkedInUser.Visible = False

            'Determine what kind of member is logging in (reg user, facebook user, etc)
            Dim memberLoginType As mediaType_Values = blMembers.getMembersLoginType(txbEmail.Text.Trim)

            Select Case memberLoginType
                Case mediaType_Values.Facebook
                    pnlFacebookUser.Visible = True
                Case mediaType_Values.LinkedIn
                    pnlLinkedInUser.Visible = True
                Case mediaType_Values.Twitter
                    pnlTwitterUser.Visible = True
                Case mediaType_Values.none
                    If blMembers.logMemberIn(txbEmail.Text.Trim, txbPassword.Text.Trim) Then
                        Select Case UmbracoContext.Current.PageId
                            Case siteNodes.BecomeAMember, siteNodes.Login
                                'Redirect home
                                Response.Redirect(_uHelper.Get_IPublishedContentByID(siteNodes.Home).Url, False)
                                HttpContext.Current.ApplicationInstance.CompleteRequest()
                            Case Else
                                'Force page to be refreshed to ensure login/out takes effect.
                                Response.Redirect(Request.Url.AbsoluteUri, False)
                                HttpContext.Current.ApplicationInstance.CompleteRequest()
                        End Select
                    Else
                        'Invalid credentials
                        pnlInvalidCredentials.Visible = True
                    End If
            End Select


        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\NavMinor.ascx.vb : lbtnLogin_Click()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Invalid credentials
            pnlInvalidCredentials.Visible = True
        End Try
    End Sub
    Protected Sub lbtnLogout_Click(sender As Object, e As EventArgs)
        'Log member out
        blMembers.logMemberOut()

        'Force page to be refreshed to ensure login/out takes effect.
        Response.Redirect(Request.Url.AbsoluteUri, False)
        HttpContext.Current.ApplicationInstance.CompleteRequest()
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
    Private Sub setLoginStatus()
        Try
            'Determine what to display for the login/out options.
            Dim isMemberLoggedIn As Boolean = blMembers.isMemberLoggedIn
            pnlLoggedIn.Visible = isMemberLoggedIn
            pnlLoggedOut.Visible = Not isMemberLoggedIn

            If isMemberLoggedIn Then
                'If logged in, do not show the following pages.
                Select Case UmbracoContext.Current.PageId
                    Case siteNodes.BecomeAMember, siteNodes.Login
                        'Redirect home
                        Response.Redirect(_uHelper.Get_IPublishedContentByID(siteNodes.Home).Url, False)
                        HttpContext.Current.ApplicationInstance.CompleteRequest()
                End Select


                'hlnkContactUs.Visible = False
                lblUserName.Visible = True
                lblUserName.Text = "Welcome back " + blMembers.GetCurrentMembersFirstName()
                pnlLoggedOut.Visible = False
            End If

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\NavMinor.ascx.vb : setLoginStatus()")

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
    End Sub
#End Region
End Class