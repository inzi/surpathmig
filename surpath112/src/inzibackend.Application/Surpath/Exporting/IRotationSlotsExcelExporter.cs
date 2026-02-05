using System.Collections.Generic;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;

namespace inzibackend.Surpath.Exporting
{
    public interface IRotationSlotsExcelExporter
    {
        FileDto ExportToFile(List<GetRotationSlotForViewDto> rotationSlots);
    }
}