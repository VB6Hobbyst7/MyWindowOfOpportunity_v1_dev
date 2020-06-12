Imports umbraco.Core
Imports umbraco.Core.Models
Imports Common

Public Class linqMedia

#Region "Properties"
    Private linq2Db As linq2SqlDataContext = New linq2SqlDataContext(ConfigurationManager.ConnectionStrings("umbracoDbDSN").ToString)

    'Umbraco Membership Helper classes | https://our.umbraco.org/documentation/Reference/Querying/MemberShipHelper/
    'Dim memberShipHelper As umbraco.Web.Security.MembershipHelper = New umbraco.Web.Security.MembershipHelper(umbraco.Web.UmbracoContext.Current)
#End Region

#Region "Selects"
#End Region

#Region "Inserts"
    'Public Function Insert(ByVal _valueDictionary As Dictionary(Of String, String)) As Boolean
    '    Try
    '        'Obtain all needed values for creating member
    '        Dim userName As String = _valueDictionary.Item(queryParameters.firstName) & " " & _valueDictionary.Item(queryParameters.lastName)
    '        Dim userId As String = _valueDictionary.Item(queryParameters.email) '_valueDictionary.Item(queryParameters.userId)
    '        Dim email As String = _valueDictionary.Item(queryParameters.email)
    '        Dim password As String = _valueDictionary.Item(queryParameters.password)
    '        Dim memberTypeAlias As String = memberTypes.member

    '        'Create member
    '        Dim MemberService As IMemberService = ApplicationContext.Current.Services.MemberService
    '        Dim MemberGroupService As IMemberGroupService = ApplicationContext.Current.Services.MemberGroupService
    '        Dim newMember As IMember = MemberService.CreateMemberWithIdentity(userId, email, userName, memberTypeAlias)
    '        newMember.IsApproved = True
    '        MemberService.SavePassword(newMember, password)

    '        'Set member values
    '        newMember.SetValue("firstName", _valueDictionary.Item(queryParameters.firstName))
    '        newMember.SetValue("lastName", _valueDictionary.Item(queryParameters.lastName))

    '        'Save new member
    '        MemberService.Save(newMember)

    '        'Return successful
    '        Return True

    '        ''Log member in
    '        'Return logMemberIn(newMember.Username, newMember.RawPasswordValue)
    '    Catch ex As Exception
    '        'Throw
    '        Return False
    '    End Try
    'End Function
#End Region

#Region "Updates"
    'Public Function Update(ByVal _profileModel As Web.Models.ProfileModel) As Boolean
    '    Return memberShipHelper.UpdateMemberProfile(_profileModel)
    'End Function

    Public Function updateMediaPropertyData_byId(ByVal _mediaId As Integer, ByVal _jsonData As String) As BusinessReturn
        'Create Validation return
        Dim ValidationReturn As BusinessReturn = New BusinessReturn

        Try
            'Instantiate variables
            Dim mediaService = ApplicationContext.Current.Services.MediaService
            Dim media As IMedia = mediaService.GetById(_mediaId)
            media.Properties(mediaProperty.umbracoFile).Value = _jsonData
            mediaService.Save(media)

            'ValidationReturn.ExceptionMessage = tempMedia.ContentType.Alias
            Return ValidationReturn

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\linqQueries\linqMedia.vb : updateMediaPropertyData_byId()")
            sb.AppendLine("_mediaId:" & _mediaId)
            sb.AppendLine("_jsonData:" & _jsonData)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Return w/ exception msg.
            ValidationReturn.ExceptionMessage = ex.Message
            Return ValidationReturn
        End Try
    End Function
#End Region

#Region "Methods"
    'Public Function doesMemberExist_byUserId(ByVal _userId As String) As Boolean
    '    'Return if exists
    '    Return ApplicationContext.Current.Services.MemberService.Exists(_userId)
    'End Function
    'Public Function doesMemberExist_byEmail(ByVal _email As String) As Boolean
    '    'Return if exists
    '    Dim member As IMember = ApplicationContext.Current.Services.MemberService.GetByEmail(_email)
    '    Return Not IsNothing(member)
    'End Function
    'Public Function isMemberLoggedIn() As Boolean
    '    Return memberShipHelper.IsLoggedIn()
    'End Function
    'Public Function getCurrentLoginStatus() As Web.Models.LoginStatusModel
    '    Return memberShipHelper.GetCurrentLoginStatus()
    'End Function
    'Public Function GetCurrentMemberId() As Integer
    '    Return memberShipHelper.GetCurrentMemberId()
    'End Function
    'Public Function GetCurrentMemberName() As String
    '    Return memberShipHelper.GetCurrentMember.Name
    'End Function
    'Public Function logMemberIn(ByVal _userName As String, ByVal _password As String) As Boolean
    '    Try
    '        If memberShipHelper.Login(_userName, _password) Then
    '            System.Web.Security.FormsAuthentication.SetAuthCookie(_userName, False)
    '            Return True
    '        Else
    '            Return False
    '        End If
    '    Catch ex As Exception
    '        Return False
    '    End Try
    'End Function
    'Public Sub logMemberOut()
    '    'Log member out
    '    Current.Session.Clear()
    '    Current.Session.Abandon()
    '    Roles.DeleteCookie()
    '    FormsAuthentication.SignOut()

    'End Sub
    Public Function doesMediaExist(ByVal _mediaId As Integer) As Boolean?
        Dim mediaService = ApplicationContext.Current.Services.MediaService
        Try
            Dim media As IMedia = mediaService.GetById(_mediaId)
            Return media.IsValid
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\linqQueries\linqMedia.vb : doesMediaExist()")
            sb.AppendLine("_mediaId:" & _mediaId)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try

    End Function

#End Region

End Class

