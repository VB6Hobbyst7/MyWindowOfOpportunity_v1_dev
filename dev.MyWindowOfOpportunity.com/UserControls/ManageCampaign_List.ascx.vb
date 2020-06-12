Imports Common
Imports System.Data
Imports umbraco.Core.Models

Partial Class UserControls_ManageCampaign_List
    Inherits System.Web.UI.UserControl

#Region "Properties"
    Public Property nodeId() As Integer
    Public Property isAdmin() As Boolean
    Private dtAssociatedNodes As DataTable
    Dim _uHelper As Uhelper = New Uhelper()
#End Region

#Region "Handles"
    Private Sub UserControls_ManageCampaign_List_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Try
                'lbl01.Text = nodeId.ToString
                'lbl02.Text = isAdmin.ToString

                'Instantiate variables
                Dim thisNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(nodeId)

            'Create datatable structure
            CreateTableStructure()

            If isAdmin Then
                'Starting from team page, get team and child info
                AddToDt(thisNode.Id, True, True)
                For Each childNode As IPublishedContent In thisNode.Children
                    AddToDt(childNode.Id, False)
                Next
            Else
                'Starting from campaign page, get it and parent team name
                AddToDt(thisNode.Parent.Id, True)
                AddToDt(thisNode.Id, False)
            End If

            '
            lstview.DataSource = dtAssociatedNodes
            lstview.DataBind()
            Catch ex As Exception
            Dim sb As New StringBuilder()
                sb.AppendLine("\UserControls\Partial Class UserControls_ManageCampaign_List.ascx.vb : UserControls_ManageCampaign_List_Load()")
                saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

            End Try
        End If
    End Sub
#End Region

#Region "Methods"
    Private Sub CreateTableStructure()
        'Instantiate new datatable
        dtAssociatedNodes = New DataTable

        'Add collumns to datatable
        dtAssociatedNodes.Columns.Add(Miscellaneous.nodeId, GetType(Integer))
        dtAssociatedNodes.Columns.Add(Miscellaneous.isTeamName, GetType(Boolean)).DefaultValue = False
        dtAssociatedNodes.Columns.Add(Miscellaneous.isAdmin, GetType(Boolean)).DefaultValue = False
        dtAssociatedNodes.AcceptChanges()
    End Sub

    Private Sub AddToDt(ByVal _thisNodeId As Integer, ByVal _isTeamName As Boolean, Optional ByVal _isAdmin As Boolean = False) ', Optional ByVal parentNodeId As Integer = -1)
        Dim dr As DataRow = dtAssociatedNodes.NewRow
        dr(Miscellaneous.nodeId) = _thisNodeId
        dr(Miscellaneous.isTeamName) = _isTeamName
        dr(Miscellaneous.isAdmin) = _isAdmin
        dtAssociatedNodes.Rows.Add(dr)
    End Sub


#End Region

End Class
