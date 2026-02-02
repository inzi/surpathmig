using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.Localization;
using Abp.MultiTenancy;

namespace inzibackend.Authorization
{
    /// <summary>
    /// Application's authorization provider.
    /// Defines permissions for the application.
    /// See <see cref="AppPermissions"/> for all permission names.
    /// </summary>
    public class AppAuthorizationProvider : AuthorizationProvider
    {
        private readonly bool _isMultiTenancyEnabled;

        public AppAuthorizationProvider(bool isMultiTenancyEnabled)
        {
            _isMultiTenancyEnabled = isMultiTenancyEnabled;
        }

        public AppAuthorizationProvider(IMultiTenancyConfig multiTenancyConfig)
        {
            _isMultiTenancyEnabled = multiTenancyConfig.IsEnabled;
        }

        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            //COMMON PERMISSIONS (FOR BOTH OF TENANTS AND HOST)

            var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));

            var medicalUnits = pages.CreateChildPermission(AppPermissions.Pages_MedicalUnits, L("MedicalUnits"));
            medicalUnits.CreateChildPermission(AppPermissions.Pages_MedicalUnits_Create, L("CreateNewMedicalUnit"));
            medicalUnits.CreateChildPermission(AppPermissions.Pages_MedicalUnits_Edit, L("EditMedicalUnit"));
            medicalUnits.CreateChildPermission(AppPermissions.Pages_MedicalUnits_Delete, L("DeleteMedicalUnit"));

            var rotationSlots = pages.CreateChildPermission(AppPermissions.Pages_RotationSlots, L("RotationSlots"));
            rotationSlots.CreateChildPermission(AppPermissions.Pages_RotationSlots_Create, L("CreateNewRotationSlot"));
            rotationSlots.CreateChildPermission(AppPermissions.Pages_RotationSlots_Edit, L("EditRotationSlot"));
            rotationSlots.CreateChildPermission(AppPermissions.Pages_RotationSlots_Delete, L("DeleteRotationSlot"));

            //var ledgerEntryDetails = pages.CreateChildPermission(AppPermissions.Pages_LedgerEntryDetails, L("LedgerEntryDetails"));
            //ledgerEntryDetails.CreateChildPermission(AppPermissions.Pages_LedgerEntryDetails_Create, L("CreateNewLedgerEntryDetail"));
            //ledgerEntryDetails.CreateChildPermission(AppPermissions.Pages_LedgerEntryDetails_Edit, L("EditLedgerEntryDetail"));
            //ledgerEntryDetails.CreateChildPermission(AppPermissions.Pages_LedgerEntryDetails_Delete, L("DeleteLedgerEntryDetail"));

            //var ledgerEntries = pages.CreateChildPermission(AppPermissions.Pages_LedgerEntries, L("LedgerEntries"));
            //ledgerEntries.CreateChildPermission(AppPermissions.Pages_LedgerEntries_Create, L("CreateNewLedgerEntry"));
            //ledgerEntries.CreateChildPermission(AppPermissions.Pages_LedgerEntries_Edit, L("EditLedgerEntry"));
            //ledgerEntries.CreateChildPermission(AppPermissions.Pages_LedgerEntries_Delete, L("DeleteLedgerEntry"));

            var tenantDocuments = pages.CreateChildPermission(AppPermissions.Pages_TenantDocuments, L("TenantDocuments"));
            tenantDocuments.CreateChildPermission(AppPermissions.Pages_TenantDocuments_Create, L("CreateNewTenantDocument"));
            tenantDocuments.CreateChildPermission(AppPermissions.Pages_TenantDocuments_Edit, L("EditTenantDocument"));
            tenantDocuments.CreateChildPermission(AppPermissions.Pages_TenantDocuments_Delete, L("DeleteTenantDocument"));

            var tenantDepartmentUsers = pages.CreateChildPermission(AppPermissions.Pages_TenantDepartmentUsers, L("TenantDepartmentUsers"));
            tenantDepartmentUsers.CreateChildPermission(AppPermissions.Pages_TenantDepartmentUsers_Create, L("CreateNewTenantDepartmentUser"));
            tenantDepartmentUsers.CreateChildPermission(AppPermissions.Pages_TenantDepartmentUsers_Edit, L("EditTenantDepartmentUser"));
            tenantDepartmentUsers.CreateChildPermission(AppPermissions.Pages_TenantDepartmentUsers_Delete, L("DeleteTenantDepartmentUser"));

            var recordStatuses = pages.CreateChildPermission(AppPermissions.Pages_RecordStatuses, L("RecordStatuses"));
            recordStatuses.CreateChildPermission(AppPermissions.Pages_RecordStatuses_Create, L("CreateNewRecordStatus"));
            recordStatuses.CreateChildPermission(AppPermissions.Pages_RecordStatuses_Edit, L("EditRecordStatus"));
            recordStatuses.CreateChildPermission(AppPermissions.Pages_RecordStatuses_Delete, L("DeleteRecordStatus"));

            var recordStates = pages.CreateChildPermission(AppPermissions.Pages_RecordStates, L("RecordStates"));
            recordStates.CreateChildPermission(AppPermissions.Pages_RecordStates_Create, L("CreateNewRecordState"));
            recordStates.CreateChildPermission(AppPermissions.Pages_RecordStates_Edit, L("EditRecordState"));
            recordStates.CreateChildPermission(AppPermissions.Pages_RecordStates_Delete, L("DeleteRecordState"));

            var tenantDocumentCategories = pages.CreateChildPermission(AppPermissions.Pages_TenantDocumentCategories, L("TenantDocumentCategories"));
            tenantDocumentCategories.CreateChildPermission(AppPermissions.Pages_TenantDocumentCategories_Create, L("CreateNewTenantDocumentCategory"));
            tenantDocumentCategories.CreateChildPermission(AppPermissions.Pages_TenantDocumentCategories_Edit, L("EditTenantDocumentCategory"));
            tenantDocumentCategories.CreateChildPermission(AppPermissions.Pages_TenantDocumentCategories_Delete, L("DeleteTenantDocumentCategory"));

            var deptCodes = pages.CreateChildPermission(AppPermissions.Pages_DeptCodes, L("DeptCodes"));
            deptCodes.CreateChildPermission(AppPermissions.Pages_DeptCodes_Create, L("CreateNewDeptCode"));
            deptCodes.CreateChildPermission(AppPermissions.Pages_DeptCodes_Edit, L("EditDeptCode"));
            deptCodes.CreateChildPermission(AppPermissions.Pages_DeptCodes_Delete, L("DeleteDeptCode"));

            var departmentUsers = pages.CreateChildPermission(AppPermissions.Pages_DepartmentUsers, L("DepartmentUsers"));
            departmentUsers.CreateChildPermission(AppPermissions.Pages_DepartmentUsers_Create, L("CreateNewDepartmentUser"));
            departmentUsers.CreateChildPermission(AppPermissions.Pages_DepartmentUsers_Edit, L("EditDepartmentUser"));
            departmentUsers.CreateChildPermission(AppPermissions.Pages_DepartmentUsers_Delete, L("DeleteDepartmentUser"));

            var cohortUsers = pages.CreateChildPermission(AppPermissions.Pages_CohortUsers, L("CohortUsers"));
            cohortUsers.CreateChildPermission(AppPermissions.Pages_CohortUsers_Create, L("CreateNewCohortUser"));
            cohortUsers.CreateChildPermission(AppPermissions.Pages_CohortUsers_Edit, L("EditCohortUser"));
            cohortUsers.CreateChildPermission(AppPermissions.Pages_CohortUsers_Delete, L("DeleteCohortUser"));
            cohortUsers.CreateChildPermission(AppPermissions.Pages_CohortUsers_Transfer, L("TransferCohortUser"));
            

            var cohortUser = pages.CreateChildPermission(AppPermissions.Pages_CohortUser, L("CohortUser"));
            cohortUser.CreateChildPermission(AppPermissions.Pages_CohortUser_Edit, L("EditCohortUserSelf"));

            var cohorts = pages.CreateChildPermission(AppPermissions.Pages_Cohorts, L("Cohorts"));
            cohorts.CreateChildPermission(AppPermissions.Pages_Cohorts_Create, L("CreateNewCohort"));
            cohorts.CreateChildPermission(AppPermissions.Pages_Cohorts_Edit, L("EditCohort"));
            cohorts.CreateChildPermission(AppPermissions.Pages_Cohorts_Delete, L("DeleteCohort"));
            cohorts.CreateChildPermission(AppPermissions.Pages_Cohorts_Migrate, L("MigrateCohort"));
            cohorts.CreateChildPermission(AppPermissions.Pages_Cohorts_MigrateBetweenDepartments, L("MigrateCohortBetweenDepartments"));
            cohorts.CreateChildPermission(AppPermissions.Pages_Cohorts_CreateDepartment, L("CreateDepartmentDuringMigration"));
            cohorts.CreateChildPermission(AppPermissions.Pages_Cohorts_ViewMigrationHistory, L("ViewMigrationHistory"));
            cohorts.CreateChildPermission(AppPermissions.Pages_Cohorts_RollbackMigration, L("RollbackMigration"));

            var tenantDepartments = pages.CreateChildPermission(AppPermissions.Pages_TenantDepartments, L("TenantDepartments"));
            tenantDepartments.CreateChildPermission(AppPermissions.Pages_TenantDepartments_Create, L("CreateNewTenantDepartment"));
            tenantDepartments.CreateChildPermission(AppPermissions.Pages_TenantDepartments_Edit, L("EditTenantDepartment"));
            tenantDepartments.CreateChildPermission(AppPermissions.Pages_TenantDepartments_Delete, L("DeleteTenantDepartment"));

            var confirmationValues = pages.CreateChildPermission(AppPermissions.Pages_ConfirmationValues, L("ConfirmationValues"), multiTenancySides: MultiTenancySides.Host);
            confirmationValues.CreateChildPermission(AppPermissions.Pages_ConfirmationValues_Create, L("CreateNewConfirmationValue"), multiTenancySides: MultiTenancySides.Host);
            confirmationValues.CreateChildPermission(AppPermissions.Pages_ConfirmationValues_Edit, L("EditConfirmationValue"), multiTenancySides: MultiTenancySides.Host);
            confirmationValues.CreateChildPermission(AppPermissions.Pages_ConfirmationValues_Delete, L("DeleteConfirmationValue"), multiTenancySides: MultiTenancySides.Host);

            var recordNotes = pages.CreateChildPermission(AppPermissions.Pages_RecordNotes, L("RecordNotes"));
            recordNotes.CreateChildPermission(AppPermissions.Pages_RecordNotes_Create, L("CreateNewRecordNote"));
            recordNotes.CreateChildPermission(AppPermissions.Pages_RecordNotes_Edit, L("EditRecordNote"));
            recordNotes.CreateChildPermission(AppPermissions.Pages_RecordNotes_Delete, L("DeleteRecordNote"));

            var recordCategories = pages.CreateChildPermission(AppPermissions.Pages_RecordCategories, L("RecordCategories"));
            recordCategories.CreateChildPermission(AppPermissions.Pages_RecordCategories_Create, L("CreateNewRecordCategory"));
            recordCategories.CreateChildPermission(AppPermissions.Pages_RecordCategories_Edit, L("EditRecordCategory"));
            recordCategories.CreateChildPermission(AppPermissions.Pages_RecordCategories_Delete, L("DeleteRecordCategory"));

            var recordCategoryRules = pages.CreateChildPermission(AppPermissions.Pages_RecordCategoryRules, L("RecordCategoryRules"));
            recordCategoryRules.CreateChildPermission(AppPermissions.Pages_RecordCategoryRules_Create, L("CreateNewRecordCategoryRule"));
            recordCategoryRules.CreateChildPermission(AppPermissions.Pages_RecordCategoryRules_Edit, L("EditRecordCategoryRule"));
            recordCategoryRules.CreateChildPermission(AppPermissions.Pages_RecordCategoryRules_Delete, L("DeleteRecordCategoryRule"));

            var records = pages.CreateChildPermission(AppPermissions.Pages_Records, L("Records"));
            records.CreateChildPermission(AppPermissions.Pages_Records_Create, L("CreateNewRecord"));
            records.CreateChildPermission(AppPermissions.Pages_Records_Edit, L("EditRecord"));
            records.CreateChildPermission(AppPermissions.Pages_Records_Delete, L("DeleteRecord"));

            var testCategories = pages.CreateChildPermission(AppPermissions.Pages_TestCategories, L("TestCategories"), multiTenancySides: MultiTenancySides.Host);
            testCategories.CreateChildPermission(AppPermissions.Pages_TestCategories_Create, L("CreateNewTestCategory"), multiTenancySides: MultiTenancySides.Host);
            testCategories.CreateChildPermission(AppPermissions.Pages_TestCategories_Edit, L("EditTestCategory"), multiTenancySides: MultiTenancySides.Host);
            testCategories.CreateChildPermission(AppPermissions.Pages_TestCategories_Delete, L("DeleteTestCategory"), multiTenancySides: MultiTenancySides.Host);

            pages.CreateChildPermission(AppPermissions.Pages_DemoUiComponents, L("DemoUiComponents"));

            var administration = pages.CreateChildPermission(AppPermissions.Pages_Administration, L("Administration"));

            // User Purchases - only accessible by the host
            var userPurchases = administration.CreateChildPermission(AppPermissions.Pages_UserPurchases, L("UserPurchases"), multiTenancySides: MultiTenancySides.Host);
            userPurchases.CreateChildPermission(AppPermissions.Pages_UserPurchases_Create, L("CreateNewUserPurchase"), multiTenancySides: MultiTenancySides.Host);
            userPurchases.CreateChildPermission(AppPermissions.Pages_UserPurchases_Edit, L("EditUserPurchase"), multiTenancySides: MultiTenancySides.Host);
            userPurchases.CreateChildPermission(AppPermissions.Pages_UserPurchases_Delete, L("DeleteUserPurchase"), multiTenancySides: MultiTenancySides.Host);
            userPurchases.CreateChildPermission(AppPermissions.Pages_UserPurchases_ApplyPayment, L("ApplyPayment"), multiTenancySides: MultiTenancySides.Host);
            userPurchases.CreateChildPermission(AppPermissions.Pages_UserPurchases_AdjustPrice, L("AdjustPrice"), multiTenancySides: MultiTenancySides.Host);

            var hospitals = administration.CreateChildPermission(AppPermissions.Pages_Administration_Hospitals, L("Hospitals"));
            hospitals.CreateChildPermission(AppPermissions.Pages_Administration_Hospitals_Create, L("CreateNewHospital"));
            hospitals.CreateChildPermission(AppPermissions.Pages_Administration_Hospitals_Edit, L("EditHospital"));
            hospitals.CreateChildPermission(AppPermissions.Pages_Administration_Hospitals_Delete, L("DeleteHospital"));

            var welcomemessages = administration.CreateChildPermission(AppPermissions.Pages_Administration_Welcomemessages, L("Welcomemessages"), multiTenancySides: MultiTenancySides.Tenant);
            welcomemessages.CreateChildPermission(AppPermissions.Pages_Administration_Welcomemessages_Create, L("CreateNewWelcomemessage"), multiTenancySides: MultiTenancySides.Tenant);
            welcomemessages.CreateChildPermission(AppPermissions.Pages_Administration_Welcomemessages_Edit, L("EditWelcomemessage"), multiTenancySides: MultiTenancySides.Tenant);
            welcomemessages.CreateChildPermission(AppPermissions.Pages_Administration_Welcomemessages_Delete, L("DeleteWelcomemessage"), multiTenancySides: MultiTenancySides.Tenant);

            var drugTestCategories = administration.CreateChildPermission(AppPermissions.Pages_Administration_DrugTestCategories, L("DrugTestCategories"), multiTenancySides: MultiTenancySides.Host);
            drugTestCategories.CreateChildPermission(AppPermissions.Pages_Administration_DrugTestCategories_Create, L("CreateNewDrugTestCategory"), multiTenancySides: MultiTenancySides.Host);
            drugTestCategories.CreateChildPermission(AppPermissions.Pages_Administration_DrugTestCategories_Edit, L("EditDrugTestCategory"), multiTenancySides: MultiTenancySides.Host);
            drugTestCategories.CreateChildPermission(AppPermissions.Pages_Administration_DrugTestCategories_Delete, L("DeleteDrugTestCategory"), multiTenancySides: MultiTenancySides.Host);

            var codeTypes = administration.CreateChildPermission(AppPermissions.Pages_Administration_CodeTypes, L("CodeTypes"));
            codeTypes.CreateChildPermission(AppPermissions.Pages_Administration_CodeTypes_Create, L("CreateNewCodeType"));
            codeTypes.CreateChildPermission(AppPermissions.Pages_Administration_CodeTypes_Edit, L("EditCodeType"));
            codeTypes.CreateChildPermission(AppPermissions.Pages_Administration_CodeTypes_Delete, L("DeleteCodeType"));

            var recordRequirements = administration.CreateChildPermission(AppPermissions.Pages_Administration_RecordRequirements, L("RecordRequirements"));
            recordRequirements.CreateChildPermission(AppPermissions.Pages_Administration_RecordRequirements_Create, L("CreateNewRecordRequirement"));
            recordRequirements.CreateChildPermission(AppPermissions.Pages_Administration_RecordRequirements_Edit, L("EditRecordRequirement"));
            recordRequirements.CreateChildPermission(AppPermissions.Pages_Administration_RecordRequirements_Delete, L("DeleteRecordRequirement"));
            recordRequirements.CreateChildPermission(AppPermissions.Pages_Administration_RecordRequirements_ManageCategories, L("ManageCategories"));
            recordRequirements.CreateChildPermission(AppPermissions.Pages_Administration_RecordRequirements_MoveCategories, L("MoveCategories"));
            recordRequirements.CreateChildPermission(AppPermissions.Pages_Administration_RecordRequirements_CopyCategories, L("CopyCategories"));

            var drugPanels = administration.CreateChildPermission(AppPermissions.Pages_Administration_DrugPanels, L("DrugPanels"), multiTenancySides: MultiTenancySides.Host);
            drugPanels.CreateChildPermission(AppPermissions.Pages_Administration_DrugPanels_Create, L("CreateNewDrugPanel"), multiTenancySides: MultiTenancySides.Host);
            drugPanels.CreateChildPermission(AppPermissions.Pages_Administration_DrugPanels_Edit, L("EditDrugPanel"), multiTenancySides: MultiTenancySides.Host);
            drugPanels.CreateChildPermission(AppPermissions.Pages_Administration_DrugPanels_Delete, L("DeleteDrugPanel"), multiTenancySides: MultiTenancySides.Host);

            var panels = administration.CreateChildPermission(AppPermissions.Pages_Administration_Panels, L("Panels"), multiTenancySides: MultiTenancySides.Host);
            panels.CreateChildPermission(AppPermissions.Pages_Administration_Panels_Create, L("CreateNewPanel"), multiTenancySides: MultiTenancySides.Host);
            panels.CreateChildPermission(AppPermissions.Pages_Administration_Panels_Edit, L("EditPanel"), multiTenancySides: MultiTenancySides.Host);
            panels.CreateChildPermission(AppPermissions.Pages_Administration_Panels_Delete, L("DeletePanel"), multiTenancySides: MultiTenancySides.Host);

            var drugs = administration.CreateChildPermission(AppPermissions.Pages_Administration_Drugs, L("Drugs"), multiTenancySides: MultiTenancySides.Host);
            drugs.CreateChildPermission(AppPermissions.Pages_Administration_Drugs_Create, L("CreateNewDrug"), multiTenancySides: MultiTenancySides.Host);
            drugs.CreateChildPermission(AppPermissions.Pages_Administration_Drugs_Edit, L("EditDrug"), multiTenancySides: MultiTenancySides.Host);
            drugs.CreateChildPermission(AppPermissions.Pages_Administration_Drugs_Delete, L("DeleteDrug"), multiTenancySides: MultiTenancySides.Host);

            var roles = administration.CreateChildPermission(AppPermissions.Pages_Administration_Roles, L("Roles"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Create, L("CreatingNewRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Edit, L("EditingRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Delete, L("DeletingRole"));

            var users = administration.CreateChildPermission(AppPermissions.Pages_Administration_Users, L("Users"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Create, L("CreatingNewUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Edit, L("EditingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Delete, L("DeletingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_ChangePermissions, L("ChangingPermissions"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Impersonation, L("LoginForUsers"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Unlock, L("Unlock"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_ChangeProfilePicture, L("UpdateUsersProfilePicture"));

            var languages = administration.CreateChildPermission(AppPermissions.Pages_Administration_Languages, L("Languages"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Create, L("CreatingNewLanguage"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Edit, L("EditingLanguage"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Delete, L("DeletingLanguages"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_ChangeTexts, L("ChangingTexts"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_ChangeDefaultLanguage, L("ChangeDefaultLanguage"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_AuditLogs, L("AuditLogs"));

            var organizationUnits = administration.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits, L("OrganizationUnits"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageOrganizationTree, L("ManagingOrganizationTree"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageMembers, L("ManagingMembers"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageRoles, L("ManagingRoles"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageDepartments, L("ManageDepartments"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_UiCustomization, L("VisualSettings"));

            var webhooks = administration.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription, L("Webhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Create, L("CreatingWebhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Edit, L("EditingWebhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_ChangeActivity, L("ChangingWebhookActivity"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Detail, L("DetailingSubscription"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_Webhook_ListSendAttempts, L("ListingSendAttempts"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_Webhook_ResendWebhook, L("ResendingWebhook"));

            var dynamicProperties = administration.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties, L("DynamicProperties"));
            dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties_Create, L("CreatingDynamicProperties"));
            dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties_Edit, L("EditingDynamicProperties"));
            dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties_Delete, L("DeletingDynamicProperties"));

            var dynamicPropertyValues = dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue, L("DynamicPropertyValue"));
            dynamicPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue_Create, L("CreatingDynamicPropertyValue"));
            dynamicPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue_Edit, L("EditingDynamicPropertyValue"));
            dynamicPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue_Delete, L("DeletingDynamicPropertyValue"));

            var dynamicEntityProperties = dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityProperties, L("DynamicEntityProperties"));
            dynamicEntityProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityProperties_Create, L("CreatingDynamicEntityProperties"));
            dynamicEntityProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityProperties_Edit, L("EditingDynamicEntityProperties"));
            dynamicEntityProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityProperties_Delete, L("DeletingDynamicEntityProperties"));

            var dynamicEntityPropertyValues = dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityPropertyValue, L("EntityDynamicPropertyValue"));
            dynamicEntityPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityPropertyValue_Create, L("CreatingDynamicEntityPropertyValue"));
            dynamicEntityPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityPropertyValue_Edit, L("EditingDynamicEntityPropertyValue"));
            dynamicEntityPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityPropertyValue_Delete, L("DeletingDynamicEntityPropertyValue"));

            //var massNotification = administration.CreateChildPermission(AppPermissions.Pages_Administration_MassNotification, L("MassNotifications"));
            //massNotification.CreateChildPermission(AppPermissions.Pages_Administration_MassNotification_Create, L("MassNotificationCreate"));

            //TENANT-SPECIFIC PERMISSIONS

            pages.CreateChildPermission(AppPermissions.Pages_Tenant_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Tenant);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_SubscriptionManagement, L("Subscription"), multiTenancySides: MultiTenancySides.Tenant);

            var legalDocuments = administration.CreateChildPermission(AppPermissions.Pages_Administration_LegalDocuments, L("LegalDocuments"), multiTenancySides: MultiTenancySides.Tenant);
            legalDocuments.CreateChildPermission(AppPermissions.Pages_Administration_LegalDocuments_View, L("ViewLegalDocuments"), multiTenancySides: MultiTenancySides.Tenant);
            legalDocuments.CreateChildPermission(AppPermissions.Pages_Administration_LegalDocuments_CreateEdit, L("CreateEditLegalDocuments"), multiTenancySides: MultiTenancySides.Tenant);

            //HOST-SPECIFIC PERMISSIONS

            var editions = pages.CreateChildPermission(AppPermissions.Pages_Editions, L("Editions"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Create, L("CreatingNewEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Edit, L("EditingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Delete, L("DeletingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_MoveTenantsToAnotherEdition, L("MoveTenantsToAnotherEdition"), multiTenancySides: MultiTenancySides.Host);

            var tenants = pages.CreateChildPermission(AppPermissions.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Create, L("CreatingNewTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Edit, L("EditingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_ChangeFeatures, L("ChangingFeatures"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Delete, L("DeletingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Impersonation, L("LoginForTenants"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_ImportUsers, L("ImportUsers"), multiTenancySides: MultiTenancySides.Host);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Host);

            var maintenance = administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Maintenance, L("Maintenance"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            //maintenance.CreateChildPermission(AppPermissions.Pages_Administration_NewVersion_Create, L("SendNewVersionNotification"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_HangfireDashboard, L("HangfireDashboard"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Host);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_BGCExport, L("BGCExport"), multiTenancySides: MultiTenancySides.Host);

            //var Surpath = pages.CreateChildPermission(AppPermissions.Pages_Surpath, L("Pages_Surpath"), multiTenancySides: MultiTenancySides.Host);
            //Surpath.CreateChildPermission(AppPermissions.Surpath_Administration_Login_Activity, L("Surpath_Administration_Login_Activity"), multiTenancySides: MultiTenancySides.Host);
            //Surpath.CreateChildPermission(AppPermissions.Surpath_Administration_Surpath_Change_Status, L("Surpath_Administration_Surpath_Change_Status"));
            //Surpath.CreateChildPermission(AppPermissions.Surpath_Administration_Surpath_Encryption_Tools, L("Surpath_Administration_Surpath_Encryption_Tools"), multiTenancySides: MultiTenancySides.Host);
            //Surpath.CreateChildPermission(AppPermissions.Surpath_Administration_Surpath_Mismatch_Tools, L("Surpath_Administration_Surpath_Mismatch_Tools"), multiTenancySides: MultiTenancySides.Host);

            ////var surpath = context.GetPermissionOrNull(AppPermissions.Surpath) ?? context.CreatePermission(AppPermissions.Surpath, L("Surpath"));
            ////var surpathPermissions = surpath.CreateChildPermission(AppPermissions.Surpath, L("Surpath_Permissions"), multiTenancySides: MultiTenancySides.Host);
            ////surpathPermissions.CreateChildPermission(AppPermissions.Surpath_Administration_Login_Activity, L("Surpath_Administration_Login_Activity"), multiTenancySides: MultiTenancySides.Host);
            ////surpathPermissions.CreateChildPermission(AppPermissions.Surpath_Administration_Surpath_Change_Status, L("Surpath_Administration_Surpath_Change_Status"));
            ////surpathPermissions.CreateChildPermission(AppPermissions.Surpath_Administration_Surpath_Compliance_Review, L("Surpath_Administration_Surpath_Compliance_Review"));
            ////surpathPermissions.CreateChildPermission(AppPermissions.Surpath_Administration_Surpath_Encryption_Tools, L("Surpath_Administration_Surpath_Encryption_Tools"), multiTenancySides: MultiTenancySides.Host);
            ////surpathPermissions.CreateChildPermission(AppPermissions.Surpath_Administration_Surpath_Mismatch_Tools, L("Surpath_Administration_Surpath_Mismatch_Tools"), multiTenancySides: MultiTenancySides.Host);

            var surpath = context.GetPermissionOrNull(AppPermissions.Surpath) ?? context.CreatePermission(AppPermissions.Surpath, L("Surpath"));
            surpath.CreateChildPermission(AppPermissions.Surpath_Administration_Login_Activity, L("Surpath_Administration_Login_Activity"), multiTenancySides: MultiTenancySides.Host);
            surpath.CreateChildPermission(AppPermissions.Surpath_Administration_Surpath_Change_Status, L("Surpath_Administration_Surpath_Change_Status"));
            surpath.CreateChildPermission(AppPermissions.Surpath_Administration_Surpath_Compliance_Review, L("Surpath_Administration_Surpath_Compliance_Review"));
            surpath.CreateChildPermission(AppPermissions.Surpath_Administration_Surpath_Encryption_Tools, L("Surpath_Administration_Surpath_Encryption_Tools"), multiTenancySides: MultiTenancySides.Host);
            surpath.CreateChildPermission(AppPermissions.Surpath_Administration_Surpath_Mismatch_Tools, L("Surpath_Administration_Surpath_Mismatch_Tools"), multiTenancySides: MultiTenancySides.Host);
            surpath.CreateChildPermission(AppPermissions.Surpath_Administration_Manual_Send_In, L("Surpath_Administration_Manual_Send_In"), multiTenancySides: MultiTenancySides.Host);
            surpath.CreateChildPermission(AppPermissions.Surpath_Administration_Schedule_Send_In, L("Surpath_Administration_Schedule_Send_In"), multiTenancySides: MultiTenancySides.Host);
            surpath.CreateChildPermission(AppPermissions.Surpath_Administration_View_Masked_Pids, L("Surpath_Administration_View_Masked_Pids"), multiTenancySides: MultiTenancySides.Host);
            surpath.CreateChildPermission(AppPermissions.Surpath_View_Host_Only_notes, L("Surpath_View_Host_Only_notes"), multiTenancySides: MultiTenancySides.Host);
            surpath.CreateChildPermission(AppPermissions.Surpath_View_Authorized_Only_notes, L("Surpath_View_Authorized_Only_notes"));
            surpath.CreateChildPermission(AppPermissions.Surpath_Drug_Screen_Download, L("Surpath_Drug_Screen_Download"));
            surpath.CreateChildPermission(AppPermissions.Surpath_Drug_Screen_Show_Status, L("Surpath_Drug_Screen_Show_Status"));
            surpath.CreateChildPermission(AppPermissions.Surpath_Drug_Screen_Change_Status, L("Surpath_Drug_Screen_Change_Status"));
            surpath.CreateChildPermission(AppPermissions.Surpath_Background_Check_Download, L("Surpath_Background_Check_Download"));
            surpath.CreateChildPermission(AppPermissions.Surpath_Background_Check_Show_Status, L("Surpath_Background_Check_Show_Status"));
            surpath.CreateChildPermission(AppPermissions.Surpath_Background_Check_Change_Status, L("Surpath_Background_Check_Change_Status"));
            //surpath.CreateChildPermission(AppPermissions.Surpath_Review_Requirement_View, L("Surpath_Review_Requirement_View"));
            surpath.CreateChildPermission(AppPermissions.Surpath_Review_Requirement_Change, L("Surpath_Review_Requirement_Change"));

            //surpath.CreateChildPermission(AppPermissions.Surpath_Ledger_Read, L("SurpathLedgerRead"));
            //surpath.CreateChildPermission(AppPermissions.Surpath_Ledger_Edit, L("SurpathLedgerEdit"));
            //surpath.CreateChildPermission(AppPermissions.Surpath_Ledger_Create, L("SurpathLedgerCreate"));
            //surpath.CreateChildPermission(AppPermissions.Surpath_Ledger_Delete, L("SurpathLedgerDelete"));

            var surpathApi = context.GetPermissionOrNull(AppPermissions.SurpathApi) ?? context.CreatePermission(AppPermissions.SurpathApi, L("Surpath_API"));
            surpathApi.CreateChildPermission(AppPermissions.Surpath_Api_Access, L("Surpath_Api_Access"), multiTenancySides: MultiTenancySides.Host);
            surpathApi.CreateChildPermission(AppPermissions.Surpath_Api_Integration, L("Surpath_Api_Integration"), multiTenancySides: MultiTenancySides.Host);

            // Payment popup permissions
            var paymentPopupConfig = administration.CreateChildPermission(AppPermissions.Pages_PaymentPopup_Configure, L("PaymentPopupConfiguration"));
            paymentPopupConfig.CreateChildPermission(AppPermissions.Pages_PaymentPopup_Configure_Global, L("GlobalPaymentPopupConfiguration"), multiTenancySides: MultiTenancySides.Host);
            paymentPopupConfig.CreateChildPermission(AppPermissions.Pages_PaymentPopup_Configure_ForCohort, L("CohortPaymentPopupConfiguration"));
            paymentPopupConfig.CreateChildPermission(AppPermissions.Pages_PaymentPopup_Configure_ForDepartment, L("DepartmentPaymentPopupConfiguration"));

            var ledgerEntryDetails = administration.CreateChildPermission(AppPermissions.Pages_LedgerEntryDetails, L("LedgerEntryDetails"), multiTenancySides: MultiTenancySides.Host);
            ledgerEntryDetails.CreateChildPermission(AppPermissions.Pages_LedgerEntryDetails_Create, L("CreateNewLedgerEntryDetail"), multiTenancySides: MultiTenancySides.Host);
            ledgerEntryDetails.CreateChildPermission(AppPermissions.Pages_LedgerEntryDetails_Edit, L("EditLedgerEntryDetail"), multiTenancySides: MultiTenancySides.Host);
            ledgerEntryDetails.CreateChildPermission(AppPermissions.Pages_LedgerEntryDetails_Delete, L("DeleteLedgerEntryDetail"), multiTenancySides: MultiTenancySides.Host);

            var tenantSurpathServices = pages.CreateChildPermission(AppPermissions.Pages_TenantSurpathServices, L("TenantSurpathServices"), multiTenancySides: MultiTenancySides.Host);
            tenantSurpathServices.CreateChildPermission(AppPermissions.Pages_TenantSurpathServices_Create, L("CreateNewTenantSurpathService"), multiTenancySides: MultiTenancySides.Host);
            tenantSurpathServices.CreateChildPermission(AppPermissions.Pages_TenantSurpathServices_Edit, L("EditTenantSurpathService"), multiTenancySides: MultiTenancySides.Host);
            tenantSurpathServices.CreateChildPermission(AppPermissions.Pages_TenantSurpathServices_Delete, L("DeleteTenantSurpathService"), multiTenancySides: MultiTenancySides.Host);
            tenantSurpathServices.CreateChildPermission(AppPermissions.Pages_TenantSurpathServices_ManageOU, L("ManageOUTenantSurpathService"));
            tenantSurpathServices.CreateChildPermission(AppPermissions.Pages_TenantSurpathServices_ManageCohort, L("ManageCohortTenantSurpathService"));
            tenantSurpathServices.CreateChildPermission(AppPermissions.Pages_TenantSurpathServices_ManageDepartment, L("ManageDepartmentTenantSurpathService"));
            tenantSurpathServices.CreateChildPermission(AppPermissions.Pages_TenantSurpathServices_AssignToTenant, L("AssignToTenantTenantSurpathService"));

            var surpathServices = administration.CreateChildPermission(AppPermissions.Pages_SurpathServices, L("SurpathServices"), multiTenancySides: MultiTenancySides.Host);
            surpathServices.CreateChildPermission(AppPermissions.Pages_SurpathServices_Create, L("CreateNewSurpathService"), multiTenancySides: MultiTenancySides.Host);
            surpathServices.CreateChildPermission(AppPermissions.Pages_SurpathServices_Edit, L("EditSurpathService"), multiTenancySides: MultiTenancySides.Host);
            surpathServices.CreateChildPermission(AppPermissions.Pages_SurpathServices_Delete, L("DeleteSurpathService"), multiTenancySides: MultiTenancySides.Host);

            var ledgerEntries = administration.CreateChildPermission(AppPermissions.Pages_LedgerEntries, L("LedgerEntries"), multiTenancySides: MultiTenancySides.Host);
            ledgerEntries.CreateChildPermission(AppPermissions.Pages_LedgerEntries_Create, L("CreateNewLedgerEntry"), multiTenancySides: MultiTenancySides.Host);
            ledgerEntries.CreateChildPermission(AppPermissions.Pages_LedgerEntries_Edit, L("EditLedgerEntry"), multiTenancySides: MultiTenancySides.Host);
            ledgerEntries.CreateChildPermission(AppPermissions.Pages_LedgerEntries_Delete, L("DeleteLedgerEntry"), multiTenancySides: MultiTenancySides.Host);

            var pidTypes = administration.CreateChildPermission(AppPermissions.Pages_PidTypes, L("PidTypes"), multiTenancySides: MultiTenancySides.Host);
            pidTypes.CreateChildPermission(AppPermissions.Pages_PidTypes_Create, L("CreateNewPidType"), multiTenancySides: MultiTenancySides.Host);
            pidTypes.CreateChildPermission(AppPermissions.Pages_PidTypes_Edit, L("EditPidType"), multiTenancySides: MultiTenancySides.Host);
            pidTypes.CreateChildPermission(AppPermissions.Pages_PidTypes_Delete, L("DeletePidType"), multiTenancySides: MultiTenancySides.Host);

            var userPids = administration.CreateChildPermission(AppPermissions.Pages_UserPids, L("UserPids"), multiTenancySides: MultiTenancySides.Host);
            userPids.CreateChildPermission(AppPermissions.Pages_UserPids_Create, L("CreateNewUserPid"), multiTenancySides: MultiTenancySides.Host);
            userPids.CreateChildPermission(AppPermissions.Pages_UserPids_Edit, L("EditUserPid"), multiTenancySides: MultiTenancySides.Host);
            userPids.CreateChildPermission(AppPermissions.Pages_UserPids_Delete, L("DeleteUserPid"), multiTenancySides: MultiTenancySides.Host);

            //    public const string Api = "SurpathAPI";
            //public const string Surpath_Api_Access = "Surpath.Host.ApiAccess";
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, inzibackendConsts.LocalizationSourceName);
        }
    }
}