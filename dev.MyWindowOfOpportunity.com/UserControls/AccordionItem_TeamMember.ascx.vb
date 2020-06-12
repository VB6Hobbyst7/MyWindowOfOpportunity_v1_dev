Imports Common

Partial Class UserControls_AccordionItem_TeamMember
    Inherits System.Web.UI.UserControl


#Region "Properties"
    Public Property nodeId As Integer
    'Public Property memberName As String
    Public Property TeamAdministrator As Boolean
    Public Property CampaignAdministrator As Boolean
    Public Property CampaignMember As Boolean

    Private blMembers As blMembers
#End Region

#Region "Handles"
    Private Sub UserControls_AccordionItem_TeamMember_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                'Instantiate variables
                blMembers = New blMembers
                Dim pnlName As String = "pnl" & nodeId
                Dim multipleRoles As Boolean = False

                'Set panel and handle name/nav
                pnl.ID = pnlName
                hlnkHandle.Attributes.Add("href", "#" & pnlName)

                Dim businessReturn As BusinessReturn = blMembers.getMemberDemographics_byId(nodeId, True)

                Dim member As Member = businessReturn.DataContainer(0)

                'Set the member name
                ltrlMemberName_heading.Text = member.Demographics.firstName & " " & member.Demographics.lastName

                'Build the role list
                If TeamAdministrator Then
                    ltrlRoles.Text = memberRole.TeamAdministrator
                    multipleRoles = True
                End If
                If CampaignAdministrator Then
                    If multipleRoles Then
                        ltrlRoles.Text += " | "
                    End If
                    ltrlRoles.Text += memberRole.CampaignAdministrator
                    multipleRoles = True
                End If
                If CampaignMember Then
                    If multipleRoles Then
                        ltrlRoles.Text += " | "
                    End If
                    ltrlRoles.Text += memberRole.CampaignMember
                    multipleRoles = True
                End If

                'obtain user image.
                If String.IsNullOrEmpty(member.Demographics.photoUrl) Then
                    imgMemberPhoto.ImageUrl = getMediaURL(mediaNodes.defaultProfileImg, Crops.members)
                Else
                    imgMemberPhoto.ImageUrl = member.Demographics.photoUrl
                End If

                'Obtain the user's short description.
                ltrlShortDescription.Text = member.Demographics.briefDescription

            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\AccordionItem_TeamMember.ascx.vb : UserControls_AccordionItem_TeamMember_Load()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Response.Write("Error: " & ex.ToString & "<br />ID: " & nodeId.ToString)
        End Try
    End Sub
#End Region

#Region "Methods"
#End Region
End Class
