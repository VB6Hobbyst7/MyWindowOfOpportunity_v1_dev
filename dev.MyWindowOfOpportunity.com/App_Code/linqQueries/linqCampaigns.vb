Imports System.Data
Imports Umbraco
Imports Common
Imports Umbraco.Core
Imports Umbraco.Core.Models
Imports Umbraco.Core.Services
Imports Umbraco.Core.Publishing
Imports Umbraco.Examine.Linq
Imports Umbraco.Web
Imports Stripe
Imports Newtonsoft.Json

Public Class linqCampaigns

#Region "Properties"
    Private linq2Db As linq2SqlDataContext = New linq2SqlDataContext(ConfigurationManager.ConnectionStrings("umbracoDbDSN").ToString)
    Private index As Index(Of Campaign)
    Private blPledges As blPledges
    Private blPhases As blPhases
    Dim _uHelper As Uhelper = New Uhelper()
#End Region

#Region "Selects"
    Public Function selectCampaigsSocialLink_byType(ByVal _nodeId As Integer, ByVal _socialMediaType As mediaType_Values) As String
        Try
            'Instantiate variables
            Dim thisNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(_nodeId)
            Dim returnValue As String = String.Empty

            Select Case _socialMediaType
                Case mediaType_Values.Facebook
                    If thisNode.HasProperty(nodeProperties.facebookUrl) Then returnValue = thisNode.GetPropertyValue(Of String)(nodeProperties.facebookUrl).Trim

                Case mediaType_Values.SupportEmail
                    If thisNode.HasProperty(nodeProperties.supportEmail) Then returnValue = thisNode.GetPropertyValue(Of String)(nodeProperties.supportEmail).Trim

                Case mediaType_Values.LinkedIn
                    If thisNode.HasProperty(nodeProperties.linkedInUrl) Then returnValue = thisNode.GetPropertyValue(Of String)(nodeProperties.linkedInUrl).Trim

                Case mediaType_Values.Twitter
                    If thisNode.HasProperty(nodeProperties.twitterUrl) Then returnValue = thisNode.GetPropertyValue(Of String)(nodeProperties.twitterUrl).Trim

                Case Else
                    Return String.Empty
            End Select

            'Return value
            Return returnValue
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqCampaigns.vb : selectCampaigsSocialLink_byType()")
            sb.AppendLine("_nodeId: " & _nodeId)
            sb.AppendLine("_socialMediaType: " & _socialMediaType.ToString)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

            Return String.Empty
        End Try
    End Function



    Public Function selectSampleCampaigns() As List(Of CampaignSummary)
        Try
            'Instantiate variables
            Dim lstCampaignSummary As New List(Of CampaignSummary)
            blPhases = New blPhases

            'Instantiate home node
            Dim homeNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(siteNodes.Home)

            'Obtain list of sample campaigns from home page
            Dim lstSampleCampaigns As List(Of IPublishedContent) = homeNode.GetPropertyValue(Of List(Of IPublishedContent))(nodeProperties.sampleCampaigns)

            'Add each sample campaign to list
            For Each sampleCampaign As IPublishedContent In lstSampleCampaigns
                'Instantiate variables
                Dim campaignSummary As New CampaignSummary
                Dim currentIndex As Int32 = 0
                Dim lastEntry As Int32 = 0
                Dim slstCampaigns As New List(Of KeyValuePair(Of Decimal, Integer))
                Dim slstCampaignsPledges As New List(Of KeyValuePair(Of Integer, Integer))
                Dim sortedasc As IOrderedEnumerable(Of KeyValuePair(Of Decimal, Integer)) 'NOT SURE WHAT THIS IS USED FOR.  MAY NOT BE NEEDED IN THIS FUNCTION

                '
                sortedasc = slstCampaigns.OrderBy(Function(k) k.Key)
                GetCampaignPledges(sampleCampaign, slstCampaigns, slstCampaignsPledges)

                lastEntry = sortedasc.Count - 1

                'Populate campaignsummary class
                campaignSummary.campaignUrl = sampleCampaign.Url

                Dim pledgedAmount As Decimal = slstCampaigns.Where(Function(item) item.Value.Equals(sortedasc(lastEntry - currentIndex).Value)) _
                 .Select(Function(x) x.Key).FirstOrDefault()

                campaignSummary.currentlyPledged = pledgedAmount

                'Obtain active phase, if any exist.
                campaignSummary.activePhaseId = blPhases.obtainActivePhaseId_byCampaignId(sampleCampaign.Id)
                If Not IsNothing(campaignSummary.activePhaseId) Then
                    campaignSummary.daysRemaining = blPhases.obtainDaysRemaining_byPhaseId(campaignSummary.activePhaseId)
                End If

                'Obtain number of phases in campaign
                campaignSummary.phaseCount = blPhases.publishedPhaseCount_byCampaignId(sampleCampaign.Id)

                '
                campaignSummary.imageUrl = getMediaURL(sampleCampaign.GetPropertyValue(Of Integer)(nodeProperties.summaryPanelImage), Crops.campaignSummaryImage)
                campaignSummary.nodeId = sampleCampaign.Id

                '
                Dim blCampaigns = New blCampaigns
                campaignSummary.statusType = blCampaigns.obtainStatusType_byId(sampleCampaign.Id)
                campaignSummary.shortDescription = getStatusTypeText(campaignSummary.statusType)

                'campaignSummary.statusType = Common.statusTypes.Active
                campaignSummary.team = sampleCampaign.Parent.Name
                campaignSummary.title = sampleCampaign.Name

                'Added to avoid attempt to divide by zero Error.
                If blPhases.getGoal_byCampaignId(sampleCampaign.Id) > 0 Then
                    If campaignSummary.currentlyPledged > 0 Then
                        campaignSummary.percentFunded = (campaignSummary.currentlyPledged / blPhases.getGoal_byCampaignId(sampleCampaign.Id))
                    Else
                        campaignSummary.percentFunded = 0
                    End If
                Else
                    campaignSummary.percentFunded = 0
                End If

                'Determine which cheer to display
                Dim percentFunded As Decimal = campaignSummary.percentFunded * 100
                Select Case True
                    Case (percentFunded >= 5 And percentFunded <= 15)
                        campaignSummary.cheer = umbraco.library.GetDictionaryItem(dictionaryEntry.greatStart)
                    Case (percentFunded >= 45 And percentFunded <= 55)
                        campaignSummary.cheer = umbraco.library.GetDictionaryItem(dictionaryEntry.halfWayThere)
                    Case (percentFunded >= 85 And percentFunded < 100)
                        campaignSummary.cheer = umbraco.library.GetDictionaryItem(dictionaryEntry.almostThere)
                    Case (percentFunded >= 100)
                        campaignSummary.cheer = umbraco.library.GetDictionaryItem(dictionaryEntry.youDidIt)
                End Select

                'Add to list
                lstCampaignSummary.Add(campaignSummary)
            Next



            'Return slstCampaigns
            Return lstCampaignSummary

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\linqQueries\linqCampaigns.vb : selectTopTrendingCampaigns()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

            Return Nothing
        End Try
    End Function



    Public Function selectTopTrendingCampaigns() As List(Of CampaignSummary)
        Try

            'Instantiate variables
            'Dim slstCampaigns As Lookup(Of Decimal, Integer)
            Dim slstCampaigns As New List(Of KeyValuePair(Of Decimal, Integer))
            Dim slstCampaignsPledges As New List(Of KeyValuePair(Of Integer, Integer))

            Dim lstCampaignSummary As New List(Of CampaignSummary)


            blPledges = New blPledges
            blPhases = New blPhases
            Dim totalToGet As Int32 = 5
            Dim currentIndex As Int32 = 0
            Dim lastEntry As Int32 = 0
            'From val In slstCampaigns Where val.Value = 1729 Select val.KeyslstCampaigns.Find(4,1729)

            'Obtain list of active nodes
            GetCampaignPledges(_uHelper.Get_IPublishedContentByID(siteNodes.Campaigns), slstCampaigns, slstCampaignsPledges)


            Dim sortedasc = slstCampaigns.OrderBy(Function(k) k.Key)

            'Adjust # of entries to show if less than 4 exist.
            If sortedasc.Count < totalToGet Then totalToGet = sortedasc.Count

            'Get the final index # of the list
            lastEntry = sortedasc.Count - 1

            'Loop thru top campaigns
            While totalToGet > 0
                'Instantiate variables
                Dim campaignSummary As New CampaignSummary
                Dim thisNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(sortedasc(lastEntry - currentIndex).Value)

                'Populate campaignsummary class
                campaignSummary.campaignUrl = thisNode.Url

                Dim pledgedAmount As Decimal = slstCampaigns.Where(Function(item) item.Value.Equals(sortedasc(lastEntry - currentIndex).Value)) _
                 .Select(Function(x) x.Key).FirstOrDefault()

                campaignSummary.currentlyPledged = pledgedAmount

                'Obtain active phase, if any exist.
                campaignSummary.activePhaseId = blPhases.obtainActivePhaseId_byCampaignId(thisNode.Id)
                If Not IsNothing(campaignSummary.activePhaseId) Then
                    campaignSummary.daysRemaining = blPhases.obtainDaysRemaining_byPhaseId(campaignSummary.activePhaseId)
                End If

                'Obtain number of phases in campaign
                campaignSummary.phaseCount = blPhases.publishedPhaseCount_byCampaignId(thisNode.Id)

                '
                campaignSummary.imageUrl = getMediaURL(thisNode.GetPropertyValue(Of Integer)(nodeProperties.summaryPanelImage), Crops.campaignSummaryImage)
                campaignSummary.nodeId = thisNode.Id

                '
                Dim blCampaigns = New blCampaigns
                campaignSummary.statusType = blCampaigns.obtainStatusType_byId(thisNode.Id)
                campaignSummary.shortDescription = getStatusTypeText(campaignSummary.statusType)

                'campaignSummary.statusType = Common.statusTypes.Active
                campaignSummary.team = thisNode.Parent.Name
                campaignSummary.title = thisNode.Name

                'Added to avoid attempt to divide by zero Error.
                If blPhases.getGoal_byCampaignId(thisNode.Id) > 0 Then
                    If campaignSummary.currentlyPledged > 0 Then
                        campaignSummary.percentFunded = (campaignSummary.currentlyPledged / blPhases.getGoal_byCampaignId(thisNode.Id))
                    Else
                        campaignSummary.percentFunded = 0
                    End If
                Else
                    campaignSummary.percentFunded = 0
                End If

                'Determine which cheer to display
                Dim percentFunded As Decimal = campaignSummary.percentFunded * 100
                Select Case True
                    Case (percentFunded >= 5 And percentFunded <= 15)
                        campaignSummary.cheer = Umbraco.library.GetDictionaryItem(dictionaryEntry.greatStart)
                    Case (percentFunded >= 45 And percentFunded <= 55)
                        campaignSummary.cheer = Umbraco.library.GetDictionaryItem(dictionaryEntry.halfWayThere)
                    Case (percentFunded >= 85 And percentFunded < 100)
                        campaignSummary.cheer = Umbraco.library.GetDictionaryItem(dictionaryEntry.almostThere)
                    Case (percentFunded >= 100)
                        campaignSummary.cheer = Umbraco.library.GetDictionaryItem(dictionaryEntry.youDidIt)
                End Select

                'Add to list
                lstCampaignSummary.Add(campaignSummary)

                'decrement total and increment current
                totalToGet -= 1
                currentIndex += 1
            End While

            'Return slstCampaigns
            Return lstCampaignSummary

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\linqQueries\linqCampaigns.vb : selectTopTrendingCampaigns()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

            Return Nothing
        End Try
    End Function
    Public Function selectCampaignSummary_byId(ByVal _campaignId As Integer) As BusinessReturn
        'Instantiate Session Variables
        Dim bReturn As New BusinessReturn

        Try
            'Instantiate IPublishedContent
            Dim campaignSummary As New CampaignSummary_TeamPg
            Dim ipCampaign As IPublishedContent
            ipCampaign = _uHelper.Get_IPublishedContentByID(_campaignId)

            'Obtain data
            campaignSummary.campaignId = _campaignId
            campaignSummary.title = ipCampaign.Name
            campaignSummary.teamName = ipCampaign.Parent.Name
            campaignSummary.campaignUrl = ipCampaign.Url
            campaignSummary.briefSummary = ipCampaign.GetPropertyValue(Of String)(nodeProperties.briefSummary)
            If ipCampaign.HasProperty(nodeProperties.summaryPanelImage) AndAlso ipCampaign.HasValue(nodeProperties.summaryPanelImage) Then
                campaignSummary.imageUrl = getMediaURL(ipCampaign.GetPropertyValue(Of String)(nodeProperties.summaryPanelImage), Crops.campaignSummaryImage)
            End If
            If ipCampaign.GetPropertyValue(Of Boolean)(nodeProperties.published) = True AndAlso ipCampaign.GetPropertyValue(Of Boolean)(nodeProperties.campaignComplete) = False Then
                campaignSummary.isActive = True
            End If

            bReturn.DataContainer.Add(campaignSummary)

        Catch ex As Exception
            bReturn.ExceptionMessage = ex.ToString

            Dim sb As New StringBuilder()
            sb.AppendLine("blCampaigns.vb : selectCampaignSummary_byId()")
            sb.AppendLine("_campaignId: " & _campaignId)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try

        Return bReturn
    End Function
    Public Function selectCampaignsById(ByVal _campaignList As List(Of Integer)) As BusinessReturn
        'Outer variables
        Dim BusinessReturn As New BusinessReturn
        ' Dim umbracoHelper As UmbracoHelper = New UmbracoHelper(UmbracoContext.Current)
        Try
            'Instantiate variables
            Dim lstCampaignSummary As New List(Of CampaignSummary)
            blPhases = New blPhases
            blPledges = New blPledges
            Dim campaignSummary As CampaignSummary

            'Loop thru top campaigns
            For Each nodeId As Integer In _campaignList
                'Variables
                campaignSummary = New CampaignSummary

                If nodeId > 0 Then
                    'Variables
                    Dim thisNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(nodeId)
                    'Dim thisNode As IPublishedContent = umbracoHelper.TypedContent(nodeId)
                    Dim campaignGoal As Decimal = blPhases.getGoal_byCampaignId(nodeId)

                    'Obtain active phase, if any exist.
                    campaignSummary.activePhaseId = blPhases.obtainActivePhaseId_byCampaignId(nodeId)
                    If Not IsNothing(campaignSummary.activePhaseId) Then
                        campaignSummary.daysRemaining = blPhases.obtainDaysRemaining_byPhaseId(campaignSummary.activePhaseId)
                    End If

                    'Populate campaignsummary class
                    campaignSummary.campaignUrl = thisNode.Url
                    campaignSummary.currentlyPledged = blPledges.selectSum_byCampaignId(nodeId)
                    campaignSummary.imageUrl = getMediaURL(thisNode.GetPropertyValue(Of Integer)(nodeProperties.summaryPanelImage), Crops.campaignSummaryImage)
                    campaignSummary.nodeId = thisNode.Id
                    Dim blCampaigns = New blCampaigns
                    campaignSummary.statusType = blCampaigns.obtainStatusType_byId(thisNode.Id)
                    campaignSummary.shortDescription = getStatusTypeText(campaignSummary.statusType)
                    campaignSummary.team = thisNode.Parent.Name
                    campaignSummary.title = thisNode.Name
                    campaignSummary._Date = thisNode.CreateDate
                    campaignSummary.Completed = thisNode.GetPropertyValue(Of Boolean)(nodeProperties.campaignComplete)
                    If campaignGoal > 0 Then
                        campaignSummary.percentFunded = (campaignSummary.currentlyPledged / blPhases.getGoal_byCampaignId(thisNode.Id))
                    Else
                        campaignSummary.percentFunded = 0
                    End If

                    'Obtain number of phases in campaign
                    campaignSummary.phaseCount = blPhases.publishedPhaseCount_byCampaignId(thisNode.Id)

                    'Determine which cheer to display
                    If campaignSummary.statusType = statusType.Complete Then
                        campaignSummary.cheer = umbraco.library.GetDictionaryItem(dictionaryEntry.completedSuccessfully)
                    Else
                        Dim percentFunded As Decimal = campaignSummary.percentFunded * 100
                        Select Case True
                            Case (percentFunded >= 5 And percentFunded <= 15)
                                campaignSummary.cheer = umbraco.library.GetDictionaryItem(dictionaryEntry.greatStart)
                            Case (percentFunded >= 45 And percentFunded <= 55)
                                campaignSummary.cheer = umbraco.library.GetDictionaryItem(dictionaryEntry.halfWayThere)
                            Case (percentFunded >= 85 And percentFunded < 100)
                                campaignSummary.cheer = umbraco.library.GetDictionaryItem(dictionaryEntry.almostThere)
                            Case (percentFunded >= 100)
                                campaignSummary.cheer = umbraco.library.GetDictionaryItem(dictionaryEntry.youDidIt)
                        End Select
                    End If


                End If
                'Add to list
                lstCampaignSummary.Add(campaignSummary)

                ''decrement total and increment current
                'totalToGet -= 1
                'currentIndex += 1
            Next

            BusinessReturn.DataContainer.Add(lstCampaignSummary)
            Return BusinessReturn
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\linqQueries\linqCampaigns.vb : selectCampaignsById()")
            sb.AppendLine("_campaignList:" & JsonConvert.SerializeObject(_campaignList))

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            BusinessReturn.ExceptionMessage = "Error [linqCampaigns.vb]: " & ex.ToString & "<br /><br />"
            Return BusinessReturn
        End Try
    End Function
    Public Function selectAllActiveCampaigns() As BusinessReturn
        'Instantiate outter-scope variables
        Dim businessReturn As BusinessReturn = New BusinessReturn

        Try
            'Instantiate variables
            Dim lstNodeIds As New List(Of Integer)

            ' Populate list with all active campaigns and the categories they belong to.
            ObtainAllActiveAndCompleteCampaigns(_uHelper.Get_IPublishedContentByID(siteNodes.Campaigns), lstNodeIds)
            'ObtainAllActiveCampaigns(_uHelper.Get_IPublishedContentByID(siteNodes.Campaigns), lstNodeIds)

            'Add data to return class
            businessReturn.DataContainer.Add(lstNodeIds)
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\linqQueries\linqCampaigns.vb : selectAllActiveCampaigns()")

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            businessReturn.ExceptionMessage = ex.ToString
        End Try

        Return businessReturn
    End Function
    Public Function selectCampaignsBySearch(ByVal _searchBy As String) As BusinessReturn
        'Instantiate outter-scope variables
        Dim businessReturn As BusinessReturn = New BusinessReturn

        Try
            'Instantiate variables
            Dim lstNodeIds As New List(Of Integer)

            ' Populate list with all active campaigns and the categories they belong to.
            ObtainAllActiveAndCompleteCampaigns(_uHelper.Get_IPublishedContentByID(siteNodes.Campaigns), lstNodeIds, _searchBy)
            'ObtainAllActiveCampaigns(_uHelper.Get_IPublishedContentByID(siteNodes.Campaigns), lstNodeIds, _searchBy)

            'Add data to return class
            businessReturn.DataContainer.Add(lstNodeIds)
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\linqQueries\linqCampaigns.vb : selectCampaignsBySearch()")
            sb.AppendLine("_searchBy:" & _searchBy)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            businessReturn.ExceptionMessage = ex.ToString
        End Try

        Return businessReturn
    End Function
    Public Function selectCampaignsWithCatagories() As BusinessReturn
        'Instantiate outter-scope variables
        Dim businessReturn As BusinessReturn = New BusinessReturn

        Try
            'Instantiate variables
            Dim lstCampaignCategories As New List(Of CampaignCategories)

            ' Populate list with all active campaigns and the categories they belong to.
            ObtainCampaignsAndCategories(_uHelper.Get_IPublishedContentByID(siteNodes.Campaigns), lstCampaignCategories)

            'If returnMsg.isValid Then
            '
            ' Dim lstCampaignCategories As List(Of CampaignCategories) = returnMsg.DataContainer(0)
            Dim lstArtistic As List(Of Integer) = New List(Of Integer)
            Dim lstBusiness As List(Of Integer) = New List(Of Integer)
            Dim lstCharity As List(Of Integer) = New List(Of Integer)
            Dim lstCommunity As List(Of Integer) = New List(Of Integer)
            Dim lstScience As List(Of Integer) = New List(Of Integer)
            Dim lstSelfHelp As List(Of Integer) = New List(Of Integer)
            Dim lstSoftware As List(Of Integer) = New List(Of Integer)
            Dim lstTechnology As List(Of Integer) = New List(Of Integer)

            '
            For Each campaign As CampaignCategories In lstCampaignCategories
                If campaign.Artistic Then lstArtistic.Add(campaign.nodeId)
                If campaign.Business Then lstBusiness.Add(campaign.nodeId)
                If campaign.Charity Then lstCharity.Add(campaign.nodeId)
                If campaign.Community Then lstCommunity.Add(campaign.nodeId)
                If campaign.Science Then lstScience.Add(campaign.nodeId)
                If campaign.SelfHelp Then lstSelfHelp.Add(campaign.nodeId)
                If campaign.Software Then lstSoftware.Add(campaign.nodeId)
                If campaign.Technology Then lstTechnology.Add(campaign.nodeId)
            Next

            'Create Accordions if data exists per list.
            If lstArtistic.Count > 0 Then
                'Dim accordion As ASP.Accordion = New ASP.Accordion
                'accordion.accordionName = Categories.Arts
                'accordion.campaignList = lstArtistic
                'accordion.accordionType = 2
                'phAccordions.Controls.Add(accordion)

                'Add both the nodeId list and concatenate the sucategory name
                businessReturn.DataContainer.Add(lstArtistic)
                businessReturn.ReturnMessage += "," & UppercaseFirstLetter(Categories.Arts)
            End If

            If lstBusiness.Count > 0 Then
                'Dim accordion As ASP.Accordion = New ASP.Accordion
                'accordion.accordionName = Categories.Business
                'accordion.campaignList = lstBusiness
                'accordion.accordionType = ASP.Accordion.accordionTypes.CampaignCategory
                'phAccordions.Controls.Add(accordion)

                'Add both the nodeId list and concatenate the sucategory name
                businessReturn.DataContainer.Add(lstBusiness)
                businessReturn.ReturnMessage += "," & UppercaseFirstLetter(Categories.Business)
            End If

            If lstCharity.Count > 0 Then
                'Dim accordion As ASP.Accordion = New ASP.Accordion
                'accordion.accordionName = Categories.Charity
                'accordion.campaignList = lstCharity
                'accordion.accordionType = ASP.Accordion.accordionTypes.CampaignCategory
                'phAccordions.Controls.Add(accordion)

                'Add both the nodeId list and concatenate the sucategory name
                businessReturn.DataContainer.Add(lstCharity)
                businessReturn.ReturnMessage += "," & UppercaseFirstLetter(Categories.Charity)
            End If

            If lstCommunity.Count > 0 Then
                'Dim accordion As ASP.Accordion = New ASP.Accordion
                'accordion.accordionName = Categories.Community
                'accordion.campaignList = lstCommunity
                'accordion.accordionType = ASP.Accordion.accordionTypes.CampaignCategory
                'phAccordions.Controls.Add(accordion)

                'Add both the nodeId list and concatenate the sucategory name
                businessReturn.DataContainer.Add(lstCommunity)
                businessReturn.ReturnMessage += "," & UppercaseFirstLetter(Categories.Community)
            End If

            If lstScience.Count > 0 Then
                'Dim accordion As ASP.Accordion = New ASP.Accordion
                'accordion.accordionName = Categories.Science
                'accordion.campaignList = lstScience
                'accordion.accordionType = ASP.Accordion.accordionTypes.CampaignCategory
                'phAccordions.Controls.Add(accordion)

                'Add both the nodeId list and concatenate the sucategory name
                businessReturn.DataContainer.Add(lstScience)
                businessReturn.ReturnMessage += "," & UppercaseFirstLetter(Categories.Science)
            End If

            If lstSelfHelp.Count > 0 Then
                'Dim accordion As ASP.Accordion = New ASP.Accordion
                'accordion.accordionName = Categories.SelfHelp
                'accordion.campaignList = lstSelfHelp
                'accordion.accordionType = ASP.Accordion.accordionTypes.CampaignCategory
                'phAccordions.Controls.Add(accordion)

                'Add both the nodeId list and concatenate the sucategory name
                businessReturn.DataContainer.Add(lstSelfHelp)
                businessReturn.ReturnMessage += "," & UppercaseFirstLetter(Categories.SelfHelp)
            End If

            If lstSoftware.Count > 0 Then
                'Dim accordion As ASP.Accordion = New ASP.Accordion
                'accordion.accordionName = Categories.Software
                'accordion.campaignList = lstSoftware
                'accordion.accordionType = ASP.Accordion.accordionTypes.CampaignCategory
                'phAccordions.Controls.Add(accordion)

                'Add both the nodeId list and concatenate the sucategory name
                businessReturn.DataContainer.Add(lstSoftware)
                businessReturn.ReturnMessage += "," & UppercaseFirstLetter(Categories.Software)
            End If

            If lstTechnology.Count > 0 Then
                'Dim accordion As ASP.Accordion = New ASP.Accordion
                'accordion.accordionName = Categories.Technology
                'accordion.campaignList = lstTechnology
                'accordion.accordionType = ASP.Accordion.accordionTypes.CampaignCategory  'UserControls_Accordion.accordionTypes.CampaignCategory
                'phAccordions.Controls.Add(accordion)

                'Add both the nodeId list and concatenate the sucategory name
                businessReturn.DataContainer.Add(lstTechnology)
                businessReturn.ReturnMessage += "," & UppercaseFirstLetter(Categories.Technology)
            End If
            'Else
            '    Response.Write("Error: " & returnMsg.ExceptionMessage)
            'End If



            ''return data
            'Return businessReturn

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\linqQueries\linqCampaigns.vb : selectCampaignsWithCatagories()")

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            businessReturn.ExceptionMessage = "Error [linqCampaigns.selectCampaignsWithCatagories]: " & ex.ToString
            Return businessReturn
        End Try

        Return businessReturn
    End Function
    Public Function obtainRatingSummary_byCampaignId(ByVal _campaignId As Integer) As BusinessReturn
        'Instantiate outter-scope variables
        Dim businessReturn As BusinessReturn = New BusinessReturn

        Try
            'Instantiate variables

            Dim brStatistics As BusinessReturn
            Dim CampaignStatistics As CampaignStatistics
            Dim blCampaigns As New blCampaigns
            Dim campaignNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(_campaignId)
            Dim ratingSummary As New RatingSummary

            'Obtain data
            brStatistics = blCampaigns.obtainStatistics(campaignNode)

            If brStatistics.isValid Then
                'Extract data
                CampaignStatistics = DirectCast(brStatistics.DataContainer(0), CampaignStatistics)

                'Determine if discovery phase is active or not
                Dim status As Common.statusType = CampaignStatistics.statusType
                If status = statusType.DiscoveryPhase Then
                    'Instantiate star variables
                    Dim lstRatings As New List(Of Int16)
                    Dim stars1 As Int16 = 0
                    Dim stars2 As Int16 = 0
                    Dim stars3 As Int16 = 0
                    Dim stars4 As Int16 = 0
                    Dim stars5 As Int16 = 0
                    Dim starAvg As Single = 0

                    'Obtain discovery IPublishedContent if it exists.

                    Dim discovery As IPublishedContent = campaignNode.Children().Where(Function(x) (x.Name.ToLower() = docTypes.discovery)).FirstOrDefault()

                    'Obtain review values and creaet rating array
                    If discovery IsNot Nothing Then
                        Dim ratingArr As String = String.Empty
                        For Each PhasesProperty As IPublishedContent In discovery.Children
                            If PhasesProperty.HasProperty("stars") Then
                                ratingArr += PhasesProperty.GetPropertyValue("stars").ToString() + ","
                                lstRatings.Add(PhasesProperty.GetPropertyValue("stars").ToString())
                            End If
                        Next
                        ratingSummary.ratingArray = ratingArr.TrimEnd(",")
                    Else
                        ratingSummary.ratingArray = 0
                    End If

                    'Determine rating totals
                    If lstRatings.Count > 0 Then
                        'Loop through each rating value
                        For Each rating As Int16 In lstRatings
                            'Count individual star values
                            Select Case rating
                                Case 1
                                    stars1 += 1
                                Case 2
                                    stars2 += 1
                                Case 3
                                    stars3 += 1
                                Case 4
                                    stars4 += 1
                                Case 5
                                    stars5 += 1
                            End Select
                            'Add sum of stars
                            starAvg += rating
                            'Increment rating counter
                            ratingSummary.reviewCount += 1
                        Next

                        'Calculate percentages
                        If stars1 > 0 Then
                            ratingSummary.star1Percentage = (stars1 / lstRatings.Count) * 100
                        End If
                        If stars2 > 0 Then
                            ratingSummary.star2Percentage = (stars2 / lstRatings.Count) * 100
                        End If
                        If stars3 > 0 Then
                            ratingSummary.star3Percentage = (stars3 / lstRatings.Count) * 100
                        End If
                        If stars4 > 0 Then
                            ratingSummary.star4Percentage = (stars4 / lstRatings.Count) * 100
                        End If
                        If stars5 > 0 Then
                            ratingSummary.star5Percentage = (stars5 / lstRatings.Count) * 100
                        End If

                        'Calculate average stars
                        ratingSummary.avgStars = Math.Round(starAvg / lstRatings.Count, 1)
                    End If
                End If
            End If

            'Add rating summary to business return.
            businessReturn.DataContainer.Add(ratingSummary)

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqCampaigns.vb : obtainRatingSummary_byCampaignId()")
            sb.AppendLine("_campaignId: " & _campaignId)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            businessReturn.ExceptionMessage = "Error [linqCampaigns.obtainRatingSummary_byCampaignId]: " & ex.ToString
            Return businessReturn
        End Try


        Return businessReturn
    End Function
    Public Function selectStripeUserId_byCampaignId(ByVal _campaignId As Integer) As String
        Try
            'Instantiate variables
            Dim ipCampaign As IPublishedContent = _uHelper.Get_IPublishedContentByID(_campaignId)

            'Obtain Stripe Client Id
            If ipCampaign.HasProperty(nodeProperties.stripeUserId) AndAlso ipCampaign.HasValue(nodeProperties.stripeUserId) Then
                Return ipCampaign.GetPropertyValue(Of String)(nodeProperties.stripeUserId)
            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqCampaigns.vb : selectStripeUserId_byCampaignId()")
            sb.AppendLine("_campaignId: " & _campaignId)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try

        'Return empty value if no id is found
        Return ""
    End Function
    Public Function obtainTeamManagedData_byId(ByVal id As Integer) As TeamSummary_Manage
        Try
            'Instantiate variables
            Dim teamSummary As TeamSummary_Manage
            Dim ipTeam As IPublishedContent = _uHelper.Get_IPublishedContentByID(id)

            'Obtain list of teams
            teamSummary = New TeamSummary_Manage With {
                .teamId = id,
                .name = ipTeam.Name,
                .teamUrl = ipTeam.Url,
                .teamEditUrl = ipTeam.Url & "?editMode=True"
            }

            Return teamSummary

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqCampaigns.vb : obtainTeamManagedData_byId()")
            sb.AppendLine("id: " & id)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

            Return Nothing
        End Try
    End Function
    Public Function obtainCampaignManagedData_byId(ByVal id As Integer) As CampaignSummary_Manage
        Try
            'Instantiate variables
            Dim campaignSummary As New CampaignSummary_Manage
            Dim ipCampaign As IPublishedContent = _uHelper.Get_IPublishedContentByID(id)

            'Obtain Campaigns
            campaignSummary.campaignId = id
            campaignSummary.title = ipCampaign.Name
            campaignSummary.team = ipCampaign.Parent.Name
            campaignSummary.campaignUrl = ipCampaign.Url
            campaignSummary.campaignEditUrl = _uHelper.Get_IPublishedContentByID(siteNodes.EditCampaign).Url & "?" & queryParameters.nodeId & "=" & HttpUtility.UrlEncode(id)
            campaignSummary.imageUrl = getMediaURL(ipCampaign.GetPropertyValue(Of Integer)(nodeProperties.summaryPanelImage), Crops.campaignSummaryImage)

            Return campaignSummary

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqCampaigns.vb : obtainCampaignManagedData_byId()")
            sb.AppendLine("Id: " & id)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

            Return Nothing
        End Try
    End Function
#End Region

#Region "Inserts"
    Public Function CreateCampaign(ByVal _teamId As Integer, ByVal _campaignName As String) As Integer

        Dim icCampaign As IContent = Nothing
        Try
            'Create a new campaign IPublishedContent
            Dim cs As IContentService = ApplicationContext.Current.Services.ContentService
            Dim teamFolder As IContent = cs.GetById(_teamId)
            icCampaign = cs.CreateContentWithIdentity(_campaignName, teamFolder, docTypes.Campaign)
            'Save values
            cs.SaveAndPublishWithStatus(icCampaign)

            'Is this a "no fees" campaign
            Dim blSiteManagement As New blSiteManagement
            If blSiteManagement.isFeeFreeTimeperiod() Then
                'Set value in new campaign
                icCampaign.SetValue(nodeProperties.noFeesApplied, True)
                cs.SaveAndPublishWithStatus(icCampaign)
            End If

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqCampaigns.vb : CreateCampaign()")
            sb.AppendLine("_teamId:" & _teamId)
            sb.AppendLine("_campaignName:" & _campaignName)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try

        'Return new Id
        Return icCampaign.Id
    End Function
#End Region

#Region "Updates"
    Public Function UpdateContent(ByVal nodeId As Int16, ByVal briefSummary As String, ByVal fullSummary As String, ByVal RemovePanelImg As Boolean, ByVal RemoveTopBannerImg As Boolean, ByVal customCss As String) As Attempt(Of PublishStatus)

        'Instantiate variables
        Try
            Dim cs As IContentService = ApplicationContext.Current.Services.ContentService
            Dim campaign As IContent = cs.GetById(nodeId)
            'Set values
            campaign.SetValue(nodeProperties.briefSummary, briefSummary)
            campaign.SetValue(nodeProperties.fullSummary, fullSummary)
            If RemovePanelImg Then campaign.SetValue(nodeProperties.summaryPanelImage, Nothing)
            If RemoveTopBannerImg Then campaign.SetValue(nodeProperties.topBannerImage, Nothing)
            campaign.SetValue(nodeProperties.customCSS, customCss)

            'Save values
            Return cs.SaveAndPublishWithStatus(campaign)
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqCampaigns.vb : UpdateContent()")
            sb.AppendLine("nodeId:" & nodeId)
            sb.AppendLine("briefSummary:" & briefSummary)
            sb.AppendLine("fullSummary:" & fullSummary)
            sb.AppendLine("RemovePanelImg" & RemovePanelImg)
            sb.AppendLine("RemoveTopBannerImg" & RemoveTopBannerImg)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return Nothing
        End Try
    End Function
    Public Function UpdateContent(ByVal nodeId As Int16, ByVal _campaignPublished As Boolean, ByVal _campaignComplete As Boolean) As Attempt(Of PublishStatus)

        'Instantiate variables
        Try
            Dim cs As IContentService = ApplicationContext.Current.Services.ContentService
            Dim campaign As IContent = cs.GetById(nodeId)
            'Set values
            campaign.SetValue(nodeProperties.published, _campaignPublished)
            campaign.SetValue(nodeProperties.campaignComplete, _campaignComplete)
            campaign.SetValue(nodeProperties.completionDate, DateTime.Now)

            'Save values
            Return cs.SaveAndPublishWithStatus(campaign)

        Catch ex As Exception

            Dim sb As New StringBuilder()
            sb.AppendLine("linqCampaigns.vb : UpdateContent()")
            sb.AppendLine("nodeId:" & nodeId)
            sb.AppendLine("_campaignPublished:" & _campaignPublished)
            sb.AppendLine("_campaignComplete:" & _campaignComplete)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return Nothing
        End Try

    End Function
    Public Function SaveFeaturedImage(ByVal _nodeId As Int16, ByVal _mediaId As Integer, ByVal propertyName As String) As Attempt(Of PublishStatus)

        'Instantiate variables
        Try
            Dim cs As IContentService = ApplicationContext.Current.Services.ContentService
            Dim campaign As IContent = cs.GetById(_nodeId)
            'Set values
            campaign.SetValue(propertyName, _mediaId)
            'campaign.SetValue(nodeProperties.summaryPanelImage, _mediaId)
            'Save values
            Return cs.SaveAndPublishWithStatus(campaign)
        Catch ex As Exception

            Dim sb As New StringBuilder()
            sb.AppendLine("linqCampaigns.vb : SaveFeaturedImage()")
            sb.AppendLine("nodeId:" & _nodeId)
            sb.AppendLine("mediaId:" & _mediaId)
            sb.AppendLine("propertyName:" & propertyName)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return Nothing
        End Try
    End Function
    Public Function UpdateCategories(ByVal nodeId As Int16, ByVal subCategory As String, ByVal cblCsv As String) As Attempt(Of PublishStatus)

        Try
            'Instantiate variables
            Dim cs As IContentService = ApplicationContext.Current.Services.ContentService
            Dim campaign As IContent = cs.GetById(nodeId)
            'Set values
            campaign.SetValue(subCategory, cblCsv)
            'Save values
            Return cs.SaveAndPublishWithStatus(campaign)
        Catch ex As Exception

            Dim sb As New StringBuilder()
            sb.AppendLine("linqCampaigns.vb : UpdateCategories()")
            sb.AppendLine("nodeId:" & nodeId)
            sb.AppendLine("subCategory:" & subCategory)
            sb.AppendLine("cblCsv:" & cblCsv)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return Nothing
        End Try
    End Function
    Public Function UpdateSocialMediaUrl(ByVal _nodeId As Int16, ByVal _socialMediaType As mediaType_Values, ByVal _url As String) As BusinessReturn
        '
        Dim BusinessReturn As BusinessReturn = New BusinessReturn

        Try
            'Instantiate variables
            Dim cs As IContentService = ApplicationContext.Current.Services.ContentService
            Dim campaign As IContent = cs.GetById(_nodeId)
            Dim attempt As Attempt(Of PublishStatus)

            'Set values
            Select Case _socialMediaType
                Case mediaType_Values.Facebook
                    campaign.SetValue(nodeProperties.facebookUrl, _url)

                'Case mediaType_Values.GooglePlus
                '    campaign.SetValue(nodeProperties.googlePlusUrl, _url)
                Case mediaType_Values.SupportEmail
                    campaign.SetValue(nodeProperties.supportEmail, _url)

                Case mediaType_Values.LinkedIn
                    campaign.SetValue(nodeProperties.linkedInUrl, _url)

                Case mediaType_Values.Twitter
                    campaign.SetValue(nodeProperties.twitterUrl, _url)
            End Select

            'Save values
            attempt = cs.SaveAndPublishWithStatus(campaign)

            'Return result.
            If (attempt.Success) Then
                Return BusinessReturn
            Else
                BusinessReturn.ExceptionMessage = attempt.Exception.ToString
                Return BusinessReturn
            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\linqQueries\linqCampaigns.vb : UpdateSocialMediaUrl")
            sb.AppendLine("_nodeId:" & _nodeId)
            sb.AppendLine("_socialMediaType:" & _socialMediaType)
            sb.AppendLine("_url:" & _url)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            BusinessReturn.ExceptionMessage = "Error: " & ex.ToString & "<br />_nodeId: " & _nodeId & "<br />_socialMediaType: " & _socialMediaType & "<br />_url: " & _url
            Return BusinessReturn
        End Try

    End Function

    Public Function SaveStripeAcctIDs(ByVal nodeId As Int16, ByVal stripeClientId As String) As BusinessReturn
        'Instantiate scope variables
        Dim bReturn As New BusinessReturn
        Dim stripeOAuthToken As New StripeOAuthToken

        Try
            'Instantiate variables
            Dim stripeOAuthTokenCreateOptions As New StripeOAuthTokenCreateOptions
            Dim stripeOAuthTokenService As New StripeOAuthTokenService
            Dim stripeRequestOptions As New StripeRequestOptions
            Dim cs As IContentService = ApplicationContext.Current.Services.ContentService
            Dim campaign As IContent = cs.GetById(nodeId)
            Dim attempt As Attempt(Of PublishStatus)

            'Add stripe api key
            stripeRequestOptions.ApiKey = ConfigurationManager.AppSettings(Miscellaneous.StripeApiKey)

            'Build stripe options list
            With stripeOAuthTokenCreateOptions
                .ClientSecret = ConfigurationManager.AppSettings(Miscellaneous.StripeConnectId)
                .Code = stripeClientId
                .GrantType = "authorization_code"
                .Scope = "read_write"
            End With

            'Create acct
            stripeOAuthToken = stripeOAuthTokenService.Create(stripeOAuthTokenCreateOptions, stripeRequestOptions)

            'Set values
            campaign.SetValue(nodeProperties.notes, JsonConvert.SerializeObject(stripeOAuthToken))
            campaign.SetValue(nodeProperties.stripeClientId, stripeClientId)
            campaign.SetValue(nodeProperties.stripePublishableKey, stripeOAuthToken.StripePublishableKey)
            campaign.SetValue(nodeProperties.stripeUserId, stripeOAuthToken.StripeUserId)
            campaign.SetValue(nodeProperties.accessToken, stripeOAuthToken.AccessToken)
            campaign.SetValue(nodeProperties.refreshToken, stripeOAuthToken.RefreshToken)

            'Save values
            attempt = cs.SaveAndPublishWithStatus(campaign)
            cs.Publish(campaign)

            'Return result
            bReturn.isValid = attempt.Success
            If Not attempt.Success Then
                bReturn.ExceptionMessage = attempt.Exception.ToString

                Dim sb As New StringBuilder()
                sb.AppendLine("linqCampaigns.vb : UpdateStripeAcctId()")
                sb.AppendLine("StripeException")
                sb.AppendLine("nodeId:" & nodeId.ToString())
                sb.AppendLine("stripeClientId:" & stripeClientId)
                sb.AppendLine(JsonConvert.SerializeObject(stripeOAuthToken))
                saveErrorMessage(getLoggedInMember, attempt.Exception.ToString, sb.ToString())
            End If
            Return bReturn

        Catch sExc As StripeException
            Dim ex As String = sExc.ToString & "<br /><br />" & JsonConvert.SerializeObject(stripeOAuthToken)
            Dim sb As New StringBuilder()
            sb.AppendLine("linqCampaigns.vb : UpdateStripeAcctId()")
            sb.AppendLine("StripeException")
            sb.AppendLine("nodeId:" & nodeId.ToString())
            sb.AppendLine("stripeClientId:" & stripeClientId)
            sb.AppendLine(JsonConvert.SerializeObject(stripeOAuthToken))
            saveErrorMessage(getLoggedInMember, ex, sb.ToString())

            bReturn.ExceptionMessage = sExc.ToString
            Return bReturn

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqCampaigns.vb : UpdateStripeAcctId()")
            sb.AppendLine("Exception")
            sb.AppendLine("nodeId:" & nodeId.ToString())
            sb.AppendLine("stripeClientId:" & stripeClientId)
            sb.AppendLine(JsonConvert.SerializeObject(stripeOAuthToken))
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

            bReturn.ExceptionMessage = ex.ToString
            Return bReturn
        End Try
    End Function

#End Region

#Region "Delete"
#End Region

#Region "Methods"
    Public Function doesCampaignExist_byName(ByVal _campaignName As String) As Boolean?
        Try
            'Return if exists
            index = New Index(Of Campaign)()
            Return (From a In index Where a.Name.Equals(_campaignName) Select a).ToArray.Any()
        Catch ex As Exception

            Dim sb As New StringBuilder()
            sb.AppendLine("linqCampaigns.vb : UpdateContent()")
            sb.AppendLine("_campaignName:" & doesCampaignExist_byName)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return Nothing
        End Try
    End Function
    Private Sub GetCampaignPledges(ByVal thisNode As IPublishedContent, ByRef slst As List(Of KeyValuePair(Of Decimal, Integer)), ByRef slstPledges As List(Of KeyValuePair(Of Integer, Integer)))

        Try

            If thisNode.DocumentTypeAlias = docTypes.Campaign Then
                If thisNode.GetPropertyValue(Of Boolean)(nodeProperties.published) = True AndAlso thisNode.GetPropertyValue(Of Boolean)(nodeProperties.campaignComplete) = False Then
                    'slst.Add(blPledges.select_byCampaignId(thisNode.Id), thisNode.Id)
                    blPledges = New blPledges
                    ' slst.Add(blPledges.selectSum_byCampaignId(thisNode.Id), thisNode.Id)
                    slst.Add(New KeyValuePair(Of Decimal, Integer)(blPledges.selectSum_byCampaignId(thisNode.Id), thisNode.Id))
                    slstPledges.Add(New KeyValuePair(Of Integer, Integer)(blPledges.selectCount_byCampaignId(thisNode.Id), thisNode.Id))
                End If
            Else
                For Each childNode As IPublishedContent In thisNode.Children
                    GetCampaignPledges(childNode, slst, slstPledges)
                Next
            End If
        Catch ex As Exception

            Dim sb As New StringBuilder()
            sb.AppendLine("linqCampaigns.vb : GetCampaignPledges()")
            sb.AppendLine("thisNode:" & JsonConvert.SerializeObject(thisNode))
            sb.AppendLine("slst:" & JsonConvert.SerializeObject(slst))
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

        End Try

    End Sub
    Public Function ObtainCampaigns(ByRef dt As DataTable, ByRef thisNode As IPublishedContent, Optional ByVal teamName As String = "") As DataTable
        '
        Try
            Select Case thisNode.DocumentTypeAlias
                Case docTypes.Team
                    For Each childNode As IPublishedContent In thisNode.Children
                        ObtainCampaigns(dt, childNode, thisNode.Name)
                    Next


                Case docTypes.Campaign
                    addCampaignToTable(dt, thisNode, teamName)


                Case Else
                    For Each childNode As IPublishedContent In thisNode.Children
                        ObtainCampaigns(dt, childNode)
                    Next
            End Select
        Catch ex As Exception

            Dim sb As New StringBuilder()
            sb.AppendLine("linqCampaigns.vb : ObtainCampaigns()")
            sb.AppendLine("dt:" & JsonConvert.SerializeObject(dt))
            sb.AppendLine("thisNode" & JsonConvert.SerializeObject(thisNode))
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

        End Try
        Return dt
    End Function
    Public Function addCampaignToTable(ByRef dt As DataTable, ByRef thisNode As IPublishedContent, Optional ByVal teamName As String = "", Optional ByVal category As String = "", Optional ByVal subcategory As String = "") As DataTable
        Try
            If thisNode.HasProperty(nodeProperties.published) Then
                If (thisNode.GetPropertyValue(Of Boolean)(nodeProperties.published)) Then
                    'If category exists, filter by category
                    If Not String.IsNullOrEmpty(category) Then
                        Select Case category
                            Case Categories.Arts
                                If Not thisNode.HasProperty(SubcategoryConverter.Artistic) OrElse
                                    String.IsNullOrEmpty(thisNode.GetPropertyValue(Of String)(SubcategoryConverter.Artistic)) Then Return dt 'Exit Function
                                If Not String.IsNullOrWhiteSpace(subcategory) AndAlso Not DoesNodeHaveSubcategory(subcategory, thisNode.GetPropertyValue(Of String)(SubcategoryConverter.Artistic)) Then Return dt
                            Case Categories.Business
                                If Not thisNode.HasProperty(SubcategoryConverter.Business) OrElse
                                    String.IsNullOrEmpty(thisNode.GetPropertyValue(Of String)(SubcategoryConverter.Business)) Then Return dt 'Exit Function
                                If Not String.IsNullOrWhiteSpace(subcategory) AndAlso Not DoesNodeHaveSubcategory(subcategory, thisNode.GetPropertyValue(Of String)(SubcategoryConverter.Business)) Then Return dt
                            Case Categories.Charity
                                If Not thisNode.HasProperty(SubcategoryConverter.Charity) OrElse
                                    String.IsNullOrEmpty(thisNode.GetPropertyValue(Of String)(SubcategoryConverter.Charity)) Then Return dt 'Exit Function
                                If Not String.IsNullOrWhiteSpace(subcategory) AndAlso Not DoesNodeHaveSubcategory(subcategory, thisNode.GetPropertyValue(Of String)(SubcategoryConverter.Charity)) Then Return dt
                            Case Categories.Community
                                If Not thisNode.HasProperty(SubcategoryConverter.Community) OrElse
                                    String.IsNullOrEmpty(thisNode.GetPropertyValue(Of String)(SubcategoryConverter.Community)) Then Return dt 'Exit Function
                                If Not String.IsNullOrWhiteSpace(subcategory) AndAlso Not DoesNodeHaveSubcategory(subcategory, thisNode.GetPropertyValue(Of String)(SubcategoryConverter.Community)) Then Return dt
                            Case Categories.Science
                                If Not thisNode.HasProperty(SubcategoryConverter.Science) OrElse
                                    String.IsNullOrEmpty(thisNode.GetPropertyValue(Of String)(SubcategoryConverter.Science)) Then Return dt 'Exit Function
                                If Not String.IsNullOrWhiteSpace(subcategory) AndAlso Not DoesNodeHaveSubcategory(subcategory, thisNode.GetPropertyValue(Of String)(SubcategoryConverter.Science)) Then Return dt
                            Case Categories.SelfHelp
                                If Not thisNode.HasProperty(SubcategoryConverter.SelfHelp) OrElse
                                    String.IsNullOrEmpty(thisNode.GetPropertyValue(Of String)(SubcategoryConverter.SelfHelp)) Then Return dt 'Exit Function
                                If Not String.IsNullOrWhiteSpace(subcategory) AndAlso Not DoesNodeHaveSubcategory(subcategory, thisNode.GetPropertyValue(Of String)(SubcategoryConverter.SelfHelp)) Then Return dt
                            Case Categories.Software
                                If Not thisNode.HasProperty(SubcategoryConverter.Software) OrElse
                                    String.IsNullOrEmpty(thisNode.GetPropertyValue(Of String)(SubcategoryConverter.Software)) Then Return dt 'Exit Function
                                If Not String.IsNullOrWhiteSpace(subcategory) AndAlso Not DoesNodeHaveSubcategory(subcategory, thisNode.GetPropertyValue(Of String)(SubcategoryConverter.Software)) Then Return dt
                            Case Categories.Technology
                                If Not thisNode.HasProperty(SubcategoryConverter.Technology) OrElse
                                    String.IsNullOrEmpty(thisNode.GetPropertyValue(Of String)(SubcategoryConverter.Technology)) Then Return dt 'Exit Function
                                If Not String.IsNullOrWhiteSpace(subcategory) AndAlso Not DoesNodeHaveSubcategory(subcategory, thisNode.GetPropertyValue(Of String)(SubcategoryConverter.Technology)) Then Return dt
                            Case Else
                                Return dt 'Exit Function
                        End Select
                    End If

                    'Instantiate variables
                    Dim dr As DataRow = dt.NewRow

                    'Add values to datarow
                    dr(Miscellaneous.nodeId) = thisNode.Id
                    If thisNode.HasProperty(Miscellaneous.creationDate) Then dr(Miscellaneous.creationDate) = thisNode.GetPropertyValue(Of Date)(Miscellaneous.creationDate)
                    If thisNode.HasProperty(SubcategoryConverter.Artistic) Then dr(SubcategoryConverter.Artistic) = thisNode.GetPropertyValue(Of String)(SubcategoryConverter.Artistic)
                    If thisNode.HasProperty(SubcategoryConverter.Business) Then dr(SubcategoryConverter.Business) = thisNode.GetPropertyValue(Of String)(SubcategoryConverter.Business)
                    If thisNode.HasProperty(SubcategoryConverter.Charity) Then dr(SubcategoryConverter.Charity) = thisNode.GetPropertyValue(Of String)(SubcategoryConverter.Charity)
                    If thisNode.HasProperty(SubcategoryConverter.Community) Then dr(SubcategoryConverter.Community) = thisNode.GetPropertyValue(Of String)(SubcategoryConverter.Community)
                    If thisNode.HasProperty(SubcategoryConverter.Science) Then dr(SubcategoryConverter.Science) = thisNode.GetPropertyValue(Of String)(SubcategoryConverter.Science)
                    If thisNode.HasProperty(SubcategoryConverter.SelfHelp) Then dr(SubcategoryConverter.SelfHelp) = thisNode.GetPropertyValue(Of String)(SubcategoryConverter.SelfHelp)
                    If thisNode.HasProperty(SubcategoryConverter.Software) Then dr(SubcategoryConverter.Software) = thisNode.GetPropertyValue(Of String)(SubcategoryConverter.Software)
                    If thisNode.HasProperty(SubcategoryConverter.Technology) Then dr(SubcategoryConverter.Technology) = thisNode.GetPropertyValue(Of String)(SubcategoryConverter.Technology)

                    'Add new row to datatable
                    dt.Rows.Add(dr)
                    dt.AcceptChanges()
                End If
            End If

            '
            Return dt
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\linqQueries\linqCampaigns.vb : addCampaignToTable()")
            sb.AppendLine("dt:" & dt.ToString())
            sb.AppendLine("thisNode:" & thisNode.ToString())
            sb.AppendLine("teamName:" & teamName)
            sb.AppendLine("category:" & category)
            sb.AppendLine("subcategory:" & subcategory)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Response.Write(ex.ToString & "<br />")
            Return dt
        End Try
    End Function
    Private Function DoesNodeHaveSubcategory(ByRef subcategory As String, ByRef subcategoryList As String) As Boolean?
        Try
            'Return if list contains subcategory or not.
            Dim newList As String() = subcategoryList.Split(",")
            Return newList.Contains(subcategory)
        Catch ex As Exception

            Dim sb As New StringBuilder()
            sb.AppendLine("linqCampaigns.vb : DoesNodeHaveSubcategory()")
            sb.AppendLine("subcategory:" & subcategory)
            sb.AppendLine("subcategoryList" & subcategoryList)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

        End Try
    End Function
    Public Sub ObtainAllActiveCampaigns(ByRef thisNode As IPublishedContent, ByRef lstNodeIds As List(Of Integer), Optional ByRef _searchBy As String = "")
        Try
            Select Case thisNode.DocumentTypeAlias
                Case docTypes.Campaign
                    'If active, add to list
                    If (thisNode.GetPropertyValue(Of Boolean)(nodeProperties.published) = True) AndAlso
                        (thisNode.GetPropertyValue(Of Boolean)(nodeProperties.campaignComplete) = False) Then
                        If String.IsNullOrEmpty(_searchBy) Then
                            lstNodeIds.Add(thisNode.Id)
                        Else
                            'If search value exists, add to list if name contains value.
                            If thisNode.Name.ToLower.Contains(_searchBy.ToLower.Trim) Then lstNodeIds.Add(thisNode.Id)
                        End If
                    End If
                Case Else
                    'Recursive call to find all campaigns
                    For Each childNode As IPublishedContent In thisNode.Children
                        ObtainAllActiveCampaigns(childNode, lstNodeIds, _searchBy)
                    Next
            End Select
        Catch ex As Exception

            Dim sb As New StringBuilder()
            sb.AppendLine("linqCampaigns.vb : ObtainAllActiveCampaigns()")
            sb.AppendLine("thisNode:" & JsonConvert.SerializeObject(thisNode))
            sb.AppendLine("lstCampaignCategories" & JsonConvert.SerializeObject(lstNodeIds))
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

        End Try
    End Sub
    Public Sub ObtainAllActiveAndCompleteCampaigns(ByRef thisNode As IPublishedContent, ByRef lstNodeIds As List(Of Integer), Optional ByRef _searchBy As String = "")
        Try
            Select Case thisNode.DocumentTypeAlias
                Case docTypes.Campaign
                    'If active, add to list
                    If (thisNode.GetPropertyValue(Of Boolean)(nodeProperties.published) = True) Then
                        If String.IsNullOrEmpty(_searchBy) Then
                            lstNodeIds.Add(thisNode.Id)
                        Else
                            'If search value exists, add to list if name contains value.
                            If thisNode.Name.ToLower.Contains(_searchBy.ToLower.Trim) Then lstNodeIds.Add(thisNode.Id)
                        End If
                    End If
                Case Else
                    'Recursive call to find all campaigns
                    For Each childNode As IPublishedContent In thisNode.Children
                        ObtainAllActiveAndCompleteCampaigns(childNode, lstNodeIds, _searchBy)
                    Next
            End Select
        Catch ex As Exception

            Dim sb As New StringBuilder()
            sb.AppendLine("linqCampaigns.vb : ObtainAllActiveCampaigns()")
            sb.AppendLine("thisNode:" & JsonConvert.SerializeObject(thisNode))
            sb.AppendLine("lstCampaignCategories" & JsonConvert.SerializeObject(lstNodeIds))
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

        End Try
    End Sub
    Private Sub ObtainCampaignsAndCategories(ByRef thisNode As IPublishedContent, ByRef lstCampaignCategories As List(Of CampaignCategories))

        Try
            Select Case thisNode.DocumentTypeAlias
                Case docTypes.Campaign
                    'If active, add to list
                    If thisNode.HasProperty(nodeProperties.published) AndAlso
                        thisNode.GetPropertyValue(Of Boolean)(nodeProperties.published) = True Then

                        'Build campaign category class and add to list
                        Dim campaignCategories As New CampaignCategories With {
                            .nodeId = thisNode.Id
                        }
                        If thisNode.HasValue(SubcategoryConverter.Artistic) Then campaignCategories.Artistic = True
                        If thisNode.HasValue(SubcategoryConverter.Business) Then campaignCategories.Business = True
                        If thisNode.HasValue(SubcategoryConverter.Charity) Then campaignCategories.Charity = True
                        If thisNode.HasValue(SubcategoryConverter.Community) Then campaignCategories.Community = True
                        If thisNode.HasValue(SubcategoryConverter.Science) Then campaignCategories.Science = True
                        If thisNode.HasValue(SubcategoryConverter.SelfHelp) Then campaignCategories.SelfHelp = True
                        If thisNode.HasValue(SubcategoryConverter.Software) Then campaignCategories.Software = True
                        If thisNode.HasValue(SubcategoryConverter.Technology) Then campaignCategories.Technology = True
                        lstCampaignCategories.Add(campaignCategories)

                    End If
                Case Else
                    For Each childNode As IPublishedContent In thisNode.Children
                        ObtainCampaignsAndCategories(childNode, lstCampaignCategories)
                    Next
            End Select
        Catch ex As Exception

            Dim sb As New StringBuilder()
            sb.AppendLine("linqCampaigns.vb : ObtainCampaignsAndCategories()")
            sb.AppendLine("thisNode:" & JsonConvert.SerializeObject(thisNode))
            sb.AppendLine("lstCampaignCategories" & JsonConvert.SerializeObject(lstCampaignCategories))
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

        End Try
    End Sub
    Public Function IsCampaignACharity_byId(ByVal id As Integer) As Boolean
        Try
            Dim ipCampaign As IPublishedContent = _uHelper.Get_IPublishedContentByID(id)

            If ipCampaign.HasValue(SubcategoryConverter.Charity) Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqCampaigns.vb : IsCampaignACharity_byId()")
            sb.AppendLine("id:" & id)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return False
        End Try
    End Function
#End Region

End Class
