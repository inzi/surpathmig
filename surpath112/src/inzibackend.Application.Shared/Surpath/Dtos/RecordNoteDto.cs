using System;
using Abp.Application.Services.Dto;

namespace inzibackend.Surpath.Dtos
{
    public class RecordNoteDto : EntityDto<Guid>
    {
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