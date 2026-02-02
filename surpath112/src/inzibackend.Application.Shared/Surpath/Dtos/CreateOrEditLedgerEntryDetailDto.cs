using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace inzibackend.Surpath.Dtos
{
    public class CreateOrEditLedgerEntryDetailDto : EntityDto<Guid?>
    {

        public string Note { get; set; }

        [Range(typeof(decimal), LedgerEntryDetailConsts.MinAmountValue, LedgerEntryDetailConsts.MaxAmountValue)]
        public decimal Amount { get; set; }

        [Range(typeof(decimal), LedgerEntryDetailConsts.MinDiscountValue, LedgerEntryDetailConsts.MaxDiscountValue)]
        public decimal Discount { get; set; }

        [Range(typeof(decimal), LedgerEntryDetailConsts.MinDiscountAmountValue, LedgerEntryDetailConsts.MaxDiscountAmountValue)]
        public decimal DiscountAmount { get; set; }

        public string MetaData { get; set; }

        public decimal AmountPaid { get; set; }

        public DateTime? DatePaidOn { get; set; }

        public Guid? LedgerEntryId { get; set; }

        public Guid SurpathServiceId { get; set; }

        public Guid? TenantSurpathServiceId { get; set; }

    }
}
