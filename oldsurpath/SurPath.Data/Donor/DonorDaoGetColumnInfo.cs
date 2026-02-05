using MySql.Data.MySqlClient;
using SurPath.MySQLHelper;
using System;
using System.Collections.Generic;
using System.Data;

namespace SurPath.Data
{
    public partial class DonorDao : DataObject
    {
        public DataTable ColumnsName()
        {
            string sqlQuery = "select donor_grid_columns_header_id as ColumnId ,donor_grid_columns_header_name as ColumnName,is_active AS IsActive from donor_grid_columns_header ";

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

        public DataTable GetColumnName()
        {
            string sqlQuery = "SELECT "
                            + "donor_grid_columns_header_id AS ColumnId, "
                            + "donor_grid_columns_header_name AS ColumnName, "
                            + "is_active AS IsActive "
                            + "from donor_grid_columns_header"
                            + " WHERE is_active = b'0'";

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

        public DataTable DisplayColumns(string strSQL, Dictionary<string, string> searchParam)
        {
            string sqlSubQuery = string.Empty;

            string subSql = " LEFT OUTER JOIN donor_test_info ON donors.donor_id = donor_test_info.donor_id ";

            List<MySqlParameter> param = new List<MySqlParameter>();

            foreach (KeyValuePair<string, string> searchItem in searchParam)
            {
                if (searchItem.Key == "DonorId")
                {
                    sqlSubQuery += "AND donors.donor_id = @DonorId ";
                    param.Add(new MySqlParameter("@DonorId", searchItem.Value));
                }
                else if (searchItem.Key == "DonorTestInfoId")
                {
                    sqlSubQuery += "AND donor_test_info.donor_test_info_id = @DonorTestInfoId ";
                    param.Add(new MySqlParameter("@DonorTestInfoId", searchItem.Value));
                }
                else if (searchItem.Key == "FirstName")
                {
                    sqlSubQuery += "AND donors.donor_first_name LIKE @DonorFirstName ";
                    param.Add(new MySqlParameter("@DonorFirstName", searchItem.Value));
                }
                else if (searchItem.Key == "LastName")
                {
                    sqlSubQuery += "AND donors.donor_last_name LIKE @DonorLastName ";
                    param.Add(new MySqlParameter("@DonorLastName", searchItem.Value));
                }
                else if (searchItem.Key == "SSN")
                {
                    sqlSubQuery += "AND donors.donor_ssn LIKE @DonorSSN ";
                    param.Add(new MySqlParameter("@DonorSSN", searchItem.Value));
                }
                else if (searchItem.Key == "DOB")
                {
                    sqlSubQuery += "AND CAST(donors.donor_date_of_birth AS DATE) = CAST(@DonorDateOfBirth AS DATE) ";
                    param.Add(new MySqlParameter("@DonorDateOfBirth", Convert.ToDateTime(searchItem.Value).ToString("yyyy-MM-dd")));
                }
                else if (searchItem.Key == "City")
                {
                    sqlSubQuery += "AND donors.donor_city LIKE @DonorCity ";
                    param.Add(new MySqlParameter("@DonorCity", searchItem.Value));
                }
                else if (searchItem.Key == "ZipCode")
                {
                    sqlSubQuery += "AND donors.donor_zip = @DonorZipCode ";
                    param.Add(new MySqlParameter("@DonorZipCode", searchItem.Value));
                }
                else if (searchItem.Key == "DonorEmail")
                {
                    sqlSubQuery += "AND donors.donor_email = @DonorEmail ";
                    param.Add(new MySqlParameter("@DonorEmail", searchItem.Value));
                }
                else if (searchItem.Key == "SpecimenId")
                {
                    sqlSubQuery += "AND donor_test_info.donor_test_info_id IN (SELECT donor_test_info_test_categories.donor_test_info_id FROM donor_test_info_test_categories WHERE donor_test_info_test_categories.specimen_id LIKE @SpecimenId) ";
                    param.Add(new MySqlParameter("@SpecimenId", searchItem.Value));
                }
                else if (searchItem.Key == "TestReason")
                {
                    sqlSubQuery += "AND donor_test_info.reason_for_test_id = @ReasonForTestId ";
                    param.Add(new MySqlParameter("@ReasonForTestId", Convert.ToInt32(searchItem.Value)));
                }
                else if (searchItem.Key == "Client")
                {
                    sqlSubQuery += "AND (donors.donor_initial_client_id = @ClientId OR donor_test_info.client_id = @ClientId) ";
                    param.Add(new MySqlParameter("@ClientId", Convert.ToInt32(searchItem.Value)));
                }
                else if (searchItem.Key == "Department")
                {
                    sqlSubQuery += "AND donor_test_info.client_department_id = @ClientDepartmentId ";
                    param.Add(new MySqlParameter("@ClientDepartmentId", Convert.ToInt32(searchItem.Value)));
                }
                //else if (searchItem.Key == "Status")
                //{
                //    if (searchItem.Value == "1" || searchItem.Value == "2")
                //    {
                //        sqlQuery += "AND donors.donor_registration_status = @TestStatus ";
                //    }
                //    else
                //    {
                //        sqlQuery += "AND donor_test_info.test_status = @TestStatus ";
                //    }
                //    param.Add(new MySqlParameter("@TestStatus", Convert.ToInt32(searchItem.Value)));
                //}
                else if (searchItem.Key == "Status")
                {
                    if (searchItem.Value == "1")
                    {
                        sqlSubQuery += "AND donor_test_info.test_status = @Status ";
                        param.Add(new MySqlParameter("@Status", Convert.ToInt32("3")));
                    }
                    if (searchItem.Value == "2")
                    {
                        sqlSubQuery += "AND donor_test_info.test_status = @Status ";
                        param.Add(new MySqlParameter("@Status", Convert.ToInt32("4")));
                    }
                    if (searchItem.Value == "3")
                    {
                        sqlSubQuery += "AND donor_test_info.test_status = @Status ";
                        param.Add(new MySqlParameter("@Status", Convert.ToInt32("5")));
                    }
                    if (searchItem.Value == "4")
                    {
                        sqlSubQuery += "AND donor_test_info.test_status = @Status ";
                        param.Add(new MySqlParameter("@Status", Convert.ToInt32("6")));
                    }
                    if (searchItem.Value == "5")
                    {
                        sqlSubQuery += "AND donor_test_info.test_status = @Status ";
                        param.Add(new MySqlParameter("@Status", Convert.ToInt32("7")));
                    }
                }
                else if (searchItem.Key == "TestCategory")
                {
                    sqlSubQuery += "AND donor_test_info.donor_test_info_id IN (SELECT donor_test_info_test_categories.donor_test_info_id FROM donor_test_info_test_categories WHERE donor_test_info_test_categories.test_category_id = @TestCategoryId) ";
                    param.Add(new MySqlParameter("@TestCategoryId", Convert.ToInt32(searchItem.Value)));
                }
                else if (searchItem.Key == "IncludeArchive")
                {
                    if (!Convert.ToBoolean(searchItem.Value))
                    {
                        sqlSubQuery += "AND donors.is_archived = b'0' ";
                    }
                }
                else if (searchItem.Key == "Walkin")
                {
                    if (!Convert.ToBoolean(searchItem.Value))
                    {
                        //
                    }
                    else
                    {
                        sqlSubQuery += "AND donor_test_info.is_walkin_donor = b'1' ";
                    }
                }
                else if (searchItem.Key == "ShowAll")
                {
                    if (!Convert.ToBoolean(searchItem.Value))
                    {
                        subSql = subSql + " AND donor_test_info.donor_test_info_id = (SELECT max(donor_test_info.donor_test_info_id) FROM donor_test_info WHERE donor_test_info.donor_id = donors.donor_id) ";
                    }
                }
                else if (searchItem.Key == "NoOfDays")
                {
                    if (!searchItem.Value.StartsWith("DR"))
                    {
                        int dateInterval = Convert.ToInt32(searchItem.Value) * -1;

                        sqlSubQuery += "AND donor_test_info.screening_time >= DATE_ADD(CURDATE(), INTERVAL @DateInterval DAY) ";
                        param.Add(new MySqlParameter("@DateInterval", dateInterval));
                    }
                    else
                    {
                        sqlSubQuery += "AND (CAST(donor_test_info.screening_time AS DATE) BETWEEN CAST(@StartDate AS DATE) AND CAST(@EndDate AS DATE)) ";
                        string[] dates = searchItem.Value.Split('#');
                        param.Add(new MySqlParameter("@StartDate", Convert.ToDateTime(dates[1]).ToString("yyyy-MM-dd")));
                        param.Add(new MySqlParameter("@EndDate", Convert.ToDateTime(dates[2]).ToString("yyyy-MM-dd")));
                    }
                }
            }
            sqlSubQuery += "ORDER BY donors.donor_first_name, donors.donor_last_name";
            string sqlQuery = "SELECT "
                            //  + "donors.donor_id AS DonorId, "
                            + "donors.donor_initial_client_id AS DonorInitialClientId, "
                            + "donors.donor_registration_status AS DonorRegistrationStatus, "
                            + "donor_test_info.is_walkin_donor AS IsWalkinDonor, "
                            + "donor_test_info.instant_test_result AS InstantTestResult, "
                            + strSQL
                            + " FROM donors "
                            + " " + subSql + " "
                            //+ "LEFT OUTER JOIN donor_test_info_test_categories ON donor_test_info.donor_test_info_id = donor_test_info_test_categories.donor_test_info_id "
                            //  + "LEFT OUTER JOIN clients ON ((donors.donor_initial_client_id = clients.client_id AND donors.donor_initial_department_id IS NULL) OR donor_test_info.client_id = clients.client_id) "
                            + "LEFT OUTER JOIN clients ON (donor_test_info.client_id = clients.client_id) "
                            //+ "LEFT OUTER JOIN client_departments ON donor_test_info.client_department_id = client_departments.client_department_id "
                            + "LEFT OUTER JOIN client_departments ON donor_test_info.client_department_id = client_departments.client_department_id "
                            + "LEFT OUTER JOIN users ON donor_test_info.collection_site_user_id = users.user_id "
                            + "WHERE 1=1 AND donors.donor_registration_status >=3 "
                            + "" + sqlSubQuery + "";

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

        public int ColumnsAdd(string columnId, string columnName)
        {
            int returnValue = 0;

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();

                try
                {
                    string sqlQuery = "UPDATE donor_grid_columns_header SET "
                                        + "is_active =b'1' "
                                        + "WHERE donor_grid_columns_header_id = @ColumnId";

                    MySqlParameter[] param = new MySqlParameter[1];

                    param[0] = new MySqlParameter("@ColumnId", columnId);
                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

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

        public int ColumnsRemove(string columnId, string columnName)
        {
            int returnValue = 0;

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();

                try
                {
                    string sqlQuery = "UPDATE donor_grid_columns_header SET "
                                        + "is_active =b'0' "
                                        + "WHERE donor_grid_columns_header_id = @ColumnId";

                    MySqlParameter[] param = new MySqlParameter[1];

                    param[0] = new MySqlParameter("@ColumnId", columnId);
                    //param[1] = new MySqlParameter("@IsActive", donor.IsActive);
                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

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
    }
}