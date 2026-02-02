using System.Collections.Generic;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;

namespace inzibackend.Surpath.Exporting
{
    public interface IConfirmationValuesExcelExporter
    {
        FileDto ExportToFile(List<GetConfirmationValueForViewDto> confirmationValues);
    }
}