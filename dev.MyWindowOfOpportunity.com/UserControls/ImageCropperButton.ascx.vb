Imports Common

Partial Class UserControls_ImageCropperButton
    Inherits System.Web.UI.UserControl


#Region "Properties"
    Private _mediaProperties As MediaProperties
    Public Property mediaProperties() As MediaProperties
        Get
            Return _mediaProperties
        End Get
        Set(value As MediaProperties)
            _mediaProperties = value
        End Set
    End Property

    'Private _cropAlias As String = String.Empty
    Public Property cropAlias() As String
        Get
            'Return _cropAlias
            Return hfldCropAlias.Value
        End Get
        Set(value As String)
            '_cropAlias = value
            hfldCropAlias.Value = value
        End Set
    End Property

    'Private _cropWidth As Integer = 0
    Public Property cropWidth() As Integer
        Get
            'Return _cropWidth
            Return CInt(hfldCropWidth.Value)
        End Get
        Set(value As Integer)
            ' _cropWidth = value
            hfldCropWidth.Value = value.ToString
        End Set
    End Property

    'Private _cropHeight As Integer = 0
    Public Property cropHeight() As Integer
        Get
            'Return _cropHeight
            Return CInt(hfldCropHeight.Value)
        End Get
        Set(value As Integer)
            '_cropHeight = value
            hfldCropHeight.Value = value.ToString
        End Set
    End Property
#End Region

#Region "Handles"
    Private Sub UserControls_ImageCropperButton_Load(sender As Object, e As EventArgs) Handles Me.PreRender
        Try
            'If Not IsPostBack Then
            '
            li.Attributes.Add("cropAlias", cropAlias.Replace(" ", ""))
            li.Attributes.Add("cropWidth", cropWidth())
            li.Attributes.Add("cropHeight", cropHeight())

            imgCrop.ImageUrl = getMediaURL(mediaProperties.media.Id, cropAlias)
            imgCrop.AlternateText = mediaProperties.media.Name
            imgCrop.Attributes.Add("title", mediaProperties.media.Name)

            ltrlCropName.Text = cropAlias


            ltrlCropSize.Text = cropWidth & "px X " & cropHeight & "px"


            'Response.Write("<br />" & imgCrop.ImageUrl & "<br />")
            'End If

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\ImageCropperButton.ascx.vb : UserControls_ImageCropperButton_Load()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
    End Sub
#End Region

#Region "Methods"
#End Region
End Class
