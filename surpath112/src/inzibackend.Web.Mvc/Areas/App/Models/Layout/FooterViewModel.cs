using inzibackend.Sessions.Dto;

namespace inzibackend.Web.Areas.App.Models.Layout
{
    public class FooterViewModel
    {
        public GetCurrentLoginInformationsOutput LoginInformations { get; set; }

        public string GetProductNameWithEdition()
        {
            const string productName = "Surpath";

            if (LoginInformations.Tenant?.Edition?.DisplayName == null)
            {
                return productName;
            }

            return productName + " 0.7.0"; // + LoginInformations.Tenant.Edition.DisplayName;
        }
    }

    public class SubheaderViewModel
    {
        public string Title { get; set; }

        public string Description { get; set; }
    }
}
