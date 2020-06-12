Imports Common
Imports umbraco
Imports umbraco.NodeFactory
Imports Stripe
Imports System.Data
Imports umbraco.Core
Imports umbraco.Core.Models
Imports umbraco.Core.Services
Imports umbraco.Core.Publishing
Imports System.Xml.XPath
Imports System.Net.Mail
Imports System.Net
Imports Newtonsoft.Json
Imports umbraco.Web
Imports FinancialHandler
Imports System.IO
Imports System.Web.Script.Services
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports Newtonsoft
Imports Newtonsoft.Json.Serialization
Imports System.Xml
Imports System.Web.Mvc
Imports System.Net.Mime


Partial Class Masterpages_Top_Level
    Inherits System.Web.UI.MasterPage



#Region "Properties"
#End Region

#Region "Handles"
    Private Sub Masterpages_Top_Level_Load(sender As Object, e As EventArgs) Handles Me.Load

        'Show down for maintenance if set to true.
        If ConfigurationManager.AppSettings("downForMaintenance") Then
            cphMainContent.Visible = False
            DownForMaintenance.Visible = True
        End If

        'Show beta test panel if in debug mode.
        'If HttpContext.Current.IsDebuggingEnabled Then
        '    pnlBetaTesting.Visible = True
        'End If

        'Used for displaying beta msg before launch date.
        'If Date.Now < New Date(2018, 6, 1) Then pnlBetaTesting.Visible = True
    End Sub
    'Private Sub btnTemp_Click(sender As Object, e As EventArgs) Handles btnTemp.Click
    '    'TEMP- Delete this when finished.
    '    submitStripeTransaction()
    'End Sub
#End Region

#Region "Methods"
#End Region



End Class

