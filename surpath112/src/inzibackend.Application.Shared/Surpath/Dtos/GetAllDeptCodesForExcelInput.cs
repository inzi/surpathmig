using Abp.Application.Services.Dto;
using System;

namespace inzibackend.Surpath.Dtos
{
    public class GetAllDeptCodesForExcelInput
    {
        public string Filter { get; set; }

        public string CodeFilter { get; set; }

        public string CodeTypeNameFilter { get; set; }

        public string TenantDepartmentNameFilter { get; set; }

    }
}