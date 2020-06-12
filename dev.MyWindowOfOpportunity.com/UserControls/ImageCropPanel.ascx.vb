Imports Common


Partial Class UserControls_ImageCropPanel
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
    Public Property cropWidth() As Integer
        Get
            Return hfldCropWidthy.Value
        End Get
        Set(value As Integer)
            hfldCropWidthy.Value = value
        End Set
    End Property
    Public Property cropHeight() As Integer
        Get
            Return hfldCropHeight.Value
        End Get
        Set(value As Integer)
            hfldCropHeight.Value = value
        End Set
    End Property
    Public Property cssLeft() As String
        Get
            Return adjustForZero(hfldLeft.Value.Replace("px", ""))
        End Get
        Set(value As String)
            hfldLeft.Value = adjustForZero(value)
        End Set
    End Property
    Public Property cssTop() As String
        Get
            Return adjustForZero(hfldTop.Value.Replace("px", ""))
        End Get
        Set(value As String)
            hfldTop.Value = adjustForZero(value)
        End Set
    End Property
    Public Property imageWidth() As String
        Get
            Return hfldImgWidth.Value.Replace("px", "")
        End Get
        Set(value As String)
            hfldImgWidth.Value = value
        End Set
    End Property
    Public Property imageHeight() As String
        Get
            Return hfldImgHeight.Value.Replace("px", "")
        End Get
        Set(value As String)
            hfldImgHeight.Value = value
        End Set
    End Property
    Public Property viewportWidth() As String
        Get
            'Dim _viewportWidth As String = pnlViewport.Width.ToString.Replace("px", "").Trim
            'If String.IsNullOrEmpty(_viewportWidth) Then _viewportWidth = 0
            'Return _viewportWidth
            Return pnlViewport.Width.ToString.Replace("px", "").Trim
        End Get
        Set(value As String)
            hfldViewportWidth.Value = value
        End Set
    End Property
    Public Property viewportHeight() As String
        Get
            'Dim _viewportHeight As String = pnlViewport.Height.ToString.Replace("px", "").Trim
            'If String.IsNullOrEmpty(_viewportHeight) Then _viewportHeight = 0
            'Return _viewportHeight
            Return pnlViewport.Height.ToString.Replace("px", "").Trim
        End Get
        Set(value As String)
            hfldViewportHeight.Value = value
        End Set
    End Property
    Public Property X1() As String
        Get
            Return adjustForZero(hfldX1.Value)
        End Get
        Set(value As String)
            hfldX1.Value = adjustForZero(value)
        End Set
    End Property
    Public Property Y1() As String
        Get
            Return adjustForZero(hfldY1.Value)
        End Get
        Set(value As String)
            hfldY1.Value = adjustForZero(value)
        End Set
    End Property
    Public Property X2() As String
        Get
            Return adjustForZero(hfldX2.Value)
        End Get
        Set(value As String)
            hfldX2.Value = adjustForZero(value)
        End Set
    End Property
    Public Property Y2() As String
        Get
            Return adjustForZero(hfldY2.Value)
        End Get
        Set(value As String)
            hfldY2.Value = adjustForZero(value)
        End Set
    End Property

#End Region

#Region "Handles"
    Private Sub UserControls_ImageCropPanel_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        Try
            '
            pnlCrop.Attributes.Add("mediaId", mediaProperties.media.Id)
            pnlCrop.Attributes.Add("cropAlias", cropAlias.Replace(" ", ""))
            'Response.Write(cropAlias)
            pnlCrop.Attributes.Add("cropWidth", cropWidth())
            pnlCrop.Attributes.Add("cropHeight", cropHeight())




            'Obtain image url with crop parameters and split.
            'imgCropped.ImageUrl = getMediaURL(mediaProperties.media.Id, crop)
            'Dim cropImgUrl As String = getMediaURL(1479, crop)
            Dim cropImgUrl As String = getMediaURL(mediaProperties.media.Id, cropAlias)
            Dim imgUrlParts As NameValueCollection = HttpUtility.ParseQueryString(cropImgUrl)

            'Obtain crop dimensions
            Dim width As Integer = CInt(imgUrlParts("width"))
            Dim height As Integer = CInt(imgUrlParts("height"))

            'Set adjusted width/height to viewport
            Dim vpWidth As Integer = Integer.Parse(Regex.Replace(pnlViewport.Width.ToString, "[^0-9]", ""))
            Dim vpHeight As Integer = (vpWidth * height) / width
            pnlViewport.Height = vpHeight

            'Set image
            imgCropped.ImageUrl = getMediaURL(mediaProperties.media.Id)
            imgCropped.AlternateText = mediaProperties.media.Name
            imgCropped.Attributes.Add("title", mediaProperties.media.Name)



            'Dim sb As StringBuilder = New StringBuilder


            'Import json into class
            Dim mjson As MediaJsonProperties = New MediaJsonProperties
            Newtonsoft.Json.JsonConvert.PopulateObject(mediaProperties.media.Properties("umbracoFile").Value.ToString, mjson)
            'Loop thru crops to find matching alias
            For Each crop As Crop In mjson.crops
                If crop.alias = cropAlias Then
                    '
                    If Not IsNothing(crop) Then
                        If Not IsNothing(crop.coordinates) Then
                            If Not IsNothing(crop.coordinates.x1) Then X1 = crop.coordinates.x1.ToString
                            If Not IsNothing(crop.coordinates.y1) Then Y1 = crop.coordinates.y1.ToString
                            If Not IsNothing(crop.coordinates.x2) Then X2 = crop.coordinates.x2.ToString
                            If Not IsNothing(crop.coordinates.y2) Then Y2 = crop.coordinates.y2.ToString
                        End If
                    End If

                    'sb.AppendLine("X1: " & X1 & "<br />")
                    'sb.AppendLine("Y1: " & Y1 & "<br />")
                    'sb.AppendLine("X2: " & X2 & "<br />")
                    'sb.AppendLine("Y2: " & Y2 & "<br />")
                    Exit For
                End If
            Next


            'ltrlFromDb.Text = sb.ToString

            'Response.Write("<br />" & cropAlias & "<br />=============<br />")


        Catch ex As Exception
            Dim sb As New StringBuilder
            sb.AppendLine("\UserControls\ImageCropPanel.ascx.vb : UserControls_ImageCropPanel_PreRender()")
            sb.AppendLine("[cropAlias] " & cropAlias)
            sb.AppendLine("[cropWidth] " & cropWidth)
            sb.AppendLine("[cropHeight] " & cropHeight)
            sb.AppendLine("[cssLeft] " & cssLeft)
            sb.AppendLine("[cssTop] " & cssTop)
            sb.AppendLine("[imageWidth] " & imageWidth)
            sb.AppendLine("[imageHeight] " & imageHeight)
            sb.AppendLine("[viewportWidth] " & viewportWidth)
            sb.AppendLine("[viewportHeight] " & viewportHeight)
            sb.AppendLine("[X1] " & X1)
            sb.AppendLine("[Y1] " & Y1)
            sb.AppendLine("[X2] " & X2)
            sb.AppendLine("[Y2] " & Y2)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString)
        End Try
    End Sub
#End Region

#Region "Methods"
    Private Function adjustForZero(ByVal val As String) As String
        '
        Const correctedVal As String = "0.000001"

        Select Case val
            Case String.Empty, "0", "0.0", "0.00"
                Return correctedVal
            Case Else
                Return val
        End Select
    End Function
#End Region

End Class
