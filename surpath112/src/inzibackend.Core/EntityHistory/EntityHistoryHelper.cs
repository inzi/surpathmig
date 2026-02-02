using inzibackend.Surpath;
using System;
using System.Linq;
using Abp.Organizations;
using inzibackend.Authorization.Roles;
using inzibackend.MultiTenancy;

namespace inzibackend.EntityHistory
{
    public static class EntityHistoryHelper
    {
        public const string EntityHistoryConfigurationName = "EntityHistory";

        public static readonly Type[] HostSideTrackedTypes =
        {
            typeof(MedicalUnit),
            typeof(RotationSlot),
            typeof(Hospital),
            typeof(Welcomemessage),
            typeof(TenantSurpathService),
            typeof(LedgerEntryDetail),
            typeof(SurpathService),
            typeof(LedgerEntry),
            typeof(PidType),
            typeof(UserPid),
            typeof(TenantDocument),
            typeof(TenantDepartmentUser),
            typeof(RecordStatus),
            typeof(RecordState),
            typeof(TenantDocumentCategory),
            typeof(DrugTestCategory),
            typeof(DeptCode),
            typeof(DepartmentUser),
            typeof(CohortUser),
            typeof(Cohort),
            typeof(TenantDepartment),
            typeof(ConfirmationValue),
            typeof(CodeType),
            typeof(RecordNote),
            typeof(RecordCategory),
            typeof(RecordRequirement),
            typeof(RecordCategoryRule),
            typeof(Record),
            typeof(DrugPanel),
            typeof(Panel),
            typeof(TestCategory),
            typeof(Drug),
            typeof(OrganizationUnit), typeof(Role), typeof(Tenant)
        };

        public static readonly Type[] TenantSideTrackedTypes =
        {
            typeof(MedicalUnit),
            typeof(RotationSlot),
            typeof(Hospital),
            typeof(Welcomemessage),
            typeof(TenantSurpathService),
            typeof(LedgerEntryDetail),
            typeof(SurpathService),
            typeof(LedgerEntry),
            typeof(PidType),
            typeof(UserPid),
            typeof(TenantDocument),
            typeof(TenantDepartmentUser),
            typeof(RecordStatus),
            typeof(RecordState),
            typeof(TenantDocumentCategory),
            typeof(DeptCode),
            typeof(DepartmentUser),
            typeof(CohortUser),
            typeof(Cohort),
            typeof(TenantDepartment),
            typeof(CodeType),
            typeof(RecordNote),
            typeof(RecordCategory),
            typeof(RecordRequirement),
            typeof(RecordCategoryRule),
            typeof(Record),
            typeof(OrganizationUnit), typeof(Role)
        };

        public static readonly Type[] TrackedTypes =
            HostSideTrackedTypes
                .Concat(TenantSideTrackedTypes)
                .GroupBy(type => type.FullName)
                .Select(types => types.First())
                .ToArray();
    }
}