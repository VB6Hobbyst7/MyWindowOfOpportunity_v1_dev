Imports umbraco
Imports umbraco.Core
Imports umbraco.Core.Models
Imports umbraco.Core.Services
Imports Common
Imports System.Web.HttpContext
Imports Stripe
Imports Newtonsoft.Json
Imports umbraco.Web


Public Class linqMembers

#Region "Properties"
    Private linq2Db As linq2SqlDataContext = New linq2SqlDataContext(ConfigurationManager.ConnectionStrings("umbracoDbDSN").ToString)
    'Umbraco Membership Helper classes | https://our.umbraco.org/documentation/Reference/Querying/MemberShipHelper/
    Dim memberShipHelper As umbraco.Web.Security.MembershipHelper = New umbraco.Web.Security.MembershipHelper(umbraco.Web.UmbracoContext.Current)
    Dim _uHelper As Uhelper = New Uhelper()
#End Region

#Region "Selects"
    Public Function getCurrentLoginStatus() As Web.Models.LoginStatusModel
        Return memberShipHelper.GetCurrentLoginStatus()
    End Function
    Public Function GetCurrentMember() As IPublishedContent
        Return memberShipHelper.GetCurrentMember
    End Function
    Public Function GetCurrentMemberId() As Integer?
        Return memberShipHelper.GetCurrentMemberId()
    End Function
    Public Function GetCurrentMemberName() As String
        Return memberShipHelper.GetCurrentMember.Name
    End Function
    Public Function GetCurrentMembersFirstName() As String
        Try
            'Dim member As IMember = ApplicationContext.Current.Services.MemberService.GetById(memberShipHelper.GetCurrentMember.Id)
            Dim member As IMember = ApplicationContext.Current.Services.MemberService.GetById(GetCurrentMemberId())
            If Not IsNothing(member) Then
                Return member.GetValue(Of String)(nodeProperties.firstName)
            Else
                Return String.Empty
            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\linqQueries\linqMembers.vb : GetCurrentMembersFirstName()")
            ' sb.AppendLine("mediaId:" & mediaId)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Return "Error: " & ex.ToString  'String.Empty
            Return String.Empty
        End Try
    End Function
    Public Function GetCurrentMemberRole(ByVal _campaignId As Integer) As String
        Try
            'Instantiate variables
            Dim campaignNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(_campaignId)
            If Not IsNothing(campaignNode) Then
                Dim campaignMemberFolder As IPublishedContent
                Dim currentMemberId As Integer = GetCurrentMemberId()
                Dim lstTeamAdministrators As New List(Of String)

                If campaignNode.Parent.HasValue(nodeProperties.teamAdministrators) Then
                    For Each id As String In campaignNode.Parent.GetPropertyValue(Of String)(nodeProperties.teamAdministrators).Split(",")
                        lstTeamAdministrators.Add(id)
                    Next

                    'Determine if member is a team admin
                    If lstTeamAdministrators.Contains(currentMemberId.ToString) Then
                        Return memberRole.TeamAdministrator
                    Else
                        'Obtain campaign's member folder
                        campaignMemberFolder = getCampaignMemberFolder(campaignNode)
                        If String.IsNullOrEmpty(campaignMemberFolder.Name) Then
                            'No campaign member folder exists.
                            Return String.Empty
                        Else
                            'Loop thru member nodes
                            For Each childNode As IPublishedContent In campaignMemberFolder.Children
                                'Determine if childnode = current member
                                If childNode.HasProperty(nodeProperties.campaignMember) Then
                                    If childNode.GetPropertyValue(Of String)(nodeProperties.campaignMember) = currentMemberId Then
                                        'Return wether user is a member or admin
                                        If childNode.GetPropertyValue(Of Boolean)(nodeProperties.campaignManager) = True Then
                                            Return memberRole.CampaignAdministrator
                                        Else
                                            Return memberRole.CampaignMember
                                        End If
                                    End If
                                End If
                            Next

                            'If no member IPublishedContent matches, return empty string
                            Return String.Empty
                        End If
                    End If

                Else
                    Return String.Empty
                End If




            Else
                Return String.Empty
            End If

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqMembers.vb : GetCurrentMemberRole()")
            sb.AppendLine("_campaignId:" & _campaignId)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return String.Empty
            'Return "Error Linq: " & ex.ToString
        End Try
    End Function
    Public Function GetCampaignMembersRole_byMemberId(ByVal _memberId As Integer, ByVal _campaignId As Integer) As String
        Try
            'Instantiate variables
            Dim campaignNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(_campaignId)
            Dim campaignMemberFolder As IPublishedContent

            Dim lstTeamAdministrators As New List(Of String)
            For Each id As String In campaignNode.Parent.GetPropertyValue(Of String)(nodeProperties.teamAdministrators).Split(",")
                lstTeamAdministrators.Add(id)
            Next


            'Determine if member is a team admin
            If lstTeamAdministrators.Contains(_memberId.ToString) Then
                Return memberRole.TeamAdministrator
            Else
                'Obtain campaign's member folder
                campaignMemberFolder = getCampaignMemberFolder(campaignNode)
                If String.IsNullOrEmpty(campaignMemberFolder.Name) Then
                    'No campaign member folder exists.
                    Return String.Empty
                Else
                    'Loop thru member nodes
                    For Each childNode As IPublishedContent In campaignMemberFolder.Children
                        'Determine if childnode = current member
                        If childNode.HasProperty(nodeProperties.campaignMember) Then
                            If childNode.GetPropertyValue(Of String)(nodeProperties.campaignMember) = _memberId Then
                                'Return wether user is a member or admin
                                If childNode.GetPropertyValue(Of Boolean)(nodeProperties.campaignManager) = True Then
                                    Return memberRole.CampaignAdministrator
                                Else
                                    Return memberRole.CampaignMember
                                End If
                            End If
                        End If
                    Next

                    'If no member IPublishedContent matches, return empty string
                    Return String.Empty
                End If
            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\linqQueries\linqMembers.vb : GetCampaignMembersRole_byMemberId()")
            sb.AppendLine("_memberId:" & _memberId)
            sb.AppendLine("_campaignId:" & _campaignId)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Return String.Empty
            Return "Error Linq: " & ex.ToString
        End Try
    End Function
    Public Function getMemberId_byEmail(ByVal _email As String) As Integer?
        'Return id
        Try
            Dim member As IMember = ApplicationContext.Current.Services.MemberService.GetByEmail(_email)
            Return member.Id
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\linqQueries\linqMembers.vb : getMemberId_byEmail()")
            sb.AppendLine("_email:" & _email)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return -1
        End Try
    End Function
    Public Function getMemberName_byId(ByVal _id As Integer) As String
        'Return id
        Try
            Dim member As IMember = ApplicationContext.Current.Services.MemberService.GetById(_id)
            If Not IsNothing(member) Then
                Return member.Name
            Else
                Return String.Empty
            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\linqQueries\linqMembers.vb : getMemberName_byId()")
            sb.AppendLine("_id:" & _id)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return String.Empty
        End Try
    End Function
    Public Function getMemberName_byGuid(ByVal _id As Guid) As String
        'Return id
        Try
            Dim member As IMember = ApplicationContext.Current.Services.MemberService.GetByKey(_id)
            If Not IsNothing(member) Then
                Return member.Name
            Else
                Return String.Empty
            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\linqQueries\linqMembers.vb : getMemberName_byGuid()")
            sb.AppendLine("_id:" & _id.ToString)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return String.Empty
        End Try
    End Function
    Public Function getMemberEmail_byId(ByVal _id As Integer) As String
        'Return id
        Try
            Dim member As IMember = ApplicationContext.Current.Services.MemberService.GetById(_id)
            Return member.Email
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\linqQueries\linqMembers.vb : getMemberEmail_byId()")
            sb.AppendLine("_id:" & _id)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return String.Empty
        End Try
    End Function
    Public Function getCurrentUsersAltEmail() As String
        'Return id
        Try
            Dim member As IMember = ApplicationContext.Current.Services.MemberService.GetByEmail(memberShipHelper.CurrentUserName)
            Return member.GetValue(Of String)(nodeProperties.alternativeEmail)
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqMembers.vb : getAltEmail_byId()")

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return String.Empty
        End Try
    End Function
    Public Function getUsersAltEmail_byId(ByVal _id As Integer) As String
        'Return id
        Try
            Dim member As IMember = ApplicationContext.Current.Services.MemberService.GetById(_id)
            Return member.GetValue(Of String)(nodeProperties.alternativeEmail)
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqMembers.vb : getAltEmail_byId()")

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return String.Empty
        End Try
    End Function
    Public Function getMemberDemographics_byId(ByVal _id As Integer, Optional ByVal _getDemographics As Boolean = False, Optional ByVal _getBillingInfo As Boolean = False,
                                               Optional ByVal _getShippingInfo As Boolean = False, Optional ByVal _getMemberProperties As Boolean = False,
                                               Optional _getPledgeProperties As Boolean = False, Optional ByVal _getStripeIDs As Boolean = False) As BusinessReturn

        '
        Dim BusinessReturn As BusinessReturn = New BusinessReturn

        Try
            If _id > 0 Then
                'Instantiate variables
                Dim member As IMember = ApplicationContext.Current.Services.MemberService.GetById(_id)
                Dim clsMember As Member = New Member

                If Not IsNothing(member) Then
                    'Obtain demographics
                    If _getDemographics Then
                        If member.HasProperty(nodeProperties.firstName) Then clsMember.Demographics.firstName = member.GetValue(Of String)(nodeProperties.firstName)
                        If member.HasProperty(nodeProperties.lastName) Then clsMember.Demographics.lastName = member.GetValue(Of String)(nodeProperties.lastName)
                        If member.HasProperty(nodeProperties.photo) AndAlso Not IsNothing(member.GetValue(nodeProperties.photo)) Then
                            clsMember.Demographics.photo = member.GetValue(Of Integer)(nodeProperties.photo)
                            clsMember.Demographics.photoUrl = getMediaURL(member.GetValue(Of Integer)(nodeProperties.photo), Crops.members)
                        Else
                            clsMember.Demographics.photo = mediaNodes.defaultProfileImg
                            clsMember.Demographics.photoUrl = getMediaURL(mediaNodes.defaultProfileImg, Crops.members)
                        End If
                        clsMember.Demographics.briefDescription = member.GetValue(Of String)(nodeProperties.briefDescription)
                    End If

                    'Obtain billing info
                    If _getBillingInfo Then
                        clsMember.BillingInfo.address01 = member.GetValue(Of String)(nodeProperties.address01_Billing)
                        clsMember.BillingInfo.address02 = member.GetValue(Of String)(nodeProperties.address02_Billing)
                        clsMember.BillingInfo.city = member.GetValue(Of String)(nodeProperties.city_Billing)
                        clsMember.BillingInfo.stateProvidence = member.GetValue(Of String)(nodeProperties.stateprovidence_Billing)
                        clsMember.BillingInfo.postalCode = member.GetValue(Of String)(nodeProperties.postalCode_Billing)
                    End If

                    'Obtain shipping info
                    If _getShippingInfo Then
                        clsMember.ShippingInfo.address01 = member.GetValue(Of String)(nodeProperties.address01_Shipping)
                        clsMember.ShippingInfo.address02 = member.GetValue(Of String)(nodeProperties.address02_Shipping)
                        clsMember.ShippingInfo.city = member.GetValue(Of String)(nodeProperties.city_Shipping)
                        clsMember.ShippingInfo.stateProvidence = member.GetValue(Of String)(nodeProperties.stateprovidence_Shipping)
                        clsMember.ShippingInfo.postalCode = member.GetValue(Of String)(nodeProperties.postalCode_Shipping)
                    End If

                    'Obtain member properties
                    If _getMemberProperties Then
                        clsMember.MembershipProperties.userId = _id
                        clsMember.MembershipProperties.nodeName = member.Name
                        clsMember.MembershipProperties.loginName = member.Username
                        clsMember.MembershipProperties.email = member.Email
                        clsMember.MembershipProperties.altEmail = member.GetValue(Of String)(nodeProperties.alternativeEmail)
                        clsMember.MembershipProperties.isFacebookAcct = member.GetValue(Of Boolean)(nodeProperties.isFacebookAcct)
                        clsMember.MembershipProperties.isLinkedInAcct = member.GetValue(Of Boolean)(nodeProperties.isLinkedInAcct)
                        clsMember.MembershipProperties.isTwitterAcct = member.GetValue(Of Boolean)(nodeProperties.isTwitterAcct)
                    End If

                    'Obtain pledges
                    If _getPledgeProperties Then
                        'Obtain member's pledges as csv list
                        Dim pledges As String = member.GetValue(Of String)(nodeProperties.pledges)

                        If pledges IsNot Nothing Then
                            'Instantiate variables
                            'Dim lstPledges As New List(Of CampaignPledge)
                            Dim pledgeIdList As List(Of String)

                            'Split list of pledge IDs
                            pledgeIdList = pledges.Split(",").ToList

                            'Loop thru all IDs
                            For Each pledgeId As String In pledgeIdList
                                'Instantiate pledge IPublishedContent
                                Dim pledgeNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(pledgeId)

                                If Not IsNothing(pledgeNode) Then
                                    If pledgeNode.DocumentTypeAlias = docTypes.Pledges Then
                                        'Instantiate new class object
                                        'Add data to object
                                        Dim campaignPledge = New CampaignPledge With {
                                        .pledgeDate = pledgeNode.GetPropertyValue(Of Date)(nodeProperties.pledgeDate),
                                        .pledgeAmount = pledgeNode.GetPropertyValue(Of Decimal)(nodeProperties.pledgeAmount),
                                        .campaignName = pledgeNode.Parent.Parent.Parent.Name,
                                        .showAsAnonymous = pledgeNode.GetPropertyValue(Of Boolean)(nodeProperties.showAsAnonymous),
                                        .fulfilled = pledgeNode.GetPropertyValue(Of Boolean)(nodeProperties.fulfilled),
                                        .canceled = pledgeNode.GetPropertyValue(Of Boolean)(nodeProperties.canceled),
                                        .transactionDeclined = pledgeNode.GetPropertyValue(Of Boolean)(nodeProperties.transactionDeclined),
                                        .reimbursed = pledgeNode.GetPropertyValue(Of Boolean)(nodeProperties.reimbursed),
                                        .campaignUrl = pledgeNode.Parent.Parent.Parent.Url
                                    }

                                        'Add object to class
                                        clsMember.PledgeList.Add(campaignPledge)
                                    End If
                                End If
                            Next
                        End If
                    End If

                    'Obtain Stripe IDs
                    If _getStripeIDs Then
                        clsMember.StripeIDs.customerId = member.GetValue(Of String)(nodeProperties.customerId)
                        'clsMember.StripeIDs.bankAcctId = member.GetValue(Of String)(nodeProperties.bankAccountId)
                        'clsMember.StripeIDs.bankAcctToken = member.GetValue(Of String)(nodeProperties.bankAccountToken)
                        'clsMember.StripeIDs.campaignAcctId = member.GetValue(Of String)(nodeProperties.campaignAccountId)
                        'clsMember.StripeIDs.fileUploadId = member.GetValue(Of String)(nodeProperties.fileUploadId)
                        clsMember.StripeIDs.creditCardId = member.GetValue(Of String)(nodeProperties.creditCardId)
                        clsMember.StripeIDs.creditCardToken = member.GetValue(Of String)(nodeProperties.creditCardToken)
                    End If

                    'Return class within businessreturn
                    BusinessReturn.DataContainer.Add(clsMember)

                End If
            End If


        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\linqQueries\linqMembers.vb : getMemberDemographics_byId()")
            sb.AppendLine("_id:" & _id)
            sb.AppendLine("_getDemographics:" & _getDemographics)
            sb.AppendLine("_getBillingInfo:" & _getBillingInfo)
            sb.AppendLine("_getShippingInfo:" & _getShippingInfo)
            sb.AppendLine("_getMemberProperties:" & _getMemberProperties)
            sb.AppendLine("_getPledgeProperties:" & _getPledgeProperties)
            sb.AppendLine("_getStripeIDs:" & _getStripeIDs)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            BusinessReturn.ExceptionMessage = ex.ToString
        End Try

        Return BusinessReturn
    End Function
    Public Function getPledgeCampaign(ByVal thisNode As IPublishedContent) As String
        Try
            If thisNode.DocumentTypeAlias = docTypes.Campaign Then
                Return thisNode.Name
            Else
                Dim childNode As IPublishedContent = thisNode.Parent
                Return getPledgeCampaign(childNode)
            End If

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\linqQueries\linqMembers.vb : getPledgeCampaign()")
            sb.AppendLine("thisNode:" & thisNode.ToString())
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return ""
        End Try

    End Function
    Public Function getMemberPhoto_byId(ByVal _memberId As Integer, ByVal _giveDefault As Boolean) As String
        Try
            'Instantiate variables.
            Dim imgUrl As String = String.Empty
            Dim member As IMember = ApplicationContext.Current.Services.MemberService.GetById(_memberId)

            'Check if image exists for user.
            If (member.HasProperty(nodeProperties.photo)) AndAlso (Not String.IsNullOrWhiteSpace(member.GetValue(Of String)(nodeProperties.photo))) Then
                'Obtain user image
                imgUrl = getMediaURL(member.GetValue(Of Integer)(nodeProperties.photo), Crops.members)
            ElseIf _giveDefault Then
                'if no image exists and user wants a default image, provide default image.
                imgUrl = getMediaURL(mediaNodes.defaultProfileImg, Crops.members)
            End If

            Return imgUrl
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\linqQueries\linqMembers.vb : getMemberPhoto_byId()")
            sb.AppendLine("_memberId:" & _memberId)
            sb.AppendLine("_giveDefault:" & _giveDefault)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Return "error: " & ex.ToString
            Return String.Empty
        End Try
    End Function
    Public Function getMemberPhotoNodeId_byId(ByVal _memberId As Integer) As Integer?
        Try
            'Instantiate variables.
            Dim imgNodeId As Integer = 0
            Dim member As IMember = ApplicationContext.Current.Services.MemberService.GetById(_memberId)

            'Check if image exists for user.
            If (member.HasProperty(nodeProperties.photo)) AndAlso (Not String.IsNullOrWhiteSpace(member.GetValue(Of String)(nodeProperties.photo))) Then
                'Obtain user image
                imgNodeId = member.GetValue(Of Integer)(nodeProperties.photo)
            End If

            Return imgNodeId
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\linqQueries\linqMembers.vb : getMemberPhotoNodeId_byId()")
            sb.AppendLine("_memberId:" & _memberId)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Return "error: " & ex.ToString
            Return 0
        End Try
    End Function

#End Region

#Region "Inserts"
    Public Function Insert(ByVal _valueDictionary As Dictionary(Of String, String)) As BusinessReturn
        'Instantiate variables
        'Set default msg for memberId
        Dim ValidationReturn As BusinessReturn = New BusinessReturn With {
            .ReturnMessage = 0
        }

        Try
            'Obtain all needed values for creating member
            Dim userName As String = _valueDictionary.Item(queryParameters.firstName) & " " & _valueDictionary.Item(queryParameters.lastName)
            Dim userId As String = _valueDictionary.Item(queryParameters.email) '_valueDictionary.Item(queryParameters.userId)
            Dim email As String = _valueDictionary.Item(queryParameters.email)
            Dim password As String = _valueDictionary.Item(queryParameters.password)
            Dim memberTypeAlias As String = memberRole.member

            'Create member
            Dim MemberService As IMemberService = ApplicationContext.Current.Services.MemberService
            Dim MemberGroupService As IMemberGroupService = ApplicationContext.Current.Services.MemberGroupService
            Dim newMember As IMember = MemberService.CreateMemberWithIdentity(userId, email, userName, memberTypeAlias)
            newMember.IsApproved = True
            MemberService.SavePassword(newMember, password)

            'Set member values
            newMember.SetValue("firstName", _valueDictionary.Item(queryParameters.firstName))
            newMember.SetValue("lastName", _valueDictionary.Item(queryParameters.lastName))

            'Set if member is using social media to login
            If _valueDictionary.ContainsKey(nodeProperties.isFacebookAcct) Then newMember.SetValue(nodeProperties.isFacebookAcct, True)
            If _valueDictionary.ContainsKey(nodeProperties.isLinkedInAcct) Then newMember.SetValue(nodeProperties.isFacebookAcct, True)
            If _valueDictionary.ContainsKey(nodeProperties.isTwitterAcct) Then newMember.SetValue(nodeProperties.isFacebookAcct, True)

            'Save new member
            MemberService.Save(newMember)

            'Save new member id
            ValidationReturn.ReturnMessage = newMember.Id

            'Create 
            CreateStripeCustomer(newMember.Id, userName, email)

            'Return successful
            Return ValidationReturn

            ''Log member in
            'Return logMemberIn(newMember.Username, newMember.RawPasswordValue)
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\linqQueries\linqMembers.vb : Insert()")
            sb.AppendLine("_valueDictionary:" & _valueDictionary.ToString())

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Throw
            ValidationReturn.ExceptionMessage = ex.ToString
            Return ValidationReturn
        End Try
    End Function
    Public Function Insert_byPreAcct(ByVal _preAccountData As PreAccountmembers) As BusinessReturn
        'Instantiate variables
        Dim ValidationReturn As BusinessReturn = New BusinessReturn
        Dim blPreAcctData As blPreAcctData = New blPreAcctData

        'Set default msg for memberId
        ValidationReturn.ReturnMessage = 0

        Try
            'Obtain all needed values for creating member
            Dim userName As String = _preAccountData.firstName & " " & _preAccountData.lastName
            Dim userId As String = _preAccountData.email
            Dim email As String = _preAccountData.email
            Dim password As String = _preAccountData.password
            Dim memberTypeAlias As String = memberRole.member
            Dim dob As Date = Date.Parse(_preAccountData.dob)

            'Create member
            Dim MemberService As IMemberService = ApplicationContext.Current.Services.MemberService
            Dim MemberGroupService As IMemberGroupService = ApplicationContext.Current.Services.MemberGroupService
            Dim newMember As IMember = MemberService.CreateMemberWithIdentity(userId, email, userName, memberTypeAlias)
            newMember.IsApproved = True
            MemberService.SavePassword(newMember, password)

            'Set member values
            'newMember.SetValue("firstName", _preAccountData.firstName)
            'newMember.SetValue("lastName", _preAccountData.lastName)
            newMember.SetValue(nodeProperties.firstName, _preAccountData.firstName)
            newMember.SetValue(nodeProperties.lastName, _preAccountData.lastName)
            newMember.SetValue(nodeProperties.dateOfBirth, dob)

            'Save new member
            MemberService.Save(newMember)

            'Save new member id
            ValidationReturn.ReturnMessage = newMember.Id

            'Create member in stripe
            CreateStripeCustomer(newMember.Id, userName, email)

            'Log Member in
            logMemberIn(userId, password)

            'Return successful
            Return ValidationReturn

        Catch ex As Exception
            'Throw
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\linqQueries\linqMembers.vb : Insert_byPreAcct()")
            sb.AppendLine("_preAccountData:" & _preAccountData.ToString())

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            ValidationReturn.ExceptionMessage = ex.ToString
            Return ValidationReturn
        End Try
    End Function
    'Public Function Insert_byPreAcctTwitter(ByVal _preAccountData As tblPreAccountData) As BusinessReturn
    '    'Instantiate variables
    '    Dim ValidationReturn As BusinessReturn = New BusinessReturn
    '    Dim blPreAcctData As blPreAcctData = New blPreAcctData

    '    'Set default msg for memberId
    '    ValidationReturn.ReturnMessage = 0

    '    Try
    '        'Obtain all needed values for creating member
    '        Dim userName As String = _preAccountData.firstName & " " & _preAccountData.lastName
    '        Dim userId As String = _preAccountData.email
    '        Dim email As String = ""
    '        Dim password As String = _preAccountData.password
    '        Dim memberTypeAlias As String = memberRole.member
    '        Dim dob As Date = Date.Parse(_preAccountData.dob)

    '        'Create member
    '        Dim MemberService As IMemberService = ApplicationContext.Current.Services.MemberService
    '        Dim MemberGroupService As IMemberGroupService = ApplicationContext.Current.Services.MemberGroupService
    '        Dim newMember As IMember = MemberService.CreateMemberWithIdentity(userId, email, userName, memberTypeAlias)
    '        newMember.IsApproved = True
    '        MemberService.SavePassword(newMember, password)

    '        'Set member values
    '        'newMember.SetValue("firstName", _preAccountData.firstName)
    '        'newMember.SetValue("lastName", _preAccountData.lastName)
    '        newMember.SetValue(nodeProperties.firstName, _preAccountData.firstName)
    '        newMember.SetValue(nodeProperties.lastName, _preAccountData.lastName)
    '        newMember.SetValue(nodeProperties.dateOfBirth, dob)

    '        'Save new member
    '        MemberService.Save(newMember)

    '        'Save new member id
    '        ValidationReturn.ReturnMessage = newMember.Id

    '        'Create member in stripe
    '        CreateStripeCustomer(newMember.Id, userName, email)

    '        'Log Member in
    '        logMemberIn(userId, password)

    '        'Mark the pre-acct as created in db.
    '        blPreAcctData.markPreAcctAsCreated(_preAccountData.preAcctId)

    '        'Return successful
    '        Return ValidationReturn

    '    Catch ex As Exception
    '        Dim sb As New StringBuilder()
    '        sb.AppendLine("\App_Code\linqQueries\linqMembers.vb : Insert_byPreAcctTwitter()")
    '        sb.AppendLine("_preAccountData:" & _preAccountData.ToString())

    '        saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
    '        'Throw
    '        ValidationReturn.ExceptionMessage = ex.ToString
    '        Return ValidationReturn
    '    End Try
    'End Function
    'Public Function Insert_byPreAcctSocailMedia(ByVal _preAccountData As tblPreAccountData) As BusinessReturn
    '    'Instantiate variables
    '    Dim ValidationReturn As BusinessReturn = New BusinessReturn
    '    Dim blPreAcctData As blPreAcctData = New blPreAcctData

    '    'Set default msg for memberId
    '    ValidationReturn.ReturnMessage = 0

    '    Try
    '        'Obtain all needed values for creating member
    '        Dim userName As String = _preAccountData.firstName & " " & _preAccountData.lastName
    '        Dim userId As String = _preAccountData.email
    '        Dim email As String = _preAccountData.email
    '        Dim password As String = _preAccountData.password
    '        Dim memberTypeAlias As String = memberRole.member
    '        Dim dob As Date = Date.Parse(_preAccountData.dob)

    '        'Create member
    '        Dim MemberService As IMemberService = ApplicationContext.Current.Services.MemberService
    '        Dim MemberGroupService As IMemberGroupService = ApplicationContext.Current.Services.MemberGroupService
    '        Dim newMember As IMember = MemberService.CreateMemberWithIdentity(userId, email, userName, memberTypeAlias)
    '        newMember.IsApproved = True
    '        MemberService.SavePassword(newMember, password)

    '        'Set member values
    '        'newMember.SetValue("firstName", _preAccountData.firstName)
    '        'newMember.SetValue("lastName", _preAccountData.lastName)
    '        newMember.SetValue(nodeProperties.firstName, _preAccountData.firstName)
    '        newMember.SetValue(nodeProperties.lastName, _preAccountData.lastName)
    '        newMember.SetValue(nodeProperties.dateOfBirth, dob)

    '        'Save new member
    '        MemberService.Save(newMember)

    '        'Save new member id
    '        ValidationReturn.ReturnMessage = newMember.Id

    '        'Create member in stripe
    '        CreateStripeCustomer(newMember.Id, userName, email)

    '        'Log Member in
    '        logMemberIn(userId, password)

    '        'Mark the pre-acct as created in db.
    '        blPreAcctData.markPreAcctAsCreated(_preAccountData.preAcctId)

    '        'Return successful
    '        Return ValidationReturn

    '    Catch ex As Exception
    '        Dim sb As New StringBuilder()
    '        sb.AppendLine("\App_Code\linqQueries\linqMembers.vb : Insert_byPreAcctSocailMedia()")
    '        sb.AppendLine("_preAccountData:" & _preAccountData.ToString())

    '        saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
    '        'Throw
    '        ValidationReturn.ExceptionMessage = ex.ToString
    '        Return ValidationReturn
    '    End Try
    'End Function
    Public Sub CreateStripeCustomer(ByVal _userId As Integer, ByVal _name As String, ByVal _email As String)
        'Instantiate variables
        Dim customerCreateOptions As StripeCustomerCreateOptions = New StripeCustomerCreateOptions
        Dim customerService = New StripeCustomerService(ConfigurationManager.AppSettings(Miscellaneous.StripeApiKey))
        Dim StripeCustomer As StripeCustomer
        Dim returnResult As BusinessReturn
        Dim member As New Member

        Try
            'Add values for creating customer.
            customerCreateOptions.Description = _name & " [" & _userId.ToString & "]"
            customerCreateOptions.Email = _email

            'Create customer in stripe
            StripeCustomer = customerService.Create(customerCreateOptions)

            'Add customer Id to member class
            member.StripeIDs.customerId = StripeCustomer.Id
            member.MembershipProperties.userId = _userId

            'Save to umbraco
            returnResult = InsertStripeIDs(member)

            If Not returnResult.isValid Then
                saveErrorMessage(_userId, returnResult.ExceptionMessage, "")
                'Else 'DELETE THESE
                '    saveErrorMessage(_userId, "TEST MSG", "everything appears to be working")
            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\linqQueries\linqMembers.vb : CreateStripeCustomer()")
            sb.AppendLine("_userId:" & _userId)
            sb.AppendLine("_name:" & _name)
            sb.AppendLine("_email:" & _email)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            saveErrorMessage(_userId, ex.ToString, "")
        End Try
    End Sub
    Public Function InsertStripeIDs(ByVal _member As Member) As BusinessReturn
        'Instantiate variables
        Dim returnResult As BusinessReturn = New BusinessReturn

        Try
            'Instantiate variables
            Dim MemberService As IMemberService = ApplicationContext.Current.Services.MemberService
            Dim member As IMember = ApplicationContext.Current.Services.MemberService.GetById(_member.MembershipProperties.userId)

            'Update the Notes field with existing card details before updating stripe information in member backoffice
            MoveStripeDataToNotes(_member.MembershipProperties.userId.ToString(), True)

            'Set campaign acct id.
            If Not String.IsNullOrWhiteSpace(_member.StripeIDs.customerId) Then
                member.SetValue(nodeProperties.customerId, _member.StripeIDs.customerId)
            End If
            'If Not String.IsNullOrWhiteSpace(_member.StripeIDs.campaignAcctId) Then
            '    member.SetValue(nodeProperties.campaignAccountId, _member.StripeIDs.campaignAcctId)
            'End If
            'If Not String.IsNullOrWhiteSpace(_member.StripeIDs.fileUploadId) Then
            '    member.SetValue(nodeProperties.fileUploadId, _member.StripeIDs.fileUploadId)
            'End If
            'If Not String.IsNullOrWhiteSpace(_member.StripeIDs.bankAcctToken) Then
            '    member.SetValue(nodeProperties.bankAccountToken, _member.StripeIDs.bankAcctToken)
            'End If
            'If Not String.IsNullOrWhiteSpace(_member.StripeIDs.bankAcctId) Then
            '    member.SetValue(nodeProperties.bankAccountId, _member.StripeIDs.bankAcctId)
            'End If
            If Not String.IsNullOrWhiteSpace(_member.StripeIDs.creditCardToken) Then
                member.SetValue(nodeProperties.creditCardToken, _member.StripeIDs.creditCardToken)
            End If
            If Not String.IsNullOrWhiteSpace(_member.StripeIDs.creditCardId) Then
                member.SetValue(nodeProperties.creditCardId, _member.StripeIDs.creditCardId)
            End If

            'Save new member
            MemberService.Save(member)

            Return returnResult
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\linqQueries\linqMembers.vb : InsertStripeIDs()")
            sb.AppendLine("_member:" & _member.ToString())

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            returnResult.ExceptionMessage = "Error: " & ex.ToString
            Return returnResult
        End Try
    End Function
    Public Function CreateCampaignMembersFolder(ByVal parentNodeId As Int16) As Int16?
        Try  'Create a new campaign member folder IPublishedContent
            Dim cs As IContentService = ApplicationContext.Current.Services.ContentService
            Dim campaign As IContent = cs.GetById(parentNodeId)
            Dim campaignMembers As IContent = cs.CreateContentWithIdentity(Miscellaneous.campaignMembers, campaign, docTypes.campaignMembers)
            cs.SaveAndPublishWithStatus(campaignMembers)
            'Return new IPublishedContent's Id
            Return campaignMembers.Id
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\linqQueries\linqMembers.vb : CreateCampaignMembersFolder()")
            sb.AppendLine("parentNodeId:" & parentNodeId)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return Nothing
        End Try
    End Function
#End Region

#Region "Updates"
    Public Function Update(ByVal _member As Member) As BusinessReturn
        'Instantiate variables
        Dim ValidationReturn As BusinessReturn = New BusinessReturn
        Dim member As IMember

        Try
            'Obtain current member
            member = ApplicationContext.Current.Services.MemberService.GetById(_member.MembershipProperties.userId)

            'Set values of member
            member.SetValue(nodeProperties.firstName, _member.Demographics.firstName)
            member.SetValue(nodeProperties.lastName, _member.Demographics.lastName)
            member.SetValue(nodeProperties.briefDescription, _member.Demographics.briefDescription)
            'member.SetValue(nodeProperties._umb_email, _member.MembershipProperties.email)
            If Not String.IsNullOrEmpty(_member.MembershipProperties.email) Then member.Email = _member.MembershipProperties.email
            If Not String.IsNullOrEmpty(_member.MembershipProperties.loginName) Then member.Username = _member.MembershipProperties.loginName
            member.SetValue(nodeProperties.alternativeEmail, _member.MembershipProperties.altEmail)

            member.SetValue(nodeProperties.address01_Billing, _member.BillingInfo.address01)
            member.SetValue(nodeProperties.address02_Billing, _member.BillingInfo.address02)
            member.SetValue(nodeProperties.city_Billing, _member.BillingInfo.city)
            member.SetValue(nodeProperties.stateprovidence_Billing, _member.BillingInfo.stateProvidence)
            member.SetValue(nodeProperties.postalCode_Billing, _member.BillingInfo.postalCode)

            member.SetValue(nodeProperties.address01_Shipping, _member.ShippingInfo.address01)
            member.SetValue(nodeProperties.address02_Shipping, _member.ShippingInfo.address02)
            member.SetValue(nodeProperties.city_Shipping, _member.ShippingInfo.city)
            member.SetValue(nodeProperties.stateprovidence_Shipping, _member.ShippingInfo.stateProvidence)
            member.SetValue(nodeProperties.postalCode_Shipping, _member.ShippingInfo.postalCode)

            'Save data to member.
            ApplicationContext.Current.Services.MemberService.Save(member)

            '
            If Not String.IsNullOrEmpty(_member.MembershipProperties.password) Then
                'Create memberservice
                Dim MemberService As IMemberService = ApplicationContext.Current.Services.MemberService
                MemberService.SavePassword(member, _member.MembershipProperties.password)
                'Save member's password
                MemberService.Save(member)
            End If

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\linqQueries\linqMembers.vb : Update()")
            sb.AppendLine("_member:" & _member.ToString())

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            ValidationReturn.ExceptionMessage = ex.ToString
        End Try

        Return ValidationReturn
    End Function
    Public Function updatePhoto(ByVal _memberId As Integer, ByVal _imediaId As Integer) As BusinessReturn
        'Instantiate variables
        Dim ValidationReturn As BusinessReturn = New BusinessReturn
        Dim member As IMember

        Try
            'Obtain member
            member = ApplicationContext.Current.Services.MemberService.GetById(_memberId)

            'Set values of member
            member.SetValue(nodeProperties.photo, _imediaId)

            'Save data to member.
            ApplicationContext.Current.Services.MemberService.Save(member)
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\linqQueries\linqMembers.vb : updatePhoto()")
            sb.AppendLine("_memberId:" & _memberId)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            ValidationReturn.ExceptionMessage = ex.ToString
        End Try

        Return ValidationReturn
    End Function
    Public Function UpdateAtCheckout(ByVal _member As Member) As BusinessReturn
        'Instantiate variables
        Dim ValidationReturn As BusinessReturn = New BusinessReturn
        Dim member As IMember

        Try
            'Obtain current member
            member = ApplicationContext.Current.Services.MemberService.GetById(_member.MembershipProperties.userId)

            'Set values of member
            'member.SetValue(nodeProperties.firstName, _member.Demographics.firstName)
            'member.SetValue(nodeProperties.lastName, _member.Demographics.lastName)
            'member.SetValue(nodeProperties.briefDescription, _member.Demographics.briefDescription)
            'member.SetValue(nodeProperties._umb_email, _member.MembershipProperties.email)
            'If Not String.IsNullOrEmpty(_member.MembershipProperties.email) Then member.Email = _member.MembershipProperties.email
            'If Not String.IsNullOrEmpty(_member.MembershipProperties.loginName) Then member.Username = _member.MembershipProperties.loginName

            member.SetValue(nodeProperties.address01_Billing, _member.BillingInfo.address01)
            member.SetValue(nodeProperties.address02_Billing, _member.BillingInfo.address02)
            member.SetValue(nodeProperties.city_Billing, _member.BillingInfo.city)
            member.SetValue(nodeProperties.stateprovidence_Billing, _member.BillingInfo.stateProvidence)
            member.SetValue(nodeProperties.postalCode_Billing, _member.BillingInfo.postalCode)

            member.SetValue(nodeProperties.address01_Shipping, _member.ShippingInfo.address01)
            member.SetValue(nodeProperties.address02_Shipping, _member.ShippingInfo.address02)
            member.SetValue(nodeProperties.city_Shipping, _member.ShippingInfo.city)
            member.SetValue(nodeProperties.stateprovidence_Shipping, _member.ShippingInfo.stateProvidence)
            member.SetValue(nodeProperties.postalCode_Shipping, _member.ShippingInfo.postalCode)

            If Not String.IsNullOrEmpty(_member.MembershipProperties.altEmail) Then member.SetValue(nodeProperties.alternativeEmail, _member.MembershipProperties.altEmail)

            'Save data to member.
            ApplicationContext.Current.Services.MemberService.Save(member)

            ''
            'If Not String.IsNullOrEmpty(_member.MembershipProperties.password) Then
            '    'Create memberservice
            '    Dim MemberService As IMemberService = ApplicationContext.Current.Services.MemberService
            '    MemberService.SavePassword(member, _member.MembershipProperties.password)
            '    'Save member's password
            '    MemberService.Save(member)
            'End If

            ValidationReturn.ReturnMessage = "Umbraco Updated Successfully"
            Return ValidationReturn

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\linqQueries\linqMembers.vb : UpdateAtCheckout()")
            sb.AppendLine("Member: " & JsonConvert.SerializeObject(_member))

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            ValidationReturn.ExceptionMessage = ex.ToString
        End Try

        Return ValidationReturn
    End Function
    Public Function UpdatePledges(ByVal _memberId As Integer, ByVal _pledgeId As Integer) As BusinessReturn
        'Instantiate variables
        Dim ValidationReturn As BusinessReturn = New BusinessReturn
        Dim member As IMember
        Dim _HoldPledgeId As String = Nothing


        Try
            'Obtain current member
            member = ApplicationContext.Current.Services.MemberService.GetById(_memberId)

            If member.HasProperty(nodeProperties.pledges) AndAlso Not IsNothing(member.GetValue(nodeProperties.pledges)) Then
                _HoldPledgeId = member.GetValue(nodeProperties.pledges).ToString()
            End If

            If IsNothing(_HoldPledgeId) Or String.IsNullOrEmpty(_HoldPledgeId) Then
                _HoldPledgeId += _pledgeId.ToString()
            Else
                _HoldPledgeId += "," + _pledgeId.ToString()
            End If

            member.SetValue(nodeProperties.pledges, _HoldPledgeId)

            'Save data to member.
            ApplicationContext.Current.Services.MemberService.Save(member)

            Return ValidationReturn

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqMembers.vb : UpdatePledges()")
            sb.AppendLine("_memberId: " & _memberId)
            sb.AppendLine("_pledgeId: " & _pledgeId)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            ValidationReturn.ExceptionMessage = ex.ToString

            Return ValidationReturn
        End Try
    End Function
    ' This function is used to moving the invaild or updated stripe information data to notes section in memeber back office
    Public Function MoveStripeDataToNotes(ByVal _memberId As String, Optional ByVal IsFinancialHandler As Boolean = False) As BusinessReturn
        'Instantiate variables
        Dim ValidationReturn As BusinessReturn = New BusinessReturn
        Dim member As IMember
        Dim _HoldStripeData As String
        Dim cb As New StringBuilder()
        Try
            'Obtain current member
            member = ApplicationContext.Current.Services.MemberService.GetById(CInt(_memberId))

            If member.HasProperty(nodeProperties.notes) AndAlso Not IsNothing(member.GetValue(nodeProperties.notes)) Then
                _HoldStripeData = member.GetValue(nodeProperties.notes).ToString()
                cb.AppendLine(" ")
                cb.AppendLine("---------------------" & DateTime.Now.ToLongTimeString() & "---------------------------")
                cb.AppendLine(_HoldStripeData)
                cb.AppendLine(" ")
            Else
                cb.AppendLine("---------------------" & DateTime.Now.ToLongTimeString() & "---------------------------")
                cb.AppendLine("Invalid Stripe informations : ")
                cb.AppendLine(" ")
            End If

            If member.HasProperty(nodeProperties.customerId) AndAlso Not IsNothing(member.GetValue(nodeProperties.customerId)) Then
                cb.AppendLine("CustomerID : " & member.GetValue(nodeProperties.customerId).ToString())
            End If

            If member.HasProperty(nodeProperties.creditCardId) AndAlso Not IsNothing(member.GetValue(nodeProperties.creditCardId)) Then
                cb.AppendLine("CreditCardId : " & member.GetValue(nodeProperties.creditCardId).ToString())
            End If

            If member.HasProperty(nodeProperties.creditCardToken) AndAlso Not IsNothing(member.GetValue(nodeProperties.creditCardToken)) Then
                cb.AppendLine("CreditCardToken : " & member.GetValue(nodeProperties.creditCardToken).ToString())
            End If

            member.SetValue(nodeProperties.notes, cb.ToString())

            'Clear field only if the request not made from the Financial Handler, if request made from the Financial Handler then do nothing 
            If IsFinancialHandler = False Then
                member.SetValue(nodeProperties.creditCardToken, String.Empty)
                member.SetValue(nodeProperties.customerId, String.Empty)
                member.SetValue(nodeProperties.creditCardId, String.Empty)
            End If

            'Save data to member.
            ApplicationContext.Current.Services.MemberService.Save(member)

            Return ValidationReturn

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqMembers.vb : MoveStripeDataToNotes()")
            sb.AppendLine("_member: " & _memberId)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            ValidationReturn.ExceptionMessage = ex.ToString

            Return ValidationReturn
        End Try
    End Function
    ' This function is used to insert the stripe customer id  in memeber back office
    Public Function InsertStripeCustomerID(ByVal _memberId As String, ByVal newCustomerId As String) As BusinessReturn
        'Instantiate variables
        Dim ValidationReturn As BusinessReturn = New BusinessReturn
        Dim member As IMember
        Try
            'Obtain current member
            member = ApplicationContext.Current.Services.MemberService.GetById(CInt(_memberId))
            member.SetValue(nodeProperties.customerId, newCustomerId)
            'Save data to member.
            ApplicationContext.Current.Services.MemberService.Save(member)

            Return ValidationReturn

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqMembers.vb : InsertStripeCustomerID()")
            sb.AppendLine("_member: " & _memberId)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            ValidationReturn.ExceptionMessage = ex.ToString

            Return ValidationReturn
        End Try
    End Function
    Public Function UpdateReviews(ByVal _memberId As Integer, ByVal _reviewId As Integer) As BusinessReturn
        'Instantiate variables
        Dim ValidationReturn As BusinessReturn = New BusinessReturn
        Dim member As IMember
        ' Dim _HoldReviewIDs As String = String.Empty
        Dim locaUdi As String = String.Empty

        Try
            'Obtain current member
            Dim IPublishedContent = ApplicationContext.Current.Services.ContentService.GetById(_reviewId)
            locaUdi = Udi.Create(Constants.UdiEntityType.Document, IPublishedContent.Key).ToString()
            member = ApplicationContext.Current.Services.MemberService.GetById(_memberId)
            If member.GetValue(nodeProperties.reviews) IsNot Nothing Then
                locaUdi = member.GetValue(nodeProperties.reviews).ToString() + "," + locaUdi
            End If
            member.SetValue(nodeProperties.reviews, locaUdi.ToString())

            'Save data to member.
            ApplicationContext.Current.Services.MemberService.Save(member)

            Return ValidationReturn

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqMembers.vb : UpdateReviews()")
            sb.AppendLine("_memberId: " & _memberId)
            sb.AppendLine("_reviewId: " & _reviewId)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            ValidationReturn.ExceptionMessage = ex.ToString

            Return ValidationReturn
        End Try
    End Function
    Public Function UpdateAltEmail(ByVal currentMemberId As Integer, ByVal altEmail As String) As BusinessReturn
        'Instantiate variables
        Dim ValidationReturn As BusinessReturn = New BusinessReturn
        Dim member As IMember

        Try
            'Obtain current member
            member = ApplicationContext.Current.Services.MemberService.GetById(currentMemberId)

            'Set values of member
            member.SetValue(nodeProperties.alternativeEmail, altEmail)

            'Save data to member.
            ApplicationContext.Current.Services.MemberService.Save(member)


            Dim sb As New StringBuilder
            sb.AppendLine("currentMemberId: " & currentMemberId)
            sb.AppendLine("altEmail: " & altEmail)
            sb.AppendLine("Name: " & member.Name)
            ValidationReturn.ReturnMessage = sb.ToString


            Return ValidationReturn

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqMembers.vb : UpdateAltEmail()")
            sb.AppendLine("currentMemberId:" & currentMemberId)
            sb.AppendLine("altEmail:" & altEmail)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            ValidationReturn.ExceptionMessage = ex.ToString
        End Try

        Return ValidationReturn
    End Function
#End Region

#Region "Deletes"
    Public Function DeleteCreditcard(ByVal _userId As Integer) As BusinessReturn
        'Instantiate variables
        Dim ValidationReturn As BusinessReturn = New BusinessReturn
        Dim member As IMember

        Try
            'Obtain current member
            member = ApplicationContext.Current.Services.MemberService.GetById(_userId)

            'Set values of member
            member.SetValue(nodeProperties.creditCardId, String.Empty)
            member.SetValue(nodeProperties.creditCardToken, String.Empty)

            'Save data to member.
            ApplicationContext.Current.Services.MemberService.Save(member)
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\linqQueries\linqMembers.vb : DeleteCreditcard()")
            sb.AppendLine("_userId:" & _userId)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            ValidationReturn.ExceptionMessage = ex.ToString
        End Try

        Return ValidationReturn
    End Function
#End Region

#Region "Log In/Out"
    Public Function isMemberLoggedIn() As Boolean
        Return memberShipHelper.IsLoggedIn()
    End Function
    Public Function logMemberIn(ByVal _userName As String, ByVal _password As String) As Boolean
        Try
            If memberShipHelper.Login(_userName, _password) Then
                'Set cookie
                System.Web.Security.FormsAuthentication.SetAuthCookie(_userName, False)

                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\linqQueries\linqMembers.vb : logMemberIn()")
            sb.AppendLine("_userName:" & _userName)
            sb.AppendLine("_password:" & _password)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return False
        End Try
    End Function
    Public Function externallogMemberIn(ByVal _userName As String) As Boolean
        Try
            'If memberShipHelper.Login(_userName, _password) Then
            System.Web.Security.FormsAuthentication.SetAuthCookie(_userName, False)
            Return True
            'Else
            '    Return False
            'End If
        Catch ex As Exception

            saveErrorMessage(getLoggedInMember, ex.ToString, "\App_Code\linqQueries\linqMembers.vb : externallogMemberIn()")
            Return False
        End Try
    End Function
    Public Sub logMemberOut()
        'Log member out
        Current.Session.Clear()
        Current.Session.Abandon()
        Roles.DeleteCookie()
        FormsAuthentication.SignOut()
    End Sub
#End Region

#Region "Methods"
    Public Function doesMemberExist_byUserId(ByVal _userId As String) As Boolean
        'Return if exists
        Return ApplicationContext.Current.Services.MemberService.Exists(_userId)
    End Function
    Public Function DoesMemberExist_byEmail(ByVal _email As String) As Boolean
        'Return if exists
        Dim member As IMember = ApplicationContext.Current.Services.MemberService.GetByEmail(_email)
        Return Not IsNothing(member)
    End Function
    Public Function getMembersLoginType(ByVal email As String) As mediaType_Values
        'Return if exists
        Dim member As IMember = ApplicationContext.Current.Services.MemberService.GetByEmail(email)

        If IsNothing(member) Then
            Return mediaType_Values.none
        Else
            'Determine what kind of account user is.
            If member.GetValue(Of Boolean)(nodeProperties.isFacebookAcct) Then
                Return mediaType_Values.Facebook
            ElseIf member.GetValue(Of Boolean)(nodeProperties.isTwitterAcct) Then
                Return mediaType_Values.Twitter
            ElseIf member.GetValue(Of Boolean)(nodeProperties.isLinkedInAcct) Then
                Return mediaType_Values.LinkedIn
            Else
                Return mediaType_Values.none
            End If
        End If


        Return Not IsNothing(member)
    End Function
    Public Function getCampaignMemberFolder(ByVal _campaignNode As IPublishedContent) As IPublishedContent
        Try
            'Loop thru child nodes, obtain campaignMember folder.
            For Each childNode As IPublishedContent In _campaignNode.Children
                If childNode.DocumentTypeAlias = docTypes.campaignMembers Then
                    Return childNode
                    Exit For
                End If
            Next
            Return Nothing
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\linqQueries\linqMembers.vb : getCampaignMemberFolder()")
            sb.AppendLine("_campaignNode:" & JsonConvert.SerializeObject(_campaignNode))
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
        Return Nothing
    End Function
    Public Sub EnsureStripeCustomerIdExist_byLoginName(ByVal _loginName As String)
        Try
            'Obtain member by email.
            Dim member As IMember = ApplicationContext.Current.Services.MemberService.GetByEmail(_loginName)

            'If property exists then exit sub
            If member.HasProperty(nodeProperties.customerId) AndAlso Not String.IsNullOrEmpty(member.GetValue(Of String)(nodeProperties.customerId)) Then
                Exit Sub
            Else
                'Value is missing.  create customer in stripe
                CreateStripeCustomer(member.Id, member.Name, _loginName)
            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\linqQueries\linqMembers.vb : EnsureStripeCustomerIdExist_byLoginName()")
            sb.AppendLine("_loginName:" & _loginName)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            saveErrorMessage(getLoggedInMember, ex.ToString, "")
        End Try
    End Sub
#End Region

End Class

