Imports Common
Imports umbraco.Core.Models
Imports umbraco.Web

Partial Class UserControls_ManageCampaign_ListItem
    Inherits System.Web.UI.UserControl

#Region "Properties"
    Public Property nodeId() As Integer
    Public Property isAdmin() As Boolean
    Public Property isTeamName() As Boolean
    Private blMembers As blMembers
    Private blTeams As blTeams
    Private Enum viewType
        vTeamName
        vCampaignName
    End Enum
    Dim _uHelper As Uhelper = New Uhelper()
#End Region

#Region "Handles"
    Private Sub UserControls_ManageCampaign_ListItem_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            'Instantiate variables
            Try
                Dim thisNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(nodeId)
                blMembers = New blMembers
                blTeams = New blTeams

                'Determine what kind of view to show text as.
                If isTeamName Then
                    'Show proper text font
                    mvItemType.ActiveViewIndex = viewType.vTeamName
                    mvItemType_Mbl.ActiveViewIndex = viewType.vTeamName
                    ltrlTeamName.Text = thisNode.Name
                    ltrlTeamName_Mbl.Text = thisNode.Name

                    'Determine if the view button is to be disabled
                    hlnkEdit.CssClass += Miscellaneous.disabled

                    'Is current user logged in?
                    Dim loginStatus As Models.LoginStatusModel = blMembers.getCurrentLoginStatus()
                    If loginStatus.IsLoggedIn Then
                        'Obtain member id
                        Dim userId As Integer = blMembers.GetCurrentMemberId()

                        'Check if member is a team administrator
                        If blTeams.isTeamAdministrator_byUserId(userId, nodeId) Then
                            'Remove disabled class
                            hlnkEdit.CssClass = hlnkEdit.CssClass.Replace(Miscellaneous.disabled, "")
                            'Set navigation urls
                            hlnkEdit.NavigateUrl = thisNode.Url & "?" & queryParameters.editMode & "=" & True
                        End If
                    End If

                    'Set navigation urls
                    hlnkView.NavigateUrl = thisNode.Url
                    'hlnkEdit.NavigateUrl = New IPublishedContent(siteNodes.EditCampaign).NiceUrl & "?" & queryParameters.nodeId & "=" & HttpUtility.UrlEncode(nodeId)
                Else
                    'Show proper text font
                    mvItemType.ActiveViewIndex = viewType.vCampaignName
                    mvItemType_Mbl.ActiveViewIndex = viewType.vCampaignName
                    ltrlCampaignName.Text = thisNode.Name
                    ltrlCampaignName_Mbl.Text = thisNode.Name
                    'Determine if the view button is to be disabled
                    If Not thisNode.GetPropertyValue(Of Boolean)(nodeProperties.published) Then hlnkView.CssClass += Miscellaneous.disabled
                    'Set navigation urls
                    hlnkView.NavigateUrl = thisNode.Url
                    hlnkEdit.NavigateUrl = _uHelper.Get_IPublishedContentByID(siteNodes.EditCampaign).Url & "?" & queryParameters.nodeId & "=" & HttpUtility.UrlEncode(nodeId)
                End If
            Catch ex As Exception
                Dim sb As New StringBuilder()
                sb.AppendLine("\UserControls\Partial Class UserControls_ManageCampaign_ListItem.ascx.vb : UserControls_ManageCampaign_ListItem_Load()")
                saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

            End Try
        End If


    End Sub
#End Region

#Region "Methods"
#End Region

End Class
