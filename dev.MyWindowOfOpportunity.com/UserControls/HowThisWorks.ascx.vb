Imports Common

Partial Class UserControls_Home_HowThisWorks
    Inherits System.Web.UI.UserControl

#Region "Properties"
    Dim _uHelper As Uhelper = New Uhelper()
#End Region

#Region "Handles"
    Private Sub UserControls_Home_HowThisWorks_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            'Add url link
            hlnkStartCampaign.NavigateUrl = _uHelper.Get_IPublishedContentByID(siteNodes.CreateACampaign).Url

        End If
    End Sub
#End Region

#Region "Methods"
#End Region
End Class
