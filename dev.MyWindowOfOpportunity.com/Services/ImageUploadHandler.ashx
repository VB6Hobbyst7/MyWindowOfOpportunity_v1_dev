<%@ WebHandler Language="VB" Class="ImageUploadHandler" %>


Imports umbraco.Core
Imports umbraco.Core.Models
Imports umbraco.Core.Services
Imports System.Drawing
Imports System.IO
Imports System.Text
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports Common


Public Class ImageUploadHandler : Implements IHttpHandler

#Region "Methods"
    Private Function ResizeImg(ByVal oldImg As Bitmap) As Bitmap
        'Instantiate variables
        Dim minWidth As Integer = 400 '1600
        Dim minHeight As Integer = 550
        Dim newImg As Bitmap

        'Determine the needed scale and adjust new width/height of image
        Dim scale As Single = Math.Max(minWidth / oldImg.Width, minHeight / oldImg.Height)
        Dim scaledWidth = CInt((oldImg.Width * scale))
        Dim scaledHeight = CInt((oldImg.Height * scale))

        'Create new bitmap using the scaled sizes.
        newImg = New Bitmap(scaledWidth, scaledHeight)
        newImg.SetResolution(oldImg.HorizontalResolution, oldImg.VerticalResolution) 'maintains DPI regardless of physical size -- may increase quality when reducing image dimensions or when printing

        'Generate new image
        Using graphic As Graphics = Graphics.FromImage(CType(newImg, System.Drawing.Image))
            graphic.CompositingMode = CompositingMode.SourceCopy 'determines whether pixels from a source image overwrite or are combined with background pixels. SourceCopy specifies that when a color is rendered, it overwrites the background color.
            graphic.CompositingQuality = CompositingQuality.HighQuality 'determines the rendering quality level of layered images.
            graphic.InterpolationMode = InterpolationMode.HighQualityBicubic 'determines how intermediate values between two endpoints are calculated
            graphic.SmoothingMode = SmoothingMode.HighQuality 'specifies whether lines, curves, and the edges of filled areas use smoothing (also called antialiasing) -- probably only works on vectors
            graphic.PixelOffsetMode = PixelOffsetMode.HighQuality 'affects rendering quality when drawing the new image
            graphic.DrawImage(oldImg, 0, 0, scaledWidth, scaledHeight)
        End Using

        Return newImg
    End Function


    Public Shared Function ImageToStream(ByVal img As Image) As MemoryStream
        'Converts an image into a memory stream
        Dim str = New MemoryStream
        img.Save(str, ImageFormat.Png)
        str.Seek(0, SeekOrigin.Begin)
        Return str
    End Function
#End Region


#Region "Handle"
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Try
            If ApplicationContext.Current IsNot Nothing Then
                If context.Request.QueryString(queryParameters.mediaId) IsNot Nothing Then
                    'Instantiate variables
                    Dim pathrefer As String = context.Request.UrlReferrer.ToString()
                    Dim Serverpath As String = HttpContext.Current.Server.MapPath("Upload")
                    Dim mediaId As Integer = CInt(context.Request.QueryString(queryParameters.mediaId))
                    Dim fileName As String = String.Empty
                    Dim postedFile As HttpPostedFile = context.Request.Files(0)

                    'Adjust image's size to a minimum to match crops.
                    Dim newImg As System.Drawing.Image = System.Drawing.Image.FromStream(postedFile.InputStream)
                    newImg = ResizeImg(CType(newImg, Bitmap))

                    'In case of IE
                    If HttpContext.Current.Request.Browser.Browser.ToUpper() = "IE" Then
                        Dim files As String() = postedFile.FileName.Split(New Char() {"\"c})
                        fileName = files(files.Length - 1)
                    Else
                        ' In case of other browsers
                        fileName = postedFile.FileName
                    End If
                    'fileName = "testImg"

                    'Instantiate variables for media services
                    Dim mediaService = ApplicationContext.Current.Services.MediaService
                    Dim memberPhoto As IMedia
                    Dim currentUserId As Integer

                    'If a current user id is included, replace filename with member id as name.
                    If context.Request.QueryString(queryParameters.currentUserId) IsNot Nothing AndAlso
                            Integer.TryParse(context.Request.QueryString(queryParameters.currentUserId), currentUserId) Then

                        'Instantiate variables
                        Dim blMembers As blMembers = New blMembers
                        Dim photoId As Integer = blMembers.getMemberPhotoNodeId_byId(currentUserId)

                        'Check if member has photo
                        If photoId > 0 Then
                            'Member has existing image.  Retrieve
                            memberPhoto = mediaService.GetById(photoId)
                            'mediaService.Delete(mediaService.GetById(photoId))
                        Else
                            'Member does not have an image.  Create a new one
                            memberPhoto = mediaService.CreateMedia(currentUserId, mediaId, Constants.Conventions.MediaTypes.Image)
                        End If

                        'Set streamed image to mediaservice
                        'memberPhoto.SetValue(Global.umbraco.Core.Constants.Conventions.Media.File, fileName, postedFile.InputStream)
                        memberPhoto.SetValue(Global.umbraco.Core.Constants.Conventions.Media.File, fileName, ImageToStream(newImg))

                        'Save the new Media object
                        mediaService.Save(memberPhoto)

                        'If photo does not exist for user, add new imedia id to member as photo.
                        If photoId = 0 Then
                            Dim businessReturn As BusinessReturn = blMembers.updatePhoto(currentUserId, memberPhoto.Id)
                        End If

                    Else
                        'Use the MediaService to create a new Media object 
                        memberPhoto = mediaService.CreateMedia(fileName, mediaId, Constants.Conventions.MediaTypes.Image)

                        'Set streamed image to mediaservice
                        'memberPhoto.SetValue(Global.umbraco.Core.Constants.Conventions.Media.File, fileName, postedFile.InputStream)
                        memberPhoto.SetValue(Global.umbraco.Core.Constants.Conventions.Media.File, fileName, ImageToStream(newImg))

                        'Save the new Media object
                        mediaService.Save(memberPhoto)
                    End If
                End If

                context.Response.AddHeader("Vary", "Accept")
                Try
                    If context.Request("HTTP_ACCEPT").Contains("application/json") Then
                        context.Response.ContentType = "application/json"
                    Else
                        context.Response.ContentType = "text/plain"
                    End If
                Catch ex As Exception
                    Dim sb As New StringBuilder()
                    sb.AppendLine("\Handlers\ImageUploadHandler.ashx : ProcessRequest() | HTTP_ACCEPT")
                    sb.AppendLine("context:" & context.ToString())
                    sb.AppendLine("QueryString: " & context.Request.QueryString.ToString())

                    saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
                    context.Response.ContentType = "text/plain"
                End Try
                context.Response.Write("Success")
            End If
        Catch exp As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\Handlers\ImageUploadHandler.ashx : ProcessRequest()")
            sb.AppendLine("context:" & context.ToString())
            sb.AppendLine("QueryString: " & context.Request.QueryString.ToString())

            saveErrorMessage(getLoggedInMember, exp.Message & vbNewLine & exp.StackTrace & vbNewLine & exp.Source, sb.ToString())
            context.Response.Write("Error in Handler: " & exp.Message)
        End Try
    End Sub
#End Region



    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class