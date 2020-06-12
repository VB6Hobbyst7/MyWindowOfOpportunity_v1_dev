Imports System.Net.Mail
Imports umbraco
Imports Common
Imports Newtonsoft.Json
Imports umbraco.Core.Models
Imports ASPSnippets.FaceBookAPI
Imports System.Web.Script.Serialization
Imports Newtonsoft.Json.Linq
Imports System.Net.Http
Imports System.Threading
Imports umbraco.Web

Public Class blMembers

#Region "Properties"
    Private linqMembers As linqMembers = New linqMembers
#End Region

#Region "Selects"
    Public Function getCurrentLoginStatus() As Web.Models.LoginStatusModel
        Return linqMembers.getCurrentLoginStatus()
    End Function
    Public Function GetCurrentMemberId() As Integer?
        Return linqMembers.GetCurrentMemberId()
    End Function
    Public Function GetCurrentMemberName() As String
        Return linqMembers.GetCurrentMemberName()
    End Function
    Public Function GetCurrentMembersFirstName() As String
        Return linqMembers.GetCurrentMembersFirstName()
    End Function
    Public Function GetCurrentMemberRole_byCampaignId(ByVal _campaignId As Integer) As String
        Try
            If isMemberLoggedIn() Then
                Return linqMembers.GetCurrentMemberRole(_campaignId)
            Else
                Return String.Empty
            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\BusinessLayer\blMembers.vb : GetCurrentMemberRole_byCampaignId()")
            sb.AppendLine("_campaignId:" & _campaignId)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return Nothing
        End Try
    End Function
    Public Function GetCampaignMembersRole_byMemberId(ByVal _memberId As Integer, ByVal _campaignId As Integer) As String
        Return linqMembers.GetCampaignMembersRole_byMemberId(_memberId, _campaignId)
    End Function
    Public Function getMemberId_byEmail(ByVal _email As String) As Integer?
        Return linqMembers.getMemberId_byEmail(_email)
    End Function
    Public Function getMemberName_byId(ByVal _id As Integer) As String
        Return linqMembers.getMemberName_byId(_id)
    End Function
    Public Function getMemberName_byGuid(ByVal _id As Guid) As String
        Return linqMembers.getMemberName_byGuid(_id)
    End Function
    Public Function getMemberEmail_byId(ByVal _id As Integer) As String
        Return linqMembers.getMemberEmail_byId(_id)
    End Function
    Public Function getCurrentUsersAltEmail() As String
        Return linqMembers.getCurrentUsersAltEmail()
    End Function
    Public Function getUsersAltEmail_byId(ByVal _id As Integer) As String
        Return linqMembers.getUsersAltEmail_byId(_id)
    End Function
    Public Function getMemberDemographics_byId(ByVal _id As Integer, Optional ByVal _getDemographics As Boolean = False, Optional ByVal _getBillingInfo As Boolean = False,
                                               Optional ByVal _getShippingInfo As Boolean = False, Optional ByVal _getMemberProperties As Boolean = False,
                                               Optional ByVal _getPledgeProperties As Boolean = False, Optional ByVal _getStripeIDs As Boolean = False) As BusinessReturn

        Return linqMembers.getMemberDemographics_byId(_id, _getDemographics, _getBillingInfo, _getShippingInfo, _getMemberProperties, _getPledgeProperties, _getStripeIDs)
    End Function
    Public Function getMemberPhoto_byId(ByVal _memberId As Integer, Optional ByVal _giveDefault As Boolean = False) As String
        Return linqMembers.getMemberPhoto_byId(_memberId, _giveDefault)
    End Function
    Public Function getMemberPhotoNodeId_byId(ByVal _memberId As Integer) As Integer?
        Return linqMembers.getMemberPhotoNodeId_byId(_memberId)
    End Function
#End Region

#Region "Insert"
    Public Function Insert(ByVal _valueDictionary As Dictionary(Of String, String)) As BusinessReturn
        Return linqMembers.Insert(_valueDictionary)
    End Function
    Public Function Insert_byPreAcct(ByVal _preAccountData As PreAccountmembers) As BusinessReturn
        Return linqMembers.Insert_byPreAcct(_preAccountData) ', isTwitter
    End Function
    Public Function InsertStripeIDs(ByVal _member As Member) As BusinessReturn
        Return linqMembers.InsertStripeIDs(_member)
    End Function
    Public Function CreateCampaignMembersFolder(ByVal parentNodeId As Int32) As Int32?
        Return linqMembers.CreateCampaignMembersFolder(parentNodeId)
    End Function
#End Region

#Region "Updates"
    Public Function Update(ByVal _member As Member) As BusinessReturn
        'Instantiate variables
        Dim businessReturn As New BusinessReturn

        Try
            'Validate
            businessReturn = Validate_MembershipProperties(_member)

            If businessReturn.isValid Then
                'Send to save
                Return linqMembers.Update(_member)
            Else
                Return businessReturn
            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\BusinessLayer\blMembers.vb : Update()")
            sb.AppendLine("_member:" & _member.ToString())

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            businessReturn.ExceptionMessage = ex.ToString
            Return businessReturn
        End Try
    End Function
    Public Function MoveStripeDateToNotes(ByVal _memberId As String) As BusinessReturn
        Return linqMembers.MoveStripeDataToNotes(_memberId)
    End Function
    Public Function InsertStripeCustomerId(ByVal _memberId As String, ByVal newCustomerId As String) As BusinessReturn
        Return linqMembers.InsertStripeCustomerID(_memberId, newCustomerId)
    End Function
    Public Function updatePhoto(ByVal _memberId As Integer, ByVal _imediaId As Integer) As BusinessReturn
        Return linqMembers.updatePhoto(_memberId, _imediaId)
    End Function
    Public Function UpdateAtCheckout(ByVal _member As Member) As BusinessReturn
        'Instantiate variables
        Dim businessReturn As New BusinessReturn

        Try
            If Not IsNothing(_member) Then
                Return linqMembers.UpdateAtCheckout(_member)
            Else
                businessReturn.ExceptionMessage = "Member = Nothing"
                Return businessReturn
            End If

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\BusinessLayer\blMembers.vb : UpdateAtCheckout()")
            sb.AppendLine("Member: " & JsonConvert.SerializeObject(_member))
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

            businessReturn.ExceptionMessage = ex.ToString
            Return businessReturn
        End Try
    End Function
    Public Function UpdatePledges(ByVal _memberId As Integer, ByVal _pledgeId As Integer) As BusinessReturn
        Return linqMembers.UpdatePledges(_memberId, _pledgeId)
    End Function
    Public Function UpdateReviews(ByVal _memberId As Integer, ByVal _reviewId As Integer) As BusinessReturn
        Return linqMembers.UpdateReviews(_memberId, _reviewId)
    End Function
    Public Function UpdateAltEmail(ByVal currentMemberId As Integer, ByVal altEmail As String) As BusinessReturn
        Return linqMembers.UpdateAltEmail(currentMemberId, altEmail)
    End Function
#End Region

#Region "Deletes"
    Public Function DeleteCreditcard(ByVal _userId As Integer) As BusinessReturn
        Return linqMembers.DeleteCreditcard(_userId)
    End Function
#End Region

#Region "Log Member In/Out"
    Public Function isMemberLoggedIn() As Boolean?
        Return linqMembers.isMemberLoggedIn()
    End Function
    Public Function logMemberIn(ByVal _userName As String, ByVal _password As String) As Boolean?
        Try
            Dim isLoggedIn As Boolean = linqMembers.logMemberIn(_userName, _password)

            If isLoggedIn Then
                'Ensure a stripe customer id exists.  if not, create it.
                linqMembers.EnsureStripeCustomerIdExist_byLoginName(_userName)
            End If

            Return isLoggedIn
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\BusinessLayer\blMembers.vb : logMemberIn()")
            sb.AppendLine("_userName:" & _userName)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
    End Function
    Public Function externalMemberlogIn(ByVal _userName As String) As Boolean?
        Return linqMembers.externallogMemberIn(_userName)
    End Function
    Public Sub logMemberOut()
        linqMembers.logMemberOut()
    End Sub
    Public Function getMembersLoginType(ByVal email As String) As mediaType_Values
        Try
            'Get what type of login the member is using.
            Return linqMembers.getMembersLoginType(email)

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\blMembers.vb : getMembersLoginType()")
            sb.AppendLine("email:" & email)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
    End Function


    Public Sub logMemberIntoFacebook()
        'https://www.aspsnippets.com/Articles/ASPNet-Get-FaceBook-User-details-like-Gender-Email-Address-Location-and-Birth-Date.aspx

        Try
            'Facebook login
            FaceBookConnect.API_Key = System.Configuration.ConfigurationManager.AppSettings("FBAPI_Key").ToString()
            FaceBookConnect.API_Secret = System.Configuration.ConfigurationManager.AppSettings("FBAPI_Secret").ToString()

            'Send user to facebook to log in.  User will then return with data.
            FaceBookConnect.Authorize("public_profile,email", ConfigurationManager.AppSettings("LoginRedirect"))
            HttpContext.Current.ApplicationInstance.CompleteRequest()
        Catch taEx As ThreadAbortException
            'Do nothing.  This is because the facebookconnect.authorize throws this error from aborting thread.
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("blMembers.vb : logMemberIntoFacebook()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
    End Sub
    Public Function logMemberIn_byFacebook(ByVal code As String) As BusinessReturn
        Dim businessReturn As New BusinessReturn

        Try
            'Instantiate variables
            Dim Data As String = String.Empty
            Dim faceBookUser As FaceBookUser
            FaceBookConnect.API_Key = System.Configuration.ConfigurationManager.AppSettings("FBAPI_Key").ToString()
            FaceBookConnect.API_Secret = System.Configuration.ConfigurationManager.AppSettings("FBAPI_Secret").ToString()


            'Extract data
            Data = FaceBookConnect.Fetch(code, "me?fields=id,email,first_name,last_name")
            If String.IsNullOrEmpty(Data) Then
                businessReturn.ExceptionMessage = "Facebook failed to return user data."
                Return businessReturn
            Else
                faceBookUser = New JavaScriptSerializer().Deserialize(Of FaceBookUser)(Data)
                If IsNothing(faceBookUser) Then
                    businessReturn.ExceptionMessage = "Facebook failed to return valid data."
                    Return businessReturn
                Else
                    'if email does not exist, create a facebook email.
                    If String.IsNullOrEmpty(faceBookUser.Email.Trim) Then
                        faceBookUser.Email = faceBookUser.Id & "@facebook.com"
                    End If

                    'If member does not exist in umbraco, create it.
                    If Not doesMemberExist_byEmail(faceBookUser.Email) Then
                        '
                        Dim memberDictionary As New Dictionary(Of String, String)
                        '
                        memberDictionary.Add(queryParameters.firstName, faceBookUser.First_Name)
                        memberDictionary.Add(queryParameters.lastName, faceBookUser.Last_name)
                        memberDictionary.Add(queryParameters.email, faceBookUser.Email)
                        memberDictionary.Add(nodeProperties.isFacebookAcct, True.ToString)
                        memberDictionary.Add(queryParameters.password, ConfigurationManager.AppSettings("FB_FakePw"))
                        '
                        Dim bReturn As New BusinessReturn
                        bReturn = Insert(memberDictionary)
                        If bReturn.isValid Then
                            '
                            If logMemberIn(faceBookUser.Email, ConfigurationManager.AppSettings("FB_FakePw")) Then
                                businessReturn.ReturnMessage = faceBookUser.First_Name
                                Return businessReturn
                            Else
                                businessReturn.ExceptionMessage = "Unable to log member in."
                                Return businessReturn
                            End If
                        Else
                            bReturn.ReturnMessage = "Failed to create membership record.  Could not log member in."
                            Return bReturn
                        End If
                    Else
                        '
                        If logMemberIn(faceBookUser.Email, ConfigurationManager.AppSettings("FB_FakePw")) Then
                            businessReturn.ReturnMessage = faceBookUser.First_Name
                            Return businessReturn
                        Else
                            businessReturn.ExceptionMessage = "Unable to log member in."
                            Return businessReturn
                        End If
                    End If
                End If
            End If

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("blMembers.vb : logMemberIn_byFacebook()")
            sb.AppendLine("code:" & code)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

            businessReturn.ExceptionMessage = ex.ToString
            Return businessReturn
        End Try

        ''NOTES
        'https://github.com/mkdynamic/omniauth-facebook/issues/61
        'https://developers.facebook.com/docs/facebook-login/permissions
    End Function
    Public Sub logMemberIntoTwitter()
        Try
            'Instantiate variables
            Dim oAuth = New oAuthTwitter()

            'Redirect the user to Twitter for authorization.
            oAuth.CallBackUrl = ConfigurationManager.AppSettings("LoginRedirect")
            HttpContext.Current.Response.Redirect(oAuth.AuthorizationLinkGet(), False)
            HttpContext.Current.ApplicationInstance.CompleteRequest()
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("blMembers.vb : logMemberIntoTwitter()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
    End Sub
    Public Function logMemberIn_byTwitter(ByVal token As String, ByVal verifier As String) As BusinessReturn
        Dim businessReturn As New BusinessReturn
        'Dim sb2 As New StringBuilder()

        Try
            'Instantiate variables
            Dim twitterApiUrl As String = "https://api.twitter.com/1.1/account/verify_credentials.json?include_email=true"
            Dim oAuth = New oAuthTwitter()
            Dim xml As String = String.Empty
            Dim jObj_Xml As JObject
            Dim lstData As List(Of JToken)
            Dim twitteruser As String = String.Empty
            Dim twitterfullname As String = String.Empty
            Dim twitterEmail As String = String.Empty
            Dim firstName As String = String.Empty
            Dim lastName As String = String.Empty
            Dim id As String = String.Empty
            'sb2.AppendLine("Twitter data")

            'Exchange the request token for an access token
            oAuth.AccessTokenGet(token, verifier)
            If String.IsNullOrEmpty(oAuth.TokenSecret) Then
                businessReturn.ExceptionMessage = "Twitter failed to return user data."
                Return businessReturn
            Else
                'Request authorization from twitter
                xml = oAuth.oAuthWebRequest(oAuthTwitter.Method.[GET], twitterApiUrl, [String].Empty)
                businessReturn.ReturnMessage = xml

                'Parse request
                jObj_Xml = JObject.Parse(xml)
                lstData = jObj_Xml.Children().ToList
                'sb2.AppendLine("---")
                'sb2.AppendLine(xml)
                'sb2.AppendLine("---")

                'Obtain data from properties
                For Each item As JProperty In lstData
                    item.CreateReader()
                    Select Case item.Name
                        Case "name"
                            twitterfullname = item.Value
                            If Not IsNothing(twitterfullname) Then
                                Dim namePairs As KeyValuePair(Of String, String) = splitName(twitterfullname).FirstOrDefault
                                firstName = namePairs.Key
                                lastName = namePairs.Value
                            End If
                        Case "screen_name"
                            twitteruser = item.Value
                        Case "email"
                            twitterEmail = item.Value
                        Case "id"
                            id = item.Value
                    End Select
                Next

                'if email does not exist, create a twitter email.
                If String.IsNullOrEmpty(twitterEmail) Then
                    twitterEmail = id & "@twitter.com"
                End If

                'If member does not exist in umbraco, create it.
                If Not doesMemberExist_byEmail(twitterEmail) Then
                    'Instantiate member dictionary
                    Dim memberDictionary As New Dictionary(Of String, String)
                    'Add data to dictionary
                    memberDictionary.Add(queryParameters.firstName, firstName)
                    memberDictionary.Add(queryParameters.lastName, lastName)
                    memberDictionary.Add(queryParameters.email, twitterEmail)
                    memberDictionary.Add(nodeProperties.isTwitterAcct, True.ToString)
                    memberDictionary.Add(queryParameters.password, ConfigurationManager.AppSettings("Tw_FakePw"))
                    'Create membership
                    Dim bReturn As New BusinessReturn
                    bReturn = Insert(memberDictionary)
                    If bReturn.isValid Then
                        'Attempt to log member in
                        If logMemberIn(twitterEmail, ConfigurationManager.AppSettings("Tw_FakePw")) Then
                            businessReturn.ReturnMessage = firstName
                            'sb2.AppendLine("successful: " & businessReturn.ReturnMessage)
                            'sb2.AppendLine("twitterEmail: " & twitterEmail)
                            'sb2.AppendLine("Tw_FakePw: " & ConfigurationManager.AppSettings("Tw_FakePw"))
                            'saveErrorMessage(getLoggedInMember, sb2.ToString, sb2.ToString())
                            Return businessReturn
                        Else
                            businessReturn.ExceptionMessage = "Unable to log member in."
                            'sb2.AppendLine("ExceptionMessage: " & businessReturn.ExceptionMessage)
                            'saveErrorMessage(getLoggedInMember, sb2.ToString, sb2.ToString())
                            Return businessReturn
                        End If
                    Else
                        bReturn.ReturnMessage = "Failed to create membership record.  Could not log member in."
                        'sb2.AppendLine("Error: " & businessReturn.ReturnMessage)
                        'saveErrorMessage(getLoggedInMember, sb2.ToString, sb2.ToString())
                        Return bReturn
                    End If
                Else
                    'Attempt to log member in
                    If logMemberIn(twitterEmail, ConfigurationManager.AppSettings("Tw_FakePw")) Then
                        businessReturn.ReturnMessage = firstName
                        'sb2.AppendLine("logged in: " & firstName)
                        'sb2.AppendLine("twitterEmail: " & twitterEmail)
                        'sb2.AppendLine("Tw_FakePw: " & ConfigurationManager.AppSettings("Tw_FakePw"))
                        'saveErrorMessage(getLoggedInMember, sb2.ToString, sb2.ToString())
                        Return businessReturn
                    Else
                        businessReturn.ExceptionMessage = "Unable to log member in."
                        'sb2.AppendLine("ExceptionMessage: " & businessReturn.ExceptionMessage)
                        'saveErrorMessage(getLoggedInMember, sb2.ToString, sb2.ToString())
                        Return businessReturn
                    End If
                End If

                'sb2.AppendLine("oauth_token: " & token)
                'sb2.AppendLine("oauth_verifier: " & verifier)
                'sb2.AppendLine(xml)
                'sb2.AppendLine("twitterfullname: " & twitterfullname)
                'sb2.AppendLine("twitteruser: " & twitteruser)
                'sb2.AppendLine("twitterEmail: " & twitterEmail)
                'If Not IsNothing(twitterfullname) Then
                '    Dim namePairs As KeyValuePair(Of String, String) = splitName(twitterfullname).FirstOrDefault
                '    sb2.AppendLine("first name: " & namePairs.Key)
                '    sb2.AppendLine("last name: " & namePairs.Value)
                'End If
                'saveErrorMessage(getLoggedInMember, sb2.ToString, sb2.ToString())

            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("blMembers.vb : logMemberIn_byTwitter()")
            sb.AppendLine("oauth_token: " & token)
            sb.AppendLine("oauth_verifier: " & verifier)
            sb.AppendLine("xml: " & businessReturn.ReturnMessage)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

            businessReturn.ExceptionMessage = ex.ToString
            Return businessReturn
        End Try
    End Function
    Public Sub logMemberIntoLinkedIn()
        Try
            ''Instantiate variables
            Dim linkedInUrl As String = "https://www.linkedin.com/oauth/v2/authorization?response_type=code&client_id=" + ConfigurationManager.AppSettings("LinkedInClientID") +
            "&redirect_uri=" + ConfigurationManager.AppSettings("LinkedInRedirectUrl") +
            "&state=" + LinkedInUtil.LinkedInState + "&scope=r_basicprofile+r_emailaddress"

            HttpContext.Current.Response.Redirect(linkedInUrl, False)
            HttpContext.Current.ApplicationInstance.CompleteRequest()
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("blMembers.vb : logMemberIntoLinkedIn()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
    End Sub
    Public Function logMemberIn_byLinkedIn(ByVal code As String) As BusinessReturn
        Dim businessReturn As New BusinessReturn

        Try
            'Instantiate variables
            Dim jsonSerializer As JavaScriptSerializer = New JavaScriptSerializer()
            Dim linkedINVM As LinkedInToken
            Dim linkedInUser As LinkedInUser
            Dim linkedInEmail As String
            Dim content As String

            'Obtain linkedIn data
            Dim linkedInUrl As String = "https://www.linkedin.com/oauth/v2/accessToken?grant_type=authorization_code&code=" + code + "&redirect_uri=" + ConfigurationManager.AppSettings("LinkedInRedirectUrl") + "&client_id=" + ConfigurationManager.AppSettings("LinkedInClientID") + "&client_secret=" + ConfigurationManager.AppSettings("LinkedInClientSecret")
            content = RestCall(HttpMethod.Post, linkedInUrl)
            linkedINVM = jsonSerializer.Deserialize(Of LinkedInToken)(content)

            linkedInUrl = "https://api.linkedin.com/v1/people/~?oauth2_access_token=" + linkedINVM.access_token + "&format=json"
            content = RestCall(HttpMethod.Get, linkedInUrl)
            linkedInUser = jsonSerializer.Deserialize(Of LinkedInUser)(content)

            linkedInUrl = "https://api.linkedin.com/v1/people/~/email-address?oauth2_access_token=" + linkedINVM.access_token + "&format=json"
            content = RestCall(HttpMethod.Get, linkedInUrl)
            linkedInEmail = Replace(content, """", "")


            'if email does not exist, create a linked in email.
            If String.IsNullOrEmpty(linkedInEmail) Then
                linkedInEmail = linkedInUser.id & "@linkedin.com"
            End If

            'If member does not exist in umbraco, create it.
            If Not doesMemberExist_byEmail(linkedInEmail) Then
                'Instantiate member dictionary
                Dim memberDictionary As New Dictionary(Of String, String)
                'Add data to dictionary
                memberDictionary.Add(queryParameters.firstName, linkedInUser.firstName)
                memberDictionary.Add(queryParameters.lastName, linkedInUser.lastName)
                memberDictionary.Add(queryParameters.email, linkedInEmail)
                memberDictionary.Add(nodeProperties.isLinkedInAcct, True.ToString)
                memberDictionary.Add(queryParameters.password, ConfigurationManager.AppSettings("LI_FakePw"))
                'Create membership
                Dim bReturn As New BusinessReturn
                bReturn = Insert(memberDictionary)
                If bReturn.isValid Then
                    'Attempt to log member in
                    If logMemberIn(linkedInEmail, ConfigurationManager.AppSettings("LI_FakePw")) Then
                        businessReturn.ReturnMessage = linkedInUser.firstName
                        Return businessReturn
                    Else
                        businessReturn.ExceptionMessage = "Unable to log member in."
                        Return businessReturn
                    End If
                Else
                    bReturn.ReturnMessage = "Failed to create membership record.  Could not log member in."
                    Return bReturn
                End If
            Else
                'Attempt to log member in
                If logMemberIn(linkedInEmail, ConfigurationManager.AppSettings("LI_FakePw")) Then
                    businessReturn.ReturnMessage = linkedInUser.firstName
                    Return businessReturn
                Else
                    businessReturn.ExceptionMessage = "Unable to log member in."
                    Return businessReturn
                End If
            End If

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("blMembers.vb : logMemberIn_byLinkedIn()")
            sb.AppendLine("code: " & code)
            sb.AppendLine("xml: " & businessReturn.ReturnMessage)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

            businessReturn.ExceptionMessage = ex.ToString
            Return businessReturn
        End Try
    End Function
#End Region

#Region "Methods"
    Public Function IsUserUnique(ByVal _email As String, ByVal _password As String) As BusinessReturn
        Return Validate(_email, _password)
    End Function
    Public Function doesMemberExist_byUserId(ByVal _userId As String) As Boolean?
        Return linqMembers.doesMemberExist_byUserId(_userId)
    End Function
    Public Function doesMemberExist_byEmail(ByVal _email As String) As Boolean?
        Return linqMembers.DoesMemberExist_byEmail(_email)
    End Function
    Public Function getCampaignMemberFolder(ByVal _campaignNode As IPublishedContent) As IPublishedContent
        Return linqMembers.getCampaignMemberFolder(_campaignNode)
    End Function
    Private Function splitName(ByVal name As String) As Dictionary(Of String, String)
        Dim dict As New Dictionary(Of String, String)

        Try
            'Instantiate variables
            Dim index As Integer?

            'Obtain location to split string
            If name.Contains(",") Then
                index = name.IndexOf(",")
            ElseIf name.Contains(" ") Then
                index = name.IndexOf(" ")
            End If

            'Split name if possible
            If IsNothing(index) Then
                dict.Add(name, String.Empty)
            Else
                dict.Add(name.Substring(0, index), name.Substring(index, (name.Length - index)))
            End If

            Return dict

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("blMembers.vb : splitName()")
            sb.AppendLine("name: " & name)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

            dict.Add(name, String.Empty)
            Return dict
        End Try
    End Function
    Public Function RestCall(ByVal httpMethod As HttpMethod, ByVal url As String) As String
        Using request As HttpRequestMessage = New HttpRequestMessage(httpMethod, url)
            Using client As HttpClient = New HttpClient()
                Using response As HttpResponseMessage = client.SendAsync(request).Result
                    Using content As HttpContent = response.Content
                        Return content.ReadAsStringAsync().Result
                    End Using
                End Using
            End Using
        End Using
        Return String.Empty
    End Function
    Public Function isUserLoggedIn_bySocialMedia() As Boolean
        Try
            'Obtain current member
            Dim currentMember As IPublishedContent = linqMembers.GetCurrentMember

            'Determine if user is logged in using social media
            Select Case True
                Case currentMember.GetPropertyValue(Of Boolean)(nodeProperties.isFacebookAcct)
                    Return True
                Case currentMember.GetPropertyValue(Of Boolean)(nodeProperties.isLinkedInAcct)
                    Return True
                Case currentMember.GetPropertyValue(Of Boolean)(nodeProperties.isTwitterAcct)
                    Return True
                Case Else
                    Return False
            End Select

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("blMembers.vb : isUserLoggedIn_bySocialMedia()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return False
        End Try
    End Function
#End Region

#Region "Validations"
    Private Function Validate(ByVal _email As String, ByVal _password As String) As BusinessReturn
        Dim returnObject As New BusinessReturn()
        Try
            'Validate Email
            If (linqMembers.DoesMemberExist_byEmail(_email)) Then
                returnObject.ValidationMessages.Add(New ValidationContainer("—This email address already exists in the system.  Login with these credentials or provide a unique email address."))
            End If

            'Validate Password: Ensure password has combination of letters/numbers and is 8+ characters long
            If Not System.Text.RegularExpressions.Regex.IsMatch(_password, "^[a-zA-Z0-9]{6,20}$") Then
                returnObject.ValidationMessages.Add(New ValidationContainer("—Invalid Password.  Please ensure your password is alphanumeric only and is between 6-20 characters."))
            End If

            'Return all validation messages
            Return returnObject
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\BusinessLayer\blMembers.vb : Validate()")
            sb.AppendLine("_email:" & _email)
            sb.AppendLine("_password:" & _password)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

            returnObject.ExceptionMessage = ex.ToString
            Return returnObject
        End Try
    End Function
    Private Function Validate_MembershipProperties(ByVal _member As Member) As BusinessReturn
        Dim returnObject As New BusinessReturn()
        Try
            'Validate user id/ email
            Try
                'check email for proper format
                Dim mailAddressLoginName As MailAddress = New MailAddress(_member.MembershipProperties.loginName)
                Dim mailAddressEmail As MailAddress = New MailAddress(_member.MembershipProperties.email)

                'throw error if email exists in system and belongs to another user id.
                If (linqMembers.DoesMemberExist_byEmail(_member.MembershipProperties.loginName)) Then
                    If getMemberId_byEmail(_member.MembershipProperties.loginName) <> _member.MembershipProperties.userId Then
                        returnObject.ValidationMessages.Add(New ValidationContainer("This email is already in use. Please select a unique email."))
                    End If
                End If
            Catch ex As Exception
                Dim sb As New StringBuilder()
                sb.AppendLine("\App_Code\BusinessLayer\blMembers.vb : Validate_MembershipProperties()")
                sb.AppendLine("_member:" & _member.ToString())

                saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
                returnObject.ValidationMessages.Add(New ValidationContainer("Please enter a valid email"))
                returnObject.ValidationMessages.Add(New ValidationContainer("<br />Error: " & ex.ToString & "<br />UserId: " & _member.MembershipProperties.userId))
            End Try

            'Validate alt email if exists
            If Not String.IsNullOrEmpty(_member.MembershipProperties.altEmail) Then
                If Not ValidEmail(_member.MembershipProperties.altEmail) Then
                    returnObject.ValidationMessages.Add(New ValidationContainer("Please provide a valid alternative email address or leave the field blank."))
                End If
            End If


            'Return all validation messages
            Return returnObject
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\BusinessLayer\blMembers.vb : Validate_MembershipProperties()")
            sb.AppendLine("_member:" & _member.ToString())

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            returnObject.ExceptionMessage = ex.ToString
            Return returnObject
        End Try
    End Function
#End Region

End Class