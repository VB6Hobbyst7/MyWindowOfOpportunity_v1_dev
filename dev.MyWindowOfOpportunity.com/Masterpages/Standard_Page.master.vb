Imports umbraco
Imports umbraco.NodeFactory
Imports umbraco.Core.Models
Imports umbraco.Web

Partial Class Masterpages_Standard_Page
    Inherits System.Web.UI.MasterPage

#Region "Properties"
    Dim _uHelper As Uhelper = New Uhelper()
#End Region

#Region "Handles"
    Private Sub Masterpages_Standard_Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            'Instantiate variables
            Dim thisNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(UmbracoContext.Current.PageId)

            'Populate page with main content
            ltrlTitle.Text = thisNode.Name
        End If
    End Sub
#End Region

#Region "Methods"
#End Region
End Class

