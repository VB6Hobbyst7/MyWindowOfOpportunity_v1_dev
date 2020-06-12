Imports umbraco
Imports Common
Imports Newtonsoft.Json
Imports umbraco.Core.Models
Imports umbraco.Web

Partial Class UserControls_editTab_Images
    Inherits System.Web.UI.UserControl


#Region "Properties"
    Private blMedia As blMedia
    Public Property thisNodeId() As String
        Get
            Return hfldThisNodeId.Value
        End Get
        Set(value As String)
            hfldThisNodeId.Value = value.ToString
        End Set
    End Property
    Private _uHelper As New Uhelper()
    Public Property campaignComplete As Boolean = False
#End Region

#Region "Handles"
    Private Sub UserControls_editTab_Images_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not String.IsNullOrEmpty(thisNodeId) Then
            Try
                If Not IsPostBack Then

                    'Instantiate variables
                    Dim thisNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(CInt(thisNodeId))

                    'Set hidden field with tab name
                    hfldTabName.Value = tabNames.images

                    '
                    If thisNode.HasProperty(nodeProperties.mediaFolder) Then hfldMediaFolder.Value = thisNode.GetPropertyValue(Of Integer)(nodeProperties.mediaFolder)

                    'Obtain all campaign images
                    obtainCampaignImages()
                End If
            Catch ex As Exception
                Dim sb As New StringBuilder()
                sb.AppendLine("\UserControls\editTab_Images.ascx.vb : UserControls_editTab_Images_Load()")
                saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
                'Response.Write("Error: " & ex.ToString)
            End Try
        End If
    End Sub
    Private Sub btnDeleteImage_Click(sender As Object, e As EventArgs) Handles btnDeleteImage.Click
        Try
            blMedia = New blMedia
            Dim mediaId As Integer = CInt(hfldMediaIdToDelete.Value)

            Dim result As BusinessReturn = blMedia.deleteMedia_byId(mediaId)

            If result.isValid Then
                'Show success msg


                'Refresh images in list.
                obtainCampaignImages()
            Else
                'Show error msg
                'Response.Write(result.ExceptionMessage)
            End If

            'Response.Write("clicked: " & hfld.Value)
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\editTab_Images.ascx.vb : btnDeleteImage_Click()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Response.Write(ex.ToString)
        End Try
    End Sub
    Private Sub btnRefreshPage_Click(sender As Object, e As EventArgs) Handles btnRefreshPage.Click
        'Obtain all campaign images
        'obtainCampaignImages()

        'Refresh Page
        Response.Redirect(Request.Url.PathAndQuery, False)
        HttpContext.Current.ApplicationInstance.CompleteRequest()
    End Sub
#End Region

#Region "Methods"
    Private Sub obtainCampaignImages()
        Try  'Instantiate variables
            blMedia = New blMedia
            Dim thisNode = _uHelper.Get_IPublishedContentByID(thisNodeId)

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
            System.IO.File.WriteAllText(path + "browserImageList_campaign.json", JSONString)

            'add images datatable to listview
            If result.isValid Then
                'Add list to Image Library
                lstviewImageLibrary.DataSource = result.DataContainer(0)
                lstviewImageLibrary.DataBind()
                'Add list to Image Cropper library
                lstViewImageCroppers.DataSource = result.DataContainer(0)
                lstViewImageCroppers.DataBind()



                'gv.DataSource = result.DataContainer(0)
                'gv.DataBind()


            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\editTab_Images.ascx.vb : obtainCampaignImages()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
    End Sub
#End Region
End Class