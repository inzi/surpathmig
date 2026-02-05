using inzibackend;

using System;
using Abp.Application.Services.Dto;
using static inzibackend.AppConsts;

namespace inzibackend.Surpath.Dtos
{
    public class HospitalDto : EntityDto
    {
        public string Name { get; set; }

        public string PrimaryContact { get; set; }

        public string PrimaryContactPhone { get; set; }

        public string PrimaryContactEmail { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public enumUSStates State { get; set; }

        public string ZipCode { get; set; }

    }
}