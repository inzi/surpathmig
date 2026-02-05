using Microsoft.VisualStudio.TestTools.UnitTesting;
using Surpath.ClearStar.BL;
using System;
using System.Xml;

namespace Surpath.CSTest.Tests
{
    [TestClass()]
    public class ServiceBLTests
    {
        [TestMethod()]
        public void GetServicesDetail()
        {
            var creds = DefaultCredentialsBL.GetCredentials();
            Surpath.ClearStar.BL.ServiceBL service = new ServiceBL();
            //string sCustID = "SLSS_00001";

            try
            {
                string sCustID = "SLSS_00042";
                //XmlNode node = service.GetServices(sCustID);
                XmlNode node = service.GetServiceDetail(sCustID);
                Assert.AreEqual("SLSS_00042", sCustID);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //Assert.Fail();
        }

        [TestMethod()]
        public void GetServices()
        {
            var creds = DefaultCredentialsBL.GetCredentials();
            Surpath.ClearStar.BL.ServiceBL service = new ServiceBL();
            //string sCustID = "SLSS_00042";
            string sCustID = "SLSS_00001";//Surscan

            try
            {
                XmlNode node = service.GetServices(sCustID);
                //Assert.AreEqual("SLSS_00042", sCustID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}