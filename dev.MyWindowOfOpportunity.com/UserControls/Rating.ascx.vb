Imports Common
Imports umbraco.Core
Imports umbraco.Core.Models
Imports umbraco.Core.Services
Imports umbraco.Web

Partial Class UserControls_Rating
    Inherits System.Web.UI.UserControl


#Region "Properties"
    Private blMembers As blMembers = New blMembers
    Private blIsAlreadySubmittedRating As Boolean = False
    Private _nodeId As Integer
    Dim _uHelper As Uhelper = New Uhelper()
    Public Property NodeId() As Integer
        Get
            Return _nodeId
        End Get
        Set(ByVal value As Integer)
            _nodeId = value
        End Set
    End Property
#End Region

#Region "Handles"
    Private Sub UserControls_Rating_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            'Instantiate variables
            Try
                Dim ms As ContentService = ApplicationContext.Current.Services.ContentService
                Dim blPhases As New blPhases
                Dim thisNode As IPublishedContent = UmbracoContext.Current.PublishedContentRequest.PublishedContent
                Dim bReturn As BusinessReturn
                NodeId = thisNode.Id

                'Is member logged in?
                If blMembers.isMemberLoggedIn() Then
                    'Member is logged in

                    'Instantiate variables
                    Dim disableRating As Boolean = False
                    Dim memberId As Integer? = blMembers.GetCurrentMemberId()

                    If Not IsNothing(memberId) Then
                        Dim blCampaigns As New blCampaigns
                        If blCampaigns.isUserAMemberOfCampaign(NodeId, memberId) Then
                            '✓	show stars as disabled with rating
                            '✓	show comments with previous msg and disabled
                            '✓	hide submit button
                            txtComment.Disabled = True
                            hlnkSubmit.Visible = False
                            hdnIsMemberAlreadyGivenRating.Value = True
                            txtComment.InnerText = "Campaign members cannot rate this campaign."
                        Else

                            'Set hidden field value
                            hdnIsMemberLoggedIn.Value = True

                            'Determine if member submitted review already.
                            bReturn = blPhases.hasMemberSubmittedReview(blMembers.GetCurrentMemberId(), NodeId)
                            blIsAlreadySubmittedRating = bReturn.isValid

                            'Has member submitted review?
                            If blIsAlreadySubmittedRating Then
                                '✓	show stars as disabled with rating
                                '✓	show comments with previous msg and disabled
                                '✓	hide submit button
                                txtComment.Disabled = True
                                hlnkSubmit.Visible = False
                                hdnIsMemberAlreadyGivenRating.Value = True

                                Dim rating As Rating = DirectCast(bReturn.DataContainer(0), Rating)
                                txtComment.InnerText = rating.message
                                hdnRating.Value = rating.stars

                            End If
                        End If
                    End If

                Else
                    'Member is not logged in
                    hdnIsMemberLoggedIn.Value = False
                    '✓	disable submit button
                    '✓	disable stars
                    '✓	disable comments
                    '✓	show login warning
                    txtComment.Disabled = True
                    hlnkSubmit.CssClass += " disabled"
                    hdnIsMemberAlreadyGivenRating.Value = True
                End If

                'Response.Write("isMemberLoggedIn: " & blMembers.isMemberLoggedIn())
                'Response.Write("<br>CurrentMemberId: " & blMembers.GetCurrentMemberId())
                'Response.Write("<br>NodeId: " & NodeId)
                'Response.Write("<br>submitted: " & blIsAlreadySubmittedRating)

                'Add IPublishedContent id to uc
                ucRatingSummary.thisNodeId = NodeId

            Catch ex As Exception
                Dim sb As New StringBuilder()
                sb.AppendLine("\UserControls\UserControls_Rating.ascx.vb : UserControls_Rating()")

                saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            End Try
        End If
    End Sub

    Protected Sub btnSubRating_Click(sender As Object, e As EventArgs)
        Try
            'Response.Write("click<br>")
            'Instantiate variables
            Dim blPhases As New blPhases
            Dim bReturn As BusinessReturn

            'Submit review
            bReturn = blPhases.createReview(Convert.ToInt16(hdnRating.Value), txtComment.InnerText, UmbracoContext.Current.PageId)

            If bReturn.isValid Then
                'Show thankyou msg and disable controls
                pnlThankyou.Visible = True
                txtComment.Disabled = True
                hlnkSubmit.Visible = False
                hdnIsMemberAlreadyGivenRating.Value = True
            Else
                'Show error message
                pnlError.Visible = True
            End If

        Catch ex As Exception
            'divErrorAlert.Visible = True
            'divWarning.Visible = False
            'divThanxAlert.Visible = False

            Dim sb As New StringBuilder()
            sb.AppendLine("Rating.ascx.vb : btnSubRating_Click()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Response.Write("Error: " & ex.ToString)
        End Try
    End Sub
#End Region

#Region "Methods"
#End Region

End Class
