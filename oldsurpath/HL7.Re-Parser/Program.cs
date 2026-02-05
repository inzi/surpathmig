using SurPath.Enum;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using Tamir.SharpSsh.jsch;

namespace HL7.ReParser
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Started");
                Console.WriteLine("Executing Lab mismatched Report");


                int fileCount = Directory.GetFiles(ConfigurationManager.AppSettings["LabMissmatchedReportsPath"].ToString().Trim()).Length;

                if (fileCount > 0)
                {
                    Console.WriteLine(string.Format("{0} LAB mismatched Report file(s) available", fileCount.ToString()));

                    HL7.Manager.HL7Parser.DoHL7Parser(ReportType.LabReport, ConfigurationManager.AppSettings["LabMissmatchedReportsPath"].ToString().Trim());
                    Console.WriteLine("Lab mismatched Report Parsing Completed");
                    
                    //DeleteFiles(sourcePath, ref fileList, ftpHost, ftpUser, ftpPassword, ftpPort);
                    //Console.WriteLine("Deleted the lab report files from FTP");

                    //UploadFiles(ConfigurationManager.AppSettings["MROReportFileOutboundPath"].ToString().Trim(), ConfigurationManager.AppSettings["MROReportFTPOutboundPath"].ToString().Trim(), ftpHost, ftpUser, ftpPassword, ftpPort);

                    
                }
                else
                {
                    Console.WriteLine("No Lab mismatched Report file available");
                }

                Console.WriteLine("Executing MRO mismatched Report");

                int mroFileCount = Directory.GetFiles(ConfigurationManager.AppSettings["MroMissmatchedReportsPath"].ToString().Trim()).Length;

                if (mroFileCount > 0)
                {
                    Console.WriteLine(string.Format("{0} MRO Report file(s) available", mroFileCount.ToString()));

                    HL7.Manager.HL7Parser.DoHL7Parser(ReportType.LabReport, ConfigurationManager.AppSettings["MroMissmatchedReportsPath"].ToString().Trim());
                    Console.WriteLine("MRO mismatched report Parsing Completed");

                    //DeleteFiles(sourcePath, ref fileList, ftpHost, ftpUser, ftpPassword, ftpPort);
                    //Console.WriteLine("Deleted the lab report files from FTP");

                    //UploadFiles(ConfigurationManager.AppSettings["MROReportFileOutboundPath"].ToString().Trim(), ConfigurationManager.AppSettings["MROReportFTPOutboundPath"].ToString().Trim(), ftpHost, ftpUser, ftpPassword, ftpPort);

                    
                }
                else
                {
                    Console.WriteLine("No MRO mismatched Report file available");
                }

                Console.WriteLine("Completed");

                if (ConfigurationManager.AppSettings["IsManualInteraction"].ToString().Trim().ToUpper() == "TRUE")
                {
                    Console.WriteLine("Press enter to exit");
                    Console.Read();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("End with the error");
                Console.WriteLine("");
                Console.WriteLine(ex.Message);
                Console.WriteLine("");


                if (ConfigurationManager.AppSettings["IsManualInteraction"].ToString().Trim().ToUpper() == "TRUE")
                {
                    Console.WriteLine("Press enter to exit");
                    Console.Read();
                }
            }
        }

        private static void DownloadFiles(string sourcePath, string destPath, ref List<string> fileList, string ftpHost, string ftpUser, string ftpPassword, int ftpPort)
        {
            fileList = new List<string>();

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
                            || file.getFilename().ToString().ToUpper().EndsWith(".PDF"))
                        {
                            fileList.Add(file.getFilename());
                        }
                    }
                }
            }

            foreach (string fileName in fileList)
            {
                c.get(pwd + "/" + fileName, lpwd);
                Console.WriteLine(fileName);
            }

            c.quit();
            session.disconnect();
        }

        private static void DeleteFiles(string sourcePath, ref List<string> fileList, string ftpHost, string ftpUser, string ftpPassword, int ftpPort)
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

            String pwd = c.pwd();

            foreach (string fileName in fileList)
            {
                c.rm(pwd + "/" + fileName);
                Console.WriteLine(fileName);
            }

            c.quit();
            session.disconnect();
        }

        private static void UploadFiles(string sourcePath, string destPath, string ftpHost, string ftpUser, string ftpPassword, int ftpPort)
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
                Console.WriteLine(reportFile.Substring(reportFile.LastIndexOf('\\') + 1));
            }

            c.quit();
            session.disconnect();

            foreach (string reportFile in Directory.EnumerateFiles(sourcePath))
            {
                File.Delete(reportFile);
            }
        }

        public class MyUserInfo : UserInfo
        {
            public MyUserInfo(string password)
            {
                passwd = password;
            }

            public String getPassword() { return passwd; }

            public bool promptYesNo(String str)
            {
                return true;
            }

            String passwd;

            public String getPassphrase() { return null; }

            public bool promptPassphrase(String message) { return true; }

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
            int elapsed = -1;

            System.Timers.Timer timer;

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
}
