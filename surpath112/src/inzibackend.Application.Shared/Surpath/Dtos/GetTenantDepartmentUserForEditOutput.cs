using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace inzibackend.Surpath.Dtos
{
    public class GetTenantDepartmentUserForEditOutput
    {
        public CreateOrEditTenantDepartmentUserDto TenantDepartmentUser { get; set; }

        public string UserName { get; set; }

        public string TenantDepartmentName { get; set; }

    }
}