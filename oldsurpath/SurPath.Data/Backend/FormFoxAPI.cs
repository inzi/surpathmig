using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using SurPath.Enum;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace SurPath.Data.Backend
{
    #region FFMPSearch
    public class FFMPSearch
    {
        public ILogger _logger { get; set; }


        public string FormFoxUrl { get; set; } = "https://stage.formfox.com/api/";
        public string method { get; } = "FFMPSearch";
        public string FormFoxMethodUrl { get; set; } = "https://stage.formfox.com/api/FFMPSearch";
        public string username { get; set; } = "";
        public string password { get; set; } = "";

        public string usernameAlt { get; set; } = "";
        public string passwordAlt { get; set; } = "";

        public FFMPSearchPayload payload { get; set; } = new FFMPSearchPayload();

        public string ConnectionString { get; set; }
        public List<string> PriceTiers { get; set; } = new List<string>();
        public List<string> ExcludedTerms { get; set; } = new List<string>();
        public CultureInfo culture = new CultureInfo(ConfigurationManager.AppSettings["Culture"].ToString().Trim());
        public List<string> FormFoxServices { get; set; } = new List<string>() { "NDOTU" };
        //public int Radius { get; set; } = 30;

        public List<FFMPSites> Sites = new List<FFMPSites>();

        public bool WasAnIssue = false;
        public int FFMPSearchSearchType { get; set; } = 1;
        public FFMPSearch(ILogger __logger = null)
        {
            if (__logger == null)
            {
                this._logger = new LoggerConfiguration().CreateLogger();
            }
            else
            {
                this._logger = __logger;
                _logger.Debug($"FFMPSearch invoked");
            }

            this.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString().Trim();
            _logger.Debug($"constring");

            this.username = ConfigurationManager.AppSettings["FormFoxUsername"].ToString().Trim();
            _logger.Debug($"username");
            this.password = ConfigurationManager.AppSettings["FormFoxPassword"].ToString().Trim();
            _logger.Debug($"password");
            this.usernameAlt = ConfigurationManager.AppSettings["FormFoxUsernameAlt"].ToString().Trim();
            _logger.Debug($"usernameAlt");
            this.passwordAlt = ConfigurationManager.AppSettings["FormFoxPasswordAlt"].ToString().Trim();
            _logger.Debug($"passwordAlt");

            var _FormFoxPriceTiers = ConfigurationManager.AppSettings["FormFoxPriceTiers"].ToString().Trim();
            if (!(string.IsNullOrEmpty(_FormFoxPriceTiers)))
            {
                _FormFoxPriceTiers.Split(',').ToList<string>().ForEach(_pt =>
                this.PriceTiers.Add(_pt.ToUpper().Trim())
                );

            }
            _logger.Debug($"_FormFoxLabExcludedTerms");

            var _FormFoxLabExcludedTerms = ConfigurationManager.AppSettings["FormFoxLabExcludedTerms"].ToString().Trim();
            if (!(string.IsNullOrEmpty(_FormFoxLabExcludedTerms)))
            {
                _FormFoxLabExcludedTerms.Split(',').ToList<string>().ForEach(_et =>
                this.ExcludedTerms.Add(_et.ToUpper().Trim())
                );

            }
            _logger.Debug($"_FormFoxPriceTiers");

            var _FormFoxServices = ConfigurationManager.AppSettings["FormFoxServices"].ToString().Trim();
            if (!(string.IsNullOrEmpty(_FormFoxServices)))
            {
                this.FormFoxServices = _FormFoxServices.Split(',').ToList<string>();
            }
            _logger.Debug($"_FormFoxServices");

            //var _FormFoxRadius = ConfigurationManager.AppSettings["FormFoxRadius"].ToString().Trim();
            //int.TryParse(_FormFoxRadius, out int __FormFoxRadius);
            //if (__FormFoxRadius > 0) this.Radius = __FormFoxRadius;
            //_logger.Debug($"_FormFoxRadius");

            this.FormFoxUrl = ConfigurationManager.AppSettings["FormFoxUrl"].ToString().Trim();
            if (!this.FormFoxUrl.EndsWith("/")) this.FormFoxUrl = this.FormFoxUrl + "/";
            this.FormFoxMethodUrl = this.FormFoxUrl + this.method;
            _logger.Debug($"FormFoxUrl {this.FormFoxUrl}");

            // FFMPSearchSearchType
            int _FFMPSearchSearchType = this.FFMPSearchSearchType;
            if (ConfigurationManager.AppSettings["FFMPSearchSearchType"] != null)
            {
                int.TryParse(ConfigurationManager.AppSettings["FFMPSearchSearchType"].ToString().Trim(), out _FFMPSearchSearchType);
            }
            this.FFMPSearchSearchType = _FFMPSearchSearchType;

            _logger.Debug("FFMPSearch ready");
        }


        public string GetAuthHeader()
        {
            string svcCredentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(username + ":" + password));
            svcCredentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(usernameAlt + ":" + passwordAlt));
            return "Basic " + svcCredentials;
        }

        public void AnalyzeSitesFromFormfox(string sitesString, ref List<FFMPSearchResult> __Sites, ref List<FFMPSearchResult> _Sites, ref SenderTracker sendertracker)
        {


            try
            {
                __Sites = JsonConvert.DeserializeObject<List<FFMPSearchResult>>(sitesString);

                _logInfo($"{__Sites.Count()} sites in results", ref sendertracker);

                sendertracker.AddData($"All locations returned Marketplace Enabled? {__Sites.TrueForAll(IsMarketplaceEnabled)}");
                sendertracker.AddData($"All locations returned A Distance? {__Sites.TrueForAll(DistanceIsNotNull)}");

                sendertracker.AddData($"Number of non-marketplace enabled locations returned: {__Sites.Where(f => f.MarketplaceEnabled == false).Count()}");
                sendertracker.AddData($"Number of locations with no name, address, or city returned: {__Sites.Where(f => HasValidData(f) == false).Count()}");
                sendertracker.AddData($"Number of locations with excluded keywords: {__Sites.Where(ContainsExcludedKeyword).Count()}");

                _logger.Debug($"All locations returned Marketplace Enabled? {__Sites.TrueForAll(IsMarketplaceEnabled)}");
                _logger.Debug($"All locations returned A Distance? {__Sites.TrueForAll(DistanceIsNotNull)}");
                _logger.Debug($"Number of non-marketplace enabled locations returned: {__Sites.Where(f => f.MarketplaceEnabled == false).Count()}");
                _logger.Debug($"Number of locations with no name, address, or city returned: {__Sites.Where(f => HasValidData(f) == false).Count()}");
                _logger.Debug($"Number of locations with excluded keywords: {__Sites.Where(ContainsExcludedKeyword).Count()}");

                sendertracker.AddData($"Removing all non-marketplace locations from results");
                _logger.Debug($"Removing all non-marketplace locations from results");
                __Sites.RemoveAll(IsNotMarketplaceEnabled);

                sendertracker.AddData($"Removing all locations without valid data");
                _logger.Debug($"Removing all locations without valid data");

                foreach (FFMPSearchResult f in __Sites.Where(f => HasValidData(f) == false).ToList())
                {
                    string _msg = $"Removing {f.Code} - has invalid data";
                    sendertracker.AddData(_msg);
                    _logger.Debug(_msg);
                }
                __Sites.RemoveAll(f => HasValidData(f) == false);
                sendertracker.AddData($"Removing all locations with excluded keywords");
                _logger.Debug($"Removing all locations with excluded keywords");

                __Sites.RemoveAll(ContainsExcludedKeyword);

                if (__Sites.TrueForAll(DistanceIsNotNull) == false)
                {
                    _logInfo("Sites with null distance in results!", ref sendertracker);
                    _logInfo($"{__Sites.Where(s => s.Distance < 0).ToList().Count()} sites returned NULL for distance", ref sendertracker);
                    var _nullDistanceSites = __Sites.Where(DistanceIsNull).ToList();

                    foreach (var __s in _nullDistanceSites)
                    {
                        _logInfo($"Location Code {__s.Code} {__s.Name} returned NULL for distance", ref sendertracker);
                        foreach (FFMPService _f in __s.Services)
                        {
                            _logInfo($"Code {_f.Code}, Rank {_f.Rank}, PriceTier {_f.PriceList}", ref sendertracker);
                        }

                    }
                    //var _nulldistancesitelist = string.Join(",", __Sites.Where(s => s.Distance < 0).Select(s => s.Code).ToArray());
                    //sendertracker.AddData($"Site Codes for locations with null distance: {_nulldistancesitelist}");
                    //__Sites = __Sites.Where(s => s.Distance >= 0).ToList();
                    __Sites.RemoveAll(DistanceIsNull);
                    
                }

            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
            }
            sendertracker.AddData("Received sites:");

            sendertracker.AddData(JsonConvert.SerializeObject(__Sites, Newtonsoft.Json.Formatting.Indented));
            _logger.Debug($"PullSites __Sites deserialized, {__Sites.Count} found");
            if (__Sites.Count > 0)
            {
                _logger.Debug($"PullSites setting _Sites to found results");

                _Sites = __Sites.Where(s => s.Services.Any(_s => _s.PriceList != "")).ToList();

            }
            else
            {
                _logger.Debug($"Empty result");

            }

             
        }

        private void _logInfo(string s, ref SenderTracker sendertracker)
        {
            sendertracker.AddData(s);
            _logger.Debug(s);
        }

        public List<FFMPSearchResult> PullSites(string Address1, string City, string State, string Zip, int __radius, ref SenderTracker sendertracker, string LabCode = "CRL")
        {

            _logger.Debug($"PullSites called - Radius = {__radius}");
            sendertracker.AddData("Pulling sites");
            try
            {

                payload.Address1 = Address1;
                payload.City = City;
                payload.State = State;
                payload.Zip = Zip;

                if (this.FormFoxServices.Count > 0)
                {
                    payload.Services = new List<string>();
                    foreach (string _s in this.FormFoxServices)
                    {
                        payload.Services.Add(_s);
                    }
                }

                //payload.Services.Add("NDOTU");
                //payload.Services.Add("BALC");
                payload.SearchRadius1 = __radius;
                payload.SearchRadius2 = __radius;

                payload.LabCode = "CRL";
                payload.PriceTier = new List<string>();
                foreach (string _pt in this.PriceTiers)
                {
                    payload.PriceTier.Add(_pt.ToUpper().Trim());
                }
                payload.SearchType = this.FFMPSearchSearchType;
                var objAsJson = new JavaScriptSerializer().Serialize(payload);

                StringContent stringContent = new StringContent(objAsJson);
                sendertracker.AddData(JsonConvert.SerializeObject(payload, Newtonsoft.Json.Formatting.Indented));
                List<FFMPSearchResult> _Sites;
                _logger.Debug("PullSites Initiating httpclient");
                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, new Uri(this.FormFoxMethodUrl));
                    request.Headers.Add("Authorization", GetAuthHeader());
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    request.Content = stringContent;
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    _logger.Debug("PullSites Initiating sending");
                    _logger.Debug($"{objAsJson}");
                    var response = client.SendAsync(request).GetAwaiter().GetResult();
                    var sitesString = response.Content.ReadAsStringAsync().Result;
                    _logger.Debug("PullSites Initiating sitestring retrieved");

                    _logger.Debug("sitesString:");
                    _logger.Debug($"{sitesString}");
                    sendertracker.AddData("");

                    _logger.Debug("end sitesString:");
                    _Sites = new List<FFMPSearchResult>();
                    var __Sites = new List<FFMPSearchResult>();
                    _logger.Debug("Deserializing sitesString:");
                    var _ValidJson = IsValidJson(sitesString);
                    if (_ValidJson.Item1 == true)
                    {
                        _logger.Debug("siteString is valid JSON");
                        try
                        {
                            JObject obj = JObject.Parse(sitesString);
                            if (_ValidJson.Item2.Equals(JTokenType.Array.ToString(), StringComparison.CurrentCultureIgnoreCase))
                            {
                                // Do Analysis:
                                AnalyzeSitesFromFormfox(sitesString, ref __Sites, ref _Sites, ref sendertracker);
                            }
                            else
                            {
                                _logger.Error("Return value not an array, see siteString value for details");
                            }
                        }
                        catch (Exception ex)
                        {
                            WasAnIssue = true;
                            _logger.Error("DeserializeObject failed! No Sites?");
                            _logger.Error(ex.ToString());
                            if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                            if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                        }

                    }
                    else
                    {
                        WasAnIssue = true;
                        _logger.Error($"FFMPSearch invalid JSON response");
                        _logger.Error($"{sitesString}");
                    }



                }
                this.Sites = new List<FFMPSites>();

                _logger.Debug($"PullSites getting sites for {this.PriceTiers.Count} price tiers");

                // go through the price tiers
                // and add a list of sites in that price tier ordered by distance
                if (this.PriceTiers.Count > 0)
                {
                    foreach (string _pt in this.PriceTiers)
                    {
                        _logger.Debug($"Checking Price Tier '{_pt.Trim()}'");
                        var _tempSites = _Sites.Where(s => s.Services.Any(_s => _s.PriceList.Equals(_pt.Trim(), StringComparison.InvariantCultureIgnoreCase) == true) && s.MarketplaceEnabled == true).ToList();
                        // sort our sites closest first.
                        _tempSites = _tempSites.OrderBy(s => s.Distance).ToList();
                        _tempSites = _tempSites.Where(s => s.Distance < __radius).ToList();
                        
                        this.Sites.Add(new FFMPSites() { PriceTier = _pt, SiteList = _tempSites });

                    }
                }
                else
                {
                    this.Sites.Add(new FFMPSites() { PriceTier = "", SiteList = _Sites });
                }

                _logger.Debug($"PullSites returning {_Sites.Count} sites");
                return _Sites;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                throw;
            }
        }

        private static bool DistanceIsNotNull(FFMPSearchResult f)
        {
            return f.Distance > -1;
        }

        private static bool DistanceIsNull(FFMPSearchResult f)
        {
            return f.Distance < 0;
        }
        private static bool IsMarketplaceEnabled(FFMPSearchResult f)
        {
            return f.MarketplaceEnabled == true;
        }
        private static bool IsNotMarketplaceEnabled(FFMPSearchResult f)
        {
            return f.MarketplaceEnabled == false;
        }

        private bool ContainsExcludedKeyword(FFMPSearchResult f)
        {
            bool retval = false;
            foreach (string _ex in ExcludedTerms)
            {
                bool _test = culture.CompareInfo.IndexOf(f.Name, _ex, CompareOptions.IgnoreCase) >= 0;
                if (retval == false && _test == true)
                {
                    retval = true;
                    break;
                }
            }

            return retval;
        }

        private static bool HasValidData(FFMPSearchResult f)
        {
            bool retval = true;

            if (string.IsNullOrEmpty(f.Name) || string.IsNullOrEmpty(f.Address1) || string.IsNullOrEmpty(f.City))
            {
                retval = false;
            }

            // return f.MarketplaceEnabled == false;
            return retval;
        }
        private static bool HasInValidData(FFMPSearchResult f)
        {
            bool retval = false;

            if (!(string.IsNullOrEmpty(f.Name)) || !(string.IsNullOrEmpty(f.Address1)) || !(string.IsNullOrEmpty(f.City)))
            {
                retval = true;
            }

            // return f.MarketplaceEnabled == false;
            return retval;
        }
        private static Tuple<bool,string> IsValidJson(string strInput)
        {
            if (string.IsNullOrWhiteSpace(strInput)) { return new Tuple<bool, string>(false, string.Empty); }
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
                (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = JToken.Parse(strInput);
                    return new Tuple<bool, string>(true, obj.Type.ToString());
                }
                catch (JsonReaderException jex)
                {
                    //Exception in parsing json
                    return new Tuple<bool, string>(false,string.Empty);
                }
                catch (Exception ex) //some other exception
                {
                    return new Tuple<bool, string>(false, string.Empty);
                }
            }
            else
            {
                return new Tuple<bool, string>(false, string.Empty);
            }
        }
        private static bool TryParseJSON(string json)
        {
            JObject jObject;
            try
            {
                jObject = JObject.Parse(json);
                return true;
            }
            catch
            {
                jObject = null;
                return false;
            }
        }
    }
    #endregion FFMPSearch

    #region CreateOrder
    public class FFCreateOrder
    {
        public bool WasAnIssue = false;
        public ILogger _logger { get; set; }
        public FFMPSearch fFMPSearch { get; set; }
        public string SendingFacility { get; set; }
        public string SendingFacilityTimeZone { get; set; }
        public string ClientReferenceID { get; set; }
        public string SendingFacilityID { get; set; }
        public PersonalData PersonalData { get; set; } = new PersonalData();
        public CreateOrderTest CreateOrderTest { get; set; } = new CreateOrderTest();
        public string username { get; set; } = "";
        public string password { get; set; } = "";
        private string FormFoxCreateOrderUrl { get; set; }
        public string ConnectionString { get; set; }
        public int FormFoxRetryCount { get; set; } = 2;
        public int FormFoxRetrySleepInMs { get; set; } = 200;
        public string ToXml(object obj)
        {
            string retval = null;
            if (obj != null)
            {
                StringBuilder sb = new StringBuilder();
                using (XmlWriter writer = XmlWriter.Create(sb, new XmlWriterSettings() { OmitXmlDeclaration = true }))
                {
                    new XmlSerializer(obj.GetType()).Serialize(writer, obj);
                }
                retval = sb.ToString();
            }
            return retval;
        }

        public FFCreateOrder(ILogger __logger = null)
        {
            if (__logger == null)
            {
                this._logger = new LoggerConfiguration().CreateLogger();
            }
            else
            {
                this._logger = __logger;
                _logger.Debug($"FFCreateOrder invoked");
            }
            this.FormFoxCreateOrderUrl = ConfigurationManager.AppSettings["FormFoxCreateOrderUrl"].ToString().Trim();
            this.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString().Trim();

            this.username = ConfigurationManager.AppSettings["FormFoxUsername"].ToString().Trim();

            this.password = ConfigurationManager.AppSettings["FormFoxPassword"].ToString().Trim();
            this.SendingFacility = ConfigurationManager.AppSettings["FormFoxSendingFacility"].ToString().Trim();
            this.SendingFacilityID = ConfigurationManager.AppSettings["FormFoxSendingFacilityID"].ToString().Trim();
            this.ClientReferenceID = ConfigurationManager.AppSettings["ClientReferenceID"].ToString().Trim();
            this.CreateOrderTest.SendingFacility = this.SendingFacility;
            this.CreateOrderTest.SendingFacilityTimeZone = "-5";

            this.fFMPSearch = new FFMPSearch(_logger);

            if (int.TryParse(ConfigurationManager.AppSettings["FormFoxRetryCount"].ToString().Trim(), out int _FormFoxRetryCount))
            {
                this.FormFoxRetryCount = _FormFoxRetryCount;
            }

            if (int.TryParse(ConfigurationManager.AppSettings["FormFoxRetrySleepInMs"].ToString().Trim(), out int _FormFoxRetrySleepInMs))
            {
                this.FormFoxRetrySleepInMs = _FormFoxRetrySleepInMs;
            }
            //this.CreateOrderTest.SendingFacilityID = this.SendingFacilityID;


            _logger.Debug($"FFCreateOrder Ready");

        }
        public HttpWebRequest CreateSOAPWebRequest()
        {
            //Making Web Request  
            HttpWebRequest Req = (HttpWebRequest)WebRequest.Create(this.FormFoxCreateOrderUrl);
            //SOAPAction  
            //Req.Headers.Add(@"SOAPAction:https://www.formfoxtest.com/v2/ffordersvc/CreateOrder");
            //Req.Headers.Add(@"Host:www.formfoxtest.com");
            //Content_type  
            Req.ContentType = "text/xml";
            Req.Accept = "*/*";
            //HTTP method  
            Req.Method = "POST";
            //return HttpWebRequest  
            return Req;
        }
        public Tuple<string, string, int> CreateOrder(SenderTracker senderTracker)
        {
            senderTracker.AddData($"Start of Create Order");
            WasAnIssue = false;
            int CreateOrderErrCode = 0;
            //StringBuilder ThisCreateOrderConversation = new StringBuilder();
            List<string> _locationsTried = new List<string>();
            FFMPSearchResult _site = new FFMPSearchResult();
            _logger.Debug("CreateOrder process");
            string ReferenceTestID = string.Empty;
            string MarketplaceOrderNumber = string.Empty;
            try
            {
                //ThisCreateOrderConversation = new StringBuilder();
                _logger.Debug($"CreateOrder starting");

                var _tries = this.FormFoxRetryCount;
                while (_tries > 0)
                {
                    senderTracker.AddData($"Creating an order - Try # {(this.FormFoxRetryCount - _tries) + 1}");

                    bool ffSiteFound = false;
                    string _pricetier = string.Empty;

                    // The sites are ordered by price tier then distance
                    // so we get the first site that's not in our list of tried locations
                    _logger.Debug($"Searching Price Tiers, excluding _locationsTried");

                    foreach (string _pt in fFMPSearch.PriceTiers)
                    {
                        if (ffSiteFound == true)
                        {
                            break;
                            //   continue;
                        }

                        _logger.Debug($"Checking price tier {_pt}");
                        // Get the site list for this price tier
                        if (fFMPSearch.Sites.Where(s => s.PriceTier.Equals(_pt, StringComparison.InvariantCultureIgnoreCase)).Count() > 0)
                        {
                            FFMPSites _ptFFMPSites = fFMPSearch.Sites.Where(s => s.PriceTier.Equals(_pt, StringComparison.InvariantCultureIgnoreCase)).First();

                            var ptSites = _ptFFMPSites.SiteList;
                            _logger.Debug($"{ptSites.Count} locations in {_pt} found");
                            _logger.Debug($"_locationsTried in {_pt} price tier: {_locationsTried.Where(_l => _l == _pt).Count()}");
                            foreach (var ptSite in ptSites)
                            {
                                if (ffSiteFound == true) continue;

                                _logger.Debug($"Evaluating ptSite: Code: {ptSite.Code}  Distance: {ptSite.Distance} Price Level: {_pt}");
                                var _tried = _locationsTried.Where(_l => _l == ptSite.Code).Count();
                                if (_tried == 0)
                                {

                                    _site = ptSite;
                                    ffSiteFound = true;
                                    _pricetier = _pt;
                                    _logger.Debug($"matched a site");
                                    senderTracker.AddData($"Matched site # {ptSite.Code}");

                                    break;
                                }
                                else
                                {

                                }

                            }

                        }
                        //List<FFMPSites> ptSites = fFMPSearch.Sites.Where(s => s.PriceTier.Equals(_pt, StringComparison.InvariantCultureIgnoreCase)).ToList();
                        //_logger.Debug($"{ptSites} locations in {_pt} found");
                        //_logger.Debug($"_locationsTried in {_pt} price tier: {_locationsTried.Where(_l => _l == _pt).Count()}");
                        //foreach(var ptSite in ptSites)
                        //{
                        //    _logger.Debug($"Evaluating ptSite: Code: {_site.Code}  Distance: {_site.Distance} Price Level: {_pricetier}");
                        //    var _tried = _locationsTried.Where(_l=>_l == ptSite.)
                        //    if (ptSite.SiteList.Exists(s => _locationsTried.Exists(_s => _s == s.Code) == false))
                        //    {

                        //        _site = ptSite.SiteList.Where(s => _locationsTried.Exists(_s => _s == s.Code)==false).First();
                        //        ffSiteFound = true;
                        //        _pricetier = _pt;
                        //        _logger.Debug($"matched a site");

                        //    }
                        //    else
                        //    {

                        //    }

                        //}

                    }

                    if (ffSiteFound == true)
                    {
                        // _site = fFMPSearch.Sites.Where(s => s.PriceTier.EndsWith(_pricetier, StringComparison.InvariantCultureIgnoreCase)).First().Closest();
                        CreateOrderTest.Services.CollectionSiteID = _site.Code;

                        senderTracker.AddData($"Site selected: {_site.Code}");
                        _logger.Debug($"_site: Code: {_site.Code}  Distance: {_site.Distance} Price Level: {_pricetier} Markplace Enabled? {_site.MarketplaceEnabled.ToString()}");
                        senderTracker.AddData($"Site Details: Code: {_site.Code}  Distance: {_site.Distance} Price Level: {_pricetier} Markplace Enabled? {_site.MarketplaceEnabled.ToString()}");
                        string orderXml = ToXml(CreateOrderTest);
                        _logger.Debug("orderXml");
                        _logger.Debug(orderXml);
                        // Our test facility ID is FF00095379
                        senderTracker.AddData("");
                        senderTracker.AddData("Order XML for this try");
                        senderTracker.AddData(FormattedXML(orderXml));
                        senderTracker.AddData("");

                        ReferenceTestID = string.Empty;
                        MarketplaceOrderNumber = string.Empty;

                        //Calling CreateSOAPWebRequest method  
                        HttpWebRequest request = CreateSOAPWebRequest();
                        _logger.Debug($"CreateOrder Soap Request created");
                        XmlDocument SOAPReqBody = new XmlDocument();

                        string soapEnvelope = $@"<?xml version=""1.0"" encoding=""utf-8""?>  
<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ffor=""https://www.formfox.com/v2/ffordersvc"">  
   <soapenv:Body>
      <ffor:CreateOrder>
         <ffor:Username>{this.username}</ffor:Username>
         <ffor:Password>{this.password}</ffor:Password>
         <ffor:OrderXML><![CDATA[{orderXml}]]></ffor:OrderXML>
      </ffor:CreateOrder>
   </soapenv:Body>
</soapenv:Envelope>";



                        //SOAP Body Request  
                        SOAPReqBody.LoadXml(soapEnvelope);
                        _logger.Debug($"CreateOrder Body Loaded");
                        _logger.Debug($"{soapEnvelope}");
                        _logger.Debug(request.Headers.ToString());
                        _logger.Debug($"Tries remaining: {_tries}");

                        senderTracker.AddData($"CreateOrder Body Loaded");
                        senderTracker.AddData(FormattedXML(soapEnvelope));
                        senderTracker.AddData($"");
                        senderTracker.AddData($"Headers:");
                        senderTracker.AddData(request.Headers.ToString());
                        senderTracker.AddData($"");

                        CookieContainer cookieJar = new CookieContainer();
                        request.CookieContainer = cookieJar;
                        using (Stream stream = request.GetRequestStream())
                        {
                            SOAPReqBody.Save(stream);
                        }
                        _logger.Debug($"CreateOrder envelope sent, getting response");
                        _logger.Debug($"Breaking open the cookieJar");
                        _logger.Debug("{cookie.Name} ; {cookie.Value} ; {cookie.Domain}");
                        foreach (var cookie in CookieContainerExtensions.List(cookieJar))
                        {
                            _logger.Debug($"{cookie.Name} ; {cookie.Value} ; {cookie.Domain}");
                        }
                        _logger.Debug(JsonConvert.SerializeObject(request, Newtonsoft.Json.Formatting.Indented));


                        //Geting response from request  
                        using (WebResponse Serviceres = request.GetResponse())
                        {
                            var ServiceResult = string.Empty;
                            using (StreamReader rd = new StreamReader(Serviceres.GetResponseStream()))
                            {
                                //reading stream  
                                ServiceResult = rd.ReadToEnd();
                            }
                            //writting stream result on console  
                            _logger.Debug($"Received response");
                            _logger.Debug(JsonConvert.SerializeObject(ServiceResult, Newtonsoft.Json.Formatting.Indented));
                            senderTracker.AddData("Received response");
                            senderTracker.AddData(FormattedXML(ServiceResult));
                            senderTracker.AddData($"");

                            //reading stream  
                            XmlSerializer OrderResultEnvelopeSerializer = new XmlSerializer(typeof(CreateOrderResultEnvelope));
                            XmlSerializer orderTestResultsSerializer = new XmlSerializer(typeof(OrderTestResults));

                            CreateOrderResultEnvelope OrderResultEnvelope;
                            using (TextReader reader = new StringReader(ServiceResult))
                            {

                                OrderResultEnvelope = (CreateOrderResultEnvelope)OrderResultEnvelopeSerializer.Deserialize(reader);
                            }
                            _logger.Debug($"CreateOrder response envelope received");

                            OrderTestResults orderTestResults;
                            string InnerServiceResult = OrderResultEnvelope.Body.CreateOrderResponse.CreateOrderResult;
                            StringWriter myWriter = new StringWriter();
                            _logger.Debug($"CreateOrder decoding InnerServiceResult");

                            // Decode the encoded string.
                            HttpUtility.HtmlDecode(InnerServiceResult, myWriter);
                            _logger.Debug($"CreateOrder deserializing orderTestResults");
                            using (TextReader reader = new StringReader(myWriter.ToString()))
                            {

                                orderTestResults = (OrderTestResults)orderTestResultsSerializer.Deserialize(reader);
                            }
                            string _responseText = myWriter.ToString();
                            _logger.Debug("InnerServiceResult");
                            _logger.Debug(FormattedXML(InnerServiceResult));
                            senderTracker.AddData("InnerServiceResult");
                            senderTracker.AddData(FormattedXML(InnerServiceResult));
                            senderTracker.AddData($"");

                            _logger.Debug($"CreateOrder orderTestResults Deserialized");
                            _logger.Debug($"orderTestResults: {orderTestResults}");
                            //if (orderTestResults.RequestStatus.Contains("accepted",StringComparison.InvariantCultureIgnoreCase))
                            if (orderTestResults.RequestStatus.IndexOf("accepted", StringComparison.InvariantCultureIgnoreCase) >= 0)
                            {
                                _logger.Debug($"CreateOrder call accepted {orderTestResults.ReferenceTestID}");
                                MarketplaceOrderNumber = orderTestResults.MarketplaceOrderNumber;

                                senderTracker.AddData("MarketplaceOrderNumber");
                                senderTracker.AddData($"{orderTestResults.MarketplaceOrderNumber}");

                                _logger.Debug($"MarketplaceOrderNumber: {MarketplaceOrderNumber}");
                                _logger.Debug($"This is try {_tries} out of {this.FormFoxRetryCount}");
                                if (!(string.IsNullOrEmpty(MarketplaceOrderNumber)))
                                {
                                    // If this value is not set, the marketplace refused the order

                                    ReferenceTestID = orderTestResults.ReferenceTestID;
                                    CreateOrderErrCode = 0;
                                    // Success! Stop trying.

                                    if (_tries == this.FormFoxRetryCount) _logger.Debug($"Success on first try");

                                    _tries = -5;
                                }
                                else
                                {

                                    CreateOrderErrCode = (int)NotificationClinicExceptions.FormFoxMarketplaceRejected;
                                    _logger.Debug($"This order was FormFoxMarketplaceRejected. CreateOrderErrCode now {CreateOrderErrCode}");
                                    senderTracker.AddData($"This order was FormFoxMarketplaceRejected. CreateOrderErrCode now {CreateOrderErrCode}");
                                }
                            }
                            else
                            {
                                _logger.Debug($"CreateOrder call NOT accepted!!");
                                _logger.Debug($"{_responseText}");
                                CreateOrderErrCode = (int)NotificationClinicExceptions.FormFoxOrderRejected;
                                senderTracker.AddData($"CreateOrder call NOT accepted!!");
                                _logger.Debug($"Order {orderTestResults.ReferenceTestID} needs to be canceled");

                                //                                if (true==false)
                                //                                {

                                //                                    HttpWebRequest cancelRequest = CreateSOAPWebRequest();
                                //                                    _logger.Debug($"CreateOrder Soap Request created");
                                //                                    XmlDocument SOAPCancelBody = new XmlDocument();

                                //                                    string soapCancelEnvelope = $@"<?xml version=""1.0"" encoding=""utf-8""?>  
                                //<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ffor=""https://www.formfox.com/v2/ffordersvc"">  
                                //   <soapenv:Body>
                                //      <ffor:RemoveTestResults>
                                //         <ffor:Username>{this.username}</ffor:Username>
                                //         <ffor:Password>{this.password}</ffor:Password>
                                //         <ffor:TestID></ffor:TestID>
                                //         <ffor:SendingFacility></ffor:SendingFacility>
                                //         <ffor:ProcessType></ffor:ProcessType>
                                //      </ffor:RemoveTestResults>
                                //   </soapenv:Body>
                                //</soapenv:Envelope>";
                                //                                }



                            }

                        }
                    }

                    _tries = _tries - 1;
                    if (_tries > -5)
                    {
                        // Add this to a list of tried locations

                        _logger.Debug($"CreateOrderTest.Services.CollectionSiteID added to _locationsTried");

                        _locationsTried.Add(CreateOrderTest.Services.CollectionSiteID);
                        WasAnIssue = true;
                    }

                    if (_tries > 0)
                    {
                        // we're going to try again, so sleep a little bit
                        _logger.Debug("Trying again...");
                        Thread.Sleep(this.FormFoxRetrySleepInMs);
                        senderTracker.AddData("Trying again...");
                    }
                    else
                    {
                        if (_tries > -5)
                        {
                            _logger.Debug($"Giving up - Failure - exhausted FormFoxRetryCount setting from config file. Tries at {_tries}. -5 == success");
                            _logger.Debug($"CreateOrderErrCode: {CreateOrderErrCode}");
                            CreateOrderErrCode = (int)NotificationClinicExceptions.FormFoxOrderGeneralFailure;
                            WasAnIssue = true;
                            ReferenceTestID = string.Empty;
                            MarketplaceOrderNumber = string.Empty;

                        }
                        else
                        {
                            _logger.Debug($"_tries = {_tries} - if -5 or lower, success");
                        }

                    }

                }
                _logger.Debug("Tries complete");

                senderTracker.AddData("");
                senderTracker.AddData("");
                if (CreateOrderErrCode != 0)
                    senderTracker.AddData($"Error: {((NotificationClinicExceptions)CreateOrderErrCode).ToString()}");
                senderTracker.AddData("returning:");
                senderTracker.AddData($"Test Id: '{ReferenceTestID.ToString()}'");
                senderTracker.AddData($"MP Id: '{MarketplaceOrderNumber.ToString()}'");
                senderTracker.AddData($"Error Code: '{CreateOrderErrCode}'");

                _logger.Debug("returning:");
                _logger.Debug($"Test Id: '{ReferenceTestID.ToString()}'");
                _logger.Debug($"MP Id: '{MarketplaceOrderNumber.ToString()}'");
                _logger.Debug($"Error Code: '{CreateOrderErrCode}'");
                return new Tuple<string, string, int>(ReferenceTestID, MarketplaceOrderNumber, CreateOrderErrCode); ;

            }
            catch (Exception ex)
            {
                WasAnIssue = true;
                CreateOrderErrCode = (int)NotificationClinicExceptions.FormFoxOrderGeneralFailure;
                MarketplaceOrderNumber = string.Empty;
                ReferenceTestID = string.Empty;
                _logger.Error(ex.ToString());
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                throw;
            }
        }
        //public CreateOrderTest CreateOrderRequestFactory()
        //{

        //    // required for CreateOrder
        //    //Sending Facility
        //    //Process Type
        //    //Primary Donor ID
        //    //First Name
        //    //Last Name
        //    //WorkPhone or HomePhone  If there is an MRO associated with the account.
        //    //Valid Laboratory ID(for lab drug tests)
        //    //Sample Type
        //    //Valid laboratory account(for lab drug tests)
        //    //Complete / Valid < TestProcedure > section
        //    //Valid Unit Codes(for lab drug tests)
        //    //*Reason for Test



        //    //com.formfoxtest.www.FFOrderSvc x = new com.formfoxtest.www.FFOrderSvc();
        //    //CreateOrderTest createOrderTest = new CreateOrderTest();

        //    //createOrderTest.PersonalData = new PersonalData();
        //    //createOrderTest.PersonalData.PrimaryID = "1234567890";
        //    //createOrderTest.PersonalData.PrimaryIDType = "SSN";
        //    //createOrderTest.PersonalData.PersonName = new PersonName();
        //    //createOrderTest.PersonalData.PersonName.GivenName = "Chris";
        //    //createOrderTest.PersonalData.PersonName.FamilyName = "Norman";
        //    //createOrderTest.PersonalData.ContactMethod = new ContactMethod();
        //    //createOrderTest.PersonalData.ContactMethod.Telephone = new List<Telephone>();
        //    //createOrderTest.PersonalData.ContactMethod.Telephone.Add(new Telephone() { Type = "Mobile", FormattedNumber = "2148019441" });
        //    //createOrderTest.Services = new Services();
        //    //createOrderTest.Services.Service = new List<Service>();
        //    //createOrderTest.Services.Service.Add(new Service() { Type = "Drug", LaboratoryAccount = "adas" });
        //    //x.CreateOrderAsync("SRSC0216", "XR5_2bPcv9", createOrderTest.ToString());


        //    //return createOrderTest;

        //}
        //public static CookieCollection GetAllCookies(CookieContainer cookieJar)
        //{
        //    CookieCollection cookieCollection = new CookieCollection();

        //    Hashtable table = (Hashtable)cookieJar.GetType().InvokeMember("m_domainTable",
        //                                                                    BindingFlags.NonPublic |
        //                                                                    BindingFlags.GetField |
        //                                                                    BindingFlags.Instance,
        //                                                                    null,
        //                                                                    cookieJar,
        //                                                                    new object[] { });

        //    foreach (var tableKey in table.Keys)
        //    {
        //        String str_tableKey = (string)tableKey;

        //        if (str_tableKey[0] == '.')
        //        {
        //            str_tableKey = str_tableKey.Substring(1);
        //        }

        //        SortedList list = (SortedList)table[tableKey].GetType().InvokeMember("m_list",
        //                                                                    BindingFlags.NonPublic |
        //                                                                    BindingFlags.GetField |
        //                                                                    BindingFlags.Instance,
        //                                                                    null,
        //                                                                    table[tableKey],
        //                                                                    new object[] { });

        //        foreach (var listKey in list.Keys)
        //        {
        //            String url = "https://" + str_tableKey + (string)listKey;
        //            cookieCollection.Add(cookieJar.GetCookies(new Uri(url)));
        //        }
        //    }

        //    return cookieCollection;
        //}
        // Format the XML text.
        private string FormattedXML(string rawxml)
        {
            try
            {
                XmlDocument xml_document = new XmlDocument();
                xml_document.LoadXml(rawxml);
                StringWriter string_writer = new StringWriter();
                XmlTextWriter xml_text_writer = new XmlTextWriter(string_writer);
                xml_text_writer.Formatting = System.Xml.Formatting.Indented;
                xml_document.WriteTo(xml_text_writer);

                // Display the result.
                return string_writer.ToString();
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to format XML");
                _logger.Error(ex.ToString());
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                return rawxml;
            }

        }
    }


    #endregion CreateOrder


    #region RequestAuthorization
    public class FFRequestAuthorization
    {

        public string FormFoxUrl { get; set; } = "https://stage.formfox.com/api/";
        public string method { get; } = "FFMPSearch";
        public string FormFoxMethodUrl { get; set; } = "https://stage.formfox.com/api/FFMPSearch";
        public string FormFoxRequestAuthorizationUrl { get; set; }
        public string username { get; set; } = "";
        public string password { get; set; } = "";

        public string FormFoxPDFFolder { get; set; }
        public string PDFBase64 = string.Empty;
        public ILogger _logger { get; set; }

        public FFRequestAuthorization(ILogger __logger = null)
        {
            if (__logger == null)
            {
                this._logger = new LoggerConfiguration().CreateLogger();
            }
            else
            {
                this._logger = __logger;
                _logger.Debug($"FFRequestAuthorization invoked");
            }

            this.username = ConfigurationManager.AppSettings["FormFoxUsername"].ToString().Trim();

            this.password = ConfigurationManager.AppSettings["FormFoxPassword"].ToString().Trim();

            this.FormFoxPDFFolder = ConfigurationManager.AppSettings["FormFoxPDFFolder"].ToString().Trim();

            this.FormFoxRequestAuthorizationUrl = ConfigurationManager.AppSettings["FormFoxRequestAuthorizationUrl"].ToString().Trim();
        }


        public string GetPDF(string ReferenceTestID, SenderTracker senderTracker)
        {
            _logger.Debug($"FFRequestAuthorization GetPDF for {ReferenceTestID}");

            string _payload = $@"<?xml version=""1.0"" encoding=""UTF-8""?>
<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ffor=""https://www.formfox.com/ffordersvc"">
   <soapenv:Header/>
   <soapenv:Body>
      <ffor:RequestAuthorization>
         <ffor:Username>{this.username}</ffor:Username>
         <ffor:Password>{this.password}</ffor:Password>
         <!--Optional:-->
         <ffor:ReferenceTestID>{ReferenceTestID}</ffor:ReferenceTestID>
         <!--Optional:-->
         <ffor:ClientReferenceID></ffor:ClientReferenceID>
      </ffor:RequestAuthorization>
   </soapenv:Body>
</soapenv:Envelope>";

            XmlDocument soapEnvelopeDocument = new XmlDocument();

            soapEnvelopeDocument.LoadXml(_payload);
            //var length = soapEnvelopeDocument.ToString().Length;

            _logger.Debug($"Sending RequstAuthorization Envelope:");
            _logger.Debug($"{_payload}");

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(this.FormFoxRequestAuthorizationUrl);
            webRequest.Headers.Add("SOAPAction", "https://www.formfox.com/ffordersvc/RequestAuthorization");
            //webRequest.ContentLength = length;
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            _logger.Debug($"Sending RequstAuthorization");
            senderTracker.AddData("Requesting PDF");
            using (Stream stream = webRequest.GetRequestStream())
            {
                soapEnvelopeDocument.Save(stream);
            }
            // begin async call to web request.
            IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);

            // suspend this thread until call is complete. You might want to
            // do something usefull here like update your UI.
            asyncResult.AsyncWaitHandle.WaitOne();

            // get the response from the completed web request.
            string soapResult;
            using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
            {
                using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                {
                    soapResult = rd.ReadToEnd();
                }
                //Console.Write(soapResult);
            }

            XDocument soapResultXDoc = XDocument.Load(new StringReader(soapResult));
            XNamespace soap = "http://schemas.xmlsoap.org/soap/envelope/";
            XNamespace ffns = "https://www.formfox.com/ffordersvc";
            var responseXml = soapResultXDoc.Element(soap + "Envelope").Element(soap + "Body")
                                  .Element(ffns + "RequestAuthorizationResponse").Element(ffns + "RequestAuthorizationResult");

            var _RequestStatus = responseXml.Descendants("RequestStatus").First().Value;
            var _ReferenceTestID = responseXml.Descendants("ReferenceTestID").First().Value;
            var _ClientReferenceID = responseXml.Descendants("ClientReferenceID").First().Value;
            var _ImagesData = responseXml.Descendants("ImagesData").First().Descendants("ImageData").First().Value;
            //byte[] bytes = Convert.FromBase64String(_ImagesData);
            this.FormFoxPDFFolder = (this.FormFoxPDFFolder.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar);
            string filename = $"{FormFoxPDFFolder}{_ReferenceTestID}.pdf";
            senderTracker.AddData($"PDF assigned filename {filename}");
            this.PDFBase64 = _ImagesData;


            using (System.IO.FileStream stream = System.IO.File.Create(filename))
            {
                System.Byte[] byteArray = System.Convert.FromBase64String(_ImagesData);
                stream.Write(byteArray, 0, byteArray.Length);
            }
            senderTracker.AddData($"PDF {filename} saved");
            return filename;
        }

        public void LoadPDFFromFile(string _ReferenceTestID)
        {
            string _FormFoxPDFFolder = (this.FormFoxPDFFolder.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar);
            string filename = $"{_FormFoxPDFFolder}{_ReferenceTestID}.pdf";

            byte[] _fileBytes = File.ReadAllBytes(filename);
        }
    }
    #endregion RequestAuthorization



    #region inboundclasses
    /* 
 Licensed under the Apache License, Version 2.0

 http://www.apache.org/licenses/LICENSE-2.0
 */

    //[XmlRoot(ElementName = "PersonName", Namespace = "https://FormFox.com")]
    //public class PersonName
    //{
    //    [XmlElement(ElementName = "GivenName", Namespace = "https://FormFox.com")]
    //    public string GivenName { get; set; }
    //    [XmlElement(ElementName = "MiddleName", Namespace = "https://FormFox.com")]
    //    public string MiddleName { get; set; }
    //    [XmlElement(ElementName = "FamilyName", Namespace = "https://FormFox.com")]
    //    public string FamilyName { get; set; }
    //}

    //[XmlRoot(ElementName = "Gender", Namespace = "https://FormFox.com")]
    //public class Gender
    //{
    //    [XmlElement(ElementName = "IdValue", Namespace = "https://FormFox.com")]
    //    public string IdValue { get; set; }
    //}

    //[XmlRoot(ElementName = "Telephone", Namespace = "https://FormFox.com")]
    //public class Telephone
    //{
    //    [XmlElement(ElementName = "FormattedNumber", Namespace = "https://FormFox.com")]
    //    public string FormattedNumber { get; set; }
    //    [XmlAttribute(AttributeName = "type")]
    //    public string Type { get; set; }
    //    [XmlElement(ElementName = "IdValue", Namespace = "https://FormFox.com")]
    //    public string IdValue { get; set; }
    //}

    //[XmlRoot(ElementName = "ContactMethod", Namespace = "https://FormFox.com")]
    //public class ContactMethod
    //{
    //    [XmlElement(ElementName = "Telephone", Namespace = "https://FormFox.com")]
    //    public List<Telephone> Telephone { get; set; }
    //}

    //[XmlRoot(ElementName = "DeliveryAddress", Namespace = "https://FormFox.com")]
    //public class DeliveryAddress
    //{
    //    [XmlElement(ElementName = "AddressLine", Namespace = "https://FormFox.com")]
    //    public List<string> AddressLine { get; set; }
    //}

    //[XmlRoot(ElementName = "PostalAddress", Namespace = "https://FormFox.com")]
    //public class PostalAddress
    //{
    //    [XmlElement(ElementName = "PostalCode", Namespace = "https://FormFox.com")]
    //    public string PostalCode { get; set; }
    //    [XmlElement(ElementName = "Region", Namespace = "https://FormFox.com")]
    //    public string Region { get; set; }
    //    [XmlElement(ElementName = "Municipality", Namespace = "https://FormFox.com")]
    //    public string Municipality { get; set; }
    //    [XmlElement(ElementName = "DeliveryAddress", Namespace = "https://FormFox.com")]
    //    public DeliveryAddress DeliveryAddress { get; set; }
    //    [XmlElement(ElementName = "CountryCode", Namespace = "https://FormFox.com")]
    //    public string CountryCode { get; set; }
    //}

    //[XmlRoot(ElementName = "Company", Namespace = "https://FormFox.com")]
    //public class Company
    //{
    //    [XmlElement(ElementName = "IdValue", Namespace = "https://FormFox.com")]
    //    public string IdValue { get; set; }
    //    [XmlElement(ElementName = "IdName", Namespace = "https://FormFox.com")]
    //    public string IdName { get; set; }
    //}

    //[XmlRoot(ElementName = "Location", Namespace = "https://FormFox.com")]
    //public class Location
    //{
    //    [XmlElement(ElementName = "IdValue", Namespace = "https://FormFox.com")]
    //    public string IdValue { get; set; }
    //    [XmlElement(ElementName = "IdName", Namespace = "https://FormFox.com")]
    //    public string IdName { get; set; }
    //}

    //[XmlRoot(ElementName = "DemographicDetail", Namespace = "https://FormFox.com")]
    //public class DemographicDetail
    //{
    //    [XmlElement(ElementName = "Company", Namespace = "https://FormFox.com")]
    //    public Company Company { get; set; }
    //    [XmlElement(ElementName = "Location", Namespace = "https://FormFox.com")]
    //    public Location Location { get; set; }
    //    [XmlElement(ElementName = "User1", Namespace = "https://FormFox.com")]
    //    public string User1 { get; set; }
    //    [XmlElement(ElementName = "User2", Namespace = "https://FormFox.com")]
    //    public string User2 { get; set; }
    //    [XmlElement(ElementName = "User3", Namespace = "https://FormFox.com")]
    //    public string User3 { get; set; }
    //    [XmlElement(ElementName = "User4", Namespace = "https://FormFox.com")]
    //    public string User4 { get; set; }
    //}


    //[XmlRoot(ElementName = "PersonalData", Namespace = "https://FormFox.com")]
    //public class PersonalData
    //{
    //    [XmlElement(ElementName = "PrimaryId", Namespace = "https://FormFox.com")]
    //    public string PrimaryId { get; set; }
    //    [XmlElement(ElementName = "PrimaryIdType", Namespace = "https://FormFox.com")]
    //    public string PrimaryIdType { get; set; }
    //    [XmlElement(ElementName = "AlternateID", Namespace = "https://FormFox.com")]
    //    public string AlternateID { get; set; }
    //    [XmlElement(ElementName = "AltIDType", Namespace = "https://FormFox.com")]
    //    public string AltIDType { get; set; }
    //    [XmlElement(ElementName = "PersonName", Namespace = "https://FormFox.com")]
    //    public PersonName PersonName { get; set; }
    //    [XmlElement(ElementName = "Gender", Namespace = "https://FormFox.com")]
    //    public Gender Gender { get; set; }
    //    [XmlElement(ElementName = "DateofBirth", Namespace = "https://FormFox.com")]
    //    public string DateofBirth { get; set; }
    //    [XmlElement(ElementName = "ContactMethod", Namespace = "https://FormFox.com")]
    //    public ContactMethod ContactMethod { get; set; }
    //    [XmlElement(ElementName = "PostalAddress", Namespace = "https://FormFox.com")]
    //    public PostalAddress PostalAddress { get; set; }
    //    [XmlElement(ElementName = "DemographicDetail", Namespace = "https://FormFox.com")]
    //    public DemographicDetail DemographicDetail { get; set; }
    //}

    //[XmlRoot(ElementName = "CollectionSite", Namespace = "https://FormFox.com")]
    //public class CollectionSite
    //{
    //    [XmlElement(ElementName = "SiteID", Namespace = "https://FormFox.com")]
    //    public string SiteID { get; set; }
    //    [XmlElement(ElementName = "FFSiteCode", Namespace = "https://FormFox.com")]
    //    public string FFSiteCode { get; set; }
    //    [XmlElement(ElementName = "CollectorName", Namespace = "https://FormFox.com")]
    //    public string CollectorName { get; set; }
    //    [XmlElement(ElementName = "CollectorID", Namespace = "https://FormFox.com")]
    //    public string CollectorID { get; set; }
    //    [XmlElement(ElementName = "SiteName", Namespace = "https://FormFox.com")]
    //    public string SiteName { get; set; }
    //    [XmlElement(ElementName = "SiteTimeZone", Namespace = "https://FormFox.com")]
    //    public string SiteTimeZone { get; set; }
    //    [XmlElement(ElementName = "PostalAddress", Namespace = "https://FormFox.com")]
    //    public PostalAddress PostalAddress { get; set; }
    //    [XmlElement(ElementName = "Telephone", Namespace = "https://FormFox.com")]
    //    public Telephone Telephone { get; set; }
    //}

    //[XmlRoot(ElementName = "ReasonForTest", Namespace = "https://FormFox.com")]
    //public class ReasonForTest
    //{
    //    [XmlElement(ElementName = "IdValue", Namespace = "https://FormFox.com")]
    //    public string IdValue { get; set; }
    //    [XmlElement(ElementName = "IdName", Namespace = "https://FormFox.com")]
    //    public string IdName { get; set; }
    //}



    //[XmlRoot(ElementName = "TestProcedure", Namespace = "https://FormFox.com")]
    //public class TestProcedure
    //{
    //    [XmlElement(ElementName = "IdSampleType", Namespace = "https://FormFox.com")]
    //    public string IdSampleType { get; set; }
    //    [XmlElement(ElementName = "IdTestMethod", Namespace = "https://FormFox.com")]
    //    public string IdTestMethod { get; set; }
    //}

    //[XmlRoot(ElementName = "UnitCodes", Namespace = "https://FormFox.com")]
    //public class UnitCodes
    //{
    //    [XmlElement(ElementName = "IdValue", Namespace = "https://FormFox.com")]
    //    public string IdValue { get; set; }
    //}

    //[XmlRoot(ElementName = "ImageData", Namespace = "https://FormFox.com")]
    //public class ImageDataElement
    //{
    //    [XmlElement(ElementName = "ImageName", Namespace = "https://FormFox.com")]
    //    public string ImageName { get; set; }
    //    [XmlElement(ElementName = "ImageData", Namespace = "https://FormFox.com")]
    //    public string ImageData { get; set; }
    //}

    //[XmlRoot(ElementName = "ImagesData", Namespace = "https://FormFox.com")]
    //public class ImagesData
    //{
    //    [XmlElement(ElementName = "ImageData", Namespace = "https://FormFox.com")]
    //    public ImageDataElement ImageDatas { get; set; }
    //}

    //[XmlRoot(ElementName = "Screening", Namespace = "https://FormFox.com")]
    //public class Screening
    //{
    //    [XmlElement(ElementName = "DOTTest", Namespace = "https://FormFox.com")]
    //    public string DOTTest { get; set; }
    //    [XmlElement(ElementName = "TestingAuthority", Namespace = "https://FormFox.com")]
    //    public string TestingAuthority { get; set; }
    //    [XmlElement(ElementName = "RequestSplitSample", Namespace = "https://FormFox.com")]
    //    public string RequestSplitSample { get; set; }
    //    [XmlElement(ElementName = "RequestObservation", Namespace = "https://FormFox.com")]
    //    public string RequestObservation { get; set; }
    //    [XmlElement(ElementName = "OrderCommentsToCollector", Namespace = "https://FormFox.com")]
    //    public string OrderCommentsToCollector { get; set; }
    //    [XmlElement(ElementName = "TestProcedure", Namespace = "https://FormFox.com")]
    //    public TestProcedure TestProcedure { get; set; }
    //    [XmlElement(ElementName = "UnitCodes", Namespace = "https://FormFox.com")]
    //    public UnitCodes UnitCodes { get; set; }
    //    [XmlElement(ElementName = "LaboratoryID", Namespace = "https://FormFox.com")]
    //    public string LaboratoryID { get; set; }
    //    [XmlElement(ElementName = "LaboratoryAccount", Namespace = "https://FormFox.com")]
    //    public string LaboratoryAccount { get; set; }
    //    [XmlElement(ElementName = "VerifiedBy", Namespace = "https://FormFox.com")]
    //    public string VerifiedBy { get; set; }
    //    [XmlElement(ElementName = "DateCollected", Namespace = "https://FormFox.com")]
    //    public string DateCollected { get; set; }
    //    [XmlElement(ElementName = "CollectionStatus", Namespace = "https://FormFox.com")]
    //    public string CollectionStatus { get; set; }
    //    [XmlElement(ElementName = "SpecimenID", Namespace = "https://FormFox.com")]
    //    public string SpecimenID { get; set; }
    //    [XmlElement(ElementName = "FormNumber", Namespace = "https://FormFox.com")]
    //    public string FormNumber { get; set; }
    //    [XmlElement(ElementName = "SplitSample", Namespace = "https://FormFox.com")]
    //    public string SplitSample { get; set; }
    //    [XmlElement(ElementName = "Observed", Namespace = "https://FormFox.com")]
    //    public string Observed { get; set; }
    //    [XmlElement(ElementName = "ScreenResult", Namespace = "https://FormFox.com")]
    //    public string ScreenResult { get; set; }
    //    [XmlElement(ElementName = "TempInRange", Namespace = "https://FormFox.com")]
    //    public string TempInRange { get; set; }
    //    [XmlElement(ElementName = "CollectorComments", Namespace = "https://FormFox.com")]
    //    public string CollectorComments { get; set; }
    //    [XmlElement(ElementName = "ImagesData", Namespace = "https://FormFox.com")]
    //    public ImagesData ImagesData { get; set; }
    //    [XmlAttribute(AttributeName = "type")]
    //    public string Type { get; set; }
    //}

    //[XmlRoot(ElementName = "Screenings", Namespace = "https://FormFox.com")]
    //public class Screenings
    //{
    //    [XmlElement(ElementName = "WhoOrderedTest", Namespace = "https://FormFox.com")]
    //    public string WhoOrderedTest { get; set; }
    //    [XmlElement(ElementName = "DateOrdered", Namespace = "https://FormFox.com")]
    //    public string DateOrdered { get; set; }
    //    [XmlElement(ElementName = "ScheduledDate", Namespace = "https://FormFox.com")]
    //    public string ScheduledDate { get; set; }
    //    [XmlElement(ElementName = "CollectionSite", Namespace = "https://FormFox.com")]
    //    public CollectionSite CollectionSite { get; set; }
    //    [XmlElement(ElementName = "ReasonForTest", Namespace = "https://FormFox.com")]
    //    public ReasonForTest ReasonForTest { get; set; }
    //    [XmlElement(ElementName = "CSOs", Namespace = "https://FormFox.com")]
    //    public string CSOs { get; set; }
    //    [XmlElement(ElementName = "ArrivalDate", Namespace = "https://FormFox.com")]
    //    public string ArrivalDate { get; set; }
    //    [XmlElement(ElementName = "BillingParty", Namespace = "https://FormFox.com")]
    //    public string BillingParty { get; set; }
    //    [XmlElement(ElementName = "Screening", Namespace = "https://FormFox.com")]
    //    public Screening Screening { get; set; }
    //}

    //[XmlRoot(ElementName = "ResultOrderResults", Namespace = "https://FormFox.com")]
    //public class ResultOrderResults
    //{
    //    [XmlElement(ElementName = "SendingFacility", Namespace = "https://FormFox.com")]
    //    public string SendingFacility { get; set; }
    //    [XmlElement(ElementName = "SendingFacilityTimeZone", Namespace = "https://FormFox.com")]
    //    public string SendingFacilityTimeZone { get; set; }
    //    [XmlElement(ElementName = "ClientReferenceID", Namespace = "https://FormFox.com")]
    //    public string ClientReferenceID { get; set; }
    //    [XmlElement(ElementName = "MailID", Namespace = "https://FormFox.com")]
    //    public string MailID { get; set; }
    //    [XmlElement(ElementName = "ReferenceTestID", Namespace = "https://FormFox.com")]
    //    public string ReferenceTestID { get; set; }
    //    [XmlElement(ElementName = "PersonalData", Namespace = "https://FormFox.com")]
    //    public PersonalData PersonalData { get; set; }
    //    [XmlElement(ElementName = "Screenings", Namespace = "https://FormFox.com")]
    //    public List<Screenings> Screenings { get; set; }
    //    [XmlAttribute(AttributeName = "xmlns")]
    //    public string Xmlns { get; set; }
    //}


    #endregion inboundclases

    public static class CookieContainerExtensions
    {
        public static List<Cookie> List(this CookieContainer container)
        {
            var cookies = new List<Cookie>();

            var table = (Hashtable)container.GetType().InvokeMember("m_domainTable",
                                                                    BindingFlags.NonPublic |
                                                                    BindingFlags.GetField |
                                                                    BindingFlags.Instance,
                                                                    null,
                                                                    container,
                                                                    new object[] { });

            foreach (var key in table.Keys)
            {
                var domain = key as string;

                if (domain == null)
                    continue;

                if (domain.StartsWith("."))
                    domain = domain.Substring(1);

                var httpAddress = string.Format("http://{0}/", domain);
                var httpsAddress = string.Format("https://{0}/", domain);

                if (Uri.TryCreate(httpAddress, UriKind.RelativeOrAbsolute, out var httpUri))
                {
                    foreach (Cookie cookie in container.GetCookies(httpUri))
                    {
                        cookies.Add(cookie);
                    }
                }
                if (Uri.TryCreate(httpsAddress, UriKind.RelativeOrAbsolute, out var httpsUri))
                {
                    foreach (Cookie cookie in container.GetCookies(httpsUri))
                    {
                        cookies.Add(cookie);
                    }
                }
            }

            return cookies;
        }
    }

}
