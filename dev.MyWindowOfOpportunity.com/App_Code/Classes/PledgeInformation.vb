Imports Microsoft.VisualBasic

Public Class PledgeInformation
    Public Property campaignId As Integer?
    Public Property phaseId As Integer?
    Public Property rewardId As Integer?

    Public Property applyFee As Boolean = False

    Public Property pledgeTotal As Integer?
    Public Property feeTotal As Integer = 0
    Public Property afterFee As Integer?
End Class
