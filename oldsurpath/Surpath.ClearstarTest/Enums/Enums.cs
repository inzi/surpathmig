namespace Surpath.CSTest
{
    public class Enums
    {
        public enum DeliveryMedhodId
        {
            Fax = 1,
            Email = 2,
            HardCopy = 3,
            FaxCallFirst = 4,
            Internet = 5
        }

        public enum PaymentTermsId
        {
            Empty = 0,
            PayPerProfile = 1
        }

        public static class TimeZone
        {
            public static string CST { get { return "Central Standard Time"; } }
            public static string CSTM { get { return "Central Standard Time (Mexico)"; } }
            public static string PST { get { return "Pacific Standard Time"; } }
            public static string MST { get { return "US Mountain Standard Time"; } }
            public static string EST { get { return "W. Europe Standard Time"; } }
        }
    }
}