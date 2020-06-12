Imports Microsoft.VisualBasic
Imports umbraco.Examine.Linq.Attributes
Imports Common

<NodeTypeAlias("AlphaFolder")>
Public Class AlphaFolder
    <Field("nodeName")>
    Public Property Name() As String
        Get
            Return _Name
        End Get
        Set
            _Name = Value
        End Set
    End Property
    Private _Name As String

    <Field("id")>
    Public Property Id() As Integer
        Get
            Return _Id
        End Get
        Set
            _Id = Value
        End Set
    End Property
    Private _Id As Integer



    Public Property Result() As Examine.SearchResult
        Get
            Return _Result
        End Get
        Set
            _Result = Value
        End Set
    End Property
    Private _Result As Examine.SearchResult
    'will automatically set the result from Examine
End Class