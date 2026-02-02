using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using inzibackend.DataExporting.Excel.NPOI;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;
using inzibackend.Storage;

namespace inzibackend.Surpath.Exporting
{
    public class RecordCategoriesExcelExporter : NpoiExcelExporterBase, IRecordCategoriesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public RecordCategoriesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetRecordCategoryForViewDto> recordCategories)
        {
            return CreateExcelPackage(
                "RecordCategories.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("RecordCategories"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Instructions"),
                        (L("RecordRequirement")) + L("Name"),
                        (L("RecordCategoryRule")) + L("Name")
                        );

                    AddObjects(
                        sheet, recordCategories,
                        _ => _.RecordCategory.Name,
                        _ => _.RecordCategory.Instructions,
                        _ => _.RecordRequirementName,
                        _ => _.RecordCategoryRuleName
                        );

                });
        }
    }
}