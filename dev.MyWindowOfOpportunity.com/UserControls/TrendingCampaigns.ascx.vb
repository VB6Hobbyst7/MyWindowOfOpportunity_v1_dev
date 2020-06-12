Imports Common

Partial Class UserControls_Home_TrendingCampaigns
    Inherits System.Web.UI.UserControl
#Region "Properties"
#End Region

#Region "Handles"
    Private Sub UserControls_Home_TrendingCampaigns_Load(sender As Object, e As EventArgs) Handles Me.Load
        'Instantiate variables
        Try
            Dim blCampaigns As New blCampaigns

            'Obtain a list of the top active campaigns
            lstviewCampaignPanels.DataSource = blCampaigns.selectTopTrendingCampaigns()
            lstviewCampaignPanels.DataBind()
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\TrendingCampaigns.ascx.vb : UserControls_Home_TrendingCampaigns_Load()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
    End Sub
#End Region

#Region "Methods"
#End Region

End Class
