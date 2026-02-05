using System;
using System.Collections.Generic;
using System.Text;

namespace inzibackend.Surpath.Purchase
{
    public class AuthorizeNetSettings
    {
        public string ApiLoginID { get; set; }
        public string PublicClientKey { get; set; }
        public string ApiTransactionKey { get; set; }
        public string TransactionId { get; set; }
    }
}
