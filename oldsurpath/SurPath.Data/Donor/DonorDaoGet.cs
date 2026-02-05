using MySql.Data.MySqlClient;
using SurPath.Entity;
using SurPath.Enum;
using SurPath.MySQLHelper;
using System;
using System.Collections.Generic;
using System.Data;

namespace SurPath.Data
{
    public partial class DonorDao : DataObject
    {
        public DataTable GetSpecimenId(string specimenId)
        {
            string sqlQuery = "SELECT "
                            + "donor_test_info_id AS DonorTestInfoId, "
                            + "test_category_id AS TestCategoryId, "
                            + "test_panel_id AS TestPanelId, "
                            + "specimen_id AS SpecimenId, "
                            + "hair_test_panel_days AS HairTestPanelDays, "
                            + "test_panel_price AS TestPanelPrice, "
                            + "test_panel_status AS TestPanelStatus, "
                            + "last_modified_by AS LastModifiedBy "
                            + "FROM  donor_test_info_test_categories "
                            + "WHERE specimen_id = @SpecimenId ";

            MySqlParameter[] param = new MySqlParameter[1];

            param[0] = new MySqlParameter("@SpecimenId", specimenId);

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

        public Donor Get(int donorId)
        {
            Donor donor = null;

            string sqlQuery = "SELECT "
                                + "donor_id AS DonorId, "
                                + "donorClearStarProfId AS DonorClearStarProfId, "
                                + "donor_first_name AS DonorFirstName, "
                                + "donor_mi AS DonorMI, "
                                + "donor_last_name AS DonorLastName, "
                                + "donor_suffix AS DonorSuffix, "
                                + "donor_ssn AS DonorSSN, "
                                + "donor_pid AS donor_pid, "
                                + "donor_date_of_birth AS DonorDateOfBirth, "
                                + "donor_phone_1 AS DonorPhone1, "
                                + "donor_phone_2 AS DonorPhone2, "
                                + "donor_address_1 AS DonorAddress1, "
                                + "donor_address_2 AS DonorAddress2, "
                                + "donor_city AS DonorCity, "
                                + "donor_state AS DonorState, "
                                + "donor_zip AS DonorZip, "
                                + "donor_email AS DonorEmail, "
                                + "donor_gender AS DonorGender, "
                                + "donor_initial_client_id AS DonorInitialClientId, "
                                + "donor_initial_department_id AS DonorInitialDepartmentId, "
                                + "donor_registration_status AS DonorRegistrationStatus, "
                                + "is_synchronized AS IsSynchronized, "
                                + "is_archived AS IsArchived, "
                                + "is_hidden_web AS IsHiddenWeb, "
                                + "created_on AS CreatedOn, "
                                + "created_by AS CreatedBy, "
                                + "last_modified_on AS LastModifiedOn, "
                                + "last_modified_by AS LastModifiedBy, "
                                + " (select count(*) from donor_documents where (is_approved is null or is_approved=0) and donor_id = @DonorID ) as NotApproved "
                                + "FROM donors WHERE donor_id = @DonorId ";

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                MySqlParameter[] param = new MySqlParameter[1];
                param[0] = new MySqlParameter("@DonorId", donorId);

                MySqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                if (dr.Read())
                {
                    donor = new Donor();

                    donor.DonorId = (int)dr["DonorId"];
                    try
                    {
                        object value = dr["DonorClearStarProfId"];
                        if (value != DBNull.Value)
                        {
                            donor.DonorClearStarProfId = (string)dr["DonorClearStarProfId"];
                        }
                    }
                    catch (Exception ex)
                    {
                        donor.DonorClearStarProfId = string.Empty;
                    }

                    donor.DonorFirstName = (string)dr["DonorFirstName"];
                    donor.DonorMI = dr["DonorMI"].ToString();
                    donor.DonorLastName = (string)dr["DonorLastName"];
                    donor.DonorSuffix = dr["DonorSuffix"].ToString();
                    donor.DonorSSN = dr["DonorSSN"].ToString();
                    donor.DonorDateOfBirth = dr["DonorDateOfBirth"] != DBNull.Value ? Convert.ToDateTime(dr["DonorDateOfBirth"], this.Culture) : DateTime.MinValue;
                    donor.DonorPhone1 = dr["DonorPhone1"].ToString();
                    donor.DonorPhone2 = dr["DonorPhone2"].ToString();
                    donor.DonorAddress1 = dr["DonorAddress1"].ToString();
                    donor.DonorAddress2 = dr["DonorAddress2"].ToString();
                    donor.DonorCity = dr["DonorCity"].ToString();
                    donor.DonorState = dr["DonorState"].ToString();
                    donor.DonorZip = dr["DonorZip"].ToString();
                    donor.DonorEmail = dr["DonorEmail"].ToString();
                    donor.DonorGender = dr["DonorGender"] != DBNull.Value ? (Gender)(Convert.ToInt32(dr["DonorGender"].ToString())) : Gender.None;
                    donor.DonorInitialClientId = dr["DonorInitialClientId"] != null && dr["DonorInitialClientId"].ToString() != string.Empty ? (int)dr["DonorInitialClientId"] : 0;
                    donor.DonorInitialDepartmentId = dr["DonorInitialDepartmentId"] != null && dr["DonorInitialDepartmentId"].ToString() != string.Empty ? (int)dr["DonorInitialDepartmentId"] : 0;
                    donor.DonorRegistrationStatusValue = dr["DonorRegistrationStatus"] != null ? (DonorRegistrationStatus)dr["DonorRegistrationStatus"] : DonorRegistrationStatus.None;
                    donor.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    donor.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    donor.IsHiddenWeb = dr["IsHiddenWeb"].ToString() == "1" ? true : false;
                    donor.CreatedOn = (DateTime)dr["CreatedOn"];
                    donor.CreatedBy = (string)dr["CreatedBy"];
                    donor.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    donor.LastModifiedBy = (string)dr["LastModifiedBy"];
                    donor.NotApproved = (string)dr["NotApproved"].ToString();
                }
            }
            if (donor != null) PopulatePids(ref donor);
            return donor;
        }

        public Donor GetWeb(int donorId)
        {
            Donor donor = null;

            string sqlQuery = "SELECT "
                                + "donor_id AS DonorId, "
                                + "donorClearStarProfId AS DonorClearStarProfId, "
                                + "donor_first_name AS DonorFirstName, "
                                + "donor_mi AS DonorMI, "
                                + "donor_last_name AS DonorLastName, "
                                + "donor_suffix AS DonorSuffix, "
                                + "donor_ssn AS DonorSSN, "
                                + "donor_pid AS donor_pid, "
                                + "donor_date_of_birth AS DonorDateOfBirth, "
                                + "donor_phone_1 AS DonorPhone1, "
                                + "donor_phone_2 AS DonorPhone2, "
                                + "donor_address_1 AS DonorAddress1, "
                                + "donor_address_2 AS DonorAddress2, "
                                + "donor_city AS DonorCity, "
                                + "donor_state AS DonorState, "
                                + "donor_zip AS DonorZip, "
                                + "donor_email AS DonorEmail, "
                                + "donor_gender AS DonorGender, "
                                + "donor_initial_client_id AS DonorInitialClientId, "
                                + "donor_initial_department_id AS DonorInitialDepartmentId, "
                                + "donor_registration_status AS DonorRegistrationStatus, "
                                + "is_synchronized AS IsSynchronized, "
                                + "is_archived AS IsArchived, "
                                + "is_hidden_web AS IsHiddenWeb, "
                                + "created_on AS CreatedOn, "
                                + "created_by AS CreatedBy, "
                                + "last_modified_on AS LastModifiedOn, "
                                + "last_modified_by AS LastModifiedBy "
                                + "FROM donors WHERE donor_id = @DonorId and is_hidden_web=0";

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                MySqlParameter[] param = new MySqlParameter[1];
                param[0] = new MySqlParameter("@DonorId", donorId);

                MySqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                if (dr.Read())
                {
                    donor = new Donor();

                    donor.DonorId = (int)dr["DonorId"];
                    donor.DonorFirstName = (string)dr["DonorFirstName"];
                    donor.DonorMI = dr["DonorMI"].ToString();
                    donor.DonorLastName = (string)dr["DonorLastName"];
                    donor.DonorSuffix = dr["DonorSuffix"].ToString();
                    donor.DonorSSN = dr["DonorSSN"].ToString();
                    donor.DonorDateOfBirth = dr["DonorDateOfBirth"] != DBNull.Value ? Convert.ToDateTime(dr["DonorDateOfBirth"], this.Culture) : DateTime.MinValue;
                    donor.DonorPhone1 = dr["DonorPhone1"].ToString();
                    donor.DonorPhone2 = dr["DonorPhone2"].ToString();
                    donor.DonorAddress1 = dr["DonorAddress1"].ToString();
                    donor.DonorAddress2 = dr["DonorAddress2"].ToString();
                    donor.DonorCity = dr["DonorCity"].ToString();
                    donor.DonorState = dr["DonorState"].ToString();
                    donor.DonorZip = dr["DonorZip"].ToString();
                    donor.DonorEmail = dr["DonorEmail"].ToString();
                    donor.DonorGender = dr["DonorGender"] != DBNull.Value ? (Gender)(Convert.ToInt32(dr["DonorGender"].ToString())) : Gender.None;
                    donor.DonorInitialClientId = dr["DonorInitialClientId"] != null && dr["DonorInitialClientId"].ToString() != string.Empty ? (int)dr["DonorInitialClientId"] : 0;
                    donor.DonorInitialDepartmentId = dr["DonorInitialDepartmentId"] != null && dr["DonorInitialDepartmentId"].ToString() != string.Empty ? (int)dr["DonorInitialDepartmentId"] : 0;
                    donor.DonorRegistrationStatusValue = dr["DonorRegistrationStatus"] != null ? (DonorRegistrationStatus)dr["DonorRegistrationStatus"] : DonorRegistrationStatus.None;
                    donor.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    donor.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    donor.IsHiddenWeb = dr["IsHiddenWeb"].ToString() == "1" ? true : false;
                    donor.CreatedOn = (DateTime)dr["CreatedOn"];
                    donor.CreatedBy = (string)dr["CreatedBy"];
                    donor.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    donor.LastModifiedBy = (string)dr["LastModifiedBy"];
                }
            }
            if (donor != null) PopulatePids(ref donor);
            return donor;
        }

        public Donor GetBySSN(string ssn)
        {
            Donor donor = null;

            string sqlQuery = "SELECT "
                                + "donor_id AS DonorId, "
                                + "donorClearStarProfId AS DonorClearStarProfId, "
                                + "donor_first_name AS DonorFirstName, "
                                + "donor_mi AS DonorMI, "
                                + "donor_last_name AS DonorLastName, "
                                + "donor_suffix AS DonorSuffix, "
                                + "donor_ssn AS DonorSSN, "
                                + "donor_pid AS donor_pid, "
                                + "donor_date_of_birth AS DonorDateOfBirth, "
                                + "donor_phone_1 AS DonorPhone1, "
                                + "donor_phone_2 AS DonorPhone2, "
                                + "donor_address_1 AS DonorAddress1, "
                                + "donor_address_2 AS DonorAddress2, "
                                + "donor_city AS DonorCity, "
                                + "donor_state AS DonorState, "
                                + "donor_zip AS DonorZip, "
                                + "donor_email AS DonorEmail, "
                                + "donor_gender AS DonorGender, "
                                + "donor_initial_client_id AS DonorInitialClientId, "
                                + "donor_initial_department_id AS DonorInitialDepartmentId, "
                                + "donor_registration_status AS DonorRegistrationStatus, "
                                + "is_synchronized AS IsSynchronized, "
                                + "is_archived AS IsArchived, "
                                + "is_hidden_web as IsHiddenWeb, "
                                + "created_on AS CreatedOn, "
                                + "created_by AS CreatedBy, "
                                + "last_modified_on AS LastModifiedOn, "
                                + "last_modified_by AS LastModifiedBy "
                                + "FROM donors WHERE is_archived = 0 AND donor_ssn = @SSN";

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                MySqlParameter[] param = new MySqlParameter[1];
                param[0] = new MySqlParameter("@SSN", ssn);

                MySqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                if (dr.Read())
                {
                    donor = new Donor();

                    donor.DonorId = (int)dr["DonorId"];
                    donor.DonorFirstName = (string)dr["DonorFirstName"];
                    donor.DonorMI = dr["DonorMI"].ToString();
                    donor.DonorLastName = (string)dr["DonorLastName"];
                    donor.DonorSuffix = dr["DonorSuffix"].ToString();
                    donor.DonorSSN = dr["DonorSSN"].ToString();
                    donor.DonorDateOfBirth = dr["DonorDateOfBirth"] != DBNull.Value ? Convert.ToDateTime(dr["DonorDateOfBirth"], this.Culture) : DateTime.MinValue;
                    donor.DonorPhone1 = dr["DonorPhone1"].ToString();
                    donor.DonorPhone2 = dr["DonorPhone2"].ToString();
                    donor.DonorAddress1 = dr["DonorAddress1"].ToString();
                    donor.DonorAddress2 = dr["DonorAddress2"].ToString();
                    donor.DonorCity = dr["DonorCity"].ToString();
                    donor.DonorState = dr["DonorState"].ToString();
                    donor.DonorZip = dr["DonorZip"].ToString();
                    donor.DonorEmail = dr["DonorEmail"].ToString();
                    donor.DonorGender = dr["DonorGender"] != DBNull.Value ? (Gender)(Convert.ToInt32(dr["DonorGender"].ToString())) : Gender.None;
                    donor.DonorInitialClientId = dr["DonorInitialClientId"] != null && dr["DonorInitialClientId"].ToString() != string.Empty ? (int)dr["DonorInitialClientId"] : 0;
                    donor.DonorInitialDepartmentId = dr["DonorInitialDepartmentId"] != null && dr["DonorInitialDepartmentId"].ToString() != string.Empty ? (int)dr["DonorInitialDepartmentId"] : 0;
                    donor.DonorRegistrationStatusValue = dr["DonorRegistrationStatus"] != null ? (DonorRegistrationStatus)dr["DonorRegistrationStatus"] : DonorRegistrationStatus.None;
                    donor.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    donor.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    donor.CreatedOn = (DateTime)dr["CreatedOn"];
                    donor.CreatedBy = (string)dr["CreatedBy"];
                    donor.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    donor.LastModifiedBy = (string)dr["LastModifiedBy"];
                    donor.IsHiddenWeb = dr["IsHiddenWeb"].ToString() == "1" ? true : false;
                }
            }
            if (donor != null) PopulatePids(ref donor);
            return donor;
        }

        public Donor GetBySSNWeb(string ssn)
        {
            Donor donor = null;

            string sqlQuery = "SELECT "
                                + "donor_id AS DonorId, "
                                + "donorClearStarProfId AS DonorClearStarProfId, "
                                + "donor_first_name AS DonorFirstName, "
                                + "donor_mi AS DonorMI, "
                                + "donor_last_name AS DonorLastName, "
                                + "donor_suffix AS DonorSuffix, "
                                + "donor_pid AS donor_pid, "
                                + "donor_ssn AS DonorSSN, "
                                + "donor_date_of_birth AS DonorDateOfBirth, "
                                + "donor_phone_1 AS DonorPhone1, "
                                + "donor_phone_2 AS DonorPhone2, "
                                + "donor_address_1 AS DonorAddress1, "
                                + "donor_address_2 AS DonorAddress2, "
                                + "donor_city AS DonorCity, "
                                + "donor_state AS DonorState, "
                                + "donor_zip AS DonorZip, "
                                + "donor_email AS DonorEmail, "
                                + "donor_gender AS DonorGender, "
                                + "donor_initial_client_id AS DonorInitialClientId, "
                                + "donor_initial_department_id AS DonorInitialDepartmentId, "
                                + "donor_registration_status AS DonorRegistrationStatus, "
                                + "is_synchronized AS IsSynchronized, "
                                + "is_archived AS IsArchived, "
                                + "is_hidden_web as IsHiddenWeb, "
                                + "created_on AS CreatedOn, "
                                + "created_by AS CreatedBy, "
                                + "last_modified_on AS LastModifiedOn, "
                                + "last_modified_by AS LastModifiedBy "
                                + "FROM donors WHERE is_archived = 0 AND donor_ssn = @SSN and is_hidden_web=0 ";

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                MySqlParameter[] param = new MySqlParameter[1];
                param[0] = new MySqlParameter("@SSN", ssn);

                MySqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                if (dr.Read())
                {
                    donor = new Donor();

                    donor.DonorId = (int)dr["DonorId"];
                    donor.DonorFirstName = (string)dr["DonorFirstName"];
                    donor.DonorMI = dr["DonorMI"].ToString();
                    donor.DonorLastName = (string)dr["DonorLastName"];
                    donor.DonorSuffix = dr["DonorSuffix"].ToString();
                    donor.DonorSSN = dr["DonorSSN"].ToString();
                    donor.DonorDateOfBirth = dr["DonorDateOfBirth"] != DBNull.Value ? Convert.ToDateTime(dr["DonorDateOfBirth"], this.Culture) : DateTime.MinValue;
                    donor.DonorPhone1 = dr["DonorPhone1"].ToString();
                    donor.DonorPhone2 = dr["DonorPhone2"].ToString();
                    donor.DonorAddress1 = dr["DonorAddress1"].ToString();
                    donor.DonorAddress2 = dr["DonorAddress2"].ToString();
                    donor.DonorCity = dr["DonorCity"].ToString();
                    donor.DonorState = dr["DonorState"].ToString();
                    donor.DonorZip = dr["DonorZip"].ToString();
                    donor.DonorEmail = dr["DonorEmail"].ToString();
                    donor.DonorGender = dr["DonorGender"] != DBNull.Value ? (Gender)(Convert.ToInt32(dr["DonorGender"].ToString())) : Gender.None;
                    donor.DonorInitialClientId = dr["DonorInitialClientId"] != null && dr["DonorInitialClientId"].ToString() != string.Empty ? (int)dr["DonorInitialClientId"] : 0;
                    donor.DonorInitialDepartmentId = dr["DonorInitialDepartmentId"] != null && dr["DonorInitialDepartmentId"].ToString() != string.Empty ? (int)dr["DonorInitialDepartmentId"] : 0;
                    donor.DonorRegistrationStatusValue = dr["DonorRegistrationStatus"] != null ? (DonorRegistrationStatus)dr["DonorRegistrationStatus"] : DonorRegistrationStatus.None;
                    donor.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    donor.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    donor.CreatedOn = (DateTime)dr["CreatedOn"];
                    donor.CreatedBy = (string)dr["CreatedBy"];
                    donor.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    donor.LastModifiedBy = (string)dr["LastModifiedBy"];
                }
            }
            if (donor != null) PopulatePids(ref donor);
            return donor;
        }

        public Donor GetByEmail(string email)
        {
            Donor donor = null;

            string sqlQuery = "SELECT "
                                + "donor_id AS DonorId, "
                                + "donorClearStarProfId AS DonorClearStarProfId, "
                                + "donor_first_name AS DonorFirstName, "
                                + "donor_mi AS DonorMI, "
                                + "donor_last_name AS DonorLastName, "
                                + "donor_suffix AS DonorSuffix, "
                                + "donor_ssn AS DonorSSN, "
                                + "donor_pid AS donor_pid, "
                                + "donor_date_of_birth AS DonorDateOfBirth, "
                                + "donor_phone_1 AS DonorPhone1, "
                                + "donor_phone_2 AS DonorPhone2, "
                                + "donor_address_1 AS DonorAddress1, "
                                + "donor_address_2 AS DonorAddress2, "
                                + "donor_city AS DonorCity, "
                                + "donor_state AS DonorState, "
                                + "donor_zip AS DonorZip, "
                                + "donor_email AS DonorEmail, "
                                + "donor_gender AS DonorGender, "
                                + "donor_initial_client_id AS DonorInitialClientId, "
                                + "donor_initial_department_id AS DonorInitialDepartmentId, "
                                + "donor_registration_status AS DonorRegistrationStatus, "
                                + "is_synchronized AS IsSynchronized, "
                                + "is_archived AS IsArchived, "
                                 + "is_hidden_web as IsHiddenWeb, "
                                + "created_on AS CreatedOn, "
                                + "created_by AS CreatedBy, "
                                + "last_modified_on AS LastModifiedOn, "
                                + "last_modified_by AS LastModifiedBy "
                                + "FROM donors WHERE is_archived = 0 AND donor_email = @Email";

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                MySqlParameter[] param = new MySqlParameter[1];
                param[0] = new MySqlParameter("@Email", email);

                MySqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                if (dr.Read())
                {
                    donor = new Donor();

                    donor.DonorId = (int)dr["DonorId"];
                    donor.DonorFirstName = (string)dr["DonorFirstName"];
                    donor.DonorMI = dr["DonorMI"].ToString();
                    donor.DonorLastName = (string)dr["DonorLastName"];
                    donor.DonorSuffix = dr["DonorSuffix"].ToString();
                    donor.DonorSSN = dr["DonorSSN"].ToString();
                    donor.DonorDateOfBirth = dr["DonorDateOfBirth"] != DBNull.Value ? Convert.ToDateTime(dr["DonorDateOfBirth"], this.Culture) : DateTime.MinValue;
                    donor.DonorPhone1 = dr["DonorPhone1"].ToString();
                    donor.DonorPhone2 = dr["DonorPhone2"].ToString();
                    donor.DonorAddress1 = dr["DonorAddress1"].ToString();
                    donor.DonorAddress2 = dr["DonorAddress2"].ToString();
                    donor.DonorCity = dr["DonorCity"].ToString();
                    donor.DonorState = dr["DonorState"].ToString();
                    donor.DonorZip = dr["DonorZip"].ToString();
                    donor.DonorEmail = dr["DonorEmail"].ToString();
                    donor.DonorGender = dr["DonorGender"] != DBNull.Value ? (Gender)(Convert.ToInt32(dr["DonorGender"].ToString())) : Gender.None;
                    donor.DonorInitialClientId = dr["DonorInitialClientId"] != null && dr["DonorInitialClientId"].ToString() != string.Empty ? (int)dr["DonorInitialClientId"] : 0;
                    donor.DonorInitialDepartmentId = dr["DonorInitialDepartmentId"] != null && dr["DonorInitialDepartmentId"].ToString() != string.Empty ? (int)dr["DonorInitialDepartmentId"] : 0;
                    donor.DonorRegistrationStatusValue = dr["DonorRegistrationStatus"] != null ? (DonorRegistrationStatus)dr["DonorRegistrationStatus"] : DonorRegistrationStatus.None;
                    donor.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    donor.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    donor.CreatedOn = (DateTime)dr["CreatedOn"];
                    donor.CreatedBy = (string)dr["CreatedBy"];
                    donor.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    donor.LastModifiedBy = (string)dr["LastModifiedBy"];
                }
            }
            if (donor != null) PopulatePids(ref donor);
            return donor;
        }

        public Donor PopulatePids(ref Donor donor)
        {

//            string sqlQuery = @"
//select * from individual_pids where donor_id = @donor_id
//";
            BackendData backendData = new BackendData(null,null,_logger);
            // GetDonorPIDS
            donor.PidTypeValues = backendData.GetDonorPIDS(donor.DonorId);
            return donor;
        }

        public Donor GetByPID(string pid)
        {
            Donor donor = null;

            string sqlQuery = @"
                    SELECT 
donors.donor_id AS DonorId, 
donors.donorClearStarProfId AS DonorClearStarProfId, 
donors.donor_first_name AS DonorFirstName, 
donors.donor_mi AS DonorMI, 
donors.donor_last_name AS DonorLastName, 
donors.donor_suffix AS DonorSuffix, 
donors.donor_ssn AS DonorSSN, 
donors.donor_pid AS donor_pid, 
donors.donor_date_of_birth AS DonorDateOfBirth, 
donors.donor_phone_1 AS DonorPhone1, 
donors.donor_phone_2 AS DonorPhone2, 
donors.donor_address_1 AS DonorAddress1, 
donors.donor_address_2 AS DonorAddress2, 
donors.donor_city AS DonorCity, 
donors.donor_state AS DonorState, 
donors.donor_zip AS DonorZip, 
donors.donor_email AS DonorEmail, 
donors.donor_gender AS DonorGender, 
donors.donor_initial_client_id AS DonorInitialClientId, 
donors.donor_initial_department_id AS DonorInitialDepartmentId, 
donors.donor_registration_status AS DonorRegistrationStatus, 
donors.is_synchronized AS IsSynchronized, 
donors.is_archived AS IsArchived, 
donors.is_hidden_web as IsHiddenWeb, 
donors.created_on AS CreatedOn, 
donors.created_by AS CreatedBy, 
donors.last_modified_on AS LastModifiedOn, 
donors.last_modified_by AS LastModifiedBy 
FROM donors 
INNER JOIN individual_pids on individual_pids.donor_id = donors.donor_id
WHERE is_archived = 0 AND individual_pids.pid = @pid";

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                MySqlParameter[] param = new MySqlParameter[1];
                param[0] = new MySqlParameter("@pid", pid);

                MySqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                
                if (dr.Read())
                {
                    
                    donor = new Donor();

                    donor.DonorId = (int)dr["DonorId"];
                    donor.DonorFirstName = (string)dr["DonorFirstName"];
                    donor.DonorMI = dr["DonorMI"].ToString();
                    donor.DonorLastName = (string)dr["DonorLastName"];
                    donor.DonorSuffix = dr["DonorSuffix"].ToString();
                    donor.DonorSSN = dr["DonorSSN"].ToString();
                    donor.DonorDateOfBirth = dr["DonorDateOfBirth"] != DBNull.Value ? Convert.ToDateTime(dr["DonorDateOfBirth"], this.Culture) : DateTime.MinValue;
                    donor.DonorPhone1 = dr["DonorPhone1"].ToString();
                    donor.DonorPhone2 = dr["DonorPhone2"].ToString();
                    donor.DonorAddress1 = dr["DonorAddress1"].ToString();
                    donor.DonorAddress2 = dr["DonorAddress2"].ToString();
                    donor.DonorCity = dr["DonorCity"].ToString();
                    donor.DonorState = dr["DonorState"].ToString();
                    donor.DonorZip = dr["DonorZip"].ToString();
                    donor.DonorEmail = dr["DonorEmail"].ToString();
                    donor.DonorGender = dr["DonorGender"] != DBNull.Value ? (Gender)(Convert.ToInt32(dr["DonorGender"].ToString())) : Gender.None;
                    donor.DonorInitialClientId = dr["DonorInitialClientId"] != null && dr["DonorInitialClientId"].ToString() != string.Empty ? (int)dr["DonorInitialClientId"] : 0;
                    donor.DonorInitialDepartmentId = dr["DonorInitialDepartmentId"] != null && dr["DonorInitialDepartmentId"].ToString() != string.Empty ? (int)dr["DonorInitialDepartmentId"] : 0;
                    donor.DonorRegistrationStatusValue = dr["DonorRegistrationStatus"] != null ? (DonorRegistrationStatus)dr["DonorRegistrationStatus"] : DonorRegistrationStatus.None;
                    donor.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    donor.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    donor.CreatedOn = (DateTime)dr["CreatedOn"];
                    donor.CreatedBy = (string)dr["CreatedBy"];
                    donor.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    donor.LastModifiedBy = (string)dr["LastModifiedBy"];
                }
            }

            if (donor != null) PopulatePids(ref donor);

            return donor;
        }

        public Donor GetByPIDAndEmail(string email, string pid)
        {
            Donor donor = null;

            string sqlQuery = @"
                    SELECT 
donors.donor_id AS DonorId, 
donors.donorClearStarProfId AS DonorClearStarProfId, 
donors.donor_first_name AS DonorFirstName, 
donors.donor_mi AS DonorMI, 
donors.donor_last_name AS DonorLastName, 
donors.donor_suffix AS DonorSuffix, 
donors.donor_ssn AS DonorSSN, 
donors.donor_pid AS donor_pid, 
donors.donor_date_of_birth AS DonorDateOfBirth, 
donors.donor_phone_1 AS DonorPhone1, 
donors.donor_phone_2 AS DonorPhone2, 
donors.donor_address_1 AS DonorAddress1, 
donors.donor_address_2 AS DonorAddress2, 
donors.donor_city AS DonorCity, 
donors.donor_state AS DonorState, 
donors.donor_zip AS DonorZip, 
donors.donor_email AS DonorEmail, 
donors.donor_gender AS DonorGender, 
donors.donor_initial_client_id AS DonorInitialClientId, 
donors.donor_initial_department_id AS DonorInitialDepartmentId, 
donors.donor_registration_status AS DonorRegistrationStatus, 
donors.is_synchronized AS IsSynchronized, 
donors.is_archived AS IsArchived, 
donors.is_hidden_web as IsHiddenWeb, 
donors.created_on AS CreatedOn, 
donors.created_by AS CreatedBy, 
donors.last_modified_on AS LastModifiedOn, 
donors.last_modified_by AS LastModifiedBy 
FROM donors 
INNER JOIN individual_pids on individual_pids.donor_id = donors.donor_id
WHERE is_archived = 0 AND individual_pids.pid = @pid AND donor_email = @Email";

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                MySqlParameter[] param = new MySqlParameter[1];
                ParamHelper p = new ParamHelper();
                p.Param = new MySqlParameter("@pid", pid);
                p.Param = new MySqlParameter("@Email", email);

                MySqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, p.Params);


                if (dr.Read())
                {

                    donor = new Donor();

                    donor.DonorId = (int)dr["DonorId"];
                    donor.DonorFirstName = (string)dr["DonorFirstName"];
                    donor.DonorMI = dr["DonorMI"].ToString();
                    donor.DonorLastName = (string)dr["DonorLastName"];
                    donor.DonorSuffix = dr["DonorSuffix"].ToString();
                    donor.DonorSSN = dr["DonorSSN"].ToString();
                    donor.DonorDateOfBirth = dr["DonorDateOfBirth"] != DBNull.Value ? Convert.ToDateTime(dr["DonorDateOfBirth"], this.Culture) : DateTime.MinValue;
                    donor.DonorPhone1 = dr["DonorPhone1"].ToString();
                    donor.DonorPhone2 = dr["DonorPhone2"].ToString();
                    donor.DonorAddress1 = dr["DonorAddress1"].ToString();
                    donor.DonorAddress2 = dr["DonorAddress2"].ToString();
                    donor.DonorCity = dr["DonorCity"].ToString();
                    donor.DonorState = dr["DonorState"].ToString();
                    donor.DonorZip = dr["DonorZip"].ToString();
                    donor.DonorEmail = dr["DonorEmail"].ToString();
                    donor.DonorGender = dr["DonorGender"] != DBNull.Value ? (Gender)(Convert.ToInt32(dr["DonorGender"].ToString())) : Gender.None;
                    donor.DonorInitialClientId = dr["DonorInitialClientId"] != null && dr["DonorInitialClientId"].ToString() != string.Empty ? (int)dr["DonorInitialClientId"] : 0;
                    donor.DonorInitialDepartmentId = dr["DonorInitialDepartmentId"] != null && dr["DonorInitialDepartmentId"].ToString() != string.Empty ? (int)dr["DonorInitialDepartmentId"] : 0;
                    donor.DonorRegistrationStatusValue = dr["DonorRegistrationStatus"] != null ? (DonorRegistrationStatus)dr["DonorRegistrationStatus"] : DonorRegistrationStatus.None;
                    donor.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    donor.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    donor.CreatedOn = (DateTime)dr["CreatedOn"];
                    donor.CreatedBy = (string)dr["CreatedBy"];
                    donor.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    donor.LastModifiedBy = (string)dr["LastModifiedBy"];
                }
            }
            if (donor != null) PopulatePids(ref donor);
            return donor;
        }
        

        public Donor GetByLoginAndProgram(string donor_email, string department_name)
        {
            Donor donor = null;

            string sqlQuery = @"
                    select 
d.donor_id AS DonorId, 
d.donorClearStarProfId AS DonorClearStarProfId, 
d.donor_first_name AS DonorFirstName, 
d.donor_mi AS DonorMI, 
d.donor_last_name AS DonorLastName, 
d.donor_suffix AS DonorSuffix, 
d.donor_ssn AS DonorSSN, 
d.donor_pid AS donor_pid, 
d.donor_date_of_birth AS DonorDateOfBirth, 
d.donor_phone_1 AS DonorPhone1, 
d.donor_phone_2 AS DonorPhone2, 
d.donor_address_1 AS DonorAddress1, 
d.donor_address_2 AS DonorAddress2, 
d.donor_city AS DonorCity, 
d.donor_state AS DonorState, 
d.donor_zip AS DonorZip, 
d.donor_email AS DonorEmail, 
d.donor_gender AS DonorGender, 
d.donor_initial_client_id AS DonorInitialClientId, 
d.donor_initial_department_id AS DonorInitialDepartmentId, 
d.donor_registration_status AS DonorRegistrationStatus, 
d.is_synchronized AS IsSynchronized, 
d.is_archived AS IsArchived, 
d.is_hidden_web as IsHiddenWeb, 
d.created_on AS CreatedOn, 
d.created_by AS CreatedBy, 
d.last_modified_on AS LastModifiedOn, 
d.last_modified_by AS LastModifiedBy 
from client_departments cd
inner join clients c on c.client_id = cd.client_id
inner join donors d on d.donor_initial_client_id = c.client_id -- and d.donor_initial_department_id = cd.client_department_id 
inner join users u on u.donor_id = d.donor_id
where u.is_archived = 0 and
cd.department_name = @department_name
and d.donor_email = @donor_email
-- and d.
;
";

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                MySqlParameter[] param = new MySqlParameter[1];
                ParamHelper p = new ParamHelper();
                p.Param = new MySqlParameter("@donor_email", donor_email);
                p.Param = new MySqlParameter("@department_name", department_name);

                MySqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, p.Params);


                if (dr.Read())
                {

                    donor = new Donor();

                    donor.DonorId = (int)dr["DonorId"];
                    donor.DonorFirstName = (string)dr["DonorFirstName"];
                    donor.DonorMI = dr["DonorMI"].ToString();
                    donor.DonorLastName = (string)dr["DonorLastName"];
                    donor.DonorSuffix = dr["DonorSuffix"].ToString();
                    donor.DonorSSN = dr["DonorSSN"].ToString();
                    donor.DonorDateOfBirth = dr["DonorDateOfBirth"] != DBNull.Value ? Convert.ToDateTime(dr["DonorDateOfBirth"], this.Culture) : DateTime.MinValue;
                    donor.DonorPhone1 = dr["DonorPhone1"].ToString();
                    donor.DonorPhone2 = dr["DonorPhone2"].ToString();
                    donor.DonorAddress1 = dr["DonorAddress1"].ToString();
                    donor.DonorAddress2 = dr["DonorAddress2"].ToString();
                    donor.DonorCity = dr["DonorCity"].ToString();
                    donor.DonorState = dr["DonorState"].ToString();
                    donor.DonorZip = dr["DonorZip"].ToString();
                    donor.DonorEmail = dr["DonorEmail"].ToString();
                    donor.DonorGender = dr["DonorGender"] != DBNull.Value ? (Gender)(Convert.ToInt32(dr["DonorGender"].ToString())) : Gender.None;
                    donor.DonorInitialClientId = dr["DonorInitialClientId"] != null && dr["DonorInitialClientId"].ToString() != string.Empty ? (int)dr["DonorInitialClientId"] : 0;
                    donor.DonorInitialDepartmentId = dr["DonorInitialDepartmentId"] != null && dr["DonorInitialDepartmentId"].ToString() != string.Empty ? (int)dr["DonorInitialDepartmentId"] : 0;
                    donor.DonorRegistrationStatusValue = dr["DonorRegistrationStatus"] != null ? (DonorRegistrationStatus)dr["DonorRegistrationStatus"] : DonorRegistrationStatus.None;
                    donor.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    donor.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    donor.CreatedOn = (DateTime)dr["CreatedOn"];
                    donor.CreatedBy = (string)dr["CreatedBy"];
                    donor.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    donor.LastModifiedBy = (string)dr["LastModifiedBy"];
                }
            }
            if (donor != null) PopulatePids(ref donor);
            return donor;
        }


        public Donor GetByLoginPIDAndProgram(string donor_email, string pid, string department_name)
        {
            Donor donor = null;

            string sqlQuery = @"
                    select 
d.donor_id AS DonorId, 
d.donorClearStarProfId AS DonorClearStarProfId, 
d.donor_first_name AS DonorFirstName, 
d.donor_mi AS DonorMI, 
d.donor_last_name AS DonorLastName, 
d.donor_suffix AS DonorSuffix, 
d.donor_ssn AS DonorSSN, 
d.donor_pid AS donor_pid, 
d.donor_date_of_birth AS DonorDateOfBirth, 
d.donor_phone_1 AS DonorPhone1, 
d.donor_phone_2 AS DonorPhone2, 
d.donor_address_1 AS DonorAddress1, 
d.donor_address_2 AS DonorAddress2, 
d.donor_city AS DonorCity, 
d.donor_state AS DonorState, 
d.donor_zip AS DonorZip, 
d.donor_email AS DonorEmail, 
d.donor_gender AS DonorGender, 
d.donor_initial_client_id AS DonorInitialClientId, 
d.donor_initial_department_id AS DonorInitialDepartmentId, 
d.donor_registration_status AS DonorRegistrationStatus, 
d.is_synchronized AS IsSynchronized, 
d.is_archived AS IsArchived, 
d.is_hidden_web as IsHiddenWeb, 
d.created_on AS CreatedOn, 
d.created_by AS CreatedBy, 
d.last_modified_on AS LastModifiedOn, 
d.last_modified_by AS LastModifiedBy 
from client_departments cd
inner join clients c on c.client_id = cd.client_id
inner join donors d on d.donor_initial_client_id = c.client_id -- and d.donor_initial_department_id = cd.client_department_id 
inner join users u on u.donor_id = d.donor_id
where u.is_archived = 0 and
cd.department_name = @department_name
and d.donor_email = @donor_email
-- and d.
;
";

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                MySqlParameter[] param = new MySqlParameter[1];
                ParamHelper p = new ParamHelper();
                p.Param = new MySqlParameter("@donor_email", donor_email);
                p.Param = new MySqlParameter("@department_name", department_name);

                MySqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, p.Params);


                if (dr.Read())
                {

                    donor = new Donor();

                    donor.DonorId = (int)dr["DonorId"];
                    donor.DonorFirstName = (string)dr["DonorFirstName"];
                    donor.DonorMI = dr["DonorMI"].ToString();
                    donor.DonorLastName = (string)dr["DonorLastName"];
                    donor.DonorSuffix = dr["DonorSuffix"].ToString();
                    donor.DonorSSN = dr["DonorSSN"].ToString();
                    donor.DonorDateOfBirth = dr["DonorDateOfBirth"] != DBNull.Value ? Convert.ToDateTime(dr["DonorDateOfBirth"], this.Culture) : DateTime.MinValue;
                    donor.DonorPhone1 = dr["DonorPhone1"].ToString();
                    donor.DonorPhone2 = dr["DonorPhone2"].ToString();
                    donor.DonorAddress1 = dr["DonorAddress1"].ToString();
                    donor.DonorAddress2 = dr["DonorAddress2"].ToString();
                    donor.DonorCity = dr["DonorCity"].ToString();
                    donor.DonorState = dr["DonorState"].ToString();
                    donor.DonorZip = dr["DonorZip"].ToString();
                    donor.DonorEmail = dr["DonorEmail"].ToString();
                    donor.DonorGender = dr["DonorGender"] != DBNull.Value ? (Gender)(Convert.ToInt32(dr["DonorGender"].ToString())) : Gender.None;
                    donor.DonorInitialClientId = dr["DonorInitialClientId"] != null && dr["DonorInitialClientId"].ToString() != string.Empty ? (int)dr["DonorInitialClientId"] : 0;
                    donor.DonorInitialDepartmentId = dr["DonorInitialDepartmentId"] != null && dr["DonorInitialDepartmentId"].ToString() != string.Empty ? (int)dr["DonorInitialDepartmentId"] : 0;
                    donor.DonorRegistrationStatusValue = dr["DonorRegistrationStatus"] != null ? (DonorRegistrationStatus)dr["DonorRegistrationStatus"] : DonorRegistrationStatus.None;
                    donor.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    donor.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    donor.CreatedOn = (DateTime)dr["CreatedOn"];
                    donor.CreatedBy = (string)dr["CreatedBy"];
                    donor.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    donor.LastModifiedBy = (string)dr["LastModifiedBy"];
                }
            }
            if (donor != null) PopulatePids(ref donor);
            return donor;
        }


        public DataTable GetList(Dictionary<string, string> searchParam)
        {
            string sqlQuery = "SELECT "
                                + "donor_id AS DonorId, "
                                + "donorClearStarProfId AS DonorClearStarProfId, "
                                + "donor_first_name AS DonorFirstName, "
                                + "donor_mi AS DonorMI, "
                                + "donor_last_name AS DonorLastName, "
                                + "donor_suffix AS DonorSuffix, "
                                + "donor_ssn AS DonorSSN, "
                                + "donor_pid AS donor_pid, "
                                + "donor_date_of_birth AS DonorDateOfBirth, "
                                + "donor_phone_1 AS DonorPhone1, "
                                + "donor_phone_2 AS DonorPhone2, "
                                + "donor_address_1 AS DonorAddress1, "
                                + "donor_address_2 AS DonorAddress2, "
                                + "donor_city AS DonorCity, "
                                + "donor_state AS DonorState, "
                                + "donor_zip AS DonorZip, "
                                + "donor_email AS DonorEmail, "
                                + "donor_gender AS DonorGender, "
                                + "donor_initial_client_id AS DonorInitialClientId, "
                                + "donor_initial_department_id AS DonorInitialDepartmentId, "
                                + "donor_registration_status AS DonorRegistrationStatus, "
                                + "is_synchronized AS IsSynchronized, "
                                + "is_archived AS IsArchived, "
                                + "created_on AS CreatedOn, "
                                + "created_by AS CreatedBy, "
                                + "last_modified_on AS LastModifiedOn, "
                                + "last_modified_by AS LastModifiedBy "
                                + "FROM donors WHERE is_archived =0 ";

            List<MySqlParameter> param = new List<MySqlParameter>();
            // bool isInActiveFlag = false;

            foreach (KeyValuePair<string, string> searchItem in searchParam)
            {
                if (searchItem.Key == "GeneralSearch")
                {
                    sqlQuery += " AND (donor_first_name LIKE @SearchKeyword OR donor_mi LIKE @SearchKeyword OR donor_suffix LIKE @SearchKeyword OR donor_last_name LIKE @SearchKeyword OR donor_ssn LIKE @SearchKeyword OR donor_date_of_birth LIKE @SearchKeyword OR donor_address_1 LIKE @SearchKeyword OR donor_address_2 LIKE @SearchKeyword OR donor_city LIKE @SearchKeyword OR donor_state LIKE @SearchKeyword OR donor_zip LIKE @SearchKeyword OR donor_phone_1 LIKE @SearchKeyword OR donor_phone_2 LIKE @SearchKeyword OR donor_email LIKE @SearchKeyword) ";

                    param.Add(new MySqlParameter("@SearchKeyword", searchItem.Value));
                }
            }

            sqlQuery += " ORDER BY donor_first_name";

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

        public DataTable GetDonorsByAttorney(int attorneyId)
        {
            string sqlQuery = "SELECT "
                            + "donors.donor_id AS DonorId, "
                            + "donorClearStarProfId AS DonorClearStarProfId, "
                            + "donors.donor_first_name AS DonorFirstName, "
                            + "donors.donor_last_name AS DonorLastName, "
                            + "donors.donor_ssn AS DonorSSN, "
                            + "donors.donor_pid AS donor_pid, "
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
                            + "donor_test_info.is_instant_test AS IsInstantTest, donor_test_info.instant_test_result As InstanTestResult, "
                            + "donor_test_info.is_donor_refused AS IsDonorRefused, "
                            + "donor_test_info.test_status AS TestStatus, "
                            + "donor_test_info.test_overall_result AS TestOverallResult, "
                            + "CONCAT_WS(' ', users.user_first_name, users.user_last_name) AS CollectorName, "
                            //+ "donor_test_info_test_categories.test_category_id AS TestCategoryId, "
                            //+ "donor_test_info_test_categories.test_panel_id AS TestPanelId, "
                            //+ "donor_test_info_test_categories.specimen_id AS SpecimenId, "
                            + "(select GROUP_CONCAT(donor_test_info_test_categories.specimen_id SEPARATOR ', ') from donor_test_info_test_categories where donor_test_info_test_categories.donor_test_info_id = donor_test_info.donor_test_info_id) AS SpecimenId, "
                            + "donor_test_info.screening_time AS SpecimenDate, "
                            //+ "donor_test_info_test_categories.hair_test_panel_days AS HairTestPanelDays, "
                            + "clients.client_name AS ClientName, "
                            + "client_departments.department_name AS ClientDepartmentName, "
                            + "client_departments.clearstarcode As ClearStarCode "
                            + "FROM donor_test_info "
                            + "INNER JOIN donor_test_info_attorneys ON donor_test_info_attorneys.donor_test_info_id = donor_test_info.donor_test_info_id "
                            + "INNER JOIN donors ON donor_test_info.donor_id = donors.donor_id "
                            //+ "LEFT OUTER JOIN donor_test_info_test_categories ON donor_test_info.donor_test_info_id = donor_test_info_test_categories.donor_test_info_id "
                            // + "LEFT OUTER JOIN clients ON ((donors.donor_initial_client_id = clients.client_id AND donors.donor_initial_department_id IS NULL) OR donor_test_info.client_id = clients.client_id) "
                            + "LEFT OUTER JOIN clients ON donor_test_info.client_id = clients.client_id "
                            + "LEFT OUTER JOIN client_departments ON donor_test_info.client_department_id = client_departments.client_department_id "
                            + "LEFT OUTER JOIN users ON donor_test_info.collection_site_user_id = users.user_id "
                            + "WHERE test_status >3 AND donor_test_info_attorneys.attorney_id = @AttornyeId ";

            MySqlParameter[] param = new MySqlParameter[1];
            param[0] = new MySqlParameter("@AttornyeId", attorneyId);

            sqlQuery += "ORDER BY donors.donor_first_name, donors.donor_last_name";

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

        public int GetDonorIDByDetails(string donor_first_name, string donor_last_name, string donor_ssn, string donor_date_of_birth)
        {
            int retval = 0;

            string sqlQuery = @"
                    select max(donor_id) from donors where
                    donor_first_name = @donor_first_name and
                    donor_last_name = @donor_last_name and
                    donor_ssn = @donor_ssn and
                    donor_date_of_birth = date(@donor_date_of_birth);
                    ";

            List<MySqlParameter> mySqlParameters = new List<MySqlParameter>();
            mySqlParameters.Add(new MySqlParameter("@donor_first_name", donor_first_name));
            mySqlParameters.Add(new MySqlParameter("@donor_last_name", donor_last_name));
            mySqlParameters.Add(new MySqlParameter("@donor_ssn", donor_ssn));
            mySqlParameters.Add(new MySqlParameter("@donor_date_of_birth", donor_date_of_birth));

            //MySqlParameter[] param = new MySqlParameter[1];
            //param[0] = new MySqlParameter("@donor_first_name", donor_first_name);
            //param[1] = new MySqlParameter("@donor_last_name", donor_last_name);
            //param[2] = new MySqlParameter("@donor_ssn", donor_ssn);
            //param[3] = new MySqlParameter("@donor_date_of_birth", donor_date_of_birth);

            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sqlQuery, mySqlParameters.ToArray());

            if (ds.Tables.Count > 0)
            {
                retval = (int)ds.Tables[0].Rows[0][0];
            }
            return retval;
        }

        public DataTable GetDonorsByCourt(int courtId)
        {
            string sqlQuery = "SELECT "
                            + "donors.donor_id AS DonorId, "
                            + "donorClearStarProfId AS DonorClearStarProfId, "
                            + "donors.donor_first_name AS DonorFirstName, "
                            + "donors.donor_last_name AS DonorLastName, "
                            + "donors.donor_ssn AS DonorSSN, "
                            + "donors.donor_pid AS donor_pid, "
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
                            + "donor_test_info.is_instant_test AS IsInstantTest, donor_test_info.instant_test_result As InstanTestResult, "
                            + "donor_test_info.is_donor_refused AS IsDonorRefused, "
                            + "donor_test_info.test_status AS TestStatus, "
                            + "donor_test_info.test_overall_result AS TestOverallResult, "
                            + "CONCAT_WS(' ', users.user_first_name, users.user_last_name) AS CollectorName, "
                            //+ "donor_test_info_test_categories.test_category_id AS TestCategoryId, "
                            //+ "donor_test_info_test_categories.test_panel_id AS TestPanelId, "
                            //+ "donor_test_info_test_categories.specimen_id AS SpecimenId, "
                            + "(select GROUP_CONCAT(donor_test_info_test_categories.specimen_id SEPARATOR ', ') from donor_test_info_test_categories where donor_test_info_test_categories.donor_test_info_id = donor_test_info.donor_test_info_id) AS SpecimenId, "
                            + "donor_test_info.screening_time AS SpecimenDate, "
                            //+ "donor_test_info_test_categories.hair_test_panel_days AS HairTestPanelDays, "
                            + "clients.client_name AS ClientName, "
                            + "client_departments.department_name AS ClientDepartmentName, "
                            + "client_departments.clearstarcode As ClearStarCode "
                            + "FROM donor_test_info "
                            + "INNER JOIN donors ON donor_test_info.donor_id = donors.donor_id "
                            //+ "LEFT OUTER JOIN donor_test_info_test_categories ON donor_test_info.donor_test_info_id = donor_test_info_test_categories.donor_test_info_id "
                            //+ "LEFT OUTER JOIN clients ON ((donors.donor_initial_client_id = clients.client_id AND donors.donor_initial_department_id IS NULL) OR donor_test_info.client_id = clients.client_id) "
                            + "LEFT OUTER JOIN clients ON donor_test_info.client_id = clients.client_id "
                            + "LEFT OUTER JOIN client_departments ON donor_test_info.client_department_id = client_departments.client_department_id "
                            + "LEFT OUTER JOIN users ON donor_test_info.collection_site_user_id = users.user_id "
                            + "WHERE test_status >3 AND donor_test_info.court_id = @CourtId ";

            MySqlParameter[] param = new MySqlParameter[1];
            param[0] = new MySqlParameter("@CourtId", courtId);

            sqlQuery += "ORDER BY donors.donor_first_name, donors.donor_last_name";

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

        public DataTable GetDonorsByJudge(int judgeId)
        {
            string sqlQuery = "SELECT "
                            + "donors.donor_id AS DonorId, "
                            + "donorClearStarProfId AS DonorClearStarProfId, "
                            + "donors.donor_first_name AS DonorFirstName, "
                            + "donors.donor_last_name AS DonorLastName, "
                            + "donors.donor_ssn AS DonorSSN, "
                            + "donors.donor_pid AS donor_pid, "
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
                            + "donor_test_info.test_status AS TestStatus, "
                            + "donor_test_info.test_overall_result AS TestOverallResult, "
                            + "donor_test_info.is_instant_test AS IsInstantTest, donor_test_info.instant_test_result As InstanTestResult, "
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
                             + "client_departments.clearstarcode As ClearStarCode "
                            + "FROM donor_test_info "
                            + "INNER JOIN donors ON donor_test_info.donor_id = donors.donor_id "
                            //+ "LEFT OUTER JOIN donor_test_info_test_categories ON donor_test_info.donor_test_info_id = donor_test_info_test_categories.donor_test_info_id "
                            //+ "LEFT OUTER JOIN clients ON ((donors.donor_initial_client_id = clients.client_id AND donors.donor_initial_department_id IS NULL) OR donor_test_info.client_id = clients.client_id) "
                            + "LEFT OUTER JOIN clients ON donor_test_info.client_id = clients.client_id "
                            + "LEFT OUTER JOIN client_departments ON donor_test_info.client_department_id = client_departments.client_department_id "
                            + "LEFT OUTER JOIN users ON donor_test_info.collection_site_user_id = users.user_id "
                            + "WHERE test_status >3 AND donor_test_info.judge_id = @JudgeId ";

            MySqlParameter[] param = new MySqlParameter[1];
            param[0] = new MySqlParameter("@JudgeId", judgeId);

            sqlQuery += "ORDER BY donors.donor_first_name, donors.donor_last_name";

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

        public DataTable GetDonorActivityList(int donorTestInfoId)
        {
            //string sqlQuery = "SELECT "
            //                    + "donor_test_activity.donor_test_activity_id AS DonorTestActivityId, "
            //                    + "donor_test_activity.donor_test_info_id AS DonorTestInfoId, "
            //                    + "donor_test_activity.activity_datetime AS ActivityDateTime, "
            //                    + "donor_test_activity.activity_user_id AS ActivityUserId, "
            //                    + "donor_test_activity.activity_category_id AS ActivityCategoryId, "
            //                    + "donor_test_activity.is_activity_visible AS IsActivityVisible, "
            //                    + "donor_test_activity.activity_note AS ActivityNote, "
            //                    + "donor_test_activity.is_synchronized AS IsSynchronized, "
            //                    + "concat_ws(' ', users.user_first_name, users.user_last_name) AS ActivityUserName "
            //                    + "FROM donor_test_activity "
            //                    + "INNER JOIN users ON donor_test_activity.activity_user_id = users.user_id "
            //                    + "WHERE donor_test_info_id = @DonorTestInfoId ORDER BY donor_test_activity.donor_test_activity_id";

            string sqlQuery = @"SELECT
                donor_test_activity.donor_test_activity_id AS DonorTestActivityId,
                donor_test_activity.donor_test_info_id AS DonorTestInfoId,
                donor_test_activity.activity_datetime AS ActivityDateTime,
                donor_test_activity.activity_user_id AS ActivityUserId,
                donor_test_activity.activity_category_id AS ActivityCategoryId,
                donor_test_activity.is_activity_visible AS IsActivityVisible,
                donor_test_activity.activity_note AS ActivityNote,
                donor_test_activity.is_synchronized AS IsSynchronized,
                if (donor_test_activity.activity_user_id=0,""SYSTEM"",concat_ws(' ', users.user_first_name, users.user_last_name)) AS ActivityUserName
                -- concat_ws(' ', users.user_first_name, users.user_last_name) AS ActivityUserName
                FROM donor_test_activity
                left outer join users ON donor_test_activity.activity_user_id = users.user_id
                WHERE donor_test_info_id =  @DonorTestInfoId
                ORDER BY donor_test_activity.donor_test_activity_id;";

            MySqlParameter[] param = new MySqlParameter[1];

            param[0] = new MySqlParameter("@DonorTestInfoId", donorTestInfoId);

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

        public DataTable GetTestInfoReverseEntryList(int donorId, int donorTestInfoId)
        {
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
                        + "is_bc as IsBC, "
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
                        + "is_reverse_entry AS ISReverseEntry,"
                        + "is_synchronized AS IsSynchronized, "
                        + "created_on AS CreatedOn, "
                        + "created_by AS CreatedBy, "
                        + "last_modified_on AS LastModifiedOn, "
                        + "last_modified_by AS LastModifiedBy "
                        + "FROM donor_test_info "
                        + "WHERE is_reverse_entry= 1 and donor_test_info_id = @DonorTestInfoId and donor_id = @DonorId";

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                MySqlParameter[] param = new MySqlParameter[2];
                param[0] = new MySqlParameter("@donor_test_info_id", donorTestInfoId);
                param[1] = new MySqlParameter("@DonorId", donorId);
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

        public DataTable GetMoreThanTestInfoList(int donorId)
        {
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
                               + "WHERE donor_id = @DonorId";
            MySqlParameter[] param = new MySqlParameter[1];

            param[0] = new MySqlParameter("@DonorId", donorId);

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

        public DataTable GetDonorDocumentList(int donorId, bool HideFilesFromSystem = true)
        {
            string sqlQuery = "SELECT "
                            + "donor_document_id AS DonorDocumentId, "
                            + "donor_id AS DonorId, "
                            + "document_upload_time AS DocumentUploadTime, "
                            + "dateRequired AS DateRequired, "
                            + "document_title AS DocumentTitle, "
                            + "source AS Source, "
                            + "uploaded_by AS UploadedBy, "
                            + "is_Notify As isNotify, "
                            + "lastNotified As LastNotified, "
                            + "is_needsApproval As isNeedsApproval, "
                            + "is_Approved As isApproved, "
                            + "is_rejected As isRejected, "
                            + "is_updateable As isUpdateable, "
                            + "is_Archived As isArchived, "
                            + "file_name AS FileName "
                            + "FROM donor_documents "
                            + "WHERE donor_id = @DonorId ";
            if (HideFilesFromSystem == true)
            {
                sqlQuery += "AND Uploaded_by != 'SYSTEM' ";
            }

            sqlQuery += "ORDER BY document_title";

            MySqlParameter[] param = new MySqlParameter[1];

            param[0] = new MySqlParameter("@DonorId", donorId);

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

        public DataTable GetDonorDocumentTypes(int clientDepartmentId)
        {
            string sqlQuery = "select * from client_dept_doctypes where client_department_id = @ClientDepartmentId  and is_archived = 0 order by  duedate, description";

            MySqlParameter[] param = new MySqlParameter[1];

            param[0] = new MySqlParameter("@ClientDepartmentId", clientDepartmentId);

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

        public DonorDocument GetDonorDocument(int donorDocumentId, bool excludeBySystem = true)
        {
            DonorDocument donorDocument = null;
            string _xBySys = (excludeBySystem == true) ? "1" : "0";
            string sqlQuery = "SELECT "
                            + "donor_document_id AS DonorDocumentId, "
                            + "donor_id AS DonorId, "
                            + "document_upload_time AS DocumentUploadTime, "
                            + "document_title AS DocumentTitle, "
                            + "document_content AS DocumentContent, "
                            + "source AS Source, "
                            + "uploaded_by AS UploadedBy, "
                            + "file_name AS FileName, "
                            + "is_synchronized AS IsSynchronized "
                            + "FROM donor_documents "
                            + "WHERE donor_document_id = @DonorDocumentId "
                            //    + $"AND (Uploaded_by != 'SYSTEM') = {_xBySys};"
                            + $@"AND (
(uploaded_by = 'SYSTEM' AND 0 = {_xBySys})
OR
(uploaded_by != 'SYSTEM')
)";

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                MySqlParameter[] param = new MySqlParameter[1];
                param[0] = new MySqlParameter("@DonorDocumentId", donorDocumentId);

                MySqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                if (dr.Read())
                {
                    donorDocument = new DonorDocument();

                    donorDocument.DonorDocumentId = (int)dr["DonorDocumentId"];
                    donorDocument.DonorId = (int)dr["DonorId"];
                    donorDocument.DocumentUploadTime = (DateTime)dr["DocumentUploadTime"];
                    donorDocument.DocumentTitle = dr["DocumentTitle"].ToString();
                    donorDocument.DocumentContent = (byte[])(dr["DocumentContent"]);
                    donorDocument.Source = dr["Source"].ToString();
                    donorDocument.UploadedBy = dr["UploadedBy"].ToString();
                    donorDocument.FileName = dr["FileName"].ToString();
                    donorDocument.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                }
            }

            return donorDocument;
        }

        public DataTable SearchDonorByClient(Dictionary<string, string> searchParam, UserType userType, int currentUserId)
        {
            string sqlQuery = "SELECT "
                            + "donors.donor_id AS DonorId, "
                            + "donorClearStarProfId AS DonorClearStarProfId, "
                            + "donors.donor_first_name AS DonorFirstName, "
                            + "donors.donor_last_name AS DonorLastName, "
                            + "donors.donor_ssn AS DonorSSN, "
                            + "donors.donor_pid AS donor_pid, "
                            + "donors.donor_date_of_birth AS DonorDateOfBirth, "
                            + "donors.donor_initial_client_id AS DonorInitialClientId, "
                            + "donors.donor_registration_status AS DonorRegistrationStatus, "
                            + "donors.donor_city as DonorCity, "
                            + "donors.donor_email as DonorEmail, "
                            + "donors.donor_zip as DonorZipCode, "
                            + "donors.is_hidden_web as IsHiddenWeb, "
                            + "donors.is_archived AS IsDonorArchived, "
                            + "donor_test_info.donor_test_info_id AS DonorTestInfoId, "
                            + "donor_test_info.client_id AS TestInfoClientId, "
                            + "donor_test_info.client_department_id AS TestInfoDepartmentId, "
                            + "donor_test_info.mro_type_id AS MROTypeId, "
                            + "donor_test_info.payment_type_id AS PaymentTypeId, "
                            + "donor_test_info.test_requested_date AS TestRequestedDate, "
                            + "donor_test_info.reason_for_test_id AS ReasonForTestId, "
                            + "donor_test_info.is_walkin_donor AS IsWalkinDonor, "
                            + "donor_test_info.instant_test_result AS InstantTestResult, "
                            + "donor_test_info.is_instant_test AS IsInstantTest, "
                            + "donor_test_info.is_donor_refused AS IsDonorRefused, "
                            + "donor_test_info.total_payment_amount AS TotalPaymentAmount, "
                            + "donor_test_info.payment_method_id AS PaymentMethodId, "
                            + "donor_test_info.test_status AS TestStatus, "
                            + "donor_test_info.test_overall_result AS TestOverallResult, "
                            + "CONCAT_WS(' ', users.user_first_name, users.user_last_name) AS CollectorName, "
                            + "GROUP_CONCAT(donor_test_info_test_categories.test_category_id) AS TestCategoryId, "
                            //+ "donor_test_info_test_categories.test_panel_id AS TestPanelId, "
                            //+ "donor_test_info_test_categories.specimen_id AS SpecimenId, "
                            + "(select GROUP_CONCAT(donor_test_info_test_categories.specimen_id SEPARATOR ', ') from donor_test_info_test_categories where donor_test_info_test_categories.donor_test_info_id = donor_test_info.donor_test_info_id) AS SpecimenId, "
                            + "donor_test_info.screening_time AS SpecimenDate, "
                           //+ "donor_test_info_test_categories.hair_test_panel_days AS HairTestPanelDays, "
                           + "clients.client_name AS ClientName, "
                            + "client_departments.department_name AS ClientDepartmentName, "
                            + "client_departments.clearstarcode AS ClearStarCode, "
                            + "if (bn.notified_by_email is null, 'Old Data',IF (bn.notified_by_email = 0, 'Not Notified',bn.notified_by_email_timestamp)) as Notified_by_email_timestamp, "
                            + "if (bn.backend_notifications_id is null, 0, bn.backend_notifications_id) as backend_notifications_id, "
                            + "if (bnwd.show_web_notify_button is null, 0, bnwd.show_web_notify_button) as show_web_notify_button, "
                             + "dt.DocsTotal, dt.DocsNotApproved, dt.DocsRejected "
                            + "FROM donors "
                //+ "LEFT OUTER JOIN donor_test_info ON donors.donor_id = donor_test_info.donor_id AND donor_test_info.donor_test_info_id = (SELECT max(donor_test_info.donor_test_info_id) FROM donor_test_info WHERE donor_test_info.donor_id = donors.donor_id) "
                + "LEFT OUTER JOIN donor_test_info ON donors.donor_id = donor_test_info.donor_id "
                + "LEFT OUTER JOIN donor_test_info_test_categories ON donor_test_info.donor_test_info_id = donor_test_info_test_categories.donor_test_info_id "
                + "LEFT OUTER JOIN clients ON (donor_test_info.client_id = clients.client_id) "
                            //+ "LEFT OUTER JOIN clients ON ((donors.donor_initial_client_id = clients.client_id AND donors.donor_initial_department_id IS NULL) OR donor_test_info.client_id = clients.client_id) "
                            //+ "LEFT OUTER JOIN clients ON donors.donor_initial_client_id = clients.client_id AND donors.donor_initial_department_id IS NULL "
                            + "LEFT OUTER JOIN client_departments ON donor_test_info.client_department_id = client_departments.client_department_id "
                            + "LEFT OUTER JOIN users ON donor_test_info.collection_site_user_id = users.user_id "
                            + "LEFT OUTER JOIN donordocumenttotals dt on dt.donor_id = donor_test_info.donor_id "
                            + "LEFT OUTER JOIN backend_notifications bn on bn.donor_test_info_id = donor_test_info.donor_test_info_id "
                            + "left outer join backend_notification_window_data bnwd on clients.client_id = bnwd.client_id and client_departments.client_department_id = bnwd.client_department_id "
                            + "WHERE 1=1 AND test_status >2 ";

            List<MySqlParameter> param = new List<MySqlParameter>();

            foreach (KeyValuePair<string, string> searchItem in searchParam)
            {
                if (searchItem.Key == "DonorId")
                {
                    sqlQuery += "AND donors.donor_id = @DonorId ";
                    param.Add(new MySqlParameter("@DonorId", searchItem.Value));
                }
                else if (searchItem.Key == "DonorTestInfoId")
                {
                    sqlQuery += "AND donor_test_info.donor_test_info_id = @DonorTestInfoId ";
                    param.Add(new MySqlParameter("@DonorTestInfoId", searchItem.Value));
                }
                else if (searchItem.Key == "FirstName")
                {
                    sqlQuery += "AND donors.donor_first_name LIKE @DonorFirstName ";
                    param.Add(new MySqlParameter("@DonorFirstName", searchItem.Value));
                }
                else if (searchItem.Key == "LastName")
                {
                    sqlQuery += "AND donors.donor_last_name LIKE @DonorLastName ";
                    param.Add(new MySqlParameter("@DonorLastName", searchItem.Value));
                }
                else if (searchItem.Key == "SSN")
                {
                    sqlQuery += "AND donors.donor_ssn LIKE @DonorSSN ";
                    param.Add(new MySqlParameter("@DonorSSN", searchItem.Value));
                }
                else if (searchItem.Key == "DOB")
                {
                    sqlQuery += "AND CAST(donors.donor_date_of_birth AS DATE) = CAST(@DonorDateOfBirth AS DATE) ";
                    param.Add(new MySqlParameter("@DonorDateOfBirth", Convert.ToDateTime(searchItem.Value).ToString("yyyy-MM-dd")));
                }
                else if (searchItem.Key == "City")
                {
                    sqlQuery += "AND donors.donor_city LIKE @DonorCity ";
                    param.Add(new MySqlParameter("@DonorCity", searchItem.Value));
                }
                else if (searchItem.Key == "ZipCode")
                {
                    sqlQuery += "AND donors.donor_zip = @DonorZipCode ";
                    param.Add(new MySqlParameter("@DonorZipCode", searchItem.Value));
                }
                else if (searchItem.Key == "DonorEmail")
                {
                    sqlQuery += "AND donors.donor_email = @DonorEmail ";
                    param.Add(new MySqlParameter("@DonorEmail", searchItem.Value));
                }
                else if (searchItem.Key == "SpecimenId")
                {
                    sqlQuery += "AND donor_test_info.donor_test_info_id IN (SELECT donor_test_info_test_categories.donor_test_info_id FROM donor_test_info_test_categories WHERE donor_test_info_test_categories.specimen_id LIKE @SpecimenId) ";
                    param.Add(new MySqlParameter("@SpecimenId", searchItem.Value));
                }
                else if (searchItem.Key == "TestReason")
                {
                    sqlQuery += "AND donor_test_info.reason_for_test_id = @ReasonForTestId ";
                    param.Add(new MySqlParameter("@ReasonForTestId", Convert.ToInt32(searchItem.Value)));
                }
                else if (searchItem.Key == "Client")
                {
                    sqlQuery += "AND (donors.donor_initial_client_id = @ClientId OR donor_test_info.client_id = @ClientId) ";
                    param.Add(new MySqlParameter("@ClientId", Convert.ToInt32(searchItem.Value)));
                }
                else if (searchItem.Key == "Department")
                {
                    sqlQuery += "AND donor_test_info.client_department_id = @ClientDepartmentId ";
                    param.Add(new MySqlParameter("@ClientDepartmentId", Convert.ToInt32(searchItem.Value)));
                }
                else if (searchItem.Key == "Status")
                {
                    if (searchItem.Value == "1" || searchItem.Value == "2")
                    {
                        sqlQuery += "AND donors.donor_registration_status = @Status ";
                    }
                    else
                    {
                        sqlQuery += "AND donor_test_info.test_status = @Status ";
                    }
                    param.Add(new MySqlParameter("@Status", Convert.ToInt32(searchItem.Value)));
                }
                else if (searchItem.Key == "TestCategory")
                {
                    sqlQuery += "AND donor_test_info.donor_test_info_id IN (SELECT donor_test_info_test_categories.donor_test_info_id FROM donor_test_info_test_categories WHERE donor_test_info_test_categories.test_category_id = @TestCategoryId) ";
                    param.Add(new MySqlParameter("@TestCategoryId", Convert.ToInt32(searchItem.Value)));
                }
                else if (searchItem.Key == "TestResult")
                {
                    sqlQuery += "AND donor_test_info.test_overall_result = @ResultForTest ";
                    param.Add(new MySqlParameter("@ResultForTest", Convert.ToInt32(searchItem.Value)));
                }
                else if (searchItem.Key == "IncludeArchive")
                {
                    if (!Convert.ToBoolean(searchItem.Value))
                    {
                        sqlQuery += "AND donors.is_archived = b'0' ";
                    }
                }
                else if (searchItem.Key == "NoOfDays")
                {
                    if (!searchItem.Value.StartsWith("DR"))
                    {
                        int dateInterval = Convert.ToInt32(searchItem.Value) * -1;

                        sqlQuery += "AND donor_test_info.screening_time >= DATE_ADD(CURDATE(), INTERVAL @DateInterval DAY) ";
                        param.Add(new MySqlParameter("@DateInterval", dateInterval));
                    }
                    else
                    {
                        sqlQuery += "AND (CAST(donor_test_info.screening_time AS DATE) BETWEEN CAST(@StartDate AS DATE) AND CAST(@EndDate AS DATE)) ";
                        string[] dates = searchItem.Value.Split('#');
                        param.Add(new MySqlParameter("@StartDate", Convert.ToDateTime(dates[1]).ToString("yyyy-MM-dd")));
                        param.Add(new MySqlParameter("@EndDate", Convert.ToDateTime(dates[2]).ToString("yyyy-MM-dd")));
                    }
                }
                else if (searchItem.Key == "DonorSearchFilterList")
                {
                    int.TryParse(searchItem.Value, out int _DonorSearchFilterList);
                    if (DonorSearchFilterList.None != (DonorSearchFilterList)_DonorSearchFilterList)
                    {

                        DateTime.TryParse(searchParam["afterDateFilter"], out DateTime __dtAfterFilter);
                        DateTime.TryParse(searchParam["beforeDateFilter"], out DateTime __dtBeforeFilter);

                        var _dtAfterFilter = __dtAfterFilter.ToString("yyyy-MM-dd");
                        var _dtBeforeFilter = __dtBeforeFilter.ToString("yyyy-MM-dd");
                        if (DonorSearchFilterList.Registration == (DonorSearchFilterList)_DonorSearchFilterList)
                        {
                            sqlQuery += $"AND(donor_test_info.test_requested_date between '{_dtAfterFilter}' and '{_dtBeforeFilter}') ";

                        }
                        else if (DonorSearchFilterList.Testing == (DonorSearchFilterList)_DonorSearchFilterList)
                        {
                            sqlQuery += $"AND(donor_test_info.screening_time between '{_dtAfterFilter}' and '{_dtBeforeFilter}') ";
                        }
                    }
                }
            }

            if ((userType == UserType.TPA && currentUserId != 1) && userType != UserType.Donor)
            {
                sqlQuery += "AND donor_test_info.client_department_id IN (select client_department_id from user_departments where user_id = " + currentUserId.ToString() + ")";
            }

            if (userType == UserType.Client && currentUserId != 1)
            {
                sqlQuery += "AND  client_departments.client_department_id IN (select client_department_id from user_departments where user_id = " + currentUserId.ToString() + ")";
            }

            //sqlQuery += "ORDER BY donors.donor_first_name, donors.donor_last_name";
            sqlQuery += "group by donor_test_info.donor_test_info_id";
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

        public DataTable GetInActiveUser(string UserName, string Password)
        {
            string sqlQuery = "select user_id AS UserId ,user_name AS UserName ,user_password AS PassWord ,user_first_name AS FirstName , user_last_name AS LastName ,is_user_active AS IsActive from users where is_user_active='0' and user_name=@UserName";

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                MySqlParameter[] param = new MySqlParameter[1];
                param[0] = new MySqlParameter("@UserName", UserName);
                // param[1] = new MySqlParameter("@PassWord", Password);

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
        }

        public DataTable GetInActiveUserWithPW(string UserName, string Password)
        {
            string sqlQuery = "select user_id AS UserId ,user_name AS UserName ,user_password AS PassWord ,user_first_name AS FirstName , user_last_name AS LastName ,is_user_active AS IsActive from users where is_user_active='0' and user_name=@UserName";

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                MySqlParameter[] param = new MySqlParameter[2];
                param[0] = new MySqlParameter("@UserName", UserName);
                param[1] = new MySqlParameter("@PassWord", Password);

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
        }
    }
}