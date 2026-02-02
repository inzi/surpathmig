using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurPath.Entity
{
    //[Serializable]
    public class ApiCLients
    {
        public List<IntegrationPartnerClient> clients { get; set; } = new List<IntegrationPartnerClient>();
    }

    //[Serializable]
    public class ApiIntegrationResult
    {
        public bool success { get; set; } = false;
        public string message { get; set; } = string.Empty;
    }

    //[Serializable]
    public class ApiIntegrationFilter
    {
        public DateTime fromDateTime { get; set; }
        public DateTime toDateTime { get; set; }
        public int maxResults { get; set; } = 0;
        //        public bool assendingResults { get; set; } = false;
        public string partner_client_id { get; set; }
        public string partner_client_code { get; set; }
        public string partner_donor_id { get; set; }
    }

    public class ApiIntegrationMatch
    {
        public string donor_email { get; set; }
        public string donor_first_name { get; set; }
        public string donor_last_name { get; set; }
        public string partner_donor_id { get; set; } // external id
        public string partner_client_code { get; set; } // School code
        public string partner_client_id { get; set; } // School code id
        public string partner_client_name { get; set; }
        public bool exact_only { get; set; } = true;
        public int maxResults { get; set; } = 0;
    }

    public class ApiIntegrationMatchResult
    {
        public int donor_id { get; set; }
        public string donor_email { get; set; }
        public string donor_first_name { get; set; }
        public string donor_last_name { get; set; }
        public string partner_donor_id { get; set; } // external id
        public string partner_client_code { get; set; } // School code
        public string partner_client_id { get; set; } // School code id
        public string partner_client_name { get; set; }
    }

    public class ApiIntegrationMatchResults
    {
        public bool result { get; set; } = false;
        public ApiIntegrationMatch apiIntegrationMatch { get; set; } = new ApiIntegrationMatch();
        public string message { get; set; } = string.Empty;
        public List<ApiIntegrationMatchResult> exact_matchs = new List<ApiIntegrationMatchResult>();
        public List<ApiIntegrationMatchResult> possible_matchs = new List<ApiIntegrationMatchResult>();
    }

    //[Serializable]
    public class IntegrationDonorDocument
    {
        public int id { get; set; }
        public string filename { get; set; }
        public string base64 { get; set; }
        public DateTime created_on { get; set; }
        public DateTime received_on { get; set; }
        public string report_type { get; set; }
        public int report_typeid { get; set; }
        public DateTime test_requested_date { get; set; }
    }

    //[Serializable]
    public class IntegrationDonorTests
    {
        public string id { get; set; }
        public string category { get; set; }
        public int categoryid { get; set; }
        public string status { get; set; }
        public int statusid { get; set; }
        public string bccode { get; set; }
        public DateTime created_on { get; set; }
        public DateTime test_requested_date { get; set; }
        public IntegrationDonorDocument document { get; set; }
    }

    //[Serializable]
    public class IntegrationDonor
    {
        public string id { get; set; }
        public string partner_donor_id { get; set; }
        public List<IntegrationDonorTests> tests { get; set; } = new List<IntegrationDonorTests>();
    }

    //[Serializable]
    public class IntegrationDonors
    {
        public List<IntegrationDonor> donors { get; set; } = new List<IntegrationDonor>();
    }

    public class IntegrationDonorsDataRow
    {
        public string donorid { get; set; }
        public string donoraltid { get; set; }
        public string donortestid { get; set; }
        public string testcategory { get; set; }
        public int testcategoryid { get; set; }
        public string teststatus { get; set; }
        public int testid { get; set; }
        public int teststatusid { get; set; }
        public string donorClearstarProfId { get; set; }
        public string deptclearstarcode { get; set; }
        public DateTime testcreated_on { get; set; }
        public DateTime test_requested_date { get; set; }
        public int docid { get; set; }
        public string docbase64 { get; set; }
        public DateTime doccreated_on { get; set; }
        public DateTime doctest_requested_date { get; set; }
        public string filename { get; set; }
        public DateTime received_on { get; set; }
        public string report_type { get; set; }
        public int report_typeid { get; set; }
        public int final_report_id { get; set; }
        public int donor_document_id { get; set; }
        public DateTime document_upload_time { get; set; }
        public string document_content { get; set; }
        public string document_title { get; set; }
        public int donor_document_typeId { get; set; }

    }

    public class IntegrationPartnerRelease
    {
        //        CREATE TABLE `backend_integration_partner_release` (
        //  `backend_integration_partner_release_id` int (11) NOT NULL AUTO_INCREMENT,
        public int backend_integration_partner_release_id { get; set; }
        //  `backend_integration_partner_release_GUID` char (36) NOT NULL DEFAULT '',
        public string backend_integration_partner_release_GUID { get; set; }
        //  `donor_test_info_id` int (11) NOT NULL,
        public int donor_test_info_id { get; set; }
        //   `report_info_id` int (11) NOT NULL DEFAULT '0',
        public int report_info_id { get; set; }
        //  `donor_document_id` int (11) NOT NULL DEFAULT '0',
        public int donor_document_id { get; set; }
        //  `background_check` int (11) NOT NULL DEFAULT '0',
        public bool background_check { get; set; }
        //  `released` tinyint(4) NOT NULL DEFAULT '0',
        public bool released { get; set; }
        //  `sent` tinyint(4) NOT NULL DEFAULT '0',
        public bool sent { get; set; }
        //  `created_on` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
        public DateTime created_on { get; set; }
        //  `last_modified_on` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
        public DateTime last_modified_on { get; set; }
        //  `last_modified_by` varchar(200) NOT NULL,
        public string last_modified_by { get; set; }
        //  `released_by` varchar(200) NOT NULL,
        public string released_by { get; set; }
        //  `sent_on` timestamp NULL DEFAULT NULL,
        public DateTime sent_on { get; set; }
        public int donor_test_test_category_id { get; set; }
        //  PRIMARY KEY(`backend_integration_partner_release_id`),
        //  UNIQUE KEY `id_UNIQUE` (`backend_integration_partner_release_id`)
        //) ENGINE=InnoDB DEFAULT CHARSET=latin1;

        public int donor_id { get; set; }
        public int client_id { get; set; }
        public int client_department_id { get; set; }


    }


}
