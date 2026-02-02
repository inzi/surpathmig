using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using inzibackend.DataExporting.Excel.NPOI;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;
using inzibackend.Storage;

namespace inzibackend.Surpath.Exporting
{
    public class RecordStatesExcelExporter : NpoiExcelExporterBase, IRecordStatesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public RecordStatesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetRecordStateForViewDto> recordStates)
        {
            return CreateExcelPackage(
                "RecordStates.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("RecordStates"));

                    AddHeader(
                        sheet,
                        L("State"),
                        L("Notes"),
                        (L("Record")) + L("filename"),
                        (L("RecordCategory")) + L("Name"),
                        (L("User")) + L("Name"),
                        (L("RecordStatus")) + L("StatusName")
                        );

                    AddObjects(
                        sheet, recordStates,
                        _ => _.RecordState.State,
                        _ => _.RecordState.Notes,
                        _ => _.Recordfilename,
                        _ => _.RecordCategoryName,
                        _ => _.UserName,
                        _ => _.RecordStatusStatusName
                        );

                });
        }
    }
}