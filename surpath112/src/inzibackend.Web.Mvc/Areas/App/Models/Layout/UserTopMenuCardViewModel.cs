using inzibackend.Sessions.Dto;
using Abp.Extensions;

namespace inzibackend.Web.Areas.App.Models.Layout
{
    public class UserTopMenuCardViewModel
    {
        public bool IsMultiTenancyEnabled { get; set; }

        public bool IsImpersonatedLogin { get; set; }

        public bool HasUiCustomizationPagePermission { get; set; }

        public GetCurrentLoginInformationsOutput LoginInformations { get; set; }

        public string TogglerCssClass { get; set; }

        public string TextCssClass { get; set; }

        public string SymbolCssClass { get; set; }
        
        public string SymbolTextCssClass { get; set; }

        public string AnchorCssClass { get; set; }
        
        public bool RenderOnlyIcon { get; set; }

        public UserTopMenuCardViewModel()
        {
            AnchorCssClass = "btn btn-icon btn-active-light-primary position-relative w-30px h-30px w-md-40px h-md-40px";
        }
        
        public string GetShownLoginName()
        {
            var userName = "<span id=\"HeaderCurrentUserName\">" + LoginInformations.User.UserName + "</span>";

            if (!IsMultiTenancyEnabled)
            {
                return userName;
            }

            return LoginInformations.Tenant == null
                ? "<span class='tenancy-name'>.\\</span>" + userName
                : "<span class='tenancy-name'>" + LoginInformations.Tenant.TenancyName + "\\" + "</span>" + userName;
        }

        ////////
        public bool ProfilePickInsteadOfInitial { get; set; } = false;
        public string ProfilePickInsteadOfInitialClass { get; set; }


        public string LogoSkinOverride { get; set; }

        public string LogoClassOverride { get; set; }

        public string GetLogoUrl(string appPath, string logoSkin)
        {
            if (!LogoSkinOverride.IsNullOrEmpty())
            {
                logoSkin = LogoSkinOverride;
            }

            if (LoginInformations?.Tenant?.LogoId == null)
            {
                return appPath + $"Common/Images/app-logo-on-{logoSkin}.svg";
            }

            //id parameter is used to prevent caching only.
            return appPath + "TenantCustomization/GetLogo?tenantId=" + LoginInformations?.Tenant?.Id;
        }
    }
}
