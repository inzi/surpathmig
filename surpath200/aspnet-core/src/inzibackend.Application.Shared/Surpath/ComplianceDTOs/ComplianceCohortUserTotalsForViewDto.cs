using System;
using System.Collections.Generic;

namespace inzibackend.Surpath.Dtos
{

    public class ComplianceCohortUserTotalsForViewDto
    {
        public Guid Id { get; set; }
        public string HtmlColor { get; set; }
        public string StatusName { get; set; }
        public string CSSCLass { get; set; }
        public int Count { get; set; }
        public Guid CohortUserId { get; set; }
        public string UserName { get; set; }
        public Guid TenantDepartmentId { get; set; }

    }
}