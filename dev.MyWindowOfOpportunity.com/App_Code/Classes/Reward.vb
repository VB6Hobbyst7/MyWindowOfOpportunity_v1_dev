Imports Microsoft.VisualBasic

Public Class Reward
    'Public properties
    Public Property nodeId As String = String.Empty
    Public Property rewardTitle As String = String.Empty
    Public Property shortDescription As String = String.Empty
    Public Property contributionAmount As Double = 0F
    Public Property quantityAvailable As Integer = 0
    Public Property quantityClaimed As Integer = 0
    Public Property estimatedShippingMonth As String = String.Empty
    Public Property estimatedShippingYear As String = String.Empty
    Public Property featuredImgName As String = String.Empty
    Public Property featuredImgUrl As String = String.Empty

    Public Property isAvailable As Boolean = False
    Public Property showOnTimeline As Boolean = False


    Public Sub New()
    End Sub
End Class
