using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using inzibackend.DataExporting.Excel.NPOI;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;
using inzibackend.Storage;

namespace inzibackend.Surpath.Exporting
{
    public class TestCategoriesExcelExporter : NpoiExcelExporterBase, ITestCategoriesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public TestCategoriesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetTestCategoryForViewDto> testCategories)
        {
            return CreateExcelPackage(
                "TestCategories.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("TestCategories"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("InternalName")
                        );

                    AddObjects(
                        sheet, testCategories,
                        _ => _.TestCategory.Name,
                        _ => _.TestCategory.InternalName
                        );

                });
        }
    }
}