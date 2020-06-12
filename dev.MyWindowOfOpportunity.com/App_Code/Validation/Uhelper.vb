Imports umbraco.Core.Models
Imports umbraco.Web
Imports Common

Public Class Uhelper


    Public Function Get_IPublishedContentByID(ByVal _thisNodeId As Integer) As IPublishedContent
        Try
            Dim Helper As UmbracoHelper = New UmbracoHelper(UmbracoContext.Current)
            Dim ipContent As IPublishedContent = Helper.TypedContent(_thisNodeId)
            Return ipContent
        Catch ex As Exception
            'Save error to umbraco
            Dim sb As New StringBuilder
            sb.AppendLine("Uhelper.vb | Get_IPublishedContentByID()")
            sb.AppendLine("_thisNodeId" & _thisNodeId)
            saveErrorMessage(getLoggedInMember(), ex.ToString, sb.ToString)
            Return Nothing
        End Try
    End Function

    Public Function GetUmbracoHelper() As UmbracoHelper
        Return New UmbracoHelper
    End Function

End Class
