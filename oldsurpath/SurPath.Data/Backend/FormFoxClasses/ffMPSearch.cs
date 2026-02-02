using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace SurPath.Data.Backend
{
    #region FFMPSearch

  
    [Serializable]
    public class FFMPSearchPayload
    {
        public string Address1 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public List<string> Services { get; set; } = new List<string>();
        public int SearchRadius1 { get; set; }
        public int SearchRadius2 { get; set; }
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
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double Distance { get; set; } = -1;
        public bool MarketplaceEnabled { get; set; }
        public List<FFMPService> Services { get; set; }
        public List<object> Amenities { get; set; }
        public bool NoMarketplaceId { get; set; } = false;
    }

    public class FFMPSites
    {
        public List<FFMPSearchResult> SiteList { get; set; } = new List<FFMPSearchResult>();
        public string PriceTier { get; set; }

        public FFMPSearchResult Closest()
        {
            if (this.SiteList.Count > 0)
            {
                return this.SiteList.OrderBy(s => s.Distance).First();

            }
            else
            {
                return new FFMPSearchResult();
            }
        }
    }
    #endregion FFMPSearch

}
