using Abp.Application.Services.Dto;
using System;

namespace inzibackend.Surpath.Dtos
{
    public class GetAllPidTypesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public string DescriptionFilter { get; set; }

        public int? MaskPidFilter { get; set; }

        public DateTime? MaxCreatedOnFilter { get; set; }
        public DateTime? MinCreatedOnFilter { get; set; }

        public DateTime? MaxModifiedOnFilter { get; set; }
        public DateTime? MinModifiedOnFilter { get; set; }

        public long? MaxCreatedByFilter { get; set; }
        public long? MinCreatedByFilter { get; set; }

        public long? MaxLastModifiedByFilter { get; set; }
        public long? MinLastModifiedByFilter { get; set; }

        public int? IsActiveFilter { get; set; }

        public string PidInputMaskFilter { get; set; }

        public int? RequiredFilter { get; set; }

    }
}