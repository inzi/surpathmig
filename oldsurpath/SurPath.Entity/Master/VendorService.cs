using SurPath.Enum;
using System;

namespace SurPath.Entity.Master
{
    public class VendorService
    {
        #region Private Variables

        private int _vendorServiceId;
        private int _vendorId;
        private string _vendorServiceNameValue;
        private double _cost;
        private int _testCategoryId;
        private YesNo _isObserved;
        private SpecimenFormType _formTypeId;
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
        /// Get or Set the Vendor Service Id value.
        /// </summary>
        public int VendorServiceId
        {
            get
            {
                return this._vendorServiceId;
            }
            set
            {
                this._vendorServiceId = value;
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
        /// Get or Set the Vendor Service Name value.
        /// </summary>
        public string VendorServiceNameValue
        {
            get
            {
                return this._vendorServiceNameValue;
            }
            set
            {
                this._vendorServiceNameValue = value;
            }
        }

        /// <summary>
        /// Get or Set the screen value.
        /// </summary>
        public double Cost
        {
            get
            {
                return this._cost;
            }
            set
            {
                this._cost = value;
            }
        }

        /// <summary>
        /// Get or Set the Test Category Id value.
        /// </summary>
        public int TestCategoryId
        {
            get
            {
                return this._testCategoryId;
            }
            set
            {
                this._testCategoryId = value;
            }
        }

        public YesNo IsObserved
        {
            get
            {
                return this._isObserved;
            }
            set
            {
                this._isObserved = value;
            }
        }

        public SpecimenFormType FormTypeId
        {
            get
            {
                return this._formTypeId;
            }
            set
            {
                this._formTypeId = value;
            }
        }

        /// <summary>
        /// Get or Set the Vendor active status.
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