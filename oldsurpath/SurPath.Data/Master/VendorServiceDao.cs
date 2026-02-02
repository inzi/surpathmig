using MySql.Data.MySqlClient;
using SurPath.Entity.Master;
using SurPath.Enum;
using SurPath.MySQLHelper;
using System;
using System.Collections.Generic;
using System.Data;

namespace SurPath.Data.Master
{
    public class VendorServiceDao : DataObject
    {
        #region Constructor

        public VendorServiceDao()
        {
            //
        }

        #endregion Constructor

        #region Public Methods

        public int Insert(VendorService vendorService)
        {
            int returnValue = 0;

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();

                try
                {
                    string sqlQuery = "INSERT INTO vendor_services (vendor_service_name, cost, test_category_id, is_observed, form_type_id, is_active, is_synchronized, is_archived, created_on, created_by, last_modified_on, last_modified_by) VALUES (";
                    sqlQuery += " @VendorServiceNameValue, @Cost, @TestCategoryId, @IsObserved, @FormTypeId, @IsActive, b'0', b'0', NOW(), @CreatedBy, NOW(), @LastModifiedBy)";

                    MySqlParameter[] param = new MySqlParameter[8];
                    param[0] = new MySqlParameter("@VendorServiceNameValue", vendorService.VendorServiceNameValue);
                    param[1] = new MySqlParameter("@Cost", vendorService.Cost);
                    param[2] = new MySqlParameter("@TestCategoryId", vendorService.TestCategoryId);
                    param[3] = new MySqlParameter("@IsObserved", vendorService.IsObserved);
                    param[4] = new MySqlParameter("@FormTypeId", vendorService.FormTypeId);
                    param[5] = new MySqlParameter("@IsActive", vendorService.IsActive);
                    param[6] = new MySqlParameter("@CreatedBy", vendorService.CreatedBy);
                    param[7] = new MySqlParameter("@LastModifiedBy", vendorService.LastModifiedBy);

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                    sqlQuery = "SELECT LAST_INSERT_ID()";

                    returnValue = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlQuery));

                    sqlQuery = "INSERT INTO vendor_vendor_services (vendor_id, vendor_service_id, is_synchronized, created_on, created_by, last_modified_on, last_modified_by) VALUES (";
                    sqlQuery += "@VendorId, @VendorServiceId, b'0', NOW(), @CreatedBy, NOW(), @LastModifiedBy)";

                    param = new MySqlParameter[4];
                    param[0] = new MySqlParameter("@VendorId", vendorService.VendorId);
                    param[1] = new MySqlParameter("@VendorServiceId", returnValue);
                    param[2] = new MySqlParameter("@CreatedBy", vendorService.CreatedBy);
                    param[3] = new MySqlParameter("@LastModifiedBy", vendorService.LastModifiedBy);

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

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

        public int Update(VendorService vendorService)
        {
            int returnValue = 0;

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();

                try
                {
                    string sqlQuery = "UPDATE vendor_services SET "
                                    + "vendor_service_name = @VendorServiceNameValue, "
                                    + "cost = @Cost, "
                                    + "test_category_id = @TestCategoryId, "
                                    + "is_observed = @IsObserved, "
                                    + "form_type_id = @FormTypeId, "
                                    + "is_active = @IsActive ,"
                                    + "is_synchronized = b'0', "
                                    + "last_modified_on = NOW(), "
                                    + "last_modified_by = @LastModifiedBy "
                                    + "WHERE vendor_service_id = @VendorServiceId";

                    MySqlParameter[] param = new MySqlParameter[8];
                    param[0] = new MySqlParameter("@VendorServiceId", vendorService.VendorServiceId);
                    param[1] = new MySqlParameter("@VendorServiceNameValue", vendorService.VendorServiceNameValue);
                    param[2] = new MySqlParameter("@Cost", vendorService.Cost);
                    param[3] = new MySqlParameter("@TestCategoryId", vendorService.TestCategoryId);
                    param[4] = new MySqlParameter("@IsObserved", vendorService.IsObserved);
                    param[5] = new MySqlParameter("@FormTypeId", vendorService.FormTypeId);
                    param[6] = new MySqlParameter("@IsActive", vendorService.IsActive);
                    param[7] = new MySqlParameter("@LastModifiedBy", vendorService.LastModifiedBy);

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

        public int Delete(int vendorServiceId, string currentUserName)
        {
            int returnValue = 0;

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();

                //string sqlCount1Query = "Select count(*) from vendor_vendor_services where vendor_service_id = " + vendorServiceId + "";

                //int table1 = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlCount1Query));

                //if (table1 <= 0)
                //{
                try
                {
                    string sqlQuery = "UPDATE vendor_services SET is_synchronized = b'0', is_archived = b'1', last_modified_on = NOW(), last_modified_by = @LastModifiedBy WHERE vendor_service_id = @VendorServiceId";

                    MySqlParameter[] param = new MySqlParameter[2];
                    param[0] = new MySqlParameter("@VendorServiceId", vendorServiceId);
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
                //}
            }

            return returnValue;
        }

        public VendorService Get(int vendorServiceId)
        {
            VendorService vendorService = null;

            string sqlQuery = "SELECT vendor_services.vendor_service_id AS VendorServiceId, vendor_vendor_services.vendor_id AS VendorId, vendor_services.vendor_service_name AS VendorServiceNameValue, vendor_services.cost AS Cost, vendor_services.is_observed AS IsObserved, vendor_services.test_category_id AS TestCategoryId, vendor_services.form_type_id AS FormTypeId, vendor_services.is_active As IsActive, vendor_services.is_synchronized AS IsSynchronized, vendor_services.is_archived AS IsArchived, vendor_services.created_on AS CreatedOn, vendor_services.created_by AS CreatedBy, vendor_services.last_modified_on AS LastModifiedOn, vendor_services.last_modified_by AS LastModifiedBy "
                              + "FROM vendor_services "
                              + "INNER JOIN vendor_vendor_services ON vendor_services.vendor_service_id = vendor_vendor_services.vendor_service_id "
                              + "WHERE vendor_services.vendor_service_id = @VendorServiceId";

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                MySqlParameter[] param = new MySqlParameter[1];
                param[0] = new MySqlParameter("@VendorServiceId", vendorServiceId);

                MySqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                if (dr.Read())
                {
                    vendorService = new VendorService();
                    vendorService.VendorServiceId = (int)dr["VendorServiceId"];
                    vendorService.VendorId = (int)dr["VendorId"];
                    vendorService.VendorServiceNameValue = (string)dr["VendorServiceNameValue"];
                    vendorService.TestCategoryId = dr["TestCategoryId"] != DBNull.Value ? Convert.ToInt32(dr["TestCategoryId"]) : 0;
                    vendorService.IsObserved = dr["IsObserved"] != DBNull.Value ? (YesNo)(Convert.ToInt32(dr["IsObserved"].ToString())) : YesNo.None;
                    vendorService.FormTypeId = dr["FormTypeId"] != DBNull.Value ? (SpecimenFormType)(Convert.ToInt32(dr["FormTypeId"].ToString())) : SpecimenFormType.None;
                    vendorService.Cost = (double)dr["Cost"];
                    vendorService.IsActive = dr["IsActive"].ToString() == "1" ? true : false;
                    vendorService.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    vendorService.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    vendorService.CreatedOn = (DateTime)dr["CreatedOn"];
                    vendorService.CreatedBy = (string)dr["CreatedBy"];
                    vendorService.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    vendorService.LastModifiedBy = (string)dr["LastModifiedBy"];
                }
            }

            return vendorService;
        }

        public VendorService GetVendorServiceByCostParam(int vendorId, int testCategoryId, YesNo observedType, SpecimenFormType formType)
        {
            VendorService vendorService = null;

            string sqlQuery = "SELECT vendor_services.vendor_service_id AS VendorServiceId, vendor_vendor_services.vendor_id AS VendorId, vendor_services.vendor_service_name AS VendorServiceNameValue, vendor_services.cost AS Cost, vendor_services.is_observed AS IsObserved, vendor_services.test_category_id AS TestCategoryId, vendor_services.form_type_id AS FormTypeId, vendor_services.is_active As IsActive, vendor_services.is_synchronized AS IsSynchronized, vendor_services.is_archived AS IsArchived, vendor_services.created_on AS CreatedOn, vendor_services.created_by AS CreatedBy, vendor_services.last_modified_on AS LastModifiedOn, vendor_services.last_modified_by AS LastModifiedBy "
                              + "FROM vendor_services "
                              + "INNER JOIN vendor_vendor_services ON vendor_services.vendor_service_id = vendor_vendor_services.vendor_service_id "
                              + "WHERE is_archived = 0 AND is_active = b'1' AND vendor_vendor_services.vendor_id = @VendorId "
                              + "AND vendor_services.test_category_id = @TestCategoryId "
                              + "AND vendor_services.is_observed = @IsObserved "
                              + "AND vendor_services.form_type_id = @FormTypeId";

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                MySqlParameter[] param = new MySqlParameter[4];
                param[0] = new MySqlParameter("@VendorId", vendorId);
                param[1] = new MySqlParameter("@TestCategoryId", testCategoryId);
                param[2] = new MySqlParameter("@IsObserved", (int)observedType);
                param[3] = new MySqlParameter("@FormTypeId", (int)formType);

                MySqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                if (dr.Read())
                {
                    vendorService = new VendorService();
                    vendorService.VendorServiceId = (int)dr["VendorServiceId"];
                    vendorService.VendorId = (int)dr["VendorId"];
                    vendorService.VendorServiceNameValue = (string)dr["VendorServiceNameValue"];
                    vendorService.TestCategoryId = dr["TestCategoryId"] != DBNull.Value ? Convert.ToInt32(dr["TestCategoryId"]) : 0;
                    vendorService.IsObserved = dr["IsObserved"] != DBNull.Value ? (YesNo)(Convert.ToInt32(dr["IsObserved"].ToString())) : YesNo.None;
                    vendorService.FormTypeId = dr["FormTypeId"] != DBNull.Value ? (SpecimenFormType)(Convert.ToInt32(dr["FormTypeId"].ToString())) : SpecimenFormType.None;
                    vendorService.Cost = (double)dr["Cost"];
                    vendorService.IsActive = dr["IsActive"].ToString() == "1" ? true : false;
                    vendorService.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    vendorService.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    vendorService.CreatedOn = (DateTime)dr["CreatedOn"];
                    vendorService.CreatedBy = (string)dr["CreatedBy"];
                    vendorService.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    vendorService.LastModifiedBy = (string)dr["LastModifiedBy"];
                }
            }

            return vendorService;
        }

        public DataTable GetByVendorServiceName(int vendorId, string vendorServiceName)
        {
            string sqlQuery = "SELECT vendor_services.vendor_service_id AS VendorServiceId, vendor_vendor_services.vendor_id AS VendorId, vendor_services.vendor_service_name AS VendorServiceNameValue, vendor_services.cost AS Cost, vendor_services.is_observed AS IsObserved, vendor_services.test_category_id AS TestCategoryId, vendor_services.form_type_id AS FormTypeId, vendor_services.is_active As IsActive, vendor_services.is_synchronized AS IsSynchronized, vendor_services.is_archived AS IsArchived, vendor_services.created_on AS CreatedOn, vendor_services.created_by AS CreatedBy, vendor_services.last_modified_on AS LastModifiedOn, vendor_services.last_modified_by AS LastModifiedBy "
                              + "FROM vendor_services "
                              + "INNER JOIN vendor_vendor_services ON vendor_services.vendor_service_id = vendor_vendor_services.vendor_service_id "
                              + "WHERE is_archived = 0 AND vendor_vendor_services.vendor_id = @VendorId AND UPPER(vendor_services.vendor_service_name) = UPPER(@VendorServiceName)";

            MySqlParameter[] param = new MySqlParameter[2];
            param[0] = new MySqlParameter("@VendorId", vendorId);
            param[1] = new MySqlParameter("@VendorServiceName", vendorServiceName);

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

        public DataTable GetList(int vendorId)
        {
            string sqlQuery = "SELECT vendor_services.vendor_service_id AS VendorServiceId, vendor_vendor_services.vendor_id AS VendorId, vendor_services.vendor_service_name AS VendorServiceNameValue, vendor_services.cost AS Cost,vendor_services.is_observed AS IsObserved, vendor_services.test_category_id AS TestCategoryId, vendor_services.form_type_id AS FormTypeId, vendor_services.is_active As IsActive, vendor_services.is_synchronized AS IsSynchronized, vendor_services.is_archived AS IsArchived, vendor_services.created_on AS CreatedOn, vendor_services.created_by AS CreatedBy, vendor_services.last_modified_on AS LastModifiedOn, vendor_services.last_modified_by AS LastModifiedBy "
                              + "FROM vendor_services "
                              + "INNER JOIN vendor_vendor_services ON vendor_services.vendor_service_id = vendor_vendor_services.vendor_service_id "
                              + "WHERE is_archived = 0 AND is_active = b'1' AND vendor_vendor_services.vendor_id = @VendorId ORDER BY vendor_services.vendor_service_name";

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

        public DataTable GetList(int vendorId, Dictionary<string, string> searchParam)
        {
            string sqlQuery = "SELECT vendor_services.vendor_service_id AS VendorServiceId, vendor_vendor_services.vendor_id AS VendorId, vendor_services.vendor_service_name AS VendorServiceNameValue, vendor_services.cost AS Cost, vendor_services.is_observed AS IsObserved, vendor_services.test_category_id AS TestCategoryId, vendor_services.form_type_id AS FormTypeId, vendor_services.is_active As IsActive, vendor_services.is_synchronized AS IsSynchronized, vendor_services.is_archived AS IsArchived, vendor_services.created_on AS CreatedOn, vendor_services.created_by AS CreatedBy, vendor_services.last_modified_on AS LastModifiedOn, vendor_services.last_modified_by AS LastModifiedBy "
                              + "FROM vendor_services "
                              + "INNER JOIN vendor_vendor_services ON vendor_services.vendor_service_id = vendor_vendor_services.vendor_service_id "
                              + "WHERE is_archived = 0 AND vendor_vendor_services.vendor_id = '" + vendorId + "' ";

            //MySqlParameter[] param = new MySqlParameter[2];
            //param[0] = new MySqlParameter("@VendorId", vendorId);
            //param[1] = new MySqlParameter("@SearchKeyword", searchParam["GeneralSearch"].ToString());
            //DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sqlQuery, param);

            List<MySqlParameter> paramList = new List<MySqlParameter>();
            bool isInActiveFlag = false;

            foreach (KeyValuePair<string, string> searchItem in searchParam)
            {
                if (searchItem.Key == "GeneralSearch")
                {
                    sqlQuery += "AND (vendor_services.vendor_service_name LIKE @SearchKeyword OR vendor_services.cost LIKE @SearchKeyword OR vendor_services.is_observed LIKE @SearchKeyword OR vendor_services.test_category_id LIKE @SearchKeyword OR vendor_services.form_type_id LIKE @SearchKeyword) ";

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
                sqlQuery += "AND vendor_services.is_active = b'1' ";
            }

            sqlQuery += "ORDER BY vendor_services.vendor_service_name";

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

        public DataTable sorting(int vendorId, string searchparam, bool active, string getInActive = null)
        {
            string sql = string.Empty;
            string sqlQuery = "SELECT vendor_services.vendor_service_id AS VendorServiceId, vendor_vendor_services.vendor_id AS VendorId, vendor_services.vendor_service_name AS VendorServiceNameValue, vendor_services.cost AS Cost, vendor_services.is_observed AS IsObserved, vendor_services.test_category_id AS TestCategoryId, vendor_services.form_type_id AS FormTypeId, vendor_services.is_active As IsActive, vendor_services.is_synchronized AS IsSynchronized, vendor_services.is_archived AS IsArchived, vendor_services.created_on AS CreatedOn, vendor_services.created_by AS CreatedBy, vendor_services.last_modified_on AS LastModifiedOn, vendor_services.last_modified_by AS LastModifiedBy "
                               + "FROM vendor_services "
                               + "INNER JOIN vendor_vendor_services ON vendor_services.vendor_service_id = vendor_vendor_services.vendor_service_id "
                               + "WHERE is_archived = 0 AND vendor_vendor_services.vendor_id = '" + vendorId + "'  ";

            if (active == false)
            {
                sqlQuery += "AND vendor_services.is_active = b'1'";
            }

            if (searchparam == "vendorServiceName")
            {
                if (getInActive == "1")
                {
                    sql = "ORDER BY vendor_services.vendor_service_name ";
                }
                else
                {
                    sql = "ORDER BY vendor_services.vendor_service_name desc";
                }
            }
            if (searchparam == "testCategory")
            {
                if (getInActive == "1")
                {
                    sql = "ORDER BY vendor_services.test_category_id ";
                }
                else
                {
                    sql = "ORDER BY vendor_services.test_category_id desc";
                }
            }
            if (searchparam == "isObserved")
            {
                if (getInActive == "1")
                {
                    sql = "ORDER BY vendor_services.is_observed ";
                }
                else
                {
                    sql = "ORDER BY vendor_services.is_observed desc";
                }
            }
            if (searchparam == "formType")
            {
                if (getInActive == "1")
                {
                    sql = "ORDER BY vendor_services.form_type_id ";
                }
                else
                {
                    sql = "ORDER BY vendor_services.form_type_id desc";
                }
            }
            if (searchparam == "isActive")
            {
                if (getInActive == "1")
                {
                    sql = "ORDER BY vendor_services.is_active";
                }
                else
                {
                    sql = "ORDER BY vendor_services.is_active desc";
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