using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using inzibackend.DataExporting.Excel.NPOI;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;
using inzibackend.Storage;

namespace inzibackend.Surpath.Exporting
{
    public class RecordCategoryRulesExcelExporter : NpoiExcelExporterBase, IRecordCategoryRulesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public RecordCategoryRulesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetRecordCategoryRuleForViewDto> recordCategoryRules)
        {
            return CreateExcelPackage(
                    "RecordCategoryRules.xlsx",
                    excelPackage =>
                    {

                        var sheet = excelPackage.CreateSheet(L("RecordCategoryRules"));

                        AddHeader(
                            sheet,
                        L("Name"),
                        L("Description"),
                        L("Notify"),
                        L("ExpireInDays"),
                        L("WarnDaysBeforeFirst"),
                        L("Expires"),
                        L("Required"),
                        L("IsSurpathOnly"),
                        L("WarnDaysBeforeSecond"),
                        L("WarnDaysBeforeFinal"),
                        L("MetaData")
                            );

                        AddObjects(
                            sheet, recordCategoryRules,
                        _ => _.RecordCategoryRule.Name,
                        _ => _.RecordCategoryRule.Description,
                        _ => _.RecordCategoryRule.Notify,
                        _ => _.RecordCategoryRule.ExpireInDays,
                        _ => _.RecordCategoryRule.WarnDaysBeforeFirst,
                        _ => _.RecordCategoryRule.Expires,
                        _ => _.RecordCategoryRule.Required,
                        _ => _.RecordCategoryRule.IsSurpathOnly,
                        _ => _.RecordCategoryRule.WarnDaysBeforeSecond,
                        _ => _.RecordCategoryRule.WarnDaysBeforeFinal,
                        _ => _.RecordCategoryRule.MetaData
                            );

                    });

        }
    }
}