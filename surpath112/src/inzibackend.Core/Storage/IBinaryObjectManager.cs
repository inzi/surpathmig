using System;
using System.Threading.Tasks;

namespace inzibackend.Storage
{
    public interface IBinaryObjectManager
    {
        Task<BinaryObject> GetOrNullAsync(Guid id, int? TenantId = null, bool tracking = true);

        Task<String?> GetDescription(Guid id, int? TenantId = null);

        Task SaveAsync(BinaryObject file, int? TenantId = null);

        Task DeleteAsync(Guid id, int? TenantId = null);
    }
}