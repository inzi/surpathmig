using Abp.Application.Services;
using inzibackend.MultiTenancy.Payments.AuthorizeNet.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace inzibackend.MultiTenancy.Payments.AuthorizeNet
{
    public interface IAuthorizeNetPaymentAppService : IApplicationService
    {
        Task ConfirmPayment(AuthorizeNetPaymentInput input);

    }
}
