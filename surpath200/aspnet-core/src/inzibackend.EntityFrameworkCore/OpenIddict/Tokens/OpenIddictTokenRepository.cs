using Abp.Domain.Uow;
using Abp.EntityFrameworkCore;
using Abp.OpenIddict.EntityFrameworkCore.Tokens;
using inzibackend.EntityFrameworkCore;

namespace inzibackend.OpenIddict.Tokens;

public class OpenIddictTokenRepository : EfCoreOpenIddictTokenRepository<inzibackendDbContext>
{
    public OpenIddictTokenRepository(
        IDbContextProvider<inzibackendDbContext> dbContextProvider,
        IUnitOfWorkManager unitOfWorkManager) : base(dbContextProvider, unitOfWorkManager)
    {
    }
}

