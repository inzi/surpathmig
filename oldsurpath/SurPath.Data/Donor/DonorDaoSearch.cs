using MySql.Data.MySqlClient;
using SurPath.Enum;
using SurPath.MySQLHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace SurPath.Data
{
    public partial class DonorDao : DataObject
    {
        public DataTable DonorSearch(Dictionary<string, string> searchParam)
        {
            string sqlQuery = "SELECT "
                            + "donors.donor_id AS DonorId, "
                            + "donorClearStarProfId AS DonorClearStarProfId, "
                            + "donors.donor_first_name AS DonorFirstName, "
                            + "donors.donor_last_name AS DonorLastName, "
                            + "donors.donor_ssn AS DonorSSN, "
                            + "donors.donor_pid AS donor_pid, "
                            + "donors.donor_date_of_birth AS DonorDateOfBirth, "
                            + "donors.donor_city AS DonorCity, "
                            + "donors.donor_email AS DonorEmail, "
                            + "donors.donor_zip AS DonorZip, "
                            + "donors.is_archived AS IsDonorArchived "
                            + "FROM donors "
                            + "WHERE 1=1 AND donors.donor_registration_status >=3 ";

            List<MySqlParameter> param = new List<MySqlParameter>();

            foreach (KeyValuePair<string, string> searchItem in searchParam)
            {
                if (searchItem.Key == "DonorId")
                {
                    sqlQuery += "AND donors.donor_id = @DonorId ";
                    param.Add(new MySqlParameter("@DonorId", searchItem.Value));
                }
                else if (searchItem.Key == "FirstName")
                {
                    sqlQuery += "AND donors.donor_first_name LIKE @DonorFirstName ";
                    param.Add(new MySqlParameter("@DonorFirstName", searchItem.Value));
                }
                else if (searchItem.Key == "LastName")
                {
                    sqlQuery += "AND donors.donor_last_name LIKE @DonorLastName ";
                    param.Add(new MySqlParameter("@DonorLastName", searchItem.Value));
                }
                else if (searchItem.Key == "SSN")
                {
                    sqlQuery += "AND donors.donor_ssn LIKE @DonorSSN ";
                    param.Add(new MySqlParameter("@DonorSSN", searchItem.Value));
                }
                else if (searchItem.Key == "DOB")
                {
                    sqlQuery += "AND CAST(donors.donor_date_of_birth AS DATE) = CAST(@DonorDateOfBirth AS DATE) ";
                    param.Add(new MySqlParameter("@DonorDateOfBirth", Convert.ToDateTime(searchItem.Value).ToString("yyyy-MM-dd")));
                }
                else if (searchItem.Key == "City")
                {
                    sqlQuery += "AND donors.donor_city LIKE @DonorCity ";
                    param.Add(new MySqlParameter("@DonorCity", searchItem.Value));
                }
                else if (searchItem.Key == "ZipCode")
                {
                    sqlQuery += "AND donors.donor_zip = @DonorZipCode ";
                    param.Add(new MySqlParameter("@DonorZipCode", searchItem.Value));
                }
                else if (searchItem.Key == "Email")
                {
                    sqlQuery += "AND donors.donor_email = @DonorEmail ";
                    param.Add(new MySqlParameter("@DonorEmail", searchItem.Value));
                }
                else if (searchItem.Key == "IncludeArchive")
                {
                    if (!Convert.ToBoolean(searchItem.Value))
                    {
                        sqlQuery += "AND donors.is_archived = b'0' ";
                    }
                }
            }

            sqlQuery += "ORDER BY donors.donor_first_name, donors.donor_last_name";

            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sqlQuery, param.ToArray());

            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        public DataTable SearchFromVendorDashboard(Dictionary<string, string> searchParam)
        {
            string searchSql = searchParam["searchSql"];
            string sqlQuery = "SELECT "
                                + "donors.donor_id AS DonorId, "
                                + "donorClearStarProfId AS DonorClearStarProfId, "
                                + "donors.donor_first_name AS DonorFirstName, "
                                + "donors.donor_last_name AS DonorLastName, "
                                + "donor_test_info.form_type_id AS DonorFormType,"
                                + "donor_test_info.schedule_date AS ScheduleDate,"
                                + "donor_test_info.donor_test_info_id AS DonorTestInfoID,"
                                + "GROUP_CONCAT(donor_test_info_test_categories.test_category_id) AS DonorTestType,"
                                //+ "donor_test_info_test_categories.test_category_id AS DonorTestType,"
                                + "donors.donor_ssn AS DonorSSN, "
                                + "donors.donor_pid AS donor_pid, "
                                + "donors.donor_date_of_birth AS DonorDateOfBirth, "
                                + "donors.donor_city AS DonorCity, "
                                + "donors.donor_email AS DonorEmail, "
                                + "donors.donor_zip AS DonorZip, "
                                + "donors.is_archived AS IsDonorArchived "
                                + "FROM donors "
                                + "inner join donor_test_info on donors.donor_id=donor_test_info.donor_id "
                                + "inner join donor_test_info_test_categories on donor_test_info.donor_test_info_id = donor_test_info_test_categories.donor_test_info_id "
                                + "inner join users on donor_test_info.collection_site_1_id =users.vendor_id "
                                + "or donor_test_info.collection_site_2_id = users.vendor_id "
                                + "or donor_test_info.collection_site_3_id = users.vendor_id "
                                + "or donor_test_info.collection_site_4_id = users.vendor_id "
                                + "WHERE " + searchSql + " AND donor_test_info.test_status in (3,4,5) AND donors.is_archived = b'0' ";

            List<MySqlParameter> param = new List<MySqlParameter>();

            //sqlQuery += " group by donor_test_info.donor_id";
            sqlQuery += " group by donor_test_info.donor_test_info_id";

            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sqlQuery, param.ToArray());

            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        public DataTable SearchDonor(Dictionary<string, string> searchParam, UserType userType, int currentUserId, string currentUserName, bool showAll)
        {
            string subSql = "LEFT OUTER JOIN donor_test_info ON donors.donor_id = donor_test_info.donor_id ";
            if (showAll == false)
            {
                subSql = subSql + "AND donor_test_info.donor_test_info_id = (SELECT MAX(donor_test_info.donor_test_info_id) FROM donor_test_info WHERE donor_test_info.donor_id = donors.donor_id) ";
            }

            string sqlQuery = "SELECT "
                            + "donors.donor_id AS DonorId, "
                            + "donorClearStarProfId AS DonorClearStarProfId, "
                            + "donors.donor_first_name AS DonorFirstName, "
                            + "donors.donor_last_name AS DonorLastName, "
                            + "donors.donor_ssn AS DonorSSN, "
                            + "donors.donor_pid AS donor_pid, "
                            + "donors.donor_date_of_birth AS DonorDateOfBirth, "
                            + "donors.donor_initial_client_id AS DonorInitialClientId, "
                            + "donors.donor_registration_status AS DonorRegistrationStatus, "
                            + "donors.donor_city as DonorCity, "
                            + "donors.donor_email as DonorEmail, "
                            + "donors.donor_zip as DonorZipCode, "
                            + "donor_test_info.test_requested_date as DonorTestRegisteredDate, "
                            + "donors.is_archived AS IsDonorArchived, "
                            + "donor_test_info.donor_test_info_id AS DonorTestInfoId, "
                            + "donor_test_info.client_id AS TestInfoClientId, "
                            + "donor_test_info.client_department_id AS TestInfoDepartmentId, "
                            + "donor_test_info.mro_type_id AS MROTypeId, "
                            + "donor_test_info.payment_type_id AS PaymentTypeId, "
                            + "donor_test_info.test_requested_date AS TestRequestedDate, "
                            + "donor_test_info.reason_for_test_id AS ReasonForTestId, "
                            + "donor_test_info.is_walkin_donor AS IsWalkinDonor, "
                            + "donor_test_info.instant_test_result AS InstantTestResult, "
                            + "donor_test_info.is_instant_test AS IsInstantTest, "
                            + "donor_test_info.total_payment_amount AS TotalPaymentAmount, "
                            + "donor_test_info.payment_method_id AS PaymentMethodId, "
                            + "donor_test_info.payment_received AS PaymentReceived, "
                            + "donor_test_info.payment_date AS PaymentDate, "
                            + "donor_test_info.test_status AS TestStatus, "
                            + "donor_test_info.test_overall_result AS TestOverallResult, "
                            //+ "bn.notified_by_email as Notified_by_email, "
                            + "if (bn.notified_by_email is null, 'Old Data',IF (bn.notified_by_email = 0, 'Not Notified',bn.notified_by_email_timestamp)) as Notified_by_email_timestamp, "
                            + "if (bn.backend_notifications_id is null, 0, bn.backend_notifications_id) as backend_notifications_id, "
                            // if use_formox is enabled and 
                            //+ "if (bffo.ReferenceTestID is null AND bnwd.use_formfox >0, 'FF Not Used','FF Used') as Notified_via_FormFox, "
                            + "if (bnwd.use_formfox <1, 'Not FF Enabled',if (bffo.ReferenceTestID is null, 'FF Not Used',CONCAT('FF Used',' (',bffo.ReferenceTestID,')'))) as Notified_via_FormFox, "
                            + "if (bnwd.backend_notification_window_data_id is null, 0, bnwd.backend_notification_window_data_id) as backend_notification_window_data_id, "
                            + "CONCAT_WS(' ', users.user_first_name, users.user_last_name) AS CollectorName, "
                            //+ "donor_test_info_test_categories.test_category_id AS TestCategoryId, "
                            //+ "donor_test_info_test_categories.test_panel_id AS TestPanelId, "
                            //+ "donor_test_info_test_categories.specimen_id AS SpecimenId, "
                            + "(select GROUP_CONCAT(donor_test_info_test_categories.specimen_id SEPARATOR ', ') from donor_test_info_test_categories where donor_test_info_test_categories.donor_test_info_id = donor_test_info.donor_test_info_id) AS SpecimenId, "
                            + "donor_test_info.screening_time AS SpecimenDate, "
                           //+ "donor_test_info_test_categories.hair_test_panel_days AS HairTestPanelDays, "
                           + "clients.client_name AS ClientName, "
                            + "client_departments.department_name AS ClientDepartmentName, "
                            + "client_departments.clearstarcode As ClearStarCode, "
                            + "dt.DocsTotal, dt.DocsNotApproved, dt.DocsRejected "
                            + "FROM donors "
                            + " " + subSql + " "
                    //+ "LEFT OUTER JOIN donor_test_info ON donors.donor_id = donor_test_info.donor_id "
                    //+ "LEFT OUTER JOIN donor_test_info_test_categories ON donor_test_info.donor_test_info_id = donor_test_info_test_categories.donor_test_info_id "
                    + "LEFT OUTER JOIN clients ON (donor_test_info.client_id = clients.client_id) " //(donors.donor_initial_client_id = clients.client_id AND donors.donor_initial_department_id IS NULL) OR
                                                                                                    // + "LEFT OUTER JOIN clients ON donors.donor_initial_client_id = clients.client_id AND donors.donor_initial_department_id IS NULL "
                            + "LEFT OUTER JOIN client_departments ON donor_test_info.client_department_id = client_departments.client_department_id "
                            + "LEFT OUTER JOIN users ON donor_test_info.collection_site_user_id = users.user_id "
                            + "LEFT OUTER JOIN donordocumenttotals dt on dt.donor_id = donor_test_info.donor_id "
                            + "LEFT OUTER JOIN backend_notifications bn on bn.donor_test_info_id = donor_test_info.donor_test_info_id "
                            + "LEFT OUTER JOIN backend_formfox_orders bffo on bffo.donor_test_info_id = donor_test_info.donor_test_info_id "
                            + "LEFT OUTER JOIN backend_notification_window_data bnwd on bnwd.client_id = donor_test_info.client_id and bnwd.client_department_id = donor_test_info.client_department_id "
                            + "WHERE 1=1 AND test_status >3 "; // AND donors.donor_registration_status != 2  ";

            if (showAll == false) sqlQuery = sqlQuery + "AND donors.donor_registration_status != 2  ";

            List<MySqlParameter> param = new List<MySqlParameter>();

            foreach (KeyValuePair<string, string> searchItem in searchParam)
            {
                // Filters
                //searchParam.Add("cbFilter", cbFilter.SelectedValue.ToString());
                //searchParam.Add("dtAfterFilter", dtAfterFilter.ToString());
                //searchParam.Add("dtBeforeFilter", dtBeforeFilter.ToString());
                if (searchItem.Key == "cbFilter")
                {
                    string _dtAfterFilter = searchParam["dtAfterFilter"];
                    string _dtBeforeFilter = searchParam["dtBeforeFilter"];


                    if (searchItem.Value.ToUpper().Contains("tested".ToUpper()))
                    {
                        sqlQuery += $"AND(donor_test_info.screening_time between '{_dtAfterFilter}' and '{_dtBeforeFilter}') ";
                        // AND(donor_test_info.screening_time between '2020-8-1' and '2021-1-1')
                    }
                    if (searchItem.Value.ToUpper().Contains("registration".ToUpper()))
                    {
                        sqlQuery += $"AND(donor_test_info.test_requested_date between '{_dtAfterFilter}' and '{_dtBeforeFilter}') ";
                    }

                }


                if (searchItem.Key == "DonorId")
                {
                    sqlQuery += "AND donors.donor_id = @DonorId ";
                    param.Add(new MySqlParameter("@DonorId", searchItem.Value));
                }
                else if (searchItem.Key == "DonorTestInfoId")
                {
                    sqlQuery += "AND donor_test_info.donor_test_info_id = @DonorTestInfoId ";
                    param.Add(new MySqlParameter("@DonorTestInfoId", searchItem.Value));
                }
                else if (searchItem.Key == "FirstName")
                {
                    sqlQuery += "AND donors.donor_first_name LIKE @DonorFirstName ";
                    param.Add(new MySqlParameter("@DonorFirstName", searchItem.Value));
                }
                else if (searchItem.Key == "LastName")
                {
                    sqlQuery += "AND donors.donor_last_name LIKE @DonorLastName ";
                    param.Add(new MySqlParameter("@DonorLastName", searchItem.Value));
                }
                else if (searchItem.Key == "SSN")
                {
                    sqlQuery += "AND donors.donor_ssn LIKE @DonorSSN ";
                    param.Add(new MySqlParameter("@DonorSSN", searchItem.Value));
                }
                else if (searchItem.Key == "DOB")
                {
                    sqlQuery += "AND CAST(donors.donor_date_of_birth AS DATE) = CAST(@DonorDateOfBirth AS DATE) ";
                    param.Add(new MySqlParameter("@DonorDateOfBirth", Convert.ToDateTime(searchItem.Value).ToString("yyyy-MM-dd")));
                }
                else if (searchItem.Key == "City")
                {
                    sqlQuery += "AND donors.donor_city LIKE @DonorCity ";
                    param.Add(new MySqlParameter("@DonorCity", searchItem.Value));
                }
                else if (searchItem.Key == "ZipCode")
                {
                    sqlQuery += "AND donors.donor_zip = @DonorZipCode ";
                    param.Add(new MySqlParameter("@DonorZipCode", searchItem.Value));
                }
                else if (searchItem.Key == "DonorEmail")
                {
                    sqlQuery += "AND donors.donor_email = @DonorEmail ";
                    param.Add(new MySqlParameter("@DonorEmail", searchItem.Value));
                }
                else if (searchItem.Key == "SpecimenId")
                {
                    sqlQuery += "AND donor_test_info.donor_test_info_id IN (SELECT donor_test_info_test_categories.donor_test_info_id FROM donor_test_info_test_categories WHERE donor_test_info_test_categories.specimen_id LIKE @SpecimenId) ";
                    param.Add(new MySqlParameter("@SpecimenId", searchItem.Value));
                }
                else if (searchItem.Key == "TestReason")
                {
                    sqlQuery += "AND donor_test_info.reason_for_test_id = @ReasonForTestId ";
                    param.Add(new MySqlParameter("@ReasonForTestId", Convert.ToInt32(searchItem.Value)));
                }
                else if (searchItem.Key == "Client")
                {
                    sqlQuery += "AND (donors.donor_initial_client_id = @ClientId OR donor_test_info.client_id = @ClientId) ";
                    param.Add(new MySqlParameter("@ClientId", Convert.ToInt32(searchItem.Value)));
                }
                else if (searchItem.Key == "Department")
                {
                    sqlQuery += "AND donor_test_info.client_department_id = @ClientDepartmentId ";
                    param.Add(new MySqlParameter("@ClientDepartmentId", Convert.ToInt32(searchItem.Value)));
                }
                else if (searchItem.Key == "Status")
                {
                    if (searchItem.Value == "1")
                    {
                        sqlQuery += "AND donor_test_info.test_status  = @Status OR donors.donor_registration_status = 1 ";
                        param.Add(new MySqlParameter("@Status", Convert.ToInt32("3")));
                    }
                    if (searchItem.Value == "2")
                    {
                        sqlQuery += "AND donor_test_info.test_status = @Status ";
                        param.Add(new MySqlParameter("@Status", Convert.ToInt32("4")));
                    }
                    if (searchItem.Value == "3")
                    {
                        sqlQuery += "AND donor_test_info.test_status = @Status ";
                        param.Add(new MySqlParameter("@Status", Convert.ToInt32("5")));
                    }
                    if (searchItem.Value == "4")
                    {
                        sqlQuery += "AND donor_test_info.test_status = @Status ";
                        param.Add(new MySqlParameter("@Status", Convert.ToInt32("6")));
                    }
                    if (searchItem.Value == "5")
                    {
                        sqlQuery += "AND donor_test_info.test_status = @Status ";
                        param.Add(new MySqlParameter("@Status", Convert.ToInt32("7")));
                    }
                }
                else if (searchItem.Key == "TestCategory")
                {
                    sqlQuery += "AND donor_test_info.donor_test_info_id IN (SELECT donor_test_info_test_categories.donor_test_info_id FROM donor_test_info_test_categories WHERE donor_test_info_test_categories.test_category_id = @TestCategoryId) ";
                    param.Add(new MySqlParameter("@TestCategoryId", Convert.ToInt32(searchItem.Value)));
                }
                else if (searchItem.Key == "IncludeArchive")
                {
                    if (!Convert.ToBoolean(searchItem.Value))
                    {
                        sqlQuery += "AND donors.is_archived = b'0' ";
                    }
                }
                else if (searchItem.Key == "Walkin")
                {
                    if (!Convert.ToBoolean(searchItem.Value))
                    {
                        // sqlQuery += "AND donor_test_info.is_walkin_donor = b'0' OR donor_test_info.is_walkin_donor = null ";
                    }
                    else
                    {
                        sqlQuery += "AND donor_test_info.is_walkin_donor = b'1' ";
                    }
                }
                else if (searchItem.Key == "NoOfDays")
                {
                    if (!searchItem.Value.StartsWith("DR"))
                    {
                        int dateInterval = Convert.ToInt32(searchItem.Value) * -1;

                        sqlQuery += "AND donor_test_info.screening_time >= DATE_ADD(CURDATE(), INTERVAL @DateInterval DAY) ";
                        param.Add(new MySqlParameter("@DateInterval", dateInterval));
                    }
                    else
                    {
                        sqlQuery += "AND (CAST(donor_test_info.screening_time AS DATE) BETWEEN CAST(@StartDate AS DATE) AND CAST(@EndDate AS DATE)) ";
                        string[] dates = searchItem.Value.Split('#');
                        param.Add(new MySqlParameter("@StartDate", Convert.ToDateTime(dates[1]).ToString("yyyy-MM-dd")));
                        param.Add(new MySqlParameter("@EndDate", Convert.ToDateTime(dates[2]).ToString("yyyy-MM-dd")));
                    }
                }
            }

            if (!(currentUserName.ToUpper() == "ADMIN" || currentUserName.ToUpper() == "DAVID" || currentUserName.ToUpper() == "TIM"))
            {
                if ((userType == UserType.TPA && currentUserId != 1) && userType != UserType.Donor)
                {
                    sqlQuery += "AND donor_test_info.client_department_id IN (select client_department_id from user_departments where user_id = " + currentUserId.ToString() + ")";
                }

                if (userType == UserType.Vendor && currentUserId != 1)
                {
                    sqlQuery += "AND  client_departments.client_department_id IN (select client_department_id from user_departments where user_id = " + currentUserId.ToString() + ")";
                }

                if (userType == UserType.Client && currentUserId != 1)
                {
                    sqlQuery += "AND  client_departments.client_department_id IN (select client_department_id from user_departments where user_id = " + currentUserId.ToString() + ")";
                }
            }

            sqlQuery += "ORDER BY donors.donor_first_name, donors.donor_last_name";

            _logger.Debug("Search Donor Query");
            string tmp = sqlQuery;

            foreach (var p in param.ToArray())
            {
                tmp = tmp.Replace('@' + p.ParameterName.ToString(), "'" + p.Value.ToString() + "'");
            }

            _logger.Debug(tmp);
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sqlQuery, param.ToArray());
            stopWatch.Stop();
            _logger.Debug($"Query return in seconds: {stopWatch.Elapsed.TotalSeconds.ToString()}");
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        public DataTable SearchDonorFromTestHistory(Dictionary<string, string> searchParam, UserType userType, int currentUserId, string currentUserName)
        {
            string sqlQuery = "SELECT "
                            + "donors.donor_id AS DonorId, "
                            + "donorClearStarProfId AS DonorClearStarProfId, "
                            + "donors.donor_first_name AS DonorFirstName, "
                            + "donors.donor_last_name AS DonorLastName, "
                            + "donors.donor_ssn AS DonorSSN, "
                            + "donors.donor_pid AS donor_pid, "
                            + "donors.donor_date_of_birth AS DonorDateOfBirth, "
                            + "donors.donor_initial_client_id AS DonorInitialClientId, "
                            + "donors.donor_registration_status AS DonorRegistrationStatus, "
                            + "donors.donor_city as DonorCity, "
                            + "donors.donor_email as DonorEmail, "
                            + "donors.donor_zip as DonorZipCode, "
                            + "donors.is_archived AS IsDonorArchived, "
                            + "donor_test_info.donor_test_info_id AS DonorTestInfoId, "
                            + "donor_test_info.client_id AS TestInfoClientId, "
                            + "donor_test_info.client_department_id AS TestInfoDepartmentId, "
                            + "donor_test_info.mro_type_id AS MROTypeId, "
                            + "donor_test_info.payment_type_id AS PaymentTypeId, "
                            + "donor_test_info.test_requested_date AS TestRequestedDate, "
                            + "donor_test_info.reason_for_test_id AS ReasonForTestId, "
                            + "donor_test_info.total_payment_amount AS TotalPaymentAmount, "
                            + "donor_test_info.payment_method_id AS PaymentMethodId, "
                            + "donor_test_info.test_status AS TestStatus, "
                            + "donor_test_info.test_overall_result AS TestOverallResult, "
                            + "donor_test_info.is_walkin_donor AS IsWalkinDonor, "
                            + "donor_test_info.instant_test_result AS InstantTestResult, "
                            + "donor_test_info.is_instant_test AS IsInstantTest, "
                            + "donor_test_info.is_donor_refused AS IsDonorRefused, "
                            + "CONCAT_WS(' ', users.user_first_name, users.user_last_name) AS CollectorName, "
                            //+ "donor_test_info_test_categories.test_category_id AS TestCategoryId, "
                            //+ "donor_test_info_test_categories.test_panel_id AS TestPanelId, "
                            //+ "donor_test_info_test_categories.specimen_id AS SpecimenId, "
                            + "(select GROUP_CONCAT(donor_test_info_test_categories.specimen_id SEPARATOR ', ') from donor_test_info_test_categories where donor_test_info_test_categories.donor_test_info_id = donor_test_info.donor_test_info_id) AS SpecimenId, "
                            + "donor_test_info.screening_time AS SpecimenDate, "
                           //+ "donor_test_info_test_categories.hair_test_panel_days AS HairTestPanelDays, "
                           + "clients.client_name AS ClientName, "
                            + "client_departments.department_name AS ClientDepartmentName, "
                             + "client_departments.clearstarcode As ClearStarCode, "
                             + "dt.DocsTotal, dt.DocsNotApproved, dt.DocsRejected "
                            + "FROM donors "
                            //+ "LEFT OUTER JOIN donor_test_info ON donors.donor_id = donor_test_info.donor_id AND donor_test_info.donor_test_info_id = (SELECT max(donor_test_info.donor_test_info_id) FROM donor_test_info WHERE donor_test_info.donor_id = donors.donor_id) "
                            + "LEFT OUTER JOIN donor_test_info ON donors.donor_id = donor_test_info.donor_id "
                //  + "LEFT OUTER JOIN donor_test_info_test_categories ON donor_test_info.donor_test_info_id = donor_test_info_test_categories.donor_test_info_id "
                + "LEFT OUTER JOIN clients ON (donor_test_info.client_id = clients.client_id) " //(donors.donor_initial_client_id = clients.client_id AND donors.donor_initial_department_id IS NULL) OR
                                                                                                // + "LEFT OUTER JOIN clients ON donors.donor_initial_client_id = clients.client_id AND donors.donor_initial_department_id IS NULL "
                            + "LEFT OUTER JOIN client_departments ON donor_test_info.client_department_id = client_departments.client_department_id "
                            + "LEFT OUTER JOIN users ON donor_test_info.collection_site_user_id = users.user_id "
                            + "LEFT OUTER JOIN donordocumenttotals dt on dt.donor_id = donor_test_info.donor_id "
                            + "WHERE 1=1 AND test_status >3 ";

            List<MySqlParameter> param = new List<MySqlParameter>();

            foreach (KeyValuePair<string, string> searchItem in searchParam)
            {
                if (searchItem.Key == "DonorId")
                {
                    sqlQuery += "AND donors.donor_id = @DonorId ";
                    param.Add(new MySqlParameter("@DonorId", searchItem.Value));
                }
                else if (searchItem.Key == "DonorTestInfoId")
                {
                    sqlQuery += "AND donor_test_info.donor_test_info_id = @DonorTestInfoId ";
                    param.Add(new MySqlParameter("@DonorTestInfoId", searchItem.Value));
                }
                else if (searchItem.Key == "FirstName")
                {
                    sqlQuery += "AND donors.donor_first_name LIKE @DonorFirstName ";
                    param.Add(new MySqlParameter("@DonorFirstName", searchItem.Value));
                }
                else if (searchItem.Key == "LastName")
                {
                    sqlQuery += "AND donors.donor_last_name LIKE @DonorLastName ";
                    param.Add(new MySqlParameter("@DonorLastName", searchItem.Value));
                }
                else if (searchItem.Key == "SSN")
                {
                    sqlQuery += "AND donors.donor_ssn LIKE @DonorSSN ";
                    param.Add(new MySqlParameter("@DonorSSN", searchItem.Value));
                }
                else if (searchItem.Key == "DOB")
                {
                    sqlQuery += "AND CAST(donors.donor_date_of_birth AS DATE) = CAST(@DonorDateOfBirth AS DATE) ";
                    param.Add(new MySqlParameter("@DonorDateOfBirth", Convert.ToDateTime(searchItem.Value).ToString("yyyy-MM-dd")));
                }
                else if (searchItem.Key == "City")
                {
                    sqlQuery += "AND donors.donor_city LIKE @DonorCity ";
                    param.Add(new MySqlParameter("@DonorCity", searchItem.Value));
                }
                else if (searchItem.Key == "ZipCode")
                {
                    sqlQuery += "AND donors.donor_zip = @DonorZipCode ";
                    param.Add(new MySqlParameter("@DonorZipCode", searchItem.Value));
                }
                else if (searchItem.Key == "DonorEmail")
                {
                    sqlQuery += "AND donors.donor_email = @DonorEmail ";
                    param.Add(new MySqlParameter("@DonorEmail", searchItem.Value));
                }
                else if (searchItem.Key == "SpecimenId")
                {
                    sqlQuery += "AND donor_test_info.donor_test_info_id IN (SELECT donor_test_info_test_categories.donor_test_info_id FROM donor_test_info_test_categories WHERE donor_test_info_test_categories.specimen_id LIKE @SpecimenId) ";
                    param.Add(new MySqlParameter("@SpecimenId", searchItem.Value));
                }
                else if (searchItem.Key == "TestReason")
                {
                    sqlQuery += "AND donor_test_info.reason_for_test_id = @ReasonForTestId ";
                    param.Add(new MySqlParameter("@ReasonForTestId", Convert.ToInt32(searchItem.Value)));
                }
                else if (searchItem.Key == "Client")
                {
                    sqlQuery += "AND (donors.donor_initial_client_id = @ClientId OR donor_test_info.client_id = @ClientId) ";
                    param.Add(new MySqlParameter("@ClientId", Convert.ToInt32(searchItem.Value)));
                }
                else if (searchItem.Key == "Department")
                {
                    sqlQuery += "AND donor_test_info.client_department_id = @ClientDepartmentId ";
                    param.Add(new MySqlParameter("@ClientDepartmentId", Convert.ToInt32(searchItem.Value)));
                }
                else if (searchItem.Key == "Status")
                {
                    if (searchItem.Value == "1")
                    {
                        sqlQuery += "AND donor_test_info.test_status = @Status ";
                        param.Add(new MySqlParameter("@Status", Convert.ToInt32("3")));
                    }
                    if (searchItem.Value == "2")
                    {
                        sqlQuery += "AND donor_test_info.test_status = @Status ";
                        param.Add(new MySqlParameter("@Status", Convert.ToInt32("4")));
                    }
                    if (searchItem.Value == "3")
                    {
                        sqlQuery += "AND donor_test_info.test_status = @Status ";
                        param.Add(new MySqlParameter("@Status", Convert.ToInt32("5")));
                    }
                    if (searchItem.Value == "4")
                    {
                        sqlQuery += "AND donor_test_info.test_status = @Status ";
                        param.Add(new MySqlParameter("@Status", Convert.ToInt32("6")));
                    }
                    if (searchItem.Value == "5")
                    {
                        sqlQuery += "AND donor_test_info.test_status = @Status ";
                        param.Add(new MySqlParameter("@Status", Convert.ToInt32("7")));
                    }
                }
                else if (searchItem.Key == "TestCategory")
                {
                    sqlQuery += "AND donor_test_info.donor_test_info_id IN (SELECT donor_test_info_test_categories.donor_test_info_id FROM donor_test_info_test_categories WHERE donor_test_info_test_categories.test_category_id = @TestCategoryId) ";
                    param.Add(new MySqlParameter("@TestCategoryId", Convert.ToInt32(searchItem.Value)));
                }
                else if (searchItem.Key == "IncludeArchive")
                {
                    if (!Convert.ToBoolean(searchItem.Value))
                    {
                        sqlQuery += "AND donors.is_archived = b'0' ";
                    }
                }
                else if (searchItem.Key == "NoOfDays")
                {
                    if (!searchItem.Value.StartsWith("DR"))
                    {
                        int dateInterval = Convert.ToInt32(searchItem.Value) * -1;

                        sqlQuery += "AND donor_test_info.screening_time >= DATE_ADD(CURDATE(), INTERVAL @DateInterval DAY) ";
                        param.Add(new MySqlParameter("@DateInterval", dateInterval));
                    }
                    else
                    {
                        sqlQuery += "AND (CAST(donor_test_info.screening_time AS DATE) BETWEEN CAST(@StartDate AS DATE) AND CAST(@EndDate AS DATE)) ";
                        string[] dates = searchItem.Value.Split('#');
                        param.Add(new MySqlParameter("@StartDate", Convert.ToDateTime(dates[1]).ToString("yyyy-MM-dd")));
                        param.Add(new MySqlParameter("@EndDate", Convert.ToDateTime(dates[2]).ToString("yyyy-MM-dd")));
                    }
                }
            }
            if (currentUserName != null)
            {
                if (!(currentUserName.ToUpper() == "ADMIN" || currentUserName.ToUpper() == "DAVID" || currentUserName.ToUpper() == "TIM"))
                {
                    if ((userType == UserType.TPA && currentUserId != 1) && userType != UserType.Donor)
                    {
                        sqlQuery += "AND donor_test_info.client_department_id IN (select client_department_id from user_departments where user_id = " + currentUserId.ToString() + ")";
                    }
                }
            }

            sqlQuery += "ORDER BY donors.donor_first_name, donors.donor_last_name";

            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sqlQuery, param.ToArray());

            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }
    }
}