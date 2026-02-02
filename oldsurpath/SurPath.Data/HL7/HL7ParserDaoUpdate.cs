using MySql.Data.MySqlClient;
using RTF;
using SurPath.Data.Backend;
using SurPath.Entity;
using SurPath.Enum;
using SurPath.MySQLHelper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SurPath.Data
{
    /// <summary>
    /// UPDATES
    /// </summary>
    public partial class HL7ParserDao : DataObject
    {


        public bool UpdateReport(ReportType reportType, ReportInfo reportDetails, List<OBR_Info> obrList, RTFBuilderbase crlReport, bool archiveExistingReport, ReturnValues returnValues,
            string fileName, int NumPassed, int TotalTo, BackendParserHelper backendParserHelper)
        {
            bool returnValue = false;
            BackendData data = new BackendData(null, null, _logger);

            // check if the FILE is in report info and skip if so BitConverter.ToString(sha1.ComputeHash(bytes)).Replace("-", string.Empty);
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
            // calulate a checksum of the data
            // at this point - reportDetails.data_checksum is the actual data
            byte[] _data_checksum = Encoding.ASCII.GetBytes(reportDetails.data_checksum);
            reportDetails.data_checksum = BitConverter.ToString(sha1.ComputeHash(_data_checksum)).Replace("-", string.Empty); // reportDetails.data_checksum = reportCheckSumData;
            backendParserHelper.data_checksum = reportDetails.data_checksum;
            backendParserHelper.specimen_id = reportDetails.SpecimenId;
            backendParserHelper.report_type = (int)reportType;
            backendParserHelper.CheckHistory();

            //bool is_data_of_record = backendParserHelper.is_data_of_record;
            bool DryRun = false;
            if (this.ConfigKeyExists("DryRun")) bool.TryParse(ConfigurationManager.AppSettings["DryRun"].ToString(), out DryRun);
            if (DryRun == true)
            {
                _logger.Debug("This is a dry run");
            }

            if (!backendParserHelper.Add_To_Database) // This is the latest file, there's no reason to parse this file
            {

                _logger.Information("Repeat file:" + "(" + NumPassed + "/" + TotalTo + ")" + fileName);
                if (backendParserHelper.is_data_of_record)
                {
                    _logger.Information($" {fileName} is the current data of record");

                }
                else
                {
                    _logger.Information($" {fileName} was previously entered, then superceeded by another file");

                }

                _logger.Information("-------------------------------------------------------------------------------");
            }
            // this is a new record - add it, superceed all others, and make it the data of record.

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();
                backendParserHelper.ReportHelperItem = new BackendParserReportHelperItem();

                try
                {
                    ParamHelper _param = new ParamHelper();

                    //Lab Report
                    byte[] labReport = null;
                    try
                    {
                        _logger.Information("Updating Report - ClientID:" + (reportDetails.ClientId + "").ToString() + " Donor:" + (reportDetails.DonorFirstName + " " + reportDetails.DonorLastName).ToString() + " File:" + fileName.ToString());
                    }
                    catch (Exception ex)
                    {
                        _logger.Information("Error Updating Report" + ex.StackTrace.ToString());
                    }
                    if (crlReport.ToString() != string.Empty)
                    {
                        labReport = Encoding.ASCII.GetBytes(crlReport.ToString());
                    }

                    // We gen a SH1 of the file
                    string lab_report_checksum = BitConverter.ToString(sha1.ComputeHash(labReport)).Replace("-", string.Empty);

                    // check the crlReport for the magic "empty" hash, and if so - make it unique.
                    if (lab_report_checksum.Equals("2993129fc9bc4da620cba0767792e2c10368fba8", StringComparison.InvariantCultureIgnoreCase))
                    {
                        // Add some meta data to the RTF report to ensure the file has unique content
                        _logger.Debug($"EMPTY RTF FOUND - {fileName} has checksum {lab_report_checksum}");
                        crlReport.AppendLine(string.Empty);
                        crlReport.AppendLine(string.Empty);
                        crlReport.AppendLine($"Internal MetaData:");
                        crlReport.AppendLine($"{reportDetails.SpecimenId} - {fileName}");
                        crlReport.AppendLine($"{Guid.NewGuid().ToString()}");
                        labReport = Encoding.ASCII.GetBytes(crlReport.ToString());
                        lab_report_checksum = BitConverter.ToString(sha1.ComputeHash(labReport)).Replace("-", string.Empty);
                    }

                    //Donor DOB
                    string donorDOB = string.Empty;
                    if (reportDetails.DonorDOB != null && reportDetails.DonorDOB != string.Empty)
                    {
                        donorDOB = DateTime.ParseExact(reportDetails.DonorDOB.Substring(0, 8), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                    }
                    // DonorTestInfo donorTestInfo = null;


                    // 20200327 - We need to add donors and donor test info records if we can.
                    //// We can only do this if we can add the donor
                    //// we can only add the donor if the PID is unused.
                    //// if it's not unused, we have a donor

                    try
                    {
                        // Should go with specimen ID first and foremost
                        // if it matches, we'll get everything, donor, lab code, ssn (or pid), donor details, client details, and test info
                        // in one query

                        // then, if we DON'T match, we can try and determine why

                        // if we have a match, and the pid doesn't match SSN, we can update that as a PID for that donor
                        // if we have a SSN, we could treat as an other type.

                        //Finding by SpecimenId

                        // We can check formfox orders to see if the specimen ID is in there.
                        // If it is, we can get everything
                        // We can check if formfox orders has the specimen ID, which we only get on a result.
                        // If get that ID, we populate return values on that ID
                        ParamGetformfoxorders p = new ParamGetformfoxorders();
                        p.formfoxorders.SpecimenID = reportDetails.SpecimenId;
                        _logger.Debug("Checking formfox orders for specimen id first");
                        formfoxorders _formfoxorders = data.GetFormFoxOrder(p);
                        if (_formfoxorders.backend_formfox_orders_id > 0)
                        {
                            // We have this specimen already, which means we have the donor test info
                            // we have it because it's a FormFox Order - was ordered through formfox
                            _logger.Debug($"This is a Formfox order!");
                            returnValues.DonorTestInfoId = _formfoxorders.donor_test_info_id;
                            GetSpecimenDetailsUsingFormfoxOrder(returnValues, reportDetails, trans, backendParserHelper);
                            _logger.Debug($"Formfox order identified! {_formfoxorders.backend_formfox_orders_id} donor test info: {_formfoxorders.donor_test_info_id} - ReportDetails DonorID {reportDetails.DonorId}");

                        }
                        else
                        {
                            _logger.Debug("No formfox order found, checking donor_test_info_test_categories in case it's an MRO report");
                            // We normally will NOT get this unless it's an MRO
                            GetSpecimenDetails(returnValues, reportDetails, trans, backendParserHelper);
                        }


                        //Finding the ClientId, ClientDepartmentId & ClientDeptTestPanelId
                        GetClientDetails(reportDetails, returnValues, trans, fileName, NumPassed, TotalTo, reportType);

                        // Determine what PID type we got from the file
                        SetPIDType(reportDetails, returnValues, ref backendParserHelper);


                        ProcessLabClientCode(reportDetails, obrList);

                        if (DryRun == false)
                        {
                            //Archive the report if it's the report we have is not the current report
                            // We only want to do this if we don't have a mismatch!
                            // if it's an archive, we can presume we don't have a mismatch
                            // but legacy data has mismatch data in the database
                            ArchiveReport(reportType, reportDetails, archiveExistingReport, trans);
                        }
                        else
                        {
                            _logger.Debug("Would Archive - this is a dry run");
                        }

                        //Verifying the SSN format
                        ValidateDonorSSn(reportDetails, returnValues);


                        //Finding the DonorId
                        // if we already have it, no need to make this call
                        _logger.Debug($"Donor Found Check: backendParserHelper.ReportHelperItem.DonorFound {backendParserHelper.ReportHelperItem.DonorFound}");

                        if (backendParserHelper.ReportHelperItem.DonorFound == false)
                        {
                            _logger.Debug("Donor Found is false, calling GetDonorDetails");
                            GetDonorDetails(reportDetails, returnValues, trans, ref backendParserHelper);
                            // If we don't have this donor, we can try and add them. 
                            // We can't add a donor if the PID is already in use, however

                        }
                        _logger.Debug($"Donor Found Result: backendParserHelper.ReportHelperItem.DonorFound {backendParserHelper.ReportHelperItem.DonorFound}");

                    }
                    catch (Exception ex)
                    {
                        _logger.Information($"Error Updating Report" + ex.StackTrace.ToString());
                        _logger.Information($"File: {fileName}");
                        throw;
                    }

                    // 20200327
                    // backendParserHelper.ReportHelperItem.SpecimenIDMatched

                    #region backfill
                    _logger.Debug($"Starting backfill tests and logic");
                    // let's backfill if we can
                    // first - do we have a donor? If not, and it's not because it's a multi match
                    // we can add the donor

                    // If this is a discard - maybe treat special? Throw into mismatch client
                    // If we don't have a client because of a labcode mismatch - backfill the lab code
                    // unless it's a discard - we'll leave those out of the equation






                    // Backfill - 

                    // we could backfill a donor if we have a client, labcode, and specimen id, but if we had a specimen id, the donor would register
                    // if have donor and match a lab code, could we backfill the test info? Probably

                    //////// Optionally - dump all mismatches into a mismatch client and department
                    //////For catch all Department, I was thinking of using a test category of MM(for mismatch).CAT_MM.Then a test panel call MISMATCH
                    //////Then, I’d use the insert the unknown lab code into client_departments for the MM client, using the unknown lab code, and create a record in client_dept_test_categories for the client department with the MM category.
                    //////Then, in client_dept_test_panels I’d insert new category id, the MISMATCH panel ID.
                    ///// if don't get them at this point - we need to go the above and set them on the returnValues.
                    //// Then we can allow the backfill of the donor if there's no match.
                    //// 
                    //// And we need to generate a new type of mismatch report: 
                    //// where it reports on dumps into the mismatch and why
                    /// Specimen ID: 2052506677 	Donor: FREEMAN^EBONEE (FOUND)		Collection Date: 202001030000 		Lab Code: 0VN.MPOS.CMAINLND (NOT FOUND)
                    ///      
                    ///

                    //////With that – we have enough info to backfill a donor



                    //if (returnValues.DonorId == 0
                    //    && returnValues.ClientId == 0
                    //    && returnValues.ClientDepartmentId == 0
                    //    && (returnValues.ClientDeptTestPanelId == 0 || returnValues.ClientDeptTestPanelId > 0)
                    //    && reportType == ReportType.LabReport && donorTestInfo.DonorTestInfoId == 0)
                    //{
                    //    // This is completely unknown
                    //    // if we reverse insert, we'll need to create test panels and everything.
                    //    AddDonorInfoWithTestDetailsWithUnknownPanel(reportDetails, returnValues, trans, reportType != ReportType.QuestLabReport); // only quest donors are created non-archived

                    //}





                    //For QUEST files - 
                    // We have a lab code - let's try and get the quest client by labcode
                    if (reportType == ReportType.QuestLabReport && returnValues.ClientId==0)
                    {
                        // Use reportDetails.OBRQuestCode to identify the client and dept.
                    }


                    // We add the donor and test info if the donor SSN (PID match doesn't happen) is not found
                    // Wo do it with ALL lab reports and we'll set the donor unverified flag (new) in the event it's NOT later, if we can't find the record
                    // A quest report. Unverified donors should be hidden from everything - almost like 'archived'

                    // of note - when it's parsing the OBR, it goes to get get quest lab code info so it knows how to back fill.
                    // it does this in GetOBRDetails in hl7parser, line 1010 as of this writing

                    _logger.Information("Testing for QUEST backfill");
                    _logger.Debug($"returnValues.DonorId {returnValues.DonorId}");
                    _logger.Debug($"backendParserHelper.ReportHelperItem.donor_pid_match_count {backendParserHelper.ReportHelperItem.donor_pid_match_count}");
                    _logger.Debug($"returnValues.ClientId {returnValues.ClientId}");
                    _logger.Debug($"returnValues.ClientDepartmentId {returnValues.ClientDepartmentId}");
                    _logger.Debug($"returnValues.ClientDeptTestPanelId {returnValues.ClientDeptTestPanelId}");
                    _logger.Debug($"returnValues.DonorTestInfoId {returnValues.DonorTestInfoId}");
                    _logger.Debug($"reportDetails.PIDType {reportDetails.PIDType}");

                    if (
                        returnValues.DonorId == 0 // No donor found
                        && backendParserHelper.ReportHelperItem.donor_pid_match_count < 1 // this is a unique pid, it's safe to add.
                        && returnValues.ClientId > 0 // We have a client
                        && returnValues.ClientDepartmentId > 0 // we have a department
                        && (returnValues.ClientDeptTestPanelId == 0 || returnValues.ClientDeptTestPanelId > 0) // would this ever be less than zero? Why this test.
                        && reportType == ReportType.QuestLabReport // this is a lab report, maybe results for someone like JCP, who sends people in without them being registered
                        && returnValues.DonorTestInfoId == 0 // we don't have a donor test info record, so the specimen ID is unknown
                        && reportDetails.PIDType != (int)PidTypes.Invalid // If we have NO PID, we don't add. This is very bad; but should be very rare.
                        )
                    {
                        _logger.Debug("Backfilling donor test info for QUEST");
                        _logger.Debug($"returnValues.DonorId {returnValues.DonorId}");
                        _logger.Debug($"backendParserHelper.ReportHelperItem.donor_pid_match_count {backendParserHelper.ReportHelperItem.donor_pid_match_count}");
                        _logger.Debug($"returnValues.ClientId {returnValues.ClientId}");
                        _logger.Debug($"returnValues.ClientDepartmentId {returnValues.ClientDepartmentId}");
                        _logger.Debug($"returnValues.ClientDeptTestPanelId {returnValues.ClientDeptTestPanelId}");
                        _logger.Debug($"reportType {reportType}");
                        _logger.Debug($"returnValues.DonorTestInfoId {returnValues.DonorTestInfoId}");
                        _logger.Debug($"returnValues.PIDType invalid? {reportDetails.PIDType != (int)PidTypes.Invalid}");

                        bool ReverseEnterAllLabs = false;
                        if (this.ConfigKeyExists("CreateAllMismatchDonors")) bool.TryParse(ConfigurationManager.AppSettings["CreateAllMismatchDonors"].ToString(), out ReverseEnterAllLabs);

                        if (ReverseEnterAllLabs || reportType == ReportType.QuestLabReport)
                        {
                            if (ConfigurationManager.AppSettings["IsDonorReverseEntryWithTestInfo"].ToString().Trim().ToUpper() == "TRUE")
                            {
                                _logger.Debug($"This is a quest lab report and IsDonorReverseEntryWithTestInfo is set to true, backfilling");

                                backendParserHelper.ReportHelperItem.donor_backfilled = true;

                                if (DryRun == false)
                                {
                                    //AddDonorInfoWithTestDetails(reportDetails, returnValues, trans, reportType != ReportType.QuestLabReport); // only quest donors are created non-archived 
                                    AddDonorInfoWithTestDetails(reportDetails, returnValues, trans, false); // only quest donors are created non-archived 

                                }
                                else
                                {
                                    _logger.Debug("Would backfill donor for quest - this is a dry run");
                                }
                            }
                        }

                    }
                    else
                    {
                        _logger.Information("Testing for QUEST backfill negative");
                    }

                    string sqlQuery = "";
                    //MySqlParameter[] param = null;

                    //sqlQuery = "SELECT "
                    //        + "donor_test_info.donor_test_info_id AS DonorTestInfoId, "
                    //        + "donor_test_info.mro_type_id AS MROTypeId "
                    //        + "FROM donor_test_info "
                    //        + "INNER JOIN donor_test_info_test_categories ON donor_test_info_test_categories.donor_test_info_id = donor_test_info.donor_test_info_id "
                    //        + "WHERE donor_test_info_test_categories.specimen_id = @SpecimenId";

                    // need to pull integration partner
                    sqlQuery = @"
                        SELECT 
                        donor_test_info.donor_test_info_id AS DonorTestInfoId,
                        donor_test_info.mro_type_id AS MROTypeId,
                        clients.client_name as client_name,
                        client_departments.department_name as department_name,
                        IF(bipm.backend_integration_partner_client_map_id is null, 0,1) as IntegrationPartner
                        FROM donor_test_info 
                        INNER JOIN donor_test_info_test_categories ON donor_test_info_test_categories.donor_test_info_id = donor_test_info.donor_test_info_id 
                        left outer join clients on clients.client_id = donor_test_info.client_id
                        left outer join client_departments on client_departments.client_department_id = donor_test_info.client_department_id
                        left outer join backend_integration_partner_client_map bipm on clients.client_id = bipm.client_id and client_departments.client_department_id = bipm.client_department_id and bipm.active > 0
                        WHERE donor_test_info_test_categories.specimen_id = @SpecimenId;
";

                    //_logger.Information("Query1:" + sqlQuery);
                    _param.reset();
                    _param.Param = new MySqlParameter("@SpecimenId", reportDetails.SpecimenId);

                    MySqlDataReader dr = SqlHelper.ExecuteReader(trans, CommandType.Text, sqlQuery, _param.Params);

                    if (dr.Read())
                    {
                        returnValues.DonorTestInfoId = Convert.ToInt32(dr["DonorTestInfoId"]);
                        returnValues.MROType = dr["MROTypeId"] != DBNull.Value ? (ClientMROTypes)(Convert.ToInt32(dr["MROTypeId"].ToString())) : ClientMROTypes.None;
                        reportDetails.ClientName = dr["client_name"].ToString();
                        reportDetails.ClientDepartmentName = dr["department_name"].ToString();
                        returnValues.IntegrationPartner = Convert.ToInt32(dr["IntegrationPartner"]) > 0;
                    }

                    dr.Close();
                    _logger.Information("Specimenid ID:" + reportDetails.SpecimenId);
                    //Applying intelligence to find the test info
                    // THIS REALLY needs be refactored.
                    // This should get the values, then update the categories at the insert below.
                    // doing this now is a bad idea
                    // TODO
                    if (reportType == ReportType.LabReport || reportType == ReportType.QuestLabReport)
                    {
                        if (returnValues.DonorTestInfoId == 0)
                        {
                            _logger.Debug("Trying to find by 'Intelligence' function");
                            if (ConfigurationManager.AppSettings["IsAutoMatchTestInfo"].ToString().Trim().ToUpper() == "TRUE")
                            {
                                DoTestInfoIntelligence(reportDetails, returnValues, trans);
                                // FindDonorTestInfoIdByIntelligence
                            }

                        }
                        else
                        {
                            _logger.Debug("Calling SetDonorTestInfoIdIntelligence");

                            // if we have the donor test info id and it's a lab or quest report, we need to update the categories.
                            if (ConfigurationManager.AppSettings["IsAutoMatchTestInfo"].ToString().Trim().ToUpper() == "TRUE")
                            {
                                SetDonorTestInfoIdIntelligence(reportDetails, returnValues, trans);
                                // FindDonorTestInfoIdByIntelligence
                            }
                        }


                    }
                    else
                    {
                        _logger.Debug("ReportType is neither Lab or Quest type");
                    }


                    _logger.Debug("Testing for backfilling donor for everything except a test record (IsDonorReverseEntryWithTestInfo flag)");
                    _logger.Debug($"returnValues.DonorId {returnValues.DonorId}");
                    _logger.Debug($"backendParserHelper.ReportHelperItem.donor_pid_match_count {backendParserHelper.ReportHelperItem.donor_pid_match_count}");
                    _logger.Debug($"returnValues.ClientId {returnValues.ClientId}");
                    _logger.Debug($"returnValues.ClientDepartmentId {returnValues.ClientDepartmentId}");
                    _logger.Debug($"returnValues.ClientDeptTestPanelId {returnValues.ClientDeptTestPanelId}");
                    _logger.Debug($"reportType {reportType}");
                    _logger.Debug($"returnValues.DonorTestInfoId {returnValues.DonorTestInfoId}");
                    _logger.Debug($"returnValues.PIDType invalid? {reportDetails.PIDType != (int)PidTypes.Invalid}");
                    // If we have everything but a test record at this point - we try and backfill.
                    if (
                        // If we have a donor, and we have a client, and we have a lab code- we can backfill the donor_test_info request
                        // AddTestInfoDetails(reportDetails, returnValues, trans);

                        returnValues.DonorId > 0 // donor found
                        && backendParserHelper.ReportHelperItem.donor_pid_match_count < 1 // this is a unique pid, it's safe to add.
                        && returnValues.ClientId > 0 // We have a client
                        && returnValues.ClientDepartmentId > 0 // we have a department
                        && (returnValues.ClientDeptTestPanelId == 0 || returnValues.ClientDeptTestPanelId > 0) // would this ever be less than zero? Why this test.
                        && reportType == ReportType.LabReport // this is a lab report, maybe results for someone like JCP, who sends people in without them being registered
                        && returnValues.DonorTestInfoId == 0 // we don't have a donor test info record, so the specimen ID is unknown
                        && reportDetails.PIDType != (int)PidTypes.Invalid // If we have NO PID, we don't add. This is very bad; but should be very rare.
                         )
                    {

                        _logger.Debug("Backfilling donor for everything except a test record (IsDonorReverseEntryWithTestInfo flag)");
                        _logger.Debug($"returnValues.DonorId {returnValues.DonorId}");
                        _logger.Debug($"backendParserHelper.ReportHelperItem.donor_pid_match_count {backendParserHelper.ReportHelperItem.donor_pid_match_count}");
                        _logger.Debug($"returnValues.ClientId {returnValues.ClientId}");
                        _logger.Debug($"returnValues.ClientDepartmentId {returnValues.ClientDepartmentId}");
                        _logger.Debug($"returnValues.ClientDeptTestPanelId {returnValues.ClientDeptTestPanelId}");
                        _logger.Debug($"reportType {reportType}");
                        _logger.Debug($"returnValues.DonorTestInfoId {returnValues.DonorTestInfoId}");
                        _logger.Debug($"returnValues.PIDType invalid? {reportDetails.PIDType != (int)PidTypes.Invalid}");


                        if (ConfigurationManager.AppSettings["IsDonorReverseEntryWithTestInfo"].ToString().Trim().ToUpper() == "TRUE")
                        {
                            _logger.Debug("IsDonorReverseEntryWithTestInfo is true, executing");
                            AddTestInfoDetails(reportDetails, returnValues, trans);

                        }
                        else
                        {
                            _logger.Debug("IsDonorReverseEntryWithTestInfo is false, skipping");
                        }
                    }
                    else
                    {
                        _logger.Information("Testing for backfilling donor for everything except a test record skipped");
                    }
                    // Here, we backfill everything if we don't have it and it's not a quest report (which automatically backfill)
                    _logger.Information("Testing for non-quest backfill");
                    _logger.Debug($"returnValues.DonorId {returnValues.DonorId}");
                    _logger.Debug($"backendParserHelper.ReportHelperItem.donor_pid_match_count {backendParserHelper.ReportHelperItem.donor_pid_match_count}");
                    _logger.Debug($"returnValues.ClientId {returnValues.ClientId}");
                    _logger.Debug($"returnValues.ClientDepartmentId {returnValues.ClientDepartmentId}");
                    _logger.Debug($"returnValues.ClientDeptTestPanelId {returnValues.ClientDeptTestPanelId}");
                    _logger.Debug($"returnValues.DonorTestInfoId {returnValues.DonorTestInfoId}");
                    _logger.Debug($"reportDetails.PIDType {reportDetails.PIDType}");
                    if (
                       returnValues.DonorId == 0 // No donor found
                       && backendParserHelper.ReportHelperItem.donor_pid_match_count < 1 // this is a unique pid, it's safe to add.
                       && returnValues.ClientId > 0 // We have a client
                       && returnValues.ClientDepartmentId > 0 // we have a department
                       && (returnValues.ClientDeptTestPanelId == 0 || returnValues.ClientDeptTestPanelId > 0) // would this ever be less than zero? Why this test.
                       && reportType == ReportType.LabReport // this is a lab report, maybe results for someone like JCP, who sends people in without them being registered
                       && reportType != ReportType.QuestLabReport // this is a lab report, maybe results for someone like JCP, who sends people in without them being registered
                       && returnValues.DonorTestInfoId == 0 // we don't have a donor test info record, so the specimen ID is unknown
                       && reportDetails.PIDType != (int)PidTypes.Invalid // If we have NO PID, we don't add. This is very bad; but should be very rare.
                       && returnValues.TestStatus == DonorRegistrationStatus.None // we may have a completed test already, in which case we do NOT want to create a new donor test info record
                       )
                    {
                        _logger.Debug("Backfilling for everything");
                        _logger.Debug($"returnValues.DonorId {returnValues.DonorId}");
                        _logger.Debug($"backendParserHelper.ReportHelperItem.donor_pid_match_count {backendParserHelper.ReportHelperItem.donor_pid_match_count}");
                        _logger.Debug($"returnValues.ClientId {returnValues.ClientId}");
                        _logger.Debug($"returnValues.ClientDepartmentId {returnValues.ClientDepartmentId}");
                        _logger.Debug($"returnValues.ClientDeptTestPanelId {returnValues.ClientDeptTestPanelId}");
                        _logger.Debug($"reportType {reportType}");
                        _logger.Debug($"returnValues.DonorTestInfoId {returnValues.DonorTestInfoId}");
                        _logger.Debug($"returnValues.PIDType invalid? {reportDetails.PIDType != (int)PidTypes.Invalid}");


                        bool ReverseEnterAllLabs = false;
                        if (this.ConfigKeyExists("CreateAllMismatchDonors")) bool.TryParse(ConfigurationManager.AppSettings["CreateAllMismatchDonors"].ToString(), out ReverseEnterAllLabs);

                        if (ReverseEnterAllLabs)
                        {
                            if (ConfigurationManager.AppSettings["IsDonorReverseEntryWithTestInfo"].ToString().Trim().ToUpper() == "TRUE")
                            {
                                backendParserHelper.ReportHelperItem.donor_backfilled = true;

                                if (DryRun == false)
                                {
                                    AddDonorInfoWithTestDetails(reportDetails, returnValues, trans, false);

                                }
                                else
                                {
                                    _logger.Debug("Would backfill donor for lab report - this is a dry run");
                                }
                            }
                        }

                    }
                    else
                    {
                        _logger.Information("Non-quest backfill skipped");
                    }
                    _logger.Debug($"End of backfill testing a logic");
                    #endregion backfill


                    //_logger.Debug("Last ditch effort to get test info record");

                    //// If this is a lab report, and we know the client, dept, and donor - and their PID was unique (only 1 match) and there is only one donor test info record for that donor
                    //// then we can safely use it.
                    //if (
                    //    // If we have a donor, and we have a client, and we have a lab code- we can backfill the donor_test_info request
                    //    // AddTestInfoDetails(reportDetails, returnValues, trans);
                    //    (reportType == ReportType.LabReport || reportType == ReportType.QuestLabReport)
                    //    && returnValues.DonorId > 0 // donor found
                    //    && backendParserHelper.ReportHelperItem.donor_pid_match_count == 1 // this pid was only found for one donor
                    //    && returnValues.ClientId > 0 // We have a client
                    //    && returnValues.ClientDepartmentId > 0 // we have a department
                    //    && (returnValues.ClientDeptTestPanelId == 0 || returnValues.ClientDeptTestPanelId > 0) // would this ever be less than zero? Why this test.
                    //    && returnValues.DonorTestInfoId == 0 // we don't have a donor test info record for some reason
                    //    && reportDetails.PIDType != (int)PidTypes.Invalid // If we have NO PID, we don't add. This is very bad; but should be very rare.
                    //     )
                    //{

                    //}



                    ////Adding test info in case of donor available
                    //if (returnValues.DonorTestInfoId == 0
                    //    && returnValues.DonorId > 0
                    //    && returnValues.ClientId > 0
                    //    && returnValues.ClientDepartmentId > 0
                    //    && returnValues.ClientDeptTestPanelId > 0
                    //    && returnValues.TestInfoRecordCount == 0
                    //    && (reportType == ReportType.LabReport || reportType == ReportType.QuestLabReport))
                    //{
                    //    if (ConfigurationManager.AppSettings["IsTestInfoReverseEntry"].ToString().Trim().ToUpper() == "TRUE")
                    //    {
                    //        if (DryRun == false)
                    //        {
                    //            AddTestInfoDetails(reportDetails, returnValues, trans);
                    //        }
                    //        else
                    //        {
                    //            _logger.Debug("Would add test info details - this is a dry run");
                    //        }
                    //    }
                    //}

                    /// Capture details of trying to match:

                    backendParserHelper.ReportHelperItem.SpecimenIDFound = String.IsNullOrEmpty(reportDetails.SpecimenId) == false;
                    backendParserHelper.ReportHelperItem.CollectionDateFound = String.IsNullOrEmpty(reportDetails.SpecimenCollectionDate) == false;
                    backendParserHelper.ReportHelperItem.LabCodeFound = reportDetails.ClientDepartmentId > 0;
                    backendParserHelper.ReportHelperItem.PanelFound = String.IsNullOrEmpty(reportDetails.TestPanelCode) == false;

                    backendParserHelper.ReportHelperItem.FileName = fileName;
                    backendParserHelper.ReportHelperItem.ReportTypeEnum = reportType;
                    backendParserHelper.ReportHelperItem.SpecimenID = (string.IsNullOrEmpty(reportDetails.SpecimenId)) ? string.Empty : reportDetails.SpecimenId;
                    backendParserHelper.ReportHelperItem.Donor = $"{reportDetails.DonorFirstName} {reportDetails.DonorMI} {reportDetails.DonorLastName}";
                    backendParserHelper.ReportHelperItem.donor_id = returnValues.DonorId.ToString();
                    backendParserHelper.ReportHelperItem.DonorFound = returnValues.DonorId > 0;
                    backendParserHelper.ReportHelperItem.CollectionDate = (string.IsNullOrEmpty(reportDetails.SpecimenCollectionDate)) ? string.Empty : reportDetails.SpecimenCollectionDate;
                    backendParserHelper.ReportHelperItem.LabCode = (string.IsNullOrEmpty(reportDetails.LabCode)) ? string.Empty : reportDetails.LabCode;
                    backendParserHelper.ReportHelperItem.CrlClientCode = (string.IsNullOrEmpty(reportDetails.CrlClientCode)) ? string.Empty : reportDetails.CrlClientCode;
                    if (backendParserHelper.ReportHelperItem.ReportTypeEnum == ReportType.QuestLabReport)
                    {
                        backendParserHelper.ReportHelperItem.CrlClientCode = (string.IsNullOrEmpty(reportDetails.QuestCode)) ? string.Empty : reportDetails.QuestCode;
                    }
                    backendParserHelper.ReportHelperItem.Panel = (string.IsNullOrEmpty(reportDetails.TestPanelCode)) ? string.Empty : reportDetails.TestPanelCode;
                    backendParserHelper.ReportHelperItem.Client = (string.IsNullOrEmpty(reportDetails.ClientName)) ? string.Empty : reportDetails.ClientName;
                    backendParserHelper.ReportHelperItem.ClientDepartment = (string.IsNullOrEmpty(reportDetails.ClientDepartmentName)) ? string.Empty : reportDetails.ClientDepartmentName;

                    backendParserHelper.ReportHelperItem.client_department_id = reportDetails.ClientDepartmentId;
                    backendParserHelper.ReportHelperItem.client_id = reportDetails.ClientId;
                    backendParserHelper.ReportHelperItem.donor_test_info_id = returnValues.DonorTestInfoId;








                    OverAllTestResult overAllResult = OverAllTestResult.None;

                    // At this point - either we have match or we don't. let's only insert on a match
                    // nothing can be done with mismatched data

                    // if this is a labreport, and it's completed, we should not do anything
                    // Just consider the lab report processed (per conversation with David 4/12/21
                    // A completed test can ONLY be updated by an MRO report.

                    _logger.Debug($"backendParserHelper.Add_To_Database {backendParserHelper.Add_To_Database}");

                    if (returnValues.DonorTestInfoId > 0)
                    {
                        if (DryRun == false)
                        {
                            //Finding the Overall Result

                            // Possible bug - if we get a non-quest lab report and all OBX records are negative, it leaves this test as NONE result
                            // We need to test all OBX for negative, and if all OBX are negative, then this result is negative

                            //int i = 0;
                            bool AllNegative = true;
                            _logger.Information("Reading OBR Info");
                            foreach (OBR_Info obr in obrList)
                            {

                                if (obr.SectionHeader.ToUpper().Contains("CONFIRMATION") || reportType == ReportType.QuestLabReport)
                                {
                                    //List<string> y = new List<string>();



                                    //if (obr.observatinos.Where(x => !(string.IsNullOrEmpty(x.Status) && ( x.Status.ToUpper().Contains("POSITIVE") || x.Status.ToUpper().Contains("POS") ))).ToList().Count() >0)
                                    //{
                                    //    overAllResult = OverAllTestResult.Positive;
                                    //    _logger.Information(obx.ToString());
                                    //    break;
                                    //}

                                    foreach (OBX_Info obx in obr.observatinos)
                                    {
                                        try
                                        {
                                            if (!string.IsNullOrEmpty(obx.Status))
                                            {
                                                overAllResult = DetermineOverallResult(obx.Status);
                                                if (overAllResult == OverAllTestResult.Positive) break;
                                                // We have bool flag for all negatives. as long as it's true, and the result is negative
                                                // we leave it as true. If we get anything other than negative
                                                // we set it to false. After testing, if AllNegative is still true, the test was negative
                                                if (AllNegative == true && !(overAllResult == OverAllTestResult.Negative)) AllNegative = false;
                                            }

                                        }
                                        catch (Exception ex)
                                        {
                                            var exception = ex.StackTrace.ToString();
                                        }
                                        //var blah = 1;
                                    }
                                }
                                else if (obr.SectionHeader.ToUpper().Contains("INITIAL TEST") || obr.SectionHeader.ToUpper().Contains("SUBSTANCE ABUSE PANEL"))
                                {
                                    foreach (OBX_Info obx in obr.observatinos)
                                    {
                                        overAllResult = DetermineOverallResult(obx.Status);
                                        if (overAllResult == OverAllTestResult.Positive) break;
                                        if (AllNegative == true && !(overAllResult == OverAllTestResult.Negative)) AllNegative = false;
                                        //i++;
                                    }
                                }
                                else if (reportType == ReportType.MROReport)
                                {
                                    try
                                    {
                                        foreach (OBX_Info obx in obr.observatinos)
                                        {
                                            overAllResult = DetermineOverallResult(obx.Status);
                                            if (overAllResult == OverAllTestResult.Positive) break;
                                            if (AllNegative == true && !(overAllResult == OverAllTestResult.Negative)) AllNegative = false;
                                            //i++;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.Information("Error: HL7Parser OBX Status" + ex.Message + ex.StackTrace.ToString());
                                        //throw;
                                    }
                                }
                            }

                            // if all negative and we didn't find a positive, it's a negative, regardless of type of report.
                            if (AllNegative == true && overAllResult == OverAllTestResult.None)
                            {
                                // every OBX in this lab report is negative, the test is negative

                                //overAllResult = OverAllTestResult.Negative;
                                _logger.Information($"Lab report all negative, setting overallresult to negative");
                            }


                            //Finding MPOS & MALL
                            if (reportType == ReportType.LabReport || reportType == ReportType.QuestLabReport)
                            {
                                if (returnValues.DonorTestInfoId > 0)
                                {
                                    if ((returnValues.MROType == ClientMROTypes.MALL) || (returnValues.MROType == ClientMROTypes.MPOS && overAllResult == OverAllTestResult.Positive))
                                    {
                                        _logger.Debug($"Setting MroAttentionFlag to true");
                                        returnValues.MroAttentionFlag = true;
                                    }
                                }
                            }
                            //if (!IsCurrentLabReportFile)
                            //{
                            sqlQuery = "INSERT INTO report_info ("
                                            + "report_type, "
                                            + "specimen_id, "
                                            + "lab_sample_id, "
                                            + "ssn_id, "
                                            + "donor_last_name, "
                                            + "donor_first_name, "
                                            + "donor_mi, "
                                            + "donor_dob, "
                                            + "donor_gender, "
                                            + "lab_report, "
                                            + "lab_report_checksum, "
                                            + "data_checksum, "
                                            + "lab_report_source_filename, "
                                            + "screening_time, "

                                            + "lab_name, "
                                            + "lab_code, "
                                            + "lab_report_date, "
                                            + "donor_driving_license, "
                                            + "test_panel_code, "
                                            + "test_panel_name, "

                                            + "donor_test_info_id, "
                                            + "report_status, "
                                            + "is_synchronized, "
                                            + "is_archived, "
                                            + "created_on, "
                                            + "created_by, "
                                            + "last_modified_on, "
                                            + "last_modified_by) VALUES ("
                                            + "@ReportType, "
                                            + "@SpecimenId, "
                                            + "@LabSampleId, "
                                            + "@SsnId, "
                                            + "@DonorLastName, "
                                            + "@DonorFirstName, "
                                            + "@DonorMI, "
                                            + "@DonorDOB, "
                                            + "@DonorGender, "
                                            + "@LabReport, "
                                            + "@lab_report_checksum, "
                                            + "@data_checksum, "
                                            + "@lab_report_source_filename, "
                                            + "@screening_time, "

                                            + "@LabName, "
                                            + "@LabCode, "
                                            + "@LabReportDate, "
                                            + "@DonorDrivingLicense, "
                                            + "@TestPanelCode, "
                                            + "@TestPanelName, "

                                            + "@DonorTestInfoId, "
                                            + "@ReportStatus, "
                                            + "b'0', b'0', NOW(), 'SYSTEM', NOW(), 'SYSTEM')";

                            _param.reset();
                            _param.Param = new MySqlParameter("@ReportType", (int)reportType);
                            _param.Param = new MySqlParameter("@SpecimenId", reportDetails.SpecimenId);
                            _param.Param = new MySqlParameter("@LabSampleId", reportDetails.LabSampleId);
                            _param.Param = new MySqlParameter("@SsnId", reportDetails.SsnId);
                            _param.Param = new MySqlParameter("@DonorLastName", reportDetails.DonorLastName);
                            _param.Param = new MySqlParameter("@DonorFirstName", reportDetails.DonorFirstName);
                            _param.Param = new MySqlParameter("@DonorMI", reportDetails.DonorMI);
                            _param.Param = new MySqlParameter("@DonorDOB", donorDOB);
                            _param.Param = new MySqlParameter("@DonorGender", reportDetails.DonorGender);
                            _param.Param = new MySqlParameter("@LabReport", labReport);
                            _param.Param = new MySqlParameter("@lab_report_checksum", lab_report_checksum);
                            _param.Param = new MySqlParameter("@data_checksum", reportDetails.data_checksum);
                            _param.Param = new MySqlParameter("@lab_report_source_filename", reportDetails.lab_report_source_filename);

                            DateTime _SpecimenCollectionDateTime;
                            if (!DateTime.TryParseExact(reportDetails.SpecimenCollectionDate, "yyyyMMddHHmm", CultureInfo.InvariantCulture, DateTimeStyles.None, out _SpecimenCollectionDateTime))
                            {
                                _param.Param = new MySqlParameter("@screening_time", null);
                            }
                            else
                            {
                                _param.Param = new MySqlParameter("@screening_time", _SpecimenCollectionDateTime);
                            }

                            _param.Param = new MySqlParameter("@LabName", reportDetails.LabName);
                            _param.Param = new MySqlParameter("@LabCode", reportDetails.LabCode);
                            _param.Param = new MySqlParameter("@LabReportDate", reportDetails.LabReportDate);
                            _param.Param = new MySqlParameter("@DonorDrivingLicense", reportDetails.PID_NODASHES_4);
                            _param.Param = new MySqlParameter("@TestPanelCode", reportDetails.TestPanelCode);
                            _param.Param = new MySqlParameter("@TestPanelName", reportDetails.TestPanelName);

                            _param.Param = new MySqlParameter("@DonorTestInfoId", returnValues.DonorTestInfoId);
                            _param.Param = new MySqlParameter("@ReportStatus", (int)overAllResult);

                            if (backendParserHelper.Add_To_Database)
                            {
                                _logger.Debug($"Inserting Report file");

                                SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, _param.Params);

                                sqlQuery = "SELECT LAST_INSERT_ID()";
                                //_logger.Information("Query2:" + sqlQuery);
                                returnValues.ReportId = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlQuery));
                                _logger.Debug($"Inserted Report file {returnValues.ReportId}");

                            }
                            else
                            {
                                // we've alrady processed this file, no need to insert it gain
                                returnValues.ReportId = backendParserHelper.report_info_id;
                            }


                            if (backendParserHelper.Add_To_Database) // we only add the obr obx info on a new file. The file contents don't change & and none of this passed out except report_info_id (key)
                            {

                                _logger.Debug("Ensuring Test Categories are set (Backfill sanity check");
                                if (reportType == ReportType.LabReport || reportType == ReportType.QuestLabReport)
                                {

                                    _logger.Debug("Calling SetDonorTestInfoIdIntelligence");

                                    // if we have the donor test info id and it's a lab or quest report, we need to update the categories.
                                    if (ConfigurationManager.AppSettings["IsAutoMatchTestInfo"].ToString().Trim().ToUpper() == "TRUE")
                                    {
                                        SetDonorTestInfoIdIntelligence(reportDetails, returnValues, trans);
                                        // FindDonorTestInfoIdByIntelligence
                                    }


                                }
                                else
                                {
                                    _logger.Debug("ReportType is neither Lab or Quest type");
                                }



                                _logger.Information("Inserting Report Info ReportID:" + returnValues.ReportId.ToString());
                                _logger.Information("Reading OBR Info");
                                foreach (OBR_Info obr in obrList)
                                {
                                    string specimenCollectionDate = string.Empty;
                                    if (obr.SpecimenCollectionDate != null && obr.SpecimenCollectionDate != string.Empty)
                                    {
                                        specimenCollectionDate = DateTime.ParseExact(obr.SpecimenCollectionDate.Substring(0, 8), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                                    }

                                    string specimenReceivedDate = string.Empty;
                                    if (obr.SpecimenReceivedDate != null && obr.SpecimenReceivedDate != string.Empty)
                                    {
                                        specimenReceivedDate = DateTime.ParseExact(obr.SpecimenReceivedDate.Substring(0, 8), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                                    }

                                    string crlTransmitDate = string.Empty;
                                    if (obr.CrlTransmitDate != null && obr.CrlTransmitDate != string.Empty)
                                    {
                                        crlTransmitDate = DateTime.ParseExact(obr.CrlTransmitDate.Substring(0, 8), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                                    }

                                    sqlQuery = "INSERT INTO obr_info ("
                                                                    + "report_info_id, "
                                                                    + "transmited_order, "
                                                                    + "collection_site_info, "
                                                                    + "specimen_collection_date, "
                                                                    + "specimen_received_date, "
                                                                    + "crl_client_code, "
                                                                    + "specimen_type, "
                                                                    + "section_header, "
                                                                    + "crl_transmit_date, "
                                                                    + "service_section_id, "
                                                                    + "order_status, "
                                                                    + "reason_type, "

                                                                    + "collection_site_id, "
                                                                    + "specimen_action_code, "
                                                                    + "tpa_code, "
                                                                    + "region_code, "
                                                                    + "client_code, "
                                                                    + "deaprtment_code, "

                                                                    + "is_synchronized, "
                                                                    + "created_on, "
                                                                    + "created_by, "
                                                                    + "last_modified_on, "
                                                                    + "last_modified_by) VALUES ("
                                                                    + "@ReportInfoId, "
                                                                    + "@TransmitedOrder, "
                                                                    + "@CollectionSiteInfo, "
                                                                    + "@SpecimenCollectionDate, "
                                                                    + "@SpecimenReceivedDate, "
                                                                    + "@CrlClientCode, "
                                                                    + "@SpecimenType, "
                                                                    + "@SectionHeader, "
                                                                    + "@CrlTransmitDate, "
                                                                    + "@ServiceSectionId, "
                                                                    + "@OrderStatus, "
                                                                    + "@ReasonType, "

                                                                    + "@CollectionSiteId, "
                                                                    + "@SpecimenActionCode, "
                                                                    + "@TpaCode, "
                                                                    + "@RegionCode, "
                                                                    + "@ClientCode, "
                                                                    + "@DepartmentCode, "

                                                                    + "b'0', NOW(), 'SYSTEM', NOW(), 'SYSTEM')";
                                    _param.reset();
                                    //param = new MySqlParameter[18];

                                    _param.Param = new MySqlParameter("@ReportInfoId", returnValues.ReportId);
                                    _param.Param = new MySqlParameter("@TransmitedOrder", obr.TransmitedOrder);
                                    _param.Param = new MySqlParameter("@CollectionSiteInfo", obr.CollectionSiteInfo);
                                    _param.Param = new MySqlParameter("@SpecimenCollectionDate", specimenCollectionDate);
                                    _param.Param = new MySqlParameter("@SpecimenReceivedDate", specimenReceivedDate);
                                    _param.Param = new MySqlParameter("@CrlClientCode", obr.CrlClientCode);
                                    _param.Param = new MySqlParameter("@SpecimenType", obr.SpecimenType);
                                    _param.Param = new MySqlParameter("@SectionHeader", obr.SectionHeader);
                                    _param.Param = new MySqlParameter("@CrlTransmitDate", crlTransmitDate);
                                    _param.Param = new MySqlParameter("@ServiceSectionId", obr.ServiceSectionId);
                                    _param.Param = new MySqlParameter("@OrderStatus", obr.OrderStatus);
                                    _param.Param = new MySqlParameter("@ReasonType", obr.ReasonType);

                                    _param.Param = new MySqlParameter("@CollectionSiteId", obr.CollectionSiteId);
                                    _param.Param = new MySqlParameter("@SpecimenActionCode", obr.SpecimenActionCode);
                                    _param.Param = new MySqlParameter("@TpaCode", obr.TpaCode);
                                    _param.Param = new MySqlParameter("@RegionCode", obr.RegionCode);
                                    _param.Param = new MySqlParameter("@ClientCode", obr.ClientCode);
                                    _param.Param = new MySqlParameter("@DepartmentCode", obr.DepartmentCode);

                                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, _param.Params);

                                    sqlQuery = "SELECT LAST_INSERT_ID()";
                                    //_logger.Information("Query3:" + sqlQuery);
                                    _logger.Information("Inserting ObrInfo");

                                    int obrId = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlQuery));
                                    _logger.Information("Reading OBX Info");
                                    foreach (OBX_Info obx in obr.observatinos)
                                    {
                                        sqlQuery = "INSERT INTO OBX_Info ("
                                                                        + "obr_info_id, "
                                                                        + "sequence, "
                                                                        + "test_code, "
                                                                        + "test_name, "
                                                                        + "result, "
                                                                        + "status, "
                                                                        + "unit_of_measure, "
                                                                        + "reference_range, "
                                                                        + "order_status, "
                                                                        + "is_synchronized, "
                                                                        + "created_on, "
                                                                        + "created_by, "
                                                                        + "last_modified_on, "
                                                                        + "last_modified_by) VALUES ("
                                                                        + "@OBRInfoId, "
                                                                        + "@Sequence, "
                                                                        + "@TestCode, "
                                                                        + "@TestName, "
                                                                        + "@Result, "
                                                                        + "@Status, "
                                                                        + "@UnitOfMeasure, "
                                                                        + "@ReferenceRange, "
                                                                        + "@OrderStatus, "
                                                                        + "b'0', NOW(), 'SYSTEM', NOW(), 'SYSTEM')";

                                        _param.reset();

                                        _param.Param = new MySqlParameter("@OBRInfoId", obrId);
                                        _param.Param = new MySqlParameter("@Sequence", obx.Sequence);
                                        _param.Param = new MySqlParameter("@TestCode", obx.TestCode);
                                        _param.Param = new MySqlParameter("@TestName", obx.TestName);
                                        _param.Param = new MySqlParameter("@Result", obx.Result);
                                        _param.Param = new MySqlParameter("@Status", obx.Status);
                                        _param.Param = new MySqlParameter("@UnitOfMeasure", obx.UnitOfMeasure);
                                        _param.Param = new MySqlParameter("@ReferenceRange", obx.ReferenceRange);
                                        _param.Param = new MySqlParameter("@OrderStatus", obx.OrderStatus);

                                        //_logger.Information("Query4:" + sqlQuery);
                                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, _param.Params);

                                    }

                                }
                            } //if (backendParserHelper.Add_To_Database) // we only add the obr obx info on a new file. The file contents don't change & and none of this passed out except report_info_id (key)


                            //}
                            //else
                            //{
                            //    // Just a note that we set ReportId already
                            //    // do we need to do anything else
                            //    _logger.Information($"Inserts skipped, this is current record_info recordset in database. All data already in system");
                            //}


                            OverAllTestResult testInfoResult = OverAllTestResult.None;
                            DonorRegistrationStatus testInfoStatus = DonorRegistrationStatus.Processing;


                            // BUG - If we have a "no contact" positive, then get an MRO report after the fact
                            // We should update the results.

                            //if (overAllResult == OverAllTestResult.Negative)
                            //{
                            //    testInfoResult = OverAllTestResult.Negative;
                            //    testInfoStatus = DonorRegistrationStatus.Completed;
                            //}

                            // if this donor didn't exist before - we may still need to add this data

                            if (overAllResult == OverAllTestResult.Negative)
                            {
                                // A negative is a negative - there's no reason to check a negative against the MRO type

                                testInfoResult = OverAllTestResult.Negative;
                                testInfoStatus = DonorRegistrationStatus.Completed;
                            }
                            else if (overAllResult == OverAllTestResult.Positive)
                            {
                                if (reportType == ReportType.MROReport)
                                {
                                    testInfoResult = OverAllTestResult.Positive;
                                    testInfoStatus = DonorRegistrationStatus.Completed;
                                }
                                else
                                {
                                    // this is a positive and not an MRO, thus
                                    // it doesn't matter if it's an MPOS, MALL, quest or lab
                                    // Only a positive MRO sets results to positive complete
                                    testInfoResult = OverAllTestResult.Positive;
                                    testInfoStatus = DonorRegistrationStatus.Processing;
                                }
                            }
                            else if (overAllResult == OverAllTestResult.None && reportType == ReportType.MROReport)
                            {
                                // something is wrong and the MRO doesn't have a NEG or POS answer - INVALID or Discard (as per david, via SMS mar 11 2020 10:45PM (approx)
                                // Set it to complete, no result
                                // This would occur with an INVALID MRO result type
                                testInfoResult = OverAllTestResult.None;
                                testInfoStatus = DonorRegistrationStatus.Completed;

                            }

                            // We will update the test record with these results, no matter what
                            // If we fall through everything, it'll be result none - processing
                            // We'll do an update anyway, just in case.

                            if (testInfoResult != OverAllTestResult.None)
                            {

                                // UpdateSpecimenDetails(reportDetails, returnValues, trans, false);

                                //Update the test info table if the test status not completed and this is a lab report
                                if (!(returnValues.TestStatus == DonorRegistrationStatus.Completed && reportType == ReportType.LabReport))
                                {
                                    try
                                    {
                                        sqlQuery = "UPDATE donor_test_info SET donor_test_info.test_overall_result = @TestOverAllResult, donor_test_info.test_status = @TestStatus WHERE donor_test_info_id = @DonorTestInfoId";

                                        _param.reset();

                                        _param.Param = new MySqlParameter("@DonorTestInfoId", returnValues.DonorTestInfoId);
                                        _param.Param = new MySqlParameter("@TestOverAllResult", testInfoResult);
                                        _param.Param = new MySqlParameter("@TestStatus", testInfoStatus);
                                        //_logger.Information("Query5:" + sqlQuery);
                                        _logger.Information($"Updating Donor Test Info: {testInfoResult.ToString()} - {testInfoStatus.ToString()}");
                                        SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlQuery, _param.Params);

                                        // add the record as released

                                        //sqlQuery = @"
                                        //    INSERT INTO surpathlive.backend_integration_partner_release
                                        //    (
                                        //    backend_integration_partner_release_GUID, report_info_id, donor_test_info_id, released, last_modified_by, released_by
                                        //    ) 
                                        //    VALUES 
                                        //    (
                                        //    UUID(),@donor_test_info_id, @report_info_id, 1, 'PARSER', 'SYSTEM'
                                        //    );
                                        //    ";
                                        //_param.reset();

                                        //_param.Param = new MySqlParameter("@DonorTestInfoId", returnValues.DonorTestInfoId);
                                        //_param.Param = new MySqlParameter("@report_info_id", returnValues.ReportId);

                                        //_logger.Information($"Releasing Donor Test Info: {testInfoResult.ToString()} - {testInfoStatus.ToString()} report");
                                        //SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlQuery, _param.Params);
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.Error(ex.Message);
                                        if (ex.InnerException != null)
                                        {
                                            _logger.Error(ex.InnerException.ToString());

                                        }
                                        if (ex.StackTrace != null)
                                        {
                                            _logger.Error(ex.StackTrace.ToString());

                                        }
                                        throw;
                                    }

                                    // If this is a lab report that's been resent, it could be updating on top of an MRO report.
                                    // If it's positive, a lab report, and we have a specimen ID, we need to archive any MRO reports for that lab report
                                    // because we must treat new labreports as an *update* to a specemin ID. 
                                    if (testInfoResult == OverAllTestResult.Positive && reportType != ReportType.MROReport && !string.IsNullOrEmpty(backendParserHelper.ReportHelperItem.SpecimenID))
                                    {
                                        sqlQuery = "UPDATE report_info SET is_synchronized = b'0', is_archived = b'1', last_modified_on = NOW(), last_modified_by = 'SYSTEM' WHERE report_type = @ReportType AND specimen_id = @SpecimenId";

                                        MySqlParameter[] param = new MySqlParameter[2];

                                        param[0] = new MySqlParameter("@ReportType", (int)ReportType.MROReport);
                                        param[1] = new MySqlParameter("@SpecimenId", reportDetails.SpecimenId);

                                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);
                                    }

                                    if (returnValues.IntegrationPartner == true)
                                    {
                                        // Create a record for every test category into the backend_integration_partner_release table



                                        // if this is an integration partner, add a record into the backend_integration_partner_release table
                                        // if it's negative, automatically release it.
                                        sqlQuery = @"
                                    INSERT INTO surpathlive.backend_integration_partner_release
                                    (backend_integration_partner_release_GUID, donor_test_info_id, report_info_id, released, last_modified_by, released_by) 
                                    VALUES (@backend_integration_partner_release_GUID, @donor_test_info_id, @report_info_id, @released,  'PARSER', 'na');
                                    ";

                                        var released = 0;
                                        if (testInfoResult == OverAllTestResult.Negative) released = 1; // negatives are automatically released

                                        _param.reset();

                                        _param.Param = new MySqlParameter("@backend_integration_partner_release_GUID", Guid.NewGuid().ToString());
                                        _param.Param = new MySqlParameter("@donor_test_info_id", returnValues.DonorTestInfoId);
                                        _param.Param = new MySqlParameter("@released", released);
                                        _param.Param = new MySqlParameter("@report_info_id", returnValues.ReportId);

                                        _logger.Information($"Inserting backend_integration_partner_release record: {testInfoResult.ToString()} - Released? {released == 1}");
                                        SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlQuery, _param.Params);
                                    }



                                    // Finally - set this report file as the data of record
                                    backendParserHelper.Set_As_Data_Of_record(returnValues.ReportId);

                                }

                            }

                        }
                        else
                        {
                            _logger.Debug("Would insert - this is a dry run");
                        }
                    } // All of this only happens if it's NOT a mismatch

                    if (returnValues.DonorTestInfoId == 0) // && backendParserHelper.Add_To_Database
                    {
                        _logger.Debug($"No DonorTestInfoId exists - couldn't match");
                        backendParserHelper.ReportHelperItem.mismatch = true;

                        if (DryRun == false)
                        {
                            ////Log we have something we can't match
                            //if (returnValues.DonorId == 0
                            //    && returnValues.ClientId > 0
                            //    && returnValues.ClientDepartmentId > 0
                            //    && (returnValues.ClientDeptTestPanelId == 0 || returnValues.ClientDeptTestPanelId > 0)
                            //    && donorTestInfo.DonorTestInfoId == 0)
                            //{
                            //    _logger.Information($"We didn't match something here....");
                            //    _logger.Information($"Report received of type {reportType.ToString()}");
                            //    if (returnValues.DonorId == 0) _logger.Information($"No donor id found");
                            //    if (returnValues.DonorTestInfoId == 0) _logger.Information($"No specimen id match found in donor test info");

                            //}


                            // this is a mismatch if not known, and new, insert; otherwise update the count. A mismatch is never the file of record.
                            if (backendParserHelper.mismatched_report_id < 1)
                            {
                                _logger.Information($"This is a new mismatch");


                                // New mismatch
                                // Normal Mismatch operation
                                sqlQuery = "INSERT INTO mismatched_reports ("
                                                  + "report_info_id, "
                                                  + "specimen_id, "
                                                  + "donor_full_name, "
                                                  + "client_code, "
                                                  + "date_of_test, "
                                                  + "ssn_id, "
                                                  + "donor_pid, "
                                                  + "donor_pid_type_id, "
                                                  + "donor_dob, "
                                                  + "mismatched_count, "
                                                  + "is_synchronized, "
                                                  + "is_archived, "
                                                  + "created_on, "
                                                  + "created_by, "
                                                  + "last_modified_on, "
                                                  + "last_modified_by, "
                                                  + "file_name) "
                                                  + " VALUES ("
                                                  + "@ReportID, "
                                                  + "@SpecimenId, "
                                                  + "@donor_full_name, "
                                                  + "@ClientCode, "
                                                  + "@DateOfTest, "
                                                  + "@SsnId, "
                                                  + "@donor_pid, "
                                                  + "@donor_pid_type_id, "
                                                  + "@DonorDOB, "
                                                  + "@MismatchedCount, "
                                                  + " b'0', b'0', NOW(), 'SYSTEM', NOW(), 'SYSTEM', @FileName)";

                                string donorName = reportDetails.DonorFirstName + " " + reportDetails.DonorLastName;
                                //ParamHelper _param = new ParamHelper();
                                _param.reset();
                                _param.Param = new MySqlParameter("@ReportID", returnValues.ReportId); // set to zero if null?
                                _param.Param = new MySqlParameter("@SpecimenId", reportDetails.SpecimenId);
                                _param.Param = new MySqlParameter("@donor_full_name", donorName);
                                _param.Param = new MySqlParameter("@ClientCode", reportDetails.CrlClientCode);
                                //_logger.Information("Query7:" + sqlQuery);
                                _logger.Information("Inserting Into Mismatched Report");
                                if (reportDetails.SpecimenCollectionDate != string.Empty)
                                {
                                    if (reportDetails.SpecimenCollectionDate != null && reportDetails.SpecimenCollectionDate != string.Empty)
                                    {
                                        _param.Param = new MySqlParameter("@DateOfTest", DateTime.ParseExact(reportDetails.SpecimenCollectionDate.Substring(0, 8), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy"));
                                    }
                                    else
                                    {
                                        _param.Param = new MySqlParameter("@DateOfTest", null);
                                    }
                                }
                                else
                                {
                                    _param.Param = new MySqlParameter("@DateOfTest", null);
                                }

                                _param.Param = new MySqlParameter("@SsnId", reportDetails.SsnId);
                                _param.Param = new MySqlParameter("@DonorDOB", donorDOB);
                                _param.Param = new MySqlParameter("@MismatchedCount", 1);
                                _param.Param = new MySqlParameter("@FileName", fileName);
                                _param.Param = new MySqlParameter("@donor_pid", reportDetails.PID);
                                _param.Param = new MySqlParameter("@donor_pid_type_id", reportDetails.PIDType);
                                //_param.Param = new MySqlParameter("@data_checksum", reportDetails.data_checksum);



                                SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, _param.Params);

                                sqlQuery = "SELECT LAST_INSERT_ID()";
                                _logger.Information("Query8:" + sqlQuery);
                                returnValues.MismatchRecordId = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlQuery));
                                backendParserHelper.ReportHelperItem.mismatched_count = 1; // this is the first time we've mismatched this file

                            }
                            else
                            {
                                _logger.Information($"This is a known mismatch");


                                // this is a mismatch reprocess. It's already in the database
                                // we'll count the number of processes
                                // TODO - add mechanism to move to never match if we process a file more than 90 days
                                // or if we process it more than 100 times - since mismatches are reprocessed once  day (currently)
                                // age of mismatch is more robust and should be used
                                if (backendParserHelper.mismatched_report_id > 0)
                                {
                                    // if we have a mismatched_report_id we'll go the database.
                                    _param.reset();
                                    _param.Param = new MySqlParameter("@mismatched_report_id", backendParserHelper.mismatched_report_id);
                                    backendParserHelper.mismatched_count = backendParserHelper.mismatched_count + 1;
                                    _param.Param = new MySqlParameter("@mismatched_count", backendParserHelper.mismatched_count);

                                    sqlQuery = @"update mismatched_reports mr1
                                    set mr1.mismatched_count = @mismatched_count
                                    where mr1.mismatched_report_id = @mismatched_report_id";

                                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, _param.Params);

                                    returnValues.MismatchRecordId = backendParserHelper.mismatched_report_id;

                                    backendParserHelper.ReportHelperItem.mismatched_count = backendParserHelper.mismatched_count;

                                }
                            }
                        }
                        else
                        {
                            returnValues.MismatchRecordId = backendParserHelper.mismatched_report_id;

                            backendParserHelper.ReportHelperItem.mismatched_count = backendParserHelper.mismatched_count;

                            _logger.Debug("Would Mismatch - this is a dry run");
                        }


                    }


                    trans.Commit();


                    returnValue = true;
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message);
                    _logger.Error(ex.StackTrace.ToString());
                    _logger.Error("RolledBack Report changes  for File:" + "(" + NumPassed + "/" + TotalTo + ")" + fileName);
                    _logger.Error("-------------------------------------------------------------------------------");
                    trans.Rollback();
                    _logger.Error(ex.ToString());
                    if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                    if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                    returnValue = false;
                    throw;
                }
            }
            _logger.Information("Finished updating database for File:" + "(" + NumPassed + "/" + TotalTo + ")" + fileName);
            _logger.Information("UpdateReportReturnValue:" + returnValue.ToString());
            _logger.Information("-------------------------------------------------------------------------------");





            return returnValue;
        }


        private OverAllTestResult DetermineOverallResult(string Status)
        {
            if (string.IsNullOrEmpty(Status)) return OverAllTestResult.None;
            _logger.Information("DetermineOverallResult...");
            if (Status.ToUpper().Contains("POSITIVE") || Status.ToUpper().Contains("POS"))
            {
                _logger.Information($"Result: {Status}");
                return OverAllTestResult.Positive;

            }
            if (Status.ToUpper().Contains("NEGATIVE") || Status.ToUpper().Contains("NEG"))
            {
                _logger.Information($"Result: {Status}");
                return OverAllTestResult.Negative;

            }
            // If Status isn't empty and it's not postivie or negative - then we have some other type of issue
            _logger.Information($"Result of OTHER: {Status}");
            return OverAllTestResult.None;
        }


        public void UploadPDFReport(string specimenId, DonorDocument donorDocument)
        {
            int docId = 0;

            // final_report_id is set here, it's actually the donor_document_id
            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                string sqlGetDocId = "SELECT REPORT_INFO_ID AS REPORTID, SPECIMEN_ID AS SPECIMENID, "
                                    + "FINAL_REPORT_ID AS DOCUMENTID FROM REPORT_INFO WHERE SPECIMEN_ID = @specimen ";

                MySqlTransaction trans = conn.BeginTransaction();
                MySqlParameter[] prmGetDocId = new MySqlParameter[1];
                prmGetDocId[0] = new MySqlParameter("@specimen", specimenId);

                MySqlDataReader dr = SqlHelper.ExecuteReader(trans, CommandType.Text, sqlGetDocId, prmGetDocId);

                if (dr.Read())
                {
                    docId = !string.IsNullOrEmpty(dr["DOCUMENTID"].ToString()) ? Convert.ToInt32(dr["DOCUMENTID"]) : 0;
                }
                dr.Close();

                if (docId > 0)
                {
                    try
                    {
                        string sqlUpdateDoc = " UPDATE donor_documents SET document_upload_time = NOW(),"
                                                + " document_title = @DocumentTitle, document_content = @DocumentContent, "
                                                + " source = @Source, uploaded_by = @UploadedBy, "
                                                + " file_name = @FileName WHERE donor_document_id = @DocumentId";

                        MySqlParameter[] prmUpdateDocumet = new MySqlParameter[6];

                        prmUpdateDocumet[0] = new MySqlParameter("@DocumentTitle", donorDocument.DocumentTitle);
                        prmUpdateDocumet[1] = new MySqlParameter("@DocumentContent", donorDocument.DocumentContent);
                        prmUpdateDocumet[2] = new MySqlParameter("@Source", donorDocument.Source);
                        prmUpdateDocumet[3] = new MySqlParameter("@UploadedBy", donorDocument.UploadedBy);
                        prmUpdateDocumet[4] = new MySqlParameter("@FileName", donorDocument.FileName);
                        prmUpdateDocumet[5] = new MySqlParameter("@DocumentId", docId);

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlUpdateDoc, prmUpdateDocumet);

                        //Update report_info table

                        sqlUpdateDoc = "UPDATE report_info SET final_report_id = @DocumentId WHERE specimen_id = @SpecimenId";

                        prmUpdateDocumet = new MySqlParameter[2];

                        prmUpdateDocumet[0] = new MySqlParameter("@DocumentId", docId);
                        prmUpdateDocumet[1] = new MySqlParameter("@SpecimenId", specimenId);

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlUpdateDoc, prmUpdateDocumet);

                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        _logger.Error(ex.ToString());
                        if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                        if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                        throw;
                    }
                }
                else
                {
                    try
                    {
                        string sqlQuery = "INSERT INTO donor_documents ("
                                            + "donor_id, "
                                            + "document_upload_time, "
                                            + "document_title, "
                                            + "document_content, "
                                            + "source, "
                                            + "uploaded_by, "
                                            + "file_name, "
                                            + "is_synchronized) VALUES ("
                                            + "@DonorId, "
                                            + "NOW(),"
                                            + "@DocumentTitle, "
                                            + "@DocumentContent, "
                                            + "@Source, "
                                            + "@UploadedBy, "
                                            + "@FileName, "
                                            + "b'0')";

                        MySqlParameter[] param = new MySqlParameter[6];

                        param[0] = new MySqlParameter("@DonorId", donorDocument.DonorId);
                        param[1] = new MySqlParameter("@DocumentTitle", donorDocument.DocumentTitle);
                        param[2] = new MySqlParameter("@DocumentContent", donorDocument.DocumentContent);
                        param[3] = new MySqlParameter("@Source", donorDocument.Source);
                        param[4] = new MySqlParameter("@UploadedBy", donorDocument.UploadedBy);
                        param[5] = new MySqlParameter("@FileName", donorDocument.FileName);

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                        sqlQuery = "SELECT LAST_INSERT_ID()";

                        docId = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlQuery));

                        sqlQuery = "UPDATE report_info SET final_report_id = @DocumentId WHERE specimen_id = @SpecimenId";

                        param = new MySqlParameter[2];

                        param[0] = new MySqlParameter("@DocumentId", docId);
                        param[1] = new MySqlParameter("@SpecimenId", specimenId);

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        _logger.Error(ex.ToString());
                        if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                        if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                        throw;
                    }
                }

                // Add a release record if this is an integration partner

                // get the donor test info 


            }
        }

        public void UploadPDFChainOfCustodyReport(string specimenId, DonorDocument donorDocument, ResultOrderResults resultOrderResults, Screening _drugTestResult, DonorTestInfo donorTestInfo)
        {
            _logger.Debug($"UploadPDFChainOfCustodyReport upload for specimenId {specimenId} donorTestInfo {donorTestInfo}");
            ParamHelper _param = new ParamHelper();
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();

            // final_report_id is set here, it's actually the donor_document_id
            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                string sqlGetDocId = "SELECT REPORT_INFO_ID AS REPORTID, SPECIMEN_ID AS SPECIMENID, "
                                    + "FINAL_REPORT_ID AS DOCUMENTID FROM REPORT_INFO WHERE SPECIMEN_ID = @specimen AND report_type = @report_type ";

                MySqlTransaction trans = conn.BeginTransaction();
                //MySqlParameter[] prmGetDocId = new MySqlParameter[3];
                //prmGetDocId[0] = new MySqlParameter("@specimen", specimenId);
                //prmGetDocId[1] = new MySqlParameter("@source", donorDocument.Source);
                //prmGetDocId[2] = new MySqlParameter("@report_type", (int)ReportType.ChainOfCustodyReport);
                _param.reset();
                _param.Param = new MySqlParameter("@specimen", specimenId);
                //_param.Param = new MySqlParameter("@source", donorDocument.Source);
                _param.Param = new MySqlParameter("@report_type", (int)ReportType.ChainOfCustodyReport);

                MySqlDataReader dr = SqlHelper.ExecuteReader(trans, CommandType.Text, sqlGetDocId, _param.Params);
                int docId = 0;

                if (dr.Read())
                {
                    docId = !string.IsNullOrEmpty(dr["DOCUMENTID"].ToString()) ? Convert.ToInt32(dr["DOCUMENTID"]) : 0;
                }
                dr.Close();

                if (docId > 0)
                {
                    _logger.Debug($"Existing chain of custody report found {docId}");
                    try
                    {
                        string sqlUpdateDoc = " UPDATE donor_documents SET document_upload_time = NOW(),"
                                                + " document_title = @DocumentTitle, document_content = @DocumentContent, "
                                                + " source = @Source, uploaded_by = @UploadedBy, "
                                                + " file_name = @FileName WHERE donor_document_id = @DocumentId";

                        _param.reset();

                        _param.Param = new MySqlParameter("@DocumentTitle", donorDocument.DocumentTitle);
                        _param.Param = new MySqlParameter("@DocumentContent", donorDocument.DocumentContent);
                        _param.Param = new MySqlParameter("@Source", donorDocument.Source);
                        _param.Param = new MySqlParameter("@UploadedBy", donorDocument.UploadedBy);
                        _param.Param = new MySqlParameter("@FileName", donorDocument.FileName);
                        _param.Param = new MySqlParameter("@DocumentId", docId);

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlUpdateDoc, _param.Params);

                        //Update report_info table
                        _logger.Debug($"Setting report_info final_report_id to {docId}");
                        sqlUpdateDoc = "UPDATE report_info SET final_report_id = @DocumentId WHERE specimen_id = @SpecimenId AND report_type = @ReportType";

                        _param.reset();


                        _param.Param = new MySqlParameter("@DocumentId", docId);
                        _param.Param = new MySqlParameter("@SpecimenId", specimenId);
                        _param.Param = new MySqlParameter("@ReportType", (int)ReportType.ChainOfCustodyReport);


                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlUpdateDoc, _param.Param);

                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        _logger.Error(ex.ToString());
                        if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                        if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                        throw;
                    }
                }
                else
                {
                    try
                    {
                        string sqlQuery = "INSERT INTO donor_documents ("
                                            + "donor_id, "
                                            + "document_upload_time, "
                                            + "document_title, "
                                            + "document_content, "
                                            + "source, "
                                            + "uploaded_by, "
                                            + "file_name, "
                                            + "is_synchronized) VALUES ("
                                            + "@DonorId, "
                                            + "NOW(),"
                                            + "@DocumentTitle, "
                                            + "@DocumentContent, "
                                            + "@Source, "
                                            + "@UploadedBy, "
                                            + "@FileName, "
                                            + "b'0')";

                        _param.reset();


                        _param.Param = new MySqlParameter("@DonorId", donorDocument.DonorId);
                        _param.Param = new MySqlParameter("@DocumentTitle", donorDocument.DocumentTitle);
                        _param.Param = new MySqlParameter("@DocumentContent", donorDocument.DocumentContent);
                        _param.Param = new MySqlParameter("@Source", donorDocument.Source);
                        _param.Param = new MySqlParameter("@UploadedBy", donorDocument.UploadedBy);
                        _param.Param = new MySqlParameter("@FileName", donorDocument.FileName);

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, _param.Params);

                        sqlQuery = "SELECT LAST_INSERT_ID()";

                        int documentId = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlQuery));

                        // Add this to reports
                        sqlQuery = "INSERT INTO report_info ("
                                       + "report_type, "
                                       + "specimen_id, "
                                       + "lab_sample_id, "
                                       + "ssn_id, "
                                       + "donor_last_name, "
                                       + "donor_first_name, "
                                       + "donor_mi, "
                                       + "donor_dob, "
                                       + "donor_gender, "
                                       + "lab_report, "
                                       + "lab_report_checksum, "
                                       + "data_checksum, "
                                       + "lab_report_source_filename, "
                                       + "screening_time, "

                                       + "lab_name, "
                                       + "lab_code, "
                                       + "lab_report_date, "
                                       + "donor_driving_license, "
                                       + "test_panel_code, "
                                       + "test_panel_name, "

                                       + "donor_test_info_id, "
                                       + "report_status, "
                                       + "is_synchronized, "
                                       + "is_archived, "
                                       + "created_on, "
                                       + "created_by, "
                                       + "last_modified_on, "
                                       + "last_modified_by) VALUES ("
                                       + "@ReportType, "
                                       + "@SpecimenId, "
                                       + "@LabSampleId, "
                                       + "@SsnId, "
                                       + "@DonorLastName, "
                                       + "@DonorFirstName, "
                                       + "@DonorMI, "
                                       + "@DonorDOB, "
                                       + "@DonorGender, "
                                       + "@LabReport, "
                                       + "@lab_report_checksum, "
                                       + "@data_checksum, "
                                       + "@lab_report_source_filename, "
                                       + "@screening_time, "

                                       + "@LabName, "
                                       + "@LabCode, "
                                       + "@LabReportDate, "
                                       + "@DonorDrivingLicense, "
                                       + "@TestPanelCode, "
                                       + "@TestPanelName, "

                                       + "@DonorTestInfoId, "
                                       + "@ReportStatus, "
                                       + "b'0', b'0', NOW(), 'SYSTEM', NOW(), 'SYSTEM')";

                        _param.reset();
                        _param.Param = new MySqlParameter("@ReportType", (int)ReportType.ChainOfCustodyReport);
                        _param.Param = new MySqlParameter("@SpecimenId", specimenId);
                        _param.Param = new MySqlParameter("@LabSampleId", "");
                        _param.Param = new MySqlParameter("@SsnId", resultOrderResults.PersonalData.PrimaryId);
                        _param.Param = new MySqlParameter("@DonorLastName", resultOrderResults.PersonalData.PersonName.FamilyName);
                        _param.Param = new MySqlParameter("@DonorFirstName", resultOrderResults.PersonalData.PersonName.GivenName);
                        if (resultOrderResults.PersonalData.PersonName.MiddleName.Length > 0)
                        {
                            _param.Param = new MySqlParameter("@DonorMI", resultOrderResults.PersonalData.PersonName.MiddleName.Trim().Substring(0, 1));

                        }
                        else
                        {
                            _param.Param = new MySqlParameter("@DonorMI", "");

                        }
                        if (DateTime.TryParse(resultOrderResults.PersonalData.DateofBirth, out DateTime _dob))
                        {
                            _param.Param = new MySqlParameter("@DonorDOB", _dob.ToString("yyyy/MM/dd"));
                        }
                        else
                        {
                            _param.Param = new MySqlParameter("@DonorDOB", "");
                        }

                        _param.Param = new MySqlParameter("@DonorGender", resultOrderResults.PersonalData.Gender.IdValue);
                        _param.Param = new MySqlParameter("@LabReport", donorDocument.DocumentContent);
                        string lab_report_checksum = BitConverter.ToString(sha1.ComputeHash(donorDocument.DocumentContent)).Replace("-", string.Empty);
                        _param.Param = new MySqlParameter("@lab_report_checksum", lab_report_checksum);
                        _param.Param = new MySqlParameter("@data_checksum", "");
                        _param.Param = new MySqlParameter("@lab_report_source_filename", donorDocument.FileName);

                        DateTime _SpecimenCollectionDateTime;
                        if (DateTime.TryParse(_drugTestResult.DateCollected, out DateTime _dateCollected))
                        {
                            _param.Param = new MySqlParameter("@screening_time", _dateCollected);

                        }
                        else
                        {
                            _param.Param = new MySqlParameter("@screening_time", null);

                        }


                        _param.Param = new MySqlParameter("@LabName", _drugTestResult.LaboratoryID);
                        _param.Param = new MySqlParameter("@LabCode", _drugTestResult.LaboratoryAccount);
                        _param.Param = new MySqlParameter("@LabReportDate", null);
                        _param.Param = new MySqlParameter("@DonorDrivingLicense", null);
                        _param.Param = new MySqlParameter("@TestPanelCode", _drugTestResult.UnitCodes.IdValue);
                        _param.Param = new MySqlParameter("@TestPanelName", null);

                        _param.Param = new MySqlParameter("@DonorTestInfoId", donorTestInfo.DonorTestInfoId);
                        _param.Param = new MySqlParameter("@ReportStatus", (int)OverAllTestResult.Other);

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, _param.Params);

                        sqlQuery = "SELECT LAST_INSERT_ID()";
                        //_logger.Information("Query2:" + sqlQuery);
                        //returnValues.ReportId = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlQuery));

                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        _logger.Error(ex.ToString());
                        if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                        if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                        throw;
                    }
                }
            }
        }


        //public bool UpdateReParsingReport(ReportType reportType, ReportInfo reportDetails, List<OBR_Info> obrList, RTFBuilderbase crlReport, bool archiveExistingReport, ReturnValues returnValues, string fileName, int NumPassed, int TotalTo)
        //{
        //    bool returnValue = false;

        //    using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
        //    {
        //        conn.Open();

        //        MySqlTransaction trans = conn.BeginTransaction();

        //        try
        //        {
        //            //Lab Report
        //            byte[] labReport = null;

        //            if (crlReport.ToString() != string.Empty)
        //            {
        //                labReport = Encoding.ASCII.GetBytes(crlReport.ToString());
        //            }

        //            //Donor DOB
        //            string donorDOB = string.Empty;
        //            if (reportDetails.DonorDOB != null && reportDetails.DonorDOB != string.Empty)
        //            {
        //                donorDOB = DateTime.ParseExact(reportDetails.DonorDOB.Substring(0, 8), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
        //            }

        //            ProcessLabClientCode(reportDetails, obrList);

        //            //Archvive the report
        //            ArchiveReport(reportType, reportDetails, archiveExistingReport, trans);

        //            //Verifying the SSN format
        //            ValidateDonorSSn(reportDetails, returnValues);

        //            //Finding the DonorId
        //            GetDonorDetails(reportDetails, returnValues, trans);

        //            //Finding the ClientId, ClientDepartmentId & ClientDeptTestPanelId
        //            GetClientDetails(reportDetails, returnValues, trans, fileName, NumPassed, TotalTo, reportType);

        //            //Finding SpecimenId
        //            DonorTestInfo donorTestInfo = GetSpecimenDetails(returnValues, reportDetails, trans);

        //            //Add the donor and test info if the donor SSN is not found
        //            if (returnValues.DonorId == 0
        //                && returnValues.ClientId > 0
        //                && returnValues.ClientDepartmentId > 0
        //                && (returnValues.ClientDeptTestPanelId == 0 || returnValues.ClientDeptTestPanelId > 0)
        //                && reportType == ReportType.LabReport && donorTestInfo.DonorTestInfoId == 0)
        //            {
        //                if (ConfigurationManager.AppSettings["IsDonorReverseEntryWithTestInfo"].ToString().Trim().ToUpper() == "TRUE")
        //                {
        //                    AddDonorInfoWithTestDetails(reportDetails, returnValues, trans);
        //                }
        //            }

        //            string sqlQuery = "";
        //            MySqlParameter[] param = null;

        //            sqlQuery = "SELECT "
        //                    + "donor_test_info.donor_test_info_id AS DonorTestInfoId, "
        //                    + "donor_test_info.mro_type_id AS MROTypeId "
        //                    + "FROM donor_test_info "
        //                    + "INNER JOIN donor_test_info_test_categories ON donor_test_info_test_categories.donor_test_info_id = donor_test_info.donor_test_info_id "
        //                    + "WHERE donor_test_info_test_categories.specimen_id = @SpecimenId";

        //            param = new MySqlParameter[1];
        //            param[0] = new MySqlParameter("@SpecimenId", reportDetails.SpecimenId);

        //            MySqlDataReader dr = SqlHelper.ExecuteReader(trans, CommandType.Text, sqlQuery, param);

        //            if (dr.Read())
        //            {
        //                returnValues.DonorTestInfoId = Convert.ToInt32(dr["DonorTestInfoId"]);
        //                returnValues.MROType = dr["MROTypeId"] != DBNull.Value ? (ClientMROTypes)(Convert.ToInt32(dr["MROTypeId"].ToString())) : ClientMROTypes.None;
        //            }
        //            dr.Close();

        //            //Applying intelligence to find the test info
        //            if (returnValues.DonorTestInfoId == 0 && reportType == ReportType.LabReport)
        //            {
        //                if (ConfigurationManager.AppSettings["IsAutoMatchTestInfo"].ToString().Trim().ToUpper() == "TRUE")
        //                {
        //                    DoTestInfoIntelligence(reportDetails, returnValues, trans);
        //                }
        //            }

        //            //Addinng test info in case of donor available
        //            if (returnValues.DonorTestInfoId == 0
        //                && returnValues.DonorId > 0
        //                && returnValues.ClientId > 0
        //                && returnValues.ClientDepartmentId > 0
        //                && returnValues.ClientDeptTestPanelId > 0
        //                && returnValues.TestInfoRecordCount == 0
        //                && reportType == ReportType.LabReport)
        //            {
        //                if (ConfigurationManager.AppSettings["IsTestInfoReverseEntry"].ToString().Trim().ToUpper() == "TRUE")
        //                {
        //                    AddTestInfoDetails(reportDetails, returnValues, trans);
        //                }
        //            }

        //            OverAllTestResult overAllResult = OverAllTestResult.None;

        //            //Finding the Overall Result
        //            int i = 0;
        //            foreach (OBR_Info obr in obrList)
        //            {
        //                if (obr.SectionHeader.ToUpper().Contains("CONFIRMATION") || reportType == ReportType.QuestLabReport)
        //                {
        //                    foreach (OBX_Info obx in obr.observatinos)
        //                    {
        //                        if (obx.Status.ToUpper().Contains("POSITIVE") || obx.Status.ToUpper().Contains("POS"))
        //                        {
        //                            overAllResult = OverAllTestResult.Positive;
        //                            break;
        //                        }
        //                    }
        //                }
        //                else if (obr.SectionHeader.ToUpper().Contains("INITIAL TEST") || obr.SectionHeader.ToUpper().Contains("SUBSTANCE ABUSE PANEL"))
        //                {
        //                    foreach (OBX_Info obx in obr.observatinos)
        //                    {
        //                        if (obx.Status.ToUpper().Contains("POSITIVE") || obx.Status.ToUpper().Contains("POS"))
        //                        {
        //                            overAllResult = OverAllTestResult.Positive;
        //                            break;
        //                        }
        //                        i++;
        //                    }
        //                }
        //                else if (reportType == ReportType.MROReport)
        //                {
        //                    foreach (OBX_Info obx in obr.observatinos)
        //                    {
        //                        if (obx.Status.ToUpper().Contains("POSITIVE") || obx.Status.ToUpper().Contains("POS"))
        //                        {
        //                            overAllResult = OverAllTestResult.Positive;
        //                            break;
        //                        }
        //                        i++;
        //                    }
        //                }
        //            }

        //            if (i > 0 && overAllResult == OverAllTestResult.None)
        //            {
        //                overAllResult = OverAllTestResult.Negative;
        //            }
        //            //
        //            bool isReportExists = false;
        //            MySqlParameter[] prmReportInfo = null;

        //            sqlQuery = "SELECT report_info.specimen_id AS SpecimenID FROM report_info "
        //                    + "WHERE report_info.specimen_id = @SpecimenId";

        //            prmReportInfo = new MySqlParameter[1];
        //            prmReportInfo[0] = new MySqlParameter("@SpecimenId", reportDetails.SpecimenId);

        //            MySqlDataReader drReportInfo = SqlHelper.ExecuteReader(trans, CommandType.Text, sqlQuery, prmReportInfo);

        //            if (drReportInfo.Read())
        //            {
        //                isReportExists = true;
        //            }
        //            drReportInfo.Close();

        //            if (!isReportExists)
        //            {
        //                sqlQuery = "INSERT INTO report_info (report_type, specimen_id, lab_sample_id, ssn_id, donor_last_name, donor_first_name, donor_mi, "
        //                                   + " donor_dob, donor_gender,  lab_report,  lab_name, lab_code, lab_report_date, donor_driving_license,test_panel_code, "
        //                                   + " test_panel_name, donor_test_info_id,  report_status,  is_synchronized,  is_archived,  created_on,  created_by, "
        //                                   + "last_modified_on, "
        //                                   + "last_modified_by) VALUES (@ReportType, @SpecimenId, @LabSampleId, @SsnId, @DonorLastName, @DonorFirstName, @DonorMI, "
        //                                   + "@DonorDOB, @DonorGender, @LabReport, @LabName, @LabCode, @LabReportDate, @DonorDrivingLicense, @TestPanelCode, "
        //                                   + "@TestPanelName, @DonorTestInfoId, @ReportStatus, b'0', b'0', NOW(), 'SYSTEM', NOW(), 'SYSTEM')";

        //                param = new MySqlParameter[18];

        //                param[0] = new MySqlParameter("@ReportType", (int)reportType);
        //                param[1] = new MySqlParameter("@SpecimenId", reportDetails.SpecimenId);
        //                param[2] = new MySqlParameter("@LabSampleId", reportDetails.LabSampleId);
        //                param[3] = new MySqlParameter("@SsnId", reportDetails.SsnId);
        //                param[4] = new MySqlParameter("@DonorLastName", reportDetails.DonorLastName);
        //                param[5] = new MySqlParameter("@DonorFirstName", reportDetails.DonorFirstName);
        //                param[6] = new MySqlParameter("@DonorMI", reportDetails.DonorMI);
        //                param[7] = new MySqlParameter("@DonorDOB", donorDOB);
        //                param[8] = new MySqlParameter("@DonorGender", reportDetails.DonorGender);
        //                param[9] = new MySqlParameter("@LabReport", labReport);

        //                param[10] = new MySqlParameter("@LabName", reportDetails.LabName);
        //                param[11] = new MySqlParameter("@LabCode", reportDetails.LabCode);
        //                param[12] = new MySqlParameter("@LabReportDate", reportDetails.LabReportDate);
        //                param[13] = new MySqlParameter("@DonorDrivingLicense", reportDetails.DonorDrivingLicense);
        //                param[14] = new MySqlParameter("@TestPanelCode", reportDetails.TestPanelCode);
        //                param[15] = new MySqlParameter("@TestPanelName", reportDetails.TestPanelName);

        //                param[16] = new MySqlParameter("@DonorTestInfoId", returnValues.DonorTestInfoId);
        //                param[17] = new MySqlParameter("@ReportStatus", (int)overAllResult);

        //                SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

        //                sqlQuery = "SELECT LAST_INSERT_ID()";

        //                returnValues.ReportId = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlQuery));

        //                foreach (OBR_Info obr in obrList)
        //                {
        //                    string specimenCollectionDate = string.Empty;
        //                    if (obr.SpecimenCollectionDate != null && obr.SpecimenCollectionDate != string.Empty)
        //                    {
        //                        specimenCollectionDate = DateTime.ParseExact(obr.SpecimenCollectionDate.Substring(0, 8), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
        //                    }

        //                    string specimenReceivedDate = string.Empty;
        //                    if (obr.SpecimenReceivedDate != null && obr.SpecimenReceivedDate != string.Empty)
        //                    {
        //                        specimenReceivedDate = DateTime.ParseExact(obr.SpecimenReceivedDate.Substring(0, 8), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
        //                    }

        //                    string crlTransmitDate = string.Empty;
        //                    if (obr.CrlTransmitDate != null && obr.CrlTransmitDate != string.Empty)
        //                    {
        //                        crlTransmitDate = DateTime.ParseExact(obr.CrlTransmitDate.Substring(0, 8), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
        //                    }

        //                    sqlQuery = "INSERT INTO obr_info ("
        //                                    + "report_info_id, "
        //                                    + "transmited_order, "
        //                                    + "collection_site_info, "
        //                                    + "specimen_collection_date, "
        //                                    + "specimen_received_date, "
        //                                    + "crl_client_code, "
        //                                    + "specimen_type, "
        //                                    + "section_header, "
        //                                    + "crl_transmit_date, "
        //                                    + "service_section_id, "
        //                                    + "order_status, "
        //                                    + "reason_type, "

        //                                    + "collection_site_id, "
        //                                    + "specimen_action_code, "
        //                                    + "tpa_code, "
        //                                    + "region_code, "
        //                                    + "client_code, "
        //                                    + "deaprtment_code, "

        //                                    + "is_synchronized, "
        //                                    + "created_on, "
        //                                    + "created_by, "
        //                                    + "last_modified_on, "
        //                                    + "last_modified_by) VALUES ("
        //                                    + "@ReportInfoId, "
        //                                    + "@TransmitedOrder, "
        //                                    + "@CollectionSiteInfo, "
        //                                    + "@SpecimenCollectionDate, "
        //                                    + "@SpecimenReceivedDate, "
        //                                    + "@CrlClientCode, "
        //                                    + "@SpecimenType, "
        //                                    + "@SectionHeader, "
        //                                    + "@CrlTransmitDate, "
        //                                    + "@ServiceSectionId, "
        //                                    + "@OrderStatus, "
        //                                    + "@ReasonType, "

        //                                    + "@CollectionSiteId, "
        //                                    + "@SpecimenActionCode, "
        //                                    + "@TpaCode, "
        //                                    + "@RegionCode, "
        //                                    + "@ClientCode, "
        //                                    + "@DepartmentCode, "

        //                                    + "b'0', NOW(), 'SYSTEM', NOW(), 'SYSTEM')";

        //                    param = new MySqlParameter[18];

        //                    param[0] = new MySqlParameter("@ReportInfoId", returnValues.ReportId);
        //                    param[1] = new MySqlParameter("@TransmitedOrder", obr.TransmitedOrder);
        //                    param[2] = new MySqlParameter("@CollectionSiteInfo", obr.CollectionSiteInfo);
        //                    param[3] = new MySqlParameter("@SpecimenCollectionDate", specimenCollectionDate);
        //                    param[4] = new MySqlParameter("@SpecimenReceivedDate", specimenReceivedDate);
        //                    param[5] = new MySqlParameter("@CrlClientCode", obr.CrlClientCode);
        //                    param[6] = new MySqlParameter("@SpecimenType", obr.SpecimenType);
        //                    param[7] = new MySqlParameter("@SectionHeader", obr.SectionHeader);
        //                    param[8] = new MySqlParameter("@CrlTransmitDate", crlTransmitDate);
        //                    param[9] = new MySqlParameter("@ServiceSectionId", obr.ServiceSectionId);
        //                    param[10] = new MySqlParameter("@OrderStatus", obr.OrderStatus);
        //                    param[11] = new MySqlParameter("@ReasonType", obr.ReasonType);

        //                    param[12] = new MySqlParameter("@CollectionSiteId", obr.CollectionSiteId);
        //                    param[13] = new MySqlParameter("@SpecimenActionCode", obr.SpecimenActionCode);
        //                    param[14] = new MySqlParameter("@TpaCode", obr.TpaCode);
        //                    param[15] = new MySqlParameter("@RegionCode", obr.RegionCode);
        //                    param[16] = new MySqlParameter("@ClientCode", obr.ClientCode);
        //                    param[17] = new MySqlParameter("@DepartmentCode", obr.DepartmentCode);

        //                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

        //                    sqlQuery = "SELECT LAST_INSERT_ID()";

        //                    int obrId = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlQuery));

        //                    foreach (OBX_Info obx in obr.observatinos)
        //                    {
        //                        sqlQuery = "INSERT INTO OBX_Info ("
        //                                                        + "obr_info_id, "
        //                                                        + "sequence, "
        //                                                        + "test_code, "
        //                                                        + "test_name, "
        //                                                        + "result, "
        //                                                        + "status, "
        //                                                        + "unit_of_measure, "
        //                                                        + "reference_range, "
        //                                                        + "order_status, "
        //                                                        + "is_synchronized, "
        //                                                        + "created_on, "
        //                                                        + "created_by, "
        //                                                        + "last_modified_on, "
        //                                                        + "last_modified_by) VALUES ("
        //                                                        + "@OBRInfoId, "
        //                                                        + "@Sequence, "
        //                                                        + "@TestCode, "
        //                                                        + "@TestName, "
        //                                                        + "@Result, "
        //                                                        + "@Status, "
        //                                                        + "@UnitOfMeasure, "
        //                                                        + "@ReferenceRange, "
        //                                                        + "@OrderStatus, "
        //                                                        + "b'0', NOW(), 'SYSTEM', NOW(), 'SYSTEM')";

        //                        param = new MySqlParameter[9];

        //                        param[0] = new MySqlParameter("@OBRInfoId", obrId);
        //                        param[1] = new MySqlParameter("@Sequence", obx.Sequence);
        //                        param[2] = new MySqlParameter("@TestCode", obx.TestCode);
        //                        param[3] = new MySqlParameter("@TestName", obx.TestName);
        //                        param[4] = new MySqlParameter("@Result", obx.Result);
        //                        param[5] = new MySqlParameter("@Status", obx.Status);
        //                        param[6] = new MySqlParameter("@UnitOfMeasure", obx.UnitOfMeasure);
        //                        param[7] = new MySqlParameter("@ReferenceRange", obx.ReferenceRange);
        //                        param[8] = new MySqlParameter("@OrderStatus", obx.OrderStatus);

        //                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);
        //                    }
        //                }
        //            }

        //            //Finding MPOS & MALL
        //            if (reportType == ReportType.LabReport)
        //            {
        //                if (returnValues.DonorTestInfoId > 0)
        //                {
        //                    if ((returnValues.MROType == ClientMROTypes.MALL) || (returnValues.MROType == ClientMROTypes.MPOS && overAllResult == OverAllTestResult.Positive))
        //                    {
        //                        returnValues.MroAttentionFlag = true;
        //                    }
        //                }
        //            }

        //            OverAllTestResult testInfoResult = OverAllTestResult.None;
        //            DonorRegistrationStatus testInfoStatus = DonorRegistrationStatus.Processing;
        //            bool labReportFlag = false;
        //            if (returnValues.DonorTestInfoId > 0)
        //            {
        //                //if (overAllResult == OverAllTestResult.Negative)
        //                //{
        //                //    testInfoResult = OverAllTestResult.Negative;
        //                //    testInfoStatus = DonorRegistrationStatus.Completed;
        //                //}

        //                if (overAllResult == OverAllTestResult.Negative && returnValues.MROType == ClientMROTypes.MPOS)
        //                {
        //                    testInfoResult = OverAllTestResult.Negative;
        //                    testInfoStatus = DonorRegistrationStatus.Completed;
        //                }
        //                else if (overAllResult == OverAllTestResult.Negative && returnValues.MROType == ClientMROTypes.MALL && reportType == ReportType.MROReport)
        //                {
        //                    testInfoResult = OverAllTestResult.Negative;
        //                    testInfoStatus = DonorRegistrationStatus.Completed;
        //                }
        //                else if (overAllResult == OverAllTestResult.Positive && reportType == ReportType.MROReport)
        //                {
        //                    testInfoResult = OverAllTestResult.Positive;
        //                    testInfoStatus = DonorRegistrationStatus.Completed;
        //                }
        //                else if (overAllResult == OverAllTestResult.Positive && returnValues.MROType == ClientMROTypes.MPOS && reportType == ReportType.LabReport)
        //                {
        //                    //  testInfoResult = OverAllTestResult.Positive;
        //                    testInfoStatus = DonorRegistrationStatus.Processing;
        //                    labReportFlag = true;
        //                }
        //                if (labReportFlag == false)
        //                {
        //                    if (testInfoResult != OverAllTestResult.None)
        //                    {
        //                        //Update the test info table

        //                        sqlQuery = "UPDATE donor_test_info SET donor_test_info.test_overall_result = @TestOverAllResult, donor_test_info.test_status = @TestStatus WHERE donor_test_info_id = @DonorTestInfoId";

        //                        param = new MySqlParameter[3];

        //                        param[0] = new MySqlParameter("@DonorTestInfoId", returnValues.DonorTestInfoId);
        //                        param[1] = new MySqlParameter("@TestOverAllResult", testInfoResult);
        //                        param[2] = new MySqlParameter("@TestStatus", testInfoStatus);

        //                        SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlQuery, param);
        //                    }
        //                }
        //                else if (labReportFlag == true)
        //                {
        //                    //Update the test info table
        //                    sqlQuery = "UPDATE donor_test_info SET donor_test_info.test_overall_result = @TestOverAllResult, donor_test_info.test_status = @TestStatus WHERE donor_test_info_id = @DonorTestInfoId";

        //                    param = new MySqlParameter[3];

        //                    param[0] = new MySqlParameter("@DonorTestInfoId", returnValues.DonorTestInfoId);
        //                    param[1] = new MySqlParameter("@TestOverAllResult", testInfoResult);
        //                    param[2] = new MySqlParameter("@TestStatus", testInfoStatus);

        //                    SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlQuery, param);
        //                }
        //            }

        //            if (returnValues.DonorTestInfoId == 0)
        //            {
        //                bool isFileExists = false;
        //                MySqlParameter[] prmMismatchedFileInfo = null;

        //                sqlQuery = "SELECT mismatched_reports.mismatched_report_id AS MisMatchedReportID, "
        //                        + "mismatched_reports.specimen_id AS SpecimenID,  mismatched_reports.mismatched_count as MismatchedCount "
        //                        + "FROM mismatched_reports "
        //                        + "WHERE mismatched_reports.file_name = @FileName";

        //                prmMismatchedFileInfo = new MySqlParameter[1];
        //                prmMismatchedFileInfo[0] = new MySqlParameter("@FileName", fileName);

        //                MySqlDataReader drMismatchedInfo = SqlHelper.ExecuteReader(trans, CommandType.Text, sqlQuery, prmMismatchedFileInfo);
        //                int misMatchedCount = 0;
        //                int misMatchedReportID = 0;
        //                if (drMismatchedInfo.Read())
        //                {
        //                    misMatchedCount = Convert.ToInt32(drMismatchedInfo["MismatchedCount"]);
        //                    misMatchedReportID = Convert.ToInt32(drMismatchedInfo["MisMatchedReportID"]);
        //                    isFileExists = true;
        //                }
        //                drMismatchedInfo.Close();
        //                if (isFileExists)
        //                {
        //                    //Update the test info table
        //                    misMatchedCount = misMatchedCount + 1;
        //                    int isUnmatched = 0;
        //                    if (misMatchedCount >= Convert.ToInt32(ConfigurationManager.AppSettings["MaxReparsingCount"].ToString()))
        //                    {
        //                        isUnmatched = 1;
        //                    }
        //                    sqlQuery = "UPDATE mismatched_reports SET mismatched_reports.mismatched_count = @MismatchedCount, is_unmatched = @IsUnmatched, "
        //                    + "last_modified_on = NOW(), last_modified_by = @LastModifiedBy WHERE mismatched_report_id = @misMatchedReportID";

        //                    param = new MySqlParameter[4];

        //                    param[0] = new MySqlParameter("@MismatchedCount", misMatchedCount);
        //                    param[1] = new MySqlParameter("@misMatchedReportID", misMatchedReportID);
        //                    param[2] = new MySqlParameter("@LastModifiedBy", "SYSTEM");
        //                    param[3] = new MySqlParameter("@IsUnmatched", isUnmatched);

        //                    SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlQuery, param);
        //                    returnValues.MismatchedCount = misMatchedCount;
        //                    returnValues.MismatchRecordId = misMatchedReportID;
        //                }
        //                else
        //                {
        //                    sqlQuery = "INSERT INTO mismatched_reports ("
        //                                            + "report_info_id, "
        //                                            + "specimen_id, "
        //                                            + "donor_full_name, "
        //                                            + "client_code, "
        //                                            + "date_of_test, "
        //                                            + "ssn_id, "
        //                                            + "mismatched_count, "
        //                                            + "donor_dob, "
        //                                            + "is_synchronized, "
        //                                            + "is_archived, "
        //                                            + "created_on, "
        //                                            + "created_by, "
        //                                            + "last_modified_on, "
        //                                            + "last_modified_by, "
        //                                            + "file_name) "
        //                                            + " VALUES ("
        //                                            + "@ReportID, "
        //                                            + "@SpecimenId, "
        //                                            + "@donor_full_name, "
        //                                            + "@ClientCode, "
        //                                            + "@DateOfTest, "
        //                                            + "@SsnId, "
        //                                            + "@MismatchedCount, "
        //                                            + "@DonorDOB, "
        //                                            + " b'0', b'0', NOW(), 'SYSTEM', NOW(), 'SYSTEM', @FileName)";

        //                    string donorName = reportDetails.DonorFirstName + " " + reportDetails.DonorLastName;
        //                    param = new MySqlParameter[9];

        //                    param[0] = new MySqlParameter("@ReportID", returnValues.ReportId);
        //                    param[1] = new MySqlParameter("@SpecimenId", reportDetails.SpecimenId);
        //                    param[2] = new MySqlParameter("@donor_full_name", donorName);
        //                    param[3] = new MySqlParameter("@ClientCode", reportDetails.CrlClientCode);

        //                    if (reportDetails.SpecimenCollectionDate != string.Empty)
        //                    {
        //                        if (reportDetails.SpecimenCollectionDate != null && reportDetails.SpecimenCollectionDate != string.Empty)
        //                        {
        //                            param[4] = new MySqlParameter("@DateOfTest", DateTime.ParseExact(reportDetails.SpecimenCollectionDate.Substring(0, 8), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy"));
        //                        }
        //                        else
        //                        {
        //                            param[4] = new MySqlParameter("@DateOfTest", null);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        param[4] = new MySqlParameter("@DateOfTest", null);
        //                    }

        //                    param[5] = new MySqlParameter("@SsnId", reportDetails.SsnId);
        //                    param[6] = new MySqlParameter("@MismatchedCount", 1);
        //                    param[7] = new MySqlParameter("@DonorDOB", donorDOB);
        //                    param[8] = new MySqlParameter("@FileName", fileName);

        //                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

        //                    sqlQuery = "SELECT LAST_INSERT_ID()";

        //                    returnValues.MismatchRecordId = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlQuery));
        //                    returnValues.MismatchedCount = 1;
        //                }
        //            }
        //            trans.Commit();
        //            _logger.Information("Finished updating database for File:" + "(" + NumPassed + "/" + TotalTo + ")" + fileName);
        //            _logger.Information("---------------------------------------------------------------------------------------------");
        //            returnValue = true;
        //        }
        //        catch (Exception ex)
        //        {
        //            trans.Rollback();
        //            _logger.Information("Rollback DB for File:" + "(" + NumPassed + "/" + TotalTo + ")" + fileName);
        //            _logger.Information("---------------------------------------------------------------------------------------------");
        //            throw ex;
        //        }
        //    }

        //    return returnValue;
        //}

        private void UpdateSpecimenDetails(ReportInfo reportDetails, ReturnValues returnValues, MySqlTransaction trans, bool isBackfill)
        {
            string sqlQuery = string.Empty; // "UPDATE donor_test_info SET is_reverse_entry = b'1', screening_time = @ScreeningTime ,test_status= @TestStatus WHERE donor_test_info_id = @DonorTestInfoId";
            if (isBackfill == true)
            {
                sqlQuery = "UPDATE donor_test_info SET is_reverse_entry = b'1', screening_time = @ScreeningTime ,test_status= @TestStatus WHERE donor_test_info_id = @DonorTestInfoId";
            }
            else
            {
                sqlQuery = "UPDATE donor_test_info SET is_reverse_entry = b'0', screening_time = @ScreeningTime ,test_status= @TestStatus WHERE donor_test_info_id = @DonorTestInfoId";
            }

            MySqlParameter[] param = new MySqlParameter[3];

            DonorRegistrationStatus testStatus = DonorRegistrationStatus.Processing;

            param[0] = new MySqlParameter("@DonorTestInfoId", returnValues.DonorTestInfoId);

            if (reportDetails.SpecimenCollectionDate != string.Empty)
            {
                if (reportDetails.SpecimenCollectionDate != null && reportDetails.SpecimenCollectionDate != string.Empty)
                {
                    param[1] = new MySqlParameter("@ScreeningTime", DateTime.ParseExact(reportDetails.SpecimenCollectionDate.Substring(0, 8), "yyyyMMdd", CultureInfo.InvariantCulture));
                }
                else
                {
                    param[1] = new MySqlParameter("@ScreeningTime", null);
                }
            }
            else
            {
                param[1] = new MySqlParameter("@ScreeningTime", null);
            }

            param[2] = new MySqlParameter("@TestStatus", testStatus);

            SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlQuery, param);

            sqlQuery = "UPDATE donor_test_info_test_categories SET specimen_id = @SpecimenId WHERE test_category_id = 1 AND donor_test_info_id = @DonorTestInfoId";

            param = new MySqlParameter[2];

            param[0] = new MySqlParameter("@DonorTestInfoId", returnValues.DonorTestInfoId);
            param[1] = new MySqlParameter("@SpecimenId", reportDetails.SpecimenId);

            _logger.Debug($"Setting SpecimenId to {reportDetails.SpecimenId} in donor_test_info_test_categories for test category 1, donor test info id { returnValues.DonorTestInfoId}");

            SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlQuery, param);
        }


        public void UpdateDonorPIDsfromDonorTable()
        {

            try
            {
                string sqlQuery = @"
insert into individual_pids
( donor_id, pid, pid_type_id, individual_pid_type_description, mask_pid)
select d.donor_id, d.donor_ssn, 1,""SSN"",1 from donors d
left outer join individual_pids i on i.donor_id = d.donor_id
left outer join backend_donor_link bdl on bdl.donor_id_old = d.donor_id
where i.individual_pid_id is null and d.donor_ssn is not null and bdl.donor_id_old is null;
                ";
                ParamHelper _param = new ParamHelper();

                using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
                {
                    conn.Open();

                    MySqlTransaction trans = conn.BeginTransaction();

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, _param.Params);
                    trans.Commit();

                }

            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                _logger.Error(ex.InnerException.ToString());
                throw;
            }
        }

    }
}