
Public Class NavigationItem

    Public Property nodeId As Integer = 0
    Public Property parentNodeId As Integer = 0
    Public Property isParent As Boolean = False
    Public Property title As String = String.Empty
    Public Property navigationUrl As String = String.Empty
    Public Property level As Integer = -1
    Public Property sortOrder As Integer = -1
    Public Property description As String = String.Empty


    Public Sub New()
    End Sub
End Class
