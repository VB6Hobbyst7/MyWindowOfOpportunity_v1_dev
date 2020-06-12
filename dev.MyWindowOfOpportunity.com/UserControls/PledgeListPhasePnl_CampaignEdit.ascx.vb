Imports Common
Imports umbraco.Web

Partial Class UserControls_PledgeListPhasePnl
    Inherits System.Web.UI.UserControl


#Region "Properties"
    Public Property phaseNo() As Int16 = 0
    Public Property lstCampaignPledges() As List(Of CampaignPledge)
#End Region

#Region "Handles"
    Private Sub UserControls_PledgeListPhasePnl_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                If IsNothing(lstCampaignPledges) OrElse lstCampaignPledges.Count = 0 Then
                    phasePnl.Visible = False
                Else
                    'Instantiate variables
                    Dim thisNodeId As Integer = UmbracoContext.Current.PageId

                    'Show phase number
                    ltrlPhaseNo.Text = phaseNo.ToString



                    lstPledgeItems.DataSource = lstCampaignPledges
                    lstPledgeItems.DataBind()
                End If
            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\PledgeListItem_CampaignEdit.ascx.vb : UserControls_PledgeListPhasePnl_Load()")


            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Response.Write("Error: " & ex.ToString)
        End Try
    End Sub
#End Region

#Region "Methods"
#End Region
End Class
