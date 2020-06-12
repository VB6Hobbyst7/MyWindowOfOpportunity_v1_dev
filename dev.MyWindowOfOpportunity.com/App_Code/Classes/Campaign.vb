Imports Microsoft.VisualBasic
Imports umbraco.Examine.Linq.Attributes
Imports Common

<NodeTypeAlias("Campaign")>
Public Class Campaign
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

    Public Property Id() As Integer
        Get
            Return _Id
        End Get
        Set
            _Id = Value
        End Set
    End Property
    Private _Id As Integer

    <Field(nodeProperties.briefSummary)>
    Public Property briefSummary() As String
        Get
            Return _briefSummary
        End Get
        Set
            _briefSummary = Value
        End Set
    End Property
    Private _briefSummary As String





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