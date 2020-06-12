
Partial Class UserControls_DaysRemaining
    Inherits System.Web.UI.UserControl


#Region "Properties"
    Public Property daysRemaining As Int16
    Public Property daysRemaining_percentage As Int16


    '<input type = "hidden" runat="server" id="hfldDaysRemainingPercent" Class="hfldDaysRemainingPercent" value="75" />
    '<input type = "hidden" runat="server" id="hfldDaysRemaining" Class="hfldDaysRemaining" value="5" />
#End Region

#Region "Handles"
    Private Sub UserControls_DaysRemaining_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            hfldDaysRemaining.Value = daysRemaining
            hfldDaysRemainingPercent.Value = daysRemaining_percentage
        End If
    End Sub
#End Region

#Region "Methods"
#End Region
End Class
