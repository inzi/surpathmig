using BackendHelpers;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SurPath.Entity;
using SurPath.Enum;
using SurPath.MySQLHelper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace SurPath.Data
{
    /// <summary>
    /// Inserts
    /// </summary>
    public partial class HL7ParserDao : DataObject
    {
        public bool CheckDuplicateSpecimenId(ReportType reportType, string specimenId)
        {
            bool returnValue = false;

            string sqlQuery = "SELECT * FROM report_info WHERE report_type = @ReportType AND specimen_id = @SpecimenId";

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                MySqlParameter[] param = new MySqlParameter[2];
                param[0] = new MySqlParameter("@ReportType", (int)reportType);
                param[1] = new MySqlParameter("@SpecimenId", specimenId);

                MySqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                if (dr.Read())
                {
                    returnValue = true;
                }
            }

            return returnValue;
        }
        public static string JsonParserMasks;
        public static PIDHelper pidHelper = new PIDHelper();

        public bool UpdateReportFileMetaData(int report_info_id, string lab_report_checksum, string lab_report_source_filename, string data_checksum)
        {
            // This assumes previous versions of files for this specimen id will have is_archived set to 1

            // ReportType reportType, string specimenId)

            bool returnValue = false;


            ParamHelper param = new ParamHelper();

            param.reset();
            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();
                MySqlTransaction trans = conn.BeginTransaction();
                try
                {
                    string sqlQuery = @"UPDATE surpathlive.report_info
                            SET
                              ,lab_report_checksum = @lab_report_checksum -- varchar(255)
                              ,lab_report_source_filename = @lab_report_source_filename -- varchar(1024)
                              ,data_checksum = @data_checksum -- varchar(255)
                            WHERE report_info_id = @report_info_id -- int(11);";

                    param.Param = new MySqlParameter("@report_info_id", report_info_id);
                    param.Param = new MySqlParameter("@lab_report_checksum", lab_report_checksum);
                    param.Param = new MySqlParameter("@data_checksum", data_checksum);
                    param.Param = new MySqlParameter("@lab_report_source_filename", lab_report_source_filename);
                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param.Params);

                    trans.Commit();
                    return returnValue = true;
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
        /// This function checks to see if this file has already been stored in report_info
        /// there is zero reason to add it twice along with parsed fields
        /// as nothing in the report will be different.
        /// </summary>
        /// <param name="SHA1Hash"></param>
        /// <returns></returns>
        public int IsLatestReport(string SHA1Hash, string filename, ReportType reportType, string specimenId, string data_checksum)
        {
            // This assumes previous versions of files for this specimen id will have is_archived set to 1

            // ReportType reportType, string specimenId)

            int returnValue = 0;
            string lab_report_source_filename = "*";
            // what is this magic checksum? This magic checksum is an empty RTF file that lives in a lot of rows.
            // TODO - make a utility that goes through those records, creates a new RTF file with the specimen ID in it
            // then saves it back

            if (SHA1Hash.Equals("2993129fc9bc4da620cba0767792e2c10368fba8", StringComparison.InvariantCultureIgnoreCase))
            {
                _logger.Information($"{filename} generates and empty report, no DSP records.");
                _logger.Information($"{filename} is likely MRALL record.");
                return 0;
            }

            ParamHelper param = new ParamHelper();
            string sqlQuery = @"SELECT report_info_id, lab_report_source_filename FROM report_info 
                                WHERE lab_report_checksum != '2993129fc9bc4da620cba0767792e2c10368fba8' 
                                and lab_report_checksum = @SHA1Hash 
                                and report_type = @ReportType 
                                AND specimen_id = @SpecimenId
                                and data_checksum = @data_checksum
                                and is_archived=b'0' limit 1;";

            // 
            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {

                param.Param = new MySqlParameter("@SHA1Hash", SHA1Hash);
                param.Param = new MySqlParameter("@ReportType", (int)reportType);
                param.Param = new MySqlParameter("@SpecimenId", specimenId);
                param.Param = new MySqlParameter("@data_checksum", data_checksum);


                MySqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param.Params);

                if (dr.Read())
                {
                    returnValue = Convert.ToInt32(dr["report_info_id"]);
                    lab_report_source_filename = dr["lab_report_source_filename"].ToString();
                }

            }

            if (string.IsNullOrEmpty(lab_report_source_filename) && returnValue > 0)
            {
                _logger.Information($"{filename} is not set for the record, setting");
                param.reset();
                using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
                {
                    conn.Open();
                    MySqlTransaction trans = conn.BeginTransaction();
                    try
                    {
                        sqlQuery = "update report_info set lab_report_source_filename = @lab_report_source_filename WHERE report_info_id = @report_info_id and is_archived=b'0';";

                        param.Param = new MySqlParameter("@report_info_id", returnValue);

                        param.Param = new MySqlParameter("@lab_report_source_filename", filename);
                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param.Params);

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

            return returnValue;

        }

        //public void UpdateReportInfoFilename(string SHA1Hash, string filename)
        //{
        //    using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
        //    {
        //        conn.Open();

        //        MySqlTransaction trans = conn.BeginTransaction();
        //        try
        //        {
        //            string sqlQuery = "Update report_info set is_Archived = 1 where specimen_id = @SpecID";
        //            ParamHelper param = new ParamHelper();
        //            param.Param = new MySqlParameter("@SpecID", SpeciminId);

        //            SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

        //            trans.Commit();
        //        }
        //        catch (Exception ex)
        //        {
        //            trans.Rollback();
        //            throw ex;
        //        }
        //    }
        //}
        private void ProcessLabClientCode(ReportInfo reportDetails, List<OBR_Info> obrList)
        {
            _logger.Information($"Processing Lab Client Code for: {reportDetails.SpecimenId}");


            //Lab client code
            foreach (OBR_Info obr in obrList)
            {
                if ((reportDetails.TpaCode == null || reportDetails.TpaCode == string.Empty) && obr.TpaCode != string.Empty)
                {
                    reportDetails.TpaCode = obr.TpaCode;
                }

                if ((reportDetails.RegionCode == null || reportDetails.RegionCode == string.Empty) && obr.RegionCode != string.Empty)
                {
                    reportDetails.RegionCode = obr.RegionCode;
                }

                if ((reportDetails.ClientCode == null || reportDetails.ClientCode == string.Empty) && obr.ClientCode != string.Empty)
                {
                    reportDetails.ClientCode = obr.ClientCode;
                }

                if ((reportDetails.DepartmentCode == null || reportDetails.DepartmentCode == string.Empty) && obr.DepartmentCode != string.Empty)
                {
                    reportDetails.DepartmentCode = obr.DepartmentCode;
                }

                if ((reportDetails.SpecimenCollectionDate == null || reportDetails.SpecimenCollectionDate == string.Empty) && obr.SpecimenCollectionDate != string.Empty)
                {
                    reportDetails.SpecimenCollectionDate = obr.SpecimenCollectionDate;
                }

                if ((reportDetails.CrlClientCode == null || reportDetails.CrlClientCode == string.Empty) && obr.CrlClientCode != string.Empty)
                {
                    reportDetails.CrlClientCode = obr.CrlClientCode;
                }

                if ((reportDetails.QuestCode == null || reportDetails.QuestCode == string.Empty) && obr.OBRQuestCode != string.Empty)
                {
                    reportDetails.QuestCode = obr.OBRQuestCode;
                }

                if (reportDetails.TpaCode != null
                    && reportDetails.TpaCode != string.Empty
                    && reportDetails.RegionCode != null
                    && reportDetails.RegionCode != string.Empty
                    && reportDetails.ClientCode != null
                    && reportDetails.ClientCode != string.Empty
                    && reportDetails.DepartmentCode != null
                    && reportDetails.DepartmentCode != string.Empty
                    && reportDetails.SpecimenCollectionDate != null
                    && reportDetails.SpecimenCollectionDate != string.Empty
                    && reportDetails.CrlClientCode != null
                    && reportDetails.CrlClientCode != string.Empty
                    )
                {
                    _logger.Information("ProcessLabClientCode All Values Set");

                    break;
                }
            }
            _logger.Information("Processed Client Code");
        }
        private string MakeSafeSSNID(string v)
        {
            string TempSSNID = new String(v.Where(Char.IsDigit).ToArray());
            TempSSNID = new string('0', 9) + TempSSNID;
            TempSSNID = TempSSNID.Substring(TempSSNID.Length - 9);
            return TempSSNID;
        }

        private void SetPIDType(ReportInfo reportDetails, ReturnValues returnValues, ref BackendParserHelper backendParserHelper)
        {
            bool VerbosePidHelper = false;
            if (this.ConfigKeyExists("VerbosePidHelper")) bool.TryParse(ConfigurationManager.AppSettings["VerbosePidHelper"].ToString(), out VerbosePidHelper);
            bool ShowPidInLog = VerbosePidHelper;

            _logger.Information($"Getting Pid Type for Specimen ID: {reportDetails.SpecimenId}");
            _logger.Debug($"backendParserHelper.report_type={backendParserHelper.report_type}");
            _logger.Debug($"reportDetails.ReportType={reportDetails.ReportType}");

            if (this.ConfigKeyExists("ShowPidInLog")) bool.TryParse(ConfigurationManager.AppSettings["ShowPidInLog"].ToString(), out ShowPidInLog);
            pidHelper.ShowPidInLog = ShowPidInLog;
            pidHelper._logger = _logger;

            if (string.IsNullOrEmpty(reportDetails.PID_NODASHES_4) && string.IsNullOrEmpty(reportDetails.PID_DASHES_19))
            {
                _logger.Debug($"PID_NODASHES_4 && PID_DASHES_19 are empty, invalid PID");
                reportDetails.PIDType = (int)PidTypes.Invalid;
                backendParserHelper.ReportHelperItem.NoPIDInFile = true;
                return;
            }

            if (backendParserHelper.report_type == (int)ReportType.QuestLabReport)
            {
                _logger.Debug($"This is a quest lab report, using SSN");

                reportDetails.PIDType = (int)PidTypes.QuestDonorId;
                reportDetails.SsnId = MakeSafeSSNID(reportDetails.PID_DASHES_19); // assign to SSN for now for legacy usage, padded in case Tim only entered partial
                reportDetails.PID = reportDetails.SsnId;
                return;
            }

            // Ok, it's NOT a quest file...
            _logger.Debug($"We have PID info, and this is not a quest file");


                // We get a our two pids, and there are ID w/dashes and id w/o dashes (sometimes DL) fields defined in HL7.
                // 19 is generall considered non-ssn while 4 could be. I3Screen SSNs are set there.
                // Babblefish takes whatever ID given. 
                // stores in HL7 file as:
                ////// We'll store info in format ID^^VAL separated by triple carrots (^^^)
                ////// string BabbleInfo = $"issuingAuthority^^{issuingAuthority.ToString()}^^^";
                // Col 19 of PID segment is ID or SSN WITH dashes
                // col 4 of PID segment is ID or SSN with NO DASHES
                // This only applies to non-quest files
                // We will default to a PID being a SSN if it's 9 digits for safety reasons. 
                // we have NOT set SSN id yet....

                try
                {
                string pid_4 = (reportDetails.PID_NODASHES_4 + string.Empty).Trim();
                string pid_19 = (reportDetails.PID_DASHES_19 + string.Empty).Trim();

                if (ShowPidInLog == true)
                {
                    _logger.Debug($"Working with PID_4: {pid_4} and PID_19: {pid_19}");
                }

                bool pid_4_19_equal = pid_4 == pid_19; // raw equal 

                PidTypes pid_4_type = GetPidTypeByMask(pid_4);
                PidTypes pid_19_type = GetPidTypeByMask(pid_19);


                // if we matched SSN (9 digits) - we'll assume SSN, just in case

                if (pid_19_type == PidTypes.SSN && pid_4_type == PidTypes.SSN)
                {
                    _logger.Debug($"SSN type, both pid_19 and pid_4 match, this is an SSN");
                    reportDetails.SsnId = MakeSafeSSNID(pid_4);
                    reportDetails.PID = pid_4;
                    reportDetails.PIDType = (int)PidTypes.SSN;
                    return;
                }

                if (pid_4_type == PidTypes.SSN)
                {
                    _logger.Debug($"Pid 4 is an SSN, using");
                    // pid 4 is a SSN
                    reportDetails.SsnId = MakeSafeSSNID(pid_4);
                    reportDetails.PID = pid_4;
                    reportDetails.PIDType = (int)PidTypes.SSN;
                    return;
                }

                if (pid_19_type == PidTypes.SSN)
                {
                    _logger.Debug($"Pid 19 is an SSN, using");
                    // pid 19 is a SSN
                    reportDetails.SsnId = MakeSafeSSNID(pid_19);
                    reportDetails.PID = pid_19;
                    reportDetails.PIDType = (int)PidTypes.SSN;
                    return;
                }
                _logger.Debug("Neither PID is a valid SSN");
                // So, we don't have a SSN at this point
                // in either field 4 or 19
                List<PidMask> pid4Masks = pidHelper.Evaluate(pid_4);
                List<PidMask> pid19Masks = pidHelper.Evaluate(pid_19);

                //    // if we match a type 0, it's a excluded or uknown type
                bool pid_4_invalid = pid4Masks.Where(x => (int)x.Type == (int)PidTypes.Invalid).Count() > 0;
                bool pid_19_invalid = pid19Masks.Where(x => (int)x.Type == (int)PidTypes.Invalid).Count() > 0;

                // they're both invalid, we don't have a pid type.
                if (pid_4_invalid == true && pid_19_invalid == true)
                {
                    _logger.Information($"Both PID fields are invalid \"n/s\" for example.");
                    reportDetails.PIDType = (int)PidTypes.Invalid;
                    backendParserHelper.ReportHelperItem.NoPIDInFile = true;
                    return;
                }

                //////// if we don't have anything, we use pid_4, and if empty, pid_19
                //////if (pid4Masks.Where(x => (int)x.Type != (int)PidTypes.Invalid).Count() < 1 && pid19Masks.Where(x => (int)x.Type != (int)PidTypes.Invalid).Count() < 1)
                //////{
                //////    // neither is invalid, and not an SSN, so it's an other
                //////    if (!string.IsNullOrEmpty(pid_4))
                //////    {
                //////        reportDetails.PID = pid_4;
                //////        reportDetails.SsnId = MakeSafeSSNID(reportDetails.PID);
                //////        reportDetails.PIDType = (int)PidTypes.Other;
                //////        return;
                //////    }
                //////    else
                //////    {
                //////        reportDetails.PID = pid_19;
                //////        reportDetails.SsnId = reportDetails.PID;
                //////        reportDetails.PIDType = (int)PidTypes.Other;
                //////        return;
                //////    }

                //////}


                // we'll defer to field 4 first, then 19
                if (pid4Masks.Count() == 1 && !pid_4_invalid)
                {

                    // we have a match for something on pid 4

                    reportDetails.SsnId =MakeSafeSSNID(pid_4);// this is for legacy support
                    reportDetails.PID = pid_4;
                    reportDetails.PIDType = (int)pid4Masks.FirstOrDefault().Type;
                    //(ReportType)r.ReportTypeEnum).ToString()
                    _logger.Debug($"PID 4 type identified: {((PidTypes)pid4Masks.FirstOrDefault().Type).ToString()}");
                    return;
                }
                if (pid19Masks.Count() == 1 && !pid_19_invalid)
                {
                    reportDetails.SsnId = MakeSafeSSNID(pid_19);  // this is for legacy support
                    reportDetails.PID = pid_19;
                    reportDetails.PIDType = (int)pid19Masks.FirstOrDefault().Type;
                    _logger.Debug($"PID 19 type identified: {((PidTypes)pid19Masks.FirstOrDefault().Type).ToString()}");
                    return;
                }
                // lastly, if we have multiple type matches, we'll call it an "other" because we don't know
                if ((pid4Masks.Count() > 1 || pid4Masks.Count() == 0 ) && !pid_4_invalid)
                {
                    // we have a match for something on pid 4
                    reportDetails.SsnId = MakeSafeSSNID(pid_4);  // this is for legacy support
                    reportDetails.PID = pid_4;
                    reportDetails.PIDType = (int)PidTypes.Other;
                    _logger.Debug($"PID 4 type isn't invalid but it is unidentified: Other");
                    return;
                }
                if ((pid19Masks.Count() >1 || pid4Masks.Count()==0) && !pid_19_invalid)
                {
                    reportDetails.SsnId = MakeSafeSSNID(pid_19);  // this is for legacy support
                    reportDetails.PID = pid_19;
                    reportDetails.PIDType = (int)PidTypes.Other;
                    _logger.Debug($"PID 19 type isn't invalid but it is unidentified: Other");
                    return;
                }


                // if we're here - we don't know anything - we *shouldn't* be here

                _logger.Information(new string('!', 50));
                _logger.Information("");

                _logger.Information("SetPIDType anomaly !!");
                _logger.Information("");
                _logger.Information(new string('!', 50));


                //    bool PIDeqSSN = reportDetails.SsnId.Equals(reportDetails.PID, StringComparison.InvariantCultureIgnoreCase);
                //    bool PIDeqDL = reportDetails.PID_DASHES_4.Equals(reportDetails.PID, StringComparison.InvariantCultureIgnoreCase);


                //    List<PidMask> pidMasks = pidHelper.Evaluate(reportDetails.PID);



                //    // if we get a single valid match - we go with that.
                //    if (pidMasks.Count() == 1 && !IsInvalidPID)
                //    {
                //        reportDetails.PIDType = (int)pidMasks.FirstOrDefault().Type;
                //        return;
                //    }

                //    // If we match a ssn, we'll treat it as such
                //    //if (pidMasks.Where(x => (int)x.Type == (int)PidTypes.SSN).Count() > 0) pidValidSSN = true;
                //    //if (pidMasks.Where(x => (int)x.Type == (int)PidTypes.DL).Count() > 0) pidValidDL = true;

                //    if (ssnIsValid && PIDeqSSN)
                //    {
                //        // valid SSN and PID matches SSN
                //        reportDetails.PIDType = (int)PidTypes.SSN;
                //    }
                //    else if (dlIsValid && PIDeqDL)
                //    {
                //        // DL is valid and PID matches DL
                //        reportDetails.PIDType = (int)PidTypes.DL;
                //    }
                //    else if (!dlIsValid && ssnIsValid && PIDeqDL)
                //    {
                //        // DL is PID, but it isn't a valid DL. Our SSN is valid,so use that.
                //        reportDetails.PIDType = (int)PidTypes.SSN;
                //        reportDetails.PID = reportDetails.SsnId;

                //    }
                //    else if (!dlIsValid && !ssnIsValid && PIDeqDL)
                //    {
                //        // DL is invalid, the SSN is invalid, but it matches a driver's licence. We'll presume the DL was put in the SSN field
                //        reportDetails.PIDType = (int)PidTypes.DL;
                //        reportDetails.PID = reportDetails.SsnId;
                //        // since SSN is invalid - we'll erase it to avoid sql errors
                //        reportDetails.SsnId = "N/S";
                //        reportDetails.PID_DASHES_4 = reportDetails.PID;
                //    }
                //    else if (!IsInvalidPID)
                //    {
                //        // It's not a SSN, and it's not a DL - but it's also not one of our excluded values ('like N/S')
                //        reportDetails.PIDType = (int)PidTypes.Other;

                //    }
                //    else
                //    {
                //        // Test if SSN contains something other than a SSN
                //        pidMasks = pidHelper.Evaluate(reportDetails.SsnId);
                //        bool IsInvalidSSN = pidMasks.Where(x => (int)x.Type == (int)PidTypes.None).Count() > 0;
                //        if (pidMasks.Count() == 1 && !IsInvalidSSN)
                //        {
                //            reportDetails.PID = reportDetails.SsnId;
                //            reportDetails.PIDType = (int)pidMasks.FirstOrDefault().Type;

                //        }
                //        else
                //        {
                //            reportDetails.PID = reportDetails.SsnId;
                //            reportDetails.PIDType = (int)PidTypes.Other;
                //            // SSN is something we don't know, but it's not invalid, so we'll use it
                //        }

                //    }


                //    // last - we check one last time that the PID is not invalid
                //    pidMasks = pidHelper.Evaluate(reportDetails.PID);

                //    // if we match a type 0, it's a excluded type
                //    IsInvalidPID = pidMasks.Where(x => (int)x.Type == (int)PidTypes.None).Count() > 0;

                //    //if we STILL have an invalid pid, We can't classify it, maybe let's use the specemin id
                //    if (IsInvalidPID)
                //    {
                //        reportDetails.PIDType = (int)PidTypes.Other;
                //    }


                //    if (!ssnIsValid)
                //    {
                //        // last but not least, in case the SSN would break the DB:
                //        //reportDetails.SsnId = "N/S";

                //        // Stop Gap until UI is updated - we need to put something in SSN that will 
                //        // be searchable.
                //        // Temproarily - we will pull any numbers from the PID - the right 9 characters 
                //        // take 9 right most, padding with zeros in case it's too short
                //        reportDetails.SsnId = MakeSafeSSNID(reportDetails.PID);
                //    }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                _logger.Error(ex.InnerException.ToString());
                backendParserHelper.ReportHelperItem.NoPIDInFile = true;
            }



        }

        private PidTypes GetPidTypeByMask(string pid)
        {
            List<PidMask> pidMasks = pidHelper.Evaluate(pid);

            // If we match a ssn, we'll treat it as such
            if (pidMasks.Where(x => (int)x.Type == (int)PidTypes.SSN).Count() > 0) return PidTypes.SSN;
            if (pidMasks.Where(x => (int)x.Type == (int)PidTypes.DL).Count() > 0) return PidTypes.DL;
            if (pidMasks.Where(x => (int)x.Type == (int)PidTypes.Passport).Count() > 0) return PidTypes.Passport;

            return PidTypes.Other;
        }

        private bool IsPidType(string pid, PidTypes pidTypes)
        {
            List<PidMask> pidMasks = pidHelper.Evaluate(pid);

            if (pidMasks.Where(x => (int)x.Type == (int)pidTypes).Count() > 0) return true;

            return false;
        }

        private void ValidateDonorSSn(ReportInfo reportDetails, ReturnValues returnValues)
        {
            _logger.Information($"Validating SSN for: {reportDetails.SpecimenId}");


            if (string.IsNullOrEmpty(reportDetails.SsnId) && string.IsNullOrEmpty(reportDetails.PID))
            {
                returnValues.ErrorFlag = true;
                returnValues.ErrorMessage = "No Donor Identification Data in file.";
                reportDetails.PIDType = (int)PidTypes.Invalid;
                return;

            }

            // TODO Commenting this out temporarily - because returning false might result in mismatch?
            //if (string.IsNullOrEmpty(reportDetails.SsnId))
            //{
            //    returnValues.ErrorFlag = false;
            //    returnValues.ErrorMessage = "SSN not supplied in file.";
            //    return;

            //}

            string _DigitsSuppliedSSN = Regex.Match(reportDetails.SsnId, @"\d+").Value;
            // This is deliverable 2 code
            //if (_DigitsSuppliedSSN.Length > 9)
            //{
            //    _DigitsSuppliedSSN = _DigitsSuppliedSSN.Substring(_DigitsSuppliedSSN.Length - 9);
            //}

            //if (_DigitsSuppliedSSN.Length < 9)
            //{
            //    _DigitsSuppliedSSN = MakeSafeSSNID(_DigitsSuppliedSSN);
            //}

            ////reportDetails.SsnId = reportDetails.SsnId.Trim().Replace("-", "");
            ////reportDetails.SsnId = reportDetails.SsnId.Substring(0, 3) + "-" + reportDetails.SsnId.Substring(3, 2) + "-" + reportDetails.SsnId.Substring(5);
            //reportDetails.SsnId = _DigitsSuppliedSSN.Insert(5, "-").Insert(3, "-");


            if (_DigitsSuppliedSSN.Length != 9)
            {

                returnValues.ErrorFlag = false;
                returnValues.ErrorMessage = "SSN supplied is invalid format (not 9 digits), must be other type of ID";

                // if we got an invalid SSN in column 4 - is it a drivers' license?
                if (reportDetails.SsnId.Equals(reportDetails.PID_NODASHES_4, StringComparison.InvariantCultureIgnoreCase))
                {
                    reportDetails.SsnId = string.Empty; // not a ssn, a DL
                }
            }
            else
            {
                //reportDetails.SsnId = reportDetails.SsnId.Trim().Replace("-", "");
                //reportDetails.SsnId = reportDetails.SsnId.Substring(0, 3) + "-" + reportDetails.SsnId.Substring(3, 2) + "-" + reportDetails.SsnId.Substring(5);
                reportDetails.SsnId = _DigitsSuppliedSSN.Insert(5, "-").Insert(3, "-");
            }
        }

        private void ArchiveReport(ReportType reportType, ReportInfo reportDetails, bool archiveExistingReport, MySqlTransaction trans)
        {
            string sqlQuery = "UPDATE report_info SET is_synchronized = b'0', is_archived = b'1', last_modified_on = NOW(), last_modified_by = 'SYSTEM' WHERE report_type = @ReportType AND specimen_id = @SpecimenId";

            MySqlParameter[] param = new MySqlParameter[2];

            param[0] = new MySqlParameter("@ReportType", (int)reportType);
            param[1] = new MySqlParameter("@SpecimenId", reportDetails.SpecimenId);

            if (archiveExistingReport)
            {
                SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);
            }
        }

        private void WriteClientLogFile(ReportInfo reportDetails, ReturnValues returnValues, string FileName, int Numpassed, int TotalTo)
        {
            returnValues.ErrorFlag = true;
            returnValues.ErrorMessage = "Lab Code does not match.";

            StreamWriter log;
            FileStream fileStream = null;
            DirectoryInfo logDirInfo = null;
            FileInfo logFileInfo;

            string logFilePath = "D:\\Logs\\";
            logFilePath = logFilePath + "MismatchErrorLog" + "." + "txt";
            logFileInfo = new FileInfo(logFilePath);
            logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
            if (!logDirInfo.Exists) logDirInfo.Create();
            if (!logFileInfo.Exists)
            {
                fileStream = logFileInfo.Create();
            }
            else
            {
                fileStream = new FileStream(logFilePath, FileMode.Append);
            }
            log = new StreamWriter(fileStream);

            var timedate = "Date:" + DateTime.Now.ToString() + "(count:" + Numpassed + "/" + TotalTo + ")";
            var blah = "SpecID:" + reportDetails.SpecimenId.ToString();
            var blah2 = "Error:" + returnValues.ErrorMessage.ToString();
            var blah3 = string.Empty;
            try
            {
                blah3 = "LabCodePassedOnReport:" + reportDetails.CrlClientCode.ToString();
            }
            catch (Exception)
            {
                blah3 = "LabCodePassedOnReport:" + "NULL";
            }

            var blah4 = "LabName:" + reportDetails.LabName.ToString();
            var blah5 = "FileName:" + FileName;
            //var blah5 = "LabCode:" + reportDetails.CrlClientCode.ToString();

            log.WriteLine(timedate + " " + blah + " " + blah2 + " " + blah3 + " " + blah4 + " " + blah5);
            log.Close();
            Console.Write("[WriteClientLogFile - HL7ParserDao] Please See Logfile: " + logFilePath);
        }

        /// <summary>
        /// This function appears to match the details to records that are awaiting results
        /// TODO There's a bug when the two types of (03032020 - two types of what?  - investigate)
        /// </summary>
        /// <param name="reportDetails"></param>
        /// <param name="returnValues"></param>
        /// <param name="trans"></param>
        /// <param name="reportType"></param>
        private void DoTestInfoIntelligence(ReportInfo reportDetails, ReturnValues returnValues, MySqlTransaction trans)
        {
            _logger.Debug("DoTestInfoIntelligence called");
            int daysToAutoMatch = Convert.ToInt32(ConfigurationManager.AppSettings["DaysToAutoMatch"].ToString().Trim()) * -1;
            string reportDate = reportDetails.LabReportDate.Substring(0, 4) + "-" + reportDetails.LabReportDate.Substring(4, 2) + "-" + reportDetails.LabReportDate.Substring(6, 2);
            string sqlQuery = string.Empty;
            MySqlParameter[] param = null;
            int recCount = 0;

            _logger.Debug("Trying via Ordering provider");
            //Using Ordering Provider
            if (returnValues.DonorId > 0
                && returnValues.ClientId > 0
                && returnValues.ClientDepartmentId > 0)
            {
                _logger.Debug("Have DonorID, Client ID, and Client Dept Id");
                _logger.Debug(returnValues.DonorId.ToString());
                _logger.Debug(returnValues.ClientId.ToString());
                _logger.Debug(returnValues.ClientDepartmentId.ToString());
                _logger.Debug(reportDate.ToString());
                _logger.Debug(daysToAutoMatch.ToString());


                sqlQuery = "SELECT "
                                + "donor_test_info.donor_test_info_id AS DonorTestInfoId, "
                                + "donor_test_info.mro_type_id AS MROTypeId "
                                + "FROM donor_test_info "
                                + "INNER JOIN donor_test_info_test_categories ON donor_test_info_test_categories.donor_test_info_id = donor_test_info.donor_test_info_id "
                                + "WHERE test_status IN (3, 4) "
                                + "AND test_category_id in (1,2,3)"
                                + "AND DATE_ADD(DATE(@ReportDate), INTERVAL @DaysToAutoMatch DAY) <= DATE(test_requested_date) "
                                + "AND donor_test_info.donor_id = @DonorId "
                                + "AND donor_test_info.client_id = @ClientId "
                                + "AND donor_test_info.client_department_id = @ClientDepartmentId";

                param = new MySqlParameter[5];
                param[0] = new MySqlParameter("@ReportDate", reportDate);
                param[1] = new MySqlParameter("@DaysToAutoMatch", daysToAutoMatch);
                param[2] = new MySqlParameter("@DonorId", returnValues.DonorId);
                param[3] = new MySqlParameter("@ClientId", returnValues.ClientId);
                param[4] = new MySqlParameter("@ClientDepartmentId", returnValues.ClientDepartmentId);

                MySqlDataReader dr = SqlHelper.ExecuteReader(trans, CommandType.Text, sqlQuery, param);

                while (dr.Read())
                {
                    returnValues.DonorTestInfoId = Convert.ToInt32(dr["DonorTestInfoId"]);
                    _logger.Debug($"Found a donor test info id. returnValues.DonorTestInfoId = {returnValues.DonorTestInfoId}");
                    returnValues.MROType = dr["MROTypeId"] != DBNull.Value ? (ClientMROTypes)(Convert.ToInt32(dr["MROTypeId"].ToString())) : ClientMROTypes.None;
                    recCount++;
                }
                dr.Close();

                if (recCount == 1)
                {
                    // if only one donor_test_info record matches donor id, client id, dept id, is in pre-reg or in q, and is a UA, Hair, or DNA test
                    // and the report came in within x days of them registering - we have matched and we're done.
                    _logger.Debug("We found one result via Ordering provider - able to match");
                    UpdateSpecimenDetails(reportDetails, returnValues, trans, false);

                    return;
                }
                else
                {
                    // We got mutiple results
                    _logger.Debug("We found multiple results for ordering provider- unable to match");
                    returnValues.TestInfoRecordCount = recCount;
                    returnValues.DonorTestInfoId = 0;
                    returnValues.MROType = ClientMROTypes.None;
                }
            }
            _logger.Debug("Trying via test panel");
            //Using Test Panel Information
            if (returnValues.DonorId > 0
                && returnValues.ClientId == 0
                && returnValues.ClientDepartmentId == 0
                && returnValues.TestInfoRecordCount == 0
                && reportDetails.TestPanelCode != string.Empty)
            {
                sqlQuery = "SELECT "
                                + "donor_test_info.donor_test_info_id AS DonorTestInfoId, "
                                + "donor_test_info.mro_type_id AS MROTypeId "
                                + "FROM donor_test_info "
                                + "INNER JOIN donor_test_info_test_categories ON donor_test_info_test_categories.donor_test_info_id = donor_test_info.donor_test_info_id "
                                + "INNER JOIN test_panels ON test_panels.test_panel_id = donor_test_info_test_categories.test_panel_id "
                                + "WHERE test_status IN (3, 4) "
                                + "AND donor_test_info.donor_id = @DonorId "
                                + "AND DATE_ADD(DATE(@ReportDate), INTERVAL @DaysToAutoMatch DAY) <= DATE(test_requested_date) "
                                + "AND test_panels.test_panel_name = @TestPanelCode";

                param = new MySqlParameter[4];
                param[0] = new MySqlParameter("@ReportDate", reportDate);
                param[1] = new MySqlParameter("@DonorId", returnValues.DonorId);
                param[2] = new MySqlParameter("@DaysToAutoMatch", daysToAutoMatch);
                param[3] = new MySqlParameter("@TestPanelCode", reportDetails.TestPanelCode);

                MySqlDataReader dr = SqlHelper.ExecuteReader(trans, CommandType.Text, sqlQuery, param);

                while (dr.Read())
                {
                    returnValues.DonorTestInfoId = Convert.ToInt32(dr["DonorTestInfoId"]);
                    _logger.Debug($"Found a donor test info id. returnValues.DonorTestInfoId = {returnValues.DonorTestInfoId}");

                    returnValues.MROType = dr["MROTypeId"] != DBNull.Value ? (ClientMROTypes)(Convert.ToInt32(dr["MROTypeId"].ToString())) : ClientMROTypes.None;
                    recCount++;
                }
                dr.Close();

                if (recCount == 1)
                {
                    // if only one donor_test_info record matches donor id, test panel name, is in pre-reg or in q
                    // and the report came in within x days of them registering - we have matched and we're done.
                    _logger.Debug("We found one result via test panel- able to match");

                    UpdateSpecimenDetails(reportDetails, returnValues, trans,false);

                    return;
                }
                else
                {
                    returnValues.TestInfoRecordCount = recCount;
                    returnValues.DonorTestInfoId = 0;
                    returnValues.MROType = ClientMROTypes.None;
                    _logger.Debug("We found multiple results for test panel - unable to match");

                }
            }
            _logger.Debug("Trying by collection date");
            //Using Specimen Collection Date (Report Date & Cut off days)
            if (returnValues.DonorId > 0
                && returnValues.ClientId == 0
                && returnValues.ClientDepartmentId == 0
                && returnValues.TestInfoRecordCount == 0
                && (reportDetails.TestPanelCode == string.Empty || reportDetails.TestPanelCode == null))
            {
                sqlQuery = "SELECT "
                                + "donor_test_info.donor_test_info_id AS DonorTestInfoId, "
                                + "donor_test_info.mro_type_id AS MROTypeId "
                                + "FROM donor_test_info "
                                + "INNER JOIN donor_test_info_test_categories ON donor_test_info_test_categories.donor_test_info_id = donor_test_info.donor_test_info_id "
                                + "INNER JOIN test_panels ON test_panels.test_panel_id = donor_test_info_test_categories.test_panel_id "
                                + "WHERE test_status IN (3, 4) "
                                + "AND donor_test_info.donor_id = @DonorId "
                                + "AND DATE_ADD(DATE(@ReportDate), INTERVAL @DaysToAutoMatch DAY) <= DATE(test_requested_date)";

                param = new MySqlParameter[3];
                param[0] = new MySqlParameter("@ReportDate", reportDate);
                param[1] = new MySqlParameter("@DonorId", returnValues.DonorId);
                param[2] = new MySqlParameter("@DaysToAutoMatch", daysToAutoMatch);

                MySqlDataReader dr = SqlHelper.ExecuteReader(trans, CommandType.Text, sqlQuery, param);

                while (dr.Read())
                {
                    returnValues.DonorTestInfoId = Convert.ToInt32(dr["DonorTestInfoId"]);
                    _logger.Debug($"Found a donor test info id. returnValues.DonorTestInfoId = {returnValues.DonorTestInfoId}");

                    returnValues.MROType = dr["MROTypeId"] != DBNull.Value ? (ClientMROTypes)(Convert.ToInt32(dr["MROTypeId"].ToString())) : ClientMROTypes.None;
                    recCount++;
                }
                dr.Close();

                if (recCount == 1)
                {
                    // if only one donor_test_info record matches donor id, is in pre-reg or in q, and we got the report within x days of registration date.
                    // and the report came in within x days of them registering - we have matched and we're done.
                    _logger.Debug("We found one result by collection date- able to match");

                    UpdateSpecimenDetails(reportDetails, returnValues, trans,false);

                    return;
                }
                else
                {
                    _logger.Debug("We found multiple results for collection date - unable to match");

                    returnValues.TestInfoRecordCount = recCount;
                    returnValues.DonorTestInfoId = 0;
                    returnValues.MROType = ClientMROTypes.None;
                }

                // We were unable to match this up.
                _logger.Debug("No conditions meet or no matches identified. Returning");
            }
        }

        private void SetDonorTestInfoIdIntelligence(ReportInfo reportDetails, ReturnValues returnValues, MySqlTransaction trans)
        {
            _logger.Debug("SetDonorTestInfoIdIntelligence called");

            int daysToAutoMatch = Convert.ToInt32(ConfigurationManager.AppSettings["DaysToAutoMatch"].ToString().Trim()) * -1;
            string reportDate = reportDetails.LabReportDate.Substring(0, 4) + "-" + reportDetails.LabReportDate.Substring(4, 2) + "-" + reportDetails.LabReportDate.Substring(6, 2);
            string sqlQuery = string.Empty;
            MySqlParameter[] param = null;
            int recCount = 0;

            //If we have the donor test info ID already, go ahead and update the test records 
            if (returnValues.DonorId > 0
                && returnValues.ClientId > 0
                && returnValues.ClientDepartmentId > 0
                && returnValues.DonorTestInfoId > 0)
            {
                _logger.Debug($"SetDonorTestInfoIdIntelligence has the DonorTestInfoId, donorid, clientid, & ClientDepartmentId. Updating... {returnValues.DonorTestInfoId} {returnValues.DonorId} {returnValues.ClientId} {returnValues.ClientDepartmentId}");
                sqlQuery = "SELECT "
                               + "donor_test_info.donor_test_info_id AS DonorTestInfoId, "
                               + "donor_test_info.mro_type_id AS MROTypeId, "
                               + "donor_test_info.test_status AS TestStatus "
                               + "FROM donor_test_info "
                               + "INNER JOIN donor_test_info_test_categories ON donor_test_info_test_categories.donor_test_info_id = donor_test_info.donor_test_info_id "
                               + "WHERE "
                               // + "test_status IN (3, 4) AND "
                               + "donor_test_info.donor_test_info_id = @donor_test_info_id "
                               + "AND test_category_id in (1,2,3)"
                               + "AND DATE_ADD(DATE(@ReportDate), INTERVAL @DaysToAutoMatch DAY) <= DATE(test_requested_date) "
                               + "AND donor_test_info.donor_id = @DonorId "
                               + "AND donor_test_info.client_id = @ClientId "
                               + "AND donor_test_info.client_department_id = @ClientDepartmentId";

                param = new MySqlParameter[6];
                param[0] = new MySqlParameter("@ReportDate", reportDate);
                param[1] = new MySqlParameter("@DaysToAutoMatch", daysToAutoMatch);
                param[2] = new MySqlParameter("@DonorId", returnValues.DonorId);
                param[3] = new MySqlParameter("@donor_test_info_id", returnValues.DonorTestInfoId);
                param[4] = new MySqlParameter("@ClientId", returnValues.ClientId);
                param[5] = new MySqlParameter("@ClientDepartmentId", returnValues.ClientDepartmentId);

                MySqlDataReader dr = SqlHelper.ExecuteReader(trans, CommandType.Text, sqlQuery, param);

                while (dr.Read())
                {
                    returnValues.DonorTestInfoId = Convert.ToInt32(dr["DonorTestInfoId"]);
                    returnValues.MROType = dr["MROTypeId"] != DBNull.Value ? (ClientMROTypes)(Convert.ToInt32(dr["MROTypeId"].ToString())) : ClientMROTypes.None;
                    returnValues.TestStatus = (DonorRegistrationStatus)((int)dr["TestStatus"]);
                    recCount++;

                }
                dr.Close();

                if (recCount == 1)
                {
                    // if only one donor_test_info record matches donor id, client id, dept id, is in pre-reg or in q, and is a UA, Hair, or DNA test
                    // and the report came in within x days of them registering - we have matched and we're done.
                    if (returnValues.TestStatus == DonorRegistrationStatus.InQueue || returnValues.TestStatus == DonorRegistrationStatus.Registered)
                    {
                        _logger.Debug("We found one result in SetDonorTestInfoIdIntelligence - able to match");

                        UpdateSpecimenDetails(reportDetails, returnValues, trans,false);
                    }

                    return;
                }
                else if (recCount > 1)
                {
                    // We got mutiple results
                    // We should *never* get multiple results

                    returnValues.TestInfoRecordCount = recCount;
                    returnValues.DonorTestInfoId = 0;
                    returnValues.MROType = ClientMROTypes.None;
                    returnValues.TestStatus = DonorRegistrationStatus.None;
                }
                else
                {
                    // we got NO result


                }
            }
            else
            {
                _logger.Debug("We didn't have donor id, test info id, client, and dept id - doing nothing");
            }

        }
    }
}