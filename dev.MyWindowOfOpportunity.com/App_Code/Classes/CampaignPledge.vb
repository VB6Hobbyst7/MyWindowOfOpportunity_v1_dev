Public Class CampaignPledge

    Public Property pledgeId As System.Nullable(Of Integer) = Nothing
    Public Property pledgeName As String = String.Empty
    Public Property campaignId As System.Nullable(Of Integer) = Nothing
    Public Property campaignName As String = String.Empty
    Public Property campaignUrl As String = String.Empty
    Public Property phaseId As System.Nullable(Of Integer) = Nothing
    Public Property phaseNumber As System.Nullable(Of Integer) = Nothing

    Public Property pledgingMember As System.Nullable(Of Integer) = Nothing
    Public Property pledgingMemberName As String = String.Empty
    Public Property pledgingMemberImgUrl As String = String.Empty
    Public Property showAsAnonymous As Boolean = False

    Public Property pledgeAmount As System.Nullable(Of Decimal) = 0F
    Public Property stripeFee As System.Nullable(Of Decimal) = 0F
    Public Property mwooFee As System.Nullable(Of Decimal) = 0F
    Public Property netPledgeAmount As System.Nullable(Of Decimal) = 0F

    Public Property pledgeDate As System.Nullable(Of Date) = Nothing
    Public Property rewardSelected As System.Nullable(Of Integer) = Nothing
    Public Property rewardFulfillment As New RewardFulfillment

    Public Property balanceTransactionId As String = String.Empty
    Public Property chargeId As String = String.Empty
    Public Property chargeStatus As String = String.Empty
    Public Property customerId As String = String.Empty
    Public Property destinationId As String = String.Empty
    Public Property transferId As String = String.Empty
    Public Property transferGroup As String = String.Empty
    Public Property notes As String = String.Empty

    Public Property fulfilled As Boolean = False
    Public Property fulfillmentDate As System.Nullable(Of Date) = Nothing

    Public Property transfered As Boolean = False
    Public Property transferDate As System.Nullable(Of Date) = Nothing

    Public Property canceled As Boolean = False
    Public Property dateCanceled As System.Nullable(Of Date) = Nothing

    Public Property transactionDeclined As Boolean = False
    Public Property dateDeclined As System.Nullable(Of Date) = Nothing

    Public Property reimbursed As Boolean = False
    Public Property reimbursedDate As System.Nullable(Of Date) = Nothing



    Public Sub New()
    End Sub
End Class



