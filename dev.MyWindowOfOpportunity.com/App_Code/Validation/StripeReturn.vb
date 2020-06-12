Imports Microsoft.VisualBasic

Public Class StripeReturn
    Dim _msgError As String
    Public Property Failed() As String
        Get
            Return _msgError
        End Get
        Set(ByVal value As String)
            _msgError = value
        End Set
    End Property

    Dim _msgSuccess As String
    Public Property Success() As String
        Get
            Return _msgSuccess
        End Get
        Set(ByVal value As String)
            _msgSuccess = value
        End Set
    End Property
    Dim _json As String
    Public Property GetJson() As String
        Get
            Return _json
        End Get
        Set(ByVal value As String)
            _json = value
        End Set
    End Property
End Class
