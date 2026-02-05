using MySql.Data.MySqlClient;
using Serilog;
using SurPath.Entity.Master;
using SurPath.MySQLHelper;
using System;
using System.Collections.Generic;
using System.Data;

namespace SurPath.Data.Master
{
    public class DepartmentDao : DataObject
    {
        #region Constructor
        public static ILogger _logger { get; set; }

        public DepartmentDao(ILogger __logger = null)
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

        public int Insert(Department department)
        {
            int returnValue = 0;

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();

                try
                {
                    string sqlQuery = "INSERT INTO departments (department_name,is_active, is_synchronized, is_archived, created_on, created_by, last_modified_on, last_modified_by) VALUES (";
                    sqlQuery += "@DepartmentNameValue,@IsActive, b'0', b'0', NOW(), @CreatedBy, NOW(), @LastModifiedBy)";

                    MySqlParameter[] param = new MySqlParameter[4];
                    param[0] = new MySqlParameter("@DepartmentNameValue", department.DepartmentNameValue);
                    param[1] = new MySqlParameter("@IsActive", department.IsActive);
                    param[2] = new MySqlParameter("@CreatedBy", department.CreatedBy);
                    param[3] = new MySqlParameter("@LastModifiedBy", department.LastModifiedBy);

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                    sqlQuery = "SELECT LAST_INSERT_ID()";

                    returnValue = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlQuery));

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

        public int Update(Department department)
        {
            int returnValue = 0;

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();

                try
                {
                    string sqlQuery = "UPDATE departments SET "
                                    + "department_name = @DepartmentNameValue, "
                                    + "is_active=@IsActive, "
                                    + "is_synchronized = b'0', "
                                    + "last_modified_on = NOW(), "
                                    + "last_modified_by = @LastModifiedBy "
                                    + "WHERE department_id = @DepartmentId";

                    MySqlParameter[] param = new MySqlParameter[4];
                    param[0] = new MySqlParameter("@DepartmentId", department.DepartmentId);
                    param[1] = new MySqlParameter("@DepartmentNameValue", department.DepartmentNameValue);
                    param[2] = new MySqlParameter("@IsActive", department.IsActive);
                    param[3] = new MySqlParameter("@LastModifiedBy", department.LastModifiedBy);

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

        public int Delete(int departmentId, string currentUserName)
        {
            int returnValue = 0;

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();
                string sqlCount1Query = "Select count(*) from users where is_archived = 0 AND department_id = " + departmentId + "";

                int table1 = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlCount1Query));
                if (table1 <= 0)
                {
                    try
                    {
                        string sqlQuery = "UPDATE departments SET is_archived = b'1',is_synchronized = b'1', last_modified_on = NOW(), last_modified_by = @LastModifiedBy WHERE department_id = @DepartmentId";

                        MySqlParameter[] param = new MySqlParameter[2];
                        param[0] = new MySqlParameter("@DepartmentId", departmentId);
                        param[1] = new MySqlParameter("@LastModifiedBy", currentUserName);

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
            }

            return returnValue;
        }

        public Department Get(int departmentId)
        {
            Department department = null;
            string sqlQuery = "SELECT department_id AS DepartmentId, department_name AS DepartmentNameValue,is_active AS IsActive, is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM departments WHERE department_id = @DepartmentId";

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                MySqlParameter[] param = new MySqlParameter[1];
                param[0] = new MySqlParameter("@DepartmentId", departmentId);

                MySqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                if (dr.Read())
                {
                    department = new Department();
                    department.DepartmentId = (int)dr["DepartmentId"];
                    department.DepartmentNameValue = (string)dr["DepartmentNameValue"];
                    department.IsActive = dr["IsActive"].ToString() == "1" ? true : false;
                    department.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    department.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    department.CreatedOn = (DateTime)dr["CreatedOn"];
                    department.CreatedBy = (string)dr["CreatedBy"];
                    department.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    department.LastModifiedBy = (string)dr["LastModifiedBy"];
                }
            }

            return department;
        }

        public DataTable GetByDepartmentName(string departmentName)
        {
            string sqlQuery = "SELECT department_id AS DepartmentId, department_name AS DepartmentNameValue, is_active AS IsActive, is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM departments WHERE is_archived = 0 AND UPPER(department_name) = UPPER(@DepartmentName)";

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

        public DataTable GetList()
        {
            string sqlQuery = "SELECT department_id AS DepartmentId, department_name AS DepartmentNameValue, is_active AS IsActive, is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM departments WHERE is_archived = 0 ORDER BY department_name";

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
            string sqlQuery = "SELECT department_id AS DepartmentId, department_name AS DepartmentNameValue,is_active AS IsActive, is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy "
                              + "FROM departments WHERE is_archived = 0 ";

            List<MySqlParameter> param = new List<MySqlParameter>();
            bool isInActiveFlag = false;

            foreach (KeyValuePair<string, string> searchItem in searchParam)
            {
                if (searchItem.Key == "GeneralSearch")
                {
                    sqlQuery += "AND department_name LIKE @SearchKeyword ";

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

            sqlQuery += " ORDER BY department_name";

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
            string sqlQuery = "SELECT department_id AS DepartmentId, department_name AS DepartmentNameValue,is_active AS IsActive, is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy "
                             + "FROM departments WHERE is_archived = 0 ";

            if (active == false)
            {
                sqlQuery += "AND is_active = b'1'";
            }

            if (searchparam == "departmentName")
            {
                if (getInActive == "1")
                {
                    sql = "ORDER BY DepartmentNameValue";
                }
                else
                {
                    sql = "ORDER BY DepartmentNameValue desc";
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