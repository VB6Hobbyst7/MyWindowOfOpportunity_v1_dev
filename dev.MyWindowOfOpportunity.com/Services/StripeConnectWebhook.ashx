<%@ WebHandler Language="VB" Class="StripeConnectWebhook" %>

Imports System
Imports System.IO
Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Web.Script.Serialization
Imports Stripe.StripeResponse
Imports Stripe
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports Common




Public Class StripeConnectWebhook : Implements IHttpHandler

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Try
            'Instantiate variables
            Dim blSiteManagement As New blSiteManagement
            Dim jsonMsg As String = New StreamReader(context.Request.InputStream).ReadToEnd()

            'Submit stripe msg to umbraco
            blSiteManagement.parseHookMsg(siteNodes.connectHooks, jsonMsg)

            'Return status code as ok
            context.Response.ContentType = "text/plain"
            context.Response.StatusCode = 200

        Catch ex As Exception
            'Save error message
            Dim sb As New StringBuilder()
            sb.AppendLine("StripeConnectWebhook.ashx: ProcessRequest()")
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())

            'Send error response to stripe
            context.Response.ContentType = "text/plain"
            context.Response.StatusCode = 500
        End Try
    End Sub

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class


'account.application.authorized
'account.application.deauthorized
'account.external_account.created
'account.external_account.deleted
'account.external_account.updated
'account.updated
'application_fee.created
'application_fee.refund.updated
'application_fee.refunded
'balance.available
'batch.request
'charge.captured
'charge.dispute.closed
'charge.dispute.created
'charge.dispute.funds_reinstated
'charge.dispute.funds_withdrawn
'charge.dispute.updated
'charge.expired
'charge.failed
'charge.pending
'charge.refund.updated
'charge.refunded
'charge.succeeded
'charge.updated
'coupon.created
'coupon.deleted
'coupon.updated
'customer.bank_account.deleted
'customer.created
'customer.deleted
'customer.discount.created
'customer.discount.deleted
'customer.discount.updated
'customer.source.created
'customer.source.deleted
'customer.source.expiring
'customer.source.updated
'customer.subscription.created
'customer.subscription.deleted
'customer.subscription.trial_will_end
'customer.subscription.updated
'customer.updated
'File.created
'invoice.created
'invoice.payment_failed
'invoice.payment_succeeded
'invoice.sent
'invoice.upcoming
'invoice.updated
'invoiceitem.created
'invoiceitem.deleted
'invoiceitem.updated
'issuer_fraud_record.created
'order.created
'order.payment_failed
'order.payment_succeeded
'order.updated
'order_return.created
'Payout.canceled
'Payout.created
'Payout.failed
'Payout.paid
'Payout.updated
'plan.created
'plan.deleted
'plan.updated
'product.created
'product.deleted
'product.updated
'recipient.created
'recipient.deleted
'recipient.updated
'review.closed
'review.opened
'sigma.scheduled_query_run.created
'sku.created
'sku.deleted
'sku.updated
'Source.canceled
'Source.chargeable
'Source.failed
'Source.mandate_notification
'Source.transaction.created
'topup.created
'topup.failed
'topup.succeeded
'transfer.created
'transfer.reversed
'transfer.updated


