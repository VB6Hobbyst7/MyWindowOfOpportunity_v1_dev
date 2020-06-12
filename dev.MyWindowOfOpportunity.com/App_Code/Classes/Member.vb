Imports Microsoft.VisualBasic

Public Class Member
    Public Property generalMsg As String = String.Empty
    Public Property errorMsg As String = String.Empty
    Public Property isValid As Boolean = True

    Public Property isTeamAdmin As Boolean = False
    Public Property isCampaignAdmin As Boolean = False
    Public Property isCampaignMember As Boolean = False

    Public Property Demographics As Demographics = New Demographics
    Public Property BillingInfo As BillingInfo = New BillingInfo
    Public Property ShippingInfo As ShippingInfo = New ShippingInfo
    Public Property MembershipProperties As MembershipProperties = New MembershipProperties
    Public Property PledgeList As List(Of CampaignPledge) = New List(Of CampaignPledge)
    Public Property StripeIDs As StripeIDs = New StripeIDs

    Public Sub New()
    End Sub
End Class


Public Class PreAccountmembers
    Public Property preAcctId() As Integer
        Get
            Return _preAcctId
        End Get
        Set
            _preAcctId = Value
        End Set
    End Property
    Private _preAcctId As Integer

    Public Property firstName() As String
        Get
            Return _firstName
        End Get
        Set
            _firstName = Value
        End Set
    End Property
    Private _firstName As String

    Public Property lastName() As String
        Get
            Return _lastName
        End Get
        Set
            _lastName = Value
        End Set
    End Property
    Private _lastName As String

    Public Property password() As String
        Get
            Return _password
        End Get
        Set
            _password = Value
        End Set
    End Property
    Private _password As String


    Public Property email() As String
        Get
            Return _email
        End Get
        Set
            _email = Value
        End Set
    End Property
    Private _email As String

    Public Property dob() As String
        Get
            Return _dob
        End Get
        Set
            _dob = Value
        End Set
    End Property
    Private _dob As String
End Class
