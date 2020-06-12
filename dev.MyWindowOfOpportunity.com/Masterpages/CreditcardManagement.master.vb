Imports Umbraco
Imports Umbraco.NodeFactory
Imports Common
Imports Stripe

Partial Class Masterpages_CreditcardManagement
    Inherits System.Web.UI.MasterPage


#Region "Properties"
    Private blMembers As blMembers
#End Region

#Region "Handles"
    Private Sub Masterpages_CreditcardManagement_Load(sender As Object, e As EventArgs) Handles Me.Load
        'If member is not logged in, send to homepage.
        isMemberLoggedIn()

        If Not IsPostBack Then
            'Instantiate variables
            Dim returnMsg As BusinessReturn
            Dim member As Member

            'Set public stripe id
            hfldPublicKey.Value = System.Configuration.ConfigurationManager.AppSettings(Miscellaneous.StripePublicApiKey).ToString()

            'Obtain member's stripe ids
            returnMsg = blMembers.getMemberDemographics_byId(hfldCurrentUserId.Value, False, False, False, False, False, True)

            If returnMsg.isValid Then

                'Obtain data
                member = returnMsg.DataContainer(0)

                '
                If Not IsNothing(member) Then

                    If String.IsNullOrWhiteSpace(member.StripeIDs.creditCardId) Then
                        'No card exists for member
                        '==================================
                        'Show proper elements
                        pnlUpdate.CssClass += " hide"
                        'pnlSubmit.Visible = True
                        'pnlUpdate.Visible = False

                    Else
                        'Card already exists for member
                        '==================================
                        'Show proper elements
                        pnlSubmit.CssClass += " hide"
                        'pnlSubmit.Visible = False
                        'pnlUpdate.Visible = True
                        hfldShowAlertMsg_Info.Value = True
                        ucAlertMsg_Info.AlertMsg = "A credit card is assigned to this account."
                        'ucAlertMsg_Info.AdditionalText  = ""

                    End If

                    'Add IDs to hidden fields
                    If Not String.IsNullOrWhiteSpace(member.StripeIDs.customerId) Then hfldCustomerId.Value = member.StripeIDs.customerId
                    If Not String.IsNullOrWhiteSpace(member.StripeIDs.creditCardId) Then hfldCardId.Value = member.StripeIDs.creditCardId
                    If Not String.IsNullOrWhiteSpace(member.StripeIDs.creditCardToken) Then hfldCardToken.Value = member.StripeIDs.creditCardToken

                    'If a card exists, display it
                    If Not String.IsNullOrWhiteSpace(member.StripeIDs.customerId) AndAlso Not String.IsNullOrWhiteSpace(member.StripeIDs.creditCardId) Then
                        Dim cardService As StripeCardService = New StripeCardService()
                        cardService.ApiKey = System.Configuration.ConfigurationManager.AppSettings(Miscellaneous.StripeApiKey).ToString()
                        Dim stripeCard As StripeCard = cardService.Get(member.StripeIDs.customerId, member.StripeIDs.creditCardId)

                        '
                        If Not String.IsNullOrWhiteSpace(stripeCard.Name) Then hfldName.Value = stripeCard.Name
                        If Not String.IsNullOrWhiteSpace(stripeCard.AddressZip) Then hfldPostalCode.Value = stripeCard.AddressZip
                        If Not String.IsNullOrWhiteSpace(stripeCard.Last4) Then hfldLast4.Value = "xxxxxxxxxxxx" & stripeCard.Last4
                        If Not String.IsNullOrWhiteSpace(stripeCard.ExpirationMonth) Then hfldExpirationMonth.Value = stripeCard.ExpirationMonth
                        If Not String.IsNullOrWhiteSpace(stripeCard.ExpirationYear) Then hfldExpirationYear.Value = stripeCard.ExpirationYear.ToString.Substring(stripeCard.ExpirationYear.ToString.Length - 2)
                        If Not String.IsNullOrWhiteSpace(stripeCard.Brand) Then hfldBrand.Value = stripeCard.Brand


                        ''TEMP
                        'Dim lst As New List(Of StripeCard)
                        'lst.Add(stripeCard)
                        'gv.DataSource = lst
                        'gv.DataBind()
                    End If
                End If
            End If
        End If
    End Sub
#End Region

#Region "Methods"
    Private Sub isMemberLoggedIn()
        'Instantiate variables
        blMembers = New blMembers
        Dim loginStatus As Web.Models.LoginStatusModel

        'Is current user logged in?
        loginStatus = blMembers.getCurrentLoginStatus()
        If Not loginStatus.IsLoggedIn Then
            'Save current page and go to login page
            Session(Sessions.previousPage) = HttpContext.Current.Request.Url.AbsoluteUri
            Response.Redirect(New Node(siteNodes.Login).NiceUrl, False)
            HttpContext.Current.ApplicationInstance.CompleteRequest()
        Else
            'Save current loggedin id to hfld
            hfldCurrentUserId.Value = blMembers.GetCurrentMemberId()
        End If
    End Sub
#End Region

End Class