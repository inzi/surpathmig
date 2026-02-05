using System;

namespace inzibackend.Surpath.Dtos
{
    public class GetUserPurchaseForViewDto
    {
        public UserPurchaseDto UserPurchase { get; set; }
        
        public string UserName { get; set; }
        
        public string SurpathServiceName { get; set; }
        
        public string TenantSurpathServiceName { get; set; }
        
        public string CohortName { get; set; }
    }
}