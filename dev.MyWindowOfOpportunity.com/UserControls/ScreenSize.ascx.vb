Imports Common
Imports umbraco
Imports umbraco.Core.Models
Imports umbraco.Web

Partial Class UserControls_ScreenSize
    Inherits System.Web.UI.MasterPage


#Region "Properties"
    Dim _uHelper As Uhelper = New Uhelper()
#End Region

#Region "Handles"
    Private Sub UserControls_ScreenSize_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If CBool(System.Configuration.ConfigurationManager.AppSettings("showNotes").ToString) = True Then
                'Instantiate variables
                Dim thisNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(UmbracoContext.Current.PageId)

                'If any notes exist, display on screen
                If thisNode.HasProperty(nodeProperties.notesFrontEnd) Then
                    ltrlNotesFrontEnd.Text = thisNode.GetPropertyValue(Of String)(nodeProperties.notesFrontEnd)
                    pnlNotes.Visible = True
                End If
                If thisNode.HasProperty(nodeProperties.notesBackEnd) Then
                    ltrlNotesBackEnd.Text = thisNode.GetPropertyValue(Of String)(nodeProperties.notesBackEnd)
                    pnlNotes.Visible = True
                End If
            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\ScreenSize.ascx.vb : UserControls_ScreenSize_Load()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
    End Sub
#End Region

#Region "Methods"
#End Region


End Class

