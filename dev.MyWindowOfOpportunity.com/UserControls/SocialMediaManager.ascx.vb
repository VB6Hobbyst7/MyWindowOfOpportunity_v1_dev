Imports Common


Partial Class UserControls_SocialMediaManager
    Inherits System.Web.UI.UserControl


#Region "Properties"
    Private _nodeId As Int16 'pass to entry uc
    Public Property nodeId As Int16
        Get
            Return _nodeId
        End Get
        Set(value As Int16)
            _nodeId = value
        End Set
    End Property
#End Region

#Region "Handles"
    Private Sub UserControls_SocialMediaManager_Load(sender As Object, e As EventArgs) Handles Me.Load
        ''Send node id to uc's
        'ucSocialMediaEntry_Facebook.nodeId = nodeId
        'ucSocialMediaEntry_GooglePlus.nodeId = nodeId
        'ucSocialMediaEntry_LinkedIn.nodeId = nodeId
        'ucSocialMediaEntry_Twitter.nodeId = nodeId
    End Sub

    Private Sub UserControls_SocialMediaManager_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        Try
            'Send node id to uc's
            ucSocialMediaEntry_Facebook.nodeId = nodeId
            ucSocialMediaEntry_LinkedIn.nodeId = nodeId
            ucSocialMediaEntry_Twitter.nodeId = nodeId
            ucSocialMediaEntry_SupportEmail.nodeId = nodeId
            ' ucSocialMediaEntry_GooglePlus.nodeId = nodeId

            'Hide all messages
            ucAlertMsg_Invalid.Visible = False
            ucAlertMsg_Success.Visible = False

            'If session exists from submitting social media:
            If Not IsNothing(Session(Sessions.socialMediaSavedSuccessfully)) Then
                'Show message
                If Session(Sessions.socialMediaSavedSuccessfully) = True Then
                    ucAlertMsg_Success.Visible = True
                Else
                    ucAlertMsg_Invalid.Visible = True
                End If

                'Clear session
                Session(Sessions.socialMediaSavedSuccessfully) = Nothing
            End If

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\UserControls_SocialMediaManager.ascx.vb : UserControls_SocialMediaManager_PreRender()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Response.Write(ex.ToString)
        End Try
    End Sub
#End Region

#Region "Methods"
#End Region
End Class
