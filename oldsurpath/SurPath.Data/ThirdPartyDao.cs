using MySql.Data.MySqlClient;
using SurPath.Entity;
using SurPath.MySQLHelper;
using System;
using System.Collections.Generic;
using System.Data;

namespace SurPath.Data
{
    public class ThirdPartyDao : DataObject
    {
        #region Constructor

        public ThirdPartyDao()
        {
            //
        }

        #endregion Constructor

        #region Public Methods

        public int Insert(ThirdParty thirdParty)
        {
            int returnValue = 0;
            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();

                try
                {
                    string sqlQuery = "INSERT INTO donor_third_parties (donor_id, third_party_first_name, third_party_last_name, third_party_address_1, third_party_address_2,third_party_city,third_party_state,third_party_zip,third_party_phone,third_party_fax,third_party_email, is_active, is_synchronized, is_archived, created_on, created_by, last_modified_on, last_modified_by) VALUES (";
                    sqlQuery += "@DonorId, @ThirdPartyFirstName, @ThirdPartyLastName, @ThirdPartyAddress1, @ThirdPartyAddress2,@ThirdPartyCity,@ThirdPartyState,@ThirdPartyZip,@ThirdPartyPhone,@ThirdPartyFax,@ThirdPartyEmail,  @IsActive, b'0', b'0', NOW(), @CreatedBy, NOW(), @LastModifiedBy)";

                    MySqlParameter[] param = new MySqlParameter[14];
                    param[0] = new MySqlParameter("@DonorId", thirdParty.DonorId);
                    param[1] = new MySqlParameter("@ThirdPartyFirstName", thirdParty.ThirdPartyFirstName);
                    param[2] = new MySqlParameter("@ThirdPartyLastName", thirdParty.ThirdPartyLastName);
                    param[3] = new MySqlParameter("@ThirdPartyAddress1", thirdParty.ThirdPartyAddress1);
                    param[4] = new MySqlParameter("@ThirdPartyAddress2", thirdParty.ThirdPartyAddress2);
                    param[5] = new MySqlParameter("@ThirdPartyCity", thirdParty.ThirdPartyCity);
                    param[6] = new MySqlParameter("@ThirdPartyState", thirdParty.ThirdPartyState);
                    param[7] = new MySqlParameter("@ThirdPartyZip", thirdParty.ThirdPartyZip);
                    param[8] = new MySqlParameter("@ThirdPartyPhone", thirdParty.ThirdPartyPhone);
                    param[9] = new MySqlParameter("@ThirdPartyFax", thirdParty.ThirdPartyFax);
                    param[10] = new MySqlParameter("@ThirdPartyEmail", thirdParty.ThirdPartyEmail);
                    param[11] = new MySqlParameter("@IsActive", thirdParty.IsActive);
                    param[12] = new MySqlParameter("@CreatedBy", thirdParty.CreatedBy);
                    param[13] = new MySqlParameter("@LastModifiedBy", thirdParty.LastModifiedBy);

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                    sqlQuery = "SELECT LAST_INSERT_ID()";

                    returnValue = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlQuery));

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

        public int Update(ThirdParty thirdParty)
        {
            int returnValue = 0;

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();

                try
                {
                    string sqlQuery = "UPDATE donor_third_parties SET "
                                    + "third_party_first_name = @ThirdPartyFirstName, "
                                    + "third_party_last_name = @ThirdPartyLastName, "
                                    + "third_party_address_1 = @ThirdPartyAddress1, "
                                    + "third_party_address_2 = @ThirdPartyAddress2, "
                                    + "third_party_city = @ThirdPartyCity,"
                                    + "third_party_state= @ThirdPartyState, "
                                    + "third_party_zip=@ThirdPartyZip, "
                                    + "third_party_phone=@ThirdPartyPhone, "
                                    + "third_party_fax=@ThirdPartyFax,"
                                    + "third_party_email=@ThirdPartyEmail, "
                                    + "is_active = @IsActive ,"
                                    + "is_synchronized = b'0', "
                                    + "last_modified_on = NOW(), "
                                    + "last_modified_by = @LastModifiedBy "
                                    + "WHERE third_party_id = @ThirdPartyId";

                    MySqlParameter[] param = new MySqlParameter[13];
                    param[0] = new MySqlParameter("@ThirdPartyId", thirdParty.ThirdPartyId);
                    param[1] = new MySqlParameter("@ThirdPartyFirstName", thirdParty.ThirdPartyFirstName);
                    param[2] = new MySqlParameter("@ThirdPartyLastName", thirdParty.ThirdPartyLastName);
                    param[3] = new MySqlParameter("@ThirdPartyAddress1", thirdParty.ThirdPartyAddress1);
                    param[4] = new MySqlParameter("@ThirdPartyAddress2", thirdParty.ThirdPartyAddress2);
                    param[5] = new MySqlParameter("@ThirdPartyCity", thirdParty.ThirdPartyCity);
                    param[6] = new MySqlParameter("@ThirdPartyState", thirdParty.ThirdPartyState);
                    param[7] = new MySqlParameter("@ThirdPartyZip", thirdParty.ThirdPartyZip);
                    param[8] = new MySqlParameter("@ThirdPartyPhone", thirdParty.ThirdPartyPhone);
                    param[9] = new MySqlParameter("@ThirdPartyFax", thirdParty.ThirdPartyFax);
                    param[10] = new MySqlParameter("@ThirdPartyEmail", thirdParty.ThirdPartyEmail);
                    param[11] = new MySqlParameter("@IsActive", thirdParty.IsActive);
                    param[12] = new MySqlParameter("@LastModifiedBy", thirdParty.LastModifiedBy);

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

        public int Delete(int thirdPartyId, string currentUserName)
        {
            int returnValue = 0;

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();
                //string sqlCount1Query = "Select count(*) from donor_test_info_third_parties where third_party_id = " + thirdPartyId + "";

                //int table1 = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlCount1Query));

                //if (table1 <= 0)
                //{
                try
                {
                    string sqlQuery = "UPDATE donor_third_parties SET is_synchronized = b'0', is_archived = b'1', last_modified_on = NOW(), last_modified_by = @LastModifiedBy WHERE third_party_id = @ThirdPartyId";

                    MySqlParameter[] param = new MySqlParameter[2];
                    param[0] = new MySqlParameter("@ThirdPartyId", thirdPartyId);
                    param[1] = new MySqlParameter("@LastModifiedBy", currentUserName);

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
            //  }

            return returnValue;
        }

        public ThirdParty Get(int thirdPartyId)
        {
            ThirdParty thirdParty = null;
            string sqlQuery = "SELECT third_party_id AS ThirdPartyId, donor_id AS DonorId, third_party_first_name AS ThirdPartyFirstName, third_party_last_name AS ThirdPartyLastName, third_party_address_1 AS ThirdPartyAddress1, third_party_address_2 AS ThirdPartyAddress2,third_party_city AS ThirdPartyCity,third_party_state AS ThirdPartyState,third_party_zip AS ThirdPartyZip,third_party_phone AS ThirdPartyPhone,third_party_fax AS ThirdPartyFax,third_party_email AS ThirdPartyEmail,is_active AS IsActive,is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM donor_third_parties WHERE third_party_id = @ThirdPartyId";

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                MySqlParameter[] param = new MySqlParameter[1];
                param[0] = new MySqlParameter("@ThirdPartyId", thirdPartyId);

                MySqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                if (dr.Read())
                {
                    thirdParty = new ThirdParty();
                    thirdParty.ThirdPartyId = (int)dr["ThirdPartyId"];
                    thirdParty.DonorId = (int)dr["DonorId"];
                    thirdParty.ThirdPartyFirstName = (string)dr["ThirdPartyFirstName"];
                    thirdParty.ThirdPartyLastName = (string)dr["ThirdPartyLastName"];
                    thirdParty.ThirdPartyAddress1 = (string)dr["ThirdPartyAddress1"];
                    thirdParty.ThirdPartyAddress2 = dr["ThirdPartyAddress2"].ToString();
                    thirdParty.ThirdPartyCity = (string)dr["ThirdPartyCity"];
                    thirdParty.ThirdPartyState = (string)dr["ThirdPartyState"];
                    thirdParty.ThirdPartyZip = (string)dr["ThirdPartyZip"];
                    thirdParty.ThirdPartyPhone = dr["ThirdPartyPhone"].ToString();
                    thirdParty.ThirdPartyFax = dr["ThirdPartyFax"].ToString();
                    thirdParty.ThirdPartyEmail = (string)dr["ThirdPartyEmail"];
                    thirdParty.IsActive = dr["IsActive"].ToString() == "1" ? true : false;
                    thirdParty.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    thirdParty.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    thirdParty.CreatedOn = (DateTime)dr["CreatedOn"];
                    thirdParty.CreatedBy = (string)dr["CreatedBy"];
                    thirdParty.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    thirdParty.LastModifiedBy = (string)dr["LastModifiedBy"];
                }
            }
            return thirdParty;
        }

        public DataTable GetList()
        {
            //   string sqlQuery = "SELECT third_party_id AS ThirdPartyId, third_party_first_name AS ThirdPartyFirstName, third_party_last_name AS ThirdPartyLastName, third_party_address_1 AS ThirdPartyAddress1, third_party_address_2 AS ThirdPartyAddress2,third_party_city AS ThirdPartyCity,third_party_state AS ThirdPartyState,third_party_zip AS ThirdPartyZip,third_party_phone AS ThirdPartyPhone,third_party_fax AS ThirdPartyFax,third_party_email AS ThirdPartyEmail,is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM donor_third_parties WHERE is_archived = 0 ORDER BY third_party_first_name";
            string sqlQuery = "SELECT donor_third_parties.third_party_id AS ThirdPartyId, donor_third_parties.donor_id AS DonorId, donor_third_parties.third_party_first_name AS ThirdPartyFirstName, donor_third_parties.third_party_last_name AS ThirdPartyLastName, donor_third_parties.third_party_address_1 AS ThirdPartyAddress1, donor_third_parties.third_party_address_2 AS ThirdPartyAddress2, donor_third_parties.third_party_city AS ThirdPartyCity, donor_third_parties.third_party_state AS ThirdPartyState, donor_third_parties.third_party_zip AS ThirdPartyZip, donor_third_parties.third_party_phone AS ThirdPartyPhone, donor_third_parties.third_party_fax AS ThirdPartyFax, donor_third_parties.third_party_email AS ThirdPartyEmail, donor_third_parties.is_synchronized AS IsSynchronized, donor_third_parties.is_archived AS IsArchived, donor_third_parties.created_on AS CreatedOn, donor_third_parties.created_by AS CreatedBy, donor_third_parties.last_modified_on AS LastModifiedOn, donor_third_parties.last_modified_by AS LastModifiedBy "
                           + "FROM donor_third_parties "
                           + "WHERE donor_third_parties.is_archived = 0 ORDER BY donor_third_parties.third_party_first_name";

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

        public DataTable GetList(int donorId)
        {
            //   string sqlQuery = "SELECT third_party_id AS ThirdPartyId, third_party_first_name AS ThirdPartyFirstName, third_party_last_name AS ThirdPartyLastName, third_party_address_1 AS ThirdPartyAddress1, third_party_address_2 AS ThirdPartyAddress2,third_party_city AS ThirdPartyCity,third_party_state AS ThirdPartyState,third_party_zip AS ThirdPartyZip,third_party_phone AS ThirdPartyPhone,third_party_fax AS ThirdPartyFax,third_party_email AS ThirdPartyEmail,is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM donor_third_parties WHERE is_archived = 0 ORDER BY third_party_first_name";
            string sqlQuery = "SELECT donor_third_parties.third_party_id AS ThirdPartyId, donor_third_parties.donor_id AS DonorId, donor_third_parties.third_party_first_name AS ThirdPartyFirstName, donor_third_parties.third_party_last_name AS ThirdPartyLastName, donor_third_parties.third_party_address_1 AS ThirdPartyAddress1, donor_third_parties.third_party_address_2 AS ThirdPartyAddress2, donor_third_parties.third_party_city AS ThirdPartyCity, donor_third_parties.third_party_state AS ThirdPartyState, donor_third_parties.third_party_zip AS ThirdPartyZip, donor_third_parties.third_party_phone AS ThirdPartyPhone, donor_third_parties.third_party_fax AS ThirdPartyFax, donor_third_parties.third_party_email AS ThirdPartyEmail, donor_third_parties.is_active AS IsActive, donor_third_parties.is_synchronized AS IsSynchronized, donor_third_parties.is_archived AS IsArchived, donor_third_parties.created_on AS CreatedOn, donor_third_parties.created_by AS CreatedBy, donor_third_parties.last_modified_on AS LastModifiedOn, donor_third_parties.last_modified_by AS LastModifiedBy "
                             + "FROM donor_third_parties "
                             + "WHERE donor_third_parties.is_archived = 0 AND donor_third_parties.donor_id = @DonorId ORDER BY donor_third_parties.third_party_first_name";

            MySqlParameter[] param = new MySqlParameter[1];
            param[0] = new MySqlParameter("@DonorId", donorId);

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

        public DataTable GetList(int donorId, Dictionary<string, string> searchParam)
        {
            // string sqlQuery = "SELECT third_party_id AS ThirdPartyId, third_party_first_name AS ThirdPartyFirstName,  donor_third_parties.third_party_last_name AS ThirdPartyLastName,  donor_third_parties.third_party_address_1 AS ThirdPartyAddress1,  donor_third_parties.third_party_address_2 AS ThirdPartyAddress2,  donor_third_parties.third_party_city AS ThirdPartyCity,  donor_third_parties.third_party_state AS ThirdPartyState,  donor_third_parties.third_party_zip AS ThirdPartyZip,  donor_third_parties.third_party_phone AS ThirdPartyPhone,  donor_third_parties.third_party_fax AS ThirdPartyFax,  donor_third_parties.third_party_email AS ThirdPartyEmail,is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM donor_third_parties WHERE is_archived = 0 AND (donor_third_parties.third_party_first_name LIKE @SearchKeyword OR donor_third_parties.third_party_last_name LIKE @SearchKeyword  OR donor_third_parties.third_party_address_1 LIKE @SearchKeyword OR donor_third_parties.third_party_address_2 LIKE @SearchKeyword OR donor_third_parties.third_party_city LIKE @SearchKeyword OR donor_third_parties.third_party_state LIKE @SearchKeyword OR donor_third_parties.third_party_zip LIKE @SearchKeyword OR donor_third_parties.third_party_phone LIKE @SearchKeyword OR donor_third_parties.third_party_fax LIKE @SearchKeyword OR donor_third_parties.third_party_email LIKE @SearchKeyword) ORDER BY donor_third_parties.third_party_first_name";

            string sqlQuery = "SELECT donor_third_parties.third_party_id AS ThirdPartyId, donor_third_parties.donor_id AS DonorId, donor_third_parties.third_party_first_name AS ThirdPartyFirstName, donor_third_parties.third_party_last_name AS ThirdPartyLastName, donor_third_parties.third_party_address_1 AS ThirdPartyAddress1, donor_third_parties.third_party_address_2 AS ThirdPartyAddress2, donor_third_parties.third_party_city AS ThirdPartyCity, donor_third_parties.third_party_state AS ThirdPartyState, donor_third_parties.third_party_zip AS ThirdPartyZip, donor_third_parties.third_party_phone AS ThirdPartyPhone, donor_third_parties.third_party_fax AS ThirdPartyFax, donor_third_parties.third_party_email AS ThirdPartyEmail, donor_third_parties.is_active AS IsActive, donor_third_parties.is_synchronized AS IsSynchronized, donor_third_parties.is_archived AS IsArchived, donor_third_parties.created_on AS CreatedOn, donor_third_parties.created_by AS CreatedBy, donor_third_parties.last_modified_on AS LastModifiedOn, donor_third_parties.last_modified_by AS LastModifiedBy "
                             + "FROM donor_third_parties "
                             + "WHERE donor_third_parties.is_archived = 0 AND donor_third_parties.donor_id ='" + donorId + "'";

            //MySqlParameter[] param = new MySqlParameter[2];
            //param[0] = new MySqlParameter("@DonorId", donorId);
            //param[1] = new MySqlParameter("@SearchKeyword", searchParam["GeneralSearch"].ToString());

            List<MySqlParameter> paramList = new List<MySqlParameter>();
            bool isInActiveFlag = false;

            foreach (KeyValuePair<string, string> searchItem in searchParam)
            {
                if (searchItem.Key == "GeneralSearch")
                {
                    sqlQuery += "AND (donor_third_parties.third_party_first_name LIKE @SearchKeyword OR donor_third_parties.third_party_last_name LIKE @SearchKeyword  OR donor_third_parties.third_party_address_1 LIKE @SearchKeyword OR donor_third_parties.third_party_address_2 LIKE @SearchKeyword OR donor_third_parties.third_party_city LIKE @SearchKeyword OR donor_third_parties.third_party_state LIKE @SearchKeyword OR donor_third_parties.third_party_zip LIKE @SearchKeyword OR donor_third_parties.third_party_phone LIKE @SearchKeyword OR donor_third_parties.third_party_fax LIKE @SearchKeyword OR donor_third_parties.third_party_email LIKE @SearchKeyword)";
                    paramList.Add(new MySqlParameter("@SearchKeyword", searchItem.Value));
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
                sqlQuery += "AND  donor_third_parties.is_active = b'1' ";
            }

            sqlQuery += "ORDER BY donor_third_parties.third_party_first_name";

            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sqlQuery, paramList.ToArray());
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        public DataTable sorting(int donorId, string searchparam, bool active, string getInActive = null)
        {
            string sql = string.Empty;
            string sqlQuery = "SELECT donor_third_parties.third_party_id AS ThirdPartyId, donor_third_parties.donor_id AS DonorId, donor_third_parties.third_party_first_name AS ThirdPartyFirstName, donor_third_parties.third_party_last_name AS ThirdPartyLastName, donor_third_parties.third_party_address_1 AS ThirdPartyAddress1, donor_third_parties.third_party_address_2 AS ThirdPartyAddress2, donor_third_parties.third_party_city AS ThirdPartyCity, donor_third_parties.third_party_state AS ThirdPartyState, donor_third_parties.third_party_zip AS ThirdPartyZip, donor_third_parties.third_party_phone AS ThirdPartyPhone, donor_third_parties.third_party_fax AS ThirdPartyFax, donor_third_parties.third_party_email AS ThirdPartyEmail, donor_third_parties.is_active AS IsActive, donor_third_parties.is_synchronized AS IsSynchronized, donor_third_parties.is_archived AS IsArchived, donor_third_parties.created_on AS CreatedOn, donor_third_parties.created_by AS CreatedBy, donor_third_parties.last_modified_on AS LastModifiedOn, donor_third_parties.last_modified_by AS LastModifiedBy "
                            + "FROM donor_third_parties "
                            + "WHERE donor_third_parties.is_archived = 0 AND donor_third_parties.donor_id ='" + donorId + "'";

            if (active == false)
            {
                sqlQuery += "AND donor_third_parties.is_active = b'1'";
            }

            if (searchparam == "firstName")
            {
                if (getInActive == "1")
                {
                    sql = "ORDER BY donor_third_parties.third_party_first_name";
                }
                else
                {
                    sql = "ORDER BY donor_third_parties.third_party_first_name desc";
                }
            }
            if (searchparam == "lastName")
            {
                if (getInActive == "1")
                {
                    sql = "ORDER BY donor_third_parties.third_party_last_name";
                }
                else
                {
                    sql = "ORDER BY donor_third_parties.third_party_last_name desc";
                }
            }

            if (searchparam == "city")
            {
                if (getInActive == "1")
                {
                    sql = "ORDER BY donor_third_parties.third_party_city";
                }
                else
                {
                    sql = "ORDER BY donor_third_parties.third_party_city desc";
                }
            }
            if (searchparam == "state")
            {
                if (getInActive == "1")
                {
                    sql = "ORDER BY donor_third_parties.third_party_state";
                }
                else
                {
                    sql = "ORDER BY donor_third_parties.third_party_state desc";
                }
            }
            if (searchparam == "email")
            {
                if (getInActive == "1")
                {
                    sql = "ORDER BY donor_third_parties.third_party_email";
                }
                else
                {
                    sql = "ORDER BY donor_third_parties.third_party_email desc";
                }
            }
            if (searchparam == "isActive")
            {
                if (getInActive == "1")
                {
                    sql = "ORDER BY donor_third_parties.is_active";
                }
                else
                {
                    sql = "ORDER BY donor_third_parties.is_active desc";
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