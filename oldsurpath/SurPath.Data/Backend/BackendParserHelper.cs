using MySql.Data.MySqlClient;
using Serilog;
using SurPath.Enum;
using SurPath.MySQLHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;

namespace SurPath.Data
{
    public class BackendParserHelper
    {
        private static ILogger _logger;
        private string _FileName { get; set; }
        private string _FilenameNoPath { get; set; }
        private bool _alreadyParsed = false;

        public bool Add_To_Database { get; set; } = false;
        public bool is_data_of_record { get; set; }
        public int The_current_data_file_of_record { get; set; }

        public BackendParserReportHelper Report { get; set; }
        public BackendParserReportHelperItem ReportHelperItem { get; set; } = new BackendParserReportHelperItem();
        public string filename
        { get { return _FileName; } }
        private string _FileChecksum { get; set; }
        public string file_checksum
        { get { return _FileChecksum; } }
        public string data_checksum { get; set; }
        public string specimen_id { get; set; }
        public int report_type { get; set; }
        public int report_info_id { get; set; }
        public bool IsMismatch
        { get { return mismatched_report_id < 1; } }
        public int mismatched_report_id { get; set; }
        public int mismatched_count { get; set; }
        public bool NewMismatch { get; set; } = false;
        public int parser_file_activity_id { get; set; }
        public DateTime downloaded_on { get; set; }
        public DateTime parsed_on { get; set; }
        public DateTime inserted_on { get; set; }

        public int parse_attempt_count { get; set; }

        private string ConnectionString;
        private CultureInfo Culture;
        private SHA1CryptoServiceProvider sha1;

        public BackendParserHelper(string _filename, ILogger __logger = null)
        {
            init(__logger);

            byte[] _file = new byte[1];
            _FileChecksum = string.Empty;
            if (File.Exists(_filename))
            {
                _FileName = _filename;
                _file = File.ReadAllBytes(_FileName);
                _FileChecksum = BitConverter.ToString(sha1.ComputeHash(_file)).Replace("-", string.Empty); // reportDetails.data_checksum = reportCheckSumData;
            }
        }

        //TODO this would be much better if there were a single function with all parmeters
        private bool Check(string __filename, string __data_checksum, string __specimen_id)
        {
            throw new NotImplementedException();

            if (string.IsNullOrEmpty(__specimen_id)) return true;
            if (string.IsNullOrEmpty(__data_checksum)) return true;
            if (string.IsNullOrEmpty(__filename)) return true;

            specimen_id = __specimen_id;
            data_checksum = __data_checksum;
            //filename = __filename;

            byte[] _file = new byte[1];
            _FileChecksum = string.Empty;
            if (File.Exists(__filename))
            {
                _FileName = __filename;
                _file = File.ReadAllBytes(_FileName);
                _FileChecksum = BitConverter.ToString(sha1.ComputeHash(_file)).Replace("-", string.Empty); // reportDetails.data_checksum = reportCheckSumData;
            }
            ParseThisFile();

            return false;
        }

        public void CheckHistory()
        {
            _logger.Debug("Parser Helper Checking History");
            if (_alreadyParsed) return;

            ParseThisFile();
            _alreadyParsed = true;
        }

        public void Set_As_Data_Of_record(int report_info_id)
        {
            //if (addFile) AddFileToLog();
            if (parser_file_activity_id < 1) AddFileToLog();

            SetFileOfRecord(report_info_id);
        }

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

        public void init(ILogger __logger)
        {
            if (__logger != null)
            {
                _logger = __logger;
                _logger.Debug("BackendParserHelper logger online");
                _logger.Debug("Path to this lib: " + AssemblyDirectory);
            }
            else
            {
                _logger = new LoggerConfiguration().CreateLogger();
            }
            sha1 = new SHA1CryptoServiceProvider();
            ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString().Trim();
            string cultureSetting = ConfigurationManager.AppSettings["Culture"]?.ToString()?.Trim();

            Culture = !string.IsNullOrEmpty(cultureSetting) ? new CultureInfo(cultureSetting) : new CultureInfo("en-US");
            // Culture = new CultureInfo(ConfigurationManager.AppSettings["Culture"].ToString().Trim());
            _logger.Debug("Spinning up BackendParserReportHelper for BackendParserHelper instance");
            Report = new BackendParserReportHelper(__logger);
        }

        public void save()
        {
            AddFileToLog();
        }

        #region private

        /// <summary>
        /// check the file checksum, the specamin, the data checksum against data or record. If
        /// </summary>
        /// <returns></returns>
        private bool ParseThisFile()
        {
            if (string.IsNullOrEmpty(specimen_id)) return true;
            if (string.IsNullOrEmpty(data_checksum)) return true;
            if (string.IsNullOrEmpty(file_checksum)) return true;
            if (string.IsNullOrEmpty(filename)) return true;

            _FilenameNoPath = Path.GetFileName(filename);

            GetFileRecord();
            if (parser_file_activity_id > 0 && is_data_of_record == true) return false;

            if (parser_file_activity_id < 1)
            {
                // this is a new file we've never seen before.
            }
            // default is to parse, therefore..
            return true;
        }

        public void AddFileToLog()
        {
            string sqlQuery = @"
                INSERT INTO surpathlive.parser_file_activity(
                   filename
                  ,specimen_id
                  ,report_type
                  ,file_checksum
                  ,data_checksum
                  ,downloaded_on
                  ,parsed_on
                  ,parse_attempt_count
                ) VALUES
                (
                  @filename  -- filename - IN varchar(255)
                  ,@specimen_id  -- specimen_id - IN varchar(50)
                  ,@report_type -- report_type - IN int(11)
                  ,@file_checksum  -- file_checksum - IN varchar(255)
                  ,@data_checksum  -- data_checksum - IN varchar(255)
                  ,CURRENT_TIMESTAMP -- downloaded_on - IN timestamp
                  ,CURRENT_TIMESTAMP -- parsed_on - IN timestamp
                  ,0 -- parse_attempt_count - IN int(11)
                );
                ";

            ParamHelper param = new ParamHelper();
            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();
                MySqlTransaction trans = conn.BeginTransaction();

                param.Param = new MySqlParameter("@filename", _FilenameNoPath);
                param.Param = new MySqlParameter("@file_checksum", _FileChecksum);
                param.Param = new MySqlParameter("@specimen_id", specimen_id);
                param.Param = new MySqlParameter("@data_checksum", data_checksum);
                param.Param = new MySqlParameter("@report_type", report_type);
                try
                {
                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param.Params);

                    sqlQuery = "SELECT LAST_INSERT_ID()";

                    parser_file_activity_id = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlQuery));

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    _logger.Error($"Attempt top add  {filename} to parser activity log for {specimen_id} failed!");
                    _logger.Error(ex.Message);
                    _logger.Error(ex.InnerException.ToString());
                    //throw ex;
                }
            }
        }

        private void SetFileOfRecord(int report_info_id)
        {
            string sqlQuery1 = @"
                UPDATE surpathlive.parser_file_activity
                SET
                    is_data_of_record = 0 -- bit(1)
                WHERE specimen_id = @specimen_id AND
                      report_type = @report_type;

";
            string sqlQuery2 = @"
                UPDATE surpathlive.parser_file_activity
                SET
                    parsed_on = CURRENT_TIMESTAMP-- timestamp
                    ,report_info_id = @report_info_id
                    ,is_data_of_record = 1-- bit(1)
                WHERE parser_file_activity_id = @parser_file_activity_id -- int(11);
";

            ParamHelper param = new ParamHelper();
            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();
                MySqlTransaction trans = conn.BeginTransaction();
                try
                {
                    param.Param = new MySqlParameter("@specimen_id", specimen_id);
                    param.Param = new MySqlParameter("@report_type", report_type);

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery1, param.Params);

                    param.reset();
                    param.Param = new MySqlParameter("@report_info_id", report_info_id);
                    param.Param = new MySqlParameter("@parser_file_activity_id", parser_file_activity_id);
                    //param.Param = new MySqlParameter("@is_data_of_record", 1);

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery2, param.Params);

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    _logger.Error($"Attempt top set {report_info_id} as data of record for sample id {specimen_id} - file: {filename} - FAILED!");
                    _logger.Error(ex.Message);
                    _logger.Error(ex.InnerException.ToString());

                    throw ex;
                }
            }
        }

        private void GetFileRecord()
        {
            //string sqlQuery = @"SELECT
            //                parser_file_activity_id,
            //                downloaded_on,
            //                parsed_on,
            //                is_data_of_record,
            //                report_info_id,
            //                inserted_on,
            //                parse_attempt_count
            //                FROM surpathlive.parser_file_activity
            //                where
            //                    filename = @filename AND
            //                    specimen_id = @specimen_id AND
            //                    file_checksum = @file_checksum AND
            //                    data_checksum = @data_checksum AND
            //                    report_type = @report_type
            //                order by parser_file_activity_id DESC;";

            //--no records, new file
            //-- CurrentDataFileOfRecord > 0, is the current data file of record
            //--CurrentDataFileOfRecord = 0, file has been parsed in past and superceeded

            string sqlQuery = @"
                    select p.*,
                    IFNULL(pb.latest_parser_file_activity_id,0) as CurrentDataFileOfRecord,
                    IFNULL(mm.mismatched_report_id,0) as mismatched_report_id,
                    IFNULL(mm.mismatched_count, 0) as mismatched_count,
                    r.is_archived as CurrentDataFileOfRecordArchived
                    from parser_file_activity p
                      left outer join
                      (
                      select p2.parser_file_activity_id as latest_parser_file_activity_id from parser_file_activity p2

                      where
                        p2.file_checksum =  @file_checksum AND
                        p2.data_checksum = @data_checksum AND
                        p2.specimen_id = @specimen_id AND
                        p2.is_data_of_record = 1
                        order by p2.parser_file_activity_id desc limit 1
                      ) pb on pb.latest_parser_file_activity_id = p.parser_file_activity_id
                      left outer join
                      (
                      select * from mismatched_reports where mismatched_reports.file_name = @filename order by mismatched_report_id desc limit 1
                      ) mm on mm.file_name = p.filename
                      left outer join report_info r on r.report_info_id = p.report_info_id
                      where
                        p.file_checksum = @file_checksum AND
                        p.data_checksum = @data_checksum AND
                        p.specimen_id = @specimen_id AND
                        p.filename = @filename
                        ;
            ";

            ParamHelper param = new ParamHelper();
            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                param.Param = new MySqlParameter("@filename", _FilenameNoPath); // We sometimes get files that are *exactly* the same.
                param.Param = new MySqlParameter("@file_checksum", _FileChecksum);
                param.Param = new MySqlParameter("@specimen_id", specimen_id);
                param.Param = new MySqlParameter("@data_checksum", data_checksum);
                param.Param = new MySqlParameter("@report_type", report_type);

                MySqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param.Params);

                if (dr.Read())
                {
                    parser_file_activity_id = Convert.ToInt32(dr["parser_file_activity_id"]);
                    downloaded_on = Convert.ToDateTime(dr["downloaded_on"]);
                    report_info_id = Convert.ToInt32(dr["report_info_id"]);
                    is_data_of_record = Convert.ToInt32(dr["is_data_of_record"]) > 0;
                    mismatched_report_id = Convert.ToInt32(dr["mismatched_report_id"]);
                    mismatched_count = Convert.ToInt32(dr["mismatched_count"]);
                    inserted_on = Convert.ToDateTime(dr["inserted_on"]);
                    downloaded_on = Convert.ToDateTime(dr["downloaded_on"]);
                    parse_attempt_count = Convert.ToInt32(dr["parse_attempt_count"]);
                    The_current_data_file_of_record = Convert.ToInt32(dr["CurrentDataFileOfRecord"]);
                    var CurrentDataFileOfRecordArchived = Convert.ToInt32(dr["CurrentDataFileOfRecordArchived"]);

                    _logger.Debug($"This is the document of record. Archived? {CurrentDataFileOfRecordArchived > 0}");
                    Add_To_Database = false;
                    if (CurrentDataFileOfRecordArchived > 0)
                    {
                        _logger.Error($"ATTN: {_FilenameNoPath} is considered data of record, but it was archived in report_info!! Clearing this flag");
                        Add_To_Database = true;
                    }
                    //is_data_of_record
                }
                else
                {
                    Add_To_Database = true;
                }
            }
            // bump parse count
            if (parser_file_activity_id > 0)
            {
                using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
                {
                    conn.Open();

                    MySqlTransaction trans = conn.BeginTransaction();
                    try
                    {
                        param.reset();
                        sqlQuery = "update parser_file_activity p set p.parse_attempt_count = p.parse_attempt_count + 1 where p.parser_file_activity_id = @parser_file_activity_id ; ";
                        param.Param = new MySqlParameter("@parser_file_activity_id", parser_file_activity_id);
                        SqlHelper.ExecuteNonQuery(conn, CommandType.Text, sqlQuery, param.Params);
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        _logger.Error($"Attempt top add increae {filename} parse count for {specimen_id} failed!");
                        _logger.Error(ex.Message);
                        _logger.Error(ex.InnerException.ToString());
                        //throw ex;
                    }
                }
            }

            // check if a mismatch
            if (parser_file_activity_id < 1)
            {
                // We always reparse a mismatch and it's only added to parser_file_activity when it's added to the database.
                // this is to ensure if lab codes are entered, it's parsed and added to the databaes.
                using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
                {
                    param.reset();
                    sqlQuery = @"
                select mm.mismatched_report_id, mm.mismatched_count
                    from mismatched_reports mm
                where
                    mm.file_name = @filename order by mm.mismatched_report_id desc limit 1;
";

                    param.Param = new MySqlParameter("@filename", _FilenameNoPath); // We sometimes get files that are *exactly* the same.
                    MySqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param.Params);

                    if (dr.Read())
                    {
                        mismatched_report_id = Convert.ToInt32(dr["mismatched_report_id"]);
                        mismatched_count = Convert.ToInt32(dr["mismatched_count"]);
                        parse_attempt_count = mismatched_count;
                    }
                    else
                    {
                        Add_To_Database = true;
                    }
                    //_isMismatch = Convert.ToInt32(SqlHelper.ExecuteScalar(conn, CommandType.Text, sqlQuery, param.Params)) > 0;
                }
            }
        }

        #endregion private
    }

    public class BackendParserReportHelper
    {
        // this class will generate a report of mismatches
        private static ILogger _logger;

        public List<BackendParserReportHelperItem> Items = new List<BackendParserReportHelperItem>();
        private string _report = string.Empty;

        public BackendParserReportHelper(ILogger __logger = null)
        {
            init(__logger);
        }

        /// <summary>
        /// This function should generate the actual report
        /// </summary>
        /// <returns></returns>
        public string Report()
        {
            return _report;
        }

        private void init(ILogger __logger)
        {
            if (__logger != null)
            {
                _logger = __logger;
                _logger.Debug("BackendParserReportHelper logger online");
                _logger.Debug("Path to this lib: " + AssemblyDirectory);
            }
            else
            {
                _logger = new LoggerConfiguration().CreateLogger();
            }
        }

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
    }

    public class BackendParserReportHelperItem
    {
        public string FileName { get; set; } = string.Empty;
        public ReportType ReportTypeEnum { get; set; }
        public string SpecimenID { get; set; } = string.Empty;
        public string Donor { get; set; } = string.Empty;
        public string donor_id { get; set; } = string.Empty;
        public bool donor_backfilled { get; set; } = false;
        public bool multiple_donor_pid_found { get; set; } = false;
        public int donor_pid_match_count { get; set; } = 0;

        public bool donor_found_by_specimenID { get; set; } = false;
        public string Client { get; set; } = string.Empty;
        public int client_id { get; set; } = 0;
        public string ClientDepartment { get; set; } = string.Empty;
        public int client_department_id { get; set; } = 0;
        private string _CollectionDate { get; set; } = string.Empty;

        public string LabCode { get; set; } = string.Empty;
        public string CrlClientCode { get; set; } = string.Empty;

        public string Panel { get; set; } = string.Empty;
        public string OBR { get; set; } = string.Empty;
        public string ORC { get; set; } = string.Empty;
        public bool NonContactPositive { get; set; } = false;
        public int donor_test_info_id { get; set; } = 0;

        public bool SpecimenIDFound { get; set; } = false;
        public bool SpecimenIDMatched { get; set; } = false;
        public bool DonorFound { get; set; } = false;
        public bool CollectionDateFound { get; set; } = false;
        public bool LabCodeFound { get; set; } = false;
        public bool LabCodeMatched { get; set; } = false;
        public bool PanelFound { get; set; } = false;

        public bool mismatch { get; set; } = false;
        public int mismatched_count { get; set; } = 0;

        public bool NoPIDInFile { get; set; } = false;

        public string donor_lastname { get; set; } = string.Empty;
        public string donor_firstname { get; set; } = string.Empty;
        public string donor_name { get; set; } = string.Empty;

        private TimeAgo _Age { get; set; } = TimeAgo._Unknown;

        public string CollectionDate
        {
            get
            {
                return _CollectionDate;
            }
            set
            {
                _CollectionDate = value;
                try
                {
                    if (CollectionDateFound == false) return;
                    DateTime now = DateTime.Now;
                    DateTime _SpecimenCollectionDateTime;
                    DateTime.TryParseExact(value, "yyyyMMddHHmm", CultureInfo.InvariantCulture, DateTimeStyles.None, out _SpecimenCollectionDateTime);
                    TimeSpan elapsed = now.Subtract(_SpecimenCollectionDateTime);
                    int daysAgo = (int)elapsed.TotalDays;
                    _Age = GetTimeAgo(daysAgo);
                }
                catch (Exception)
                {
                }
            }
        }

        public TimeAgo Age
        {
            get
            {
                return _Age;
            }
        }

        private TimeAgo GetTimeAgo(int days)
        {
            if (days < 30) return TimeAgo._0to30;
            if (days > 30 && days < 60) return TimeAgo._30to60;
            if (days > 60 && days < 90) return TimeAgo._60to90;
            if (days > 90 && days < 120) return TimeAgo._90to120;
            if (days > 120 && days < 180) return TimeAgo._120to180;
            if (days > 180) return TimeAgo._180Plus;
            return TimeAgo._Unknown;
        }
    }

    public enum TimeAgo
    {
        [Description("Unknown")]
        _Unknown = 0,

        [Description("0-30 Days")]
        _0to30 = 1,

        [Description("30-60 Days")]
        _30to60 = 2,

        [Description("60-90 Days")]
        _60to90 = 3,

        [Description("90-120 Days")]
        _90to120 = 4,

        [Description("120-180 Days")]
        _120to180 = 5,

        [Description("180+ Days")]
        _180Plus = 6
    }

    public class BackendParserReportbuilder
    {
        public int Width { get; set; } = 120;

        private string _reportText { get; set; } = string.Empty;

        public void newLine(string text = null)
        {
            if (string.IsNullOrEmpty(text)) text = string.Empty;
            _reportText += text + "\r\n";
        }

        public void BlankLines(int count = 0)
        {
            if (count == 0) count = 1; // do blank line if no line counts are passed.
            for (int i = 0; i < count; i++)
            {
                newLine();
            }
        }

        public void NewHorizontalBar(char _l = '_')
        {
            newLine(new String(_l, Width));
        }

        public void NewHeader(string header)
        {
            int l = header.Length / 2;
            int pad = (Width / 2) - l;
            string _format = "{0," + pad.ToString() + "}";
            string _header = string.Format(_format, header);
            newLine(_header);
            NewHorizontalBar();
        }

        public string ReportText
        { get { return _reportText; } }

        public string Line
        { set { newLine(value); } }
        public string Header
        { set { NewHeader(value); } }
        public string Bar
        { set { NewHorizontalBar(); } }
    }
}