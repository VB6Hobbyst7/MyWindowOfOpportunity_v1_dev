Imports Common
Imports System.Data
Imports umbraco.Core.Models
Imports umbraco.Web

Partial Class Masterpages_List_Page
    Inherits System.Web.UI.MasterPage

#Region "Properties"
    Dim blCampaigns As blCampaigns
    Dim blSiteManagement As blSiteManagement
    Dim linqCampaigns As linqCampaigns
    Dim _uHelper As Uhelper = New Uhelper()

#End Region

#Region "Handles"
    Private Sub Masterpages_List_Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                'Instantiate variables
                Dim thisNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(UmbracoContext.Current.PageId)
                Dim querystringCollection As New NameValueCollection

                'Obtain querystring if present
                If Request.QueryString.HasKeys() Then
                    querystringCollection = HttpUtility.ParseQueryString(Request.QueryString.ToString)
                End If

                'Populate category dropdown and set to category if in querystring 
                PopulateDdl(prevalues.CategoryList, Categories.Categories, ddlCategories)
                If querystringCollection.AllKeys.Contains(queryParameters.Category) Then
                    ddlCategories.SelectedValue = querystringCollection.Get(queryParameters.Category)
                End If

                'Populate Subcategory dropdown and set to subcategory if in querystring 
                PopulateSubcategoryDdl()
                If querystringCollection.AllKeys.Contains(queryParameters.Subcategory) Then
                    ddlSubcategories.SelectedValue = querystringCollection.Get(queryParameters.Subcategory)
                End If


                If querystringCollection.AllKeys.Contains(queryParameters.SortBy) Then
                    ddlSortBy.SelectedValue = querystringCollection.Get(queryParameters.SortBy)
                End If

                ''Populate Sort-by options
                'PopulateSortByDdl()

                'Add proper url for "List All" and reset buttons.
                hlnkListAll.NavigateUrl = thisNode.Url & "?" & queryParameters.ListAll & "=True"
                hlnkReset.NavigateUrl = thisNode.Url

                'Reset buttons
                hlnkListAll.Enabled = True
                hlnkReset.Enabled = True

                'Determine which view to display
                With querystringCollection
                    If .AllKeys.Contains(queryParameters.Subcategory) Then
                        'Display Full List
                        mvLists.SetActiveView(Me.vFullList)
                        'Obtain list by subcategory
                        ObtainSubcategoryList(querystringCollection.Get(queryParameters.Category), querystringCollection.Get(queryParameters.Subcategory))
                        '
                        'ltrlSubcategoryName.Text = UppercaseFirstLetter(ddlSubcategories.SelectedItem.Text.ToLowerInvariant)

                    ElseIf .AllKeys.Contains(queryParameters.Category) Then
                        phSelectedCategory.Visible = True
                        'Display Parital List
                        mvLists.SetActiveView(Me.vPartialList)
                        'Obtain list by specific category
                        ObtainCategoryList(querystringCollection.Get(queryParameters.Category))

                    ElseIf .AllKeys.Contains(queryParameters.ListAll) Then
                        'Obtain all active campaigns
                        ObtainAllList()
                        'Hide pretitle
                        phAllCampaigns.Visible = True
                        phFullList.Visible = False
                        'Disable button
                        hlnkListAll.Enabled = False

                    ElseIf .AllKeys.Contains(queryParameters.Search) Then
                        'Obtain a list by a custom search
                        ObtainCustomList(querystringCollection.Get(queryParameters.Search))
                        '
                        ltrlTitle.Text = "Custom Search"

                    Else
                        phAllCategories.Visible = True
                        'Obtain list by each category
                        ObtainCategories()
                        'Hide pretitle
                        h6PreTitle_partial.Visible = False
                        'Disable button
                        hlnkReset.Enabled = False
                    End If
                End With
            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("/Masterpages/List_Page.master.vb : Masterpages_List_Page_Load()")

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Response.Write("Error: " & ex.ToString)
        End Try
    End Sub
    Private Sub ddlCategories_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCategories.SelectedIndexChanged
        'Redirect using selected category
        If String.IsNullOrEmpty(ddlCategories.SelectedValue) Then
            Response.Redirect(_uHelper.Get_IPublishedContentByID(siteNodes.Campaigns).Url, False)
            HttpContext.Current.ApplicationInstance.CompleteRequest()
        Else
            Response.Redirect(_uHelper.Get_IPublishedContentByID(siteNodes.Campaigns).Url & "?" & queryParameters.Category & "=" & ddlCategories.SelectedValue, False)
            HttpContext.Current.ApplicationInstance.CompleteRequest()
        End If
    End Sub
    Private Sub ddlSubcategories_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlSubcategories.SelectedIndexChanged
        'Redirect using selected category
        If String.IsNullOrWhiteSpace(ddlSubcategories.SelectedValue) Then
            Response.Redirect(_uHelper.Get_IPublishedContentByID(siteNodes.Campaigns).Url & "?" & queryParameters.Category & "=" & ddlCategories.SelectedValue, False)
            HttpContext.Current.ApplicationInstance.CompleteRequest()
        Else
            Response.Redirect(_uHelper.Get_IPublishedContentByID(siteNodes.Campaigns).Url & "?" & queryParameters.Category & "=" & ddlCategories.SelectedValue &
                              "&" & queryParameters.Subcategory & "=" & ddlSubcategories.SelectedValue, False)
            HttpContext.Current.ApplicationInstance.CompleteRequest()
        End If
    End Sub
    Private Sub DdlSortBy_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlSortBy.SelectedIndexChanged
        'Redirect using selected sortby
        If String.IsNullOrWhiteSpace(ddlSortBy.SelectedValue) Then
            Response.Redirect(Request.Url.AbsoluteUri, False)
            HttpContext.Current.ApplicationInstance.CompleteRequest()
        Else
            '
            If Not IsNothing(Request.QueryString(queryParameters.Subcategory)) Then

                Response.Redirect(_uHelper.Get_IPublishedContentByID(siteNodes.Campaigns).Url & "?" & queryParameters.Category & "=" & ddlCategories.SelectedValue &
                                              "&" & queryParameters.Subcategory & "=" & ddlSubcategories.SelectedValue & "&" & queryParameters.SortBy & "=" & ddlSortBy.SelectedValue, False)
                HttpContext.Current.ApplicationInstance.CompleteRequest()

            ElseIf Not IsNothing(Request.QueryString(queryParameters.Category)) Then

                Response.Redirect(_uHelper.Get_IPublishedContentByID(siteNodes.Campaigns).Url & "?" & queryParameters.Category & "=" & ddlCategories.SelectedValue & "&" & queryParameters.SortBy & "=" & ddlSortBy.SelectedValue, False)
                HttpContext.Current.ApplicationInstance.CompleteRequest()

            ElseIf Not IsNothing(Request.QueryString(queryParameters.ListAll)) Then
                Response.Redirect(_uHelper.Get_IPublishedContentByID(siteNodes.Campaigns).Url & "?" & queryParameters.ListAll & "=" & Request.QueryString(queryParameters.ListAll) & "&" & queryParameters.SortBy & "=" & ddlSortBy.SelectedValue, False)
                HttpContext.Current.ApplicationInstance.CompleteRequest()

            Else

                Response.Redirect(_uHelper.Get_IPublishedContentByID(siteNodes.Campaigns).Url & "?" & queryParameters.SortBy & "=" & ddlSortBy.SelectedValue, False)
                HttpContext.Current.ApplicationInstance.CompleteRequest()

            End If

        End If

    End Sub
#End Region

#Region "Methods"
    Private Sub PopulateSubcategoryDdl()
        '
        Select Case ddlCategories.SelectedValue
            Case Categories.Arts
                PopulateDdl(prevalues.Subcategories_Artistic, Miscellaneous.Subcategories, ddlSubcategories)
            Case Categories.Business
                PopulateDdl(prevalues.Subcategories_Business, Miscellaneous.Subcategories, ddlSubcategories)
            Case Categories.Charity
                PopulateDdl(prevalues.Subcategories_Charity, Miscellaneous.Subcategories, ddlSubcategories)
            Case Categories.Community
                PopulateDdl(prevalues.Subcategories_Community, Miscellaneous.Subcategories, ddlSubcategories)
            Case Categories.Science
                PopulateDdl(prevalues.Subcategories_Science, Miscellaneous.Subcategories, ddlSubcategories)
            Case Categories.SelfHelp
                PopulateDdl(prevalues.Subcategories_SelfHelp, Miscellaneous.Subcategories, ddlSubcategories)
            Case Categories.Software
                PopulateDdl(prevalues.Subcategories_Software, Miscellaneous.Subcategories, ddlSubcategories)
            Case Categories.Technology
                PopulateDdl(prevalues.Subcategories_Technology, Miscellaneous.Subcategories, ddlSubcategories)
            Case Else
                'clear ddl data
                ddlSubcategories.DataSource = Nothing
                ddlSubcategories.DataBind()

                'Add a blank entry at top of list.
                ddlSubcategories.Items.Insert(0, New ListItem(Miscellaneous.Subcategories.ToUpper, String.Empty))
                ddlSubcategories.SelectedIndex = 0
                ddlSubcategories.Enabled = False
        End Select
    End Sub
    Private Sub PopulateDdl(ByVal datatypeId As Integer, ByVal ddlName As String, ByRef ddl As DropDownList) 'prevalues.CategoryList, Categories.Categories, ddlCategories
        'Instantiate variables
        Dim categoryListItems As ListItemCollection
        blCampaigns = New blCampaigns

        'Obtain records
        categoryListItems = blCampaigns.obtainCategories(datatypeId)

        'Bind data to list
        ddl.DataSource = categoryListItems
        ddl.DataTextField = "Text"
        ddl.DataValueField = "Value"
        ddl.DataBind()

        'Add a blank entry at top of list.
        ddl.Items.Insert(0, New ListItem(ddlName.ToUpper, String.Empty))
        ddl.SelectedIndex = 0
    End Sub
    Private Sub ObtainCategories()
        'Display Parital List
        mvLists.SetActiveView(Me.vPartialList)

        'Instantiate variables
        'Dim categoryListItems As ListItemCollection
        blCampaigns = New blCampaigns

        'Obtain list of all categories
        'categoryListItems = blCampaigns.obtainCategories(prevalues.CategoryList) 'from DataTypes

        'Obtain list of all active campaign and the categories they belong to.
        Dim returnMsg As BusinessReturn = blCampaigns.selectCampaignsWithCatagories()

        If returnMsg.isValid Then
            'Instantiate variables
            Dim index As UInt16 = 0
            Dim strArray As String
            Dim lstSubcategories As List(Of String)

            'Splice return message into subcategories
            strArray = returnMsg.ReturnMessage

            If Not String.IsNullOrWhiteSpace(strArray) Then
                If strArray.Substring(0, 1) = "," Then strArray = strArray.Remove(0, 1)
                lstSubcategories = strArray.Split(",").ToList

                'Create accordions for each subcategory
                For Each nodeIds As List(Of Integer) In returnMsg.DataContainer
                    Dim accordion As New ASP.Accordion With {
                        .AccordionName = UppercaseFirstLetter(lstSubcategories(index)),
                        .campaignList = nodeIds,
                        .AccordionType = UserControls_Accordion.AccordionTypes.CampaignCategory
                    }
                    phAccordions.Controls.Add(accordion)
                    'Increment to next item.
                    index += 1
                Next
            End If

        Else
            'Display error message
            'Response.Write(returnMsg.ExceptionMessage)
        End If
    End Sub
    Private Sub ObtainCategoryList(ByVal Category As String)
        Try
            'Obtain list by subcategory
            Dim businessReturn As BusinessReturn = blCampaigns.showListBySubcategory(Category)
            If businessReturn.isValid Then
                'Instantiate variables
                Dim index As UInt16 = 0
                Dim strArray As String
                Dim lstSubcategories As List(Of String)

                'Splice return message into subcategories
                strArray = businessReturn.ReturnMessage
                If Not IsNothing(strArray) AndAlso Not String.IsNullOrWhiteSpace(strArray) Then
                    'split csv into list
                    If strArray.Substring(0, 1) = "," Then strArray = strArray.Remove(0, 1)
                    lstSubcategories = strArray.Split(",").ToList

                    'Create accordions for each subcategory
                    For Each lstNodeIds As List(Of Integer) In businessReturn.DataContainer
                        Dim accordion As New ASP.Accordion With {
                            .AccordionName = UppercaseFirstLetter(lstSubcategories(index)),
                            .campaignList = lstNodeIds,
                            .AccordionType = UserControls_Accordion.AccordionTypes.CampaignCategory
                        }
                        phAccordions.Controls.Add(accordion)
                        'Increment to next item.
                        index += 1
                    Next
                Else
                    phNoCampaigns.Visible = True
                End If

                'Update the page title.
                ltrlListName.Text = Category

                'Add the proper image icon for category
                blSiteManagement = New blSiteManagement
                imgSelectedCategory.ImageUrl = blSiteManagement.obtainCategoryIcon_byCategory(Category)
                imgSelectedCategory.AlternateText = Category
            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("List_Page.master.vb : ObtainCategoryList()")
            sb.AppendLine("Category: " & Category)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
    End Sub
    Private Sub ObtainFullCategoryList(ByVal Category As String)
        'Obtain list by subcategory
        Dim businessReturn As BusinessReturn = blCampaigns.showListBySubcategory(Category)
        Dim lstCampaignSummary As List(Of CampaignSummary)
        Dim lst As New List(Of Integer)
        If businessReturn.isValid Then
            'Instantiate variables
            Dim index As UInt16 = 0
            Dim strArray As String
            Dim lstSubcategories As List(Of String)

            'Splice return message into subcategories
            strArray = businessReturn.ReturnMessage
            If strArray.Substring(0, 1) = "," Then strArray = strArray.Remove(0, 1)
            lstSubcategories = strArray.Split(",").ToList

            'Create accordions for each subcategory
            For Each nodeIds As List(Of Integer) In businessReturn.DataContainer
                For Each node As Integer In nodeIds
                    If node > 0 Then
                        If Not lst.Contains(node) Then
                            lst.Add(node)
                        End If
                    End If
                Next
            Next
            'Obtain all campaigns by ID as class
            businessReturn = blCampaigns.selectCampaignsById(lst)

            If businessReturn.isValid Then
                'Extract data from container.
                lstCampaignSummary = blCampaigns.SortBy(businessReturn.DataContainer(0), Request.QueryString(queryParameters.SortBy))
                'Populate listview
                lstviewCampaigns.DataSource = lstCampaignSummary
                lstviewCampaigns.DataBind()
                'Else
                '    'Error occured when creating list of campaigns.
                '    Response.Write(businessReturn.ExceptionMessage)
                '    lbl.Text = "here 1"
            End If
            'Update the page title.
            ltrlListName.Text = Category
            ltrlTitle.Text = Category

        End If
    End Sub
    Private Sub ObtainSubcategoryList(ByVal Category As String, ByVal Subcategory As String)
        'Instantiate variables
        Dim dtCampaigns As DataTable
        Dim BusinessReturn As BusinessReturn
        Dim lstCampaignSummary As List(Of CampaignSummary)
        Dim lst As New List(Of Integer)
        blCampaigns = New blCampaigns

        dtCampaigns = blCampaigns.ObtainPartialList(Category, Subcategory)

        'Loop thru table and obtain node IDs
        For Each dr As DataRow In dtCampaigns.Rows
            lst.Add(dr("nodeId"))
        Next

        'Obtain all campaigns by ID as class
        BusinessReturn = blCampaigns.selectCampaignsById(lst)

        If BusinessReturn.isValid Then
            'Extract data from container.
            lstCampaignSummary = blCampaigns.SortBy(BusinessReturn.DataContainer(0), Request.QueryString(queryParameters.SortBy))

            'Populate listview
            lstviewCampaigns.DataSource = lstCampaignSummary
            lstviewCampaigns.DataBind()

        Else
            'Error occured when creating list of campaigns.
            'Response.Write(BusinessReturn.ExceptionMessage)
        End If

        'Set title and Image
        ltrlTitle.Text = Subcategory
        blSiteManagement = New blSiteManagement
        imgFullList.ImageUrl = blSiteManagement.obtainCategoryIcon_byCategory(Category)

    End Sub
    Private Sub ObtainCustomList(ByVal searchBy As String)
        'Display Full List that match search
        mvLists.SetActiveView(Me.vFullList)

        'Instantiate variables
        Dim BusinessReturn As BusinessReturn
        Dim lstCampaignSummary As New List(Of CampaignSummary)
        Dim lst As New List(Of Integer)

        'Display Full List that match search
        mvLists.SetActiveView(Me.vFullList)

        'Get list of campaigns with thier categories.
        BusinessReturn = blCampaigns.selectCampaignsBySearch(searchBy)

        If BusinessReturn.isValid Then
            'Extract data from container.
            lst = BusinessReturn.DataContainer(0)

            'Get campaigns by Id
            BusinessReturn = blCampaigns.selectCampaignsById(lst)

            If BusinessReturn.isValid Then
                lstCampaignSummary = blCampaigns.SortBy(BusinessReturn.DataContainer(0), Request.QueryString(queryParameters.SortBy))
                'Populate listview
                lstviewCampaigns.DataSource = BusinessReturn.DataContainer(0)
                lstviewCampaigns.DataBind()
            Else
                'Error occured when creating list of campaigns.
                'Response.Write(BusinessReturn.ExceptionMessage)
            End If
        Else
            'Error occured when creating list of campaigns.
            'Response.Write(BusinessReturn.ExceptionMessage)
        End If

        imgFullList.ImageUrl = "~/Images/Icons/search.png"
        imgFullList.AlternateText = "Search"

    End Sub
    Private Sub ObtainAllList()
        'Instantiate variables
        Dim BusinessReturn As BusinessReturn
        Dim lstCampaignSummary As List(Of CampaignSummary)
        Dim lst As New List(Of Integer)

        'Display Full List that match search
        mvLists.SetActiveView(Me.vFullList)

        'Get list of campaigns with thier categories.
        BusinessReturn = blCampaigns.selectAllActiveCampaigns()

        If BusinessReturn.isValid Then
            'Extract data from container.
            lst = BusinessReturn.DataContainer(0)

            'Get campaigns by Id
            BusinessReturn = blCampaigns.selectCampaignsById(lst)

            If BusinessReturn.isValid Then
                'lstCampaignSummary = BusinessReturn.DataContainer(0)
                lstCampaignSummary = blCampaigns.SortBy(BusinessReturn.DataContainer(0), Request.QueryString(queryParameters.SortBy))
                'Populate listview
                lstviewCampaigns.DataSource = lstCampaignSummary
                lstviewCampaigns.DataBind()
            Else
                'Error occured when creating list of campaigns.
                'Response.Write(BusinessReturn.ExceptionMessage)
            End If
        Else
            'Error occured when creating list of campaigns.
            'Response.Write(BusinessReturn.ExceptionMessage)
        End If
    End Sub

#End Region
End Class
