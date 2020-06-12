Imports Microsoft.VisualBasic

Public Class FinancialData
    'Instantiate variables
    Public demography As Demographics = New Demographics
    Public Property acceptedTerms As Boolean = False

    Public Sub New()
    End Sub

    Public Class Demographics
        Private _firstName As String = String.Empty
        Private _lastName As String = String.Empty
        Private _email As String = String.Empty

        Private _address1 As String = String.Empty
        Private _address2 As String = String.Empty
        Private _city As String = String.Empty
        Private _state As String = String.Empty
        Private _postal As String = String.Empty
        Private _country As String = String.Empty

        Private _dobMonth As Nullable(Of Integer)
        Private _dobDay As Nullable(Of Integer)
        Private _dobYear As Nullable(Of Integer)

        Public Sub New()
        End Sub

        Public Property firstName() As String
            Get
                Return _firstName
            End Get
            Set(value As String)
                _firstName = value
            End Set
        End Property
        Public Property lastName() As String
            Get
                Return _lastName
            End Get
            Set(value As String)
                _lastName = value
            End Set
        End Property
        Public Property email() As Integer
            Get
                Return _email
            End Get
            Set(value As Integer)
                _email = value
            End Set
        End Property

        Public Property address1() As String
            Get
                Return _address1
            End Get
            Set(value As String)
                _address1 = value
            End Set
        End Property
        Public Property address2() As String
            Get
                Return _address2
            End Get
            Set(value As String)
                _address2 = value
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
        Public Property state() As String
            Get
                Return _state
            End Get
            Set(value As String)
                _state = value
            End Set
        End Property
        Public Property postal() As String
            Get
                Return _postal
            End Get
            Set(value As String)
                _postal = value
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

        Public Property dobMonth() As Integer
            Get
                Return _dobMonth
            End Get
            Set(value As Integer)
                _dobMonth = value
            End Set
        End Property
        Public Property dobDay() As Integer
            Get
                Return _dobDay
            End Get
            Set(value As Integer)
                _dobDay = value
            End Set
        End Property
        Public Property dobYear() As Integer
            Get
                Return _dobYear
            End Get
            Set(value As Integer)
                _dobYear = value
            End Set
        End Property
    End Class
End Class
