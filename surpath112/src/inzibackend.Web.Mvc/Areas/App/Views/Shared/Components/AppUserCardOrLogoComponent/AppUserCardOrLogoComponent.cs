using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using inzibackend.Web.Areas.App.Models.Layout;
using inzibackend.Web.Session;
using inzibackend.Web.Views;
using Abp.Configuration.Startup;
using Abp.Runtime.Session;
using inzibackend.Authorization;

namespace inzibackend.Web.Areas.App.Views.Shared.Components.AppUserCardOrLogo
{
    public class AppUserCardOrLogoComponent : inzibackendViewComponent
    {
        private readonly IMultiTenancyConfig _multiTenancyConfig;
        private readonly IAbpSession _abpSession;
        private readonly IPerRequestSessionCache _sessionCache;

        public AppUserCardOrLogoComponent(
            IPerRequestSessionCache sessionCache,
            IMultiTenancyConfig multiTenancyConfig,
            IAbpSession abpSession)
        {
            _sessionCache = sessionCache;
            _multiTenancyConfig = multiTenancyConfig;
            _abpSession = abpSession;
        }

        public async Task<IViewComponentResult> InvokeAsync(
            string togglerCssClass,
            string textCssClass,
            string symbolCssClass,
            string symbolTextCssClas,
            string anchorCssClass,
            string logoSkin = null,
            string logoClass = "",
            bool renderOnlyIcon = false
          )
        {

                var headerModel = new LogoViewModel
                {
                    LoginInformations = await _sessionCache.GetCurrentLoginInformationsAsync(),
                    LogoSkinOverride = logoSkin,
                    LogoClassOverride = logoClass
                };

                //return View(headerModel);

            return View(new AppUserCardOrLogoViewModel
            {
                LoginInformations = await _sessionCache.GetCurrentLoginInformationsAsync(),
                IsMultiTenancyEnabled = _multiTenancyConfig.IsEnabled,
                IsImpersonatedLogin = _abpSession.ImpersonatorUserId.HasValue,
                HasUiCustomizationPagePermission = await PermissionChecker.IsGrantedAsync(AppPermissions.Pages_Administration_UiCustomization),
                TogglerCssClass = togglerCssClass,
                TextCssClass = textCssClass,
                SymbolCssClass = symbolCssClass,
                SymbolTextCssClass = symbolTextCssClas,
                AnchorCssClass = anchorCssClass,
                RenderOnlyIcon = renderOnlyIcon,
                LogoSkinOverride = logoSkin,
                LogoClassOverride = logoClass
            });
        }
    }
}