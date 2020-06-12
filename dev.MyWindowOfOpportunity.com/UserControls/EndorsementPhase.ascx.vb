Imports Common
Imports umbraco.Core.Models

Partial Class UserControls_EndorsementPhase
    Inherits System.Web.UI.UserControl


#Region "Properties"
    Public Property nodeId As Integer = -1
    Dim _uHelper As Uhelper = New Uhelper()
#End Region

#Region "Handles"
    Private Sub MUserControls_CampaignPhase_Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Try

                'Instantiate variables
                Dim thisNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(nodeId)
                Dim currentEndorsements As Integer = 0
                Dim percentageFunded As UInt16 = 0
                Dim blEndorsements As blEndorsements = New blEndorsements

                'Get current # of endorsements
                currentEndorsements = blEndorsements.selectCount_byCampaignId(nodeId)
                'Display endorsement count
                ltrlCurrentEndorsements.Text = currentEndorsements.ToString
                'Display endorsements needed
                ltrlEndorsementGoal.Text = siteValues.endorsementsNeeded
                'Obtain percentage funded
                percentageFunded = (currentEndorsements / 30) * 100
                'Add phase's percentage
                If percentageFunded > 100 Then
                    ucProgressBar.percentage = 100
                Else
                    ucProgressBar.percentage = percentageFunded
                End If
            Catch ex As Exception
                Dim sb As New StringBuilder()
                sb.AppendLine("\UserControls\UserControls_EndorsementPhase.ascx.vb : MUserControls_CampaignPhase_Page_Load()")
                saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            End Try
        End If

    End Sub
#End Region

#Region "Methods"
#End Region

End Class
