Imports System.Web.Script.Services
Imports System.Web.Services
Imports Newtonsoft.Json
Imports Stripe
Imports Common
Imports umbraco.Core
Imports umbraco.Core.Publishing
Imports umbraco.Core.Models
Imports umbraco.Web



' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<System.Web.Script.Services.ScriptService()>
<WebService(Namespace:="http://tempuri.org/")>
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Public Class GenericServices
    Inherits System.Web.Services.WebService


#Region "Properties"
    Private blMembers As blMembers
    Private blEmails As blEmails
    Private statusCode As Integer = 200 '200 = OK; 400 = Bad Request
    Private errorMsg As String = String.Empty
    Private _uHelper As Uhelper = New Uhelper()
#End Region

#Region "Public WebMethods"
    '<WebMethod()>
    'Public Function HelloWorld() As String
    '    Return "Hello World"
    'End Function
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function ResubmitAuthenticationEmail(ByVal nodeId As String) As String
        'Scope variables
        Dim isValid As New IsValid

        Try
            'Instantiate variables
            Dim ipPreAcct As IPublishedContent = _uHelper.Get_IPublishedContentByID(nodeId)

            If IsNothing(ipPreAcct) Then
                isValid.isValid = False
                isValid.resultStatus = 400
                isValid.resultMsg = "Invalid pre-acct"
            Else
                'Send email
                blEmails = New blEmails
                Dim firstName As String = ipPreAcct.GetPropertyValue(Of String)(nodeProperties.firstName).Trim.ToLower.ToFirstUpper
                Dim lastName As String = ipPreAcct.GetPropertyValue(Of String)(nodeProperties.lastName).Trim.ToLower.ToFirstUpper
                Dim email As String = ipPreAcct.GetPropertyValue(Of String)(nodeProperties.email).Trim.ToLower

                Dim url As String = _uHelper.Get_IPublishedContentByID(siteNodes.BecomeAMember).UrlAbsolute & "?" & queryParameters.preAcctId & "=" & nodeId

                blEmails.sendVerifyEmailToCreateAcct(url, firstName + " " + lastName, email)

                isValid.isValid = True
                isValid.resultStatus = 200
                isValid.resultMsg = url
            End If

            Return Newtonsoft.Json.JsonConvert.SerializeObject(isValid)

        Catch ex As Exception
            isValid.isValid = False
            isValid.resultStatus = 500
            isValid.resultMsg = "Error: " & ex.ToString
            Dim sb As New StringBuilder()
            sb.AppendLine("GenericServices.vb : ResubmitAuthenticationEmail()")
            sb.AppendLine("nodeId:" & nodeId)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return Newtonsoft.Json.JsonConvert.SerializeObject(isValid)
        End Try
    End Function
#End Region

#Region "Private Methods"

#End Region
End Class