using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using inzibackend.DataExporting.Excel.NPOI;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;
using inzibackend.Storage;

namespace inzibackend.Surpath.Exporting
{
    public class DrugPanelsExcelExporter : NpoiExcelExporterBase, IDrugPanelsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public DrugPanelsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetDrugPanelForViewDto> drugPanels)
        {
            return CreateExcelPackage(
                "DrugPanels.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("DrugPanels"));

                    AddHeader(
                        sheet,
                        (L("Drug")) + L("Name"),
                        (L("Panel")) + L("Name")
                        );

                    AddObjects(
                        sheet, drugPanels,
                        _ => _.DrugName,
                        _ => _.PanelName
                        );

                });
        }
    }
}