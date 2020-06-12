Imports umbraco
Imports Common
Imports umbraco.Core
Imports umbraco.Core.Models
Imports umbraco.Core.Services
Imports umbraco.Core.Publishing
Imports umbraco.Web

Public Class linqRewards

#Region "Properties"
    'Private linq2Db As linq2SqlDataContext = New linq2SqlDataContext(ConfigurationManager.ConnectionStrings("umbracoDbDSN").ToString)
    Dim _uHelper As Uhelper = New Uhelper()
#End Region

#Region "Selects"
    Public Function obtainRewards_byCampaignId(ByVal _campaignId As Integer) As BusinessReturn
        'Master variables
        Dim businessReturn As New BusinessReturn

        Try
            'Instantiate variables
            Dim reward As Reward
            Dim lst As New List(Of Reward)
            Dim rewardsNode As IPublishedContent = GetRewardFolder(_campaignId)

            'Loop thru rewards folder
            For Each childNode As IPublishedContent In rewardsNode.Children
                'Intantiate new reward
                'Obtain data
                reward = New Reward With {
                    .nodeId = childNode.Id,
                    .rewardTitle = childNode.Name
                }
                If childNode.HasProperty(nodeProperties.contributionAmount) Then reward.contributionAmount = childNode.GetPropertyValue(Of Double)(nodeProperties.contributionAmount)
                If childNode.HasProperty(nodeProperties.featuredImage) Then
                    reward.featuredImgName = getMediaName(childNode.GetPropertyValue(Of String)(nodeProperties.featuredImage))
                    reward.featuredImgUrl = getMediaURL(childNode.GetPropertyValue(Of String)(nodeProperties.featuredImage))
                End If

                If childNode.HasProperty(nodeProperties.claimed) Then reward.quantityClaimed = childNode.GetPropertyValue(Of String)(nodeProperties.claimed)

                If childNode.HasProperty(nodeProperties.available) Then
                    reward.quantityAvailable = childNode.GetPropertyValue(Of String)(nodeProperties.available)
                    If reward.quantityAvailable > 0 Then reward.isAvailable = True
                End If

                If childNode.HasProperty(nodeProperties.shortDescription) Then reward.shortDescription = childNode.GetPropertyValue(Of String)(nodeProperties.shortDescription)
                If childNode.HasProperty(nodeProperties.showOnTimeline) Then reward.showOnTimeline = childNode.GetPropertyValue(Of Boolean)(nodeProperties.showOnTimeline)



                If childNode.HasProperty(nodeProperties.estimatedShippingMonth) Then reward.estimatedShippingMonth = getPreValueEntry(prevalues.Dropdown_MonthsAbreviated, childNode.GetPropertyValue(Of String)(nodeProperties.estimatedShippingMonth))
                If childNode.HasProperty(nodeProperties.estimatedShippingYear) Then reward.estimatedShippingYear = getPreValueEntry(prevalues.Dropdown_Years, childNode.GetPropertyValue(Of String)(nodeProperties.estimatedShippingYear))

                'Add reward to list
                lst.Add(reward)
            Next

            '
            businessReturn.DataContainer.Add(lst)

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqRewards.vb: obtainRewards_byCampaignId()")
            sb.AppendLine("_campaignId:" & _campaignId)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

            businessReturn.ExceptionMessage = "Error [linqRewards.vb]: " & ex.ToString
        End Try

        Return businessReturn
    End Function
#End Region

#Region "Inserts"
    Public Function CreateRewardsFolder(ByVal _parentNodeId As Int16) As Int16
        Try
            'Create a new IPublishedContent
            Dim cs As IContentService = ApplicationContext.Current.Services.ContentService
            Dim campaign As IContent = cs.GetById(_parentNodeId)
            Dim rewards As IContent = cs.CreateContentWithIdentity(docTypes.Rewards, campaign, docTypes.Rewards)
            cs.SaveAndPublishWithStatus(rewards)
            'Return new IPublishedContent's Id
            Return rewards.Id
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqRewards.vb : CreateRewardsFolder()")
            sb.AppendLine("_parentNodeId:" & _parentNodeId)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return 0
        End Try

    End Function
    Public Function CreateRewardEntry(ByVal _parentNodeId As Int16, ByVal _title As String, ByVal _shortDescription As String,
                                      ByVal _contributionAmount As String, ByVal _available As Integer, ByVal _claimed As Integer,
                                      ByVal _estimatedShippingMonth As String, ByVal _estimatedShippingYear As String, ByVal _showOnTimeline As Boolean,
                                      ByVal _mediaId As Integer) As BusinessReturn
        'Create a new  IPublishedContent
        Dim BusinessReturn As New BusinessReturn()
        Try

            Dim cs As IContentService = ApplicationContext.Current.Services.ContentService
            Dim rewards As IContent = cs.GetById(_parentNodeId)
            Dim rewardEntry As IContent = cs.CreateContentWithIdentity(_title, rewards, docTypes.Reward)
            Dim updateResponse As Attempt(Of PublishStatus)

            'Set default values
            rewardEntry.Name = _title
            rewardEntry.SetValue(nodeProperties.shortDescription, _shortDescription)
            rewardEntry.SetValue(nodeProperties.contributionAmount, _contributionAmount)
            rewardEntry.SetValue(nodeProperties.available, _available)
            rewardEntry.SetValue(nodeProperties.claimed, _claimed)
            rewardEntry.SetValue(nodeProperties.estimatedShippingMonth, _estimatedShippingMonth)
            rewardEntry.SetValue(nodeProperties.estimatedShippingYear, _estimatedShippingYear)
            rewardEntry.SetValue(nodeProperties.showOnTimeline, _showOnTimeline)
            rewardEntry.SetValue(nodeProperties.featuredImage, _mediaId)
            'Save new reward entry
            updateResponse = cs.SaveAndPublishWithStatus(rewardEntry)

            'Return new IPublishedContent's Id
            If (updateResponse.Success) Then
                BusinessReturn.ReturnMessage = rewardEntry.Id
                Return BusinessReturn
            Else
                BusinessReturn.ExceptionMessage = updateResponse.Exception.ToString
                Return BusinessReturn
            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqRewards.vb : CreateRewardEntry()")
            sb.AppendLine("_title:" & _title)
            sb.AppendLine("_shortDescription:" & _shortDescription)
            sb.AppendLine("_mediaId:" & _mediaId)
            sb.AppendLine("_contributionAmount:" & _contributionAmount)
            sb.AppendLine("_available:" & _available)
            sb.AppendLine("_claimed:" & _claimed)
            sb.AppendLine("_estimatedShippingMonth:" & _estimatedShippingMonth)
            sb.AppendLine("_estimatedShippingYear:" & _estimatedShippingYear)
            sb.AppendLine("_showOnTimeline:" & _showOnTimeline)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return BusinessReturn
        End Try
    End Function
#End Region

#Region "Updates"
    Public Function UpdateRewardEntry(ByVal _nodeId As Int16, ByVal _title As String, ByVal _shortDescription As String,
                                      ByVal _contributionAmount As String, ByVal _available As Integer, ByVal _claimed As Integer,
                                      ByVal _estimatedShippingMonth As String, ByVal _estimatedShippingYear As String, ByVal _showOnTimeline As Boolean,
                                      ByVal _mediaId As Integer) As BusinessReturn
        '
        Dim BusinessReturn As New BusinessReturn()

        Try
            'Instantiate variables
            Dim cs As IContentService = ApplicationContext.Current.Services.ContentService
            Dim rewardEntry As IContent = cs.GetById(_nodeId)
            Dim updateResponse As Attempt(Of PublishStatus)

            'Set values 
            rewardEntry.Name = _title
            rewardEntry.SetValue(nodeProperties.shortDescription, _shortDescription)
            rewardEntry.SetValue(nodeProperties.contributionAmount, _contributionAmount)
            rewardEntry.SetValue(nodeProperties.available, _available)
            rewardEntry.SetValue(nodeProperties.claimed, _claimed)
            rewardEntry.SetValue(nodeProperties.estimatedShippingMonth, _estimatedShippingMonth)
            rewardEntry.SetValue(nodeProperties.estimatedShippingYear, _estimatedShippingYear)
            rewardEntry.SetValue(nodeProperties.showOnTimeline, _showOnTimeline)
            rewardEntry.SetValue(nodeProperties.featuredImage, _mediaId)
            'Save values
            updateResponse = cs.SaveAndPublishWithStatus(rewardEntry)
            If (updateResponse.Success) Then
                BusinessReturn.ReturnMessage = rewardEntry.Id
                Return BusinessReturn
            Else
                BusinessReturn.ExceptionMessage = updateResponse.Exception.ToString
                Return BusinessReturn
            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqRewards.vb : UpdateRewardAvailableNumber()")
            sb.AppendLine("_nodeId:" & _nodeId)
            sb.AppendLine("_title:" & _title)
            sb.AppendLine("_shortDescription:" & _shortDescription)
            sb.AppendLine("_contributionAmount:" & _contributionAmount)
            sb.AppendLine("_available:" & _available)
            sb.AppendLine("_claimed:" & _claimed)
            sb.AppendLine("_estimatedShippingMonth:" & _estimatedShippingMonth)
            sb.AppendLine("_estimatedShippingYear:" & _estimatedShippingYear)
            sb.AppendLine("_showOnTimeline:" & _showOnTimeline)
            sb.AppendLine("_mediaId:" & _mediaId)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

            BusinessReturn.ExceptionMessage = ex.ToString
            Return BusinessReturn
        End Try
    End Function
    Public Sub UpdateRewardAvailableNumber(ByVal rewardId As Integer)
        Try
            If rewardId > 0 Then
                'Instantiate variables
                Dim cs As IContentService = ApplicationContext.Current.Services.ContentService
                Dim rewardEntry As IContent = cs.GetById(rewardId)
                'Dim updateResponse As Attempt(Of PublishStatus)
                Dim BusinessReturn As New BusinessReturn()
                Dim RewardAviableNumber As Integer = CInt(rewardEntry.GetValue(nodeProperties.available))
                Dim RewardClaimedNumber As Integer = CInt(rewardEntry.GetValue(nodeProperties.claimed))
                RewardAviableNumber -= 1
                RewardClaimedNumber += 1

                rewardEntry.SetValue(nodeProperties.available, RewardAviableNumber)
                rewardEntry.SetValue(nodeProperties.claimed, RewardClaimedNumber)

                '
                cs.SaveAndPublishWithStatus(rewardEntry)
                cs.Save(rewardEntry)
            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqRewards.vb : UpdateRewardAvailableNumber()")
            sb.AppendLine("rewardId:" & rewardId)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
    End Sub
#End Region

#Region "Delete"
#End Region

#Region "Private Methods"
    Private Function GetRewardFolder(ByVal _campaignId As Integer) As IPublishedContent
        Try   'Instantiate variables
            Dim campaignNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(_campaignId)

            'Loop thru child nodes to locate the reward folder
            For Each childNode As IPublishedContent In campaignNode.Children
                If childNode.DocumentTypeAlias = docTypes.Rewards Then Return childNode
            Next

            'If a reward folder is not found, create one and return folder IPublishedContent.
            Return _uHelper.Get_IPublishedContentByID(CreateRewardsFolder(_campaignId))
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqRewards.vb : GetRewardFolder()")
            sb.AppendLine("_campaignId:" & _campaignId)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return Nothing
        End Try
    End Function
#End Region

End Class

