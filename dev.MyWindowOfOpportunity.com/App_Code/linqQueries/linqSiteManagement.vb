Imports umbraco
Imports Common
Imports umbraco.Core.Models
Imports Examine
Imports umbraco.Web
Imports Newtonsoft.Json
Imports umbraco.Core.Services
Imports umbraco.Core
Imports Stripe

Public Class linqSiteManagement

#Region "Properties"
    'Private linq2Db As linq2SqlDataContext = New linq2SqlDataContext(ConfigurationManager.ConnectionStrings("umbracoDbDSN").ToString)
    Dim _uHelper As Uhelper = New Uhelper()

#End Region

#Region "Selects"
    Public Function compiledPage_byNodeId(ByVal _nodeId As Integer) As CompiledPage
        'Instantiate variables
        Dim compiledNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(_nodeId)

        Dim segment As Segment
        Dim compiledPage As CompiledPage = New CompiledPage
        Dim lstSegment As New List(Of Segment)
        Dim lstSideSegment As New List(Of Segment)
        compiledPage.nodeId = _nodeId
        compiledPage.title = compiledNode.GetPropertyValue(Of String)(nodeProperties.title)
        compiledPage.description = compiledNode.GetPropertyValue(Of String)(nodeProperties.description)

        ' compiledPage.title =compiledNode.
        Try
            For Each item As IPublishedContent In compiledNode.Children
                'Instantiate variables
                'Obtain data
                segment = New Segment With {
                    .nodeId = item.Id,
                    .heading = item.GetPropertyValue(Of String)(nodeProperties.heading),
                    .content = item.GetPropertyValue(Of String)(nodeProperties.bookmarkContent),
                    .showOnSide = item.GetPropertyValue(Of Boolean)(nodeProperties.showOnSide)
                }

                Dim bookmarkID As String = item.UrlName.Replace("-", "")
                segment.anchor = bookmarkID
                'Add to proper list
                If segment.showOnSide Then
                    lstSideSegment.Add(segment)
                Else
                    lstSegment.Add(segment)
                End If
            Next
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqSiteManagement.vb : compiledPage_byNodeId()")
            sb.AppendLine("_nodeId:" & _nodeId)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try

        compiledPage.lstSegments = lstSegment
        compiledPage.lstSideSegments = lstSideSegment

        Return compiledPage
    End Function
    Public Function obtainFooterNav() As BusinessReturn
        'Scope Variables
        Dim businessReturn As New BusinessReturn

        Try
            'Instantiate variables
            Dim navigationItem As NavigationItem
            Dim lstNavigationItem As New List(Of NavigationItem)
            Dim ipContent As IPublishedContent


            'USE EXAMINE.EXAMINEMANAGER TO SEARCH   https://our.umbraco.org/documentation/reference/searching/examine/overview-explanation#querying-with-examine
            '===============================================
            'Instantiate search provider and criteria
            Dim searchProvider As Providers.BaseSearchProvider = Examine.ExamineManager.Instance.SearchProviderCollection(searchIndex.NavigationSearcher)
            Dim searchCriteria As SearchCriteria.ISearchCriteria = searchProvider.CreateSearchCriteria()

            'Obtain all marked items
            Dim queryParents As Examine.SearchCriteria.IBooleanOperation = searchCriteria.Field(searchField.showInFooter, CInt(True)).[And]().Field(searchField.level, 2).[And]().OrderBy(searchField.sortOrder & "[Type=INT]")
            Dim searchResultParent As ISearchResults = searchProvider.Search(queryParents.Compile())

            'Loop thru each id and build link list
            For Each parentNode As Examine.SearchResult In searchResultParent
                'Instantiate variables
                navigationItem = New NavigationItem
                ipContent = _uHelper.Get_IPublishedContentByID(parentNode.Id)

                'Populate class
                navigationItem.nodeId = parentNode.Id
                navigationItem.parentNodeId = parentNode.Fields(searchField.parentID)
                navigationItem.title = parentNode.Fields(searchField.nodeName)
                navigationItem.sortOrder = parentNode.Fields(searchField.sortOrder)
                navigationItem.isParent = True
                navigationItem.navigationUrl = ipContent.Url
                navigationItem.level = parentNode.Fields(searchField.level)

                'Add class to list
                lstNavigationItem.Add(navigationItem)

                'OBTAIN CHILDREN
                '==========================================
                'Obtain all marked items
                Dim searchCriteriaChildren As SearchCriteria.ISearchCriteria = searchProvider.CreateSearchCriteria()
                Dim queryChildren As Examine.SearchCriteria.IBooleanOperation = searchCriteriaChildren.Field(searchField.showInFooter, CInt(True)).[And]().Field(searchField.level, 3).[And]().Field(searchField.parentID, parentNode.Id).[And]().OrderBy(searchField.sortOrder & "[Type=INT]")
                Dim searchResultChildren As ISearchResults = searchProvider.Search(queryChildren.Compile())

                'Loop thru each id and build link list
                For Each childNode As Examine.SearchResult In searchResultChildren
                    'Instantiate variables
                    navigationItem = New NavigationItem
                    ipContent = _uHelper.Get_IPublishedContentByID(childNode.Id)

                    'Populate class
                    navigationItem.nodeId = childNode.Id
                    navigationItem.parentNodeId = childNode.Fields(searchField.parentID)
                    navigationItem.sortOrder = childNode.Fields(searchField.sortOrder)
                    navigationItem.isParent = False
                    navigationItem.level = childNode.Fields(searchField.level)

                    Select Case ipContent.DocumentTypeAlias
                        Case docTypes.navigationLink
                            Dim ipNavigateTo As IPublishedContent = ipContent.GetPropertyValue(Of IPublishedContent)(nodeProperties.navigateTo)
                            navigationItem.navigationUrl = ipNavigateTo.Url
                            navigationItem.title = ipContent.GetPropertyValue(Of String)(nodeProperties.title)
                        Case Else 'docTypes.segment
                            'Obtain parent url and add anchor to current url
                            navigationItem.navigationUrl = ipContent.Parent.Url & "#" & ipContent.UrlName.Replace("-", "")
                            navigationItem.title = childNode.Fields(searchField.nodeName)
                    End Select

                    'Add class to list
                    lstNavigationItem.Add(navigationItem)
                Next
            Next

            businessReturn.DataContainer.Add(lstNavigationItem)

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqSiteManagement.vb : obtainFooterNav()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try

        Return businessReturn
    End Function
    Public Function obtainMainNav() As BusinessReturn
        'Scope Variables
        Dim businessReturn As New BusinessReturn

        Try
            'Instantiate variables
            Dim lstNavigationItem As New List(Of NavigationItem)
            'Dim uHelper As UmbracoHelper = New Umbraco.Web.UmbracoHelper(Umbraco.Web.UmbracoContext.Current)
            'Dim ipContent As IPublishedContent
            'Dim navigationItem As NavigationItem


            'USE EXAMINE.EXAMINEMANAGER TO SEARCH   https://our.umbraco.org/documentation/reference/searching/examine/overview-explanation#querying-with-examine
            '===============================================
            'Instantiate search provider and criteria
            Dim searchProvider As Providers.BaseSearchProvider = Examine.ExamineManager.Instance.SearchProviderCollection(searchIndex.NavigationSearcher)
            Dim searchCriteria As SearchCriteria.ISearchCriteria = searchProvider.CreateSearchCriteria()

            'Obtain all marked items
            Dim queryParents As Examine.SearchCriteria.IBooleanOperation = searchCriteria.Field(searchField.showInNavigation, CInt(True)).[And]().Field(searchField.level, 2).[And]().OrderBy(searchField.sortOrder & "[Type=INT]")
            Dim searchResultParent As ISearchResults = searchProvider.Search(queryParents.Compile())

            'Loop thru each id and build link list
            For Each parentNode As Examine.SearchResult In searchResultParent
                'Add class to list
                lstNavigationItem.Add(createParentNav(parentNode))


                'OBTAIN CHILDREN
                '==========================================
                'Obtain all marked items
                Dim searchCriteriaChildren As SearchCriteria.ISearchCriteria = searchProvider.CreateSearchCriteria()
                Dim queryChildren As Examine.SearchCriteria.IBooleanOperation = searchCriteriaChildren.Field(searchField.showInNavigation, CInt(True)).[And]().Field(searchField.level, 3).[And]().Field(searchField.parentID, parentNode.Id).[And]().OrderBy(searchField.sortOrder & "[Type=INT]")
                Dim searchResultChildren As ISearchResults = searchProvider.Search(queryChildren.Compile())

                'Loop thru each id and build link list
                For Each childNode As Examine.SearchResult In searchResultChildren
                    'Add class to list
                    lstNavigationItem.Add(createChildNav(childNode))
                Next
            Next

            'Save navigation
            businessReturn.DataContainer.Add(lstNavigationItem)

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqSiteManagement.vb : obtainMainNav()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try

        Return businessReturn
    End Function

    Public Function obtainAllCategoryIcons() As BusinessReturn
        'Instantiate scope variabes
        Dim businessReturn As New BusinessReturn

        Try
            'Instantiate variables
            Dim categoryIcon As CategoryIcon
            Dim lstCategoryIcons As New List(Of CategoryIcon)
            Dim ipContent As IPublishedContent = _uHelper.Get_IPublishedContentByID(siteNodes.Home)


            'Category 1
            categoryIcon = New CategoryIcon With {
                .categoryIconUrl = getMediaURL(ipContent.GetPropertyValue(Of String)(nodeProperties.category01Icon)),
                .categoryName = ipContent.GetPropertyValue(Of String)(nodeProperties.category01)
            }
            lstCategoryIcons.Add(categoryIcon)

            'Category 2
            categoryIcon = New CategoryIcon With {
                .categoryIconUrl = getMediaURL(ipContent.GetPropertyValue(Of String)(nodeProperties.category02Icon)),
                .categoryName = ipContent.GetPropertyValue(Of String)(nodeProperties.category02)
            }
            lstCategoryIcons.Add(categoryIcon)

            'Category 3
            categoryIcon = New CategoryIcon With {
                .categoryIconUrl = getMediaURL(ipContent.GetPropertyValue(Of String)(nodeProperties.category03Icon)),
                .categoryName = ipContent.GetPropertyValue(Of String)(nodeProperties.category03)
            }
            lstCategoryIcons.Add(categoryIcon)

            'Category 4
            categoryIcon = New CategoryIcon With {
                .categoryIconUrl = getMediaURL(ipContent.GetPropertyValue(Of String)(nodeProperties.category04Icon)),
                .categoryName = ipContent.GetPropertyValue(Of String)(nodeProperties.category04)
            }
            lstCategoryIcons.Add(categoryIcon)

            'Category 5
            categoryIcon = New CategoryIcon With {
                .categoryIconUrl = getMediaURL(ipContent.GetPropertyValue(Of String)(nodeProperties.category05Icon)),
                .categoryName = ipContent.GetPropertyValue(Of String)(nodeProperties.category05)
            }
            lstCategoryIcons.Add(categoryIcon)

            'Category 6
            categoryIcon = New CategoryIcon With {
                .categoryIconUrl = getMediaURL(ipContent.GetPropertyValue(Of String)(nodeProperties.category06Icon)),
                .categoryName = ipContent.GetPropertyValue(Of String)(nodeProperties.category06)
            }
            lstCategoryIcons.Add(categoryIcon)

            'Category 7
            categoryIcon = New CategoryIcon With {
                .categoryIconUrl = getMediaURL(ipContent.GetPropertyValue(Of String)(nodeProperties.category07Icon)),
                .categoryName = ipContent.GetPropertyValue(Of String)(nodeProperties.category07)
            }
            lstCategoryIcons.Add(categoryIcon)

            'Category 8
            categoryIcon = New CategoryIcon With {
                .categoryIconUrl = getMediaURL(ipContent.GetPropertyValue(Of String)(nodeProperties.category08Icon)),
                .categoryName = ipContent.GetPropertyValue(Of String)(nodeProperties.category08)
            }
            lstCategoryIcons.Add(categoryIcon)

            'Return data
            businessReturn.DataContainer.Add(lstCategoryIcons)
            Return businessReturn

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("blSiteManagement.vb : obtainAllCategoryIcons()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

            businessReturn.ExceptionMessage = ex.ToString
            Return businessReturn
        End Try
    End Function
#End Region

#Region "Inserts"
    Public Sub createHookMsg(ByVal parentId As Integer, ByVal doctype As String, ByVal jsonMsg As String, ByVal eventType As String)
        Try
            'Create a new node
            Dim cs As IContentService = ApplicationContext.Current.Services.ContentService
            Dim icParent As IContent = cs.GetById(parentId)
            Dim stripeEvent = StripeEventUtility.ParseEvent(jsonMsg)

            Dim timeStamp As DateTime = DateTime.Now.AddHours(3)
            If Not IsNothing(stripeEvent.Created) Then timeStamp = stripeEvent.Created

            Dim icHook As IContent = cs.CreateContentWithIdentity(timeStamp.ToString & " | " & eventType, icParent, doctype)

            'Set values
            icHook.SetValue(nodeProperties.receivedOn, stripeEvent.Created)
            icHook.SetValue(nodeProperties.hookMessage, jsonMsg)

            'Save values
            cs.SaveAndPublishWithStatus(icHook)

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("blSiteManagement.vb : parseHookMsg()")
            sb.AppendLine("parentId:" & parentId)
            sb.AppendLine("doctype:" & doctype)
            sb.AppendLine("eventType:" & eventType)
            sb.AppendLine("jsonMsg:")
            sb.AppendLine(jsonMsg)
            saveErrorMessage(-1, ex.Message, sb.ToString())
        End Try
    End Sub
#End Region

#Region "Private Methods"
    Private Function createParentNav(ByVal parentNode As Examine.SearchResult) As NavigationItem
        Try
            'Instantiate variables
            Dim navigationItem As NavigationItem = New NavigationItem
            Dim ipContent As IPublishedContent

            'Obtain parent IPublishedContent
            ipContent = _uHelper.Get_IPublishedContentByID(parentNode.Id)

            'Populate class
            navigationItem.nodeId = parentNode.Id
            navigationItem.parentNodeId = parentNode.Fields(searchField.parentID)
            navigationItem.title = parentNode.Fields("title") 'Newtonsoft.Json.JsonConvert.SerializeObject(parentNode) ' 'parentNode.Fields(searchField.nodeName)
            navigationItem.sortOrder = parentNode.Fields(searchField.sortOrder)
            navigationItem.isParent = True
            navigationItem.navigationUrl = ipContent.Url
            navigationItem.level = parentNode.Fields(searchField.level)
            If ipContent.HasProperty(nodeProperties.description) Then navigationItem.description = ipContent.GetPropertyValue(nodeProperties.description)

            Return navigationItem
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqSiteManagement.vb : createParentNav()")
            sb.AppendLine("parentNode:" & JsonConvert.SerializeObject(parentNode))
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return Nothing
        End Try
    End Function
    Private Function createChildNav(ByVal childNode As Examine.SearchResult) As NavigationItem
        'Instantiate variables
        Dim navigationItem As NavigationItem = New NavigationItem

        Try
            Dim ipContent As IPublishedContent

            'Obtain parent IPublishedContent
            ipContent = _uHelper.Get_IPublishedContentByID(childNode.Id)

            'Populate class
            navigationItem.nodeId = childNode.Id
            navigationItem.parentNodeId = childNode.Fields(searchField.parentID)
            navigationItem.sortOrder = childNode.Fields(searchField.sortOrder)
            navigationItem.isParent = False
            navigationItem.level = childNode.Fields(searchField.level)

            Select Case ipContent.DocumentTypeAlias
                Case docTypes.navigationLink
                    Dim ipNavigateTo As IPublishedContent = ipContent.GetPropertyValue(Of IPublishedContent)(nodeProperties.navigateTo)
                    navigationItem.navigationUrl = ipNavigateTo.Url
                    navigationItem.title = ipContent.GetPropertyValue(Of String)(nodeProperties.title)
                Case docTypes.segment
                    'Obtain parent url and add anchor to current url
                    navigationItem.navigationUrl = ipContent.Parent.Url & "#" & ipContent.UrlName.Replace("-", "")
                    navigationItem.title = childNode.Fields(searchField.nodeName)
            End Select

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqSiteManagement.vb : createChildNav()")
            sb.AppendLine("parentNode:" & JsonConvert.SerializeObject(childNode))
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

        End Try
        Return navigationItem
    End Function
#End Region
End Class

