<%@ WebHandler Language="VB" Class="CampaignImages" %>

Imports Common
Imports System.Data


Public Class CampaignImages : Implements IHttpHandler
#Region "Properties"
    Private Structure imgList
        Const mediaId As String = "mediaId"
        Const imgUrl As String = "imgUrl"
        Const imgName As String = "imgName"
        Const imageThumb As String = "imageThumb"
    End Structure
#End Region

#Region "Handles"

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Try
            'Instantiate variables
            Dim blMedia As blMedia = New blMedia
            Dim BusinessReturn As BusinessReturn
            Dim thisNodeId As Integer = CInt(context.Request.QueryString("nodeId"))

            'Obtain image thumbs.
            BusinessReturn = blMedia.selectImageThumbs_byCampaignId(thisNodeId)

            'Convert page to a json response.
            context.Response.Clear()
            context.Response.ContentType = "application/json; charset=utf-8"
            context.Response.Write(BusinessReturn.ReturnMessage)
            'context.Response.Write(JsonHelper.ToJson(BusinessReturn.ReturnMessage))
            context.Response.End()
            'End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\Handlers\CampaignImages.ashx : ProcessRequest()")
            sb.AppendLine("context:" & context.ToString())

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try

    End Sub

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property


#End Region


#Region "Methods"
    Public Function createTable_ImageList() As DataTable
        'Instantiate variables
        Dim dt As DataTable = New DataTable
        'Add Columns
        dt.Columns.Add(imgList.mediaId, GetType(Integer))
        dt.Columns.Add(imgList.imgUrl, GetType(String))
        dt.Columns.Add(imgList.imgName, GetType(String))
        dt.Columns.Add(imgList.imageThumb, GetType(String))
        Return dt
    End Function
#End Region
End Class



Public Class ImageItem
    Public Property image As String
    Public Property thumb As String
End Class


Public Class JsonHelper
    ''' <summary>
    ''' Convert Object to Json String
    ''' </summary>
    ''' <param name="obj">The object to convert</param>
    ''' <returns>Json representation of the Object in string</returns>
    Public Shared Function ToJson(obj As Object) As String
        Return Newtonsoft.Json.JsonConvert.SerializeObject(obj)
    End Function
End Class
