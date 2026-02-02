using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using inzibackend.MultiTenancy.Accounting.Dto;

namespace inzibackend.MultiTenancy.Accounting
{
    public interface IInvoiceAppService
    {
        Task<InvoiceDto> GetInvoiceInfo(EntityDto<long> input);

        Task CreateInvoice(CreateInvoiceDto input);
    }
}
