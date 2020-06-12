Imports Microsoft.VisualBasic

Public Class MembershipProperties
    Public Property userId() As Integer = 0
    Public Property nodeName() As String = String.Empty
    Public Property loginName() As String = String.Empty
    Public Property password() As String = String.Empty
    Public Property email() As String = String.Empty
    Public Property altEmail() As String = String.Empty
    Public Property isFacebookAcct As Boolean = False
    Public Property isLinkedInAcct As Boolean = False
    Public Property isTwitterAcct As Boolean = False
End Class
