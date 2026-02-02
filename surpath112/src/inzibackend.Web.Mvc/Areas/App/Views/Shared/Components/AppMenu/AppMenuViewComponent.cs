using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Navigation;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc;
using inzibackend.MultiTenancy;
using inzibackend.Web.Areas.App.Models.Layout;
using inzibackend.Web.Areas.App.Startup;
using inzibackend.Web.Views;
using inzibackend.Configuration;
using Abp.Configuration;

namespace inzibackend.Web.Areas.App.Views.Shared.Components.AppMenu
{
    public class AppMenuViewComponent : inzibackendViewComponent
    {
        private readonly IUserNavigationManager _userNavigationManager;
        private readonly IAbpSession _abpSession;
        private readonly TenantManager _tenantManager;
        private readonly ISettingManager SettingManager;

        public AppMenuViewComponent(
            IUserNavigationManager userNavigationManager,
            IAbpSession abpSession,
            TenantManager tenantManager,
            ISettingManager settingManager)
        {
            _userNavigationManager = userNavigationManager;
            _abpSession = abpSession;
            _tenantManager = tenantManager;
            SettingManager = settingManager;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="isLeftMenuUsed">set to true for rendering left aside menu</param>
        /// <param name="iconMenu">set to render main menu items as icons. Only valid for left menu</param>
        /// <param name="currentPageName">Name of the current pagae</param>
        /// <param name="height">height of the menu</param>
        /// <returns></returns>
        public async Task<IViewComponentResult> InvokeAsync(bool isLeftMenuUsed, bool iconMenu = false, string currentPageName = null, string height = "auto")
        {

            var _theme = SettingManager.GetSettingValue(AppSettings.UiManagement.Theme);
            var model = new MenuViewModel
            {
                Menu = await _userNavigationManager.GetMenuAsync(AppModNavigationProvider.MenuName, _abpSession.ToUserIdentifier()),
                Height = height,
                CurrentPageName = currentPageName,
                IconMenu = iconMenu
            };

            if (_theme.Equals("theme0", System.StringComparison.InvariantCultureIgnoreCase))
            {
                model = new MenuViewModel
                {
                    Menu = await _userNavigationManager.GetMenuAsync(AppSPNavigationProvider.MenuName, _abpSession.ToUserIdentifier()),
                    Height = height,
                    CurrentPageName = currentPageName,
                    IconMenu = iconMenu
                };
            }

            if (AbpSession.TenantId == null)
            {
                return GetView(model, isLeftMenuUsed);
            }

            var tenant = await _tenantManager.GetByIdAsync(AbpSession.TenantId.Value);
            if (tenant.EditionId.HasValue)
            {
                return GetView(model, isLeftMenuUsed);
            }

            var subscriptionManagement = FindMenuItemOrNull(model.Menu.Items, AppPageNames.Tenant.SubscriptionManagement);
            if (subscriptionManagement != null)
            {
                subscriptionManagement.IsVisible = false;
            }

            return GetView(model, isLeftMenuUsed);
        }

        public UserMenuItem FindMenuItemOrNull(IList<UserMenuItem> userMenuItems, string name)
        {
            if (userMenuItems == null)
            {
                return null;
            }

            foreach (var menuItem in userMenuItems)
            {
                if (menuItem.Name == name)
                {
                    return menuItem;
                }

                var found = FindMenuItemOrNull(menuItem.Items, name);
                if (found != null)
                {
                    return found;
                }
            }

            return null;
        }

        private IViewComponentResult GetView(MenuViewModel model, bool isLeftMenuUsed)
        {
            return View(isLeftMenuUsed ? "Default" : "Top", model);
        }
    }
}