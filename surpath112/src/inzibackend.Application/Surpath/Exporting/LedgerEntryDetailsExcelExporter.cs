using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using inzibackend.DataExporting.Excel.NPOI;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;
using inzibackend.Storage;
using System;

namespace inzibackend.Surpath.Exporting
{
    public class LedgerEntryDetailsExcelExporter : NpoiExcelExporterBase, ILedgerEntryDetailsExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public LedgerEntryDetailsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetLedgerEntryDetailForViewDto> ledgerEntryDetails)
        {
            // Cache these values once to avoid repeated calls to GetUserId() for every row
            var tenantId = _abpSession.TenantId;
            var userId = _abpSession.GetUserId();

            return CreateExcelPackage(
                    "LedgerEntryDetails.xlsx",
                    excelPackage =>
                    {
                        var sheet = excelPackage.CreateSheet(L("LedgerEntryDetails"));

                        AddHeader(
                            sheet,
                        L("Note"),
                        L("Amount"),
                        L("Discount"),
                        L("DiscountAmount"),
                        L("MetaData"),
                        L("AmountPaid"),
                        L("DatePaidOn"),
                        (L("LedgerEntry")) + L("TransactionId"),
                        (L("SurpathService")) + L("Name"),
                        (L("TenantSurpathService")) + L("Name")
                            );

                        AddObjects(
                            sheet, ledgerEntryDetails,
                        _ => _.LedgerEntryDetail.Note,
                        _ => _.LedgerEntryDetail.Amount,
                        _ => _.LedgerEntryDetail.Discount,
                        _ => _.LedgerEntryDetail.DiscountAmount,
                        _ => _.LedgerEntryDetail.MetaData,
                        _ => _.LedgerEntryDetail.AmountPaid,
                        _ => _.LedgerEntryDetail.DatePaidOn.HasValue ? _timeZoneConverter.Convert(_.LedgerEntryDetail.DatePaidOn.Value, tenantId, userId) : (DateTime?)null,
                        _ => _.LedgerEntryTransactionId,
                        _ => _.SurpathServiceName,
                        _ => _.TenantSurpathServiceName
                            );

                        for (var i = 1; i <= ledgerEntryDetails.Count; i++)
                        {
                            SetCellDataFormat(sheet.GetRow(i).Cells[7 - 1], "yyyy-mm-dd");
                        }
                        sheet.AutoSizeColumn(7 - 1);
                    });
        }
    }
}