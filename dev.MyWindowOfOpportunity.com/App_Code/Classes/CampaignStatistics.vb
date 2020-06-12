Imports Microsoft.VisualBasic
Imports Common


Public Class CampaignStatistics
    Public Property campaignId As Integer? = Nothing
    Public Property contributions As Integer = 0
    Public Property funded As Integer = 0
    Public Property campaignGoal As Double = 0F
    Public Property currentPledges As Double = 0F
    Public Property complete As Boolean = False
    Public Property published As Boolean = False
    Public Property completionDate As Date
    Public Property publishDate As Date
    Public Property previousStatusType As statusType = statusType.none
    Public Property statusType As statusType = statusType.Unpublished
    Public Property DiscoveryPhase As New DiscoveryPhase
    Public Property lstPhases As New List(Of Phase)

    Public Property stripeUserIdProvided As Boolean = False
    Public Property stripeUserId As String = String.Empty
    'Public Property stripeClientId As String = String.Empty
    'Public Property stripePublishableKey As String = String.Empty
    'Public Property accessToken As String = String.Empty
    'Public Property refreshToken As String = String.Empty


#Region "Methods"
    Public Sub New()
    End Sub
#End Region


End Class



