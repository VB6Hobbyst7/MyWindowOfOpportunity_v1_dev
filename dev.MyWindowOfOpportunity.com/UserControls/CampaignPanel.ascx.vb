Imports Common

Partial Class UserControls_CampaignPanel
    Inherits System.Web.UI.UserControl


#Region "Properties"
    Public Property CampaignSummary As CampaignSummary
#End Region

#Region "Handles"
    Private Sub UserControls_CampaignPanel_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then

            Try
                'Determine what view to show
                If CampaignSummary.statusType = statusType.DiscoveryPhase Then
                    divCampaignStats.Visible = False
                    divCampaignRatingReview.Visible = True
                Else
                    divCampaignStats.Visible = True
                    divCampaignRatingReview.Visible = False
                End If


                If Not CampaignSummary.nodeId > 0 Then
                    showViewMorePnl()
                Else
                    'Add campaign id to rating summary
                    ucRatingSummary.thisNodeId = CampaignSummary.nodeId
                    If CampaignSummary.Completed Then
                        phCompleteCampaign.Visible = True
                        pnlCampaign.CssClass += statusTypeValues.Complete
                    Else
                        phActiveCampaign.Visible = True
                    End If

                    'Apply class data to controls
                    hlnkCampaign.NavigateUrl = CampaignSummary.campaignUrl
                    imgCampaign.ImageUrl = CampaignSummary.imageUrl
                    ltrlTitle.Text = CampaignSummary.title
                    ltrlAuthor.Text = CampaignSummary.team
                    ucProgressBar.percentage = CampaignSummary.percentFunded * 100
                    ltrlPledged.Text = FormatCurrency(CampaignSummary.currentlyPledged, , , TriState.True, TriState.True).Replace(".00", "")
                    ltrlPercentage.Text = FormatPercent(CampaignSummary.percentFunded, 0)

                    ltrlPledged_Complete.Text = ltrlPledged.Text
                    ltrlPercentage_Complete.Text = ltrlPercentage.Text
                    'ltrlDescription.Text = CampaignSummary.shortDescription 'statusType
                    'If CampaignSummary.statusType = statusType.Complete Then imgRewardIcon.Visible = True
                    ucDaysRemaining.daysRemaining = CampaignSummary.daysRemaining
                    ucDaysRemaining.daysRemaining_percentage = (100 / siteValues.campaignLength) * (CampaignSummary.daysRemaining)
                End If

                'Show cheer if present
                If String.IsNullOrEmpty(CampaignSummary.cheer) Then
                    pnlCheer.Visible = False
                Else
                    ltrlCheer.Text = CampaignSummary.cheer
                End If

                'Display phase meters
                ucPhaseMeter01.phaseStatus = CampaignSummary.statusType
                ucPhaseMeter02.phaseStatus = CampaignSummary.statusType
                ucPhaseMeter01.phaseCount = CampaignSummary.phaseCount
                ucPhaseMeter02.phaseCount = CampaignSummary.phaseCount

            Catch ex As Exception
                Dim sb As New StringBuilder()
                sb.AppendLine("\UserControls\CampaignPanel.ascx.vb : UserControls_CampaignPanel_Load()")
                saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            End Try
        End If
    End Sub
#End Region

#Region "Methods"
    Private Sub UserControls_CampaignPanel_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        'Obtain review count if it exists. [called after review calculations are complete during load]
        If Not IsPostBack Then
            ltrlReviewCount.Text = ucRatingSummary.reviewCount.ToString  'discovery.Children.Count
        End If
    End Sub
    Private Sub showViewMorePnl()
        'Show proper panel
        pnlCampaign.Visible = False
        pnlViewMore.Visible = True

        'Determine if we need to show info for categories or subcategories
        Dim querystringCollection As New NameValueCollection
        If Request.QueryString.HasKeys() Then
            querystringCollection = HttpUtility.ParseQueryString(Request.QueryString.ToString)
        End If
        With querystringCollection
            If Not .AllKeys.Contains(queryParameters.Category) Then
                'Display Parital List
                phSubcategory.Visible = True
            End If
        End With

        'Add content from class
        imgViewMore.ImageUrl = CampaignSummary.imageUrl
        hlnkViewMore.NavigateUrl = CampaignSummary.campaignUrl
    End Sub
#End Region
End Class
