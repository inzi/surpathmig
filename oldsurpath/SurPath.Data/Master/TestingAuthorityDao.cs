using MySql.Data.MySqlClient;
using SurPath.Entity.Master;
using SurPath.MySQLHelper;
using System;
using System.Collections.Generic;
using System.Data;

namespace SurPath.Data.Master
{
    public class TestingAuthorityDao : DataObject
    {
        #region Constructor

        public TestingAuthorityDao()
        {
            //
        }

        #endregion Constructor

        #region Public Methods

        public int Insert(TestingAuthority testingAuthority)
        {
            int returnValue = 0;

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();

                try
                {
                    string sqlQuery = "INSERT INTO testing_authority (testing_authority_name,is_active, is_synchronized, is_archived, created_on, created_by, last_modified_on, last_modified_by) VALUES (";
                    sqlQuery += "@TestingAuthorityName,@IsActive, b'0', b'0', NOW(), @CreatedBy, NOW(), @LastModifiedBy)";

                    MySqlParameter[] param = new MySqlParameter[4];
                    param[0] = new MySqlParameter("@TestingAuthorityName", testingAuthority.TestingAuthorityName);
                    param[1] = new MySqlParameter("@IsActive", testingAuthority.IsActive);
                    param[2] = new MySqlParameter("@CreatedBy", testingAuthority.CreatedBy);
                    param[3] = new MySqlParameter("@LastModifiedBy", testingAuthority.LastModifiedBy);

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

        public int Update(TestingAuthority testingAuthority)
        {
            int returnValue = 0;

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();

                try
                {
                    string sqlQuery = "UPDATE testing_authority SET "
                                    + "testing_authority_name = @TestingAuthorityName, "
                                     + "is_active= @IsActive, "
                                    + "is_synchronized = b'0', "
                                    + "last_modified_on = NOW(), "
                                    + "last_modified_by = @LastModifiedBy "
                                    + "WHERE testing_authority_id = @TestingAuthorityId";

                    MySqlParameter[] param = new MySqlParameter[4];
                    param[0] = new MySqlParameter("@TestingAuthorityId", testingAuthority.TestingAuthorityId);
                    param[1] = new MySqlParameter("@IsActive", testingAuthority.IsActive);
                    param[2] = new MySqlParameter("@TestingAuthorityName", testingAuthority.TestingAuthorityName);
                    param[3] = new MySqlParameter("@LastModifiedBy", testingAuthority.LastModifiedBy);

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

        public int Delete(int testingAuthorityId, string currentUserName)
        {
            int returnValue = 0;

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();
                try
                {
                    string sqlCount1Query = "Select count(*) from donor_test_info where testing_authority_id = " + testingAuthorityId + "";

                    int table1 = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlCount1Query));
                    if (table1 <= 0)
                    {
                        string sqlQuery = "UPDATE testing_authority SET is_archived = b'1',is_synchronized = b'1', last_modified_on = NOW(), last_modified_by = @LastModifiedBy WHERE testing_authority_id = @TestingAuthorityId";

                        MySqlParameter[] param = new MySqlParameter[2];
                        param[0] = new MySqlParameter("@TestingAuthorityId", testingAuthorityId);
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

        public TestingAuthority Get(int testingAuthorityId)
        {
            TestingAuthority testingAuthority = null;
            string sqlQuery = "SELECT testing_authority_id AS TestingAuthorityId, testing_authority_name AS TestingAuthorityName, is_active AS IsActive, is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM testing_authority WHERE testing_authority_id = @TestingAuthorityId";

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                MySqlParameter[] param = new MySqlParameter[1];
                param[0] = new MySqlParameter("@TestingAuthorityId", testingAuthorityId);

                MySqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                if (dr.Read())
                {
                    testingAuthority = new TestingAuthority();
                    testingAuthority.TestingAuthorityId = (int)dr["TestingAuthorityId"];
                    testingAuthority.TestingAuthorityName = (string)dr["TestingAuthorityName"];
                    testingAuthority.IsActive = dr["IsActive"].ToString() == "1" ? true : false;
                    testingAuthority.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    testingAuthority.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    testingAuthority.CreatedOn = (DateTime)dr["CreatedOn"];
                    testingAuthority.CreatedBy = (string)dr["CreatedBy"];
                    testingAuthority.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    testingAuthority.LastModifiedBy = (string)dr["LastModifiedBy"];
                }
            }

            return testingAuthority;
        }

        public DataTable GetByTestingAuthorityName(string testingAuthorityName)
        {
            string sqlQuery = "SELECT testing_authority_id AS TestingAuthorityId, testing_authority_name AS TestingAuthorityName, is_active AS IsActive, is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM testing_authority WHERE is_archived = 0 AND UPPER(testing_authority_name) = UPPER(@TestingAuthorityName)";

            MySqlParameter[] param = new MySqlParameter[1];
            param[0] = new MySqlParameter("@TestingAuthorityName", testingAuthorityName);

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

        public DataTable GetList(string getInActive = null)
        {
            string sql = string.Empty;

            string sqlQuery = "SELECT testing_authority_id AS TestingAuthorityId, testing_authority_name AS TestingAuthorityName, is_active AS IsActive, is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy ";

            if (getInActive != "1")
            {
                sql = "FROM testing_authority WHERE is_archived = 0  AND is_active = b'1' ORDER BY testing_authority_name";
            }
            else
            {
                sql = "FROM testing_authority WHERE is_archived = 0 ORDER BY testing_authority_name";
            }
            sqlQuery = sqlQuery + sql;

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
            string sqlQuery = "SELECT testing_authority_id AS TestingAuthorityId, testing_authority_name AS TestingAuthorityName, is_active AS IsActive, is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy "
                              + "FROM testing_authority WHERE is_archived = 0 ";

            List<MySqlParameter> param = new List<MySqlParameter>();
            bool isInActiveFlag = false;

            foreach (KeyValuePair<string, string> searchItem in searchParam)
            {
                if (searchItem.Key == "GeneralSearch")
                {
                    sqlQuery += "AND testing_authority_name LIKE @SearchKeyword ";

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

            sqlQuery += "ORDER BY testing_authority_name";

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
            string sqlQuery = "SELECT testing_authority_id AS TestingAuthorityId, testing_authority_name AS TestingAuthorityName, is_active AS IsActive, is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy "
                                + "FROM testing_authority WHERE is_archived = 0 ";

            if (active == false)
            {
                sqlQuery += "AND is_active = b'1'";
            }

            if (searchparam == "testingAuthorityName")
            {
                if (getInActive == "1")
                {
                    sql = "ORDER BY testing_authority_name";
                }
                else
                {
                    sql = "ORDER BY testing_authority_name desc";
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