using Abp.Application.Services.Dto;
using System;

namespace inzibackend.Surpath.Dtos
{
    public class GetAllRecordStatesForExcelInput
    {
        public string Filter { get; set; }

        public int? StateFilter { get; set; }

        public string NotesFilter { get; set; }

        public string RecordfilenameFilter { get; set; }

        public string RecordCategoryNameFilter { get; set; }

        public string UserNameFilter { get; set; }

        public string RecordStatusStatusNameFilter { get; set; }

    }
}