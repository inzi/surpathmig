using SurPath.Enum;
using System;
using System.Collections.Generic;

namespace SurPath.Entity
{
    public class Donor
    {
        #region Private Variables

        private int _donorId;
        private string _donorClearStarProfId;
        private string _donorFirstName;
        private string _donorMI;
        private string _donorLastName;
        private bool _isHiddenWeb;
        private string _donorSuffix;
        private string _donorSSN;
        private DateTime _donorDateOfBirth;
        private string _donorPhone1;
        private string _donorPhone2;
        private string _donorAddress1;
        private string _donorAddress2;
        private string _donorCity;
        private string _donorState;
        private string _donorZip;
        private string _donorEmail;
        private Gender _donorGender;
        private int? _donorInitialClientId;
        private int? _donorInitialDepartmentId;
        private DonorRegistrationStatus _donorRegistrationStatusValue;
        private bool _isSynchronized;
        private bool _isArchived;
        private DateTime _createdOn;
        private string _createdBy;
        private DateTime _lastModifiedOn;
        private string _lastModifiedBy;
        private string _notapproved;

        private double _programAmount;
        private int _donorTestInfoId;
        private ClientPaymentTypes _clientPaymentType;
        private ClientDepartment _clientDepartment;

        private bool _isWalkinDonor;

        private string _columnname;
        private int _columnid;
        private bool _isActive;

        #endregion Private Variables

        #region Public Properties

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

        public string DonorClearStarProfId
        {
            get
            {
                return this._donorClearStarProfId;
            }
            set
            {
                this._donorClearStarProfId = value;
            }
        }

        public string DonorFirstName
        {
            get
            {
                return this._donorFirstName;
            }
            set
            {
                this._donorFirstName = value;
            }
        }

        public string DonorMI
        {
            get
            {
                return this._donorMI;
            }
            set
            {
                this._donorMI = value;
            }
        }

        public string DonorLastName
        {
            get
            {
                return this._donorLastName;
            }
            set
            {
                this._donorLastName = value;
            }
        }

        public bool IsHiddenWeb
        {
            get
            {
                return this._isHiddenWeb;
            }
            set
            {
                this._isHiddenWeb = value;
            }
        }

        public string DonorSuffix
        {
            get
            {
                return this._donorSuffix;
            }
            set
            {
                this._donorSuffix = value;
            }
        }

        public string DonorSSN
        {
            get
            {
                return this._donorSSN;
            }
            set
            {
                this._donorSSN = value;
            }
        }

        public DateTime DonorDateOfBirth
        {
            get
            {
                return this._donorDateOfBirth;
            }
            set
            {
                this._donorDateOfBirth = value;
            }
        }

        public string DonorPhone1
        {
            get
            {
                return this._donorPhone1;
            }
            set
            {
                this._donorPhone1 = value;
            }
        }

        public string DonorPhone2
        {
            get
            {
                return this._donorPhone2;
            }
            set
            {
                this._donorPhone2 = value;
            }
        }

        public string DonorAddress1
        {
            get
            {
                return this._donorAddress1;
            }
            set
            {
                this._donorAddress1 = value;
            }
        }

        public string DonorAddress2
        {
            get
            {
                return this._donorAddress2;
            }
            set
            {
                this._donorAddress2 = value;
            }
        }

        public string DonorCity
        {
            get
            {
                return this._donorCity;
            }
            set
            {
                this._donorCity = value;
            }
        }

        public string DonorState
        {
            get
            {
                return this._donorState;
            }
            set
            {
                this._donorState = value;
            }
        }

        public string DonorZip
        {
            get
            {
                return this._donorZip;
            }
            set
            {
                this._donorZip = value;
            }
        }

        public string DonorEmail
        {
            get
            {
                return this._donorEmail;
            }
            set
            {
                this._donorEmail = value;
            }
        }

        public Gender DonorGender
        {
            get
            {
                return this._donorGender;
            }
            set
            {
                this._donorGender = value;
            }
        }

        public int? DonorInitialClientId
        {
            get
            {
                return this._donorInitialClientId;
            }
            set
            {
                this._donorInitialClientId = value;
            }
        }

        public int? DonorInitialDepartmentId
        {
            get
            {
                return this._donorInitialDepartmentId;
            }
            set
            {
                this._donorInitialDepartmentId = value;
            }
        }

        public DonorRegistrationStatus DonorRegistrationStatusValue
        {
            get
            {
                return this._donorRegistrationStatusValue;
            }
            set
            {
                this._donorRegistrationStatusValue = value;
            }
        }

        public string TemporaryPassword { get; set; }

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

        public string NotApproved

        {
            get
            {
                return this._notapproved;
            }
            set
            {
                this._notapproved = value;
            }
        }

        public double ProgramAmount
        {
            get
            {
                return this._programAmount;
            }
            set
            {
                this._programAmount = value;
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

        public ClientPaymentTypes ClientPaymentType
        {
            get
            {
                return this._clientPaymentType;
            }
            set
            {
                this._clientPaymentType = value;
            }
        }

        public ClientDepartment ClientDepartment
        {
            get
            {
                return this._clientDepartment;
            }
            set
            {
                this._clientDepartment = value;
            }
        }

        public bool IsWalkinDonor
        {
            get
            {
                return this._isWalkinDonor;
            }
            set
            {
                this._isWalkinDonor = value;
            }
        }

        public string ColumnName
        {
            get
            {
                return this._columnname;
            }
            set
            {
                this._columnname = value;
            }
        }

        public int ColumnId
        {
            get
            {
                return this._columnid;
            }
            set
            {
                this._columnid = value;
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

        public bool notified { get; set; } = false;
        public DateTime notified_datetime { get; set; } = DateTime.Now;
        public List<PIDTypeValue> PidTypeValues { get; set; } = new List<PIDTypeValue>();
        #endregion Public Properties
    }
}