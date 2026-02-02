using System.Threading.Tasks;
using Abp.Dependency;

namespace inzibackend.Surpath.Registration
{
    public interface IAuthNetGateway
    {
        Task<AuthNetTransactionResult> CapturePreAuthAsync(
            AuthNetSubmit authNetSubmit,
            AuthNetCaptureResultDto captureReceipt,
            decimal amount,
            string tenantIdentifier);

        Task<AuthNetTransactionResult> VoidPreAuthAsync(string transactionId, string tenantIdentifier);
    }

    public class AuthNetGateway : IAuthNetGateway, ITransientDependency
    {
        private readonly AuthNetManager _authNetManager;

        public AuthNetGateway(AuthNetManager authNetManager)
        {
            _authNetManager = authNetManager;
        }

        public Task<AuthNetTransactionResult> CapturePreAuthAsync(
            AuthNetSubmit authNetSubmit,
            AuthNetCaptureResultDto captureReceipt,
            decimal amount,
            string tenantIdentifier)
        {
            return _authNetManager.CapturePreAuthCreditCardRequest(authNetSubmit, captureReceipt, amount, tenantIdentifier);
        }

        public Task<AuthNetTransactionResult> VoidPreAuthAsync(string transactionId, string tenantIdentifier)
        {
            return _authNetManager.VoidPreAuthAsync(transactionId, tenantIdentifier);
        }
    }
}
