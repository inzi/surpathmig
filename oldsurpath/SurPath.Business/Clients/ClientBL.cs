using SurPath.Data;
using SurPath.Entity;
using SurPath.Enum;
using System;
using System.Collections.Generic;
using System.Data;

namespace SurPath.Business
{
    public class ClientBL : BusinessObject
    {
        private ClientDao clientDao = new ClientDao();

        /// <summary>
        /// Save the Client information to the database.
        /// </summary>
        /// <param name="client">Client information which one need to be added to the database.</param>
        /// <returns>Returns ClientId, the auto increament value.</returns>
        public int Save(Client client)
        {
            if (client.ClientId == 0)
            {
                return clientDao.Insert(client);
            }
            else
            {
                return clientDao.Update(client);
            }
        }

        /// <summary>
        /// Deletes the Client information from database.
        /// </summary>
        /// <param name="clientId">ClientId to be deleted.</param>
        /// <param name="currentUserName">Current username who is deleting the record.</param>
        /// <returns>Returns number of records deleted from the database.</returns>
        public int Delete(int clientId, string currentUserName)
        {
            return clientDao.Delete(clientId, currentUserName);
        }

        /// <summary>
        /// Get the User information by ClientId
        /// </summary>
        /// <param name="clientId">ClientId which one need to be get from the database.</param>
        /// <returns>Returns Client information.</returns>
        public Client Get(int clientId)
        {
            try
            {
                Client client = clientDao.Get(clientId);

                if (client != null)
                {
                    client.ClientDepartments = GetClientDepartmentList(client.ClientId);
                }

                return client;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get the User information by ClientId
        /// </summary>
        /// <param name="clientCode">Client Code which one need to be get from the database.</param>
        /// <returns>Returns Client information.</returns>
        public Client Get(string clientCode)
        {
            try
            {
                Client client = clientDao.Get(clientCode);

                if (client != null)
                {
                    client.ClientDepartments = GetClientDepartmentList(client.ClientId);
                }

                return client;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get all the Client informations.
        /// </summary>
        /// <returns>Returns Client information list.</returns>
        public List<Client> GetList(string getInActive = null)
        {
            try
            {
                DataTable dtClients = clientDao.GetList(getInActive);

                List<Client> clientList = new List<Client>();

                foreach (DataRow dr in dtClients.Rows)
                {
                    Client client = new Client();

                    client.ClientId = (int)dr["ClientId"];
                    client.ClientCode = (string)dr["ClientCode"];
                    client.ClientName = (string)dr["ClientName"];
                    client.ClientDivision = dr["ClientDivision"].ToString();
                    client.ClientTypeId = (ClientTypes)dr["ClientTypeId"];
                    client.LaboratoryVendorId = dr["LaboratoryVendorId"].ToString() != string.Empty ? (int?)dr["LaboratoryVendorId"] : null;
                    client.MROVendorId = dr["MROVendorId"].ToString() != string.Empty ? (int?)dr["MROVendorId"] : null;
                    client.MROTypeId = (ClientMROTypes)dr["MROTypeId"];
                    client.IsMailingAddressPhysical = dr["IsMailingAddressPhysical"].ToString() == "1" ? true : false;
                    client.IsClientActive = dr["IsClientActive"].ToString() == "1" ? true : false;
                    client.SalesRepresentativeId = dr["SalesRepresentativeId"].ToString() != string.Empty ? (int?)dr["SalesRepresentativeId"] : null;
                    client.SalesComissions = dr["SalesComissions"].ToString() != string.Empty ? (double?)dr["SalesComissions"] : null;
                    client.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    client.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    client.CreatedOn = (DateTime)dr["CreatedOn"];
                    client.CreatedBy = (string)dr["CreatedBy"];
                    client.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    client.LastModifiedBy = (string)dr["LastModifiedBy"];

                    DataTable dtClientContact = clientDao.GetClientContatListByClientId(client.ClientId);

                    if (dtClientContact != null)
                    {
                        if (dtClientContact.Rows.Count == 1)
                        {
                            client.ClientContact = new ClientContact();

                            client.ClientContact.ClientContactId = (int)dtClientContact.Rows[0]["ClientContactId"];
                            client.ClientContact.ClientId = (int)dtClientContact.Rows[0]["ClientId"];
                            client.ClientContact.ClientContactFirstName = (string)dtClientContact.Rows[0]["ClientContactFirstName"];
                            client.ClientContact.ClientContactLastName = (string)dtClientContact.Rows[0]["ClientContactLastName"];
                            client.ClientContact.ClientContactPhone = dtClientContact.Rows[0]["ClientContactPhone"].ToString();
                            client.ClientContact.ClientContactFax = dtClientContact.Rows[0]["ClientContactFax"].ToString();
                            client.ClientContact.ClientContactEmail = dtClientContact.Rows[0]["ClientContactEmail"].ToString();
                            client.ClientContact.IsSynchronized = dtClientContact.Rows[0]["IsSynchronized"].ToString() == "1" ? true : false;
                            client.ClientContact.CreatedOn = (DateTime)dtClientContact.Rows[0]["CreatedOn"];
                            client.ClientContact.CreatedBy = (string)dtClientContact.Rows[0]["CreatedBy"];
                            client.ClientContact.LastModifiedOn = (DateTime)dtClientContact.Rows[0]["LastModifiedOn"];
                            client.ClientContact.LastModifiedBy = (string)dtClientContact.Rows[0]["LastModifiedBy"];

                            client.MainContact = client.ClientContact.ClientContactFirstName + " " + client.ClientContact.ClientContactLastName;
                            client.ClientPhone = client.ClientContact.ClientContactPhone;
                            client.ClientFax = client.ClientContact.ClientContactFax;
                            client.ClientEmail = client.ClientContact.ClientContactEmail;
                        }
                    }

                    DataTable dtClientAddresses = clientDao.GetClientAddressListByClientId(client.ClientId);

                    if (dtClientAddresses != null)
                    {
                        foreach (DataRow drAddress in dtClientAddresses.Rows)
                        {
                            ClientAddress address = new ClientAddress();

                            address.AddressId = (int)drAddress["ClientAddressId"];
                            address.ClientId = (int)drAddress["ClientId"];
                            address.AddressTypeId = (AddressTypes)drAddress["AddressTypeId"];
                            address.Address1 = (string)drAddress["ClientAddress1"];
                            address.Address2 = drAddress["ClientAddress2"].ToString();
                            address.City = (string)drAddress["ClientCity"];
                            address.State = (string)drAddress["ClientState"];
                            address.ZipCode = (string)drAddress["ClientZip"];
                            address.IsSynchronized = drAddress["IsSynchronized"].ToString() == "1" ? true : false;
                            address.CreatedOn = (DateTime)drAddress["CreatedOn"];
                            address.CreatedBy = (string)drAddress["CreatedBy"];
                            address.LastModifiedOn = (DateTime)drAddress["LastModifiedOn"];
                            address.LastModifiedBy = (string)drAddress["LastModifiedBy"];

                            client.ClientAddresses.Add(address);

                            if (address.AddressTypeId == AddressTypes.PhysicalAddress1)
                            {
                                client.ClientCity = address.City;
                                client.ClientState = address.State;
                            }
                        }
                    }

                    clientList.Add(client);
                }

                return clientList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get all the Client informations.
        /// </summary>
        /// <returns>Returns Client information list.</returns>
        public DataTable GetListDT()
        {
            try
            {
                DataTable dtClients = clientDao.GetList();

                return dtClients;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get all the Client informations.
        /// </summary>
        /// <returns>Returns Client information list.</returns>
        public DataTable GetListDT(int currentUserId)
        {
            try
            {
                DataTable dtClients = clientDao.GetList(currentUserId);

                return dtClients;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get all the Client informations based on the search criteria.
        /// </summary>
        /// <param name="searchParam">Collection of search criteria</param>
        /// <returns>Returns Client information list.</returns>
        public List<Client> GetList(Dictionary<string, string> searchParam)
        {
            try
            {
                DataTable dtClients = clientDao.GetList(searchParam);

                List<Client> clientList = new List<Client>();

                foreach (DataRow dr in dtClients.Rows)
                {
                    Client client = new Client();

                    client.ClientId = (int)dr["ClientId"];
                    client.ClientCode = (string)dr["ClientCode"];
                    client.ClientName = (string)dr["ClientName"];
                    client.ClientDivision = dr["ClientDivision"].ToString();
                    client.ClientTypeId = (ClientTypes)dr["ClientTypeId"];
                    client.LaboratoryVendorId = dr["LaboratoryVendorId"].ToString() != string.Empty ? (int?)dr["LaboratoryVendorId"] : null;
                    client.MROVendorId = dr["MROVendorId"].ToString() != string.Empty ? (int?)dr["MROVendorId"] : null;
                    client.MROTypeId = (ClientMROTypes)dr["MROTypeId"];
                    client.IsMailingAddressPhysical = dr["IsMailingAddressPhysical"].ToString() == "1" ? true : false;
                    client.IsClientActive = dr["IsClientActive"].ToString() == "1" ? true : false;
                    client.SalesRepresentativeId = dr["SalesRepresentativeId"].ToString() != string.Empty ? (int?)dr["SalesRepresentativeId"] : null;
                    client.SalesComissions = dr["SalesComissions"].ToString() != string.Empty ? (double?)dr["SalesComissions"] : null;
                    client.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    client.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    client.CreatedOn = (DateTime)dr["CreatedOn"];
                    client.CreatedBy = (string)dr["CreatedBy"];
                    client.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    client.LastModifiedBy = (string)dr["LastModifiedBy"];

                    DataTable dtClientContact = clientDao.GetClientContatListByClientId(client.ClientId);

                    if (dtClientContact != null)
                    {
                        if (dtClientContact.Rows.Count == 1)
                        {
                            client.ClientContact = new ClientContact();

                            client.ClientContact.ClientContactId = (int)dtClientContact.Rows[0]["ClientContactId"];
                            client.ClientContact.ClientId = (int)dtClientContact.Rows[0]["ClientId"];
                            client.ClientContact.ClientContactFirstName = (string)dtClientContact.Rows[0]["ClientContactFirstName"];
                            client.ClientContact.ClientContactLastName = (string)dtClientContact.Rows[0]["ClientContactLastName"];
                            client.ClientContact.ClientContactPhone = dtClientContact.Rows[0]["ClientContactPhone"].ToString();
                            client.ClientContact.ClientContactFax = dtClientContact.Rows[0]["ClientContactFax"].ToString();
                            client.ClientContact.ClientContactEmail = dtClientContact.Rows[0]["ClientContactEmail"].ToString();
                            client.ClientContact.IsSynchronized = dtClientContact.Rows[0]["IsSynchronized"].ToString() == "1" ? true : false;
                            client.ClientContact.CreatedOn = (DateTime)dtClientContact.Rows[0]["CreatedOn"];
                            client.ClientContact.CreatedBy = (string)dtClientContact.Rows[0]["CreatedBy"];
                            client.ClientContact.LastModifiedOn = (DateTime)dtClientContact.Rows[0]["LastModifiedOn"];
                            client.ClientContact.LastModifiedBy = (string)dtClientContact.Rows[0]["LastModifiedBy"];

                            client.MainContact = client.ClientContact.ClientContactFirstName + " " + client.ClientContact.ClientContactLastName;
                            client.ClientPhone = client.ClientContact.ClientContactPhone;
                            client.ClientFax = client.ClientContact.ClientContactFax;
                            client.ClientEmail = client.ClientContact.ClientContactEmail;
                        }
                    }

                    DataTable dtClientAddresses = clientDao.GetClientAddressListByClientId(client.ClientId);

                    if (dtClientAddresses != null)
                    {
                        foreach (DataRow drAddress in dtClientAddresses.Rows)
                        {
                            ClientAddress address = new ClientAddress();

                            address.AddressId = (int)drAddress["ClientAddressId"];
                            address.ClientId = (int)drAddress["ClientId"];
                            address.AddressTypeId = (AddressTypes)drAddress["AddressTypeId"];
                            address.Address1 = (string)drAddress["ClientAddress1"];
                            address.Address2 = drAddress["ClientAddress2"].ToString();
                            address.City = (string)drAddress["ClientCity"];
                            address.State = (string)drAddress["ClientState"];
                            address.ZipCode = (string)drAddress["ClientZip"];
                            address.IsSynchronized = drAddress["IsSynchronized"].ToString() == "1" ? true : false;
                            address.CreatedOn = (DateTime)drAddress["CreatedOn"];
                            address.CreatedBy = (string)drAddress["CreatedBy"];
                            address.LastModifiedOn = (DateTime)drAddress["LastModifiedOn"];
                            address.LastModifiedBy = (string)drAddress["LastModifiedBy"];

                            client.ClientAddresses.Add(address);

                            if (address.AddressTypeId == AddressTypes.PhysicalAddress1)
                            {
                                client.ClientCity = address.City;
                                client.ClientState = address.State;
                            }
                        }
                    }

                    clientList.Add(client);
                }
                return clientList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetByEmail(string Email)
        {
            try
            {
                DataTable client = clientDao.GetByEmail(Email);

                return client;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Client> Sorting(string searchparam, bool active, string getInActive = null)
        {
            try
            {
                DataTable dtClient = clientDao.sorting(searchparam, active, getInActive);

                List<Client> clientList = new List<Client>();

                foreach (DataRow dr in dtClient.Rows)
                {
                    Client client = new Client();

                    client.ClientId = (int)dr["ClientId"];
                    client.ClientCode = (string)dr["ClientCode"];
                    client.ClientName = (string)dr["ClientName"];
                    client.ClientDivision = dr["ClientDivision"].ToString();
                    client.ClientTypeId = (ClientTypes)dr["ClientTypeId"];
                    client.LaboratoryVendorId = dr["LaboratoryVendorId"].ToString() != string.Empty ? (int?)dr["LaboratoryVendorId"] : null;
                    client.MROVendorId = dr["MROVendorId"].ToString() != string.Empty ? (int?)dr["MROVendorId"] : null;
                    client.MROTypeId = (ClientMROTypes)dr["MROTypeId"];
                    client.IsMailingAddressPhysical = dr["IsMailingAddressPhysical"].ToString() == "1" ? true : false;
                    client.IsClientActive = dr["IsClientActive"].ToString() == "1" ? true : false;
                    client.SalesRepresentativeId = dr["SalesRepresentativeId"].ToString() != string.Empty ? (int?)dr["SalesRepresentativeId"] : null;
                    client.SalesComissions = dr["SalesComissions"].ToString() != string.Empty ? (double?)dr["SalesComissions"] : null;
                    client.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    client.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    client.CreatedOn = (DateTime)dr["CreatedOn"];
                    client.CreatedBy = (string)dr["CreatedBy"];
                    client.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    client.LastModifiedBy = (string)dr["LastModifiedBy"];

                    DataTable dtClientContact = clientDao.GetClientContatListByClientId(client.ClientId);

                    if (dtClientContact != null)
                    {
                        if (dtClientContact.Rows.Count == 1)
                        {
                            client.ClientContact = new ClientContact();

                            client.ClientContact.ClientContactId = (int)dtClientContact.Rows[0]["ClientContactId"];
                            client.ClientContact.ClientId = (int)dtClientContact.Rows[0]["ClientId"];
                            client.ClientContact.ClientContactFirstName = (string)dtClientContact.Rows[0]["ClientContactFirstName"];
                            client.ClientContact.ClientContactLastName = (string)dtClientContact.Rows[0]["ClientContactLastName"];
                            client.ClientContact.ClientContactPhone = dtClientContact.Rows[0]["ClientContactPhone"].ToString();
                            client.ClientContact.ClientContactFax = dtClientContact.Rows[0]["ClientContactFax"].ToString();
                            client.ClientContact.ClientContactEmail = dtClientContact.Rows[0]["ClientContactEmail"].ToString();
                            client.ClientContact.IsSynchronized = dtClientContact.Rows[0]["IsSynchronized"].ToString() == "1" ? true : false;
                            client.ClientContact.CreatedOn = (DateTime)dtClientContact.Rows[0]["CreatedOn"];
                            client.ClientContact.CreatedBy = (string)dtClientContact.Rows[0]["CreatedBy"];
                            client.ClientContact.LastModifiedOn = (DateTime)dtClientContact.Rows[0]["LastModifiedOn"];
                            client.ClientContact.LastModifiedBy = (string)dtClientContact.Rows[0]["LastModifiedBy"];

                            client.MainContact = client.ClientContact.ClientContactFirstName + " " + client.ClientContact.ClientContactLastName;
                            client.ClientPhone = client.ClientContact.ClientContactPhone;
                            client.ClientFax = client.ClientContact.ClientContactFax;
                            client.ClientEmail = client.ClientContact.ClientContactEmail;
                        }
                    }

                    DataTable dtClientAddresses = clientDao.GetClientAddressListByClientId(client.ClientId);

                    if (dtClientAddresses != null)
                    {
                        foreach (DataRow drAddress in dtClientAddresses.Rows)
                        {
                            ClientAddress address = new ClientAddress();

                            address.AddressId = (int)drAddress["ClientAddressId"];
                            address.ClientId = (int)drAddress["ClientId"];
                            address.AddressTypeId = (AddressTypes)drAddress["AddressTypeId"];
                            address.Address1 = (string)drAddress["ClientAddress1"];
                            address.Address2 = drAddress["ClientAddress2"].ToString();
                            address.City = (string)drAddress["ClientCity"];
                            address.State = (string)drAddress["ClientState"];
                            address.ZipCode = (string)drAddress["ClientZip"];
                            address.IsSynchronized = drAddress["IsSynchronized"].ToString() == "1" ? true : false;
                            address.CreatedOn = (DateTime)drAddress["CreatedOn"];
                            address.CreatedBy = (string)drAddress["CreatedBy"];
                            address.LastModifiedOn = (DateTime)drAddress["LastModifiedOn"];
                            address.LastModifiedBy = (string)drAddress["LastModifiedBy"];

                            client.ClientAddresses.Add(address);

                            if (address.AddressTypeId == AddressTypes.PhysicalAddress1)
                            {
                                client.ClientCity = address.City;
                                client.ClientState = address.State;
                            }
                        }
                    }

                    clientList.Add(client);
                }
                return clientList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Save the Client Department to the database.
        /// </summary>
        /// <param name="client">Client Department which one need to be added to the database.</param>
        /// <returns>Returns ClientDepartmentId, the auto increament value.</returns>
        public int SaveClientDepartment(ClientDepartment clientDepartment)
        {
            if (clientDepartment.ClientDepartmentId == 0)
            {
                return clientDao.InsertClientDepartment(clientDepartment);
            }
            else
            {
                return clientDao.UpdateClientDepartment(clientDepartment);
            }
        }

        public int SaveClientDocType(ClientDocTypes dt)
        {
            if (dt.ClientDoctypeId == 0)
            {
                return clientDao.InsertClientDepartmentDocType(dt);
            }
            else
            {
                return clientDao.UpdateClientDocumentType(dt);
            }
        }

        public int DeleteClientDepartment(int clientDepartmentId, string currentUserName)
        {
            return clientDao.DeleteClientDepartment(clientDepartmentId, currentUserName);
        }

        public ClientDepartment GetClientDepartment(int clientDepartmentId)
        {
            try
            {
                ClientDepartment clientDepartment = clientDao.GetClientDepartment(clientDepartmentId);

                if (clientDepartment != null)
                {
                    DataTable dtClientDepartmentCategories = clientDao.GetClientDepartmentTestCategories(clientDepartment.ClientDepartmentId);

                    if (dtClientDepartmentCategories != null)
                    {
                        foreach (DataRow drClientDeptTestCategory in dtClientDepartmentCategories.Rows)
                        {
                            ClientDeptTestCategory clientDeptTestCategory = new ClientDeptTestCategory();

                            clientDeptTestCategory.ClientDeptTestCategoryId = (int)drClientDeptTestCategory["ClientDeptTestCategoryId"];
                            clientDeptTestCategory.ClientDepartmentId = (int)drClientDeptTestCategory["ClientDepartmentId"];
                            clientDeptTestCategory.TestCategoryId = (TestCategories)drClientDeptTestCategory["TestCategoryId"];
                            clientDeptTestCategory.DisplayOrder = (int)drClientDeptTestCategory["DisplayOrder"];
                            clientDeptTestCategory.TestPanelPrice = drClientDeptTestCategory["TestPanelPrice"].ToString() != string.Empty ? (double?)drClientDeptTestCategory["TestPanelPrice"] : null;
                            clientDeptTestCategory.IsSynchronized = drClientDeptTestCategory["IsSynchronized"].ToString() == "1" ? true : false;
                            clientDeptTestCategory.CreatedOn = (DateTime)drClientDeptTestCategory["CreatedOn"];
                            clientDeptTestCategory.CreatedBy = (string)drClientDeptTestCategory["CreatedBy"];
                            clientDeptTestCategory.LastModifiedOn = (DateTime)drClientDeptTestCategory["LastModifiedOn"];
                            clientDeptTestCategory.LastModifiedBy = (string)drClientDeptTestCategory["LastModifiedBy"];

                            DataTable dtClientDeptTestPanels = clientDao.GetClientDepartmentTestPanels(clientDeptTestCategory.ClientDeptTestCategoryId);

                            foreach (DataRow drClientDeptTestPanel in dtClientDeptTestPanels.Rows)
                            {
                                ClientDeptTestPanel clientDeptTestPanel = new ClientDeptTestPanel();

                                clientDeptTestPanel.ClientDeptTestPanelId = (int)drClientDeptTestPanel["ClientDeptTestPanelId"];
                                clientDeptTestPanel.ClientDeptTestCategoryId = (int)drClientDeptTestPanel["ClientDeptTestCategoryId"];
                                clientDeptTestPanel.TestPanelId = (int)drClientDeptTestPanel["TestPanelId"];
                                clientDeptTestPanel.TestPanelName = (string)drClientDeptTestPanel["TestPanelName"];
                                clientDeptTestPanel.TestPanelPrice = (double)drClientDeptTestPanel["TestPanelPrice"];
                                clientDeptTestPanel.DisplayOrder = (int)drClientDeptTestPanel["DisplayOrder"];
                                clientDeptTestPanel.IsMainTestPanel = drClientDeptTestPanel["IsMainTestPanel"].ToString() == "1" ? true : false;
                                clientDeptTestPanel.Is1TestPanel = drClientDeptTestPanel["Is1TestPanel"].ToString() == "1" ? true : false;
                                clientDeptTestPanel.Is2TestPanel = drClientDeptTestPanel["Is2TestPanel"].ToString() == "1" ? true : false;
                                clientDeptTestPanel.Is3TestPanel = drClientDeptTestPanel["Is3TestPanel"].ToString() == "1" ? true : false;
                                clientDeptTestPanel.Is4TestPanel = drClientDeptTestPanel["Is4TestPanel"].ToString() == "1" ? true : false;
                                clientDeptTestPanel.IsSynchronized = drClientDeptTestPanel["IsSynchronized"].ToString() == "1" ? true : false;
                                clientDeptTestPanel.CreatedOn = (DateTime)drClientDeptTestPanel["CreatedOn"];
                                clientDeptTestPanel.CreatedBy = (string)drClientDeptTestPanel["CreatedBy"];
                                clientDeptTestPanel.LastModifiedOn = (DateTime)drClientDeptTestPanel["LastModifiedOn"];
                                clientDeptTestPanel.LastModifiedBy = (string)drClientDeptTestPanel["LastModifiedBy"];

                                clientDeptTestCategory.ClientDeptTestPanels.Add(clientDeptTestPanel);
                            }

                            clientDepartment.ClientDeptTestCategories.Add(clientDeptTestCategory);

                            if (drClientDeptTestCategory["TestCategoryId"].ToString() == ((int)TestCategories.UA).ToString())
                            {
                                clientDepartment.IsUA = true;
                            }
                            else if (drClientDeptTestCategory["TestCategoryId"].ToString() == ((int)TestCategories.Hair).ToString())
                            {
                                clientDepartment.IsHair = true;
                            }
                            else if (drClientDeptTestCategory["TestCategoryId"].ToString() == ((int)TestCategories.DNA).ToString())
                            {
                                clientDepartment.IsDNA = true;
                            }
                            else if (drClientDeptTestCategory["TestCategoryId"].ToString() == ((int)TestCategories.RC).ToString())
                            {
                                clientDepartment.IsRecordKeeping = true;
                            }
                            else if (drClientDeptTestCategory["TestCategoryId"].ToString() == ((int)TestCategories.BC).ToString())
                            {
                                clientDepartment.IsBC = true;
                            }
                        }
                    }
                    else
                    {
                        clientDepartment.IsUA = false;
                        clientDepartment.IsHair = false;
                        clientDepartment.IsDNA = false;
                        clientDepartment.IsBC = false;
                        clientDepartment.IsRecordKeeping = false;
                    }
                }

                return clientDepartment;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ClientDepartment> GetClientDepartmentList(int clientId, string getInActive = null)
        {
            try
            {
                DataTable dtClientDepartment = clientDao.GetClientDepartmentList(clientId, getInActive);

                List<ClientDepartment> clientDepartmentList = new List<ClientDepartment>();

                foreach (DataRow drClientDepartment in dtClientDepartment.Rows)
                {
                    ClientDepartment clientDepartment = new ClientDepartment();

                    clientDepartment.ClientDepartmentId = (int)drClientDepartment["ClientDepartmentId"];
                    clientDepartment.ClientId = (int)drClientDepartment["ClientId"];
                    clientDepartment.DepartmentName = drClientDepartment["DepartmentName"].ToString();
                    clientDepartment.LabCode = drClientDepartment["LabCode"].ToString();
                    clientDepartment.MROTypeId = (ClientMROTypes)((int)drClientDepartment["MROTypeId"]);
                    clientDepartment.PaymentTypeId = (ClientPaymentTypes)drClientDepartment["PaymentTypeId"];
                    clientDepartment.IsMailingAddressPhysical = drClientDepartment["IsMailingAddressPhysical"].ToString() == "1" ? true : false;
                    clientDepartment.SalesRepresentativeId = drClientDepartment["SalesRepresentativeId"].ToString() != string.Empty ? (int?)drClientDepartment["SalesRepresentativeId"] : null;
                    clientDepartment.SalesComissions = drClientDepartment["SalesComissions"].ToString() != string.Empty ? (double?)drClientDepartment["SalesComissions"] : null;
                    clientDepartment.IsDepartmentActive = drClientDepartment["IsDepartmentActive"].ToString() == "1" ? true : false;
                    clientDepartment.IsSynchronized = drClientDepartment["IsSynchronized"].ToString() == "1" ? true : false;
                    clientDepartment.IsArchived = drClientDepartment["IsArchived"].ToString() == "1" ? true : false;
                    clientDepartment.CreatedOn = (DateTime)drClientDepartment["CreatedOn"];
                    clientDepartment.CreatedBy = (string)drClientDepartment["CreatedBy"];
                    clientDepartment.LastModifiedOn = (DateTime)drClientDepartment["LastModifiedOn"];
                    clientDepartment.LastModifiedBy = (string)drClientDepartment["LastModifiedBy"];

                    DataTable dtDeptContact = clientDao.GetClientDepartmentContact(clientDepartment.ClientDepartmentId);
                    if (dtDeptContact.Rows.Count == 1)
                    {
                        clientDepartment.ClientContact = new ClientContact();

                        clientDepartment.ClientContact.ClientContactId = (int)dtDeptContact.Rows[0]["ClientContactId"];
                        clientDepartment.ClientContact.ClientDepartmentId = (int)dtDeptContact.Rows[0]["ClientDepartmentId"];
                        clientDepartment.ClientContact.ClientContactFirstName = (string)dtDeptContact.Rows[0]["ClientContactFirstName"];
                        clientDepartment.ClientContact.ClientContactLastName = (string)dtDeptContact.Rows[0]["ClientContactLastName"];
                        clientDepartment.ClientContact.ClientContactPhone = dtDeptContact.Rows[0]["ClientContactPhone"].ToString();
                        clientDepartment.ClientContact.ClientContactFax = dtDeptContact.Rows[0]["ClientContactFax"].ToString();
                        clientDepartment.ClientContact.ClientContactEmail = dtDeptContact.Rows[0]["ClientContactEmail"].ToString();
                        clientDepartment.ClientContact.IsSynchronized = dtDeptContact.Rows[0]["IsSynchronized"].ToString() == "1" ? true : false;
                        clientDepartment.ClientContact.CreatedOn = (DateTime)dtDeptContact.Rows[0]["CreatedOn"];
                        clientDepartment.ClientContact.CreatedBy = (string)dtDeptContact.Rows[0]["CreatedBy"];
                        clientDepartment.ClientContact.LastModifiedOn = (DateTime)dtDeptContact.Rows[0]["LastModifiedOn"];
                        clientDepartment.ClientContact.LastModifiedBy = (string)dtDeptContact.Rows[0]["LastModifiedBy"];

                        clientDepartment.MainContact = clientDepartment.ClientContact.ClientContactFirstName + " " + clientDepartment.ClientContact.ClientContactLastName;
                        clientDepartment.ClientPhone = clientDepartment.ClientContact.ClientContactPhone;
                        clientDepartment.ClientFax = clientDepartment.ClientContact.ClientContactFax;
                        clientDepartment.ClientEmail = clientDepartment.ClientContact.ClientContactEmail;
                    }

                    DataTable dtDeptAddresses = clientDao.GetClientDepartmentAddresses(clientDepartment.ClientDepartmentId);
                    if (dtDeptAddresses.Rows.Count > 0)
                    {
                        foreach (DataRow drAddress in dtDeptAddresses.Rows)
                        {
                            ClientAddress address = new ClientAddress();

                            address.AddressId = (int)drAddress["ClientAddressId"];
                            address.ClientDepartmentId = (int)drAddress["ClientDepartmentId"];
                            address.AddressTypeId = (AddressTypes)drAddress["AddressTypeId"];
                            address.Address1 = (string)drAddress["ClientAddress1"];
                            address.Address2 = drAddress["ClientAddress2"].ToString();
                            address.City = (string)drAddress["ClientCity"];
                            address.State = (string)drAddress["ClientState"];
                            address.ZipCode = (string)drAddress["ClientZip"];
                            address.IsSynchronized = drAddress["IsSynchronized"].ToString() == "1" ? true : false;
                            address.CreatedOn = (DateTime)drAddress["CreatedOn"];
                            address.CreatedBy = (string)drAddress["CreatedBy"];
                            address.LastModifiedOn = (DateTime)drAddress["LastModifiedOn"];
                            address.LastModifiedBy = (string)drAddress["LastModifiedBy"];

                            clientDepartment.ClientAddresses.Add(address);

                            if (address.AddressTypeId == AddressTypes.PhysicalAddress1)
                            {
                                clientDepartment.ClientCity = address.City;
                                clientDepartment.ClientState = address.State;
                            }
                        }
                    }

                    DataTable dtClientDepartmentCategories = clientDao.GetClientDepartmentTestCategories(clientDepartment.ClientDepartmentId);

                    if (dtClientDepartmentCategories != null)
                    {
                        foreach (DataRow drClientDeptTestCategory in dtClientDepartmentCategories.Rows)
                        {
                            ClientDeptTestCategory clientDeptTestCategory = new ClientDeptTestCategory();

                            clientDeptTestCategory.ClientDeptTestCategoryId = (int)drClientDeptTestCategory["ClientDeptTestCategoryId"];
                            clientDeptTestCategory.ClientDepartmentId = (int)drClientDeptTestCategory["ClientDepartmentId"];
                            clientDeptTestCategory.TestCategoryId = (TestCategories)drClientDeptTestCategory["TestCategoryId"];
                            clientDeptTestCategory.DisplayOrder = (int)drClientDeptTestCategory["DisplayOrder"];
                            clientDeptTestCategory.TestPanelPrice = drClientDeptTestCategory["TestPanelPrice"].ToString() != string.Empty ? (double?)drClientDeptTestCategory["TestPanelPrice"] : null;
                            clientDeptTestCategory.IsSynchronized = drClientDeptTestCategory["IsSynchronized"].ToString() == "1" ? true : false;
                            clientDeptTestCategory.CreatedOn = (DateTime)drClientDeptTestCategory["CreatedOn"];
                            clientDeptTestCategory.CreatedBy = (string)drClientDeptTestCategory["CreatedBy"];
                            clientDeptTestCategory.LastModifiedOn = (DateTime)drClientDeptTestCategory["LastModifiedOn"];
                            clientDeptTestCategory.LastModifiedBy = (string)drClientDeptTestCategory["LastModifiedBy"];

                            DataTable dtClientDeptTestPanels = clientDao.GetClientDepartmentTestPanels(clientDeptTestCategory.ClientDeptTestCategoryId);

                            foreach (DataRow drClientDeptTestPanel in dtClientDeptTestPanels.Rows)
                            {
                                ClientDeptTestPanel clientDeptTestPanel = new ClientDeptTestPanel();

                                clientDeptTestPanel.ClientDeptTestPanelId = (int)drClientDeptTestPanel["ClientDeptTestPanelId"];
                                clientDeptTestPanel.ClientDeptTestCategoryId = (int)drClientDeptTestPanel["ClientDeptTestCategoryId"];
                                clientDeptTestPanel.TestPanelId = (int)drClientDeptTestPanel["TestPanelId"];
                                clientDeptTestPanel.TestPanelPrice = (double)drClientDeptTestPanel["TestPanelPrice"];
                                clientDeptTestPanel.DisplayOrder = (int)drClientDeptTestPanel["DisplayOrder"];
                                clientDeptTestPanel.IsMainTestPanel = drClientDeptTestPanel["IsMainTestPanel"].ToString() == "1" ? true : false;
                                clientDeptTestPanel.Is1TestPanel = drClientDeptTestPanel["Is1TestPanel"].ToString() == "1" ? true : false;
                                clientDeptTestPanel.Is2TestPanel = drClientDeptTestPanel["Is2TestPanel"].ToString() == "1" ? true : false;
                                clientDeptTestPanel.Is3TestPanel = drClientDeptTestPanel["Is3TestPanel"].ToString() == "1" ? true : false;
                                clientDeptTestPanel.Is4TestPanel = drClientDeptTestPanel["Is4TestPanel"].ToString() == "1" ? true : false;
                                clientDeptTestPanel.IsSynchronized = drClientDeptTestPanel["IsSynchronized"].ToString() == "1" ? true : false;
                                clientDeptTestPanel.CreatedOn = (DateTime)drClientDeptTestPanel["CreatedOn"];
                                clientDeptTestPanel.CreatedBy = (string)drClientDeptTestPanel["CreatedBy"];
                                clientDeptTestPanel.LastModifiedOn = (DateTime)drClientDeptTestPanel["LastModifiedOn"];
                                clientDeptTestPanel.LastModifiedBy = (string)drClientDeptTestPanel["LastModifiedBy"];

                                clientDeptTestCategory.ClientDeptTestPanels.Add(clientDeptTestPanel);
                            }

                            clientDepartment.ClientDeptTestCategories.Add(clientDeptTestCategory);

                            if (drClientDeptTestCategory["TestCategoryId"].ToString() == ((int)TestCategories.UA).ToString())
                            {
                                clientDepartment.IsUA = true;
                            }
                            else if (drClientDeptTestCategory["TestCategoryId"].ToString() == ((int)TestCategories.Hair).ToString())
                            {
                                clientDepartment.IsHair = true;
                            }
                            else if (drClientDeptTestCategory["TestCategoryId"].ToString() == ((int)TestCategories.DNA).ToString())
                            {
                                clientDepartment.IsDNA = true;
                            }
                            else if (drClientDeptTestCategory["TestCategoryId"].ToString() == ((int)TestCategories.RC).ToString())
                            {
                                clientDepartment.IsRecordKeeping = true;
                            }
                            else if (drClientDeptTestCategory["TestCategoryId"].ToString() == ((int)TestCategories.BC).ToString())
                            {
                                clientDepartment.IsBC = true;
                            }
                        }
                    }
                    else
                    {
                        clientDepartment.IsUA = false;
                        clientDepartment.IsHair = false;
                        clientDepartment.IsDNA = false;
                        clientDepartment.IsBC = false;
                        clientDepartment.IsRecordKeeping = false;
                    }

                    clientDepartmentList.Add(clientDepartment);
                }

                return clientDepartmentList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetClientDepartmentList()
        {
            try
            {
                DataTable dtClientDepartment = clientDao.GetClientDepartmentList();

                return dtClientDepartment;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetClientDepartmentListByUserId(int currentUserId)
        {
            try
            {
                DataTable dtClientDepartment = clientDao.GetClientDepartmentListByUserId(currentUserId);

                return dtClientDepartment;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get all the Client Department informations based on the search criteria.
        /// </summary>
        /// <param name="searchParam">Collection of search criteria</param>
        /// <returns>Returns Client Department information list.</returns>
        public List<ClientDepartment> GetClientDepartmentList(int clientId, Dictionary<string, string> searchParam)
        {
            try
            {
                DataTable dtClientDepartment = clientDao.GetClientDepartmentList(clientId, searchParam);

                List<ClientDepartment> clientDepartmentList = new List<ClientDepartment>();

                foreach (DataRow dr in dtClientDepartment.Rows)
                {
                    ClientDepartment clientDepartment = new ClientDepartment();

                    clientDepartment.ClientDepartmentId = (int)dr["ClientDepartmentId"];
                    clientDepartment.ClientId = (int)dr["ClientId"];
                    clientDepartment.DepartmentName = dr["DepartmentName"].ToString();
                    clientDepartment.LabCode = dr["LabCode"].ToString();

                    //try
                    //{
                    //    clientDepartment.QuestCode = dr["QuestCode"].ToString();
                    //}
                    //catch (Exception ex)
                    //{
                    //    clientDepartment.QuestCode = null;
                    //}

                    try
                    {
                        object value = dr["QuestCode"];
                        if (value != DBNull.Value)
                        {
                            clientDepartment.QuestCode = (string)dr["QuestCode"];
                        }
                    }
                    catch (Exception ex)
                    {
                        clientDepartment.QuestCode = string.Empty;
                    }

                    //try
                    //{
                    //    clientDepartment.ClearStarCode = dr["ClearStarCode"].ToString();
                    //}
                    //catch (Exception ex)
                    //{
                    //    clientDepartment.ClearStarCode = null;
                    //}

                    try
                    {
                        object value2 = dr["ClearStarCode"];
                        if (value2 != DBNull.Value)
                        {
                            clientDepartment.ClearStarCode = (string)dr["ClearStarCode"];
                        }
                    }
                    catch (Exception ex)
                    {
                        clientDepartment.ClearStarCode = string.Empty;
                    }

                    if (dr["FormFoxCode"] != DBNull.Value) clientDepartment.FormFoxCode = (string)dr["FormFoxCode"];

                    clientDepartment.MROTypeId = (ClientMROTypes)((int)dr["MROTypeId"]);
                    clientDepartment.PaymentTypeId = (ClientPaymentTypes)dr["PaymentTypeId"];
                    clientDepartment.IsMailingAddressPhysical = dr["IsMailingAddressPhysical"].ToString() == "1" ? true : false;
                    clientDepartment.SalesRepresentativeId = dr["SalesRepresentativeId"].ToString() != string.Empty ? (int?)dr["SalesRepresentativeId"] : null;
                    clientDepartment.SalesComissions = dr["SalesComissions"].ToString() != string.Empty ? (double?)dr["SalesComissions"] : null;
                    clientDepartment.IsDepartmentActive = dr["IsDepartmentActive"].ToString() == "1" ? true : false;
                    clientDepartment.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    clientDepartment.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    clientDepartment.CreatedOn = (DateTime)dr["CreatedOn"];
                    clientDepartment.CreatedBy = (string)dr["CreatedBy"];
                    clientDepartment.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    clientDepartment.LastModifiedBy = (string)dr["LastModifiedBy"];

                    DataTable dtClientDepartmentCategories = clientDao.GetClientDepartmentTestCategories(clientDepartment.ClientDepartmentId);
                    if (dtClientDepartmentCategories != null)
                    {
                        foreach (DataRow drClientDepartment in dtClientDepartmentCategories.Rows)
                        {
                            if (drClientDepartment["TestCategoryId"].ToString() == ((int)TestCategories.UA).ToString())
                            {
                                clientDepartment.IsUA = true;
                            }
                            else if (drClientDepartment["TestCategoryId"].ToString() == ((int)TestCategories.Hair).ToString())
                            {
                                clientDepartment.IsHair = true;
                            }
                            else if (drClientDepartment["TestCategoryId"].ToString() == ((int)TestCategories.DNA).ToString())
                            {
                                clientDepartment.IsDNA = true;
                            }
                            else if (drClientDepartment["TestCategoryId"].ToString() == ((int)TestCategories.RC).ToString())
                            {
                                clientDepartment.IsRecordKeeping = true;
                            }
                            else if (drClientDepartment["TestCategoryId"].ToString() == ((int)TestCategories.BC).ToString())
                            {
                                clientDepartment.IsBC = true;
                            }
                        }
                    }
                    else
                    {
                        clientDepartment.IsUA = false;
                        clientDepartment.IsHair = false;
                        clientDepartment.IsDNA = false;
                        clientDepartment.IsBC = false;
                        clientDepartment.IsRecordKeeping = false;
                    }

                    DataTable dtClientContact = clientDao.GetClientContatList(clientDepartment.ClientId);

                    if (dtClientContact != null)
                    {
                        if (dtClientContact.Rows.Count == 1)
                        {
                            clientDepartment.ClientContact = new ClientContact();

                            clientDepartment.ClientContact.ClientContactId = (int)dtClientContact.Rows[0]["ClientContactId"];
                            clientDepartment.ClientContact.ClientDepartmentId = (int)dtClientContact.Rows[0]["ClientDepartmentId"];
                            clientDepartment.ClientContact.ClientContactFirstName = (string)dtClientContact.Rows[0]["ClientContactFirstName"];
                            clientDepartment.ClientContact.ClientContactLastName = (string)dtClientContact.Rows[0]["ClientContactLastName"];
                            clientDepartment.ClientContact.ClientContactPhone = dtClientContact.Rows[0]["ClientContactPhone"].ToString();
                            clientDepartment.ClientContact.ClientContactFax = dtClientContact.Rows[0]["ClientContactFax"].ToString();
                            clientDepartment.ClientContact.ClientContactEmail = dtClientContact.Rows[0]["ClientContactEmail"].ToString();
                            clientDepartment.ClientContact.IsSynchronized = dtClientContact.Rows[0]["IsSynchronized"].ToString() == "1" ? true : false;
                            clientDepartment.ClientContact.CreatedOn = (DateTime)dtClientContact.Rows[0]["CreatedOn"];
                            clientDepartment.ClientContact.CreatedBy = (string)dtClientContact.Rows[0]["CreatedBy"];
                            clientDepartment.ClientContact.LastModifiedOn = (DateTime)dtClientContact.Rows[0]["LastModifiedOn"];
                            clientDepartment.ClientContact.LastModifiedBy = (string)dtClientContact.Rows[0]["LastModifiedBy"];

                            clientDepartment.MainContact = clientDepartment.ClientContact.ClientContactFirstName + " " + clientDepartment.ClientContact.ClientContactLastName;
                            clientDepartment.ClientPhone = clientDepartment.ClientContact.ClientContactPhone;
                            clientDepartment.ClientFax = clientDepartment.ClientContact.ClientContactFax;
                            clientDepartment.ClientEmail = clientDepartment.ClientContact.ClientContactEmail;
                        }
                    }

                    DataTable dtClientAddresses = clientDao.GetClientAddressList(clientDepartment.ClientId);

                    if (dtClientAddresses != null)
                    {
                        foreach (DataRow drAddress in dtClientAddresses.Rows)
                        {
                            ClientAddress address = new ClientAddress();

                            address.AddressId = (int)drAddress["ClientAddressId"];
                            address.ClientDepartmentId = (int)drAddress["ClientDepartmentId"];
                            address.AddressTypeId = (AddressTypes)drAddress["AddressTypeId"];
                            address.Address1 = (string)drAddress["ClientAddress1"];
                            address.Address2 = drAddress["ClientAddress2"].ToString();
                            address.City = (string)drAddress["ClientCity"];
                            address.State = (string)drAddress["ClientState"];
                            address.ZipCode = (string)drAddress["ClientZip"];
                            address.IsSynchronized = drAddress["IsSynchronized"].ToString() == "1" ? true : false;
                            address.CreatedOn = (DateTime)drAddress["CreatedOn"];
                            address.CreatedBy = (string)drAddress["CreatedBy"];
                            address.LastModifiedOn = (DateTime)drAddress["LastModifiedOn"];
                            address.LastModifiedBy = (string)drAddress["LastModifiedBy"];

                            clientDepartment.ClientAddresses.Add(address);

                            if (address.AddressTypeId == AddressTypes.PhysicalAddress1)
                            {
                                clientDepartment.ClientCity = address.City;
                                clientDepartment.ClientState = address.State;
                            }
                        }
                    }

                    clientDepartmentList.Add(clientDepartment);
                }

                return clientDepartmentList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetClientDepartments(int clientDepartmentId)
        {
            try
            {
                DataTable dtClientDepartment = clientDao.GetClientDepartments(clientDepartmentId);
                return dtClientDepartment;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetClientDepartmentDocTypes(int clientDepartmentId)
        {
            try
            {
                DataTable dtClientDeptDocTypes = clientDao.GetClientDepartmentDocTypes(clientDepartmentId);
                return dtClientDeptDocTypes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetClientContatListByClientId(int clientId)
        {
            try
            {
                DataTable dtClients = clientDao.GetClientContatListByClientId(clientId);

                return dtClients;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //start here
        public DataTable GetClientDashboard(string clientdepartmentID)
        {
            try
            {
                DataTable dtClients = clientDao.GetClientDashboard(clientdepartmentID);

                return dtClients;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetDonorId(string departmentId, string teststatus)
        {
            try
            {
                DataTable donorid = clientDao.GetDonorId(departmentId, teststatus);
                return donorid;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable ClientDepartment(int clientDepartmentId)
        {
            try
            {
                DataTable dtClients = clientDao.ClientDepartments(clientDepartmentId);

                return dtClients;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable BCClientDepartment(int clientDepartmentId)
        {
            try
            {
                DataTable dtClients = clientDao.BCClientDepartments(clientDepartmentId);

                return dtClients;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable DNAClientDepartment(int clientDepartmentId)
        {
            try
            {
                DataTable dtClients = clientDao.DNAClientDepartments(clientDepartmentId);

                return dtClients;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetClientLabCode(string labCode)
        {
            try
            {
                DataTable clientDepartmentLabCode = clientDao.GetByLabCode(labCode);

                return clientDepartmentLabCode;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ClientDepartment> SortingClientDepartment(int clientId, string searchparam, bool active, string getInActive = null)
        {
            try
            {
                DataTable dtClientDepartment = clientDao.sortingClientDepartment(clientId, searchparam, active, getInActive);

                List<ClientDepartment> clientDepartmentList = new List<ClientDepartment>();

                foreach (DataRow dr in dtClientDepartment.Rows)
                {
                    ClientDepartment clientDepartment = new ClientDepartment();

                    clientDepartment.ClientDepartmentId = (int)dr["ClientDepartmentId"];
                    clientDepartment.ClientId = (int)dr["ClientId"];
                    clientDepartment.DepartmentName = dr["DepartmentName"].ToString();
                    clientDepartment.LabCode = dr["LabCode"].ToString();
                    clientDepartment.MROTypeId = (ClientMROTypes)((int)dr["MROTypeId"]);
                    clientDepartment.PaymentTypeId = (ClientPaymentTypes)dr["PaymentTypeId"];
                    clientDepartment.IsMailingAddressPhysical = dr["IsMailingAddressPhysical"].ToString() == "1" ? true : false;
                    clientDepartment.SalesRepresentativeId = dr["SalesRepresentativeId"].ToString() != string.Empty ? (int?)dr["SalesRepresentativeId"] : null;
                    clientDepartment.SalesComissions = dr["SalesComissions"].ToString() != string.Empty ? (double?)dr["SalesComissions"] : null;
                    clientDepartment.IsDepartmentActive = dr["IsDepartmentActive"].ToString() == "1" ? true : false;
                    clientDepartment.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    clientDepartment.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    clientDepartment.CreatedOn = (DateTime)dr["CreatedOn"];
                    clientDepartment.CreatedBy = (string)dr["CreatedBy"];
                    clientDepartment.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    clientDepartment.LastModifiedBy = (string)dr["LastModifiedBy"];

                    DataTable dtClientDepartmentCategories = clientDao.GetClientDepartmentTestCategories(clientDepartment.ClientDepartmentId);
                    if (dtClientDepartmentCategories != null)
                    {
                        foreach (DataRow drClientDepartment in dtClientDepartmentCategories.Rows)
                        {
                            if (drClientDepartment["TestCategoryId"].ToString() == ((int)TestCategories.UA).ToString())
                            {
                                clientDepartment.IsUA = true;
                            }
                            else if (drClientDepartment["TestCategoryId"].ToString() == ((int)TestCategories.Hair).ToString())
                            {
                                clientDepartment.IsHair = true;
                            }
                            else if (drClientDepartment["TestCategoryId"].ToString() == ((int)TestCategories.DNA).ToString())
                            {
                                clientDepartment.IsDNA = true;
                            }
                            else if (drClientDepartment["TestCategoryId"].ToString() == ((int)TestCategories.RC).ToString())
                            {
                                clientDepartment.IsRecordKeeping = true;
                            }
                            else if (drClientDepartment["TestCategoryId"].ToString() == ((int)TestCategories.BC).ToString())
                            {
                                clientDepartment.IsBC = true;
                            }
                        }
                    }
                    else
                    {
                        clientDepartment.IsUA = false;
                        clientDepartment.IsHair = false;
                        clientDepartment.IsDNA = false;
                        clientDepartment.IsBC = false;
                        clientDepartment.IsRecordKeeping = false;
                    }

                    DataTable dtClientContact = clientDao.GetClientContatList(clientDepartment.ClientId);

                    if (dtClientContact != null)
                    {
                        if (dtClientContact.Rows.Count == 1)
                        {
                            clientDepartment.ClientContact = new ClientContact();

                            clientDepartment.ClientContact.ClientContactId = (int)dtClientContact.Rows[0]["ClientContactId"];
                            clientDepartment.ClientContact.ClientDepartmentId = (int)dtClientContact.Rows[0]["ClientDepartmentId"];
                            clientDepartment.ClientContact.ClientContactFirstName = (string)dtClientContact.Rows[0]["ClientContactFirstName"];
                            clientDepartment.ClientContact.ClientContactLastName = (string)dtClientContact.Rows[0]["ClientContactLastName"];
                            clientDepartment.ClientContact.ClientContactPhone = dtClientContact.Rows[0]["ClientContactPhone"].ToString();
                            clientDepartment.ClientContact.ClientContactFax = dtClientContact.Rows[0]["ClientContactFax"].ToString();
                            clientDepartment.ClientContact.ClientContactEmail = dtClientContact.Rows[0]["ClientContactEmail"].ToString();
                            clientDepartment.ClientContact.IsSynchronized = dtClientContact.Rows[0]["IsSynchronized"].ToString() == "1" ? true : false;
                            clientDepartment.ClientContact.CreatedOn = (DateTime)dtClientContact.Rows[0]["CreatedOn"];
                            clientDepartment.ClientContact.CreatedBy = (string)dtClientContact.Rows[0]["CreatedBy"];
                            clientDepartment.ClientContact.LastModifiedOn = (DateTime)dtClientContact.Rows[0]["LastModifiedOn"];
                            clientDepartment.ClientContact.LastModifiedBy = (string)dtClientContact.Rows[0]["LastModifiedBy"];

                            clientDepartment.MainContact = clientDepartment.ClientContact.ClientContactFirstName + " " + clientDepartment.ClientContact.ClientContactLastName;
                            clientDepartment.ClientPhone = clientDepartment.ClientContact.ClientContactPhone;
                            clientDepartment.ClientFax = clientDepartment.ClientContact.ClientContactFax;
                            clientDepartment.ClientEmail = clientDepartment.ClientContact.ClientContactEmail;
                        }
                    }

                    DataTable dtClientAddresses = clientDao.GetClientAddressList(clientDepartment.ClientId);

                    if (dtClientAddresses != null)
                    {
                        foreach (DataRow drAddress in dtClientAddresses.Rows)
                        {
                            ClientAddress address = new ClientAddress();

                            address.AddressId = (int)drAddress["ClientAddressId"];
                            address.ClientDepartmentId = (int)drAddress["ClientDepartmentId"];
                            address.AddressTypeId = (AddressTypes)drAddress["AddressTypeId"];
                            address.Address1 = (string)drAddress["ClientAddress1"];
                            address.Address2 = drAddress["ClientAddress2"].ToString();
                            address.City = (string)drAddress["ClientCity"];
                            address.State = (string)drAddress["ClientState"];
                            address.ZipCode = (string)drAddress["ClientZip"];
                            address.IsSynchronized = drAddress["IsSynchronized"].ToString() == "1" ? true : false;
                            address.CreatedOn = (DateTime)drAddress["CreatedOn"];
                            address.CreatedBy = (string)drAddress["CreatedBy"];
                            address.LastModifiedOn = (DateTime)drAddress["LastModifiedOn"];
                            address.LastModifiedBy = (string)drAddress["LastModifiedBy"];

                            clientDepartment.ClientAddresses.Add(address);

                            if (address.AddressTypeId == AddressTypes.PhysicalAddress1)
                            {
                                clientDepartment.ClientCity = address.City;
                                clientDepartment.ClientState = address.State;
                            }
                        }
                    }

                    clientDepartmentList.Add(clientDepartment);
                }

                return clientDepartmentList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}