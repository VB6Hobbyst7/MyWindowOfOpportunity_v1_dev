Public Class ValidationContainer
    Dim _ErrorMessage As String
    'Dim _ControlName As String

    Public ReadOnly Property ErrorMessage() As String
        Get
            Return _ErrorMessage
        End Get
    End Property

    'Public ReadOnly Property ControlName() As String
    '    Get
    '        Return _ControlName
    '    End Get
    'End Property

    Public Sub New(ByVal ErrorMessage As String) ', ByVal ControlName As String)
        _ErrorMessage = ErrorMessage
        '_ControlName = ControlName
    End Sub
End Class
