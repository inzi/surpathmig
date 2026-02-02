using SurPath.Enum;
using System;

namespace SurPath.Entity
{
    public class DonorTestInfoTestCategories
    {
        #region Private Variables

        private int _donorTestTestCategoryId;
        private int _donorTestInfoId;
        private TestCategories _testCategoryId;
        private int? _testPanelId;
        private string _specimenId;
        private int? _hairTestPanelDays;
        private int _testPanelResult;
        private DonorRegistrationStatus _testPanelStatus;
        private double? _testPanelCost;
        private double? _testPanelPrice;
        private bool _isSynchronized;
        private DateTime _createdOn;
        private string _createdBy;
        private DateTime _lastModifiedOn;
        private string _lastModifiedBy;
        private string _testPanelName;

        #endregion Private Variables

        #region Public Properties

        public int DonorTestTestCategoryId
        {
            get
            {
                return this._donorTestTestCategoryId;
            }
            set
            {
                this._donorTestTestCategoryId = value;
            }
        }

        public int DonorTestInfoId
        {
            get
            {
                return this._donorTestInfoId;
            }
            set
            {
                this._donorTestInfoId = value;
            }
        }

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

        public int? TestPanelId
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

        public string SpecimenId
        {
            get
            {
                return this._specimenId;
            }
            set
            {
                this._specimenId = value;
            }
        }

        public int? HairTestPanelDays
        {
            get
            {
                return this._hairTestPanelDays;
            }
            set
            {
                this._hairTestPanelDays = value;
            }
        }

        public int TestPanelResult
        {
            get
            {
                return this._testPanelResult;
            }
            set
            {
                this._testPanelResult = value;
            }
        }

        public DonorRegistrationStatus TestPanelStatus
        {
            get
            {
                return this._testPanelStatus;
            }
            set
            {
                this._testPanelStatus = value;
            }
        }

        public double? TestPanelCost
        {
            get
            {
                return this._testPanelCost;
            }
            set
            {
                this._testPanelCost = value;
            }
        }

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

        #endregion Public Properties
    }
}