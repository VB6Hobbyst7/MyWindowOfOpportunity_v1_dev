Imports umbraco
Imports Common
Imports System.Xml.XPath
Imports System.Data
Imports umbraco.Core.Models
Imports umbraco.Web

Partial Class UserControls_editTab_Categories
    Inherits System.Web.UI.UserControl


#Region "Properties"
    'Private _thisNode As IPublishedContent
    'Public Property thisNode() As IPublishedContent
    '    Get
    '        Return _thisNode
    '    End Get
    '    Set(value As IPublishedContent)
    '        _thisNode = value
    '    End Set
    'End Property

    Public Property thisNodeId() As String
        Get
            Return hfldThisNodeId.Value
        End Get
        Set(value As String)
            hfldThisNodeId.Value = value.ToString
        End Set
    End Property

    Private blockgridClass As String = "small-block-grid-1 medium-block-grid-2 large-block-grid-3"
    Private hide As String = " hide"

    Private blCampaigns As New blCampaigns
    Private dtCurrentCategories As DataTable
    Private Structure dtColumn
        Const category As String = "category"
        Const subcategories As String = "subcategories"
    End Structure
    Dim _uHelper As Uhelper = New Uhelper()
    Public Property campaignComplete As Boolean = False
#End Region

#Region "Handles"
    Private Sub UserControls_editTab_Categories_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not String.IsNullOrEmpty(thisNodeId) Then
            Try  'Instantiate variables
                Dim thisNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(CInt(thisNodeId))
                Dim categoryListItems As ListItemCollection
                Dim firstSubList As Boolean = True

                'Set the contact us link.
                hlnkContactUs.NavigateUrl = _uHelper.Get_IPublishedContentByID(siteNodes.ContactUs).Url

                'Set hidden field with tab name
                hfldTabName.Value = tabNames.categories

                'Obtain all categories
                categoryListItems = ObtainCategories(prevalues.CategoryList, False)

                'Determine if campaign is unpublished 
                If blCampaigns.obtainStatusType_byId(CInt(thisNodeId)) = statusType.Unpublished Then
                    'Bind categories to list
                    rblCategories.DataSource = categoryListItems
                    rblCategories.DataTextField = "Text"
                    rblCategories.DataValueField = "Value"
                    rblCategories.DataBind()
                    rblCategories.SelectedIndex = 0

                    'Loop thru each category to obtain subcategories
                    For Each category As ListItem In categoryListItems
                        Dim categoryEntry As ASP.usercontrols_categoryentry_ascx = New ASP.usercontrols_categoryentry_ascx With {
                            .category = category,
                            .thisNode = thisNode,
                            .firstSubList = firstSubList
                        }
                        firstSubList = False

                        'Add checkboxlist to placeholder
                        phSubcategories.Controls.Add(categoryEntry)
                    Next

                    'disable if campaign is complete
                    If campaignComplete Then
                        lbtnSave.Enabled = False
                        lbtnSave.CssClass += " disabled"
                    End If
                Else
                    phSelectCategories.Visible = False
                End If


            Catch ex As Exception
                Dim sb As New StringBuilder()
                sb.AppendLine("editTab_Categories.ascx.vb : UserControls_editTab_Categories_Load()")
                saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            End Try

        End If
    End Sub
    Private Sub UserControls_editTab_Categories_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        Try
            If Not String.IsNullOrEmpty(thisNodeId) Then
                'Instantiate variables
                dtCurrentCategories = New DataTable
                Dim thisNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(CInt(thisNodeId))

                'Add columns to datatable
                dtCurrentCategories.Columns.Add(dtColumn.category)
                dtCurrentCategories.Columns.Add(dtColumn.subcategories)
                dtCurrentCategories.AcceptChanges()

                '
                If thisNode.HasValue(SubcategoryConverter.Artistic) Then
                    AddCurrentCategoriesToDt(Categories.Arts, thisNode.GetPropertyValue(Of String)(SubcategoryConverter.Artistic))
                End If
                If thisNode.HasValue(SubcategoryConverter.Business) Then
                    AddCurrentCategoriesToDt(Categories.Business, thisNode.GetPropertyValue(Of String)(SubcategoryConverter.Business))
                End If
                If thisNode.HasValue(SubcategoryConverter.Charity) Then
                    AddCurrentCategoriesToDt(Categories.Charity, thisNode.GetPropertyValue(Of String)(SubcategoryConverter.Charity))
                End If
                If thisNode.HasValue(SubcategoryConverter.Community) Then
                    AddCurrentCategoriesToDt(Categories.Community, thisNode.GetPropertyValue(Of String)(SubcategoryConverter.Community))
                End If
                If thisNode.HasValue(SubcategoryConverter.Science) Then
                    AddCurrentCategoriesToDt(Categories.Science, thisNode.GetPropertyValue(Of String)(SubcategoryConverter.Science))
                End If
                If thisNode.HasValue(SubcategoryConverter.SelfHelp) Then
                    AddCurrentCategoriesToDt(Categories.SelfHelp, thisNode.GetPropertyValue(Of String)(SubcategoryConverter.SelfHelp))
                End If
                If thisNode.HasValue(SubcategoryConverter.Software) Then
                    AddCurrentCategoriesToDt(Categories.Software, thisNode.GetPropertyValue(Of String)(SubcategoryConverter.Software))
                End If
                If thisNode.HasValue(SubcategoryConverter.Technology) Then
                    AddCurrentCategoriesToDt(Categories.Technology, thisNode.GetPropertyValue(Of String)(SubcategoryConverter.Technology))
                End If

                '
                lstviewSelectedCategories.DataSource = dtCurrentCategories
                lstviewSelectedCategories.DataBind()

            End If

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("editTab_Categories.ascx.vb : PreRender()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
    End Sub

    Private Sub lbtnSave_Click(sender As Object, e As EventArgs) Handles lbtnSave.Click
        Try
            'Instantiate variables
            Dim successfulSubmission As Boolean = True
            Dim alert As ASP.usercontrols_alertmsg_ascx

            For Each item As Control In phSubcategories.Controls
                'Find usercontrol within listitem
                Dim uc As ASP.usercontrols_categoryentry_ascx = DirectCast(item, ASP.usercontrols_categoryentry_ascx)

                '
                If Not uc.submitData Then successfulSubmission = False
            Next

            'Check if submission was valid
            If (successfulSubmission) Then
                'Show success msg
                alert = New ASP.usercontrols_alertmsg_ascx With {
                    .MessageType = UserControls_AlertMsg.msgType.Success,
                    .AlertMsg = "Saved Successfully"
                }
                phAlertMsg.Controls.Add(alert)
            Else
                'Show alert msg
                alert = New ASP.usercontrols_alertmsg_ascx With {
                    .MessageType = UserControls_AlertMsg.msgType.Alert,
                    .AlertMsg = "Error. Unable to save data."
                }
                phAlertMsg.Controls.Add(alert)
            End If


        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\editTab_Categories.ascx.vb : lbtnSave_Click()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Response.Write("<br />Error: " & ex.ToString)
        End Try
    End Sub
#End Region

#Region "Methods"
    Private Function ObtainCategories(ByVal datatypeId As Integer, Optional ByVal getPrevalues As Boolean = False) As ListItemCollection
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
                    listItems.Add(New ListItem(preValueIterator.Current.GetAttribute("id", "").ToString, preValueIterator.Current.Value))
                Else
                    listItems.Add(New ListItem(preValueIterator.Current.Value, preValueIterator.Current.Value.Replace(" ", "")))
                End If

            End While
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("editTab_Categories.ascx.vb : ObtainCategories()")
            sb.AppendLine("datatypeId" & datatypeId)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
        '
        Return listItems
    End Function
    Private Sub SetSelectedSubcategories(ByRef cbl As CheckBoxList, ByVal subCategoryProperty As String)
        Try
            'Instantiate variables
            Dim thisNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(CInt(thisNodeId))

            'Obtain all subcategories selected in umbraco
            If thisNode.HasProperty(subCategoryProperty) Then
                Dim subcategories As String() = thisNode.GetPropertyValue(Of String)(subCategoryProperty).Split(", ")

                'Loop through each item in the checkbox list
                For Each item As ListItem In cbl.Items
                    'if item matches list of selected items, select checkbox.
                    If subcategories.Contains(item.Text) Then
                        item.Selected = True
                    End If
                Next
            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\editTab_Categories.ascx.vb : setSelectedSubcategories()")
            sb.AppendLine("cbl:" & cbl.ToString())
            sb.AppendLine("subCategoryProperty:" & subCategoryProperty)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Response.Write("error in:  " & subCategoryProperty)
        End Try
    End Sub
    Private Sub AddCurrentCategoriesToDt(ByVal category As String, ByVal subcategories As String)
        Try
            'Instantiate variables
            Dim dr As DataRow = dtCurrentCategories.NewRow
            Dim sb As New StringBuilder

            'Split subcategories
            If Not String.IsNullOrWhiteSpace(subcategories) Then
                Dim subcategory As String() = subcategories.Split(",")
                For Each item As String In subcategory
                    sb.Append(item & "<br />")
                Next
            End If

            'Add data to row
            dr(dtColumn.category) = category
            dr(dtColumn.subcategories) = sb.ToString

            'add row to datatable
            dtCurrentCategories.Rows.Add(dr)

        Catch ex As Exception
            Dim sbEx As New StringBuilder()
            sbEx.AppendLine("editTab_Categories.ascx.vb : AddCurrentCategoriesToDt()")
            sbEx.AppendLine("category:" & category)
            sbEx.AppendLine("subcategories:" & subcategories)
            saveErrorMessage(getLoggedInMember, ex.ToString, sbEx.ToString())
        End Try
    End Sub
#End Region
End Class