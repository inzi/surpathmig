using SurPath.Enum;
using System;
using System.Collections.Generic;

namespace SurPath.Entity
{
    /// <summary>
    /// Client Department entity
    /// </summary>
    public class ClientDepartment
    {
        #region Private Variables

        private int _clientDepartmentId;
        private string _fullLabCode;
        private int _clientId;
        private string _departmentName;
        private ClientMROTypes _mroTypeId;
        private ClientPaymentTypes _paymentTypeId;
        private bool _isDepartmentActive;

        private List<ClientDeptTestCategory> _clientDeptTestCategories = new List<ClientDeptTestCategory>();

        private bool _isPhysicalAddressAsClient;
        private bool _isMailingAddressPhysical;
        private int? _salesRepresentativeId;
        private double? _salesComissions;
        private ClientContact _clientContact = new ClientContact();
        private List<ClientAddress> _clientAddresses = new List<ClientAddress>();
        private string _clientCity;
        private string _clientState;
        private string _mainContact;
        private string _clientPhone;
        private string _clientFax;
        private string _clientEmail;

        private bool _isUA;
        private bool _isHair;
        private bool _isDNA;
        private bool _isRecordKeeping;
        private bool _isBC;
        private bool _isSynchronized;
        private bool _isArchived;
        private DateTime _createdOn;
        private string _createdBy;
        private DateTime _lastModifiedOn;
        private string _lastModifiedBy;

        public string _labCode;
        public string _questCode;
        public string _clearStarCode;
        public string _formfoxCode = string.Empty;

        private int _reason_for_test_default = 2;

        #endregion Private Variables

        #region Public Properties
        public bool integrationPartner { get; set; } = false;
        public bool requireLogin { get; set; } = false;
        public bool require_remote_login { get; set; } = false;
        public int backend_integration_partner_id { get; set; } = 0;
        public int backend_integration_partner_client_map_id { get; set; } = 0;
        public string login_url { get; set; } = string.Empty;
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
        /// Get or Set the Client Department Id.
        /// </summary>
        public string FullLabCode
        {
            get
            {
                return this._fullLabCode;
            }
            set
            {
                this._fullLabCode = value;
            }
        }

        public int reason_for_test_default
        {
            get
            {
                return this._reason_for_test_default;
            }
            set
            {
                this._reason_for_test_default = value;
            }
        }

        /// <summary>
        /// Get or Set the Client Id.
        /// </summary>
        public int ClientId
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
        /// Get or Set the Department Name.
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
        /// Get or Set the MRO Type Id.
        /// </summary>
        public ClientMROTypes MROTypeId
        {
            get
            {
                return this._mroTypeId;
            }
            set
            {
                this._mroTypeId = value;
            }
        }

        /// <summary>
        /// Get or Set the Client Payment Type Id.
        /// </summary>
        public ClientPaymentTypes PaymentTypeId
        {
            get
            {
                return this._paymentTypeId;
            }
            set
            {
                this._paymentTypeId = value;
            }
        }

        /// <summary>
        /// Get or Set the data Department status.
        /// </summary>
        public bool IsDepartmentActive
        {
            get
            {
                return this._isDepartmentActive;
            }
            set
            {
                this._isDepartmentActive = value;
            }
        }

        /// <summary>
        /// Get or Set the data Department Test Categories.
        /// </summary>
        public List<ClientDeptTestCategory> ClientDeptTestCategories
        {
            get
            {
                return this._clientDeptTestCategories;
            }
            set
            {
                this._clientDeptTestCategories = value;
            }
        }

        /// <summary>
        /// Get or Set the Is Physical Address same as Client.
        /// </summary>
        public bool IsPhysicalAddressAsClient
        {
            get
            {
                return this._isPhysicalAddressAsClient;
            }
            set
            {
                this._isPhysicalAddressAsClient = value;
            }
        }

        /// <summary>
        /// Get or Set the Is Mailing Address Physical.
        /// </summary>
        public bool IsMailingAddressPhysical
        {
            get
            {
                return this._isMailingAddressPhysical;
            }
            set
            {
                this._isMailingAddressPhysical = value;
            }
        }

        /// <summary>
        /// Get or Set the Sales Representative Id.
        /// </summary>
        public int? SalesRepresentativeId
        {
            get
            {
                return this._salesRepresentativeId;
            }
            set
            {
                this._salesRepresentativeId = value;
            }
        }

        /// <summary>
        /// Get or Set the Sales Comissions.
        /// </summary>
        public double? SalesComissions
        {
            get
            {
                return this._salesComissions;
            }
            set
            {
                this._salesComissions = value;
            }
        }

        /// <summary>
        /// Get or Set the Client Contact information.
        /// </summary>
        public ClientContact ClientContact
        {
            get
            {
                return this._clientContact;
            }
            set
            {
                this._clientContact = value;
            }
        }

        /// <summary>
        /// Get or Set the Client Address information.
        /// </summary>
        public List<ClientAddress> ClientAddresses
        {
            get
            {
                return this._clientAddresses;
            }
            set
            {
                this._clientAddresses = value;
            }
        }

        /// <summary>
        /// Get or Set the Client City.
        /// </summary>
        public string ClientCity
        {
            get
            {
                return this._clientCity;
            }
            set
            {
                this._clientCity = value;
            }
        }

        /// <summary>
        /// Get or Set the Client State.
        /// </summary>
        public string ClientState
        {
            get
            {
                return this._clientState;
            }
            set
            {
                this._clientState = value;
            }
        }

        /// <summary>
        /// Get or Set the Main Contact.
        /// </summary>
        public string MainContact
        {
            get
            {
                return this._mainContact;
            }
            set
            {
                this._mainContact = value;
            }
        }

        /// <summary>
        /// Get or Set the Client Phone.
        /// </summary>
        public string ClientPhone
        {
            get
            {
                return this._clientPhone;
            }
            set
            {
                this._clientPhone = value;
            }
        }

        /// <summary>
        /// Get or Set the Client Fax.
        /// </summary>
        public string ClientFax
        {
            get
            {
                return this._clientFax;
            }
            set
            {
                this._clientFax = value;
            }
        }

        /// <summary>
        /// Get or Set the Client Email.
        /// </summary>
        public string ClientEmail
        {
            get
            {
                return this._clientEmail;
            }
            set
            {
                this._clientEmail = value;
            }
        }

        /// <summary>
        /// Get or Set the UA Category available or not.
        /// </summary>
        public bool IsUA
        {
            get
            {
                return this._isUA;
            }
            set
            {
                this._isUA = value;
            }
        }

        /// <summary>
        /// Get or Set the Hair Category available or not.
        /// </summary>
        public bool IsHair
        {
            get
            {
                return this._isHair;
            }
            set
            {
                this._isHair = value;
            }
        }

        /// <summary>
        /// Get or Set the DNA
        /// </summary>
        public bool IsDNA
        {
            get
            {
                return this._isDNA;
            }
            set
            {
                this._isDNA = value;
            }
        }

        /// <summary>
        /// Get or Set the Recordkeeping
        /// </summary>
        public bool IsRecordKeeping
        {
            get
            {
                return this._isRecordKeeping;
            }
            set
            {
                this._isRecordKeeping = value;
            }
        }

        /// <summary>
        /// Get or Set the Background Check.
        /// </summary>
        public bool IsBC
        {
            get
            {
                return this._isBC;
            }
            set
            {
                this._isBC = value;
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

        public string LabCode
        {
            get
            {
                return this._labCode;
            }
            set
            {
                this._labCode = value;
            }
        }

        public string QuestCode
        {
            get
            {
                return this._questCode;
            }
            set
            {
                this._questCode = value;
            }
        }

        public string ClearStarCode
        {
            get
            {
                return this._clearStarCode;
            }
            set
            {
                this._clearStarCode = value;
            }
        }

        public string FormFoxCode {
            get
            {
                return this._formfoxCode;
            }
            set
            {
                this._formfoxCode = value;
            }
        }

        #endregion Public Properties
    }
}