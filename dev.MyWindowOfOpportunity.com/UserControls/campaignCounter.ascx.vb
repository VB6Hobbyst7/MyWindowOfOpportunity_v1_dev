Imports umbraco
Imports System.Xml.XPath
Imports Common
Imports System.Data
Imports umbraco.Core.Models
Imports umbraco.Web

Partial Class UserControls_campaignCounter
    Inherits System.Web.UI.UserControl

#Region "Properties"
    Dim _uHelper As Uhelper = New Uhelper()
    Private dtCounter As DataTable
    Private counter As Integer = 0
#End Region

#Region "Handles"
    Private Sub Masterpages_List_Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try 'Create table
            dtCounter = createCounterTable()
            '
            LoopThruSubcategories()

            'loop thru campaigns, obtain all
            dtCounter = ObtainCampaigns(dtCounter, _uHelper.Get_IPublishedContentByID(siteNodes.Campaigns))

            'Clean up datatable
            cleanDatatable(dtCounter)

            '
            gv.DataSource = dtCounter
            gv.DataBind()

            ltrlCount.Text = counter.ToString
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\UserControls_campaignCounter.ascx.vb : Masterpages_List_Page_Load()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
    End Sub
#End Region

#Region "Methods"
    Private Function createCounterTable() As DataTable

        Try 'Instantiate variables
            Dim dt As DataTable = New DataTable
            '
            dt.Columns.Add("Category", GetType(String))
            dt.Columns.Add("Subcategory", GetType(String))
            dt.Columns.Add("Count", GetType(Integer))

            Return dt
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\UserControls_campaignCounter.ascx.vb : createCounterTable()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return Nothing
        End Try
    End Function
    Private Sub LoopThruSubcategories()
        Try  '
            AddToTable(prevalues.Subcategories_Artistic, Categories.Arts)
            AddToTable(prevalues.Subcategories_Business, Categories.Business)
            AddToTable(prevalues.Subcategories_Charity, Categories.Charity)
            AddToTable(prevalues.Subcategories_Community, Categories.Community)
            AddToTable(prevalues.Subcategories_Science, Categories.Science)
            AddToTable(prevalues.Subcategories_SelfHelp, Categories.SelfHelp)
            AddToTable(prevalues.Subcategories_Software, Categories.Software)
            AddToTable(prevalues.Subcategories_Technology, Categories.Technology)
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\UserControls_campaignCounter.ascx.vb : LoopThruSubcategories()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
    End Sub
    Private Sub AddToTable(ByVal datatypeId As Integer, ByVal Category As String)
        Try 'Instantiate variables
            Dim preValueRootElementIterator As XPathNodeIterator
            Dim preValueIterator As XPathNodeIterator

            'Obtain datatype prevalue.  (Must move to first object entry or else iterator will return null.)
            preValueRootElementIterator = umbraco.library.GetPreValues(datatypeId)
            preValueRootElementIterator.MoveNext()

            'Obtain datatype's prevalues as list
            preValueIterator = preValueRootElementIterator.Current.SelectChildren("preValue", "")

            'Loop thru list and obtain a key/value set.
            While preValueIterator.MoveNext()
                Dim dr As DataRow = dtCounter.NewRow

                'categoryListItems.Add(New ListItem(preValueIterator.Current.Value.ToUpper, preValueIterator.Current.Value.Replace(" ", "")))
                dr("Category") = Category
                dr("Subcategory") = preValueIterator.Current.Value.ToUpper
                dr("Count") = 0

                'Add new row to datatable
                dtCounter.Rows.Add(dr)
                dtCounter.AcceptChanges()
            End While
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\UserControls_campaignCounter.ascx.vb : AddToTable()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
    End Sub
    Private Function ObtainCampaigns(ByRef dt As DataTable, ByRef thisNode As IPublishedContent) As DataTable
        Try '
            Select Case thisNode.DocumentTypeAlias
                Case docTypes.Campaign
                    addCampaignToTable(dt, thisNode)
                    counter += 1
                Case Else
                    For Each childNode As IPublishedContent In thisNode.Children
                        ObtainCampaigns(dt, childNode)
                    Next
            End Select

            Return dt
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\UserControls_campaignCounter.ascx.vb : ObtainCampaigns()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return Nothing
        End Try
    End Function
    Private Sub addCampaignToTable(ByRef dt As DataTable, ByRef thisNode As IPublishedContent)
        Try  '
            If thisNode.HasProperty(Campaigns.artisticSubcategory) Then
                Dim subCategoryData As String = thisNode.GetPropertyValue(Of String)(Campaigns.artisticSubcategory)
                Dim subCategories As String() = subCategoryData.Split(",")
                For Each subCategory As String In subCategories
                    Dim result() As DataRow = dt.Select("Category = '" & Categories.Arts & "' AND Subcategory = '" & subCategory & "'")
                    For Each dr As DataRow In result
                        dr("Count") = dr("Count") + 1
                    Next
                Next
            End If

            If thisNode.HasProperty(Campaigns.businessSubcategories) Then
                Dim subCategoryData As String = thisNode.GetPropertyValue(Of String)(Campaigns.businessSubcategories)
                Dim subCategories As String() = subCategoryData.Split(",")
                For Each subCategory As String In subCategories
                    Dim result() As DataRow = dt.Select("Category = '" & Categories.Business & "' AND Subcategory = '" & subCategory & "'")
                    For Each dr As DataRow In result
                        dr("Count") = dr("Count") + 1
                    Next
                Next
            End If

            If thisNode.HasProperty(Campaigns.charitySubcategory) Then
                Dim subCategoryData As String = thisNode.GetPropertyValue(Of String)(Campaigns.charitySubcategory)
                Dim subCategories As String() = subCategoryData.Split(",")
                For Each subCategory As String In subCategories
                    Dim result() As DataRow = dt.Select("Category = '" & Categories.Charity & "' AND Subcategory = '" & subCategory & "'")
                    For Each dr As DataRow In result
                        dr("Count") = dr("Count") + 1
                    Next
                Next
            End If

            If thisNode.HasProperty(Campaigns.communitySubcategories) Then
                Dim subCategoryData As String = thisNode.GetPropertyValue(Of String)(Campaigns.communitySubcategories)
                Dim subCategories As String() = subCategoryData.Split(",")
                For Each subCategory As String In subCategories
                    Dim result() As DataRow = dt.Select("Category = '" & Categories.Community & "' AND Subcategory = '" & subCategory & "'")
                    For Each dr As DataRow In result
                        dr("Count") = dr("Count") + 1
                    Next
                Next
            End If

            If thisNode.HasProperty(Campaigns.scienceSubcategory) Then
                Dim subCategoryData As String = thisNode.GetPropertyValue(Of String)(Campaigns.scienceSubcategory)
                Dim subCategories As String() = subCategoryData.Split(",")
                For Each subCategory As String In subCategories
                    Dim result() As DataRow = dt.Select("Category = '" & Categories.Science & "' AND Subcategory = '" & subCategory & "'")
                    For Each dr As DataRow In result
                        dr("Count") = dr("Count") + 1
                    Next
                Next
            End If

            If thisNode.HasProperty(Campaigns.selfHelpSubcategory) Then
                Dim subCategoryData As String = thisNode.GetPropertyValue(Of String)(Campaigns.selfHelpSubcategory)
                Dim subCategories As String() = subCategoryData.Split(",")
                For Each subCategory As String In subCategories
                    Dim result() As DataRow = dt.Select("Category = '" & Categories.SelfHelp & "' AND Subcategory = '" & subCategory & "'")
                    For Each dr As DataRow In result
                        dr("Count") = dr("Count") + 1
                    Next
                Next
            End If

            If thisNode.HasProperty(Campaigns.softwareSubcategory) Then
                Dim subCategoryData As String = thisNode.GetPropertyValue(Of String)(Campaigns.softwareSubcategory)
                Dim subCategories As String() = subCategoryData.Split(",")
                For Each subCategory As String In subCategories
                    Dim result() As DataRow = dt.Select("Category = '" & Categories.Software & "' AND Subcategory = '" & subCategory & "'")
                    For Each dr As DataRow In result
                        dr("Count") = dr("Count") + 1
                    Next
                Next
            End If

            If thisNode.HasProperty(Campaigns.technologySubcategory) Then
                Dim subCategoryData As String = thisNode.GetPropertyValue(Of String)(Campaigns.technologySubcategory)
                Dim subCategories As String() = subCategoryData.Split(",")
                For Each subCategory As String In subCategories
                    Dim result() As DataRow = dt.Select("Category = '" & Categories.Technology & "' AND Subcategory = '" & subCategory & "'")
                    For Each dr As DataRow In result
                        dr("Count") = dr("Count") + 1
                    Next
                Next
            End If

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\UserControls_campaignCounter.ascx.vb : addCampaignToTable()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
    End Sub
    Private Sub cleanDatatable(ByVal dt As DataTable)
        Dim category As String = String.Empty

        For Each dr As DataRow In dt.Rows
            If dr("Category") = category Then
                dr("Category") = String.Empty
            Else
                category = dr("Category")
            End If
        Next
    End Sub

#End Region
End Class
