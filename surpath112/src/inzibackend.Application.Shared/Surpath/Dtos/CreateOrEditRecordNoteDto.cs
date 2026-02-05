using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace inzibackend.Surpath.Dtos
{
    public class CreateOrEditRecordNoteDto : EntityDto<Guid?>
    {

        [Required]
        public string Note { get; set; }

        public DateTime Created { get; set; }

        public bool AuthorizedOnly { get; set; }

        public bool HostOnly { get; set; }

        public bool SendNotification { get; set; }

        public Guid? RecordStateId { get; set; }

        public long? UserId { get; set; }

        public long? NotifyUserId { get; set; }

    }
}