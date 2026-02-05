using Surpath.CSTest.Models;
using System.Web.Configuration;

namespace Surpath.ClearStar.BL
{
    public static class DefaultCredentialsBL
    {
        public static DefaultCredentialsModel GetCredentials()
        {
            DefaultCredentialsModel cred = new DefaultCredentialsModel();
            string testgateway = WebConfigurationManager.AppSettings["UseTestGateway"];
            string sUserName = string.Empty;
            string sPassword = string.Empty;
            if (bool.Parse(testgateway))
            {
                //Test Server Credentials
                sUserName = WebConfigurationManager.AppSettings["TestLoginName"];
                sPassword = WebConfigurationManager.AppSettings["TestPassword"];
            }
            else
            {
                //Prod Server Credentials
                sUserName = WebConfigurationManager.AppSettings["ProdLoginName"];
                sPassword = WebConfigurationManager.AppSettings["ProdPassword"];
            }
            int iBOID = int.Parse(WebConfigurationManager.AppSettings["BoID"]);
            cred.BoID = iBOID;
            cred.UserName = sUserName;
            cred.Password = sPassword;
            return cred;
        }
    }
}