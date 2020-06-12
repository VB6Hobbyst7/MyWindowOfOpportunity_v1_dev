Imports Common
Partial Class UserControls_UserImageMngr
    Inherits System.Web.UI.UserControl
#Region "Properties"
    Private blMedia As blMedia
    Public Property memberId() As String
        Get
            Return hfldThisNodeId.Value
        End Get
        Set(value As String)
            hfldThisNodeId.Value = value.ToString
        End Set
    End Property
#End Region

#Region "Handles"
    Private Sub UserControls_TeamImageMngrLoad(sender As Object, e As EventArgs) Handles Me.PreRender
        Try
            'Obtain all campaign images
            obtainCampaignImages()
            'End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\UserImageMngr.ascx.vb : UserControls_TeamImageMngrLoad()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
        'End If
    End Sub
    Private Sub btnDeleteImage_Click(sender As Object, e As EventArgs) Handles btnDeleteImage.ServerClick
        Try
            '
            blMedia = New blMedia

            Dim result As BusinessReturn = blMedia.deleteMedia_byMemberId(memberId)

            If result.isValid Then
                'Refresh images in list.
                obtainCampaignImages()
            Else
                'Show error msg
                Dim sb As New StringBuilder()
                sb.AppendLine("\UserControls\UserImageMngr.ascx.vb : btnDeleteImage_Click()")
                saveErrorMessage(getLoggedInMember, result.ExceptionMessage, result.ExceptionMessage)
            End If

            'Set active tab
            setTabCookie(tabNames.profile)

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\UserImageMngr.ascx.vb : btnDeleteImage_Click()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Response.Write(ex.ToString)
        End Try
        'Response.Write("ALERT: Code Missing!!!")
    End Sub
    Private Sub btnRefreshPage_Click(sender As Object, e As EventArgs) Handles btnRefreshPage.ServerClick
        Try
            'Obtain all campaign images
            obtainCampaignImages()
            'Set active tab
            setTabCookie(tabNames.profile)
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\UserImageMngr.ascx.vb : btnRefreshPage_Click()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
    End Sub
#End Region

#Region "Methods"
    Private Sub obtainCampaignImages()
        'Instantiate variables
        blMedia = New blMedia

        Try
            If Not String.IsNullOrEmpty(memberId) AndAlso IsNumeric(memberId) Then
                'Obtain list of images and add to listview
                Dim result As BusinessReturn = blMedia.selectImage_byMemberId(memberId, Crops.members)

                'add images datatable to listview
                If result.isValid Then
                    'Add list to Image Library
                    lstviewImageLibrary.DataSource = result.DataContainer(0)
                    lstviewImageLibrary.DataBind()
                    'Add list to Image Cropper library
                    lstViewImageCroppers.DataSource = result.DataContainer(0)
                    lstViewImageCroppers.DataBind()
                End If
            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\UserImageMngr.ascx.vb : obtainCampaignImages()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Response.Write(ex.ToString)
        End Try

    End Sub
#End Region
End Class