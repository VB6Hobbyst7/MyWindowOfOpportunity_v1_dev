Imports System.Globalization
Imports Common
Imports umbraco.Web

Partial Class UserControls_PledgeListItem
    Inherits System.Web.UI.UserControl


#Region "Properties"
    Public Property campaignPledge() As CampaignPledge
#End Region

#Region "Handles"
    Private Sub UserControls_PledgeListItem_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        Try
            If Not IsPostBack Then
                If Not IsNothing(campaignPledge) Then
                    'Dim lst As New List(Of CampaignPledge)
                    'lst.Add(campaignPledge)

                    'gv.DataSource = lst
                    'gv.DataBind()




                    'Instantiate variables
                    Dim thisNodeId As Integer = UmbracoContext.Current.PageId

                    'Show data
                    If Not IsNothing(campaignPledge.pledgeDate) Then ltrlDate.Text = CDate(campaignPledge.pledgeDate).ToString("MMMM d, yyyy")
                    If Not IsNothing(campaignPledge.pledgeAmount) Then ltrlPledged.Text = CDec(campaignPledge.pledgeAmount).ToString("C", CultureInfo.CurrentCulture)
                    'ltrlCampaign.Text = campaignPledge.campaignName
                    hlnkCampaign.Text = campaignPledge.campaignName
                    hlnkCampaign.NavigateUrl = campaignPledge.campaignUrl

                    'ltrlPhaseNo.Text = "1"

                    'Determine the proper pledge status
                    Select Case True
                        Case campaignPledge.canceled
                            pnlPledge.CssClass += pledgeStatusStruct.canceled
                        Case campaignPledge.transactionDeclined
                            pnlPledge.CssClass += pledgeStatusStruct.declined
                        Case campaignPledge.reimbursed
                            pnlPledge.CssClass += pledgeStatusStruct.reimbursed
                        Case campaignPledge.fulfilled
                            pnlPledge.CssClass += pledgeStatusStruct.fulfilled
                    End Select







                End If

            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\PledgeListItem_AcctEdit.ascx.vb : UserControls_PledgeListItem_PreRender()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Response.Write("<br />Error: " & ex.ToString)
        End Try
    End Sub
#End Region

#Region "Methods"
#End Region
End Class
