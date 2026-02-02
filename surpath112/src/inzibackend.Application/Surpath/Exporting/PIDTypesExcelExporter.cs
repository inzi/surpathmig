using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using inzibackend.DataExporting.Excel.NPOI;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;
using inzibackend.Storage;

namespace inzibackend.Surpath.Exporting
{
    public class PidTypesExcelExporter : NpoiExcelExporterBase, IPidTypesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public PidTypesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetPidTypeForViewDto> pidTypes)
        {
            // Cache these values once to avoid repeated calls to GetUserId() for every row
            var tenantId = _abpSession.TenantId;
            var userId = _abpSession.GetUserId();

            return CreateExcelPackage(
                "PidTypes.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("PidTypes"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Description"),
                        L("MaskPid"),
                        L("CreatedOn"),
                        L("ModifiedOn"),
                        L("CreatedBy"),
                        L("LastModifiedBy"),
                        L("IsActive"),
                        L("PidInputMask"),
                        L("Required")
                        );

                    AddObjects(
                        sheet, pidTypes,
                        _ => _.PidType.Name,
                        _ => _.PidType.Description,
                        _ => _.PidType.MaskPid,
                        _ => _timeZoneConverter.Convert(_.PidType.CreatedOn, tenantId, userId),
                        _ => _timeZoneConverter.Convert(_.PidType.ModifiedOn, tenantId, userId),
                        _ => _.PidType.CreatedBy,
                        _ => _.PidType.LastModifiedBy,
                        _ => _.PidType.IsActive,
                        _ => _.PidType.PidInputMask,
                        _ => _.PidType.Required
                        );

                    for (var i = 1; i <= pidTypes.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[4], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(4); for (var i = 1; i <= pidTypes.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[5], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(5);
                });
        }
    }
}