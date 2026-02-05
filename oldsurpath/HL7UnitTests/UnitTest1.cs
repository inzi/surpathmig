using BackendHelpers;
using HL7.Manager;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using RTF;
using SurPath.Business;
using SurPath.Data;
using SurPath.Entity;
using SurPath.Enum;
using SurPath.MySQLHelper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace HL7UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void HL7LabFileProcessTest()
        {
            string workingFolder = @"C:\APPREPORTS\SurPathReports\CRL";

            HL7.Manager.HL7Parser.DoHL7Parser(ReportType.LabReport, workingFolder.Trim());
        }

        [TestMethod]
        public void HL7MROFileProcessTest()
        {
            string workingFolder = @"C:\APPREPORTS\SurPathReports\CRL";

            HL7.Manager.HL7Parser.DoHL7Parser(ReportType.MROReport, workingFolder.Trim());
        }

        [TestMethod]
        public void DBTest()
        {
            int report_info_id = 4064971;
            MySqlCommand command;
            ParamHelper p = new ParamHelper();


        }

        [TestMethod]
        public void nameTest()
        {
            ParamHelper p = new ParamHelper();
            string ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString().Trim();
            MySqlConnection conn = new MySqlConnection(ConnectionString);

            string sqlQuery = "select * from donors where donor_mi is null limit 1;";
            MySqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, p.Params);
            int RowCount = 0;
            bool same_name = true;
            string donor_name = string.Empty;

            while (dr.Read())
            {

                string _donor_name = (string)dr["donor_first_name"].ToString() + " " + (string)dr["donor_mi"].ToString() + " " + (string)dr["donor_last_name"].ToString();

                if (RowCount == 1)
                {
                    // first row - store the temp name for comparison
                    donor_name = _donor_name;
                }
                else
                {
                    // if we get a different name, we set this to false and never check again
                    if (same_name == true)
                    {
                        if (!donor_name.Equals(_donor_name, StringComparison.InvariantCultureIgnoreCase))
                        {
                            same_name = false;
                        }
                    }

                }

            }
            dr.Close();

        }

        [TestMethod]
        public void regexTest()
        {
            Regex r = new Regex("N\\S");
            string s = @"N\S";
            bool i = r.IsMatch(s);

            Assert.IsTrue(i);



        }

        [TestMethod]
        public void EmailTest()
        {
            //MailManager m = new MailManager();
            //m.SendMail("chris@inzi.com", $"unit test {DateTime.Now.ToString()}", "This is an email from a unit test.");
            Email oMail = new Email();
            oMail.SendMailMessage("unit test body", $"Unit test email {DateTime.Now.ToString()}");


        }

        //[TestMethod]
        //public void PidHelperTest()
        //{
        //    PIDHelper p = new PIDHelper();

        //    p.Evaluate("636-74-6934");
        //}

        [TestMethod]
        public void AgeTest()
        {
            string d = "201803120000";
            DateTime now = DateTime.Now;
            DateTime _SpecimenCollectionDateTime;
            DateTime.TryParseExact(d, "yyyyMMddHHmm", CultureInfo.InvariantCulture, DateTimeStyles.None, out _SpecimenCollectionDateTime);
            TimeSpan elapsed = now.Subtract(_SpecimenCollectionDateTime);
            int daysAgo = (int)elapsed.TotalDays;

            //string slot = TimeAgo(daysAgo);
            Assert.IsFalse(daysAgo == 0);

        }


        private string MakeSafeSSID(string v)
        {
            string TempSSNID = new String(v.Where(Char.IsDigit).ToArray());
            TempSSNID = new string('0', 9) + TempSSNID;
            TempSSNID = TempSSNID.Substring(TempSSNID.Length - 9);
            return TempSSNID;
        }

        [TestMethod]
        public void TestSafeSSID()
        {
            Debug.WriteLine(MakeSafeSSID("fi23n293823rn2oi98"));
            Debug.WriteLine(MakeSafeSSID("TXDL12930515"));
            Debug.WriteLine(MakeSafeSSID("x22  f"));
            Debug.WriteLine(MakeSafeSSID("AR15Ammo"));


            var x = 1;
        }

        [TestMethod]
        public void PidHelperTest()
        {
            PIDHelper pidHelper = new PIDHelper();

            List<PidMask> pidMatches= pidHelper.Evaluate("636-74-6934");

            bool SSN = pidMatches.Where(x => (int)x.Type == (int)PidTypes.SSN).Count() > 0;

            int matches = pidMatches.Count;
        }

        [TestMethod]
        public void pullReport()
        {
            // public bool GetReportByID(int report_info_id, ReportInfo reportDetails, ref List<OBR_Info> obrList, ref RTFBuilderbase crlReport)
            ReportType reportType = ReportType.LabReport;
            int report_info_id = 3577452;
            ReportInfo reportDetails = null;
            List<OBR_Info> obrList = null;

            RTFBuilderbase crlReport = null;

            HL7ParserDao hl7ParserDao = new HL7ParserDao();
            if (hl7ParserDao.GetReportByID(report_info_id, reportDetails, ref obrList, ref crlReport))
            {
                string Rtf = crlReport.ToString();
                byte[] bytes = Encoding.ASCII.GetBytes(Rtf);
                string filename = "c:\\logs2\\report.rtf";

                if (File.Exists(filename)) File.Delete(filename);
                FileInfo fileInfo = new FileInfo(filename);
                FileStream fileStream = fileInfo.Open(FileMode.Create,
                    FileAccess.Write);

                // Write bytes to file
                fileStream.Write(bytes,
                    0, bytes.Length);
                fileStream.Flush();
                fileStream.Close();
            }


        }

        //[TestMethod]
        //public void pullDocument()
        //{
        //    // public bool GetReportByID(int report_info_id, ReportInfo reportDetails, ref List<OBR_Info> obrList, ref RTFBuilderbase crlReport)
            
        //    int document_id = 4064971;
        //    ReportInfo reportDetails = null;    
        //    List<OBR_Info> obrList = null;

        //    RTFBuilderbase crlReport = null;

        //    HL7ParserDao hl7ParserDao = new HL7ParserDao();
        //    hl7ParserDao.GetReportByID(report_info_id, reportDetails, ref obrList, ref crlReport);

        //    string Rtf = crlReport.ToString();
        //    byte[] bytes = Encoding.ASCII.GetBytes(Rtf);
        //    string filename = "c:\\logs2\\report.rtf";

        //    if (File.Exists(filename)) File.Delete(filename);
        //    FileInfo fileInfo = new FileInfo(filename);
        //    FileStream fileStream = fileInfo.Open(FileMode.Create,
        //        FileAccess.Write);

        //    // Write bytes to file
        //    fileStream.Write(bytes,
        //        0, bytes.Length);
        //    fileStream.Flush();
        //    fileStream.Close();
        //}

        [TestMethod]
        public void buildRTFMetaData()
        {
            string _metadata = $"{DateTime.Now.ToString("MM/dd/yyyy h:mm tt")} - {"this is a file.rtf"}";

            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(_metadata);
            string metadata = System.Convert.ToBase64String(plainTextBytes);
            

            var base64EncodedBytes = System.Convert.FromBase64String(metadata);
            string docmetadata = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            Assert.IsTrue(docmetadata == _metadata);

        }

        [TestMethod]
        public void CreateSH1Hash()
        {
            string reportData = "This is text to generate a SH1 hash for";

            while (reportData.Length < 5)
            {
                string _r = Path.GetRandomFileName();
                reportData += _r.Replace(".", ""); // Remove period.

            }

            byte[] bytes = Encoding.ASCII.GetBytes(reportData);
            string hash2;
            using (SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider())
            {

                hash2 = BitConverter.ToString(sha1.ComputeHash(bytes)).Replace("-", string.Empty);
            }

            Console.WriteLine(hash2);


        }

        [TestMethod]
        public void ParamHelperTest()
        {
            ParamHelper p = new ParamHelper();

            MySqlParameter ptest = new MySqlParameter("@DonorInitialClientId", "test1");

            p.Param = ptest;
            Assert.IsTrue(p.Param == ptest);
            p.Param = new MySqlParameter("@DonorInitialDepartmentId", "test1");
            p.Param = new MySqlParameter("@DonorRegistrationStatusValue", 5);
            p.Param = new MySqlParameter("@CreatedBy", "SYSTEM");
            p.Param = new MySqlParameter("@LastModifiedBy", "SYSTEM");

            MySqlParameter[] mps = p.Params;

            Assert.IsTrue(mps.Length == 5);
        }


    }
}