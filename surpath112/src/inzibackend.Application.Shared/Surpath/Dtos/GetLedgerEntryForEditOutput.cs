using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace inzibackend.Surpath.Dtos
{
    public class GetLedgerEntryForEditOutput
    {
        public CreateOrEditLedgerEntryDto LedgerEntry { get; set; }

        public string UserName { get; set; }

        public string TenantDocumentName { get; set; }

        public string CohortName { get; set; }

    }
}