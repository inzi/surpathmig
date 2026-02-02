using System;
using System.Collections.Generic;
using System.Text;

namespace inzibackend.Surpath.Compliance
{
    public class TenantRequirement
    {
        public RecordRequirement RecordRequirement { get; set; }
        public RecordCategory RecordCategory { get; set; }
        public RecordCategoryRule RecordCategoryRule { get; set; }
        public TenantSurpathService TenantSurpathService { get; set; } // Added for hierarchical service checking
    }
}
