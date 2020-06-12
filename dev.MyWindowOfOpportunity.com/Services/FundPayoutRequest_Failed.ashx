﻿<%@ WebHandler Language="VB" Class="FundPayoutRequest_Failed" %>

Imports System
Imports System.Web
Imports Common

Public Class FundPayoutRequest_Failed : Implements IHttpHandler

#Region "Properties"
    Private blPledges As blPledges
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property
#End Region

#Region "Handles"
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Try
            'Instantiate variables
            blPledges = New blPledges

            'Run nightly update
            blPledges.submitFundPayouts(siteNodes.failedPayouts)

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("ERROR: \Handlers\FundPayoutRequest_Failed.ashx : ProcessRequest()")
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