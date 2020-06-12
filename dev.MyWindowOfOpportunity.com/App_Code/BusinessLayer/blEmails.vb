Imports Common
Imports System.Net.Mail
Imports System.Net
Imports System.Net.Mime
Imports umbraco.Core.Models
Imports umbraco.Web
Imports System.IO
Imports Stripe


Public Class blEmails

#Region "Properties"
    Private _uHelper As Uhelper = New Uhelper()
#End Region

#Region "Emails"
    Public Sub sendCampaignEmail_byPledge_refundedTransaction(ByVal ipPledge As IPublishedContent)
        Try
            'Instantiate variables
            Dim blMembers As New blMembers
            Dim blTeams As New blTeams
            Dim userName As String = String.Empty
            Dim userEmail As String = String.Empty
            Dim message As String = "Thank you for your support, but unfortunately the campaign you supported did not successfully reach their goal.  Thus, we are refunding your pledge.  If the campaign administration re-initiate their campaign, please feel free to again support thier endevour.  Thank you."

            'Obtain membership information for sending email.
            Dim bReturn As BusinessReturn = blMembers.getMemberDemographics_byId(ipPledge.GetPropertyValue(Of Integer)(nodeProperties.pledgingMember), False, False, False, True, False, False)

            'Proceed if valid data is returned.
            If bReturn.isValid Then
                'Extract data
                Dim clsMember As Member = DirectCast(bReturn.DataContainer(0), Member)
                userName = clsMember.MembershipProperties.nodeName
                userEmail = clsMember.MembershipProperties.email

                If Not String.IsNullOrEmpty(userName) AndAlso Not String.IsNullOrEmpty(userEmail) Then
                    'Obtain smtp
                    Dim smtp As New SmtpClient(System.Configuration.ConfigurationManager.AppSettings("SMTPhost").ToString(), System.Configuration.ConfigurationManager.AppSettings("SMTPport").ToString()) With
                        {
                            .Credentials = New NetworkCredential(System.Configuration.ConfigurationManager.AppSettings("SMTPusername").ToString(), System.Configuration.ConfigurationManager.AppSettings("SMTPpassword").ToString())
                        }

                    'set the content by openning the files.
                    Dim filePath_html As String = HttpContext.Current.Request.PhysicalApplicationPath + "Emails\Nightly\Campaign.html"
                    Dim filePath_Text As String = HttpContext.Current.Request.PhysicalApplicationPath + "Emails\Nightly\Campaign.txt"
                    Dim emailBody_Html_original As String = System.IO.File.ReadAllText(filePath_html)
                    Dim emailBody_Text_original As String = System.IO.File.ReadAllText(filePath_Text)

                    'Create new version of files
                    Dim emailBody_Html As String = emailBody_Html_original
                    Dim emailBody_Text As String = emailBody_Text_original

                    'Insert data into page
                    emailBody_Html = emailBody_Html.Replace("[NAME]", userName)
                    emailBody_Html = emailBody_Html.Replace("[MSG]", message)
                    emailBody_Html = emailBody_Html.Replace("[YEAR]", Date.Today.Year)

                    emailBody_Text = emailBody_Text.Replace("[NAME]", userName)
                    emailBody_Text = emailBody_Text.Replace("[MSG]", message)
                    emailBody_Text = emailBody_Text.Replace("[YEAR]", Date.Today.Year)

                    'Create mail message
                    Dim Msg As MailMessage = New MailMessage With {.From = New MailAddress(System.Configuration.ConfigurationManager.AppSettings("SMTPusername").ToString())}
                    Msg.To.Add(New MailAddress(userEmail))
                    If Not String.IsNullOrEmpty(clsMember.MembershipProperties.altEmail) Then
                        Try
                            Msg.To.Add(New MailAddress(clsMember.MembershipProperties.altEmail))
                        Catch ex As Exception
                            Dim sb As New StringBuilder()
                            sb.AppendLine("blEmails.vb")
                            sb.AppendLine("Alt email failed.")
                            sb.AppendLine("User Id: " & clsMember.MembershipProperties.userId)
                            sb.AppendLine("Name: " & clsMember.Demographics.firstName & " " & clsMember.Demographics.lastName)
                            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
                        End Try
                    End If

                    'Set email parameters
                    Msg.BodyEncoding = Encoding.UTF8
                    Msg.SubjectEncoding = Encoding.UTF8
                    Msg.Subject = "My Window of Opportunity- Pledge refunded"
                    Msg.IsBodyHtml = True
                    Msg.Body = ""

                    Dim alternateHtml As AlternateView = AlternateView.CreateAlternateViewFromString(emailBody_Html, New Mime.ContentType(MediaTypeNames.Text.Html))
                    Dim alternateText As AlternateView = AlternateView.CreateAlternateViewFromString(emailBody_Text, New Mime.ContentType(MediaTypeNames.Text.Plain))

                    Msg.AlternateViews.Add(alternateText)
                    Msg.AlternateViews.Add(alternateHtml)

                    'Send email
                    smtp.Send(Msg)
                End If
            End If

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("blEmails.vb: sendCampaignEmail_byPledge_refundedTransaction")
            sb.AppendLine("ipPledge: " & ipPledge.Id)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
    End Sub
    Public Sub sendCampaignEmail_byLstTeamAdmins_refundedTransaction(ByVal lstTeamAdministrators As List(Of Integer))
        Try
            'Instantiate variables
            Dim blMembers As New blMembers
            Dim blTeams As New blTeams
            Dim userName As String = String.Empty
            Dim userEmail As String = String.Empty
            Dim message As String = "We are very sorry to inform you that your campaign has not met its goal.  All pledges for this phase have been canceled."

            'Obtain smtp
            Dim smtp As New SmtpClient(System.Configuration.ConfigurationManager.AppSettings("SMTPhost").ToString(), System.Configuration.ConfigurationManager.AppSettings("SMTPport").ToString()) With
                {
                    .Credentials = New NetworkCredential(System.Configuration.ConfigurationManager.AppSettings("SMTPusername").ToString(), System.Configuration.ConfigurationManager.AppSettings("SMTPpassword").ToString())
                }

            'set the content by openning the files.
            Dim filePath_html As String = HttpContext.Current.Request.PhysicalApplicationPath + "Emails\Nightly\Campaign.html"
            Dim filePath_Text As String = HttpContext.Current.Request.PhysicalApplicationPath + "Emails\Nightly\Campaign.txt"
            Dim emailBody_Html_original As String = System.IO.File.ReadAllText(filePath_html)
            Dim emailBody_Text_original As String = System.IO.File.ReadAllText(filePath_Text)



            'Loop thru all admin IDs
            For Each adminId As Integer In lstTeamAdministrators
                'Obtain admin's name
                Dim memberName As String = blMembers.getMemberName_byId(adminId)

                'Create new version of files
                Dim emailBody_Html As String = emailBody_Html_original
                Dim emailBody_Text As String = emailBody_Text_original

                'Insert data into page
                emailBody_Html = emailBody_Html.Replace("[NAME]", memberName)
                emailBody_Html = emailBody_Html.Replace("[MSG]", message)
                emailBody_Html = emailBody_Html.Replace("[YEAR]", Date.Today.Year)

                emailBody_Text = emailBody_Text.Replace("[NAME]", memberName)
                emailBody_Text = emailBody_Text.Replace("[MSG]", message)
                emailBody_Text = emailBody_Text.Replace("[YEAR]", Date.Today.Year)

                'Create mail message
                Dim Msg As MailMessage = New MailMessage With {.From = New MailAddress(System.Configuration.ConfigurationManager.AppSettings("SMTPusername").ToString())}
                Msg.To.Add(New MailAddress(blMembers.getMemberEmail_byId(adminId)))
                Try
                    Dim altEmail As String = blMembers.getUsersAltEmail_byId(adminId)
                    If Not String.IsNullOrEmpty(altEmail) Then
                        Msg.To.Add(New MailAddress(altEmail))
                    End If
                Catch ex As Exception
                    Dim sb As New StringBuilder()
                    sb.AppendLine("blEmails.vb")
                    sb.AppendLine("Alt email failed.")
                    sb.AppendLine("User Id: " & adminId)
                    sb.AppendLine("Name: " & memberName)
                    saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
                End Try

                'Set email parameters
                Msg.BodyEncoding = Encoding.UTF8
                Msg.SubjectEncoding = Encoding.UTF8
                Msg.Subject = "My Window of Opportunity- Campaign phase did not reach goal"
                Msg.IsBodyHtml = True
                Msg.Body = ""

                Dim alternateHtml As AlternateView = AlternateView.CreateAlternateViewFromString(emailBody_Html, New Mime.ContentType(MediaTypeNames.Text.Html))
                Dim alternateText As AlternateView = AlternateView.CreateAlternateViewFromString(emailBody_Text, New Mime.ContentType(MediaTypeNames.Text.Plain))

                Msg.AlternateViews.Add(alternateText)
                Msg.AlternateViews.Add(alternateHtml)

                'Send email
                smtp.Send(Msg)
            Next

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("blEmails.vb: sendCampaignEmail_byLstTeamAdmins_refundedTransaction")
            sb.AppendLine("lstTeamAdministrators: " & String.Join(",", lstTeamAdministrators.ToArray()))
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
    End Sub
    Public Sub sendCampaignEmail_byLstTeamAdmins_DiscoveryPhaseSuccessful(ByVal lstTeamAdministrators As List(Of Integer))
        Try
            'Instantiate variables
            Dim blMembers As New blMembers
            Dim blTeams As New blTeams
            Dim userName As String = String.Empty
            Dim userEmail As String = String.Empty
            Dim message As String = "Congratulations, your Discovery Phase is complete!!  Please log in to review any comments that have been submitted to your campaign."

            'Obtain smtp
            Dim smtp As New SmtpClient(System.Configuration.ConfigurationManager.AppSettings("SMTPhost").ToString(), System.Configuration.ConfigurationManager.AppSettings("SMTPport").ToString()) With
                {
                    .Credentials = New NetworkCredential(System.Configuration.ConfigurationManager.AppSettings("SMTPusername").ToString(), System.Configuration.ConfigurationManager.AppSettings("SMTPpassword").ToString())
                }

            'set the content by openning the files.
            Dim filePath_html As String = HttpContext.Current.Request.PhysicalApplicationPath + "Emails\Nightly\Campaign.html"
            Dim filePath_Text As String = HttpContext.Current.Request.PhysicalApplicationPath + "Emails\Nightly\Campaign.txt"
            Dim emailBody_Html_original As String = System.IO.File.ReadAllText(filePath_html)
            Dim emailBody_Text_original As String = System.IO.File.ReadAllText(filePath_Text)



            'Loop thru all admin IDs
            For Each adminId As Integer In lstTeamAdministrators
                'Obtain admin's name
                Dim memberName As String = blMembers.getMemberName_byId(adminId)

                'Create new version of files
                Dim emailBody_Html As String = emailBody_Html_original
                Dim emailBody_Text As String = emailBody_Text_original

                'Insert data into page
                emailBody_Html = emailBody_Html.Replace("[NAME]", memberName)
                emailBody_Html = emailBody_Html.Replace("[MSG]", message)
                emailBody_Html = emailBody_Html.Replace("[YEAR]", Date.Today.Year)

                emailBody_Text = emailBody_Text.Replace("[NAME]", memberName)
                emailBody_Text = emailBody_Text.Replace("[MSG]", message)
                emailBody_Text = emailBody_Text.Replace("[YEAR]", Date.Today.Year)

                'Create mail message
                Dim Msg As MailMessage = New MailMessage With {.From = New MailAddress(System.Configuration.ConfigurationManager.AppSettings("SMTPusername").ToString())}
                Msg.To.Add(New MailAddress(blMembers.getMemberEmail_byId(adminId)))
                Try
                    Dim altEmail As String = blMembers.getUsersAltEmail_byId(adminId)
                    If Not String.IsNullOrEmpty(altEmail) Then
                        Msg.To.Add(New MailAddress(altEmail))
                    End If
                Catch ex As Exception
                    Dim sb As New StringBuilder()
                    sb.AppendLine("blEmails.vb")
                    sb.AppendLine("Alt email failed.")
                    sb.AppendLine("User Id: " & adminId)
                    sb.AppendLine("Name: " & memberName)
                    saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
                End Try

                'Set email parameters
                Msg.BodyEncoding = Encoding.UTF8
                Msg.SubjectEncoding = Encoding.UTF8
                Msg.Subject = "My Window of Opportunity- Discovery phase completed successfully"
                Msg.IsBodyHtml = True
                Msg.Body = ""

                Dim alternateHtml As AlternateView = AlternateView.CreateAlternateViewFromString(emailBody_Html, New Mime.ContentType(MediaTypeNames.Text.Html))
                Dim alternateText As AlternateView = AlternateView.CreateAlternateViewFromString(emailBody_Text, New Mime.ContentType(MediaTypeNames.Text.Plain))

                Msg.AlternateViews.Add(alternateText)
                Msg.AlternateViews.Add(alternateHtml)

                'Send email
                smtp.Send(Msg)
            Next

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("blEmails.vb: sendCampaignEmail_byLstTeamAdmins_refundedTransaction")
            sb.AppendLine("lstTeamAdministrators: " & String.Join(",", lstTeamAdministrators.ToArray()))
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
    End Sub
    Public Sub sendCampaignEmail_byLstTeamAdmins_SuccessfulCampaign(ByVal lstTeamAdministrators As List(Of Integer))
        Try
            'Instantiate variables
            Dim blMembers As New blMembers
            Dim blTeams As New blTeams
            Dim userName As String = String.Empty
            Dim userEmail As String = String.Empty
            Dim message As String = "Congratulations!!!  You have successfully completed your campaign as of " & Date.Today.ToLongDateString & ".  Please allow 5-7 business days for funds to be transfered to your account."

            'Obtain smtp
            Dim smtp As New SmtpClient(System.Configuration.ConfigurationManager.AppSettings("SMTPhost").ToString(), System.Configuration.ConfigurationManager.AppSettings("SMTPport").ToString()) With
                {
                    .Credentials = New NetworkCredential(System.Configuration.ConfigurationManager.AppSettings("SMTPusername").ToString(), System.Configuration.ConfigurationManager.AppSettings("SMTPpassword").ToString())
                }

            'set the content by openning the files.
            Dim filePath_html As String = HttpContext.Current.Request.PhysicalApplicationPath + "Emails\Nightly\Campaign.html"
            Dim filePath_Text As String = HttpContext.Current.Request.PhysicalApplicationPath + "Emails\Nightly\Campaign.txt"
            Dim emailBody_Html_original As String = System.IO.File.ReadAllText(filePath_html)
            Dim emailBody_Text_original As String = System.IO.File.ReadAllText(filePath_Text)



            'Loop thru all admin IDs
            For Each adminId As Integer In lstTeamAdministrators
                'Obtain admin's name
                Dim memberName As String = blMembers.getMemberName_byId(adminId)

                'Create new version of files
                Dim emailBody_Html As String = emailBody_Html_original
                Dim emailBody_Text As String = emailBody_Text_original

                'Insert data into page
                emailBody_Html = emailBody_Html.Replace("[NAME]", memberName)
                emailBody_Html = emailBody_Html.Replace("[MSG]", message)
                emailBody_Html = emailBody_Html.Replace("[YEAR]", Date.Today.Year)

                emailBody_Text = emailBody_Text.Replace("[NAME]", memberName)
                emailBody_Text = emailBody_Text.Replace("[MSG]", message)
                emailBody_Text = emailBody_Text.Replace("[YEAR]", Date.Today.Year)

                'Create mail message
                Dim Msg As MailMessage = New MailMessage With {.From = New MailAddress(System.Configuration.ConfigurationManager.AppSettings("SMTPusername").ToString())}
                Msg.To.Add(New MailAddress(blMembers.getMemberEmail_byId(adminId)))
                Try
                    Dim altEmail As String = blMembers.getUsersAltEmail_byId(adminId)
                    If Not String.IsNullOrEmpty(altEmail) Then
                        Msg.To.Add(New MailAddress(altEmail))
                    End If
                Catch ex As Exception
                    Dim sb As New StringBuilder()
                    sb.AppendLine("blEmails.vb")
                    sb.AppendLine("Alt email failed.")
                    sb.AppendLine("User Id: " & adminId)
                    sb.AppendLine("Name: " & memberName)
                    saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
                End Try

                'Set email parameters
                Msg.BodyEncoding = Encoding.UTF8
                Msg.SubjectEncoding = Encoding.UTF8
                Msg.Subject = "My Window of Opportunity- Discovery phase completed successfully"
                Msg.IsBodyHtml = True
                Msg.Body = ""

                Dim alternateHtml As AlternateView = AlternateView.CreateAlternateViewFromString(emailBody_Html, New Mime.ContentType(MediaTypeNames.Text.Html))
                Dim alternateText As AlternateView = AlternateView.CreateAlternateViewFromString(emailBody_Text, New Mime.ContentType(MediaTypeNames.Text.Plain))

                Msg.AlternateViews.Add(alternateText)
                Msg.AlternateViews.Add(alternateHtml)

                'Send email
                smtp.Send(Msg)
            Next

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("blEmails.vb: sendCampaignEmail_byLstTeamAdmins_refundedTransaction")
            sb.AppendLine("lstTeamAdministrators: " & String.Join(",", lstTeamAdministrators.ToArray()))
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
    End Sub
    Public Sub sendCampaignEmail_byLstTeamAdmins_SuccessfulPhase_LaunchNext(ByVal lstTeamAdministrators As List(Of Integer))
        Try
            'Instantiate variables
            Dim blMembers As New blMembers
            Dim blTeams As New blTeams
            Dim userName As String = String.Empty
            Dim userEmail As String = String.Empty
            Dim message As String = "Congratulations!!!  You have successfully completed the latest phase in your campaign as of " & Date.Today.ToLongDateString & ".  Please allow 5-7 business days for funds to be transfered to your account.  When you are ready, go ahead and launch the next campaign."

            'Obtain smtp
            Dim smtp As New SmtpClient(System.Configuration.ConfigurationManager.AppSettings("SMTPhost").ToString(), System.Configuration.ConfigurationManager.AppSettings("SMTPport").ToString()) With
                {
                    .Credentials = New NetworkCredential(System.Configuration.ConfigurationManager.AppSettings("SMTPusername").ToString(), System.Configuration.ConfigurationManager.AppSettings("SMTPpassword").ToString())
                }

            'set the content by openning the files.
            Dim filePath_html As String = HttpContext.Current.Request.PhysicalApplicationPath + "Emails\Nightly\Campaign.html"
            Dim filePath_Text As String = HttpContext.Current.Request.PhysicalApplicationPath + "Emails\Nightly\Campaign.txt"
            Dim emailBody_Html_original As String = System.IO.File.ReadAllText(filePath_html)
            Dim emailBody_Text_original As String = System.IO.File.ReadAllText(filePath_Text)



            'Loop thru all admin IDs
            For Each adminId As Integer In lstTeamAdministrators
                'Obtain admin's name
                Dim memberName As String = blMembers.getMemberName_byId(adminId)

                'Create new version of files
                Dim emailBody_Html As String = emailBody_Html_original
                Dim emailBody_Text As String = emailBody_Text_original

                'Insert data into page
                emailBody_Html = emailBody_Html.Replace("[NAME]", memberName)
                emailBody_Html = emailBody_Html.Replace("[MSG]", message)
                emailBody_Html = emailBody_Html.Replace("[YEAR]", Date.Today.Year)

                emailBody_Text = emailBody_Text.Replace("[NAME]", memberName)
                emailBody_Text = emailBody_Text.Replace("[MSG]", message)
                emailBody_Text = emailBody_Text.Replace("[YEAR]", Date.Today.Year)

                'Create mail message
                Dim Msg As MailMessage = New MailMessage With {.From = New MailAddress(System.Configuration.ConfigurationManager.AppSettings("SMTPusername").ToString())}
                Msg.To.Add(New MailAddress(blMembers.getMemberEmail_byId(adminId)))

                Dim altEmail As String = blMembers.getUsersAltEmail_byId(adminId)
                If Not String.IsNullOrEmpty(altEmail) Then
                    Try
                        Msg.To.Add(New MailAddress(altEmail))
                    Catch ex As Exception
                        Dim sb As New StringBuilder()
                        sb.AppendLine("blEmails.vb")
                        sb.AppendLine("Alt email failed.")
                        sb.AppendLine("Admin Id: " & adminId)
                        saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
                    End Try
                End If

                'Set email parameters
                Msg.BodyEncoding = Encoding.UTF8
                Msg.SubjectEncoding = Encoding.UTF8
                Msg.Subject = "My Window of Opportunity- Discovery phase completed successfully"
                Msg.IsBodyHtml = True
                Msg.Body = ""

                Dim alternateHtml As AlternateView = AlternateView.CreateAlternateViewFromString(emailBody_Html, New Mime.ContentType(MediaTypeNames.Text.Html))
                Dim alternateText As AlternateView = AlternateView.CreateAlternateViewFromString(emailBody_Text, New Mime.ContentType(MediaTypeNames.Text.Plain))

                Msg.AlternateViews.Add(alternateText)
                Msg.AlternateViews.Add(alternateHtml)

                'Send email
                smtp.Send(Msg)
            Next

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("blEmails.vb: sendCampaignEmail_byLstTeamAdmins_refundedTransaction")
            sb.AppendLine("lstTeamAdministrators: " & String.Join(",", lstTeamAdministrators.ToArray()))
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
    End Sub
    Public Function sendPledgEmail_Successful(ByVal bussReturn As BusinessReturn, stripeCharge As StripeCharge, ByVal _member As Member) As Boolean
        'This function used for to send email to customer once the charge made successfully, Parameter used bussReturn that pass value of object created while making the charge _member parameter used to get member property as like email,name. 

        'Instantiate variables
        Dim uri As String = ConfigurationManager.AppSettings(Miscellaneous.siteUrl) + HttpContext.Current.Request.ApplicationPath
        'Dim uri As String = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + HttpContext.Current.Request.ApplicationPath

        Try
            'Obtain member from return
            Dim pledge As CampaignPledge = DirectCast(bussReturn.DataContainer(0), CampaignPledge)

            'Obtain smtp
            Dim smtp As New SmtpClient(System.Configuration.ConfigurationManager.AppSettings("SMTPhost").ToString(), System.Configuration.ConfigurationManager.AppSettings("SMTPport").ToString()) With
                {
                    .Credentials = New NetworkCredential(System.Configuration.ConfigurationManager.AppSettings("SMTPusername").ToString(), System.Configuration.ConfigurationManager.AppSettings("SMTPpassword").ToString())
                }

            'set the content by openning the files.
            Dim filePath_html As String = HttpContext.Current.Request.PhysicalApplicationPath + "Emails\FinancialHandler\Successful.html"
            Dim filePath_Text As String = HttpContext.Current.Request.PhysicalApplicationPath + "Emails\FinancialHandler\Successful.txt"
            Dim emailBody_Html_original As String = System.IO.File.ReadAllText(filePath_html)
            Dim emailBody_Text_original As String = System.IO.File.ReadAllText(filePath_Text)

            'Create new version of files
            Dim emailBody_Html As String = emailBody_Html_original
            Dim emailBody_Text As String = emailBody_Text_original

            'Insert data into page
            emailBody_Html = emailBody_Html.Replace("[NAME]", _member.Demographics.firstName)
            emailBody_Text = emailBody_Text.Replace("[NAME]", _member.Demographics.firstName)

            emailBody_Html = emailBody_Html.Replace("[LINK]", uri & _uHelper.Get_IPublishedContentByID(siteNodes.EditCampaign).Url & "?" & queryParameters.nodeId & "=" & pledge.campaignId)
            emailBody_Text = emailBody_Text.Replace("[LINK]", uri & _uHelper.Get_IPublishedContentByID(siteNodes.EditCampaign).Url & "?" & queryParameters.nodeId & "=" & pledge.campaignId)

            emailBody_Html = emailBody_Html.Replace("[PLEDGED]", FormatCurrency(stripeCharge.Amount / 100, 2))
            emailBody_Text = emailBody_Text.Replace("[PLEDGED]", FormatCurrency(stripeCharge.Amount / 100, 2))

            emailBody_Html = emailBody_Html.Replace("[CAMPAIGNNAME]", pledge.campaignName)
            emailBody_Text = emailBody_Text.Replace("[CAMPAIGNNAME]", pledge.campaignName)

            If pledge.showAsAnonymous Then
                emailBody_Html = emailBody_Html.Replace("[MEMBERNAME]", "Anonymous")
                emailBody_Text = emailBody_Text.Replace("[MEMBERNAME]", "Anonymous")
            Else
                emailBody_Html = emailBody_Html.Replace("[MEMBERNAME]", pledge.pledgingMemberName.Split(" ")(0))
                emailBody_Text = emailBody_Text.Replace("[MEMBERNAME]", pledge.pledgingMemberName.Split(" ")(0))
            End If

            emailBody_Html = emailBody_Html.Replace("[YEAR]", Date.Today.Year)
            emailBody_Text = emailBody_Text.Replace("[YEAR]", Date.Today.Year)

            'Create mail message
            Dim Msg As MailMessage = New MailMessage With {.From = New MailAddress(System.Configuration.ConfigurationManager.AppSettings("SMTPusername").ToString())}
            For Each toEmail In getManagementEmails_byCampaignId(pledge.campaignId)
                Msg.To.Add(New MailAddress(toEmail))
            Next

            'Set email parameters
            Msg.BodyEncoding = Encoding.UTF8
            Msg.SubjectEncoding = Encoding.UTF8
            Msg.Subject = "My Window of Opportunity- Pledge Successfully Submitted"
            Msg.IsBodyHtml = True
            Msg.Body = ""

            Dim alternateHtml As AlternateView = AlternateView.CreateAlternateViewFromString(emailBody_Html, New Mime.ContentType(MediaTypeNames.Text.Html))
            Dim alternateText As AlternateView = AlternateView.CreateAlternateViewFromString(emailBody_Text, New Mime.ContentType(MediaTypeNames.Text.Plain))

            Msg.AlternateViews.Add(alternateText)
            Msg.AlternateViews.Add(alternateHtml)

            'Send email
            smtp.Send(Msg)

            Return True
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("FinancialHandler.vb : SendEmail()")
            sb.AppendLine("bussReturn:" & bussReturn.ReturnMessage)
            sb.AppendLine("_MemberId:" & _member.MembershipProperties.userId)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

            Return False
        End Try
    End Function
    Public Function sendVerifyEmailToCreateAcct(ByVal encryptedUrl As String, ByVal userName As String, ByVal userEmail As String) As Boolean
        'Instantiate variables
        'Dim pageUri As Uri = HttpContext.Current.Request.Url
        'encryptedUrl = pageUri.Scheme & "://" & pageUri.Host & encryptedUrl


        Try
            'Obtain smtp
            Dim smtp As New SmtpClient(System.Configuration.ConfigurationManager.AppSettings("SMTPhost").ToString(), System.Configuration.ConfigurationManager.AppSettings("SMTPport").ToString()) With
                {
                    .Credentials = New NetworkCredential(System.Configuration.ConfigurationManager.AppSettings("SMTPusername").ToString(), System.Configuration.ConfigurationManager.AppSettings("SMTPpassword").ToString())
                }

            'set the content by openning the files.
            Dim filePath_html As String = HttpContext.Current.Request.PhysicalApplicationPath + "Emails\VerifyEmailToCreateAcct\VerifyEmailToCreateAcct.html"
            Dim filePath_Text As String = HttpContext.Current.Request.PhysicalApplicationPath + "Emails\VerifyEmailToCreateAcct\VerifyEmailToCreateAcct.txt"
            Dim emailBody_Html_original As String = System.IO.File.ReadAllText(filePath_html)
            Dim emailBody_Text_original As String = System.IO.File.ReadAllText(filePath_Text)

            'Create new version of files
            Dim emailBody_Html As String = emailBody_Html_original
            Dim emailBody_Text As String = emailBody_Text_original

            'Insert data into page
            emailBody_Html = emailBody_Html.Replace("[NAME]", userName)
            emailBody_Html = emailBody_Html.Replace("[LINK]", encryptedUrl)
            emailBody_Html = emailBody_Html.Replace("[YEAR]", Date.Today.Year)

            emailBody_Text = emailBody_Text.Replace("[NAME]", userName)
            emailBody_Text = emailBody_Text.Replace("[LINK]", encryptedUrl)
            emailBody_Text = emailBody_Text.Replace("[YEAR]", Date.Today.Year)

            'Create mail message
            Dim Msg As MailMessage = New MailMessage With {.From = New MailAddress(System.Configuration.ConfigurationManager.AppSettings("SMTPusername").ToString())}
            Msg.To.Add(New MailAddress(userEmail))

            'Set email parameters
            Msg.BodyEncoding = Encoding.UTF8
            Msg.SubjectEncoding = Encoding.UTF8
            Msg.Subject = "My Window of Opportunity- Email Verification"
            Msg.IsBodyHtml = True
            Msg.Body = ""

            Dim alternateHtml As AlternateView = AlternateView.CreateAlternateViewFromString(emailBody_Html, New Mime.ContentType(MediaTypeNames.Text.Html))
            Dim alternateText As AlternateView = AlternateView.CreateAlternateViewFromString(emailBody_Text, New Mime.ContentType(MediaTypeNames.Text.Plain))

            Msg.AlternateViews.Add(alternateText)
            Msg.AlternateViews.Add(alternateHtml)

            'Send email
            smtp.Send(Msg)

            Return True
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\BecomeAMember.ascx.vb : sendEmail()")
            sb.AppendLine("encryptedUrl:" & encryptedUrl)
            sb.AppendLine("userName:" & userName)
            sb.AppendLine("userEmail:" & userEmail)
            ' Response.Write(ex.ToString)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            ''encryptedUrl As String, ByVal userName As String, ByVal userEmail
            'Response.Write("<br />encryptedUrl = " & encryptedUrl)
            'Response.Write("<br />userName = " & userName)
            'Response.Write("<br />userEmail = " & userEmail)

            Return False
        End Try
    End Function
    Public Function SendResetPasswordEmail(ByVal encryptedUrl As String, ByVal userName As String, ByVal userEmail As String) As Boolean
        Try
            'Instantiate variables
            'Dim uri As String = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + HttpContext.Current.Request.ApplicationPath
            'Dim pageUri As Uri = HttpContext.Current.Request.Url
            'encryptedUrl = pageUri.Scheme & "://" & pageUri.Host & "/" & encryptedUrl
            'encryptedUrl = "http://" + encryptedUrl

            'Obtain smtp
            Dim smtp As New SmtpClient(System.Configuration.ConfigurationManager.AppSettings("SMTPhost").ToString(), System.Configuration.ConfigurationManager.AppSettings("SMTPport").ToString()) With
                {
                    .Credentials = New NetworkCredential(System.Configuration.ConfigurationManager.AppSettings("SMTPusername").ToString(), System.Configuration.ConfigurationManager.AppSettings("SMTPpassword").ToString())
                }

            'set the content by openning the files.
            Dim filePath_html As String = HttpContext.Current.Request.PhysicalApplicationPath + "Emails\ResetPassword\ResetPassword.html"
            Dim filePath_Text As String = HttpContext.Current.Request.PhysicalApplicationPath + "Emails\ResetPassword\ResetPassword.txt"
            Dim emailBody_Html_original As String = System.IO.File.ReadAllText(filePath_html)
            Dim emailBody_Text_original As String = System.IO.File.ReadAllText(filePath_Text)

            'Create new version of files
            Dim emailBody_Html As String = emailBody_Html_original
            Dim emailBody_Text As String = emailBody_Text_original

            'Insert data into page
            emailBody_Html = emailBody_Html.Replace("[NAME]", userName)
            emailBody_Text = emailBody_Text.Replace("[NAME]", userName)

            emailBody_Html = emailBody_Html.Replace("[LINK]", encryptedUrl)
            emailBody_Text = emailBody_Text.Replace("[LINK]", encryptedUrl)

            emailBody_Html = emailBody_Html.Replace("[YEAR]", Date.Today.Year)
            emailBody_Text = emailBody_Text.Replace("[YEAR]", Date.Today.Year)

            'Create mail message
            Dim Msg As MailMessage = New MailMessage With {.From = New MailAddress(System.Configuration.ConfigurationManager.AppSettings("SMTPusername").ToString())}
            Msg.To.Add(New MailAddress(userEmail))

            'Set email parameters
            Msg.BodyEncoding = Encoding.UTF8
            Msg.SubjectEncoding = Encoding.UTF8
            Msg.Subject = "My Window of Opportunity- Reset Password"
            Msg.IsBodyHtml = True
            Msg.Body = ""

            Dim alternateHtml As AlternateView = AlternateView.CreateAlternateViewFromString(emailBody_Html, New Mime.ContentType(MediaTypeNames.Text.Html))
            Dim alternateText As AlternateView = AlternateView.CreateAlternateViewFromString(emailBody_Text, New Mime.ContentType(MediaTypeNames.Text.Plain))

            Msg.AlternateViews.Add(alternateText)
            Msg.AlternateViews.Add(alternateHtml)

            'Send email
            smtp.Send(Msg)

            Return True
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\ResetPassword..ascx.vb : SendEmail()")
            sb.AppendLine("encryptedUrl:" & encryptedUrl)
            sb.AppendLine("userName:" & userName)
            sb.AppendLine("userEmail:" & userEmail)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Response.Write(ex.ToString)
            Return False
        End Try
    End Function
    Public Function sendEmail_ConfirmMembership(ByVal newEmailAddress As String,
                                                ByVal role As String,
                                                ByVal uriAuthority As String,
                                                ByVal nodeId As String) As Boolean
        Try
            'Instantiate variables
            Dim toAddress As String = String.Empty
            Dim mailSubject As String = String.Empty
            Dim jsonLink As String
            Dim invitationLink As String = String.Empty
            Dim campaignOrTeamName As String = String.Empty
            Dim filePath_html As String = String.Empty
            Dim filePath_Text As String = String.Empty

            'Create proper invitation link
            Select Case _uHelper.Get_IPublishedContentByID(UmbracoContext.Current.PageId).DocumentTypeAlias
                Case docTypes.Team
                    'Obtain json data
                    jsonLink = createEncryptedJsonLink_EditTeam(newEmailAddress, role)
                    'Create link
                    invitationLink = uriAuthority & Miscellaneous.handler_CampaignInvitation & "?" & queryParameters.params_teamInvitation & "=" & jsonLink
                    'Obtain email path
                    filePath_html = HttpContext.Current.Request.PhysicalApplicationPath + "Emails\AcceptInvitation-TeamAdmin\AcceptInvitation.html"
                    filePath_Text = HttpContext.Current.Request.PhysicalApplicationPath + "Emails\AcceptInvitation-TeamAdmin\AcceptInvitation.txt"

                    campaignOrTeamName = _uHelper.Get_IPublishedContentByID(UmbracoContext.Current.PageId).Name

                Case docTypes.editCampaign
                    'Obtain json data
                    jsonLink = createEncryptedJsonLink_EditCampaign(newEmailAddress, role, nodeId)
                    'Create link
                    invitationLink = uriAuthority & Miscellaneous.handler_CampaignInvitation & "?" & queryParameters.params_campaignInvitation & "=" & jsonLink
                    'Obtain email path
                    filePath_html = HttpContext.Current.Request.PhysicalApplicationPath + "Emails\AcceptInvitation\AcceptInvitation.html"
                    filePath_Text = HttpContext.Current.Request.PhysicalApplicationPath + "Emails\AcceptInvitation\AcceptInvitation.txt"

                    campaignOrTeamName = _uHelper.Get_IPublishedContentByID(nodeId).Name
            End Select


            'Obtain smtp
            Dim smtp As New SmtpClient(System.Configuration.ConfigurationManager.AppSettings("SMTPhost").ToString(), System.Configuration.ConfigurationManager.AppSettings("SMTPport").ToString()) With
                {
                    .Credentials = New NetworkCredential(System.Configuration.ConfigurationManager.AppSettings("SMTPusername").ToString(), System.Configuration.ConfigurationManager.AppSettings("SMTPpassword").ToString())
                }

            'set the content by openning the files.
            Dim emailBody_Html_original As String = System.IO.File.ReadAllText(filePath_html)
            Dim emailBody_Text_original As String = System.IO.File.ReadAllText(filePath_Text)

            'Create new version of files
            Dim emailBody_Html As String = emailBody_Html_original
            Dim emailBody_Text As String = emailBody_Text_original

            'Insert data into page
            emailBody_Html = emailBody_Html.Replace("[LINK]", invitationLink)
            emailBody_Text = emailBody_Text.Replace("[LINK]", invitationLink)

            emailBody_Html = emailBody_Html.Replace("[NAME]", campaignOrTeamName)
            emailBody_Text = emailBody_Text.Replace("[NAME]", campaignOrTeamName)

            emailBody_Html = emailBody_Html.Replace("[YEAR]", Date.Today.Year)
            emailBody_Text = emailBody_Text.Replace("[YEAR]", Date.Today.Year)

            'Create mail message
            Dim Msg As MailMessage = New MailMessage With {.From = New MailAddress(System.Configuration.ConfigurationManager.AppSettings("SMTPusername").ToString())}
            Msg.To.Add(New MailAddress(newEmailAddress))

            'Set email parameters
            Msg.BodyEncoding = Encoding.UTF8
            Msg.SubjectEncoding = Encoding.UTF8
            Msg.Subject = "My Window of Opportunity- You are invited to become a campaign member"
            Msg.IsBodyHtml = True
            Msg.Body = ""

            Dim alternateHtml As AlternateView = AlternateView.CreateAlternateViewFromString(emailBody_Html, New Mime.ContentType(MediaTypeNames.Text.Html))
            Dim alternateText As AlternateView = AlternateView.CreateAlternateViewFromString(emailBody_Text, New Mime.ContentType(MediaTypeNames.Text.Plain))

            Msg.AlternateViews.Add(alternateText)
            Msg.AlternateViews.Add(alternateHtml)

            'Send email
            smtp.Send(Msg)

            Return True
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\TeamMemberEntry.ascx.vb : sendEmail_ConfirmMembership()")


            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Response.Write(ex.ToString)
            'Session(Sessions.errorMsg) = ex.ToString
            Return False
        End Try
    End Function
    Public Function sendContactUsEmail(ByVal userName As String, ByVal userEmail As String, ByVal phonenumber As String, ByVal subject As String, ByVal message As String) As Boolean
        Try
            'Instantiate variables

            'Obtain smtp
            Dim smtp As New SmtpClient(System.Configuration.ConfigurationManager.AppSettings("SMTPhost").ToString(), System.Configuration.ConfigurationManager.AppSettings("SMTPport").ToString()) With
                {
                    .Credentials = New NetworkCredential(System.Configuration.ConfigurationManager.AppSettings("SMTPusername").ToString(), System.Configuration.ConfigurationManager.AppSettings("SMTPpassword").ToString())
                }

            'set the content by openning the files.
            Dim filePath_html As String = HttpContext.Current.Request.PhysicalApplicationPath + "Emails\ContactUs\ContactUs.html"
            Dim filePath_Text As String = HttpContext.Current.Request.PhysicalApplicationPath + "Emails\ContactUs\ContactUs.txt"
            Dim emailBody_Html As String = System.IO.File.ReadAllText(filePath_html)
            Dim emailBody_Text As String = System.IO.File.ReadAllText(filePath_Text)

            'Insert data into page
            emailBody_Html = emailBody_Html.Replace("[NAME]", "Administrator")
            emailBody_Text = emailBody_Text.Replace("[NAME]", "Administrator")

            emailBody_Html = emailBody_Html.Replace("[USERNAME]", userName)
            emailBody_Text = emailBody_Text.Replace("[USERNAME]", userName)

            emailBody_Html = emailBody_Html.Replace("[USEREMAIL]", userEmail)
            emailBody_Text = emailBody_Text.Replace("[USEREMAIL]", userEmail)

            emailBody_Html = emailBody_Html.Replace("[USERPHONE]", phonenumber)
            emailBody_Text = emailBody_Text.Replace("[USERPHONE]", phonenumber)

            emailBody_Html = emailBody_Html.Replace("[SUBJECT]", subject)
            emailBody_Text = emailBody_Text.Replace("[SUBJECT]", subject)

            emailBody_Html = emailBody_Html.Replace("[USERMESSAGE]", message)
            emailBody_Text = emailBody_Text.Replace("[USERMESSAGE]", message)

            emailBody_Html = emailBody_Html.Replace("[YEAR]", Date.Today.Year)
            emailBody_Text = emailBody_Text.Replace("[YEAR]", Date.Today.Year)

            'Create mail message
            Dim Msg As MailMessage = New MailMessage With {
                .From = New MailAddress(System.Configuration.ConfigurationManager.AppSettings("SMTPusername").ToString())
            }
            Msg.To.Add(New MailAddress(System.Configuration.ConfigurationManager.AppSettings("SMTPusername").ToString()))

            'Set email parameters
            Msg.BodyEncoding = Encoding.Default
            Msg.SubjectEncoding = Encoding.Default
            Msg.Subject = "My Window of Opportunity- Contact Us Enquiry"
            Msg.IsBodyHtml = True
            Msg.Body = ""

            Dim alternateHtml As AlternateView = AlternateView.CreateAlternateViewFromString(emailBody_Html, New Mime.ContentType(MediaTypeNames.Text.Html))
            Dim alternateText As AlternateView = AlternateView.CreateAlternateViewFromString(emailBody_Text, New Mime.ContentType(MediaTypeNames.Text.Plain))

            Msg.AlternateViews.Add(alternateText)
            Msg.AlternateViews.Add(alternateHtml)

            'Send email
            smtp.Send(Msg)

            Return True
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("blEmails.vb : sendContactUsEmail()")
            sb.AppendLine("userName:" & userName)
            sb.AppendLine("userEmail:" & userEmail)
            sb.AppendLine("phonenumber:" & phonenumber)
            sb.AppendLine("subject:" & subject)
            sb.AppendLine("message:" & message)

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            '   Response.Write(ex.ToString)
            Return False
        End Try
    End Function
    Public Sub sendReportSuspiciousActivityEmail(ByVal message As String)
        Try
            'Obtain smtp
            Dim smtp As New SmtpClient(System.Configuration.ConfigurationManager.AppSettings("SMTPhost").ToString(), System.Configuration.ConfigurationManager.AppSettings("SMTPport").ToString()) With
                {
                    .Credentials = New NetworkCredential(System.Configuration.ConfigurationManager.AppSettings("SMTPusername").ToString(), System.Configuration.ConfigurationManager.AppSettings("SMTPpassword").ToString())
                }

            'set the content by openning the files.
            Dim filePath_html As String = HttpContext.Current.Request.PhysicalApplicationPath + "Emails\ContactUs\ReportCampaign.html"
            Dim filePath_Text As String = HttpContext.Current.Request.PhysicalApplicationPath + "Emails\ContactUs\ReportCampaign.txt"
            Dim emailBody_Html As String = System.IO.File.ReadAllText(filePath_html)
            Dim emailBody_Text As String = System.IO.File.ReadAllText(filePath_Text)

            'Insert data into page
            emailBody_Html = emailBody_Html.Replace("[USERMESSAGE]", message)
            emailBody_Text = emailBody_Text.Replace("[USERMESSAGE]", message)

            emailBody_Html = emailBody_Html.Replace("[YEAR]", Date.Today.Year)
            emailBody_Text = emailBody_Text.Replace("[YEAR]", Date.Today.Year)

            'Create mail message
            Dim Msg As MailMessage = New MailMessage With {
                .From = New MailAddress(System.Configuration.ConfigurationManager.AppSettings("SMTPusername").ToString())
            }
            Msg.To.Add(New MailAddress(System.Configuration.ConfigurationManager.AppSettings("SMTPusername").ToString()))

            'Set email parameters
            Msg.BodyEncoding = Encoding.Default
            Msg.SubjectEncoding = Encoding.Default
            Msg.Subject = "My Window of Opportunity- Suspicious Activity"
            Msg.IsBodyHtml = True
            Msg.Body = ""

            Dim alternateHtml As AlternateView = AlternateView.CreateAlternateViewFromString(emailBody_Html, New Mime.ContentType(MediaTypeNames.Text.Html))
            Dim alternateText As AlternateView = AlternateView.CreateAlternateViewFromString(emailBody_Text, New Mime.ContentType(MediaTypeNames.Text.Plain))

            Msg.AlternateViews.Add(alternateText)
            Msg.AlternateViews.Add(alternateHtml)

            'Send email
            smtp.Send(Msg)

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("blEmails.vb : sendReportSuspiciousActivityEmail()")
            sb.AppendLine("message:" & message)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try
    End Sub
#End Region

#Region "Private Methods"
    Private Function createEncryptedJsonLink_EditCampaign(ByVal newEmailAddress As String, ByVal role As String, ByVal nodeId As String) As String
        Try

            'Set up temp data
            Dim campaignInvitation As CampaignInvitation = New CampaignInvitation With {
            .email = newEmailAddress,
            .campaignId = nodeId,
            .dateSubmitted = DateTime.Now,
            .roleValue = role
        }

            'Convert class to json
            Dim jsonResult As String = Newtonsoft.Json.JsonConvert.SerializeObject(campaignInvitation)

            Return WebUtility.UrlEncode(jsonResult)

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\BusinessLayer\blMedia.vb : createEncryptedJsonLink_EditCampaign()")
            sb.AppendLine("newEmailAddress:" & newEmailAddress)
            sb.AppendLine("role:" & role)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return Nothing
        End Try
    End Function
    Private Function createEncryptedJsonLink_EditTeam(ByVal newEmailAddress As String, ByVal role As String) As String
        Try
            'Set up temp data
            Dim teamInvitation As TeamInvitation = New TeamInvitation With {
            .email = newEmailAddress,
            .teamId = UmbracoContext.Current.PageId,  'thisNodeId
            .dateSubmitted = DateTime.Now,
            .roleValue = role
        }

            'Convert class to json
            Dim jsonResult As String = Newtonsoft.Json.JsonConvert.SerializeObject(teamInvitation)

            Return WebUtility.UrlEncode(jsonResult)

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\BusinessLayer\blMedia.vb : createEncryptedJsonLink_EditTeam()")
            sb.AppendLine("newEmailAddress:" & newEmailAddress)
            sb.AppendLine("role:" & role)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return Nothing
        End Try
    End Function
#End Region

End Class