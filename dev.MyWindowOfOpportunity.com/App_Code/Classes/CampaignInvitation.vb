Imports Microsoft.VisualBasic


Public Class CampaignInvitation
    Private _campaignId As Integer
    Private _dateSubmitted As DateTime
    Private _roleValue As String = String.Empty
    Private _email As String = String.Empty


    Public Property campaignId() As Integer
        Get
            Return _campaignId
        End Get
        Set(value As Integer)
            _campaignId = value
        End Set
    End Property
    Public Property dateSubmitted() As DateTime
        Get
            Return _dateSubmitted
        End Get
        Set(value As DateTime)
            _dateSubmitted = value
        End Set
    End Property

    Public Property roleValue() As String
        Get
            Return _roleValue
        End Get
        Set(value As String)
            _roleValue = value
        End Set
    End Property
    Public Property email() As String
        Get
            Return _email
        End Get
        Set(value As String)
            _email = value
        End Set
    End Property

    Public Sub New()
    End Sub
End Class

'http://127.0.0.1/Services/CampaignInvitations.ashx?params_campaignInvitation=55K1okHVa5ddioneqKppTKYGrM9N%2fPMm7r3jFdTw78iZe4Zfp41eqjtaHDpt2hJg4%2bfmcd2Exntw%2fKpjRaOBPgASWRmxOlYTf%2f%2bWaHzjrGM%2bL%2fnT%2fpJnozT66I174s91HPabTD%2bgx%2fEQ6ERGpBL%2bjz5YLJFg8ayA0Zv6NUP%2b1ACN5f6BllAi93%2fSydz0Bu8by4reo9%2bC2cHm8iYGjYEFHbKptDqGPyStOOPodHEnqv1pr3ctnFdcQfbKUXrLHxmLpIPlfFUlGQ64VdvRYnPnZHPZHtCGNPwa5zxdsOuDJopTU9Sxkubo3WI1J7j9aK%2fw29EXqPvrj12HO%2ftbSMhhqkoX%2fa0ZBTMrMDkxYm3xP5BExNcVMZt5%2beyYklRs2XdPRa40hVa24OjLNyXGfj%2bv%2b%2fQkZKcf%2fNIiZvXcu3vOVkEml8p8DtjeAHWUv%2fhm%2fpNl
