using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using inzibackend.DataExporting.Excel.NPOI;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;
using inzibackend.Storage;

namespace inzibackend.Surpath.Exporting
{
    public class DrugsExcelExporter : NpoiExcelExporterBase, IDrugsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public DrugsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetDrugForViewDto> drugs)
        {
            return CreateExcelPackage(
                "Drugs.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("Drugs"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Code")
                        );

                    AddObjects(
                        sheet, drugs,
                        _ => _.Drug.Name,
                        _ => _.Drug.Code
                        );

                });
        }
    }
}