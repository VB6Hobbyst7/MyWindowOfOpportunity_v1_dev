Imports Microsoft.VisualBasic

Public Class ContactUs
    Public Property personName() As String
        Get
            Return _personName
        End Get
        Set
            _personName = Value
        End Set
    End Property
    Private _personName As String

    Public Property email() As String
        Get
            Return _email
        End Get
        Set
            _email = Value
        End Set
    End Property
    Private _email As String

    Public Property phoneNumber() As String
        Get
            Return _phoneNumber
        End Get
        Set
            _phoneNumber = Value
        End Set
    End Property
    Private _phoneNumber As String
    Public Property subject() As String
        Get
            Return _subject
        End Get
        Set
            _subject = Value
        End Set
    End Property
    Private _subject As String

    Public Property message() As String
        Get
            Return _message
        End Get
        Set
            _message = Value
        End Set
    End Property
    Private _message As String

    Public Property addedDate() As DateTime
        Get
            Return _addedDate
        End Get
        Set
            _addedDate = Value
        End Set
    End Property
    Private _addedDate As DateTime
End Class
