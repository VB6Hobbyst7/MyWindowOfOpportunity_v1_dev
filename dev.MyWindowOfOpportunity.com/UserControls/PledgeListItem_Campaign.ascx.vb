Imports System.Globalization
Imports Common
Imports umbraco.Web

Partial Class UserControls_PledgeListItem
    Inherits System.Web.UI.UserControl


#Region "Properties"
    'Private Structure _pledgeStatusStruct
    '    Const fulfilled As String = "fulfilled"
    '    Const canceled As String = "canceled"
    '    Const declined As String = "declined"
    '    Const reimbursed As String = "reimbursed"
    'End Structure
    'Public Enum pledgeStatusEnum
    '    none
    '    fulfilled
    '    canceled
    '    declined
    '    reimbursed
    'End Enum
    'Private _pledgeStatus As pledgeStatusEnum = pledgeStatusEnum.none
    'Public Property pledgeStatus As pledgeStatusEnum
    '    Get
    '        Return _pledgeStatus
    '    End Get
    '    Set(value As pledgeStatusEnum)
    '        _pledgeStatus = value
    '    End Set
    'End Property

    'Public Property pledged As Double = 0
    'Public Property pledgeDate As Date
    'Public Property donatedBy As String = String.Empty
    Public Property campaignPledge() As CampaignPledge
#End Region

#Region "Handles"
    Private Sub UserControls_PledgeListItem_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                If Not IsNothing(campaignPledge) Then

                    'Instantiate variables
                    Dim thisNodeId As Integer = UmbracoContext.Current.PageId

                    'Show data
                    If Not IsNothing(campaignPledge.pledgeDate) Then ltrlDate_Campaign.Text = CDate(campaignPledge.pledgeDate).ToString("MMMM d, yyyy")
                    ltrlDonatedBy_Campaign.Text = campaignPledge.pledgingMemberName
                    If Not IsNothing(campaignPledge.pledgeAmount) Then ltrlPledged_Campaign.Text = CDec(campaignPledge.pledgeAmount).ToString("C", CultureInfo.CurrentCulture)
                    memberImgUrl.ImageUrl = campaignPledge.pledgingMemberImgUrl
                    memberImgUrl.AlternateText = campaignPledge.pledgingMemberName
                    memberImgUrl.Attributes.Add("title", campaignPledge.pledgingMemberName)



                    'Dim lst As New List(Of CampaignPledge)
                    'lst.Add(campaignPledge)
                    'gv.DataSource = lst
                    'gv.DataBind()

                End If
            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\PledgeListItem_Campaign.ascx.vb : UserControls_PledgeListItem_Load()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Response.Write("<br />Error: " & ex.ToString)
        End Try
    End Sub
#End Region

#Region "Methods"
#End Region
End Class
