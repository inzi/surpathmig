using System.ComponentModel;

namespace SurPath.Enum
{
    public enum VendorTypes
    {
        [Description("None")]
        None = 0,

        [Description("Collection Center")]
        CollectionCenter = 1,

        [Description("Lab")]
        Lab = 2,

        [Description("MRO")]
        MRO = 3
    }

    public enum VendorStatus
    {
        [Description("None")]
        None = 0,

        [Description("Active")]
        Active = 1,

        [Description("Inactive")]
        Inactive = 2
    }
}