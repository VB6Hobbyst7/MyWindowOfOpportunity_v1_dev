Imports Common
Partial Class UserControls_ShipTo
    Inherits System.Web.UI.UserControl


#Region "Properties"
    Public Property campaignPledges As CampaignPledge
#End Region

#Region "Handles"
    Private Sub UserControls_ShipTo_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not IsNothing(campaignPledges) AndAlso Not IsNothing(campaignPledges.rewardFulfillment) Then
                'Instantiate variables
                Dim sb As New StringBuilder

                'Display Shipping Info
                If Not String.IsNullOrEmpty(campaignPledges.rewardFulfillment.memberName) Then sb.AppendLine(campaignPledges.rewardFulfillment.memberName & "<br />")
                If Not String.IsNullOrEmpty(campaignPledges.rewardFulfillment.shippingInfo.address01) Then sb.AppendLine(campaignPledges.rewardFulfillment.shippingInfo.address01 & "<br />")
                If Not String.IsNullOrEmpty(campaignPledges.rewardFulfillment.shippingInfo.address02) Then sb.AppendLine(campaignPledges.rewardFulfillment.shippingInfo.address02 & "<br />")
                If Not String.IsNullOrEmpty(campaignPledges.rewardFulfillment.shippingInfo.city) Then sb.AppendLine(campaignPledges.rewardFulfillment.shippingInfo.city & ", ")
                If Not String.IsNullOrEmpty(campaignPledges.rewardFulfillment.shippingInfo.stateProvidence) Then sb.AppendLine(campaignPledges.rewardFulfillment.shippingInfo.stateProvidence.ToUpperInvariant & " ")
                If Not String.IsNullOrEmpty(campaignPledges.rewardFulfillment.shippingInfo.postalCode) Then sb.AppendLine(campaignPledges.rewardFulfillment.shippingInfo.postalCode)
                lblShippingInfo.Text = sb.ToString

                'Display email link
                If Not String.IsNullOrEmpty(campaignPledges.rewardFulfillment.memberEmail) Then
                    hlnkMailTo.Visible = True
                    hlnkMailTo.NavigateUrl += campaignPledges.rewardFulfillment.memberEmail
                End If

                'Display other data
                lblReward.Text = campaignPledges.rewardFulfillment.rewardTitle
                txbTrackingNo.Text = campaignPledges.rewardFulfillment.trackingNo
                txbNotes.Text = campaignPledges.rewardFulfillment.campaignMngrNotes
                cbFulfilled.Checked = campaignPledges.rewardFulfillment.rewardShipped

                pnlFieldset.Attributes.Add("data-rewardid", campaignPledges.rewardFulfillment.rewardId)
                hlnkSave.Attributes.Add("data-rewardid", campaignPledges.rewardFulfillment.rewardId)

                pnlFieldset.Attributes.Add("data-pledgeid", campaignPledges.pledgeId)
                hlnkSave.Attributes.Add("data-pledgeid", campaignPledges.pledgeId)
            End If

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\UserControls_ShipTo.ascx.vb : UserControls_ShipTo_Load()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

        End Try
    End Sub
#End Region

#Region "Methods"
#End Region
End Class
