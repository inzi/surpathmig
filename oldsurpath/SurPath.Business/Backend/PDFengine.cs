using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using Serilog;
using SurPath.Business;
using SurPath.Business.Backend.PdfTextract;
using SurPath.Data;
using SurPath.Data.Backend;
using SurPath.Entity;
using SurPath.Enum;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading;

namespace SurpathBackend
{
    public class PDFengine : IPDEngine
    {

        //public bool FormFoxEnabled = true;
        public string PDFBase64 = string.Empty;
        public List<string> FormFoxEnabledDeptIDs = new List<string>();
        //string appSectionGroup = "PDFEngine/PDFEngineSettings";
        private string appSectionGroup = "PDFEngine/PDFEngineSettings";

        private string EngineSettingsFileName = "PDFEngineSettingsGlobal.json";

        public List<ClientNotificationDataSettings> AllClientDataSettings { get; set; } = new List<ClientNotificationDataSettings>();

        public PDFEngineSettings EngineSettings { get; set; } = new PDFEngineSettings();
        public PdfRenderSettings RenderSettings { get; set; } = new PdfRenderSettings();
        public PdfRenderSettings DefaultRenderSettingsObject { get; set; } = new PdfRenderSettings();
        public string CurrentTemplate { get; set; } = String.Empty;
        public string PDFTemplateFolder { get; set; } = String.Empty;
        public string DefaultTemplate { get; set; } = String.Empty;
        public string PDFConfigFolder { get; set; } = String.Empty;
        public string MROName { get; set; } = string.Empty;
        public string DefaultRenderSettingsFile { get; set; } = String.Empty;

        //public string JSON_PDF_Services_To_Test_Mapping { get; set; } = String.Empty;
        private BackendData backendData;

        private BackendFiles backendFiles;

        public PdfDocument CurrentDocument = new PdfDocument();
        public PdfPage CurrentPDFPage;
        private XGraphics gfx;
        private string _loadedSettings = string.Empty;

        private bool Ready = false;
        private string NotReadyMsg = "PDFEngine Not Ready. Double Check settings in config.";
        public string libPath { get; set; } = string.Empty;
        private static ILogger _logger;

        private string FormFoxTestLocation = string.Empty;
        private string FormFoxTestLabCode = string.Empty;
        private bool FormFoxSendAddressOnCreateOrder = false;
        private bool FormFoxAutoFallbackToDatabase = false;
        private int FormFoxASAPHoursDeadline = 0;
        private int FFMPSearchSearchType = 1;
        private int FormFoxSleepAfterOrderAuthFormSleepInSecs = 0;
        private bool FormFoxCancelOrderOnFailure = false;
        public string FormFoxValidAuthFormSearchString = string.Empty;
        public CultureInfo culture = new CultureInfo(ConfigurationManager.AppSettings["Culture"].ToString().Trim());

        public SenderTracker ThisSenderTracker = new SenderTracker();
        public bool WasAnIssue = false;
        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        public PDFengine(ILogger __logger = null)
        {
            if (__logger != null)
            {
                _logger = __logger;
                _logger.Debug("PDFengine logger online");
                _logger.Debug("Path to this lib: " + AssemblyDirectory);
            }
            else
            {
                _logger = new LoggerConfiguration().CreateLogger();
            }
            backendFiles = new BackendFiles();
            // PDFTemplateFolder = ConfigurationManager.AppSettings["PDFTemplateFolder"];
            LoadSettings();
            _logger.Debug("LoadSettings Complete, FormFoxEnabledDeptIDs being parsed");

            var _FormFoxEnabledDeptIDs = ConfigurationManager.AppSettings["FormFoxEnabledDeptIDs"].ToString().Trim();
            if (string.IsNullOrEmpty(_FormFoxEnabledDeptIDs) == false)
            {
                string[] _ffeIds = _FormFoxEnabledDeptIDs.Split(',');
                foreach (string __ffeId in _ffeIds)
                {
                    this.FormFoxEnabledDeptIDs.Add(__ffeId.ToUpper().Trim());
                }
            }

            //this.FormFoxEnabled = true;
            //bool.TryParse(ConfigurationManager.AppSettings["FormFoxEnabled"].ToString().Trim(), out this.FormFoxEnabled);

            this.FormFoxTestLocation = ConfigurationManager.AppSettings["FormFoxTestLocation"].ToString().Trim();
            this.FormFoxTestLabCode = ConfigurationManager.AppSettings["FormFoxTestLabCode"].ToString().Trim();
            bool _FormFoxSendAddressOnCreateOrder = this.FormFoxSendAddressOnCreateOrder;
            bool.TryParse(ConfigurationManager.AppSettings["FormFoxSendAddressOnCreateOrder"].ToString().Trim(), out _FormFoxSendAddressOnCreateOrder);
            this.FormFoxSendAddressOnCreateOrder = _FormFoxSendAddressOnCreateOrder;

            bool _FormFoxAutoFallbackToDatabase = this.FormFoxAutoFallbackToDatabase;
            bool.TryParse(ConfigurationManager.AppSettings["FormFoxAutoFallbackToDatabase"].ToString().Trim(), out _FormFoxAutoFallbackToDatabase);
            this.FormFoxAutoFallbackToDatabase = _FormFoxAutoFallbackToDatabase;

            //FormFoxCancelOrderOnFailure
            bool _FormFoxCancelOrderOnFailure = this.FormFoxCancelOrderOnFailure;
            bool.TryParse(ConfigurationManager.AppSettings["FormFoxCancelOrderOnFailure"].ToString().Trim(), out _FormFoxCancelOrderOnFailure);
            this.FormFoxCancelOrderOnFailure = _FormFoxCancelOrderOnFailure;

            int _FormFoxASAPHoursDeadline = this.FormFoxASAPHoursDeadline;
            int.TryParse(ConfigurationManager.AppSettings["FormFoxASAPHoursDeadline"].ToString().Trim(), out _FormFoxASAPHoursDeadline);
            this.FormFoxASAPHoursDeadline = _FormFoxASAPHoursDeadline;


            int _FormFoxSleepAfterOrderAuthFormSleepInSecs = this.FormFoxSleepAfterOrderAuthFormSleepInSecs;
            int.TryParse(ConfigurationManager.AppSettings["FormFoxSleepAfterOrderAuthFormSleepInSecs"].ToString().Trim(), out _FormFoxSleepAfterOrderAuthFormSleepInSecs);
            this.FormFoxSleepAfterOrderAuthFormSleepInSecs = _FormFoxSleepAfterOrderAuthFormSleepInSecs;

            _logger.Debug($"DELAY TEST CALCULATION");
            _logger.Debug($"FormFoxSleepAfterOrderAuthFormSleepInSecs setting {this.FormFoxSleepAfterOrderAuthFormSleepInSecs}");
            int _Delay = this.FormFoxSleepAfterOrderAuthFormSleepInSecs * 1000;
            _logger.Debug($"_Delay calculated as {_Delay} ms");
            if (_Delay < 1) _Delay = 250;
            _logger.Debug($"_Delay would be {_Delay} ms");



            this.FormFoxValidAuthFormSearchString = ConfigurationManager.AppSettings["FormFoxValidAuthFormSearchString"].ToString().Trim();


            this.libPath = Path.GetDirectoryName(
                     Assembly.GetAssembly(typeof(PDFengine)).CodeBase);
        }

        public Byte[] CurrentDocumentAsMemoryStreamBytes()
        {
            if (!Ready) throw new Exception(NotReadyMsg);
            // If we're using FormFox, we will download the PDF instead of rendering it.
            //if (this.FormFoxEnabled == false)
            //{

            //    MemoryStream memoryStream = new MemoryStream();
            //    CurrentDocument.Save(memoryStream, true);
            //    //memoryStream.Seek(0, SeekOrigin.Begin);
            //    //memoryStream.Flush();

            //    return memoryStream.ToArray();
            //}
            //else
            //{
            return Convert.FromBase64String(this.PDFBase64);
            //}
        }

        public bool LoadPDFConfig(string _ConfigName)
        {
            //// Always load from file so manual send ins work.
            //// If using FormFox, return True
            //if (this.FormFoxEnabled == true)
            //{
            //    Ready = true;
            //    return Ready;
            //}

            bool success = false;
            try
            {
                LoadConfigFromFile(_ConfigName);

                success = true;
            }
            catch (Exception)
            {
                success = false;
            }
            return success;
        }

        public bool LoadTemplate(string _TemplateName = null)
        {
            //// If using FormFox, return True
            //if (this.FormFoxEnabled==true)
            //{
            //    Ready = true;
            //    return Ready;
            //}

            if (string.IsNullOrEmpty(_TemplateName)) _TemplateName = EngineSettings.Settings["DefaultTemplate"];
            if (string.IsNullOrEmpty(_TemplateName)) throw new Exception("No Default Template defined!");
            _logger.Debug($"Setting _TemplateName { _TemplateName}");

            try
            {
                _logger.Debug($"Loading { _TemplateName}");

                LoadTemplateFromFile(_TemplateName);

                CurrentPDFPage = CurrentDocument.Pages[0];

                gfx = XGraphics.FromPdfPage(CurrentPDFPage);

                Ready = true;
            }
            catch (Exception ex)
            {
                _logger.Error("PDF Engine Not Ready! Error!");
                _logger.Error(ex.Message);
                _logger.Error(ex.InnerException.ToString());

                Ready = false;
            }
            return Ready;
        }

        public object RenderTemplate(object Template, object Data)
        {
            throw new NotImplementedException();
        }

        public object SaveTemplate(object Template, string TemplateName)
        {
            throw new NotImplementedException();
        }

        public int GetTextHeight(string text, PDFRenderElement pDFRenderElement)
        {
            if (!Ready) throw new Exception(NotReadyMsg);

            XFont pdfFont = new XFont(pDFRenderElement.FontName, pDFRenderElement.FontSize);
            XSize xSize = gfx.MeasureString(text, pdfFont);
            return (int)xSize.Height;
        }

        public int GetTextWidth(string text, PDFRenderElement pDFRenderElement)
        {
            if (!Ready) throw new Exception(NotReadyMsg);

            XFont pdfFont = new XFont(pDFRenderElement.FontName, pDFRenderElement.FontSize);
            XSize xSize = gfx.MeasureString(text, pdfFont);
            return (int)xSize.Width;
        }

        public int[] GetTextSize(PDFRenderElement pDFRenderElement, string text = "")
        {
            if (!Ready) throw new Exception(NotReadyMsg);
            string testContent = text;
            if (string.IsNullOrEmpty(testContent)) testContent = pDFRenderElement.Text;

            XFont pdfFont = new XFont(pDFRenderElement.FontName, pDFRenderElement.FontSize);
            XSize xSize = gfx.MeasureString(testContent, pdfFont);
            return new int[] { (int)xSize.Width, (int)xSize.Height };
        }

        public int[] GetElementContentSize(PDFRenderElement pDFRenderElement, string text = "")
        {
            int[] retval = { 0, 0 };
            if (!Ready) throw new Exception(NotReadyMsg);
            string testContent = text;
            if (string.IsNullOrEmpty(testContent)) testContent = pDFRenderElement.Text;

            XFont font;
            if (pDFRenderElement.Bold)
            {
                font = new XFont(pDFRenderElement.FontName, pDFRenderElement.FontSize, XFontStyle.Bold);
            }
            else
            {
                font = new XFont(pDFRenderElement.FontName, pDFRenderElement.FontSize, XFontStyle.Regular);
            }

            //var lines = text.Split('\n');
            var lines = testContent.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            double totalHeight = 0;
            double maxWidth = 0;

            foreach (string line in lines)
            {
                XSize size = gfx.MeasureString(line, font);
                // double height = size.Height + (size.Height * Math.Floor(size.Width / width));
                totalHeight += size.Height;
                if (maxWidth < size.Width) maxWidth = size.Width;
            }
            retval[0] = (int)maxWidth;
            retval[1] = (int)totalHeight;

            return retval;
        }

        public void RenderElementOntoPDF(PDFRenderElement pDFRenderElement)
        {
            if (!Ready) throw new Exception(NotReadyMsg);

            int fontsize = pDFRenderElement.FontSize;
            string fontname = pDFRenderElement.FontName;

            double top = (double)pDFRenderElement.Top;
            double left = (double)pDFRenderElement.Left;

            double width = (double)pDFRenderElement.Width; // (double)CurrentPDFPage.Width - left;

            double height = (double)pDFRenderElement.Height; // (double)CurrentPDFPage.Height - top;

            bool bDrawBox = (bool)pDFRenderElement.DrawBox;
            bool bDrawBoxSolid = (bool)pDFRenderElement.DrawBoxSolid;

            double BoxPadding = (double)pDFRenderElement.BoxPadding;

            XBrush TextBrushColor = new XSolidBrush(XColor.FromArgb(pDFRenderElement.TextColorRed, pDFRenderElement.TextColorGreen, pDFRenderElement.TextColorBlue));
            XBrush BoxBrushColor = new XSolidBrush(XColor.FromArgb(pDFRenderElement.BoxColorRed, pDFRenderElement.BoxColorGreen, pDFRenderElement.BoxColorBlue));

            XFont font;
            if (pDFRenderElement.Bold)
            {
                font = new XFont(fontname, fontsize, XFontStyle.Bold);
            }
            else
            {
                font = new XFont(fontname, fontsize, XFontStyle.Regular);
            }

            XTextFormatter tf = new XTextFormatter(gfx);

            XStringFormat format = new XStringFormat();
            format.LineAlignment = XLineAlignment.Near;
            format.Alignment = XStringAlignment.Near;

            XRect rect = new XRect(left, top, width, height);

            tf.Alignment = XParagraphAlignment.Justify;

            if (bDrawBox)
            {
                XRect boxRect = new XRect(left - BoxPadding / 2, top - BoxPadding / 2, width + BoxPadding, height + BoxPadding);

                XPen xpen;
                if (bDrawBoxSolid)
                {
                    var brush = new XSolidBrush(XColor.FromArgb(pDFRenderElement.BoxAlpha, pDFRenderElement.BoxColorRed, pDFRenderElement.BoxColorGreen, pDFRenderElement.BoxColorBlue));
                    gfx.DrawRectangle(brush, boxRect);
                }
                else
                {
                    xpen = new XPen(XColor.FromArgb(pDFRenderElement.BoxColorRed, pDFRenderElement.BoxColorGreen, pDFRenderElement.BoxColorBlue), 0.4);
                    gfx.DrawRectangle(xpen, boxRect);
                }
            }

            //if (backgroundbox)
            //{
            //    XRect bgbox_rect = new XRect((double)left, (double)top, (double)width, (double)height);
            //    var brush = new XSolidBrush(XColor.FromArgb(255, 255, 255, 255));
            //

            //}

            tf.DrawString(pDFRenderElement.Text, font, TextBrushColor, rect, format);
        }

        public void DrawLine(int x1, int y1, int x2, int y2, int alpha = 255)
        {
            XPen pen = new XPen(XColor.FromArgb(alpha, 50, 200, 100), 0.4);
            gfx.DrawLine(pen, x1, y1, x2, y2);
        }

        public void DrawRectable(int top, int left, int width, int height, int alpha = 255)
        {
            XRect rect = new XRect((double)left, (double)top, (double)width, (double)height);
            var brush = new XSolidBrush(XColor.FromArgb(alpha, 50, 200, 100));
            gfx.DrawRectangle(brush, rect);

            //gfx.DrawRectangle()
            //gfx.DrawRectangle(XBrushes.SeaGreen, rect);

            //BeginBox(gfx, number, "DrawPath (closed)");

            //XPen pen = new XPen(XColors.Navy, Math.PI);
            //pen.DashStyle = XDashStyle.Dash;

            //XGraphicsPath path = new XGraphicsPath();
            //path.AddLine(10, 120, 50, 60);
            //path.AddArc(50, 20, 110, 80, 180, 180);
            //path.AddLine(160, 60, 220, 100);
            //path.CloseFigure();
            //gfx.DrawPath(pen, path);

            //EndBox(gfx);
        }

        public void WriteOntoPDF(string StringToWrite)
        {
            if (!Ready) throw new Exception(NotReadyMsg);

            CurrentPDFPage = CurrentDocument.Pages[0];

            gfx = XGraphics.FromPdfPage(CurrentPDFPage);

            int fontsize = GetIntSetting("FontSize", 20);
            string fontname = GetStringSetting("FontName", "Verdana");

            double top = (double)GetIntSetting("TopOfList");
            double left = (double)GetIntSetting("LeftOfList");

            double width = (double)GetIntSetting("MaxListWidth"); // (double)CurrentPDFPage.Width - left;

            double height = (double)GetIntSetting("MaxListHeight"); // (double)CurrentPDFPage.Height - top;

            bool bDrawBox = (bool)GetBoolSetting("DrawBox", false);

            double BoxPadding = (double)GetIntSetting("BoxPadding", 6);

            //XFont font = new XFont(fontname, fontsize, XFontStyle.Bold);

            XFont font = new XFont(fontname, fontsize, XFontStyle.Regular);

            XTextFormatter tf = new XTextFormatter(gfx);

            XStringFormat format = new XStringFormat();
            format.LineAlignment = XLineAlignment.Near;
            format.Alignment = XStringAlignment.Near;

            XPen xpen = new XPen(XColors.Navy, 0.4);

            XRect rect = new XRect(left, top, width, height);

            tf.Alignment = XParagraphAlignment.Justify;

            //gfx.DrawRectangle(XBrushes.SeaGreen, rect);
            if (bDrawBox)
            {
                XRect penrect = new XRect(left - BoxPadding / 2, top - BoxPadding / 2, width + BoxPadding, height + BoxPadding);

                gfx.DrawRectangle(xpen, penrect);
            }

            tf.DrawString(StringToWrite, font, XBrushes.Black, rect, format);

            //gfx.DrawString(StringToWrite, font, XBrushes.Black, new XRect(top, left , CurrentPDFPage.Width, CurrentPDFPage.Height), XStringFormats.Center);
        }

        public byte[] StringToPDF(string StringToWrite, string Title)
        {
            PdfDocument tempDocument = new PdfDocument();

            LayoutHelper helper = new LayoutHelper(tempDocument, XUnit.FromInch(.25), XUnit.FromInch(11 - .25));
            XUnit left = XUnit.FromInch(.25);


            //int linesleft = totalLines;
            const int normalFontSize = 8;

            tempDocument.Info.Title = Title;
            PdfPage pdfPage = tempDocument.AddPage();
            XGraphics graph = XGraphics.FromPdfPage(pdfPage);

            XFont fontNormal = new XFont("Lucida Console", normalFontSize, XFontStyle.Regular);

            //bool washeader = false;

            string[] lines = StringToWrite.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            foreach (string line in lines)
            {

                XUnit top = helper.GetLinePosition(normalFontSize + 2, normalFontSize);

                helper.Gfx.DrawString(line, fontNormal, XBrushes.Black, left, top, XStringFormats.TopLeft);
            }


            MemoryStream memoryStream = new MemoryStream();
            tempDocument.Save(memoryStream, true);

            return memoryStream.ToArray();
        }

        //======================

        private int GetIntSetting(string name, int defaultvalue = 0)
        {
            int retval = 20;
            int.TryParse(EngineSettings.Settings[name], out retval);
            if (retval < 2) retval = defaultvalue;
            return retval;
        }

        private string GetStringSetting(string name, string defaultvalue = "")
        {
            string retval = defaultvalue;
            try
            {
                retval = EngineSettings.Settings[name];
            }
            catch (Exception ex)
            {
            }
            if (string.IsNullOrEmpty(retval)) retval = defaultvalue;
            return retval;
        }

        private bool GetBoolSetting(string name, bool defaultvalue)
        {
            bool retval = defaultvalue;
            try
            {
                bool.TryParse(EngineSettings.Settings[name], out retval);
            }
            catch (Exception ex)
            {
            }
            return retval;
        }

        /// <summary>
        /// The default method for populating a PDF template for a donor
        /// </summary>
        /// <param name="NotificationInfo"></param>
        /// <param name="notification"></param>
        /// <returns></returns>
        public bool PopulateRenderFromNotificationData(NotificationInformation NotificationInfo, Notification notification)
        {
            bool retval = false;
            try
            {
                List<ClientNotificationDataSettings> ln = new List<ClientNotificationDataSettings>();
                this.AllClientDataSettings = backendData.GetAllClientNotificationDataSettings();
                if (this.AllClientDataSettings.Where(x => x.client_department_id == NotificationInfo.client_department_id && x.client_id == NotificationInfo.client_id).ToList().Count == 0)
                {
                    // No client info for this client

                    throw new Exception($"No Client Notification Data Settings for client id: {NotificationInfo.client_id}, department id: {NotificationInfo.client_department_id}");
                }
                ClientNotificationDataSettings clientNotificationDataSettings = this.AllClientDataSettings.Where(x => x.client_department_id == NotificationInfo.client_department_id && x.client_id == NotificationInfo.client_id).First();
                retval = PopulateRenderFromNotificationData(NotificationInfo, notification, clientNotificationDataSettings, null);
                _logger.Debug($"PopulateRenderFromNotificationData returned {retval}");
            }
            catch (Exception ex)
            {
                _logger.Error("PopulateRenderFromNotificationData ERROR LOGGING");
                _logger.Error(ex.ToString());
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                throw;
            }

            return retval;
        }

        /// <summary>
        /// This overload method populates using the supplied ClientNotificationDataSettings. Can be used programatically.
        /// </summary>
        /// <param name="NotificationInfo"></param>
        /// <param name="notification"></param>
        /// <param name="clientNotificationDataSettings"></param>
        /// <returns></returns>
        public bool PopulateRenderFromNotificationData(NotificationInformation NotificationInfo, Notification notification, ClientNotificationDataSettings clientNotificationDataSettings, string RenderSettingsJson = "")
        {

            bool retval = false;
            bool ffSiteFound = false;
            bool _FormFoxSendInSuccessful = false;
            SenderTracker senderTracker = new SenderTracker(_logger);
            this.PDFBase64 = string.Empty;

            // FormFoxAutoFallbackToDatabase 
            try
            {
                _logger.Debug("PopulateRenderFromNotificationData");
                //TODO - we need a flag on this function and if the donor is sent in via the GUI, we use the OLD system - NOT FORMFOX

                _logger.Debug($"{clientNotificationDataSettings.client_name} use_formox? {clientNotificationDataSettings.use_formfox.ToString()}");
                bool useFormFoxThisPass = clientNotificationDataSettings.use_formfox;
                _logger.Debug($"FormFoxAutoFallbackToDatabase = {this.FormFoxAutoFallbackToDatabase}");
                bool _OverRideForceDB = this.FormFoxAutoFallbackToDatabase == true && clientNotificationDataSettings.use_formfox == true;
                _logger.Debug($"_OverRideForceDB is {_OverRideForceDB.ToString()}");

                _logger.Debug($"This notification set to force_db? {notification.force_db.ToString()}");
                if (notification.force_db == true) useFormFoxThisPass = false;

                _logger.Debug($"PopulateRenderFromNotificationData useFormFoxThisPass {useFormFoxThisPass}");
                DateTime OrderDeadline = DateTime.Now;
                WasAnIssue = false;
                try
                {
                    if (useFormFoxThisPass == true || _OverRideForceDB == true)
                    {
                        // If we're using formfox - we get locations from them, and download their PDF, save that, and send it.
                        // we keep the old logic for manual send ins.
                        FFCreateOrder ffco = new FFCreateOrder(_logger);

                        int _FormFoxFailureReason = 0;

                        // FFMPSearch ffApi = new FFMPSearch(_logger);
                        //ffApi.PullSites()
                        DonorBL donorBL = new DonorBL(_logger);
                        Donor donor = donorBL.Get(NotificationInfo.donor_id, "backend");
                        _logger.Debug($"PopulateRenderFromNotificationData Pulling sites");
                        var _dist = RenderSettings.Max_Clinic_Distance;
                        // this expands the search radius if set via exceptions
                        if (notification.clinic_radius > _dist)
                        {
                            _logger.Debug($"Notification radius is more than defaults, using that value instead");
                            _dist = notification.clinic_radius;
                        }
                        else
                        {
                            _logger.Debug($"Notification radius is the same or less than defaults, using department defaults");
                        }
                        //ffApi.PullSites(donor.DonorAddress1, donor.DonorCity, donor.DonorState, donor.DonorZip, _dist);
                        //if (notification.clinic_radius > _dist) _dist = notification.clinic_radius;
                        senderTracker.AddData(DateTime.Now.ToString());
                        senderTracker.AddData($"Donor ID: {NotificationInfo.donor_id} Donor Test Info ID: {NotificationInfo.donor_test_info_id}");
                        senderTracker.AddData($"Donor: {donor.DonorFirstName} {donor.DonorLastName}");
                        senderTracker.AddData("");

                        ffco.fFMPSearch.PullSites(donor.DonorAddress1, donor.DonorCity, donor.DonorState, donor.DonorZip, _dist, ref senderTracker);


                        //FFMPSearchResult _site = new FFMPSearchResult();
                        string _pricetier = string.Empty;
                        var _sitesFound = 0;

                        foreach (string _pt in ffco.fFMPSearch.PriceTiers)
                        {
                            if (ffco.fFMPSearch.Sites.Exists(s => s.PriceTier.Equals(_pt, StringComparison.InvariantCultureIgnoreCase)))
                            {
                                var _sitelistCount = ffco.fFMPSearch.Sites.Where(s => s.PriceTier.Equals(_pt, StringComparison.InvariantCultureIgnoreCase)).ToList().First().SiteList.Count();
                                _logger.Debug($"For {_pt} - Sites in price tier: {_sitelistCount}");
                                _sitesFound = _sitesFound + _sitelistCount; // ffco.fFMPSearch.Sites.Where(s => s.PriceTier.Equals(_pt, StringComparison.InvariantCultureIgnoreCase)).ToList().Count();
                            }

                        }

                        //foreach (string _pt in ffco.fFMPSearch.PriceTiers)
                        //{
                        //    var _count = ffco.fFMPSearch.Sites.Where(s => s.PriceTier.Equals(_pt, StringComparison.InvariantCultureIgnoreCase)).ToList().Count();
                        //    _logger.Debug($"For {_pt} - Sites in price tier: {_count}");
                        //    _sitesFound = _sitesFound + _count; // ffco.fFMPSearch.Sites.Where(s => s.PriceTier.Equals(_pt, StringComparison.InvariantCultureIgnoreCase)).ToList().Count();
                        //}
                        ffSiteFound = _sitesFound > 0;
                        _logger.Debug($"Pull Sites results - Sites Found? {ffSiteFound}");



                        string tempClientCode = string.Empty; // transient for logging
                        if (ffSiteFound == true)
                        {
                            //_site = ffco.fFMPSearch.Sites.Where(s => s.PriceTier.EndsWith(_pricetier, StringComparison.InvariantCultureIgnoreCase)).First().Closest();

                            // senderTracker.FFMPSearchResults.Add(_site);
                            //senderTracker.AddData($"Site selected: {_site.Code}");
                            _logger.Debug($"Creating order");

                            //_logger.Debug($"_site: Code: {_site.Code}  Distance: {_site.Distance} Price Level: {_pricetier}");
                            //senderTracker.AddData($"_site: Code: {_site.Code}  Distance: {_site.Distance} Price Level: {_pricetier}");
                            // place order
                            ffco.CreateOrderTest.SendingFacility = "SURSCAN";
                            ffco.CreateOrderTest.SendingFacilityTimeZone = "-5";
                            ffco.CreateOrderTest.ClientReferenceID = "FFQA";
                            ffco.CreateOrderTest.ProcessType = "P";
                            //ffco.CreateOrderTest.SendingFacilityID = "";

                            ffco.CreateOrderTest.PersonalData = new PersonalData();
                            ffco.CreateOrderTest.PersonalData.DateofBirth = donor.DonorDateOfBirth.ToString("yyyy/MM/dd");// "1980/05/01";
                            ffco.CreateOrderTest.PersonalData.PrimaryID = donor.DonorSSN;
                            ffco.CreateOrderTest.PersonalData.PrimaryIDType = "SSN";
                            ffco.CreateOrderTest.PersonalData.PersonName = new PersonName();
                            ffco.CreateOrderTest.PersonalData.PersonName.GivenName = donor.DonorFirstName;
                            ffco.CreateOrderTest.PersonalData.PersonName.FamilyName = donor.DonorLastName;
                            if (donor.DonorGender == SurPath.Enum.Gender.Male) ffco.CreateOrderTest.PersonalData.Gender.IdValue = "M";
                            if (donor.DonorGender == SurPath.Enum.Gender.Female) ffco.CreateOrderTest.PersonalData.Gender.IdValue = "F";
                            if (donor.DonorGender == SurPath.Enum.Gender.None) ffco.CreateOrderTest.PersonalData.Gender.IdValue = "";

                            ffco.CreateOrderTest.PersonalData.ContactMethod = new ContactMethod();
                            ffco.CreateOrderTest.PersonalData.ContactMethod.Telephone = new List<Telephone>();
                            ffco.CreateOrderTest.PersonalData.ContactMethod.Telephone.Add(new Telephone() { Type = "Mobile", FormattedNumber = donor.DonorPhone1 });

                            // Employer / Company info
                            ClientBL clientBL = new ClientBL();
                            Client client = clientBL.Get(NotificationInfo.client_id);
                            ClientDepartment clientDepartment = clientBL.GetClientDepartment(NotificationInfo.client_department_id);

                            ffco.CreateOrderTest.PersonalData.DemographicDetail.Company.IdName = client.ClientName;
                            ffco.CreateOrderTest.PersonalData.DemographicDetail.Company.IdValue = client.ClientCode;
                            tempClientCode = client.ClientCode;
                            ffco.CreateOrderTest.PersonalData.DemographicDetail.Company.PostalAddress.Municipality = string.Empty;
                            ffco.CreateOrderTest.PersonalData.DemographicDetail.Company.PostalAddress.PostalCode = string.Empty;
                            ffco.CreateOrderTest.PersonalData.DemographicDetail.Company.PostalAddress.Region = string.Empty;
                            ffco.CreateOrderTest.PersonalData.DemographicDetail.Company.PostalAddress.DeliveryAddress.AddressLine.Add(String.Empty);
                            ffco.CreateOrderTest.DonorNotification.AuthorizationLetter.SendToDonorMobile = "N";
                            ffco.CreateOrderTest.DonorNotification.AuthorizationLetter.SendToDonoremail = "N";
                            //

                            var _moreAddress = client.ClientAddresses
                                .Where(ca =>
                                    string.IsNullOrEmpty(ca.Address1) == false
                                    && string.IsNullOrEmpty(ca.City) == false
                                    && string.IsNullOrEmpty(ca.State) == false
                                    && string.IsNullOrEmpty(ca.ZipCode) == false
                                    && ca.AddressTypeId == AddressTypes.PhysicalAddress1
                                    ).ToList();
                            // Got Addresses?
                            ClientAddress _ca = null;
                            _logger.Debug("Checking Client Address");
                            if (_moreAddress.Count > 0)
                            {
                                //Got Address 1?
                                if (client.ClientAddresses.Exists(ca => ca.AddressTypeId == AddressTypes.PhysicalAddress1))
                                {
                                    _ca = client.ClientAddresses.Where(ca => ca.AddressTypeId == AddressTypes.PhysicalAddress1).First();
                                    _logger.Debug($"Client address found");
                                }
                            }
                            bool FormFoxContactSet = false;

                            if (this.FormFoxSendAddressOnCreateOrder == true)
                            {
                                _logger.Debug($"FormFoxSendAddressOnCreateOrder is set to {this.FormFoxSendAddressOnCreateOrder.ToString()}");
                                if (_ca != null)
                                {
                                    string _addy = _ca.Address1;


                                    if (string.IsNullOrEmpty(_ca.Address2) == false)
                                        _addy = _addy + " " + _ca.Address2;
                                    //ffco.CreateOrderTest.PersonalData.DemographicDetail.Company.PostalAddress.DeliveryAddress.AddressLine.Add(_ca.Address2);
                                    ffco.CreateOrderTest.PersonalData.DemographicDetail.Company.PostalAddress.DeliveryAddress.AddressLine.Add(_addy);
                                    ffco.CreateOrderTest.PersonalData.DemographicDetail.Company.PostalAddress.Municipality = _ca.City;
                                    ffco.CreateOrderTest.PersonalData.DemographicDetail.Company.PostalAddress.PostalCode = _ca.ZipCode;
                                    ffco.CreateOrderTest.PersonalData.DemographicDetail.Company.PostalAddress.Region = _ca.State;

                                    // get rid of any empty lines.
                                    ffco.CreateOrderTest.PersonalData.DemographicDetail.Company.PostalAddress.DeliveryAddress.AddressLine =
                                        ffco.CreateOrderTest.PersonalData.DemographicDetail.Company.PostalAddress.DeliveryAddress.AddressLine.Where(al => string.IsNullOrEmpty(al) == false).ToList();

                                    _logger.Debug($"Setting client contact information:");
                                    _logger.Debug("Verifying we have contact info");
                                    senderTracker.AddData($"Contact For Marketplace");
                                    if (string.IsNullOrEmpty(client.ClientContact.ClientContactFirstName) ||
                                        string.IsNullOrEmpty(client.ClientContact.ClientContactLastName) ||
                                        string.IsNullOrEmpty(client.ClientContact.ClientContactPhone)
                                        )
                                    {
                                        _logger.Debug("Client contact information missing, trying to fall back to department");
                                        if (string.IsNullOrEmpty(clientDepartment.ClientContact.ClientContactFirstName) ||
                                           string.IsNullOrEmpty(clientDepartment.ClientContact.ClientContactLastName) ||
                                           string.IsNullOrEmpty(clientDepartment.ClientContact.ClientContactPhone)
                                           )
                                        {
                                            _logger.Error($"NO CLIENT CONTACT INFORMATION FOUND, CANNOT PLACE ORDER");
                                            senderTracker.AddData($"NO CLIENT CONTACT INFORMATION FOUND, CANNOT PLACE ORDER");
                                            FormFoxContactSet = false;

                                        }
                                        else
                                        {
                                            // use dept info
                                            _logger.Debug("Using Dept contact info");
                                            FormFoxContactSet = true;
                                            ffco.CreateOrderTest.PersonalData.DemographicDetail.Company.ContactMethod.ContactName = (clientDepartment.ClientContact.ClientContactFirstName + " " + clientDepartment.ClientContact.ClientContactLastName).Trim();
                                            ffco.CreateOrderTest.PersonalData.DemographicDetail.Company.ContactMethod.Telephone.FormattedNumber = (clientDepartment.ClientContact.ClientContactPhone + "").Trim();
                                            senderTracker.AddData($"{ffco.CreateOrderTest.PersonalData.DemographicDetail.Company.ContactMethod.ContactName}");
                                            senderTracker.AddData($"{ffco.CreateOrderTest.PersonalData.DemographicDetail.Company.ContactMethod.Telephone.FormattedNumber}");

                                        }

                                    }
                                    else
                                    {
                                        // use client info
                                        _logger.Debug("Using client contact info");
                                        FormFoxContactSet = true;
                                        ffco.CreateOrderTest.PersonalData.DemographicDetail.Company.ContactMethod.ContactName = (client.ClientContact.ClientContactFirstName + " " + client.ClientContact.ClientContactLastName).Trim();
                                        ffco.CreateOrderTest.PersonalData.DemographicDetail.Company.ContactMethod.Telephone.FormattedNumber = (client.ClientContact.ClientContactPhone + "").Trim();
                                        senderTracker.AddData($"{ffco.CreateOrderTest.PersonalData.DemographicDetail.Company.ContactMethod.ContactName}");
                                        senderTracker.AddData($"{ffco.CreateOrderTest.PersonalData.DemographicDetail.Company.ContactMethod.Telephone.FormattedNumber}");
                                    }

                                    _logger.Debug("Address set");
                                    ffco.CreateOrderTest.PersonalData.DemographicDetail.Company.PostalAddress.DeliveryAddress.AddressLine.ForEach(al =>

                                    _logger.Debug($"al=> {al.ToString()}")
                                    );

                                    FormFoxContactSet = true;



                                }
                                else
                                {
                                    _logger.Debug("_ca is null [PDFEngine 706]");
                                }
                            }


                            //MRO information
                            //EngineSettings.Settings["MROName"];
                            //ffco.CreateOrderTest

                            ffco.CreateOrderTest.Services = new Services();
                            ffco.CreateOrderTest.Services.DateOrdered = DateTime.Now.ToString("yyyy/MM/dd");
                            ffco.CreateOrderTest.Services.ScheduledDate = DateTime.Now.ToString("yyyy/MM/dd");
                            //DateTime OrderDeadline = DateTime.Now.AddDays(3);
                            OrderDeadline = DateTime.Now;
                            //FormFoxASAPHoursDeadline
                            if (clientNotificationDataSettings.send_asap == true)
                            {
                                if (this.FormFoxASAPHoursDeadline > 0)
                                {
                                    OrderDeadline = OrderDeadline.AddHours(this.FormFoxASAPHoursDeadline);
                                    // set the OrderDeadline to the client's TimeZone
                                    TimeZoneInfo _clientTZ = TimeZoneInfo.FindSystemTimeZoneById(client.client_timezoneinfo);
                                    DateTime _TZDeadline = TimeZoneInfo.ConvertTime(OrderDeadline, TimeZoneInfo.Local, _clientTZ);

                                    DateTime _utcDeadline = _TZDeadline.ToUniversalTime();
                                    ffco.CreateOrderTest.Services.ExpirationDate = _utcDeadline.ToString("yyyy/MM/dd HH:mm:ss");
                                }

                            }
                            else
                            {
                                OrderDeadline = OrderDeadline.AddHours(clientNotificationDataSettings.delay_in_hours);

                                // set the OrderDeadline to the client's TimeZone
                                TimeZoneInfo _clientTZ = TimeZoneInfo.FindSystemTimeZoneById(client.client_timezoneinfo);
                                DateTime _TZDeadline = TimeZoneInfo.ConvertTime(OrderDeadline, TimeZoneInfo.Local, _clientTZ);

                                DateTime _utcDeadline = _TZDeadline.ToUniversalTime();
                                ffco.CreateOrderTest.Services.ExpirationDate = _utcDeadline.ToString("yyyy/MM/dd HH:mm:ss");

                            }

                            // we'll get the sites when we try and create the order
                            // ffco.CreateOrderTest.Services.CollectionSiteID = _site.Code; // Needs to come from FFMPSearch


                            //if (NotificationInfo.lab_code.Equals(this.FormFoxTestLabCode, StringComparison.InvariantCultureIgnoreCase) == true)
                            //{
                            //    ffco.CreateOrderTest.Services.CollectionSiteID = this.FormFoxTestLocation;
                            //}
                            if (FormFoxContactSet == true)
                            {

                                ffco.CreateOrderTest.Services.ReasonForTest.IdValue = "RAN";
                                ffco.CreateOrderTest.Services.ReasonForTest.IdName = "Random";
                                ffco.CreateOrderTest.Services.Service.UnitCodes.IdValue = "W215";
                                ffco.CreateOrderTest.Services.Service = new Service();

                                ffco.CreateOrderTest.Services.Service.Type = "Drug";
                                ffco.CreateOrderTest.Services.Service.AgreeToPay = true.ToString();
                                ffco.CreateOrderTest.Services.Service.DOTTest = "N";

                                ffco.CreateOrderTest.Services.Service.TestingAuthority = "FMCSA";
                                ffco.CreateOrderTest.Services.Service.LaboratoryID = "CRL";
                                ffco.CreateOrderTest.Services.Service.LaboratoryAccount = NotificationInfo.lab_code; // "CRL.FFOX.DOTTEST.WFTEST"; // this is their lab code, translated from ours
                                _logger.Debug($"Lab code (ffco.CreateOrderTest.Services.Service.LaboratoryAccount) {ffco.CreateOrderTest.Services.Service.LaboratoryAccount}");
                                ffco.CreateOrderTest.Services.Service.TestProcedure.IdSampleType = "UR";
                                ffco.CreateOrderTest.Services.Service.TestProcedure.IdTestMethod = "LAB";
                                ffco.CreateOrderTest.Services.Service.UnitCodes.IdValue = NotificationInfo.test_panel_name; // "W215";

                                //ffco.CreateOrderTest.Services.CollectionSiteID = _site.Code;



                                //_logger.Debug("orderXml");
                                //_logger.Debug(orderXml);
                                //// Our test facility ID is FF00095379
                                _logger.Debug($"CreateOrder being called");
                                //senderTracker.AddData("");
                                //senderTracker.AddData("Order XML");
                                //senderTracker.AddData(orderXml);
                                Tuple<string, string, int> _COTump = ffco.CreateOrder(senderTracker);
                                _logger.Debug("Createorder back");
                                string ReferenceTestID = _COTump.Item1;
                                string MarketPlaceOrderId = _COTump.Item2;
                                int CreateOrderErrCode = _COTump.Item3;
                                _FormFoxFailureReason = CreateOrderErrCode;
                                if (ffco.WasAnIssue == true || ffco.fFMPSearch.WasAnIssue == true)
                                {
                                    senderTracker.AddData($"FFCO reports issue. {ffco.WasAnIssue } or {ffco.fFMPSearch.WasAnIssue}");
                                    _logger.Debug($"FFCO reports issue. {ffco.WasAnIssue } or {ffco.fFMPSearch.WasAnIssue}");
                                    WasAnIssue = true; // set this flag to email us issues
                                }
                                else
                                {
                                    senderTracker.AddData("FFCO reports no issues");
                                }
                                // We only sent someone in if we got a ff order id, a marketplace id, and no errors
                                if (!(string.IsNullOrEmpty(ReferenceTestID)) && !(string.IsNullOrEmpty(MarketPlaceOrderId)) && CreateOrderErrCode == 0)
                                {
                                    _logger.Debug($"FormFoxSleepAfterOrderAuthFormSleepInSecs setting {this.FormFoxSleepAfterOrderAuthFormSleepInSecs}");
                                    int _Delay = this.FormFoxSleepAfterOrderAuthFormSleepInSecs * 1000;
                                    _logger.Debug($"_Delay calculated as {_Delay}");
                                    if (_Delay < 1) _Delay = 250;
                                    _logger.Debug($"Sleeping {_Delay} ms before requesting auth form...");
                                    Thread.Sleep(_Delay); // Give Formfox a little time before pulling auth form


                                    _logger.Debug($"PopulateRenderFromNotificationData CreateOrder returned {ReferenceTestID}");

                                    // download PDF
                                    _logger.Debug($"PopulateRenderFromNotificationData Requesting PDF");
                                    FFRequestAuthorization _ra = new FFRequestAuthorization(_logger);
                                    string filename = _ra.GetPDF(ReferenceTestID, senderTracker);
                                    this.PDFBase64 = _ra.PDFBase64;
                                    ParamSetformfoxorders paramSetformfoxorders = new ParamSetformfoxorders();
                                    paramSetformfoxorders.formfoxorders.deadline = OrderDeadline;
                                    paramSetformfoxorders.formfoxorders.ReferenceTestID = ReferenceTestID;
                                    paramSetformfoxorders.formfoxorders.donor_test_info_id = notification.donor_test_info_id;
                                    paramSetformfoxorders.formfoxorders.filename = Path.GetFileName(filename);
                                    var id = backendData.SetFormFoxOrder(paramSetformfoxorders);
                                    var msg = $"Order placed with FormFox. FF ReferenceTestID {ReferenceTestID} Marketplace Order ID {MarketPlaceOrderId}.";
                                    backendData.SetDonorActivity(new ParamSetDonorActivity() { donor_test_info_id = notification.donor_test_info_id, activity_category_id = (int)DonorActivityCategories.Notification, activity_note = msg, activity_user_id = 0 });
                                    senderTracker.AddData(msg);

                                    // Check the PDF to see if it's a Marketplace authform.
                                    _logger.Debug("Checking Auth Form for Marketplace");
                                    //byte[] _AuthFormBytes = System.Convert.FromBase64String(this.PDFBase64);

                                    var bytes = Convert.FromBase64String(this.PDFBase64);
                                    //var contents = new StreamContent(new MemoryStream(bytes));
                                    PdfDocument pdfDocument = PdfReader.Open(new MemoryStream(bytes));
                                    _logger.Debug($"Document Created");
                                    _FormFoxSendInSuccessful = true;

                                    retval = true;
                                    if (!(string.IsNullOrEmpty(this.FormFoxValidAuthFormSearchString)))
                                    {
                                        _logger.Debug($"Searching for text: {this.FormFoxValidAuthFormSearchString}");

                                        bool _MarketPlaceAuthForm = PdfTextExtractor.ContainsText(pdfDocument, this.FormFoxValidAuthFormSearchString, culture, _logger);

                                        if (_MarketPlaceAuthForm == false)
                                        {
                                            _logger.Debug($"INVALID Authform detected!!!");
                                            _FormFoxFailureReason = (int)NotificationClinicExceptions.FormFoxInvalidAuthForm;
                                            ffSiteFound = false;
                                            retval = false;
                                            _FormFoxSendInSuccessful = false;

                                        }
                                        else
                                        {
                                            notification.clinic_exception = 0;
                                            _logger.Debug($"VALID Authform detected");
                                        }
                                    }
                                    else
                                    {
                                        _logger.Debug("No clinic exception - setting clinic_exception to 0");
                                        notification.clinic_exception = 0;
                                    }

                                }
                                else
                                {
                                    _logger.Debug($"Issue! [RMC01] {(NotificationClinicExceptions)CreateOrderErrCode}");
                                    senderTracker.AddData($"Issue! [RMC01] {(NotificationClinicExceptions)CreateOrderErrCode}");
                                    ffSiteFound = false;
                                    retval = false;

                                    _FormFoxFailureReason = CreateOrderErrCode; // (int)NotificationClinicExceptions.FormFoxFailed;
                                }

                            }
                            else
                            {
                                // contact info is jacked up.
                                _FormFoxFailureReason = (int)NotificationClinicExceptions.FormFoxClientContactMissing;
                                _logger.Debug($"Issue! [Contact] {(NotificationClinicExceptions)_FormFoxFailureReason}");
                                senderTracker.AddData($"Issue! [Contact]  {(NotificationClinicExceptions)_FormFoxFailureReason}");
                                ffSiteFound = false;
                                retval = false;
                            }
                        }
                        else
                        {
                            _FormFoxFailureReason = (int)NotificationClinicExceptions.FormFoxClinicNotFound;
                            _logger.Debug($"Issue! [FFC01] {(NotificationClinicExceptions)_FormFoxFailureReason}");
                            senderTracker.AddData($"Issue! [FFC01] {(NotificationClinicExceptions)_FormFoxFailureReason}");
                            ffSiteFound = false;
                            retval = false;

                        }

                        if (ffSiteFound == false && _OverRideForceDB == false)
                        {
                            _logger.Debug($"There was some issue. Current err code: {_FormFoxFailureReason}");
                            notification.clinic_exception_timestamp = DateTime.Now;
                            notification.clinic_exception = _FormFoxFailureReason;
                            notification.notify_next_window = false;
                            notification.notify_now = false;
                            notification.notify_again = false;
                            string msgplus = string.Empty;
                            if (_FormFoxFailureReason == (int)NotificationClinicExceptions.FormFoxOrderGeneralFailure) msgplus = " Possible issue with LabCode in Formfox";

                            // notification.notify_manual = true; // This could potentially try the old way if no FF sites found
                            // notification.notify_now = 0; // clear notify now? this would stop service from trying repeatedly, but it'd require send in again.
                            backendData.SetDonorNotification(new ParamSetDonorNotification() { notification = notification });
                            var msg = $"FormFox Failure for {NotificationInfo.donor_email} - Test Info Id {notification.donor_test_info_id} using range {RenderSettings.Max_Clinic_Distance} CODE: {((NotificationClinicExceptions)_FormFoxFailureReason).ToString()} {_FormFoxFailureReason}";
                            _logger.Debug(msg);

                            backendData.SetDonorActivity(new ParamSetDonorActivity() { donor_test_info_id = notification.donor_test_info_id, activity_category_id = (int)DonorActivityCategories.Notification, activity_note = msg, activity_user_id = 0 });
                            msg = $"Details of failed order: {clientNotificationDataSettings.client_name} dept id: {clientNotificationDataSettings.client_department_id} Code:{tempClientCode} Donor: {donor.DonorFirstName} {donor.DonorLastName}";
                            msg = msg + msgplus;
                            _logger.Debug(msg);

                            senderTracker.AddData(msg);

                            backendData.SetDonorActivity(new ParamSetDonorActivity() { donor_test_info_id = notification.donor_test_info_id, activity_category_id = (int)DonorActivityCategories.Notification, activity_note = msg, activity_user_id = 0 });
                            // Set the FF status to failed for this FF order
                            // this way, there's a record of us trying and it failing
                            // For the status page.
                            ParamSetformfoxorders paramSetformfoxorders = new ParamSetformfoxorders();
                            paramSetformfoxorders.formfoxorders.deadline = OrderDeadline;
                            paramSetformfoxorders.formfoxorders.ReferenceTestID = "na";
                            paramSetformfoxorders.formfoxorders.donor_test_info_id = notification.donor_test_info_id;
                            paramSetformfoxorders.formfoxorders.filename = "na";
                            paramSetformfoxorders.formfoxorders.status = "FF Failed";
                            var id = backendData.SetFormFoxOrder(paramSetformfoxorders);
                            // Do something with the tracker.. 
                            // Email it?
                            ThisSenderTracker = senderTracker;

                            // Per david, never try the old way if FF fails.
                            return false;

                        }

                    }
                }
                catch (Exception ex)
                {

                    _logger.Error("SOMETHING WENT WRONG");

                    _logger.Error(ex.ToString());
                    if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                    if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());

                    _FormFoxSendInSuccessful = false;
                    ThisSenderTracker = senderTracker;
                    retval = false;
                    // We don't throw this - we try and send in the old way

                    // Per david, never try the old way if FF fails.
                    return false;
                }
                _logger.Debug($"Past FF send in");
                if (useFormFoxThisPass == false || (_OverRideForceDB == true && _FormFoxSendInSuccessful == false))
                {
                    _logger.Debug("Trying surpath method");
                    LoadSettings(); // load the settings from the database - to be sure we pick up changes.

                    // now set render settings to the client
                    if (!string.IsNullOrEmpty(clientNotificationDataSettings.pdf_render_settings_filename))
                        LoadConfigFromFile(clientNotificationDataSettings.pdf_render_settings_filename);

                    // if RenderSettingsJson is passed, override. Used for previews
                    if (!string.IsNullOrEmpty(RenderSettingsJson)) SetRenderSettingsFromJson(RenderSettingsJson);

                    // TODO we need to address timezones and DST vs SDT and HOLIDAYS
                    string Expiration = "As Soon As Possible";
                    if (!string.IsNullOrEmpty(RenderSettings.ASAPText)) Expiration = RenderSettings.ASAPText;
                    if (!clientNotificationDataSettings.send_asap)
                    {
                        string ExpirationDateTimeOrNote = " by 4:00 PM";
                        if (!string.IsNullOrEmpty(RenderSettings.ExpirationDateTimeOrNote)) Expiration = RenderSettings.ExpirationDateTimeOrNote;
                        // calculate the expiration date


                        Expiration = DateTime.Now.AddHours(NotificationInfo.delay_in_hours).ToString("M/dd/yyyy") + " by " + DateTime.Now.AddHours(NotificationInfo.delay_in_hours).ToString("h:mm tt");
                        //+ ExpirationDateTimeOrNote;
                    }
                    SetRenderElementText("Expiration", Expiration);

                    //SetRenderElementText("Expiration")
                    SetRenderElementText("lab_code", NotificationInfo.lab_code);
                    SetRenderElementText("test_panel_name", NotificationInfo.test_panel_name);

                    // Services to be performed - use mapping
                    SetRenderElementText("test_category_name", NotificationInfo.test_category_name);
                    // Update if there's a mapping
                    string settingsJson = EngineSettings.Json();

                    JObject jObject = JObject.Parse(settingsJson);

                    // Here we get definable text for test category names from app.config
                    string TestMappingJson = (string)jObject["JSON_PDF_Services_To_Test_Mapping"];
                    JObject jObjectMappings = JObject.Parse(TestMappingJson);
                    //string settingsText = (string)jObjectMappings["UA"];
                    string settingsText = (string)jObjectMappings[string.IsNullOrEmpty(NotificationInfo.test_category_name) ? string.Empty : NotificationInfo.test_category_name];
                    if (!string.IsNullOrEmpty(settingsText))
                    {
                        SetRenderElementText("test_category_name", settingsText);
                    }
                    else
                    {
                        SetRenderElementText("test_category_name", NotificationInfo.test_category_name);
                    }

                    // MROName
                    string _MROName = EngineSettings.Settings["MROName"];
                    _logger.Debug($"MROName from EngineSettings = {_MROName}");

                    if (!string.IsNullOrEmpty(RenderSettings.MROName))
                    {
                        _MROName = RenderSettings.MROName;
                        _logger.Debug($"MROName from RenderSettings = {RenderSettings.MROName}");
                    }
                    _logger.Debug($"MROName now = {_MROName}");
                    if (_MROName == null)
                    {
                        _logger.Debug($"MROName is null!! Setting to empty string.");
                        _MROName = string.Empty;
                    }
                    _logger.Debug($"Setting MROName = {_MROName.ToString()}");
                    SetRenderElementText("MROName", _MROName);

                    // Specimen_Referral
                    if (!string.IsNullOrEmpty(RenderSettings.Specimen_Referral)) SetRenderElementText("Specimen_Referral", RenderSettings.Specimen_Referral);

                    // make sure we can render clinics
                    if (RenderSettings.Elements.Where(x => x.Name.Equals("Clinics", StringComparison.InvariantCultureIgnoreCase)).ToList().Count < 1)
                    {
                        throw new Exception("Invalid PDF config - No Clinics element found");
                    }

                    // Get clinics
                    ParamGetClinicsForZip p = new ParamGetClinicsForZip();
                    p._zip = NotificationInfo.donor_zip;
                    p._dist = RenderSettings.Max_Clinic_Distance;
                    // this expands the search radius if set via exceptions
                    if (notification.clinic_radius > p._dist) p._dist = notification.clinic_radius;

                    List<CollectionFacility> collectionFacilities = backendData.GetClinicsForZip(p);
                    _logger.Debug($"Clinic range completed.. Number of clinics found: {collectionFacilities.Count.ToString()}");
                    if (collectionFacilities.Count < 1)
                    {
                        notification.clinic_exception_timestamp = DateTime.Now;
                        notification.clinic_exception = (int)NotificationClinicExceptions.NotInRange;
                        notification.notify_next_window = false;
                        notification.notify_now = false;
                        notification.notify_again = false;
                        //notification.notify_manual = notification.notify_manual;
                        // notification.notify_now = 0; // clear notify now? this would stop service from trying repeatedly, but it'd require send in again.
                        backendData.SetDonorNotification(new ParamSetDonorNotification() { notification = notification });
                        _logger.Debug($"No clinics in range for {NotificationInfo.donor_email} - Test Info Id {notification.donor_test_info_id} using range {p._dist}");
                        //throw new Exception(@"No clinics found within range: {P._dist}");
                        ThisSenderTracker = senderTracker;

                        return false;
                    }
                    else
                    {
                        // No clinic exceptions any more
                        notification.clinic_exception_timestamp = DateTime.Now;
                        notification.clinic_exception = 0;
                    }

                    // We have at least one clinic

                    foreach (PDFRenderElement pe in RenderSettings.Elements)
                    {
                        _logger.Debug($"Rendering elements - {pe.Name} - {pe.Text.ToString()}");
                        if (pe.Text == null)
                        {
                            _logger.Debug($"{pe.Name} text is null! Null is not allowed, skipping");
                            continue;
                        }
                        RenderElementOntoPDF(pe);
                    }

                    PDFRenderElement clinicsElement = RenderSettings.Elements.Where(x => x.Name.Equals("Clinics", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                    int top = clinicsElement.Top;
                    int left = clinicsElement.Left;
                    int width = clinicsElement.Width;
                    int height = clinicsElement.Height;
                    int maxTextWidth = (int)Math.Floor((double)width / RenderSettings.CollectionSiteNumberOfColumns);
                    int ClinicsElementColumnWidth = (int)Math.Round((double)clinicsElement.Width / RenderSettings.CollectionSiteNumberOfColumns, MidpointRounding.ToEven);
                    string stringAddress = string.Empty;

                    int currentColumn = 0;
                    int FontSize = -1;
                    int ThisRowHeight = -1;
                    int ThisRowTop = 0;
                    int totalHeight = 0;
                    foreach (CollectionFacility c in collectionFacilities)
                    {
                        // create an element for the address
                        PDFRenderElement thisClinicElement = new PDFRenderElement()
                        {
                            FontName = RenderSettings.ClinicDefaultElement.FontName,
                            FontSize = RenderSettings.ClinicDefaultElement.FontSize,
                            BoxAlpha = RenderSettings.ClinicDefaultElement.BoxAlpha,
                            BoxPadding = RenderSettings.ClinicDefaultElement.BoxPadding,
                            TextColorBlue = RenderSettings.ClinicDefaultElement.TextColorBlue,
                            TextColorGreen = RenderSettings.ClinicDefaultElement.TextColorGreen,
                            TextColorRed = RenderSettings.ClinicDefaultElement.TextColorRed,
                            Bold = RenderSettings.ClinicDefaultElement.Bold,
                            DrawBox = RenderSettings.ClinicDefaultElement.DrawBox,
                            DrawBoxSolid = RenderSettings.ClinicDefaultElement.DrawBoxSolid,
                            BoxColorRed = RenderSettings.ClinicDefaultElement.BoxColorRed,
                            BoxColorBlue = RenderSettings.ClinicDefaultElement.BoxColorBlue,
                            BoxColorGreen = RenderSettings.ClinicDefaultElement.BoxColorGreen
                        };

                        // We've calculated a font size, so lete's use that.
                        if (FontSize > 0) thisClinicElement.FontSize = FontSize;
                        // create the text
                        stringAddress = string.Empty;
                        stringAddress = c.vendor_name + Environment.NewLine;
                        stringAddress += c.vendor_address_1 + " " + c.vendor_address_2 + Environment.NewLine;
                        stringAddress += c.vendor_city + " " + c.vendor_state + ", " + c.vendor_zip + Environment.NewLine;
                        stringAddress += c.vendor_phone + Environment.NewLine;
                        thisClinicElement.Text = stringAddress;
                        // get it's width and height

                        int[] aSize = GetElementContentSize(thisClinicElement);
                        // drop the font size till it's less than half the column size

                        while (aSize[0] > maxTextWidth && FontSize == -1)
                        {
                            thisClinicElement.FontSize = thisClinicElement.FontSize - 1;
                            aSize = GetElementContentSize(thisClinicElement);
                        }
                        // once we have a working font size, we'll use it for everyone
                        // TODO - go through all results to find the smallest font size for the widest address?
                        FontSize = thisClinicElement.FontSize;
                        // if the total height exceeds the clinics box - stop.
                        if (totalHeight + aSize[1] > clinicsElement.Height) break;

                        if (aSize[1] > ThisRowHeight) ThisRowHeight = aSize[1];
                        // render it at the left of the box and the top of the row
                        thisClinicElement.Top = top + ThisRowTop;
                        thisClinicElement.Left = left + (ClinicsElementColumnWidth * currentColumn);
                        thisClinicElement.Width = ClinicsElementColumnWidth;
                        thisClinicElement.Height = aSize[1];
                        RenderElementOntoPDF(thisClinicElement);
                        // increase column count
                        currentColumn++;
                        // if it's > columns, go back to 0
                        if (currentColumn == RenderSettings.CollectionSiteNumberOfColumns)
                        {
                            ThisRowTop += ThisRowHeight;
                            totalHeight += ThisRowHeight;
                            ThisRowHeight = -1;
                            currentColumn = 0;
                        }
                    }
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        CurrentDocument.Save(memoryStream, true);
                        this.PDFBase64 = Convert.ToBase64String(memoryStream.ToArray());
                    }
                    retval = true;

                }
                else
                {
                    _logger.Debug($"Did not try old method.. returning {retval}");
                }
                //int elementHeight = GetTextHeight("X", clinicsElement);
                // The PDF document has been rendered,
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                throw;
            }
            ThisSenderTracker = senderTracker;

            return retval;
        }

        private void SetRenderElementText(string name, string text)
        {
            {
                text = string.IsNullOrEmpty(text) ? string.Empty : text;
                var p = this.RenderSettings;
                if (p.Elements.Exists(e => e.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)))
                {
                    p.Elements.Where(e => e.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)).First().Text = text;
                }
            }
        }

        public bool LoadSettings()
        {
            string EngineSettingsJSON = string.Empty;

            string path = AppDomain.CurrentDomain.BaseDirectory;

            NameValueCollection _settings;

            _settings = ConfigurationManager.GetSection(appSectionGroup) as NameValueCollection;
            // copy to a list so it's not read only
            _logger.Debug("Loading settings from config file... [LoadSettings()]");

            foreach (string key in _settings)
            {
                _logger.Debug($"Config File Setting {key} found with value {_settings[key]}");
                if (EngineSettings.Settings[key] == null)
                {
                    EngineSettings.Settings.Add(key, _settings[key]);
                }
                else
                {
                    EngineSettings.Settings[key] = _settings[key];
                }
            }

            if (backendFiles.databaseFileExists(EngineSettingsFileName) < 1)
            {
                _logger.Information("Saving config file settings to database because no global file exists.");
                string _settingsJSON = EngineSettings.Json();
                backendFiles.SaveTextFile(EngineSettingsFileName, _settingsJSON);
            }
            else
            {
                _logger.Information("Global config found in database, loading. To overwrite with config file settings, launch the service with -r.");
            }
            //string ConfigFileEngineSettingsJSON = EngineSettings.Json();
            _logger.Debug($"Loading PDFEngineSettings from {EngineSettingsFileName} database version");
            EngineSettingsJSON = backendFiles.ReadTextFile(EngineSettingsFileName);
            EngineSettings.FromJson(EngineSettingsJSON);

            if (EngineSettings.Settings.Count == 0)
            {
                _logger.Error("NO SETTINGS FOR PDFENGINE, BAILING....");
                return false;
            }
            else
            {
                //foreach (var key in EngineSettings.Values.AllKeys)

                //    Console.WriteLine(key + " = " + EngineSettings.Values[key]);
                //}

                PDFTemplateFolder = EngineSettings.Settings["PDFTemplateFolder"];
                DefaultTemplate = EngineSettings.Settings["DefaultTemplate"];
                PDFConfigFolder = EngineSettings.Settings["PDFConfigFolder"];
                DefaultRenderSettingsFile = EngineSettings.Settings["DefaultRenderSettingsFile"];
                MROName = EngineSettings.Settings["MROName"];
                CurrentTemplate = DefaultTemplate;

                _logger.Debug($"PDFTemplateFolder {PDFTemplateFolder}");
                _logger.Debug($"DefaultTemplate {DefaultTemplate}");
                _logger.Debug($"PDFConfigFolder {PDFConfigFolder}");
                _logger.Debug($"DefaultRenderSettingsFile {DefaultRenderSettingsFile}");
                _logger.Debug($"CurrentTemplate {CurrentTemplate}");
                _logger.Debug($"MROName {MROName}");

                //JSON_PDF_Services_To_Test_Mapping = EngineSettings.Values["JSON_PDF_Services_To_Test_Mapping"];

                // Load the default render config
                LoadConfigFromFile(DefaultRenderSettingsFile);
                LoadConfigFromFile(DefaultRenderSettingsFile, true);
                backendData = new BackendData(null, null, _logger);

                // load the user's config
                _logger.Debug("Load Settings Complete");
                return true;
            }
        }

        public bool SaveEngineSettings()
        {
            try
            {
                _logger.Debug("Saving config file settings to database");
                string _settingsJSON = EngineSettings.Json();
                backendFiles.SaveTextFile(EngineSettingsFileName, _settingsJSON);

                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        //public bool SavePDFGlobalsToConfig()
        //{
        //    try
        //    {
        //        Configuration configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        //        var section = (configFile.GetSection(appSectionGroup));
        //        AppSettingsSection section2 = (AppSettingsSection)configFile.GetSection(appSectionGroup);
        //        var settings = ((AppSettingsSection)configFile.GetSection(appSectionGroup)).Settings;

        //        foreach (string key in EngineSettings.Settings)
        //        {
        //            if (settings[key] == null)
        //            {
        //                settings.Add(key, EngineSettings.Settings[key]);
        //            }
        //            else
        //            {
        //                settings[key].Value = EngineSettings.Settings[key];
        //            }
        //        }
        //        configFile.AppSettings.SectionInformation.ForceSave = true;
        //        configFile.Save(ConfigurationSaveMode.Modified);
        //        ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);

        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //        throw;
        //    }
        //}

        public void SaveCurrentDocument(string _filename)
        {
            PDFTemplateFolder = PDFTemplateFolder.TrimEnd('\\') + '\\';
            string filename = PDFTemplateFolder + _filename;
            if (File.Exists(filename)) File.Delete(filename);
            CurrentDocument.Save(filename);
        }

        public void GetCurrentDocument()
        {
        }

        /// <summary>
        /// Loads PDF template from folder in PDFTemplateFolder
        /// </summary>
        private void LoadTemplateFromFile(string TemplateName)
        {
            //PDFTemplateFolder = PDFTemplateFolder.TrimEnd('\\') + '\\';

            string filename = TemplateName;
            MemoryStream contents = backendFiles.databaseFileRead(TemplateName);

            if (contents.Length < 1)
            {
                _logger.Error($"LoadTemplateFromFile: { filename} is missing!!");

                throw new Exception($"Template {TemplateName} Not Found.");
            }
            else
            {
                _logger.Debug($"LoadTemplateFromFile: { filename} found, setting into engine.");

                PdfDocument TemplateDocument = PdfReader.Open(contents, PdfDocumentOpenMode.Import);
                CurrentDocument = new PdfDocument();
                for (int Pg = 0; Pg < TemplateDocument.Pages.Count; Pg++)
                {
                    CurrentDocument.AddPage(TemplateDocument.Pages[Pg]);
                }
            }
        }

        public PdfRenderSettings ReadPdfRenderSettings(string ConfigName)
        {
            PdfRenderSettings pdfRenderSettings_temp = new PdfRenderSettings();

            string ConfigJson = backendFiles.ReadTextFile(ConfigName);
            _logger.Debug($"Reading {ConfigName}");
            if (string.IsNullOrEmpty(ConfigJson))
            {
                _logger.Error($"{ConfigName} doesn't exist!!! Loading Defaults");

                ConfigJson = backendFiles.ReadTextFile(DefaultRenderSettingsFile);
            }
            else
            {
                _logger.Debug($"LoadConfigFromFile: { ConfigName} found, setting into engine.");
            }

            if (!string.IsNullOrEmpty(ConfigJson))
            {
                _logger.Debug("ConfigJson text read - Applying...");
                pdfRenderSettings_temp = JsonConvert.DeserializeObject<PdfRenderSettings>(ConfigJson);

            }
            else
            {
                // it's empty
                _logger.Error($"ConfigJson is empty!!");
            }
            return pdfRenderSettings_temp;
        }

        private void LoadConfigFromFile(string ConfigName, bool LoadDefault = false)
        {
            string ConfigJson = backendFiles.ReadTextFile(ConfigName);
            _logger.Debug($"Reading {ConfigName}");
            if (string.IsNullOrEmpty(ConfigJson))
            {
                _logger.Error($"{ConfigName} doesn't exist!!! Throwing");

                throw new Exception($"Config {ConfigName} Not Found in database");
            }
            else
            {
                _logger.Debug($"LoadConfigFromFile: { ConfigName} found, setting into engine.");
            }

            if (!string.IsNullOrEmpty(ConfigJson))
            {
                _logger.Debug("ConfigJson text read - Applying...");
                if (!LoadDefault)
                {
                    PdfRenderSettings pdfRenderSettings_temp = new PdfRenderSettings();
                    pdfRenderSettings_temp = JsonConvert.DeserializeObject<PdfRenderSettings>(ConfigJson);
                    RenderSettings = pdfRenderSettings_temp;
                    _loadedSettings = this.GetRenderSettingsAsJson();
                }
                else
                {
                    _logger.Debug($"Loading default...");

                    PdfRenderSettings pdfRenderSettings_temp = new PdfRenderSettings();
                    pdfRenderSettings_temp = JsonConvert.DeserializeObject<PdfRenderSettings>(ConfigJson);
                    DefaultRenderSettingsObject = pdfRenderSettings_temp;
                }
            }
            else
            {
                // it's empty
                _logger.Error($"ConfigJson is empty!!");
            }
        }

        public bool SetRenderSettingsFromJson(string _json)
        {
            bool retval = false;
            try
            {
                if (!string.IsNullOrEmpty(_json))
                {
                    PdfRenderSettings pdfRenderSettings_temp = new PdfRenderSettings();
                    pdfRenderSettings_temp = JsonConvert.DeserializeObject<PdfRenderSettings>(_json);
                    RenderSettings = pdfRenderSettings_temp;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return retval;
        }

        public bool IsDirty()
        {
            try
            {
                return _loadedSettings.Equals(GetRenderSettingsAsJson(), StringComparison.InvariantCultureIgnoreCase);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ValidateJson(string _jsontotest)
        {
            try
            {
                PdfRenderSettings pdfRenderSettings_temp = JsonConvert.DeserializeObject<PdfRenderSettings>(_jsontotest);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string GetRenderSettingsAsJson(bool getDefault = false)
        {
            string retval = string.Empty;

            try
            {
                if (getDefault)
                {
                    retval = JsonConvert.SerializeObject(DefaultRenderSettingsObject, Formatting.Indented); //Serialize<PdfRenderSettings>(pdfRenderSettings);
                }
                else
                {
                    retval = JsonConvert.SerializeObject(RenderSettings, Formatting.Indented); //Serialize<PdfRenderSettings>(pdfRenderSettings);
                }
            }
            catch (Exception Ex)
            {
                throw;
            }

            return retval;
        }

        public bool SaveRenderSettingsToFile(string filename)
        {
            bool success = false;
            try
            {
                string _json = JsonConvert.SerializeObject(RenderSettings, Formatting.Indented); //Serialize<PdfRenderSettings>(pdfRenderSettings);

                success = backendFiles.SaveTextFile(filename, _json) > 0;
            }
            catch (Exception ex)
            {
                throw;
            }

            return success;
        }
    }

    public class LayoutHelper
    {
        private readonly PdfDocument _document;
        private readonly XUnit _topPosition;
        private readonly XUnit _bottomMargin;
        private XUnit _currentPosition;

        public LayoutHelper(PdfDocument document, XUnit topPosition, XUnit bottomMargin)
        {
            _document = document;
            _topPosition = topPosition;
            _bottomMargin = bottomMargin;
            // Set a value outside the page - a new page will be created on the first request.
            _currentPosition = _topPosition; // bottomMargin + 10000;
            if (document.PageCount < 1) CreatePage();
        }

        public XUnit GetLinePosition(XUnit requestedHeight)
        {
            return GetLinePosition(requestedHeight, -1f);
        }

        public XUnit GetLinePosition(XUnit requestedHeight, XUnit requiredHeight)
        {
            XUnit required = requiredHeight == -1f ? requestedHeight : requiredHeight;
            if (_currentPosition + required > _bottomMargin)
                CreatePage();
            XUnit result = _currentPosition;
            _currentPosition += requestedHeight;
            return result;
        }

        public XGraphics Gfx { get; private set; }
        public PdfPage Page { get; private set; }

        void CreatePage()
        {
            Page = _document.AddPage();
            Page.Size = PageSize.A4;
            Page.TrimMargins.All = 1;
            Gfx = XGraphics.FromPdfPage(Page);
            _currentPosition = _topPosition;
        }
    }
}