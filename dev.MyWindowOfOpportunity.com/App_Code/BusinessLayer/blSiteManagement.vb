Imports System.Data
Imports Common
Imports Stripe

Public Class blSiteManagement

#Region "Properties"
    Private linqSiteManagement As linqSiteManagement = New linqSiteManagement
#End Region

#Region "Selects"
    Public Function compiledPage_byNodeId(ByVal _nodeId As Integer) As CompiledPage
        Return linqSiteManagement.compiledPage_byNodeId(_nodeId)
    End Function
    Public Function obtainFooterNav() As BusinessReturn
        Return linqSiteManagement.obtainFooterNav()
    End Function
    Public Function obtainMainNav() As BusinessReturn
        Return linqSiteManagement.obtainMainNav()
    End Function
    Public Function obtainCategoryIcon_byCategory(ByVal _category As String) As String
        'Instantiate scope variabes
        Dim businessReturn As New BusinessReturn

        Try
            'Instantiate variables
            Dim categoryIcon As CategoryIcon
            Dim lstCategoryIcons As List(Of CategoryIcon)


            'Obtain data
            businessReturn = linqSiteManagement.obtainAllCategoryIcons()

            If businessReturn.isValid Then
                'Extract data
                lstCategoryIcons = DirectCast(businessReturn.DataContainer(0), List(Of CategoryIcon))

                'Clear return
                businessReturn = New BusinessReturn

                If Not IsNothing(lstCategoryIcons) Then
                    'Obtain matching icon class and add to return.
                    categoryIcon = (From x In lstCategoryIcons Where x.categoryName.Replace(" ", "") = _category.Replace(" ", "") Select x).FirstOrDefault
                    If Not IsNothing(categoryIcon) Then
                        Return categoryIcon.categoryIconUrl
                    End If
                End If
            End If

            Return String.Empty

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("blSiteManagement.vb : obtainAllCategoryIcons()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

            Return String.Empty
        End Try
    End Function
    Public Function isFeeFreeTimeperiod() As Boolean?
        'Scope variables
        Dim isFeeFree As Boolean = False

        Try
            'Instantiate local variables
            Dim noFeeStartdate As String = ConfigurationManager.AppSettings(Miscellaneous.noFeeStartdate)
            Dim noFeeEnddate As String = ConfigurationManager.AppSettings(Miscellaneous.noFeeEnddate)
            Dim startDate As Date = New Date(1, 1, 1)
            Dim endDate As Date = New Date(1, 1, 1)

            'Determine if we are in a fee-free time period
            If Not String.IsNullOrWhiteSpace(noFeeStartdate) AndAlso IsDate(noFeeStartdate) Then
                If Not String.IsNullOrWhiteSpace(noFeeEnddate) AndAlso IsDate(noFeeEnddate) Then
                    startDate = CDate(noFeeStartdate)
                    endDate = CDate(noFeeEnddate)
                    If Date.Today >= startDate AndAlso Date.Today <= endDate Then
                        isFeeFree = True
                    End If
                End If
            End If
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("blSiteManagement.vb : isFeeFreeTimeperiod()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
        End Try

        Return isFeeFree
    End Function
#End Region

#Region "Inserts"
    Public Sub parseHookMsg(ByVal rootId As Integer, ByVal jsonMsg As String)
        Try
            'Instantiate variables
            Dim lqSiteManagement As New linqSiteManagement
            Dim stripeEvent As StripeEvent

            'Parse stripe event
            stripeEvent = StripeEventUtility.ParseEvent(jsonMsg)  'Dim stripeCharge As StripeCharge = Stripe.Mapper(Of StripeCharge).MapFromJson(stripeEvent.Data.Object.ToString)

            Select Case stripeEvent.Type
                Case StripeEvents.ChargeCaptured,
                 StripeEvents.ChargeDisputeClosed,
                 StripeEvents.ChargeDisputeCreated,
                 StripeEvents.ChargeDisputeFundsReinstated,
                 StripeEvents.ChargeDisputeFundsWithdrawn,
                 StripeEvents.ChargeDisputeUpdated,
                 StripeEvents.ChargeFailed,
                 StripeEvents.ChargePending,
                 StripeEvents.ChargeRefunded,
                 StripeEvents.ChargeRefundUpdated,
                 StripeEvents.ChargeSucceeded,
                 StripeEvents.ChargeUpdated
                    Select Case rootId
                        Case siteNodes.accountHooks
                            lqSiteManagement.createHookMsg(siteNodes.acctCharges, docTypes.charge, jsonMsg, stripeEvent.Type)
                        Case siteNodes.connectHooks
                            lqSiteManagement.createHookMsg(siteNodes.connectCharges, docTypes.charge, jsonMsg, stripeEvent.Type)
                    End Select

                Case StripeEvents.PayoutCanceled,
                   StripeEvents.PayoutCreated,
                   StripeEvents.PayoutFailed,
                   StripeEvents.PayoutPaid,
                   StripeEvents.PayoutUpdated
                    Select Case rootId
                        Case siteNodes.accountHooks
                            lqSiteManagement.createHookMsg(siteNodes.acctPayouts, docTypes.payoutHook, jsonMsg, stripeEvent.Type)
                        Case siteNodes.connectHooks
                            lqSiteManagement.createHookMsg(siteNodes.connectPayouts, docTypes.payoutHook, jsonMsg, stripeEvent.Type)
                    End Select

                Case StripeEvents.TransferCreated,
                  StripeEvents.TransferReversed,
                  StripeEvents.TransferUpdated
                    Select Case rootId
                        Case siteNodes.accountHooks
                            lqSiteManagement.createHookMsg(siteNodes.acctTransfers, docTypes.transfer, jsonMsg, stripeEvent.Type)
                        Case siteNodes.connectHooks
                            lqSiteManagement.createHookMsg(siteNodes.connectTransfers, docTypes.transfer, jsonMsg, stripeEvent.Type)
                    End Select

                Case Else
                    Select Case rootId
                        Case siteNodes.accountHooks
                            lqSiteManagement.createHookMsg(siteNodes.acctGeneric, docTypes.genericHook, jsonMsg, stripeEvent.Type)
                        Case siteNodes.connectHooks
                            lqSiteManagement.createHookMsg(siteNodes.connectGeneric, docTypes.genericHook, jsonMsg, stripeEvent.Type)
                    End Select
            End Select

        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("blSiteManagement.vb : parseHookMsg()")
            sb.AppendLine("rootId:" & rootId)
            sb.AppendLine("jsonMsg:")
            sb.AppendLine(jsonMsg)
            saveErrorMessage(-1, ex.Message, sb.ToString())
        End Try
    End Sub
#End Region

End Class
