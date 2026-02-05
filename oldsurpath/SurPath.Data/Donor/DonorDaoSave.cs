using MySql.Data.MySqlClient;
using SurPath.Entity;
using SurPath.Enum;
using SurPath.MySQLHelper;
using System;
using System.Data;

namespace SurPath.Data
{
    public partial class DonorDao : DataObject
    {
        public void SavePaymentDetails(DonorTestInfo donorTestInfo, string fromWeb = null, PaymentOverride paymentOverride = null)
        {
            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();

                try
                {

                    string sqlQuery = "UPDATE donor_test_info SET "
                        + "payment_date = @PaymentDate, "
                        + "payment_method_id = @PaymentMethodId, "
                        + "payment_note = @PaymentNote, "
                        + "payment_status = @PaymentStatus, "
                        + "payment_received = @IsPaymentReceived, "
                        + "test_status = @TestStatus, "
                        + "is_synchronized = b'0', "
                        + "last_modified_on = NOW(), "
                        + "last_modified_by = @LastModifiedBy "
                        + "WHERE donor_test_info_id = @DonorTestInfoId";

                    ParamHelper p = new ParamHelper();
                    //MySqlParameter[] param = new MySqlParameter[8];

                    p.Param = new MySqlParameter("@DonorTestInfoId", donorTestInfo.DonorTestInfoId);
                    p.Param = new MySqlParameter("@TestStatus", (int)donorTestInfo.TestStatus);
                    p.Param = new MySqlParameter("@PaymentStatus", (int)donorTestInfo.PaymentStatus);
                    p.Param = new MySqlParameter("@PaymentNote", donorTestInfo.PaymentNote);
                    p.Param = new MySqlParameter("@PaymentMethodId", (int)donorTestInfo.PaymentMethodId);
                    p.Param = new MySqlParameter("@PaymentDate", donorTestInfo.PaymentDate);
                    p.Param = new MySqlParameter("@LastModifiedBy", donorTestInfo.LastModifiedBy);
                    p.Param = new MySqlParameter("@IsPaymentReceived", donorTestInfo.IsPaymentReceived);
                    
                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, p.Params);

                    if (paymentOverride!=null)
                    {
                        // set totals from the override
                        sqlQuery = "UPDATE donor_test_info SET "
                        + "total_payment_amount = @TotalPaymentAmount, "
                        + "payment_note = @PaymentNote, "
                        + "last_modified_on = NOW(), "
                        + "last_modified_by = @LastModifiedBy "
                        + "WHERE donor_test_info_id = @DonorTestInfoId";

                        var totalpaymentamount = double.Parse(paymentOverride.PaymentAmount);
                        var _note = $"[Override {paymentOverride.TransID}]";
                        if (donorTestInfo.PaymentNote != null)
                        {
                            _note = donorTestInfo.PaymentNote + " " + _note;
                        }

                        p.reset();
                        p.Param = new MySqlParameter("@DonorTestInfoId", donorTestInfo.DonorTestInfoId);
                        p.Param = new MySqlParameter("@TotalPaymentAmount", totalpaymentamount);
                        p.Param = new MySqlParameter("@PaymentNote", _note);
                        p.Param = new MySqlParameter("@LastModifiedBy", donorTestInfo.LastModifiedBy);
                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, p.Params);

                    }

                    sqlQuery = "SELECT user_id FROM users where UPPER(user_name) = UPPER(@DonorEmail) and is_archived=0";
                    p.reset();
                    p.Param = new MySqlParameter("@DonorEmail", donorTestInfo.LastModifiedBy);

                    int currentUserId = (int)SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlQuery, p.Params);

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

                    p.reset();

                    p.Param = new MySqlParameter("@DonorTestInfoId", donorTestInfo.DonorTestInfoId);
                    p.Param = new MySqlParameter("@ActivityUserId", currentUserId);
                    p.Param = new MySqlParameter("@ActivityCategoryId", (int)DonorActivityCategories.General);
                    p.Param = new MySqlParameter("@IsActivityVisible", true);
                    if (fromWeb == "Yes" && donorTestInfo.PaymentMethodId == PaymentMethod.Cash)
                    {
                        p.Param = new MySqlParameter("@ActivityNote", string.Format("Due amount...$ {0}.", donorTestInfo.TotalPaymentAmount.ToString()));
                    }
                    else
                    {
                        p.Param = new MySqlParameter("@ActivityNote", string.Format("Payment made...$ {0}.", donorTestInfo.TotalPaymentAmount.ToString()));
                    }
                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, p.Params);

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
        }

        public void SaveReversePaymentDetails(DonorTestInfo donorTestInfo)
        {
            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();

                try
                {
                    string sqlQuery = "UPDATE donor_test_info SET "
                        + "payment_date = @PaymentDate, "
                        + "payment_method_id = @PaymentMethodId, "
                        + "payment_note = @PaymentNote, "
                        + "payment_status = @PaymentStatus, "
                        + "payment_received = @IsPaymentReceived, "
                        + "test_status = @TestStatus, "
                        + "is_synchronized = b'0', "
                        + "last_modified_on = NOW(), "
                        + "last_modified_by = @LastModifiedBy "
                        + "WHERE donor_test_info_id = @DonorTestInfoId";

                    MySqlParameter[] param = new MySqlParameter[8];

                    param[0] = new MySqlParameter("@DonorTestInfoId", donorTestInfo.DonorTestInfoId);
                    param[1] = new MySqlParameter("@TestStatus", (int)donorTestInfo.TestStatus);
                    param[2] = new MySqlParameter("@PaymentStatus", (int)donorTestInfo.PaymentStatus);
                    param[3] = new MySqlParameter("@PaymentNote", donorTestInfo.PaymentNote);
                    param[4] = new MySqlParameter("@PaymentMethodId", (int)donorTestInfo.PaymentMethodId);
                    param[5] = new MySqlParameter("@PaymentDate", donorTestInfo.PaymentDate);
                    param[6] = new MySqlParameter("@LastModifiedBy", donorTestInfo.LastModifiedBy);
                    param[7] = new MySqlParameter("@IsPaymentReceived", donorTestInfo.IsPaymentReceived);

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                    sqlQuery = "SELECT user_id FROM users where UPPER(user_name) = UPPER(@DonorEmail) and is_archived=0";

                    param = new MySqlParameter[1];

                    param[0] = new MySqlParameter("@DonorEmail", donorTestInfo.LastModifiedBy);

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

                    param[0] = new MySqlParameter("@DonorTestInfoId", donorTestInfo.DonorTestInfoId);
                    param[1] = new MySqlParameter("@ActivityUserId", currentUserId);
                    param[2] = new MySqlParameter("@ActivityCategoryId", (int)DonorActivityCategories.General);
                    param[3] = new MySqlParameter("@IsActivityVisible", true);
                    //if (fromWeb == "Yes" && donorTestInfo.PaymentMethodId == PaymentMethod.Cash)
                    //{
                    //    param[4] = new MySqlParameter("@ActivityNote", string.Format("Due amount...$ {0}.", donorTestInfo.TotalPaymentAmount.ToString()));

                    //}
                    // else
                    // {
                    param[4] = new MySqlParameter("@ActivityNote", string.Format("Payment made...$ {0}.", donorTestInfo.TotalPaymentAmount.ToString()));
                    // }
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

        public void SaveTestInfoDetailsBeforPayment(DonorTestInfo donorTestInfo)
        {
            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();

                try
                {
                    string sqlDelQuery = "DELETE FROM donor_test_info_test_categories WHERE donor_test_info_id = @DonorTestInfoId";
                    MySqlParameter[] param = new MySqlParameter[1];

                    param[0] = new MySqlParameter("@DonorTestInfoId", donorTestInfo.DonorTestInfoId);
                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlDelQuery, param);

                    string sqlTestCategory = "INSERT INTO donor_test_info_test_categories ("
                                                + "donor_test_info_id, test_category_id, test_panel_id, hair_test_panel_days, test_panel_status, "
                                                + "test_panel_cost, test_panel_price, is_synchronized, created_on, created_by, last_modified_on, last_modified_by) "
                                                + "VALUES (@DonorTestInfoId, @TestCategoryId, @TestPanelId, @HairTestPanelDays, @TestPanelStatus, @TestPanelCost, "
                                                + "@TestPanelPrice, b'0', NOW(), @CreatedBy, NOW(), @LastModifiedBy)";

                    double totalPaymentAmount = 0.0;
                    double totalTestPanelCost = 0.0;
                    bool IsHairUpdated = true;
                    bool IsUAUpdated = true;
                    bool IsDNAUpdated = true;
                    bool IsBCUpdated = true;

                    foreach (DonorTestInfoTestCategories testCategory in donorTestInfo.TestInfoTestCategories)
                    {
                        int? testPanelId = null;
                        int? hairTestPanelDays = null;
                        int? testCategoryId = null;
                        double? testPanelCost = null;
                        double? testPanelPrice = null;
                        bool makeEntry = false;

                        if (donorTestInfo.IsUA == true && IsUAUpdated == true)
                        {
                            if (testCategory.TestCategoryId == TestCategories.UA)
                            {
                                IsUAUpdated = false;
                                makeEntry = true;
                                testPanelId = testCategory.TestPanelId;
                                testPanelPrice = testCategory.TestPanelPrice;
                                testCategoryId = (int)TestCategories.UA;
                                if (testPanelId != null)
                                {
                                    string sqlTestPanelCost = "SELECT IFNULL(cost, 0) FROM test_panels WHERE test_panel_id = @TestPanelId";

                                    param = new MySqlParameter[1];
                                    param[0] = new MySqlParameter("@TestPanelId", testPanelId);

                                    testPanelCost = Convert.ToDouble(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlTestPanelCost, param));
                                }
                            }
                        }
                        else if (donorTestInfo.IsHair == true && IsHairUpdated == true)
                        {
                            if (testCategory.TestCategoryId == TestCategories.Hair)
                            {
                                IsHairUpdated = false;
                                makeEntry = true;
                                testPanelId = testCategory.TestPanelId;
                                testPanelPrice = testCategory.TestPanelPrice;
                                testCategoryId = (int)TestCategories.Hair;
                                if (testPanelId != null)
                                {
                                    string sqlTestPanelCost = "SELECT IFNULL(cost, 0) FROM test_panels WHERE test_panel_id = @TestPanelId";

                                    param = new MySqlParameter[1];
                                    param[0] = new MySqlParameter("@TestPanelId", testPanelId);

                                    testPanelCost = Convert.ToDouble(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlTestPanelCost, param));
                                }
                                hairTestPanelDays = testCategory.HairTestPanelDays;
                            }
                        }
                        else if (donorTestInfo.IsDNA == true && IsDNAUpdated == true)
                        {
                            if (testCategory.TestCategoryId == TestCategories.DNA)
                            {
                                IsDNAUpdated = false;
                                makeEntry = true;
                                testPanelPrice = testCategory.TestPanelPrice;
                                testCategoryId = (int)TestCategories.DNA;

                                string sqlTestPanelCost = "SELECT IFNULL(dna_test_panel_cost, 0) FROM cost_master";

                                testPanelCost = Convert.ToDouble(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlTestPanelCost));
                            }
                        }
                        else if (donorTestInfo.IsBC == true && IsBCUpdated == true)
                        {
                            if (testCategory.TestCategoryId == TestCategories.BC)
                            {
                                IsBCUpdated = false;
                                makeEntry = true;
                                testPanelPrice = testCategory.TestPanelPrice;
                                testCategoryId = (int)TestCategories.BC;

                                string sqlTestPanelCost = "SELECT IFNULL(bc_test_panel_cost, 0) FROM cost_master";

                                testPanelCost = Convert.ToDouble(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlTestPanelCost));
                            }
                        }

                        param = new MySqlParameter[9];

                        param[0] = new MySqlParameter("@DonorTestInfoId", donorTestInfo.DonorTestInfoId);
                        param[1] = new MySqlParameter("@TestCategoryId", testCategoryId);
                        param[2] = new MySqlParameter("@TestPanelId", testPanelId);
                        param[3] = new MySqlParameter("@HairTestPanelDays", hairTestPanelDays);
                        param[4] = new MySqlParameter("@TestPanelStatus", (int)DonorRegistrationStatus.Registered);
                        param[5] = new MySqlParameter("@TestPanelCost", testPanelCost);
                        param[6] = new MySqlParameter("@TestPanelPrice", testPanelPrice);
                        param[7] = new MySqlParameter("@CreatedBy", donorTestInfo.LastModifiedBy);
                        param[8] = new MySqlParameter("@LastModifiedBy", donorTestInfo.LastModifiedBy);

                        if (testPanelPrice != null)
                        {
                            totalPaymentAmount += Convert.ToDouble(testPanelPrice);
                        }

                        if (testPanelCost != null)
                        {
                            totalTestPanelCost += Convert.ToDouble(testPanelCost);
                        }
                        if (makeEntry)
                        {
                            SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlTestCategory, param);
                        }
                    }

                    string sqlQuery = "UPDATE donor_test_info SET total_payment_amount = @TotalPaymentAmount, "
                          + "is_ua = @IsUA, "
                          + "is_hair = @IsHair, "
                          + "is_dna = @IsDNA, "
                          + "is_bc = @IsBC, "
                          + "last_modified_on = NOW(), "
                          + "last_modified_by = @LastModifiedBy, "
                          + "laboratory_cost = @TotalTestPanelCost WHERE donor_test_info_id = @DonorTestInfoId";

                    param = new MySqlParameter[8];
                    param[0] = new MySqlParameter("@DonorTestInfoId", donorTestInfo.DonorTestInfoId);
                    param[1] = new MySqlParameter("@TotalPaymentAmount", totalPaymentAmount);
                    param[2] = new MySqlParameter("@TotalTestPanelCost", totalTestPanelCost);
                    param[3] = new MySqlParameter("@IsUA", donorTestInfo.IsUA);
                    param[4] = new MySqlParameter("@IsHair", donorTestInfo.IsHair);
                    param[5] = new MySqlParameter("@IsDNA", donorTestInfo.IsDNA);
                    param[6] = new MySqlParameter("@LastModifiedBy", donorTestInfo.LastModifiedBy);
                    param[5] = new MySqlParameter("@IsBC", donorTestInfo.IsBC);

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                    //Activity Note
                    sqlQuery = "SELECT user_id FROM users where UPPER(user_name) = UPPER(@DonorEmail) and is_archived=0";

                    param = new MySqlParameter[1];

                    param[0] = new MySqlParameter("@DonorEmail", donorTestInfo.LastModifiedBy);

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

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
        }

        public void SaveTestInfoDetails(DonorTestInfo donorTestInfo)
        {
            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();

                try
                {
                    string sqlQuery = "UPDATE donor_test_info SET "
                        + "is_ua = @IsUA, "
                        + "is_hair = @IsHair, "
                        + "is_dna = @IsDNA, "
                        + "is_bc = @IsBC, "
                        + "reason_for_test_id = @ReasonForTestId, "
                        + "other_reason = @OtherReason, "
                        + "is_temperature_in_range = @IsTemperatureInRange, "
                        + "temperature_of_specimen = @TemperatureOfSpecimen, "
                        + "testing_authority_id = @TestingAuthorityId, "
                        + "specimen_collection_cup_id = @SpecimenCollectionCupId, "
                        + "is_observed = @IsObserved, "
                        + "form_type_id = @FormTypeId, "
                        + "is_adulteration_sign = @IsAdulterationSign, "
                        + "is_quantity_sufficient = @IsQuantitySufficient, "
                        + "collection_site_vendor_id = @CollectionSiteVendorId, "
                        + "collection_site_location_id = @CollectionSiteLocationId, "
                        + "collection_site_user_id = @CollectionSiteUserId, "
                        + "screening_time = {0}, "
                        + "is_donor_refused = @IsDonorRefused, "
                        + "collection_site_remarks = @CollectionSiteRemarks, "
                        //    + "collection_site_remarks = @CollectionSiteRemarks, "
                        + "total_payment_amount = @TotalPaymentAmount, "
                        + "vendor_cost = @VendorCost, "
                        + "test_status = @TestStatus, "
                        + "is_instant_test = @IsInstantTest, "
                        + "instant_test_result = @InstantTestResult, "
                        + "test_overall_result = @TestOverallResult, "
                        + "is_synchronized = b'0', "
                        + "last_modified_on = NOW(), "
                        + "last_modified_by = @LastModifiedBy "
                        + "WHERE donor_test_info_id = @DonorTestInfoId";

                    MySqlParameter[] param = new MySqlParameter[27];

                    param[0] = new MySqlParameter("@DonorTestInfoId", donorTestInfo.DonorTestInfoId);

                    param[1] = new MySqlParameter("@IsUA", donorTestInfo.IsUA);
                    param[2] = new MySqlParameter("@IsHair", donorTestInfo.IsHair);
                    param[3] = new MySqlParameter("@IsDNA", donorTestInfo.IsDNA);

                    param[4] = new MySqlParameter("@ReasonForTestId", donorTestInfo.ReasonForTestId);
                    param[5] = new MySqlParameter("@OtherReason", donorTestInfo.OtherReason);

                    param[6] = new MySqlParameter("@SpecimenCollectionCupId", donorTestInfo.SpecimenCollectionCupId);
                    param[7] = new MySqlParameter("@IsObserved", donorTestInfo.IsObserved);
                    param[8] = new MySqlParameter("@FormTypeId", donorTestInfo.FormTypeId);
                    param[9] = new MySqlParameter("@TestingAuthorityId", donorTestInfo.TestingAuthorityId);

                    param[10] = new MySqlParameter("@IsTemperatureInRange", donorTestInfo.IsTemperatureInRange);
                    param[11] = new MySqlParameter("@TemperatureOfSpecimen", donorTestInfo.TemperatureOfSpecimen);

                    param[12] = new MySqlParameter("@IsAdulterationSign", donorTestInfo.IsAdulterationSign);
                    param[13] = new MySqlParameter("@IsQuantitySufficient", donorTestInfo.IsQuantitySufficient);

                    param[14] = new MySqlParameter("@CollectionSiteVendorId", donorTestInfo.CollectionSiteVendorId);
                    param[15] = new MySqlParameter("@CollectionSiteLocationId", donorTestInfo.CollectionSiteLocationId);
                    param[16] = new MySqlParameter("@CollectionSiteUserId", donorTestInfo.CollectionSiteUserId);

                    if (donorTestInfo.TestStatus == DonorRegistrationStatus.Processing || (donorTestInfo.InstantTestResult == InstantTestResult.Negative && donorTestInfo.TestStatus == DonorRegistrationStatus.Completed))
                    {
                        sqlQuery = string.Format(sqlQuery, "NOW()");
                    }
                    else
                    {
                        sqlQuery = string.Format(sqlQuery, "NULL");
                    }

                    param[17] = new MySqlParameter("@IsDonorRefused", donorTestInfo.IsDonorRefused);
                    param[18] = new MySqlParameter("@CollectionSiteRemarks", donorTestInfo.CollectionSiteRemarks);

                    param[19] = new MySqlParameter("@TotalPaymentAmount", donorTestInfo.TotalPaymentAmount);
                    param[20] = new MySqlParameter("@VendorCost", donorTestInfo.VendorCost);

                    param[21] = new MySqlParameter("@TestStatus", (int)donorTestInfo.TestStatus);

                    param[22] = new MySqlParameter("@IsInstantTest", donorTestInfo.IsInstantTest);
                    param[23] = new MySqlParameter("@InstantTestResult", donorTestInfo.InstantTestResult);
                    param[24] = new MySqlParameter("@TestOverallResult", donorTestInfo.TestOverallResult);
                    param[25] = new MySqlParameter("@LastModifiedBy", donorTestInfo.LastModifiedBy);
                    param[26] = new MySqlParameter("@IsBC", donorTestInfo.IsBC);

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                    //Test Category
                    string specimenId = string.Empty;
                    //if (donorTestInfo.TestStatus == DonorRegistrationStatus.Processing)
                    //{
                    sqlQuery = "UPDATE donor_test_info_test_categories SET "
                                + "specimen_id = @SpecimenId, "
                                + "hair_test_panel_days = @HairTestPanelDays, "
                                + "test_panel_price = @TestPanelPrice, "
                                + "test_panel_status = @TestPanelStatus, "
                                + "is_synchronized = b'0', "
                                + "last_modified_on = NOW(), "
                                + "last_modified_by = @LastModifiedBy "
                                + "WHERE donor_test_test_category_id = @DonorTestTestCategoryId";

                    foreach (DonorTestInfoTestCategories donorTestCategory in donorTestInfo.TestInfoTestCategories)
                    {
                        param = new MySqlParameter[6];

                        param[0] = new MySqlParameter("@DonorTestTestCategoryId", donorTestCategory.DonorTestTestCategoryId);
                        param[1] = new MySqlParameter("@SpecimenId", donorTestCategory.SpecimenId);
                        param[2] = new MySqlParameter("@HairTestPanelDays", donorTestCategory.HairTestPanelDays);
                        param[3] = new MySqlParameter("@TestPanelPrice", donorTestCategory.TestPanelPrice);
                        param[4] = new MySqlParameter("@TestPanelStatus", (int)donorTestInfo.TestStatus);
                        param[5] = new MySqlParameter("@LastModifiedBy", donorTestInfo.LastModifiedBy);

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                        if (donorTestInfo.TestStatus == DonorRegistrationStatus.Processing || (donorTestInfo.IsDonorRefused == false && donorTestInfo.TestStatus == DonorRegistrationStatus.Completed && donorTestInfo.InstantTestResult == InstantTestResult.Negative))
                        {
                            if (donorTestCategory.TestCategoryId == TestCategories.UA || donorTestCategory.TestCategoryId == TestCategories.Hair)
                            {
                                if (donorTestCategory.SpecimenId.Trim() != string.Empty && donorTestCategory.SpecimenId.Trim() != null)
                                {
                                    if (specimenId.Trim() == string.Empty)
                                    {
                                        specimenId += donorTestCategory.SpecimenId.Trim();
                                    }
                                    else
                                    {
                                        specimenId += ", " + donorTestCategory.SpecimenId.Trim();
                                    }
                                }
                            }
                        }
                    }
                    //}

                    //Activity Note
                    sqlQuery = "SELECT user_id FROM users where UPPER(user_name) = UPPER(@DonorEmail) and is_archived=0";

                    param = new MySqlParameter[1];

                    param[0] = new MySqlParameter("@DonorEmail", donorTestInfo.LastModifiedBy);

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

                    string donorActivityNote = string.Empty;

                    if (donorTestInfo.IsTemperatureInRange != YesNo.None)
                    {
                        if (donorTestInfo.IsTemperatureInRange == YesNo.No)
                        {
                            donorActivityNote = "Specimen Collected with the temperature of {0}.";
                        }
                        else
                        {
                            donorActivityNote = "Specimen Collected with the temperature range of acceptence.";
                        }

                        param = new MySqlParameter[5];

                        param[0] = new MySqlParameter("@DonorTestInfoId", donorTestInfo.DonorTestInfoId);
                        param[1] = new MySqlParameter("@ActivityUserId", currentUserId);
                        param[2] = new MySqlParameter("@ActivityCategoryId", (int)DonorActivityCategories.General);
                        param[3] = new MySqlParameter("@IsActivityVisible", true);
                        param[4] = new MySqlParameter("@ActivityNote", string.Format(donorActivityNote, donorTestInfo.TemperatureOfSpecimen.ToString()));

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);
                    }

                    if (!donorTestInfo.IsInstantTest)
                    {
                        if (specimenId != string.Empty)
                        {
                            donorActivityNote = "Test Info updated with the status of {0} and the Specimen ID(s) of {1}.";
                        }
                        else
                        {
                            donorActivityNote = "Test Info updated with the status of {0}.";
                        }

                        param = new MySqlParameter[5];

                        param[0] = new MySqlParameter("@DonorTestInfoId", donorTestInfo.DonorTestInfoId);
                        param[1] = new MySqlParameter("@ActivityUserId", currentUserId);
                        param[2] = new MySqlParameter("@ActivityCategoryId", (int)DonorActivityCategories.General);
                        param[3] = new MySqlParameter("@IsActivityVisible", true);
                        param[4] = new MySqlParameter("@ActivityNote", string.Format(donorActivityNote, donorTestInfo.TestStatus.ToDescriptionString(), specimenId));

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);
                    }
                    else
                    {
                        if (donorTestInfo.IsInstantTest && donorTestInfo.InstantTestResult == InstantTestResult.Positive)
                        {
                            donorActivityNote = "Test Info updated with the status of {0} instant test result as {1} and the Specimen ID(s) as {2}.The donor has to take a regular test.";
                        }
                        else if (donorTestInfo.IsInstantTest && donorTestInfo.InstantTestResult == InstantTestResult.Negative)
                        {
                            donorActivityNote = "Test Info updated with the  status of {0} instant test result as {1} and the Specimen ID(s) as {2}.";
                        }

                        param = new MySqlParameter[5];

                        param[0] = new MySqlParameter("@DonorTestInfoId", donorTestInfo.DonorTestInfoId);
                        param[1] = new MySqlParameter("@ActivityUserId", currentUserId);
                        param[2] = new MySqlParameter("@ActivityCategoryId", (int)DonorActivityCategories.General);
                        param[3] = new MySqlParameter("@IsActivityVisible", true);
                        param[4] = new MySqlParameter("@ActivityNote", string.Format(donorActivityNote, donorTestInfo.TestStatus.ToDescriptionString(), donorTestInfo.InstantTestResult, specimenId));

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
        }

        public void SaveReverseEntryTestInfoDetails(DonorTestInfo donorTestInfo, Donor donor, User user)
        {
            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();

                try
                {
                    string sqlQuery = "UPDATE donor_test_info SET "
                        + "is_ua = @IsUA, "
                        + "is_hair = @IsHair, "
                        + "is_dna = @IsDNA, "
                         + "is_bc = @IsBC, "
                        + "reason_for_test_id = @ReasonForTestId, "
                        + "other_reason = @OtherReason, "
                        + "is_temperature_in_range = @IsTemperatureInRange, "
                        + "temperature_of_specimen = @TemperatureOfSpecimen, "
                        + "testing_authority_id = @TestingAuthorityId, "
                        + "specimen_collection_cup_id = @SpecimenCollectionCupId, "
                        + "is_observed = @IsObserved, "
                        + "form_type_id = @FormTypeId, "
                        + "is_adulteration_sign = @IsAdulterationSign, "
                        + "is_quantity_sufficient = @IsQuantitySufficient, "
                        + "collection_site_vendor_id = @CollectionSiteVendorId, "
                        + "collection_site_location_id = @CollectionSiteLocationId, "
                        + "collection_site_user_id = @CollectionSiteUserId, "
                         // + "screening_time = {0}, "
                         + "screening_time = @ScreeningTime, "
                        + "is_donor_refused = @IsDonorRefused, "
                        + "collection_site_remarks = @CollectionSiteRemarks, "
                        //    + "collection_site_remarks = @CollectionSiteRemarks, "
                        + "total_payment_amount = @TotalPaymentAmount, "
                        + "vendor_cost = @VendorCost, "
                        + "test_status = @TestStatus, "
                        + "is_instant_test = @IsInstantTest, "
                        + "instant_test_result = @InstantTestResult, "
                        + "test_overall_result = @TestOverallResult, "
                        + "is_reverse_entry =b'0', "
                        + "is_synchronized = b'0', "
                        + "last_modified_on = NOW(), "
                        + "last_modified_by = @LastModifiedBy "
                        + "WHERE donor_test_info_id = @DonorTestInfoId";

                    MySqlParameter[] param = new MySqlParameter[28];

                    param[0] = new MySqlParameter("@DonorTestInfoId", donorTestInfo.DonorTestInfoId);

                    param[1] = new MySqlParameter("@IsUA", donorTestInfo.IsUA);
                    param[2] = new MySqlParameter("@IsHair", donorTestInfo.IsHair);
                    param[3] = new MySqlParameter("@IsDNA", donorTestInfo.IsDNA);

                    param[4] = new MySqlParameter("@ReasonForTestId", donorTestInfo.ReasonForTestId);
                    param[5] = new MySqlParameter("@OtherReason", donorTestInfo.OtherReason);

                    param[6] = new MySqlParameter("@SpecimenCollectionCupId", donorTestInfo.SpecimenCollectionCupId);
                    param[7] = new MySqlParameter("@IsObserved", donorTestInfo.IsObserved);
                    param[8] = new MySqlParameter("@FormTypeId", donorTestInfo.FormTypeId);
                    param[9] = new MySqlParameter("@TestingAuthorityId", donorTestInfo.TestingAuthorityId);

                    param[10] = new MySqlParameter("@IsTemperatureInRange", donorTestInfo.IsTemperatureInRange);
                    param[11] = new MySqlParameter("@TemperatureOfSpecimen", donorTestInfo.TemperatureOfSpecimen);

                    param[12] = new MySqlParameter("@IsAdulterationSign", donorTestInfo.IsAdulterationSign);
                    param[13] = new MySqlParameter("@IsQuantitySufficient", donorTestInfo.IsQuantitySufficient);

                    param[14] = new MySqlParameter("@CollectionSiteVendorId", donorTestInfo.CollectionSiteVendorId);
                    param[15] = new MySqlParameter("@CollectionSiteLocationId", donorTestInfo.CollectionSiteLocationId);
                    param[16] = new MySqlParameter("@CollectionSiteUserId", donorTestInfo.CollectionSiteUserId);

                    if (donorTestInfo.TestStatus == DonorRegistrationStatus.Processing || donorTestInfo.TestStatus == DonorRegistrationStatus.Completed)
                    {
                        // sqlQuery = string.Format(sqlQuery,donorTestInfo.ScreeningTime);
                        param[17] = new MySqlParameter("@ScreeningTime", donorTestInfo.ScreeningTime);
                    }
                    else
                    {
                        // sqlQuery = string.Format(sqlQuery, "NULL");
                        param[17] = new MySqlParameter("@ScreeningTime", "NULL");
                    }

                    param[18] = new MySqlParameter("@IsDonorRefused", donorTestInfo.IsDonorRefused);
                    param[19] = new MySqlParameter("@CollectionSiteRemarks", donorTestInfo.CollectionSiteRemarks);

                    param[20] = new MySqlParameter("@TotalPaymentAmount", donorTestInfo.TotalPaymentAmount);
                    param[21] = new MySqlParameter("@VendorCost", donorTestInfo.VendorCost);

                    param[22] = new MySqlParameter("@TestStatus", (int)donorTestInfo.TestStatus);

                    param[23] = new MySqlParameter("@IsInstantTest", donorTestInfo.IsInstantTest);
                    param[24] = new MySqlParameter("@InstantTestResult", donorTestInfo.InstantTestResult);
                    param[25] = new MySqlParameter("@TestOverallResult", donorTestInfo.TestOverallResult);
                    // param[25] = new MySqlParameter("@IsReverseEntry", false);
                    param[26] = new MySqlParameter("@LastModifiedBy", donorTestInfo.LastModifiedBy);
                    param[27] = new MySqlParameter("@IsBC", donorTestInfo.IsBC);

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                    //Test Category
                    string specimenId = string.Empty;

                    sqlQuery = "UPDATE donor_test_info_test_categories SET "
                                + "specimen_id = @SpecimenId, "
                                + "hair_test_panel_days = @HairTestPanelDays, "
                                + "test_panel_price = @TestPanelPrice, "
                                + "test_panel_status = @TestPanelStatus, "
                                + "is_synchronized = b'0', "
                                + "last_modified_on = NOW(), "
                                + "last_modified_by = @LastModifiedBy "
                                + "WHERE donor_test_test_category_id = @DonorTestTestCategoryId";

                    foreach (DonorTestInfoTestCategories donorTestCategory in donorTestInfo.TestInfoTestCategories)
                    {
                        param = new MySqlParameter[6];

                        param[0] = new MySqlParameter("@DonorTestTestCategoryId", donorTestCategory.DonorTestTestCategoryId);
                        param[1] = new MySqlParameter("@SpecimenId", donorTestCategory.SpecimenId);
                        param[2] = new MySqlParameter("@HairTestPanelDays", donorTestCategory.HairTestPanelDays);
                        param[3] = new MySqlParameter("@TestPanelPrice", donorTestCategory.TestPanelPrice);
                        param[4] = new MySqlParameter("@TestPanelStatus", (int)donorTestInfo.TestStatus);
                        param[5] = new MySqlParameter("@LastModifiedBy", donorTestInfo.LastModifiedBy);

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                        if (donorTestInfo.TestStatus == DonorRegistrationStatus.Processing || (donorTestInfo.IsDonorRefused == false && donorTestInfo.TestStatus == DonorRegistrationStatus.Completed && donorTestInfo.InstantTestResult == InstantTestResult.Negative))
                        {
                            if (donorTestCategory.TestCategoryId == TestCategories.UA || donorTestCategory.TestCategoryId == TestCategories.Hair)
                            {
                                if (donorTestCategory.SpecimenId.Trim() != string.Empty)
                                {
                                    if (specimenId.Trim() == string.Empty)
                                    {
                                        specimenId += donorTestCategory.SpecimenId.Trim();
                                    }
                                    else
                                    {
                                        specimenId += ", " + donorTestCategory.SpecimenId.Trim();
                                    }
                                }
                            }
                        }
                    }

                    //Activity Note
                    sqlQuery = "SELECT user_id FROM users where UPPER(user_name) = UPPER(@DonorEmail) and is_archived=0";

                    param = new MySqlParameter[1];

                    param[0] = new MySqlParameter("@DonorEmail", donorTestInfo.LastModifiedBy);

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

                    string donorActivityNote = string.Empty;

                    if (donorTestInfo.IsTemperatureInRange != YesNo.None)
                    {
                        if (donorTestInfo.IsTemperatureInRange == YesNo.No)
                        {
                            donorActivityNote = "Specimen Collected with the temperature of {0}.";
                        }
                        else
                        {
                            donorActivityNote = "Specimen Collected with the temperature range of acceptence.";
                        }

                        param = new MySqlParameter[5];

                        param[0] = new MySqlParameter("@DonorTestInfoId", donorTestInfo.DonorTestInfoId);
                        param[1] = new MySqlParameter("@ActivityUserId", currentUserId);
                        param[2] = new MySqlParameter("@ActivityCategoryId", (int)DonorActivityCategories.General);
                        param[3] = new MySqlParameter("@IsActivityVisible", true);
                        param[4] = new MySqlParameter("@ActivityNote", string.Format(donorActivityNote, donorTestInfo.TemperatureOfSpecimen.ToString()));

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);
                    }

                    if (!donorTestInfo.IsInstantTest)
                    {
                        if (specimenId != string.Empty)
                        {
                            donorActivityNote = "Test Info updated with the status of {0} and the Specimen ID(s) of {1}.";
                        }
                        else
                        {
                            donorActivityNote = "Test Info updated with the status of {0}.";
                        }

                        param = new MySqlParameter[5];

                        param[0] = new MySqlParameter("@DonorTestInfoId", donorTestInfo.DonorTestInfoId);
                        param[1] = new MySqlParameter("@ActivityUserId", currentUserId);
                        param[2] = new MySqlParameter("@ActivityCategoryId", (int)DonorActivityCategories.General);
                        param[3] = new MySqlParameter("@IsActivityVisible", true);
                        param[4] = new MySqlParameter("@ActivityNote", string.Format(donorActivityNote, donorTestInfo.TestStatus.ToDescriptionString(), specimenId));

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);
                    }
                    else
                    {
                        if (donorTestInfo.IsInstantTest && donorTestInfo.InstantTestResult == InstantTestResult.Positive)
                        {
                            donorActivityNote = "Test Info updated with the status of {0} instant test result as {1} and the Specimen ID(s) as {2}.The donor has to take a regular test.";
                        }
                        else if (donorTestInfo.IsInstantTest && donorTestInfo.InstantTestResult == InstantTestResult.Negative)
                        {
                            donorActivityNote = "Test Info updated with the  status of {0} instant test result as {1} and the Specimen ID(s) as {2}.";
                        }

                        param = new MySqlParameter[5];

                        param[0] = new MySqlParameter("@DonorTestInfoId", donorTestInfo.DonorTestInfoId);
                        param[1] = new MySqlParameter("@ActivityUserId", currentUserId);
                        param[2] = new MySqlParameter("@ActivityCategoryId", (int)DonorActivityCategories.General);
                        param[3] = new MySqlParameter("@IsActivityVisible", true);
                        param[4] = new MySqlParameter("@ActivityNote", string.Format(donorActivityNote, donorTestInfo.TestStatus.ToDescriptionString(), donorTestInfo.InstantTestResult, specimenId));

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);
                    }

                    sqlQuery = "INSERT INTO users (user_name, user_password, is_user_active, user_first_name, user_last_name, user_phone_number, user_fax, user_email, change_password_required, user_type, department_id, donor_id, client_id, vendor_id, attorney_id, court_id, judge_id, is_synchronized, is_archived, created_on, created_by, last_modified_on, last_modified_by) VALUES (";
                    sqlQuery += "@Username, @UserPassword, @IsUserActive, @UserFirstName, @UserLastName, @UserPhoneNumber, @UserFax, @UserEmail, @ChangePasswordRequired, @UserType, @DepartmentId, @DonorId, @ClientId, @VendorId, @AttorneyId, @CourtId, @JudgeId, b'0', b'0', NOW(), @CreatedBy, NOW(), @LastModifiedBy)";

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

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                    //sqlQuery = "SELECT LAST_INSERT_ID()";

                    //user.UserId = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlQuery));

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
        }

        public void SaveVendorInfoDetails(DonorTestInfo donorTestInfo)
        {
            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();

                try
                {
                    string sqlQuery = "UPDATE donor_test_info SET "
                        + "collection_site_1_id  = @CollectionSite1Id, "
                        + "collection_site_2_id  = @CollectionSite2Id, "
                        + "collection_site_3_id  = @CollectionSite3Id, "
                        + "collection_site_4_id  = @CollectionSite4Id, "
                        + "schedule_date = @ScheduleDate, "
                        + "is_synchronized = b'0', "
                        + "last_modified_on = NOW(), "
                        + "last_modified_by = @LastModifiedBy "
                        + "WHERE donor_test_info_id = @DonorTestInfoId";

                    MySqlParameter[] param = new MySqlParameter[7];

                    param[0] = new MySqlParameter("@DonorTestInfoId", donorTestInfo.DonorTestInfoId);
                    param[1] = new MySqlParameter("@CollectionSite1Id", donorTestInfo.CollectionSite1Id);
                    param[2] = new MySqlParameter("@CollectionSite2Id", donorTestInfo.CollectionSite2Id);
                    param[3] = new MySqlParameter("@CollectionSite3Id", donorTestInfo.CollectionSite3Id);
                    param[4] = new MySqlParameter("@CollectionSite4Id", donorTestInfo.CollectionSite4Id);
                    param[5] = new MySqlParameter("@ScheduleDate", donorTestInfo.ScheduleDate);
                    param[6] = new MySqlParameter("@LastModifiedBy", donorTestInfo.LastModifiedBy);

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

        public void SaveLegalInfoDetails(DonorTestInfo donorTestInfo, bool isValidString)
        {
            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();

                try
                {
                    string sqlQuery = "UPDATE donor_test_info SET "
                        + "program_type_id = @ProgramTypeId, "
                        + "is_surscan_determines_dates = @IsSurscanDeterminesDates, "
                        + "is_tp_determines_dates = @IsTpDeterminesSates, "
                        + "program_start_date = @ProgramStartDate, "
                        + "program_end_date = @ProgramEndDate, "
                        + "case_number = @CaseNumber, "
                        + "court_id = @CourtId, "
                        + "judge_id = @JudgeId, "
                        + "special_notes = @SpecialNotes, "
                        + "is_synchronized = b'0', "
                        + "last_modified_on = NOW(), "
                        + "last_modified_by = @LastModifiedBy "
                        + "WHERE donor_test_info_id = @DonorTestInfoId";

                    MySqlParameter[] param = new MySqlParameter[11];

                    param[0] = new MySqlParameter("@DonorTestInfoId", donorTestInfo.DonorTestInfoId);
                    param[1] = new MySqlParameter("@ProgramTypeId", (int)donorTestInfo.ProgramTypeId);
                    param[2] = new MySqlParameter("@IsSurscanDeterminesDates", donorTestInfo.IsSurscanDeterminesDates);
                    param[3] = new MySqlParameter("@IsTpDeterminesSates", donorTestInfo.IsTpDeterminesDates);
                    param[4] = new MySqlParameter("@ProgramStartDate", donorTestInfo.ProgramStartDate);
                    param[5] = new MySqlParameter("@ProgramEndDate", donorTestInfo.ProgramEndDate);
                    param[6] = new MySqlParameter("@CaseNumber", donorTestInfo.CaseNumber);
                    param[7] = new MySqlParameter("@CourtId", donorTestInfo.CourtId);
                    param[8] = new MySqlParameter("@JudgeId", donorTestInfo.JudgeId);
                    param[9] = new MySqlParameter("@SpecialNotes", donorTestInfo.SpecialNotes);
                    param[10] = new MySqlParameter("@LastModifiedBy", donorTestInfo.LastModifiedBy);

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                    //Attorney
                    sqlQuery = "SELECT COUNT(*) FROM donor_test_info_attorneys WHERE donor_test_info_id = @DonorTestInfoId";

                    param = new MySqlParameter[1];

                    param[0] = new MySqlParameter("@DonorTestInfoId", donorTestInfo.DonorTestInfoId);

                    int count = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlQuery, param));

                    if (count == 3)
                    {
                        sqlQuery = "UPDATE donor_test_info_attorneys SET "
                                        + "attorney_id = @AttorneyId, "
                                        + "is_synchronized = b'0', "
                                        + "last_modified_on = NOW(), "
                                        + "last_modified_by = @LastModifiedBy "
                                        + "WHERE donor_test_info_id = @DonorTestInfoId AND display_order = @DisplayOrder";
                    }
                    else
                    {
                        sqlQuery = "INSERT INTO donor_test_info_attorneys ("
                                        + "donor_test_info_id, "
                                        + "display_order, "
                                        + "attorney_id, "
                                        + "is_synchronized, "
                                        + "created_on, "
                                        + "created_by, "
                                        + "last_modified_on, "
                                        + "last_modified_by) "
                                        + "VALUES ( "
                                        + "@DonorTestInfoId, "
                                        + "@DisplayOrder, "
                                        + "@AttorneyId, "
                                        + "b'0', "
                                        + "NOW(), "
                                        + "@CreatedBy, "
                                        + "NOW(), "
                                        + "@LastModifiedBy)";
                    }

                    //Attorney 1
                    param = new MySqlParameter[5];

                    param[0] = new MySqlParameter("@DonorTestInfoId", donorTestInfo.DonorTestInfoId);
                    param[1] = new MySqlParameter("@DisplayOrder", 1);
                    param[2] = new MySqlParameter("@AttorneyId", donorTestInfo.AttorneyId1);
                    param[3] = new MySqlParameter("@CreatedBy", donorTestInfo.LastModifiedBy);
                    param[4] = new MySqlParameter("@LastModifiedBy", donorTestInfo.LastModifiedBy);

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                    //Attorney 2
                    param = new MySqlParameter[5];

                    param[0] = new MySqlParameter("@DonorTestInfoId", donorTestInfo.DonorTestInfoId);
                    param[1] = new MySqlParameter("@DisplayOrder", 2);
                    param[2] = new MySqlParameter("@AttorneyId", donorTestInfo.AttorneyId2);
                    param[3] = new MySqlParameter("@CreatedBy", donorTestInfo.LastModifiedBy);
                    param[4] = new MySqlParameter("@LastModifiedBy", donorTestInfo.LastModifiedBy);

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                    //Attorney 3
                    param = new MySqlParameter[5];

                    param[0] = new MySqlParameter("@DonorTestInfoId", donorTestInfo.DonorTestInfoId);
                    param[1] = new MySqlParameter("@DisplayOrder", 3);
                    param[2] = new MySqlParameter("@AttorneyId", donorTestInfo.AttorneyId3);
                    param[3] = new MySqlParameter("@CreatedBy", donorTestInfo.LastModifiedBy);
                    param[4] = new MySqlParameter("@LastModifiedBy", donorTestInfo.LastModifiedBy);

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                    //Third Party
                    sqlQuery = "SELECT COUNT(*) FROM donor_test_info_third_parties WHERE donor_test_info_id = @DonorTestInfoId";

                    param = new MySqlParameter[1];

                    param[0] = new MySqlParameter("@DonorTestInfoId", donorTestInfo.DonorTestInfoId);

                    count = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlQuery, param));

                    if (count == 3)
                    {
                        sqlQuery = "UPDATE donor_test_info_third_parties SET "
                                        + "third_party_id = @ThirdPartyId, "
                                        + "is_synchronized = b'0', "
                                        + "last_modified_on = NOW(), "
                                        + "last_modified_by = @LastModifiedBy "
                                        + "WHERE donor_test_info_id = @DonorTestInfoId AND display_order = @DisplayOrder";
                    }
                    else
                    {
                        sqlQuery = "INSERT INTO donor_test_info_third_parties ("
                                        + "donor_test_info_id, "
                                        + "display_order, "
                                        + "third_party_id, "
                                        + "is_synchronized, "
                                        + "created_on, "
                                        + "created_by, "
                                        + "last_modified_on, "
                                        + "last_modified_by) "
                                        + "VALUES ( "
                                        + "@DonorTestInfoId, "
                                        + "@DisplayOrder, "
                                        + "@ThirdPartyId, "
                                        + "b'0', "
                                        + "NOW(), "
                                        + "@CreatedBy, "
                                        + "NOW(), "
                                        + "@LastModifiedBy)";
                    }

                    //Third Party 1
                    param = new MySqlParameter[5];

                    param[0] = new MySqlParameter("@DonorTestInfoId", donorTestInfo.DonorTestInfoId);
                    param[1] = new MySqlParameter("@DisplayOrder", 1);
                    param[2] = new MySqlParameter("@ThirdPartyId", donorTestInfo.ThirdPartyInfoId1);
                    param[3] = new MySqlParameter("@CreatedBy", donorTestInfo.LastModifiedBy);
                    param[4] = new MySqlParameter("@LastModifiedBy", donorTestInfo.LastModifiedBy);

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                    //Third Party 1
                    param = new MySqlParameter[5];

                    param[0] = new MySqlParameter("@DonorTestInfoId", donorTestInfo.DonorTestInfoId);
                    param[1] = new MySqlParameter("@DisplayOrder", 2);
                    param[2] = new MySqlParameter("@ThirdPartyId", donorTestInfo.ThirdPartyInfoId2);
                    param[3] = new MySqlParameter("@CreatedBy", donorTestInfo.LastModifiedBy);
                    param[4] = new MySqlParameter("@LastModifiedBy", donorTestInfo.LastModifiedBy);

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                    //sqlQuery = "SELECT user_id FROM users where UPPER(user_name) = UPPER(@DonorEmail)";

                    //param = new MySqlParameter[1];

                    //param[0] = new MySqlParameter("@DonorEmail", donorTestInfo.LastModifiedBy);

                    //int currentUserId = (int)SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlQuery, param);

                    //sqlQuery = "INSERT INTO donor_test_activity ("
                    //                + "donor_test_info_id, "
                    //                + "activity_datetime, "
                    //                + "activity_user_id, "
                    //                + "activity_category_id, "
                    //                + "is_activity_visible, "
                    //                + "activity_note, "
                    //                + "is_synchronized) VALUES ("
                    //                + "@DonorTestInfoId, "
                    //                + "NOW(), "
                    //                + "@ActivityUserId, "
                    //                + "@ActivityCategoryId, "
                    //                + "@IsActivityVisible, "
                    //                + "@ActivityNote, "
                    //                + "b'0')";

                    //param = new MySqlParameter[5];

                    //param[0] = new MySqlParameter("@DonorTestInfoId", donorTestInfo.DonorTestInfoId);
                    //param[1] = new MySqlParameter("@ActivityUserId", currentUserId);
                    //param[2] = new MySqlParameter("@ActivityCategoryId", (int)DonorActivityCategories.General);
                    //param[3] = new MySqlParameter("@IsActivityVisible", true);
                    //param[4] = new MySqlParameter("@ActivityNote", string.Format("Payment made...$ {0}.", donorTestInfo.TotalPaymentAmount.ToString()));

                    //SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                    if (donorTestInfo.SpecialNotes.Trim() != string.Empty && isValidString == true)
                    {
                        //string SpecialNotes1 = donorTestInfo.SpecialNotes;
                        //if (SpecialNotes != SpecialNotes1)
                        // {
                        //Activity Note
                        sqlQuery = "SELECT user_id FROM users where UPPER(user_name) = UPPER(@DonorEmail) and is_archived=0";

                        param = new MySqlParameter[1];

                        param[0] = new MySqlParameter("@DonorEmail", donorTestInfo.LastModifiedBy);

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
                        string legalActivityNote = string.Empty;
                        DonorActivityNote donorActivityNote = new DonorActivityNote();

                        param[0] = new MySqlParameter("@DonorTestInfoId", donorTestInfo.DonorTestInfoId);
                        param[1] = new MySqlParameter("@ActivityUserId", currentUserId);
                        param[2] = new MySqlParameter("@ActivityCategoryId", (int)DonorActivityCategories.General);
                        param[3] = new MySqlParameter("@IsActivityVisible", true);
                        param[4] = new MySqlParameter("@ActivityNote", string.Format("Legal Info Special Notes:" + donorTestInfo.SpecialNotes + "."));

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);
                    }
                    // }

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
        }
    }
}