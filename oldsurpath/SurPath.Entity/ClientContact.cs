using System;

namespace SurPath.Entity
{
    public class ClientContact
    {
        #region Private Variables

        private int _clientContactId;
        private int _clientId;
        private int _clientDepartmentId;
        private string _clientContactFirstName;
        private string _clientContactLastName;
        private string _clientContactPhone;
        private string _clientContactFax;
        private string _clientContactEmail;
        private bool _isSynchronized;
        private bool _isArchived;
        private DateTime _createdOn;
        private string _createdBy;
        private DateTime _lastModifiedOn;
        private string _lastModifiedBy;

        #endregion Private Variables

        #region Public Properties

        /// <summary>
        /// Get or Set the Client Contact Id value.
        /// </summary>
        public int ClientContactId
        {
            get
            {
                return this._clientContactId;
            }
            set
            {
                this._clientContactId = value;
            }
        }

        /// <summary>
        /// Get or Set the Client Id value.
        /// </summary>
        public int ClientId
        {
            get
            {
                return this._clientId;
            }
            set
            {
                this._clientId = value;
            }
        }

        /// <summary>
        /// Get or Set the Client Department Id value.
        /// </summary>
        public int ClientDepartmentId
        {
            get
            {
                return this._clientDepartmentId;
            }
            set
            {
                this._clientDepartmentId = value;
            }
        }

        /// <summary>
        /// Get or Set the Client Contact First Name value.
        /// </summary>
        public string ClientContactFirstName
        {
            get
            {
                return this._clientContactFirstName;
            }
            set
            {
                this._clientContactFirstName = value;
            }
        }

        /// <summary>
        /// Get or Set the Client Contact Last Name value.
        /// </summary>
        public string ClientContactLastName
        {
            get
            {
                return this._clientContactLastName;
            }
            set
            {
                this._clientContactLastName = value;
            }
        }

        /// <summary>
        /// Get or Set the Client Contact Phone value.
        /// </summary>
        public string ClientContactPhone
        {
            get
            {
                return this._clientContactPhone;
            }
            set
            {
                this._clientContactPhone = value;
            }
        }

        /// <summary>
        /// Get or Set the Client Contact Fax value.
        /// </summary>
        public string ClientContactFax
        {
            get
            {
                return this._clientContactFax;
            }
            set
            {
                this._clientContactFax = value;
            }
        }

        /// <summary>
        /// Get or Set the Client Contact Email value.
        /// </summary>
        public string ClientContactEmail
        {
            get
            {
                return this._clientContactEmail;
            }
            set
            {
                this._clientContactEmail = value;
            }
        }

        /// <summary>
        /// Get or Set the data Syncronization status.
        /// </summary>
        public bool IsSynchronized
        {
            get
            {
                return this._isSynchronized;
            }
            set
            {
                this._isSynchronized = value;
            }
        }

        /// <summary>
        /// Get or Set the data archived status.
        /// </summary>
        public bool IsArchived
        {
            get
            {
                return this._isArchived;
            }
            set
            {
                this._isArchived = value;
            }
        }

        /// <summary>
        /// Get or Set the created date & time of the record.
        /// </summary>
        public DateTime CreatedOn
        {
            get
            {
                return this._createdOn;
            }
            set
            {
                this._createdOn = value;
            }
        }

        /// <summary>
        /// Get or Set the created user name of the record.
        /// </summary>
        public string CreatedBy
        {
            get
            {
                return this._createdBy;
            }
            set
            {
                this._createdBy = value;
            }
        }

        /// <summary>
        /// Get or Set the last modified date & time of the record.
        /// </summary>
        public DateTime LastModifiedOn
        {
            get
            {
                return this._lastModifiedOn;
            }
            set
            {
                this._lastModifiedOn = value;
            }
        }

        /// <summary>
        /// Get or Set the last modified user name of the record.
        /// </summary>
        public string LastModifiedBy
        {
            get
            {
                return this._lastModifiedBy;
            }
            set
            {
                this._lastModifiedBy = value;
            }
        }

        #endregion Public Properties
    }
}