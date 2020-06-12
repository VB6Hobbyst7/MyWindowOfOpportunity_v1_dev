Imports umbraco
Imports Common
Imports System.Data

Partial Class UserControls_ManageCampaigns
    Inherits System.Web.UI.UserControl

#Region "Properties"
    Private userId As Integer
    Private dtAssociatedNodes As DataTable
    Private blMembers As blMembers
    Private blCampaigns As blCampaigns
    Dim _uHelper As Uhelper = New Uhelper()
#End Region

#Region "Handles"
    Private Sub UserControls_ManageCampaigns_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Try
                'Instantiate variables
                blMembers = New blMembers

                'Is current user logged in?
                Dim loginStatus As Web.Models.LoginStatusModel = blMembers.getCurrentLoginStatus()
                If LoginStatus.IsLoggedIn Then
                    'build page
                    loadPage()
                Else
                    'Save current page and go to login page
                    Session(Sessions.previousPage) = HttpContext.Current.Request.Url.AbsoluteUri
                    Response.Redirect(_uHelper.Get_IPublishedContentByID(siteNodes.Login).Url, False)
                    HttpContext.Current.ApplicationInstance.CompleteRequest()
                End If

            Catch ex As Exception
                Dim sb As New StringBuilder()
                sb.AppendLine("\UserControls\ManageCampaigns.ascx.vb : UserControls_ManageCampaigns_Load()")
                saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
                'Response.Write(ex.ToString)
            End Try
        End If
    End Sub
#End Region

#Region "Methods"
    Private Sub loadPage()
        'Instantiate variables
        blCampaigns = New blCampaigns
        Dim bReturn As BusinessReturn
        Dim manageCampaigns As ManageCampaigns

        'Obtain current user id
        userId = blMembers.GetCurrentMemberId()

        'Set page values
        hlnkCreateCampaign.NavigateUrl = _uHelper.Get_IPublishedContentByID(siteNodes.CreateACampaign).Url




        ''Obtain list of campaigns that user has access to.
        'createTableStructure()
        'userMemberOfCampaign(New IPublishedContent(siteNodes.Campaigns), False)

        ''Display data in list
        'lstviewCampaignList.DataSource = dtAssociatedNodes
        'lstviewCampaignList.DataBind()




        'Obtain data
        bReturn = blCampaigns.obtainTeamsAndCampaigns_byUserId(userId)

        If bReturn.isValid Then
            'Extract data
            manageCampaigns = DirectCast(bReturn.DataContainer(0), ManageCampaigns)

            'gv.DataSource = manageCampaigns.lstCampaignSummary
            'gv2.DataSource = manageCampaigns.lstTeamSummary
            'gv.DataBind()
            'gv2.DataBind()

            lstvCampaigns.DataSource = manageCampaigns.lstCampaignSummary
            lstvCampaigns.DataBind()

            lstvTeams.DataSource = manageCampaigns.lstTeamSummary
            lstvTeams.DataBind()


            'gv.DataSource = bReturn.DataContainer(0)
            'gv2.DataSource = bReturn.DataContainer(1)
            'gv3.DataSource = bReturn.DataContainer(2)

            'gv.DataBind()
            'gv2.DataBind()
            'gv3.DataBind()

            'Response.Write(bReturn.DataContainer.Count & "<br />")
            'Response.Write(bReturn.DataContainer(0).Count & "<br />")
            'Response.Write(bReturn.DataContainer(1).Count & "<br />")
            'Response.Write(bReturn.DataContainer(2).Count & "<br />")
        Else
            Response.Write("Error: " & bReturn.ExceptionMessage)
        End If
    End Sub
#End Region

End Class
