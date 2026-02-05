using SurPath.Enum;
using System;
using System.Collections.Generic;

namespace SurPath.Entity
{
    public class ClientDeptTestCategory
    {
        #region Private Variables

        private int _clientDeptTestCategoryId;
        private int _clientDepartmentId;
        private TestCategories _testCategoryId;
        private int _displayOrder;
        private double? _testPanelPrice;

        private List<ClientDeptTestPanel> _clientDeptTestPanels = new List<ClientDeptTestPanel>();

        private bool _isSynchronized;
        private DateTime _createdOn;
        private string _createdBy;
        private DateTime _lastModifiedOn;
        private string _lastModifiedBy;

        #endregion Private Variables

        #region Public Properties

        /// <summary>
        /// Get or Set the Client Department Test Category Id.
        /// </summary>
        public int ClientDeptTestCategoryId
        {
            get
            {
                return this._clientDeptTestCategoryId;
            }
            set
            {
                this._clientDeptTestCategoryId = value;
            }
        }

        /// <summary>
        /// Get or Set the Client Department Id.
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
        /// Get or Set the Test Category Id.
        /// </summary>
        public TestCategories TestCategoryId
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
        /// Get or Set the Display Order.
        /// </summary>
        public int DisplayOrder
        {
            get
            {
                return this._displayOrder;
            }
            set
            {
                this._displayOrder = value;
            }
        }

        /// <summary>
        /// Get or Set the Test Panel Price.
        /// </summary>
        public double? TestPanelPrice
        {
            get
            {
                return this._testPanelPrice;
            }
            set
            {
                this._testPanelPrice = value;
            }
        }

        /// <summary>
        /// Get or Set the Department Test Category - Test Panels.
        /// </summary>
        public List<ClientDeptTestPanel> ClientDeptTestPanels
        {
            get
            {
                return this._clientDeptTestPanels;
            }
            set
            {
                this._clientDeptTestPanels = value;
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