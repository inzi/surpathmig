using Abp;
using System.Threading.Tasks;

namespace inzibackend.Authorization.Users.DataCleaners;

public interface IUserDataCleaner
{
    Task CleanUserData(UserIdentifier userIdentifier);
}

