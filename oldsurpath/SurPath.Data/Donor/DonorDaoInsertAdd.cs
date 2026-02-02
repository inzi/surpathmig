using MySql.Data.MySqlClient;
using Serilog;
using SurPath.Entity;
using SurPath.Enum;
using SurPath.MySQLHelper;
using System;
using System.Data;

namespace SurPath.Data
{
    public partial class DonorDao : DataObject
    {
        #region Constructor

        public ILogger _logger { get; set; }
        public ILogger Logger { get { return this._logger; } set { this._logger = value; } }
        private string __sessionid;

        public string SessionID
        {
            get { return this.__sessionid; }
            set
            {
                this.__sessionid = value;
            }
        }

        public DonorDao(ILogger __logger = null, string _SessionID = "")
        {
            this.SessionID = _SessionID;

            if (__logger == null)
            {
                this.Logger = new LoggerConfiguration().CreateLogger();
            }
            else
            {
                this.Logger = __logger;
                logDebug($"DonorDao - SessionID: {this.SessionID.ToString()}");
            }
        }

        #endregion Constructor

        #region Public Methods

        public int Insert(Donor donor)
        {
            int returnValue = 0;

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();

                try
                {
                    #region SQLQuery InsertDonor

                    string sqlQuery = "INSERT INTO donors ("
                                        + "donor_first_name, "
                                        + "donor_mi, "
                                        + "donor_last_name, "
                                        + "donor_suffix, "
                                        + "donor_ssn, "
                                        + "donor_date_of_birth, "
                                        + "donor_phone_1, "
                                        + "donor_phone_2, "
                                        + "donor_address_1, "
                                        + "donor_address_2, "
                                        + "donor_city, "
                                        + "donor_state, "
                                        + "donor_zip, "
                                        + "donor_email, "
                                        + "donor_gender, "
                                        + "donor_registration_status, "
                                        + "is_synchronized, "
                                        + "is_archived, "
                                        + "created_on, "
                                        + "created_by, "
                                        + "last_modified_on, "
                                        + "last_modified_by) VALUES ("
                                        + "@DonorFirstName, "
                                        + "@DonorMI, "
                                        + "@DonorLastName, "
                                        + "@DonorSuffix, "
                                        + "@DonorSSN, "
                                        + "@DonorDateOfBirth, "
                                        + "@DonorPhone1, "
                                        + "@DonorPhone2, "
                                        + "@DonorAddress1, "
                                        + "@DonorAddress2, "
                                        + "@DonorCity, "
                                        + "@DonorState, "
                                        + "@DonorZip, "
                                        + "@DonorEmail, "
                                        + "@DonorGender, "
                                        + "@DonorRegistrationStatusValue, "
                                        + "b'0', b'0', NOW(), @CreatedBy, NOW(), @LastModifiedBy)";

                    #endregion SQLQuery InsertDonor

                    #region SQLParamaters

                    MySqlParameter[] param = new MySqlParameter[18];

                    param[0] = new MySqlParameter("@DonorFirstName", donor.DonorFirstName);
                    param[1] = new MySqlParameter("@DonorMI", donor.DonorMI);
                    param[2] = new MySqlParameter("@DonorLastName", donor.DonorLastName);
                    param[3] = new MySqlParameter("@DonorSuffix", donor.DonorSuffix);
                    param[4] = new MySqlParameter("@DonorSSN", donor.DonorSSN);
                    param[5] = new MySqlParameter("@DonorDateOfBirth", donor.DonorDateOfBirth);
                    param[6] = new MySqlParameter("@DonorPhone1", donor.DonorPhone1);
                    param[7] = new MySqlParameter("@DonorPhone2", donor.DonorPhone2);
                    param[8] = new MySqlParameter("@DonorAddress1", donor.DonorAddress1);
                    param[9] = new MySqlParameter("@DonorAddress2", donor.DonorAddress2);
                    param[10] = new MySqlParameter("@DonorCity", donor.DonorCity);
                    param[11] = new MySqlParameter("@DonorState", donor.DonorState);
                    param[12] = new MySqlParameter("@DonorZip", donor.DonorZip);
                    param[13] = new MySqlParameter("@DonorEmail", donor.DonorEmail);
                    param[14] = new MySqlParameter("@DonorGender", ((int)donor.DonorGender).ToString());
                    param[15] = new MySqlParameter("@DonorRegistrationStatusValue", (int)donor.DonorRegistrationStatusValue);
                    param[16] = new MySqlParameter("@CreatedBy", donor.CreatedBy);
                    param[17] = new MySqlParameter("@LastModifiedBy", donor.LastModifiedBy);

                    #endregion SQLParamaters

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

        public int AddDonor(Donor donor, User user, ClientDepartment clientDepartment, int currentUserId, ref int donorTestInfoId)
        {
            #region GetDAOs
            if (clientDepartment.ClientId < 1 || clientDepartment.ClientDepartmentId < 1) throw new Exception("ERROR: Client ID and Client Department ID are required");

            ClientDao clientDao = new ClientDao();
            Client client = null;

            VendorDao vendorDao = new VendorDao();
            Vendor vendor = null;

            #endregion GetDAOs

            double mallCost = 0.0;
            double mposCost = 0.0;
            int returnValue = 0;
            double totalPaymentAmount = 0.0;
            double totalTestPanelCost = 0.0;

            #region CheckClientDepartment - Get PaymentType(Client) - GetStatus of Donor Registration

            if (clientDepartment.ClientId != null)
            {
                client = clientDao.Get(clientDepartment.ClientId);

                if (client != null)
                {
                    if (client.MROVendorId != null)
                    {
                        vendor = vendorDao.Get(Convert.ToInt32(client.MROVendorId));

                        if (vendor != null && vendor.MALLMROCost != null)
                        {
                            mallCost = Convert.ToDouble(vendor.MALLMROCost);
                        }
                        if (vendor != null && vendor.MPOSMROCost != null)
                        {
                            mposCost = Convert.ToDouble(vendor.MPOSMROCost);
                        }
                    }
                }
                if (clientDepartment.PaymentTypeId == ClientPaymentTypes.InvoiceClient)
                {
                    donor.DonorRegistrationStatusValue = DonorRegistrationStatus.InQueue;
                }
                else
                {
                    donor.DonorRegistrationStatusValue = DonorRegistrationStatus.Registered;
                }
            }

            #endregion CheckClientDepartment - Get PaymentType(Client) - GetStatus of Donor Registration

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();

                try
                {
                    #region SQLQuery InsertInto Donors

                    string sqlQuery = "INSERT INTO donors ("
                                        + "donor_first_name, "
                                        + "donor_mi, "
                                        + "donor_last_name, "
                                        + "donor_suffix, "
                                        + "donor_ssn, "
                                        + "donor_date_of_birth, "
                                        + "donor_phone_1, "
                                        + "donor_phone_2, "
                                        + "donor_address_1, "
                                        + "donor_address_2, "
                                        + "donor_city, "
                                        + "donor_state, "
                                        + "donor_zip, "
                                        + "donor_email, "
                                        + "donor_gender, "
                                        + "donor_initial_client_id, "
                                        + "donor_initial_department_id, "
                                        + "donor_registration_status, "
                                        + "is_synchronized, "
                                        + "is_archived, "
                                        + "created_on, "
                                        + "created_by, "
                                        + "last_modified_on, "
                                        + "last_modified_by) VALUES ("
                                        + "@DonorFirstName, "
                                        + "@DonorMI, "
                                        + "@DonorLastName, "
                                        + "@DonorSuffix, "
                                        + "@DonorSSN, "
                                        + "@DonorDateOfBirth, "
                                        + "@DonorPhone1, "
                                        + "@DonorPhone2, "
                                        + "@DonorAddress1, "
                                        + "@DonorAddress2, "
                                        + "@DonorCity, "
                                        + "@DonorState, "
                                        + "@DonorZip, "
                                        + "@DonorEmail, "
                                        + "@DonorGender, "
                                        + "@DonorInitialClientId, "
                                        + "@DonorInitialDepartmentId, "
                                        + "@DonorRegistrationStatusValue, "
                                        + "b'0', b'0', NOW(), @CreatedBy, NOW(), @LastModifiedBy)";

                    #endregion SQLQuery InsertInto Donors

                    #region SQLParamaters

                    MySqlParameter[] param = new MySqlParameter[20];

                    param[0] = new MySqlParameter("@DonorFirstName", donor.DonorFirstName);
                    param[1] = new MySqlParameter("@DonorMI", donor.DonorMI);
                    param[2] = new MySqlParameter("@DonorLastName", donor.DonorLastName);
                    param[3] = new MySqlParameter("@DonorSuffix", donor.DonorSuffix);
                    param[4] = new MySqlParameter("@DonorSSN", donor.DonorSSN);
                    param[5] = new MySqlParameter("@DonorDateOfBirth", donor.DonorDateOfBirth);
                    param[6] = new MySqlParameter("@DonorPhone1", donor.DonorPhone1);
                    param[7] = new MySqlParameter("@DonorPhone2", donor.DonorPhone2);
                    param[8] = new MySqlParameter("@DonorAddress1", donor.DonorAddress1);
                    param[9] = new MySqlParameter("@DonorAddress2", donor.DonorAddress2);
                    param[10] = new MySqlParameter("@DonorCity", donor.DonorCity);
                    param[11] = new MySqlParameter("@DonorState", donor.DonorState);
                    param[12] = new MySqlParameter("@DonorZip", donor.DonorZip);
                    param[13] = new MySqlParameter("@DonorEmail", donor.DonorEmail);
                    param[14] = new MySqlParameter("@DonorGender", ((int)donor.DonorGender).ToString());

                    param[15] = new MySqlParameter("@DonorInitialClientId", donor.DonorInitialClientId);
                    param[16] = new MySqlParameter("@DonorInitialDepartmentId", donor.DonorInitialDepartmentId);

                    param[17] = new MySqlParameter("@DonorRegistrationStatusValue", (int)donor.DonorRegistrationStatusValue);
                    param[18] = new MySqlParameter("@CreatedBy", donor.CreatedBy);
                    param[19] = new MySqlParameter("@LastModifiedBy", donor.LastModifiedBy);

                    #endregion SQLParamaters

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                    sqlQuery = "SELECT LAST_INSERT_ID()";
                    returnValue = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlQuery));

                    donor.DonorId = returnValue;
                    user.DonorId = returnValue;

                    #region SQL Query Insert into Users

                    sqlQuery = "INSERT INTO users (user_name, user_password, is_user_active, user_first_name, user_last_name, user_phone_number, user_fax, user_email, change_password_required, user_type, department_id, donor_id, client_id, vendor_id, attorney_id, court_id, judge_id, is_synchronized, is_archived, created_on, created_by, last_modified_on, last_modified_by) VALUES (";
                    sqlQuery += "@Username, @UserPassword, @IsUserActive, @UserFirstName, @UserLastName, @UserPhoneNumber, @UserFax, @UserEmail, @ChangePasswordRequired, @UserType, @DepartmentId, @DonorId, @ClientId, @VendorId, @AttorneyId, @CourtId, @JudgeId, b'0', b'0', NOW(), @CreatedBy, NOW(), @LastModifiedBy)";

                    #endregion SQL Query Insert into Users

                    #region SQLParameters

                    param = new MySqlParameter[19];
                    param[0] = new MySqlParameter("@Username", user.Username);
                    param[1] = new MySqlParameter("@UserPassword", user.UserPassword);
                    param[2] = new MySqlParameter("@IsUserActive", user.IsUserActive);
                    param[3] = new MySqlParameter("@UserFirstName", user.UserFirstName);
                    param[4] = new MySqlParameter("@UserLastName", user.UserLastName);
                    param[5] = new MySqlParameter("@UserPhoneNumber", user.UserPhoneNumber);
                    param[6] = new MySqlParameter("@UserFax", user.UserFax);
                    param[7] = new MySqlParameter("@UserEmail", user.UserEmail);
                    param[8] = new MySqlParameter("@ChangePasswordRequired", user.ChangePasswordRequired);
                    param[9] = new MySqlParameter("@UserType", user.UserType);
                    param[10] = new MySqlParameter("@DepartmentId", user.DepartmentId);
                    param[11] = new MySqlParameter("@DonorId", user.DonorId);
                    param[12] = new MySqlParameter("@ClientId", user.ClientId);
                    param[13] = new MySqlParameter("@VendorId", user.VendorId);
                    param[14] = new MySqlParameter("@AttorneyId", user.AttorneyId);
                    param[15] = new MySqlParameter("@CourtId", user.CourtId);
                    param[16] = new MySqlParameter("@JudgeId", user.JudgeId);
                    param[17] = new MySqlParameter("@CreatedBy", "SYSTEM");
                    param[18] = new MySqlParameter("@LastModifiedBy", "SYSTEM");

                    #endregion SQLParameters

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                    sqlQuery = "SELECT LAST_INSERT_ID()";
                    user.UserId = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlQuery));

                    #region SQLQuery Insert DonorTestInfo


                    sqlQuery = "INSERT INTO donor_test_info ("
                                        + "donor_id, "
                                        + "client_id, "
                                        + "client_department_id, "
                                        + "mro_type_id, "
                                        + "payment_type_id, "
                                        + "test_requested_date, "
                                        + "test_requested_by, "
                                        + "test_status, "
                                        + "is_walkin_donor, "
                                        + "is_synchronized, "
                                        + "created_on, "
                                        + "created_by, "
                                        + "last_modified_on, "
                                        + "last_modified_by "
                                        + ") VALUES ( "
                                        + "@DonorId, "
                                        + "@ClientId, "
                                        + "@ClientDepartmentId, "
                                        + "@MROTypeId, "
                                        + "@PaymentTypeId, "
                                        + "NOW(), "
                                        + "@TestRequestedBy, "
                                        + "@TestStatus, "
                                        + "@IsWalkinDonor, "
                                        + "b'0', NOW(), @CreatedBy, NOW(), @LastModifiedBy)";

                    #endregion SQLQuery Insert DonorTestInfo

                    #region SQLParameters

                    param = new MySqlParameter[10];
                    param[0] = new MySqlParameter("@DonorId", donor.DonorId);
                    param[1] = new MySqlParameter("@ClientId", clientDepartment.ClientId);
                    param[2] = new MySqlParameter("@ClientDepartmentId", clientDepartment.ClientDepartmentId);
                    param[3] = new MySqlParameter("@MROTypeId", (int)clientDepartment.MROTypeId);
                    param[4] = new MySqlParameter("@PaymentTypeId", (int)clientDepartment.PaymentTypeId);
                    param[5] = new MySqlParameter("@TestRequestedBy", currentUserId);
                    param[6] = new MySqlParameter("@TestStatus", (int)donor.DonorRegistrationStatusValue);
                    param[7] = new MySqlParameter("@IsWalkinDonor", donor.IsWalkinDonor);
                    param[8] = new MySqlParameter("@CreatedBy", donor.CreatedBy);
                    param[9] = new MySqlParameter("@LastModifiedBy", donor.LastModifiedBy);

                    #endregion SQLParameters

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                    sqlQuery = "SELECT LAST_INSERT_ID()";
                    donorTestInfoId = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlQuery));

                    #region SQLQuery Insert DonorTestInfoCategories

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

                    #endregion SQLQuery Insert DonorTestInfoCategories

                    #region UpdatePaymentInfo

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

                            if (testCategory.TestCategoryId == TestCategories.UA || testCategory.TestCategoryId == TestCategories.Hair)
                            {
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

                                    param = new MySqlParameter[1];
                                    param[0] = new MySqlParameter("@TestPanelId", testPanelId);

                                    testPanelCost = Convert.ToDouble(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlTestPanelCost, param));
                                }
                            }
                            else if (testCategory.TestCategoryId == TestCategories.DNA)
                            {
                                testPanelPrice = testCategory.TestPanelPrice;

                                string sqlTestPanelCost = "SELECT IFNULL(dna_test_panel_cost, 0) FROM cost_master";

                                testPanelCost = Convert.ToDouble(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlTestPanelCost));
                            }

                            #region SQLParameters

                            param = new MySqlParameter[9];

                            param[0] = new MySqlParameter("@DonorTestInfoId", donorTestInfoId);
                            param[1] = new MySqlParameter("@TestCategoryId", (int)testCategory.TestCategoryId);
                            param[2] = new MySqlParameter("@TestPanelId", testPanelId);
                            param[3] = new MySqlParameter("@HairTestPanelDays", hairTestPanelDays);
                            param[4] = new MySqlParameter("@TestPanelStatus", (int)DonorRegistrationStatus.Registered);
                            param[5] = new MySqlParameter("@TestPanelCost", testPanelCost);
                            param[6] = new MySqlParameter("@TestPanelPrice", testPanelPrice);
                            param[7] = new MySqlParameter("@CreatedBy", donor.CreatedBy);
                            param[8] = new MySqlParameter("@LastModifiedBy", donor.LastModifiedBy);

                            #endregion SQLParameters

                            if (testPanelPrice != null)
                            {
                                totalPaymentAmount += Convert.ToDouble(testPanelPrice);
                            }
                            if (testPanelCost != null)
                            {
                                totalTestPanelCost += Convert.ToDouble(testPanelCost);
                            }

                            SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlTestCategory, param);
                        }

                        #region UpdatePaymentAmount

                        if (clientDepartment.MROTypeId == ClientMROTypes.MALL)
                        {
                            #region SQLQuery Update DonorTestInfo

                            sqlQuery = "UPDATE donor_test_info SET total_payment_amount = @TotalPaymentAmount, laboratory_cost = @TotalTestPanelCost, mro_cost = @MROCost WHERE donor_test_info_id = @DonorTestInfoId";

                            #endregion SQLQuery Update DonorTestInfo

                            #region SQLParameters

                            param = new MySqlParameter[4];

                            param[0] = new MySqlParameter("@DonorTestInfoId", donorTestInfoId);
                            param[1] = new MySqlParameter("@TotalPaymentAmount", totalPaymentAmount);
                            param[2] = new MySqlParameter("@TotalTestPanelCost", totalTestPanelCost);
                            param[3] = new MySqlParameter("@MROCost", mallCost);

                            #endregion SQLParameters
                        }

                        if (clientDepartment.MROTypeId == ClientMROTypes.MPOS)
                        {
                            #region SQLQuery UpdateDonorTestInfo

                            sqlQuery = "UPDATE donor_test_info SET total_payment_amount = @TotalPaymentAmount, laboratory_cost = @TotalTestPanelCost, mro_cost = @MROCost WHERE donor_test_info_id = @DonorTestInfoId";

                            #endregion SQLQuery UpdateDonorTestInfo

                            #region SQLParameters

                            param = new MySqlParameter[4];

                            param[0] = new MySqlParameter("@DonorTestInfoId", donorTestInfoId);
                            param[1] = new MySqlParameter("@TotalPaymentAmount", totalPaymentAmount);
                            param[2] = new MySqlParameter("@TotalTestPanelCost", totalTestPanelCost);
                            param[3] = new MySqlParameter("@MROCost", mposCost);

                            #endregion SQLParameters
                        }
                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                        if (clientDepartment.PaymentTypeId == ClientPaymentTypes.InvoiceClient)
                        {
                            DateTime date = DateTime.Now;

                            #region SQLQuery UpdateDonorTestInfo

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

                            #endregion SQLQuery UpdateDonorTestInfo

                            #region SQLParameters

                            param = new MySqlParameter[7];

                            param[0] = new MySqlParameter("@DonorTestInfoId", donorTestInfoId);
                            param[1] = new MySqlParameter("@PaymentStatus", (int)PaymentStatus.Paid);
                            param[2] = new MySqlParameter("@PaymentNote", client.ClientName);
                            param[3] = new MySqlParameter("@PaymentMethodId", (int)PaymentMethod.Cash);
                            param[4] = new MySqlParameter("@PaymentDate", date);
                            param[5] = new MySqlParameter("@LastModifiedBy", donor.LastModifiedBy);
                            param[6] = new MySqlParameter("@IsPaymentReceived", 1);

                            #endregion SQLParameters

                            SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);
                        }
                        donor.ProgramAmount = totalPaymentAmount;

                        #endregion UpdatePaymentAmount
                    }

                    #endregion UpdatePaymentInfo

                    #region InsertActivityNotes

                    #region SQLQuery InsertInto DonorTestActivity

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

                    #endregion SQLQuery InsertInto DonorTestActivity

                    if (!donor.IsWalkinDonor)
                    {
                        #region SQLParamaters

                        param = new MySqlParameter[5];

                        param[0] = new MySqlParameter("@DonorTestInfoId", donorTestInfoId);
                        param[1] = new MySqlParameter("@ActivityUserId", currentUserId);
                        param[2] = new MySqlParameter("@ActivityCategoryId", (int)DonorActivityCategories.General);
                        param[3] = new MySqlParameter("@IsActivityVisible", true);
                        param[4] = new MySqlParameter("@ActivityNote", "New test registered" + (totalPaymentAmount > 0 ? " (Payment Due Amount: $" + totalPaymentAmount.ToString() + ")" : ""));

                        #endregion SQLParamaters

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);
                    }
                    else
                    {
                        string donorActivityNote = string.Empty;

                        #region SQLParamaters

                        param = new MySqlParameter[5];

                        param[0] = new MySqlParameter("@DonorTestInfoId", donorTestInfoId);
                        param[1] = new MySqlParameter("@ActivityUserId", currentUserId);
                        param[2] = new MySqlParameter("@ActivityCategoryId", (int)DonorActivityCategories.General);
                        param[3] = new MySqlParameter("@IsActivityVisible", true);
                        param[4] = new MySqlParameter("@ActivityNote", "New test registered. Donor is registered as a walk-in donor to take instant test" + (totalPaymentAmount > 0 ? " (Payment Due Amount: $" + totalPaymentAmount.ToString() + ")" : ""));

                        #endregion SQLParamaters

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);
                    }

                    #endregion InsertActivityNotes

                    #region Insert TestActivity

                    if (clientDepartment.PaymentTypeId == ClientPaymentTypes.InvoiceClient)
                    {
                        #region SQLQuery Select Users

                        sqlQuery = "SELECT user_id FROM users where UPPER(user_name) = UPPER(@DonorEmail) and is_archived=0";

                        #endregion SQLQuery Select Users

                        #region SQLParamaters

                        param = new MySqlParameter[1];
                        param[0] = new MySqlParameter("@DonorEmail", donor.LastModifiedBy);

                        #endregion SQLParamaters

                        currentUserId = (int)SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlQuery, param);

                        #region SQLQuery Insert DonorTESTActivity

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

                        #endregion SQLQuery Insert DonorTESTActivity

                        #region SQLParameters

                        param = new MySqlParameter[5];

                        param[0] = new MySqlParameter("@DonorTestInfoId", donorTestInfoId);
                        param[1] = new MySqlParameter("@ActivityUserId", currentUserId);
                        param[2] = new MySqlParameter("@ActivityCategoryId", (int)DonorActivityCategories.General);
                        param[3] = new MySqlParameter("@IsActivityVisible", true);
                        param[4] = new MySqlParameter("@ActivityNote", string.Format("Payment made...$ {0}.", totalPaymentAmount.ToString()));

                        #endregion SQLParameters

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);
                    }

                    #endregion Insert TestActivity

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

        public int AddTest(Donor donor, ClientDepartment clientDepartment, int currentUserId, ref int donorTestInfoId)
        {
            if (clientDepartment.ClientId < 1 || clientDepartment.ClientDepartmentId < 1) throw new Exception("ERROR: Client ID and Client Department ID are required");

            int returnValue = 0;

            ClientDao clientDao = new ClientDao();
            Client client = null;

            VendorDao vendorDao = new VendorDao();
            Vendor vendor = null;

            double mallCost = 0.0;
            double mposCost = 0.0;

            if (clientDepartment.ClientId != null)
            {
                client = clientDao.Get(clientDepartment.ClientId);

                if (client != null)
                {
                    if (client.MROVendorId != null)
                    {
                        vendor = vendorDao.Get(Convert.ToInt32(client.MROVendorId));

                        if (vendor != null && vendor.MALLMROCost != null)
                        {
                            mallCost = Convert.ToDouble(vendor.MALLMROCost);
                        }
                        if (vendor != null && vendor.MPOSMROCost != null)
                        {
                            mposCost = Convert.ToDouble(vendor.MPOSMROCost);
                        }
                    }
                }
                if (clientDepartment.PaymentTypeId == ClientPaymentTypes.InvoiceClient)
                {
                    donor.DonorRegistrationStatusValue = DonorRegistrationStatus.InQueue;
                }
                else
                {
                    donor.DonorRegistrationStatusValue = DonorRegistrationStatus.Registered;
                }
            }

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();

                try
                {
                    string sqlQuery = "UPDATE donors SET "
                                        + "donor_first_name = @DonorFirstName, "
                                        + "donor_mi = @DonorMI, "
                                        + "donor_last_name = @DonorLastName, "
                                        + "donor_suffix = @DonorSuffix, "
                                        + "donor_ssn = @DonorSSN, "
                                        + "donor_date_of_birth = @DonorDateOfBirth, "
                                        + "donor_phone_1 = @DonorPhone1, "
                                        + "donor_phone_2 = @DonorPhone2, "
                                        + "donor_address_1 = @DonorAddress1, "
                                        + "donor_address_2 = @DonorAddress2, "
                                        + "donor_city = @DonorCity, "
                                        + "donor_state = @DonorState, "
                                        + "donor_zip = @DonorZip, "
                                        + "donor_email = @DonorEmail, "
                                        + "donor_gender = @DonorGender, "
                                        + "donor_registration_status = @DonorRegistrationStatusValue, "
                                        + "is_synchronized = b'0', "
                                        + "last_modified_on = NOW(), "
                                        + "last_modified_by = @LastModifiedBy "
                                        + "WHERE donor_id = @DonorId";

                    MySqlParameter[] param = new MySqlParameter[18];

                    param[0] = new MySqlParameter("@DonorId", donor.DonorId);
                    param[1] = new MySqlParameter("@DonorFirstName", donor.DonorFirstName);
                    param[2] = new MySqlParameter("@DonorMI", donor.DonorMI);
                    param[3] = new MySqlParameter("@DonorLastName", donor.DonorLastName);
                    param[4] = new MySqlParameter("@DonorSuffix", donor.DonorSuffix);
                    param[5] = new MySqlParameter("@DonorSSN", donor.DonorSSN);
                    param[6] = new MySqlParameter("@DonorDateOfBirth", donor.DonorDateOfBirth);
                    param[7] = new MySqlParameter("@DonorPhone1", donor.DonorPhone1);
                    param[8] = new MySqlParameter("@DonorPhone2", donor.DonorPhone2);
                    param[9] = new MySqlParameter("@DonorAddress1", donor.DonorAddress1);
                    param[10] = new MySqlParameter("@DonorAddress2", donor.DonorAddress2);
                    param[11] = new MySqlParameter("@DonorCity", donor.DonorCity);
                    param[12] = new MySqlParameter("@DonorState", donor.DonorState);
                    param[13] = new MySqlParameter("@DonorZip", donor.DonorZip);
                    param[14] = new MySqlParameter("@DonorEmail", donor.DonorEmail);
                    param[15] = new MySqlParameter("@DonorGender", ((int)donor.DonorGender).ToString());
                    param[16] = new MySqlParameter("@DonorRegistrationStatusValue", (int)donor.DonorRegistrationStatusValue);
                    param[17] = new MySqlParameter("@LastModifiedBy", donor.LastModifiedBy);

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                    sqlQuery = "UPDATE users SET "
                                        + "user_first_name = @DonorFirstName, "
                                        + "user_last_name = @DonorLastName, "
                                        + "user_phone_number = @DonorPhone1, "
                                        + "is_synchronized = b'0', "
                                        + "last_modified_on = NOW(), "
                                        + "last_modified_by = @LastModifiedBy "
                                        + "WHERE donor_id = @DonorId";

                    param = new MySqlParameter[5];

                    param[0] = new MySqlParameter("@DonorId", donor.DonorId);
                    param[1] = new MySqlParameter("@DonorFirstName", donor.DonorFirstName);
                    param[2] = new MySqlParameter("@DonorLastName", donor.DonorLastName);
                    param[3] = new MySqlParameter("@DonorPhone1", donor.DonorPhone1);
                    param[4] = new MySqlParameter("@LastModifiedBy", donor.LastModifiedBy);

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                    

                    sqlQuery = "INSERT INTO donor_test_info ("
                                        + "donor_id, "
                                        + "client_id, "
                                        + "client_department_id, "
                                        + "mro_type_id, "
                                        + "payment_type_id, "
                                        + "test_requested_date, "
                                        + "test_requested_by, "
                                        + "test_status, "
                                        + "is_walkin_donor, "
                                        + "is_synchronized, "
                                        + "created_on, "
                                        + "created_by, "
                                        + "last_modified_on, "
                                        + "last_modified_by "
                                        + ") VALUES ( "
                                        + "@DonorId, "
                                        + "@ClientId, "
                                        + "@ClientDepartmentId, "
                                        + "@MROTypeId, "
                                        + "@PaymentTypeId, "
                                        + "NOW(), "
                                        + "@TestRequestedBy, "
                                        + "@TestStatus, "
                                        + "@IsWalkinDonor, "
                                        + "b'0', NOW(), @CreatedBy, NOW(), @LastModifiedBy)";

                    param = new MySqlParameter[10];

                    param[0] = new MySqlParameter("@DonorId", donor.DonorId);
                    param[1] = new MySqlParameter("@ClientId", clientDepartment.ClientId);
                    param[2] = new MySqlParameter("@ClientDepartmentId", clientDepartment.ClientDepartmentId);
                    param[3] = new MySqlParameter("@MROTypeId", clientDepartment.MROTypeId);
                    param[4] = new MySqlParameter("@PaymentTypeId", (int)clientDepartment.PaymentTypeId);
                    param[5] = new MySqlParameter("@TestRequestedBy", currentUserId);
                    param[6] = new MySqlParameter("@IsWalkinDonor", donor.IsWalkinDonor);
                    param[7] = new MySqlParameter("@TestStatus", (int)donor.DonorRegistrationStatusValue);
                    param[8] = new MySqlParameter("@CreatedBy", donor.CreatedBy);
                    param[9] = new MySqlParameter("@LastModifiedBy", donor.LastModifiedBy);

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                    sqlQuery = "SELECT LAST_INSERT_ID()";

                    donorTestInfoId = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlQuery));

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

                    double totalPaymentAmount = 0.0;
                    double totalTestPanelCost = 0.0;

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

                            if (testCategory.TestCategoryId == TestCategories.UA || testCategory.TestCategoryId == TestCategories.Hair)
                            {
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

                                    param = new MySqlParameter[1];
                                    param[0] = new MySqlParameter("@TestPanelId", testPanelId);

                                    testPanelCost = Convert.ToDouble(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlTestPanelCost, param));
                                }
                            }
                            else if (testCategory.TestCategoryId == TestCategories.DNA)
                            {
                                testPanelPrice = testCategory.TestPanelPrice;

                                string sqlTestPanelCost = "SELECT IFNULL(dna_test_panel_cost, 0) FROM cost_master";

                                testPanelCost = Convert.ToDouble(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlTestPanelCost));
                            }

                            param = new MySqlParameter[9];

                            param[0] = new MySqlParameter("@DonorTestInfoId", donorTestInfoId);
                            param[1] = new MySqlParameter("@TestCategoryId", (int)testCategory.TestCategoryId);
                            param[2] = new MySqlParameter("@TestPanelId", testPanelId);
                            param[3] = new MySqlParameter("@HairTestPanelDays", hairTestPanelDays);
                            param[4] = new MySqlParameter("@TestPanelStatus", (int)DonorRegistrationStatus.Registered);
                            param[5] = new MySqlParameter("@TestPanelCost", testPanelCost);
                            param[6] = new MySqlParameter("@TestPanelPrice", testPanelPrice);
                            param[7] = new MySqlParameter("@CreatedBy", donor.CreatedBy);
                            param[8] = new MySqlParameter("@LastModifiedBy", donor.LastModifiedBy);

                            if (testPanelPrice != null)
                            {
                                totalPaymentAmount += Convert.ToDouble(testPanelPrice);
                            }

                            if (testPanelCost != null)
                            {
                                totalTestPanelCost += Convert.ToDouble(testPanelCost);
                            }

                            SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlTestCategory, param);
                        }
                        if (clientDepartment.MROTypeId == ClientMROTypes.MALL)
                        {
                            sqlQuery = "UPDATE donor_test_info SET total_payment_amount = @TotalPaymentAmount, laboratory_cost = @TotalTestPanelCost, mro_cost = @MROCost WHERE donor_test_info_id = @DonorTestInfoId";

                            param = new MySqlParameter[4];

                            param[0] = new MySqlParameter("@DonorTestInfoId", donorTestInfoId);
                            param[1] = new MySqlParameter("@TotalPaymentAmount", totalPaymentAmount);
                            param[2] = new MySqlParameter("@TotalTestPanelCost", totalTestPanelCost);
                            param[3] = new MySqlParameter("@MROCost", mallCost);
                        }
                        if (clientDepartment.MROTypeId == ClientMROTypes.MPOS)
                        {
                            sqlQuery = "UPDATE donor_test_info SET total_payment_amount = @TotalPaymentAmount, laboratory_cost = @TotalTestPanelCost, mro_cost = @MROCost WHERE donor_test_info_id = @DonorTestInfoId";

                            param = new MySqlParameter[4];

                            param[0] = new MySqlParameter("@DonorTestInfoId", donorTestInfoId);
                            param[1] = new MySqlParameter("@TotalPaymentAmount", totalPaymentAmount);
                            param[2] = new MySqlParameter("@TotalTestPanelCost", totalTestPanelCost);
                            param[3] = new MySqlParameter("@MROCost", mposCost);
                        }

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

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

                            param = new MySqlParameter[7];

                            param[0] = new MySqlParameter("@DonorTestInfoId", donorTestInfoId);
                            param[1] = new MySqlParameter("@PaymentStatus", (int)PaymentStatus.Paid);
                            param[2] = new MySqlParameter("@PaymentNote", client.ClientName);
                            param[3] = new MySqlParameter("@PaymentMethodId", (int)PaymentMethod.Cash);
                            param[4] = new MySqlParameter("@PaymentDate", date);
                            param[5] = new MySqlParameter("@LastModifiedBy", donor.LastModifiedBy);
                            param[6] = new MySqlParameter("@IsPaymentReceived", 1);

                            SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);
                        }

                        donor.ProgramAmount = totalPaymentAmount;
                    }

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

                    if (!donor.IsWalkinDonor)
                    {
                        param = new MySqlParameter[5];

                        param[0] = new MySqlParameter("@DonorTestInfoId", donorTestInfoId);
                        param[1] = new MySqlParameter("@ActivityUserId", currentUserId);
                        param[2] = new MySqlParameter("@ActivityCategoryId", (int)DonorActivityCategories.General);
                        param[3] = new MySqlParameter("@IsActivityVisible", true);
                        param[4] = new MySqlParameter("@ActivityNote", "New test registered" + (totalPaymentAmount > 0 ? " (Payment Due Amount: $" + totalPaymentAmount.ToString() + ")" : ""));

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);
                    }
                    else
                    {
                        string donorActivityNote = string.Empty;

                        // donorActivityNote = "Donor is registered as a walk-in donor to take instant test result";
                        param = new MySqlParameter[5];

                        param[0] = new MySqlParameter("@DonorTestInfoId", donorTestInfoId);
                        param[1] = new MySqlParameter("@ActivityUserId", currentUserId);
                        param[2] = new MySqlParameter("@ActivityCategoryId", (int)DonorActivityCategories.General);
                        param[3] = new MySqlParameter("@IsActivityVisible", true);
                        param[4] = new MySqlParameter("@ActivityNote", "New test registered. Donor is registered as a walk-in donor to take instant test" + (totalPaymentAmount > 0 ? " (Payment Due Amount: $" + totalPaymentAmount.ToString() + ")" : ""));

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);
                    }
                    if (clientDepartment.PaymentTypeId == ClientPaymentTypes.InvoiceClient)
                    {
                        sqlQuery = "SELECT user_id FROM users where UPPER(user_name) = UPPER(@DonorEmail) and is_archived=0";

                        param = new MySqlParameter[1];

                        param[0] = new MySqlParameter("@DonorEmail", donor.LastModifiedBy);

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

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);
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

        public double AddTest(Donor donor, ClientDepartment clientDepartment, User user)
        {
            if (clientDepartment.ClientId < 1 || clientDepartment.ClientDepartmentId < 1) throw new Exception("ERROR: Client ID and Client Department ID are required");

            double totalPaymentAmount = 0.0;

            ClientDao clientDao = new ClientDao();
            Client client = null;

            VendorDao vendorDao = new VendorDao();
            Vendor vendor = null;

            double mallCost = 0.0;
            double mposCost = 0.0;
            donor.LastModifiedBy = user.Username.ToString();

            if (clientDepartment.ClientId != null)
            {
                client = clientDao.Get(clientDepartment.ClientId);

                if (client != null)
                {
                    if (client.MROVendorId != null)
                    {
                        vendor = vendorDao.Get(Convert.ToInt32(client.MROVendorId));

                        if (vendor != null && vendor.MALLMROCost != null)
                        {
                            mallCost = Convert.ToDouble(vendor.MALLMROCost);
                        }
                        if (vendor != null && vendor.MPOSMROCost != null)
                        {
                            mposCost = Convert.ToDouble(vendor.MPOSMROCost);
                        }
                    }
                }
                if (clientDepartment.PaymentTypeId == ClientPaymentTypes.InvoiceClient)
                {
                    donor.DonorRegistrationStatusValue = DonorRegistrationStatus.InQueue;
                }
                else
                {
                    donor.DonorRegistrationStatusValue = DonorRegistrationStatus.Registered;
                }
            }
            
            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();

                try
                {


                    string sqlQuery = "INSERT INTO donor_test_info ("
                                        + "donor_id, "
                                        + "client_id, "
                                        + "client_department_id, "
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
                                        + "@MROTypeId, "
                                        + "@PaymentTypeId, "
                                        + "NOW(), "
                                        + "@TestRequestedBy, "
                                        + "@TestStatus, "
                                        + "b'0', NOW(), @CreatedBy, NOW(), @LastModifiedBy)";

                    MySqlParameter[] param = new MySqlParameter[9];

                    param[0] = new MySqlParameter("@DonorId", donor.DonorId);
                    param[1] = new MySqlParameter("@ClientId", clientDepartment.ClientId);
                    param[2] = new MySqlParameter("@ClientDepartmentId", clientDepartment.ClientDepartmentId);
                    param[3] = new MySqlParameter("@MROTypeId", clientDepartment.MROTypeId);
                    param[4] = new MySqlParameter("@PaymentTypeId", (int)clientDepartment.PaymentTypeId);
                    param[5] = new MySqlParameter("@TestRequestedBy", user.UserId);
                    param[6] = new MySqlParameter("@TestStatus", (int)donor.DonorRegistrationStatusValue);
                    param[7] = new MySqlParameter("@CreatedBy", user.Username);
                    param[8] = new MySqlParameter("@LastModifiedBy", user.Username);

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                    sqlQuery = "SELECT LAST_INSERT_ID()";

                    int donorTestInfoId = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlQuery));

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

                    double totalTestPanelCost = 0.0;

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

                            if (testCategory.TestCategoryId == TestCategories.UA || testCategory.TestCategoryId == TestCategories.Hair)
                            {
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

                                    param = new MySqlParameter[1];
                                    param[0] = new MySqlParameter("@TestPanelId", testPanelId);

                                    testPanelCost = Convert.ToDouble(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlTestPanelCost, param));
                                }
                            }
                            else if (testCategory.TestCategoryId == TestCategories.DNA)
                            {
                                testPanelPrice = testCategory.TestPanelPrice;

                                string sqlTestPanelCost = "SELECT IFNULL(dna_test_panel_cost, 0) FROM cost_master";

                                testPanelCost = Convert.ToDouble(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlTestPanelCost));
                            }

                            param = new MySqlParameter[9];

                            param[0] = new MySqlParameter("@DonorTestInfoId", donorTestInfoId);
                            param[1] = new MySqlParameter("@TestCategoryId", (int)testCategory.TestCategoryId);
                            param[2] = new MySqlParameter("@TestPanelId", testPanelId);
                            param[3] = new MySqlParameter("@HairTestPanelDays", hairTestPanelDays);
                            param[4] = new MySqlParameter("@TestPanelStatus", (int)DonorRegistrationStatus.Registered);
                            param[5] = new MySqlParameter("@TestPanelCost", testPanelCost);
                            param[6] = new MySqlParameter("@TestPanelPrice", testPanelPrice);
                            param[7] = new MySqlParameter("@CreatedBy", user.Username);
                            param[8] = new MySqlParameter("@LastModifiedBy", user.Username);

                            if (testPanelPrice != null)
                            {
                                totalPaymentAmount += Convert.ToDouble(testPanelPrice);
                            }

                            if (testPanelCost != null)
                            {
                                totalTestPanelCost += Convert.ToDouble(testPanelCost);
                            }

                            SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlTestCategory, param);
                        }
                        if (clientDepartment.MROTypeId == ClientMROTypes.MALL)
                        {
                            sqlQuery = "UPDATE donor_test_info SET total_payment_amount = @TotalPaymentAmount, laboratory_cost = @TotalTestPanelCost, mro_cost = @MROCost WHERE donor_test_info_id = @DonorTestInfoId";

                            param = new MySqlParameter[4];

                            param[0] = new MySqlParameter("@DonorTestInfoId", donorTestInfoId);
                            param[1] = new MySqlParameter("@TotalPaymentAmount", totalPaymentAmount);
                            param[2] = new MySqlParameter("@TotalTestPanelCost", totalTestPanelCost);
                            param[3] = new MySqlParameter("@MROCost", mallCost);
                        }
                        if (clientDepartment.MROTypeId == ClientMROTypes.MPOS)
                        {
                            sqlQuery = "UPDATE donor_test_info SET total_payment_amount = @TotalPaymentAmount, laboratory_cost = @TotalTestPanelCost, mro_cost = @MROCost WHERE donor_test_info_id = @DonorTestInfoId";

                            param = new MySqlParameter[4];

                            param[0] = new MySqlParameter("@DonorTestInfoId", donorTestInfoId);
                            param[1] = new MySqlParameter("@TotalPaymentAmount", totalPaymentAmount);
                            param[2] = new MySqlParameter("@TotalTestPanelCost", totalTestPanelCost);
                            param[3] = new MySqlParameter("@MROCost", mposCost);
                        }

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

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

                            param = new MySqlParameter[7];

                            param[0] = new MySqlParameter("@DonorTestInfoId", donorTestInfoId);
                            param[1] = new MySqlParameter("@PaymentStatus", (int)PaymentStatus.Paid);
                            param[2] = new MySqlParameter("@PaymentNote", client.ClientName);
                            param[3] = new MySqlParameter("@PaymentMethodId", (int)PaymentMethod.Cash);
                            param[4] = new MySqlParameter("@PaymentDate", date);
                            param[5] = new MySqlParameter("@LastModifiedBy", donor.LastModifiedBy);
                            param[6] = new MySqlParameter("@IsPaymentReceived", 1);

                            SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);
                        }
                        donor.DonorTestInfoId = donorTestInfoId;
                        donor.ProgramAmount = totalPaymentAmount;
                    }

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
                    param[1] = new MySqlParameter("@ActivityUserId", user.UserId);
                    param[2] = new MySqlParameter("@ActivityCategoryId", (int)DonorActivityCategories.General);
                    param[3] = new MySqlParameter("@IsActivityVisible", true);
                    param[4] = new MySqlParameter("@ActivityNote", "New test registered" + (totalPaymentAmount > 0 ? " (Payment Due Amount: $" + totalPaymentAmount.ToString() + ")" : ""));

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                    if (clientDepartment.PaymentTypeId == ClientPaymentTypes.InvoiceClient)
                    {
                        sqlQuery = "SELECT user_id FROM users where UPPER(user_name) = UPPER(@DonorEmail) and is_archived=0";

                        param = new MySqlParameter[1];

                        param[0] = new MySqlParameter("@DonorEmail", donor.LastModifiedBy);

                        int currentUserId = (int)SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlQuery, param);

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

            return totalPaymentAmount;
        }

        public void AddDonorActivityNote(DonorActivityNote donorActivityNote)
        {
            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();

                try
                {
                    string sqlQuery = "INSERT INTO donor_test_activity ("
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

                    MySqlParameter[] param = new MySqlParameter[5];

                    param[0] = new MySqlParameter("@DonorTestInfoId", donorActivityNote.DonorTestInfoId);
                    param[1] = new MySqlParameter("@ActivityUserId", donorActivityNote.ActivityUserId);
                    param[2] = new MySqlParameter("@ActivityCategoryId", (int)DonorActivityCategories.General);
                    param[3] = new MySqlParameter("@IsActivityVisible", donorActivityNote.IsActivityVisible);
                    param[4] = new MySqlParameter("@ActivityNote", donorActivityNote.ActivityNote);

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
        }

        #endregion Public Methods

        private void logDebug(string _t)
        {
            _logger.Debug($"{this.SessionID} - DonorDao - {_t.ToString()}");
        }

        private void LogAnError(Exception ex)
        {
            _logger.Error("- DonorDao - ERROR");
            _logger.Error(ex.Message);
            if (!(ex.InnerException == null)) _logger.Error(ex.InnerException.ToString());
        }
    }
}