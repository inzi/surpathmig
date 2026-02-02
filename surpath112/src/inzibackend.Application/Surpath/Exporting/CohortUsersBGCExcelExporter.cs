using System;
using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using inzibackend.DataExporting.Excel.NPOI;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;
using inzibackend.Storage;
using inzibackend.Authorization;
using Abp.Authorization;
using System.Linq;

namespace inzibackend.Surpath.Exporting
{
    public class CohortUsersBGCExcelExporter : NpoiExcelExporterBase, ICohortUsersBGCExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;
        private readonly IPermissionChecker _permissionChecker;

        public CohortUsersBGCExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            IPermissionChecker permissionChecker,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
            _permissionChecker = permissionChecker;
        }

        public FileDto ExportToFile(List<GetCohortUserForBGCExportDto> cohortUsers)
        {
            return CreateExcelPackage(
                "BGCImport.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet(L("CohortUsers"));

                    // Add header row matching BGC template format
                    AddHeader(
                        sheet,
                        ("First Name"),
                        ("Middle"),
                        ("Last Name"),
                        ("Suffix"),
                        ("SSN"),
                        ("Birth Date"),
                        ("Gender"),
                        ("Country"),
                        ("Address1"),
                        ("Address2"),
                        ("City/District"),
                        ("State/Region/Province"),
                        ("Zip/postal Code"),
                        ("County"),
                        ("Email Address"),
                        ("Phone No"),
                        ("Accounting Code"),
                        ("Position"),
                        ("Folder"),
                        ("Comments"),
                        ("DL State"),
                        ("License Number"),
                        ("DL Terms"),
                        ("Passport Number"),
                        ("Issuing Country"),
                        ("Medical License"),
                        ("Job Location State"),
                        ("Job Location City")
                        );

                    // Add data rows
                    AddObjects(
                        sheet, cohortUsers,
                        _ => _.UserEditDto.Name ?? "",                                                           // First Name
                        _ => _.UserEditDto.Middlename ?? "",                                                     // Middle
                        _ => _.UserEditDto.Surname ?? "",                                                        // Last Name
                        _ => "",                                                                                 // Suffix (not in current schema)
                        _ => _.UserPids?.FirstOrDefault(p => p.PidType.Name == "SSN")?.Pid ?? "",            // SSN
                        _ => _.UserEditDto.DateOfBirth != DateTime.MinValue ? _.UserEditDto.DateOfBirth.ToString("MM-dd-yyyy") : "", // Birth Date
                        _ => "",                                                                                 // Gender (not in current schema)
                        _ => "United States",                                                                    // Country
                        _ => _.UserEditDto.Address ?? "",                                                        // Address1
                        _ => _.UserEditDto.SuiteApt ?? "",                                                       // Address2
                        _ => _.UserEditDto.City ?? "",                                                           // City/District
                        _ => _.UserEditDto.State ?? "",                                                          // State/Region/Province
                        _ => _.UserEditDto.Zip ?? "",                                                            // Zip/postal Code
                        _ => "",                                                                                 // County (not in current schema)
                        _ => "", //_.UserEditDto.EmailAddress ?? "",                                                   // Email Address
                        _ => "", //_.UserEditDto.PhoneNumber ?? "",                                                    // Phone No
                        _ => "",                                                                                 // Accounting Code (not in current schema)
                        _ => "",                                                                                 // Position (not in current schema)
                        _ => "", //_.CohortName ?? "",                                                                 // Folder (using Cohort Name)
                        _ => "",                                                                                 // Comments (not in current schema)
                        _ => "", //_.UserEditDto.State ?? "",                                                          // DL State (using address state)
                        _ => "", //_.UserPids?.FirstOrDefault(p => p.PidType.Name == "DL")?.Pid ?? "",             // License Number (Driver's License)
                        _ => "",                                                                                 // DL Terms (not in current schema)
                        _ => "", //_.UserPids?.FirstOrDefault(p => p.PidType.Name == "Passport")?.Pid ?? "",       // Passport Number
                        _ => "",                                                                                 // Issuing Country (not in current schema)
                        _ => "",                                                                                 // Medical License (not in current schema)
                        _ => "",                                                                                 // Job Location State (not in current schema)
                        _ => ""                                                                                  // Job Location City (not in current schema)
                        );
                });
        }
    }
}