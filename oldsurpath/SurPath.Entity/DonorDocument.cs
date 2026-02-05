using System;

namespace SurPath.Entity
{
    public class DonorDocument
    {
        #region Private Variables

        private int _donorDocumentId;
        private int _donorId;
        private DateTime _documentUploadTime;
        private DateTime? _dateRequired;
        private string _documentTitle;
        private string _source;
        private string _uploadedBy;
        private string _fileName;
        private bool _isSynchronized;
        private bool _isNotify;
        private DateTime _lastNotified;
        private bool _isNeedsApproval;
        private bool _isApproved;
        private bool _isRejected;
        private bool _isUpdateable;
        private bool _isArchived;

        private byte[] _documentContent;

        #endregion Private Variables

        #region Public Properties

        public int DonorDocumentId
        {
            get
            {
                return this._donorDocumentId;
            }
            set
            {
                this._donorDocumentId = value;
            }
        }

        public int DonorId
        {
            get
            {
                return this._donorId;
            }
            set
            {
                this._donorId = value;
            }
        }

        public DateTime DocumentUploadTime
        {
            get
            {
                return this._documentUploadTime;
            }
            set
            {
                this._documentUploadTime = value;
            }
        }

        public DateTime? DateRequired
        {
            get
            {
                return this._dateRequired;
            }
            set
            {
                this._dateRequired = value;
            }
        }

        public string DocumentTitle
        {
            get
            {
                return this._documentTitle;
            }
            set
            {
                this._documentTitle = value;
            }
        }

        public string Source
        {
            get
            {
                return this._source;
            }
            set
            {
                this._source = value;
            }
        }

        public string UploadedBy
        {
            get
            {
                return this._uploadedBy;
            }
            set
            {
                this._uploadedBy = value;
            }
        }

        public string FileName
        {
            get
            {
                return this._fileName;
            }
            set
            {
                this._fileName = value;
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

        public bool IsNotify
        {
            get
            {
                return this._isNotify;
            }
            set
            {
                this._isNotify = value;
            }
        }

        public DateTime LastNotified
        {
            get
            {
                return this._lastNotified;
            }
            set
            {
                this._lastNotified = value;
            }
        }

        public bool IsNeedsApproval
        {
            get
            {
                return this._isNeedsApproval;
            }
            set
            {
                this._isNeedsApproval = value;
            }
        }

        public bool IsRejected
        {
            get
            {
                return this._isRejected;
            }
            set
            {
                this._isRejected = value;
            }
        }

        public bool IsApproved
        {
            get
            {
                return this._isApproved;
            }
            set
            {
                this._isApproved = value;
            }
        }

        public bool IsUpdatable
        {
            get
            {
                return this._isUpdateable;
            }
            set
            {
                this._isUpdateable = value;
            }
        }

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

        public DateTime Date
        {
            get
            {
                return this._documentUploadTime.Date;
            }
            set
            {
                //TimeSpan time = this.Time;
                this._documentUploadTime = value.Date;
            }
        }

        public TimeSpan Time
        {
            get
            {
                return this._documentUploadTime.TimeOfDay;
            }
            set
            {
                this._documentUploadTime = this.Date + value;
            }
        }

        public byte[] DocumentContent
        {
            get
            {
                return this._documentContent;
            }
            set
            {
                this._documentContent = value;
            }
        }

        #endregion Public Properties
    }
}