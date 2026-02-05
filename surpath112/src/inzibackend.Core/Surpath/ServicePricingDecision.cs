using System;

namespace inzibackend.Surpath
{
    /// <summary>
    /// Represents the resolved pricing outcome for a Surpath service after applying hierarchy overrides.
    /// </summary>
    public class ServicePricingDecision
    {
        public Guid SurpathServiceId { get; set; }
        public double EffectivePrice { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsInvoiced { get; set; }

        /// <summary>
        /// The amount that should actually be charged to the end user.
        /// </summary>
        public double PriceToCharge => IsInvoiced ? 0d : EffectivePrice;
    }
}
