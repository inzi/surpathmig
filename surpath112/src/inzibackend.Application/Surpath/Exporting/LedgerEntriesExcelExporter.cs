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
    public class LedgerEntriesExcelExporter : NpoiExcelExporterBase, ILedgerEntriesExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public LedgerEntriesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetLedgerEntryForViewDto> ledgerEntries)
        {
            // Cache these values once to avoid repeated calls to GetUserId() for every row
            var tenantId = _abpSession.TenantId;
            var userId = _abpSession.GetUserId();

            return CreateExcelPackage(
                    "LedgerEntries.xlsx",
                    excelPackage =>
                    {
                        var sheet = excelPackage.CreateSheet(L("LedgerEntries"));

                        AddHeader(
                            sheet,
                        L("Name"),
                        L("ServiceType"),
                        L("Amount"),
                        L("DiscountAmount"),
                        L("TotalPrice"),
                        L("PaymentPeriodType"),
                        L("ExpirationDate"),
                        L("TransactionName"),
                        L("TransactionKey"),
                        L("TransactionId"),
                        L("Settled"),
                        L("AmountDue"),
                        L("PaymentToken"),
                        L("AuthNetCustomerProfileId"),
                        L("AuthNetCustomerPaymentProfileId"),
                        L("AuthNetCustomerAddressId"),
                        L("AccountNumber"),
                        L("Note"),
                        L("MetaData"),
                        L("AuthCode"),
                        L("ReferenceTransactionId"),
                        L("TransactionHash"),
                        L("AccountType"),
                        L("TransactionCode"),
                        L("TransactionMessage"),
                        L("AuthNetTransHashSha2"),
                        L("AuthNetNetworkTransId"),
                        L("PaidAmount"),
                        L("PaidInCash"),
                        L("AvailableUserBalance"),
                        L("CardNameOnCard"),
                        L("CardZipCode"),
                        L("BalanceForward"),
                        (L("User")) + L("Name"),
                        (L("TenantDocument")) + L("Name"),
                        (L("Cohort")) + L("Name")
                            );

                        AddObjects(
                            sheet, ledgerEntries,
                        _ => _.LedgerEntry.Name,
                        _ => _.LedgerEntry.ServiceType,
                        _ => _.LedgerEntry.Amount,
                        _ => _.LedgerEntry.DiscountAmount,
                        _ => _.LedgerEntry.TotalPrice,
                        _ => _.LedgerEntry.PaymentPeriodType,
                        _ => _.LedgerEntry.ExpirationDate.HasValue ? _timeZoneConverter.Convert(_.LedgerEntry.ExpirationDate.Value, tenantId, userId) : (DateTime?)null,
                        _ => _.LedgerEntry.TransactionName,
                        _ => _.LedgerEntry.TransactionKey,
                        _ => _.LedgerEntry.TransactionId,
                        _ => _.LedgerEntry.Settled,
                        _ => _.LedgerEntry.AmountDue,
                        _ => _.LedgerEntry.PaymentToken,
                        _ => _.LedgerEntry.AuthNetCustomerProfileId,
                        _ => _.LedgerEntry.AuthNetCustomerPaymentProfileId,
                        _ => _.LedgerEntry.AuthNetCustomerAddressId,
                        _ => _.LedgerEntry.AccountNumber,
                        _ => _.LedgerEntry.Note,
                        _ => _.LedgerEntry.MetaData,
                        _ => _.LedgerEntry.AuthCode,
                        _ => _.LedgerEntry.ReferenceTransactionId,
                        _ => _.LedgerEntry.TransactionHash,
                        _ => _.LedgerEntry.AccountType,
                        _ => _.LedgerEntry.TransactionCode,
                        _ => _.LedgerEntry.TransactionMessage,
                        _ => _.LedgerEntry.AuthNetTransHashSha2,
                        _ => _.LedgerEntry.AuthNetNetworkTransId,
                        _ => _.LedgerEntry.PaidAmount,
                        _ => _.LedgerEntry.PaidInCash,
                        _ => _.LedgerEntry.AvailableUserBalance,
                        _ => _.LedgerEntry.CardNameOnCard,
                        _ => _.LedgerEntry.CardZipCode,
                        _ => _.LedgerEntry.BalanceForward,
                        _ => _.UserName,
                        _ => _.TenantDocumentName,
                        _ => _.CohortName
                            );

                        for (var i = 1; i <= ledgerEntries.Count; i++)
                        {
                            SetCellDataFormat(sheet.GetRow(i).Cells[7 - 1], "yyyy-mm-dd");
                        }
                        sheet.AutoSizeColumn(7 - 1);
                    });
        }
    }
}