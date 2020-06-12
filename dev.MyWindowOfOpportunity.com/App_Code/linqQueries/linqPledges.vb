Imports umbraco
Imports Common
Imports umbraco.Core.Publishing
Imports umbraco.Core.Services
Imports umbraco.Core
Imports umbraco.Core.Models
Imports Stripe
Imports umbraco.Web
Imports Newtonsoft.Json
Imports System.Web.Script.Serialization

Public Class linqPledges

#Region "Properties"
    'Private linq2Db As linq2SqlDataContext = New linq2SqlDataContext(ConfigurationManager.ConnectionStrings("umbracoDbDSN").ToString)
    Dim _uHelper As Uhelper = New Uhelper()

#End Region


#Region "Selects"
    Public Function select_byPhaseId(ByVal _phaseId As Integer, Optional ByVal _amountOnly As Boolean = False) As List(Of CampaignPledge)
        'Instantiate variables
        Dim phase As IPublishedContent = _uHelper.Get_IPublishedContentByID(_phaseId)
        Dim lstCampaignPledges As New List(Of CampaignPledge)
        Dim campaignPledge As CampaignPledge
        Dim blMembers As New blMembers

        Try
            For Each pledge As IPublishedContent In phase.Children
                'Instantiate variables
                campaignPledge = New CampaignPledge

                'obtain fulfilled and unfulfilled
                If pledge.GetPropertyValue(Of Boolean)(nodeProperties.canceled) Or
                    pledge.GetPropertyValue(Of Boolean)(nodeProperties.transactionDeclined) Or
                    pledge.GetPropertyValue(Of Boolean)(nodeProperties.reimbursed) Then
                    Continue For
                Else


                    campaignPledge.pledgeAmount = pledge.GetPropertyValue(Of Decimal)(nodeProperties.pledgeAmount)


                End If

                'Obtain campaign data
                If _amountOnly = False Then
                    campaignPledge.campaignId = phase.Parent.Parent.Id
                    campaignPledge.campaignName = phase.Parent.Parent.Name
                    campaignPledge.canceled = pledge.GetPropertyValue(Of Boolean)(nodeProperties.canceled)
                    campaignPledge.dateCanceled = pledge.GetPropertyValue(Of Date)(nodeProperties.dateCanceled)
                    campaignPledge.dateDeclined = pledge.GetPropertyValue(Of Date)(nodeProperties.dateDeclined)
                    campaignPledge.fulfilled = pledge.GetPropertyValue(Of Boolean)(nodeProperties.fulfilled)
                    campaignPledge.fulfillmentDate = pledge.GetPropertyValue(Of Date)(nodeProperties.fulfillmentDate)
                    campaignPledge.phaseId = _phaseId
                    campaignPledge.phaseNumber = phase.GetPropertyValue(Of Decimal)(nodeProperties.phaseNumber)
                    campaignPledge.pledgeDate = pledge.GetPropertyValue(Of Date)(nodeProperties.pledgeDate)
                    campaignPledge.pledgeId = pledge.Id
                    campaignPledge.pledgingMember = pledge.GetPropertyValue(Of Integer)(nodeProperties.pledgingMember)
                    If campaignPledge.pledgingMember > 0 Then
                        campaignPledge.pledgingMemberImgUrl = blMembers.getMemberPhoto_byId(campaignPledge.pledgingMember)
                        campaignPledge.pledgingMemberName = blMembers.getMemberName_byId(campaignPledge.pledgingMember)
                    End If
                    campaignPledge.reimbursed = pledge.GetPropertyValue(Of Boolean)(nodeProperties.reimbursed)
                    campaignPledge.reimbursedDate = pledge.GetPropertyValue(Of Date)(nodeProperties.reimbursedDate)
                    campaignPledge.showAsAnonymous = pledge.GetPropertyValue(Of Boolean)(nodeProperties.showAsAnonymous)
                    campaignPledge.transactionDeclined = pledge.GetPropertyValue(Of Boolean)(nodeProperties.transactionDeclined)
                End If

                'Add campaign to list
                lstCampaignPledges.Add(campaignPledge)
            Next
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\linqQueries\linqPledges.vb : select_byPhaseId()")
            sb.AppendLine("_phaseId:" & _phaseId)
            sb.AppendLine("_amountOnly:" & _amountOnly)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try


        Return lstCampaignPledges
    End Function
    Public Function selectSum_byCampaignId(ByVal _campaignId As Integer) As Decimal
        'Instantiate variables
        Dim sum As Decimal = 0F
        Dim campaign As IPublishedContent = _uHelper.Get_IPublishedContentByID(_campaignId)

        Try
            'Drill thru campaign's parts until you get to the pledges.
            For Each campaignSegment As IPublishedContent In campaign.Children
                If campaignSegment.DocumentTypeAlias = docTypes.Phases Then
                    For Each phase As IPublishedContent In campaignSegment.Children
                        For Each pledge As IPublishedContent In phase.Children
                            'If valid, add pledge amount to sum
                            If pledge.GetPropertyValue(Of Boolean)(nodeProperties.canceled) Or
                                    pledge.GetPropertyValue(Of Boolean)(nodeProperties.transactionDeclined) Or
                                    pledge.GetPropertyValue(Of Boolean)(nodeProperties.reimbursed) Then
                                Continue For
                            Else
                                sum += pledge.GetPropertyValue(Of Decimal)(nodeProperties.pledgeAmount)
                            End If
                        Next
                    Next
                End If
            Next

            'Return total sum of valid pledges.
            Return sum
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\linqQueries\linqPledges.vb : selectSum_byCampaignId()")
            sb.AppendLine("_campaignId:" & _campaignId)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return sum
        End Try
    End Function
    Public Function selectSum_byPhaseId(ByVal _phaseId As Integer, ByVal _Type As String) As Decimal
        'Instantiate variables
        Dim sum As Decimal = 0F
        Dim Phase As IPublishedContent = _uHelper.Get_IPublishedContentByID(_phaseId)

        Try
            'Drill thru campaign's parts until you get to the pledges.
            For Each pledge As IPublishedContent In Phase.Children
                If (_Type = nodeProperties.canceled) Then
                    If pledge.GetPropertyValue(Of Boolean)(nodeProperties.canceled) Then
                        sum += pledge.GetPropertyValue(Of Decimal)(nodeProperties.pledgeAmount)
                    Else
                        Continue For
                    End If
                End If
                If (_Type = nodeProperties.reimbursed) Then
                    If pledge.GetPropertyValue(Of Boolean)(nodeProperties.reimbursed) Then
                        sum += pledge.GetPropertyValue(Of Decimal)(nodeProperties.pledgeAmount)
                    Else
                        Continue For
                    End If
                End If
                If (_Type = nodeProperties.fulfilled) Then
                    If pledge.GetPropertyValue(Of Boolean)(nodeProperties.fulfilled) Then
                        sum += pledge.GetPropertyValue(Of Decimal)(nodeProperties.pledgeAmount)
                    Else
                        Continue For
                    End If
                End If
                If (_Type = nodeProperties.transactionDeclined) Then
                    If pledge.GetPropertyValue(Of Boolean)(nodeProperties.transactionDeclined) Then
                        sum += pledge.GetPropertyValue(Of Decimal)(nodeProperties.pledgeAmount)
                    Else
                        Continue For
                    End If
                End If
            Next

            'Return total sum of valid pledges.
            Return sum
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\linqQueries\linqPledges.vb : selectSum_byPhaseId()")
            sb.AppendLine("_phaseId:" & _phaseId)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return sum
        End Try
    End Function
    Public Function selectSum_byPhaseId(ByVal _phaseId As Integer) As Decimal
        'Instantiate variables
        Dim sum As Decimal = 0F
        Dim Phase As IPublishedContent = _uHelper.Get_IPublishedContentByID(_phaseId)

        Try
            'Drill thru campaign's parts until you get to the pledges.


            For Each pledge As IPublishedContent In Phase.Children
                'If valid, add pledge amount to sum
                If pledge.GetPropertyValue(Of Boolean)(nodeProperties.canceled) Or
                                    pledge.GetPropertyValue(Of Boolean)(nodeProperties.transactionDeclined) Or
                                    pledge.GetPropertyValue(Of Boolean)(nodeProperties.reimbursed) Then
                    Continue For
                Else
                    sum += pledge.GetPropertyValue(Of Decimal)(nodeProperties.pledgeAmount)
                End If
            Next


            'Return total sum of valid pledges.
            Return sum
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\linqQueries\linqPledges.vb : selectSum_byPhaseId()")
            sb.AppendLine("_phaseId:" & _phaseId)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return sum
        End Try
    End Function
    Public Function selectCount_byPhaseId(ByVal _phaseId As Integer) As Decimal
        'Instantiate variables
        Dim sum As Integer = 0
        Dim Phase As IPublishedContent = _uHelper.Get_IPublishedContentByID(_phaseId)

        Try
            'Drill thru campaign's parts until you get to the pledges.


            For Each pledge As IPublishedContent In Phase.Children
                'If valid, add pledge amount to sum
                If pledge.GetPropertyValue(Of Boolean)(nodeProperties.canceled) Or
                                    pledge.GetPropertyValue(Of Boolean)(nodeProperties.transactionDeclined) Or
                                    pledge.GetPropertyValue(Of Boolean)(nodeProperties.reimbursed) Then
                    Continue For
                Else
                    sum += 1
                End If
            Next


            'Return total sum of valid pledges.
            Return sum
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\linqQueries\linqPledges.vb : selectCount_byPhaseId()")
            sb.AppendLine("_phaseId:" & _phaseId)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return sum
        End Try
    End Function
    Public Function selectCount_byPhaseId(ByVal _phaseId As Integer, ByVal _Type As String) As Decimal
        'Instantiate variables
        Dim sum As Integer = 0
        Dim Phase As IPublishedContent = _uHelper.Get_IPublishedContentByID(_phaseId)

        Try
            'Drill thru campaign's parts until you get to the pledges.


            For Each pledge As IPublishedContent In Phase.Children
                If (_Type = nodeProperties.canceled) Then

                    If pledge.GetPropertyValue(Of Boolean)(nodeProperties.canceled) Then
                        sum += 1

                    Else
                        Continue For
                    End If

                End If
                If (_Type = nodeProperties.reimbursed) Then

                    If pledge.GetPropertyValue(Of Boolean)(nodeProperties.reimbursed) Then
                        sum += 1

                    Else
                        Continue For
                    End If

                End If
                If (_Type = nodeProperties.fulfilled) Then

                    If pledge.GetPropertyValue(Of Boolean)(nodeProperties.fulfilled) Then
                        sum += 1

                    Else
                        Continue For
                    End If

                End If
                If (_Type = nodeProperties.transactionDeclined) Then

                    If pledge.GetPropertyValue(Of Boolean)(nodeProperties.transactionDeclined) Then
                        sum += 1

                    Else
                        Continue For
                    End If

                End If

            Next


            'Return total sum of valid pledges.
            Return sum
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\linqQueries\linqPledges.vb : selectCount_byPhaseId()")
            sb.AppendLine("_phaseId:" & _phaseId)
            sb.AppendLine("_Type" & _Type)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return sum
        End Try
    End Function
    Public Function selectCount_byCampaignId(ByVal _campaignId As Integer) As Integer

        'Instantiate variables
        Dim sum As Decimal = 0F
        Dim campaign As IPublishedContent = _uHelper.Get_IPublishedContentByID(_campaignId)

        Try
            'Drill thru campaign's parts until you get to the pledges.
            For Each campaignSegment As IPublishedContent In campaign.Children
                If campaignSegment.DocumentTypeAlias = docTypes.Phases Then
                    For Each phase As IPublishedContent In campaignSegment.Children
                        For Each pledge As IPublishedContent In phase.Children
                            'If valid, add pledge amount to sum
                            If pledge.GetPropertyValue(Of Boolean)(nodeProperties.canceled) Or
                                    pledge.GetPropertyValue(Of Boolean)(nodeProperties.transactionDeclined) Or
                                    pledge.GetPropertyValue(Of Boolean)(nodeProperties.reimbursed) Then
                                Continue For
                            Else


                                sum += 1  'pledge.GetProperty(Of Decimal)(nodeProperties.pledgeAmount)


                            End If
                        Next
                    Next
                End If
            Next

            'Return total sum of valid pledges.
            Return sum
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\linqQueries\linqPledges.vb : selectCount_byCampaignId()")
            sb.AppendLine("_campaignId:" & _campaignId)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return sum
        End Try
    End Function
    Public Function selectAll_byCampaignId_forEditCampaign(ByVal _campaignId As Integer) As BusinessReturn
        'Instantiate variables
        Dim bReturn As New BusinessReturn
        Dim campaignNode As IPublishedContent
        Dim lstPhases As New List(Of IPublishedContent)
        Dim phaseIndex As UInt16 = 0
        Dim campaignPledge As CampaignPledge
        Dim lstCampaignPledges As New List(Of CampaignPledge)
        Dim blMembers As blMembers


        Try
            'Initialize variables
            campaignNode = _uHelper.Get_IPublishedContentByID(_campaignId)

            If Not IsNothing(campaignNode) Then
                'Obtain phases folder
                For Each childNode As IPublishedContent In campaignNode.Children
                    If childNode.DocumentTypeAlias = docTypes.Phases Then
                        'Obtain each phase folder
                        For Each phaseNode As IPublishedContent In childNode.Children
                            lstPhases.Add(phaseNode)
                        Next

                        'Once complete, exit for loop
                        Exit For
                    End If
                Next

                'Obtain phase data
                If lstPhases.Count > 0 Then
                    For Each phase As IPublishedContent In lstPhases
                        'Increment phase index
                        phaseIndex += 1

                        'Loop thru phase
                        For Each pledge As IPublishedContent In phase.Children
                            'Instantiate new class object
                            'Add data to object
                            campaignPledge = New CampaignPledge With {
                                .phaseId = phaseIndex,
                                .phaseNumber = pledge.GetPropertyValue(Of Decimal)(nodeProperties.phaseNumber),
                                .pledgeAmount = pledge.GetPropertyValue(Of Decimal)(nodeProperties.pledgeAmount),
                                .pledgeDate = pledge.GetPropertyValue(Of Date)(nodeProperties.pledgeDate),
                                .showAsAnonymous = pledge.GetPropertyValue(Of Boolean)(nodeProperties.showAsAnonymous),
                                .pledgeId = pledge.Id
                            }

                            If campaignPledge.showAsAnonymous Then
                                campaignPledge.pledgingMemberName = Miscellaneous.Anonymous
                            Else
                                blMembers = New blMembers

                                campaignPledge.pledgingMember = pledge.GetPropertyValue(Of Integer)(nodeProperties.pledgingMember)
                                If campaignPledge.pledgingMember > 0 Then
                                    campaignPledge.pledgingMemberName = blMembers.getMemberName_byId(campaignPledge.pledgingMember)
                                End If
                            End If
                            campaignPledge.fulfilled = pledge.GetPropertyValue(Of Boolean)(nodeProperties.fulfilled)
                            campaignPledge.canceled = pledge.GetPropertyValue(Of Boolean)(nodeProperties.canceled)
                            campaignPledge.transactionDeclined = pledge.GetPropertyValue(Of Boolean)(nodeProperties.transactionDeclined)
                            campaignPledge.reimbursed = pledge.GetPropertyValue(Of Boolean)(nodeProperties.reimbursed)







                            'Dim sb2 As New StringBuilder()
                            'sb2.AppendLine("***** TEST *****")
                            'sb.AppendLine("_campaignId:" & _campaignId)


                            'Obtain reward fulfillment data
                            If campaignPledge.fulfilled Then
                                If pledge.HasValue(nodeProperties.rewardSelected) Then

                                    blMembers = New blMembers
                                    Dim member As Member
                                    Dim memberId As Integer? = pledge.GetPropertyValue(Of Integer)(nodeProperties.pledgingMember)

                                    If Not IsNothing(memberId) Then
                                        Dim businessReturn As BusinessReturn = blMembers.getMemberDemographics_byId(memberId, False, False, True, True)
                                        If businessReturn.isValid Then
                                            'Extract data
                                            member = DirectCast(businessReturn.DataContainer(0), Member)

                                            'Instantiate variables
                                            Dim rewardFulfillment As New RewardFulfillment
                                            Dim shippingInfo As ShippingInfo = member.ShippingInfo

                                            'Populate classes with data
                                            rewardFulfillment.rewardId = pledge.GetPropertyValue(Of Integer)(nodeProperties.rewardSelected)
                                            rewardFulfillment.rewardTitle = _uHelper.Get_IPublishedContentByID(rewardFulfillment.rewardId).Name
                                            rewardFulfillment.rewardShipped = pledge.GetPropertyValue(Of Boolean)(nodeProperties.rewardShipped)
                                            rewardFulfillment.trackingNo = pledge.GetPropertyValue(Of String)(nodeProperties.trackingNo)
                                            rewardFulfillment.campaignMngrNotes = pledge.GetPropertyValue(Of String)(nodeProperties.campaignMngrNotes)

                                            rewardFulfillment.memberId = memberId
                                            If Not campaignPledge.showAsAnonymous Then rewardFulfillment.memberName = campaignPledge.pledgingMemberName
                                            rewardFulfillment.memberEmail = member.MembershipProperties.email


                                            'Save classes
                                            rewardFulfillment.shippingInfo = shippingInfo
                                            campaignPledge.rewardFulfillment = rewardFulfillment
                                        End If
                                    End If

                                End If
                            End If

                            'Add object to class
                            lstCampaignPledges.Add(campaignPledge)
                        Next
                    Next
                End If

                'Add data to return msg.
                bReturn.DataContainer.Add(lstCampaignPledges)
            End If


            Return bReturn
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqPledges.vb : selectAll_byCampaignId_forEditCampaign()")
            sb.AppendLine("_campaignId:" & _campaignId)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            bReturn.ExceptionMessage = ex.ToString
            Return bReturn
        End Try
    End Function
    Public Function selectAll_byCampaignId_forCampaignPg(ByVal _campaignId As Integer) As BusinessReturn
        'Instantiate variables
        Dim bReturn As New BusinessReturn
        Dim ipCampaign As IPublishedContent
        Dim lstPhases As New List(Of IPublishedContent)
        Dim phaseIndex As UInt16 = 0
        Dim campaignPledge As CampaignPledge
        Dim lstCampaignPledges As New List(Of CampaignPledge)
        Dim blMembers As blMembers
        Dim memberPhotoNodeId As Integer = 0


        Try
            'Initialize variables
            ipCampaign = _uHelper.Get_IPublishedContentByID(_campaignId)

            'Obtain phases folder
            If Not IsNothing(ipCampaign) Then
                For Each childNode As IPublishedContent In ipCampaign.Children
                    If childNode.DocumentTypeAlias = docTypes.Phases Then
                        'Obtain each phase folder
                        For Each phaseNode As IPublishedContent In childNode.Children
                            lstPhases.Add(phaseNode)
                        Next

                        'Once complete, exit for loop
                        Exit For
                    End If
                Next
            End If


            'Obtain phase data
            If lstPhases.Count > 0 Then
                For Each phase As IPublishedContent In lstPhases
                    'Increment phase index
                    'phaseIndex += 1



                    'Loop thru phases
                    For Each pledgeNode As IPublishedContent In phase.Children
                        'Skip the following phases
                        If pledgeNode.GetPropertyValue(Of Boolean)(nodeProperties.canceled) = True OrElse
                            pledgeNode.GetPropertyValue(Of Boolean)(nodeProperties.transactionDeclined) = True OrElse
                            pledgeNode.GetPropertyValue(Of Boolean)(nodeProperties.reimbursed) = True Then
                            Continue For
                        End If

                        'Instantiate new class object
                        'Add data to object
                        campaignPledge = New CampaignPledge With {
                            .phaseId = phase.Id, 'phaseIndex,
                            .phaseNumber = phase.GetPropertyValue(Of Decimal)(nodeProperties.phaseNumber),
                            .pledgeAmount = pledgeNode.GetPropertyValue(Of Decimal)(nodeProperties.pledgeAmount),
                            .pledgeDate = pledgeNode.GetPropertyValue(Of Date)(nodeProperties.pledgeDate),
                            .showAsAnonymous = pledgeNode.GetPropertyValue(Of Boolean)(nodeProperties.showAsAnonymous)
                        }

                        If campaignPledge.showAsAnonymous Then
                            campaignPledge.pledgingMemberName = Miscellaneous.Anonymous
                            campaignPledge.pledgingMemberImgUrl = getMediaURL(mediaNodes.defaultProfileImg, Crops.campaignSummaryImage)
                        Else
                            blMembers = New blMembers


                            campaignPledge.pledgingMember = pledgeNode.GetPropertyValue(Of Integer)(nodeProperties.pledgingMember)
                            If campaignPledge.pledgingMember > 0 Then
                                campaignPledge.pledgingMemberName = blMembers.getMemberName_byId(campaignPledge.pledgingMember)

                                'Obtain member's photo Id
                                memberPhotoNodeId = blMembers.getMemberPhotoNodeId_byId(pledgeNode.GetPropertyValue(Of Integer)(nodeProperties.pledgingMember))
                                If memberPhotoNodeId > 0 Then
                                    campaignPledge.pledgingMemberImgUrl = getMediaURL(memberPhotoNodeId, Crops.campaignSummaryImage)
                                Else
                                    campaignPledge.pledgingMemberImgUrl = getMediaURL(mediaNodes.defaultProfileImg, Crops.campaignSummaryImage)
                                End If
                            End If
                        End If

                        'Add object to class
                        lstCampaignPledges.Add(campaignPledge)
                    Next
                Next
            End If

            'Add data to return msg.
            bReturn.DataContainer.Add(lstCampaignPledges)

            Return bReturn
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\linqQueries\linqPledges.vb : selectAll_byCampaignId_forCampaignPg()")
            sb.AppendLine("_campaignId:" & _campaignId)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            bReturn.ExceptionMessage = ex.ToString
            Return bReturn
        End Try
    End Function
    Public Function obtainPledgeStatus_byId(ByVal _pledgeId As Integer) As CampaignPledge
        Try
            '
            Dim pledge As New CampaignPledge
            Dim pledgeNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(_pledgeId)

            'Obtain data
            pledge.pledgeId = _pledgeId
            If pledgeNode.HasProperty(nodeProperties.fulfilled) Then pledge.fulfilled = pledgeNode.GetPropertyValue(Of Boolean)(nodeProperties.fulfilled)
            If pledgeNode.HasProperty(nodeProperties.transactionDeclined) Then pledge.transactionDeclined = pledgeNode.GetPropertyValue(Of Boolean)(nodeProperties.transactionDeclined)
            If pledgeNode.HasProperty(nodeProperties.canceled) Then pledge.canceled = pledgeNode.GetPropertyValue(Of Boolean)(nodeProperties.canceled)
            If pledgeNode.HasProperty(nodeProperties.reimbursed) Then pledge.reimbursed = pledgeNode.GetPropertyValue(Of Boolean)(nodeProperties.reimbursed)

            Return pledge

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqPledges.vb : obtainPledgeStatus_byId()")
            sb.AppendLine("_pledgeId:" & _pledgeId)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return Nothing
        End Try
    End Function
    Public Function obtainIncompletePledges_byPhase(ByVal activePhaseId As Integer) As List(Of CampaignPledge)
        Dim lstPledges As New List(Of CampaignPledge)

        Try
            'Obtain active phase
            Dim ipActivePhase As IPublishedContent = _uHelper.Get_IPublishedContentByID(activePhaseId)
            Dim pledge As CampaignPledge

            'Loop thru pledges
            For Each ipPledge As IPublishedContent In ipActivePhase.Children
                'Only add id if pledge is marked as fulfilled.
                If ipPledge.GetPropertyValue(Of Boolean)(nodeProperties.fulfilled) = True AndAlso
                    ipPledge.GetPropertyValue(Of Boolean)(nodeProperties.transfered) = False Then

                    pledge = New CampaignPledge
                    pledge.pledgeId = ipPledge.Id
                    pledge.chargeId = ipPledge.GetPropertyValue(Of String)(nodeProperties.chargeId)
                    pledge.destinationId = ipPledge.GetPropertyValue(Of String)(nodeProperties.destinationId)
                    lstPledges.Add(pledge)
                End If
            Next
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqPledges.vb : obtainIncompletePledges_byPhase()")
            sb.AppendLine("activePhaseId: " & activePhaseId)
            sb.AppendLine("lstPledges: " & JsonConvert.SerializeObject(lstPledges))
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try

        Return lstPledges
    End Function
#End Region

#Region "Inserts"
    Public Function CreatePledge(ByVal _pledge As CampaignPledge) As BusinessReturn
        'Instantiate global variables
        Dim businessReturn As New BusinessReturn

        Try
            'Instantiate variables
            Dim cs As IContentService = ApplicationContext.Current.Services.ContentService
            Dim PledgeName As String
            Dim phase As IContent

            'Create a new Pledge IPublishedContent
            PledgeName = _pledge.pledgeAmount
            phase = cs.GetById(_pledge.phaseId)

            Dim iPledge As IContent = cs.CreateContentWithIdentity(PledgeName, phase, docTypes.Pledges)

            'Set default values
            iPledge.SetValue(nodeProperties.pledgeAmount, _pledge.pledgeAmount)
            iPledge.SetValue(nodeProperties.stripeFees, _pledge.stripeFee)
            iPledge.SetValue(nodeProperties.mWoOFees, _pledge.mwooFee)
            iPledge.SetValue(nodeProperties.netPledgeAmount, _pledge.netPledgeAmount)
            iPledge.SetValue(nodeProperties.pledgingMember, _pledge.pledgingMember)
            iPledge.SetValue(nodeProperties.pledgeDate, _pledge.pledgeDate) 'Note: this time is based in Europe
            iPledge.SetValue(nodeProperties.showAsAnonymous, _pledge.showAsAnonymous)
            iPledge.SetValue(nodeProperties.balanceTransactionId, _pledge.balanceTransactionId)
            iPledge.SetValue(nodeProperties.chargeId, _pledge.chargeId)
            iPledge.SetValue(nodeProperties.chargeStatus, _pledge.chargeStatus)
            iPledge.SetValue(nodeProperties.customerId, _pledge.customerId)
            iPledge.SetValue(nodeProperties.destinationId, _pledge.destinationId)
            iPledge.SetValue(nodeProperties.transferId, _pledge.transferId)
            iPledge.SetValue(nodeProperties.transferGroup, _pledge.transferGroup)
            iPledge.SetValue(nodeProperties.notes, DateTime.Now & " | " & _pledge.notes)
            iPledge.SetValue(nodeProperties.fulfilled, _pledge.fulfilled)
            iPledge.SetValue(nodeProperties.fulfillmentDate, _pledge.fulfillmentDate)

            If Not IsNothing(_pledge.rewardSelected) AndAlso _pledge.rewardSelected > 0 Then
                iPledge.SetValue(nodeProperties.rewardSelected, _pledge.rewardSelected)
            End If

            'Save new Pledge
            cs.SaveAndPublishWithStatus(iPledge)

            'Return new IPublishedContent's Id
            _pledge.pledgeId = iPledge.Id
            businessReturn.DataContainer.Add(_pledge)
            Return businessReturn

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqPledges.vb : CreatePledge()")
            sb.AppendLine("_pledge:")
            sb.AppendLine(JsonHelper.ToJson(_pledge))
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

            businessReturn.ExceptionMessage = ex.ToString
            Return businessReturn
        End Try

    End Function
    Public Sub createPayoutRequest(ByVal payout As Payout)
        Try
            'Instantiate variables
            Dim cs As IContentService = ApplicationContext.Current.Services.ContentService
            Dim icPendingPayouts As IContent = cs.GetById(siteNodes.pendingPayouts)
            Dim icName As String

            'Create the payout request's name
            icName = payout.campaign.Name & " | $" & (CInt(payout.payoutTotal) / 100).ToString

            Dim icPayout As IContent = cs.CreateContentWithIdentity(icName, icPendingPayouts, docTypes.payout)

            'Set default values
            icPayout.SetValue(nodeProperties.campaign, payout.campaign.Id)
            icPayout.SetValue(nodeProperties.activePhase, payout.activePhase.Id)
            icPayout.SetValue(nodeProperties.payoutTotal, payout.payoutTotal)
            icPayout.SetValue(nodeProperties.dateToProcess, payout.dateToProcess)
            icPayout.SetValue(nodeProperties.notes, DateTime.Now & " | Payout request created.")

            'Save new Pledge
            cs.SaveAndPublishWithStatus(icPayout)

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqPledges.vb : createPayoutRequest()")
            sb.AppendLine("payout: " & payout.campaign.Name & " | $" & (CInt(payout.payoutTotal) / 100).ToString)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
    End Sub
#End Region

#Region "Updates"
    Public Function UpdatePledgeStatus(ByVal pledgeId As Int16, ByVal stripeCharge As StripeCharge) As Attempt(Of PublishStatus)
        Try
            'Instantiate variables
            Dim cs As IContentService = ApplicationContext.Current.Services.ContentService
            Dim pledge As IContent = cs.GetById(pledgeId)

            'Determine what type of status payment is
            Select Case stripeCharge.Status
                Case "succeeded"
                    pledge.SetValue(nodeProperties.fulfilled, True)
                    pledge.SetValue(nodeProperties.fulfillmentDate, DateTime.Now)
                    pledge.SetValue(nodeProperties.transactionId, stripeCharge.BalanceTransactionId)
                Case "pending"
                'pledge.SetValue(nodeProperties.transactionDeclined, True)
                'pledge.SetValue(nodeProperties.dateDeclined, DateTime.Now)
                Case "failed"
                    pledge.SetValue(nodeProperties.transactionDeclined, True)
                    pledge.SetValue(nodeProperties.dateDeclined, DateTime.Now)
            End Select

            'Convert stripecharge to json and save in notes.
            Dim sb As New StringBuilder
            sb.AppendLine("Transaction Submitted: " & DateTime.Now.ToString)
            sb.AppendLine("==================================")
            sb.AppendLine(Newtonsoft.Json.JsonConvert.SerializeObject(stripeCharge))
            pledge.SetValue(nodeProperties.notes, addToNotes(pledge.Id, sb.ToString))


            'Save values
            Return cs.SaveAndPublishWithStatus(pledge)
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqPledges.vb : UpdatePledgeStatus()")
            sb.AppendLine("pledgeId: " & pledgeId)
            sb.AppendLine("stripeCharge: " & JsonConvert.SerializeObject(stripeCharge))
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

            Return Nothing
        End Try
    End Function
    Public Function UpdateContentCancelled(ByVal pledgeId As Int16, ByVal _canceled As Boolean) As Attempt(Of PublishStatus)
        Try
            'Instantiate variables
            Dim cs As IContentService = ApplicationContext.Current.Services.ContentService
            Dim pledge As IContent = cs.GetById(pledgeId)
            'Set values
            pledge.SetValue(nodeProperties.canceled, _canceled)
            pledge.SetValue(nodeProperties.dateCanceled, DateTime.Now)
            pledge.SetValue(nodeProperties.notes, addToNotes(pledge.Id, DateTime.Now & " | Canceled transaction.  Phase did not meet its goal."))

            'Save values
            Return cs.SaveAndPublishWithStatus(pledge)
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqPledges.vb : UpdateContentCancelled()")
            sb.AppendLine("pledgeId: " & pledgeId)
            sb.AppendLine("_canceled: " & _canceled)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

            Return Nothing
        End Try
    End Function
    Public Function refundPledge(ByVal ipPledge As IPublishedContent) As Attempt(Of PublishStatus)
        Try

            '	refund pledge
            '	mark as cancelled/refunded with note
            '	send email to pledger

            'Instantiate variables
            Dim chargeId As String = ipPledge.GetPropertyValue(Of String)(nodeProperties.chargeId)
            Dim pledgeNotes As String = ipPledge.GetPropertyValue(Of String)(nodeProperties.notes)
            Dim refundOptions As New StripeRefundCreateOptions
            Dim refundService As New StripeRefundService
            Dim refund As StripeRefund
            Dim cs As IContentService = ApplicationContext.Current.Services.ContentService
            Dim icPledge As IContent = cs.GetById(ipPledge.Id)
            Dim attempt As New Attempt(Of PublishStatus)
            Dim sb As New StringBuilder




            If icPledge.Published Then
                'Submit refund
                refundService.ApiKey = ConfigurationManager.AppSettings(Miscellaneous.StripeApiKey)
                refundOptions.Reason = "requested_by_customer"
                refundOptions.Metadata = New Dictionary(Of String, String)() From {{"Reason", "Campaign failed to meet goal.  All pledges are refunded."}}
                refund = refundService.Create(chargeId, refundOptions)

                'Set values
                sb.AppendLine(DateTime.Now.ToString & " | Pledge refunded by Stripe")
                sb.AppendLine(refund.StripeResponse.ResponseJson)
                sb.AppendLine("")
                sb.AppendLine("==================")
                sb.AppendLine("")
                sb.AppendLine(pledgeNotes)

                icPledge.SetValue(nodeProperties.notes, sb.ToString)
                If refund.Status = "succeeded" Then
                    'uncheck fulfilled
                    'check reimbursed
                    'add reimbursed date
                    icPledge.SetValue(nodeProperties.fulfilled, False)
                    icPledge.SetValue(nodeProperties.reimbursed, True)
                    icPledge.SetValue(nodeProperties.reimbursedDate, DateTime.Now.ToString)
                Else
                    'log error
                    Dim sbError As New StringBuilder()
                    sbError.AppendLine("failed to refund pledge. Must be refunded manually")
                    sbError.AppendLine("linqPledges.vb : refundPledge()")
                    sbError.AppendLine("pledgeId: " & ipPledge.Id)
                    sbError.AppendLine("name: " & ipPledge.Name)
                    sbError.AppendLine(refund.StripeResponse.ResponseJson)
                    saveErrorMessage(getLoggedInMember, sbError.ToString, sbError.ToString())
                End If


                'Save values
                attempt = cs.SaveAndPublishWithStatus(icPledge)
                cs.Publish(icPledge)
            End If



            Return attempt

        Catch strEx As StripeException
            Dim sb As New StringBuilder()
            sb.AppendLine("linqPledges.vb : refundPledge()")
            sb.AppendLine("STRIPE EXCEPTION")
            sb.AppendLine("pledge: " & ipPledge.Id & " | " & ipPledge.Name)
            sb.AppendLine("phase: " & ipPledge.Parent.Name & " | " & ipPledge.Parent.Id)
            sb.AppendLine("campaign: " & ipPledge.Parent.Parent.Parent.Name & " | " & ipPledge.Parent.Parent.Parent.Id)
            saveErrorMessage(getLoggedInMember, strEx.ToString, sb.ToString())

            Return Nothing

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqPledges.vb : refundPledge()")
            sb.AppendLine("EXCEPTION")
            sb.AppendLine("pledge: " & ipPledge.Id & " | " & ipPledge.Name)
            sb.AppendLine("phase: " & ipPledge.Parent.Name & " | " & ipPledge.Parent.Id)
            sb.AppendLine("campaign: " & ipPledge.Parent.Parent.Parent.Name & " | " & ipPledge.Parent.Parent.Parent.Id)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

            Return Nothing
        End Try
    End Function
    Public Function UpdatePledgeMngrNotes(ByVal pledgeId As Integer, ByVal rewardShipped As Boolean, ByVal trackingNo As String, ByVal campaignMngrNotes As String) As Attempt(Of PublishStatus)
        Try
            'Instantiate variables
            Dim cs As IContentService = ApplicationContext.Current.Services.ContentService
            Dim pledge As IContent = cs.GetById(pledgeId)

            'Set values
            pledge.SetValue(nodeProperties.rewardShipped, rewardShipped)
            pledge.SetValue(nodeProperties.trackingNo, trackingNo)
            pledge.SetValue(nodeProperties.campaignMngrNotes, campaignMngrNotes)

            'Save values
            Return cs.SaveAndPublishWithStatus(pledge)
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqPledges.vb : UpdatePledgeMngrNotes()")
            sb.AppendLine("pledgeId: " & pledgeId)
            sb.AppendLine("rewardShipped: " & rewardShipped)
            sb.AppendLine("trackingNo: " & trackingNo)
            sb.AppendLine("campaignMngrNotes: " & campaignMngrNotes)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

            Return Nothing
        End Try
    End Function
    Public Function UpdatePledgePayoutStatus(ByVal pledgeId As Int16, ByVal payout As StripePayout) As Attempt(Of PublishStatus)
        Try
            'Instantiate variables
            Dim cs As IContentService = ApplicationContext.Current.Services.ContentService
            Dim pledge As IContent = cs.GetById(pledgeId)

            '
            pledge.SetValue(nodeProperties.transfered, True)
            pledge.SetValue(nodeProperties.transferDate, DateTime.Now)
            pledge.SetValue(nodeProperties.notes, addToNotes(pledgeId, JsonConvert.SerializeObject(payout)))


            'Save values
            Return cs.SaveAndPublishWithStatus(pledge)
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqPledges.vb : UpdatePledgePayoutStatus()")
            sb.AppendLine("pledgeId: " & pledgeId)
            sb.AppendLine("payout: " & JsonConvert.SerializeObject(payout))
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

            Return Nothing
        End Try
    End Function
    Public Function UpdatePayoutRequestNotes(ByVal transferRequestId As Integer, ByVal notes As String, ByVal isComplete As Boolean, Optional ByVal dateToProcess As Date? = Nothing) As Attempt(Of PublishStatus)
        Try
            'Instantiate variables
            Dim cs As IContentService = ApplicationContext.Current.Services.ContentService
            Dim icTransfer As IContent = cs.GetById(transferRequestId)

            '
            icTransfer.SetValue(nodeProperties.notes, addToNotes(transferRequestId, notes))
            icTransfer.SetValue(nodeProperties.isComplete, isComplete)
            If Not IsNothing(dateToProcess) Then icTransfer.SetValue(nodeProperties.dateToProcess, dateToProcess)

            'Save values
            Return cs.SaveAndPublishWithStatus(icTransfer)
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqPledges.vb : UpdateTransferRequestNotes()")
            sb.AppendLine("transferRequestId: " & transferRequestId)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

            Return Nothing
        End Try
    End Function
#End Region

#Region "Delete"
#End Region

#Region "Methods"
    'Public Function anyPendingTransfers(ByVal ipCampaign As IPublishedContent) As Boolean
    '    'Instantiate scope variables
    '    Dim hasPending As Boolean = True

    '    Try
    '        If ipCampaign.HasProperty(nodeProperties.stripeUserId) AndAlso ipCampaign.HasValue(nodeProperties.stripeUserId) Then
    '            'Instantiate variables
    '            Dim stripeRequestOptions As New StripeRequestOptions
    '            Dim balanceService = New StripeBalanceService()
    '            Dim response As StripeBalance

    '            'Obtain available and pending balance
    '            stripeRequestOptions.ApiKey = ConfigurationManager.AppSettings(Miscellaneous.StripeApiKey)
    '            stripeRequestOptions.StripeConnectAccountId = ipCampaign.GetPropertyValue(Of String)(nodeProperties.stripeUserId)
    '            response = balanceService.Get(stripeRequestOptions)

    '            'Determine if a pending balance exists.
    '            If response.Pending.Count = 0 Then
    '                hasPending = False
    '            Else
    '                If response.Pending(0).Amount = 0 Then hasPending = False
    '            End If

    '        End If
    '    Catch ex As Exception
    '        Dim sb As New StringBuilder()
    '        sb.AppendLine("linqPledges.vb : anyPendingTransfers()")
    '        sb.AppendLine("ipCampaign: " & ipCampaign.Id & " | " & ipCampaign.Name)
    '        saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

    '    End Try
    '    Return hasPending
    'End Function
    Public Function obtainAllTransfers(ByVal ipCampaign As IPublishedContent) As StripeList(Of StripeTransfer)

        Try
            If ipCampaign.HasProperty(nodeProperties.stripeUserId) AndAlso ipCampaign.HasValue(nodeProperties.stripeUserId) Then
                ''Instantiate variables
                Dim transferService As New StripeTransferService
                Dim transferListOptions As New StripeTransferListOptions
                Dim transferItems As StripeList(Of StripeTransfer)
                Dim requestOptions As New StripeRequestOptions

                'Assign variables
                transferListOptions.TransferGroup = ipCampaign.Id
                requestOptions.ApiKey = ConfigurationManager.AppSettings(Miscellaneous.StripeApiKey)

                'Obtain list of data
                transferItems = transferService.List(transferListOptions, requestOptions)

                Return transferItems
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqPledges.vb : obtainAllTransfers()")
            sb.AppendLine("ipCampaign: " & ipCampaign.Id & " | " & ipCampaign.Name)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

            Return Nothing
        End Try
    End Function
    Public Function obtainAccountDetails(ByVal ipCampaign As IPublishedContent) As StripeAccount
        Try
            If ipCampaign.HasProperty(nodeProperties.stripeUserId) AndAlso ipCampaign.HasValue(nodeProperties.stripeUserId) Then
                'Instantiate variables
                Dim accountService As New StripeAccountService
                Dim requestOptions As New StripeRequestOptions
                Dim acct As StripeAccount

                'Assign variables
                requestOptions.ApiKey = ConfigurationManager.AppSettings(Miscellaneous.StripeApiKey)

                'Obtain list of data
                acct = accountService.Get(ipCampaign.GetPropertyValue(Of String)(nodeProperties.stripeUserId), requestOptions)

                Return acct
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqPledges.vb : obtainAccountDetails()")
            sb.AppendLine("ipCampaign: " & ipCampaign.Id & " | " & ipCampaign.Name)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

            Return Nothing
        End Try
    End Function
    Public Function payoutFunds(ByVal payoutId As Integer, ByVal destinationId As String, ByVal amount As Integer) As StripePayout
        Try
            'Instantiate variables
            Dim payoutOptions As StripePayoutCreateOptions
            Dim payoutService As New StripePayoutService
            Dim payout As New StripePayout
            Dim requestOptions As New StripeRequestOptions

            'Set api key
            payoutService.ApiKey = ConfigurationManager.AppSettings(Miscellaneous.StripeApiKey)

            'Set stripe acct id to payout funds to
            ''ipCampaign.GetPropertyValue(Of String)(nodeProperties.stripeUserId), 'financialId,
            payoutOptions = New StripePayoutCreateOptions() With {
                .Currency = "usd",
                .Amount = amount'.Destination = "ba_1BXCfQJNOlo9J3OBrPkT4wzi", '"acct_1BWEDtJNOlo9J3OB",
            }

            requestOptions.StripeConnectAccountId = destinationId ' "acct_1BWEDtJNOlo9J3OB"
            requestOptions.ApiKey = ConfigurationManager.AppSettings(Miscellaneous.StripeApiKey)

            'payout funds
            payout = payoutService.Create(payoutOptions, requestOptions)

            'do we need to use this: 
            'https://stripe.com/docs/connect/payouts#webhooks

            Return payout

        Catch se As StripeException
            Dim sb As New StringBuilder()
            sb.AppendLine("linqPledges.vb : payoutFunds()- StripeException")
            sb.AppendLine("payoutId: " & payoutId)
            sb.AppendLine("destinationId: " & destinationId)
            sb.AppendLine("amount: " & amount & " ¢")
            sb.AppendLine(" ")
            sb.AppendLine("HTTP status code: " & se.HttpStatusCode)
            sb.AppendLine(se.Message)
            saveErrorMessage(getLoggedInMember, se.ToString, sb.ToString())

            'Update payout request
            UpdatePayoutRequestNotes(payoutId, sb.ToString(), False, Date.Today.AddDays(3))
            'move to failed folder
            movePayoutRequest(payoutId, siteNodes.failedPayouts)

            Return Nothing


        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqPledges.vb : payoutFunds()- Exception")
            sb.AppendLine("payoutId: " & payoutId)
            sb.AppendLine("destinationId: " & destinationId)
            sb.AppendLine("amount: " & amount & " ¢")
            sb.AppendLine(sb.ToString())
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

            'Update payout request
            UpdatePayoutRequestNotes(payoutId, sb.ToString(), False, Date.Today.AddDays(1))
            'move to failed folder
            movePayoutRequest(payoutId, siteNodes.failedPayouts)

            Return Nothing
        End Try
    End Function
    Public Sub submitFundPayouts(ByVal payoutFolder As Integer)
        'Instantiate variables
        Dim blPledges As New blPledges
        Dim ipPendingPayouts As IPublishedContent = _uHelper.Get_IPublishedContentByID(payoutFolder)

        'Loop thru each node and find any pending payouts that are ready to be processed.
        For Each ipPayout As IPublishedContent In ipPendingPayouts.Children
            Try
                If ipPayout.GetPropertyValue(Of Date)(nodeProperties.dateToProcess) <= Date.Today AndAlso
                    ipPayout.GetPropertyValue(Of Boolean)(nodeProperties.isComplete) = False Then

                    'Obtain list of all valid pledge IDs for this phase from umbraco
                    Dim lstPledges As List(Of CampaignPledge) = blPledges.obtainIncompletePledges_byPhase(ipPayout.GetPropertyValue(Of Integer)(nodeProperties.activePhase))

                    'payout funds
                    Dim payout As StripePayout = blPledges.transferFunds(ipPayout.Id, lstPledges(0).destinationId, ipPayout.GetPropertyValue(Of Integer)(nodeProperties.payoutTotal))

                    'do we need to use this: https://stripe.com/docs/connect/payouts#webhooks

                    'Update each pledge with status
                    For Each pledge As CampaignPledge In lstPledges
                        If pledge.transfered Then
                            Dim attempt As Attempt(Of PublishStatus) = blPledges.UpdatePledgePayoutStatus(pledge.pledgeId, payout) 'lstTransfers.Where(Function(x) x.SourceTransactionId = pledge.chargeId).FirstOrDefault)
                        End If
                    Next

                    'Move payout request depending on returned code.
                    'If String.IsNullOrEmpty(payout.FailureCode) Then
                    If Not IsNothing(payout) AndAlso String.IsNullOrEmpty(payout.FailureCode) Then
                        'Successful
                        '===============================
                        'Update payout request
                        UpdatePayoutRequestNotes(ipPayout.Id, JsonConvert.SerializeObject(payout), True)

                        'move to success folder
                        movePayoutRequest(ipPayout.Id, siteNodes.successfulPayouts)

                    Else
                        'Failed
                        '===============================
                        'Update payout request
                        UpdatePayoutRequestNotes(ipPayout.Id, JsonConvert.SerializeObject(payout), False, Date.Today.AddDays(1))

                            'move to failed folder
                            movePayoutRequest(ipPayout.Id, siteNodes.failedPayouts)
                        End If
                    End If
            Catch ex As Exception
                Dim sb As New StringBuilder()
                sb.AppendLine("linqPledges.vb : submitFundPayouts()")
                sb.AppendLine("Failed to payout funds:")
                sb.AppendLine("ipPayout:" & ipPayout.Id & " | " & ipPayout.Name)
                saveErrorMessage("", ex.ToString, sb.ToString())
            End Try
        Next
    End Sub
    Private Sub movePayoutRequest(ByVal payoutId As Integer, ByVal parentId As Integer)
        Try
            'Instantiate variables
            Dim cs As IContentService = ApplicationContext.Current.Services.ContentService
            Dim icPayout As IContent = cs.GetById(payoutId)

            'Move to new parent
            cs.Move(icPayout, parentId)

            cs.SaveAndPublishWithStatus(icPayout)

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("linqPledges.vb : movePayoutRequest()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
    End Sub
#End Region

End Class



'Public Function transferFunds(ByVal financialId As String, ByVal amount As Integer) As StripeTransfer
'    Try
'        'Instantiate variables
'        Dim transferService As New StripeTransferService
'        Dim transferOptions As StripeTransferCreateOptions
'        Dim stripeTransfer As StripeTransfer

'        'Assign variables
'        transferService.ApiKey = ConfigurationManager.AppSettings(Miscellaneous.StripeApiKey)
'        transferOptions = New StripeTransferCreateOptions() With {
'            .Currency = "usd",
'            .Destination = financialId,
'            .Amount = amount
'        }

'        'transfer funds
'        stripeTransfer = transferService.Create(transferOptions)

'        Return stripeTransfer

'    Catch ex As Exception
'        Dim sb As New StringBuilder()
'        sb.AppendLine("linqPledges.vb : transferFunds()")
'        sb.AppendLine("financialId: " & financialId)
'        sb.AppendLine("amount: " & amount)
'        saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

'        Return Nothing
'    End Try
'End Function