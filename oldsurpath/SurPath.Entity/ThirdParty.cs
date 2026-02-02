using System;

namespace SurPath.Entity
{
    public class ThirdParty
    {
        #region Private Variables

        private int _donorId;
        private int _thirdPartyId;
        private string _thirdPartyFirstName;
        private string _thirdPartyLastName;
        private string _thirdPartyAddress1;
        private string _thirdPartyAddress2;
        private string _thirdPartyCity;
        private string _thirdPartyState;
        private string _thirdPartyZip;
        private string _thirdPartyPhone;
        private string _thirdPartyFax;
        private string _thirdPartyEmail;
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
        /// Get or Set the Donor Id value.
        /// </summary>
        public int DonorId
        {
            get
            {
                return this._donorId;
            }
            set
            {
                this._donorId = value;
            }
        }

        /// <summary>
        /// Get or Set the Third Party Id value.
        /// </summary>
        public int ThirdPartyId
        {
            get
            {
                return this._thirdPartyId;
            }
            set
            {
                this._thirdPartyId = value;
            }
        }

        /// <summary>
        /// Get or Set the Third Party First Name value.
        /// </summary>
        public string ThirdPartyFirstName
        {
            get
            {
                return this._thirdPartyFirstName;
            }
            set
            {
                this._thirdPartyFirstName = value;
            }
        }

        /// <summary>
        /// Get or Set the Third Party Last Name value.
        /// </summary>
        public string ThirdPartyLastName
        {
            get
            {
                return this._thirdPartyLastName;
            }
            set
            {
                this._thirdPartyLastName = value;
            }
        }

        /// <summary>
        /// Get the User Display Name value.
        /// </summary>
        public string UserDisplayName
        {
            get
            {
                return this._thirdPartyFirstName + " " + this._thirdPartyLastName;
            }
        }

        /// <summary>
        /// Get or Set the Third Party Address 1 value.
        /// </summary>
        public string ThirdPartyAddress1
        {
            get
            {
                return this._thirdPartyAddress1;
            }
            set
            {
                this._thirdPartyAddress1 = value;
            }
        }

        /// <summary>
        /// Get or Set the Third Party Address 2 value.
        /// </summary>
        public string ThirdPartyAddress2
        {
            get
            {
                return this._thirdPartyAddress2;
            }
            set
            {
                this._thirdPartyAddress2 = value;
            }
        }

        /// <summary>
        /// Get or Set the Third Party City value.
        /// </summary>
        public string ThirdPartyCity
        {
            get
            {
                return this._thirdPartyCity;
            }
            set
            {
                this._thirdPartyCity = value;
            }
        }

        /// <summary>
        /// Get or Set the Third Party State value.
        /// </summary>
        public string ThirdPartyState
        {
            get
            {
                return this._thirdPartyState;
            }
            set
            {
                this._thirdPartyState = value;
            }
        }

        /// <summary>
        /// Get or Set the Third Party ZipCode value.
        /// </summary>
        public string ThirdPartyZip
        {
            get
            {
                return this._thirdPartyZip;
            }
            set
            {
                this._thirdPartyZip = value;
            }
        }

        /// <summary>
        /// Get or Set the Third Party Phone value.
        /// </summary>
        public string ThirdPartyPhone
        {
            get
            {
                return this._thirdPartyPhone;
            }
            set
            {
                this._thirdPartyPhone = value;
            }
        }

        /// <summary>
        /// Get or Set the Third Party Fax value.
        /// </summary>
        public string ThirdPartyFax
        {
            get
            {
                return this._thirdPartyFax;
            }
            set
            {
                this._thirdPartyFax = value;
            }
        }

        /// <summary>
        /// Get or Set the Third Party Email value.
        /// </summary>
        public string ThirdPartyEmail
        {
            get
            {
                return this._thirdPartyEmail;
            }
            set
            {
                this._thirdPartyEmail = value;
            }
        }

        /// <summary>
        /// Get or Set the Third Party active status.
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