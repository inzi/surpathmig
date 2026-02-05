using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace inzibackend.Surpath.Dtos
{
    public class CreateOrEditUserPidDto : EntityDto<Guid?>
    {

        [Required]
        public string Pid { get; set; }

        public bool Validated { get; set; }

        public Guid PidTypeId { get; set; }

        public long? UserId { get; set; }

    }
}