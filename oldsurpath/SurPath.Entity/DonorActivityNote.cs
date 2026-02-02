using SurPath.Enum;
using System;

namespace SurPath.Entity
{
    public class DonorActivityNote
    {
        #region Private Variables

        private int _donorTestActivityId;
        private int _donorTestInfoId;
        private DateTime _activityDateTime;
        private int _activityUserId;
        private string _activityUserName;
        private DonorActivityCategories _activityCategoryId;
        private bool _isActivityVisible;
        private string _activityNote;
        private bool _isSynchronized;

        #endregion Private Variables

        #region Public Properties

        public int DonorTestActivityId
        {
            get
            {
                return this._donorTestActivityId;
            }
            set
            {
                this._donorTestActivityId = value;
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

        public DateTime ActivityDateTime
        {
            get
            {
                return this._activityDateTime;
            }
            set
            {
                this._activityDateTime = value;
            }
        }

        public int ActivityUserId
        {
            get
            {
                return this._activityUserId;
            }
            set
            {
                this._activityUserId = value;
            }
        }

        public string ActivityUserName
        {
            get
            {
                return this._activityUserName;
            }
            set
            {
                this._activityUserName = value;
            }
        }

        public DonorActivityCategories ActivityCategoryId
        {
            get
            {
                return this._activityCategoryId;
            }
            set
            {
                this._activityCategoryId = value;
            }
        }

        public bool IsActivityVisible
        {
            get
            {
                return this._isActivityVisible;
            }
            set
            {
                this._isActivityVisible = value;
            }
        }

        public string ActivityNote
        {
            get
            {
                return this._activityNote;
            }
            set
            {
                this._activityNote = value;
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

        #endregion Public Properties
    }
}