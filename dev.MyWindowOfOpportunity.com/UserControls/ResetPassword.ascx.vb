Imports Common
Imports umbraco.Core.Services
Imports umbraco.Core
Imports umbraco.Web

Partial Class UserControls_ResetPassword
    Inherits System.Web.UI.UserControl


#Region "Properties"
    Dim _uHelper As Uhelper = New Uhelper()
#End Region

#Region "Handles"
    Private Sub UserControls_ResetPassword_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                'Login Url
                LoginUrl.NavigateUrl = _uHelper.Get_IPublishedContentByID(Common.siteNodes.Login).Url

                If Request.QueryString("resetNode") <> "" Then

                    SetupResetPassowrd.ActiveViewIndex = 2

                End If
            End If

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\ResetPassword.ascx.vb : UserControls_ResetPassword_Load()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Response.Write("Error: " & ex.ToString & "<br />")
        End Try
    End Sub

    Private Sub BtnReset_Click(sender As Object, e As EventArgs) Handles btnReset.ServerClick

        Dim Email_User As New blMembers
        Dim Member_Id As Integer
        Dim MemberMode As Object
        Dim ms As IMemberService = ApplicationContext.Current.Services.MemberService
        Try
            If Request.QueryString("resetNode") <> "" And txbPassword.Text <> "" Then
                Member_Id = CInt(Request.QueryString("resetNode"))
                MemberMode = ms.GetById(Member_Id)
                ms.SavePassword(MemberMode, txbPassword.Text)
                SetupResetPassowrd.ActiveViewIndex = 3
            End If

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\ResetPassword.ascx.vb : BtnReset_Click()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Response.Write("Error: " & ex.ToString & "<br />")

        End Try

    End Sub

    Private Sub BtnPassWord_Click(sender As Object, e As EventArgs) Handles btnPassWord.ServerClick

        Dim Email_User As New blMembers
        Dim active As Boolean
        Dim active_user_id As Integer
        Dim url As String
        Dim sentemail As Boolean
        Dim blEmails As New blEmails

        Try
            If ucTxbEmail.Text.Length > 0 Then

                active = Email_User.doesMemberExist_byEmail(ucTxbEmail.Text)

                If active = True Then
                    active_user_id = Email_User.getMemberId_byEmail(ucTxbEmail.Text)

                    url = _uHelper.Get_IPublishedContentByID(siteNodes.ResetPassword).UrlAbsolute & "?resetNode=" & active_user_id.ToString()

                    sentemail = blEmails.SendResetPasswordEmail(url, Email_User.getMemberName_byId(active_user_id), ucTxbEmail.Text)

                    If sentemail = True Then
                        SetupResetPassowrd.ActiveViewIndex = 1
                    End If

                Else
                    InvaildEmailMessage.Visible = True

                End If

            End If

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\ResetPassword.ascx.vb : BtnPassWord_Click()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Response.Write("Error: " & ex.ToString & "<br />")

        End Try


    End Sub

#End Region

#Region "Methods"
#End Region
End Class
