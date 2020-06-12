Imports umbraco
Imports umbraco.NodeFactory
Imports Common
Imports umbraco.Core.Models
Imports umbraco.Core
Imports System.Web.Script.Serialization
Imports System.Net.Http
Imports Stripe
Imports umbraco.Core.Services

Partial Class Masterpages_EditAccount
    Inherits System.Web.UI.MasterPage

#Region "Properties"
    Private blMembers As blMembers
#End Region

#Region "Handles"
    Private Sub Masterpages_EditAccount_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            'Instantiate variables
            blMembers = New blMembers
            'Dim BusinessReturn As BusinessReturn
            'Dim clsMember As Member

            'Is current user logged in?
            Dim loginStatus As Web.Models.LoginStatusModel = blMembers.getCurrentLoginStatus()
            If Not loginStatus.IsLoggedIn Then
                'Save current page and go to login page
                Session(Sessions.previousPage) = HttpContext.Current.Request.Url.AbsoluteUri
                Response.Redirect(New Node(siteNodes.Login).NiceUrl, False)
                HttpContext.Current.ApplicationInstance.CompleteRequest()
            Else
                'Save current loggedin id to hfld
                hfldCurrentUserId.Value = blMembers.GetCurrentMemberId()
                ucUserImageMngr.memberId = hfldCurrentUserId.Value

                'Is user logged in using social media?
                If blMembers.isUserLoggedIn_bySocialMedia() Then
                    'Hide credentials panel and tabs
                    phCredentials.Visible = False
                    phCredentialsMbl.Visible = False
                    phCredentialsPnl.Visible = False
                End If
            End If

            If Not IsPostBack Then
                ucAlertMsgPnl_Success.Visible = False
                ucAlertMsgPnl_Error.Visible = False
            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\Masterpages\EditAccount.master.vb : Masterpages_EditAccount_Load()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
    End Sub
    Private Sub Masterpages_EditAccount_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        GetDetail()
    End Sub

    Public Sub GetDetail()
        Try
            'Instantiate variables
            Dim businessReturn As BusinessReturn
            Dim clsMember As Member
            blMembers = New blMembers
            'Dim pledgesList As New List(Of CampaignPledge)

            'Obtain member data
            If Not String.IsNullOrEmpty(hfldCurrentUserId.Value) AndAlso IsNumeric(hfldCurrentUserId.Value) Then
                businessReturn = blMembers.getMemberDemographics_byId(CInt(hfldCurrentUserId.Value), True, True, True, True, True)

                If businessReturn.isValid Then
                    clsMember = businessReturn.DataContainer(0)

                    'Populate General tab
                    If Not String.IsNullOrEmpty(clsMember.MembershipProperties.loginName) Then txbUserId.Text = clsMember.MembershipProperties.loginName
                    If Not String.IsNullOrEmpty(clsMember.MembershipProperties.email) Then
                        ViewState("oldemail") = clsMember.MembershipProperties.email
                        txbEmail.Text = clsMember.MembershipProperties.email
                    End If
                    txbEmail.Text = clsMember.MembershipProperties.email
                    If Not String.IsNullOrEmpty(clsMember.Demographics.firstName) Then txbFirstName.Text = clsMember.Demographics.firstName
                    If Not String.IsNullOrEmpty(clsMember.Demographics.lastName) Then txbLastName.Text = clsMember.Demographics.lastName
                    If Not String.IsNullOrEmpty(clsMember.MembershipProperties.altEmail) Then txbAltEmail.Text = clsMember.MembershipProperties.altEmail
                    If Not String.IsNullOrEmpty(clsMember.Demographics.briefDescription) Then txbDescription.Text = clsMember.Demographics.briefDescription

                    'Populate Billing tab
                    If Not String.IsNullOrEmpty(clsMember.BillingInfo.address01) Then txbBillingAddress01.Text = clsMember.BillingInfo.address01
                    If Not String.IsNullOrEmpty(clsMember.BillingInfo.address02) Then txbBillingAddress02.Text = clsMember.BillingInfo.address02
                    If Not String.IsNullOrEmpty(clsMember.BillingInfo.city) Then txbBillingCity.Text = clsMember.BillingInfo.city
                    If Not String.IsNullOrEmpty(clsMember.BillingInfo.stateProvidence) Then txbBillingState.Text = clsMember.BillingInfo.stateProvidence
                    If Not String.IsNullOrEmpty(clsMember.BillingInfo.postalCode) Then txbBillingPostalCode.Text = clsMember.BillingInfo.postalCode

                    'Populate Shipping tab
                    If Not String.IsNullOrEmpty(clsMember.ShippingInfo.address01) Then txbShippingAddress01.Text = clsMember.ShippingInfo.address01
                    If Not String.IsNullOrEmpty(clsMember.ShippingInfo.address02) Then txbShippingAddress02.Text = clsMember.ShippingInfo.address02
                    If Not String.IsNullOrEmpty(clsMember.ShippingInfo.city) Then txbShippingCity.Text = clsMember.ShippingInfo.city
                    If Not String.IsNullOrEmpty(clsMember.ShippingInfo.stateProvidence) Then txbShippingState.Text = clsMember.ShippingInfo.stateProvidence
                    If Not String.IsNullOrEmpty(clsMember.ShippingInfo.postalCode) Then txbShippingPostalCode.Text = clsMember.ShippingInfo.postalCode

                    '
                    If (clsMember.PledgeList.Count > 0) Then
                        ucPledgeListPnl_AcctEdit.lstCampaignPledges = clsMember.PledgeList
                    End If

                    'BusinessReturn = blMembers.getMemberDemographics_byId(CInt(hfldCurrentUserId.Value), True, True, True, True)
                    '
                    'TEMP: DISPLAY DATA IN CLASS
                    'tempDisplayData(clsMember)
                End If
            End If



        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\Masterpages\EditAccount.master.vb : Masterpages_EditAccount_PreRender()")

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Redirect to home page if something goes wrong.
            'Response.Redirect("/")
            'Response.Write("Error: " & ex.ToString & "<br />")
        End Try
    End Sub
    Private Sub lbtnUpdateAcct_Click(sender As Object, e As EventArgs) Handles lbtnUpdateAcct.Click
        If Page.IsValid Then
            UpdateDetail()
        End If
    End Sub

    Protected Sub lbtnCreditCard_Click(sender As Object, e As EventArgs)
        Response.Redirect(New Node(siteNodes.CreditCardManager).NiceUrl, False)
        HttpContext.Current.ApplicationInstance.CompleteRequest()
    End Sub
    Protected Sub lbtnBankAcct_Click(sender As Object, e As EventArgs)
        Response.Redirect(New Node(siteNodes.FinancialManager).NiceUrl, False)
        HttpContext.Current.ApplicationInstance.CompleteRequest()
    End Sub
#End Region

#Region "Methods"
    Public Sub UpdateDetail()

        'Instantiate variables
        Dim BusinessReturn As New BusinessReturn
        Dim member As Member = New Member
        Dim Demographics As Demographics = New Demographics
        Dim BillingInfo As BillingInfo = New BillingInfo
        Dim ShippingInfo As ShippingInfo = New ShippingInfo
        Dim MembershipProperties As MembershipProperties = New MembershipProperties

        Try
            '
            Demographics.briefDescription = txbDescription.Text.Trim
            Demographics.firstName = txbFirstName.Text.Trim
            Demographics.lastName = txbLastName.Text.Trim

            '
            BillingInfo.address01 = txbBillingAddress01.Text.Trim
            BillingInfo.address02 = txbBillingAddress02.Text.Trim
            BillingInfo.city = txbBillingCity.Text.Trim
            BillingInfo.stateProvidence = txbBillingState.Text.Trim
            BillingInfo.postalCode = txbBillingPostalCode.Text.Trim

            '
            ShippingInfo.address01 = txbShippingAddress01.Text.Trim
            ShippingInfo.address02 = txbShippingAddress02.Text.Trim
            ShippingInfo.city = txbShippingCity.Text.Trim
            ShippingInfo.stateProvidence = txbShippingState.Text.Trim
            ShippingInfo.postalCode = txbShippingPostalCode.Text.Trim

            '
            MembershipProperties.loginName = txbUserId.Text.Trim
            MembershipProperties.email = txbEmail.Text.Trim
            MembershipProperties.altEmail = txbAltEmail.Text.Trim.ToLower
            MembershipProperties.userId = CInt(hfldCurrentUserId.Value)

            'If a new valid password has been provided, include in class.
            If CBool(hfldAcceptPwChange.Value) = True Then
                MembershipProperties.password = txbPassword.Text.Trim
            End If

            '
            member.Demographics = Demographics
            member.BillingInfo = BillingInfo
            member.ShippingInfo = ShippingInfo
            member.MembershipProperties = MembershipProperties

            'Update member
            BusinessReturn = blMembers.Update(member)


            If BusinessReturn.isValid Then
                'if saved successfully, show saved banner msg.
                UpdateStripeCustomer(txbEmail.Text.Trim)
                If ViewState("oldemail") IsNot Nothing AndAlso ViewState("oldemail") <> txbEmail.Text Then
                    Dim blmembers As blMembers = New blMembers
                    blmembers.logMemberOut()
                    Response.Redirect(New Node(siteNodes.Login).NiceUrl + "?loginupdated=true", False)
                    HttpContext.Current.ApplicationInstance.CompleteRequest()
                End If

                'Show successful msg
                ucAlertMsgPnl_Success.Visible = True
                ucAlertMsgPnl_Error.Visible = False
            Else
                'Show error message
                Dim sb As StringBuilder = New StringBuilder
                sb.Append(BusinessReturn.ExceptionMessage & "<br />")
                For Each msg As ValidationContainer In BusinessReturn.ValidationMessages
                    sb.Append("&mdash;" & msg.ErrorMessage & "<br />")
                Next
                ucAlertMsgPnl_Error.AlertMsg += "<br />" & sb.ToString
                ucAlertMsgPnl_Error.Visible = True
                ucAlertMsgPnl_Success.Visible = False
            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\Masterpages\EditAccount.master.vb : lbtnUpdateAcct_Click()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'if errored, show error msg/banner
            ucAlertMsgPnl_Error.AlertMsg += "<br />" & ex.ToString
            ucAlertMsgPnl_Error.Visible = True
            ucAlertMsgPnl_Success.Visible = False
        End Try

    End Sub
    Public Sub UpdateStripeCustomer(ByVal email As String)
        Dim customerUpdateOptions As StripeCustomerUpdateOptions = New StripeCustomerUpdateOptions
        Dim customerService = New StripeCustomerService(ConfigurationManager.AppSettings(Miscellaneous.StripeApiKey))
        Dim StripeCustomer As StripeCustomer

        Dim MemberService As IMemberService = ApplicationContext.Current.Services.MemberService
        Dim member As IMember = ApplicationContext.Current.Services.MemberService.GetById(CInt(hfldCurrentUserId.Value))

        Try
            'Add values for creating customer.
            customerUpdateOptions.Description = member.Name & " [" & member.Id.ToString() & "]"
            customerUpdateOptions.Email = email

            Dim custid = member.GetValue(nodeProperties.customerId)
            StripeCustomer = customerService.Get(custid)

            'Create customer in stripe
            StripeCustomer = customerService.Update(custid, customerUpdateOptions)

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\Masterpages\EditAccount.vb : UpdateStripeCustomer()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
    End Sub
    'Public Sub tempDisplayData(ByRef clsMember As Member)
    '    'TEMP: DISPLAY DATA IN CLASS
    '    Dim lstDemographics As New List(Of Demographics)
    '    Dim lstBillingInfo As New List(Of BillingInfo)
    '    Dim lstShippingInfo As New List(Of ShippingInfo)
    'Dim lstMembershipProperties As New List(Of MembershipProperties)

    '    lstDemographics.Add(clsMember.Demographics)
    '    lstBillingInfo.Add(clsMember.BillingInfo)
    '    lstShippingInfo.Add(clsMember.ShippingInfo)
    '    lstMembershipProperties.Add(clsMember.MembershipProperties)

    '    gv1.DataSource = lstDemographics
    '    gv2.DataSource = lstBillingInfo
    '    gv3.DataSource = lstShippingInfo
    '    gv4.DataSource = lstMembershipProperties
    '    gv1.DataBind()
    '    gv2.DataBind()
    '    gv3.DataBind()
    '    gv4.DataBind()

    '    pnlTempData.Visible = True
    'End Sub
#End Region


End Class

