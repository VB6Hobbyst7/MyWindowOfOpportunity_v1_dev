Imports Examine
Imports Microsoft.VisualBasic
Imports umbraco.Core
Imports umbraco.Web
Imports umbraco.Web.Search
Imports umbraco.Web.Trees

Public Class RegisterEvents
    Inherits ApplicationEventHandler


    Protected Overrides Sub ApplicationStarted(ByVal umbracoApplication As UmbracoApplicationBase, ByVal applicationContext As ApplicationContext)
        System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12
    End Sub

End Class