using System;
using System.Collections.Generic;
using System.Text;

namespace inzibackend.Surpath
{
    public enum EnumClientPaymentType
    {
        InvoiceClient = 0,

        DonorPays = 1,

        Other = 2,

        DeferDonorPerpetualPay = 3
    }
}

//public enum ClientPaymentTypes
//{
//    [Description("None")]
//    None = 0,

//    [Description("Donor Pays")]
//    DonorPays = 1,

//    [Description("Invoice Client")]
//    InvoiceClient = 2
//}