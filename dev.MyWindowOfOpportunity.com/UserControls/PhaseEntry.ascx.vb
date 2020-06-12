Imports umbraco
Imports Common
Imports System.Data
Imports umbraco.Core
Imports umbraco.Core.Services
Imports umbraco.Core.Models
Imports umbraco.Web

Partial Class UserControls_PhaseEntry
    Inherits System.Web.UI.UserControl


#Region "Properties"
    Public Property thisNodeId() As Int16
    Public ReadOnly Property phaseTitle As String
        Get
            Return txbTitle.Text.Trim
        End Get
    End Property
    Public ReadOnly Property phaseGoal As String
        Get
            Return txbGoal.Text.Trim
        End Get
    End Property
    Public ReadOnly Property shortDescription As String
        Get
            Return txbShortDescription.Text.Trim
        End Get
    End Property
    Public ReadOnly Property currentlyActive As Boolean
        Get
            Return False 'cbActive.Checked
        End Get
    End Property
    Public ReadOnly Property published As Boolean
        Get
            Return cbPublished.Checked
        End Get
    End Property
    Public Property phaseNo As Integer = -1
    Public Property isPrimary As Boolean = False
    Public Property isDiscovery As Boolean = False
    Dim lstRatingComments As List(Of RatingComments) = New List(Of RatingComments)
    Dim _uHelper As Uhelper = New Uhelper()
#End Region

#Region "Handles"
    Private Sub UserControls_PhaseEntry_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Try
                If Not IsNothing(getCampaignId(thisNodeId)) Then
                    'Add campaign IPublishedContent id to user control
                    ucRatingSummary.thisNodeId = getCampaignId(thisNodeId)

                    Dim ms As ContentService = ApplicationContext.Current.Services.ContentService
                    Dim checkPhaseType = ms.GetById(Convert.ToInt32(thisNodeId))

                    'Add IPublishedContent Id to panel as an attribute
                    pnlPhaseEntry.Attributes.Add("nodeId", thisNodeId)

                    If isDiscovery Then
                        pnlPhaseEntry.CssClass += " isDiscovery"
                        divDiscovery.Visible = True
                        divDiscoveryDescription.Visible = True
                        pnlPhaseDescription.Visible = False
                        pnlStatusFields.Visible = False
                        divPhase.Visible = False
                        GetCampaignRating()
                    Else
                        divDiscovery.Visible = False
                        divDiscoveryDescription.Visible = False
                        pnlPhaseDescription.Visible = True
                        divPhase.Visible = True
                        'End If

                        If (isPrimary) Then
                            'remove class [hide] and hide option to turn phase on/off.
                            pnlPhaseEntry.CssClass = "row pnlPhaseEntry"
                            phPublished.Visible = False
                            phPrimaryNotes.Visible = True
                        End If

                        If thisNodeId <> -1 Then
                            'Instantiate variables
                            Dim thisNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(thisNodeId)

                            'Obtain phase status'
                            Dim isActive As Boolean = thisNode.GetPropertyValue(Of Boolean)(nodeProperties.phaseActive)
                            Dim isFailed As Boolean = thisNode.GetPropertyValue(Of Boolean)(nodeProperties.phaseFailed)
                            Dim isComplete As Boolean = thisNode.GetPropertyValue(Of Boolean)(nodeProperties.phaseComplete)
                            Dim isPublished As Boolean = thisNode.GetPropertyValue(Of Boolean)(nodeProperties.published)
                            Select Case True
                                Case isComplete
                                    ltrlStatus.Text = statusTypeValues.Complete
                                Case isFailed
                                    ltrlStatus.Text = statusTypeValues.Pending
                                Case isActive
                                    ltrlStatus.Text = statusTypeValues.Active
                                Case isPublished
                                    ltrlStatus.Text = statusTypeValues.Pending
                                Case Else
                                    ltrlStatus.Text = statusTypeValues.Inactive
                            End Select
                            Select Case True
                                Case isFailed, isComplete, isActive
                                    cbPublished.Checked = True
                                    'cbPublished.Enabled = False
                            End Select






                            'Obtain data and populate page.
                            phaseNo = thisNode.GetPropertyValue(Of Integer)(nodeProperties.phaseNumber)
                            pnlPhaseEntry.Attributes.Add("data-phaseNo", phaseNo)
                            txbGoal.Text = thisNode.GetPropertyValue(Of String)(nodeProperties.goal)
                            'display notes only on 3rd phase
                            If phaseNo = 3 Then pnlNotes.Visible = True
                            txbTitle.Text = thisNode.Name
                            If thisNode.HasProperty(nodeProperties.phaseDescription) Then txbShortDescription.Text = thisNode.GetPropertyValue(Of String)(nodeProperties.phaseDescription)
                            If thisNode.HasProperty(nodeProperties.published) Then cbPublished.Checked = thisNode.GetPropertyValue(Of Boolean)(nodeProperties.published)
                        End If
                        'check if message is to be shown.
                        Try
                            If Not IsNothing(Request.QueryString(queryParameters.entryId)) Then
                                Dim entryId As Integer = CInt(Request.QueryString(queryParameters.entryId))
                                If entryId = thisNodeId Then
                                    If Not IsNothing(Session(Miscellaneous.msgType)) Then
                                        'Show message
                                        Dim alert As ASP.usercontrols_alertmsg_ascx = New ASP.usercontrols_alertmsg_ascx With {
                                            .MessageType = CInt(Session(Miscellaneous.msgType))
                                        }
                                        Select Case CInt(Session(Miscellaneous.msgType))
                                            Case UserControls_AlertMsg.msgType.Success
                                                alert.AlertMsg = "Saved Successfully"
                                            Case UserControls_AlertMsg.msgType.Alert
                                                alert.AlertMsg = "Error. Unable to save data."
                                        'DisplayValidationErrors()
                                            Case UserControls_AlertMsg.msgType.Warning
                                        'alert.AlertMsg = ""
                                            Case UserControls_AlertMsg.msgType.Info
                                                'alert.AlertMsg = ""
                                        End Select
                                        'phAlertMsg.Controls.Add(alert)
                                        Session(Miscellaneous.msgType) = Nothing
                                    End If
                                End If
                            End If
                        Catch ex As Exception
                            Dim sb As New StringBuilder()
                            sb.AppendLine("\UserControls\PhaseEntry.ascx.vb : UserControls_PhaseEntry_Load()")
                            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
                        End Try

                        'Assign relation between checkbox and label
                        'lblActive.Attributes.Add("for", cbActive.ClientID)
                        lblPublished.Attributes.Add("for", cbPublished.ClientID)

                    End If
                End If
            Catch ex As Exception
                Dim sb As New StringBuilder()
                sb.AppendLine("\UserControls\PhaseEntry.ascx.vb : UserControls_PhaseEntry_Load()")
                saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
                'Response.Write(ex.ToString)
            End Try
        End If
    End Sub
#End Region

#Region "Methods"
    Public Sub GetCampaignRating()
        Try 'Instantiate variables
            Dim ms As ContentService = ApplicationContext.Current.Services.ContentService
            'Dim BusinessReturn As BusinessReturn
            'Dim CampaignStatistics As CampaignStatistics
            'Dim blCampaigns As New blCampaigns
            'Dim thisNode As IPublishedContent = IPublishedContent.GetCurrent
            Dim campaignNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(HttpUtility.UrlDecode(Request.QueryString("nodeId")))

            'Obtain data
            'BusinessReturn = blCampaigns.obtainStatistics(campaignNode)
            'If BusinessReturn.isValid Then
            '    'Extract data
            '    CampaignStatistics = DirectCast(BusinessReturn.DataContainer(0), CampaignStatistics)

            'Determine if discovery phase is active or not
            'Dim status As Common.statusType = CampaignStatistics.statusType
            'If status = statusType.DiscoveryPhase Then
            'Obtain discovery IPublishedContent if it exists.
            Dim campaign As IContent = ms.GetById(campaignNode.Id)
            'Dim discovery As IContent = campaign.Children().Where(Function(x As umbraco.Core.Models.Content) (x.Name.ToLower() = docTypes.discovery)).FirstOrDefault()

            Dim discoverylist = campaignNode.Children.OfTypes(docTypes.discovery).FirstOrDefault()
            If discoverylist.Children.Count > 0 Then
                For Each PhasesProperty As IPublishedContent In discoverylist.Children
                    Dim uCampaign As RatingComments = New RatingComments()
                    If PhasesProperty.HasValue(nodeProperties.message) Then uCampaign.ReviewDetail = PhasesProperty.GetPropertyValue(Of String)(nodeProperties.message)
                    uCampaign.Rating = PhasesProperty.GetPropertyValue(Of String)(nodeProperties.stars)
                    uCampaign.RatingDate = PhasesProperty.CreateDate.ToString("MMM d, yyyy")
                    uCampaign.NavigateUrl = campaignNode.Url
                    lstRatingComments.Add(uCampaign)
                Next
            End If

            If lstRatingComments.Count > 0 Then
                lvRatingComments.DataSource = lstRatingComments.OrderByDescending(Function(x) CDate(x.RatingDate)).ToList()
                lvRatingComments.DataBind()

                'gv.DataSource = lstRatingComments.OrderByDescending(Function(x) CDate(x.RatingDate)).ToList()
                'gv.DataBind()
            End If
            'End If
            'End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\PhaseEntry.ascx.vb : GetCampaignRating()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
    End Sub
#End Region
End Class