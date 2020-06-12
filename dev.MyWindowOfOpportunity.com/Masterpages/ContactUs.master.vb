Imports System.IO
Imports System.Net
Imports System.Net.Mail
Imports System.Net.Mime
Imports Common
Imports umbraco
Imports umbraco.NodeFactory
Imports umbraco.Core.Models
Imports umbraco.Web

Partial Class Masterpages_ContactUs
    Inherits System.Web.UI.MasterPage


#Region "Properties"
    Dim _uHelper As Uhelper = New Uhelper()
    Private Enum view
        contactUs
        thankyou
    End Enum
#End Region

#Region "Handles"
    Private Sub Masterpages_Standard_Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            'Instantiate variables
            Dim thisNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(UmbracoContext.Current.PageId)
            'Populate page with main content
            ltrlTitle.Text = thisNode.Name

            Dim socialNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(siteNodes.Home)

            'Get company info 
            If socialNode.HasProperty(nodeProperties.companyName) Then
                lblCompanyName.Text = socialNode.GetPropertyValue(Of String)(nodeProperties.companyName).Trim
            End If
            If socialNode.HasProperty(nodeProperties.address01) Then
                lblAddress.Text = socialNode.GetPropertyValue(Of String)(nodeProperties.address01).Trim
            End If
            If socialNode.HasProperty(nodeProperties.address02) Then
                lblAddress.Text = lblAddress.Text + " " + socialNode.GetPropertyValue(Of String)(nodeProperties.address02).Trim
            End If
            If socialNode.HasProperty(nodeProperties.city_company) Then
                lblCity.Text = socialNode.GetPropertyValue(Of String)(nodeProperties.city_company).Trim

            End If
            If socialNode.HasProperty(nodeProperties.state_company) Then
                lblState.Text = socialNode.GetPropertyValue(Of String)(nodeProperties.state_company).Trim
            End If
            If socialNode.HasProperty(nodeProperties.postalCode_company) Then
                lblPostalCode.Text = socialNode.GetPropertyValue(Of String)(nodeProperties.postalCode_company).Trim
            End If
            If socialNode.HasProperty(nodeProperties.phone_company) Then
                lblPhone.Text = socialNode.GetPropertyValue(Of String)(nodeProperties.phone_company).Trim
            End If
            If socialNode.HasProperty(nodeProperties.fax_company) Then
                lblFax.Text = socialNode.GetPropertyValue(Of String)(nodeProperties.fax_company).Trim
            End If

            'Get Social Media links 
            If socialNode.HasProperty(nodeProperties.facebookUrl) Then
                anchor_facebook.HRef = socialNode.GetPropertyValue(Of String)(nodeProperties.facebookUrl).Trim
            End If
            If socialNode.HasProperty(nodeProperties.twitterUrl) Then
                anchor_twitter.HRef = socialNode.GetPropertyValue(Of String)(nodeProperties.twitterUrl).Trim
            End If
            If socialNode.HasProperty(nodeProperties.linkedInUrl) Then
                anchor_linkedin.HRef = socialNode.GetPropertyValue(Of String)(nodeProperties.linkedInUrl).Trim
            End If
            If socialNode.HasProperty(nodeProperties.supportEmail) Then
                Dim email = socialNode.GetPropertyValue(Of String)(nodeProperties.supportEmail).Trim
                anchor_supportEmail.HRef = "mailto:" + email
                hlnkEmail.NavigateUrl = "mailto:" + email
                hlnkEmail.Text = email
            End If
        Else
            'lblErrorCaptcha.Text = String.Empty
        End If
    End Sub
    Private Sub btnSubmitMessage_Click(sender As Object, e As EventArgs) Handles btnSubmitMessage.Click
        Try
            If Page.IsValid Then
                'Instantiate variables
                Dim returnObject As New BusinessReturn()
                Dim blContactUs As New blContactUs
                Dim blEmais As New blEmails

                'Submit data
                returnObject = blContactUs.InsertContactUs(txbFullName.Text, txbEmail.Text, txbPhoneNumber.Text, txbSubject.Text, txbMessage.Text, UmbracoContext.Current.PageId)

                'Determine if data has been saved properly
                If returnObject.isValid Then
                    'Build url
                    If returnObject.ReturnMessage = "Success" Then
                        blEmais.sendContactUsEmail(txbFullName.Text, txbEmail.Text, txbPhoneNumber.Text, txbSubject.Text, txbMessage.Text)
                        mvContactUs.ActiveViewIndex = view.thankyou
                    Else
                        saveContactData("Submitted successfully, but did not return a success message.")
                    End If
                Else
                    'hfldErrorMsg.Value = "Error 1: " & returnObject.ExceptionMessage
                    saveContactData(returnObject.ExceptionMessage)
                End If
                'End If
            Else
                'hfldErrorMsg.Value = "Page is not valid"
                saveContactData("Page is not valid")
            End If



        Catch ex As Exception
            saveContactData(ex.ToString)
        End Try
    End Sub
#End Region

#Region "Methods"
    Private Sub saveContactData(ByVal errorMsg As String)
        'Instantiate variables
        Dim sb As New StringBuilder()

        'Compile data
        sb.AppendLine("ContactUs.master.vb : btnSubmitMessage_Click()")
        sb.AppendLine("Full Name: " & txbFullName.Text)
        sb.AppendLine("Email: " & txbEmail.Text)
        sb.AppendLine("Phone Number: " & txbPhoneNumber.Text)
        sb.AppendLine("Subject: " & txbSubject.Text)
        sb.AppendLine("Message: " & txbMessage.Text)
        sb.AppendLine("PageId: " & UmbracoContext.Current.PageId)

        'Save data as error message in umbraco
        saveErrorMessage(getLoggedInMember, errorMsg, sb.ToString())
    End Sub
#End Region
End Class






'submit acct data and obtaining id
'Captcha1.ValidateCaptcha(txtCaptcha.Text.Trim())
'If Not Captcha1.UserValidated Then
'    lblErrorCaptcha.Text = "Invalid Captcha"
'Else
