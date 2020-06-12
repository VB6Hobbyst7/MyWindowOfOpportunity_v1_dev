Imports Common


Partial Class UserControls_progressBar
    Inherits System.Web.UI.UserControl


#Region "Properties"
    Private _percentage As Int16 = 0
    Public Property percentage As Int16
        Get
            Return _percentage
        End Get
        Set(value As Int16)
            _percentage = value
        End Set
    End Property

    Private _fullSize As Boolean = False
    Public Property fullSize As Boolean
        Get
            Return _fullSize
        End Get
        Set(value As Boolean)
            _fullSize = value
        End Set
    End Property

    Public Property showMarker As Boolean = True
    Public Property colorOverride As colorSchemePercentages = colorSchemePercentages.na
#End Region

#Region "Handles"
    Private Sub UserControls_progressBar_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        Try
            If fullSize Then
                '======== FULL PANEL =========
                outerPanel.CssClass += " fullSize"
                'pnlMarker.Visible = True
                pnlMarker.Visible = showMarker
                hfldPercentage.Value = percentage.ToString
                innerPanel.Style.Add("width", "0%")

                'Add class properties to inner panel
                If colorOverride <> colorSchemePercentages.na Then
                    Select Case colorOverride
                        Case colorSchemePercentages.bgcolor_10percent
                            innerPanel.CssClass += " " & colorSchemePercentage.bgcolor_10percent
                        Case colorSchemePercentages.bgcolor_20percent
                            innerPanel.CssClass += " " & colorSchemePercentage.bgcolor_20percent
                        Case colorSchemePercentages.bgcolor_30percent
                            innerPanel.CssClass += " " & colorSchemePercentage.bgcolor_30percent
                        Case colorSchemePercentages.bgcolor_40percent
                            innerPanel.CssClass += " " & colorSchemePercentage.bgcolor_40percent
                        Case colorSchemePercentages.bgcolor_50percent
                            innerPanel.CssClass += " " & colorSchemePercentage.bgcolor_50percent
                        Case colorSchemePercentages.bgcolor_60percent
                            innerPanel.CssClass += " " & colorSchemePercentage.bgcolor_60percent
                        Case colorSchemePercentages.bgcolor_70percent
                            innerPanel.CssClass += " " & colorSchemePercentage.bgcolor_70percent
                        Case colorSchemePercentages.bgcolor_80percent
                            innerPanel.CssClass += " " & colorSchemePercentage.bgcolor_80percent
                        Case colorSchemePercentages.bgcolor_90percent
                            innerPanel.CssClass += " " & colorSchemePercentage.bgcolor_90percent
                        Case Else
                            innerPanel.CssClass += " " & colorSchemePercentage.bgcolor_100percent
                    End Select
                ElseIf percentage >= 100 Then
                    innerPanel.CssClass += " bgcolor_100percent"
                Else
                    'Build name for background color depending on percentage to next 10th place.
                    innerPanel.CssClass += " bgcolor_" & (percentage + ((10 - (percentage Mod 10)) Mod 10)).ToString & "percent"
                End If

                'Adjust styling for 0%
                If percentage = 0 Then
                    innerPanel.Style.Add("box-shadow", "none")
                    innerPanel.Style.Add("border", "none")
                End If

            Else
                '======== SMALLER CAMPAIGN PANEL =========
                'Add style properties to inner panel
                If percentage = 0 Then
                    innerPanel.Style.Add("box-shadow", "none")
                    innerPanel.Style.Add("border", "none")
                ElseIf percentage >= 100 Then
                    innerPanel.Style.Add("width", "100%")

                    HiddenField2.Value = percentage.ToString
                    innerPanel.CssClass += " bgcolor_100percent"
                Else


                    innerPanel.Style.Add("width", percentage.ToString & "%")
                    HiddenField2.Value = percentage.ToString
                    'Build name for background color depending on percentage to next 10th place.
                    innerPanel.CssClass += " bgcolor_" & (percentage + ((10 - (percentage Mod 10)) Mod 10)).ToString & "percent"
                End If
            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\UserControls_progressBar.ascx.vb : UserControls_progressBar_PreRender()")

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
    End Sub
#End Region

#Region "Methods"
#End Region
End Class
