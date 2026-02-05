using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace inzibackend.Surpath.Dtos
{
    public class CreateOrEditPidTypeDto : EntityDto<Guid?>
    {

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public bool MaskPid { get; set; }

        public string PidRegex { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }

        public long CreatedBy { get; set; }

        public long LastModifiedBy { get; set; }

        public bool IsActive { get; set; }

        public string PidInputMask { get; set; }

        public bool Required { get; set; }

    }
}