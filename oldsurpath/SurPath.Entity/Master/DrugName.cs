// ===============================================================================
// DrugName.cs
//
// This file contains the properties Drug Names.
// ===============================================================================
// Release history
// VERSION	DESCRIPTION
//
// ===============================================================================
// Copyright (C) 2014 SaaSWorks Technologies Pvt. Ltd.
// http://www.saasworksit.com
// All rights reserved.
// ==============================================================================

using System;

namespace SurPath.Entity.Master
{
    /// <summary>
    /// Drug Name entity
    /// </summary>
    public class DrugName
    {
        #region Private Variables

        private int _drugNameId;
        private string _drugNameValue;
        private string _drugCode;
        private string _uaScreenValue;
        private string _uaConfirmationValue;
        private string _hairScreenValue;
        private string _hairConfirmationValue;
        private string _UAunitOfMeasurement;
        private string _HairunitOfMeasurement;
        private bool _isUA;
        private bool _isHair;
        private bool _isBC;
        private bool _isDNA;
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
        /// Get or Set the Drug Name Id value.
        /// </summary>
        public int DrugNameId
        {
            get
            {
                return this._drugNameId;
            }
            set
            {
                this._drugNameId = value;
            }
        }

        /// <summary>
        /// Get or Set the Drug Name value.
        /// </summary>
        public string DrugNameValue
        {
            get
            {
                return this._drugNameValue;
            }
            set
            {
                this._drugNameValue = value;
            }
        }

        /// <summary>
        /// Get or Set the Drug Code value.
        /// </summary>
        public string DrugCodeValue
        {
            get
            {
                return this._drugCode;
            }
            set
            {
                this._drugCode = value;
            }
        }

        /// <summary>
        /// Get or Set the UA screen value.
        /// </summary>
        public string UAScreenValue
        {
            get
            {
                return this._uaScreenValue;
            }
            set
            {
                this._uaScreenValue = value;
            }
        }

        /// <summary>
        /// Get or Set the UA confirmation value.
        /// </summary>
        public string UAConfirmationValue
        {
            get
            {
                return this._uaConfirmationValue;
            }
            set
            {
                this._uaConfirmationValue = value;
            }
        }

        /// <summary>
        /// Get or Set the Hair screen value.
        /// </summary>
        public string HairScreenValue
        {
            get
            {
                return this._hairScreenValue;
            }
            set
            {
                this._hairScreenValue = value;
            }
        }

        /// <summary>
        /// Get or Set the Hair confirmation value.
        /// </summary>
        public string HairConfirmationValue
        {
            get
            {
                return this._hairConfirmationValue;
            }
            set
            {
                this._hairConfirmationValue = value;
            }
        }

        /// <summary>
        /// Get or Set the Unit of Measurement for UA.
        /// </summary>
        public string UAUnitOfMeasurement
        {
            get
            {
                return this._UAunitOfMeasurement;
            }
            set
            {
                this._UAunitOfMeasurement = value;
            }
        }

        /// <summary>
        /// Get or Set the Unit of Measurement for Hair.
        /// </summary>
        public string HairUnitOfMeasurement
        {
            get
            {
                return this._HairunitOfMeasurement;
            }
            set
            {
                this._HairunitOfMeasurement = value;
            }
        }

        /// <summary>
        /// Get or Set the UA Category available or not.
        /// </summary>
        public bool IsUA
        {
            get
            {
                return this._isUA;
            }
            set
            {
                this._isUA = value;
            }
        }

        /// <summary>
        /// Get or Set the Hair Category available or not.
        /// </summary>
        public bool IsHair
        {
            get
            {
                return this._isHair;
            }
            set
            {
                this._isHair = value;
            }
        }

        /// <summary>
        /// Get or Set the Hair Category available or not.
        /// </summary>
        public bool IsBC
        {
            get
            {
                return this._isBC;
            }
            set
            {
                this._isBC = value;
            }
        }

        /// <summary>
        /// Get or Set the Hair Category available or not.
        /// </summary>
        public bool IsDNA
        {
            get
            {
                return this._isDNA;
            }
            set
            {
                this._isDNA = value;
            }
        }

        /// <summary>
        /// Get or Set the Active status.
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