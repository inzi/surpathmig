using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace inzibackend.Surpath.Dtos
{
    public class CreateOrEditCohortUserDto : EntityDto<Guid?>
    {

        public Guid? CohortId { get; set; }

        public long UserId { get; set; }

    }
}