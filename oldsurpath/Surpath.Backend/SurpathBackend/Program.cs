using CommandLine;
using CommandLine.Text;
using Serilog;
using SurPath.Data.Backend;
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

namespace SurpathBackend
{
    internal static class Program
    {
        private static Options options;
        public static ServiceSettings serviceSettings;
        public static ILogger _logger;
        public static BackendFiles backendFiles;
        public static bool IsProduction;

        public class ServiceSettings
        {
            // Consider grabbing these from a config for settings.json file - or even as switches. [add to options with default values]
            public string ServiceName { get; set; } = "Surpath.Backend";

            public string ServiceDisplayName { get; set; } = "Surpath.Backend";
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
                var FormFoxPDFFolder = ConfigurationManager.AppSettings["FormFoxPDFFolder"].ToString().Trim();
                Directory.CreateDirectory(FormFoxPDFFolder);
                var FormFoxDumpFolder = ConfigurationManager.AppSettings["FormFoxDumpFolder"].ToString().Trim();
                Directory.CreateDirectory(FormFoxDumpFolder);
                serviceSettings = new ServiceSettings();
                _logger.Debug("Logger Loaded - Debug Level visible");
                if (System.Environment.UserInteractive)
                {
                    //Console.WriteLine("starting");

                    //Console.WriteLine("parsing");
                    _logger.Debug("Interactive mode");
                    //var _fff = new FFMPSearch();


                    //_logger.Debug($"Config File: {AppDomain.CurrentDomain.SetupInformation.ConfigurationFile}");

                    options = new Options();

                    Parser.Default.ParseArguments(args, options);
                    backendFiles = new BackendFiles(null, _logger);
                    RunOptionsAndReturnExitCode();
                }

                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                new SurpathService(_logger)
                };
                _logger.Debug("Running Service");
                ServiceBase.Run(ServicesToRun);
            }
            catch (Exception ex)
            {
                PanicEmail(ex);
                throw;
                //StringBuilder stringBuilder = new StringBuilder();

                //_logger.Error(ex.Message);
                //stringBuilder.AppendLine(ex.Message);
                //stringBuilder.AppendLine(String.Empty);
                //if (ex.InnerException != null)
                //{
                //    _logger.Error(ex.InnerException.ToString());
                //    stringBuilder.AppendLine(ex.InnerException.ToString());

                //}
                //if (ex.StackTrace != null)
                //{
                //    _logger.Error(ex.StackTrace.ToString());
                //    stringBuilder.AppendFormat(ex.StackTrace.ToString());

                //}

                //backendLogic.SendError(stringBuilder.ToString());

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
        private static void RunOptionsAndReturnExitCode()
        {
            bool _mgmt = false;

            if (options.LoadFolder == true)
            {
                Console.WriteLine($"Loading files from {backendFiles.AllFilesFolder}");
                if (options.ForceOverwrite) Console.WriteLine($"Overwriting files");
                backendFiles.SyncFolderToDatabase(null, options.ForceOverwrite);
                _mgmt = true;
            }
            if (options.OutputFolder == true)
            {
                Console.WriteLine($"Dumping files from {backendFiles.AllFilesFolder}");
                backendFiles.SyncFolderFromDatabase();
                _mgmt = true;
            }
            if (options.ResetGlobals == true)
            {
                Console.WriteLine($"Reloading PDFEngine Settings from Config {AppDomain.CurrentDomain.SetupInformation.ConfigurationFile}");
                backendFiles.ResetGlobals();
                _mgmt = true;
            }
            if (options.Go)
            {
                Go();
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
            backendFiles.ShowPDFEngineGlobals();
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
            BackendServiceWorker workerDebug = new BackendServiceWorker(_logger);
            workerDebug.Work();
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

    [Option('g', "go", Required = false, DefaultValue = false, HelpText = "Run the service job manually. Not Implemented")]
    public bool Go { get; set; }

    [Option('a', "account", Required = false, DefaultValue = "", HelpText = "Account. If blank, uses system account.")]
    public string Account { get; set; }

    [Option('p', "password", Required = false, DefaultValue = "", HelpText = "Account Password. Ignored if using system acct.")]
    public string Password { get; set; }

    [Option('n', "name", Required = false, DefaultValue = "MyService", HelpText = "Service Name")]
    public string ServiceName { get; set; }

    [Option('d', "display", Required = false, DefaultValue = "My Self Installing Service", HelpText = "Service Friendly name / Description")]
    public string ServiceDisplayName { get; set; }

    [Option('l', "loadfolder", Required = false, DefaultValue = false, HelpText = "Put all files in AllFilesFolder (config setting) into database")]
    public bool LoadFolder { get; set; }

    [Option('o', "outputFolder", Required = false, DefaultValue = false, HelpText = "Pull all files in database to AllFilesFolder (config setting)")]
    public bool OutputFolder { get; set; }

    [Option('f', "forceOverwrite", Required = false, DefaultValue = false, HelpText = "Overwrite the files in the database from AllFilesFolder (config setting)")]
    public bool ForceOverwrite { get; set; }

    [Option('r', "ResetGlobals", Required = false, DefaultValue = false, HelpText = "Reset PDFEngine Globals from config file (PDFEngineSettings section)")]
    public bool ResetGlobals { get; set; }

    [HelpOption]
    public string GetUsage()
    {
        HelpText _ht = HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));

        return _ht;
    }
}