Imports umbraco
Imports Common
Imports umbraco.Core
Imports umbraco.Core.Publishing
Imports umbraco.Core.Models
Imports umbraco.Web

Partial Class UserControls_FeaturedImageSelector
    Inherits System.Web.UI.UserControl


#Region "Properties"
    Public Enum _viewPanels
        ShowImg = 0
        SelectImg = 1
    End Enum


    Public Property propertyName() As String
        Get
            Return hfldPropertyName.Value
        End Get
        Set(value As String)
            hfldPropertyName.Value = value
        End Set
    End Property
    Public Property cropName As String = String.Empty
    Public Property thisNodeId() As String
        Get
            Return hfldCurrentNodeId.Value
        End Get
        Set(value As String)
            hfldCurrentNodeId.Value = value
        End Set
    End Property
    Public Property activeView() As _viewPanels
        Get
            Return __viewPanel
        End Get
        Set(value As _viewPanels)
            __viewPanel = value
        End Set
    End Property
    Public ReadOnly Property RemoveSelectedImg As Boolean
        Get
            Return CBool(hfldRemoveSelectedImg.Value)
        End Get
    End Property


    Private __viewPanel As _viewPanels = 0
    Private blMedia As blMedia
    Private blTeams As blTeams
    Private blCampaigns As blCampaigns
    Private _uHelper As New Uhelper()
#End Region

#Region "Handles"
    Private Sub UserControls_FeaturedImageSelector_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                'Set active view
                mvFeaturedImageSelector.ActiveViewIndex = activeView()

                'Set data
                If activeView = _viewPanels.SelectImg Then
                    'Obtain list of banners to select
                    obtainBannerImages()

                    If UmbracoContext.Current.PublishedContentRequest.PublishedContent.DocumentTypeAlias = docTypes.Team Then
                        h6Campaign.Visible = False
                        h6Team.Visible = True
                    End If
                End If
            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\FeaturedImageSelector.ascx.vb : UserControls_FeaturedImageSelector_Load()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Response.Write("Error: " & ex.ToString)
        End Try
    End Sub
    Private Sub UserControls_FeaturedImageSelector_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        Try
            'Set data
            If activeView = _viewPanels.ShowImg Then
                'Obtain the currently selected banner from umbraco
                obtainCurrentBanner()
            End If
            'End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\FeaturedImageSelector.ascx.vb : UserControls_FeaturedImageSelector_PreRender()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Response.Write("Error: " & ex.ToString)
        End Try
    End Sub
    Private Sub btnSelectBanner_Click(sender As Object, e As EventArgs) Handles btnSelectBanner.Click
        Try
            'Response.Write("{" & hfldSelectedPropertyName.Value & "} ")

            Select Case UmbracoContext.Current.PublishedContentRequest.PublishedContent.DocumentTypeAlias
                Case docTypes.Team
                    'Save team image
                    setTeamImage()
                Case docTypes.editCampaign
                    'Set the current banner to selected banner.
                    setCurrentBanner()
            End Select
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\FeaturedImageSelector.ascx.vb : btnSelectBanner_Click()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
    End Sub
#End Region

#Region "Methods"
    Private Sub obtainBannerImages()
        If thisNodeId <> -1 Then
            Dim thisNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(thisNodeId)

            Try
                'Instantiate variables
                blMedia = New blMedia
                blTeams = New blTeams
                Dim result As BusinessReturn = New BusinessReturn

                'Obtain list of images and add to listview
                Select Case thisNode.DocumentTypeAlias
                    Case docTypes.Team
                        result = blTeams.selectImages_byTeamId(thisNode)
                    Case docTypes.Campaign
                        result = blMedia.selectImages_byCampaignId(thisNode, cropName)
                    Case Else
                        result = blMedia.selectImages_byCampaignId(thisNode, cropName)
                End Select
                'Dim result As BusinessReturn = blMedia.selectImages_byCampaignId(New IPublishedContent(thisNodeId), Crops.campaignFeaturedImage)
                'add images datatable to listview
                If result.isValid Then
                    'Add list to Image Library
                    lstviewImageLibrary.DataSource = result.DataContainer(0)
                    lstviewImageLibrary.DataBind()
                End If
            Catch ex As Exception
                Dim sb As New StringBuilder()
                sb.AppendLine("\UserControls\FeaturedImageSelector.ascx.vb : obtainBannerImages()")
                sb.AppendLine("thisNodeId: " & thisNodeId)
                saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
                'Response.Write("Error: " & ex.ToString & "<br />Aliase: " & thisNode.NodeTypeAlias & "<br />thisNodeId: " & thisNodeId & "<br />Crop: " & Crops.campaignFeaturedImage)
            End Try
        End If

    End Sub
    Private Sub obtainCurrentBanner()
        Try
            'Instantiate variables
            Dim thisNode As IPublishedContent
            Dim imgProperty As String = String.Empty

            '
            If String.IsNullOrEmpty(thisNodeId) Then
                thisNode = UmbracoContext.Current.PublishedContentRequest.PublishedContent
                thisNodeId = thisNode.Id
            Else
                thisNode = _uHelper.Get_IPublishedContentByID(thisNodeId)
            End If

            'If a featured image banner exists, obtain it.
            If thisNode.HasProperty(propertyName) AndAlso thisNode.HasValue(propertyName) Then
                'Instantiate variables
                blMedia = New blMedia

                'Assign image parts to image panel.
                pnlSelectedImage.Visible = True
                selectedImage.ImageUrl = getMediaURL(thisNode.GetPropertyValue(Of String)(propertyName), cropName)
                lblDelete.Attributes.Add("mediaId", thisNode.GetPropertyValue(Of String)(propertyName))

                'If a featured image exists, obtain image name.
                If thisNode.HasValue(thisNode.GetPropertyValue(Of String)(propertyName)) Then
                    Dim businessReturn As BusinessReturn = blMedia.selectMediaName_byId(thisNode.GetPropertyValue(Of String)(propertyName))
                    If businessReturn.isValid Then lblTitle.Text = businessReturn.ReturnMessage
                End If

            ElseIf thisNode.HasProperty(nodeProperties.teamImage) AndAlso thisNode.HasValue(nodeProperties.teamImage) Then
                'Instantiate variables
                blMedia = New blMedia

                'Assign image parts to image panel.
                pnlSelectedImage.Visible = True
                selectedImage.ImageUrl = getMediaURL(thisNode.GetPropertyValue(Of String)(nodeProperties.teamImage))
                lblDelete.Attributes.Add("mediaId", thisNode.GetPropertyValue(Of String)(nodeProperties.teamImage))

                'If a featured image exists, obtain image name.
                If thisNode.HasValue(thisNode.GetPropertyValue(Of String)(nodeProperties.teamImage)) Then
                    Dim businessReturn As BusinessReturn = blMedia.selectMediaName_byId(thisNode.GetPropertyValue(Of String)(nodeProperties.teamImage))
                    If businessReturn.isValid Then lblTitle.Text = businessReturn.ReturnMessage
                End If

            Else
                'No featured image.  Hide image panel.
                pnlSelectedImage.Visible = False
            End If

            'If session exists, show proper alert message.
            If Not IsNothing(Session(Sessions.FeaturedImageUpdated)) Then
                'Instantiate variables
                Dim alert As ASP.usercontrols_alertmsg_ascx = New ASP.usercontrols_alertmsg_ascx
                alert.showIcon = False

                'Set what type of alert to display.
                If CBool(Session(Sessions.FeaturedImageUpdated)) Then
                    'Show success msg
                    alert.MessageType = UserControls_AlertMsg.msgType.Success
                    alert.AlertMsg = "Updated Successfully"
                Else
                    'Show alert msg
                    alert.MessageType = UserControls_AlertMsg.msgType.Alert
                    alert.AlertMsg = "Unable to update"
                End If

                'Assign alert to placeholder
                phAlertMsg.Controls.Add(alert)

                'Clear temp session variable
                Session(Sessions.FeaturedImageUpdated) = Nothing
            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\FeaturedImageSelector.ascx.vb : obtainCurrentBanner()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Response.Write("Error: " & ex.ToString)
        End Try
    End Sub
    Private Sub setCurrentBanner()
        Try
            'Instantiate variables
            blCampaigns = New blCampaigns
            Dim updateResponse As Attempt(Of PublishStatus)

            'Submit updated data
            updateResponse = blCampaigns.SaveFeaturedImage(CInt(hfldCurrentNodeId.Value), CInt(hfldSelectedMediaId.Value), hfldSelectedPropertyName.Value)

            'Show if response was successfull or not.
            Session(Sessions.FeaturedImageUpdated) = updateResponse.Success

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\FeaturedImageSelector.ascx.vb : setCurrentBanner()")

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Response.Write("Error: " & ex.ToString)
        End Try
    End Sub
    Private Sub setTeamImage()
        Try
            'Instantiate variables
            blTeams = New blTeams
            Dim updateResponse As Attempt(Of PublishStatus)

            'Submit updated data
            updateResponse = blTeams.SaveTeamImage(CInt(hfldCurrentNodeId.Value), CInt(hfldSelectedMediaId.Value))

            'Show if response was successfull or not.
            Session(Sessions.FeaturedImageUpdated) = updateResponse.Success


            'Set active tab
            setTabCookie(tabNames.content)
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\FeaturedImageSelector.ascx.vb : setTeamImage()")

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Response.Write("Error: " & ex.ToString)
        End Try
    End Sub
#End Region
End Class
