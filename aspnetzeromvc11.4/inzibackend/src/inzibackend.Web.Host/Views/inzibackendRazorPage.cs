using Abp.AspNetCore.Mvc.Views;

namespace inzibackend.Web.Views
{
    public abstract class inzibackendRazorPage<TModel> : AbpRazorPage<TModel>
    {
        protected inzibackendRazorPage()
        {
            LocalizationSourceName = inzibackendConsts.LocalizationSourceName;
        }
    }
}
