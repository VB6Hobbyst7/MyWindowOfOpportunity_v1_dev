Imports Common
Imports umbraco.Core
Imports umbraco.Core.Models
Imports umbraco.Core.Services

Partial Class UserControls_editTab_Phases
    Inherits System.Web.UI.UserControl


#Region "Properties"
    'Private _thisNode As IPublishedContent
    'Public Property thisNode() As IPublishedContent
    '    Get
    '        Return _thisNode
    '    End Get
    '    Set(value As IPublishedContent)
    '        _thisNode = value
    '    End Set
    'End Property
    Private _uHelper As Uhelper = New Uhelper()
    Public Property thisNodeId() As String
        Get
            Return hfldThisNodeId.Value
        End Get
        Set(value As String)
            hfldThisNodeId.Value = value.ToString
        End Set
    End Property

    Private blPhases As blPhases = New blPhases
    Public Property campaignComplete As Boolean = False
#End Region

#Region "Handles"
    Private Sub UserControls_editTab_Phases_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not String.IsNullOrEmpty(thisNodeId) Then
            Try
                'Instantiate variables
                Dim thisNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(CInt(thisNodeId))
                Dim phases As IPublishedContent
                Dim phaseIDs As List(Of Integer) = New List(Of Integer)
                Dim BusinessReturn As BusinessReturn
                Dim CampaignStatistics As CampaignStatistics
                Dim blCampaigns As New blCampaigns
                Dim isCharity As Boolean = blCampaigns.IsCampaignACharity_byId(CInt(thisNodeId))
                'Dim refresh As Boolean = False

                'Set hidden field with tab name
                hfldTabName.Value = tabNames.phases

                'Loop thru all children nodes to determine if a Phases IPublishedContent exists.
                For Each childNode As IPublishedContent In thisNode.Children
                    If childNode.DocumentTypeAlias = docTypes.Phases Then
                        phases = childNode
                        Exit For
                    End If
                Next

                'If no phases IPublishedContent was found, create a new IPublishedContent.
                If IsNothing(phases) Then
                    'Create a new phase IPublishedContent
                    phases = _uHelper.Get_IPublishedContentByID(blPhases.CreatePhases(thisNode.Id))
                End If

                'Obtain data
                Dim campaignNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(thisNode.Id)

                '
                If Not isCharity Then
                    BusinessReturn = blCampaigns.obtainStatistics(campaignNode)
                    If BusinessReturn.isValid Then
                        'Obtain campaign data
                        CampaignStatistics = DirectCast(BusinessReturn.DataContainer(0), CampaignStatistics)

                        'To get the status type
                        Dim status As Common.statusType = CampaignStatistics.statusType
                        If status = statusType.DiscoveryPhase Then
                            Dim campaign = _uHelper.Get_IPublishedContentByID(Convert.ToInt32(thisNode.Id))
                            Dim discovery = campaign.Children().Where(Function(x) (x.Name.ToLower() = docTypes.discovery)).FirstOrDefault()
                            If discovery IsNot Nothing Then
                                phaseIDs.Add(discovery.Id)
                            End If
                        End If
                    End If
                End If


                'Get list of all phase IPublishedContent IDs
                For Each childNode As IPublishedContent In phases.Children
                    phaseIDs.Add(childNode.Id)
                    If isCharity Then Exit For 'Charities only get the 1st phase
                Next


                'Add any missing phase nodes
                If isCharity Then
                    'Delete all but 1st phase
                    'Dim first As Boolean = True
                    Dim phase2Keep As IPublishedContent
                    For Each childNode As IPublishedContent In phases.Children
                        'Take only the first published node.
                        phase2Keep = childNode
                        Exit For
                    Next

                    If Not IsNothing(phase2Keep) Then
                        '
                        Dim icService As IContentService = ApplicationContext.Current.Services.ContentService
                        Dim icPhases As IContent = icService.GetById(phases.Id)
                        For Each icChildNode As IContent In icPhases.Children
                            If icChildNode.Id <> phase2Keep.Id Then
                                'Delete phase entry
                                icService.Delete(icChildNode)
                            End If
                        Next
                    End If

                Else
                    'Create missing phases
                    While phaseIDs.Count < 3
                        phaseIDs.Add(blPhases.CreatePhase(phases.Id, phaseIDs.Count + 1))
                        'refresh = True
                    End While
                End If

                'Create a listItem for each id and add it to the rbl
                If Not IsPostBack Then
                    Dim index As Int16 = 1
                    Dim isDiscoveryExists As Boolean = False
                    For Each id As Integer In phaseIDs
                        Dim listItem As ListItem = New ListItem
                        Dim checkPhaseType = _uHelper.Get_IPublishedContentByID(Convert.ToInt32(id))
                        If checkPhaseType.Name = "Discovery" Then
                            listItem.Text = "Discovery Phase"
                            listItem.Attributes.Add("data-discovery", True)
                            isDiscoveryExists = True
                        Else
                            If isDiscoveryExists = True Then
                                listItem.Text = "Phase " & (index - 1).ToString
                            Else
                                listItem.Text = "Phase " & index.ToString
                            End If
                            listItem.Attributes.Add("data-phase", True)
                        End If
                        listItem.Value = id
                        rbl.Items.Add(listItem)
                        index += 1
                    Next
                End If

                'Set first item as active
                rbl.SelectedIndex = 0

                'Add each phase panel to control
                Dim firstPhase As Boolean = True
                For Each id As Integer In phaseIDs
                    Dim phaseEntry As ASP.usercontrols_phaseentry_ascx = New ASP.usercontrols_phaseentry_ascx With {
                        .thisNodeId = id
                    }
                    If _uHelper.Get_IPublishedContentByID(id).DocumentTypeAlias = docTypes.discovery Then
                        phaseEntry.isDiscovery = True
                    Else
                        phaseEntry.isPrimary = firstPhase
                        firstPhase = False 'Set to false so only first phase is marked as primary.
                    End If
                    phPhases.Controls.Add(phaseEntry)
                Next

                'disable if campaign is complete
                If campaignComplete Then lbtnSave.Enabled = False

                'Determine if campaign is a charity
                If isCharity Then
                    pnlCharityInstructions.Visible = True
                    pnlInstructions.Visible = False
                End If

                '
                'If refresh Then ApplicationContext.Current.Services.ContentService.BuildXmlCache()

                'Determine if campaign is unpublished 
                If blCampaigns.obtainStatusType_byId(CInt(thisNodeId)) <> statusType.Unpublished Then
                    lbtnSave.Visible = False
                    pnlPhaseEntries.Enabled = False
                End If
            Catch ex As Exception
                Dim sb As New StringBuilder()
                sb.AppendLine("\UserControls\editTab_Phases.ascx.vb : UserControls_editTab_Phases_Load()")

                saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
                'Response.Write("Error: " & ex.ToString & "<br />")
            End Try
        End If

    End Sub
    Private Sub lbtnSave_Click(sender As Object, e As EventArgs) Handles lbtnSave.Click
        'Loop thru each panel
        'Obtain data for panel
        'submit to save.
        '   show alert


        Try
            '
            Dim submittedSuccessfully As Boolean = True

            'Loop thru each item in the listview
            For Each item As Control In phPhases.Controls
                '
                If TypeOf item Is ASP.usercontrols_phaseentry_ascx Then
                    'Instantiate variables
                    blPhases = New blPhases
                    Dim returnObject As BusinessReturn
                    Dim phaseEntry As ASP.usercontrols_phaseentry_ascx = DirectCast(item, ASP.usercontrols_phaseentry_ascx)

                    If phaseEntry.isDiscovery = False Then
                        'Submit data for updating
                        If phaseEntry.isPrimary Then
                            returnObject = blPhases.UpdatePhase(phaseEntry.thisNodeId, phaseEntry.phaseTitle, True, phaseEntry.phaseGoal, phaseEntry.shortDescription)
                        Else
                            returnObject = blPhases.UpdatePhase(phaseEntry.thisNodeId, phaseEntry.phaseTitle, phaseEntry.published, phaseEntry.phaseGoal, phaseEntry.shortDescription)
                        End If

                        'If unsuccessfull, show error message.
                        If Not returnObject.isValid Then
                            submittedSuccessfully = False
                            'Response.Write("Error: " & returnObject.ExceptionMessage & "<br />")
                        End If

                    End If
                End If
            Next



            'Add alert message as to outcome of submissions.
            Dim alert As ASP.usercontrols_alertmsg_ascx = New ASP.usercontrols_alertmsg_ascx
            If submittedSuccessfully Then
                'Show success msg
                alert.MessageType = UserControls_AlertMsg.msgType.Success
                alert.AlertMsg = "Saved Successfully"
            Else
                'Show alert msg
                alert.MessageType = UserControls_AlertMsg.msgType.Alert
                alert.AlertMsg = "Error. Unable to save data."
            End If
            phAlertMsg.Controls.Add(alert)

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\editTab_Phases.ascx.vb : lbtnSave_Click()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Response.Write(ex.ToString)
        End Try
    End Sub
#End Region

#Region "Methods"
#End Region
End Class