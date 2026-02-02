using MySql.Data.MySqlClient;
using SurPath.Entity;
using SurPath.Enum;
using SurPath.MySQLHelper;
using System;
using System.Collections.Generic;
using System.Data;

namespace SurPath.Data
{
    public partial class DonorDao : DataObject
    {
        public DonorAccounting GetAccountingDetails(int donorId, int donorTestInfoId)
        {
            DonorAccounting donorAccounting = new DonorAccounting();
            double mrocost = 0.0;
            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                //UA Revenue - for the test
                string sqlQuery = "SELECT IFNULL(test_panel_price, 0) AS UARevenue FROM donor_test_info_test_categories "
                                    + "INNER JOIN donor_test_info ON donor_test_info.donor_test_info_id = donor_test_info_test_categories.donor_test_info_id "
                                    + "WHERE donor_test_info.donor_test_info_id = @DonorTestInfoId and donor_test_info.payment_status = 1 and test_category_id = 1";

                MySqlParameter[] param = new MySqlParameter[1];
                param[0] = new MySqlParameter("@DonorTestInfoId", donorTestInfoId);

                MySqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                if (dr.Read())
                {
                    if (dr["UARevenue"] != null && dr["UARevenue"] != DBNull.Value)
                    {
                        donorAccounting.UARevenue = (double)dr["UARevenue"];
                    }
                }
                dr.Close();

                //Hair Revenue - for the test
                sqlQuery = "SELECT IFNULL(test_panel_price, 0) AS HairRevenue FROM donor_test_info_test_categories "
                             + "INNER JOIN donor_test_info ON donor_test_info.donor_test_info_id = donor_test_info_test_categories.donor_test_info_id "
                             + "WHERE  donor_test_info.donor_test_info_id = @DonorTestInfoId and donor_test_info.payment_status = 1 and test_category_id = 2";

                param = new MySqlParameter[1];
                param[0] = new MySqlParameter("@DonorTestInfoId", donorTestInfoId);

                dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                if (dr.Read())
                {
                    if (dr["HairRevenue"] != null && dr["HairRevenue"] != DBNull.Value)
                    {
                        donorAccounting.HairRevenue = (double)dr["HairRevenue"];
                    }
                }
                dr.Close();

                //DNA Revenue - for the test
                sqlQuery = "SELECT IFNULL(test_panel_price, 0) AS DNARevenue FROM donor_test_info_test_categories "
                            + "INNER JOIN donor_test_info ON donor_test_info.donor_test_info_id = donor_test_info_test_categories.donor_test_info_id "
                            + "WHERE donor_test_info.donor_test_info_id = @DonorTestInfoId and donor_test_info.payment_status = 1 and test_category_id = 3";

                param = new MySqlParameter[1];
                param[0] = new MySqlParameter("@DonorTestInfoId", donorTestInfoId);

                dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                if (dr.Read())
                {
                    if (dr["DNARevenue"] != null && dr["DNARevenue"] != DBNull.Value)
                    {
                        donorAccounting.DNARevenue = (double)dr["DNARevenue"];
                    }
                }
                dr.Close();

                //Cost - for the test
                sqlQuery = "SELECT IFNULL(laboratory_cost, 0) AS LaboratoryCost, "
                            + "IFNULL(cup_cost, 0) AS CupCost, "
                            + "IFNULL(shipping_cost, 0) AS ShippingCost, "
                            + "IFNULL(vendor_cost, 0) AS VendorCost "
                            + "FROM donor_test_info "
                            + "WHERE donor_test_info_id = @DonorTestInfoId and donor_test_info.test_status >= 6";

                param = new MySqlParameter[1];
                param[0] = new MySqlParameter("@DonorTestInfoId", donorTestInfoId);

                dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                if (dr.Read())
                {
                    if (dr["LaboratoryCost"] != null && dr["LaboratoryCost"] != DBNull.Value)
                    {
                        donorAccounting.LaboratoryCost = (double)dr["LaboratoryCost"];
                    }

                    if (dr["CupCost"] != null && dr["CupCost"] != DBNull.Value)
                    {
                        donorAccounting.CupCost = (double)dr["CupCost"];
                    }
                    if (dr["ShippingCost"] != null && dr["ShippingCost"] != DBNull.Value)
                    {
                        donorAccounting.ShippingCost = (double)dr["ShippingCost"];
                    }
                    if (dr["VendorCost"] != null && dr["VendorCost"] != DBNull.Value)
                    {
                        donorAccounting.VendorCost = (double)dr["VendorCost"];
                    }
                }
                dr.Close();

                sqlQuery = "SELECT report_info.report_type AS ReportType,report_info.report_status AS ReportStatus,donor_test_info.mro_type_id AS MROType, "
                             + "IFNULL(mro_cost, 0) AS MROCost "
                             + "FROM donor_test_info "
                             + "INNER JOIN report_info ON report_info.donor_test_info_id = donor_test_info.donor_test_info_id "
                             + "WHERE donor_test_info.donor_test_info_id = @DonorTestInfoId and donor_test_info.test_status >= 6 and report_info.report_type = 1 and report_info.is_archived=0 ";

                param = new MySqlParameter[1];
                param[0] = new MySqlParameter("@DonorTestInfoId", donorTestInfoId);

                dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                if (dr.Read())
                {
                    if (dr["MROCost"] != null && dr["MROCost"] != DBNull.Value)
                    {
                        if (dr["MROType"].ToString() == "1" && (dr["ReportStatus"].ToString() == "0" || dr["ReportStatus"].ToString() == "2"))
                        {
                            donorAccounting.MROCost = 0.0;
                        }
                        else
                        {
                            donorAccounting.MROCost = (double)dr["MROCost"];
                        }
                    }
                }
                dr.Close();

                //UA Revenue - Cumulative
                sqlQuery = "SELECT SUM(IFNULL(test_panel_price, 0)) AS UARevenue FROM donor_test_info_test_categories "
                            + "INNER JOIN donor_test_info ON donor_test_info.donor_test_info_id = donor_test_info_test_categories.donor_test_info_id "
                            + "WHERE donor_test_info.donor_id = @DonorId and donor_test_info.payment_status = 1 and test_category_id = 1";

                param = new MySqlParameter[1];
                param[0] = new MySqlParameter("@DonorId", donorId);

                dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                if (dr.Read())
                {
                    if (dr["UARevenue"] != null && dr["UARevenue"] != DBNull.Value)
                    {
                        donorAccounting.CumulativeUARevenue = (double)dr["UARevenue"];
                    }
                }
                dr.Close();

                //Hair Revenue - Cumulative
                sqlQuery = "SELECT SUM(IFNULL(test_panel_price, 0)) AS HairRevenue FROM donor_test_info_test_categories "
                            + "INNER JOIN donor_test_info ON donor_test_info.donor_test_info_id = donor_test_info_test_categories.donor_test_info_id "
                            + "WHERE donor_test_info.donor_id = @DonorId and donor_test_info.payment_status =1 and test_category_id = 2";

                param = new MySqlParameter[1];
                param[0] = new MySqlParameter("@DonorId", donorId);

                dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                if (dr.Read())
                {
                    if (dr["HairRevenue"] != null && dr["HairRevenue"] != DBNull.Value)
                    {
                        donorAccounting.CumulativeHairRevenue = (double)dr["HairRevenue"];
                    }
                }
                dr.Close();

                //DNA Revenue - Cumulative
                sqlQuery = "SELECT SUM(IFNULL(test_panel_price, 0)) AS DNARevenue FROM donor_test_info_test_categories "
                            + "INNER JOIN donor_test_info ON donor_test_info.donor_test_info_id = donor_test_info_test_categories.donor_test_info_id "
                            + "WHERE donor_test_info.donor_id = @DonorId and donor_test_info.payment_status =1 and test_category_id = 3";

                param = new MySqlParameter[1];
                param[0] = new MySqlParameter("@DonorId", donorId);

                dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                if (dr.Read())
                {
                    if (dr["DNARevenue"] != null && dr["DNARevenue"] != DBNull.Value)
                    {
                        donorAccounting.CumulativeDNARevenue = (double)dr["DNARevenue"];
                    }
                }
                dr.Close();

                //Cost - Cumulative
                sqlQuery = "SELECT SUM(IFNULL(laboratory_cost, 0)) AS LaboratoryCost, "
                            //+ "SUM(IFNULL(mro_cost, 0)) AS MROCost, "
                            + "SUM(IFNULL(cup_cost, 0)) AS CupCost, "
                            + "SUM(IFNULL(shipping_cost, 0)) AS ShippingCost, "
                            + "SUM(IFNULL(vendor_cost, 0)) AS VendorCost "
                            + "FROM donor_test_info "
                            + "WHERE donor_id = @DonorId  and donor_test_info.test_status >= 6";

                param = new MySqlParameter[1];
                param[0] = new MySqlParameter("@DonorId", donorId);

                dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                if (dr.Read())
                {
                    if (dr["LaboratoryCost"] != null && dr["LaboratoryCost"] != DBNull.Value)
                    {
                        donorAccounting.CumulativeLaboratoryCost = (double)dr["LaboratoryCost"];
                    }
                    //if (dr["MROCost"] != null && dr["MROCost"] != DBNull.Value)
                    //{
                    //    donorAccounting.CumulativeMROCost = (double)dr["MROCost"];
                    //}
                    if (dr["CupCost"] != null && dr["CupCost"] != DBNull.Value)
                    {
                        donorAccounting.CumulativeCupCost = (double)dr["CupCost"];
                    }
                    if (dr["ShippingCost"] != null && dr["ShippingCost"] != DBNull.Value)
                    {
                        donorAccounting.CumulativeShippingCost = (double)dr["ShippingCost"];
                    }
                    if (dr["VendorCost"] != null && dr["VendorCost"] != DBNull.Value)
                    {
                        donorAccounting.CumulativeVendorCost = (double)dr["VendorCost"];
                    }
                }
                dr.Close();

                sqlQuery = "SELECT SUM(IFNULL(mro_cost, 0)) AS MROCost "
                            + "FROM donor_test_info "
                            + "INNER JOIN report_info ON report_info.donor_test_info_id = donor_test_info.donor_test_info_id "
                            + "INNER JOIN donors ON donors.donor_id = donor_test_info.donor_id "
                            + "WHERE donors.donor_id = @DonorId and donor_test_info.test_status >= 6 and donor_test_info.mro_type_id = 1 and "
                            + "report_info.report_type = 1 and report_info.report_status = 1 and report_info.is_archived = 0 ";
                param = new MySqlParameter[1];
                param[0] = new MySqlParameter("@DonorId", donorId);

                dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                if (dr.Read())
                {
                    if (dr["MROCost"] != null && dr["MROCost"] != DBNull.Value)
                    {
                        mrocost = (double)dr["MROCost"];
                    }
                }
                dr.Close();

                sqlQuery = "SELECT SUM(IFNULL(mro_cost, 0)) AS MROCost "
                           + "FROM donor_test_info "
                           + "INNER JOIN donors ON donors.donor_id = donor_test_info.donor_id "
                           + "WHERE donors.donor_id = @DonorId and donor_test_info.test_status >= 6 and donor_test_info.mro_type_id = 2";
                param = new MySqlParameter[1];
                param[0] = new MySqlParameter("@DonorId", donorId);

                dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                if (dr.Read())
                {
                    if (dr["MROCost"] != null && dr["MROCost"] != DBNull.Value)
                    {
                        mrocost += (double)dr["MROCost"];
                    }
                }
                dr.Close();

                donorAccounting.CumulativeMROCost = mrocost;
            }

            return donorAccounting;
        }

        public DonorAccounting GetAccountingDetails(DateTime dtStart, DateTime dtEnd, string clientDepartmentList)
        {
            DonorAccounting donorAccounting = new DonorAccounting();
            double cumrocost = 0.0;
            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                //UA Revenue - for the test
                string sqlQuery = "SELECT SUM(IFNULL(donor_test_info_test_categories.test_panel_price, 0)) AS UARevenue FROM donor_test_info_test_categories "
                            + "INNER JOIN donor_test_info ON donor_test_info_test_categories.donor_test_info_id = donor_test_info.donor_test_info_id "
                            + "WHERE (CAST(donor_test_info.test_requested_date AS DATE) BETWEEN CAST(@StartDate AS DATE) AND CAST(@EndDate AS DATE)) "
                            + "AND donor_test_info.payment_status = 1 "
                            + "AND donor_test_info.client_department_id IN (" + clientDepartmentList + ") "
                            + "AND donor_test_info_test_categories.test_category_id = 1";

                MySqlParameter[] param = new MySqlParameter[2];
                param[0] = new MySqlParameter("@StartDate", dtStart.ToString("yyyy-MM-dd"));
                param[1] = new MySqlParameter("@EndDate", dtEnd.ToString("yyyy-MM-dd"));

                MySqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                if (dr.Read())
                {
                    if (dr["UARevenue"] != null && dr["UARevenue"] != DBNull.Value)
                    {
                        donorAccounting.UARevenue = (double)dr["UARevenue"];
                    }
                }
                dr.Close();

                //Hair Revenue - for the test
                sqlQuery = "SELECT SUM(IFNULL(donor_test_info_test_categories.test_panel_price, 0)) AS HairRevenue FROM donor_test_info_test_categories "
                            + "INNER JOIN donor_test_info ON donor_test_info_test_categories.donor_test_info_id = donor_test_info.donor_test_info_id "
                            + "WHERE (CAST(donor_test_info.test_requested_date AS DATE) BETWEEN CAST(@StartDate AS DATE) AND CAST(@EndDate AS DATE)) "
                            + "AND donor_test_info.payment_status = 1 "
                            + "AND donor_test_info.client_department_id IN (" + clientDepartmentList + ") "
                            + "AND donor_test_info_test_categories.test_category_id = 2";

                param = new MySqlParameter[2];
                param[0] = new MySqlParameter("@StartDate", dtStart.ToString("yyyy-MM-dd"));
                param[1] = new MySqlParameter("@EndDate", dtEnd.ToString("yyyy-MM-dd"));

                dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                if (dr.Read())
                {
                    if (dr["HairRevenue"] != null && dr["HairRevenue"] != DBNull.Value)
                    {
                        donorAccounting.HairRevenue = (double)dr["HairRevenue"];
                    }
                }
                dr.Close();

                //DNA Revenue - for the test
                sqlQuery = "SELECT SUM(IFNULL(donor_test_info_test_categories.test_panel_price, 0)) AS DNARevenue FROM donor_test_info_test_categories "
                            + "INNER JOIN donor_test_info ON donor_test_info_test_categories.donor_test_info_id = donor_test_info.donor_test_info_id "
                            + "WHERE (CAST(donor_test_info.test_requested_date AS DATE) BETWEEN CAST(@StartDate AS DATE) AND CAST(@EndDate AS DATE)) "
                            + "AND donor_test_info.payment_status = 1 "
                            + "AND donor_test_info.client_department_id IN (" + clientDepartmentList + ") "
                            + "AND donor_test_info_test_categories.test_category_id = 3";

                param = new MySqlParameter[2];
                param[0] = new MySqlParameter("@StartDate", dtStart.ToString("yyyy-MM-dd"));
                param[1] = new MySqlParameter("@EndDate", dtEnd.ToString("yyyy-MM-dd"));

                dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                if (dr.Read())
                {
                    if (dr["DNARevenue"] != null && dr["DNARevenue"] != DBNull.Value)
                    {
                        donorAccounting.DNARevenue = (double)dr["DNARevenue"];
                    }
                }
                dr.Close();

                //Cost - for the test
                sqlQuery = "SELECT SUM(IFNULL(laboratory_cost, 0)) AS LaboratoryCost, "
                            // + "SUM(IFNULL(mro_cost, 0)) AS MROCost, "
                            + "SUM(IFNULL(cup_cost, 0)) AS CupCost, "
                            + "SUM(IFNULL(shipping_cost, 0)) AS ShippingCost, "
                            + "SUM(IFNULL(vendor_cost, 0)) AS VendorCost "
                            + "FROM donor_test_info "
                            + "WHERE client_department_id IN (" + clientDepartmentList + ") and donor_test_info.test_status >= 6 "
                            + "AND (CAST(donor_test_info.test_requested_date AS DATE) BETWEEN CAST(@StartDate AS DATE) AND CAST(@EndDate AS DATE))";

                param = new MySqlParameter[2];
                param[0] = new MySqlParameter("@StartDate", dtStart.ToString("yyyy-MM-dd"));
                param[1] = new MySqlParameter("@EndDate", dtEnd.ToString("yyyy-MM-dd"));

                dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                if (dr.Read())
                {
                    if (dr["LaboratoryCost"] != null && dr["LaboratoryCost"] != DBNull.Value)
                    {
                        donorAccounting.LaboratoryCost = (double)dr["LaboratoryCost"];
                    }
                    //if (dr["MROCost"] != null && dr["MROCost"] != DBNull.Value)
                    //{
                    //    donorAccounting.MROCost = (double)dr["MROCost"];
                    //}
                    if (dr["CupCost"] != null && dr["CupCost"] != DBNull.Value)
                    {
                        donorAccounting.CupCost = (double)dr["CupCost"];
                    }
                    if (dr["ShippingCost"] != null && dr["ShippingCost"] != DBNull.Value)
                    {
                        donorAccounting.ShippingCost = (double)dr["ShippingCost"];
                    }
                    if (dr["VendorCost"] != null && dr["VendorCost"] != DBNull.Value)
                    {
                        donorAccounting.VendorCost = (double)dr["VendorCost"];
                    }
                }
                dr.Close();
                //Cost - for the test
                sqlQuery = "SELECT "
                            + "SUM(IFNULL(mro_cost, 0)) AS MROCost "
                            + "FROM donor_test_info "
                            + "INNER JOIN report_info ON report_info.donor_test_info_id = donor_test_info.donor_test_info_id "
                            + "INNER JOIN donors ON donors.donor_id = donor_test_info.donor_id "
                            + "WHERE donor_test_info.client_department_id IN (" + clientDepartmentList + ") and donor_test_info.test_status >= 6 and report_info.report_type=1 and report_info.report_status = 1 and report_info.is_archived = 0  "
                            + "AND (CAST(donor_test_info.test_requested_date AS DATE) BETWEEN CAST(@StartDate AS DATE) AND CAST(@EndDate AS DATE))";

                param = new MySqlParameter[2];
                param[0] = new MySqlParameter("@StartDate", dtStart.ToString("yyyy-MM-dd"));
                param[1] = new MySqlParameter("@EndDate", dtEnd.ToString("yyyy-MM-dd"));

                dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                if (dr.Read())
                {
                    if (dr["MROCost"] != null && dr["MROCost"] != DBNull.Value)
                    {
                        cumrocost = (double)dr["MROCost"];
                    }
                }
                dr.Close();
                sqlQuery = "SELECT "
                            + "SUM(IFNULL(mro_cost, 0)) AS MROCost "
                            + "FROM donor_test_info "
                            + "INNER JOIN donors ON donors.donor_id = donor_test_info.donor_id "
                            + "WHERE donor_test_info.client_department_id IN (" + clientDepartmentList + ") and donor_test_info.test_status >= 6 and donor_test_info.mro_type_id=2 "
                            + "AND (CAST(donor_test_info.test_requested_date AS DATE) BETWEEN CAST(@StartDate AS DATE) AND CAST(@EndDate AS DATE))";

                param = new MySqlParameter[2];
                param[0] = new MySqlParameter("@StartDate", dtStart.ToString("yyyy-MM-dd"));
                param[1] = new MySqlParameter("@EndDate", dtEnd.ToString("yyyy-MM-dd"));

                dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, param);

                if (dr.Read())
                {
                    if (dr["MROCost"] != null && dr["MROCost"] != DBNull.Value)
                    {
                        cumrocost += (double)dr["MROCost"];
                    }
                }
                dr.Close();

                donorAccounting.MROCost = cumrocost;
            }

            return donorAccounting;
        }

        public DataTable GetDonorPaymentList(Dictionary<string, string> searchParam)
        {
            string generalSearch = string.Empty;

            string sqlQuery = "SELECT donor_test_info.donor_id AS DonorId, "
                                + " donor_test_info.total_payment_amount AS TotalPaymentAmount, donor_test_info.payment_method_id AS PaymentMethodId,"
                                + " donor_test_info.payment_status AS PaymentStatus, donor_test_info.schedule_date AS ScheduleDate, "
                                + " donors.donor_first_name AS DonorFirstName, donors.donor_last_name AS DonorLastName, "
                                + " donor_test_info.created_on AS CreatedOn, donor_test_info.created_by AS CreatedBy, "
                                + " donor_test_info.last_modified_on AS LastModifiedOn, donor_test_info.last_modified_by AS LastModifiedBy "
                                + " FROM donor_test_info"
                                + " INNER JOIN donors ON donor_test_info.donor_id  = donors.donor_id "
                                + " WHERE donor_test_info.test_status >=3  ";

            List<MySqlParameter> param = new List<MySqlParameter>();
            bool isSearchKeyword = false;

            foreach (KeyValuePair<string, string> searchItem in searchParam)
            {
                if (searchItem.Key == "PaymentMethod")
                {
                    sqlQuery += "AND donor_test_info.payment_method_id = @PaymentMethodId ";
                    PaymentMethod paymentMethod = PaymentMethod.None;

                    if (searchItem.Value == "Cash")
                    {
                        paymentMethod = PaymentMethod.Cash;
                        sqlQuery += "AND donor_test_info.payment_method_id = @PaymentMethodId ";
                    }
                    else if (searchItem.Value == "Card")
                    {
                        paymentMethod = PaymentMethod.Card;
                        sqlQuery += "AND donor_test_info.payment_method_id = @PaymentMethodId ";
                    }
                    else if (searchItem.Value == "All")
                    {
                        string payment = PaymentMethod.Cash + "," + PaymentMethod.Card;
                        sqlQuery += "AND donor_test_info.payment_method_id != @PaymentMethodId";
                    }
                    param.Add(new MySqlParameter("@PaymentMethodId", (int)paymentMethod));

                    isSearchKeyword = true;
                }
                else if (searchItem.Key == "dates")
                {
                    sqlQuery += " AND (CAST(donor_test_info.schedule_date AS DATE) BETWEEN (@StartDate) AND (@EndDate)) ";

                    string[] dates = searchItem.Value.Split('#');
                    param.Add(new MySqlParameter("@StartDate", Convert.ToDateTime(dates[0]).ToString("yyyy-MM-dd")));
                    param.Add(new MySqlParameter("@EndDate", Convert.ToDateTime(dates[1]).ToString("yyyy-MM-dd")));

                    //param.Add(new MySqlParameter("@StartDate", searchItem.Value));
                    //param.Add(new MySqlParameter("@EndDate", searchItem.Value));
                }
            }
            //if (generalSearch != string.Empty && isSearchKeyword)
            //{
            //    sqlQuery += " And (" + generalSearch + ") ";
            //}
            sqlQuery += " ORDER BY donor_first_name";

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
    }
}