using System;
using Abp.Application.Services.Dto;

namespace inzibackend.Surpath.Dtos
{
    public class DrugDto : EntityDto<Guid>
    {
        public string Name { get; set; }

        public string Code { get; set; }

    }
}