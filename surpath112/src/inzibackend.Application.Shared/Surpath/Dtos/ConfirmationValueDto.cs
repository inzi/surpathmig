using inzibackend.Surpath;

using System;
using Abp.Application.Services.Dto;

namespace inzibackend.Surpath.Dtos
{
    public class ConfirmationValueDto : EntityDto<Guid>
    {
        public double ScreenValue { get; set; }

        public double ConfirmValue { get; set; }

        public EnumUnitOfMeasurement UnitOfMeasurement { get; set; }

        public Guid DrugId { get; set; }

        public Guid TestCategoryId { get; set; }

    }
}