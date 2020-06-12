Imports umbraco.Core
Imports Common

Public Class blContactUs
#Region "Properties"
    Private linqContactUsData As linqContactUs = New linqContactUs
#End Region

#Region "Insert"
    Public Function InsertContactUs(ByVal personName As String, ByVal email As String, ByVal phoneNumber As String, ByVal subject As String, ByVal message As String, ByVal _ParentNodeId As Integer) As BusinessReturn
        'Instantiate variables
        Dim _contactUs As ContactUs = New ContactUs
        Dim result As BusinessReturn = New BusinessReturn
        Dim _dob As Date = Date.Today

        Try
            'Add Data to class
            _contactUs.personName = personName.Trim.ToLower.ToFirstUpper
            _contactUs.email = email.Trim.ToLower
            _contactUs.phoneNumber = phoneNumber.Trim
            _contactUs.subject = subject.Trim
            _contactUs.message = message.ToString
            _contactUs.addedDate = DateTime.Now
            'Save data to database
            Return linqContactUsData.insertContactUs(_contactUs, _ParentNodeId)

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("/App_Code\BusinessLayer\blContactUs.vb : InsertContactUs()")
            sb.AppendLine("personName:" & personName)
            sb.AppendLine("email:" & email)
            sb.AppendLine("phoneNumber:" & phoneNumber)
            sb.AppendLine("subject:" & subject)
            sb.AppendLine("message:" & message)
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
