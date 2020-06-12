Imports System.Collections.Generic

Public Class BusinessReturn

    Dim _Valid As Boolean = True
    Dim _ExceptionMessage As String
    Dim _ValidationMessages As New List(Of ValidationContainer)
    Dim _DataContainer As New List(Of Object)
    Dim _ErrorField As String
    Dim _ReturnMessage As String

    ''' <summary>
    ''' Use this property for setting single code exception messages.
    ''' Setting this property will also set isValid to false.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ExceptionMessage() As String
        Get
            Return _ExceptionMessage
        End Get
        Set(ByVal value As String)
            _ExceptionMessage = value
        End Set
    End Property

    ''' <summary>
    ''' Use this property for setting a message (such as simple warnings) to be returned to the UI.
    ''' Setting this property will NOT cause isValid to return false.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ReturnMessage() As String
        Get
            Return _ReturnMessage
        End Get
        Set(ByVal value As String)
            _ReturnMessage = value
        End Set
    End Property

    ''' <summary>
    ''' Indicates if the method suceeded or not
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property isValid() As Boolean
        Get
            If (_ValidationMessages.Count > 0 Or _ExceptionMessage <> String.Empty) Then
                Return False
            Else
                Return True
            End If
        End Get
        Set(ByVal value As Boolean)
            _Valid = value
        End Set
    End Property

    ''' <summary>
    ''' List of validation errors message. Use this over Exception Message if there are multiple exceptions.
    ''' Setting this property causes isValid to return False.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property ValidationMessages() As List(Of ValidationContainer)
        Get
            Return _ValidationMessages
        End Get
    End Property

    ''' <summary>
    ''' Name of the validation control to handle the error
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ErrorField() As String
        Get
            Return _ErrorField
        End Get
        Set(ByVal value As String)
            _ErrorField = value
        End Set
    End Property

    ''' <summary>
    ''' Generic container to hold datasets
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property DataContainer() As List(Of Object)
        Get
            Return _DataContainer
        End Get
        Set(ByVal value As List(Of Object))
            _DataContainer = value
        End Set
    End Property

End Class
