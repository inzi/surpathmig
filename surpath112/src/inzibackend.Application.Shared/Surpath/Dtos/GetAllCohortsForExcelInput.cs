using Abp.Application.Services.Dto;
using System;

namespace inzibackend.Surpath.Dtos
{
    public class GetAllCohortsForExcelInput
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public string DescriptionFilter { get; set; }

        public int? DefaultCohortFilter { get; set; }

        public string TenantDepartmentNameFilter { get; set; }

    }
}