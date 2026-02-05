using System;
using System.Collections.Generic;

namespace inzibackend.Surpath.Dtos
{
    public class ComplianceTotalsForViewDto
    {
        public Guid Id { get; set; }
        public string HtmlColor { get; set; }
        public string StatusName { get; set; }
        public string CSSCLass { get; set; }
        public int Count { get; set; }

    }

}