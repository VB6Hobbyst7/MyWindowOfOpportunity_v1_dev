Imports Common
Public Class blTimelines

#Region "Properties"
    Private linqTimelines As linqTimelines = New linqTimelines
#End Region


#Region "Inserts"
    Public Function CreateTimelinesFolder(ByVal _parentNodeId As Int16) As Int16?
        Return linqTimelines.CreateTimelinesFolder(_parentNodeId)
    End Function
    Public Function CreateTimelineEntry(ByVal _parentNodeId As Int16, ByVal _title As String, ByVal _entryDate As Date,
                                        ByVal _showOnTimeline As Boolean, ByVal _summary As String) As BusinessReturn

        Return linqTimelines.CreateTimelineEntry(_parentNodeId, _title, _entryDate, _showOnTimeline, _summary)
    End Function
#End Region

#Region "Selects"
#End Region

#Region "Updates"
    Public Function UpdateTimelineEntry(ByVal _nodeId As Int16, ByVal _title As String, ByVal _entryDate As String,
                                        ByVal _showOnTimeline As Boolean, ByVal _summary As String) As BusinessReturn
        Dim ValidationReturn As BusinessReturn
        Try
            'Set default values if not provided
            If String.IsNullOrEmpty(_entryDate) Then
                _entryDate = Date.Today
            End If

            'Perform Validation
            ValidationReturn = Validate(_title, _entryDate)

            'If error occured, return w/ message
            If Not ValidationReturn.isValid Then
                Return ValidationReturn
            Else
                Return linqTimelines.UpdateTimelineEntry(_nodeId, _title, _entryDate, _showOnTimeline, _summary)
            End If

        Catch ex As Exception

            Dim sb As New StringBuilder()
            sb.AppendLine("blTimeLines.vb : UpdateTimelineEntry()")
            sb.AppendLine("_title: " & _title)
            sb.AppendLine("_entryDate" & _entryDate)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

            Return Nothing

        End Try
    End Function
#End Region

#Region "Delete"
#End Region

#Region "Methods"
#End Region

#Region "Validations"
    Private Function Validate(ByVal _title As String, ByVal _entryDate As String) As BusinessReturn
        Dim ValidationMessages As New BusinessReturn()

        Try
            If String.IsNullOrWhiteSpace(_title) Then
                ValidationMessages.ValidationMessages.Add(New ValidationContainer("Title cannot be blank."))
            End If

            If Not IsDate(_entryDate) Then
                ValidationMessages.ValidationMessages.Add(New ValidationContainer("Invalid entry date.."))
            End If

        Catch ex As Exception

            Dim sb As New StringBuilder()
            sb.AppendLine("blTimeLines.vb : Validate()")
            sb.AppendLine("_title: " & _title)
            sb.AppendLine("_entryDate" & _entryDate)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

            Return ValidationMessages

        End Try

        Return ValidationMessages
    End Function
#End Region
End Class
