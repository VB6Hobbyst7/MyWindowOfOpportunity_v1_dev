Imports Common
Imports umbraco.Core.Models
Imports umbraco.Web

Partial Class UserControls_TimelineSegment
    Inherits System.Web.UI.UserControl


#Region "Properties"
    Dim _uHelper As Uhelper = New Uhelper()
    Public Property nodeId As Integer = -1
    'Public Property oddCount As Boolean = True
    'Private Enum panelSide
    '    vLeftAlign = 0
    '    vRightAlign = 1
    'End Enum
#End Region

#Region "Handles"
    Private Sub UserControls_TimelineSegment_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then

            Try 'Instantiate variables
                Dim thisNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(nodeId)
                Dim sb As StringBuilder = New StringBuilder
                Dim entryDate As DateTime

                'Obtain entry date and convert to proper string format
                entryDate = thisNode.GetPropertyValue(Of DateTime)(nodeProperties.entryDate)


                ltrlDay.Text = entryDate.ToString("MMM d")
                ltrlYear.Text = entryDate.ToString("yyyy")
                ltrlTitle.Text = thisNode.Name
                ltrlContent.Text = thisNode.GetPropertyValue(nodeProperties.summary).ToString()
            Catch ex As Exception
                Dim sb As New StringBuilder()
                sb.AppendLine("\UserControls\TimelineSegment.ascx.vb : UserControls_TimelineSegment_Load()")
                saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            End Try
        End If
    End Sub
#End Region

#Region "Methods"
#End Region
End Class
