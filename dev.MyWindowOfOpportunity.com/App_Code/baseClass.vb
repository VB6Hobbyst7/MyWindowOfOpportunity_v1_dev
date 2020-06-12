Imports Common

Public MustInherit Class baseClass
    Inherits System.Web.UI.UserControl

#Region "Properties"
#End Region

#Region "Validation Methods"
    Public Sub DisplayValidationErrors(ByVal returnObject As BusinessReturn, Optional ByVal ValidationGroup As String = "")
        Try  'Display all validation errors
            For I = 0 To (returnObject.ValidationMessages.Count - 1)
                AddValidationMsg(returnObject.ValidationMessages(I).ErrorMessage, ValidationGroup)
            Next

            'Display any exception messages
            If (returnObject.ExceptionMessage <> String.Empty) Then
                AddValidationMsg(returnObject.ExceptionMessage, ValidationGroup)
            End If
        Catch ex As Exception
            'Save error to umbraco
            Dim sb As New StringBuilder
            sb.AppendLine("baseClass.vb | DisplayValidationErrors()")
            sb.AppendLine("returnObject" & JsonHelper.ToJson(returnObject))
            saveErrorMessage(getLoggedInMember(), ex.ToString, sb.ToString)
        End Try
    End Sub
    Public Sub AddValidationMsg(ByVal msg As String, Optional ByVal ValidationGroup As String = "")
        Try

            'Create custom validation
            Dim cv As New CustomValidator With {
            .IsValid = False,
            .ErrorMessage = msg
        }

            'If a validation group is provided, show only in that validation group
            If Not String.IsNullOrEmpty(ValidationGroup) Then
                cv.ValidationGroup = ValidationGroup
            End If

            'Add msg to summary
            Me.Page.Validators.Add(cv)
        Catch ex As Exception
            'Save error to umbraco
            Dim sb As New StringBuilder
            sb.AppendLine("baseClass.vb | AddValidationMsg()")
            sb.AppendLine("msg" & msg)
            saveErrorMessage(getLoggedInMember(), ex.ToString, sb.ToString)
        End Try
    End Sub
#End Region

End Class
