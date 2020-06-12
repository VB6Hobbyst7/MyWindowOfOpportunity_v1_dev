Imports umbraco
Imports umbraco.NodeFactory
Imports Common
Imports umbraco.Core.Services
Imports umbraco.Core.Models
Imports umbraco.Core
Imports System.IO
Imports System.Net.Mail
Imports System.Net
Imports umbraco.Web
Imports System.Threading

Partial Class Masterpages_EditCampaign
    Inherits System.Web.UI.MasterPage

#Region "Properties"
    Private blMembers As blMembers
    Private blCampaigns As blCampaigns
    Private _uHelper As Uhelper = New Uhelper()

    Private Enum mviews
        vEditMode = 0
        vWizardMode = 1
    End Enum
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
    Private blPhases As blPhases = New blPhases
#End Region

#Region "Handles"

    Private Sub Masterpages_EditCampaign_Load(sender As Object, e As EventArgs) Handles Me.Load
        'If Not IsPostBack Then
        Try
            'Instantiate variables
            Dim thisNodeId As Integer

            'Save stripe acct id if provided in querystring
            If Request.QueryString(queryParameters.code) IsNot Nothing Then
                blCampaigns = New blCampaigns
                Dim br As BusinessReturn = blCampaigns.SaveStripeAcctIDs(CShort(Request.QueryString(queryParameters.state)), Request.QueryString(queryParameters.code))
            End If

            'Obtain the node id from the querystring
            If Request.QueryString(queryParameters.state) IsNot Nothing Then
                'Redirect to page with properly structured querystring
                Dim newUrl As String = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Path) & "?" & queryParameters.nodeId & "=" & Request.QueryString(queryParameters.state)

                Server.ClearError()
                Response.Redirect(newUrl, False)
                HttpContext.Current.Response.Flush() 'Sends all currently buffered output To the client.
                HttpContext.Current.Response.SuppressContent = True 'Gets Or sets a value indicating whether To send HTTP content To the client.
                HttpContext.Current.ApplicationInstance.CompleteRequest() 'Causes ASP.NET To bypass all events And filtering In the HTTP pipeline chain Of execution And directly execute the EndRequest Event.
                HttpContext.Current.Response.End()
            ElseIf Request.QueryString(queryParameters.nodeId) IsNot Nothing Then
                'Get campaign id from querystring
                thisNodeId = CInt(Request.QueryString(queryParameters.nodeId))
            Else
                'Redirect to home page
                Server.ClearError()
                Response.Redirect(_uHelper.Get_IPublishedContentByID(siteNodes.Login).Url, False)
                HttpContext.Current.Response.Flush() 'Sends all currently buffered output To the client.
                HttpContext.Current.Response.SuppressContent = True 'Gets Or sets a value indicating whether To send HTTP content To the client.
                HttpContext.Current.ApplicationInstance.CompleteRequest() 'Causes ASP.NET To bypass all events And filtering In the HTTP pipeline chain Of execution And directly execute the EndRequest Event.
                HttpContext.Current.Response.End()
            End If

            'Instantiate campaign node
            Dim thisNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(thisNodeId)
            If Not IsNothing(thisNode) Then
                'Dim BusinessReturn As BusinessReturn
                'Dim CampaignStatistics As CampaignStatistics
                blMembers = New blMembers
                Dim currentView As mviews = mviews.vEditMode
                Dim loginStatus As Web.Models.LoginStatusModel

                'Is current user logged in?
                loginStatus = blMembers.getCurrentLoginStatus()
                If Not loginStatus.IsLoggedIn Then
                    'Save current page and go to login page
                    Session(Sessions.previousPage) = HttpContext.Current.Request.Url.AbsoluteUri
                    Response.Redirect(_uHelper.Get_IPublishedContentByID(siteNodes.Login).Url, False)
                    Context.ApplicationInstance.CompleteRequest()
                Else
                    'Save current loggedin id to hfld
                    hfldCurrentUserId.Value = blMembers.GetCurrentMemberId()

                    ''Is user a member in this campaign?
                    'If ("" = "") Then
                    'Else
                    'End If
                End If

                'Determine if the wizard should be shone or not.
                Try
                    If Not String.IsNullOrEmpty(Request.QueryString(queryParameters.stepByStep)) Then
                        If CBool(Request.QueryString(queryParameters.stepByStep)) = True Then
                            currentView = mviews.vWizardMode
                        End If
                    End If
                Catch ex As Exception
                    Dim sb As New StringBuilder()
                    sb.AppendLine("\Masterpages\EditAccount.master.vb : Masterpages_EditCampaign_Load()")
                    saveErrorMessage(getLoggedInMember, ex.Message & vbNewLine & ex.StackTrace & vbNewLine & ex.Source, sb.ToString())
                End Try

                'Set the multiviews active view
                mvEditViews.ActiveViewIndex = currentView

                '
                Select Case currentView
                    Case mviews.vEditMode
                        ''Send campaign node to all user controls
                        uceditTab_Content.thisNodeId = thisNode.Id
                        uceditTab_Categories.thisNodeId = thisNode.Id
                        uceditTab_Phases.thisNodeId = thisNode.Id
                        uceditTab_Timeline.thisNodeId = thisNode.Id
                        uceditTab_Images.thisNodeId = thisNode.Id
                        uceditTab_Rewards.thisNodeId = thisNode.Id
                        uceditTab_TeamMembers.thisNodeId = thisNode.Id
                        editTab_Pledges.CurrentNodeId = thisNode.Id

                        If IsPostBack Then
                            'Set what tab is being displayed in session for other tabs to use.
                            If IsNothing(hfldActiveTab.Value) Then
                                Session(Miscellaneous.ActiveTab) = Nothing
                            Else
                                Session(Miscellaneous.ActiveTab) = hfldActiveTab.Value
                            End If
                        End If

                        displayData(thisNodeId)

                    Case mviews.vWizardMode
                        'Send campaign node to all user controls
                        uceditTab_Content_Wiz.thisNodeId = thisNode.Id
                        uceditTab_Categories_Wiz.thisNodeId = thisNode.Id
                        uceditTab_Images_Wiz.thisNodeId = thisNode.Id
                        uceditTab_Phases_Wiz.thisNodeId = thisNode.Id
                        uceditTab_TeamMembers_Wiz.thisNodeId = thisNode.Id
                        uceditTab_Rewards_Wiz.thisNodeId = thisNode.Id
                        uceditTab_Timeline_Wiz.thisNodeId = thisNode.Id

                        'Campaign & Team Name
                        ltrlCampaignName_Wiz.Text = thisNode.Name
                        hlnkTeamName_Wiz.Text = thisNode.Parent.Name
                        hlnkTeamName_Wiz.NavigateUrl = thisNode.Parent.Url

                        'Create link to exit wizard
                        hlnkExitWizard.NavigateUrl = exitWizardUrl()

                        'Add the proper content for the beginning and end of the wizard.
                        ltrlIntroduction.Text = _uHelper.Get_IPublishedContentByID(UmbracoContext.Current.PageId).GetPropertyValue(Of String)(nodeProperties.introductionContent)
                        ltrlCompletion.Text = _uHelper.Get_IPublishedContentByID(UmbracoContext.Current.PageId).GetPropertyValue(Of String)(nodeProperties.completionContent)
                End Select

                'Publish Visible Status controls
                Dim _currentMembersRole As String
                Dim _Campaginstatus As Boolean = False
                Dim _CampaginCompletionStatus As Boolean = False
                Dim _pledgeStatus As Integer = 0

                For Each ph In thisNode.Children.OfTypes(docTypes.Phases).ToList()
                    For Each phase In ph.Children.OfTypes(docTypes.Phase).ToList()
                        _pledgeStatus = phase.Children.OfTypes(docTypes.Pledges).Count()

                        If _pledgeStatus > 0 Then
                            Exit For
                        End If
                    Next
                Next

                If thisNode.HasProperty(nodeProperties.published) Then _Campaginstatus = thisNode.GetPropertyValue(Of String)(nodeProperties.published)
                If thisNode.HasProperty(nodeProperties.campaignComplete) Then _CampaginCompletionStatus = thisNode.GetPropertyValue(Of String)(nodeProperties.campaignComplete)
                _currentMembersRole = blMembers.GetCurrentMemberRole_byCampaignId(thisNode.Id)

                'Send completion status to all edit controls.
                uceditTab_Categories.campaignComplete = _CampaginCompletionStatus
                uceditTab_Content.campaignComplete = _CampaginCompletionStatus
                uceditTab_Images.campaignComplete = _CampaginCompletionStatus
                uceditTab_Phases.campaignComplete = _CampaginCompletionStatus
                uceditTab_Rewards.campaignComplete = _CampaginCompletionStatus
                uceditTab_TeamMembers.campaignComplete = _CampaginCompletionStatus
                uceditTab_Timeline.campaignComplete = _CampaginCompletionStatus

                'If _currentMembersRole = memberRole.CampaignAdministrator And _Campaginstatus = True And _pledgeStatus = 0 Then
                '    'lbtnPublish.Visible = False
                '    'lbtnUnPublish.Visible = True
                'End If
                If _Campaginstatus = True And _CampaginCompletionStatus = True Or _CampaginCompletionStatus = True Or _Campaginstatus = True Then
                    'lbtnPublish.Visible = False
                    'lbtnPreview.Visible = False
                    'lnkViewCampaign.Visible = True
                    lnkViewCampaign.HRef = thisNode.Url
                End If

            End If

        Catch abortEx As ThreadAbortException
            'Do NOTHING!!!
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("EditCampaign.master.vb : Load()")
            sb.AppendLine(Request.Url.ToString())

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
    End Sub
    Private Sub Masterpages_EditCampaign_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        '
        Try
            If Not String.IsNullOrEmpty(Request.QueryString(queryParameters.stepByStep)) Then
                If CBool(Request.QueryString(queryParameters.stepByStep)) = True Then
                    'Instantiate variables
                    Dim wizardStepCount As UInt16 = mvWizardSteps.Views.Count - 1
                    Dim wizardStep As UInt16 = 0

                    'Set wizard step
                    If Not String.IsNullOrEmpty(Request.QueryString(queryParameters.wizardStep)) Then
                        If IsNumeric(Request.QueryString(queryParameters.wizardStep)) Then
                            wizardStep = CInt(Request.QueryString(queryParameters.wizardStep))
                        End If
                    End If
                    'If Not IsPostBack Then
                    '    Session(Sessions.wizartStep) = 0
                    'End If

                    'Set active wizard step
                    mvWizardSteps.ActiveViewIndex = wizardStep

                    'Reset all buttons
                    pnlStartBtn.Visible = False
                    pnlPrevious.Visible = False
                    pnlNext.Visible = False
                    pnlComplete.Visible = False

                    'Show proper buttons for the active step
                    If wizardStep = 0 Then
                        pnlStartBtn.Visible = True
                        phSteps.Visible = False
                    ElseIf wizardStep = wizardStepCount Then
                        pnlComplete.Visible = True
                        phSteps.Visible = False
                        pnlExitWizard.Visible = False
                    Else
                        pnlPrevious.Visible = True
                        pnlNext.Visible = True
                        phSteps.Visible = True
                        ltrlStep.Text = wizardStep
                        ltrlSteps.Text = wizardStepCount - 1
                    End If
                End If
            End If

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\Masterpages\EditAccount.master.vb : Masterpages_EditCampaign_Load()")
            saveErrorMessage(getLoggedInMember, ex.Message & vbNewLine & ex.StackTrace & vbNewLine & ex.Source, sb.ToString())
            'Response.Write("Error: " & ex.ToString)
        End Try
    End Sub
    Private Sub lbtnComplete_Click(sender As Object, e As EventArgs) Handles lbtnComplete.Click
        'Refresh page without wizard.
        Response.Redirect(exitWizardUrl(), False)
        HttpContext.Current.ApplicationInstance.CompleteRequest()
    End Sub
    Private Sub lbtnNext_Click(sender As Object, e As EventArgs) Handles lbtnNext.Click, lbtnStart.Click
        'Next step
        Response.Redirect(navigateToStep(1), False)
        HttpContext.Current.ApplicationInstance.CompleteRequest()
        'Session(Sessions.wizartStep) += 1
        'Response.Write(navigateToStep(1))
    End Sub
    Private Sub lbtnPrevious_Click(sender As Object, e As EventArgs) Handles lbtnPrevious.Click
        'Previous step
        Response.Redirect(navigateToStep(-1), False)
        HttpContext.Current.ApplicationInstance.CompleteRequest()
        'Session(Sessions.wizartStep) -= 1
        'Response.Write(navigateToStep(-1))
    End Sub

    Private Sub btnPublishHidden_Click(sender As Object, e As EventArgs) Handles btnPublishHidden.Click

        Dim ms As IContentService = ApplicationContext.Current.Services.ContentService
        Dim Node As IContent
        Dim thisNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(CInt(Request.QueryString(queryParameters.nodeId)))
        Dim LaunchDate As DateTime = DateTime.Now

        'Node validation > The following objects need to be verified to ensure that they are Not NULL Or empty in umbraco 

        Dim Category_Bool As Boolean = False
        Dim FeatureImage As Boolean = False
        Dim BriefSummary As Boolean = False
        Dim FullSummary As Boolean = False




        If thisNode.HasProperty(nodeProperties.summaryPanelImage) Then
            If thisNode.HasValue(nodeProperties.summaryPanelImage) Then
                FeatureImage = True
            End If
        End If
        If thisNode.HasProperty(nodeProperties.briefSummary) Then
            If thisNode.HasValue(nodeProperties.briefSummary) Then
                BriefSummary = True
            End If
        End If
        If thisNode.HasProperty(nodeProperties.fullSummary) Then
            If thisNode.HasValue(nodeProperties.fullSummary) Then
                FullSummary = True
            End If
        End If
        If thisNode.HasProperty(SubcategoryConverter.Artistic) Then
            If thisNode.HasValue(SubcategoryConverter.Artistic) Then
                Category_Bool = True
            End If
        End If
        If thisNode.HasProperty(SubcategoryConverter.Business) Then
            If thisNode.HasValue(SubcategoryConverter.Business) Then
                Category_Bool = True
            End If
        End If
        If thisNode.HasProperty(SubcategoryConverter.Charity) Then
            If thisNode.HasValue(SubcategoryConverter.Charity) Then
                Category_Bool = True
            End If
        End If
        If thisNode.HasProperty(SubcategoryConverter.Community) Then
            If thisNode.HasValue(SubcategoryConverter.Community) Then
                Category_Bool = True
            End If
        End If
        If thisNode.HasProperty(SubcategoryConverter.Science) Then
            If thisNode.HasValue(SubcategoryConverter.Science) Then
                Category_Bool = True
            End If
        End If
        If thisNode.HasProperty(SubcategoryConverter.SelfHelp) Then
            If thisNode.HasValue(SubcategoryConverter.SelfHelp) Then
                Category_Bool = True
            End If
        End If
        If thisNode.HasProperty(SubcategoryConverter.Software) Then
            If thisNode.HasValue(SubcategoryConverter.Software) Then
                Category_Bool = True
            End If
        End If
        If thisNode.HasProperty(SubcategoryConverter.Technology) Then
            If thisNode.HasValue(SubcategoryConverter.Technology) Then
                Category_Bool = True
            End If
        End If

        Try
            If Category_Bool = True And BriefSummary = True And FullSummary = True And FeatureImage = True Then

                Node = ms.GetById(CInt(Request.QueryString(queryParameters.nodeId)))

                ''Added this If condition for  testing.
                'If ltrlLaunchDate.Text = "TBD" Then
                '    LaunchDate = DateTime.Now.ToShortDateString()
                'Else
                '    LaunchDate = Convert.ToDateTime(ltrlLaunchDate.Text).ToShortDateString()
                'End If

                Node.SetValue(nodeProperties.published, True)
                Node.SetValue(nodeProperties.datePublished, LaunchDate)

                ms.SaveAndPublishWithStatus(Node)

                Response.Redirect(Request.Url.PathAndQuery, False)
                HttpContext.Current.ApplicationInstance.CompleteRequest()
            Else

                Dim Ul As New HtmlGenericControl("ul")
                Dim Li As HtmlGenericControl

                If Category_Bool = False Then
                    Li = New HtmlGenericControl("li") With {
                        .InnerText = "Select at least one category from the CATEGORIES tab."
                    }
                    Ul.Controls.Add(Li)
                End If

                If FeatureImage = False Then
                    Li = New HtmlGenericControl("li") With {
                        .InnerText = "Select a featured image in the CONTENT tab."
                    }
                    Ul.Controls.Add(Li)
                End If

                If BriefSummary = False Then
                    Li = New HtmlGenericControl("li") With {
                        .InnerText = "Add a brief summary in the CONTENT tab."
                    }
                    Ul.Controls.Add(Li)
                End If

                If FullSummary = False Then
                    Li = New HtmlGenericControl("li") With {
                        .InnerText = "Add your campaign's full summary in the CONTENT tab."
                    }

                    Ul.Controls.Add(Li)
                End If

                Dim sb As New StringBuilder()
                Dim htw As New HtmlTextWriter(New System.IO.StringWriter(sb))
                Ul.RenderControl(htw)
                MainucAlertMsg_Error.AdditionalText = sb.ToString()
                pnlAlertPublishMsg.Visible = True

            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\Masterpages\EditAccount.master.vb : btnPublishHidden_Click()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try

    End Sub

    Private Sub btnLaunchDiscoveryPhase_Click(sender As Object, e As EventArgs) Handles btnLaunchDiscoveryPhase.Click
        Try
            'Instantiate variables
            Dim campaignId As Integer = CInt(Request.QueryString(queryParameters.nodeId))
            Dim businessReturn As BusinessReturn

            businessReturn = blPhases.createDiscoveryPhase(campaignId)

            MainucAlertMsg_Error.MessageType = UserControls_AlertMsg.msgType.Success
            MainucAlertMsg_Error.AlertMsg = "Discovery Phase created successfully"

            'Update data on screen
            displayData(campaignId)
        Catch ex As Exception

        End Try
    End Sub
    Private Sub btnLaunchPhase1_Click(sender As Object, e As EventArgs) Handles btnLaunchPhase1.Click
        Dim thisNodeId As Integer = CInt(Request.QueryString(queryParameters.nodeId))
        '
        blPhases.DeactivateDiscoveryPhase(thisNodeId, Now().ToString() & " - Deactivated due to Phase 1 being activated.")

        'launch phase 1
        blPhases.UpdatePhase(thisNodeId, 1)

        'Update data on screen
        displayData(thisNodeId)
    End Sub
    Private Sub btnLaunchPhase2_Click(sender As Object, e As EventArgs) Handles btnLaunchPhase2.Click
        'Dim thisNode As Node = New Node(CInt(Request.QueryString(queryParameters.nodeId)))
        Dim thisNodeId As Integer = CInt(Request.QueryString(queryParameters.nodeId))
        blPhases.UpdatePhase(thisNodeId, 2)
        'Update data on screen
        displayData(thisNodeId)
    End Sub
    Private Sub btnLaunchPhase3_Click(sender As Object, e As EventArgs) Handles btnLaunchPhase3.Click
        'Dim thisNode As Node = New Node(CInt(Request.QueryString(queryParameters.nodeId)))
        Dim thisNodeId As Integer = CInt(Request.QueryString(queryParameters.nodeId))
        blPhases.UpdatePhase(thisNodeId, 3)
        'Update data on screen
        displayData(thisNodeId)
    End Sub
#End Region

#Region "Methods"
    Private Function exitWizardUrl() As String
        'Instantiate variables
        Dim querystringCollection As New NameValueCollection

        'Obtain querystring if present
        If Request.QueryString.HasKeys() Then
            querystringCollection = HttpUtility.ParseQueryString(Request.QueryString.ToString)
        End If

        'Remove parameter from querystring
        If querystringCollection.AllKeys.Contains(queryParameters.stepByStep) Then
            querystringCollection.Remove(queryParameters.stepByStep)
        End If
        If querystringCollection.AllKeys.Contains(queryParameters.wizardStep) Then
            querystringCollection.Remove(queryParameters.wizardStep)
        End If

        'Refresh page without wizard.
        Return Request.Url.AbsolutePath & "?" & querystringCollection.ToString()
    End Function
    Private Function navigateToStep(ByVal _step As Int16) As String
        'Instantiate variables
        Dim querystringCollection As New NameValueCollection

        'Obtain querystrings if present
        If Request.QueryString.HasKeys() Then
            querystringCollection = HttpUtility.ParseQueryString(Request.QueryString.ToString)
        End If

        'Remove parameter from querystring
        If querystringCollection.AllKeys.Contains(queryParameters.wizardStep) Then
            'edit parameter
            querystringCollection(queryParameters.wizardStep) = CInt(querystringCollection(queryParameters.wizardStep)) + _step
            'Response.Write("Edit wizardStep: " & querystringCollection(queryParameters.wizardStep) & "<br />")
        Else
            'add parameter
            querystringCollection.Add(queryParameters.wizardStep, "1")
            'Response.Write("Add wizardStep: " & querystringCollection(queryParameters.wizardStep) & "<br />")
        End If

        'Refresh page without wizard.
        Return Request.Url.AbsolutePath & "?" & querystringCollection.ToString()
    End Function

    Private Sub displayData(ByVal thisNodeId As Integer)

        'OBTAIN DATA FROM UMBRACO
        '==========================================
        'Instantiate variables
        Dim thisNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(thisNodeId)
        Dim BusinessReturn As BusinessReturn
        Dim CampaignStatistics As CampaignStatistics
        blCampaigns = New blCampaigns

        'Campaign & Team Name
        ltrlCampaignName.Text = thisNode.Name
        hlnkTeamName.Text = thisNode.Parent.Name
        hlnkTeamName.NavigateUrl = thisNode.Parent.Url

        '
        BusinessReturn = blCampaigns.obtainStatistics(thisNode, True)
        If BusinessReturn.isValid Then
            'Obtain campaign data
            CampaignStatistics = DirectCast(BusinessReturn.DataContainer(0), CampaignStatistics)


            '===================================================
            '===================================================
            ''TEMP..USED To SEE DATA In GRID
            'Dim lstCampaigns As New List(Of CampaignStatistics)
            'lstCampaigns.Add(CampaignStatistics)
            'Dim lstDiscPhase As New List(Of DiscoveryPhase)
            'lstDiscPhase.Add(CampaignStatistics.DiscoveryPhase)

            'gv1.DataSource = lstCampaigns
            'gv1.DataBind()

            'gv2.DataSource = CampaignStatistics.lstPhases
            'gv2.DataBind()

            'gv3.DataSource = lstDiscPhase
            'gv3.DataBind()

            'gv4.DataSource = CampaignStatistics.lstPhases(0).lstPledges
            'gv4.DataBind()

            'gv5.DataSource = CampaignStatistics.lstPhases(1).lstPreviousPhases
            'gv5.DataBind()
            '===================================================
            '===================================================


            'Hide all buttons and clear notes
            phPreview.Visible = False
            phCreateStripeAcct.Visible = False
            phPublishCampaign.Visible = False
            phLaunchDiscoveryPhase.Visible = False
            phLaunchPhase1.Visible = False
            phLaunchPhase2.Visible = False
            phLaunchPhase3.Visible = False
            phViewCampaign.Visible = False
            'phUnpublish.Visible = False
            lblPhaseStatusNotes.Text = String.Empty


            '==================================================================
            '   SHOW CAMPAIGN STATUS
            '==================================================================

            'Campaign goal
            Dim campaignGoal As String() = CampaignStatistics.campaignGoal.ToString.Split(".")
            ltrlCampaignGoal_Dollars.Text = String.Format("{0:c0}", CInt(campaignGoal(0)))
            If campaignGoal.Count > 1 Then ltrlCampaignGoal_Cents.Text = campaignGoal(1) Else ltrlCampaignGoal_Cents.Text = "00"

            'Campaign Pledges
            Dim currentPledges As String() = CampaignStatistics.currentPledges.ToString.Split(".")
            ltrlCurrentlyPledged_Dollars.Text = String.Format("{0:c0}", CInt(currentPledges(0)))
            If currentPledges.Count > 1 Then ltrlCurrentlyPledged_Cents.Text = currentPledges(1) Else ltrlCurrentlyPledged_Cents.Text = "00"

            'Determine % of campaign's goal is met.
            If CampaignStatistics.funded > 0 Then
                'ucProgressBar_TotalPledges.percentage = CInt((CampaignStatistics.currentPledges / CampaignStatistics.campaignGoal) * 100)
                ucProgressBar_TotalPledges.percentage = CampaignStatistics.funded
            End If

            'Display date published
            lblPublishDate.Text = CampaignStatistics.publishDate.ToString("MMMM dd, yyyy")
            '==================================================================

            'Display data depending on campaign's status
            displayData_byCampaignStatus(CampaignStatistics)

            Dim campaignStatus = thisNode.GetPropertyValue(Of String)(nodeProperties.published)

        Else
            'Response.Write("error: " & BusinessReturn.ExceptionMessage)
        End If

        'Set preview link
        lbtnPreview.NavigateUrl = thisNode.Url + "?previewer=desktop"
    End Sub
    Private Function createDataEntry(ByVal fieldTxt As String, ByVal dataTxt As String) As HtmlGenericControl  '<li> <strong>Field: </strong>    <span class="italic">Data</span>    </li>
        'Instantiate variables
        Dim li As New HtmlGenericControl("li")
        Dim field As New Label
        Dim data As New Label

        'Add controls in proper order to parent control
        li.Controls.Add(field)
        li.Controls.Add(data)

        field.CssClass = "bold"
        data.CssClass = "italic"

        field.Text = fieldTxt
        If Not String.IsNullOrEmpty(dataTxt) Then field.Text += ": "
        data.Text = dataTxt

        Return li
    End Function

    Private Sub displayData_byCampaignStatus(ByRef CampaignStatistics As CampaignStatistics)
        'Show buttons and data depending on status
        Select Case CampaignStatistics.statusType
            Case statusType.AccountSetup
                'Unpublished
                displayDataByCampaignStatus_AccountSetup(CampaignStatistics)

            Case statusType.Unpublished
                'Unpublished
                displayDataByCampaignStatus_Unpublished(CampaignStatistics)

            Case statusType.DiscoveryPhase
                'Discovery Active
                displayDataByCampaignStatus_DiscoveryActive(CampaignStatistics)

            Case statusType.Phase1Pending
                'Phase 1 Pending
                displayDataByCampaignStatus_PendingPhase1(CampaignStatistics)

            Case statusType.Phase1Active
                'Phase 1 Active
                displayDataByCampaignStatus_Phase1Active(CampaignStatistics)

            Case statusType.Phase2Pending
                'Phase 2 Pending
                displayDataByCampaignStatus_PendingPhase2(CampaignStatistics)

            Case statusType.Phase2Active
                'Phase 2 Active
                displayDataByCampaignStatus_Phase2Active(CampaignStatistics)

            Case statusType.Phase3Pending
                'Phase 3 Pending
                displayDataByCampaignStatus_PendingPhase3(CampaignStatistics)

            Case statusType.Phase3Active
                'Phase 3 Active
                displayDataByCampaignStatus_Phase3Active(CampaignStatistics)

            Case statusType.Complete
                'Complete
                displayDataByCampaignStatus_Complete(CampaignStatistics)
        End Select
    End Sub

    Private Sub displayDataByCampaignStatus_AccountSetup(ByRef CampaignStatistics As CampaignStatistics)
        'Display panels and text
        phPreview.Visible = True
        phCreateStripeAcct.Visible = True
        pnlPublishedDate.Visible = False
        lblPhaseStatus.Text = statusTypeValues.Unpublished

        'Add notes from library.
        lblPhaseStatusNotes.Text = umbraco.library.GetDictionaryItem(dictionaryEntry.createStripeAcct)

        'Build stripe acct creation url
        Dim sbStripeUrl As New StringBuilder
        sbStripeUrl.Append(Miscellaneous.StripeUrl)
        sbStripeUrl.Append("?redirect_uri=" & Request.Url.GetLeftPart(UriPartial.Path))
        sbStripeUrl.Append("&client_id=" & ConfigurationManager.AppSettings(Miscellaneous.StripeConnectId))
        sbStripeUrl.Append("&state=" & CampaignStatistics.campaignId) 'Request.QueryString(queryParameters.nodeId))
        sbStripeUrl.Append("&scope=read_write")

        hlnkCreateStripeAcct.NavigateUrl = sbStripeUrl.ToString

    End Sub
    Private Sub displayDataByCampaignStatus_Unpublished(ByRef CampaignStatistics As CampaignStatistics)
        'Display panels and text
        phPreview.Visible = True
        phPublishCampaign.Visible = True
        pnlPublishedDate.Visible = False
        lblPhaseStatus.Text = statusTypeValues.Unpublished

        'Add notes from library.
        lblPhaseStatusNotes.Text = umbraco.library.GetDictionaryItem(dictionaryEntry.unpublishedNotes)
    End Sub
    Private Sub displayDataByCampaignStatus_DiscoveryActive(ByRef CampaignStatistics As CampaignStatistics)
        'Display panels and text
        phViewCampaign.Visible = True
        phLaunchPhase1.Visible = True
        phDiscoveryPhase.Visible = True
        phCurrentPledges.Visible = False
        lblPhaseStatus.Text = statusTypeValues.DiscoveryPhase

        'Days Remaining
        If Date.Today >= CampaignStatistics.DiscoveryPhase.completionDate Then
            phPhaseStatus.Controls.Add(createDataEntry("Days Remaining", 0))
        Else
            phPhaseStatus.Controls.Add(createDataEntry("Days Remaining", (CampaignStatistics.DiscoveryPhase.completionDate - Date.Today).Value.Days.ToString))
        End If

        'Phase start date
        If Not IsNothing(CampaignStatistics.DiscoveryPhase.activationDate) Then
            phPhaseStatus.Controls.Add(createDataEntry("Start Date", CampaignStatistics.DiscoveryPhase.activationDate.Value.ToString("MMMM dd, yyyy")))
        End If

        'Phase end date
        If Not IsNothing(CampaignStatistics.DiscoveryPhase.activationDate) Then
            phPhaseStatus.Controls.Add(createDataEntry("Completion Date", CampaignStatistics.DiscoveryPhase.completionDate.Value.ToString("MMMM dd, yyyy")))
        End If

        'Add notes from library.
        lblPhaseStatusNotes.Text = umbraco.library.GetDictionaryItem(dictionaryEntry.discoveryPhaseNotes)

        'Add campaign node id to user control
        ucRatingSummary.thisNodeId = getCampaignId(CampaignStatistics.campaignId)
    End Sub
    Private Sub displayDataByCampaignStatus_PendingPhase1(ByRef CampaignStatistics As CampaignStatistics)
        '
        blCampaigns = New blCampaigns
        Dim isCharity As Boolean = blCampaigns.IsCampaignACharity_byId(CampaignStatistics.campaignId)

        'Display panels and text
        phViewCampaign.Visible = True
        phLaunchDiscoveryPhase.Visible = Not isCharity
        phLaunchPhase1.Visible = True
        lblPhaseStatus.Text = statusTypeValues.Phase1Pending

        'Add notes from library.
        If isCharity Then
            lblPhaseStatusNotes.Text = umbraco.library.GetDictionaryItem(dictionaryEntry.pendingPhase)
        Else
            lblPhaseStatusNotes.Text = umbraco.library.GetDictionaryItem(dictionaryEntry.discoveryPhaseNotes)
        End If


        'Display previous phase data
        Select Case CampaignStatistics.previousStatusType
            Case statusType.DiscoveryPhase
                'Display discovery data
                discoveryPhase(CampaignStatistics.DiscoveryPhase)

            Case statusType.Phase1Failed
                'Obtain previous failed phase
                previousFailedPhase((From x In CampaignStatistics.lstPhases(0).lstPreviousPhases Where x.phaseNumber = 1 Select x).FirstOrDefault)

        End Select
    End Sub
    Private Sub displayDataByCampaignStatus_Phase1Active(ByRef CampaignStatistics As CampaignStatistics)
        phViewCampaign.Visible = True
        lblPhaseStatus.Text = statusTypeValues.Phase1Active

        'Display active phase data
        activePhaseStats(CampaignStatistics.lstPhases(0))
    End Sub
    Private Sub displayDataByCampaignStatus_PendingPhase2(ByRef CampaignStatistics As CampaignStatistics)
        phViewCampaign.Visible = True
        phLaunchPhase2.Visible = True
        lblPhaseStatus.Text = statusTypeValues.Phase2Pending

        'Display previous phase data
        Select Case CampaignStatistics.previousStatusType
            Case statusType.Phase1Succeeded
                'Obtain previous phase
                previousSuccessfulPhase((From x In CampaignStatistics.lstPhases Where x.phaseNumber = 1 Select x).FirstOrDefault)

            Case statusType.Phase2Failed
                ''Obtain previous phase
                'Dim previousPhase As Phase = (From x In CampaignStatistics.lstPhases(1).lstPreviousPhases Where x.phaseNumber = 1 Select x).FirstOrDefault

                'Obtain previous phase
                previousFailedPhase((From x In CampaignStatistics.lstPhases(1).lstPreviousPhases Where x.phaseNumber = 2 Select x).FirstOrDefault)
        End Select

        'Add notes from library.
        Dim tempDate As Date = Date.Today
        If Not IsNothing(CampaignStatistics.lstPhases(0).completionDate) Then tempDate = CampaignStatistics.lstPhases(0).completionDate
        lblPhaseStatusNotes.Text = umbraco.library.GetDictionaryItem(dictionaryEntry.phaseComplete).Replace("[DATE]", tempDate.ToLongDateString)

    End Sub
    Private Sub displayDataByCampaignStatus_Phase2Active(ByRef CampaignStatistics As CampaignStatistics)
        phViewCampaign.Visible = True
        lblPhaseStatus.Text = statusTypeValues.Phase2Active

        'Display active phase data
        activePhaseStats(CampaignStatistics.lstPhases(1))
    End Sub
    Private Sub displayDataByCampaignStatus_PendingPhase3(ByRef CampaignStatistics As CampaignStatistics)
        phViewCampaign.Visible = True
        phLaunchPhase3.Visible = True
        lblPhaseStatus.Text = statusTypeValues.Phase3Pending

        'Display previous phase data
        Select Case CampaignStatistics.previousStatusType
            Case statusType.Phase2Succeeded
                'Obtain previous phase
                previousSuccessfulPhase((From x In CampaignStatistics.lstPhases(1).lstPreviousPhases Where x.phaseNumber = 1 Select x).FirstOrDefault)

            Case statusType.Phase3Failed
                'Obtain previous phase
                previousFailedPhase((From x In CampaignStatistics.lstPhases(2).lstPreviousPhases Where x.phaseNumber = 3 Select x).FirstOrDefault)
        End Select

        'Add notes from library.
        Dim tempDate As Date = Date.Today
        If Not IsNothing(CampaignStatistics.lstPhases(0).completionDate) Then tempDate = CampaignStatistics.lstPhases(1).completionDate
        lblPhaseStatusNotes.Text = umbraco.library.GetDictionaryItem(dictionaryEntry.phaseComplete).Replace("[DATE]", tempDate.ToLongDateString)

    End Sub
    Private Sub displayDataByCampaignStatus_Phase3Active(ByRef CampaignStatistics As CampaignStatistics)
        phViewCampaign.Visible = True
        lblPhaseStatus.Text = statusTypeValues.Phase3Active

        'Display active phase data
        activePhaseStats(CampaignStatistics.lstPhases(2))
    End Sub

    Private Sub discoveryPhase(ByRef discoveryPhase As DiscoveryPhase)
        'Display previous data
        phPreviousPhase.Visible = True
        lblPreviousPhase.Text = "Discovery"
        'Response.Write("discoveryPhase()")

        'Previous Discovery Phase start date
        If Not IsNothing(discoveryPhase.activationDate) Then
            phPhaseStatus.Controls.Add(createDataEntry("Start Date", discoveryPhase.activationDate.Value.ToString("MMMM dd, yyyy")))
        End If

        'Previous Discovery Phase end date
        If Not IsNothing(discoveryPhase.activationDate) Then
            phPhaseStatus.Controls.Add(createDataEntry("Completion Date", discoveryPhase.completionDate.Value.ToString("MMMM dd, yyyy")))
        End If

        'Rating
        If Not IsNothing(discoveryPhase.rating) Then
            phPhaseStatus.Controls.Add(createDataEntry("Rating", discoveryPhase.rating & " out of 5 stars"))
        End If
    End Sub
    Private Sub activePhaseStats(ByVal currentPhase As Phase)
        Try
            'Days Remaining
            If Date.Today >= currentPhase.completionDate Then
                phPhaseStatus.Controls.Add(createDataEntry("Days Remaining", 0))
            Else
                phPhaseStatus.Controls.Add(createDataEntry("Days Remaining", (currentPhase.completionDate - Date.Today).Value.Days.ToString))
            End If

            'Phase start date
            If Not IsNothing(currentPhase.activationDate) Then
                phPhaseStatus.Controls.Add(createDataEntry("Start Date", currentPhase.activationDate.Value.ToString("MMMM dd, yyyy")))
            End If

            'Phase end date
            If Not IsNothing(currentPhase.activationDate) Then
                phPhaseStatus.Controls.Add(createDataEntry("Completion Date", currentPhase.completionDate.Value.ToString("MMMM dd, yyyy")))
            End If

            'Phase's goal
            phPhaseStatus.Controls.Add(createDataEntry("Phase Goal", currentPhase.goal.ToString("c")))

            'Phase's current pledges
            phPhaseStatus.Controls.Add(createDataEntry("Phase Pledges", currentPhase.pledgeTotal.ToString("c")))

            'Phase % (As meter)
            Dim li As New HtmlGenericControl("li")
            li.Attributes.Add("class", "summaryProgressbar")
            Dim progressBar As New ASP.usercontrols_progressbar_ascx
            progressBar.fullSize = True
            If (currentPhase.pledgeTotal > 0) Then
                progressBar.percentage = CInt((currentPhase.pledgeTotal / currentPhase.goal) * 100)
            Else
                progressBar.percentage = 0
            End If
            li.Controls.Add(progressBar)
            phPhaseStatus.Controls.Add(li)
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("EditAccount.master.vb : activePhaseStats()")
            sb.AppendLine("currentPhase id: " & currentPhase.id)

            saveErrorMessage(getLoggedInMember, ex.Message & vbNewLine & ex.StackTrace & vbNewLine & ex.Source, sb.ToString())
            'Response.Write("Error: " & ex.ToString & "<br /><br />" & sb.ToString)
        End Try
    End Sub
    Private Sub previousFailedPhase(ByRef previousPhase As Phase)
        If Not IsNothing(previousPhase) Then
            'Display previous data
            phPreviousPhase.Visible = True
            lblPreviousPhase.Text = previousPhase.name
            'Response.Write("previousFailedPhase()")

            'Show status
            phPhaseStatus.Controls.Add(createDataEntry("Status", "Unsuccessful"))

            'Phase start date
            If Not IsNothing(previousPhase.activationDate) Then
                phPhaseStatus.Controls.Add(createDataEntry("Start Date", previousPhase.activationDate.Value.ToString("MMMM dd, yyyy")))
            End If

            'Previous Phase end date
            If Not IsNothing(previousPhase.completionDate) Then
                phPhaseStatus.Controls.Add(createDataEntry("Completion Date", previousPhase.completionDate.Value.ToString("MMMM dd, yyyy")))
            End If

            'Previous Phase goal
            If Not IsNothing(previousPhase.goal) Then
                phPhaseStatus.Controls.Add(createDataEntry("Phase Goal", FormatCurrency(previousPhase.goal)))
            End If

            'Previous Phase pledges
            If Not IsNothing(previousPhase.pledgeTotal) Then
                phPhaseStatus.Controls.Add(createDataEntry("Pledges", FormatCurrency(previousPhase.pledgeTotal)))
            End If

        End If
    End Sub
    Private Sub previousSuccessfulPhase(ByRef previousPhase As Phase)
        'Display previous data
        phPreviousPhase.Visible = True
        lblPreviousPhase.Text = previousPhase.name
        'Response.Write("previousSuccessfulPhase()")

        If Not IsNothing(previousPhase) Then
            'Show status
            phPhaseStatus.Controls.Add(createDataEntry("Status", "Completed Successfully"))

            'Previous Phase goal
            If Not IsNothing(previousPhase.goal) Then
                phPhaseStatus.Controls.Add(createDataEntry("Phase Goal", FormatCurrency(previousPhase.goal)))
            End If

            'Previous Phase pledges
            If Not IsNothing(previousPhase.pledgeTotal) Then
                phPhaseStatus.Controls.Add(createDataEntry("Pledged", FormatCurrency(previousPhase.pledgeTotal)))
            End If
        End If
    End Sub
    Private Sub displayDataByCampaignStatus_Complete(ByRef CampaignStatistics As CampaignStatistics)
        phViewCampaign.Visible = True
        lblPhaseStatus.Text = statusTypeValues.Complete

        'Add notes from library.
        lblPhaseStatusNotes.Text = umbraco.library.GetDictionaryItem(dictionaryEntry.campaignComplete).Replace("[DATE]", CampaignStatistics.completionDate.ToLongDateString)

    End Sub

#End Region

End Class