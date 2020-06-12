Imports Microsoft.VisualBasic
Imports Common


Public Class CampaignCategories
#Region "Private Variables"
    Private _nodeId As Int32 = 0
    Private _Artistic As Boolean = False
    Private _Business As Boolean = False
    Private _Charity As Boolean = False
    Private _Community As Boolean = False
    Private _Science As Boolean = False
    Private _SelfHelp As Boolean = False
    Private _Software As Boolean = False
    Private _Technology As Boolean = False
#End Region

#Region "Public Properties"
    Public Property nodeId() As Integer
        Get
            Return _nodeId
        End Get
        Set(value As Integer)
            _nodeId = value
        End Set
    End Property
    Public Property Artistic() As Boolean
        Get
            Return _Artistic
        End Get
        Set(value As Boolean)
            _Artistic = value
        End Set
    End Property
    Public Property Business() As Boolean
        Get
            Return _Business
        End Get
        Set(value As Boolean)
            _Business = value
        End Set
    End Property
    Public Property Charity() As Boolean
        Get
            Return _Charity
        End Get
        Set(value As Boolean)
            _Charity = value
        End Set
    End Property
    Public Property Community() As Boolean
        Get
            Return _Community
        End Get
        Set(value As Boolean)
            _Community = value
        End Set
    End Property
    Public Property Science() As Boolean
        Get
            Return _Science
        End Get
        Set(value As Boolean)
            _Science = value
        End Set
    End Property
    Public Property SelfHelp() As Boolean
        Get
            Return _SelfHelp
        End Get
        Set(value As Boolean)
            _SelfHelp = value
        End Set
    End Property
    Public Property Software() As Boolean
        Get
            Return _Software
        End Get
        Set(value As Boolean)
            _Software = value
        End Set
    End Property
    Public Property Technology() As Boolean
        Get
            Return _Technology
        End Get
        Set(value As Boolean)
            _Technology = value
        End Set
    End Property
#End Region

#Region "Methods"
    Public Sub New()
    End Sub
#End Region

End Class



'http://127.0.0.1/Services/CampaignInvitations.ashx?params_campaignInvitation=55K1okHVa5ddioneqKppTKYGrM9N%2fPMm7r3jFdTw78iZe4Zfp41eqjtaHDpt2hJg4%2bfmcd2Exntw%2fKpjRaOBPgASWRmxOlYTf%2f%2bWaHzjrGM%2bL%2fnT%2fpJnozT66I174s91HPabTD%2bgx%2fEQ6ERGpBL%2bjz5YLJFg8ayA0Zv6NUP%2b1ACN5f6BllAi93%2fSydz0Bu8by4reo9%2bC2cHm8iYGjYEFHbKptDqGPyStOOPodHEnqv1pr3ctnFdcQfbKUXrLHxmLpIPlfFUlGQ64VdvRYnPnZHPZHtCGNPwa5zxdsOuDJopTU9Sxkubo3WI1J7j9aK%2fw29EXqPvrj12HO%2ftbSMhhqkoX%2fa0ZBTMrMDkxYm3xP5BExNcVMZt5%2beyYklRs2XdPRa40hVa24OjLNyXGfj%2bv%2b%2fQkZKcf%2fNIiZvXcu3vOVkEml8p8DtjeAHWUv%2fhm%2fpNl
