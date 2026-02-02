using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace inzibackend.Surpath.Dtos
{
    public class GetLedgerEntryDetailForEditOutput
    {
        public CreateOrEditLedgerEntryDetailDto LedgerEntryDetail { get; set; }

        public string LedgerEntryTransactionId { get; set; }

        public string SurpathServiceName { get; set; }

        public string TenantSurpathServiceName { get; set; }

    }
}