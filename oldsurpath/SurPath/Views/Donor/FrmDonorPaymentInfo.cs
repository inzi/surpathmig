using SurPath.Business;
using SurPath.Entity;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace SurPath
{
    public partial class FrmDonorPaymentInfo : Form
    {
        // private bool haveEditRights = false;

        #region Constructor

        public FrmDonorPaymentInfo()
        {
            InitializeComponent();
        }

        #endregion Constructor

        #region Event Methods

        private void FrmDonorPaymentInfo_Load(object sender, EventArgs e)
        {
            try
            {
                InitializeControls();
                LoadDonorPaymentInfo(0);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred while loading the page.");
            }
        }

        private void FrmDonorPaymentInfo_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.frmMain.frmDonorPaymentInfo = null;
        }

        private void dgvDonorPaymentInfo_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
        }

        #endregion Event Methods

        #region Private Methods

        private void InitializeControls()
        {
            dgvDonorPaymentInfo.AutoGenerateColumns = false;
            cmbPaymentMethod.SelectedIndex = 0;
            txtFromDate.Text = string.Empty;
            txtToDate.Text = string.Empty;
            //if (!(Program.currentUserName.ToUpper() == Program.adminUsername.ToUpper() || Program.currentUserName.ToUpper() == Program.adminUsername1.ToUpper()))
            //{
            //    //VENDOR_ADD
            //    DataRow[] vendorInfoAdd = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.VENDOR_ADD.ToDescriptionString() + "'");

            //    if (vendorInfoAdd.Length > 0)
            //    {
            //        tsbNew.Visible = true;
            //    }
            //    else
            //    {
            //        tsbNew.Visible = false;
            //    }
            //}
        }

        private void LoadDonorPaymentInfo(int selectedIndex)
        {
            DonorBL donorBL = new DonorBL();
            List<DonorTestInfo> donorList = null;
            Dictionary<string, string> searchParam = new Dictionary<string, string>();

            DateTime fromDate = Convert.ToDateTime(txtFromDate.Text);
            DateTime toDate = Convert.ToDateTime(txtToDate.Text);
            string param = fromDate.ToString("MM/dd/yyyy") + "#" + toDate.ToString("MM/dd/yyyy");

            searchParam.Add("PaymentMethod", cmbPaymentMethod.Text);
            searchParam.Add("dates", param);
            // searchParam.Add("EndDate", "%" + toDate.ToString("MM/dd/yyyy") + "%");

            donorList = donorBL.GetDonorPaymentList(searchParam);
            dgvDonorPaymentInfo.DataSource = donorList;

            if (dgvDonorPaymentInfo.Rows.Count > 0)
            {
                if (selectedIndex > dgvDonorPaymentInfo.Rows.Count - 1)
                {
                    selectedIndex = dgvDonorPaymentInfo.Rows.Count - 1;
                }
                dgvDonorPaymentInfo.Rows[selectedIndex].Selected = true;
                dgvDonorPaymentInfo.Focus();
            }
        }

        #endregion Private Methods

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadDonorPaymentInfo(0);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                LoadDonorPaymentInfo(0);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void cmbPaymentMethod_TextChanged(object sender, EventArgs e)
        {
            cmbPaymentMethod.CausesValidation = false;
        }

        private void btnSearch_TextChanged(object sender, EventArgs e)
        {
            btnSearch.CausesValidation = false;
        }

        private void btnExport_TextChanged(object sender, EventArgs e)
        {
            btnExport.CausesValidation = false;
        }

        private void txtToDate_ValueChanged(object sender, EventArgs e)
        {
            txtFromDate.CausesValidation = false;
        }

        private void txtFromDate_ValueChanged(object sender, EventArgs e)
        {
            txtToDate.CausesValidation = false;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (printDialog1.ShowDialog() == DialogResult.OK)
            {
                printDocument1.Print();
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Bitmap dataGridViewImage = new Bitmap(this.dgvDonorPaymentInfo.Width, this.dgvDonorPaymentInfo.Height);

            dgvDonorPaymentInfo.DrawToBitmap(dataGridViewImage, new Rectangle(0, 0, this.dgvDonorPaymentInfo.Width, this.dgvDonorPaymentInfo.Height));

            e.Graphics.DrawImage(dataGridViewImage, 0, 0);
        }

        private void printDocument1_BeginPrint(object sender, PrintEventArgs e)
        {
        }
    }
}