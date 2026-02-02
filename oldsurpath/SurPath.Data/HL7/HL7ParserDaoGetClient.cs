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
    /// <summary>
    /// Gets Client
    /// </summary>
    public partial class HL7ParserDao : DataObject
    {

        public DataTable GetMismatchedData(string mismatchedIds)
        {
            string sql = string.Empty;

            string sqlQuery = "SELECT "

                            + "specimen_id AS SpecimenId, "
                            + "donor_full_name AS DonorName, "
                            + "client_code AS ClientCode, "
                            + "date_of_test AS DateOfTest, "
                            + "ssn_id AS SSNId, "
                            + "donor_dob AS DonorDOB FROM mismatched_reports WHERE mismatched_report_id in (" + mismatchedIds + ")";

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

        #region Private Methods

        private Client GetClient(int clientId, MySqlTransaction trans)
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
                            + "last_modified_by AS LastModifiedBy "
                            + "FROM clients WHERE client_id = @ClientId";

            MySqlParameter[] param = new MySqlParameter[1];
            param[0] = new MySqlParameter("@ClientId", clientId);

            MySqlDataReader dr = SqlHelper.ExecuteReader(trans, CommandType.Text, sqlQuery, param);

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

            return client;
        }

        private void GetClientDetails(ReportInfo reportDetails, ReturnValues returnValues, MySqlTransaction trans, string FileName, int NumPassed, int TotalTo, ReportType reportType)
        {
            _logger.Information($"(1) Getting Client Details for: {reportDetails.SpecimenId}");
            var qcode = reportDetails.QuestCode;
            if (!string.IsNullOrEmpty(reportDetails.CrlClientCode))
            {
                ParamHelper param = new ParamHelper();
                string sqlQuery = string.Empty;

                if (string.IsNullOrEmpty(returnValues.ClientDepartmentId.ToString())) returnValues.ClientDepartmentId = 0;
                if (string.IsNullOrEmpty(returnValues.ClientId.ToString())) returnValues.ClientId = 0;
                param.reset();
                if (returnValues.ClientDepartmentId > 0 && returnValues.ClientId > 0)
                {
                    _logger.Debug($"We already have the client ID and the Dept ID: ClientID: {returnValues.ClientId} DeptID {returnValues.ClientDepartmentId}");

                    sqlQuery = @"
                                SELECT 
                                cd.client_department_id AS ClientDepartmentId, 
                                cd.department_name as department_name, 
                                cd.client_id AS ClientId, 
                                c.client_name as client_name
                                FROM client_departments cd
                                left outer join clients c on c.client_id = cd.client_id
                                WHERE cd.is_archived = 0 AND c.client_id = @client_id AND cd.client_department_id = @client_department_id
                                ";

                    sqlQuery = @"                                
                                SELECT
                                cd.client_department_id AS ClientDepartmentId, 
                                cd.department_name as department_name, 
                                cd.client_id AS ClientId, 
                                c.client_name as client_name,
                                IF(bipm.backend_integration_partner_client_map_id is null, 0,1) as IntegrationPartner
                                FROM client_departments cd
                                left outer join clients c on c.client_id = cd.client_id
                                left outer join backend_integration_partner_client_map bipm on c.client_id = bipm.client_id and bipm.active > 0
                                    and ( cd.client_department_id = bipm.client_department_id OR bipm.client_department_id = 0)
                                WHERE cd.is_archived = 0 AND c.client_id = @client_id AND cd.client_department_id = @client_department_id
                            ";

                    param.Param = new MySqlParameter("@client_id", returnValues.ClientId);

                    param.Param = new MySqlParameter("@client_department_id", returnValues.ClientDepartmentId);

                }
                else if(reportType == ReportType.QuestLabReport && !(string.IsNullOrEmpty(reportDetails.QuestCode)))
                {
                    sqlQuery = @"                                
                                SELECT
                                cd.client_department_id AS ClientDepartmentId, 
                                cd.department_name as department_name, 
                                cd.client_id AS ClientId, 
                                c.client_name as client_name,
                                IF(bipm.backend_integration_partner_client_map_id is null, 0,1) as IntegrationPartner
                                FROM client_departments cd
                                left outer join clients c on c.client_id = cd.client_id
                                left outer join backend_integration_partner_client_map bipm on c.client_id = bipm.client_id and bipm.active > 0
                                    and ( cd.client_department_id = bipm.client_department_id OR bipm.client_department_id = 0)
                                WHERE cd.is_archived = 0 AND (UPPER(cd.QuestCode) = UPPER(@LabCode))
                            ";

                    param.Param = new MySqlParameter("@LabCode", reportDetails.QuestCode);


                }
                else
                {
                    _logger.Debug($"We don't have the client ID or the Dept ID: ClientID: {returnValues.ClientId} DeptID {returnValues.ClientDepartmentId}");

                    _logger.Debug($"reportDetails.CrlClientCode is not empty: {reportDetails.CrlClientCode}");
                    //sqlQuery = "SELECT "
                    //            + "client_department_id AS ClientDepartmentId, "
                    //            + "department_name as department_name, "
                    //            + "client_id AS ClientId "
                    //            + "FROM client_departments "
                    //            + "WHERE is_archived = 0 AND UPPER(lab_code) = UPPER(@LabCode)";

                    //sqlQuery = @"
                    //            SELECT 
                    //            cd.client_department_id AS ClientDepartmentId, 
                    //            cd.department_name as department_name, 
                    //            cd.client_id AS ClientId, 
                    //            c.client_name as client_name
                    //            FROM client_departments cd
                    //            left outer join clients c on c.client_id = cd.client_id
                    //            WHERE cd.is_archived = 0 AND UPPER(cd.lab_code) = UPPER(@LabCode)
                    //            ";

                    // this is for formfox, who mangles the labcode
                    sqlQuery = @"
                                SELECT 
                                cd.client_department_id AS ClientDepartmentId, 
                                cd.department_name as department_name, 
                                cd.client_id AS ClientId, 
                                c.client_name as client_name
                                FROM client_departments cd
                                left outer join clients c on c.client_id = cd.client_id
                                WHERE cd.is_archived = 0 AND (UPPER(cd.lab_code) = UPPER(@LabCode) OR UPPER(cd.FormFoxCode) = UPPER(@LabCode))
                                ";

                    sqlQuery = @"                                
                                SELECT
                                cd.client_department_id AS ClientDepartmentId, 
                                cd.department_name as department_name, 
                                cd.client_id AS ClientId, 
                                c.client_name as client_name,
                                IF(bipm.backend_integration_partner_client_map_id is null, 0,1) as IntegrationPartner
                                FROM client_departments cd
                                left outer join clients c on c.client_id = cd.client_id
                                left outer join backend_integration_partner_client_map bipm on c.client_id = bipm.client_id and bipm.active > 0
                                    and ( cd.client_department_id = bipm.client_department_id OR bipm.client_department_id = 0)
                                WHERE cd.is_archived = 0 AND (UPPER(cd.lab_code) = UPPER(@LabCode) OR UPPER(cd.FormFoxCode) = UPPER(@LabCode))
                            ";

                    param.Param = new MySqlParameter("@LabCode", reportDetails.CrlClientCode);
                }
                //MySqlParameter[] param = new MySqlParameter[1];
                //param.reset();
                
                //param[0] = new MySqlParameter("@LabCode", reportDetails.LabCode);
                //_logger.Information("GetClientDetails Query1:" + sqlQuery);
                try
                {
                    _logger.Information("LabCode:" + reportDetails.CrlClientCode.ToString());
                }
                catch (Exception)
                {
                    _logger.Information("LabCode:" + "null");
                }


                MySqlDataReader dr1 = SqlHelper.ExecuteReader(trans, CommandType.Text, sqlQuery, param.Params);

                if (dr1.Read())
                {
                    _logger.Information("GetClientDetails has records: " + dr1.HasRows.ToString());
                    returnValues.ClientDepartmentId = Convert.ToInt32(dr1["ClientDepartmentId"]);
                    returnValues.ClientId = Convert.ToInt32(dr1["ClientId"]);
                    returnValues.IntegrationPartner = Convert.ToInt32(dr1["IntegrationPartner"]) > 0;
                    reportDetails.ClientDepartmentId = returnValues.ClientDepartmentId;
                    reportDetails.ClientId = returnValues.ClientId;
                    reportDetails.ClientDepartmentName = dr1["department_name"].ToString();
                    reportDetails.ClientName = dr1["client_name"].ToString();
                    _logger.Information($"Getting Client Details for: {reportDetails.SpecimenId} - Client Identified - {reportDetails.ClientName} {reportDetails.ClientDepartmentName}");
                }
                else
                {
                    _logger.Information($"Getting Client Details for: {reportDetails.SpecimenId} - Client Not Identified");

                }
                dr1.Close();
                // We could find the client and the department by specimen - but if that existed (donor test info) the lab code would exist
                // We could potentially add the lab code here, but if was used by multiple clients
                // We would have a problem
                // For example, 0VN.MPOS.DISCARD


                //if (returnValues.ClientDepartmentId < 1)
                //{
                //    // we didn't find them - let's try by specimen id
                //    if (!(string.IsNullOrEmpty(reportDetails.SpecimenId)))
                //    {

                //    }
                //}

                if (returnValues.ClientDepartmentId > 0)
                {
                    if (reportDetails.TestPanelCode != string.Empty)
                    {
                        sqlQuery = "SELECT "
                            + "client_dept_test_panel_id AS ClientDeptTestPanelId "
                            + "FROM client_dept_test_panels "
                            + "INNER JOIN client_dept_test_categories ON client_dept_test_categories.client_dept_test_category_id = client_dept_test_panels.client_dept_test_category_id "
                            + "INNER JOIN test_panels ON test_panels.test_panel_id = client_dept_test_panels.test_panel_id "
                            + "WHERE client_dept_test_categories.test_category_id = 1 "
                            + "AND client_dept_test_categories.client_department_id = @ClientDepartmentId ";
                        //+ "AND test_panels.test_panel_name IN ({0})";
                        //this removed test of test panel check

                        /////////////////////////////////
                        if (reportType == ReportType.QuestLabReport)
                        {
                            reportDetails.TestPanelCode = "T719, T819, V909";
                        }
                        string[] tmpTestPanelCode = reportDetails.TestPanelCode.Split(',');
                        string testPanelCode = string.Empty;
                        foreach (string tmp in tmpTestPanelCode)
                        {
                            if (testPanelCode != string.Empty)
                            {
                                testPanelCode += ", '" + tmp.Trim() + "'";
                            }
                            else
                            {
                                testPanelCode += "'" + tmp.Trim() + "'";
                            }
                        }

                        if (testPanelCode == string.Empty)
                        {
                            testPanelCode = "'0'";
                        }

                        sqlQuery = string.Format(sqlQuery, testPanelCode);

                        param.reset(); // = new MySqlParameter[1];
                        param.Param = new MySqlParameter("@ClientDepartmentId", reportDetails.ClientDepartmentId);
                        //param[1] = new MySqlParameter("@TestPanelCode", testPanelCode);

                        dr1 = SqlHelper.ExecuteReader(trans, CommandType.Text, sqlQuery, param.Params);

                        if (dr1.Read())
                        {
                            var blah = dr1["ClientDeptTestPanelId"];
                            returnValues.ClientDeptTestPanelId = Convert.ToInt32(dr1["ClientDeptTestPanelId"]);
                            _logger.Information($"Getting Client Details for: {reportDetails.SpecimenId} - Client Identified - {reportDetails.ClientName} {reportDetails.ClientDepartmentName} - Test Panel ID: {returnValues.ClientDeptTestPanelId}");

                        }
                        dr1.Close();

                        if (returnValues.ClientDeptTestPanelId == 0)
                        {
                            returnValues.ErrorFlag = true;
                            returnValues.ErrorMessage = "Test Panel Name does not match.";
                        }
                    }
                }
                else
                {
                    // WriteClientLogFile(reportDetails, returnValues, FileName, NumPassed, TotalTo);
                }
            }
        }

        private ClientDepartment GetClientDepartment(ReturnValues returnValues, MySqlTransaction trans)
        {
            ClientDepartment clientDepartment = GetClientDepartment(returnValues.ClientDepartmentId, trans);

            if (clientDepartment != null)
            {
                DataTable dtClientDepartmentCategories = GetClientDepartmentTestCategories(clientDepartment.ClientDepartmentId, trans);

                if (dtClientDepartmentCategories != null)
                {
                    foreach (
                        DataRow drClientDeptTestCategory in dtClientDepartmentCategories.Rows)
                    {
                        ClientDeptTestCategory clientDeptTestCategory = new ClientDeptTestCategory();

                        clientDeptTestCategory.ClientDeptTestCategoryId = (int)drClientDeptTestCategory["ClientDeptTestCategoryId"];
                        clientDeptTestCategory.ClientDepartmentId = (int)drClientDeptTestCategory["ClientDepartmentId"];
                        clientDeptTestCategory.TestCategoryId = (TestCategories)drClientDeptTestCategory["TestCategoryId"];
                        clientDeptTestCategory.DisplayOrder = (int)drClientDeptTestCategory["DisplayOrder"];
                        clientDeptTestCategory.TestPanelPrice = drClientDeptTestCategory["TestPanelPrice"].ToString() != string.Empty ? (double?)drClientDeptTestCategory["TestPanelPrice"] : null;
                        clientDeptTestCategory.IsSynchronized = drClientDeptTestCategory["IsSynchronized"].ToString() == "1" ? true : false;
                        clientDeptTestCategory.CreatedOn = (DateTime)drClientDeptTestCategory["CreatedOn"];
                        clientDeptTestCategory.CreatedBy = (string)drClientDeptTestCategory["CreatedBy"];
                        clientDeptTestCategory.LastModifiedOn = (DateTime)drClientDeptTestCategory["LastModifiedOn"];
                        clientDeptTestCategory.LastModifiedBy = (string)drClientDeptTestCategory["LastModifiedBy"];

                        DataTable dtClientDeptTestPanels = GetClientDepartmentTestPanels(clientDeptTestCategory.ClientDeptTestCategoryId, trans);

                        foreach (DataRow drClientDeptTestPanel in dtClientDeptTestPanels.Rows)
                        {
                            ClientDeptTestPanel clientDeptTestPanel = new ClientDeptTestPanel();

                            clientDeptTestPanel.ClientDeptTestPanelId = (int)drClientDeptTestPanel["ClientDeptTestPanelId"];
                            clientDeptTestPanel.ClientDeptTestCategoryId = (int)drClientDeptTestPanel["ClientDeptTestCategoryId"];
                            clientDeptTestPanel.TestPanelId = (int)drClientDeptTestPanel["TestPanelId"];
                            clientDeptTestPanel.TestPanelName = (string)drClientDeptTestPanel["TestPanelName"];
                            clientDeptTestPanel.TestPanelPrice = (double)drClientDeptTestPanel["TestPanelPrice"];
                            clientDeptTestPanel.DisplayOrder = (int)drClientDeptTestPanel["DisplayOrder"];
                            clientDeptTestPanel.IsMainTestPanel = drClientDeptTestPanel["IsMainTestPanel"].ToString() == "1" ? true : false;
                            clientDeptTestPanel.Is1TestPanel = drClientDeptTestPanel["Is1TestPanel"].ToString() == "1" ? true : false;
                            clientDeptTestPanel.Is2TestPanel = drClientDeptTestPanel["Is2TestPanel"].ToString() == "1" ? true : false;
                            clientDeptTestPanel.Is3TestPanel = drClientDeptTestPanel["Is3TestPanel"].ToString() == "1" ? true : false;
                            clientDeptTestPanel.Is4TestPanel = drClientDeptTestPanel["Is4TestPanel"].ToString() == "1" ? true : false;
                            clientDeptTestPanel.IsSynchronized = drClientDeptTestPanel["IsSynchronized"].ToString() == "1" ? true : false;
                            clientDeptTestPanel.CreatedOn = (DateTime)drClientDeptTestPanel["CreatedOn"];
                            clientDeptTestPanel.CreatedBy = (string)drClientDeptTestPanel["CreatedBy"];
                            clientDeptTestPanel.LastModifiedOn = (DateTime)drClientDeptTestPanel["LastModifiedOn"];
                            clientDeptTestPanel.LastModifiedBy = (string)drClientDeptTestPanel["LastModifiedBy"];

                            clientDeptTestCategory.ClientDeptTestPanels.Add(clientDeptTestPanel);
                        }

                        clientDepartment.ClientDeptTestCategories.Add(clientDeptTestCategory);

                        if (drClientDeptTestCategory["TestCategoryId"].ToString() == ((int)TestCategories.UA).ToString())
                        {
                            clientDepartment.IsUA = true;
                        }
                        else if (drClientDeptTestCategory["TestCategoryId"].ToString() == ((int)TestCategories.Hair).ToString())
                        {
                            clientDepartment.IsHair = true;
                        }
                        else if (drClientDeptTestCategory["TestCategoryId"].ToString() == ((int)TestCategories.DNA).ToString())
                        {
                            clientDepartment.IsDNA = true;
                        }
                        else if (drClientDeptTestCategory["TestCategoryId"].ToString() == ((int)TestCategories.BC).ToString())
                        {
                            clientDepartment.IsBC = true;
                        }
                        else if (drClientDeptTestCategory["TestCategoryId"].ToString() == ((int)TestCategories.RC).ToString())
                        {
                            clientDepartment.IsRecordKeeping = true;
                        }
                    }
                }
                else
                {
                    clientDepartment.IsUA = false;
                    clientDepartment.IsHair = false;
                    clientDepartment.IsDNA = false;
                    clientDepartment.IsBC = false;
                    clientDepartment.IsRecordKeeping = false;
                }
            }

            return clientDepartment;
        }

        private ClientDepartment GetClientDepartment(int clientDepartmentId, MySqlTransaction trans)
        {
            ClientDepartment clientDepartment = null;

            string sqlQuery = "SELECT client_department_id AS ClientDepartmentId, client_id AS ClientId, department_name AS DepartmentName, mro_type_id AS MROTypeId, payment_type_id AS PaymentTypeId, is_department_active AS IsDepartmentActive, is_contact_info_as_client AS IsContactInfoAsClient, is_mailing_address_physical AS IsMailingAddressPhysical, sales_representative_id AS SalesRepresentativeId, sales_comissions AS SalesComissions, is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM client_departments WHERE client_department_id = @clientDepartmentId";

            MySqlParameter[] param = new MySqlParameter[1];
            param[0] = new MySqlParameter("@ClientDepartmentId", clientDepartmentId);

            MySqlDataReader dr = SqlHelper.ExecuteReader(trans, CommandType.Text, sqlQuery, param);

            if (dr.Read())
            {
                clientDepartment = new ClientDepartment();
                clientDepartment.ClientDepartmentId = (int)dr["ClientDepartmentId"];
                clientDepartment.ClientId = (int)dr["ClientId"];
                clientDepartment.DepartmentName = (string)dr["DepartmentName"];
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
            }
            dr.Close();

            if (clientDepartment != null)
            {
                sqlQuery = "SELECT client_dept_test_category_id AS ClientDeptTestCategoryIdId, client_department_id AS ClientDepartmentId, test_category_id AS TestCategoryId,display_order AS DisplayOrder,test_panel_price AS TestPanelPrice , is_synchronized AS IsSynchronized, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM client_dept_test_categories WHERE client_department_id = @ClientDepartmentId ORDER BY display_order";

                param = new MySqlParameter[1];
                param[0] = new MySqlParameter("@ClientDepartmentId", clientDepartmentId);

                dr = SqlHelper.ExecuteReader(trans, CommandType.Text, sqlQuery, param);
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

                    dr = SqlHelper.ExecuteReader(trans, CommandType.Text, sqlQuery, param);

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

                    dr = SqlHelper.ExecuteReader(trans, CommandType.Text, sqlQuery, param);

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

            return clientDepartment;
        }

        public List<string> DepatLabCodes()
        {
            List<string> res = new List<string>();

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();

                ClientDepartment clientDepartment = null;

                string sqlQuery = @"
SELECT 
lab_code AS FullLabCode
FROM client_departments";

                MySqlParameter[] param = new MySqlParameter[1];

                MySqlDataReader dr = SqlHelper.ExecuteReader(trans, CommandType.Text, sqlQuery, param);

                if (dr.Read())
                {
                    res.Add((string)dr["FullLabCode"]);

                }
                dr.Close();
            }
            return res;
        }

        public ClientDepartment GetClientDepartmentByLab_code(string lab_code)
        {
            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();

                ClientDepartment clientDepartment = null;

                string sqlQuery = @"
SELECT client_department_id AS ClientDepartmentId, 
lab_code AS FullLabCode, 
client_id AS ClientId, 
department_name AS DepartmentName, 
mro_type_id AS MROTypeId, 
payment_type_id AS PaymentTypeId, 
is_department_active AS IsDepartmentActive, 
is_contact_info_as_client AS IsContactInfoAsClient, 
is_mailing_address_physical AS IsMailingAddressPhysical, 
sales_representative_id AS SalesRepresentativeId, 
sales_comissions AS SalesComissions, 
is_synchronized AS IsSynchronized, 
is_archived AS IsArchived, 
created_on AS CreatedOn, 
created_by AS CreatedBy, 
last_modified_on AS LastModifiedOn, 
last_modified_by AS LastModifiedBy 
FROM client_departments WHERE lab_code = @lab_code";

                MySqlParameter[] param = new MySqlParameter[1];
                param[0] = new MySqlParameter("@lab_code", lab_code);

                MySqlDataReader dr = SqlHelper.ExecuteReader(trans, CommandType.Text, sqlQuery, param);

                if (dr.Read())
                {
                    clientDepartment = new ClientDepartment();
                    clientDepartment.ClientDepartmentId = (int)dr["ClientDepartmentId"];
                    clientDepartment.FullLabCode = (string)dr["FullLabCode"];
                    clientDepartment.DepartmentName = (string)dr["DepartmentName"];
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
                }
                dr.Close();

                if (clientDepartment != null)
                {
                    sqlQuery = "SELECT client_dept_test_category_id AS ClientDeptTestCategoryIdId, client_department_id AS ClientDepartmentId, test_category_id AS TestCategoryId,display_order AS DisplayOrder,test_panel_price AS TestPanelPrice , is_synchronized AS IsSynchronized, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM client_dept_test_categories WHERE client_department_id = @ClientDepartmentId ORDER BY display_order";

                    param = new MySqlParameter[1];
                    param[0] = new MySqlParameter("@ClientDepartmentId", clientDepartment.ClientDepartmentId);

                    dr = SqlHelper.ExecuteReader(trans, CommandType.Text, sqlQuery, param);
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

                        param[0] = new MySqlParameter("@ClientDepartmentId", clientDepartment.ClientDepartmentId);

                        dr = SqlHelper.ExecuteReader(trans, CommandType.Text, sqlQuery, param);

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
                        param[0] = new MySqlParameter("@ClientDepartmentId", clientDepartment.ClientDepartmentId);

                        dr = SqlHelper.ExecuteReader(trans, CommandType.Text, sqlQuery, param);

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

                return clientDepartment;
            }
        }
        public ClientDepartment GetClientDepartmentQuestInfo(string QuestId)
        {
            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();

                ClientDepartment clientDepartment = null;

                string sqlQuery = @"
SELECT client_department_id AS ClientDepartmentId, 
lab_code AS FullLabCode, 
client_id AS ClientId, 
department_name AS DepartmentName, 
mro_type_id AS MROTypeId, 
payment_type_id AS PaymentTypeId, 
is_department_active AS IsDepartmentActive, 
is_contact_info_as_client AS IsContactInfoAsClient, 
is_mailing_address_physical AS IsMailingAddressPhysical, 
sales_representative_id AS SalesRepresentativeId, 
sales_comissions AS SalesComissions, 
is_synchronized AS IsSynchronized, 
is_archived AS IsArchived, 
created_on AS CreatedOn, 
created_by AS CreatedBy, 
last_modified_on AS LastModifiedOn, 
last_modified_by AS LastModifiedBy 
FROM client_departments WHERE QuestCode = @QuestId";

                MySqlParameter[] param = new MySqlParameter[1];
                param[0] = new MySqlParameter("@QuestId", QuestId);

                MySqlDataReader dr = SqlHelper.ExecuteReader(trans, CommandType.Text, sqlQuery, param);

                if (dr.Read())
                {
                    clientDepartment = new ClientDepartment();
                    clientDepartment.ClientDepartmentId = (int)dr["ClientDepartmentId"];
                    clientDepartment.FullLabCode = (string)dr["FullLabCode"];
                    clientDepartment.DepartmentName = (string)dr["DepartmentName"];
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
                }
                dr.Close();

                if (clientDepartment != null)
                {
                    sqlQuery = "SELECT client_dept_test_category_id AS ClientDeptTestCategoryIdId, client_department_id AS ClientDepartmentId, test_category_id AS TestCategoryId,display_order AS DisplayOrder,test_panel_price AS TestPanelPrice , is_synchronized AS IsSynchronized, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM client_dept_test_categories WHERE client_department_id = @ClientDepartmentId ORDER BY display_order";

                    param = new MySqlParameter[1];
                    param[0] = new MySqlParameter("@ClientDepartmentId", clientDepartment.ClientDepartmentId);

                    dr = SqlHelper.ExecuteReader(trans, CommandType.Text, sqlQuery, param);
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

                        param[0] = new MySqlParameter("@ClientDepartmentId", clientDepartment.ClientDepartmentId);

                        dr = SqlHelper.ExecuteReader(trans, CommandType.Text, sqlQuery, param);

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
                        param[0] = new MySqlParameter("@ClientDepartmentId", clientDepartment.ClientDepartmentId);

                        dr = SqlHelper.ExecuteReader(trans, CommandType.Text, sqlQuery, param);

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

                return clientDepartment;
            }
        }

        private DataTable GetClientDepartmentTestCategories(int clientDepartmentId, MySqlTransaction trans)
        {
            string sqlQuery = "SELECT client_dept_test_category_id AS ClientDeptTestCategoryId, client_department_id AS ClientDepartmentId, test_category_id AS TestCategoryId, display_order AS DisplayOrder, test_panel_price AS TestPanelPrice, is_synchronized AS IsSynchronized, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM client_dept_test_categories WHERE client_department_id = @ClientDepartmentId ORDER BY display_order";

            MySqlParameter[] param = new MySqlParameter[1];
            param[0] = new MySqlParameter("@ClientDepartmentId", clientDepartmentId);

            DataSet ds = SqlHelper.ExecuteDataset(trans, CommandType.Text, sqlQuery, param);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        private DataTable GetClientDepartmentTestPanels(int clientDepartmentTestCategoryId, MySqlTransaction trans)
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

            DataSet ds = SqlHelper.ExecuteDataset(trans, CommandType.Text, sqlQuery, param);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        #endregion Private Methods
    }
}