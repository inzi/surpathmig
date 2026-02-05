using MySql.Data.MySqlClient;
using RTF;
using Serilog;
using SurPath.Entity;
using SurPath.Enum;
using SurPath.MySQLHelper;
using System;
using System.Collections.Generic;
using System.Data;

namespace SurPath.Data
{
    /// <summary>
    /// Gets Report
    /// </summary>
    public partial class HL7ParserDao : DataObject
    {
        //public static ILogger _logger;

        //public HL7ParserDao(ILogger __logger = null)
        //{
        //    if (__logger == null)
        //    {
        //        _logger = new LoggerConfiguration().CreateLogger();
        //    }
        //    else
        //    {
        //        _logger = __logger;
        //    }
        //}

        public int DetermineLabReportType(string specimenId, List<int> _types = null)
        {
            _logger.Debug($"DetermineLabReportType called {specimenId}");
            if (_types == null)
            {
                _logger.Debug("Types empty, defaulting to 1,2,3");
                _types = new List<int>() { 1, 2, 3 }; // lab, mro, or quest
            }
            try
            {
                _logger.Debug($"DetermineLabReportType for specimenId: {specimenId}");
                string sqlQuery = "SELECT "
                                    + "report_type "
                                    + "FROM report_info WHERE specimen_id = @SpecimenID";
                if (_types.Count>0)
                {
                    sqlQuery = sqlQuery + " AND report_type in (";
                    foreach(int _t in _types)
                    {
                        sqlQuery = sqlQuery + $"{_t},";
                    }
                    sqlQuery = sqlQuery.TrimEnd(',');
                    sqlQuery = sqlQuery + ")";
                }
                _logger.Debug($"DetermineLabReportType sqlQuery: {sqlQuery}");
                MySqlParameter[] param = new MySqlParameter[1];
                param[0] = new MySqlParameter("@SpecimenID", specimenId);

                DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sqlQuery, param);
                if (ds.Tables.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    var reportType = row[0];
                    return (int)reportType;

                }
                else
                {
                    return 0;
                }
            }
            catch(Exception ex)
            {
                _logger.Error(ex.Message);
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                
                return 0;
            }
        }

        public ReportInfo GetReportDetails(ReportType reportType, string specimenId, ReportInfo reportDetails)
        {
            _logger.Debug($"GetReportDetails: reportType {reportType.ToString()} specimenId {specimenId}");

            string sqlQuery = "SELECT "
                                + "report_info_id AS ReportId, "
                                + "specimen_id AS SpecimenId, "
                                + "lab_sample_id AS LabSampleId, "
                                + "ssn_id AS SsnId, "
                                + "donor_last_name AS DonorLastName, "
                                + "donor_first_name AS DonorFirstName, "
                                + "donor_mi AS DonorMI, "
                                + "donor_dob AS DonorDOB, "
                                + "donor_gender AS DonorGender, "
                                + "lab_report AS LabReport, "
                                + "OCTET_LENGTH(lab_report) as LabReportLength, "
                                + "screening_time as Screening_Time, "
                                + "created_on AS CreatedOn "
                                 //+ "FROM report_info WHERE is_archived = b'0' AND report_type = @ReportType AND specimen_id = @SpecimenId";
                                 + "FROM report_info WHERE is_archived = b'0' AND specimen_id = @SpecimenId AND report_type = @ReportType";

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                MySqlParameter[] param = new MySqlParameter[2];
                param[0] = new MySqlParameter("@ReportType", (int)reportType);
                param[1] = new MySqlParameter("@SpecimenId", specimenId);

                MySqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                if (dr.Read())
                {
                    _logger.Debug($"Results from query returned");
                    reportDetails = new ReportInfo();

                    reportDetails.ReportId = (int)dr["ReportId"];
                    reportDetails.SpecimenId = dr["SpecimenId"].ToString();
                    reportDetails.LabSampleId = dr["LabSampleId"].ToString();
                    reportDetails.SsnId = dr["SsnId"].ToString();
                    reportDetails.DonorLastName = dr["DonorLastName"].ToString();
                    reportDetails.DonorFirstName = dr["DonorFirstName"].ToString();
                    reportDetails.DonorMI = dr["DonorMI"].ToString();
                    reportDetails.DonorDOB = dr["DonorDOB"].ToString();
                    reportDetails.DonorGender = dr["DonorGender"].ToString();
                    reportDetails.CreatedOn = Convert.ToDateTime(dr["CreatedOn"]);
                    reportDetails.SpecimenCollectionDate = dr["Screening_Time"].ToString();
                    // TODO - need to test this to ensure it's not way in the past (date time min) - and if so, put N/A or something
                    // we'll use 6 months from creation time of this report

                    DateTime.TryParse(reportDetails.SpecimenCollectionDate, out DateTime _DTTEST);
                    TimeSpan timeSpan = reportDetails.CreatedOn - _DTTEST;

                    if (Math.Abs((int)timeSpan.TotalDays) > 60) reportDetails.SpecimenCollectionDate = "N/A";
                    if (dr["LabReport"] != DBNull.Value)
                    {
                        _logger.Debug($"LabReport not null, fetching...");
                        int blobLength = (int)(long)dr["LabReportLength"];
                        //crlReport = new RTFBuilder();
                        reportDetails.LabReportByte = (byte[])dr["LabReport"];

                        reportDetails.LabReport = System.Text.Encoding.UTF8.GetString(reportDetails.LabReportByte);
                    }
                    else
                    {
                        _logger.Debug($"LabReport is null!");

                    }
                }
                else
                {
                    _logger.Debug($"NO RESULTS FROM QUERY");
                }
                dr.Close();
            }

            return reportDetails;
        }

        public bool GetReportByID(int report_info_id, ReportInfo reportDetails, ref List<OBR_Info> obrList, ref RTFBuilderbase crlReport)
        {
            bool returnValue = false;

            try
            {
                string sqlQuery = "SELECT "
                                    + "report_info_id AS ReportId, "
                                    + "specimen_id AS SpecimenId, "
                                    + "lab_sample_id AS LabSampleId, "
                                    + "ssn_id AS SsnId, "
                                    + "donor_last_name AS DonorLastName, "
                                    + "donor_first_name AS DonorFirstName, "
                                    + "donor_mi AS DonorMI, "
                                    + "donor_dob AS DonorDOB, "
                                    + "donor_gender AS DonorGender, "
                                    + "lab_report AS LabReport, "
                                    + "created_on AS CreatedOn "
                                    + "FROM report_info WHERE report_info_id = @report_info_id;";
                //+ "FROM report_info WHERE is_archived = b'0' AND specimen_id = @SpecimenId";

                using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
                {
                    ParamHelper param = new ParamHelper();
                    param.Param = new MySqlParameter("@report_info_id", (int)report_info_id);
                    //param.Param = new MySqlParameter("@report_info_id", report_info_id.ToString());

                    MySqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param.Params);

                    if (dr.Read())
                    {
                        reportDetails = new ReportInfo();

                        reportDetails.ReportId = (int)dr["ReportId"];
                        reportDetails.SpecimenId = dr["SpecimenId"].ToString();
                        reportDetails.LabSampleId = dr["LabSampleId"].ToString();
                        reportDetails.SsnId = dr["SsnId"].ToString();
                        reportDetails.DonorLastName = dr["DonorLastName"].ToString();
                        reportDetails.DonorFirstName = dr["DonorFirstName"].ToString();
                        reportDetails.DonorMI = dr["DonorMI"].ToString();
                        reportDetails.DonorDOB = dr["DonorDOB"].ToString();
                        reportDetails.DonorGender = dr["DonorGender"].ToString();
                        reportDetails.CreatedOn = Convert.ToDateTime(dr["CreatedOn"]);

                        if (dr["LabReport"] != DBNull.Value)
                        {
                            byte[] _LabReport = (byte[])dr["LabReport"];

                            crlReport = new RTFBuilder();

                            string data = System.Text.Encoding.UTF8.GetString(_LabReport);

                            string dataRtf = System.Text.Encoding.UTF8.GetString(_LabReport);

                            crlReport.AppendRTF(dataRtf);
                            //crlReport.Append();
                        }

                        returnValue = true;
                    }
                    dr.Close();

                    if (reportDetails != null)
                    {
                        sqlQuery = "SELECT "
                                        + "obr_info_id AS OBRInfoId, "
                                        + "transmited_order AS TransmitedOrder, "
                                        + "collection_site_info AS CollectionSiteInfo, "
                                        + "specimen_collection_date AS SpecimenCollectionDate, "
                                        + "specimen_received_date AS SpecimenReceivedDate, "
                                        + "crl_client_code AS CrlClientCode, "
                                        + "specimen_type AS SpecimenType, "
                                        + "section_header AS SectionHeader, "
                                        + "crl_transmit_date AS CrlTransmitDate, "
                                        + "service_section_id AS ServiceSectionId, "
                                        + "order_status AS OrderStatus, "
                                        + "reason_type AS ReasonType "
                                        + "FROM obr_info WHERE report_info_id = @ReportInfoId";

                        param.reset();
                        param.Param = new MySqlParameter("@ReportInfoId", reportDetails.ReportId);

                        dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param.Params);

                        obrList = new List<OBR_Info>();

                        while (dr.Read())
                        {
                            OBR_Info obr = new OBR_Info();

                            obr.OBRInfoId = (int)dr["OBRInfoId"];
                            obr.TransmitedOrder = (int)dr["TransmitedOrder"];
                            obr.CollectionSiteInfo = dr["CollectionSiteInfo"].ToString();
                            obr.SpecimenCollectionDate = dr["SpecimenCollectionDate"].ToString();
                            obr.SpecimenReceivedDate = dr["SpecimenReceivedDate"].ToString();
                            obr.CrlClientCode = dr["CrlClientCode"].ToString();
                            obr.SpecimenType = dr["SpecimenType"].ToString();
                            obr.SectionHeader = dr["SectionHeader"].ToString();
                            obr.CrlTransmitDate = dr["CrlTransmitDate"].ToString();
                            obr.ServiceSectionId = dr["ServiceSectionId"].ToString();
                            obr.OrderStatus = dr["OrderStatus"].ToString();
                            obr.ReasonType = dr["ReasonType"].ToString();

                            obr.observatinos = new List<OBX_Info>();

                            obrList.Add(obr);
                        }
                        dr.Close();

                        foreach (OBR_Info obr in obrList)
                        {
                            sqlQuery = "SELECT "
                                            + "obx_info_id AS OBXInfoId, "
                                            + "sequence AS Sequence, "
                                            + "test_code AS TestCode, "
                                            + "test_name AS TestName, "
                                            + "result AS Result, "
                                            + "status AS Status, "
                                            + "unit_of_measure AS UnitOfMeasure, "
                                            + "reference_range AS ReferenceRange, "
                                            + "order_status AS OrderStatus "
                                            + "FROM obx_info WHERE obr_info_id = @OBRInfoId";

                            param.reset();
                            param.Param = new MySqlParameter("@OBRInfoId", obr.OBRInfoId);

                            dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param.Params);

                            while (dr.Read())
                            {
                                OBX_Info obx = new OBX_Info();

                                obx.OBXInfoId = (int)dr["OBXInfoId"];
                                obx.Sequence = (int)dr["Sequence"];
                                obx.TestCode = dr["TestCode"].ToString();
                                obx.TestName = dr["TestName"].ToString();
                                obx.Result = dr["Result"].ToString();
                                obx.Status = dr["Status"].ToString();
                                obx.UnitOfMeasure = dr["UnitOfMeasure"].ToString();
                                obx.ReferenceRange = dr["ReferenceRange"].ToString();
                                obx.OrderStatus = dr["OrderStatus"].ToString();

                                obr.observatinos.Add(obx);
                            }
                            dr.Close();
                        }
                    }
                }

                returnValue = true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                throw;
            }

            return returnValue;
        }

        public bool GetReport(ReportType reportType, string specimenId, ReportInfo reportDetails, ref List<OBR_Info> obrList, ref RTFBuilderbase crlReport)
        {
            bool returnValue = false;

            try
            {
                string sqlQuery = "SELECT "
                                    + "report_info_id AS ReportId, "
                                    + "specimen_id AS SpecimenId, "
                                    + "lab_sample_id AS LabSampleId, "
                                    + "ssn_id AS SsnId, "
                                    + "donor_last_name AS DonorLastName, "
                                    + "donor_first_name AS DonorFirstName, "
                                    + "donor_mi AS DonorMI, "
                                    + "donor_dob AS DonorDOB, "
                                    + "donor_gender AS DonorGender, "
                                    + "lab_report AS LabReport, "
                                    + "created_on AS CreatedOn "
                                    + "FROM report_info WHERE is_archived = b'0' AND report_type = @ReportType AND specimen_id = @SpecimenId";
                //+ "FROM report_info WHERE is_archived = b'0' AND specimen_id = @SpecimenId";

                using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
                {
                    MySqlParameter[] param = new MySqlParameter[2];
                    param[0] = new MySqlParameter("@ReportType", (int)reportType);
                    param[1] = new MySqlParameter("@SpecimenId", specimenId);

                    MySqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                    if (dr.Read())
                    {
                        reportDetails = new ReportInfo();

                        reportDetails.ReportId = (int)dr["ReportId"];
                        reportDetails.SpecimenId = dr["SpecimenId"].ToString();
                        reportDetails.LabSampleId = dr["LabSampleId"].ToString();
                        reportDetails.SsnId = dr["SsnId"].ToString();
                        reportDetails.DonorLastName = dr["DonorLastName"].ToString();
                        reportDetails.DonorFirstName = dr["DonorFirstName"].ToString();
                        reportDetails.DonorMI = dr["DonorMI"].ToString();
                        reportDetails.DonorDOB = dr["DonorDOB"].ToString();
                        reportDetails.DonorGender = dr["DonorGender"].ToString();
                        reportDetails.CreatedOn = Convert.ToDateTime(dr["CreatedOn"]);

                        if (dr["LabReport"] != DBNull.Value)
                        {
                            crlReport = new RTFBuilder();
                            crlReport.Append(System.Text.Encoding.UTF8.GetString((byte[])dr["LabReport"]));
                        }

                        returnValue = true;
                    }
                    dr.Close();

                    if (reportDetails != null)
                    {
                        sqlQuery = "SELECT "
                                        + "obr_info_id AS OBRInfoId, "
                                        + "transmited_order AS TransmitedOrder, "
                                        + "collection_site_info AS CollectionSiteInfo, "
                                        + "specimen_collection_date AS SpecimenCollectionDate, "
                                        + "specimen_received_date AS SpecimenReceivedDate, "
                                        + "crl_client_code AS CrlClientCode, "
                                        + "specimen_type AS SpecimenType, "
                                        + "section_header AS SectionHeader, "
                                        + "crl_transmit_date AS CrlTransmitDate, "
                                        + "service_section_id AS ServiceSectionId, "
                                        + "order_status AS OrderStatus, "
                                        + "reason_type AS ReasonType "
                                        + "FROM obr_info WHERE report_info_id = @ReportInfoId";

                        param = new MySqlParameter[1];
                        param[0] = new MySqlParameter("@ReportInfoId", reportDetails.ReportId);

                        dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                        obrList = new List<OBR_Info>();

                        while (dr.Read())
                        {
                            OBR_Info obr = new OBR_Info();

                            obr.OBRInfoId = (int)dr["OBRInfoId"];
                            obr.TransmitedOrder = (int)dr["TransmitedOrder"];
                            obr.CollectionSiteInfo = dr["CollectionSiteInfo"].ToString();
                            obr.SpecimenCollectionDate = dr["SpecimenCollectionDate"].ToString();
                            obr.SpecimenReceivedDate = dr["SpecimenReceivedDate"].ToString();
                            obr.CrlClientCode = dr["CrlClientCode"].ToString();
                            obr.SpecimenType = dr["SpecimenType"].ToString();
                            obr.SectionHeader = dr["SectionHeader"].ToString();
                            obr.CrlTransmitDate = dr["CrlTransmitDate"].ToString();
                            obr.ServiceSectionId = dr["ServiceSectionId"].ToString();
                            obr.OrderStatus = dr["OrderStatus"].ToString();
                            obr.ReasonType = dr["ReasonType"].ToString();

                            obr.observatinos = new List<OBX_Info>();

                            obrList.Add(obr);
                        }
                        dr.Close();

                        foreach (OBR_Info obr in obrList)
                        {
                            sqlQuery = "SELECT "
                                            + "obx_info_id AS OBXInfoId, "
                                            + "sequence AS Sequence, "
                                            + "test_code AS TestCode, "
                                            + "test_name AS TestName, "
                                            + "result AS Result, "
                                            + "status AS Status, "
                                            + "unit_of_measure AS UnitOfMeasure, "
                                            + "reference_range AS ReferenceRange, "
                                            + "order_status AS OrderStatus "
                                            + "FROM obx_info WHERE obr_info_id = @OBRInfoId";

                            param = new MySqlParameter[1];
                            param[0] = new MySqlParameter("@OBRInfoId", obr.OBRInfoId);

                            dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                            while (dr.Read())
                            {
                                OBX_Info obx = new OBX_Info();

                                obx.OBXInfoId = (int)dr["OBXInfoId"];
                                obx.Sequence = (int)dr["Sequence"];
                                obx.TestCode = dr["TestCode"].ToString();
                                obx.TestName = dr["TestName"].ToString();
                                obx.Result = dr["Result"].ToString();
                                obx.Status = dr["Status"].ToString();
                                obx.UnitOfMeasure = dr["UnitOfMeasure"].ToString();
                                obx.ReferenceRange = dr["ReferenceRange"].ToString();
                                obx.OrderStatus = dr["OrderStatus"].ToString();

                                obr.observatinos.Add(obx);
                            }
                            dr.Close();
                        }
                    }
                }

                returnValue = true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                throw;
            }

            return returnValue;
        }

        public bool GetReportLAB(ReportType reportType, string specimenId, ReportInfo reportDetails, ref List<OBR_Info> obrList, ref RTFBuilderbase crlReport)
        {
            bool returnValue = false;

            try
            {
                string sqlQuery = "SELECT "
                                    + "report_info_id AS ReportId, "
                                    + "specimen_id AS SpecimenId, "
                                    + "lab_sample_id AS LabSampleId, "
                                    + "ssn_id AS SsnId, "
                                    + "donor_last_name AS DonorLastName, "
                                    + "donor_first_name AS DonorFirstName, "
                                    + "donor_mi AS DonorMI, "
                                    + "donor_dob AS DonorDOB, "
                                    + "donor_gender AS DonorGender, "
                                    + "lab_report AS LabReport, "
                                    + "created_on AS CreatedOn "
                                    + "FROM report_info WHERE is_archived = b'0' AND report_type = @ReportType AND specimen_id = @SpecimenId";
                //+ "FROM report_info WHERE is_archived = b'0' AND specimen_id = @SpecimenId";

                using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
                {
                    MySqlParameter[] param = new MySqlParameter[2];
                    param[0] = new MySqlParameter("@ReportType", (int)reportType);
                    param[1] = new MySqlParameter("@SpecimenId", specimenId);

                    MySqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                    if (dr.Read())
                    {
                        reportDetails = new ReportInfo();

                        reportDetails.ReportId = (int)dr["ReportId"];
                        reportDetails.SpecimenId = dr["SpecimenId"].ToString();
                        reportDetails.LabSampleId = dr["LabSampleId"].ToString();
                        reportDetails.SsnId = dr["SsnId"].ToString();
                        reportDetails.DonorLastName = dr["DonorLastName"].ToString();
                        reportDetails.DonorFirstName = dr["DonorFirstName"].ToString();
                        reportDetails.DonorMI = dr["DonorMI"].ToString();
                        reportDetails.DonorDOB = dr["DonorDOB"].ToString();
                        reportDetails.DonorGender = dr["DonorGender"].ToString();
                        reportDetails.CreatedOn = Convert.ToDateTime(dr["CreatedOn"]);

                        if (dr["LabReport"] != DBNull.Value)
                        {
                            crlReport = new RTFBuilder();
                            crlReport.Append(System.Text.Encoding.UTF8.GetString((byte[])dr["LabReport"]));
                        }

                        returnValue = true;
                    }
                    dr.Close();

                    if (reportDetails != null)
                    {
                        sqlQuery = "SELECT "
                                        + "obr_info_id AS OBRInfoId, "
                                        + "transmited_order AS TransmitedOrder, "
                                        + "collection_site_info AS CollectionSiteInfo, "
                                        + "specimen_collection_date AS SpecimenCollectionDate, "
                                        + "specimen_received_date AS SpecimenReceivedDate, "
                                        + "crl_client_code AS CrlClientCode, "
                                        + "specimen_type AS SpecimenType, "
                                        + "section_header AS SectionHeader, "
                                        + "crl_transmit_date AS CrlTransmitDate, "
                                        + "service_section_id AS ServiceSectionId, "
                                        + "order_status AS OrderStatus, "
                                        + "reason_type AS ReasonType "
                                        + "FROM obr_info WHERE report_info_id = @ReportInfoId";

                        param = new MySqlParameter[1];
                        param[0] = new MySqlParameter("@ReportInfoId", reportDetails.ReportId);

                        dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                        obrList = new List<OBR_Info>();

                        while (dr.Read())
                        {
                            OBR_Info obr = new OBR_Info();

                            obr.OBRInfoId = (int)dr["OBRInfoId"];
                            obr.TransmitedOrder = (int)dr["TransmitedOrder"];
                            obr.CollectionSiteInfo = dr["CollectionSiteInfo"].ToString();
                            obr.SpecimenCollectionDate = dr["SpecimenCollectionDate"].ToString();
                            obr.SpecimenReceivedDate = dr["SpecimenReceivedDate"].ToString();
                            obr.CrlClientCode = dr["CrlClientCode"].ToString();
                            obr.SpecimenType = dr["SpecimenType"].ToString();
                            obr.SectionHeader = dr["SectionHeader"].ToString();
                            obr.CrlTransmitDate = dr["CrlTransmitDate"].ToString();
                            obr.ServiceSectionId = dr["ServiceSectionId"].ToString();
                            obr.OrderStatus = dr["OrderStatus"].ToString();
                            obr.ReasonType = dr["ReasonType"].ToString();

                            obr.observatinos = new List<OBX_Info>();

                            obrList.Add(obr);
                        }
                        dr.Close();

                        foreach (OBR_Info obr in obrList)
                        {
                            sqlQuery = "SELECT "
                                            + "obx_info_id AS OBXInfoId, "
                                            + "sequence AS Sequence, "
                                            + "test_code AS TestCode, "
                                            + "test_name AS TestName, "
                                            + "result AS Result, "
                                            + "status AS Status, "
                                            + "unit_of_measure AS UnitOfMeasure, "
                                            + "reference_range AS ReferenceRange, "
                                            + "order_status AS OrderStatus "
                                            + "FROM obx_info WHERE obr_info_id = @OBRInfoId";

                            param = new MySqlParameter[1];
                            param[0] = new MySqlParameter("@OBRInfoId", obr.OBRInfoId);

                            dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                            while (dr.Read())
                            {
                                OBX_Info obx = new OBX_Info();

                                obx.OBXInfoId = (int)dr["OBXInfoId"];
                                obx.Sequence = (int)dr["Sequence"];
                                obx.TestCode = dr["TestCode"].ToString();
                                obx.TestName = dr["TestName"].ToString();
                                obx.Result = dr["Result"].ToString();
                                obx.Status = dr["Status"].ToString();
                                obx.UnitOfMeasure = dr["UnitOfMeasure"].ToString();
                                obx.ReferenceRange = dr["ReferenceRange"].ToString();
                                obx.OrderStatus = dr["OrderStatus"].ToString();

                                obr.observatinos.Add(obx);
                            }
                            dr.Close();
                        }
                    }
                }

                returnValue = true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                throw;
            }

            return returnValue;
        }

        public bool GetReportQuest(ReportType reportType, string specimenId, ReportInfo reportDetails, ref List<OBR_Info> obrList, ref RTFBuilderbase crlReport)
        {
            bool returnValue = false;

            try
            {
                string sqlQuery = "SELECT "
                                    + "report_info_id AS ReportId, "
                                    + "specimen_id AS SpecimenId, "
                                    + "lab_sample_id AS LabSampleId, "
                                    + "ssn_id AS SsnId, "
                                    + "donor_last_name AS DonorLastName, "
                                    + "donor_first_name AS DonorFirstName, "
                                    + "donor_mi AS DonorMI, "
                                    + "donor_dob AS DonorDOB, "
                                    + "donor_gender AS DonorGender, "
                                    + "lab_report AS LabReport, "
                                    + "created_on AS CreatedOn "
                 //+ "FROM report_info WHERE is_archived = b'0' AND report_type = @ReportType AND specimen_id = @SpecimenId";
                 + "FROM report_info WHERE is_archived = b'0' AND specimen_id = @SpecimenId";

                using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
                {
                    MySqlParameter[] param = new MySqlParameter[2];
                    param[0] = new MySqlParameter("@ReportType", (int)reportType);
                    param[1] = new MySqlParameter("@SpecimenId", specimenId);

                    MySqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                    if (dr.Read())
                    {
                        reportDetails = new ReportInfo();

                        reportDetails.ReportId = (int)dr["ReportId"];
                        reportDetails.SpecimenId = dr["SpecimenId"].ToString();
                        reportDetails.LabSampleId = dr["LabSampleId"].ToString();
                        reportDetails.SsnId = dr["SsnId"].ToString();
                        reportDetails.DonorLastName = dr["DonorLastName"].ToString();
                        reportDetails.DonorFirstName = dr["DonorFirstName"].ToString();
                        reportDetails.DonorMI = dr["DonorMI"].ToString();
                        reportDetails.DonorDOB = dr["DonorDOB"].ToString();
                        reportDetails.DonorGender = dr["DonorGender"].ToString();
                        reportDetails.CreatedOn = Convert.ToDateTime(dr["CreatedOn"]);

                        if (dr["LabReport"] != DBNull.Value)
                        {
                            crlReport = new RTFBuilder();
                            crlReport.Append(System.Text.Encoding.UTF8.GetString((byte[])dr["LabReport"]));
                        }

                        returnValue = true;
                    }
                    dr.Close();

                    if (reportDetails != null)
                    {
                        sqlQuery = "SELECT "
                                        + "obr_info_id AS OBRInfoId, "
                                        + "transmited_order AS TransmitedOrder, "
                                        + "collection_site_info AS CollectionSiteInfo, "
                                        + "specimen_collection_date AS SpecimenCollectionDate, "
                                        + "specimen_received_date AS SpecimenReceivedDate, "
                                        + "crl_client_code AS CrlClientCode, "
                                        + "specimen_type AS SpecimenType, "
                                        + "section_header AS SectionHeader, "
                                        + "crl_transmit_date AS CrlTransmitDate, "
                                        + "service_section_id AS ServiceSectionId, "
                                        + "order_status AS OrderStatus, "
                                        + "reason_type AS ReasonType "
                                        + "FROM obr_info WHERE report_info_id = @ReportInfoId";

                        param = new MySqlParameter[1];
                        param[0] = new MySqlParameter("@ReportInfoId", reportDetails.ReportId);

                        dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                        obrList = new List<OBR_Info>();

                        while (dr.Read())
                        {
                            OBR_Info obr = new OBR_Info();

                            obr.OBRInfoId = (int)dr["OBRInfoId"];
                            obr.TransmitedOrder = (int)dr["TransmitedOrder"];
                            obr.CollectionSiteInfo = dr["CollectionSiteInfo"].ToString();
                            obr.SpecimenCollectionDate = dr["SpecimenCollectionDate"].ToString();
                            obr.SpecimenReceivedDate = dr["SpecimenReceivedDate"].ToString();
                            obr.CrlClientCode = dr["CrlClientCode"].ToString();
                            obr.SpecimenType = dr["SpecimenType"].ToString();
                            obr.SectionHeader = dr["SectionHeader"].ToString();
                            obr.CrlTransmitDate = dr["CrlTransmitDate"].ToString();
                            obr.ServiceSectionId = dr["ServiceSectionId"].ToString();
                            obr.OrderStatus = dr["OrderStatus"].ToString();
                            obr.ReasonType = dr["ReasonType"].ToString();

                            obr.observatinos = new List<OBX_Info>();

                            obrList.Add(obr);
                        }
                        dr.Close();

                        foreach (OBR_Info obr in obrList)
                        {
                            sqlQuery = "SELECT "
                                            + "obx_info_id AS OBXInfoId, "
                                            + "sequence AS Sequence, "
                                            + "test_code AS TestCode, "
                                            + "test_name AS TestName, "
                                            + "result AS Result, "
                                            + "status AS Status, "
                                            + "unit_of_measure AS UnitOfMeasure, "
                                            + "reference_range AS ReferenceRange, "
                                            + "order_status AS OrderStatus "
                                            + "FROM obx_info WHERE obr_info_id = @OBRInfoId";

                            param = new MySqlParameter[1];
                            param[0] = new MySqlParameter("@OBRInfoId", obr.OBRInfoId);

                            dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                            while (dr.Read())
                            {
                                OBX_Info obx = new OBX_Info();

                                obx.OBXInfoId = (int)dr["OBXInfoId"];
                                obx.Sequence = (int)dr["Sequence"];
                                obx.TestCode = dr["TestCode"].ToString();
                                obx.TestName = dr["TestName"].ToString();
                                obx.Result = dr["Result"].ToString();
                                obx.Status = dr["Status"].ToString();
                                obx.UnitOfMeasure = dr["UnitOfMeasure"].ToString();
                                obx.ReferenceRange = dr["ReferenceRange"].ToString();
                                obx.OrderStatus = dr["OrderStatus"].ToString();

                                obr.observatinos.Add(obx);
                            }
                            dr.Close();
                        }
                    }
                }

                returnValue = true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                throw;
            }

            return returnValue;
        }

        public bool GetReportMRO(ReportType reportType, string specimenId, ReportInfo reportDetails, ref List<OBR_Info> obrList, ref RTFBuilderbase crlReport)
        {
            bool returnValue = false;

            try
            {
                string sqlQuery = "SELECT "
                                    + "report_info_id AS ReportId, "
                                    + "specimen_id AS SpecimenId, "
                                    + "lab_sample_id AS LabSampleId, "
                                    + "ssn_id AS SsnId, "
                                    + "donor_last_name AS DonorLastName, "
                                    + "donor_first_name AS DonorFirstName, "
                                    + "donor_mi AS DonorMI, "
                                    + "donor_dob AS DonorDOB, "
                                    + "donor_gender AS DonorGender, "
                                    + "lab_report AS LabReport, "
                                    + "created_on AS CreatedOn "
                                    + "FROM report_info WHERE is_archived = b'0' AND report_type = @ReportType AND specimen_id = @SpecimenId";
                //+ "FROM report_info WHERE is_archived = b'0' AND specimen_id = @SpecimenId";

                using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
                {
                    MySqlParameter[] param = new MySqlParameter[2];
                    param[0] = new MySqlParameter("@ReportType", (int)reportType);
                    param[1] = new MySqlParameter("@SpecimenId", specimenId);

                    MySqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                    if (dr.Read())
                    {
                        reportDetails = new ReportInfo();

                        reportDetails.ReportId = (int)dr["ReportId"];
                        reportDetails.SpecimenId = dr["SpecimenId"].ToString();
                        reportDetails.LabSampleId = dr["LabSampleId"].ToString();
                        reportDetails.SsnId = dr["SsnId"].ToString();
                        reportDetails.DonorLastName = dr["DonorLastName"].ToString();
                        reportDetails.DonorFirstName = dr["DonorFirstName"].ToString();
                        reportDetails.DonorMI = dr["DonorMI"].ToString();
                        reportDetails.DonorDOB = dr["DonorDOB"].ToString();
                        reportDetails.DonorGender = dr["DonorGender"].ToString();
                        reportDetails.CreatedOn = Convert.ToDateTime(dr["CreatedOn"]);

                        if (dr["LabReport"] != DBNull.Value)
                        {
                            crlReport = new RTFBuilder();
                            crlReport.Append(System.Text.Encoding.UTF8.GetString((byte[])dr["LabReport"]));
                        }

                        returnValue = true;
                    }
                    dr.Close();

                    if (reportDetails != null)
                    {
                        sqlQuery = "SELECT "
                                        + "obr_info_id AS OBRInfoId, "
                                        + "transmited_order AS TransmitedOrder, "
                                        + "collection_site_info AS CollectionSiteInfo, "
                                        + "specimen_collection_date AS SpecimenCollectionDate, "
                                        + "specimen_received_date AS SpecimenReceivedDate, "
                                        + "crl_client_code AS CrlClientCode, "
                                        + "specimen_type AS SpecimenType, "
                                        + "section_header AS SectionHeader, "
                                        + "crl_transmit_date AS CrlTransmitDate, "
                                        + "service_section_id AS ServiceSectionId, "
                                        + "order_status AS OrderStatus, "
                                        + "reason_type AS ReasonType "
                                        + "FROM obr_info WHERE report_info_id = @ReportInfoId";

                        param = new MySqlParameter[1];
                        param[0] = new MySqlParameter("@ReportInfoId", reportDetails.ReportId);

                        dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                        obrList = new List<OBR_Info>();

                        while (dr.Read())
                        {
                            OBR_Info obr = new OBR_Info();

                            obr.OBRInfoId = (int)dr["OBRInfoId"];
                            obr.TransmitedOrder = (int)dr["TransmitedOrder"];
                            obr.CollectionSiteInfo = dr["CollectionSiteInfo"].ToString();
                            obr.SpecimenCollectionDate = dr["SpecimenCollectionDate"].ToString();
                            obr.SpecimenReceivedDate = dr["SpecimenReceivedDate"].ToString();
                            obr.CrlClientCode = dr["CrlClientCode"].ToString();
                            obr.SpecimenType = dr["SpecimenType"].ToString();
                            obr.SectionHeader = dr["SectionHeader"].ToString();
                            obr.CrlTransmitDate = dr["CrlTransmitDate"].ToString();
                            obr.ServiceSectionId = dr["ServiceSectionId"].ToString();
                            obr.OrderStatus = dr["OrderStatus"].ToString();
                            obr.ReasonType = dr["ReasonType"].ToString();

                            obr.observatinos = new List<OBX_Info>();

                            obrList.Add(obr);
                        }
                        dr.Close();

                        foreach (OBR_Info obr in obrList)
                        {
                            sqlQuery = "SELECT "
                                            + "obx_info_id AS OBXInfoId, "
                                            + "sequence AS Sequence, "
                                            + "test_code AS TestCode, "
                                            + "test_name AS TestName, "
                                            + "result AS Result, "
                                            + "status AS Status, "
                                            + "unit_of_measure AS UnitOfMeasure, "
                                            + "reference_range AS ReferenceRange, "
                                            + "order_status AS OrderStatus "
                                            + "FROM obx_info WHERE obr_info_id = @OBRInfoId";

                            param = new MySqlParameter[1];
                            param[0] = new MySqlParameter("@OBRInfoId", obr.OBRInfoId);

                            dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                            while (dr.Read())
                            {
                                OBX_Info obx = new OBX_Info();

                                obx.OBXInfoId = (int)dr["OBXInfoId"];
                                obx.Sequence = (int)dr["Sequence"];
                                obx.TestCode = dr["TestCode"].ToString();
                                obx.TestName = dr["TestName"].ToString();
                                obx.Result = dr["Result"].ToString();
                                obx.Status = dr["Status"].ToString();
                                obx.UnitOfMeasure = dr["UnitOfMeasure"].ToString();
                                obx.ReferenceRange = dr["ReferenceRange"].ToString();
                                obx.OrderStatus = dr["OrderStatus"].ToString();

                                obr.observatinos.Add(obx);
                            }
                            dr.Close();
                        }
                    }
                }

                returnValue = true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                throw;
            }

            return returnValue;
        }

        public ReportInfo GetReportDetailsById(string ReportId, ReportInfo reportDetails)
        {
            string sqlQuery = "SELECT "
                                + "report_info_id AS ReportId, "
                                + "specimen_id AS SpecimenId, "
                                + "lab_sample_id AS LabSampleId, "
                                + "ssn_id AS SsnId, "
                                + "donor_last_name AS DonorLastName, "
                                + "donor_first_name AS DonorFirstName, "
                                + "donor_mi AS DonorMI, "
                                + "donor_dob AS DonorDOB, "
                                + "donor_gender AS DonorGender, "
                                + "lab_report AS LabReport, "
                                + "created_on AS CreatedOn "
                                + "FROM report_info WHERE is_archived = b'0' AND report_info_id = @ReportInfoId";

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                MySqlParameter[] param = new MySqlParameter[1];
                param[0] = new MySqlParameter("@ReportInfoId", ReportId);

                MySqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                if (dr.Read())
                {
                    reportDetails = new ReportInfo();

                    reportDetails.ReportId = (int)dr["ReportId"];
                    reportDetails.SpecimenId = dr["SpecimenId"].ToString();
                    reportDetails.LabSampleId = dr["LabSampleId"].ToString();
                    reportDetails.SsnId = dr["SsnId"].ToString();
                    reportDetails.DonorLastName = dr["DonorLastName"].ToString();
                    reportDetails.DonorFirstName = dr["DonorFirstName"].ToString();
                    reportDetails.DonorMI = dr["DonorMI"].ToString();
                    reportDetails.DonorDOB = dr["DonorDOB"].ToString();
                    reportDetails.DonorGender = dr["DonorGender"].ToString();
                    reportDetails.CreatedOn = Convert.ToDateTime(dr["CreatedOn"]);

                    if (dr["LabReport"] != DBNull.Value)
                    {
                        //crlReport = new RTFBuilder();
                        reportDetails.LabReport = System.Text.Encoding.UTF8.GetString((byte[])dr["LabReport"]);
                    }
                }
                dr.Close();
            }

            return reportDetails;
        }
    }
}