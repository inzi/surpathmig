using System;
using System.Collections.Generic;
using System.Text;

namespace inzibackend.Surpath
{
    public enum EnumOverAllTestResult
    {
        None = 0,

        Positive = 1,

        Negative = 2,

        Discard = 4,

        Other = 4
    }
}

//public enum OverAllTestResult
//{
//    [Description("None")]
//    None = 0,

//    [Description("POSITIVE")]
//    Positive = 1,

//    [Description("NEGATIVE")]
//    Negative = 2,

//    [Description("DISCARD")]
//    Discard = 4,

//    [Description("OTHER")]
//    Other = 4
//}