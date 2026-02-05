using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace inzibackend.Surpath
{
    public enum EnumStatusComplianceImpact
    {
        [Description("InformationOnly")]
        InformationOnly = 0,
        [Description("Compliant")]
        Compliant = 1,
        [Description("NotCompliant")]
        NotCompliant = 2,

    }
}
//public enum ReportType
//{
//    [Description("None")]
//    None = 0,
//    [Description("LabReport")]
//    LabReport = 1,
//    [Description("MROReport")]
//    MROReport = 2,
//    [Description("QuestLabReport")]
//    QuestLabReport = 3,
//    [Description("ChainOfCustodyReport")]
//    ChainOfCustodyReport = 4
//}