Imports Microsoft.VisualBasic


Public Class TeamSummary
    Public Property teamId As Integer?
    Public Property mediaFolderId As Integer?
    Public Property teamImageId As Integer?
    Public Property teamName As String = String.Empty
    Public Property whoAreWe As String = String.Empty
    Public Property teamImageName As String = String.Empty
    Public Property teamImageUrl As String = String.Empty
    Public Property facebookUrl As String = String.Empty
    Public Property twitterUrl As String = String.Empty
    Public Property googlePlusUrl As String = String.Empty
    Public Property supportEmailUrl As String = String.Empty
    Public Property linkedInUrl As String = String.Empty
    Public Property removeTeamImage As Boolean = False

    Public Property lstMembers As New List(Of Member)


    Public Sub New()
    End Sub
End Class