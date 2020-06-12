Imports System.IO
Imports System.Web
Imports System.Web.Script.Serialization
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports Stripe.StripeResponse
Imports Stripe
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text




' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class StripeConnectWebhook
    Inherits System.Web.Services.WebService

    <WebMethod()>
    Public Function ReceiveData(ByVal context As HttpContext) As String
        Try



            'Try
            '    Dim js As JavaScriptSerializer = New JavaScriptSerializer()
            '    Dim sb As StringBuilder = New StringBuilder()
            '    Dim streamLength As Integer
            '    Dim streamRead As Integer
            '    Dim s As Stream = context.Request.InputStream
            '    streamLength = Convert.ToInt32(s.Length)
            '    sw.WriteLine(Length:=_, +streamLength)
            '    Dim streamArray As Byte() = New Byte(streamLength - 1) {}
            '    streamRead = s.Read(streamArray, 0, streamLength)

            '    For i As Integer = 0 To streamLength - 1
            '        sb.Append(Convert.ToChar(streamArray(i)))
            '    Next

            '    Dim raw As String = sb.ToString()
            '    sw.WriteLine(raw)
            '    Dim stripeResponse As StripeHookResponse = CType(js.Deserialize(raw, GetType(StripeHookResponse)), StripeHookResponse)
            '    context.Response.ContentType = "text/plain"
            '    context.Response.StatusCode = 200
            'Catch e As Exception
            '    context.Response.ContentType = "text/plain"
            '    context.Response.StatusCode = 301
            'End Try

            'sw.Flush()
            'sw.Close()





            '' 
            'Dim json = New StreamReader(context).ReadToEnd()
            'Dim stripeEvent = StripeEventUtility.ParseEvent(json)





            'Imports System
            'Imports System.IO
            'Imports Microsoft.AspNetCore.Mvc
            'Imports Stripe

            'Namespace workspace.Controllers
            '    <Route("api/[controller]")>
            '    Public Class StripeWebHook
            '        Inherits Controller

            '        <HttpPost>
            '        Public Sub Index()
            '            Dim json = New StreamReader(HttpContext.Request.Body).ReadToEnd()
            '            Dim stripeEvent = StripeEventUtility.ParseEvent(json)
            '        End Sub
            '    End Class
            'End Namespace



            Return 200

        Catch ex As Exception

            Return 0
        End Try
    End Function

End Class