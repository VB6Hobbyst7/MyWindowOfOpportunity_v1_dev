Imports System.Data
Imports Common
Imports umbraco.Core
Imports umbraco.Core.Models
Imports umbraco.Core.Publishing
Imports System.Xml.XPath
Imports umbraco.Web
Imports Newtonsoft.Json

Public Class blTeams

#Region "Properties"
    Private linqTeams As linqTeams = New linqTeams
    Private _uHelper As Uhelper = New Uhelper()
    Private Structure imgList
        Const mediaId As String = "mediaId"
        Const imgUrl As String = "imgUrl"
        Const imgThumb As String = "imgThumb"
        Const imgName As String = "imgName"
        Const cropTypes As String = "cropTypes"
    End Structure
#End Region


#Region "Selects"
    Public Function getRootFolder_byName(ByVal _name As Char) As Integer?
        Return linqTeams.getRootFolder_byName(_name)
    End Function
    Public Function getAllRootFolders() As List(Of AlphaFolder)
        Return linqTeams.getAllRootFolders()
    End Function
    Public Function selectImages_byTeamId(ByVal _thisNode As IPublishedContent, Optional ByVal crop As String = "") As BusinessReturn ' DataTable
        'Create Validation return
        Dim ValidationReturn As BusinessReturn = New BusinessReturn
        Try
            'Obtain image crops
            Dim results As ListItemCollection = selectImageCrops()
            Dim cropTypes As String = "{crops:" & results(0).Value & "}"
            'Note: to parse the json result, we need to add a heading parameter.  In this case, we added {crops}
            'Example: "{crops:[ { ""width"": 813, ""height"": 225, ""Alias"": ""Campaign Featured Image"" }, { ""width"": 386, ""height"": 316, ""Alias"": ""Reward Image"" } ]}"

            ''Check if IPublishedContent has a mediafolder assigned to it. If no, create folder in media
            'If Not _thisNode.HasProperty(nodeProperties.mediaFolder) Then
            '    ValidationReturn = createCampaignsMediaFolder(_thisNode.Id)
            '    If Not ValidationReturn.isValid Then Return ValidationReturn
            'End If

            'Create new datatable to store media info
            Dim dt As DataTable = createTable_ImageList()

            'Get media folder Id
            Dim mediaFolderId As Integer = _thisNode.GetPropertyValue(Of Integer)(nodeProperties.mediaFolder)
            'Dim tempMedia As IMedia
            'Loop thru all child media elements and obtain all images
            Dim mediaService = ApplicationContext.Current.Services.MediaService
            For Each media As IMedia In mediaService.GetChildren(mediaFolderId)
                'tempMedia = media
                If media.ContentType.Alias = Constants.Conventions.MediaTypes.Image Then

                    'Add data to table
                    Dim dr As DataRow = dt.NewRow
                    dr(imgList.mediaId) = media.Id
                    dr(imgList.imgUrl) = getMediaURL(media.Id, crop)
                    'dr(imgList.imgThumb) = getMediaURL(media.Id).Replace(".", "_big-thumb.")
                    dr(imgList.imgName) = media.Name
                    dr(imgList.cropTypes) = cropTypes
                    dt.Rows.Add(dr)
                End If
            Next

            'Return dt
            ValidationReturn.DataContainer.Add(dt)

            'ValidationReturn.ExceptionMessage = tempMedia.ContentType.Alias
            Return ValidationReturn

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\BusinessLayer\blTeams.vb : selectImages_byTeamId()")
            sb.AppendLine("_thisNode:" & _thisNode.ToString())
            sb.AppendLine("crop:" & crop)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Return w/ exception msg.
            ValidationReturn.ExceptionMessage = ex.Message
            Return ValidationReturn
        End Try
    End Function
    Public Function getTeamContent_byId(ByVal _teamNodeId As Integer) As BusinessReturn
        Return linqTeams.getTeamContent_byId(_teamNodeId)
    End Function
    Public Function getTeamAdministratorsContent_byId(ByVal _teamNodeId As Integer) As TeamSummary
        Return linqTeams.getTeamAdministratorsContent_byId(_teamNodeId)
    End Function
#End Region

#Region "Insert"
    Public Function CreateTeam(ByVal _alphaFolderId As Integer, ByVal _teamName As String, ByVal _userId As Integer) As Integer
        Return linqTeams.CreateTeam(_alphaFolderId, _teamName, _userId)
    End Function
#End Region

#Region "Updates"
    Public Function UpdateTeamMember(ByVal _memberId As Int32, ByVal _campaignId As Int32, ByVal _campaignManager As String) As BusinessReturn
        Return linqTeams.UpdateTeamMember(_memberId, _campaignId, _campaignManager)
    End Function
    Public Function AddTeamAdministratorToCampaign_byMemberId(ByVal _memberId As Integer, ByVal _campaignId As Integer) As BusinessReturn
        Return linqTeams.AddTeamAdministratorToCampaign_byMemberId(_memberId, _campaignId)
    End Function
    Public Function AddTeamAdministratorToTeam_byMemberId(ByVal _memberId As Integer, ByVal _teamId As Integer) As BusinessReturn
        Return linqTeams.AddTeamAdministratorToTeam_byMemberId(_memberId, _teamId)
    End Function
    Public Function AddCampaignMember_byMemberId(ByVal _memberId As Integer, ByVal _campaignId As Integer, ByVal _campaignManager As String) As BusinessReturn
        Return linqTeams.AddCampaignMember_byMemberId(_memberId, _campaignId, _campaignManager)
    End Function
    Public Function SaveTeamImage(ByVal _nodeId As Int16, ByVal _mediaId As Integer) As Attempt(Of PublishStatus)
        Return linqTeams.SaveTeamImage(_nodeId, _mediaId)
    End Function
    Public Function SaveTeamContent(ByVal _teamNodeId As Integer, ByVal _whoAreWe As String, ByVal _removeTeamImg As Boolean) As Attempt(Of PublishStatus)
        Try
            Dim teamSummary As TeamSummary = New TeamSummary With {
            .teamId = _teamNodeId,
            .whoAreWe = _whoAreWe,
            .removeTeamImage = _removeTeamImg
        }
            Return linqTeams.SaveTeamContent(teamSummary)

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\BusinessLayer\blTeams.vb : SaveTeamContent()")
            sb.AppendLine("_teamNodeId:" & _teamNodeId)
            sb.AppendLine("_whoAreWe:" & _whoAreWe)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return Nothing
        End Try
    End Function
#End Region

#Region "Delete"
    Public Function RemoveFromCampaign_byMemberId(ByVal _memberId As Integer, ByVal _campaignId As Integer, ByVal _memberRole As String) As BusinessReturn
        Try
            If _memberRole = memberRole.TeamAdministrator Then
                Return linqTeams.RemoveFromTeam_byMemberId(_memberId, _campaignId)
            Else
                Return linqTeams.RemoveFromCampaign_byMemberId(_memberId, _campaignId)
            End If

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\BusinessLayer\blTeams.vb : RemoveFromCampaign_byMemberId()")
            sb.AppendLine("_memberId:" & _memberId)
            sb.AppendLine("_campaignId:" & _campaignId)
            sb.AppendLine("_memberRole:" & _memberRole)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return Nothing
        End Try
    End Function
    Public Function RemoveFromTeam_byMemberId(ByVal _memberId As Integer, ByVal _campaignId As Integer, ByVal _memberRole As String) As BusinessReturn
        Try
            Return linqTeams.RemoveFromTeam_byMemberId(_memberId, _campaignId, True)
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\BusinessLayer\blTeams.vb : RemoveFromTeam_byMemberId()")
            sb.AppendLine("_memberId:" & _memberId)
            sb.AppendLine("_campaignId:" & _campaignId)
            sb.AppendLine("_memberRole:" & _memberRole)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return Nothing
        End Try
    End Function
#End Region

#Region "Methods"
    Public Function doesTeamExist_byName(ByVal _TeamName As String) As Boolean?
        Return linqTeams.doesTeamExist_byName(_TeamName)
    End Function
    Public Function isTeamAdministrator_byUserId(ByVal _userId As Integer, ByVal _teamNodeId As Integer) As Boolean?
        Return linqTeams.isTeamAdministrator_byUserId(_userId, _teamNodeId)
    End Function
    Public Function selectImageCrops() As ListItemCollection
        'Instantiate variables
        Dim preValueRootElementIterator As XPathNodeIterator
        Dim preValueIterator As XPathNodeIterator
        Dim listItems As New ListItemCollection()

        Try
            'Obtain datatype prevalue.  (Must move to first object entry or else iterator will return null.)
            preValueRootElementIterator = umbraco.library.GetPreValues(prevalues.ImageCropper)
            preValueRootElementIterator.MoveNext()

            'Obtain datatype's prevalues as list
            preValueIterator = preValueRootElementIterator.Current.SelectChildren("preValue", "")

            'Loop thru list and obtain a key/value set.
            While preValueIterator.MoveNext()
                'If getPrevalues Then
                listItems.Add(New ListItem(preValueIterator.Current.GetAttribute("id", "").ToString, preValueIterator.Current.Value))
                'Else
                'listItems.Add(New ListItem(preValueIterator.Current.Value, preValueIterator.Current.Value.Replace(" ", "")))
                ' End If

            End While
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\BusinessLayer\blTeams.vb : selectImageCrops()")

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
        '
        Return listItems
    End Function
    Public Function createTable_ImageList() As DataTable
        'Instantiate variables
        Dim dt As DataTable = New DataTable
        'Add Columns
        dt.Columns.Add(imgList.mediaId, GetType(Integer))
        dt.Columns.Add(imgList.imgUrl, GetType(String))
        dt.Columns.Add(imgList.imgThumb, GetType(String))
        dt.Columns.Add(imgList.imgName, GetType(String))
        dt.Columns.Add(imgList.cropTypes, GetType(String))
        Return dt
    End Function
#End Region

#Region "Validations"
    Public Function Validate(ByVal _currentReturn As BusinessReturn, ByVal _teamName As String) As BusinessReturn
        Try
            If String.IsNullOrWhiteSpace(_teamName) Then
                '_currentReturn.ValidationMessages.Add(New ValidationContainer("*Field is required"))
                _currentReturn.ExceptionMessage = "Field is required"
            Else
                If doesTeamExist_byName(_teamName) Then
                    '_currentReturn.ValidationMessages.Add(New ValidationContainer("*Team already exists"))
                    _currentReturn.ExceptionMessage = "Team already exists"
                End If
            End If

            Return _currentReturn
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\BusinessLayer\blTeams.vb : Validate()")
            sb.AppendLine("_currentReturn:" & JsonConvert.SerializeObject(_currentReturn))
            sb.AppendLine("_teamName:" & _teamName)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

            Return _currentReturn
        End Try
    End Function
#End Region
End Class
