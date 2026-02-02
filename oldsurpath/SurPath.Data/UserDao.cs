using MySql.Data.MySqlClient;
using Serilog;
using SurPath.Entity;
using SurPath.Enum;
using SurPath.MySQLHelper;
using System;
using System.Collections.Generic;
using System.Data;

namespace SurPath.Data
{
    public class UserDao : DataObject
    {
        #region Constructor
        public static ILogger _logger { get; set; }

        public UserDao(ILogger __logger = null)
        {
            if (__logger == null)
            {
                _logger = new LoggerConfiguration().CreateLogger();
            }
            else
            {
                _logger = __logger;
            }
        }

        #endregion Constructor

        #region Public Methods

        /// <summary>
        /// Inserts the User information to the database.
        /// </summary>
        /// <param name="user">User information which one need to be added to the database.</param>
        /// <returns>Returns UserId, the auto increament value.</returns>
        public int Insert(User user)
        {
            int returnValue = 0;

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();

                try
                {
                    string sqlQuery = "INSERT INTO users (user_name, user_password, is_user_active, user_first_name, user_last_name, user_phone_number, user_fax, user_email, change_password_required, user_type, department_id, donor_id, client_id, vendor_id, attorney_id, court_id, judge_id, is_synchronized, is_archived, created_on, created_by, last_modified_on, last_modified_by) VALUES (";
                    sqlQuery += "@Username, @UserPassword, @IsUserActive, @UserFirstName, @UserLastName, @UserPhoneNumber, @UserFax, @UserEmail, @ChangePasswordRequired, @UserType, @DepartmentId, @DonorId, @ClientId, @VendorId,  @AttorneyId, @CourtId, @JudgeId, b'0', b'0', NOW(), @CreatedBy, NOW(), @LastModifiedBy)";

                    MySqlParameter[] param = new MySqlParameter[19];
                    param[0] = new MySqlParameter("@Username", user.Username);
                    param[1] = new MySqlParameter("@UserPassword", user.UserPassword);
                    param[2] = new MySqlParameter("@IsUserActive", user.IsUserActive);
                    param[3] = new MySqlParameter("@UserFirstName", user.UserFirstName);
                    param[4] = new MySqlParameter("@UserLastName", user.UserLastName);
                    param[5] = new MySqlParameter("@UserPhoneNumber", user.UserPhoneNumber);
                    param[6] = new MySqlParameter("@UserFax", user.UserFax);
                    param[7] = new MySqlParameter("@UserEmail", user.UserEmail);
                    param[8] = new MySqlParameter("@ChangePasswordRequired", user.ChangePasswordRequired);
                    param[9] = new MySqlParameter("@UserType", user.UserType);
                    param[10] = new MySqlParameter("@DepartmentId", user.DepartmentId);
                    param[11] = new MySqlParameter("@DonorId", user.DonorId);
                    param[12] = new MySqlParameter("@ClientId", user.ClientId);
                    param[13] = new MySqlParameter("@VendorId", user.VendorId);
                    param[14] = new MySqlParameter("@AttorneyId", user.AttorneyId);
                    param[15] = new MySqlParameter("@CourtId", user.CourtId);
                    param[16] = new MySqlParameter("@JudgeId", user.JudgeId);
                    param[17] = new MySqlParameter("@CreatedBy", user.CreatedBy);
                    param[18] = new MySqlParameter("@LastModifiedBy", user.LastModifiedBy);

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                    sqlQuery = "SELECT LAST_INSERT_ID()";

                    returnValue = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlQuery));

                    user.UserId = returnValue;

                    //Client Department Mapping
                    sqlQuery = "DELETE FROM user_departments WHERE user_id = @UserId";

                    param = new MySqlParameter[1];
                    param[0] = new MySqlParameter("@UserId", user.UserId);

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                    sqlQuery = "INSERT INTO user_departments (user_id, client_department_id, is_synchronized, created_on, created_by, last_modified_on, last_modified_by) VALUES (";
                    sqlQuery += "@UserId, @ClientDepartmentId, b'0', NOW(), @CreatedBy, NOW(), @LastModifiedBy)";

                    foreach (int clientDepartmentId in user.ClientDepartmentList)
                    {
                        param = new MySqlParameter[4];

                        param[0] = new MySqlParameter("@UserId", user.UserId);
                        param[1] = new MySqlParameter("@ClientDepartmentId", clientDepartmentId);
                        param[2] = new MySqlParameter("@CreatedBy", user.CreatedBy);
                        param[3] = new MySqlParameter("@LastModifiedBy", user.LastModifiedBy);

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);
                    }

                    //Auth Rule Mapping
                    sqlQuery = "DELETE FROM user_auth_rules WHERE user_id = @UserId";

                    param = new MySqlParameter[1];
                    param[0] = new MySqlParameter("@UserId", user.UserId);

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                    sqlQuery = "INSERT INTO user_auth_rules (user_id, auth_rule_id, is_synchronized, created_on, created_by, last_modified_on, last_modified_by) VALUES (";
                    sqlQuery += "@UserId, @AuthRuleId, b'0', NOW(), @CreatedBy, NOW(), @LastModifiedBy)";

                    foreach (int authRuleId in user.AuthRuleList)
                    {
                        param = new MySqlParameter[4];

                        param[0] = new MySqlParameter("@UserId", user.UserId);
                        param[1] = new MySqlParameter("@AuthRuleId", authRuleId);
                        param[2] = new MySqlParameter("@CreatedBy", user.CreatedBy);
                        param[3] = new MySqlParameter("@LastModifiedBy", user.LastModifiedBy);

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);
                    }

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

            return returnValue;
        }

        /// <summary>
        /// Updates the User information to the database.
        /// </summary>
        /// <param name="user">User information which one need to be updated to the database.</param>
        /// <returns>Returns number of records affected in the database.</returns>
        public int Update(User user)
        {
            int returnValue = 0;

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();

                try
                {
                    string sqlQuery = "UPDATE users SET "
                                    + "user_name = @Username, "
                                    + "user_password = @UserPassword, "
                                    + "is_user_active = @IsUserActive, "
                                    + "user_first_name = @UserFirstName, "
                                    + "user_last_name = @UserLastName, "
                                    + "user_phone_number = @UserPhoneNumber, "
                                    + "user_fax = @UserFax, "
                                    + "user_email = @UserEmail, "
                                    + "change_password_required = @ChangePasswordRequired, "
                                    + "user_type = @UserType, "
                                    + "department_id = @DepartmentId, "
                                    + "donor_id  = @DonorId, "
                                    + "client_id = @ClientId, "
                                    + "vendor_id = @VendorId, "
                                    + "attorney_id  = @AttorneyId, "
                                    + "court_id = @CourtId, "
                                    + "judge_id = @JudgeId, "
                                    + "is_synchronized = b'0', "
                                    + "last_modified_on = NOW(), "
                                    + "last_modified_by = @LastModifiedBy "
                                    + "WHERE user_id = @UserId";

                    MySqlParameter[] param = new MySqlParameter[19];
                    param[0] = new MySqlParameter("@UserId", user.UserId);
                    param[1] = new MySqlParameter("@Username", user.Username);
                    param[2] = new MySqlParameter("@UserPassword", user.UserPassword);
                    param[3] = new MySqlParameter("@IsUserActive", user.IsUserActive);
                    param[4] = new MySqlParameter("@UserFirstName", user.UserFirstName);
                    param[5] = new MySqlParameter("@UserLastName", user.UserLastName);
                    param[6] = new MySqlParameter("@UserPhoneNumber", user.UserPhoneNumber);
                    param[7] = new MySqlParameter("@UserFax", user.UserFax);
                    param[8] = new MySqlParameter("@UserEmail", user.UserEmail);
                    param[9] = new MySqlParameter("@ChangePasswordRequired", user.ChangePasswordRequired);
                    param[10] = new MySqlParameter("@UserType", user.UserType);
                    param[11] = new MySqlParameter("@DepartmentId", user.DepartmentId);
                    param[12] = new MySqlParameter("@DonorId", user.DonorId);
                    param[13] = new MySqlParameter("@ClientId", user.ClientId);
                    param[14] = new MySqlParameter("@VendorId", user.VendorId);
                    param[15] = new MySqlParameter("@AttorneyId", user.AttorneyId);
                    param[16] = new MySqlParameter("@CourtId", user.CourtId);
                    param[17] = new MySqlParameter("@JudgeId", user.JudgeId);
                    param[18] = new MySqlParameter("@LastModifiedBy", user.LastModifiedBy);

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                    //Client Department Mapping
                    sqlQuery = "DELETE FROM user_departments WHERE user_id = @UserId";

                    param = new MySqlParameter[1];
                    param[0] = new MySqlParameter("@UserId", user.UserId);

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                    sqlQuery = "INSERT INTO user_departments (user_id, client_department_id, is_synchronized, created_on, created_by, last_modified_on, last_modified_by) VALUES (";
                    sqlQuery += "@UserId, @ClientDepartmentId, b'0', NOW(), @CreatedBy, NOW(), @LastModifiedBy)";

                    foreach (int clientDepartmentId in user.ClientDepartmentList)
                    {
                        param = new MySqlParameter[4];

                        param[0] = new MySqlParameter("@UserId", user.UserId);
                        param[1] = new MySqlParameter("@ClientDepartmentId", clientDepartmentId);
                        param[2] = new MySqlParameter("@CreatedBy", user.LastModifiedBy);
                        param[3] = new MySqlParameter("@LastModifiedBy", user.LastModifiedBy);

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);
                    }

                    //Auth Rule Mapping
                    sqlQuery = "DELETE FROM user_auth_rules WHERE user_id = @UserId";

                    param = new MySqlParameter[1];
                    param[0] = new MySqlParameter("@UserId", user.UserId);

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                    sqlQuery = "INSERT INTO user_auth_rules (user_id, auth_rule_id, is_synchronized, created_on, created_by, last_modified_on, last_modified_by) VALUES (";
                    sqlQuery += "@UserId, @AuthRuleId, b'0', NOW(), @CreatedBy, NOW(), @LastModifiedBy)";

                    foreach (int authRuleId in user.AuthRuleList)
                    {
                        param = new MySqlParameter[4];

                        param[0] = new MySqlParameter("@UserId", user.UserId);
                        param[1] = new MySqlParameter("@AuthRuleId", authRuleId);
                        param[2] = new MySqlParameter("@CreatedBy", user.LastModifiedBy);
                        param[3] = new MySqlParameter("@LastModifiedBy", user.LastModifiedBy);

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);
                    }

                    trans.Commit();

                    returnValue = 1;
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

            return returnValue;
        }

        /// <summary>
        /// Deletes the User information from database.
        /// </summary>
        /// <param name="userId">User Id which one will be deleted.</param>
        /// <param name="currentUsername">Current username who is deleting the record.</param>
        /// <returns>Returns number of records deleted from the database.</returns>
        public int Delete(int userId, string currentUsername)
        {
            int returnValue = 0;

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();

                //string sqlCount1Query = "Select count(*) from clients where is_archived = 0 AND sales_representative_id = " + userId + "";

                //int table1 = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlCount1Query));

                //if (table1 <= 0)
                //{
                //    string sqlCount2Query = "Select count(*) from client_departments where is_archived = 0 AND sales_representative_id = " + userId + "";

                //    int table2 = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlCount2Query));
                //    if (table2 <= 0)
                //    {
                //        string sqlCount3Query = "Select count(*) from donor_test_info where test_requested_by = " + userId + "";

                //        int table3 = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlCount3Query));
                //        if (table3 <= 0)
                //        {
                //            string sqlCount4Query = "Select count(*) from donor_test_info where collection_site_user_id = " + userId + "";

                //            int table4 = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlCount4Query));
                //            if (table4 <= 0)
                //            {
                //                string sqlCount5Query = "Select count(*) from user_departments where user_id = " + userId + "";

                //                int table5 = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlCount5Query));
                //                if (table5 <= 0)
                //                {
                //                    string sqlCount6Query = "Select count(*) from user_auth_rules where user_id = " + userId + "";

                //                    int table6 = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlCount6Query));
                //                    if (table6 <= 0)
                //                    {
                //                        string sqlCount7Query = "Select count(*) from donor_test_activity where activity_user_id = " + userId + "";

                //                        int table7 = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlCount7Query));
                //                        if (table7 <= 0)
                //                        {
                try
                {
                    string sqlQuery = "UPDATE users SET is_synchronized = b'0', is_archived = b'1', last_modified_on = NOW(), last_modified_by = @LastModifiedBy WHERE user_id = @UserId";

                    MySqlParameter[] param = new MySqlParameter[2];
                    param[0] = new MySqlParameter("@UserId", userId);
                    param[1] = new MySqlParameter("@LastModifiedBy", currentUsername);

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                    trans.Commit();

                    returnValue = 1;
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
            //    }
            //}
            //                }
            //            }
            //        }
            //    }
            //}

            return returnValue;
        }

        /// <summary>
        /// Reset the User password.
        /// </summary>
        /// <param name="usernameOrEmail">Username / Email which one need to be updated to the database.</param>
        /// <param name="newPassword">New password which one need to reset.</param>
        /// <returns>Returns number of records affected in the database.</returns>
        public int ResetPassword(string usernameOrEmail, string newPassword, string currentUsername)
        {
            int returnValue = 0;

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();

                try
                {
                    string sqlQuery = "UPDATE users SET user_password = @NewPassword, change_password_required = b'1', is_synchronized = b'0', last_modified_on = NOW(), last_modified_by = @LastModifiedBy WHERE (UPPER(user_name) = UPPER(@UsernameOrEmail) OR UPPER(user_email) = UPPER(@UsernameOrEmail))";

                    MySqlParameter[] param = new MySqlParameter[3];
                    param[0] = new MySqlParameter("@UsernameOrEmail", usernameOrEmail);
                    param[1] = new MySqlParameter("@NewPassword", newPassword);
                    param[2] = new MySqlParameter("@LastModifiedBy", currentUsername);

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                    trans.Commit();

                    returnValue = 1;
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

            return returnValue;
        }

        /// <summary>
        /// Change the User password.
        /// </summary>
        /// <param name="username">Username which one need to be updated to the database.</param>
        /// <param name="newPassword">New password which one need to reset.</param>
        /// <returns>Returns number of records affected in the database.</returns>
        public int ChangePassword(string username, string newPassword)
        {
            int returnValue = 0;

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();

                try
                {
                    string sqlQuery1 = "UPDATE users SET user_password = @NewPassword, change_password_required = b'0', is_synchronized = b'0', last_modified_on = NOW(), last_modified_by = @LastModifiedBy WHERE UPPER(user_name) = UPPER(@Username)";

                    MySqlParameter[] param1 = new MySqlParameter[3];
                    param1[0] = new MySqlParameter("@Username", username);
                    param1[1] = new MySqlParameter("@NewPassword", newPassword);
                    param1[2] = new MySqlParameter("@LastModifiedBy", username);

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery1, param1);

                    trans.Commit();

                    returnValue = 1;
                    
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

            return returnValue;
        }

        /// <summary>
        /// Get the User information by User Id
        /// </summary>
        /// <param name="userId">User Id which one need to be get from the database.</param>
        /// <returns>Returns User information.</returns>
        public User Get(int userId)
        {
            User user = null;
            string sqlQuery = "SELECT user_id AS UserId, user_name AS Username, user_password AS UserPassword, is_user_active AS IsUserActive, user_first_name AS UserFirstName, user_last_name AS UserLastName, user_phone_number AS UserPhoneNumber, user_fax AS UserFax, user_email AS UserEmail, change_password_required AS ChangePasswordRequired, user_type AS UserType, department_id AS DepartmentId, donor_id AS DonorId, client_id AS ClientId, vendor_id AS VendorId, attorney_id AS AttorneyId, court_id AS CourtId, judge_id AS JudgeId, is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM users WHERE user_id = @UserId";

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                MySqlParameter[] param = new MySqlParameter[1];
                param[0] = new MySqlParameter("@UserId", userId);

                MySqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                if (dr.Read())
                {
                    user = new User();
                    user.UserId = (int)dr["UserId"];
                    user.Username = (string)dr["Username"];
                    user.UserPassword = (string)dr["UserPassword"];
                    user.IsUserActive = dr["IsUserActive"].ToString() == "1" ? true : false;
                    user.UserFirstName = (string)dr["UserFirstName"];
                    user.UserLastName = dr["UserLastName"].ToString();
                    user.UserPhoneNumber = dr["UserPhoneNumber"].ToString();
                    user.UserFax = dr["UserFax"].ToString();
                    user.UserEmail = dr["UserEmail"].ToString();
                    user.ChangePasswordRequired = dr["ChangePasswordRequired"].ToString() == "1" ? true : false;
                    user.UserType = (UserType)dr["UserType"];
                    user.DepartmentId = dr["DepartmentId"].ToString() != string.Empty ? (int?)dr["DepartmentId"] : null;
                    user.DonorId = dr["DonorId"].ToString() != string.Empty ? (int?)dr["DonorId"] : null;
                    user.ClientId = dr["ClientId"].ToString() != string.Empty ? (int?)dr["ClientId"] : null;
                    user.VendorId = dr["VendorId"].ToString() != string.Empty ? (int?)dr["VendorId"] : null;
                    user.AttorneyId = dr["AttorneyId"].ToString() != string.Empty ? (int?)dr["AttorneyId"] : null;
                    user.CourtId = dr["CourtId"].ToString() != string.Empty ? (int?)dr["CourtId"] : null;
                    user.JudgeId = dr["JudgeId"].ToString() != string.Empty ? (int?)dr["JudgeId"] : null;
                    user.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    user.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    user.CreatedOn = (DateTime)dr["CreatedOn"];
                    user.CreatedBy = (string)dr["CreatedBy"];
                    user.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    user.LastModifiedBy = (string)dr["LastModifiedBy"];
                }
                dr.Close();

                if (user != null)
                {
                    sqlQuery = "SELECT client_department_id AS ClientDepartmentId FROM user_departments WHERE user_id = @UserId";

                    dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);
                    while (dr.Read())
                    {
                        user.ClientDepartmentList.Add(Convert.ToInt32(dr["ClientDepartmentId"]));
                    }
                    dr.Close();

                    //
                    sqlQuery = "SELECT auth_rule_id AS AuthRuleId FROM user_auth_rules WHERE user_id = @UserId";

                    dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);
                    while (dr.Read())
                    {
                        user.AuthRuleList.Add(Convert.ToInt32(dr["AuthRuleId"]));
                    }
                    dr.Close();
                }
            }

            return user;
        }

        /// <summary>
        /// Get the User information by User Id
        /// </summary>
        /// <param name="username">Username which one need to be get from the database.</param>
        /// <returns>Returns User information.</returns>
        public User Get(string username)
        {
            User user = null;
            string sqlQuery = "SELECT user_id AS UserId, user_name AS Username, user_password AS UserPassword, is_user_active AS IsUserActive, user_first_name AS UserFirstName, user_last_name AS UserLastName, user_phone_number AS UserPhoneNumber, user_fax AS UserFax, user_email AS UserEmail, change_password_required AS ChangePasswordRequired, user_type AS UserType, department_id AS DepartmentId, donor_id AS DonorId, client_id AS ClientId, vendor_id AS VendorId, attorney_id AS AttorneyId, court_id AS CourtId, judge_id AS JudgeId, is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM users WHERE is_archived = 0 AND UPPER(user_name) = UPPER(@Username)";

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                MySqlParameter[] param = new MySqlParameter[1];
                param[0] = new MySqlParameter("@Username", username);

                MySqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                if (dr.Read())
                {
                    user = new User();
                    user.UserId = (int)dr["UserId"];
                    user.Username = (string)dr["Username"];
                    user.UserPassword = (string)dr["UserPassword"];
                    user.IsUserActive = dr["IsUserActive"].ToString() == "1" ? true : false;
                    user.UserFirstName = (string)dr["UserFirstName"];
                    user.UserLastName = dr["UserLastName"].ToString();
                    user.UserPhoneNumber = dr["UserPhoneNumber"].ToString();
                    user.UserFax = dr["UserFax"].ToString();
                    user.UserEmail = dr["UserEmail"].ToString();
                    user.ChangePasswordRequired = dr["ChangePasswordRequired"].ToString() == "1" ? true : false;
                    user.UserType = (UserType)dr["UserType"];
                    user.DepartmentId = dr["DepartmentId"].ToString() != string.Empty ? (int?)dr["DepartmentId"] : null;
                    user.DonorId = dr["DonorId"].ToString() != string.Empty ? (int?)dr["DonorId"] : null;
                    user.ClientId = dr["ClientId"].ToString() != string.Empty ? (int?)dr["ClientId"] : null;
                    user.VendorId = dr["VendorId"].ToString() != string.Empty ? (int?)dr["VendorId"] : null;
                    user.AttorneyId = dr["AttorneyId"].ToString() != string.Empty ? (int?)dr["AttorneyId"] : null;
                    user.CourtId = dr["CourtId"].ToString() != string.Empty ? (int?)dr["CourtId"] : null;
                    user.JudgeId = dr["JudgeId"].ToString() != string.Empty ? (int?)dr["JudgeId"] : null;
                    user.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    user.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    user.CreatedOn = (DateTime)dr["CreatedOn"];
                    user.CreatedBy = (string)dr["CreatedBy"];
                    user.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    user.LastModifiedBy = (string)dr["LastModifiedBy"];
                }
            }

            return user;
        }

        public User GetWithPassword(string username, string user_password)
        {
            User user = null;
            string sqlQuery = "SELECT user_id AS UserId, user_name AS Username, user_password AS UserPassword, is_user_active AS IsUserActive, user_first_name AS UserFirstName, user_last_name AS UserLastName, user_phone_number AS UserPhoneNumber, user_fax AS UserFax, user_email AS UserEmail, change_password_required AS ChangePasswordRequired, user_type AS UserType, department_id AS DepartmentId, donor_id AS DonorId, client_id AS ClientId, vendor_id AS VendorId, attorney_id AS AttorneyId, court_id AS CourtId, judge_id AS JudgeId, is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM users WHERE is_archived = 0 AND UPPER(user_name) = UPPER(@Username) AND user_password = @user_password";

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                MySqlParameter[] param = new MySqlParameter[2];
                param[0] = new MySqlParameter("@Username", username);
                param[1] = new MySqlParameter("@user_password", user_password);

                MySqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                if (dr.Read())
                {
                    user = new User();
                    user.UserId = (int)dr["UserId"];
                    user.Username = (string)dr["Username"];
                    user.UserPassword = (string)dr["UserPassword"];
                    user.IsUserActive = dr["IsUserActive"].ToString() == "1" ? true : false;
                    user.UserFirstName = (string)dr["UserFirstName"];
                    user.UserLastName = dr["UserLastName"].ToString();
                    user.UserPhoneNumber = dr["UserPhoneNumber"].ToString();
                    user.UserFax = dr["UserFax"].ToString();
                    user.UserEmail = dr["UserEmail"].ToString();
                    user.ChangePasswordRequired = dr["ChangePasswordRequired"].ToString() == "1" ? true : false;
                    user.UserType = (UserType)dr["UserType"];
                    user.DepartmentId = dr["DepartmentId"].ToString() != string.Empty ? (int?)dr["DepartmentId"] : null;
                    user.DonorId = dr["DonorId"].ToString() != string.Empty ? (int?)dr["DonorId"] : null;
                    user.ClientId = dr["ClientId"].ToString() != string.Empty ? (int?)dr["ClientId"] : null;
                    user.VendorId = dr["VendorId"].ToString() != string.Empty ? (int?)dr["VendorId"] : null;
                    user.AttorneyId = dr["AttorneyId"].ToString() != string.Empty ? (int?)dr["AttorneyId"] : null;
                    user.CourtId = dr["CourtId"].ToString() != string.Empty ? (int?)dr["CourtId"] : null;
                    user.JudgeId = dr["JudgeId"].ToString() != string.Empty ? (int?)dr["JudgeId"] : null;
                    user.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    user.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    user.CreatedOn = (DateTime)dr["CreatedOn"];
                    user.CreatedBy = (string)dr["CreatedBy"];
                    user.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    user.LastModifiedBy = (string)dr["LastModifiedBy"];
                }
            }

            return user;
        }




        /// <summary>
        /// Get the User information by User Id
        /// </summary>
        /// <param name="usernameOrEmail">Username or Email which one need to be get from the database.</param>
        /// <returns>Returns User information.</returns>
        public User GetByUsernameOrEmail(string usernameOrEmail)
        {
            User user = null;
            string sqlQuery = "SELECT user_id AS UserId, user_name AS Username, user_password AS UserPassword, is_user_active AS IsUserActive, user_first_name AS UserFirstName, user_last_name AS UserLastName, user_phone_number AS UserPhoneNumber, user_fax AS UserFax, user_email AS UserEmail, change_password_required AS ChangePasswordRequired, user_type AS UserType, department_id AS DepartmentId, donor_id AS DonorId, client_id AS ClientId, vendor_id AS VendorId, attorney_id AS AttorneyId, court_id AS CourtId, judge_id AS JudgeId, is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM users WHERE is_archived = 0 AND (UPPER(user_name) = UPPER(@UsernameOrEmail) OR UPPER(user_email) = UPPER(@UsernameOrEmail))";

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                MySqlParameter[] param = new MySqlParameter[1];
                param[0] = new MySqlParameter("@UsernameOrEmail", usernameOrEmail);

                MySqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                if (dr.Read())
                {
                    user = new User();
                    user.UserId = (int)dr["UserId"];
                    user.Username = (string)dr["Username"];
                    user.UserPassword = (string)dr["UserPassword"];
                    user.IsUserActive = dr["IsUserActive"].ToString() == "1" ? true : false;
                    user.UserFirstName = (string)dr["UserFirstName"];
                    user.UserLastName = dr["UserLastName"].ToString();
                    user.UserPhoneNumber = dr["UserPhoneNumber"].ToString();
                    user.UserFax = dr["UserFax"].ToString();
                    user.UserEmail = dr["UserEmail"].ToString();
                    user.ChangePasswordRequired = dr["ChangePasswordRequired"].ToString() == "1" ? true : false;
                    user.UserType = (UserType)dr["UserType"];
                    user.DepartmentId = dr["DepartmentId"].ToString() != string.Empty ? (int?)dr["DepartmentId"] : null;
                    user.DonorId = dr["DonorId"].ToString() != string.Empty ? (int?)dr["DonorId"] : null;
                    user.ClientId = dr["ClientId"].ToString() != string.Empty ? (int?)dr["ClientId"] : null;
                    user.VendorId = dr["VendorId"].ToString() != string.Empty ? (int?)dr["VendorId"] : null;
                    user.AttorneyId = dr["AttorneyId"].ToString() != string.Empty ? (int?)dr["AttorneyId"] : null;
                    user.CourtId = dr["CourtId"].ToString() != string.Empty ? (int?)dr["CourtId"] : null;
                    user.JudgeId = dr["JudgeId"].ToString() != string.Empty ? (int?)dr["JudgeId"] : null;
                    user.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    user.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    user.CreatedOn = (DateTime)dr["CreatedOn"];
                    user.CreatedBy = (string)dr["CreatedBy"];
                    user.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    user.LastModifiedBy = (string)dr["LastModifiedBy"];
                }
                dr.Close();

                if (user != null)
                {
                    if (user.DonorId != null)
                    {
                        string sqlGetTestInfoId = "SELECT donor_id AS DonorId, MAX(donor_test_info_id) AS DonorTestInfoId FROM donor_test_info WHERE donor_id = @DonorId";
                        using (MySqlConnection con = new MySqlConnection(this.ConnectionString))
                        {
                            MySqlParameter[] para = new MySqlParameter[1];
                            para[0] = new MySqlParameter("@DonorId", user.DonorId);

                            MySqlDataReader datatReader = SqlHelper.ExecuteReader(con, CommandType.Text, sqlGetTestInfoId, para);

                            if (datatReader.Read())
                            {
                                if (!string.IsNullOrEmpty(datatReader["DonorTestInfoId"].ToString()))
                                {
                                    string sqlGetTestStatus = " SELECT donor_test_info_id AS DonorTestInfoId, test_status AS TestStatus FROM donor_test_info WHERE donor_test_info_id = @DonorTestInfoId";
                                    MySqlParameter[] para1 = new MySqlParameter[1];
                                    para1[0] = new MySqlParameter("@DonorTestInfoId", datatReader["DonorTestInfoId"]);

                                    datatReader.Close();

                                    MySqlDataReader datatReader1 = SqlHelper.ExecuteReader(con, CommandType.Text, sqlGetTestStatus, para1);

                                    if (datatReader1.Read())
                                    {
                                        if (!string.IsNullOrEmpty(datatReader1["DonorTestInfoId"].ToString()))
                                        {
                                            DonorRegistrationStatus drStatus = datatReader1["TestStatus"] != DBNull.Value ? (DonorRegistrationStatus)(Convert.ToInt32(datatReader1["TestStatus"].ToString())) : DonorRegistrationStatus.None;
                                            if (drStatus == DonorRegistrationStatus.Completed)
                                            {
                                                user.ProgramExists = "No";
                                            }
                                            else
                                            {
                                                user.ProgramExists = "Yes";
                                            }
                                        }
                                        else
                                        {
                                            user.ProgramExists = "No";
                                        }
                                    }
                                    else
                                    {
                                        user.ProgramExists = "No";
                                    }
                                    datatReader1.Close();
                                }
                                else
                                {
                                    user.ProgramExists = "No";
                                }
                            }
                        }
                    }
                }
            }

            return user;
        }

        public User GetByUserID(int user_id)
        {
            User user = null;
            string sqlQuery = "SELECT user_id AS UserId, user_name AS Username, user_password AS UserPassword, is_user_active AS IsUserActive, user_first_name AS UserFirstName, user_last_name AS UserLastName, user_phone_number AS UserPhoneNumber, user_fax AS UserFax, user_email AS UserEmail, change_password_required AS ChangePasswordRequired, user_type AS UserType, department_id AS DepartmentId, donor_id AS DonorId, client_id AS ClientId, vendor_id AS VendorId, attorney_id AS AttorneyId, court_id AS CourtId, judge_id AS JudgeId, is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM users WHERE is_archived = 0 AND user_id = @user_id";

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                MySqlParameter[] param = new MySqlParameter[1];
                param[0] = new MySqlParameter("@user_id", user_id);

                MySqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                if (dr.Read())
                {
                    user = new User();
                    user.UserId = (int)dr["UserId"];
                    user.Username = (string)dr["Username"];
                    user.UserPassword = (string)dr["UserPassword"];
                    user.IsUserActive = dr["IsUserActive"].ToString() == "1" ? true : false;
                    user.UserFirstName = (string)dr["UserFirstName"];
                    user.UserLastName = dr["UserLastName"].ToString();
                    user.UserPhoneNumber = dr["UserPhoneNumber"].ToString();
                    user.UserFax = dr["UserFax"].ToString();
                    user.UserEmail = dr["UserEmail"].ToString();
                    user.ChangePasswordRequired = dr["ChangePasswordRequired"].ToString() == "1" ? true : false;
                    user.UserType = (UserType)dr["UserType"];
                    user.DepartmentId = dr["DepartmentId"].ToString() != string.Empty ? (int?)dr["DepartmentId"] : null;
                    user.DonorId = dr["DonorId"].ToString() != string.Empty ? (int?)dr["DonorId"] : null;
                    user.ClientId = dr["ClientId"].ToString() != string.Empty ? (int?)dr["ClientId"] : null;
                    user.VendorId = dr["VendorId"].ToString() != string.Empty ? (int?)dr["VendorId"] : null;
                    user.AttorneyId = dr["AttorneyId"].ToString() != string.Empty ? (int?)dr["AttorneyId"] : null;
                    user.CourtId = dr["CourtId"].ToString() != string.Empty ? (int?)dr["CourtId"] : null;
                    user.JudgeId = dr["JudgeId"].ToString() != string.Empty ? (int?)dr["JudgeId"] : null;
                    user.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    user.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    user.CreatedOn = (DateTime)dr["CreatedOn"];
                    user.CreatedBy = (string)dr["CreatedBy"];
                    user.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    user.LastModifiedBy = (string)dr["LastModifiedBy"];
                }
                dr.Close();

                if (user != null)
                {
                    if (user.DonorId != null)
                    {
                        string sqlGetTestInfoId = "SELECT donor_id AS DonorId, MAX(donor_test_info_id) AS DonorTestInfoId FROM donor_test_info WHERE donor_id = @DonorId";
                        using (MySqlConnection con = new MySqlConnection(this.ConnectionString))
                        {
                            MySqlParameter[] para = new MySqlParameter[1];
                            para[0] = new MySqlParameter("@DonorId", user.DonorId);

                            MySqlDataReader datatReader = SqlHelper.ExecuteReader(con, CommandType.Text, sqlGetTestInfoId, para);

                            if (datatReader.Read())
                            {
                                if (!string.IsNullOrEmpty(datatReader["DonorTestInfoId"].ToString()))
                                {
                                    string sqlGetTestStatus = " SELECT donor_test_info_id AS DonorTestInfoId, test_status AS TestStatus FROM donor_test_info WHERE donor_test_info_id = @DonorTestInfoId";
                                    MySqlParameter[] para1 = new MySqlParameter[1];
                                    para1[0] = new MySqlParameter("@DonorTestInfoId", datatReader["DonorTestInfoId"]);

                                    datatReader.Close();

                                    MySqlDataReader datatReader1 = SqlHelper.ExecuteReader(con, CommandType.Text, sqlGetTestStatus, para1);

                                    if (datatReader1.Read())
                                    {
                                        if (!string.IsNullOrEmpty(datatReader1["DonorTestInfoId"].ToString()))
                                        {
                                            DonorRegistrationStatus drStatus = datatReader1["TestStatus"] != DBNull.Value ? (DonorRegistrationStatus)(Convert.ToInt32(datatReader1["TestStatus"].ToString())) : DonorRegistrationStatus.None;
                                            if (drStatus == DonorRegistrationStatus.Completed)
                                            {
                                                user.ProgramExists = "No";
                                            }
                                            else
                                            {
                                                user.ProgramExists = "Yes";
                                            }
                                        }
                                        else
                                        {
                                            user.ProgramExists = "No";
                                        }
                                    }
                                    else
                                    {
                                        user.ProgramExists = "No";
                                    }
                                    datatReader1.Close();
                                }
                                else
                                {
                                    user.ProgramExists = "No";
                                }
                            }
                        }
                    }
                }
            }

            return user;
        }

        public Tuple<User, bool> GetUserByUsernamePasswordAndDepartmentName(string user_name, string user_password, string department_name)
        {
            
            User user = new User();
            bool authenticated = false;
            string sqlQuery = @"
SELECT u.user_id                  AS UserId,
       u.user_name                AS Username,
       u.user_password            AS UserPassword,
       u.is_user_active           AS IsUserActive,
       u.user_first_name          AS UserFirstName,
       u.user_last_name           AS UserLastName,
       u.user_phone_number        AS UserPhoneNumber,
       u.user_fax                 AS UserFax,
       u.user_email               AS UserEmail,
       u.change_password_required AS ChangePasswordRequired,
       u.user_type                AS UserType,
       u.department_id            AS DepartmentId,
       u.donor_id                 AS DonorId,
       u.client_id                AS ClientId,
       u.vendor_id                AS VendorId,
       u.attorney_id              AS AttorneyId,
       u.court_id                 AS CourtId,
       u.judge_id                 AS JudgeId,
       u.is_synchronized          AS IsSynchronized,
       u.is_archived              AS IsArchived,
       u.created_on               AS CreatedOn,
       u.created_by               AS CreatedBy,
       u.last_modified_on         AS LastModifiedOn,
       u.last_modified_by         AS LastModifiedBy,
       IF(u.user_password = @user_password,1,0) AS PasswordMatches
FROM users u
  inner join donors d on d.donor_id = u.donor_id
  inner join clients c on d.donor_initial_client_id = c.client_id
  inner join client_departments cd on c.client_id = cd.client_id
  
WHERE     u.is_archived = 0
      AND UPPER(u.user_name) = UPPER(@user_name)
      -- AND u.user_password = @user_password
      and UPPER(cd.department_name) = UPPER(@department_name)
      ;
";

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                ParamHelper param = new ParamHelper();
                param.Param = new MySqlParameter("@user_name", user_name);
                param.Param = new MySqlParameter("@user_password", user_password);
                param.Param = new MySqlParameter("@department_name", department_name);

                MySqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param.Params);

                if (dr.Read())
                {
                    authenticated = ((long)dr["PasswordMatches"]) > 0;

                    user.UserId = (int)dr["UserId"];
                    user.Username = (string)dr["Username"];
                    user.UserPassword = (string)dr["UserPassword"];
                    user.IsUserActive = dr["IsUserActive"].ToString() == "1" ? true : false;
                    user.UserFirstName = (string)dr["UserFirstName"];
                    user.UserLastName = dr["UserLastName"].ToString();
                    user.UserPhoneNumber = dr["UserPhoneNumber"].ToString();
                    user.UserFax = dr["UserFax"].ToString();
                    user.UserEmail = dr["UserEmail"].ToString();
                    user.ChangePasswordRequired = dr["ChangePasswordRequired"].ToString() == "1" ? true : false;
                    user.UserType = (UserType)dr["UserType"];
                    user.DepartmentId = dr["DepartmentId"].ToString() != string.Empty ? (int?)dr["DepartmentId"] : null;
                    user.DonorId = dr["DonorId"].ToString() != string.Empty ? (int?)dr["DonorId"] : null;
                    user.ClientId = dr["ClientId"].ToString() != string.Empty ? (int?)dr["ClientId"] : null;
                    user.VendorId = dr["VendorId"].ToString() != string.Empty ? (int?)dr["VendorId"] : null;
                    user.AttorneyId = dr["AttorneyId"].ToString() != string.Empty ? (int?)dr["AttorneyId"] : null;
                    user.CourtId = dr["CourtId"].ToString() != string.Empty ? (int?)dr["CourtId"] : null;
                    user.JudgeId = dr["JudgeId"].ToString() != string.Empty ? (int?)dr["JudgeId"] : null;
                    user.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    user.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    user.CreatedOn = (DateTime)dr["CreatedOn"];
                    user.CreatedBy = (string)dr["CreatedBy"];
                    user.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    user.LastModifiedBy = (string)dr["LastModifiedBy"];
                }
                dr.Close();

                if (user != null)
                {
                    if (user.DonorId != null)
                    {
                        string sqlGetTestInfoId = "SELECT donor_id AS DonorId, MAX(donor_test_info_id) AS DonorTestInfoId FROM donor_test_info WHERE donor_id = @DonorId";
                        using (MySqlConnection con = new MySqlConnection(this.ConnectionString))
                        {
                            MySqlParameter[] para = new MySqlParameter[1];
                            para[0] = new MySqlParameter("@DonorId", user.DonorId);

                            MySqlDataReader datatReader = SqlHelper.ExecuteReader(con, CommandType.Text, sqlGetTestInfoId, para);

                            if (datatReader.Read())
                            {
                                if (!string.IsNullOrEmpty(datatReader["DonorTestInfoId"].ToString()))
                                {
                                    string sqlGetTestStatus = " SELECT donor_test_info_id AS DonorTestInfoId, test_status AS TestStatus FROM donor_test_info WHERE donor_test_info_id = @DonorTestInfoId";
                                    MySqlParameter[] para1 = new MySqlParameter[1];
                                    para1[0] = new MySqlParameter("@DonorTestInfoId", datatReader["DonorTestInfoId"]);

                                    datatReader.Close();

                                    MySqlDataReader datatReader1 = SqlHelper.ExecuteReader(con, CommandType.Text, sqlGetTestStatus, para1);

                                    if (datatReader1.Read())
                                    {
                                        if (!string.IsNullOrEmpty(datatReader1["DonorTestInfoId"].ToString()))
                                        {
                                            DonorRegistrationStatus drStatus = datatReader1["TestStatus"] != DBNull.Value ? (DonorRegistrationStatus)(Convert.ToInt32(datatReader1["TestStatus"].ToString())) : DonorRegistrationStatus.None;
                                            if (drStatus == DonorRegistrationStatus.Completed)
                                            {
                                                user.ProgramExists = "No";
                                            }
                                            else
                                            {
                                                user.ProgramExists = "Yes";
                                            }
                                        }
                                        else
                                        {
                                            user.ProgramExists = "No";
                                        }
                                    }
                                    else
                                    {
                                        user.ProgramExists = "No";
                                    }
                                    datatReader1.Close();
                                }
                                else
                                {
                                    user.ProgramExists = "No";
                                }
                            }
                        }
                    }
                }
            }

            return new Tuple<User, bool>(user, authenticated);





        }

   
        
        /// <summary>
        /// Get the User information by Donor Id
        /// </summary>
        /// <param name="DonorId">Donor Id of which one need to be get from the database.</param>
        /// <returns>Returns User information.</returns>
        public User GetByDonorId(int DonorId)
        {
            User user = null;
            string donor_id = DonorId.ToString();
            string sqlQuery = "SELECT user_id AS UserId, user_name AS Username, user_password AS UserPassword, is_user_active AS IsUserActive, user_first_name AS UserFirstName, user_last_name AS UserLastName, user_phone_number AS UserPhoneNumber, user_fax AS UserFax, user_email AS UserEmail, change_password_required AS ChangePasswordRequired, user_type AS UserType, department_id AS DepartmentId, donor_id AS DonorId, client_id AS ClientId, vendor_id AS VendorId, attorney_id AS AttorneyId, court_id AS CourtId, judge_id AS JudgeId, is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM users WHERE is_archived = 0 AND donor_id = @donor_id";

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                MySqlParameter[] param = new MySqlParameter[1];
                param[0] = new MySqlParameter("@donor_id", donor_id);

                MySqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                if (dr.Read())
                {
                    user = new User();
                    user.UserId = (int)dr["UserId"];
                    user.Username = (string)dr["Username"];
                    user.UserPassword = (string)dr["UserPassword"];
                    user.IsUserActive = dr["IsUserActive"].ToString() == "1" ? true : false;
                    user.UserFirstName = (string)dr["UserFirstName"];
                    user.UserLastName = dr["UserLastName"].ToString();
                    user.UserPhoneNumber = dr["UserPhoneNumber"].ToString();
                    user.UserFax = dr["UserFax"].ToString();
                    user.UserEmail = dr["UserEmail"].ToString();
                    user.ChangePasswordRequired = dr["ChangePasswordRequired"].ToString() == "1" ? true : false;
                    user.UserType = (UserType)dr["UserType"];
                    user.DepartmentId = dr["DepartmentId"].ToString() != string.Empty ? (int?)dr["DepartmentId"] : null;
                    user.DonorId = dr["DonorId"].ToString() != string.Empty ? (int?)dr["DonorId"] : null;
                    user.ClientId = dr["ClientId"].ToString() != string.Empty ? (int?)dr["ClientId"] : null;
                    user.VendorId = dr["VendorId"].ToString() != string.Empty ? (int?)dr["VendorId"] : null;
                    user.AttorneyId = dr["AttorneyId"].ToString() != string.Empty ? (int?)dr["AttorneyId"] : null;
                    user.CourtId = dr["CourtId"].ToString() != string.Empty ? (int?)dr["CourtId"] : null;
                    user.JudgeId = dr["JudgeId"].ToString() != string.Empty ? (int?)dr["JudgeId"] : null;
                    user.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    user.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    user.CreatedOn = (DateTime)dr["CreatedOn"];
                    user.CreatedBy = (string)dr["CreatedBy"];
                    user.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    user.LastModifiedBy = (string)dr["LastModifiedBy"];
                }
                dr.Close();

                if (user != null)
                {
                    if (user.DonorId != null)
                    {
                        string sqlGetTestInfoId = "SELECT donor_id AS DonorId, MAX(donor_test_info_id) AS DonorTestInfoId FROM donor_test_info WHERE donor_id = @DonorId";
                        using (MySqlConnection con = new MySqlConnection(this.ConnectionString))
                        {
                            MySqlParameter[] para = new MySqlParameter[1];
                            para[0] = new MySqlParameter("@DonorId", user.DonorId);

                            MySqlDataReader datatReader = SqlHelper.ExecuteReader(con, CommandType.Text, sqlGetTestInfoId, para);

                            if (datatReader.Read())
                            {
                                if (!string.IsNullOrEmpty(datatReader["DonorTestInfoId"].ToString()))
                                {
                                    string sqlGetTestStatus = " SELECT donor_test_info_id AS DonorTestInfoId, test_status AS TestStatus FROM donor_test_info WHERE donor_test_info_id = @DonorTestInfoId";
                                    MySqlParameter[] para1 = new MySqlParameter[1];
                                    para1[0] = new MySqlParameter("@DonorTestInfoId", datatReader["DonorTestInfoId"]);

                                    datatReader.Close();

                                    MySqlDataReader datatReader1 = SqlHelper.ExecuteReader(con, CommandType.Text, sqlGetTestStatus, para1);

                                    if (datatReader1.Read())
                                    {
                                        if (!string.IsNullOrEmpty(datatReader1["DonorTestInfoId"].ToString()))
                                        {
                                            DonorRegistrationStatus drStatus = datatReader1["TestStatus"] != DBNull.Value ? (DonorRegistrationStatus)(Convert.ToInt32(datatReader1["TestStatus"].ToString())) : DonorRegistrationStatus.None;
                                            if (drStatus == DonorRegistrationStatus.Completed)
                                            {
                                                user.ProgramExists = "No";
                                            }
                                            else
                                            {
                                                user.ProgramExists = "Yes";
                                            }
                                        }
                                        else
                                        {
                                            user.ProgramExists = "No";
                                        }
                                    }
                                    else
                                    {
                                        user.ProgramExists = "No";
                                    }
                                    datatReader1.Close();
                                }
                                else
                                {
                                    user.ProgramExists = "No";
                                }
                            }
                        }
                    }
                }
            }

            return user;
        }

        /// <summary>
        /// Get all the User informations.
        /// </summary>
        /// <returns>Returns User information list.</returns>
        public DataTable GetList()
        {
            string sqlQuery = "SELECT user_id AS UserId, user_name AS Username, user_password AS UserPassword, is_user_active AS IsUserActive, user_first_name AS UserFirstName, user_last_name AS UserLastName, user_phone_number AS UserPhoneNumber, user_fax AS UserFax, user_email AS UserEmail, change_password_required AS ChangePasswordRequired, user_type AS UserType, department_id AS DepartmentId, donor_id AS DonorId, client_id AS ClientId, vendor_id AS VendorId, attorney_id AS AttorneyId, court_id AS CourtId, judge_id AS JudgeId, is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM users WHERE is_archived = 0 ORDER BY user_first_name, user_last_name, user_name";

            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sqlQuery);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get all the User informations based on the search criteria.
        /// </summary>
        /// <param name="searchParam">Collection of search criteria</param>
        /// <returns>Returns User information list.</returns>
        public DataTable GetList(Dictionary<string, string> searchParam)
        {
            string generalSearch = string.Empty;
            string generalSearch1 = string.Empty;

            string sqlQuery = "SELECT "
                            + "users.user_id AS UserId, "
                            + "users.user_name AS Username, "
                            + "users.user_password AS UserPassword, "
                            + "users.is_user_active AS IsUserActive, "
                            + "users.user_first_name AS UserFirstName, "
                            + "users.user_last_name AS UserLastName, "
                            + "users.user_phone_number AS UserPhoneNumber, "
                            + "users.user_fax AS UserFax, "
                            + "users.user_email AS UserEmail, "
                            + "users.change_password_required AS ChangePasswordRequired, "
                            + "users.user_type AS UserType, "
                            + "users.department_id AS DepartmentId, "
                            + "users.donor_id AS DonorId, "
                            + "users.client_id AS ClientId, "
                            + "users.vendor_id AS VendorId, "
                            + "users.court_id AS CourtId, "
                            + "users.attorney_id AS AttorneyId, "
                            + "users.judge_id AS JudgeId, "
                            + "users.is_synchronized AS IsSynchronized, "
                            + "users.is_archived AS IsArchived, "
                            + "users.created_on AS CreatedOn, "
                            + "users.created_by AS CreatedBy, "
                            + "users.last_modified_on AS LastModifiedOn, "
                            + "users.last_modified_by AS LastModifiedBy, "
                            + "departments.department_name AS DepartmentName, "
                            + "clients.client_name AS ClientName, "
                            + "vendors.vendor_name AS VendorName, "
                            + "attorneys.attorney_first_name + ' ' + attorneys.attorney_last_name AS AttorneyName, "
                            + "courts.court_name AS CourtName, "
                            + "judges.judge_first_name + ' ' + judges.judge_last_name AS JudgeName "
                            + "FROM users "
                            + "LEFT OUTER JOIN departments ON departments.department_id = users.department_id "
                            + "LEFT OUTER JOIN donors ON donors.donor_id = users.donor_id "
                            + "LEFT OUTER JOIN clients ON clients.client_id = users.client_id "
                            + "LEFT OUTER JOIN vendors ON vendors.vendor_id = users.vendor_id "
                            + "LEFT OUTER JOIN attorneys ON attorneys.attorney_id = users.attorney_id "
                            + "LEFT OUTER JOIN courts ON courts.court_id = users.court_id "
                            + "LEFT OUTER JOIN judges ON judges.judge_id = users.judge_id "
                            + "WHERE users.is_archived = 0 ";

            List<MySqlParameter> param = new List<MySqlParameter>();

            bool isInActiveFlag = false;
            bool isSearchKeyword = false;

            foreach (KeyValuePair<string, string> searchItem in searchParam)
            {
                if (searchItem.Key == "GeneralSearch")
                {
                    generalSearch += "users.user_name LIKE @SearchKeyword OR "
                                + "users.user_first_name LIKE @SearchKeyword OR "
                                + "users.user_last_name LIKE @SearchKeyword OR "
                                + "users.user_phone_number LIKE @SearchKeyword OR "
                                + "users.user_fax LIKE @SearchKeyword OR "
                                + "users.user_email LIKE @SearchKeyword OR "
                                + "users.user_type LIKE @SearchKeyword ";

                    param.Add(new MySqlParameter("@SearchKeyword", searchItem.Value));
                    isSearchKeyword = true;
                }
                else if (searchItem.Key == "UserType")
                {
                    if (searchItem.Value != "All")
                    {
                        sqlQuery += "AND user_type = @UserType ";
                        UserType userType = UserType.None;

                        if (searchItem.Value == "TPA")
                        {
                            userType = UserType.TPA;
                            generalSearch1 += "OR departments.department_name LIKE @SearchKeyword ";
                        }
                        else if (searchItem.Value == "Donor")
                        {
                            userType = UserType.Donor;
                            generalSearch1 += "OR (donors.donor_first_name + ' ' + donors.donor_last_name) LIKE @SearchKeyword";
                        }
                        else if (searchItem.Value == "Client")
                        {
                            userType = UserType.Client;
                            generalSearch1 += "OR clients.client_name LIKE @SearchKeyword ";
                        }
                        else if (searchItem.Value == "Vendor")
                        {
                            userType = UserType.Vendor;
                            generalSearch1 += "OR vendors.vendor_name LIKE @SearchKeyword ";
                        }
                        else if (searchItem.Value == "Attorney")
                        {
                            userType = UserType.Attorney;
                            generalSearch1 += "OR (attorneys.attorney_first_name + ' ' + attorneys.attorney_last_name) LIKE @SearchKeyword ";
                        }
                        else if (searchItem.Value == "Court")
                        {
                            userType = UserType.Court;
                            generalSearch1 += "OR courts.court_name LIKE @SearchKeyword ";
                        }
                        else if (searchItem.Value == "Judge")
                        {
                            userType = UserType.Judge;
                            generalSearch1 += "OR (judges.judge_first_name + ' ' + judges.judge_last_name) LIKE @SearchKeyword ";
                        }

                        param.Add(new MySqlParameter("@UserType", (int)userType));
                    }
                }
                else if (searchItem.Key == "IncludeInactive")
                {
                    if (Convert.ToBoolean(searchItem.Value))
                    {
                        isInActiveFlag = true;
                    }
                }
            }

            if (!isInActiveFlag)
            {
                sqlQuery += "AND is_user_active = b'1' ";
            }

            if (generalSearch != string.Empty && isSearchKeyword)
            {
                sqlQuery += "AND (" + generalSearch + generalSearch1 + ") ";
            }

            sqlQuery += "ORDER BY user_first_name, user_last_name, user_name";
            //sqlQuery += " Limit 500";

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

        /// <summary>
        /// Get all the User Authorization Rules informations.
        /// </summary>
        /// <returns>Returns User Authorization Rules information list.</returns>
        public DataTable GetUserAuthorizationRules(string username)
        {
            string sqlQuery = "select "
                            + "users.user_id AS UserId, "
                            + "users.user_name AS Username, "
                            + "auth_rules.auth_rule_id AS AuthRuleId, "
                            + "auth_rules.auth_rule_name AS AuthRuleName,  "
                            + "auth_rules.internal_name AS AuthRuleInternalName, "
                            + "auth_rules.parent_auth_rule_id AS AuthRuleParentId, "
                            + "auth_rule_categories.auth_rule_category_id AS AuthRuleCategoryId, "
                            + "auth_rule_categories.auth_rule_category_name AS AuthRuleCategoryName, "
                            + "auth_rule_categories.internal_name AS AuthRuleCategoryInternalName, "
                            + "auth_rule_categories.parent_auth_rule_category_id AS AuthRuleCategoryParentId "
                            + "FROM "
                            + "user_auth_rules "
                            + "INNER JOIN users ON users.user_id = user_auth_rules.user_id "
                            + "INNER JOIN auth_rules ON auth_rules.auth_rule_id = user_auth_rules.auth_rule_id "
                            + "INNER JOIN auth_rule_categories ON auth_rule_categories.auth_rule_category_id = auth_rules.auth_rule_category_id WHERE UPPER(users.user_name) = UPPER(@Username) ORDER BY auth_rule_categories.auth_rule_category_id, auth_rules.auth_rule_id";

            MySqlParameter[] param = new MySqlParameter[1];
            param[0] = new MySqlParameter("@Username", username);

            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sqlQuery, param);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        public DataTable GetUserAuthCategories()
        {
            string sqlQuery = "select "
                            + "auth_rule_category_id AS AuthRuleCategoryId, "
                            + "auth_rule_category_name AS AuthRuleCategoryName, "
                            + "internal_name AS AuthRuleCategoryInternalName, "
                            + "parent_auth_rule_category_id AS AuthRuleCategoryParentId, "
                            + "is_active AS IsActive, "
                            + "user_type AS UserType "
                            + "FROM "
                            + "auth_rule_categories WHERE is_archived = 0 ORDER BY auth_rule_category_id";

            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sqlQuery);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        public DataTable GetUserAuthRules()
        {
            string sqlQuery = "select "
                            + "auth_rule_id AS AuthRuleId, "
                            + "auth_rule_name AS AuthRuleName,  "
                            + "internal_name AS AuthRuleInternalName, "
                            + "parent_auth_rule_id AS AuthRuleParentId, "
                            + "auth_rule_category_id AS AuthRuleCategoryId "
                            + "FROM "
                            + "auth_rules "
                            + "WHERE is_archived = 0 ORDER BY auth_rule_id";

            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sqlQuery);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get all the User informations by email.
        /// </summary>
        /// <param name="email">Email address.</param>
        /// <returns>Returns User information list.</returns>
        public DataTable GetList(string userEmail)
        {
            string sqlQuery = "SELECT user_id AS UserId, user_name AS Username, user_password AS UserPassword, is_user_active AS IsUserActive, user_first_name AS UserFirstName, user_last_name AS UserLastName, user_phone_number AS UserPhoneNumber, user_fax AS UserFax, user_email AS UserEmail, change_password_required AS ChangePasswordRequired, user_type AS UserType, department_id AS DepartmentId, donor_id AS DonorId, client_id AS ClientId, vendor_id AS VendorId, attorney_id AS AttorneyId, court_id AS CourtId, judge_id AS JudgeId, is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM users WHERE is_archived = 0 AND UPPER(user_email) = UPPER(@UserEmail) ORDER BY user_first_name,  user_last_name, user_name";

            MySqlParameter[] param = new MySqlParameter[1];
            param[0] = new MySqlParameter("@UserEmail", userEmail);

            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sqlQuery, param);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get all the User informations by email.
        /// </summary>
        /// <param name="departmentName">Department Name.</param>
        /// <returns>Returns User information list.</returns>
        public DataTable GetListByDepartment(string departmentName)
        {
            string sqlQuery = "SELECT "
                                + "user_id AS UserId, "
                                + "user_name AS Username, "
                                + "user_password AS UserPassword, "
                                + "is_user_active AS IsUserActive, "
                                + "user_first_name AS UserFirstName, "
                                + "user_last_name AS UserLastName, "
                                + "user_phone_number AS UserPhoneNumber, "
                                + "user_fax AS UserFax, "
                                + "user_email AS UserEmail, "
                                + "change_password_required AS ChangePasswordRequired, "
                                + "user_type AS UserType, "
                                + "department_id AS DepartmentId, "
                                + "donor_id AS DonorId, "
                                + "client_id AS ClientId, "
                                + "vendor_id AS VendorId, "
                                + "attorney_id AS AttorneyId, "
                                + "court_id AS CourtId, "
                                + "judge_id AS JudgeId, "
                                + "is_synchronized AS IsSynchronized, "
                                + "is_archived AS IsArchived, "
                                + "created_on AS CreatedOn, "
                                + "created_by AS CreatedBy, "
                                + "last_modified_on AS LastModifiedOn, "
                                + "last_modified_by AS LastModifiedBy "
                                + "FROM users WHERE is_archived = 0 AND "
                                + "department_id IN (SELECT department_id FROM departments WHERE UPPER(department_name) = UPPER(@DepartmentName)) "
                                + "ORDER BY user_first_name,  user_last_name, user_name";

            MySqlParameter[] param = new MySqlParameter[1];
            param[0] = new MySqlParameter("@DepartmentName", departmentName);

            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sqlQuery, param);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get all the User Authorization Rules Categories informations.
        /// </summary>
        /// <returns>Returns User Authorization Rules Categories information list.</returns>
        public DataTable GetUserAuthorizationRulesCategories()
        {
            // string sqlQuery = "select auth_rule_category_id AS AuthRuleCategoryId,auth_rule_category_name AS AuthRuleCategoryName,internal_name AS InternalName,parent_auth_rule_category_id AS ParentAuthRuleCategoryId,is_active AS IsActive,user_type AS UserType,is_synchronized AS IsSynchronized,is_archived AS IsArchived,created_on AS CreatedOn,created_by AS CreatedBy,last_modified_on AS LastModifiedOn,last_modified_by AS LastModifiedBy FROM auth_rule_categories WHERE is_archived = 0  ORDER BY auth_rule_category_id";
            string sqlQuery = "select distinct AuthRuleCategories.auth_rule_category_name as AuthRuleCategories_authrulecategoryname, AuthRuleCategories.auth_rule_category_id as AuthRuleCategories_authrulecategoryid "
                             + "from auth_rule_categories as AuthRuleCategories "
                             + "left outer join auth_rule_categories as down "
                             + "on down.parent_auth_rule_category_id=AuthRuleCategories.auth_rule_category_id "
                             + "where AuthRuleCategories.parent_auth_rule_category_id is null "
                             + "order by AuthRuleCategories_authrulecategoryname";

            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sqlQuery);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get all the User Authorization Rules Sub Categories informations.
        /// </summary>
        /// <returns>Returns User Authorization Rules Sub Categories information list.</returns>
        public DataTable GetUserAuthorizationRulesSubCategories(int AuthRuleCategoryId)
        {
            string sqlQuery = "Select distinct AuthRuleCategories.auth_rule_category_name as AuthRuleCategories_authrulecategoryname,AuthRuleCategories.auth_rule_category_id as AuthRuleCategories_AuthRuleCategoryId,AuthRuleCategories.parent_auth_rule_category_id as AuthRuleCategories_ParentAuthRuleCategoryId from auth_rule_categories as AuthRuleCategories "
                              + "left outer join auth_rule_categories as down on AuthRuleCategories.auth_rule_category_id=down.parent_auth_rule_category_id "
                              + "where AuthRuleCategories.parent_auth_rule_category_id=@AuthRuleCategoryId and AuthRuleCategories.parent_auth_rule_category_id is not null order by AuthRuleCategories_authrulecategoryname ";

            MySqlParameter[] param = new MySqlParameter[1];
            param[0] = new MySqlParameter("@AuthRuleCategoryId", AuthRuleCategoryId);

            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sqlQuery, param);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        public DataTable GetUserAuthorizationRules(int AuthRuleCategoryId)
        {
            string sqlQuery = "Select AuthRules.auth_rule_id as AuthRuleId, AuthRules.auth_rule_name as AuthRuleName, AuthRules.auth_rule_category_id as AuthRuleCategoryId from auth_rules as AuthRules left outer join auth_rule_categories as AuthRuleCategories on AuthRuleCategories.auth_rule_category_id=AuthRules.auth_rule_category_id "
                              + "where AuthRuleCategories.auth_rule_category_id=@AuthRuleCategoryId "
                              + "and AuthRules.is_archived = 0 order by AuthRules.auth_rule_name ";

            MySqlParameter[] param = new MySqlParameter[2];
            param[0] = new MySqlParameter("@AuthRuleCategoryId", AuthRuleCategoryId);
            //  param[1] = new MySqlParameter("@AuthRuleSubCategoryName", AuthRuleSubCategoryName);

            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sqlQuery, param);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        public DataTable GetUserDepartment(int UserId)
        {
            string sqlQuery = "SELECT client_department_id AS ClientDepartmentId FROM user_departments WHERE user_id = @UserId";

            MySqlParameter[] param = new MySqlParameter[1];
            param[0] = new MySqlParameter("@UserId", UserId);

            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sqlQuery, param);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        public User GetDonor(int donorId)
        {
            User user = null;
            string sqlQuery = "SELECT user_id AS UserId, user_name AS Username, user_password AS UserPassword, is_user_active AS IsUserActive, user_first_name AS UserFirstName, user_last_name AS UserLastName, user_phone_number AS UserPhoneNumber, user_fax AS UserFax, user_email AS UserEmail, change_password_required AS ChangePasswordRequired, user_type AS UserType, department_id AS DepartmentId, donor_id AS DonorId, client_id AS ClientId, vendor_id AS VendorId, attorney_id AS AttorneyId, court_id AS CourtId, judge_id AS JudgeId, is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM users WHERE donor_id = @DonorId";

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                MySqlParameter[] param = new MySqlParameter[1];
                param[0] = new MySqlParameter("@DonorId", donorId);

                MySqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                if (dr.Read())
                {
                    user = new User();
                    user.UserId = (int)dr["UserId"];
                    user.Username = (string)dr["Username"];
                    user.UserPassword = (string)dr["UserPassword"];
                    user.IsUserActive = dr["IsUserActive"].ToString() == "1" ? true : false;
                    user.UserFirstName = (string)dr["UserFirstName"];
                    user.UserLastName = dr["UserLastName"].ToString();
                    user.UserPhoneNumber = dr["UserPhoneNumber"].ToString();
                    user.UserFax = dr["UserFax"].ToString();
                    user.UserEmail = dr["UserEmail"].ToString();
                    user.ChangePasswordRequired = dr["ChangePasswordRequired"].ToString() == "1" ? true : false;
                    user.UserType = (UserType)dr["UserType"];
                    user.DepartmentId = dr["DepartmentId"].ToString() != string.Empty ? (int?)dr["DepartmentId"] : null;
                    user.DonorId = dr["DonorId"].ToString() != string.Empty ? (int?)dr["DonorId"] : null;
                    user.ClientId = dr["ClientId"].ToString() != string.Empty ? (int?)dr["ClientId"] : null;
                    user.VendorId = dr["VendorId"].ToString() != string.Empty ? (int?)dr["VendorId"] : null;
                    user.AttorneyId = dr["AttorneyId"].ToString() != string.Empty ? (int?)dr["AttorneyId"] : null;
                    user.CourtId = dr["CourtId"].ToString() != string.Empty ? (int?)dr["CourtId"] : null;
                    user.JudgeId = dr["JudgeId"].ToString() != string.Empty ? (int?)dr["JudgeId"] : null;
                    user.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    user.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    user.CreatedOn = (DateTime)dr["CreatedOn"];
                    user.CreatedBy = (string)dr["CreatedBy"];
                    user.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    user.LastModifiedBy = (string)dr["LastModifiedBy"];
                }
                dr.Close();

                //if (user != null)
                //{
                //    sqlQuery = "SELECT client_department_id AS ClientDepartmentId FROM user_departments WHERE user_id = @UserId";

                //    dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);
                //    while (dr.Read())
                //    {
                //        user.ClientDepartmentList.Add(Convert.ToInt32(dr["ClientDepartmentId"]));
                //    }
                //    dr.Close();

                //    //
                //    sqlQuery = "SELECT auth_rule_id AS AuthRuleId FROM user_auth_rules WHERE user_id = @UserId";

                //    dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);
                //    while (dr.Read())
                //    {
                //        user.AuthRuleList.Add(Convert.ToInt32(dr["AuthRuleId"]));
                //    }
                //    dr.Close();
                //}
            }

            return user;
        }

        public DataTable GetByEmail(string Email)
        {
            string sqlQuery = "SELECT user_id AS UserId, user_name AS Username, user_password AS UserPassword, is_user_active AS IsUserActive, user_first_name AS UserFirstName, user_last_name AS UserLastName, user_phone_number AS UserPhoneNumber, user_fax AS UserFax, user_email AS UserEmail, change_password_required AS ChangePasswordRequired, user_type AS UserType, department_id AS DepartmentId, donor_id AS DonorId, client_id AS ClientId, vendor_id AS VendorId, attorney_id AS AttorneyId, court_id AS CourtId, judge_id AS JudgeId, is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM users WHERE is_archived = 0 AND (UPPER(user_name) = UPPER(@UserEmail) OR UPPER(user_email) = UPPER(@UserEmail)) ORDER BY user_first_name, user_last_name, user_name";

            MySqlParameter[] param = new MySqlParameter[1];
            param[0] = new MySqlParameter("@UserEmail", Email);

            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sqlQuery, param);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        public DataTable sorting(string searchparam, string user, bool active, string getInActive = null)
        {
            string sql = string.Empty;

            string sqlQuery = "SELECT "
                            + "users.user_id AS UserId, "
                            + "users.user_name AS Username, "
                            + "users.user_password AS UserPassword, "
                            + "users.is_user_active AS IsUserActive, "
                            + "users.user_first_name AS UserFirstName, "
                            + "users.user_last_name AS UserLastName, "
                            + "users.user_phone_number AS UserPhoneNumber, "
                            + "users.user_fax AS UserFax, "
                            + "users.user_email AS UserEmail, "
                            + "users.change_password_required AS ChangePasswordRequired, "
                            + "users.user_type AS UserType, "
                            + "users.department_id AS DepartmentId, "
                            + "users.donor_id AS DonorId, "
                            + "users.client_id AS ClientId, "
                            + "users.vendor_id AS VendorId, "
                            + "users.court_id AS CourtId, "
                            + "users.attorney_id AS AttorneyId, "
                            + "users.judge_id AS JudgeId, "
                            + "users.is_synchronized AS IsSynchronized, "
                            + "users.is_archived AS IsArchived, "
                            + "users.created_on AS CreatedOn, "
                            + "users.created_by AS CreatedBy, "
                            + "users.last_modified_on AS LastModifiedOn, "
                            + "users.last_modified_by AS LastModifiedBy, "
                            + "departments.department_name AS DepartmentName, "
                            + "clients.client_name AS ClientName, "
                            + "vendors.vendor_name AS VendorName, "
                            + "attorneys.attorney_first_name + ' ' + attorneys.attorney_last_name AS AttorneyName, "
                            + "courts.court_name AS CourtName, "
                            + "judges.judge_first_name + ' ' + judges.judge_last_name AS JudgeName "
                            + "FROM users "
                            + "LEFT OUTER JOIN departments ON departments.department_id = users.department_id "
                            + "LEFT OUTER JOIN donors ON donors.donor_id = users.donor_id "
                            + "LEFT OUTER JOIN clients ON clients.client_id = users.client_id "
                            + "LEFT OUTER JOIN vendors ON vendors.vendor_id = users.vendor_id "
                            + "LEFT OUTER JOIN attorneys ON attorneys.attorney_id = users.attorney_id "
                            + "LEFT OUTER JOIN courts ON courts.court_id = users.court_id "
                            + "LEFT OUTER JOIN judges ON judges.judge_id = users.judge_id "
                            + "WHERE users.is_archived = 0  ";

            if (user == "TPA")
            {
                sqlQuery += "AND users.user_type = 1 ";
            }
            if (user == "Donor")
            {
                sqlQuery += "AND users.user_type = 2 ";
            }
            if (user == "Client")
            {
                sqlQuery += "AND users.user_type = 3 ";
            }
            if (user == "Vendor")
            {
                sqlQuery += "AND users.user_type = 4 ";
            }
            if (user == "Attorney")
            {
                sqlQuery += "AND users.user_type = 5 ";
            }

            if (user == "Court")
            {
                sqlQuery += "AND users.user_type = 6 ";
            }
            if (user == "Judge")
            {
                sqlQuery += "AND users.user_type = 7 ";
            }

            if (active == false)
            {
                sqlQuery += "AND is_user_active = b'1'";
            }

            if (searchparam == "firstName")
            {
                if (getInActive == "1")
                {
                    sql = "ORDER BY user_first_name";
                }
                else
                {
                    sql = "ORDER BY user_first_name desc";
                }
            }
            if (searchparam == "lastName")
            {
                if (getInActive == "1")
                {
                    sql = "ORDER BY user_last_name";
                }
                else
                {
                    sql = "ORDER BY user_last_name desc";
                }
            }
            if (searchparam == "userName")
            {
                if (getInActive == "1")
                {
                    sql = "ORDER BY user_name";
                }
                else
                {
                    sql = "ORDER BY user_name desc";
                }
            }
            if (searchparam == "userType")
            {
                if (getInActive == "1")
                {
                    sql = "ORDER BY user_type";
                }
                else
                {
                    sql = "ORDER BY user_type desc";
                }
            }
            if (searchparam == "userTypeNames")
            {
                if (getInActive == "1")
                {
                    sql = "ORDER BY user_type";
                }
                else
                {
                    sql = "ORDER BY user_type desc";
                }
            }

            if (searchparam == "isActive")
            {
                if (getInActive == "1")
                {
                    sql = "ORDER BY is_user_active";
                }
                else
                {
                    sql = "ORDER BY is_user_active desc";
                }
            }

            sqlQuery = sqlQuery + sql;

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sqlQuery);
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

        /// <summary>
        /// Updates the User information to the database.
        /// </summary>
        /// <param name="user">User information which one need to be updated to the database.</param>
        /// <returns>Returns number of records affected in the database.</returns>
        public int UserUpdate(User user)
        {
            int returnValue = 0;

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();

                try
                {
                    string sqlQuery = "UPDATE users SET "
                                    + "user_name = @Username, "
                                    + "user_password = @UserPassword, "
                                    + "is_user_active = @IsUserActive, "
                                    + "user_first_name = @UserFirstName, "
                                    + "user_last_name = @UserLastName, "
                                    + "user_phone_number = @UserPhoneNumber, "
                                    + "user_fax = @UserFax, "
                                    + "user_email = @UserEmail, "
                                    + "change_password_required = @ChangePasswordRequired, "
                                    + "user_type = @UserType, "
                                    + "department_id = @DepartmentId, "
                                    + "donor_id  = @DonorId, "
                                    + "client_id = @ClientId, "
                                    + "vendor_id = @VendorId, "
                                    + "attorney_id  = @AttorneyId, "
                                    + "court_id = @CourtId, "
                                    + "judge_id = @JudgeId, "
                                    + "is_synchronized = b'0', "
                                    + "last_modified_on = NOW(), "
                                    + "last_modified_by = @LastModifiedBy "
                                    + "WHERE user_id = @UserId";

                    MySqlParameter[] param = new MySqlParameter[19];
                    param[0] = new MySqlParameter("@UserId", user.UserId);
                    param[1] = new MySqlParameter("@Username", user.Username);
                    param[2] = new MySqlParameter("@UserPassword", user.UserPassword);
                    param[3] = new MySqlParameter("@IsUserActive", user.IsUserActive);
                    param[4] = new MySqlParameter("@UserFirstName", user.UserFirstName);
                    param[5] = new MySqlParameter("@UserLastName", user.UserLastName);
                    param[6] = new MySqlParameter("@UserPhoneNumber", user.UserPhoneNumber);
                    param[7] = new MySqlParameter("@UserFax", user.UserFax);
                    param[8] = new MySqlParameter("@UserEmail", user.UserEmail);
                    param[9] = new MySqlParameter("@ChangePasswordRequired", user.ChangePasswordRequired);
                    param[10] = new MySqlParameter("@UserType", user.UserType);
                    param[11] = new MySqlParameter("@DepartmentId", user.DepartmentId);
                    param[12] = new MySqlParameter("@DonorId", user.DonorId);
                    param[13] = new MySqlParameter("@ClientId", user.ClientId);
                    param[14] = new MySqlParameter("@VendorId", user.VendorId);
                    param[15] = new MySqlParameter("@AttorneyId", user.AttorneyId);
                    param[16] = new MySqlParameter("@CourtId", user.CourtId);
                    param[17] = new MySqlParameter("@JudgeId", user.JudgeId);
                    param[18] = new MySqlParameter("@LastModifiedBy", user.LastModifiedBy);

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                    trans.Commit();

                    returnValue = 1;
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

            return returnValue;
        }

        public DataTable GetVendorName(int vendorId)
        {
            string sqlQuery = "SELECT user_id AS UserId, user_name AS Username, user_password AS UserPassword, is_user_active AS IsUserActive, user_first_name AS UserFirstName, user_last_name AS UserLastName, user_phone_number AS UserPhoneNumber, user_fax AS UserFax, user_email AS UserEmail, change_password_required AS ChangePasswordRequired, user_type AS UserType, department_id AS DepartmentId, donor_id AS DonorId, client_id AS ClientId, vendor_id AS VendorId, attorney_id AS AttorneyId, court_id AS CourtId, judge_id AS JudgeId, is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM users WHERE is_archived = 0 AND vendor_id = @VendorId";

            MySqlParameter[] param = new MySqlParameter[1];
            param[0] = new MySqlParameter("@VendorId", vendorId);

            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sqlQuery, param);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        #endregion Public Methods
    }
}