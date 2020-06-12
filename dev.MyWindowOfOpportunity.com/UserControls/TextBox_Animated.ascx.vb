Imports Common

<ValidationProperty("Text")>
Partial Class UserControls_TextBox_Animated
    Inherits System.Web.UI.UserControl


#Region "Properties"
    'Private _text As String = ""
    Public Property Text() As String
        Get
            Return txb.Text.Trim '.Replace("$", "")
        End Get
        Set(value As String)
            '_text = value
            If Not IsNothing(value) Then
                txb.Text = value.Trim
            End If
        End Set
    End Property

    Public WriteOnly Property ValidationGroup As String
        Set(value As String)
            txb.ValidationGroup = value
            rfv.ValidationGroup = value
        End Set
    End Property

    Public WriteOnly Property CausesValidation As Boolean
        Set(value As Boolean)
            txb.CausesValidation = value
            rfv.Enabled = value
        End Set
    End Property

    Public WriteOnly Property ErrorMessage As String
        Set(value As String)
            rfv.ErrorMessage = value
            rfv.Text = value
        End Set
    End Property

    Public WriteOnly Property CustomErrorMessage As String
        Set(value As String)
            lblCustomErrorMgs.Text += value
            lblCustomErrorMgs.Visible = True
        End Set
    End Property

    Public WriteOnly Property Title As String
        Set(value As String)
            ltrlTitle.Text = value
        End Set
    End Property

    Public WriteOnly Property isPassword As Boolean
        Set(value As Boolean)
            If value Then txb.TextMode = TextBoxMode.Password
        End Set
    End Property

    Public WriteOnly Property isEnabled As Boolean
        Set(value As Boolean)
            txb.Enabled = value
        End Set
    End Property

    Public WriteOnly Property isMultiLine As Boolean
        Set(value As Boolean)
            If value Then
                txb.TextMode = TextBoxMode.MultiLine
                txb.Rows = 9
                txb.Attributes.Add("multiple", "multiple")
                txb.CssClass += " multiline"
            End If
        End Set
    End Property

    Public Property isCurrency As Boolean = False
    Public Property isNumeric As Boolean = False
    Public Property additionalClass As String = String.Empty

    Public Enum _textMode
        _Color
        _Date
        _DateTime
        _DateTimeLocal
        _Email
        _Month
        _MultiLine
        _Number
        _Password
        _Phone
        _Range
        _Search
        _SingleLine
        _Time
        _Url
        _Week
    End Enum
    Public WriteOnly Property TextMode As _textMode
        Set(value As _textMode)
            'Set text mode if provided
            Select Case value
                Case _textMode._Color
                    txb.TextMode = TextBoxMode.Color
                Case _textMode._Date
                    txb.TextMode = TextBoxMode.Date
                Case _textMode._DateTime
                    txb.TextMode = TextBoxMode.DateTime
                Case _textMode._DateTimeLocal
                    txb.TextMode = TextBoxMode.DateTimeLocal
                Case _textMode._Email
                    txb.TextMode = TextBoxMode.Email
                Case _textMode._Month
                    txb.TextMode = TextBoxMode.Month
                Case _textMode._MultiLine
                    txb.TextMode = TextBoxMode.MultiLine
                Case _textMode._Number
                    txb.TextMode = TextBoxMode.Number
                Case _textMode._Password
                    txb.TextMode = TextBoxMode.Password
                Case _textMode._Phone
                    txb.TextMode = TextBoxMode.Phone
                Case _textMode._Range
                    txb.TextMode = TextBoxMode.Range
                Case _textMode._Search
                    txb.TextMode = TextBoxMode.Search
                Case _textMode._SingleLine
                    txb.TextMode = TextBoxMode.SingleLine
                Case _textMode._Time
                    txb.TextMode = TextBoxMode.Time
                Case _textMode._Url
                    txb.TextMode = TextBoxMode.Url
                Case _textMode._Week
                    txb.TextMode = TextBoxMode.Week
            End Select
        End Set
    End Property

    Public WriteOnly Property MaxLength As Integer
        Set(value As Integer)
            txb.MaxLength = value
        End Set
    End Property

    Public WriteOnly Property isRequired As Boolean
        Set(value As Boolean)
            If value = True Then txb.Attributes.Add("required", True)
        End Set
    End Property

    Public WriteOnly Property min As Integer
        Set(value As Integer)
            txb.Attributes.Add("min", value)
        End Set
    End Property

    Public WriteOnly Property max As Integer
        Set(value As Integer)
            txb.Attributes.Add("max", value)
        End Set
    End Property
#End Region

#Region "Handles"
    Private Sub UserControls_TextBox_Animated_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try  'If Not IsPostBack Then
            '
            If isCurrency Then
                If Not txb.CssClass.Contains(" isCurrency") Then txb.CssClass += " isCurrency"
                txb.Attributes.Add("data-a-sign", "$")
            ElseIf isNumeric Then
                If Not txb.CssClass.Contains(" isNumeric") Then txb.CssClass += " isNumeric"
                'txb.Attributes.Add("data-a-sign", "")
            Else
                'txb.Text = _text
            End If
            'End If

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\TextBox_Animated.ascx.vb : UserControls_TextBox_Animated_Load()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
    End Sub

    Private Sub UserControls_TextBox_Animated_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        '
        If Not String.IsNullOrWhiteSpace(additionalClass) Then
            inputOuterPnl.Attributes("class") += " " & additionalClass
        End If

    End Sub
#End Region

#Region "Methods"
#End Region
End Class

