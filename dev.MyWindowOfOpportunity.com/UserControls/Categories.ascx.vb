Imports Common
Imports umbraco.Web
Imports umbraco.Core.Models

Partial Class UserControls_Home_Categories
    Inherits System.Web.UI.UserControl


#Region "Properties"
    Dim _uHelper As Uhelper = New Uhelper()
#End Region

#Region "Handles"
    Private Sub UserControls_Home_Categories_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            'Instantiate variables
            Dim thisNode As IPublishedContent = UmbracoContext.Current.PublishedContentRequest.PublishedContent

            'Category 1
            hlnkCategory01.NavigateUrl = _uHelper.Get_IPublishedContentByID(siteNodes.Campaigns).Url & "?Category=" & thisNode.GetPropertyValue(Of String)(nodeProperties.category01).Replace(" ", "")
            imgCategory01.ImageUrl = getMediaURL(thisNode.GetPropertyValue(Of String)(nodeProperties.category01Icon))
            ltrlCategory01.Text = thisNode.GetPropertyValue(Of String)(nodeProperties.category01)

            'Category 2
            hlnkCategory02.NavigateUrl = _uHelper.Get_IPublishedContentByID(siteNodes.Campaigns).Url & "?Category=" & thisNode.GetPropertyValue(Of String)(nodeProperties.category02).Replace(" ", "")
            imgCategory02.ImageUrl = getMediaURL(thisNode.GetPropertyValue(Of String)(nodeProperties.category02Icon))
            ltrlCategory02.Text = thisNode.GetPropertyValue(Of String)(nodeProperties.category02)

            'Category 3
            hlnkCategory03.NavigateUrl = _uHelper.Get_IPublishedContentByID(siteNodes.Campaigns).Url & "?Category=" & thisNode.GetPropertyValue(Of String)(nodeProperties.category03).Replace(" ", "")
            imgCategory03.ImageUrl = getMediaURL(thisNode.GetPropertyValue(Of String)(nodeProperties.category03Icon))
            ltrlCategory03.Text = thisNode.GetPropertyValue(Of String)(nodeProperties.category03)

            'Category 4
            hlnkCategory04.NavigateUrl = _uHelper.Get_IPublishedContentByID(siteNodes.Campaigns).Url & "?Category=" & thisNode.GetPropertyValue(Of String)(nodeProperties.category04).Replace(" ", "")
            imgCategory04.ImageUrl = getMediaURL(thisNode.GetPropertyValue(Of String)(nodeProperties.category04Icon))
            ltrlCategory04.Text = thisNode.GetPropertyValue(Of String)(nodeProperties.category04)

            'Category 5
            hlnkCategory05.NavigateUrl = _uHelper.Get_IPublishedContentByID(siteNodes.Campaigns).Url & "?Category=" & thisNode.GetPropertyValue(Of String)(nodeProperties.category05).Replace(" ", "")
            imgCategory05.ImageUrl = getMediaURL(thisNode.GetPropertyValue(Of String)(nodeProperties.category05Icon))
            ltrlCategory05.Text = thisNode.GetPropertyValue(Of String)(nodeProperties.category05)

            'Category 6
            hlnkCategory06.NavigateUrl = _uHelper.Get_IPublishedContentByID(siteNodes.Campaigns).Url & "?Category=" & thisNode.GetPropertyValue(Of String)(nodeProperties.category06).Replace(" ", "")
            imgCategory06.ImageUrl = getMediaURL(thisNode.GetPropertyValue(Of String)(nodeProperties.category06Icon))
            ltrlCategory06.Text = thisNode.GetPropertyValue(Of String)(nodeProperties.category06)

            'Category 7
            hlnkCategory07.NavigateUrl = _uHelper.Get_IPublishedContentByID(siteNodes.Campaigns).Url & "?Category=" & thisNode.GetPropertyValue(Of String)(nodeProperties.category07).Replace(" ", "")
            imgCategory07.ImageUrl = getMediaURL(thisNode.GetPropertyValue(Of String)(nodeProperties.category07Icon))
            ltrlCategory07.Text = thisNode.GetPropertyValue(Of String)(nodeProperties.category07)

            'Category 8
            hlnkCategory08.NavigateUrl = _uHelper.Get_IPublishedContentByID(siteNodes.Campaigns).Url & "?Category=" & thisNode.GetPropertyValue(Of String)(nodeProperties.category08).Replace(" ", "")
            imgCategory08.ImageUrl = getMediaURL(thisNode.GetPropertyValue(Of String)(nodeProperties.category08Icon))
            ltrlCategory08.Text = thisNode.GetPropertyValue(Of String)(nodeProperties.category08)

            ''Category 9
            'hlnkCategory09.NavigateUrl = New IPublishedContent(siteNodes.Campaigns).NiceUrl & "?Category=" & thisNode.GetPropertyValue(Of String)(nodeProperties.category09).Replace(" ", "")
            'imgCategory09.ImageUrl = getMediaURL(thisNode.GetPropertyValue(Of String)(nodeProperties.category09Icon))
            'ltrlCategory09.Text = thisNode.GetPropertyValue(Of String)(nodeProperties.category09)

            'View All
            hlnkViewAll.NavigateUrl = _uHelper.Get_IPublishedContentByID(siteNodes.Campaigns).Url
            hlnkViewAll_Mbl.NavigateUrl = hlnkViewAll.NavigateUrl
        End If
    End Sub
#End Region

#Region "Methods"
#End Region

End Class
