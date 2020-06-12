Imports Common
Imports umbraco
Imports umbraco.Core.Models
Imports umbraco.Web

Partial Class UserControls_CampaignPhase
    Inherits System.Web.UI.UserControl


#Region "Properties"
    Dim _uHelper As Uhelper = New Uhelper()
    Public Property nodeId As Integer = -1
    Public Property phaseId As Integer = -1
    Public Property phaseGoal As Decimal = 0
    Public Property phasePledged As Decimal = 0
    Public Property pledgeCount As UInt16 = 0
    Public Property forceInactive As Boolean = False
    Private blPhases As blPhases
#End Region

#Region "Handles"
    Private Sub MUserControls_CampaignPhase_Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                'Instantiate variables
                Dim ipPhase As IPublishedContent = _uHelper.Get_IPublishedContentByID(nodeId)
                Dim phaseActive As Boolean = False
                Dim phaseComplete As Boolean = False
                Dim percentageFunded As UInt16 = 0

                'Obtain goal from umbraco.
                phaseGoal = ipPhase.GetPropertyValue(Of Decimal)(nodeProperties.goal)
                Dim goal As String() = phaseGoal.ToString.Split(".")
                ltrlGoalDollarAmt.Text = String.Format("{0:c0}", CInt(goal(0)))
                If goal.Count > 1 Then ltrlGoalCents.Text = goal(1) Else ltrlGoalCents.Text = "00"

                '
                ltrlTitle.Text = ipPhase.Name
                If ipPhase.HasProperty(nodeProperties.phaseActive) Then phaseActive = ipPhase.GetPropertyValue(Of Boolean)(nodeProperties.phaseActive)
                If ipPhase.HasProperty(nodeProperties.phaseComplete) Then phaseComplete = ipPhase.GetPropertyValue(Of Boolean)(nodeProperties.phaseComplete)
                If ipPhase.HasProperty(nodeProperties.phaseDescription) Then
                    ltrlPhaseDescription.Text = ipPhase.GetPropertyValue(Of String)(nodeProperties.phaseDescription)
                Else
                    h4LearnMore.Visible = False
                End If

                'Obtain list & count of all pledges for phase.
                Dim blPledges As blPledges = New blPledges
                Dim lstCampaignPledges As List(Of CampaignPledge) = blPledges.select_byPhaseId(nodeId)
                For Each pledge As CampaignPledge In lstCampaignPledges
                    If pledge.reimbursed = False And pledge.canceled = False And pledge.transactionDeclined = False Then
                        pledgeCount += 1
                        phasePledged += pledge.pledgeAmount
                    End If

                Next
                ltrlPledges.Text = pledgeCount.ToString
                Dim pledged As String() = phasePledged.ToString.Split(".")
                ltrlPledgedDollarAmt.Text = String.Format("{0:c0}", CInt(pledged(0)))
                If pledged.Count > 1 Then ltrlPledgedCents.Text = pledged(1).Substring(0, 2) Else ltrlPledgedCents.Text = "00"

                'Display phase #
                ltrlPhaseNo.Text = phaseId.ToString

                If phaseGoal > 0 Then
                    'Obtain percentage funded
                    percentageFunded = (phasePledged / phaseGoal) * 100

                    'If phase is active or not
                    If phaseActive And (forceInactive = False) Then
                        'Add phase's percentage
                        ucProgressBar.percentage = percentageFunded

                        'Show if fully funded
                        If percentageFunded > 100 Then pnlPhaseMsg.Visible = True
                        'Show button to fund phase and assign url
                        If Not phaseComplete Then pnlFundThisPhase.Visible = True
                        hlnkFundThisCampaign.NavigateUrl = _uHelper.Get_IPublishedContentByID(siteNodes.Checkout).Url & "?nodeId=" & HttpUtility.UrlEncode(ipPhase.Parent.Parent.Id)

                        'Hide the fund button if a stripe account does not exist.
                        Dim blCampaigns = New blCampaigns
                        If String.IsNullOrEmpty(blCampaigns.selectStripeUserId_byCampaignId(ipPhase.Parent.Parent.Id)) Then
                            hlnkFundThisCampaign.Visible = False
                        End If

                        'Hide fund button if no phases are active
                        blPhases = New blPhases
                        If Not blPhases.anyActivePhases_byCampaignId(ipPhase.Parent.Parent.Id) Then
                            hlnkFundThisCampaign.Visible = False
                        End If

                        'Determine how many days remain in phase
                        ltrlDaysRemaining.Text = blPhases.obtainDaysRemaining_byPhaseId(nodeId)
                        pnlDaysRemaining.Visible = True

                    ElseIf phaseComplete Then
                        pnlPhase.CssClass += " complete"
                        pnlProgressBar.Visible = False
                        pnlComplete.Visible = True
                    Else
                        pnlPhase.CssClass += " inactive"
                        pnlProgressBar.Visible = False
                        pnlPendingPhase.Visible = True
                    End If
                End If

            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\CampaignPhase.ascx.vb : MUserControls_CampaignPhase_Page_Load()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Response.Write("Error: " & ex.ToString)
        End Try
    End Sub
#End Region

#Region "Methods"
#End Region

End Class
