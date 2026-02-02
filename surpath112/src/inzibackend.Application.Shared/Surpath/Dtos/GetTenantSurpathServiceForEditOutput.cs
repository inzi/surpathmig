using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace inzibackend.Surpath.Dtos
{
    public class GetTenantSurpathServiceForEditOutput
    {
        public CreateOrEditTenantSurpathServiceDto TenantSurpathService { get; set; }

        public string SurpathServiceName { get; set; }

        public string TenantDepartmentName { get; set; }

        public string CohortName { get; set; }

        public string UserName { get; set; }

        public string RecordCategoryRuleName { get; set; }

    }
}