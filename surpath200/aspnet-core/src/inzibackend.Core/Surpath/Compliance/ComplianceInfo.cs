using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inzibackend.Surpath.Compliance
{
    public class ComplianceInfo
    {
        public long? UserId { get; set; }
        public List<TenantRequirement> NonSurpathOnlyRequirements { get; set; } = new List<TenantRequirement>();
        public List<TenantRequirement> SurpathOnlyRequirements { get; set; } = new List<TenantRequirement>();
        public List<TenantRequirement> RequirementsForUser { get; set; } = new List<TenantRequirement>();
        public List<Guid?> UserDeptList { get; set; } = new List<Guid?>();
        public List<UserMembership> UserMembershipsList { get; set; } = new List<UserMembership>();
        public List<RecordCategoryRule> RecordCategoryRules { get; set; } = new List<RecordCategoryRule>();
        public List<SurpathService> SurpathServices { get; set; } = new List<SurpathService>();
        public List<TenantSurpathService> TenantSurpathServices { get; set; } = new List<TenantSurpathService>();

    }

}
