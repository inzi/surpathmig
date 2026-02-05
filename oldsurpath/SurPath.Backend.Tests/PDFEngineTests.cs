using Microsoft.VisualStudio.TestTools.UnitTesting;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using SurPath.Business.Backend.PdfTextract;
using SurPath.Enum;
using SurpathBackend.Classes;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace SurPath.Backend.Tests
{
    [TestClass]
    public class PDFEngineTests
    {
        //BackendData d;
        //string ConnString = "server = 127.0.0.1; port = 3306; Username = surpath; Password = z24XrByS1; database = surpathlive; convert zero datetime=True";

        //[TestInitialize]
        //public void TestInit()
        //{
        //    d = new BackendData(ConnString);
        //}

        [TestMethod]
        public void enumTeste1()
        {
            var _FormFoxFailureReason = 52;
            var s = ((NotificationClinicExceptions)_FormFoxFailureReason).ToString();
                var x = 1;
        }

        [TestMethod]
        public void TimeZoneDump()
        {
            foreach (TimeZoneInfo zone in TimeZoneInfo.GetSystemTimeZones())
            {
                int zoneLen = zone.DisplayName.IndexOf(")");
                string timeZone = zone.DisplayName.Substring(0, zoneLen + 1);
                Debug.WriteLine(timeZone + " " + zone.Id);
            }

            foreach (TimeZoneInfo zone in USATimeZoneHelper.USATZList())
            {
                int zoneLen = zone.DisplayName.IndexOf(")");
                string timeZone = zone.DisplayName.Substring(0, zoneLen + 1);
                Debug.WriteLine(timeZone + " " + zone.Id);
            }
            // Alaskan Standard Time
            //Hawaiian Standard Time
            // Aleutian Standard Time
            var x = 1;
        }

        [TestMethod]
        public void ConvertDeadlineTZ()
        {
            DateTime OrderDeadline = DateTime.Now;
            //FormFoxASAPHoursDeadline
            OrderDeadline = OrderDeadline.AddHours(48);

            // set the OrderDeadline to the client's TimeZone

            TimeZoneInfo _clientTZ = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime _TZDeadline = TimeZoneInfo.ConvertTime(OrderDeadline, TimeZoneInfo.Local, _clientTZ);

            DateTime _utcDeadline = _TZDeadline.ToUniversalTime();
            var _utcD = OrderDeadline.ToString("yyyy/MM/dd HH:mm:ss");

            var x = 1;
        }

        [TestMethod]
        public void MarketPlacePDF()
        {
            string _testString = "Plan your visit";

            string fileName = "c:\\dev\\temp\\51385724.pdf"; //false
            CultureInfo culture = new CultureInfo("en-US", false);
            byte[] bytes = File.ReadAllBytes(fileName);
            PdfDocument pdfDocument = PdfReader.Open(new MemoryStream(bytes));

            //  pdfDocument.Save("c:\\dev\\temp\\working.pdf");

            string test = PdfTextExtractor.GetText(fileName);

            bool _MarketPlaceAuthForm = PdfTextExtractor.ContainsText(pdfDocument, _testString, culture);

            Assert.IsFalse(_MarketPlaceAuthForm);

            fileName = "c:\\dev\\temp\\51385730.pdf"; //true
            bytes = File.ReadAllBytes(fileName);
            pdfDocument = PdfReader.Open(new MemoryStream(bytes));

            // pdfDocument.Save("c:\\dev\\temp\\working2.pdf");

            _MarketPlaceAuthForm = PdfTextExtractor.ContainsText(pdfDocument, _testString, culture);

            Assert.IsTrue(_MarketPlaceAuthForm);
            var x = 1;
        }

        //[TestMethod]
        //public void Integrity()
        //{
        //    PDFengine pDFengine = new PDFengine();

        //    Assert.IsTrue(!String.IsNullOrEmpty(pDFengine.PDFTemplateFolder));

        //    string settingsJson = pDFengine.EngineSettings.Json();

        //    JObject jObject = JObject.Parse(settingsJson);

        //    Assert.IsTrue(!String.IsNullOrEmpty(settingsJson));
        //    string TestMappingJson = (string)jObject["JSON_PDF_Services_To_Test_Mapping"];
        //    JObject jObjectMappings = JObject.Parse(TestMappingJson);

        //    string uaText = (string)jObjectMappings["UA"];

        //    Assert.IsTrue(!String.IsNullOrEmpty(uaText));

        //    pDFengine.SaveSettings();

        //}

        //[TestMethod]
        //public void UpdatePDFSettings()
        //{
        //    PDFengine pDFengine = new PDFengine();

        //    string settingsJson = pDFengine.EngineSettings.Json();

        //    JObject jObject = JObject.Parse(settingsJson);

        //    string TestMappingJson = (string)jObject["JSON_PDF_Services_To_Test_Mapping"];
        //    string TestMappingJson_orig = TestMappingJson;

        //    JObject jObjectMappings = JObject.Parse(TestMappingJson);

        //    jObjectMappings["UnitTest"] = "Unit Test";

        //    jObject["JSON_PDF_Services_To_Test_Mapping"] = jObjectMappings.ToString();
        //    jObject["UnitTestSetting"] = "true";
        //    //.Values.Add(new KeyValuePair<string, string>(){ "UnitTest", "Unit Test" });

        //        //

        //    pDFengine.EngineSettings.FromJson(jObject.ToString());

        //    pDFengine.SaveSettings();

        //    // Reload the settings.
        //    pDFengine.LoadSettings();
        //    settingsJson = pDFengine.EngineSettings.Json();
        //    jObject = JObject.Parse(settingsJson);
        //    TestMappingJson = (string)jObject["JSON_PDF_Services_To_Test_Mapping"];
        //    jObjectMappings = JObject.Parse(TestMappingJson);
        //    string SettingText = (string)jObjectMappings["UnitTest"];
        //    Assert.IsTrue(!string.IsNullOrEmpty(SettingText));
        //    Assert.IsTrue(!string.IsNullOrEmpty(pDFengine.EngineSettings.Settings["UnitTest"]));
        //    // Remove Setting
        //    pDFengine.EngineSettings.FromJson(TestMappingJson_orig);
        //    pDFengine.SaveSettings();

        //}

        //[TestMethod]
        //public void Serialization()
        //{
        //    string TestValue = String.Empty;
        //    string TestValueName = "PDFTemplateFolder";

        //    PDFengine pDFengine = new PDFengine();
        //    JavaScriptSerializer json_serializer = new JavaScriptSerializer();
        //    PDFEngineSettings pDFEngineSettings = new PDFEngineSettings();

        //    TestValue = pDFengine.EngineSettings.Settings[TestValueName];

        //    string settingsJson = pDFengine.EngineSettings.Json();

        //    Assert.IsTrue(pDFEngineSettings.Settings.Count == 0);

        //    //Dictionary<string, object> _settings = (Dictionary<string, object>)json_serializer.DeserializeObject(settingsJson);

        //    pDFEngineSettings.FromJson(settingsJson);

        //    Assert.IsTrue(pDFEngineSettings.Settings[TestValueName] == TestValue);

        //    Assert.IsFalse(pDFEngineSettings.Settings[TestValueName] == TestValue + "Not this");
        //}

        //[TestMethod]
        //public void LoadTemplateFromFile()
        //{
        //    string TemplateName = "PDFTemplateTest1.pdf";

        //    PDFengine pDFengine = new PDFengine();
        //    pDFengine.LoadTemplate(TemplateName);

        //    Assert.IsTrue(pDFengine.CurrentDocument.PageCount > 0);

        //}

        //[TestMethod]

        //public void WriteOnTemplateAndSave()
        //{
        //    const string LatinText =
        //    "Facin exeraessisit la consenim iureet dignibh eu facilluptat vercil dunt autpat. " +
        //    "Ecte magna faccum dolor sequisc iliquat, quat, quipiss equipit accummy niate magna " +
        //    "facil iure eraesequis am velit, quat atis dolore dolent luptat nulla adio odipissectet " +
        //    "lan venis do essequatio conulla facillandrem zzriusci bla ad minim inis nim velit eugait " +
        //    "aut aut lor at ilit ut nulla ate te eugait alit augiamet ad magnim iurem il eu feuissi.\n" +
        //    "Guer sequis duis eu feugait luptat lum adiamet, si tate dolore mod eu facidunt adignisl in " +
        //    "henim dolorem nulla faccum vel inis dolutpatum iusto od min ex euis adio exer sed del " +
        //    "dolor ing enit veniamcon vullutat praestrud molenis ciduisim doloborem ipit nulla consequisi.\n" +
        //    "Nos adit pratetu eriurem delestie del ut lumsandreet nis exerilisit wis nos alit venit praestrud " +
        //    "dolor sum volore facidui blaor erillaortis ad ea augue corem dunt nis  iustinciduis euisi.\n" +
        //    "Ut ulputate volore min ut nulpute dolobor sequism olorperilit autatie modit wisl illuptat dolore " +
        //    "min ut in ute doloboreet ip ex et am dunt at.";

        //    string StringToWrite = "this is a test\n" + DateTime.Now.ToString();

        //    StringToWrite += LatinText;

        //    string TemplateName = "PDFTemplateTest1.pdf";

        //    string _filename = "Test.PDF";

        //    PDFengine pDFengine = new PDFengine();
        //    pDFengine.LoadTemplate(TemplateName);
        //    pDFengine.WriteOntoPDF(StringToWrite);
        //    pDFengine.SaveCurrentDocument(_filename);

        //}

        //[TestMethod]
        //public void SanitizeLetter2()
        //{
        //    string TemplateName = "letter2.pdf";

        //    string _filename = "letter2san.PDF";

        //    PDFengine pDFengine = new PDFengine();
        //    pDFengine.LoadTemplate(TemplateName);

        //    // PDFSHarp is always 1/72 dpi - so for 8.5 inches, to get x it's inches * 72
        //    // 612 x 792
        //    int gridstep = 100;

        //    for (int x=0; x<=600; x+= gridstep)
        //    {
        //        pDFengine.DrawLine(x, 0, x,792);
        //    }
        //    for (int y = 0; y <= 790; y += gridstep)
        //    {
        //        pDFengine.DrawLine(0, y, 612, y);
        //    }
        //    gridstep = 50;
        //    int alpha = 100;
        //    for (int x = 0; x <= 600; x += gridstep)
        //    {
        //        pDFengine.DrawLine(x, 0, x, 792, alpha);
        //    }
        //    for (int y = 0; y <= 790; y += gridstep)
        //    {
        //        pDFengine.DrawLine(0, y, 612, y, alpha);
        //    }

        //    gridstep = 10;
        //    alpha = 20;
        //    for (int x = 0; x <= 600; x += gridstep)
        //    {
        //        pDFengine.DrawLine(x, 0, x, 792, alpha);
        //    }
        //    for (int y = 0; y <= 790; y += gridstep)
        //    {
        //        pDFengine.DrawLine(0, y, 612, y, alpha);
        //    }

        //    //for (int x=0; x< 500;  x+=10)
        //    //{
        //    //    pDFengine.DrawRectable(x, x, 5, 5);

        //    //}
        //    //pDFengine.DrawRectable(10, 10, 10, 10);
        //    //pDFengine.DrawRectable(20, 20, 10, 10);
        //    //pDFengine.DrawRectable(30, 30, 10, 10);

        //    pDFengine.DrawRectable(83,190,230,17, 200);
        //    pDFengine.SaveCurrentDocument(_filename);
        //}

        //[TestMethod]
        //public void PopulatePDFFromDataWithConfigFile()
        //{
        //    PDFengine pDFengine = new PDFengine();

        //    string _output_filename = "Test_From_RenderSettings_Config.PDF";

        //    string clientCode = "TestSurScan";
        //    ParamGetNotificationInfoForDonorInfoId p = new ParamGetNotificationInfoForDonorInfoId
        //    {
        //        donor_test_info_id = 93991
        //    };

        //    NotificationInformation notificationData = d.GetNotificationInfoForDonorInfoId(p);

        //    //int rows = (int)Math.Round((double)(collectionFacilities.Count / pdfRenderSettings.CollectionSiteNumberOfColumns), MidpointRounding.AwayFromZero);
        //    PDFRenderElement clinicsElement = pDFengine.RenderSettings.Elements.Where(x => x.Name.Equals("Clinics", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
        //    Assert.IsNotNull(clinicsElement);
        //    string RenderSettingsString = JsonConvert.SerializeObject(pDFengine.RenderSettings); //Serialize<PdfRenderSettings>(pdfRenderSettings);
        //    pDFengine.LoadTemplate(pDFengine.RenderSettings.TemplateFileName);
        //    foreach (PDFRenderElement pe in pDFengine.RenderSettings.Elements)
        //    {
        //        pDFengine.RenderElementOntoPDF(pe);

        //    }

        //    ParamGetClinicsForDonor paramClinics = new ParamGetClinicsForDonor();

        //    paramClinics.donor_id = 6;
        //    paramClinics._dist = 30;
        //    List<CollectionFacility> collectionFacilities = d.GetClinicsForDonor(paramClinics);
        //    Assert.IsTrue(collectionFacilities.Count > 0);

        //    int top = clinicsElement.Top;
        //    int left = clinicsElement.Left;
        //    int width = clinicsElement.Width;
        //    int height = clinicsElement.Height;
        //    int maxTextWidth = (int)Math.Floor((double)width / pDFengine.RenderSettings.CollectionSiteNumberOfColumns);
        //    int ClinicsElementColumnWidth = (int)Math.Round((double)clinicsElement.Width / pDFengine.RenderSettings.CollectionSiteNumberOfColumns, MidpointRounding.ToEven);
        //    string stringAddress = string.Empty;

        //    int currentColumn = 0;
        //    int FontSize = -1;
        //    int ThisRowHeight = -1;
        //    int ThisRowTop = 0;
        //    int totalHeight = 0;
        //    foreach (CollectionFacility c in collectionFacilities)
        //    {
        //        // create an element for the address
        //        PDFRenderElement thisClinicElement = new PDFRenderElement()
        //        {
        //            FontName = pDFengine.RenderSettings.ClinicDefaultElement.FontName,
        //            FontSize = pDFengine.RenderSettings.ClinicDefaultElement.FontSize,
        //            BoxAlpha = pDFengine.RenderSettings.ClinicDefaultElement.BoxAlpha,
        //            BoxPadding = pDFengine.RenderSettings.ClinicDefaultElement.BoxPadding,
        //            TextColorBlue = pDFengine.RenderSettings.ClinicDefaultElement.TextColorBlue,
        //            TextColorGreen = pDFengine.RenderSettings.ClinicDefaultElement.TextColorGreen,
        //            TextColorRed = pDFengine.RenderSettings.ClinicDefaultElement.TextColorRed,
        //            Bold = pDFengine.RenderSettings.ClinicDefaultElement.Bold,
        //            DrawBox = pDFengine.RenderSettings.ClinicDefaultElement.DrawBox,
        //            DrawBoxSolid = pDFengine.RenderSettings.ClinicDefaultElement.DrawBoxSolid,
        //            BoxColorRed = pDFengine.RenderSettings.ClinicDefaultElement.BoxColorRed,
        //            BoxColorBlue = pDFengine.RenderSettings.ClinicDefaultElement.BoxColorBlue,
        //            BoxColorGreen = pDFengine.RenderSettings.ClinicDefaultElement.BoxColorGreen
        //        };

        //        // We've calculated a font size, so lete's use that.
        //        if (FontSize > 0) thisClinicElement.FontSize = FontSize;
        //        // create the text
        //        stringAddress = string.Empty;
        //        stringAddress = c.vendor_name + Environment.NewLine;
        //        stringAddress += c.vendor_address_1 + " " + c.vendor_address_2 + Environment.NewLine;
        //        stringAddress += c.vendor_city + " " + c.vendor_state + ", " + c.vendor_zip + Environment.NewLine;
        //        stringAddress += c.vendor_phone + Environment.NewLine;
        //        thisClinicElement.Text = stringAddress;
        //        // get it's width and height

        //        int[] aSize = pDFengine.GetElementContentSize(thisClinicElement);
        //        // drop the font size till it's less than half the column size

        //        while (aSize[0] > maxTextWidth && FontSize == -1)
        //        {
        //            thisClinicElement.FontSize = thisClinicElement.FontSize - 1;
        //            aSize = pDFengine.GetElementContentSize(thisClinicElement);
        //        }
        //        // once we have a working font size, we'll use it for everyone
        //        // TODO - go through all results to find the smallest font size for the widest address?
        //        FontSize = thisClinicElement.FontSize;
        //        // if the total height exceeds the clinics box - stop.
        //        if (totalHeight + aSize[1] > clinicsElement.Height) break;

        //        if (aSize[1] > ThisRowHeight) ThisRowHeight = aSize[1];
        //        // render it at the left of the box and the top of the row
        //        thisClinicElement.Top = top + ThisRowTop;
        //        thisClinicElement.Left = left + (ClinicsElementColumnWidth * currentColumn);
        //        thisClinicElement.Width = ClinicsElementColumnWidth;
        //        thisClinicElement.Height = aSize[1];
        //        pDFengine.RenderElementOntoPDF(thisClinicElement);
        //        // increase column count
        //        currentColumn++;
        //        // if it's > columns, go back to 0
        //        if (currentColumn == pDFengine.RenderSettings.CollectionSiteNumberOfColumns)
        //        {
        //            ThisRowTop += ThisRowHeight;
        //            totalHeight += ThisRowHeight;
        //            ThisRowHeight = -1;
        //            currentColumn = 0;
        //        }

        //    }
        //    int elementHeight = pDFengine.GetTextHeight("X", clinicsElement);

        //    pDFengine.SaveCurrentDocument(_output_filename);

        //}

        //[TestMethod]
        //public void PopuldatePDFfromDonorTestInfoID()
        //{
        //    PDFengine pDFengine = new PDFengine();
        //    pDFengine.LoadTemplate();
        //    List<NotificationInformation> l = d.GetNotificationInformationList();
        //    foreach(NotificationInformation n in l)
        //    {
        //        pDFengine.PopulateRenderFromNotificationData(n);
        //        pDFengine.SaveCurrentDocument(n.donor_email + ".pdf");
        //    }

        //}

        //[TestMethod]
        //public void PopulatePDFFromData()
        //{
        //    PDFengine pDFengine = new PDFengine();

        //    PdfRenderSettings pdfRenderSettings = new PdfRenderSettings();

        //    int r = 200, g = 200, b = 100;

        //    pdfRenderSettings.Elements.Add(new PDFRenderElement()
        //    {
        //        Name = "Expiration",
        //        Top = 85,
        //        Left = 195,
        //        Width = 250,
        //        Height = 15,
        //        Text = "Expiration",
        //        DrawBox = true,
        //        DrawBoxSolid = true,
        //        BoxColorRed = r,
        //        BoxColorBlue = b,
        //        BoxColorGreen = g
        //    });

        //    pdfRenderSettings.Elements.Add(new PDFRenderElement()
        //    {
        //        Name = "MROName",
        //        Top = 140,
        //        Left = 398,
        //        Width = 100,
        //        Height = 10,
        //        Text = "MRO Dr Name",
        //        DrawBox = true,
        //        DrawBoxSolid = true,
        //        BoxColorRed = r,
        //        BoxColorBlue = b,
        //        BoxColorGreen = g
        //    });

        //    pdfRenderSettings.Elements.Add(new PDFRenderElement()
        //    {
        //        Name = "AccountInfo",
        //        Top = 275,
        //        Left = 48,
        //        Width = 200,
        //        Height = 25,
        //        Text = "Account Info",
        //        Bold = true,
        //        DrawBox = true,
        //        DrawBoxSolid = true,
        //        BoxColorRed = r,
        //        BoxColorBlue = b,
        //        BoxColorGreen = g
        //    });

        //    pdfRenderSettings.Elements.Add(new PDFRenderElement()
        //    {
        //        Name = "Panel",
        //        Top = 316,
        //        Left = 280,
        //        Width = 60,
        //        Height = 10,
        //        Text = "Panel",
        //        Bold = false,
        //        DrawBox = true,
        //        DrawBoxSolid = true,
        //        BoxColorRed = r,
        //        BoxColorBlue = b,
        //        BoxColorGreen = g
        //    });

        //    pdfRenderSettings.Elements.Add(new PDFRenderElement()
        //    {
        //        Name = "Clinics",
        //        Top = 370,
        //        Left = 50,
        //        Width = 515,
        //        Height = 275,
        //        Text = "",
        //        FontSize = 8,
        //        Bold = false,
        //        DrawBox = true,
        //        DrawBoxSolid = true,
        //        BoxColorRed = r,
        //        BoxColorBlue = b,
        //        BoxColorGreen = g
        //    });

        //    pdfRenderSettings.Elements.Add(new PDFRenderElement()
        //    {
        //        Name = "Details",
        //        Top = 280,
        //        Left = 395,
        //        Width = 150,
        //        Height = 40,
        //        Text = "Details",
        //        Bold = false,
        //        DrawBox = true,
        //        DrawBoxSolid = true,
        //        BoxColorRed = r,
        //        BoxColorBlue = b,
        //        BoxColorGreen = g
        //    });

        //    pdfRenderSettings.Elements.Add(new PDFRenderElement()
        //    {
        //        Name = "Service",
        //        Top = 320,
        //        Left = 48,
        //        Width = 150,
        //        Height = 20,
        //        Text = "Service to be performed",
        //        Bold = false,
        //        DrawBox = true,
        //        DrawBoxSolid = true,
        //        BoxColorRed = r,
        //        BoxColorBlue = b,
        //        BoxColorGreen = g
        //    });

        //    pdfRenderSettings.Elements.Add(new PDFRenderElement()
        //    {
        //        Name = "SpecimenReferral",
        //        Top = 336,
        //        Left = 305,
        //        Width = 250,
        //        Height = 25,
        //        Text = "Specimen Referral",
        //        Bold = true,
        //        DrawBox = true,
        //        DrawBoxSolid = true,
        //        BoxColorRed = r,
        //        BoxColorBlue = b,
        //        BoxColorGreen = g
        //    });

        //    pdfRenderSettings.TemplateFileName = "letter2.pdf";

        //    string _output_filename = "Test.PDF";

        //    string clientCode = "TestSurScan";

        //    //int rows = (int)Math.Round((double)(collectionFacilities.Count / pdfRenderSettings.CollectionSiteNumberOfColumns), MidpointRounding.AwayFromZero);
        //    PDFRenderElement clinicsElement = pdfRenderSettings.Elements.Where(x => x.Name.Equals("Clinics", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
        //    pdfRenderSettings.ClinicDefaultElement.FontSize = 8;
        //    Assert.IsNotNull(clinicsElement);
        //    string RenderSettingsString = JsonConvert.SerializeObject(pdfRenderSettings); //Serialize<PdfRenderSettings>(pdfRenderSettings);
        //    pDFengine.LoadTemplate(pdfRenderSettings.TemplateFileName);
        //    foreach(PDFRenderElement pe in pdfRenderSettings.Elements)
        //    {
        //        pDFengine.RenderElementOntoPDF(pe);

        //    }

        //    ParamGetClinicsForDonor p = new ParamGetClinicsForDonor();

        //    p.donor_id = 6;
        //    p._dist = 30;
        //    List<CollectionFacility> collectionFacilities = d.GetClinicsForDonor(p);
        //    Assert.IsTrue(collectionFacilities.Count > 0);

        //    int top = clinicsElement.Top;
        //    int left = clinicsElement.Left;
        //    int width = clinicsElement.Width;
        //    int height = clinicsElement.Height;
        //    int maxTextWidth = (int)Math.Floor((double)width / pdfRenderSettings.CollectionSiteNumberOfColumns);
        //    int ClinicsElementColumnWidth = (int)Math.Round((double)clinicsElement.Width / pdfRenderSettings.CollectionSiteNumberOfColumns, MidpointRounding.ToEven);
        //    string stringAddress = string.Empty;

        //    int currentColumn = 0;
        //    int FontSize = -1;
        //    int ThisRowHeight = -1;
        //    int ThisRowTop = 0;
        //    int totalHeight = 0;
        //    foreach (CollectionFacility c in collectionFacilities)
        //    {
        //        // create an element for the address
        //        PDFRenderElement thisClinicElement = new PDFRenderElement()
        //        {
        //            FontName = pdfRenderSettings.ClinicDefaultElement.FontName,
        //            FontSize = pdfRenderSettings.ClinicDefaultElement.FontSize,
        //            BoxAlpha = pdfRenderSettings.ClinicDefaultElement.BoxAlpha,
        //            BoxPadding = pdfRenderSettings.ClinicDefaultElement.BoxPadding,
        //            TextColorBlue = pdfRenderSettings.ClinicDefaultElement.TextColorBlue,
        //            TextColorGreen = pdfRenderSettings.ClinicDefaultElement.TextColorGreen,
        //            TextColorRed = pdfRenderSettings.ClinicDefaultElement.TextColorRed,
        //            Bold = pdfRenderSettings.ClinicDefaultElement.Bold,
        //            DrawBox = pdfRenderSettings.ClinicDefaultElement.DrawBox,
        //            DrawBoxSolid = pdfRenderSettings.ClinicDefaultElement.DrawBoxSolid,
        //            BoxColorRed = pdfRenderSettings.ClinicDefaultElement.BoxColorRed,
        //            BoxColorBlue = pdfRenderSettings.ClinicDefaultElement.BoxColorBlue,
        //            BoxColorGreen = pdfRenderSettings.ClinicDefaultElement.BoxColorGreen
        //        };

        //        // We've calculated a font size, so lete's use that.
        //        if (FontSize > 0) thisClinicElement.FontSize = FontSize;
        //        // create the text
        //        stringAddress = string.Empty;
        //        stringAddress = c.vendor_name + Environment.NewLine;
        //        stringAddress += c.vendor_address_1 + " " + c.vendor_address_2 + Environment.NewLine;
        //        stringAddress += c.vendor_city + " " + c.vendor_state + ", " + c.vendor_zip + Environment.NewLine;
        //        stringAddress += c.vendor_phone + Environment.NewLine;
        //        thisClinicElement.Text = stringAddress;
        //        // get it's width and height

        //        int[] aSize = pDFengine.GetElementContentSize(thisClinicElement);
        //        // drop the font size till it's less than half the column size

        //        while (aSize[0]> maxTextWidth && FontSize == -1)
        //        {
        //            thisClinicElement.FontSize = thisClinicElement.FontSize - 1;
        //            aSize = pDFengine.GetElementContentSize(thisClinicElement);
        //        }
        //        // once we have a working font size, we'll use it for everyone
        //        // TODO - go through all results to find the smallest font size for the widest address?
        //        FontSize = thisClinicElement.FontSize;
        //        // if the total height exceeds the clinics box - stop.
        //        if (totalHeight + aSize[1] > clinicsElement.Height) break;

        //        if (aSize[1] > ThisRowHeight) ThisRowHeight = aSize[1];
        //        // render it at the left of the box and the top of the row
        //        thisClinicElement.Top = top + ThisRowTop;
        //        thisClinicElement.Left = left + (ClinicsElementColumnWidth * currentColumn);
        //        thisClinicElement.Width = ClinicsElementColumnWidth;
        //        thisClinicElement.Height = aSize[1];
        //        pDFengine.RenderElementOntoPDF(thisClinicElement);
        //        // increase column count
        //        currentColumn++;
        //        // if it's > columns, go back to 0
        //        if (currentColumn == pdfRenderSettings.CollectionSiteNumberOfColumns)
        //        {
        //            ThisRowTop += ThisRowHeight;
        //            totalHeight += ThisRowHeight;
        //            ThisRowHeight = -1;
        //            currentColumn = 0;
        //        }

        //    }
        //    int elementHeight = pDFengine.GetTextHeight("X", clinicsElement);

        //    pDFengine.SaveCurrentDocument(_output_filename);

        //}

        ////private WriteOnPDFTemplate(PDFengine pDFengine, string StringToWrite)
        ////{
        ////}
        //public static string Serialize<T>(T obj)
        //{
        //    DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
        //    MemoryStream ms = new MemoryStream();
        //    serializer.WriteObject(ms, obj);
        //    string retVal = Encoding.UTF8.GetString(ms.ToArray());
        //    return retVal;
        //}

        //public static T Deserialize<T>(string json)
        //{
        //    T obj = Activator.CreateInstance<T>();
        //    MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
        //    DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
        //    obj = (T)serializer.ReadObject(ms);
        //    ms.Close();
        //    return obj;
        //}
    }
}