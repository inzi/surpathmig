using inzibackend.Authorization.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers.Bases;
using Microsoft.Extensions.Configuration;
using Abp.Extensions;
using inzibackend.Configuration;
using inzibackend.Surpath.SurpathPay;
using inzibackend.Surpath.Logging;
using Abp.UI;
using inzibackend.Web;
using Twilio.Http;

namespace inzibackend.Surpath
{
    public class AuthNetManager : inzibackendDomainServiceBase
    {
        // Surpath22
        //"AuthorizeNet": {
        //    "IsActive": "true",
        //    "UseSandbox": "true",
        //    "ApiLoginID": "2w5zy8AQu",
        //    "ApiTransactionKey": "888gWPkB729s9kAX"
        //    "PublicClientKey": "274DQ5z7g564dqC4CB6UFBvvby57kadP3HJB7EKv4YyVLmaU38R3Z9ttLg2FdMxa"
        //  }


        // Surpath
        //"AuthorizeNet": {
        //  "IsActive": "true",
        //  "UseSandbox": "false",
        //  "ApiLoginID": "2Z7kdS95Ks9",
        //  "ApiTransactionKey": "4s3s3WnbKM6J7k2S",
        //  "PublicClientKey": "44PreAERWDUnPX4Hmj6eFr4W8xrQpBwGrN22MnNr95T8AM3L55gvD7D6jdVF344C"
        //}


        private readonly UserManager _userManager;
        private readonly SurpathPayManager _surpathPayManager;
        private readonly IConfigurationRoot _appConfiguration;

        public AuthNetManager(
            UserManager userManager,
            SurpathPayManager surpathPayManager
            ,IAppConfigurationAccessor configurationAccessor
            )
        {
            _userManager = userManager;
            _surpathPayManager = surpathPayManager;

            _appConfiguration = configurationAccessor.Configuration;
            //_appConfiguration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder(), Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),true);
        }

        //public async Task<bool?> ChargeCreditCardRequest(opaqueDataType opaqueData, long userid, Decimal amount)
        public async Task<bool> ChargeCreditCardRequest(AuthNetSubmit authNetSubmit, long userid, Decimal amount, string ChargeDescription = "")
        {
            Logger.Debug($"AuthNetManager ChargeCreditCardRequest start | userId={userid} | {PaymentLogHelper.SummarizeAuthNetSubmit(authNetSubmit, amount)}");

            var opaqueData = new opaqueDataType
            {
                dataDescriptor = authNetSubmit.dataDescriptor,
                dataValue = authNetSubmit.dataValue

            };
            var UseSandbox = _appConfiguration["Payment:AuthorizeNet:UseSandbox"].To<bool>();
            if (UseSandbox)
                ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;
            else
                ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.PRODUCTION;
            //ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;

            var _user = await _userManager.GetUserByIdAsync(userid);
            var _tenantid = _user.TenantId;
            
            var ApiLoginID = _appConfiguration["Payment:AuthorizeNet:ApiLoginID"];
            var ApiTransactionKey = _appConfiguration["Payment:AuthorizeNet:ApiTransactionKey"];

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = ApiTransactionKey,
            };

            var billingAddress = new customerAddressType
            {
                firstName = _user.Name,
                lastName = _user.Surname,
                address = _user.Address,
                city = _user.City,
                zip = _user.Zip
            };

            //var billingAddress = new customerAddressType
            //{
            //    firstName = "John",
            //    lastName = "Doe",
            //    address = "123 My St",
            //    city = "OurTown",
            //    zip = "98004"
            //};

            //standard api call to retrieve response
            var paymentType = new paymentType { Item = opaqueData };

            // Add line Items
            if (string.IsNullOrEmpty(ChargeDescription))
                ChargeDescription = "Surscan.com";
            var lineItems = new lineItemType[2];
            lineItems[0] = new lineItemType { itemId = "1", name = ChargeDescription, quantity = 1, unitPrice = amount };

            var transactionRequest = new transactionRequestType
            {
                transactionType = transactionTypeEnum.authCaptureTransaction.ToString(),    // charge the card
                amount = amount,
                payment = paymentType,
                billTo = billingAddress,
                lineItems = lineItems,
                
            };
            var request = new createTransactionRequest { transactionRequest = transactionRequest };

            // instantiate the controller that will call the service
            var controller = new createTransactionController(request);
            controller.Execute();

            // get the response from the service (errors contained if any)
            var response = controller.GetApiResponse();


            
            //return response.transactionResponse;
            // validate response
            if (response != null)
            {
                if (response.messages.resultCode == messageTypeEnum.Ok)
                {
                    if (response.transactionResponse.messages != null)
                    {
                        await _surpathPayManager.CreateLedgerEntry(response.transactionResponse, userid, _tenantid, amount, authNetSubmit);
                        Logger.Debug($"AuthNetManager ChargeCreditCardRequest end {userid}");
                        return true;

                        //Console.WriteLine("Successfully created transaction with Transaction ID: " + response.transactionResponse.transId);
                        //Console.WriteLine("Response Code: " + response.transactionResponse.responseCode);
                        //Console.WriteLine("Message Code: " + response.transactionResponse.messages[0].code);
                        //Console.WriteLine("Description: " + response.transactionResponse.messages[0].description);
                        //Console.WriteLine("Success, Auth Code : " + response.transactionResponse.authCode);
                    }
                    else
                    {

                        var _failureMsg = L("VerifyPaymentInformation") + "<br>" + response.transactionResponse.errors[0].errorText + "<br>" + response.transactionResponse.errors[0].errorCode;

                        throw new UserFriendlyException(L("PaymentFailed"), _failureMsg);
                        Logger.Debug($"AuthNetManager ChargeCreditCardRequest end {userid}");
                        return false;
                        //Console.WriteLine("Failed Transaction.");
                        //if (response.transactionResponse.errors != null)
                        //{
                        //    Console.WriteLine("Error Code: " + response.transactionResponse.errors[0].errorCode);
                        //    Console.WriteLine("Error message: " + response.transactionResponse.errors[0].errorText);
                        //}
                    }
                }
                else
                {
                    
                    
                    var _failureMsg = L("VerifyPaymentInformation") + "<br>" + response.transactionResponse.errors[0].errorText + "<br>" + response.transactionResponse.errors[0].errorCode;

                    Logger.Error("Failed Transaction.");
                    Logger.Error(_user.Id.ToString());
                    Logger.Error(_user.UserName + " " + _user.Surname);
                    Logger.Error("Amount: " + string.Format("{0:C}", amount));
                    Logger.Error(_user.EmailAddress);
                    
                    if (response.transactionResponse != null && response.transactionResponse.errors != null)
                    {
                        Logger.Error("Error Code: " + response.transactionResponse.errors[0].errorCode);
                        Logger.Error("Error message: " + response.transactionResponse.errors[0].errorText);
                    }
                    else
                    {
                        Logger.Error("Error Code: " + response.messages.message[0].code);
                        Logger.Error("Error message: " + response.messages.message[0].text);
                    }
                    Logger.Debug($"AuthNetManager ChargeCreditCardRequest end {userid}");

                    return false;
                }
            }
            else
            {
                Logger.Debug($"AuthNetManager ChargeCreditCardRequest end {userid}");
                return false;
            }
            //return false;
        }
        public async Task<createTransactionResponse> ChargeCreditCardRequest(AuthNetSubmit authNetSubmit, Decimal amount, string ChargeDescription = "")
        {
            var opaqueData = new opaqueDataType
            {
                dataDescriptor = authNetSubmit.dataDescriptor,
                dataValue = authNetSubmit.dataValue

            };
            var UseSandbox = _appConfiguration["Payment:AuthorizeNet:UseSandbox"].To<bool>();
            if (UseSandbox)
                ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;
            else
                ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.PRODUCTION;
            //ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;


            var ApiLoginID = _appConfiguration["Payment:AuthorizeNet:ApiLoginID"];
            var ApiTransactionKey = _appConfiguration["Payment:AuthorizeNet:ApiTransactionKey"];

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = ApiTransactionKey,
            };

            var billingAddress = new customerAddressType
            {
                firstName = authNetSubmit.FirstNameOnCard,
                lastName = authNetSubmit.LastNameOnCard,
                address = authNetSubmit.BillingAddress,
                city = authNetSubmit.BillingCity,
                state = authNetSubmit.BillingState,
                zip = authNetSubmit.BillingZipCode
            };

            //var billingAddress = new customerAddressType
            //{
            //    firstName = "John",
            //    lastName = "Doe",
            //    address = "123 My St",
            //    city = "OurTown",
            //    zip = "98004"
            //};

            //standard api call to retrieve response
            var paymentType = new paymentType { Item = opaqueData };

            // Add line Items
            if (string.IsNullOrEmpty(ChargeDescription))
                ChargeDescription = "Surscan.com";
            var lineItems = new lineItemType[2];
            lineItems[0] = new lineItemType { itemId = "1", name = ChargeDescription, quantity = 1, unitPrice = amount };
            //lineItems[1] = new lineItemType { itemId = "2", name = "snowboard", quantity = 1, unitPrice = new Decimal(450.00) };

            var transactionRequest = new transactionRequestType
            {
                transactionType = transactionTypeEnum.authCaptureTransaction.ToString(),    // charge the card
                amount = amount,
                payment = paymentType,
                billTo = billingAddress,
                lineItems = lineItems,
                order = new orderType { description = ChargeDescription }

            };
            var request = new createTransactionRequest { transactionRequest = transactionRequest };

            // instantiate the controller that will call the service
            var controller = new createTransactionController(request);
            controller.Execute();

            // get the response from the service (errors contained if any)
            var response = controller.GetApiResponse();



            //return response.transactionResponse;
            // validate response
            if (response != null)
            {
                if (response.messages.resultCode == messageTypeEnum.Ok)
                {
                    if (response.transactionResponse.messages != null)
                    {
                       // await _surpathPayManager.CreateLedgerEntry(response.transactionResponse, userid, _tenantid, amount, authNetSubmit);
                        return response;

                        //Console.WriteLine("Successfully created transaction with Transaction ID: " + response.transactionResponse.transId);
                        //Console.WriteLine("Response Code: " + response.transactionResponse.responseCode);
                        //Console.WriteLine("Message Code: " + response.transactionResponse.messages[0].code);
                        //Console.WriteLine("Description: " + response.transactionResponse.messages[0].description);
                        //Console.WriteLine("Success, Auth Code : " + response.transactionResponse.authCode);
                    }
                    else
                    {

                        //var _failureMsg = L("VerifyPaymentInformation") + "<br>" + response.transactionResponse.errors[0].errorText + "<br>" + response.transactionResponse.errors[0].errorCode;

                        //throw new UserFriendlyException(L("PaymentFailed"), _failureMsg);
                        return response;
                        //Console.WriteLine("Failed Transaction.");
                        //if (response.transactionResponse.errors != null)
                        //{
                        //    Console.WriteLine("Error Code: " + response.transactionResponse.errors[0].errorCode);
                        //    Console.WriteLine("Error message: " + response.transactionResponse.errors[0].errorText);
                        //}
                    }
                }
                else
                {

                    Logger.Error("Registration Failed Transaction.");
                    Logger.Error($"Card: {PaymentLogHelper.DescribeCard(authNetSubmit)}");
                    Logger.Error("Amount: " + string.Format("{0:C}", amount));
                    Logger.Error($"response: {PaymentLogHelper.SummarizeResponse(response)}");
                    if (response.transactionResponse != null && response.transactionResponse.errors != null)
                    {
                        //var _failureMsg = L("VerifyPaymentInformation") + "<br>" + response.transactionResponse.errors[0].errorText + "<br>" + response.transactionResponse.errors[0].errorCode;

                        Logger.Error("Error Code: " + response.transactionResponse.errors[0].errorCode);
                        Logger.Error("Error message: " + response.transactionResponse.errors[0].errorText);
                    }
                    else
                    {
                        Logger.Error("Error Code: " + response.messages.message[0].code);
                        Logger.Error("Error message: " + response.messages.message[0].text);
                    }
                    return response;
                }
            }
            else
            {
                //var transactionResponseMessages = new List<transactionResponseMessage>();//  { code = "err", description = "Something went wrong" }[];
                //transactionResponseMessages.Add(new transactionResponseMessage() { code = "err", description = "Something went wrong" });
                throw new UserFriendlyException(L("PaymentFailed"), "Something went wrong!");
            }
            //return false;
        }
        public async Task<AuthNetTransactionResult> CapturePreAuthCreditCardRequest(AuthNetSubmit authNetSubmit, AuthNetCaptureResultDto authNetCaptureResultDto, decimal amount, string chargeDescription = "")
        {
            if (authNetCaptureResultDto == null)
            {
                throw new ArgumentNullException(nameof(authNetCaptureResultDto));
            }

            if (authNetCaptureResultDto.TransactionId.IsNullOrWhiteSpace())
            {
                throw new ArgumentException("A valid transaction id is required to capture a pre-authorization.", nameof(authNetCaptureResultDto));
            }

            chargeDescription = NormalizeChargeDescription(chargeDescription);

            var useSandbox = _appConfiguration["Payment:AuthorizeNet:UseSandbox"].To<bool>();
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = useSandbox
                ? AuthorizeNet.Environment.SANDBOX
                : AuthorizeNet.Environment.PRODUCTION;

            var apiLoginId = _appConfiguration["Payment:AuthorizeNet:ApiLoginID"];
            var apiTransactionKey = _appConfiguration["Payment:AuthorizeNet:ApiTransactionKey"];

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType
            {
                name = apiLoginId,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = apiTransactionKey,
            };

            var lineItems = new[]
            {
                new lineItemType { itemId = "1", name = chargeDescription, quantity = 1, unitPrice = amount }
            };

            var transactionRequest = new transactionRequestType
            {
                transactionType = transactionTypeEnum.priorAuthCaptureTransaction.ToString(),
                amount = amount,
                refTransId = authNetCaptureResultDto.TransactionId,
                lineItems = lineItems,
                order = new orderType { description = chargeDescription }
            };

            var request = new createTransactionRequest { transactionRequest = transactionRequest };
            Logger.Info($"CapturePreAuthCreditCardRequest request={PaymentLogHelper.SummarizeRequest(request)}");

            var controller = new createTransactionController(request);
            controller.Execute();

            var response = controller.GetApiResponse();
            Logger.Info($"CapturePreAuthCreditCardRequest response={PaymentLogHelper.SummarizeResponse(response)}");

            var result = InterpretTransactionResponse("CapturePreAuthCreditCardRequest", response, "Payment gateway returned an unsuccessful response.");

            if (!result.Succeeded && authNetSubmit != null)
            {
                Logger.Error($"Registration Failed Transaction. amount={amount} | {PaymentLogHelper.DescribeCard(authNetSubmit)}");
            }

            return result;
        }

        public Task<AuthNetTransactionResult> VoidPreAuthAsync(string transactionId, string chargeDescription = "")
        {
            if (transactionId.IsNullOrWhiteSpace())
            {
                throw new ArgumentException("Transaction id must be provided.", nameof(transactionId));
            }

            chargeDescription = NormalizeChargeDescription(chargeDescription);

            var useSandbox = _appConfiguration["Payment:AuthorizeNet:UseSandbox"].To<bool>();
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = useSandbox
                ? AuthorizeNet.Environment.SANDBOX
                : AuthorizeNet.Environment.PRODUCTION;

            var apiLoginId = _appConfiguration["Payment:AuthorizeNet:ApiLoginID"];
            var apiTransactionKey = _appConfiguration["Payment:AuthorizeNet:ApiTransactionKey"];

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType
            {
                name = apiLoginId,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = apiTransactionKey,
            };

            var transactionRequest = new transactionRequestType
            {
                transactionType = transactionTypeEnum.voidTransaction.ToString(),
                refTransId = transactionId,
                order = new orderType { description = chargeDescription }
            };

            var request = new createTransactionRequest { transactionRequest = transactionRequest };
            Logger.Info($"VoidPreAuthAsync request={PaymentLogHelper.SummarizeRequest(request)}");

            var controller = new createTransactionController(request);
            controller.Execute();

            var response = controller.GetApiResponse();
            Logger.Info($"VoidPreAuthAsync response={PaymentLogHelper.SummarizeResponse(response)}");

            var result = InterpretTransactionResponse("VoidPreAuthAsync", response, "Unable to void the payment authorization.");

            return Task.FromResult(result);
        }
        public async Task<createTransactionResponse> PreAuthCreditCardRequest(AuthNetSubmit authNetSubmit, Decimal amount, string ChargeDescription = "")
        {
            var cardSummary = PaymentLogHelper.DescribeCard(authNetSubmit);
            Logger.Debug($"AuthNetManager PreAuthCreditCardRequest start | amount={amount} | {cardSummary}");

            var opaqueData = new opaqueDataType
            {
                dataDescriptor = authNetSubmit.dataDescriptor,
                dataValue = authNetSubmit.dataValue

            };
            var UseSandbox = _appConfiguration["Payment:AuthorizeNet:UseSandbox"].To<bool>();
            if (UseSandbox)
                ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;
            else
                ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.PRODUCTION;
            //ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;


            var ApiLoginID = _appConfiguration["Payment:AuthorizeNet:ApiLoginID"];
            var ApiTransactionKey = _appConfiguration["Payment:AuthorizeNet:ApiTransactionKey"];

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = ApiTransactionKey,
            };

            var billingAddress = new customerAddressType
            {
                firstName = authNetSubmit.FirstNameOnCard,
                lastName = authNetSubmit.LastNameOnCard,
                address = authNetSubmit.BillingAddress,
                city = authNetSubmit.BillingCity,
                zip = authNetSubmit.BillingZipCode
            };


            //standard api call to retrieve response
            var paymentType = new paymentType { Item = opaqueData };

            // Add line Items
            ChargeDescription = NormalizeChargeDescription(ChargeDescription);
            var lineItems = new lineItemType[2];
            lineItems[0] = new lineItemType { itemId = "1", name = ChargeDescription, quantity = 1, unitPrice = amount };
            //lineItems[1] = new lineItemType { itemId = "2", name = "snowboard", quantity = 1, unitPrice = new Decimal(450.00) };

            var transactionRequest = new transactionRequestType
            {
                transactionType = transactionTypeEnum.authOnlyTransaction.ToString(),    // pre-auth the card
                amount = amount,
                payment = paymentType,
                billTo = billingAddress,
                lineItems = lineItems,
                order = new orderType { description = ChargeDescription }

            };
            
            var request = new createTransactionRequest { transactionRequest = transactionRequest };
            Logger.Info($"PreAuthCreditCardRequest request={PaymentLogHelper.SummarizeRequest(request)}");
            // instantiate the controller that will call the service
            var controller = new createTransactionController(request);
            controller.Execute();

            // get the response from the service (errors contained if any)
            var response = controller.GetApiResponse();
            Logger.Info($"PreAuthCreditCardRequest response={PaymentLogHelper.SummarizeResponse(response)}");
            // validate response
            if (response != null)
            {
                if (response.messages.resultCode == messageTypeEnum.Ok)
                {
                    Logger.Debug($"AuthNetManager PreAuthCreditCardRequest end | amount={amount} | {cardSummary}");
                    foreach (var msg in response.messages.message)
                    {
                        Logger.Debug($"AuthNetManager PreAuthCreditCardRequest message | amount={amount} | code={msg.code} | text={msg.text}");
                    }
                    if (response.transactionResponse.messages != null)
                    {
                        return response;
                    }
                    else
                    {

                        return response;
                    }
                }
                else
                {

                    Logger.Error("Registration Failed Transaction.");
                    Logger.Error($"Card: {cardSummary}");
                    Logger.Error("Amount: " + string.Format("{0:C}", amount));
                    Logger.Error($"response: {PaymentLogHelper.SummarizeResponse(response)}");
                    if (response.transactionResponse != null && response.transactionResponse.errors != null)
                    {
                        //var _failureMsg = L("VerifyPaymentInformation") + "<br>" + response.transactionResponse.errors[0].errorText + "<br>" + response.transactionResponse.errors[0].errorCode;

                        Logger.Error("Error Code: " + response.transactionResponse.errors[0].errorCode);
                        Logger.Error("Error message: " + response.transactionResponse.errors[0].errorText);
                    }
                    else
                    {
                        Logger.Error("Error Code: " + response.messages.message[0].code);
                        Logger.Error("Error message: " + response.messages.message[0].text);
                    }
                    foreach(var msg in response.messages.message)
                    {
                        Logger.Debug($"AuthNetManager PreAuthCreditCardRequest message | amount={amount} | code={msg.code} | text={msg.text}");
                    }

                    Logger.Debug($"AuthNetManager PreAuthCreditCardRequest end | amount={amount} | {cardSummary}");
                    return response;
                }
            }
            else
            {
                Logger.Debug($"AuthNetManager PreAuthCreditCardRequest end | amount={amount} | {cardSummary}");
                var transactionResponseMessages = new List<transactionResponseMessage>();//  { code = "err", description = "Something went wrong" }[];
                transactionResponseMessages.Add(new transactionResponseMessage() { code = "err", description = "Something went wrong" });
                throw new UserFriendlyException(L("PaymentFailed"), "Something went wrong!");
            }
        }
        private AuthNetTransactionResult InterpretTransactionResponse(string operationName, createTransactionResponse response, string defaultErrorMessage)
        {
            if (response == null)
            {
                Logger.Error($"{operationName}: payment gateway returned null response.");
                return AuthNetTransactionResult.Failure("null_response", defaultErrorMessage);
            }

            if (response.messages?.resultCode == messageTypeEnum.Ok && response.transactionResponse != null)
            {
                if (response.transactionResponse.messages != null || response.transactionResponse.responseCode == "1")
                {
                    return AuthNetTransactionResult.Success(response);
                }
            }

            var gatewayError = response.transactionResponse?.errors?.FirstOrDefault();
            var messageError = response.messages?.message?.FirstOrDefault();

            var errorCode = gatewayError?.errorCode ?? messageError?.code ?? "unknown_error";
            var errorMessage = gatewayError?.errorText ?? messageError?.text ?? defaultErrorMessage;

            Logger.Error($"{operationName} failed. Code={errorCode} Message={errorMessage}");
            return AuthNetTransactionResult.Failure(errorCode, errorMessage, response);
        }

        private static string NormalizeChargeDescription(string chargeDescription)
        {
            if (string.IsNullOrWhiteSpace(chargeDescription))
            {
                chargeDescription = "Surscan.com";
            }

            chargeDescription = chargeDescription.Replace("(", string.Empty).Replace(")", string.Empty);

            if (chargeDescription.Length > 31)
            {
                chargeDescription = chargeDescription.Substring(0, 31);
            }

            return chargeDescription;
        }
    }
}
