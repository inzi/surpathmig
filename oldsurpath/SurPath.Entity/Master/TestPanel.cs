using System;
using System.Collections.Generic;

namespace SurPath.Entity.Master
{
    /// <summary>
    /// Test Panel entity
    /// </summary>
    public class TestPanel
    {
        #region Private Variables

        private int _testPanelId;
        private string _testPanelName;
        private string _testPanelDescription;
        private int _testCategoryId;
        private double _testCost;
        private bool _isActive;
        private List<int> _drugNames = new List<int>();
        private bool _isSynchronized;
        private bool _isArchived;
        private DateTime _createdOn;
        private string _createdBy;
        private DateTime _lastModifiedOn;
        private string _lastModifiedBy;

        #endregion Private Variables

        #region Public Properties

        /// <summary>
        /// Get or Set the Test Panel Id value.
        /// </summary>
        public int TestPanelId
        {
            get
            {
                return this._testPanelId;
            }
            set
            {
                this._testPanelId = value;
            }
        }

        /// <summary>
        /// Get or Set the Test Panel Name value.
        /// </summary>
        public string TestPanelName
        {
            get
            {
                return this._testPanelName;
            }
            set
            {
                this._testPanelName = value;
            }
        }

        /// <summary>
        /// Get or Set the Test Panel Description.
        /// </summary>
        public string TestPanelDescription
        {
            get
            {
                return this._testPanelDescription;
            }
            set
            {
                this._testPanelDescription = value;
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

        /// <summary>
        /// Get or Set the Test cost value.
        /// </summary>
        public double TestCost
        {
            get
            {
                return this._testCost;
            }
            set
            {
                this._testCost = value;
            }
        }

        /// <summary>
        /// Get or Set the TestPanel active status.
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
        /// Get or Set the Drug Names list.
        /// </summary>
        public List<int> DrugNames
        {
            get
            {
                return this._drugNames;
            }
            set
            {
                this._drugNames = value;
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