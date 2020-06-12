Imports Common
Imports umbraco.Core.Models
Imports umbraco.Web

Partial Class UserControls_Reward
    Inherits System.Web.UI.UserControl


#Region "Properties"
    Public Property nodeId As Integer = 0
    Dim _uHelper As Uhelper = New Uhelper()
#End Region

#Region "Handles"
    Private Sub UserControls_Reward_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Try
                'Instantiate variables
                Dim thisNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(nodeId)

                'Get contribution amount
                Dim contributionAmount As String() = thisNode.GetPropertyValue(Of String)(nodeProperties.contributionAmount).Split(".")
                ltrlContributionAmount.Text = String.Format("{0:c0}", CInt(contributionAmount(0)))

                'Get featured image
                imgFeatured.ImageUrl = getMediaURL(thisNode.GetPropertyValue(Of String)(nodeProperties.featuredImage))
                imgFeatured.AlternateText = thisNode.Name & " - " & getMediaName(thisNode.GetPropertyValue(Of String)(nodeProperties.featuredImage))
                imgFeatured.Attributes.Add("title", thisNode.Name)

                'Obtain other values for reward
                ltrlHeading.Text = thisNode.Name
                ltrlShortDescription.Text = thisNode.GetPropertyValue(Of String)(nodeProperties.shortDescription)
                ltrlAvailable.Text = thisNode.GetPropertyValue(Of String)(nodeProperties.available)
                ltrlClaimed.Text = thisNode.GetPropertyValue(Of String)(nodeProperties.claimed)

                'Generate estimated shipping date value
                If thisNode.GetPropertyValue(Of String)(nodeProperties.estimatedShippingYear) = "TBD" Then
                    ltrlShippingDate.Text = "TBD"
                Else
                    ltrlShippingDate.Text = thisNode.GetPropertyValue(Of String)(nodeProperties.estimatedShippingMonth) & "&nbsp;" &
                        thisNode.GetPropertyValue(Of String)(nodeProperties.estimatedShippingYear)
                End If

            Catch ex As Exception
                Dim sb As New StringBuilder()
                sb.AppendLine("\UserControls\Reward.ascx.vb : UserControls_Reward_Load()")

                saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
                'Response.Write(ex.ToString)
            End Try
        End If
    End Sub
#End Region

#Region "Methods"
#End Region
End Class
