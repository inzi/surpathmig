using Abp.AspNetCore.Mvc.ViewComponents;

namespace inzibackend.Web.Public.Views;

public abstract class inzibackendViewComponent : AbpViewComponent
{
    protected inzibackendViewComponent()
    {
        LocalizationSourceName = inzibackendConsts.LocalizationSourceName;
    }
}

