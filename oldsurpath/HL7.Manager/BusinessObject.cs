using System.Configuration;

namespace HL7.Manager
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
            this._culture = ConfigurationManager.AppSettings["Culture"].ToString().Trim();
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
}