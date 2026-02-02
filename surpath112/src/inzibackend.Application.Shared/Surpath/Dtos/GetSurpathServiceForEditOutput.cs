using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace inzibackend.Surpath.Dtos
{
    public class GetSurpathServiceForEditOutput
    {
        public CreateOrEditSurpathServiceDto SurpathService { get; set; }

        public string TenantDepartmentName { get; set; }

        public string CohortName { get; set; }

        public string UserName { get; set; }

        public string RecordCategoryRuleName { get; set; }

    }
}