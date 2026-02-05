using MySql.Data.MySqlClient;
using SurPath.Entity.Master;
using SurPath.MySQLHelper;
using System;
using System.Data;

namespace SurPath.Data
{
    public class ZipCodeDao : DataObject
    {
        public int Insert(ZipCode zipCode)
        {
            int returnValue = 0;

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();
                MySqlTransaction trans = conn.BeginTransaction();
                try
                {
                    int returnval = 0;
                    string sqlQuery = "SELECT EXISTS(SELECT donor_zip FROM neareset_zip_code WHERE donor_zip = " + zipCode.Zip + ")";

                    returnval = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sqlQuery));
                    if (returnval == 0)
                    {
                        if (zipCode.NearestZip.Count > 0)
                        {
                            foreach (string nearestZipCode in zipCode.NearestZip)
                            {
                                sqlQuery = "INSERT INTO neareset_zip_code (donor_zip, nearest_vendor_zip) VALUES (";
                                sqlQuery += "@DonorZip, @NearestVendorZip)";

                                MySqlParameter[] param = new MySqlParameter[2];
                                param[0] = new MySqlParameter("@DonorZip", zipCode.Zip);
                                param[1] = new MySqlParameter("@NearestVendorZip", nearestZipCode);

                                SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);
                                returnValue = 1;
                            }
                        }
                    }
                    if (returnval == 1)
                    {
                        sqlQuery = "DELETE FROM neareset_zip_code "
                                          + "WHERE donor_zip = @DonorZip";

                        MySqlParameter[] param = new MySqlParameter[1];
                        param[0] = new MySqlParameter("@DonorZip", zipCode.Zip);

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);

                        if (zipCode.NearestZip.Count > 0)
                        {
                            foreach (string nearestZipCode in zipCode.NearestZip)
                            {
                                sqlQuery = "INSERT INTO neareset_zip_code (donor_zip, nearest_vendor_zip) VALUES (";
                                sqlQuery += "@DonorZip, @NearestVendorZip)";

                                param = new MySqlParameter[2];
                                param[0] = new MySqlParameter("@DonorZip", zipCode.Zip);
                                param[1] = new MySqlParameter("@NearestVendorZip", nearestZipCode);

                                SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlQuery, param);
                                returnValue = 1;
                            }
                        }
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

        public DataTable GetZip()
        {
            string sqlQuery = "SELECT zip_id AS ZipId, zip AS Zip, city AS City, state AS State FROM zip_codes ORDER BY zip";

            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sqlQuery);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
            throw new NotImplementedException();
        }

        public DataTable GetState()
        {
            string sqlQuery = "SELECT zip_id AS ZipId, zip AS Zip, city AS City, state AS State  FROM zip_codes  group by state ORDER BY state";

            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sqlQuery);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
            throw new NotImplementedException();
        }

        public DataTable GetZipcodeForState()
        {
            string sqlQuery = "SELECT zip_id AS ZipId, zip AS Zip, city AS City, state AS State FROM zip_codes ORDER BY zip";

            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sqlQuery);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
            throw new NotImplementedException();
        }

        public DataTable GetZipcodeById(int ID)
        {
            string sqlQuery = "SELECT zip AS Zip FROM zip_codes where zip_id = @ZipId";

            MySqlParameter[] param = new MySqlParameter[1];
            param[0] = new MySqlParameter("@ZipId", ID);

            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sqlQuery, param);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
            throw new NotImplementedException();
        }

        public ZipCode Get(int zipId)
        {
            ZipCode zipCode = null;
            string sqlQuery = "SELECT zip_id AS ZipId, zip AS Zip, city AS City, state AS State FROM zip_codes where zip_id = @ZipId ORDER BY state";

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                MySqlParameter[] param = new MySqlParameter[1];
                param[0] = new MySqlParameter("@ZipId", zipId);

                MySqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                if (dr.Read())
                {
                    zipCode = new ZipCode();
                    zipCode.ZipId = (int)dr["ZipId"];
                    zipCode.Zip = (string)dr["Zip"];
                    zipCode.State = (string)dr["State"];
                }
            }
            return zipCode;
        }

        public NearestZipCode LoadZip(string donorzip)
        {
            NearestZipCode nearestZipCode = null;
            string sqlQuery = "SELECT neareset_zip_code.donor_zip AS DonorZip, neareset_zip_code.nearest_vendor_zip AS NearestZip,zip_codes.zip_id AS VendorId FROM neareset_zip_code inner join zip_codes on neareset_zip_code.nearest_vendor_zip = zip_codes.zip WHERE donor_zip = @DonorZip";

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                MySqlParameter[] param = new MySqlParameter[1];
                param[0] = new MySqlParameter("@DonorZip", donorzip);

                MySqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);
                nearestZipCode = new NearestZipCode();
                while (dr.Read())
                {
                    if (dr["DonorZip"] != null)
                    {
                        nearestZipCode.NearestZip.Add((int)dr["VendorId"]);
                    }
                }
                return nearestZipCode;
            }
        }
    }
}