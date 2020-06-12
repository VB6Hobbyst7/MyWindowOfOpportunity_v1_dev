Imports umbraco.Web
Imports Common
Imports umbraco.Core.Models

Partial Class UserControls_TopLevel_Banner
    Inherits System.Web.UI.UserControl

#Region "Properties"
    Private _uHelper As Uhelper = New Uhelper()
    Private Enum views
        home
        standard
        narrow
    End Enum
#End Region

#Region "Handles"
    Private Sub UserControls_TopLevel_Banner_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            'Determine which version of the banner to show.
            Try
                Dim thNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(UmbracoContext.Current.PageId)
                Select Case thNode.DocumentTypeAlias
                    Case docTypes.Home
                        mvBanners.ActiveViewIndex = views.home

                        'Obtain banner images
                        imgHomeBanner.ImageUrl = getMediaURL(thNode.GetPropertyValue(nodeProperties.homeBanner), Crops.bannerHomePage)
                        imgHomeBanner.AlternateText = "My Window of Opportunity"
                        imgHomeBanner.Attributes.Add("title", "My Window of Opportunity")

                    Case docTypes.Campaign, docTypes.Checkout, docTypes.editCampaign, docTypes.Team
                        mvBanners.ActiveViewIndex = views.narrow
                    Case Else
                        mvBanners.ActiveViewIndex = views.standard
                End Select



                'Assign homepage link to main logos.
                hlnkMainLogo_HomePg.NavigateUrl = _uHelper.Get_IPublishedContentByID(siteNodes.Home).Url
                hlnkMainLogo_StandardPg.NavigateUrl = _uHelper.Get_IPublishedContentByID(siteNodes.Home).Url
                hlnkMainLogo_Smaller.NavigateUrl = _uHelper.Get_IPublishedContentByID(siteNodes.Home).Url
                hlnkStartFreeCampaign.NavigateUrl = _uHelper.Get_IPublishedContentByID(siteNodes.CreateACampaign).Url

                'Obtain links to social medias
                Dim thisNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(siteNodes.Home)
                With thisNode
                    'Medium-up
                    If .HasProperty(nodeProperties.facebookUrl) Then hlnkFacebook.NavigateUrl = .GetPropertyValue(Of String)(nodeProperties.facebookUrl) Else hlnkFacebook.Visible = False
                    If .HasProperty(nodeProperties.twitterUrl) Then hlnkTwitter.NavigateUrl = .GetPropertyValue(Of String)(nodeProperties.twitterUrl) Else hlnkTwitter.Visible = False
                    If .HasProperty(nodeProperties.linkedInUrl) Then hlnkLinkedIn.NavigateUrl = .GetPropertyValue(Of String)(nodeProperties.linkedInUrl) Else hlnkLinkedIn.Visible = False
                    'If .HasProperty(nodeProperties.supportEmail) Then hlnkEmail.NavigateUrl += .GetPropertyValue(Of String)(nodeProperties.supportEmail) Else hlnkEmail.Visible = False
                    hlnkEmail.NavigateUrl = _uHelper.Get_IPublishedContentByID(siteNodes.ContactUs).Url

                    'Small-down
                    If .HasProperty(nodeProperties.facebookUrl) Then hlnkFacebook_sm.NavigateUrl = .GetPropertyValue(Of String)(nodeProperties.facebookUrl) Else hlnkFacebook_sm.Visible = False
                    If .HasProperty(nodeProperties.twitterUrl) Then hlnkTwitter_sm.NavigateUrl = .GetPropertyValue(Of String)(nodeProperties.twitterUrl) Else hlnkTwitter_sm.Visible = False
                    If .HasProperty(nodeProperties.linkedInUrl) Then hlnkLinkedIn_sm.NavigateUrl = .GetPropertyValue(Of String)(nodeProperties.linkedInUrl) Else hlnkLinkedIn_sm.Visible = False
                    'If .HasProperty(nodeProperties.supportEmail) Then hlnkEmail_sm.NavigateUrl += .GetPropertyValue(Of String)(nodeProperties.supportEmail) Else hlnkEmail_sm.Visible = False
                    hlnkEmail_sm.NavigateUrl = _uHelper.Get_IPublishedContentByID(siteNodes.ContactUs).Url
                End With
            Catch ex As Exception
                Dim sb As New StringBuilder()
                sb.AppendLine("\UserControls\UserControls_TopLevel_Banner.ascx.vb : UserControls_TopLevel_Banner_Load()")
                saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            End Try
        End If
    End Sub
#End Region

#Region "Methods"
#End Region
End Class
