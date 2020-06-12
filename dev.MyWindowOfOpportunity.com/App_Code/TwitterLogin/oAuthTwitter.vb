
Imports System.Collections.Specialized
Imports System.Configuration
Imports System.IO
Imports System.Net
Imports System.Web
Imports Common


Public Class oAuthTwitter
    Inherits OAuthBase
#Region "Method enum"

    Public Enum Method
        [GET]
        POST
        DELETE
    End Enum

#End Region

    Public Const REQUEST_TOKEN As String = "https://api.twitter.com/oauth/request_token"
    Public Const AUTHORIZE As String = "https://api.twitter.com/oauth/authorize"
    Public Const ACCESS_TOKEN As String = "https://api.twitter.com/oauth/access_token"

    Private _callBackUrl As String = ""
    Private _consumerKey As String = ""
    Private _consumerSecret As String = ""
    Private _oauthVerifier As String = ""
    Private _token As String = ""
    Private _tokenSecret As String = ""

#Region "Properties"
    Public Property ConsumerKey() As String
        Get
            If _consumerKey.Length = 0 Then
                _consumerKey = ConfigurationManager.AppSettings("consumerKey")
            End If
            Return _consumerKey
        End Get
        Set
            _consumerKey = Value
        End Set
    End Property
    Public Property ConsumerSecret() As String
        Get
            If _consumerSecret.Length = 0 Then
                _consumerSecret = ConfigurationManager.AppSettings("consumerSecret")
            End If
            Return _consumerSecret
        End Get
        Set
            _consumerSecret = Value
        End Set
    End Property
    Public Property Token() As String
        Get
            Return _token
        End Get
        Set
            _token = Value
        End Set
    End Property
    Public Property TokenSecret() As String
        Get
            Return _tokenSecret
        End Get
        Set
            _tokenSecret = Value
        End Set
    End Property
    Public Property CallBackUrl() As String
        Get
            Return _callBackUrl
        End Get
        Set
            _callBackUrl = Value
        End Set
    End Property
    Public Property OAuthVerifier() As String
        Get
            Return _oauthVerifier
        End Get
        Set
            _oauthVerifier = Value
        End Set
    End Property
#End Region

    ''' <summary>
    ''' Get the link to Twitter's authorization page for this application.
    ''' </summary>
    ''' <returns>The url with a valid request token, or a null string.</returns>
    Public Function AuthorizationLinkGet() As String
        Dim ret As String = Nothing

        Dim response As String = oAuthWebRequest(Method.[GET], REQUEST_TOKEN, [String].Empty)
        If response.Length > 0 Then
            'response contains token and token secret.  We only need the token.
            Dim qs As NameValueCollection = HttpUtility.ParseQueryString(response)

            If qs("oauth_callback_confirmed") IsNot Nothing Then
                If qs("oauth_callback_confirmed") <> "true" Then
                    Throw New Exception("OAuth callback not confirmed.")
                End If
            End If

            If qs("oauth_token") IsNot Nothing Then
                ret = (AUTHORIZE & Convert.ToString("?oauth_token=")) + qs("oauth_token")
            End If
        End If
        Return ret
    End Function

    ''' <summary>
    ''' Exchange the request token for an access token.
    ''' </summary>
    ''' <param name="authToken">The oauth_token is supplied by Twitter's authorization page following the callback.</param>
    ''' <param name="oauthVerifier">An oauth_verifier parameter is provided to the client either in the pre-configured callback URL</param>
    Public Sub AccessTokenGet(authToken As String, oauthVerifier__1 As String)
        Token = authToken
        OAuthVerifier = oauthVerifier__1

        Dim response As String = oAuthWebRequest(Method.[GET], ACCESS_TOKEN, [String].Empty)

        If response.Length > 0 Then
            'Store the Token and Token Secret
            Dim qs As NameValueCollection = HttpUtility.ParseQueryString(response)
            If qs("oauth_token") IsNot Nothing Then
                Token = qs("oauth_token")
            End If
            If qs("oauth_token_secret") IsNot Nothing Then
                TokenSecret = qs("oauth_token_secret")
            End If
        End If
    End Sub

    ''' <summary>
    ''' Submit a web request using oAuth.
    ''' </summary>
    ''' <param name="method">GET or POST</param>
    ''' <param name="url">The full url, including the querystring.</param>
    ''' <param name="postData">Data to post (querystring format)</param>
    ''' <returns>The web server response.</returns>
    Public Function oAuthWebRequest(method__1 As Method, url As String, postData As String) As String
        Dim outUrl As String = ""
        Dim querystring As String = ""
        Dim ret As String = ""


        'Setup postData for signing.
        'Add the postData to the querystring.
        If method__1 = Method.POST OrElse method__1 = Method.DELETE Then
            If postData.Length > 0 Then
                'Decode the parameters and re-encode using the oAuth UrlEncode method.
                Dim qs As NameValueCollection = HttpUtility.ParseQueryString(postData)
                postData = ""
                For Each key As String In qs.AllKeys
                    If postData.Length > 0 Then
                        postData += "&"
                    End If
                    qs(key) = HttpUtility.UrlDecode(qs(key))
                    qs(key) = UrlEncode(qs(key))
                    postData += (key & Convert.ToString("=")) + qs(key)
                Next
                If url.IndexOf("?") > 0 Then
                    url += "&"
                Else
                    url += "?"
                End If
                url += postData
            End If
        End If

        Dim uri = New Uri(url)

        Dim nonce As String = GenerateNonce()
        Dim timeStamp As String = GenerateTimeStamp()

        'Generate Signature
        Dim sig As String = GenerateSignature(uri, ConsumerKey, ConsumerSecret, Token, TokenSecret, CallBackUrl,
            OAuthVerifier, method__1.ToString(), timeStamp, nonce, outUrl, querystring)

        querystring += "&oauth_signature=" + UrlEncode(sig)

        'Convert the querystring to postData
        If method__1 = Method.POST OrElse method__1 = Method.DELETE Then
            postData = querystring
            querystring = ""
        End If

        If querystring.Length > 0 Then
            outUrl += "?"
        End If
        If False Then
            querystring = querystring & Convert.ToString("&include_email=true")
        End If
        ret = WebRequest(method__1, outUrl & querystring, postData)

        Return ret
    End Function
    Public Function oAuthWebRequest1(method__1 As Method, url As String, postData As String) As String
        Dim outUrl As String = ""
        Dim querystring As String = ""
        Dim ret As String = ""


        'Setup postData for signing.
        'Add the postData to the querystring.
        If method__1 = Method.POST OrElse method__1 = Method.DELETE Then
            If postData.Length > 0 Then
                'Decode the parameters and re-encode using the oAuth UrlEncode method.
                Dim qs As NameValueCollection = HttpUtility.ParseQueryString(postData)
                postData = ""
                For Each key As String In qs.AllKeys
                    If postData.Length > 0 Then
                        postData += "&"
                    End If
                    qs(key) = HttpUtility.UrlDecode(qs(key))
                    qs(key) = UrlEncode(qs(key))
                    postData += (key & Convert.ToString("=")) + qs(key)
                Next
                If url.IndexOf("?") > 0 Then
                    url += "&"
                Else
                    url += "?"
                End If
                url += postData
            End If
        End If

        Dim uri = New Uri(url)

        Dim nonce As String = GenerateNonce()
        Dim timeStamp As String = GenerateTimeStamp()

        'Generate Signature
        Dim sig As String = GenerateSignature(uri, ConsumerKey, ConsumerSecret, Token, TokenSecret, CallBackUrl,
            OAuthVerifier, method__1.ToString(), timeStamp, nonce, outUrl, querystring)

        querystring += "&include_email=true&oauth_signature=" + UrlEncode(sig)

        'Convert the querystring to postData
        If method__1 = Method.POST OrElse method__1 = Method.DELETE Then
            postData = querystring
            querystring = ""
        End If

        If querystring.Length > 0 Then
            outUrl += "?"
        End If



        ret = WebRequest(method__1, outUrl & querystring, postData)

        Return ret
    End Function

    ''' <summary>
    ''' Web Request Wrapper
    ''' </summary>
    ''' <param name="method">Http Method</param>
    ''' <param name="url">Full url to the web resource</param>
    ''' <param name="postData">Data to post in querystring format</param>
    ''' <returns>The web server response.</returns>
    Public Function WebRequest(method__1 As Method, url As String, postData As String) As String
        Dim webRequest__2 As HttpWebRequest = Nothing
        Dim requestWriter As StreamWriter = Nothing
        Dim responseData As String = ""

        webRequest__2 = TryCast(System.Net.WebRequest.Create(url), HttpWebRequest)
        webRequest__2.Method = method__1.ToString()
        webRequest__2.ServicePoint.Expect100Continue = False
        'webRequest.UserAgent  = "Identify your application please.";
        'webRequest.Timeout = 20000;

        If method__1 = Method.POST OrElse method__1 = Method.DELETE Then
            webRequest__2.ContentType = "application/x-www-form-urlencoded"

            'POST the data.
            requestWriter = New StreamWriter(webRequest__2.GetRequestStream())
            Try
                requestWriter.Write(postData)
            Catch ex As Exception
                Dim sb As New StringBuilder()
                sb.AppendLine("\App_Code\TwitterLogin\oAuthTwitter.vb : WebRequest()")
                sb.AppendLine("url:" & url)
                sb.AppendLine("postData:" & postData)

                saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

                Throw
            Finally
                requestWriter.Close()
                requestWriter = Nothing
            End Try
        End If

        If Not IsNothing(webRequest__2) Then
            responseData = WebResponseGet(webRequest__2)
        End If

        webRequest__2 = Nothing

        Return responseData
    End Function

    ''' <summary>
    ''' Process the web response.
    ''' </summary>
    ''' <param name="webRequest">The request object.</param>
    ''' <returns>The response data.</returns>
    Public Function WebResponseGet(webRequest As HttpWebRequest) As String
        Dim responseReader As StreamReader = Nothing
        Dim responseData As String = ""

        Try
            responseReader = New StreamReader(webRequest.GetResponse().GetResponseStream())
            responseData = responseReader.ReadToEnd()
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\TwitterLogin\oAuthTwitter.vb : WebResponseGet()")
            sb.AppendLine("webRequest:" & webRequest.ToString())
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

            Throw
        Finally
            webRequest.GetResponse().GetResponseStream().Close()
            responseReader.Close()
            responseReader = Nothing
        End Try

        Return responseData
    End Function
End Class

