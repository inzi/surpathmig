using inzibackend.Surpath;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace inzibackend.Surpath.Dtos
{
    public class CreateOrEditRecordStateDto : EntityDto<Guid?>
    {

        public EnumRecordState State { get; set; }

        public string Notes { get; set; }

        public Guid? RecordId { get; set; }

        public Guid? RecordCategoryId { get; set; }

        public long? UserId { get; set; }

        public Guid RecordStatusId { get; set; }

        public RecordDto RecordDto { get; set; } = new RecordDto();
        public RecordCategoryDto RecordCategoryDto { get; set; }= new RecordCategoryDto();
        //public RecordStateDto RecordState { get; set; } = new RecordStateDto();
    }
}