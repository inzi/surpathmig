using Abp.Domain.Services;

namespace inzibackend;

public abstract class inzibackendDomainServiceBase : DomainService
{
    /* Add your common members for all your domain services. */

    protected inzibackendDomainServiceBase()
    {
        LocalizationSourceName = inzibackendConsts.LocalizationSourceName;
    }
}

