using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using inzibackend.DataExporting.Excel.MiniExcel;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;
using inzibackend.Storage;

namespace inzibackend.Surpath.Exporting
{
    public class CodeTypesExcelExporter : MiniExcelExcelExporterBase, ICodeTypesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public CodeTypesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public async Task<FileDto> ExportToFile(List<GetCodeTypeForViewDto> codeTypes)
        {
            var items = codeTypes.Select(codeType => new Dictionary<string, object>
            {
                { L("Name"), codeType.CodeType.Name }
            }).ToList();

            return await CreateExcelPackageAsync("CodeTypes.xlsx", items);
        }
    }
}