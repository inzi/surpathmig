using MySql.Data.MySqlClient;
using Serilog;
using SurPath.Entity.Master;
using SurPath.Enum;
using SurPath.MySQLHelper;
using System;
using System.Collections.Generic;
using System.Data;

namespace SurPath.Data.Master
{
    public class TestPanelDao : DataObject
    {
        #region Constructor
        public static ILogger _logger { get; set; }

        public TestPanelDao(ILogger __logger = null)
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

        public int Insert(TestPanel testPanel)
        {
            int returnValue = 0;

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();

                try
                {
                    string sqlQuery = "INSERT INTO test_panels (test_panel_name, test_panel_description, test_category_id, cost, is_active, is_synchronized, is_archived, created_on, created_by, last_modified_on, last_modified_by) VALUES (";
                    sqlQuery += "@TestPanelName, @TestPanelDescription, @TestCategoryId, @TestCost,@IsActive, b'0', b'0', NOW(), @CreatedBy, NOW(), @LastModifiedBy)";

                    MySqlParameter[] param = new MySqlParameter[7];
                    param[0] = new MySqlParameter("@TestPanelName", testPanel.TestPanelName);
                    param[1] = new MySqlParameter("@TestPanelDescription", testPanel.TestPanelDescription);
                    param[2] = new MySqlParameter("@TestCategoryId", testPanel.TestCategoryId);
                    param[3] = new MySqlParameter("@TestCost", testPanel.TestCost);
                    param[4] = new MySqlParameter("@IsActive", testPanel.IsActive);
                    param[5] = new MySqlParameter("@CreatedBy", testPanel.CreatedBy);
                    param[6] = new MySqlParameter("@LastModifiedBy", testPanel.LastModifiedBy);

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                    sqlQuery = "SELECT LAST_INSERT_ID()";

                    returnValue = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlQuery));

                    if (testPanel.DrugNames.Count > 0)
                    {
                        foreach (int drugNameId in testPanel.DrugNames)
                        {
                            sqlQuery = "INSERT INTO test_panel_drug_names (test_panel_id, drug_name_id, is_synchronized, created_on, created_by, last_modified_on, last_modified_by) VALUES (";
                            sqlQuery += "@TestPanelId, @DrugNameId, b'0', NOW(), @CreatedBy, NOW(), @LastModifiedBy)";

                            param = new MySqlParameter[4];
                            param[0] = new MySqlParameter("@TestPanelId", returnValue);
                            param[1] = new MySqlParameter("@DrugNameId", drugNameId);
                            param[2] = new MySqlParameter("@CreatedBy", testPanel.CreatedBy);
                            param[3] = new MySqlParameter("@LastModifiedBy", testPanel.LastModifiedBy);

                            SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);
                        }
                    }

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

        public int Update(TestPanel testPanel)
        {
            int returnValue = 0;

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();

                try
                {
                    string sqlQuery = "UPDATE test_panels SET "
                                    + "test_panel_name = @TestPanelName, "
                                    + "test_panel_description = @TestPanelDescription, "
                                    + "test_category_id = @TestCategoryId, "
                                    + "cost = @TestCost, "
                                    + "is_active =@IsActive, "
                                    + "is_synchronized = b'0', "
                                    + "last_modified_on = NOW(), "
                                    + "last_modified_by = @LastModifiedBy "
                                    + "WHERE test_panel_id = @TestPanelId";

                    MySqlParameter[] param = new MySqlParameter[7];
                    param[0] = new MySqlParameter("@TestPanelId", testPanel.TestPanelId);
                    param[1] = new MySqlParameter("@TestPanelName", testPanel.TestPanelName);
                    param[2] = new MySqlParameter("@TestPanelDescription", testPanel.TestPanelDescription);
                    param[3] = new MySqlParameter("@TestCategoryId", testPanel.TestCategoryId);
                    param[4] = new MySqlParameter("@TestCost", testPanel.TestCost);
                    param[5] = new MySqlParameter("@IsActive", testPanel.IsActive);
                    param[6] = new MySqlParameter("@LastModifiedBy", testPanel.LastModifiedBy);

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                    sqlQuery = "DELETE FROM test_panel_drug_names WHERE test_panel_id = @TestPanelId";

                    param = new MySqlParameter[1];
                    param[0] = new MySqlParameter("@TestPanelId", testPanel.TestPanelId);

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                    if (testPanel.DrugNames.Count > 0)
                    {
                        foreach (int drugNameId in testPanel.DrugNames)
                        {
                            sqlQuery = "INSERT INTO test_panel_drug_names (test_panel_id, drug_name_id, is_synchronized, created_on, created_by, last_modified_on, last_modified_by) VALUES (";
                            sqlQuery += "@TestPanelId, @DrugNameId, b'0', NOW(), @CreatedBy, NOW(), @LastModifiedBy)";

                            param = new MySqlParameter[4];
                            param[0] = new MySqlParameter("@TestPanelId", testPanel.TestPanelId);
                            param[1] = new MySqlParameter("@DrugNameId", drugNameId);
                            param[2] = new MySqlParameter("@CreatedBy", testPanel.LastModifiedBy);
                            param[3] = new MySqlParameter("@LastModifiedBy", testPanel.LastModifiedBy);

                            SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);
                        }
                    }

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

        public int Delete(int TestPanelId, string currentUsername)
        {
            int returnValue = 0;

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();
                try
                {
                    string sqlCount1Query = "Select count(*) from client_dept_test_panels "
                                          + "inner join client_dept_test_categories on client_dept_test_panels.client_dept_test_category_id =client_dept_test_categories.client_dept_test_category_id "
                                          + "inner join client_departments on client_dept_test_categories.client_department_id =client_departments.client_department_id "
                                          + "inner join clients on client_departments.client_id =clients.client_id "
                                          + "where client_dept_test_panels.test_panel_id = " + TestPanelId + " and clients.is_archived = 0 ";

                    int table1 = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlCount1Query));
                    if (table1 <= 0)
                    {
                        string sqlCount3Query = "Select count(*) from donor_test_info_test_categories where test_panel_id = " + TestPanelId + "";

                        int table2 = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlCount3Query));
                        if (table2 <= 0)
                        {
                            string sqlQuery = "UPDATE test_panels SET is_synchronized = b'0', is_archived = b'1', last_modified_on = NOW(), last_modified_by = @LastModifiedBy WHERE test_panel_id = @TestPanelId";

                            MySqlParameter[] param = new MySqlParameter[2];
                            param[0] = new MySqlParameter("@TestPanelId", TestPanelId);
                            param[1] = new MySqlParameter("@LastModifiedBy", currentUsername);

                            SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                            trans.Commit();

                            returnValue = 1;
                        }
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

        public int TestPanelActive(int TestPanelId)
        {
            int returnValue = 0;

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();

                string sqlCount1Query = "Select count(*) from client_dept_test_panels "
                                       + "inner join client_dept_test_categories on client_dept_test_panels.client_dept_test_category_id =client_dept_test_categories.client_dept_test_category_id "
                                       + "inner join client_departments on client_dept_test_categories.client_department_id =client_departments.client_department_id "
                                       + "inner join clients on client_departments.client_id =clients.client_id "
                                       + "where client_dept_test_panels.test_panel_id = " + TestPanelId + " and clients.is_archived = 0 ";

                returnValue = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlCount1Query));
            }
            return returnValue;
        }

        public int UnmapTestPanel(TestPanel testPanel)
        {
            int returnValue = 0;

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();
                try
                {
                    string sqlTestPanelUpdate = "UPDATE client_dept_test_panels SET "
                                                         + "test_panel_id = '0', "
                                                         + "test_panel_price = '0.00', "
                                                         + "is_synchronized = b'0', "
                                                         + "last_modified_on = NOW(), "
                                                         + "last_modified_by = @LastModifiedBy "
                                                         + "WHERE test_panel_id = @TestPanelId";

                    MySqlParameter[] param = new MySqlParameter[2];
                    param[0] = new MySqlParameter("@TestPanelId", testPanel.TestPanelId);
                    param[1] = new MySqlParameter("@LastModifiedBy", testPanel.LastModifiedBy);

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlTestPanelUpdate, param);
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

        public TestPanel Get(int TestPanelId)
        {
            TestPanel testPanel = null;
            string sqlQuery = "SELECT test_panel_id AS TestPanelId, test_panel_name AS TestPanelName, test_panel_description AS TestPanelDescription, test_category_id AS TestCategoryId, cost AS TestCost,is_active AS IsActive, is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM test_panels WHERE test_panel_id = @TestPanelId";

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                MySqlParameter[] param = new MySqlParameter[1];
                param[0] = new MySqlParameter("@TestPanelId", TestPanelId);

                MySqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                if (dr.Read())
                {
                    testPanel = new TestPanel();
                    testPanel.TestPanelId = (int)dr["TestPanelId"];
                    testPanel.TestPanelName = (string)dr["TestPanelName"];
                    testPanel.TestPanelDescription = dr["TestPanelDescription"].ToString();
                    testPanel.TestCategoryId = (int)dr["TestCategoryId"];
                    testPanel.TestCost = (double)dr["TestCost"];
                    testPanel.IsActive = dr["IsActive"].ToString() == "1" ? true : false;
                    testPanel.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    testPanel.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    testPanel.CreatedOn = (DateTime)dr["CreatedOn"];
                    testPanel.CreatedBy = (string)dr["CreatedBy"];
                    testPanel.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    testPanel.LastModifiedBy = (string)dr["LastModifiedBy"];
                }
                dr.Close();

                sqlQuery = "SELECT drug_name_id AS DrugNameId FROM test_panel_drug_names WHERE test_panel_id = @TestPanelId";

                param = new MySqlParameter[1];
                param[0] = new MySqlParameter("@TestPanelId", TestPanelId);

                dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                if (testPanel != null)
                {
                    while (dr.Read())
                    {
                        testPanel.DrugNames.Add((int)dr["DrugNameId"]);
                    }
                }
            }

            return testPanel;
        }

        public DataTable GetList()
        {
            string sqlQuery = "SELECT test_panel_id AS TestPanelId, test_panel_name AS TestPanelName, test_panel_description AS TestPanelDescription, test_category_id AS TestCategoryId, cost AS TestCost, is_active AS IsActive, is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM test_panels WHERE is_archived = 0 ORDER BY test_panel_name";

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

        public DataTable GetListByCatgory(TestCategories testCategory)
        {
            string sqlQuery = "SELECT test_panel_id AS TestPanelId, test_panel_name AS TestPanelName, test_panel_description AS TestPanelDescription, test_category_id AS TestCategoryId, cost AS TestCost, is_active AS IsActive, is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM test_panels WHERE is_archived = 0 AND test_category_id = @TestCategoryId ORDER BY test_panel_name";

            MySqlParameter[] param = new MySqlParameter[1];
            param[0] = new MySqlParameter("@TestCategoryId", (int)testCategory);

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

        public DataTable GetListByUA()
        {
            string sqlQuery = "SELECT test_panel_id AS TestPanelId, test_panel_name AS TestPanelName, test_panel_description AS TestPanelDescription, test_category_id AS TestCategoryId, cost AS TestCost, is_active AS IsActive, is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM test_panels WHERE is_archived = 0 AND test_category_id = 1 ORDER BY test_panel_name";

            //MySqlParameter[] param = new MySqlParameter[1];
            //param[0] = new MySqlParameter("@TestCategoryId", (int)testCategory);

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

        public DataTable GetListByHair()
        {
            string sqlQuery = "SELECT test_panel_id AS TestPanelId, test_panel_name AS TestPanelName, test_panel_description AS TestPanelDescription, test_category_id AS TestCategoryId, cost AS TestCost, is_active AS IsActive, is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM test_panels WHERE is_archived = 0 AND test_category_id = 2 ORDER BY test_panel_name";

            //MySqlParameter[] param = new MySqlParameter[1];
            //param[0] = new MySqlParameter("@TestCategoryId", (int)testCategory);

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

        public DataTable GetByTestPanelCode(string PanelName)
        {
            string sqlQuery = "SELECT test_panel_id AS TestPanelId, test_panel_name AS TestPanelName, test_panel_description AS TestPanelDescription, test_category_id AS TestCategoryId, cost AS TestCost, is_active AS IsActive, is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM test_panels WHERE is_archived = 0 AND Upper(test_panel_name) = UPPER(@PanelName)";

            MySqlParameter[] param = new MySqlParameter[1];
            param[0] = new MySqlParameter("@PanelName", PanelName);

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
            string sqlQuery = "SELECT test_panel_id AS TestPanelId, test_panel_name AS TestPanelName, test_panel_description AS TestPanelDescription, test_category_id AS TestCategoryId, cost AS TestCost, is_active AS IsActive, is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy "
                              + "FROM test_panels WHERE is_archived = 0 ";

            List<MySqlParameter> param = new List<MySqlParameter>();
            bool isInActiveFlag = false;

            foreach (KeyValuePair<string, string> searchItem in searchParam)
            {
                if (searchItem.Key == "GeneralSearch")
                {
                    sqlQuery += " AND (test_panel_name LIKE @SearchKeyword OR test_category_id IN (SELECT test_category_id FROM test_categories WHERE test_category_name LIKE @SearchKeyword) OR cost LIKE @SearchKeyword)";

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

            sqlQuery += " ORDER BY test_panel_name ";

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

        public DataTable GetDrugNameList(int testPanelId)
        {
            string sqlQuery = "SELECT drug_name_id AS DrugNameId FROM test_panel_drug_names WHERE test_panel_id = @TestPanelId";

            MySqlParameter[] param = new MySqlParameter[1];
            param[0] = new MySqlParameter("@TestPanelId", testPanelId);

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

        public DataTable sorting(string searchparam, bool active, string getInActive = null)
        {
            string sql = string.Empty;
            string sqlQuery = "SELECT test_panel_id AS TestPanelId, test_panel_name AS TestPanelName, test_panel_description AS TestPanelDescription, test_category_id AS TestCategoryId, cost AS TestCost, is_active AS IsActive, is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy "
                             + "FROM test_panels WHERE is_archived = 0 ";
            if (active == false)
            {
                sqlQuery += "AND is_active = b'1'";
            }

            if (searchparam == "testPanelName")
            {
                if (getInActive == "1")
                {
                    sql = "ORDER BY TestPanelName";
                }
                else
                {
                    sql = "ORDER BY TestPanelName desc";
                }
            }
            if (searchparam == "testCategory")
            {
                if (getInActive == "1")
                {
                    sql = "ORDER BY TestCategoryId";
                }
                else
                {
                    sql = "ORDER BY TestCategoryId desc";
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