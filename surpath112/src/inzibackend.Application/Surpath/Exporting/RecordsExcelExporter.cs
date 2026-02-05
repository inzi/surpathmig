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
    public class RecordsExcelExporter : NpoiExcelExporterBase, IRecordsExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public RecordsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetRecordForViewDto> records)
        {
            // Cache these values once to avoid repeated calls to GetUserId() for every row
            var tenantId = _abpSession.TenantId;
            var userId = _abpSession.GetUserId();

            return CreateExcelPackage(
                    "Records.xlsx",
                    excelPackage =>
                    {
                        var sheet = excelPackage.CreateSheet(L("Records"));

                        AddHeader(
                            sheet,
                        L("filedata"),
                        L("filename"),
                        L("physicalfilepath"),
                        L("metadata"),
                        L("BinaryObjId"),
                        L("DateUploaded"),
                        L("DateLastUpdated"),
                        L("InstructionsConfirmed"),
                        L("EffectiveDate"),
                        L("ExpirationDate"),
                        (L("TenantDocumentCategory")) + L("Name")
                            );

                        AddObjects(
                            sheet, records,
                        _ => _.Record.filedataFileName,
                        _ => _.Record.filename,
                        _ => _.Record.physicalfilepath,
                        _ => _.Record.metadata,
                        _ => _.Record.BinaryObjId,
                        _ => _.Record.DateUploaded.HasValue ? _timeZoneConverter.Convert(_.Record.DateUploaded.Value, tenantId, userId) : (DateTime?)null,
                        _ => _.Record.DateLastUpdated.HasValue ? _timeZoneConverter.Convert(_.Record.DateLastUpdated.Value, tenantId, userId) : (DateTime?)null,
                        _ => _.Record.InstructionsConfirmed,
                        _ => _.Record.EffectiveDate.HasValue ? _timeZoneConverter.Convert(_.Record.EffectiveDate.Value, tenantId, userId) : (DateTime?)null,
                        _ => _.Record.ExpirationDate.HasValue ? _timeZoneConverter.Convert(_.Record.ExpirationDate.Value, tenantId, userId) : (DateTime?)null,
                        _ => _.TenantDocumentCategoryName
                            );

                        for (var i = 1; i <= records.Count; i++)
                        {
                            SetCellDataFormat(sheet.GetRow(i).Cells[6 - 1], "yyyy-mm-dd");
                        }
                        sheet.AutoSizeColumn(6 - 1); for (var i = 1; i <= records.Count; i++)
                        {
                            SetCellDataFormat(sheet.GetRow(i).Cells[7 - 1], "yyyy-mm-dd");
                        }
                        sheet.AutoSizeColumn(7 - 1); for (var i = 1; i <= records.Count; i++)
                        {
                            SetCellDataFormat(sheet.GetRow(i).Cells[9 - 1], "yyyy-mm-dd");
                        }
                        sheet.AutoSizeColumn(9 - 1); for (var i = 1; i <= records.Count; i++)
                        {
                            SetCellDataFormat(sheet.GetRow(i).Cells[10 - 1], "yyyy-mm-dd");
                        }
                        sheet.AutoSizeColumn(10 - 1);
                    });
        }
    }
}