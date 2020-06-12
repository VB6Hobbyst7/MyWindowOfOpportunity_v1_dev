Imports Microsoft.VisualBasic

Public Class RewardFulfillment
    Public Property rewardId As Integer?
    Public Property rewardTitle As String = String.Empty
    Public Property rewardShipped As Boolean = False
    Public Property trackingNo As String = String.Empty
    Public Property campaignMngrNotes As String = String.Empty

    Public Property memberId As Integer = 0
    Public Property memberName As String = String.Empty
    Public Property memberEmail As String = String.Empty
    Public Property shippingInfo As New ShippingInfo


    Public Sub New()
    End Sub
End Class
