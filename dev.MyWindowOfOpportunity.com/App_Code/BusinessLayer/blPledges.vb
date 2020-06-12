Imports System.Data
Imports Common
Imports umbraco.Core.Models
Imports umbraco.Core
Imports umbraco.Core.Publishing
Imports Stripe
Imports Newtonsoft.Json
Imports System.Web.Script.Serialization

Public Class blPledges

#Region "Properties"
    Private linqCampaigns As linqCampaigns = New linqCampaigns
    Private linqPledges As linqPledges = New linqPledges
    Private blPledges As blPledges
    Private blRewards As blRewards
    Private _uHelper As Uhelper = New Uhelper()

    Public Sub New()
    End Sub
#End Region


#Region "Selects"
    Public Function select_byPhaseId(ByVal _phaseId As Integer, Optional ByVal _amountOnly As Boolean = False) As List(Of CampaignPledge)
        Return linqPledges.select_byPhaseId(_phaseId, _amountOnly)
    End Function
    Public Function selectSum_byCampaignId(ByVal _campaignId As Integer) As Decimal?
        Return linqPledges.selectSum_byCampaignId(_campaignId)
    End Function
    Public Function selectSum_byPhaseId(ByVal _phaseId As Integer) As Decimal?
        Return linqPledges.selectSum_byPhaseId(_phaseId)
    End Function
    Public Function selectSum_byPhaseId(ByVal _phaseId As Integer, ByVal _Type As String) As Decimal?
        Return linqPledges.selectSum_byPhaseId(_phaseId, _Type)
    End Function
    Public Function selectCount_byPhaseId(ByVal _phaseId As Integer) As Decimal?
        Return linqPledges.selectCount_byPhaseId(_phaseId)
    End Function
    Public Function selectCount_byPhaseId(ByVal _phaseId As Integer, ByVal _Type As String) As Decimal?
        Return linqPledges.selectCount_byPhaseId(_phaseId, _Type)
    End Function
    Public Function selectCount_byCampaignId(ByVal _campaignId As Integer) As Integer?
        Return linqPledges.selectCount_byCampaignId(_campaignId)
    End Function
    Public Function selectAll_byCampaignId_forEditCampaign(ByVal _campaignId As Integer) As BusinessReturn
        'Instantiate variables
        Dim bReturn As BusinessReturn
        Dim lstCampaignPledges As List(Of CampaignPledge)

        Try
            'obtain data
            bReturn = linqPledges.selectAll_byCampaignId_forEditCampaign(_campaignId)

            If bReturn.isValid Then
                If bReturn.DataContainer.Count > 0 Then
                    'Extract data from return
                    lstCampaignPledges = bReturn.DataContainer(0)
                    'Sort data 
                    Dim result As List(Of CampaignPledge) = lstCampaignPledges.OrderByDescending(Function(C) C.phaseId).ThenByDescending(Function(C) C.pledgeDate).ToList
                    'Readd data to return
                    bReturn = New BusinessReturn
                    bReturn.DataContainer.Add(result)
                End If
            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\BusinessLayer\blPledges.vb : selectAll_byCampaignId_forEditCampaign()")
            sb.AppendLine("_campaignId" & _campaignId)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return Nothing
        End Try
        Return bReturn
    End Function
    Public Function selectAll_byCampaignId_forCampaignPg(ByVal _campaignId As Integer) As BusinessReturn
        'Instantiate variables
        Dim bReturn As BusinessReturn
        Dim lstCampaignPledges As List(Of CampaignPledge)

        Try
            'obtain data
            bReturn = linqPledges.selectAll_byCampaignId_forCampaignPg(_campaignId)

            If bReturn.isValid Then
                'Extract data from return
                lstCampaignPledges = bReturn.DataContainer(0)
                'Sort data 
                Dim result As List(Of CampaignPledge) = lstCampaignPledges.OrderByDescending(Function(C) C.phaseId).ThenByDescending(Function(C) C.pledgeDate).ToList
                'Readd data to return
                bReturn = New BusinessReturn
                bReturn.DataContainer.Add(result)
            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\BusinessLayer\blPledges.vb : selectAll_byCampaignId_forCampaignPg()")
            sb.AppendLine("_campaignId" & _campaignId)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return Nothing
        End Try
        Return bReturn
    End Function
    Public Function obtainPledgeStatus_byId(ByVal _pledgeId As Integer) As CampaignPledge
        Return linqPledges.obtainPledgeStatus_byId(_pledgeId)
    End Function
    Public Function obtainIncompletePledges_byPhase(ByVal activePhaseId As Integer) As List(Of CampaignPledge)
        Return linqPledges.obtainIncompletePledges_byPhase(activePhaseId)
    End Function
#End Region

#Region "Inserts"
    Public Function CreatePledge(ByVal stripeCharge As StripeCharge, ByVal campaignId As Integer, ByVal phaseId As String, ByVal rewardId As String, ByVal memberId As Integer, ByVal showAsAnonymous As Boolean) As BusinessReturn
        'Instantiate global variables
        Dim businessReturn As New BusinessReturn

        Try
            'Instantiate variables
            Dim pledge As New CampaignPledge
            Dim sb As New StringBuilder
            Dim blMembers As blMembers = New blMembers
            Dim blCampaigns = New blCampaigns
            Dim memberUpdateReturn As New BusinessReturn

            'Build class for passing data from stripe charge
            pledge.phaseId = CInt(phaseId)
            pledge.pledgeAmount = stripeCharge.Amount / 100
            Dim stripeFee As Integer = blCampaigns.getStripeFee(stripeCharge.Amount)
            If stripeFee > 0 Then pledge.stripeFee = CDec(stripeFee / 100)
            Dim mwooFee As Integer = blCampaigns.getMWoOFees(campaignId, stripeCharge.Amount)
            If mwooFee > 0 Then pledge.mwooFee = CDec(mwooFee / 100)
            pledge.netPledgeAmount = ((pledge.pledgeAmount - pledge.mwooFee) - pledge.stripeFee)

            pledge.pledgeName = "$" & pledge.pledgeAmount.ToString
            pledge.pledgingMember = memberId
            pledge.showAsAnonymous = showAsAnonymous
            pledge.pledgeDate = stripeCharge.Created 'Note: this time is based in Europe

            pledge.balanceTransactionId = stripeCharge.BalanceTransactionId
            pledge.chargeId = stripeCharge.Id
            pledge.chargeStatus = stripeCharge.Status
            pledge.customerId = stripeCharge.CustomerId
            pledge.destinationId = stripeCharge.DestinationId
            pledge.transferId = stripeCharge.TransferId
            pledge.transferGroup = stripeCharge.TransferGroup


            pledge.fulfilled = True
            pledge.fulfillmentDate = DateTime.Now

            pledge.campaignId = getCampaignId(pledge.phaseId)
            pledge.campaignName = _uHelper.Get_IPublishedContentByID(pledge.campaignId).Name
            If Not showAsAnonymous Then pledge.pledgingMemberName = blMembers.getMemberName_byId(memberId)

            If Not IsNothing(rewardId) Then
                pledge.rewardSelected = CInt(rewardId)

                'Send to reward list to adjust available/claimed
                blRewards = New blRewards
                blRewards.UpdateRewardAvailableNumber(pledge.rewardSelected)
            End If

            sb.AppendLine("Pledge Submitted to Stripe: ")
            sb.AppendLine(JsonConvert.SerializeObject(stripeCharge))
            pledge.notes = sb.ToString

            'Pass data to umbraco
            businessReturn = linqPledges.CreatePledge(pledge)

            If businessReturn.isValid Then

                '
                memberUpdateReturn = blMembers.UpdatePledges(memberId, CInt(businessReturn.DataContainer(0).PledgeId))
                businessReturn.ReturnMessage = memberUpdateReturn.isValid

                Return businessReturn
            Else
                Return businessReturn
            End If

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\BusinessLayer\blPledges.vb : CreatePledge()")
            sb.AppendLine("phaseId: " & phaseId)
            sb.AppendLine("rewardId: " & rewardId)
            sb.AppendLine("memberId: " & memberId)
            sb.AppendLine("showAsAnonymous: " & showAsAnonymous.ToString)

            Dim jsonSerializer As JavaScriptSerializer = New JavaScriptSerializer()
            sb.AppendLine("stripeCharge: " & jsonSerializer.Serialize(stripeCharge))

            'ByVal  As StripeCharge, ByVal  As String, ByVal  As String, ByVal  As Integer, ByVal  As Boolean) As BusinessReturn
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

            businessReturn.ExceptionMessage = ex.ToString
            Return businessReturn
        End Try
    End Function
    Public Sub createPayoutRequest(ByVal payout As Payout)
        linqPledges.createPayoutRequest(payout)
    End Sub
#End Region

#Region "Updates"
    Public Function UpdateContentCancelled(ByVal pledgeId As Int16, ByVal _canceled As Boolean) As Attempt(Of PublishStatus)
        Return linqPledges.UpdateContentCancelled(pledgeId, _canceled)
    End Function
    Public Function UpdatePledgeStatus(ByVal pledgeId As Int16, ByVal stripeCharge As StripeCharge) As Attempt(Of PublishStatus)
        Return linqPledges.UpdatePledgeStatus(pledgeId, stripeCharge)
    End Function
    Public Function refundPledge(ByVal ipPledge As IPublishedContent) As Attempt(Of PublishStatus)
        Return linqPledges.refundPledge(ipPledge)
    End Function
    Public Function UpdatePledgeMngrNotes(ByVal pledgeId As Integer, ByVal rewardShipped As Boolean, ByVal trackingNo As String, ByVal campaignMngrNotes As String) As Attempt(Of PublishStatus)
        Return linqPledges.UpdatePledgeMngrNotes(pledgeId, rewardShipped, trackingNo, campaignMngrNotes)
    End Function
    Public Function UpdatePledgePayoutStatus(ByVal destinationId As Int16, ByVal payout As StripePayout) As Attempt(Of PublishStatus)
        Return linqPledges.UpdatePledgePayoutStatus(destinationId, payout)
    End Function
    Public Sub submitFundPayouts(ByVal payoutFolder As Integer)
        linqPledges.submitFundPayouts(payoutFolder)
    End Sub
#End Region

#Region "Delete"
#End Region

#Region "Methods"
    'Public Function anyPendingTransfers(ByVal ipCampaign As IPublishedContent) As Boolean?
    '    Return linqPledges.anyPendingTransfers(ipCampaign)
    'End Function
    Public Function obtainAllTransfers(ByVal ipCampaign As IPublishedContent) As StripeList(Of StripeTransfer)
        Return linqPledges.obtainAllTransfers(ipCampaign)
    End Function
    Public Function obtainAccountDetails(ByVal ipCampaign As IPublishedContent) As StripeAccount
        Return linqPledges.obtainAccountDetails(ipCampaign)

    End Function
    Public Function transferFunds(ByVal transferId As Integer, ByVal financialId As String, ByVal amount As Integer) As StripePayout
        Return linqPledges.payoutFunds(transferId, financialId, amount)
    End Function
#End Region

#Region "Validations"
#End Region

#Region "Private Methods"
#End Region

End Class