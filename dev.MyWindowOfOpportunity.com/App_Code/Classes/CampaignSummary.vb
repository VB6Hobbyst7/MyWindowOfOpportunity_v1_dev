Imports Common


'Represents a mini campaign panel found in the trending now and campaign list pages.
Public Class CampaignSummary
    Public Property nodeId() As UInt32
    Public Property campaignUrl() As String
    Public Property cheer() As String
    Public Property currentlyPledged() As Decimal
    Public Property daysRemaining() As UInt16 = 0
    Public Property imageUrl() As String
    Public Property percentFunded() As Decimal = 0F
    Public Property shortDescription() As String
    Public Property team() As String
    Public Property title() As String
    Public Property statusType As statusType = statusType.Unpublished
    Public Property activePhaseId As Integer?
    Public Property _Date() As Date
    Public Property Completed() As Boolean
    Public Property Failed() As Boolean
    Public Property phaseCount() As UInt16 = 0


    Public Sub New()
    End Sub

End Class
