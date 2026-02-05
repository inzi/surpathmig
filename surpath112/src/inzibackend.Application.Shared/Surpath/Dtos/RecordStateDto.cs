using inzibackend.Surpath;

using System;
using Abp.Application.Services.Dto;

namespace inzibackend.Surpath.Dtos
{
    public class RecordStateDto : EntityDto<Guid>
    {
        public EnumRecordState State { get; set; }

        public string Notes { get; set; }

        public Guid? RecordId { get; set; }

        public Guid? RecordCategoryId { get; set; }

        public long? UserId { get; set; }

        public Guid RecordStatusId { get; set; }

        public RecordDto RecordDto { get; set; }

        public RecordCategoryDto RecordCategoryDto { get; set; } = new RecordCategoryDto();

        public bool IsArchived { get; set; } = false;

        public DateTime? ArchivedTime { get; set; }

        public long? ArchivedByUserId { get; set; }
    }
}