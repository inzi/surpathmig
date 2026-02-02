using MySql.Data.MySqlClient;
using SurPath.MySQLHelper;
using System;
using System.Data;

namespace SurPath.Data
{
    public partial class DonorDao : DataObject
    {
        public DataTable GetPerformanceDetails(DateTime dtStart, DateTime dtEnd, string clientDepartmentList)
        {
            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                //UA Revenue - for the test
                string sqlQuery = "SELECT "
                            + "client_id AS ClientId, client_department_id AS ClientDepartmentId, "
                            + "SUM(CASE WHEN test_status = 1 THEN 1 ELSE 0 END) AS PreregistrationCount, "
                            + "SUM(CASE WHEN test_status = 2 THEN 1 ELSE 0 END) AS ActivatedCount, "
                            + "SUM(CASE WHEN test_status = 3 THEN 1 ELSE 0 END) AS RegisteredCount, "
                            + "SUM(CASE WHEN test_status = 4 THEN 1 ELSE 0 END) AS InQueueCount, "
                            + "SUM(CASE WHEN test_status = 5 THEN 1 ELSE 0 END) AS SuspensionQueueCount, "
                            + "SUM(CASE WHEN test_status = 6 THEN 1 ELSE 0 END) AS ProcessingCount, "
                            + "SUM(CASE WHEN test_status = 7 THEN 1 ELSE 0 END) AS CompletedCount "
                            + "FROM donor_test_info "
                            + "WHERE (CAST(test_requested_date AS DATE) BETWEEN CAST(@StartDate AS DATE) AND CAST(@EndDate AS DATE)) "
                            + "AND client_department_id IN (" + clientDepartmentList + ")"
                            + "GROUP BY client_id, client_department_id ";

                MySqlParameter[] param = new MySqlParameter[2];
                param[0] = new MySqlParameter("@StartDate", dtStart.ToString("yyyy-MM-dd"));
                param[1] = new MySqlParameter("@EndDate", dtEnd.ToString("yyyy-MM-dd"));

                DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sqlQuery, param);

                DataTable dtPerformance = ds.Tables[0];

                return dtPerformance;
            }
        }

        public DataTable GetPerformanceDetailsBySorting(DateTime dtStart, DateTime dtEnd, string clientDepartmentList, string searchparam, string getInActive = null)
        {
            string sql = string.Empty;
            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                //UA Revenue - for the test
                string sqlQuery = "SELECT "
                            + "donor_test_info.client_id AS ClientId, donor_test_info.client_department_id AS ClientDepartmentId,clients.client_name, client_departments.department_name, "
                            + "SUM(CASE WHEN test_status = 1 THEN 1 ELSE 0 END) AS PreregistrationCount, "
                            + "SUM(CASE WHEN test_status = 2 THEN 1 ELSE 0 END) AS ActivatedCount, "
                            + "SUM(CASE WHEN test_status = 3 THEN 1 ELSE 0 END) AS RegisteredCount, "
                            + "SUM(CASE WHEN test_status = 4 THEN 1 ELSE 0 END) AS InQueueCount, "
                            + "SUM(CASE WHEN test_status = 5 THEN 1 ELSE 0 END) AS SuspensionQueueCount, "
                            + "SUM(CASE WHEN test_status = 6 THEN 1 ELSE 0 END) AS ProcessingCount, "
                            + "SUM(CASE WHEN test_status = 7 THEN 1 ELSE 0 END) AS CompletedCount "
                            + "FROM donor_test_info "
                            + "inner join clients on clients.client_id = donor_test_info.client_id "
                            + "inner join client_departments on client_departments.client_department_id = donor_test_info.client_department_id "
                            + "WHERE (CAST(test_requested_date AS DATE) BETWEEN CAST(@StartDate AS DATE) AND CAST(@EndDate AS DATE)) "
                            + "AND donor_test_info.client_department_id IN (" + clientDepartmentList + ")"
                            + "GROUP BY donor_test_info.client_id, donor_test_info.client_department_id ";

                if (searchparam == "client")
                {
                    if (getInActive == "1")
                    {
                        sql = "ORDER BY clients.client_name";
                    }
                    else
                    {
                        sql = "ORDER BY clients.client_name desc";
                    }
                }

                if (searchparam == "department")
                {
                    if (getInActive == "1")
                    {
                        sql = "ORDER BY client_departments.department_name";
                    }
                    else
                    {
                        sql = "ORDER BY client_departments.department_name desc";
                    }
                }

                if (searchparam == "registered")
                {
                    if (getInActive == "1")
                    {
                        sql = "ORDER BY donor_test_info.test_status = 3";
                    }
                    else
                    {
                        sql = "ORDER BY donor_test_info.test_status = 3 desc";
                    }
                }
                if (searchparam == "inQueue")
                {
                    if (getInActive == "1")
                    {
                        sql = "ORDER BY donor_test_info.test_status = 4";
                    }
                    else
                    {
                        sql = "ORDER BY donor_test_info.test_status = 4 desc";
                    }
                }
                if (searchparam == "suspensionQueue")
                {
                    if (getInActive == "1")
                    {
                        sql = "ORDER BY donor_test_info.test_status = 5";
                    }
                    else
                    {
                        sql = "ORDER BY donor_test_info.test_status = 5 desc";
                    }
                }
                if (searchparam == "processing")
                {
                    if (getInActive == "1")
                    {
                        sql = "ORDER BY donor_test_info.test_status = 6";
                    }
                    else
                    {
                        sql = "ORDER BY donor_test_info.test_status = 6 desc";
                    }
                }
                if (searchparam == "completed")
                {
                    if (getInActive == "1")
                    {
                        sql = "ORDER BY donor_test_info.test_status = 7";
                    }
                    else
                    {
                        sql = "ORDER BY donor_test_info.test_status = 7 desc";
                    }
                }

                sqlQuery = sqlQuery + sql;

                MySqlParameter[] param = new MySqlParameter[2];
                param[0] = new MySqlParameter("@StartDate", dtStart.ToString("yyyy-MM-dd"));
                param[1] = new MySqlParameter("@EndDate", dtEnd.ToString("yyyy-MM-dd"));

                DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sqlQuery, param);

                DataTable dtPerformance = ds.Tables[0];

                return dtPerformance;
            }
        }
    }
}