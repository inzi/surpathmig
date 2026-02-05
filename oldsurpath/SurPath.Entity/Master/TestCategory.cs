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
    /// Test Category entity
    /// </summary>
    public class TestCategory
    {
        #region Private Variables

        private int _testCategoryId;
        private string _testCategoryName;
        private string _internalName;
        private bool _isSynchronized;
        private DateTime _createdOn;
        private string _createdBy;
        private DateTime _lastModifiedOn;
        private string _lastModifiedBy;

        #endregion Private Variables

        #region Public Properties

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

        /// <summary>
        /// Get or Set the Test Category Name, like UA, Hair, DNA and etc.
        /// </summary>
        public string TestCategoryName
        {
            get
            {
                return this._testCategoryName;
            }
            set
            {
                this._testCategoryName = value;
            }
        }

        /// <summary>
        /// Get or Set the Internal Name of the Test Category.
        /// </summary>
        public string InternalName
        {
            get
            {
                return this._internalName;
            }
            set
            {
                this._internalName = value;
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