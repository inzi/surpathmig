using Abp.Domain.Uow;
using Abp.EntityFrameworkCore;
using Abp.OpenIddict.EntityFrameworkCore.Applications;
using inzibackend.EntityFrameworkCore;

namespace inzibackend.OpenIddict.Applications;

public class OpenIddictApplicationRepository : EfCoreOpenIddictApplicationRepository<inzibackendDbContext>
{
    public OpenIddictApplicationRepository(
        IDbContextProvider<inzibackendDbContext> dbContextProvider,
        IUnitOfWorkManager unitOfWorkManager) : base(dbContextProvider, unitOfWorkManager)
    {
    }
}

