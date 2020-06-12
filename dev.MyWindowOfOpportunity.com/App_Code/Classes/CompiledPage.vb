Imports Microsoft.VisualBasic

Public Class CompiledPage
    'Class Properties
    Public Property nodeId As Integer? = Nothing
    Public Property nodeName As String = String.Empty
    Public Property title As String = String.Empty
    Public Property description As String = String.Empty
    Public Property showInNavigation As Boolean = False
    Public Property showInFooter As Boolean = False

    Public Property lstSegments As New List(Of Segment)
    Public Property lstSideSegments As New List(Of Segment)



    Public Sub New()
    End Sub
End Class
