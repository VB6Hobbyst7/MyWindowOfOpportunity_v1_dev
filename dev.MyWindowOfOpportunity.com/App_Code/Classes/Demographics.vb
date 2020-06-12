Imports Microsoft.VisualBasic

Public Class Demographics
    Private _firstName As String = String.Empty
    Private _lastName As String = String.Empty
    Private _dateOfBirth As Date? = Nothing
    Private _photo As Integer = 0
    Private _photoUrl As String = String.Empty
    Private _briefDescription As String = String.Empty
    Private _ssn As String = String.Empty


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
    Public Property dateOfBirth() As Date?
        Get
            Return _dateOfBirth
        End Get
        Set(value As Date?)
            _dateOfBirth = value
        End Set
    End Property
    Public Property photo() As Integer
        Get
            Return _photo
        End Get
        Set(value As Integer)
            _photo = value
        End Set
    End Property
    Public Property photoUrl() As String
        Get
            Return _photoUrl
        End Get
        Set(value As String)
            _photoUrl = value
        End Set
    End Property
    Public Property briefDescription() As String
        Get
            Return _briefDescription
        End Get
        Set(value As String)
            _briefDescription = value
        End Set
    End Property
    Public Property ssn As String
        Get
            Return _ssn
        End Get
        Set(value As String)
            _ssn = value
        End Set
    End Property
End Class
