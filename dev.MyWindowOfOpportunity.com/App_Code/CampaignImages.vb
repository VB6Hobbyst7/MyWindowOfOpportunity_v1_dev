Imports System.Web.Services
Imports System.Web.Script.Services




'<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<System.Web.Script.Services.ScriptService()>
<WebService(Namespace:="http://127.0.0.1/Services/CampaignImages.asmx")>
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<System.ComponentModel.ToolboxItem(False)>
Public Class CampaignImages
    Inherits System.Web.Services.WebService


    '<WebMethod>
    'Public Function HelloWorldInJson() As String
    '    Return JsonHelper.ToJson("Hello World in Json")
    'End Function



    '<System.Web.Script.Services.ScriptMethod(ResponseFormat:=System.Web.Script.Services.ResponseFormat.Json)>
    '<ScriptMethod(UseHttpGet:=True)>
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=True, XmlSerializeString:=False)>
    Public Function getImages() As String '(ByVal mediaFolderId As Integer)
        '
        Dim ImageList As List(Of ImageItem) = New List(Of ImageItem)
        Dim ImageItem As ImageItem = New ImageItem
        ImageItem.image = "http://localhost/media/1003/20150103_224243.jpg"
        ImageItem.thumb = "http://localhost/media/1003/20150103_224243_thumb.jpg"
        ImageList.Add(ImageItem)
        ImageItem = New ImageItem
        ImageItem.image = "http://localhost/media/1003/20150103_224243.jpg"
        ImageItem.thumb = "http://localhost/media/1003/20150103_224243_thumb.jpg"
        ImageList.Add(ImageItem)


        'Dim obj As String = Newtonsoft.Json.JsonConvert.SerializeObject(ImageList)

        Return JsonHelper.ToJson(ImageList)

        'Me.Context.Response.ContentType = "application/json; charset=utf-8"
        'Me.Context.Response.Write(JsonHelper.ToJson(ImageList))

        'Dim ser As JavaScriptSerializer = New JavaScriptSerializer()
        'Return ser.Serialize(ImageList)

        'Me.Context.Response.ContentType = "application/json; charset=utf-8"
        'Me.Context.Response.Write(JsonHelper.ToJson(ImageList))

        'Return ImageList

    End Function




    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=True, XmlSerializeString:=False)>
    Public Sub getImages2() 'As String '(ByVal mediaFolderId As Integer)
        '
        Dim ImageList As List(Of ImageItem) = New List(Of ImageItem)
        Dim ImageItem As ImageItem = New ImageItem
        ImageItem.image = "http://127.0.0.1/media/1003/20150103_224243.jpg"
        ImageItem.thumb = "http://127.0.0.1/media/1003/20150103_224243_thumb.jpg"
        ImageList.Add(ImageItem)
        ImageItem = New ImageItem
        ImageItem.image = "http://127.0.0.1/media/1003/20150103_224243.jpg"
        ImageItem.thumb = "http://127.0.0.1/media/1003/20150103_224243_thumb.jpg"
        ImageList.Add(ImageItem)


        'Dim obj As String = Newtonsoft.Json.JsonConvert.SerializeObject(ImageList)

        'Return JsonHelper.ToJson(ImageList)

        'Me.Context.Response.ContentType = "application/json; charset=utf-8"
        'Me.Context.Response.Write(JsonHelper.ToJson(ImageList))

        'Dim ser As JavaScriptSerializer = New JavaScriptSerializer()
        'Return ser.Serialize(ImageList)

        Me.Context.Response.ContentType = "application/json; charset=utf-8"
        Me.Context.Response.Write(JsonHelper.ToJson(ImageList))

        'Return ImageList

    End Sub
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


'[
'  {
'    "image": "http://localhost/media/1003/20150103_224243.jpg",
'    "thumb": "http://localhost/media/1003/20150103_224243_thumb.jpg"
'  },
'  {
'    "image": "http://localhost/media/1003/20150103_224243.jpg",
'    "thumb": "http://localhost/media/1003/20150103_224243_thumb.jpg"
'  }
']