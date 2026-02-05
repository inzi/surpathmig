using MySql.Data.MySqlClient;
using SurPath.Entity;
using SurPath.Entity.Master;
using SurPath.MySQLHelper;
using System;
using System.Collections.Generic;
using System.Data;

namespace SurPath.Data.Master
{
    public class AttorneyDao : DataObject
    {
        #region Constructor

        public AttorneyDao()
        {
            //
        }

        #endregion Constructor

        #region Public Methods

        public int Insert(Attorney attorney, User user)
        {
            int returnValue = 0;
            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();

                try
                {
                    string sqlQuery = "INSERT INTO attorneys (attorney_first_name, attorney_last_name, attorney_address_1, attorney_address_2,attorney_city,attorney_state,attorney_zip,attorney_phone,attorney_fax,attorney_email, is_active, is_synchronized, is_archived, created_on, created_by, last_modified_on, last_modified_by) VALUES (";
                    sqlQuery += "@AttorneyFirstName, @AttorneyLastName, @AttorneyAddress1, @AttorneyAddress2,@AttorneyCity,@AttorneyState,@AttorneyZip,@AttorneyPhone,@AttorneyFax,@AttorneyEmail,@IsActive, b'0', b'0', NOW(), @CreatedBy, NOW(), @LastModifiedBy)";

                    MySqlParameter[] param = new MySqlParameter[13];

                    param[0] = new MySqlParameter("@AttorneyFirstName", attorney.AttorneyFirstName);
                    param[1] = new MySqlParameter("@AttorneyLastName", attorney.AttorneyLastName);
                    param[2] = new MySqlParameter("@AttorneyAddress1", attorney.AttorneyAddress1);
                    param[3] = new MySqlParameter("@AttorneyAddress2", attorney.AttorneyAddress2);
                    param[4] = new MySqlParameter("@AttorneyCity", attorney.AttorneyCity);
                    param[5] = new MySqlParameter("@AttorneyState", attorney.AttorneyState);
                    param[6] = new MySqlParameter("@AttorneyZip", attorney.AttorneyZip);
                    param[7] = new MySqlParameter("@AttorneyPhone", attorney.AttorneyPhone);
                    param[8] = new MySqlParameter("@AttorneyFax", attorney.AttorneyFax);
                    param[9] = new MySqlParameter("@AttorneyEmail", attorney.AttorneyEmail);
                    param[10] = new MySqlParameter("@IsActive", attorney.IsActive);
                    param[11] = new MySqlParameter("@CreatedBy", attorney.CreatedBy);
                    param[12] = new MySqlParameter("@LastModifiedBy", attorney.LastModifiedBy);

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                    sqlQuery = "SELECT LAST_INSERT_ID()";

                    returnValue = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlQuery));

                    attorney.AttorneyId = returnValue;
                    user.AttorneyId = returnValue;

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

        public int Update(Attorney attorney)
        {
            int returnValue = 0;

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();

                try
                {
                    string sqlQuery = "UPDATE attorneys SET "
                                    + "attorney_first_name = @AttorneyFirstName, "
                                    + "attorney_last_name = @AttorneyLastName, "
                                    + "attorney_address_1 = @AttorneyAddress1, "
                                    + "attorney_address_2 = @AttorneyAddress2, "
                                    + "attorney_city = @AttorneyCity,"
                                    + "attorney_state= @AttorneyState, "
                                    + "attorney_zip=@AttorneyZip, "
                                    + "attorney_phone=@AttorneyPhone, "
                                    + "attorney_fax=@AttorneyFax,"
                                    + "attorney_email=@AttorneyEmail, "
                                    + "is_active=@IsActive, "
                                    + "is_synchronized = b'0', "
                                    + "last_modified_on = NOW(), "
                                    + "last_modified_by = @LastModifiedBy "
                                    + "WHERE attorney_id = @AttorneyId";

                    MySqlParameter[] param = new MySqlParameter[13];
                    param[0] = new MySqlParameter("@AttorneyId", attorney.AttorneyId);
                    param[1] = new MySqlParameter("@AttorneyFirstName", attorney.AttorneyFirstName);
                    param[2] = new MySqlParameter("@AttorneyLastName", attorney.AttorneyLastName);
                    param[3] = new MySqlParameter("@AttorneyAddress1", attorney.AttorneyAddress1);
                    param[4] = new MySqlParameter("@AttorneyAddress2", attorney.AttorneyAddress2);
                    param[5] = new MySqlParameter("@AttorneyCity", attorney.AttorneyCity);
                    param[6] = new MySqlParameter("@AttorneyState", attorney.AttorneyState);
                    param[7] = new MySqlParameter("@AttorneyZip", attorney.AttorneyZip);
                    param[8] = new MySqlParameter("@AttorneyPhone", attorney.AttorneyPhone);
                    param[9] = new MySqlParameter("@AttorneyFax", attorney.AttorneyFax);
                    param[10] = new MySqlParameter("@AttorneyEmail", attorney.AttorneyEmail);
                    param[11] = new MySqlParameter("@IsActive", attorney.IsActive);
                    param[12] = new MySqlParameter("@LastModifiedBy", attorney.LastModifiedBy);

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

        public int Delete(int attorneyId, string currentUserName)
        {
            int returnValue = 0;

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();
                try
                {
                    string sqlCount1Query = "Select count(*) from donor_test_info_attorneys where attorney_id = " + attorneyId + "";

                    int table1 = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlCount1Query));

                    if (table1 <= 0)
                    {
                        string sqlQuery = "UPDATE attorneys SET is_synchronized = b'0', is_archived = b'1', last_modified_on = NOW(), last_modified_by = @LastModifiedBy WHERE attorney_id = @AttorneyId";

                        MySqlParameter[] param = new MySqlParameter[2];
                        param[0] = new MySqlParameter("@AttorneyId", attorneyId);
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

        public Attorney Get(int attorneyId)
        {
            Attorney attorney = null;
            string sqlQuery = "SELECT attorney_id AS AttorneyId, attorney_first_name AS AttorneyFirstName, attorney_last_name AS AttorneyLastName, attorney_address_1 AS AttorneyAddress1, attorney_address_2 AS AttorneyAddress2,attorney_city AS AttorneyCity,attorney_state AS AttorneyState,attorney_zip AS AttorneyZip,attorney_phone AS AttorneyPhone,attorney_fax AS AttorneyFax,attorney_email AS AttorneyEmail, is_active AS IsActive, is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM attorneys WHERE attorney_id = @AttorneyId";

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                MySqlParameter[] param = new MySqlParameter[1];
                param[0] = new MySqlParameter("@AttorneyId", attorneyId);

                MySqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                if (dr.Read())
                {
                    attorney = new Attorney();
                    attorney.AttorneyId = (int)dr["AttorneyId"];
                    attorney.AttorneyFirstName = (string)dr["AttorneyFirstName"];
                    attorney.AttorneyLastName = (string)dr["AttorneyLastName"];
                    attorney.AttorneyAddress1 = (string)dr["AttorneyAddress1"];
                    attorney.AttorneyAddress2 = dr["AttorneyAddress2"].ToString();
                    attorney.AttorneyCity = (string)dr["AttorneyCity"];
                    attorney.AttorneyState = (string)dr["AttorneyState"];
                    attorney.AttorneyZip = (string)dr["AttorneyZip"];
                    attorney.AttorneyPhone = dr["AttorneyPhone"].ToString();
                    attorney.AttorneyFax = dr["AttorneyFax"].ToString();
                    attorney.AttorneyEmail = dr["AttorneyEmail"].ToString();
                    attorney.IsActive = dr["IsActive"].ToString() == "1" ? true : false;
                    attorney.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    attorney.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    attorney.CreatedOn = (DateTime)dr["CreatedOn"];
                    attorney.CreatedBy = (string)dr["CreatedBy"];
                    attorney.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    attorney.LastModifiedBy = (string)dr["LastModifiedBy"];
                }
            }
            return attorney;
        }

        public DataTable GetList(string getInActive = null)
        {
            string sqlQuery = "SELECT attorney_id AS AttorneyId, attorney_first_name AS AttorneyFirstName, attorney_last_name AS AttorneyLastName, attorney_address_1 AS AttorneyAddress1, attorney_address_2 AS AttorneyAddress2,attorney_city AS AttorneyCity,attorney_state AS AttorneyState,attorney_zip AS AttorneyZip,attorney_phone AS AttorneyPhone,attorney_fax AS AttorneyFax,attorney_email AS AttorneyEmail, is_active AS IsActive, is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM attorneys WHERE is_archived = 0 ORDER BY attorney_first_name";

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

        public DataTable GetByEmail(string Email)
        {
            string sqlQuery = "SELECT attorney_id AS AttorneyId, attorney_first_name AS AttorneyFirstName, attorney_last_name AS AttorneyLastName, attorney_address_1 AS AttorneyAddress1, attorney_address_2 AS AttorneyAddress2,attorney_city AS AttorneyCity,attorney_state AS AttorneyState,attorney_zip AS AttorneyZip,attorney_phone AS AttorneyPhone,attorney_fax AS AttorneyFax,attorney_email AS AttorneyEmail, is_active AS IsActive, is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM attorneys WHERE is_archived = 0 AND LOWER(attorney_email) = LOWER(@AttorneyEmail)";

            MySqlParameter[] param = new MySqlParameter[1];
            param[0] = new MySqlParameter("@AttorneyEmail", Email);

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

        public DataTable GetList(Dictionary<string, string> searchParam)
        {
            string sqlQuery = "SELECT attorney_id AS AttorneyId, attorney_first_name AS AttorneyFirstName, attorney_last_name AS AttorneyLastName, attorney_address_1 AS AttorneyAddress1, attorney_address_2 AS AttorneyAddress2,attorney_city AS AttorneyCity,attorney_state AS AttorneyState,attorney_zip AS AttorneyZip,attorney_phone AS AttorneyPhone,attorney_fax AS AttorneyFax,attorney_email AS AttorneyEmail, is_active AS IsActive, is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy "
                              + "FROM attorneys WHERE is_archived = 0 ";

            List<MySqlParameter> param = new List<MySqlParameter>();
            bool isInActiveFlag = false;

            foreach (KeyValuePair<string, string> searchItem in searchParam)
            {
                if (searchItem.Key == "GeneralSearch")
                {
                    sqlQuery += " AND (attorney_first_name LIKE @SearchKeyword OR attorney_last_name LIKE @SearchKeyword  OR attorney_address_1 LIKE @SearchKeyword OR attorney_address_2 LIKE @SearchKeyword OR attorney_city LIKE @SearchKeyword OR attorney_state LIKE @SearchKeyword OR attorney_zip LIKE @SearchKeyword OR attorney_phone LIKE @SearchKeyword OR attorney_fax LIKE @SearchKeyword OR attorney_email LIKE @SearchKeyword) ";

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

            sqlQuery += " ORDER BY attorney_first_name";

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
            string sqlQuery = "SELECT attorney_id AS AttorneyId, attorney_first_name AS AttorneyFirstName, attorney_last_name AS AttorneyLastName, attorney_address_1 AS AttorneyAddress1, attorney_address_2 AS AttorneyAddress2,attorney_city AS AttorneyCity,attorney_state AS AttorneyState,attorney_zip AS AttorneyZip,attorney_phone AS AttorneyPhone,attorney_fax AS AttorneyFax,attorney_email AS AttorneyEmail, is_active AS IsActive, is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy "
                               + "FROM attorneys WHERE is_archived = 0 ";
            if (active == false)
            {
                sqlQuery += "AND is_active = b'1'";
            }

            if (searchparam == "firstName")
            {
                if (getInActive == "1")
                {
                    sql = "ORDER BY AttorneyFirstName";
                }
                else
                {
                    sql = "ORDER BY AttorneyFirstName desc";
                }
            }
            if (searchparam == "lastName")
            {
                if (getInActive == "1")
                {
                    sql = "ORDER BY AttorneyLastName";
                }
                else
                {
                    sql = "ORDER BY AttorneyLastName desc";
                }
            }
            if (searchparam == "city")
            {
                if (getInActive == "1")
                {
                    sql = "ORDER BY AttorneyCity";
                }
                else
                {
                    sql = "ORDER BY AttorneyCity desc";
                }
            }
            if (searchparam == "state")
            {
                if (getInActive == "1")
                {
                    sql = "ORDER BY AttorneyState";
                }
                else
                {
                    sql = "ORDER BY AttorneyState desc";
                }
            }
            if (searchparam == "email")
            {
                if (getInActive == "1")
                {
                    sql = "ORDER BY AttorneyEmail";
                }
                else
                {
                    sql = "ORDER BY AttorneyEmail desc";
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