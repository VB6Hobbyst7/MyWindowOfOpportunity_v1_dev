Imports System.Data
Imports Common
Imports umbraco.Core.Models
Imports umbraco.Web

Partial Class UserControls_Timeline
    Inherits System.Web.UI.UserControl


#Region "Properties"
    Dim _uHelper As Uhelper = New Uhelper()
    Private dt As DataTable
    Private isOdd As Boolean = True
    Private Structure dtStructure
        Const nodeId As String = "nodeId"
        Const entryDate As String = "entryDate"
        Const oddCount As String = "oddCount"
    End Structure



#End Region

#Region "Handles"
    Private Sub UserControls_Timeline_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then

            Try
                'Instantiate variables
                Dim thisNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(UmbracoContext.Current.PageId)

                'Create datatable
                createTable()

                'Obtain all timeline segments
                'Loop thru child nodes to find the timeline folder.
                For Each Timeline As IPublishedContent In thisNode.Children
                    If Timeline.DocumentTypeAlias = docTypes.Timeline Then
                        'Loop thru all of the timeline segments
                        For Each timelineSegments As IPublishedContent In Timeline.Children
                            'Add record to datatable
                            Dim dr As DataRow = dt.NewRow
                            dr(dtStructure.nodeId) = timelineSegments.Id
                            dr(dtStructure.entryDate) = timelineSegments.GetPropertyValue(Of Date)(nodeProperties.entryDate)
                            dr(dtStructure.oddCount) = isOdd
                            dt.Rows.Add(dr)
                            'Switch between even/odd
                            isOdd = Not isOdd
                        Next

                        'Exit the loop.
                        Exit For
                    End If
                Next

                '
                lstviewTimeEntries.DataSource = dt
                lstviewTimeEntries.DataBind()

            Catch ex As Exception
                Dim sb As New StringBuilder()
                sb.AppendLine("\UserControls\Timeline.ascx.vb : UserControls_Timeline_Load()")
                saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            End Try
        End If
    End Sub
#End Region

#Region "Methods"
    Private Sub createTable()
        'Instantiate variables
        dt = New DataTable
        '
        dt.Columns.Add(dtStructure.nodeId, GetType(Integer))
        dt.Columns.Add(dtStructure.entryDate, GetType(Date))
        dt.Columns.Add(dtStructure.oddCount, GetType(Boolean))
    End Sub
#End Region

End Class
