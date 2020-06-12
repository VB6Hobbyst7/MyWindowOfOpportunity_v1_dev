Imports Common
Imports System.Data
Imports umbraco.Core.Models
Imports umbraco.Web

Partial Class UserControls_editTab_Timeline
    Inherits System.Web.UI.UserControl


#Region "Properties"
    Private timelineListNode As IPublishedContent

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

    Private newTimelineId As UInt32 = 0
    Private _uHelper As New Uhelper()
    Public Property campaignComplete As Boolean = False
#End Region

#Region "Handles"
    Private Sub UserControls_editTab_Timeline_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not String.IsNullOrEmpty(thisNodeId) Then
            If Not IsPostBack Then
                Try
                    'Instantiate variables
                    Dim thisNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(CInt(thisNodeId))

                    'Set hidden field with tab name
                    hfldTabName.Value = tabNames.timeline

                    'Obtain root of timeline list
                    For Each childNode As IPublishedContent In thisNode.Children
                        If childNode.DocumentTypeAlias = docTypes.Timeline Then
                            timelineListNode = childNode
                            Exit For
                        End If
                    Next

                    'If no timeline folder exists, create one.
                    If IsNothing(timelineListNode) Then
                        Dim blTimelines As blTimelines = New blTimelines
                        timelineListNode = _uHelper.Get_IPublishedContentByID(blTimelines.CreateTimelinesFolder(thisNode.Id))
                    End If

                    'Save root timeline IPublishedContent id in session for -1 panel
                    Session(Miscellaneous.timelineRootNodeId) = timelineListNode.Id

                    'Update lists with data
                    updateList()

                Catch ex As Exception
                    Dim sb As New StringBuilder()
                    sb.AppendLine("\UserControls\editTab_Timeline.ascx.vb : UserControls_editTab_Timeline_Load()")
                    saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
                    'Response.Write("Error: Timelines- " & ex.ToString)
                End Try
            End If
        End If
    End Sub
    Private Sub UserControls_editTab_Timeline_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        If Not String.IsNullOrEmpty(thisNodeId) Then
            Try
                If IsPostBack Then 'AndAlso (Not IsNothing(Session(Miscellaneous.ActiveTab))) AndAlso (hfldTabName.Value = Session(Miscellaneous.ActiveTab)) Then
                    'Reset hidden field
                    If hfldNewTimelineEntry.Value = True Then hfldNewTimelineEntry.Value = False

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
                                'Response.Write("ExceptionMessage: " & Session(Miscellaneous.ExceptionMessage) & "<br />")
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
                sb.AppendLine("\UserControls\editTab_Timeline.ascx.vb : UserControls_editTab_Timeline_PreRender()")
                saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
                'Response.Write("<br />Error: " & ex.ToString)
            End Try
        End If

    End Sub
    Private Sub lbtnSave_Click(sender As Object, e As EventArgs) Handles lbtnSaveTimelineContent.Click
        Try
            'Instantiate variables
            Dim returnObject As BusinessReturn = New BusinessReturn
            Dim blTimelines As blTimelines = New blTimelines
            'Dim submittedSuccessfully As Boolean = True

            If hfldNewTimelineEntry.Value = True Then
                'Submit data for updating
                returnObject = blTimelines.CreateTimelineEntry(CShort(Session(Miscellaneous.timelineRootNodeId)), newTimelineEntry.entryTitle, newTimelineEntry.entryDate, newTimelineEntry.isActive, newTimelineEntry.editorText)


                'Response.Write("Session(Miscellaneous.timelineRootNodeId): " & Session(Miscellaneous.timelineRootNodeId) & "<br>")
                'Response.Write("newTimelineEntry.entryTitle: " & newTimelineEntry.entryTitle & "<br>")
                'Response.Write("newTimelineEntry.entryDate: " & newTimelineEntry.entryDate & "<br>")
                'Response.Write("newTimelineEntry.isActive: " & newTimelineEntry.isActive & "<br>")
                'Response.Write("newTimelineEntry.editorText: " & newTimelineEntry.editorText & "<br>")
                'Response.Write("===================<br>")

                If returnObject.isValid Then
                    'Save newly created IPublishedContent id.
                    newTimelineId = CUInt(returnObject.ReturnMessage)
                    'Else
                    '    'If unsuccessfull, show error message.
                    '    submittedSuccessfully = False
                    '    'Response.Write("Error saving new timeline: " & returnObject.ExceptionMessage & "<br />")
                End If
            End If

            'Loop thru each item in the listview
            For Each item As ListViewItem In lvTimelineEntries.Items
                'Find usercontrol within listitem
                Dim uc As UserControls_TimelineEntry = DirectCast(item.FindControl("TimelineEntry"), UserControls_TimelineEntry)

                'update entry
                returnObject = blTimelines.UpdateTimelineEntry(uc.thisNodeId, uc.entryTitle, uc.entryDate, uc.isActive, uc.editorText)

                'Response.Write("uc.thisNodeId: " & uc.thisNodeId & "<br>")
                'Response.Write("uc.entryTitle: " & uc.entryTitle & "<br>")
                'Response.Write("uc.entryDate: " & uc.entryDate & "<br>")
                'Response.Write("uc.isActive: " & uc.isActive & "<br>")
                'Response.Write("uc.editorText: " & uc.editorText & "<br>")
                'Response.Write("===================<br>")

                ''If unsuccessfull Then, show Error message.
                'If Not returnObject.isValid Then
                '    submittedSuccessfully = False
                '    'Response.Write("Error updating timeline: " & returnObject.ExceptionMessage & "<br />")
                'End If
            Next

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("editTab_Timeline.ascx.vb : lbtnSave_Click()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Response.Write("Error: " & ex.ToString)
        End Try

        Response.Write("Clicked @ " & DateTime.Now.ToLongTimeString)
    End Sub
#End Region

#Region "Methods"
    Private Sub updateList()
        Try
            'Instantiate variables
            'Dim dtTimelines As DataTable = New DataTable
            'Dim lstTimelines As IEnumerable(Of IPublishedContent)
            Dim lstTimelineEntrys As New List(Of TimelineEntry)
            Dim lstSortedTimelineEntrys As New List(Of TimelineEntry)
            Dim thisNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(CInt(thisNodeId))

            If IsNothing(timelineListNode) Then
                'Obtain root of timeline list
                thisNode = _uHelper.Get_IPublishedContentByID(thisNode.Id)
                For Each childNode As IPublishedContent In thisNode.Children
                    If childNode.DocumentTypeAlias = docTypes.Timeline Then
                        timelineListNode = childNode
                        Exit For
                    End If
                Next
            End If

            If Not IsNothing(timelineListNode) Then
                'Does any timeline entries exist
                'If timelineListNode.Children.Count > 0 Then
                '    ''Obtain list of timeline nodes 
                '    'dtTimelines = timelineListNode.Children

                '    ''sort datatable
                '    'dtTimelines.DefaultView.Sort = nodeProperties.Entry_Date & " " & Miscellaneous.asc
                'End If
                For Each ipTimeline As IPublishedContent In timelineListNode.Children
                    Dim timelineEntry As New TimelineEntry
                    timelineEntry.id = ipTimeline.Id
                    timelineEntry.title = ipTimeline.Name
                    timelineEntry.entryDate = ipTimeline.GetPropertyValue(Of Date)(nodeProperties.entryDate)
                    timelineEntry.showOnTimeline = ipTimeline.GetPropertyValue(Of Boolean)(nodeProperties.showOnTimeline)
                    timelineEntry.summary = ipTimeline.GetPropertyValue(Of Date)(nodeProperties.summary)

                    lstTimelineEntrys.Add(timelineEntry)
                Next
            End If

            'Sort list
            lstSortedTimelineEntrys = lstTimelineEntrys.OrderBy(Function(x) x.entryDate).ToList

            'If dtTimelines.Rows.Count > 0 Then
            'If lstTimelineEntrys.Count > 0 Then
            ''Add entries to list of buttons
            'lvTimelineBtns.Items.Clear()
            'lvTimelineBtns.DataSource = dtTimelines.DefaultView.ToTable
            'lvTimelineBtns.DataBind()

            ''Add list to listview
            'lvTimelineEntries.Items.Clear()
            'lvTimelineEntries.DataSource = dtTimelines.DefaultView.ToTable
            'lvTimelineEntries.DataBind()

            lvTimelineBtns.Items.Clear()
            lvTimelineBtns.DataSource = lstSortedTimelineEntrys
            lvTimelineBtns.DataBind()

            'Add list to listview
            lvTimelineEntries.Items.Clear()
            lvTimelineEntries.DataSource = lstSortedTimelineEntrys
            lvTimelineEntries.DataBind()


            'gv.DataSource = lstSortedTimelineEntrys
            'gv.DataBind()

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("editTab_Timeline.ascx.vb : updateList()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Response.Write("<br />Update Error: " & ex.ToString & "<br />")
        End Try
    End Sub
#End Region
End Class