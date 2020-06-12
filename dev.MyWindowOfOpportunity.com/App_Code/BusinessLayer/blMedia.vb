Imports umbraco
Imports umbraco.NodeFactory
Imports Common
Imports umbraco.Core
Imports umbraco.Core.Services
Imports umbraco.Core.Models
Imports System.Data
Imports System.Xml.XPath
Imports umbraco.Web

Public Class blMedia

#Region "Properties"
    Private linqMedia As linqMedia = New linqMedia
    Private Structure imgList
        Const mediaId As String = "mediaId"
        Const imgUrl As String = "imgUrl"
        Const imgThumb As String = "imgThumb"
        Const imgName As String = "imgName"
        Const cropTypes As String = "cropTypes"
    End Structure
    Private _uHelper As Uhelper = New Uhelper()
#End Region

#Region "Selects"
    Public Function selectImages_byCampaignId(ByVal _thisNode As IPublishedContent, Optional ByVal crop As String = "") As BusinessReturn ' DataTable
        'Create Validation return
        Dim ValidationReturn As BusinessReturn = New BusinessReturn
        Try
            'Obtain image crops
            Dim results As ListItemCollection = selectImageCrops()
            Dim cropTypes As String = "{crops:" & results(0).Value & "}"
            'Note: to parse the json result, we need to add a heading parameter.  In this case, we added {crops}
            'Example: "{crops:[ { ""width"": 813, ""height"": 225, ""Alias"": ""Campaign Featured Image"" }, { ""width"": 386, ""height"": 316, ""Alias"": ""Reward Image"" } ]}"

            'Check if node has a mediafolder assigned to it. If no, create folder in media
            If Not _thisNode.HasProperty(nodeProperties.mediaFolder) Then
                ValidationReturn = createCampaignsMediaFolder(_thisNode.Id)
                If Not ValidationReturn.isValid Then Return ValidationReturn
            End If

            'Create new datatable to store media info
            Dim dt As DataTable = createTable_ImageList()

            'Get media folder Id
            Dim mediaFolderId As Integer = _thisNode.GetPropertyValue(Of Integer)(nodeProperties.mediaFolder)
            Dim tempMedia As IMedia
            'Loop thru all child media elements and obtain all images
            Dim mediaService = ApplicationContext.Current.Services.MediaService
            For Each media As IMedia In mediaService.GetChildren(mediaFolderId)
                tempMedia = media
                If media.ContentType.Alias = Constants.Conventions.MediaTypes.Image Then

                    'Add data to table
                    Dim dr As DataRow = dt.NewRow
                    dr(imgList.mediaId) = media.Id
                    dr(imgList.imgUrl) = getMediaURL(media.Id, crop) ', 60)
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
            sb.AppendLine("\App_Code\BusinessLayer\blMedia.vb : selectImages_byCampaignId()")
            sb.AppendLine("_thisNode:" & _thisNode.ToString())
            sb.AppendLine("crop:" & crop)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Return w/ exception msg.
            ValidationReturn.ExceptionMessage = ex.Message
            Return ValidationReturn
        End Try
    End Function
    Public Function selectImage_byMemberId(ByVal _memberId As Integer, Optional ByVal crop As String = "") As BusinessReturn ' DataTable
        'Create Validation return
        Dim ValidationReturn As BusinessReturn = New BusinessReturn
        Try
            'Instantiate variables
            Dim member As IMember
            Dim cropList As ListItemCollection
            Dim cropTypes As String
            Dim mediaId As Integer
            Dim media As IMedia
            Dim dtImageList As DataTable = createTable_ImageList()

            '
            member = ApplicationContext.Current.Services.MemberService.GetById(_memberId)

            'Obtain image crops
            cropList = selectImageCrops()
            cropTypes = "{crops:" & cropList(0).Value & "}"
            'Note: to parse the json result, we need to add a heading parameter.  In this case, we added {crops}
            'Example: "{crops:[ { ""width"": 813, ""height"": 225, ""Alias"": ""Campaign Featured Image"" }, { ""width"": 386, ""height"": 316, ""Alias"": ""Reward Image"" } ]}"


            'Check if image exists for user.
            If (member.HasProperty(nodeProperties.photo)) AndAlso (Not String.IsNullOrWhiteSpace(member.GetValue(Of String)(nodeProperties.photo))) Then
                'Obtain user image id
                'imgUrl = getMediaURL(member.GetValue(Of Integer)(nodeProperties.photo), Crops.members)
                mediaId = member.GetValue(Of Integer)(nodeProperties.photo)
                media = selectMedia_byId(mediaId)

                If media.ContentType.Alias = Constants.Conventions.MediaTypes.Image Then

                    'Add data to table
                    Dim dr As DataRow = dtImageList.NewRow
                    dr(imgList.mediaId) = media.Id
                    dr(imgList.imgUrl) = getMediaURL(media.Id, crop)
                    'dr(imgList.imgThumb) = getMediaURL(media.Id).Replace(".", "_big-thumb.")
                    dr(imgList.imgName) = media.Name
                    dr(imgList.cropTypes) = cropTypes
                    dtImageList.Rows.Add(dr)
                End If
            End If


            'Return dt
            ValidationReturn.DataContainer.Add(dtImageList)

            'ValidationReturn.ExceptionMessage = tempMedia.ContentType.Alias
            Return ValidationReturn

        Catch ex As Exception
            'Return w/ exception msg.
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\BusinessLayer\blMedia.vb : selectImage_byMemberId()")
            sb.AppendLine("_memberId:" & _memberId)
            sb.AppendLine("crop:" & crop)
            ValidationReturn.ExceptionMessage = ex.Message
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return ValidationReturn
        End Try
    End Function
    Public Function selectMedia_byId(ByVal _mediaId As Integer) As IMedia
        Try
            'Instantiate variables
            Dim mediaService = ApplicationContext.Current.Services.MediaService
            Return mediaService.GetById(_mediaId)
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\BusinessLayer\blMedia.vb : selectMedia_byId()")
            sb.AppendLine("_mediaId:" & _mediaId)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
    End Function
    Public Function selectMediaData_byId(ByVal _mediaId As Integer) As BusinessReturn
        'Create Validation return
        Dim ValidationReturn As BusinessReturn = New BusinessReturn
        Try
            'Instantiate variables
            Dim mediaService = ApplicationContext.Current.Services.MediaService
            Dim media As IMedia = mediaService.GetById(_mediaId)
            Dim mediaProperties As MediaProperties = New MediaProperties

            '
            mediaProperties.umbracoHeight = CInt(media.Properties(mediaProperty.umbracoHeight).Value)
            mediaProperties.umbracoWidth = CInt(media.Properties(mediaProperty.umbracoWidth).Value)
            mediaProperties.umbracoBytes = CInt(media.Properties(mediaProperty.umbracoBytes).Value)
            mediaProperties.umbracoExtension = media.Properties(mediaProperty.umbracoExtension).Value
            mediaProperties.umbracoFile = media.Properties(mediaProperty.umbracoFile).Value
            mediaProperties.media = media

            '
            ValidationReturn.DataContainer.Add(mediaProperties)

            'ValidationReturn.ExceptionMessage = tempMedia.ContentType.Alias
            Return ValidationReturn

        Catch ex As Exception
            'Return w/ exception msg.
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\BusinessLayer\blMedia.vb : selectMediaData_byId()")
            sb.AppendLine("_mediaId:" & _mediaId)

            ValidationReturn.ExceptionMessage = ex.Message
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return ValidationReturn
        End Try
    End Function
    Public Function selectMediaName_byId(ByVal _mediaId As Integer) As BusinessReturn
        'Create Validation return
        Dim ValidationReturn As BusinessReturn = New BusinessReturn
        Try
            'Instantiate variables
            Dim mediaService = ApplicationContext.Current.Services.MediaService
            Dim media As IMedia = mediaService.GetById(_mediaId)

            'Obtain image name.
            If media.IsValid Then ValidationReturn.ReturnMessage = media.Name
            Return ValidationReturn

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\BusinessLayer\blMedia.vb : selectMediaName_byId()")
            sb.AppendLine("_mediaId:" & _mediaId)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Return w/ exception msg.
            ValidationReturn.ExceptionMessage = ex.Message
            Return ValidationReturn
        End Try
    End Function
    Public Function selectImageCrops() As ListItemCollection
        'Instantiate variables
        Dim preValueRootElementIterator As XPathNodeIterator
        Dim preValueIterator As XPathNodeIterator
        Dim listItems As New ListItemCollection()

        Try 'Obtain datatype prevalue.  (Must move to first object entry or else iterator will return null.)
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
            sb.AppendLine("\App_Code\BusinessLayer\blMedia.vb : selectImageCrops()")

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

        End Try
        '
        Return listItems
    End Function
    Public Function selectImageThumbs_byCampaignId(ByVal _campaignId As Integer) As BusinessReturn
        'Instantiate variables
        Dim BusinessReturn As BusinessReturn = New BusinessReturn

        Try
            'Instantiate variables
            Dim ipCampaign As IPublishedContent = _uHelper.Get_IPublishedContentByID(_campaignId)

            'If node contains a media folder, return the media folder's images
            If ipCampaign.HasProperty(nodeProperties.mediaFolder) AndAlso ipCampaign.HasValue(nodeProperties.mediaFolder) Then
                'Instantiate variables
                Dim blMedia As blMedia = New blMedia
                Dim mediaFolderId As Integer = ipCampaign.GetPropertyValue(Of Integer)(nodeProperties.mediaFolder)
                Dim tempMedia As IMedia
                Dim ImageList As List(Of ImageItem) = New List(Of ImageItem)
                Dim ImageItem As ImageItem

                'Loop thru all child media elements and obtain all images
                Dim mediaService = ApplicationContext.Current.Services.MediaService
                For Each media As IMedia In mediaService.GetChildren(mediaFolderId)
                    tempMedia = media
                    If media.ContentType.Alias = Constants.Conventions.MediaTypes.Image Then
                        'Add data to image list
                        ImageItem = New ImageItem
                        ImageItem.image = getMediaURL(media.Id)
                        ImageItem.thumb = getMediaURL(media.Id).Replace(".", "_thumb.")
                        ImageList.Add(ImageItem)
                    End If
                Next

                'Save data in a json format
                BusinessReturn.ReturnMessage = JsonHelper.ToJson(ImageList)
                Return BusinessReturn
            Else
                Return BusinessReturn
            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\BusinessLayer\blMedia.vb : selectImageThumbs_byCampaignId()")
            sb.AppendLine("_campaignId:" & _campaignId)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return BusinessReturn
        End Try
    End Function
#End Region

#Region "Insert"
    Public Function createCampaignsMediaFolder(ByVal _campaignId As Integer) As BusinessReturn
        'Does campaign have media folder?
        '   Yes: return valid
        '   No: 
        '       -Create variables
        '       -does media.Campaigns.alphaFolder exist?
        '           -no: create, get id
        '           -yes; get id
        '
        '       -does media.Campaigns.alphaFolder.teamNode exist?
        '           -no: create, get id
        '           -yes; get id
        '
        '       -does media.Campaigns.alphaFolder.teamNode.Campaign exist?
        '           -no: create, get id
        '           -yes; get id
        '
        '       -campaign.mediaFolder = id
        '==========================================

        'Create Validation return
        Dim ValidationReturn As BusinessReturn = New BusinessReturn
        Try
            If _campaignId <> -1 Then
                'Instantiate variables
                Dim ipCampaign As IPublishedContent = _uHelper.Get_IPublishedContentByID(_campaignId)

                'Check if node has a mediafolder assigned to it. If no, create folder in media
                If Not ipCampaign.HasValue(nodeProperties.mediaFolder) Then
                    'Instantiate variables
                    Dim cs As IContentService = ApplicationContext.Current.Services.ContentService
                    Dim ipCampaignTeam As IPublishedContent = ipCampaign.Parent
                    Dim campaignAlphaFolderName As String = ipCampaignTeam.Parent.Name
                    Dim mediaService = ApplicationContext.Current.Services.MediaService

                    'Loop thru media alphafolders to locate matching folder name.
                    For Each mediaAlphaFolder As IMedia In mediaService.GetChildren(mediaNodes.Campaigns)
                        'If alphaFolder = name: continue
                        If mediaAlphaFolder.Name = campaignAlphaFolderName Then
                            'Instantiate variables
                            Dim mediaTeam As IMedia = Nothing

                            'Loop thru media teams in the media alphaFolder
                            For Each teamInMedia As IMedia In mediaService.GetChildren(mediaAlphaFolder.Id)
                                'if media has matching team folder, get it.
                                If teamInMedia.Name = ipCampaignTeam.Name Then
                                    mediaTeam = teamInMedia
                                    Exit For
                                End If
                            Next

                            'Create new media folder if it doesnt exist.
                            If IsNothing(mediaTeam) Then
                                'Use the MediaService to create a new Team media folder 
                                Dim mediaFolder As IMedia = mediaService.CreateMedia(ipCampaignTeam.Name, mediaAlphaFolder.Id, Constants.Conventions.MediaTypes.Folder)
                                'Save the new Media object
                                mediaService.Save(mediaFolder)
                                'Get new folder
                                mediaTeam = mediaFolder
                                'Save media Team folder to content team folder
                                Dim icCampaignTeam As IContent = cs.GetById(ipCampaignTeam.Id)
                                icCampaignTeam.SetValue(nodeProperties.mediaFolder, mediaTeam.Id)
                                cs.SaveAndPublishWithStatus(icCampaignTeam)
                                'icCampaignTeam.Publish(False)
                            End If

                            'Instantiate variables
                            Dim mediaCampaign As IMedia = Nothing

                            'Loop thru media teams in the media alphaFolder
                            For Each campaignsInMedia As IMedia In mediaService.GetChildren(mediaTeam.Id)
                                'if media has matching campaign folder, 
                                If campaignsInMedia.Name = ipCampaign.Name Then
                                    mediaCampaign = campaignsInMedia
                                    Exit For
                                End If
                            Next

                            'Create new media folder for campaign if it doesnt exist.
                            If IsNothing(mediaCampaign) Then
                                'Use the MediaService to create a new Team media folder 
                                Dim mediaFolder As IMedia = mediaService.CreateMedia(ipCampaign.Name, mediaTeam.Id, Constants.Conventions.MediaTypes.Folder)
                                'Save the new Media object
                                mediaService.Save(mediaFolder)
                                'Get new folder
                                mediaCampaign = mediaFolder
                            End If

                            '
                            'ipCampaign.SetProperty(nodeProperties.mediaFolder, mediaCampaign.Id)
                            'ipCampaign.Publish(True)
                            Dim icCampaign As IContent = cs.GetById(ipCampaign.Id)
                            icCampaign.SetValue(nodeProperties.mediaFolder, mediaCampaign.Id)
                            cs.SaveAndPublishWithStatus(icCampaign)

                            Exit For
                        End If
                    Next
                End If
            End If


            'ValidationReturn.ReturnMessage = "all ok"
            Return ValidationReturn
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\BusinessLayer\blMedia.vb : createCampaignsMediaFolder()")
            sb.AppendLine("_campaignId: " & _campaignId)
            saveErrorMessage(getLoggedInMember(), ex.ToString, sb.ToString)

            'Return w/ exception msg.
            ValidationReturn.ExceptionMessage = ex.Message
            Return ValidationReturn
        End Try
    End Function
#End Region

#Region "Updates"
    Public Function updateMediaPropertyData_byId(ByVal _mediaId As Integer, ByVal _jsonData As String) As BusinessReturn
        'Create Validation return
        Dim ValidationReturn As BusinessReturn = New BusinessReturn

        Try
            'Validate that data is in JSon format
            ValidationReturn = Validate_UpdateMediaPropertyData(_mediaId, _jsonData)

            If ValidationReturn.isValid Then
                'Submit data to database
                Return linqMedia.updateMediaPropertyData_byId(_mediaId, _jsonData)
            Else
                Return ValidationReturn
            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\BusinessLayer\blMedia.vb : updateMediaPropertyData_byId()")
            sb.AppendLine("_mediaId:" & _mediaId)
            sb.AppendLine("_jsonData:" & _jsonData)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Return w/ exception msg.
            ValidationReturn.ExceptionMessage = ex.Message
            Return ValidationReturn
        End Try
    End Function
#End Region

#Region "Delete"
    Public Function deleteMedia_byId(ByVal _mediaId As Integer) As BusinessReturn
        'Create Validation return
        Dim ValidationReturn As BusinessReturn = New BusinessReturn
        Try
            'Instantiate variables for media services
            Dim mediaService = ApplicationContext.Current.Services.MediaService
            Dim mediaFile As IMedia

            'Delete media by ID 
            mediaFile = mediaService.GetById(_mediaId)
            mediaService.Delete(mediaFile)

            'Return valiation result
            Return ValidationReturn
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\BusinessLayer\blMedia.vb : deleteMedia_byId()")
            sb.AppendLine("_mediaId:" & _mediaId)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Return w/ exception msg.
            ValidationReturn.ExceptionMessage = ex.Message
            Return ValidationReturn
        End Try
    End Function
    Public Function deleteMedia_byMemberId(ByVal _memberId As Integer) As BusinessReturn
        'Create Validation return
        Dim ValidationReturn As BusinessReturn = New BusinessReturn
        Try
            'Instantiate variables for media services
            Dim mediaService = ApplicationContext.Current.Services.MediaService
            Dim mediaFile As IMedia
            Dim member As IMember
            Dim mediaId As Integer
            Dim MemberService As IMemberService = ApplicationContext.Current.Services.MemberService

            'Obtain member by id
            member = MemberService.GetById(_memberId)

            'Check if image exists for user.
            If (member.HasProperty(nodeProperties.photo)) AndAlso (Not String.IsNullOrWhiteSpace(member.GetValue(Of String)(nodeProperties.photo))) Then
                'Obtain user image id
                'imgUrl = getMediaURL(member.GetValue(Of Integer)(nodeProperties.photo), Crops.members)
                mediaId = member.GetValue(Of Integer)(nodeProperties.photo)

                'Delete mediaId from member
                member.SetValue(nodeProperties.photo, String.Empty)
                MemberService.Save(member)

                'Delete media by ID 
                mediaFile = mediaService.GetById(mediaId)
                mediaService.Delete(mediaFile)
            End If

            'Return valiation result
            Return ValidationReturn
        Catch ex As Exception
            'Return w/ exception msg.
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\BusinessLayer\blMedia.vb : deleteMedia_byMemberId()")
            sb.AppendLine("_memberId:" & _memberId)

            ValidationReturn.ExceptionMessage = ex.Message
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return ValidationReturn
        End Try
    End Function
#End Region

#Region "Methods"
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
    Private Function Validate(ByVal _email As String, ByVal _password As String) As BusinessReturn
        Dim returnObject As New BusinessReturn()
        Try
            ''Validate User Id
            'If (linqMembers.doesMemberExist_byUserId(_userId)) Then
            '    returnObject.ValidationMessages.Add(New ValidationContainer("ID Exists"))
            'End If

            ''Validate Email
            'If (linqMembers.doesMemberExist_byEmail(_email)) Then
            '    returnObject.ValidationMessages.Add(New ValidationContainer("duplicate email- cannot use."))
            'End If

            ''Validate Password: Ensure password has combination of letters/numbers and is 8+ characters long
            'If Not System.Text.RegularExpressions.Regex.IsMatch(_password, "^[a-zA-Z0-9]{8,16}$") Then
            '    returnObject.ValidationMessages.Add(New ValidationContainer("Invalid Password.  Please ensure your password is alphanumeric only, between 8-16 characters."))
            'End If

            'Return all validation messages
            Return returnObject
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\BusinessLayer\blMedia.vb : _email()")
            sb.AppendLine("_email:" & _email)
            sb.AppendLine("_password:" & _password)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            returnObject.ExceptionMessage = ex.ToString
            Return returnObject
        End Try
    End Function
    Private Function Validate_UpdateMediaPropertyData(ByVal _mediaId As Integer, ByVal _jsonData As String) As BusinessReturn
        Dim returnObject As New BusinessReturn()
        Try
            '
            If Not linqMedia.doesMediaExist(_mediaId) Then
                returnObject.ValidationMessages.Add(New ValidationContainer("Invalid Media Id. This image may have been deleted.  Try refreshing the page first."))
            End If

            'TO DO:
            '   validate json to schema, or atleast is valid json.
            '? Dim serializer = New JavaScriptSerializer()
            '? Dim result = serializer.Deserialize(Of Dictionary(Of String, Object))(_jsonData)


            'Return all validation messages
            Return returnObject
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\BusinessLayer\blMedia.vb : Validate_UpdateMediaPropertyData()")
            sb.AppendLine("_mediaId:" & _mediaId)
            sb.AppendLine("_jsonData:" & _jsonData)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            returnObject.ExceptionMessage = ex.ToString
            Return returnObject
        End Try
    End Function
#End Region

End Class
