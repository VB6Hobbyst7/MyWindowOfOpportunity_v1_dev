Imports Microsoft.VisualBasic

Public Class StripeIDs
    Private _publicKey As String
    Private _customerId As String
    Private _campaignAcctId As String
    Private _bankAcctToken As String
    Private _bankAcctId As String
    Private _fileUploadId As String
    Private _creditCardId As String
    Private _creditCardToken As String

    Public Property publicKey() As String
        Get
            Return _publicKey
        End Get
        Set(value As String)
            _publicKey = value
        End Set
    End Property
    Public Property customerId() As String
        Get
            Return _customerId
        End Get
        Set(value As String)
            _customerId = value
        End Set
    End Property
    Public Property campaignAcctId() As String
        Get
            Return _campaignAcctId
        End Get
        Set(value As String)
            _campaignAcctId = value
        End Set
    End Property
    Public Property bankAcctToken() As String
        Get
            Return _bankAcctToken
        End Get
        Set(value As String)
            _bankAcctToken = value
        End Set
    End Property
    Public Property bankAcctId() As String
        Get
            Return _bankAcctId
        End Get
        Set(value As String)
            _bankAcctId = value
        End Set
    End Property
    Public Property fileUploadId() As String
        Get
            Return _fileUploadId
        End Get
        Set(value As String)
            _fileUploadId = value
        End Set
    End Property
    Public Property creditCardId() As String
        Get
            Return _creditCardId
        End Get
        Set(value As String)
            _creditCardId = value
        End Set
    End Property
    Public Property creditCardToken() As String
        Get
            Return _creditCardToken
        End Get
        Set(value As String)
            _creditCardToken = value
        End Set
    End Property
End Class
