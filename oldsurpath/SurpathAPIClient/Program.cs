using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurpathAPIClient
{
    public class Program
    {

        public static string SurpathAPIURL = string.Empty;
        public static string SurpathKey = string.Empty;
        public static bool AllowHttp = false;
        public static bool ValidateOnStartup = false;
        public static string partner_donor_id = "myid2";

        static void Main(string[] args)
        {

            Console.WriteLine("See source for examples");

            Console.WriteLine("Press any start");

            Console.ReadKey();
            
            if (ConfigurationManager.AppSettings.AllKeys.Contains("SurpathKey"))
            {
                SurpathKey = ConfigurationManager.AppSettings["SurpathKey"].ToString().Trim();

                loadAppSettings();

                RunExamples();

            }
            else
            {
                Console.WriteLine("Set SurpathKey in config before testing");

            }


            Console.WriteLine();
            Console.WriteLine("Press any key to continue");

            Console.ReadKey();

        }

        static void RunExamples()
        {

            if (string.IsNullOrEmpty(SurpathKey) || string.IsNullOrEmpty(SurpathAPIURL))
            {
                Console.WriteLine("See instructions and set your key or url");
                return;

            }
            Console.WriteLine(); Console.WriteLine();
            // create test client
            SurpathAPITestClient surpathAPITestClient = new SurpathAPITestClient(SurpathAPIURL, SurpathKey, ValidateOnStartup, AllowHttp);

            // call echo to validate online (optional)
            var _res = surpathAPITestClient.Echo().GetAwaiter().GetResult();

            Console.WriteLine($"Echo Results: {_res}");

            // get integration settings
            var _settings = surpathAPITestClient.getSettings().GetAwaiter().GetResult();

            Console.WriteLine(_settings.ToString());
            Console.Write(surpathAPITestClient.JsonResult);
            Console.WriteLine(); Console.WriteLine();


            // save the login url to variable
            var _login_url = _settings.login_url;

            // change it
            _settings.login_url = SurpathIntegrationHelper.b64encode("https://example.com/");

            var _htmlInstructions = _settings.html_instructions;
            var _newHtml = $@"
    <div>
        Visit <a href='{_settings.login_url}'>{_settings.login_url}</a> by clicking here to find your ID
    </div>
";
            _settings.html_instructions = SurpathIntegrationHelper.b64encode(_newHtml);
            // save it
            _res = surpathAPITestClient.postSettings(_settings).GetAwaiter().GetResult();

            Console.WriteLine($"Save Results: {_res}");
            Console.Write(surpathAPITestClient.JsonResult);
            Console.WriteLine(); Console.WriteLine();


            // set it back
             _settings.login_url = _login_url;
            _res = surpathAPITestClient.postSettings(_settings).GetAwaiter().GetResult();

            Console.WriteLine($"Set back and save Results: {_res}");
            Console.Write(surpathAPITestClient.JsonResult);
            Console.WriteLine(); Console.WriteLine();


            Console.WriteLine($"Match Donor");
            ApiIntegrationMatch apiIntegrationMatch = new ApiIntegrationMatch();
            // Complete this data for exact matches
            apiIntegrationMatch.partner_donor_id = partner_donor_id; 
            apiIntegrationMatch.donor_first_name = "";
            apiIntegrationMatch.donor_last_name = "";
            apiIntegrationMatch.partner_client_name = "";
            surpathAPITestClient.getDonorMatch(apiIntegrationMatch).GetAwaiter().GetResult();
            Console.WriteLine($"donormatch Results: {surpathAPITestClient.JsonResult}");
            Console.WriteLine(); Console.WriteLine();


            ApiIntegrationFilter apiIntegrationFilter = new ApiIntegrationFilter();

            var _d = surpathAPITestClient.getDonors(apiIntegrationFilter).GetAwaiter().GetResult();
            Console.WriteLine($"getDonors Results: {surpathAPITestClient.JsonResult}");
            Console.WriteLine(); Console.WriteLine();


            apiIntegrationFilter.fromDateTime = DateTime.Now.Date.AddHours(-12);

            _d = surpathAPITestClient.getDonors(apiIntegrationFilter).GetAwaiter().GetResult();
            Console.WriteLine($"getDonors filtered Results: {surpathAPITestClient.JsonResult}");
            Console.WriteLine(); Console.WriteLine();


            apiIntegrationFilter.partner_donor_id = partner_donor_id;

            var _docs = surpathAPITestClient.getDonorDocuments(apiIntegrationFilter).GetAwaiter().GetResult();
            Console.WriteLine($"getDonorDocuments filtered Results: {surpathAPITestClient.JsonResult}");
            Console.WriteLine(); Console.WriteLine();
            

        }

        /// <summary>
        /// Load parameters from config file
        /// </summary>
        static void loadAppSettings()
        {

            if (ConfigurationManager.AppSettings.AllKeys.Contains("SurpathAPIURL"))
            {
                SurpathAPIURL = ConfigurationManager.AppSettings["SurpathAPIURL"].ToString().Trim();
            }
            else
            {
                SurpathAPIURL = @"https://stage.surpath.com/api/Integration";
            }

            if (ConfigurationManager.AppSettings.AllKeys.Contains("AllowHttp"))
            {
                var _allowHttp = ConfigurationManager.AppSettings["AllowHttp"].ToString().Trim();
                bool.TryParse(_allowHttp, out bool allowHttp);
                AllowHttp = allowHttp;
            }

            if (ConfigurationManager.AppSettings.AllKeys.Contains("ValidateOnStartup"))
            {
                var _validateOnStartup = ConfigurationManager.AppSettings["ValidateOnStartup"].ToString().Trim();
                bool.TryParse(_validateOnStartup, out bool validateOnStartup);
                ValidateOnStartup = validateOnStartup;
            }

            if (ConfigurationManager.AppSettings.AllKeys.Contains("partner_donor_id"))
            {
                partner_donor_id = ConfigurationManager.AppSettings["partner_donor_id"].ToString().Trim();
            }
        }

    }
}
