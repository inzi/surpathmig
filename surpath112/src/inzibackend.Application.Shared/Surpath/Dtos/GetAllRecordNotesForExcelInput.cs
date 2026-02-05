using Abp.Application.Services.Dto;
using System;

namespace inzibackend.Surpath.Dtos
{
    public class GetAllRecordNotesForExcelInput
    {
        public string Filter { get; set; }

        public string NoteFilter { get; set; }

        public DateTime? MaxCreatedFilter { get; set; }
        public DateTime? MinCreatedFilter { get; set; }

        public int? AuthorizedOnlyFilter { get; set; }

        public int? HostOnlyFilter { get; set; }

        public int? SendNotificationFilter { get; set; }

        public string RecordStateNotesFilter { get; set; }

        public string UserNameFilter { get; set; }

        public string UserName2Filter { get; set; }

        public Guid? RecordStateIdFilter { get; set; }
    }
}