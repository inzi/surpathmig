using MySql.Data.MySqlClient;
using Serilog;
using SurPath.Entity;
using SurPath.Enum;
using SurPath.MySQLHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace SurPath.Data
{
    public partial class HL7ParserDao : DataObject
    {

        public static ILogger _logger { get; set; }

        public HL7ParserDao(ILogger __logger = null)
        {
            if (__logger == null)
            {
                _logger = new LoggerConfiguration().CreateLogger();
            }
            else
            {
                _logger = __logger;
            }
        }

        private void AddPidForNewDonorID(int donor_id, string PID, int pid_type_id, MySqlTransaction trans)
        {
            try
            {
                if (PID.Equals("N/S", StringComparison.InvariantCultureIgnoreCase)) return;
                if (PID.Equals("ILLEGIBLE", StringComparison.InvariantCultureIgnoreCase)) return;
                if (string.IsNullOrEmpty(PID)) return;
                if (donor_id < 1) return;
                if (pid_type_id < 1) return; // don't insert invalid PIDs


                int existingRowID = Convert.ToInt32(
                        SqlHelper.ExecuteScalar(
                            trans,
                            CommandType.Text,
                            "select individual_pid_id from individual_pids where donor_id=@donor_id AND pid_type_id = @pid_type_id;",
                              new List<MySqlParameter>()
                                {
                               new MySqlParameter("@donor_id", donor_id),
                               new MySqlParameter("@pid_type_id", pid_type_id),
                                }.ToArray()
                            )
                        );

                if (existingRowID < 1)
                {// insert
                    SqlHelper.ExecuteNonQuery(
                        trans,
                        CommandType.Text,
                        @"INSERT INTO individual_pids(
                           donor_id
                          ,pid
                          ,pid_type_id
                          ,mask_pid
                          ,validated
                          ,individual_pid_type_description
                        ) VALUES (
                           @donor_id -- donor_id - IN int(11)
                          ,@pid -- pid - IN varchar(255)
                          ,@pid_type_id -- pid_type_id - IN varchar(10)
                          ,@mask_pid
                          ,@validated
                          ,@pid_type_name -- individual_pid_type_description - IN varchar(1024)
                        )",
                            new List<MySqlParameter>()
                            {
                           new MySqlParameter("@donor_id", donor_id),
                            new MySqlParameter("@pid", PID),
                            new MySqlParameter("@pid_type_id", pid_type_id),
                            new MySqlParameter("@pid_type_name", ((PidTypes)pid_type_id).ToString() ),
                            new MySqlParameter("@mask_pid", pid_type_id==(int)PidTypes.SSN? 1 :0 ),
                            new MySqlParameter("@validated",  false ),
                            }.ToArray()
                        );
                }
                //else
                //{// update
                //    SqlHelper.ExecuteNonQuery(
                //        trans,
                //        CommandType.Text,
                //        "update individual_pids set donor_id = @donor_id, pid = @pid, pid_type_id = @pid_type_id WHE;",
                //            new List<MySqlParameter>()
                //            {
                //               new MySqlParameter("@donor_id", donor_id),
                //                new MySqlParameter("@pid", PID),
                //                new MySqlParameter("@pid_type_id", pid_type_id),
                //            }.ToArray()
                //        );

                //}
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                _logger.Error(ex.InnerException.ToString());
                throw;
            }
        }

        private void AddDonorInfoWithTestDetails(ReportInfo reportDetails, ReturnValues returnValues, MySqlTransaction trans, bool _SetDonorAsArchived = false, bool _AddTestInfo = true)
        {
            #region SQLQuery InsertIntoDonors

            int _IsArchived = 0;
            if (_SetDonorAsArchived) _IsArchived = 1;
            int _Unverified = _IsArchived;

            string sqlQuery = "INSERT INTO donors ("
                                        + "donor_first_name, "
                                        + "donor_mi, "
                                        + "donor_last_name, "
                                        + "donor_ssn, "
                                        + "donor_pid, "
                                        + "donor_pid_type_id, "
                                        + "donor_date_of_birth, "
                                        + "donor_gender, "
                                        + "donor_initial_client_id, "
                                        + "donor_initial_department_id, "
                                        + "donor_registration_status, "
                                        + "is_synchronized, "
                                        + "is_archived, "
                                        + "unverified, "
                                        + "created_on, "
                                        + "created_by, "
                                        + "last_modified_on, "
                                        + "last_modified_by) VALUES ("
                                        + "@DonorFirstName, "
                                        + "@DonorMI, "
                                        + "@DonorLastName, "
                                        + "@DonorSSN, "
                                        + "@donor_pid, "
                                        + "@donor_pid_type_id, "
                                        + "@DonorDateOfBirth, "
                                        + "@DonorGender, "
                                        + "@DonorInitialClientId, "
                                        + "@DonorInitialDepartmentId, "
                                        + "@DonorRegistrationStatusValue, "
                                        + "b'0', @_IsArchived, @_Unverified, NOW(), @CreatedBy, NOW(), @LastModifiedBy)";

            #endregion SQLQuery InsertIntoDonors

            #region SQLParamaters

            //MySqlParameter[] param = new MySqlParameter[12];
            ParamHelper paramHelper = new ParamHelper();

            if (reportDetails.DonorFirstName != string.Empty)
            {
                if (reportDetails.DonorFirstName != null && reportDetails.DonorFirstName != string.Empty)
                {
                    paramHelper.Param = new MySqlParameter("@DonorFirstName", reportDetails.DonorFirstName);
                }
                else
                {
                    paramHelper.Param = new MySqlParameter("@DonorFirstName", string.Empty);
                }
            }
            else
            {
                paramHelper.Param = new MySqlParameter("@DonorFirstName", string.Empty);
            }

            paramHelper.Param = new MySqlParameter("@DonorMI", reportDetails.DonorMI);

            if (reportDetails.DonorLastName != string.Empty)
            {
                if (reportDetails.DonorLastName != null && reportDetails.DonorLastName != string.Empty)
                {
                    paramHelper.Param = new MySqlParameter("@DonorLastName", reportDetails.DonorLastName);
                }
                else
                {
                    paramHelper.Param = new MySqlParameter("@DonorLastName", string.Empty);
                }
            }
            else
            {
                paramHelper.Param = new MySqlParameter("@DonorLastName", string.Empty);
            }

            paramHelper.Param = new MySqlParameter("@DonorSSN", reportDetails.SsnId);

            if (reportDetails.DonorDOB != string.Empty)
            {
                if (reportDetails.DonorDOB != null && reportDetails.DonorDOB != string.Empty)
                {
                    paramHelper.Param = new MySqlParameter("@DonorDateOfBirth", DateTime.ParseExact(reportDetails.DonorDOB.Substring(0, 8), "yyyyMMdd", CultureInfo.InvariantCulture));
                }
                else
                {
                    paramHelper.Param = new MySqlParameter("@DonorDateOfBirth", null);
                }
            }
            else
            {
                paramHelper.Param = new MySqlParameter("@DonorDateOfBirth", null);
            }
            try
            {
                if (reportDetails.DonorGender != string.Empty)
                {
                    if (reportDetails.DonorGender.ToUpper() == "M" || reportDetails.DonorGender.ToUpper() == "MALE")
                    {
                        paramHelper.Param = new MySqlParameter("@DonorGender", ((int)Gender.Male).ToString());
                    }
                    else if (reportDetails.DonorGender.ToUpper() == "F" || reportDetails.DonorGender.ToUpper() == "FEMALE")
                    {
                        paramHelper.Param = new MySqlParameter("@DonorGender", ((int)Gender.Female).ToString());
                    }
                    else
                    {
                        paramHelper.Param = new MySqlParameter("@DonorGender", null);
                    }
                }
                else
                {
                    paramHelper.Param = new MySqlParameter("@DonorGender", null);
                }
            }
            catch (Exception ex)
            {
                paramHelper.Param = new MySqlParameter("@DonorGender", null);
            }

            paramHelper.Param = new MySqlParameter("@DonorInitialClientId", reportDetails.ClientId);
            paramHelper.Param = new MySqlParameter("@DonorInitialDepartmentId", reportDetails.ClientDepartmentId);
            paramHelper.Param = new MySqlParameter("@DonorRegistrationStatusValue", (int)DonorRegistrationStatus.Registered);
            paramHelper.Param = new MySqlParameter("@CreatedBy", "SYSTEM");
            paramHelper.Param = new MySqlParameter("@LastModifiedBy", "SYSTEM");
            paramHelper.Param = new MySqlParameter("@donor_pid", reportDetails.PID);
            paramHelper.Param = new MySqlParameter("@donor_pid_type_id", reportDetails.PIDType);
            paramHelper.Param = new MySqlParameter("@_IsArchived", _IsArchived);
            paramHelper.Param = new MySqlParameter("@_Unverified", _Unverified);
            #endregion SQLParamaters

            SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, paramHelper.Params);

            sqlQuery = "SELECT LAST_INSERT_ID()";

            returnValues.DonorId = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlQuery));

            AddPidForNewDonorID(returnValues.DonorId, reportDetails.PID, reportDetails.PIDType, trans);
            //AddPidForNewDonorID(returnValues.DonorId, reportDetails.SsnId, (int)PidTypes.SSN, trans);
            //AddPidForNewDonorID(returnValues.DonorId, reportDetails.PID_NODASHES_4, (int)PidTypes.DL, trans);

            if (_AddTestInfo == true)
            {
                AddTestInfoDetails(reportDetails, returnValues, trans);
            }
        }


        /// <summary>
        /// this will go to our mismatch client & department
        /// </summary>
        /// <param name="reportDetails"></param>
        /// <param name="returnValues"></param>
        /// <param name="trans"></param>
        /// <param name="_SetDonorAsArchived"></param>
        private void AddDonorInfoWithTestDetailsWithUnknownPanel(ReportInfo reportDetails, ReturnValues returnValues, MySqlTransaction trans, bool _SetDonorAsArchived = false)
        {
            
            #region SQLQuery InsertIntoDonors

            int _IsArchived = 0;
            if (_SetDonorAsArchived) _IsArchived = 1;
            int _Unverified = _IsArchived;

            string sqlQuery = "INSERT INTO donors ("
                                        + "donor_first_name, "
                                        + "donor_mi, "
                                        + "donor_last_name, "
                                        + "donor_ssn, "
                                        + "donor_pid, "
                                        + "donor_pid_type_id, "
                                        + "donor_date_of_birth, "
                                        + "donor_gender, "
                                        + "donor_initial_client_id, "
                                        + "donor_initial_department_id, "
                                        + "donor_registration_status, "
                                        + "is_synchronized, "
                                        + "is_archived, "
                                        + "unverified, "
                                        + "created_on, "
                                        + "created_by, "
                                        + "last_modified_on, "
                                        + "last_modified_by) VALUES ("
                                        + "@DonorFirstName, "
                                        + "@DonorMI, "
                                        + "@DonorLastName, "
                                        + "@DonorSSN, "
                                        + "@donor_pid, "
                                        + "@donor_pid_type_id, "
                                        + "@DonorDateOfBirth, "
                                        + "@DonorGender, "
                                        + "@DonorInitialClientId, "
                                        + "@DonorInitialDepartmentId, "
                                        + "@DonorRegistrationStatusValue, "
                                        + "b'0', @_IsArchived, @_Unverified, NOW(), @CreatedBy, NOW(), @LastModifiedBy)";

            #endregion SQLQuery InsertIntoDonors

            #region SQLParamaters

            //MySqlParameter[] param = new MySqlParameter[12];
            ParamHelper paramHelper = new ParamHelper();

            if (reportDetails.DonorFirstName != string.Empty)
            {
                if (reportDetails.DonorFirstName != null && reportDetails.DonorFirstName != string.Empty)
                {
                    paramHelper.Param = new MySqlParameter("@DonorFirstName", reportDetails.DonorFirstName);
                }
                else
                {
                    paramHelper.Param = new MySqlParameter("@DonorFirstName", string.Empty);
                }
            }
            else
            {
                paramHelper.Param = new MySqlParameter("@DonorFirstName", string.Empty);
            }

            paramHelper.Param = new MySqlParameter("@DonorMI", reportDetails.DonorMI);

            if (reportDetails.DonorLastName != string.Empty)
            {
                if (reportDetails.DonorLastName != null && reportDetails.DonorLastName != string.Empty)
                {
                    paramHelper.Param = new MySqlParameter("@DonorLastName", reportDetails.DonorLastName);
                }
                else
                {
                    paramHelper.Param = new MySqlParameter("@DonorLastName", string.Empty);
                }
            }
            else
            {
                paramHelper.Param = new MySqlParameter("@DonorLastName", string.Empty);
            }

            paramHelper.Param = new MySqlParameter("@DonorSSN", reportDetails.SsnId);

            if (reportDetails.DonorDOB != string.Empty)
            {
                if (reportDetails.DonorDOB != null && reportDetails.DonorDOB != string.Empty)
                {
                    paramHelper.Param = new MySqlParameter("@DonorDateOfBirth", DateTime.ParseExact(reportDetails.DonorDOB.Substring(0, 8), "yyyyMMdd", CultureInfo.InvariantCulture));
                }
                else
                {
                    paramHelper.Param = new MySqlParameter("@DonorDateOfBirth", null);
                }
            }
            else
            {
                paramHelper.Param = new MySqlParameter("@DonorDateOfBirth", null);
            }
            try
            {
                if (reportDetails.DonorGender != string.Empty)
                {
                    if (reportDetails.DonorGender.ToUpper() == "M" || reportDetails.DonorGender.ToUpper() == "MALE")
                    {
                        paramHelper.Param = new MySqlParameter("@DonorGender", ((int)Gender.Male).ToString());
                    }
                    else if (reportDetails.DonorGender.ToUpper() == "F" || reportDetails.DonorGender.ToUpper() == "FEMALE")
                    {
                        paramHelper.Param = new MySqlParameter("@DonorGender", ((int)Gender.Female).ToString());
                    }
                    else
                    {
                        paramHelper.Param = new MySqlParameter("@DonorGender", null);
                    }
                }
                else
                {
                    paramHelper.Param = new MySqlParameter("@DonorGender", null);
                }
            }
            catch (Exception ex)
            {
                paramHelper.Param = new MySqlParameter("@DonorGender", null);
            }

            paramHelper.Param = new MySqlParameter("@DonorInitialClientId", reportDetails.ClientId);
            paramHelper.Param = new MySqlParameter("@DonorInitialDepartmentId", reportDetails.ClientDepartmentId);
            paramHelper.Param = new MySqlParameter("@DonorRegistrationStatusValue", (int)DonorRegistrationStatus.Registered);
            paramHelper.Param = new MySqlParameter("@CreatedBy", "SYSTEM");
            paramHelper.Param = new MySqlParameter("@LastModifiedBy", "SYSTEM");
            paramHelper.Param = new MySqlParameter("@donor_pid", reportDetails.PID);
            paramHelper.Param = new MySqlParameter("@donor_pid_type_id", reportDetails.PIDType);
            paramHelper.Param = new MySqlParameter("@_IsArchived", _IsArchived);
            paramHelper.Param = new MySqlParameter("@_Unverified", _Unverified);
            #endregion SQLParamaters

            SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, paramHelper.Params);

            sqlQuery = "SELECT LAST_INSERT_ID()";

            returnValues.DonorId = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlQuery));

            AddPidForNewDonorID(returnValues.DonorId, reportDetails.PID, reportDetails.PIDType, trans);
            AddPidForNewDonorID(returnValues.DonorId, reportDetails.SsnId, (int)PidTypes.SSN, trans);
            AddPidForNewDonorID(returnValues.DonorId, reportDetails.PID_NODASHES_4, (int)PidTypes.DL, trans);


            AddTestInfoDetails(reportDetails, returnValues, trans);
        }


        //private void GetDonorInfoWithTestDetails(ReportInfo reportDetails, ReturnValues returnValues, MySqlTransaction trans)
        //{
        //    #region SQLQuery InsertIntoDonors

        //    string sqlQuery = "INSERT INTO donors ("
        //                                + "donor_first_name, "
        //                                + "donor_mi, "
        //                                + "donor_last_name, "
        //                                + "donor_ssn, "
        //                                + "donor_pid, "
        //                                + "donor_pid_type_id, "
        //                                + "donor_date_of_birth, "
        //                                + "donor_gender, "
        //                                + "donor_initial_client_id, "
        //                                + "donor_initial_department_id, "
        //                                + "donor_registration_status, "
        //                                + "is_synchronized, "
        //                                + "is_archived, "
        //                                + "created_on, "
        //                                + "created_by, "
        //                                + "last_modified_on, "
        //                                + "last_modified_by) VALUES ("
        //                                + "@DonorFirstName, "
        //                                + "@DonorMI, "
        //                                + "@DonorLastName, "
        //                                + "@DonorSSN, "
        //                                + "@donor_pid, "
        //                                + "@donor_pid_type_id, "
        //                                + "@DonorDateOfBirth, "
        //                                + "@DonorGender, "
        //                                + "@DonorInitialClientId, "
        //                                + "@DonorInitialDepartmentId, "
        //                                + "@DonorRegistrationStatusValue, "
        //                                + "b'0', b'0', NOW(), @CreatedBy, NOW(), @LastModifiedBy)";

        //    #endregion SQLQuery InsertIntoDonors

        //    #region SQLParamaters

        //    //MySqlParameter[] param = new MySqlParameter[12];
        //    ParamHelper paramHelper = new ParamHelper();

        //    if (reportDetails.DonorFirstName != string.Empty)
        //    {
        //        if (reportDetails.DonorFirstName != null && reportDetails.DonorFirstName != string.Empty)
        //        {
        //            paramHelper.Param = new MySqlParameter("@DonorFirstName", reportDetails.DonorFirstName);
        //        }
        //        else
        //        {
        //            paramHelper.Param = new MySqlParameter("@DonorFirstName", string.Empty);
        //        }
        //    }
        //    else
        //    {
        //        paramHelper.Param = new MySqlParameter("@DonorFirstName", string.Empty);
        //    }

        //    paramHelper.Param = new MySqlParameter("@DonorMI", reportDetails.DonorMI);

        //    if (reportDetails.DonorLastName != string.Empty)
        //    {
        //        if (reportDetails.DonorLastName != null && reportDetails.DonorLastName != string.Empty)
        //        {
        //            paramHelper.Param = new MySqlParameter("@DonorLastName", reportDetails.DonorLastName);
        //        }
        //        else
        //        {
        //            paramHelper.Param = new MySqlParameter("@DonorLastName", string.Empty);
        //        }
        //    }
        //    else
        //    {
        //        paramHelper.Param = new MySqlParameter("@DonorLastName", string.Empty);
        //    }

        //    paramHelper.Param = new MySqlParameter("@DonorSSN", reportDetails.SsnId);

        //    if (reportDetails.DonorDOB != string.Empty)
        //    {
        //        if (reportDetails.DonorDOB != null && reportDetails.DonorDOB != string.Empty)
        //        {
        //            paramHelper.Param = new MySqlParameter("@DonorDateOfBirth", DateTime.ParseExact(reportDetails.DonorDOB.Substring(0, 8), "yyyyMMdd", CultureInfo.InvariantCulture));
        //        }
        //        else
        //        {
        //            paramHelper.Param = new MySqlParameter("@DonorDateOfBirth", null);
        //        }
        //    }
        //    else
        //    {
        //        paramHelper.Param = new MySqlParameter("@DonorDateOfBirth", null);
        //    }
        //    try
        //    {
        //        if (reportDetails.DonorGender != string.Empty)
        //        {
        //            if (reportDetails.DonorGender.ToUpper() == "M" || reportDetails.DonorGender.ToUpper() == "MALE")
        //            {
        //                paramHelper.Param = new MySqlParameter("@DonorGender", ((int)Gender.Male).ToString());
        //            }
        //            else if (reportDetails.DonorGender.ToUpper() == "F" || reportDetails.DonorGender.ToUpper() == "FEMALE")
        //            {
        //                paramHelper.Param = new MySqlParameter("@DonorGender", ((int)Gender.Female).ToString());
        //            }
        //            else
        //            {
        //                paramHelper.Param = new MySqlParameter("@DonorGender", null);
        //            }
        //        }
        //        else
        //        {
        //            paramHelper.Param = new MySqlParameter("@DonorGender", null);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        paramHelper.Param = new MySqlParameter("@DonorGender", null);
        //    }

        //    paramHelper.Param = new MySqlParameter("@DonorInitialClientId", reportDetails.ClientId);
        //    paramHelper.Param = new MySqlParameter("@DonorInitialDepartmentId", reportDetails.ClientDepartmentId);
        //    paramHelper.Param = new MySqlParameter("@DonorRegistrationStatusValue", (int)DonorRegistrationStatus.Registered);
        //    paramHelper.Param = new MySqlParameter("@CreatedBy", "SYSTEM");
        //    paramHelper.Param = new MySqlParameter("@LastModifiedBy", "SYSTEM");
        //    paramHelper.Param = new MySqlParameter("@donor_pid", reportDetails.PID);
        //    paramHelper.Param = new MySqlParameter("@donor_pid_type_id", reportDetails.PIDType);
        //    #endregion SQLParamaters

        //    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, paramHelper.Params);

        //    sqlQuery = "SELECT LAST_INSERT_ID()";

        //    returnValues.DonorId = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlQuery));

        //    // AddTestInfoDetails(reportDetails, returnValues, trans);
        //}


        private void AddTestInfoDetails(ReportInfo reportDetails, ReturnValues returnValues, MySqlTransaction trans)
        {


            ClientDepartment clientDepartment = GetClientDepartment(returnValues, trans);
            Client client = null;
            Vendor vendor = null;

            if (clientDepartment.ClientId < 1 || clientDepartment.ClientDepartmentId < 1) throw new Exception("ERROR: Client ID and Client Department ID are required");

            DonorRegistrationStatus testStatus = DonorRegistrationStatus.Processing;

            double mallCost = 0.0;
            double mposCost = 0.0;

            if (clientDepartment != null)
            {
                returnValues.MROType = (ClientMROTypes)clientDepartment.MROTypeId;
                client = GetClient(clientDepartment.ClientId, trans);

                if (client != null)
                {
                    if (client.MROVendorId != null)
                    {
                        vendor = GetVendor(Convert.ToInt32(client.MROVendorId), trans);

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

                #region SQLQuery INTO donor_test_info



                string sqlQuery = "INSERT INTO donor_test_info ("
                                            + "donor_id, "
                                            + "client_id, "
                                            + "client_department_id, "
                                            + "mro_type_id, "
                                            + "payment_type_id, "
                                            + "screening_time, "
                                            + "test_requested_date, "
                                            + "test_requested_by, "
                                            + "test_status, "
                                            + "is_walkin_donor, "
                                            + "is_reverse_entry, "
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
                                            + "@ScreeningTime, "
                                            + "@SpecimenCollectionDate, "
                                            + "@TestRequestedBy, "
                                            + "@TestStatus, "
                                            + "@IsWalkinDonor, "
                                            + "b'1', b'0', NOW(), @CreatedBy, NOW(), @LastModifiedBy)";

                #endregion SQLQuery INTO donor_test_info

                #region SQLParamaters

                MySqlParameter[] param = new MySqlParameter[12];

                param[0] = new MySqlParameter("@DonorId", returnValues.DonorId);
                param[1] = new MySqlParameter("@ClientId", clientDepartment.ClientId);
                param[2] = new MySqlParameter("@ClientDepartmentId", clientDepartment.ClientDepartmentId);
                param[3] = new MySqlParameter("@MROTypeId", (int)clientDepartment.MROTypeId);
                param[4] = new MySqlParameter("@PaymentTypeId", (int)clientDepartment.PaymentTypeId);

                if (reportDetails.SpecimenCollectionDate != string.Empty)
                {
                    if (reportDetails.SpecimenCollectionDate != null && reportDetails.SpecimenCollectionDate != string.Empty)
                    {
                        param[5] = new MySqlParameter("@ScreeningTime", DateTime.ParseExact(reportDetails.SpecimenCollectionDate.Substring(0, 8), "yyyyMMdd", CultureInfo.InvariantCulture));
                        param[6] = new MySqlParameter("@SpecimenCollectionDate", DateTime.ParseExact(reportDetails.SpecimenCollectionDate.Substring(0, 8), "yyyyMMdd", CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        param[5] = new MySqlParameter("@ScreeningTime", null);
                        param[6] = new MySqlParameter("@SpecimenCollectionDate", null);
                    }
                }
                else
                {
                    param[5] = new MySqlParameter("@ScreeningTime", null);
                    param[6] = new MySqlParameter("@SpecimenCollectionDate", null);
                }

                param[7] = new MySqlParameter("@TestRequestedBy", 1);
                param[8] = new MySqlParameter("@TestStatus", testStatus);
                param[9] = new MySqlParameter("@IsWalkinDonor", false);
                param[10] = new MySqlParameter("@CreatedBy", "SYSTEM");
                param[11] = new MySqlParameter("@LastModifiedBy", "SYSTEM");

                #endregion SQLParamaters

                SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                sqlQuery = "SELECT LAST_INSERT_ID()";

                returnValues.DonorTestInfoId = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlQuery));

                #region SQLQuery INSERT INTO donor_test_info_test_categories

                string sqlTestCategory = "INSERT INTO donor_test_info_test_categories ("
                                            + "donor_test_info_id, "
                                            + "test_category_id, "
                                            + "test_panel_id, "
                                            + "specimen_id, "
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
                                            + "@SpecimenId, "
                                            + "@HairTestPanelDays, "
                                            + "@TestPanelStatus, "
                                            + "@TestPanelCost, "
                                            + "@TestPanelPrice, "
                                            + "b'0', "
                                            + "NOW(), "
                                            + "@CreatedBy, "
                                            + "NOW(), "
                                            + "@LastModifiedBy)";

                #endregion SQLQuery INSERT INTO donor_test_info_test_categories

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

                        if (testCategory.TestCategoryId == TestCategories.UA || testCategory.TestCategoryId == TestCategories.Hair || testCategory.TestCategoryId == TestCategories.BC || testCategory.TestCategoryId == TestCategories.RC)
                        //    if (testCategory.TestCategoryId == TestCategories.UA || testCategory.TestCategoryId == TestCategories.Hair)
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

                        string specimenId = "";

                        if (testCategory.TestCategoryId == TestCategories.UA)
                        {
                            specimenId = reportDetails.SpecimenId;
                        }


                        #region SQLParamaters

                        param = new MySqlParameter[10];

                        param[0] = new MySqlParameter("@DonorTestInfoId", returnValues.DonorTestInfoId);
                        param[1] = new MySqlParameter("@TestCategoryId", (int)testCategory.TestCategoryId);
                        param[2] = new MySqlParameter("@TestPanelId", testPanelId);
                        param[3] = new MySqlParameter("@SpecimenId", specimenId);
                        param[4] = new MySqlParameter("@HairTestPanelDays", hairTestPanelDays);
                        param[5] = new MySqlParameter("@TestPanelStatus", testStatus);
                        param[6] = new MySqlParameter("@TestPanelCost", testPanelCost);
                        param[7] = new MySqlParameter("@TestPanelPrice", testPanelPrice);
                        param[8] = new MySqlParameter("@CreatedBy", "SYSTEM");
                        param[9] = new MySqlParameter("@LastModifiedBy", "SYSTEM");

                        #endregion SQLParamaters

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

                        param[0] = new MySqlParameter("@DonorTestInfoId", returnValues.DonorTestInfoId);
                        param[1] = new MySqlParameter("@TotalPaymentAmount", totalPaymentAmount);
                        param[2] = new MySqlParameter("@TotalTestPanelCost", totalTestPanelCost);
                        param[3] = new MySqlParameter("@MROCost", mallCost);
                    }

                    if (clientDepartment.MROTypeId == ClientMROTypes.MPOS)
                    {
                        sqlQuery = "UPDATE donor_test_info SET total_payment_amount = @TotalPaymentAmount, laboratory_cost = @TotalTestPanelCost, mro_cost = @MROCost WHERE donor_test_info_id = @DonorTestInfoId";

                        param = new MySqlParameter[4];

                        param[0] = new MySqlParameter("@DonorTestInfoId", returnValues.DonorTestInfoId);
                        param[1] = new MySqlParameter("@TotalPaymentAmount", totalPaymentAmount);
                        param[2] = new MySqlParameter("@TotalTestPanelCost", totalTestPanelCost);
                        param[3] = new MySqlParameter("@MROCost", mposCost);
                    }

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);
                }

                #region SQLQuery NSERT INTO donor_test_activity

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

                #endregion SQLQuery NSERT INTO donor_test_activity

                #region SQLParamaters

                param = new MySqlParameter[5];

                param[0] = new MySqlParameter("@DonorTestInfoId", returnValues.DonorTestInfoId);
                param[1] = new MySqlParameter("@ActivityUserId", 1);
                param[2] = new MySqlParameter("@ActivityCategoryId", (int)DonorActivityCategories.General);
                param[3] = new MySqlParameter("@IsActivityVisible", true);
                param[4] = new MySqlParameter("@ActivityNote", "New test registered" + (totalPaymentAmount > 0 ? " (Payment Due Amount: $" + totalPaymentAmount.ToString() + ")" : ""));

                #endregion SQLParamaters

                SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);
            }
        }
    }
}