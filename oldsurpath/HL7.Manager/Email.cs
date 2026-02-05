using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using SurPath.Data;
using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Xml;

namespace HL7.Manager
{
    public class Email
    {
        private string FromAddress;
        private string strToAddress;
        private string strSmtpClient;
        private string UserID;
        private string Password;
        private string ReplyTo;
        private string SMTPPort;
        private Boolean bEnableSSL;
        private NetworkCredential basicCredential;

        private void InitMail()
        {
            FromAddress = System.Configuration.ConfigurationManager.AppSettings.Get("FromAddress");
            strToAddress = System.Configuration.ConfigurationManager.AppSettings.Get("ToAddress");
            strSmtpClient = System.Configuration.ConfigurationManager.AppSettings.Get("SMTPHost");
            ReplyTo = System.Configuration.ConfigurationManager.AppSettings.Get("ReplyTo");

            UserID = System.Configuration.ConfigurationManager.AppSettings.Get("UserID");
            Password = System.Configuration.ConfigurationManager.AppSettings.Get("Password");
            basicCredential = new NetworkCredential(UserID.Trim(), Password.Trim());
            
            SMTPPort = System.Configuration.ConfigurationManager.AppSettings.Get("SMTPPort");
            if (System.Configuration.ConfigurationManager.AppSettings.Get("EnableSSL").ToUpper() == "YES")
            {
                bEnableSSL = true;
            }
            else
            {
                bEnableSSL = false;
            }
        }

        //public string GetGeneratedPDF(string reportId)
        //{
        //    ReportInfo rtInfo = new ReportInfo();
        //    HL7ParserBL hl7Parser = new HL7ParserBL();
        //    rtInfo = hl7Parser.GetReportDetailsById(reportId, rtInfo);

        //    rtInfo.LabReport = rtInfo.LabReport.Replace(@"\line", "");
        //    rtInfo.LabReport = rtInfo.LabReport.Replace(@"{", "");
        //    rtInfo.LabReport = rtInfo.LabReport.Replace(@"}", "");

        //    return rtInfo.LabReport.ToString();

        //}

        public static String GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }

        public void SendMail(string mismatchedIds, string[] param, string resultFileName)
        {
            XmlDocument xdoc = new XmlDocument();
            string mailFormatxml = ConfigurationManager.AppSettings["MailFormat"].ToString().Trim() + "MailFormat.xml";
            string messageId = "Subject";
            string subject = System.Configuration.ConfigurationManager.AppSettings.Get("MissMatchedReportSubject");
            string pdfPassword = System.Configuration.ConfigurationManager.AppSettings.Get("MissMatchedPdfPassword");
            string body = "";
            XmlNode mailNode = default(XmlNode);
            int n = 0;

            if ((System.IO.File.Exists(mailFormatxml)))
            {
                xdoc.Load(mailFormatxml);
                mailNode = xdoc.SelectSingleNode("MailFormats/MailFormat[@Id='" + messageId + "']");
                //subject = mailNode.SelectSingleNode("Subject").InnerText;
                body = mailNode.SelectSingleNode("Body").InnerText;
                if ((param == null))
                {
                    throw new Exception("Mail format file not found.");
                }
                else
                {
                    for (n = 0; n <= param.Length - 1; n++)
                    {
                        body = body.Replace(n.ToString() + "?", param[n]);
                        subject = subject.Replace(n.ToString() + "?", param[n]);
                    }
                }

                InitMail();

                HL7ParserDao hl7Parser = new HL7ParserDao();
                //rtInfo = hl7Parser.GetReportDetailsById(reportId, rtInfo);

                DataTable dt = hl7Parser.GetMismatchedData(mismatchedIds);
                Document document = new Document(PageSize.A4, 88f, 88f, 10f, 10f);
                Font NormalFont = FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK);

                DateTime dateTime = DateTime.Now;
                //Building an HTML string.
                StringBuilder html = new StringBuilder();

                html.Append("<html><body><div style='text-align: center'>    <img src=' " + System.Configuration.ConfigurationManager.AppSettings.Get("ProductLogo") + "' alt=''> </div>");
                html.Append("<div style='text-align: center; margin-top:20px; padding-right:20px;'> <p style='font-size: large; font-family:calibri; font-weight: 500; color:#00579b; '> Mismatched reports details</p> </div>");
                html.Append("<div style='text-align:center; padding-right:20px;' >");
                html.Append("<p style='font-family:calibri'; font-size:large; font-weight:500; color:#00579b'> Date : <span style='color:#cd2028;'>  " + dateTime.ToString("MM/dd/yyyy") + " </span> </p>");
                html.Append("<p style='font-family:calibri'; font-size:large; font-weight:500; color:#00579b'> Time : <span style='color:#cd2028;'> " + dateTime.ToString("HH:mm:ss") + " </span> </p> </div>");
                html.Append("<div> <br /> </div>");
                //Table start.
                html.Append("<table border='1'>");

                //Building the Header row.
                html.Append("<tr bgcolor: '#ff9800;' style='text-align: center'>");
                foreach (DataColumn column in dt.Columns)
                {
                    html.Append("<th style='padding-left: 8px; padding-bottom: 5px;'>");
                    if (column.ColumnName == "SpecimenId")
                    {
                        html.Append("Specimen ID");
                    }
                    else if (column.ColumnName == "DonorName")
                    {
                        html.Append("Donor Name");
                    }
                    else if (column.ColumnName == "ClientCode")
                    {
                        html.Append("Client");
                    }
                    else if (column.ColumnName == "DateOfTest")
                    {
                        html.Append("Date of Test");
                    }
                    else if (column.ColumnName == "SSNId")
                    {
                        html.Append("SSN");
                    }
                    else if (column.ColumnName == "DonorDOB")
                    {
                        html.Append("DoB");
                    }

                    html.Append("</th>");
                }
                html.Append("</tr>");

                //Building the Data rows.
                int j = 0;
                foreach (DataRow row in dt.Rows)
                {
                    string rowColor = "#fff9c4";

                    if (j % 2 == 0)
                    {
                        rowColor = "#ffcc80";
                    }
                    html.Append("<tr bgcolor = '" + rowColor + ";'>");
                    foreach (DataColumn column in dt.Columns)
                    {
                        html.Append("<td style='font-size: 9px;'>");
                        html.Append(row[column.ColumnName]);
                        html.Append("</td>");
                    }
                    j++;
                    html.Append("</tr>");
                }

                //Table end.
                html.Append("</table>");
                html.Append("</body>");
                html.Append("</html>");

                string lapReport = html.ToString();// GetGeneratedPDF(reportId);
                StringReader sr = new StringReader(lapReport.ToString());
                Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                //Paragraph Paragraph = new Paragraph(lapReport.ToString());

                HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                MemoryStream memoryStream = new MemoryStream();
                String timeStamp = GetTimestamp(DateTime.Now);

                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                writer.SetEncryption(PdfWriter.STRENGTH128BITS, pdfPassword, pdfPassword, PdfWriter.AllowCopy | PdfWriter.AllowPrinting);
                pdfDoc.Open();
                // Paragraph.Alignment = Element.ALIGN_JUSTIFIED_ALL;
                htmlparser.Parse(sr);
                // pdfDoc.Add(Paragraph);
                //htmlparser.Parse(sr);
                pdfDoc.Close();
                byte[] bytes = memoryStream.ToArray();
                memoryStream.Close();
                string[] mailRecipients = strToAddress.Split(',');

                dynamic MailMessage = new MailMessage();
                MailMessage.From = new MailAddress(FromAddress);
                if (mailRecipients.Length > 0)
                {
                    for (int idx = 0; idx < mailRecipients.Length; idx++)
                    {
                        MailMessage.To.Add(mailRecipients[idx]);
                    }
                }
                MailMessage.Subject = subject;
                MailMessage.IsBodyHtml = true;
                MailMessage.Body = body;
                MailMessage.Attachments.Add(new Attachment(new MemoryStream(bytes), resultFileName + timeStamp + ".pdf"));
                MailMessage.ReplyTo = new MailAddress(FromAddress);

                SmtpClient SmtpClient = new SmtpClient();
                SmtpClient.Host = strSmtpClient;
                SmtpClient.EnableSsl = bEnableSSL;
                SmtpClient.Port = Convert.ToInt32(SMTPPort);
                SmtpClient.UseDefaultCredentials = false;

                SmtpClient.Credentials = basicCredential; // new System.Net.NetworkCredential(UserID, Password);
                try
                {
                    SmtpClient.Send(MailMessage);
                }
                catch (SmtpFailedRecipientsException ex)
                {
                    for (int i = 0; i <= ex.InnerExceptions.Length; i++)
                    {
                        SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
                        if ((status == SmtpStatusCode.MailboxBusy) | (status == SmtpStatusCode.MailboxUnavailable))
                        {
                            System.Threading.Thread.Sleep(5000);
                            SmtpClient.Send(MailMessage);
                        }
                    }
                }
            }
        }

        public void SendMailMessage(string __body, string __subject) //, string[] param, string resultFileName)
        {
            //XmlDocument xdoc = new XmlDocument();
            //string mailFormatxml = ConfigurationManager.AppSettings["MailFormat"].ToString().Trim() + "MailFormat.xml";
            //string messageId = "Subject";
            //string subject = System.Configuration.ConfigurationManager.AppSettings.Get("MissMatchedReportSubject");
            //string pdfPassword = System.Configuration.ConfigurationManager.AppSettings.Get("MissMatchedPdfPassword");
            //string body = "";
            //XmlNode mailNode = default(XmlNode);
            //int n = 0;

            //if ((System.IO.File.Exists(mailFormatxml)))
            //{
            //    xdoc.Load(mailFormatxml);
            //    mailNode = xdoc.SelectSingleNode("MailFormats/MailFormat[@Id='" + messageId + "']");
            //    //subject = mailNode.SelectSingleNode("Subject").InnerText;
            //    body = mailNode.SelectSingleNode("Body").InnerText;
            //    if ((param == null))
            //    {
            //        throw new Exception("Mail format file not found.");
            //    }
            //    else
            //    {
            //        for (n = 0; n <= param.Length - 1; n++)
            //        {
            //            body = body.Replace(n.ToString() + "?", param[n]);
            //            subject = subject.Replace(n.ToString() + "?", param[n]);
            //        }
            //    }

            InitMail();

            //    HL7ParserDao hl7Parser = new HL7ParserDao();
            //    //rtInfo = hl7Parser.GetReportDetailsById(reportId, rtInfo);

            //    DataTable dt = hl7Parser.GetMismatchedData(mismatchedIds);
            //    Document document = new Document(PageSize.A4, 88f, 88f, 10f, 10f);
            //    Font NormalFont = FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK);

            //    DateTime dateTime = DateTime.Now;
            //    //Building an HTML string.
            //    StringBuilder html = new StringBuilder();

            //    html.Append("<html><body><div style='text-align: center'>    <img src=' " + System.Configuration.ConfigurationManager.AppSettings.Get("ProductLogo") + "' alt=''> </div>");
            //    html.Append("<div style='text-align: center; margin-top:20px; padding-right:20px;'> <p style='font-size: large; font-family:calibri; font-weight: 500; color:#00579b; '> Mismatched reports details</p> </div>");
            //    html.Append("<div style='text-align:center; padding-right:20px;' >");
            //    html.Append("<p style='font-family:calibri'; font-size:large; font-weight:500; color:#00579b'> Date : <span style='color:#cd2028;'>  " + dateTime.ToString("MM/dd/yyyy") + " </span> </p>");
            //    html.Append("<p style='font-family:calibri'; font-size:large; font-weight:500; color:#00579b'> Time : <span style='color:#cd2028;'> " + dateTime.ToString("HH:mm:ss") + " </span> </p> </div>");
            //    html.Append("<div> <br /> </div>");
            //    //Table start.
            //    html.Append("<table border='1'>");

            //    //Building the Header row.
            //    html.Append("<tr bgcolor: '#ff9800;' style='text-align: center'>");
            //    foreach (DataColumn column in dt.Columns)
            //    {
            //        html.Append("<th style='padding-left: 8px; padding-bottom: 5px;'>");
            //        if (column.ColumnName == "SpecimenId")
            //        {
            //            html.Append("Specimen ID");
            //        }
            //        else if (column.ColumnName == "DonorName")
            //        {
            //            html.Append("Donor Name");
            //        }
            //        else if (column.ColumnName == "ClientCode")
            //        {
            //            html.Append("Client");
            //        }
            //        else if (column.ColumnName == "DateOfTest")
            //        {
            //            html.Append("Date of Test");
            //        }
            //        else if (column.ColumnName == "SSNId")
            //        {
            //            html.Append("SSN");
            //        }
            //        else if (column.ColumnName == "DonorDOB")
            //        {
            //            html.Append("DoB");
            //        }

            //        html.Append("</th>");
            //    }
            //    html.Append("</tr>");

            //    //Building the Data rows.
            //    int j = 0;
            //    foreach (DataRow row in dt.Rows)
            //    {
            //        string rowColor = "#fff9c4";

            //        if (j % 2 == 0)
            //        {
            //            rowColor = "#ffcc80";
            //        }
            //        html.Append("<tr bgcolor = '" + rowColor + ";'>");
            //        foreach (DataColumn column in dt.Columns)
            //        {
            //            html.Append("<td style='font-size: 9px;'>");
            //            html.Append(row[column.ColumnName]);
            //            html.Append("</td>");
            //        }
            //        j++;
            //        html.Append("</tr>");
            //    }

            //    //Table end.
            //    html.Append("</table>");
            //    html.Append("</body>");
            //    html.Append("</html>");

            //    string lapReport = html.ToString();// GetGeneratedPDF(reportId);
            //    StringReader sr = new StringReader(lapReport.ToString());
            //    Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
            //    //Paragraph Paragraph = new Paragraph(lapReport.ToString());

            //    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            //    MemoryStream memoryStream = new MemoryStream();
            //    String timeStamp = GetTimestamp(DateTime.Now);

            //    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
            //    writer.SetEncryption(PdfWriter.STRENGTH128BITS, pdfPassword, pdfPassword, PdfWriter.AllowCopy | PdfWriter.AllowPrinting);
            //    pdfDoc.Open();
            //    // Paragraph.Alignment = Element.ALIGN_JUSTIFIED_ALL;
            //    htmlparser.Parse(sr);
            //    // pdfDoc.Add(Paragraph);
            //    //htmlparser.Parse(sr);
            //    pdfDoc.Close();
            //    byte[] bytes = memoryStream.ToArray();
            //    memoryStream.Close();
            string[] mailRecipients = strToAddress.Split(',');

            MailMessage _MailMessage = new MailMessage();
            _MailMessage.From = new MailAddress(FromAddress);
            if (mailRecipients.Length > 0)
            {
                for (int idx = 0; idx < mailRecipients.Length; idx++)
                {
                    _MailMessage.To.Add(mailRecipients[idx]);
                }
            }
            _MailMessage.Subject = __subject;
            _MailMessage.IsBodyHtml = false;
            _MailMessage.Body = __body;
            _MailMessage.ReplyTo = new MailAddress(FromAddress);

            SmtpClient SmtpClient = new SmtpClient();
            SmtpClient.Host = strSmtpClient;
            SmtpClient.EnableSsl = bEnableSSL;
            SmtpClient.Port = Convert.ToInt32(SMTPPort);
            SmtpClient.UseDefaultCredentials = false;

            SmtpClient.Credentials = basicCredential; // new System.Net.NetworkCredential(UserID, Password);



            try
            {
                SmtpClient.Send(_MailMessage);
            }
            catch (SmtpFailedRecipientsException ex)
            {
                for (int i = 0; i <= ex.InnerExceptions.Length; i++)
                {
                    SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
                    if ((status == SmtpStatusCode.MailboxBusy) | (status == SmtpStatusCode.MailboxUnavailable))
                    {
                        System.Threading.Thread.Sleep(5000);
                        SmtpClient.Send(_MailMessage);
                    }
                }
            }

        }
        public void SendMailMessageEnhanced(string ToAddresses, string __body, string __subject, Byte[] attachmentobject = null, string attachmentmimetype = "", string attachmentname = "") //, string[] param, string resultFileName)
        {
            //XmlDocument xdoc = new XmlDocument();
            //string mailFormatxml = ConfigurationManager.AppSettings["MailFormat"].ToString().Trim() + "MailFormat.xml";
            //string messageId = "Subject";
            //string subject = System.Configuration.ConfigurationManager.AppSettings.Get("MissMatchedReportSubject");
            //string pdfPassword = System.Configuration.ConfigurationManager.AppSettings.Get("MissMatchedPdfPassword");
            //string body = "";
            //XmlNode mailNode = default(XmlNode);
            //int n = 0;

            //if ((System.IO.File.Exists(mailFormatxml)))
            //{
            //    xdoc.Load(mailFormatxml);
            //    mailNode = xdoc.SelectSingleNode("MailFormats/MailFormat[@Id='" + messageId + "']");
            //    //subject = mailNode.SelectSingleNode("Subject").InnerText;
            //    body = mailNode.SelectSingleNode("Body").InnerText;
            //    if ((param == null))
            //    {
            //        throw new Exception("Mail format file not found.");
            //    }
            //    else
            //    {
            //        for (n = 0; n <= param.Length - 1; n++)
            //        {
            //            body = body.Replace(n.ToString() + "?", param[n]);
            //            subject = subject.Replace(n.ToString() + "?", param[n]);
            //        }
            //    }

            InitMail();

            //    HL7ParserDao hl7Parser = new HL7ParserDao();
            //    //rtInfo = hl7Parser.GetReportDetailsById(reportId, rtInfo);

            //    DataTable dt = hl7Parser.GetMismatchedData(mismatchedIds);
            //    Document document = new Document(PageSize.A4, 88f, 88f, 10f, 10f);
            //    Font NormalFont = FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK);

            //    DateTime dateTime = DateTime.Now;
            //    //Building an HTML string.
            //    StringBuilder html = new StringBuilder();

            //    html.Append("<html><body><div style='text-align: center'>    <img src=' " + System.Configuration.ConfigurationManager.AppSettings.Get("ProductLogo") + "' alt=''> </div>");
            //    html.Append("<div style='text-align: center; margin-top:20px; padding-right:20px;'> <p style='font-size: large; font-family:calibri; font-weight: 500; color:#00579b; '> Mismatched reports details</p> </div>");
            //    html.Append("<div style='text-align:center; padding-right:20px;' >");
            //    html.Append("<p style='font-family:calibri'; font-size:large; font-weight:500; color:#00579b'> Date : <span style='color:#cd2028;'>  " + dateTime.ToString("MM/dd/yyyy") + " </span> </p>");
            //    html.Append("<p style='font-family:calibri'; font-size:large; font-weight:500; color:#00579b'> Time : <span style='color:#cd2028;'> " + dateTime.ToString("HH:mm:ss") + " </span> </p> </div>");
            //    html.Append("<div> <br /> </div>");
            //    //Table start.
            //    html.Append("<table border='1'>");

            //    //Building the Header row.
            //    html.Append("<tr bgcolor: '#ff9800;' style='text-align: center'>");
            //    foreach (DataColumn column in dt.Columns)
            //    {
            //        html.Append("<th style='padding-left: 8px; padding-bottom: 5px;'>");
            //        if (column.ColumnName == "SpecimenId")
            //        {
            //            html.Append("Specimen ID");
            //        }
            //        else if (column.ColumnName == "DonorName")
            //        {
            //            html.Append("Donor Name");
            //        }
            //        else if (column.ColumnName == "ClientCode")
            //        {
            //            html.Append("Client");
            //        }
            //        else if (column.ColumnName == "DateOfTest")
            //        {
            //            html.Append("Date of Test");
            //        }
            //        else if (column.ColumnName == "SSNId")
            //        {
            //            html.Append("SSN");
            //        }
            //        else if (column.ColumnName == "DonorDOB")
            //        {
            //            html.Append("DoB");
            //        }

            //        html.Append("</th>");
            //    }
            //    html.Append("</tr>");

            //    //Building the Data rows.
            //    int j = 0;
            //    foreach (DataRow row in dt.Rows)
            //    {
            //        string rowColor = "#fff9c4";

            //        if (j % 2 == 0)
            //        {
            //            rowColor = "#ffcc80";
            //        }
            //        html.Append("<tr bgcolor = '" + rowColor + ";'>");
            //        foreach (DataColumn column in dt.Columns)
            //        {
            //            html.Append("<td style='font-size: 9px;'>");
            //            html.Append(row[column.ColumnName]);
            //            html.Append("</td>");
            //        }
            //        j++;
            //        html.Append("</tr>");
            //    }

            //    //Table end.
            //    html.Append("</table>");
            //    html.Append("</body>");
            //    html.Append("</html>");

            //    string lapReport = html.ToString();// GetGeneratedPDF(reportId);
            //    StringReader sr = new StringReader(lapReport.ToString());
            //    Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
            //    //Paragraph Paragraph = new Paragraph(lapReport.ToString());

            //    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            //    MemoryStream memoryStream = new MemoryStream();
            //    String timeStamp = GetTimestamp(DateTime.Now);

            //    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
            //    writer.SetEncryption(PdfWriter.STRENGTH128BITS, pdfPassword, pdfPassword, PdfWriter.AllowCopy | PdfWriter.AllowPrinting);
            //    pdfDoc.Open();
            //    // Paragraph.Alignment = Element.ALIGN_JUSTIFIED_ALL;
            //    htmlparser.Parse(sr);
            //    // pdfDoc.Add(Paragraph);
            //    //htmlparser.Parse(sr);
            //    pdfDoc.Close();
            //    byte[] bytes = memoryStream.ToArray();
            //    memoryStream.Close();
            string[] mailRecipients = ToAddresses.Split(',');

            MailMessage _MailMessage = new MailMessage();
            _MailMessage.From = new MailAddress(FromAddress);
            if (mailRecipients.Length > 0)
            {
                for (int idx = 0; idx < mailRecipients.Length; idx++)
                {
                    _MailMessage.To.Add(mailRecipients[idx]);
                }
            }
            _MailMessage.Subject = __subject;
            _MailMessage.IsBodyHtml = false;
            _MailMessage.Body = __body;
            _MailMessage.ReplyTo = new MailAddress(FromAddress);

            if (attachmentobject != null && !string.IsNullOrEmpty(attachmentmimetype) && !string.IsNullOrEmpty(attachmentname))
            {
                // we have an attachment
                Attachment attachment = new Attachment(new MemoryStream(attachmentobject), attachmentmimetype);
                attachment.ContentDisposition.FileName = attachmentname;
                _MailMessage.Attachments.Add(attachment);
            }

            SmtpClient SmtpClient = new SmtpClient();
            SmtpClient.Host = strSmtpClient;
            SmtpClient.EnableSsl = bEnableSSL;
            SmtpClient.Port = Convert.ToInt32(SMTPPort);
            SmtpClient.UseDefaultCredentials = false;

            SmtpClient.Credentials = basicCredential; // new System.Net.NetworkCredential(UserID, Password);



            try
            {
                SmtpClient.Send(_MailMessage);
            }
            catch (SmtpFailedRecipientsException ex)
            {
                for (int i = 0; i <= ex.InnerExceptions.Length; i++)
                {
                    SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
                    if ((status == SmtpStatusCode.MailboxBusy) | (status == SmtpStatusCode.MailboxUnavailable))
                    {
                        System.Threading.Thread.Sleep(5000);
                        SmtpClient.Send(_MailMessage);
                    }
                }
            }

        }


        public void SendMailAttachment(string[] param, string fileInfo, string Recipients)
        {
            try
            {
                InitMail();

                Font NormalFont = FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK);
                DateTime dateTime = DateTime.Now;
                Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);

                //string strFileName = Path.GetFileName(fileInfo);

                //using (MemoryStream ms = new MemoryStream())
                //{
                //    Document doc = new Document();
                //    PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(Path.Combine(fileInfo), FileMode.Create));
                //    doc.AddTitle("Document Title");
                //    doc.Open();
                //    doc.Add(new Paragraph("Mismatched Info"));
                //    doc.Close();
                //}

                MemoryStream ms = new MemoryStream();
                using (FileStream fs = File.Open(fileInfo, FileMode.Open, FileAccess.Read))
                {
                    fs.CopyTo(ms);
                }
                byte[] bytes = ms.ToArray();
                var sr = new StreamReader(ms);
                var myStr = sr.ReadToEnd();

                Paragraph Paragraph = new Paragraph(myStr);
                String timeStamp = GetTimestamp(DateTime.Now);
                MemoryStream memoryStream = new MemoryStream();

                //PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);

                //PdfWriter writer = PdfWriter.GetInstance(pdfDoc, new FileStream(fileInfo, FileMode.Open));
                //pdfDoc.Open();
                //Paragraph.Alignment = Element.ALIGN_JUSTIFIED_ALL;

                //pdfDoc.Add(Paragraph);
                //pdfDoc.Close();
                ////
                //byte[] bytes2 = memoryStream.ToArray();
                //memoryStream.Close();
                string[] mailRecipients = strToAddress.Split(',');

                dynamic MailMessage = new MailMessage();
                MailMessage.From = new MailAddress(FromAddress);
                if (mailRecipients.Length > 0)
                {
                    for (int idx = 0; idx < mailRecipients.Length; idx++)
                    {
                        MailMessage.To.Add(mailRecipients[idx]);
                    }
                }
                MailMessage.Subject = "Mismatched Report";
                MailMessage.IsBodyHtml = true;
                MailMessage.Body = "Mismatched Report";
                MailMessage.Attachments.Add(new Attachment(new MemoryStream(bytes), "MismatchFile" + timeStamp + ".txt"));
                MailMessage.ReplyTo = new MailAddress(FromAddress);

                SmtpClient SmtpClient = new SmtpClient();
                SmtpClient.Host = strSmtpClient;
                SmtpClient.EnableSsl = bEnableSSL;
                SmtpClient.Port = Convert.ToInt32(SMTPPort);
                SmtpClient.UseDefaultCredentials = false;

                SmtpClient.Credentials = basicCredential; // new System.Net.NetworkCredential(UserID, Password);
                try
                {
                    SmtpClient.Send(MailMessage);
                }
                catch (SmtpFailedRecipientsException ex)
                {
                    for (int i = 0; i <= ex.InnerExceptions.Length; i++)
                    {
                        SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
                        if ((status == SmtpStatusCode.MailboxBusy) | (status == SmtpStatusCode.MailboxUnavailable))
                        {
                            System.Threading.Thread.Sleep(5000);
                            SmtpClient.Send(MailMessage);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Mail Error" + ex.Message);
                throw;
            }
        }
    }
}