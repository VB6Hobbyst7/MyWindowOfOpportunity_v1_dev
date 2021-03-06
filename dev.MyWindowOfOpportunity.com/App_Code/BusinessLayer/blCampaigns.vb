Imports System.Data
Imports Common
Imports umbraco
Imports umbraco.Core
Imports umbraco.Core.Models
Imports umbraco.Core.Publishing
Imports System.Xml.XPath
Imports Newtonsoft.Json
Imports umbraco.Web
Imports Stripe
Imports umbraco.Core.Services

Public Class blCampaigns

#Region "Properties"
    Private linqCampaigns As New linqCampaigns
    Private blEmails As New blEmails
    Private _uHelper As Uhelper = New Uhelper()
#End Region


#Region "Selects"
    Public Function selectCampaigsSocialLink_byType(ByVal _nodeId As Integer, ByVal _socialMediaType As mediaType_Values) As String
        Return linqCampaigns.selectCampaigsSocialLink_byType(_nodeId, _socialMediaType)
    End Function
    Public Function selectTopTrendingCampaigns() As List(Of CampaignSummary)
        Return linqCampaigns.selectTopTrendingCampaigns
    End Function
    Public Function selectSampleCampaigns() As List(Of CampaignSummary)
        Return linqCampaigns.selectSampleCampaigns
    End Function
    Public Function selectAllActiveCampaigns() As BusinessReturn
        Return linqCampaigns.selectAllActiveCampaigns()
    End Function
    Public Function selectCampaignsBySearch(ByVal _searchBy As String) As BusinessReturn
        Return linqCampaigns.selectCampaignsBySearch(_searchBy)
    End Function
    Public Function selectCampaignsById(ByVal _campaignList As List(Of Integer)) As BusinessReturn
        Return linqCampaigns.selectCampaignsById(_campaignList)
    End Function
    Public Function selectCampaignsWithCatagories() As BusinessReturn
        Return linqCampaigns.selectCampaignsWithCatagories()
    End Function
    Public Function obtainRatingSummary_byCampaignId(ByVal _campaignId As Integer) As BusinessReturn
        Return linqCampaigns.obtainRatingSummary_byCampaignId(_campaignId)
    End Function
    Public Function selectCampaignSummary_byId(ByVal _campaignId As Integer) As BusinessReturn
        Return linqCampaigns.selectCampaignSummary_byId(_campaignId)
    End Function
    Public Function selectStripeUserId_byCampaignId(ByVal _campaignId As Integer) As String
        Return linqCampaigns.selectStripeUserId_byCampaignId(_campaignId)
    End Function
    Public Function obtainTeamsAndCampaigns_byUserId(ByVal userId As Integer) As BusinessReturn
        'Instantiate outter-scope variables
        Dim businessReturn As BusinessReturn = New BusinessReturn

        Try
            'Instantiate variables
            Dim lstTeamIDs As New List(Of Integer)
            Dim lstTeamAdminIDs As New List(Of Integer)
            Dim lstCampaignIDs As New List(Of Integer)
            Dim manageCampaigns As New ManageCampaigns

            '
            obtainTeamAndCampaignIDs(lstTeamIDs, lstTeamAdminIDs, lstCampaignIDs, userId)

            '
            For Each id As Integer In lstTeamAdminIDs
                manageCampaigns.lstTeamSummary.Add(linqCampaigns.obtainTeamManagedData_byId(id))
            Next

            '
            For Each id As Integer In lstCampaignIDs
                manageCampaigns.lstCampaignSummary.Add(linqCampaigns.obtainCampaignManagedData_byId(id))
            Next

            '
            businessReturn.DataContainer.Add(manageCampaigns)
            Return businessReturn

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("blCampaigns.vb : obtainTeamsAndCampaign_byUserId()")
            sb.AppendLine("userId: " & userId)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

            businessReturn.ExceptionMessage = "Error [blCampaigns.obtainTeamsAndCampaign_byUserId]: " & ex.ToString
            Return businessReturn
        End Try

        'Return linqCampaigns.obtainTeamsAndCampaign_byUserId(userId)
    End Function
    Public Function obtainAllCampaigns_byTeamId(ByVal teamId As Integer) As BusinessReturn
        Try
            'Instantiate variabes
            Dim ipTeamNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(teamId)
            Dim bReturn As New BusinessReturn
            Dim bReturn_Campaign As BusinessReturn

            'Obtain each campaign under team
            For Each ipCampaign As IPublishedContent In ipTeamNode.Children
                bReturn_Campaign = New BusinessReturn
                bReturn_Campaign = selectCampaignSummary_byId(ipCampaign.Id)

                If bReturn_Campaign.isValid Then
                    'Obtain campaign data and add to return
                    bReturn.DataContainer.Add(bReturn_Campaign.DataContainer(0))
                End If
            Next

            Return bReturn
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("blCampaigns.vb : obtainAllCampaigns_byTeamId()")
            sb.AppendLine("teamId: " & teamId)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return Nothing
        End Try
    End Function
#End Region

#Region "Insert"
    Public Function CreateCampaign(ByVal _teamId As Integer, ByVal _campaignName As String) As Integer?
        Return linqCampaigns.CreateCampaign(_teamId, _campaignName)
    End Function
#End Region

#Region "Updates"
    Public Function submitCategoryData(ByVal subCategory As String, ByVal cblList As List(Of String), ByVal nodeId As Integer) As Attempt(Of PublishStatus)

        'For Each cbl As CheckBoxList In phSubcategories.Controls
        'Instantiate variables
        'Dim cblList As List(Of String) = New List(Of String)
        Dim cblCsv As String = String.Empty

        'Add each checked item into the list
        'For Each item As ListItem In cbl.Items
        '    If item.Selected Then
        '        cblList.Add(item.Value)
        '    End If
        'Next

        Try
            'Join each item into a csv
            cblCsv = String.Join(",", cblList.ToArray())

            'Obtain category from checkboxlist attribute
            'Dim subCategory As String = cbl.Attributes("category")
            'Dim subCategory As String = pnl.Attributes("category")
            'Adjust subcategory value for saving to umbraco
            Select Case subCategory
                Case Categories.Arts
                    subCategory = SubcategoryConverter.Artistic
                Case Categories.Business
                    subCategory = SubcategoryConverter.Business
                Case Categories.Charity
                    subCategory = SubcategoryConverter.Charity
                Case Categories.Community
                    subCategory = SubcategoryConverter.Community
                Case Categories.Science
                    subCategory = SubcategoryConverter.Science
                Case Categories.SelfHelp
                    subCategory = SubcategoryConverter.SelfHelp
                Case Categories.Software
                    subCategory = SubcategoryConverter.Software
                Case Categories.Technology
                    subCategory = SubcategoryConverter.Technology
                Case Else
            End Select
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("blCampaigns.vb : submitCategoryData()")
            sb.AppendLine("subCategory: " & subCategory)
            sb.AppendLine("cblList :" & JsonConvert.SerializeObject(cblList))
            sb.AppendLine("NodeId : " & nodeId)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try


        'Submit data for updating
        Return UpdateCategories(nodeId, subCategory, cblCsv)
    End Function
    Public Function UpdateContent(ByVal nodeId As Int16, ByVal briefSummary As String, ByVal fullSummary As String, ByVal RemovePanelImg As Boolean, ByVal RemoveTopBannerImg As Boolean, ByVal customCss As String) As Attempt(Of PublishStatus)
        Return linqCampaigns.UpdateContent(nodeId, briefSummary, fullSummary, RemovePanelImg, RemoveTopBannerImg, customCss)
    End Function
    Public Function UpdateContent(ByVal nodeId As Int16, ByVal _campaignPublished As Boolean, ByVal _campaignComplete As Boolean) As Attempt(Of PublishStatus)
        Return linqCampaigns.UpdateContent(nodeId, _campaignPublished, _campaignComplete)
    End Function
    Public Function UpdateCategories(ByVal nodeId As Int16, ByVal subCategory As String, ByVal cblCsv As String) As Attempt(Of PublishStatus)
        Return linqCampaigns.UpdateCategories(nodeId, subCategory, cblCsv)
    End Function
    Public Function SaveFeaturedImage(ByVal _nodeId As Int16, ByVal _mediaId As Integer, ByVal propertyName As String) As Attempt(Of PublishStatus)
        Return linqCampaigns.SaveFeaturedImage(_nodeId, _mediaId, propertyName)
    End Function
    Public Function UpdateSocialMediaUrl(ByVal _nodeId As Int16, ByVal _socialMediaType As mediaType_Values, ByVal _url As String) As BusinessReturn
        'Instantiate variables
        Dim BusinessReturn As New BusinessReturn
        Try
            If _socialMediaType = mediaType_Values.SupportEmail Then
                BusinessReturn = ValidateEmail(_url)
            Else
                BusinessReturn = ValidateUrl(_url)
            End If

            If BusinessReturn.isValid Then
                Return linqCampaigns.UpdateSocialMediaUrl(_nodeId, _socialMediaType, _url)
            Else
                Return BusinessReturn
            End If

        Catch ex As Exception

            Dim sb As New StringBuilder()
            sb.AppendLine("blCampaigns.vb : UpdateSocialMediaUrl()")
            sb.AppendLine("_socialMediaType: " & _socialMediaType)
            sb.AppendLine("_url: " & _url)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return BusinessReturn

        End Try
    End Function
    Public Function SaveStripeAcctIDs(ByVal nodeId As Int16, ByVal stripeClientId As String) As BusinessReturn
        Return linqCampaigns.SaveStripeAcctIDs(nodeId, stripeClientId)
    End Function
    Public Sub nightlyUpdate()
        Try
            'Instantiate variables
            Dim lstCampaignIDs As New List(Of Integer)
            Dim blPhases As New blPhases
            Dim blPledges As New blPledges
            Dim blMembers As New blMembers

            'Obtain a list of all active campaign IDs
            linqCampaigns.ObtainAllActiveCampaigns(_uHelper.Get_IPublishedContentByID(siteNodes.Campaigns), lstCampaignIDs)

            'Loop thru each active campaign as IPublishedContent 
            For Each campaignId As Integer In lstCampaignIDs
                Try
                    'Obtain campaign detail and active phase Id
                    Dim ipCampaign As IPublishedContent = _uHelper.Get_IPublishedContentByID(campaignId)
                    Dim activePhaseId As Integer? = blPhases.obtainActivePhaseId_byCampaignId(campaignId)

                    'Proceed if campaign has an active phase
                    If Not IsNothing(activePhaseId) AndAlso activePhaseId <> 0 Then
                        'Obtain active phase in campaign
                        Dim ipActivePhase As IPublishedContent = _uHelper.Get_IPublishedContentByID(activePhaseId)

                        'Obtain phase's completion date. convert dateTime to only date
                        Dim phaseDate As DateTime = ipActivePhase.GetPropertyValue(Of DateTime)(nodeProperties.completionDate)
                        Dim phaseCompletionDate As New Date(phaseDate.Year, phaseDate.Month, phaseDate.Day)

                        If phaseCompletionDate.Year > 1 Then
                            'Is this campaign a charity?
                            Dim blCampaigns As New blCampaigns
                            Dim isCharity As Boolean = blCampaigns.IsCampaignACharity_byId(campaignId)
                            Dim proceed As Boolean = False

                            'Determine if phase can be processed
                            proceed = Date.Today >= phaseCompletionDate

                            'Proceed if phase is complete (of if enough time has elapsed for charity).
                            If proceed Then

                                'Obtain list of team admin node IDs for sending emails
                                Dim lstTeamAdministrators As List(Of Integer) = ipCampaign.Parent.GetPropertyValue(Of String)(nodeProperties.teamAdministrators).Split(",").ToList.ConvertAll(Function(str) Int32.Parse(str))
                                For Each memberId As Integer In obtainCampaignManagers_byCampaignId(campaignId)
                                    If Not lstTeamAdministrators.Contains(memberId) Then
                                        lstTeamAdministrators.Add(memberId)
                                    End If
                                Next

                                'Is acct a charity or normal acct.
                                If isCharity Then
                                    'Is campaign past completion date
                                    If Date.Today >= phaseCompletionDate Then
                                        'Update active phase
                                        blPhases.UpdateSuccessfulPhase(ipActivePhase)

                                        'Mark campaign as complete
                                        UpdateContent(ipCampaign.Id, True, True)

                                        'Send email to administators.
                                        blEmails.sendCampaignEmail_byLstTeamAdmins_SuccessfulCampaign(lstTeamAdministrators)
                                    End If

                                    'Create a fund payout record with extended date for transfering.
                                    createFundPayoutRequest(ipActivePhase, ipCampaign)

                                Else
                                    'Mark active phase as complete
                                    blPhases.UpdateSuccessfulPhase(ipActivePhase)

                                    'Is there any pending phases?
                                    If blPhases.anyPendingPhases(ipCampaign) Then
                                        'YES | Send email to administators.
                                        blEmails.sendCampaignEmail_byLstTeamAdmins_SuccessfulPhase_LaunchNext(lstTeamAdministrators)
                                    Else
                                        'NO | mark campaign as complete
                                        UpdateContent(ipCampaign.Id, True, True)

                                        'Send email to administators.
                                        blEmails.sendCampaignEmail_byLstTeamAdmins_SuccessfulCampaign(lstTeamAdministrators)
                                    End If

                                    'Create a fund payout record with extended date for transfering.
                                    createFundPayoutRequest(ipActivePhase, ipCampaign)
                                End If

                            End If
                        End If
                    Else
                        'Does the campaign have an active discovery phase?
                        Dim bReturn As BusinessReturn = blPhases.obtainDiscoveryPhase_byCampaignId(campaignId)
                        If bReturn.isValid Then
                            'Extract data
                            Dim ipDiscoveryPhase As IPublishedContent = DirectCast(bReturn.DataContainer(0), IPublishedContent)

                            'Is phase active?
                            If ipDiscoveryPhase.GetPropertyValue(Of Boolean)(nodeProperties.phaseActive) Then
                                'Is phase past its completion date.
                                If ipDiscoveryPhase.HasValue(nodeProperties.completionDate) Then
                                    If Date.Today >= ipDiscoveryPhase.GetPropertyValue(Of Date)(nodeProperties.completionDate) Then
                                        'Deactivate phase
                                        blPhases.DeactivateDiscoveryPhase(ipDiscoveryPhase.Id, Date.Now & " | Phase complete", True)

                                        'Obtain list of team admin node IDs for sending emails
                                        Dim lstTeamAdministrators As List(Of Integer) = ipCampaign.Parent.GetPropertyValue(Of String)(nodeProperties.teamAdministrators).Split(",").ToList.ConvertAll(Function(str) Int32.Parse(str))
                                        For Each memberId As Integer In obtainCampaignManagers_byCampaignId(campaignId)
                                            If Not lstTeamAdministrators.Contains(memberId) Then
                                                lstTeamAdministrators.Add(memberId)
                                            End If
                                        Next

                                        'Send email to admins
                                        blEmails.sendCampaignEmail_byLstTeamAdmins_DiscoveryPhaseSuccessful(lstTeamAdministrators)
                                    End If
                                Else
                                    'Deactivate phase
                                    blPhases.DeactivateDiscoveryPhase(ipDiscoveryPhase.Id, Date.Now & " | Phase complete (Completion date was missing)", True)
                                End If
                            Else
                                'Is there any pending campaigns?
                                If Not blPhases.pendingPhasesExist_byCampaignId(campaignId) Then
                                    'No more pending phases.  Mark campaign as complete
                                    UpdateContent(campaignId, True, True)

                                    'Obtain list of team admin node IDs for sending emails
                                    Dim lstTeamAdministrators As List(Of Integer) = ipCampaign.Parent.GetPropertyValue(Of String)(nodeProperties.teamAdministrators).Split(",").ToList.ConvertAll(Function(str) Int32.Parse(str))
                                    For Each memberId As Integer In obtainCampaignManagers_byCampaignId(campaignId)
                                        If Not lstTeamAdministrators.Contains(memberId) Then
                                            lstTeamAdministrators.Add(memberId)
                                        End If
                                    Next

                                    'Send email to administators.
                                    blEmails.sendCampaignEmail_byLstTeamAdmins_SuccessfulCampaign(lstTeamAdministrators)
                                End If
                            End If
                        End If
                    End If
                Catch exCampaign As Exception
                    Dim sbCampaign As New StringBuilder()
                    sbCampaign.AppendLine("blCampaigns.vb : nightlyUpdate() - For Each Campaign")
                    sbCampaign.AppendLine("Campaign Id: " & campaignId)

                    Dim ipCampaign As IPublishedContent = _uHelper.Get_IPublishedContentByID(campaignId)
                    If Not IsNothing(ipCampaign) Then
                        sbCampaign.AppendLine("Campaign: " & ipCampaign.Name)
                        sbCampaign.AppendLine("Team: " & ipCampaign.Parent.Name)
                    Else
                        sbCampaign.AppendLine("Campaign id [" & campaignId & "] is nothing.")
                    End If
                    saveErrorMessage(getLoggedInMember, exCampaign.ToString, sbCampaign.ToString())
                End Try
            Next
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("blCampaigns.vb : nightlyUpdate()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
    End Sub
    Private Sub createFundPayoutRequest(ByVal ipActivePhase As IPublishedContent, ByVal ipCampaign As IPublishedContent)
        Try
            'Instantiate variables
            Dim blPledges As New blPledges
            Dim lstTransfers As New List(Of StripeTransfer)
            Dim lstPledges As List(Of CampaignPledge)
            Dim allTransfers As StripeList(Of StripeTransfer)
            Dim payoutTotal As Integer = 0

            'Obtain list of all valid pledge IDs for this phase from umbraco
            lstPledges = blPledges.obtainIncompletePledges_byPhase(ipActivePhase.Id)

            If lstPledges.Count > 0 Then
                'Obtain all transactions
                allTransfers = blPledges.obtainAllTransfers(ipCampaign)

                If allTransfers.Count > 0 Then
                    'Extract only transfers which have a matching charge id
                    For Each stripeTransfer As StripeTransfer In allTransfers
                        For Each pledge As CampaignPledge In lstPledges
                            If pledge.chargeId = stripeTransfer.SourceTransactionId Then
                                lstTransfers.Add(stripeTransfer)
                                pledge.transfered = True
                                Exit For
                            End If
                        Next
                    Next

                    If lstTransfers.Count > 0 Then
                        'Obtain transfer total
                        For Each stripeTransfer As StripeTransfer In lstTransfers
                            payoutTotal += stripeTransfer.Amount
                        Next
                        If payoutTotal > 0 Then
                            'Create # of business days to add
                            Dim additionalDays As Int16 = 3
                            Select Case Date.Today.DayOfWeek
                                Case DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Tuesday
                                    'keep normal value
                                Case DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday
                                    additionalDays += 2
                                Case DayOfWeek.Saturday
                                    additionalDays += 1
                            End Select

                            'Create transfer record to be done at a later date
                            Dim payout As New Payout
                            payout.activePhase = ipActivePhase
                            payout.campaign = ipCampaign
                            payout.dateToProcess = Date.Today.AddDays(additionalDays)
                            payout.payoutTotal = payoutTotal

                            'Create transfer request
                            blPledges.createPayoutRequest(payout)

                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("blCampaigns.vb : createFundPayoutRequest()")
            sb.AppendLine("ipCampaign: " & ipCampaign.Id & " | " & ipCampaign.Name)
            sb.AppendLine("ipActivePhase: " & ipActivePhase.Id & " | " & ipActivePhase.Name)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
    End Sub
#End Region

#Region "Delete"
#End Region

#Region "Methods"
    Public Function doesCampaignExist_byName(ByVal _campaignName As String) As Boolean?
        Return linqCampaigns.doesCampaignExist_byName(_campaignName)
    End Function
    Private Sub obtainTeamAndCampaignIDs(ByRef lstTeamIDs As List(Of Integer), ByRef lstTeamAdminIDs As List(Of Integer), ByRef lstCampaignIDs As List(Of Integer),
                                         ByVal userId As Integer, Optional ByRef ipContent As IPublishedContent = Nothing, Optional ByVal isAdmin As Boolean = False)
        Try
            'If ip is nothing, set as root campaign folder
            If IsNothing(ipContent) Then
                ipContent = _uHelper.Get_IPublishedContentByID(siteNodes.Campaigns)
            End If

            '
            Select Case ipContent.DocumentTypeAlias
                Case docTypes.Campaigns
                    For Each ipChild In ipContent.Children
                        obtainTeamAndCampaignIDs(lstTeamIDs, lstTeamAdminIDs, lstCampaignIDs, userId, ipChild)
                    Next

                Case docTypes.AlphaFolder
                    For Each ipChild In ipContent.Children
                        obtainTeamAndCampaignIDs(lstTeamIDs, lstTeamAdminIDs, lstCampaignIDs, userId, ipChild)
                    Next

                Case docTypes.Team


                    'Check if member is a team administrator
                    If ipContent.HasProperty(nodeProperties.teamAdministrators) Then

                        Dim lstMembers As List(Of Integer) = ipContent.GetPropertyValue(Of String)(nodeProperties.teamAdministrators).Split(",").ToList.ConvertAll(Function(str) Int32.Parse(str))

                        If Not IsNothing(lstMembers) Then
                            If lstMembers.Contains(userId) Then
                                'Save IDs
                                lstTeamIDs.Add(ipContent.Id)
                                lstTeamAdminIDs.Add(ipContent.Id)
                                isAdmin = True
                            End If
                        End If
                    End If

                    For Each ipChild In ipContent.Children
                        obtainTeamAndCampaignIDs(lstTeamIDs, lstTeamAdminIDs, lstCampaignIDs, userId, ipChild, isAdmin)
                    Next

                Case docTypes.Campaign
                    If isAdmin Then
                        'Automatically add campaign since user is an admin
                        lstCampaignIDs.Add(ipContent.Id)
                    Else
                        'Obtain member's list for campaign
                        Dim campaignMembers As IPublishedContent = ipContent.Children.Where(Function(x) x.DocumentTypeAlias = docTypes.campaignMembers).FirstOrDefault
                        If Not IsNothing(campaignMembers) Then
                            'If any of the campaign member's match the user, add campaign to list, but not as admin
                            If campaignMembers.Children.Any(Function(x) x.GetPropertyValue(Of Integer)(nodeProperties.campaignMember) = userId) Then
                                If Not lstCampaignIDs.Contains(ipContent.Id) Then lstCampaignIDs.Add(ipContent.Id)
                                If Not lstTeamIDs.Contains(ipContent.Parent.Id) Then lstTeamIDs.Add(ipContent.Parent.Id)
                            End If
                        End If
                    End If

            End Select
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("blCampaigns.vb : obtainTeamAndCampaignIDs()")
            sb.AppendLine("userId: " & userId)
            sb.AppendLine("ipContent.Id: " & ipContent.Id)
            sb.AppendLine("isAdmin: " & isAdmin.ToString)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try

    End Sub
    Public Function obtainStatistics(ByRef thisNode As IPublishedContent, Optional ByVal _getPhases As Boolean = False, Optional ByVal _getPledges As Boolean = False) As BusinessReturn
        Dim BusinessReturn As BusinessReturn = New BusinessReturn
        Try
            If Not IsNothing(thisNode) Then

                'Instantiate variables
                Dim CampaignStatistics As CampaignStatistics = New CampaignStatistics
                Dim blPledges As blPledges = New blPledges
                Dim blPhases As blPhases = New blPhases
                Dim campaignGoal As Decimal = 0
                Dim campaignPledges As Decimal = 0
                Dim campaignContributors As UInt16 = 0
                Dim lstCampaignPledges As List(Of CampaignPledge)

                'Add campaign id to class
                CampaignStatistics.campaignId = thisNode.Id

                'find Phases folder
                For Each phases As IPublishedContent In thisNode.Children
                    If phases.DocumentTypeAlias = docTypes.Phases Then
                        For Each phase As IPublishedContent In phases.Children
                            If phase.GetPropertyValue(Of Boolean)(nodeProperties.published) = True Then
                                'add phase goal to total goal
                                campaignGoal += phase.GetPropertyValue(Of Decimal)(nodeProperties.goal)

                                'Obtain list & count of all pledges for phase.
                                lstCampaignPledges = blPledges.select_byPhaseId(phase.Id, True)
                                For Each pledge As CampaignPledge In lstCampaignPledges
                                    campaignContributors += 1
                                    campaignPledges += pledge.pledgeAmount
                                Next
                            End If
                        Next
                        Exit For
                    End If
                Next



                'Add values to class
                CampaignStatistics.campaignGoal = campaignGoal
                CampaignStatistics.contributions = campaignContributors
                CampaignStatistics.currentPledges = campaignPledges

                'Determine % funded
                If campaignGoal > 0 Then
                    CampaignStatistics.funded = CInt((campaignPledges / campaignGoal) * 100) '.ToString.Split(".")
                End If

                '
                If thisNode.HasProperty(nodeProperties.published) Then CampaignStatistics.published = thisNode.GetPropertyValue(Of Boolean)(nodeProperties.published)
                If thisNode.HasProperty(nodeProperties.campaignComplete) Then CampaignStatistics.complete = thisNode.GetPropertyValue(Of Boolean)(nodeProperties.campaignComplete)

                If _getPhases Then
                    CampaignStatistics.lstPhases = blPhases.obtainPhaseStatuses(thisNode.Id, _getPledges)
                End If

                'Campaign activity dates
                If thisNode.HasProperty(nodeProperties.datePublished) AndAlso thisNode.HasValue(nodeProperties.datePublished) Then
                    CampaignStatistics.publishDate = thisNode.GetPropertyValue(Of Date)(nodeProperties.datePublished)
                End If

                If thisNode.HasProperty(nodeProperties.completionDate) AndAlso thisNode.HasValue(nodeProperties.completionDate) Then
                    CampaignStatistics.completionDate = thisNode.GetPropertyValue(Of Date)(nodeProperties.completionDate).ToLongDateString
                    'Else
                    '    ltrlCompletionDate.Text = "TBD"
                End If

                'Obtain Discovery Phase's status
                For Each childNode As IPublishedContent In thisNode.Children.OfTypes(docTypes.discovery)
                    CampaignStatistics.DiscoveryPhase.nodeId = childNode.Id
                    'If childNode.HasProperty(nodeProperties.phaseActive) Then
                    CampaignStatistics.DiscoveryPhase.phaseActive = childNode.GetPropertyValue(Of Boolean)(nodeProperties.phaseActive)
                    'If CampaignStatistics.DiscoveryPhase.phaseActive Then
                    CampaignStatistics.DiscoveryPhase.activationDate = childNode.GetPropertyValue(Of Date)(nodeProperties.activationDate)
                    CampaignStatistics.DiscoveryPhase.completionDate = childNode.GetPropertyValue(Of Date)(nodeProperties.completionDate)

                    'Instantiate variables
                    Dim stars1 As Int16 = 0
                    Dim stars2 As Int16 = 0
                    Dim stars3 As Int16 = 0
                    Dim stars4 As Int16 = 0
                    Dim stars5 As Int16 = 0
                    Dim starAvg As Single = 0

                    'Retrieve all review ratings.  (not comments)
                    For Each reviewNode As IPublishedContent In childNode.Children
                        Dim rating As New Rating With {
                            .stars = reviewNode.GetPropertyValue(Of Integer)(nodeProperties.stars)
                        }
                        CampaignStatistics.DiscoveryPhase.lstRatings.Add(rating)
                    Next

                    'Calculate star rating out of 5
                    'Determine rating totals
                    If CampaignStatistics.DiscoveryPhase.lstRatings.Count > 0 Then
                        'Loop through each rating value
                        For Each rating In CampaignStatistics.DiscoveryPhase.lstRatings
                            'Count individual star values
                            Select Case rating.stars
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
                            starAvg += rating.stars
                        Next

                        'Calculate average stars
                        CampaignStatistics.DiscoveryPhase.rating = Math.Round(starAvg / CampaignStatistics.DiscoveryPhase.lstRatings.Count, 1)
                    End If
                    Exit For
                Next

                '
                If thisNode.HasProperty(nodeProperties.stripeUserId) AndAlso Not String.IsNullOrEmpty(thisNode.GetPropertyValue(Of String)(nodeProperties.stripeUserId)) Then
                    CampaignStatistics.stripeUserId = thisNode.GetPropertyValue(Of String)(nodeProperties.stripeUserId)
                    CampaignStatistics.stripeUserIdProvided = True
                End If

                'Determine status
                CampaignStatistics.statusType = obtainStatusType(CampaignStatistics)
                'Determine previous status' result
                CampaignStatistics.previousStatusType = obtainPreviousPhaseStatus(CampaignStatistics)

                '
                BusinessReturn.DataContainer.Add(CampaignStatistics)
            Else
                BusinessReturn.ExceptionMessage = "no valid iPublishContent"
            End If

            Return BusinessReturn
        Catch ex As Exception
            BusinessReturn.ExceptionMessage = ex.ToString

            Dim sb As New StringBuilder()
            sb.AppendLine("blCampaigns.vb : obtainStatistics()")
            If Not IsNothing(thisNode) Then sb.AppendLine("thisNode:" & thisNode.Id)
            sb.AppendLine("_getPhases:" & _getPhases.ToString())
            sb.AppendLine("_getPledges:" & _getPledges.ToString())

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            ' saveErrorMessage(String.Empty, ex.ToString, String.Empty)
            Return BusinessReturn
        End Try

    End Function
    Private Function obtainStatusType(ByRef CampaignStatistics As CampaignStatistics) As statusType
        Try
            'Determine what status the campaign's lifecycle is in.
            If CampaignStatistics.complete Then
                Return statusType.Complete

            ElseIf CampaignStatistics.DiscoveryPhase.phaseActive Then
                Return statusType.DiscoveryPhase

            ElseIf Not CampaignStatistics.stripeUserIdProvided Then
                Return statusType.AccountSetup

            ElseIf CampaignStatistics.published Then
                'Instantiate local variables
                Dim complete As Boolean = False
                Dim active As Boolean = False
                Dim phaseCount As UInt16 = CampaignStatistics.lstPhases.Count

                While phaseCount > 0
                    'Decrement counter 
                    phaseCount -= 1

                    'Exit loop if either are true
                    If CampaignStatistics.lstPhases(phaseCount).complete Then
                        complete = True
                        Exit While
                    ElseIf CampaignStatistics.lstPhases(phaseCount).active Then
                        active = True
                        Exit While
                    End If
                End While

                'Adjust counter to standard
                phaseCount += 1

                'Dim sb As New StringBuilder()
                'sb.AppendLine("blCampaigns.vb : obtainStatusType()")
                'sb.AppendLine("phaseCount:" & phaseCount.ToString())
                'sb.AppendLine("complete:" & complete.ToString())
                'sb.AppendLine("active:" & active.ToString())
                'saveErrorMessage("", "TEST", sb.ToString())

                '
                If complete Then
                    Select Case phaseCount
                        Case 3
                            Return statusType.Complete
                        Case 2
                            Return statusType.Phase3Pending
                        Case 1
                            Return statusType.Phase2Pending
                        Case Else
                            Return Nothing
                    End Select
                ElseIf active Then
                    Select Case phaseCount
                        Case 3
                            Return statusType.Phase3Active
                        Case 2
                            Return statusType.Phase2Active
                        Case 1
                            Return statusType.Phase1Active
                        Case Else
                            Return Nothing
                    End Select
                Else
                    Return statusType.Phase1Pending
                End If
            Else
                Return statusType.Unpublished
            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("blCampaigns.vb : obtainStatusType()")
            sb.AppendLine("CampaignStatistics:" & JsonConvert.SerializeObject(CampaignStatistics))
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

            Return Nothing
        End Try
    End Function
    Private Function obtainPreviousPhaseStatus(ByRef CampaignStatistics As CampaignStatistics) As statusType
        Try

            Select Case CampaignStatistics.statusType
                Case statusType.Phase1Pending
                    If CampaignStatistics.lstPhases.Count > 0 AndAlso CampaignStatistics.lstPhases(0).lstPreviousPhases.Any(Function(x) x.phaseNumber = 1) Then
                        Return statusType.Phase1Failed '|Previous|	Phase 1 failed
                    ElseIf CampaignStatistics.DiscoveryPhase.nodeId > 0 Then
                        Dim tempNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(CampaignStatistics.DiscoveryPhase.nodeId)
                        Dim tempCompletionDate As Date = tempNode.GetPropertyValue(Of Date)(nodeProperties.completionDate)
                        If tempCompletionDate.Year > 2000 Then
                            Return statusType.DiscoveryPhase '|Previous|	Discovery Phase
                        End If
                        Return statusType.none
                    End If

                Case statusType.Phase2Pending
                    If CampaignStatistics.lstPhases.Count > 1 AndAlso CampaignStatistics.lstPhases(1).lstPreviousPhases.Any(Function(x) x.phaseNumber = 2) Then
                        Return statusType.Phase2Failed '|Previous|	Phase 2 failed
                    ElseIf CampaignStatistics.lstPhases(0).complete = True Then
                        Return statusType.Phase1Succeeded '|Previous|	Phase 1 succeeded
                    End If

                Case statusType.Phase3Pending
                    If CampaignStatistics.lstPhases.Count > 2 AndAlso CampaignStatistics.lstPhases(2).lstPreviousPhases.Any(Function(x) x.phaseNumber = 3) Then
                        Return statusType.Phase2Failed '|Previous|	Phase 3 failed
                    ElseIf CampaignStatistics.lstPhases(1).complete = True Then
                        Return statusType.Phase1Succeeded '|Previous|	Phase 2 succeeded
                    End If

            End Select

            Return statusType.none
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("blCampaigns.vb : obtainPreviousPhaseStatus()")
            sb.AppendLine("CampaignStatistics:" & JsonConvert.SerializeObject(CampaignStatistics))
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

            Return Nothing
        End Try
    End Function
    Public Function obtainStatusType_byId(ByVal campaignNodeId As Integer) As statusType
        Try
            'Instantiate variables
            Dim campaignNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(campaignNodeId)
            Dim campaigPublished As Boolean = False
            Dim campaigComplete As Boolean = False
            Dim discoveryPhaseActive As Boolean = False
            Dim blPhases As New blPhases

            'Determine if campaign is published or complete
            If campaignNode.HasProperty(nodeProperties.published) Then campaigPublished = campaignNode.GetPropertyValue(Of Boolean)(nodeProperties.published)
            If campaignNode.HasProperty(nodeProperties.campaignComplete) Then campaigComplete = campaignNode.GetPropertyValue(Of Boolean)(nodeProperties.campaignComplete)

            'Obtain Discovery Phase's status
            For Each childNode As IPublishedContent In campaignNode.Children.OfTypes(docTypes.discovery)
                If childNode.HasProperty(nodeProperties.phaseActive) Then
                    discoveryPhaseActive = childNode.GetPropertyValue(Of Boolean)(nodeProperties.phaseActive)
                End If
            Next

            'Determine what status the campaign's lifecycle is in.
            If campaigComplete Then
                Return statusType.Complete

            ElseIf discoveryPhaseActive Then
                Return statusType.DiscoveryPhase

            ElseIf campaigPublished Then
                'Instantiate local variables
                Dim complete As Boolean = False
                Dim active As Boolean = False
                Dim lstPhases As New List(Of Phase)
                Dim phaseCount As UInt16 = 0 'CampaignStatistics.lstPhases.Count

                'Obtain phases
                lstPhases = blPhases.obtainPhaseStatuses(campaignNodeId, False)
                phaseCount = lstPhases.Count

                While phaseCount > 0
                    'Decrement counter 
                    phaseCount -= 1

                    'Exit loop if either are true
                    If lstPhases(phaseCount).complete Then
                        complete = True
                        Exit While
                    ElseIf lstPhases(phaseCount).active Then
                        active = True
                        Exit While
                    End If
                End While

                'Adjust counter to standard
                phaseCount += 1

                '
                If complete Then
                    Select Case phaseCount
                        Case 3
                            Return statusType.Complete
                        Case 2
                            Return statusType.Phase3Pending
                        Case 1
                            Return statusType.Phase2Pending
                        Case Else
                            Return Nothing
                    End Select
                ElseIf active Then
                    Select Case phaseCount
                        Case 3
                            Return statusType.Phase3Active
                        Case 2
                            Return statusType.Phase2Active
                        Case 1
                            Return statusType.Phase1Active
                        Case Else
                            Return Nothing
                    End Select
                Else
                    Return statusType.Phase1Pending
                End If
            Else
                Return statusType.Unpublished
            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("blCampaigns.vb : obtainStatusType_byId()")
            sb.AppendLine("CampaignNodeId:" & campaignNodeId)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

            Return Nothing
        End Try
    End Function
    Public Function ObtainFullList() As DataTable
        'Instantiate variables
        Dim dt As New CampaignList.dtCampaignsDataTable

        'loop thru campaigns, obtain all
        Try
            dt = linqCampaigns.ObtainCampaigns(dt, _uHelper.Get_IPublishedContentByID(siteNodes.Campaigns))
        Catch ex As Exception

            Dim sb As New StringBuilder()
            sb.AppendLine("blCampaigns.vb : ObtainFullList()")

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try


        Return dt
    End Function
    Public Function obtainCategories(ByVal datatypeId As Integer) As ListItemCollection
        Try
            'Instantiate variables
            Dim preValueRootElementIterator As XPathNodeIterator
            Dim preValueIterator As XPathNodeIterator
            Dim listItems As New ListItemCollection()

            'Obtain datatype prevalue.  (Must move to first object entry or else iterator will return null.)
            preValueRootElementIterator = umbraco.library.GetPreValues(datatypeId)
            preValueRootElementIterator.MoveNext()

            'Obtain datatype's prevalues as list
            preValueIterator = preValueRootElementIterator.Current.SelectChildren("preValue", "")

            'Loop thru list and obtain a key/value set.
            While preValueIterator.MoveNext()
                listItems.Add(New ListItem(preValueIterator.Current.Value.ToUpper, preValueIterator.Current.Value))
                'listItems.Add(New ListItem(preValueIterator.Current.Value.ToUpper, preValueIterator.Current.Value.Replace(" ", "")))
            End While

            Return listItems

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("blCampaigns.vb : obtainCategories()")
            sb.AppendLine("datatypeId: " & datatypeId)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

            Return Nothing
        End Try
    End Function
    Public Function showListBySubcategory(ByVal selectedCategory As String) As BusinessReturn
        'Instantiate variables
        Dim subcategoryListItems As ListItemCollection
        Dim lstNodeIds As List(Of Integer)
        Dim businessReturn As New BusinessReturn

        Try
            'Obtain subcategories
            Select Case selectedCategory 'ddlCategories.SelectedValue
                Case Categories.Arts
                    subcategoryListItems = obtainCategories(prevalues.Subcategories_Artistic)
                Case Categories.Business
                    subcategoryListItems = obtainCategories(prevalues.Subcategories_Business)
                Case Categories.Charity
                    subcategoryListItems = obtainCategories(prevalues.Subcategories_Charity)
                Case Categories.Community
                    subcategoryListItems = obtainCategories(prevalues.Subcategories_Community)
                Case Categories.Science
                    subcategoryListItems = obtainCategories(prevalues.Subcategories_Science)
                Case Categories.SelfHelp
                    subcategoryListItems = obtainCategories(prevalues.Subcategories_SelfHelp)
                Case Categories.Software
                    subcategoryListItems = obtainCategories(prevalues.Subcategories_Software)
                Case Categories.Technology
                    subcategoryListItems = obtainCategories(prevalues.Subcategories_Technology)
                Case Else
                    subcategoryListItems = New ListItemCollection
            End Select

            'Obtain list of all active campaigns
            Dim dt As CampaignList.dtCampaignsDataTable = ObtainPartialList(selectedCategory) 'ddlCategories.SelectedValue)

            'Sort table
            dt.DefaultView.Sort = Miscellaneous.creationDate & " desc"

            'For each subcategory:
            For Each subcategoryItem As ListItem In subcategoryListItems
                'Instantiate variables
                lstNodeIds = New List(Of Integer) 'New CampaignList.dtCampaignListDataTable
                Dim subcategory As String = subcategoryItem.Value.ToLower 'Obtain category name

                'Get top N
                Select Case selectedCategory 'ddlCategories.SelectedValue
                    Case Categories.Arts
                        lstNodeIds = GetTopNOf_Artistic(subcategory, dt)
                    Case Categories.Business
                        lstNodeIds = GetTopNOf_Business(subcategory, dt)
                    Case Categories.Charity
                        lstNodeIds = GetTopNOf_Charity(subcategory, dt)
                    Case Categories.Community
                        lstNodeIds = GetTopNOf_Community(subcategory, dt)
                    Case Categories.Science
                        lstNodeIds = GetTopNOf_Science(subcategory, dt)
                    Case Categories.SelfHelp
                        lstNodeIds = GetTopNOf_SelfHelp(subcategory, dt)
                    Case Categories.Software
                        lstNodeIds = GetTopNOf_Software(subcategory, dt)
                    Case Categories.Technology
                        lstNodeIds = GetTopNOf_Technology(subcategory, dt)
                    Case Else
                        'subcategoryListItems = New ListItemCollection
                End Select

                'If records exist, add to data container
                If lstNodeIds.Count > 0 Then
                    'Add both the nodeId list and concatenate the sucategory name
                    businessReturn.DataContainer.Add(lstNodeIds)
                    businessReturn.ReturnMessage += "," & UppercaseFirstLetter(subcategory)
                End If
            Next

            Return businessReturn

        Catch ex As Exception

            Dim sb As New StringBuilder()
            sb.AppendLine("blCampaigns.vb : showListBySubcategory()")
            sb.AppendLine("selectedCategory:" & selectedCategory)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            businessReturn.ExceptionMessage = "Error [blCampaigns.showListBySubcategory]: " & ex.ToString
            Return businessReturn

        End Try

    End Function
    Public Function showListByFullcategory(ByVal selectedCategory As String) As BusinessReturn
        'Instantiate variables
        Dim subcategoryListItems As ListItemCollection
        Dim dtNodeIds As List(Of Integer)  'CampaignList.dtCampaignListDataTable
        Dim businessReturn As New BusinessReturn

        Try


            'Obtain subcategories
            Select Case selectedCategory 'ddlCategories.SelectedValue
                Case Categories.Arts
                    subcategoryListItems = obtainCategories(prevalues.Subcategories_Artistic)
                Case Categories.Business
                    subcategoryListItems = obtainCategories(prevalues.Subcategories_Business)
                Case Categories.Charity
                    subcategoryListItems = obtainCategories(prevalues.Subcategories_Charity)
                Case Categories.Community
                    subcategoryListItems = obtainCategories(prevalues.Subcategories_Community)
                Case Categories.Science
                    subcategoryListItems = obtainCategories(prevalues.Subcategories_Science)
                Case Categories.SelfHelp
                    subcategoryListItems = obtainCategories(prevalues.Subcategories_SelfHelp)
                Case Categories.Software
                    subcategoryListItems = obtainCategories(prevalues.Subcategories_Software)
                Case Categories.Technology
                    subcategoryListItems = obtainCategories(prevalues.Subcategories_Technology)
                Case Else
                    subcategoryListItems = New ListItemCollection
            End Select

            'Obtain list of all active campaigns
            Dim dt As CampaignList.dtCampaignsDataTable = ObtainPartialList(selectedCategory) 'ddlCategories.SelectedValue)

            'Sort table
            dt.DefaultView.Sort = Miscellaneous.creationDate & " desc"

            'For each subcategory:
            For Each subcategoryItem As ListItem In subcategoryListItems
                'Instantiate variables
                dtNodeIds = New List(Of Integer) 'New CampaignList.dtCampaignListDataTable
                Dim subcategory As String = subcategoryItem.Value.ToLower 'Obtain category name

                'Get top N
                Select Case selectedCategory 'ddlCategories.SelectedValue
                    Case Categories.Arts
                        dtNodeIds = GetTopNOf_Artistic(subcategory, dt)
                    Case Categories.Business
                        dtNodeIds = GetTopNOf_Business(subcategory, dt)
                    Case Categories.Charity
                        dtNodeIds = GetTopNOf_Charity(subcategory, dt)
                    Case Categories.Community
                        dtNodeIds = GetTopNOf_Community(subcategory, dt)
                    Case Categories.Science
                        dtNodeIds = GetTopNOf_Science(subcategory, dt)
                    Case Categories.SelfHelp
                        dtNodeIds = GetTopNOf_SelfHelp(subcategory, dt)
                    Case Categories.Software
                        dtNodeIds = GetTopNOf_Software(subcategory, dt)
                    Case Categories.Technology
                        dtNodeIds = GetTopNOf_Technology(subcategory, dt)
                    Case Else
                        'subcategoryListItems = New ListItemCollection
                End Select

                'If records exist, add to data container
                If dtNodeIds.Count > 0 Then
                    'Add both the nodeId list and concatenate the sucategory name
                    businessReturn.DataContainer.Add(dtNodeIds)
                    businessReturn.ReturnMessage += "," & UppercaseFirstLetter(subcategory)
                End If
            Next


            Return businessReturn

        Catch ex As Exception

            Dim sb As New StringBuilder()
            sb.AppendLine("/App_Code/BusinessLayer/blCampaigns.vb : showListByFullcategory()")
            sb.AppendLine("selectedCategory:" & selectedCategory)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            businessReturn.ExceptionMessage = "Error [blCampaigns.showListBySubcategory]: " & ex.ToString
            Return businessReturn

        End Try

    End Function
    Public Function ObtainPartialList(ByRef category As String, Optional ByRef subcategory As String = "") As DataTable
        'Instantiate variables
        Dim dt As New CampaignList.dtCampaignsDataTable
        Try
            'loop thru campaigns, obtain all
            dt = ObtainCampaigns_byCategory(dt, _uHelper.Get_IPublishedContentByID(siteNodes.Campaigns), category, subcategory)

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("blCampaigns.vb : ObtainPartialList()")
            sb.AppendLine("Category : " & category)
            sb.AppendLine("Subcategory :" & subcategory)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

        End Try

        Return dt

    End Function
    Private Function ObtainCampaigns_byCategory(ByRef dt As DataTable, ByRef thisNode As IPublishedContent, ByRef category As String, Optional ByVal subcategory As String = "", Optional ByVal teamName As String = "") As DataTable
        '
        Try
            Select Case thisNode.DocumentTypeAlias
                Case docTypes.Team
                    For Each childNode As IPublishedContent In thisNode.Children
                        ObtainCampaigns_byCategory(dt, childNode, category, subcategory, thisNode.Name)
                    Next
                Case docTypes.Campaign
                    linqCampaigns = New linqCampaigns
                    linqCampaigns.addCampaignToTable(dt, thisNode, teamName, category, subcategory)
                Case Else
                    For Each childNode As IPublishedContent In thisNode.Children
                        ObtainCampaigns_byCategory(dt, childNode, category, subcategory)
                    Next
            End Select


        Catch ex As Exception

            Dim sb As New StringBuilder()
            sb.AppendLine("blCampaigns.vb : ObtainCampaigns_byCategory()")
            sb.AppendLine("dt: " & JsonConvert.SerializeObject(dt))
            sb.AppendLine("thisNode :" & thisNode.Id)
            sb.AppendLine("category : " & category)
            sb.AppendLine("subcategory :" & subcategory)
            sb.AppendLine("TeamName : " & teamName)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())


        End Try


        Return dt


    End Function
    Public Function getProperIcon(ByVal category As String) As String

        Try
            'Determine which icon to display
            Select Case category
                Case Categories.SelfHelp
                    Return getMediaURL(_uHelper.Get_IPublishedContentByID(siteNodes.Home).GetPropertyValue(Of String)(nodeProperties.category01Icon))
                Case Categories.Charity
                    Return getMediaURL(_uHelper.Get_IPublishedContentByID(siteNodes.Home).GetPropertyValue(Of String)(nodeProperties.category02Icon))
                Case Categories.Arts
                    Return getMediaURL(_uHelper.Get_IPublishedContentByID(siteNodes.Home).GetPropertyValue(Of String)(nodeProperties.category04Icon))
                Case Categories.Science
                    Return getMediaURL(_uHelper.Get_IPublishedContentByID(siteNodes.Home).GetPropertyValue(Of String)(nodeProperties.category05Icon))
                Case Categories.Technology
                    Return getMediaURL(_uHelper.Get_IPublishedContentByID(siteNodes.Home).GetPropertyValue(Of String)(nodeProperties.category06Icon))
                Case Categories.Software
                    Return getMediaURL(_uHelper.Get_IPublishedContentByID(siteNodes.Home).GetPropertyValue(Of String)(nodeProperties.category07Icon))
                Case Categories.Community
                    Return getMediaURL(_uHelper.Get_IPublishedContentByID(siteNodes.Home).GetPropertyValue(Of String)(nodeProperties.category08Icon))
                Case Categories.Business
                    Return getMediaURL(_uHelper.Get_IPublishedContentByID(siteNodes.Home).GetPropertyValue(Of String)(nodeProperties.category09Icon))

                Case Else
                    Return String.Empty
            End Select

        Catch ex As Exception

            Dim sb As New StringBuilder()
            sb.AppendLine("blCampaigns.vb : getProperIcon()")
            sb.AppendLine("category: " & category)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return String.Empty

        End Try


    End Function
    Public Function SortBy(ByVal lstCampaignSummary As List(Of CampaignSummary), ByVal Sort As String) As List(Of CampaignSummary)

        Dim lstReturnSummary As List(Of CampaignSummary) = Nothing
        Try
            If Sort = queryParameters.NAME Then

                lstReturnSummary = lstCampaignSummary.OrderBy(Function(x) x.title).ToList()

            ElseIf Sort = queryParameters._DATE Then

                lstReturnSummary = lstCampaignSummary.OrderByDescending(Function(x) x._Date).ToList()

            ElseIf Sort = queryParameters.TEAMNAME Then

                lstReturnSummary = lstCampaignSummary.OrderBy(Function(x) x.team).ToList()

            ElseIf Sort = queryParameters.COMPLETED Then

                lstReturnSummary = lstCampaignSummary.OrderBy(Function(x) x.Completed).ToList()

            ElseIf Sort = queryParameters.FAILED Then

                lstReturnSummary = lstCampaignSummary.OrderBy(Function(x) x.Failed).ToList()

            End If
        Catch ex As Exception

            Dim sb As New StringBuilder()
            sb.AppendLine("blCampaigns.vb : SortBy()")
            sb.AppendLine("lstCampaignSummary: " & JsonConvert.SerializeObject(lstCampaignSummary))
            sb.AppendLine("Sort : " & Sort)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

        End Try



        If IsNothing(lstReturnSummary) Then

            Return lstCampaignSummary

        Else

            Return lstReturnSummary

        End If


    End Function
    Public Function adjustDonationWithFee(ByVal campaignId As Integer, ByVal donationAmount As Integer) As Integer
        Try
            'Instantiate variables
            Dim blPhases As New blPhases
            Dim blPledges As New blPledges
            Dim campaignGoal As Single
            Dim campaignPledges As Single
            Dim currentPhaseNo As Integer
            Dim isFreeCampaign As Boolean = False
            Dim ipCampaign As IPublishedContent = _uHelper.Get_IPublishedContentByID(campaignId)
            Dim addMwooFee As Boolean = False
            Dim percent_stripeFee As Single = CSng(ConfigurationManager.AppSettings(Miscellaneous.stripeCardFeePercent))
            Dim stripeAdditionalFee As Integer = CInt(ConfigurationManager.AppSettings(Miscellaneous.stripeAdditionalFee))
            Dim stripeFee As Integer = CInt((percent_stripeFee * donationAmount) + stripeAdditionalFee)

            'Is this campaign marked as free?
            If ipCampaign.GetPropertyValue(Of Boolean)(nodeProperties.noFeesApplied) = True Then isFreeCampaign = True


            'Is this a charity?
            If Not isFreeCampaign Then
                isFreeCampaign = IsCampaignACharity_byId(campaignId)
            End If


            'Is this the free phase?
            If Not isFreeCampaign Then
                'Obtain current phase number
                currentPhaseNo = blPhases.obtainActivePhaseNo_byCampaignId(campaignId)
                'Determine if a fee needs to be added or not
                If currentPhaseNo = 1 Then
                    'Obtain the campaign's goal and pledges
                    campaignGoal = blPhases.getGoal_byCampaignId(campaignId)
                    campaignPledges = blPledges.selectSum_byCampaignId(campaignId)
                    'Has the campaign reached 1/3 of the campaign's total goal?
                    If campaignPledges >= (campaignGoal / 3) Then addMwooFee = True
                Else
                    addMwooFee = True
                End If
            End If

            '
            Dim result As Integer = 0
            Dim mwooFee As Integer
            Dim percent_MWoOFee As Single = CSng(ConfigurationManager.AppSettings(Miscellaneous.MWoOFeePercent))

            'Return donation amount with or without fee
            If addMwooFee Then
                'Instantiate variables

                'Determine fee amount
                mwooFee = CInt(percent_MWoOFee * donationAmount)
                'mwooFee = Math.Round(percent_MWoOFee * donationAmount)
                'Dim stripeFee As Integer = ((percent_stripeFee * donationAmount) * 100) + stripeAdditionalFee

                'Return with fee subtracted
                result = ((donationAmount - mwooFee) - stripeFee)
            Else
                'Return without mwoo fee
                result = (donationAmount - stripeFee)
            End If


            'Dim sb As New StringBuilder()
            'sb.AppendLine("SUMMARY: blCampaigns.vb : adjustDonationWithFee()")
            'sb.AppendLine("percent_MWoOFee:" & percent_MWoOFee)
            'sb.AppendLine("percent_stripeFee:" & percent_stripeFee)
            'sb.AppendLine("stripeAdditionalFee: " & stripeAdditionalFee)
            'sb.AppendLine("==========================")
            'sb.AppendLine("donationAmount:" & donationAmount.ToString())
            'sb.AppendLine("stripeFee:" & stripeFee)
            'sb.AppendLine("addMwooFee:" & addMwooFee)
            'sb.AppendLine("mwooFee:" & mwooFee)
            'sb.AppendLine("result:" & result)
            'saveErrorMessage(getLoggedInMember, sb.ToString, sb.ToString())

            Return result

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("blCampaigns.vb : adjustDonationWithFee()")
            sb.AppendLine("campaignId:" & campaignId.ToString())
            sb.AppendLine("donationAmount:" & donationAmount.ToString())
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

            'Due to error return full donation to submission.
            Return donationAmount
        End Try
    End Function

    Public Function getMWoOFees(ByVal campaignId As Integer, ByVal donationAmount As Integer) As Integer
        Try
            'Instantiate variables
            Dim blPhases As New blPhases
            Dim blPledges As New blPledges
            Dim campaignGoal As Single
            Dim campaignPledges As Single
            Dim currentPhaseNo As Integer
            Dim isFreeCampaign As Boolean = False
            Dim ipCampaign As IPublishedContent = _uHelper.Get_IPublishedContentByID(campaignId)
            Dim addMwooFee As Boolean = False

            'Is this campaign marked as free?
            If ipCampaign.GetPropertyValue(Of Boolean)(nodeProperties.noFeesApplied) = True Then isFreeCampaign = True

            'Is this a charity?
            If Not isFreeCampaign Then
                isFreeCampaign = IsCampaignACharity_byId(campaignId)
            End If

            'Is this a free campaign?
            If isFreeCampaign Then
                Return 0
            Else
                'Is this the free phase?
                If Not isFreeCampaign Then
                    'Obtain current phase number
                    currentPhaseNo = blPhases.obtainActivePhaseNo_byCampaignId(campaignId)
                    'Determine if a fee needs to be added or not
                    If currentPhaseNo = 1 Then
                        'Obtain the campaign's goal and pledges
                        campaignGoal = blPhases.getGoal_byCampaignId(campaignId)
                        campaignPledges = blPledges.selectSum_byCampaignId(campaignId)
                        'Has the campaign reached 1/3 of the campaign's total goal?
                        If campaignPledges >= (campaignGoal / 3) Then addMwooFee = True
                    Else
                        addMwooFee = True
                    End If
                End If

                'Return donation amount with or without fee
                If addMwooFee Then
                    'Instantiate variables
                    Dim percent_MWoOFee As Single = CSng(ConfigurationManager.AppSettings(Miscellaneous.MWoOFeePercent))

                    'Determine fee amount
                    Return CInt(percent_MWoOFee * donationAmount)
                Else
                    'Return without mwoo fee
                    Return 0
                End If
            End If

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("blCampaigns.vb : getMWoOFees()")
            sb.AppendLine("campaignId:" & campaignId.ToString())
            sb.AppendLine("donationAmount:" & donationAmount.ToString())
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

            Return 0
        End Try
    End Function
    Public Function getStripeFee(ByVal donationAmount As Integer) As Integer
        Try
            'Instantiate variables
            Dim percent_stripeFee As Single = CSng(ConfigurationManager.AppSettings(Miscellaneous.stripeCardFeePercent))
            Dim stripeAdditionalFee As Integer = CInt(ConfigurationManager.AppSettings(Miscellaneous.stripeAdditionalFee))

            Return CInt((percent_stripeFee * donationAmount) + stripeAdditionalFee)

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("blCampaigns.vb : getStripeFee()")
            sb.AppendLine("donationAmount:" & donationAmount.ToString())
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

            'Due to error return full donation to submission.
            Return 0
        End Try
    End Function

    Public Function isUserAMemberOfCampaign(ByVal campaignId As Integer, ByVal memberId As Integer) As Boolean
        Try
            'Instantiate variables
            Dim isMember As Boolean = False

            'Obtain id list of team administrators
            Dim ipCampaign As IPublishedContent = _uHelper.Get_IPublishedContentByID(campaignId)
            Dim lstCampaignMemberIDs As List(Of String) = (ipCampaign.Parent.GetPropertyValue(Of String)(nodeProperties.teamAdministrators).Split(",")).ToList

            'Obtain campaign's member folder
            Dim campaignMemberNode As IPublishedContent = getCampaignMemberFolder(campaignId)

            'Loop thru each member node
            For Each childNode As IPublishedContent In campaignMemberNode.Children
                If childNode.HasProperty(nodeProperties.campaignMember) AndAlso childNode.HasValue(nodeProperties.campaignMember) Then
                    Dim id = childNode.GetPropertyValue(Of String)(nodeProperties.campaignMember)
                    lstCampaignMemberIDs.Add(childNode.GetPropertyValue(Of Integer)(nodeProperties.campaignMember))
                End If
            Next

            'Determine if user is a member of campaign
            If lstCampaignMemberIDs.Contains(memberId) Then
                isMember = True
            End If

            Return isMember
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("blCampaigns.vb : isUserAMemberOfCampaign()")
            sb.AppendLine("campaignId:" & campaignId)
            sb.AppendLine("memberId:" & memberId)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

            Return False
        End Try
    End Function
    Private Function getCampaignMemberFolder(ByVal campaignId As Integer) As IPublishedContent
        'Instantiate variables

        Try
            Dim ipCampaign As IPublishedContent = _uHelper.Get_IPublishedContentByID(campaignId)
            'Loop thru child nodes, obtain campaignMember folder.
            For Each childNode In ipCampaign.Children
                If childNode.DocumentTypeAlias = docTypes.campaignMembers Then
                    Return childNode
                End If
            Next

        Catch ex As Exception

            Dim sb As New StringBuilder()
            sb.AppendLine("blCampaigns.vb :getCampaignMemberFolder()")
            sb.AppendLine("campaignId: " & campaignId)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

        End Try

        Return Nothing

    End Function
    Private Function obtainCampaignManagers_byCampaignId(ByVal campaignId As Integer) As List(Of Integer)
        'Scope variables
        Dim lstManagers As New List(Of Integer)

        Try
            'Instantiate variables
            Dim membersNode As IPublishedContent = getCampaignMemberFolder(campaignId)

            '
            If Not IsNothing(membersNode) Then
                For Each member As IPublishedContent In membersNode.Children
                    If member.HasProperty(nodeProperties.campaignManager) AndAlso member.GetPropertyValue(Of Boolean)(nodeProperties.campaignManager) = True Then
                        lstManagers.Add(member.GetPropertyValue(Of Integer)(nodeProperties.campaignMember))
                    End If
                Next
            End If

            Return lstManagers

        Catch ex As Exception

            Dim sb As New StringBuilder()
            sb.AppendLine("blCampaigns.vb : obtainCampaignManagers_byCampaignId()")
            sb.AppendLine("campaignId: " & campaignId)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

            Return lstManagers

        End Try

    End Function
    Public Function IsCampaignACharity_byId(ByVal id As Integer) As Boolean
        Try
            Return linqCampaigns.IsCampaignACharity_byId(id)
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("blCampaigns.vb : IsCampaignACharity_byId()")
            sb.AppendLine("id:" & id)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return False
        End Try
    End Function
#End Region

#Region "Get top N of"
    Private Function GetTopNOf_Artistic(ByVal subcategory As String, ByRef dt As CampaignList.dtCampaignsDataTable) As List(Of Integer) ' CampaignList.dtCampaignListDataTable
        'Instantiate variables
        Dim dtNodeIds = New List(Of Integer) 'CampaignList.dtCampaignListDataTable

        Try

            'Get top n of each subcategory
            Dim counter As UInt16 = Miscellaneous.campaignsToShow
            For Each dr As DataRowView In dt.DefaultView
                'Exit if max nodes has been reached.
                If counter = 0 Then
                    dtNodeIds.Add(-1) 'Add a -1 to indicate that there are more items. 
                    Exit For
                End If

                'Split campaign's subcategory list into an array
                Dim subcategories As String() = dr(SubcategoryConverter.Artistic).ToString.ToLower.Split(",")

                '
                Select Case subcategory
                    Case Subcategory_Artistic.Art.ToLower
                        If subcategories.Contains(Subcategory_Artistic.Art.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Artistic.Comics.ToLower
                        If subcategories.Contains(Subcategory_Artistic.Comics.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Artistic.Crafts.ToLower
                        If subcategories.Contains(Subcategory_Artistic.Crafts.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Artistic.Creative.ToLower
                        If subcategories.Contains(Subcategory_Artistic.Creative.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Artistic.Dance.ToLower
                        If subcategories.Contains(Subcategory_Artistic.Dance.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Artistic.Fashion.ToLower
                        If subcategories.Contains(Subcategory_Artistic.Fashion.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Artistic.Food.ToLower
                        If subcategories.Contains(Subcategory_Artistic.Food.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Artistic.Illustration.ToLower
                        If subcategories.Contains(Subcategory_Artistic.Illustration.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Artistic.Music.ToLower
                        If subcategories.Contains(Subcategory_Artistic.Music.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Artistic.Photography.ToLower
                        If subcategories.Contains(Subcategory_Artistic.Photography.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Artistic.Publishing.ToLower
                        If subcategories.Contains(Subcategory_Artistic.Publishing.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Artistic.Theater.ToLower
                        If subcategories.Contains(Subcategory_Artistic.Theater.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Artistic.VideoFilmAndWeb.ToLower
                        If subcategories.Contains(Subcategory_Artistic.VideoFilmAndWeb.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Artistic.Writing.ToLower
                        If subcategories.Contains(Subcategory_Artistic.Writing.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Artistic.Other.ToLower
                        If subcategories.Contains(Subcategory_Artistic.Other.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Else
                End Select

                ''Exit when max nodes has been reached.
                'If counter = 0 Then Exit For
            Next
        Catch ex As Exception

            Dim sb As New StringBuilder()
            sb.AppendLine("blCampaigns.vb : GetTopNOf_Artistic()")
            sb.AppendLine("subcategory : " & subcategory)
            sb.AppendLine("dt :" & JsonConvert.SerializeObject(dt))
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

        End Try

        Return dtNodeIds

    End Function
    Private Function GetTopNOf_Business(ByVal subcategory As String, ByRef dt As CampaignList.dtCampaignsDataTable) As List(Of Integer) ' CampaignList.dtCampaignListDataTable
        'Instantiate variables
        Dim dtNodeIds = New List(Of Integer) 'CampaignList.dtCampaignListDataTable
        Try
            'Get top n of each subcategory
            Dim counter As UInt16 = Miscellaneous.campaignsToShow
            For Each dr As DataRowView In dt.DefaultView
                'Exit if max nodes has been reached.
                If counter = 0 Then
                    dtNodeIds.Add(-1) 'Add a -1 to indicate that there are more items. 
                    Exit For
                End If

                'Split campaign's subcategory list into an array
                Dim subcategories As String() = dr(SubcategoryConverter.Business).ToString.ToLower.Split(",")

                Select Case subcategory
                    Case Subcategory_Business.InternetBased.ToLower
                        If subcategories.Contains(Subcategory_Business.InternetBased.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Business.Manufacturing.ToLower
                        If subcategories.Contains(Subcategory_Business.Manufacturing.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Business.SmallBusiness.ToLower
                        If subcategories.Contains(Subcategory_Business.SmallBusiness.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Business.SmallTown.ToLower
                        If subcategories.Contains(Subcategory_Business.SmallTown.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Business.Startups.ToLower
                        If subcategories.Contains(Subcategory_Business.Startups.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Business.Other.ToLower
                        If subcategories.Contains(Subcategory_Business.Other.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Else
                End Select

                ''Exit when max nodes has been reached.
                'If counter = 0 Then Exit For
            Next

        Catch ex As Exception

            Dim sb As New StringBuilder()
            sb.AppendLine("blCampaigns.vb : GetTopNOf_Bussiness()")
            sb.AppendLine("subcategory : " & subcategory)
            sb.AppendLine("dt :" & JsonConvert.SerializeObject(dt))
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

        End Try
        Return dtNodeIds

    End Function
    Private Function GetTopNOf_Charity(ByVal subcategory As String, ByRef dt As CampaignList.dtCampaignsDataTable) As List(Of Integer) ' CampaignList.dtCampaignListDataTable
        'Instantiate variables
        Dim dtNodeIds = New List(Of Integer) 'CampaignList.dtCampaignListDataTable
        Try
            'Get top n of each subcategory
            Dim counter As UInt16 = Miscellaneous.campaignsToShow
            For Each dr As DataRowView In dt.DefaultView
                'Exit if max nodes has been reached.
                If counter = 0 Then
                    dtNodeIds.Add(-1) 'Add a -1 to indicate that there are more items. 
                    Exit For
                End If

                'Split campaign's subcategory list into an array
                Dim subcategories As String() = dr(SubcategoryConverter.Charity).ToString.ToLower.Split(",")

                Select Case subcategory
                    Case Subcategory_Charity.Animals.ToLower
                        If subcategories.Contains(Subcategory_Charity.Animals.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Charity.Emergencies.ToLower
                        If subcategories.Contains(Subcategory_Charity.Emergencies.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Charity.Family.ToLower
                        If subcategories.Contains(Subcategory_Charity.Family.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Charity.FireVictim.ToLower
                        If subcategories.Contains(Subcategory_Charity.FireVictim.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Charity.MedicalHealth.ToLower
                        If subcategories.Contains(Subcategory_Charity.MedicalHealth.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Charity.Memorials.ToLower
                        If subcategories.Contains(Subcategory_Charity.Memorials.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Charity.Moving.ToLower
                        If subcategories.Contains(Subcategory_Charity.Moving.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Charity.VolunteerCenters.ToLower
                        If subcategories.Contains(Subcategory_Charity.VolunteerCenters.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Charity.Other.ToLower
                        If subcategories.Contains(Subcategory_Charity.Other.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Else
                End Select

                ''Exit when max nodes has been reached.
                'If counter = 0 Then Exit For
            Next
        Catch ex As Exception

            Dim sb As New StringBuilder()
            sb.AppendLine("blCampaigns.vb : GetTopNOf_Charity()")
            sb.AppendLine("subcategory : " & subcategory)
            sb.AppendLine("dt :" & JsonConvert.SerializeObject(dt))
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

        End Try

        Return dtNodeIds
    End Function
    Private Function GetTopNOf_Community(ByVal subcategory As String, ByRef dt As CampaignList.dtCampaignsDataTable) As List(Of Integer) ' CampaignList.dtCampaignListDataTable
        'Instantiate variables
        Dim dtNodeIds = New List(Of Integer) ' CampaignList.dtCampaignListDataTable
        Try

            'Get top n of each subcategory
            Dim counter As UInt16 = Miscellaneous.campaignsToShow
            For Each dr As DataRowView In dt.DefaultView
                'Exit if max nodes has been reached.
                If counter = 0 Then
                    dtNodeIds.Add(-1) 'Add a -1 to indicate that there are more items. 
                    Exit For
                End If

                'Split campaign's subcategory list into an array
                Dim subcategories As String() = dr(SubcategoryConverter.Community).ToString.ToLower.Split(",")

                Select Case subcategory
                    Case Subcategory_Community.CommunityDevelopment.ToLower
                        If subcategories.Contains(Subcategory_Community.CommunityDevelopment.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Community.Environmental.ToLower
                        If subcategories.Contains(Subcategory_Community.Environmental.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Community.Events.ToLower
                        If subcategories.Contains(Subcategory_Community.Events.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Community.JournalismAndNews.ToLower
                        If subcategories.Contains(Subcategory_Community.JournalismAndNews.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Community.PoliticalCauses.ToLower
                        If subcategories.Contains(Subcategory_Community.PoliticalCauses.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Community.Religion.ToLower
                        If subcategories.Contains(Subcategory_Community.Religion.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Community.Travel.ToLower
                        If subcategories.Contains(Subcategory_Community.Travel.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Community.YouthDevelopment.ToLower
                        If subcategories.Contains(Subcategory_Community.YouthDevelopment.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Community.Other.ToLower
                        If subcategories.Contains(Subcategory_Community.Other.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Else
                End Select

                ''Exit when max nodes has been reached.
                'If counter = 0 Then Exit For
            Next
        Catch ex As Exception

            Dim sb As New StringBuilder()
            sb.AppendLine("blCampaigns.vb : GetTopNOf_Community()")
            sb.AppendLine("subcategory : " & subcategory)
            sb.AppendLine("dt :" & JsonConvert.SerializeObject(dt))
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

        End Try

        Return dtNodeIds

    End Function
    Private Function GetTopNOf_Science(ByVal subcategory As String, ByRef dt As CampaignList.dtCampaignsDataTable) As List(Of Integer) ' CampaignList.dtCampaignListDataTable
        'Instantiate variables
        Dim dtNodeIds = New List(Of Integer) ' CampaignList.dtCampaignListDataTable
        Try
            'Get top n of each subcategory
            Dim counter As UInt16 = Miscellaneous.campaignsToShow
            For Each dr As DataRowView In dt.DefaultView
                'Exit if max nodes has been reached.
                If counter = 0 Then
                    dtNodeIds.Add(-1) 'Add a -1 to indicate that there are more items. 
                    Exit For
                End If

                'Split campaign's subcategory list into an array
                Dim subcategories As String() = dr(SubcategoryConverter.Science).ToString.ToLower.Split(",")

                Select Case subcategory
                    Case Subcategory_Science.Archaeology.ToLower
                        If subcategories.Contains(Subcategory_Science.Archaeology.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Science.Astronomy.ToLower
                        If subcategories.Contains(Subcategory_Science.Astronomy.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Science.Astrophysics.ToLower
                        If subcategories.Contains(Subcategory_Science.Astrophysics.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Science.Biology.ToLower
                        If subcategories.Contains(Subcategory_Science.Biology.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Science.DataAnalysis.ToLower
                        If subcategories.Contains(Subcategory_Science.DataAnalysis.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Science.EarthScience.ToLower
                        If subcategories.Contains(Subcategory_Science.EarthScience.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Science.Geology.ToLower
                        If subcategories.Contains(Subcategory_Science.Geology.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Science.Mathematics.ToLower
                        If subcategories.Contains(Subcategory_Science.Mathematics.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Science.MedicalResearch.ToLower
                        If subcategories.Contains(Subcategory_Science.MedicalResearch.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Science.Physics.ToLower
                        If subcategories.Contains(Subcategory_Science.Physics.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Science.Research.ToLower
                        If subcategories.Contains(Subcategory_Science.Research.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Science.ResearchPublication.ToLower
                        If subcategories.Contains(Subcategory_Science.ResearchPublication.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Science.SpaceExploration.ToLower
                        If subcategories.Contains(Subcategory_Science.SpaceExploration.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Science.Theory.ToLower
                        If subcategories.Contains(Subcategory_Science.Theory.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Else
                End Select

                ''Exit when max nodes has been reached.
                'If counter = 0 Then Exit For
            Next

        Catch ex As Exception

            Dim sb As New StringBuilder()
            sb.AppendLine("blCampaigns.vb : GetTopNOf_Science()")
            sb.AppendLine("subcategory : " & subcategory)
            sb.AppendLine("dt :" & JsonConvert.SerializeObject(dt))
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

        End Try

        Return dtNodeIds
    End Function
    Private Function GetTopNOf_SelfHelp(ByVal subcategory As String, ByRef dt As CampaignList.dtCampaignsDataTable) As List(Of Integer) ' CampaignList.dtCampaignListDataTable
        'Instantiate variables
        Dim dtNodeIds = New List(Of Integer) 'CampaignList.dtCampaignListDataTable
        Try
            'Get top n of each subcategory
            Dim counter As UInt16 = Miscellaneous.campaignsToShow
            For Each dr As DataRowView In dt.DefaultView
                'Exit if max nodes has been reached.
                If counter = 0 Then
                    dtNodeIds.Add(-1) 'Add a -1 to indicate that there are more items. 
                    Exit For
                End If

                'Split campaign's subcategory list into an array
                Dim subcategories As String() = dr(SubcategoryConverter.SelfHelp).ToString.ToLower.Split(",")

                Select Case subcategory

                    Case Subcategory_Science.MedicalResearch.ToLower
                        If subcategories.Contains(Subcategory_Science.MedicalResearch.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Science.Physics.ToLower
                        If subcategories.Contains(Subcategory_Science.Physics.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Science.Research.ToLower
                        If subcategories.Contains(Subcategory_Science.Research.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Science.ResearchPublication.ToLower
                        If subcategories.Contains(Subcategory_Science.ResearchPublication.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Science.SpaceExploration.ToLower
                        If subcategories.Contains(Subcategory_Science.SpaceExploration.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Science.Theory.ToLower
                        If subcategories.Contains(Subcategory_Science.Theory.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Else
                End Select

                ''Exit when max nodes has been reached.
                'If counter = 0 Then Exit For
            Next

        Catch ex As Exception

            Dim sb As New StringBuilder()
            sb.AppendLine("blCampaigns.vb : GetTopNOf_SelfHelp()")
            sb.AppendLine("subcategory : " & subcategory)
            sb.AppendLine("dt :" & JsonConvert.SerializeObject(dt))
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

        End Try

        Return dtNodeIds

    End Function
    Private Function GetTopNOf_Software(ByVal subcategory As String, ByRef dt As CampaignList.dtCampaignsDataTable) As List(Of Integer) ' CampaignList.dtCampaignListDataTable
        'Instantiate variables
        Dim dtNodeIds = New List(Of Integer) 'CampaignList.dtCampaignListDataTable
        Try
            'Get top n of each subcategory
            Dim counter As UInt16 = Miscellaneous.campaignsToShow
            For Each dr As DataRowView In dt.DefaultView
                'Exit if max nodes has been reached.
                If counter = 0 Then
                    dtNodeIds.Add(-1) 'Add a -1 to indicate that there are more items. 
                    Exit For
                End If

                'Split campaign's subcategory list into an array
                Dim subcategories As String() = dr(SubcategoryConverter.Software).ToString.ToLower.Split(",")

                Select Case subcategory

                    Case Subcategory_Software.Browser.ToLower
                        If subcategories.Contains(Subcategory_Software.Browser.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Software.CellPhoneApps.ToLower
                        If subcategories.Contains(Subcategory_Software.CellPhoneApps.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Software.ComputerApps.ToLower
                        If subcategories.Contains(Subcategory_Software.ComputerApps.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Software.LinuxSoftware.ToLower
                        If subcategories.Contains(Subcategory_Software.LinuxSoftware.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Software.Programs.ToLower
                        If subcategories.Contains(Subcategory_Software.Programs.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Software.VideoGames.ToLower
                        If subcategories.Contains(Subcategory_Software.VideoGames.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Else
                End Select

                ''Exit when max nodes has been reached.
                'If counter = 0 Then Exit For
            Next
        Catch ex As Exception

            Dim sb As New StringBuilder()
            sb.AppendLine("blCampaigns.vb : GetTopNOf_Software()")
            sb.AppendLine("subcategory : " & subcategory)
            sb.AppendLine("dt :" & JsonConvert.SerializeObject(dt))
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

        End Try

        Return dtNodeIds
    End Function
    Private Function GetTopNOf_Technology(ByVal subcategory As String, ByRef dt As CampaignList.dtCampaignsDataTable) As List(Of Integer) ' CampaignList.dtCampaignListDataTable
        'Instantiate variables
        Dim dtNodeIds = New List(Of Integer) 'CampaignList.dtCampaignListDataTable
        Try
            'Get top n of each subcategory
            Dim counter As UInt16 = Miscellaneous.campaignsToShow
            For Each dr As DataRowView In dt.DefaultView
                'Exit if max nodes has been reached.
                If counter = 0 Then
                    dtNodeIds.Add(-1) 'Add a -1 to indicate that there are more items. 
                    Exit For
                End If

                'Split campaign's subcategory list into an array
                Dim subcategories As String() = dr(SubcategoryConverter.Technology).ToString.ToLower.Split(",")

                Select Case subcategory

                    Case Subcategory_Technology.Printer3D.ToLower
                        If subcategories.Contains(Subcategory_Technology.Printer3D.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Technology.Aerospace.ToLower
                        If subcategories.Contains(Subcategory_Technology.Aerospace.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Technology.Agriculture.ToLower
                        If subcategories.Contains(Subcategory_Technology.Agriculture.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Technology.AstronomyHardware.ToLower
                        If subcategories.Contains(Subcategory_Technology.AstronomyHardware.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Technology.BioAndMedicine.ToLower
                        If subcategories.Contains(Subcategory_Technology.BioAndMedicine.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Technology.Design.ToLower
                        If subcategories.Contains(Subcategory_Technology.Design.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Technology.EnergyProduction.ToLower
                        If subcategories.Contains(Subcategory_Technology.EnergyProduction.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Technology.Engineering.ToLower
                        If subcategories.Contains(Subcategory_Technology.Engineering.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Technology.Gadgets.ToLower
                        If subcategories.Contains(Subcategory_Technology.Gadgets.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Technology.GreenTech.ToLower
                        If subcategories.Contains(Subcategory_Technology.GreenTech.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Technology.Materials.ToLower
                        If subcategories.Contains(Subcategory_Technology.Materials.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Technology.Phone.ToLower
                        If subcategories.Contains(Subcategory_Technology.Phone.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Technology.Prototype.ToLower
                        If subcategories.Contains(Subcategory_Technology.Prototype.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Technology.Robotics.ToLower
                        If subcategories.Contains(Subcategory_Technology.Robotics.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Technology.SpaceExploration.ToLower
                        If subcategories.Contains(Subcategory_Technology.SpaceExploration.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Subcategory_Technology.Other.ToLower
                        If subcategories.Contains(Subcategory_Technology.Other.ToLower) Then
                            dtNodeIds.Add(dr(Miscellaneous.nodeId)) 'Add nodeId to list
                            counter -= 1
                        End If

                    Case Else
                End Select

                ''Exit when max nodes has been reached.
                'If counter = 0 Then Exit For
            Next
        Catch ex As Exception

            Dim sb As New StringBuilder()
            sb.AppendLine("blCampaigns.vb : GetTopNOf_Technology()")
            sb.AppendLine("subcategory : " & subcategory)
            sb.AppendLine("dt :" & JsonConvert.SerializeObject(dt))
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

        End Try

        Return dtNodeIds

    End Function
#End Region

#Region "Validations"
    Public Function Validate(ByVal _currentReturn As BusinessReturn, ByVal _campaignName As String, Optional ByVal _altEmail As String = "") As BusinessReturn

        Try
            If String.IsNullOrWhiteSpace(_campaignName) Then
                _currentReturn.ValidationMessages.Add(New ValidationContainer("*Field is required"))
                '_currentReturn.ExceptionMessage = "Field is required"
            Else
                If doesCampaignExist_byName(_campaignName) Then
                    _currentReturn.ValidationMessages.Add(New ValidationContainer("*Campaign already exists"))
                    '_currentReturn.ExceptionMessage = "Campaign already exists"
                End If
            End If

            If Not String.IsNullOrWhiteSpace(_altEmail) Then
                'Ensure value is an email
                If Not ValidEmail(_altEmail) Then
                    _currentReturn.ValidationMessages.Add(New ValidationContainer("*Invalid _email"))
                    '_currentReturn.ExceptionMessage = "Invalid _email"
                End If
            End If

            Return _currentReturn

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("blCampaigns.vb : Validate()")
            sb.AppendLine("_currentReturn: " & _currentReturn.ExceptionMessage)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return _currentReturn
        End Try


    End Function
    Public Function ValidateUrl(ByVal _url As String) As BusinessReturn
        'Instantiate variables
        Dim BusinessReturn As BusinessReturn = New BusinessReturn
        Try
            'Check url
            If String.IsNullOrWhiteSpace(_url) Then
                'All is ok, return empty
                Return BusinessReturn
            Else
                'if not valid url, send error.
                If Not ValidUrl(_url) Then
                    BusinessReturn.ExceptionMessage = "Invalid url"
                End If
            End If

            Return BusinessReturn

        Catch ex As Exception

            Dim sb As New StringBuilder()
            sb.AppendLine("blCampaigns.vb : ValidateUrl()")
            sb.AppendLine("_url: " & _url)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return BusinessReturn

        End Try

    End Function
    Public Function ValidateEmail(ByVal _email As String) As BusinessReturn
        'Instantiate variables
        Dim BusinessReturn As BusinessReturn = New BusinessReturn

        Try
            If String.IsNullOrWhiteSpace(_email) Then
                'All is ok, return empty
                Return BusinessReturn
            Else
                'if not valid, send error.
                If Not ValidEmail(_email) Then
                    BusinessReturn.ExceptionMessage = "Invalid _email"
                End If
            End If
            Return BusinessReturn

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("blCampaigns.vb : ValidateEmail()")
            sb.AppendLine("_email: " & _email)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return BusinessReturn

        End Try

    End Function
#End Region

End Class