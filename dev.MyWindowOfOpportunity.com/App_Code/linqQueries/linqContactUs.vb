Imports Common
Imports umbraco.Core
Imports umbraco.Core.Models
Imports umbraco.Core.Services

Public Class linqContactUs

#Region "Inserts"
    Public Function insertContactUs(ByVal _contactUs As ContactUs, ByVal _ParentNodeId As Integer) As BusinessReturn
        'Instantiate variables
        Dim result As BusinessReturn = New BusinessReturn

        Try

            Dim cs As IContentService = ApplicationContext.Current.Services.ContentService
            Dim contactUsMsgs As IContent = cs.GetById(siteNodes.ClientMessages)
            Dim ContactUsEntry As IContent = cs.CreateContent(_contactUs.personName, contactUsMsgs, docTypes.messageItem, 0)
            Dim updateResponse As Attempt(Of Publishing.PublishStatus)
            'Set default values
            ContactUsEntry.SetValue(nodeProperties.personName, _contactUs.personName)
            ContactUsEntry.SetValue(nodeProperties.email, _contactUs.email)
            ContactUsEntry.SetValue(nodeProperties.phoneNumber, _contactUs.phoneNumber)
            ContactUsEntry.SetValue(nodeProperties.subject, _contactUs.subject)
            ContactUsEntry.SetValue(nodeProperties.message, _contactUs.message)
            ContactUsEntry.SetValue(nodeProperties.addedDate, _contactUs.addedDate)

            updateResponse = cs.SaveAndPublishWithStatus(ContactUsEntry)
            'Return new IPublishedContent's response
            If (updateResponse.Success) Then
                result.ReturnMessage = "Success"
                Return result
            Else
                result.ExceptionMessage = updateResponse.Exception.ToString
                Return result
            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\linqQueries\linqContactUs.vb : insertContactUs()")
            sb.AppendLine("_contactUs:" & _contactUs.ToString())
            sb.AppendLine("_ParentNodeId:" & _ParentNodeId)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Save error to umbraco
            ' saveErrorMessage(String.Empty, ex.ToString, String.Empty)
            'Return error msg
            result.ExceptionMessage = ex.ToString
            Return result
        End Try
    End Function
#End Region
End Class
