using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace inzibackend.Surpath.Dtos
{
    public class GetDepartmentUserForEditOutput
    {
        public CreateOrEditDepartmentUserDto DepartmentUser { get; set; }

        public string UserName { get; set; }

        public string TenantDepartmentName { get; set; }

    }
}