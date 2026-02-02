using CommandLine;
using CommandLine.Text;
using Serilog;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace HL7ParserService
{
    internal static class Program
    {
        private static Options options;
        public static ServiceSettings serviceSettings;
        public static ILogger _logger;
        public static bool IsProduction;
        public class ServiceSettings
        {
            // Consider grabbing these from a config for settings.json file - or even as switches. [add to options with default values]
            public string ServiceName { get; set; } = "Surpath.HL7Parser";

            public string ServiceDisplayName { get; set; } = "Surpath.HL7Parser";
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main(string[] args)
        {
            try
            {
                bool _isProduction = false;
                bool.TryParse(ConfigurationManager.AppSettings["Production"].ToString().Trim(), out _isProduction);
                IsProduction = _isProduction;

                _logger = new LoggerConfiguration().ReadFrom.AppSettings().CreateLogger();

                _logger.Information("Logger Loaded - program start main");
                serviceSettings = new ServiceSettings();
                _logger.Debug("Logger Loaded - Debug Level visible");
                if (System.Environment.UserInteractive)
                {
                    _logger.Debug("Interactive mode");

                    options = new Options();

                    Parser.Default.ParseArguments(args, options);
                    RunOptionsAndReturnExitCode();
                }
                _logger.Debug("Running as service");
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                    new HL7ParserServiceClass(_logger)
                };
                _logger.Debug("Running Service");
                ServiceBase.Run(ServicesToRun);
            }
            catch (Exception ex)
            {
                PanicEmail(ex);
                throw;
            }
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
            message.Subject = "HL7 PARSER SERVICE PANIC" + DateTime.Now.ToString("yyyyy MMMM dd (dddd), HH:mm:ss");

            message.IsBodyHtml = true;
            string mailBody = string.Empty;
            mailBody += "The HL7Service has stopped\r\n";
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
        private static void RunOptionsAndReturnExitCode()
        {
            bool _mgmt = false;
            if (options.Go)
            {
                Go();
                _mgmt = true;
            }
            if (options.StageTest)
            {
                RunStageTest();
                _mgmt = true;
            }
            if (_mgmt == true)
            {
                Console.WriteLine($"Press any key to close");
                Console.ReadKey();
            }
            if (options.Install == false && options.Uninstall == false && _mgmt == false)
            {
                // One or the other is required
                ShowHelp();
            }
            // If here - we're installing or uninstalling

            if (!string.IsNullOrEmpty(options.ServiceName)) serviceSettings.ServiceName = options.ServiceName;
            if (!string.IsNullOrEmpty(options.ServiceDisplayName)) serviceSettings.ServiceDisplayName = options.ServiceDisplayName;

            if (options.Install)
            {
                Install();
            }
            if (options.Uninstall)
            {
                Uninstall();
            }

            Environment.Exit((int)ExitCode.Success);
        }

        private static void ShowHelp()
        {
            Console.WriteLine(options.GetUsage());
            Console.WriteLine("");
            Console.WriteLine("PDF Engine Globals:");
            //backendFiles.ShowPDFEngineGlobals();
            Environment.Exit((int)ExitCode.Success);
        }

        private static void Install()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;

            UriBuilder uri = new UriBuilder(codeBase);

            string assemblyPath = Uri.UnescapeDataString(uri.Path);

            ServiceInstaller serviceInstaller = new ServiceInstaller();
            string account = options.Account;
            string password = options.Password;

            Console.WriteLine($"Installing - {serviceSettings.ServiceName} - {assemblyPath}");

            if (!string.IsNullOrEmpty(account))
            {
                Console.WriteLine($"Setting credentials to {account} with password {password}");
            }

            bool _success = serviceInstaller.Install(serviceSettings.ServiceName, serviceSettings.ServiceDisplayName, assemblyPath, account, password, "Automatic", options.Start);

            Console.WriteLine("Service installed, modify service for credentials if necessary.");

            Environment.Exit((int)ExitCode.Success);
        }

        private static void Uninstall()
        {
            ServiceInstaller serviceInstaller = new ServiceInstaller();
            serviceInstaller.Uninstall(serviceSettings.ServiceName);
            Environment.Exit((int)ExitCode.Success);
        }

        private static void Go()
        {
            HL7ParserWorker workerDebug = new HL7ParserWorker(_logger);
            workerDebug.Work();
        }

        private static void RunStageTest()
        {
            try
            {
                _logger.Information("Project Concert Stage Test Utility");
                _logger.Information("===================================");

                int? donorId = null;
                if (!string.IsNullOrEmpty(options.DonorId))
                {
                    if (!int.TryParse(options.DonorId, out int parsedDonorId))
                    {
                        _logger.Error("Invalid donor ID. Please provide a numeric donor ID.");
                        return;
                    }
                    donorId = parsedDonorId;
                }

                string connectionString = ConfigurationManager.AppSettings["ConnectionString"];
                if (string.IsNullOrEmpty(connectionString))
                {
                    _logger.Error("Connection string not found in configuration!");
                    return;
                }

                using (var conn = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
                {
                    conn.Open();

                    if (donorId.HasValue && !options.NoReset)
                    {
                        _logger.Information($"Setting donor {donorId} test status to 4...");
                        string updateQuery = @"
                            UPDATE donor_test_info 
                            SET test_status = 4,
                                last_modified_by = 'StageTest',
                                last_modified_on = NOW()
                            WHERE donor_id = @donorId
                            AND test_status != 4
                            ORDER BY donor_test_info_id DESC
                            LIMIT 1";

                        using (var cmd = new MySql.Data.MySqlClient.MySqlCommand(updateQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@donorId", donorId);
                            int rowsAffected = cmd.ExecuteNonQuery();
                            
                            if (rowsAffected > 0)
                                _logger.Information($"Successfully updated test status for donor {donorId}");
                            else
                                _logger.Warning($"No rows updated. Donor {donorId} may already have test_status = 4");
                        }
                    }
                }

                _logger.Information("Running HL7Stage Generator...");
                HL7Stage stage = new HL7Stage(_logger);
                bool success = stage.Gen(donorId);

                if (success)
                {
                    _logger.Information("HL7Stage generation completed successfully!");
                    _logger.Information($"Lab Reports: {ConfigurationManager.AppSettings["LabReportFilePath"]}");
                    _logger.Information($"MRO Reports: {ConfigurationManager.AppSettings["MROReportFileInboundPath"]}");
                }
                else
                {
                    _logger.Error("HL7Stage generation failed!");
                }

                if (donorId.HasValue && options.ResetAfter)
                {
                    using (var conn = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
                    {
                        conn.Open();
                        string resetQuery = @"
                            UPDATE donor_test_info 
                            SET test_status = 6,
                                last_modified_by = 'StageTest-Reset',
                                last_modified_on = NOW()
                            WHERE donor_id = @donorId
                            AND test_status = 4
                            ORDER BY donor_test_info_id DESC
                            LIMIT 1";

                        using (var cmd = new MySql.Data.MySqlClient.MySqlCommand(resetQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@donorId", donorId);
                            cmd.ExecuteNonQuery();
                            _logger.Information($"Reset donor {donorId} test status back to 6");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred during stage test");
            }
        }
    }
    public class ServiceInstaller //: Installer
    {
        private string strServiceName = "";

        public ServiceInstaller()
        {
        }

        public bool Install(string name, string displayName, string binPath, string userName, string unecryptedPassword, string startupType, bool startnow = false)
        {
            bool result = false;
            try
            {
                strServiceName = name;
                // Determine statuptype
                string startupTypeConverted = string.Empty;
                switch (startupType)
                {
                    case "Automatic":
                        startupTypeConverted = "auto";
                        break;

                    case "Disabled":
                        startupTypeConverted = "disabled";
                        break;

                    case "Manual":
                        startupTypeConverted = "demand";
                        break;

                    default:
                        startupTypeConverted = "auto";
                        break;
                }

                // Determine if service has to be created (Create) or edited (Config)
                StringBuilder builder = new StringBuilder();
                ServiceController control = ServiceController.GetServices().FirstOrDefault(s => s.ServiceName == name);
                if (control == null)
                {
                    Console.WriteLine("Service doesn't exit, creating...");
                    builder.AppendFormat("{0} {1} ", "Create", name);
                }
                else
                {
                    Console.WriteLine("Service exists, updating...");

                    builder.AppendFormat("{0} {1} ", "Config", name);
                }

                builder.AppendFormat("binPath= \"{0}\"  ", binPath);
                builder.AppendFormat("displayName= \"{0}\"  ", displayName);

                if (!string.IsNullOrEmpty(userName))
                {
                    Console.WriteLine($"Username supplied: {userName}");
                    Console.WriteLine($"Ensure {userName} has logon as service rights.");

                    builder.AppendFormat("obj= \"{0}\"  ", userName);
                }

                if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(unecryptedPassword) && !unecryptedPassword.Equals(@"NT AUTHORITY\Local Service") && !unecryptedPassword.Equals(@"NT AUTHORITY\NetworkService"))
                {
                    Console.WriteLine($"Password supplied: {unecryptedPassword}");

                    builder.AppendFormat("password= \"{0}\"  ", unecryptedPassword);
                }

                builder.AppendFormat("start= \"{0}\"  ", startupTypeConverted);

                // Execute sc.exe commando
                using (Process process = new Process())
                {
                    process.StartInfo.FileName = @"sc.exe";
                    process.StartInfo.Arguments = builder.ToString();
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.StartInfo.UseShellExecute = false;

                    Console.WriteLine($"Calling {process.StartInfo.FileName} with arguments {process.StartInfo.Arguments}");
                    process.Start();
                    string stdoutx = process.StandardOutput.ReadToEnd();
                    string stderrx = process.StandardError.ReadToEnd();
                    process.WaitForExit();
                    if (process.ExitCode == 0) result = true;
                    if (result)
                    {
                        Console.WriteLine(stdoutx);
                        if (startnow)
                        {
                            var controller = new ServiceController(name);
                            controller.Start();
                            Console.WriteLine($"{strServiceName} installed and started");
                        }
                    }
                    else
                    {
                        Console.WriteLine(stderrx);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return result;
        }

        public bool Uninstall(string name)
        {
            StringBuilder builder = new StringBuilder();
            ServiceController control = ServiceController.GetServices().FirstOrDefault(s => s.ServiceName == name);
            if (control == null)
            {
                Console.WriteLine($"Service {name} doesn't exist");
                return false;
            }
            if (control.Status == ServiceControllerStatus.Running)
            {
                Console.WriteLine($"Service {name} stopping...");

                control.Stop();
            }
            bool isStopped = false;
            while (!isStopped)
            {
                ServiceController control2 = ServiceController.GetServices().FirstOrDefault(s => s.ServiceName == name);
                isStopped = control2.Status == ServiceControllerStatus.Stopped;
                Thread.Sleep(500);
            }
            Thread.Sleep(1000); // Allow Windows to register the service as stopped.

            builder.AppendFormat("{0} {1} ", "Delete", name);
            bool result = false;

            // Execute sc.exe commando
            using (Process process = new Process())
            {
                process.StartInfo.FileName = @"sc.exe";
                process.StartInfo.Arguments = builder.ToString();
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.Start();
                result = true;
                Console.WriteLine($"Service {name} uninstalled");
            }
            return result;
        }
    }

    internal enum ExitCode : int
    {
        Success = 0,
        InvalidLogin = 1,
        InvalidFilename = 2,
        UnknownError = 10
    }

    internal class Options
    {
        [Option('i', "install", Required = false, DefaultValue = false, HelpText = "Install Service.")]
        public bool Install { get; set; }

        [Option('u', "uninstall", Required = false, DefaultValue = false, HelpText = "Uninstall Service.")]
        public bool Uninstall { get; set; }

        [Option('s', "start", Required = false, DefaultValue = false, HelpText = "Start Service.")]
        public bool Start { get; set; }

        [Option('a', "account", Required = false, DefaultValue = "", HelpText = "Account. If blank, uses system account.")]
        public string Account { get; set; }

        [Option('p', "password", Required = false, DefaultValue = "", HelpText = "Account Password. Ignored if using system acct.")]
        public string Password { get; set; }

        [Option('n', "name", Required = false, DefaultValue = "Surpath.HL7Parser", HelpText = "Service Name")]
        public string ServiceName { get; set; }

        [Option('d', "display", Required = false, DefaultValue = "Surpath.HL7Parser Service", HelpText = "Service Friendly name / Description")]
        public string ServiceDisplayName { get; set; }

        [Option('g', "go", Required = false, DefaultValue = false, HelpText = "Run the service job manually.")]
        public bool Go { get; set; }

        [Option('t', "stage-test", Required = false, DefaultValue = false, HelpText = "Run HL7Stage test for Project Concert integration.")]
        public bool StageTest { get; set; }

        [Option("donor-id", Required = false, DefaultValue = "", HelpText = "Specific donor ID to test (for stage-test).")]
        public string DonorId { get; set; }

        [Option("no-reset", Required = false, DefaultValue = false, HelpText = "Don't set donor test status to 4 (for stage-test).")]
        public bool NoReset { get; set; }

        [Option("reset-after", Required = false, DefaultValue = false, HelpText = "Reset donor test status to 6 after generation (for stage-test).")]
        public bool ResetAfter { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            HelpText _ht = HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));

            return _ht;
        }
    }
}
