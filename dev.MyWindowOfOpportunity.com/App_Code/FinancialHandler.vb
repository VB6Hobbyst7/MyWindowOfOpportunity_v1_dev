Imports System.Web.Script.Services
Imports System.Web.Services
Imports Newtonsoft.Json
Imports Stripe
Imports Common
Imports umbraco.Core
Imports umbraco.Core.Publishing



' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<System.Web.Script.Services.ScriptService()>
<WebService(Namespace:="http://tempuri.org/")>
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Public Class FinancialHandler
    Inherits System.Web.Services.WebService


#Region "Properties"
    Private linqMembers As linqMembers = New linqMembers
    Private blMembers As blMembers
    Private blEmails As blEmails
    Private statusCode As Integer = 200 '200 = OK; 400 = Bad Request
    Private errorMsg As String = String.Empty
    Private _uHelper As Uhelper = New Uhelper()
#End Region

#Region "Public WebMethods"
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function GetAcctData(ByVal userId As String) As Member
        'Instantiate variables
        Dim clsMember As Member

        Try
            'Instantiate variables
            blMembers = New blMembers
            Dim BusinessReturn As BusinessReturn

            'Obtain member's demographics
            BusinessReturn = blMembers.getMemberDemographics_byId(CInt(userId), True, True, False, True, False)
            clsMember = BusinessReturn.DataContainer(0)

            Return clsMember
        Catch ex As Exception
            clsMember = New Member
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\FinancialHandler.vb : GetAcctData()")
            sb.AppendLine("userId:" & userId)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return clsMember
        End Try
    End Function
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function CreateStripeAcct(ByVal currentUserId As String, ByVal firstName As String, ByVal lastName As String, ByVal email As String,
                                ByVal dob As String, ByVal address01 As String, ByVal address02 As String, ByVal city As String,
                                ByVal state As String, ByVal postalCode As String, ByVal country As String, ByVal ssn As String) As Member
        'Instantiate variables
        Dim clsMember As Member
        Dim icurrentUserId = GetCurrentMemberId()
        Try
            'Instantiate variables
            clsMember = New Member

            'Add all values into member's class
            clsMember.MembershipProperties.userId = currentUserId
            clsMember.MembershipProperties.email = email

            clsMember.BillingInfo.address01 = address01
            clsMember.BillingInfo.address02 = address02
            clsMember.BillingInfo.city = city
            clsMember.BillingInfo.stateProvidence = state
            clsMember.BillingInfo.postalCode = postalCode
            clsMember.BillingInfo.country = country

            clsMember.Demographics.firstName = firstName
            clsMember.Demographics.lastName = lastName
            clsMember.Demographics.ssn = ssn
            If Not String.IsNullOrWhiteSpace(dob) AndAlso IsDate(dob) Then clsMember.Demographics.dateOfBirth = CDate(dob)

            'Create stripe acct
            CreateStripeAcct(clsMember)

            '
            Return clsMember
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\FinancialHandler.vb : CreateStripeAcct()")
            sb.AppendLine("currentUserId:" & currentUserId)
            sb.AppendLine("firstName:" & firstName)
            sb.AppendLine("lastName:" & lastName)
            sb.AppendLine("email:" & email)
            sb.AppendLine("dob:" & dob)
            sb.AppendLine("address01:" & address01)
            sb.AppendLine("address02:" & address02)
            sb.AppendLine("city:" & city)
            sb.AppendLine("state:" & state)
            sb.AppendLine("postalCode:" & postalCode)
            sb.AppendLine("country:" & country)
            sb.AppendLine("ssn:" & ssn)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            clsMember = New Member With {
                .errorMsg = "Error: " & ex.ToString
            }
            Return clsMember
        End Try
    End Function
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function ConvertTokenToBankAcctId(ByVal currentUserId As String, ByVal campaignAcctId As String, ByVal bankAcctToken As String) As Member
        'Instantiate variables
        Dim clsMember As Member

        Try
            'Instantiate variables
            clsMember = New Member
            blMembers = New blMembers
            Dim businessResult As BusinessReturn
            Dim StripeAccountUpdateOptions As New StripeAccountUpdateOptions
            Dim bankAccountOptions As New StripeAccountBankAccountOptions
            Dim accountService = New StripeAccountService()
            Dim stripeAcct As StripeAccount

            'Add token as bank account
            bankAccountOptions.TokenId = bankAcctToken
            StripeAccountUpdateOptions.ExternalBankAccount = bankAccountOptions

            'Update account with new bank account
            stripeAcct = accountService.Update(campaignAcctId, StripeAccountUpdateOptions)

            'Add values into member's class
            clsMember.MembershipProperties.userId = currentUserId
            clsMember.StripeIDs.bankAcctToken = bankAcctToken
            clsMember.StripeIDs.bankAcctId = stripeAcct.ExternalAccounts.Data.Item(0).BankAccount.Id

            'Save campaign acct id to umbraco member
            businessResult = blMembers.InsertStripeIDs(clsMember)

            'Add error to member class if exists
            If Not businessResult.isValid Then
                clsMember.errorMsg = businessResult.ExceptionMessage
            End If

            Return clsMember
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\FinancialHandler.vb : ConvertTokenToBankAcctId()")
            sb.AppendLine("currentUserId:" & currentUserId)
            sb.AppendLine("campaignAcctId:" & campaignAcctId)
            sb.AppendLine("bankAcctToken:" & bankAcctToken)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            clsMember = New Member With {
                .errorMsg = "Error: " & ex.ToString
            }
            Return clsMember
        End Try
    End Function
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function RetrieveAcctData_byId(ByVal campaignAcctId As String) As Member
        Try  'Retrieve account data
            Return RetrieveAcctData(campaignAcctId)
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\FinancialHandler.vb : RetrieveAcctData_byId()")
            sb.AppendLine("campaignAcctId:" & campaignAcctId)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return Nothing
        End Try
    End Function
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function AddCCTokenToCustomer(ByVal currentUserId As String, ByVal customerId As String, ByVal ccToken As String) As Member
        'Instantiate variables 
        Dim member As Member = New Member
        Dim icurrentUserId = GetCurrentMemberId()

        'Dim loginStatus As umbraco.Web.Models.LoginStatusModel = blMembers.getCurrentLoginStatus()
        'Dim loginStatus As umbraco.Web.Models.LoginStatusModel

        ''Is current user logged in?
        'loginStatus = blMembers.getCurrentLoginStatus()

        '//HiddenField2.value = blMembers.GetCurrentMemberId()
        Try
            If String.IsNullOrWhiteSpace(icurrentUserId) Or String.IsNullOrWhiteSpace(customerId) Or String.IsNullOrWhiteSpace(ccToken) Then
                Dim sb As New StringBuilder()
                sb.AppendLine("FinancialHandler.vb : AddCCTokenToCustomer()")
                sb.AppendLine("IsNullOrWhiteSpace(currentUserId)")
                sb.AppendLine("currentUserId:" & currentUserId)
                sb.AppendLine("customerId:" & customerId)
                sb.AppendLine("ccToken:" & ccToken)
                saveErrorMessage(getLoggedInMember, sb.ToString, sb.ToString())

                'Return error message
                member.errorMsg = "We apologize, but there was an error submitting your pledge. Error: A parameter is empty."
                Return member
            Else
                'Instantiate variables
                Dim businessResult As BusinessReturn
                Dim blMembers As blMembers = New blMembers
                Dim customerUpdateOptions As New StripeCustomerUpdateOptions()
                Dim customerService As New StripeCustomerService()

                'Add token to update option
                customerUpdateOptions.SourceToken = ccToken

                'Submit token to be connected to the customer
                Dim customer As StripeCustomer = customerService.Update(customerId, customerUpdateOptions)

                'Add stripe data to member class
                member.MembershipProperties.userId = CInt(icurrentUserId)
                member.StripeIDs.customerId = customerId
                member.StripeIDs.creditCardToken = ccToken
                member.StripeIDs.creditCardId = customer.Sources.Data(0).Card.Id

                'Save Stripe IDs to umbraco member
                businessResult = blMembers.InsertStripeIDs(member)

                'Show result msg
                If businessResult.isValid Then
                    Return member
                Else
                    member.errorMsg = "Error: " & businessResult.ExceptionMessage
                    Return member
                End If
            End If
            'Stripe Exception
        Catch ex As StripeException

            Dim sb As New StringBuilder()
            sb.AppendLine("FinancialHandler.vb : AddCCTokenToCustomer()")
            sb.AppendLine("StripeException")
            sb.AppendLine("currentUserId:" & icurrentUserId)
            sb.AppendLine("customerId:" & customerId)
            sb.AppendLine("ccToken:" & ccToken)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

            member.errorMsg = ex.Message
            Return member
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("FinancialHandler.vb : AddCCTokenToCustomer()")
            sb.AppendLine("Exception")
            sb.AppendLine("currentUserId:" & icurrentUserId)
            sb.AppendLine("customerId:" & customerId)
            sb.AppendLine("ccToken:" & ccToken)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            If Not (member.errorMsg.Length > 0) Then
                member.errorMsg = "Error: " & ex.ToString
            End If

            Return member
        End Try
    End Function
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function DeleteCreditcard(ByVal currentUserId As String, ByVal customerId As String, ByVal cardId As String) As Member
        'Instantiate variables
        Dim member As Member = New Member
        Dim returnMsg As BusinessReturn
        Dim cardService = New StripeCardService()
        blMembers = New blMembers

        Try
            'Submit creditcard deletion from stripe
            Dim stripeDeleted As StripeDeleted = cardService.Delete(customerId, cardId)

            If stripeDeleted.Deleted Then
                'Delete ID from umbraco
                returnMsg = blMembers.DeleteCreditcard(CInt(currentUserId))

                'Show result
                If returnMsg.isValid Then
                    'Successful.  return member class
                    Return member
                Else
                    'show error msg
                    member.isValid = False
                    member.errorMsg = returnMsg.ExceptionMessage
                    Return member
                End If

            Else
                'Stripe failed to delete creditcard
                member.isValid = False
                member.errorMsg = stripeDeleted.StripeResponse.ToString
                Return member
            End If

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\FinancialHandler.vb : DeleteCreditcard()")
            sb.AppendLine("currentUserId:" & currentUserId)
            sb.AppendLine("customerId:" & customerId)
            sb.AppendLine("cardId:" & cardId)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            member.isValid = False
            member.errorMsg = "Error: " & ex.ToString
            Return member
        End Try

    End Function
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function SubmitPledge(ByVal currentUserId As String, ByVal CustomerId As String, ByVal creditcardId As String, ByVal activePhaseNode As String, ByVal SelectedReward As String,
                                 ByVal PledgeAmount As String, ByVal ShowAsAnonymous As String, ByVal BillingAdd1 As String, ByVal BillingAdd2 As String, ByVal BillingCity As String,
                                 ByVal BillingState As String, ByVal BillingZip As String, ByVal ShippingAdd1 As String, ByVal ShippingAdd2 As String, ByVal ShippingCity As String,
                                 ByVal ShippingState As String, ByVal ShippingZip As String, ByVal campaignId As String, ByVal stripeUserId As String, ByVal alternativeEmail As String) As StripeReturn

        Dim stripeReturn As New StripeReturn

        Try
            'Instantiate variables
            blMembers = New blMembers
            blEmails = New blEmails
            Dim businessReturn As BusinessReturn = blMembers.getMemberDemographics_byId(currentUserId, False, True, True, True, False, True)
            Dim bUpdateReturn As BusinessReturn
            Dim saveToUmbracoReturn As BusinessReturn
            Dim member As Member
            Dim blPledges As New blPledges

            If businessReturn.isValid Then
                'Obtain member from return
                member = DirectCast(businessReturn.DataContainer(0), Member)

                'ERROR
                If IsNothing(member) Then
                    stripeReturn.Failed = ("Member is Nothing.  FIX!  CurrentUserId: " & currentUserId)
                End If

                'Update member data with new data
                member.MembershipProperties.userId = currentUserId
                member.BillingInfo.address01 = BillingAdd1
                member.BillingInfo.address02 = BillingAdd2
                member.BillingInfo.city = BillingCity
                member.BillingInfo.stateProvidence = BillingState
                member.BillingInfo.postalCode = BillingZip

                member.ShippingInfo.address01 = ShippingAdd1
                member.ShippingInfo.address02 = ShippingAdd2
                member.ShippingInfo.city = ShippingCity
                member.ShippingInfo.stateProvidence = ShippingState
                member.ShippingInfo.postalCode = ShippingZip

                member.MembershipProperties.altEmail = alternativeEmail.Trim.ToLower

                'Update account information in umbraco
                bUpdateReturn = blMembers.UpdateAtCheckout(member)

                If bUpdateReturn.isValid Then
                    'Submit pledge to stripe
                    Dim stripeCharge As StripeCharge = SubmitPledgeToStripe(currentUserId, CustomerId, creditcardId, activePhaseNode, SelectedReward, PledgeAmount, campaignId, stripeUserId)

                    If IsNothing(stripeCharge) Then
                        stripeReturn.Failed = "The card has been declined for an unknown reason. Please try again. If subsequent payments are declined, you should contact your bank for more information."
                    Else
                        Select Case stripeCharge.Status
                            Case "succeeded"
                                saveToUmbracoReturn = blPledges.CreatePledge(stripeCharge, CInt(campaignId), activePhaseNode, SelectedReward, CInt(currentUserId), CBool(ShowAsAnonymous))
                                blEmails.sendPledgEmail_Successful(saveToUmbracoReturn, stripeCharge, member)

                            Case Else
                                '
                                stripeReturn.Failed = stripeCharge.FailureMessage
                        End Select
                    End If

                    '
                    stripeReturn.GetJson = JsonConvert.SerializeObject(stripeCharge)
                Else
                    '
                    stripeReturn.Failed = bUpdateReturn.ExceptionMessage
                End If
            Else
                stripeReturn.Failed = "Member failed to be obtained.  = Nothing"
            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\FinancialHandler.vb : SubmitPledge()")
            sb.AppendLine("currentUserId:" & currentUserId)
            sb.AppendLine("customerId:" & CustomerId)
            sb.AppendLine("creditcardId:" & creditcardId)
            sb.AppendLine("activePhaseNode:" & activePhaseNode)
            sb.AppendLine("SelectedReward:" & SelectedReward)
            sb.AppendLine("PledgeAmount:" & PledgeAmount)
            sb.AppendLine("BillingAdd1:" & BillingAdd1)
            sb.AppendLine("BillingAdd2:" & BillingAdd2)
            sb.AppendLine("BillingCity:" & BillingCity)
            sb.AppendLine("BillingState:" & BillingState)
            sb.AppendLine("BillingZip:" & BillingZip)
            sb.AppendLine("ShippingAdd1:" & ShippingAdd1)
            sb.AppendLine("ShippingAdd2:" & ShippingAdd2)
            sb.AppendLine("ShippingCity:" & ShippingCity)
            sb.AppendLine("ShippingState:" & ShippingState)
            sb.AppendLine("ShippingZip:" & ShippingZip)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'member.errorMsg = "Error: " & ex.ToString
            'Return member
            stripeReturn.Failed = "Error: " & ex.ToString
        End Try
        Return stripeReturn
    End Function
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function SaveShippingDetails(ByVal pledgeId As String, ByVal rewardShipped As String, ByVal trackingNo As String, ByVal campaignMngrNotes As String) As IsValid
        'Scope variables
        Dim isValid As New IsValid

        Try
            'Instantiate variables
            Dim blPledges As blPledges
            Dim isRewardShipped As Boolean = False
            Dim publishAttempt As Attempt(Of PublishStatus)

            If IsNumeric(pledgeId) Then
                'Convert to boolean
                If Boolean.TryParse(rewardShipped, isRewardShipped) Then
                    isRewardShipped = Boolean.Parse(rewardShipped)
                End If

                'Submit pledge notes
                blPledges = New blPledges
                publishAttempt = blPledges.UpdatePledgeMngrNotes(CInt(pledgeId), isRewardShipped, trackingNo, campaignMngrNotes)

                'Save result info
                isValid.isValid = publishAttempt.Success
                isValid.resultMsg = JsonConvert.SerializeObject(publishAttempt)
            Else
                'Error saving shipping details.  No pledge id
                Dim sb As New StringBuilder()
                sb.AppendLine("FinancialHandler.vb : SaveShippingDetails()")
                sb.AppendLine("Error saving shipping details.  No pledge id")
                sb.AppendLine("pledgeId:" & pledgeId)
                sb.AppendLine("rewardShipped:" & rewardShipped)
                sb.AppendLine("trackingNo:" & trackingNo)
                sb.AppendLine("campaignMngrNotes:" & campaignMngrNotes)
                saveErrorMessage(getLoggedInMember, sb.ToString, sb.ToString())

                isValid.isValid = False
                isValid.resultMsg = sb.ToString
            End If

            Return isValid

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("FinancialHandler.vb : SaveShippingDetails()")
            sb.AppendLine("pledgeId:" & pledgeId)
            sb.AppendLine("fulfilled:" & rewardShipped)
            sb.AppendLine("trackingNo:" & trackingNo)
            sb.AppendLine("campaignMngrNotes:" & campaignMngrNotes)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

            isValid.isValid = False
            isValid.resultMsg = "ERROR: " + ex.ToString + " | " + sb.ToString()
            Return isValid
        End Try


    End Function
#End Region

#Region "Private Methods"
    Private Sub CreateStripeAcct(ByRef _member As Member)
        Try
            'Instantiate variables
            blMembers = New blMembers
            Dim businessResult As BusinessReturn
            Dim stripeAcctCreateOptions = New StripeAccountCreateOptions()
            Dim stripeLegalEntity = New StripeAccountLegalEntityOptions
            Dim stripeAcctService = New StripeAccountService()
            Dim stripeAcct As StripeAccount
            Dim dob As Date = _member.Demographics.dateOfBirth

            'Compile new campaign account in stripe
            With stripeAcctCreateOptions
                .Email = _member.MembershipProperties.email
                '.Managed = True
                '.Type = "custom"
                .Country = _member.BillingInfo.country
                .ProductDescription = "Member Id: " & _member.MembershipProperties.userId
                .TosAcceptanceDate = DateTime.UtcNow  'GetCurrentUnixTimestamp()
                .TosAcceptanceIp = GetIPv4Address()
            End With

            'Add additional data to stripe campaign account
            With stripeLegalEntity
                .Type = "individual"
                .FirstName = _member.Demographics.firstName
                .LastName = _member.Demographics.lastName
                .BirthDay = dob.Day
                .BirthMonth = dob.Month
                .BirthYear = dob.Year
                .AddressLine1 = _member.BillingInfo.address01
                .AddressLine2 = _member.BillingInfo.address02
                .AddressCity = _member.BillingInfo.city
                .AddressState = _member.BillingInfo.stateProvidence
                .AddressPostalCode = _member.BillingInfo.postalCode
                .AddressTwoLetterCountry = _member.BillingInfo.country
                .PersonalAddressTwoLetterCountry = _member.BillingInfo.country
                .PersonalIdNumber = GetIntOnly(_member.Demographics.ssn)
                If .PersonalIdNumber.Length > 4 Then 'Get last 4 of ssn
                    .SSNLast4 = .PersonalIdNumber.Substring(.PersonalIdNumber.Length - 4)
                End If
            End With

            'Add legal entity to stripe creation options
            stripeAcctCreateOptions.LegalEntity = stripeLegalEntity

            'Create new campaign account in stripe
            stripeAcct = stripeAcctService.Create(stripeAcctCreateOptions)

            'Save campaign account id to class
            _member.StripeIDs.campaignAcctId = stripeAcct.Id

            'Save campaign acct id to umbraco member
            businessResult = blMembers.InsertStripeIDs(_member)

            'Add error to member class if exists
            If Not businessResult.isValid Then
                _member.errorMsg = businessResult.ExceptionMessage
            End If

            'Class is a ByRef... return not needed.
        Catch ex As StripeException
            'Obtain error messages
            _member.errorMsg = "<br />Error: " & ex.HttpStatusCode.ToString
            _member.errorMsg += " | " & ex.Message
            If Not IsNothing(ex.InnerException) Then _member.errorMsg += " | " & ex.InnerException.ToString
            _member.errorMsg += " | " & ex.StripeError.ErrorType

            'Determine what kind of error had occured
            Select Case ex.StripeError.ErrorType
                Case "card_error"
                    _member.errorMsg += "<br />card_error: " & ex.StripeError.Code & " | " & ex.StripeError.Message
                    ' or better yet, handle based on error code: exception.StripeError.Code
                    Exit Select
                Case "api_error"
                    _member.errorMsg += "<br />api_error"
                    Exit Select
                Case "invalid_request_error"
                    'do some stuff
                    _member.errorMsg += "<br />invalid_request_error"
                    Exit Select
                Case Else
                    Throw
            End Select

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\FinancialHandler.vb : CreateStripeAcct()")
            sb.AppendLine("_member:" & _member.ToString())

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            _member.errorMsg += "Error: " & ex.ToString
        End Try
    End Sub
    Private Function RetrieveAcctData(ByVal campaignAcctId As String) As Member
        'Instantiate variables
        Dim clsMember As Member = New Member

        Try
            'Instantiate variables
            blMembers = New blMembers
            Dim businessResult As BusinessReturn


            'MOVE THIS TO DATALAYER
            '=============================================================
            'businessResult = New BusinessReturn

            'Dim accountService = New StripeAccountService()
            'Dim stripeAcct As StripeAccount = accountService.Get(campaignAcctId)
            'Dim legalEntity As StripeLegalEntity = stripeAcct.LegalEntity
            'Dim bankAccount As StripeBankAccount = stripeAcct.ExternalAccounts.Data.Item(0).BankAccount
            'Dim address As StripeAddress = stripeAcct.LegalEntity.Address
            'Dim birthDay As StripeBirthDay = stripeAcct.LegalEntity.BirthDay

            'clsMember.MembershipProperties.email = stripeAcct.Email
            'clsMember.Demographics.firstName = legalEntity.FirstName
            'clsMember.Demographics.lastName = legalEntity.LastName
            'clsMember.Demographics.dateOfBirth = New Date(birthDay.Year, birthDay.Month, birthDay.Day)
            'clsMember.Demographics.ssn = "xxx-xx-" & bankAccount.Last4
            'clsMember.BillingInfo.address01 = address.Line1
            'clsMember.BillingInfo.address02 = address.Line2
            'clsMember.BillingInfo.city = address.CityOrTown
            'clsMember.BillingInfo.stateProvidence = address.State
            'clsMember.BillingInfo.postalCode = address.PostalCode
            '=============================================================
            businessResult = New BusinessReturn

            Dim accountService = New StripeAccountService()
            Dim stripeAcct As StripeAccount = accountService.Get(campaignAcctId)
            If Not IsNothing(stripeAcct) Then
                clsMember.MembershipProperties.email = stripeAcct.Email

                If Not IsNothing(stripeAcct.LegalEntity) Then
                    Dim legalEntity As StripeLegalEntity = stripeAcct.LegalEntity

                    clsMember.Demographics.firstName = legalEntity.FirstName
                    clsMember.Demographics.lastName = legalEntity.LastName

                    If Not IsNothing(stripeAcct.LegalEntity.Address) Then
                        Dim address As StripeAddress = stripeAcct.LegalEntity.Address

                        clsMember.BillingInfo.address01 = address.Line1
                        clsMember.BillingInfo.address02 = address.Line2
                        clsMember.BillingInfo.city = address.City
                        clsMember.BillingInfo.stateProvidence = address.State
                        clsMember.BillingInfo.postalCode = address.PostalCode
                    End If

                    If Not IsNothing(stripeAcct.LegalEntity.BirthDay) Then
                        Dim birthDay As StripeBirthDay = stripeAcct.LegalEntity.BirthDay

                        clsMember.Demographics.dateOfBirth = New Date(birthDay.Year, birthDay.Month, birthDay.Day)
                    End If
                End If

                If stripeAcct.ExternalAccounts.Data.Count > 0 Then
                    Dim bankAccount As StripeBankAccount = stripeAcct.ExternalAccounts.Data.Item(0).BankAccount

                    clsMember.Demographics.ssn = "xxx-xx-" & bankAccount.Last4
                End If
            End If
            '=============================================================

            'Add error to member class if exists
            If Not businessResult.isValid Then
                clsMember.errorMsg = businessResult.ExceptionMessage
            End If

            Return clsMember

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\FinancialHandler.vb : RetrieveAcctData()")
            sb.AppendLine("campaignAcctId:" & campaignAcctId)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            clsMember = New Member With {
                .errorMsg = "Error: " & ex.ToString
            }
            Return clsMember
        End Try
    End Function
    Private Function GetIPv4Address() As String
        GetIPv4Address = String.Empty
        Dim strHostName As String = System.Net.Dns.GetHostName()
        Dim iphe As System.Net.IPHostEntry = System.Net.Dns.GetHostEntry(strHostName)

        For Each ipheal As System.Net.IPAddress In iphe.AddressList
            If ipheal.AddressFamily = System.Net.Sockets.AddressFamily.InterNetwork Then
                GetIPv4Address = ipheal.ToString()
            End If
        Next
    End Function
    Private Function GetCurrentUnixTimestamp() As Double?
        'create Timespan by subtracting the value provided from the Unix Epoch
        Dim currDate As DateTime = DateTime.Now.ToLocalTime
        Dim span As TimeSpan = (currDate - New DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))
        'return the total seconds (which is a UNIX timestamp)
        Return span.TotalSeconds
    End Function
    Private Shared Function GetIntOnly(ByVal value As String) As Integer?
        Dim returnVal As String = String.Empty
        Dim collection As MatchCollection = Regex.Matches(value, "\d+")
        For Each m As Match In collection
            returnVal += m.ToString()
        Next
        Return Convert.ToInt32(returnVal)
    End Function
    Private Function SubmitPledgeToStripe(ByVal currentUserId As String, ByVal CustomerId As String, ByVal creditcardId As String, ByVal activePhaseNode As String,
                                          ByVal SelectedReward As String, ByVal PledgeAmount As String, ByVal campaignId As String, ByVal stripeUserId As String) As StripeCharge
        Try
            'Instantiate variables
            Dim blCampaigns As New blCampaigns
            Dim myCharge As New StripeChargeCreateOptions()
            Dim chargeService As New StripeChargeService()
            Dim stripeCharge As StripeCharge
            Dim sb As New StringBuilder

            '
            myCharge.Amount = (PledgeAmount * 100)
            myCharge.Currency = "usd"
            myCharge.SourceTokenOrExistingSourceId = creditcardId
            myCharge.CustomerId = CustomerId

            'Transfer $$ to campaign's sub-acct in stripe
            myCharge.Destination = stripeUserId
            myCharge.DestinationAmount = blCampaigns.adjustDonationWithFee(campaignId, myCharge.Amount)

            '
            myCharge.Capture = True

            'Create the description text for the charge.
            Dim pledgeInfo As New PledgeInformation
            pledgeInfo.campaignId = campaignId
            pledgeInfo.phaseId = activePhaseNode
            pledgeInfo.rewardId = SelectedReward
            pledgeInfo.pledgeTotal = PledgeAmount
            pledgeInfo.afterFee = (myCharge.DestinationAmount / 100)
            If pledgeInfo.pledgeTotal <> pledgeInfo.afterFee Then
                pledgeInfo.applyFee = True
                pledgeInfo.feeTotal = (pledgeInfo.pledgeTotal - pledgeInfo.afterFee)
            End If

            '
            myCharge.Description = JsonConvert.SerializeObject(pledgeInfo)
            'Add campaign id as a transfer group id.
            myCharge.TransferGroup = campaignId

            'Submit pledge
            stripeCharge = chargeService.Create(myCharge)

            Return stripeCharge
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\FinancialHandler.vb : submitPledgeToStripe()")
            sb.AppendLine("currentUserId:" & currentUserId)
            sb.AppendLine("customerId:" & CustomerId)
            sb.AppendLine("creditcardId:" & creditcardId)
            sb.AppendLine("activePhaseNode:" & activePhaseNode)
            sb.AppendLine("SelectedReward:" & SelectedReward)
            sb.AppendLine("PledgeAmount:" & PledgeAmount)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

            'WriteException(ex)
            'Throw New SoapException(ex.Message, New XmlQualifiedName(SoapException.ServerFaultCode.Name), ex)
            'Throw New Exception("Error: " & ex.ToString)
            statusCode = 400
            errorMsg = "Error: " & ex.ToString
            Return Nothing
        End Try
    End Function

    Public Function GetCurrentMemberId() As Integer?
        Return linqMembers.GetCurrentMemberId()
    End Function
#End Region

End Class
