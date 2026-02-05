using Serilog;
using SurPath.Entity;
using SurPath.Enum;
using System;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SurPath
{
    internal static class Program
    {
        
        public static FrmMain frmMain;
        public static string superAdmin = ConfigurationManager.AppSettings["SuperAdmin"].ToString().Trim();
        public static string superAdmin1 = ConfigurationManager.AppSettings["SuperAdmin1"].ToString().Trim();
        public static string superAdmin2 = ConfigurationManager.AppSettings["SuperAdmin2"].ToString().Trim();
        
        public static string currentUserName;
        public static int currentUserId = 0;
        public static UserType currentUserType = UserType.None;
        public static DataTable dtUserAuthRules = null;
        public static ILogger _logger;

        // public static Regex regexEmail = new Regex(@"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$");
        public static Regex regexEmail = new Regex(@"^(?("")("".+?(?<!\\)""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]*\.)+[a-zA-Z0-9][\-a-zA-Z0-9]{0,22}[a-zA-Z0-9]))$");

        public static Regex regexPhoneNumber = new Regex(@"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}");
        public static Regex regexZipCode = new Regex(@"^\d{5}(?:[-\s]\d{4})?$");
        public static Regex regexMoney = new Regex(@"^\$?(\.\d{1,3}(\,\d{3})*|(\d+))(\.\d{2})?$");
        public static CultureInfo culture = new CultureInfo(ConfigurationManager.AppSettings["Culture"].ToString().Trim());
        public static bool IsProduction { get; set; }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            try
            {
                _logger = new LoggerConfiguration().ReadFrom.AppSettings().CreateLogger();

                _logger.Information("Logger Loaded - program start main");

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.CurrentCulture = culture;
                var FormFoxPDFFolder = ConfigurationManager.AppSettings["FormFoxPDFFolder"].ToString().Trim();
                Directory.CreateDirectory(FormFoxPDFFolder);
                var FormFoxDumpFolder = ConfigurationManager.AppSettings["FormFoxDumpFolder"].ToString().Trim();
                Directory.CreateDirectory(FormFoxDumpFolder);

                MessageBoxManager.Register(); // for custom message boxes


                FrmLogin fvrmLogin = new FrmLogin();

                if (fvrmLogin.ShowDialog() == DialogResult.OK)
                {
                    SurPath.Business.UserAuthentication userAuthBL = new Business.UserAuthentication();

                    User user = userAuthBL.GetByUsernameOrEmail(currentUserName);

                    currentUserId = user.UserId;
                    currentUserType = user.UserType;

                    if (user.ChangePasswordRequired)
                    {
                        FrmChangePassword frmChangePassword = new FrmChangePassword("ChangePasswordRequired");

                        if (frmChangePassword.ShowDialog() != DialogResult.OK)
                        {
                            return;
                        }
                    }

                    if (!(currentUserName.ToUpper() == superAdmin.ToUpper() || currentUserName.ToUpper() == superAdmin1.ToUpper() || currentUserName.ToUpper() == superAdmin2.ToUpper()))
                    {
                        dtUserAuthRules = userAuthBL.GetUserAuthorizationRules(currentUserName);
                    }

                    frmMain = new FrmMain();
                    bool _isProduction = true;
                    bool.TryParse(ConfigurationManager.AppSettings["Production"].ToString().Trim(), out _isProduction);
                    if (!_isProduction) frmMain.Text = frmMain.Text + " " + "THIS IS STAGING, NOT PRODUCTION".ToUpper();
                    Program.IsProduction = _isProduction;
                    Application.Run(frmMain);
                    
                }
                else
                {
                    _logger.Debug($"Exiting application");
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
                _logger.Debug($"Trying to send panic email");
                _logger.Error(ex.Message);
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                _logger.Debug(ex.StackTrace);
                //PanicEmail(ex);
                throw;
            }
            Application.Exit();
                }

        public static void LogError(Exception ex)
        {
            Program._logger.Error(ex.ToString());
            if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
            if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
        }
        private static void PanicEmail(Exception ex)
        {
            _logger.Debug("Sending Panic Email");
            SmtpClient smtpClient = new SmtpClient();
            NetworkCredential basicCredential = new NetworkCredential(ConfigurationManager.AppSettings["SmtpFromAddress"].ToString().Trim(), ConfigurationManager.AppSettings["SmtpFromAddressPassword"].ToString().Trim());
            MailMessage message = new MailMessage();
            MailAddress fromAddress = new MailAddress(ConfigurationManager.AppSettings["SmtpFromAddress"].ToString().Trim());

            smtpClient.Host = ConfigurationManager.AppSettings["SmtpHost"].ToString().Trim();
            smtpClient.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"].ToString().Trim());
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = basicCredential;
            smtpClient.EnableSsl = true;

            message.From = fromAddress;
            message.Subject = "SURPATH CRASH" + DateTime.Now.ToString("yyyyy MMMM dd (dddd), HH:mm:ss");

            message.IsBodyHtml = true;
            string mailBody = string.Empty;
            mailBody += "SEND INS HAVE HALTED DUE TO A SERVICE PANIC\r\n";
            mailBody += ex.Message + "\r\n";
            if (ex.InnerException != null) mailBody += ex.InnerException.ToString() + "\r\n";
            mailBody += ex.StackTrace + "\r\n";
            message.Body = mailBody;
            message.To.Add("chris@inzi.com");
            //message.To.Add("david@surscan.com");

            try
            {
                smtpClient.Send(message);
            }
            catch (Exception ex2)
            {
                _logger.Error("Unable to send panic email");
                _logger.Error(ex2.Message);
                if (ex2.InnerException != null) _logger.Error(ex2.InnerException.ToString());
                _logger.Error(ex2.StackTrace);
                throw ex2;
                //
            }
        }

        public static void EmailError(Exception ex)
        {
            _logger.Debug("Sending Error Email");
            SmtpClient smtpClient = new SmtpClient();
            NetworkCredential basicCredential = new NetworkCredential(ConfigurationManager.AppSettings["SmtpFromAddress"].ToString().Trim(), ConfigurationManager.AppSettings["SmtpFromAddressPassword"].ToString().Trim());
            MailMessage message = new MailMessage();
            MailAddress fromAddress = new MailAddress(ConfigurationManager.AppSettings["SmtpFromAddress"].ToString().Trim());

            smtpClient.Host = ConfigurationManager.AppSettings["SmtpHost"].ToString().Trim();
            smtpClient.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"].ToString().Trim());
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = basicCredential;
            smtpClient.EnableSsl = true;

            message.From = fromAddress;
            message.Subject = "SURPATH ERROR" + DateTime.Now.ToString("yyyyy MMMM dd (dddd), HH:mm:ss");

            message.IsBodyHtml = true;
            string mailBody = string.Empty;
            mailBody += "ERROR EVENT IN WINDOWS APP\r\n";
            mailBody += ex.Message + "\r\n";
            if (ex.InnerException != null) mailBody += ex.InnerException.ToString() + "\r\n";
            mailBody += ex.StackTrace + "\r\n";
            message.Body = mailBody;
            message.To.Add("chris@inzi.com");
            //message.To.Add("david@surscan.com");

            try
            {
                smtpClient.Send(message);
            }
            catch (Exception ex2)
            {
                _logger.Error("Unable to send panic email");
                _logger.Error(ex2.Message);
                if (ex2.InnerException != null) _logger.Error(ex2.InnerException.ToString());
                _logger.Error(ex2.StackTrace);
                throw ex2;
                //
            }
        }

    }
   
    public static class EnumExtensions
    {
        public static string ToDescriptionString(this SurPath.Enum.TestCategories val)
        {
            System.ComponentModel.DescriptionAttribute[] attributes = (System.ComponentModel.DescriptionAttribute[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }

        public static string ToDescriptionString(this SurPath.Enum.AuthorizationCategories val)
        {
            System.ComponentModel.DescriptionAttribute[] attributes = (System.ComponentModel.DescriptionAttribute[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }

        public static string ToDescriptionString(this SurPath.Enum.AuthorizationRules val)
        {
            System.ComponentModel.DescriptionAttribute[] attributes = (System.ComponentModel.DescriptionAttribute[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }

        public static string ToDescriptionString(this SurPath.Enum.VendorTypes val)
        {
            System.ComponentModel.DescriptionAttribute[] attributes = (System.ComponentModel.DescriptionAttribute[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }

        public static string ToDescriptionString(this SurPath.Enum.VendorStatus val)
        {
            System.ComponentModel.DescriptionAttribute[] attributes = (System.ComponentModel.DescriptionAttribute[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }

        public static string ToDescriptionString(this SurPath.Enum.DonorRegistrationStatus val)
        {
            System.ComponentModel.DescriptionAttribute[] attributes = (System.ComponentModel.DescriptionAttribute[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }

        public static string ToDescriptionString(this SurPath.Enum.DonorActivityCategories val)
        {
            System.ComponentModel.DescriptionAttribute[] attributes = (System.ComponentModel.DescriptionAttribute[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }

        public static string ToDescriptionString(this SurPath.Enum.OverAllTestResult val)
        {
            System.ComponentModel.DescriptionAttribute[] attributes = (System.ComponentModel.DescriptionAttribute[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }

        public static string ToDescriptionString(this SurPath.Enum.ClientPaymentTypes val)
        {
            System.ComponentModel.DescriptionAttribute[] attributes = (System.ComponentModel.DescriptionAttribute[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }

        public static string ToDescriptionString(this SurPath.Enum.TestInfoReasonForTest val)
        {
            System.ComponentModel.DescriptionAttribute[] attributes = (System.ComponentModel.DescriptionAttribute[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }
    }
}