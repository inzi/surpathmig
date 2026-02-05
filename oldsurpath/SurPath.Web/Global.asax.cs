using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Serilog;

namespace SurPathWeb
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static ILogger _logger;
        public static bool Production = false;
        public static bool Dev = false;

        protected void Application_Start()
        {
            try
            {
                MvcApplication._logger = new LoggerConfiguration().ReadFrom.AppSettings().CreateLogger();

                _logger.Information("Web app started");

                AreaRegistration.RegisterAllAreas();

                //WebApiConfig.Register(GlobalConfiguration.Configuration);
                GlobalConfiguration.Configure(WebApiConfig.Register);

                FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
                RouteConfig.RegisterRoutes(RouteTable.Routes);
                BundleConfig.RegisterBundles(BundleTable.Bundles);
                _logger.Information("Web app ready");
                //throw new DivideByZeroException();

                var FormFoxPDFFolder = ConfigurationManager.AppSettings["FormFoxPDFFolder"].ToString().Trim();
                Directory.CreateDirectory(FormFoxPDFFolder);
                var FormFoxDumpFolder = ConfigurationManager.AppSettings["FormFoxDumpFolder"].ToString().Trim();
                Directory.CreateDirectory(FormFoxDumpFolder);

                var _production = ConfigurationManager.AppSettings["Production"].ToString().Trim();
                bool.TryParse(_production, out bool isProduction);
                MvcApplication.Production = isProduction;

                if (ConfigurationManager.AppSettings.AllKeys.Contains("Dev"))
                {
                    var _dev = ConfigurationManager.AppSettings["Dev"].ToString().Trim();
                    bool.TryParse(_dev, out bool isDev);
                    MvcApplication.Dev = isDev;
                }


            }
            catch (Exception ex)
            {
                PanicEmail(ex);
                throw;
            }

        }
        public static void LogError(Exception ex)
        {
            MvcApplication._logger.Error(ex.ToString());
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
            message.Subject = "SURPATH WEB CRASH" + DateTime.Now.ToString("yyyyy MMMM dd (dddd), HH:mm:ss");

            message.IsBodyHtml = true;
            string mailBody = string.Empty;
            mailBody += "WEB APP CRASH\r\n";
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
}