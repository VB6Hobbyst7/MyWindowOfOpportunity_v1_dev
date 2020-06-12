Imports System.Data
Imports Common

Partial Class CheckOut_RewardList
    Inherits UserControl

#Region "Properties"
    'Private dt As DataTable
    'Private Structure DtStructure
    '    Const NodeId As String = "NodeId"
    '    'Const PledgeAmount As String = "PledgeAmount"
    'End Structure

    Private blRewards As blRewards


#End Region

#Region "Handle"
    Private Sub UserControls_CheckOut_RewardList_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Try
                'Instantiate variables
                Dim businessReturn As New BusinessReturn
                Dim lstRewards As New List(Of Reward)
                blRewards = New blRewards
                Dim nodeId As Integer = CInt(HttpUtility.UrlDecode(Request.QueryString(queryParameters.nodeId)))

                'Obtain data
                businessReturn = blRewards.obtainRewards_byCampaignId(nodeId)

                If businessReturn.isValid Then
                    'Extract data
                    lstRewards = DirectCast(businessReturn.DataContainer(0), List(Of Reward))

                    'Display data
                    lstviewRewards.DataSource = lstRewards.OrderBy(Function(x) x.contributionAmount)


                    lstviewRewards.DataBind()

                    'TEMP--- DELETE WHEN FINISHED [only used to see data in class]
                    'gv.DataSource = lstRewards
                    'gv.DataBind()
                End If


            Catch ex As Exception
                Dim sb As New StringBuilder()
                sb.AppendLine("\UserControls\CheckOut_RewardList.ascx.vb : Load()")

                saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
                'Response.Write(ex.ToString)
            End Try
        End If
    End Sub

    Private Sub lstviewRewards_ItemDataBound(sender As Object, e As ListViewItemEventArgs) Handles lstviewRewards.ItemDataBound
        'If hidden field is marked as false then set panel as inactive.
        Try
            If e.Item.ItemType = ListViewItemType.DataItem Then
                Dim pnl As Panel = DirectCast(e.Item.FindControl("pnlRewardItem"), Panel)
                Dim hfld As HiddenField = DirectCast(e.Item.FindControl("hfldInactive"), HiddenField)
                If Not IsNothing(pnl) AndAlso Not IsNothing(hfld) Then
                    If hfld.Value = False.ToString Then
                        pnl.CssClass += "inactive"
                    End If
                End If
            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\CheckOut_RewardList.ascx.vb : lstviewRewards_ItemDataBound()")

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Response.Write(ex.ToString)
        End Try
    End Sub
#End Region

#Region "Methods"
#End Region
End Class
