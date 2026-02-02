using System.Threading.Tasks;
using Abp.Application.Services;
using inzibackend.Sessions.Dto;

namespace inzibackend.Sessions;

public interface ISessionAppService : IApplicationService
{
    Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();

    Task<UpdateUserSignInTokenOutput> UpdateUserSignInToken();
}

