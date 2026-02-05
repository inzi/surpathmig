using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Surpath.Formfox
{
    public class SurpathFormfox
    {

        public SurpathFormfox()
        {

        }

        public void CreateOrder()
        {
            try
            {

                XmlDocument xmlDoc = CreateOrderRequestFactory();
                //XmlDocument xmlDoc1 = new XmlDocument();
                XmlElement xmlRoot;
                XmlNode xmlNode;


            }
            catch (Exception ex)
            {

                //_logger.Error(ex.ToString());
                //if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                //if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                throw;
            }
        }


        public XmlDocument CreateOrderRequestFactory()
    {

            // required for CreateOrder
            //Sending Facility
            //Process Type
            //Primary Donor ID
            //First Name
            //Last Name
            //WorkPhone or HomePhone  If there is an MRO associated with the account.
            //Valid Laboratory ID(for lab drug tests)
            //Sample Type
            //Valid laboratory account(for lab drug tests)
            //Complete / Valid < TestProcedure > section
            //Valid Unit Codes(for lab drug tests)
            //*Reason for Test


            XmlDocument xmlDoc = new XmlDocument();

            com.formfoxtest.www.FFOrderSvc x = new com.formfoxtest.www.FFOrderSvc();
            CreateOrderTest createOrderTest = new CreateOrderTest();
            createOrderTest.SendingFacility = "SURSCAN";
            createOrderTest.SendingFacilityTimeZone = "-5";
            createOrderTest.ClientReferenceID = "test";
            createOrderTest.SendingFacilityID = "262";
            createOrderTest.PersonalData.PrimaryID = "1234567890";
            createOrderTest.PersonalData.PrimaryIDType = "SSN";
            createOrderTest.PersonalData.PersonName.GivenName = "Chris";
            createOrderTest.PersonalData.PersonName.FamilyName = "Norman";
            createOrderTest.PersonalData.ContactMethod.Telephone.Add(new Telephone() { Type = "Mobile", FormattedNumber = "2148019441" });
            createOrderTest.Services.Service.Add(new Service() { Type = "Drug", LaboratoryAccount = "adas" });
            x.CreateOrderAsync("SRSC0216", "XR5_2bPcv9", createOrderTest.ToString());


            return xmlDoc;

        }
        public HttpWebRequest CreateSOAPWebRequestCreateOrderTest()
        {
            //Making Web Request    
            HttpWebRequest Req = (HttpWebRequest)WebRequest.Create(@"https://www.formfox.com/v2/ffordersvc/CreateOrder");
            //SOAPAction    
            Req.Headers.Add(@"SOAPAction:https://www.formfox.com/v2/ffordersvc/CreateOrder");
            //Content_type    
            Req.ContentType = "text/xml;charset=\"utf-8\"";
            Req.Accept = "text/xml";
            //HTTP method    
            Req.Method = "POST";
            //return HttpWebRequest    
            return Req;
        }

        public void InvokeServiceCreateOrderTest(string Username, string Password, string OrderXml)
        {
            //Calling CreateSOAPWebRequest method    
            HttpWebRequest request = CreateSOAPWebRequestCreateOrderTest();

            XmlDocument SOAPReqBody = new XmlDocument();
            //SOAP Body Request    

            string SoapBody = string.Empty;
            SoapBody += @"<?xml version=""1.0"" encoding=""utf-8""?>";
            SoapBody += @"<soap: Envelope xmlns: soap = ""http: //schemas.xmlsoap.org/soap/envelope/"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:soap12=""http://www.w3.org/2001/XMLSchema"">";
            SoapBody += @"<soap: Body >";
            SoapBody += @"<CreateOrder xmlns = ""https://www.formfoxtest.com/v2/ffordersvc"" >";

            SoapBody += @"<Username>SRSC0216</Username>";
            SoapBody += @"<Password>XR5_2bPcv9</Password>";
            SoapBody += @"<OrderXML>";
            // Add our request object xml
            SoapBody += @"</OrderXML>";


            //  < Username > string </ Username >

            //  < Password > string </ Password >

            //  < OrderXML > string </ OrderXML >

            SoapBody += @" </CreateOrder>";


            //SOAPReqBody.LoadXml(@" <?xml version=""1.0"" encoding=""utf-8""?>   < soap: Envelope xmlns: soap = ""http: //schemas.xmlsoap.org/soap/envelope/"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
            //< soap: Body >< Addition xmlns = ""http: //tempuri.org/"">    

            // < a > " + a + @" < / a >   < b > " + b + @" < / b >   < / Addition >   < / soap:Body >   < / soap:Envelope > "

            //);



            //    using (Stream stream = request.GetRequestStream())
            //    {
            //        SOAPReqBody.Save(stream);
            //    }
            //    //Geting response from request    
            //    using (WebResponse Serviceres = request.GetResponse())
            //    {
            //        using (StreamReader rd = new StreamReader(Serviceres.GetResponseStream()))
            //        {
            //            //reading stream    
            //            var ServiceResult = rd.ReadToEnd();
            //            //writting stream result on console    
            //            Console.WriteLine(ServiceResult);
            //            Console.ReadLine();
            //        }
            //    }
        }
    }


}
