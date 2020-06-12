Imports Common

Public Class blRewards

#Region "Properties"
    Private linqRewards As linqRewards = New linqRewards
#End Region


#Region "Selects"
    Public Function obtainRewards_byCampaignId(ByVal _campaignId As Integer) As BusinessReturn
        Return linqRewards.obtainRewards_byCampaignId(_campaignId)
    End Function
#End Region

#Region "Inserts"
    Public Function CreateRewardsFolder(ByVal _parentNodeId As Int16) As Int16?
        Return linqRewards.CreateRewardsFolder(_parentNodeId)
    End Function
    Public Function CreateRewardEntry(ByVal _parentNodeId As Int16, ByVal _title As String, ByVal _shortDescription As String,
                                      ByVal _contributionAmount As String, ByVal _available As String, ByVal _claimed As String,
                                      ByVal _estimatedShippingMonth As String, ByVal _estimatedShippingYear As String, ByVal _showOnTimeline As Boolean,
                                      ByVal _mediaId As Integer) As BusinessReturn

        Dim ValidationReturn As New BusinessReturn

        Try

            'Set default values if not provided
            If String.IsNullOrEmpty(_contributionAmount) Then
                _contributionAmount = "0"
            Else
                _contributionAmount = _contributionAmount.Replace("$", "").Replace(",", "")
            End If
            _available = getNumeric(_available)
            _claimed = getNumeric(_claimed)

            'Perform Validation
            ValidationReturn = Validate(_title, _contributionAmount, _available, _claimed)

            'If error occured, return w/ message
            If Not ValidationReturn.isValid Then
                Return ValidationReturn
            Else
                Return linqRewards.CreateRewardEntry(_parentNodeId, _title, _shortDescription, CDbl(_contributionAmount), CInt(_available), CInt(_claimed),
                                                     _estimatedShippingMonth, _estimatedShippingYear, _showOnTimeline, _mediaId)
            End If

        Catch ex As Exception

            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\BusinessLayer\blRewards.vb : CreateRewardEntry()")
            sb.AppendLine("_parentNodeId:" & _parentNodeId)
            sb.AppendLine("_title:" & _title)
            sb.AppendLine("_shortDescription:" & _shortDescription)
            sb.AppendLine("_contributionAmount:" & _contributionAmount)
            sb.AppendLine("_available:" & _available)
            sb.AppendLine("_claimed" & _claimed)
            sb.AppendLine("_estimatedShippingMonth" & _estimatedShippingMonth)
            sb.AppendLine("_estimatedShippingYear" & _estimatedShippingYear)
            sb.AppendLine("_showOnTimeline" & _showOnTimeline)
            sb.AppendLine("_mediaId" & _mediaId)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

            Return ValidationReturn

        End Try

    End Function
#End Region

#Region "Updates"
    Public Function UpdateRewardEntry(ByVal _nodeId As Int16, ByVal _title As String, ByVal _shortDescription As String,
                                      ByVal _contributionAmount As String, ByVal _available As Integer, ByVal _claimed As Integer,
                                      ByVal _estimatedShippingMonth As String, ByVal _estimatedShippingYear As String, ByVal _showOnTimeline As Boolean,
                                      ByVal _mediaId As Integer) As BusinessReturn

        Dim ValidationReturn As New BusinessReturn

        Try
            'Set default goal value if not provided
            If String.IsNullOrEmpty(_contributionAmount) Then
                _contributionAmount = "0"
            Else
                _contributionAmount = _contributionAmount.Replace("$", "").Replace(",", "")
            End If

            'Perform Validation
            ValidationReturn = Validate(_title, _contributionAmount, _available, _claimed)

            'If error occured, return w/ message
            If Not ValidationReturn.isValid Then
                Return ValidationReturn
            Else
                Return linqRewards.UpdateRewardEntry(_nodeId, _title, _shortDescription, _contributionAmount, CInt(_available), CInt(_claimed),
                                                     _estimatedShippingMonth, _estimatedShippingYear, _showOnTimeline, _mediaId)
            End If

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\BusinessLayer\blRewards.vb : UpdateRewardEntry()")
            sb.AppendLine("_nodeId:" & _nodeId)
            sb.AppendLine("_title:" & _title)
            sb.AppendLine("_shortDescription:" & _shortDescription)
            sb.AppendLine("_contributionAmount:" & _contributionAmount)
            sb.AppendLine("_available:" & _available)
            sb.AppendLine("_claimed" & _claimed)
            sb.AppendLine("_estimatedShippingMonth" & _estimatedShippingMonth)
            sb.AppendLine("_estimatedShippingYear" & _estimatedShippingYear)
            sb.AppendLine("_showOnTimeline" & _showOnTimeline)
            sb.AppendLine("_mediaId" & _mediaId)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return ValidationReturn
        End Try

    End Function
    Public Sub UpdateRewardAvailableNumber(ByVal rewardId As Integer)
        linqRewards.UpdateRewardAvailableNumber(rewardId)
    End Sub
#End Region

#Region "Delete"
#End Region

#Region "Methods"
#End Region

#Region "Validations"
    Private Function Validate(ByVal _title As String, ByVal _contributionAmount As String, ByVal _available As String, ByVal _claimed As String) As BusinessReturn
        'instantiate variables
        Dim ValidationMessages As New BusinessReturn()
        Dim int As Integer
        'Dim dbl As Double
        Try
            If String.IsNullOrWhiteSpace(_title) Then ValidationMessages.ValidationMessages.Add(New ValidationContainer("Title cannot be blank."))

            Dim number As Decimal
            If Not Decimal.TryParse(_contributionAmount, number) Then
                'Add failure message to validation
                ValidationMessages.ValidationMessages.Add(New ValidationContainer("Invalid numeric value for goal. " & _contributionAmount))
            End If

            If Not Int32.TryParse(_available, int) Then ValidationMessages.ValidationMessages.Add(New ValidationContainer("Availability must be numeric."))
            If Not Int32.TryParse(_claimed, int) Then ValidationMessages.ValidationMessages.Add(New ValidationContainer("Amount claimed must be numeric."))
            'If (_estimatedShippingMonth = "-1") Then ValidationMessages.ValidationMessages.Add(New ValidationContainer("Select a shipping month."))
            'If (_estimatedShippingYear = "-1") Then ValidationMessages.ValidationMessages.Add(New ValidationContainer("Select a shipping year."))

            If Not ValidationMessages.isValid Then
                'Save all validation messages in umbraco as errors.
                Dim sb As New StringBuilder()
                sb.AppendLine("blRewards.vb : Validate()")
                For Each msg As ValidationContainer In ValidationMessages.ValidationMessages
                    sb.AppendLine(msg.ToString)
                Next
                saveErrorMessage(getLoggedInMember, sb.ToString(), sb.ToString())
            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\BusinessLayer\blRewards.vb : Validate()")
            sb.AppendLine("_title:" & _title)
            sb.AppendLine("_contributionAmount:" & _contributionAmount)
            sb.AppendLine("_available" & _available)
            sb.AppendLine("_claimed" & _claimed)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

        End Try
        Return ValidationMessages
    End Function
#End Region
End Class
