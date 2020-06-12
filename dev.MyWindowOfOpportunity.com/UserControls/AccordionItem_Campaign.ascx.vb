Imports Common
Partial Class UserControls_AccordionItem_Campaign
    Inherits System.Web.UI.UserControl


#Region "Properties"
    Public Property nodeId As Integer
    Public Property isActive As Boolean = True
    Private blCampaigns As blCampaigns
#End Region

#Region "Handles"
    Private Sub UserControls_AccordionItem_Campaign_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            'Instantiate variables
            Try
                Dim pnlName As String = "pnl" & nodeId
                blCampaigns = New blCampaigns
                Dim businessReturn As BusinessReturn
                Dim campaignSummary As CampaignSummary_TeamPg


                '
                businessReturn = blCampaigns.selectCampaignSummary_byId(nodeId)

                If businessReturn.isValid Then
                    'Obtain campaign data
                    campaignSummary = DirectCast(businessReturn.DataContainer(0), CampaignSummary_TeamPg)

                    'Dim lst As New List(Of CampaignSummary_TeamPg)
                    'lst.Add(campaignSummary)
                    'gv1.DataSource = lst
                    'gv1.DataBind()


                    'Display data
                    ltrlCampaignName_Handle.Text = campaignSummary.title
                    ltrlBriefSummary.Text = campaignSummary.briefSummary
                    If isActive Then
                        hrefViewCampaign.NavigateUrl = campaignSummary.campaignUrl
                    Else
                        hrefViewCampaign.Visible = False
                    End If

                    pnl.Attributes.Add("data-campaignid", campaignSummary.campaignId)

                    If String.IsNullOrEmpty(campaignSummary.imageUrl) Then

                    Else
                        imgFeaturedImage.ImageUrl = campaignSummary.imageUrl
                    End If

                End If

                'Set panel and handle name/nav
                pnl.ID = pnlName
                hlnkHandle.Attributes.Add("href", "#" & pnlName)

            Catch ex As Exception
                Dim sb As New StringBuilder()
                sb.AppendLine("\UserControls\UserControls_AccordionItem_Campaign.ascx.vb : UserControls_AccordionItem_Campaign_Load()")
                saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            End Try
        End If
    End Sub
#End Region

#Region "Methods"
#End Region
End Class
