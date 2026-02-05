using MySql.Data.MySqlClient;
using SurPath.Entity.Master;
using SurPath.Enum;
using SurPath.MySQLHelper;
using System;
using System.Collections.Generic;
using System.Data;

namespace SurPath.Data.Master
{
    /// <summary>
    /// Drug Name related data access process.
    /// </summary>
    public class DrugNameDao : DataObject
    {
        #region Constructor

        public DrugNameDao()
        {
            //
        }

        #endregion Constructor

        #region Public Methods

        /// <summary>
        /// Inserts the Drug Name information to the database.
        /// </summary>
        /// <param name="drugName">Drug Name information which one need to be added to the database.</param>
        /// <returns>Returns DrugNameId, the auto increament value.</returns>
        public int Insert(DrugName drugName)
        {
            int returnValue = 0;

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();

                try
                {
                    string sqlQuery = "INSERT INTO drug_names (drug_name,drug_code,ua_screen_value, ua_confirmation_value,hair_screen_value,hair_confirmation_value,ua_unit_of_measurement,hair_unit_of_measurement,is_active, is_synchronized, is_archived, created_on, created_by, last_modified_on, last_modified_by, isBC, isDNA) VALUES (";
                    sqlQuery += "@DrugNameValue,@DrugCodeValue, @UAScreenValue, @UAConfirmationValue,@HairScreenValue,@HairConfirmationValue,@UAUnitOfMeasurement,@HairUnitOfMeasurement, @IsActive, b'0', b'0', NOW(), @CreatedBy, NOW(), @LastModifiedBy, @isBC, isDNA)";

                    MySqlParameter[] param = new MySqlParameter[13];
                    param[0] = new MySqlParameter("@DrugNameValue", drugName.DrugNameValue);
                    param[1] = new MySqlParameter("@DrugCodeValue", drugName.DrugCodeValue);
                    param[2] = new MySqlParameter("@UAScreenValue", drugName.UAScreenValue);
                    param[3] = new MySqlParameter("@UAConfirmationValue", drugName.UAConfirmationValue);
                    param[4] = new MySqlParameter("@HairScreenValue", drugName.HairScreenValue);
                    param[5] = new MySqlParameter("@HairConfirmationValue", drugName.HairConfirmationValue);
                    param[6] = new MySqlParameter("@UAUnitOfMeasurement", drugName.UAUnitOfMeasurement);
                    param[7] = new MySqlParameter("@HairUnitOfMeasurement", drugName.HairUnitOfMeasurement);
                    param[8] = new MySqlParameter("@IsActive", drugName.IsActive);
                    param[9] = new MySqlParameter("@CreatedBy", drugName.CreatedBy);
                    param[10] = new MySqlParameter("@LastModifiedBy", drugName.LastModifiedBy);
                    param[11] = new MySqlParameter("@isBC", drugName.IsBC);
                    param[12] = new MySqlParameter("@isDNA", drugName.IsDNA);

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                    sqlQuery = "SELECT LAST_INSERT_ID()";

                    returnValue = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlQuery));

                    drugName.DrugNameId = returnValue;

                    sqlQuery = "INSERT INTO drug_names_categories (drug_name_id, test_category_id, is_synchronized, created_on, created_by, last_modified_on, last_modified_by) VALUES (@DrugNameId, @TestCategoryId, b'0', NOW(), @CreatedBy, NOW(), @LastModifiedBy)";

                    if (drugName.IsUA)
                    {
                        param = new MySqlParameter[4];
                        param[0] = new MySqlParameter("@DrugNameId", drugName.DrugNameId);
                        param[1] = new MySqlParameter("@TestCategoryId", (int)TestCategories.UA);
                        param[2] = new MySqlParameter("@CreatedBy", drugName.CreatedBy);
                        param[3] = new MySqlParameter("@LastModifiedBy", drugName.LastModifiedBy);

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);
                    }

                    if (drugName.IsHair)
                    {
                        param = new MySqlParameter[4];
                        param[0] = new MySqlParameter("@DrugNameId", drugName.DrugNameId);
                        param[1] = new MySqlParameter("@TestCategoryId", (int)TestCategories.Hair);
                        param[2] = new MySqlParameter("@CreatedBy", drugName.CreatedBy);
                        param[3] = new MySqlParameter("@LastModifiedBy", drugName.LastModifiedBy);

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);
                    }

                    if (drugName.IsBC)
                    {
                        param = new MySqlParameter[4];
                        param[0] = new MySqlParameter("@DrugNameId", drugName.DrugNameId);
                        param[1] = new MySqlParameter("@TestCategoryId", (int)TestCategories.BC);
                        param[2] = new MySqlParameter("@CreatedBy", drugName.CreatedBy);
                        param[3] = new MySqlParameter("@LastModifiedBy", drugName.LastModifiedBy);

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);
                    }
                    if (drugName.IsDNA)
                    {
                        param = new MySqlParameter[4];
                        param[0] = new MySqlParameter("@DrugNameId", drugName.DrugNameId);
                        param[1] = new MySqlParameter("@TestCategoryId", (int)TestCategories.DNA);
                        param[2] = new MySqlParameter("@CreatedBy", drugName.CreatedBy);
                        param[3] = new MySqlParameter("@LastModifiedBy", drugName.LastModifiedBy);

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);
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

        /// <summary>
        /// Updates the Drug Name information to the database.
        /// </summary>
        /// <param name="drugName">Drug Name information which one need to be updated to the database.</param>
        /// <returns>Returns number of records affected in the database.</returns>
        public int Update(DrugName drugName)
        {
            int returnValue = 0;

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();

                try
                {
                    string sqlQuery = "UPDATE drug_names SET "
                                    + "drug_name=@DrugNameValue, "
                                    + "drug_code=@DrugCodeValue,"
                                    + "ua_screen_value= @UAScreenValue, "
                                    + "ua_confirmation_value=@UAConfirmationValue, "
                                    + "hair_screen_value= @HairScreenValue, "
                                    + "hair_confirmation_value=@HairConfirmationValue, "
                                    + "ua_unit_of_measurement = @UAUnitOfMeasurement, "
                                    + "hair_unit_of_measurement = @HairUnitOfMeasurement, "
                                    + "is_active = @IsActive, "
                                    + "is_synchronized = b'0', "
                                    + "last_modified_on = NOW(), "
                                    + "last_modified_by = @LastModifiedBy "
                                    + "WHERE drug_name_id = @DrugNameId";

                    MySqlParameter[] param = new MySqlParameter[11];
                    param[0] = new MySqlParameter("@DrugNameId", drugName.DrugNameId);
                    param[1] = new MySqlParameter("@DrugNameValue", drugName.DrugNameValue);
                    param[2] = new MySqlParameter("@DrugCodeValue", drugName.DrugCodeValue);
                    param[3] = new MySqlParameter("@UAScreenValue", drugName.UAScreenValue);
                    param[4] = new MySqlParameter("@UAConfirmationValue", drugName.UAConfirmationValue);
                    param[5] = new MySqlParameter("@HairScreenValue", drugName.HairScreenValue);
                    param[6] = new MySqlParameter("@HairConfirmationValue", drugName.HairConfirmationValue);
                    param[7] = new MySqlParameter("@UAUnitOfMeasurement", drugName.UAUnitOfMeasurement);
                    param[8] = new MySqlParameter("@HairUnitOfMeasurement", drugName.HairUnitOfMeasurement);
                    param[9] = new MySqlParameter("@IsActive", drugName.IsActive);
                    param[10] = new MySqlParameter("@LastModifiedBy", drugName.LastModifiedBy);

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                    sqlQuery = "INSERT INTO drug_names_categories (drug_name_id, test_category_id, is_synchronized, created_on, created_by, last_modified_on, last_modified_by) "
                                + "SELECT @DrugNameId, @TestCategoryId, b'0', NOW(), @CreatedBy, NOW(), @LastModifiedBy FROM DUAL "
                                + "WHERE NOT EXISTS (SELECT drug_name_categories_id FROM drug_names_categories WHERE drug_name_id = @DrugNameId AND test_category_id = @TestCategoryId)";

                    string sqlDelQuery = "DELETE FROM drug_names_categories WHERE drug_name_id = @DrugNameId AND test_category_id = @TestCategoryId";

                    if (drugName.IsUA)
                    {
                        param = new MySqlParameter[4];
                        param[0] = new MySqlParameter("@DrugNameId", drugName.DrugNameId);
                        param[1] = new MySqlParameter("@TestCategoryId", (int)TestCategories.UA);
                        param[2] = new MySqlParameter("@CreatedBy", drugName.CreatedBy);
                        param[3] = new MySqlParameter("@LastModifiedBy", drugName.LastModifiedBy);

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);
                    }
                    else
                    {
                        param = new MySqlParameter[2];
                        param[0] = new MySqlParameter("@DrugNameId", drugName.DrugNameId);
                        param[1] = new MySqlParameter("@TestCategoryId", (int)TestCategories.UA);

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlDelQuery, param);
                    }

                    if (drugName.IsHair)
                    {
                        param = new MySqlParameter[4];
                        param[0] = new MySqlParameter("@DrugNameId", drugName.DrugNameId);
                        param[1] = new MySqlParameter("@TestCategoryId", (int)TestCategories.Hair);
                        param[2] = new MySqlParameter("@CreatedBy", drugName.CreatedBy);
                        param[3] = new MySqlParameter("@LastModifiedBy", drugName.LastModifiedBy);

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);
                    }
                    else
                    {
                        param = new MySqlParameter[2];
                        param[0] = new MySqlParameter("@DrugNameId", drugName.DrugNameId);
                        param[1] = new MySqlParameter("@TestCategoryId", (int)TestCategories.Hair);

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlDelQuery, param);
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

        /// <summary>
        /// Deletes the Drug Name information from database.
        /// </summary>
        /// <param name="drugNameId">Drug Name Id which one will be deleted.</param>
        /// <param name="currentUsername">Current username who is deleting the record.</param>
        /// <returns>Returns number of records deleted from the database.</returns>
        public int Delete(int drugNameId, string currentUsername)
        {
            int returnValue = 0;

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();

                //string sqlCount1Query = "Select count(*) from drug_names_categories where drug_name_id = " + drugNameId + "";

                //int table1 = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlCount1Query));

                //if (table1 <= 0)
                //{
                string sqlCount2Query = "Select count(*) from test_panel_drug_names where drug_name_id = " + drugNameId + "";

                int table2 = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlCount2Query));
                if (table2 <= 0)
                {
                    try
                    {
                        string sqlQuery = "UPDATE drug_names SET is_synchronized = b'0', is_archived = b'1', last_modified_on = NOW(), last_modified_by = @LastModifiedBy WHERE drug_name_id = @DrugNameId";

                        MySqlParameter[] param = new MySqlParameter[2];
                        param[0] = new MySqlParameter("@DrugNameId", drugNameId);
                        param[1] = new MySqlParameter("@LastModifiedBy", currentUsername);

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
                //}
            }

            return returnValue;
        }

        /// <summary>
        /// Get the Drug Name information by DrugNameId
        /// </summary>
        /// <param name="drugNameId">DrugNameId which one need to be get from the database.</param>
        /// <returns>Returns Drug Name information.</returns>
        public DrugName Get(int drugNameId)
        {
            DrugName drugName = null;
            string sqlQuery = "SELECT drug_name_id AS DrugNameId, drug_name AS DrugNameValue,drug_code As DrugCodeValue,ua_screen_value AS UAScreenValue, ua_confirmation_value AS UAConfirmationValue,hair_screen_value AS HairScreenValue, hair_confirmation_value AS HairConfirmationValue,ua_unit_of_measurement AS UAUnitOfMeasurement, hair_unit_of_measurement AS HairUnitOfMeasurement, is_active AS IsActive, is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM drug_names WHERE drug_name_id = @DrugNameId";

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                MySqlParameter[] param = new MySqlParameter[1];
                param[0] = new MySqlParameter("@DrugNameId", drugNameId);

                MySqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                if (dr.Read())
                {
                    drugName = new DrugName();
                    drugName.DrugNameId = (int)dr["DrugNameId"];
                    drugName.DrugNameValue = (string)dr["DrugNameValue"];
                    drugName.DrugCodeValue = (string)dr["DrugCodeValue"];
                    drugName.UAScreenValue = dr["UAScreenValue"].ToString();
                    drugName.UAConfirmationValue = dr["UAConfirmationValue"].ToString();
                    drugName.HairScreenValue = dr["HairScreenValue"].ToString();
                    drugName.HairConfirmationValue = dr["HairConfirmationValue"].ToString();
                    drugName.UAUnitOfMeasurement = dr["UAUnitOfMeasurement"].ToString();
                    drugName.HairUnitOfMeasurement = dr["HairUnitOfMeasurement"].ToString();
                    drugName.IsActive = dr["IsActive"].ToString() == "1" ? true : false;
                    drugName.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    drugName.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    drugName.CreatedOn = (DateTime)dr["CreatedOn"];
                    drugName.CreatedBy = (string)dr["CreatedBy"];
                    drugName.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    drugName.LastModifiedBy = (string)dr["LastModifiedBy"];
                }
                dr.Close();

                if (drugName != null)
                {
                    sqlQuery = "SELECT drug_name_categories_id AS DrugNameCategoriesId, drug_name_id AS DrugNameId, test_category_id AS TestCategoryId, is_synchronized AS IsSynchronized, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM drug_names_categories WHERE drug_name_id = @DrugNameId ORDER BY test_category_id";

                    param = new MySqlParameter[1];
                    param[0] = new MySqlParameter("@DrugNameId", drugNameId);

                    dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);
                    while (dr.Read())
                    {
                        if (dr["TestCategoryId"].ToString() == ((int)TestCategories.UA).ToString())
                        {
                            drugName.IsUA = true;
                        }
                        else if (dr["TestCategoryId"].ToString() == ((int)TestCategories.Hair).ToString())
                        {
                            drugName.IsHair = true;
                        }
                    }
                    dr.Close();
                }
            }

            return drugName;
        }

        /// <summary>
        /// Get the Drug Name information by DrugName
        /// </summary>
        /// <param name="drugName">DrugName which one need to be get from the database.</param>
        /// <returns>Returns Drug Name information.</returns>
        public DataTable GetByDrugName(string drugName)
        {
            string sqlQuery = "SELECT drug_name_id AS DrugNameId, drug_name AS DrugNameValue, drug_code As DrugCodeValue,ua_screen_value AS UAScreenValue, ua_confirmation_value AS UAConfirmationValue,hair_screen_value AS HairScreenValue, hair_confirmation_value AS HairConfirmationValue, ua_unit_of_measurement AS UAUnitOfMeasurement, hair_unit_of_measurement AS HairUnitOfMeasurement, is_active AS IsActive, is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM drug_names WHERE is_archived = 0 AND Upper(drug_name) = UPPER(@DrugNameValue)";

            MySqlParameter[] param = new MySqlParameter[1];
            param[0] = new MySqlParameter("@DrugNameValue", drugName);

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

        /// <summary>
        /// Get all the Drug Name informations.
        /// </summary>
        /// <returns>Returns Drug Name information list.</returns>
        public DataTable GetList()
        {
            string sqlQuery = "SELECT drug_name_id AS DrugNameId, drug_name AS DrugNameValue,drug_code As DrugCodeValue,ua_screen_value AS UAScreenValue, ua_confirmation_value AS UAConfirmationValue,hair_screen_value AS HairScreenValue, hair_confirmation_value AS HairConfirmationValue, ua_unit_of_measurement AS UAUnitOfMeasurement, hair_unit_of_measurement AS HairUnitOfMeasurement, is_active AS IsActive, is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM drug_names WHERE is_archived = 0 AND is_active = b'1' ORDER BY drug_name";

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

        /// <summary>
        /// Get all the Drug Name informations based on the Test Category.
        /// </summary>
        /// <param name="testCategory">Test Category</param>
        /// <returns>Returns Drug Name information list.</returns>
        public DataTable GetList(TestCategories testCategory)
        {
            string sqlQuery = "SELECT drug_names.drug_name_id AS DrugNameId, drug_names.drug_name AS DrugNameValue,drug_names.drug_code As DrugCodeValue,drug_names.ua_screen_value AS UAScreenValue,drug_names.ua_confirmation_value AS UAConfirmationValue,drug_names.hair_screen_value AS HairScreenValue, drug_names.hair_confirmation_value AS HairConfirmationValue, drug_names.ua_unit_of_measurement AS UAUnitOfMeasurement, drug_names.hair_unit_of_measurement AS HairUnitOfMeasurement, drug_names.is_active AS IsActive, drug_names.is_synchronized AS IsSynchronized, drug_names.is_archived AS IsArchived, drug_names.created_on AS CreatedOn, drug_names.created_by AS CreatedBy, drug_names.last_modified_on AS LastModifiedOn, drug_names.last_modified_by AS LastModifiedBy "
                            + "FROM drug_names INNER JOIN drug_names_categories ON drug_names.drug_name_id = drug_names_categories.drug_name_id "
                            + "WHERE drug_names.is_archived = 0 AND drug_names.is_active = b'1' AND drug_names_categories.test_category_id = @TestCategoryId ORDER BY drug_names.drug_name";

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

        /// <summary>
        /// Get all the Drug Name informations based on the search criteria.
        /// </summary>
        /// <param name="searchParam">Collection of search criteria</param>
        /// <returns>Returns Drug Name information list.</returns>
        public DataTable GetList(Dictionary<string, string> searchParam)
        {
            string sqlQuery = "SELECT drug_name_id AS DrugNameId,drug_name AS DrugNameValue,drug_code As DrugCodeValue,ua_screen_value AS UAScreenValue, ua_confirmation_value AS UAConfirmationValue,hair_screen_value AS HairScreenValue, hair_confirmation_value AS HairConfirmationValue, ua_unit_of_measurement AS UAUnitOfMeasurement, hair_unit_of_measurement AS HairUnitOfMeasurement,  is_active AS IsActive, is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM drug_names WHERE is_archived = 0";

            List<MySqlParameter> param = new List<MySqlParameter>();
            bool isInActiveFlag = false;

            foreach (KeyValuePair<string, string> searchItem in searchParam)
            {
                if (searchItem.Key == "GeneralSearch")
                {
                    sqlQuery += "  AND (drug_name LIKE @SearchKeyword OR drug_code LIKE @SearchKeyword OR ua_screen_value LIKE @SearchKeyword OR ua_confirmation_value LIKE @SearchKeyword OR hair_screen_value LIKE @SearchKeyword OR hair_confirmation_value LIKE @SearchKeyword)";

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

            sqlQuery += " ORDER BY drug_name";

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

        /// <summary>
        /// Get all the Drug Name - Test Categories informations based on the drug name id.
        /// </summary>
        /// <param name="drugNameId">Drug name id</param>
        /// <returns>Returns Drug Name - Test Categories information list.</returns>
        public DataTable GetDrugNameTestCategories(int drugNameId)
        {
            string sqlQuery = "SELECT drug_name_categories_id AS DrugNameCategoriesId, drug_name_id AS DrugNameId, test_category_id AS TestCategoryId, is_synchronized AS IsSynchronized, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM drug_names_categories WHERE drug_name_id = @DrugNameId ORDER BY test_category_id";

            MySqlParameter[] param = new MySqlParameter[1];
            param[0] = new MySqlParameter("@DrugNameId", drugNameId);

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

            //  string sqlQuery = "SELECT drug_name_id AS DrugNameId,drug_name AS DrugNameValue,drug_code As DrugCodeValue,ua_screen_value AS UAScreenValue, ua_confirmation_value AS UAConfirmationValue,hair_screen_value AS HairScreenValue, hair_confirmation_value AS HairConfirmationValue, ua_unit_of_measurement AS UAUnitOfMeasurement, hair_unit_of_measurement AS HairUnitOfMeasurement,  is_active AS IsActive, is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM drug_names WHERE is_archived = 0 ";

            string sqlQuery = "SELECT drug_names.drug_name_id AS DrugNameId,drug_name AS DrugNameValue,drug_code As DrugCodeValue,drug_names_categories.test_category_id AS TestCategoryId,ua_screen_value AS UAScreenValue, "
                              + "ua_confirmation_value AS UAConfirmationValue,hair_screen_value AS HairScreenValue, hair_confirmation_value AS HairConfirmationValue, "
                              + "ua_unit_of_measurement AS UAUnitOfMeasurement, hair_unit_of_measurement AS HairUnitOfMeasurement,  is_active AS IsActive, "
                              + "drug_names.is_synchronized AS IsSynchronized, is_archived AS IsArchived, drug_names.created_on AS CreatedOn, drug_names.created_by AS CreatedBy,"
                              + "drug_names.last_modified_on AS LastModifiedOn, drug_names.last_modified_by AS LastModifiedBy FROM drug_names "
                              + "inner JOIN drug_names_categories ON drug_names_categories.drug_name_id = drug_names.drug_name_id "
                              + "WHERE is_archived = 0 ";
            if (active == false)
            {
                sqlQuery += "AND is_active = b'1'";
            }
            sqlQuery += "group by drug_names.drug_name_id ";

            if (searchparam == "drugName")
            {
                if (getInActive == "1")
                {
                    sql = "ORDER BY DrugNameValue";
                }
                else
                {
                    sql = "ORDER BY DrugNameValue desc";
                }
            }
            //if (searchparam == "isUA")
            //{
            //    if (getInActive == "1")
            //    {
            //        sql = "ORDER BY drug_names_categories.test_category_id ='1' ";
            //    }
            //    else
            //    {
            //        sql = "ORDER BY drug_names_categories.test_category_id ='1' desc";
            //    }
            //}
            //if (searchparam == "isHair")
            //{
            //    if (getInActive == "1")
            //    {
            //        sql = "ORDER BY drug_names_categories.test_category_id ='2' ";
            //    }
            //    else
            //    {
            //        sql = "ORDER BY drug_names_categories.test_category_id ='2' desc";
            //    }
            //}
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