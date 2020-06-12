<%@ WebHandler Language="VB" Class="MemberExist" %>

Imports System
Imports System.Web
Imports Common


Public Class MemberExist : Implements IHttpHandler


#Region "Properties"
#End Region

#Region "Handles"
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Try
            'Instanatiate variables
            Dim blMembers As blMembers = New blMembers
            Dim email As String = context.Request.QueryString("email")
            Dim doesEmailExist As Boolean = blMembers.doesMemberExist_byEmail(email)

            'Convert page to a json response.
            context.Response.Clear()
            context.Response.ContentType = "application/json; charset=utf-8"
            context.Response.Write(JsonHelper.ToJson(doesEmailExist))

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\Handlers\MemberExist.ashx : ProcessRequest()")
            sb.AppendLine("context:" & context.ToString())

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            context.Response.Clear()
            context.Response.ContentType = "application/json; charset=utf-8"
            context.Response.Write("Error: " & ex.ToString)
            context.Response.End()
        End Try

        'Finalize response
        context.Response.End()
    End Sub

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property
#End Region

#Region "Methods"
#End Region


End Class