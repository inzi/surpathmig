// ===============================================================================
// Court.cs
//
// This file contains the properties Court.
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
    /// Court entity
    /// </summary>
    public class Court
    {
        #region Private Variables

        private int _courtId;
        private string _courtusername;
        private string _courtPassword;
        private string _courtName;
        private string _courtAddress1;
        private string _courtAddress2;
        private string _courtCity;
        private string _courtState;
        private string _courtZip;
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
        /// Get or Set the Court Id value.
        /// </summary>
        public int CourtId
        {
            get
            {
                return this._courtId;
            }
            set
            {
                this._courtId = value;
            }
        }

        /// <summary>
        /// Get or Set the Court Username value.
        /// </summary>
        public string CourtUsername
        {
            get
            {
                return this._courtusername;
            }
            set
            {
                this._courtusername = value;
            }
        }

        /// <summary>
        /// Get or Set the Court Password value.
        /// </summary>
        public string CourtPassword
        {
            get
            {
                return this._courtPassword;
            }
            set
            {
                this._courtPassword = value;
            }
        }

        /// <summary>
        /// Get or Set the Court Name value.
        /// </summary>
        public string CourtName
        {
            get
            {
                return this._courtName;
            }
            set
            {
                this._courtName = value;
            }
        }

        /// <summary>
        /// Get or Set the Court Address 1 value.
        /// </summary>
        public string CourtAddress1
        {
            get
            {
                return this._courtAddress1;
            }
            set
            {
                this._courtAddress1 = value;
            }
        }

        /// <summary>
        /// Get or Set the Court Address 2 value.
        /// </summary>
        public string CourtAddress2
        {
            get
            {
                return this._courtAddress2;
            }
            set
            {
                this._courtAddress2 = value;
            }
        }

        /// <summary>
        /// Get or Set the Court City value.
        /// </summary>
        public string CourtCity
        {
            get
            {
                return this._courtCity;
            }
            set
            {
                this._courtCity = value;
            }
        }

        /// <summary>
        /// Get or Set the Court State value.
        /// </summary>
        public string CourtState
        {
            get
            {
                return this._courtState;
            }
            set
            {
                this._courtState = value;
            }
        }

        /// <summary>
        /// Get or Set the Court Zip value.
        /// </summary>
        public string CourtZip
        {
            get
            {
                return this._courtZip;
            }
            set
            {
                this._courtZip = value;
            }
        }

        /// <summary>
        /// Get or Set the Court active status.
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