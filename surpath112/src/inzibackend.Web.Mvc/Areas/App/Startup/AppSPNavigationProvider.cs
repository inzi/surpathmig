using Abp.Application.Navigation;
using Abp.Authorization;
using Abp.Localization;
using inzibackend.Authorization;

namespace inzibackend.Web.Areas.App.Startup
{
    public class AppSPNavigationProvider : NavigationProvider
    {
        public const string MenuName = "AppSp";

        public override void SetNavigation(INavigationProviderContext context)
        {
            var menu = context.Manager.Menus[MenuName] = new MenuDefinition(MenuName, new FixedLocalizableString("Main Menu"));

            //var menuItemsAll = new MenuItemDefinition(
            //    AppPageNames.Common.CohortUsers,
            //    L("CohortUserStatusPage"),
            //    url: "/App/CohortUsers/ViewCohortUser/",
            //    icon: "flaticon-home",
            //    permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_CohortUser)
            //    );
            var menuItemsAll = new MenuItemDefinition(
                          AppPageNames.Common.CohortUsers,
                          L("CohortUserStatusPage"),
                          url: "/App/CohortUsers/ViewCohortUser/",
                          icon: "flaticon-home",
                          permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_CohortUser)
                          );

            var mrHolders = new MenuItemDefinition(AppPageNames.Common.Cohorts, L("HolderLinks"),
                  icon: "flaticon-interface-8",
                  permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration));

            var mrLib = new MenuItemDefinition(AppPageNames.Common.Cohorts, L("Library"), icon: "flaticon-interface-8");
            var miLib = new MenuItemDefinition(
                            AppPageNames.Common.TenantDocumentCategories,
                            L("LibraryCategories"),
                            url: "App/TenantDocumentCategories",
                            icon: "flaticon-more",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_TenantDocumentCategories)
                        );
            mrLib.AddItem(miLib);

            var mrDepts = new MenuItemDefinition(AppPageNames.Common.TenantDepartments, L("DepartmentsMenu"), icon: "flaticon-interface-8");

            var submi = new MenuItemDefinition(AppPageNames.Common.TenantDepartments,
                L("TenantDepartments"),
                url: "App/TenantDepartments",
                icon: "flaticon-more",
                permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_TenantDepartments)
            );

            mrDepts.AddItem(submi);

            submi = new MenuItemDefinition(
                AppPageNames.Common.TenantDepartmentUsers,
                L("DepartmentUsers"),
                url: "App/TenantDepartmentUsers",
                icon: "flaticon-more",
                permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_TenantDepartmentUsers)
                );
            mrDepts.AddItem(submi);

            submi = new MenuItemDefinition(
                AppPageNames.Common.DeptCodes,
                L("DeptCodes"),
                url: "App/DeptCodes",
                icon: "flaticon-more",
                permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_DeptCodes)
                );

            mrDepts.AddItem(submi);

            //var menuRootRotationSlots = new MenuItemDefinition(
            //    AppPageNames.Common.RotationSlots,
            //    L("SlotRotations"),
            //    url: "App/SlotRotations",
            //    icon: "flaticon-more",
            //    permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_RotationSlots)
            //);

            var menuRootRotationSlots = new MenuItemDefinition(
                    AppPageNames.Common.RotationSlots,
                    L("SlotRotations"),
                    url: "App/RotationSlots",
                    icon: "flaticon-calendar",
                    permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_RotationSlots)
            );
            ////
            //// Cohorts
            ////// Start cohors

            var mrChorts = new MenuItemDefinition(AppPageNames.Common.Cohorts, L("CohortsMenu"), icon: "flaticon-map");

            submi = new MenuItemDefinition(
            AppPageNames.Common.CohortUsers,
            L("Cohorts"),
            url: "App/Cohorts",
            icon: "flaticon-more",
            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Cohorts)
            );
            mrChorts.AddItem(submi);

            submi = new MenuItemDefinition(
            AppPageNames.Common.CohortUsers,
            L("CohortUsers"),
            url: "App/CohortUsers",
            icon: "flaticon-more",
            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_CohortUsers)
            );
            mrChorts.AddItem(submi);

            //)

            /// end cohorts
            ////

            ////
            //// Records
            ///

            var mrRecords = new MenuItemDefinition(AppPageNames.Common.Records, L("RecordsMenu"), icon: "flaticon-interface-8");

            mrRecords.AddItem(new MenuItemDefinition(
            AppPageNames.Common.RecordStates,
            L("RecordStates"),
            url: "App/RecordStates",
            icon: "flaticon-more",
            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_RecordStates)
            )
            );

            mrRecords.AddItem(new MenuItemDefinition(
            AppPageNames.Common.RecordRequirements,
            L("RecordRequirements"),
            url: "App/RecordRequirements",
            icon: "flaticon-more",
            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_RecordRequirements)
            ));

            mrRecords
            .AddItem(new MenuItemDefinition(
            AppPageNames.Common.Records,
            L("Records"),
            url: "App/Records",
            icon: "flaticon-more",
            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Records)
            ));

            mrRecords
            .AddItem(new MenuItemDefinition(
            AppPageNames.Common.RecordNotes,
            L("RecordNotes"),
            url: "App/RecordNotes",
            icon: "flaticon-more",
            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_RecordNotes)
            ));

            mrRecords
            .AddItem(new MenuItemDefinition(
            AppPageNames.Common.RecordCategories,
            L("RecordCategories"),
            url: "App/RecordCategories",
            icon: "flaticon-more",
            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_RecordCategories)
            ));

            mrRecords
            .AddItem(new MenuItemDefinition(
            AppPageNames.Common.RecordCategoryRules,
            L("RecordCategoryRules"),
            url: "App/RecordCategoryRules",
            icon: "flaticon-more",
            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_RecordCategoryRules)
            ));

            mrRecords
            .AddItem(new MenuItemDefinition(
            AppPageNames.Common.RecordStatuses,
            L("RecordStatuses"),
            url: "App/RecordStatuses",
            icon: "flaticon-more",
            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_RecordStatuses)
            ));

            mrHolders.AddItem(mrRecords);

            mrHolders.AddItem(new MenuItemDefinition(
                        AppPageNames.Host.UserPids,
                        L("UserPids"),
                        url: "App/UserPids",
                        icon: "flaticon-more",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_UserPids)
                    )
                );

            mrHolders.AddItem(new MenuItemDefinition(
                                 AppPageNames.Host.PidTypes,
                                 L("PidTypes"),
                                 url: "App/PidTypes",
                                 icon: "flaticon-more",
                                 permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_PidTypes)
                             )
                         );

            //)/// End records

            //// Start Surpath Values

            var mrSurpathVals = new MenuItemDefinition(AppPageNames.Common.Records, L("TestingMenu"), icon: "flaticon-interface-8",
                permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_Host_Settings));

            mrSurpathVals.AddItem(new MenuItemDefinition(
            AppPageNames.Host.ConfirmationValues,
            L("ConfirmationValues"),
            url: "App/ConfirmationValues",
            icon: "flaticon-more",
            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_ConfirmationValues)
            ));

            mrSurpathVals.AddItem(new MenuItemDefinition(
            AppPageNames.Host.DrugPanels,
            L("DrugPanels"),
            url: "App/DrugPanels",
            icon: "flaticon-more",
            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_DrugPanels)
            ));

            mrSurpathVals.AddItem(new MenuItemDefinition(
            AppPageNames.Host.Panels,
            L("Panels"),
            url: "App/Panels",
            icon: "flaticon-more",
            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_Panels)
            ));

            mrSurpathVals.AddItem(new MenuItemDefinition(
            AppPageNames.Host.Drugs,
            L("Drugs"),
            url: "App/Drugs",
            icon: "flaticon-more",
            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_Drugs)
            ));

            mrSurpathVals.AddItem(new MenuItemDefinition(
            AppPageNames.Host.DrugTestCategories,
            L("DrugTestCategories"),
            url: "App/DrugTestCategories",
            icon: "flaticon-more",
            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_DrugTestCategories)
            ));

            mrSurpathVals.AddItem(new MenuItemDefinition(
            AppPageNames.Common.CodeTypes,
            L("CodeTypes"),
            url: "App/CodeTypes",
            icon: "flaticon-more",
            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_CodeTypes)
            ));

            ///// Administration
            var mrAdministration = new MenuItemDefinition(AppPageNames.Common.Administration, L("Administration"), icon: "flaticon-interface-8"); ////// Administration

            mrAdministration.AddItem(new MenuItemDefinition(
            AppPageNames.Host.Tenants,
            L("Tenants"),
            url: "App/Tenants",
            icon: "flaticon-list-3",
            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Tenants)
            ));

            var mrAdminClinicRotation = new MenuItemDefinition(AppPageNames.Common.Administration, L("ClinicRotation"), icon: "flaticon-interface-8"); ////// Administration
            mrAdminClinicRotation.AddItem(new MenuItemDefinition(
                        AppPageNames.Common.Hospitals,
                        L("Hospitals"),
                        url: "App/Hospitals",
                        icon: "flaticon-more",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_Hospitals)
                    )
                );
            mrAdminClinicRotation.AddItem(new MenuItemDefinition(
                        AppPageNames.Common.MedicalUnits,
                        L("MedicalUnits"),
                        url: "App/MedicalUnits",
                        icon: "flaticon-more",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_MedicalUnits)
                    )
                );
            mrAdministration.AddItem(mrAdminClinicRotation);

            mrAdministration.AddItem(new MenuItemDefinition(
            AppPageNames.Host.Editions,
            L("Editions"),
            url: "App/Editions",
            icon: "flaticon-app",
            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Editions)
            ));

            //var mrSurpathPricing = new MenuItemDefinition(AppPageNames.Common.Administration, L("SurpathServices"), icon: "flaticon-interface-8"); ////// Administration
            //mrSurpathPricing.AddItem(new MenuItemDefinition(
            //AppPageNames.Host.SurpathServices,
            //L("SurpathServices"),
            //url: "App/SurpathServices",
            //icon: "flaticon-more",
            //permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_SurpathServices)
            //));

            //mrSurpathPricing.AddItem(new MenuItemDefinition(
            //AppPageNames.Host.SurpathServicesManage,
            //L("ManageSurpathServices"),
            //url: "App/SurpathServices/Manage",
            //icon: "flaticon-more",
            //permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_SurpathServices)
            //));

            var tenantSurpathServicesMenu = new MenuItemDefinition(
                AppPageNames.Host.TenantSurpathServices,
                L("SurpathServices"),
                icon: "flaticon-more",
                permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_TenantSurpathServices)
            );

            tenantSurpathServicesMenu
                .AddItem(new MenuItemDefinition(
                    AppPageNames.Host.SurpathServices,
                    L("SurpathServices"),
                    url: "App/SurpathServices",
                    icon: "flaticon-more",
                    permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_SurpathServices)
                ))
                //.AddItem(new MenuItemDefinition(
                //    AppPageNames.Host.TenantSurpathServices + ".Index",
                //    L("ServicesList"),
                //    url: "App/TenantSurpathServices",
                //    icon: "flaticon-list",
                //    permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_TenantSurpathServices)
                //))
                .AddItem(new MenuItemDefinition(
                    AppPageNames.Host.TenantSurpathServices + ".PricingManagement",
                    L("PricingManagement"),
                    url: "App/TenantSurpathServices/PricingManagement",
                    icon: "flaticon-coins",
                    permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_TenantSurpathServices)
                ));

            //mrAdministration.AddItem(mrSurpathPricing);

            mrAdministration.AddItem(new MenuItemDefinition(
            AppPageNames.Common.OrganizationUnits,
            L("OrganizationUnits"),
            url: "App/OrganizationUnits",
            icon: "flaticon-map",
            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_OrganizationUnits)
            ));

            mrAdministration.AddItem(new MenuItemDefinition(
            AppPageNames.Common.Roles,
            L("Roles"),
            url: "App/Roles",
            icon: "flaticon-suitcase",
            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_Roles)
            ));

            mrAdministration.AddItem(new MenuItemDefinition(
            AppPageNames.Common.Users,
            L("Users"),
            url: "App/Users",
            icon: "flaticon-users",
            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_Users)
            ));

            // Add User Purchases
            mrAdministration.AddItem(new MenuItemDefinition(
                        AppPageNames.Host.UserPurchases,
                        L("UserPurchases"),
                        url: "App/UserPurchases",
                        icon: "flaticon-more",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_UserPurchases)
                    )
                );

            mrAdministration.AddItem(new MenuItemDefinition(
                        AppPageNames.Tenant.Welcomemessages,
                        L("Welcomemessages"),
                        url: "App/Welcomemessages",
                        icon: "flaticon-browser",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_Welcomemessages)
                    )
                );

            //mrAdministration.AddItem(new MenuItemDefinition(
            //            AppPageNames.Tenant.LegalDocuments,
            //             L("LegalDocuments"),
            //            url: "App/LegalDocuments",
            //            icon: "flaticon-file-1",
            //            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_LegalDocuments)
            //        )
            //    );

            mrAdministration.AddItem(new MenuItemDefinition(
            AppPageNames.Common.Languages,
            L("Languages"),
            url: "App/Languages",
            icon: "flaticon-tabs",
            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_Languages)
            ));

            mrAdministration.AddItem(new MenuItemDefinition(
            AppPageNames.Common.AuditLogs,
            L("AuditLogs"),
            url: "App/AuditLogs",
            icon: "flaticon-folder-1",
            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_AuditLogs)
            ));

            mrAdministration.AddItem(new MenuItemDefinition(
            AppPageNames.Host.Maintenance,
            L("Maintenance"),
            url: "App/Maintenance",
            icon: "flaticon-lock",
            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_Host_Maintenance)
            ));

            mrAdministration.AddItem(new MenuItemDefinition(
            AppPageNames.Tenant.SubscriptionManagement,
            L("Subscription"),
            url: "App/SubscriptionManagement",
            icon: "flaticon-refresh",
            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_Tenant_SubscriptionManagement)
            ));

            mrAdministration.AddItem(new MenuItemDefinition(
            AppPageNames.Common.UiCustomization,
            L("VisualSettings"),
            url: "App/UiCustomization",
            icon: "flaticon-medical",
            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_UiCustomization)
            ));

            mrAdministration.AddItem(new MenuItemDefinition(
            AppPageNames.Common.WebhookSubscriptions,
            L("WebhookSubscriptions"),
            url: "App/WebhookSubscription",
            icon: "flaticon2-world",
            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_WebhookSubscription)
            ));

            mrAdministration.AddItem(new MenuItemDefinition(
            AppPageNames.Common.DynamicProperties,
            L("DynamicProperties"),
            url: "App/DynamicProperty",
            icon: "flaticon-interface-8",
            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_DynamicProperties)
            ));

            mrAdministration.AddItem(new MenuItemDefinition(
            AppPageNames.Host.Settings,
            L("Settings"),
            url: "App/HostSettings",
            icon: "flaticon-settings",
            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_Host_Settings)
            ));

            mrAdministration.AddItem(new MenuItemDefinition(
            AppPageNames.Tenant.Settings,
            L("Settings"),
            url: "App/Settings",
            icon: "flaticon-settings",
            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_Tenant_Settings)
            ));

            menu.AddItem(menuItemsAll);
            menu.AddItem(mrDepts);
            menu.AddItem(mrChorts);
            menu.AddItem(mrAdministration);
            menu.AddItem(tenantSurpathServicesMenu);
            menu.AddItem(mrLib);
            menu.AddItem(menuRootRotationSlots);
            menu.AddItem(mrSurpathVals);
            menu.AddItem(mrHolders);
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, inzibackendConsts.LocalizationSourceName);
        }
    }
}