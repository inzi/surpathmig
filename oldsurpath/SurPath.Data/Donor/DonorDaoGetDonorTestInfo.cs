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
        public DonorTestInfo GetDonorTestInfoByDonorIdDonorTestInfoId(int donorId, int donorTestInfoId)
        {
            DonorTestInfo donorTestInfo = null;

            string sqlQuery = "SELECT "
                        + "donor_test_info_id AS DonorTestInfoId, "
                        + "donor_id AS DonorId, "
                        + "client_id AS ClientId, "
                        + "client_department_id AS ClientDepartmentId, "
                        + "mro_type_id AS MROTypeId, "
                        + "payment_type_id AS PaymentTypeId, "
                        + "test_requested_date AS TestRequestedDate, "
                        + "test_requested_by AS TestRequestedBy, "
                        + "is_ua AS IsUA, "
                        + "is_hair AS IsHair, "
                        + "is_dna AS IsDNA, "
                        + "is_bc AS IsBC, "
                        + "reason_for_test_id AS ReasonForTestId, "
                        + "other_reason AS OtherReason, "
                        + "is_temperature_in_range AS IsTemperatureInRange, "
                        + "temperature_of_specimen AS TemperatureOfSpecimen, "
                        + "testing_authority_id AS TestingAuthorityId, "
                        + "specimen_collection_cup_id AS SpecimenCollectionCupId, "
                        + "is_observed AS IsObserved, "
                        + "form_type_id AS FormTypeId, "
                        + "is_adulteration_sign AS IsAdulterationSign, "
                        + "is_quantity_sufficient AS IsQuantitySufficient, "
                        + "collection_site_vendor_id AS CollectionSiteVendorId, "
                        + "collection_site_location_id AS CollectionSiteLocationId, "
                        + "collection_site_user_id AS CollectionSiteUserId, "
                        + "screening_time AS ScreeningTime, "
                        + "is_donor_refused AS IsDonorRefused, "
                        + "collection_site_remarks AS CollectionSiteRemarks, "
                        + "laboratory_vendor_id AS LaboratoryVendorId, "
                        + "mro_vendor_id AS MROVendorId, "
                        + "test_overall_result AS TestOverallResult, "
                        + "test_status AS TestStatus, "
                        + "program_type_id AS ProgramTypeId, "
                        + "is_surscan_determines_dates AS IsSurscanDeterminesDates, "
                        + "is_tp_determines_dates AS IsTpDeterminesDates, "
                        + "program_start_date AS ProgramStartDate, "
                        + "program_end_date AS ProgramEndDate, "
                        + "case_number AS CaseNumber, "
                        + "court_id AS CourtId, "
                        + "judge_id AS JudgeId, "
                        + "special_notes AS SpecialNotes, "
                        + "total_payment_amount AS TotalPaymentAmount, "
                        + "payment_date AS PaymentDate, "
                        + "payment_method_id AS PaymentMethodId, "
                        + "payment_note AS PaymentNote, "
                        + "payment_status AS PaymentStatus, "
                        + "laboratory_cost AS LaboratoryCost, "
                        + "mro_cost AS MROCost, "
                        + "cup_cost AS CupCost, "
                        + "shipping_cost AS ShippingCost, "
                        + "vendor_cost AS VendorCost, "
                        + "collection_site_1_id AS CollectionSite1Id, "
                        + "collection_site_2_id AS  CollectionSite2Id, "
                        + "collection_site_3_id AS CollectionSite3Id, "
                        + "collection_site_4_id AS CollectionSite4Id, "
                        + "schedule_date AS ScheduleDate, "
                        + "is_walkin_donor AS IsWalkinDonor, "
                        + "is_instant_test AS IsInstantTest, "
                        + "instant_test_result AS InstantTestResult, "
                        + "is_reverse_entry AS IsReverseEntry, "
                        + "is_synchronized AS IsSynchronized, "
                        + "created_on AS CreatedOn, "
                        + "created_by AS CreatedBy, "
                        + "last_modified_on AS LastModifiedOn, "
                        + "last_modified_by AS LastModifiedBy "
                        + "FROM donor_test_info "
                        + "WHERE donor_test_info_id = @DonorTestInfoId and donor_id = @DonorId";

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                MySqlParameter[] param = new MySqlParameter[2];
                param[0] = new MySqlParameter("@DonorId", donorId);
                param[1] = new MySqlParameter("@DonorTestInfoId", donorTestInfoId);

                MySqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                if (dr.Read())
                {
                    donorTestInfo = new DonorTestInfo();

                    donorTestInfo.DonorTestInfoId = (int)dr["DonorTestInfoId"];
                    donorTestInfo.DonorId = (int)dr["DonorId"];
                    donorTestInfo.ClientId = (int)dr["ClientId"];
                    donorTestInfo.ClientDepartmentId = (int)dr["ClientDepartmentId"];
                    donorTestInfo.MROTypeId = dr["MROTypeId"] != DBNull.Value ? (ClientMROTypes)(Convert.ToInt32(dr["MROTypeId"].ToString())) : ClientMROTypes.None;
                    donorTestInfo.PaymentTypeId = dr["PaymentTypeId"] != DBNull.Value ? (ClientPaymentTypes)(Convert.ToInt32(dr["PaymentTypeId"].ToString())) : ClientPaymentTypes.None;
                    donorTestInfo.TestRequestedDate = dr["TestRequestedDate"] != DBNull.Value ? Convert.ToDateTime(dr["TestRequestedDate"].ToString()) : DateTime.MinValue;
                    donorTestInfo.TestRequestedBy = (int)dr["TestRequestedBy"];

                    if (dr["IsUA"] != DBNull.Value)
                    {
                        donorTestInfo.IsUA = dr["IsUA"].ToString() == "1" ? true : false;
                    }

                    if (dr["IsHair"] != DBNull.Value)
                    {
                        donorTestInfo.IsHair = dr["IsHair"].ToString() == "1" ? true : false; ;
                    }

                    if (dr["IsDNA"] != DBNull.Value)
                    {
                        donorTestInfo.IsDNA = dr["IsDNA"].ToString() == "1" ? true : false; ;
                    }
                    if (dr["IsBC"] != DBNull.Value)
                    {
                        donorTestInfo.IsBC = dr["IsBC"].ToString() == "1" ? true : false; ;
                    }

                    donorTestInfo.ReasonForTestId = dr["ReasonForTestId"] != DBNull.Value ? (TestInfoReasonForTest)(Convert.ToInt32(dr["ReasonForTestId"].ToString())) : TestInfoReasonForTest.None;
                    donorTestInfo.OtherReason = dr["OtherReason"] != DBNull.Value ? dr["OtherReason"].ToString() : null;
                    donorTestInfo.IsTemperatureInRange = dr["IsTemperatureInRange"] != DBNull.Value ? (YesNo)(Convert.ToInt32(dr["IsTemperatureInRange"].ToString())) : YesNo.None;

                    if (dr["TemperatureOfSpecimen"] != DBNull.Value)
                    {
                        donorTestInfo.TemperatureOfSpecimen = Convert.ToDouble(dr["TemperatureOfSpecimen"]);
                    }

                    if (dr["TestingAuthorityId"] != DBNull.Value)
                    {
                        donorTestInfo.TestingAuthorityId = Convert.ToInt32(dr["TestingAuthorityId"]);
                    }

                    donorTestInfo.SpecimenCollectionCupId = dr["SpecimenCollectionCupId"] != DBNull.Value ? (SpecimenCollectionCupType)(Convert.ToInt32(dr["SpecimenCollectionCupId"].ToString())) : SpecimenCollectionCupType.None;
                    donorTestInfo.IsObserved = dr["IsObserved"] != DBNull.Value ? (YesNo)(Convert.ToInt32(dr["IsObserved"].ToString())) : YesNo.None;
                    donorTestInfo.FormTypeId = dr["FormTypeId"] != DBNull.Value ? (SpecimenFormType)(Convert.ToInt32(dr["FormTypeId"].ToString())) : SpecimenFormType.None;
                    donorTestInfo.IsAdulterationSign = dr["IsAdulterationSign"] != DBNull.Value ? (YesNo)(Convert.ToInt32(dr["IsAdulterationSign"].ToString())) : YesNo.None;
                    donorTestInfo.IsQuantitySufficient = dr["IsQuantitySufficient"] != DBNull.Value ? (YesNo)(Convert.ToInt32(dr["IsQuantitySufficient"].ToString())) : YesNo.None;

                    if (dr["CollectionSiteVendorId"] != DBNull.Value)
                    {
                        donorTestInfo.CollectionSiteVendorId = Convert.ToInt32(dr["CollectionSiteVendorId"]);
                    }

                    if (dr["CollectionSiteLocationId"] != DBNull.Value)
                    {
                        donorTestInfo.CollectionSiteLocationId = Convert.ToInt32(dr["CollectionSiteLocationId"]);
                    }

                    if (dr["CollectionSiteUserId"] != DBNull.Value)
                    {
                        donorTestInfo.CollectionSiteUserId = Convert.ToInt32(dr["CollectionSiteUserId"]);
                    }

                    if (dr["ScreeningTime"] != DBNull.Value)
                    {
                        donorTestInfo.ScreeningTime = Convert.ToDateTime(dr["ScreeningTime"]);
                    }

                    if (dr["IsDonorRefused"] != DBNull.Value)
                    {
                        donorTestInfo.IsDonorRefused = Convert.ToBoolean(dr["IsDonorRefused"]);
                    }

                    donorTestInfo.CollectionSiteRemarks = dr["CollectionSiteRemarks"] != DBNull.Value ? dr["CollectionSiteRemarks"].ToString() : null;

                    if (dr["LaboratoryVendorId"] != DBNull.Value)
                    {
                        donorTestInfo.LaboratoryVendorId = Convert.ToInt32(dr["LaboratoryVendorId"]);
                    }

                    if (dr["MROVendorId"] != DBNull.Value)
                    {
                        donorTestInfo.MROVendorId = Convert.ToInt32(dr["MROVendorId"]);
                    }

                    if (dr["TestOverallResult"] != DBNull.Value)
                    {
                        donorTestInfo.TestOverallResult = Convert.ToInt32(dr["TestOverallResult"]);
                    }

                    donorTestInfo.TestStatus = dr["TestStatus"] != DBNull.Value ? (DonorRegistrationStatus)(Convert.ToInt32(dr["TestStatus"].ToString())) : DonorRegistrationStatus.None;
                    donorTestInfo.ProgramTypeId = dr["ProgramTypeId"] != DBNull.Value ? (ProgramType)(Convert.ToInt32(dr["ProgramTypeId"].ToString())) : ProgramType.None;

                    if (dr["IsSurscanDeterminesDates"] != DBNull.Value)
                    {
                        donorTestInfo.IsSurscanDeterminesDates = Convert.ToBoolean(dr["IsSurscanDeterminesDates"]);
                    }

                    if (dr["IsTpDeterminesDates"] != DBNull.Value)
                    {
                        donorTestInfo.IsTpDeterminesDates = Convert.ToBoolean(dr["IsTpDeterminesDates"]);
                    }

                    if (dr["ProgramStartDate"] != DBNull.Value)
                    {
                        donorTestInfo.ProgramStartDate = Convert.ToDateTime(dr["ProgramStartDate"]);
                    }

                    if (dr["ProgramEndDate"] != DBNull.Value)
                    {
                        donorTestInfo.ProgramEndDate = Convert.ToDateTime(dr["ProgramEndDate"]);
                    }

                    donorTestInfo.CaseNumber = dr["CaseNumber"] != DBNull.Value ? dr["CaseNumber"].ToString() : null;

                    if (dr["CourtId"] != DBNull.Value)
                    {
                        donorTestInfo.CourtId = Convert.ToInt32(dr["CourtId"]);
                    }

                    if (dr["JudgeId"] != DBNull.Value)
                    {
                        donorTestInfo.JudgeId = Convert.ToInt32(dr["JudgeId"]);
                    }

                    donorTestInfo.SpecialNotes = dr["SpecialNotes"] != DBNull.Value ? dr["SpecialNotes"].ToString() : null;

                    if (dr["TotalPaymentAmount"] != DBNull.Value)
                    {
                        donorTestInfo.TotalPaymentAmount = Convert.ToDouble(dr["TotalPaymentAmount"]);
                    }

                    if (dr["PaymentDate"] != DBNull.Value)
                    {
                        donorTestInfo.PaymentDate = Convert.ToDateTime(dr["PaymentDate"]);
                    }

                    donorTestInfo.PaymentMethodId = dr["PaymentMethodId"] != DBNull.Value ? (PaymentMethod)(Convert.ToInt32(dr["PaymentMethodId"].ToString())) : PaymentMethod.None;
                    donorTestInfo.PaymentNote = dr["PaymentNote"] != DBNull.Value ? dr["PaymentNote"].ToString() : null;
                    donorTestInfo.PaymentStatus = dr["PaymentStatus"] != DBNull.Value ? (PaymentStatus)(Convert.ToInt32(dr["PaymentStatus"].ToString())) : PaymentStatus.None;

                    if (dr["LaboratoryCost"] != DBNull.Value)
                    {
                        donorTestInfo.LaboratoryCost = Convert.ToDouble(dr["LaboratoryCost"]);
                    }

                    if (dr["MROCost"] != DBNull.Value)
                    {
                        donorTestInfo.MROCost = Convert.ToDouble(dr["MROCost"]);
                    }

                    if (dr["CupCost"] != DBNull.Value)
                    {
                        donorTestInfo.CupCost = Convert.ToDouble(dr["CupCost"]);
                    }

                    if (dr["ShippingCost"] != DBNull.Value)
                    {
                        donorTestInfo.ShippingCost = Convert.ToDouble(dr["ShippingCost"]);
                    }

                    if (dr["VendorCost"] != DBNull.Value)
                    {
                        donorTestInfo.VendorCost = Convert.ToDouble(dr["VendorCost"]);
                    }
                    if (dr["CollectionSite1Id"] != DBNull.Value)
                    {
                        donorTestInfo.CollectionSite1Id = Convert.ToInt32(dr["CollectionSite1Id"]);
                    }
                    if (dr["CollectionSite2Id"] != DBNull.Value)
                    {
                        donorTestInfo.CollectionSite2Id = Convert.ToInt32(dr["CollectionSite2Id"]);
                    }
                    if (dr["CollectionSite3Id"] != DBNull.Value)
                    {
                        donorTestInfo.CollectionSite3Id = Convert.ToInt32(dr["CollectionSite3Id"]);
                    }
                    if (dr["CollectionSite4Id"] != DBNull.Value)
                    {
                        donorTestInfo.CollectionSite4Id = Convert.ToInt32(dr["CollectionSite4Id"]);
                    }
                    if (dr["ScheduleDate"] != DBNull.Value)
                    {
                        donorTestInfo.ScheduleDate = Convert.ToDateTime(dr["ScheduleDate"]);
                    }

                    if (dr["IsWalkinDonor"] != DBNull.Value)
                    {
                        donorTestInfo.IsWalkinDonor = dr["IsWalkinDonor"].ToString() == "1" ? true : false;
                    }
                    if (dr["IsInstantTest"] != DBNull.Value)
                    {
                        donorTestInfo.IsInstantTest = dr["IsInstantTest"].ToString() == "1" ? true : false;
                    }

                    donorTestInfo.InstantTestResult = dr["InstantTestResult"] != DBNull.Value ? (InstantTestResult)(Convert.ToInt32(dr["InstantTestResult"].ToString())) : InstantTestResult.None;

                    donorTestInfo.IsReverseEntry = dr["IsReverseEntry"].ToString() == "1" ? true : false;

                    //if (dr["IsRegularAfterInstantTest"] != DBNull.Value)
                    //{
                    //    donorTestInfo.IsRegularAfterInstantTest = dr["IsRegularAfterInstantTest"].ToString() == "1" ? true : false; ;
                    //}
                    donorTestInfo.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    donorTestInfo.CreatedOn = (DateTime)dr["CreatedOn"];
                    donorTestInfo.CreatedBy = (string)dr["CreatedBy"];
                    donorTestInfo.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    donorTestInfo.LastModifiedBy = (string)dr["LastModifiedBy"];
                }
            }

            return donorTestInfo;
        }

        public DonorTestInfo GetDonorTestInfo(int donorTestInfoId)
        {
            DonorTestInfo donorTestInfo = null;

            string sqlQuery = "SELECT "
                                + "donor_test_info_id AS DonorTestInfoId, "
                                + "donor_id AS DonorId, "
                                + "client_id AS ClientId, "
                                + "client_department_id AS ClientDepartmentId, "
                                + "mro_type_id AS MROTypeId, "
                                + "payment_type_id AS PaymentTypeId, "
                                + "test_requested_date AS TestRequestedDate, "
                                + "test_requested_by AS TestRequestedBy, "
                                + "is_ua AS IsUA, "
                                + "is_hair AS IsHair, "
                                + "is_dna AS IsDNA, "
                                + "is_bc AS IsBC, "
                                + "reason_for_test_id AS ReasonForTestId, "
                                + "other_reason AS OtherReason, "
                                + "is_temperature_in_range AS IsTemperatureInRange, "
                                + "temperature_of_specimen AS TemperatureOfSpecimen, "
                                + "donor_test_info.testing_authority_id AS TestingAuthorityId, "
                                + "specimen_collection_cup_id AS SpecimenCollectionCupId, "
                                + "is_observed AS IsObserved, "
                                + "form_type_id AS FormTypeId, "
                                + "is_adulteration_sign AS IsAdulterationSign, "
                                + "is_quantity_sufficient AS IsQuantitySufficient, "
                                + "collection_site_vendor_id AS CollectionSiteVendorId, "
                                + "collection_site_location_id AS CollectionSiteLocationId, "
                                + "collection_site_user_id AS CollectionSiteUserId, "
                                + "screening_time AS ScreeningTime, "
                                + "is_donor_refused AS IsDonorRefused, "
                                + "collection_site_remarks AS CollectionSiteRemarks, "
                                + "laboratory_vendor_id AS LaboratoryVendorId, "
                                + "mro_vendor_id AS MROVendorId, "
                                + "test_overall_result AS TestOverallResult, "
                                + "test_status AS TestStatus, "
                                + "program_type_id AS ProgramTypeId, "
                                + "is_surscan_determines_dates AS IsSurscanDeterminesDates, "
                                + "is_tp_determines_dates AS IsTpDeterminesDates, "
                                + "program_start_date AS ProgramStartDate, "
                                + "program_end_date AS ProgramEndDate, "
                                + "case_number AS CaseNumber, "
                                + "court_id AS CourtId, "
                                + "judge_id AS JudgeId, "
                                + "special_notes AS SpecialNotes, "
                                + "total_payment_amount AS TotalPaymentAmount, "
                                + "payment_date AS PaymentDate, "
                                + "payment_method_id AS PaymentMethodId, "
                                + "payment_note AS PaymentNote, "
                                + "payment_status AS PaymentStatus, "
                                + "laboratory_cost AS LaboratoryCost, "
                                + "mro_cost AS MROCost, "
                                + "cup_cost AS CupCost, "
                                + "shipping_cost AS ShippingCost, "
                                + "vendor_cost AS VendorCost, "
                                + "collection_site_1_id AS CollectionSite1Id, "
                                + "collection_site_2_id AS  CollectionSite2Id, "
                                + "collection_site_3_id AS CollectionSite3Id, "
                                + "collection_site_4_id AS CollectionSite4Id, "
                                + "schedule_date AS ScheduleDate, "
                                + "is_walkin_donor AS IsWalkinDonor, "
                                + "is_instant_test AS IsInstantTest, "
                                + "payment_received AS IsPaymentReceived, "
                                + "instant_test_result AS InstantTestResult, "
                                + "is_reverse_entry AS IsReverseEntry,"
                                + "donor_test_info.is_synchronized AS IsSynchronized, "
                                + "donor_test_info.created_on AS CreatedOn, "
                                + "donor_test_info.created_by AS CreatedBy, "
                                + "donor_test_info.last_modified_on AS LastModifiedOn, "
                                + "donor_test_info.last_modified_by AS LastModifiedBy, "
                                + "testing_authority.testing_authority_name AS TestingAuthorityName "
                                + "FROM donor_test_info "
                                + " left outer join testing_authority on donor_test_info.testing_authority_id = testing_authority.testing_authority_id "
                                + "WHERE donor_test_info_id = @DonorTestInfoId";

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                MySqlParameter[] param = new MySqlParameter[1];
                param[0] = new MySqlParameter("@DonorTestInfoId", donorTestInfoId);

                MySqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                if (dr.Read())
                {
                    donorTestInfo = new DonorTestInfo();

                    donorTestInfo.DonorTestInfoId = (int)dr["DonorTestInfoId"];
                    donorTestInfo.DonorId = (int)dr["DonorId"];
                    donorTestInfo.ClientId = (int)dr["ClientId"];
                    donorTestInfo.ClientDepartmentId = (int)dr["ClientDepartmentId"];
                    donorTestInfo.MROTypeId = dr["MROTypeId"] != DBNull.Value ? (ClientMROTypes)(Convert.ToInt32(dr["MROTypeId"].ToString())) : ClientMROTypes.None;
                    donorTestInfo.PaymentTypeId = dr["PaymentTypeId"] != DBNull.Value ? (ClientPaymentTypes)(Convert.ToInt32(dr["PaymentTypeId"].ToString())) : ClientPaymentTypes.None;
                    donorTestInfo.TestRequestedDate = dr["TestRequestedDate"] != DBNull.Value ? Convert.ToDateTime(dr["TestRequestedDate"].ToString()) : DateTime.MinValue;
                    donorTestInfo.TestRequestedBy = (int)dr["TestRequestedBy"];

                    if (dr["IsUA"] != DBNull.Value)
                    {
                        donorTestInfo.IsUA = dr["IsUA"].ToString() == "1" ? true : false;
                    }

                    if (dr["IsHair"] != DBNull.Value)
                    {
                        donorTestInfo.IsHair = dr["IsHair"].ToString() == "1" ? true : false; ;
                    }

                    if (dr["IsDNA"] != DBNull.Value)
                    {
                        donorTestInfo.IsDNA = dr["IsDNA"].ToString() == "1" ? true : false; ;
                    }
                    if (dr["IsBC"] != DBNull.Value)
                    {
                        donorTestInfo.IsBC = dr["IsBC"].ToString() == "1" ? true : false; ;
                    }

                    donorTestInfo.ReasonForTestId = dr["ReasonForTestId"] != DBNull.Value ? (TestInfoReasonForTest)(Convert.ToInt32(dr["ReasonForTestId"].ToString())) : TestInfoReasonForTest.None;
                    donorTestInfo.OtherReason = dr["OtherReason"] != DBNull.Value ? dr["OtherReason"].ToString() : null;
                    donorTestInfo.IsTemperatureInRange = dr["IsTemperatureInRange"] != DBNull.Value ? (YesNo)(Convert.ToInt32(dr["IsTemperatureInRange"].ToString())) : YesNo.None;

                    if (dr["TemperatureOfSpecimen"] != DBNull.Value)
                    {
                        donorTestInfo.TemperatureOfSpecimen = Convert.ToDouble(dr["TemperatureOfSpecimen"]);
                    }

                    if (dr["TestingAuthorityId"] != DBNull.Value)
                    {
                        donorTestInfo.TestingAuthorityId = Convert.ToInt32(dr["TestingAuthorityId"]);
                        donorTestInfo.TestingAuthorityName = dr["TestingAuthorityName"].ToString();
                    }

                    donorTestInfo.SpecimenCollectionCupId = dr["SpecimenCollectionCupId"] != DBNull.Value ? (SpecimenCollectionCupType)(Convert.ToInt32(dr["SpecimenCollectionCupId"].ToString())) : SpecimenCollectionCupType.None;
                    donorTestInfo.IsObserved = dr["IsObserved"] != DBNull.Value ? (YesNo)(Convert.ToInt32(dr["IsObserved"].ToString())) : YesNo.None;
                    donorTestInfo.FormTypeId = dr["FormTypeId"] != DBNull.Value ? (SpecimenFormType)(Convert.ToInt32(dr["FormTypeId"].ToString())) : SpecimenFormType.None;
                    donorTestInfo.IsAdulterationSign = dr["IsAdulterationSign"] != DBNull.Value ? (YesNo)(Convert.ToInt32(dr["IsAdulterationSign"].ToString())) : YesNo.None;
                    donorTestInfo.IsQuantitySufficient = dr["IsQuantitySufficient"] != DBNull.Value ? (YesNo)(Convert.ToInt32(dr["IsQuantitySufficient"].ToString())) : YesNo.None;

                    if (dr["CollectionSiteVendorId"] != DBNull.Value)
                    {
                        donorTestInfo.CollectionSiteVendorId = Convert.ToInt32(dr["CollectionSiteVendorId"]);
                    }

                    if (dr["CollectionSiteLocationId"] != DBNull.Value)
                    {
                        donorTestInfo.CollectionSiteLocationId = Convert.ToInt32(dr["CollectionSiteLocationId"]);
                    }

                    if (dr["CollectionSiteUserId"] != DBNull.Value)
                    {
                        donorTestInfo.CollectionSiteUserId = Convert.ToInt32(dr["CollectionSiteUserId"]);
                    }

                    if (dr["ScreeningTime"] != DBNull.Value)
                    {
                        donorTestInfo.ScreeningTime = Convert.ToDateTime(dr["ScreeningTime"]);
                    }

                    if (dr["IsDonorRefused"] != DBNull.Value)
                    {
                        donorTestInfo.IsDonorRefused = Convert.ToBoolean(dr["IsDonorRefused"]);
                    }

                    donorTestInfo.CollectionSiteRemarks = dr["CollectionSiteRemarks"] != DBNull.Value ? dr["CollectionSiteRemarks"].ToString() : null;

                    if (dr["LaboratoryVendorId"] != DBNull.Value)
                    {
                        donorTestInfo.LaboratoryVendorId = Convert.ToInt32(dr["LaboratoryVendorId"]);
                    }

                    if (dr["MROVendorId"] != DBNull.Value)
                    {
                        donorTestInfo.MROVendorId = Convert.ToInt32(dr["MROVendorId"]);
                    }

                    if (dr["TestOverallResult"] != DBNull.Value)
                    {
                        donorTestInfo.TestOverallResult = Convert.ToInt32(dr["TestOverallResult"]);
                    }

                    _logger.Debug($"donorTestInfo.TestStatus = {donorTestInfo.TestStatus}");
                    _logger.Debug($"Database did not return null for dr[\"TestStatus\"] {dr["TestStatus"] != DBNull.Value} -> value: {Convert.ToInt32(dr["TestStatus"].ToString())}");
                    if (dr["TestStatus"] != DBNull.Value) {
                        _logger.Debug($"Will set test status to: {(DonorRegistrationStatus)(Convert.ToInt32(dr["TestStatus"].ToString()))}");
                    } 
                    else
                    {
                        _logger.Debug($"Will set test status to none, value was null: {DonorRegistrationStatus.None.ToDescriptionString()}");
                    }
                    donorTestInfo.TestStatus = dr["TestStatus"] != DBNull.Value ? (DonorRegistrationStatus)(Convert.ToInt32(dr["TestStatus"].ToString())) : DonorRegistrationStatus.None;
                    donorTestInfo.ProgramTypeId = dr["ProgramTypeId"] != DBNull.Value ? (ProgramType)(Convert.ToInt32(dr["ProgramTypeId"].ToString())) : ProgramType.None;

                    if (dr["IsSurscanDeterminesDates"] != DBNull.Value)
                    {
                        donorTestInfo.IsSurscanDeterminesDates = Convert.ToBoolean(dr["IsSurscanDeterminesDates"]);
                    }

                    if (dr["IsTpDeterminesDates"] != DBNull.Value)
                    {
                        donorTestInfo.IsTpDeterminesDates = Convert.ToBoolean(dr["IsTpDeterminesDates"]);
                    }

                    if (dr["ProgramStartDate"] != DBNull.Value)
                    {
                        donorTestInfo.ProgramStartDate = Convert.ToDateTime(dr["ProgramStartDate"]);
                    }

                    if (dr["ProgramEndDate"] != DBNull.Value)
                    {
                        donorTestInfo.ProgramEndDate = Convert.ToDateTime(dr["ProgramEndDate"]);
                    }

                    donorTestInfo.CaseNumber = dr["CaseNumber"] != DBNull.Value ? dr["CaseNumber"].ToString() : null;

                    if (dr["CourtId"] != DBNull.Value)
                    {
                        donorTestInfo.CourtId = Convert.ToInt32(dr["CourtId"]);
                    }

                    if (dr["JudgeId"] != DBNull.Value)
                    {
                        donorTestInfo.JudgeId = Convert.ToInt32(dr["JudgeId"]);
                    }

                    donorTestInfo.SpecialNotes = dr["SpecialNotes"] != DBNull.Value ? dr["SpecialNotes"].ToString() : null;

                    if (dr["TotalPaymentAmount"] != DBNull.Value)
                    {
                        donorTestInfo.TotalPaymentAmount = Convert.ToDouble(dr["TotalPaymentAmount"]);
                    }

                    if (dr["PaymentDate"] != DBNull.Value)
                    {
                        donorTestInfo.PaymentDate = Convert.ToDateTime(dr["PaymentDate"]);
                    }

                    donorTestInfo.PaymentMethodId = dr["PaymentMethodId"] != DBNull.Value ? (PaymentMethod)(Convert.ToInt32(dr["PaymentMethodId"].ToString())) : PaymentMethod.None;
                    donorTestInfo.PaymentNote = dr["PaymentNote"] != DBNull.Value ? dr["PaymentNote"].ToString() : null;
                    donorTestInfo.PaymentStatus = dr["PaymentStatus"] != DBNull.Value ? (PaymentStatus)(Convert.ToInt32(dr["PaymentStatus"].ToString())) : PaymentStatus.None;

                    if (dr["LaboratoryCost"] != DBNull.Value)
                    {
                        donorTestInfo.LaboratoryCost = Convert.ToDouble(dr["LaboratoryCost"]);
                    }

                    if (dr["MROCost"] != DBNull.Value)
                    {
                        donorTestInfo.MROCost = Convert.ToDouble(dr["MROCost"]);
                    }

                    if (dr["CupCost"] != DBNull.Value)
                    {
                        donorTestInfo.CupCost = Convert.ToDouble(dr["CupCost"]);
                    }

                    if (dr["ShippingCost"] != DBNull.Value)
                    {
                        donorTestInfo.ShippingCost = Convert.ToDouble(dr["ShippingCost"]);
                    }

                    if (dr["VendorCost"] != DBNull.Value)
                    {
                        donorTestInfo.VendorCost = Convert.ToDouble(dr["VendorCost"]);
                    }
                    if (dr["CollectionSite1Id"] != DBNull.Value)
                    {
                        donorTestInfo.CollectionSite1Id = Convert.ToInt32(dr["CollectionSite1Id"]);
                    }
                    if (dr["CollectionSite2Id"] != DBNull.Value)
                    {
                        donorTestInfo.CollectionSite2Id = Convert.ToInt32(dr["CollectionSite2Id"]);
                    }
                    if (dr["CollectionSite3Id"] != DBNull.Value)
                    {
                        donorTestInfo.CollectionSite3Id = Convert.ToInt32(dr["CollectionSite3Id"]);
                    }
                    if (dr["CollectionSite4Id"] != DBNull.Value)
                    {
                        donorTestInfo.CollectionSite4Id = Convert.ToInt32(dr["CollectionSite4Id"]);
                    }
                    if (dr["ScheduleDate"] != DBNull.Value)
                    {
                        donorTestInfo.ScheduleDate = Convert.ToDateTime(dr["ScheduleDate"]);
                    }

                    if (dr["IsWalkinDonor"] != DBNull.Value)
                    {
                        donorTestInfo.IsWalkinDonor = dr["IsWalkinDonor"].ToString() == "1" ? true : false; ;
                    }
                    if (dr["IsInstantTest"] != DBNull.Value)
                    {
                        donorTestInfo.IsInstantTest = dr["IsInstantTest"].ToString() == "1" ? true : false; ;
                    }
                    if (dr["IsPaymentReceived"] != DBNull.Value)
                    {
                        donorTestInfo.IsPaymentReceived = dr["IsPaymentReceived"].ToString() == "1" ? true : false; ;
                    }
                    donorTestInfo.InstantTestResult = dr["InstantTestResult"] != DBNull.Value ? (InstantTestResult)(Convert.ToInt32(dr["InstantTestResult"].ToString())) : InstantTestResult.None;

                    //if (dr["IsRegularAfterInstantTest"] != DBNull.Value)
                    //{
                    //    donorTestInfo.IsRegularAfterInstantTest = dr["IsRegularAfterInstantTest"].ToString() == "1" ? true : false; ;
                    //}
                    if (dr["IsReverseEntry"] != DBNull.Value)
                    {
                        donorTestInfo.IsReverseEntry = dr["IsReverseEntry"].ToString() == "1" ? true : false;
                    }
                    donorTestInfo.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    donorTestInfo.CreatedOn = (DateTime)dr["CreatedOn"];
                    donorTestInfo.CreatedBy = (string)dr["CreatedBy"];
                    donorTestInfo.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    donorTestInfo.LastModifiedBy = (string)dr["LastModifiedBy"];
                }
                dr.Close();
                _logger.Debug($"dr closed, donorTestInfo.TestStatus = {donorTestInfo.TestStatus}");
                //Donor Test Category
                if (donorTestInfo != null)
                {
                    //sqlQuery = "SELECT "
                    //            + "donor_test_test_category_id AS DonorTestTestCategoryId, "
                    //            + "donor_test_info_id AS DonorTestInfoId, "
                    //            + "test_category_id AS TestCategoryId, "
                    //            + "test_panel_id AS TestPanelId, "
                    //            + "specimen_id AS SpecimenId, "
                    //            + "hair_test_panel_days AS HairTestPanelDays, "
                    //            + "test_panel_result AS TestPanelResult, "
                    //            + "test_panel_status AS TestPanelStatus, "
                    //            + "test_panel_cost AS TestPanelCost, "
                    //            + "test_panel_price AS TestPanelPrice, "
                    //            + "is_synchronized AS IsSynchronized, "
                    //            + "created_on AS CreatedOn, "
                    //            + "created_by AS CreatedBy, "
                    //            + "last_modified_on AS LastModifiedOn, "
                    //            + "last_modified_by AS LastModifiedBy "
                    //            + "FROM donor_test_info_test_categories "
                    //            + "WHERE donor_test_info_id = @DonorTestInfoId";

                    sqlQuery = "SELECT "
                    + " donor_test_info_test_categories.donor_test_test_category_id AS DonorTestTestCategoryId, "
                    + " donor_test_info_test_categories.donor_test_info_id AS DonorTestInfoId, "
                    + " donor_test_info_test_categories.test_category_id AS TestCategoryId, "
                    + " donor_test_info_test_categories.test_panel_id AS TestPanelId, "
                    + " donor_test_info_test_categories.specimen_id AS SpecimenId, "
                    + " donor_test_info_test_categories.hair_test_panel_days AS HairTestPanelDays, "
                    + " donor_test_info_test_categories.test_panel_result AS TestPanelResult, "
                    + " donor_test_info_test_categories.test_panel_status AS TestPanelStatus, "
                    + " donor_test_info_test_categories.test_panel_cost AS TestPanelCost, "
                    + " donor_test_info_test_categories.test_panel_price AS TestPanelPrice, "
                    + " donor_test_info_test_categories.is_synchronized AS IsSynchronized, "
                    + " donor_test_info_test_categories.created_on AS CreatedOn, "
                    + " donor_test_info_test_categories.created_by AS CreatedBy, "
                    + " donor_test_info_test_categories.last_modified_on AS LastModifiedOn, "
                    + " donor_test_info_test_categories.last_modified_by AS LastModifiedBy,"
                    + " test_panels.test_panel_name AS TestPanelName "
                    + " FROM donor_test_info_test_categories "
                    + " LEFT OUTER JOIN test_panels ON test_panels.test_panel_id = donor_test_info_test_categories.test_panel_id "
                    + " WHERE donor_test_info_id = @DonorTestInfoId";

                    param = new MySqlParameter[1];
                    param[0] = new MySqlParameter("@DonorTestInfoId", donorTestInfoId);

                    dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                    while (dr.Read())
                    {
                        DonorTestInfoTestCategories testCategory = new DonorTestInfoTestCategories();

                        testCategory.DonorTestTestCategoryId = (int)dr["DonorTestTestCategoryId"];
                        testCategory.DonorTestInfoId = (int)dr["DonorTestInfoId"];
                        testCategory.TestCategoryId = (TestCategories)((int)dr["TestCategoryId"]);

                        if (dr["TestPanelId"] != DBNull.Value)
                        {
                            testCategory.TestPanelId = Convert.ToInt32(dr["TestPanelId"]);
                            testCategory.TestPanelName = (string)dr["TestPanelName"];
                        }

                        testCategory.SpecimenId = dr["SpecimenId"] != DBNull.Value ? dr["SpecimenId"].ToString() : null;

                        if (dr["HairTestPanelDays"] != DBNull.Value)
                        {
                            testCategory.HairTestPanelDays = Convert.ToInt32(dr["HairTestPanelDays"]);
                        }

                        if (dr["TestPanelResult"] != DBNull.Value)
                        {
                            testCategory.TestPanelResult = Convert.ToInt32(dr["TestPanelResult"]);
                        }

                        testCategory.TestPanelStatus = dr["TestPanelStatus"] != DBNull.Value ? (DonorRegistrationStatus)((int)dr["TestPanelStatus"]) : DonorRegistrationStatus.None;

                        if (dr["TestPanelCost"] != DBNull.Value)
                        {
                            testCategory.TestPanelCost = Convert.ToDouble(dr["TestPanelCost"]);
                        }

                        if (dr["TestPanelPrice"] != DBNull.Value)
                        {
                            testCategory.TestPanelPrice = Convert.ToDouble(dr["TestPanelPrice"]);
                        }

                        testCategory.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                        testCategory.CreatedOn = (DateTime)dr["CreatedOn"];
                        testCategory.CreatedBy = (string)dr["CreatedBy"];
                        testCategory.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                        testCategory.LastModifiedBy = (string)dr["LastModifiedBy"];

                        donorTestInfo.TestInfoTestCategories.Add(testCategory);
                    }
                    dr.Close();
                    _logger.Debug($"dr closed after categories, donorTestInfo.TestStatus = {donorTestInfo.TestStatus}");
                    //Attorney Mapping
                    sqlQuery = "SELECT "
                                + "display_order AS DisplayOrder, "
                                + "attorney_id AS AttorneyId "
                                + "FROM donor_test_info_attorneys "
                                + "WHERE donor_test_info_id = @DonorTestInfoId";

                    param = new MySqlParameter[1];
                    param[0] = new MySqlParameter("@DonorTestInfoId", donorTestInfoId);

                    dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                    while (dr.Read())
                    {
                        if (Convert.ToInt32(dr["DisplayOrder"]) == 1)
                        {
                            if (dr["AttorneyId"] != null && dr["AttorneyId"].ToString() != string.Empty)
                            {
                                donorTestInfo.AttorneyId1 = Convert.ToInt32(dr["AttorneyId"]);
                            }
                            else
                            {
                                donorTestInfo.AttorneyId1 = null; ;
                            }
                        }
                        else if (Convert.ToInt32(dr["DisplayOrder"]) == 2)
                        {
                            if (dr["AttorneyId"] != null && dr["AttorneyId"].ToString() != string.Empty)
                            {
                                donorTestInfo.AttorneyId2 = Convert.ToInt32(dr["AttorneyId"]);
                            }
                            else
                            {
                                donorTestInfo.AttorneyId2 = null; ;
                            }
                        }
                        else if (Convert.ToInt32(dr["DisplayOrder"]) == 3)
                        {
                            if (dr["AttorneyId"] != null && dr["AttorneyId"].ToString() != string.Empty)
                            {
                                donorTestInfo.AttorneyId3 = Convert.ToInt32(dr["AttorneyId"]);
                            }
                            else
                            {
                                donorTestInfo.AttorneyId3 = null; ;
                            }
                        }
                    }
                    dr.Close();

                    //Third Party Mapping
                    sqlQuery = "SELECT "
                                + "display_order AS DisplayOrder, "
                                + "third_party_id AS ThirdPartyId "
                                + "FROM donor_test_info_third_parties "
                                + "WHERE donor_test_info_id = @DonorTestInfoId";

                    param = new MySqlParameter[1];
                    param[0] = new MySqlParameter("@DonorTestInfoId", donorTestInfoId);

                    dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                    while (dr.Read())
                    {
                        if (Convert.ToInt32(dr["DisplayOrder"]) == 1)
                        {
                            if (dr["ThirdPartyId"] != null && dr["ThirdPartyId"].ToString() != string.Empty)
                            {
                                donorTestInfo.ThirdPartyInfoId1 = Convert.ToInt32(dr["ThirdPartyId"]);
                            }
                            else
                            {
                                donorTestInfo.ThirdPartyInfoId1 = null;
                            }
                        }
                        else if (Convert.ToInt32(dr["DisplayOrder"]) == 2)
                        {
                            if (dr["ThirdPartyId"] != null && dr["ThirdPartyId"].ToString() != string.Empty)
                            {
                                donorTestInfo.ThirdPartyInfoId2 = Convert.ToInt32(dr["ThirdPartyId"]);
                            }
                            else
                            {
                                donorTestInfo.ThirdPartyInfoId2 = null;
                            }
                        }
                    }
                    dr.Close();
                }
            }

            return donorTestInfo;
        }

        public DonorTestInfo GetDonorTestInfoByDonorId(int donorId)
        {
            DonorTestInfo donorTestInfo = null;

            string sqlQuery = "SELECT "
                                + "MAX(donor_test_info_id) AS DonorTestInfoId, "
                                + "donor_id AS DonorId, "
                                + "client_id AS ClientId, "
                                + "client_department_id AS ClientDepartmentId, "
                                + "mro_type_id AS MROTypeId, "
                                + "payment_type_id AS PaymentTypeId, "
                                + "test_requested_date AS TestRequestedDate, "
                                + "test_requested_by AS TestRequestedBy, "
                                + "is_ua AS IsUA, "
                                + "is_hair AS IsHair, "
                                + "is_dna AS IsDNA, "
                                + "is_bc AS IsBC, "
                                + "reason_for_test_id AS ReasonForTestId, "
                                + "other_reason AS OtherReason, "
                                + "is_temperature_in_range AS IsTemperatureInRange, "
                                + "temperature_of_specimen AS TemperatureOfSpecimen, "
                                + "testing_authority_id AS TestingAuthorityId, "
                                + "specimen_collection_cup_id AS SpecimenCollectionCupId, "
                                + "is_observed AS IsObserved, "
                                + "form_type_id AS FormTypeId, "
                                + "is_adulteration_sign AS IsAdulterationSign, "
                                + "is_quantity_sufficient AS IsQuantitySufficient, "
                                + "collection_site_vendor_id AS CollectionSiteVendorId, "
                                + "collection_site_location_id AS CollectionSiteLocationId, "
                                + "collection_site_user_id AS CollectionSiteUserId, "
                                + "screening_time AS ScreeningTime, "
                                + "is_donor_refused AS IsDonorRefused, "
                                + "collection_site_remarks AS CollectionSiteRemarks, "
                                + "laboratory_vendor_id AS LaboratoryVendorId, "
                                + "mro_vendor_id AS MROVendorId, "
                                + "test_overall_result AS TestOverallResult, "
                                + "test_status AS TestStatus, "
                                + "program_type_id AS ProgramTypeId, "
                                + "is_surscan_determines_dates AS IsSurscanDeterminesDates, "
                                + "is_tp_determines_dates AS IsTpDeterminesDates, "
                                + "program_start_date AS ProgramStartDate, "
                                + "program_end_date AS ProgramEndDate, "
                                + "case_number AS CaseNumber, "
                                + "court_id AS CourtId, "
                                + "judge_id AS JudgeId, "
                                + "special_notes AS SpecialNotes, "
                                + "total_payment_amount AS TotalPaymentAmount, "
                                + "payment_date AS PaymentDate, "
                                + "payment_method_id AS PaymentMethodId, "
                                + "payment_note AS PaymentNote, "
                                + "payment_status AS PaymentStatus, "
                                + "laboratory_cost AS LaboratoryCost, "
                                + "mro_cost AS MROCost, "
                                + "cup_cost AS CupCost, "
                                + "shipping_cost AS ShippingCost, "
                                + "vendor_cost AS VendorCost, "
                                + "collection_site_1_id AS CollectionSite1Id, "
                                + "collection_site_2_id AS  CollectionSite2Id, "
                                + "collection_site_3_id AS CollectionSite3Id, "
                                + "collection_site_4_id AS CollectionSite4Id, "
                                + "schedule_date AS ScheduleDate, "
                                + "is_walkin_donor AS IsWalkinDonor, "
                                + "is_instant_test AS IsInstantTest, "
                                + "instant_test_result AS InstantTestResult, "
                                + "is_synchronized AS IsSynchronized, "
                                + "created_on AS CreatedOn, "
                                + "created_by AS CreatedBy, "
                                + "last_modified_on AS LastModifiedOn, "
                                + "last_modified_by AS LastModifiedBy "
                                + "FROM donor_test_info "
                                + "WHERE donor_id = @DonorId";

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                MySqlParameter[] param = new MySqlParameter[1];
                param[0] = new MySqlParameter("@DonorId", donorId);

                MySqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                if (dr.Read())
                {
                    donorTestInfo = new DonorTestInfo();
                    if (dr["DonorTestInfoId"].ToString() != "")
                    {
                        donorTestInfo.DonorTestInfoId = (int)dr["DonorTestInfoId"];
                        dr.Close();

                        sqlQuery = "SELECT "
                                   + "donor_test_info_id AS DonorTestInfoId, "
                                   + "donor_id AS DonorId, "
                                   + "client_id AS ClientId, "
                                   + "client_department_id AS ClientDepartmentId, "
                                   + "mro_type_id AS MROTypeId, "
                                   + "payment_type_id AS PaymentTypeId, "
                                   + "test_requested_date AS TestRequestedDate, "
                                   + "test_requested_by AS TestRequestedBy, "
                                   + "is_ua AS IsUA, "
                                   + "is_hair AS IsHair, "
                                   + "is_dna AS IsDNA, "
                                   + "is_bc AS IsBC, "
                                   + "reason_for_test_id AS ReasonForTestId, "
                                   + "other_reason AS OtherReason, "
                                   + "is_temperature_in_range AS IsTemperatureInRange, "
                                   + "temperature_of_specimen AS TemperatureOfSpecimen, "
                                   + "testing_authority_id AS TestingAuthorityId, "
                                   + "specimen_collection_cup_id AS SpecimenCollectionCupId, "
                                   + "is_observed AS IsObserved, "
                                   + "form_type_id AS FormTypeId, "
                                   + "is_adulteration_sign AS IsAdulterationSign, "
                                   + "is_quantity_sufficient AS IsQuantitySufficient, "
                                   + "collection_site_vendor_id AS CollectionSiteVendorId, "
                                   + "collection_site_location_id AS CollectionSiteLocationId, "
                                   + "collection_site_user_id AS CollectionSiteUserId, "
                                   + "screening_time AS ScreeningTime, "
                                   + "is_donor_refused AS IsDonorRefused, "
                                   + "collection_site_remarks AS CollectionSiteRemarks, "
                                   + "laboratory_vendor_id AS LaboratoryVendorId, "
                                   + "mro_vendor_id AS MROVendorId, "
                                   + "test_overall_result AS TestOverallResult, "
                                   + "test_status AS TestStatus, "
                                   + "program_type_id AS ProgramTypeId, "
                                   + "is_surscan_determines_dates AS IsSurscanDeterminesDates, "
                                   + "is_tp_determines_dates AS IsTpDeterminesDates, "
                                   + "program_start_date AS ProgramStartDate, "
                                   + "program_end_date AS ProgramEndDate, "
                                   + "case_number AS CaseNumber, "
                                   + "court_id AS CourtId, "
                                   + "judge_id AS JudgeId, "
                                   + "special_notes AS SpecialNotes, "
                                   + "total_payment_amount AS TotalPaymentAmount, "
                                   + "payment_date AS PaymentDate, "
                                   + "payment_method_id AS PaymentMethodId, "
                                   + "payment_note AS PaymentNote, "
                                   + "payment_status AS PaymentStatus, "
                                   + "laboratory_cost AS LaboratoryCost, "
                                   + "mro_cost AS MROCost, "
                                   + "cup_cost AS CupCost, "
                                   + "shipping_cost AS ShippingCost, "
                                   + "vendor_cost AS VendorCost, "
                                   + "collection_site_1_id AS CollectionSite1Id, "
                                   + "collection_site_2_id AS  CollectionSite2Id, "
                                   + "collection_site_3_id AS CollectionSite3Id, "
                                   + "collection_site_4_id AS CollectionSite4Id, "
                                   + "schedule_date AS ScheduleDate, "
                                   + "is_walkin_donor AS IsWalkinDonor, "
                                   + "is_instant_test AS IsInstantTest, "
                                   + "instant_test_result AS InstantTestResult, "
                                   + "is_synchronized AS IsSynchronized, "
                                   + "created_on AS CreatedOn, "
                                   + "created_by AS CreatedBy, "
                                   + "last_modified_on AS LastModifiedOn, "
                                   + "last_modified_by AS LastModifiedBy "
                                   + "FROM donor_test_info "
                                   + "WHERE donor_test_info_id = " + donorTestInfo.DonorTestInfoId + "";
                        dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                        if (dr.Read())
                        {
                            donorTestInfo.DonorTestInfoId = (int)dr["DonorTestInfoId"];
                            donorTestInfo.DonorId = (int)dr["DonorId"];
                            donorTestInfo.ClientId = (int)dr["ClientId"];
                            donorTestInfo.ClientDepartmentId = (int)dr["ClientDepartmentId"];
                            donorTestInfo.MROTypeId = dr["MROTypeId"] != DBNull.Value ? (ClientMROTypes)(Convert.ToInt32(dr["MROTypeId"].ToString())) : ClientMROTypes.None;
                            donorTestInfo.PaymentTypeId = dr["PaymentTypeId"] != DBNull.Value ? (ClientPaymentTypes)(Convert.ToInt32(dr["PaymentTypeId"].ToString())) : ClientPaymentTypes.None;
                            donorTestInfo.TestRequestedDate = dr["TestRequestedDate"] != DBNull.Value ? Convert.ToDateTime(dr["TestRequestedDate"].ToString()) : DateTime.MinValue;
                            donorTestInfo.TestRequestedBy = (int)dr["TestRequestedBy"];

                            if (dr["IsUA"] != DBNull.Value)
                            {
                                donorTestInfo.IsUA = dr["IsUA"].ToString() == "1" ? true : false;
                            }

                            if (dr["IsHair"] != DBNull.Value)
                            {
                                donorTestInfo.IsHair = dr["IsHair"].ToString() == "1" ? true : false; ;
                            }

                            if (dr["IsDNA"] != DBNull.Value)
                            {
                                donorTestInfo.IsDNA = dr["IsDNA"].ToString() == "1" ? true : false; ;
                            }
                            if (dr["IsBC"] != DBNull.Value)
                            {
                                donorTestInfo.IsBC = dr["IsBC"].ToString() == "1" ? true : false; ;
                            }

                            donorTestInfo.ReasonForTestId = dr["ReasonForTestId"] != DBNull.Value ? (TestInfoReasonForTest)(Convert.ToInt32(dr["ReasonForTestId"].ToString())) : TestInfoReasonForTest.None;
                            donorTestInfo.OtherReason = dr["OtherReason"] != DBNull.Value ? dr["OtherReason"].ToString() : null;
                            donorTestInfo.IsTemperatureInRange = dr["IsTemperatureInRange"] != DBNull.Value ? (YesNo)(Convert.ToInt32(dr["IsTemperatureInRange"].ToString())) : YesNo.None;

                            if (dr["TemperatureOfSpecimen"] != DBNull.Value)
                            {
                                donorTestInfo.TemperatureOfSpecimen = Convert.ToDouble(dr["TemperatureOfSpecimen"]);
                            }

                            if (dr["TestingAuthorityId"] != DBNull.Value)
                            {
                                donorTestInfo.TestingAuthorityId = Convert.ToInt32(dr["TestingAuthorityId"]);
                            }

                            donorTestInfo.SpecimenCollectionCupId = dr["SpecimenCollectionCupId"] != DBNull.Value ? (SpecimenCollectionCupType)(Convert.ToInt32(dr["SpecimenCollectionCupId"].ToString())) : SpecimenCollectionCupType.None;
                            donorTestInfo.IsObserved = dr["IsObserved"] != DBNull.Value ? (YesNo)(Convert.ToInt32(dr["IsObserved"].ToString())) : YesNo.None;
                            donorTestInfo.FormTypeId = dr["FormTypeId"] != DBNull.Value ? (SpecimenFormType)(Convert.ToInt32(dr["FormTypeId"].ToString())) : SpecimenFormType.None;
                            donorTestInfo.IsAdulterationSign = dr["IsAdulterationSign"] != DBNull.Value ? (YesNo)(Convert.ToInt32(dr["IsAdulterationSign"].ToString())) : YesNo.None;
                            donorTestInfo.IsQuantitySufficient = dr["IsQuantitySufficient"] != DBNull.Value ? (YesNo)(Convert.ToInt32(dr["IsQuantitySufficient"].ToString())) : YesNo.None;

                            if (dr["CollectionSiteVendorId"] != DBNull.Value)
                            {
                                donorTestInfo.CollectionSiteVendorId = Convert.ToInt32(dr["CollectionSiteVendorId"]);
                            }

                            if (dr["CollectionSiteLocationId"] != DBNull.Value)
                            {
                                donorTestInfo.CollectionSiteLocationId = Convert.ToInt32(dr["CollectionSiteLocationId"]);
                            }

                            if (dr["CollectionSiteUserId"] != DBNull.Value)
                            {
                                donorTestInfo.CollectionSiteUserId = Convert.ToInt32(dr["CollectionSiteUserId"]);
                            }

                            if (dr["ScreeningTime"] != DBNull.Value)
                            {
                                donorTestInfo.ScreeningTime = Convert.ToDateTime(dr["ScreeningTime"]);
                            }

                            if (dr["IsDonorRefused"] != DBNull.Value)
                            {
                                donorTestInfo.IsDonorRefused = Convert.ToBoolean(dr["IsDonorRefused"]);
                            }

                            donorTestInfo.CollectionSiteRemarks = dr["CollectionSiteRemarks"] != DBNull.Value ? dr["CollectionSiteRemarks"].ToString() : null;

                            if (dr["LaboratoryVendorId"] != DBNull.Value)
                            {
                                donorTestInfo.LaboratoryVendorId = Convert.ToInt32(dr["LaboratoryVendorId"]);
                            }

                            if (dr["MROVendorId"] != DBNull.Value)
                            {
                                donorTestInfo.MROVendorId = Convert.ToInt32(dr["MROVendorId"]);
                            }

                            if (dr["TestOverallResult"] != DBNull.Value)
                            {
                                donorTestInfo.TestOverallResult = Convert.ToInt32(dr["TestOverallResult"]);
                            }

                            donorTestInfo.TestStatus = dr["TestStatus"] != DBNull.Value ? (DonorRegistrationStatus)(Convert.ToInt32(dr["TestStatus"].ToString())) : DonorRegistrationStatus.None;
                            donorTestInfo.ProgramTypeId = dr["ProgramTypeId"] != DBNull.Value ? (ProgramType)(Convert.ToInt32(dr["ProgramTypeId"].ToString())) : ProgramType.None;

                            if (dr["IsSurscanDeterminesDates"] != DBNull.Value)
                            {
                                donorTestInfo.IsSurscanDeterminesDates = Convert.ToBoolean(dr["IsSurscanDeterminesDates"]);
                            }

                            if (dr["IsTpDeterminesDates"] != DBNull.Value)
                            {
                                donorTestInfo.IsTpDeterminesDates = Convert.ToBoolean(dr["IsTpDeterminesDates"]);
                            }

                            if (dr["ProgramStartDate"] != DBNull.Value)
                            {
                                donorTestInfo.ProgramStartDate = Convert.ToDateTime(dr["ProgramStartDate"]);
                            }

                            if (dr["ProgramEndDate"] != DBNull.Value)
                            {
                                donorTestInfo.ProgramEndDate = Convert.ToDateTime(dr["ProgramEndDate"]);
                            }

                            donorTestInfo.CaseNumber = dr["CaseNumber"] != DBNull.Value ? dr["CaseNumber"].ToString() : null;

                            if (dr["CourtId"] != DBNull.Value)
                            {
                                donorTestInfo.CourtId = Convert.ToInt32(dr["CourtId"]);
                            }

                            if (dr["JudgeId"] != DBNull.Value)
                            {
                                donorTestInfo.JudgeId = Convert.ToInt32(dr["JudgeId"]);
                            }

                            donorTestInfo.SpecialNotes = dr["SpecialNotes"] != DBNull.Value ? dr["SpecialNotes"].ToString() : null;

                            if (dr["TotalPaymentAmount"] != DBNull.Value)
                            {
                                donorTestInfo.TotalPaymentAmount = Convert.ToDouble(dr["TotalPaymentAmount"]);
                            }

                            if (dr["PaymentDate"] != DBNull.Value)
                            {
                                donorTestInfo.PaymentDate = Convert.ToDateTime(dr["PaymentDate"]);
                            }

                            donorTestInfo.PaymentMethodId = dr["PaymentMethodId"] != DBNull.Value ? (PaymentMethod)(Convert.ToInt32(dr["PaymentMethodId"].ToString())) : PaymentMethod.None;
                            donorTestInfo.PaymentNote = dr["PaymentNote"] != DBNull.Value ? dr["PaymentNote"].ToString() : null;
                            donorTestInfo.PaymentStatus = dr["PaymentStatus"] != DBNull.Value ? (PaymentStatus)(Convert.ToInt32(dr["PaymentStatus"].ToString())) : PaymentStatus.None;

                            if (dr["LaboratoryCost"] != DBNull.Value)
                            {
                                donorTestInfo.LaboratoryCost = Convert.ToDouble(dr["LaboratoryCost"]);
                            }

                            if (dr["MROCost"] != DBNull.Value)
                            {
                                donorTestInfo.MROCost = Convert.ToDouble(dr["MROCost"]);
                            }

                            if (dr["CupCost"] != DBNull.Value)
                            {
                                donorTestInfo.CupCost = Convert.ToDouble(dr["CupCost"]);
                            }

                            if (dr["ShippingCost"] != DBNull.Value)
                            {
                                donorTestInfo.ShippingCost = Convert.ToDouble(dr["ShippingCost"]);
                            }

                            if (dr["VendorCost"] != DBNull.Value)
                            {
                                donorTestInfo.VendorCost = Convert.ToDouble(dr["VendorCost"]);
                            }
                            if (dr["CollectionSite1Id"] != DBNull.Value)
                            {
                                donorTestInfo.CollectionSite1Id = Convert.ToInt32(dr["CollectionSite1Id"]);
                            }
                            if (dr["CollectionSite2Id"] != DBNull.Value)
                            {
                                donorTestInfo.CollectionSite2Id = Convert.ToInt32(dr["CollectionSite2Id"]);
                            }
                            if (dr["CollectionSite3Id"] != DBNull.Value)
                            {
                                donorTestInfo.CollectionSite3Id = Convert.ToInt32(dr["CollectionSite3Id"]);
                            }
                            if (dr["CollectionSite4Id"] != DBNull.Value)
                            {
                                donorTestInfo.CollectionSite4Id = Convert.ToInt32(dr["CollectionSite4Id"]);
                            }
                            if (dr["ScheduleDate"] != DBNull.Value)
                            {
                                donorTestInfo.ScheduleDate = Convert.ToDateTime(dr["ScheduleDate"]);
                            }

                            if (dr["IsWalkinDonor"] != DBNull.Value)
                            {
                                donorTestInfo.IsWalkinDonor = dr["IsWalkinDonor"].ToString() == "1" ? true : false;
                            }
                            if (dr["IsInstantTest"] != DBNull.Value)
                            {
                                donorTestInfo.IsInstantTest = dr["IsInstantTest"].ToString() == "1" ? true : false;
                            }

                            donorTestInfo.InstantTestResult = dr["InstantTestResult"] != DBNull.Value ? (InstantTestResult)(Convert.ToInt32(dr["InstantTestResult"].ToString())) : InstantTestResult.None;

                            //if (dr["IsRegularAfterInstantTest"] != DBNull.Value)
                            //{
                            //    donorTestInfo.IsRegularAfterInstantTest = dr["IsRegularAfterInstantTest"].ToString() == "1" ? true : false; ;
                            //}
                            donorTestInfo.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                            donorTestInfo.CreatedOn = (DateTime)dr["CreatedOn"];
                            donorTestInfo.CreatedBy = (string)dr["CreatedBy"];
                            donorTestInfo.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                            donorTestInfo.LastModifiedBy = (string)dr["LastModifiedBy"];
                        }
                    }
                }
            }

            return donorTestInfo;
        }
    }
}