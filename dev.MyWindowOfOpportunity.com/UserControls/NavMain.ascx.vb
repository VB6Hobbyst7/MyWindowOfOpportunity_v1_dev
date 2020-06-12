Imports Common
Imports umbraco.Web

Partial Class UserControls_TopLevel_NavMain
    Inherits System.Web.UI.UserControl


#Region "Property"
    Private isEven As Boolean = False
#End Region

#Region "Handles"
    Private Sub UserControls_TopLevel_NavMain_Init(sender As Object, e As EventArgs) Handles Me.Init
        Try   'If Not IsPostBack Then
            'Instantiate variables
            Dim businessReturn As BusinessReturn
            Dim blSiteManagement = New blSiteManagement
            Dim lstNavigationItems As List(Of NavigationItem)
            Dim lstMinorNavItems As New List(Of NavigationItem)

            'Obtain Data
            businessReturn = blSiteManagement.obtainMainNav()

            If businessReturn.isValid Then
                'Extract Data
                lstNavigationItems = DirectCast(businessReturn.DataContainer(0), List(Of NavigationItem))

                'gv.DataSource = lstNavigationItems
                'gv.DataBind()

                'Build menus
                createDesktopMenu(lstNavigationItems)
                createTabletMenu(lstNavigationItems)
                createMobileMenu(lstNavigationItems)

                'gv.DataSource = lstMinorNavItems
                'gv.DataBind()
            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\UserControls_TopLevel_NavMain.ascx.vb : UserControls_TopLevel_NavMain_Init()")

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
        'End If
    End Sub
    Private Sub lbtnLogout_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'Log member out
        Dim blMembers As New blMembers
        blMembers.logMemberOut()

        'Force page to be refreshed to ensure login/out takes effect.
        Response.Redirect(Request.Url.AbsoluteUri, False)
        HttpContext.Current.ApplicationInstance.CompleteRequest()

    End Sub
#End Region

#Region "Methods"
    Private Sub createDesktopMenu(ByVal lstNavigationItems As List(Of NavigationItem))
        Try

            'Instantiate variables
            Dim lstParentNavs As List(Of NavigationItem) = lstNavigationItems.Where(Function(x) x.isParent = True).ToList
            Dim index As UInt16 = 0
            Dim isFirstEntry As Boolean = True

            For Each parentNav As NavigationItem In lstParentNavs
                'Adjust index
                If isFirstEntry Then
                    isFirstEntry = False 'Ensures the first parent is not moved to the wrong placeholder
                Else
                    'Increment index
                    index += 1
                End If


                'Add data from parent to menu panel.
                Select Case index
                    Case 0
                        hlnkCampaign1.NavigateUrl = parentNav.navigationUrl
                        hlnkCampaign1.Text = parentNav.title
                        If Not String.IsNullOrEmpty(parentNav.description) Then hlnkCampaignDesc1.Text = parentNav.description
                    Case 1
                        hlnkInvest1.NavigateUrl = parentNav.navigationUrl
                        hlnkInvest1.Text = parentNav.title
                        If Not String.IsNullOrEmpty(parentNav.description) Then ltrlInvestDesc1.Text = parentNav.description
                    Case 2
                        hlnkMarket1.NavigateUrl = parentNav.navigationUrl
                        hlnkMarket1.Text = parentNav.title
                        If Not String.IsNullOrEmpty(parentNav.description) Then ltrlMarketDesc1.Text = parentNav.description
                End Select


                'Obtain child navs and add to proper panel in main navigation
                Dim columnIndex As UInt16 = 0
                For Each childNav As NavigationItem In lstNavigationItems.Where(Function(x) x.parentNodeId = parentNav.nodeId).ToList

                    Select Case index
                        Case 0
                            Select Case columnIndex Mod 3
                                Case 0
                                    createMenuLink(childNav, pnlCampaignLinks_1)
                                Case 1
                                    createMenuLink(childNav, pnlCampaignLinks_2)
                                Case 2
                                    createMenuLink(childNav, pnlCampaignLinks_3)
                            End Select

                        Case 1
                            Select Case columnIndex Mod 3
                                Case 0
                                    createMenuLink(childNav, pnlInvestLinks_1)
                                Case 1
                                    createMenuLink(childNav, pnlInvestLinks_2)
                                Case 2
                                    createMenuLink(childNav, pnlInvestLinks_3)
                            End Select

                        Case 2
                            Select Case columnIndex Mod 3
                                Case 0
                                    createMenuLink(childNav, pnlMarketLinks_1)
                                Case 1
                                    createMenuLink(childNav, pnlMarketLinks_2)
                                Case 2
                                    createMenuLink(childNav, pnlMarketLinks_3)
                            End Select
                    End Select

                    columnIndex += 1
                Next
            Next
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\UserControls_TopLevel_NavMain.ascx.vb : createDesktopMenu()")

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
    End Sub
    Private Sub createMenuLink(ByRef childNav As NavigationItem, ByRef currentPnl As Panel)
        Try
            'Instantiate variables
            Dim hlnk As HyperLink = New HyperLink
            Dim pnl As Panel = New Panel
            'Add data to link
            hlnk.NavigateUrl = childNav.navigationUrl
            hlnk.Text = childNav.title
            'Add link to panel
            pnl.Controls.Add(hlnk)
            currentPnl.Controls.Add(pnl)
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\UserControls_TopLevel_NavMain.ascx.vb : createMenuLink()")

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
    End Sub

    Private Sub createTabletMenu(ByVal lstNavigationItems As List(Of NavigationItem))
        Try
            'Instantiate variables
            Dim ulParent As HtmlGenericControl = New HtmlGenericControl("ul")
            Dim liParent As HtmlGenericControl
            Dim ulChild As HtmlGenericControl
            Dim liChild As HtmlGenericControl
            Dim href As HtmlGenericControl
            Dim lbl As HtmlGenericControl = New HtmlGenericControl("label")
            Dim childUlAdded As Boolean = False

            'Initialize variables
            ulParent.Attributes.Add("class", "off-canvas-list")

            'Add label to menu
            liParent = New HtmlGenericControl("li")
            liParent.Controls.Add(lbl)
            ulParent.Controls.Add(liParent)

            lbl.InnerText = "Learn More"

            For Each navItem As NavigationItem In lstNavigationItems
                If navItem.isParent Then
                    'Reset boolean
                    childUlAdded = False
                    'Create new li and add to parent
                    liParent = New HtmlGenericControl("li")
                    ulParent.Controls.Add(liParent)
                    'Add title
                    href = New HtmlGenericControl("a")
                    href.Attributes.Add("href", "/")
                    href.InnerText = navItem.title
                    liParent.Controls.Add(href)

                Else
                    If childUlAdded = False Then
                        '
                        liParent.Attributes.Add("class", "has-submenu")
                        'Create new sub-menu
                        ulChild = New HtmlGenericControl("ul")
                        ulChild.Attributes.Add("class", "left-submenu")
                        liParent.Controls.Add(ulChild)
                        'Create a back button
                        liChild = New HtmlGenericControl("li")
                        href = New HtmlGenericControl("a")
                        liChild.Controls.Add(href)
                        ulChild.Controls.Add(liChild)
                        liChild.Attributes.Add("class", "back")
                        href.Attributes.Add("href", "#")
                        href.InnerText = "Back"
                        'Set bool
                        childUlAdded = True
                    End If

                    'Create new link and add to parent control
                    liChild = New HtmlGenericControl("li")
                    href = New HtmlGenericControl("a")
                    liChild.Controls.Add(href)
                    ulChild.Controls.Add(liChild)
                    'Add navigation and text to link
                    href.Attributes.Add("href", navItem.navigationUrl)
                    href.InnerText = navItem.title

                End If
            Next

            phTableMenu.Controls.Add(ulParent)

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\UserControls_TopLevel_NavMain.ascx.vb : createTabletMenu()")

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
    End Sub
    Private Sub createMobileMenu(ByVal lstNavigationItems As List(Of NavigationItem))
        Try 'Instantiate variables
            Dim ulParent As HtmlGenericControl = New HtmlGenericControl("ul")
            Dim liParent As HtmlGenericControl
            Dim ulChild As HtmlGenericControl
            Dim liChild As HtmlGenericControl
            Dim href As HtmlGenericControl
            Dim lbl As HtmlGenericControl
            Dim childUlAdded As Boolean = False

            'Initialize variables
            ulParent.Attributes.Add("class", "off-canvas-list")

            'ADD MAIN MENU
            '============================================================
            '===
            'Instantiate umbraco helper
            Dim uHelper As UmbracoHelper = New umbraco.Web.UmbracoHelper(umbraco.Web.UmbracoContext.Current)

            'Add label to menu
            liParent = New HtmlGenericControl("li")
            ulParent.Controls.Add(liParent)

            lbl = New HtmlGenericControl("label") With {
                .InnerText = "Main Menu"
            }
            liParent.Controls.Add(lbl)


            'Determine what to display for the login/out options.
            Dim blMembers As New blMembers
            Dim isMemberLoggedIn As Boolean = blMembers.isMemberLoggedIn
            If isMemberLoggedIn Then

                Dim lbtn As New LinkButton With {
                    .Text = "Logout",
                    .ID = "lbtnLogout"
                }
                AddHandler lbtn.Click, AddressOf Me.lbtnLogout_Click
                liParent.Controls.Add(lbtn)

                href = New HtmlGenericControl("a")
                href.Attributes.Add("href", uHelper.TypedContent(siteNodes.CreateACampaign).Url)
                href.InnerText = "New Campaign"
                liParent.Controls.Add(href)

                href = New HtmlGenericControl("a")
                href.Attributes.Add("href", uHelper.TypedContent(siteNodes.ManageCampaigns).Url)
                href.InnerText = "Manage Campaigns"
                liParent.Controls.Add(href)

                href = New HtmlGenericControl("a")
                href.Attributes.Add("href", uHelper.TypedContent(siteNodes.EditAccount).Url)
                href.InnerText = "Edit Account"
                liParent.Controls.Add(href)

            Else
                href = New HtmlGenericControl("a")
                href.Attributes.Add("href", uHelper.TypedContent(siteNodes.Login).Url)
                href.InnerText = "Login"
                liParent.Controls.Add(href)

                href = New HtmlGenericControl("a")
                href.Attributes.Add("href", uHelper.TypedContent(siteNodes.BecomeAMember).Url)
                href.InnerText = "Become A Member"
                liParent.Controls.Add(href)

                href = New HtmlGenericControl("a")
                href.Attributes.Add("href", uHelper.TypedContent(siteNodes.ContactUs).Url)
                href.InnerText = "Contact Us"
                liParent.Controls.Add(href)

            End If
            '===
            '============================================================



            'ADD LEARN MORE MENU
            '============================================================
            '===
            'Add label to menu
            liParent = New HtmlGenericControl("li")
            lbl = New HtmlGenericControl("label")
            liParent.Controls.Add(lbl)
            ulParent.Controls.Add(liParent)

            lbl.InnerText = "Learn More"

            For Each navItem As NavigationItem In lstNavigationItems
                If navItem.isParent Then
                    'Reset boolean
                    childUlAdded = False
                    'Create new li and add to parent
                    liParent = New HtmlGenericControl("li")
                    ulParent.Controls.Add(liParent)
                    'Add title
                    href = New HtmlGenericControl("a")
                    href.Attributes.Add("href", "/")
                    href.InnerText = navItem.title
                    liParent.Controls.Add(href)

                Else
                    If childUlAdded = False Then
                        '
                        liParent.Attributes.Add("class", "has-submenu")
                        'Create new sub-menu
                        ulChild = New HtmlGenericControl("ul")
                        ulChild.Attributes.Add("class", "right-submenu")
                        liParent.Controls.Add(ulChild)
                        'Create a back button
                        liChild = New HtmlGenericControl("li")
                        href = New HtmlGenericControl("a")
                        liChild.Controls.Add(href)
                        ulChild.Controls.Add(liChild)
                        liChild.Attributes.Add("class", "back")
                        href.Attributes.Add("href", "#")
                        href.InnerText = "Back"
                        'Set bool
                        childUlAdded = True
                    End If

                    'Create new link and add to parent control
                    liChild = New HtmlGenericControl("li")
                    href = New HtmlGenericControl("a")
                    liChild.Controls.Add(href)
                    ulChild.Controls.Add(liChild)
                    'Add navigation and text to link
                    href.Attributes.Add("href", navItem.navigationUrl)
                    href.InnerText = navItem.title

                End If
            Next
            '===
            '============================================================

            phMobileMenu.Controls.Add(ulParent)
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\UserControls_TopLevel_NavMain.ascx.vb : createMobileMenu()")

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
    End Sub
#End Region
End Class







'Private Sub createMobileMenu(ByVal lstNavigationItems As List(Of NavigationItem))
'    'Instantiate variables
'    Dim ulParent As HtmlGenericControl = New HtmlGenericControl("ul")
'    Dim liParent As HtmlGenericControl
'    Dim ulChild As HtmlGenericControl
'    Dim liChild As HtmlGenericControl
'    Dim href As HtmlGenericControl
'    Dim lbl As HtmlGenericControl
'    Dim childUlAdded As Boolean = False

'    'Initialize variables
'    ulParent.Attributes.Add("class", "off-canvas-list")

'    'Add label to menu
'    liParent = New HtmlGenericControl("li")
'    lbl = New HtmlGenericControl("label")
'    liParent.Controls.Add(lbl)
'    ulParent.Controls.Add(liParent)

'    lbl.InnerText = "Learn More"

'    For Each navItem As NavigationItem In lstNavigationItems
'        If navItem.isParent Then
'            'Reset boolean
'            childUlAdded = False
'            'Create new li and add to parent
'            liParent = New HtmlGenericControl("li")
'            ulParent.Controls.Add(liParent)
'            'Add title
'            href = New HtmlGenericControl("a")
'            href.Attributes.Add("href", "/")
'            href.InnerText = navItem.title
'            liParent.Controls.Add(href)

'        Else
'            If childUlAdded = False Then
'                '
'                liParent.Attributes.Add("class", "has-submenu")
'                'Create new sub-menu
'                ulChild = New HtmlGenericControl("ul")
'                ulChild.Attributes.Add("class", "right-submenu")
'                liParent.Controls.Add(ulChild)
'                'Create a back button
'                liChild = New HtmlGenericControl("li")
'                href = New HtmlGenericControl("a")
'                liChild.Controls.Add(href)
'                ulChild.Controls.Add(liChild)
'                liChild.Attributes.Add("class", "back")
'                href.Attributes.Add("href", "#")
'                href.InnerText = "Back"
'                'Set bool
'                childUlAdded = True
'            End If

'            'Create new link and add to parent control
'            liChild = New HtmlGenericControl("li")
'            href = New HtmlGenericControl("a")
'            liChild.Controls.Add(href)
'            ulChild.Controls.Add(liChild)
'            'Add navigation and text to link
'            href.Attributes.Add("href", navItem.navigationUrl)
'            href.InnerText = navItem.title

'        End If
'    Next

'    phMobileMenu.Controls.Add(ulParent)





'    Dim uHelper As UmbracoHelper = New Umbraco.Web.UmbracoHelper(Umbraco.Web.UmbracoContext.Current)
'    Dim ipContent As IPublishedContent

'    ''Obtain parent IPublishedContent
'    'ipContent = uHelper.TypedContent(parentNode.Id)


'    'Add label to menu
'    liParent = New HtmlGenericControl("li")
'    ulParent.Controls.Add(liParent)

'    lbl = New HtmlGenericControl("label")
'    lbl.InnerText = "Main Menu"
'    liParent.Controls.Add(lbl)




'    'Determine what to display for the login/out options.
'    Dim blMembers As New blMembers
'    Dim isMemberLoggedIn As Boolean = blMembers.isMemberLoggedIn
'    If isMemberLoggedIn Then

'        Dim lbtn As New LinkButton
'        lbtn.Text = "Logout"
'        lbtn.ID = "lbtnLogout"
'        AddHandler lbtn.Click, AddressOf Me.lbtnLogout_Click
'        liParent.Controls.Add(lbtn)

'        href = New HtmlGenericControl("a")
'        href.Attributes.Add("href", uHelper.TypedContent(siteNodes.CreateACampaign).Url)
'        href.InnerText = "New Campaign"
'        liParent.Controls.Add(href)

'        href = New HtmlGenericControl("a")
'        href.Attributes.Add("href", uHelper.TypedContent(siteNodes.ManageCampaigns).Url)
'        href.InnerText = "Manage Campaigns"
'        liParent.Controls.Add(href)

'        href = New HtmlGenericControl("a")
'        href.Attributes.Add("href", uHelper.TypedContent(siteNodes.EditAccount).Url)
'        href.InnerText = "Edit Account"
'        liParent.Controls.Add(href)

'    Else
'        href = New HtmlGenericControl("a")
'        href.Attributes.Add("href", uHelper.TypedContent(siteNodes.Login).Url)
'        href.InnerText = "Login"
'        liParent.Controls.Add(href)

'        href = New HtmlGenericControl("a")
'        href.Attributes.Add("href", uHelper.TypedContent(siteNodes.BecomeAMember).Url)
'        href.InnerText = "Become A Member"
'        liParent.Controls.Add(href)

'        href = New HtmlGenericControl("a")
'        href.Attributes.Add("href", uHelper.TypedContent(siteNodes.ContactUs).Url)
'        href.InnerText = "Contact Us"
'        liParent.Controls.Add(href)

'    End If
'End Sub