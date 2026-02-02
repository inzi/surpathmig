using System;
using Abp.Application.Services.Dto;

namespace inzibackend.Surpath.Dtos
{
    public class LedgerEntryDetailDto : EntityDto<Guid>
    {
        public string Note { get; set; }

        public decimal Amount { get; set; }

        public decimal Discount { get; set; }

        public decimal DiscountAmount { get; set; }

        public string MetaData { get; set; }

        public decimal AmountPaid { get; set; }

        public DateTime? DatePaidOn { get; set; }

        public Guid? LedgerEntryId { get; set; }

        public Guid SurpathServiceId { get; set; }

        public Guid? TenantSurpathServiceId { get; set; }

    }
}
