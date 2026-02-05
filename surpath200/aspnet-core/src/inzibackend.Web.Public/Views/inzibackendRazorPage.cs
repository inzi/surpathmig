using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace inzibackend.Web.Public.Views;

public abstract class inzibackendRazorPage<TModel> : AbpRazorPage<TModel>
{
    [RazorInject]
    public IAbpSession AbpSession { get; set; }

    protected inzibackendRazorPage()
    {
        LocalizationSourceName = inzibackendConsts.LocalizationSourceName;
    }
}

