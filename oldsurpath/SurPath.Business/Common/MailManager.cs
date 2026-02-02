using SurPath.Entity;
using SurpathBackend;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Xml;
using System.Xml.Xsl;

namespace SurPath.Business
{
    public class MailManager
    {
        public string SMTPFromAddress = String.Empty;
        bool Production = false;
        string TestEmailAddress = string.Empty;
        public MailManager()
        {
            this.SMTPFromAddress = ConfigurationManager.AppSettings["SmtpFromAddress"].ToString().Trim();
            var _production = ConfigurationManager.AppSettings["Production"].ToString().Trim();
            bool.TryParse(_production, out bool isProduction);
            this.Production = isProduction;
            if (!this.Production)
            {
                TestEmailAddress = ConfigurationManager.AppSettings["TestEmailAddress"].ToString().Trim();
            }
        }

        public string ToEmailProdCheck(string toEmail)
        {
            if (!this.Production)
            {
                toEmail = TestEmailAddress;
            }
            return toEmail;
        }

        public void SendMail(string toEmail, string subject, string mailBody, Byte[] attachmentobject = null, string attachmentmimetype = "", string attachmentname = "")
        {
            SmtpClient smtpClient = new SmtpClient();
            NetworkCredential basicCredential = new NetworkCredential(this.SMTPFromAddress, ConfigurationManager.AppSettings["SmtpFromAddressPassword"].ToString().Trim());
            MailMessage message = new MailMessage();
            MailAddress fromAddress = new MailAddress(this.SMTPFromAddress);

            smtpClient.Host = ConfigurationManager.AppSettings["SmtpHost"].ToString().Trim();
            smtpClient.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"].ToString().Trim());
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = basicCredential;
            smtpClient.EnableSsl = true;

            message.From = fromAddress;
            message.Subject = subject;

            message.IsBodyHtml = true;
            message.Body = mailBody;
            message.To.Add(ToEmailProdCheck(toEmail));

            if (attachmentobject != null && !string.IsNullOrEmpty(attachmentmimetype) && !string.IsNullOrEmpty(attachmentname))
            {
                // we have an attachment
                Attachment attachment = new Attachment(new MemoryStream(attachmentobject), attachmentmimetype);
                attachment.ContentDisposition.FileName = attachmentname;
                message.Attachments.Add(attachment);
            }

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

        public string GenerateNotificationMail(NotificationInformation n)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlDocument xmlDoc1 = new XmlDocument();
            XmlElement xmlRoot;
            XmlNode xmlNode;

            xmlDoc.LoadXml(
           "<?xml version=\"1.0\" encoding=\"utf-8\" ?>" +
           "<Root>" +
           "<Recipient/>" +
           "<Program/>" +
           "<Note/>" +
           "<Again/>" +
           "</Root>");

            xmlRoot = xmlDoc.DocumentElement;

            xmlNode = xmlRoot.SelectSingleNode("/Root/Recipient");
            xmlNode.InnerText = n.donor_email;

            xmlRoot = xmlDoc.DocumentElement;

            xmlNode = xmlRoot.SelectSingleNode("/Root/Program");
            string name = n.department_name;
            xmlNode.InnerText = name.Trim();

            xmlRoot = xmlDoc.DocumentElement;

            xmlNode = xmlRoot.SelectSingleNode("/Root/Note");
            xmlNode.InnerText = string.Empty;

            xmlNode = xmlRoot.SelectSingleNode("/Root/Again");
            xmlNode.InnerText = n.notify_again.ToString().ToLower();

            XslCompiledTransform xslDoc = new XslCompiledTransform();
            //string xsltTemplate = "default_backend_email_template.xslt";// NotificationEmailTemplate
            //string xsltTemplate = ConfigurationManager.AppSettings["NotificationEmailTemplate"].ToString().Trim();
            string xsltTemplate = new BackendFiles().ReadTextFile(ConfigurationManager.AppSettings["NotificationEmailTemplate"].ToString().Trim());
            using (StringReader sr = new StringReader(xsltTemplate))
            {
                using (XmlReader xr = XmlReader.Create(sr))
                {
                    xslDoc.Load(xr);
                }
            }

            //xslDoc.Load(ConfigurationManager.AppSettings["XSLTPath"].ToString().Trim() + xsltTemplate);

            XsltArgumentList xslArgs = new XsltArgumentList();
            StringWriter writer = new StringWriter();
            xslDoc.Transform(xmlDoc, xslArgs, writer);

            string mailBody = writer.ToString();

            return mailBody;
        }

        public string SendRegistrationMail(User user, string mode)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlDocument xmlDoc1 = new XmlDocument();
            XmlElement xmlRoot;
            XmlNode xmlNode;

            xmlDoc.LoadXml(
           "<?xml version=\"1.0\" encoding=\"utf-8\" ?>" +
           "<Root>" +
           "<ID/>" +
           "<Name/>" +
           "<Username/>" +
           "<Password/>" +
           "</Root>");

            xmlRoot = xmlDoc.DocumentElement;

            xmlNode = xmlRoot.SelectSingleNode("/Root/ID");
            xmlNode.InnerText = user.UserId.ToString();

            xmlRoot = xmlDoc.DocumentElement;

            xmlNode = xmlRoot.SelectSingleNode("/Root/Name");
            string name = user.UserFirstName + ' ' + user.UserLastName;
            xmlNode.InnerText = name.Trim();

            xmlRoot = xmlDoc.DocumentElement;

            xmlNode = xmlRoot.SelectSingleNode("/Root/Username");
            xmlNode.InnerText = user.Username;

            xmlNode = xmlRoot.SelectSingleNode("/Root/Password");
            xmlNode.InnerText = UserAuthentication.Decrypt(user.UserPassword, true);

            XslCompiledTransform xslDoc = new XslCompiledTransform();
            if (mode == "Registration")
            {
                xslDoc.Load(ConfigurationManager.AppSettings["XSLTPath"].ToString().Trim() + "Registration.xslt");
            }
            else if (mode == "ForgotPassword")
            {
                xslDoc.Load(ConfigurationManager.AppSettings["XSLTPath"].ToString().Trim() + "ForgotPassword.xslt");
            }

            XsltArgumentList xslArgs = new XsltArgumentList();
            StringWriter writer = new StringWriter();
            xslDoc.Transform(xmlDoc, xslArgs, writer);

            string mailBody = writer.ToString();

            mailBody = mailBody.Replace("@Model.ActivationLink", ConfigurationManager.AppSettings["ActivationLink"].ToString().Trim() + UserAuthentication.URLIDEncrypt(user.UserId.ToString(), false));
            mailBody = mailBody.Replace("@Model.HeaderLogo", ConfigurationManager.AppSettings["MailerHeaderLogo"].ToString().Trim());

            return mailBody;
        }

        public string SendClientProgramRegistrationMail(Donor donor, Client client, ClientDepartment clientDepartment)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlDocument xmlDoc1 = new XmlDocument();
            XmlElement xmlRoot;
            XmlNode xmlNode;

            xmlDoc.LoadXml(
           "<?xml version=\"1.0\" encoding=\"utf-8\" ?>" +
           "<Root>" +
           "<ClientName/>" +
           "<DonorName/>" +
           "<ClientDepartmentName/>" +
           "<PaymentAmount/>" +
           "</Root>");

            xmlRoot = xmlDoc.DocumentElement;

            xmlNode = xmlRoot.SelectSingleNode("/Root/ClientName");
            xmlNode.InnerText = client.ClientName.Trim();

            xmlRoot = xmlDoc.DocumentElement;

            xmlNode = xmlRoot.SelectSingleNode("/Root/DonorName");
            string donorName = donor.DonorFirstName + ' ' + donor.DonorLastName;
            xmlNode.InnerText = donorName.Trim();

            xmlRoot = xmlDoc.DocumentElement;
            xmlNode = xmlRoot.SelectSingleNode("/Root/ClientDepartmentName");
            xmlNode.InnerText = clientDepartment.DepartmentName;

            xmlRoot = xmlDoc.DocumentElement;
            xmlNode = xmlRoot.SelectSingleNode("/Root/PaymentAmount");
            xmlNode.InnerText = donor.ProgramAmount.ToString();

            XslCompiledTransform xslDoc = new XslCompiledTransform();
            xslDoc.Load(ConfigurationManager.AppSettings["XSLTPath"].ToString().Trim() + "ClientProgramRegsitration.xslt");

            XsltArgumentList xslArgs = new XsltArgumentList();
            StringWriter writer = new StringWriter();
            xslDoc.Transform(xmlDoc, xslArgs, writer);

            string mailBody = writer.ToString();

            mailBody = mailBody.Replace("@Model.HeaderLogo", ConfigurationManager.AppSettings["MailerHeaderLogo"].ToString().Trim());

            return mailBody;
        }

        public string SendDonorProgramRegistrationMail(Donor donor, Client client, ClientDepartment clientDepartment)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlDocument xmlDoc1 = new XmlDocument();
            XmlElement xmlRoot;
            XmlNode xmlNode;

            xmlDoc.LoadXml(
           "<?xml version=\"1.0\" encoding=\"utf-8\" ?>" +
           "<Root>" +
           "<DonorName/>" +
           "<ClientName/>" +
           "<ClientDepartmentName/>" +
           "<PaymentAmount/>" +
           "</Root>");

            xmlRoot = xmlDoc.DocumentElement;

            xmlNode = xmlRoot.SelectSingleNode("/Root/ClientName");
            xmlNode.InnerText = client.ClientName.Trim();

            xmlRoot = xmlDoc.DocumentElement;

            xmlNode = xmlRoot.SelectSingleNode("/Root/DonorName");
            string donorName = donor.DonorFirstName + ' ' + donor.DonorLastName;
            xmlNode.InnerText = donorName.Trim();

            xmlRoot = xmlDoc.DocumentElement;
            xmlNode = xmlRoot.SelectSingleNode("/Root/ClientDepartmentName");
            xmlNode.InnerText = clientDepartment.DepartmentName;

            xmlRoot = xmlDoc.DocumentElement;
            xmlNode = xmlRoot.SelectSingleNode("/Root/PaymentAmount");
            xmlNode.InnerText = donor.ProgramAmount.ToString();

            XslCompiledTransform xslDoc = new XslCompiledTransform();
            xslDoc.Load(ConfigurationManager.AppSettings["XSLTPath"].ToString().Trim() + "DonorProgramRegsitration.xslt");

            XsltArgumentList xslArgs = new XsltArgumentList();
            StringWriter writer = new StringWriter();
            xslDoc.Transform(xmlDoc, xslArgs, writer);

            string mailBody = writer.ToString();

            mailBody = mailBody.Replace("@Model.HeaderLogo", ConfigurationManager.AppSettings["MailerHeaderLogo"].ToString().Trim());
            mailBody = mailBody.Replace("@Model.HeaderLogoBC", ConfigurationManager.AppSettings["MailerHeaderLogoBC"].ToString().Trim());

            return mailBody;
        }

        public string SendTPAProgramRegistrationMail(Donor donor, Client client, ClientDepartment clientDepartment)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlDocument xmlDoc1 = new XmlDocument();
            XmlElement xmlRoot;
            XmlNode xmlNode;

            xmlDoc.LoadXml(
           "<?xml version=\"1.0\" encoding=\"utf-8\" ?>" +
           "<Root>" +
           "<DonorName/>" +
           "<DonorEmail/>" +
           "<DonorSSN/>" +
           "<DonorDOB/>" +
           "<DonorCity/>" +
           "<DonorState/>" +
           "<DonorZipCode/>" +
           "<ClientName/>" +
           "<ClientDepartmentName/>" +
           "<PaymentAmount/>" +
           "</Root>");

            xmlRoot = xmlDoc.DocumentElement;

            xmlNode = xmlRoot.SelectSingleNode("/Root/DonorName");
            string donorName = donor.DonorFirstName + ' ' + donor.DonorLastName;
            xmlNode.InnerText = donorName.Trim();

            xmlRoot = xmlDoc.DocumentElement;

            xmlNode = xmlRoot.SelectSingleNode("/Root/DonorEmail");
            xmlNode.InnerText = donor.DonorEmail;

            xmlRoot = xmlDoc.DocumentElement;
            xmlNode = xmlRoot.SelectSingleNode("/Root/DonorSSN");
            xmlNode.InnerText = "***-**-" + donor.DonorSSN.Substring(7);

            xmlRoot = xmlDoc.DocumentElement;
            xmlNode = xmlRoot.SelectSingleNode("/Root/DonorDOB");
            xmlNode.InnerText = donor.DonorDateOfBirth.ToString("MM/dd/yyyy");

            xmlRoot = xmlDoc.DocumentElement;
            xmlNode = xmlRoot.SelectSingleNode("/Root/DonorCity");
            xmlNode.InnerText = donor.DonorCity;

            xmlRoot = xmlDoc.DocumentElement;
            xmlNode = xmlRoot.SelectSingleNode("/Root/DonorState");
            xmlNode.InnerText = donor.DonorState;

            xmlRoot = xmlDoc.DocumentElement;
            xmlNode = xmlRoot.SelectSingleNode("/Root/DonorZipCode");
            xmlNode.InnerText = donor.DonorZip;

            xmlRoot = xmlDoc.DocumentElement;

            xmlNode = xmlRoot.SelectSingleNode("/Root/ClientName");
            xmlNode.InnerText = client.ClientName.Trim();

            xmlRoot = xmlDoc.DocumentElement;
            xmlNode = xmlRoot.SelectSingleNode("/Root/ClientDepartmentName");
            xmlNode.InnerText = clientDepartment.DepartmentName;

            xmlRoot = xmlDoc.DocumentElement;
            xmlNode = xmlRoot.SelectSingleNode("/Root/PaymentAmount");
            xmlNode.InnerText = donor.ProgramAmount.ToString();

            XslCompiledTransform xslDoc = new XslCompiledTransform();
            xslDoc.Load(ConfigurationManager.AppSettings["XSLTPath"].ToString().Trim() + "TPAProgramRegsitration.xslt");

            XsltArgumentList xslArgs = new XsltArgumentList();
            StringWriter writer = new StringWriter();
            xslDoc.Transform(xmlDoc, xslArgs, writer);

            string mailBody = writer.ToString();

            mailBody = mailBody.Replace("@Model.HeaderLogo", ConfigurationManager.AppSettings["MailerHeaderLogo"].ToString().Trim());

            return mailBody;
        }

        public string SendDonorPaymentConfirmationMail(Donor donor, DonorTestInfo donorTestInfo, ClientDepartment clientDepartment, Client client)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlDocument xmlDoc1 = new XmlDocument();
            XmlElement xmlRoot;
            XmlNode xmlNode;

            xmlDoc.LoadXml(
           "<?xml version=\"1.0\" encoding=\"utf-8\" ?>" +
           "<Root>" +
           "<DonorName/>" +
           "<ClientName/>" +
           "<ClientDepartmentName/>" +
           "<PaymentAmount/>" +
           "<PaymentMethod/>" +
           "</Root>");

            xmlRoot = xmlDoc.DocumentElement;

            xmlNode = xmlRoot.SelectSingleNode("/Root/ClientName");
            xmlNode.InnerText = client.ClientName.Trim();

            xmlRoot = xmlDoc.DocumentElement;

            xmlNode = xmlRoot.SelectSingleNode("/Root/DonorName");
            string donorName = donor.DonorFirstName + ' ' + donor.DonorLastName;
            xmlNode.InnerText = donorName.Trim();

            xmlRoot = xmlDoc.DocumentElement;
            xmlNode = xmlRoot.SelectSingleNode("/Root/ClientDepartmentName");
            xmlNode.InnerText = clientDepartment.DepartmentName;

            xmlRoot = xmlDoc.DocumentElement;
            xmlNode = xmlRoot.SelectSingleNode("/Root/PaymentAmount");
            xmlNode.InnerText = donorTestInfo.TotalPaymentAmount.ToString();

            if (donorTestInfo.PaymentMethodId.ToString() == "Cash")
            {
                xmlRoot = xmlDoc.DocumentElement;
                xmlNode = xmlRoot.SelectSingleNode("/Root/PaymentMethod");
                xmlNode.InnerText = "Cash / Money Order";
            }
            else if (donorTestInfo.PaymentMethodId.ToString() == "Card")
            {
                xmlRoot = xmlDoc.DocumentElement;
                xmlNode = xmlRoot.SelectSingleNode("/Root/PaymentMethod");
                xmlNode.InnerText = "Credit / Debit Card";
            }

            XslCompiledTransform xslDoc = new XslCompiledTransform();
            xslDoc.Load(ConfigurationManager.AppSettings["XSLTPath"].ToString().Trim() + "PaymentConfirmation.xslt");

            XsltArgumentList xslArgs = new XsltArgumentList();
            StringWriter writer = new StringWriter();
            xslDoc.Transform(xmlDoc, xslArgs, writer);

            string mailBody = writer.ToString();

            mailBody = mailBody.Replace("@Model.HeaderLogo", ConfigurationManager.AppSettings["MailerHeaderLogo"].ToString().Trim());

            return mailBody;
        }
    }
}