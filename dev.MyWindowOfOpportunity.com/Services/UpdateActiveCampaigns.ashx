<%@ WebHandler Language="VB" Class="UpdateActiveCampaigns" %>

Imports System
Imports System.Web
Imports Common

Public Class UpdateActiveCampaigns : Implements IHttpHandler

#Region "Properties"
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property
    Private blCampaigns As blCampaigns
#End Region

#Region "Handles"
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        'context.Response.ContentType = "text/plain"
        'context.Response.Write("Hello World")
        Try
            'Instantiate variables
            blCampaigns = New blCampaigns

            'Run nightly update
            blCampaigns.nightlyUpdate()

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\Handlers\UpdateActiveCampaigns.ashx : ProcessRequest()")
            sb.AppendLine("context:" & context.ToString())
            saveErrorMessage(0, ex.ToString, sb.ToString())

            context.Response.ContentType = "text/plain"
            context.Response.Write("<br />Error: " & ex.ToString & "<br/><br />" & sb.ToString())
        End Try
    End Sub
#End Region

#Region "Methods"

#End Region


End Class