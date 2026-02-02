using System.Threading.Tasks;
using Abp.Authorization.Users;
using inzibackend.Authorization.Users;

namespace inzibackend.Authorization;

public static class UserManagerExtensions
{
    public static async Task<User> GetAdminAsync(this UserManager userManager)
    {
        return await userManager.FindByNameAsync(AbpUserBase.AdminUserName);
    }

    public static User GetAdmin(this UserManager userManager)
    {
        return userManager.FindByNameOrEmail(AbpUserBase.AdminUserName);
    }
}

