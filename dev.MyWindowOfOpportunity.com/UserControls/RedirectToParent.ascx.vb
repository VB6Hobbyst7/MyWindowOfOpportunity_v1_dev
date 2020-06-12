Imports umbraco.Web
Imports umbraco.Core.Models

Public Class UserControls_RedirectToParent
    Inherits System.Web.Mvc.ViewUserControl


#Region "Properties"
#End Region

#Region "Handles"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Obtain current node
        Dim ipCurrentNode As IPublishedContent = UmbracoContext.Current.PublishedContentRequest.PublishedContent

        'Do a 301 redirect to new location.
        Response.Status = "301 Moved Permanently"
        Response.AddHeader("Location", ipCurrentNode.Parent.Url)
    End Sub
#End Region

#Region "Methods"
#End Region

End Class