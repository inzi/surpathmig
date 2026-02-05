using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;

namespace SurpathBackend
{
    public partial class SurpathService : ServiceBase
    {
        // Our Service Configuration
        public IConfigurationRoot ConfigurationRoot;

        // Our Logger
        public static ILogger _logger;

        // Service Control
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        private readonly ManualResetEvent _runCompleteEvent = new ManualResetEvent(false);

        // Service Settings
        private int RunIntervalMS;  // Time delay between processing

        public SurpathService(ILogger __logger)
        {
            _logger = __logger; // new LoggerConfiguration().ReadFrom.AppSettings().CreateLogger();
            _logger.Debug("SurpathService loaded. Initializing");
            this.RunIntervalMS = 5 * 60 * 1000; // 5 minutes default
            int RunInterval = this.RunIntervalMS / 1000;
            if (ConfigurationManager.AppSettings["RunInterval"] != null)
            {
                RunInterval = this.RunIntervalMS / 1000;
                _logger.Debug($"RunInterval set in app config. Loading to override the default of {RunInterval}");

                int.TryParse(ConfigurationManager.AppSettings["RunInterval"].ToString().Trim(), out RunInterval);
                this.RunIntervalMS = RunInterval * 1000;
            }
            RunInterval = this.RunIntervalMS / 1000;
            _logger.Debug($"RunInterval in ms {this.RunIntervalMS}. In seconds {RunInterval}");

            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // 5 second warmup  - for no reason other than to give an admin a lifeline if they start it by accident
            Thread.Sleep(5000);
            StartupEmail();
            try
            {
                _logger.Debug("Service On Start.");
                Task.Run(() => RunAsync(_cancellationTokenSource.Token));
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                _logger.Debug(ex.StackTrace);
                PanicEmail(ex);
            }
            finally
            {
                _runCompleteEvent.Set();
            }
        }

        protected override void OnStop()
        {
            try
            {
                _logger.Information("SurPath Backend: Shutting down");
                _cancellationTokenSource.Cancel();
                _runCompleteEvent.WaitOne();
                ShutdownEmail();

            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                _logger.Debug(ex.StackTrace);
                PanicEmail(ex);
            }
        }

        /// <summary>
        /// Async main task - needs a cancelation token
        /// </summary>
        /// <param name="cancellationToken">Plain CancellationToken</param>
        /// <returns></returns>
        public async Task RunAsync(CancellationToken cancellationToken)
        {
            try
            {
                _logger.Debug("Surpath Backend Service: Starting services");
                SurpathBackendWorkerWrapper worker = new SurpathBackendWorkerWrapper(_logger, ConfigurationRoot);
                while (!cancellationToken.IsCancellationRequested)
                {
                    // Refresh our runtime settings
                    //UpdateRuntimeSettings();

                    //Define the task to be performed
                    var tasks = new List<Task>
                    {
                       // Do our work here
                       worker.Process(cancellationToken) ?? Task.Delay(TimeSpan.FromSeconds(30)),

                       Task.Delay(TimeSpan.FromSeconds(30))     // Small delay - avoid race condition for service loop
                    };
                    _logger.Debug("Waiting for all process to complete..");
                    await Task.WhenAll(tasks.ToArray());
                    _logger.Debug($"Process complete. Sleeping for {RunIntervalMS / 1000} seconds....");
                    _logger.Debug($"Next run at: {DateTime.Now.Add(TimeSpan.FromMilliseconds(RunIntervalMS))}");

                    // Sleep until next run
                    Thread.Sleep(RunIntervalMS);
                }
                _logger.Information("Surpath Backend Service: Stopping services");
            }
            catch (Exception ex)
            {
                _logger.Information("Surpath Backend Service: Failed during execution and has to stop");
                _logger.Error(ex.Message);
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                _logger.Debug(ex.StackTrace);
                PanicEmail(ex);
                Stop();     //Stop the service
            }
        }

        public void PanicEmail(Exception ex)
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
            message.Subject = "BACKEND SERVICE PANIC PANIC PANIC" + DateTime.Now.ToString("yyyyy MMMM dd (dddd), HH:mm:ss");
            if (Program.IsProduction == false) message.Subject = "(STAGING) " + message.Subject;

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

        public void StartupEmail()
        {
            _logger.Debug("Sending Startup Email");
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
            //message.Subject = "BACKEND SERVICE START " + DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss");
            message.Subject = "BACKEND SERVICE START " + DateTime.Now.ToString("yyyyy MMMM dd (dddd), HH:mm:ss");
            if (Program.IsProduction == false) message.Subject = "(STAGING) " + message.Subject;

            message.IsBodyHtml = true;
            string mailBody = string.Empty;
            mailBody += "Backend service is starting, send ins are being processed";
            message.Body = mailBody;
            message.To.Add("chris@inzi.com");
            message.To.Add("david@surscan.com");

            try
            {
                smtpClient.Send(message);
            }
            catch (Exception ex2)
            {
                _logger.Error("Unable to send startup email");
                _logger.Error(ex2.Message);
                if (ex2.InnerException != null) _logger.Error(ex2.InnerException.ToString());
                _logger.Error(ex2.StackTrace);
                throw ex2;
                //
            }
        }


        public void ShutdownEmail()
        {
            _logger.Debug("Sending Stop Email");
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
            //message.Subject = "BACKEND SERVICE START " + DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss");
            message.Subject = "BACKEND SERVICE NORMAL STOP " + DateTime.Now.ToString("yyyyy MMMM dd (dddd), HH:mm:ss");
            if (Program.IsProduction == false) message.Subject = "(STAGING) " + message.Subject;

            message.IsBodyHtml = true;
            string mailBody = string.Empty;
            mailBody += "Backend service is stopping normally, send ins are not being processed";
            message.Body = mailBody;
            message.To.Add("chris@inzi.com");
            message.To.Add("david@surscan.com");

            try
            {
                smtpClient.Send(message);
            }
            catch (Exception ex2)
            {
                _logger.Error("Unable to stop startup email");
                _logger.Error(ex2.Message);
                if (ex2.InnerException != null) _logger.Error(ex2.InnerException.ToString());
                _logger.Error(ex2.StackTrace);
                throw ex2;
                //
            }
        }
    }
}