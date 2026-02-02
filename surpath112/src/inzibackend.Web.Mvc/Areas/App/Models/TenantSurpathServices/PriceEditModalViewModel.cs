using System;

namespace inzibackend.Web.Areas.App.Models.TenantSurpathServices
{
    public class PriceEditModalViewModel
    {
        public string Level { get; set; }
        public string TargetId { get; set; }
        public string TargetName { get; set; }
        public double? CurrentPrice { get; set; }
        public bool CurrentIsInvoiced { get; set; }
        public Guid ServiceId { get; set; }
        public int TenantId { get; set; }
    }
}