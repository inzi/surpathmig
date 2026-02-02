using MySql.Data.MySqlClient;
using RTF;
using Serilog;
using SurPath.Data;
using SurPath.Enum;
using SurPath.MySQLHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Report_info
{
    class Program
    {

        public static string ConnectionString { get; set; }
        public static ILogger _logger { get; set; }
        public static SHA1CryptoServiceProvider sha1 { get; set; }
        static void Main(string[] args)
        {
            init();
            // This tools is to clean up the database

            // first, identify all the duplicate report_info records. 
            // We can do this by their file hash, but have to exclude "2993129fc9bc4da620cba0767792e2c10368fba8"
            // that's an empty RTF file.

            // So the first thing to do is to get all of these
            // recreate the RTF like the parser does
            // drop the specimen id into it
            // put the blob back in the database and update the checksums
            updateEmptyRTFs();


        }

        static void init()
        {
            _logger = new LoggerConfiguration().ReadFrom.AppSettings().CreateLogger();
            _logger.Information("Logger Loaded");
            ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString().Trim();
            sha1 = new SHA1CryptoServiceProvider();

        }


        static string GetChecksum(string o)
        {
            string checksum = string.Empty;

            // calulate a checksum of the data
            // at this point - reportDetails.data_checksum is the actual data
            byte[] _data_checksum = Encoding.ASCII.GetBytes(o);
            //reportDetails.data_checksum = BitConverter.ToString(sha1.ComputeHash(_data_checksum)).Replace("-", string.Empty); // reportDetails.data_checksum = reportCheckSumData;
            checksum = BitConverter.ToString(sha1.ComputeHash(_data_checksum)).Replace("-", string.Empty);

            return checksum;
        }

        static void updateEmptyRTFs()
        {
            RTFBuilderbase crlReport = new RTFBuilder();
            crlReport.AppendLine(""); // note this - crlReport.AppendLine(ConvertAsciiToUnicode(segment[3])); this wants unicode
            string checksum = GetChecksum(crlReport.ToString());
            string myRTF = crlReport.ToString();
            string RTF = "{\\rtf1\\ansi\\ansicpg1252\\deff0\\deflang3081{\\fonttbl{\\f0\\fswiss\\fprq2\\fcharset0 Arial;}}\r\n{\\colortbl ;\\red0\\green0\\blue0;\\red255\\green255\\blue255;}\r\n\\viewkind4\\uc1\\pard\\plain\\f0\\fs20 \r\n\\\\{\\\\rtf1\\\\ansi\\\\ansicpg1252\\\\deff0\\\\deflang3081\\\\{\\\\fonttbl\\\\{\\\\f0\\\\fswiss\\\\fprq2\\\\fcharset0 Arial;\\\\}\\\\}\\line \\\\{\\\\colortbl ;\\\\red0\\\\green0\\\\blue0;\\\\red255\\\\green255\\\\blue255;\\\\}\\line \\\\viewkind4\\\\uc1\\\\pard\\\\plain\\\\f0\\\\fs20 \\line \\\\}\\line }";
            if (!checksum.Equals("2993129fc9bc4da620cba0767792e2c10368fba8", StringComparison.InvariantCultureIgnoreCase))
            {
                _logger.Error("we're not matching parser!");
                return;
            }


        // We gen a SH1 of the file


        _logger.Information($"updateEmptyRTFs start");
            using (MySqlConnection conn = new MySqlConnection(Program.ConnectionString))
            {
                ParamHelper p = new ParamHelper();
                string sqlQuery = @"select * from report_info where lab_report_checksum = @emptyRTFHash and specimen_id is not null";

                p.Param = new MySqlParameter("@emptyRTFHash", "2993129fc9bc4da620cba0767792e2c10368fba8");

                using (MySqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, p.Params))
                {
                    while (dr.Read())
                    {
                        crlReport = new RTFBuilder();

                        _logger.Information($"Found empty RTF - {dr["report_info_id"]}");
                    }
                }


            }

            _logger.Information($"updateEmptyRTFs return");

        }

    }
}
