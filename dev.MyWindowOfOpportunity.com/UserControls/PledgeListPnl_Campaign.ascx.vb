Imports Common
Partial Class UserControls_Pledges_Listing
    Inherits System.Web.UI.UserControl


#Region "Properties"
    Private blPledges As New blPledges

    Private _nodeId As Integer
    Public Property NodeId() As Integer
        Get
            Return _nodeId
        End Get
        Set(ByVal value As Integer)
            _nodeId = value
        End Set
    End Property
#End Region

#Region "Handles"
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                'Instantiate variables
                blPledges = New blPledges
                Dim bReturn As BusinessReturn
                Dim lstCampaignPledges As List(Of CampaignPledge)
                Dim lstCampaignPledges_Phase01 As New List(Of CampaignPledge)
                Dim lstCampaignPledges_Phase02 As New List(Of CampaignPledge)
                Dim lstCampaignPledges_Phase03 As New List(Of CampaignPledge)

                '
                bReturn = blPledges.selectAll_byCampaignId_forCampaignPg(NodeId)

                If bReturn.isValid Then
                    lstCampaignPledges = bReturn.DataContainer(0)

                    'Show proper msgs
                    If lstCampaignPledges.Count = 0 Then
                        'Show a 'no pledges' msg
                        pnlNoPledges.Visible = True
                    Else
                        'Seperate pledges into phase lists
                        For Each pledge As CampaignPledge In lstCampaignPledges
                            Select Case pledge.phaseNumber
                                Case 1
                                    lstCampaignPledges_Phase01.Add(pledge)
                                Case 2
                                    lstCampaignPledges_Phase02.Add(pledge)
                                Case 3
                                    lstCampaignPledges_Phase03.Add(pledge)
                                Case Else
                            End Select
                        Next

                        'Add pledge list to phase panel
                        ucPhase01.lstCampaignPledges = lstCampaignPledges_Phase01
                        ucPhase02.lstCampaignPledges = lstCampaignPledges_Phase02
                        ucPhase03.lstCampaignPledges = lstCampaignPledges_Phase03


                        ''
                        'gv.DataSource = lstCampaignPledges
                        'gv.DataBind()

                        'GridView1.DataSource = lstCampaignPledges_Phase01
                        'GridView2.DataSource = lstCampaignPledges_Phase02
                        'GridView3.DataSource = lstCampaignPledges_Phase03

                        'GridView1.DataBind()
                        'GridView2.DataBind()
                        'GridView3.DataBind()
                    End If
                Else
                    'Response.Write("Invalid: " & bReturn.ExceptionMessage)
                End If

            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\PledgeListPnl_Campaign.ascx.vb : Page_Load()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Response.Write("Error: " & ex.ToString)
        End Try

    End Sub
#End Region

#Region "Methods"
#End Region

End Class
