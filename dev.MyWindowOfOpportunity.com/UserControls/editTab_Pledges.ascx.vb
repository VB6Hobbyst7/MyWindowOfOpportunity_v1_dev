Imports Common

Partial Class UserControls_editTab_Pledges
    Inherits System.Web.UI.UserControl


#Region "Properties"
    Private blPledges As New blPledges
    Private blMembers As blMembers = New blMembers
    Dim _uHelper As Uhelper = New Uhelper()
    Private _CurrentNodeId As Integer
    Public Property CurrentNodeId() As Integer
        Get
            Return _CurrentNodeId
        End Get
        Set(ByVal value As Integer)
            _CurrentNodeId = value
        End Set
    End Property

    Private _lstPledges As New ListView
    Public Property lstPledges() As ListView
        Get
            Return _lstPledges
        End Get
        Set(ByVal value As ListView)
            _lstPledges = value
        End Set
    End Property

    Private _pledgeList As List(Of CampaignPledge)
    Public Property pledgeList() As List(Of CampaignPledge)
        Get
            Return _pledgeList
        End Get
        Set(value As List(Of CampaignPledge))
            _pledgeList = value
        End Set
    End Property

    Public _memberId As String
    Public Property memberId() As String
        Get
            Return _memberId
        End Get
        Set(ByVal value As String)
            _memberId = value
        End Set
    End Property
    Public Property campaignComplete As Boolean = False
#End Region


#Region "Handles"
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        'Dim pledgesList As New List(Of Pledges)

        'getAllPledgesByCampaign(CurrentNode, pledgesList, CurrentNode.Name)

        'lstviewPledges.DataSource = pledgesList
        'lstviewPledges.DataBind()

        Try
            If Not IsPostBack Then
                'Instantiate variables
                blPledges = New blPledges
                Dim bReturn As BusinessReturn
                Dim lstCampaignPledges As List(Of CampaignPledge)
                Dim lstCampaignPledges_Phase01 As New List(Of CampaignPledge)
                Dim lstCampaignPledges_Phase02 As New List(Of CampaignPledge)
                Dim lstCampaignPledges_Phase03 As New List(Of CampaignPledge)

                'Obtain all records
                bReturn = blPledges.selectAll_byCampaignId_forEditCampaign(CurrentNodeId)
                If bReturn.DataContainer.Count > 0 Then
                    lstCampaignPledges = bReturn.DataContainer(0)

                    'Show proper msgs
                    If lstCampaignPledges.Count = 0 Then
                        'Show a 'no pledges' msg
                        pnlNoPledges.Visible = True
                    Else
                        'Seperate pledges into phase lists
                        For Each pledge As CampaignPledge In lstCampaignPledges
                            Select Case pledge.phaseId
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
                    End If
                End If

            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\editTab_Pledges.ascx.vb : Page_Load()")


            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Response.Write("Error: " & ex.ToString)
        End Try




    End Sub
#End Region


#Region "Methods"
#End Region
End Class
