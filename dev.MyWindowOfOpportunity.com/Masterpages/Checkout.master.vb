Imports umbraco
Imports umbraco.NodeFactory
Imports Common
Imports Stripe
Imports umbraco.Core.Models
Imports umbraco.Web

Partial Class Masterpages_Checkout
    Inherits MasterPage


#Region "Properties"
    Private blMembers As blMembers
    Private blCampaigns As blCampaigns
    Public _uHelper As Uhelper = New Uhelper()
    Public Property campaignGoal As Decimal = 0
    Public Property campaignPledges As Decimal = 0
    Public Property PhaseGoal As Decimal = 0
#End Region

#Region "Handles"
    Private Sub Masterpages_Checkout_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                'If member is not logged in, send to homepage.
                If IsMemberLoggedIn() Then
                    'Instantiate variables
                    blCampaigns = New blCampaigns

                    Dim thisNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(HttpUtility.UrlDecode(Request.QueryString("nodeId")))
                    hfldCampaignId.Value = thisNode.Id

                    'Obtain stripe client id
                    hfldStripeUserId.Value = blCampaigns.selectStripeUserId_byCampaignId(thisNode.Id)

                    If String.IsNullOrEmpty(hfldStripeUserId.Value) Then
                        'Return user to campaing page
                        Response.Redirect(thisNode.Url, False)
                        HttpContext.Current.ApplicationInstance.CompleteRequest()
                    Else
                        'Obtain data from umbraco.
                        ltrlCampaignTitle.Text = thisNode.Name
                        ltrlTeamName.Text = thisNode.Parent.Name

                        'Credit card information update in step3 // no post-back call, retrieving details from stripe API
                        'Please Check reference in CreditcardManagement.master.vb

                        Dim returnMsg As BusinessReturn
                        Dim member As Member

                        'Set public stripe id
                        hfldPublicKey.Value = ConfigurationManager.AppSettings(Miscellaneous.StripePublicApiKey).ToString()

                        'Obtain member's stripe ids

                        'Is user logged in using social media?
                        If blMembers.isUserLoggedIn_bySocialMedia() Then
                            returnMsg = blMembers.getMemberDemographics_byId(hfldCurrentUserId.Value, False, True, True, True, False, True)
                        Else
                            returnMsg = blMembers.getMemberDemographics_byId(hfldCurrentUserId.Value, False, True, True, False, False, True)
                        End If

                        If returnMsg.isValid Then
                            'Obtain data
                            'thisNode = IPublishedContent.GetCurrent
                            member = returnMsg.DataContainer(0)

                            '
                            If Not IsNothing(member) Then
                                'Billing and Shipping details set to textbox
                                If Not IsNothing(member.BillingInfo) AndAlso Not IsNothing(member.ShippingInfo) Then
                                    txtBillingAddress1.Text = member.BillingInfo.address01
                                    txtBillingAddress2.Text = member.BillingInfo.address02
                                    txtBillingCity.Text = member.BillingInfo.city
                                    txtBillingState.Text = member.BillingInfo.stateProvidence
                                    txtBillingZip.Text = member.BillingInfo.postalCode

                                    txtShippingAddress1.Text = member.ShippingInfo.address01
                                    txtShippingAddress2.Text = member.ShippingInfo.address02
                                    txtShippingCity.Text = member.ShippingInfo.city
                                    txtShippingState.Text = member.ShippingInfo.stateProvidence
                                    txtShippingZip.Text = member.ShippingInfo.postalCode
                                End If

                                'Show alt email is user is a social media user.
                                If Not String.IsNullOrEmpty(member.MembershipProperties.altEmail) Then
                                    pnlAltEmail.Visible = True
                                    txbAltEmail.Text = member.MembershipProperties.altEmail
                                End If






                                LoadCardandCustomer(member)
                            End If
                        End If

                        'Obtain Campaign Overview data
                        Dim isPhaseNumberOne As Boolean = False
                        Dim Phases As IEnumerable(Of IPublishedContent) = thisNode.Children.OfTypes(docTypes.Phases)

                        For Each phase In Phases.First().Children.OfTypes(docTypes.Phase).ToList()
                            'Use data if phase is published
                            If phase.GetPropertyValue(Of Boolean)(nodeProperties.published) Then
                                ' CountPhase += 1
                                campaignGoal += CDec(If(phase.HasProperty(nodeProperties.goal), phase.GetPropertyValue(Of String)(nodeProperties.goal), Nothing))

                                'Obtain list & count of all pledges for phase.
                                Dim blPledges As blPledges = New blPledges
                                Dim lstCampaignPledges As List(Of CampaignPledge) = blPledges.select_byPhaseId(phase.Id)
                                For Each pledge As CampaignPledge In lstCampaignPledges
                                    If pledge.reimbursed = False And pledge.canceled = False And pledge.transactionDeclined = False Then
                                        campaignPledges += pledge.pledgeAmount
                                    End If
                                Next


                                If phase.GetPropertyValue(Of String)(nodeProperties.phaseActive) = True Then

                                    ltrlActivePhase.Text = String.Format("{0}", phase.Name)
                                    PhaseGoal = phase.GetPropertyValue(Of Decimal)(nodeProperties.goal)
                                    ltrPhaseNode.Value = phase.Id
                                    If phase.GetPropertyValue(Of Integer)(nodeProperties.phaseNumber) = 1 Then
                                        isPhaseNumberOne = True
                                    End If
                                End If
                            End If
                        Next

                        'Hidden field for keep track that PhaseGoal is 100% completed or not, if 100% complete then some fee applied on Pledge
                        If Not String.IsNullOrEmpty(PhaseGoal) AndAlso Not String.IsNullOrEmpty(campaignPledges) Then
                            If PhaseGoal > campaignPledges AndAlso isPhaseNumberOne Then
                                ltrGoalBool.Value = False
                            Else
                                ltrGoalBool.Value = True
                            End If
                        End If

                        'Display campaign goal
                        Dim goal As String() = campaignGoal.ToString.Split(".")
                        ltrlGoalDollarAmt.Text = String.Format("{0:c0}", CInt(goal(0)))
                        If goal.Count > 1 Then ltrlGoalCents.Text = goal(1).Substring(0, 2) Else ltrlGoalCents.Text = "00"

                        'Display total pledges
                        Dim pledges As String() = campaignPledges.ToString.Split(".")
                        ltrlPledgedDollarAmt.Text = String.Format("{0:c0}", CInt(pledges(0)))
                        If pledges.Count > 1 Then ltrlPledgedCents.Text = pledges(1).Substring(0, 2) Else ltrlPledgedCents.Text = "00"

                        'Display phase goal
                        Dim Phgoal As String() = PhaseGoal.ToString.Split(".")
                        ltrlPhaseGoal.Text = String.Format("{0:c0}", CInt(Phgoal(0)))
                        If Phgoal.Count > 1 Then ltrlPhaseCents.Text = Phgoal(1).Substring(0, 2) Else ltrlPhaseCents.Text = "00"

                    End If

                    'Set link to service agreements
                    hlnkReadServiceAgr.NavigateUrl = _uHelper.Get_IPublishedContentByID(siteNodes.Legal).Url
                End If
            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\Masterpages\Checkout.master.vb : Masterpages_Checkout_Load()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Response.Write(ex.ToString)
        End Try
    End Sub
    Private Sub LbtnPushBack_ServerClick(sender As Object, e As EventArgs) Handles lbtnPushBack.ServerClick
        '
        Dim thisNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(HttpUtility.UrlDecode(Request.QueryString("nodeId")))
        Response.Redirect(thisNode.Url)
        HttpContext.Current.ApplicationInstance.CompleteRequest()
        '
    End Sub
#End Region

#Region "Methods"
    Private Function IsMemberLoggedIn() As Boolean
        'Instantiate variables
        blMembers = New blMembers
        Dim loginStatus As Web.Models.LoginStatusModel

        'Is current user logged in?
        loginStatus = blMembers.getCurrentLoginStatus()
        If Not loginStatus.IsLoggedIn Then
            'Save current page and go to login page
            Session(Sessions.previousPage) = HttpContext.Current.Request.Url.AbsoluteUri
            Response.Redirect(_uHelper.Get_IPublishedContentByID(siteNodes.Login).Url, False)
            Context.ApplicationInstance.CompleteRequest()
            Return False
        Else
            'Save current loggedin id to hfld
            hfldCurrentUserId.Value = blMembers.GetCurrentMemberId()

            'Ensure that the current user id exists.
            If String.IsNullOrWhiteSpace(hfldCurrentUserId.Value) Then
                'Save current page and go to login page
                Session(Sessions.previousPage) = HttpContext.Current.Request.Url.AbsoluteUri
                Response.Redirect(_uHelper.Get_IPublishedContentByID(siteNodes.Login).Url, False)
                Context.ApplicationInstance.CompleteRequest()
                Return False
            Else
                Return True
            End If
        End If
    End Function

    'Load the stripe card and customer id in hidden fields 
    Private Sub LoadCardandCustomer(ByVal Member As Member)

        'Add IDs to hidden fields

        If Not String.IsNullOrWhiteSpace(Member.StripeIDs.customerId) AndAlso GetStripeCustomerStatus(Member) Then

            hfldCustomerId.Value = Member.StripeIDs.customerId

            If Not String.IsNullOrWhiteSpace(Member.StripeIDs.creditCardToken) Then hfldCardToken.Value = Member.StripeIDs.creditCardToken

            'If a card exists, display it
            If Not String.IsNullOrWhiteSpace(Member.StripeIDs.creditCardId) Then

                hfldCardId.Value = Member.StripeIDs.creditCardId
                Try
                    Dim cardService As StripeCardService = New StripeCardService() With {
                        .ApiKey = System.Configuration.ConfigurationManager.AppSettings(Miscellaneous.StripeApiKey).ToString()
                    }
                    Dim stripeCard As StripeCard = cardService.Get(Member.StripeIDs.customerId, Member.StripeIDs.creditCardId)

                    '
                    If Not String.IsNullOrWhiteSpace(stripeCard.Name) Then hfldName.Value = stripeCard.Name
                    If Not String.IsNullOrWhiteSpace(stripeCard.AddressZip) Then hfldPostalCode.Value = stripeCard.AddressZip
                    If Not String.IsNullOrWhiteSpace(stripeCard.Last4) Then hfldLast4.Value = "xxxxxxxxxxxx" & stripeCard.Last4
                    If Not String.IsNullOrWhiteSpace(stripeCard.ExpirationMonth) Then hfldExpirationMonth.Value = stripeCard.ExpirationMonth
                    If Not String.IsNullOrWhiteSpace(stripeCard.ExpirationYear) Then hfldExpirationYear.Value = stripeCard.ExpirationYear.ToString.Substring(stripeCard.ExpirationYear.ToString.Length - 2)
                    If Not String.IsNullOrWhiteSpace(stripeCard.Brand) Then hfldBrand.Value = stripeCard.Brand

                Catch ex As Exception
                    Dim sb As New StringBuilder()
                    sb.AppendLine("\Masterpages\Checkout.master.vb : Masterpages_Checkout_Load() Sub-error")
                    sb.AppendLine("member.StripeIDs.customerId: " & Member.StripeIDs.customerId)
                    sb.AppendLine("member.StripeIDs.creditCardId: " & Member.StripeIDs.creditCardId)
                    sb.AppendLine("IF THIS CONTINUES TO OCCUR, THEN FIND OUT WHERE THE CARD ID IS BEING UPDATED IN STRIPE AND HAVE UMBRACO UPDATE AS WELL.")
                    saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
                    'Response.Write(ex.ToString)
                End Try
            End If
        Else

            Try
                Dim _stripeCustomer As New StripeCustomer
                _stripeCustomer = CreateStripeCustomerAccount(hfldCurrentUserId.Value)

                If Not IsNothing(_stripeCustomer) Then
                    hfldCustomerId.Value = _stripeCustomer.Id
                    blMembers.InsertStripeCustomerId(hfldCurrentUserId.Value, _stripeCustomer.Id)
                End If

            Catch ex As Exception
                Dim sb As New StringBuilder()
                sb.AppendLine("\Masterpages\Checkout.master.vb : Masterpages_Checkout_Load() Sub-error")
                sb.AppendLine("IF THIS CONTINUES TO OCCUR, THEN FIND OUT WHERE THE Current user ID IS BEING retrive from system or API issue.")
                saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            End Try

        End If




    End Sub

    ' Please Note : some function, we are not using at this moment, but they can be used in future 

    ' This function can used to create Stripe Customer account, it's return the stripeCustomer object.
    Public Function CreateStripeCustomerAccount(ByVal currentUserId As String) As StripeCustomer

        'Instantiate variables
        Dim returnBusiness As BusinessReturn

        Dim StripeCustomerCreateOptions = New StripeCustomerCreateOptions()
        Dim customerService = New StripeCustomerService()
        Dim _member As Member
        Dim _stripeCustomer As New StripeCustomer

        returnBusiness = blMembers.getMemberDemographics_byId(currentUserId, True, False, False, False, False, True)

        If returnBusiness.isValid Then

            _member = returnBusiness.DataContainer(0)

            StripeCustomerCreateOptions.Email = _member.MembershipProperties.email
            StripeCustomerCreateOptions.Description = _member.Demographics.firstName

            ' this line help to create a customer account using the stripe customerservice
            _stripeCustomer = customerService.Create(StripeCustomerCreateOptions)

            If Not IsNothing(_stripeCustomer) Then
                Return _stripeCustomer
            End If

        End If

        Return _stripeCustomer

    End Function

    'This function return customer object based on customer id, it can be used for customer validation  of either customer exisit or not
    Public Function GetStripeCustomerAccount(ByVal customerId As String) As StripeCustomer
        Dim customerService = New StripeCustomerService()
        Dim _stripeCustomer As New StripeCustomer
        _stripeCustomer = customerService.Get(customerId)

        Return _stripeCustomer
    End Function

    ' This function return boolean value, for example if we pass a customer id then if id is vaild and stripe can return true else false
    ' Please note :  if function retrun False value then we don't need to clean the Customer id bcoz we are creating the new customer id and while pledge submission it's auto update in back office
    Public Function GetStripeCustomerStatus(ByVal Member As Member) As Boolean
        Dim customer As New StripeCustomer
        customer = GetStripeCustomerAccount(Member.StripeIDs.customerId)

        If Not IsNothing(customer) AndAlso IsNothing(customer.Deleted) Then
            Return True
        Else

            blMembers.MoveStripeDateToNotes(hfldCurrentUserId.Value)
            Return False
        End If

    End Function

    'This function can delete the stripe customer account from the stripe based on the customerid string as like "cus_BOU1LruK5a2XcU".
    Public Function DeleteStripeCustomerAccount(ByVal customerId As String) As StripeDeleted

        Dim customerService = New StripeCustomerService()
        Dim stripeDelete As New StripeDeleted

        stripeDelete = customerService.Delete(customerId)

        Return stripeDelete

    End Function
#End Region
End Class