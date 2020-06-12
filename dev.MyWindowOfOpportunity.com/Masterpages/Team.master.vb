Imports umbraco
Imports umbraco.NodeFactory
Imports Common
Imports System.Data
Imports System.Net
Imports umbraco.Core
Imports umbraco.Core.Publishing
Imports umbraco.Web
Imports umbraco.Core.Models
Imports System.Threading

Partial Class Masterpages_Team
    Inherits System.Web.UI.MasterPage


#Region "Properties"
    Private blMembers As blMembers
    Private blTeams As blTeams
    Dim _uHelper As Uhelper = New Uhelper()
    Private Enum mviews
        vTeamSummary
        vEditTeam
    End Enum
#End Region

#Region "Handles"
    Private Sub Masterpages_Team_Load(sender As Object, e As EventArgs) Handles Me.Load
        'Instantiate variables
        Dim ipThisNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(UmbracoContext.Current.PageId)

        '
        ucTeamImageMngr.thisNodeId = ipThisNode.Id
        ucSelectFeaturedImage.thisNodeId = ipThisNode.Id
        ucShowFeaturedImg.propertyName = nodeProperties.teamImage
        ucShowFeaturedImg.thisNodeId = ipThisNode.Id
        ucSocialMediaManager.nodeId = ipThisNode.Id
        ucTeamEdit_TeamMembers.thisNodeId = ipThisNode.Id

        hlnkEditBtn.NavigateUrl = ipThisNode.Url & "?editMode=True"
        hlnkSummaryBtn.NavigateUrl = ipThisNode.Url
        'If IsPostBack Then
        '    'Set what tab is being displayed in session for other tabs to use.
        '    If IsNothing(hfldActiveTab.Value) Then
        '        Session(Miscellaneous.ActiveTab) = Nothing
        '    Else
        '        Session(Miscellaneous.ActiveTab) = hfldActiveTab.Value
        '    End If
        'End If
    End Sub
    Private Sub Masterpages_Standard_Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        'Instantiate return container
        Dim bReturn_TeamContent As BusinessReturn = New BusinessReturn

        Try
            '
            blMembers = New blMembers
            Dim loginStatus As Web.Models.LoginStatusModel = blMembers.getCurrentLoginStatus()

            'If querystring has editmode parameter, show edit mode.
            mviewTeamSummary.ActiveViewIndex = mviews.vTeamSummary
            If Context.Request.QueryString(queryParameters.editMode) IsNot Nothing Then
                If CBool(Request.QueryString(queryParameters.editMode)) = True Then
                    'mviewTeamSummary.ActiveViewIndex = mviews.vEditTeam

                    'Instantiate variables
                    Dim isValidUser As Boolean = False
                    blTeams = New blTeams
                    blMembers = New blMembers

                    If loginStatus.IsLoggedIn Then
                        'Get current loggedin id
                        Dim currentMemberId As Integer? = blMembers.GetCurrentMemberId()
                        If Not IsNothing(currentMemberId) Then
                            If blTeams.isTeamAdministrator_byUserId(currentMemberId, UmbracoContext.Current.PageId) Then
                                isValidUser = True
                            End If
                        End If
                    End If

                    '
                    If isValidUser Then
                        mviewTeamSummary.ActiveViewIndex = mviews.vEditTeam
                    Else
                        Response.Redirect(_uHelper.Get_IPublishedContentByID(UmbracoContext.Current.PageId).Url, True)
                        'Context.ApplicationInstance.CompleteRequest()
                        'http://dev.mywindowofopportunity.com/campaigns/f/fifthlabs/?editMode=True
                    End If

                End If
            End If


            'Instantiate variables
            Dim thisNodeId As Integer = Node.getCurrentNodeId
            Dim userId As Integer = 0
            blMembers = New blMembers
            blTeams = New blTeams
            Dim teamSummary As TeamSummary




            'Obtain team content
            bReturn_TeamContent = blTeams.getTeamContent_byId(thisNodeId)
            If bReturn_TeamContent.isValid Then
                'Extract data
                teamSummary = DirectCast(bReturn_TeamContent.DataContainer(0), TeamSummary)

                'Display team name
                ltrlTeamName.Text = teamSummary.teamName
                ltrlTeamName_EditPg.Text = teamSummary.teamName
                hdfdTemplateContent.Value = teamSummary.whoAreWe

                'Display team photo is available
                If Not String.IsNullOrEmpty(teamSummary.teamImageUrl) Then
                    imgTeamPhoto.ImageUrl = teamSummary.teamImageUrl
                    imgTeamPhoto.AlternateText = teamSummary.teamName
                Else
                    pnlTeamImg.Visible = False
                End If

                'Display team description text
                If Not String.IsNullOrEmpty(teamSummary.whoAreWe) Then
                    'ltrlWhoAreWe.Text = teamSummary.whoAreWe.Replace(Environment.NewLine, "<br />")
                    ltrlWhoAreWe.Text = teamSummary.whoAreWe
                Else
                    pnlWhoAreWe.Visible = False
                End If

                'Create list of social links
                CreateSocialLinks(teamSummary)

                '
                lstviewTeamMembers.DataSource = teamSummary.lstMembers
                lstviewTeamMembers.DataBind()

                '=======================================
                ''DEBUGGING ONLY
                'Dim lst As New List(Of TeamSummary)
                'lst.Add(teamSummary)
                'gv1.DataSource = lst
                'gv1.DataBind()

                'gv2.DataSource = teamSummary.lstMembers
                'gv2.DataBind()

                'Dim lstMP As New List(Of MembershipProperties)
                'For Each member As Member In teamSummary.lstMembers
                '    lstMP.Add(member.MembershipProperties)
                'Next
                'gv3.DataSource = lstMP
                'gv3.DataBind()

                'Dim lstDemog As New List(Of Demographics)
                'For Each member As Member In teamSummary.lstMembers
                '    lstDemog.Add(member.Demographics)
                'Next
                'gv4.DataSource = lstDemog
                'gv4.DataBind()
                '=======================================
            End If


            'Instantiate variables
            Dim lstCampaignSummaries As New List(Of CampaignSummary_TeamPg)
            Dim blCampaigns As New blCampaigns
            Dim bReturn_Campaigns As BusinessReturn


            'Obtain campaigns for team
            bReturn_Campaigns = blCampaigns.obtainAllCampaigns_byTeamId(thisNodeId)
            If bReturn_Campaigns.isValid Then
                'Obtain campaign data
                For Each item As Object In bReturn_Campaigns.DataContainer
                    lstCampaignSummaries.Add(DirectCast(item, CampaignSummary_TeamPg))
                Next

                'lstviewCampaigns.DataSource = lstCampaignSummaries.Where(Function(x) x.isActive = True)
                lstviewCampaigns.DataSource = lstCampaignSummaries.OrderByDescending(Function(x) x.isActive).ThenBy(Function(x) x.title)
                lstviewCampaigns.DataBind()


                '=======================================
                ''DEBUGGING ONLY
                'gv5.DataSource = lstCampaignSummaries
                'gv5.DataBind()
                '=======================================
            End If




            'Is current user logged in?
            If loginStatus.IsLoggedIn Then
                'Obtain member id
                userId = blMembers.GetCurrentMemberId()

                'Check if member is a team administrator
                If blTeams.isTeamAdministrator_byUserId(userId, thisNodeId) Then
                    'Show edit button.
                    'lbtnEdit.Visible = True
                    hlnkEditBtn.Visible = True
                End If
            End If
            'End If

            'If session exists, show proper alert message.
            If Not IsNothing(Session(Sessions.summaryUpdatedMsg)) Then
                'Instantiate variables
                Dim alert As ASP.usercontrols_alertmsg_ascx = New ASP.usercontrols_alertmsg_ascx

                'Set what type of alert to display.
                If CBool(Session(Sessions.summaryUpdatedMsg)) Then
                    'Show success msg
                    alert.MessageType = UserControls_AlertMsg.msgType.Success
                    alert.AlertMsg = "Updated Successfully"
                Else
                    'Show alert msg
                    alert.MessageType = UserControls_AlertMsg.msgType.Alert
                    alert.AlertMsg = "Unable to update"
                End If

                'Assign alert to placeholder
                phAlertMsg.Controls.Add(alert)

                'Clear temp session variable
                Session(Sessions.summaryUpdatedMsg) = Nothing
            End If
        Catch ext As ThreadAbortException
            'ignore.  caused by redirect
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\Masterpages\Team.master.vb : Masterpages_Standard_Page_PreRender()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Response.Write("Error: " & ex.ToString)
            'Response.Write("<br><br>Error: " & businessReturn.ExceptionMessage)
        End Try
    End Sub
    Private Sub lbtnSaveContent_Click(sender As Object, e As EventArgs) Handles lbtnSaveContent.Click
        'Instantiate variables
        blTeams = New blTeams
        If String.IsNullOrEmpty(hdfdTemplateContent.Value) Then
            Session(Sessions.summaryUpdatedMsg) = False
        Else
            Dim result As Attempt(Of PublishStatus) = blTeams.SaveTeamContent(Node.getCurrentNodeId, hdfdTemplateContent.Value, ucShowFeaturedImg.RemoveSelectedImg)
            Session(Sessions.summaryUpdatedMsg) = result.Success

            'Set active tab
            setTabCookie(tabNames.content)
            hdfdTemplateContent.Value = ""

            'Refresh page
            'Response.Redirect(Request.Url.AbsolutePath & "?" & queryParameters.editMode & "=" & True, False)
            HttpContext.Current.ApplicationInstance.CompleteRequest()
        End If
    End Sub
#End Region

#Region "Methods"
    Private Sub CreateSocialLinks(ByRef teamSummary As TeamSummary)
        'Obtain Social Medias
        If Not String.IsNullOrEmpty(teamSummary.facebookUrl) Then
            hlnkFacebook.NavigateUrl = teamSummary.facebookUrl
            hlnkFacebook.Visible = True
        End If
        If Not String.IsNullOrEmpty(teamSummary.twitterUrl) Then
            hlnkTwitter.NavigateUrl = teamSummary.twitterUrl
            hlnkTwitter.Visible = True
        End If
        If Not String.IsNullOrEmpty(teamSummary.linkedInUrl) Then
            hlnkLinkedIn.NavigateUrl = teamSummary.linkedInUrl
            hlnkLinkedIn.Visible = True
        End If
        If Not String.IsNullOrEmpty(teamSummary.supportEmailUrl) Then
            hlnkSupportEmail.NavigateUrl = "mailto:" & teamSummary.supportEmailUrl
            hlnkSupportEmail.Visible = True
        End If
    End Sub
#End Region
End Class

