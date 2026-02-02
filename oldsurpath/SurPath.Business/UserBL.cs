using SurPath.Business.Master;
using SurPath.Data;
using SurPath.Entity;
using SurPath.Entity.Master;
using SurPath.Enum;
using SurpathBackend;
using System;
using System.Collections.Generic;
using System.Data;

namespace SurPath.Business
{
    public class UserBL : BusinessObject
    {
        private UserDao userDao = new UserDao();

        #region Public Methods

        public int Save(User user)
        {
            if (user.UserId == 0)
            {
                return userDao.Insert(user);
            }
            else
            {
                return userDao.Update(user);
            }
        }

        public int Delete(int userId, string currentUserName)
        {
            return userDao.Delete(userId, currentUserName);
        }

        public int ChangePassword(string username, string newPassword)
        {
            return userDao.ChangePassword(username, newPassword);
        }

        public int ChangeDonorPassword(int donorId, string oldPassword, string newPassword)
        {
            DonorBL donorBL = new DonorBL();
            Donor donor = donorBL.Get(donorId, "Desktop");

            if (donor == null)
            {
                throw new Exception("User does not exists.");
            }

            UserBL userBL = new UserBL();
            User user = userBL.GetByUsernameOrEmail(donor.DonorEmail);

            if (user != null)
            {
                if (user.UserPassword != UserAuthentication.Encrypt(oldPassword, true))
                {
                    throw new Exception("The old password does not match with the database value.");
                }
            }

            int retval = userDao.ChangePassword(user.Username, UserAuthentication.Encrypt(newPassword, true));
            if (retval>0)
            {
                // Capture The PW Change
                BackendLogic _backendLogic = new BackendLogic();
                _backendLogic.SetUserActivity(user.UserId, (int)UserActivityCategories.Security, $"User {user.UserId} {user.UserEmail} of type {(UserType.Donor).ToString()} changed password");
                _backendLogic = null;
            }

            return retval;
        }

        public User GetDonor(int userId)
        {
            try
            {
                User user = userDao.GetDonor(userId);

                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public User Get(int donorId)
        {
            try
            {
                User user = userDao.Get(donorId);

                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public User Get(string username)
        {
            try
            {
                User user = userDao.Get(username);

                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public User GetByUsernameOrEmail(string username)
        {
            try
            {
                User user = userDao.GetByUsernameOrEmail(username);

                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public User GetByDonorId(int DonorId)
        {
            try
            {
                User user = userDao.GetByDonorId(DonorId);

                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<User> GetList()
        {
            try
            {
                DataTable dtUsers = userDao.GetList();

                List<User> userList = new List<User>();

                foreach (DataRow dr in dtUsers.Rows)
                {
                    User user = new User();

                    user.UserId = (int)dr["UserId"];

                    user.Username = (string)dr["Username"];
                    user.UserPassword = (string)dr["UserPassword"];
                    user.IsUserActive = dr["IsUserActive"].ToString() == "1" ? true : false;
                    user.UserFirstName = (string)dr["UserFirstName"];
                    user.UserLastName = (string)dr["UserLastName"] != string.Empty ? (string)dr["UserLastName"] : null;
                    user.UserPhoneNumber = dr["UserPhoneNumber"].ToString();
                    user.UserFax = dr["UserFax"].ToString();
                    user.UserEmail = (string)dr["UserEmail"] != string.Empty ? (string)dr["UserEmail"] : null;
                    user.ChangePasswordRequired = dr["ChangePasswordRequired"].ToString() == "1" ? true : false;
                    user.UserType = (UserType)dr["UserType"];
                    if (dr["DepartmentId"].ToString() == string.Empty)
                    {
                        user.DepartmentId = dr["DepartmentId"].ToString() != string.Empty ? (int?)dr["DepartmentId"] : null;
                    }
                    else if (dr["DepartmentId"].ToString() != string.Empty)
                    {
                        int departmentId = Convert.ToInt32(dr["DepartmentId"].ToString());
                        DepartmentBL departmentBL = new DepartmentBL();
                        Department department = departmentBL.Get(departmentId);
                        string departmentName = department.DepartmentNameValue;
                        user.DepartmentName = departmentName.ToString();
                    }
                    else if (dr["DepartmentId"].ToString() == null)
                    {
                        user.DepartmentId = dr["DepartmentId"].ToString() != string.Empty ? (int?)dr["DepartmentId"] : null;
                    }

                    if (dr["DonorId"].ToString() == string.Empty)
                    {
                        user.DonorId = dr["DonorId"].ToString() != string.Empty ? (int?)dr["DonorId"] : null;
                    }
                    else if (dr["DonorId"].ToString() != string.Empty)
                    {
                        int donorId = Convert.ToInt32(dr["DonorId"].ToString());
                        DonorBL donorBL = new DonorBL();
                        Donor donor = donorBL.Get(donorId, "Desktop");
                        string donarName = donor.DonorFirstName + " " + donor.DonorLastName;
                        user.DonorName = donarName.ToString();
                    }
                    else if (dr["DonorId"].ToString() == null)
                    {
                        user.DonorId = dr["DonorId"].ToString() != string.Empty ? (int?)dr["DonorId"] : null;
                    }

                    if (dr["ClientId"].ToString() == string.Empty)
                    {
                        user.ClientId = dr["ClientId"].ToString() != string.Empty ? (int?)dr["ClientId"] : null;
                    }
                    else if (dr["ClientId"].ToString() != string.Empty)
                    {
                        int clientId = Convert.ToInt32(dr["ClientId"].ToString());
                        ClientBL clientBL = new ClientBL();
                        Client client = clientBL.Get(clientId);
                        string clientName = client.ClientName;
                        user.ClientNames = clientName.ToString();
                    }
                    else if (dr["ClientId"].ToString() == null)
                    {
                        user.ClientId = dr["ClientId"].ToString() != string.Empty ? (int?)dr["ClientId"] : null;
                    }

                    if (dr["VendorId"].ToString() == string.Empty)
                    {
                        user.VendorId = dr["VendorId"].ToString() != string.Empty ? (int?)dr["VendorId"] : null;
                    }
                    else if (dr["VendorId"].ToString() != string.Empty)
                    {
                        int vendorId = Convert.ToInt32(dr["VendorId"].ToString());
                        VendorBL vendorBL = new VendorBL();
                        Vendor vendor = vendorBL.Get(vendorId);
                        string vendorName = vendor.VendorName;
                        user.VendorNames = vendorName.ToString();
                    }
                    else if (dr["VendorId"].ToString() == null)
                    {
                        user.VendorId = dr["VendorId"].ToString() != string.Empty ? (int?)dr["VendorId"] : null;
                    }

                    if (dr["AttorneyId"].ToString() == string.Empty)
                    {
                        user.AttorneyId = dr["AttorneyId"].ToString() != string.Empty ? (int?)dr["AttorneyId"] : null;
                    }
                    else if (dr["AttorneyId"].ToString() != string.Empty)
                    {
                        int attorneyId = Convert.ToInt32(dr["AttorneyId"].ToString());
                        AttorneyBL attorneyBL = new AttorneyBL();
                        Attorney attorney = attorneyBL.Get(attorneyId);
                        string attorneyName = attorney.AttorneyFirstName + " " + attorney.AttorneyLastName;
                        user.AttorneyNames = attorneyName.ToString();
                    }
                    else if (dr["AttorneyId"].ToString() == null)
                    {
                        user.AttorneyId = dr["AttorneyId"].ToString() != string.Empty ? (int?)dr["AttorneyId"] : null;
                    }

                    if (dr["CourtId"].ToString() == string.Empty)
                    {
                        user.CourtId = dr["CourtId"].ToString() != string.Empty ? (int?)dr["CourtId"] : null;
                    }
                    else if (dr["CourtId"].ToString() != string.Empty)
                    {
                        int courtId = Convert.ToInt32(dr["CourtId"].ToString());
                        CourtBL courtBL = new CourtBL();
                        Court court = courtBL.Get(courtId);
                        string courtName = court.CourtName;
                        user.CourtNames = courtName.ToString();
                    }
                    else if (dr["CourtId"].ToString() == null)
                    {
                        user.CourtId = dr["CourtId"].ToString() != string.Empty ? (int?)dr["CourtId"] : null;
                    }

                    if (dr["JudgeId"].ToString() == string.Empty)
                    {
                        user.JudgeId = dr["JudgeId"].ToString() != string.Empty ? (int?)dr["JudgeId"] : null;
                    }
                    else if (dr["JudgeId"].ToString() != string.Empty)
                    {
                        int JudgeId = Convert.ToInt32(dr["JudgeId"].ToString());
                        JudgeBL judgeBL = new JudgeBL();
                        Judge judge = judgeBL.Get(JudgeId);
                        string judgeName = judge.JudgeFirstName + " " + judge.JudgeLastName;
                        user.JudgeNames = judgeName.ToString();
                    }
                    else if (dr["JudgeId"].ToString() == null)
                    {
                        user.CourtId = dr["JudgeId"].ToString() != string.Empty ? (int?)dr["JudgeId"] : null;
                    }

                    if (user.DepartmentName != null)
                    {
                        string departmentName = user.DepartmentName;
                        user.UserTypeNames = departmentName.ToString();
                    }
                    else if (user.DonorName != null)
                    {
                        string donorName = user.DonorName;
                        user.UserTypeNames = donorName.ToString();
                    }
                    else if (user.ClientNames != null)
                    {
                        string clientNames = user.ClientNames;
                        user.UserTypeNames = clientNames.ToString();
                    }
                    else if (user.VendorNames != null)
                    {
                        string vendorNames = user.VendorNames;
                        user.UserTypeNames = vendorNames.ToString();
                    }
                    else if (user.AttorneyNames != null)
                    {
                        string attorneyNames = user.AttorneyNames;
                        user.UserTypeNames = attorneyNames.ToString();
                    }
                    else if (user.CourtNames != null)
                    {
                        string courtNames = user.CourtNames;
                        user.UserTypeNames = courtNames.ToString();
                    }
                    else if (user.JudgeNames != null)
                    {
                        string judgeNames = user.JudgeNames;
                        user.UserTypeNames = judgeNames.ToString();
                    }
                    else
                    {
                        user.UserTypeNames = null;
                    }

                    user.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    user.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    user.CreatedOn = (DateTime)dr["CreatedOn"];
                    user.CreatedBy = (string)dr["CreatedBy"];
                    user.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    user.LastModifiedBy = (string)dr["LastModifiedBy"];

                    userList.Add(user);
                }

                return userList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<User> GetList(Dictionary<string, string> searchParam)
        {
            try
            {
                DataTable dtUsers = userDao.GetList(searchParam);

                List<User> userList = new List<User>();

                foreach (DataRow dr in dtUsers.Rows)
                {
                    User user = new User();

                    if ((int)dr["UserId"] == 138)
                    {
                        user.UserId = (int)dr["UserId"];
                    }

                    user.UserId = (int)dr["UserId"];
                    user.Username = (string)dr["Username"];
                    user.UserPassword = (string)dr["UserPassword"];
                    user.IsUserActive = dr["IsUserActive"].ToString() == "1" ? true : false;
                    user.UserFirstName = (string)dr["UserFirstName"];
                    user.UserLastName = dr["UserLastName"].ToString();
                    user.UserPhoneNumber = dr["UserPhoneNumber"].ToString();
                    user.UserFax = dr["UserFax"].ToString();
                    user.UserEmail = dr["UserEmail"].ToString();
                    user.ChangePasswordRequired = dr["ChangePasswordRequired"].ToString() == "1" ? true : false;
                    user.UserType = (UserType)dr["UserType"];

                    if (dr["DepartmentId"].ToString() == string.Empty)
                    {
                        user.DepartmentId = dr["DepartmentId"].ToString() != string.Empty ? (int?)dr["DepartmentId"] : null;
                    }
                    else if (dr["DepartmentId"].ToString() != string.Empty)
                    {
                        int departmentId = Convert.ToInt32(dr["DepartmentId"].ToString());
                        DepartmentBL departmentBL = new DepartmentBL();
                        Department department = departmentBL.Get(departmentId);
                        string departmentName = department.DepartmentNameValue;
                        user.DepartmentName = departmentName.ToString();
                    }
                    else if (dr["DepartmentId"].ToString() == null)
                    {
                        user.DepartmentId = dr["DepartmentId"].ToString() != string.Empty ? (int?)dr["DepartmentId"] : null;
                    }

                    if (dr["DonorId"].ToString() == string.Empty)
                    {
                        user.DonorId = dr["DonorId"].ToString() != string.Empty ? (int?)dr["DonorId"] : null;
                    }
                    else if (dr["DonorId"].ToString() != string.Empty)
                    {
                        int donorId = Convert.ToInt32(dr["DonorId"].ToString());
                        DonorBL donorBL = new DonorBL();
                        Donor donor = donorBL.Get(donorId, "Desktop");
                        string donarName = "not found";
                        if (donor != null)
                        {
                            donarName = donor.DonorFirstName + " " + donor.DonorLastName;
                        }
                        
                        user.DonorName = donarName.ToString();
                    }
                    else if (dr["DonorId"].ToString() == null)
                    {
                        user.DonorId = dr["DonorId"].ToString() != string.Empty ? (int?)dr["DonorId"] : null;
                    }

                    if (dr["ClientId"].ToString() == string.Empty)
                    {
                        user.ClientId = dr["ClientId"].ToString() != string.Empty ? (int?)dr["ClientId"] : null;
                    }
                    else if (dr["ClientId"].ToString() != string.Empty)
                    {
                        int clientId = Convert.ToInt32(dr["ClientId"].ToString());
                        ClientBL clientBL = new ClientBL();

                        Client client = clientBL.Get(clientId);
                        string clientName = client.ClientName;
                        user.ClientNames = clientName.ToString();
                    }
                    else if (dr["ClientId"].ToString() == null)
                    {
                        user.ClientId = dr["ClientId"].ToString() != string.Empty ? (int?)dr["ClientId"] : null;
                    }

                    if (dr["VendorId"].ToString() == string.Empty)
                    {
                        user.VendorId = dr["VendorId"].ToString() != string.Empty ? (int?)dr["VendorId"] : null;
                    }
                    else if (dr["VendorId"].ToString() != string.Empty)
                    {
                        int vendorId = Convert.ToInt32(dr["VendorId"].ToString());
                        VendorBL vendorBL = new VendorBL();
                        Vendor vendor = vendorBL.Get(vendorId);
                        string vendorName = vendor.VendorName;
                        user.VendorNames = vendorName.ToString();
                    }
                    else if (dr["VendorId"].ToString() == null)
                    {
                        user.VendorId = dr["VendorId"].ToString() != string.Empty ? (int?)dr["VendorId"] : null;
                    }

                    if (dr["AttorneyId"].ToString() == string.Empty)
                    {
                        user.AttorneyId = dr["AttorneyId"].ToString() != string.Empty ? (int?)dr["AttorneyId"] : null;
                    }
                    else if (dr["AttorneyId"].ToString() != string.Empty)
                    {
                        int attorneyId = Convert.ToInt32(dr["AttorneyId"].ToString());
                        AttorneyBL attorneyBL = new AttorneyBL();
                        Attorney attorney = attorneyBL.Get(attorneyId);
                        string attorneyName = attorney.AttorneyFirstName + " " + attorney.AttorneyLastName;
                        user.AttorneyNames = attorneyName.ToString();
                    }
                    else if (dr["AttorneyId"].ToString() == null)
                    {
                        user.AttorneyId = dr["AttorneyId"].ToString() != string.Empty ? (int?)dr["AttorneyId"] : null;
                    }

                    if (dr["CourtId"].ToString() == string.Empty)
                    {
                        user.CourtId = dr["CourtId"].ToString() != string.Empty ? (int?)dr["CourtId"] : null;
                    }
                    else if (dr["CourtId"].ToString() != string.Empty)
                    {
                        int courtId = Convert.ToInt32(dr["CourtId"].ToString());
                        CourtBL courtBL = new CourtBL();
                        Court court = courtBL.Get(courtId);
                        string courtName = court.CourtName;
                        user.CourtNames = courtName.ToString();
                    }
                    else if (dr["CourtId"].ToString() == null)
                    {
                        user.CourtId = dr["CourtId"].ToString() != string.Empty ? (int?)dr["CourtId"] : null;
                    }

                    if (dr["JudgeId"].ToString() == string.Empty)
                    {
                        user.JudgeId = dr["JudgeId"].ToString() != string.Empty ? (int?)dr["JudgeId"] : null;
                    }
                    else if (dr["JudgeId"].ToString() != string.Empty)
                    {
                        int JudgeId = Convert.ToInt32(dr["JudgeId"].ToString());
                        JudgeBL judgeBL = new JudgeBL();
                        Judge judge = judgeBL.Get(JudgeId);
                        string judgeName = judge.JudgeFirstName + " " + judge.JudgeLastName;
                        user.JudgeNames = judgeName.ToString();
                    }
                    else if (dr["JudgeId"].ToString() == null)
                    {
                        user.CourtId = dr["JudgeId"].ToString() != string.Empty ? (int?)dr["JudgeId"] : null;
                    }

                    if (user.DepartmentName != null)
                    {
                        string departName = user.DepartmentName;
                        user.UserTypeNames = departName.ToString();
                    }
                    else if (user.DonorName != null)
                    {
                        string donorName = user.DonorName;
                        user.UserTypeNames = donorName.ToString();
                    }
                    else if (user.ClientNames != null)
                    {
                        string clientNames = user.ClientNames;
                        user.UserTypeNames = clientNames.ToString();
                    }
                    else if (user.VendorNames != null)
                    {
                        string vendorNames = user.VendorNames;
                        user.UserTypeNames = vendorNames.ToString();
                    }
                    else if (user.AttorneyNames != null)
                    {
                        string attorneyNames = user.AttorneyNames;
                        user.UserTypeNames = attorneyNames.ToString();
                    }
                    else if (user.CourtNames != null)
                    {
                        string courtNames = user.CourtNames;
                        user.UserTypeNames = courtNames.ToString();
                    }
                    else if (user.JudgeNames != null)
                    {
                        string judgeNames = user.JudgeNames;
                        user.UserTypeNames = judgeNames.ToString();
                    }
                    else
                    {
                        user.UserTypeNames = null;
                    }

                    user.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    user.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    user.CreatedOn = (DateTime)dr["CreatedOn"];
                    user.CreatedBy = (string)dr["CreatedBy"];
                    user.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    user.LastModifiedBy = (string)dr["LastModifiedBy"];

                    userList.Add(user);
                }

                return userList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<User> GetList(string userEmail)
        {
            try
            {
                DataTable dtUsers = userDao.GetList(userEmail);

                List<User> userList = new List<User>();

                foreach (DataRow dr in dtUsers.Rows)
                {
                    User user = new User();

                    user.UserId = (int)dr["UserId"];
                    user.Username = (string)dr["Username"];
                    user.UserPassword = (string)dr["UserPassword"];
                    user.IsUserActive = dr["IsUserActive"].ToString() == "1" ? true : false;
                    user.UserFirstName = (string)dr["UserFirstName"];
                    user.UserLastName = dr["UserLastName"].ToString();
                    user.UserPhoneNumber = dr["UserPhoneNumber"].ToString();
                    user.UserFax = dr["UserFax"].ToString();
                    user.UserEmail = dr["UserEmail"].ToString();
                    user.ChangePasswordRequired = dr["ChangePasswordRequired"].ToString() == "1" ? true : false;
                    user.UserType = (UserType)dr["UserType"];
                    user.DepartmentId = dr["DepartmentId"].ToString() != string.Empty ? (int?)dr["DepartmentId"] : null;
                    user.DonorId = dr["DonorId"].ToString() != string.Empty ? (int?)dr["DonorId"] : null;
                    user.ClientId = dr["ClientId"].ToString() != string.Empty ? (int?)dr["ClientId"] : null;
                    user.VendorId = dr["VendorId"].ToString() != string.Empty ? (int?)dr["VendorId"] : null;
                    user.AttorneyId = dr["AttorneyId"].ToString() != string.Empty ? (int?)dr["AttorneyId"] : null;
                    user.CourtId = dr["CourtId"].ToString() != string.Empty ? (int?)dr["CourtId"] : null;
                    user.JudgeId = dr["JudgeId"].ToString() != string.Empty ? (int?)dr["JudgeId"] : null;
                    user.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    user.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    user.CreatedOn = (DateTime)dr["CreatedOn"];
                    user.CreatedBy = (string)dr["CreatedBy"];
                    user.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    user.LastModifiedBy = (string)dr["LastModifiedBy"];

                    userList.Add(user);
                }

                return userList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<User> GetListByDepartment(string departmentName)
        {
            try
            {
                DataTable dtUsers = userDao.GetListByDepartment(departmentName);

                List<User> userList = new List<User>();

                foreach (DataRow dr in dtUsers.Rows)
                {
                    User user = new User();

                    user.UserId = (int)dr["UserId"];
                    user.Username = (string)dr["Username"];
                    user.UserPassword = (string)dr["UserPassword"];
                    user.IsUserActive = dr["IsUserActive"].ToString() == "1" ? true : false;
                    user.UserFirstName = (string)dr["UserFirstName"];
                    user.UserLastName = (string)dr["UserLastName"] != string.Empty ? (string)dr["UserLastName"] : null;
                    user.UserPhoneNumber = dr["UserPhoneNumber"].ToString();
                    user.UserFax = dr["UserFax"].ToString();
                    user.UserEmail = (string)dr["UserEmail"] != string.Empty ? (string)dr["UserEmail"] : null;
                    user.ChangePasswordRequired = dr["ChangePasswordRequired"].ToString() == "1" ? true : false;
                    user.UserType = (UserType)dr["UserType"];
                    user.DepartmentId = dr["DepartmentId"].ToString() != string.Empty ? (int?)dr["DepartmentId"] : null;
                    user.DonorId = dr["DonorId"].ToString() != string.Empty ? (int?)dr["DonorId"] : null;
                    user.ClientId = dr["ClientId"].ToString() != string.Empty ? (int?)dr["ClientId"] : null;
                    user.VendorId = dr["VendorId"].ToString() != string.Empty ? (int?)dr["VendorId"] : null;
                    user.AttorneyId = dr["AttorneyId"].ToString() != string.Empty ? (int?)dr["AttorneyId"] : null;
                    user.CourtId = dr["CourtId"].ToString() != string.Empty ? (int?)dr["CourtId"] : null;
                    user.JudgeId = dr["JudgeId"].ToString() != string.Empty ? (int?)dr["JudgeId"] : null;
                    user.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    user.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    user.CreatedOn = (DateTime)dr["CreatedOn"];
                    user.CreatedBy = (string)dr["CreatedBy"];
                    user.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    user.LastModifiedBy = (string)dr["LastModifiedBy"];

                    userList.Add(user);
                }

                return userList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<User> GetListAuthorizationRulesCategories()
        {
            try
            {
                DataTable dtUsers = userDao.GetUserAuthorizationRulesCategories();

                List<User> userAuthList = new List<User>();

                foreach (DataRow dr in dtUsers.Rows)
                {
                    User user = new User();

                    user.AuthRuleCategoryId = (int)dr["AuthRuleCategories_AuthRuleCategoryId"];
                    user.AuthRuleCategoryName = dr["AuthRuleCategories_authrulecategoryname"].ToString();

                    userAuthList.Add(user);
                }
                return userAuthList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<User> GetListAuthorizationRulesSubCategories(int AuthRuleCategoryId)
        {
            try
            {
                DataTable dtUsers = userDao.GetUserAuthorizationRulesSubCategories(AuthRuleCategoryId);

                List<User> userAuthList = new List<User>();

                foreach (DataRow dr in dtUsers.Rows)
                {
                    User user = new User();
                    user.AuthRuleCategoryId = (int)dr["AuthRuleCategories_AuthRuleCategoryId"];
                    user.AuthRuleSubCategoryName = dr["AuthRuleCategories_authrulecategoryname"].ToString();

                    userAuthList.Add(user);
                }
                return userAuthList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<User> GetListAuthorizationRules(int AuthRuleCategoryId)
        {
            try
            {
                DataTable dtUsers = userDao.GetUserAuthorizationRules(AuthRuleCategoryId);

                List<User> userAuthList = new List<User>();

                foreach (DataRow dr in dtUsers.Rows)
                {
                    User user = new User();
                    user.AuthRuleCategoryId = (int)dr["AuthRuleCategoryId"];
                    user.AuthRuleName = dr["AuthRuleName"].ToString();

                    userAuthList.Add(user);
                }
                return userAuthList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetUserAuthCategories()
        {
            UserDao userDao = new UserDao();
            return userDao.GetUserAuthCategories();
        }

        public DataTable GetUserAuthRules()
        {
            UserDao userDao = new UserDao();
            return userDao.GetUserAuthRules();
        }

        public DataTable GetUserDepartment(int UserId)
        {
            try
            {
                DataTable user = userDao.GetUserDepartment(UserId);

                return user;
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
                DataTable user = userDao.GetByEmail(Email);

                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<User> Sorting(string searchparam, string usertype, bool active, string getInActive = null)

        {
            try
            {
                DataTable dtUsers = userDao.sorting(searchparam, usertype, active, getInActive);

                List<User> userList = new List<User>();

                foreach (DataRow dr in dtUsers.Rows)
                {
                    User user = new User();

                    user.UserId = (int)dr["UserId"];
                    user.Username = (string)dr["Username"];
                    user.UserPassword = (string)dr["UserPassword"];
                    user.IsUserActive = dr["IsUserActive"].ToString() == "1" ? true : false;
                    user.UserFirstName = (string)dr["UserFirstName"];
                    user.UserLastName = dr["UserLastName"].ToString() != string.Empty ? dr["UserLastName"].ToString() : null;
                    user.UserPhoneNumber = dr["UserPhoneNumber"].ToString();
                    user.UserFax = dr["UserFax"].ToString();
                    user.UserEmail = dr["UserEmail"].ToString() != string.Empty ? dr["UserEmail"].ToString() : null;
                    user.ChangePasswordRequired = dr["ChangePasswordRequired"].ToString() == "1" ? true : false;
                    user.UserType = (UserType)dr["UserType"];
                    if (dr["DepartmentId"].ToString() == string.Empty)
                    {
                        user.DepartmentId = dr["DepartmentId"].ToString() != string.Empty ? (int?)dr["DepartmentId"] : null;
                    }
                    else if (dr["DepartmentId"].ToString() != string.Empty)
                    {
                        int departmentId = Convert.ToInt32(dr["DepartmentId"].ToString());
                        DepartmentBL departmentBL = new DepartmentBL();
                        Department department = departmentBL.Get(departmentId);
                        string departmentName = department.DepartmentNameValue;
                        user.DepartmentName = departmentName.ToString();
                    }
                    else if (dr["DepartmentId"].ToString() == null)
                    {
                        user.DepartmentId = dr["DepartmentId"].ToString() != string.Empty ? (int?)dr["DepartmentId"] : null;
                    }

                    if (dr["DonorId"].ToString() == string.Empty)
                    {
                        user.DonorId = dr["DonorId"].ToString() != string.Empty ? (int?)dr["DonorId"] : null;
                    }
                    else if (dr["DonorId"].ToString() != string.Empty)
                    {
                        int donorId = Convert.ToInt32(dr["DonorId"].ToString());
                        DonorBL donorBL = new DonorBL();
                        Donor donor = donorBL.Get(donorId, "Desktop");
                        string donarName = donor.DonorFirstName + " " + donor.DonorLastName;
                        user.DonorName = donarName.ToString();
                    }
                    else if (dr["DonorId"].ToString() == null)
                    {
                        user.DonorId = dr["DonorId"].ToString() != string.Empty ? (int?)dr["DonorId"] : null;
                    }

                    if (dr["ClientId"].ToString() == string.Empty)
                    {
                        user.ClientId = dr["ClientId"].ToString() != string.Empty ? (int?)dr["ClientId"] : null;
                    }
                    else if (dr["ClientId"].ToString() != string.Empty)
                    {
                        int clientId = Convert.ToInt32(dr["ClientId"].ToString());
                        ClientBL clientBL = new ClientBL();
                        Client client = clientBL.Get(clientId);
                        string clientName = client.ClientName;
                        user.ClientNames = clientName.ToString();
                    }
                    else if (dr["ClientId"].ToString() == null)
                    {
                        user.ClientId = dr["ClientId"].ToString() != string.Empty ? (int?)dr["ClientId"] : null;
                    }

                    if (dr["VendorId"].ToString() == string.Empty)
                    {
                        user.VendorId = dr["VendorId"].ToString() != string.Empty ? (int?)dr["VendorId"] : null;
                    }
                    else if (dr["VendorId"].ToString() != string.Empty)
                    {
                        int vendorId = Convert.ToInt32(dr["VendorId"].ToString());
                        VendorBL vendorBL = new VendorBL();
                        Vendor vendor = vendorBL.Get(vendorId);
                        string vendorName = vendor.VendorName;
                        user.VendorNames = vendorName.ToString();
                    }
                    else if (dr["VendorId"].ToString() == null)
                    {
                        user.VendorId = dr["VendorId"].ToString() != string.Empty ? (int?)dr["VendorId"] : null;
                    }

                    if (dr["AttorneyId"].ToString() == string.Empty)
                    {
                        user.AttorneyId = dr["AttorneyId"].ToString() != string.Empty ? (int?)dr["AttorneyId"] : null;
                    }
                    else if (dr["AttorneyId"].ToString() != string.Empty)
                    {
                        int attorneyId = Convert.ToInt32(dr["AttorneyId"].ToString());
                        AttorneyBL attorneyBL = new AttorneyBL();
                        Attorney attorney = attorneyBL.Get(attorneyId);
                        string attorneyName = attorney.AttorneyFirstName + " " + attorney.AttorneyLastName;
                        user.AttorneyNames = attorneyName.ToString();
                    }
                    else if (dr["AttorneyId"].ToString() == null)
                    {
                        user.AttorneyId = dr["AttorneyId"].ToString() != string.Empty ? (int?)dr["AttorneyId"] : null;
                    }

                    if (dr["CourtId"].ToString() == string.Empty)
                    {
                        user.CourtId = dr["CourtId"].ToString() != string.Empty ? (int?)dr["CourtId"] : null;
                    }
                    else if (dr["CourtId"].ToString() != string.Empty)
                    {
                        int courtId = Convert.ToInt32(dr["CourtId"].ToString());
                        CourtBL courtBL = new CourtBL();
                        Court court = courtBL.Get(courtId);
                        string courtName = court.CourtName;
                        user.CourtNames = courtName.ToString();
                    }
                    else if (dr["CourtId"].ToString() == null)
                    {
                        user.CourtId = dr["CourtId"].ToString() != string.Empty ? (int?)dr["CourtId"] : null;
                    }

                    if (dr["JudgeId"].ToString() == string.Empty)
                    {
                        user.JudgeId = dr["JudgeId"].ToString() != string.Empty ? (int?)dr["JudgeId"] : null;
                    }
                    else if (dr["JudgeId"].ToString() != string.Empty)
                    {
                        int JudgeId = Convert.ToInt32(dr["JudgeId"].ToString());
                        JudgeBL judgeBL = new JudgeBL();
                        Judge judge = judgeBL.Get(JudgeId);
                        string judgeName = judge.JudgeFirstName + " " + judge.JudgeLastName;
                        user.JudgeNames = judgeName.ToString();
                    }
                    else if (dr["JudgeId"].ToString() == null)
                    {
                        user.CourtId = dr["JudgeId"].ToString() != string.Empty ? (int?)dr["JudgeId"] : null;
                    }

                    if (user.DepartmentName != null)
                    {
                        string departmentName = user.DepartmentName;
                        user.UserTypeNames = departmentName.ToString();
                    }
                    else if (user.DonorName != null)
                    {
                        string donorName = user.DonorName;
                        user.UserTypeNames = donorName.ToString();
                    }
                    else if (user.ClientNames != null)
                    {
                        string clientNames = user.ClientNames;
                        user.UserTypeNames = clientNames.ToString();
                    }
                    else if (user.VendorNames != null)
                    {
                        string vendorNames = user.VendorNames;
                        user.UserTypeNames = vendorNames.ToString();
                    }
                    else if (user.AttorneyNames != null)
                    {
                        string attorneyNames = user.AttorneyNames;
                        user.UserTypeNames = attorneyNames.ToString();
                    }
                    else if (user.CourtNames != null)
                    {
                        string courtNames = user.CourtNames;
                        user.UserTypeNames = courtNames.ToString();
                    }
                    else if (user.JudgeNames != null)
                    {
                        string judgeNames = user.JudgeNames;
                        user.UserTypeNames = judgeNames.ToString();
                    }
                    else
                    {
                        user.UserTypeNames = null;
                    }

                    user.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    user.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    user.CreatedOn = (DateTime)dr["CreatedOn"];
                    user.CreatedBy = (string)dr["CreatedBy"];
                    user.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    user.LastModifiedBy = (string)dr["LastModifiedBy"];

                    userList.Add(user);
                }

                return userList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UserSave(User user)
        {
            if (user.UserId == 0)
            {
                return userDao.Insert(user);
            }
            else
            {
                return userDao.UserUpdate(user);
            }
        }

        public List<User> GetVendorName(int vendorId)
        {
            try
            {
                DataTable dtUser = userDao.GetVendorName(vendorId);

                List<User> userVendorList = new List<User>();

                foreach (DataRow dr in dtUser.Rows)
                {
                    User user = new User();
                    user.UserId = (int)dr["UserId"];
                    user.UserFirstName = (string)dr["UserFirstName"];
                    user.UserLastName = (string)dr["UserLastName"] != string.Empty ? (string)dr["UserLastName"] : null;

                    userVendorList.Add(user);
                }
                return userVendorList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<User> GetList(int vendorId)
        {
            try
            {
                DataTable dtUsers = userDao.GetVendorName(vendorId);

                List<User> userList = new List<User>();

                foreach (DataRow dr in dtUsers.Rows)
                {
                    User user = new User();

                    user.UserId = (int)dr["UserId"];
                    user.Username = (string)dr["Username"];
                    user.UserPassword = (string)dr["UserPassword"];
                    user.IsUserActive = dr["IsUserActive"].ToString() == "1" ? true : false;
                    user.UserFirstName = (string)dr["UserFirstName"];
                    user.UserLastName = dr["UserLastName"].ToString();
                    user.UserPhoneNumber = dr["UserPhoneNumber"].ToString();
                    user.UserFax = dr["UserFax"].ToString();
                    user.UserEmail = dr["UserEmail"].ToString();
                    user.ChangePasswordRequired = dr["ChangePasswordRequired"].ToString() == "1" ? true : false;
                    user.UserType = (UserType)dr["UserType"];
                    user.DepartmentId = dr["DepartmentId"].ToString() != string.Empty ? (int?)dr["DepartmentId"] : null;
                    user.DonorId = dr["DonorId"].ToString() != string.Empty ? (int?)dr["DonorId"] : null;
                    user.ClientId = dr["ClientId"].ToString() != string.Empty ? (int?)dr["ClientId"] : null;
                    user.VendorId = dr["VendorId"].ToString() != string.Empty ? (int?)dr["VendorId"] : null;
                    user.AttorneyId = dr["AttorneyId"].ToString() != string.Empty ? (int?)dr["AttorneyId"] : null;
                    user.CourtId = dr["CourtId"].ToString() != string.Empty ? (int?)dr["CourtId"] : null;
                    user.JudgeId = dr["JudgeId"].ToString() != string.Empty ? (int?)dr["JudgeId"] : null;
                    user.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    user.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    user.CreatedOn = (DateTime)dr["CreatedOn"];
                    user.CreatedBy = (string)dr["CreatedBy"];
                    user.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    user.LastModifiedBy = (string)dr["LastModifiedBy"];

                    userList.Add(user);
                }

                return userList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion Public Methods
    }
}