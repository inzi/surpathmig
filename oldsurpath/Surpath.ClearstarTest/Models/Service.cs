namespace Surpath.CSTest.Models
{
    public class ServiceModelModel
    {
        public string CustId { get; set; }
        public string ServiceNo { get; set; }
        public string Description { get; set; }
        public bool isRequired { get; set; }
        public int PreferedSourceId { get; set; } //ToDo; need to discuss this value
        public bool HoldBeforeComplete { get; set; }
        public bool isCanReleaseHold { get; set; }
        public bool isVisiable { get; set; }
        public decimal Charge { get; set; } //ToDO: need to discuss this value
        public decimal ComponentCharge { get; set; }
        public bool isComponentLimit { get; set; }
        public decimal ExceededCharge { get; set; }
        public int DistId { get; set; }
        public decimal DistServiceCost { get; set; }
        public bool isIncludeDistSchg { get; set; }
    }
}