Imports Common
Imports umbraco
Imports umbraco.NodeFactory
Imports System.Net
Imports System.Net.Mail
Imports umbraco.Web
Imports umbraco.Core
Imports umbraco.Core.Models
Imports umbraco.Core.Services
Imports umbraco.interfaces

Partial Class Masterpages_Campaign_Page
    Inherits System.Web.UI.MasterPage


#Region "Properties"
    Private blPledges As blPledges
    Private blCampaigns As blCampaigns

    Dim _uHelper As Uhelper = New Uhelper()
    Private Enum endorsementViews
        vCredentials = 0
        vQuestionnair = 1
        vThankyou = 2
    End Enum

    Private _nodeId As Integer
    Public Property NodeId() As Integer
        Get
            Return _nodeId
        End Get
        Set(ByVal value As Integer)
            _nodeId = value
        End Set
    End Property
    Private Property ipCurrentPg As IPublishedContent
#End Region

#Region "Handles"
    Private Sub Masterpages_Campaign_Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        'Determine if AddThis should be displayed or not
        phAddThis.Visible = CBool(ConfigurationManager.AppSettings(Miscellaneous.isLive))

        'If Not IsPostBack Then
        'Instantiate variables
        'Dim thisNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(UmbracoContext.Current.PageId)
        ipCurrentPg = _uHelper.Get_IPublishedContentByID(UmbracoContext.Current.PageId)
        Dim blPhases As blPhases = New blPhases
        NodeId = ipCurrentPg.Id

        ''Add IPublishedContent id to user control
        'ucPledgeListPnl.NodeId = NodeId
        If ipCurrentPg.HasValue(nodeProperties.published) Then
            'Select if the "Preview mode" ribbon is to be displayed.
            pnlPreviewMode.Visible = Not ipCurrentPg.GetPropertyValue(Of Boolean)(nodeProperties.published)

            'If page is not published
            If ipCurrentPg.GetPropertyValue(Of Boolean)(nodeProperties.published) = False Then
                'Set a hidden value indicating that the page is in preview mode
                hfldPreviewMode.Value = True
                pnlPreviewMode.Visible = True
            Else
                hfldPreviewMode.Value = True
                pnlPreviewMode.Visible = False

            End If
        End If

        'Obtain data from umbraco.
        ltrlCampaignTitle.Text = ipCurrentPg.Name
        hlnkTeamName.Text = ipCurrentPg.Parent.Name
        hlnkTeamName.NavigateUrl = ipCurrentPg.Parent.Url
        hlnkTeamPage.NavigateUrl = ipCurrentPg.Parent.Url
        If ipCurrentPg.HasValue(nodeProperties.briefSummary) Then
            ltrlBriefSummary.Text = ipCurrentPg.GetPropertyValue(Of String)(nodeProperties.briefSummary)
            ltrlBriefSummary_Mbl.Text = ltrlBriefSummary.Text
        End If
        'If thisNode.HasValue(nodeProperties.fullSummary) Then
        '    ltrlFullSummary.Text = thisNode.GetPropertyValue(Of String)(nodeProperties.fullSummary)
        'End If
        hlnkFundThisCampaign.NavigateUrl = _uHelper.Get_IPublishedContentByID(siteNodes.Checkout).Url & "?nodeId=" & HttpUtility.UrlEncode(NodeId)
        hlnkFundThisCampaign_Mbl.NavigateUrl = hlnkFundThisCampaign.NavigateUrl
        'Add custom css
        If ipCurrentPg.HasValue(nodeProperties.customCSS) Then
            Dim sb As New StringBuilder
            sb.AppendLine("<style>")
            sb.AppendLine(ipCurrentPg.GetPropertyValue(Of String)(nodeProperties.customCSS))
            sb.AppendLine("</style>")
            ltrlCustomCss.Text = sb.ToString
        End If

        'Obtain images from umbraco media
        If ipCurrentPg.HasValue(nodeProperties.summaryPanelImage) Then
            imgBanner.ImageUrl = getMediaURL(ipCurrentPg.GetPropertyValue(Of String)(nodeProperties.topBannerImage), Crops.campaignFeaturedImage)
            imgBanner.AlternateText = ipCurrentPg.Name & " - " & getMediaName(ipCurrentPg.GetPropertyValue(Of String)(nodeProperties.summaryPanelImage))
            imgBanner.Attributes.Add("title", ipCurrentPg.Name)
        End If

        'Obtain Social Medias
        If ipCurrentPg.HasValue(nodeProperties.facebookUrl) Then
            hlnkFacebook.NavigateUrl = ipCurrentPg.GetPropertyValue(Of String)(nodeProperties.facebookUrl)
            hlnkFacebook.Visible = True
            'Else
            '    hlnkFacebook.CssClass = "inactive"
        End If
        If ipCurrentPg.HasValue(nodeProperties.twitterUrl) Then
            hlnkTwitter.NavigateUrl = ipCurrentPg.GetPropertyValue(Of String)(nodeProperties.twitterUrl)
            hlnkTwitter.Visible = True
            'Else
            '    hlnkTwitter.CssClass = "inactive"
        End If
        If ipCurrentPg.HasValue(nodeProperties.linkedInUrl) Then
            hlnkLinkedIn.NavigateUrl = ipCurrentPg.GetPropertyValue(Of String)(nodeProperties.linkedInUrl)
            hlnkLinkedIn.Visible = True
            'Else
            '    hlnkLinkedIn.CssClass = "inactive"
        End If
        If ipCurrentPg.HasValue(nodeProperties.supportEmail) Then
            hlnkSupportEmail.NavigateUrl = "mailto:" & ipCurrentPg.GetPropertyValue(Of String)(nodeProperties.supportEmail)
            hlnkSupportEmail.Visible = True
            'Else
            '    hlnkLinkedIn.CssClass = "inactive"
        End If

        'Obtain all phases
        obtainPhases(ipCurrentPg)

        'Ensure a discovery phase exists.  If not, create one.
        If Not blPhases.doesDiscoveryPhaseExist(NodeId) Then blPhases.createDiscoveryPhase(NodeId, True)

        'Check IsDiscovery phase IsActive or InActive
        For Each DiscoveryPhases As IPublishedContent In ipCurrentPg.Children
            If DiscoveryPhases.DocumentTypeAlias = docTypes.discovery Then
                If DiscoveryPhases.HasValue("phaseActive") And DiscoveryPhases.Properties.Any(Function(x) (x.PropertyTypeAlias = "phaseActive" AndAlso x.Value = "1")) Then
                    hlnkFundThisCampaign.Visible = False
                    pnlRating.Visible = True
                    divReward.Visible = False
                Else
                    hlnkFundThisCampaign.Visible = True
                    pnlRating.Visible = False
                    divReward.Visible = True
                End If
                Exit For
            End If
        Next

        If ipCurrentPg.GetPropertyValue(Of Boolean)(nodeProperties.campaignComplete) = True Then
            pnlCompletedCampaign.Visible = True
        End If

        'Hide the fund button if a stripe account does not exist.
        blCampaigns = New blCampaigns
        If String.IsNullOrEmpty(blCampaigns.selectStripeUserId_byCampaignId(ipCurrentPg.Id)) Then
            hlnkFundThisCampaign.Visible = False
            hlnkFundThisCampaign_Mbl.Visible = False
        ElseIf blPhases.anyActivePhases_byCampaignId(ipCurrentPg.Id) Then
            hlnkFundThisCampaign.Visible = True
            hlnkFundThisCampaign_Mbl.Visible = True
        Else
            hlnkFundThisCampaign.Visible = False
            hlnkFundThisCampaign_Mbl.Visible = False
        End If

    End Sub
    Private Sub Masterpages_Campaign_Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        'Instantiate variables
        Dim campaignGoal As Decimal = 0
        Dim campaignPledges As Decimal = 0
        Dim campaignContributors As UInt16 = 0

        'Obtain data from phases
        For Each control As Object In pnlPhases.Controls
            If control.GetType.ToString = "ASP.usercontrols_campaignphase_ascx" Then
                Dim thisControl = CType(control, ASP.usercontrols_campaignphase_ascx)
                campaignGoal += thisControl.phaseGoal
                campaignPledges += thisControl.phasePledged
                campaignContributors += thisControl.pledgeCount
            End If
        Next

        'Pass data to campaign summary
        ucCampaignSummary.campaignGoal = campaignGoal
        ucCampaignSummary.campaignPledges = campaignPledges
        ucCampaignSummary.campaignContributors = campaignContributors

        'Display percentage for pie chart.
        If campaignGoal > 0 Then
            Dim percentFunded As String() = ((campaignPledges / campaignGoal) * 100).ToString.Split(".")
            hfldPercentFunded.Value = percentFunded(0)
        End If


        ''Determine if content is full screen or not
        'If ucRewardList.isVisible = False Or divReward.Visible = False Then
        '    phFullWidthContent.Visible = True
        '    phPartialWidthContent.Visible = False

        '    If ipCurrentPg.HasValue(nodeProperties.fullSummary) Then
        '        ltrlFullSummaryFW.Text = ipCurrentPg.GetPropertyValue(Of String)(nodeProperties.fullSummary)
        '    End If

        '    'Add IPublishedContent id to user control
        '    ucPledgeListPnlFW.NodeId = NodeId
        'Else
        '    phFullWidthContent.Visible = False
        '    phPartialWidthContent.Visible = True

        '    If ipCurrentPg.HasValue(nodeProperties.fullSummary) Then
        '        ltrlFullSummary.Text = ipCurrentPg.GetPropertyValue(Of String)(nodeProperties.fullSummary)
        '    End If

        '    'Add IPublishedContent id to user control
        '    ucPledgeListPnl.NodeId = NodeId
        'End If


        If ipCurrentPg.HasValue(nodeProperties.fullSummary) Then
            ltrlFullSummary.Text = ipCurrentPg.GetPropertyValue(Of String)(nodeProperties.fullSummary)
        End If

        'Add IPublishedContent id to user control
        ucPledgeListPnl.NodeId = NodeId

    End Sub
    Private Sub btnReportCampaign_Click(sender As Object, e As EventArgs) Handles btnReportCampaign.Click
        Try
            'Instantiate variables
            Dim thisNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(UmbracoContext.Current.PageId)
            Dim blEmails As New blEmails

            ucAlertMsg.Visible = True
            ucAlertMsg.AlertMsg = "This campaign has been reported."
            ucAlertMsg.AdditionalText = "A site administrator will look into this site for inapropriate or suspicious activity."
            ucAlertMsg.MessageType = UserControls_AlertMsg.msgType.Warning

            'Send an email and submit to error message log
            Dim sb As New StringBuilder
            sb.AppendLine("The following campaign has been flagged.<hr />")
            sb.AppendLine("Campaign: " & thisNode.Name)
            sb.AppendLine("<br />Team: " & thisNode.Parent.Name)
            sb.AppendLine("<br />Url: " & HttpContext.Current.Request.Url.AbsoluteUri)

            saveErrorMessage(getLoggedInMember, sb.ToString, sb.ToString())
            blEmails.sendReportSuspiciousActivityEmail(sb.ToString)

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("Campaign_Page.master.vb : ReportCampaign()")
            sb.AppendLine("The following campaign has been flagged.<hr />")
            sb.AppendLine("Url: " & HttpContext.Current.Request.Url.AbsoluteUri)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
    End Sub
#End Region

#Region "Methods"
    Private Sub obtainPhases(ByRef thisNode As IPublishedContent)
        Try
            'Loop thru child nodes to find the Phases folder.
            For Each Phases As IPublishedContent In thisNode.Children
                If Phases.DocumentTypeAlias = docTypes.Phases Then
                    'Loop thru all of the phase records
                    For Each phase As IPublishedContent In Phases.Children
                        If phase.GetPropertyValue(Of Boolean)(nodeProperties.published) Then
                            'Create a new phase control
                            'Add the nodeId of the phase into the uc
                            Dim ucPhase As ASP.usercontrols_campaignphase_ascx = New ASP.usercontrols_campaignphase_ascx With {
                                .NodeId = phase.Id,
                                .phaseId = phase.GetPropertyValue(Of Integer)(nodeProperties.phaseNumber)
                            }
                            'ucPhase.forceInactive = endorsementsRequired
                            'Add uc to panel
                            pnlPhases.Controls.Add(ucPhase)
                        End If
                    Next

                    Exit For 'Exit the loop.
                End If
            Next

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\Masterpages\Campaign_Page.master.vb : obtainPhases()")
            sb.AppendLine("thisNode:" & thisNode.ToString())

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Response.Write("Error: " & ex.ToString)
        End Try
    End Sub
    Public Sub DisplayValidationErrors(ByVal returnObject As BusinessReturn, Optional ByVal ValidationGroup As String = "")
        'Display all validation errors
        For I = 0 To (returnObject.ValidationMessages.Count - 1)
            AddValidationMsg(returnObject.ValidationMessages(I).ErrorMessage, ValidationGroup)
        Next

        'Display any exception messages
        If (returnObject.ExceptionMessage <> String.Empty) Then
            AddValidationMsg(returnObject.ExceptionMessage, ValidationGroup)
        End If
    End Sub
    Public Sub AddValidationMsg(ByVal msg As String, Optional ByVal ValidationGroup As String = "")
        'Create custom validation
        Dim cv As New CustomValidator With {
            .IsValid = False,
            .ErrorMessage = msg
        }

        'If a validation group is provided, show only in that validation group
        If Not String.IsNullOrEmpty(ValidationGroup) Then
            cv.ValidationGroup = ValidationGroup
        End If

        'Add msg to summary
        Me.Page.Validators.Add(cv)
    End Sub
#End Region

End Class







'Protected Sub lbtnFundThisCampaign_Click(sender As Object, e As EventArgs)
'    Response.Redirect(New IPublishedContent(siteNodes.Checkout).NiceUrl & "?nodeId=" & HttpUtility.UrlEncode(NodeId))
'End Sub





'Private Sub btnEndorsement_PrevPg(sender As Object, e As EventArgs) Handles lbtnPrevPg.Click
'    'Go to previous page.
'    mvEndorsement.ActiveViewIndex = endorsementViews.vCredentials
'End Sub
'Private Sub btnEndorsement_NextPg(sender As Object, e As EventArgs) Handles lbtnQuestionnair.Click
'    'Instantiate variables
'    blEndorsement = New blEndorsements
'    Dim returnObject As BusinessReturn

'    'Check if email address is valid and if user had previously endorsed campaign
'    If Not blEndorsement.IsValidEmail(txbEmail.Text.Trim.ToLower) Then
'        'Invalid email address
'        returnObject = New BusinessReturn()
'        returnObject.ExceptionMessage = "Please enter a valid email address."
'        DisplayValidationErrors(returnObject, validationSummary_Endorsement01.ValidationGroup)
'    ElseIf blEndorsement.doesEndorserExist(txbEmail.Text.Trim.ToLower, IPublishedContent.getCurrentNodeId) Then
'        'Person already endorsed this campaign.
'        returnObject = New BusinessReturn()
'        returnObject.ExceptionMessage = "Thank you for previously endorsing this campaign."
'        DisplayValidationErrors(returnObject, validationSummary_Endorsement01.ValidationGroup)
'    Else
'        'Go to next page.
'        mvEndorsement.ActiveViewIndex = endorsementViews.vQuestionnair
'    End If
'End Sub
'Private Sub btnEndorsement_Submit(sender As Object, e As EventArgs) Handles lbtnSubmit.Click
'    'Instantiate variables
'    blEndorsement = New blEndorsements
'    Dim returnObject As New BusinessReturn()

'    Try
'        'Submit endorsement
'        returnObject = blEndorsement.Insert(UppercaseFirstLetter(txbFirstName.Text.Trim), txbEmail.Text.Trim.ToLower, IPublishedContent.getCurrentNodeId)

'        If (returnObject.isValid) Then
'            'Update entry in umbraco if enough endorsements are present
'            If blEndorsement.selectCount_byCampaignId(IPublishedContent.getCurrentNodeId) >= siteValues.endorsementsNeeded Then
'                'Instantiate variables
'                Dim thisNode As IPublishedContent = IPublishedContent.GetCurrent

'                'Assign properties
'                endorsementsRequired = False
'                thisNode.SetProperty(nodeProperties.endorsementsRequired, False)
'                thisNode.SetProperty(nodeProperties.acceptSponsors, True)
'                thisNode.SetProperty(nodeProperties.initialLaunchDate, DateTime.Now)
'                thisNode.SetProperty(nodeProperties.completionDate, DateTime.Now.AddDays(siteValues.campaignLength))
'                thisNode.Publish(True)

'                'Send emails to all who endorsed campaign.
'                'sendEmail()

'                'Set session variable for user who submitted final endorsement.
'                Session(Sessions.endorsementsComplete) = True
'            End If

'            'Go to next page.
'            mvEndorsement.ActiveViewIndex = endorsementViews.vThankyou
'            'AddValidationMsg("Insert Successful", ValidationSummary1.ValidationGroup) 'Return results to screen
'        Else
'            'Display any validation errors
'            DisplayValidationErrors(returnObject, validationSummary_Endorsement02.ValidationGroup)
'        End If
'    Catch ex As Exception
'        AddValidationMsg(ex.ToString, validationSummary_Endorsement02.ValidationGroup)
'        Response.Write(ex.ToString)
'    End Try
'End Sub
'Private Sub btnEndorsement_Complete(sender As Object, e As EventArgs) Handles lbtnClose.Click
'    'Closes panel and refreshes page.
'    Response.Redirect(Request.RawUrl)
'End Sub

'Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
'    sendEmail()
'End Sub