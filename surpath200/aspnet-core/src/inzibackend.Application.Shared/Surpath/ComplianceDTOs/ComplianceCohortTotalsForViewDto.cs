using System;
using System.Collections.Generic;

namespace inzibackend.Surpath.Dtos
{

    public class ComplianceCohortTotalsForViewDto
    {
        public Guid Id { get; set; }
        public string HtmlColor { get; set; }
        public string StatusName { get; set; }
        public string CSSCLass { get; set; }
        public int Count { get; set; }
        public Guid CohortId { get; set; }
        public string CohortName { get; set; }
        public Guid TenantDepartmentId { get; set; }

    }
}