Imports Common
Imports umbraco.Core
Imports umbraco.Core.Models
Imports umbraco.Core.Services
Imports umbraco.Web
Imports System.Data

Partial Class UserControls_MemberRatingCampaigns
    Inherits System.Web.UI.UserControl

#Region "Properties"
    Private blMembers As blMembers = New blMembers
    Private dtAssociatedNodes As DataTable
    Private userId As Integer
    Private blCampaign As blCampaigns = New blCampaigns
    Dim clist As List(Of clsCampaign) = New List(Of clsCampaign)

    Private blPhases As blPhases
    Public Property campaignSummary As CampaignSummary
    Dim _uHelper As Uhelper = New Uhelper()
#End Region
    Dim ms As MemberService = ApplicationContext.Current.Services.MemberService
    'Dim cms As CampaignService = ApplicationContext.Current.Services.c
    Private Sub UserControls_MemberRatingCampaigns_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not IsPostBack Then
            Try
                Dim loginStatus As Models.LoginStatusModel
                'Is current user logged in?
                loginStatus = blMembers.getCurrentLoginStatus()
                If loginStatus.IsLoggedIn Then
                    'Obtain current user id
                    userId = blMembers.GetCurrentMemberId()
                    GetAllListOfMemberRatingCampaign()
                    If clist.Count > 0 Then
                        rptRatingCampaign.DataSource = clist
                        rptRatingCampaign.DataBind()
                    End If
                End If
            Catch ex As Exception
                Dim sb As New StringBuilder()
                sb.AppendLine("\UserControls\Partial Class UserControls_MemberRatingCampaigns.ascx.vb : UserControls_MemberRatingCampaigns_Load()")
                saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

            End Try
        End If
    End Sub

    Public Sub GetAllListOfMemberRatingCampaign()
        Try
            Dim ms As ContentService = ApplicationContext.Current.Services.ContentService
            'Dim thisNode As IPublishedContent = IPublishedContent.GetCurrent

            Dim MemberId = blMembers.GetCurrentMemberId()

            Dim campaignlist = blCampaign.ObtainFullList()
            If campaignlist IsNot Nothing Then
                For Each dr As DataRow In campaignlist.Rows
                    Dim uCampaign As clsCampaign = New clsCampaign()
                    Dim campaign = ms.GetById(Convert.ToInt32(dr("nodeId")))
                    Dim thisNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(Convert.ToInt32(campaign.Id))

                    Dim discovery = campaign.Children().Where(Function(x As Content) (x.Name.ToLower() = docTypes.discovery)).FirstOrDefault()
                    If discovery IsNot Nothing Then
                        For Each PhasesProperty As Content In discovery.Children
                            If PhasesProperty.HasProperty("member") Then
                                If PhasesProperty.GetValue("member").ToString() = MemberId Then
                                    uCampaign.CampaignName = campaign.Name
                                    If PhasesProperty.HasProperty("message") Then
                                        If PhasesProperty.GetValue("message") IsNot Nothing Then
                                            uCampaign.ReviewDetail = PhasesProperty.GetValue("message").ToString()
                                        End If
                                    End If
                                    If PhasesProperty.HasProperty("stars") Then uCampaign.Rating = PhasesProperty.GetValue("stars").ToString()
                                    uCampaign.RatingDate = PhasesProperty.CreateDate.ToString("MMM d, yyyy")
                                    uCampaign.NavigateUrl = thisNode.Url
                                    clist.Add(uCampaign)
                                End If
                            End If
                        Next
                    End If
                Next
            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\Partial Class UserControls_MemberRatingCampaigns.ascx.vb : GetAllListOfMemberRatingCampaign()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

        End Try
    End Sub
End Class

Public Class clsCampaign
    Public Property CampaignName() As String
        Get
            Return p_Campaign
        End Get
        Set
            p_Campaign = Value
        End Set
    End Property
    Private p_Campaign As String

    Public Property ReviewDetail() As String
        Get
            Return p_ReviewDetail
        End Get
        Set
            p_ReviewDetail = Value
        End Set
    End Property
    Private p_ReviewDetail As String

    Public Property Rating() As String
        Get
            Return p_Rating
        End Get
        Set
            p_Rating = Value
        End Set
    End Property
    Private p_Rating As String
    Public Property RatingDate() As String
        Get
            Return p_RatingDate
        End Get
        Set
            p_RatingDate = Value
        End Set
    End Property
    Private p_RatingDate As String

    Public Property NavigateUrl() As String
        Get
            Return p_NavigateUrl
        End Get
        Set
            p_NavigateUrl = Value
        End Set
    End Property
    Private p_NavigateUrl As String

End Class