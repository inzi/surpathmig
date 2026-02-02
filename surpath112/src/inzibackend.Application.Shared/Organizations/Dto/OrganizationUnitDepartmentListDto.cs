using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace inzibackend.Organizations.Dto
{
    public class OrganizationUnitDepartmentListDto : EntityDto
    {
        public Guid Id { get; set; }
        public string Description { get; set; }

        public string Name { get; set; }

        public DateTime AddedTime { get; set; }
    }
}
