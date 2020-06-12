Imports Microsoft.VisualBasic


Public Class MediaJsonProperties
    Private m_src As String = String.Empty
    Private m_crops As List(Of Crop) = New List(Of Crop)
    Private m_focalPoint As FocalPoint = New FocalPoint

    Public Property src() As String
        Get
            Return m_src
        End Get
        Set
            m_src = Value
        End Set
    End Property
    Public Property crops() As List(Of Crop)
        Get
            Return m_crops
        End Get
        Set
            m_crops = Value
        End Set
    End Property
    Public Property focalPoint() As FocalPoint
        Get
            Return m_focalPoint
        End Get
        Set
            m_focalPoint = Value
        End Set
    End Property
    Public Sub New()
    End Sub
End Class

Public Class Crop
    Private m_width As Integer = 0
    Private m_height As Integer = 0
    Private m_alias As String = String.Empty
    Private m_coordinates As Coordinates = New Coordinates

    Public Property width() As Integer
        Get
            Return m_width
        End Get
        Set
            m_width = Value
        End Set
    End Property
    Public Property height() As Integer
        Get
            Return m_height
        End Get
        Set
            m_height = Value
        End Set
    End Property
    Public Property [alias]() As String
        Get
            Return m_alias
        End Get
        Set
            m_alias = Value
        End Set
    End Property
    Public Property coordinates() As Coordinates
        Get
            Return m_coordinates
        End Get
        Set
            m_coordinates = Value
        End Set
    End Property
    Public Sub New()
    End Sub
End Class
Public Class Coordinates
    Private m_x1 As Double = 0F
    Private m_x2 As Double = 0F
    Private m_y1 As Double = 0F
    Private m_y2 As Double = 0F

    Public Property x1() As Double
        Get
            Return m_x1
        End Get
        Set
            m_x1 = Value
        End Set
    End Property
    Public Property y1() As Double
        Get
            Return m_y1
        End Get
        Set
            m_y1 = Value
        End Set
    End Property
    Public Property x2() As Double
        Get
            Return m_x2
        End Get
        Set
            m_x2 = Value
        End Set
    End Property
    Public Property y2() As Double
        Get
            Return m_y2
        End Get
        Set
            m_y2 = Value
        End Set
    End Property
    Public Sub New()
    End Sub
End Class

Public Class FocalPoint
    Private m_left As Double = 0.5F
    Private m_top As Double = 0.5F

    Public Property left() As Double
        Get
            Return m_left
        End Get
        Set
            m_left = Value
        End Set
    End Property
    Public Property top() As Double
        Get
            Return m_top
        End Get
        Set
            m_top = Value
        End Set
    End Property
    Public Sub New()
    End Sub
End Class
