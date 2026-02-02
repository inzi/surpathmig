using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using inzibackend.DataExporting.Excel.NPOI;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;
using inzibackend.Storage;

namespace inzibackend.Surpath.Exporting
{
    public class UserPidsExcelExporter : NpoiExcelExporterBase, IUserPidsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public UserPidsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetUserPidForViewDto> userPids)
        {
            return CreateExcelPackage(
                "UserPids.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("UserPids"));

                    AddHeader(
                        sheet,
                        L("Pid"),
                        L("Validated"),
                        (L("PidType")) + L("Name"),
                        (L("User")) + L("Name")
                        );

                    AddObjects(
                        sheet, userPids,
                        _ => _.UserPid.Pid,
                        _ => _.UserPid.Validated,
                        _ => _.PidTypeName,
                        _ => _.UserName
                        );

                });
        }
    }
}