Imports Microsoft.VisualBasic

Public Class DiscoveryPhase
    '
    Public Property nodeId As Integer = 0
    Public Property phaseActive As Boolean = False
    Public Property activationDate As Date?
    Public Property completionDate As Date?
    Public Property lstRatings As New List(Of Rating)
    Public Property rating As Single = 0


    Public Sub New()
    End Sub
End Class
