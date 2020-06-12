Imports System.Data
Imports Common
Imports umbraco.Web

Partial Class UserControls_RewardList
    Inherits System.Web.UI.UserControl


#Region "Properties"
    'Private dt As DataTable
    'Private Structure dtStructure
    '    Const nodeId As String = "nodeId"
    'End Structure

    Private blRewards As blRewards
    Public Property isVisible As Boolean = True
#End Region

#Region "Handles"
    Private Sub UserControls_RewardList_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Try
                'Instantiate variables
                Dim businessReturn As New BusinessReturn
                Dim lstRewards As New List(Of Reward)
                blRewards = New blRewards
                Dim nodeId As Integer = UmbracoContext.Current.PageId

                'Obtain data
                businessReturn = blRewards.obtainRewards_byCampaignId(nodeId)

                If businessReturn.isValid Then
                    'Extract data
                    lstRewards = DirectCast(businessReturn.DataContainer(0), List(Of Reward))

                    If lstRewards.Count > 0 Then
                        'Display data
                        lstviewRewards.DataSource = lstRewards.OrderBy(Function(x) x.contributionAmount)
                        lstviewRewards.DataBind()
                    Else
                        isVisible = False
                        phRewardList.Visible = False
                    End If
                End If

            Catch ex As Exception
                Dim sb As New StringBuilder()
                sb.AppendLine("\UserControls\RewardList.ascx.vb : Load()")

                saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
                'Response.Write(ex.ToString)
            End Try
        End If
    End Sub
#End Region

#Region "Methods"
    'Private Sub createTable()
    '    'Instantiate variables
    '    dt = New DataTable
    '    'Add rows to dt
    '    dt.Columns.Add(dtStructure.nodeId, GetType(Integer))
    'End Sub
#End Region
End Class
