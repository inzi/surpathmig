using SurPath.Enum;
using System;
using System.Collections.Generic;

namespace SurPath.Entity
{
    /// <summary>
    /// Vendor entity
    /// </summary>
    public class Vendor
    {
        #region Private Variables

        private int _vendorId;
        private VendorTypes _vendorTypeId;
        private string _vendorName;
        private string _vendorMainContact;
        private string _vendorPhone;
        private string _vendorFax;
        private string _vendorEmail;
        private string _vendorCity;
        private string _vendorState;
        private VendorStatus _vendorStatus;
        private DateTime _inactiveDate;
        private string _inactiveReason;
        private int _isMailingAddressPhysical1;
        private double? _mposMROCost;
        private double? _mallMROCost;
        private List<int> _services = new List<int>();
        private List<VendorAddress> _addresses = new List<VendorAddress>();
        private bool _isSynchronized;
        private bool _isArchived;
        private DateTime _createdOn;
        private string _createdBy;
        private DateTime _lastModifiedOn;
        private string _lastModifiedBy;

        #endregion Private Variables

        #region Public Properties

        /// <summary>
        /// Get or Set the  Vendor Id value.
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
        /// Get or Set the  Vendor Type Id value.
        /// </summary>
        public VendorTypes VendorTypeId
        {
            get
            {
                return this._vendorTypeId;
            }
            set
            {
                this._vendorTypeId = value;
            }
        }

        /// <summary>
        /// Get or Set the Vendor Name value.
        /// </summary>
        public string VendorName
        {
            get
            {
                return this._vendorName;
            }
            set
            {
                this._vendorName = value;
            }
        }

        /// <summary>
        /// Get or Set the Vendor Main Contact value.
        /// </summary>
        public string VendorMainContact
        {
            get
            {
                return this._vendorMainContact;
            }
            set
            {
                this._vendorMainContact = value;
            }
        }

        /// <summary>
        /// Get or Set the Vendor Phone value.
        /// </summary>
        public string VendorPhone
        {
            get
            {
                return this._vendorPhone;
            }
            set
            {
                this._vendorPhone = value;
            }
        }

        /// <summary>
        /// Get or Set the Vendor Fax value.
        /// </summary>
        public string VendorFax
        {
            get
            {
                return this._vendorFax;
            }
            set
            {
                this._vendorFax = value;
            }
        }

        /// <summary>
        /// Get or Set the Vendor Email value.
        /// </summary>
        public string VendorEmail
        {
            get
            {
                return this._vendorEmail;
            }
            set
            {
                this._vendorEmail = value;
            }
        }

        /// <summary>
        /// Get or Set the Vendor City value.
        /// </summary>
        public string VendorCity
        {
            get
            {
                return this._vendorCity;
            }
            set
            {
                this._vendorCity = value;
            }
        }

        /// <summary>
        /// Get or Set the Vendor State value.
        /// </summary>
        public string VendorState
        {
            get
            {
                return this._vendorState;
            }
            set
            {
                this._vendorState = value;
            }
        }

        /// <summary>
        /// Get or Set the Vendor Status value.
        /// </summary>
        public VendorStatus VendorStatus
        {
            get
            {
                return this._vendorStatus;
            }
            set
            {
                this._vendorStatus = value;
            }
        }

        /// <summary>
        /// Get or Set the Vendor InActive Date value.
        /// </summary>
        public DateTime InactiveDate
        {
            get
            {
                return this._inactiveDate;
            }
            set
            {
                this._inactiveDate = value;
            }
        }

        /// <summary>
        /// Get or Set the Vendor Inactive Reason value.
        /// </summary>
        public string InactiveReason
        {
            get
            {
                return this._inactiveReason;
            }
            set
            {
                this._inactiveReason = value;
            }
        }

        /// <summary>
        /// Get or Set the whether the Pysical Address 1 will be the same as mailing address or not.
        /// </summary>
        public int IsMailingAddressPhysical1
        {
            get
            {
                return this._isMailingAddressPhysical1;
            }
            set
            {
                this._isMailingAddressPhysical1 = value;
            }
        }

        /// <summary>
        /// Get or Set the MPOS Cost value.
        /// </summary>
        public double? MPOSMROCost
        {
            get
            {
                return this._mposMROCost;
            }
            set
            {
                this._mposMROCost = value;
            }
        }

        /// <summary>
        /// Get or Set the MALL cost value.
        /// </summary>
        public double? MALLMROCost
        {
            get
            {
                return this._mallMROCost;
            }
            set
            {
                this._mallMROCost = value;
            }
        }

        /// <summary>
        /// Get or Set the Vendor Service list.
        /// </summary>
        public List<int> Services
        {
            get
            {
                return this._services;
            }
            set
            {
                this._services = value;
            }
        }

        /// <summary>
        /// Get or Set the Vendor Address list.
        /// </summary>
        public List<VendorAddress> Addresses
        {
            get
            {
                return this._addresses;
            }
            set
            {
                this._addresses = value;
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