using System.Configuration;
using System.Globalization;

namespace SurPath.Business
{
    /// <summary>
    /// BusinessObject is base class for all the classes in SurPath.Business
    /// </summary>
    public abstract class BusinessObject
    {
        #region Private Variables

        private string _culture;

        #endregion Private Variables

        #region Constructors

        /// <summary>
        /// Create the new instance of BusinessObject class
        /// </summary>
        public BusinessObject()
        {
            string cultureSetting = ConfigurationManager.AppSettings["Culture"]?.ToString()?.Trim();
            this._culture = !string.IsNullOrEmpty(cultureSetting) ? cultureSetting : "en-US";
        }

        #endregion Constructors

        #region Public Properties

        /// <summary>
        /// Returns the UI Culture value.
        /// </summary>
        public string Culture
        {
            get
            {
                return this._culture;
            }
        }

        #endregion Public Properties
    }

    //public static class EnumExtensions
    //{
    //    public static string ToDescriptionString(this SurPath.Enum.TestCategories val)
    //    {
    //        System.ComponentModel.DescriptionAttribute[] attributes = (System.ComponentModel.DescriptionAttribute[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
    //        return attributes.Length > 0 ? attributes[0].Description : string.Empty;
    //    }
    //}
}