using MySql.Data.MySqlClient;
using SurPath.Entity;
using SurPath.Enum;
using SurPath.MySQLHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SurPath.Data
{
    public partial class DonorDao : DataObject
    {
        public int DoDonorPreRegisteration(Donor donor, User user)
        {
            int returnValue = 0;

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();

                try
                {
                    string sqlQuery = "INSERT INTO donors ("
                                        + "donor_first_name, "
                                        + "donor_last_name, "
                                        + "donor_email, "
                                        + "donor_initial_client_id, "
                                        + "donor_registration_status, "
                                        + "is_synchronized, "
                                        + "is_archived, "
                                        + "created_on, "
                                        + "created_by, "
                                        + "last_modified_on, "
                                        + "last_modified_by) VALUES ("
                                        + "@DonorFirstName, "
                                        + "@DonorLastName, "
                                        + "@DonorEmail, "
                                        + "@DonorInitialClientId, "
                                        + "@DonorRegistrationStatusValue, "
                                        + "b'0', b'0', NOW(), @CreatedBy, NOW(), @LastModifiedBy)";

                    ParamHelper paramHelper = new ParamHelper();
                    //MySqlParameter[] param = new MySqlParameter[7];

                    paramHelper.Param = new MySqlParameter("@DonorFirstName", donor.DonorFirstName);
                    paramHelper.Param = new MySqlParameter("@DonorLastName", donor.DonorLastName);
                    paramHelper.Param = new MySqlParameter("@DonorEmail", donor.DonorEmail);
                    paramHelper.Param = new MySqlParameter("@DonorInitialClientId", donor.DonorInitialClientId);
                    paramHelper.Param = new MySqlParameter("@DonorRegistrationStatusValue", (int)donor.DonorRegistrationStatusValue);
                    paramHelper.Param = new MySqlParameter("@CreatedBy", "SYSTEM");
                    paramHelper.Param = new MySqlParameter("@LastModifiedBy", "SYSTEM");

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, paramHelper.Params);

                    sqlQuery = "SELECT LAST_INSERT_ID()";

                    returnValue = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlQuery));

                    user.DonorId = returnValue;
                    donor.DonorId = returnValue;
                    sqlQuery = "INSERT INTO users (user_name, user_password, is_user_active, user_first_name, user_last_name, user_phone_number, user_fax, user_email, change_password_required, user_type, department_id, donor_id, client_id, vendor_id, attorney_id, court_id, judge_id, is_synchronized, is_archived, created_on, created_by, last_modified_on, last_modified_by) VALUES (";
                    sqlQuery += "@Username, @UserPassword, @IsUserActive, @UserFirstName, @UserLastName, @UserPhoneNumber, @UserFax, @UserEmail, @ChangePasswordRequired, @UserType, @DepartmentId, @DonorId, @ClientId, @VendorId, @AttorneyId, @CourtId, @JudgeId, b'0', b'0', NOW(), @CreatedBy, NOW(), @LastModifiedBy)";


                    paramHelper.reset();

                    paramHelper.Param = new MySqlParameter("@Username", user.Username);
                    paramHelper.Param = new MySqlParameter("@UserPassword", user.UserPassword);
                    paramHelper.Param = new MySqlParameter("@IsUserActive", user.IsUserActive);
                    paramHelper.Param = new MySqlParameter("@UserFirstName", user.UserFirstName);
                    paramHelper.Param = new MySqlParameter("@UserLastName", user.UserLastName);
                    paramHelper.Param = new MySqlParameter("@UserPhoneNumber", user.UserPhoneNumber);
                    paramHelper.Param = new MySqlParameter("@UserFax", user.UserFax);
                    paramHelper.Param = new MySqlParameter("@UserEmail", user.UserEmail);
                    paramHelper.Param = new MySqlParameter("@ChangePasswordRequired", user.ChangePasswordRequired);
                    paramHelper.Param = new MySqlParameter("@UserType", user.UserType);
                    paramHelper.Param = new MySqlParameter("@DepartmentId", user.DepartmentId);
                    paramHelper.Param = new MySqlParameter("@DonorId", user.DonorId);
                    paramHelper.Param = new MySqlParameter("@ClientId", user.ClientId);
                    paramHelper.Param = new MySqlParameter("@VendorId", user.VendorId);
                    paramHelper.Param = new MySqlParameter("@AttorneyId", user.AttorneyId);
                    paramHelper.Param = new MySqlParameter("@CourtId", user.CourtId);
                    paramHelper.Param = new MySqlParameter("@JudgeId", user.JudgeId);
                    paramHelper.Param = new MySqlParameter("@CreatedBy", "SYSTEM");
                    paramHelper.Param = new MySqlParameter("@LastModifiedBy", "SYSTEM");

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, paramHelper.Params);

                    trans.Commit();
                    UpdateDonorPids(donor);
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

        public void UpdateDonorPids(Donor donor)
        {
            ParamHelper paramHelper = new ParamHelper();
            string sqlInsert = @"
INSERT INTO individual_pids
(donor_id, pid, pid_type_id, individual_pid_type_description, mask_pid, validated)
VALUES(@donor_id, @pid, @pid_type_id, @individual_pid_type_description, @mask_pid, @validated);
                    ";

            string sqlUpdate = @"
UPDATE individual_pids 
SET pid = @pid, individual_pid_type_description = @individual_pid_type_description, mask_pid = @mask_pid, validated= @validated
WHERE donor_id = @donor_id and pid_type_id = @pid_type_id;
";
            string sqlGetDonorPids = @"
select * from individual_pids WHERE donor_id = @donor_id;
";

            string sqlQuery = string.Empty;

            DataTable dataTable = new DataTable();


            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();

                try
                {
                    // get all the known pids for this donor
                    paramHelper.Param = new MySqlParameter("@donor_id", donor.DonorId);

                    //trans.CommandType = System.Data.CommandType.Text;
                    List<PIDTypeValue> _pidtypevalues = new List<PIDTypeValue>();
                    using (MySqlDataReader reader = SqlHelper.ExecuteReader(trans, CommandType.Text, sqlGetDonorPids, paramHelper.Params))
                    {
                        dataTable.Load(reader);
                    }
                    if (dataTable.Rows.Count > 0)
                    {
                        //_pidtypevalues.Add(new PIDTypeValue()
                        //{
                        //    PIDType = MySqlDriverType.
                        //});


                        //foreach (DataRow dr in dataTable.Rows)
                        //{
                        //    var _type_id = dr.Field<string>("pid_type_id");
                        //    var _value = dr.Field<string>("pid");

                        //}

                        _pidtypevalues = dataTable.Rows.OfType<DataRow>().Select(dr =>

                               
                                new PIDTypeValue()
                                {
                                    PIDType = Convert.ToInt32(dr.Field<string>("pid_type_id")),
                                    PIDValue = dr.Field<string>("pid"),
                                    mask = dr.Field<ulong>("mask_pid") >0,
                                    validated = dr.Field<ulong>("validated") > 0
                                }
                        ).ToList();
                    }


                    // Add PIDS to pid table
                    foreach (PIDTypeValue pv in donor.PidTypeValues)
                    {
                        if (!(string.IsNullOrEmpty(pv.PIDValue)))
                        {
                            if (pv.PIDValue.Equals("ILLEGIBLE", StringComparison.InvariantCultureIgnoreCase)) continue;

                            // is this a known pid? if so, we update
                            bool _knownPID = _pidtypevalues.Where(x => x.PIDType == (int)pv.PIDType).Count() > 0;
                            paramHelper.reset();
                            paramHelper.Param = new MySqlParameter("@donor_id", donor.DonorId);

                            if (_knownPID == true)
                            {
                                // if so, update
                                sqlQuery = sqlUpdate;
                                // otherwise, we add PID to database.
                                paramHelper.Param = new MySqlParameter("@pid", pv.PIDValue);
                                paramHelper.Param = new MySqlParameter("@pid_type_id", pv.PIDType);
                                paramHelper.Param = new MySqlParameter("@individual_pid_type_description", ((PidTypes)pv.PIDType).ToString());
                                paramHelper.Param = new MySqlParameter("@mask_pid", (pv.PIDType == (int)PidTypes.SSN) ? (sbyte)1 : (sbyte)0);
                                paramHelper.Param = new MySqlParameter("@validated", (pv.validated==true) ? (sbyte)1 : (sbyte)0);
                            }
                            else
                            {
                                // otherwise, we add PID to database.
                                sqlQuery = sqlInsert;

                                paramHelper.Param = new MySqlParameter("@pid", pv.PIDValue);
                                paramHelper.Param = new MySqlParameter("@pid_type_id", pv.PIDType);
                                paramHelper.Param = new MySqlParameter("@individual_pid_type_description", ((PidTypes)pv.PIDType).ToString());
                                paramHelper.Param = new MySqlParameter("@mask_pid", (pv.PIDType == (int)PidTypes.SSN) ? (sbyte)1 : (sbyte)0);
                                paramHelper.Param = new MySqlParameter("@validated", (pv.validated == true) ? (sbyte)1 : (sbyte)0);
                            }
                            SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, paramHelper.Params);
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
        }


        public void DoDonorInQueueUpdate(int donorId, string newPassword, string status)
        {
            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();

                try
                {
                    string sqlQuery = "UPDATE donors SET "
                                        + "donor_registration_status = @DonorRegistrationStatusValue, "
                                        + "is_synchronized = b'0', "
                                        + "last_modified_on = NOW(), "
                                        + "last_modified_by = 'SYSTEM' "
                                        + "WHERE donor_id = @DonorId";

                    MySqlParameter[] param = new MySqlParameter[2];

                    param[0] = new MySqlParameter("@DonorId", donorId);
                    if (status == "PreRegistration")
                    {
                        param[1] = new MySqlParameter("@DonorRegistrationStatusValue", (int)DonorRegistrationStatus.Activated);
                    }
                    else if (status == "Registered")
                    {
                        param[1] = new MySqlParameter("@DonorRegistrationStatusValue", (int)DonorRegistrationStatus.Registered);
                    }
                    else if (status == "InQueue")
                    {
                        param[1] = new MySqlParameter("@DonorRegistrationStatusValue", (int)DonorRegistrationStatus.InQueue);
                    }
                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                    sqlQuery = "UPDATE users SET user_password = @NewPassword, change_password_required = b'0', is_synchronized = b'0', last_modified_on = NOW(), last_modified_by = @LastModifiedBy WHERE donor_id = @DonorId";

                    param = new MySqlParameter[3];
                    param[0] = new MySqlParameter("@DonorId", donorId);
                    param[1] = new MySqlParameter("@NewPassword", newPassword);
                    param[2] = new MySqlParameter("@LastModifiedBy", "SYSTEM");

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

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
        }


        public Tuple<double, double> Price_andInsert_To_donor_test_info_test_categories(ClientDepartment clientDepartment, MySqlTransaction trans, int donorTestInfoId, string donorEmail, string sqlTestCategory, double mallCost, Client client)
        {
            ParamHelper paramHelper = new ParamHelper();
            double totalPaymentAmount = 0.0;
            double totalTestPanelCost = 0.0;
            string sqlQuery;

            if (clientDepartment.ClientDeptTestCategories.Count > 0)
            {
                foreach (ClientDeptTestCategory testCategory in clientDepartment.ClientDeptTestCategories)
                {
                    int? testPanelId = null;
                    int? hairTestPanelDays = null;
                    double? testPanelCost = null;
                    double? testPanelPrice = null;

                    if (testCategory.TestCategoryId == TestCategories.Hair)
                    {
                        hairTestPanelDays = 90;
                    }

                    if (testCategory.TestCategoryId == TestCategories.UA || testCategory.TestCategoryId == TestCategories.Hair || testCategory.TestCategoryId == TestCategories.BC || testCategory.TestCategoryId == TestCategories.RC)
                    {
                        logDebug($"testCategory is UA, Hair, BC, or RC");

                        if (testCategory.ClientDeptTestPanels.Count > 0)
                        {
                            foreach (ClientDeptTestPanel testPanel in testCategory.ClientDeptTestPanels)
                            {
                                if (testPanel.IsMainTestPanel)
                                {
                                    testPanelId = testPanel.TestPanelId;
                                    testPanelPrice = testPanel.TestPanelPrice;
                                    break;
                                }
                            }
                        }

                        if (testPanelId != null)
                        {
                            string sqlTestPanelCost = "SELECT IFNULL(cost, 0) FROM test_panels WHERE test_panel_id = @TestPanelId";

                            paramHelper.reset();
                            paramHelper.Param = new MySqlParameter("@TestPanelId", testPanelId);
                            logDebug($"Selecting cost from test_panels");

                            testPanelCost = Convert.ToDouble(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlTestPanelCost, paramHelper.Params));
                        }
                    }
                    else if (testCategory.TestCategoryId == TestCategories.DNA)
                    {
                        logDebug($"testCategory is DNA");

                        testPanelPrice = testCategory.TestPanelPrice;

                        string sqlTestPanelCost = "SELECT IFNULL(dna_test_panel_cost, 0) FROM cost_master";
                        logDebug($"Selecting cost from cost_master");

                        testPanelCost = Convert.ToDouble(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlTestPanelCost));
                    }
                    paramHelper.reset();

                    paramHelper.Param = new MySqlParameter("@DonorTestInfoId", donorTestInfoId);
                    paramHelper.Param = new MySqlParameter("@TestCategoryId", (int)testCategory.TestCategoryId);
                    paramHelper.Param = new MySqlParameter("@TestPanelId", testPanelId);
                    paramHelper.Param = new MySqlParameter("@HairTestPanelDays", hairTestPanelDays);
                    paramHelper.Param = new MySqlParameter("@TestPanelStatus", (int)DonorRegistrationStatus.Registered);
                    paramHelper.Param = new MySqlParameter("@TestPanelCost", testPanelCost);
                    paramHelper.Param = new MySqlParameter("@TestPanelPrice", testPanelPrice);
                    paramHelper.Param = new MySqlParameter("@CreatedBy", donorEmail);
                    paramHelper.Param = new MySqlParameter("@LastModifiedBy", donorEmail);

                    if (testPanelPrice != null)
                    {
                        totalPaymentAmount += Convert.ToDouble(testPanelPrice);
                    }

                    if (testPanelCost != null)
                    {
                        totalTestPanelCost += Convert.ToDouble(testPanelCost);
                    }
                    logDebug($"inserting into donor_test_info_test_categories");

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlTestCategory, paramHelper.Params);
                }

                sqlQuery = "UPDATE donor_test_info SET total_payment_amount = @TotalPaymentAmount, laboratory_cost = @TotalTestPanelCost, mro_cost = @MROCost WHERE donor_test_info_id = @DonorTestInfoId";

                paramHelper.reset();

                paramHelper.Param = new MySqlParameter("@DonorTestInfoId", donorTestInfoId);
                paramHelper.Param = new MySqlParameter("@TotalPaymentAmount", totalPaymentAmount);
                paramHelper.Param = new MySqlParameter("@TotalTestPanelCost", totalTestPanelCost);
                paramHelper.Param = new MySqlParameter("@MROCost", mallCost);

                logDebug($"updating donor_test_info");

                SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, paramHelper.Params);

                if (clientDepartment.PaymentTypeId == ClientPaymentTypes.InvoiceClient)
                {
                    DateTime date = DateTime.Now;

                    sqlQuery = "UPDATE donor_test_info SET "
                              + "payment_date = @PaymentDate, "
                              + "payment_method_id = @PaymentMethodId, "
                              + "payment_note = @PaymentNote, "
                              + "payment_status = @PaymentStatus, "
                              + "is_synchronized = b'0', "
                              + "last_modified_on = NOW(), "
                              + "last_modified_by = @LastModifiedBy, "
                              + "payment_received = @IsPaymentReceived "
                              + "WHERE donor_test_info_id = @DonorTestInfoId";

                    paramHelper.reset();

                    paramHelper.Param = new MySqlParameter("@DonorTestInfoId", donorTestInfoId);
                    paramHelper.Param = new MySqlParameter("@PaymentStatus", (int)PaymentStatus.Paid);
                    paramHelper.Param = new MySqlParameter("@PaymentNote", client.ClientName);
                    paramHelper.Param = new MySqlParameter("@PaymentMethodId", (int)PaymentMethod.Cash);
                    paramHelper.Param = new MySqlParameter("@PaymentDate", date);
                    paramHelper.Param = new MySqlParameter("@LastModifiedBy", donorEmail);
                    paramHelper.Param = new MySqlParameter("@IsPaymentReceived", 1);
                    logDebug($"updating donor_test_info (is InvoiceClient)");

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, paramHelper.Params);
                }
            }
            return new Tuple<double, double>(totalPaymentAmount, totalTestPanelCost);
        }

        public string DoDonorRegistrationTestRequest(int donorId, string donorEmail, ClientDepartment clientDepartment, bool CalculateOnly = false)
        {
            logDebug($"DoDonorRegistrationTestRequest - donorId {donorId}, donorEmail {donorEmail}, clientDepartment {clientDepartment.ClientDepartmentId}, CalculateOnly {CalculateOnly.ToString()}");

            if (clientDepartment.ClientId < 1 || clientDepartment.ClientDepartmentId < 1) throw new Exception("ERROR: Client ID and Client Department ID are required");


            double totalPaymentAmount = 0.0;
            double totalTestPanelCost = 0.0;
            string retValue = string.Empty;
            int donorTestInfoId = 0;
            ParamHelper paramHelper = new ParamHelper();

            ClientDao clientDao = new ClientDao();
            Client client = null;

            VendorDao vendorDao = new VendorDao();
            Vendor vendor = null;
            int drStatus = (int)DonorRegistrationStatus.Registered;
            double mallCost = 0.0;

            client = clientDao.Get(clientDepartment.ClientId);
            logDebug($"client retrieved - client {client.ClientId.ToString()}");

            if (client != null)
            {
                logDebug($"client.MROVendorId {client.MROVendorId}");
                if (client.MROVendorId != null)
                {
                    vendor = vendorDao.Get(Convert.ToInt32(client.MROVendorId));
                    logDebug($"vendor {vendor.VendorId}");
                    if (vendor != null && vendor.MALLMROCost != null)
                    {
                        mallCost = Convert.ToDouble(vendor.MALLMROCost);
                        logDebug($"mallCost {mallCost}");
                    }
                }
            }
            if (clientDepartment.PaymentTypeId == ClientPaymentTypes.InvoiceClient)
            {
                logDebug($"clientDepartment is invoice client");
                drStatus = (int)DonorRegistrationStatus.InQueue;
            }

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();
                logDebug($"Beginning sql transaction");
                MySqlTransaction trans = conn.BeginTransaction();

                MySqlParameter[] param;

                // We need to build a test_info model
                if (CalculateOnly)
                {
                    // This calculates the total without inserting into the database
                    if (clientDepartment.ClientDeptTestCategories.Count > 0)
                    {
                        logDebug($"clientDepartment.ClientDeptTestCategories.Count {clientDepartment.ClientDeptTestCategories.Count}");
                        try
                        {
                            foreach (ClientDeptTestCategory testCategory in clientDepartment.ClientDeptTestCategories)
                            {
                                logDebug($"testCategory {testCategory.TestCategoryId}");

                                int? testPanelId = null;
                                int? hairTestPanelDays = null;
                                double? testPanelCost = null;
                                double? testPanelPrice = null;

                                if (testCategory.TestCategoryId == TestCategories.Hair)
                                {
                                    hairTestPanelDays = 90;
                                }

                                if (testCategory.TestCategoryId == TestCategories.UA || testCategory.TestCategoryId == TestCategories.Hair || testCategory.TestCategoryId == TestCategories.BC || testCategory.TestCategoryId == TestCategories.RC)
                                {
                                    logDebug($"testCategory is UA, Hair, BC, or RC");

                                    if (testCategory.ClientDeptTestPanels.Count > 0)
                                    {
                                        foreach (ClientDeptTestPanel testPanel in testCategory.ClientDeptTestPanels)
                                        {
                                            logDebug($"testPanel {testPanel.TestPanelId}");
                                            if (testPanel.IsMainTestPanel)
                                            {
                                                testPanelId = testPanel.TestPanelId;
                                                testPanelPrice = testPanel.TestPanelPrice;
                                                break;
                                            }
                                        }
                                    }

                                    if (testPanelId != null)
                                    {
                                        logDebug($"testPanelId {testPanelId} is main test panel");

                                        string sqlTestPanelCost = "SELECT IFNULL(cost, 0) FROM test_panels WHERE test_panel_id = @TestPanelId";
                                        param = new MySqlParameter[2];

                                        param = new MySqlParameter[1];
                                        param[0] = new MySqlParameter("@TestPanelId", testPanelId);

                                        testPanelCost = Convert.ToDouble(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlTestPanelCost, param));
                                    }
                                    if (testPanelId == null) logDebug($"testPanelId is null, main test panel not found");
                                }
                                else if (testCategory.TestCategoryId == TestCategories.DNA)
                                {
                                    logDebug($"testCategory is DNA");

                                    testPanelPrice = testCategory.TestPanelPrice;

                                    string sqlTestPanelCost = "SELECT IFNULL(dna_test_panel_cost, 0) FROM cost_master";

                                    testPanelCost = Convert.ToDouble(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlTestPanelCost));
                                }

                                //param = new MySqlParameter[9];

                                //param[0] = new MySqlParameter("@DonorTestInfoId", donorTestInfoId);
                                //param[1] = new MySqlParameter("@TestCategoryId", (int)testCategory.TestCategoryId);
                                //param[2] = new MySqlParameter("@TestPanelId", testPanelId);
                                //param[3] = new MySqlParameter("@HairTestPanelDays", hairTestPanelDays);
                                //param[4] = new MySqlParameter("@TestPanelStatus", (int)DonorRegistrationStatus.Registered);
                                //param[5] = new MySqlParameter("@TestPanelCost", testPanelCost);
                                //param[6] = new MySqlParameter("@TestPanelPrice", testPanelPrice);
                                //param[7] = new MySqlParameter("@CreatedBy", donorEmail);
                                //param[8] = new MySqlParameter("@LastModifiedBy", donorEmail);

                                if (testPanelPrice != null)
                                {
                                    totalPaymentAmount += Convert.ToDouble(testPanelPrice);
                                }

                                if (testPanelCost != null)
                                {
                                    totalTestPanelCost += Convert.ToDouble(testPanelCost);
                                }
                            }
                            logDebug($"Committing sql transaction [ no inserts or updaates as this is calc only");
                            logDebug($"totalPaymentAmount: {totalPaymentAmount.ToString()}, totalTestPanelCost {totalTestPanelCost.ToString()}");

                            trans.Commit();
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                }
                else
                {
                    try
                    {

                        logDebug($"Not a calculate only, creating test requests");

                        string sqlQuery = "UPDATE donors SET "
                                            + "donor_registration_status = @DonorRegistrationStatusValue, "
                                            + "is_synchronized = b'0', "
                                            + "last_modified_on = NOW(), "
                                            + "last_modified_by = 'SYSTEM' "
                                            + "WHERE donor_id = @DonorId";

                        param = new MySqlParameter[2];

                        param[0] = new MySqlParameter("@DonorId", donorId);
                        param[1] = new MySqlParameter("@DonorRegistrationStatusValue", drStatus);

                        logDebug($"Setting donor_registration_status");
                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                        // err above

                        sqlQuery = "INSERT INTO donor_test_info ("
                                            + "donor_id, "
                                            + "client_id, "
                                            + "client_department_id, "
                                            + "reason_for_test_id, "
                                            + "mro_type_id, "
                                            + "payment_type_id, "
                                            + "test_requested_date, "
                                            + "test_requested_by, "
                                            + "test_status, "
                                            + "is_synchronized, "
                                            + "created_on, "
                                            + "created_by, "
                                            + "last_modified_on, "
                                            + "last_modified_by "
                                            + ") VALUES ( "
                                            + "@DonorId, "
                                            + "@ClientId, "
                                            + "@ClientDepartmentId, "
                                            + "@reason_for_test_id, "
                                            + "@MROTypeId, "
                                            + "@PaymentTypeId, "
                                            + "NOW(), "
                                            + "@TestRequestedBy, "
                                            + "@TestStatus, "
                                            + "b'0', "
                                            + "NOW(), "
                                            + "@CreatedBy, "
                                            + "NOW(), "
                                            + "@LastModifiedBy)";

                        param = new MySqlParameter[10];

                        param[0] = new MySqlParameter("@DonorId", donorId);
                        param[1] = new MySqlParameter("@ClientId", clientDepartment.ClientId);
                        param[2] = new MySqlParameter("@ClientDepartmentId", clientDepartment.ClientDepartmentId);
                        param[3] = new MySqlParameter("@reason_for_test_id", clientDepartment.reason_for_test_default);
                        param[4] = new MySqlParameter("@MROTypeId", (int)clientDepartment.MROTypeId);
                        param[5] = new MySqlParameter("@PaymentTypeId", (int)clientDepartment.PaymentTypeId);
                        param[6] = new MySqlParameter("@TestRequestedBy", donorId);
                        param[7] = new MySqlParameter("@TestStatus", drStatus);
                        param[8] = new MySqlParameter("@CreatedBy", donorEmail);
                        param[9] = new MySqlParameter("@LastModifiedBy", donorEmail);

                        logDebug($"Inserting donor_test_info");
                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                        sqlQuery = "SELECT LAST_INSERT_ID()";

                        donorTestInfoId = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlQuery));
                        logDebug($"donorTestInfoId {donorTestInfoId}");

                        string sqlTestCategory = "INSERT INTO donor_test_info_test_categories ("
                                                    + "donor_test_info_id, "
                                                    + "test_category_id, "
                                                    + "test_panel_id, "
                                                    + "hair_test_panel_days, "
                                                    + "test_panel_status, "
                                                    + "test_panel_cost, "
                                                    + "test_panel_price, "
                                                    + "is_synchronized, "
                                                    + "created_on, "
                                                    + "created_by, "
                                                    + "last_modified_on, "
                                                    + "last_modified_by) "
                                                    + "VALUES ("
                                                    + "@DonorTestInfoId, "
                                                    + "@TestCategoryId, "
                                                    + "@TestPanelId, "
                                                    + "@HairTestPanelDays, "
                                                    + "@TestPanelStatus, "
                                                    + "@TestPanelCost, "
                                                    + "@TestPanelPrice, "
                                                    + "b'0', "
                                                    + "NOW(), "
                                                    + "@CreatedBy, "
                                                    + "NOW(), "
                                                    + "@LastModifiedBy)";

                        //Tuple<double, double> totals = Price_andInsert_To_donor_test_info_test_categories(clientDepartment, trans, donorTestInfoId, donorEmail, sqlTestCategory, mallCost, client);

                        //totalPaymentAmount = totals.Item1;
                        //totalTestPanelCost = totals.Item2;
                        if (clientDepartment.ClientDeptTestCategories.Count > 0)
                        {
                            foreach (ClientDeptTestCategory testCategory in clientDepartment.ClientDeptTestCategories)
                            {
                                int? testPanelId = null;
                                int? hairTestPanelDays = null;
                                double? testPanelCost = null;
                                double? testPanelPrice = null;

                                if (testCategory.TestCategoryId == TestCategories.Hair)
                                {
                                    hairTestPanelDays = 90;
                                }

                                if (testCategory.TestCategoryId == TestCategories.UA || testCategory.TestCategoryId == TestCategories.Hair || testCategory.TestCategoryId == TestCategories.BC || testCategory.TestCategoryId == TestCategories.RC)
                                {
                                    logDebug($"testCategory is UA, Hair, BC, or RC");

                                    if (testCategory.ClientDeptTestPanels.Count > 0)
                                    {
                                        foreach (ClientDeptTestPanel testPanel in testCategory.ClientDeptTestPanels)
                                        {
                                            if (testPanel.IsMainTestPanel)
                                            {
                                                testPanelId = testPanel.TestPanelId;
                                                testPanelPrice = testPanel.TestPanelPrice;
                                                break;
                                            }
                                        }
                                    }

                                    if (testPanelId != null)
                                    {
                                        string sqlTestPanelCost = "SELECT IFNULL(cost, 0) FROM test_panels WHERE test_panel_id = @TestPanelId";

                                        paramHelper.reset();
                                        paramHelper.Param = new MySqlParameter("@TestPanelId", testPanelId);
                                        logDebug($"Selecting cost from test_panels");

                                        testPanelCost = Convert.ToDouble(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlTestPanelCost, paramHelper.Params));
                                    }
                                }
                                else if (testCategory.TestCategoryId == TestCategories.DNA)
                                {
                                    logDebug($"testCategory is DNA");

                                    testPanelPrice = testCategory.TestPanelPrice;

                                    string sqlTestPanelCost = "SELECT IFNULL(dna_test_panel_cost, 0) FROM cost_master";
                                    logDebug($"Selecting cost from cost_master");

                                    testPanelCost = Convert.ToDouble(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlTestPanelCost));
                                }
                                paramHelper.reset();

                                paramHelper.Param = new MySqlParameter("@DonorTestInfoId", donorTestInfoId);
                                paramHelper.Param = new MySqlParameter("@TestCategoryId", (int)testCategory.TestCategoryId);
                                paramHelper.Param = new MySqlParameter("@TestPanelId", testPanelId);
                                paramHelper.Param = new MySqlParameter("@HairTestPanelDays", hairTestPanelDays);
                                paramHelper.Param = new MySqlParameter("@TestPanelStatus", (int)DonorRegistrationStatus.Registered);
                                paramHelper.Param = new MySqlParameter("@TestPanelCost", testPanelCost);
                                paramHelper.Param = new MySqlParameter("@TestPanelPrice", testPanelPrice);
                                paramHelper.Param = new MySqlParameter("@CreatedBy", donorEmail);
                                paramHelper.Param = new MySqlParameter("@LastModifiedBy", donorEmail);

                                if (testPanelPrice != null)
                                {
                                    totalPaymentAmount += Convert.ToDouble(testPanelPrice);
                                }

                                if (testPanelCost != null)
                                {
                                    totalTestPanelCost += Convert.ToDouble(testPanelCost);
                                }
                                logDebug($"inserting into donor_test_info_test_categories");

                                SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlTestCategory, paramHelper.Params);
                            }

                            sqlQuery = "UPDATE donor_test_info SET total_payment_amount = @TotalPaymentAmount, laboratory_cost = @TotalTestPanelCost, mro_cost = @MROCost WHERE donor_test_info_id = @DonorTestInfoId";

                            paramHelper.reset();

                            paramHelper.Param = new MySqlParameter("@DonorTestInfoId", donorTestInfoId);
                            paramHelper.Param = new MySqlParameter("@TotalPaymentAmount", totalPaymentAmount);
                            paramHelper.Param = new MySqlParameter("@TotalTestPanelCost", totalTestPanelCost);
                            paramHelper.Param = new MySqlParameter("@MROCost", mallCost);

                            logDebug($"updating donor_test_info");

                            SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, paramHelper.Params);

                            if (clientDepartment.PaymentTypeId == ClientPaymentTypes.InvoiceClient)
                            {
                                DateTime date = DateTime.Now;

                                sqlQuery = "UPDATE donor_test_info SET "
                                          + "payment_date = @PaymentDate, "
                                          + "payment_method_id = @PaymentMethodId, "
                                          + "payment_note = @PaymentNote, "
                                          + "payment_status = @PaymentStatus, "
                                          + "is_synchronized = b'0', "
                                          + "last_modified_on = NOW(), "
                                          + "last_modified_by = @LastModifiedBy, "
                                          + "payment_received = @IsPaymentReceived "
                                          + "WHERE donor_test_info_id = @DonorTestInfoId";

                                paramHelper.reset();

                                paramHelper.Param = new MySqlParameter("@DonorTestInfoId", donorTestInfoId);
                                paramHelper.Param = new MySqlParameter("@PaymentStatus", (int)PaymentStatus.Paid);
                                paramHelper.Param = new MySqlParameter("@PaymentNote", client.ClientName);
                                paramHelper.Param = new MySqlParameter("@PaymentMethodId", (int)PaymentMethod.Cash);
                                paramHelper.Param = new MySqlParameter("@PaymentDate", date);
                                paramHelper.Param = new MySqlParameter("@LastModifiedBy", donorEmail);
                                paramHelper.Param = new MySqlParameter("@IsPaymentReceived", 1);
                                logDebug($"updating donor_test_info (is InvoiceClient)");

                                SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, paramHelper.Params);

                            }
                        }


                        sqlQuery = "SELECT user_id FROM users where UPPER(user_name) = UPPER(@DonorEmail) and is_archived=0";

                        param = new MySqlParameter[1];

                        param[0] = new MySqlParameter("@DonorEmail", donorEmail);

                        logDebug($"selecting user_id");

                        int currentUserId = (int)SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlQuery, param);
                        logDebug($"currentUserId = {currentUserId}");

                        sqlQuery = "INSERT INTO donor_test_activity ("
                                        + "donor_test_info_id, "
                                        + "activity_datetime, "
                                        + "activity_user_id, "
                                        + "activity_category_id, "
                                        + "is_activity_visible, "
                                        + "activity_note, "
                                        + "is_synchronized) VALUES ("
                                        + "@DonorTestInfoId, "
                                        + "NOW(), "
                                        + "@ActivityUserId, "
                                        + "@ActivityCategoryId, "
                                        + "@IsActivityVisible, "
                                        + "@ActivityNote, "
                                        + "b'0')";

                        param = new MySqlParameter[5];

                        param[0] = new MySqlParameter("@DonorTestInfoId", donorTestInfoId);
                        param[1] = new MySqlParameter("@ActivityUserId", currentUserId);
                        param[2] = new MySqlParameter("@ActivityCategoryId", (int)DonorActivityCategories.General);
                        param[3] = new MySqlParameter("@IsActivityVisible", true);
                        param[4] = new MySqlParameter("@ActivityNote", "New test registered" + (totalPaymentAmount > 0 ? " (Payment Due Amount: $" + totalPaymentAmount.ToString() + ")" : ""));
                        logDebug($"inserting  donor_test_activity  {donorTestInfoId}, {currentUserId}");
                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);
                        if (clientDepartment.PaymentTypeId == ClientPaymentTypes.InvoiceClient)
                        {
                            sqlQuery = "SELECT user_id FROM users where UPPER(user_name) = UPPER(@DonorEmail) and is_archived=0";

                            param = new MySqlParameter[1];

                            param[0] = new MySqlParameter("@DonorEmail", donorEmail);
                            logDebug($"selecting  user_id  (invoice client)");

                            currentUserId = (int)SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlQuery, param);

                            sqlQuery = "INSERT INTO donor_test_activity ("
                                            + "donor_test_info_id, "
                                            + "activity_datetime, "
                                            + "activity_user_id, "
                                            + "activity_category_id, "
                                            + "is_activity_visible, "
                                            + "activity_note, "
                                            + "is_synchronized) VALUES ("
                                            + "@DonorTestInfoId, "
                                            + "NOW(), "
                                            + "@ActivityUserId, "
                                            + "@ActivityCategoryId, "
                                            + "@IsActivityVisible, "
                                            + "@ActivityNote, "
                                            + "b'0')";

                            param = new MySqlParameter[5];

                            param[0] = new MySqlParameter("@DonorTestInfoId", donorTestInfoId);
                            param[1] = new MySqlParameter("@ActivityUserId", currentUserId);
                            param[2] = new MySqlParameter("@ActivityCategoryId", (int)DonorActivityCategories.General);
                            param[3] = new MySqlParameter("@IsActivityVisible", true);
                            param[4] = new MySqlParameter("@ActivityNote", string.Format("Payment made...$ {0}.", totalPaymentAmount.ToString()));
                            logDebug($"inserting  donor_test_activity (invoice client)");

                            SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);
                        }
                        logDebug($"Committing sql transaction");
                        trans.Commit();
                        // All our inserts are done. Before we commit - register a notification
                        logDebug($"Creating notification");

                        BackendData d = new BackendData(this.ConnectionString, null, this._logger);

                        d.SetDonorActivity(new ParamSetDonorActivity() { donor_test_info_id = donorTestInfoId, activity_category_id = (int)DonorActivityCategories.Information, activity_note = "Override price set", activity_user_id = currentUserId });

                        // Create the record for this donortestinfoid
                        logDebug($"Setting donor notification");
                        logDebug($"creating paramSetDonorNotification notification");
                        var _notification = new Notification() { donor_test_info_id = donorTestInfoId, created_by = donorEmail, client_department_id = clientDepartment.ClientDepartmentId, client_id = clientDepartment.ClientId, notify_reset_sendin = true, notify_manual = false, force_db = false };
                        logDebug($"Setting param with _notification");
                        var paramSetDonorNotification = new ParamSetDonorNotification() { notification = _notification };
                        logDebug($"calling paramSetDonorNotification");
                        d.SetDonorNotification(paramSetDonorNotification);
                        // Log that it was created in the activity log
                        logDebug($"Setting donor notification creation activity");
                        d.SetDonorActivity(new ParamSetDonorActivity() { donor_test_info_id = donorTestInfoId, activity_category_id = (int)DonorActivityCategories.Notification, activity_note = "Backend system records created", activity_user_id = currentUserId });

                    }
                    catch (Exception ex)
                    {
                        LogAnError(ex);
                        trans.Rollback();
                        throw ex;
                    }
                }
            }
            logDebug($"Returning");

            return donorTestInfoId + "," + totalPaymentAmount.ToString();
        }

        public int UploadDonorDocument(DonorDocument donorDocument)
        {
            int returnValue = 0;

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();

                try
                {
                    string sqlQuery = "INSERT INTO donor_documents ("
                                        + "donor_id, "
                                        + "document_upload_time, "
                                        + "document_title, "
                                        + "document_content, "
                                        + "source, "
                                        + "uploaded_by, "
                                        + "file_name, "
                                        + "is_NeedsApproval, "
                                        + "is_Rejected, "
                                        + "is_Updateable, "
                                        + "is_synchronized) VALUES ("
                                        + "@DonorId, "
                                        + "NOW(),"
                                        + "@DocumentTitle, "
                                        + "@DocumentContent, "
                                        + "@Source, "
                                        + "@UploadedBy, "
                                        + "@FileName, "
                                        + "b'1', "
                                        + "b'0', "
                                        + "b'1', "
                                        + "b'0')";

                    MySqlParameter[] param = new MySqlParameter[6];

                    param[0] = new MySqlParameter("@DonorId", donorDocument.DonorId);
                    param[1] = new MySqlParameter("@DocumentTitle", donorDocument.DocumentTitle);
                    param[2] = new MySqlParameter("@DocumentContent", donorDocument.DocumentContent);
                    param[3] = new MySqlParameter("@Source", donorDocument.Source);
                    param[4] = new MySqlParameter("@UploadedBy", donorDocument.UploadedBy);
                    param[5] = new MySqlParameter("@FileName", donorDocument.FileName);

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

        public int UpdateDonorDocumentApproved(int DonorDocumentId)
        {
            int returnValue = 0;

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();

                try
                {
                    string sqlQuery = "update donor_documents set is_Approved = 1, is_Rejected = 0 where donor_document_id = @DocumentId";

                    MySqlParameter[] param = new MySqlParameter[1];

                    param[0] = new MySqlParameter("@DocumentId", DonorDocumentId);

                    returnValue = Convert.ToInt32(SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param));

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

        public int UpdateDonorDocumentReject(int DonorDocumentId)
        {
            int returnValue = 0;

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();

                try
                {
                    string sqlQuery = "update donor_documents set is_Rejected = 1, is_approved = 0 where donor_document_id = @DocumentId";

                    MySqlParameter[] param = new MySqlParameter[1];

                    param[0] = new MySqlParameter("@DocumentId", DonorDocumentId);

                    returnValue = Convert.ToInt32(SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param));

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