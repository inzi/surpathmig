// ===============================================================================
// User.cs
//
// This file contains the properties User.
// ===============================================================================
// Release history
// VERSION	DESCRIPTION
//
// ===============================================================================
// Copyright (C) 2014 SaaSWorks Technologies Pvt. Ltd.
// http://www.saasworksit.com
// All rights reserved.
// ==============================================================================

using SurPath.Enum;
using System;
using System.Collections.Generic;

namespace SurPath.Entity
{
    public class User
    {
        #region Private Variables

        private int _userId;
        private string _username;
        private string _userPassword;
        private bool _isUserActive;
        private string _userFirstName;
        private string _userLastName;
        private string _userPhoneNumber;
        private string _userFax;
        private string _userEmail;

        private bool _changePasswordRequired;
        private UserType _userType;
        private int? _departmentId;
        private int? _donorId;
        private int? _clientId;
        private int? _vendorId;
        private int? _attorneyId;
        private int? _courtId;
        private int? _judgeId;

        private string _departmentName;
        private string _donorName;
        private string _clientName;
        private string _vendorName;
        private string _attorneyName;
        private string _courtName;
        private string _judgeName;
        private string _userTypeName;

        //private List<Client> _clientNameList = new List<Client>();
        //private List<ClientDepartment> _departmentNameList = new List<ClientDepartment>();

        private int _authRuleCategoryId;
        private string _authRuleCategoryName;
        private string _authRuleSubCategoryName;
        private string _authRuleName;

        private bool _isSynchronized;
        private bool _isArchived;
        private DateTime _createdOn;
        private string _createdBy;
        private DateTime _lastModifiedOn;
        private string _lastModifiedBy;

        private List<int> _clientDepartmentList = new List<int>();
        private List<int> _authRuleList = new List<int>();

        private string _programExists;

        #endregion Private Variables

        #region Public Properties

        /// <summary>
        /// Get or Set the User Id value.
        /// </summary>
        public int UserId
        {
            get
            {
                return this._userId;
            }
            set
            {
                this._userId = value;
            }
        }

        /// <summary>
        /// Get or Set the Username value.
        /// </summary>
        public string Username
        {
            get
            {
                return this._username;
            }
            set
            {
                this._username = value;
            }
        }

        /// <summary>
        /// Get or Set the User Password value.
        /// </summary>
        public string UserPassword
        {
            get
            {
                return this._userPassword;
            }
            set
            {
                this._userPassword = value;
            }
        }

        /// <summary>
        /// Get or Set the Usr active status.
        /// </summary>
        public bool IsUserActive
        {
            get
            {
                return this._isUserActive;
            }
            set
            {
                this._isUserActive = value;
            }
        }

        /// <summary>
        /// Get or Set the User First Name value.
        /// </summary>
        public string UserFirstName
        {
            get
            {
                return this._userFirstName;
            }
            set
            {
                this._userFirstName = value;
            }
        }

        /// <summary>
        /// Get or Set the User Last Name value.
        /// </summary>
        public string UserLastName
        {
            get
            {
                return this._userLastName;
            }
            set
            {
                this._userLastName = value;
            }
        }

        /// <summary>
        /// Get the User Display Name value.
        /// </summary>
        public string UserDisplayName
        {
            get
            {
                return this._userFirstName + " " + this._userLastName;
            }
        }

        /// <summary>
        /// Get or Set the User Phone Number value.
        /// </summary>
        public string UserPhoneNumber
        {
            get
            {
                return this._userPhoneNumber;
            }
            set
            {
                this._userPhoneNumber = value;
            }
        }

        /// <summary>
        /// Get or Set the User Fax value.
        /// </summary>
        public string UserFax
        {
            get
            {
                return this._userFax;
            }
            set
            {
                this._userFax = value;
            }
        }

        /// <summary>
        /// Get or Set the User Email value.
        /// </summary>
        public string UserEmail
        {
            get
            {
                return this._userEmail;
            }
            set
            {
                this._userEmail = value;
            }
        }

        /// <summary>
        /// Get or Set the user password change status.
        /// </summary>
        public bool ChangePasswordRequired
        {
            get
            {
                return this._changePasswordRequired;
            }
            set
            {
                this._changePasswordRequired = value;
            }
        }

        /// <summary>
        /// Get or Set the User Type value.
        /// </summary>
        public UserType UserType
        {
            get
            {
                return this._userType;
            }
            set
            {
                this._userType = value;
            }
        }

        /// <summary>
        /// Get or Set the Department Id value.
        /// </summary>
        public int? DepartmentId
        {
            get
            {
                return this._departmentId;
            }
            set
            {
                this._departmentId = value;
            }
        }

        /// <summary>
        /// Get or Set the Donor Id value.
        /// </summary>
        public int? DonorId
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

        /// <summary>
        /// Get or Set the Client Id value.
        /// </summary>
        public int? ClientId
        {
            get
            {
                return this._clientId;
            }
            set
            {
                this._clientId = value;
            }
        }

        /// <summary>
        /// Get or Set the Vendor Id value.
        /// </summary>
        public int? VendorId
        {
            get
            {
                return this._vendorId;
            }
            set
            {
                this._vendorId = value;
            }
        }

        /// <summary>
        /// Get or Set the Attorney Id value.
        /// </summary>
        public int? AttorneyId
        {
            get
            {
                return this._attorneyId;
            }
            set
            {
                this._attorneyId = value;
            }
        }

        /// <summary>
        /// Get or Set the Court Id value.
        /// </summary>
        public int? CourtId
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
        /// Get or Set the Judge Id value.
        /// </summary>
        public int? JudgeId
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
        /// Get or Set the Departnment Name information.
        /// </summary>
        public string DepartmentName
        {
            get
            {
                return this._departmentName;
            }
            set
            {
                this._departmentName = value;
            }
        }

        /// <summary>
        /// Get or Set the Donor Name information.
        /// </summary>
        public string DonorName
        {
            get
            {
                return this._donorName;
            }
            set
            {
                this._donorName = value;
            }
        }

        /// <summary>
        /// Get or Set the Client Name information.
        /// </summary>
        public string ClientNames
        {
            get
            {
                return this._clientName;
            }
            set
            {
                this._clientName = value;
            }
        }

        /// <summary>
        /// Get or Set the Vendor Name information.
        /// </summary>
        public string VendorNames
        {
            get
            {
                return this._vendorName;
            }
            set
            {
                this._vendorName = value;
            }
        }

        /// <summary>
        /// Get or Set the Attorney Name information.
        /// </summary>
        public string AttorneyNames
        {
            get
            {
                return this._attorneyName;
            }
            set
            {
                this._attorneyName = value;
            }
        }

        /// <summary>
        /// Get or Set the Court Name information.
        /// </summary>
        public string CourtNames
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
        /// Get or Set the Judge Name information.
        /// </summary>
        public string JudgeNames
        {
            get
            {
                return this._judgeName;
            }
            set
            {
                this._judgeName = value;
            }
        }

        /// <summary>
        /// Get or Set the User Type Name information.
        /// </summary>
        public string UserTypeNames
        {
            get
            {
                return this._userTypeName;
            }
            set
            {
                this._userTypeName = value;
            }
        }

        ///// <summary>
        ///// Get or Set the Client Name information.
        ///// </summary>
        //public List<Client> ClientName
        //{
        //    get
        //    {
        //        return this._clientNameList;
        //    }
        //    set
        //    {
        //        this._clientNameList = value;
        //    }
        //}

        ///// <summary>
        ///// Get or Set the Client Department information.
        ///// </summary>
        //public List<ClientDepartment> ClientDepartmentsName
        //{
        //    get
        //    {
        //        return this._departmentNameList;
        //    }
        //    set
        //    {
        //        this._departmentNameList = value;
        //    }
        //}

        /// <summary>
        /// Get or Set the Auth Rule Category Id information.
        /// </summary>
        public int AuthRuleCategoryId
        {
            get
            {
                return this._authRuleCategoryId;
            }
            set
            {
                this._authRuleCategoryId = value;
            }
        }

        /// <summary>
        /// Get or Set the Auth Rule Category Name information.
        /// </summary>
        public string AuthRuleCategoryName
        {
            get
            {
                return this._authRuleCategoryName;
            }
            set
            {
                this._authRuleCategoryName = value;
            }
        }

        /// <summary>
        /// Get or Set theAuth Rule Sub Category Name information.
        /// </summary>
        public string AuthRuleSubCategoryName
        {
            get
            {
                return this._authRuleSubCategoryName;
            }
            set
            {
                this._authRuleSubCategoryName = value;
            }
        }

        /// <summary>
        /// Get or Set theAuth Rule Sub Category Name information.
        /// </summary>
        public string AuthRuleName
        {
            get
            {
                return this._authRuleName;
            }
            set
            {
                this._authRuleName = value;
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

        public List<int> ClientDepartmentList
        {
            get
            {
                return this._clientDepartmentList;
            }
            set
            {
                this._clientDepartmentList = value;
            }
        }

        public List<int> AuthRuleList
        {
            get
            {
                return this._authRuleList;
            }
            set
            {
                this._authRuleList = value;
            }
        }

        /// <summary>
        /// Find if the donor already have a test
        /// </summary>
        public string ProgramExists
        {
            get
            {
                return this._programExists;
            }
            set
            {
                this._programExists = value;
            }
        }

        #endregion Public Properties
    }
}