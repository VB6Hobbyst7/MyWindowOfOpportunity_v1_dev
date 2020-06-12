Imports Microsoft.VisualBasic

Public Class Phase
    Public Property id As Integer = 0
    Public Property name As String = String.Empty
    Public Property phaseNumber As Integer? = Nothing

    Public Property published As Boolean = False
    Public Property active As Boolean = False
    Public Property failed As Boolean = False
    Public Property complete As Boolean = False

    Public Property activationDate As Date? = Nothing
    Public Property failedDate As Date? = Nothing
    Public Property completionDate As Date? = Nothing

    Public Property description As String = String.Empty
    Public Property goal As Decimal = 0

    Public Property lstPledges As New List(Of CampaignPledge)
    Public Property pledgeTotal As Decimal = 0
    Public Property pledgeCount As Integer = 0

    Public Property lstPreviousPhases As New List(Of Phase)


    Public Sub New()
    End Sub
End Class