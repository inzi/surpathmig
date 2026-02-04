using System.Collections.Generic;
using System.Threading.Tasks;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;

namespace inzibackend.Surpath.Exporting
{
    public interface ICodeTypesExcelExporter
    {
        Task<FileDto> ExportToFile(List<GetCodeTypeForViewDto> codeTypes);
    }
}