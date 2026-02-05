using Serilog;
using System.ComponentModel;
using System;
using System.Configuration;
using System.Globalization;

namespace SurPath.Data
{
    /// <summary>
    /// DataObject is base class for all the classes in SurPath.Data
    /// </summary>
    public abstract class DataObject
    {
        #region Private Variables

        private string _connectionString;
        private CultureInfo _culture;

        public bool MisMatchAssignToClient { get; set; } = false;
        public int MisMatchDefaultClientID { get; set; } = 0;
        public int MisMatchDefaultClientDepartmentID { get; set; } = 0;

        #endregion Private Variables

        #region Constructors

        /// <summary>
        /// DataObject is base class for all the classes in SurPath.Data
        /// </summary>
        public DataObject() //(ILogger __logger = null)
        {
            //if (__logger == null)
            //{
            //    _logger = new LoggerConfiguration().CreateLogger();
            //}
            //else
            //{
            //    _logger = __logger;
            //}

            this._connectionString = ConfigurationManager.AppSettings["ConnectionString"]?.ToString()?.Trim() ?? string.Empty;
            // adding a comment to see if line number changes
            // Use Culture from config if available, otherwise default to en-US
            string cultureSetting = ConfigurationManager.AppSettings["Culture"]?.ToString()?.Trim();
            this._culture = !string.IsNullOrEmpty(cultureSetting) ? new CultureInfo(cultureSetting) : new CultureInfo("en-US");

            if (ConfigKeyExists("MisMatchAssignToClient") && ConfigKeyExists("MisMatchDefaultClientID") && ConfigKeyExists("MisMatchDefaultClientDepartmentID"))
            {
                //< add key = "MisMatchAssignToClient" value = "true" />
                bool.TryParse(ConfigurationManager.AppSettings["MisMatchAssignToClient"].ToString().Trim(), out bool _MisMatchAssignToClient);
                MisMatchAssignToClient = _MisMatchAssignToClient;

                //< add key = "MisMatchDefaultClientID" value = "111" />
                int.TryParse(ConfigurationManager.AppSettings["MisMatchDefaultClientID"].ToString().Trim(), out int _MisMatchDefaultClientID);
                MisMatchDefaultClientID = _MisMatchDefaultClientID;

                //< add key = "MisMatchDefaultClientDepartmentID" value = "380" />
                int.TryParse(ConfigurationManager.AppSettings["MisMatchDefaultClientDepartmentID"].ToString().Trim(), out int _MisMatchDefaultClientDepartmentID);
                MisMatchDefaultClientDepartmentID = _MisMatchDefaultClientDepartmentID;
            }
        }

        public bool ConfigKeyExists(string _configkey)
        {
            return ConfigurationManager.AppSettings[_configkey] != null;
        }

        #endregion Constructors

        #region Public Properties

        /// <summary>
        /// Returns the Connection String for the desired database.
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return this._connectionString;
            }
        }

        /// <summary>
        /// Returns the UI Culture value.
        /// </summary>
        public CultureInfo Culture
        {
            get
            {
                return this._culture;
            }
        }

        #endregion Public Properties
    }

    public static class EnumExtensions
    {
        public static string ToDescriptionString(this SurPath.Enum.OverAllTestResult val)
        {
            System.ComponentModel.DescriptionAttribute[] attributes = (System.ComponentModel.DescriptionAttribute[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }

        public static string ToDescriptionString(this SurPath.Enum.TestCategories val)
        {
            System.ComponentModel.DescriptionAttribute[] attributes = (System.ComponentModel.DescriptionAttribute[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }

        public static string ToDescriptionString(this SurPath.Enum.DonorRegistrationStatus val)
        {
            System.ComponentModel.DescriptionAttribute[] attributes = (System.ComponentModel.DescriptionAttribute[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }
    }
}