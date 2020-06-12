Imports Common
Imports umbraco.Core
Imports umbraco.Core.Models
Imports umbraco.Core.Services
Imports umbraco.Core.Publishing

Public Class linqTimelines

#Region "Properties"
    Private linq2Db As linq2SqlDataContext = New linq2SqlDataContext(ConfigurationManager.ConnectionStrings("umbracoDbDSN").ToString)
#End Region

#Region "Selects"
#End Region

#Region "Inserts"
    Public Function CreateTimelinesFolder(ByVal _parentNodeId As Int16) As Int16?
        'Create a new node
        Try
            Dim cs As IContentService = ApplicationContext.Current.Services.ContentService
            Dim campaign As IContent = cs.GetById(_parentNodeId)
            Dim timeline As IContent = cs.CreateContentWithIdentity(docTypes.Timeline, campaign, docTypes.Timeline)
            cs.SaveAndPublishWithStatus(timeline)
            'Return new node's Id
            Return timeline.Id

        Catch ex As Exception

            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\linqQueries\linqTimelines.vb : CreateTimelinesFolder()")
            sb.AppendLine("_parentNodeId:" & _parentNodeId)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

        End Try

    End Function
    Public Function CreateTimelineEntry(ByVal _parentNodeId As Int16, ByVal _title As String, ByVal _entryDate As Date,
                                        ByVal _showOnTimeline As Boolean, ByVal _summary As String) As BusinessReturn
        'Instantiate business return 
        Dim BusinessReturn As New BusinessReturn()

        Try
            'Instantiate variables
            Dim cs As IContentService = ApplicationContext.Current.Services.ContentService
            Dim timelines As IContent = cs.GetById(_parentNodeId)
            Dim timelineEntry As IContent = cs.CreateContentWithIdentity(_title, timelines, docTypes.TimelineEntry)
            Dim updateResponse As Attempt(Of PublishStatus)

            'Set default values
            timelineEntry.SetValue(nodeProperties.entryDate, _entryDate)
            timelineEntry.SetValue(nodeProperties.showOnTimeline, _showOnTimeline)
            timelineEntry.SetValue(nodeProperties.summary, _summary)

            'Save
            updateResponse = cs.SaveAndPublishWithStatus(timelineEntry)

            'Return new node's Id
            If (updateResponse.Success) Then
                BusinessReturn.ReturnMessage = timelineEntry.Id
                Return BusinessReturn
            Else
                BusinessReturn.ExceptionMessage = updateResponse.Exception.ToString
                Return BusinessReturn
            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\linqQueries\linqTimelines.vb : CreateTimelineEntry()")
            sb.AppendLine("_parentNodeId:" & _parentNodeId)
            sb.AppendLine("_title:" & _title)
            sb.AppendLine("_entryDate:" & _entryDate)
            sb.AppendLine("_showOnTimeline:" & _showOnTimeline)
            sb.AppendLine("_summary:" & _summary)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            BusinessReturn.ExceptionMessage = ex.ToString
            Return BusinessReturn
        End Try
    End Function
#End Region

#Region "Updates"
    Public Function UpdateTimelineEntry(ByVal _nodeId As Int16, ByVal _title As String, ByVal _entryDate As String,
                                        ByVal _showOnTimeline As Boolean, ByVal _summary As String) As BusinessReturn
        'Instantiate variables
        Dim cs As IContentService = ApplicationContext.Current.Services.ContentService
        Dim updateResponse As Attempt(Of PublishStatus)
        Dim BusinessReturn As New BusinessReturn()
        Try
            Dim timelineEntry As IContent = cs.GetById(_nodeId)
            'Set values 
            timelineEntry.Name = _title
            timelineEntry.SetValue(nodeProperties.entryDate, CDate(_entryDate))
            timelineEntry.SetValue(nodeProperties.showOnTimeline, _showOnTimeline)
            timelineEntry.SetValue(nodeProperties.summary, _summary)
            'Save values
            updateResponse = cs.SaveAndPublishWithStatus(timelineEntry)
            If (updateResponse.Success) Then
                BusinessReturn.ReturnMessage = timelineEntry.Id
                Return BusinessReturn
            Else
                BusinessReturn.ExceptionMessage = updateResponse.Exception.ToString
                Return BusinessReturn
            End If

        Catch ex As Exception

            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\linqQueries\linqTimelines.vb : UpdateTimelineEntry()")
            sb.AppendLine("_parentNodeId:" & _nodeId)
            sb.AppendLine("_title:" & _title)
            sb.AppendLine("_entryDate:" & _entryDate)
            sb.AppendLine("_showOnTimeline:" & _showOnTimeline)
            sb.AppendLine("_summary:" & _summary)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            BusinessReturn.ExceptionMessage = ex.ToString
            Return BusinessReturn

        End Try
    End Function
#End Region

#Region "Delete"
#End Region

#Region "Private Methods"
#End Region

End Class

