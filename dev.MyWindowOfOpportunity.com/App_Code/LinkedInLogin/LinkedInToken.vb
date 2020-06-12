Imports Microsoft.VisualBasic

Public Class LinkedInToken
    Private _access_token As String
    Public Property access_token() As String
        Get
            Return _access_token
        End Get
        Set(ByVal value As String)
            _access_token = value
        End Set
    End Property
    Private _expires_in As String
    Public Property expires_in() As String
        Get
            Return _expires_in
        End Get
        Set(ByVal value As String)
            _expires_in = value
        End Set
    End Property
End Class

Public Structure LinkedInUtil
    Const LinkedInState As String = "987654321"
End Structure

