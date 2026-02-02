using MySql.Data.MySqlClient;
using Serilog;
using SurPath.Entity;
using SurPath.Enum;
using SurPath.MySQLHelper;
using System;
using System.Collections.Generic;
using System.Data;

namespace SurPath.Data
{
    public class VendorDao : DataObject
    {
        #region Constructor
        public static ILogger _logger { get; set; }

        public VendorDao(ILogger __logger = null)
        {
            if (__logger == null)
            {
                _logger = new LoggerConfiguration().CreateLogger();
            }
            else
            {
                _logger = __logger;
            }
        }

        #endregion Constructor

        #region Public Methods

        public int Insert(Vendor vendor)
        {
            int returnValue = 0;

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();
                try
                {
                    string sqlQuery = "INSERT INTO vendors (vendor_type_id, vendor_name, main_contact, vendor_phone, vendor_fax, vendor_email, vendor_status, inactive_date, inactive_reason, is_mailing_address_physical1, mpos_mro_cost, mall_mro_cost, is_synchronized, is_archived, created_on, created_by, last_modified_on, last_modified_by) VALUES (";
                    sqlQuery += "@VendorTypeId, @VendorName, @VendorMainContact, @VendorPhone, @VendorFax, @VendorEmail, @VendorStatus, @InactiveDate, @InactiveReason, @IsMailingAddressPhysical1, @MPOSMROCost, @MALLMROCost, b'0', b'0', NOW(), @CreatedBy, NOW(), @LastModifiedBy)";

                    MySqlParameter[] param = new MySqlParameter[14];
                    param[0] = new MySqlParameter("@VendorTypeId", (int)vendor.VendorTypeId);
                    param[1] = new MySqlParameter("@VendorName", vendor.VendorName);
                    param[2] = new MySqlParameter("@VendorMainContact", vendor.VendorMainContact);
                    param[3] = new MySqlParameter("@VendorPhone", vendor.VendorPhone);
                    param[4] = new MySqlParameter("@VendorFax", vendor.VendorFax);
                    param[5] = new MySqlParameter("@VendorEmail", vendor.VendorEmail);
                    param[6] = new MySqlParameter("@VendorStatus", (int)vendor.VendorStatus);
                    param[7] = new MySqlParameter("@InactiveDate", vendor.InactiveDate);
                    param[8] = new MySqlParameter("@InactiveReason", vendor.InactiveReason);
                    param[9] = new MySqlParameter("@IsMailingAddressPhysical1", vendor.IsMailingAddressPhysical1);
                    param[10] = new MySqlParameter("@MPOSMROCost", vendor.MPOSMROCost);
                    param[11] = new MySqlParameter("@MALLMROCost", vendor.MALLMROCost);
                    param[12] = new MySqlParameter("@CreatedBy", vendor.CreatedBy);
                    param[13] = new MySqlParameter("@LastModifiedBy", vendor.LastModifiedBy);

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                    sqlQuery = "SELECT LAST_INSERT_ID()";

                    returnValue = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlQuery));

                    sqlQuery = "INSERT INTO vendor_addresses (vendor_id, address_type_id, vendor_address_1, vendor_address_2, vendor_city, vendor_state, vendor_zip, vendor_phone, vendor_fax, vendor_email, is_synchronized, created_on, created_by, last_modified_on, last_modified_by) VALUES (";
                    sqlQuery += "@VendorId, @AddressTypeId, @VendorAddress1, @VendorAddress2, @VendorCity, @VendorState, @VendorZip, @VendorPhone, @VendorFax, @VendorEmail, b'0', NOW(), @CreatedBy, NOW(), @LastModifiedBy)";

                    foreach (VendorAddress address in vendor.Addresses)
                    {
                        param = new MySqlParameter[12];

                        param[0] = new MySqlParameter("@VendorId", returnValue);
                        param[1] = new MySqlParameter("@AddressTypeId", (int)address.AddressTypeId);
                        param[2] = new MySqlParameter("@VendorAddress1", address.Address1);
                        param[3] = new MySqlParameter("@VendorAddress2", address.Address2);
                        param[4] = new MySqlParameter("@VendorCity", address.City);
                        param[5] = new MySqlParameter("@VendorState", address.State);
                        param[6] = new MySqlParameter("@VendorZip", address.ZipCode);
                        param[7] = new MySqlParameter("@VendorPhone", address.Phone);
                        param[8] = new MySqlParameter("@VendorFax", address.Fax);
                        param[9] = new MySqlParameter("@VendorEmail", address.Email);
                        param[10] = new MySqlParameter("@CreatedBy", vendor.CreatedBy);
                        param[11] = new MySqlParameter("@LastModifiedBy", vendor.LastModifiedBy);

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);
                    }

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }

            return returnValue;
        }

        public int Update(Vendor vendor)
        {
            int returnValue = 0;

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();

                try
                {
                    string sqlQuery = "UPDATE vendors SET "
                                    + "vendor_type_id = @VendorTypeId, "
                                    + "vendor_name = @VendorName, "
                                    + "main_contact = @VendorMainContact, "
                                    + "vendor_phone = @VendorPhone, "
                                    + "vendor_fax = @VendorFax, "
                                    + "vendor_email = @VendorEmail, "
                                    + "vendor_status = @VendorStatus,"
                                    + "inactive_date = @InactiveDate,"
                                    + "inactive_reason = @InactiveReason,"
                                    + "is_mailing_address_physical1 = @IsMailingAddressPhysical1,"
                                    + "mpos_mro_cost = @MPOSMROCost,"
                                    + "mall_mro_cost = @MALLMROCost,"
                                    + "is_synchronized = b'0', "
                                    + "last_modified_on = NOW(), "
                                    + "last_modified_by = @LastModifiedBy "
                                    + "WHERE vendor_id = @VendorId";

                    MySqlParameter[] param = new MySqlParameter[14];
                    param[0] = new MySqlParameter("@VendorId", vendor.VendorId);
                    param[1] = new MySqlParameter("@VendorTypeId", (int)vendor.VendorTypeId);
                    param[2] = new MySqlParameter("@VendorName", vendor.VendorName);
                    param[3] = new MySqlParameter("@VendorMainContact", vendor.VendorMainContact);
                    param[4] = new MySqlParameter("@VendorPhone", vendor.VendorPhone);
                    param[5] = new MySqlParameter("@VendorFax", vendor.VendorFax);
                    param[6] = new MySqlParameter("@VendorEmail", vendor.VendorEmail);
                    param[7] = new MySqlParameter("@VendorStatus", (int)vendor.VendorStatus);
                    param[8] = new MySqlParameter("@InactiveDate", vendor.InactiveDate);
                    param[9] = new MySqlParameter("@InactiveReason", vendor.InactiveReason);
                    param[10] = new MySqlParameter("@IsMailingAddressPhysical1", vendor.IsMailingAddressPhysical1);
                    param[11] = new MySqlParameter("@MPOSMROCost", vendor.MPOSMROCost);
                    param[12] = new MySqlParameter("@MALLMROCost", vendor.MALLMROCost);
                    param[13] = new MySqlParameter("@LastModifiedBy", vendor.LastModifiedBy);

                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                    foreach (VendorAddress address in vendor.Addresses)
                    {
                        if (address.AddressId == 0)
                        {
                            sqlQuery = "INSERT INTO vendor_addresses (vendor_id, address_type_id, vendor_address_1, vendor_address_2, vendor_city, vendor_state, vendor_zip, vendor_phone, vendor_fax, vendor_email, is_synchronized, created_on, created_by, last_modified_on, last_modified_by) VALUES (";
                            sqlQuery += "@VendorId, @AddressTypeId, @VendorAddress1, @VendorAddress2, @VendorCity, @VendorState, @VendorZip, @VendorPhone, @VendorFax, @VendorEmail, b'0', NOW(), @CreatedBy, NOW(), @LastModifiedBy)";
                        }
                        else
                        {
                            sqlQuery = "UPDATE vendor_addresses SET "
                                        + "vendor_address_1 = @VendorAddress1, "
                                        + "vendor_address_2 = @VendorAddress2, "
                                        + "vendor_city = @VendorCity, "
                                        + "vendor_state = @VendorState, "
                                        + "vendor_zip = @VendorZip, "
                                        + "vendor_phone = @VendorPhone, "
                                        + "vendor_fax = @VendorFax, "
                                        + "vendor_email = @VendorEmail, "
                                        + "is_synchronized = b'0', "
                                        + "last_modified_on = NOW(), "
                                        + "last_modified_by = @LastModifiedBy "
                                        + "WHERE vendor_address_id = @VendorAddressId";
                        }

                        param = new MySqlParameter[13];

                        param[0] = new MySqlParameter("@VendorAddressId", address.AddressId);
                        param[1] = new MySqlParameter("@VendorId", vendor.VendorId);
                        param[2] = new MySqlParameter("@AddressTypeId", (int)address.AddressTypeId);
                        param[3] = new MySqlParameter("@VendorAddress1", address.Address1);
                        param[4] = new MySqlParameter("@VendorAddress2", address.Address2);
                        param[5] = new MySqlParameter("@VendorCity", address.City);
                        param[6] = new MySqlParameter("@VendorState", address.State);
                        param[7] = new MySqlParameter("@VendorZip", address.ZipCode);
                        param[8] = new MySqlParameter("@VendorPhone", address.Phone);
                        param[9] = new MySqlParameter("@VendorFax", address.Fax);
                        param[10] = new MySqlParameter("@VendorEmail", address.Email);
                        param[11] = new MySqlParameter("@CreatedBy", vendor.LastModifiedBy);
                        param[12] = new MySqlParameter("@LastModifiedBy", vendor.LastModifiedBy);

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);
                    }

                    trans.Commit();

                    returnValue = 1;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }

            return returnValue;
        }

        public int Delete(int VendorId, string currentUsername)
        {
            int returnValue = 0;

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();
                try
                {
                    string sqlCount1Query = "Select count(*) from clients where laboratory_vendor_id = " + VendorId + " and is_archived = 0";

                    int table1 = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlCount1Query));
                    if (table1 <= 0)
                    {
                        string sqlCount2Query = "Select count(*) from clients where mro_vendor_id = " + VendorId + " and is_archived = 0";

                        int table2 = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlCount2Query));
                        if (table2 <= 0)
                        {
                            string sqlCount3Query = "Select count(*) from donor_test_info where collection_site_vendor_id = " + VendorId + "";

                            int table3 = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlCount3Query));
                            if (table3 <= 0)
                            {
                                string sqlCount4Query = "Select count(*) from donor_test_info where laboratory_vendor_id = " + VendorId + "";

                                int table4 = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlCount4Query));
                                if (table4 <= 0)
                                {
                                    string sqlCount5Query = "Select count(*) from donor_test_info where mro_vendor_id = " + VendorId + "";

                                    int table5 = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlCount5Query));
                                    if (table5 <= 0)
                                    {
                                        string sqlCount6Query = "Select count(*) from donor_test_info where collection_site_1_id = " + VendorId + "";

                                        int table6 = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlCount6Query));
                                        if (table6 <= 0)
                                        {
                                            string sqlCount7Query = "Select count(*) from donor_test_info where collection_site_2_id = " + VendorId + ""; ;

                                            int table7 = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlCount7Query));
                                            if (table7 <= 0)
                                            {
                                                string sqlCount8Query = "Select count(*) from donor_test_info where collection_site_3_id = " + VendorId + ""; ;

                                                int table8 = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlCount8Query));
                                                if (table8 <= 0)
                                                {
                                                    string sqlCount9Query = "Select count(*) from donor_test_info where collection_site_4_id = " + VendorId + ""; ;

                                                    int table9 = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlCount9Query));
                                                    if (table9 <= 0)
                                                    {
                                                        string sqlQuery = "UPDATE vendors SET is_synchronized = b'0', is_archived = b'1', last_modified_on = NOW(), last_modified_by = @LastModifiedBy WHERE vendor_id = @VendorId";

                                                        MySqlParameter[] param = new MySqlParameter[2];
                                                        param[0] = new MySqlParameter("@VendorId", VendorId);
                                                        param[1] = new MySqlParameter("@LastModifiedBy", currentUsername);

                                                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                                                        trans.Commit();

                                                        returnValue = 1;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }

            return returnValue;
        }

        public Vendor Get(int VendorId)
        {
            Vendor vendor = null;
            string sqlQuery = "SELECT vendor_id AS VendorId , vendor_type_id AS VendorTypeId,vendor_name AS VendorName,main_contact AS VendorMainContact,vendor_phone AS VendorPhone,vendor_fax AS VendorFax, vendor_email AS VendorEmail,vendor_status AS VendorStatus,inactive_date AS InactiveDate,inactive_reason AS InactiveReason , is_mailing_address_physical1 AS IsMailingAddressPhysical1, mpos_mro_cost AS MPOSMROCost, mall_mro_cost AS MALLMROCost, is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM vendors WHERE is_archived = 0 AND vendor_id = @VendorId";

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                MySqlParameter[] param = new MySqlParameter[1];
                param[0] = new MySqlParameter("@VendorId", VendorId);

                MySqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                if (dr.Read())
                {
                    vendor = new Vendor();
                    vendor.VendorId = (int)dr["VendorId"];
                    vendor.VendorTypeId = (VendorTypes)dr["VendorTypeId"];
                    vendor.VendorName = (string)dr["VendorName"];
                    vendor.VendorMainContact = (string)dr["VendorMainContact"];
                    vendor.VendorPhone = (string)dr["VendorPhone"];
                    vendor.VendorFax = (string)dr["VendorFax"];
                    vendor.VendorEmail = (string)dr["VendorEmail"];
                    vendor.VendorStatus = (VendorStatus)dr["VendorStatus"];
                    vendor.InactiveDate = Convert.ToDateTime(dr["InactiveDate"], this.Culture);
                    vendor.InactiveReason = (string)dr["InactiveReason"];
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
                }
                dr.Close();

                sqlQuery = "SELECT vendor_service_id AS VendorServiceId FROM vendor_vendor_services WHERE vendor_id = @VendorId";

                param = new MySqlParameter[1];
                param[0] = new MySqlParameter("@VendorId", VendorId);

                dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                if (vendor != null)
                {
                    while (dr.Read())
                    {
                        vendor.Services.Add((int)dr["VendorServiceId"]);
                    }
                }
                dr.Close();

                sqlQuery = "SELECT vendor_address_id AS VendorAddressId, "
                            + "vendor_id AS VendorId, "
                            + "address_type_id AS AddressTypeId, "
                            + "vendor_address_1 AS VendorAddress1, "
                            + "vendor_address_2 AS VendorAddress2, "
                            + "vendor_city AS VendorCity, "
                            + "vendor_state AS VendorState, "
                            + "vendor_zip AS VendorZip, "
                            + "vendor_phone AS VendorPhone, "
                            + "vendor_fax AS VendorFax, "
                            + "vendor_email AS VendorEmail, "
                            + "is_synchronized AS IsSynchronized, "
                            + "created_on AS CreatedOn, "
                            + "created_by AS CreatedBy, "
                            + "last_modified_on AS LastModifiedOn, "
                            + "last_modified_by AS LastModifiedBy "
                            + "FROM vendor_addresses "
                            + "WHERE address_type_id = 1 AND vendor_id = @VendorId";

                param = new MySqlParameter[1];
                param[0] = new MySqlParameter("@VendorId", VendorId);

                dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                if (vendor != null)
                {
                    while (dr.Read())
                    {
                        VendorAddress address = new VendorAddress();

                        address.AddressId = (int)dr["VendorAddressId"];
                        address.VendorId = (int)dr["VendorId"];
                        address.AddressTypeId = (AddressTypes)dr["AddressTypeId"];
                        address.Address1 = (string)dr["VendorAddress1"];
                        address.Address2 = dr["VendorAddress2"].ToString();
                        address.City = (string)dr["VendorCity"];
                        address.State = (string)dr["VendorState"];
                        address.ZipCode = (string)dr["VendorZip"];
                        address.Phone = (string)dr["VendorPhone"];
                        address.Fax = dr["VendorFax"].ToString();
                        address.Email = (string)dr["VendorEmail"];
                        address.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                        address.CreatedOn = (DateTime)dr["CreatedOn"];
                        address.CreatedBy = (string)dr["CreatedBy"];
                        address.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                        address.LastModifiedBy = (string)dr["LastModifiedBy"];

                        vendor.Addresses.Add(address);

                        if (address.AddressTypeId == AddressTypes.PhysicalAddress1)
                        {
                            vendor.VendorCity = address.City;
                            vendor.VendorState = address.State;
                        }
                    }
                }
                dr.Close();
            }

            return vendor;
        }

        public DataTable GetList()
        {
            string sqlQuery = "SELECT vendor_id AS VendorId , vendor_type_id AS VendorTypeId, vendor_name AS VendorName,main_contact AS VendorMainContact,vendor_phone AS VendorPhone,vendor_fax AS VendorFax, vendor_email AS VendorEmail,vendor_status AS VendorStatus,inactive_date AS InactiveDate,inactive_reason AS InactiveReason,is_mailing_address_physical1 AS IsMailingAddressPhysical1 , mpos_mro_cost AS MPOSMROCost, mall_mro_cost AS MALLMROCost, is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM vendors WHERE is_archived = 0 ORDER BY vendor_name";

            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sqlQuery);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        public DataTable GetCollectionCenterList()
        {
            string sqlQuery = "SELECT vendor_id AS VendorId , vendor_type_id AS VendorTypeId, vendor_name AS VendorName,main_contact AS VendorMainContact,vendor_phone AS VendorPhone,vendor_fax AS VendorFax, vendor_email AS VendorEmail,vendor_status AS VendorStatus,inactive_date AS InactiveDate,inactive_reason AS InactiveReason,is_mailing_address_physical1 AS IsMailingAddressPhysical1 , mpos_mro_cost AS MPOSMROCost, mall_mro_cost AS MALLMROCost, is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM vendors WHERE vendor_type_id = 1 AND is_archived = 0 ORDER BY vendor_name";
            // string sqlQuery = "SELECT vendors.vendor_id AS VendorId , vendors.vendor_type_id AS VendorTypeId, vendors.vendor_name AS VendorName,vendors.main_contact AS VendorMainContact,vendors.vendor_phone AS VendorPhone,vendors.vendor_fax AS VendorFax, vendors.vendor_email AS VendorEmail,vendors.vendor_status AS VendorStatus,vendors.inactive_date AS InactiveDate,vendors.inactive_reason AS InactiveReason,vendors.is_mailing_address_physical1 AS IsMailingAddressPhysical1 , mpos_mro_cost AS MPOSMROCost, vendors.mall_mro_cost AS MALLMROCost, vendors.is_synchronized AS IsSynchronized, vendors.is_archived AS IsArchived, vendors.created_on AS CreatedOn, vendors.created_by AS CreatedBy, vendors.last_modified_on AS LastModifiedOn, vendors.last_modified_by AS LastModifiedBy, neareset_zip_code.donor_zip,neareset_zip_code.nearest_vendor_zip from neareset_zip_code "
            //+ "inner join donors on neareset_zip_code.donor_zip =donors.donor_zip "
            //+ "inner join vendor_addresses on neareset_zip_code.nearest_vendor_zip =vendor_addresses.vendor_zip "
            //+ "inner join vendors on vendor_addresses.vendor_id =vendors.vendor_id "
            //+ "where donors.donor_zip = @DonorZip AND vendors.vendor_type_id = 1 AND vendors.is_archived = 0 ";

            //MySqlParameter[] param = new MySqlParameter[1];
            //param[0] = new MySqlParameter("@DonorZip", zipcode);

            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sqlQuery);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        public DataTable GetCollectionCenterVendorList(string zip)
        {
            string sqlQuery = "SELECT vendors.vendor_id AS VendorId , vendors.vendor_type_id AS VendorTypeId, vendors.vendor_name AS VendorName,vendors.main_contact AS VendorMainContact,vendors.vendor_phone AS VendorPhone,vendors.vendor_fax AS VendorFax, vendors.vendor_email AS VendorEmail,vendors.vendor_status AS VendorStatus,vendors.inactive_date AS InactiveDate,vendors.inactive_reason AS InactiveReason,vendors.is_mailing_address_physical1 AS IsMailingAddressPhysical1 , mpos_mro_cost AS MPOSMROCost, vendors.mall_mro_cost AS MALLMROCost, vendors.is_synchronized AS IsSynchronized, vendors.is_archived AS IsArchived, vendors.created_on AS CreatedOn, vendors.created_by AS CreatedBy, vendors.last_modified_on AS LastModifiedOn, vendors.last_modified_by AS LastModifiedBy "
                              + "from vendors  "
                              + "inner join vendor_addresses on vendors.vendor_id =vendor_addresses.vendor_id  "
                              + "where vendor_addresses.vendor_zip IN (" + zip + ") AND vendors.vendor_type_id = 1 AND vendors.is_archived = 0  ";

            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sqlQuery);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        public DataTable GetByEmail(string Email)
        {
            string sqlQuery = "SELECT vendor_id AS VendorId,vendor_type_id AS VendorTypeId,vendor_name AS VendorName,main_contact AS MainContact,vendor_phone AS VendorPhone, vendor_fax AS VendorFax, vendor_email AS VendorEmail, vendor_status AS VendoStatus, inactive_date AS InactiveDate, inactive_reason AS InactiveReason, is_mailing_address_physical1 AS IsMailingAddressPhysical1, mpos_mro_cost AS MPOSMROCost,mall_mro_cost AS MALLMROCost, is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM vendors WHERE LOWER(vendor_email) = LOWER(@VendorEmail)";

            MySqlParameter[] param = new MySqlParameter[1];
            param[0] = new MySqlParameter("@VendorEmail", Email);

            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sqlQuery, param);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        public DataTable GetList(Dictionary<string, string> searchParam)
        {
            string generalSearch = string.Empty;
            string generalSearch1 = string.Empty;

            string sqlQuery = "SELECT vendors.vendor_id AS VendorId , vendors.vendor_type_id AS VendorTypeId,vendors.vendor_name AS VendorName,vendors.main_contact AS VendorMainContact,vendorAddresses.vendor_city AS VendorCity, vendorAddresses.vendor_state AS VendorState,vendors.vendor_phone AS VendorPhone, vendors.vendor_fax AS VendorFax, vendors.vendor_email AS VendorEmail, vendors.vendor_status AS VendorStatus, vendors.inactive_date AS InactiveDate,vendors.inactive_reason AS InactiveReason,vendors.is_mailing_address_physical1 AS IsMailingAddressPhysical1 , vendors.mpos_mro_cost AS MPOSMROCost, vendors.mall_mro_cost AS MALLMROCost, vendors.is_synchronized AS IsSynchronized, vendors.is_archived AS IsArchived, vendors.created_on AS CreatedOn, vendors.created_by AS CreatedBy, vendors.last_modified_on AS LastModifiedOn, vendors.last_modified_by AS LastModifiedBy from vendors vendors left outer join vendor_addresses vendorAddresses "
                              + "ON vendors.vendor_id = vendorAddresses.vendor_id "
                              + "WHERE vendorAddresses.address_type_id='1' and vendors.is_archived = 0 ";

            List<MySqlParameter> param = new List<MySqlParameter>();

            bool isInActiveFlag = false;
            bool isSearchKeyword = false;

            foreach (KeyValuePair<string, string> searchItem in searchParam)
            {
                if (searchItem.Key == "GeneralSearch")
                {
                    generalSearch += "vendors.vendor_name LIKE @SearchKeyword OR "
                                + "vendors.vendor_type_id LIKE @SearchKeyword OR "
                                + "vendors.main_contact LIKE @SearchKeyword OR "
                                + "vendors.vendor_phone LIKE @SearchKeyword OR "
                                + "vendors.vendor_fax LIKE @SearchKeyword OR "
                                + "vendors.vendor_email LIKE @SearchKeyword OR "
                                + "vendorAddresses.vendor_city LIKE @SearchKeyword OR "
                                + "vendorAddresses.vendor_state LIKE @SearchKeyword ";

                    param.Add(new MySqlParameter("@SearchKeyword", searchItem.Value));
                    isSearchKeyword = true;
                }
                else if (searchItem.Key == "VendorType")
                {
                    if (searchItem.Value != "All")
                    {
                        sqlQuery += "AND vendors.vendor_type_id = @VendorType ";
                        VendorTypes vendorType = VendorTypes.None;

                        if (searchItem.Value == "Collection Center")
                        {
                            vendorType = VendorTypes.CollectionCenter;
                            generalSearch1 += "OR vendors.vendor_type_id LIKE @SearchKeyword ";
                        }
                        else if (searchItem.Value == "Lab")
                        {
                            vendorType = VendorTypes.Lab;
                            generalSearch1 += "OR vendors.vendor_type_id LIKE @SearchKeyword ";
                        }
                        else if (searchItem.Value == "MRO")
                        {
                            vendorType = VendorTypes.MRO;
                            generalSearch1 += "OR vendors.vendor_type_id LIKE @SearchKeyword ";
                        }

                        param.Add(new MySqlParameter("@VendorType", (int)vendorType));
                    }
                }
                else if (searchItem.Key == "IncludeInactive")
                {
                    if (Convert.ToBoolean(searchItem.Value))
                    {
                        isInActiveFlag = true;
                    }
                }
            }

            if (!isInActiveFlag)
            {
                sqlQuery += " AND vendors.vendor_status = '1' ";
            }

            if (generalSearch != string.Empty && isSearchKeyword)
            {
                sqlQuery += "AND (" + generalSearch + generalSearch1 + ") ";
            }

            sqlQuery += "ORDER BY vendors.vendor_name";

            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sqlQuery, param.ToArray());
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        public DataTable GetListByVendorType(VendorTypes vendorType)
        {
            string sqlQuery = "SELECT vendor_id AS VendorId , vendor_type_id AS VendorTypeId,vendor_name AS VendorName,main_contact AS VendorMainContact,vendor_phone AS VendorPhone,vendor_fax AS VendorFax, vendor_email AS VendorEmail,vendor_status AS VendorStatus,inactive_date AS InactiveDate,inactive_reason AS InactiveReason,is_mailing_address_physical1 AS IsMailingAddressPhysical1 , mpos_mro_cost AS MPOSMROCost, mall_mro_cost AS MALLMROCost, is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM vendors WHERE is_archived = 0 AND vendor_type_id = @VendorTypeId ORDER BY vendor_name";

            MySqlParameter[] param = new MySqlParameter[1];
            param[0] = new MySqlParameter("@VendorTypeId", (int)vendorType);

            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sqlQuery, param);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        public DataTable GetVendorServices(int vendorId)
        {
            string sqlQuery = "SELECT vendor_service_id AS VendorServiceId FROM vendor_vendor_services WHERE vendor_id = @VendorId";

            MySqlParameter[] param = new MySqlParameter[1];
            param[0] = new MySqlParameter("@VendorId", vendorId);

            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sqlQuery, param);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        public DataTable GetVendorAddresses(int vendorId)
        {
            string sqlQuery = "SELECT vendor_address_id AS VendorAddressId, "
                            + "vendor_id AS VendorId, "
                            + "address_type_id AS AddressTypeId, "
                            + "vendor_address_1 AS VendorAddress1, "
                            + "vendor_address_2 AS VendorAddress2, "
                            + "vendor_city AS VendorCity, "
                            + "vendor_state AS VendorState, "
                            + "vendor_zip AS VendorZip, "
                            + "vendor_phone AS VendorPhone, "
                            + "vendor_fax AS VendorFax, "
                            + "vendor_email AS VendorEmail, "
                            + "is_synchronized AS IsSynchronized, "
                            + "created_on AS CreatedOn, "
                            + "created_by AS CreatedBy, "
                            + "last_modified_on AS LastModifiedOn, "
                            + "last_modified_by AS LastModifiedBy "
                            + "FROM vendor_addresses "
                            + "WHERE address_type_id = 1 AND vendor_id = @VendorId";

            MySqlParameter[] param = new MySqlParameter[1];
            param[0] = new MySqlParameter("@VendorId", vendorId);

            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sqlQuery, param);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        public Vendor GetAddress(int VendorId)
        {
            Vendor vendor = null;
            string sqlQuery = "SELECT vendor_id AS VendorId , vendor_type_id AS VendorTypeId,vendor_name AS VendorName,main_contact AS VendorMainContact,vendor_phone AS VendorPhone,vendor_fax AS VendorFax, vendor_email AS VendorEmail,vendor_status AS VendorStatus,inactive_date AS InactiveDate,inactive_reason AS InactiveReason , is_mailing_address_physical1 AS IsMailingAddressPhysical1, mpos_mro_cost AS MPOSMROCost, mall_mro_cost AS MALLMROCost, is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM vendors WHERE is_archived = 0 AND vendor_id = @VendorId";

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                MySqlParameter[] param = new MySqlParameter[1];
                param[0] = new MySqlParameter("@VendorId", VendorId);

                MySqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                if (dr.Read())
                {
                    vendor = new Vendor();
                    vendor.VendorId = (int)dr["VendorId"];
                    vendor.VendorTypeId = (VendorTypes)dr["VendorTypeId"];
                    vendor.VendorName = (string)dr["VendorName"];
                    vendor.VendorMainContact = (string)dr["VendorMainContact"];
                    vendor.VendorPhone = (string)dr["VendorPhone"];
                    vendor.VendorFax = (string)dr["VendorFax"];
                    vendor.VendorEmail = (string)dr["VendorEmail"];
                    vendor.VendorStatus = (VendorStatus)dr["VendorStatus"];
                    vendor.InactiveDate = Convert.ToDateTime(dr["InactiveDate"], this.Culture);
                    vendor.InactiveReason = (string)dr["InactiveReason"];
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
                }
                dr.Close();

                sqlQuery = "SELECT vendor_service_id AS VendorServiceId FROM vendor_vendor_services WHERE vendor_id = @VendorId";

                param = new MySqlParameter[1];
                param[0] = new MySqlParameter("@VendorId", VendorId);

                dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                if (vendor != null)
                {
                    while (dr.Read())
                    {
                        vendor.Services.Add((int)dr["VendorServiceId"]);
                    }
                }
                dr.Close();

                sqlQuery = "SELECT vendor_address_id AS VendorAddressId, "
                            + "vendor_id AS VendorId, "
                            + "address_type_id AS AddressTypeId, "
                            + "vendor_address_1 AS VendorAddress1, "
                            + "vendor_address_2 AS VendorAddress2, "
                            + "vendor_city AS VendorCity, "
                            + "vendor_state AS VendorState, "
                            + "vendor_zip AS VendorZip, "
                            + "vendor_phone AS VendorPhone, "
                            + "vendor_fax AS VendorFax, "
                            + "vendor_email AS VendorEmail, "
                            + "is_synchronized AS IsSynchronized, "
                            + "created_on AS CreatedOn, "
                            + "created_by AS CreatedBy, "
                            + "last_modified_on AS LastModifiedOn, "
                            + "last_modified_by AS LastModifiedBy "
                            + "FROM vendor_addresses "
                            + "WHERE vendor_id = @VendorId";

                param = new MySqlParameter[1];
                param[0] = new MySqlParameter("@VendorId", VendorId);

                dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                if (vendor != null)
                {
                    while (dr.Read())
                    {
                        VendorAddress address = new VendorAddress();

                        address.AddressId = (int)dr["VendorAddressId"];
                        address.VendorId = (int)dr["VendorId"];
                        address.AddressTypeId = (AddressTypes)dr["AddressTypeId"];
                        address.Address1 = (string)dr["VendorAddress1"];
                        address.Address2 = dr["VendorAddress2"].ToString();
                        address.City = (string)dr["VendorCity"];
                        address.State = (string)dr["VendorState"];
                        address.ZipCode = (string)dr["VendorZip"];
                        address.Phone = (string)dr["VendorPhone"];
                        address.Fax = dr["VendorFax"].ToString();
                        address.Email = (string)dr["VendorEmail"];
                        address.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                        address.CreatedOn = (DateTime)dr["CreatedOn"];
                        address.CreatedBy = (string)dr["CreatedBy"];
                        address.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                        address.LastModifiedBy = (string)dr["LastModifiedBy"];

                        vendor.Addresses.Add(address);

                        if (address.AddressTypeId == AddressTypes.PhysicalAddress1)
                        {
                            vendor.VendorCity = address.City;
                            vendor.VendorState = address.State;
                        }
                    }
                }
                dr.Close();
            }

            return vendor;
        }

        public DataTable GetVendorCollectionCenterList()
        {
            string sqlQuery = "SELECT vendor_id AS VendorId , vendor_type_id AS VendorTypeId, vendor_name AS VendorName,main_contact AS VendorMainContact,vendor_phone AS VendorPhone,vendor_fax AS VendorFax, vendor_email AS VendorEmail,vendor_status AS VendorStatus,inactive_date AS InactiveDate,inactive_reason AS InactiveReason,is_mailing_address_physical1 AS IsMailingAddressPhysical1 , mpos_mro_cost AS MPOSMROCost, mall_mro_cost AS MALLMROCost, is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM vendors WHERE vendor_type_id = 1 AND is_archived = 0 and vendor_status='1' ORDER BY vendor_name";
            // string sqlQuery = "SELECT vendors.vendor_id AS VendorId , vendors.vendor_type_id AS VendorTypeId, vendors.vendor_name AS VendorName,vendors.main_contact AS VendorMainContact,vendors.vendor_phone AS VendorPhone,vendors.vendor_fax AS VendorFax, vendors.vendor_email AS VendorEmail,vendors.vendor_status AS VendorStatus,vendors.inactive_date AS InactiveDate,vendors.inactive_reason AS InactiveReason,vendors.is_mailing_address_physical1 AS IsMailingAddressPhysical1 , mpos_mro_cost AS MPOSMROCost, vendors.mall_mro_cost AS MALLMROCost, vendors.is_synchronized AS IsSynchronized, vendors.is_archived AS IsArchived, vendors.created_on AS CreatedOn, vendors.created_by AS CreatedBy, vendors.last_modified_on AS LastModifiedOn, vendors.last_modified_by AS LastModifiedBy, neareset_zip_code.donor_zip,neareset_zip_code.nearest_vendor_zip from neareset_zip_code "
            //+ "inner join donors on neareset_zip_code.donor_zip =donors.donor_zip "
            //+ "inner join vendor_addresses on neareset_zip_code.nearest_vendor_zip =vendor_addresses.vendor_zip "
            //+ "inner join vendors on vendor_addresses.vendor_id =vendors.vendor_id "
            //+ "where donors.donor_zip = @DonorZip AND vendors.vendor_type_id = 1 AND vendors.is_archived = 0 ";

            //MySqlParameter[] param = new MySqlParameter[1];
            //param[0] = new MySqlParameter("@DonorZip", zipcode);

            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sqlQuery);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        public DataTable sorting(string VendorType, string searchparam, bool active, string getInActive = null)
        {
            string sql = string.Empty;
            string sqlQuery = "SELECT vendors.vendor_id AS VendorId , vendors.vendor_type_id AS VendorTypeId,vendors.vendor_name AS VendorName,vendors.main_contact AS VendorMainContact,vendorAddresses.vendor_city AS VendorCity, vendorAddresses.vendor_state AS VendorState,vendors.vendor_phone AS VendorPhone, vendors.vendor_fax AS VendorFax, vendors.vendor_email AS VendorEmail, vendors.vendor_status AS VendorStatus, vendors.inactive_date AS InactiveDate,vendors.inactive_reason AS InactiveReason,vendors.is_mailing_address_physical1 AS IsMailingAddressPhysical1 , vendors.mpos_mro_cost AS MPOSMROCost, vendors.mall_mro_cost AS MALLMROCost, vendors.is_synchronized AS IsSynchronized, vendors.is_archived AS IsArchived, vendors.created_on AS CreatedOn, vendors.created_by AS CreatedBy, vendors.last_modified_on AS LastModifiedOn, vendors.last_modified_by AS LastModifiedBy from vendors vendors left outer join vendor_addresses vendorAddresses "
                              + "ON vendors.vendor_id = vendorAddresses.vendor_id "
                              + "WHERE vendorAddresses.address_type_id='1' and vendors.is_archived = 0 ";

            if (active == false)
            {
                sqlQuery += "AND vendors.vendor_status = b'1' ";
            }
            if (VendorType != "All")
            {
                VendorTypes vendorType = VendorTypes.None;
                //    sqlQuery += "AND vendors.vendor_type_id = 0 ";
                if (VendorType == "Collection Center")
                {
                    vendorType = VendorTypes.CollectionCenter;
                    sqlQuery += "AND vendors.vendor_type_id = 1  ";
                }
                else if (VendorType == "Lab")
                {
                    vendorType = VendorTypes.Lab;
                    sqlQuery += "AND vendors.vendor_type_id = 2  ";
                }
                else if (VendorType == "MRO")
                {
                    vendorType = VendorTypes.MRO;
                    sqlQuery += "AND vendors.vendor_type_id = 3 ";
                }
            }

            if (searchparam == "vendorName")
            {
                if (getInActive == "1")
                {
                    sql = "ORDER BY vendors.vendor_name";
                }
                else
                {
                    sql = "ORDER BY vendors.vendor_name desc";
                }
            }
            if (searchparam == "vendorTypes")
            {
                if (getInActive == "1")
                {
                    sql = "ORDER BY vendors.vendor_type_id";
                }
                else
                {
                    sql = "ORDER BY vendors.vendor_type_id desc";
                }
            }
            if (searchparam == "vendorMainName")
            {
                if (getInActive == "1")
                {
                    sql = "ORDER BY vendors.main_contact";
                }
                else
                {
                    sql = "ORDER BY vendors.main_contact desc";
                }
            }
            if (searchparam == "vendorEmail")
            {
                if (getInActive == "1")
                {
                    sql = "ORDER BY vendors.vendor_email";
                }
                else
                {
                    sql = "ORDER BY vendors.vendor_email desc";
                }
            }
            if (searchparam == "city")
            {
                if (getInActive == "1")
                {
                    sql = "ORDER BY vendorAddresses.vendor_city";
                }
                else
                {
                    sql = "ORDER BY vendorAddresses.vendor_city desc";
                }
            }
            if (searchparam == "state")
            {
                if (getInActive == "1")
                {
                    sql = "ORDER BY vendorAddresses.vendor_state";
                }
                else
                {
                    sql = "ORDER BY vendorAddresses.vendor_state desc";
                }
            }
            if (searchparam == "isActive")
            {
                if (getInActive == "1")
                {
                    sql = "ORDER BY vendors.vendor_status";
                }
                else
                {
                    sql = "ORDER BY vendors.vendor_status desc";
                }
            }

            sqlQuery = sqlQuery + sql;

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sqlQuery);
                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion Public Methods
    }
}