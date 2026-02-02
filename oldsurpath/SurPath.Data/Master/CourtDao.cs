using MySql.Data.MySqlClient;
using SurPath.Entity;
using SurPath.Entity.Master;
using SurPath.MySQLHelper;
using System;
using System.Collections.Generic;
using System.Data;

namespace SurPath.Data.Master
{
    /// <summary>
    /// Court related data access process.
    /// </summary>

    public class CourtDao : DataObject
    {
        #region Constructor

        public CourtDao()
        {
            //
        }

        #endregion Constructor

        #region Public Methods

        public int Insert(Court court, User user)
        {
            int returnValue = 0;
            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();

                try
                {
                    string sqlQuery = "INSERT INTO courts (court_user_name, court_name, court_address_1, court_address_2, court_city,court_state,court_zip, is_active, is_synchronized, is_archived, created_on, created_by, last_modified_on, last_modified_by) VALUES (";
                    sqlQuery += "@CourtUsername, @CourtName, @CourtAddress1, @CourtAddress2, @CourtCity,@CourtState,@CourtZip, @IsActive, b'0', b'0', NOW(), @CreatedBy, NOW(), @LastModifiedBy)";

                    MySqlParameter[] param = new MySqlParameter[10];
                    param[0] = new MySqlParameter("@CourtUsername", court.CourtUsername);
                    param[1] = new MySqlParameter("@CourtName", court.CourtName);
                    param[2] = new MySqlParameter("@CourtAddress1", court.CourtAddress1);
                    param[3] = new MySqlParameter("@CourtAddress2", court.CourtAddress2);
                    param[4] = new MySqlParameter("@CourtCity", court.CourtCity);
                    param[5] = new MySqlParameter("@CourtState", court.CourtState);
                    param[6] = new MySqlParameter("@CourtZip", court.CourtZip);
                    param[7] = new MySqlParameter("@IsActive", court.IsActive);
                    param[8] = new MySqlParameter("@CreatedBy", court.CreatedBy);
                    param[9] = new MySqlParameter("@LastModifiedBy", court.LastModifiedBy);

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                    sqlQuery = "SELECT LAST_INSERT_ID()";

                    returnValue = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlQuery));

                    court.CourtId = returnValue;
                    user.CourtId = returnValue;

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

        public int Update(Court court)
        {
            int returnValue = 0;

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();

                try
                {
                    string sqlQuery = "UPDATE courts SET "
                                    + "court_name = @CourtName, "
                                    + "court_address_1 = @CourtAddress1, "
                                    + "court_address_2 = @CourtAddress2, "
                                    + "court_city = @CourtCity, "
                                    + "court_state = @CourtState,"
                                    + "court_zip= @CourtZip,"
                                    + "is_active= @IsActive,"
                                    + "is_synchronized = b'0', "
                                    + "last_modified_on = NOW(), "
                                    + "last_modified_by = @LastModifiedBy "
                                    + "WHERE court_id = @CourtId";

                    MySqlParameter[] param = new MySqlParameter[10];
                    param[0] = new MySqlParameter("@CourtId", court.CourtId);
                    param[1] = new MySqlParameter("@CourtUsername", court.CourtUsername);
                    param[2] = new MySqlParameter("@CourtName", court.CourtName);
                    param[3] = new MySqlParameter("@CourtAddress1", court.CourtAddress1);
                    param[4] = new MySqlParameter("@CourtAddress2", court.CourtAddress2);
                    param[5] = new MySqlParameter("@CourtCity", court.CourtCity);
                    param[6] = new MySqlParameter("@CourtState", court.CourtState);
                    param[7] = new MySqlParameter("@CourtZip", court.CourtZip);
                    param[8] = new MySqlParameter("@IsActive", court.IsActive);
                    param[9] = new MySqlParameter("@LastModifiedBy", court.LastModifiedBy);

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

        public int Delete(int courtId, string currentUserName)
        {
            int returnValue = 0;

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();
                try
                {
                    string sqlCount1Query = "Select count(*) from donor_test_info where court_id = " + courtId + " and test_status != 7";

                    int table1 = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlCount1Query));

                    if (table1 <= 0)
                    {
                        string sqlQuery = "UPDATE courts SET is_synchronized = b'0', is_archived = b'1', last_modified_on = NOW(), last_modified_by = @LastModifiedBy WHERE court_id = @CourtId";

                        MySqlParameter[] param = new MySqlParameter[2];
                        param[0] = new MySqlParameter("@CourtId", courtId);
                        param[1] = new MySqlParameter("@LastModifiedBy", currentUserName);

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

        public Court Get(int courtId)
        {
            Court court = null;
            string sqlQuery = "SELECT court_id AS CourtId, court_user_name AS CourtUsername, court_name AS CourtName, court_address_1 AS CourtAddress1, court_address_2 AS CourtAddress2, court_city AS CourtCity,court_state AS CourtState,court_zip AS CourtZip, is_active AS IsActive, is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM courts WHERE court_id = @CourtId";

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                MySqlParameter[] param = new MySqlParameter[1];
                param[0] = new MySqlParameter("@CourtId", courtId);

                MySqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                if (dr.Read())
                {
                    court = new Court();
                    court.CourtId = (int)dr["CourtId"];
                    court.CourtUsername = (string)dr["CourtUsername"];
                    court.CourtName = (string)dr["CourtName"];
                    court.CourtAddress1 = (string)dr["CourtAddress1"];
                    court.CourtAddress2 = dr["CourtAddress2"].ToString();
                    court.CourtCity = (string)dr["CourtCity"];
                    court.CourtState = (string)dr["CourtState"];
                    court.CourtZip = (string)dr["CourtZip"];
                    court.IsActive = dr["IsActive"].ToString() == "1" ? true : false;
                    court.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    court.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    court.CreatedOn = (DateTime)dr["CreatedOn"];
                    court.CreatedBy = (string)dr["CreatedBy"];
                    court.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    court.LastModifiedBy = (string)dr["LastModifiedBy"];
                }
            }
            return court;
        }

        public DataTable GetByCourtName(string courtName)
        {
            string sqlQuery = "SELECT court_id AS CourtId, court_user_name AS CourtUsername, court_name AS CourtName, court_address_1 AS CourtAddress1, court_address_2 AS CourtAddress2, court_city AS CourtCity,court_state AS CourtState,court_zip AS CourtZip, is_active AS IsActive,is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM courts WHERE is_archived = 0 AND Upper(court_name) = UPPER(@CourtName)";

            MySqlParameter[] param = new MySqlParameter[1];
            param[0] = new MySqlParameter("@CourtName", courtName);

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

        public DataTable GetByEmail(string email)
        {
            string sqlQuery = "SELECT court_id AS CourtId, court_user_name AS CourtUsername, court_name AS CourtName, court_address_1 AS CourtAddress1, court_address_2 AS CourtAddress2, court_city AS CourtCity,court_state AS CourtState,court_zip AS CourtZip, is_active AS IsActive,is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM courts WHERE is_archived = 0 AND Upper(court_user_name) = UPPER(@CourtUsername)";

            MySqlParameter[] param = new MySqlParameter[1];
            param[0] = new MySqlParameter("@CourtUsername", email);

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
            string sqlQuery = "SELECT court_id AS CourtId, court_user_name AS CourtUsername, court_name AS CourtName, court_address_1 AS CourtAddress1, court_address_2 AS CourtAddress2, court_city AS CourtCity,court_state AS CourtState,court_zip AS CourtZip ,  is_active AS IsActive,is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM courts WHERE is_archived = 0 ORDER BY court_name";

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
            string sqlQuery = "SELECT court_id AS CourtId, court_user_name AS CourtUsername, court_name AS CourtName, court_address_1 AS CourtAddress1, court_address_2 AS CourtAddress2, court_city AS CourtCity,court_state AS CourtState,court_zip AS CourtZip , is_active AS IsActive, is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM courts WHERE is_archived = 0 ";

            List<MySqlParameter> param = new List<MySqlParameter>();
            bool isInActiveFlag = false;

            foreach (KeyValuePair<string, string> searchItem in searchParam)
            {
                if (searchItem.Key == "GeneralSearch")
                {
                    sqlQuery += " AND (court_user_name LIKE @SearchKeyword OR court_name LIKE @SearchKeyword OR court_address_1 LIKE @SearchKeyword  OR court_address_2 LIKE @SearchKeyword  OR court_city LIKE @SearchKeyword OR court_state  LIKE @SearchKeyword  OR court_zip LIKE @SearchKeyword)";

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

            sqlQuery += "ORDER BY court_name";

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
            string sqlQuery = "SELECT court_id AS CourtId, court_user_name AS CourtUsername, court_name AS CourtName, court_address_1 AS CourtAddress1, court_address_2 AS CourtAddress2, court_city AS CourtCity,court_state AS CourtState,court_zip AS CourtZip , is_active AS IsActive, is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM courts WHERE is_archived = 0 ";
            if (active == false)
            {
                sqlQuery += "AND is_active = b'1'";
            }

            if (searchparam == "courtName")
            {
                if (getInActive == "1")
                {
                    sql = "ORDER BY CourtName";
                }
                else
                {
                    sql = "ORDER BY CourtName desc";
                }
            }
            if (searchparam == "courtUserName")
            {
                if (getInActive == "1")
                {
                    sql = "ORDER BY CourtUsername";
                }
                else
                {
                    sql = "ORDER BY CourtUsername desc";
                }
            }
            if (searchparam == "city")
            {
                if (getInActive == "1")
                {
                    sql = "ORDER BY CourtCity";
                }
                else
                {
                    sql = "ORDER BY CourtCity desc";
                }
            }
            if (searchparam == "state")
            {
                if (getInActive == "1")
                {
                    sql = "ORDER BY CourtState";
                }
                else
                {
                    sql = "ORDER BY CourtState desc";
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