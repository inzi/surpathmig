using inzibackend.MultiTenancy.Payments.AuthorizeNet;

namespace inzibackend.Web.Controllers
{
    public class AuthorizeNetController : AuthorizeNetControllerBase
    {
        public AuthorizeNetController(AuthorizeNetManager _Manager, AuthorizeNetConfiguration _Configuration, IAuthorizeNetPaymentAppService _PaymentAppService) : base(_Manager, _Configuration, _PaymentAppService)
        {
        }
    }
}
