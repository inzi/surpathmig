using SurPath.Data;
using SurPath.Data.Master;
using SurPath.Entity;
using SurPath.Entity.Master;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace SurPath.Business.Master
{
    /// <summary>
    /// Attorney related business process.
    /// </summary>
    ///
    public class AttorneyBL : BusinessObject
    {
        private AttorneyDao attorneyDao = new AttorneyDao();

        public int Save(Attorney attorney)
        {
            if (attorney.AttorneyId == 0)
            {
                //Username / Email validation
                UserDao userDao = new UserDao();
                User user = userDao.GetByUsernameOrEmail(attorney.AttorneyEmail);
                if (user != null)
                {
                    throw new Exception("The email address already exists.");
                }

                RandomStringGenerator rsg = new RandomStringGenerator(true, false, true, false);
                string newPassword = rsg.Generate(6, 8);
                string newPasswordEncrypted = UserAuthentication.Encrypt(newPassword, true);

                user = new User();

                user.Username = attorney.AttorneyEmail;
                user.UserPassword = newPasswordEncrypted;
                user.IsUserActive = true;
                user.UserFirstName = attorney.AttorneyFirstName;
                user.UserLastName = attorney.AttorneyLastName;
                user.UserPhoneNumber = attorney.AttorneyPhone;
                user.UserFax = attorney.AttorneyFax;
                user.UserEmail = attorney.AttorneyEmail;
                user.ChangePasswordRequired = true;
                user.UserType = Enum.UserType.Attorney;
                user.DepartmentId = null;
                user.DonorId = null;
                user.ClientId = null;
                user.VendorId = null;
                user.AttorneyId = null;
                user.CourtId = null;
                user.JudgeId = null;
                user.CreatedBy = attorney.CreatedBy;
                user.LastModifiedBy = attorney.LastModifiedBy;

                int attorneyId = attorneyDao.Insert(attorney, user);

                //Send the mail with newPassword and other donor information
                try
                {
                    MailManager mailManager = new MailManager();
                    string mailBody = mailManager.SendRegistrationMail(user, "Registration");
                    if (attorney.AttorneyEmail != string.Empty)
                    {
                        mailManager.SendMail(attorney.AttorneyEmail, ConfigurationManager.AppSettings["RegistrationMailSubject"].ToString().Trim(), mailBody);
                    }
                }
                catch
                {
                    throw new Exception("Not able to send mail.");
                }

                attorney.AttorneyId = attorneyId;
                return attorneyId;
            }
            else
            {
                return attorneyDao.Update(attorney);
            }
        }

        public int Delete(int attorneyId, string currentUserName)
        {
            return attorneyDao.Delete(attorneyId, currentUserName);
        }

        public Attorney Get(int attorneyId)
        {
            try
            {
                Attorney attorney = attorneyDao.Get(attorneyId);

                return attorney;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetByEmail(string Email)
        {
            try
            {
                DataTable attorneys = attorneyDao.GetByEmail(Email);

                return attorneys;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Attorney> GetList(string getInActive = null)
        {
            try
            {
                DataTable dtAttorney = attorneyDao.GetList(getInActive);

                List<Attorney> attorneyList = new List<Attorney>();
                foreach (DataRow dr in dtAttorney.Rows)
                {
                    Attorney attorney = new Attorney();

                    attorney.AttorneyId = (int)dr["AttorneyId"];
                    attorney.AttorneyFirstName = dr["AttorneyFirstName"].ToString();
                    attorney.AttorneyLastName = dr["AttorneyLastName"].ToString();
                    attorney.AttorneyAddress1 = dr["AttorneyAddress1"].ToString();
                    attorney.AttorneyAddress2 = dr["AttorneyAddress2"].ToString();
                    attorney.AttorneyCity = dr["AttorneyCity"].ToString();
                    attorney.AttorneyState = dr["AttorneyState"].ToString();
                    attorney.AttorneyZip = dr["AttorneyZip"].ToString();
                    attorney.AttorneyPhone = dr["AttorneyPhone"].ToString();
                    attorney.AttorneyFax = dr["AttorneyFax"].ToString();
                    attorney.AttorneyEmail = dr["AttorneyEmail"].ToString();
                    attorney.IsActive = dr["IsActive"].ToString() == "1" ? true : false;
                    attorney.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    attorney.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    attorney.CreatedOn = (DateTime)dr["CreatedOn"];
                    attorney.CreatedBy = (string)dr["CreatedBy"];
                    attorney.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    attorney.LastModifiedBy = (string)dr["LastModifiedBy"];

                    attorneyList.Add(attorney);
                }

                return attorneyList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Attorney> GetList(Dictionary<string, string> searchParam)
        {
            try
            {
                DataTable dtAttorney = attorneyDao.GetList(searchParam);

                List<Attorney> attorneyList = new List<Attorney>();
                foreach (DataRow dr in dtAttorney.Rows)
                {
                    Attorney attorney = new Attorney();

                    attorney.AttorneyId = (int)dr["AttorneyId"];
                    attorney.AttorneyFirstName = dr["AttorneyFirstName"].ToString();
                    attorney.AttorneyLastName = dr["AttorneyLastName"].ToString();
                    attorney.AttorneyAddress1 = dr["AttorneyAddress1"].ToString();
                    attorney.AttorneyAddress2 = dr["AttorneyAddress2"].ToString();
                    attorney.AttorneyCity = dr["AttorneyCity"].ToString();
                    attorney.AttorneyState = dr["AttorneyState"].ToString();
                    attorney.AttorneyZip = dr["AttorneyZip"].ToString();
                    attorney.AttorneyPhone = dr["AttorneyPhone"].ToString();
                    attorney.AttorneyFax = dr["AttorneyFax"].ToString();
                    attorney.AttorneyEmail = dr["AttorneyEmail"].ToString();
                    attorney.IsActive = dr["IsActive"].ToString() == "1" ? true : false;
                    attorney.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    attorney.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    attorney.CreatedOn = (DateTime)dr["CreatedOn"];
                    attorney.CreatedBy = (string)dr["CreatedBy"];
                    attorney.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    attorney.LastModifiedBy = (string)dr["LastModifiedBy"];

                    attorneyList.Add(attorney);
                }

                return attorneyList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Attorney> Sorting(string searchparam, bool active, string getInActive = null)
        {
            try
            {
                DataTable dtAttorney = attorneyDao.sorting(searchparam, active, getInActive);

                List<Attorney> attorneyList = new List<Attorney>();

                foreach (DataRow dr in dtAttorney.Rows)
                {
                    Attorney attorney = new Attorney();

                    attorney.AttorneyId = (int)dr["AttorneyId"];
                    attorney.AttorneyFirstName = dr["AttorneyFirstName"].ToString();
                    attorney.AttorneyLastName = dr["AttorneyLastName"].ToString();
                    attorney.AttorneyAddress1 = dr["AttorneyAddress1"].ToString();
                    attorney.AttorneyAddress2 = dr["AttorneyAddress2"].ToString();
                    attorney.AttorneyCity = dr["AttorneyCity"].ToString();
                    attorney.AttorneyState = dr["AttorneyState"].ToString();
                    attorney.AttorneyZip = dr["AttorneyZip"].ToString();
                    attorney.AttorneyPhone = dr["AttorneyPhone"].ToString();
                    attorney.AttorneyFax = dr["AttorneyFax"].ToString();
                    attorney.AttorneyEmail = dr["AttorneyEmail"].ToString();
                    attorney.IsActive = dr["IsActive"].ToString() == "1" ? true : false;
                    attorney.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    attorney.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    attorney.CreatedOn = (DateTime)dr["CreatedOn"];
                    attorney.CreatedBy = (string)dr["CreatedBy"];
                    attorney.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    attorney.LastModifiedBy = (string)dr["LastModifiedBy"];

                    attorneyList.Add(attorney);
                }

                return attorneyList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}