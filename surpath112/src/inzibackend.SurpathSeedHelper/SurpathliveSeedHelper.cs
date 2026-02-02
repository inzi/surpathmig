using MySql.Data.MySqlClient;
using System.Configuration;
using System.Globalization;

namespace inzibackend.SurpathSeedHelper
{
    public class SurpathliveSeedHelper
    {

        public string ConnectionString { get; set; }
        public CultureInfo Culture { get; set; }
        public MySqlTransaction TransactionObject { get; set; }
        public MySqlConnection conn { get; set; }
        public SurpathliveSeedHelper(string _ConnectionString)
        {
            this.ConnectionString = _ConnectionString;
            this.conn = new MySqlConnection(this.ConnectionString);
        }

    }
}