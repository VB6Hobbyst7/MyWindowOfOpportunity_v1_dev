Imports umbraco
Imports Common
Imports umbraco.Core.Models
Imports umbraco.Web

Partial Class UserControls_TimelineEntry
    Inherits System.Web.UI.UserControl


#Region "Properties"
    Dim _uHelper As Uhelper = New Uhelper()
    'Private _thisNodeId As Int16
    Public Property thisNodeId() As Int16
        Get
            'Return _thisNodeId
            Return hfldNodeId.Value
        End Get
        Set(value As Int16)
            '_thisNodeId = value
            hfldNodeId.Value = value.ToString
        End Set
    End Property

    Public ReadOnly Property entryTitle As String
        Get
            Return txbTitle.Text.Trim
        End Get
    End Property

    Public ReadOnly Property entryDate As String
        Get
            Return txbCalendar.Text.Trim
        End Get
    End Property

    Public ReadOnly Property editorText As String
        Get
            Return hfldTimelineContent.Value
        End Get
    End Property

    Public ReadOnly Property isActive As Boolean
        Get
            Return cbActive.Checked
        End Get
    End Property

    Private blTimelines As blTimelines
#End Region

#Region "Handles"
    Private Sub UserControls_TimelineEntry_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        Try
            'Add IPublishedContent Id to panel as an attribute
            pnl.Attributes.Add(Miscellaneous.data + Miscellaneous.nodeId, thisNodeId)

            If thisNodeId = -1 Then
                clearData()
            Else
                'Instantiate variables
                Dim thisNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(thisNodeId)

                'Obtain data and populate page.
                txbTitle.Text = thisNode.Name 'GetProperty(nodeProperties.heading).Value
                If thisNode.HasProperty(nodeProperties.entryDate) Then txbCalendar.Text = thisNode.GetPropertyValue(Of Date)(nodeProperties.entryDate).ToString("MMM dd, yyyy")
                If thisNode.HasProperty(nodeProperties.summary) Then
                    hfldTimelineContent.Value = thisNode.GetPropertyValue(Of String)(nodeProperties.summary)
                End If
                If thisNode.HasProperty(nodeProperties.showOnTimeline) Then cbActive.Checked = thisNode.GetPropertyValue(Of Boolean)(nodeProperties.showOnTimeline)
            End If

            'Assign relation between checkbox and label
            lbl.Attributes.Add("for", cbActive.ClientID)

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\TimelineEntry.ascx.vb : UserControls_TimelineEntry_PreRender()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Response.Write(ex.ToString)
        End Try
    End Sub
#End Region

#Region "Methods"
    Private Sub clearData()
        'Obtain data and populate page.
        txbTitle.Text = ""
        txbCalendar.Text = ""
        hfldTimelineContent.Value = ""
        'ckEditor.Text = ""
        cbActive.Checked = False
    End Sub
#End Region
End Class

