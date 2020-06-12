Imports Common
Imports System.Data
Imports umbraco.Core.Models
Imports umbraco.Web

Partial Class UserControls_editTab_Rewards
    Inherits System.Web.UI.UserControl


#Region "Properties"
    Private rewardListNode As IPublishedContent
    Dim _uHelper As Uhelper = New Uhelper()

    'Private _thisNode As IPublishedContent
    'Public Property thisNode() As IPublishedContent
    '    Get
    '        Return _thisNode
    '    End Get
    '    Set(value As IPublishedContent)
    '        _thisNode = value
    '    End Set
    'End Property

    Public Property thisNodeId() As String
        Get
            Return hfldThisNodeId.Value
        End Get
        Set(value As String)
            hfldThisNodeId.Value = value.ToString
        End Set
    End Property

    Private newRewardId As UInt32 = 0
    Public Property campaignComplete As Boolean = False
#End Region

#Region "Handles"
    Private Sub UserControls_editTab_Rewards_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not String.IsNullOrEmpty(thisNodeId) Then
            If Not IsPostBack Then
                Try
                    'Instantiate variables
                    Dim thisNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(CInt(thisNodeId))

                    'Set hidden field with tab name
                    hfldTabName.Value = tabNames.rewards

                    'Save campaign id to minor image selector for retrieving list of campaign's images.
                    ucMinorImageSelector.thisCampaignId = thisNode.Id

                    'Obtain root of timeline list
                    For Each childNode As IPublishedContent In thisNode.Children
                        If childNode.DocumentTypeAlias = docTypes.Rewards Then
                            rewardListNode = childNode
                            Exit For
                        End If
                    Next

                    'If no reward folder exists, create one.
                    If IsNothing(rewardListNode) Then
                        Dim blRewards As blRewards = New blRewards
                        rewardListNode = _uHelper.Get_IPublishedContentByID(blRewards.CreateRewardsFolder(thisNode.Id))
                    End If

                    'Save root timeline IPublishedContent id in session for -1 panel
                    Session(Miscellaneous.rewardRootNodeId) = rewardListNode.Id

                    'Update lists with data
                    updateList()

                    'disable is campaign is complete
                    If campaignComplete Then lbtnSave.Enabled = False

                Catch ex As Exception
                    Dim sb As New StringBuilder()
                    sb.AppendLine("\UserControls\editTab_Rewards.ascx.vb : UserControls_editTab_Rewards_Load()")

                    saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
                    'Response.Write("Error: Timelines- " & ex.ToString)
                End Try
            End If
        End If

    End Sub
    Private Sub UserControls_editTab_Rewards_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        If Not String.IsNullOrEmpty(thisNodeId) Then
            Try
                If IsPostBack Then 'AndAlso (Not IsNothing(Session(Miscellaneous.ActiveTab))) AndAlso (hfldTabName.Value = Session(Miscellaneous.ActiveTab)) Then
                    'Reset hidden field
                    If hfldNewRewardEntry.Value = True Then hfldNewRewardEntry.Value = False

                    'Update lists
                    updateList()

                    'Show msgs if any exist.
                    If Not IsNothing(Session(Miscellaneous.msgType)) Then
                        'Instantaite variables
                        Dim alert = New ASP.usercontrols_alertmsg_ascx

                        'Determine what msg type to show.
                        Select Case Session(Miscellaneous.msgType)
                            Case UserControls_AlertMsg.msgType.Success
                                'Show success msg
                                alert.MessageType = UserControls_AlertMsg.msgType.Success
                                alert.AlertMsg = "Saved Successfully"

                            Case UserControls_AlertMsg.msgType.Alert
                                'Show alert msg
                                alert.MessageType = UserControls_AlertMsg.msgType.Alert
                                alert.AlertMsg = "Error. Unable to save data."
                                'Show error message
                                'Response.Write(Session(Miscellaneous.ExceptionMessage) & "<br />")
                        End Select

                        'Display alert msg
                        phAlertMsg.Controls.Add(alert)

                        'Clear session variables
                        Session(Miscellaneous.msgType) = Nothing
                        Session(Miscellaneous.ExceptionMessage) = Nothing
                    End If
                End If

            Catch ex As Exception
                Dim sb As New StringBuilder()
                sb.AppendLine("\UserControls\editTab_Rewards.ascx.vb : UserControls_editTab_Rewards_PreRender()")


                saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
                'Response.Write("<br />Error: " & ex.ToString)
            End Try
        End If

    End Sub
    Private Sub lbtnSave_Click(sender As Object, e As EventArgs) Handles lbtnSave.Click
        Try
            'Instantiate variables
            Dim returnObject As BusinessReturn = New BusinessReturn
            Dim blRewards As blRewards = New blRewards
            Dim submittedSuccessfully As Boolean = True

            'Reset new id variable
            newRewardId = 0

            '
            If hfldNewRewardEntry.Value = True Then
                'Submit new entry
                returnObject = blRewards.CreateRewardEntry(CShort(Session(Miscellaneous.rewardRootNodeId)), newRewardEntry.heading, newRewardEntry.briefSummary,
                                                            newRewardEntry.contributionAmount, newRewardEntry.quantityAvailable, newRewardEntry.numberClaimed,
                                                            newRewardEntry.month, newRewardEntry.year, newRewardEntry.showOnTimeline, newRewardEntry.featuredImgId)

                'Response.Write("<br />CShort(Session(Miscellaneous.rewardRootNodeId)): " & CShort(Session(Miscellaneous.rewardRootNodeId)))
                'Response.Write("<br />newRewardEntry.heading: " & newRewardEntry.heading)
                'Response.Write("<br />newRewardEntry.briefSummary: " & newRewardEntry.briefSummary)
                'Response.Write("<br />newRewardEntry.contributionAmount: " & newRewardEntry.contributionAmount)
                'Response.Write("<br />newRewardEntry.quantityAvailable: " & newRewardEntry.quantityAvailable)
                'Response.Write("<br />newRewardEntry.numberClaimed: " & newRewardEntry.numberClaimed)
                'Response.Write("<br />newRewardEntry.month: " & newRewardEntry.month)
                'Response.Write("<br />newRewardEntry.year: " & newRewardEntry.year)
                'Response.Write("<br />newRewardEntry.showOnTimeline: " & newRewardEntry.showOnTimeline)
                'Response.Write("<br />newRewardEntry.featuredImgId: " & newRewardEntry.featuredImgId)

                If returnObject.isValid Then
                    'Save newly created IPublishedContent id.
                    newRewardId = CUInt(returnObject.ReturnMessage)
                Else
                    'If unsuccessfull, show error message.
                    submittedSuccessfully = False
                    'Response.Write("Error saving new reward: " & returnObject.ExceptionMessage & "<br />")
                    'gv.DataSource = returnObject.ValidationMessages
                    'gv.DataBind()
                End If
            End If

            'Loop thru each item in the listview
            For Each item As ListViewItem In lvRewardEntries.Items
                'Find usercontrol within listitem
                Dim uc As UserControls_RewardEntry = DirectCast(item.FindControl("RewardEntry"), UserControls_RewardEntry)

                'update entry
                returnObject = blRewards.UpdateRewardEntry(uc.thisNodeId, uc.heading, uc.briefSummary,
                                                        uc.contributionAmount, uc.quantityAvailable, uc.numberClaimed,
                                                        uc.month, uc.year, uc.showOnTimeline, uc.featuredImgId)

                'If unsuccessfull, show error message.
                If Not returnObject.isValid Then
                    submittedSuccessfully = False
                    'Response.Write("Error updating reward: " & returnObject.ExceptionMessage & "<br />")
                    For Each msg As ValidationContainer In returnObject.ValidationMessages
                        'Response.Write("Error updating reward: " & msg.ErrorMessage & "<br />")
                    Next
                End If
            Next

            'Show results
            If returnObject.isValid Then
                Session(Miscellaneous.msgType) = UserControls_AlertMsg.msgType.Success
            Else
                Session(Miscellaneous.msgType) = UserControls_AlertMsg.msgType.Alert
                Session(Miscellaneous.ExceptionMessage) = returnObject.ExceptionMessage
            End If

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\editTab_Rewards.ascx.vb : lbtnSave_Click()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Response.Write("<br />Error Saving: " & ex.ToString & "<br />")
        End Try
    End Sub
#End Region

#Region "Methods"
    Private Sub updateList()
        Try
            'Instantiate variables
            'Dim dtRewards As DataTable = New DataTable
            Dim lstRewards As New List(Of Reward)
            Dim lstSortedRewards As New List(Of Reward)


            If IsNothing(rewardListNode) Then
                'Obtain root of timeline list
                'thisNode = New IPublishedContent(thisNode.Id)
                Dim thisNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(CInt(thisNodeId))
                For Each childNode As IPublishedContent In thisNode.Children
                    If childNode.DocumentTypeAlias = docTypes.Rewards Then
                        rewardListNode = childNode
                        Exit For
                    End If
                Next
            End If


            If Not IsNothing(rewardListNode) Then
                'Does any timeline entries exist
                'If rewardListNode.Children.Count > 0 Then
                '    'Obtain list of timeline nodes as datatable
                '    dtRewards = rewardListNode.Children

                '    'sort datatable
                '    dtRewards.DefaultView.Sort = nodeProperties.contribution_Amount & " " & Miscellaneous.asc
                'End If

                For Each ipReward As IPublishedContent In rewardListNode.Children
                    Dim reward As New Reward
                    reward.nodeId = ipReward.Id
                    reward.rewardTitle = ipReward.Name
                    reward.contributionAmount = ipReward.GetPropertyValue(Of Double)(nodeProperties.contributionAmount)
                    lstRewards.Add(reward)
                Next
            End If

            'Sort list
            lstSortedRewards = lstRewards.OrderBy(Function(x) x.contributionAmount).ToList


            ''
            'If dtRewards.Rows.Count > 0 Then
            '    'Add list to listview
            '    lvRewardEntries.Items.Clear()
            '    lvRewardEntries.DataSource = dtRewards.DefaultView.ToTable
            '    lvRewardEntries.DataBind()

            '    'Add entries to list of buttons
            '    lvRewardBtns.Items.Clear()
            '    lvRewardBtns.DataSource = dtRewards.DefaultView.ToTable
            '    lvRewardBtns.DataBind()
            'End If

            'gv.DataSource = dtRewards.DefaultView.ToTable
            'gv.DataBind()

            '
            'Add list to listview
            lvRewardEntries.Items.Clear()
            lvRewardEntries.DataSource = lstSortedRewards
            lvRewardEntries.DataBind()

            'Add entries to list of buttons
            lvRewardBtns.Items.Clear()
            lvRewardBtns.DataSource = lstSortedRewards
            lvRewardBtns.DataBind()

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("editTab_Rewards.ascx.vb : updateList()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
    End Sub
#End Region
End Class