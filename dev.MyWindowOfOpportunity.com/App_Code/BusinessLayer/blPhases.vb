Imports Common
Imports Newtonsoft.Json
Imports umbraco.Core.Models

Public Class blPhases

#Region "Properties"
    Private linqPhases As LinqPhases = New LinqPhases
    Private _uHelper As Uhelper = New Uhelper()
#End Region


#Region "Selects"
    Public Function getGoal_byCampaignId(ByVal _campaignId As Integer) As Decimal?
        Return linqPhases.getGoal_byCampaignId(_campaignId)
    End Function
    Public Function obtainActivePhaseId_byCampaignId(ByVal _campaignId As Integer) As Integer?
        Return linqPhases.obtainActivePhaseId_byCampaignId(_campaignId)
    End Function
    Public Function obtainActivePhaseNo_byCampaignId(ByVal campaignId As Integer) As Integer?
        Return linqPhases.obtainActivePhaseNo_byCampaignId(campaignId)
    End Function
    Public Function pendingPhasesExist_byCampaignId(ByVal _campaignId As Integer) As Boolean?
        Return linqPhases.pendingPhasesExist_byCampaignId(_campaignId)
    End Function
    Public Function obtainPhaseStatuses(ByVal _campaignId As Integer, Optional ByVal _getPledges As Boolean = False) As List(Of Phase)
        Dim lstPhases As New List(Of Phase)

        Try
            'Instantiate variables
            Dim phaseFolderId As Integer? = linqPhases.obtainPhaseFolderId_byCampaign(_uHelper.Get_IPublishedContentByID(_campaignId))

            'Loop thru all phase nodes
            If Not IsNothing(phaseFolderId) Then
                Dim phaseFolder As IPublishedContent = _uHelper.Get_IPublishedContentByID(phaseFolderId)
                For Each phase As IPublishedContent In phaseFolder.Children
                    lstPhases.Add(linqPhases.obtainPhaseStatus_byId(phase.Id, _getPledges))
                Next
            End If

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("/App_Code/BusinessLayer/blPhases.vb : obtainPhaseStatuses()")
            sb.AppendLine("_campaignId:" & _campaignId.ToString())
            sb.AppendLine("_getPledges:" & _getPledges.ToString())

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try

        Return lstPhases
    End Function
    Public Function obtainDaysRemaining_byPhaseId(ByVal _phaseId As Integer) As Integer?
        Return linqPhases.obtainDaysRemaining_byPhaseId(_phaseId)
    End Function
    Public Function hasMemberSubmittedReview(ByVal _memberId As Integer, ByVal _campaignId As Integer) As BusinessReturn
        Return linqPhases.hasMemberSubmittedReview(_memberId, _campaignId)
    End Function
    Public Function createReview(ByVal _rating As Int16, ByVal _review As String, ByVal _campaignId As Integer) As BusinessReturn
        Dim blMembers As New blMembers
        Return linqPhases.createReview(_rating, _review, _campaignId, blMembers.GetCurrentMemberId())
    End Function
    Public Function obtainDiscoveryPhase_byCampaignId(ByVal campaignId As Integer) As BusinessReturn
        Return linqPhases.obtainDiscoveryPhase_byCampaignId(campaignId)
    End Function
    Public Function obtainPhaseFolder_byCampaign(ByVal thisNode As IPublishedContent) As IPublishedContent
        Return linqPhases.obtainPhaseFolder_byCampaign(thisNode)
    End Function
    Public Function obtainPhaseFolderId_byCampaign(ByVal thisNode As IPublishedContent) As Integer?
        Return linqPhases.obtainPhaseFolderId_byCampaign(thisNode)
    End Function
#End Region

#Region "Inserts"
    Public Function CreatePhases(ByVal parentNodeId As Int16) As Int16
        Return linqPhases.CreatePhases(parentNodeId)
    End Function
    Public Function CreatePhase(ByVal parentNodeId As Int16, Optional ByVal phaseNo As Int16? = Nothing) As Int16
        Return linqPhases.CreatePhase(parentNodeId, phaseNo)
    End Function
    Public Function createDiscoveryPhase(ByVal campaignId As Integer, Optional ByVal setInactive As Boolean = False) As BusinessReturn
        Return linqPhases.createDiscoveryPhase(campaignId, setInactive)
    End Function
#End Region

#Region "Updates"
    Public Function UpdatePhase(ByVal _nodeId As Int16, ByVal _title As String, ByVal _published As Boolean,
                                ByVal _goal As String, ByVal _phaseDescription As String) As BusinessReturn
        Try
            'Set default goal value if not provided
            If String.IsNullOrEmpty(_goal) Then
                _goal = "0"
            Else
                _goal = _goal.Replace("$", "").Replace(",", "")
            End If
            '_goal = Convert.ToDecimal(_goal).ToString("c")

            'Perform Validation
            Dim ValidationReturn As BusinessReturn = Validate(_title, _goal)

            'If error occured, return w/ message
            If Not ValidationReturn.isValid Then
                Return ValidationReturn
            Else
                Return linqPhases.UpdatePhase(_nodeId, _title, _published, _goal, _phaseDescription)
            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("/App_Code/BusinessLayer/blPhases.vb : UpdatePhase()")
            sb.AppendLine("_nodeId:" & _nodeId)
            sb.AppendLine("_published:" & _published)
            sb.AppendLine("_title:" & _title)
            sb.AppendLine("_goal:" & _goal)
            sb.AppendLine("_phaseDescription:" & _phaseDescription)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return Nothing
        End Try
    End Function
    Public Function UpdatePhase(ByVal _nodeId As Int16, ByVal _published As Boolean, ByVal _phaseComplete As Boolean) As BusinessReturn
        Return linqPhases.UpdatePhase(_nodeId, _published, _phaseComplete)
    End Function
    Public Sub UpdateSuccessfulPhase(ByVal activePhase As IPublishedContent)
        linqPhases.UpdateSuccessfulPhase(activePhase)
    End Sub
    Public Sub CopyPhase(ByVal _nodeId As Int16)
        linqPhases.CopyPhase(_nodeId)
    End Sub
    Public Sub DeactivateDiscoveryPhase(ByVal _nodeId As Int32, ByVal _notes As String, Optional ByVal _isDiscoveryId As Boolean = False) 'As BusinessReturn
        linqPhases.DeactivateDiscoveryPhase(_nodeId, _notes, _isDiscoveryId)
    End Sub
    Public Function UpdatePhase(ByVal _nodeId As Int32, ByVal _order As Int32) As Boolean?
        Return linqPhases.UpdatePhase(_nodeId, _order)
    End Function
    Public Function ResetPhase(ByVal _phaseId As Int16) As BusinessReturn
        Return linqPhases.ResetPhase(_phaseId)
    End Function
#End Region

#Region "Delete"
#End Region

#Region "Methods"
    Public Function doesDiscoveryPhaseExist(ByVal campaignId As Integer) As Boolean?
        Return linqPhases.doesDiscoveryPhaseExist(campaignId)
    End Function
    Public Function anyActivePhases_byCampaignId(ByVal _campaignId As Integer) As Boolean?
        Return linqPhases.anyActivePhases_byCampaignId(_campaignId)
    End Function
    Public Function anyPendingPhases(ByVal ipCampaign As IPublishedContent) As Boolean?
        Return linqPhases.anyPendingPhases(ipCampaign)
    End Function
    Public Function publishedPhaseCount_byCampaignId(ByVal _campaignId As Integer) As UInt16
        Return linqPhases.publishedPhaseCount_byCampaignId(_campaignId)
    End Function
#End Region

#Region "Validations"
    Private Function Validate(ByVal _title As String, ByVal _goal As String) As BusinessReturn
        Dim ValidationMessages As New BusinessReturn()
        Try
            If String.IsNullOrWhiteSpace(_title) Then
                'Add failure message to validation
                ValidationMessages.ValidationMessages.Add(New ValidationContainer("Title cannot be blank."))
            End If

            Dim number As Decimal
            If Not Decimal.TryParse(_goal, number) Then
                'Add failure message to validation
                ValidationMessages.ValidationMessages.Add(New ValidationContainer("Invalid numeric value for goal."))
            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("/App_Code/BusinessLayer/blPhases.vb : Validate()")
            sb.AppendLine("_title:" & _title)
            sb.AppendLine("_goal:" & _goal)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
        Return ValidationMessages

    End Function
#End Region
End Class
