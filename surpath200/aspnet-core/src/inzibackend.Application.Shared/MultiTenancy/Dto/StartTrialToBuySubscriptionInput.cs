using inzibackend.MultiTenancy.Payments;

namespace inzibackend.MultiTenancy.Dto;

public class StartTrialToBuySubscriptionInput
{
    public PaymentPeriodType PaymentPeriodType { get; set; }

    public string SuccessUrl { get; set; }

    public string ErrorUrl { get; set; }
}

