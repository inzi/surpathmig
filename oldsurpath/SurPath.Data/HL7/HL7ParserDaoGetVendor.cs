using MySql.Data.MySqlClient;
using SurPath.Entity;
using SurPath.Enum;
using SurPath.MySQLHelper;
using System;
using System.Data;

namespace SurPath.Data
{
    /// <summary>
    /// Gets Vendor
    /// </summary>
    public partial class HL7ParserDao : DataObject
    {
        private Vendor GetVendor(int VendorId, MySqlTransaction trans)
        {
            Vendor vendor = null;
            string sqlQuery = "SELECT vendor_id AS VendorId , vendor_type_id AS VendorTypeId,vendor_name AS VendorName,main_contact AS VendorMainContact,vendor_phone AS VendorPhone,vendor_fax AS VendorFax, vendor_email AS VendorEmail,vendor_status AS VendorStatus,inactive_date AS InactiveDate,inactive_reason AS InactiveReason , is_mailing_address_physical1 AS IsMailingAddressPhysical1, mpos_mro_cost AS MPOSMROCost, mall_mro_cost AS MALLMROCost, is_synchronized AS IsSynchronized, is_archived AS IsArchived, created_on AS CreatedOn, created_by AS CreatedBy, last_modified_on AS LastModifiedOn, last_modified_by AS LastModifiedBy FROM vendors WHERE is_archived = 0 AND vendor_id = @VendorId";

            MySqlParameter[] param = new MySqlParameter[1];
            param[0] = new MySqlParameter("@VendorId", VendorId);

            MySqlDataReader dr = SqlHelper.ExecuteReader(trans, CommandType.Text, sqlQuery, param);

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

            return vendor;
        }
    }
}