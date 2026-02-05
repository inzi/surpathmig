using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Dependency;
using inzibackend.Dto;

namespace inzibackend.DataImporting.Excel;

public interface IExcelInvalidEntityExporter<TEntityDto> : ITransientDependency
{
    Task<FileDto> ExportToFile(List<TEntityDto> entities);
}