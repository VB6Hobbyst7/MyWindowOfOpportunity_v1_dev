Imports Common
Imports umbraco.Core
Imports umbraco.Core.Models
Imports umbraco.Core.Services
Imports umbraco.Core.Publishing
Imports umbraco.Web

Public Class linqPreAcctData

#Region "Properties"
    Private linq2Db As linq2SqlDataContext = New linq2SqlDataContext(ConfigurationManager.ConnectionStrings("umbracoDbDSN").ToString)
    Dim _uHelper As Uhelper = New Uhelper()
#End Region

#Region "Selects"
    Public Function selectRecord_byId(ByVal _id As Integer) As BusinessReturn
        'Instantiate variables
        Dim result As BusinessReturn = New BusinessReturn
        Dim preAccount_result As PreAccountmembers = New PreAccountmembers

        Try
            'Create a new campaign IPublishedContent
            Dim cs As IContentService = ApplicationContext.Current.Services.ContentService
            Dim ic As IContent = cs.GetById(_id)

            If Not IsNothing(ic) Then
                'Obtain record to be deleted
                preAccount_result.preAcctId = Convert.ToString(_id)
                If ic.HasProperty(nodeProperties.firstName) Then preAccount_result.firstName = ic.GetValue(Of String)(nodeProperties.firstName)
                If ic.HasProperty(nodeProperties.lastName) Then preAccount_result.lastName = ic.GetValue(Of String)(nodeProperties.lastName)
                If ic.HasProperty(nodeProperties.email) Then preAccount_result.email = ic.GetValue(Of String)(nodeProperties.email)
                If ic.HasProperty(nodeProperties.password) Then preAccount_result.password = ic.GetValue(Of String)(nodeProperties.password)
                If ic.HasProperty(nodeProperties.dateOfBirth) Then preAccount_result.dob = ic.GetValue(Of String)(nodeProperties.dateOfBirth)
                result.DataContainer.Add(preAccount_result)

                Return result
            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\linqPreAcctData.vb : selectRecord_byId()")
            sb.AppendLine("_id:" & _id)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

            result.ExceptionMessage = ex.ToString
            Return result
        End Try
    End Function
#End Region

#Region "Inserts"
    Public Function addData(ByVal preAcct As PreAccount) As BusinessReturn
        'Instantiate variables
        Dim result As BusinessReturn = New BusinessReturn

        Try

            Dim cs As IContentService = ApplicationContext.Current.Services.ContentService
            Dim preAccountMember As IContent = cs.GetById(siteNodes.PreAccountMembers)
            Dim PreAccountEntry As IContent = cs.CreateContent(preAcct.firstName + " " + preAcct.lastName, preAccountMember, docTypes.preAccountMember, 0)
            Dim updateResponse As Attempt(Of PublishStatus)
            PreAccountEntry.SetValue(nodeProperties.firstName, preAcct.firstName)
            PreAccountEntry.SetValue(nodeProperties.lastName, preAcct.lastName)
            PreAccountEntry.SetValue(nodeProperties.password, preAcct.password)
            PreAccountEntry.SetValue(nodeProperties.email, preAcct.email)
            PreAccountEntry.SetValue(nodeProperties.dateOfBirth, preAcct.dob)

            'Set default values
            updateResponse = cs.SaveAndPublishWithStatus(PreAccountEntry)
            'Return new IPublishedContent's Id
            If (updateResponse.Success) Then
                result.ReturnMessage = PreAccountEntry.Id
                Return result
            Else
                result.ExceptionMessage = updateResponse.Exception.ToString
                Return result
            End If

        Catch ex As Exception
            'Save error to umbraco
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\linqQueries\linqPreAcctData.vb : addData()")
            sb.AppendLine("preAcct:" & preAcct.ToString())

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'saveErrorMessage(String.Empty, ex.ToString, String.Empty)
            'Return error msg
            result.ExceptionMessage = ex.ToString
            Return result
        End Try
    End Function
#End Region

#Region "Updates"
#End Region

#Region "Delete"
    Public Sub deleteRecord(ByVal _id As Integer)
        Try
            'Obtain record to be deleted
            'Dim PreAccountMemberNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(_id)
            Dim cs As IContentService = ApplicationContext.Current.Services.ContentService
            Dim _memberNode As IContent = cs.GetById(_id)
            cs.Delete(_memberNode)

        Catch ex As Exception
            'Save error to umbraco
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\linqQueries\linqPreAcctData.vb : deleteRecord()")
            sb.AppendLine("_id:" & _id)

            ' saveErrorMessage(String.Empty, ex.ToString, "Id: " & _id.ToString)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
    End Sub
#End Region

#Region "Methods"
#End Region

End Class

