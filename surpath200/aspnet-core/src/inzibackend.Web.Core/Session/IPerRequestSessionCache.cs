using System.Threading.Tasks;
using inzibackend.Sessions.Dto;

namespace inzibackend.Web.Session;

public interface IPerRequestSessionCache
{
    Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformationsAsync();
}

