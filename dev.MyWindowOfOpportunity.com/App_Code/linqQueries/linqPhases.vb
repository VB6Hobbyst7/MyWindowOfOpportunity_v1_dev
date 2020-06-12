Imports System.Data
Imports umbraco
Imports Common
Imports umbraco.Core
Imports umbraco.Core.Models
Imports umbraco.Core.Services
Imports umbraco.Core.Publishing
Imports umbraco.Web
Imports umbraco.Core.Constants
Imports Newtonsoft.Json

Public Class LinqPhases

#Region "Properties"
    Private linq2Db As linq2SqlDataContext = New linq2SqlDataContext(ConfigurationManager.ConnectionStrings("umbracoDbDSN").ToString)
    Private _uHelper As Uhelper = New Uhelper()
    Private phaseLength As Int16 = 30 '1
#End Region

#Region "Selects"
    Public Function getGoal_byCampaignId(ByVal _campaignId As Integer) As Decimal?  'String
        Try
            'Instantiate variables
            Dim goal As Decimal = 0
            Dim phaseFolderId As Integer = 0
            Dim campaignNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(_campaignId)
            Dim phasesNode As IPublishedContent

            'Obtain the phase folder id
            phaseFolderId = obtainPhaseFolderId_byCampaign(campaignNode)

            If phaseFolderId > 0 Then
                'If Not IsNothing(phaseFolderId) Then
                'Create the phase folder IPublishedContent
                phasesNode = _uHelper.Get_IPublishedContentByID(phaseFolderId)
                'Loop thru each child phase and obtain it's goal
                For Each phaseNode As IPublishedContent In phasesNode.Children
                    If phaseNode.GetPropertyValue(Of Boolean)(nodeProperties.published) Then
                        If phaseNode.HasProperty(nodeProperties.goal) Then
                            goal += phaseNode.GetPropertyValue(Of Decimal)(nodeProperties.goal)
                        End If
                    End If
                Next
            End If

            'Return the total goal
            Return goal
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\linqQueries\linqPhases.vb : getGoal_byCampaignId()")
            sb.AppendLine("_campaignId:" & _campaignId)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return 0D
        End Try
    End Function
    Public Function obtainActivePhaseId_byCampaignId(ByVal _campaignId As Integer) As Integer?
        Try
            'Instantiate variables
            Dim phaseFolderId As Integer? = obtainPhaseFolderId_byCampaign(_uHelper.Get_IPublishedContentByID(_campaignId))

            If Not IsNothing(phaseFolderId) Then
                'Instantiate variable
                Dim phasesFolder As IPublishedContent = _uHelper.Get_IPublishedContentByID(phaseFolderId)

                'Loop thru and locate active phase
                For Each phase As IPublishedContent In phasesFolder.Children
                    If phase.HasProperty(nodeProperties.phaseActive) AndAlso phase.GetPropertyValue(Of Boolean)(nodeProperties.phaseActive) = True Then
                        Return phase.Id
                    End If
                Next

            End If

            'No active phases. 
            Return Nothing
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqPhases.vb : obtainActivePhaseId_byCampaignId()")
            sb.AppendLine("_campaignId:" & _campaignId)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return Nothing
        End Try
    End Function
    Public Function obtainActivePhaseNo_byCampaignId(ByVal campaignId As Integer) As Integer?
        Try
            'Instantiate variables

            Dim activePhaseId As Integer?
            Dim ipCampaign As IPublishedContent

            'Obtain active phase id
            activePhaseId = obtainActivePhaseId_byCampaignId(campaignId)
            If IsNothing(activePhaseId) Then
                Return 0
            Else
                'Initialize campaign IPublishedContent
                ipCampaign = _uHelper.Get_IPublishedContentByID(activePhaseId)
                'Obtain phase number
                If ipCampaign.HasProperty(nodeProperties.phaseNumber) AndAlso ipCampaign.HasValue(nodeProperties.phaseNumber) Then
                    Return ipCampaign.GetPropertyValue(Of Integer)(nodeProperties.phaseNumber)
                Else
                    Return 0
                End If
            End If

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqPhases.vb : obtainActivePhaseNo_byCampaignId()")
            sb.AppendLine("campaignId:" & campaignId)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return 0
        End Try
    End Function
    Public Function pendingPhasesExist_byCampaignId(ByVal _campaignId As Integer) As Boolean
        'Scope variables
        Dim pendingPhaseExists As Boolean = False

        Try
            'Instantiate variables
            Dim phaseFolderId As Integer? = obtainPhaseFolderId_byCampaign(_uHelper.Get_IPublishedContentByID(_campaignId))

            If Not IsNothing(phaseFolderId) Then
                'Instantiate variable
                Dim phasesFolder As IPublishedContent = _uHelper.Get_IPublishedContentByID(phaseFolderId)

                'Loop thru and locate active phase
                For Each phase As IPublishedContent In phasesFolder.Children
                    If phase.GetPropertyValue(Of Boolean)(nodeProperties.published) Then
                        If phase.GetPropertyValue(Of Boolean)(nodeProperties.phaseActive) Then
                            Exit For
                        Else
                            If phase.GetPropertyValue(Of Boolean)(nodeProperties.phaseComplete) Then
                                Continue For
                            Else
                                pendingPhaseExists = True
                                Exit For
                            End If
                        End If
                    Else
                        Exit For
                    End If
                Next
            End If

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\linqQueries\linqPhases.vb : obtainActivePhaseId_byCampaignId()")
            sb.AppendLine("_campaignId:" & _campaignId)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try

        Return pendingPhaseExists
    End Function
    Public Function obtainPhaseStatus_byId(ByVal _phaseId As Integer, Optional ByVal _getPledges As Boolean = False) As Phase
        Try
            'Instantiate variables
            Dim phase As New Phase
            Dim phaseNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(_phaseId)
            Dim blPledges As New blPledges

            'Obtain data
            phase.id = _phaseId
            phase.name = phaseNode.Name

            If phase.name.Contains("(") AndAlso phase.name.Contains(")") Then
                Dim xst As Integer = phase.name.IndexOf("(")
                Dim xend As Integer = phase.name.IndexOf(")")

                Dim xsub As String = phase.name.Substring(xst, (xend - xst) + 1)
                phase.name = phase.name.Replace(xsub, String.Empty)
            End If

            If phaseNode.HasProperty(nodeProperties.published) Then phase.published = phaseNode.GetPropertyValue(Of Boolean)(nodeProperties.published)
            If phaseNode.HasProperty(nodeProperties.phaseActive) Then phase.active = phaseNode.GetPropertyValue(Of Boolean)(nodeProperties.phaseActive)
            If phaseNode.HasProperty(nodeProperties.phaseFailed) Then phase.failed = phaseNode.GetPropertyValue(Of Boolean)(nodeProperties.phaseFailed)
            If phaseNode.HasProperty(nodeProperties.phaseComplete) Then phase.complete = phaseNode.GetPropertyValue(Of Boolean)(nodeProperties.phaseComplete)

            If phaseNode.HasProperty(nodeProperties.activationDate) Then phase.activationDate = phaseNode.GetPropertyValue(Of Date)(nodeProperties.activationDate)
            If phaseNode.HasProperty(nodeProperties.failedDate) Then phase.failedDate = phaseNode.GetPropertyValue(Of Date)(nodeProperties.failedDate)
            If phaseNode.HasProperty(nodeProperties.completionDate) Then phase.completionDate = phaseNode.GetPropertyValue(Of Date)(nodeProperties.completionDate)

            If phaseNode.HasProperty(nodeProperties.goal) Then phase.goal = phaseNode.GetPropertyValue(Of Decimal)(nodeProperties.goal)
            If phaseNode.HasProperty(nodeProperties.phaseNumber) Then phase.phaseNumber = phaseNode.GetPropertyValue(Of Integer)(nodeProperties.phaseNumber)

            'Obtain pledges if true
            If _getPledges Then
                For Each pledge As IPublishedContent In phaseNode.Children
                    phase.lstPledges.Add(blPledges.obtainPledgeStatus_byId(pledge.Id))
                Next
            End If

            'Obtain pledge count and totals
            For Each pledge As IPublishedContent In phaseNode.Children
                'Go to next pledge if any of the following is true.
                If pledge.GetPropertyValue(Of Boolean)(nodeProperties.canceled) Then Continue For
                If pledge.GetPropertyValue(Of Boolean)(nodeProperties.reimbursed) Then Continue For
                If pledge.GetPropertyValue(Of Boolean)(nodeProperties.transactionDeclined) Then Continue For

                phase.pledgeTotal += pledge.GetPropertyValue(Of Decimal)(nodeProperties.pledgeAmount)
                phase.pledgeCount += 1
            Next

            If phaseNode.HasProperty(nodeProperties.previousPhases) AndAlso phaseNode.HasValue(nodeProperties.previousPhases) Then
                For Each previousPhase As IPublishedContent In phaseNode.GetPropertyValue(nodeProperties.previousPhases)
                    phase.lstPreviousPhases.Add(obtainPhaseStatus_byId(previousPhase.Id))
                Next
                'For Each previousPhaseId As Integer In phaseNode.GetPropertyValue(Of IPublishedContent)(nodeProperties.previousPhases).Id
                '    phase.lstPreviousPhases.Add(obtainPhaseStatus_byId(getIdFromGuid_byType(previousPhaseId, UmbracoObjectTypes.Document)))
                'Next
            End If



            Return phase
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\linqQueries\linqPhases.vb : obtainPhaseStatus_byId()")
            sb.AppendLine("_phaseId:" & _phaseId)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return Nothing
        End Try
    End Function
    Public Function obtainDaysRemaining_byPhaseId(ByVal _phaseId As Integer) As Integer
        'Instantiate scope variables
        Dim daysRemaining As UInt16 = 0

        Try
            'Proceed if an actual id exists.
            If _phaseId > 0 Then
                'Instantiate variables

                Dim ipPhase As IPublishedContent = _uHelper.Get_IPublishedContentByID(_phaseId)

                '
                Dim completionDate As DateTime = ipPhase.GetPropertyValue(Of DateTime)(nodeProperties.completionDate)

                '
                If completionDate > DateTime.Now Then
                    daysRemaining = (completionDate - Date.Today).Days
                End If

                If daysRemaining > 30 Then daysRemaining = 30
            End If

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqPhases.vb : obtainDaysRemaining_byPhaseId()")
            sb.AppendLine("_phaseId:" & _phaseId)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try

        'Return results
        Return daysRemaining
    End Function
    Public Function hasMemberSubmittedReview(ByVal _memberId As Integer, ByVal _campaignId As Integer) As BusinessReturn
        'Instantiate scope variables
        Dim bReturn As New BusinessReturn
        'Dim submitted As Boolean = False

        Try
            'Instantiate variables

            Dim ipCampaign As IPublishedContent
            Dim ipDiscoveryPhase As IPublishedContent
            Dim rating As Rating

            'Obtain discovery IPublishedContent
            ipCampaign = _uHelper.Get_IPublishedContentByID(_campaignId)
            ipDiscoveryPhase = ipCampaign.Children.Where(Function(x) (x.DocumentTypeAlias = docTypes.discovery)).FirstOrDefault()

            'Loop thru all review to find if 1 is submitted by member
            Dim submitted As Boolean = ipDiscoveryPhase.Children.Where(Function(x) (x.GetPropertyValue(Of Integer)(nodeProperties.member) = _memberId)).Any()
            'bReturn.isValid = ipDiscoveryPhase.Children.Where(Function(x) (x.GetPropertyValue(Of Integer)(nodeProperties.member) = _memberId)).Any()
            If Not submitted Then
                bReturn.ExceptionMessage = submitted.ToString
            End If

            If bReturn.isValid Then
                'Obtain review
                Dim ipReview As IPublishedContent = ipDiscoveryPhase.Children.Where(Function(x) (x.GetPropertyValue(Of Integer)(nodeProperties.member) = _memberId)).FirstOrDefault
                'Create new rating review and save data
                rating = New Rating With {
                    .nodeId = ipReview.Id,
                    .stars = ipReview.GetPropertyValue(Of Integer)(nodeProperties.stars),
                    .message = ipReview.GetPropertyValue(Of String)(nodeProperties.message)
                }
                'Add rating to business return
                bReturn.DataContainer.Add(rating)
            End If

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqPhases.vb : hasMemberSubmittedReview()")
            sb.AppendLine("_campaignId: " & _campaignId.ToString)
            sb.AppendLine("_memberId: " & _memberId.ToString)
            saveErrorMessage(getLoggedInMember, ex.Message & vbNewLine & ex.StackTrace & vbNewLine & ex.Source, sb.ToString())
        End Try

        'Return submitted
        Return bReturn
    End Function
    Public Function obtainPhaseFolder_byCampaign(ByVal thisNode As IPublishedContent) As IPublishedContent
        'Dim nodeId As Integer?
        Try
            Dim ipNode As IPublishedContent
            If thisNode.DocumentTypeAlias = docTypes.Phases Then
                Return thisNode
            Else
                For Each childNode As IPublishedContent In thisNode.Children
                    ipNode = obtainPhaseFolder_byCampaign(childNode)
                    If Not IsNothing(ipNode) Then Return ipNode
                Next
            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqPhases.vb : obtainPhaseFolderId_byCampaign()")
            sb.AppendLine("thisNode: " & JsonConvert.SerializeObject(thisNode))
            saveErrorMessage(getLoggedInMember, ex.Message & vbNewLine & ex.StackTrace & vbNewLine & ex.Source, sb.ToString())
        End Try
        Return Nothing
    End Function
    Public Function obtainPhaseFolderId_byCampaign(ByVal thisNode As IPublishedContent) As Integer?
        Dim nodeId As Integer?
        Try
            If thisNode.DocumentTypeAlias = docTypes.Phases Then
                Return thisNode.Id
            Else
                For Each childNode As IPublishedContent In thisNode.Children
                    nodeId = obtainPhaseFolderId_byCampaign(childNode)
                    If Not IsNothing(nodeId) Then Return nodeId
                Next
            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqPhases.vb : obtainPhaseFolderId_byCampaign()")
            sb.AppendLine("thisNode: " & JsonConvert.SerializeObject(thisNode))
            saveErrorMessage(getLoggedInMember, ex.Message & vbNewLine & ex.StackTrace & vbNewLine & ex.Source, sb.ToString())
        End Try

        Return nodeId
    End Function
    Public Function obtainDiscoveryPhase_byCampaignId(ByVal campaignId As Integer) As BusinessReturn
        'Instantiate scope variables
        Dim businessReturn As New BusinessReturn

        Try
            'Instantiate variables
            Dim ipCampaign As IPublishedContent = _uHelper.Get_IPublishedContentByID(campaignId)
            Dim ipDiscoveryPhase As IPublishedContent = ipCampaign.Children.Where(Function(x As IPublishedContent) (x.DocumentTypeAlias.Equals(docTypes.discovery))).FirstOrDefault

            'discoveryPhaseExist = Not IsNothing(ipDiscoveryPhase)

            If IsNothing(ipDiscoveryPhase) Then
                businessReturn.isValid = False
            Else
                businessReturn.DataContainer.Add(ipDiscoveryPhase)
            End If

            Return businessReturn

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqPhases.vb : obtainDiscoveryPhaseId_byCampaignId()")
            sb.AppendLine("campaignId: " & campaignId.ToString)
            saveErrorMessage(getLoggedInMember, ex.Message & vbNewLine & ex.StackTrace & vbNewLine & ex.Source, sb.ToString())

            businessReturn.isValid = False
            Return businessReturn
        End Try
    End Function
#End Region

#Region "Inserts"
    Public Function CreatePhases(ByVal parentNodeId As Int16) As Int16?
        Try
            'Create a new phase IPublishedContent
            Dim cs As IContentService = ApplicationContext.Current.Services.ContentService
            Dim campaign As IContent = cs.GetById(parentNodeId)
            Dim phases As IContent = cs.CreateContentWithIdentity(docTypes.Phases, campaign, docTypes.Phases)
            cs.SaveAndPublishWithStatus(phases)
            'Return new IPublishedContent's Id
            Return phases.Id
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqPhases.vb : CreatePhases()")
            sb.AppendLine("parentNodeId: " & parentNodeId)
            saveErrorMessage(getLoggedInMember, ex.Message & vbNewLine & ex.StackTrace & vbNewLine & ex.Source, sb.ToString())
            Return Nothing
        End Try
    End Function
    Public Function CreatePreviousPhasesFolder(ByVal parentNodeId As Int32) As Int32?
        Try
            'Create a new phase IPublishedContent
            Dim cs As IContentService = ApplicationContext.Current.Services.ContentService
            Dim campaign As IContent = cs.GetById(parentNodeId)
            Dim previousPhases As IContent = cs.CreateContentWithIdentity(Miscellaneous.PreviousPhases, campaign, docTypes.PreviousPhases)
            cs.SaveAndPublishWithStatus(previousPhases)
            'Return new IPublishedContent's Id
            Return previousPhases.Id
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqPhases.vb : CreatePreviousPhasesFolder()")
            sb.AppendLine("parentNodeId: " & parentNodeId)
            saveErrorMessage(getLoggedInMember, ex.Message & vbNewLine & ex.StackTrace & vbNewLine & ex.Source, sb.ToString())
            Return Nothing
        End Try
    End Function
    Public Function CreatePhase(ByVal parentNodeId As Int16, Optional ByVal phaseNo As Int16? = Nothing) As Int16?
        Try

            'Is this first phase?
            Dim firstPhase As Boolean = True
            If _uHelper.Get_IPublishedContentByID(parentNodeId).Children.Count > 0 Then firstPhase = False
            'Create a new phase IPublishedContent
            Dim cs As IContentService = ApplicationContext.Current.Services.ContentService
            Dim phases As IContent = cs.GetById(parentNodeId)
            Dim phase As IContent = cs.CreateContentWithIdentity(docTypes.Phase, phases, docTypes.Phase)
            'Set default values
            phase.SetValue(nodeProperties.goal, 0)
            phase.SetValue(nodeProperties.published, firstPhase)
            If Not IsNothing(phaseNo) Then phase.Name = "Phase " & phaseNo.ToString & "- Unpublished"

            'Set Phase Number to define either phase is first one, second one. ... etc.
            If firstPhase Then
                phase.SetValue(nodeProperties.phaseNumber, 1)
            Else
                phase.SetValue(nodeProperties.phaseNumber, _uHelper.Get_IPublishedContentByID(parentNodeId).Children.Count + 1)
            End If

            'Save new phase
            cs.SaveAndPublishWithStatus(phase)
            'Return new IPublishedContent's Id
            Return phase.Id
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqPhases.vb : CreatePhase()")
            sb.AppendLine("parentNodeId: " & parentNodeId)
            saveErrorMessage(getLoggedInMember, ex.Message & vbNewLine & ex.StackTrace & vbNewLine & ex.Source, sb.ToString())
            Return Nothing
        End Try
    End Function
    Public Function createDiscoveryPhase(ByVal campaignId As Integer, Optional ByVal setInactive As Boolean = False) As BusinessReturn
        'Instantiate upper variables
        Dim businessReturn As New BusinessReturn

        Try
            'Instantiate variables
            Dim campaignNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(campaignId)
            Dim lstDiscoveryNodes As List(Of IPublishedContent) = campaignNode.Children.OfTypes(docTypes.discovery).ToList()
            Dim updateResponse As Attempt(Of PublishStatus)
            Dim cs As IContentService = ApplicationContext.Current.Services.ContentService
            Dim iCampaign As IContent = cs.GetById(campaignId)
            Dim discoveryNode As IContent


            If lstDiscoveryNodes.Count > 0 Then
                'Update existing discovery IPublishedContent
                discoveryNode = cs.GetById(lstDiscoveryNodes(0).Id)
            Else
                'Create new discovery IPublishedContent
                discoveryNode = cs.CreateContentWithIdentity(docTypes.discovery.ToFirstUpper, iCampaign, docTypes.discovery)
            End If

            'Set values
            If Not setInactive Then
                discoveryNode.SetValue(nodeProperties.phaseActive, True)
                discoveryNode.SetValue(nodeProperties.activationDate, DateTime.Now)
                discoveryNode.SetValue(nodeProperties.completionDate, DateTime.Now.AddDays(phaseLength))
            End If


            'Save values
            updateResponse = cs.SaveAndPublishWithStatus(discoveryNode)


            If Not updateResponse.Success Then
                Dim sb As New StringBuilder()
                sb.AppendLine("linqPhases.vb : createDiscoveryPhase()")
                sb.AppendLine("Failed to create/update the Discovery Phase")
                sb.AppendLine("campaignId: " & campaignId.ToString)
                sb.AppendLine("discoveryNode.Id: " & discoveryNode.Id)
                saveErrorMessage(getLoggedInMember, updateResponse.Exception.ToString, sb.ToString())
                businessReturn.ExceptionMessage = updateResponse.Exception.ToString
            End If

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqPhases.vb : createDiscoveryPhase()")
            sb.AppendLine("campaignId: " & campaignId.ToString)
            saveErrorMessage(getLoggedInMember, ex.Message & vbNewLine & ex.StackTrace & vbNewLine & ex.Source, sb.ToString())
            businessReturn.ExceptionMessage = "Error: " & ex.ToString
        End Try

        Return businessReturn
    End Function
    Public Function createReview(ByVal _rating As Int16, ByVal _review As String, ByVal _campaignId As Integer, ByVal _memberId As Integer) As BusinessReturn
        'Instantiate upper variables
        Dim businessReturn As New BusinessReturn

        Try
            'Instantiate variables
            Dim uHelper As UmbracoHelper = New umbraco.Web.UmbracoHelper(umbraco.Web.UmbracoContext.Current)
            Dim cs As IContentService = ApplicationContext.Current.Services.ContentService
            Dim ipCampaign As IPublishedContent
            Dim ipDiscoveryPhase As IPublishedContent
            Dim icDiscoveryPhase As IContent
            Dim icNewReview As IContent
            Dim updateResponse As Attempt(Of PublishStatus)
            Dim blMembers As New blMembers

            'Obtain discovery IPublishedContent
            ipCampaign = uHelper.TypedContent(_campaignId)
            ipDiscoveryPhase = ipCampaign.Children.Where(Function(x) (x.DocumentTypeAlias = docTypes.discovery)).FirstOrDefault()

            'Create new review IPublishedContent
            icDiscoveryPhase = cs.GetById(ipDiscoveryPhase.Id)
            icNewReview = cs.CreateContentWithIdentity(DateTime.Now.ToString("yyyy-MM-dd HH:mm:tt"), icDiscoveryPhase, docTypes.review)

            'Set review values
            icNewReview.SetValue(nodeProperties.stars, _rating)
            icNewReview.SetValue(nodeProperties.message, _review)
            icNewReview.SetValue(nodeProperties.member, _memberId)
            icNewReview.SetValue(nodeProperties.keepInformed, True)

            'Save values
            updateResponse = cs.SaveAndPublishWithStatus(icNewReview)

            If updateResponse.Success Then
                'Save review details in member profile
                blMembers.UpdateReviews(_memberId, Convert.ToInt32(icNewReview.Id))
            Else
                'Save failed response
                Dim sb As New StringBuilder()
                sb.AppendLine("linqPhases.vb : createReview()")
                sb.AppendLine("Failed to create/update a new review")
                sb.AppendLine("campaignId: " & _campaignId)
                sb.AppendLine("memberId: " & _memberId)
                sb.AppendLine("rating: " & _rating)
                sb.AppendLine("review: " & _review)
                saveErrorMessage(getLoggedInMember, updateResponse.Exception.ToString, sb.ToString())
                businessReturn.ExceptionMessage = updateResponse.Exception.ToString
            End If

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqPhases.vb : createReview()")
            sb.AppendLine("campaignId: " & _campaignId)
            sb.AppendLine("memberId: " & _memberId)
            sb.AppendLine("rating: " & _rating)
            sb.AppendLine("review: " & _review)
            saveErrorMessage(getLoggedInMember, ex.Message & vbNewLine & ex.StackTrace & vbNewLine & ex.Source, sb.ToString())
            businessReturn.ExceptionMessage = "Error: " & ex.ToString
        End Try

        Return businessReturn
    End Function
#End Region

#Region "Updates"
    Public Sub DeactivateDiscoveryPhase(ByVal _nodeId As Int32, ByVal _notes As String, Optional ByVal _isDiscoveryId As Boolean = False)
        Try
            'Instantiate variables
            Dim discoveryNode As IPublishedContent
            'Dim thisNode As IPublishedContent = New IPublishedContent(_nodeId)


            'Obtain discovery IPublishedContent
            If _isDiscoveryId Then
                discoveryNode = _uHelper.Get_IPublishedContentByID(_nodeId)
            Else
                discoveryNode = _uHelper.Get_IPublishedContentByID(_nodeId).Children.OfTypes(docTypes.discovery).ToList().FirstOrDefault
            End If


            If Not IsNothing(discoveryNode) AndAlso discoveryNode.Id > 0 Then
                'For Each discoveryNode In thisNode.GetChildNodesByType(docTypes.discovery).ToList()
                'Instantiate variables
                Dim updateResponse As Attempt(Of PublishStatus)
                Dim cs As IContentService = ApplicationContext.Current.Services.ContentService
                Dim discovery As IContent = cs.GetById(discoveryNode.Id)

                'Set values
                discovery.SetValue(nodeProperties.phaseActive, False)
                discovery.SetValue(nodeProperties.notes, addToNotes(discovery.Id, DateTime.Now & " | " & _notes))
                discovery.SetValue(nodeProperties.completionDate, DateTime.Now)

                'Save values
                updateResponse = cs.SaveAndPublishWithStatus(discovery)

                'Leave error messages in umbraco error log.
                If Not updateResponse.Success Then
                    Dim sb As New StringBuilder()
                    sb.AppendLine("linqPhases.vb : DeactivateDiscoveryPhase()")
                    sb.AppendLine("Failed to deactivate the Discovery Phase")
                    sb.AppendLine("_nodeId: " & _nodeId.ToString)
                    sb.AppendLine("_notes: " & _notes)
                    sb.AppendLine("discoveryNode.Id: " & discoveryNode.Id)
                    saveErrorMessage(getLoggedInMember, updateResponse.Exception.ToString, sb.ToString())
                End If
                'Next
            End If


        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqPhases.vb : DeactivateDiscoveryPhase()")
            sb.AppendLine("_nodeId: " & _nodeId.ToString)
            sb.AppendLine("_notes: " & _notes)
            sb.AppendLine("_isDiscoveryId" & _isDiscoveryId)
            saveErrorMessage(getLoggedInMember, ex.Message & vbNewLine & ex.StackTrace & vbNewLine & ex.Source, sb.ToString())
        End Try
    End Sub
    Public Function UpdatePhase(ByVal _nodeId As Int16, ByVal _title As String, ByVal _published As Boolean, ByVal _goal As String, ByVal _phaseDescription As String) As BusinessReturn
        'Instantiate variables
        Dim BusinessReturn As New BusinessReturn()

        Try
            Dim cs As IContentService = ApplicationContext.Current.Services.ContentService
            Dim phase As IContent = cs.GetById(_nodeId)
            Dim updateResponse As Attempt(Of PublishStatus)

            'Set values
            phase.Name = _title
            phase.SetValue(nodeProperties.published, _published)
            phase.SetValue(nodeProperties.goal, _goal)
            phase.SetValue(nodeProperties.phaseDescription, _phaseDescription)

            'Save values
            updateResponse = cs.SaveAndPublishWithStatus(phase)
            If (updateResponse.Success) Then
                BusinessReturn.ReturnMessage = _nodeId
                Return BusinessReturn
            Else
                BusinessReturn.ExceptionMessage = updateResponse.Exception.ToString
                Return BusinessReturn
            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqPhases.vb : UpdatePhase()")
            sb.AppendLine("_nodeId: " & _nodeId)
            sb.AppendLine("_published: " & _published)
            sb.AppendLine("_goal: " & _goal)
            sb.AppendLine("_phaseDescription" & _phaseDescription)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return BusinessReturn
        End Try
    End Function
    Public Function UpdatePhase(ByVal _nodeId As Int16, ByVal _published As Boolean, ByVal _phaseComplete As Boolean) As BusinessReturn
        Dim BusinessReturn As New BusinessReturn()

        Try
            'Instantiate variables
            Dim cs As IContentService = ApplicationContext.Current.Services.ContentService
            Dim phase As IContent = cs.GetById(_nodeId)
            Dim updateResponse As Attempt(Of PublishStatus)

            'Set values
            phase.SetValue(nodeProperties.published, _published)
            phase.SetValue(nodeProperties.phaseComplete, _phaseComplete)
            phase.SetValue(nodeProperties.completionDate, DateTime.Now)

            'Save values
            updateResponse = cs.SaveAndPublishWithStatus(phase)

            If (updateResponse.Success) Then
                BusinessReturn.ReturnMessage = _nodeId
                Return BusinessReturn
            Else
                BusinessReturn.ExceptionMessage = updateResponse.Exception.ToString
                Return BusinessReturn
            End If

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqPhases.vb : UpdatePhase()")
            sb.AppendLine("_nodeId: " & _nodeId)
            sb.AppendLine("_published: " & _published)
            sb.AppendLine("_phaseComplete: " & _phaseComplete)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return BusinessReturn
        End Try
    End Function
    Public Function UpdatePhase(ByVal _nodeId As Int32, ByVal _order As Int32) As Boolean
        Try
            'Instantiate variables
            Dim campaignNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(_nodeId) '.Parent
            Dim updateResponse As Attempt(Of PublishStatus)
            Dim loopCount As Int16 = 1
            'Dim cs As IContentService = ApplicationContext.Current.Services.ContentService
            'Dim discovery As IContent = cs.GetById(_nodeId)

            '
            For Each phasesNode In campaignNode.Children.OfTypes(docTypes.Phases).ToList()
                For Each phase In phasesNode.Children.OfTypes(docTypes.Phase).ToList()
                    If (loopCount + 1) = _order Then
                        Dim cServices As IContentService = ApplicationContext.Current.Services.ContentService
                        Dim cPhase As IContent = cServices.GetById(phase.Id)
                        'Set values
                        cPhase.SetValue(nodeProperties.phaseActive, False)
                        'Save values
                        updateResponse = cServices.SaveAndPublishWithStatus(cPhase)
                    ElseIf loopCount = _order Then
                        Dim cServices As IContentService = ApplicationContext.Current.Services.ContentService
                        Dim cPhase As IContent = cServices.GetById(phase.Id)
                        'Set values
                        cPhase.SetValue(nodeProperties.phaseActive, True)
                        cPhase.SetValue(nodeProperties.activationDate, DateTime.Now)
                        cPhase.SetValue(nodeProperties.completionDate, DateTime.Now.AddDays(phaseLength))
                        cPhase.SetValue(nodeProperties.failedDate, String.Empty)
                        cPhase.SetValue(nodeProperties.phaseFailed, False)
                        cPhase.SetValue(nodeProperties.notes, addToNotes(cPhase.Id, DateTime.Now & " | Phase activated."))
                        'Save values
                        updateResponse = cServices.SaveAndPublishWithStatus(cPhase)
                        Exit For
                    End If
                    loopCount += 1
                Next
            Next
            Return True
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqPhases.vb : UpdatePhase()")
            sb.AppendLine("_nodeId: " & _nodeId.ToString)
            sb.AppendLine("_order: " & _order.ToString)

            saveErrorMessage(getLoggedInMember, ex.Message & vbNewLine & ex.StackTrace & vbNewLine & ex.Source, sb.ToString())
            Return False
        End Try
    End Function
    Public Sub UpdateSuccessfulPhase(ByVal activePhase As IPublishedContent)
        Try
            'Instantiate variables
            Dim cs As IContentService = ApplicationContext.Current.Services.ContentService
            Dim CurrentPhase As IContent = cs.GetById(activePhase.Id)
            Dim blPledges As New blPledges
            Dim sb As New StringBuilder

            'Set values in phase.
            sb.AppendLine(DateTime.Now.ToLongDateString & " @ " & DateTime.Now.ToLongTimeString)
            sb.AppendLine("Phase was completed successfully.")
            CurrentPhase.SetValue(nodeProperties.notes, addToNotes(CurrentPhase.Id, sb.ToString))

            CurrentPhase.SetValue(nodeProperties.phaseComplete, True)
            CurrentPhase.SetValue(nodeProperties.completionDate, DateTime.Now.ToString())
            CurrentPhase.SetValue(nodeProperties.phaseActive, False)
            CurrentPhase.SetValue(nodeProperties.published, True)
            'CurrentPhase.SetValue(nodeProperties.pledgedTotal, Convert.ToString(blPledges.selectSum_byPhaseId(activePhase.Id)))

            'CurrentPhase.SetValue(nodeProperties.fulfilledTotal, Convert.ToString(blPledges.selectCount_byPhaseId(activePhase.Id, nodeProperties.fulfilled)))
            'CurrentPhase.SetValue(nodeProperties.reimbursedTotal, Convert.ToString(blPledges.selectCount_byPhaseId(activePhase.Id, nodeProperties.reimbursed)))
            'CurrentPhase.SetValue(nodeProperties.canceledTotal, Convert.ToString(blPledges.selectCount_byPhaseId(activePhase.Id, nodeProperties.canceled)))
            'CurrentPhase.SetValue(nodeProperties.declinedTotal, Convert.ToString(blPledges.selectCount_byPhaseId(activePhase.Id, nodeProperties.transactionDeclined)))

            'CurrentPhase.SetValue(nodeProperties.pledgeCount, Convert.ToString(blPledges.selectCount_byPhaseId(activePhase.Id)))
            'CurrentPhase.SetValue(nodeProperties.fulfilledCount, Convert.ToString(blPledges.selectCount_byPhaseId(activePhase.Id, nodeProperties.fulfilled)))
            'CurrentPhase.SetValue(nodeProperties.reimbursedCount, Convert.ToString(blPledges.selectCount_byPhaseId(activePhase.Id, nodeProperties.reimbursed)))
            'CurrentPhase.SetValue(nodeProperties.canceledCount, Convert.ToString(blPledges.selectCount_byPhaseId(activePhase.Id, nodeProperties.canceled)))
            'CurrentPhase.SetValue(nodeProperties.declinedCount, Convert.ToString(blPledges.selectCount_byPhaseId(activePhase.Id, nodeProperties.transactionDeclined)))

            cs.SaveAndPublishWithStatus(CurrentPhase)
            cs.Save(CurrentPhase)
            cs.RePublishAll()
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqPhases.vb : UpdateSuccessfulPhase()")
            sb.AppendLine("activePhase: " & Newtonsoft.Json.JsonConvert.SerializeObject(activePhase))
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
    End Sub
    Public Sub CopyPhase(ByVal _nodeId As Integer)
        Try
            'Instantiate variables
            Dim blPledges As New blPledges
            Dim cs As IContentService = ApplicationContext.Current.Services.ContentService
            Dim lstUDIs As New List(Of String)

            'Obtain failed IPublishedContent/iContent
            Dim failedPhase As IContent = cs.GetById(_nodeId)
            Dim failedPhaseNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(_nodeId)
            'Locate previousPhase folder
            Dim previousPhase As IPublishedContent = failedPhaseNode.Parent.Parent.Children.OfTypes(docTypes.PreviousPhases).FirstOrDefault()

            'Does a previous phase folder exist?
            If IsNothing(previousPhase) Then
                'Create new previousPhase folder
                previousPhase = _uHelper.Get_IPublishedContentByID(CreatePreviousPhasesFolder(failedPhase.Parent.ParentId))
            End If

            'Create copy of failed phase and relocate in previousPhase folder
            Dim copiedPhase As IContent = cs.Copy(failedPhase, previousPhase.Id, False)

            'Set coppied IPublishedContent's values and save
            copiedPhase.SetValue(nodeProperties.notes, addToNotes(copiedPhase.Id, DateTime.Now & " | Previous phase was unsuccessful.  Copied phase."))
            copiedPhase.SetValue(nodeProperties.phaseComplete, True)
            copiedPhase.SetValue(nodeProperties.phaseActive, False)
            copiedPhase.SetValue(nodeProperties.published, True)
            'copiedPhase.SetValue(nodeProperties.pledgedTotal, Convert.ToString(blPledges.selectSum_byPhaseId(_nodeId)))
            'copiedPhase.SetValue(nodeProperties.pledgeCount, Convert.ToString(blPledges.selectCount_byPhaseId(_nodeId)))
            cs.SaveAndPublishWithStatus(copiedPhase)
            cs.Publish(copiedPhase)

            'Extract existing previous phases and add new phase udi
            If Not String.IsNullOrEmpty(failedPhase.GetValue(Of String)(nodeProperties.previousPhases)) Then
                lstUDIs = failedPhase.GetValue(Of String)(nodeProperties.previousPhases).Split(",").ToList
            End If
            lstUDIs.Add(Udi.Create(UdiEntityType.Document, copiedPhase.Key).ToString)

            'Update fail phase's values and save.
            failedPhase.SetValue(nodeProperties.previousPhases, String.Join(",", lstUDIs.ToArray()))
            failedPhase.SetValue(nodeProperties.notes, addToNotes(failedPhase.Id, DateTime.Now & " | Previous phase was unsuccessful."))
            cs.SaveAndPublishWithStatus(failedPhase)
            cs.Publish(failedPhase)

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqPhases.vb : CopyPhase()")
            sb.AppendLine("_nodeId: " & _nodeId.ToString)
            saveErrorMessage(getLoggedInMember, ex.Message & vbNewLine & ex.StackTrace & vbNewLine & ex.Source, sb.ToString())
        End Try
    End Sub
    Public Function ResetPhase(ByVal _phaseId As Int16) As BusinessReturn
        'Instantiate scope variables
        Dim BusinessReturn As New BusinessReturn()

        Try
            'Instantiate variables
            Dim updateResponse As Attempt(Of PublishStatus)
            Dim cs As IContentService = ApplicationContext.Current.Services.ContentService
            Dim phase As IContent = cs.GetById(_phaseId)

            'Set values
            phase.SetValue(nodeProperties.phaseActive, False)
            phase.SetValue(nodeProperties.phaseFailed, True)
            phase.SetValue(nodeProperties.phaseComplete, False)

            phase.SetValue(nodeProperties.activationDate, String.Empty)
            phase.SetValue(nodeProperties.failedDate, DateTime.Now)
            phase.SetValue(nodeProperties.completionDate, String.Empty)

            phase.SetValue(nodeProperties.notes, addToNotes(phase.Id, DateTime.Now & " | Previous phase was unsuccessful."))

            'Save values
            updateResponse = cs.SaveAndPublishWithStatus(phase)
            'cs.Publish(phase)

            If (updateResponse.Success) Then
                'Delete child pledge nodes
                For Each pledge In phase.Children
                    cs.Delete(pledge)
                Next

                'Return result
                BusinessReturn.ReturnMessage = _phaseId
                Return BusinessReturn
            Else
                Dim sb As New StringBuilder()
                sb.AppendLine("linqPhases.vb : ResetPhase()")
                sb.AppendLine("_phaseId: " & _phaseId.ToString)
                saveErrorMessage(getLoggedInMember, "Failed to update phase: " & updateResponse.Exception.ToString, sb.ToString())

                BusinessReturn.ExceptionMessage = updateResponse.Exception.ToString
                Return BusinessReturn
            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqPhases.vb : ResetPhase()")
            sb.AppendLine("_phaseId: " & _phaseId.ToString)
            saveErrorMessage(getLoggedInMember, ex.Message & vbNewLine & ex.StackTrace & vbNewLine & ex.Source, sb.ToString())

            BusinessReturn.ExceptionMessage = ex.ToString
            Return BusinessReturn
        End Try
    End Function
#End Region

#Region "Delete"
#End Region

#Region "Methods"
    Public Function doesDiscoveryPhaseExist(ByVal campaignId As Integer) As Boolean
        'Instantiate upper-variables
        Dim discoveryPhaseExist As Boolean = False

        Try
            'Instantiate variables

            Dim ipCampaign As IPublishedContent = _uHelper.Get_IPublishedContentByID(campaignId)
            Dim ipDiscoveryPhase As IPublishedContent = ipCampaign.Children.Where(Function(x As IPublishedContent) (x.DocumentTypeAlias.Equals(docTypes.discovery))).FirstOrDefault

            discoveryPhaseExist = Not IsNothing(ipDiscoveryPhase)

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqPhases.vb : doesDiscoveryPhaseExist()")
            sb.AppendLine("campaignId: " & campaignId.ToString)
            saveErrorMessage(getLoggedInMember, ex.Message & vbNewLine & ex.StackTrace & vbNewLine & ex.Source, sb.ToString())
        End Try

        Return discoveryPhaseExist
    End Function
    Public Function anyActivePhases_byCampaignId(ByVal _campaignId As Integer) As Boolean
        Try
            'Instantiate variables
            Dim phaseFolderId As Integer? = obtainPhaseFolderId_byCampaign(_uHelper.Get_IPublishedContentByID(_campaignId))

            If Not IsNothing(phaseFolderId) Then
                'Loop thru and locate active phase
                Dim phasesFolder As IPublishedContent = _uHelper.Get_IPublishedContentByID(phaseFolderId)
                For Each phase As IPublishedContent In phasesFolder.Children
                    If phase.HasProperty(nodeProperties.phaseActive) AndAlso phase.GetPropertyValue(Of Boolean)(nodeProperties.phaseActive) = True Then
                        Return True
                    End If
                Next
            End If

            'No active phases. 
            Return False

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqPhases.vb : anyActivePhases_byCampaignId()")
            sb.AppendLine("_campaignId:" & _campaignId)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

            Return False
        End Try
    End Function
    Public Function anyPendingPhases(ByVal ipCampaign As IPublishedContent) As Boolean
        Try
            'Instantiate variables
            Dim ipPhaseFolder As IPublishedContent = obtainPhaseFolder_byCampaign(ipCampaign)
            Dim cs As IContentService = ApplicationContext.Current.Services.ContentService
            Dim icPhase As IContent

            'Determine if any phases are pending
            If Not IsNothing(ipPhaseFolder) Then
                For Each phase As IPublishedContent In ipPhaseFolder.Children
                    icPhase = cs.GetById(phase.Id)
                    If icPhase.GetValue(Of Boolean)(nodeProperties.published) = True Then
                        If icPhase.GetValue(Of Boolean)(nodeProperties.phaseComplete) = False Then
                            Return True
                        End If
                    End If
                Next
            End If

            Return False
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqPhases.vb : anyPendingPhases()")
            sb.AppendLine("campaign: " & ipCampaign.Id & " | " & ipCampaign.Name)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

            Return False
        End Try
    End Function
    Public Function publishedPhaseCount_byCampaignId(ByVal _campaignId As Integer) As UInt16
        Try
            'Instantiate variables
            Dim phaseFolderId As Integer? = obtainPhaseFolderId_byCampaign(_uHelper.Get_IPublishedContentByID(_campaignId))
            Dim phaseCount As UInt16 = 0

            If Not IsNothing(phaseFolderId) Then
                'Loop thru and locate published phases
                Dim phasesFolder As IPublishedContent = _uHelper.Get_IPublishedContentByID(phaseFolderId)
                For Each phase As IPublishedContent In phasesFolder.Children
                    If phase.HasProperty(nodeProperties.published) AndAlso phase.GetPropertyValue(Of Boolean)(nodeProperties.published) = True Then
                        phaseCount += 1
                    End If
                Next
            End If

            '
            Return phaseCount

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqPhases.vb : publishedPhaseCount_byCampaignId()")
            sb.AppendLine("_campaignId:" & _campaignId)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

            Return False
        End Try
    End Function
#End Region

End Class