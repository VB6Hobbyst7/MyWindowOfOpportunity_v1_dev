Imports Microsoft.VisualBasic

Public Class ShippingInfo
    Private _address01 As String = String.Empty
    Private _address02 As String = String.Empty
    Private _city As String = String.Empty
    Private _stateProvidence As String = String.Empty
    Private _postalCode As String = String.Empty
    Private _country As String = String.Empty


    Public Property address01() As String
        Get
            Return _address01
        End Get
        Set(value As String)
            _address01 = value
        End Set
    End Property
    Public Property address02() As String
        Get
            Return _address02
        End Get
        Set(value As String)
            _address02 = value
        End Set
    End Property
    Public Property city() As String
        Get
            Return _city
        End Get
        Set(value As String)
            _city = value
        End Set
    End Property
    Public Property stateProvidence() As String
        Get
            Return _stateProvidence
        End Get
        Set(value As String)
            _stateProvidence = value
        End Set
    End Property
    Public Property postalCode() As String
        Get
            Return _postalCode
        End Get
        Set(value As String)
            _postalCode = value
        End Set
    End Property
    Public Property country() As String
        Get
            Return _country
        End Get
        Set(value As String)
            _country = value
        End Set
    End Property
End Class
