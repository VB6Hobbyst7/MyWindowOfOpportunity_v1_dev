Imports umbraco
Imports Common
Imports umbraco.Core
Imports umbraco.Core.Publishing
Imports umbraco.Core.Models
Imports umbraco.Web

Partial Class UserControls_editTab_Content
    Inherits System.Web.UI.UserControl


#Region "Properties"
    'Private _thisNode As IPublishedContent
    'Public Property thisNode() As IPublishedContent
    '    Get
    '        Return _thisNode
    '    End Get
    '    Set(value As IPublishedContent)
    '        _thisNode = value
    '    End Set
    'End Property

    Public Property thisNodeId() As String
        Get
            Return hfldThisNodeId.Value
        End Get
        Set(value As String)
            hfldThisNodeId.Value = value.ToString
        End Set
    End Property
    Dim _uHelper As Uhelper = New Uhelper()
    Private blCampaigns As blCampaigns
    Public Property campaignComplete As Boolean = False
#End Region

#Region "Handles"
    Private Sub UserControls_editTab_Campaign_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not String.IsNullOrEmpty(thisNodeId) Then
            If Not IsPostBack Then
                Try
                    'Instantiate variables
                    Dim thisNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(CInt(thisNodeId))

                    'Obtain campaign data
                    If thisNode.HasProperty(nodeProperties.briefSummary) Then txbBriefSummary.Text = thisNode.GetPropertyValue(Of String)(nodeProperties.briefSummary)
                    If thisNode.HasProperty(nodeProperties.fullSummary) Then
                        hdfdTemplateContent.Value = thisNode.GetPropertyValue(Of String)(nodeProperties.fullSummary)

                        'Pass IPublishedContent to controls
                        ucSelectFeaturedImage.thisNodeId = thisNodeId
                        ucSelectedPanelImg.thisNodeId = thisNodeId
                        ucSelectedBannerImg.thisNodeId = thisNodeId

                        'rectangle shape
                        ucSelectedBannerImg.propertyName = nodeProperties.topBannerImage
                        ucSelectedBannerImg.cropName = Crops.campaignFeaturedImage

                        'square shape
                        ucSelectedPanelImg.propertyName = nodeProperties.summaryPanelImage
                        ucSelectedPanelImg.cropName = Crops.campaignSummaryImage

                    End If

                    If thisNode.HasValue(nodeProperties.customCSS) Then txbCustomCss.Text = thisNode.GetPropertyValue(Of String)(nodeProperties.customCSS)

                    'disable is campaign is complete
                    If campaignComplete Then btnSaveContent.Disabled = True

                Catch ex As Exception
                    Dim sb As New StringBuilder()
                    sb.AppendLine("editTab_Content.ascx.vb : UserControls_editTab_Campaign_Load()")
                    saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
                    'Response.Write("Campaign Tab: " & ex.ToString & "<br />")
                End Try
            End If

            '
            ucSocialMediaManager.nodeId = thisNodeId

            '
        End If
    End Sub
    Private Sub lbtnSave_Click(sender As Object, e As EventArgs) Handles lbtnSaveContent.Click
        Try
            'Instantiate variables
            blCampaigns = New blCampaigns
            Dim updateResponse As Attempt(Of PublishStatus)
            Dim alert As ASP.usercontrols_alertmsg_ascx

            'Set hidden field with tab name
            hfldTabName.Value = tabNames.content

            'Submit data for updating
            updateResponse = blCampaigns.UpdateContent(thisNodeId, txbBriefSummary.Text.Trim, hdfdTemplateContent.Value, ucSelectedPanelImg.RemoveSelectedImg, ucSelectedBannerImg.RemoveSelectedImg, txbCustomCss.Text.Trim)

            'If valid, 
            If (updateResponse.Success) Then
                'Show success msg
                alert = New ASP.usercontrols_alertmsg_ascx With {
                    .MessageType = UserControls_AlertMsg.msgType.Success,
                    .AlertMsg = "Saved Successfully"
                }
                phAlertMsg.Controls.Add(alert)
            Else
                'Show alert msg
                alert = New ASP.usercontrols_alertmsg_ascx With {
                    .MessageType = UserControls_AlertMsg.msgType.Alert,
                    .AlertMsg = "Error. Unable to save data."
                }
                phAlertMsg.Controls.Add(alert)
            End If

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\editTab_Content.ascx.vb : lbtnSave_Click()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Response.Write(ex.ToString)
        End Try
    End Sub
#End Region

#Region "Methods"


#End Region
End Class
