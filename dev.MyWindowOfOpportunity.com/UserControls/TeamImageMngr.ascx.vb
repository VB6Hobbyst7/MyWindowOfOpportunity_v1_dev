Imports umbraco
Imports Common
Imports umbraco.Core.Models
Imports Newtonsoft.Json
Imports umbraco.Web
Imports umbraco.Core.Services
Imports umbraco.Core

Partial Class UserControls_TeamImageMngr
    Inherits System.Web.UI.UserControl


#Region "Properties"
    Private blMedia As blMedia
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
#End Region

#Region "Handles"
    Private Sub UserControls_TeamImageMngrLoad(sender As Object, e As EventArgs) Handles Me.PreRender
        If Not String.IsNullOrEmpty(thisNodeId) Then
            Try
                'Instantiate variables
                Dim ipTeam As IPublishedContent = _uHelper.Get_IPublishedContentByID(CInt(thisNodeId))

                'Set hidden field with tab name
                hfldTabName.Value = tabNames.images

                'Obtain media folder
                If ipTeam.HasValue(nodeProperties.mediaFolder) Then
                    hfldMediaFolder.Value = ipTeam.GetPropertyValue(Of Integer)(nodeProperties.mediaFolder)
                Else
                    'Media folder is not in cache.  Republish and attempt to get value again.
                    Dim cs As IContentService = ApplicationContext.Current.Services.ContentService
                    Dim icTeam As IContent = cs.GetById(ipTeam.Id)
                    cs.SaveAndPublishWithStatus(icTeam)
                    hfldMediaFolder.Value = icTeam.GetValue(Of Integer)(nodeProperties.mediaFolder)
                End If

                'Obtain all campaign images
                obtainCampaignImages()
                'End If
            Catch ex As Exception
                Dim sb As New StringBuilder()
                sb.AppendLine("\UserControls\TeamImageMngr.ascx.vb : UserControls_TeamImageMngrLoad()")
                saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
                'Response.Write("Error: " & ex.ToString)
            End Try
        End If
    End Sub
    Private Sub btnDeleteImage_Click(sender As Object, e As EventArgs) Handles btnDeleteImage.Click
        Try
            '
            blMedia = New blMedia
            Dim mediaId As Integer = CInt(hfldMediaIdToDelete.Value)

            Dim result As BusinessReturn = blMedia.deleteMedia_byId(mediaId)

            If result.isValid Then
                'Show success msg

                ''Refresh images in list.
                'obtainCampaignImages()

                'Refresh page
                Response.Redirect(Request.Url.AbsolutePath & "?" & queryParameters.editMode & "=" & True, False)
                HttpContext.Current.ApplicationInstance.CompleteRequest()
            Else
                'Show error msg
                'Response.Write(result.ExceptionMessage)
            End If

            'Response.Write("clicked: " & hfld.Value)
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\TeamImageMngr.ascx.vb : btnDeleteImage_Click()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Response.Write(ex.ToString)
        End Try
    End Sub
    Private Sub btnRefreshPage_Click(sender As Object, e As EventArgs) Handles btnRefreshPage.Click
        ''Obtain all campaign images
        'obtainCampaignImages()

        'Refresh page
        Response.Redirect(Request.Url.AbsolutePath & "?" & queryParameters.editMode & "=" & True, False)
        HttpContext.Current.ApplicationInstance.CompleteRequest()
    End Sub
#End Region

#Region "Methods"
    Private Sub obtainCampaignImages()
        Try
            'Instantiate variables
            blMedia = New blMedia
            Dim thisNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(CInt(thisNodeId))

            'Obtain list of images and add to listview
            Dim result As BusinessReturn = blMedia.selectImages_byCampaignId(thisNode)

            Dim JSONString As String
            Dim path As String

            'Convert json to string
            JSONString = JsonConvert.SerializeObject(result.DataContainer(0))
            JSONString = JSONString.Replace("imgUrl", "image")
            'path = Server.MapPath("~/ckeditor/")
            path = Server.MapPath("~/ckeditor_v4_7/")
            'Write that JSON To txt file,  
            System.IO.File.WriteAllText(path + "browserImageList_team.json", JSONString)

            'add images datatable to listview
            If result.isValid Then
                'Add list to Image Library
                lstviewImageLibrary.DataSource = result.DataContainer(0)
                lstviewImageLibrary.DataBind()
                'Add list to Image Cropper library
                lstViewImageCroppers.DataSource = result.DataContainer(0)
                lstViewImageCroppers.DataBind()
                '    Response.Write("obtained Campaign Images")
                'Else
                '    Response.Write("error obtainCampaignImages: " & result.ExceptionMessage)


            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\TeamImageMngr.ascx.vb : obtainCampaignImages()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Response.Write(ex.ToString)
        End Try
    End Sub
#End Region
End Class