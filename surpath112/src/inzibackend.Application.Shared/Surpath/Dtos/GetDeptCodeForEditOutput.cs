using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace inzibackend.Surpath.Dtos
{
    public class GetDeptCodeForEditOutput
    {
        public CreateOrEditDeptCodeDto DeptCode { get; set; }

        public string CodeTypeName { get; set; }

        public string TenantDepartmentName { get; set; }

    }
}