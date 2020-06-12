Imports umbraco
Imports Common
Imports umbraco.Core.Models
Imports umbraco.Web

Partial Class UserControls_MinorImageSelector_Rewards
    Inherits System.Web.UI.UserControl


#Region "Properties"
    Private blMedia As blMedia
    Private blCampaigns As blCampaigns

    Public Enum _viewPanels
        DisplayImg = 0
        SelectImg = 1
    End Enum
    Private _viewPanel As _viewPanels = 0
    Public Enum _tabs
        Rewards
        TeamMembers
        Timeline
        Phases
        Categories
        Content
    End Enum
    Private _thisTab As _tabs = 0

    Public Property thisRewardId() As Int32
        Get
            Return CInt(hfldCurrentRewardId.Value)
        End Get
        Set(value As Int32)
            hfldCurrentRewardId.Value = value
        End Set
    End Property
    Public Property thisCampaignId() As Int32
        Get
            Return CInt(hfldCurrentCampaignId.Value)
        End Get
        Set(value As Int32)
            hfldCurrentCampaignId.Value = value
        End Set
    End Property
    Public Property activeView() As _viewPanels
        Get
            Return _viewPanel
        End Get
        Set(value As _viewPanels)
            _viewPanel = value
        End Set
    End Property
    Public Property thisTab() As _tabs
        Get
            Return _thisTab
        End Get
        Set(value As _tabs)
            _thisTab = value
        End Set
    End Property
    Public ReadOnly Property featuredImgId As Integer
        Get
            If String.IsNullOrEmpty(hfldMediaId.Value) Then
                Return 0
            Else
                Return CInt(hfldMediaId.Value)
            End If
        End Get
    End Property

    Dim _uHelper As Uhelper = New Uhelper()
#End Region

#Region "Handles"
    Private Sub UserControls_MinorImageSelector_Load(sender As Object, e As EventArgs) Handles Me.PreRender
        Try
            'If Not IsPostBack Then
            'Set active view
            mvFeaturedImageSelector.ActiveViewIndex = activeView()

            'Show data depending on what view panel is being displayed.
            If activeView = _viewPanels.DisplayImg Then
                '
                obtainCurrentImage()
            Else
                'Obtain data for selection panel.
                obtainRewardImages()
            End If

            'End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\MinorImageSelector_Rewards.ascx.vb : UserControls_MinorImageSelector_Load()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Response.Write("<br />Error: " & ex.ToString & "<br />")
        End Try
    End Sub
#End Region

#Region "Methods"
    Private Sub obtainRewardImages()
        Try
            'Instantiate variables
            blMedia = New blMedia
            Dim thisNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(thisCampaignId)

            'Obtain list of images and add to listview
            Dim result As BusinessReturn = blMedia.selectImages_byCampaignId(thisNode, Crops.rewardImage)
            'add images datatable to listview
            If result.isValid Then
                'Add list to Image Library
                lstviewImageLibrary.DataSource = result.DataContainer(0)
                lstviewImageLibrary.DataBind()
            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\MinorImageSelector_Rewards.ascx.vb : obtainRewardImages()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

            'Response.Write("Error: " & ex.ToString)
        End Try
    End Sub
    Private Sub obtainCurrentImage()
        Try
            If thisRewardId <> -1 Then
                'Obtain IPublishedContent
                Dim thisNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(thisRewardId)
                'Does IPublishedContent have a featured image?
                If thisNode.HasProperty(nodeProperties.featuredImage) Then
                    'Save media id to hidden field
                    hfldMediaId.Value = thisNode.GetPropertyValue(Of String)(nodeProperties.featuredImage)
                    'Obtain image's src.
                    selectedImage.ImageUrl = getMediaURL(thisNode.GetPropertyValue(Of String)(nodeProperties.featuredImage), Crops.rewardImage)
                Else
                End If
            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\MinorImageSelector_Rewards.ascx.vb : obtainCurrentImage()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

            'Response.Write("Error: " & ex.ToString)
        End Try
    End Sub
#End Region
End Class