using inzibackend.Surpath.Dtos;
using inzibackend.Dto;
using inzibackend.Storage;
using System.Collections.Generic;
using System.Linq;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using inzibackend.DataExporting.Excel.NPOI;
using NPOI.SS.UserModel;
using inzibackend.MultiTenancy.Payments;

namespace inzibackend.Surpath.Exporting
{
    public class UserPurchasesExcelExporter : NpoiExcelExporterBase, IUserPurchasesExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public UserPurchasesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
            base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetUserPurchaseForViewDto> userPurchases)
        {
            // Cache these values once to avoid repeated calls to GetUserId() for every row
            var tenantId = _abpSession.TenantId;
            var userId = _abpSession.GetUserId();

            return CreateExcelPackage(
                "UserPurchases.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("UserPurchases");

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Description"),
                        L("Status"),
                        L("OriginalPrice"),
                        L("AdjustedPrice"),
                        L("DiscountAmount"),
                        L("AmountPaid"),
                        L("BalanceDue"),
                        L("PaymentPeriodType"),
                        L("PurchaseDate"),
                        L("ExpirationDate"),
                        L("IsRecurring"),
                        L("User"),
                        L("SurpathService"),
                        L("TenantSurpathService"),
                        L("Cohort")
                    );

                    AddObjects(
                        sheet, userPurchases,
                        _ => _.UserPurchase.Name,
                        _ => _.UserPurchase.Description,
                        _ => _.UserPurchase.Status.ToString(),
                        _ => _.UserPurchase.OriginalPrice,
                        _ => _.UserPurchase.AdjustedPrice,
                        _ => _.UserPurchase.DiscountAmount,
                        _ => _.UserPurchase.AmountPaid,
                        _ => _.UserPurchase.BalanceDue,
                        _ => GetPaymentPeriodType(_.UserPurchase.PaymentPeriodType),
                        _ => _timeZoneConverter.Convert(_.UserPurchase.PurchaseDate, tenantId, userId),
                        _ => _.UserPurchase.ExpirationDate.HasValue ? _timeZoneConverter.Convert(_.UserPurchase.ExpirationDate.Value, tenantId, userId) : "",
                        _ => _.UserPurchase.IsRecurring ? L("Yes") : L("No"),
                        _ => _.UserName,
                        _ => _.SurpathServiceName,
                        _ => _.TenantSurpathServiceName,
                        _ => _.CohortName
                    );

                    for (var i = 0; i < 17; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });
        }

        private string GetPaymentPeriodType(PaymentPeriodType paymentPeriodType)
        {
            switch (paymentPeriodType)
            {
                case PaymentPeriodType.Daily:
                    return L("Daily");
                case PaymentPeriodType.Weekly:
                    return L("Weekly");
                case PaymentPeriodType.Monthly:
                    return L("Monthly");
                case PaymentPeriodType.Annual:
                    return L("Annual");
                default:
                    return "";
            }
        }
    }
}