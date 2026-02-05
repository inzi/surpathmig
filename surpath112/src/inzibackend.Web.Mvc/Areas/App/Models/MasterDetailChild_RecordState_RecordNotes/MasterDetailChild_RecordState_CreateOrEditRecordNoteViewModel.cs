using inzibackend.Surpath.Dtos;
using System.Collections.Generic;

using Abp.Extensions;

namespace inzibackend.Web.Areas.App.Models.RecordNotes
{
    public class MasterDetailChild_RecordState_CreateOrEditRecordNoteModalViewModel
    {
        public CreateOrEditRecordNoteDto RecordNote { get; set; }

        public string UserName { get; set; }

        public string UserName2 { get; set; }

        public List<RecordNoteUserLookupTableDto> RecordNoteUserList { get; set; }

        public bool IsEditMode => RecordNote.Id.HasValue;
    }
}