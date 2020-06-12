Imports Common

Partial Class UserControls_MinorImageSelector_TeamMembers
    Inherits System.Web.UI.UserControl


#Region "Properties"
    Private blMedia As blMedia
    Private blCampaigns As blCampaigns

    'Public Enum _viewPanels
    '    DisplayImg = 0
    '    SelectImg = 1
    'End Enum
    'Private _viewPanel As _viewPanels = 0
    'Public Enum _tabs
    '    Rewards
    '    TeamMembers
    '    Timeline
    '    Phases
    '    Categories
    '    Content
    'End Enum
    'Private _thisTab As _tabs = 0

    Public Property teamMemberId() As Int32
        Get
            Return CInt(hfldTeamMemberId.Value)
        End Get
        Set(value As Int32)
            hfldTeamMemberId.Value = value
        End Set
    End Property
    Public Property photoId() As Int32
        Get
            Return CInt(hfldPhotoId.Value)
        End Get
        Set(value As Int32)
            hfldPhotoId.Value = value
        End Set
    End Property
    'Public Property activeView() As _viewPanels
    '    Get
    '        Return _viewPanel
    '    End Get
    '    Set(value As _viewPanels)
    '        _viewPanel = value
    '    End Set
    'End Property
    'Public Property thisTab() As _tabs
    '    Get
    '        Return _thisTab
    '    End Get
    '    Set(value As _tabs)
    '        _thisTab = value
    '    End Set
    'End Property
    'Public ReadOnly Property featuredImgId As Integer
    '    Get
    '        If String.IsNullOrEmpty(hfldMediaId.Value) Then
    '            Return 0
    '        Else
    '            Return CInt(hfldMediaId.Value)
    '        End If
    '    End Get
    'End Property
#End Region


#Region "Handles"
    Private Sub UserControls_MinorImageSelector_Load(sender As Object, e As EventArgs) Handles Me.PreRender
        Try
            'lbl.Text += teamMemberId.ToString & "; "
            'lbl.Text += photoId.ToString & "; "
            'lbl.Text += getMediaURL(photoId, Crops.members) & "; "


            'Set active view
            'mvFeaturedImageSelector.ActiveViewIndex = activeView()

            'Show data depending on what view panel is being displayed.
            'If activeView = _viewPanels.DisplayImg Then
            '    '
            If teamMemberId <> -1 Then
                '
                obtainCurrentImage()
            End If
            'Else
            '    'Obtain data for selection panel.
            '    obtainRewardImages()
            'End If

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\MinorImageSelector_TeamMembers.ascx.vb : UserControls_MinorImageSelector_Load()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Response.Write("<br />Error: " & ex.ToString & "<br />")
        End Try
    End Sub
#End Region

#Region "Methods"
    Private Sub obtainCurrentImage()
        'Obtain image's src.
        selectedImage.ImageUrl = getMediaURL(photoId, Crops.members)
        selectedImage.AlternateText = getMediaName(photoId)
        selectedImage.Attributes.Add("title", getMediaName(photoId))
    End Sub
#End Region
End Class