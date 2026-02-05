using Serilog;
using SurPath.Data;
using SurPath.Enum;
using SurpathBackend;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Tamir.SharpSsh.jsch;

namespace HL7.Parser
{
    internal class Program
    {
        public static bool DoFTP = false;

        public static ILogger _logger;
        public static BackendParserReportHelper ReportHelper;
        public static Process proc;
        public static string JsonParserMasks;
        public static bool DryRun = false;


        public static long peakPagedMem = 0,
            peakWorkingSet = 0,
            peakVirtualMem = 0;

        private static void Main(string[] args)
        {
            try
            {
                DateTime start = DateTime.Now;
                proc = Process.GetCurrentProcess();

                _logger = new LoggerConfiguration().ReadFrom.AppSettings().CreateLogger();
                _logger.Information("Logger Loaded");
                HL7.Manager.HL7Parser._logger = _logger;
                _logger.Debug("Spinning up BackendParserReportHelper for PROGRAM instance");
                ReportHelper = new BackendParserReportHelper(_logger);
                HL7.Manager.HL7Parser.ReportHelper = ReportHelper;

                Boolean.TryParse(ConfigurationManager.AppSettings["DoFTP"].ToString().Trim().ToUpper(), out bool _DoDownload);

                if (_DoDownload == true)
                {
                    DoFTP = true;
                }
                
                if (ConfigKeyExists("DryRun")) bool.TryParse(ConfigurationManager.AppSettings["DryRun"].ToString(), out DryRun);
                if (DryRun == true)
                {
                    _logger.Debug("This is a dry run, no downloads, no moving of files");
                    DoFTP = false;
                }
                // TODO - need to refactor the BackendFiles into it's own project so everything can reference
                // DONE - Get parsing rexex's loaded
                // update our parsing regexes
                if (ConfigKeyExists("AllFilesFolder") && ConfigKeyExists("JsonParserMasks"))
                {
                    //BackendFiles backendfiles = new BackendFiles(null, _logger);
                    //backendfiles.SyncFileToDatabase(ConfigurationManager.AppSettings["JsonParserMasks"].ToString(), null, true);
                    //// now load from the db and set to JsonParserMasks
                    //JsonParserMasks = backendfiles.ReadTextFile(ConfigurationManager.AppSettings["JsonParserMasks"].ToString());
                    //HL7.Manager.HL7Parser.JsonParserMasks = JsonParserMasks;

                }

                _logger.Information("Syncing SSN for legacy support");
                HL7.Manager.HL7Parser.SyncSSNsLegacy();

                //Labs

                ProcessQuestLabFiles();
                LogMemoryUsage();
                ProcessCRLLabFiles();
                LogMemoryUsage();
                //MROs
                ProcessMROFiles();
                LogMemoryUsage();

                _logger.Information("Completed Entire Process");
                _logger.Information("Sending Final Log Email");
                DateTime finish = DateTime.Now;
                TimeSpan elapsed = finish.Subtract(start);
                List<string> memStats = new List<string>()
                {
                    $"  Peak physical memory usage : {peakWorkingSet}",
                    $"  Peak paged memory usage    : {peakPagedMem}",
                    $"  Peak virtual memory usage  : {peakVirtualMem}",
                    $"  Start: {start} - Finish {finish} - {((int)elapsed.TotalSeconds).ToString()} seconds to run"

                };



                HL7.Manager.HL7Parser.SendFinalProcessedEmail(memStats);

                ShowMemStats();
                ExitProgramCheck();
                Environment.ExitCode = (int)ExitCode.Success;


            }
            catch (Exception ex)
            {
                _logger.Information("There was an error processing files:");
                _logger.Information("");
                _logger.Information(ex.Message + ex.StackTrace);
                _logger.Information("");

                ExitProgramCheck();
                Environment.ExitCode = (int)ExitCode.UnknownError;
            }
        }
        public static bool ConfigKeyExists(string _configkey)
        {
            return ConfigurationManager.AppSettings[_configkey] != null;
        }
        public static void LogMemoryUsage()
        {
            _logger.Information($"{proc} -");
            _logger.Information("-------------------------------------");

            _logger.Information($"  Physical memory usage     : {proc.WorkingSet64}");
            _logger.Information($"  Base priority             : {proc.BasePriority}");
            _logger.Information($"  Priority class            : {proc.PriorityClass}");
            _logger.Information($"  User processor time       : {proc.UserProcessorTime}");
            _logger.Information($"  Privileged processor time : {proc.PrivilegedProcessorTime}");
            _logger.Information($"  Total processor time      : {proc.TotalProcessorTime}");
            _logger.Information($"  Paged system memory size  : {proc.PagedSystemMemorySize64}");
            _logger.Information($"  Paged memory size         : {proc.PagedMemorySize64}");

            // Update the values for the overall peak memory statistics.
            peakPagedMem = proc.PeakPagedMemorySize64;
            peakVirtualMem = proc.PeakVirtualMemorySize64;
            peakWorkingSet = proc.PeakWorkingSet64;
        }

        public static void ShowMemStats()
        {
            // Display peak memory statistics for the process.
            _logger.Information($"  Peak physical memory usage : {peakWorkingSet}");
            _logger.Information($"  Peak paged memory usage    : {peakPagedMem}");
            _logger.Information($"  Peak virtual memory usage  : {peakVirtualMem}");
        }

        private static bool ProcessCRLLabFiles()
        {
            bool complete = false;
            try
            {
                _logger.Information("Processing CRL Lab Reports");

                List<string> fileList = new List<string>();

                string ftpHost = ConfigurationManager.AppSettings["LabFTPHost"].ToString().Trim();
                string ftpUser = ConfigurationManager.AppSettings["LabFTPUsername"].ToString().Trim();
                string ftpPassword = ConfigurationManager.AppSettings["LabFTPPassword"].ToString().Trim();
                int ftpPort = Convert.ToInt32(ConfigurationManager.AppSettings["LabFTPPort"].ToString().Trim());

                string sourcePath = ConfigurationManager.AppSettings["LabReportFTPPath"].ToString().Trim();
                string destPath = ConfigurationManager.AppSettings["LabReportFilePath"].ToString().Trim();

                DownloadFiles(sourcePath, destPath, ref fileList, ftpHost, ftpUser, ftpPassword, ftpPort);
                _logger.Information($"Processing {fileList.Count.ToString()} CRL Lab files");

                if (fileList.Count > 0)
                {
                    _logger.Information($"{fileList.Count.ToString()} LAB Report file(s) to be processed");


                    HL7.Manager.HL7Parser.DoHL7Parser(ReportType.LabReport, ConfigurationManager.AppSettings["LabReportFilePath"].ToString().Trim());
                    _logger.Information("Lab Report Parsing Completed");

                    DeleteFiles(sourcePath, ref fileList, ftpHost, ftpUser, ftpPassword, ftpPort);
                    _logger.Information("Deleted the lab report files from FTP");

                    UploadFiles(ConfigurationManager.AppSettings["MROReportFileOutboundPath"].ToString().Trim(), ConfigurationManager.AppSettings["MROReportFTPOutboundPath"].ToString().Trim(), ftpHost, ftpUser, ftpPassword, ftpPort);

                    _logger.Information("CRL Lab Reports Completed");
                }
                else
                {
                    _logger.Information("No CRL Lab Report files processed");
                }
            }
            catch (Exception ex)
            {
                complete = false;

                _logger.Error(ex.ToString());
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                throw;
            }
            return complete;
        }

        private static bool ProcessQuestLabFiles()
        {
            bool complete = false;
            try
            {
                _logger.Information("Processing Quest Lab Reports");

                List<string> fileList = new List<string>();

                string ftpHost = ConfigurationManager.AppSettings["QuestFTPHost"].ToString().Trim();
                string ftpUser = ConfigurationManager.AppSettings["QuestFTPUsername"].ToString().Trim();
                string ftpPassword = ConfigurationManager.AppSettings["QuestFTPPassword"].ToString().Trim();
                int ftpPort = Convert.ToInt32(ConfigurationManager.AppSettings["QuestFTPPort"].ToString().Trim());

                string sourcePath = ConfigurationManager.AppSettings["QuestReportFTPPath"].ToString().Trim();
                string destPath = ConfigurationManager.AppSettings["QuestReportFilePath"].ToString().Trim();
                
                DownloadFiles(sourcePath, destPath, ref fileList, ftpHost, ftpUser, ftpPassword, ftpPort);
                _logger.Information($"Processing {fileList.Count.ToString()} Quest files");


                if (fileList.Count > 0)
                {
                    _logger.Information(string.Format("{0} Quest Report file(s) downloaded", fileList.Count.ToString()));
                    _logger.Information($"{fileList.Count.ToString()} Quest Report file(s) to be processed");


                    //Check for Multiple Entries in files and then Seperate
                    HL7.Manager.HL7Parser.ParseMulitDonorQuest(ConfigurationManager.AppSettings["QuestReportFilePath"].ToString().Trim(), ref fileList);
                    ///-------------------------------------------------------------------
                    ///Handle Separated Files now
                    HL7.Manager.HL7Parser.DoHL7Parser(ReportType.QuestLabReport, ConfigurationManager.AppSettings["QuestReportFilePath"].ToString().Trim());
                    _logger.Information("Quest Lab Report Parsing Completed");

                    DeleteFiles(sourcePath, ref fileList, ftpHost, ftpUser, ftpPassword, ftpPort);
                    _logger.Information("Deleted the lab report files from FTP");

                    UploadFiles(ConfigurationManager.AppSettings["MROReportFileOutboundPath"].ToString().Trim(), ConfigurationManager.AppSettings["MROReportFTPOutboundPath"].ToString().Trim(), ftpHost, ftpUser, ftpPassword, ftpPort);

                    _logger.Information("Quest Lab Reports Completed");
                }
                else
                {
                    _logger.Information("No Quest Lab Report files processed");
                }
            }
            catch (Exception ex)
            {
                _logger.Information(ex.Message + ex.StackTrace);

                complete = false;
            }
            return complete;
        }

        private static bool ProcessMROFiles()
        {
            bool complete = false;

            _logger.Information("Processing MRO Reports");

            List<string> fileList = new List<string>();

            string ftpHost = ConfigurationManager.AppSettings["MROFTPHost"].ToString().Trim();
            string ftpUser = ConfigurationManager.AppSettings["MROFTPUsername"].ToString().Trim();
            string ftpPassword = ConfigurationManager.AppSettings["MROFTPPassword"].ToString().Trim();
            int ftpPort = Convert.ToInt32(ConfigurationManager.AppSettings["MROFTPPort"].ToString().Trim());

            string sourcePath = ConfigurationManager.AppSettings["MROReportFTPInboundPath"].ToString().Trim();
            string destPath = ConfigurationManager.AppSettings["MROReportFileInboundPath"].ToString().Trim();

            DownloadFiles(sourcePath, destPath, ref fileList, ftpHost, ftpUser, ftpPassword, ftpPort);
            if (fileList.Count > 0)
            {
                _logger.Information($"Processing {fileList.Count.ToString()} MRO files in {destPath} folder");
                ////Check for Multiple Entries in files and then Seperate
                HL7.Manager.HL7Parser.ParseMultiMRO(ConfigurationManager.AppSettings["MROReportFileInboundPath"].ToString().Trim(), ref fileList);

                HL7.Manager.HL7Parser.DoHL7Parser(ReportType.MROReport, ConfigurationManager.AppSettings["MROReportFileInboundPath"].ToString().Trim());
                _logger.Information("MRO Report Parsing Completed");

                DeleteFiles(sourcePath, ref fileList, ftpHost, ftpUser, ftpPassword, ftpPort);

                _logger.Information("Deleted the MRO report files from FTP");

                _logger.Information("MRO Report Completed");
            }
            else
            {
                _logger.Information("No MRO Report files processed");
            }

            return complete;
        }

        private static void DownloadFiles(string sourcePath, string destPath, ref List<string> fileList, string ftpHost, string ftpUser, string ftpPassword, int ftpPort)
        {
            _logger.Debug("Download Files Called");
            _logger.Debug($"sourcePath {sourcePath}");
            _logger.Debug($"destPath {destPath}");
            _logger.Debug($"ftpHost {ftpHost}");
            _logger.Debug($"ftpPort {ftpPort}");
            _logger.Debug($"ftpPassword ********");

            string _SupportedFileExtensions = ".rpt, .pdf, .hl7, .pdf";
            if (ConfigurationManager.AppSettings["SupportedFileExtensions"] != null) _SupportedFileExtensions = ConfigurationManager.AppSettings["SupportedFileExtensions"].ToString();
            List<string> _fileExtensions = _SupportedFileExtensions.Split(',').Select(t => t.ToUpper().Trim()).ToList();
            try
            {
        

                List<string> dlfileList = new List<string>();
                if (DoFTP)
                {
                    JSch jsch = new JSch();

                    String host = ftpHost;
                    String user = ftpUser;
                    int port = ftpPort;
                    Session session = jsch.getSession(user, host, port);

                    UserInfo ui = new MyUserInfo(ftpPassword);
                    session.setUserInfo(ui);

                    session.connect();

                    Channel channel = session.openChannel("sftp");
                    channel.connect();
                    ChannelSftp c = (ChannelSftp)channel;

                    c.cd(sourcePath);
                    c.lcd(destPath);

                    String pwd = c.pwd();
                    String lpwd = c.lpwd();

                    ArrayList vv = c.ls(pwd);
                    _logger.Debug($"ArrayList -> {vv.Count} files");
                    if (vv != null)
                    {
                        for (int ii = 0; ii < vv.Count; ii++)
                        {
                            object obj = vv[ii];
                            if (obj is ChannelSftp.LsEntry)
                            {
                                Tamir.SharpSsh.jsch.ChannelSftp.LsEntry file = (Tamir.SharpSsh.jsch.ChannelSftp.LsEntry)obj;
                                //if (file.getFilename().ToString().ToUpper().EndsWith(".RPT")
                                //    || file.getFilename().ToString().ToUpper().EndsWith(".HL7")
                                //    || file.getFilename().ToString().ToUpper().EndsWith(".RES")
                                //    || file.getFilename().ToString().ToUpper().EndsWith(".PDF"))
                                //{
                                //    dlfileList.Add(file.getFilename());
                                //}
                                if (_fileExtensions.Any(file.getFilename().ToString().ToUpper().EndsWith))
                                {
                                    dlfileList.Add(file.getFilename());

                                }
                            }
                        }
                    }

                    _logger.Information($"{dlfileList.Count.ToString()} found to download");
                    _logger.Information($"Downloading {dlfileList.Count.ToString()} files");
                    int count = 0;
                    foreach (string fileName in dlfileList)
                    {
                        try
                        {
                            count++;
                            c.get(pwd + "/" + fileName, lpwd);
                            _logger.Information(count.ToString() + " of " + dlfileList.Count.ToString() + " Reading from :" + pwd + "/" + fileName);
                            _logger.Information(">>>>>>>>" + destPath.ToString() + "/" + fileName.ToString());
                            _logger.Information("_________________________________________________________________________________");
                        }
                        catch (Exception ex)
                        {
                            _logger.Error(ex.Message.ToString());
                            _logger.Error(ex.InnerException.ToString());
                        }
                    }

                    c.quit();
                    session.disconnect();
                    _logger.Information($"{dlfileList.Count.ToString()} files downloaded.");

                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                throw;
            }
            // Get all downloaded files and existing files into the files to process list.

            fileList = Directory.EnumerateFiles(destPath).Where(f=> _fileExtensions.Any(Path.GetExtension(f).ToUpper().EndsWith)).ToList();


            // TODO - Filter out unsupported filetypes, like desktop.ini by using SupportFileExtensions


        }

        private static void DeleteFiles(string sourcePath, ref List<string> fileList, string ftpHost, string ftpUser, string ftpPassword, int ftpPort)
        {
            if (DoFTP)
            {
                List<string> dlfileList = new List<string>();

                _logger.Information($"Deleting {fileList.Count.ToString()} files off of {sourcePath}");

                JSch jsch = new JSch();

                String host = ftpHost;
                String user = ftpUser;
                int port = ftpPort;
                Session session = jsch.getSession(user, host, port);

                UserInfo ui = new MyUserInfo(ftpPassword);
                session.setUserInfo(ui);

                session.connect();

                Channel channel = session.openChannel("sftp");
                channel.connect();
                ChannelSftp c = (ChannelSftp)channel;

                c.cd(sourcePath);

                String pwd = c.pwd();
                String lpwd = c.lpwd();

                ArrayList vv = c.ls(pwd);
                if (vv != null)
                {
                    for (int ii = 0; ii < vv.Count; ii++)
                    {
                        object obj = vv[ii];
                        if (obj is ChannelSftp.LsEntry)
                        {
                            Tamir.SharpSsh.jsch.ChannelSftp.LsEntry file = (Tamir.SharpSsh.jsch.ChannelSftp.LsEntry)obj;
                            if (file.getFilename().ToString().ToUpper().EndsWith(".RPT")
                                || file.getFilename().ToString().ToUpper().EndsWith(".HL7")
                                || file.getFilename().ToString().ToUpper().EndsWith(".RES")
                                || file.getFilename().ToString().ToUpper().EndsWith(".PDF"))
                            {
                                dlfileList.Add(file.getFilename());
                            }
                        }
                    }
                }

                // Only delete files we successfully downloaded!
                // filelist is a list of files in the folder + what was downloaded.
                List<string> filesToDelete = dlfileList.Intersect(fileList.Select(x => Path.GetFileName(x)).ToList()).ToList();

                foreach (string fileName in filesToDelete)
                {
                    c.rm(pwd + "/" + fileName);
                    _logger.Information($"Deleted {fileName}");
                }

                c.quit();
                session.disconnect();
            }
        }

        private static void UploadFiles(string sourcePath, string destPath, string ftpHost, string ftpUser, string ftpPassword, int ftpPort)
        {
            if (DoFTP)
            {
                JSch jsch = new JSch();

                String host = ftpHost;
                String user = ftpUser;
                int port = ftpPort;
                Session session = jsch.getSession(user, host, port);

                UserInfo ui = new MyUserInfo(ftpPassword);
                session.setUserInfo(ui);

                session.connect();

                Channel channel = session.openChannel("sftp");
                channel.connect();
                ChannelSftp c = (ChannelSftp)channel;

                c.lcd(sourcePath);
                c.cd(destPath);

                String pwd = c.pwd();
                String lpwd = c.lpwd();

                foreach (string reportFile in Directory.EnumerateFiles(sourcePath))
                {
                    c.put(reportFile, pwd);
                    _logger.Information(reportFile.Substring(reportFile.LastIndexOf('\\') + 1));
                }

                c.quit();
                session.disconnect();

                foreach (string reportFile in Directory.EnumerateFiles(sourcePath))
                {
                    File.Delete(reportFile);
                }
            }
        }

        private static void ExitProgramCheck()
        {
            if (ConfigurationManager.AppSettings["IsManualInteraction"].ToString().Trim().ToUpper() == "TRUE")
            {
                _logger.Information("Press enter to exit");
                Console.Read();
            }
        }

        public class MyUserInfo : UserInfo
        {
            public MyUserInfo(string password)
            {
                passwd = password;
            }

            public String getPassword()
            {
                return passwd;
            }

            public bool promptYesNo(String str)
            {
                return true;
            }

            private String passwd;

            public String getPassphrase()
            {
                return null;
            }

            public bool promptPassphrase(String message)
            {
                return true;
            }

            public bool promptPassword(String message)
            {
                if (passwd != string.Empty)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public void showMessage(string message)
            {
                throw new System.NotImplementedException();
            }
        }

        public class MyProgressMonitor : SftpProgressMonitor
        {
            private ConsoleProgressBar bar;
            private long c = 0;
            private long max = 0;
            private long percent = -1;
            private int elapsed = -1;

            private System.Timers.Timer timer;

            public override void init(int op, String src, String dest, long max)
            {
                bar = new ConsoleProgressBar();
                this.max = max;
                elapsed = 0;
                timer = new System.Timers.Timer(1000);
                timer.Start();
                timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            }

            public override bool count(long c)
            {
                this.c += c;
                if (percent >= this.c * 100 / max) { return true; }
                percent = this.c * 100 / max;

                string note = ("Transfering... [Elapsed time: " + elapsed + "]");

                bar.Update((int)this.c, (int)max, note);
                return true;
            }

            public override void end()
            {
                timer.Stop();
                timer.Dispose();
                string note = ("Done in " + elapsed + " seconds!");
                bar.Update((int)this.c, (int)max, note);
                bar = null;
            }

            private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
            {
                this.elapsed++;
            }
        }

        private static String help =
            "      Available commands:\n" +
            "      * means unimplemented command.\n" +
            "cd path                       Change remote directory to 'path'\n" +
            "lcd path                      Change local directory to 'path'\n" +
            "chgrp grp path                Change group of file 'path' to 'grp'\n" +
            "chmod mode path               Change permissions of file 'path' to 'mode'\n" +
            "chown own path                Change owner of file 'path' to 'own'\n" +
            "help                          Display this help text\n" +
            "get remote-path [local-path]  Download file\n" +
            "get-resume remote-path [local-path]  Resume to download file.\n" +
            "get-append remote-path [local-path]  Append remote file to local file\n" +
            "*lls [ls-options [path]]      Display local directory listing\n" +
            "ln oldpath newpath            Symlink remote file\n" +
            "*lmkdir path                  Create local directory\n" +
            "lpwd                          Print local working directory\n" +
            "ls [path]                     Display remote directory listing\n" +
            "*lumask umask                 Set local umask to 'umask'\n" +
            "mkdir path                    Create remote directory\n" +
            "put local-path [remote-path]  Upload file\n" +
            "put-resume local-path [remote-path]  Resume to upload file\n" +
            "put-append local-path [remote-path]  Append local file to remote file.\n" +
            "pwd                           Display remote working directory\n" +
            "stat path                     Display info about path\n" +
            "exit                          Quit sftp\n" +
            "quit                          Quit sftp\n" +
            "rename oldpath newpath        Rename remote file\n" +
            "rmdir path                    Remove remote directory\n" +
            "rm path                       Delete remote file\n" +
            "symlink oldpath newpath       Symlink remote file\n" +
            "rekey                         Key re-exchanging\n" +
            "compression level             Packet compression will be enabled\n" +
            "version                       Show SFTP version\n" +
            "?                             Synonym for help";
    }

    internal enum ExitCode : int
    {
        Success = 0,
        InvalidLogin = 1,
        InvalidFilename = 2,
        UnknownError = 10
    }
}