using inzibackend.Surpath.Dtos;

namespace inzibackend.Web.Areas.App.Models.UserPurchases
{
    public class CreateOrEditUserPurchaseModalViewModel
    {
        public CreateOrEditUserPurchaseDto UserPurchase { get; set; }

        public string UserName { get; set; }

        public string SurpathServiceName { get; set; }

        public string TenantSurpathServiceName { get; set; }

        public string CohortName { get; set; }

        public bool IsEditMode => UserPurchase.Id.HasValue;
    }
}