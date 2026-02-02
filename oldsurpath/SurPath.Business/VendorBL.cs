using SurPath.Data;
using SurPath.Entity;
using SurPath.Enum;
using System;
using System.Collections.Generic;
using System.Data;

namespace SurPath.Business
{
    /// <summary>
    /// Vendor related business process.
    /// </summary>
    ///
    public class VendorBL : BusinessObject
    {
        private VendorDao vendorDao = new VendorDao();

        public int Save(Vendor vendor)
        {
            if (vendor.VendorId == 0)
            {
                return vendorDao.Insert(vendor);
            }
            else
            {
                return vendorDao.Update(vendor);
            }
        }

        public int Delete(int vendorId, string currentUserName)
        {
            return vendorDao.Delete(vendorId, currentUserName);
        }

        public Vendor Get(int vendorId)
        {
            try
            {
                Vendor vendor = vendorDao.Get(vendorId);

                return vendor;
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
                DataTable vendor = vendorDao.GetByEmail(Email);

                return vendor;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetVendorAddresses(int vendorId)
        {
            try
            {
                DataTable vendor = vendorDao.GetVendorAddresses(vendorId);

                return vendor;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Vendor> GetList()
        {
            try
            {
                DataTable dtVendor = vendorDao.GetList();

                List<Vendor> vendorList = new List<Vendor>();

                foreach (DataRow dr in dtVendor.Rows)
                {
                    Vendor vendor = new Vendor();

                    vendor.VendorId = (int)dr["VendorId"];
                    vendor.VendorTypeId = (VendorTypes)dr["VendorTypeId"];
                    vendor.VendorName = dr["VendorName"].ToString();
                    vendor.VendorMainContact = dr["VendorMainContact"].ToString();
                    vendor.VendorPhone = dr["VendorPhone"].ToString();
                    vendor.VendorFax = dr["VendorFax"].ToString();
                    vendor.VendorEmail = dr["VendorEmail"].ToString();
                    vendor.VendorStatus = (VendorStatus)dr["VendorStatus"];
                    vendor.InactiveDate = (DateTime)dr["InactiveDate"];
                    vendor.InactiveReason = dr["InactiveReason"].ToString();
                    // vendor.IsMailingAddressPhysical1 = (int)dr["IsMailingAddressPhysical1"].ToString() == "1" ? true : false;
                    vendor.IsMailingAddressPhysical1 = (int)dr["IsMailingAddressPhysical1"];
                    vendor.MPOSMROCost = dr["MPOSMROCost"].ToString() != string.Empty ? (double?)dr["MPOSMROCost"] : null;
                    vendor.MALLMROCost = dr["MALLMROCost"].ToString() != string.Empty ? (double?)dr["MALLMROCost"] : null;
                    vendor.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    vendor.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    vendor.CreatedOn = (DateTime)dr["CreatedOn"];
                    vendor.CreatedBy = (string)dr["CreatedBy"];
                    vendor.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    vendor.LastModifiedBy = (string)dr["LastModifiedBy"];

                    DataTable dtVendorServices = vendorDao.GetVendorServices(vendor.VendorId);

                    if (dtVendorServices != null)
                    {
                        foreach (DataRow drService in dtVendorServices.Rows)
                        {
                            vendor.Services.Add((int)drService["VendorServiceId"]);
                        }
                    }

                    DataTable dtVendorAddresses = vendorDao.GetVendorAddresses(vendor.VendorId);

                    if (dtVendorAddresses != null)
                    {
                        foreach (DataRow drAddress in dtVendorAddresses.Rows)
                        {
                            VendorAddress address = new VendorAddress();

                            address.AddressId = (int)drAddress["VendorAddressId"];
                            address.VendorId = (int)drAddress["VendorId"];
                            address.AddressTypeId = (AddressTypes)drAddress["AddressTypeId"];
                            address.Address1 = (string)drAddress["VendorAddress1"];
                            address.Address2 = drAddress["VendorAddress2"].ToString();
                            address.City = (string)drAddress["VendorCity"];
                            address.State = (string)drAddress["VendorState"];
                            address.ZipCode = (string)drAddress["VendorZip"];
                            address.Phone = (string)drAddress["VendorPhone"];
                            address.Fax = drAddress["VendorFax"].ToString();
                            address.Email = (string)drAddress["VendorEmail"];
                            address.IsSynchronized = drAddress["IsSynchronized"].ToString() == "1" ? true : false;
                            address.CreatedOn = (DateTime)drAddress["CreatedOn"];
                            address.CreatedBy = (string)drAddress["CreatedBy"];
                            address.LastModifiedOn = (DateTime)drAddress["LastModifiedOn"];
                            address.LastModifiedBy = (string)drAddress["LastModifiedBy"];

                            vendor.Addresses.Add(address);

                            if (address.AddressTypeId == AddressTypes.PhysicalAddress1)
                            {
                                vendor.VendorCity = address.City;
                                vendor.VendorState = address.State;
                            }
                        }
                    }

                    vendorList.Add(vendor);
                }

                return vendorList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Vendor> GetCollectionCenterList()
        {
            try
            {
                DataTable dtVendor = vendorDao.GetCollectionCenterList();

                List<Vendor> vendorList = new List<Vendor>();

                foreach (DataRow dr in dtVendor.Rows)
                {
                    Vendor vendor = new Vendor();

                    vendor.VendorId = (int)dr["VendorId"];
                    vendor.VendorTypeId = (VendorTypes)dr["VendorTypeId"];
                    vendor.VendorName = dr["VendorName"].ToString();
                    vendor.VendorMainContact = dr["VendorMainContact"].ToString();
                    vendor.VendorPhone = dr["VendorPhone"].ToString();
                    vendor.VendorFax = dr["VendorFax"].ToString();
                    vendor.VendorEmail = dr["VendorEmail"].ToString();
                    vendor.VendorStatus = (VendorStatus)dr["VendorStatus"];
                    vendor.InactiveDate = (DateTime)dr["InactiveDate"];
                    vendor.InactiveReason = dr["InactiveReason"].ToString();
                    //vendor.IsMailingAddressPhysical1 = dr["IsMailingAddressPhysical1"].ToString() == "1" ? true : false;
                    vendor.IsMailingAddressPhysical1 = (int)dr["IsMailingAddressPhysical1"];
                    vendor.MPOSMROCost = dr["MPOSMROCost"].ToString() != string.Empty ? (double?)dr["MPOSMROCost"] : null;
                    vendor.MALLMROCost = dr["MALLMROCost"].ToString() != string.Empty ? (double?)dr["MALLMROCost"] : null;
                    vendor.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    vendor.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    vendor.CreatedOn = (DateTime)dr["CreatedOn"];
                    vendor.CreatedBy = (string)dr["CreatedBy"];
                    vendor.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    vendor.LastModifiedBy = (string)dr["LastModifiedBy"];

                    DataTable dtVendorServices = vendorDao.GetVendorServices(vendor.VendorId);

                    if (dtVendorServices != null)
                    {
                        foreach (DataRow drService in dtVendorServices.Rows)
                        {
                            vendor.Services.Add((int)drService["VendorServiceId"]);
                        }
                    }

                    DataTable dtVendorAddresses = vendorDao.GetVendorAddresses(vendor.VendorId);

                    if (dtVendorAddresses != null)
                    {
                        foreach (DataRow drAddress in dtVendorAddresses.Rows)
                        {
                            VendorAddress address = new VendorAddress();

                            address.AddressId = (int)drAddress["VendorAddressId"];
                            address.VendorId = (int)drAddress["VendorId"];
                            address.AddressTypeId = (AddressTypes)drAddress["AddressTypeId"];
                            address.Address1 = (string)drAddress["VendorAddress1"];
                            address.Address2 = drAddress["VendorAddress2"].ToString();
                            address.City = (string)drAddress["VendorCity"];
                            address.State = (string)drAddress["VendorState"];
                            address.ZipCode = (string)drAddress["VendorZip"];
                            address.Phone = (string)drAddress["VendorPhone"];
                            address.Fax = drAddress["VendorFax"].ToString();
                            address.Email = (string)drAddress["VendorEmail"];
                            address.IsSynchronized = drAddress["IsSynchronized"].ToString() == "1" ? true : false;
                            address.CreatedOn = (DateTime)drAddress["CreatedOn"];
                            address.CreatedBy = (string)drAddress["CreatedBy"];
                            address.LastModifiedOn = (DateTime)drAddress["LastModifiedOn"];
                            address.LastModifiedBy = (string)drAddress["LastModifiedBy"];

                            vendor.Addresses.Add(address);

                            if (address.AddressTypeId == AddressTypes.PhysicalAddress1)
                            {
                                vendor.VendorCity = address.City;
                                vendor.VendorState = address.State;
                            }
                        }
                    }

                    vendorList.Add(vendor);
                }

                return vendorList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Vendor> GetCollectionCenterVendorList(string zip)
        {
            try
            {
                DataTable dtVendor = vendorDao.GetCollectionCenterVendorList(zip);

                List<Vendor> vendorList = new List<Vendor>();

                foreach (DataRow dr in dtVendor.Rows)
                {
                    Vendor vendor = new Vendor();

                    vendor.VendorId = (int)dr["VendorId"];
                    vendor.VendorTypeId = (VendorTypes)dr["VendorTypeId"];
                    vendor.VendorName = dr["VendorName"].ToString();
                    vendor.VendorMainContact = dr["VendorMainContact"].ToString();
                    vendor.VendorPhone = dr["VendorPhone"].ToString();
                    vendor.VendorFax = dr["VendorFax"].ToString();
                    vendor.VendorEmail = dr["VendorEmail"].ToString();
                    vendor.VendorStatus = (VendorStatus)dr["VendorStatus"];
                    vendor.InactiveDate = (DateTime)dr["InactiveDate"];
                    vendor.InactiveReason = dr["InactiveReason"].ToString();
                    //vendor.IsMailingAddressPhysical1 = dr["IsMailingAddressPhysical1"].ToString() == "1" ? true : false;
                    vendor.IsMailingAddressPhysical1 = (int)dr["IsMailingAddressPhysical1"];
                    vendor.MPOSMROCost = dr["MPOSMROCost"].ToString() != string.Empty ? (double?)dr["MPOSMROCost"] : null;
                    vendor.MALLMROCost = dr["MALLMROCost"].ToString() != string.Empty ? (double?)dr["MALLMROCost"] : null;
                    vendor.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    vendor.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    vendor.CreatedOn = (DateTime)dr["CreatedOn"];
                    vendor.CreatedBy = (string)dr["CreatedBy"];
                    vendor.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    vendor.LastModifiedBy = (string)dr["LastModifiedBy"];

                    DataTable dtVendorServices = vendorDao.GetVendorServices(vendor.VendorId);

                    if (dtVendorServices != null)
                    {
                        foreach (DataRow drService in dtVendorServices.Rows)
                        {
                            vendor.Services.Add((int)drService["VendorServiceId"]);
                        }
                    }

                    DataTable dtVendorAddresses = vendorDao.GetVendorAddresses(vendor.VendorId);

                    if (dtVendorAddresses != null)
                    {
                        foreach (DataRow drAddress in dtVendorAddresses.Rows)
                        {
                            VendorAddress address = new VendorAddress();

                            address.AddressId = (int)drAddress["VendorAddressId"];
                            address.VendorId = (int)drAddress["VendorId"];
                            address.AddressTypeId = (AddressTypes)drAddress["AddressTypeId"];
                            address.Address1 = (string)drAddress["VendorAddress1"];
                            address.Address2 = drAddress["VendorAddress2"].ToString();
                            address.City = (string)drAddress["VendorCity"];
                            address.State = (string)drAddress["VendorState"];
                            address.ZipCode = (string)drAddress["VendorZip"];
                            address.Phone = (string)drAddress["VendorPhone"];
                            address.Fax = drAddress["VendorFax"].ToString();
                            address.Email = (string)drAddress["VendorEmail"];
                            address.IsSynchronized = drAddress["IsSynchronized"].ToString() == "1" ? true : false;
                            address.CreatedOn = (DateTime)drAddress["CreatedOn"];
                            address.CreatedBy = (string)drAddress["CreatedBy"];
                            address.LastModifiedOn = (DateTime)drAddress["LastModifiedOn"];
                            address.LastModifiedBy = (string)drAddress["LastModifiedBy"];

                            vendor.Addresses.Add(address);

                            if (address.AddressTypeId == AddressTypes.PhysicalAddress1)
                            {
                                vendor.VendorCity = address.City;
                                vendor.VendorState = address.State;
                            }
                        }
                    }

                    vendorList.Add(vendor);
                }

                return vendorList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Vendor> GetList(Dictionary<string, string> searchParam)
        {
            try
            {
                DataTable dtVendor = vendorDao.GetList(searchParam);

                List<Vendor> vendorList = new List<Vendor>();

                foreach (DataRow dr in dtVendor.Rows)
                {
                    Vendor vendor = new Vendor();

                    vendor.VendorId = (int)dr["VendorId"];
                    vendor.VendorTypeId = (VendorTypes)dr["VendorTypeId"];
                    vendor.VendorName = dr["VendorName"].ToString();
                    vendor.VendorMainContact = dr["VendorMainContact"].ToString();
                    vendor.VendorPhone = dr["VendorPhone"].ToString();
                    vendor.VendorFax = dr["VendorFax"].ToString();
                    vendor.VendorEmail = dr["VendorEmail"].ToString();
                    vendor.VendorStatus = (VendorStatus)dr["VendorStatus"];
                    vendor.InactiveDate = (DateTime)dr["InactiveDate"];
                    vendor.InactiveReason = dr["InactiveReason"].ToString();
                    vendor.IsMailingAddressPhysical1 = (int)dr["IsMailingAddressPhysical1"];
                    //vendor.IsMailingAddressPhysical1 = dr["IsMailingAddressPhysical1"].ToString() == "1" ? true : false;

                    vendor.MPOSMROCost = dr["MPOSMROCost"].ToString() != string.Empty ? (double?)dr["MPOSMROCost"] : null;
                    vendor.MALLMROCost = dr["MALLMROCost"].ToString() != string.Empty ? (double?)dr["MALLMROCost"] : null;
                    vendor.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    vendor.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    vendor.CreatedOn = (DateTime)dr["CreatedOn"];
                    vendor.CreatedBy = (string)dr["CreatedBy"];
                    vendor.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    vendor.LastModifiedBy = (string)dr["LastModifiedBy"];

                    DataTable dtVendorList = vendorDao.GetVendorServices(vendor.VendorId);

                    if (dtVendorList != null)
                    {
                        foreach (DataRow drVendor in dtVendorList.Rows)
                        {
                            vendor.Services.Add((int)drVendor["VendorServiceId"]);
                        }
                    }

                    DataTable dtVendorAddresses = vendorDao.GetVendorAddresses(vendor.VendorId);

                    if (dtVendorAddresses != null)
                    {
                        foreach (DataRow drAddress in dtVendorAddresses.Rows)
                        {
                            VendorAddress address = new VendorAddress();

                            address.AddressId = (int)drAddress["VendorAddressId"];
                            address.VendorId = (int)drAddress["VendorId"];
                            address.AddressTypeId = (AddressTypes)drAddress["AddressTypeId"];
                            address.Address1 = (string)drAddress["VendorAddress1"];
                            address.Address2 = drAddress["VendorAddress2"].ToString();
                            address.City = (string)drAddress["VendorCity"];
                            address.State = (string)drAddress["VendorState"];
                            address.ZipCode = (string)drAddress["VendorZip"];
                            address.Phone = (string)drAddress["VendorPhone"];
                            address.Fax = drAddress["VendorFax"].ToString();
                            address.Email = (string)drAddress["VendorEmail"];
                            address.IsSynchronized = drAddress["IsSynchronized"].ToString() == "1" ? true : false;
                            address.CreatedOn = (DateTime)drAddress["CreatedOn"];
                            address.CreatedBy = (string)drAddress["CreatedBy"];
                            address.LastModifiedOn = (DateTime)drAddress["LastModifiedOn"];
                            address.LastModifiedBy = (string)drAddress["LastModifiedBy"];

                            vendor.Addresses.Add(address);

                            if (address.AddressTypeId == AddressTypes.PhysicalAddress1)
                            {
                                vendor.VendorCity = address.City;
                                vendor.VendorState = address.State;
                            }
                        }
                    }

                    vendorList.Add(vendor);
                }

                return vendorList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Vendor> GetListByVendorType(VendorTypes vendorType)
        {
            try
            {
                DataTable dtVendor = vendorDao.GetListByVendorType(vendorType);

                List<Vendor> vendorList = new List<Vendor>();

                foreach (DataRow dr in dtVendor.Rows)
                {
                    Vendor vendor = new Vendor();

                    vendor.VendorId = (int)dr["VendorId"];
                    vendor.VendorTypeId = (VendorTypes)dr["VendorTypeId"];
                    vendor.VendorName = dr["VendorName"].ToString();
                    vendor.VendorMainContact = dr["VendorMainContact"].ToString();
                    vendor.VendorPhone = dr["VendorPhone"].ToString();
                    vendor.VendorFax = dr["VendorFax"].ToString();
                    vendor.VendorEmail = dr["VendorEmail"].ToString();
                    vendor.VendorStatus = (VendorStatus)dr["VendorStatus"];
                    vendor.InactiveDate = (DateTime)dr["InactiveDate"];
                    vendor.InactiveReason = dr["InactiveReason"].ToString();
                    // vendor.IsMailingAddressPhysical1 = dr["IsMailingAddressPhysical1"].ToString() == "1" ? true : false;
                    vendor.IsMailingAddressPhysical1 = (int)dr["IsMailingAddressPhysical1"];
                    vendor.MPOSMROCost = dr["MPOSMROCost"].ToString() != string.Empty ? (double?)dr["MPOSMROCost"] : null;
                    vendor.MALLMROCost = dr["MALLMROCost"].ToString() != string.Empty ? (double?)dr["MALLMROCost"] : null;
                    vendor.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    vendor.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    vendor.CreatedOn = (DateTime)dr["CreatedOn"];
                    vendor.CreatedBy = (string)dr["CreatedBy"];
                    vendor.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    vendor.LastModifiedBy = (string)dr["LastModifiedBy"];

                    DataTable dtVendorServices = vendorDao.GetVendorServices(vendor.VendorId);

                    if (dtVendorServices != null)
                    {
                        foreach (DataRow drService in dtVendorServices.Rows)
                        {
                            vendor.Services.Add((int)drService["VendorServiceId"]);
                        }
                    }

                    DataTable dtVendorAddresses = vendorDao.GetVendorAddresses(vendor.VendorId);

                    if (dtVendorAddresses != null)
                    {
                        foreach (DataRow drAddress in dtVendorAddresses.Rows)
                        {
                            VendorAddress address = new VendorAddress();

                            address.AddressId = (int)drAddress["VendorAddressId"];
                            address.VendorId = (int)drAddress["VendorId"];
                            address.AddressTypeId = (AddressTypes)drAddress["AddressTypeId"];
                            address.Address1 = (string)drAddress["VendorAddress1"];
                            address.Address2 = drAddress["VendorAddress2"].ToString();
                            address.City = (string)drAddress["VendorCity"];
                            address.State = (string)drAddress["VendorState"];
                            address.ZipCode = (string)drAddress["VendorZip"];
                            address.Phone = (string)drAddress["VendorPhone"];
                            address.Fax = drAddress["VendorFax"].ToString();
                            address.Email = (string)drAddress["VendorEmail"];
                            address.IsSynchronized = drAddress["IsSynchronized"].ToString() == "1" ? true : false;
                            address.CreatedOn = (DateTime)drAddress["CreatedOn"];
                            address.CreatedBy = (string)drAddress["CreatedBy"];
                            address.LastModifiedOn = (DateTime)drAddress["LastModifiedOn"];
                            address.LastModifiedBy = (string)drAddress["LastModifiedBy"];

                            vendor.Addresses.Add(address);

                            if (address.AddressTypeId == AddressTypes.PhysicalAddress1)
                            {
                                vendor.VendorCity = address.City;
                                vendor.VendorState = address.State;
                            }
                        }
                    }

                    vendorList.Add(vendor);
                }

                return vendorList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Vendor GetAddress(int vendorId)
        {
            try
            {
                Vendor vendor = vendorDao.GetAddress(vendorId);

                return vendor;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Vendor> GetVendorCollectionCenterList()
        {
            try
            {
                DataTable dtVendor = vendorDao.GetVendorCollectionCenterList();

                List<Vendor> vendorList = new List<Vendor>();

                foreach (DataRow dr in dtVendor.Rows)
                {
                    Vendor vendor = new Vendor();

                    vendor.VendorId = (int)dr["VendorId"];
                    vendor.VendorTypeId = (VendorTypes)dr["VendorTypeId"];
                    vendor.VendorName = dr["VendorName"].ToString();
                    vendor.VendorMainContact = dr["VendorMainContact"].ToString();
                    vendor.VendorPhone = dr["VendorPhone"].ToString();
                    vendor.VendorFax = dr["VendorFax"].ToString();
                    vendor.VendorEmail = dr["VendorEmail"].ToString();
                    vendor.VendorStatus = (VendorStatus)dr["VendorStatus"];
                    vendor.InactiveDate = (DateTime)dr["InactiveDate"];
                    vendor.InactiveReason = dr["InactiveReason"].ToString();
                    //vendor.IsMailingAddressPhysical1 = dr["IsMailingAddressPhysical1"].ToString() == "1" ? true : false;
                    vendor.IsMailingAddressPhysical1 = (int)dr["IsMailingAddressPhysical1"];
                    vendor.MPOSMROCost = dr["MPOSMROCost"].ToString() != string.Empty ? (double?)dr["MPOSMROCost"] : null;
                    vendor.MALLMROCost = dr["MALLMROCost"].ToString() != string.Empty ? (double?)dr["MALLMROCost"] : null;
                    vendor.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    vendor.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    vendor.CreatedOn = (DateTime)dr["CreatedOn"];
                    vendor.CreatedBy = (string)dr["CreatedBy"];
                    vendor.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    vendor.LastModifiedBy = (string)dr["LastModifiedBy"];

                    DataTable dtVendorServices = vendorDao.GetVendorServices(vendor.VendorId);

                    if (dtVendorServices != null)
                    {
                        foreach (DataRow drService in dtVendorServices.Rows)
                        {
                            vendor.Services.Add((int)drService["VendorServiceId"]);
                        }
                    }

                    DataTable dtVendorAddresses = vendorDao.GetVendorAddresses(vendor.VendorId);

                    if (dtVendorAddresses != null)
                    {
                        foreach (DataRow drAddress in dtVendorAddresses.Rows)
                        {
                            VendorAddress address = new VendorAddress();

                            address.AddressId = (int)drAddress["VendorAddressId"];
                            address.VendorId = (int)drAddress["VendorId"];
                            address.AddressTypeId = (AddressTypes)drAddress["AddressTypeId"];
                            address.Address1 = (string)drAddress["VendorAddress1"];
                            address.Address2 = drAddress["VendorAddress2"].ToString();
                            address.City = (string)drAddress["VendorCity"];
                            address.State = (string)drAddress["VendorState"];
                            address.ZipCode = (string)drAddress["VendorZip"];
                            address.Phone = (string)drAddress["VendorPhone"];
                            address.Fax = drAddress["VendorFax"].ToString();
                            address.Email = (string)drAddress["VendorEmail"];
                            address.IsSynchronized = drAddress["IsSynchronized"].ToString() == "1" ? true : false;
                            address.CreatedOn = (DateTime)drAddress["CreatedOn"];
                            address.CreatedBy = (string)drAddress["CreatedBy"];
                            address.LastModifiedOn = (DateTime)drAddress["LastModifiedOn"];
                            address.LastModifiedBy = (string)drAddress["LastModifiedBy"];

                            vendor.Addresses.Add(address);

                            if (address.AddressTypeId == AddressTypes.PhysicalAddress1)
                            {
                                vendor.VendorCity = address.City;
                                vendor.VendorState = address.State;
                            }
                        }
                    }

                    vendorList.Add(vendor);
                }

                return vendorList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Vendor> Sorting(string VendorType, string searchparam, bool active, string getInActive = null)
        {
            try
            {
                DataTable dtVendor = vendorDao.sorting(VendorType, searchparam, active, getInActive);

                List<Vendor> vendorList = new List<Vendor>();

                foreach (DataRow dr in dtVendor.Rows)
                {
                    Vendor vendor = new Vendor();

                    vendor.VendorId = (int)dr["VendorId"];
                    vendor.VendorTypeId = (VendorTypes)dr["VendorTypeId"];
                    vendor.VendorName = dr["VendorName"].ToString();
                    vendor.VendorMainContact = dr["VendorMainContact"].ToString();
                    vendor.VendorPhone = dr["VendorPhone"].ToString();
                    vendor.VendorFax = dr["VendorFax"].ToString();
                    vendor.VendorEmail = dr["VendorEmail"].ToString();
                    vendor.VendorStatus = (VendorStatus)dr["VendorStatus"];
                    vendor.InactiveDate = (DateTime)dr["InactiveDate"];
                    vendor.InactiveReason = dr["InactiveReason"].ToString();
                    vendor.IsMailingAddressPhysical1 = (int)dr["IsMailingAddressPhysical1"];
                    //vendor.IsMailingAddressPhysical1 = dr["IsMailingAddressPhysical1"].ToString() == "1" ? true : false;

                    vendor.MPOSMROCost = dr["MPOSMROCost"].ToString() != string.Empty ? (double?)dr["MPOSMROCost"] : null;
                    vendor.MALLMROCost = dr["MALLMROCost"].ToString() != string.Empty ? (double?)dr["MALLMROCost"] : null;
                    vendor.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    vendor.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    vendor.CreatedOn = (DateTime)dr["CreatedOn"];
                    vendor.CreatedBy = (string)dr["CreatedBy"];
                    vendor.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    vendor.LastModifiedBy = (string)dr["LastModifiedBy"];

                    DataTable dtVendorList = vendorDao.GetVendorServices(vendor.VendorId);

                    if (dtVendorList != null)
                    {
                        foreach (DataRow drVendor in dtVendorList.Rows)
                        {
                            vendor.Services.Add((int)drVendor["VendorServiceId"]);
                        }
                    }

                    DataTable dtVendorAddresses = vendorDao.GetVendorAddresses(vendor.VendorId);

                    if (dtVendorAddresses != null)
                    {
                        foreach (DataRow drAddress in dtVendorAddresses.Rows)
                        {
                            VendorAddress address = new VendorAddress();

                            address.AddressId = (int)drAddress["VendorAddressId"];
                            address.VendorId = (int)drAddress["VendorId"];
                            address.AddressTypeId = (AddressTypes)drAddress["AddressTypeId"];
                            address.Address1 = (string)drAddress["VendorAddress1"];
                            address.Address2 = drAddress["VendorAddress2"].ToString();
                            address.City = (string)drAddress["VendorCity"];
                            address.State = (string)drAddress["VendorState"];
                            address.ZipCode = (string)drAddress["VendorZip"];
                            address.Phone = (string)drAddress["VendorPhone"];
                            address.Fax = drAddress["VendorFax"].ToString();
                            address.Email = (string)drAddress["VendorEmail"];
                            address.IsSynchronized = drAddress["IsSynchronized"].ToString() == "1" ? true : false;
                            address.CreatedOn = (DateTime)drAddress["CreatedOn"];
                            address.CreatedBy = (string)drAddress["CreatedBy"];
                            address.LastModifiedOn = (DateTime)drAddress["LastModifiedOn"];
                            address.LastModifiedBy = (string)drAddress["LastModifiedBy"];

                            vendor.Addresses.Add(address);

                            if (address.AddressTypeId == AddressTypes.PhysicalAddress1)
                            {
                                vendor.VendorCity = address.City;
                                vendor.VendorState = address.State;
                            }
                        }
                    }

                    vendorList.Add(vendor);
                }

                return vendorList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}