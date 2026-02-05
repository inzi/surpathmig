using MySql.Data.MySqlClient;
using Serilog;
using SurPath.Entity;
using SurPath.Enum;
using SurPath.MySQLHelper;
using System;
using System.Collections.Generic;
using System.Data;

namespace SurPath.Data
{
    public class ClientDao : DataObject
    {
        #region Constructor
        public static ILogger _logger;

        public ClientDao(ILogger __logger = null)
        {
            if (!(__logger == null)) _logger = __logger;
        }

        #endregion Constructor

        #region Public Methods

        /// <summary>
        /// Inserts the Client information to the database.
        /// </summary>
        /// <param name="client">Client information which one need to be added to the database.</param>
        /// <returns>Returns ClientId, the auto increament value.</returns>
        public int Insert(Client client)
        {
            int returnValue = 0;

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();

                try
                {
                    string sqlQuery = "INSERT INTO clients ("
                                        + "client_name, "
                                        + "client_division, "
                                        + "client_type_id, "
                                        + "laboratory_vendor_id, "
                                        + "mro_vendor_id, "
                                        + "mro_type_id, "
                                        + "is_client_active, "
                                        + "can_edit_test_category, "
                                        + "client_code, "
                                        + "is_mailing_address_physical, "
                                        + "sales_representative_id, "
                                        + "sales_comissions, "
                                        + "is_synchronized, "
                                        + "is_archived, "
                                        + "created_on, "
                                        + "created_by, "
                                        + "last_modified_on, "
                                        + "last_modified_by, "
                                        + "client_timezoneinfo"

                                        + ") VALUES ("
                                        + "@ClientName, "
                                        + "@ClientDivision, "
                                        + "@ClientTypeId, "
                                        + "@LaboratoryVendorId, "
                                        + "@MROVendorId, "
                                        + "@MROTypeId, "
                                        + "@IsClientActive, "
                                        + "@CanEditTestCategory, "
                                        + "@ClientCode, "
                                        + "@IsMailingAddressPhysical, "
                                        + "@SalesRepresentativeId, "
                                        + "@SalesComissions, "
                                        + "b'0', "
                                        + "b'0', "
                                        + "NOW(), "
                                        + "@CreatedBy, "
                                        + "NOW(), "
                                        + "@LastModifiedBy, "
                                        + "@client_timezoneinfo)";

                    //MySqlParameter[] param = new MySqlParameter[14];

                    ParamHelper param = new ParamHelper();

                    param.Param = new MySqlParameter("@ClientName", client.ClientName);
                    param.Param= new MySqlParameter("@ClientDivision", client.ClientDivision);
                    param.Param= new MySqlParameter("@ClientTypeId", (int)client.ClientTypeId);
                    param.Param= new MySqlParameter("@LaboratoryVendorId", client.LaboratoryVendorId);
                    param.Param= new MySqlParameter("@MROVendorId", client.MROVendorId);
                    param.Param= new MySqlParameter("@MROTypeId", (int)client.MROTypeId);
                    param.Param= new MySqlParameter("@IsClientActive", client.IsClientActive);
                    param.Param= new MySqlParameter("@CanEditTestCategory", client.CanEditTestCategory);
                    param.Param= new MySqlParameter("@ClientCode", client.ClientCode);
                    param.Param= new MySqlParameter("@IsMailingAddressPhysical", client.IsMailingAddressPhysical);
                    param.Param = new MySqlParameter("@SalesRepresentativeId", client.SalesRepresentativeId);
                    param.Param = new MySqlParameter("@SalesComissions", client.SalesComissions);
                    param.Param = new MySqlParameter("@CreatedBy", client.CreatedBy);
                    param.Param = new MySqlParameter("@LastModifiedBy", client.LastModifiedBy);
                    param.Param = new MySqlParameter("@client_timezoneinfo", client.client_timezoneinfo);

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param.Params);

                    sqlQuery = "SELECT LAST_INSERT_ID()";

                    returnValue = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlQuery));

                    client.ClientId = returnValue;

                    sqlQuery = "INSERT INTO client_contacts ("
                                + "client_id,"
                                + "client_contact_first_name, "
                                + "client_contact_last_name, "
                                + "client_contact_phone, "
                                + "client_contact_fax, "
                                + "client_contact_email, "
                                + "is_synchronized, "
                                + "created_on, "
                                + "created_by, "
                                + "last_modified_on, "
                                + "last_modified_by"
                                + ") VALUES ("
                                + "@ClientId,"
                                + "@ClientContactFirstName, "
                                + "@ClientContactLastName, "
                                + "@ClientContactPhone, "
                                + "@ClientContactFax, "
                                + "@ClientContactEmail, "
                                + "b'0', "
                                + "NOW(), "
                                + "@CreatedBy, "
                                + "NOW(), "
                                + "@LastModifiedBy)";

                    if (client.ClientContact != null)
                    {
                        param.reset();

                        param.Param = new MySqlParameter("@ClientId", client.ClientId);
                        param.Param = new MySqlParameter("@ClientContactFirstName", client.ClientContact.ClientContactFirstName);
                        param.Param = new MySqlParameter("@ClientContactLastName", client.ClientContact.ClientContactLastName);
                        param.Param = new MySqlParameter("@ClientContactPhone", client.ClientContact.ClientContactPhone);
                        param.Param = new MySqlParameter("@ClientContactFax", client.ClientContact.ClientContactFax);
                        param.Param = new MySqlParameter("@ClientContactEmail", client.ClientContact.ClientContactEmail);
                        param.Param = new MySqlParameter("@CreatedBy", client.CreatedBy);
                        param.Param = new MySqlParameter("@LastModifiedBy", client.LastModifiedBy);

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param.Params);
                    }

                    sqlQuery = "INSERT INTO client_addresses ("
                                + "client_id,"
                                + "address_type_id, "
                                + "client_address_1, "
                                + "client_address_2, "
                                + "client_city, "
                                + "client_state, "
                                + "client_zip, "
                                + "is_synchronized, "
                                + "created_on, "
                                + "created_by, "
                                + "last_modified_on, "
                                + "last_modified_by"
                                + ") VALUES ("
                                + "@ClientId, "
                                + "@AddressTypeId, "
                                + "@Address1, "
                                + "@Address2, "
                                + "@City, "
                                + "@State, "
                                + "@ZipCode, "
                                + "b'0', "
                                + "NOW(), "
                                + "@CreatedBy, "
                                + "NOW(), "
                                + "@LastModifiedBy)";

                    if (client.ClientAddresses.Count > 0)
                    {
                        foreach (ClientAddress address in client.ClientAddresses)
                        {
                            param.reset();

                            param.Param = new MySqlParameter("@ClientId", client.ClientId);
                            param.Param = new MySqlParameter("@AddressTypeId", (int)address.AddressTypeId);
                            param.Param = new MySqlParameter("@Address1", address.Address1);
                            param.Param = new MySqlParameter("@Address2", address.Address2);
                            param.Param = new MySqlParameter("@City", address.City);
                            param.Param = new MySqlParameter("@State", address.State);
                            param.Param = new MySqlParameter("@ZipCode", address.ZipCode);
                            param.Param = new MySqlParameter("@CreatedBy", client.CreatedBy);
                            param.Param = new MySqlParameter("@LastModifiedBy", client.LastModifiedBy);

                            SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param.Params);
                        }
                    }

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

        /// <summary>
        /// Updates the Client information to the database.
        /// </summary>
        /// <param name="client">Client information which one need to be updated to the database.</param>
        /// <returns>Returns number of records affected in the database.</returns>
        public int Update(Client client)
        {
            int returnValue = 0;

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();

                try
                {
                    string sqlQuery = "UPDATE clients SET "
                                        + "client_name = @ClientName, "
                                        + "client_division = @ClientDivision, "
                                        + "client_type_id = @ClientTypeId, "
                                        + "laboratory_vendor_id = @LaboratoryVendorId, "
                                        + "mro_vendor_id = @MROVendorId, "
                                        + "mro_type_id = @MROTypeId, "
                                        + "is_mailing_address_physical = @IsMailingAddressPhysical, "
                                        + "is_client_active = @IsClientActive, "
                                        + "can_edit_test_category = @CanEditTestCategory, "
                                        + "sales_representative_id = @SalesRepresentativeId, "
                                        + "sales_comissions = @SalesComissions, "
                                        + "client_code = @ClientCode, "
                                        + "client_timezoneinfo = @client_timezoneinfo, "
                                        + "is_synchronized = b'0', "
                                        + "last_modified_on = NOW(), "
                                        + "last_modified_by = @LastModifiedBy "
                                        + "WHERE client_id = @ClientId";

                    ParamHelper param = new ParamHelper();
                    param.Param = new MySqlParameter("@ClientId", client.ClientId);
                    param.Param= new MySqlParameter("@ClientName", client.ClientName);
                    param.Param= new MySqlParameter("@ClientDivision", client.ClientDivision);
                    param.Param= new MySqlParameter("@ClientTypeId", (int)client.ClientTypeId);
                    param.Param= new MySqlParameter("@LaboratoryVendorId", client.LaboratoryVendorId);
                    param.Param= new MySqlParameter("@MROVendorId", client.MROVendorId);
                    param.Param= new MySqlParameter("@MROTypeId", (int)client.MROTypeId);
                    param.Param= new MySqlParameter("@IsMailingAddressPhysical", client.IsMailingAddressPhysical);
                    param.Param= new MySqlParameter("@IsClientActive", client.IsClientActive);
                    param.Param= new MySqlParameter("@CanEditTestCategory", client.CanEditTestCategory);
                    param.Param = new MySqlParameter("@SalesRepresentativeId", client.SalesRepresentativeId);
                    param.Param = new MySqlParameter("@SalesComissions", client.SalesComissions);
                    param.Param = new MySqlParameter("@ClientCode", client.ClientCode);
                    param.Param = new MySqlParameter("@LastModifiedBy", client.LastModifiedBy);
                    param.Param = new MySqlParameter("@client_timezoneinfo", client.client_timezoneinfo);
                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param.Params);

                    sqlQuery = "UPDATE client_contacts SET "
                                        + "client_contact_first_name = @ClientContactFirstName, "
                                        + "client_contact_last_name = @ClientContactLastName, "
                                        + "client_contact_phone = @ClientContactPhone, "
                                        + "client_contact_fax = @ClientContactFax, "
                                        + "client_contact_email = @ClientContactEmail, "
                                        + "is_synchronized = b'0', "
                                        + "last_modified_on = NOW(), "
                                        + "last_modified_by = @LastModifiedBy "
                                        + "WHERE client_contact_id = @ClientContactId";

                    if (client.ClientContact != null)
                    {
                        param.reset();
                        
                        param.Param  = new MySqlParameter("@ClientContactId", client.ClientContact.ClientContactId);
                        param.Param  = new MySqlParameter("@ClientContactFirstName", client.ClientContact.ClientContactFirstName);
                        param.Param  = new MySqlParameter("@ClientContactLastName", client.ClientContact.ClientContactLastName);
                        param.Param  = new MySqlParameter("@ClientContactPhone", client.ClientContact.ClientContactPhone);
                        param.Param  = new MySqlParameter("@ClientContactFax", client.ClientContact.ClientContactFax);
                        param.Param  = new MySqlParameter("@ClientContactEmail", client.ClientContact.ClientContactEmail);
                        param.Param = new MySqlParameter("@LastModifiedBy", client.LastModifiedBy);

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param.Params);
                    }

                    if (client.ClientAddresses.Count > 0)
                    {
                        foreach (ClientAddress address in client.ClientAddresses)
                        {
                            if (address.AddressId == 0)
                            {
                                sqlQuery = "INSERT INTO client_addresses ("
                                        + "client_id, "
                                        + "address_type_id, "
                                        + "client_address_1, "
                                        + "client_address_2, "
                                        + "client_city, "
                                        + "client_state, "
                                        + "client_zip, "
                                        + "is_synchronized, "
                                        + "created_on, "
                                        + "created_by, "
                                        + "last_modified_on, "
                                        + "last_modified_by "
                                        + ") VALUES ("
                                        + "@ClientId, "
                                        + "@AddressTypeId, "
                                        + "@Address1, "
                                        + "@Address2, "
                                        + "@City, "
                                        + "@State, "
                                        + "@ZipCode, "
                                        + "b'0', "
                                        + "NOW(), "
                                        + "@CreatedBy, "
                                        + "NOW(), "
                                        + "@LastModifiedBy)";
                            }
                            else
                            {
                                sqlQuery = "UPDATE client_addresses SET "
                                        + "client_address_1 = @Address1, "
                                        + "client_address_2 = @Address2, "
                                        + "client_city = @City, "
                                        + "client_state = @State, "
                                        + "client_zip = @ZipCode, "
                                        + "is_synchronized = b'0', "
                                        + "last_modified_on = NOW(), "
                                        + "last_modified_by = @LastModifiedBy "
                                        + "WHERE client_address_id = @ClientAddressId";
                            }
                            //param = new MySqlParameter[10];
                            param.reset();

                            param.Param = new MySqlParameter("@ClientAddressId", address.AddressId);
                            param.Param = new MySqlParameter("@ClientId", client.ClientId);
                            param.Param = new MySqlParameter("@AddressTypeId", (int)address.AddressTypeId);
                            param.Param = new MySqlParameter("@Address1", address.Address1);
                            param.Param = new MySqlParameter("@Address2", address.Address2);
                            param.Param = new MySqlParameter("@City", address.City);
                            param.Param = new MySqlParameter("@State", address.State);
                            param.Param = new MySqlParameter("@ZipCode", address.ZipCode);
                            param.Param = new MySqlParameter("@CreatedBy", client.LastModifiedBy);
                            param.Param = new MySqlParameter("@LastModifiedBy", client.LastModifiedBy);

                            SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param.Params);
                        }
                    }

                    string sqlDeptTestCateUpdate = "UPDATE client_dept_test_categories SET "
                                                + "test_panel_price = @TestPanelPrice, "
                                                + "is_synchronized = b'0', "
                                                + "last_modified_on = NOW(), "
                                                + "last_modified_by = @LastModifiedBy "
                                                + "WHERE client_dept_test_category_id = @ClientDeptTestCategoryId";

                    string sqlTestPanelDelete = "DELETE FROM client_dept_test_panels WHERE client_dept_test_panel_id = @ClientDeptTestPanelId";

                    string sqlTestPanelInsert = "INSERT client_dept_test_panels ("
                                                    + "client_dept_test_category_id, "
                                                    + "test_panel_id, "
                                                    + "test_panel_price, "
                                                    + "display_order, "
                                                    + "is_main_test_panel, "
                                                    + "is_1_test_panel, "
                                                    + "is_2_test_panel, "
                                                    + "is_3_test_panel, "
                                                    + "is_4_test_panel, "
                                                    + "is_synchronized, "
                                                    + "created_on, "
                                                    + "created_by, "
                                                    + "last_modified_on, "
                                                    + "last_modified_by "
                                                    + ") VALUES ( "
                                                    + "@ClientDeptTestCategoryId, "
                                                    + "@TestPanelId, "
                                                    + "@TestPanelPrice, "
                                                    + "@DisplayOrder, "
                                                    + "@IsMainTestPanel, "
                                                    + "@Is1TestPanel, "
                                                    + "@Is2TestPanel, "
                                                    + "@Is3TestPanel, "
                                                    + "@Is4TestPanel, "
                                                    + "b'0', "
                                                    + "NOW(), "
                                                    + "@CreatedBy, "
                                                    + "NOW(), "
                                                    + "@LastModifiedBy "
                                                    + ")";

                    string sqlTestPanelUpdate = "UPDATE client_dept_test_panels SET "
                                                    + "test_panel_id = @TestPanelId, "
                                                    + "test_panel_price = @TestPanelPrice, "
                                                    + "is_synchronized = b'0', "
                                                    + "last_modified_on = NOW(), "
                                                    + "last_modified_by = @LastModifiedBy "
                                                    + "WHERE client_dept_test_panel_id = @ClientDeptTestPanelId";

                    if (client.ClientDepartments.Count > 0)
                    {
                        foreach (ClientDepartment dept in client.ClientDepartments)
                        {
                            foreach (ClientDeptTestCategory testCategory in dept.ClientDeptTestCategories)
                            {
                                if (testCategory.TestCategoryId == TestCategories.UA || testCategory.TestCategoryId == TestCategories.Hair || testCategory.TestCategoryId == TestCategories.BC || testCategory.TestCategoryId == TestCategories.DNA || testCategory.TestCategoryId == TestCategories.RC)
                                {
                                    foreach (ClientDeptTestPanel testpanel in testCategory.ClientDeptTestPanels)
                                    {
                                        if ((testpanel.ClientDeptTestPanelId == 0 || testpanel.ClientDeptTestPanelId == 1) && testpanel.TestPanelId > 0 && testpanel.TestPanelPrice > 0)
                                        {
                                            param.reset();

                                            param.Param = new MySqlParameter("@ClientDeptTestCategoryId", testpanel.ClientDeptTestCategoryId);
                                            param.Param = new MySqlParameter("@TestPanelId", testpanel.TestPanelId);
                                            param.Param= new MySqlParameter("@TestPanelPrice", testpanel.TestPanelPrice);
                                            param.Param= new MySqlParameter("@DisplayOrder", testpanel.DisplayOrder);
                                            param.Param= new MySqlParameter("@IsMainTestPanel", testpanel.IsMainTestPanel);
                                            param.Param= new MySqlParameter("@Is1TestPanel", testpanel.Is1TestPanel);
                                            param.Param= new MySqlParameter("@Is2TestPanel", testpanel.Is2TestPanel);
                                            param.Param= new MySqlParameter("@Is3TestPanel", testpanel.Is3TestPanel);
                                            param.Param= new MySqlParameter("@Is4TestPanel", testpanel.Is4TestPanel);
                                            param.Param= new MySqlParameter("@CreatedBy", testpanel.CreatedBy);
                                            param.Param = new MySqlParameter("@LastModifiedBy", testpanel.LastModifiedBy);

                                            SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlTestPanelInsert, param.Params);
                                        }
                                        else if (testpanel.ClientDeptTestPanelId != 0 && testpanel.TestPanelId == 0)
                                        {
                                            param.reset();

                                            param.Param = new MySqlParameter("@ClientDeptTestPanelId", testpanel.ClientDeptTestPanelId);

                                            SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlTestPanelDelete, param.Params);
                                        }
                                        else
                                        {
                                            param.reset();

                                            param.Param = new MySqlParameter("@ClientDeptTestPanelId", testpanel.ClientDeptTestPanelId);/*ClientDeptTestPanelId); ;*/
                                            param.Param = new MySqlParameter("@TestPanelId", testpanel.TestPanelId);
                                            param.Param = new MySqlParameter("@TestPanelPrice", testpanel.TestPanelPrice);
                                            param.Param = new MySqlParameter("@LastModifiedBy", testpanel.LastModifiedBy);

                                            SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlTestPanelUpdate, param.Params);
                                        }
                                    }
                                }
                                else if (testCategory.TestCategoryId == TestCategories.DNA)
                                {
                                    param.reset();

                                    param.Param = new MySqlParameter("@ClientDeptTestCategoryId", testCategory.ClientDeptTestCategoryId);
                                    param.Param = new MySqlParameter("@TestPanelPrice", testCategory.TestPanelPrice);
                                    param.Param = new MySqlParameter("@LastModifiedBy", client.LastModifiedBy);

                                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlDeptTestCateUpdate, param.Params);
                                }
                                else if (testCategory.TestCategoryId == TestCategories.BC)
                                {
                                    param.reset();

                                    param.Param = new MySqlParameter("@ClientDeptTestCategoryId", testCategory.ClientDeptTestCategoryId);
                                    param.Param = new MySqlParameter("@TestPanelPrice", testCategory.TestPanelPrice);
                                    param.Param = new MySqlParameter("@LastModifiedBy", client.LastModifiedBy);

                                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlDeptTestCateUpdate, param.Params);
                                }
                                else if (testCategory.TestCategoryId == TestCategories.RC)
                                {
                                    param.reset();

                                    param.Param = new MySqlParameter("@ClientDeptTestCategoryId", testCategory.ClientDeptTestCategoryId);
                                    param.Param = new MySqlParameter("@TestPanelPrice", testCategory.TestPanelPrice);
                                    param.Param = new MySqlParameter("@LastModifiedBy", client.LastModifiedBy);

                                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlDeptTestCateUpdate, param.Params);
                                }
                            }
                        }
                    }

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

        /// <summary>
        /// Deletes the Client information from database.
        /// </summary>
        /// <param name="clientId">Client Id which one will be deleted.</param>
        /// <param name="currentUsername">Current username who is deleting the record.</param>
        /// <returns>Returns number of records deleted from the database.</returns>
        public int Delete(int clientId, string currentUsername)
        {
            int returnValue = 0;

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();
                try
                {
                    string sqlCount1Query = "Select count(*) from donors where donor_initial_client_id = " + clientId + " and is_archived = 0 ";

                    int table1 = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlCount1Query));

                    if (table1 <= 0)
                    {
                        string sqlCount2Query = "Select count(*) from donor_test_info where client_id = " + clientId + " and test_status != 7";

                        int table2 = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlCount2Query));

                        if (table2 <= 0)
                        {
                            string sqlQuery = "UPDATE clients SET is_synchronized = b'0', is_archived = b'1', last_modified_on = NOW(), last_modified_by = @LastModifiedBy WHERE client_id = @ClientId";

                            MySqlParameter[] param = new MySqlParameter[2];
                            param[0] = new MySqlParameter("@ClientId", clientId);
                            param[1] = new MySqlParameter("@LastModifiedBy", currentUsername);

                            SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                            trans.Commit();

                            returnValue = 1;
                        }
                    }
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

        /// <summary>
        /// Get the Client information by ClientId
        /// </summary>
        /// <param name="clientId">Client Id which one need to be get from the database.</param>
        /// <returns>Returns Client information.</returns>
        public Client Get(int clientId)
        {
            Client client = null;

            string sqlQuery = "SELECT "
                            + "client_id AS ClientId, "
                            + "client_name AS ClientName, "
                            + "client_division AS ClientDivision, "
                            + "client_type_id AS ClientTypeId, "
                            + "laboratory_vendor_id AS LaboratoryVendorId, "
                            + "mro_vendor_id AS MROVendorId, "
                            + "mro_type_id AS MROTypeId, "
                            + "is_client_active AS IsClientActive, "
                            + "can_edit_test_category AS CanEditTestCategory, "
                            + "client_code AS ClientCode, "
                            + "is_mailing_address_physical AS IsMailingAddressPhysical, "
                            + "sales_representative_id AS SalesRepresentativeId, "
                            + "sales_comissions AS SalesComissions, "
                            + "is_synchronized AS IsSynchronized, "
                            + "is_archived AS IsArchived, "
                            + "created_on AS CreatedOn, "
                            + "created_by AS CreatedBy, "
                            + "last_modified_on AS LastModifiedOn, "
                            + "last_modified_by AS LastModifiedBy, "
                            + "client_timezoneinfo "
                            + "FROM clients WHERE client_id = @ClientId";

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                MySqlParameter[] param = new MySqlParameter[1];
                param[0] = new MySqlParameter("@ClientId", clientId);

                MySqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                if (dr.Read())
                {
                    client = new Client();

                    client.ClientId = (int)dr["ClientId"];
                    client.ClientCode = (string)dr["ClientCode"];
                    client.ClientName = (string)dr["ClientName"];
                    client.client_timezoneinfo = (string)dr["client_timezoneinfo"];
                    client.ClientDivision = dr["ClientDivision"].ToString();
                    client.ClientTypeId = (ClientTypes)dr["ClientTypeId"];
                    client.LaboratoryVendorId = dr["LaboratoryVendorId"].ToString() != string.Empty ? (int?)dr["LaboratoryVendorId"] : null;
                    client.MROVendorId = dr["MROVendorId"].ToString() != string.Empty ? (int?)dr["MROVendorId"] : null;
                    client.MROTypeId = (ClientMROTypes)((int)dr["MROTypeId"]);
                    client.IsClientActive = dr["IsClientActive"].ToString() == "1" ? true : false;
                    client.CanEditTestCategory = dr["CanEditTestCategory"].ToString() == "1" ? true : false;

                    client.IsMailingAddressPhysical = dr["IsMailingAddressPhysical"].ToString() == "1" ? true : false;
                    client.SalesRepresentativeId = dr["SalesRepresentativeId"].ToString() != string.Empty ? (int?)dr["SalesRepresentativeId"] : null;
                    client.SalesComissions = dr["SalesComissions"].ToString() != string.Empty ? (int?)dr["SalesComissions"] : null;

                    client.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    client.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    client.CreatedOn = (DateTime)dr["CreatedOn"];
                    client.CreatedBy = (string)dr["CreatedBy"];
                    client.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    client.LastModifiedBy = (string)dr["LastModifiedBy"];
                }
                dr.Close();

                if (client != null)
                {
                    #region Main Contact

                    sqlQuery = "SELECT "
                                        + "client_contact_id AS ClientContactId, "
                                        + "client_id AS ClientId, "
                                        + "client_contact_first_name AS ClientContactFirstName, "
                                        + "client_contact_last_name AS ClientContactLastName, "
                                        + "client_contact_phone AS ClientContactPhone, "
                                        + "client_contact_fax AS ClientContactFax, "
                                        + "client_contact_email AS ClientContactEmail, "
                                        + "is_synchronized AS IsSynchronized, "
                                        + "created_on AS CreatedOn, "
                                        + "created_by AS CreatedBy, "
                                        + "last_modified_on AS LastModifiedOn, "
                                        + "last_modified_by AS LastModifiedBy "
                                        + "FROM client_contacts WHERE client_id = @ClientId ORDER BY client_contact_first_name, client_contact_last_name";

                    param = new MySqlParameter[1];

                    param[0] = new MySqlParameter("@ClientId", clientId);

                    dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                    if (dr.Read())
                    {
                        client.ClientContact = new ClientContact();

                        client.ClientContact.ClientContactId = (int)dr["ClientContactId"];
                        client.ClientContact.ClientId = (int)dr["ClientId"];
                        client.ClientContact.ClientContactFirstName = (string)dr["ClientContactFirstName"];
                        client.ClientContact.ClientContactLastName = (string)dr["ClientContactLastName"];
                        client.ClientContact.ClientContactPhone = dr["ClientContactPhone"].ToString();
                        client.ClientContact.ClientContactFax = dr["ClientContactFax"].ToString();
                        client.ClientContact.ClientContactEmail = dr["ClientContactEmail"].ToString();
                        client.ClientContact.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                        client.ClientContact.CreatedOn = (DateTime)dr["CreatedOn"];
                        client.ClientContact.CreatedBy = (string)dr["CreatedBy"];
                        client.ClientContact.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                        client.ClientContact.LastModifiedBy = (string)dr["LastModifiedBy"];

                        client.MainContact = client.ClientContact.ClientContactFirstName + " " + client.ClientContact.ClientContactLastName;
                        client.ClientPhone = client.ClientContact.ClientContactPhone;
                        client.ClientFax = client.ClientContact.ClientContactFax;
                        client.ClientEmail = client.ClientContact.ClientContactEmail;
                    }
                    dr.Close();

                    #endregion Main Contact

                    #region Client Addresses

                    sqlQuery = "SELECT "
                                        + "client_address_id AS ClientAddressId, "
                                        + "client_id AS ClientId, "
                                        + "address_type_id AS AddressTypeId, "
                                        + "client_address_1 AS ClientAddress1, "
                                        + "client_address_2 AS ClientAddress2, "
                                        + "client_city AS ClientCity, "
                                        + "client_state AS ClientState, "
                                        + "client_zip AS ClientZip, "
                                        + "is_synchronized AS IsSynchronized, "
                                        + "created_on AS CreatedOn, "
                                        + "created_by AS CreatedBy, "
                                        + "last_modified_on AS LastModifiedOn, "
                                        + "last_modified_by AS LastModifiedBy "
                                        + "FROM client_addresses WHERE client_id = @ClientId ORDER BY address_type_id";

                    param = new MySqlParameter[1];
                    param[0] = new MySqlParameter("@ClientId", clientId);

                    dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                    while (dr.Read())
                    {
                        ClientAddress address = new ClientAddress();

                        address.AddressId = (int)dr["ClientAddressId"];
                        address.ClientId = (int)dr["ClientId"];
                        address.AddressTypeId = (AddressTypes)dr["AddressTypeId"];
                        address.Address1 = (string)dr["ClientAddress1"];
                        address.Address2 = dr["ClientAddress2"].ToString();
                        address.City = (string)dr["ClientCity"];
                        address.State = (string)dr["ClientState"];
                        address.ZipCode = (string)dr["ClientZip"];
                        address.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                        address.CreatedOn = (DateTime)dr["CreatedOn"];
                        address.CreatedBy = (string)dr["CreatedBy"];
                        address.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                        address.LastModifiedBy = (string)dr["LastModifiedBy"];

                        client.ClientAddresses.Add(address);

                        if (address.AddressTypeId == AddressTypes.PhysicalAddress1)
                        {
                            client.ClientCity = address.City;
                            client.ClientState = address.State;
                        }
                    }
                    dr.Close();

                    #endregion Client Addresses
                }
            }

            return client;
        }

        /// <summary>
        /// Get the Client information by ClientId
        /// </summary>
        /// <param name="clientCode">Client Code which one need to be get from the database.</param>
        /// <returns>Returns Client information.</returns>
        public Client Get(string clientCode)
        {
            Client client = null;

            //string sqlQuery =
            //    @"
            //                SELECT
            //                client_id AS ClientId, 
            //                client_name AS ClientName, 
            //                client_division AS ClientDivision, 
            //                client_type_id AS ClientTypeId, 
            //                laboratory_vendor_id AS LaboratoryVendorId, 
            //                mro_vendor_id AS MROVendorId, 
            //                mro_type_id AS MROTypeId, 
            //                is_client_active AS IsClientActive, 
            //                client_code AS ClientCode, 
            //                is_mailing_address_physical AS IsMailingAddressPhysical, 
            //                sales_representative_id AS SalesRepresentativeId, 
            //                sales_comissions AS SalesComissions, 
            //                is_synchronized AS IsSynchronized, 
            //                is_archived AS IsArchived, 
            //                created_on AS CreatedOn, 
            //                created_by AS CreatedBy, 
            //                last_modified_on AS LastModifiedOn, 
            //                last_modified_by AS LastModifiedBy
            //                FROM clients 
            //                WHERE UPPER(client_code) = UPPER(@ClientCode)
            //                ";

            string sqlQuery =
                @"
          SELECT clients.client_id                   AS ClientId,
       clients.client_name                 AS ClientName,
       clients.client_division             AS ClientDivision,
       clients.client_type_id              AS ClientTypeId,
       clients.laboratory_vendor_id        AS LaboratoryVendorId,
       clients.mro_vendor_id               AS MROVendorId,
       clients.mro_type_id                 AS MROTypeId,
       clients.is_client_active            AS IsClientActive,
       clients.client_code                 AS ClientCode,
       clients.is_mailing_address_physical AS IsMailingAddressPhysical,
       clients.sales_representative_id     AS SalesRepresentativeId,
       clients.sales_comissions            AS SalesComissions,
       clients.is_synchronized             AS IsSynchronized,
       clients.is_archived                 AS IsArchived,
       clients.created_on                  AS CreatedOn,
       clients.created_by                  AS CreatedBy,
       clients.last_modified_on            AS LastModifiedOn,
       clients.last_modified_by            AS LastModifiedBy,
       IF(BIPM.backend_integration_partner_client_map_id IS NOT NULL,
          1,
          IF(BIPM.backend_integration_partner_id IS NOT NULL, 1, 0))
          AS IntegrationPartner,
       IF(BIPM.backend_integration_partner_client_map_id IS NOT NULL,
          BIPM.require_login,
          0)
          AS require_login,
       IF(BIPM.backend_integration_partner_client_map_id IS NOT NULL,
          BIPM.require_remote_login,
          0)
          AS require_remote_login,
       if(BIPM.backend_integration_partner_id is null,0,BIPM.backend_integration_partner_id) as backend_integration_partner_id,
       if(BIPM.backend_integration_partner_client_map_id is null,0, BIPM.backend_integration_partner_client_map_id) as backend_integration_partner_client_map_id,
       if (BIP.login_url is null,'', BIP.login_url) as login_url,
       if (BIPM.client_department_id=0,1,0) as client_wide
FROM clients
     LEFT OUTER JOIN backend_integration_partner_client_map BIPM
        ON BIPM.client_id = clients.client_id AND BIPM.active > 0 AND BIPM.client_department_id = 0
     LEFT OUTER JOIN backend_integration_partners BIP
        ON     BIP.active > 0
           AND BIPM.backend_integration_partner_id =
                  BIP.backend_integration_partner_id
WHERE UPPER(client_code) = UPPER(@ClientCode)
            ";

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                MySqlParameter[] param = new MySqlParameter[1];
                param[0] = new MySqlParameter("@ClientCode", clientCode);

                MySqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                if (dr.Read())
                {
                    client = new Client();

                    client.ClientId = (int)dr["ClientId"];
                    client.ClientCode = (string)dr["ClientCode"];
                    client.ClientName = (string)dr["ClientName"];
                    client.ClientDivision = dr["ClientDivision"].ToString();
                    client.ClientTypeId = (ClientTypes)dr["ClientTypeId"];
                    client.LaboratoryVendorId = dr["LaboratoryVendorId"].ToString() != string.Empty ? (int?)dr["LaboratoryVendorId"] : null;
                    client.MROVendorId = dr["MROVendorId"].ToString() != string.Empty ? (int?)dr["MROVendorId"] : null;
                    client.MROTypeId = (ClientMROTypes)((int)dr["MROTypeId"]);
                    client.IsClientActive = dr["IsClientActive"].ToString() == "1" ? true : false;

                    client.IsMailingAddressPhysical = dr["IsMailingAddressPhysical"].ToString() == "1" ? true : false;
                    client.SalesRepresentativeId = dr["SalesRepresentativeId"].ToString() != string.Empty ? (int?)dr["SalesRepresentativeId"] : null;
                    client.SalesComissions = dr["SalesComissions"].ToString() != string.Empty ? (int?)dr["SalesComissions"] : null;

                    client.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    client.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    client.CreatedOn = (DateTime)dr["CreatedOn"];
                    client.CreatedBy = (string)dr["CreatedBy"];
                    client.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    client.LastModifiedBy = (string)dr["LastModifiedBy"];

                    client.IntegrationPartner = (long)dr["IntegrationPartner"] > 0;
                    client.client_wide = (long)dr["client_wide"] > 0;
                    client.require_login = (long)dr["require_login"] > 0;
                    client.require_remote_login = (long)dr["require_remote_login"] > 0;
                    var backend_integration_partner_id = (long)dr["backend_integration_partner_id"];
                    var backend_integration_partner_client_map_id = (long)dr["backend_integration_partner_client_map_id"];
                    client.backend_integration_partner_id = (int)backend_integration_partner_id;
                    client.backend_integration_partner_client_map_id = (int)backend_integration_partner_client_map_id;
                    client.login_url = (string)dr["login_url"];

                }
                dr.Close();

                if (client != null)
                {
                    #region Main Contact

                    sqlQuery = "SELECT "
                                        + "client_contact_id AS ClientContactId, "
                                        + "client_id AS ClientId, "
                                        + "client_contact_first_name AS ClientContactFirstName, "
                                        + "client_contact_last_name AS ClientContactLastName, "
                                        + "client_contact_phone AS ClientContactPhone, "
                                        + "client_contact_fax AS ClientContactFax, "
                                        + "client_contact_email AS ClientContactEmail, "
                                        + "is_synchronized AS IsSynchronized, "
                                        + "created_on AS CreatedOn, "
                                        + "created_by AS CreatedBy, "
                                        + "last_modified_on AS LastModifiedOn, "
                                        + "last_modified_by AS LastModifiedBy "
                                        + "FROM client_contacts WHERE client_id = @ClientId ORDER BY client_contact_first_name, client_contact_last_name";

                    param = new MySqlParameter[1];

                    param[0] = new MySqlParameter("@ClientId", client.ClientId);

                    dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                    if (dr.Read())
                    {
                        client.ClientContact = new ClientContact();

                        client.ClientContact.ClientContactId = (int)dr["ClientContactId"];
                        client.ClientContact.ClientId = (int)dr["ClientId"];
                        client.ClientContact.ClientContactFirstName = (string)dr["ClientContactFirstName"];
                        client.ClientContact.ClientContactLastName = (string)dr["ClientContactLastName"];
                        client.ClientContact.ClientContactPhone = dr["ClientContactPhone"].ToString();
                        client.ClientContact.ClientContactFax = dr["ClientContactFax"].ToString();
                        client.ClientContact.ClientContactEmail = dr["ClientContactEmail"].ToString();
                        client.ClientContact.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                        client.ClientContact.CreatedOn = (DateTime)dr["CreatedOn"];
                        client.ClientContact.CreatedBy = (string)dr["CreatedBy"];
                        client.ClientContact.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                        client.ClientContact.LastModifiedBy = (string)dr["LastModifiedBy"];

                        client.MainContact = client.ClientContact.ClientContactFirstName + " " + client.ClientContact.ClientContactLastName;
                        client.ClientPhone = client.ClientContact.ClientContactPhone;
                        client.ClientFax = client.ClientContact.ClientContactFax;
                        client.ClientEmail = client.ClientContact.ClientContactEmail;
                    }
                    dr.Close();

                    #endregion Main Contact

                    #region Client Addresses

                    sqlQuery = "SELECT "
                                        + "client_address_id AS ClientAddressId, "
                                        + "client_id AS ClientId, "
                                        + "address_type_id AS AddressTypeId, "
                                        + "client_address_1 AS ClientAddress1, "
                                        + "client_address_2 AS ClientAddress2, "
                                        + "client_city AS ClientCity, "
                                        + "client_state AS ClientState, "
                                        + "client_zip AS ClientZip, "
                                        + "is_synchronized AS IsSynchronized, "
                                        + "created_on AS CreatedOn, "
                                        + "created_by AS CreatedBy, "
                                        + "last_modified_on AS LastModifiedOn, "
                                        + "last_modified_by AS LastModifiedBy "
                                        + "FROM client_addresses WHERE client_id = @ClientId ORDER BY address_type_id";

                    param = new MySqlParameter[1];
                    param[0] = new MySqlParameter("@ClientId", client.ClientId);

                    dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                    while (dr.Read())
                    {
                        ClientAddress address = new ClientAddress();

                        address.AddressId = (int)dr["ClientAddressId"];
                        address.ClientId = (int)dr["ClientId"];
                        address.AddressTypeId = (AddressTypes)dr["AddressTypeId"];
                        address.Address1 = (string)dr["ClientAddress1"];
                        address.Address2 = dr["ClientAddress2"].ToString();
                        address.City = (string)dr["ClientCity"];
                        address.State = (string)dr["ClientState"];
                        address.ZipCode = (string)dr["ClientZip"];
                        address.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                        address.CreatedOn = (DateTime)dr["CreatedOn"];
                        address.CreatedBy = (string)dr["CreatedBy"];
                        address.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                        address.LastModifiedBy = (string)dr["LastModifiedBy"];

                        client.ClientAddresses.Add(address);

                        if (address.AddressTypeId == AddressTypes.PhysicalAddress1)
                        {
                            client.ClientCity = address.City;
                            client.ClientState = address.State;
                        }
                    }
                    dr.Close();

                    #endregion Client Addresses

                    #region integration
                    // If this is an integration that requires remote login to register for - we should check that.
                    // Specifically - DeSales with ProjectConcert
                    // The test for this is if there's a map with client ID of zero.

                    #endregion integration

                }
            }

            return client;
        }

        /// <summary>
        /// Get all the Client informations.
        /// </summary>
        /// <returns>Returns Client information list.</returns>
        public DataTable GetList(string getInActive = null)
        {
            string sql = string.Empty;

            string sqlQuery = "SELECT "
                            + "client_id AS ClientId, "
                            + "client_name AS ClientName, "
                            + "client_division AS ClientDivision, "
                            + "client_type_id AS ClientTypeId, "
                            + "laboratory_vendor_id AS LaboratoryVendorId, "
                            + "mro_vendor_id AS MROVendorId, "
                            + "mro_type_id AS MROTypeId, "
                            + "is_mailing_address_physical AS IsMailingAddressPhysical, "
                            + "is_client_active AS IsClientActive, "
                            + "sales_representative_id AS SalesRepresentativeId, "
                            + "sales_comissions AS SalesComissions, "
                            + "client_code AS ClientCode, "
                            + "is_synchronized AS IsSynchronized, "
                            + "is_archived AS IsArchived, "
                            + "created_on AS CreatedOn, "
                            + "created_by AS CreatedBy, "
                            + "last_modified_on AS LastModifiedOn, "
                            + "last_modified_by AS LastModifiedBy ";
            if (getInActive == "1")
            {
                sql = "FROM clients WHERE is_archived = 0 ORDER BY client_name";
            }
            else
            {
                sql = "FROM clients WHERE is_archived = 0 AND is_client_active =b'1' ORDER BY client_name";
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

        /// <summary>
        /// Get all the Client informations.
        /// </summary>
        /// <param name="currentUserId">Current User Id</param>
        /// <returns>Returns Client information list.</returns>
        public DataTable GetList(int currentUserId)
        {
            string sqlQuery = "SELECT "
                            + "client_id AS ClientId, "
                            + "client_name AS ClientName, "
                            + "client_division AS ClientDivision, "
                            + "client_type_id AS ClientTypeId, "
                            + "laboratory_vendor_id AS LaboratoryVendorId, "
                            + "mro_vendor_id AS MROVendorId, "
                            + "mro_type_id AS MROTypeId, "
                            + "is_mailing_address_physical AS IsMailingAddressPhysical, "
                            + "is_client_active AS IsClientActive, "
                            + "sales_representative_id AS SalesRepresentativeId, "
                            + "sales_comissions AS SalesComissions, "
                            + "client_code AS ClientCode, "
                            + "is_synchronized AS IsSynchronized, "
                            + "is_archived AS IsArchived, "
                            + "created_on AS CreatedOn, "
                            + "created_by AS CreatedBy, "
                            + "last_modified_on AS LastModifiedOn, "
                            + "last_modified_by AS LastModifiedBy "
                            + "FROM clients WHERE is_archived = 0 "
                            + "AND client_id IN (SELECT client_id FROM client_departments WHERE client_department_id IN (SELECT client_department_id FROM user_departments WHERE user_id = @UserId)) "
                            + "ORDER BY client_name";

            MySqlParameter[] param = new MySqlParameter[1];

            param[0] = new MySqlParameter("@UserId", currentUserId);

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
        /// Get all the Client informations based on the search criteria.
        /// </summary>
        /// <param name="searchParam">Collection of search criteria</param>
        /// <returns>Returns Client information list.</returns>
        public DataTable GetList(Dictionary<string, string> searchParam)
        {
            string sqlQuery = "SELECT "
                           + "clients.client_id AS ClientId, "
                           + "clients.client_name AS ClientName, "
                           + "clients.client_division AS ClientDivision, "
                           + "clients.client_type_id AS ClientTypeId, "
                           + "clients.laboratory_vendor_id AS LaboratoryVendorId, "
                           + "clients.mro_vendor_id AS MROVendorId, "
                           + "clients.mro_type_id AS MROTypeId, "
                //+ "client_addresses.client_city AS City, "
                //+ "client_addresses.client_state AS State, "
                //+ "client_addresses.client_zip AS ZipCode, "
                //+ "client_contacts.client_contact_first_name AS ClientContactFirstName, "
                //+ "client_contacts.client_contact_last_name AS ClientContactLastName, "
                //+ "client_contacts.client_contact_phone AS ClientContactPhone, "
                //+ "client_contacts.client_contact_fax AS ClientContactFax, "
                //+ "client_contacts.client_contact_email AS ClientContactEmail, "
                + "clients.is_mailing_address_physical AS IsMailingAddressPhysical, "
                           + "clients.is_client_active AS IsClientActive, "
                + "clients.sales_representative_id AS SalesRepresentativeId, "
                + "clients.sales_comissions AS SalesComissions, "
                           + "clients.client_code AS ClientCode, "
                           + "clients.is_synchronized AS IsSynchronized, "
                           + "clients.is_archived AS IsArchived, "
                           + "clients.created_on AS CreatedOn, "
                           + "clients.created_by AS CreatedBy, "
                           + "clients.last_modified_on AS LastModifiedOn, "
                           + "clients.last_modified_by AS LastModifiedBy "
                           + "FROM clients "
                           //+ "INNER JOIN client_addresses ON client_addresses.client_id = clients.client_id "
                           //+ "INNER JOIN client_contacts on client_contacts.client_id = clients.client_id "
                           + "WHERE clients.is_archived= 0 ";

            List<MySqlParameter> param = new List<MySqlParameter>();
            bool isInActiveFlag = false;
            // bool isSearchKeyword = false;

            foreach (KeyValuePair<string, string> searchItem in searchParam)
            {
                if (searchItem.Key == "GeneralSearch")
                {
                    sqlQuery += "AND (clients.client_name LIKE @SearchKeyword "
                                + "OR clients.client_code LIKE @SearchKeyword) ";
                    //+ "OR client_addresses.client_city LIKE @SearchKeyword "
                    //+ "OR client_addresses.client_state LIKE @SearchKeyword "
                    //+ "OR client_contacts.client_contact_first_name LIKE @SearchKeyword "
                    //+ "OR client_contacts.client_contact_last_name LIKE @SearchKeyword "
                    //+ "OR client_contacts.client_contact_phone LIKE @SearchKeyword "
                    //+ "OR client_contacts.client_contact_fax LIKE @SearchKeyword "
                    //+ "OR client_contacts.client_contact_email LIKE @SearchKeyword) ";

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
                sqlQuery += "AND clients.is_client_active = '1' ";
            }

            sqlQuery += "ORDER BY clients.client_name ";

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
        /// Get all the Client Contact informations based on the client id.
        /// </summary>
        /// <param name="clientId">Client Id</param>
        /// <returns>Returns Client Contact information list.</returns>
        public DataTable GetClientContatListByClientId(int clientId)
        {
            string sqlQuery = "SELECT "
                            + "client_contact_id AS ClientContactId, "
                            + "client_id AS ClientId, "
                            + "client_department_id AS ClientDepartmentId,"
                            + "client_contact_first_name AS ClientContactFirstName, "
                            + "client_contact_last_name AS ClientContactLastName, "
                            + "client_contact_phone AS ClientContactPhone, "
                            + "client_contact_fax AS ClientContactFax, "
                            + "client_contact_email AS ClientContactEmail, "
                            + "is_synchronized AS IsSynchronized, "
                            + "created_on AS CreatedOn, "
                            + "created_by AS CreatedBy, "
                            + "last_modified_on AS LastModifiedOn, "
                            + "last_modified_by AS LastModifiedBy "
                            + "FROM client_contacts WHERE client_id = @ClientId ORDER BY client_contact_first_name, client_contact_last_name";

            MySqlParameter[] param = new MySqlParameter[1];
            param[0] = new MySqlParameter("@ClientId", clientId);

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
        /// Get all the Client Contact informations based on the client id.
        /// </summary>
        /// <param name="clientId">Client Id</param>
        /// <returns>Returns Client Contact information list.</returns>
        public DataTable GetClientContatList(int clientDepartmentId)
        {
            string sqlQuery = "SELECT "
                            + "client_contact_id AS ClientContactId, "
                            + "client_id AS ClientId, "
                            + "client_department_id AS ClientDepartmentId,"
                            + "client_contact_first_name AS ClientContactFirstName, "
                            + "client_contact_last_name AS ClientContactLastName, "
                            + "client_contact_phone AS ClientContactPhone, "
                            + "client_contact_fax AS ClientContactFax, "
                            + "client_contact_email AS ClientContactEmail, "
                            + "is_synchronized AS IsSynchronized, "
                            + "created_on AS CreatedOn, "
                            + "created_by AS CreatedBy, "
                            + "last_modified_on AS LastModifiedOn, "
                            + "last_modified_by AS LastModifiedBy "
                            + "FROM client_contacts WHERE client_department_id = @ClientDepartmentId ORDER BY client_contact_first_name, client_contact_last_name";

            MySqlParameter[] param = new MySqlParameter[1];
            param[0] = new MySqlParameter("@ClientDepartmentId", clientDepartmentId);

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
        /// Get all the Client Address informations based on the client id.
        /// </summary>
        /// <param name="clientId">Client Id</param>
        /// <returns>Returns Client Address information list.</returns>
        public DataTable GetClientAddressListByClientId(int clientId)
        {
            string sqlQuery = "SELECT "
                            + "client_address_id AS ClientAddressId, "
                            + "client_id AS ClientId, "
                            + "client_department_id AS ClientDepartmentId,"
                            + "address_type_id AS AddressTypeId, "
                            + "client_address_1 AS ClientAddress1, "
                            + "client_address_2 AS ClientAddress2, "
                            + "client_city AS ClientCity, "
                            + "client_state AS ClientState, "
                            + "client_zip AS ClientZip, "
                            + "is_synchronized AS IsSynchronized, "
                            + "created_on AS CreatedOn, "
                            + "created_by AS CreatedBy, "
                            + "last_modified_on AS LastModifiedOn, "
                            + "last_modified_by AS LastModifiedBy "
                            + "FROM client_addresses WHERE client_id = @ClientId ORDER BY address_type_id";

            MySqlParameter[] param = new MySqlParameter[1];
            param[0] = new MySqlParameter("@ClientId", clientId);

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
        /// Get all the Client Address informations based on the client id.
        /// </summary>
        /// <param name="clientId">Client Id</param>
        /// <returns>Returns Client Address information list.</returns>
        public DataTable GetClientAddressList(int clientDepartmentId)
        {
            string sqlQuery = "SELECT "
                            + "client_address_id AS ClientAddressId, "
                            + "client_id AS ClientId, "
                            + "client_department_id AS ClientDepartmentId,"
                            + "address_type_id AS AddressTypeId, "
                            + "client_address_1 AS ClientAddress1, "
                            + "client_address_2 AS ClientAddress2, "
                            + "client_city AS ClientCity, "
                            + "client_state AS ClientState, "
                            + "client_zip AS ClientZip, "
                            + "is_synchronized AS IsSynchronized, "
                            + "created_on AS CreatedOn, "
                            + "created_by AS CreatedBy, "
                            + "last_modified_on AS LastModifiedOn, "
                            + "last_modified_by AS LastModifiedBy "
                            + "FROM client_addresses WHERE client_department_id = @ClientDepartmentId ORDER BY address_type_id";

            MySqlParameter[] param = new MySqlParameter[1];
            param[0] = new MySqlParameter("@ClientDepartmentId", clientDepartmentId);

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

        public DataTable GetByEmail(string Email)
        {
            string sqlQuery = "SELECT client_department_id AS ClientDepartmentId,client_contact_first_name AS ClientContactFirstName,client_contact_last_name AS client_ContactLastName,client_contact_phone AS ClientContactPhone,client_contact_fax AS ClientContactFax, client_contact_email AS ClientContactEmail, is_synchronized AS IsSynchronized, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM client_contacts WHERE LOWER(client_contact_email) = LOWER(@ClientContactEmail)";

            MySqlParameter[] param = new MySqlParameter[1];
            param[0] = new MySqlParameter("@ClientContactEmail", Email);

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
            string sqlQuery = "SELECT "
                            + "clients.client_id AS ClientId, "
                            + "clients.client_name AS ClientName, "
                            + "clients.client_division AS ClientDivision, "
                            + "clients.client_type_id AS ClientTypeId, "
                            + "clients.laboratory_vendor_id AS LaboratoryVendorId, "
                            + "clients.mro_vendor_id AS MROVendorId, "
                            + "clients.mro_type_id AS MROTypeId, "
                 + "clients.is_mailing_address_physical AS IsMailingAddressPhysical, "
                            + "clients.is_client_active AS IsClientActive, "
                 + "clients.sales_representative_id AS SalesRepresentativeId, "
                 + "clients.sales_comissions AS SalesComissions, "
                            + "clients.client_code AS ClientCode, "
                            + "clients.is_synchronized AS IsSynchronized, "
                            + "clients.is_archived AS IsArchived, "
                            + "clients.created_on AS CreatedOn, "
                            + "clients.created_by AS CreatedBy, "
                            + "clients.last_modified_on AS LastModifiedOn, "
                            + "clients.last_modified_by AS LastModifiedBy "
                            + "FROM clients "
                            + "WHERE clients.is_archived= 0 ";

            if (active == false)
            {
                sqlQuery += "AND clients.is_client_active = b'1'";
            }

            if (searchparam == "clientName")
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

            if (searchparam == "isActive")
            {
                if (getInActive == "1")
                {
                    sql = "ORDER BY clients.is_client_active";
                }
                else
                {
                    sql = "ORDER BY clients.is_client_active desc";
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

        #region ClientDept Public Methods

        /// <summary>
        /// Inserts the Client Department to the database.
        /// </summary>
        /// <param name="clientDepartment">Client Department which one need to be added to the database.</param>
        /// <returns>Returns ClientDepartmentId, the auto increament value.</returns>
        public int InsertClientDepartment(ClientDepartment clientDepartment)
        {
            int returnValue = 0;

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();

                try
                {
                    string sqlQuery = $@"INSERT INTO client_departments 
    (client_id, department_name,lab_code, mro_type_id, payment_type_id, is_mailing_address_physical, sales_representative_id, sales_comissions, is_contact_info_as_client, FormFoxCode, is_department_active, is_synchronized,is_archived, created_on, created_by, last_modified_on, last_modified_by) VALUES 
            (";
                    sqlQuery += $@"
        @ClientId, @DepartmentName,@LabCode,@MROTypeId,@PaymentTypeId, @IsMailingAddressPhysical, @SalesRepresentativeId, @SalesComissions, @IsContactInfoAsClient, @FormFoxCode,"
                               + " @IsDepartmentActive, b'0',b'0', NOW(), @CreatedBy, NOW(), @LastModifiedBy)";

                    MySqlParameter[] param = new MySqlParameter[13];
                    param[0] = new MySqlParameter("@ClientId", clientDepartment.ClientId);
                    param[1] = new MySqlParameter("@DepartmentName", clientDepartment.DepartmentName);
                    param[2] = new MySqlParameter("@LabCode", clientDepartment.LabCode);
                    param[3] = new MySqlParameter("@MROTypeId", (int)clientDepartment.MROTypeId);
                    param[4] = new MySqlParameter("@PaymentTypeId", (int)clientDepartment.PaymentTypeId);
                    param[5] = new MySqlParameter("@IsMailingAddressPhysical", clientDepartment.IsMailingAddressPhysical);
                    param[6] = new MySqlParameter("@SalesRepresentativeId", clientDepartment.SalesRepresentativeId);
                    param[7] = new MySqlParameter("@SalesComissions", clientDepartment.SalesComissions);
                    param[8] = new MySqlParameter("@IsContactInfoAsClient", clientDepartment.IsPhysicalAddressAsClient);
                    param[9] = new MySqlParameter("@IsDepartmentActive", clientDepartment.IsDepartmentActive);
                    param[10] = new MySqlParameter("@CreatedBy", clientDepartment.CreatedBy);
                    param[11] = new MySqlParameter("@LastModifiedBy", clientDepartment.LastModifiedBy);
                    param[12] = new MySqlParameter("@FormFoxCode", clientDepartment.FormFoxCode);


                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                    sqlQuery = "SELECT LAST_INSERT_ID()";

                    returnValue = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlQuery));

                    clientDepartment.ClientDepartmentId = returnValue;

                    sqlQuery = "INSERT INTO client_dept_test_categories (client_department_id, test_category_id,display_order,test_panel_price, is_synchronized, created_on, created_by, last_modified_on, last_modified_by) VALUES (@ClientDepartmentId, @TestCategoryId,@DisplayOrder,@TestPanelPrice, b'0', NOW(), @CreatedBy, NOW(), @LastModifiedBy)";

                    if (clientDepartment.IsUA)
                    {
                        param = new MySqlParameter[6];
                        param[0] = new MySqlParameter("@ClientDepartmentId", clientDepartment.ClientDepartmentId);
                        param[1] = new MySqlParameter("@TestCategoryId", (int)TestCategories.UA);
                        param[2] = new MySqlParameter("@DisplayOrder", 1);
                        param[3] = new MySqlParameter("@TestPanelPrice", 0);
                        param[4] = new MySqlParameter("@CreatedBy", clientDepartment.CreatedBy);
                        param[5] = new MySqlParameter("@LastModifiedBy", clientDepartment.LastModifiedBy);

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);
                    }

                    if (clientDepartment.IsHair)
                    {
                        param = new MySqlParameter[6];
                        param[0] = new MySqlParameter("@ClientDepartmentId", clientDepartment.ClientDepartmentId);
                        param[1] = new MySqlParameter("@TestCategoryId", (int)TestCategories.Hair);
                        param[2] = new MySqlParameter("@DisplayOrder", 2);
                        param[3] = new MySqlParameter("@TestPanelPrice", 0);
                        param[4] = new MySqlParameter("@CreatedBy", clientDepartment.CreatedBy);
                        param[5] = new MySqlParameter("@LastModifiedBy", clientDepartment.LastModifiedBy);

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);
                    }

                    if (clientDepartment.IsDNA)
                    {
                        param = new MySqlParameter[6];
                        param[0] = new MySqlParameter("@ClientDepartmentId", clientDepartment.ClientDepartmentId);
                        param[1] = new MySqlParameter("@TestCategoryId", (int)TestCategories.DNA);
                        param[2] = new MySqlParameter("@DisplayOrder", 3);
                        param[3] = new MySqlParameter("@TestPanelPrice", 0);
                        param[4] = new MySqlParameter("@CreatedBy", clientDepartment.CreatedBy);
                        param[5] = new MySqlParameter("@LastModifiedBy", clientDepartment.LastModifiedBy);

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);
                    }
                    if (clientDepartment.IsRecordKeeping)
                    {
                        param = new MySqlParameter[6];
                        param[0] = new MySqlParameter("@ClientDepartmentId", clientDepartment.ClientDepartmentId);
                        param[1] = new MySqlParameter("@TestCategoryId", (int)TestCategories.RC);
                        param[2] = new MySqlParameter("@DisplayOrder", 3);
                        param[3] = new MySqlParameter("@TestPanelPrice", 0);
                        param[4] = new MySqlParameter("@CreatedBy", clientDepartment.CreatedBy);
                        param[5] = new MySqlParameter("@LastModifiedBy", clientDepartment.LastModifiedBy);

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);
                    }

                    if (clientDepartment.IsBC)
                    {
                        param = new MySqlParameter[6];
                        param[0] = new MySqlParameter("@ClientDepartmentId", clientDepartment.ClientDepartmentId);
                        param[1] = new MySqlParameter("@TestCategoryId", (int)TestCategories.BC);
                        param[2] = new MySqlParameter("@DisplayOrder", 3);
                        param[3] = new MySqlParameter("@TestPanelPrice", 0);
                        param[4] = new MySqlParameter("@CreatedBy", clientDepartment.CreatedBy);
                        param[5] = new MySqlParameter("@LastModifiedBy", clientDepartment.LastModifiedBy);

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);
                    }

                    sqlQuery = "INSERT INTO client_contacts ("
                                + "client_department_id,"
                                + "client_contact_first_name, "
                                + "client_contact_last_name, "
                                + "client_contact_phone, "
                                + "client_contact_fax, "
                                + "client_contact_email, "
                                + "is_synchronized, "
                                + "created_on, "
                                + "created_by, "
                                + "last_modified_on, "
                                + "last_modified_by"
                                + ") VALUES ("
                                + "@ClientDepartmentId,"
                                + "@ClientContactFirstName, "
                                + "@ClientContactLastName, "
                                + "@ClientContactPhone, "
                                + "@ClientContactFax, "
                                + "@ClientContactEmail, "
                                + "b'0', "
                                + "NOW(), "
                                + "@CreatedBy, "
                                + "NOW(), "
                                + "@LastModifiedBy)";

                    if (clientDepartment.ClientContact != null)
                    {
                        param = new MySqlParameter[9];
                        param[0] = new MySqlParameter("@ClientDepartmentId", clientDepartment.ClientDepartmentId);
                        param[2] = new MySqlParameter("@ClientContactFirstName", clientDepartment.ClientContact.ClientContactFirstName);
                        param[3] = new MySqlParameter("@ClientContactLastName", clientDepartment.ClientContact.ClientContactLastName);
                        param[4] = new MySqlParameter("@ClientContactPhone", clientDepartment.ClientContact.ClientContactPhone);
                        param[5] = new MySqlParameter("@ClientContactFax", clientDepartment.ClientContact.ClientContactFax);
                        param[6] = new MySqlParameter("@ClientContactEmail", clientDepartment.ClientContact.ClientContactEmail);
                        param[7] = new MySqlParameter("@CreatedBy", clientDepartment.CreatedBy);
                        param[8] = new MySqlParameter("@LastModifiedBy", clientDepartment.LastModifiedBy);

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);
                    }

                    sqlQuery = "INSERT INTO client_addresses ("
                                + "client_department_id,"
                                + "address_type_id, "
                                + "client_address_1, "
                                + "client_address_2, "
                                + "client_city, "
                                + "client_state, "
                                + "client_zip, "
                                + "is_synchronized, "
                                + "created_on, "
                                + "created_by, "
                                + "last_modified_on, "
                                + "last_modified_by"
                                + ") VALUES ("
                                + "@ClientDepartmentId, "
                                + "@AddressTypeId, "
                                + "@Address1, "
                                + "@Address2, "
                                + "@City, "
                                + "@State, "
                                + "@ZipCode, "
                                + "b'0', "
                                + "NOW(), "
                                + "@CreatedBy, "
                                + "NOW(), "
                                + "@LastModifiedBy)";

                    if (clientDepartment.ClientAddresses.Count > 0)
                    {
                        foreach (ClientAddress address in clientDepartment.ClientAddresses)
                        {
                            param = new MySqlParameter[9];

                            param[0] = new MySqlParameter("@ClientDepartmentId", clientDepartment.ClientDepartmentId);
                            param[1] = new MySqlParameter("@AddressTypeId", (int)address.AddressTypeId);
                            param[2] = new MySqlParameter("@Address1", address.Address1);
                            param[3] = new MySqlParameter("@Address2", address.Address2);
                            param[4] = new MySqlParameter("@City", address.City);
                            param[5] = new MySqlParameter("@State", address.State);
                            param[6] = new MySqlParameter("@ZipCode", address.ZipCode);
                            param[7] = new MySqlParameter("@CreatedBy", clientDepartment.CreatedBy);
                            param[8] = new MySqlParameter("@LastModifiedBy", clientDepartment.LastModifiedBy);

                            SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);
                        }
                    }

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

        public int InsertClientDepartmentDocType(ClientDocTypes dt)
        {
            int returnValue = 0;

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();

                try
                {
                    string sqlQuery = "INSERT INTO surpathlive.client_dept_doctypes(client_departmentdoctypeid, client_department_id, created_on, description, instructions, duedate, semester, is_notifystudent, notifydays1";
                    sqlQuery += ", notifydays2, notifydays3, is_doesexpire, is_required, is_archived)"
                                + "VALUES(NULL, @client_department_id, NULL, @description, @instructions, @duedate, @semester, @is_notifystudent, @notifydays1, @notifydays2, @notifydays3, @is_doesexpire, @is_required, 0)";

                    MySqlParameter[] param = new MySqlParameter[11];
                    param[0] = new MySqlParameter("@client_department_id", dt.ClientDepartmentId);
                    param[1] = new MySqlParameter("@description", dt.Description);
                    param[2] = new MySqlParameter("@instructions", dt.Instructions);
                    param[3] = new MySqlParameter("@duedate", (DateTime)dt.DueDate);
                    param[4] = new MySqlParameter("@semester", dt.Semester);
                    param[5] = new MySqlParameter("@is_notifystudent", (bool)dt.IsNotifyStudent);
                    param[6] = new MySqlParameter("@notifydays1", (int)dt.NotifyDays1);
                    param[7] = new MySqlParameter("@notifydays2", (Int32)dt.NotifyDays2);
                    param[8] = new MySqlParameter("@notifydays3", (Int32)dt.NotifyDays3);
                    param[9] = new MySqlParameter("@is_doesexpire", (bool)dt.IsDoesExpire);
                    param[10] = new MySqlParameter("@is_required", (bool)dt.IsRequired);

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                    sqlQuery = "SELECT LAST_INSERT_ID()";

                    returnValue = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlQuery));

                    dt.ClientDoctypeId = returnValue;

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

        /// <summary>
        /// Updates the Client Department to the database.
        /// </summary>
        /// <param name="clientDepartment">Client Department which one need to be updated to the database.</param>
        /// <returns>Returns number of records affected in the database.</returns>
        public int UpdateClientDepartment(ClientDepartment clientDepartment)
        {
            int returnValue = 0;

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();

                try
                {
                    string sqlQuery = "UPDATE client_departments SET "
                                    + "department_name = @DepartmentName, "
                                    + "lab_code = @LabCode, "
                                    + "QuestCode = @QuestCode, "
                                    + "ClearStarCode = @ClearStarCode, "
                                    + "FormFoxCode = @FormFoxCode, "
                                    + "mro_type_id = @MROTypeId, "
                                    + "payment_type_id = @PaymentTypeId, "
                                    + "is_mailing_address_physical = @IsMailingAddressPhysical, "
                                    + "sales_representative_id=@SalesRepresentativeId,"
                                    + "sales_comissions=@SalesComissions,"
                                    + "is_contact_info_as_client = @IsContactInfoAsClient, "
                                    + "is_department_active=@IsDepartmentActive,"
                                    + "is_synchronized = b'0', "
                                    + "last_modified_on = NOW(), "
                                    + "last_modified_by = @LastModifiedBy "
                                    + "WHERE client_department_id = @ClientDepartmentId";

                    MySqlParameter[] param = new MySqlParameter[14];
                    param[0] = new MySqlParameter("@ClientDepartmentId", clientDepartment.ClientDepartmentId);
                    param[1] = new MySqlParameter("@DepartmentName", clientDepartment.DepartmentName);
                    param[2] = new MySqlParameter("@LabCode", clientDepartment.LabCode);
                    param[3] = new MySqlParameter("@MROTypeId", (int)clientDepartment.MROTypeId);
                    param[4] = new MySqlParameter("@PaymentTypeId", (int)clientDepartment.PaymentTypeId);
                    param[5] = new MySqlParameter("@IsMailingAddressPhysical", clientDepartment.IsMailingAddressPhysical);
                    param[6] = new MySqlParameter("@SalesRepresentativeId", clientDepartment.SalesRepresentativeId);
                    param[7] = new MySqlParameter("@SalesComissions", clientDepartment.SalesComissions);
                    param[8] = new MySqlParameter("@IsContactInfoAsClient", clientDepartment.IsPhysicalAddressAsClient);
                    param[9] = new MySqlParameter("@IsDepartmentActive", clientDepartment.IsDepartmentActive);
                    param[10] = new MySqlParameter("@LastModifiedBy", clientDepartment.LastModifiedBy);
                    param[11] = new MySqlParameter("@QuestCode", clientDepartment.QuestCode);
                    param[12] = new MySqlParameter("@ClearStarCode", clientDepartment.ClearStarCode);
                    param[13] = new MySqlParameter("@FormFoxCode", clientDepartment.FormFoxCode);

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                    sqlQuery = "INSERT INTO client_dept_test_categories (client_department_id, test_category_id,display_order,test_panel_price, is_synchronized, created_on, created_by, last_modified_on, last_modified_by) VALUES (@ClientDepartmentId, @TestCategoryId,@DisplayOrder,@TestPanelPrice, b'0', NOW(), @CreatedBy, NOW(), @LastModifiedBy)";

                    string sqlDelQuery = "DELETE FROM client_dept_test_categories WHERE client_department_id = @ClientDepartmentId AND test_category_id = @TestCategoryId";
                    string sqlDelQuery1 = "DELETE FROM client_dept_test_panels WHERE client_dept_test_category_id IN (SELECT client_dept_test_category_id FROM client_dept_test_categories WHERE client_department_id = @ClientDepartmentId AND test_category_id = @TestCategoryId)";

                    if (clientDepartment.IsUA)
                    {
                        bool flag = true;

                        foreach (ClientDeptTestCategory item in clientDepartment.ClientDeptTestCategories)
                        {
                            if (item.TestCategoryId == TestCategories.UA && item.ClientDeptTestCategoryId > 0)
                            {
                                flag = false;
                                break;
                            }
                        }

                        if (flag)
                        {
                            param = new MySqlParameter[6];
                            param[0] = new MySqlParameter("@ClientDepartmentId", clientDepartment.ClientDepartmentId);
                            param[1] = new MySqlParameter("@TestCategoryId", (int)TestCategories.UA);
                            param[2] = new MySqlParameter("@DisplayOrder", 1);
                            param[3] = new MySqlParameter("@TestPanelPrice", 0);
                            param[4] = new MySqlParameter("@CreatedBy", clientDepartment.CreatedBy);
                            param[5] = new MySqlParameter("@LastModifiedBy", clientDepartment.LastModifiedBy);

                            SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);
                        }
                    }
                    else
                    {
                        param = new MySqlParameter[2];
                        param[0] = new MySqlParameter("@ClientDepartmentId", clientDepartment.ClientDepartmentId);
                        param[1] = new MySqlParameter("@TestCategoryId", (int)TestCategories.UA);

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlDelQuery1, param);
                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlDelQuery, param);
                    }

                    if (clientDepartment.IsHair)
                    {
                        bool flag = true;

                        foreach (ClientDeptTestCategory item in clientDepartment.ClientDeptTestCategories)
                        {
                            if (item.TestCategoryId == TestCategories.Hair && item.ClientDeptTestCategoryId > 0)
                            {
                                flag = false;
                                break;
                            }
                        }

                        if (flag)
                        {
                            param = new MySqlParameter[6];
                            param[0] = new MySqlParameter("@ClientDepartmentId", clientDepartment.ClientDepartmentId);
                            param[1] = new MySqlParameter("@TestCategoryId", (int)TestCategories.Hair);
                            param[2] = new MySqlParameter("@DisplayOrder", 2);
                            param[3] = new MySqlParameter("@TestPanelPrice", 0);
                            param[4] = new MySqlParameter("@CreatedBy", clientDepartment.CreatedBy);
                            param[5] = new MySqlParameter("@LastModifiedBy", clientDepartment.LastModifiedBy);

                            SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);
                        }
                    }
                    else
                    {
                        param = new MySqlParameter[2];
                        param[0] = new MySqlParameter("@ClientDepartmentId", clientDepartment.ClientDepartmentId);
                        param[1] = new MySqlParameter("@TestCategoryId", (int)TestCategories.Hair);

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlDelQuery1, param);
                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlDelQuery, param);
                    }
                    if (clientDepartment.IsRecordKeeping)
                    {
                        var notimplemented = 0;
                    }

                    if (clientDepartment.IsDNA)
                    {
                        bool flag = true;

                        foreach (ClientDeptTestCategory item in clientDepartment.ClientDeptTestCategories)
                        {
                            if (item.TestCategoryId == TestCategories.DNA && item.ClientDeptTestCategoryId > 0)
                            {
                                flag = false;
                                break;
                            }
                        }

                        if (flag)
                        {
                            param = new MySqlParameter[6];
                            param[0] = new MySqlParameter("@ClientDepartmentId", clientDepartment.ClientDepartmentId);
                            param[1] = new MySqlParameter("@TestCategoryId", (int)TestCategories.DNA);
                            param[2] = new MySqlParameter("@DisplayOrder", 3);
                            param[3] = new MySqlParameter("@TestPanelPrice", 0);
                            param[4] = new MySqlParameter("@CreatedBy", clientDepartment.CreatedBy);
                            param[5] = new MySqlParameter("@LastModifiedBy", clientDepartment.LastModifiedBy);

                            SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);
                        }
                    }
                    else
                    {
                        param = new MySqlParameter[2];
                        param[0] = new MySqlParameter("@ClientDepartmentId", clientDepartment.ClientDepartmentId);
                        param[1] = new MySqlParameter("@TestCategoryId", (int)TestCategories.DNA);

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlDelQuery1, param);
                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlDelQuery, param);
                    }

                    if (clientDepartment.IsBC)
                    {
                        bool flag = true;

                        foreach (ClientDeptTestCategory item in clientDepartment.ClientDeptTestCategories)
                        {
                            if (item.TestCategoryId == TestCategories.BC && item.ClientDeptTestCategoryId > 0)
                            {
                                flag = false;
                                break;
                            }
                        }

                        if (flag)
                        {
                            param = new MySqlParameter[6];
                            param[0] = new MySqlParameter("@ClientDepartmentId", clientDepartment.ClientDepartmentId);
                            param[1] = new MySqlParameter("@TestCategoryId", (int)TestCategories.BC);
                            param[2] = new MySqlParameter("@DisplayOrder", 3);
                            param[3] = new MySqlParameter("@TestPanelPrice", 0);
                            param[4] = new MySqlParameter("@CreatedBy", clientDepartment.CreatedBy);
                            param[5] = new MySqlParameter("@LastModifiedBy", clientDepartment.LastModifiedBy);

                            SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);
                        }
                    }
                    else
                    {
                        param = new MySqlParameter[2];
                        param[0] = new MySqlParameter("@ClientDepartmentId", clientDepartment.ClientDepartmentId);
                        param[1] = new MySqlParameter("@TestCategoryId", (int)TestCategories.BC);

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlDelQuery1, param);
                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlDelQuery, param);
                    }
                    if (clientDepartment.IsRecordKeeping)
                    {
                        bool flag = true;

                        foreach (ClientDeptTestCategory item in clientDepartment.ClientDeptTestCategories)
                        {
                            if (item.TestCategoryId == TestCategories.RC && item.ClientDeptTestCategoryId > 0)
                            {
                                flag = false;
                                break;
                            }
                        }

                        if (flag)
                        {
                            param = new MySqlParameter[6];
                            param[0] = new MySqlParameter("@ClientDepartmentId", clientDepartment.ClientDepartmentId);
                            param[1] = new MySqlParameter("@TestCategoryId", (int)TestCategories.RC);
                            param[2] = new MySqlParameter("@DisplayOrder", 3);
                            param[3] = new MySqlParameter("@TestPanelPrice", 0);
                            param[4] = new MySqlParameter("@CreatedBy", clientDepartment.CreatedBy);
                            param[5] = new MySqlParameter("@LastModifiedBy", clientDepartment.LastModifiedBy);

                            SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);
                        }
                    }
                    else
                    {
                        param = new MySqlParameter[2];
                        param[0] = new MySqlParameter("@ClientDepartmentId", clientDepartment.ClientDepartmentId);
                        param[1] = new MySqlParameter("@TestCategoryId", (int)TestCategories.RC);

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlDelQuery1, param);
                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlDelQuery, param);
                    }
                    sqlQuery = "UPDATE client_contacts SET "
                                       + "client_contact_first_name = @ClientContactFirstName, "
                                       + "client_contact_last_name = @ClientContactLastName, "
                                       + "client_contact_phone = @ClientContactPhone, "
                                       + "client_contact_fax = @ClientContactFax, "
                                       + "client_contact_email = @ClientContactEmail, "
                                       + "is_synchronized = b'0', "
                                       + "last_modified_on = NOW(), "
                                       + "last_modified_by = @LastModifiedBy "
                                       + "WHERE client_contact_id = @ClientContactId";

                    if (clientDepartment.ClientContact != null)
                    {
                        param = new MySqlParameter[8];
                        param[0] = new MySqlParameter("@ClientContactId", clientDepartment.ClientContact.ClientContactId);
                        param[1] = new MySqlParameter("@ClientDepartmentId", clientDepartment.ClientDepartmentId);
                        param[2] = new MySqlParameter("@ClientContactFirstName", clientDepartment.ClientContact.ClientContactFirstName);
                        param[3] = new MySqlParameter("@ClientContactLastName", clientDepartment.ClientContact.ClientContactLastName);
                        param[4] = new MySqlParameter("@ClientContactPhone", clientDepartment.ClientContact.ClientContactPhone);
                        param[5] = new MySqlParameter("@ClientContactFax", clientDepartment.ClientContact.ClientContactFax);
                        param[6] = new MySqlParameter("@ClientContactEmail", clientDepartment.ClientContact.ClientContactEmail);
                        param[7] = new MySqlParameter("@LastModifiedBy", clientDepartment.LastModifiedBy);

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);
                    }

                    if (clientDepartment.ClientAddresses.Count > 0)
                    {
                        foreach (ClientAddress address in clientDepartment.ClientAddresses)
                        {
                            if (address.AddressId == 0)
                            {
                                sqlQuery = "INSERT INTO client_addresses ("
                                        + "client_department_id, "
                                        + "address_type_id, "
                                        + "client_address_1, "
                                        + "client_address_2, "
                                        + "client_city, "
                                        + "client_state, "
                                        + "client_zip, "
                                        + "is_synchronized, "
                                        + "created_on, "
                                        + "created_by, "
                                        + "last_modified_on, "
                                        + "last_modified_by "
                                        + ") VALUES ("
                                        + "@ClientDepartmentId, "
                                        + "@AddressTypeId, "
                                        + "@Address1, "
                                        + "@Address2, "
                                        + "@City, "
                                        + "@State, "
                                        + "@ZipCode, "
                                        + "b'0', "
                                        + "NOW(), "
                                        + "@CreatedBy, "
                                        + "NOW(), "
                                        + "@LastModifiedBy)";
                            }
                            else
                            {
                                sqlQuery = "UPDATE client_addresses SET "
                                        + "client_address_1 = @Address1, "
                                        + "client_address_2 = @Address2, "
                                        + "client_city = @City, "
                                        + "client_state = @State, "
                                        + "client_zip = @ZipCode, "
                                        + "is_synchronized = b'0', "
                                        + "last_modified_on = NOW(), "
                                        + "last_modified_by = @LastModifiedBy "
                                        + "WHERE client_address_id = @ClientAddressId";
                            }
                            param = new MySqlParameter[10];

                            param[0] = new MySqlParameter("@ClientAddressId", address.AddressId);
                            param[1] = new MySqlParameter("@ClientDepartmentId", clientDepartment.ClientDepartmentId);
                            param[2] = new MySqlParameter("@AddressTypeId", (int)address.AddressTypeId);
                            param[3] = new MySqlParameter("@Address1", address.Address1);
                            param[4] = new MySqlParameter("@Address2", address.Address2);
                            param[5] = new MySqlParameter("@City", address.City);
                            param[6] = new MySqlParameter("@State", address.State);
                            param[7] = new MySqlParameter("@ZipCode", address.ZipCode);
                            param[8] = new MySqlParameter("@CreatedBy", clientDepartment.LastModifiedBy);
                            param[9] = new MySqlParameter("@LastModifiedBy", clientDepartment.LastModifiedBy);

                            SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);
                        }
                    }

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

        public int UpdateClientDocumentType(ClientDocTypes dt)
        {
            int returnValue = 0;

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();

                try
                {
                    string sqlQuery = "UPDATE surpathlive.client_dept_doctypes SET   client_departmentdoctypeid = @client_departmentdoctypeid  ,client_department_id = @client_department_id  ,created_on = NULL   ,description = @description  ,instructions = @instructions   ,duedate = @duedate  ,semester = @semester, is_notifystudent = @is_notifystudent"
                                    + ",notifydays1 = @notifydays1  ,notifydays2 = @notifydays2   ,notifydays3 = @notifydays3  ,is_doesexpire = @is_doesexpire  ,is_required = @is_required  ,is_archived = @is_archived WHERE client_departmentdoctypeid = @client_departmentdoctypeid";

                    MySqlParameter[] param = new MySqlParameter[13];
                    param[0] = new MySqlParameter("@client_department_id", dt.ClientDepartmentId);
                    param[1] = new MySqlParameter("@description", dt.Description);
                    param[2] = new MySqlParameter("@instructions", dt.Instructions);
                    param[3] = new MySqlParameter("@duedate", (DateTime)dt.DueDate);
                    param[4] = new MySqlParameter("@semester", dt.Semester);
                    param[5] = new MySqlParameter("@is_notifystudent", (bool)dt.IsNotifyStudent);
                    param[6] = new MySqlParameter("@notifydays1", (int)dt.NotifyDays1);
                    param[7] = new MySqlParameter("@notifydays2", (Int32)dt.NotifyDays2);
                    param[8] = new MySqlParameter("@notifydays3", (Int32)dt.NotifyDays3);
                    param[9] = new MySqlParameter("@is_doesexpire", (bool)dt.IsDoesExpire);
                    param[10] = new MySqlParameter("@is_required", (bool)dt.IsRequired);
                    param[11] = new MySqlParameter("@client_departmentdoctypeid", (int)dt.ClientDoctypeId);
                    param[12] = new MySqlParameter("@is_archived", (bool)dt.IsArchived);

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

        /// <summary>
        /// Deletes the Client Department from database.
        /// </summary>
        /// <param name="clientDepartmentId">Client Department Id which one will be deleted.</param>
        /// <param name="currentUsername">Current username who is deleting the record.</param>
        /// <returns>Returns number of records deleted from the database.</returns>
        public int DeleteClientDepartment(int clientDepartmentId, string currentUsername)
        {
            int returnValue = 0;

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();
                try
                {
                    string sqlCount1Query = "Select count(*) from donors where donor_initial_department_id = " + clientDepartmentId + "";

                    int table1 = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlCount1Query));
                    if (table1 <= 0)
                    {
                        string sqlCount2Query = "Select count(*) from donor_test_info where client_department_id = " + clientDepartmentId + "";

                        int table2 = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlCount2Query));
                        if (table2 <= 0)
                        {
                            string sqlQuery = "UPDATE client_departments SET is_synchronized = b'0', is_archived = b'1', last_modified_on = NOW(), last_modified_by = @LastModifiedBy WHERE client_department_id = @ClientDepartmentId";

                            MySqlParameter[] param = new MySqlParameter[2];
                            param[0] = new MySqlParameter("@ClientDepartmentId", clientDepartmentId);
                            param[1] = new MySqlParameter("@LastModifiedBy", currentUsername);

                            SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                            trans.Commit();

                            returnValue = 1;
                        }
                    }
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

        /// <summary>
        /// Get the Client Department by ClientDepartmentId
        /// </summary>
        /// <param name="clientDepartmentId">Client Department Id which one need to be get from the database.</param>
        /// <returns>Returns Client Department information.</returns>
        public ClientDepartment GetClientDepartment(int clientDepartmentId)
        {
            ClientDepartment clientDepartment = null;

            string sqlQuery =
                @"
SELECT client_departments.client_department_id        AS ClientDepartmentId,
       client_departments.client_id                   AS ClientId,
       client_departments.department_name             AS DepartmentName,
       client_departments.lab_code                    AS LabCode,
       client_departments.QuestCode,
       client_departments.ClearStarCode,
       client_departments.FormFoxCode,
       client_departments.mro_type_id                 AS MROTypeId,
       client_departments.payment_type_id             AS PaymentTypeId,
       client_departments.is_department_active        AS IsDepartmentActive,
       client_departments.is_contact_info_as_client   AS IsContactInfoAsClient,
       client_departments.is_mailing_address_physical AS IsMailingAddressPhysical,
       client_departments.sales_representative_id     AS SalesRepresentativeId,
       client_departments.sales_comissions            AS SalesComissions,
       client_departments.is_synchronized             AS IsSynchronized,
       client_departments.is_archived                 AS IsArchived,
       client_departments.created_on                  AS CreatedOn,
       client_departments.created_by                  AS CreatedBy,
       client_departments.last_modified_on            AS LastModifiedOn,
       client_departments.last_modified_by            AS LastModifiedBy,
       IF(BIP.backend_integration_partner_client_map_id is not null, 1, IF(BIP.backend_integration_partner_id is not null, 1,0)) as IntegrationPartner,
       IF(BIP.backend_integration_partner_client_map_id is not null, BIP.require_login,0) as require_login, 
       IF(BIP.backend_integration_partner_client_map_id is not null, BIP.require_remote_login,0) as require_remote_login, 
       BIP.backend_integration_partner_id, 
       BIP.backend_integration_partner_client_map_id, 
       BIP2.login_url 
FROM client_departments
left outer join backend_integration_partner_client_map BIP on (BIP.client_department_id = client_departments.client_department_id OR BIP.client_department_id = 0) AND BIP.active >0 AND BIP.client_id = client_departments.client_id
LEFT OUTER JOIN backend_integration_partners BIP2 ON BIP.backend_integration_partner_id = BIP2.backend_integration_partner_id
WHERE client_departments.client_department_id = @clientDepartmentId
";

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                MySqlParameter[] param = new MySqlParameter[1];
                param[0] = new MySqlParameter("@ClientDepartmentId", clientDepartmentId);

                MySqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                if (dr.Read())
                {
                    clientDepartment = new ClientDepartment();
                    clientDepartment.ClientDepartmentId = (int)dr["ClientDepartmentId"];
                    clientDepartment.ClientId = (int)dr["ClientId"];
                    clientDepartment.DepartmentName = (string)dr["DepartmentName"];
                    clientDepartment.LabCode = (string)dr["LabCode"];
                    //try
                    //{
                    //    clientDepartment.QuestCode = (string)dr["QuestCode"];
                    //}
                    //catch(Exception ex)
                    //{
                    //    clientDepartment.QuestCode = null;
                    //}
                    try
                    {
                        object value = dr["QuestCode"];
                        if (value != DBNull.Value)
                        {
                            clientDepartment.QuestCode = (string)dr["QuestCode"];
                        }
                    }
                    catch (Exception ex)
                    {
                        clientDepartment.QuestCode = string.Empty;
                    }

                    //try
                    //{
                    //    clientDepartment.ClearStarCode = (string)dr["ClearStarCode"];
                    //}
                    //catch (Exception ex)
                    //{
                    //    clientDepartment.ClearStarCode = null;
                    //}

                    try
                    {
                        object value2 = dr["ClearStarCode"];
                        if (value2 != DBNull.Value)
                        {
                            clientDepartment.ClearStarCode = (string)dr["ClearStarCode"];
                        }
                    }
                    catch (Exception ex)
                    {
                        clientDepartment.ClearStarCode = string.Empty;
                    }

                    if (dr["FormFoxCode"] != DBNull.Value) clientDepartment.FormFoxCode = (string)dr["FormFoxCode"];

                    clientDepartment.MROTypeId = (ClientMROTypes)((int)dr["MROTypeId"]);
                    clientDepartment.PaymentTypeId = (ClientPaymentTypes)dr["PaymentTypeId"];
                    clientDepartment.IsPhysicalAddressAsClient = dr["IsContactInfoAsClient"].ToString() == "1" ? true : false;
                    clientDepartment.IsMailingAddressPhysical = dr["IsMailingAddressPhysical"].ToString() == "1" ? true : false;
                    clientDepartment.SalesRepresentativeId = dr["SalesRepresentativeId"].ToString() != string.Empty ? (int?)dr["SalesRepresentativeId"] : null;
                    clientDepartment.SalesComissions = dr["SalesComissions"].ToString() != string.Empty ? (int?)dr["SalesComissions"] : null;
                    clientDepartment.IsDepartmentActive = dr["IsDepartmentActive"].ToString() == "1" ? true : false;
                    clientDepartment.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    clientDepartment.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    clientDepartment.CreatedOn = (DateTime)dr["CreatedOn"];
                    clientDepartment.CreatedBy = (string)dr["CreatedBy"];
                    clientDepartment.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    clientDepartment.LastModifiedBy = (string)dr["LastModifiedBy"];


                    clientDepartment.integrationPartner = ((long)dr["IntegrationPartner"] == 1);
                    clientDepartment.requireLogin = ((long)dr["require_login"] == 1);
                    clientDepartment.require_remote_login = ((long)dr["require_remote_login"] == 1);
                    clientDepartment.backend_integration_partner_client_map_id = dr["backend_integration_partner_client_map_id"]== DBNull.Value ? 0 : (int)dr["backend_integration_partner_client_map_id"];
                    clientDepartment.backend_integration_partner_id = dr["backend_integration_partner_id"] == DBNull.Value ? 0 : (int)dr["backend_integration_partner_id"];
                    clientDepartment.login_url = dr["login_url"] == DBNull.Value ? String.Empty : (string)dr["login_url"];
                }
                dr.Close();

                if (clientDepartment != null)
                {
                    sqlQuery = "SELECT client_dept_test_category_id AS ClientDeptTestCategoryIdId, client_department_id AS ClientDepartmentId, test_category_id AS TestCategoryId,display_order AS DisplayOrder,test_panel_price AS TestPanelPrice , is_synchronized AS IsSynchronized, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM client_dept_test_categories WHERE client_department_id = @ClientDepartmentId ORDER BY display_order";

                    param = new MySqlParameter[1];
                    param[0] = new MySqlParameter("@ClientDepartmentId", clientDepartmentId);

                    dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);
                    while (dr.Read())
                    {
                        if (dr["TestCategoryId"].ToString() == ((int)TestCategories.UA).ToString())
                        {
                            clientDepartment.IsUA = true;
                        }
                        else if (dr["TestCategoryId"].ToString() == ((int)TestCategories.Hair).ToString())
                        {
                            clientDepartment.IsHair = true;
                        }
                        else if (dr["TestCategoryId"].ToString() == ((int)TestCategories.DNA).ToString())
                        {
                            clientDepartment.IsDNA = true;
                        }
                        else if (dr["TestCategoryId"].ToString() == ((int)TestCategories.BC).ToString())
                        {
                            clientDepartment.IsBC = true;
                        }
                    }
                    dr.Close();

                    if (clientDepartment != null)
                    {
                        #region Main Contact

                        sqlQuery = "SELECT "
                                            + "client_contact_id AS ClientContactId, "
                                            + "client_department_id AS ClientDepartmentId, "
                                            + "client_contact_first_name AS ClientContactFirstName, "
                                            + "client_contact_last_name AS ClientContactLastName, "
                                            + "client_contact_phone AS ClientContactPhone, "
                                            + "client_contact_fax AS ClientContactFax, "
                                            + "client_contact_email AS ClientContactEmail, "
                                            + "is_synchronized AS IsSynchronized, "
                                            + "created_on AS CreatedOn, "
                                            + "created_by AS CreatedBy, "
                                            + "last_modified_on AS LastModifiedOn, "
                                            + "last_modified_by AS LastModifiedBy "
                                            + "FROM client_contacts WHERE client_department_id = @ClientDepartmentId ORDER BY client_contact_first_name, client_contact_last_name";

                        param = new MySqlParameter[1];

                        param[0] = new MySqlParameter("@ClientDepartmentId", clientDepartmentId);

                        dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                        if (dr.Read())
                        {
                            clientDepartment.ClientContact = new ClientContact();

                            clientDepartment.ClientContact.ClientContactId = (int)dr["ClientContactId"];
                            clientDepartment.ClientContact.ClientDepartmentId = (int)dr["ClientDepartmentId"];
                            clientDepartment.ClientContact.ClientContactFirstName = (string)dr["ClientContactFirstName"];
                            clientDepartment.ClientContact.ClientContactLastName = (string)dr["ClientContactLastName"];
                            clientDepartment.ClientContact.ClientContactPhone = dr["ClientContactPhone"].ToString();
                            clientDepartment.ClientContact.ClientContactFax = dr["ClientContactFax"].ToString();
                            clientDepartment.ClientContact.ClientContactEmail = dr["ClientContactEmail"].ToString();
                            clientDepartment.ClientContact.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                            clientDepartment.ClientContact.CreatedOn = (DateTime)dr["CreatedOn"];
                            clientDepartment.ClientContact.CreatedBy = (string)dr["CreatedBy"];
                            clientDepartment.ClientContact.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                            clientDepartment.ClientContact.LastModifiedBy = (string)dr["LastModifiedBy"];

                            clientDepartment.MainContact = clientDepartment.ClientContact.ClientContactFirstName + " " + clientDepartment.ClientContact.ClientContactLastName;
                            clientDepartment.ClientPhone = clientDepartment.ClientContact.ClientContactPhone;
                            clientDepartment.ClientFax = clientDepartment.ClientContact.ClientContactFax;
                            clientDepartment.ClientEmail = clientDepartment.ClientContact.ClientContactEmail;
                        }
                        dr.Close();

                        #endregion Main Contact

                        #region Client Addresses

                        sqlQuery = "SELECT "
                                            + "client_address_id AS ClientAddressId, "
                                            + "client_department_id AS ClientDepartmentId, "
                                            + "address_type_id AS AddressTypeId, "
                                            + "client_address_1 AS ClientAddress1, "
                                            + "client_address_2 AS ClientAddress2, "
                                            + "client_city AS ClientCity, "
                                            + "client_state AS ClientState, "
                                            + "client_zip AS ClientZip, "
                                            + "is_synchronized AS IsSynchronized, "
                                            + "created_on AS CreatedOn, "
                                            + "created_by AS CreatedBy, "
                                            + "last_modified_on AS LastModifiedOn, "
                                            + "last_modified_by AS LastModifiedBy "
                                            + "FROM client_addresses WHERE client_department_id = @ClientDepartmentId ORDER BY address_type_id";

                        param = new MySqlParameter[1];
                        param[0] = new MySqlParameter("@ClientDepartmentId", clientDepartmentId);

                        dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                        while (dr.Read())
                        {
                            ClientAddress address = new ClientAddress();

                            address.AddressId = (int)dr["ClientAddressId"];
                            address.ClientDepartmentId = (int)dr["ClientDepartmentId"];
                            address.AddressTypeId = (AddressTypes)dr["AddressTypeId"];
                            address.Address1 = (string)dr["ClientAddress1"];
                            address.Address2 = dr["ClientAddress2"].ToString();
                            address.City = (string)dr["ClientCity"];
                            address.State = (string)dr["ClientState"];
                            address.ZipCode = (string)dr["ClientZip"];
                            address.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                            address.CreatedOn = (DateTime)dr["CreatedOn"];
                            address.CreatedBy = (string)dr["CreatedBy"];
                            address.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                            address.LastModifiedBy = (string)dr["LastModifiedBy"];

                            clientDepartment.ClientAddresses.Add(address);

                            if (address.AddressTypeId == AddressTypes.PhysicalAddress1)
                            {
                                clientDepartment.ClientCity = address.City;
                                clientDepartment.ClientState = address.State;
                            }
                        }
                        dr.Close();

                        #endregion Client Addresses
                    }
                }
            }

            return clientDepartment;
        }

        /// <summary>
        /// Get all the Client Department informations.
        /// </summary>
        /// <param name="clientId">Client Id</param>
        /// <returns>Returns Client Department information list.</returns>
        public DataTable GetClientDepartmentList(int clientId, string getInActive = null)
        {
            string sql = string.Empty;

            string sqlQuery = "SELECT "
            + "client_department_id AS ClientDepartmentId, "
            + "client_id AS ClientId, department_name AS DepartmentName, lab_code AS LabCode, "
            + "mro_type_id AS MROTypeId, "
            + "payment_type_id AS PaymentTypeId, "
            + "is_department_active AS IsDepartmentActive, "
            + "is_mailing_address_physical AS IsMailingAddressPhysical, "
            + "sales_representative_id AS SalesRepresentativeId, "
            + "sales_comissions AS SalesComissions, "
            + "is_synchronized AS IsSynchronized, "
            + "is_archived AS IsArchived, "
            + "created_on AS CreatedOn, "
            + "created_by AS CreatedBy, "
            + "last_modified_on AS LastModifiedOn, "
            + "last_modified_by AS LastModifiedBy ";

            if (getInActive == "1")
            {
                sql = "FROM client_departments WHERE is_archived = 0 AND client_id = @ClientId ORDER BY department_name";
            }
            else
            {
                sql = "FROM client_departments WHERE is_archived = 0 AND client_id = @ClientId AND is_department_active =  b'1' ORDER BY department_name";
            }

            MySqlParameter[] param = new MySqlParameter[1];
            param[0] = new MySqlParameter("@ClientId", clientId);
            sqlQuery = sqlQuery + sql;
            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sqlQuery, param);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get all the Client Department informations.
        /// </summary>
        /// <returns>Returns Client Department information list.</returns>
        public DataTable GetClientDepartmentList()
        {
            string sqlQuery = "SELECT client_department_id AS ClientDepartmentId, client_id AS ClientId, department_name AS DepartmentName, lab_code AS LabCode, mro_type_id AS MROTypeId, payment_type_id AS PaymentTypeId, is_department_active AS IsDepartmentActive, is_mailing_address_physical AS IsMailingAddressPhysical, sales_representative_id AS SalesRepresentativeId, "
                            + "sales_comissions AS SalesComissions, is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM client_departments WHERE is_archived = 0 AND is_department_active = b'1' ORDER BY client_id, department_name";

            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sqlQuery);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
            throw new NotImplementedException();
        }

        public DataTable GetClientDepartmentListByUserId(int currentUserId)
        {
            string sqlQuery = "SELECT client_department_id AS ClientDepartmentId, client_id AS ClientId, department_name AS DepartmentName, lab_code AS LabCode, mro_type_id AS MROTypeId, payment_type_id AS PaymentTypeId, is_department_active AS IsDepartmentActive, is_mailing_address_physical AS IsMailingAddressPhysical, sales_representative_id AS SalesRepresentativeId, "
                            + "sales_comissions AS SalesComissions, is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM client_departments WHERE is_archived = 0 AND is_department_active = b'1' "
                            + "AND client_department_id IN (SELECT client_department_id FROM user_departments WHERE user_id = @UserId) "
                            + "ORDER BY client_id, department_name";

            MySqlParameter[] param = new MySqlParameter[1];

            param[0] = new MySqlParameter("@UserId", currentUserId);

            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sqlQuery, param);

            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get all the Client Department informations based on the search criteria.
        /// </summary>
        /// <param name="searchParam">Collection of search criteria</param>
        /// <returns>Returns Client Department information list.</returns>
        public DataTable GetClientDepartmentList(int clientId, Dictionary<string, string> searchParam)
        {
            string sqlQuery = $@"SELECT 
                                client_department_id AS ClientDepartmentId, 
                                client_id AS ClientId, 
                                department_name AS DepartmentName,
                                lab_code AS LabCode, 
                                QuestCode, 
                                ClearStarCode, 
                                FormFoxCode, 
                                mro_type_id AS MROTypeId, 
                                payment_type_id AS PaymentTypeId, 
                                is_department_active AS IsDepartmentActive, 
                                is_mailing_address_physical AS IsMailingAddressPhysical, 
                                sales_representative_id AS SalesRepresentativeId, 
                                sales_comissions AS SalesComissions, 
                                is_synchronized AS IsSynchronized, 
                                is_archived AS IsArchived, 
                                created_on AS CreatedOn, 
                                created_by AS CreatedBy, 
                                last_modified_on AS LastModifiedOn, 
                                last_modified_by AS LastModifiedBy 
                                FROM 
                                client_departments 
                                WHERE 
                                is_archived = 0 AND 
                                client_id = {clientId} "; // '" + clientId + "' ";

            List<MySqlParameter> param = new List<MySqlParameter>();
            bool isInActiveFlag = false;

            foreach (KeyValuePair<string, string> searchItem in searchParam)
            {
                if (searchItem.Key == "GeneralSearch")
                {
                    sqlQuery += "AND (department_name LIKE @SearchKeyword "
                               + "OR lab_code LIKE @SearchKeyword )";

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
                sqlQuery += "AND is_department_active = b'1' ";
            }

            sqlQuery += " ORDER BY client_id ";

            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sqlQuery, param.ToArray());
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get all the Client Department - Test Categories informations based on the client department id.
        /// </summary>
        /// <param name="drugNameId">Drug name id</param>
        /// <returns>Returns Client Department  - Test Category information list.</returns>
        public DataTable GetClientDepartmentTestCategories(int clientDepartmentId)
        {
            string sqlQuery = "SELECT client_dept_test_category_id AS ClientDeptTestCategoryId, client_department_id AS ClientDepartmentId, test_category_id AS TestCategoryId, display_order AS DisplayOrder, test_panel_price AS TestPanelPrice, is_synchronized AS IsSynchronized, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM client_dept_test_categories WHERE client_department_id = @ClientDepartmentId ORDER BY display_order";

            MySqlParameter[] param = new MySqlParameter[1];
            param[0] = new MySqlParameter("@ClientDepartmentId", clientDepartmentId);

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
        /// Get all the Client Department - Test Categories - Test Panel informations based on the client department test category id.
        /// </summary>
        /// <param name="clientDepartmentTestCategoryId">Client Department Test Category Id</param>
        /// <returns>Returns Client Department  - Test Categories - Test Panel information list.</returns>
        public DataTable GetClientDepartmentTestPanels(int clientDepartmentTestCategoryId)
        {
            string sqlQuery = "SELECT "
                                + "client_dept_test_panel_id AS ClientDeptTestPanelId, "
                                + "client_dept_test_category_id AS ClientDeptTestCategoryId, "
                                + "client_dept_test_panels.test_panel_id AS TestPanelId, "
                                + "test_panels.test_panel_name AS TestPanelName, "
                                + "test_panel_price AS TestPanelPrice, "
                                + "display_order AS DisplayOrder, "
                                + "is_main_test_panel AS IsMainTestPanel, "
                                + "is_1_test_panel AS Is1TestPanel, "
                                + "is_2_test_panel AS Is2TestPanel, "
                                + "is_3_test_panel AS Is3TestPanel, "
                                + "is_4_test_panel As Is4TestPanel, "
                                + "client_dept_test_panels.is_synchronized AS IsSynchronized, "
                                + "client_dept_test_panels.created_on AS CreatedOn, "
                                + "client_dept_test_panels.created_by AS CreatedBy, "
                                + "client_dept_test_panels.last_modified_on AS LastModifiedOn, "
                                + "client_dept_test_panels.last_modified_by AS LastModifiedBy "
                                + "FROM client_dept_test_panels "
                                + "INNER JOIN test_panels ON client_dept_test_panels.test_panel_id = test_panels.test_panel_id "
                                + "WHERE client_dept_test_category_id = @ClientDepartmentTestCategoryId "
                                + "ORDER BY display_order";

            MySqlParameter[] param = new MySqlParameter[1];
            param[0] = new MySqlParameter("@ClientDepartmentTestCategoryId", clientDepartmentTestCategoryId);

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

        public DataTable GetClientDepartments(int clientDepartmentId)
        {
            string sqlQuery = "SELECT client_department_id AS ClientDepartmentId, client_id AS ClientId, department_name AS DepartmentName, lab_code AS LabCode, mro_type_id AS MROTypeId, payment_type_id AS PaymentTypeId, is_department_active AS IsDepartmentActive, is_mailing_address_physical AS IsMailingAddressPhysical, sales_representative_id AS SalesRepresentativeId, sales_comissions AS SalesComissions, is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM client_departments WHERE client_department_id = @clientDepartmentId";

            MySqlParameter[] param = new MySqlParameter[1];
            param[0] = new MySqlParameter("@clientDepartmentId", clientDepartmentId);

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

        public DataTable GetClientDepartmentDocTypes(int clientDepartmentId)
        {
            string sqlQuery = "select * from client_dept_doctypes where client_department_id = @clientDepartmentId  and is_archived = 0 order by  duedate, description";

            MySqlParameter[] param = new MySqlParameter[1];
            param[0] = new MySqlParameter("@ClientDepartmentId", clientDepartmentId);

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

        public DataTable GetClientDepartmentContact(int clientDepartmentId)
        {
            string sqlQuery = "SELECT "
                                + "client_contact_id AS ClientContactId, "
                                + "client_department_id AS ClientDepartmentId, "
                                + "client_contact_first_name AS ClientContactFirstName, "
                                + "client_contact_last_name AS ClientContactLastName, "
                                + "client_contact_phone AS ClientContactPhone, "
                                + "client_contact_fax AS ClientContactFax, "
                                + "client_contact_email AS ClientContactEmail, "
                                + "is_synchronized AS IsSynchronized, "
                                + "created_on AS CreatedOn, "
                                + "created_by AS CreatedBy, "
                                + "last_modified_on AS LastModifiedOn, "
                                + "last_modified_by AS LastModifiedBy "
                                + "FROM client_contacts WHERE client_department_id = @ClientDepartmentId ORDER BY client_contact_first_name, client_contact_last_name";

            MySqlParameter[] param = new MySqlParameter[1];
            param[0] = new MySqlParameter("@ClientDepartmentId", clientDepartmentId);

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

        public DataTable GetClientDepartmentAddresses(int clientDepartmentId)
        {
            string sqlQuery = "SELECT "
                            + "client_address_id AS ClientAddressId, "
                            + "client_department_id AS ClientDepartmentId, "
                            + "address_type_id AS AddressTypeId, "
                            + "client_address_1 AS ClientAddress1, "
                            + "client_address_2 AS ClientAddress2, "
                            + "client_city AS ClientCity, "
                            + "client_state AS ClientState, "
                            + "client_zip AS ClientZip, "
                            + "is_synchronized AS IsSynchronized, "
                            + "created_on AS CreatedOn, "
                            + "created_by AS CreatedBy, "
                            + "last_modified_on AS LastModifiedOn, "
                            + "last_modified_by AS LastModifiedBy "
                            + "FROM client_addresses WHERE client_department_id = @ClientDepartmentId ORDER BY address_type_id";

            MySqlParameter[] param = new MySqlParameter[1];
            param[0] = new MySqlParameter("@ClientDepartmentId", clientDepartmentId);

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

        //Start here
        public DataTable GetClientDashboard(string clientdepartmentID)
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
                            + "WHERE client_department_id IN (" + clientdepartmentID + ")"
                            + "GROUP BY client_id, client_department_id ";

                sqlQuery = @"
select
cd.client_id AS ClientId, cd.client_department_id AS ClientDepartmentId,
SUM(CASE WHEN test_status = 1 THEN 1 ELSE 0 END) AS PreregistrationCount, 
SUM(CASE WHEN test_status = 2 THEN 1 ELSE 0 END) AS ActivatedCount, 
SUM(CASE WHEN test_status = 3 THEN 1 ELSE 0 END) AS RegisteredCount, 
SUM(CASE WHEN test_status = 4 THEN 1 ELSE 0 END) AS InQueueCount, 
SUM(CASE WHEN test_status = 5 THEN 1 ELSE 0 END) AS SuspensionQueueCount, 
SUM(CASE WHEN test_status = 6 THEN 1 ELSE 0 END) AS ProcessingCount, 
SUM(CASE WHEN test_status = 7 THEN 1 ELSE 0 END) AS CompletedCount 
from client_departments cd
left outer join donor_test_info dti on cd.client_department_id = dti.client_department_id
where cd.client_department_id in (" + clientdepartmentID + @")
group by cd.client_id, cd.client_department_id;

";



                MySqlParameter[] param = new MySqlParameter[1];
                param[0] = new MySqlParameter("@ClientDepartmentId", clientdepartmentID);

                DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sqlQuery, param);

                DataTable dtPerformance = ds.Tables[0];

                return dtPerformance;
            }
        }

        public DataTable GetDonorId(string departmentId, string teststatus)
        {
            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                string sqlQuery = "SELECT "
                                + "donors.donor_id AS DonorId, "
                                + "donors.donor_first_name AS DonorFirstName, "
                                + "donors.donor_last_name AS DonorLastName, "
                                + "donors.donor_ssn AS DonorSSN, "
                                + "donors.donor_date_of_birth AS DonorDateOfBirth, "
                                + "donors.donor_initial_client_id AS DonorInitialClientId, "
                                + "donors.donor_registration_status AS DonorRegistrationStatus, "
                                + "donors.donor_city as DonorCity, "
                                + "donors.donor_email as DonorEmail, "
                                + "donors.donor_zip as DonorZipCode, "
                                + "donors.is_archived AS IsDonorArchived, "
                                + "donor_test_info.donor_test_info_id AS DonorTestInfoId, "
                                + "donor_test_info.client_id AS TestInfoClientId, "
                                + "donor_test_info.client_department_id AS TestInfoDepartmentId, "
                                + "donor_test_info.mro_type_id AS MROTypeId, "
                                + "donor_test_info.payment_type_id AS PaymentTypeId, "
                                + "donor_test_info.test_requested_date AS TestRequestedDate, "
                                + "donor_test_info.reason_for_test_id AS ReasonForTestId, "
                                + "donor_test_info.total_payment_amount AS TotalPaymentAmount, "
                                + "donor_test_info.payment_method_id AS PaymentMethodId, "
                                + "donor_test_info.test_status AS TestStatus, "
                                + "donor_test_info.test_overall_result AS TestOverallResult, "
                                + "donor_test_info.is_walkin_donor AS IsWalkinDonor, "
                                + "donor_test_info.instant_test_result AS InstantTestResult, "
                                + "donor_test_info.is_instant_test AS IsInstantTest, "
                                + "donor_test_info.is_donor_refused AS IsDonorRefused, "
                                + "CONCAT_WS(' ', users.user_first_name, users.user_last_name) AS CollectorName, "
                                //+ "donor_test_info_test_categories.test_category_id AS TestCategoryId, "
                                //+ "donor_test_info_test_categories.test_panel_id AS TestPanelId, "
                                //+ "donor_test_info_test_categories.specimen_id AS SpecimenId, "
                                + "(select GROUP_CONCAT(donor_test_info_test_categories.specimen_id SEPARATOR ', ') from donor_test_info_test_categories where donor_test_info_test_categories.donor_test_info_id = donor_test_info.donor_test_info_id) AS SpecimenId, "
                                + "donor_test_info.screening_time AS SpecimenDate, "
                               //+ "donor_test_info_test_categories.hair_test_panel_days AS HairTestPanelDays, "
                               + "clients.client_name AS ClientName, "
                                + "client_departments.department_name AS ClientDepartmentName, "
                                + "client_departments.clearstarcode As ClearStarCode, "
                                + "dt.DocsTotal, dt.DocsNotApproved, dt.DocsRejected "
                                + "FROM donors "
                                //  + "LEFT OUTER JOIN donor_test_info ON donors.donor_id = donor_test_info.donor_id AND donor_test_info.donor_test_info_id = (SELECT max(donor_test_info.donor_test_info_id) FROM donor_test_info WHERE donor_test_info.donor_id = donors.donor_id) "
                                + "LEFT OUTER JOIN donor_test_info ON donors.donor_id = donor_test_info.donor_id "
                                //  + "LEFT OUTER JOIN donor_test_info_test_categories ON donor_test_info.donor_test_info_id = donor_test_info_test_categories.donor_test_info_id "
                                + "LEFT OUTER JOIN clients ON (donor_test_info.client_id = clients.client_id) " //(donors.donor_initial_client_id = clients.client_id AND donors.donor_initial_department_id IS NULL) OR
                                                                                                                // + "LEFT OUTER JOIN clients ON donors.donor_initial_client_id = clients.client_id AND donors.donor_initial_department_id IS NULL "
                                + "LEFT OUTER JOIN client_departments ON donor_test_info.client_department_id = client_departments.client_department_id "
                                + "LEFT OUTER JOIN users ON donor_test_info.collection_site_user_id = users.user_id "
                                + "LEFT OUTER JOIN donordocumenttotals dt on dt.donor_id = donor_test_info.donor_id "
                                + "WHERE test_status >3 AND test_status IN (@TestStatus) AND client_departments.client_department_id = @ClientDepartmentId;";

                MySqlParameter[] param = new MySqlParameter[2];
                param[0] = new MySqlParameter("@ClientDepartmentId", departmentId);
                param[1] = new MySqlParameter("@TestStatus", teststatus);

                DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sqlQuery, param);

                DataTable dtPerformance = ds.Tables[0];

                return dtPerformance;
            }
        }

        public DataTable ClientDepartmentsget(int clientDepartmentId)
        {
            ClientDeptTestPanel clientDepartment = null;

            string sqlQuery = "Select MAX(client_dept_test_panels.client_dept_test_panel_id) as ClientDeptTestPanelId,client_dept_test_categories.test_category_id,client_dept_test_panels.test_panel_id as TestPanelId,client_dept_test_panels.test_panel_price as TestPanelPrice from client_dept_test_panels "
                            + "inner join client_dept_test_categories on client_dept_test_panels.client_dept_test_category_id =client_dept_test_categories.client_dept_test_category_id "
                            + "inner join client_departments on client_dept_test_categories.client_department_id =client_departments.client_department_id "
                            + "where client_departments.client_department_id = @clientDepartmentId";

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                MySqlParameter[] param = new MySqlParameter[1];
                param[0] = new MySqlParameter("@ClientDepartmentId", clientDepartmentId);

                MySqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                if (dr.Read())
                {
                    clientDepartment = new ClientDeptTestPanel();
                    clientDepartment.ClientDeptTestPanelId = (int)dr["ClientDeptTestPanelId"];
                }

                sqlQuery = "SELECT "
                         + "client_dept_test_panel_id AS ClientDeptTestPanelId, "
                         + "client_dept_test_category_id AS ClientDeptTestCategoryId, "
                         + "client_dept_test_categories.test_category_id AS TestCategoryId, "
                         + "client_dept_test_panels.test_panel_id AS TestPanelId, "
                         + "test_panel_price AS TestPanelPrice, "
                         + "display_order AS DisplayOrder, "
                         + "is_main_test_panel AS IsMainTestPanel, "
                         + "is_1_test_panel AS Is1TestPanel, "
                         + "is_2_test_panel AS Is2TestPanel, "
                         + "is_3_test_panel AS Is3TestPanel, "
                         + "is_4_test_panel As Is4TestPanel, "
                         + "client_dept_test_panels.is_synchronized AS IsSynchronized, "
                         + "client_dept_test_panels.created_on AS CreatedOn, "
                         + "client_dept_test_panels.created_by AS CreatedBy, "
                         + "client_dept_test_panels.last_modified_on AS LastModifiedOn, "
                         + "client_dept_test_panels.last_modified_by AS LastModifiedBy "
                         + "FROM client_dept_test_panels "
                         + "inner join client_dept_test_categories on client_dept_test_panels.client_dept_test_category_id =client_dept_test_categories.client_dept_test_category_id "
                         + "where client_dept_test_panels.client_dept_test_panel_id = " + clientDepartment.ClientDeptTestPanelId + "";

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

        public DataTable ClientDepartments(int clientDepartmentId)
        {
            string sqlQuery = "Select client_dept_test_categories.test_category_id as TestCategoryId,client_dept_test_panels.test_panel_id as TestPanelId,client_dept_test_panels.test_panel_price as TestPanelPrice from client_dept_test_panels "
                            + "inner join client_dept_test_categories on client_dept_test_panels.client_dept_test_category_id =client_dept_test_categories.client_dept_test_category_id "
                            + "inner join client_departments on client_dept_test_categories.client_department_id =client_departments.client_department_id "
                            + "where client_departments.client_department_id= @ClientDepartmentId";

            MySqlParameter[] param = new MySqlParameter[1];

            param[0] = new MySqlParameter("@ClientDepartmentId", clientDepartmentId);

            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sqlQuery, param);

            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
            throw new NotImplementedException();
        }

        public DataTable BCClientDepartments(int clientDepartmentId)
        {
            string sqlQuery = "Select client_dept_test_categories.test_category_id as TestCategoryId,client_dept_test_panels.test_panel_id as TestPanelId,client_dept_test_panels.test_panel_price as TestPanelPrice from client_dept_test_panels "
                            + "inner join client_dept_test_categories on client_dept_test_panels.client_dept_test_category_id =client_dept_test_categories.client_dept_test_category_id "
                            + "inner join client_departments on client_dept_test_categories.client_department_id =client_departments.client_department_id "
                            + "where client_departments.client_department_id= @ClientDepartmentId";

            MySqlParameter[] param = new MySqlParameter[1];

            param[0] = new MySqlParameter("@ClientDepartmentId", clientDepartmentId);

            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sqlQuery, param);

            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
            throw new NotImplementedException();
        }

        public DataTable DNAClientDepartments(int clientDepartmentId)
        {
            string sqlQuery = "Select client_dept_test_categories.test_category_id as TestCategoryId,client_dept_test_categories.test_panel_price as TestPanelPrice from client_dept_test_categories "
                            // + "inner join client_dept_test_categories on client_dept_test_panels.client_dept_test_category_id =client_dept_test_categories.client_dept_test_category_id "
                            + "inner join client_departments on client_dept_test_categories.client_department_id =client_departments.client_department_id "
                            + "where client_departments.client_department_id= @ClientDepartmentId";

            MySqlParameter[] param = new MySqlParameter[1];

            param[0] = new MySqlParameter("@ClientDepartmentId", clientDepartmentId);

            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sqlQuery, param);

            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
            throw new NotImplementedException();
        }

        public DataTable GetByLabCode(string labCode)
        {
            string sqlQuery = "SELECT client_department_id AS ClientDepartmentId, client_id AS ClientId, department_name AS DepartmentName, lab_code AS LabCode, mro_type_id AS MROTypeId, payment_type_id AS PaymentTypeId, is_department_active AS IsDepartmentActive, is_contact_info_as_client AS IsContactInfoAsClient, is_mailing_address_physical AS IsMailingAddressPhysical, sales_representative_id AS SalesRepresentativeId, sales_comissions AS SalesComissions, is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM client_departments WHERE UPPER(lab_code) = UPPER(@LabCode) ORDER BY department_name";

            MySqlParameter[] param = new MySqlParameter[1];
            param[0] = new MySqlParameter("@LabCode", labCode);

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

        public DataTable sortingClientDepartment(int clientId, string searchparam, bool active, string getInActive = null)
        {
            string sql = string.Empty;
            string sqlQuery = "SELECT client_department_id AS ClientDepartmentId, client_id AS ClientId, department_name AS DepartmentName,lab_code AS LabCode, mro_type_id AS MROTypeId, payment_type_id AS PaymentTypeId, is_department_active AS IsDepartmentActive, is_mailing_address_physical AS IsMailingAddressPhysical, sales_representative_id AS SalesRepresentativeId, sales_comissions AS SalesComissions, is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy "
                              + "FROM client_departments WHERE is_archived = 0 AND client_id = '" + clientId + "' ";

            if (active == false)
            {
                sqlQuery += "AND is_department_active = b'1'";
            }

            if (searchparam == "clientDepartmentName")
            {
                if (getInActive == "1")
                {
                    sql = "ORDER BY department_name";
                }
                else
                {
                    sql = "ORDER BY department_name desc";
                }
            }
            if (searchparam == "isMROType")
            {
                if (getInActive == "1")
                {
                    sql = "ORDER BY mro_type_id";
                }
                else
                {
                    sql = "ORDER BY mro_type_id desc";
                }
            }
            if (searchparam == "isPaymentType")
            {
                if (getInActive == "1")
                {
                    sql = "ORDER BY payment_type_id";
                }
                else
                {
                    sql = "ORDER BY payment_type_id desc";
                }
            }
            if (searchparam == "isDepartmentActive")
            {
                if (getInActive == "1")
                {
                    sql = "ORDER BY is_department_active ";
                }
                else
                {
                    sql = "ORDER BY is_department_active desc";
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

        //End here

        #endregion ClientDept Public Methods
    }
}