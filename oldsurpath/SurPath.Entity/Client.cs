using SurPath.Enum;
using System;
using System.Collections.Generic;

namespace SurPath.Entity
{
    /// <summary>
    /// Client entity
    /// </summary>
    public class Client
    {
        #region Private Variables

        private int _clientId;

        private string _clientName;
        private string _clientDivision;
        private ClientTypes _clientTypeId;

        private bool _isMailingAddressPhysical;
        private bool _isClientActive;
        private bool _canEditTestCategory;
        private int? _salesRepresentativeId;
        private double? _salesComissions;
        private string _clientCode;
        private int? _laboratoryVendorId;
        private int? _mroVendorId;
        private ClientMROTypes _mroTypeId;

        private ClientContact _clientContact = new ClientContact();
        private List<ClientAddress> _clientAddresses = new List<ClientAddress>();

        private List<ClientDepartment> _clientDepartments = new List<ClientDepartment>();

        private string _clientCity;
        private string _clientState;
        private string _mainContact;
        private string _clientPhone;
        private string _clientFax;
        private string _clientEmail;

        private bool _isSynchronized;
        private bool _isArchived;
        private DateTime _createdOn;
        private string _createdBy;
        private DateTime _lastModifiedOn;
        private string _lastModifiedBy;

        #endregion Private Variables

        #region Public Properties

        /// <summary>
        /// Get or Set the Client value.
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
        /// Get or Set the Client Name.
        /// </summary>
        public string ClientName
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
        /// Get or Set the Client Division.
        /// </summary>
        public string ClientDivision
        {
            get
            {
                return this._clientDivision;
            }
            set
            {
                this._clientDivision = value;
            }
        }

        /// <summary>
        /// Get or Set the Client Type Id.
        /// </summary>
        public ClientTypes ClientTypeId
        {
            get
            {
                return this._clientTypeId;
            }
            set
            {
                this._clientTypeId = value;
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
        /// Get or Set the Client active status.
        /// </summary>
        public bool IsClientActive
        {
            get
            {
                return this._isClientActive;
            }
            set
            {
                this._isClientActive = value;
            }
        }

        /// <summary>
        /// Get or Set the Can Edit TestCategory
        /// </summary>
        public bool CanEditTestCategory
        {
            get
            {
                return this._canEditTestCategory;
            }
            set
            {
                this._canEditTestCategory = value;
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
        /// Get or Set the Client Code.
        /// </summary>
        public string ClientCode
        {
            get
            {
                return this._clientCode;
            }
            set
            {
                this._clientCode = value;
            }
        }

        /// <summary>
        /// Get or Set the Laboratory Vendor Id.
        /// </summary>
        public int? LaboratoryVendorId
        {
            get
            {
                return this._laboratoryVendorId;
            }
            set
            {
                this._laboratoryVendorId = value;
            }
        }

        /// <summary>
        /// Get or Set the MRO Vendor Id.
        /// </summary>
        public int? MROVendorId
        {
            get
            {
                return this._mroVendorId;
            }
            set
            {
                this._mroVendorId = value;
            }
        }

        /// <summary>
        /// Get or Set the Client MRO Type.
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
        /// Get or Set the Client Department information.
        /// </summary>
        public List<ClientDepartment> ClientDepartments
        {
            get
            {
                return this._clientDepartments;
            }
            set
            {
                this._clientDepartments = value;
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

        public string client_timezoneinfo { get; set; }


        public bool client_wide = false;
        public bool IntegrationPartner = false;
        public bool require_login = false;
        public bool require_remote_login = false;
        public int backend_integration_partner_id = 0;
        public int backend_integration_partner_client_map_id = 0;
        public string login_url = string.Empty;

        #endregion Public Properties
    }
}