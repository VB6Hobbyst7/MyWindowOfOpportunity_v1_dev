Imports System.Data
Imports Common
Imports umbraco
Imports umbraco.Core
Imports umbraco.Core.Models
Imports umbraco.Core.Services
Imports umbraco.Examine.Linq
Imports umbraco.Core.Publishing
Imports umbraco.Web
Imports Newtonsoft.Json

Public Class linqTeams

#Region "Properties"
    Private linq2Db As linq2SqlDataContext = New linq2SqlDataContext(ConfigurationManager.ConnectionStrings("umbracoDbDSN").ToString)
    Private indexTeams As Index(Of Team)
    Private indexAlphaFolders As Index(Of AlphaFolder)
    Dim _uHelper As Uhelper = New Uhelper()
    'Private blMembers As blMembers
#End Region

#Region "Selects"
    Public Function getRootFolder_byName(ByVal _name As Char) As Integer
        Try
            indexAlphaFolders = New Index(Of AlphaFolder)()
            'Return (From a In indexAlphaFolders.AsEnumerable Where a.Name = _name Select a.Id).FirstOrDefault
            Return (From a In indexAlphaFolders.AsEnumerable Where a.Name.Equals(_name, StringComparison.OrdinalIgnoreCase) Select a.Id).FirstOrDefault

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\linqQueries\linqTeams.vb : getRootFolder_byName()")
            sb.AppendLine("_name" & _name)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return Nothing
        End Try
    End Function
    Public Function getAllRootFolders() As List(Of AlphaFolder)
        Try

            indexAlphaFolders = New Index(Of AlphaFolder)()
            Return (From a In indexAlphaFolders).ToList

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\linqQueries\linqTeams.vb : getAllRootFolders()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return Nothing
        End Try
    End Function
    Public Function getTeamContent_byId(ByVal _teamNodeId As Integer) As BusinessReturn
        'Instantiate variables
        Dim teamSummary As TeamSummary = New TeamSummary
        Dim ipTeamNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(_teamNodeId)
        Dim BusinessReturn As BusinessReturn = New BusinessReturn

        Try
            If Not IsNothing(ipTeamNode) Then
                'Get team content
                teamSummary.teamId = _teamNodeId
                teamSummary.teamName = ipTeamNode.Name
                If ipTeamNode.HasValue(nodeProperties.whoAreWe) Then teamSummary.whoAreWe = ipTeamNode.GetPropertyValue(Of String)(nodeProperties.whoAreWe)
                If ipTeamNode.HasValue(nodeProperties.facebookUrl) Then teamSummary.facebookUrl = ipTeamNode.GetPropertyValue(Of String)(nodeProperties.facebookUrl)
                If ipTeamNode.HasValue(nodeProperties.twitterUrl) Then teamSummary.twitterUrl = ipTeamNode.GetPropertyValue(Of String)(nodeProperties.twitterUrl)
                If ipTeamNode.HasValue(nodeProperties.supportEmail) Then teamSummary.supportEmailUrl = ipTeamNode.GetPropertyValue(Of String)(nodeProperties.supportEmail)
                If ipTeamNode.HasValue(nodeProperties.linkedInUrl) Then teamSummary.linkedInUrl = ipTeamNode.GetPropertyValue(Of String)(nodeProperties.linkedInUrl)
                'Get image properties
                If ipTeamNode.HasValue(nodeProperties.teamImage) Then
                    teamSummary.teamImageId = ipTeamNode.GetPropertyValue(Of Integer)(nodeProperties.teamImage)
                    teamSummary.teamImageUrl = getMediaURL(teamSummary.teamImageId)
                    teamSummary.teamImageName = getMediaName(teamSummary.teamImageId)
                End If


                '=============================================================================================================
                '       OBTAIN TEAM MEMBERS
                '=============================================================================================================
                '
                Dim lstCampaigns As New List(Of IPublishedContent)
                Dim blMembers As New blMembers

                'Obtain all team administrators
                For Each id As String In ipTeamNode.GetPropertyValue(Of String)(nodeProperties.teamAdministrators).Split(",")
                    If Not String.IsNullOrWhiteSpace(id) AndAlso CInt(id) > 0 Then

                        Dim bReturnMember As BusinessReturn = blMembers.getMemberDemographics_byId(CInt(id), True)
                        If bReturnMember.isValid AndAlso Not IsNothing(bReturnMember.DataContainer) Then
                            Dim newMember As Member = bReturnMember.DataContainer(0)

                            'Add data to record
                            newMember.MembershipProperties.userId = CInt(id)
                            newMember.isTeamAdmin = True

                            'Add member id to list
                            teamSummary.lstMembers.Add(newMember)
                        End If
                    End If

                Next

                'Obtain list of all campaigns under the team.
                For Each ipCampaign As IPublishedContent In ipTeamNode.Children
                    lstCampaigns.Add(ipCampaign)
                Next

                'Loop thru each campaign to obtain all members
                For Each campaign As IPublishedContent In lstCampaigns
                    'Obtain campaign's member folder
                    Dim campaignMemberNode As IPublishedContent = blMembers.getCampaignMemberFolder(campaign)

                    If Not IsNothing(campaignMemberNode) Then
                        'Loop thru each member node
                        For Each ipMember As IPublishedContent In campaignMemberNode.Children
                            If Not IsNothing(ipMember) Then

                                'Obtain member's ID
                                Dim memberId As Integer = ipMember.GetPropertyValue(Of Integer)(nodeProperties.campaignMember)

                                'Update existing record or create new one.
                                If Not teamSummary.lstMembers.Exists(Function(x) x.MembershipProperties.userId = memberId) Then
                                    Dim bReturnMember As BusinessReturn = blMembers.getMemberDemographics_byId(memberId, True)
                                    If bReturnMember.isValid Then
                                        Dim newMember As Member = bReturnMember.DataContainer(0)

                                        'Add data to record
                                        newMember.MembershipProperties.userId = memberId

                                        'Add member id to list
                                        teamSummary.lstMembers.Add(newMember)
                                    End If
                                End If
                            End If
                        Next
                    End If
                Next

                'Add data to return msg
                BusinessReturn.DataContainer.Add(teamSummary)
            End If

            Return BusinessReturn
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\linqQueries\linqTeams.vb : getTeamContent_byId()")
            sb.AppendLine("_teamNodeId:" & _teamNodeId)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            BusinessReturn.ExceptionMessage = ex.ToString
            Return BusinessReturn
        End Try
    End Function
    Public Function getTeamAdministratorsContent_byId(ByVal _teamNodeId As Integer) As TeamSummary
        'Instantiate variables
        Dim teamSummary As TeamSummary = New TeamSummary
        Dim thisNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(_teamNodeId)


        Try
            If Not IsNothing(thisNode) Then
                'Get team content
                teamSummary.teamId = _teamNodeId
                teamSummary.teamName = thisNode.Name
                If thisNode.HasProperty(nodeProperties.whoAreWe) Then teamSummary.whoAreWe = thisNode.GetPropertyValue(Of String)(nodeProperties.whoAreWe)
                If thisNode.HasProperty(nodeProperties.facebookUrl) Then teamSummary.facebookUrl = thisNode.GetPropertyValue(Of String)(nodeProperties.facebookUrl)
                If thisNode.HasProperty(nodeProperties.twitterUrl) Then teamSummary.twitterUrl = thisNode.GetPropertyValue(Of String)(nodeProperties.twitterUrl)
                If thisNode.HasProperty(nodeProperties.supportEmailUrl) Then teamSummary.supportEmailUrl = thisNode.GetPropertyValue(Of String)(nodeProperties.supportEmailUrl)
                If thisNode.HasProperty(nodeProperties.linkedInUrl) Then teamSummary.linkedInUrl = thisNode.GetPropertyValue(Of String)(nodeProperties.linkedInUrl)
                'Get image properties
                If thisNode.HasProperty(nodeProperties.teamImage) Then
                    teamSummary.teamImageId = thisNode.GetPropertyValue(Of Integer)(nodeProperties.teamImage)
                    teamSummary.teamImageUrl = getMediaURL(teamSummary.teamImageId)
                    teamSummary.teamImageName = getMediaName(teamSummary.teamImageId)
                End If
                'teamSummary.mediaFolderId =

                'Add data to return msg

            End If

            Return teamSummary
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\linqQueries\linqTeams.vb : getTeamAdministratorsContent_byId()")
            sb.AppendLine("_teamNodeId:" & _teamNodeId)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'BusinessReturn.ExceptionMessage = ex.ToString
            Return teamSummary
        End Try
    End Function
#End Region

#Region "Inserts"
    Public Function CreateTeam(ByVal _alphaFolderId As Integer, ByVal _teamName As String, ByVal _userId As Integer) As Integer
        Try
            'Create a team IPublishedContent
            Dim cs As IContentService = ApplicationContext.Current.Services.ContentService
            Dim alphaFolder As IContent = cs.GetById(_alphaFolderId)
            Dim team As IContent = cs.CreateContentWithIdentity(_teamName, alphaFolder, docTypes.Team)
            'Set values
            team.SetValue(nodeProperties.teamAdministrators, _userId)
            'Save values
            cs.SaveAndPublishWithStatus(team)
            'Return new IPublishedContent's Id
            Return team.Id

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\linqQueries\linqTeams.vb : CreateTeam()")
            sb.AppendLine("_alphaFolderId:" & _alphaFolderId)
            sb.AppendLine("_teamName:" & _teamName)
            sb.AppendLine("_userId" & _userId)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return 0
        End Try
    End Function
    Public Function AddTeamAdministratorToCampaign_byMemberId(ByVal _memberId As Integer, ByVal _campaignId As Integer) As BusinessReturn
        'Instantiate variables
        Dim BusinessReturn As New BusinessReturn()
        Dim Attempt As Attempt(Of PublishStatus) = New Attempt(Of PublishStatus)

        Try
            'Instantiate variables
            Dim blMembers As blMembers = New blMembers
            Dim teamNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(_campaignId).Parent
            Dim teamMembers As List(Of String)
            Dim cs As IContentService = ApplicationContext.Current.Services.ContentService
            Dim teamContent As IContent = cs.GetById(teamNode.Id)


            'Obtain list of all team members
            If teamNode.HasProperty(nodeProperties.teamAdministrators) Then
                'create list of team member IDs
                teamMembers = teamNode.GetPropertyValue(Of String)(nodeProperties.teamAdministrators).Split(",").ToList

                'If team does not contain member, add to list.
                If Not teamMembers.Contains(_memberId) Then
                    'Add member to list
                    teamMembers.Add(_memberId)
                    teamContent.SetValue(nodeProperties.teamAdministrators, String.Join(",", teamMembers.ToArray()))
                    'Save values
                    Attempt = cs.SaveAndPublishWithStatus(teamContent)
                End If
            End If

            '
            If (Attempt.Success) Then
                'Remove all member entries from child campaign member folders
                '============================================================
                'Loop thru campaign nodes
                For Each campaign As IPublishedContent In teamNode.Children
                    'Loop thru folders and locate the campaign members folder
                    For Each folder As IPublishedContent In campaign.Children
                        If folder.DocumentTypeAlias = docTypes.campaignMembers Then
                            'Loop thru each member and remove if id matches memberId
                            For Each memberNode As IPublishedContent In folder.Children
                                Dim memberId As Integer = memberNode.GetPropertyValue(Of Integer)(nodeProperties.campaignMember)
                                If memberId = _memberId Then
                                    'Delete IPublishedContent
                                    Dim _memberNode As IContent = cs.GetById(memberNode.Id)
                                    cs.Delete(_memberNode) ', blMembers.GetCurrentMemberId())

                                    'Dim _memberNode As DocumentExtensions 
                                End If
                            Next

                            'exit loop
                            Exit For
                        End If
                    Next
                Next

                Return BusinessReturn

            Else
                BusinessReturn.ExceptionMessage = Attempt.Exception.ToString
                Return BusinessReturn
            End If




        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\linqQueries\linqTeams.vb : AddTeamAdministratorToCampaign_byMemberId()")
            sb.AppendLine("_memberId:" & _memberId)
            sb.AppendLine("_campaignId:" & _campaignId)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            BusinessReturn.ExceptionMessage = ex.ToString
            Return BusinessReturn
        End Try
    End Function
    Public Function AddTeamAdministratorToTeam_byMemberId(ByVal _memberId As Integer, ByVal _teamId As Integer) As BusinessReturn
        'Instantiate variables
        Dim BusinessReturn As New BusinessReturn
        Dim Attempt As Attempt(Of PublishStatus) = New Attempt(Of PublishStatus)

        Try
            'Instantiate variables
            Dim blMembers As blMembers = New blMembers
            Dim teamNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(_teamId)
            Dim teamMembers As List(Of String)
            Dim cs As IContentService = ApplicationContext.Current.Services.ContentService
            Dim teamContent As IContent = cs.GetById(_teamId)


            'Obtain list of all team members
            If teamNode.HasProperty(nodeProperties.teamAdministrators) Then
                'create list of team member IDs
                teamMembers = teamNode.GetPropertyValue(Of String)(nodeProperties.teamAdministrators).Split(",").ToList

                'If team does not contain member, add to list.
                If Not teamMembers.Contains(_memberId) Then
                    'Add member to list
                    teamMembers.Add(_memberId)
                    teamContent.SetValue(nodeProperties.teamAdministrators, String.Join(",", teamMembers.ToArray()))
                    'Save values
                    Attempt = cs.SaveAndPublishWithStatus(teamContent)
                End If
            End If

            '
            If (Attempt.Success) Then
                'Remove all member entries from child campaign member folders
                '============================================================
                'Loop thru campaign nodes
                For Each campaign As IPublishedContent In teamNode.Children
                    'Loop thru folders and locate the campaign members folder
                    For Each folder As IPublishedContent In campaign.Children
                        If folder.DocumentTypeAlias = docTypes.campaignMembers Then
                            'Loop thru each member and remove if id matches memberId
                            For Each memberNode As IPublishedContent In folder.Children
                                Dim memberId As Integer = memberNode.GetPropertyValue(Of Integer)(nodeProperties.campaignMember)
                                If memberId = _memberId Then
                                    'Delete IPublishedContent
                                    Dim _memberNode As IContent = cs.GetById(memberNode.Id)
                                    cs.Delete(_memberNode) ', blMembers.GetCurrentMemberId())

                                    'Dim _memberNode As DocumentExtensions 
                                End If
                            Next

                            'exit loop
                            Exit For
                        End If
                    Next
                Next

                Return BusinessReturn

            Else
                BusinessReturn.ExceptionMessage = "_memberId: " & _memberId.ToString & " - _teamId: " & _teamId 'Attempt.Exception.ToString
                Return BusinessReturn
            End If




        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\linqQueries\linqTeams.vb : AddTeamAdministratorToTeam_byMemberId()")
            sb.AppendLine("_memberId:" & _memberId)
            sb.AppendLine("_teamId:" & _teamId)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            BusinessReturn.ExceptionMessage = ex.ToString
            Return BusinessReturn
        End Try
    End Function
    Public Function AddCampaignMember_byMemberId(ByVal _memberId As Integer, ByVal _campaignId As Integer, ByVal _campaignManager As String) As BusinessReturn
        'Instantiate variables
        Dim BusinessReturn As New BusinessReturn()
        Dim Attempt As Attempt(Of PublishStatus) = New Attempt(Of PublishStatus)
        Dim campaignManager As Boolean = False
        'Dim sb As StringBuilder = New StringBuilder

        'Determine if member is a manager
        If _campaignManager = memberRole.CampaignAdministrator Then campaignManager = True

        Try
            'Instantiate variables
            Dim blMembers As blMembers = New blMembers
            Dim cs As IContentService = ApplicationContext.Current.Services.ContentService
            Dim campaignNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(_campaignId)
            Dim teamNode As IPublishedContent = campaignNode.Parent
            Dim teamContent As IContent = cs.GetById(campaignNode.Parent.Id)
            Dim teamMembers As List(Of String)
            Dim isFound As Boolean = False
            Dim memberFolderExists As Boolean = False

            'sb.AppendLine("<br />Instantiated")

            'Check if campaign member folder exists.  if not, create it.
            For Each folder As IPublishedContent In campaignNode.Children
                If folder.DocumentTypeAlias = docTypes.campaignMembers Then
                    memberFolderExists = True
                    'sb.AppendLine("<br />member folder exists")
                    Exit For
                End If
            Next
            If memberFolderExists = False Then
                Dim folderId As Short = blMembers.CreateCampaignMembersFolder(_campaignId)
                'sb.AppendLine("<br />Created member folder: ID: " & folderId)
            End If


            'Loop thru folders under campaign and locate the campaign members folder
            For Each folder As IPublishedContent In campaignNode.Children
                If folder.DocumentTypeAlias = docTypes.campaignMembers Then
                    'sb.AppendLine("<br />found member folder")

                    'Loop thru each member IPublishedContent and look for a match to member Id
                    For Each memberNode As IPublishedContent In folder.Children
                        Dim memberId As Integer = memberNode.GetPropertyValue(Of Integer)(nodeProperties.campaignMember)
                        If memberId = _memberId Then
                            '
                            Dim icMember As IContent = cs.GetById(memberNode.Id)
                            'Update member
                            icMember.SetValue(nodeProperties.campaignManager, campaignManager)
                            cs.SaveAndPublishWithStatus(icMember)
                            'Set boolean and exit
                            isFound = True
                            'sb.AppendLine("<br />found and updated member")
                            Exit For
                        End If
                    Next

                    'If false, add new member IPublishedContent.
                    If Not isFound Then
                        'sb.AppendLine("<br />creating new member")
                        '
                        Dim newMember As IContent = cs.CreateContent(blMembers.getMemberName_byId(_memberId), cs.GetById(folder.Id), docTypes.campaignMember)
                        '
                        newMember.SetValue(nodeProperties.campaignManager, campaignManager)
                        newMember.SetValue(nodeProperties.campaignMember, _memberId)
                        '
                        Attempt = cs.SaveAndPublishWithStatus(newMember)
                        'sb.AppendLine("<br />member created. Success: " & Attempt.Success.ToString)
                    End If

                    'exit loop
                    Exit For
                End If
            Next

            '
            If (Attempt.Success) Then
                'REMOVE FROM TEAM MEMBER PAGE
                '===================================
                'sb.AppendLine("<br />Begin removing from team member page")
                'Obtain list of all team members
                If teamNode.HasProperty(nodeProperties.teamAdministrators) Then
                    'create list of team member IDs
                    teamMembers = teamNode.GetPropertyValue(Of String)(nodeProperties.teamAdministrators).Split(",").ToList

                    'If team contains member, remove from  list.
                    If teamMembers.Contains(_memberId) Then
                        'Remove from list and republish
                        teamMembers.Remove(_memberId)
                        'sb.AppendLine("<br />Removing from team list")
                        Dim iTeamNode As IContent = cs.GetById(teamNode.Id)
                        iTeamNode.SetValue(nodeProperties.teamAdministrators, String.Join(",", teamMembers))
                        Attempt = cs.SaveAndPublishWithStatus(iTeamNode)
                        'sb.AppendLine("<br />Updated team list")
                    End If
                End If

                'BusinessReturn.ExceptionMessage = "<br>" & sb.ToString & "<br>"
                Return BusinessReturn
            Else
                BusinessReturn.ExceptionMessage = Attempt.Success
                'BusinessReturn.ExceptionMessage = "<br>" & sb.ToString & "<br>"
                Return BusinessReturn
            End If


        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\linqQueries\linqTeams.vb : AddCampaignMember_byMemberId()")
            sb.AppendLine("_memberId:" & _memberId)
            sb.AppendLine("_campaignId:" & _campaignId)
            sb.AppendLine("_campaignManager:" & _campaignManager)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            BusinessReturn.ExceptionMessage = "catch error: " & ex.ToString '& "<br>PATH: " & sb.ToString & "<br>"
            Return BusinessReturn
        End Try
    End Function
#End Region

#Region "Updates"
    Public Function UpdateTeamMember(ByVal _memberId As Int32, ByVal _campaignId As Int32, ByVal _campaignManager As String) As BusinessReturn
        'Instantiate variables
        Dim BusinessReturn As New BusinessReturn()

        Try
            'Instantiate variables
            Dim cs As IContentService = ApplicationContext.Current.Services.ContentService
            Dim campaignMember As IContent
            Dim updateResponse As Attempt(Of PublishStatus)
            Dim campaignManager As Boolean = False

            'Obtain member IPublishedContent id within campaign
            '=====================================
            'Loop thru folders under campaign and locate the campaign members folder
            For Each folder As IPublishedContent In _uHelper.Get_IPublishedContentByID(_campaignId).Children
                If folder.DocumentTypeAlias = docTypes.campaignMembers Then
                    'Loop thru each member IPublishedContent and look for a match to member Id
                    For Each memberNode As IPublishedContent In folder.Children
                        Dim memberId As Integer = memberNode.GetPropertyValue(Of Integer)(nodeProperties.campaignMember)
                        If memberId = _memberId Then
                            'Obtain icontent from IPublishedContent id
                            campaignMember = cs.GetById(memberNode.Id)

                            Exit For
                        End If
                    Next
                    Exit For
                End If
            Next


            If IsNothing(campaignMember) Then
                'Invalid member id
                BusinessReturn.ExceptionMessage = "Campaign member was not found in campaign."
                Return BusinessReturn
            Else
                'Determine if member is a manager
                If _campaignManager = memberRole.CampaignAdministrator Then campaignManager = True

                'Obtain IPublishedContent within campaign members associated with this member (nodeId)

                'Set values 
                campaignMember.SetValue(nodeProperties.campaignManager, campaignManager)

                'Save values
                updateResponse = cs.SaveAndPublishWithStatus(campaignMember)
                If (updateResponse.Success) Then
                    BusinessReturn.ReturnMessage = campaignMember.Id
                    Return BusinessReturn
                Else
                    BusinessReturn.ExceptionMessage = updateResponse.Exception.ToString
                    Return BusinessReturn
                End If
            End If

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\linqQueries\linqTeams.vb : UpdateTeamMember()")
            sb.AppendLine("_memberId:" & _memberId)
            sb.AppendLine("_campaignId:" & _campaignId)
            sb.AppendLine("_campaignManager:" & _campaignManager)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            BusinessReturn.ExceptionMessage = ex.ToString
            Return BusinessReturn
        End Try
    End Function
    Public Function SaveTeamImage(ByVal _nodeId As Int16, ByVal _mediaId As Integer) As Attempt(Of PublishStatus)
        Try
            'Instantiate variables
            Dim cs As IContentService = ApplicationContext.Current.Services.ContentService
            Dim campaign As IContent = cs.GetById(_nodeId)
            'Set values
            campaign.SetValue(nodeProperties.teamImage, _mediaId)
            'Save values
            Return cs.SaveAndPublishWithStatus(campaign)

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\linqQueries\linqTeams.vb : SaveTeamImage()")
            sb.AppendLine("_nodeId:" & _nodeId)
            sb.AppendLine("_mediaId:" & _mediaId)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return Nothing
        End Try
    End Function
    Public Function SaveTeamContent(ByVal _TeamSummary As TeamSummary) As Attempt(Of PublishStatus)
        Try
            'Instantiate variables
            Dim cs As IContentService = ApplicationContext.Current.Services.ContentService
            Dim team As IContent = cs.GetById(_TeamSummary.teamId)
            'Set values
            team.SetValue(nodeProperties.whoAreWe, _TeamSummary.whoAreWe)
            'team.SetValue(nodeProperties.facebookUrl, _TeamSummary.facebookUrl)
            'team.SetValue(nodeProperties.twitterUrl, _TeamSummary.twitterUrl)
            ''team.SetValue(nodeProperties.googlePlusUrl, _TeamSummary.googlePlusUrl)
            'team.SetValue(nodeProperties.supportEmail, _TeamSummary.supportEmailUrl)
            'team.SetValue(nodeProperties.linkedInUrl, _TeamSummary.linkedInUrl)
            If _TeamSummary.removeTeamImage Then team.SetValue(nodeProperties.teamImage, String.Empty)

            'Save values
            Return cs.SaveAndPublishWithStatus(team)

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\linqQueries\linqTeams.vb : SaveTeamContent()")
            sb.AppendLine("_TeamSummary:" & JsonConvert.SerializeObject(_TeamSummary))
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return Nothing
        End Try
    End Function
#End Region

#Region "Delete"
    Public Function RemoveFromTeam_byMemberId(ByVal _memberId As Integer, ByVal _campaignId As Integer, Optional ByVal _fromTeamPage As Boolean = False) As BusinessReturn
        'Instantiate variables
        Dim BusinessReturn As BusinessReturn = New BusinessReturn()
        Dim Attempt As Attempt(Of PublishStatus) = New Attempt(Of PublishStatus)

        Try
            'Instantiate variables
            Dim blMembers As blMembers = New blMembers
            Dim teamNode As IPublishedContent
            Dim teamMembers As List(Of String)
            Dim cs As IContentService = ApplicationContext.Current.Services.ContentService
            Dim teamContent As IContent


            If _fromTeamPage Then
                'campaignNode = New IPublishedContent(_campaignId)
                teamNode = _uHelper.Get_IPublishedContentByID(_campaignId)
                teamContent = cs.GetById(_campaignId)
            Else
                Dim campaignNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(_campaignId)
                teamNode = campaignNode.Parent
                teamContent = cs.GetById(campaignNode.Parent.Id)
            End If



            'Obtain list of all team members
            If teamNode.HasProperty(nodeProperties.teamAdministrators) Then
                'create list of team member IDs
                teamMembers = teamNode.GetPropertyValue(Of String)(nodeProperties.teamAdministrators).Split(",").ToList

                'If team does not contain member, add to list.
                If teamMembers.Contains(_memberId) Then
                    'Add member to list
                    teamMembers.Remove(_memberId)
                    teamContent.SetValue(nodeProperties.teamAdministrators, String.Join(",", teamMembers.ToArray()))
                    'Save values
                    Attempt = cs.SaveAndPublishWithStatus(teamContent)
                End If
            End If

            'Return result
            If (Attempt.Success) Then
                Return BusinessReturn
            Else
                BusinessReturn.ExceptionMessage = Attempt.Exception.ToString
                Return BusinessReturn
            End If

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\linqQueries\linqTeams.vb : RemoveFromTeam_byMemberId()")
            sb.AppendLine("_memberId:" & _memberId)
            sb.AppendLine("_campaignId:" & _campaignId)
            sb.AppendLine("_fromTeamPage:" & _fromTeamPage)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            BusinessReturn.ExceptionMessage = ex.ToString
            Return BusinessReturn
        End Try
    End Function
    Public Function RemoveFromCampaign_byMemberId(ByVal _memberId As Integer, ByVal _campaignId As Integer) As BusinessReturn
        'Instantiate variables
        Dim BusinessReturn As New BusinessReturn()
        'Dim Attempt As Attempt(Of PublishStatus) = New Attempt(Of PublishStatus)

        Try
            'Instantiate variables
            Dim blMembers As blMembers = New blMembers
            Dim campaignNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(_campaignId)
            Dim cs As IContentService = ApplicationContext.Current.Services.ContentService
            Dim isValid As Boolean = False

            'Loop thru folders under campaign and locate the campaign members folder
            For Each folder As IPublishedContent In campaignNode.Children
                If folder.DocumentTypeAlias = docTypes.campaignMembers Then
                    'Loop thru each member IPublishedContent and look for a match to member Id
                    For Each memberNode As IPublishedContent In folder.Children
                        'Obtain memberId from property.
                        Dim memberId As Integer = memberNode.GetPropertyValue(Of Integer)(nodeProperties.campaignMember)

                        If memberId = _memberId Then
                            'Delete IPublishedContent
                            Dim _memberNode As IContent = cs.GetById(memberNode.Id)
                            cs.Delete(_memberNode) ', blMembers.GetCurrentMemberId())
                            'Mark return as valid.
                            isValid = True
                            Exit For
                        End If

                    Next
                    Exit For
                End If
            Next

            'Return result
            If (isValid) Then
                Return BusinessReturn
            Else
                BusinessReturn.ExceptionMessage = "We apologize, but we are unable to remove the member from this campaign."
                Return BusinessReturn
            End If

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\linqQueries\linqTeams.vb : RemoveFromCampaign_byMemberId()")
            sb.AppendLine("_memberId:" & _memberId)
            sb.AppendLine("_campaignId:" & _campaignId)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            BusinessReturn.ExceptionMessage = ex.ToString
            Return BusinessReturn
        End Try
    End Function
#End Region

#Region "Methods"
    Public Function doesTeamExist_byName(ByVal _teamName As String) As Boolean
        Try
            'Return if exists
            indexTeams = New Index(Of Team)()
            Return (From a In indexTeams Where a.Name.Trim.Equals(_teamName, StringComparison.OrdinalIgnoreCase) Select a).ToList.Any()

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\linqQueries\linqTeams.vb : doesTeamExist_byName()")
            sb.AppendLine("_userId:" & _teamName)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return Nothing
        End Try
    End Function
    Public Function isTeamAdministrator_byUserId(ByVal _userId As Integer, ByVal _teamNodeId As Integer) As Boolean
        Dim isAdmin As Boolean = False

        Try
            'Instantiate variables
            Dim teamNodeId As IPublishedContent = _uHelper.Get_IPublishedContentByID(_teamNodeId)

            'Check if member is a team administrator
            If teamNodeId.HasProperty(nodeProperties.teamAdministrators) Then
                Dim members As String() = teamNodeId.GetPropertyValue(Of String)(nodeProperties.teamAdministrators).Split(",")
                If members.Contains(_userId.ToString) Then
                    isAdmin = True
                End If
            End If

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\linqQueries\linqTeams.vb : isTeamAdministrator_byUserId()")
            sb.AppendLine("_userId:" & _userId)
            sb.AppendLine("_teamNodeId:" & _teamNodeId)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try

        Return isAdmin
    End Function
#End Region

End Class