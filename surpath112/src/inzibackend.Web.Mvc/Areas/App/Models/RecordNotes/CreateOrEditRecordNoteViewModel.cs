using inzibackend.Surpath.Dtos;
using System.Collections.Generic;

using Abp.Extensions;

namespace inzibackend.Web.Areas.App.Models.RecordNotes
{
    public class CreateOrEditRecordNoteModalViewModel
    {
        public CreateOrEditRecordNoteDto RecordNote { get; set; }

        public string RecordStateNotes { get; set; }

        public string UserName { get; set; }

        public string UserName2 { get; set; }

        public List<RecordNoteUserLookupTableDto> RecordNoteUserList { get; set; }

        public bool IsEditMode => RecordNote.Id.HasValue;
    }
}