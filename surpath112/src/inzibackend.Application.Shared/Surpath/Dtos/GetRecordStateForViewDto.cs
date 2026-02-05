using System;

namespace inzibackend.Surpath.Dtos
{
    public class GetRecordStateForViewDto
    {
        public RecordStateDto RecordState { get; set; }
        public RecordStatusDto RecordStatus { get; set; }
        
        public string Recordfilename { get; set; }

        public string RecordCategoryName { get; set; }

        public string UserName { get; set; }

        public string RecordStatusStatusName { get; set; }

        public string FullName { get; set; }
        public long UserId { get; set; }

        public RecordDto RecordDto { get; set; }

        public Guid? CohortUserId { get; set; }
        //public string RecordStatusHighlight { get; set; }
        //public string RecordStatusCSSClass { get; set; }

    }
}