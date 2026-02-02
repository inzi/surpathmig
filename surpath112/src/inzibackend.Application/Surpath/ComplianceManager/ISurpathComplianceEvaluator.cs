using inzibackend.Surpath.Compliance;
using inzibackend.Surpath.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace inzibackend.Surpath.ComplianceManager
{
    public interface ISurpathComplianceEvaluator
    {
        Task<ComplianceValues> GetComplianceValuesForCohortUser(Guid cohortuserid);

        Task<ComplianceValues> GetComplianceValuesForUser(int _tenantId, long _userId);

        Task<List<GetRecordStateCompliancetForViewDto>> GetComplianceStatesForUser(long _userId);

        Task<ComplianceInfo> GetComplianceInfo(int _tenantId, long _userId = 0);

        Task<ComplianceValues> GetDetailedComplianceValuesForUser(long _userId);

        Task<Dictionary<long, ComplianceValues>> GetBulkComplianceValuesForUsers(List<long> userIds, int tenantId);

        Task<List<HierarchicalRequirementCategoryDto>> GetHierarchicalRequirementCategories(
            Guid? departmentId = null,
            Guid? cohortId = null,
            long? userId = null,
            bool includeInherited = true);

        /// <summary>
        /// Recalculates compliance for a specific user, typically after a cohort migration or transfer.
        /// This method ensures that the user's compliance state is accurately reflected after changes
        /// to their cohort or department assignment.
        /// </summary>
        /// <param name="userId">The ID of the user to recalculate compliance for</param>
        /// <returns>The updated compliance values after recalculation</returns>
        Task<ComplianceValues> RecalculateUserCompliance(long userId);
    }
}