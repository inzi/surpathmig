using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using inzibackend.Authorization.Users;
using inzibackend.Features;

namespace inzibackend.Surpath.Compliance
{
    public static class SurpathOnlyRequirements
    {

        public static IEnumerable<TenantRequirement> GetAllSurpathRequirementsFromComplianceInfo(ComplianceInfo _complianceInfo,
            IEnumerable<Guid> userRecordRequirementIds = null)
        {
            var result = new List<TenantRequirement>();
            var _drug = GetSurpathRequirementFromComplianceInfo(_complianceInfo, AppFeatures.SurpathFeatureDrugTest, userRecordRequirementIds);
            if (_drug != null) result.Add(_drug);
            var _bcg = GetSurpathRequirementFromComplianceInfo(_complianceInfo, AppFeatures.SurpathFeatureBackgroundCheck, userRecordRequirementIds);
            if (_bcg != null) result.Add(_bcg);
            return result;
        }

        public static TenantRequirement GetSurpathRequirementFromComplianceInfo(ComplianceInfo _complianceInfo, string featureIdentifier,
            IEnumerable<Guid> userRecordRequirementIds = null)
        {
            // Filter by FeatureIdentifier if available, fallback to name matching for backward compatibility
            var _surpathOnlyRequirements = _complianceInfo.SurpathOnlyRequirements
                .Where(_r =>
                    // Prefer FeatureIdentifier matching
                    (_r.RecordRequirement.SurpathServiceFk != null &&
                     _r.RecordRequirement.SurpathServiceFk.FeatureIdentifier == featureIdentifier) ||
                    // Fallback to name matching for legacy data without FeatureIdentifier
                    (_r.RecordRequirement.SurpathServiceFk == null &&
                     featureIdentifier == AppFeatures.SurpathFeatureDrugTest &&
                     _r.RecordRequirement.Name.ToLower().Contains("drug")) ||
                    (_r.RecordRequirement.SurpathServiceFk == null &&
                     featureIdentifier == AppFeatures.SurpathFeatureBackgroundCheck &&
                     _r.RecordRequirement.Name.ToLower().Contains("background"))
                )
                .ToList();

            // NEW: First check if user has existing records for any of these requirements
            // This ensures we use the service they're actually enrolled in
            if (userRecordRequirementIds != null && userRecordRequirementIds.Any())
            {
                var existingRequirement = _surpathOnlyRequirements
                    .FirstOrDefault(_r => userRecordRequirementIds.Contains(_r.RecordRequirement.Id));

                if (existingRequirement != null)
                {
                    // User has records for this specific requirement - use it!
                    return existingRequirement;
                }
            }

            if (_complianceInfo.UserId != null || _complianceInfo.UserId != 0 )
            {
                // UserId assigned?
                var userRequirement = _surpathOnlyRequirements
                    .FirstOrDefault(_r => _r.RecordRequirement.TenantSurpathServiceFk!=null && _r.RecordRequirement.TenantSurpathServiceFk.UserId != _complianceInfo.UserId);
                if (userRequirement != null) return userRequirement;
            }


            // First check: Cohort-specific requirement (highest priority)
            var cohortRequirement = _surpathOnlyRequirements
                .FirstOrDefault(_r => _r.RecordRequirement.CohortId != null &&
                                     _complianceInfo.UserMembershipsList.Any(m => m.CohortId == _r.RecordRequirement.CohortId));
            if (cohortRequirement != null) return cohortRequirement;

            // Second check: Department-specific requirement
            var departmentRequirement = _surpathOnlyRequirements
                .FirstOrDefault(_r => _r.RecordRequirement.TenantDepartmentId != null &&
                                     _complianceInfo.UserDeptList.Contains(_r.RecordRequirement.TenantDepartmentId));
            if (departmentRequirement != null) return departmentRequirement;

            // Last check: Tenant-wide requirement (lowest priority)
            var finalResult = _surpathOnlyRequirements
                .FirstOrDefault(_r => _r.RecordRequirement.TenantDepartmentId == null &&
                                     _r.RecordRequirement.CohortId == null);
            return finalResult;
        }


        public static TenantRequirement GetSurpathRequirementFromRequirements(
            IEnumerable<TenantRequirement> requirements,
            CohortUser _cohortUser,
            string featureIdentifier,
            IEnumerable<Guid> userRecordRequirementIds = null)
        {
            // Filter by FeatureIdentifier if available, fallback to name matching for backward compatibility
            var _surpathOnlyRequirements = requirements
                .Where(_r =>
                    // Prefer FeatureIdentifier matching
                    (_r.RecordRequirement.SurpathServiceFk != null &&
                     _r.RecordRequirement.SurpathServiceFk.FeatureIdentifier == featureIdentifier) ||
                    // Fallback to name matching for legacy data without FeatureIdentifier
                    (_r.RecordRequirement.SurpathServiceFk == null &&
                     featureIdentifier == AppFeatures.SurpathFeatureDrugTest &&
                     _r.RecordRequirement.Name.ToLower().Contains("drug")) ||
                    (_r.RecordRequirement.SurpathServiceFk == null &&
                     featureIdentifier == AppFeatures.SurpathFeatureBackgroundCheck &&
                     _r.RecordRequirement.Name.ToLower().Contains("background"))
                )
                .ToList();

            // NEW: First check if user has existing records for any of these requirements
            // This ensures we use the service they're actually enrolled in
            if (userRecordRequirementIds != null && userRecordRequirementIds.Any())
            {
                var existingRequirement = _surpathOnlyRequirements
                    .FirstOrDefault(_r => userRecordRequirementIds.Contains(_r.RecordRequirement.Id));

                if (existingRequirement != null)
                {
                    // User has records for this specific requirement - use it!
                    return existingRequirement;
                }
            }

            // UserId assigned?
            var userRequirement = _surpathOnlyRequirements
                .FirstOrDefault(_r => _r.RecordRequirement.TenantSurpathServiceFk.UserId != _cohortUser.UserId);
            if (userRequirement != null) return userRequirement;

            // First check: Cohort-specific requirement (highest priority)
            var cohortRequirement = _surpathOnlyRequirements
                .FirstOrDefault(_r => _r.RecordRequirement.CohortId != null &&
                                     _cohortUser.CohortId == _r.RecordRequirement.CohortId);
            if (cohortRequirement != null) return cohortRequirement;

            // Second check: Department-specific requirement
            var departmentRequirement = _surpathOnlyRequirements
                .FirstOrDefault(_r => _r.RecordRequirement.TenantDepartmentId != null &&
                                     _cohortUser.TenantId == _r.RecordRequirement.TenantId &&
                                     _cohortUser.CohortFk.TenantDepartmentId == _r.RecordRequirement.TenantDepartmentId);
            if (departmentRequirement != null) return departmentRequirement;

            // Last check: Tenant-wide requirement (lowest priority)
            var finalResult = _surpathOnlyRequirements
                .FirstOrDefault(_r => _cohortUser.TenantId == _r.RecordRequirement.TenantId && _r.RecordRequirement.TenantDepartmentId == null &&
                                     _r.RecordRequirement.CohortId == null);
            return finalResult;
        }
        public static async Task<IEnumerable<TenantRequirement>> GetUserSurpathRequirements(
            IEnumerable<TenantRequirement> requirements,
            CohortUser _cohortUser,
            IEnumerable<Guid> userRecordRequirementIds = null
        )
        {
            // Get all enabled tenant services

            // Convert services to requirements
            var _requirements = requirements
                .Where(s =>
                    s.RecordRequirement.TenantSurpathServiceFk.IsPricingOverrideEnabled == true &&
                    s.RecordRequirement.TenantSurpathServiceFk.TenantId == _cohortUser.TenantId &&
                    s.RecordRequirement.TenantSurpathServiceFk.IsDeleted == false &&
                    s.RecordRequirement.IsSurpathOnly == true &&
                    s.RecordRequirement.IsDeleted == false &&
                    s.RecordRequirement.TenantDepartmentId == null
                )
                .Select(s => new TenantRequirement
                {
                    RecordRequirement = new RecordRequirement
                    {

                    }
                })
                .ToList();

            // Get drug and background requirements using the existing mechanism
            var drugRequirement = SurpathOnlyRequirements.GetSurpathRequirementFromRequirements(
                requirements,
                _cohortUser,
                AppFeatures.SurpathFeatureDrugTest,
                userRecordRequirementIds);

            var backgroundRequirement = SurpathOnlyRequirements.GetSurpathRequirementFromRequirements(
                requirements,
                _cohortUser,
                AppFeatures.SurpathFeatureBackgroundCheck,
                userRecordRequirementIds);

            var result = new List<TenantRequirement>();
            result.Add(drugRequirement);
            result.Add(backgroundRequirement);
            return result;
        }
    }
}
