using Surpath.CSTest.Service;
using System.Diagnostics;
using System.Text;
using System.Xml;

namespace Surpath.ClearStar.BL
{
    public class ServiceBL
    {
        public XmlNode GetServices(string CustId)
        {
            var cred = DefaultCredentialsBL.GetCredentials();
            //string CustID = "SLSS_00001";
            string ProfNo = "2018041204567257";
            //cred.AcctCode = AcctCode;
            //cred.Position = Position;
            Surpath.CSTest.Service.Service s = new Service();
            //Surpath.CSTest.Profile.Profile p = new Profile();

            //XmlNode result = p.GetProfileDetail(cred.UserName, cred.Password, cred.BoID, CustId, ProfNo);
            //int nodecount = result.ChildNodes.Count;

            XmlNode servresp = s.GetServices(cred.UserName, cred.Password, cred.BoID, CustId);
            //doc = XmlDocument
            StringBuilder sb = new StringBuilder();

            foreach (XmlNode node in servresp["Service"].ChildNodes)
            {
                Debug.WriteLine("Node:" + node.Name + " " + node.InnerText);
            }

            return servresp;
        }

        public XmlNode GetServiceDetail(string CustId)
        {
            var cred = DefaultCredentialsBL.GetCredentials();
            Surpath.CSTest.Service.Service s = new Service();
            string sDotConnectExample = ""; //ToDo: need to know what to set this to?
            string sGWOrderFieldsExample = ""; //ToDo: need to know what to set this to?
            string ServiceNo = "SLSS_00001"; //national background check
            //string ServiceNo = "SLSS_00081"; //Canadian Driving Record Check
            XmlNode servresp = s.GetServiceDetail(cred.UserName, cred.Password, cred.BoID, CustId, ServiceNo, sDotConnectExample, sGWOrderFieldsExample);

            foreach (XmlNode node in servresp["Servies"].ChildNodes)
            {
                Debug.WriteLine("Node:" + node.Name + " " + node.InnerText);
            }

            return servresp;    //ToDo: need to implement
            //return servresp;
        }
    }
}