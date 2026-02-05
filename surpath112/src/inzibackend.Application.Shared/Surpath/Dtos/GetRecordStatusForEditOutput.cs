using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace inzibackend.Surpath.Dtos
{
    public class GetRecordStatusForEditOutput
    {
        public CreateOrEditRecordStatusDto RecordStatus { get; set; }

        public string TenantDepartmentName { get; set; }

    }
}