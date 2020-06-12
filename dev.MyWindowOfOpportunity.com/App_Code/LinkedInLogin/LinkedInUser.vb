Imports Microsoft.VisualBasic

Public Class LinkedInUser
    'Public String firstName { Get; Set; }
    '    Public String headline { Get; Set; }
    '    Public String id { Get; Set; }
    '    Public String lastName { Get; Set; }
    '    Public Sitestandardprofilerequest siteStandardProfileRequest { Get; Set; }

    Private _firstName As String
    Public Property firstName() As String
        Get
            Return _firstName
        End Get
        Set(ByVal value As String)
            _firstName = value
        End Set
    End Property
    Private _headline As String
    Public Property headline() As String
        Get
            Return _headline
        End Get
        Set(ByVal value As String)
            _headline = value
        End Set
    End Property
    Private _id As String
    Public Property id() As String
        Get
            Return _id
        End Get
        Set(ByVal value As String)
            _id = value
        End Set
    End Property

    Private _lastName As String
    Public Property lastName() As String
        Get
            Return _lastName
        End Get
        Set(ByVal value As String)
            _lastName = value
        End Set
    End Property

    Private _siteStandardProfileRequest As Sitestandardprofilerequest
    Public Property siteStandardProfileRequest() As Sitestandardprofilerequest
        Get
            Return _siteStandardProfileRequest
        End Get
        Set(ByVal value As Sitestandardprofilerequest)
            _siteStandardProfileRequest = value
        End Set
    End Property

End Class

Public Class Sitestandardprofilerequest
    Private _url As String
    Public Property url() As String
        Get
            Return _url
        End Get
        Set(ByVal value As String)
            _url = value
        End Set
    End Property

End Class


