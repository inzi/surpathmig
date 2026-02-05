using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using inzibackend.DataExporting.Excel.NPOI;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;
using inzibackend.Storage;

namespace inzibackend.Surpath.Exporting
{
    public class MedicalUnitsExcelExporter : NpoiExcelExporterBase, IMedicalUnitsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public MedicalUnitsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetMedicalUnitForViewDto> medicalUnits)
        {
            return CreateExcelPackage(
                    "MedicalUnits.xlsx",
                    excelPackage =>
                    {

                        var sheet = excelPackage.CreateSheet(L("MedicalUnits"));

                        AddHeader(
                            sheet,
                        L("Name"),
                        L("PrimaryContact"),
                        L("PrimaryContactPhone"),
                        L("PrimaryContactEmail"),
                        L("Address1"),
                        L("Address2"),
                        L("City"),
                        L("State"),
                        L("ZipCode"),
                        (L("Hospital")) + L("Name")
                            );

                        AddObjects(
                            sheet, medicalUnits,
                        _ => _.MedicalUnit.Name,
                        _ => _.MedicalUnit.PrimaryContact,
                        _ => _.MedicalUnit.PrimaryContactPhone,
                        _ => _.MedicalUnit.PrimaryContactEmail,
                        _ => _.MedicalUnit.Address1,
                        _ => _.MedicalUnit.Address2,
                        _ => _.MedicalUnit.City,
                        _ => _.MedicalUnit.State,
                        _ => _.MedicalUnit.ZipCode,
                        _ => _.HospitalName
                            );

                    });

        }
    }
}