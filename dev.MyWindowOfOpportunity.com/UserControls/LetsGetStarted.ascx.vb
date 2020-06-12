Imports Common

Partial Class UserControls_Home_LetsGetStarted
    Inherits System.Web.UI.UserControl

#Region "Properties"
    Dim _uHelper As Uhelper = New Uhelper()
#End Region

#Region "Handles"
    Private Sub UserControls_Home_LetsGetStarted_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            'Add url links
            hlnkCreateCampaign.NavigateUrl = _uHelper.Get_IPublishedContentByID(siteNodes.Campaign).Url
            hlnkInvestment.NavigateUrl = _uHelper.Get_IPublishedContentByID(siteNodes.Invest).Url
            hlnkProduction.NavigateUrl = _uHelper.Get_IPublishedContentByID(siteNodes.Market).Url
        End If
    End Sub
#End Region

#Region "Methods"
#End Region
End Class
