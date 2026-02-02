using Abp.Application.Services.Dto;
using System;

namespace inzibackend.Surpath.Dtos
{
    public class GetAllDepartmentUsersForExcelInput
    {
        public string Filter { get; set; }

        public string UserNameFilter { get; set; }

        public string TenantDepartmentNameFilter { get; set; }

    }
}