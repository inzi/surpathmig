using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using inzibackend.DataExporting.Excel.NPOI;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;
using inzibackend.Storage;

namespace inzibackend.Surpath.Exporting
{
    public class DrugTestCategoriesExcelExporter : NpoiExcelExporterBase, IDrugTestCategoriesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public DrugTestCategoriesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetDrugTestCategoryForViewDto> drugTestCategories)
        {
            return CreateExcelPackage(
                "DrugTestCategories.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("DrugTestCategories"));

                    AddHeader(
                        sheet,
                        (L("Drug")) + L("Name"),
                        (L("Panel")) + L("Name"),
                        (L("TestCategory")) + L("Name")
                        );

                    AddObjects(
                        sheet, drugTestCategories,
                        _ => _.DrugName,
                        _ => _.PanelName,
                        _ => _.TestCategoryName
                        );

                });
        }
    }
}