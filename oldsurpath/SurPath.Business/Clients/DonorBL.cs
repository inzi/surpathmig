using Serilog;
using SurPath.Business.Master;
using SurPath.Data;
using SurPath.Entity;
using SurPath.Entity.Master;
using SurPath.Enum;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Net;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace SurPath.Business
{
    public class DonorBL : BusinessObject
    {
        private DonorDao donorDao; // = new DonorDao(ILogger _logger);
        public ILogger _logger;
        public ILogger Logger
        {
            get { return this._logger; }
            set
            {
                this._logger = value;
                if (this.donorDao != null)
                {
                    this.donorDao.Logger = value;
                    this.donorDao.SessionID = this.__sessionid;
                }
            }
        }

        private string __sessionid;

        public string SessionID
        {
            get { return this.__sessionid; }
            set
            {
                this.__sessionid = value;
                if (this.donorDao != null)
                {
                    this.donorDao.SessionID = this.__sessionid;
                }
            }
        }

        private ClientDao clientDao = new ClientDao();

        public DonorBL(ILogger __logger = null, string _SessionID = "")
        {
            this.SessionID = _SessionID;

            if (__logger == null)
            {
                this.Logger = new LoggerConfiguration().CreateLogger();
            }
            else
            {
                this.Logger = __logger;
                logDebug($"DonorBL - SessionID: {this.SessionID.ToString()}");
            }
            this.donorDao = new DonorDao(_logger, this.SessionID);
        }

        public int Save(Donor donor)
        {
            if (donor.DonorId == 0)
            {
                // are we getting a donorid of 0 here? Looks like it...
                return donorDao.Insert(donor);
            }
            else
            {
                return donorDao.Update(donor);
            }
        }
        public void UpdateDonorPids(Donor donor)
        {
            donorDao.UpdateDonorPids(donor);
        }

        public void UpdateTestToArchive(string SpeciminId)
        {
            donorDao.UpdateReportInfotoArchive(SpeciminId);
        }

        public void UpdateTestStatustoProcessing(int DonorId)
        {
            donorDao.UpdateTestStatusToProcessing(DonorId);
        }

        public Donor Get(int donorId, string source)
        {

            _logger.Debug($"Donor Get {donorId} {source}");
            Donor donor = null;
            try
            {
                if (source.ToUpper() == "DESKTOP")
                {
                    donor = donorDao.Get(donorId);
                }
                else
                {
                    donor = donorDao.Get(donorId);
                    //donor = donorDao.GetWeb(donorId);
                }
                return donor;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                throw;
            }
        }

        /// <summary>
        /// This will use FLSD to match to a donor during registration, and if a match is found, that donor_id will be used.
        /// </summary>
        /// <param name="donor_first_name"></param>
        /// <param name="donor_last_name"></param>
        /// <param name="donor_ssn"></param>
        /// <param name="donor_date_of_birth"></param>
        /// <returns></returns>
        public int MatchDonorIdByDetails(string donor_first_name, string donor_last_name, string donor_ssn, string donor_date_of_birth)
        {
            int retval = 0;
            try
            {
                retval = donorDao.GetDonorIDByDetails(donor_first_name, donor_last_name, donor_ssn, donor_date_of_birth);
            }
            catch (Exception ex)
            {
            }
            return retval;
        }

        public Donor GetBySSN(string ssn, string source)
        {
            Donor donor = null;
            try
            {
                if (source.ToUpper() == "DESKTOP")
                {
                    donor = donorDao.GetBySSN(ssn);
                }
                else
                {
                    donor = donorDao.GetBySSN(ssn);
                    //donor = donorDao.GetBySSNWeb(ssn);
                }

                return donor;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                throw;
            }
        }

        public Donor GetByPID(string pid)
        {
            try
            {
                Donor donor = donorDao.GetByPID(pid);

                return donor;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                throw;
            }
        }
        public Donor GetByPIDAndEmail(string email, string pid)
        {
            try
            {
                Donor donor = donorDao.GetByPIDAndEmail(email, pid);

                return donor;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                throw;
            }
        }

        

        public Donor GetByLoginAndProgram(string donor_email, string department_name)
        {
            try
            {
                Donor donor = donorDao.GetByLoginAndProgram(donor_email, department_name);

                return donor;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                throw;
            }
        }

        public Donor GetByEmail(string email)
        {
            try
            {
                Donor donor = donorDao.GetByEmail(email);

                return donor;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                throw;
            }
        }

        public DonorTestInfo GetDonorTestInfoByDonorId(int donorId)
        {
            try
            {
                DonorTestInfo donorTestInfo;
                donorTestInfo = donorDao.GetDonorTestInfoByDonorId(donorId);

                return donorTestInfo;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                throw;
            }
        }

        public DonorTestInfo GetDonorTestInfo(int donorId)
        {
            try
            {
                donorDao.Logger = _logger;
                DonorTestInfo donorTestInfo = donorDao.GetDonorTestInfo(donorId);

                return donorTestInfo;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                throw;
            }
        }

        public int AddDonor(Donor donor, ref int donorTestInfoId)
        {
            try
            {
                //Username / Email validation
                UserDao userDao = new UserDao();
                //if (donor.DonorEmail != string.Empty)
                //{
                //    User users = userDao.GetByUsernameOrEmail(donor.DonorEmail);
                //    if (users != null)
                //    {
                //        throw new Exception("The email address already exists.");
                //    }
                //}
                //

                RandomStringGenerator rsg = new RandomStringGenerator(true, false, true, false);
                string newPassword = rsg.Generate(6, 8);
                string newPasswordEncrypted = UserAuthentication.Encrypt(newPassword, true);

                User user = new User();

                user.Username = donor.DonorEmail;
                user.UserPassword = newPasswordEncrypted;
                user.IsUserActive = true;
                user.UserFirstName = donor.DonorFirstName;
                user.UserLastName = donor.DonorLastName;
                user.UserPhoneNumber = donor.DonorPhone1;
                user.UserFax = null;
                user.UserEmail = donor.DonorEmail;
                user.ChangePasswordRequired = true;
                user.UserType = Enum.UserType.Donor;
                user.DepartmentId = null;
                user.DonorId = null;
                user.ClientId = null;
                user.VendorId = null;
                user.AttorneyId = null;
                user.CourtId = null;
                user.JudgeId = null;
                user.CreatedBy = donor.CreatedBy;
                user.LastModifiedBy = donor.LastModifiedBy;

                ClientBL clientBL = new ClientBL();
                ClientDepartment clientDepartment = clientBL.GetClientDepartment(Convert.ToInt32(donor.DonorInitialDepartmentId));

                Client client = clientBL.Get(clientDepartment.ClientId);

                UserBL userBL = new UserBL();
                User currentUser = userBL.GetByUsernameOrEmail(donor.CreatedBy);

                int donorId = donorDao.AddDonor(donor, user, clientDepartment, currentUser.UserId, ref donorTestInfoId);
                donor.DonorId = donorId;

                //Donor Registration Mail
                try
                {
                    MailManager mailManager = new MailManager();
                    string mailBody = mailManager.SendRegistrationMail(user, "Registration");

                    if (donor.DonorEmail != string.Empty)
                    {
                        mailManager.SendMail(donor.DonorEmail, ConfigurationManager.AppSettings["RegistrationMailSubject"].ToString().Trim(), mailBody);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Not able to send mail.");
                }
                //Donor Program Registration Mail
                if (clientDepartment.PaymentTypeId == ClientPaymentTypes.DonorPays)
                {
                    try
                    {
                        MailManager mailManager = new MailManager();
                        string mailBody = mailManager.SendDonorProgramRegistrationMail(donor, client, clientDepartment);
                        if (donor.DonorEmail != string.Empty)
                        {
                            mailManager.SendMail(donor.DonorEmail, ConfigurationManager.AppSettings["ProgramRegistraionMailSubject"].ToString().Trim(), mailBody);
                        }
                    }
                    catch
                    {
                        throw new Exception("Not able to send mail.");
                    }
                }

                ////Client Program Registration Mail
                //try
                //{
                //    MailManager mailManager = new MailManager();
                //    string mailBody = mailManager.SendClientProgramRegistrationMail(donor, client, clientDepartment);
                //    if (client.ClientEmail != string.Empty)
                //    {
                //        mailManager.SendMail(clientDepartment.ClientEmail, ConfigurationManager.AppSettings["ProgramRegistraionMailSubject"].ToString().Trim(), mailBody);
                //    }
                //}
                //catch (Exception ex)
                //{
                //    throw new Exception("Not able to send mail.");
                //}

                //Client Program Registration Mail
                try
                {
                    MailManager mailManager = new MailManager();
                    string mailBody = mailManager.SendClientProgramRegistrationMail(donor, client, clientDepartment);
                    //string toEmail = string.Empty;

                    if (client.ClientEmail != string.Empty)
                    {
                        // mailManager.SendMail(client.ClientEmail, ConfigurationManager.AppSettings["ProgramRegistraionMailSubject"].ToString().Trim(), mailBody);
                    }

                    //if (clientDepartment.IsPhysicalAddressAsClient)
                    //{
                    //    toEmail = client.ClientEmail;
                    //}
                    //else
                    //{
                    //    toEmail = clientDepartment.ClientEmail;
                    //}

                    //if (toEmail != string.Empty)
                    //{
                    //    mailManager.SendMail(toEmail, ConfigurationManager.AppSettings["ProgramRegistraionMailSubject"].ToString().Trim(), mailBody);
                    //}
                }
                catch
                {
                    throw new Exception("Not able to send mail.");
                }

                //TPA Program Registration Mail
                try
                {
                    MailManager mailManager = new MailManager();
                    string mailBody = mailManager.SendTPAProgramRegistrationMail(donor, client, clientDepartment);
                    mailManager.SendMail(ConfigurationManager.AppSettings["TPAProgramToMailAddress"].ToString().Trim(), ConfigurationManager.AppSettings["ProgramRegistraionMailSubject"].ToString().Trim(), mailBody);
                }
                catch
                {
                    throw new Exception("Not able to send mail.");
                }

                return donorId;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                throw;
            }
        }

        public int AddTest(Donor donor, int clientDepartmentId, ref int donorTestInfoId)
        {
            try
            {
                ClientBL clientBL = new ClientBL();
                ClientDepartment clientDepartment = clientBL.GetClientDepartment(clientDepartmentId);

                Client client = clientBL.Get(clientDepartment.ClientId);

                UserBL userBL = new UserBL();
                User currentUser = userBL.GetByUsernameOrEmail(donor.LastModifiedBy);

                int returnValue = donorDao.AddTest(donor, clientDepartment, currentUser.UserId, ref donorTestInfoId);

                //Donor Program Registration Mail
                if (clientDepartment.PaymentTypeId == ClientPaymentTypes.DonorPays)
                {
                    try
                    {
                        MailManager mailManager = new MailManager();
                        string mailBody = mailManager.SendDonorProgramRegistrationMail(donor, client, clientDepartment);
                        if (donor.DonorEmail != string.Empty)
                        {
                            mailManager.SendMail(donor.DonorEmail, ConfigurationManager.AppSettings["ProgramRegistraionMailSubject"].ToString().Trim(), mailBody);
                        }
                    }
                    catch
                    {
                        throw new Exception("Not able to send mail.");
                    }
                }

                //Client Program Registration Mail
                try
                {
                    MailManager mailManager = new MailManager();
                    string mailBody = mailManager.SendClientProgramRegistrationMail(donor, client, clientDepartment);
                    //  string toEmail = string.Empty;
                    if (client.ClientEmail != string.Empty)
                    {
                        //mailManager.SendMail(client.ClientEmail, ConfigurationManager.AppSettings["ProgramRegistraionMailSubject"].ToString().Trim(), mailBody);
                    }

                    //if (clientDepartment.IsPhysicalAddressAsClient)
                    //{
                    //    toEmail = client.ClientEmail;
                    //}
                    //else
                    //{
                    //    toEmail = clientDepartment.ClientEmail;
                    //}

                    //if (toEmail != string.Empty)
                    //{
                    //    mailManager.SendMail(toEmail, ConfigurationManager.AppSettings["ProgramRegistraionMailSubject"].ToString().Trim(), mailBody);
                    //}
                }
                catch
                {
                    throw new Exception("Not able to send mail.");
                }

                //TPA Program Registration Mail
                try
                {
                    MailManager mailManager = new MailManager();
                    string mailBody = mailManager.SendTPAProgramRegistrationMail(donor, client, clientDepartment);
                    mailManager.SendMail(ConfigurationManager.AppSettings["TPAProgramToMailAddress"].ToString().Trim(), ConfigurationManager.AppSettings["ProgramRegistraionMailSubject"].ToString().Trim(), mailBody);
                }
                catch
                {
                    throw new Exception("Not able to send mail.");
                }

                return returnValue;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                throw;
            }
        }

        public Donor AddTest(int donorId, int clientDepartmentId, int currentUserId)
        {
            try
            {
                ClientBL clientBL = new ClientBL();
                ClientDepartment clientDepartment = clientBL.GetClientDepartment(clientDepartmentId);

                UserBL userBL = new UserBL();
                User currentUser = userBL.Get(currentUserId);

                Donor donor = Get(donorId, "Desktop");

                double returnValue = donorDao.AddTest(donor, clientDepartment, currentUser);

                donor.ProgramAmount = returnValue;
                donor.ClientDepartment = clientDepartment;
                return donor;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                throw;
            }
        }

        public int DoDonorPreRegisteration(Donor donor, string clientCode)
        {
            try
            {
                //Username / Email validation
                UserDao userDao = new UserDao();
                User user = userDao.GetByUsernameOrEmail(donor.DonorEmail);
                //if (user != null)
                //{
                //    throw new Exception("The email address already exists.");
                //}

                ClientBL clientBL = new ClientBL();

                Client client = clientBL.Get(clientCode.Trim());

                if (client == null)
                {
                    throw new Exception("Client Code does not exists.");
                }

                donor.DonorInitialClientId = client.ClientId;
                var _pw = donor.TemporaryPassword;
                RandomStringGenerator rsg = new RandomStringGenerator(true, false, true, false);
                string newPassword = rsg.Generate(6, 8);
                if (!(string.IsNullOrEmpty(_pw))) newPassword = _pw;
                string newPasswordEncrypted = UserAuthentication.Encrypt(newPassword, true);

                user = new User();

                user.Username = donor.DonorEmail;
                user.UserPassword = newPasswordEncrypted;
                user.IsUserActive = true;
                user.UserFirstName = donor.DonorFirstName;
                user.UserLastName = donor.DonorLastName;
                user.UserPhoneNumber = donor.DonorPhone1;
                user.UserFax = null;
                user.UserEmail = donor.DonorEmail;
                user.ChangePasswordRequired = true;
                user.UserType = Enum.UserType.Donor;
                user.DepartmentId = null;
                user.DonorId = null;
                user.ClientId = null;
                user.VendorId = null;
                user.AttorneyId = null;
                user.CourtId = null;
                user.JudgeId = null;
                user.CreatedBy = donor.CreatedBy;
                user.LastModifiedBy = donor.LastModifiedBy;


                donor.TemporaryPassword = newPassword;


                int donorId = donorDao.DoDonorPreRegisteration(donor, user);


                donor.DonorId = donorId;

                return donorId;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                throw;
            }
        }

        public void DoDonorInQueueUpdate(int donorId, string oldPassword, string newPassword, string status, bool GetUserByDonorId = false)
        {
            try
            {
                Donor donor = Get(donorId, "Desktop");

                if (donor == null)
                {
                    throw new Exception("User does not exists.");
                }

                UserBL userBL = new UserBL();
                User user;
                if (GetUserByDonorId)
                {
                    user = userBL.GetByDonorId(donorId);
                }
                else
                {
                    user = userBL.GetByUsernameOrEmail(donor.DonorEmail);
                }

                if (user != null)
                {
                    if (user.UserPassword != UserAuthentication.Encrypt(oldPassword, true))
                    {
                        throw new Exception("The old password does not match with the database value.");
                    }
                }

                // Prevent downgrading of donor record status
                //DonorRegistrationStatus donorRegistrationStatus = donor.DonorRegistrationStatusValue;
                //DonorRegistrationStatus DonorRegistrationStatusNew = (DonorRegistrationStatus)System.Enum.Parse(typeof(DonorRegistrationStatus), status);
                if ((int)donor.DonorRegistrationStatusValue >= (int)DonorRegistrationStatus.Registered)
                {
                    status = donor.DonorRegistrationStatusValue.ToString();
                }

                donorDao.DoDonorInQueueUpdate(donorId, UserAuthentication.Encrypt(newPassword, true), status);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                throw;
            }
        }

        public Donor DoDonorRegistrationTestRequest(int donorId, int clientDepartmentId)
        {
            try
            {
                logDebug($"DoDonorRegistrationTestRequest - donorId {donorId}, clientDepartmentId {clientDepartmentId}");
                Donor donor = donorDao.Get(donorId);
                logDebug($"Got donor {donor.DonorId}");

                ClientBL clientBL = new ClientBL();
                ClientDepartment clientDepartment;
                clientDepartment = clientBL.GetClientDepartment(clientDepartmentId);
                if (clientDepartment != null)
                {
                    logDebug($"Got clientDepartment {clientDepartment.ClientDepartmentId} {clientDepartment.DepartmentName}");

                    donor.ClientDepartment = clientDepartment;
                    logDebug($"Set donor ClientDepartment");

                    if (clientDepartment.ClientId == donor.DonorInitialClientId)
                    {
                        string strReturnValue = donorDao.DoDonorRegistrationTestRequest(donorId, donor.DonorEmail, clientDepartment);

                        string[] strResult = strReturnValue.Split(',');
                        if (strResult.Length == 2)
                        {
                            donor.DonorTestInfoId = Convert.ToInt32(strResult[0]);
                            donor.ProgramAmount = Convert.ToDouble(strResult[1]);

                        }

                    }
                    else
                    {
                        logDebug($"Program does not exist error {clientDepartment.ClientId.ToString()} {donor.DonorInitialClientId.ToString()}");

                        throw new Exception("Program does not exists.");
                    }
                }
                else
                {
                    logDebug($"Program does not exist error - clientDepartment is null");

                    throw new Exception("Program does not exists.");
                }

                return donor;
            }
            catch (Exception ex)
            {
                LogAnError(ex);
                _logger.Error(ex.ToString());
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                throw;
                
            }
        }

        /// <summary>
        /// Get Test Request info totals without a donorid, for preregistration pre DB calls
        /// </summary>
        /// <param name="donor"></param>
        /// <param name="clientDepartmentId"></param>
        /// <returns></returns>
        public void DoDonorPreRegistrationTestRequest(Donor donor, int clientDepartmentId)
        {
            bool CalculateOnly = true;
            try
            {
                ClientBL clientBL = new ClientBL();
                ClientDepartment clientDepartment;
                clientDepartment = clientBL.GetClientDepartment(clientDepartmentId);

                if (clientDepartment != null)
                {
                    donor.ClientDepartment = clientDepartment;
                    if (clientDepartment.ClientId == donor.DonorInitialClientId)
                    {
                        string strReturnValue = donorDao.DoDonorRegistrationTestRequest(0, donor.DonorEmail, clientDepartment, CalculateOnly);

                        string[] strResult = strReturnValue.Split(',');
                        if (strResult.Length == 2)
                        {
                            donor.DonorTestInfoId = Convert.ToInt32(strResult[0]);
                            donor.ProgramAmount = Convert.ToDouble(strResult[1]);
                        }
                    }
                    else
                    {
                        throw new Exception("Program does not exists.");
                    }
                }
                else
                {
                    throw new Exception("Program does not exists.");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                throw;
            }
        }

        public List<Donor> GetList(Dictionary<string, string> searchParam)
        {
            try
            {
                DataTable dtDonor = donorDao.GetList(searchParam);

                List<Donor> donorList = new List<Donor>();
                foreach (DataRow dr in dtDonor.Rows)
                {
                    Donor donor = new Donor();

                    donor.DonorId = (int)dr["DonorId"];
                    donor.DonorFirstName = (string)dr["DonorFirstName"];
                    donor.DonorMI = dr["DonorMI"].ToString();
                    donor.DonorLastName = (string)dr["DonorLastName"];
                    donor.DonorSuffix = dr["DonorSuffix"].ToString();
                    donor.DonorSSN = dr["DonorSSN"].ToString();
                    donor.DonorDateOfBirth = (DateTime)dr["DonorDateOfBirth"];
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

                    donorList.Add(donor);
                }

                return donorList;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                throw;
            }
        }

        public List<DonorTestInfo> GetDonorPaymentList(Dictionary<string, string> searchParam)
        {
            try
            {
                DataTable dtDonorTestInfo = donorDao.GetDonorPaymentList(searchParam);

                List<DonorTestInfo> donorList = new List<DonorTestInfo>();
                foreach (DataRow dr in dtDonorTestInfo.Rows)
                {
                    DonorTestInfo donorTestInfo = new DonorTestInfo();

                    donorTestInfo.DonorId = (int)dr["DonorId"];
                    if (dr["TotalPaymentAmount"] != DBNull.Value)
                    {
                        donorTestInfo.TotalPaymentAmount = Convert.ToDouble(dr["TotalPaymentAmount"]);
                    }
                    donorTestInfo.PaymentMethodId = dr["PaymentMethodId"] != DBNull.Value ? (PaymentMethod)(Convert.ToInt32(dr["PaymentMethodId"].ToString())) : PaymentMethod.None;

                    donorTestInfo.PaymentStatus = dr["PaymentStatus"] != DBNull.Value ? (PaymentStatus)(Convert.ToInt32(dr["PaymentStatus"].ToString())) : PaymentStatus.None;
                    if (dr["ScheduleDate"] != DBNull.Value)
                    {
                        donorTestInfo.ScheduleDate = Convert.ToDateTime(dr["ScheduleDate"]);
                    }

                    if (dr["DonorFirstName"] != string.Empty)
                    {
                        donorTestInfo.DonorFirstName = dr["DonorFirstName"].ToString();
                    }
                    else
                    {
                        donorTestInfo.DonorFirstName = null;
                    }
                    if (dr["DonorLastName"] != string.Empty)
                    {
                        donorTestInfo.DonorLastName = dr["DonorLastName"].ToString();
                    }
                    else
                    {
                        donorTestInfo.DonorLastName = null;
                    }

                    donorTestInfo.CreatedOn = (DateTime)dr["CreatedOn"];
                    donorTestInfo.CreatedBy = (string)dr["CreatedBy"];
                    donorTestInfo.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    donorTestInfo.LastModifiedBy = (string)dr["LastModifiedBy"];

                    donorList.Add(donorTestInfo);
                }

                return donorList;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                throw;
            }
        }

        public DataTable DonorSearch(Dictionary<string, string> searchParam)
        {
            try
            {
                return donorDao.DonorSearch(searchParam);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                throw;
            }
        }

        public DataTable SearchDonor(Dictionary<string, string> searchParam, UserType userType, int currentUserId, string currentUserName, bool showAll)
        {
            try
            {
                donorDao._logger = _logger;
                return donorDao.SearchDonor(searchParam, userType, currentUserId, currentUserName, showAll);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                throw;
            }
        }

        public DataTable SearchDonorFromTestHistory(Dictionary<string, string> searchParam, UserType userType, int currentUserId, string currentUserName = null)
        {
            try
            {
                return donorDao.SearchDonorFromTestHistory(searchParam, userType, currentUserId, currentUserName);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                throw;
            }
        }

        //Starts here

        public DataTable SearchFromVendorDashboard(Dictionary<string, string> searchParam)
        {
            try
            {
                return donorDao.SearchFromVendorDashboard(searchParam);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                throw;
            }
        }

        //end hear

        public DataTable GetDonorsByAttorney(int attorneyId)
        {
            try
            {
                return donorDao.GetDonorsByAttorney(attorneyId);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                throw;
            }
        }

        public DataTable GetDonorsByCourt(int courtId)
        {
            try
            {
                return donorDao.GetDonorsByCourt(courtId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetDonorsByJudge(int judgeId)
        {
            try
            {
                return donorDao.GetDonorsByJudge(judgeId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<DonorActivityNote> GetDonorActivityList(int donorTestInfoId)
        {
            try
            {
                DataTable dtClients = donorDao.GetDonorActivityList(donorTestInfoId);

                List<DonorActivityNote> donorActivityNoteList = new List<DonorActivityNote>();

                foreach (DataRow dr in dtClients.Rows)
                {
                    DonorActivityNote donorActivityNote = new DonorActivityNote();

                    donorActivityNote.DonorTestActivityId = (int)dr["DonorTestActivityId"];
                    donorActivityNote.DonorTestInfoId = (int)dr["DonorTestInfoId"];
                    donorActivityNote.ActivityDateTime = (DateTime)dr["ActivityDateTime"];
                    donorActivityNote.ActivityUserId = (int)dr["ActivityUserId"];
                    donorActivityNote.ActivityCategoryId = (DonorActivityCategories)((int)dr["ActivityCategoryId"]);
                    donorActivityNote.IsActivityVisible = dr["IsActivityVisible"].ToString() == "1" ? true : false;
                    donorActivityNote.ActivityNote = (string)dr["ActivityNote"];
                    donorActivityNote.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    donorActivityNote.ActivityUserName = (string)dr["ActivityUserName"];

                    donorActivityNoteList.Add(donorActivityNote);
                }

                return donorActivityNoteList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SavePaymentDetails(DonorTestInfo donorTestInfo, string fromWeb = null, PaymentOverride paymentOverride = null)
        {
            try
            {
                if (donorTestInfo.TestStatus == Enum.DonorRegistrationStatus.Registered)
                {
                    donorTestInfo.TestStatus = Enum.DonorRegistrationStatus.InQueue;
                }
                donorTestInfo.PaymentStatus = Enum.PaymentStatus.Paid;
                donorTestInfo.IsPaymentReceived = true;

                if (fromWeb == "Yes")
                {
                    if (donorTestInfo.PaymentMethodId == PaymentMethod.Cash)
                    {
                        donorTestInfo.TestStatus = Enum.DonorRegistrationStatus.Registered;
                        donorTestInfo.PaymentStatus = Enum.PaymentStatus.None;
                        donorTestInfo.IsPaymentReceived = false;
                    }
                }

                donorDao.SavePaymentDetails(donorTestInfo, fromWeb, paymentOverride);

                ////Payment Confirmation Mail

                DonorBL donorBL = new DonorBL();

                ClientBL clientBL = new ClientBL();
                Donor donor = donorBL.Get(donorTestInfo.DonorId, "Web");

                ClientDepartment clientDepartment = clientBL.GetClientDepartment(Convert.ToInt32(donorTestInfo.ClientDepartmentId));

                Client client = clientBL.Get(clientDepartment.ClientId);
                if (string.IsNullOrEmpty(fromWeb))
                {
                    if (fromWeb != "Yes")
                    {
                        try
                        {
                            MailManager mailManager = new MailManager();
                            string mailBody = mailManager.SendDonorPaymentConfirmationMail(donor, donorTestInfo, clientDepartment, client);
                            mailManager.SendMail(donor.DonorEmail, ConfigurationManager.AppSettings["PaymentConformationMailSubject"].ToString().Trim(), mailBody);
                        }
                        catch
                        {
                            throw new Exception("Not able to send mail.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (donorTestInfo.TestStatus == Enum.DonorRegistrationStatus.InQueue)
                {
                    donorTestInfo.TestStatus = Enum.DonorRegistrationStatus.Registered;
                }
                donorTestInfo.PaymentStatus = Enum.PaymentStatus.None;

                throw ex;
            }
        }

        public void SaveReversePaymentDetails(DonorTestInfo donorTestInfo)
        {
            try
            {
                if (donorTestInfo.TestStatus == Enum.DonorRegistrationStatus.Processing)
                {
                    donorTestInfo.TestStatus = Enum.DonorRegistrationStatus.Processing;
                    donorTestInfo.PaymentStatus = Enum.PaymentStatus.Paid;
                    donorTestInfo.IsPaymentReceived = true;
                }
                else if (donorTestInfo.TestStatus == Enum.DonorRegistrationStatus.Completed)
                {
                    donorTestInfo.TestStatus = Enum.DonorRegistrationStatus.Completed;
                    donorTestInfo.PaymentStatus = Enum.PaymentStatus.Paid;
                    donorTestInfo.IsPaymentReceived = true;
                }
                //if (fromWeb == "Yes")
                //{
                //    if (donorTestInfo.PaymentMethodId == PaymentMethod.Cash)
                //    {
                //        donorTestInfo.TestStatus = Enum.DonorRegistrationStatus.Registered;
                //        donorTestInfo.PaymentStatus = Enum.PaymentStatus.None;
                //        donorTestInfo.IsPaymentReceived = false;
                //    }
                //}

                donorDao.SaveReversePaymentDetails(donorTestInfo);
            }
            catch (Exception ex)
            {
                //if (donorTestInfo.TestStatus == Enum.DonorRegistrationStatus.InQueue)
                //{
                //    donorTestInfo.TestStatus = Enum.DonorRegistrationStatus.Registered;
                //}
                donorTestInfo.TestStatus = Enum.DonorRegistrationStatus.Processing;
                donorTestInfo.PaymentStatus = Enum.PaymentStatus.None;

                throw ex;
            }
        }

        public void SaveTestInfoDetails(DonorTestInfo donorTestInfo)
        {
            try
            {
                ClientBL clientBL = new ClientBL();

                int hairTestPanelDays = 90;

                foreach (DonorTestInfoTestCategories testCategory in donorTestInfo.TestInfoTestCategories)
                {
                    if (testCategory.TestCategoryId == TestCategories.Hair && testCategory.HairTestPanelDays != null)
                    {
                        hairTestPanelDays = Convert.ToInt32(testCategory.HairTestPanelDays);

                        if (donorTestInfo.PaymentStatus != PaymentStatus.Paid
                            && hairTestPanelDays > 90)
                        {
                            ClientDepartment clientDepartment = clientBL.GetClientDepartment(donorTestInfo.ClientDepartmentId);
                            if (clientDepartment != null)
                            {
                                foreach (ClientDeptTestCategory clientDeptTestCategory in clientDepartment.ClientDeptTestCategories)
                                {
                                    if (clientDeptTestCategory.TestCategoryId == TestCategories.Hair)
                                    {
                                        if (clientDeptTestCategory.ClientDeptTestPanels.Count == 1)
                                        {
                                            testCategory.TestPanelPrice = Convert.ToDouble(clientDeptTestCategory.ClientDeptTestPanels[0].TestPanelPrice) * (hairTestPanelDays / 90);
                                        }
                                        break;
                                    }
                                }
                            }
                        }

                        break;
                    }
                }

                if (donorTestInfo.PaymentStatus != PaymentStatus.Paid
                            && hairTestPanelDays > 90)
                {
                    double totalPaymentAmount = 0.0;

                    foreach (DonorTestInfoTestCategories testCategory in donorTestInfo.TestInfoTestCategories)
                    {
                        if (testCategory.TestPanelPrice != null)
                        {
                            totalPaymentAmount += Convert.ToDouble(testCategory.TestPanelPrice);
                        }
                    }
                    donorTestInfo.TotalPaymentAmount = totalPaymentAmount;
                }

                if (donorTestInfo.TestStatus == DonorRegistrationStatus.Processing && donorTestInfo.CollectionSiteVendorId != null)
                {
                    double totalVendorCost = 0.0;
                    VendorServiceBL vendorServiceBL = new VendorServiceBL();
                    foreach (DonorTestInfoTestCategories testCategory in donorTestInfo.TestInfoTestCategories)
                    {
                        VendorService vendorService = vendorServiceBL.GetVendorServiceByCostParam(Convert.ToInt32(donorTestInfo.CollectionSiteVendorId), (int)testCategory.TestCategoryId, donorTestInfo.IsObserved, donorTestInfo.FormTypeId);
                        if (vendorService != null)
                        {
                            totalVendorCost += vendorService.Cost;
                        }
                    }
                    donorTestInfo.VendorCost = totalVendorCost;
                }

                donorDao.SaveTestInfoDetails(donorTestInfo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SaveReverseEntryTestInfoDetails(DonorTestInfo donorTestInfo, Donor donor)
        {
            try
            {
                ClientBL clientBL = new ClientBL();

                int hairTestPanelDays = 90;

                foreach (DonorTestInfoTestCategories testCategory in donorTestInfo.TestInfoTestCategories)
                {
                    if (testCategory.TestCategoryId == TestCategories.Hair && testCategory.HairTestPanelDays != null)
                    {
                        hairTestPanelDays = Convert.ToInt32(testCategory.HairTestPanelDays);

                        if (donorTestInfo.PaymentStatus != PaymentStatus.Paid
                            && hairTestPanelDays > 90)
                        {
                            ClientDepartment clientDepartment = clientBL.GetClientDepartment(donorTestInfo.ClientDepartmentId);
                            if (clientDepartment != null)
                            {
                                foreach (ClientDeptTestCategory clientDeptTestCategory in clientDepartment.ClientDeptTestCategories)
                                {
                                    if (clientDeptTestCategory.TestCategoryId == TestCategories.Hair)
                                    {
                                        if (clientDeptTestCategory.ClientDeptTestPanels.Count == 1)
                                        {
                                            testCategory.TestPanelPrice = Convert.ToDouble(clientDeptTestCategory.ClientDeptTestPanels[0].TestPanelPrice) * (hairTestPanelDays / 90);
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                        break;
                    }
                }

                if (donorTestInfo.PaymentStatus != PaymentStatus.Paid
                            && hairTestPanelDays > 90)
                {
                    double totalPaymentAmount = 0.0;

                    foreach (DonorTestInfoTestCategories testCategory in donorTestInfo.TestInfoTestCategories)
                    {
                        if (testCategory.TestPanelPrice != null)
                        {
                            totalPaymentAmount += Convert.ToDouble(testCategory.TestPanelPrice);
                        }
                    }
                    donorTestInfo.TotalPaymentAmount = totalPaymentAmount;
                }

                //if (donorTestInfo.TestStatus == DonorRegistrationStatus.Processing && donorTestInfo.CollectionSiteVendorId != null)
                //{
                double totalVendorCost = 0.0;
                VendorServiceBL vendorServiceBL = new VendorServiceBL();
                foreach (DonorTestInfoTestCategories testCategory in donorTestInfo.TestInfoTestCategories)
                {
                    VendorService vendorService = vendorServiceBL.GetVendorServiceByCostParam(Convert.ToInt32(donorTestInfo.CollectionSiteVendorId), (int)testCategory.TestCategoryId, donorTestInfo.IsObserved, donorTestInfo.FormTypeId);
                    if (vendorService != null)
                    {
                        totalVendorCost += vendorService.Cost;
                    }
                }
                donorTestInfo.VendorCost = totalVendorCost;
                //}

                //UserDao userDao = new UserDao();
                //if (donor.DonorEmail != string.Empty && donor.DonorId!=null)
                //{
                //  User users = userDao.GetDonor(donor.DonorId);
                //    if (users != null)
                //    {
                //        throw new Exception("The user already exists.");
                //    }
                //}

                RandomStringGenerator rsg = new RandomStringGenerator(true, false, true, false);
                string newPassword = rsg.Generate(6, 8);
                string newPasswordEncrypted = UserAuthentication.Encrypt(newPassword, true);

                User user = new User();

                user.Username = donor.DonorEmail;
                user.UserPassword = newPasswordEncrypted;
                user.IsUserActive = true;
                user.UserFirstName = donor.DonorFirstName;
                user.UserLastName = donor.DonorLastName;
                user.UserPhoneNumber = donor.DonorPhone1;
                user.UserFax = null;
                user.UserEmail = donor.DonorEmail;
                user.ChangePasswordRequired = false;
                user.UserType = Enum.UserType.Donor;
                user.DepartmentId = null;
                if (donor.DonorId != null)
                {
                    user.DonorId = donor.DonorId;
                }
                else
                {
                    user.DonorId = null;
                }
                user.ClientId = null;
                user.VendorId = null;
                user.AttorneyId = null;
                user.CourtId = null;
                user.JudgeId = null;
                user.CreatedBy = donor.CreatedBy;
                user.LastModifiedBy = donor.LastModifiedBy;
                UserBL userBL = new UserBL();
                User currentUser = userBL.GetByUsernameOrEmail(donor.CreatedBy);

                donorDao.SaveReverseEntryTestInfoDetails(donorTestInfo, donor, user);

                //Donor Registration Mail
                try
                {
                    MailManager mailManager = new MailManager();
                    string mailBody = mailManager.SendRegistrationMail(user, "Registration");

                    if (donor.DonorEmail != string.Empty)
                    {
                        mailManager.SendMail(donor.DonorEmail, ConfigurationManager.AppSettings["RegistrationMailSubject"].ToString().Trim(), mailBody);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Not able to send mail.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SaveTestInfoDetailsBeforPayment(DonorTestInfo donorTestInfo)
        {
            try
            {
                donorDao.SaveTestInfoDetailsBeforPayment(donorTestInfo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateTestInfoSpecimenIDs(DonorTestInfo donorTestInfo)
        {
            try
            {
                donorDao.UpdateTestInfoSpecimenIDs(donorTestInfo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateHairTestPanelDays(DonorTestInfo donorTestInfo)
        {
            try
            {
                ClientBL clientBL = new ClientBL();

                int hairTestPanelDays = 90;

                foreach (DonorTestInfoTestCategories testCategory in donorTestInfo.TestInfoTestCategories)
                {
                    if (testCategory.TestCategoryId == TestCategories.Hair && testCategory.HairTestPanelDays != null)
                    {
                        hairTestPanelDays = Convert.ToInt32(testCategory.HairTestPanelDays);

                        if (donorTestInfo.PaymentStatus != PaymentStatus.Paid
                            && hairTestPanelDays > 0)
                        {
                            ClientDepartment clientDepartment = clientBL.GetClientDepartment(donorTestInfo.ClientDepartmentId);
                            if (clientDepartment != null)
                            {
                                foreach (ClientDeptTestCategory clientDeptTestCategory in clientDepartment.ClientDeptTestCategories)
                                {
                                    if (clientDeptTestCategory.TestCategoryId == TestCategories.Hair)
                                    {
                                        if (clientDeptTestCategory.ClientDeptTestPanels.Count == 1)
                                        {
                                            testCategory.TestPanelPrice = Convert.ToDouble(clientDeptTestCategory.ClientDeptTestPanels[0].TestPanelPrice) * (hairTestPanelDays / 90);
                                        }
                                        break;
                                    }
                                }
                            }
                        }

                        break;
                    }
                }

                if (donorTestInfo.PaymentStatus != PaymentStatus.Paid
                            && hairTestPanelDays > 0)
                {
                    double totalPaymentAmount = 0.0;

                    foreach (DonorTestInfoTestCategories testCategory in donorTestInfo.TestInfoTestCategories)
                    {
                        if (testCategory.TestPanelPrice != null)
                        {
                            totalPaymentAmount += Convert.ToDouble(testCategory.TestPanelPrice);
                        }
                    }
                    donorTestInfo.TotalPaymentAmount = totalPaymentAmount;
                }

                donorDao.UpdateHairTestPanelDays(donorTestInfo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SaveLegalInfoDetails(DonorTestInfo donorTestInfo, bool isValidString)
        {
            try
            {
                donorDao.SaveLegalInfoDetails(donorTestInfo, isValidString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SaveVendorInfoDetails(DonorTestInfo donorTestInfo)
        {
            try
            {
                donorDao.SaveVendorInfoDetails(donorTestInfo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddDonorActivityNote(DonorActivityNote donorActivityNote)
        {
            donorDao.AddDonorActivityNote(donorActivityNote);
        }

        public void UpdateWebHidden(int DonorId, bool Hidden)
        {
            donorDao.UpdateDonorisWebHidden(DonorId, Hidden);
        }

        public List<DonorDocument> GetDonorDocumentList(int donorId, bool HideFilesFromSystem = true)
        {
            try
            {
                DataTable dtDonorDocuments = donorDao.GetDonorDocumentList(donorId, HideFilesFromSystem);

                List<DonorDocument> donorDocumentsList = new List<DonorDocument>();

                foreach (DataRow dr in dtDonorDocuments.Rows)
                {
                    DonorDocument donorDocument = new DonorDocument();

                    donorDocument.DonorDocumentId = (int)dr["DonorDocumentId"];
                    donorDocument.DonorId = (int)dr["DonorId"];
                    donorDocument.DocumentUploadTime = (DateTime)dr["DocumentUploadTime"];
                    donorDocument.DateRequired = DateTime.MinValue;
                    donorDocument.IsArchived = dr["isArchived"].ToString() == "1" ? true : false;
                    donorDocument.IsNotify = dr["isNotify"].ToString() == "1" ? true : false;
                    donorDocument.IsNeedsApproval = dr["isNeedsApproval"].ToString() == "1" ? true : false;
                    donorDocument.IsApproved = dr["isApproved"].ToString() == "1" ? true : false;
                    donorDocument.IsRejected = dr["isRejected"].ToString() == "1" ? true : false;
                    donorDocument.IsUpdatable = dr["isUpdateable"].ToString() == "1" ? true : false;
                    donorDocument.DocumentTitle = (string)dr["DocumentTitle"];
                    donorDocument.Source = dr["Source"].ToString();
                    donorDocument.UploadedBy = (string)dr["UploadedBy"];
                    donorDocument.FileName = (string)dr["FileName"];
                    donorDocumentsList.Add(donorDocument);
                }
                return donorDocumentsList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public List<DonorDocument> GetDonorDocumentTypes(int donorId)
        //{
        //}

        public DonorDocument GetDonorDocument(int donordocumentId, bool excludeBySystem = true)
        {
            try
            {
                DonorDocument donorDocument = donorDao.GetDonorDocument(donordocumentId, excludeBySystem);

                return donorDocument;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UploadDonorDocument(DonorDocument donorDocument)
        {
            try
            {
                return donorDao.UploadDonorDocument(donorDocument);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int DonorDocumentApprove(int DonorDocumentId)
        {
            try
            {
                return donorDao.UpdateDonorDocumentApproved(DonorDocumentId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int DonorDocumentReject(int DonorDocumentId)
        {
            try
            {
                return donorDao.UpdateDonorDocumentReject(DonorDocumentId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DonorAccounting GetAccountingDetails(int donorId, int donorTestInfoId)
        {
            try
            {
                DonorAccounting donorAccounting = donorDao.GetAccountingDetails(donorId, donorTestInfoId);

                return donorAccounting;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DonorAccounting GetAccountingDetails(DateTime dtStart, DateTime dtEnd, string clientDepartmentList)
        {
            try
            {
                if (clientDepartmentList == string.Empty)
                {
                    clientDepartmentList = "0";
                }

                DonorAccounting donorAccounting = donorDao.GetAccountingDetails(dtStart, dtEnd, clientDepartmentList);

                return donorAccounting;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<DonorTestInfo> GetPerformanceDetails(DateTime dtStart, DateTime dtEnd, string clientDepartmentList)
        {
            try
            {
                if (clientDepartmentList == string.Empty)
                {
                    clientDepartmentList = "0";
                }

                DataTable dtPerformance = donorDao.GetPerformanceDetails(dtStart, dtEnd, clientDepartmentList);

                List<DonorTestInfo> donorTestInfoList = new List<DonorTestInfo>();

                foreach (DataRow dr in dtPerformance.Rows)
                {
                    DonorTestInfo donorTestInfo = new DonorTestInfo();

                    donorTestInfo.ClientId = (int)dr["ClientId"];
                    donorTestInfo.ClientDepartmentId = (int)dr["ClientDepartmentId"];
                    donorTestInfo.PreRegistration = Convert.ToInt32(dr["PreregistrationCount"]);
                    donorTestInfo.Activated = Convert.ToInt32(dr["ActivatedCount"]);
                    donorTestInfo.Registered = Convert.ToInt32(dr["RegisteredCount"]);
                    donorTestInfo.InQueue = Convert.ToInt32(dr["InQueueCount"]);
                    donorTestInfo.SuspensionQueue = Convert.ToInt32(dr["SuspensionQueueCount"]);
                    donorTestInfo.Processing = Convert.ToInt32(dr["ProcessingCount"]);
                    donorTestInfo.Completed = Convert.ToInt32(dr["CompletedCount"]);
                    //donorTestInfoList.Add(donorTestInfo);

                    Client client = clientDao.Get(donorTestInfo.ClientId);

                    donorTestInfo.ClientName = client.ClientName.ToString();

                    ClientDepartment clientDepartment = clientDao.GetClientDepartment(donorTestInfo.ClientDepartmentId);

                    donorTestInfo.ClientDeparmentName = clientDepartment.DepartmentName.ToString();

                    donorTestInfoList.Add(donorTestInfo);
                }
                return donorTestInfoList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<DonorTestInfo> GetPerformanceDetailsBySorting(DateTime dtStart, DateTime dtEnd, string clientDepartmentList, string param, string order = null)
        {
            try
            {
                if (clientDepartmentList == string.Empty)
                {
                    clientDepartmentList = "0";
                }

                DataTable dtPerformance = donorDao.GetPerformanceDetailsBySorting(dtStart, dtEnd, clientDepartmentList, param, order);

                List<DonorTestInfo> donorTestInfoList = new List<DonorTestInfo>();

                foreach (DataRow dr in dtPerformance.Rows)
                {
                    DonorTestInfo donorTestInfo = new DonorTestInfo();

                    donorTestInfo.ClientId = (int)dr["ClientId"];
                    donorTestInfo.ClientDepartmentId = (int)dr["ClientDepartmentId"];
                    donorTestInfo.PreRegistration = Convert.ToInt32(dr["PreregistrationCount"]);
                    donorTestInfo.Activated = Convert.ToInt32(dr["ActivatedCount"]);
                    donorTestInfo.Registered = Convert.ToInt32(dr["RegisteredCount"]);
                    donorTestInfo.InQueue = Convert.ToInt32(dr["InQueueCount"]);
                    donorTestInfo.SuspensionQueue = Convert.ToInt32(dr["SuspensionQueueCount"]);
                    donorTestInfo.Processing = Convert.ToInt32(dr["ProcessingCount"]);
                    donorTestInfo.Completed = Convert.ToInt32(dr["CompletedCount"]);
                    //donorTestInfoList.Add(donorTestInfo);

                    Client client = clientDao.Get(donorTestInfo.ClientId);

                    donorTestInfo.ClientName = client.ClientName.ToString();

                    ClientDepartment clientDepartment = clientDao.GetClientDepartment(donorTestInfo.ClientDepartmentId);

                    donorTestInfo.ClientDeparmentName = clientDepartment.DepartmentName.ToString();

                    donorTestInfoList.Add(donorTestInfo);
                }
                return donorTestInfoList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetSpecimenId(string specimenId)
        {
            try
            {
                return donorDao.GetSpecimenId(specimenId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int DonorResendMail(int donorId)
        {
            try
            {
                int returnvalue = 0;
                Donor donor = donorDao.Get(donorId);
                ////Username / Email validation
                UserDao userDao = new UserDao();
                User users = userDao.GetByUsernameOrEmail(donor.DonorEmail);
                //if (user != null)
                //{
                //    throw new Exception("The email address already exists.");
                //}
                ////

                //RandomStringGenerator rsg = new RandomStringGenerator(true, false, true, false);
                //string newPassword = rsg.Generate(6, 8);
                //string newPasswordEncrypted = UserAuthentication.Encrypt(newPassword, true);

                User user = new User();
                user.UserId = users.UserId;
                //user.Username = donor.DonorEmail;
                user.UserPassword = users.UserPassword;
                user.IsUserActive = true;
                user.UserFirstName = donor.DonorFirstName;
                user.UserLastName = donor.DonorLastName;
                user.UserPhoneNumber = donor.DonorPhone1;
                user.UserFax = null;
                user.UserEmail = donor.DonorEmail;
                user.ChangePasswordRequired = true;
                user.UserType = Enum.UserType.Donor;
                user.DepartmentId = null;
                user.DonorId = null;
                user.ClientId = null;
                user.VendorId = null;
                user.AttorneyId = null;
                user.CourtId = null;
                user.JudgeId = null;
                user.CreatedBy = donor.CreatedBy;
                user.LastModifiedBy = donor.LastModifiedBy;

                UserBL userBL = new UserBL();

                //Donor Registration Mail
                try
                {
                    MailManager mailManager = new MailManager();
                    string mailBody = mailManager.SendRegistrationMail(user, "Registration");

                    if (donor.DonorEmail != string.Empty)
                    {
                        mailManager.SendMail(donor.DonorEmail, ConfigurationManager.AppSettings["RegistrationMailSubject"].ToString().Trim(), mailBody);
                        returnvalue = 1;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Not able to send mail.");
                }

                return returnvalue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DonorSendSMS(int donorId, string Message)
        {
            var newmessage = "Surscan Records: " + Message + " Visit www.surscan.com to update.";
            try
            {
                int returnvalue = 0;
                Donor donor = donorDao.Get(donorId);
                ////Username / Email validation
                UserDao userDao = new UserDao();
                User users = userDao.GetByUsernameOrEmail(donor.DonorEmail);

                //User user = new User();
                //user.UserId = users.UserId;
                //user.Username = donor.DonorEmail;
                //user.UserPassword = users.UserPassword;
                //user.IsUserActive = true;
                //user.UserFirstName = donor.DonorFirstName;
                //user.UserLastName = donor.DonorLastName;
                //user.UserPhoneNumber = donor.DonorPhone1;
                //user.UserFax = null;
                //user.UserEmail = donor.DonorEmail;
                //user.ChangePasswordRequired = true;
                //user.UserType = Enum.UserType.Donor;
                //user.DepartmentId = null;
                //user.DonorId = null;
                //user.ClientId = null;
                //user.VendorId = null;
                //user.AttorneyId = null;
                //user.CourtId = null;
                //user.JudgeId = null;
                //user.CreatedBy = donor.CreatedBy;
                //user.LastModifiedBy = donor.LastModifiedBy;

                //UserBL userBL = new UserBL();

                //Donor Registration Mail
                try
                {
                    const string accountSid = "AC71c8ef0bcf11e3a2c65f4bab577a2e1e";
                    const string authToken = "ce5bf1314d9e6d4e36a825e0efc4be19";

                    TwilioClient.Init(accountSid, authToken);

                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                                | SecurityProtocolType.Tls11
                                                | SecurityProtocolType.Tls12
                                                | SecurityProtocolType.Ssl3;
                    //await Task.Delay(100000);

                    DonorActivityNote note = new DonorActivityNote();
                    note.ActivityNote = "SMS Sent = " + newmessage;
                    note.IsActivityVisible = true;
                    note.ActivityUserId = donorId;
                    note.ActivityCategoryId = DonorActivityCategories.General;
                    note.DonorTestInfoId = donor.DonorTestInfoId;

                    //note.current
                    var phonenumber = users.UserPhoneNumber;

                    if (!string.IsNullOrEmpty(phonenumber))
                    {
                        this.AddDonorActivityNote(note);

                        var message = await MessageResource.CreateAsync(
                            body: newmessage,
                            from: new Twilio.Types.PhoneNumber("+13252081790"),
                            to: new Twilio.Types.PhoneNumber(phonenumber)
                        );
                    }
                    else
                    {
                        var donorphonenumber = donor.DonorPhone1;

                        if (!string.IsNullOrEmpty(donorphonenumber))
                        {
                            this.AddDonorActivityNote(note);

                            var message = await MessageResource.CreateAsync(
                                body: newmessage,
                                from: new Twilio.Types.PhoneNumber("+13252081790"),
                                to: new Twilio.Types.PhoneNumber(phonenumber)
                            );
                        }
                    }

                    //Console.WriteLine(message.Sid);

                    return;
                }
                catch (Exception ex)
                {
                    //throw new Exception("Not able to send sms message.");
                }
            }
            catch (Exception ex)
            {
                //throw ex;
            }
        }

        public DataTable DisplayColumns(string strSQL, Dictionary<string, string> searchParam)
        {
            try
            {
                return donorDao.DisplayColumns(strSQL, searchParam);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Start Here
        public DataTable SearchDonorByClient(Dictionary<string, string> searchParam, UserType userType, int currentUserId)
        {
            try
            {
                return donorDao.SearchDonorByClient(searchParam, userType, currentUserId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //End Here

        public DataTable ColumnsName()
        {
            try
            {
                return donorDao.ColumnsName();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int ColumnsAdd(string columnId, string columnName)
        {
            return donorDao.ColumnsAdd(columnId, columnName);
        }

        public int ColumnsRemove(string columnId, string columnName)
        {
            return donorDao.ColumnsRemove(columnId, columnName);
        }

        public DataTable GetColumnName()
        {
            try
            {
                return donorDao.GetColumnName();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public DataTable GetColumns()
        //{
        //    try
        //    {
        //        return donorDao.GetColumns();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}

        public DataTable GetInActiveUser(string UserName, string PassWord)
        {
            try
            {
                return donorDao.GetInActiveUser(UserName, PassWord);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetInActiveUserWithPW(string UserName, string PassWord)
        {
            try
            {
                return donorDao.GetInActiveUserWithPW(UserName, PassWord);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DonorTestInfo GetDonorTestInfoByDonorIdDonorTestInfoId(int donorId, int donorTestInfoId)
        {
            try
            {
                DonorTestInfo donorTestInfo = donorDao.GetDonorTestInfoByDonorIdDonorTestInfoId(donorId, donorTestInfoId);

                return donorTestInfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetIsReverseEntry(int donorId, int donorTestInfoId)
        {
            try
            {
                return donorDao.GetTestInfoReverseEntryList(donorId, donorTestInfoId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ReportInfo GetMROReport(int testInfoId, ReportType reportType)
        {
            try
            {
                ReportInfo donorDocument = donorDao.GetMROReportId(testInfoId, reportType);
                return donorDocument;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ReportInfo GetLabReport(int testInfoId, ReportType reportType)
        {
            try
            {
                ReportInfo getLabReport = donorDao.GetLabReport(testInfoId, reportType);
                return getLabReport;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ReportInfo GetLabReportType(int testInfoId, string specimenId, ReportType reportType)
        {
            try
            {
                ReportInfo getLabReportType = donorDao.GetLabReportType(testInfoId, specimenId, reportType);
                return getLabReportType;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetMoreThanTestInfoList(int donorId)
        {
            try
            {
                return donorDao.GetMoreThanTestInfoList(donorId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void logDebug(string _t)
        {
            _logger.Debug($"{this.SessionID} - DonorBL - {_t.ToString()}");
        }

        private void LogAnError(Exception ex)
        {
            _logger.Error("- DonorBL - ERROR");
            _logger.Error(ex.Message);
            if (!(ex.InnerException == null)) _logger.Error(ex.InnerException.ToString());
        }
    }
}