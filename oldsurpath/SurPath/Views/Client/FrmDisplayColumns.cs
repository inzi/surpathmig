using iTextSharp.text;
using iTextSharp.text.pdf;

//using Microsoft.Office.Interop.Excel;
using SurPath.Business;
using SurPath.Enum;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;

//using SpreadsheetLight;
//using System.IO.Packaging;
//using DocumentFormat.OpenXml.Spreadsheet;

namespace SurPath
{
    public partial class FrmDisplayColumns : Form
    {
        #region Private Variables

        private System.Data.DataTable dtDonors = null;

        private Dictionary<string, string> searchParamExport = new Dictionary<string, string>();

        #endregion Private Variables

        #region Constructor

        public FrmDisplayColumns(Dictionary<string, string> searchParam)
        {
            searchParamExport = searchParam;
            InitializeComponent();
        }

        #endregion Constructor

        #region Event Methods

        private void FrmDisplayColumns_Load(object sender, EventArgs e)
        {
            if (rbAll.Checked == true)
            {
                for (int i = 0; i < chkColumnList.Items.Count; i++)
                {
                    chkColumnList.SetItemChecked(i, true);
                    rbAll.Checked = true;
                    chkColumnList.Enabled = false;
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (ValidateControls())
                {
                    if (rbExcel.Checked == true || rbCSV.Checked == true || rbWord.Checked == true)
                    {
                        //if (IsInstalled() == true)
                        //{
                        if (ExportData())
                        {
                            this.DialogResult = System.Windows.Forms.DialogResult.OK;
                            this.Close();
                        }
                        //}
                        //else
                        //{
                        //    //MessageBox.Show("Cannot export the file in this format because of the unavailable software.");
                        //    // return false;
                        //}
                    }
                    else if (rbPDF.Checked == true)
                    {
                        if (ExportData())
                        {
                            this.DialogResult = System.Windows.Forms.DialogResult.OK;
                            this.Close();
                        }
                    }
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void chkColumnList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            //if (e.NewValue.ToString() == "Checked")
            //{
            //    // first get all the checked items
            //    foreach (object checkeditem in chkColumnList.CheckedItems)
            //    {
            //        string checkItem = chkColumnList.GetItemCheckState(chkColumnList.Items.IndexOf(checkeditem)).ToString();
            //        if (checkItem == "Checked")
            //        {
            //            for (int i = 0; i < chkColumnList.CheckedItems.Count; i++)
            //            {
            //                selectedItems = chkColumnList.CheckedItems[i].ToString();
            //            }
            //        }

            //    }

            //    gvFieldList.DataSource = selectedItems;
        }

        private void rbtAll_CheckedChanged(object sender, EventArgs e)
        {
            rbAll.CausesValidation = false;
        }

        private void rbtSelection_CheckedChanged(object sender, EventArgs e)
        {
            rbSelect.CausesValidation = false;
        }

        private void chkColumnList_SelectedIndexChanged(object sender, EventArgs e)
        {
            chkColumnList.CausesValidation = false;
        }

        private void rbExcel_CheckedChanged(object sender, EventArgs e)
        {
            rbExcel.CausesValidation = false;
        }

        private void rbCSV_CheckedChanged(object sender, EventArgs e)
        {
            rbCSV.CausesValidation = false;
        }

        private void rbPDF_CheckedChanged(object sender, EventArgs e)
        {
            rbPDF.CausesValidation = false;
        }

        private void rbWord_CheckedChanged(object sender, EventArgs e)
        {
            rbWord.CausesValidation = false;
        }

        private void btnBrowse_TextChanged(object sender, EventArgs e)
        {
            btnBrowse.CausesValidation = false;
        }

        private void btnOk_TextChanged(object sender, EventArgs e)
        {
            btnOk.CausesValidation = false;
        }

        private void btnClose_TextChanged(object sender, EventArgs e)
        {
            btnClose.CausesValidation = false;
        }

        private void FrmDisplayColumns_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private void rbAll_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                for (int i = 0; i < chkColumnList.Items.Count; i++)
                {
                    chkColumnList.SetItemChecked(i, true);
                    rbAll.Checked = true;
                    chkColumnList.Enabled = false;
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void rbSelect_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                for (int i = 0; i < chkColumnList.Items.Count; i++)
                {
                    chkColumnList.SetItemChecked(i, false);
                    rbSelect.Checked = true;
                    chkColumnList.Enabled = true;
                }
                chkColumnList.Focus();

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        #endregion Event Methods

        #region Private Methods

        private bool ExportData()
        {
            try
            {
                string strsql = string.Empty;
                if (!ValidateControls())
                {
                    return false;
                }

                #region CheckBoxCheckedItems

                //Checkbox Checked Items
                if (chkColumnList.CheckedItems.Count > 0)
                {
                    for (int i = 0; i < chkColumnList.CheckedItems.Count; i++)
                    {
                        chkColumnList.SelectedItems.Clear();

                        if (chkColumnList.CheckedItems[i].ToString() == "First Name")
                        {
                            strsql = "donors.donor_first_name AS FirstName, ";
                        }
                        else if (chkColumnList.CheckedItems[i].ToString() == "Last Name")
                        {
                            strsql += "donors.donor_last_name AS LastName, ";
                        }
                        else if (chkColumnList.CheckedItems[i].ToString() == "SSN")
                        {
                            strsql += "donors.donor_ssn AS SSN, ";
                        }
                        else if (chkColumnList.CheckedItems[i].ToString() == "DOB")
                        {
                            strsql += "donors.donor_date_of_birth AS DonorDOB, ";
                        }
                        else if (chkColumnList.CheckedItems[i].ToString() == "Specimen ID")
                        {
                            strsql += "(select GROUP_CONCAT(donor_test_info_test_categories.specimen_id SEPARATOR ', ') from donor_test_info_test_categories where donor_test_info_test_categories.donor_test_info_id = donor_test_info.donor_test_info_id) AS SpecimenID, ";
                        }
                        else if (chkColumnList.CheckedItems[i].ToString() == "Specimen Date")
                        {
                            strsql += "donor_test_info.screening_time AS TestSpecimenDate, ";
                        }
                        else if (chkColumnList.CheckedItems[i].ToString() == "Client")
                        {
                            strsql += "clients.client_name AS Client, ";
                        }
                        else if (chkColumnList.CheckedItems[i].ToString() == "Department")
                        {
                            strsql += "client_departments.department_name AS Department, ";
                        }
                        else if (chkColumnList.CheckedItems[i].ToString() == "Status")
                        {
                            strsql += "donor_test_info.test_status AS TestStatus, ";
                        }
                        else if (chkColumnList.CheckedItems[i].ToString() == "Payment Mode")
                        {
                            strsql += "donor_test_info.payment_method_id AS PaymentMethodId, ";
                        }
                        else if (chkColumnList.CheckedItems[i].ToString() == "Amount")
                        {
                            strsql += "donor_test_info.total_payment_amount AS Amount, ";
                        }
                        else if (chkColumnList.CheckedItems[i].ToString() == "Result")
                        {
                            strsql += "donor_test_info.test_overall_result AS Result, ";
                        }
                        else if (chkColumnList.CheckedItems[i].ToString() == "Test Reason")
                        {
                            strsql += "donor_test_info.reason_for_test_id AS ReasonForTestId, ";
                        }
                        else if (chkColumnList.CheckedItems[i].ToString() == "Donor City")
                        {
                            strsql += "donors.donor_city as DonorCity, ";
                        }
                        else if (chkColumnList.CheckedItems[i].ToString() == "Zip Code")
                        {
                            strsql += "donors.donor_zip as ZipCode, ";
                        }
                        else if (chkColumnList.CheckedItems[i].ToString() == "MRO Type")
                        {
                            strsql += "donor_test_info.mro_type_id AS MROTypeId, ";
                        }
                        else if (chkColumnList.CheckedItems[i].ToString() == "Payment Type")
                        {
                            strsql += "donor_test_info.payment_type_id AS PaymentTypeId, ";
                        }
                    }
                    strsql = strsql.Remove(strsql.LastIndexOf(','));
                    DonorBL donorBL = new DonorBL();
                    dtDonors = donorBL.DisplayColumns(strsql, searchParamExport);

                    #endregion CheckBoxCheckedItems

                    #region DataTable Columns Name

                    if (dtDonors.Columns.Count > 0)
                    {
                        if (dtDonors.Columns["FirstName"] != null)
                        {
                            if (dtDonors.Columns["FirstName"].ColumnName.ToString() == "FirstName")
                            {
                                dtDonors.Columns.Add("First Name", typeof(String));
                                dtDonors.Columns["FirstName"].ColumnMapping = MappingType.Hidden;
                            }
                        }

                        if (dtDonors.Columns["LastName"] != null)
                        {
                            if (dtDonors.Columns["LastName"].ColumnName.ToString() == "LastName")
                            {
                                dtDonors.Columns.Add("Last Name", typeof(String));
                                dtDonors.Columns["LastName"].ColumnMapping = MappingType.Hidden;
                            }
                        }

                        if (dtDonors.Columns["DonorDOB"] != null)
                        {
                            if (dtDonors.Columns["DonorDOB"].ColumnName.ToString() == "DonorDOB")
                            {
                                dtDonors.Columns.Add("DOB", typeof(String));
                                dtDonors.Columns["DonorDOB"].ColumnMapping = MappingType.Hidden;
                            }
                            //dtDonors.Columns.Remove("DonorDOB");
                        }

                        if (dtDonors.Columns["SpecimenID"] != null)
                        {
                            if (dtDonors.Columns["SpecimenID"].ColumnName.ToString() == "SpecimenID")
                            {
                                dtDonors.Columns.Add("Specimen ID", typeof(String));
                                dtDonors.Columns["SpecimenID"].ColumnMapping = MappingType.Hidden;
                            }
                        }

                        if (dtDonors.Columns["TestSpecimenDate"] != null)
                        {
                            if (dtDonors.Columns["TestSpecimenDate"].ColumnName.ToString() == "TestSpecimenDate")
                            {
                                dtDonors.Columns.Add("Specimen Date", typeof(String));
                                dtDonors.Columns["TestSpecimenDate"].ColumnMapping = MappingType.Hidden;
                                //  dtDonors.Columns.Remove("TestSpecimenDate");
                            }
                        }

                        if (dtDonors.Columns["TestStatus"] != null)
                        {
                            if (dtDonors.Columns["TestStatus"].ColumnName.ToString() == "TestStatus")
                            {
                                dtDonors.Columns.Add("Status", typeof(String));
                                dtDonors.Columns["TestStatus"].ColumnMapping = MappingType.Hidden;
                                // dtDonors.Columns.Remove("TestStatus");
                            }
                        }
                        //if (dtDonors.Columns["TestStatus"] != null)
                        //{
                        //    if (dtDonors.Columns["TestStatus"].ColumnName.ToString() == "TestStatus")
                        //    {
                        //        dtDonors.Columns.Add("DonorRegistrationStatus", typeof(int));
                        //        dtDonors.Columns["DonorRegistrationStatus"].ColumnMapping = MappingType.Hidden;
                        //        // dtDonors.Columns.Remove("TestStatus");
                        //    }
                        //}

                        if (dtDonors.Columns["PaymentMethodId"] != null)
                        {
                            if (dtDonors.Columns["PaymentMethodId"].ColumnName.ToString() == "PaymentMethodId")
                            {
                                dtDonors.Columns.Add("Payment Mode", typeof(String));
                                dtDonors.Columns["PaymentMethodId"].ColumnMapping = MappingType.Hidden;
                                // dtDonors.Columns.Remove("PaymentMethodId");
                            }
                        }

                        if (dtDonors.Columns["ReasonForTestId"] != null)
                        {
                            if (dtDonors.Columns["ReasonForTestId"].ColumnName.ToString() == "ReasonForTestId")
                            {
                                dtDonors.Columns.Add("Test Reason", typeof(String));
                                dtDonors.Columns["ReasonForTestId"].ColumnMapping = MappingType.Hidden;
                                // dtDonors.Columns.Remove("ReasonForTestId");
                            }
                        }
                        if (dtDonors.Columns["MROTypeId"] != null)
                        {
                            if (dtDonors.Columns["MROTypeId"].ColumnName.ToString() == "MROTypeId")
                            {
                                dtDonors.Columns.Add("MRO Type", typeof(String));
                                dtDonors.Columns["MROTypeId"].ColumnMapping = MappingType.Hidden;
                                // dtDonors.Columns.Remove("MROTypeId");
                            }
                        }
                        if (dtDonors.Columns["PaymentTypeId"] != null)
                        {
                            if (dtDonors.Columns["PaymentTypeId"].ColumnName.ToString() == "PaymentTypeId")
                            {
                                dtDonors.Columns.Add("Payment Type", typeof(String));
                                dtDonors.Columns["PaymentTypeId"].ColumnMapping = MappingType.Hidden;
                                //  dtDonors.Columns.Remove("PaymentTypeId");
                            }
                        }

                        if (dtDonors.Columns["DonorCity"] != null)
                        {
                            if (dtDonors.Columns["DonorCity"].ColumnName.ToString() == "DonorCity")
                            {
                                dtDonors.Columns.Add("Donor City", typeof(String));
                                dtDonors.Columns["DonorCity"].ColumnMapping = MappingType.Hidden;
                            }
                        }

                        if (dtDonors.Columns["ZipCode"] != null)
                        {
                            if (dtDonors.Columns["ZipCode"].ColumnName.ToString() == "ZipCode")
                            {
                                dtDonors.Columns.Add("Zip Code", typeof(String));
                                dtDonors.Columns["ZipCode"].ColumnMapping = MappingType.Hidden;
                            }
                        }

                        if (dtDonors.Columns["Result"] != null && dtDonors.Columns["Result"].ToString() != string.Empty)
                        {
                            if (dtDonors.Columns["Result"].ColumnName.ToString() == "Result")
                            {
                                dtDonors.Columns.Add("Test Result", typeof(String));
                                dtDonors.Columns["Result"].ColumnMapping = MappingType.Hidden;
                                //  dtDonors.Columns.Remove("PaymentTypeId");
                            }
                        }
                    }

                    #endregion DataTable Columns Name

                    #region DataTable Rows Name

                    if (dtDonors.Rows.Count > 0)
                    {
                        foreach (DataColumn column in dtDonors.Columns)
                        {
                            for (int i = 0; i < dtDonors.Rows.Count; i++)
                            {
                                if (column.ToString() == "FirstName")
                                {
                                    if (dtDonors.Rows[i]["FirstName"].ToString() != null && dtDonors.Rows[i]["FirstName"].ToString() != string.Empty)
                                    {
                                        dtDonors.Rows[i]["First Name"] = dtDonors.Rows[i]["FirstName"].ToString(); ;
                                    }
                                }
                                else if (column.ToString() == "LastName")
                                {
                                    if (dtDonors.Rows[i]["LastName"].ToString() != null && dtDonors.Rows[i]["LastName"].ToString() != string.Empty)
                                    {
                                        dtDonors.Rows[i]["Last Name"] = dtDonors.Rows[i]["LastName"].ToString(); ;
                                    }
                                }
                                else if (column.ToString() == "SSN")
                                {
                                    if (dtDonors.Rows[i]["SSN"].ToString() != null && dtDonors.Rows[i]["SSN"].ToString() != string.Empty)
                                    {
                                        if (dtDonors.Rows[i]["SSN"].ToString().Length == 11)
                                        {
                                            // dtDonors.Rows[i]["SSN"] = "***-**-" + dtDonors.Rows[i]["SSN"].ToString().Substring(7);
                                            dtDonors.Rows[i]["SSN"] = dtDonors.Rows[i]["SSN"].ToString();
                                        }
                                    }
                                }
                                else if (column.ToString() == "DonorDOB")
                                {
                                    if (dtDonors.Rows[i]["DonorDOB"].ToString() != null && dtDonors.Rows[i]["DonorDOB"].ToString() != string.Empty)
                                    {
                                        DateTime dob = Convert.ToDateTime(dtDonors.Rows[i]["DonorDOB"]);
                                        if (dob != DateTime.MinValue)
                                        {
                                            string DOB = dob.ToString("MM/dd/yyyy");
                                            dtDonors.Rows[i]["DOB"] = DOB.ToString();
                                            //String.Format("{0:MM/dd/yy}", dob);
                                        }
                                    }
                                }
                                else if (column.ToString() == "SpecimenID")
                                {
                                    if (dtDonors.Rows[i]["SpecimenID"].ToString() != null && dtDonors.Rows[i]["SpecimenID"].ToString() != string.Empty)
                                    {
                                        dtDonors.Rows[i]["Specimen ID"] = dtDonors.Rows[i]["SpecimenID"].ToString(); ;
                                    }
                                }
                                else if (column.ToString() == "TestSpecimenDate")
                                {
                                    if (dtDonors.Rows[i]["TestSpecimenDate"].ToString() != null && dtDonors.Rows[i]["TestSpecimenDate"].ToString() != string.Empty)
                                    {
                                        DateTime specimenDate = Convert.ToDateTime(dtDonors.Rows[i]["TestSpecimenDate"]);
                                        if (specimenDate != DateTime.MinValue)
                                        {
                                            string SpecimenDate = specimenDate.ToString("MM/dd/yyyy");
                                            dtDonors.Rows[i]["Specimen Date"] = SpecimenDate.ToString();
                                        }
                                    }
                                }
                                else if (column.ToString() == "MROTypeId")
                                {
                                    if (dtDonors.Rows[i]["MROTypeId"].ToString() != null && dtDonors.Rows[i]["MROTypeId"].ToString() != string.Empty)
                                    {
                                        ClientMROTypes clientMRoTypes = (ClientMROTypes)((int)dtDonors.Rows[i]["MROTypeId"]);
                                        dtDonors.Rows[i]["MRO Type"] = clientMRoTypes.ToString();
                                        //dtDonors.Columns["MROTypeId"].ColumnMapping = MappingType.Hidden;
                                    }
                                }
                                else if (column.ToString() == "PaymentTypeId")
                                {
                                    if (dtDonors.Rows[i]["PaymentTypeId"].ToString() != null && dtDonors.Rows[i]["PaymentTypeId"].ToString() != string.Empty)
                                    {
                                        ClientPaymentTypes clientPaymentTypes = (ClientPaymentTypes)((int)dtDonors.Rows[i]["PaymentTypeId"]);
                                        dtDonors.Rows[i]["Payment Type"] = clientPaymentTypes.ToString();
                                        // dtDonors.Columns["PaymentTypeId"].ColumnMapping = MappingType.Hidden;
                                    }
                                }
                                else if (column.ToString() == "PaymentMethodId")
                                {
                                    if (dtDonors.Rows[i]["PaymentMethodId"].ToString() != null && dtDonors.Rows[i]["PaymentMethodId"].ToString() != string.Empty)
                                    {
                                        PaymentMethod paymentMethod = (PaymentMethod)((int)dtDonors.Rows[i]["PaymentMethodId"]);
                                        dtDonors.Rows[i]["Payment Mode"] = paymentMethod.ToString();
                                        // dtDonors.Columns["PaymentMethodId"].ColumnMapping = MappingType.Hidden;
                                    }
                                }
                                else if (column.ToString() == "TestStatus")
                                {
                                    if (dtDonors.Rows[i]["TestStatus"].ToString() != null && dtDonors.Rows[i]["TestStatus"].ToString() != string.Empty)
                                    {
                                        DonorRegistrationStatus status = (DonorRegistrationStatus)((int)dtDonors.Rows[i]["TestStatus"]);
                                        dtDonors.Rows[i]["Status"] = status.ToDescriptionString();
                                        // dtDonors.Columns["TestStatus"].ColumnMapping = MappingType.Hidden;
                                    }
                                    else if (dtDonors.Rows[i]["DonorRegistrationStatus"].ToString() != null && dtDonors.Rows[i]["DonorRegistrationStatus"].ToString() != string.Empty)
                                    {
                                        DonorRegistrationStatus status = (DonorRegistrationStatus)((int)dtDonors.Rows[i]["DonorRegistrationStatus"]);
                                        dtDonors.Rows[i]["Status"] = status.ToDescriptionString();
                                        dtDonors.Columns["DonorRegistrationStatus"].ColumnMapping = MappingType.Hidden;
                                    }
                                }
                                else if (column.ToString() == "ReasonForTestId")
                                {
                                    if (dtDonors.Rows[i]["ReasonForTestId"].ToString() != null && dtDonors.Rows[i]["ReasonForTestId"].ToString() != string.Empty)
                                    {
                                        TestInfoReasonForTest testInfoReasonForTest = (TestInfoReasonForTest)((int)dtDonors.Rows[i]["ReasonForTestId"]);
                                        dtDonors.Rows[i]["Test Reason"] = testInfoReasonForTest.ToString();
                                        //   dtDonors.Columns["ReasonForTestId"].ColumnMapping = MappingType.Hidden;
                                    }
                                }
                                else if (column.ToString() == "DonorCity")
                                {
                                    if (dtDonors.Rows[i]["DonorCity"].ToString() != null && dtDonors.Rows[i]["DonorCity"].ToString() != string.Empty)
                                    {
                                        dtDonors.Rows[i]["Donor City"] = dtDonors.Rows[i]["DonorCity"].ToString(); ;
                                    }
                                }
                                else if (column.ToString() == "ZipCode")
                                {
                                    if (dtDonors.Rows[i]["ZipCode"].ToString() != null && dtDonors.Rows[i]["ZipCode"].ToString() != string.Empty)
                                    {
                                        dtDonors.Rows[i]["Zip Code"] = dtDonors.Rows[i]["ZipCode"].ToString(); ;
                                    }
                                }
                                else if (column.ToString() == "Result")
                                {
                                    if (dtDonors.Rows[i]["Result"].ToString() != null && dtDonors.Rows[i]["Result"].ToString() != string.Empty)
                                    {
                                        OverAllTestResult overAllTestResult = (OverAllTestResult)((int)dtDonors.Rows[i]["Result"]);
                                        if (overAllTestResult.ToString() != "None")
                                        {
                                            dtDonors.Rows[i]["Test Result"] = overAllTestResult.ToString();
                                        }
                                        else
                                        {
                                            dtDonors.Rows[i]["Test Result"] = " ";
                                        }
                                    }
                                }
                            }
                        }
                    }

                    #endregion DataTable Rows Name

                    #region DataTable Remove Columns

                    if (dtDonors.Columns["FirstName"] != null)
                    {
                        dtDonors.Columns.Remove("FirstName");
                    }
                    if (dtDonors.Columns["LastName"] != null)
                    {
                        dtDonors.Columns.Remove("LastName");
                    }
                    if (dtDonors.Columns["DonorDOB"] != null)
                    {
                        dtDonors.Columns.Remove("DonorDOB");
                    }
                    if (dtDonors.Columns["SpecimenID"] != null)
                    {
                        dtDonors.Columns.Remove("SpecimenID");
                    }
                    if (dtDonors.Columns["TestSpecimenDate"] != null)
                    {
                        dtDonors.Columns.Remove("TestSpecimenDate");
                    }
                    if (dtDonors.Columns["PaymentMethodId"] != null)
                    {
                        dtDonors.Columns.Remove("PaymentMethodId");
                    }
                    if (dtDonors.Columns["TestStatus"] != null)
                    {
                        dtDonors.Columns.Remove("TestStatus");
                    }
                    if (dtDonors.Columns["DonorInitialClientId"] != null)
                    {
                        dtDonors.Columns.Remove("DonorInitialClientId");
                    }
                    if (dtDonors.Columns["DonorRegistrationStatus"] != null)
                    {
                        dtDonors.Columns.Remove("DonorRegistrationStatus");
                    }
                    if (dtDonors.Columns["ReasonForTestId"] != null)
                    {
                        dtDonors.Columns.Remove("ReasonForTestId");
                    }
                    if (dtDonors.Columns["MROTypeId"] != null)
                    {
                        dtDonors.Columns.Remove("MROTypeId");
                    }
                    if (dtDonors.Columns["PaymentTypeId"] != null)
                    {
                        dtDonors.Columns.Remove("PaymentTypeId");
                    }
                    if (dtDonors.Columns["IsWalkinDonor"] != null)
                    {
                        dtDonors.Columns.Remove("IsWalkinDonor");
                    }
                    if (dtDonors.Columns["InstantTestResult"] != null)
                    {
                        dtDonors.Columns.Remove("InstantTestResult");
                    }
                    if (dtDonors.Columns["DonorCity"] != null)
                    {
                        dtDonors.Columns.Remove("DonorCity");
                    }
                    if (dtDonors.Columns["ZipCode"] != null)
                    {
                        dtDonors.Columns.Remove("ZipCode");
                    }
                    if (dtDonors.Columns["Result"] != null)
                    {
                        dtDonors.Columns.Remove("Result");
                    }

                    #endregion DataTable Remove Columns

                    //string[] existingCol = { "DonorDOB", "TestSpecimenDate", "PaymentMethodId", "TestStatus", "ReasonForTestId", "MROTypeId", "PaymentTypeId" };
                    //if(existingCol!=null)
                    //foreach (string colName in existingCol)
                    //{
                    //    dtDonors.Columns.Remove(colName);
                    //}

                    if (rbAll.Checked == true)
                    {
                        if (dtDonors.Columns["First Name"] != null)
                        {
                            dtDonors.Columns["First Name"].SetOrdinal(0);
                        }
                        if (dtDonors.Columns["Last Name"] != null)
                        {
                            dtDonors.Columns["Last Name"].SetOrdinal(1);
                        }
                        if (dtDonors.Columns["DOB"] != null)
                        {
                            dtDonors.Columns["DOB"].SetOrdinal(3);
                        }
                        if (dtDonors.Columns["Specimen ID"] != null)
                        {
                            dtDonors.Columns["Specimen ID"].SetOrdinal(4);
                        }
                        if (dtDonors.Columns["Specimen Date"] != null)
                        {
                            dtDonors.Columns["Specimen Date"].SetOrdinal(5);
                        }
                        if (dtDonors.Columns["MRO Type"] != null)
                        {
                            dtDonors.Columns["MRO Type"].SetOrdinal(15);
                        }
                        if (dtDonors.Columns["Payment Type"] != null)
                        {
                            dtDonors.Columns["Payment Type"].SetOrdinal(16);
                        }
                        if (dtDonors.Columns["Payment Mode"] != null)
                        {
                            dtDonors.Columns["Payment Mode"].SetOrdinal(9);
                        }
                        if (dtDonors.Columns["Amount"] != null)
                        {
                            dtDonors.Columns["Amount"].SetOrdinal(10);
                        }
                        if (dtDonors.Columns["Test Result"] != null)
                        {
                            dtDonors.Columns["Test Result"].SetOrdinal(11);
                        }
                        if (dtDonors.Columns["Status"] != null)
                        {
                            dtDonors.Columns["Status"].SetOrdinal(8);
                        }
                        if (dtDonors.Columns["Test Reason"] != null)
                        {
                            dtDonors.Columns["Test Reason"].SetOrdinal(12);
                        }
                        if (dtDonors.Columns["Donor City"] != null)
                        {
                            dtDonors.Columns["Donor City"].SetOrdinal(13);
                        }
                        if (dtDonors.Columns["Zip Code"] != null)
                        {
                            dtDonors.Columns["Zip Code"].SetOrdinal(14);
                        }
                    }
                    else if (rbSelect.Checked == true)
                    {
                        //
                        if (dtDonors.Columns["First Name"] != null)
                        {
                            dtDonors.Columns["First Name"].SetOrdinal(0);
                        }
                        if (dtDonors.Columns["Last Name"] != null)
                        {
                            dtDonors.Columns["Last Name"].SetOrdinal(1);
                        }
                    }

                    gvFieldList.DataSource = dtDonors;

                    #region Export the File

                    if (rbExcel.Checked == true)
                    {
                        //ExportDTToExcel(dtDonors, txtBrowse.Text);
                        SaveFileDialog sfd = new SaveFileDialog();
                        sfd.Filter = "Excel Documents (*.xls)|*.xls";
                        sfd.FileName = "export.xls";
                        if (sfd.ShowDialog() == DialogResult.OK)
                        {
                            // ToCsV(dtDonors, sfd.FileName);
                            if (IsInstalled() == true)
                            {
                                ExporTtoExcel(sfd.FileName);
                            }
                            else
                            {
                                Spire.Xls.Workbook workbook = new Spire.Xls.Workbook();
                                Spire.Xls.Worksheet worksheet = workbook.Worksheets[0];
                                worksheet.InsertDataTable(dtDonors, true, 2, 1, -1, -1);
                                workbook.SaveToFile(sfd.FileName);
                                string Filename = sfd.FileName;
                                System.Diagnostics.Process.Start(Filename);
                                //MessageBox.Show("Cannot export the file in this format because of the unavailable software.");
                                //return false;
                            }
                        }
                    }
                    else if (rbCSV.Checked == true)
                    {
                        SaveFileDialog sfd = new SaveFileDialog();
                        sfd.Filter = "CSV Files (*.csv)|*.csv";
                        sfd.FileName = "export.csv";
                        if (sfd.ShowDialog() == DialogResult.OK)
                        {
                            if (IsInstalled() == true)
                            {
                                CreateCSVFile(gvFieldList, sfd.FileName); // Here dvwACH is your grid view name
                            }
                            else
                            {
                                ToCsV(dtDonors, sfd.FileName);
                            }
                        }
                    }
                    else if (rbPDF.Checked == true)
                    {
                        if (chkColumnList.CheckedItems.Count <= 7)
                        {
                            SaveFileDialog sfd = new SaveFileDialog();
                            sfd.Filter = "PDF Files (*.pdf)|*.pdf";
                            sfd.FileName = "export.pdf";
                            if (sfd.ShowDialog() == DialogResult.OK)
                            {
                                // ExportToPDF();
                                SaveDataGridViewToPDF(sfd.FileName);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Maximum of 7 fields are allowed to be exported in PDF. ");
                            return false;
                        }
                    }
                    else if (rbWord.Checked == true)
                    {
                        if (IsInstalled() == true)
                        {
                            if (chkColumnList.CheckedItems.Count <= 7)
                            {
                                SaveFileDialog sfd = new SaveFileDialog();
                                sfd.Filter = "Word Documents (*.doc)|*.doc";
                                sfd.FileName = "export.doc";
                                string filename = sfd.FileName.Trim().Substring(sfd.FileName.Trim().LastIndexOf('\\') + 1);
                                if (sfd.ShowDialog() == DialogResult.OK)
                                {
                                    // ToCsV(gvFieldList, sfd.FileName); // Here dvwACH is your grid view name

                                    ExportToWord(dtDonors, true, sfd.FileName);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Maximum of 7 fields are allowed to be exported in Word. ");
                                return false;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Cannot export the file in this format because of the unavailable software.");
                            return false;
                        }
                    }

                    #endregion Export the File
                }
                else
                {
                    MessageBox.Show("Select an item from the List ");
                    return false;
                }

                ResetControlsCauseValidation();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private void ResetControlsCauseValidation()
        {
            foreach (System.Windows.Forms.Control ctrl in this.Controls)
            {
                ctrl.CausesValidation = true;
            }
        }

        private bool ValidateControls()
        {
            try
            {
                if (chkColumnList.CheckedItems.Count == 0)
                {
                    MessageBox.Show("Select a ColumnList.");
                    chkColumnList.Focus();
                    chkColumnList.Enabled = true;
                    return false;
                }
                if (rbAll.Checked == false && rbSelect.Checked == false)
                {
                    MessageBox.Show("Choose All or Selected Option.");
                    //  rbSelect.Focus();
                    return false;
                }
                if (rbExcel.Checked == false && rbCSV.Checked == false && rbPDF.Checked == false && rbWord.Checked == false)
                {
                    MessageBox.Show("Choose the export type.");
                    // rbExcel.Focus();
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            return true;
        }

        #endregion Private Methods

        #region Public Methods

        public bool IsInstalled()
        {
            Type officeType = Type.GetTypeFromProgID("Excel.Application");
            Type officeType1 = Type.GetTypeFromProgID("Word.Application");
            //RegistryKey adobe = Registry.LocalMachine.OpenSubKey("Software").OpenSubKey("Adobe");
            bool retValue = true;
            if (officeType == null)
            {
                retValue = false;
            }
            else if (officeType1 == null)
            {
                retValue = false;
            }
            //else if (adobe == null)
            //{
            //    retValue = false;
            //}
            return retValue;
        }

        public void ExporTtoExcel(string Filename)
        {
            //SLDocument sl = new SLDocument();
            ////Read data from grid and write it in excel
            //// DataGrid dg = new DataGrid();
            //for (int rows = 0; rows < dtDonors.Rows.Count; rows++)
            //{
            //    for (int col = 0; col < dtDonors.Columns.Count; col++)
            //    {
            //        sl.SetCellValue(rows, col, dtDonors.Columns[rows].ColumnName.ToString());
            //    }
            //}
            //sl.SaveAs("Test.xlsx");

            try
            {
                // string Path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\PdfSample.pdf";
                // string Filename = "Sample.xlsx";

                Excel.ExcelUtlity obj = new Excel.ExcelUtlity();

                obj.WriteDataTableToExcel(dtDonors, "Donor Details", Filename, "Details");

                //  MessageBox.Show("Excel created");
                System.Diagnostics.Process.Start(Filename);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void CreateCSVFile(DataGridView dgv, string filename)
        {
            if (dgv.RowCount > 0)
            {
                string value = "";
                DataGridViewRow dr = new DataGridViewRow();
                // Create the CSV file to which grid data will be exported.
                StreamWriter sw = new StreamWriter(filename, false);

                for (int i = 0; i <= dgv.Columns.Count - 1; i++)
                {
                    if (i > 0)
                    {
                        sw.Write(",");
                    }
                    sw.Write(dgv.Columns[i].HeaderText);
                }

                sw.WriteLine();
                //write DataGridView rows to csv
                for (int j = 0; j <= dgv.Rows.Count - 1; j++)
                {
                    if (j > 0)
                    {
                        sw.WriteLine();
                    }

                    dr = dgv.Rows[j];

                    for (int i = 0; i <= dgv.Columns.Count - 1; i++)
                    {
                        if (i > 0)
                        {
                            sw.Write(",");
                        }

                        value = dr.Cells[i].Value.ToString();
                        //replace comma's with spaces
                        value = value.Replace(',', ' ');
                        //replace embedded newlines with spaces
                        value = value.Replace(Environment.NewLine, " ");
                        sw.Write(value);
                        System.Diagnostics.Process.Start(filename);
                    }
                }
                sw.Close();
            }
        }

        public void SaveDataGridViewToPDF(string Path)
        {
            FontFactory.RegisterDirectories();
            iTextSharp.text.Font myfont = FontFactory.GetFont("Tahoma", BaseFont.IDENTITY_H, 9, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);
            iTextSharp.text.Font myfonts = FontFactory.GetFont("Tahoma", BaseFont.IDENTITY_H, 9, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
            //Document pdfDoc = new Document(PageSize.A4,10f,10f,10f, 0f);
            Document pdfDoc = new Document(PageSize.A3, 0f, 0f, 30f, 30f);
            pdfDoc.Open();
            PdfWriter wri = PdfWriter.GetInstance(pdfDoc, new FileStream(Path, FileMode.Create));
            pdfDoc.Open();
            PdfPTable _mytable = new PdfPTable(dtDonors.Columns.Count);
            // _mytable.HorizontalAlignment = Element.ALIGN_LEFT;
            for (int j = 0; j < dtDonors.Columns.Count; ++j)
            {
                Phrase p = new Phrase(dtDonors.Columns[j].ColumnName, myfont);
                PdfPCell cell = new PdfPCell(p);
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                _mytable.AddCell(cell);
            }
            //-------------------------
            for (int i = 0; i < dtDonors.Rows.Count; ++i)
            {
                for (int j = 0; j < dtDonors.Columns.Count; ++j)
                {
                    Phrase p = new Phrase(dtDonors.Rows[i][j].ToString(), myfonts);
                    PdfPCell cell = new PdfPCell(p);
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cell.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    _mytable.AddCell(cell);
                }
            }
            //------------------------
            pdfDoc.Add(_mytable);
            pdfDoc.Close();
            System.Diagnostics.Process.Start(Path);
        }

        public static bool ExportToWord(System.Data.DataTable dt, bool isShowWord, object fileName)
        {
            bool result = true;

            Object myobj = Missing.Value; ;

            if (dt == null || dt.Rows.Count == 0)
            {
                result = false;
            }
            else
            {
                // Create the Word application
                Word.Application word = new Word.Application();

                // Word document
                Word.Document mydoc = new Word.Document();
                mydoc = word.Documents.Add(ref myobj, ref myobj, ref myobj, ref myobj);
                word.Visible = isShowWord;
                mydoc.Select();
                Word.Selection mysel = word.Selection;

                // Mysel.InlineShapes.AddPicture (picName, ref myobj, ref myobj, ref myobj);
                object top = 100;
                object left = 300;
                object hw = 100;

                // Generate a Word table file
                Word.Table mytable = mydoc.Tables.Add(mysel.Range, dt.Rows.Count, dt.Columns.Count, ref myobj, ref myobj);

                // Set the column width
                mytable.Columns.SetWidth(50, Word.WdRulerStyle.wdAdjustNone);

                // Output the column heading data
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    mytable.Cell(1, i + 1).Range.InsertAfter(dt.Columns[i].ColumnName);
                }

                // Output control records
                for (int i = 0; i < dt.Rows.Count - 1; i++)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        mytable.Cell(i + 2, j + 1).Range.InsertAfter(dt.Rows[i][j].ToString());
                    }
                }

                // mydoc.Shapes.AddPicture(picName, ref myobj, ref myobj, ref left, ref top, ref hw, ref hw, ref myobj);
                // Save text
                mydoc.SaveAs(ref fileName, ref myobj, ref myobj, ref myobj, ref myobj, ref myobj,
                ref myobj, ref myobj, ref myobj, ref myobj, ref myobj, ref myobj,
                ref myobj, ref myobj, ref myobj, ref myobj);
            }

            return result;
        }

        #endregion Public Methods

        public static void ToCsV(DataTable dtDonors, string filename)
        {
            StreamWriter sw = new StreamWriter(filename, false);
            //headers
            for (int i = 0; i < dtDonors.Columns.Count; i++)
            {
                sw.Write(dtDonors.Columns[i]);
                if (i < dtDonors.Columns.Count - 1)
                {
                    sw.Write(",");
                }
            }
            sw.Write(sw.NewLine);
            foreach (DataRow dr in dtDonors.Rows)
            {
                for (int i = 0; i < dtDonors.Columns.Count; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        string value = dr[i].ToString();
                        if (value.Contains(','))
                        {
                            value = String.Format("\"{0}\"", value);
                            sw.Write(value);
                        }
                        else
                        {
                            sw.Write(dr[i].ToString());
                        }
                    }
                    if (i < dtDonors.Columns.Count - 1)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);
            }
            sw.Close();

            System.Diagnostics.Process.Start(filename);
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (fbdExport.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    //txtBrowse.Text = fbdExport.SelectedPath.ToString().Trim();
                    Cursor.Current = Cursors.Default;
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        public void ExportToPDF()
        {
            #region pdf

            //    try
            //    {
            //        //  string Path = txtBrowse.Text;
            //        string filename = "Test.pdf";
            //        FontFactory.RegisterDirectories();
            //        iTextSharp.text.Font myfont = FontFactory.GetFont("Tahoma", BaseFont.IDENTITY_H, 12, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
            //        Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);

            //        pdfDoc.Open();
            //        PdfWriter wri = PdfWriter.GetInstance(pdfDoc, new FileStream(filename, FileMode.Create));
            //        pdfDoc.Open();
            //        PdfPTable _mytable = new PdfPTable(dtDonors.Columns.Count);

            //        for (int j = 0; j < dtDonors.Columns.Count; ++j)
            //        {
            //            Phrase p = new Phrase(dtDonors.Columns[j].ColumnName, myfont);
            //            PdfPCell cell = new PdfPCell(p);
            //            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            //            cell.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            //            _mytable.AddCell(cell);
            //        }
            //        //-------------------------
            //        for (int i = 0; i < dtDonors.Rows.Count - 1; ++i)
            //        {
            //            for (int j = 0; j < dtDonors.Columns.Count; ++j)
            //            {
            //                Phrase p = new Phrase(dtDonors.Rows[i][j].ToString(), myfont);
            //                PdfPCell cell = new PdfPCell(p);
            //                cell.HorizontalAlignment = Element.ALIGN_CENTER;
            //                cell.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            //                _mytable.AddCell(cell);
            //            }
            //        }
            //        //------------------------
            //        pdfDoc.Add(_mytable);

            //        pdfDoc.Close();
            //        wri.Close();
            //        System.Diagnostics.Process.Start(filename);
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.Message);
            //    }

            #endregion pdf
        }

        public void ExportDTToExcel(System.Data.DataTable dt, string FileName)
        {
            //FileName = "Sample.xlsx";
            //Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
            //app.Visible = false;
            //Workbook wb = app.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
            //Worksheet ws = (Worksheet)wb.ActiveSheet;
            //// Headers.
            //for (int i = 1; i < dt.Columns.Count; i++)
            //{
            //    ws.Cells[1, i] = dt.Columns[i].ColumnName;
            //}
            //// Content.
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    for (int j = 1; j < dt.Columns.Count; j++)
            //    {
            //        ws.Cells[i + 2, j] = dt.Rows[i][j].ToString();
            //    }
            //}
            //// Lots of options here. See the documentation.
            //wb.SaveAs(FileName);
            //wb.Close();
            //app.Quit();
        }
    }
}