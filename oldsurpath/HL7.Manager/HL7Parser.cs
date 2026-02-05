using EnumsNET;
using RTF;
using Serilog;
using SurPath.Data;
using SurPath.Entity;
using SurPath.Enum;
using SurpathBackend;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace HL7.Manager
{
    public static class HL7Parser
    {
        public static ILogger _logger;

        public static string JsonParserMasks;

        public static BackendParserReportHelper ReportHelper;

        private static string _culture = ConfigurationManager.AppSettings["Culture"].ToString().Trim();

        public static void ParseMulitDonorQuest(string sourcePath, ref List<string> fileList)
        {
            //This processes Quest Lab Reports to handle and seperate into multiple files.
            _logger.Information("Parsing Mulit Donors in Quest Files - ");
            foreach (string reportFile in Directory.EnumerateFiles(sourcePath))
            {
                _logger.Information("Processing File:" + reportFile);
                string fileName = reportFile.Substring(reportFile.LastIndexOf('\\') + 1);

                string[] lines = File.ReadAllLines(reportFile);
                int donors = 0;
                foreach (string item in lines)
                {
                    string[] segment = item.Split('|');

                    //_logger.Information(segment[0].ToString());

                    if (segment[0] == "PID")
                    {
                        _logger.Information("Donor Found:" + segment[5]);
                    }
                    if (segment[0] == "MSH")
                    {
                        donors++;
                    }
                }
                if (donors > 1)
                {
                    CreateMultipleFiles(donors, sourcePath, fileName, lines, ref fileList);
                }
            }
        }

        public static void CreateMultipleFiles(int donors, string sourcePath, string fileName, string[] lines, ref List<string> fileList)
        {
            _logger.Information("Moving Donors to Multiple files");
            _logger.Information("Number of Donors:" + donors.ToString());
            int counter = 1;
            bool writemode = false;
            System.IO.StreamWriter writefile = null;
            foreach (string item in lines)
            {
                string[] segment = item.Split('|');

                if (segment[0] == "MSH")
                {
                    //read next
                    if (writemode)
                    {
                        writemode = false;
                        try
                        {
                            writefile.Close();
                        }
                        catch (Exception ex)
                        {
                            _logger.Error(ex.Message.ToString());
                            _logger.Error(ex.InnerException.ToString());
                        };
                    }
                    _logger.Information("######################################################################");
                    _logger.Information("OriginalFileName:" + fileName);
                    string newFileName = fileName.ToLower().Replace(".res", "_") + counter.ToString() + ".res";
                    _logger.Information("NewFileName:" + newFileName);
                    _logger.Information("######################################################################");
                    string concat = sourcePath + "\\" + newFileName;
                    fileList.Add(concat);
                    if (File.Exists(concat)) { File.Delete(concat); }
                    writefile = new System.IO.StreamWriter(concat, true);

                    counter++;
                    writemode = true;
                }

                if (writemode)
                {
                    writefile.WriteLine(item.ToString());
                    _logger.Information(item.ToString());
                }
            }
            writefile.Close();
            // multi files are created

            try
            {
                string oldmultifile = sourcePath + "\\" + fileName;
                string TargetFile = sourcePath + "\\ProcessedMulti\\" + fileName;
                string RawTargetFile = Path.GetFileNameWithoutExtension(oldmultifile);
                string Extention = Path.GetExtension(oldmultifile);
                int FileRevision = 0;
                if (File.Exists(TargetFile))
                {
                    while (File.Exists(TargetFile))
                    {
                        FileRevision++;
                        string sFileRevision = FileRevision.ToString("D4");
                        TargetFile = sourcePath + "\\ProcessedMulti\\" + RawTargetFile + "." + sFileRevision + Extention;
                    }
                }

                File.Move(oldmultifile, TargetFile);
            }
            catch (Exception ex)
            {
                _logger.Error($"{ex.Message}");
                _logger.Error(ex.ToString());
                _logger.Error($"Renaming old file back... [Not Implemented]");
            }
        }


        public static List<string> DeptLabCodes = new List<string>();

        public static void ParseMultiMRO(string sourcePath, ref List<string> fileList)
        {
            //This processes Quest Lab Reports to handle and seperate into multiple files.
            _logger.Information("Parsing Mulit MRO result File - ");
            foreach (string reportFile in Directory.EnumerateFiles(sourcePath))
            {
                _logger.Information("Processing File:" + reportFile);
                string fileName = reportFile.Substring(reportFile.LastIndexOf('\\') + 1);

                string[] lines = File.ReadAllLines(reportFile);

                int MSHCount = lines.ToList().Where(x => x.ToUpper().StartsWith("MSH|") == true).Count();
                if (MSHCount > 1)
                {
                    BreakSingleFileIntoMultiple(MSHCount, sourcePath, fileName, lines, ref fileList);
                }
            }
        }
        public static void BreakSingleFileIntoMultiple(int results, string sourcePath, string fileName, string[] lines, ref List<string> fileList)
        {
            _logger.Information($"Breaking {fileName} into to Multiple files");
            _logger.Information("Number of files to be created:" + results.ToString());
            int counter = 1;
            bool writemode = false;
            string filenameWOExt = Path.GetFileNameWithoutExtension(fileName);
            string fileExtension = Path.GetExtension(fileName);
            System.IO.StreamWriter writefile = null;
            foreach (string item in lines)
            {
                string[] segment = item.Split('|');

                if (segment[0] == "MSH")
                {
                    //read next
                    if (writemode)
                    {
                        writemode = false;
                        try
                        {
                            writefile.Close();
                        }
                        catch (Exception ex)
                        {
                            _logger.Error(ex.Message.ToString());
                            _logger.Error(ex.InnerException.ToString());
                        };
                    }
                    _logger.Information("######################################################################");
                    _logger.Information("OriginalFileName:" + fileName);
                    string newFileName = filenameWOExt + "_" + counter.ToString() + fileExtension;
                    _logger.Information("NewFileName:" + newFileName);
                    _logger.Information("######################################################################");
                    string concat = sourcePath + "\\" + newFileName;
                    fileList.Add(concat);
                    if (File.Exists(concat)) { File.Delete(concat); }
                    writefile = new System.IO.StreamWriter(concat, true);

                    counter++;
                    writemode = true;
                }

                if (writemode)
                {
                    writefile.WriteLine(item.ToString());
                    _logger.Information(item.ToString());
                }
            }
            writefile.Close();
            // multi files are created

            try
            {
                string oldmultifile = sourcePath + "\\" + fileName;
                if (!Directory.Exists(sourcePath + "\\ProcessedMulti\\")) Directory.CreateDirectory(sourcePath + "\\ProcessedMulti\\");
                string TargetFile = sourcePath + "\\ProcessedMulti\\" + fileName;
                string RawTargetFile = Path.GetFileNameWithoutExtension(oldmultifile);
                string Extention = Path.GetExtension(oldmultifile);
                int FileRevision = 0;
                if (File.Exists(TargetFile))
                {
                    while (File.Exists(TargetFile))
                    {
                        FileRevision++;
                        string sFileRevision = FileRevision.ToString("D4");
                        TargetFile = sourcePath + "\\ProcessedMulti\\" + RawTargetFile + "." + sFileRevision + Extention;
                    }
                }

                File.Move(oldmultifile, TargetFile);
            }
            catch (Exception ex)
            {
                _logger.Error($"{ex.Message}");
                _logger.Error(ex.ToString());
                _logger.Error($"Problem breaking {fileName} into multiple files!!!!");
            }
        }

        public static bool TestForNonContactPositive(string fileName)
        {
            string matches = string.Empty;
            bool result = false;
            if (ConfigurationManager.AppSettings.AllKeys.Contains("MRONoContactStrings"))
            {
                matches = ConfigurationManager.AppSettings["MRONoContactStrings"].ToString().Trim();
            }
            string[] ncMatches = matches.Split(','); // { "NON-CONTACT POSITIVE", "NO CONTACT Positive" };
            string fileContents = File.ReadAllText(fileName).ToUpper();

            foreach (string s in ncMatches)
            {
                if (fileContents.Contains(s.ToUpper()))
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        //public static void CreateMultipleFiles(int donors, string sourcePath, string fileName, string[] lines, ref List<string> fileList)
        //{
        //    _logger.Information("Moving Donors to Multiple files");
        //    _logger.Information("Number of Donors:" + donors.ToString());
        //    int counter = 1;
        //    bool writemode = false;
        //    System.IO.StreamWriter writefile = null;
        //    foreach (string item in lines)
        //    {
        //        string[] segment = item.Split('|');

        //        if (segment[0] == "MSH")
        //        {
        //            //read next
        //            if (writemode)
        //            {
        //                writemode = false;
        //                try
        //                {
        //                    writefile.Close();
        //                }
        //                catch (Exception ex)
        //                {
        //                    _logger.Error(ex.Message.ToString());
        //                    _logger.Error(ex.InnerException.ToString());
        //                };
        //            }
        //            _logger.Information("######################################################################");
        //            _logger.Information("OriginalFileName:" + fileName);
        //            string newFileName = fileName.ToLower().Replace(".res", "_") + counter.ToString() + ".res";
        //            _logger.Information("NewFileName:" + newFileName);
        //            _logger.Information("######################################################################");
        //            string concat = sourcePath + "\\" + newFileName;
        //            fileList.Add(concat);
        //            if (File.Exists(concat)) { File.Delete(concat); }
        //            writefile = new System.IO.StreamWriter(concat, true);

        //            counter++;
        //            writemode = true;
        //        }

        //        if (writemode)
        //        {
        //            writefile.WriteLine(item.ToString());
        //            _logger.Information(item.ToString());
        //        }
        //    }
        //    writefile.Close();
        //    // multi files are created

        //    try
        //    {
        //        string oldmultifile = sourcePath + "\\" + fileName;
        //        string TargetFile = sourcePath + "\\ProcessedMulti\\" + fileName;
        //        string RawTargetFile = Path.GetFileNameWithoutExtension(oldmultifile);
        //        string Extention = Path.GetExtension(oldmultifile);
        //        int FileRevision = 0;
        //        if (File.Exists(TargetFile))
        //        {
        //            while (File.Exists(TargetFile))
        //            {
        //                FileRevision++;
        //                string sFileRevision = FileRevision.ToString("D4");
        //                TargetFile = sourcePath + "\\ProcessedMulti\\" + RawTargetFile + "." + sFileRevision + Extention;
        //            }
        //        }

        //        File.Move(oldmultifile, TargetFile);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error($"{ex.Message}");
        //        _logger.Error(ex.ToString());
        //        _logger.Error($"Renaming old file back... [Not Implemented]");
        //    }
        //}
        public static void PopulateDeptLabCodes()
        {

            HL7ParserDao hl7ParserDao = new HL7ParserDao(_logger);
            
            DeptLabCodes = hl7ParserDao.DepatLabCodes();
        }
        public static bool ValidLab_Code(string code)
        {
            return DeptLabCodes.Where(x => x.Equals(code, StringComparison.InvariantCultureIgnoreCase)).Count() > 0;
        }

        public static void SyncSSNsLegacy()
        {
            HL7ParserDao hl7ParserDao = new HL7ParserDao(_logger);

            hl7ParserDao.UpdateDonorPIDsfromDonorTable();
        }
        public static void DoHL7Parser(ReportType reportType, string sourcePath)
        {
            // Possible bug - if file list has primary file, and it's a multi quest
            // verify that the files are added to the filelist that it's using
            // so it deletes them and cleans up properly.
            //string resultFileName = "CRL Report_";
            //To-do Write the log

            if (Directory.EnumerateFiles(sourcePath).Count() > 0)
            {
                string mismatchRecordIDList = string.Empty;

                int NumPassed = 0;
                List<string> ListOfFiles = Directory.EnumerateFiles(sourcePath).ToList();
                ListOfFiles = ListOfFiles.Where(x => Path.GetFileName(x).StartsWith(".") == false).ToList();
                int TotalTo = ListOfFiles.Count();
                //foreach (string reportFile in Directory.EnumerateFiles(sourcePath))
                foreach (string reportFile in ListOfFiles)
                {
                    _logger.Information(new String('V', 56));
                    _logger.Information(new String('>', 20) + " N E W  F I L E " + new string('<', 20));
                    _logger.Information(new String('^', 56));
                    NumPassed++;
                    _logger.Information("REPORT TYPE:" + reportType);
                    string fileName = reportFile.Substring(reportFile.LastIndexOf('\\') + 1);
                    _logger.Information($"fileName - {fileName}");
                    //Only process RPT and HL7 Files
                    if (fileName.ToUpper().EndsWith(".RPT") || fileName.ToUpper().EndsWith(".HL7") || fileName.ToUpper().EndsWith(".RES"))
                    {

                        _logger.Information("Processing Report File:" + reportFile);
                        ReportInfo reportDetails = null;
                        List<OBR_Info> obrList = new List<OBR_Info>();

                        RTFBuilderbase crlReport = new RTFBuilder();

                        string[] lines = File.ReadAllLines(reportFile);

                        int currentOBROrder = 0;
                        OBR_Info currentOBRInfo = null;
                        //string mroMMRecordId = string.Empty;

                        #region process line segments

                        string repSpecId = string.Empty;
                        string repClCode = string.Empty;
                        string repLabName = string.Empty;
                        string reportCheckSumData = string.Empty;
                        int DSPlines = 0;
                        foreach (string line in lines)
                        {

                            string[] segment = line.Split('|');
                            if (segment.Length > 0)
                            {
                                reportCheckSumData += line;
                                //_logger.Information($"({fileName}) Line Segment: " + segment[0].Trim());
                                if (segment[0].Trim() == "MSH"
                                    || segment[0].Trim() == "PID"
                                    || segment[0].Trim() == "PR1")
                                {
                                    if (reportDetails == null)
                                    {
                                        _logger.Information("Creating new reportDetails object");
                                        reportDetails = new ReportInfo();
                                        reportDetails.lab_report_source_filename = fileName;
                                        DSPlines = 0;
                                        reportCheckSumData = string.Empty;
                                    }

                                    GetReportDetails(segment, reportDetails, reportType);
                                    if (segment[0].Trim() == "PID")
                                    {
                                        _logger.Information($"SpecimenId - {reportDetails.SpecimenId.ToString()}");
                                    }
                                }
                                else if (segment[0].Trim() == "DSP" || segment[0].Trim() == "NTE")
                                {
                                    DSPlines++;
                                    //_logger.Information("Processing DSP in file" + reportFile);
                                    if (segment.Length == 4)
                                    {
                                        crlReport.AppendLine(ConvertAsciiToUnicode(segment[3]));
                                    }
                                    else
                                    {
                                        crlReport.AppendLine("");
                                    }
                                }
                                else if (segment[0].Trim() == "OBR")
                                {
                                    _logger.Information("Processing OBR in file" + reportFile);
                                    reportDetails.OBRSegment = line;
                                    OBR_Info obrInfo = GetOBRDetails(segment, reportType);
                                    obrList.Add(obrInfo);
                                    if (reportType == ReportType.QuestLabReport)
                                    {
                                        //reportDetails.QuestCode = obrInfo.OBRQuestCode;
                                        //_logger.Debug($"QuestCode set to obrInfo.OBRQuestCode data {obrInfo.OBRQuestCode.ToString()}");

                                        reportDetails.SpecimenId = obrInfo.QuestSpeciminID;
                                        _logger.Debug($"SpecimenId set to obrInfo.QuestSpeciminID data {obrInfo.QuestSpeciminID.ToString()}");
                                        //reportDetails.SpecimenCollectionDate = obrInfo.SpecimenCollectionDate;
                                        _logger.Debug($"SpecimenId set to obrInfo.QuestSpeciminID data {obrInfo.QuestSpeciminID.ToString()}");
                                    }
                                    currentOBRInfo = obrInfo;
                                    currentOBROrder = currentOBRInfo.TransmitedOrder;
                                }
                                else if (segment[0].Trim() == "ORC")
                                {
                                    reportDetails.ORCSegment = line;
                                    // We see client code in ORC 17 sometimes, like this: 0VN.MPOS.JCPLENEX^^JCPENNEY-LENEXA
                                    GetReportDetails(segment, reportDetails, reportType);
                                }
                                else if (segment[0].Trim() == "OBX")
                                {
                                    _logger.Information("Processing OBX in file " + reportFile);
                                    if (currentOBRInfo != null && currentOBRInfo.TransmitedOrder == currentOBROrder)
                                    {
                                        GetOBXDetails(currentOBRInfo, segment, reportType);
                                    }
                                }


                            }
                        }

                        // The CRLClientCode - we need it.
                        // quest finds it quick
                        // if it's empty, we'll use lab_code, which we tested
                        if (string.IsNullOrEmpty(reportDetails.CrlClientCode))
                        {
                            if (!(string.IsNullOrEmpty(reportDetails.lab_code)))
                            {
                                reportDetails.CrlClientCode = reportDetails.lab_code;
                            }
                            else
                            {
                                // we can try the OBR records
                                // First - do we only have one?
                                if (1 == obrList.Select(x => x.lab_code).Distinct().Count() && !(string.IsNullOrEmpty(obrList.Select(x => x.lab_code).Distinct().FirstOrDefault())))
                                {
                                    // There's only one, and OBR thinks it has it.
                                    // if it's not blank, lean on it.
                                    reportDetails.CrlClientCode = obrList.Select(x => x.lab_code).Distinct().FirstOrDefault();

                                }
                                else if (1 == obrList.Select(x => x.CrlClientCode).Distinct().Count() && !(string.IsNullOrEmpty(obrList.Select(x => x.CrlClientCode).Distinct().FirstOrDefault())))
                                {
                                    // lastly, we'll get this crlclientcode - even if it's a mismatch, so we can report on it.
                                    reportDetails.CrlClientCode = obrList.Select(x => x.CrlClientCode).Distinct().FirstOrDefault();
                                }

                            }
                        }


                        #endregion process line segments

                        string NRPstring = "Please contact SurScan (NRP)";

                        if (reportType == ReportType.MROReport)
                        {
                            _logger.Debug("Report Type is MROReport");
                            reportDetails.data_checksum = reportCheckSumData;
                            //TODO DSPlines - if it's zero we'll get an empty RTF file.
                            if (DSPlines == 0)
                            {
                                _logger.Information($"{fileName} has no DSP lines, so the MRO RTF report will be empty");
                                // TODO should we email on this?
                                crlReport.AppendLine(NRPstring);

                            }
                            updateToDatabase(reportDetails, reportType, crlReport, obrList, ref mismatchRecordIDList, sourcePath, fileName, reportFile, NumPassed, TotalTo);
                            _logger.Information("### MRO REPORT UPDATED to DB:" + fileName);
                            //mismatchRecordIDList = mroMMRecordId;
                        }
                        else if (reportType == ReportType.LabReport)
                        {
                            _logger.Debug("Report Type is LabReport");

                            //reportDetails.data_checksum = reportCheckSumData;
                            reportDetails.data_checksum = reportCheckSumData;
                            //TODO DSPlines - if it's zero we'll get an empty RTF file.
                            if (DSPlines == 0)
                            {
                                _logger.Information($"{fileName} has no DSP lines, so the RTF report will be empty");
                                // TODO should we email on this?
                                crlReport.AppendLine(NRPstring);

                            }
                            updateToDatabase(reportDetails, reportType, crlReport, obrList, ref mismatchRecordIDList, sourcePath, fileName, reportFile, NumPassed, TotalTo);
                            _logger.Information("### CRL LAB REPORT UPDATED to DB:" + fileName);
                            //mismatchRecordIDList = mroMMRecordId;
                        }
                        else if (reportType == ReportType.QuestLabReport)
                        {
                            _logger.Debug("Report Type is Quest Lab Report");

                            try
                            {
                                reportDetails.data_checksum = reportCheckSumData;

                                updateToDatabase(reportDetails, reportType, crlReport, obrList, ref mismatchRecordIDList, sourcePath, fileName, reportFile, NumPassed, TotalTo);
                                _logger.Information("### Quest LAB REPORT UPDATED to DB:" + fileName);
                                //mismatchRecordIDList = mroMMRecordId;
                            }
                            catch (System.Exception ex)
                            {
                                var shizz = ex.StackTrace.ToString();
                                var blah = shizz;
                            }
                        }



                    }
                }

                #region Copy files to Processed Dir


                // This takes pdf files and makes them reports
                // for a donor, but it needs a donor id and a report info id.
                // if we have a mismatch, we need to move the pdf with the mistmatch file
                // because i3screen has the pdf broken out.


                //ListOfFiles.Where(f => Path.GetExtension(f).Equals(".PDF", StringComparison.InvariantCultureIgnoreCase) == true).ToList().ForEach(reportFile =>
                //    {
                //        string fileName = Path.GetFileName(reportFile);

                //        UploadPDFReport(reportFile);

                //        File.Copy(reportFile, sourcePath + "\\Processed\\" + fileName, true);
                //        _logger.Information("Processed Dir - Deleting File" + fileName);
                //        File.Delete(reportFile);
                //    }

                //    );

                foreach (string reportFile in ListOfFiles) //Directory.EnumerateFiles(sourcePath))
                {
                    string fileName = Path.GetFileName(reportFile);

                    //To-do Write the log
                    // TODO what about MRO files from i3Screen - are they getting uploaded?
                    if (fileName.ToUpper().EndsWith(".PDF"))
                    {

                        UploadPDFReport(reportFile);

                        File.Copy(reportFile, sourcePath + "\\Processed\\" + fileName, true);
                        _logger.Information("Processed Dir - Deleting File" + fileName);
                        File.Delete(reportFile);
                    }
                }

                #endregion Copy files to Processed Dir

                #region Check for invalid Files and send email

                // we're replacing this with a better report

                //if (!string.IsNullOrEmpty(mismatchRecordIDList))
                //{
                //    //Need to send the mail to with the .RPT file & .PDF file
                //    if (reportType == ReportType.MROReport)
                //    {
                //        resultFileName = "MRO Report_";
                //    }
                //    string Subject = String.Empty;
                //    string Body = String.Empty;
                //    Email oMail = new Email();
                //    oMail.SendMail(mismatchRecordIDList, new String[] { Subject, Body }, resultFileName);
                //    _logger.Information("ERROR Found - Invalid File(s)");
                //    _logger.Information(mismatchRecordIDList, new String[] { Subject, Body }, resultFileName);
                //}

                #endregion Check for invalid Files and send email
            }
        }

        #region Private Methods

        public static void SendFinalProcessedEmail(List<string> memStats)
        {
            // new report will be single report
            // maybe we need to create a discard report
            if (ReportHelper.Items.Where(x => x.mismatch == true).ToList().Count < 1 && ReportHelper.Items.Where(x => x.donor_backfilled == true).ToList().Count < 1)
            {
                // no mismatches
                return;
            }

            bool DryRun = false;
            if (ConfigKeyExists("DryRun")) bool.TryParse(ConfigurationManager.AppSettings["DryRun"].ToString(), out DryRun);

            // TIP - To align a string to the left, we use pattern with comma, followed by negative number
            // like this: LogFile.Write(string.Format("{0,-10} {1,-11} {2,-30} {3}", ...));
            // for right alignment, we use a positive number

            // document properties
            BackendParserReportbuilder report = new BackendParserReportbuilder();
            string _format = string.Empty;

            //report.Width = 250;
            report.Header = "PARSER RESULT REPORT " + DateTime.Now.ToString();
            if (DryRun==true) report.Header = "[TEST RUN] PARSER RESULT REPORT " + DateTime.Now.ToString();
            report.BlankLines(2);
            report.Line = $"Files Processed: { ReportHelper.Items.Count()}";
            report.Line = $"Files Imported: { ReportHelper.Items.Where(x => x.mismatch == false).Count()}";
            report.Line = $"Files MisMatched: { ReportHelper.Items.Where(x => x.mismatch == true).Count()}";
            report.Line = $"Donors Backfiled: { ReportHelper.Items.Where(x => x.donor_backfilled == true).Count()}";
            memStats.ForEach(l =>
                    report.Line = l
                );
            report.BlankLines(2);
            report.Header = "MISSING CLIENT CODES";
            report.BlankLines();
            report.Line = "The following client codes have not been entered, so the parser could not add this file";
            report.Line = "to the database. They must be created for a client department in order to match the file to a client.";
            report.Line = "Once they are created for a client, these files will match.";
            report.Line = "If the donor & test are not found, they will be created.";
            report.BlankLines();

            List<BackendParserReportHelperItem> items = ReportHelper.Items.Where(x => x.mismatch == true && string.IsNullOrEmpty(x.CrlClientCode.Trim()) == false).Where(x => x.LabCodeFound == false).ToList();
            //string _format = "{0,-10} {1,-11} {2,-30} {3}";
            _format = "{0, -32} {1, -20}";
            report.Line = string.Format(_format, "Client Code", "No. of Files Waiting");
            items.Where(r=>r.ReportTypeEnum!=ReportType.QuestLabReport).GroupBy(lc => lc.CrlClientCode).ToList()
                .ForEach(r =>
                         report.Line = string.Format(_format, r.Key, r.Select(x => x.FileName).Distinct().Count().ToString())
                );

            //string _format = "{0,-10} {1,-11} {2,-30} {3}";
            _format = "{0, -32} {1, -20}";
            report.Line = string.Format(_format, "Quest Code", "No. of Files Waiting");
            items.Where(r => r.ReportTypeEnum == ReportType.QuestLabReport).GroupBy(lc => lc.CrlClientCode).ToList()
                 .ForEach(r =>
                          report.Line = string.Format(_format, r.Key, r.Select(x => x.FileName).Distinct().Count().ToString())
                 );
            report.BlankLines(3);
            report.NewHorizontalBar('.');
            report.BlankLines(2);
            report.BlankLines(3);
            report.NewHorizontalBar('.');
            report.BlankLines(2);


            items = ReportHelper.Items.Where(x => x.mismatch == true).ToList();

            report.Header = "MISMATCHES BY TYPE";
            report.BlankLines();
            report.Line = "This breaks down mismatches by report type";
            report.BlankLines();
            _format = "{0, -15} {1, -5}";
            report.Line = string.Format(_format, "Report Type", "No. of Files Waiting");
            items.GroupBy(g => g.ReportTypeEnum).ToList()
                .ForEach(r =>
                    report.Line = string.Format(_format, ((ReportType)r.Key).ToString(), r.Select(x => x.FileName).Distinct().Count().ToString())
                );
            report.BlankLines(3);
            report.NewHorizontalBar('.');
            report.BlankLines(2);

            items = ReportHelper.Items.Where(x => x.mismatch == true).ToList();

            report.Header = "MISMATCHES BY AGE";
            report.BlankLines();
            report.Line = "This breaks down mismatches by AGE";
            report.BlankLines();
            _format = "{0, -20} {1, -15} ";
            report.Line = string.Format(_format, "AGE", "No. of Files");

            TimeAgo[] enums = (TimeAgo[])Enum.GetValues(typeof(TimeAgo));



            foreach (int i in Enum.GetValues(typeof(TimeAgo)))
            {
                report.Line = string.Format(_format, ((TimeAgo)i).AsString(EnumFormat.Description), items.Where(r => (int)r.Age == i).Count().ToString());
            }


            report.BlankLines(3);
            report.NewHorizontalBar('.');
            report.BlankLines(2);

            items = ReportHelper.Items.Where(x => x.mismatch == true && x.NoPIDInFile).ToList();
            items = items.OrderBy(x => x.SpecimenID).ToList();

            report.Header = "NO PID FILES";
            report.BlankLines();
            report.Line = "These are files that couldn't be imported because there was PID for donor, and specimen ID was not found.";
            report.BlankLines();
            _format = "{0, -15} {1, -20} {2, -50} {3, -50}";
            report.Line = string.Format(_format, "report Type", "Specimen ID", "Donor", "File Name");
            items.OrderBy(g => g.ReportTypeEnum).ToList()
                .ForEach(r =>
                    report.Line = string.Format(_format, ((ReportType)r.ReportTypeEnum).ToString(), r.SpecimenID, r.Donor, r.FileName)

                );
            report.BlankLines(3);
            report.NewHorizontalBar('.');
            report.BlankLines(2);

            items = ReportHelper.Items.Where(x => x.mismatch == true).ToList();
            items = items.OrderBy(x => x.SpecimenID).ToList();

            report.Header = "MISMATCHES BY REASON";
            report.BlankLines();
            report.Line = "Mismatches occur when file if neither the specimen or client cannot be found.";
            report.Line = "A specimen mismatch and a client mismatch are fatal mismatches.";
            report.Line = "";
            report.BlankLines();
            _format = "{0, -15} {1, -20} {2, -50} {3, -8} {4, -8} {5, -8} {6, -8} {7,-7} {8,-7}";
            report.Line = string.Format(_format, "report", "Spec", "File", "Donor", "Multi","Spec", "Client", "", "Could");
            report.Line = string.Format(_format, "Type", "ID", "Name", "Found", "Donors", "Found", "Found", "Fatal", "Backfill");
            items.OrderBy(g => g.ReportTypeEnum).ToList()
                .ForEach(r =>
                    report.Line = string.Format(_format, ((ReportType)r.ReportTypeEnum).ToString(), r.SpecimenID, r.FileName,
                        (r.DonorFound == true) ? "Y" : "N",
                        (r.multiple_donor_pid_found == true) ? $"Y ({r.donor_pid_match_count})" : "N",
                        (r.SpecimenIDMatched == true) ? "Y" : "N",
                        (r.client_department_id>0) ? "Y" : "N",
                        ((r.SpecimenIDFound == false) && (r.client_department_id <1)) ?"Y":"N",
                        ((r.SpecimenIDFound == true) && (r.client_department_id >0) && r.DonorFound == false && r.multiple_donor_pid_found == false) ?"Y": "N"
                    )

                );
            report.BlankLines(3);
            report.NewHorizontalBar('.');
            report.BlankLines(2);

            // Show donors with multiple "open" tests 
            items = ReportHelper.Items.Where(x => x.DonorFound == false && x.multiple_donor_pid_found == true && x.donor_name != string.Empty).ToList();
            report.Header = "DONORS WITH MULTIPLE TEST RECORDS";
            report.BlankLines();
            report.Line = "These donors were mismatched because they have multiple tests awaiting results.";
            report.Line = "The parser was unable to determine which test to associate results to.";
            report.BlankLines();
            _format = "{0, -15} {1, -20} {2, -50} {3, -50}";
            report.Line = string.Format(_format, "report Type", "Specimen ID", "File Name", "Donor Name");
            items.OrderBy(g => g.ReportTypeEnum).ToList()
                .ForEach(r =>
                    report.Line = string.Format(_format, ((ReportType)r.ReportTypeEnum).ToString(), r.SpecimenID, r.FileName, r.donor_name)

                );
            report.BlankLines(3);
            report.NewHorizontalBar('.');
            report.BlankLines(2);

            // DONORS not backfilled
            // a report of donors, had they been created, that would cause file to match

            // No Quest Code found
            // A report of quest files where the quest code is not found in client department

            items = ReportHelper.Items.Where(x => x.donor_backfilled == true).ToList();
            items = items.OrderBy(x => x.Donor).ToList();

            report.Header = "CREATED DONORS";
            report.BlankLines();
            report.Line = "These donors were automatically created because they couldn't be found.";
            report.BlankLines();
            _format = "{0, -15} {1, -20} {2, -50}";
            report.Line = string.Format(_format, "report Type", "Specimen ID", "Donor");
            items.OrderBy(g => g.ReportTypeEnum).ToList()
                .ForEach(r =>
                    report.Line = string.Format(_format, ((ReportType)r.ReportTypeEnum).ToString(), r.SpecimenID, r.Donor)

                );
            report.BlankLines(3);
            report.NewHorizontalBar('.');
            report.BlankLines(2);

            items = ReportHelper.Items;


            report.Header = "MISSING CLIENT CODE DETAILS";
            report.BlankLines();
            report.Line = "This is a list of mismatched specimen IDs because of missing Client Codes.";
            report.Line = "Passes is the number of times this file has not matched a lab code.";
            report.BlankLines();
            _format = "{0, -15} {1, -20} {2, -7} {3, -32} {4, -50}";
            report.Line = string.Format(_format, "Type", "Specimen ID", "Passes", "Client Code", "Donor");
            items.OrderByDescending(o => o.mismatched_count).ToList()
                .ForEach(r =>
                {
                    report.Line = string.Format(_format, ((ReportType)r.ReportTypeEnum).ToString(), r.SpecimenID, r.mismatched_count.ToString(), r.CrlClientCode, r.Donor);
                    report.Line = $"{new String(' ', 10)}File: {r.FileName}";
                    report.Line = $"{new String(' ', 10)}Collection D/T: {r.CollectionDate}";


                    report.BlankLines();
                }

            );
            report.BlankLines(3);
            report.NewHorizontalBar('.');
            report.BlankLines(2);


            items = ReportHelper.Items.Where(x => x.mismatch == true).ToList();

            report.Header = "MISMATCH ORC - OBR DETAILS";
            report.BlankLines();
            report.Line = "This section shows the OBR information from the file, to assist in resolving the mismatch";
            report.BlankLines();
            _format = "{0, -15} {1, -20} {2, -50}";
            report.Line = string.Format(_format, "report Type", "Specimen ID", "File Name");
            items.OrderBy(g => g.ReportTypeEnum).ToList()
                .ForEach(r =>
                    {
                        report.Line = string.Format(_format, ((ReportType)r.ReportTypeEnum).ToString(), r.SpecimenID, r.FileName);
                        report.Line = $"{new String(' ', 10)}ORC: {r.ORC}";
                        report.Line = $"{new String(' ', 10)}OBR: {r.OBR}";
                        report.BlankLines();
                    }

                );
            report.BlankLines(3);
            report.NewHorizontalBar('.');
            report.BlankLines(2);


            items = ReportHelper.Items.Where(x => string.IsNullOrEmpty(x.CrlClientCode) == true).ToList();

            report.Header = "NO CLIENT CODE FOUND IN FILE";
            report.BlankLines();
            report.Line = "This is a list of files where the client could not be determined";
            report.BlankLines();
            _format = "{0, -32} {1, -50}";
            report.Line = string.Format(_format, "Specimen ID", "File");
            items.OrderBy(g => g.ReportTypeEnum).ToList()
                .ForEach(r =>
                    {
                        report.Line = string.Format(_format, r.SpecimenID, r.FileName);
                    }
                );
            report.BlankLines(3);
            report.NewHorizontalBar('.');
            report.BlankLines(2);



            items = ReportHelper.Items;

            report.Header = "MISMATCHES BY AGE";
            report.BlankLines();
            report.Line = "This is a list of no contact MRO reports";
            report.BlankLines();
            _format = "{0, -11} {1, -20} {2, -32} {3, -50}";
            report.Line = string.Format(_format, "MisMatch", "Specimen ID", "Client Code", "Donor");
            items.OrderBy(g => g.ReportTypeEnum).ToList()
                .ForEach(r =>
                {
                    report.Line = string.Format(_format, (r.mismatch) ? "Yes" : "No", r.SpecimenID, r.CrlClientCode, r.Donor);
                    report.Line = $"{new String(' ', 10)}CLIENT: {r.Client} DEPT: {r.ClientDepartment}";
                    report.BlankLines();

                }
                );
            report.BlankLines(3);
            report.NewHorizontalBar('.');
            report.BlankLines(2);



            items = ReportHelper.Items.Where(x => x.NonContactPositive == true & x.ReportTypeEnum == ReportType.MROReport).ToList();

            report.Header = "NO CONTACT POSITIVES (BETA)";
            report.BlankLines();
            report.Line = "This is a list of no contact MRO reports";
            report.BlankLines();
            _format = "{0, -11} {1, -20} {2, -32} {3, -50}";
            report.Line = string.Format(_format, "MisMatch", "Specimen ID", "Client Code", "Donor");
            items.OrderBy(g => g.ReportTypeEnum).ToList()
                .ForEach(r =>
                {
                    report.Line = string.Format(_format, (r.mismatch) ? "Yes" : "No", r.SpecimenID, r.CrlClientCode, r.Donor);
                    report.Line = $"{new String(' ', 10)}CLIENT: {r.Client} DEPT: {r.ClientDepartment}";
                    report.BlankLines();

                }
                );
            report.BlankLines(3);
            report.NewHorizontalBar('.');
            report.BlankLines(2);


            report.Line = "END OF REPORT";

            // save the file

            // PDF 
            //BackendLogic bl = new BackendLogic("PARSER",_logger);
            // TODO add a method to render ReportText to a PDF
            // check SendNotification for it sending


            // TODO put report in CSV format

            // email the report

            Email oMail = new Email();
            string LogToAddress = ConfigurationManager.AppSettings["LogToAddress"].ToString().Trim();
            string Title = "Mismatched Report v2 " + DateTime.Now.ToString();
            if (DryRun == true) Title = "[TEST RUN] " + Title;
            //oMail.SendMailMessageEnhanced(LogToAddress, report.ReportText, Title, bl.CreatePDFFromText(report.ReportText, Title), "application/pdf", Title + ".PDF");
            //oMail.SendMailMessageEnhanced(LogToAddress, report.ReportText, Title, Encoding.ASCII.GetBytes( report.ReportText), "text/plain", Title + ".txt");
            oMail.SendMailMessageEnhanced(LogToAddress, report.ReportText, Title);
            //oMail.SendMailAttachment(new String[] { Subject, Body }, logProcessedPath, LogFileEmailTo);


            //////////Need to send the mail to with the .RPT file & .PDF file
            ////////if (reportType == ReportType.MROReport)
            ////////{
            ////////    resultFileName = "MRO Report_";
            ////////}
            ////////

            //////DirectoryInfo logDirInfo = null;
            //////FileInfo logFileInfo;
            //////FileInfo logProcessedInfo = null;
            //////string logFilePath;


            //////string LogFileEmailTo = String.Empty;
            //////if (ConfigurationManager.AppSettings.AllKeys.Contains("LogFileEmailTo"))
            //////{
            //////    LogFileEmailTo = ConfigurationManager.AppSettings["LogFileEmailTo"].ToString().Trim();
            //////}

            ////////string logFilePath = "D:\\Logs\\";
            //////string file = "MismatchErrorLog";
            //////string fileextension = "." + "txt";
            //////string logProcessedPath = logFilePath + "HL7Processed\\" + file + "_" + Guid.NewGuid().ToString() + fileextension;
            //////if (!Directory.Exists(logFilePath + "HL7Processed\\")) Directory.CreateDirectory(logFilePath + "HL7Processed\\");
            //////logFilePath = logFilePath + file + fileextension;
            //////logFileInfo = new FileInfo(logFilePath);
            //////logProcessedInfo = new FileInfo(logProcessedPath);
            //////logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);

            //////if (logFileInfo.Exists && IsValidEmail(LogFileEmailTo))
            //////{
            //////    File.Copy(logFilePath, logProcessedPath);

            //////    string Subject = String.Empty;
            //////    string Body = "Mismatched Report" + DateTime.Now.ToString();
            //////    Email oMail = new Email();
            //////    string LogToAddress = ConfigurationManager.AppSettings["LogToAddress"].ToString().Trim();
            //////    oMail.SendMailAttachment(new String[] { Subject, Body }, logProcessedPath, LogFileEmailTo);

            //////    //_logger.Information("Final Email Sent");
            //////    //_logger.Information(mismatchRecordIDList, new String[] { Subject, Body }, resultFileName);
            //////    //_logger.Information("MRO Log File moved to Processed");

            //////    File.Delete(logFilePath);
            //////    _logger.Information("MismatchedFileExists - FileCopied");
            //////}
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        public static bool ConfigKeyExists(string _configkey)
        {
            return ConfigurationManager.AppSettings[_configkey] != null;
        }
        private static void updateToDatabase(ReportInfo reportDetails, ReportType reportType, RTFBuilderbase crlReport, List<OBR_Info> obrList,
            ref string mismatchRecordIDList, string sourcePath, string fileName, string reportFile, int NumPassed, int TotalTo)
        {

            HL7ParserDao hl7ParserDao = new HL7ParserDao(_logger);
            try
            {
                if (reportDetails != null)
                {
                    
                    bool archiveExistingReport = hl7ParserDao.CheckDuplicateSpecimenId(reportType, reportDetails.SpecimenId);
                    _logger.Debug($"archiveExistingReport = {archiveExistingReport}");
                    if (archiveExistingReport)
                    {
                        _logger.Information($"This is a duplicate report for a sample ID, if new, we'll archive the old report.");
                    }
                    else
                    {
                        _logger.Information($"This is not a duplicate report for a sample ID.");
                    }

                    bool DryRun = false;
                    if (ConfigKeyExists("DryRun")) bool.TryParse(ConfigurationManager.AppSettings["DryRun"].ToString(), out DryRun);

                    ReturnValues returnValues = new ReturnValues();
                    BackendParserHelper backendParserHelper = new BackendParserHelper(reportFile, _logger);
                    try
                    {
                        bool _parseResults = hl7ParserDao.UpdateReport(reportType, reportDetails, obrList, crlReport, archiveExistingReport, returnValues, fileName, NumPassed, TotalTo, backendParserHelper);
                        backendParserHelper.ReportHelperItem.NonContactPositive = TestForNonContactPositive(reportFile);
                        backendParserHelper.ReportHelperItem.OBR = reportDetails.OBRSegment;
                        backendParserHelper.ReportHelperItem.ORC = reportDetails.ORCSegment;
                        // Get report items off of backendParserHelper
                        ReportHelper.Items.Add(backendParserHelper.ReportHelperItem);
                        _logger.Debug($"hl7ParserDao.UpdateReport results: {_parseResults}");
                        if (_parseResults && DryRun == false)
                        {
                            // Move mismatches aside
                            if (backendParserHelper.ReportHelperItem.donor_test_info_id == 0)
                            {
                                _logger.Information($"returnValues.DonorTestInfoId is 0... no test info record");
                                _logger.Information($"returnValues.ReportId = {returnValues.ReportId}");
                                _logger.Information($"returnValues.MismatchRecordId = {returnValues.MismatchRecordId }");
                                // This is a mismatch - We've set the file aside to try again
                                _logger.Information($"[updateToDatabase] Moving {fileName} to mismatched folder");

                                File.Copy(reportFile, sourcePath + "\\Mismatched\\" + fileName, true);


                                // Because we don't add mismatches to the database, we can't upload the associated PDF - os let's check for it.
                                string MatchPDFFile = Path.GetFileNameWithoutExtension(fileName) + ".PDF";
                                if (File.Exists(MatchPDFFile))
                                {
                                    File.Copy(reportFile, sourcePath + "\\Mismatched\\" + MatchPDFFile, true);

                                }
                            }
                            else
                            {
                                // this file is processed
                                if (returnValues.MroAttentionFlag)
                                {
                                    var _mroFile = ConfigurationManager.AppSettings["MROReportFileOutboundPath"].ToString().Trim() + "\\" + fileName;
                                    _logger.Debug($"MroAttentionFlag is true - copying file {reportFile} to {_mroFile}");

                                    File.Copy(reportFile, _mroFile, true);
                                }
                                _logger.Debug($"[updateToDatabase] Copying file {reportFile} to {sourcePath + "\\Processed\\" + fileName}");

                                File.Copy(reportFile, sourcePath + "\\Processed\\" + fileName, true);
                            }
                            // the file is copied to processed or mismatched, it's ok to delete it
                            _logger.Debug($"[updateToDatabase] Removing file {reportFile}");

                            var retryCounter = 5;
                            while (File.Exists(reportFile))
                            {
                                _logger.Debug($"[updateToDatabase] Delete try {5 - retryCounter}, will email if unsuccessful to avoid DB bloat");
                                File.Delete(reportFile);
                                retryCounter--;
                                if (retryCounter < 0)
                                {
                                    throw new Exception($"[updateToDatabase] Unable to delete file {reportFile}");
                                }
                            }
                        }


                        // this section moves mismatches for mro stuff and add to the report file,
                        // which I'm replacing.
                        //if (_parseResults)
                        //{
                        //    // if we got a test info id, we matched and it's processed.
                        //    // if we didn't, it's a mismatch
                        //    // Set the file aside 
                        //    _logger.Information($"returnValues.DonorTestInfoId = {returnValues.DonorTestInfoId}");
                        //    if (returnValues.DonorTestInfoId == 0)
                        //    {
                        //        _logger.Information($"returnValues.DonorTestInfoId is 0... no test info record");
                        //        _logger.Information($"returnValues.ReportId = {returnValues.ReportId}");
                        //        _logger.Information($"returnValues.MismatchRecordId = {returnValues.MismatchRecordId }");
                        //        // This is a mismatch - We've set the file aside to try again

                        //        if (returnValues.MismatchRecordId != 0)
                        //        {
                        //            if (string.IsNullOrEmpty(mismatchRecordIDList))
                        //            {
                        //                mismatchRecordIDList = returnValues.MismatchRecordId.ToString();
                        //            }
                        //            else
                        //            {
                        //                mismatchRecordIDList = mismatchRecordIDList + "," + returnValues.MismatchRecordId.ToString();
                        //            }

                        //            _logger.Information($"[updateToDatabase] mismatchRecordIDList {mismatchRecordIDList.Length.ToString()} \r\n{mismatchRecordIDList}");
                        //        }
                        //    }

                        //    // non mro's to mismatch
                        //    if (reportType != ReportType.MROReport)
                        //    {
                        //        if (returnValues.MismatchRecordId != 0 || (reportDetails.ClientDepartmentId == 0))//The second or on this was added to check for mismatched Quest files.
                        //        {
                        //            File.Copy(reportFile, sourcePath + "\\Mismatched\\" + fileName, true);
                        //            string repSpecId = string.Empty;
                        //            string repClCode = string.Empty;
                        //            string repLabName = string.Empty;
                        //            try
                        //            {
                        //                repSpecId = reportDetails.SpecimenId.ToString();
                        //            }
                        //            catch
                        //            {
                        //                repSpecId = "";
                        //            }
                        //            try
                        //            {
                        //                repClCode = reportDetails.CrlClientCode.ToString();
                        //            }
                        //            catch
                        //            {
                        //                repClCode = "";
                        //            }
                        //            try
                        //            {
                        //                repLabName = reportDetails.LabName.ToString();
                        //            }
                        //            catch
                        //            {
                        //                repLabName = "";
                        //            }
                        //            _logger.Information($"Writing to log: {repSpecId}, {repClCode}, {repLabName}, {fileName}");
                        //            WriteClientLogFileMismatchedTrue(repSpecId, repClCode, repLabName, fileName, "CRL");
                        //            _logger.Information($"Log File Update Complete");
                        //        }
                        //        else
                        //        {
                        //            if (returnValues.MroAttentionFlag)
                        //            {
                        //                File.Copy(reportFile, ConfigurationManager.AppSettings["MROReportFileOutboundPath"].ToString().Trim() + "\\" + fileName, true);
                        //            }
                        //            File.Copy(reportFile, sourcePath + "\\Processed\\" + fileName, true);
                        //        }
                        //        File.Delete(reportFile);
                        //    }

                        //}
                        //else
                        //{
                        //    //Need to send the mail to with the .RPT
                        //}
                    }
                    catch (System.Exception ex)
                    {
                        _logger.Error(ex.Message);
                        _logger.Error(ex.InnerException.ToString());
                        var blah = ex.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                
                _logger.Error(ex.ToString());
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                throw;
            }
        }

        private static void GetReportDetails(string[] segment, ReportInfo reportDetails, ReportType reportType)
        {

            if (reportType == ReportType.LabReport || reportType == ReportType.MROReport)
            {
                if (segment[0].Trim() == "MSH")
                {
                    #region MSH

                    if (segment[2].Trim() != string.Empty)
                    {
                        reportDetails.LabName = segment[2].Trim();
                    }

                    if (segment[3].Trim() != string.Empty)
                    {
                        reportDetails.LabCode = segment[3].Trim();
                    }

                    if (segment[6].Trim() != string.Empty)
                    {
                        reportDetails.LabReportDate = segment[6].Trim();
                    }

                    #endregion MSH
                }
                else if (segment[0].Trim() == "PID")
                {
                    #region PID

                    if (segment[2].Trim() != string.Empty)
                    {
                        reportDetails.SpecimenId = segment[2].Trim();
                    }

                    if (segment[3].Trim() != string.Empty)
                    {
                        reportDetails.LabSampleId = segment[3].Trim();
                    }

                    if (segment[4].Trim() != string.Empty)
                    {
                        //reportDetails.PID = segment[4].Trim();
                        reportDetails.PID_NODASHES_4 = segment[4].Trim(); 
                    }

                    if (segment[5].Trim() != string.Empty)
                    {
                        string[] donorName = segment[5].Trim().Split('^');

                        if (donorName.Length > 0)
                        {
                            if (donorName.Length == 1)
                            {
                                reportDetails.DonorLastName = donorName[0].Trim();
                            }
                            else if (donorName.Length == 2)
                            {
                                reportDetails.DonorLastName = donorName[0].Trim();
                                reportDetails.DonorFirstName = donorName[1].Trim();
                            }
                            else if (donorName.Length == 3)
                            {
                                reportDetails.DonorLastName = donorName[0].Trim();
                                reportDetails.DonorFirstName = donorName[1].Trim();
                                reportDetails.DonorMI = donorName[2].Trim();
                            }
                        }
                    }

                    if (segment[7].Trim() != string.Empty)
                    {
                        reportDetails.DonorDOB = segment[7].Trim();
                    }

                    if (segment[8].Trim() != string.Empty)
                    {
                        reportDetails.DonorGender = segment[8].Trim();
                    }
                    // lab account number lives in 18 sometimes, like this: PID|1|2046973818|N3480978|XXXXXXXX|ENLOE^KAREN^||19760516||||||816-433-9412|||||0VN.MPOS.JCPLENEX|KXXXXXXX

                    if (segment.Length >= 18)
                    {
                        if (segment[18].Trim() != string.Empty)
                        {
                            reportDetails.LabAccountNumber = segment[18].Trim();
                            if (!reportDetails.FoundDeptByLabCode)
                            {
                                if (!(string.IsNullOrEmpty(reportDetails.LabAccountNumber)))
                                {

                                    if (ValidLab_Code(reportDetails.LabAccountNumber))
                                    {
                                        reportDetails.FoundDeptByLabCode = true;
                                        reportDetails.lab_code = reportDetails.LabAccountNumber;

                                    }
                                }
                            }
                        }


                    }
                    if (segment.Length > 19)
                    {
                        if (segment[19].Trim() != string.Empty)
                        {
                            //reportDetails.SsnId = segment[19].Trim();
                            reportDetails.PID_DASHES_19 = segment[19].Trim();
                        }
                    }
                    //if (segment.Length > 20)
                    //{
                    //    if (segment[20].Trim() != string.Empty)
                    //    {
                    //        reportDetails.PID_DASHES_4 = segment[20].Trim();
                    //        reportDetails.PID = segment[20].Trim();
                    //    }
                    //}

                    #endregion PID
                }
                else if (segment[0].Trim() == "PR1")
                {
                    #region PR1

                    if (segment[3].Trim() != string.Empty)
                    {
                        string[] testPanelInfo = segment[3].Trim().Split('^');

                        if (testPanelInfo.Length > 0)
                        {
                            if (testPanelInfo.Length == 1)
                            {
                                if (string.IsNullOrEmpty(reportDetails.TestPanelCode))
                                {
                                    reportDetails.TestPanelCode = testPanelInfo[0].Trim();
                                }
                                else
                                {
                                    reportDetails.TestPanelCode += ", " + testPanelInfo[0].Trim();
                                }
                            }
                            else if (testPanelInfo.Length == 2)
                            {
                                if (string.IsNullOrEmpty(reportDetails.TestPanelCode))
                                {
                                    reportDetails.TestPanelCode = testPanelInfo[0].Trim();
                                    reportDetails.TestPanelName = testPanelInfo[1].Trim();
                                }
                                else
                                {
                                    reportDetails.TestPanelCode += ", " + testPanelInfo[0].Trim();
                                    reportDetails.TestPanelName += ", " + testPanelInfo[1].Trim();
                                }
                            }
                        }
                    }

                    #endregion PR1
                }
                else if (segment[0].Trim() == "ORC")
                {
                    if (segment.Length >= 17)
                    {

                        if (segment[17].Trim() != string.Empty)
                        {
                            // This has account information - 

                            reportDetails.AccountInformation = segment[18].Trim();

                            // Try and pull the Acct number from this
                            string[] acctInfo = reportDetails.AccountInformation.Split('^');
                            if (acctInfo.Length > 0)
                            {
                                // we got something - 
                                reportDetails.AccountInformationAcct = acctInfo[0].Trim();
                                if (!reportDetails.FoundDeptByLabCode)
                                {
                                    if (ValidLab_Code(reportDetails.AccountInformationAcct))
                                    {
                                        reportDetails.FoundDeptByLabCode = true;
                                        reportDetails.lab_code = reportDetails.AccountInformationAcct;
                                    }

                                }
                            }

                        }
                    }
                }
            }

            if (reportType == ReportType.QuestLabReport)
            {
                if (segment[0].Trim() == "MSH")
                {
                    #region MSH

                    if (segment[2].Trim() != string.Empty)
                    {
                        reportDetails.LabName = segment[2].Trim();
                    }

                    if (segment[3].Trim() != string.Empty)
                    {
                        reportDetails.LabCode = segment[3].Trim();
                    }

                    if (segment[6].Trim() != string.Empty)
                    {
                        reportDetails.LabReportDate = segment[6].Trim();
                    }

                    #endregion MSH
                }
                else if (segment[0].Trim() == "PID")
                {
                    #region PID

                    if (segment[2].Trim() != string.Empty)
                    {
                        reportDetails.SpecimenId = segment[2].Trim();
                    }

                    if (segment[3].Trim() != string.Empty)
                    {
                        reportDetails.LabSampleId = segment[3].Trim();
                    }
                    if (segment[4].Trim() != string.Empty)
                    {
                        //reportDetails.PID = segment[4].Trim();
                        reportDetails.PID_NODASHES_4 = segment[4].Trim();
                    }
                    if (segment[19].Trim() != string.Empty) // THIS IS SSN
                    {

                        // of note - this can or cannot be SSN. Per David, Tim sometimes puts incomplete info in this
                        // This is going to create a real problem if he only enteres last 6 of social, as it is 
                        // possible that the last 6 of two donor's SSN could be the same.
                        // reportDetails.SsnId = segment[19].Trim();
                        //reportDetails.PID = segment[19].Trim();
                        reportDetails.PID_DASHES_19 = segment[19].Trim();
                    }

                    if (segment[5].Trim() != string.Empty)
                    {
                        string[] donorName = segment[5].Trim().Split('^');

                        if (donorName.Length > 0)
                        {
                            if (donorName.Length == 1)
                            {
                                reportDetails.DonorLastName = donorName[0].Trim();
                            }
                            else if (donorName.Length == 2)
                            {
                                reportDetails.DonorLastName = donorName[0].Trim();
                                reportDetails.DonorFirstName = donorName[1].Trim();
                            }
                            else if (donorName.Length == 3)
                            {
                                reportDetails.DonorLastName = donorName[0].Trim();
                                reportDetails.DonorFirstName = donorName[1].Trim();
                                reportDetails.DonorMI = donorName[2].Trim();
                            }
                        }
                    }

                    if (segment[7].Trim() != string.Empty)
                    {
                        reportDetails.DonorDOB = segment[7].Trim();
                    }

                    if (segment[8].Trim() != string.Empty)
                    {
                        reportDetails.DonorGender = segment[8].Trim();
                    }

                    //if (segment.Length > 19)
                    //{
                    //    if (segment[19].Trim() != string.Empty)
                    //    {
                    //        reportDetails.PID_DASHES_19 = segment[19].Trim();
                    //        reportDetails.PID = segment[19].Trim();
                    //    }
                    //}
                    #endregion PID
                }
                else if (segment[0].Trim() == "PR1")
                {
                    #region PR1

                    if (segment[3].Trim() != string.Empty)
                    {
                        string[] testPanelInfo = segment[3].Trim().Split('^');

                        if (testPanelInfo.Length > 0)
                        {
                            if (testPanelInfo.Length == 1)
                            {
                                if (string.IsNullOrEmpty(reportDetails.TestPanelCode))
                                {
                                    reportDetails.TestPanelCode = testPanelInfo[0].Trim();
                                }
                                else
                                {
                                    reportDetails.TestPanelCode += ", " + testPanelInfo[0].Trim();
                                }
                            }
                            else if (testPanelInfo.Length == 2)
                            {
                                if (string.IsNullOrEmpty(reportDetails.TestPanelCode))
                                {
                                    reportDetails.TestPanelCode = testPanelInfo[0].Trim();
                                    reportDetails.TestPanelName = testPanelInfo[1].Trim();
                                }
                                else
                                {
                                    reportDetails.TestPanelCode += ", " + testPanelInfo[0].Trim();
                                    reportDetails.TestPanelName += ", " + testPanelInfo[1].Trim();
                                }
                            }
                        }
                    }

                    #endregion PR1
                }
            }
        }

        private static bool CheckCrlClientCode(string code)
        {
            HL7ParserDao hl7ParserDao = new HL7ParserDao(_logger);

            ClientDepartment department = hl7ParserDao.GetClientDepartmentByLab_code(code);
            return (department != null);

        }

        private static OBR_Info GetOBRDetails(string[] segment, ReportType reportType)
        {
            OBR_Info obrInfo = new OBR_Info();
            if (reportType == ReportType.LabReport || reportType == ReportType.MROReport)
            {
                try
                {
                    obrInfo.TransmitedOrder = Convert.ToInt32(segment[1]);
                    obrInfo.CollectionSiteInfo = segment[2];

                    if (reportType == ReportType.MROReport)
                    {
                        obrInfo.SpecimenCollectionDate = segment[6];
                    }
                    else
                    {
                        obrInfo.SpecimenCollectionDate = segment[7];
                    }

                    obrInfo.CollectionSiteId = segment[10];
                    obrInfo.SpecimenActionCode = segment[11];

                    obrInfo.SpecimenReceivedDate = segment[14];

                    obrInfo.CrlClientCode = segment[16];


                    if (!(string.IsNullOrEmpty(segment[12])))
                    {
                        obrInfo.LabID = segment[12];
                    }

                    // Babblefish puts this in 12, the correct column.
                    //if (!(string.IsNullOrEmpty(segment[12])) && string.IsNullOrEmpty(obrInfo.CrlClientCode))
                    //{
                    //    // there's someting in 12
                    //    obrInfo.CrlClientCode = segment[12];
                    //}

                    if (ValidLab_Code(obrInfo.CrlClientCode) && segment[16].Trim() != string.Empty)
                    {
                        obrInfo.lab_code = obrInfo.CrlClientCode;
                        string[] crlCode = segment[16].Trim().Split('.');

                        if (crlCode.Length > 0)
                        {
                            if (crlCode.Length == 1)
                            {
                                obrInfo.TpaCode = crlCode[0].Trim();
                            }
                            else if (crlCode.Length == 2)
                            {
                                obrInfo.TpaCode = crlCode[0].Trim();
                                obrInfo.RegionCode = crlCode[1].Trim();
                            }
                            else if (crlCode.Length == 3)
                            {
                                obrInfo.TpaCode = crlCode[0].Trim();

                                if (crlCode[1].Trim().ToUpper() == "MPOS" || crlCode[1].Trim().ToUpper() == "MALL")
                                {
                                    obrInfo.RegionCode = crlCode[1].Trim();
                                }
                                else
                                {
                                    obrInfo.ClientCode = crlCode[1].Trim();
                                    obrInfo.DepartmentCode = crlCode[2].Trim();
                                }
                            }
                            else if (crlCode.Length == 4)
                            {
                                obrInfo.TpaCode = crlCode[0].Trim();
                                obrInfo.RegionCode = crlCode[1].Trim();
                                obrInfo.ClientCode = crlCode[2].Trim();
                                obrInfo.DepartmentCode = crlCode[3].Trim();
                            }
                        }
                    }
                    if (ValidLab_Code(obrInfo.LabID) && segment[12].Trim() != string.Empty)
                    {
                        // We'll try seg 12, LabId
                        // if that works - use it
                        obrInfo.lab_code = obrInfo.LabID;
                        string[] crlCode = segment[12].Trim().Split('.');

                        if (crlCode.Length > 0)
                        {
                            if (crlCode.Length == 1)
                            {
                                obrInfo.TpaCode = crlCode[0].Trim();
                            }
                            else if (crlCode.Length == 2)
                            {
                                obrInfo.TpaCode = crlCode[0].Trim();
                                obrInfo.RegionCode = crlCode[1].Trim();
                            }
                            else if (crlCode.Length == 3)
                            {
                                obrInfo.TpaCode = crlCode[0].Trim();

                                if (crlCode[1].Trim().ToUpper() == "MPOS" || crlCode[1].Trim().ToUpper() == "MALL")
                                {
                                    obrInfo.RegionCode = crlCode[1].Trim();
                                }
                                else
                                {
                                    obrInfo.ClientCode = crlCode[1].Trim();
                                    obrInfo.DepartmentCode = crlCode[2].Trim();
                                }
                            }
                            else if (crlCode.Length == 4)
                            {
                                obrInfo.TpaCode = crlCode[0].Trim();
                                obrInfo.RegionCode = crlCode[1].Trim();
                                obrInfo.ClientCode = crlCode[2].Trim();
                                obrInfo.DepartmentCode = crlCode[3].Trim();
                            }
                        }
                    }

                    obrInfo.SpecimenType = segment[20];

                    obrInfo.SectionHeader = segment[21];
                    obrInfo.CrlTransmitDate = segment[22];
                    obrInfo.ServiceSectionId = segment[24];
                    obrInfo.OrderStatus = segment[25];
                    obrInfo.ReasonType = segment[31];
                }
                catch (Exception)
                {
                    _logger.Information("File is not in required format - OBR");
                    throw;
                }
            }
            if (reportType == ReportType.QuestLabReport)
            {
                HL7ParserDao hl7ParserDao = new HL7ParserDao(_logger);

                ///////////////////////////////////////////////////--------------
                //process the code that is looked up
                try
                {
                    obrInfo.TransmitedOrder = Convert.ToInt32(segment[1]);
                    obrInfo.CollectionSiteInfo = segment[2];

                    if (reportType == ReportType.MROReport)
                    {
                        obrInfo.SpecimenCollectionDate = segment[6];
                    }
                    else
                    {
                        obrInfo.SpecimenCollectionDate = segment[7];
                    }

                    obrInfo.CollectionSiteId = segment[10];
                    obrInfo.SpecimenActionCode = segment[11];

                    obrInfo.SpecimenReceivedDate = segment[14];

                    var QuestID = segment[16];
                    _logger.Information("QuestCode:" + segment[16]);
                    //Lookup the 0vn code to get the quest number
                    ClientDepartment department = hl7ParserDao.GetClientDepartmentQuestInfo(QuestID);
                    //_logger.Information(department);
                    try
                    {
                        obrInfo.CrlClientCode = department.FullLabCode.Trim();//was segment 16
                    }
                    catch (Exception ex)
                    {
                        _logger.Information("QuestMismatched:" + QuestID);
                        var blah = QuestID;
                    }
                    //Skip this part if there is no department code found this would be a mismatched file and we need to handle appropriatly
                    if (obrInfo.CrlClientCode != null)
                    {
                        if (segment[16].Trim() != string.Empty)
                        {
                            string[] crlCode = department.FullLabCode.Trim().Split('.');

                            if (crlCode.Length > 0)
                            {
                                if (crlCode.Length == 1)
                                {
                                    obrInfo.TpaCode = crlCode[0].Trim();
                                }
                                else if (crlCode.Length == 2)
                                {
                                    obrInfo.TpaCode = crlCode[0].Trim();
                                    obrInfo.RegionCode = crlCode[1].Trim();
                                }
                                else if (crlCode.Length == 3)
                                {
                                    obrInfo.TpaCode = crlCode[0].Trim();

                                    if (crlCode[1].Trim().ToUpper() == "MPOS" || crlCode[1].Trim().ToUpper() == "MALL")
                                    {
                                        obrInfo.RegionCode = crlCode[1].Trim();
                                    }
                                    else
                                    {
                                        obrInfo.ClientCode = crlCode[1].Trim();
                                        obrInfo.DepartmentCode = crlCode[2].Trim();
                                    }
                                }
                                else if (crlCode.Length == 4)
                                {
                                    obrInfo.TpaCode = crlCode[0].Trim();
                                    obrInfo.RegionCode = crlCode[1].Trim();
                                    obrInfo.ClientCode = crlCode[2].Trim();
                                    obrInfo.DepartmentCode = crlCode[3].Trim();
                                }
                            }
                        }
                    }

                    obrInfo.SpecimenType = segment[20];

                    obrInfo.SectionHeader = "INITIAL TEST";
                    obrInfo.CrlTransmitDate = segment[22];
                    obrInfo.ServiceSectionId = segment[24];
                    obrInfo.OrderStatus = segment[25];
                    obrInfo.ReasonType = segment[29]; // This was the only change.


                    obrInfo.OBRQuestCode = segment[16].ToString();

                    var prequestid = (segment[16].ToString() + segment[18].ToString()).Replace("^", "").Replace("A", "").Replace("B", "").Replace("C", "").Replace("D", "").Replace("E", "").Replace("F", "").Replace("G", "").Replace("H", "").Replace("I", "").Replace("J", "").Replace("K", "").Replace("L", "").Replace("M", "").Replace("N", "").Replace("O", "").Replace("P", "").Replace("Q", "").Replace("R", "").Replace("S", "").Replace("T", "").Replace("U", "").Replace("V", "").Replace("W", "").Replace("X", "").Replace("Y", "").Replace("Z", "");
                    obrInfo.QuestSpeciminID = prequestid;
                    _logger.Debug($"QuestSpeciminID set to {prequestid} - Seg 16 + seg 18 with replacements of alpha and ^");
                    //obrInfo.QuestSpeciminID =
                }
                catch (Exception)
                {
                    _logger.Information("File is not in required format - OBR");
                    //throw;
                }
            }
            return obrInfo;
        }

        private static bool CheckDeptLab_codeForMatch(string lab_code)
        {
            return false;
        }

        private static void GetOBXDetails(OBR_Info obrInfo, string[] segment, ReportType reportType)
        {
            int sequence = Convert.ToInt32(segment[1]);

            OBX_Info obxInfo = null;
            if (reportType == ReportType.LabReport || reportType == ReportType.MROReport)
            {
                try
                {
                    if (obrInfo.observatinos.Count == 0)
                    {
                        obxInfo = new OBX_Info();
                        obxInfo.Sequence = sequence;
                        obrInfo.observatinos.Add(obxInfo);
                    }
                    else
                    {
                        bool flag = true;

                        foreach (OBX_Info item in obrInfo.observatinos)
                        {
                            if (sequence == item.Sequence)
                            {
                                obxInfo = item;
                                flag = false;
                                break;
                            }
                        }

                        if (flag)
                        {
                            obxInfo = new OBX_Info();
                            obxInfo.Sequence = sequence;
                            obrInfo.observatinos.Add(obxInfo);
                        }
                    }

                    if (obxInfo != null)
                    {
                        string[] testDetails = segment[3].Split('^');
                        if (testDetails.Length == 2)
                        {
                            obxInfo.TestCode = testDetails[0].Trim();
                            obxInfo.TestName = testDetails[1].Trim();
                        }

                        if (reportType == ReportType.QuestLabReport)
                        {
                            if (testDetails.Length == 5)
                            {
                                obxInfo.TestCode = testDetails[3].Trim();
                                obxInfo.TestName = testDetails[4].Trim();
                            }
                        }

                        if (testDetails.Length == 1)
                        {
                            obxInfo.TestName = testDetails[0].Trim();
                        }

                        string[] resultDetails = segment[5].Split('^');
                        if (resultDetails.Length == 2)
                        {
                            if (resultDetails[0].Trim() != string.Empty
                                && (resultDetails[0].Trim().ToUpper() == "POS" || resultDetails[0].Trim().ToUpper() == "POSITIVE"
                                || resultDetails[0].Trim().ToUpper() == "NEG" || resultDetails[0].Trim().ToUpper() == "NEGATIVE"))
                            {
                                obxInfo.Status = resultDetails[0].Trim();
                            }
                            obxInfo.Result = resultDetails[1].Trim();
                        }
                        else if (resultDetails.Length == 1)
                        {
                            if (resultDetails[0].Trim().ToUpper() == "POS" || resultDetails[0].Trim().ToUpper() == "POSITIVE"
                                || resultDetails[0].Trim().ToUpper() == "NEG" || resultDetails[0].Trim().ToUpper() == "NEGATIVE")
                            {
                                obxInfo.Status = resultDetails[0].Trim();
                            }
                            else
                            {
                                if (obxInfo.Status != "POS")
                                {
                                    obxInfo.Status = "None";
                                }
                            }
                        }

                        obxInfo.UnitOfMeasure = segment[6].Trim();

                        obxInfo.ReferenceRange = segment[7].Trim();

                        if (reportType == ReportType.QuestLabReport)
                        {
                            string[] cutoff = segment[7].Split('^');
                            obxInfo.ReferenceRange = cutoff[0].Trim();
                        }
                        obxInfo.OrderStatus = segment[11].Trim();
                    }
                }
                catch (Exception)
                {
                    _logger.Information("File is not in the corect format OBX");
                    throw;
                }
            }
            if (reportType == ReportType.QuestLabReport)
            {
                try
                {
                    if (obrInfo.observatinos.Count == 0)
                    {
                        obxInfo = new OBX_Info();
                        obxInfo.Sequence = sequence;
                        obrInfo.observatinos.Add(obxInfo);
                    }
                    else
                    {
                        bool flag = true;

                        foreach (OBX_Info item in obrInfo.observatinos)
                        {
                            if (sequence == item.Sequence)
                            {
                                obxInfo = item;
                                flag = false;
                                break;
                            }
                        }

                        if (flag)
                        {
                            obxInfo = new OBX_Info();
                            obxInfo.Sequence = sequence;
                            obrInfo.observatinos.Add(obxInfo);
                        }
                    }

                    if (obxInfo != null)
                    {
                        string[] testDetails = segment[3].Split('^');
                        if (testDetails.Length == 2)
                        {
                            obxInfo.TestCode = testDetails[0].Trim();
                            obxInfo.TestName = testDetails[1].Trim();
                        }

                        if (testDetails.Length == 1)
                        {
                            obxInfo.TestName = testDetails[0].Trim();
                        }
                        if (testDetails.Length == 5)
                        {
                            obxInfo.TestCode = testDetails[3].Trim();
                            obxInfo.TestName = testDetails[4].Trim();
                        }

                        string[] resultDetails = segment[5].Split('^');
                        if (resultDetails.Length == 2)
                        {
                            if (resultDetails[0].Trim() != string.Empty
                                && (resultDetails[0].Trim().ToUpper() == "POS" || resultDetails[0].Trim().ToUpper() == "POSITIVE"
                                || resultDetails[0].Trim().ToUpper() == "NEG" || resultDetails[0].Trim().ToUpper() == "NEGATIVE"))
                            {
                                obxInfo.Status = resultDetails[0].Trim();
                            }
                            obxInfo.Result = resultDetails[1].Trim();
                        }
                        else if (resultDetails.Length == 1)
                        {
                            if (resultDetails[0].Trim().ToUpper() == "POS" || resultDetails[0].Trim().ToUpper() == "POSITIVE"
                                || resultDetails[0].Trim().ToUpper() == "NEG" || resultDetails[0].Trim().ToUpper() == "NEGATIVE")
                            {
                                obxInfo.Status = resultDetails[0].Trim();
                            }
                        }

                        obxInfo.UnitOfMeasure = segment[6].Trim();
                        obxInfo.ReferenceRange = segment[7].Trim();
                        if (reportType == ReportType.QuestLabReport)
                        {
                            string[] cutoff = segment[7].Split('^');
                            obxInfo.ReferenceRange = cutoff[0].Trim();
                        }
                        obxInfo.OrderStatus = segment[11].Trim();
                    }
                }
                catch (Exception)
                {
                    _logger.Information("File is not in the corect format OBX");
                    throw;
                }
            }
        }

        private static string ConvertAsciiToUnicode(string asciiString)
        {
            // Create two different encodings.
            Encoding aAsciiEncoding = Encoding.ASCII;
            Encoding aUnicodeEncoding = Encoding.UTF8;
            // Convert the string into a byte[].
            byte[] aAsciiBytes = aAsciiEncoding.GetBytes(asciiString);
            // Perform the conversion from one encoding to the other.
            byte[] aUnicodeBytes = Encoding.Convert(aAsciiEncoding, aUnicodeEncoding,
            aAsciiBytes);
            // Convert the new byte[] into a char[] and then into a string.
            char[] aUnicodeChars = new
            char[aUnicodeEncoding.GetCharCount(aUnicodeBytes, 0, aUnicodeBytes.Length)];
            aUnicodeEncoding.GetChars(aUnicodeBytes, 0, aUnicodeBytes.Length, aUnicodeChars, 0);
            string aUnicodeString = new string(aUnicodeChars);
            return aUnicodeString;
        }

        private static void UploadPDFReport(string reportFile)
        {
            string fileName = reportFile.Substring(reportFile.LastIndexOf('\\') + 1);
            string fileTitle = fileName.Substring(0, fileName.Length - 4);
            string specimenId = (fileTitle.Split('-'))[0].ToString();

            HL7ParserDao hl7ParserDao = new HL7ParserDao(_logger);

            Donor donor = hl7ParserDao.GetDonorDetails(specimenId);

            if (donor != null)
            {
                DonorDocument donorDocument = new DonorDocument();

                donorDocument.DonorDocumentId = 0;
                donorDocument.DonorId = donor.DonorId;
                donorDocument.DocumentTitle = fileTitle;

                FileStream stream = File.OpenRead(reportFile);
                byte[] fileBytes = new byte[stream.Length];

                stream.Read(fileBytes, 0, fileBytes.Length);
                stream.Close();

                donorDocument.DocumentContent = fileBytes;
                donorDocument.Source = "";
                donorDocument.UploadedBy = "SYSTEM";
                donorDocument.FileName = fileName;

                hl7ParserDao.UploadPDFReport(specimenId, donorDocument);
            }
        }

        public static void WriteClientLogFile(ReportInfo reportDetails, ReturnValues returnValues, string FileName, int Numpassed, int TotalTo)
        {
            returnValues.ErrorFlag = true;
            returnValues.ErrorMessage = "Lab Code does not match.";

            StreamWriter log;
            FileStream fileStream = null;
            DirectoryInfo logDirInfo = null;
            FileInfo logFileInfo;

            string logFilePath = "D:\\Logs\\";
            logFilePath = logFilePath + "MismatchErrorLog" + "." + "txt";
            logFileInfo = new FileInfo(logFilePath);
            logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
            if (!logDirInfo.Exists) logDirInfo.Create();
            if (!logFileInfo.Exists)
            {
                fileStream = logFileInfo.Create();
            }
            else
            {
                fileStream = new FileStream(logFilePath, FileMode.Append);
            }
            log = new StreamWriter(fileStream);

            var timedate = "Date:" + DateTime.Now.ToString() + "(count:" + Numpassed + "/" + TotalTo + ")";
            var blah = "SpecID:" + reportDetails.SpecimenId.ToString();
            var blah2 = "Error:" + returnValues.ErrorMessage.ToString();
            var blah3 = string.Empty;
            try
            {
                blah3 = "LabCodePassedOnReport:" + reportDetails.CrlClientCode.ToString();
            }
            catch (Exception)
            {
                blah3 = "LabCodePassedOnReport:" + "NULL";
            }

            var blah4 = "LabName:" + reportDetails.LabName.ToString();
            var blah5 = "FileName:" + FileName;

            log.WriteLine(timedate + " " + blah + " " + blah2 + " " + blah3 + " " + blah4 + " " + blah5);
            log.Close();
            _logger.Information("[WriteClientLogFile] Please See Logfile: " + logFilePath);
        }

        public static void WriteClientLogFileMismatchedTrue(string repSpecId, string repClCode, string repLabName, string FileName, string LogType)
        {
            StreamWriter log;
            FileStream fileStream = null;
            DirectoryInfo logDirInfo = null;
            FileInfo logFileInfo;
            string logFilePath;
            if (ConfigurationManager.AppSettings.AllKeys.Contains("LogFilePath"))
            {
                logFilePath = ConfigurationManager.AppSettings["LogFilePath"].ToString().Trim();
            }
            else
            {
                logFilePath = "D:\\Logs\\";
            }
            logFilePath = logFilePath + "MismatchErrorLog" + "." + "txt";
            logFileInfo = new FileInfo(logFilePath);
            logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
            if (!logDirInfo.Exists) logDirInfo.Create();
            if (!logFileInfo.Exists)
            {
                fileStream = logFileInfo.Create();
            }
            else
            {
                fileStream = new FileStream(logFilePath, FileMode.Append);
            }
            log = new StreamWriter(fileStream);

            var timedate = "Date:" + DateTime.Now.ToString();
            var blah = "SpecID:" + repSpecId;//reportDetails.SpecimenId.ToString();

            var blah3 = string.Empty;
            try
            {
                blah3 = "LabCodePassedOnReport:" + repClCode;//reportDetails.CrlClientCode.ToString();
            }
            catch (Exception)
            {
                blah3 = "LabCodePassedOnReport:" + "NULL";
            }

            var blah4 = "LabName:" + repLabName;//reportDetails.LabName.ToString();
            var blah5 = "FileName:" + FileName;
            //var blah5 = "LabCode:" + reportDetails.CrlClientCode.ToString();

            if (LogType == "CRL")
            {
                string cc = string.Empty;
                string dc = string.Empty;
                string clc = string.Empty;

                try
                {
                    cc = "Code:" + repClCode;//reportDetails.CrlClientCode.ToString();
                    //dc = "DptCode:" + reportDetails.DepartmentCode.ToString();
                    //clc ="ClCode:" + reportDetails.ClientCode.ToString();
                }
                catch (Exception)
                {
                    throw;
                }
                log.WriteLine(timedate + " " + "Logtype:" + LogType + " " + blah + " " + " " + blah4 + " " + blah5 + " " + cc);
            }
            else
            {
                log.WriteLine(timedate + " " + "Logtype:" + LogType + " " + blah + " " + " " + blah4 + " " + blah5);
            }
            log.Close();
            _logger.Information("[WriteClientLogFileMismatchedTrue] Please See Logfile: " + logFilePath);
        }

        #endregion Private Methods

        #region Re-parser

        //public static void DoHL7ReParser(ReportType reportType, string sourcePath)
        //{
        //    //To-do Write the log

        //    if (Directory.EnumerateFiles(sourcePath).Count() > 0)
        //    {
        //        int NumPassed = 0;
        //        int TotalTo = Directory.EnumerateFiles(sourcePath).Count();
        //        foreach (string reportFile in Directory.EnumerateFiles(sourcePath))
        //        {
        //            NumPassed++;

        //            string fileName = reportFile.Substring(reportFile.LastIndexOf('\\') + 1);
        //            //To-do Write the log

        //            if (fileName.ToUpper().EndsWith(".RPT") || fileName.ToUpper().EndsWith(".HL7"))
        //            {
        //                ReportInfo reportDetails = null;
        //                List<OBR_Info> obrList = new List<OBR_Info>();

        //                RTFBuilderbase crlReport = new RTFBuilder();

        //                string[] lines = File.ReadAllLines(reportFile);

        //                int currentOBROrder = 0;
        //                OBR_Info currentOBRInfo = null;
        //                int misMatchedCount = 0;
        //                int testInfoId = 0;
        //                foreach (string line in lines)
        //                {
        //                    string[] segment = line.Split('|');
        //                    if (segment.Length > 0)
        //                    {
        //                        if (segment[0].Trim() == "MSH"
        //                            || segment[0].Trim() == "PID"
        //                            || segment[0].Trim() == "PR1")
        //                        {
        //                            if (reportDetails == null)
        //                            {
        //                                reportDetails = new ReportInfo();
        //                            }

        //                            GetReportDetails(segment, reportDetails, reportType);
        //                        }
        //                        else if (segment[0].Trim() == "DSP")
        //                        {
        //                            if (segment.Length == 4)
        //                            {
        //                                crlReport.AppendLine(ConvertAsciiToUnicode(segment[3]));
        //                            }
        //                            else
        //                            {
        //                                crlReport.AppendLine("");
        //                            }
        //                        }
        //                        else if (segment[0].Trim() == "OBR")
        //                        {
        //                            OBR_Info obrInfo = GetOBRDetails(segment, reportType);
        //                            obrList.Add(obrInfo);
        //                            currentOBRInfo = obrInfo;
        //                            currentOBROrder = currentOBRInfo.TransmitedOrder;
        //                        }
        //                        else if (segment[0].Trim() == "OBX")
        //                        {
        //                            if (currentOBRInfo != null && currentOBRInfo.TransmitedOrder == currentOBROrder)
        //                            {
        //                                GetOBXDetails(currentOBRInfo, segment, reportType);
        //                            }
        //                        }
        //                        else if (segment[0].Trim() == "NTE")
        //                        {
        //                            if (reportType == ReportType.MROReport)
        //                            {
        //                                string retVal = string.Empty;
        //                                updateReParsingToDatabase(reportDetails, reportType, crlReport, obrList, sourcePath, fileName, reportFile, ref misMatchedCount, ref testInfoId, NumPassed, TotalTo);
        //                                reportDetails = null;
        //                                obrList = new List<OBR_Info>();
        //                                crlReport = new RTFBuilder();
        //                                currentOBROrder = 0;
        //                                currentOBRInfo = null;

        //                            }
        //                        }
        //                    }
        //                }
        //                //if (reportDetails != null)
        //                //{
        //                if (reportType == ReportType.MROReport)
        //                {
        //                    if (testInfoId == 0)
        //                    {
        //                        if (misMatchedCount > Convert.ToInt32(ConfigurationManager.AppSettings["MaxReparsingCount"].ToString()))
        //                        {
        //                            File.Copy(reportFile, sourcePath + "\\Unmatched\\" + fileName, true);
        //                            File.Delete(reportFile);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        File.Copy(reportFile, sourcePath + "\\Processed\\" + fileName, true);
        //                        File.Delete(reportFile);
        //                    }
        //                }
        //                else if (reportType == ReportType.LabReport)
        //                {
        //                    updateReParsingToDatabase(reportDetails, reportType, crlReport, obrList, sourcePath, fileName, reportFile, ref misMatchedCount, ref testInfoId, NumPassed, TotalTo);
        //                }
        //                //}
        //            }
        //        }

        //        //foreach (string reportFile in Directory.EnumerateFiles(sourcePath))
        //        //{
        //        //    string fileName = reportFile.Substring(reportFile.LastIndexOf('\\') + 1);
        //        //    //To-do Write the log

        //        //    if (fileName.ToUpper().EndsWith(".PDF"))
        //        //    {
        //        //        UploadPDFReport(reportFile);

        //        //        File.Copy(reportFile, sourcePath + "\\Processed\\" + fileName, true);
        //        //        File.Delete(reportFile);
        //        //    }
        //        //}

        //    }
        //}

        //private static void updateReParsingToDatabase(ReportInfo reportDetails, ReportType reportType, RTFBuilderbase crlReport, List<OBR_Info> obrList,
        //    string sourcePath, string fileName, string reportFile, ref int misMatchedCount, ref int testInfoId, int NumPassed, int TotalTo)
        //{
        //    HL7ParserDao hl7ParserDao = new HL7ParserDao();

        //    try
        //    {
        //        if (reportDetails != null)
        //        {
        //            //To-do Write the log
        //            bool archiveExistingReport = hl7ParserDao.CheckDuplicateSpecimenId(reportType, reportDetails.SpecimenId);

        //            if (archiveExistingReport)
        //            {
        //                //To-do Write the log
        //            }

        //            ReturnValues returnValues = new ReturnValues();

        //            if (hl7ParserDao.UpdateReParsingReport(reportType, reportDetails, obrList, crlReport, archiveExistingReport, returnValues, fileName, NumPassed, TotalTo))
        //            {
        //                testInfoId = returnValues.DonorTestInfoId;
        //                if (returnValues.DonorTestInfoId == 0)
        //                {
        //                    if (returnValues.MismatchRecordId != 0)
        //                    {
        //                        misMatchedCount = returnValues.MismatchedCount;
        //                    }
        //                    if (reportType != ReportType.MROReport)
        //                    {
        //                        if (returnValues.MismatchedCount >= Convert.ToInt32(ConfigurationManager.AppSettings["MaxReparsingCount"].ToString()))
        //                        {
        //                            File.Copy(reportFile, sourcePath + "\\Unmatched\\" + fileName, true);
        //                            File.Delete(reportFile);
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    if (reportType != ReportType.MROReport)
        //                    {
        //                        if (returnValues.MroAttentionFlag)
        //                        {
        //                            File.Copy(reportFile, ConfigurationManager.AppSettings["MROReportFileOutboundPath"].ToString().Trim() + "\\" + fileName, true);
        //                            File.Copy(reportFile, ConfigurationManager.AppSettings["MROReportFileInboundPath"].ToString().Trim() + "\\" + "\\Processed\\" + fileName, true);
        //                        }
        //                        else
        //                        {
        //                            File.Copy(reportFile, sourcePath + "\\Processed\\" + fileName, true);
        //                        }
        //                        File.Delete(reportFile);
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                //Need to send the mail to with the .RPT
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //To-do Write the log
        //        throw ex;
        //    }
        //}

        #endregion Re-parser
    }
}