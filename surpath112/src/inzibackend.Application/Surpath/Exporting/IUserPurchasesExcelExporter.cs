using inzibackend.Surpath.Dtos;
using inzibackend.Dto;
using System.Collections.Generic;

namespace inzibackend.Surpath.Exporting
{
    public interface IUserPurchasesExcelExporter
    {
        FileDto ExportToFile(List<GetUserPurchaseForViewDto> userPurchases);
    }
}