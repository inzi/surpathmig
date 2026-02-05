using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using inzibackend.DataExporting.Excel.NPOI;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;
using inzibackend.Storage;

namespace inzibackend.Surpath.Exporting
{
    public class HospitalsExcelExporter : NpoiExcelExporterBase, IHospitalsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public HospitalsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetHospitalForViewDto> hospitals)
        {
            return CreateExcelPackage(
                    "Hospitals.xlsx",
                    excelPackage =>
                    {

                        var sheet = excelPackage.CreateSheet(L("Hospitals"));

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
                        L("ZipCode")
                            );

                        AddObjects(
                            sheet, hospitals,
                        _ => _.Hospital.Name,
                        _ => _.Hospital.PrimaryContact,
                        _ => _.Hospital.PrimaryContactPhone,
                        _ => _.Hospital.PrimaryContactEmail,
                        _ => _.Hospital.Address1,
                        _ => _.Hospital.Address2,
                        _ => _.Hospital.City,
                        _ => _.Hospital.State,
                        _ => _.Hospital.ZipCode
                            );

                    });

        }
    }
}