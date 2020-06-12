Imports umbraco.Core.Models

Partial Class UserControls_ulMemberAccts
    Inherits System.Web.UI.UserControl

#Region "Properties"
    Private _nodeId As Integer
    Dim _uHelper As Uhelper = New Uhelper()
    Public Property nodeId() As Integer
        Get
            Return _nodeId
        End Get
        Set(value As Integer)
            _nodeId = value
        End Set
    End Property
#End Region

#Region "Handles"
    Private Sub UserControls_ulMemberAccts_Load(sender As Object, e As EventArgs) Handles Me.Load
        'If Not IsPostBack Then
        'Instantiate variables
        Dim corporateNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(nodeId)
        'Dim ulParent As HtmlGenericControl = New HtmlGenericControl("ul")

        'Obtain corporate info
        hlnkCampaignName.Text = corporateNode.Name
        hlnkCampaignName.NavigateUrl = corporateNode.Url

        '
        hlnkTeamName.Text = corporateNode.Parent.Name
        hlnkTeamName.NavigateUrl = corporateNode.Parent.Url

        ''Obtain all team nodes
        'For Each teamNode As IPublishedContent In corporateNode.Children
        '    If teamNode.NodeTypeAlias = docTypes.Team Then
        '        Dim liTeam As HtmlGenericControl = New HtmlGenericControl("li")
        '        Dim h6Team As HtmlGenericControl = New HtmlGenericControl("h6")
        '        Dim hrefTeam As HyperLink = New HyperLink
        '        Dim ulCampaign As HtmlGenericControl = New HtmlGenericControl("ul")

        '        hrefTeam.Text = teamNode.Name
        '        hrefTeam.NavigateUrl = teamNode.NiceUrl

        '        h6Team.Attributes.Add("class", "teamName")
        '        h6Team.Controls.Add(hrefTeam)
        '        liTeam.Controls.Add(h6Team)


        '        For Each campaignNode As IPublishedContent In teamNode.Children
        '            If campaignNode.NodeTypeAlias = docTypes.Campaign Then
        '                Dim liCampaign As HtmlGenericControl = New HtmlGenericControl("li")
        '                Dim h6Campaign As HtmlGenericControl = New HtmlGenericControl("h6")
        '                Dim hrefCampaign As HyperLink = New HyperLink

        '                hrefCampaign.Text = campaignNode.Name
        '                hrefCampaign.NavigateUrl = campaignNode.NiceUrl

        '                h6Campaign.Attributes.Add("class", "campaignName")
        '                h6Campaign.Controls.Add(hrefCampaign)
        '                liCampaign.Controls.Add(h6Campaign)

        '                ulCampaign.Controls.Add(liCampaign)
        '            End If
        '        Next
        '        liTeam.Controls.Add(ulCampaign)

        '        ulParent.Controls.Add(liTeam)
        '    End If
        'Next

        ''
        'phCampaignList.Controls.Add(ulParent)

        'End If
    End Sub
#End Region

#Region "Methods"
#End Region
End Class
