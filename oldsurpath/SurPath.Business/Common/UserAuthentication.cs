using SurPath.Data;
using SurPath.Entity;
using System;
using System.Configuration;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace SurPath.Business
{
    public class UserAuthentication : BusinessObject
    {
        #region Constructor

        public UserAuthentication()
        {
            //
        }

        #endregion Constructor

        #region Public Methods

        public string EncryptedPW(string pw)
        {
            return Encrypt(pw, true);
        }


        public bool CheckPassword(string username, string password)
        {
            bool retval = false;

            try
            {
                UserDao userDao = new UserDao();
                User user = userDao.Get(username);
                retval = Encrypt(password, true) == user.UserPassword;
            }
            catch (Exception)
            {

                throw;
            }
            return retval;
        }

        public Tuple<bool, int, int> ValidateUser(string username, string password)
        {
            Tuple<bool, int, int> returnValue = new Tuple<bool, int, int>(false, 0, 0);

            try
            {
                UserDao userDao = new UserDao();
                User user = userDao.Get(username);

                if (user != null)
                {
                    if (Encrypt(password, true) == user.UserPassword)
                    {
                        if (user.DonorId != null)
                        {
                            DonorBL donorBL = new DonorBL();
                            Donor donor = donorBL.Get(Convert.ToInt32(user.DonorId), "Desktop");

                            if (donor.DonorRegistrationStatusValue == Enum.DonorRegistrationStatus.PreRegistration)
                            {
                                throw new Exception("Your activation is PENDING. Please open the email SurScan sent you. Use the temporary password provided in the email, and CLICK the activation link.");
                            }
                            returnValue = new Tuple<bool, int, int>(true, user.UserId, (int)user.DonorId);
                        }
                        else
                        {
                            returnValue = new Tuple<bool, int, int>(true, user.UserId, 0);
                        }


                    }
                    else
                    {
                        user = userDao.GetWithPassword(username, Encrypt(password, true));
                        if (user != null)
                        {
                            if (user.DonorId != null)
                            {
                                DonorBL donorBL = new DonorBL();
                                Donor donor = donorBL.Get(Convert.ToInt32(user.DonorId), "Desktop");

                                if (donor.DonorRegistrationStatusValue == Enum.DonorRegistrationStatus.PreRegistration)
                                {
                                    throw new Exception("Your activation is PENDING. Please open the email SurScan sent you. Use the temporary password provided in the email, and CLICK the activation link.");
                                }
                                returnValue = new Tuple<bool, int, int>(true, user.UserId, (int)user.DonorId);
                            }
                            else
                            {
                                returnValue = new Tuple<bool, int, int>(true, user.UserId, 0);
                            }
                        }
                    }
                }




            }
            catch (Exception ex)
            {
                throw ex;
            }

            return returnValue;
        }

        public Tuple<bool, int, int> AuthDonorByUsernamePasswordAndDepartmentName(string username, string password, string department_name)
        {
            Tuple<bool, int, int> returnValue = new Tuple<bool, int, int>(false, 0, 0);

            try
            {
                UserDao userDao = new UserDao();
                var res = userDao.GetUserByUsernamePasswordAndDepartmentName(username, Encrypt(password, true), department_name);
                var user = res.Item1;
                var autenticated = res.Item2;


                if (user.DonorId != null)
                {
                    DonorBL donorBL = new DonorBL();
                    Donor donor = donorBL.Get(Convert.ToInt32(user.DonorId), "Desktop");

                    if (donor.DonorRegistrationStatusValue == Enum.DonorRegistrationStatus.PreRegistration)
                    {
                        throw new Exception("Your activation is PENDING. Please open the email SurScan sent you. Use the temporary password provided in the email, and CLICK the activation link.");
                    }
                    returnValue = new Tuple<bool, int, int>(autenticated, user.UserId, (int)user.DonorId);
                }

                
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return returnValue;
        }

        

        public DataTable GetUserAuthorizationRules(string username)
        {
            UserDao userDao = new UserDao();
            return userDao.GetUserAuthorizationRules(username);
        }

        public User GetByUsernameOrEmail(string usernameOrEmail)
        {
            try
            {
                UserDao userDao = new UserDao();
                User user = userDao.GetByUsernameOrEmail(usernameOrEmail);

                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public User GetByUserById(int user_id)
        {
            try
            {
                UserDao userDao = new UserDao();
                User user = userDao.GetByUserID(user_id);

                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SendForgotPassword(string usernameOrEmail)
        {
            bool returnValue = false;

            UserDao userDao = new UserDao();
            User user = userDao.GetByUsernameOrEmail(usernameOrEmail);

            if (user != null)
            {
                RandomStringGenerator rsg = new RandomStringGenerator(true, false, true, false);
                string newPassword = rsg.Generate(6, 8);
                string newPasswordEncrypted = Encrypt(newPassword, true);

                userDao.ResetPassword(usernameOrEmail, newPasswordEncrypted, "SYSTEM");

                user = userDao.GetByUsernameOrEmail(usernameOrEmail);

                //Send the mail with newPassword and other donor information
                try
                {
                    MailManager mailManager = new MailManager();
                    string mailBody = mailManager.SendRegistrationMail(user, "ForgotPassword");
                    mailManager.SendMail(user.UserEmail, ConfigurationManager.AppSettings["ForgotPasswordMailSubject"].ToString().Trim(), mailBody);
                }
                catch
                {
                    throw new Exception("Not able to send mail.");
                }

                returnValue = true;
            }
            else
            {
                throw new Exception("Invalid Username or Email.");
            }

            return returnValue;
        }

        public User SendForgotPasswordWeb(string usernameOrEmail)
        {
            try
            {
                UserDao userDao = new UserDao();
                User user = userDao.GetByUsernameOrEmail(usernameOrEmail);

                if (user != null)
                {
                    RandomStringGenerator rsg = new RandomStringGenerator(true, false, true, false);
                    string newPassword = rsg.Generate(6, 8);
                    string newPasswordEncrypted = Encrypt(newPassword, true);

                    userDao.ResetPassword(usernameOrEmail, newPasswordEncrypted, "SYSTEM");

                    user = userDao.GetByUsernameOrEmail(usernameOrEmail);
                }
                else
                {
                    throw new Exception("Invalid Username or Email.");
                }

                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion Public Methods

        #region Private Methods

        public static string EncryptWithKey(string toEncrypt, string key)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            hashmd5.Clear();

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tdes.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        public static string DecryptWithKey(string cipherString, string key)
        {
            byte[] keyArray;
            byte[] toEncryptArray = Convert.FromBase64String(cipherString);

            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            hashmd5.Clear();

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tdes.Clear();
            return UTF8Encoding.UTF8.GetString(resultArray);
        }


        /// <summary>
        /// Encrypt a string using dual encryption method. Return a encrypted cipher Text
        /// </summary>
        /// <param name="toEncrypt">string to be encrypted</param>
        /// <param name="useHashing">use hashing? send to for extra secirity</param>
        /// <returns></returns>
        public static string Encrypt(string toEncrypt, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            string key = "4EVRZIuKQFKCbjvtKi2KwA==";
            //System.Windows.Forms.MessageBox.Show(key);
            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tdes.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        /// DeCrypt a string using dual encryption method. Return a DeCrypted clear string
        /// </summary>
        /// <param name="cipherString">encrypted string</param>
        /// <param name="useHashing">Did you use hashing to encrypt this data? pass true is yes</param>
        /// <returns></returns>
        public static string Decrypt(string cipherString, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = Convert.FromBase64String(cipherString);
            //cipherString = cipherString.Replace("+", " ");
            //cipherString = cipherString.Replace("/", " ");
            string key = "4EVRZIuKQFKCbjvtKi2KwA==";

            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tdes.Clear();
            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        public static string URLIDEncrypt(string toEncrypt, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            string key = "4EVRZIuKQFKCbjvtKi2KwA==";
            //System.Windows.Forms.MessageBox.Show(key);
            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tdes.Clear();
            // return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            string strBase64 = Convert.ToBase64String(resultArray, 0, resultArray.Length);
            return strBase64.Replace('+', '-').Replace('/', '_').Replace('=', ',');
        }

        public static string URLIDDecrypt(string cipherString, bool useHashing)
        {
            byte[] keyArray;
            cipherString = cipherString.Replace('-', '+').Replace('_', '/').Replace(',', '=');
            byte[] toEncryptArray = Convert.FromBase64String(cipherString);
            //cipherString = cipherString.Replace("+", " ");
            //cipherString = cipherString.Replace("/", " ");
            string key = "4EVRZIuKQFKCbjvtKi2KwA==";

            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tdes.Clear();
            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        #endregion Private Methods
    }
}