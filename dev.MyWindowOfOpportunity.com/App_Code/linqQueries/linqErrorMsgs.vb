Imports Common
Imports umbraco.Core
Imports umbraco.Core.Models
Imports umbraco.Core.Services


Public Class linqErrorMsgs

#Region "Properties"
    Private linq2Db As linq2SqlDataContext = New linq2SqlDataContext(ConfigurationManager.ConnectionStrings("umbracoDbDSN").ToString)
#End Region

#Region "Selects"
#End Region

#Region "Inserts"
    Public Sub saveErrorMessage(ByVal _userId As String, ByVal _exceptionMsg As String, ByVal _generalMsg As String)

        Try
            'Create a new node
            Dim cs As IContentService = ApplicationContext.Current.Services.ContentService
            Dim siteErrors As IContent = cs.GetById(siteNodes.SiteErrors)
            Dim timeStamp As DateTime = DateTime.Now
            Dim errorMsg As IContent = cs.CreateContentWithIdentity(timeStamp.ToString, siteErrors, docTypes.errorMessage)

            'Set values
            errorMsg.SetValue(nodeProperties.exceptionMessage, _exceptionMsg)
            errorMsg.SetValue(nodeProperties.generalMessage, _generalMsg)
            If Not String.IsNullOrEmpty(_userId) AndAlso IsNumeric(_userId) AndAlso CInt(_userId) > 0 Then
                errorMsg.SetValue(nodeProperties.member, CInt(_userId))
            End If

            'Save values
            cs.SaveAndPublishWithStatus(errorMsg)

        Catch ex As Exception
            'Dim sb As New StringBuilder()
            'sb.AppendLine("\App_Code\linqQueries\linqErrorMsgs.vb : saveErrorMessage()")
            'sb.AppendLine("_userId:" & _userId)
            'sb.AppendLine("_exceptionMsg" & _exceptionMsg)
            Throw ex

        End Try

    End Sub
#End Region

#Region "Updates"
#End Region

#Region "Delete"
#End Region

#Region "Methods"
#End Region

End Class

