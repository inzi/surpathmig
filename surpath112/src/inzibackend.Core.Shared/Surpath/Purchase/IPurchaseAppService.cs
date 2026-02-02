using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using inzibackend.Surpath.Dtos;

using Abp.Application.Services.Dto;
using inzibackend.Surpath.Purchase;
using inzibackend.Surpath;
using AuthorizeNet.Api.Contracts.V1;
using System.Collections.Generic;

namespace inzibackend.Surpath.Dtos
{
    public interface IPurchaseAppService
    {
        Task<bool> DonorCurrent(long id);
        Task<Guid?> GetChortUserId(long id);
        Task<bool> SandboxPayment();
        Task<bool> DoPurchaseFromHelper(AuthNetSubmit authNetSubmit);
        Task<AuthorizeNetSettings> AuthorizeNetSettings();
        Task<createTransactionResponse> PreAuth(PreAuthDto preAuth);
        Task<List<string>> GetUserRolesAsStringList(long userId);
    }
}
