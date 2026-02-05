using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using inzibackend.DataExporting.Excel.NPOI;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;
using inzibackend.Storage;

namespace inzibackend.Surpath.Exporting
{
    public class ConfirmationValuesExcelExporter : NpoiExcelExporterBase, IConfirmationValuesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ConfirmationValuesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetConfirmationValueForViewDto> confirmationValues)
        {
            return CreateExcelPackage(
                "ConfirmationValues.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("ConfirmationValues"));

                    AddHeader(
                        sheet,
                        L("ScreenValue"),
                        L("ConfirmValue"),
                        L("UnitOfMeasurement"),
                        (L("Drug")) + L("Name"),
                        (L("TestCategory")) + L("Name")
                        );

                    AddObjects(
                        sheet, confirmationValues,
                        _ => _.ConfirmationValue.ScreenValue,
                        _ => _.ConfirmationValue.ConfirmValue,
                        _ => _.ConfirmationValue.UnitOfMeasurement,
                        _ => _.DrugName,
                        _ => _.TestCategoryName
                        );

                });
        }
    }
}