using MySql.Data.MySqlClient;
using SurPath.Entity;
using SurPath.Entity.Master;
using SurPath.MySQLHelper;
using System;
using System.Collections.Generic;
using System.Data;

namespace SurPath.Data.Master
{
    public class JudgeDao : DataObject
    {
        #region Constructor

        public JudgeDao()
        {
            //
        }

        #endregion Constructor

        #region Public Methods

        public int Insert(Judge judge, User user)
        {
            int returnValue = 0;

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();

                try
                {
                    string sqlQuery = "INSERT INTO judges (judge_user_name,judge_prefix,judge_first_name,judge_last_name,judge_suffix,judge_address_1,judge_address_2,judge_city,judge_state,judge_zip, is_active, is_synchronized, is_archived, created_on, created_by, last_modified_on, last_modified_by) VALUES (";
                    sqlQuery += "@JudgeUsername, @JudgePrefix, @JudgeFirstName, @JudgeLastName, @JudgeSuffix, @JudgeAddress1, @JudgeAddress2, @JudgeCity, @JudgeState, @JudgeZip, @IsActive, b'0', b'0', NOW(), @CreatedBy, NOW(), @LastModifiedBy)";

                    MySqlParameter[] param = new MySqlParameter[13];
                    param[0] = new MySqlParameter("@JudgeUsername", judge.JudgeUsername);
                    param[1] = new MySqlParameter("@JudgePrefix", judge.JudgePrefix);
                    param[2] = new MySqlParameter("@JudgeFirstName", judge.JudgeFirstName);
                    param[3] = new MySqlParameter("@JudgeLastName", judge.JudgeLastName);
                    param[4] = new MySqlParameter("@JudgeSuffix", judge.JudgeSuffix);
                    param[5] = new MySqlParameter("@JudgeAddress1", judge.JudgeAddress1);
                    param[6] = new MySqlParameter("@JudgeAddress2", judge.JudgeAddress2);
                    param[7] = new MySqlParameter("@JudgeCity", judge.JudgeCity);
                    param[8] = new MySqlParameter("@JudgeState", judge.JudgeState);
                    param[9] = new MySqlParameter("@JudgeZip", judge.JudgeZip);
                    param[10] = new MySqlParameter("@IsActive", judge.IsActive);
                    param[11] = new MySqlParameter("@CreatedBy", judge.CreatedBy);
                    param[12] = new MySqlParameter("@LastModifiedBy", judge.LastModifiedBy);

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                    sqlQuery = "SELECT LAST_INSERT_ID()";

                    returnValue = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlQuery));

                    judge.JudgeId = returnValue;
                    user.JudgeId = returnValue;

                    sqlQuery = "INSERT INTO users (user_name, user_password, is_user_active, user_first_name, user_last_name, user_phone_number, user_fax, user_email, change_password_required, user_type, department_id, donor_id, client_id, vendor_id, attorney_id, court_id, judge_id, is_synchronized, is_archived, created_on, created_by, last_modified_on, last_modified_by) VALUES (";
                    sqlQuery += "@Username, @UserPassword, @IsUserActive, @UserFirstName, @UserLastName, @UserPhoneNumber, @UserFax, @UserEmail, @ChangePasswordRequired, @UserType, @DepartmentId, @DonorId, @ClientId, @VendorId, @AttorneyId, @CourtId, @JudgeId, b'0', b'0', NOW(), @CreatedBy, NOW(), @LastModifiedBy)";

                    param = new MySqlParameter[19];
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
                    param[17] = new MySqlParameter("@CreatedBy", "SYSTEM");
                    param[18] = new MySqlParameter("@LastModifiedBy", "SYSTEM");

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                    sqlQuery = "SELECT LAST_INSERT_ID()";

                    user.UserId = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlQuery));

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

        public int Update(Judge judge)
        {
            int returnValue = 0;

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();

                try
                {
                    string sqlQuery = "UPDATE judges SET "
                                    + "judge_prefix = @JudgePrefix, "
                                    + "judge_first_name = @JudgeFirstName, "
                                    + "judge_last_name = @JudgeLastName, "
                                    + "judge_suffix = @JudgeSuffix, "
                                    + "judge_address_1 = @JudgeAddress1, "
                                    + "judge_address_2 =@JudgeAddress2, "
                                    + "judge_city = @JudgeCity, "
                                    + "judge_state =@JudgeState, "
                                    + "judge_zip = @JudgeZip, "
                                    + "is_active=@IsActive, "
                                    + "is_synchronized = b'0', "
                                    + "last_modified_on = NOW(), "
                                    + "last_modified_by = @LastModifiedBy "
                                    + "WHERE judge_id = @JudgeId";

                    MySqlParameter[] param = new MySqlParameter[13];
                    param[0] = new MySqlParameter("@JudgeId", judge.JudgeId);
                    param[1] = new MySqlParameter("@JudgeUsername", judge.JudgeUsername);
                    param[2] = new MySqlParameter("@JudgePrefix", judge.JudgePrefix);
                    param[3] = new MySqlParameter("@JudgeFirstName", judge.JudgeFirstName);
                    param[4] = new MySqlParameter("@JudgeLastName", judge.JudgeLastName);
                    param[5] = new MySqlParameter("@JudgeSuffix", judge.JudgeSuffix);
                    param[6] = new MySqlParameter("@JudgeAddress1", judge.JudgeAddress1);
                    param[7] = new MySqlParameter("@JudgeAddress2", judge.JudgeAddress2);
                    param[8] = new MySqlParameter("@JudgeCity", judge.JudgeCity);
                    param[9] = new MySqlParameter("@JudgeState", judge.JudgeState);
                    param[10] = new MySqlParameter("@JudgeZip", judge.JudgeZip);
                    param[11] = new MySqlParameter("@IsActive", judge.IsActive);
                    param[12] = new MySqlParameter("@LastModifiedBy", judge.LastModifiedBy);

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                    trans.Commit();

                    returnValue = 1;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }

            return returnValue;
        }

        public int Delete(int judgeId, string currentUsername)
        {
            int returnValue = 0;

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();

                try
                {
                    string sqlCount1Query = "Select count(*) from donor_test_info where judge_id = " + judgeId + " and test_status != 7";

                    int table1 = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlCount1Query));

                    if (table1 <= 0)
                    {
                        string sqlQuery = "UPDATE judges SET is_synchronized = b'0', is_archived = b'1', last_modified_on = NOW(), last_modified_by = @LastModifiedBy WHERE judge_id = @JudgeId";

                        MySqlParameter[] param = new MySqlParameter[2];
                        param[0] = new MySqlParameter("@JudgeId", judgeId);
                        param[1] = new MySqlParameter("@LastModifiedBy", currentUsername);

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                        trans.Commit();

                        returnValue = 1;
                    }
                    else
                    {
                        //
                    }
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
            return returnValue;
        }

        public Judge Get(int judgeId)
        {
            Judge judge = null;
            string sqlQuery = "SELECT judge_id AS JudgeId, judge_user_name AS JudgeUsername, judge_prefix AS JudgePrefix, judge_first_name AS JudgeFirstName, judge_last_name AS JudgeLastName, judge_suffix AS JudgeSuffix, judge_address_1 AS JudgeAddress1, judge_address_2 AS JudgeAddress2, judge_city AS JudgeCity, judge_state AS JudgeState, judge_zip AS JudgeZip, is_active AS IsActive, is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM judges WHERE judge_id = @JudgeId";

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                MySqlParameter[] param = new MySqlParameter[1];
                param[0] = new MySqlParameter("@JudgeId", judgeId);

                MySqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                if (dr.Read())
                {
                    judge = new Judge();
                    judge.JudgeId = (int)dr["JudgeId"];
                    judge.JudgeUsername = (string)dr["JudgeUsername"];
                    judge.JudgePrefix = dr["JudgePrefix"].ToString();
                    judge.JudgeFirstName = dr["JudgeFirstName"].ToString();
                    judge.JudgeLastName = dr["JudgeLastName"].ToString();
                    judge.JudgeSuffix = dr["JudgeSuffix"].ToString();
                    judge.JudgeAddress1 = dr["JudgeAddress1"].ToString();
                    judge.JudgeAddress2 = dr["JudgeAddress2"].ToString();
                    judge.JudgeCity = dr["JudgeCity"].ToString();
                    judge.JudgeState = dr["JudgeState"].ToString();
                    judge.JudgeZip = dr["JudgeZip"].ToString();
                    judge.IsActive = dr["IsActive"].ToString() == "1" ? true : false;
                    judge.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    judge.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    judge.CreatedOn = (DateTime)dr["CreatedOn"];
                    judge.CreatedBy = (string)dr["CreatedBy"];
                    judge.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    judge.LastModifiedBy = (string)dr["LastModifiedBy"];
                }
            }

            return judge;
        }

        public DataTable GetByEmail(string email)
        {
            string sqlQuery = "SELECT judge_id AS JudgeId, judge_user_name AS JudgeUsername, judge_prefix AS JudgePrefix, judge_first_name AS JudgeFirstName, judge_last_name AS JudgeLastName, judge_suffix AS JudgeSuffix, judge_address_1 AS JudgeAddress1, judge_address_2 AS JudgeAddress2, judge_city AS JudgeCity, judge_state AS JudgeState, judge_zip AS JudgeZip, is_active AS IsActive, is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM judges WHERE is_archived = 0 AND Upper(judge_user_name) = UPPER(@JudgeUsername)";

            MySqlParameter[] param = new MySqlParameter[1];
            param[0] = new MySqlParameter("@JudgeUsername", email);

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

        public DataTable GetList()
        {
            string sqlQuery = "SELECT judge_id AS JudgeId, judge_user_name AS JudgeUsername, judge_prefix AS JudgePrefix, judge_first_name AS JudgeFirstName, judge_last_name AS JudgeLastName, judge_suffix AS JudgeSuffix, judge_address_1 AS JudgeAddress1, judge_address_2 AS JudgeAddress2, judge_city AS JudgeCity, judge_state AS JudgeState, judge_zip AS JudgeZip, is_active AS IsActive, is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM judges WHERE is_archived = 0 ORDER BY judge_first_name";

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

        public DataTable GetList(Dictionary<string, string> searchParam)
        {
            string sqlQuery = "SELECT judge_id AS JudgeId, judge_user_name AS JudgeUsername, judge_prefix AS JudgePrefix, judge_first_name AS JudgeFirstName, judge_last_name AS JudgeLastName, judge_suffix AS JudgeSuffix, judge_address_1 AS JudgeAddress1, judge_address_2 AS JudgeAddress2, judge_city AS JudgeCity, judge_state AS JudgeState, judge_zip AS JudgeZip, is_active AS IsActive, is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM judges WHERE is_archived = 0 ";

            List<MySqlParameter> param = new List<MySqlParameter>();
            bool isInActiveFlag = false;

            foreach (KeyValuePair<string, string> searchItem in searchParam)
            {
                if (searchItem.Key == "GeneralSearch")
                {
                    sqlQuery += "AND ( judge_user_name LIKE @SearchKeyword OR judge_prefix LIKE @SearchKeyword OR judge_first_name LIKE @SearchKeyword OR judge_last_name LIKE @SearchKeyword OR judge_suffix LIKE @SearchKeyword OR judge_address_1 LIKE @SearchKeyword OR judge_address_2 LIKE @SearchKeyword OR judge_city LIKE @SearchKeyword OR judge_state LIKE @SearchKeyword OR judge_zip LIKE @SearchKeyword) ";

                    param.Add(new MySqlParameter("@SearchKeyword", searchItem.Value));
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
                sqlQuery += "AND is_active = b'1' ";
            }

            sqlQuery += "ORDER BY judge_first_name";

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

        public DataTable sorting(string searchparam, bool active, string getInActive = null)
        {
            string sql = string.Empty;
            string sqlQuery = "SELECT judge_id AS JudgeId, judge_user_name AS JudgeUsername, judge_prefix AS JudgePrefix, judge_first_name AS JudgeFirstName, judge_last_name AS JudgeLastName, judge_suffix AS JudgeSuffix, judge_address_1 AS JudgeAddress1, judge_address_2 AS JudgeAddress2, judge_city AS JudgeCity, judge_state AS JudgeState, judge_zip AS JudgeZip, is_active AS IsActive, is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM judges WHERE is_archived = 0 ";

            if (active == false)
            {
                sqlQuery += "AND is_active = b'1'";
            }

            if (searchparam == "firstName")
            {
                if (getInActive == "1")
                {
                    sql = "ORDER BY JudgeFirstName";
                }
                else
                {
                    sql = "ORDER BY JudgeFirstName desc";
                }
            }
            if (searchparam == "lastName")
            {
                if (getInActive == "1")
                {
                    sql = "ORDER BY JudgeLastName";
                }
                else
                {
                    sql = "ORDER BY JudgeLastName desc";
                }
            }
            if (searchparam == "suffix")
            {
                if (getInActive == "1")
                {
                    sql = "ORDER BY JudgeSuffix";
                }
                else
                {
                    sql = "ORDER BY JudgeSuffix desc";
                }
            }
            if (searchparam == "userName")
            {
                if (getInActive == "1")
                {
                    sql = "ORDER BY JudgeUsername";
                }
                else
                {
                    sql = "ORDER BY JudgeUsername desc";
                }
            }
            if (searchparam == "city")
            {
                if (getInActive == "1")
                {
                    sql = "ORDER BY JudgeCity";
                }
                else
                {
                    sql = "ORDER BY JudgeCity desc";
                }
            }
            if (searchparam == "state")
            {
                if (getInActive == "1")
                {
                    sql = "ORDER BY JudgeState";
                }
                else
                {
                    sql = "ORDER BY JudgeState desc";
                }
            }
            if (searchparam == "isActive")
            {
                if (getInActive == "1")
                {
                    sql = "ORDER BY is_active";
                }
                else
                {
                    sql = "ORDER BY is_active desc";
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

        #endregion Public Methods
    }
}