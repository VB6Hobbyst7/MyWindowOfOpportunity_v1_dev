Imports Common
Imports Newtonsoft.Json.Linq
Imports umbraco.NodeFactory
Imports umbraco.Web

Partial Class UserControls_ImageCropper
    Inherits System.Web.UI.UserControl


#Region "Properties"
    Public Property mediaId() As Integer
        Get
            Return CInt(hfldMediaId.Value)
        End Get
        Set(value As Integer)
            hfldMediaId.Value = value.ToString
        End Set
    End Property
    Public Property cropTypes() As String
        Get
            Return hfldcropTypes.Value
        End Get
        Set(value As String)
            hfldcropTypes.Value = value
        End Set
    End Property

    Private blMedia As blMedia
    Private Const _padding As Integer = 0 ' 20
#End Region

#Region "Handles"
    Private Sub UserControls_ImageCropper_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        Try
            'If IsPostBack Then
            'Response.Write("PreRender<br />")
            LoadPageValues()
            'End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\ImageCropper.ascx.vb : UserControls_ImageCropper_PreRender()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
    End Sub

    'Private Sub UserControls_ImageCropper_Load(sender As Object, e As EventArgs) Handles Me.Load
    '    Try
    '        If Not IsPostBack Then
    '            'Response.Write("Load<br />")
    '            '
    '            LoadPageValues()
    '        End If
    '    Catch ex As Exception
    '        Response.Write("<br />Error: " & ex.ToString & " MediaID: " & mediaId & "<br /><br />")
    '    End Try
    'End Sub
    Private Sub lbtnSave_Click(sender As Object, e As EventArgs) Handles lbtnSave.Click
        'Response.Write("start<br />")
        Try
            'Instantiate variables
            blMedia = New blMedia
            'Dim fromDb As String = hfldFromDb.Value
            Dim mjson As MediaJsonProperties = New MediaJsonProperties
            Dim Crop As Crop
            Dim Coordinates As Coordinates
            Dim FocalPoint As FocalPoint = New FocalPoint
            Dim Src As String = hfldSrc.Value
            'Dim sb As StringBuilder = New StringBuilder 'TEMP: obtain all data from each crop
            Dim mediaId As Integer = CInt(hfldMediaId.Value)


            'sb.AppendLine("MediaId: " & mediaId & "<br />")
            'Loop thru each item in the listview
            For Each item As ListViewItem In lstviewImageCropPanels.Items
                'Find usercontrol within listitem
                Dim uc As UserControls_ImageCropPanel = DirectCast(item.FindControl("ImageCropPanel"), UserControls_ImageCropPanel)

                'Instantiate variables
                Dim cropAlias As String = uc.cropAlias
                Dim viewportWidth As Integer = CInt(uc.viewportWidth)
                Dim viewportHeight As Integer = CInt(uc.viewportHeight)
                Dim cssLeft As Integer = CInt(uc.cssLeft)
                Dim cssTop As Integer = CInt(uc.cssTop)
                Dim imageWidth As Double = CDbl(uc.imageWidth)
                Dim imageHeight As Double = CDbl(uc.imageHeight)
                Dim cropWidth As Integer = uc.cropWidth
                Dim cropHeight As Integer = uc.cropHeight
                Dim X1 As Double = CDbl(uc.X1)
                Dim Y1 As Double = CDbl(uc.Y1)
                Dim X2 As Double = CDbl(uc.X2)
                Dim Y2 As Double = CDbl(uc.Y2)
                Crop = New Crop
                Coordinates = New Coordinates



                ''DISPLAY AS TEXT FOR TESTING ONLY..........................
                ''Get from control ImageCropperPanel.ascx
                'sb.AppendLine("<br />")
                'sb.AppendLine("crop alias: " & cropAlias & "<br />")
                'sb.AppendLine("----------Style from Browser------------<br />")
                'sb.AppendLine("viewport width: " & viewportWidth & "<br />")
                'sb.AppendLine("viewport height: " & viewportHeight & "<br />")
                'sb.AppendLine("left: " & cssLeft & "<br />")
                'sb.AppendLine("top: " & cssTop & "<br />")

                If (imageWidth = 0) Or (imageHeight = 0) Then
                    Dim umbracoWidth As Integer = hfldImageWidth.Value
                    Dim umbracoHeight As Integer = hfldImageHeight.Value
                    imageHeight = (umbracoWidth / umbracoHeight) * viewportWidth
                    '    sb.AppendLine("image width: " & viewportWidth & "<br />")
                    '    sb.AppendLine("image height: " & imageHeight & "<br />")
                    'Else
                    '    sb.AppendLine("image width: " & imageWidth & "<br />")
                    '    sb.AppendLine("image height: " & imageHeight & "<br />")
                End If

                'sb.AppendLine("----------Values to Database------------<br />")
                'sb.AppendLine("crop width: " & cropWidth & "<br />")
                'sb.AppendLine("crop height: " & cropHeight & "<br />")

                If (X1 = 0F) AndAlso (Y1 = 0F) AndAlso (X2 = 0F) AndAlso (Y2 = 0F) Then
                    X1 = toDatabase_X1(cssLeft, imageWidth)
                    Y1 = toDatabase_Y1(cssTop, imageHeight)
                    X2 = toDatabase_X2(cssLeft, imageWidth, viewportWidth)
                    Y2 = toDatabase_Y2(cssTop, imageHeight, viewportHeight)
                    '(imgHeight - viewportHeight + cssTop - _padding) / imgHeight

                    If Double.IsInfinity(X1) Then X1 = 0F
                    If Double.IsInfinity(Y1) Then Y1 = 0F
                    If Double.IsInfinity(X2) Then X2 = 0F
                    If Double.IsInfinity(Y2) Then Y2 = 0F

                    '    sb.AppendLine("X1: " & X1.ToString & "<br />")
                    '    sb.AppendLine("Y1: " & Y1.ToString & "<br />")
                    '    sb.AppendLine("X2: " & X2.ToString & "<br />")
                    '    sb.AppendLine("Y2: " & Y2.ToString & "<br />")
                    'Else
                    '    sb.AppendLine("X1: " & X1.ToString & "<br />")
                    '    sb.AppendLine("Y1: " & Y1.ToString & "<br />")
                    '    sb.AppendLine("X2: " & X2.ToString & "<br />")
                    '    sb.AppendLine("Y2: " & Y2.ToString & "<br />")
                End If

                'sb.AppendLine("<br />")

                '.....................................................................


                Coordinates.x1 = X1
                Coordinates.y1 = Y1
                Coordinates.x2 = X2
                Coordinates.y2 = Y2

                Crop.width = cropWidth
                Crop.height = cropHeight
                Crop.alias = cropAlias
                Crop.coordinates = Coordinates



                'add crop class to list of crops
                mjson.crops.Add(Crop)
            Next

            mjson.focalPoint = FocalPoint

            'Remove any unneeded parameters in image source
            If Src.Contains("?") Then
                Dim i As Int16 = Src.IndexOf("?")
                Src = Src.Substring(0, i)
            End If
            mjson.src = Src


            'Convert class to json
            Dim toDb As String = Newtonsoft.Json.JsonConvert.SerializeObject(mjson)
            'Response.Write(toDb)

            Dim businessResponse As BusinessReturn = blMedia.updateMediaPropertyData_byId(mediaId, toDb)

            'Set active tab
            If UmbracoContext.Current.PageId = siteNodes.EditAccount Then
                setTabCookie(tabNames.profile)
            End If

            Response.Redirect(Request.Url.ToString, False)
            HttpContext.Current.ApplicationInstance.CompleteRequest()

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\ImageCropper.ascx.vb : lbtnSave_Click()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Response.Write(ex.ToString)
        End Try
    End Sub
#End Region

#Region "Methods"
    Private Function toDatabase_X1(ByVal cssLeft As Integer, ByVal imageWidth As Double) As Double?
        'JS:    Return ((_padding - _left) / _width);
        Return (_padding - cssLeft) / imageWidth
    End Function
    Private Function toDatabase_Y1(ByVal cssTop As Integer, ByVal imgHeight As Double) As Double?
        'JS:    Return ((_padding - _top) / _height);
        Return (_padding - cssTop) / imgHeight
    End Function
    Private Function toDatabase_X2(ByVal cssLeft As Integer, ByVal imageWidth As Double, ByVal viewportWidth As Integer) As Double?
        'JS:    Return ((_imgWidth - _viewportWidth + _left - _padding) / _imgWidth);
        Return (imageWidth - viewportWidth + cssLeft - _padding) / imageWidth
    End Function
    Private Function toDatabase_Y2(ByVal cssTop As Integer, ByVal imgHeight As Double, ByVal viewportHeight As Integer) As Double?
        'JS:    Return ((_imgHeight - _viewportHeight + _top - _padding) / _imgHeight);
        Return (imgHeight - viewportHeight + cssTop - _padding) / imgHeight
    End Function

    Private Sub LoadPageValues()
        Try

            'Instantiate variables
            blMedia = New blMedia
            Dim businessReturn As BusinessReturn
            Dim mediaProperties As MediaProperties
            Dim lstImageCropperClass As List(Of ImageCropperClass) = New List(Of ImageCropperClass)
            Dim NodeTypeAlias As String = Node.GetCurrent.NodeTypeAlias

            'Add mediaId as attribute
            pnlImageCropper.Attributes.Add("mediaId", mediaId)

            'Obtain media properties
            businessReturn = blMedia.selectMediaData_byId(mediaId)
            mediaProperties = businessReturn.DataContainer(0)

            '========================================================================

            'lblFromDb.Text = mediaProperties.media.Properties("umbracoFile").Value.ToString
            hfldFromDb.Value = mediaProperties.media.Properties("umbracoFile").Value.ToString

            ''JSON: IMPORT INTO CLASS
            'Dim mjson As MediaJsonProperties = New MediaJsonProperties
            'Newtonsoft.Json.JsonConvert.PopulateObject(mediaProperties.media.Properties("umbracoFile").Value.ToString, mjson)


            ''JSON: EXPORT CLASS TO JSON
            'mjson.crops.Clear()
            'Dim newCrop As Crop = New Crop
            'newCrop.alias = ""
            'mjson.crops.Add(newCrop)
            'lblToDb.Text = Newtonsoft.Json.JsonConvert.SerializeObject(mjson)
            '========================================================================

            'Assign image url to main image control
            imgMain.ImageUrl = getMediaURL(mediaId)
            imgMain.AlternateText = mediaProperties.media.Name
            imgMain.Attributes.Add("title", mediaProperties.media.Name)

            'Add basic information about image for user
            lblWidth.Text = mediaProperties.umbracoWidth
            lblHeight.Text = mediaProperties.umbracoHeight
            lblSize.Text = Decimal.Round((CDec(mediaProperties.umbracoBytes) / 1024), 2, MidpointRounding.AwayFromZero)
            lblType.Text = mediaProperties.umbracoExtension

            'Assign hidden field values.
            hfldSrc.Value = getMediaURL(mediaId)
            hfldImageWidth.Value = mediaProperties.umbracoHeight
            hfldImageHeight.Value = mediaProperties.umbracoWidth

            'Create crop buttons via json values
            Dim jsonObject As JObject = JObject.Parse(cropTypes)
            Dim data As List(Of JToken) = jsonObject.Children().ToList
            For Each item As JProperty In data
                item.CreateReader()
                Select Case item.Name
                    Case "crops"
                        For Each msg As JObject In item.Values
                            Select Case NodeTypeAlias
                                Case docTypes.editCampaign
                                    Select Case msg("alias").ToString
                                        Case Crops.campaignFeaturedImage, Crops.rewardImage, Crops.campaignSummaryImage
                                            'Create new button
                                            Dim ImageCropperClass As ImageCropperClass = New ImageCropperClass
                                            'Assign values to button
                                            ImageCropperClass.cropAlias = msg("alias")
                                            ImageCropperClass.cropWidth = msg("width").ToString
                                            ImageCropperClass.cropHeight = msg("height").ToString
                                            ImageCropperClass.mediaProperties = mediaProperties

                                            'Add button to list
                                            lstImageCropperClass.Add(ImageCropperClass)
                                        Case Else
                                            'skip
                                    End Select

                                Case docTypes.Team
                                    Select Case msg("alias").ToString
                                        Case Crops.members
                                            'Create new button
                                            Dim ImageCropperClass As ImageCropperClass = New ImageCropperClass
                                            'Assign values to button
                                            ImageCropperClass.cropAlias = msg("alias")
                                            ImageCropperClass.cropWidth = msg("width").ToString
                                            ImageCropperClass.cropHeight = msg("height").ToString
                                            ImageCropperClass.mediaProperties = mediaProperties

                                            'Add button to list
                                            lstImageCropperClass.Add(ImageCropperClass)
                                        Case Else
                                            'skip
                                    End Select

                                Case docTypes.editAccount
                                    Select Case msg("alias").ToString
                                        Case Crops.members
                                            'Create new button
                                            Dim ImageCropperClass As ImageCropperClass = New ImageCropperClass
                                            'Assign values to button
                                            ImageCropperClass.cropAlias = msg("alias")
                                            ImageCropperClass.cropWidth = msg("width").ToString
                                            ImageCropperClass.cropHeight = msg("height").ToString
                                            ImageCropperClass.mediaProperties = mediaProperties

                                            'Add button to list
                                            lstImageCropperClass.Add(ImageCropperClass)
                                        Case Else
                                            'skip
                                    End Select
                                Case Else
                                    'skip
                            End Select

                        Next
                End Select
            Next

            'Add list to listviews
            lstviewImageCropperButtons.DataSource = lstImageCropperClass
            lstviewImageCropperButtons.DataBind()
            lstviewImageCropPanels.DataSource = lstImageCropperClass
            lstviewImageCropPanels.DataBind()
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\ImageCropper.ascx.vb : LoadPageValues()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
    End Sub

#End Region
End Class

Public Class ImageCropperClass
    Public Property mediaProperties As MediaProperties
    Public Property cropAlias As String
    Public Property cropWidth As String
    Public Property cropHeight As String

    Public Property ViewportWidth As String
    Public Property ViewportHeight As String
    Public Property Left As String
    Public Property Top As String
    Public Property ImgWidth As String
    Public Property ImgHeight As String

    Public Property X1 As String
    Public Property Y1 As String
    Public Property X2 As String
    Public Property Y2 As String

    Public Sub New()
    End Sub
End Class