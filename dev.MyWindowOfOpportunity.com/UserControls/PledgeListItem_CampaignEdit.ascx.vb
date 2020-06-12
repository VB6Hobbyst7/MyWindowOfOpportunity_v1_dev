Imports System.Globalization
Imports Common
Imports umbraco.Web

Partial Class UserControls_PledgeListItem
    Inherits System.Web.UI.UserControl


#Region "Properties"
    Public Property campaignPledges() As CampaignPledge
#End Region

#Region "Handles"
    Private Sub UserControls_PledgeListItem_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                If Not IsNothing(campaignPledges) Then


                    'Dim lstRewardFulfillment As New List(Of RewardFulfillment)
                    'Dim lstShippingInfo As New List(Of ShippingInfo)
                    'lstRewardFulfillment.Add(campaignPledges.rewardFulfillment)
                    'lstShippingInfo.Add(campaignPledges.rewardFulfillment.shippingInfo)

                    'gv2.DataSource = lstRewardFulfillment
                    'gv3.DataSource = lstShippingInfo

                    'gv2.DataBind()
                    'gv3.DataBind()

                    If IsNothing(campaignPledges.rewardFulfillment.rewardId) Or String.IsNullOrEmpty(campaignPledges.rewardFulfillment.rewardTitle) Then
                        'Hide controls
                        phShipmentDetails.Visible = False
                        phShipmentStatus.Visible = False
                        ucShipTo.Visible = False
                    Else
                        'Pass data to control
                        ucShipTo.campaignPledges = campaignPledges

                        '
                        imgShipmentDetails.Attributes.Add("data-rewardid", campaignPledges.rewardFulfillment.rewardId)
                        imgShipmentDetails.Attributes.Add("data-pledgeid", campaignPledges.pledgeId)

                        '
                        If campaignPledges.rewardFulfillment.rewardShipped Then
                            lblStatus.Text = "Reward Shipped"
                        Else
                            lblStatus.Text = "Reward Pending"
                        End If
                    End If


                    'Instantiate variables
                    Dim thisNodeId As Integer = UmbracoContext.Current.PageId

                    'Show data
                    If Not IsNothing(campaignPledges.pledgeDate) Then ltrlDate.Text = CDate(campaignPledges.pledgeDate).ToString("MMMM d, yyyy")
                    ltrlDonatedBy.Text = campaignPledges.pledgingMemberName
                    If Not IsNothing(campaignPledges.pledgeAmount) Then ltrlPledged.Text = CDec(campaignPledges.pledgeAmount).ToString("C", CultureInfo.CurrentCulture)

                    'Determine the proper pledge status
                    Select Case True
                        Case campaignPledges.canceled
                            pnlPledge.CssClass += pledgeStatusStruct.canceled
                            pnlPledge.Attributes.Add("data-status", pledgeStatusStruct.canceled)
                        Case campaignPledges.transactionDeclined
                            pnlPledge.CssClass += pledgeStatusStruct.declined
                            pnlPledge.Attributes.Add("data-status", pledgeStatusStruct.declined)
                        Case campaignPledges.reimbursed
                            pnlPledge.CssClass += pledgeStatusStruct.reimbursed
                            pnlPledge.Attributes.Add("data-status", pledgeStatusStruct.reimbursed)
                        Case campaignPledges.fulfilled
                            pnlPledge.CssClass += pledgeStatusStruct.fulfilled
                            pnlPledge.Attributes.Add("data-status", pledgeStatusStruct.fulfilled)
                    End Select

                    'Add pledge id to panel
                    pnlPledge.Attributes.Add("data-pledgeid", campaignPledges.pledgeId.ToString)
                    lblStatus.Attributes.Add("data-pledgeid", campaignPledges.pledgeId.ToString)
                    lblStatus.Attributes.Add("data-rewardid", campaignPledges.rewardFulfillment.rewardId.ToString)
                End If
            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("PledgeListItem_CampaignEdit.ascx.vb : Load()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Response.Write("<br />Error: " & ex.ToString)
        End Try
    End Sub
#End Region

#Region "Methods"
#End Region
End Class
