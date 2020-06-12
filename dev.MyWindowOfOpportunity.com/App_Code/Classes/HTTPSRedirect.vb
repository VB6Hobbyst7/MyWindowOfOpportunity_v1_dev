Imports SEOChecker.Extensions.Providers.UrlRewriteProvider
Imports System.Configuration
Imports Common


Public Class HTTPSRedirect
    Inherits UrlRewriteProviderBase

    Public Overrides Sub RewriteUrl(builder As UrlBuilder, config As UrlRewriteConfiguration)
        Try
            'Obtain value from web.config
            Dim httpsRedirect As Boolean = CBool(ConfigurationManager.AppSettings("httpsRedirect"))
            If httpsRedirect Then
                If Not HttpContext.Current.ApplicationInstance.Request.IsSecureConnection Then
                    RedirectAccordingToRequiresSSL(Uri.UriSchemeHttps)
                End If
            End If








            ''If httpsRedirect Then
            'If builder.Scheme.Equals("http", StringComparison.InvariantCultureIgnoreCase) Then
            '    builder.Scheme = "https"
            'End If
            ''End If



            'If Not HttpContext.Current.ApplicationInstance.Request.IsLocal AndAlso Not HttpContext.Current.ApplicationInstance.Request.IsSecureConnection Then
            '    Dim redirectUrl As String = HttpContext.Current.ApplicationInstance.Request.Url.ToString().Replace("http:", "https:")
            '    HttpContext.Current.ApplicationInstance.Response.Redirect(redirectUrl, False)
            '    HttpContext.Current.ApplicationInstance.CompleteRequest() 'WebForms
            '    'HttpContext.ApplicationInstance.CompleteRequest()  'MVC
            'End If

        Catch ex As Exception
            'Save error to umbraco
            Dim sb As New StringBuilder
            sb.AppendLine("HTTPSRedirect.vb | RewriteUrl()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
    End Sub


    Private Sub RedirectAccordingToRequiresSSL(ByVal scheme As String)
        Dim url = scheme & Uri.SchemeDelimiter + HttpContext.Current.ApplicationInstance.Request.Url.Authority + HttpContext.Current.ApplicationInstance.Request.Url.PathAndQuery
        HttpContext.Current.ApplicationInstance.Response.Redirect(url, False)
    End Sub
End Class
