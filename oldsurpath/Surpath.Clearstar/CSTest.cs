using System.Web.Configuration;

namespace Surpath.Clearstar
{
    public class CSTest
    {
        public void DisplayProfileReport()
        {
            string sUserName = WebConfigurationManager.AppSettings["UserName"];
            string sPassword = WebConfigurationManager.AppSettings["Password"];
            int iBOID = int.Parse(WebConfigurationManager.AppSettings["BoID"]);

            string sCustID = "SLSS_00001";
            string sProfNo = "2018031442776347";

            byte[] bytes = null;

            //var Response = new WebResponse();

            using (CS.Profile.Profile gwProfile = new CS.Profile.Profile())
            {
                try
                {
                    bytes = gwProfile.GetProfileReport(sUserName, sPassword, iBOID, sCustID, sProfNo, false);
                }
                catch (System.Web.Services.Protocols.SoapException ex)
                {
                    //do something
                }
                if (bytes.Length > 0)
                {
                    //Response.BufferOutput = true;
                    //Response.AddHeader("Content-Disposition", "inline");
                    //Response.ContentType = "application/pdf";
                    //Response.BinaryWrite(bytes);
                }
            }
        }
    }
}