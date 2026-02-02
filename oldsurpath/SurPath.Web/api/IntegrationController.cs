using Newtonsoft.Json;
using Serilog;
using SurPath.Business;
using SurPath.Data;
using SurPath.Entity;
using SurpathBackend;
using SurPathWeb.api;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;

namespace SurPathWeb.Controllers
{
    //[RoutePrefix("api/Integration")]
    //[Area("Configuration")]
    //[Route("[area]/api/[controller]")]
    //[ApiController]
    //[Route("api")]
    [RoutePrefix("api/integration")]

    public class IntegrationController : ApiController
    {

        private APIIntegrationHelper IntegrationHelper = new APIIntegrationHelper();

        private IntegrationPartner integrationPartner = new IntegrationPartner();

        private BackendData d = new BackendData(null,null,MvcApplication._logger);
        private BackendLogic b = new BackendLogic(null,MvcApplication._logger);

        ILogger _logger = MvcApplication._logger;

        //public string Index()
        //{
        //    return "Integration API - Contact SurScan for details.";
        //}
        [HttpGet]
        [Route("echo")]
        public ApiIntegrationResult echo()
        {
            CheckSurpathKey();

            ApiIntegrationResult result = new ApiIntegrationResult();
            result.success = true;
            result.message = DateTime.Now.ToString();
            return result;
        }


        #region clients
        [HttpGet]
        [Route("clients")]
        public List<IntegrationPartnerClientDTO> clients()
        {
            CheckSurpathKey();
            IntegrationPartnerClientListHelper integrationPartnerClientListHelper = new IntegrationPartnerClientListHelper();
            

            //IntegrationPartner ip = d.GetIntegrationPartnerByPartnerKey(new ParamGetIntegrationPartnerByPartnerKey() { partner_key = this.integrationParter.partner_key });
            integrationPartnerClientListHelper.clients = d.GetIntegrationPartnerClientsByPartnerKey(new ParamGetIntegrationPartnerClientsByPartnerKey() { partner_key = this.integrationPartner.partner_key });
            integrationPartnerClientListHelper.clients = integrationPartnerClientListHelper.clients.Where(c => c.active == true).ToList();
            return integrationPartnerClientListHelper.clientsDTO;
        }
        
        /// <summary>
        /// Update clients with partner data
        /// </summary>
        /// <param name="clients"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("clients")]
        public ApiIntegrationResult clients(List<IntegrationPartnerClientDTO> clients)
        {
            ApiIntegrationResult result = new ApiIntegrationResult();

            CheckSurpathKey();

            try
            {

                IntegrationPartnerClientListHelper integrationPartnerClientListHelper = new IntegrationPartnerClientListHelper();
                integrationPartnerClientListHelper.clients = d.GetIntegrationPartnerClientsByPartnerKey(new ParamGetIntegrationPartnerClientsByPartnerKey() { partner_key = this.integrationPartner.partner_key });
                result.message = "No updates executed. Check GUID";
                foreach (IntegrationPartnerClientDTO cl in clients)
                {
                    if (integrationPartnerClientListHelper.clients.Exists(c => c.backend_integration_partner_client_map_GUID==cl.backend_integration_partner_client_map_GUID))
                    {
                        var _idx = integrationPartnerClientListHelper.clients.FindIndex(c => (c.backend_integration_partner_client_map_GUID == cl.backend_integration_partner_client_map_GUID));

                        integrationPartnerClientListHelper.clients.Where(c => (c.backend_integration_partner_client_map_GUID == cl.backend_integration_partner_client_map_GUID)).ToList().ForEach(c =>
                        {
                            c.partner_client_id = cl.partner_client_id;
                            c.partner_client_code = cl.partner_client_code;
                            c.partner_push_folder = cl.partner_push_folder;

                        }
                        );

                        // save the client
                        b.SetIntegrationPartnerClient(integrationPartnerClientListHelper.clients.Where(c => c.backend_integration_partner_client_map_GUID == cl.backend_integration_partner_client_map_GUID).First());
                        result.message = string.Empty;
                    }
                }
                result.success = true;
            }
            catch (Exception ex)
            {
                MvcApplication.LogError(ex);
                //throw;
            }

            return result;
        }
        #endregion clients

        #region settings
        [HttpGet]
        [Route("settings")]
        public IntegrationPartnerDTO settings()
        {
            string retval = string.Empty;
            CheckSurpathKey();
            IntegrationPartnerDTO ip = new IntegrationPartnerDTO();
            try
            {
                IntegrationPartner _ip = d.GetIntegrationPartnerByPartnerKey(new ParamGetIntegrationPartnerByPartnerKey() { partner_key = this.integrationPartner.partner_key });
                ip = _ip.integrationPartnerDTO;
            }
            catch (Exception ex)
            {
                MvcApplication.LogError(ex);
            }


            return ip;
        }

        [HttpPost]
        [Route("settings")]
        public ApiIntegrationResult settings([FromBody] IntegrationPartnerDTO _ip)
        {
            ApiIntegrationResult result = new ApiIntegrationResult();

            CheckSurpathKey();
            try
            {
                //IntegrationPartner ip = b.GetIntegrationPartnersById(integrationPartner.backend_integration_partner_id);
                IntegrationPartner ip = d.GetIntegrationPartnerByPartnerKey(new ParamGetIntegrationPartnerByPartnerKey() { partner_key = this.integrationPartner.partner_key });
                ip.integrationPartnerDTO = _ip;

                b.SetIntegrationPartners(ip);
                result.success = true;
            }
            catch (Exception ex)
            {
                MvcApplication.LogError(ex);
                throw;
            }
            return result;
        }
        #endregion settings

        #region donors
        [HttpPost]
        [Route("donors")]
        public IntegrationDonors donors([FromBody] ApiIntegrationFilter _filter)
        {

            CheckSurpathKey();
            
            IntegrationDonors result = new IntegrationDonors();
            if (_filter == null)
            {
                _filter = new ApiIntegrationFilter();
            }

            try
            {
                _filter = CleanApiIntegrationFilter(_filter);
                //IntegrationPartner ip = d.GetIntegrationPartnerByPartnerKey(new ParamGetIntegrationPartnerByPartnerKey() { partner_key = this.integrationParter.partner_key });
                result = d.GetIntegrationPartnerDonorsAndDocuments(new ParamGetIntegrationPartnerDonorsAndDocuments() { backend_integration_partner_id = this.integrationPartner.backend_integration_partner_id, apiIntegrationFilter=_filter });
            }
            catch (Exception ex)
            {
                MvcApplication.LogError(ex);
            }

            return result;
        }
        [HttpPost]
        [Route("donordoc")]
        public IntegrationDonors donordoc([FromBody] ApiIntegrationFilter _filter)
        {

            CheckSurpathKey();

            IntegrationDonors result = new IntegrationDonors();
            if (_filter == null)
            {
                _filter = new ApiIntegrationFilter();
            }
            if (!string.IsNullOrEmpty(_filter.partner_donor_id))
            {
                try
                {
                    _filter = CleanApiIntegrationFilter(_filter);
                    result = d.GetIntegrationPartnerDonorsAndDocuments(new ParamGetIntegrationPartnerDonorsAndDocuments() { backend_integration_partner_id = this.integrationPartner.backend_integration_partner_id, apiIntegrationFilter = _filter, WithBase64 = true });
                }
                catch (Exception ex)
                {
                    MvcApplication.LogError(ex);
                }
            }
           

            return result;
        }
       
        [HttpPost]
        [Route("donormatch")]
        public ApiIntegrationMatchResults donormatch([FromBody] ApiIntegrationMatch _match)
        {

            CheckSurpathKey();

            ApiIntegrationMatchResults result = new ApiIntegrationMatchResults();
            if (_match == null)
            {
                result.result = false;
                result.message = "No parameters provided";
                return result;
            }
            

            if (string.IsNullOrEmpty(_match.partner_donor_id))
            {
                result.result = false;
                result.message = "partner_donor_id (external id) is required";
                return result;
            }


            try
            {

                //IntegrationPartner ip = d.GetIntegrationPartnerByPartnerKey(new ParamGetIntegrationPartnerByPartnerKey() { partner_key = this.integrationParter.partner_key });
                result = d.GetDonorMatch(new ParamGetDonorMatch() { backend_integration_partner_id = this.integrationPartner.backend_integration_partner_id, apiIntegrationMatch = _match });
                
            }
            catch (Exception ex)
            {
                MvcApplication.LogError(ex);
            }
            result.apiIntegrationMatch = _match;
            return result;
        }


        [HttpPost]
        [Route("verify")]
        public ApiIntegrationMatchResults verify([FromBody] ApiIntegrationMatch _match)
        {

            CheckSurpathKey();

            ApiIntegrationMatchResults result = new ApiIntegrationMatchResults();
            if (_match == null)
            {
                result.result = false;
                result.message = "No parameters provided";
                return result;
            }


            if (string.IsNullOrEmpty(_match.partner_donor_id))
            {
                result.result = false;
                result.message = "partner_donor_id (external id) is required";
                return result;
            }


            try
            {

                //IntegrationPartner ip = d.GetIntegrationPartnerByPartnerKey(new ParamGetIntegrationPartnerByPartnerKey() { partner_key = this.integrationParter.partner_key });
                result = d.VerifyIntegrationPid(new ParamGetDonorMatch() { backend_integration_partner_id = this.integrationPartner.backend_integration_partner_id, apiIntegrationMatch = _match });

                if (result.exact_matchs.Count() != 1)
                {
                    result.result = false;
                    var msg = "Not Found";
                    if (result.exact_matchs.Count() > 1)
                    {
                        msg = "Multiple found, cannot verify identity";
                    }
                    result.message = msg;
                }
                else
                {
                    result.result = true;
                    DonorBL donorBL = new DonorBL();
                    donorBL._logger = MvcApplication._logger;
                    var exactMatch = result.exact_matchs.First();


                   Donor donor = donorBL.Get(exactMatch.donor_id, "web");

                    // donor.PidTypeValues.Select(p => p.PIDValue.Equals(exactMatch.partner_donor_id, StringComparison.InvariantCultureIgnoreCase)).First().
                    var idx = donor.PidTypeValues.FindIndex(p => p.PIDValue.Equals(exactMatch.partner_donor_id, StringComparison.InvariantCultureIgnoreCase));
                    donor.PidTypeValues[idx].validated = true;

                    donorBL.UpdateDonorPids(donor);
                        
                    var partner_donor_id = _match.partner_donor_id;

                }
                result.possible_matchs = new List<ApiIntegrationMatchResult>();

            }
            catch (Exception ex)
            {
                MvcApplication.LogError(ex);
            }
            result.apiIntegrationMatch = _match;
            return result;
        }



        #endregion donors


        #region SurpathKey

        private string CheckSurpathKey()
        {
            string headerValue = string.Empty;
            IEnumerable<string> headerValues;
            bool throwErr = false;
            this.integrationPartner = new IntegrationPartner();
            if (Request.Headers.Contains("SurpathKey") == false)
            {
                throwErr = true;
            }
            else
            {
                if (Request.Headers.TryGetValues("SurpathKey", out headerValues))
                {
                    headerValue = headerValues.First();
                    this.integrationPartner = d.GetIntegrationPartnerByPartnerKey(new ParamGetIntegrationPartnerByPartnerKey() { partner_key = headerValue });
                }
                else
                {
                    throwErr = true;
                }
            }
            if (throwErr == true)
            {
                var msg = new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "SurpathKey Header value invalid or missing" };
                throw new HttpResponseException(msg);
            }
            return headerValue;
        }

        private ApiIntegrationFilter CleanApiIntegrationFilter(ApiIntegrationFilter apiIntegrationFilter)
        {
            var _y2k = DateTime.Now.AddYears(-1 * (DateTime.Now.Year - 2000));
            if (apiIntegrationFilter.fromDateTime <  _y2k) apiIntegrationFilter.fromDateTime = _y2k;
            if (apiIntegrationFilter.toDateTime < _y2k) apiIntegrationFilter.toDateTime = DateTime.Now;
            if (string.IsNullOrEmpty(apiIntegrationFilter.partner_client_code)) apiIntegrationFilter.partner_client_code = string.Empty;
            if (string.IsNullOrEmpty(apiIntegrationFilter.partner_client_id)) apiIntegrationFilter.partner_client_id = string.Empty;
            if (string.IsNullOrEmpty(apiIntegrationFilter.partner_donor_id)) apiIntegrationFilter.partner_donor_id = string.Empty;
            //if (string.IsNullOrEmpty(apiIntegrationFilter.)) apiIntegrationFilter.partner_donor_id = string.Empty;
            return apiIntegrationFilter;
        }

        #endregion SurpathKey


        //[HttpGet]
        //[Route("client")]
        //public IntegrationPartnerClientDTO client(IntegrationPartnerClientDTO client)
        //{
        //    CheckSurpathKey();
        //    IntegrationPartnerClientListHelper integrationPartnerClientListHelper = new IntegrationPartnerClientListHelper();


        //    //IntegrationPartner ip = d.GetIntegrationPartnerByPartnerKey(new ParamGetIntegrationPartnerByPartnerKey() { partner_key = this.integrationParter.partner_key });
        //    IntegrationPartnerClient _client = d.GetIntegrationPartnerClientByPartnerClientId(new ParamGetIntegrationPartnerClientByPartnerClientId() { IntegrationPartnerClientDTO = client });

        //    return _client.integrationPartnerClientDTO;
        //}

        //[HttpPost]
        //[Route("client")]
        //public ApiIntegrationResult client(IntegrationPartnerClientDTO client)
        //{
        //    ApiIntegrationResult result = new ApiIntegrationResult();

        //    CheckSurpathKey();

        //    try
        //    {

        //        IntegrationPartnerClientListHelper integrationPartnerClientListHelper = new IntegrationPartnerClientListHelper();
        //        integrationPartnerClientListHelper.clients = d.GetIntegrationPartnerClientsByPartnerKey(new ParamGetIntegrationPartnerClientsByPartnerKey() { partner_key = this.integrationParter.partner_key });

        //        foreach (IntegrationPartnerClientDTO cl in clients)
        //        {
        //            if (integrationPartnerClientListHelper.clients.Exists(c => c.backend_integration_partner_client_map_GUID == cl.backend_integration_partner_client_map_GUID))
        //            {
        //                integrationPartnerClientListHelper.clients.Where(c => (c.backend_integration_partner_client_map_GUID == cl.backend_integration_partner_client_map_GUID)).ToList().ForEach(c =>
        //                {
        //                    c.partner_client_id = cl.partner_client_id;
        //                    c.partner_client_code = cl.partner_client_code;
        //                }
        //                );

        //                // save the client
        //                b.SetIntegrationPartnerClient(integrationPartnerClientListHelper.clients.Where(c => c.backend_integration_partner_client_map_GUID == cl.backend_integration_partner_client_map_GUID).First());
        //            }
        //        }
        //        result.success = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        MvcApplication.LogError(ex);
        //        //throw;
        //    }

        //    return result;
        //}

    }
}
