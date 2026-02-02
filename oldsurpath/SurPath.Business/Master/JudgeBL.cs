using SurPath.Data;
using SurPath.Data.Master;
using SurPath.Entity;
using SurPath.Entity.Master;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text.RegularExpressions;

namespace SurPath.Business.Master
{
    public class JudgeBL : BusinessObject
    {
        private JudgeDao judgeDao = new JudgeDao();
        private Regex regexEmail = new Regex(@"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$");

        public int Save(Judge judge)
        {
            if (judge.JudgeId == 0)
            {
                //Username / Email validation
                UserDao userDao = new UserDao();
                User user = userDao.GetByUsernameOrEmail(judge.JudgeUsername);
                if (user != null)
                {
                    throw new Exception("The Username already exists.");
                }
                //

                //RandomStringGenerator rsg = new RandomStringGenerator(true, false, true, false);
                //judge.JudgePassword = rsg.Generate(6, 8);
                string newPasswordEncrypted = UserAuthentication.Encrypt(judge.JudgePassword, true);

                user = new User();

                user.Username = judge.JudgeUsername;
                user.UserPassword = newPasswordEncrypted;
                user.IsUserActive = true;
                user.UserFirstName = judge.JudgeFirstName;
                user.UserLastName = judge.JudgeLastName;
                user.UserPhoneNumber = null;
                user.UserFax = null;
                if (regexEmail.IsMatch(judge.JudgeUsername))
                {
                    user.UserEmail = judge.JudgeUsername;
                }
                else
                {
                    user.UserEmail = null;
                }
                user.ChangePasswordRequired = true;
                user.UserType = Enum.UserType.Judge;
                user.DepartmentId = null;
                user.DonorId = null;
                user.ClientId = null;
                user.VendorId = null;
                user.AttorneyId = null;
                user.CourtId = null;
                user.JudgeId = null;
                user.CreatedBy = judge.CreatedBy;
                user.LastModifiedBy = judge.LastModifiedBy;

                int judgeId = judgeDao.Insert(judge, user);
                //Send the mail with newPassword and other donor information
                if (judge.JudgeUsername != string.Empty)
                {
                    if (regexEmail.IsMatch(judge.JudgeUsername))
                    {
                        try
                        {
                            MailManager mailManager = new MailManager();
                            string mailBody = mailManager.SendRegistrationMail(user, "Registration");
                            if (judge.JudgeUsername != string.Empty)
                            {
                                mailManager.SendMail(judge.JudgeUsername, ConfigurationManager.AppSettings["RegistrationMailSubject"].ToString().Trim(), mailBody);
                            }
                        }
                        catch
                        {
                            throw new Exception("Not able to send mail.");
                        }
                    }
                }

                judge.JudgeId = judgeId;
                return judgeId;
            }
            else
            {
                return judgeDao.Update(judge);
            }
        }

        public int Delete(int judgeId, string currentUserName)
        {
            return judgeDao.Delete(judgeId, currentUserName);
        }

        public Judge Get(int judgeId)
        {
            try
            {
                Judge judge = judgeDao.Get(judgeId);

                return judge;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetByEmail(string email)
        {
            try
            {
                DataTable judges = judgeDao.GetByEmail(email);

                return judges;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Judge> GetList()
        {
            try
            {
                DataTable dtJudges = judgeDao.GetList();

                List<Judge> judgeList = new List<Judge>();

                foreach (DataRow dr in dtJudges.Rows)
                {
                    Judge judge = new Judge();

                    judge.JudgeId = (int)dr["JudgeId"];
                    judge.JudgeUsername = (string)dr["JudgeUsername"];
                    judge.JudgePrefix = dr["JudgePrefix"].ToString();
                    judge.JudgeFirstName = dr["JudgeFirstName"].ToString();
                    judge.JudgeLastName = dr["JudgeLastName"].ToString();
                    judge.JudgeSuffix = dr["JudgeSuffix"].ToString();
                    judge.JudgeAddress1 = dr["JudgeAddress1"].ToString();
                    judge.JudgeAddress2 = dr["JudgeAddress2"].ToString();
                    judge.JudgeCity = dr["JudgeCity"].ToString();
                    judge.JudgeState = dr["JudgeState"].ToString();
                    judge.JudgeZip = dr["JudgeZip"].ToString();
                    judge.IsActive = dr["IsActive"].ToString() == "1" ? true : false;
                    judge.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    judge.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    judge.CreatedOn = (DateTime)dr["CreatedOn"];
                    judge.CreatedBy = (string)dr["CreatedBy"];
                    judge.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    judge.LastModifiedBy = (string)dr["LastModifiedBy"];

                    judgeList.Add(judge);
                }

                return judgeList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Judge> GetList(Dictionary<string, string> searchParam)
        {
            try
            {
                DataTable dtJudges = judgeDao.GetList(searchParam);

                List<Judge> judgeList = new List<Judge>();

                foreach (DataRow dr in dtJudges.Rows)
                {
                    Judge judge = new Judge();

                    judge.JudgeId = (int)dr["JudgeId"];
                    judge.JudgeUsername = (string)dr["JudgeUsername"];
                    judge.JudgePrefix = dr["JudgePrefix"].ToString();
                    judge.JudgeFirstName = dr["JudgeFirstName"].ToString();
                    judge.JudgeLastName = dr["JudgeLastName"].ToString();
                    judge.JudgeSuffix = dr["JudgeSuffix"].ToString();
                    judge.JudgeAddress1 = dr["JudgeAddress1"].ToString();
                    judge.JudgeAddress2 = dr["JudgeAddress2"].ToString();
                    judge.JudgeCity = dr["JudgeCity"].ToString();
                    judge.JudgeState = dr["JudgeState"].ToString();
                    judge.JudgeZip = dr["JudgeZip"].ToString();
                    judge.IsActive = dr["IsActive"].ToString() == "1" ? true : false;
                    judge.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    judge.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    judge.CreatedOn = (DateTime)dr["CreatedOn"];
                    judge.CreatedBy = (string)dr["CreatedBy"];
                    judge.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    judge.LastModifiedBy = (string)dr["LastModifiedBy"];

                    judgeList.Add(judge);
                }

                return judgeList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Judge> Sorting(string searchparam, bool active, string getInActive = null)
        {
            try
            {
                DataTable dtJudge = judgeDao.sorting(searchparam, active, getInActive);

                List<Judge> judgeList = new List<Judge>();

                foreach (DataRow dr in dtJudge.Rows)
                {
                    Judge judge = new Judge();

                    judge.JudgeId = (int)dr["JudgeId"];
                    judge.JudgeUsername = (string)dr["JudgeUsername"];
                    judge.JudgePrefix = dr["JudgePrefix"].ToString();
                    judge.JudgeFirstName = dr["JudgeFirstName"].ToString();
                    judge.JudgeLastName = dr["JudgeLastName"].ToString();
                    judge.JudgeSuffix = dr["JudgeSuffix"].ToString();
                    judge.JudgeAddress1 = dr["JudgeAddress1"].ToString();
                    judge.JudgeAddress2 = dr["JudgeAddress2"].ToString();
                    judge.JudgeCity = dr["JudgeCity"].ToString();
                    judge.JudgeState = dr["JudgeState"].ToString();
                    judge.JudgeZip = dr["JudgeZip"].ToString();
                    judge.IsActive = dr["IsActive"].ToString() == "1" ? true : false;
                    judge.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    judge.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    judge.CreatedOn = (DateTime)dr["CreatedOn"];
                    judge.CreatedBy = (string)dr["CreatedBy"];
                    judge.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    judge.LastModifiedBy = (string)dr["LastModifiedBy"];

                    judgeList.Add(judge);
                }

                return judgeList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}