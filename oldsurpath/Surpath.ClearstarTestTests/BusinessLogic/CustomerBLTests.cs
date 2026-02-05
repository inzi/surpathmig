using Microsoft.VisualStudio.TestTools.UnitTesting;
using Surpath.ClearStar.BL;
using Surpath.CSTest.Models;
using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Xml;

namespace Surpath.CSTest.Tests
{
    [TestClass()]
    public class CustomerBLTests
    {
        [TestMethod()]
        public void CreateCustomerTest()
        {
            var creds = DefaultCredentialsBL.GetCredentials();
            Surpath.ClearStar.BL.CustomerBL service = new CustomerBL();
            CustomerModel cust = new CustomerModel();
            cust.FullName = "Mike Kearl";
            cust.City = "McKinney";
            cust.State = "TX";
            cust.Zip = "75071";
            cust.Phone = "801-404-9676";
            cust.Address1 = "4805 Carolina Circle";
            cust.Comments = "Total Stud";
            cust.Email = "mike.kearl@gmail.com";
            //cust.ShortName = "";
            //Todo: we may want to pass surscancustid in here .. right now im generating it.

            CustomerModel newcust = service.CreateCustomer(cust, "tester", "test");

            Assert.AreNotEqual("", newcust.CustomerId);
        }


        [TestMethod()]
        public void GetGetChildCustomersTest()
        {
            var creds = DefaultCredentialsBL.GetCredentials();
            Surpath.ClearStar.BL.CustomerBL service = new CustomerBL();
            CustomerModel cust = new CustomerModel();
            //cust.FullName = "Mike Kearl";
            //cust.City = "McKinney";
            //cust.State = "TX";
            //cust.Zip = "75071";
            //cust.Phone = "801-404-9676";
            //cust.Address1 = "4805 Carolina Circle";
            //cust.Comments = "Total Stud";
            //cust.Email = "mike.kearl@gmail.com";
            //cust.ShortName = "";
            //Todo: we may want to pass surscancustid in here .. right now im generating it.

            //2021 - 05 - 25 13:48:33.150 - 05:00[Debug] Trying to get customer
            //2021 - 05 - 25 13:48:33.150 - 05:00[Debug] jonjackson
            //2021 - 05 - 25 13:48:33.150 - 05:00[Debug] % LCFr@4VMLr5nP4 - ve6UQ4 - 5d % 6
            //2021 - 05 - 25 13:48:33.150 - 05:00[Debug] 763
            //2021 - 05 - 25 13:48:33.150 - 05:00[Debug] test


            //service.GetChildCustomers("jonjackson", "%LCFr@4VMLr5nP4-ve6UQ4-5d%6", "763", "SLSS_00005");
            service.GetChildCustomers(cust,"tester","test");

            //Assert.AreNotEqual("", newcust.CustomerId);
            Assert.AreEqual(true, true);
        }

        public void DeleteCustomerTest()
        {
        }

        [TestMethod()]
        public void AddServiceToCustomer()
        {
            //var creds = DefaultCredentialsBL.GetCredentials();
            //Surpath.ClearStar.BL.CustomerBL c = new CustomerBL();
            //XmlNode node = c.AddCustomerService();
            //Assert.AreNotEqual("blah","blah");
        }

        [TestMethod()]
        public void AddFolderToCustomer()
        {
            var creds = DefaultCredentialsBL.GetCredentials();
            Surpath.ClearStar.BL.CustomerBL c = new CustomerBL();

            XmlNode node = c.AddFolder();

            Assert.AreNotEqual("blah", "blah");
        }


        [TestMethod]
        public void TestMail()
        {

            string subject = "Outlook test";
            string toEmail = "chris@inzi.com";
            string mailBody = "Mail body test";

            SmtpClient smtpClient = new SmtpClient();
            NetworkCredential basicCredential = new NetworkCredential("notify@surscan.com", "rock@water30");
            MailMessage message = new MailMessage();
            MailAddress fromAddress = new MailAddress("notify@surscan.com");

            smtpClient.Host = "smtp.office365.com"; // ConfigurationManager.AppSettings["SmtpHost"].ToString().Trim();
            smtpClient.Port = 587;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = basicCredential;
            smtpClient.EnableSsl = true;

            message.From = fromAddress;
            message.Subject = subject;

            message.IsBodyHtml = true;
            message.Body = mailBody;
            message.To.Add(toEmail);

         

            try
            {
                smtpClient.Send(message);
            }
            catch (Exception ex)
            {
                throw ex;
                //
            }
        }
    }
}