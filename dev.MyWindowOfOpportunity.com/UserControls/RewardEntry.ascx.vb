Imports Common
Imports System.Xml.XPath
'Imports umbraco.cms.businesslogic.datatype
Imports umbraco.Core.Models
Imports umbraco.Web

Partial Class UserControls_RewardEntry
    Inherits System.Web.UI.UserControl


#Region "Properties"
    Public Property thisNodeId() As Int32
        Get
            'Return _thisNodeId
            Return hfldNodeId.Value
        End Get
        Set(value As Int32)
            '_thisNodeId = value
            hfldNodeId.Value = value.ToString
            'vsErrorMsgs.ValidationGroup = "val" & value.ToString
            txbContributionAmount.ValidationGroup = "val" & value.ToString
            txbQuantityAvailable.ValidationGroup = "val" & value.ToString
            txbNumberClaimed.ValidationGroup = "val" & value.ToString
        End Set
    End Property
    Public ReadOnly Property heading As String
        Get
            Return txbHeading.Text.Trim
        End Get
    End Property
    Public ReadOnly Property briefSummary As String
        Get
            Return txbBriefSummary.Text.Trim
        End Get
    End Property
    Public ReadOnly Property contributionAmount As String
        Get
            Return txbContributionAmount.Text.Trim
        End Get
    End Property
    Public ReadOnly Property quantityAvailable As String
        Get
            Return txbQuantityAvailable.Text.Trim
        End Get
    End Property
    Public ReadOnly Property numberClaimed As String
        Get
            Return txbNumberClaimed.Text.Trim
        End Get
    End Property
    Public ReadOnly Property month As String
        Get
            Return ddlMonth.SelectedValue
        End Get
    End Property
    Public ReadOnly Property year As String
        Get
            Return ddlYear.SelectedValue
        End Get
    End Property
    Public ReadOnly Property showOnTimeline As Boolean
        Get
            Return cbShowOnTimeline.Checked
        End Get
    End Property
    Public ReadOnly Property featuredImgId As Integer
        Get
            Return ucMinorImage.featuredImgId
        End Get
    End Property

    Private blRewards As blRewards
    Dim _uHelper As Uhelper = New Uhelper()
#End Region

#Region "Handles"
    Private Sub UserControls_RewardEntry_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        Try
            'Add IPublishedContent Id to panel as an attribute and to featured image.
            pnl.Attributes.Add("nodeId", thisNodeId)
            ucMinorImage.thisRewardId = thisNodeId
            'ddlMonth.Items.Clear()

            'Populate months dropdownlist with default data
            ddlMonth.DataSource = obtainDropdownData(prevalues.Dropdown_MonthsAbreviated, True)
            ddlMonth.DataTextField = "Text"
            ddlMonth.DataValueField = "Value"
            ddlMonth.DataBind()

            'Populate years dropdownlist with default data
            ddlYear.DataSource = obtainDropdownData(prevalues.Dropdown_Years, True)
            ddlYear.DataTextField = "Text"
            ddlYear.DataValueField = "Value"
            ddlYear.DataBind()

            If thisNodeId = -1 Then
                clearData()
            Else
                'Instantiate variables
                Dim thisNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(thisNodeId)

                'Obtain data and populate page.
                txbHeading.Text = thisNode.Name
                txbBriefSummary.Text = thisNode.GetPropertyValue(Of String)(nodeProperties.shortDescription)
                txbContributionAmount.Text = thisNode.GetPropertyValue(Of String)(nodeProperties.contributionAmount)
                txbQuantityAvailable.Text = thisNode.GetPropertyValue(Of String)(nodeProperties.available)
                txbNumberClaimed.Text = thisNode.GetPropertyValue(Of String)(nodeProperties.claimed)
                Try
                    ddlMonth.SelectedValue = thisNode.GetPropertyValue(Of String)(nodeProperties.estimatedShippingMonth)
                Catch ex As Exception
                    Dim sb As New StringBuilder()
                    sb.AppendLine("\UserControls\RewardEntry.ascx.vb : UserControls_RewardEntry_PreRender()")


                    saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
                    ddlMonth.SelectedIndex = 0
                End Try
                Try
                    ddlYear.SelectedValue = thisNode.GetPropertyValue(Of String)(nodeProperties.estimatedShippingYear)
                Catch ex1 As Exception
                    Dim sb As New StringBuilder()
                    sb.AppendLine("RewardEntry.ascx.vb : UserControls_RewardEntry_PreRender()")
                    saveErrorMessage(getLoggedInMember, ex1.ToString(), sb.ToString())
                    ddlYear.SelectedIndex = 0
                End Try
                cbShowOnTimeline.Checked = thisNode.GetPropertyValue(Of Boolean)(nodeProperties.showOnTimeline)
            End If

            'Assign relation between checkbox and label
            lbl.Attributes.Add("for", cbShowOnTimeline.ClientID)
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\RewardEntry.ascx.vb : UserControls_RewardEntry_PreRender()")

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Response.Write("<br />Reward Entry PreRender- Error: " & ex.ToString & " - nodeId: " & thisNodeId & "<br />")
        End Try
        'End If
    End Sub

#End Region

#Region "Methods"
    Private Function obtainDropdownData(ByVal datatypeId As Integer, Optional ByVal getPrevalues As Boolean = False) As ListItemCollection
        'Instantiate variables
        Dim preValueRootElementIterator As XPathNodeIterator
        Dim preValueIterator As XPathNodeIterator
        Dim listItems As New ListItemCollection()

        Try
            'Obtain datatype prevalue.  (Must move to first object entry or else iterator will return null.)
            preValueRootElementIterator = umbraco.library.GetPreValues(datatypeId)
            preValueRootElementIterator.MoveNext()

            'Obtain datatype's prevalues as list
            preValueIterator = preValueRootElementIterator.Current.SelectChildren("preValue", "")

            'Loop thru list and obtain a key/value set.
            While preValueIterator.MoveNext()
                If getPrevalues Then
                    listItems.Add(New ListItem(preValueIterator.Current.Value, preValueIterator.Current.GetAttribute("id", "").ToString))
                Else
                    listItems.Add(New ListItem(preValueIterator.Current.Value, preValueIterator.Current.Value.Replace(" ", "")))
                End If

            End While
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\RewardEntry.ascx.vb : obtainDropdownData()")

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
        '
        Return listItems
    End Function

    Private Sub clearData()
        'Obtain data and populate page.
        txbHeading.Text = ""
        txbBriefSummary.Text = ""
        txbContributionAmount.Text = ""
        txbQuantityAvailable.Text = ""
        txbNumberClaimed.Text = ""
        ddlMonth.SelectedIndex = 0
        ddlYear.SelectedIndex = 0
        cbShowOnTimeline.Checked = False
    End Sub
#End Region
End Class



