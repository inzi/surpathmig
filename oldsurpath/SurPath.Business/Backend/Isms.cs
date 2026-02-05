using System.Threading.Tasks;

namespace SurpathBackend
{
    public interface Isms
    {
        Task<bool> SendSMS(string ApiKey, string Token, string FromID, string ToID, string Message);

        object ProcessInboundSMS(object incomingMessage);

        bool SetSMSAutoResponse(string ApiKey, string Message);

        bool ProcessOutboundSMS();
    }
}