using MySql.Data.MySqlClient;
using Serilog;
using SurPath.Entity;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;

namespace SurpathBackend
{
    /*
     *
     *
https://dev.mysql.com/doc/connector-net/en/connector-net-programming-blob-serverprep.html

You might need to modify the max_allowed_packet system variable. This variable determines how large of a packet
(that is, a single row) can be sent to the MySQL server. By default, the server only accepts a maximum size of 1MB
from the client application. If you intend to exceed 1MB in your file transfers, increase this number.

The max_allowed_packet option can be modified using the MySQL Workbench Server Administration screen. Adjust the
Maximum permitted option in the Data / Memory size section of the Networking tab to an appropriate setting. After
adjusting the value, click the Apply button and restart the server using the Startup / Shutdown screen of MySQL
Workbench. You can also adjust this value directly in the my.cnf file (add a line that reads max_allowed_packet=xxM),
or use the SET max_allowed_packet=xxM; syntax from within MySQL.

Try to be conservative when setting max_allowed_packet, as transfers of BLOB data can take some time to complete.
Try to set a value that will be adequate for your intended use and increase the value if necessary.

        to allow 16M files - SET GLOBAL max_allowed_packet=16777216;

        -- since this is storing documents in db, it's likely already high

     *
     */

    public class BackendFiles
    {
        private static ILogger _logger;
        public string ConnectionString { get; set; }
        public CultureInfo Culture { get; set; }
        public MySqlConnection conn { get; set; }
        public string AllFilesFolder { get; set; }

        public string EngineSettingsFileName { get; } = "PDFEngineSettingsGlobal.json";

        public BackendFiles(string _ConnectionString = null, ILogger __logger = null, bool syncFiles = true)
        {
            if (__logger != null)
            {
                _logger = __logger;

            }
            else
            {
                _logger = new LoggerConfiguration().CreateLogger();
                _logger.Debug("No logger passed, created");

            }
            _logger.Debug("BackendFiles logger online");
            _logger.Debug("Path to this lib: " + AssemblyDirectory);

            this.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString().Trim();

            if (!(ConfigurationManager.AppSettings["AllFilesFolder"] == null))
            {
                this.AllFilesFolder = ConfigurationManager.AppSettings["AllFilesFolder"].ToString().Trim();
            }
            this.Culture = new CultureInfo(ConfigurationManager.AppSettings["Culture"].ToString().Trim());

            if (!string.IsNullOrEmpty(_ConnectionString)) ConnectionString = _ConnectionString;

            this.conn = new MySqlConnection(this.ConnectionString);

            _logger.Debug("BackendFiles created, loading any new files just in case...");
            _logger.Debug("");
            if (syncFiles==true)
            {
                SyncFolderToDatabase(this.AllFilesFolder); // load any files added to AllFilesFolder
            }
            else
            {
                _logger.Debug($"File sync skipped");
            }
            
            _logger.Debug("");
            _logger.Debug("BackendFiles created, loading any new files finished");
        }

        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        #region Files

        public bool ResetGlobals()
        {
            string appSectionGroup = "PDFEngine/PDFEngineSettings";
            _logger.Debug($"Overwriting Globals from config file section {appSectionGroup}");
            if (System.Environment.UserInteractive) { Console.WriteLine($"Overwriting Globals from config file section {appSectionGroup}"); };

            string EngineSettingsJSON = string.Empty;

            string path = AppDomain.CurrentDomain.BaseDirectory;

            PDFEngineSettings EngineSettings = new PDFEngineSettings();

            NameValueCollection _settings;

            _settings = ConfigurationManager.GetSection(appSectionGroup) as NameValueCollection;
            // copy to a list so it's not read only
            _logger.Debug("Loading settings from config file...");

            foreach (string key in _settings)
            {
                string _logInfo = $"adding {key} with value {_settings[key]}";
                _logger.Debug(_logInfo);
                if (System.Environment.UserInteractive) { Console.WriteLine(_logInfo); };

                if (EngineSettings.Settings[key] == null)
                {
                    EngineSettings.Settings.Add(key, _settings[key]);
                }
                else
                {
                    EngineSettings.Settings[key] = _settings[key];
                }
            }

            _logger.Debug($"Saving config file settings to database as {EngineSettingsFileName}");
            if (System.Environment.UserInteractive) { Console.WriteLine($"Saving config file settings to database as {EngineSettingsFileName}"); };

            string _settingsJSON = EngineSettings.Json();
            if (System.Environment.UserInteractive) { Console.WriteLine($"_settingsJSON:"); };
            if (System.Environment.UserInteractive) { Console.WriteLine($" {_settingsJSON}"); };

            SaveTextFile(EngineSettingsFileName, _settingsJSON);

            return true;
        }

        public void ShowPDFEngineGlobals()
        {
            if (!System.Environment.UserInteractive) return;
            string globalsJson = ReadTextFile(EngineSettingsFileName);
            Console.Write(globalsJson);
            return;
        }

        public MemoryStream databaseFileRead(string file_name)
        {
            MemoryStream memoryStream = new MemoryStream();
            try
            {
                if (this.conn.State == ConnectionState.Closed) conn.Open();

                string sql = "select file_content from backend_files where file_name = @file_name;";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.Add(new MySqlParameter("@file_name", file_name));
                    using (var sqlQueryResult = cmd.ExecuteReader())
                        if (sqlQueryResult != null && sqlQueryResult.HasRows)
                        {
                            sqlQueryResult.Read();
                            var blob = new Byte[(sqlQueryResult.GetBytes(0, 0, null, 0, int.MaxValue))];
                            sqlQueryResult.GetBytes(0, 0, blob, 0, blob.Length);
                            memoryStream.Write(blob, 0, blob.Length);
                            memoryStream.Position = 0;
                            _logger.Debug($"Successfully read {file_name}");
                        }
                };
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to read {file_name}");
                _logger.Error(ex.Message);
                _logger.Error(ex.InnerException.ToString());
            }

            return memoryStream;
        }

        public int databaseFileExists(string file_name)
        {
            int result = 0;
            try
            {
                file_name = Path.GetFileName(file_name);
                if (this.conn.State == ConnectionState.Closed) conn.Open();

                string sql = "select file_id from backend_files where file_name = @file_name;";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.Add(new MySqlParameter("@file_name", file_name));
                    using (MySqlDataReader sqlQueryResult = cmd.ExecuteReader())
                    {
                        if (sqlQueryResult.HasRows)
                        {
                            sqlQueryResult.Read();
                            result = (int)sqlQueryResult.GetInt32(sqlQueryResult.GetOrdinal("file_id"));
                            _logger.Debug($"Successfully checked if {file_name} exists. RESULT: {result}");
                        }
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to check if {file_name} exists");
                _logger.Error(ex.Message);
                _logger.Error(ex.InnerException.ToString());
            }
            return result;
        }

        public int databaseFilePut(MemoryStream fileToPut, string file_name, string file_content_type = "")
        {
            int file_id = 0;
            try
            {
                if (string.IsNullOrEmpty(file_content_type))
                {
                    file_content_type = MimeMapping.GetMimeMapping(file_name);
                    //file_content_type = MimeMappingStealer.GetMimeMapping(file_name);
                }
                _logger.Debug("databaseFilePut");
                _logger.Debug($"file_name {file_name}");
                _logger.Debug($"file_content_type {file_content_type}");


                byte[] file_content = fileToPut.ToArray();
                file_name = file_name.ToLower();
                file_content_type = file_content_type.ToLower();
                int file_size = file_content.Length;
                file_id = databaseFileExists(file_name);
                _logger.Debug($"datatbaseFilePut: {file_name} exists, id: {file_id}");
                string sql = "insert into backend_files (file_content, file_content_type, file_name, file_size) values (@file_content, @file_content_type, @file_name, @file_size); select last_insert_id();";

                if (file_id > 0)
                {
                    _logger.Debug($"datatbaseFilePut: updating file id {file_id}");

                    sql = "update backend_files set file_content = @file_content, file_content_type = @file_content_type, file_name = @file_name, file_size = @file_size where file_id = @file_id; select @file_id;";
                }
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.Add(new MySqlParameter("@file_name", file_name));
                    cmd.Parameters.Add(new MySqlParameter("@file_content_type", file_content_type));
                    cmd.Parameters.Add(new MySqlParameter("@file_content", file_content));
                    cmd.Parameters.Add(new MySqlParameter("@file_size", file_size));

                    if (file_id > 0)
                    {
                        cmd.Parameters.Add(new MySqlParameter("@file_id", file_id));
                    }

                    file_id = Convert.ToInt32(cmd.ExecuteScalar());
                    _logger.Debug($"Successfully saved {file_name} exists. RESULT ID: {file_id}");
                };
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to save {file_name}");
                _logger.Error(ex.Message);
                if (ex.InnerException!=null)
                {
                    _logger.Error(ex.InnerException.ToString());
                }
            }

            return file_id;
        }

        public int SaveTextFile(string file_name, string file_content)
        {
            try
            {
                byte[] byteArray = Encoding.ASCII.GetBytes(file_content);
                MemoryStream stream = new MemoryStream(byteArray);

                int id = databaseFilePut(stream, file_name);
                stream.Close();
                return id;
            }
            catch (Exception ex)
            {
                _logger.Error($"Unable to save text file {file_name}");
                _logger.Error(ex.Message);
                _logger.Error(ex.InnerException.ToString());
                return 0;
            }
        }

        public string ReadTextFile(string file_name)
        {
            try
            {
                MemoryStream streamOut = databaseFileRead(file_name);
                // convert stream to string
                StreamReader reader = new StreamReader(streamOut);
                reader.DiscardBufferedData();        // reader now reading from position 0

                string file_contentOut = reader.ReadToEnd();
                streamOut.Close();
                reader.Close();
                return file_contentOut;
            }
            catch (Exception ex)
            {
                _logger.Error($"Unable to save text file {file_name}");
                _logger.Error(ex.Message);
                _logger.Error(ex.InnerException.ToString());
                return string.Empty;
            }
        }

        public void SyncFolderToDatabase(string folder_name = "", bool overwrite = false)
        {
            if (string.IsNullOrEmpty(folder_name)) folder_name = this.AllFilesFolder;
            if (string.IsNullOrEmpty(folder_name)) return;
            _logger.Debug($"Syncing files from {folder_name}");
            foreach (string file_name in Directory.EnumerateFiles(folder_name, "*.*"))
            {
                try
                {
                    using (MemoryStream ms = new MemoryStream())
                    using (FileStream file = new FileStream(file_name, FileMode.Open, FileAccess.Read))
                    {
                        string _file_name = Path.GetFileName(file_name);
                        if (_file_name.Equals(EngineSettingsFileName, StringComparison.InvariantCultureIgnoreCase))
                        {
                            _logger.Debug($"{EngineSettingsFileName} can only be set with -r, to reload from config file. skipping...");
                            continue;
                        }
                        byte[] bytes = new byte[file.Length];
                        file.Read(bytes, 0, (int)file.Length);
                        ms.Write(bytes, 0, (int)file.Length);

                        if (databaseFileExists(_file_name) > 0 && overwrite == false)
                        {
                            _logger.Debug($"{_file_name} already exist and overwrite not true");
                        }

                        if (databaseFileExists(_file_name) > 0 && overwrite == true)
                        {
                            _logger.Debug($"Overwriting {_file_name} with folder version.");

                            databaseFilePut(ms, _file_name);
                        }
                        else if (databaseFileExists(_file_name) == 0)
                        {
                            _logger.Debug($"{_file_name} is new, adding...");

                            databaseFilePut(ms, _file_name);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error($"Unable to sync file: {file_name}");
                    _logger.Error(ex.Message);
                    if (!(ex.InnerException == null)) _logger.Error(ex.InnerException.ToString());
                }
            }
        }

        public void SyncFileToDatabase(string file_name, string folder_name = "", bool overwrite = false)
        {
            if (string.IsNullOrEmpty(folder_name)) folder_name = this.AllFilesFolder;
            if (string.IsNullOrEmpty(folder_name)) return;
            if (file_name.Equals(EngineSettingsFileName, StringComparison.InvariantCultureIgnoreCase))
            {
                _logger.Debug($"{EngineSettingsFileName} can only be set with -r, to reload from config file. skipping...");
                return;
            }
            folder_name = folder_name.TrimEnd('\\') + '\\';
            file_name = folder_name + file_name;
            if (!File.Exists(file_name)) return;

            _logger.Debug($"Syncing file {file_name} from {folder_name}");
            using (MemoryStream ms = new MemoryStream())
            using (FileStream file = new FileStream(file_name, FileMode.Open, FileAccess.Read))
            {
                string _file_name = Path.GetFileName(file_name);

                {
                    byte[] bytes = new byte[file.Length];
                    file.Read(bytes, 0, (int)file.Length);
                    ms.Write(bytes, 0, (int)file.Length);

                    if (databaseFileExists(_file_name) > 0 && overwrite == false)
                    {
                        _logger.Debug($"{_file_name} already exist and overwrite not true");
                    }

                    if (databaseFileExists(_file_name) > 0 && overwrite == true)
                    {
                        _logger.Debug($"Overwriting {_file_name} with folder version.");

                        databaseFilePut(ms, _file_name);
                    }
                    else if (databaseFileExists(_file_name) == 0)
                    {
                        _logger.Debug($"{_file_name} is new, adding...");

                        databaseFilePut(ms, _file_name);
                    }
                }

            }
        }

        public void SyncFolderFromDatabase(string folder_name = "")
        {
            if (string.IsNullOrEmpty(folder_name)) folder_name = this.AllFilesFolder;
            if (string.IsNullOrEmpty(folder_name)) return;
            _logger.Debug($"Syncing folder {folder_name} database");
            folder_name = folder_name.TrimEnd('\\') + '\\';
            try
            {
                if (this.conn.State == ConnectionState.Closed) conn.Open();

                string sql = "select * from backend_files;";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    using (var sqlQueryResult = cmd.ExecuteReader())
                        if (sqlQueryResult != null)
                        {
                            while (sqlQueryResult.Read())
                            {
                                int file_id = sqlQueryResult.GetInt32(sqlQueryResult.GetOrdinal("file_id"));
                                string file_update_time = sqlQueryResult.GetDateTime(sqlQueryResult.GetOrdinal("file_update_time")).ToString();
                                int file_size = sqlQueryResult.GetInt32(sqlQueryResult.GetOrdinal("file_size"));
                                string file_content_type = sqlQueryResult.GetString(sqlQueryResult.GetOrdinal("file_content_type")).ToString();
                                string file_name = sqlQueryResult.GetString(sqlQueryResult.GetOrdinal("file_name")).ToString();
                                int ordinal = sqlQueryResult.GetOrdinal("file_content");
                                var blob = new Byte[(sqlQueryResult.GetBytes(ordinal, 0, null, 0, int.MaxValue))];
                                sqlQueryResult.GetBytes(ordinal, 0, blob, 0, blob.Length);
                                using (MemoryStream ms = new MemoryStream())
                                using (FileStream file = new FileStream(folder_name + file_name, FileMode.Create, System.IO.FileAccess.Write))
                                {
                                    ms.Write(blob, 0, blob.Length);
                                    ms.Position = 0;
                                    byte[] bytes = new byte[ms.Length];
                                    ms.Read(bytes, 0, (int)ms.Length);
                                    file.Write(bytes, 0, bytes.Length);
                                    ms.Close();
                                }
                            }
                        }
                };
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed sync {folder_name} from database");
                _logger.Error(ex.Message);
                _logger.Error(ex.InnerException.ToString());
            }
        }

        #endregion Files

        /// <summary>
        /// Exposes the Mime Mapping method that Microsoft hid from us.
        /// </summary>
        //public static class MimeMappingStealer
        //{
        //    // The get mime mapping method info
        //    private static readonly MethodInfo _getMimeMappingMethod = null;

        //    /// <summary>
        //    /// Static constructor sets up reflection.
        //    /// </summary>
        //    static MimeMappingStealer()
        //    {
        //        // Load hidden mime mapping class and method from System.Web
        //        var assembly = Assembly.GetAssembly(typeof(HttpApplication));
        //        Type mimeMappingType = assembly.GetType("System.Web.MimeMapping");
        //        _getMimeMappingMethod = mimeMappingType.GetMethod("GetMimeMapping",
        //            BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public |
        //            BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
        //    }

        //    /// <summary>
        //    /// Exposes the hidden Mime mapping method.
        //    /// </summary>
        //    /// <param name="fileName">The file name.</param>
        //    /// <returns>The mime mapping.</returns>
        //    public static string GetMimeMapping(string fileName)
        //    {
        //        return (string)_getMimeMappingMethod.Invoke(null /*static method*/, new[] { fileName });
        //    }
        //}
    }
}