using Abp.Zero.Ldap.Authentication;
using Abp.Zero.Ldap.Configuration;
using inzibackend.Authorization.Users;
using inzibackend.MultiTenancy;

namespace inzibackend.Authorization.Ldap;

public class AppLdapAuthenticationSource : LdapAuthenticationSource<Tenant, User>
{
    public AppLdapAuthenticationSource(ILdapSettings settings, IAbpZeroLdapModuleConfig ldapModuleConfig)
        : base(settings, ldapModuleConfig)
    {
    }
}

