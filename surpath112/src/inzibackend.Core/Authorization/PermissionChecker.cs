using Abp.Authorization;
using inzibackend.Authorization.Roles;
using inzibackend.Authorization.Users;

namespace inzibackend.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
