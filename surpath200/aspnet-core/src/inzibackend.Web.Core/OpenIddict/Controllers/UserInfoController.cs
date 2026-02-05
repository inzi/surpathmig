using Abp.AspNetCore.OpenIddict.Claims;
using Abp.AspNetCore.OpenIddict.Controllers;
using Abp.Authorization;
using Abp.Authorization.Users;
using inzibackend.Authorization.Roles;
using inzibackend.Authorization.Users;
using inzibackend.MultiTenancy;
using OpenIddict.Abstractions;

namespace inzibackend.Web.OpenIddict.Controllers;

public class UserInfoController : UserInfoController<Tenant, Role, User>
{
    public UserInfoController(AbpSignInManager<Tenant, Role, User> signInManager,
        AbpUserManager<Role, User> userManager, IOpenIddictApplicationManager applicationManager,
        IOpenIddictAuthorizationManager authorizationManager, IOpenIddictScopeManager scopeManager,
        IOpenIddictTokenManager tokenManager,
        AbpOpenIddictClaimsPrincipalManager openIddictClaimsPrincipalManager) : base(signInManager, userManager,
        applicationManager, authorizationManager, scopeManager, tokenManager, openIddictClaimsPrincipalManager)
    {
    }
}

