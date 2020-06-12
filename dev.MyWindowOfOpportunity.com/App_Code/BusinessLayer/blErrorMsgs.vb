Public Class blErrorMsgs

#Region "Properties"
    Private linqErrorMsgs As linqErrorMsgs = New linqErrorMsgs
#End Region


#Region "Selects"
#End Region

#Region "Insert"
    Public Sub saveErrorMessage(ByVal _userId As String, ByVal _exceptionMsg As String, ByVal _generalMsg As String)
        linqErrorMsgs.saveErrorMessage(_userId, _exceptionMsg, _generalMsg)
    End Sub
#End Region

#Region "Updates"
#End Region

#Region "Delete"
#End Region

#Region "Methods"
#End Region

#Region "Validations"
#End Region
End Class
