using System;

namespace SurPath.Entity
{
    public class ClientDeptTestPanel
    {
        #region Private Variables

        private int _clientDeptTestPanelId;
        private int _clientDeptTestCategoryId;
        private int _testPanelId;
        private string _testPanelName;
        private double _testPanelPrice;
        private int _displayOrder;
        private bool _isMainTestPanel;
        private bool _is1TestPanel;
        private bool _is2TestPanel;
        private bool _is3TestPanel;
        private bool _is4TestPanel;
        private bool _isSynchronized;
        private DateTime _createdOn;
        private string _createdBy;
        private DateTime _lastModifiedOn;
        private string _lastModifiedBy;

        #endregion Private Variables

        #region Public Properties

        /// <summary>
        /// Get or Set the Client Department Test Panel Id.
        /// </summary>
        public int ClientDeptTestPanelId
        {
            get
            {
                return this._clientDeptTestPanelId;
            }
            set
            {
                this._clientDeptTestPanelId = value;
            }
        }

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
        /// Get or Set the Test Panel Id.
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
        /// Get or Set the Test Panel Name.
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
        /// Get or Set the Test Panel Price.
        /// </summary>
        public double TestPanelPrice
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
        /// Get or Set the IsMainTestPanel.
        /// </summary>
        public bool IsMainTestPanel
        {
            get
            {
                return this._isMainTestPanel;
            }
            set
            {
                this._isMainTestPanel = value;
            }
        }

        /// <summary>
        /// Get or Set the Is1TestPanel.
        /// </summary>
        public bool Is1TestPanel
        {
            get
            {
                return this._is1TestPanel;
            }
            set
            {
                this._is1TestPanel = value;
            }
        }

        /// <summary>
        /// Get or Set the Is2TestPanel.
        /// </summary>
        public bool Is2TestPanel
        {
            get
            {
                return this._is2TestPanel;
            }
            set
            {
                this._is2TestPanel = value;
            }
        }

        /// <summary>
        /// Get or Set the Is3TestPanel.
        /// </summary>
        public bool Is3TestPanel
        {
            get
            {
                return this._is3TestPanel;
            }
            set
            {
                this._is3TestPanel = value;
            }
        }

        /// <summary>
        /// Get or Set the Is4TestPanel.
        /// </summary>
        public bool Is4TestPanel
        {
            get
            {
                return this._is4TestPanel;
            }
            set
            {
                this._is4TestPanel = value;
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