Imports umbraco
Imports umbraco.Core.Models
Imports umbraco.NodeFactory
Imports umbraco.Web

Partial Class Masterpages_CompiledPage
    Inherits System.Web.UI.MasterPage

#Region "Properties"
    Dim _uHelper As Uhelper = New Uhelper()
#End Region

#Region "Handles"
    Private Sub Masterpages_CompiledPage_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            'Instantiate variables
            Dim thisNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(UmbracoContext.Current.PageId)

            'Populate page with main content

            Dim blPledges As blSiteManagement = New blSiteManagement
            Dim compiledPage As CompiledPage
            compiledPage = blPledges.compiledPage_byNodeId(thisNode.Id)

            'Add data to page
            ltrlTitle.Text = compiledPage.title
            lstvMainContent.DataSource = compiledPage.lstSegments
            lstvMainContent.DataBind()

            lstvSideContent.DataSource = compiledPage.lstSideSegments
            lstvSideContent.DataBind()


            'Dim lstCompiledPages As New List(Of CompiledPage)
            'lstCompiledPages.Add(compiledPage)
            'gv1.DataSource = lstCompiledPages
            'gv1.DataBind()

            'gv3.DataSource = compiledPage.lstSegments
            'gv3.DataBind()

            'gv2.DataSource = compiledPage.lstSideSegments
            'gv2.DataBind()
        End If
    End Sub
#End Region

#Region "Methods"
#End Region

End Class

