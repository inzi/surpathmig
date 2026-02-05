using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Web.Services.Protocols;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace fft
{
    [Serializable]
    public class FFMPSearchPayload
    {
        public string Address1 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public List<string> Services { get; set; } = new List<string>();
        public int SearchRadius1 { get; set; }
        public string LabCode { get; set; }
        public List<string> PriceTier { get; set; } = new List<string>();
        public int SearchType { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class FFMPService
    {
        public string Code { get; set; }
        public double? Rank { get; set; }
        public string PriceList { get; set; } = "";
    }

    public class FFMPSearchResult

    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public int MasterAccount { get; set; }
        public string TimeZone { get; set; }
        public int TZOffset { get; set; }
        public double Distance { get; set; }
        public bool FormFoxEnabled { get; set; }
        public bool MarketplaceEnabled { get; set; }
        public List<FFMPService> Services { get; set; }
        public List<object> Amenities { get; set; }
    }



    public class FFMPSearch
    {
        public string url { get; set; } = "https://stage.formfox.com/api/FFMPSearch";
        public string username { get; set; } = "SurScan";
        public string password { get; set; } = "9EwreV69uz";
        public FFMPSearchPayload payload { get; set; } = new FFMPSearchPayload();

        public string GetAuthHeader()
        {
            string svcCredentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(username + ":" + password));
            return "Basic " + svcCredentials;
        }

        public async Task<bool> invoke()
        {
            await PullSites();
            return true;

        }

        public async Task<List<FFMPSearchResult>> PullSites(string _priceLevel = null)
        {

            payload.Address1 = "1 Main St";
            payload.City = "Salt Lake City";
            payload.State = "UT";
            payload.Zip = "84115";
            payload.Services.Add("NDOTU");
            //payload.Services.Add("NDOTU");
            //payload.Services.Add("BALC");
            payload.SearchRadius1 = 30;
            payload.LabCode = "CRL";
            if (!(string.IsNullOrEmpty(_priceLevel)))
            {
                payload.PriceTier.Add(_priceLevel);
            }
            //payload.PriceTier.Add("Gold");
            //payload.PriceTier.Add("Silver");
            payload.SearchType = 2;
            var objAsJson = new JavaScriptSerializer().Serialize(payload);

            StringContent stringContent = new StringContent(objAsJson);

            List<FFMPSearchResult> _Sites;

            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Post, new Uri(url));
                request.Headers.Add("Authorization", GetAuthHeader());
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Content = stringContent;
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = await client.SendAsync(request);
                var sitesString = response.Content.ReadAsStringAsync().Result;
                _Sites = JsonConvert.DeserializeObject<List<FFMPSearchResult>>(sitesString);
                var __sites = _Sites.Where(s => s.Services.Any(_s => _s.PriceList != "")).ToList();
                var PlatSites = __sites.Where(s => s.Services.Any(_s => _s.PriceList.Equals("Platinum", StringComparison.InvariantCultureIgnoreCase) == true)).ToList();
            }
            return _Sites;
        }

        public async Task<bool> invokeLib()
        {
            ////////HttpWebRequest Req = (HttpWebRequest)WebRequest.Create(@"https://stage.formfox.com/api/FFMPSearch");
            //////////SOAPAction  
            ////////Req.Headers.Add(@"SOAPAction:https://www.formfoxtest.com/v2/ffordersvc/CreateOrder");
            ////////Req.Headers.Add(@"Host:www.formfoxtest.com");
            //////////Content_type  
            ////////Req.ContentType = "text/xml;charset=\"utf-8\"";
            ////////Req.Accept = "text/xml";

            payload.Address1 = "1 Main St";
            payload.City = "Salt Lake City";
            payload.State = "UT";
            payload.Zip = "84115";
            payload.Services.Add("DOTU");
            payload.Services.Add("NDOTU");
            payload.Services.Add("BALC");
            payload.SearchRadius1 = 30;
            payload.LabCode = "CRL";
            payload.PriceTier.Add("Platinum");
            //payload.PriceTier.Add("Gold");
            //payload.PriceTier.Add("Silver");
            payload.SearchType = 2;


            //var objAsJson = JsonConvert.SerializeObject(myObject);
            //////var content = new StringContent(objAsJson, Encoding.UTF8, "application/json");
            ////////content.Headers.Add("Authorization", "Basic " + GetAuthHeader());
            //////HttpClientHandler handler = new HttpClientHandler();
            //////handler.CookieContainer = new CookieContainer();
            //////handler.UseCookies = true;

            //////var _httpClient = new HttpClient(handler);
            //////_httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Basic " + GetAuthHeader());
            //////_httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //////_httpClient.DefaultRequestHeaders.Add("Cookie", "BIGipServerstage.formfox.com_pool=689143818.47873.0000; TS01949c93=01cc52c0ef3d46cc493e6c9e33b46cf0b0c79802ef52f7ffa40ed5845e5949159b2720f2750a14708ef8034e8fdbb2b70678ddb652bb0e60b12aaa766b2907c3787ed3241e");

            //////var result = await _httpClient.PostAsync(url, content); //or PostAsync for POST

            //////FFMPSearchResult myDeserializedClass = JsonConvert.DeserializeObject<FFMPSearchResult>(result.ToString());
            var objAsJson = new JavaScriptSerializer().Serialize(payload);

            var client = new RestClient(url);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            var authVal = "Basic " + GetAuthHeader();
            request.AddHeader("Authorization", GetAuthHeader());
            //request.AddHeader("Authorization", "Basic U3VyU2Nhbjo5RXdyZVY2OXV6");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", objAsJson, ParameterType.RequestBody);
            //request.AddParameter("application/json", "{\"Address1\":\"1 Main St\",\"City\":\"Salt Lake City\",\"State\":\"UT\",\"Zip\":\"84115\",\"Services\":[\"DOTU\",\"NDOTU\",\"BALC\"],\"SearchRadius1\":5,\"LabCode\":\"CRL\",\"PriceTier\":[\"Platinum\",\"Gold\", \"Silver\"],\"SearchType\":2}", ParameterType.RequestBody);
            //            request.AddParameter("application/json", "{\r\n    \"Address1\": \"1 Main St\",\r\n    \"City\": \"Salt Lake City\",\r\n    \"State\": \"UT\",\r\n    \"Zip\": \"84115\",\r\n    \"Services\": [\r\n        \"DOTU\",\r\n        \"NDOTU\",\r\n        \"BALC\"\r\n    ],\r\n    \"SearchRadius1\": 5,\r\n    \"LabCode\": \"CRL\",\r\n    \"PriceTier\":[\"Platinum\",\"Gold\", \"Silver\"],\r\n    \"SearchType\": 2\r\n}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            var contentJson = response.Content;
            List<FFMPSearchResult> PlatSites = JsonConvert.DeserializeObject<List<FFMPSearchResult>>(contentJson);

            payload.PriceTier = new List<string>() { "Gold" };
            objAsJson = new JavaScriptSerializer().Serialize(payload);

            client = new RestClient(url);
            request.Parameters.Clear();
            request.AddHeader("Authorization", GetAuthHeader());
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", objAsJson, ParameterType.RequestBody);
            client.ClearHandlers();
            client.CookieContainer = new CookieContainer();
            response = client.Execute(request);
            List<FFMPSearchResult> GoldSites = JsonConvert.DeserializeObject<List<FFMPSearchResult>>(contentJson);

            payload.PriceTier = new List<string>() { "Silver" };
            objAsJson = new JavaScriptSerializer().Serialize(payload);
            client = new RestClient(url);
            request.Parameters.Clear();
            request.AddHeader("Authorization", GetAuthHeader());
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", objAsJson, ParameterType.RequestBody);
            client.CookieContainer = new CookieContainer();
            response = client.Execute(request);

            List<FFMPSearchResult> SilverSites = JsonConvert.DeserializeObject<List<FFMPSearchResult>>(contentJson);


            //var GoldSites = myDeserializedClass.Where(r=>r.cl)

            Console.WriteLine(response.Content);

            var x = 1;
            return true;
        }

    }


    public class FormFoxRequestAuthorization
    {
        public void GetPDF(string ReferenceTestID)
        {
            string un = "SurScan";
            string pw = "9EwreV69uz";
            string rtid = "30726926";

            string _payload = $@"<?xml version=""1.0"" encoding=""UTF-8""?>
<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ffor=""https://www.formfox.com/ffordersvc"">
   <soapenv:Header/>
   <soapenv:Body>
      <ffor:RequestAuthorization>
         <ffor:Username>{un}</ffor:Username>
         <ffor:Password>{pw}</ffor:Password>
         <!--Optional:-->
         <ffor:ReferenceTestID>{rtid}</ffor:ReferenceTestID>
         <!--Optional:-->
         <ffor:ClientReferenceID></ffor:ClientReferenceID>
      </ffor:RequestAuthorization>
   </soapenv:Body>
</soapenv:Envelope>";

            XmlDocument soapEnvelopeDocument = new XmlDocument();
            soapEnvelopeDocument.LoadXml(_payload);
            var length = soapEnvelopeDocument.ToString().Length;

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("https://stage.formfox.com/FFOrderSvc/FFOrderSvc.asmx");
            webRequest.Headers.Add("SOAPAction", "https://www.formfox.com/ffordersvc/RequestAuthorization");
            //webRequest.ContentLength = length;
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
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
                Console.Write(soapResult);
            }
            //return webRequest;
            //XmlDocument soapResultXML = new XmlDocument();
            //soapResultXML.LoadXml(soapResult);
            //XDocument soapResultXML = XDocument.Load(new StringReader(soapResult));
            //XNamespace ns = "https://www.formfox.com/ffordersvc";
            //var soapBody = soapResultXML.GetElementsByTagName("soap:Body")[0];
            //var bodyXML = soapResultXML.Descendants(ns + "Body").First().Value;

            //XDocument soapResultBody = XDocument.Load(new StringReader(bodyXML));

            //var RequestAuthorizationResponse = soapResultXML.Descendants("RequestAuthorizationResponse").First().Value;

            XDocument soapResultXDoc = XDocument.Load(new StringReader(soapResult));
            XNamespace soap = "http://schemas.xmlsoap.org/soap/envelope/";
            XNamespace ffns = "https://www.formfox.com/ffordersvc";
            var responseXml = soapResultXDoc.Element(soap + "Envelope").Element(soap + "Body")
                                  .Element(ffns + "RequestAuthorizationResponse").Element(ffns+"RequestAuthorizationResult");

            var _RequestStatus = responseXml.Descendants("RequestStatus").First().Value;
            var _ReferenceTestID = responseXml.Descendants("ReferenceTestID").First().Value;
            var _ClientReferenceID = responseXml.Descendants("ClientReferenceID").First().Value;
            var _ImagesData = responseXml.Descendants("ImagesData").First().Descendants("ImageData").First().Value;
            //byte[] bytes = Convert.FromBase64String(_ImagesData);

            using (System.IO.FileStream stream = System.IO.File.Create($"c:\\temp\\{_ReferenceTestID}.pdf"))
            {
                System.Byte[] byteArray = System.Convert.FromBase64String(_ImagesData);
                stream.Write(byteArray, 0, byteArray.Length);
            }

            var x = 1;

            /// This needs to be integrated into the send in process
        }
        //private static void InsertSoapEnvelopeIntoWebRequest(XmlDocument soapEnvelopeXml, HttpWebRequest webRequest)
        //{
        //    using (Stream stream = webRequest.GetRequestStream())
        //    {
        //        soapEnvelopeXml.Save(stream);
        //    }
        //}


    }
    // using System.Xml.Serialization;
    // XmlSerializer serializer = new XmlSerializer(typeof(RequestAuthorizationResponse));
    // using (StringReader reader = new StringReader(xml))
    // {
    //    var test = (RequestAuthorizationResponse)serializer.Deserialize(reader);
    // }

    [XmlRoot(ElementName = "ImagesData")]
    public class ImagesData
    {

        [XmlElement(ElementName = "ImageData")]
        public string ImageData { get; set; }
    }

    [XmlRoot(ElementName = "RequestAuthorization")]
    public class RequestAuthorization
    {

        [XmlElement(ElementName = "ReferenceTestID")]
        public int ReferenceTestID { get; set; }

        [XmlElement(ElementName = "ClientReferenceID")]
        public object ClientReferenceID { get; set; }

        [XmlElement(ElementName = "RequestStatus")]
        public string RequestStatus { get; set; }

        [XmlElement(ElementName = "ImagesData")]
        public ImagesData ImagesData { get; set; }

        [XmlAttribute(AttributeName = "xmlns")]
        public object Xmlns { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "RequestAuthorizationResult")]
    public class RequestAuthorizationResult
    {

        [XmlElement(ElementName = "RequestAuthorization")]
        public RequestAuthorization RequestAuthorization { get; set; }
    }

    [XmlRoot(ElementName = "RequestAuthorizationResponse")]
    public class RequestAuthorizationResponse
    {

        [XmlElement(ElementName = "RequestAuthorizationResult")]
        public RequestAuthorizationResult RequestAuthorizationResult { get; set; }

        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }

        [XmlText]
        public string Text { get; set; }
    }



}
