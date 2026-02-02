using System;

namespace SurPath.Entity.Master
{
    public class Judge
    {
        #region Private Variables

        private int _judgeId;
        private string _judgeusername;
        private string _judgePassword;
        private string _judgePrefix;
        private string _judgeFirstName;
        private string _judgeLastName;
        private string _judgeSuffix;
        private string _judgeAddress1;
        private string _judgeAddress2;
        private string _judgeCity;
        private string _judgeState;
        private string _judgeZip;
        private bool _isActive;
        private bool _isSynchronized;
        private bool _isArchived;
        private DateTime _createdOn;
        private string _createdBy;
        private DateTime _lastModifiedOn;
        private string _lastModifiedBy;

        #endregion Private Variables

        #region Public Properties

        public int JudgeId
        {
            get
            {
                return this._judgeId;
            }
            set
            {
                this._judgeId = value;
            }
        }

        /// <summary>
        /// Get or Set the Judge Username value.
        /// </summary>
        public string JudgeUsername
        {
            get
            {
                return this._judgeusername;
            }
            set
            {
                this._judgeusername = value;
            }
        }

        /// <summary>
        /// Get or Set the Judge Password value.
        /// </summary>
        public string JudgePassword
        {
            get
            {
                return this._judgePassword;
            }
            set
            {
                this._judgePassword = value;
            }
        }

        public string JudgePrefix
        {
            get
            {
                return this._judgePrefix;
            }
            set
            {
                this._judgePrefix = value;
            }
        }

        public string JudgeFirstName
        {
            get
            {
                return this._judgeFirstName;
            }
            set
            {
                this._judgeFirstName = value;
            }
        }

        public string JudgeLastName
        {
            get
            {
                return this._judgeLastName;
            }
            set
            {
                this._judgeLastName = value;
            }
        }

        public string UserDisplayName
        {
            get
            {
                return this._judgeFirstName + " " + this._judgeLastName;
            }
        }

        public string JudgeSuffix
        {
            get
            {
                return this._judgeSuffix;
            }
            set
            {
                this._judgeSuffix = value;
            }
        }

        public string JudgeAddress1
        {
            get
            {
                return this._judgeAddress1;
            }
            set
            {
                this._judgeAddress1 = value;
            }
        }

        public string JudgeAddress2
        {
            get
            {
                return this._judgeAddress2;
            }
            set
            {
                this._judgeAddress2 = value;
            }
        }

        public string JudgeCity
        {
            get
            {
                return this._judgeCity;
            }
            set
            {
                this._judgeCity = value;
            }
        }

        public string JudgeState
        {
            get
            {
                return this._judgeState;
            }
            set
            {
                this._judgeState = value;
            }
        }

        public string JudgeZip
        {
            get
            {
                return this._judgeZip;
            }
            set
            {
                this._judgeZip = value;
            }
        }

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