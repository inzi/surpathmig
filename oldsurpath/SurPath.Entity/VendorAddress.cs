using SurPath.Enum;
using System;

namespace SurPath.Entity
{
    /// <summary>
    /// Vendor Address entity
    /// </summary>
    public class VendorAddress
    {
        #region Private Variables

        private int _addressId;
        private int _vendorId;
        private AddressTypes _addressTypeId;
        private string _address1;
        private string _address2;
        private string _city;
        private string _state;
        private string _zipCode;
        private string _phone;
        private string _fax;
        private string _email;
        private bool _isSynchronized;
        private bool _isArchived;
        private DateTime _createdOn;
        private string _createdBy;
        private DateTime _lastModifiedOn;
        private string _lastModifiedBy;

        #endregion Private Variables

        #region Public Properties

        /// <summary>
        /// Get or Set the  Address Id value.
        /// </summary>
        public int AddressId
        {
            get
            {
                return this._addressId;
            }
            set
            {
                this._addressId = value;
            }
        }

        /// <summary>
        /// Get or Set the Vendor Id value.
        /// </summary>
        public int VendorId
        {
            get
            {
                return this._vendorId;
            }
            set
            {
                this._vendorId = value;
            }
        }

        /// <summary>
        /// Get or Set the Address Type Id value.
        /// </summary>
        public AddressTypes AddressTypeId
        {
            get
            {
                return this._addressTypeId;
            }
            set
            {
                this._addressTypeId = value;
            }
        }

        /// <summary>
        /// Get or Set the Address 1 value.
        /// </summary>
        public string Address1
        {
            get
            {
                return this._address1;
            }
            set
            {
                this._address1 = value;
            }
        }

        /// <summary>
        /// Get or Set the Address 2 value.
        /// </summary>
        public string Address2
        {
            get
            {
                return this._address2;
            }
            set
            {
                this._address2 = value;
            }
        }

        /// <summary>
        /// Get or Set the City value.
        /// </summary>
        public string City
        {
            get
            {
                return this._city;
            }
            set
            {
                this._city = value;
            }
        }

        /// <summary>
        /// Get or Set the State value.
        /// </summary>
        public string State
        {
            get
            {
                return this._state;
            }
            set
            {
                this._state = value;
            }
        }

        /// <summary>
        /// Get or Set the Zip Code value.
        /// </summary>
        public string ZipCode
        {
            get
            {
                return this._zipCode;
            }
            set
            {
                this._zipCode = value;
            }
        }

        /// <summary>
        /// Get or Set the Phone value.
        /// </summary>
        public string Phone
        {
            get
            {
                return this._phone;
            }
            set
            {
                this._phone = value;
            }
        }

        /// <summary>
        /// Get or Set the Fax value.
        /// </summary>
        public string Fax
        {
            get
            {
                return this._fax;
            }
            set
            {
                this._fax = value;
            }
        }

        /// <summary>
        /// Get or Set the Email value.
        /// </summary>
        public string Email
        {
            get
            {
                return this._email;
            }
            set
            {
                this._email = value;
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