Imports umbraco
Imports Common
Imports umbraco.Web
Imports umbraco.Core.Models

Partial Class UserControls_CampaignSummary
    Inherits System.Web.UI.UserControl

#Region "Properties"
    Dim _uHelper As Uhelper = New Uhelper()
    Dim _campaignGoal As Decimal = 0
    Public Property campaignGoal As Decimal
        Get
            Return _campaignGoal
        End Get
        Set(value As Decimal)
            _campaignGoal = value
        End Set
    End Property

    Dim _campaignPledges As Decimal = 0
    Public Property campaignPledges As Decimal
        Get
            Return _campaignPledges
        End Get
        Set(value As Decimal)
            _campaignPledges = value
        End Set
    End Property

    Dim _campaignContributors As UInt16 = 0
    Public Property campaignContributors As Int16
        Get
            Return _campaignContributors
        End Get
        Set(value As Int16)
            _campaignContributors = value
        End Set
    End Property

    Dim _percentFundedValue As String = ""
    Public Property percentFundedValue As String
        Get
            Return _percentFundedValue
        End Get
        Set(value As String)
            _percentFundedValue = value
        End Set
    End Property

    Dim _endorsementMode As Boolean = False
    Public Property endorsementMode As Boolean
        Get
            Return _endorsementMode
        End Get
        Set(value As Boolean)
            _endorsementMode = value
        End Set
    End Property

#End Region

#Region "Handles"
    Private Sub UserControls_CampaignSummary_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        If Not IsPostBack Then
            Try
                'If endorsementMode Then
                '    mvSummaryPanels.ActiveViewIndex = 0
                'Else
                '    mvSummaryPanels.ActiveViewIndex = 1
                'Display campaign goal
                Dim thisNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(UmbracoContext.Current.PageId)
                Dim goal As String() = campaignGoal.ToString.Split(".")
                ltrlGoalDollarAmt.Text = String.Format("{0:c0}", CInt(goal(0)))
                If goal.Count > 1 Then ltrlGoalCents.Text = goal(1).Substring(0, 2) Else ltrlGoalCents.Text = "00"

                'Display total pledges
                Dim pledges As String() = campaignPledges.ToString.Split(".")
                ltrlPledgedDollarAmt.Text = String.Format("{0:c0}", CInt(pledges(0)))
                If pledges.Count > 1 Then ltrlPledgedCents.Text = pledges(1).Substring(0, 2) Else ltrlPledgedCents.Text = "00"

                'Obtain completion date and estimate time remaining
                If thisNode.HasProperty(nodeProperties.completionDate) Then
                    If thisNode.HasValue(nodeProperties.completionDate) Then
                        'Show completion date
                        Dim completionDate As DateTime = thisNode.GetPropertyValue(Of DateTime)(nodeProperties.completionDate)
                        ltrlCompletionDate.Text = completionDate.ToString("[ddd] MMMM dd, yyyy")
                    Else
                        phCompletionDate.Visible = False
                    End If



                    'If String.IsNullOrEmpty(thisNode.GetPropertyValue(Of String)(nodeProperties.completionDate).Trim) Then
                    '    'hide panel
                    '    phCompletionDate.Visible = False
                    'Else
                    '    'Completion Date
                    '    Response.Write("thisNode: " & thisNode.Name)
                    '    Dim completionDate As DateTime = thisNode.GetPropertyValue(Of DateTime)(nodeProperties.completionDate)
                    '    ltrlCompletionDate.Text = completionDate.ToString("[ddd] MMMM dd, yyyy")
                    'End If
                Else
                    'hide panel
                    phCompletionDate.Visible = False
                End If


                'Display other summary data.
                ltrlTotalContributors.Text = campaignContributors.ToString
                If campaignGoal > 0 Then
                    Dim percentFunded As String() = ((campaignPledges / campaignGoal) * 100).ToString.Split(".")
                    ltrlPercentFunded.Text = percentFunded(0)

                    'Save percent funded to property for parent control to grab.
                    percentFundedValue = percentFunded(0)
                End If

                'End If
            Catch ex As Exception
                Dim sb As New StringBuilder()
                sb.AppendLine("\UserControls\CampaignSummary.ascx.vb : UserControls_CampaignSummary_PreRender()")

                saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
                'Response.Write(ex.ToString)
            End Try
        End If
    End Sub
#End Region

#Region "Methods"
#End Region
End Class



'Private Sub timerTimeRemaining_Tick(sender As Object, e As EventArgs) Handles timerTimeRemaining.Tick
'    If Not IsNothing(hfldSeconds.Value) Then
'        '
'        Dim daysLeft As Int16 = Convert.ToInt16(hfldDays.Value)
'        Dim hoursLeft As Int16 = Convert.ToInt16(hfldHours.Value)
'        Dim minutesLeft As Int16 = Convert.ToInt16(hfldMinutes.Value)
'        Dim secondsLeft As Int16 = Convert.ToInt16(hfldSeconds.Value)

'        '
'        secondsLeft -= 1

'        '
'        If secondsLeft < 0 Then
'            secondsLeft = 59
'            minutesLeft -= 1
'        End If
'        If minutesLeft < 0 Then
'            minutesLeft = 59
'            hoursLeft -= 1
'        End If
'        If hoursLeft < 0 Then
'            hoursLeft = 59
'            daysLeft -= 1
'        End If

'        'Create time span
'        Dim sbSpan As StringBuilder = New StringBuilder
'        sbSpan.Append(daysLeft.ToString("D2") & "d : ")
'        sbSpan.Append(hoursLeft.ToString("D2") & "h : ")
'        'sbSpan.Append(minutesLeft.ToString("D2") & "m : ")
'        'sbSpan.Append(secondsLeft.ToString("D2") & "s")
'        sbSpan.Append(minutesLeft.ToString("D2") & "m")
'        lblTimeRemaining.Text = sbSpan.ToString

'        '
'        hfldDays.Value = daysLeft
'        hfldHours.Value = hoursLeft
'        hfldMinutes.Value = minutesLeft
'        hfldSeconds.Value = secondsLeft
'    End If


'End Sub