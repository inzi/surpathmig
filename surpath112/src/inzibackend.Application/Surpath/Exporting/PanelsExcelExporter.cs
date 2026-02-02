using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using inzibackend.DataExporting.Excel.NPOI;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;
using inzibackend.Storage;

namespace inzibackend.Surpath.Exporting
{
    public class PanelsExcelExporter : NpoiExcelExporterBase, IPanelsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public PanelsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetPanelForViewDto> panels)
        {
            return CreateExcelPackage(
                "Panels.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("Panels"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Cost"),
                        L("Description"),
                        (L("TestCategory")) + L("Name")
                        );

                    AddObjects(
                        sheet, panels,
                        _ => _.Panel.Name,
                        _ => _.Panel.Cost,
                        _ => _.Panel.Description,
                        _ => _.TestCategoryName
                        );

                });
        }
    }
}