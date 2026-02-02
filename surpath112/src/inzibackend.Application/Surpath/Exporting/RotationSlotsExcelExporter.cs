using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using inzibackend.DataExporting.Excel.NPOI;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;
using inzibackend.Storage;
using System;

namespace inzibackend.Surpath.Exporting
{
    public class RotationSlotsExcelExporter : NpoiExcelExporterBase, IRotationSlotsExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public RotationSlotsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetRotationSlotForViewDto> rotationSlots)
        {
            // Cache these values once to avoid repeated calls to GetUserId() for every row
            var tenantId = _abpSession.TenantId;
            var userId = _abpSession.GetUserId();

            return CreateExcelPackage(
                    "RotationSlots.xlsx",
                    excelPackage =>
                    {
                        var sheet = excelPackage.CreateSheet(L("RotationSlots"));

                        AddHeader(
                            sheet,
                        L("SlotId"),
                        L("AvailableSlots"),
                        L("ShiftStartDate"),
                        L("ShiftEndDate"),
                        L("ShiftStartTime"),
                        L("ShiftEndTime"),
                        L("ShiftHours"),
                        L("NotifyHospital"),
                        L("HospitalNotifiedDateTime"),
                        L("ShiftType"),
                        L("BidStartDateTime"),
                        L("BidEndDateTime"),
                        (L("Hospital")) + L("Name"),
                        (L("MedicalUnit")) + L("Name")
                            );

                        AddObjects(
                            sheet, rotationSlots,
                        _ => _.RotationSlot.SlotId,
                        _ => _.RotationSlot.AvailableSlots,
                        _ => _timeZoneConverter.Convert(_.RotationSlot.ShiftStartDate, tenantId, userId),
                        _ => _timeZoneConverter.Convert(_.RotationSlot.ShiftEndDate, tenantId, userId),
                        _ => _timeZoneConverter.Convert(_.RotationSlot.ShiftStartTime, tenantId, userId),
                        _ => _timeZoneConverter.Convert(_.RotationSlot.ShiftEndTime, tenantId, userId),
                        _ => _.RotationSlot.ShiftHours,
                        _ => _.RotationSlot.NotifyHospital,
                        _ => _.RotationSlot.HospitalNotifiedDateTime.HasValue ? _timeZoneConverter.Convert(_.RotationSlot.HospitalNotifiedDateTime.Value, tenantId, userId) : (DateTime?)null,
                        _ => _.RotationSlot.ShiftType,
                        _ => _timeZoneConverter.Convert(_.RotationSlot.BidStartDateTime, tenantId, userId),
                        _ => _timeZoneConverter.Convert(_.RotationSlot.BidEndDateTime, tenantId, userId),
                        _ => _.HospitalName,
                        _ => _.MedicalUnitName
                            );

                        for (var i = 1; i <= rotationSlots.Count; i++)
                        {
                            SetCellDataFormat(sheet.GetRow(i).Cells[3 - 1], "yyyy-mm-dd");
                        }
                        sheet.AutoSizeColumn(3 - 1); for (var i = 1; i <= rotationSlots.Count; i++)
                        {
                            SetCellDataFormat(sheet.GetRow(i).Cells[4 - 1], "yyyy-mm-dd");
                        }
                        sheet.AutoSizeColumn(4 - 1); for (var i = 1; i <= rotationSlots.Count; i++)
                        {
                            SetCellDataFormat(sheet.GetRow(i).Cells[5 - 1], "yyyy-mm-dd");
                        }
                        sheet.AutoSizeColumn(5 - 1); for (var i = 1; i <= rotationSlots.Count; i++)
                        {
                            SetCellDataFormat(sheet.GetRow(i).Cells[6 - 1], "yyyy-mm-dd");
                        }
                        sheet.AutoSizeColumn(6 - 1); for (var i = 1; i <= rotationSlots.Count; i++)
                        {
                            SetCellDataFormat(sheet.GetRow(i).Cells[9 - 1], "yyyy-mm-dd");
                        }
                        sheet.AutoSizeColumn(9 - 1); for (var i = 1; i <= rotationSlots.Count; i++)
                        {
                            SetCellDataFormat(sheet.GetRow(i).Cells[11 - 1], "yyyy-mm-dd");
                        }
                        sheet.AutoSizeColumn(11 - 1); for (var i = 1; i <= rotationSlots.Count; i++)
                        {
                            SetCellDataFormat(sheet.GetRow(i).Cells[12 - 1], "yyyy-mm-dd");
                        }
                        sheet.AutoSizeColumn(12 - 1);
                    });
        }
    }
}