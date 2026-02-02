using System;
using System.Collections.Generic;
using System.Text;

namespace inzibackend.Surpath.Importing
{
    [Serializable]
    public class ImportFromSurscanLiveJobArgs
    {
        public int? TenantId { get; set; }
        public long? UserId { get; set; }
        public int client_id { get; set; }
        public int user_id { get; set; } = 0;
        public int donor_id { get; set; } = 0;
        public int max_donors { get; set; } = 50;
        public int days_old { get; set; } = 90;

        public int? NewTenantId { get; set; }
        public long? NewTeantAdminId { get; set; }
        public string NewTenancyName { get; set; }
        public string NewTenantName { get; set; }
        public string ClientCode { get; set; }
        public string NewAdminEmail { get; set; }

        public bool Created { get; set; } = false;

    }
}
