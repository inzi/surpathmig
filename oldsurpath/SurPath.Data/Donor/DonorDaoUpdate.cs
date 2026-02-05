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
        public void UpdateTestInfoSpecimenIDs(DonorTestInfo donorTestInfo)
        {
            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();

                try
                {
                    string sqlQuery = "UPDATE donor_test_info SET "
                        + "is_synchronized = b'0', "
                        + "last_modified_on = NOW(), "
                        + "last_modified_by = @LastModifiedBy "
                        + "WHERE donor_test_info_id = @DonorTestInfoId";

                    MySqlParameter[] param = new MySqlParameter[2];

                    param[0] = new MySqlParameter("@DonorTestInfoId", donorTestInfo.DonorTestInfoId);
                    param[1] = new MySqlParameter("@LastModifiedBy", donorTestInfo.LastModifiedBy);

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                    //Test Category
                    string specimenId = string.Empty;
                    if (donorTestInfo.TestStatus == DonorRegistrationStatus.Processing)
                    {
                        sqlQuery = "UPDATE donor_test_info_test_categories SET "
                                    + "specimen_id = @SpecimenId, "
                                    + "is_synchronized = b'0', "
                                    + "last_modified_on = NOW(), "
                                    + "last_modified_by = @LastModifiedBy "
                                    + "WHERE donor_test_test_category_id = @DonorTestTestCategoryId";

                        foreach (DonorTestInfoTestCategories donorTestCategory in donorTestInfo.TestInfoTestCategories)
                        {
                            param = new MySqlParameter[3];

                            param[0] = new MySqlParameter("@DonorTestTestCategoryId", donorTestCategory.DonorTestTestCategoryId);
                            param[1] = new MySqlParameter("@SpecimenId", donorTestCategory.SpecimenId);
                            param[2] = new MySqlParameter("@LastModifiedBy", donorTestInfo.LastModifiedBy);

                            SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                            if (donorTestCategory.TestCategoryId == TestCategories.UA || donorTestCategory.TestCategoryId == TestCategories.Hair)
                            {
                                if (donorTestCategory.SpecimenId.Trim() != string.Empty)
                                {
                                    if (specimenId.Trim() == string.Empty)
                                    {
                                        specimenId += donorTestCategory.SpecimenId.Trim();
                                    }
                                    else
                                    {
                                        specimenId += ", " + donorTestCategory.SpecimenId.Trim();
                                    }
                                }
                            }
                        }
                    }

                    //Activity Note
                    sqlQuery = "SELECT user_id FROM users where UPPER(user_name) = UPPER(@DonorEmail) and is_archived=0";

                    param = new MySqlParameter[1];

                    param[0] = new MySqlParameter("@DonorEmail", donorTestInfo.LastModifiedBy);

                    int currentUserId = (int)SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlQuery, param);

                    sqlQuery = "INSERT INTO donor_test_activity ("
                                    + "donor_test_info_id, "
                                    + "activity_datetime, "
                                    + "activity_user_id, "
                                    + "activity_category_id, "
                                    + "is_activity_visible, "
                                    + "activity_note, "
                                    + "is_synchronized) VALUES ("
                                    + "@DonorTestInfoId, "
                                    + "NOW(), "
                                    + "@ActivityUserId, "
                                    + "@ActivityCategoryId, "
                                    + "@IsActivityVisible, "
                                    + "@ActivityNote, "
                                    + "b'0')";

                    string donorActivityNote = string.Empty;

                    donorActivityNote = "Test Info updated with the Specimen ID(s) of {0}.";

                    param = new MySqlParameter[5];

                    param[0] = new MySqlParameter("@DonorTestInfoId", donorTestInfo.DonorTestInfoId);
                    param[1] = new MySqlParameter("@ActivityUserId", currentUserId);
                    param[2] = new MySqlParameter("@ActivityCategoryId", (int)DonorActivityCategories.General);
                    param[3] = new MySqlParameter("@IsActivityVisible", true);
                    param[4] = new MySqlParameter("@ActivityNote", string.Format(donorActivityNote, specimenId));

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
        }

        public void UpdateTestInfoClientIDandDepartment(int donorId, int clientId, int deptId)
        {

            try
            {
                if (clientId<1 || donorId< 1)
                throw new Exception($"ERROR: client id or client department id are 0! ClientID: {clientId} Client Department ID {deptId}");

                using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
                {
                    conn.Open();

                    MySqlTransaction trans = conn.BeginTransaction();

                    try
                    {
                        string sqlQuery = "UPDATE donor_test_info SET "
                             + "client_id = @DClientId,"
                             + "client_department_id = @DDeptId "
                            + "WHERE donor_id = @DonorId";

                        MySqlParameter[] param = new MySqlParameter[3];

                        param[0] = new MySqlParameter("@DClientId", clientId);
                        param[1] = new MySqlParameter("@DDeptId", deptId);
                        param[2] = new MySqlParameter("@DonorId", donorId);
                        //param[0] = new MySqlParameter("@DonorTestInfoId", donorTestInfo.DonorTestInfoId);
                        //param[1] = new MySqlParameter("@LastModifiedBy", donorTestInfo.LastModifiedBy);

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
            }
        }

        public void UpdateHairTestPanelDays(DonorTestInfo donorTestInfo)
        {
            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();

                try
                {
                    string sqlQuery = "UPDATE donor_test_info SET "
                        + "total_payment_amount = @TotalPaymentAmount, "
                        + "is_synchronized = b'0', "
                        + "last_modified_on = NOW(), "
                        + "last_modified_by = @LastModifiedBy "
                        + "WHERE donor_test_info_id = @DonorTestInfoId";

                    MySqlParameter[] param = new MySqlParameter[3];

                    param[0] = new MySqlParameter("@DonorTestInfoId", donorTestInfo.DonorTestInfoId);
                    param[1] = new MySqlParameter("@TotalPaymentAmount", donorTestInfo.TotalPaymentAmount);
                    param[2] = new MySqlParameter("@LastModifiedBy", donorTestInfo.LastModifiedBy);

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);
                    double? amount = donorTestInfo.TotalPaymentAmount;
                    string days = string.Empty;
                    //Test Category
                    if (donorTestInfo.TestStatus == DonorRegistrationStatus.Registered)
                    {
                        sqlQuery = "UPDATE donor_test_info_test_categories SET "
                                    + "hair_test_panel_days = @HairTestPanelDays, "
                                    + "test_panel_price = @TestPanelPrice, "
                                    + "is_synchronized = b'0', "
                                    + "last_modified_on = NOW(), "
                                    + "last_modified_by = @LastModifiedBy "
                                    + "WHERE donor_test_test_category_id = @DonorTestTestCategoryId";

                        foreach (DonorTestInfoTestCategories donorTestCategory in donorTestInfo.TestInfoTestCategories)
                        {
                            param = new MySqlParameter[4];

                            param[0] = new MySqlParameter("@DonorTestTestCategoryId", donorTestCategory.DonorTestTestCategoryId);
                            param[1] = new MySqlParameter("@HairTestPanelDays", donorTestCategory.HairTestPanelDays);
                            param[2] = new MySqlParameter("@TestPanelPrice", donorTestCategory.TestPanelPrice);
                            param[3] = new MySqlParameter("@LastModifiedBy", donorTestInfo.LastModifiedBy);

                            SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);
                            if (donorTestCategory.TestCategoryId == TestCategories.Hair)
                            {
                                days = donorTestCategory.HairTestPanelDays.ToString();
                            }
                        }
                    }

                    //Activity Note
                    sqlQuery = "SELECT user_id FROM users where UPPER(user_name) = UPPER(@DonorEmail) and is_archived=0";

                    param = new MySqlParameter[1];

                    param[0] = new MySqlParameter("@DonorEmail", donorTestInfo.LastModifiedBy);

                    int currentUserId = (int)SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlQuery, param);

                    sqlQuery = "INSERT INTO donor_test_activity ("
                                    + "donor_test_info_id, "
                                    + "activity_datetime, "
                                    + "activity_user_id, "
                                    + "activity_category_id, "
                                    + "is_activity_visible, "
                                    + "activity_note, "
                                    + "is_synchronized) VALUES ("
                                    + "@DonorTestInfoId, "
                                    + "NOW(), "
                                    + "@ActivityUserId, "
                                    + "@ActivityCategoryId, "
                                    + "@IsActivityVisible, "
                                    + "@ActivityNote, "
                                    + "b'0')";

                    string donorActivityNote = string.Empty;

                    donorActivityNote = "Test Info updated with the hair test panel days of {0}, the due amount is {1}.";

                    param = new MySqlParameter[5];

                    param[0] = new MySqlParameter("@DonorTestInfoId", donorTestInfo.DonorTestInfoId);
                    param[1] = new MySqlParameter("@ActivityUserId", currentUserId);
                    param[2] = new MySqlParameter("@ActivityCategoryId", (int)DonorActivityCategories.General);
                    param[3] = new MySqlParameter("@IsActivityVisible", true);
                    param[4] = new MySqlParameter("@ActivityNote", string.Format(donorActivityNote, days, amount));

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
        }

        public int Update(Donor donor)
        {
            int returnValue = 0;

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();

                try
                {
                    string sqlQuery = "UPDATE donors SET "
                                        + "donor_first_name = @DonorFirstName, "
                                        + "donor_mi = @DonorMI, "
                                        + "donor_last_name = @DonorLastName, "
                                        + "donor_suffix = @DonorSuffix, "
                                        + "donor_ssn = @DonorSSN, "
                                        + "donor_date_of_birth = @DonorDateOfBirth, "
                                        + "donor_phone_1 = @DonorPhone1, "
                                        + "donor_phone_2 = @DonorPhone2, "
                                        + "donor_address_1 = @DonorAddress1, "
                                        + "donor_address_2 = @DonorAddress2, "
                                        + "donor_city = @DonorCity, "
                                        + "donor_state = @DonorState, "
                                        + "donor_zip = @DonorZip, "
                                        + "donor_email = @DonorEmail, "
                                        + "donor_gender = @DonorGender, "
                                        + "donor_registration_status = @DonorRegistrationStatusValue, "
                                        + "is_synchronized = b'0', "
                                        + "last_modified_on = NOW(), "
                                        + "last_modified_by = @LastModifiedBy, "
                                        + "is_hidden_web = @IsHiddenWeb, "
                                        + "donor_initial_client_id = @DClientId,"
                                        + "donor_initial_department_id = @DDeptId, "
                                        + "donorClearStarProfId = @DClearStarProfId "
                                        + "WHERE donor_id = @DonorId";

                    MySqlParameter[] param = new MySqlParameter[22];

                    param[0] = new MySqlParameter("@DonorId", donor.DonorId);
                    param[1] = new MySqlParameter("@DonorFirstName", donor.DonorFirstName);
                    param[2] = new MySqlParameter("@DonorMI", donor.DonorMI);
                    param[3] = new MySqlParameter("@DonorLastName", donor.DonorLastName);
                    param[4] = new MySqlParameter("@DonorSuffix", donor.DonorSuffix);
                    param[5] = new MySqlParameter("@DonorSSN", donor.DonorSSN);
                    param[6] = new MySqlParameter("@DonorDateOfBirth", donor.DonorDateOfBirth);
                    param[7] = new MySqlParameter("@DonorPhone1", donor.DonorPhone1);
                    param[8] = new MySqlParameter("@DonorPhone2", donor.DonorPhone2);
                    param[9] = new MySqlParameter("@DonorAddress1", donor.DonorAddress1);
                    param[10] = new MySqlParameter("@DonorAddress2", donor.DonorAddress2);
                    param[11] = new MySqlParameter("@DonorCity", donor.DonorCity);
                    param[12] = new MySqlParameter("@DonorState", donor.DonorState);
                    param[13] = new MySqlParameter("@DonorZip", donor.DonorZip);
                    param[14] = new MySqlParameter("@DonorEmail", donor.DonorEmail);
                    param[15] = new MySqlParameter("@DonorGender", ((int)donor.DonorGender).ToString());
                    param[16] = new MySqlParameter("@DonorRegistrationStatusValue", (int)donor.DonorRegistrationStatusValue);
                    param[17] = new MySqlParameter("@LastModifiedBy", donor.LastModifiedBy);
                    param[18] = new MySqlParameter("@IsHiddenWeb", donor.IsHiddenWeb);
                    param[19] = new MySqlParameter("@DClientId", donor.DonorInitialClientId);
                    param[20] = new MySqlParameter("@DDeptId", donor.DonorInitialDepartmentId);
                    param[21] = new MySqlParameter("@DClearStarProfId", donor.DonorClearStarProfId);

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                    UpdateTestInfoClientIDandDepartment(donor.DonorId, (int)donor.DonorInitialClientId, (int)donor.DonorInitialDepartmentId);

                    string sqlQuery2 = "update users set is_archived = true where donor_id in (select donor_id from donor_test_info where TIMESTAMPDIFF(hour, created_on, now()) > 2 and payment_date is null and donor_id >= 15144)";
                    //SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery2);

                    if (donor.DonorRegistrationStatusValue != DonorRegistrationStatus.PreRegistration)
                    {
                        sqlQuery = "UPDATE users SET "
                                            + "user_first_name = @DonorFirstName, "
                                            + "user_last_name = @DonorLastName, "
                                            + "user_phone_number = @DonorPhone1, "
                                            + "user_email = @DonorEmail, "
                                            + "is_synchronized = b'0', "
                                            + "last_modified_on = NOW(), "
                                            + "last_modified_by = @LastModifiedBy "
                                            + "WHERE donor_id = @DonorId";

                        param = new MySqlParameter[6];

                        param[0] = new MySqlParameter("@DonorId", donor.DonorId);
                        param[1] = new MySqlParameter("@DonorFirstName", donor.DonorFirstName);
                        param[2] = new MySqlParameter("@DonorLastName", donor.DonorLastName);
                        param[3] = new MySqlParameter("@DonorPhone1", donor.DonorPhone1);
                        param[4] = new MySqlParameter("@DonorEmail", donor.DonorEmail);
                        param[5] = new MySqlParameter("@LastModifiedBy", donor.LastModifiedBy);

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);
                    }
                    else
                    {
                        sqlQuery = "UPDATE users SET "
                                            + "user_first_name = @DonorFirstName, "
                                            + "user_last_name = @DonorLastName, "
                                            + "user_phone_number = @DonorPhone1, "
                                            + "user_email = @DonorEmail, "
                                            + "user_name = @Username, "
                                            + "is_synchronized = b'0', "
                                            + "last_modified_on = NOW(), "
                                            + "last_modified_by = @LastModifiedBy "
                                            + "WHERE donor_id = @DonorId";

                        param = new MySqlParameter[7];

                        param[0] = new MySqlParameter("@DonorId", donor.DonorId);
                        param[1] = new MySqlParameter("@DonorFirstName", donor.DonorFirstName);
                        param[2] = new MySqlParameter("@DonorLastName", donor.DonorLastName);
                        param[3] = new MySqlParameter("@DonorPhone1", donor.DonorPhone1);
                        param[4] = new MySqlParameter("@DonorEmail", donor.DonorEmail);
                        param[5] = new MySqlParameter("@Username", donor.DonorEmail);
                        param[6] = new MySqlParameter("@LastModifiedBy", donor.LastModifiedBy);

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);
                    }

                    returnValue = 1;
                    
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }

            return returnValue;
        }

        public void UpdateDonorisWebHidden(int DonorId, bool Hidden)
        {
            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();

                try
                {
                    //update donor_test_info set is_hidden_web = 1 where donor_id = 28680
                    string sqlQuery = "Update donors set is_hidden_web=@isWebHidden where donor_id=@DonorTestInfoId";

                    MySqlParameter[] param = new MySqlParameter[5];

                    param[0] = new MySqlParameter("@isWebHidden", (bool)Hidden);
                    param[1] = new MySqlParameter("@DonorTestInfoId", DonorId);

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
        }

        public void UpdateReportInfotoArchive(string SpeciminId)
        {
            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();
                try
                {
                    string sqlQuery = "Update report_info set is_Archived = 1 where specimen_id = @SpecID";
                    MySqlParameter[] param = new MySqlParameter[1];
                    param[0] = new MySqlParameter("@SpecID", SpeciminId);

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
        }

        public void UpdateTestStatusToProcessing(int DonorId)
        {
            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();
                try
                {
                    string sqlQuery = "Update donor_test_info set test_status=6, test_overall_result = 0 where donor_id = @Donor_ID";
                    MySqlParameter[] param = new MySqlParameter[1];
                    param[0] = new MySqlParameter("@Donor_ID", DonorId);

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
        }
    }
}