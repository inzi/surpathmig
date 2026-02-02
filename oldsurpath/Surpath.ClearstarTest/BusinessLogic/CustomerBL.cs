using Newtonsoft.Json;
using Serilog;
using Surpath.CSTest.Customer;
using Surpath.CSTest.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Linq;

namespace Surpath.ClearStar.BL
{
    public class CustomerBL
    {
        public ILogger _logger;
        public CustomerBL(ILogger __logger = null)
        {
            if (__logger != null)
            {
                _logger = __logger;
            }
            else
            {
                _logger = new LoggerConfiguration().CreateLogger();
            }
        }
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

        public CustomerModel CreateCustomer(CustomerModel cust, string Position, string AcctCode)
        {
            _logger.Debug("CreateCustomer");

            //Get Credentials
            var cred = DefaultCredentialsBL.GetCredentials();
            cred.AcctCode = AcctCode;
            cred.Position = Position;
            _logger.Debug($"Credential created: {AcctCode} {Position}");
            //Init the web Service.
            Surpath.CSTest.Customer.Customer c = new Customer();
            //Set defaults that wern't set in the passed in customer.
            cust.ParentId = "SLSS_00005";//ToDo: What do we want set Parent to.  //do we want a main school and then the departments.  rather not
            cust.PaymentTerms = 0; //ToDO: Pay per Profile
            //cust.ShortName = "Test User" + "23"; //Guid.NewGuid().ToString().ToUpper();
            //cust.ShortName = "master user3";  //TODO:Set this to OVN CODE?
            cust.isCentInvoice = false;
            cust.isCentContract = false;
            cust.Comments = "Auto Created";  //TODO: Do we want comments here?
            cust.isSurcharge = true; //passed on to customer
            cust.isReqReview = true;  //So that pass fail will work;
            cust.DeliveryMethod = 2;  // Mail Delivery Method could possibly be 5 if we program something to recieve.
            cust.DeliveryAddress = "notify@surscan";
            cust.DeliveryFrom = string.Empty;
            cust.TimeZone = "Central Standard Time";
            cust.DateFormat = string.Empty;//causes default format
            cust.TaxRate = 8; //ToDo: Put in config  - does it need to be by school?? Discuss What should this be? do we need configurable
            cust.InvGroup = "Monthly"; //ToDo: set as monthy there are no standard values for this.
            cust.isInvoiceCompProfiles = true; //ToDo: orders must be complete in order to fulfill. correct???
            cust.DistId = 0;//no distributer id
            cust.isReviewFlaggedOnly = false; //for pass fail.
            cust.isAutoSummarize = true;  //if more than simple pass fail is needed description
            cust.isAutoPass = true; //so that those that have no flags (adverse data) get the pass applied ASAP
            cust.isCrossCustomerActive = false;
            cust.isPassFailDashboardActive = true;
            cust.isBatchImportActive = false;
            cust.isResultsTagActive = true;
            cust.isProfileExpirationActive = false; //ToDo: are we setting an expiration?
            cust.isInvoiceCompProfiles = false; //invoice per profile once 100% //ToDO: need to discuss
            _logger.Debug("cust object");

            _logger.Debug(JsonConvert.SerializeObject(cust, Newtonsoft.Json.Formatting.Indented));

            try
            {
                var _msgType = string.Empty;

                //c.AddFolder()
                _logger.Debug("Finding customer");
                XmlNode _getCust = c.GetChildCustomers(cred.UserName, cred.Password, cred.BoID, cust.ParentId);
                _logger.Debug("XMLNode Returned");
                _logger.Debug(xmlNodeToString(_getCust, 1));
                // Check for errors
                foreach (XmlNode enode in _getCust["ErrorStatus"])
                {
                    if (enode.Name.Equals("Type", StringComparison.InvariantCultureIgnoreCase))
                    {
                        _msgType = enode.InnerText;
                    }
                }
                if (string.IsNullOrEmpty(_msgType)==true)
                {

                    _logger.Debug($"Searching for customer with shortname {cust.ShortName}");

                    _logger.Debug("starting foreach of nodes");
                    foreach (XmlNode rnode in _getCust)
                    {
                        Debug.WriteLine("Node:" + rnode.Name + " " + rnode.InnerText);
                        _logger.Debug("Node:" + rnode.Name + " " + rnode.InnerText);
                        if (rnode.Name.Equals("Customer", StringComparison.CurrentCultureIgnoreCase))
                        {
                            if (rnode["sShortName"].InnerText.Equals(cust.ShortName, StringComparison.InvariantCultureIgnoreCase))
                            {
                                var _custId = rnode["sCustID"].InnerText;
                                _logger.Debug($"Found {cust.ShortName} - _custId = {_custId}");
                                cust.CustomerId = _custId;
                                return cust;
                            }
                        }
                    } 
                }
                else
                {
                    _logger.Debug($"Couldn't get child customers - msgType response = {_msgType}");
                }
                _logger.Debug("Customer doesn't exist, adding");
                _msgType = string.Empty;
                _logger.Debug("Trying to create newcust object");
                XmlNode newcust = c.CreateCustomer2(cred.UserName, cred.Password, cred.BoID, cust.ParentId, cust.PaymentTerms, cust.ShortName, cust.FullName, cust.Address1, cust.Address2, cust.City, cust.State, cust.Zip, cust.Phone, cust.Fax, cust.isCentInvoice, cust.isCentContract, cust.Comments, cust.isSurcharge, cust.isReqReview, cust.Email, cust.DeliveryMethod, cust.DeliveryAddress, cust.DeliveryFrom, cust.TimeZone, cust.DateFormat, Int32.Parse(cust.TaxRate.ToString()), cust.InvGroup, cust.isInvoiceCompProfiles, cust.DistId, cust.isReviewFlaggedOnly, cust.isAutoSummarize, cust.isAutoPass, cust.isCrossCustomerActive, cust.isPassFailDashboardActive, cust.isBatchImportActive, cust.isResultsTagActive, cust.isProfileExpirationActive, cust.isInvoiceCompProfiles);
                _logger.Debug("XMLNode NewCust Returned");
                _logger.Debug(xmlNodeToString(newcust, 1));
                // Check for errors
                foreach(XmlNode enode in newcust["ErrorStatus"])
                {
                    if (enode.Name.Equals("Type",StringComparison.InvariantCultureIgnoreCase))
                    {
                        _msgType = enode.InnerText;
                    }
                }

                if (string.IsNullOrEmpty(_msgType)==true)
                {
                    _logger.Debug("Has Customer child");
                    try
                    {

                        _logger.Debug("starting foreach of nodes");
                        foreach (XmlNode node in newcust["Customer"].ChildNodes)
                        {
                            Debug.WriteLine("Node:" + node.Name + " " + node.InnerText);
                            _logger.Debug("Node:" + node.Name + " " + node.InnerText);

                            if (node.InnerText.Contains("10ErrorSQL:Cannot insert duplicate key row in object"))
                            {
                                _logger.Debug("node innertext contains 10ErrorSQL:Cannot insert duplicate key row in object");
                                return cust;
                            }
                            else
                            {
                                if (node.Name == "sNewCustID")
                                {
                                    cust.CustomerId = node.InnerText;
                                }
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        _logger.Error(ex.ToString());
                        if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                        if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                        return cust;
                    }
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.ToString());
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                throw ex;
            }

            return cust;
        }

        public CustomerModel GetChildCustomers(CustomerModel cust, string Position, string AcctCode)
        {
            var _labcode = "0VN.MPOS.DALLASC.MAMMO";

            try
            {
                Surpath.CSTest.Customer.Customer c = new Customer();

                var cred = DefaultCredentialsBL.GetCredentials();
                cred.AcctCode = AcctCode;
                cred.Position = Position;
                _logger.Debug($"Credential created: {AcctCode} {Position}");

                _logger.Debug("Trying to get customers");
                _logger.Debug($"{cred.UserName}");
                _logger.Debug($"{cred.Password}");
                _logger.Debug($"{cred.BoID}");
                _logger.Debug($"{cust.ParentId}");


                XmlNode _getCust = c.GetChildCustomers(cred.UserName, cred.Password, cred.BoID, "SLSS_00005");
                _logger.Debug("XMLNode Returned");
                _logger.Debug(xmlNodeToString(_getCust, 1));

                _logger.Debug("starting foreach of nodes");
                foreach (XmlNode rnode in _getCust)
                {
                    Debug.WriteLine("Node:" + rnode.Name + " " + rnode.InnerText);
                    _logger.Debug("Node:" + rnode.Name + " " + rnode.InnerText);
                    if (rnode.Name.Equals("Customer", StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (rnode["sShortName"].InnerText.Equals(_labcode, StringComparison.InvariantCultureIgnoreCase))
                        {
                            _logger.Debug($"Found {_labcode}");
                            var _custId = rnode["sCustID"].InnerText;
                            cust.CustomerId = _custId;
                            return cust;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                return cust;
            }
            return cust;
        }

        public XmlNode AddCustomerService(string CustID, string ServiceNo, string Description)
        {
            var cred = DefaultCredentialsBL.GetCredentials();
            //string CustID = "SLSS_00001";
            //string ServiceNo = "SLSS_0016";//Employment Verification
            //string Description = "Employment Verification";

            Surpath.CSTest.Customer.Customer c = new Customer();
            XmlNode custresp = c.AddService(cred.UserName, cred.Password, cred.BoID, CustID, ServiceNo, Description, false, 0, false, false, true, 45, 45, 1, 100, 0, 0, false);

            return custresp;
        }

        public void DeleteCustomer()
        {
        }

        public XmlNode AddFolder()
        {
            var cred = DefaultCredentialsBL.GetCredentials();
            string CustID = "SLSS_00001";
            string ServiceNo = "SLSS_0016";//Employment Verification
            string FolderName = CustID;
            Surpath.CSTest.Customer.Customer c = new Customer();
            XmlNode custresp = c.AddFolder(cred.UserName, cred.Password, cred.BoID, CustID, FolderName);

            return custresp;
        }

        private string xmlNodeToString(XmlNode node, int indentation)
        {
            using (var sw = new System.IO.StringWriter())
            {
                using (var xw = new System.Xml.XmlTextWriter(sw))
                {
                    xw.Formatting = System.Xml.Formatting.Indented;
                    xw.Indentation = indentation;
                    node.WriteContentTo(xw);
                }
                return sw.ToString();
            }
        }
    }
}