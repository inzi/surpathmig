using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SurpathAPIClient
{

    public static class SurpathIntegrationHelper
    {
        public static string b64encode(string b)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(b);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string b64decode(string b)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(b);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
        public static string StringNullChecker(String s)
        {
            if (string.IsNullOrEmpty((string)s)) return string.Empty;
            return (string)s;
        }
        public static string EncryptWithKey(string toEncrypt, string key)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            hashmd5.Clear();

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tdes.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        public static string DecryptWithKey(string cipherString, string key)
        {
            byte[] keyArray;
            byte[] toEncryptArray = Convert.FromBase64String(cipherString);

            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            hashmd5.Clear();

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tdes.Clear();
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
    }

    /// <summary>
    /// For attempting to match a donor
    /// </summary>
    public class ApiIntegrationMatch
    {
        public string donor_first_name { get; set; } = string.Empty;
        public string donor_last_name { get; set; } = string.Empty;
        public string partner_donor_id { get; set; } = string.Empty; // external id
        public string partner_client_code { get; set; } = string.Empty; // School code
        public string partner_client_id { get; set; } = string.Empty; // School code id
        public string partner_client_name { get; set; } = string.Empty; // School Name
        public int maxResults { get; set;  } = 0;
    }
    public class ApiIntegrationMatchResult
    {
        public string donor_first_name { get; set; }
        public string donor_last_name { get; set; }
        public string partner_donor_id { get; set; } // external id
        public string partner_client_code { get; set; } // School code
        public string partner_client_id { get; set; } // School code id
        public string partner_client_name { get; set; }
    }

    /// <summary>
    /// Results of donormatch, with exact and possible
    /// </summary>
    public class ApiIntegrationMatchResults
    {
        public bool result { get; set; } = false;
        public ApiIntegrationMatch apiIntegrationMatch { get; set; } = new ApiIntegrationMatch();
        public string message { get; set; } = string.Empty;
        public List<ApiIntegrationMatch> exact_matchs = new List<ApiIntegrationMatch>();
        public List<ApiIntegrationMatch> possible_matchs = new List<ApiIntegrationMatch>();
    }

    /// <summary>
    /// Base model for test result
    /// </summary>
    public class ApiIntegrationResult
    {
        public bool success { get; set; } = false;
        public string message { get; set; } = string.Empty;
    }

    /// <summary>
    /// Model for client data exchange
    /// </summary>
    public class IntegrationPartnerClientDTO
    {
        public Guid backend_integration_partner_client_map_GUID { get; set; } // Surpath use only
        public string client_name { get; set; } // this is OUR client name
        public string client_department_name { get; set; } // this is OUR client department name
        public string partner_client_id { get; set; } // this is the integrdation partner's ID for the client
        public string partner_client_code { get; set; } // this is the integrdation partner's client code for the client
        public DateTime created_on { get; set; } // Surpath use only
        public DateTime last_modified_on { get; set; } // Surpath use only
        public string last_modified_by { get; set; } // Surpath use only
    }

    /// <summary>
    /// Model for filtering results sets
    /// </summary>
    public class ApiIntegrationFilter
    {
        public DateTime fromDateTime { get; set; } // Only results after this date
        public DateTime toDateTime { get; set; }  // Only results before this date
        public int maxResults { get; set; } = 0; // Maximum results

        public string partner_client_id { get; set; } // Your client ID
        public string partner_client_code { get; set; } // Your client code
        public string partner_donor_id { get; set; } // Your ID for the donor
    }

    /// <summary>
    /// Model for returned multi donor result
    /// </summary>
    public class IntegrationDonors
    {
        public List<IntegrationDonor> donors { get; set; } = new List<IntegrationDonor>(); // List of Donors
    }


    /// <summary>
    /// Base donor model
    /// </summary>
    public class IntegrationDonor
    {
        public string id { get; set; } // The Donor's ID
        public string partner_donor_id { get; set; } // Your ID for the donor
        public List<IntegrationDonorTests> tests { get; set; } = new List<IntegrationDonorTests>(); // Lists of tests (drug, background, etc)
    }

    /// <summary>
    /// Base model for donor tests
    /// </summary>
    public class IntegrationDonorTests
    {
        public string id { get; set; } // The ID of the test
        public string category { get; set; } // The test category
        public int categoryid { get; set; } // The ID of the test category
        public string status { get; set; } // The status of the test
        public int statusid { get; set; } // The ID of the status of the test
        public string bccode { get; set; } // The background check code
        public DateTime created_on { get; set; } // When this object was created
        public DateTime test_requested_date { get; set; } // When the test was requested
        public IntegrationDonorDocument document { get; set; }
    }

    /// <summary>
    /// The base model for test related documents
    /// </summary>
    public class IntegrationDonorDocument
    {
        public int id { get; set; } // The ID of the document
        public string filename { get; set; } // The filename of the document
        public string base64 { get; set; } // The document's content base64 encode
        public DateTime created_on { get; set; } // When the document was created
        public DateTime received_on { get; set; } // When the document was received (See Filter)
        public string report_type { get; set; } // What type of report this is (drug (lab/mro), background, etc.)
        public DateTime test_requested_date { get; set; } // When the test was requested
    }

    /// <summary>
    /// Integration Settings
    /// </summary>
    public class IntegrationPartnerDTO
    {
        public IntegrationPartnerDTO()
        {
            this.html_instructions = string.Empty;
            this.login_url = string.Empty;
        }
        public int backend_integration_partner_id { get; set; } // The ID of the integration
        public string partner_name { get; set; } // Your company name
        public string partner_key { get; set; } // Your key
        public string partner_crypto { get; set; } // Share cryptography key for encrypting / decrypting
        public string html_instructions { get; set; } = string.Empty; // Base64 encoded string (HTML), used by donors for instructions on how to get your ID for the donor
        public string login_url { get; set; } = string.Empty; // Base64 encoded string, If integration requires remote login, where surpath links the user to
        public string partner_push_host { get; set; } // Used for SFTP push 
        public string partner_push_username { get; set; } // SFTP login
        public string partner_push_password { get; set; } // SFTP password
        public string partner_push_port { get; set; } // SFTP port
        public string partner_push_path { get; set; } // optional sub folder
    }

    /*
     * html_instructions
     * 
     * This is best formed as a div tag, with external references for images
     * 
     * Surpath uses bootstrap theming and be tested on stage
     * 
     * A basic example might be:
     * 
     * <div>
     * <p>Login at this url: <a href="[url]" target="_blank">[url]</a></p>
     * <p>Go to profile</p>
     * <p>copy the value from the field 'MY ID'</p>
     * <p>Past that value into the integation ID on the Surpath Registration form</p>
     * <p>MAKE SURE IT IS CORRECT OR YOUR RESULTS COULD BE DELAYED!!</p>
     * </div>
     * 
     * 
     */

}
