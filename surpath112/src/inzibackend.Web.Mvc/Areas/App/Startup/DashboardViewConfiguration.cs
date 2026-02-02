using System.Collections.Generic;
using inzibackend.Web.DashboardCustomization;


namespace inzibackend.Web.Areas.App.Startup
{
    public class DashboardViewConfiguration
    {
        public Dictionary<string, WidgetViewDefinition> WidgetViewDefinitions { get; } = new Dictionary<string, WidgetViewDefinition>();

        public Dictionary<string, WidgetFilterViewDefinition> WidgetFilterViewDefinitions { get; } = new Dictionary<string, WidgetFilterViewDefinition>();

        public DashboardViewConfiguration()
        {
            var jsAndCssFileRoot = "/Areas/App/Views/CustomizableDashboard/Widgets/";
            var viewFileRoot = "App/Widgets/";

            #region FilterViewDefinitions

            WidgetFilterViewDefinitions.Add(inzibackendDashboardCustomizationConsts.Filters.FilterDateRangePicker,
                new WidgetFilterViewDefinition(
                    inzibackendDashboardCustomizationConsts.Filters.FilterDateRangePicker,
                    "~/Areas/App/Views/Shared/Components/CustomizableDashboard/Widgets/DateRangeFilter.cshtml",
                    jsAndCssFileRoot + "DateRangeFilter/DateRangeFilter.min.js",
                    jsAndCssFileRoot + "DateRangeFilter/DateRangeFilter.min.css")
            );

            // Surscan Working Filter
            WidgetFilterViewDefinitions.Add(inzibackendDashboardCustomizationConsts.Filters.SurpathDeptFilter,
                new WidgetFilterViewDefinition(
                    inzibackendDashboardCustomizationConsts.Filters.SurpathDeptFilter,
                    //viewFileRoot + "SurpathDeptFilter.cshtml",
                    "~/Areas/App/Views/Shared/Components/CustomizableDashboard/Widgets/SurpathDeptFilter.cshtml",
                    jsAndCssFileRoot + "SurpathDeptFilter/SurpathDeptFilter.min.js",
                    jsAndCssFileRoot + "SurpathDeptFilter/SurpathDeptFilter.min.css")
            );

            //add your filters iew definitions here
            #endregion

            #region WidgetViewDefinitions

            #region TenantWidgets

            WidgetViewDefinitions.Add(inzibackendDashboardCustomizationConsts.Widgets.Tenant.DailySales,
                new WidgetViewDefinition(
                    inzibackendDashboardCustomizationConsts.Widgets.Tenant.DailySales,
                    viewFileRoot + "DailySales",
                    jsAndCssFileRoot + "DailySales/DailySales.min.js",
                    jsAndCssFileRoot + "DailySales/DailySales.min.css"));

            WidgetViewDefinitions.Add(inzibackendDashboardCustomizationConsts.Widgets.Tenant.GeneralStats,
                new WidgetViewDefinition(
                    inzibackendDashboardCustomizationConsts.Widgets.Tenant.GeneralStats,
                    viewFileRoot + "GeneralStats",
                    jsAndCssFileRoot + "GeneralStats/GeneralStats.min.js",
                    jsAndCssFileRoot + "GeneralStats/GeneralStats.min.css"));

            WidgetViewDefinitions.Add(inzibackendDashboardCustomizationConsts.Widgets.Tenant.ProfitShare,
                new WidgetViewDefinition(
                    inzibackendDashboardCustomizationConsts.Widgets.Tenant.ProfitShare,
                    viewFileRoot + "ProfitShare",
                    jsAndCssFileRoot + "ProfitShare/ProfitShare.min.js",
                    jsAndCssFileRoot + "ProfitShare/ProfitShare.min.css"));

            WidgetViewDefinitions.Add(inzibackendDashboardCustomizationConsts.Widgets.Tenant.MemberActivity,
                new WidgetViewDefinition(
                    inzibackendDashboardCustomizationConsts.Widgets.Tenant.MemberActivity,
                    viewFileRoot + "MemberActivity",
                    jsAndCssFileRoot + "MemberActivity/MemberActivity.min.js",
                    jsAndCssFileRoot + "MemberActivity/MemberActivity.min.css"));

            WidgetViewDefinitions.Add(inzibackendDashboardCustomizationConsts.Widgets.Tenant.RegionalStats,
                new WidgetViewDefinition(
                    inzibackendDashboardCustomizationConsts.Widgets.Tenant.RegionalStats,
                    viewFileRoot + "RegionalStats",
                    jsAndCssFileRoot + "RegionalStats/RegionalStats.min.js",
                    jsAndCssFileRoot + "RegionalStats/RegionalStats.min.css",
                    12,
                    10));

            WidgetViewDefinitions.Add(inzibackendDashboardCustomizationConsts.Widgets.Tenant.SalesSummary,
                new WidgetViewDefinition(
                    inzibackendDashboardCustomizationConsts.Widgets.Tenant.SalesSummary,
                    viewFileRoot + "SalesSummary",
                    jsAndCssFileRoot + "SalesSummary/SalesSummary.min.js",
                    jsAndCssFileRoot + "SalesSummary/SalesSummary.min.css",
                    6,
                    10));

            WidgetViewDefinitions.Add(inzibackendDashboardCustomizationConsts.Widgets.Tenant.TopStats,
                new WidgetViewDefinition(
                    inzibackendDashboardCustomizationConsts.Widgets.Tenant.TopStats,
                    viewFileRoot + "TopStats",
                    jsAndCssFileRoot + "TopStats/TopStats.min.js",
                    jsAndCssFileRoot + "TopStats/TopStats.min.css",
                    12,
                    10));

            //add your tenant side widget definitions here
            // Surscan Working Filter
            WidgetViewDefinitions.Add(inzibackendDashboardCustomizationConsts.Widgets.Tenant.SurpathDept,
                new WidgetViewDefinition(
                    inzibackendDashboardCustomizationConsts.Widgets.Tenant.SurpathDept,
                    viewFileRoot + "SurpathDeptWidget",
                    jsAndCssFileRoot + "SurpathDept/SurpathDept.min.js",
                    jsAndCssFileRoot + "SurpathDept/SurpathDept.min.css",
                    12,
                    10));

            WidgetViewDefinitions.Add(inzibackendDashboardCustomizationConsts.Widgets.Tenant.SurpathCohortCompliance,
                new WidgetViewDefinition(
                    inzibackendDashboardCustomizationConsts.Widgets.Tenant.SurpathCohortCompliance,
                    viewFileRoot + "SurpathCohortComplianceWidget",
                    jsAndCssFileRoot + "SurpathCohortCompliance/SurpathCohortCompliance.min.js",
                    jsAndCssFileRoot + "SurpathCohortCompliance/SurpathCohortCompliance.min.css",
                    12,
                    10));

            #endregion

            #region HostWidgets

            WidgetViewDefinitions.Add(inzibackendDashboardCustomizationConsts.Widgets.Host.IncomeStatistics,
                new WidgetViewDefinition(
                    inzibackendDashboardCustomizationConsts.Widgets.Host.IncomeStatistics,
                    viewFileRoot + "IncomeStatistics",
                    jsAndCssFileRoot + "IncomeStatistics/IncomeStatistics.min.js",
                    jsAndCssFileRoot + "IncomeStatistics/IncomeStatistics.min.css"));

            WidgetViewDefinitions.Add(inzibackendDashboardCustomizationConsts.Widgets.Host.TopStats,
                new WidgetViewDefinition(
                    inzibackendDashboardCustomizationConsts.Widgets.Host.TopStats,
                    viewFileRoot + "HostTopStats",
                    jsAndCssFileRoot + "HostTopStats/HostTopStats.min.js",
                    jsAndCssFileRoot + "HostTopStats/HostTopStats.min.css"));

            WidgetViewDefinitions.Add(inzibackendDashboardCustomizationConsts.Widgets.Host.EditionStatistics,
                new WidgetViewDefinition(
                    inzibackendDashboardCustomizationConsts.Widgets.Host.EditionStatistics,
                    viewFileRoot + "EditionStatistics",
                    jsAndCssFileRoot + "EditionStatistics/EditionStatistics.min.js",
                    jsAndCssFileRoot + "EditionStatistics/EditionStatistics.min.css"));

            WidgetViewDefinitions.Add(inzibackendDashboardCustomizationConsts.Widgets.Host.SubscriptionExpiringTenants,
                new WidgetViewDefinition(
                    inzibackendDashboardCustomizationConsts.Widgets.Host.SubscriptionExpiringTenants,
                    viewFileRoot + "SubscriptionExpiringTenants",
                    jsAndCssFileRoot + "SubscriptionExpiringTenants/SubscriptionExpiringTenants.min.js",
                    jsAndCssFileRoot + "SubscriptionExpiringTenants/SubscriptionExpiringTenants.min.css",
                    6,
                    10));

            WidgetViewDefinitions.Add(inzibackendDashboardCustomizationConsts.Widgets.Host.RecentTenants,
                new WidgetViewDefinition(
                    inzibackendDashboardCustomizationConsts.Widgets.Host.RecentTenants,
                    viewFileRoot + "RecentTenants",
                    jsAndCssFileRoot + "RecentTenants/RecentTenants.min.js",
                    jsAndCssFileRoot + "RecentTenants/RecentTenants.min.css"));

            //add your host side widgets definitions here
            #endregion

            #endregion
        }
    }
}
