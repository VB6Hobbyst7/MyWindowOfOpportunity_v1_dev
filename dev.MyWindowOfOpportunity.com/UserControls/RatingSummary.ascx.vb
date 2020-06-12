Imports Common


Partial Class UserControls_RatingSummary
    Inherits System.Web.UI.UserControl


#Region "Properties"
    Public Property thisNodeId() As Int16
    Public Property reviewCount() As Integer = 0
    Public Property condensedView() As Boolean = False
    Public Property transparentBg() As Boolean = False

    Private Enum view
        condensed
        fullWidth
    End Enum
#End Region

#Region "Handles"
    Private Sub UserControls_RatingSummary_PreRender(sender As Object, e As EventArgs) Handles Me.Load
        Try
            'Add campaign id to panel as data attribute.
            pnlRatingBorder.Attributes.Add("data-CampaignId", thisNodeId)

            'Add transparency if applicable.
            If transparentBg Then pnlRatingBorder.CssClass += Miscellaneous.clearBg

            'Obtain data for rating summary
            obtainRatingSummary()
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\UserControls_RatingSummary.ascx.vb : UserControls_RatingSummary_PreRender()")

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
        '
    End Sub
#End Region

#Region "Methods"
    Public Sub obtainRatingSummary()
        'Instantiate variables
        Dim businessReturn As BusinessReturn
        Dim ratingSummary As RatingSummary
        Dim blCampaigns As New blCampaigns

        Try
            'Obtain data
            businessReturn = blCampaigns.obtainRatingSummary_byCampaignId(thisNodeId)

            If businessReturn.isValid Then
                'Extract data
                ratingSummary = DirectCast(businessReturn.DataContainer(0), RatingSummary)

                'Dim lst As New List(Of RatingSummary)
                'lst.Add(ratingSummary)
                'gv.DataSource = lst
                'gv.DataBind()


                '
                If condensedView() Then
                    'Display proper view
                    mvRatingSummaries.ActiveViewIndex = view.condensed

                    'Display percentages
                    ucProgressBar_1Star.percentage = ratingSummary.star1Percentage
                    ltrl_1Star.Text = ratingSummary.star1Percentage

                    ucProgressBar_2Stars.percentage = ratingSummary.star2Percentage
                    ltrl_2Stars.Text = ratingSummary.star2Percentage

                    ucProgressBar_3Stars.percentage = ratingSummary.star3Percentage
                    ltrl_3Stars.Text = ratingSummary.star3Percentage

                    ucProgressBar_4Stars.percentage = ratingSummary.star4Percentage
                    ltrl_4Stars.Text = ratingSummary.star4Percentage

                    ucProgressBar_5Stars.percentage = ratingSummary.star5Percentage
                    ltrl_5Stars.Text = ratingSummary.star5Percentage

                    'Calculate average stars
                    ltrlAvgStars.Text = ratingSummary.avgStars

                Else
                    'Display proper view
                    mvRatingSummaries.ActiveViewIndex = view.fullWidth

                    'Display percentages
                    ucProgressBar_1Star_full.percentage = ratingSummary.star1Percentage
                    ltrl_1Star_full.Text = ratingSummary.star1Percentage

                    ucProgressBar_2Stars_full.percentage = ratingSummary.star2Percentage
                    ltrl_2Stars_full.Text = ratingSummary.star2Percentage

                    ucProgressBar_3Stars_full.percentage = ratingSummary.star3Percentage
                    ltrl_3Stars_full.Text = ratingSummary.star3Percentage

                    ucProgressBar_4Stars_full.percentage = ratingSummary.star4Percentage
                    ltrl_4Stars_full.Text = ratingSummary.star4Percentage

                    ucProgressBar_5Stars_full.percentage = ratingSummary.star5Percentage
                    ltrl_5Stars_full.Text = ratingSummary.star5Percentage

                    'Calculate average stars
                    ltrlAvgStars_full.Text = ratingSummary.avgStars
                End If


                'Add array of ratings to hidden field
                hdnCampaignRatingArray.Value = ratingSummary.ratingArray

                'Add count of 
                hfldReviewCount.Value = ratingSummary.reviewCount
                reviewCount = ratingSummary.reviewCount
            End If

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\UserControls_RatingSummary.ascx.vb : obtainRatingSummary()")

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
    End Sub

#End Region
End Class