Imports umbraco
Imports Common
Imports System.Xml.XPath
Imports umbraco.Core
Imports umbraco.Core.Publishing
Imports umbraco.Core.Models
Imports umbraco.Web

Partial Class UserControls_CategoryEntry
    Inherits System.Web.UI.UserControl


#Region "Properties"
    Private _category As ListItem
    Public Property category() As ListItem
        Get
            Return _category
        End Get
        Set(value As ListItem)
            _category = value
        End Set
    End Property

    Private _thisNode As IPublishedContent
    Public Property thisNode() As IPublishedContent
        Get
            Return _thisNode
        End Get
        Set(value As IPublishedContent)
            _thisNode = value
        End Set
    End Property

    Private _firstSubList As Boolean = False
    Public Property firstSubList() As Boolean
        Get
            Return _firstSubList
        End Get
        Set(value As Boolean)
            _firstSubList = value
        End Set
    End Property

    Public ReadOnly Property submitData() As Boolean
        Get
            Return saveData()
        End Get
    End Property

    Private blockgridClass As String = "small-block-grid-1 medium-block-grid-2 large-block-grid-3"
    Private hide As String = " hide"
    Private blCampaigns As blCampaigns
    Private _uHelper As Uhelper = New Uhelper()
#End Region

#Region "Handles"
    Private Sub UserControls_CategoryEntry_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            'Instantiate variables
            Dim subcategoryListItems As ListItemCollection = New ListItemCollection
            Dim subCategoryProperty As String = String.Empty

            'Clear previous data from list
            subcategoryListItems.Clear()

            'Obtain list of subcategories
            Select Case category.Value
                Case Categories.Arts
                    subcategoryListItems = obtainCategories(prevalues.Subcategories_Artistic, True)
                    subCategoryProperty = SubcategoryConverter.Artistic
                Case Categories.Business
                    subcategoryListItems = obtainCategories(prevalues.Subcategories_Business, True)
                    subCategoryProperty = SubcategoryConverter.Business
                Case Categories.Charity
                    subcategoryListItems = obtainCategories(prevalues.Subcategories_Charity, True)
                    subCategoryProperty = SubcategoryConverter.Charity
                    phCharityInstructions.Visible = True
                Case Categories.Community
                    subcategoryListItems = obtainCategories(prevalues.Subcategories_Community, True)
                    subCategoryProperty = SubcategoryConverter.Community
                Case Categories.Science
                    subcategoryListItems = obtainCategories(prevalues.Subcategories_Science, True)
                    subCategoryProperty = SubcategoryConverter.Science
                Case Categories.SelfHelp
                    subcategoryListItems = obtainCategories(prevalues.Subcategories_SelfHelp, True)
                    subCategoryProperty = SubcategoryConverter.SelfHelp
                Case Categories.Software
                    subcategoryListItems = obtainCategories(prevalues.Subcategories_Software, True)
                    subCategoryProperty = SubcategoryConverter.Software
                Case Categories.Technology
                    subcategoryListItems = obtainCategories(prevalues.Subcategories_Technology, True)
                    subCategoryProperty = SubcategoryConverter.Technology
                Case Else
            End Select

            'Add properties to radiobutton list
            cbl.ID = "cbl" & category.Value
            cbl.RepeatLayout = RepeatLayout.UnorderedList
            cbl.CssClass = blockgridClass
            If firstSubList Then
                firstSubList = False
            Else
                '
                pnl.CssClass += hide
            End If
            '
            pnl.Attributes.Add("category", category.Value)
            pnl.Attributes.Add("type", Miscellaneous.Subcategory)

            'Bind categories to list
            cbl.DataSource = subcategoryListItems
            cbl.DataTextField = "Value"
            cbl.DataValueField = "Text"
            cbl.DataBind()

            'Set selected subcategories from umbraco
            setSelectedSubcategories(cbl, subCategoryProperty)
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\UserControls_CategoryEntry.ascx.vb : UserControls_CategoryEntry_Load()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
    End Sub
#End Region

#Region "Methods"
    Private Function obtainCategories(ByVal datatypeId As Integer, Optional ByVal getPrevalues As Boolean = False) As ListItemCollection
        'Instantiate variables
        Dim listItems As New ListItemCollection()

        Try
            Dim preValueRootElementIterator As XPathNodeIterator
            Dim preValueIterator As XPathNodeIterator

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
            sb.AppendLine("\UserControls\CategoryEntry.ascx.vb : obtainCategories()")
            sb.AppendLine("datatypeId" & datatypeId)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
        '
        Return listItems
    End Function
    Private Sub setSelectedSubcategories(ByRef cbl As CheckBoxList, ByVal subCategoryProperty As String)
        Try
            'Obtain all subcategories selected in umbraco
            If thisNode.HasProperty(subCategoryProperty) Then
                Dim subCatProp = thisNode.GetPropertyValue(Of String)(subCategoryProperty)

                If Not String.IsNullOrWhiteSpace(subCatProp) Then
                    Dim subcategories As String() = subCatProp.Split(", ")
                    'Loop through each item in the checkbox list
                    For Each item As ListItem In cbl.Items
                        'if item matches list of selected items, select checkbox.
                        If subcategories.Contains(item.Text) Then
                            item.Selected = True
                        End If
                    Next
                End If
            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("CategoryEntry.ascx.vb : setSelectedSubcategories()")
            sb.AppendLine("cbl:" & cbl.ToString())
            sb.AppendLine("subCategoryProperty:" & subCategoryProperty)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Response.Write("error in:  " & subCategoryProperty)
        End Try
    End Sub
    Private Function saveData() As Boolean
        Try
            'Instantiate variables
            Dim updateResponse As Attempt(Of PublishStatus) '= New Attempt(Of PublishStatus)
            blCampaigns = New blCampaigns

            'For Each cbl As CheckBoxList In phSubcategories.Controls
            'Instantiate variables
            Dim cblList As List(Of String) = New List(Of String)
            'Dim cblCsv As String

            'Add each checked item into the list
            For Each item As ListItem In cbl.Items
                If item.Selected Then
                    cblList.Add(item.Value)
                End If
            Next

            ''Join each item into a csv
            'cblCsv = String.Join(",", cblList.ToArray())

            ''Obtain category from checkboxlist attribute
            ''Dim subCategory As String = cbl.Attributes("category")
            'Dim subCategory As String = pnl.Attributes("category")

            ''Adjust subcategory value for saving to umbraco
            'Select Case subCategory
            '    Case Categories.Arts
            '        subCategory = SubcategoryConverter.Artistic
            '    Case Categories.Business
            '        subCategory = SubcategoryConverter.Business
            '    Case Categories.Charity
            '        subCategory = SubcategoryConverter.Charity
            '    Case Categories.Community
            '        subCategory = SubcategoryConverter.Community
            '    Case Categories.Science
            '        subCategory = SubcategoryConverter.Science
            '    Case Categories.SelfHelp
            '        subCategory = SubcategoryConverter.SelfHelp
            '    Case Categories.Software
            '        subCategory = SubcategoryConverter.Software
            '    Case Categories.Technology
            '        subCategory = SubcategoryConverter.Technology
            '    Case Else
            'End Select

            'Submit data for updating
            'updateResponse = blCampaigns.UpdateCategories(thisNode.Id, subCategory, cblCsv)

            updateResponse = blCampaigns.submitCategoryData(pnl.Attributes("category"), cblList, thisNode.Id)

            'Public Function submitCategoryData(ByVal subCategory As String, ByVal cblList As List(Of String), ByVal nodeId As Integer) As Attempt(Of PublishStatus)



            'If not valid, exit
            'If Not (updateResponse.Success) Then
            '    Exit For
            'End If
            'Next


            Return updateResponse.Success

            ''If valid, 
            'If (updateResponse.Success) Then
            '    'Show success msg
            '    alert = New ASP.usercontrols_alertmsg_ascx
            '    alert.MessageType = UserControls_AlertMsg.msgType.Success
            '    alert.AlertMsg = "Saved Successfully"
            '    phAlertMsg.Controls.Add(alert)
            'Else
            '    'Show alert msg
            '    alert = New ASP.usercontrols_alertmsg_ascx
            '    alert.MessageType = UserControls_AlertMsg.msgType.Alert
            '    alert.AlertMsg = "Error. Unable to save data."
            '    phAlertMsg.Controls.Add(alert)
            'End If

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\CategoryEntry.ascx.vb : saveData()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Response.Write(ex.ToString)
            Return False
        End Try
    End Function
#End Region
End Class
