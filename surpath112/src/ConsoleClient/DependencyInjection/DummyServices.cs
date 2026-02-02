using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleClient.DependencyInjection
{
    using inzibackend.Url;

    namespace ConsoleClient.DummyServices
    {
        public class DummyWebUrlService : IWebUrlService
        {
            public string WebSiteRootAddressFormat { get; } = string.Empty;

            public string ServerRootAddressFormat { get; } = string.Empty;

            public bool SupportsTenancyNameInUrl { get; }

            public List<string> GetRedirectAllowedExternalWebSites()
            {
                return new List<string>();
            }

            public string GetSiteRootAddress(string tenancyName = null)
            {
                return "http://localhost";
            }

            public string GetServerRootAddress(string tenancyName = null)
            {
                return "http://localhost";
            }
        }
    }
}
