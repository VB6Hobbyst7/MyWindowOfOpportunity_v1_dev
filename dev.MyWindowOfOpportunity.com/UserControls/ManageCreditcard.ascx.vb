Imports Common
Imports Stripe

Partial Class UserControls_ManageCreditcard
    Inherits System.Web.UI.UserControl

    Private linqMembers As linqMembers = New linqMembers
    Public Function GetCurrentMemberId() As Integer?
        Return linqMembers.GetCurrentMemberId()
    End Function
#Region "Properties"
    Private blMembers As blMembers
#End Region

#Region "Handles"
    Private Sub UserControls_ManageCreditcard_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Try

                'Instantiate variables
                Dim returnMsg As BusinessReturn
                Dim member As Member
                blMembers = New blMembers

                'Set public stripe id
                hfldPublicKey.Value = System.Configuration.ConfigurationManager.AppSettings(Miscellaneous.StripePublicApiKey).ToString()
                'hfldcurrentuserid.Value = GetCurrentMemberId()

                'Obtain member's stripe ids
                'returnMsg = blMembers.getMemberDemographics_byId(blMembers.GetCurrentMemberId(), False, False, False, False, False, True)
                returnMsg = blMembers.getMemberDemographics_byId(GetCurrentMemberId(), False, False, False, False, False, True)

                If returnMsg.isValid AndAlso returnMsg.DataContainer.Count > 0 Then
                    'Extract data
                    member = returnMsg.DataContainer(0)
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
                            'ucAlertMsg_Info.AlertMsg = "A creditcard is assigned to this account."
                            'ucAlertMsg_Info.AdditionalText  = ""

                        End If

                        'Add IDs to hidden fields
                        If Not String.IsNullOrWhiteSpace(member.StripeIDs.customerId) Then hfldCustomerId.Value = member.StripeIDs.customerId
                        If Not String.IsNullOrWhiteSpace(member.StripeIDs.creditCardId) Then hfldCardId.Value = member.StripeIDs.creditCardId
                        If Not String.IsNullOrWhiteSpace(member.StripeIDs.creditCardToken) Then hfldCardToken.Value = member.StripeIDs.creditCardToken

                        'If a card exists, display it
                        If Not String.IsNullOrWhiteSpace(member.StripeIDs.customerId) AndAlso Not String.IsNullOrWhiteSpace(member.StripeIDs.creditCardId) Then
                            Dim cardService As StripeCardService = New StripeCardService With {
                                .ApiKey = System.Configuration.ConfigurationManager.AppSettings(Miscellaneous.StripeApiKey).ToString()
                            }
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
            Catch ex As Exception
                Dim sb As New StringBuilder()
                sb.AppendLine("\UserControls\Partial Class UserControls_ManageCreditcard.ascx.vb : UserControls_ManageCreditcard_Load()")
                saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

            End Try
        End If

    End Sub
#End Region

#Region "Methods"
#End Region
End Class
