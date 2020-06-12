Imports Common


Partial Class UserControls_PhaseMeter
    Inherits System.Web.UI.UserControl

#Region "Properties"
    Public Property phaseStatus() As statusType = statusType.none
    Public Property phaseCount() As Int16 = 0

    Private phaseIcon As String = "phaseIcon "
    Private Structure status
        Const active As String = "active"
        Const complete As String = "complete"
        Const discovery As String = "discovery"
        Const finished As String = "finished"
        Const inactive As String = "inactive"
    End Structure
#End Region


#Region "Handles"
    Private Sub UserControls_PhaseMeter_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            Select Case phaseStatus
                Case statusType.DiscoveryPhase
                    pnlPhaseMeter.CssClass += status.discovery
                    imgPhase01.CssClass = phaseIcon & status.discovery

                Case statusType.Phase1Active, statusType.Phase2Pending
                    imgPhase01.CssClass = phaseIcon & status.active

                Case statusType.Phase2Active, statusType.Phase3Pending
                    imgPhase01.CssClass = phaseIcon & status.complete
                    imgPhase02.CssClass = phaseIcon & status.active

                Case statusType.Phase3Active
                    imgPhase01.CssClass = phaseIcon & status.complete
                    imgPhase02.CssClass = phaseIcon & status.complete
                    imgPhase03.CssClass = phaseIcon & status.active

                Case statusType.Complete
                    pnlPhaseMeter.CssClass += status.complete
                    imgPhase01.CssClass = phaseIcon & status.complete
                    imgPhase02.CssClass = phaseIcon & status.complete
                    imgPhase03.CssClass = phaseIcon & status.complete
                    imgComplete.CssClass = phaseIcon & status.finished

                Case Else
                    'set all as inactive
            End Select

            'Show number of phases
            Select Case phaseCount
                Case 1
                    imgPhase01.Visible = True

                Case 2
                    imgPhase01.Visible = True
                    imgPhase02.Visible = True

                Case 3
                    imgPhase01.Visible = True
                    imgPhase02.Visible = True
                    imgPhase03.Visible = True

                Case Else
                    phPhaseMeter.Visible = False
            End Select

            hfldPhaseCount.Value = phaseCount
            hfldPhaseStatus.Value = phaseStatus
        Catch ex As Exception

        End Try
    End Sub
#End Region


#Region "Methods"
#End Region
End Class



'blCampaigns = New blCampaigns
'Dim isCharity As Boolean = blCampaigns.IsCampaignACharity_byId(CampaignStatistics.campaignId)


''Determine if campaign is unpublished 
'If blCampaigns.obtainStatusType_byId(CInt(thisNodeId)) <> statusType.Unpublished Then