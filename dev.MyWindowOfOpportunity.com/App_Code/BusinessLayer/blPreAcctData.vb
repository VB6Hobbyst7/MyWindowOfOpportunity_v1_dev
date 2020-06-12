Imports Common
Imports umbraco.Core

Public Class blPreAcctData

#Region "Properties"
    Private linqPreAcctData As linqPreAcctData = New linqPreAcctData
#End Region


#Region "Selects"
    Public Function selectRecord_byId(ByVal _id As Integer) As BusinessReturn
        Dim result As New BusinessReturn

        Try
            result = linqPreAcctData.selectRecord_byId(_id)

            If result.isValid Then
                'Decrypt data if it exists.
                If result.DataContainer.Count > 0 Then
                    'Obtain record from response
                    Dim result_preAccount As PreAccountmembers = New PreAccountmembers
                    result_preAccount = result.DataContainer(0)

                    'Decrypt data
                    result_preAccount.preAcctId = result_preAccount.preAcctId
                    result_preAccount.email = result_preAccount.email
                    result_preAccount.firstName = result_preAccount.firstName
                    result_preAccount.lastName = result_preAccount.lastName
                    result_preAccount.password = result_preAccount.password
                    result_preAccount.dob = result_preAccount.dob
                    'Add data back to response
                    result = New BusinessReturn
                    result.DataContainer.Add(result_preAccount)
                End If
            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("blPreAcctData.vb : selectRecord_byId()")
            sb.AppendLine("id: " & _id)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try

        Return result

    End Function
#End Region

#Region "Insert"
    Public Function addData(ByVal firstName As String, ByVal lastName As String, ByVal email As String, ByVal password As String, ByVal dob As String) As BusinessReturn
        'Instantiate variables
        Dim preAcct As PreAccount = New PreAccount
        Dim result As BusinessReturn = New BusinessReturn
        Dim _dob As Date = Date.Today

        Try
            'Convert date string to date
            If IsDate(dob) Then
                _dob = Date.Parse(dob)
            End If

            'Add Data to class
            preAcct.firstName = firstName.Trim.ToLower.ToFirstUpper
            preAcct.lastName = lastName.Trim.ToLower.ToFirstUpper
            preAcct.email = email.Trim.ToLower
            preAcct.password = password.Trim
            preAcct.dob = _dob.ToString
            preAcct.dateSubmitted = DateTime.Now.ToShortDateString

            'Save data to database
            Return linqPreAcctData.addData(preAcct)
        Catch ex As Exception
            'Save error to umbraco
            '  saveErrorMessage(String.Empty, ex.ToString, String.Empty)
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\BusinessLayer\blPreAcctData.vb : addData()")
            sb.AppendLine("firstName:" & firstName)
            sb.AppendLine("lastName:" & lastName)
            sb.AppendLine("email:" & email)
            sb.AppendLine("password:" & password)
            sb.AppendLine("dob:" & dob)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
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
        linqPreAcctData.deleteRecord(_id)
    End Sub
#End Region

#Region "Methods"
#End Region

#Region "Validations"
#End Region
End Class
