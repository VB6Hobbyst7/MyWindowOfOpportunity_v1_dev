Imports Common
Imports umbraco.Core.Models

Partial Class UserControls_ulTeamAccts
    Inherits System.Web.UI.UserControl

#Region "Properties"
    Private _nodeId As Integer
    Dim _uHelper As Uhelper = New Uhelper()
    Public Property nodeId() As Integer
        Get
            Return _nodeId
        End Get
        Set(value As Integer)
            _nodeId = value
        End Set
    End Property
#End Region

#Region "Handles"
    Private Sub UserControls_ulTeamAccts_Load(sender As Object, e As EventArgs) Handles Me.Load
        'If Not IsPostBack Then

        '<ul class="ulTeamAccts">
        '    <li>
        '        <h6 class="teamName"><a href="#">Team Name</a></h6>
        '        <ul>
        '            <li>
        '                <h6 class="campaignName"><a href="#">Campaign Name 03</a></h6>
        '            </li>
        '        </ul>
        '    </li>
        '</ul>

        Try
            'Instantiate variables
            Dim teamNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(nodeId)
            Dim ulParent As HtmlGenericControl = New HtmlGenericControl("ul")

            'Obtain corporate info
            hlnkTeamName.Text = teamNode.Name
            hlnkTeamName.NavigateUrl = teamNode.Url

            'Obtain all team nodes
            For Each campaignNode As IPublishedContent In teamNode.Children
                If campaignNode.DocumentTypeAlias = docTypes.Campaign Then
                    Dim liTeam As HtmlGenericControl = New HtmlGenericControl("li")
                    Dim h6Team As HtmlGenericControl = New HtmlGenericControl("h6")
                    Dim hrefTeam As HyperLink = New HyperLink
                    Dim ulCampaign As HtmlGenericControl = New HtmlGenericControl("ul")

                    hrefTeam.Text = campaignNode.Name
                    hrefTeam.NavigateUrl = campaignNode.Url

                    h6Team.Attributes.Add("class", "teamName")
                    h6Team.Controls.Add(hrefTeam)
                    liTeam.Controls.Add(h6Team)

                    liTeam.Controls.Add(ulCampaign)
                    ulParent.Controls.Add(liTeam)
                End If
            Next

            '
            phTeamList.Controls.Add(ulParent)
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\ulTeamAccts.ascx.vb : UserControls_ulTeamAccts_Load()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
        'End If
    End Sub
#End Region

#Region "Methods"
#End Region
End Class
