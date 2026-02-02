using MySql.Data.MySqlClient;
using SurPath.Entity;
using SurPath.Enum;
using SurPath.MySQLHelper;
using System;
using System.Data;

namespace SurPath.Data
{
    public partial class DonorDao : DataObject
    {
        public ReportInfo GetMROReportId(int testInfoId, ReportType reportType)
        {
            ReportInfo reportInfo = null;
            string sqlQuery = "SELECT "
                                    + "report_info_id AS ReportId, "
                                    + "report_type AS ReportType, "
                                    + "specimen_id AS SpecimenId, "
                                    + "ssn_id AS SsnId, "
                                    + "donor_last_name AS DonorLastName, "
                                    + "donor_first_name AS DonorFirstName, "
                                    + "donor_mi AS DonorMI, "
                                    + "donor_dob AS DonorDOB, "
                                    + "donor_gender AS DonorGender, "
                                    + "final_report_id AS FinalReportId, "
                                    + "donor_test_info_id AS DonorTestInfoId "
                                    + " FROM report_info WHERE is_archived = b'0' and donor_test_info_id = @DonorTestInfoId and report_type=@ReportType";

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                MySqlParameter[] param = new MySqlParameter[2];
                param[0] = new MySqlParameter("@DonorTestInfoId", testInfoId);
                param[1] = new MySqlParameter("@ReportType", reportType);

                MySqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                if (dr.Read())
                {
                    reportInfo = new ReportInfo();
                    if (dr["FinalReportId"] != null && dr["FinalReportId"] != DBNull.Value)
                    {
                        reportInfo.FinalReportId = (int)dr["FinalReportId"];
                    }
                }
            }

            return reportInfo;
        }

        public ReportInfo GetLabReport(int testInfoId, ReportType reportType)
        {
            ReportInfo reportInfoLab = null;
            string sqlQuery = "SELECT "
                                    + "report_info_id AS ReportId, "
                                    + "report_type AS ReportType, "
                                    + "specimen_id AS SpecimenId, "
                                    + "ssn_id AS SsnId, "
                                    + "donor_last_name AS DonorLastName, "
                                    + "donor_first_name AS DonorFirstName, "
                                    + "donor_mi AS DonorMI, "
                                    + "donor_dob AS DonorDOB, "
                                    + "donor_gender AS DonorGender, "
                                    + "report_status AS ReportStatus, "
                                    + "donor_test_info_id AS DonorTestInfoId "
                                    + " FROM report_info WHERE is_archived = b'0' and donor_test_info_id = @DonorTestInfoId and report_type=@ReportType";
            //+ " FROM report_info WHERE is_archived = b'0' and donor_test_info_id = @DonorTestInfoId and (report_type=1 or report_type=2)";
            //+ " FROM report_info WHERE is_archived = b'0' and donor_test_info_id = @DonorTestInfoId";

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                MySqlParameter[] param = new MySqlParameter[2];
                param[0] = new MySqlParameter("@DonorTestInfoId", testInfoId);
                param[1] = new MySqlParameter("@ReportType", reportType);

                MySqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                if (dr.Read())
                {
                    reportInfoLab = new ReportInfo();
                    if (dr["ReportStatus"] != null && dr["ReportStatus"] != DBNull.Value)
                    {
                        reportInfoLab.ReportStatus = (OverAllTestResult)(int)(dr["ReportStatus"]);
                    }
                    if (dr["ReportType"] != null && dr["ReportType"] != DBNull.Value)
                    {
                        reportInfoLab.ReportType = (ReportType)(int)(dr["ReportType"]);
                    }
                }
            }

            return reportInfoLab;
        }

        public ReportInfo GetLabReportType(int testInfoId, string specimenid, ReportType reportType)
        {
            ReportInfo reportInfoLab = null;
            //int reptype = 0;

            //    reptype = 2;

            string sqlQuery = "SELECT "
                                    + "report_info_id AS ReportId, "
                                    + "report_type AS ReportType, "
                                    + "specimen_id AS SpecimenId, "
                                    + "ssn_id AS SsnId, "
                                    + "donor_last_name AS DonorLastName, "
                                    + "donor_first_name AS DonorFirstName, "
                                    + "donor_mi AS DonorMI, "
                                    + "donor_dob AS DonorDOB, "
                                    + "donor_gender AS DonorGender, "
                                    + "report_status AS ReportStatus, "
                                    + "donor_test_info_id AS DonorTestInfoId "
                                    + " FROM report_info WHERE is_archived = b'0' and donor_test_info_id = @DonorTestInfoId and specimen_id=@SpecimenId and report_type=@ReportType ";

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                MySqlParameter[] param = new MySqlParameter[3];
                param[0] = new MySqlParameter("@DonorTestInfoId", testInfoId);
                param[1] = new MySqlParameter("@SpecimenId", specimenid);
                param[2] = new MySqlParameter("@ReportType", reportType);

                MySqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                if (dr.Read())
                {
                    reportInfoLab = new ReportInfo();
                    if (dr["ReportStatus"] != null && dr["ReportStatus"] != DBNull.Value)
                    {
                        reportInfoLab.ReportStatus = (OverAllTestResult)(int)(dr["ReportStatus"]);
                    }
                    if (dr["ReportType"] != null && dr["ReportType"] != DBNull.Value)
                    {
                        reportInfoLab.ReportType = (ReportType)(int)(dr["ReportType"]);
                    }
                }
            }

            return reportInfoLab;
        }
    }
}