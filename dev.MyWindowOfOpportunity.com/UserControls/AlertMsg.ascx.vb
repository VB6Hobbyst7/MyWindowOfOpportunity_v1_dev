Imports Common
Partial Class UserControls_AlertMsg
    Inherits System.Web.UI.UserControl


#Region "Properties"
    Public Enum msgType
        DontShow
        Alert
        Warning
        Success
        Info
    End Enum
    Public Property MessageType() As msgType
        Get
            Return _MessageType
        End Get
        Set(value As msgType)
            _MessageType = value
        End Set
    End Property
    Public Property AlertMsg As String = String.Empty
    Public Property AdditionalText As String = String.Empty
    Public Property isHidden As Boolean = False
    Public Property additionalClass As String = String.Empty
    Public Property showIcon() As Boolean = True

    Private _MessageType As msgType
#End Region

#Region "Handles"
    Private Sub UserControls_AlertMsg_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        Try 'Determine which message type to display
            Select Case MessageType()
                Case msgType.Alert 'RED ALERT BOX
                    pnlAlertBox.CssClass += " alert"
                    pnlOuterRow.CssClass += " alert"
                    iFi.Attributes.Add("class", "fi-x")

                Case msgType.Info 'BLUE ALERT BOX
                    pnlAlertBox.CssClass += " info"
                    pnlOuterRow.CssClass += " info"
                    iFi.Attributes.Add("class", "fi-eye")

                Case msgType.Success 'GREEN ALERT BOX
                    pnlAlertBox.CssClass += " success"
                    pnlOuterRow.CssClass += " success"
                    iFi.Attributes.Add("class", "fi-check")

                Case msgType.Warning 'ORANGE ALERT BOX
                    pnlAlertBox.CssClass += " warning"
                    pnlOuterRow.CssClass += " warning"
                    iFi.Attributes.Add("class", "fi-alert")
                Case Else
                    pnlAlertBox.Visible = False
            End Select

            'Display text
            If Not String.IsNullOrWhiteSpace(AlertMsg) Then ltrlMsg.Text = AlertMsg.Trim
            If Not String.IsNullOrWhiteSpace(AdditionalText) Then ltrlAdditionalText.Text = AdditionalText.Trim

            If isHidden Then
                'Set panel to hidden.
                pnlOuterRow.CssClass += " hide"
            Else
                'Remove hidden attribute
                pnlOuterRow.CssClass.Replace(" hide", "")
            End If

            'show/hide icon panel
            pnlIcon.Visible = showIcon()

            If Not String.IsNullOrWhiteSpace(additionalClass) Then
                pnlAlertBox.CssClass += " " & additionalClass
                pnlOuterRow.CssClass += " " & additionalClass
            End If

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\UserControls_AlertMsg.ascx.vb : UserControls_AlertMsg_PreRender()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
    End Sub

#End Region

#Region "Methods"
#End Region
End Class
