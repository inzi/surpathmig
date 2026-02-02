using inzibackend.MultiTenancy.Payments.AuthorizeNet;
using inzibackend.MultiTenancy.Payments.AuthorizeNet.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inzibackend.MultiTenancy.Payments
{
    public class AuthorizeNetPaymentAppService : inzibackendAppServiceBase, IAuthorizeNetPaymentAppService
    {
        Task IAuthorizeNetPaymentAppService.ConfirmPayment(AuthorizeNetPaymentInput input)
        {
            throw new NotImplementedException();
        }
    }
}
