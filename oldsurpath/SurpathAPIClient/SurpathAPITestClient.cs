using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace SurpathAPIClient
{
    public class SurpathAPITestClient
    {

        private string __SurpathAPIURL { get; set; } = string.Empty;
        private string __SurpathKey { get; set; } = string.Empty;

        public string JsonResult { get; set; } = string.Empty;

        public SurpathAPITestClient(string SurpathAPIURL, string SurpathKey, bool validateOnStartup = false, bool allowHttp = false)
        {
            if (!SurpathAPIURL.EndsWith("/")) SurpathAPIURL = SurpathAPIURL + "/";
            if (!validURL(SurpathAPIURL, allowHttp)) throw new Exception("Invalid Surpath API URL");

            this.__SurpathAPIURL = SurpathAPIURL;
            this.__SurpathKey = SurpathKey;

            if (validateOnStartup==true)
            {
                if (Echo().GetAwaiter().GetResult() == false) throw new Exception("Surpath API Unavailable");
            }
        }


        #region private

        private void SetJsonResult(ref MemoryStream ms)
        {
            StreamReader reader = new StreamReader(ms);
            this.JsonResult = reader.ReadToEnd();
            ms.Seek(0, SeekOrigin.Begin);
        }
        private bool validURL(string url, bool allowHttp = false)
        {
            try
            {
                Uri uriResult;
                bool isvalid = Uri.TryCreate(url, UriKind.Absolute, out uriResult) && (uriResult.Scheme == Uri.UriSchemeHttps);
                if (allowHttp)
                    isvalid = isvalid = Uri.TryCreate(url, UriKind.Absolute, out uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
                return isvalid;
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }

        private async Task<string> BuildURI(string method)
        {
            try
            {
                var _uri = new Uri(__SurpathAPIURL);
                var _url = new Uri(_uri, method);
                return _url.ToString();
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        #endregion end private

        /// <summary>
        /// Call echo to check response
        /// </summary>
        /// <returns></returns>
        public async Task<bool> Echo()
        {
            try
            {
                var method = "echo";

                var http = new HttpClient();
                var url = await BuildURI(method);
                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(url),
                    Method = HttpMethod.Get,
                };
                request.Headers.Add("surpathkey", this.__SurpathKey);
                var response = await http.SendAsync(request);

                this.JsonResult = await response.Content.ReadAsStringAsync();
                ApiIntegrationResult data = JsonConvert.DeserializeObject<ApiIntegrationResult>(this.JsonResult);
                return data.success;
            }
            catch (Exception ex)
            {
                // return false;
                throw;
            }
        }

        /// <summary>
        /// Get integration settings
        /// </summary>
        /// <returns></returns>
        public async Task<IntegrationPartnerDTO> getSettings()
        {
            try
            {
                var method = "settings";

                var http = new HttpClient();
                var url = await BuildURI(method);
                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(url),
                    Method = HttpMethod.Get,
                };
                request.Headers.Add("surpathkey", this.__SurpathKey);
                var response = await http.SendAsync(request);

                this.JsonResult = await response.Content.ReadAsStringAsync();
                IntegrationPartnerDTO data = JsonConvert.DeserializeObject<IntegrationPartnerDTO>(this.JsonResult);
                return data;
            }
            catch (Exception ex)
            {
                // return false;
                throw;
            }
        }

        /// <summary>
        /// Save settings
        /// </summary>
        /// <param name="integrationPartnerDTO"></param>
        /// <returns></returns>
        public async Task<bool> postSettings(IntegrationPartnerDTO integrationPartnerDTO)
        {
            try
            {
                var method = "settings";

                var _payload = new JavaScriptSerializer().Serialize(integrationPartnerDTO);
                var buffer = System.Text.Encoding.UTF8.GetBytes(_payload);
                ByteArrayContent byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                byteContent.Headers.Add("surpathkey", this.__SurpathKey);

                var http = new HttpClient();
                var url = await BuildURI(method);
                var response = await http.PostAsync(url, byteContent);

                this.JsonResult = await response.Content.ReadAsStringAsync();
                ApiIntegrationResult data = JsonConvert.DeserializeObject<ApiIntegrationResult>(this.JsonResult);
                return data.success;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// Get a list of integrated clients
        /// </summary>
        /// <returns></returns>
        public async Task<List<IntegrationPartnerClientDTO>> getclients()
        {
            try
            {
                List<IntegrationPartnerClientDTO> result = new List<IntegrationPartnerClientDTO>();
                var method = "clients";

                var http = new HttpClient();
                var url = await BuildURI(method);
                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(url),
                    Method = HttpMethod.Get,
                };
                request.Headers.Add("surpathkey", this.__SurpathKey);
                var response = await http.SendAsync(request);

                this.JsonResult = await response.Content.ReadAsStringAsync();
                IntegrationPartnerClientDTO data = JsonConvert.DeserializeObject<IntegrationPartnerClientDTO>(this.JsonResult);
                return result;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// Get a list of integrated clients
        /// </summary>
        /// <returns></returns>
        public async Task<bool> postclients(List<IntegrationPartnerClientDTO> clients)
        {
            try
            {
                var method = "clients";

                var _payload = new JavaScriptSerializer().Serialize(clients);
                var buffer = System.Text.Encoding.UTF8.GetBytes(_payload);
                ByteArrayContent byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                byteContent.Headers.Add("surpathkey", this.__SurpathKey);

                var http = new HttpClient();
                var url = await BuildURI(method);
                var response = await http.PostAsync(url, byteContent);

                this.JsonResult = await response.Content.ReadAsStringAsync();
                ApiIntegrationResult data = JsonConvert.DeserializeObject<ApiIntegrationResult>(this.JsonResult);
                return data.success;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// Get donors associated with the integration
        /// </summary>
        /// <param name="apiIntegrationFilter"></param>
        /// <returns></returns>
        public async Task<IntegrationDonors> getDonors(ApiIntegrationFilter apiIntegrationFilter = null)
        {
            try
            {
                string method = "donors";

                if (apiIntegrationFilter == null) apiIntegrationFilter = new ApiIntegrationFilter();

                var _payload = new JavaScriptSerializer().Serialize(apiIntegrationFilter);
                var buffer = System.Text.Encoding.UTF8.GetBytes(_payload);
                ByteArrayContent byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                byteContent.Headers.Add("surpathkey", this.__SurpathKey);

                var http = new HttpClient();
                var url = await BuildURI(method);
                var response = await http.PostAsync(url, byteContent);

                this.JsonResult = await response.Content.ReadAsStringAsync();
                IntegrationDonors data = JsonConvert.DeserializeObject<IntegrationDonors>(this.JsonResult);
                return data;
            }
            catch (Exception ex)
            {
                // return false;
                throw;
            }
        }

        /// <summary>
        /// Get donors with documents associated with the integration
        /// </summary>
        /// <param name="apiIntegrationFilter"></param>
        /// <returns></returns>
        public async Task<IntegrationDonors> getDonorDocuments(ApiIntegrationFilter apiIntegrationFilter = null)
        {
            try
            {
                string method = "donordoc";

                if (apiIntegrationFilter == null) apiIntegrationFilter = new ApiIntegrationFilter();

                var _payload = new JavaScriptSerializer().Serialize(apiIntegrationFilter);
                var buffer = System.Text.Encoding.UTF8.GetBytes(_payload);
                ByteArrayContent byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                byteContent.Headers.Add("surpathkey", this.__SurpathKey);

                var http = new HttpClient();
                var url = await BuildURI(method);
                var response = await http.PostAsync(url, byteContent);

                this.JsonResult = await response.Content.ReadAsStringAsync();
                IntegrationDonors data = JsonConvert.DeserializeObject<IntegrationDonors>(this.JsonResult);
                return data;
            }
            catch (Exception ex)
            {
                // return false;
                throw;
            }
        }

        public async Task<ApiIntegrationMatchResults> getDonorMatch(ApiIntegrationMatch apiIntegrationMatch = null)
        {
            try
            {
                string method = "donormatch";

                if (apiIntegrationMatch == null) apiIntegrationMatch = new ApiIntegrationMatch();

                if (string.IsNullOrEmpty(apiIntegrationMatch.partner_donor_id))
                {
                    throw new Exception("partner_donor_id is required");
                }

                var _payload = new JavaScriptSerializer().Serialize(apiIntegrationMatch);
                var buffer = System.Text.Encoding.UTF8.GetBytes(_payload);
                ByteArrayContent byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                byteContent.Headers.Add("surpathkey", this.__SurpathKey);

                var http = new HttpClient();
                var url = await BuildURI(method);
                var response = await http.PostAsync(url, byteContent);


                this.JsonResult = await response.Content.ReadAsStringAsync();
                ApiIntegrationMatchResults data = JsonConvert.DeserializeObject<ApiIntegrationMatchResults>(this.JsonResult);
                return data;
            }
            catch (Exception ex)
            {
                // return false;
                throw;
            }
        }

    }
}
