using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using inzibackend.DataExporting.Excel.NPOI;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;
using inzibackend.Storage;

namespace inzibackend.Surpath.Exporting
{
    public class RecordNotesExcelExporter : NpoiExcelExporterBase, IRecordNotesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public RecordNotesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetRecordNoteForViewDto> recordNotes)
        {
            // Cache these values once to avoid repeated calls to GetUserId() for every row
            var tenantId = _abpSession.TenantId;
            var userId = _abpSession.GetUserId();

            return CreateExcelPackage(
                "RecordNotes.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("RecordNotes"));

                    AddHeader(
                        sheet,
                        L("Note"),
                        L("Created"),
                        L("AuthorizedOnly"),
                        L("HostOnly"),
                        L("SendNotification"),
                        (L("RecordState")) + L("Notes"),
                        (L("User")) + L("Name"),
                        (L("User")) + L("Name")
                        );

                    AddObjects(
                        sheet, recordNotes,
                        _ => _.RecordNote.Note,
                        _ => _timeZoneConverter.Convert(_.RecordNote.Created, tenantId, userId),
                        _ => _.RecordNote.AuthorizedOnly,
                        _ => _.RecordNote.HostOnly,
                        _ => _.RecordNote.SendNotification,
                        _ => _.RecordStateNotes,
                        _ => _.UserName,
                        _ => _.UserName2
                        );

                    for (var i = 1; i <= recordNotes.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[2], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(2);
                });
        }
    }
}