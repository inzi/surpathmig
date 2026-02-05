using Abp.Domain.Uow;
using Abp.EntityFrameworkCore;
using Abp.OpenIddict.EntityFrameworkCore.Scopes;
using inzibackend.EntityFrameworkCore;

namespace inzibackend.OpenIddict.Scopes;

public class OpenIddictScopeRepository : EfCoreOpenIddictScopeRepository<inzibackendDbContext>
{
    public OpenIddictScopeRepository(
        IDbContextProvider<inzibackendDbContext> dbContextProvider,
        IUnitOfWorkManager unitOfWorkManager) : base(dbContextProvider, unitOfWorkManager)
    {
    }
}

