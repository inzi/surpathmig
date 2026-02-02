using System.Threading.Tasks;
using Abp.Dependency;

namespace inzibackend.MultiTenancy.Accounting
{
    public interface IInvoiceNumberGenerator : ITransientDependency
    {
        Task<string> GetNewInvoiceNumber();
    }
}