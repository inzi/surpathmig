using Serilog;
using System;
using System.Net;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace SurpathBackend
{
    public class BackendSMS : Isms
    {
        private static ILogger _logger;
        public string Name { get; } = "BackendSMS";

        public BackendSMS(ILogger __logger = null)
        {
            if (__logger != null)
            {
                _logger = __logger;
                if (_logger != null) _logger.Debug($"{this.Name} logger online");
            }
            else
            {
                _logger = new LoggerConfiguration().CreateLogger();
            }

            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                            | SecurityProtocolType.Tls11
                            | SecurityProtocolType.Tls12
                            | SecurityProtocolType.Ssl3;
        }

        /// <summary>
        /// This handles inbound SMS
        /// calls backend_get_sms_autoresponse
        /// When Twilio calls back to our URL, the controller will pass to this function
        /// This function examines the sent SMS table and get the last text sent to the sending number.
        /// If the number is unknown, it ignores it (or replies with a generic message?)
        /// [IgnoreUnknownSMSInbound and UnknownSMSResponseText in appsettings, SMSEngine section]
        /// It gets the client_id from the table and then uses that to pull out the custom responses
        /// for the client.
        /// For details, see: https://www.twilio.com/docs/sms/quickstart/csharp-dotnet-framework#receive-and-reply-to-inbound-sms-messages-with-aspnet-mvc
        ///
        /// </summary>
        /// <param name="ApiKey"></param>
        /// <returns></returns>
        public object ProcessInboundSMS(object incomingMessage)
        {
            // From Webhook

            // Get top 1 result from sms outbound table

            // Process no results

            // Pull custom or generic message from SQL query (join custom message or generic)

            // Send reply

            throw new NotImplementedException();
        }

        /// <summary>
        /// This method will pull unsent messages from the backend_sms_outbound table, and update the table when sending is complate
        /// </summary>
        /// <returns></returns>
        public bool ProcessOutboundSMS()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SendSMS(string ApiKey, string Token, string FromID, string ToID, string Message)
        {
            _logger.Debug($"BackendSMS SendSMS CALLED");
            if (String.IsNullOrEmpty(FromID)) FromID = "+13252081790";
            if (String.IsNullOrEmpty(ApiKey)) ApiKey = "AC71c8ef0bcf11e3a2c65f4bab577a2e1e";
            if (String.IsNullOrEmpty(Token)) Token = "ce5bf1314d9e6d4e36a825e0efc4be19";

            try
            {
                _logger.Debug($"Attempting to send SMS: {FromID} to {ToID} -> {Message}   using {ApiKey}");
                TwilioClient.Init(ApiKey, Token);
                //Task.Run(async () => await donorBL.DonorSendSMS(donor_id, "test using donorbl")).GetAwaiter().GetResult();
                MessageResource message;
                message = Task.Run(async () => await MessageResource.CreateAsync(
                    body: Message,
                    from: new Twilio.Types.PhoneNumber(FromID),
                    to: new Twilio.Types.PhoneNumber(ToID)
                    )).GetAwaiter().GetResult();

                //TODO evaluate message as to wether the user opted out and if so log it
                _logger.Debug($"From message:");
                _logger.Debug($"Body: {message.Body}");
                _logger.Debug($"Status: {message.Status}");
                _logger.Debug($"ErrorCode: {message.ErrorCode.ToString()}");
                _logger.Debug($"ErrorMessage: {message.ErrorMessage}");
                return true;
            }
            catch (Exception ex)
            {
                var x = ex.Message;
                _logger.Error($"Problem sending SMS!!!");
                _logger.Error(ex, "SMS SEND EXCEPTION");
            }

            return false;
        }

        public bool SetSMSAutoResponse(string ApiKey, string Message)
        {
            throw new NotImplementedException();
        }
    }
}