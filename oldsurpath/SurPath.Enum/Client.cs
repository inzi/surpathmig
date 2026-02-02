using System.ComponentModel;

namespace SurPath.Enum
{
    public enum ClientTypes
    {
        None = 0,
        School = 1,
        Company = 2
    }

    public enum ClientMROTypes
    {
        None = 0,
        MPOS = 1,
        MALL = 2
    }

    public enum ClientPaymentTypes
    {
        [Description("None")]
        None = 0,

        [Description("Donor Pays")]
        DonorPays = 1,

        [Description("Invoice Client")]
        InvoiceClient = 2
    }
}