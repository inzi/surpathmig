using System.Linq;
using AuthorizeNet.Api.Contracts.V1;
using inzibackend.Surpath.Purchase;
using Newtonsoft.Json;

namespace inzibackend.Surpath.Logging
{
    public static class PaymentLogHelper
    {
        private static string MaskCardLastFour(string lastFour)
        {
            if (string.IsNullOrWhiteSpace(lastFour))
            {
                return "unknown";
            }

            var digits = lastFour.Length <= 4
                ? lastFour
                : lastFour.Substring(lastFour.Length - 4);

            return $"****{digits}";
        }

        private static bool HasToken(AuthNetSubmit submit)
        {
            return submit != null && !string.IsNullOrWhiteSpace(submit.dataValue);
        }

        public static string SummarizePreAuth(string tenantName, PreAuthDto dto)
        {
            if (dto == null)
            {
                return JsonConvert.SerializeObject(new { tenant = tenantName, preAuth = "null" });
            }

            var submit = dto.AuthNetSubmit;
            var summary = new
            {
                tenant = tenantName,
                dto.TransactionId,
                amount = submit?.amount,
                hasToken = HasToken(submit),
                cardLastFour = MaskCardLastFour(submit?.CardLastFour),
                itemCount = submit?.AuthNetItems?.Count ?? 0,
                serviceCount = submit?.TenantSurpathServiceIds?.Count ?? 0,
                differentBilling = submit?.DifferentBillingAddress ?? false
            };

            return JsonConvert.SerializeObject(summary);
        }

        public static string SummarizeAuthNetSubmit(AuthNetSubmit submit, decimal? amount = null)
        {
            if (submit == null)
            {
                return JsonConvert.SerializeObject(new { amount, card = "null" });
            }

            var summary = new
            {
                amount,
                hasToken = HasToken(submit),
                cardLastFour = MaskCardLastFour(submit.CardLastFour),
                itemCount = submit.AuthNetItems?.Count ?? 0,
                serviceCount = submit.TenantSurpathServiceIds?.Count ?? 0,
                differentBilling = submit.DifferentBillingAddress
            };

            return JsonConvert.SerializeObject(summary);
        }

        public static string SummarizeRequest(createTransactionRequest request)
        {
            if (request == null)
            {
                return "request=null";
            }

            var transactionRequest = request.transactionRequest;
            var summary = new
            {
                transactionType = transactionRequest?.transactionType,
                amount = transactionRequest?.amount,
                lineItemCount = transactionRequest?.lineItems?.Length ?? 0,
                hasOpaquePayment = transactionRequest?.payment?.Item is opaqueDataType,
                invoiceNumber = transactionRequest?.order?.invoiceNumber
            };

            return JsonConvert.SerializeObject(summary);
        }

        public static string SummarizeResponse(createTransactionResponse response)
        {
            if (response == null)
            {
                return "response=null";
            }

            var transaction = response.transactionResponse;

            var summary = new
            {
                resultCode = response.messages?.resultCode.ToString(),
                transId = transaction?.transId ?? transaction?.refTransID,
                transaction?.responseCode,
                transaction?.authCode,
                transaction?.avsResultCode,
                transaction?.cvvResultCode,
                messageCodes = transaction?.messages?.Select(m => m.code).ToArray(),
                errorCodes = transaction?.errors?.Select(e => e.errorCode).ToArray(),
                gatewayMessages = response.messages?.message?.Select(m => m.code).ToArray()
            };

            return JsonConvert.SerializeObject(summary);
        }

        public static string DescribeCard(AuthNetSubmit submit)
        {
            return JsonConvert.SerializeObject(new
            {
                cardLastFour = MaskCardLastFour(submit?.CardLastFour),
                hasToken = HasToken(submit)
            });
        }
    }
}
