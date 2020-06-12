Imports umbraco.Web
Imports Common
Imports umbraco.Core.Models

Partial Class UserControls_TopLevel_Footer
    Inherits System.Web.UI.UserControl


#Region "Properties"
    Dim _uHelper As Uhelper = New Uhelper()
#End Region

#Region "Handles"
    Private Sub UserControls_TopLevel_Footer_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            'Instantiate variables
            Dim homeNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(siteNodes.Home)
            Dim index As UInt16 = 0


            With homeNode
                If .HasProperty(nodeProperties.facebookUrl) Then
                    hlnkFacebook.NavigateUrl = .GetPropertyValue(Of String)(nodeProperties.facebookUrl)
                    hlnkFacebook_sm.NavigateUrl = .GetPropertyValue(Of String)(nodeProperties.facebookUrl)
                Else
                    hlnkFacebook.Visible = False
                    hlnkFacebook_sm.Visible = False
                End If

                If .HasProperty(nodeProperties.twitterUrl) Then
                    hlnkTwitter.NavigateUrl = .GetPropertyValue(Of String)(nodeProperties.twitterUrl)
                    hlnkTwitter_sm.NavigateUrl = .GetPropertyValue(Of String)(nodeProperties.twitterUrl)
                Else
                    hlnkTwitter.Visible = False
                    hlnkTwitter_sm.Visible = False
                End If

                If .HasProperty(nodeProperties.linkedInUrl) Then
                    hlnkLinkedIn.NavigateUrl = .GetPropertyValue(Of String)(nodeProperties.linkedInUrl)
                    hlnkLinkedIn_sm.NavigateUrl = .GetPropertyValue(Of String)(nodeProperties.linkedInUrl)
                Else
                    hlnkLinkedIn.Visible = False
                    hlnkLinkedIn_sm.Visible = False
                End If

                'If .HasProperty(nodeProperties.supportEmail) Then
                '    hlnkEmailUs.NavigateUrl += .GetPropertyValue(Of String)(nodeProperties.supportEmail)
                '    hlnkEmailUs_sm.NavigateUrl += .GetPropertyValue(Of String)(nodeProperties.supportEmail)
                'Else
                '    hlnkEmailUs.Visible = False
                '    hlnkEmailUs_sm.Visible = False
                'End If
                hlnkEmailUs.NavigateUrl = _uHelper.Get_IPublishedContentByID(siteNodes.ContactUs).Url
                hlnkEmailUs_sm.NavigateUrl = _uHelper.Get_IPublishedContentByID(siteNodes.ContactUs).Url

                'Copyright info
                ltrlCopyrightTitle.Text = .GetPropertyValue(Of String)(nodeProperties.copyrightTitle)
                ltrlCopyrightTitle_sm.Text = .GetPropertyValue(Of String)(nodeProperties.copyrightTitle)
                ltrlCopyrightYear.Text = Today.Year.ToString
                ltrlCopyrightYear_sm.Text = Today.Year.ToString




                'OBTAIN LINKS FOR FOOTER
                '=================================
                'Instantiate variables
                Dim businessReturn As BusinessReturn
                Dim blSiteManagement = New blSiteManagement

                'Obtain data
                businessReturn = blSiteManagement.obtainFooterNav()

                If businessReturn.isValid Then
                    'Extract data
                    Dim lstNavigationItems As List(Of NavigationItem) = DirectCast(businessReturn.DataContainer(0), List(Of NavigationItem))

                    'Instantiate variables
                    Dim isFirstEntry As Boolean = True
                    Dim phColumn As PlaceHolder = phColumn01
                    Dim blockquote As HtmlGenericControl = New HtmlGenericControl("blockquote")
                    Dim pnl As Panel
                    Dim href As HyperLink
                    Dim h5 As HtmlGenericControl

                    'Add links to footer
                    For Each navItem As NavigationItem In lstNavigationItems
                        '
                        If navItem.isParent Then
                            'Adjust index
                            If isFirstEntry Then
                                isFirstEntry = False 'Ensures the first parent is not moved to the wrong placeholder
                            Else
                                'Increment/Recycle index
                                index += 1
                                If index > 2 Then index = 0
                            End If

                            'Set placeholder according to index
                            Select Case index
                                Case 0
                                    phColumn = phColumn01
                                Case 1
                                    phColumn = phColumn02
                                Case 2
                                    phColumn = phColumn03
                            End Select


                            h5 = New HtmlGenericControl("h5")
                            href = New HyperLink
                            blockquote = New HtmlGenericControl("blockquote")

                            'Create primary category name
                            h5.InnerText = navItem.title.Trim.ToUpper
                            href.NavigateUrl = navItem.navigationUrl
                            href.Controls.Add(h5)
                            phColumn.Controls.Add(href)
                            phColumn.Controls.Add(blockquote)

                        Else
                            'Initialize variables
                            pnl = New Panel
                            'Create hyperlink
                            href = New HyperLink With {
                                .Text = navItem.title.Trim,
                                .NavigateUrl = navItem.navigationUrl
                            }

                            'Add controls
                            pnl.Controls.Add(href)
                            blockquote.Controls.Add(pnl)

                        End If
                    Next

                    'gv.DataSource = lstNavigationItems
                    'gv.DataBind()
                End If


            End With
            'End If

            'Set bug link
            Dim uHelper As UmbracoHelper = New umbraco.Web.UmbracoHelper(umbraco.Web.UmbracoContext.Current)
            hlnkSeeABug.NavigateUrl = uHelper.TypedContent(siteNodes.ContactUs).Url
            hlnkSeeABug_sm.NavigateUrl = uHelper.TypedContent(siteNodes.ContactUs).Url

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\Footer.ascx.vb : UserControls_TopLevel_Footer_Load()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Response.Write("Error: " & ex.ToString)
        End Try

    End Sub
#End Region

#Region "Methods"
#End Region

End Class
