using inzibackend.Editions;
using inzibackend.Editions.Dto;
using inzibackend.MultiTenancy.Payments;
using inzibackend.Security;
using inzibackend.MultiTenancy.Payments.Dto;

namespace inzibackend.Web.Models.TenantRegistration
{
    public class TenantRegisterViewModel
    {
        public PasswordComplexitySetting PasswordComplexitySetting { get; set; }

        public int? EditionId { get; set; }

        public SubscriptionStartType? SubscriptionStartType { get; set; }

        public EditionSelectDto Edition { get; set; }

        public EditionPaymentType EditionPaymentType { get; set; }
    }
}
