using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inzibackend.Surpath.Importing
{
    public class ClientRecordCategoryClass
    {
        public int client_id { get; set; }
        public int client_department_id { get; set; }
        public string lab_code { get; set; }
        public string client_code { get; set; }
        public string description { get; set; }
        public string instructions { get; set; }
    }
}
