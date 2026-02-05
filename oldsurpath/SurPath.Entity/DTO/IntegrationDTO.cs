using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurPath.Entity
{
    public class IntegrationPartnerClientDTO
    {
        public Guid backend_integration_partner_client_map_GUID { get; set; } // Surpath use only
        public string client_name { get; set; } // this is OUR client name
        public string client_department_name { get; set; } // this is OUR client department name
        public string partner_client_id { get; set; } // this is the integration partner's ID for the client
        public string partner_client_code { get; set; } // this is the integration partner's client code for the client
        //public string html_instructions { get; set; } // Base64 encoded HTML for user instructions
        //public string login_url { get; set; } // base64 encoded URL for user to login
        public string partner_push_folder { get; set; } // this optional value for ftp pushes, a drop folder. If supplied will append to partner_push_path
        public DateTime created_on { get; set; } // Surpath use only
        public DateTime last_modified_on { get; set; } // Surpath use only
        public string last_modified_by { get; set; } // Surpath use only
        //public bool active { get; set; } // Surpath use only
    }
}
