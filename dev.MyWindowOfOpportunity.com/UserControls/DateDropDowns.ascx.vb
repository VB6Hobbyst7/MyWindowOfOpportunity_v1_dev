Imports Common
Partial Class UserControls_DateDropDowns
    Inherits System.Web.UI.UserControl


#Region "Properties"
    Public Property mustBe18 As Boolean = False
    Private Enum daysInMonth
        Jan = 31
        Feb = 28
        Mar = 31
        Apr = 30
        May = 31
        Jun = 30
        Jul = 31
        Aug = 31
        Sep = 30
        Oct = 31
        Nov = 30
        Dec = 31
    End Enum
    Private dayOfMonth As List(Of String)

    Public Property Day As String
        Get
            Return ddlDay.SelectedValue
        End Get
        Set(value As String)
            ddlDay.SelectedValue = value.Trim
        End Set
    End Property
    Public Property Month As String
        Get
            Return ddlMonth.SelectedValue
        End Get
        Set(value As String)
            ddlMonth.SelectedValue = value.Trim
        End Set
    End Property
    Public Property Year As String
        Get
            Return ddlYear.SelectedValue
        End Get
        Set(value As String)
            ddlYear.SelectedValue = value.Trim
        End Set
    End Property

    Public WriteOnly Property ValidationGroup As String
        Set(value As String)
            rfvDay.ValidationGroup = value
            rfvMonth.ValidationGroup = value
            rfvYear.ValidationGroup = value
        End Set
    End Property
    Public WriteOnly Property CausesValidation As Boolean
        Set(value As Boolean)
            rfvDay.Enabled = value
            rfvMonth.Enabled = value
            rfvYear.Enabled = value
            ddlDay.CausesValidation = value
            ddlMonth.CausesValidation = value
            ddlYear.CausesValidation = value
        End Set
    End Property
#End Region

#Region "Handles"
    Private Sub UserControls_DateDropDowns_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            'Populate months and years ddl
            populateMonths()
            populateYears()
        End If
    End Sub
    Private Sub ddlMonth_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlMonth.SelectedIndexChanged
        If ddlMonth.SelectedIndex = 0 Then
            'Reset dropdown and deactivate
            ddlDay.SelectedIndex = 0
            ddlYear.SelectedIndex = 0
            ddlDay.Enabled = False
            ddlYear.Enabled = False
        Else
            'Enable dropdown
            ddlDay.Enabled = True
            'Populate list of days according to month
            populateDays()
        End If
    End Sub
    Private Sub ddlDay_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlDay.SelectedIndexChanged
        If ddlDay.SelectedIndex = 0 Then
            'Reset dropdown and deactivate
            ddlYear.SelectedIndex = 0
            ddlYear.Enabled = False
        Else
            'Enable dropdown
            ddlYear.Enabled = True
        End If
    End Sub
    Private Sub ddlYear_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlYear.SelectedIndexChanged
        'Determine if selected year is a leapyear
        If ddlMonth.SelectedValue = "2" Then
            populateDays()
        End If
    End Sub

#End Region

#Region "Methods"
    Private Sub populateDayOfMonthList()
        '
        dayOfMonth = New List(Of String)
        'Create day of month
        dayOfMonth.Add("1st")
        dayOfMonth.Add("2nd")
        dayOfMonth.Add("3rd")
        dayOfMonth.Add("4th")
        dayOfMonth.Add("5th")
        dayOfMonth.Add("6th")
        dayOfMonth.Add("7th")
        dayOfMonth.Add("8th")
        dayOfMonth.Add("9th")
        dayOfMonth.Add("10th")
        dayOfMonth.Add("11th")
        dayOfMonth.Add("12th")
        dayOfMonth.Add("13th")
        dayOfMonth.Add("14th")
        dayOfMonth.Add("15th")
        dayOfMonth.Add("16th")
        dayOfMonth.Add("17th")
        dayOfMonth.Add("18th")
        dayOfMonth.Add("19th")
        dayOfMonth.Add("20th")
        dayOfMonth.Add("21st")
        dayOfMonth.Add("22nd")
        dayOfMonth.Add("23rd")
        dayOfMonth.Add("24th")
        dayOfMonth.Add("25th")
        dayOfMonth.Add("26th")
        dayOfMonth.Add("27th")
        dayOfMonth.Add("28th")
        dayOfMonth.Add("29th")
        dayOfMonth.Add("30th")
        dayOfMonth.Add("31st")
    End Sub
    Private Sub populateMonths()
        ddlMonth.Items.Insert(1, New ListItem("January", "1"))
        ddlMonth.Items.Insert(2, New ListItem("February", "2"))
        ddlMonth.Items.Insert(3, New ListItem("March", "3"))
        ddlMonth.Items.Insert(4, New ListItem("April", "4"))
        ddlMonth.Items.Insert(5, New ListItem("May", "5"))
        ddlMonth.Items.Insert(6, New ListItem("June", "6"))
        ddlMonth.Items.Insert(7, New ListItem("July", "7"))
        ddlMonth.Items.Insert(8, New ListItem("August", "8"))
        ddlMonth.Items.Insert(9, New ListItem("September", "9"))
        ddlMonth.Items.Insert(10, New ListItem("October", "10"))
        ddlMonth.Items.Insert(11, New ListItem("November ", "11"))
        ddlMonth.Items.Insert(12, New ListItem("December", "12"))
    End Sub
    Private Sub populateYears()
        'Instantiate variables
        Dim yearCounter As Int16 = Today.Year
        Dim index As UInt16 = 1

        'Adjust year counter if user must be 18
        If mustBe18 Then yearCounter -= 18

        'Build list of years
        While yearCounter > (Today.Year - 100)
            '
            ddlYear.Items.Insert(index, New ListItem(yearCounter, yearCounter))

            'Adjust counters
            yearCounter -= 1
            index += 1
        End While
    End Sub
    Private Sub populateDays()
        'Get currently selected item
        Dim currentItem As String = ddlDay.SelectedValue
        Dim totalDays As UInt16 = 0
        Dim index As UInt16 = 0

        'Create list of days within a month
        populateDayOfMonthList()

        'Determine the number of days in month
        Select Case ddlMonth.SelectedValue
            Case "1"
                totalDays = daysInMonth.Jan
            Case "2"
                totalDays = daysInMonth.Feb
                'Determine if selected year is a leapyear
                If ddlYear.SelectedIndex > 0 Then
                    If DateTime.IsLeapYear(ddlYear.SelectedValue) Then totalDays += 1
                End If
            Case "3"
                totalDays = daysInMonth.Mar
            Case "4"
                totalDays = daysInMonth.Apr
            Case "5"
                totalDays = daysInMonth.May
            Case "6"
                totalDays = daysInMonth.Jun
            Case "7"
                totalDays = daysInMonth.Jul
            Case "8"
                totalDays = daysInMonth.Aug
            Case "9"
                totalDays = daysInMonth.Sep
            Case "10"
                totalDays = daysInMonth.Oct
            Case "11"
                totalDays = daysInMonth.Nov
            Case "12"
                totalDays = daysInMonth.Dec
        End Select

        '
        ddlDay.Items.Clear()
        ddlDay.Items.Add(New ListItem("Day", ""))

        'Build list of years
        While index < totalDays
            'Add days to ddl
            ddlDay.Items.Add(New ListItem(dayOfMonth(index), index + 1))
            'ddlDay.Items.Insert(index + 1, New ListItem(dayOfMonth(index), index + 1))

            'Adjust counter
            index += 1
        End While

        'Reselect current item if applicable.
        Try
            ddlDay.SelectedValue = currentItem
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\DateDropDowns.ascx.vb : populateDays()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            ddlDay.SelectedIndex = 0
        End Try
    End Sub
#End Region
End Class
