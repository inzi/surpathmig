using AuthorizeNet.Api.Contracts.V1;
using System;
using System.Collections.Generic;

namespace inzibackend.Surpath
{
    public class AuthNetCaptureResultDto
    {
        public string TransactionId { get; set; }

        public string AuthCode { get; set; }
    }

    public class AuthNetPreAuthDto
    {
        public AuthNetSubmit authNetSubmit { get; set; }

        decimal amount { get; set; }
    }

    public class AuthNetSubmit
    {
        public string dataValue { get; set; }

        public string dataDescriptor { get; set; }

        public decimal amount { get; set; }

        public List<Guid> TenantSurpathServiceIds { get; set; } = new List<Guid>();

        public string FirstNameOnCard { get; set; }

        public string LastNameOnCard { get; set; }

        public string CardNameOnCard { get; set; }

        public string CardLastFour { get; set; }

        public string BillingAddress { get; set; }

        public string BillingCity { get; set; }

        public string BillingState { get; set; }

        public string BillingZipCode { get; set; }

        public bool DifferentBillingAddress { get; set; } = false;

        public List<AuthNetItems> AuthNetItems { get; set; } = new List<AuthNetItems>();

        public string TransactionId { get; set; }

        public string AuthCode { get; set; }
    }

    public class AuthNetItems
    {
        public Guid Id { get; set; }

        public string ProductName { get; set; }

        public decimal ProductPrice { get; set; } = 0;

        public int ProductQuantity { get; set; } = 0;

        public decimal ProductTotal { get; set; } = 0;

        public decimal AmountPaid { get; set; } = 0;
    }

    public class AuthNetTransactionResult
    {
        private AuthNetTransactionResult(bool succeeded, createTransactionResponse response, string errorCode, string errorMessage)
        {
            Succeeded = succeeded;
            Response = response;
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }

        public bool Succeeded { get; }

        public string ErrorCode { get; }

        public string ErrorMessage { get; }

        public createTransactionResponse Response { get; }

        public string TransactionId =>
            Response?.transactionResponse?.transId ??
            Response?.transactionResponse?.refTransID;

        public static AuthNetTransactionResult Success(createTransactionResponse response)
        {
            return new AuthNetTransactionResult(true, response, null, null);
        }

        public static AuthNetTransactionResult Failure(string errorCode, string errorMessage, createTransactionResponse response = null)
        {
            return new AuthNetTransactionResult(false, response, errorCode, errorMessage);
        }
    }
}

