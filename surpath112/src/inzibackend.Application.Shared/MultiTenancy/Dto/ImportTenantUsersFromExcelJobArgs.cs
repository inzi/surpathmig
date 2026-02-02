using System;
using Abp;

namespace inzibackend.MultiTenancy.Dto
{
    public class ImportTenantUsersFromExcelJobArgs
    {
        public int TenantId { get; set; }

        public Guid BinaryObjectId { get; set; }

        public UserIdentifier User { get; set; }
    }
}
