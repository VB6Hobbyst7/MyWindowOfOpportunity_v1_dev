Imports Microsoft.VisualBasic
Imports umbraco.Core.Models

Public Class Transfer
    Public Property id As Integer?
    Public Property activePhase As IPublishedContent
    Public Property activePhaseId As Integer?
    Public Property campaign As IPublishedContent
    Public Property campaignId As Integer?
    Public Property dateToProcess As Date?
    Public Property transferTotal As Integer?
    Public Property notes As String = String.Empty

End Class
