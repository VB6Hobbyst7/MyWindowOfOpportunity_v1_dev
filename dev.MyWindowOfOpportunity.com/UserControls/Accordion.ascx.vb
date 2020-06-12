Imports Common
Imports System.Data
Imports umbraco.Web
Imports umbraco.Core.Models

Partial Class UserControls_Accordion
    Inherits System.Web.UI.UserControl


#Region "Properties"
    Public Enum AccordionTypes
        TeamMembers = 0
        Campaigns = 1
        CampaignCategory = 2
        CampaignSubcategory = 3
    End Enum
    Public Property AccordionType As AccordionTypes
        Get
            Return _accordionTypes
        End Get
        Set(value As AccordionTypes)
            _accordionTypes = value
        End Set
    End Property
    Public Property IsActive As Boolean
        Get
            Return _isActive
        End Get
        Set(value As Boolean)
            _isActive = value
        End Set
    End Property
    'used if accordion is a category or subcategory list.
    Public Property AccordionName As String = String.Empty
    Public Property campaignList As List(Of Integer)
    Public Property groupName As String = String.Empty


    Private _accordionTypes As AccordionTypes = 0
    Private _isActive As Boolean = True
    Private blMembers As blMembers
    Private umbracoHelper As UmbracoHelper = New UmbracoHelper(UmbracoContext.Current)
    Private Structure DtTeamMemberColumns
        Const nodeId As String = "nodeId"
        Const TeamAdministrator As String = "TeamAdministrator"
        Const CampaignAdministrator As String = "CampaignAdministrator"
        Const CampaignMember As String = "CampaignMember"
    End Structure
#End Region

#Region "Handles"
    Private Sub UserControls_Accordion_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try

            'lbl.Text = groupName + " | " + AccordionName + " | " + AccordionType.ToString

            If Not IsPostBack Then
                'Set accordion view
                mvAccordionType.ActiveViewIndex = AccordionType

                Select Case AccordionType
                    Case AccordionTypes.TeamMembers
                        'Obtain list of all members
                        ObtainAllMembers()

                    Case AccordionTypes.Campaigns
                        'Obtain campaigns owned by team
                        ObtainCampaigns()

                        'If inactive, add class to ul and make control as inactive
                        If Not IsActive Then
                            Dim ulAccordion = DirectCast(lstviewCampaigns.FindControl("ulAccordion"), HtmlGenericControl)
                            If Not IsNothing(ulAccordion) Then ulAccordion.Attributes("class") += Miscellaneous.inactive
                        End If

                    Case AccordionTypes.CampaignCategory
                        'Instantiate variables
                        Dim pnlName As String = pnlContent_.ID & AccordionName.Replace(" ", "")
                        Dim blCampaigns As New blCampaigns
                        Dim BusinessReturn As BusinessReturn
                        Dim lstCampaignSummary As List(Of CampaignSummary)
                        pnlContent_.ID = pnlName
                        hlnkHandle_Category.Attributes.Add("href", "#" & pnlName)

                        ltrlCategoryName.Text = AccordionName

                        'Add grouping name to data-accordion if needed
                        If String.IsNullOrEmpty(groupName) Then
                            ulCampaignCategory.Attributes.Add("data-accordion", "")
                        Else
                            ulCampaignCategory.Attributes.Add("data-accordion", groupName)
                        End If


                        ''TEST- to show node IDs.
                        'For Each item As Integer In campaignList
                        '    ltrlCategoryName.Text += " " & item.ToString
                        'Next

                        'Obtain all campaigns by ID as class
                        BusinessReturn = blCampaigns.selectCampaignsById(campaignList)

                        If BusinessReturn.isValid Then
                            'Extract data from container.
                            lstCampaignSummary = BusinessReturn.DataContainer(0)

                            'If count > max then take the top n and add a view-more link
                            If lstCampaignSummary.Count > Miscellaneous.campaignsToShow Then
                                'Take top n
                                lstCampaignSummary = lstCampaignSummary.Take(Miscellaneous.campaignsToShow).ToList()

                                'Instantiate variables
                                Dim dummyCampaignSummary As New CampaignSummary
                                Dim strCategory As String = String.Empty
                                Dim strSubcategory As String = String.Empty
                                Dim strSortBy As String = String.Empty

                                'Obtain data from querystring
                                Dim querystringCollection As New NameValueCollection
                                If Request.QueryString.HasKeys() Then
                                    querystringCollection = HttpUtility.ParseQueryString(Request.QueryString.ToString)
                                End If
                                If querystringCollection.AllKeys.Contains(queryParameters.Category) Then strCategory = querystringCollection(queryParameters.Category)
                                If querystringCollection.AllKeys.Contains(queryParameters.Subcategory) Then strSubcategory = querystringCollection(queryParameters.Subcategory)
                                If querystringCollection.AllKeys.Contains(queryParameters.SortBy) Then strSortBy = querystringCollection(queryParameters.SortBy)



                                'dummyCampaignSummary.title = AccordionName


                                'If ddlCategories IsNot Nothing And Not String.IsNullOrWhiteSpace(ddlCategories.SelectedValue) Then
                                '    dummyCampaignSummary.campaignUrl = "~/campaigns/?Category=" & ddlCategories.SelectedValue & "&ViewAll=True"
                                '    If ddlSubcategories IsNot Nothing And Not String.IsNullOrWhiteSpace(ddlSubcategories.SelectedValue) Then
                                '        dummyCampaignSummary.campaignUrl = "~/campaigns/?Category=" & ddlCategories.SelectedValue & "&Subcategory=" & ddlSubcategories.SelectedValue & "&ViewAll=True"
                                '    Else
                                '        dummyCampaignSummary.campaignUrl = "~/campaigns/?Category=" & ddlCategories.SelectedValue & "&Subcategory=" & AccordionName & "&ViewAll=True"
                                '    End If
                                'Else
                                '    dummyCampaignSummary.campaignUrl = "~/campaigns/?Category=" & AccordionName & "&ViewAll=True"

                                'End If



                                'Create the 'view-more' url
                                Dim newUrl As New StringBuilder()
                                newUrl.Append("~/campaigns/?Category=")
                                If String.IsNullOrEmpty(strCategory) Then
                                    newUrl.Append(AccordionName)
                                Else
                                    newUrl.Append(strCategory)
                                    newUrl.Append("&Subcategory=")
                                    newUrl.Append(AccordionName)
                                End If
                                If Not String.IsNullOrEmpty(strSortBy) Then
                                    newUrl.Append("&SortBy=")
                                    newUrl.Append(strSortBy)
                                End If

                                dummyCampaignSummary.campaignUrl = newUrl.ToString

                                'Obtain image
                                dummyCampaignSummary.imageUrl = getMediaURL(mediaNodes.logoIcon_FullSize, Crops.campaignSummaryImage)

                                'Add to list
                                lstCampaignSummary.Add(dummyCampaignSummary)
                            End If

                            'Obtain a list of the top active campaigns.
                            lstviewCampaignPanels.DataSource = blCampaigns.SortBy(lstCampaignSummary, Request.QueryString(queryParameters.SortBy))
                            lstviewCampaignPanels.DataBind()
                        End If
                End Select
            End If

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\Accordion.ascx.vb : UserControls_Accordion_Load()")

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Response.Write("Error [Accordion.ascx.vb]: " & ex.ToString & "<br /><br />")
        End Try
    End Sub
#End Region

#Region "Methods"
    Private Sub ObtainAllMembers()
        Try  'Instantiate variables
            Dim lstMemberIDs As List(Of Integer) = New List(Of Integer)
            Dim teamNode As IPublishedContent = umbracoHelper.TypedContent(UmbracoContext.Current.PageId)
            Dim lstTeamAdministrators As New List(Of String)
            Dim lstCampaigns As List(Of IPublishedContent) = New List(Of IPublishedContent)
            blMembers = New blMembers
            Dim dt As DataTable = New DataTable
            Dim dr As DataRow
            Dim dc As DataColumn

            'Add columns to table
            dt.Columns.Add(DtTeamMemberColumns.nodeId, GetType(UInt32))
            'dt.Columns.Add(dtTeamMemberColumns.memberName, GetType(String))
            'Create custom columns with default values for table
            dc = New DataColumn
            With dc
                .ColumnName = DtTeamMemberColumns.TeamAdministrator
                .DataType = GetType(Boolean)
                .DefaultValue = False
            End With
            dt.Columns.Add(dc)

            dc = New DataColumn
            With dc
                .ColumnName = DtTeamMemberColumns.CampaignAdministrator
                .DataType = GetType(Boolean)
                .DefaultValue = False
            End With
            dt.Columns.Add(dc)

            dc = New DataColumn
            With dc
                .ColumnName = DtTeamMemberColumns.CampaignMember
                .DataType = GetType(Boolean)
                .DefaultValue = False
            End With
            dt.Columns.Add(dc)
            dt.AcceptChanges()


            'Obtain list of all campaigns under the team.
            For Each childNode As IPublishedContent In teamNode.Children
                lstCampaigns.Add(childNode)
            Next



            'Obtain all team administrators
            For Each id As String In teamNode.GetPropertyValue(Of String)(nodeProperties.teamAdministrators).Split(",")
                lstTeamAdministrators.Add(id)
            Next

            For Each teamAdminId As String In lstTeamAdministrators
                'Create new record
                dr = dt.NewRow
                'Add data to record
                dr(DtTeamMemberColumns.nodeId) = CInt(teamAdminId)
                dr(DtTeamMemberColumns.TeamAdministrator) = True
                dt.Rows.Add(dr)

                'Add member id to list
                lstMemberIDs.Add(teamAdminId)
            Next

            'Loop thru each campaign
            For Each campaign As IPublishedContent In lstCampaigns

                'Obtain campaign's member folder
                Dim campaignMemberNode As IPublishedContent = blMembers.getCampaignMemberFolder(campaign)
                If Not IsNothing(campaignMemberNode) Then 'AndAlso campaignMemberNode.Id <> 0 Then

                    'Loop thru each member node
                    For Each childNode As IPublishedContent In campaignMemberNode.Children
                        If Not IsNothing(childNode) Then
                            '
                            If lstMemberIDs.Contains(childNode.GetPropertyValue(Of String)(nodeProperties.campaignMember)) Then
                                'Update existing entry with additional roles
                                Dim _dr As DataRow = dt.Select(DtTeamMemberColumns.nodeId & " = " & childNode.GetPropertyValue(Of String)(nodeProperties.campaignMember)).FirstOrDefault
                                If childNode.GetPropertyValue(Of String)(nodeProperties.campaignManager) Then
                                    _dr(DtTeamMemberColumns.CampaignAdministrator) = True
                                Else
                                    _dr(DtTeamMemberColumns.CampaignMember) = True
                                End If
                            Else
                                'Add data to record
                                dr = dt.NewRow
                                dr(DtTeamMemberColumns.nodeId) = childNode.GetPropertyValue(Of String)(nodeProperties.campaignMember)
                                If childNode.GetPropertyValue(Of String)(nodeProperties.campaignManager) Then
                                    dr(DtTeamMemberColumns.CampaignAdministrator) = True
                                Else
                                    dr(DtTeamMemberColumns.CampaignMember) = True
                                End If
                                dt.Rows.Add(dr)

                                'Add member id to list
                                lstMemberIDs.Add(childNode.GetPropertyValue(Of String)(nodeProperties.campaignMember))
                            End If
                        End If
                    Next
                End If
            Next


            lstviewTeamMembers.DataSource = dt
            lstviewTeamMembers.DataBind()
            'Add grouping name to data-accordion if needed
            Dim ulAccordion = DirectCast(lstviewTeamMembers.FindControl("ulteamMembers"), HtmlGenericControl)
            If Not IsNothing(ulAccordion) Then ulAccordion.Attributes.Add("data-accordion", groupName)

            'gvTeamMembers.DataSource = dt
            'gvTeamMembers.DataBind()
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\Accordion.ascx.vb : ObtainAllMembers()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
    End Sub
    Private Sub ObtainCampaigns()
        'Instantiate variables
        Try
            Dim teamNode As IPublishedContent = umbracoHelper.TypedContent(UmbracoContext.Current.PageId)
            Dim dictCampaign As Dictionary(Of Integer, Boolean) = New Dictionary(Of Integer, Boolean)

            'Loop thru each campaign and obtain by active/inactive
            For Each campaign As IPublishedContent In teamNode.Children
                Dim activeCampaign As Boolean = campaign.GetPropertyValue(Of String)(nodeProperties.published)
                'Dim FAILED As Boolean
                'Dim COMPLETED As Boolean
                If activeCampaign = IsActive Then
                    dictCampaign.Add(campaign.Id, IsActive)
                End If
            Next

            'Add data to listview
            lstviewCampaigns.DataSource = dictCampaign
            lstviewCampaigns.DataBind()
            'Add grouping name to data-accordion if needed
            Dim ulAccordion = DirectCast(lstviewCampaigns.FindControl("ulAccordion"), HtmlGenericControl)
            If Not IsNothing(ulAccordion) Then ulAccordion.Attributes.Add("data-accordion", groupName)
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\Accordion.ascx.vb : ObtainCampaigns()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
    End Sub
#End Region


End Class
