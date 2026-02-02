using Abp.Domain.Uow;
using Abp.EntityFrameworkCore;
using Abp.OpenIddict.EntityFrameworkCore.Authorizations;
using inzibackend.EntityFrameworkCore;

namespace inzibackend.OpenIddict.Authorizations;

public class OpenIddictAuthorizationRepository : EfCoreOpenIddictAuthorizationRepository<inzibackendDbContext>
{
    public OpenIddictAuthorizationRepository(
        IDbContextProvider<inzibackendDbContext> dbContextProvider,
        IUnitOfWorkManager unitOfWorkManager) : base(dbContextProvider, unitOfWorkManager)
    {
    }
}

