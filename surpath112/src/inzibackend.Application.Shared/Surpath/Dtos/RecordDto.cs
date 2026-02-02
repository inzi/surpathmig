using System;
using Abp.Application.Services.Dto;

namespace inzibackend.Surpath.Dtos
{
    public class RecordDto : EntityDto<Guid>
    {

        public Guid? filedata { get; set; }

        public string filedataFileName { get; set; }

        public string filename { get; set; }

        public string physicalfilepath { get; set; }

        public string metadata { get; set; }

        public Guid BinaryObjId { get; set; }

        public DateTime? DateUploaded { get; set; }

        public DateTime? DateLastUpdated { get; set; }

        public bool InstructionsConfirmed { get; set; }

        public DateTime? EffectiveDate { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public Guid? TenantDocumentCategoryId { get; set; }

    }
}