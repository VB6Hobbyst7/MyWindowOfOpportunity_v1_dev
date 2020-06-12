Imports Common


Partial Class UserControls_SocialMediaEntry
    Inherits System.Web.UI.UserControl


#Region "Properties"
    Private blCampaigns As blCampaigns
    'Private _media As Int16
    'Private _nodeId As Int16


    Public Property socialMedia As mediaType_Values
        Get
            Return hfldMediaType.Value
        End Get
        Set(value As mediaType_Values)
            hfldMediaType.Value = value
        End Set
    End Property
    Public Property nodeId As String
        Get
            Return hfldNodeId.Value
        End Get
        Set(value As String)
            hfldNodeId.Value = value
        End Set
    End Property
#End Region

#Region "Handles"
    Private Sub UserControls_SocialMediaEntry_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        'Instantiate variables
        blCampaigns = New blCampaigns

        'Add proper class to panel and title
        Select Case socialMedia
            Case mediaType_Values.Facebook
                pnlAddress.CssClass = mediaType_Strings.Facebook
                txbAddress.Title = mediaType_Strings.Facebook + " Address"
                txbAddress.Text = blCampaigns.selectCampaigsSocialLink_byType(nodeId, mediaType_Values.Facebook)
                lbtnCancel.Attributes.Add(Miscellaneous.mediaType, mediaType_Strings.Facebook)

            Case mediaType_Values.LinkedIn
                pnlAddress.CssClass = mediaType_Strings.LinkedIn
                txbAddress.Title = mediaType_Strings.LinkedIn + " Address"
                txbAddress.Text = blCampaigns.selectCampaigsSocialLink_byType(nodeId, mediaType_Values.LinkedIn)
                lbtnCancel.Attributes.Add(Miscellaneous.mediaType, mediaType_Strings.LinkedIn)

            Case mediaType_Values.Twitter
                pnlAddress.CssClass = mediaType_Strings.Twitter
                txbAddress.Title = mediaType_Strings.Twitter + " Address"
                txbAddress.Text = blCampaigns.selectCampaigsSocialLink_byType(nodeId, mediaType_Values.Twitter)
                lbtnCancel.Attributes.Add(Miscellaneous.mediaType, mediaType_Strings.Twitter)

            Case mediaType_Values.SupportEmail
                pnlAddress.CssClass = mediaType_Strings.SupportEmail
                txbAddress.Title = "Support Email Address"
                txbAddress.Text = blCampaigns.selectCampaigsSocialLink_byType(nodeId, mediaType_Values.SupportEmail)
                lbtnCancel.Attributes.Add(Miscellaneous.mediaType, mediaType_Strings.SupportEmail)



                'Case mediaType_Values.GooglePlus
                '    pnlAddress.CssClass = mediaType_Strings.GooglePlus
                '    txbAddress.Title = mediaType_Strings.GooglePlus + " Address"
                '    txbAddress.Text = blCampaigns.selectCampaigsSocialLink_byType(nodeId, mediaType_Values.GooglePlus)
                '    lbtnCancel.Attributes.Add(Miscellaneous.mediaType, mediaType_Strings.GooglePlus)
        End Select
    End Sub

    Private Sub lbtnSave_Click(sender As Object, e As EventArgs) Handles lbtnSave.Click
        Try
            'Instantiate variables
            blCampaigns = New blCampaigns
            Dim BusinessReturn As BusinessReturn = New BusinessReturn
            Dim mediaType As mediaType_Values = 0

            'Obtain current social media type
            Select Case socialMedia
                Case mediaType_Values.Facebook
                    'BusinessReturn = blCampaigns.UpdateSocialMediaUrl(nodeId, mediaType_Values.Facebook, txbAddress.Text.Trim.ToLower)
                    mediaType = mediaType_Values.Facebook
                Case mediaType_Values.SupportEmail
                    'BusinessReturn = blCampaigns.UpdateSocialMediaUrl(nodeId, mediaType_Values.GooglePlus, txbAddress.Text.Trim.ToLower)
                    mediaType = mediaType_Values.SupportEmail
                Case mediaType_Values.LinkedIn
                    'BusinessReturn = blCampaigns.UpdateSocialMediaUrl(nodeId, mediaType_Values.LinkedIn, txbAddress.Text.Trim.ToLower)
                    mediaType = mediaType_Values.LinkedIn
                Case mediaType_Values.Twitter
                    'BusinessReturn = blCampaigns.UpdateSocialMediaUrl(nodeId, mediaType_Values.Twitter, txbAddress.Text.Trim.ToLower)
                    mediaType = mediaType_Values.Twitter


                    'Case mediaType_Values.GooglePlus
                    '    'BusinessReturn = blCampaigns.UpdateSocialMediaUrl(nodeId, mediaType_Values.GooglePlus, txbAddress.Text.Trim.ToLower)
                    '    mediaType = mediaType_Values.GooglePlus
            End Select

            'Save field data
            BusinessReturn = blCampaigns.UpdateSocialMediaUrl(nodeId, mediaType, txbAddress.Text.Trim.ToLower)


            'Show message and set active tab
            Session(Sessions.socialMediaSavedSuccessfully) = BusinessReturn.isValid


            'Set the tab to open
            setTabCookie(tabNames.content)


            If Not BusinessReturn.isValid Then
                'Response.Write("<br />Error: " & BusinessReturn.ExceptionMessage)
            End If



            ''Display error message
            'If Not BusinessReturn.isValid Then
            '    Response.Write(BusinessReturn.ExceptionMessage)
            'End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\UserControls\SocialMediaEntry.ascx.vb : lbtnSave_Click()")

            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            'Response.Write("Error: " + ex.ToString)
        End Try
    End Sub

#End Region

#Region "Methods"
#End Region
End Class
