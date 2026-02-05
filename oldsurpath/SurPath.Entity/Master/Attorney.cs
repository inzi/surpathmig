using System;

namespace SurPath.Entity.Master
{
    /// <summary>
    /// Court entity
    /// </summary>
    public class Attorney
    {
        #region Private Variables

        private int _attorneyId;
        private string _attorneyFirstName;
        private string _attorneyLastName;
        private string _attorneyAddress1;
        private string _attorneyAddress2;
        private string _attorneyCity;
        private string _attorneyState;
        private string _attorneyZip;
        private string _attorneyPhone;
        private string _attorneyFax;
        private string _attorneyEmail;
        private bool _isActive;
        private bool _isSynchronized;
        private bool _isArchived;
        private DateTime _createdOn;
        private string _createdBy;
        private DateTime _lastModifiedOn;
        private string _lastModifiedBy;

        #endregion Private Variables

        #region Public Properties

        /// <summary>
        /// Get or Set the Attorney Id value.
        /// </summary>
        public int AttorneyId
        {
            get
            {
                return this._attorneyId;
            }
            set
            {
                this._attorneyId = value;
            }
        }

        /// <summary>
        /// Get or Set the Attorney First Name value.
        /// </summary>
        public string AttorneyFirstName
        {
            get
            {
                return this._attorneyFirstName;
            }
            set
            {
                this._attorneyFirstName = value;
            }
        }

        /// <summary>
        /// Get or Set the Attorney Last Name value.
        /// </summary>
        public string AttorneyLastName
        {
            get
            {
                return this._attorneyLastName;
            }
            set
            {
                this._attorneyLastName = value;
            }
        }

        /// <summary>
        /// Get the User Display Name value.
        /// </summary>
        public string UserDisplayName
        {
            get
            {
                return this._attorneyFirstName + " " + this._attorneyLastName;
            }
        }

        /// <summary>

        /// <summary>
        /// Get or Set the Attorney Address 1 value.
        /// </summary>
        public string AttorneyAddress1
        {
            get
            {
                return this._attorneyAddress1;
            }
            set
            {
                this._attorneyAddress1 = value;
            }
        }

        /// <summary>
        /// Get or Set the Attorney Address 2 value.
        /// </summary>
        public string AttorneyAddress2
        {
            get
            {
                return this._attorneyAddress2;
            }
            set
            {
                this._attorneyAddress2 = value;
            }
        }

        /// <summary>
        /// Get or Set the Attorney City value.
        /// </summary>
        public string AttorneyCity
        {
            get
            {
                return this._attorneyCity;
            }
            set
            {
                this._attorneyCity = value;
            }
        }

        /// <summary>
        /// Get or Set the Attorney State value.
        /// </summary>
        public string AttorneyState
        {
            get
            {
                return this._attorneyState;
            }
            set
            {
                this._attorneyState = value;
            }
        }

        /// <summary>
        /// Get or Set the Attorney ZipCode value.
        /// </summary>
        public string AttorneyZip
        {
            get
            {
                return this._attorneyZip;
            }
            set
            {
                this._attorneyZip = value;
            }
        }

        /// <summary>
        /// Get or Set the Attorney Phone value.
        /// </summary>
        public string AttorneyPhone
        {
            get
            {
                return this._attorneyPhone;
            }
            set
            {
                this._attorneyPhone = value;
            }
        }

        /// <summary>
        /// Get or Set the Attorney Fax value.
        /// </summary>
        public string AttorneyFax
        {
            get
            {
                return this._attorneyFax;
            }
            set
            {
                this._attorneyFax = value;
            }
        }

        /// <summary>
        /// Get or Set the Attorney Email value.
        /// </summary>
        public string AttorneyEmail
        {
            get
            {
                return this._attorneyEmail;
            }
            set
            {
                this._attorneyEmail = value;
            }
        }

        /// <summary>
        /// Get or Set the Attorney active status.
        /// </summary>
        public bool IsActive
        {
            get
            {
                return this._isActive;
            }
            set
            {
                this._isActive = value;
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