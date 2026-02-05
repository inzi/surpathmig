using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace inzibackend.Surpath.Dtos
{
    public class GetRecordNoteForEditOutput
    {
        public CreateOrEditRecordNoteDto RecordNote { get; set; }

        public string RecordStateNotes { get; set; }

        public string UserName { get; set; }

        public string UserName2 { get; set; }

    }
}