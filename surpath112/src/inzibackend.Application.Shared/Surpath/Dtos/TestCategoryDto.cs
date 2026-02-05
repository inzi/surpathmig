using System;
using Abp.Application.Services.Dto;

namespace inzibackend.Surpath.Dtos
{
    public class TestCategoryDto : EntityDto<Guid>
    {
        public string Name { get; set; }

        public string InternalName { get; set; }

    }
}