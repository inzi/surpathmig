using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inzibackend.Surpath.Importing
{
    public class ClientUserOrAdmin
    {
        public long Id { get; set; }
        public int user_id { get; set; }
        public int donor_id { get; set; }
        public string user_email { get; set; }
        public string user_first_name { get; set; }
        public string user_last_name { get; set; }
        public string user_password { get; set; }
        public int client_id { get; set; }
        public string client_code { get; set; }
        public string client_name { get; set; }
        public int is_client_active { get; set; }
        public int client_department_id { get; set; }
        public int is_contact_info_as_client { get; set; }
        public string lab_code { get; set; }
        public string phone_number { get; set; }
    }
}
