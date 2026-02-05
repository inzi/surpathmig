namespace inzibackend.Surpath.Dtos
{
    public class GetUserPurchaseForEditOutput
    {
        public CreateOrEditUserPurchaseDto UserPurchase { get; set; }
        
        public string UserName { get; set; }
        
        public string SurpathServiceName { get; set; }
        
        public string TenantSurpathServiceName { get; set; }
        
        public string CohortName { get; set; }
    }
}