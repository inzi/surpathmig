using Abp.Application.Services.Dto;

namespace inzibackend.Surpath.Dtos
{
    public class RecordStateRecordStatusLookupTableDto
    {
        public string Id { get; set; }

        public string DisplayName { get; set; }

        public bool IsDefault { get; set; }
        public bool RequireNoteOnSet { get; set; } = false;
        public bool IsSurpathServiceStatus { get; set; }

    }
}