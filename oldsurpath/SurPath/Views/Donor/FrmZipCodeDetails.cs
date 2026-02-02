using SurPath.Business;
using SurPath.Business.Master;
using SurPath.Entity;
using SurPath.Entity.Master;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SurPath
{
    public partial class FrmZipCodeDetails : Form
    {
        private ZipCodeBL zipCodeBL = new ZipCodeBL();
        private VendorBL vendorBL = new VendorBL();

        public FrmZipCodeDetails()
        {
            InitializeComponent();
        }

        private void FrmZipCodeDetails_Load(object sender, EventArgs e)
        {
            LoadState();
            LoadZip();
            LoadZipCode();
            dgvVendorInfo.AutoGenerateColumns = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbState_SelectedIndexChanged(object sender, EventArgs e)
        {
            //LoadZipCode();
        }

        private void cmbZipcode_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.dgvVendorInfo.DataSource = null;
            this.dgvVendorInfo.Rows.Clear();

            if (cmbZipcode.SelectedIndex > 0)
            {
                string stt = string.Empty;
                int zipid = Convert.ToInt32(cmbZipcode.SelectedValue);
                ZipCode zipcodes = zipCodeBL.Get(zipid);
                stt = zipcodes.Zip.ToString();
                NearestZipCode nearestZipCode = zipCodeBL.LoadZip(stt);
                if (nearestZipCode != null)
                {
                    chkZipCodeList.ValueList = nearestZipCode.NearestZip;
                }

                //List<ZipCode> zipList = new List<ZipCode>();

                //zipList = zipCodeBL.GetZip();

                //cmbZipcode.DataSource = zipList;
                //cmbZipcode.ValueMember = "ZipId";
                //cmbZipcode.DisplayMember = "Zip";

                //cmbZipcode.SelectedValue = stt;
            }
        }

        private void LoadState()
        {
            try
            {
                List<ZipCode> stateList = new List<ZipCode>();

                stateList = zipCodeBL.GetState();

                ZipCode tmpState = new ZipCode();
                tmpState.State = "(Select State)";

                stateList.Insert(0, tmpState);

                cmbState.DataSource = stateList;
                cmbState.DisplayMember = "State";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadZipCode()
        {
            try
            {
                List<ZipCode> zipList = new List<ZipCode>();

                zipList = zipCodeBL.GetZipcodeForState();

                ZipCode tmpZip = new ZipCode();
                tmpZip.Zip = "(Select Zip Code)";

                zipList.Insert(0, tmpZip);

                cmbZipcode.DataSource = zipList;
                cmbZipcode.ValueMember = "ZipId";
                cmbZipcode.DisplayMember = "Zip";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadZip()
        {
            try
            {
                List<ZipCode> zipList = new List<ZipCode>();

                zipList = zipCodeBL.GetZip();
                chkZipCodeList.SelectedValue = new List<int>();
                chkZipCodeList.DataSource = zipList;
                chkZipCodeList.DisplayMember = "Zip";
                chkZipCodeList.ValueMember = "ZipId";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmZipCodeDetails_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.frmMain.frmZipCodeDetails = null;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (SaveData())
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    this.Close();
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private bool SaveData()
        {
            try
            {
                ZipCode zipCode = new ZipCode();

                zipCode.Zip = cmbZipcode.Text.Trim();

                List<int> zipcodeList = new List<int>();
                zipcodeList = chkZipCodeList.ValueList;
                List<string> ZipcodeList = new List<string>();

                foreach (int zipcode in zipcodeList)
                {
                    ZipCode zipcodes = zipCodeBL.Get(zipcode);
                    string stt = zipcodes.Zip.ToString();
                    ZipcodeList.Add(stt);
                }
                if (ZipcodeList.Count > 4)
                {
                    MessageBox.Show("you cannot select more than 4 zip codes.");
                    return false;
                }
                zipCode.NearestZip = ZipcodeList;

                int returnVal = zipCodeBL.Save(zipCode);
                if (returnVal == 1)
                {
                    MessageBox.Show("Applied.");
                    return false;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
        }

        private void chkZipCodeList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
        }

        private void btnAvailability_Click(object sender, EventArgs e)
        {
            List<int> zipcodeList = new List<int>();
            zipcodeList = chkZipCodeList.ValueList;
            List<string> ZipcodeList = new List<string>();
            string zip = string.Empty;
            foreach (int zipcode in zipcodeList)
            {
                ZipCode zipcodes = zipCodeBL.Get(zipcode);
                string stt = zipcodes.Zip.ToString();
                //  ZipcodeList.Add(stt);
                if (stt.Trim() == string.Empty)
                {
                    zip = stt;
                }
                else
                {
                    zip += "," + stt;
                }
            }
            char[] MyChar = { ',' };
            string vendorzip = zip.TrimStart(MyChar);

            List<Vendor> vendorList = vendorBL.GetCollectionCenterVendorList(vendorzip);

            dgvVendorInfo.DataSource = vendorList;

            if (dgvVendorInfo.Rows.Count > 0)
            {
                lblCount.Text = dgvVendorInfo.Rows.Count.ToString();
                dgvVendorInfo.Rows[0].Selected = true;
                dgvVendorInfo.Focus();
            }
            else
            {
                lblCount.Text = " 0";
                MessageBox.Show("No Records Found");
            }
        }
    }
}