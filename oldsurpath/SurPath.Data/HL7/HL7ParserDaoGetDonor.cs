using MySql.Data.MySqlClient;
using Serilog;
using SurPath.Entity;
using SurPath.Enum;
using SurPath.MySQLHelper;
using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace SurPath.Data
{
    /// <summary>
    /// Gets Donor
    /// </summary>
    public partial class HL7ParserDao : DataObject
    {


        private DonorTestInfo GetSpecimenDetails(ReturnValues returnValues, ReportInfo reportDetails, MySqlTransaction trans, BackendParserHelper backendParserHelper)
        {
            // This should be our first test, if it matches, we get donor, client, etc. 
            _logger.Information($"Getting Specimen Details for: {reportDetails.SpecimenId} [GetSpecimenDetails]");

            DonorTestInfo donorTestInfo = new DonorTestInfo();
            string sqlQuery = @"
select 
  donor_test_info.donor_test_info_id AS DonorTestInfoId 
  ,donor_test_info_test_categories.specimen_id AS SpecimenId
  ,donor_test_info.mro_type_id AS MROTypeId
  ,donor_test_info.donor_id as DonorID
  ,donor_test_info.client_id as ClientID
  ,donor_test_info.client_department_id as ClientDepartmentId
  ,clients.client_name as ClientName
  ,client_departments.department_name as ClientDepartmentName
from donor_test_info
  INNER JOIN donor_test_info_test_categories ON donor_test_info_test_categories.donor_test_info_id = donor_test_info.donor_test_info_id
  left outer join clients on clients.client_id = donor_test_info.client_id
  left outer join client_departments on donor_test_info.client_department_id = client_departments.client_department_id
WHERE donor_test_info_test_categories.specimen_id = @SpecimenId;
";

            MySqlParameter[] param = new MySqlParameter[1];

            param[0] = new MySqlParameter("@SpecimenId", reportDetails.SpecimenId);

            MySqlDataReader dr = SqlHelper.ExecuteReader(trans, CommandType.Text, sqlQuery, param);

            if (dr.Read())
            {
                donorTestInfo.DonorTestInfoId = Convert.ToInt32(dr["DonorTestInfoId"]);
                reportDetails.SpecimenId = dr["SpecimenId"].ToString();
                reportDetails.DonorId = (int)dr["DonorID"];
                reportDetails.ClientId = (int)dr["ClientID"];
                reportDetails.ClientDepartmentId = (int)dr["ClientDepartmentId"];
                reportDetails.ClientName = dr["ClientName"].ToString();
                reportDetails.ClientDepartmentName = dr["ClientDepartmentName"].ToString();

                backendParserHelper.ReportHelperItem.donor_found_by_specimenID = true;
                backendParserHelper.ReportHelperItem.DonorFound = true;
                backendParserHelper.ReportHelperItem.SpecimenIDMatched = true;

                returnValues.DonorId = (int)dr["DonorID"];

                returnValues.DonorTestInfoId = donorTestInfo.DonorTestInfoId;
                returnValues.MROType = dr["MROTypeId"] != DBNull.Value ? (ClientMROTypes)(Convert.ToInt32(dr["MROTypeId"].ToString())) : ClientMROTypes.None;

                _logger.Information($"Getting Specimen Results for: {reportDetails.SpecimenId} - Found donor test info record [GetSpecimenDetails]");

            }
            else
            {
                backendParserHelper.ReportHelperItem.SpecimenIDMatched = false;
                _logger.Information($"Getting Specimen Results for: {reportDetails.SpecimenId} - Donor test info record NOT FOUND [GetSpecimenDetails]");

            }
            dr.Close();
            return donorTestInfo;
        }

        private DonorTestInfo GetSpecimenDetailsUsingFormfoxOrder(ReturnValues returnValues, ReportInfo reportDetails, MySqlTransaction trans, BackendParserHelper backendParserHelper)
        {
            // This should be our first test, if it matches, we get donor, client, etc. 
            _logger.Information($"(GetSpecimenDetailsUsingFormfoxOrder) Getting Specimen Details for: {reportDetails.SpecimenId} [GetSpecimenDetailsUsingFormfoxOrder]");

            DonorTestInfo donorTestInfo = new DonorTestInfo();
            string sqlQuery = @"
select 
  donor_test_info.donor_test_info_id AS DonorTestInfoId 
  ,backend_formfox_orders.SpecimenID AS SpecimenId
  ,donor_test_info.mro_type_id AS MROTypeId
  ,donor_test_info.donor_id as DonorID
  ,donor_test_info.client_id as ClientID
  ,donor_test_info.client_department_id as ClientDepartmentId
  ,clients.client_name as ClientName
  ,client_departments.department_name as ClientDepartmentName
from donor_test_info
    inner join backend_formfox_orders on backend_formfox_orders.donor_test_info_id = donor_test_info.donor_test_info_id
  INNER JOIN donor_test_info_test_categories ON donor_test_info_test_categories.donor_test_info_id = donor_test_info.donor_test_info_id
  left outer join clients on clients.client_id = donor_test_info.client_id
  left outer join client_departments on donor_test_info.client_department_id = client_departments.client_department_id

WHERE backend_formfox_orders.SpecimenID = @SpecimenId;
";

            //MySqlParameter[] param = new MySqlParameter[1];
            ParamHelper _param = new ParamHelper();

            _param.Param = new MySqlParameter("@SpecimenId", reportDetails.SpecimenId);
            //_param.Param = new MySqlParameter("@ReferenceTestID", "");
            MySqlDataReader dr = SqlHelper.ExecuteReader(trans, CommandType.Text, sqlQuery, _param.Params);

            if (dr.Read())
            {
                donorTestInfo.DonorTestInfoId = Convert.ToInt32(dr["DonorTestInfoId"]);
                reportDetails.SpecimenId = dr["SpecimenId"].ToString();
                reportDetails.DonorId = (int)dr["DonorID"];
                reportDetails.ClientId = (int)dr["ClientID"];
                reportDetails.ClientDepartmentId = (int)dr["ClientDepartmentId"];
                reportDetails.ClientName = dr["ClientName"].ToString();
                reportDetails.ClientDepartmentName = dr["ClientDepartmentName"].ToString();

                backendParserHelper.ReportHelperItem.donor_found_by_specimenID = true;
                backendParserHelper.ReportHelperItem.DonorFound = true;
                backendParserHelper.ReportHelperItem.SpecimenIDMatched = true;

                returnValues.DonorId = reportDetails.DonorId;
                returnValues.DonorTestInfoId = donorTestInfo.DonorTestInfoId;
                returnValues.ClientDepartmentId = reportDetails.ClientDepartmentId;
                returnValues.ClientId = reportDetails.ClientId;

                returnValues.MROType = dr["MROTypeId"] != DBNull.Value ? (ClientMROTypes)(Convert.ToInt32(dr["MROTypeId"].ToString())) : ClientMROTypes.None;

                _logger.Information($"Getting Specimen Results for: {reportDetails.SpecimenId} - Found donor test info record [GetSpecimenDetailsUsingFormfoxOrder]");

            }
            else
            {
                backendParserHelper.ReportHelperItem.SpecimenIDMatched = false;
                _logger.Information($"Getting Specimen Results for: {reportDetails.SpecimenId} - Donor test info record NOT FOUND [GetSpecimenDetailsUsingFormfoxOrder]");

            }
            dr.Close();
            return donorTestInfo;
        }


        private void GetDonorDetails(ReportInfo reportDetails, ReturnValues returnValues, MySqlTransaction trans, ref BackendParserHelper backendParserHelper)
        {
            _logger.Information($"Getting Donor Details for: {reportDetails.SpecimenId}");
            int daysToAutoMatch = Convert.ToInt32(ConfigurationManager.AppSettings["DaysToAutoMatch"].ToString().Trim()) * -1;
            string reportDate = reportDetails.LabReportDate.Substring(0, 4) + "-" + reportDetails.LabReportDate.Substring(4, 2) + "-" + reportDetails.LabReportDate.Substring(6, 2);
            // check if our PID exists for multiple donors
            // if so, we can't match a donor because we could match to the wrong one.
            // we need a constraint on PIDs but can't implement
            // because 111-11-1111 is valid for foreign background check donors (per david)


            //            // Match by PID and always get the newest donor in case there are duplicates
            //            // check SSN as well for legacy data, just in case
            //            string sqlQuery = "SELECT "
            //                                + "donor_id AS DonorId "
            //                                + "FROM donors "
            //                                + "WHERE is_archived = 0 AND (donor_pid = @donor_pid OR donor_ssn = @donor_pid)" +
            //                                " order by donor_id desc limit 1";  // this gets the latest record only

            //            // the unverified is for backfilled accounts created by mismatch holder client
            //            sqlQuery = @"
            //                    select d.* from individual_pids ip
            //                    left outer join donors d on d.donor_id = ip.donor_id
            //                    where ip.pid = @donor_pid and (d.is_archived = 0 OR (d.is_archived = 1 AND d.unverified=1)) and d.donor_id is not null
            //                    order by d.donor_id desc limit 1;
            //";

            // allow multiple results - if we get more than one, it's a mismatch and we have to rely on specimen id
            // or we could associate with wrong donor
            // we use donor test info to exclude tests in the past in case the donor
            // has two donor accounts and one test has been completed in the past.
            // if we've matched a client & department, we should filter based on that.
            string sqlQuery = @"
                    select d.*, dti.donor_test_info_id, dti.test_requested_date, dti.client_id, dti.client_department_id
                    from individual_pids ip
                    left outer join donors d on d.donor_id = ip.donor_id
                    left outer join donor_test_info dti on d.donor_id = dti.donor_id
                    left outer join backend_donor_link bdl on bdl.donor_id_old = d.donor_id
                    where 
                    (
                        ip.pid = @donor_pid 
                        or 
                        ip.pid = @donor_pid2
                    )
                    and 
                    (
                        d.is_archived = 0 
                        OR 
                            (
                                d.is_archived = 1 
                                AND 
                                d.unverified=1
                            )
                    ) 
                    and 
                    d.donor_id is not null
                    AND DATE_ADD(DATE(@ReportDate), INTERVAL @DaysToAutoMatch DAY) <= DATE(test_requested_date)
                    and dti.test_status !=7
                    and bdl.donor_id_old is null
                    and ( dti.client_id = @client_id OR @client_id <0)
                    and ( dti.client_department_id = @client_department_id OR @client_department_id < 0)
                    order by d.donor_id desc;
";



            ParamHelper p = new ParamHelper();
            p.Param = new MySqlParameter("@donor_pid", reportDetails.PID);
            p.Param = new MySqlParameter("@ReportDate", reportDate);
            p.Param = new MySqlParameter("@DaysToAutoMatch", daysToAutoMatch);



            string donor_pid_alternative = reportDetails.PID;

            // if we have the client and dept id - let's restrict to that
            if (reportDetails.ClientId > 0 && reportDetails.ClientDepartmentId > 0)
            {
                p.Param = new MySqlParameter("@client_id", reportDetails.ClientId);
                p.Param = new MySqlParameter("@client_department_id", reportDetails.ClientDepartmentId);

            }
            else
            {
                p.Param = new MySqlParameter("@client_id", -1);
                p.Param = new MySqlParameter("@client_department_id", -1);
            }
            if (reportDetails.PIDType == (int)PidTypes.SSN)
            {
                _logger.Debug($"PIDType is SSN");
                // If the PID
                Regex regex;
                regex = new Regex("^\\d{3}-\\d{2}-\\d{4}$");
                bool _PIDHasDashes = regex.IsMatch(donor_pid_alternative);
                if (_PIDHasDashes)
                {
                    // A SSN with dashes, remove them
                    donor_pid_alternative = new String(donor_pid_alternative.Where(Char.IsDigit).ToArray());
                }
                else
                {
                    // 9 digits, considered SSN, no dashes, add them to try and match donor
                    donor_pid_alternative = donor_pid_alternative.Insert(5, "-").Insert(3, "-");
                }
                p.Param = new MySqlParameter("@donor_pid2", donor_pid_alternative);


            }
            else if (reportDetails.PIDType == (int)PidTypes.QuestDonorId)
            {
                _logger.Debug("PIDType is quest donor id");
                // SSNID is in use:
                donor_pid_alternative = reportDetails.SsnId;
                Regex regex;
                regex = new Regex("^\\d{3}-\\d{2}-\\d{4}$");
                bool _PIDHasDashes = regex.IsMatch(donor_pid_alternative);
                if (!_PIDHasDashes == true)
                {
                    _logger.Debug("Adding dashes to PID, because it is quest and an SSN");
                    donor_pid_alternative = donor_pid_alternative.Insert(5, "-").Insert(3, "-");
                }
                else
                {
                    _logger.Debug("_PIDHasDashes already has dashes");
                }
                p.Param = new MySqlParameter("@donor_pid2", donor_pid_alternative);

            }
            else
            {
                _logger.Debug($"PIDType is not SSN");
                if (!string.IsNullOrEmpty(reportDetails.PID))
                {
                    _logger.Debug($"PID is not empty, using PID");
                    p.Param = new MySqlParameter("@donor_pid2", reportDetails.PID);
                }
                else
                {
                    _logger.Debug($"PID is empty, using PID_DASHES_19");
                    p.Param = new MySqlParameter("@donor_pid2", reportDetails.PID_DASHES_19);
                }

            }

            // _logger.Debug($"GetDonorDetails Params: {reportDetails.PID}, {reportDetails.PID_DASHES_19}, {reportDate}, {daysToAutoMatch}");

            MySqlDataReader dr = SqlHelper.ExecuteReader(trans, CommandType.Text, sqlQuery, p.Params);
            int RowCount = 0;
            bool same_name = true;
            string donor_name = string.Empty;

            while (dr.Read())
            {
                if (returnValues.ClientId != 0 && returnValues.ClientDepartmentId != 0)
                {
                    // we know the client id - so we only want to check donors for this dept.
                    _logger.Debug("we know the client id - so we only want to check donors for this dept.");
                    int _client_id = (int)dr["client_id"];
                    int _client_department_id = (int)dr["client_department_id"];
                    if (returnValues.ClientId == _client_id && returnValues.ClientDepartmentId == _client_department_id)
                    {
                        RowCount++;
                        returnValues.DonorId = Convert.ToInt32(dr["donor_id"]);
                        reportDetails.DonorId = returnValues.DonorId;

                        //string _donor_name = (string)dr["donor_first_name"].ToString() + " " + (string)dr["donor_mi"].ToString() + " " + (string)dr["donor_last_name"].ToString();
                        string _donor_name = (string)dr["donor_first_name"].ToString() + " " + (string)dr["donor_last_name"].ToString();

                        _logger.Debug($"Donor Found - Donor_id = {returnValues.DonorId} {_donor_name}");
                        if (RowCount == 1)
                        {
                            _logger.Debug($"First Donor found {_donor_name}");
                            // first row - store the temp name for comparison
                            donor_name = _donor_name;
                        }
                        else
                        {
                            _logger.Debug($"Another donor found {_donor_name}");
                            // if we get a different name, we set this to false and never check again
                            if (same_name == true)
                            {
                                _logger.Debug($"Haven't found a donor with a different name yet");
                                if (!donor_name.Equals(_donor_name, StringComparison.InvariantCultureIgnoreCase))
                                {
                                    _logger.Debug($"This donor doesn't match the last donor name we found {donor_name} is different than {_donor_name}");
                                    same_name = false;
                                }
                            }

                        }
                    }

                }
                else
                {
                    // we don't know the client id so we have to check them all
                    _logger.Debug("we don't know the client id so we have to check them all");
                    RowCount++;
                    returnValues.DonorId = Convert.ToInt32(dr["donor_id"]);
                    reportDetails.DonorId = returnValues.DonorId;

                    //string _donor_name = (string)dr["donor_first_name"].ToString() + " " + (string)dr["donor_mi"].ToString() + " " + (string)dr["donor_last_name"].ToString();
                    string _donor_name = (string)dr["donor_first_name"].ToString() + " " + (string)dr["donor_last_name"].ToString();

                    _logger.Debug($"Donor Found - Donor_id = {returnValues.DonorId} {_donor_name}");
                    if (RowCount == 1)
                    {
                        _logger.Debug($"First Donor found {_donor_name}");
                        // first row - store the temp name for comparison
                        donor_name = _donor_name;
                    }
                    else
                    {
                        _logger.Debug($"Another donor found {_donor_name}");
                        // if we get a different name, we set this to false and never check again
                        if (same_name == true)
                        {
                            _logger.Debug($"Haven't found a donor with a different name yet");
                            if (!donor_name.Equals(_donor_name, StringComparison.InvariantCultureIgnoreCase))
                            {
                                _logger.Debug($"This donor doesn't match the last donor name we found {donor_name} is different than {_donor_name}");
                                same_name = false;
                            }
                        }

                    }
                }



            }
            dr.Close();


            backendParserHelper.ReportHelperItem.donor_pid_match_count = RowCount;


            if (RowCount > 1)
            {
                _logger.Information($"Multiple donors matched! Same name? {same_name.ToString()}");
                // this pid is associated with multiple donors
                // no way to tell which one
                backendParserHelper.ReportHelperItem.multiple_donor_pid_found = true;

                returnValues.DonorId = 0;
                reportDetails.DonorId = 0;

                // if we got multiple of the same donor, we report that
                if (same_name == true)
                {
                    _logger.Information($"Same donor mutiple times (same name and pid) {donor_name}");
                    backendParserHelper.ReportHelperItem.donor_name = donor_name;
                }
                else
                {
                    backendParserHelper.ReportHelperItem.donor_name = string.Empty;
                }


            }
            if (returnValues.DonorId == 0 && reportDetails.DonorId == 0 && !(string.IsNullOrEmpty(reportDetails.SpecimenId)))
            {
                Donor donor = GetDonorDetails(reportDetails.SpecimenId);
                if (donor != null)
                {
                    backendParserHelper.ReportHelperItem.donor_found_by_specimenID = true;
                    returnValues.DonorId = donor.DonorId;
                    reportDetails.DonorId = donor.DonorId;
                }

            }
            if (reportDetails.DonorId > 0)
            {
                backendParserHelper.ReportHelperItem.DonorFound = true;
            }
        }
        // try by specimenID
        public Donor GetDonorDetails(string specimenId)
        {
            Donor donor = null;

            string sqlQuery = "SELECT "
                                + "donor_id AS DonorId, "
                                + "donorClearStarProfId AS DonorClearStarProfId, "
                                + "donor_first_name AS DonorFirstName, "
                                + "donor_mi AS DonorMI, "
                                + "donor_last_name AS DonorLastName, "
                                + "donor_suffix AS DonorSuffix, "
                                + "donor_ssn AS DonorSSN, "
                                + "donor_pid AS donor_pid, "
                                + "donor_date_of_birth AS DonorDateOfBirth, "
                                + "donor_phone_1 AS DonorPhone1, "
                                + "donor_phone_2 AS DonorPhone2, "
                                + "donor_address_1 AS DonorAddress1, "
                                + "donor_address_2 AS DonorAddress2, "
                                + "donor_city AS DonorCity, "
                                + "donor_state AS DonorState, "
                                + "donor_zip AS DonorZip, "
                                + "donor_email AS DonorEmail, "
                                + "donor_gender AS DonorGender, "
                                + "donor_initial_client_id AS DonorInitialClientId, "
                                + "donor_initial_department_id AS DonorInitialDepartmentId, "
                                + "donor_registration_status AS DonorRegistrationStatus, "
                                + "is_synchronized AS IsSynchronized, "
                                + "is_archived AS IsArchived, "
                                + "created_on AS CreatedOn, "
                                + "created_by AS CreatedBy, "
                                + "last_modified_on AS LastModifiedOn, "
                                + "last_modified_by AS LastModifiedBy "
                                + "FROM donors WHERE donor_id = ("
                                + "SELECT distinct "
                                + "donor_id "
                                + "FROM donor_test_info "
                                + "INNER JOIN donor_test_info_test_categories ON donor_test_info_test_categories.donor_test_info_id = donor_test_info.donor_test_info_id "
                                + "WHERE donor_test_info_test_categories.specimen_id = @SpecimenId)";

            //_logger.Debug("BEGIN SQL QUERY");
            //_logger.Debug("");
            //_logger.Debug(sqlQuery);
            //_logger.Debug("");
            //_logger.Debug("END SQL QUERY");

            // Super wierd scenario where student Erica Ortega took one test but we got two files
            // so for now, till I know why, I'm going to check the sub query count and abort if we get more than one
            var DupSqlTest = "SELECT distinct "
                                + "donor_id "
                                + "FROM donor_test_info "
                                + "INNER JOIN donor_test_info_test_categories ON donor_test_info_test_categories.donor_test_info_id = donor_test_info.donor_test_info_id "
                                + "WHERE donor_test_info_test_categories.specimen_id = @SpecimenId";
            var ThereCanBeOnlyOne = 0;

            bool safeToCheckBYSpecimenId = false;
            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                MySqlParameter[] param = new MySqlParameter[1];
                param[0] = new MySqlParameter("@SpecimenId", specimenId);
                MySqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, DupSqlTest, param);
                while (dr.Read())
                {
                    ThereCanBeOnlyOne++;
                    _logger.Debug($"ThereCanBeOnlyOne {ThereCanBeOnlyOne}");

                }
                safeToCheckBYSpecimenId = ThereCanBeOnlyOne == 1;
            }
            if (safeToCheckBYSpecimenId)
            {
                using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
                {
                    MySqlParameter[] param = new MySqlParameter[1];
                    param[0] = new MySqlParameter("@SpecimenId", specimenId);

                    MySqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                    if (dr.Read())
                    {
                        donor = new Donor();

                        donor.DonorId = (int)dr["DonorId"];
                        donor.DonorFirstName = (string)dr["DonorFirstName"];
                        donor.DonorMI = dr["DonorMI"].ToString();
                        donor.DonorLastName = (string)dr["DonorLastName"];
                        donor.DonorSuffix = dr["DonorSuffix"].ToString();
                        donor.DonorSSN = dr["DonorSSN"].ToString();
                        donor.DonorDateOfBirth = dr["DonorDateOfBirth"] != DBNull.Value ? Convert.ToDateTime(dr["DonorDateOfBirth"], this.Culture) : DateTime.MinValue;
                        donor.DonorPhone1 = dr["DonorPhone1"].ToString();
                        donor.DonorPhone2 = dr["DonorPhone2"].ToString();
                        donor.DonorAddress1 = dr["DonorAddress1"].ToString();
                        donor.DonorAddress2 = dr["DonorAddress2"].ToString();
                        donor.DonorCity = dr["DonorCity"].ToString();
                        donor.DonorState = dr["DonorState"].ToString();
                        donor.DonorZip = dr["DonorZip"].ToString();
                        donor.DonorEmail = dr["DonorEmail"].ToString();
                        donor.DonorGender = dr["DonorGender"] != DBNull.Value ? (Gender)(Convert.ToInt32(dr["DonorGender"].ToString())) : Gender.None;
                        donor.DonorInitialClientId = dr["DonorInitialClientId"] != null && dr["DonorInitialClientId"].ToString() != string.Empty ? (int)dr["DonorInitialClientId"] : 0;
                        donor.DonorInitialDepartmentId = dr["DonorInitialDepartmentId"] != null && dr["DonorInitialDepartmentId"].ToString() != string.Empty ? (int)dr["DonorInitialDepartmentId"] : 0;
                        donor.DonorRegistrationStatusValue = dr["DonorRegistrationStatus"] != null ? (DonorRegistrationStatus)dr["DonorRegistrationStatus"] : DonorRegistrationStatus.None;
                        donor.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                        donor.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                        donor.CreatedOn = (DateTime)dr["CreatedOn"];
                        donor.CreatedBy = (string)dr["CreatedBy"];
                        donor.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                        donor.LastModifiedBy = (string)dr["LastModifiedBy"];
                    }
                }
            }
            else
            {
                if (ThereCanBeOnlyOne>1)
                {
                    _logger.Error($"ThereCanBeOnlyOne safety check fail!!!!! Issue with Specimen ID: {specimenId}");

                }
            }


            return donor;
        }
    }
}