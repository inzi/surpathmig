using SurPath.Business;
using SurPath.Entity;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace SurPath
{
    public partial class FrmDonorDocumentDetails : Form
    {
        #region Private Variables

        private int _donor_document_id = 0;
        private int _currentDonorId;

        private DonorBL donorBL = new DonorBL();
        private ClientBL clientBL = new ClientBL();

        #endregion Private Variables

        #region Constructor

        public FrmDonorDocumentDetails()
        {
            InitializeComponent();
        }

        public FrmDonorDocumentDetails(int currentDonorId)
        {
            InitializeComponent();
            this._currentDonorId = currentDonorId;
        }

        #endregion Constructor

        #region Event Methods

        private void FrmDonorDocumentDetails_Load(object sender, EventArgs e)
        {
            //cmbDocumentType.SelectedIndex = 0;
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                InitializeControls();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Error occurred while loading the page.");
            }
        }

        private void FrmDonorDocumentDetails_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                bool validationFlag = true;

                foreach (Control ctrl in this.Controls)
                {
                    if (ctrl.CausesValidation == false)
                    {
                        validationFlag = false;
                        break;
                    }
                }

                if (validationFlag == false)
                {
                    DialogResult userConfirmation = MessageBox.Show("Do you want to save changes?", "SurPath Drug Testing", MessageBoxButtons.YesNoCancel);

                    if (userConfirmation == DialogResult.Cancel)
                    {
                        e.Cancel = true;
                    }
                    else if (userConfirmation == DialogResult.Yes)
                    {
                        if (!SaveData())
                        {
                            e.Cancel = true;
                        }
                        else
                        {
                            this.DialogResult = System.Windows.Forms.DialogResult.OK;
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

        private void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (SaveData())
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    this.Close();
                    Cursor.Current = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void btnFile_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.Title = "Select file to be upload";
                //  fileDialog.Filter = "PDF Files|*.pdf|All Files|*.*"; //txt Files|*.txt,Excel Files |*.xlsx,PDF Files|*.pdf" //{ "jpg", "jpeg", "png", "gif", "txt", "ppt", "doc", "docx", "xls", "xlsx", "ods", "pdf" };
                // fileDialog.Filter = "All files (*.*)|*.*";

                fileDialog.Filter = "All files(*.txt;*.ppt;*.doc;*.docx;*.xls;*.xlsx;*.ods;*.pdf;*.BMP;*.JPG;*.GIF,*.PNG,*.TIFF)|*.txt;*.ppt;*.doc;*.docx;*.xls;*.xlsx;*.ods;*.pdf*;*.BMP;*.JPG;*.GIF;*.PNG;*.TIFF";
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtDonorFiles.Text = fileDialog.FileName.ToString();
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void cmbDocumentType_TextChanged(object sender, EventArgs e)
        {
            cmbDocumentType.CausesValidation = false;
        }

        private void txtDonorFiles_TextChanged(object sender, EventArgs e)
        {
            txtDonorFiles.CausesValidation = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                this.Close();
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

        private void InitializeControls()
        {
            ResetControlsCauseValidation();
            LoadDocumentTypes();
        }

        private void LoadDocumentTypes()
        {
            try
            {
                var donorinfo = donorBL.Get(this._currentDonorId, "Web");
                var clientDepartment = donorBL.GetDonorTestInfoByDonorId(this._currentDonorId);
                var clientDeptId = clientDepartment.ClientDepartmentId;
                var ClientDocs = clientBL.GetClientDepartmentDocTypes(clientDeptId);

                foreach (DataRow row in ClientDocs.Rows)
                {
                    this.cmbDocumentType.Items.Add(row["Description"].ToString());
                }
            }
            catch (Exception)
            {
                MessageBox.Show("No Document Types Available");
            }

            //this.cmbDocumentType.Items.Add("Letter Of Attestation");
            //this.cmbDocumentType.Items.Add("CPR Certificates");
            //this.cmbDocumentType.Items.Add("Orientation Certificates");
            //this.cmbDocumentType.Items.Add("Child Abuse Registries");
            //this.cmbDocumentType.Items.Add("Diplomas");
            //this.cmbDocumentType.Items.Add("Demographic Profiles");
            //this.cmbDocumentType.Items.Add("Fingerprint Results");
            //this.cmbDocumentType.Items.Add("Driver License");
            //this.cmbDocumentType.Items.Add("Liability, MalPractice & Personal Insurance");
            //this.cmbDocumentType.Items.Add("Nursing Registries");
            //this.cmbDocumentType.Items.Add("Confidentiality Statements");
            //this.cmbDocumentType.Items.Add("Rn Licenses");
            //this.cmbDocumentType.Items.Add("Business Affidavit");
            //this.cmbDocumentType.Items.Add("Court Orders");
            //this.cmbDocumentType.Items.Add("Other");
        }

        private void ResetControlsCauseValidation()
        {
            foreach (Control ctrl in this.Controls)
            {
                ctrl.CausesValidation = true;
            }
        }

        private bool SaveData()
        {
            try
            {
                if (!ValidateControls())
                {
                    return false;
                }

                DonorDocument donorDocument = null;
                donorDocument = new DonorDocument();

                donorDocument.DonorDocumentId = 0;

                donorDocument.DonorId = this._currentDonorId;

                //if (cmbDocumentType.SelectedIndex == 0)
                //{
                //    donorDocument.DocumentTitle = string.Empty;
                //}
                //else
                //{
                donorDocument.DocumentTitle = cmbDocumentType.SelectedItem.ToString();
                //}

                FileStream stream = File.OpenRead(txtDonorFiles.Text.Trim());
                byte[] fileBytes = new byte[stream.Length];

                stream.Read(fileBytes, 0, fileBytes.Length);
                stream.Close();

                donorDocument.DocumentContent = fileBytes;
                donorDocument.Source = "";
                donorDocument.UploadedBy = Program.currentUserName;
                donorDocument.FileName = txtDonorFiles.Text.Trim().Substring(txtDonorFiles.Text.Trim().LastIndexOf('\\') + 1);

                int returnVal = donorBL.UploadDonorDocument(donorDocument);

                if (returnVal > 0)
                {
                    donorDocument.DonorDocumentId = returnVal;
                    this.DonorDocumentId = returnVal;

                    ResetControlsCauseValidation();

                    return true;
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

        private bool ValidateControls()
        {
            try
            {
                //if (cmbDocumentType.SelectedIndex == 0)
                //{
                //    Cursor.Current = Cursors.Default;
                //    MessageBox.Show("Document Type must be selected.");
                //    cmbDocumentType.Focus();
                //    return false;
                //}
                if (txtDonorFiles.Text.Trim() == string.Empty)
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Document cannot be empty.");
                    txtDonorFiles.Focus();
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

        private void UploadFile()
        {
            string filetype;
            string filename;
            string filePath;

            filename = txtDonorFiles.Text.Substring(Convert.ToInt32(txtDonorFiles.Text.LastIndexOf("\\")) + 1, txtDonorFiles.Text.Length - (Convert.ToInt32(txtDonorFiles.Text.LastIndexOf("\\")) + 1));
            filetype = txtDonorFiles.Text.Substring(Convert.ToInt32(txtDonorFiles.Text.LastIndexOf(".")) + 1, txtDonorFiles.Text.Length - (Convert.ToInt32(txtDonorFiles.Text.LastIndexOf(".")) + 1));
            filePath = txtDonorFiles.Text;

            byte[] FileBytes = null;

            try
            {
                // Open file to read using file path
                FileStream fileStream = new FileStream(txtDonorFiles.Text, System.IO.FileMode.Open, System.IO.FileAccess.Read);

                // Add filestream to binary reader
                BinaryReader binaryReader = new BinaryReader(fileStream);

                // get total byte length of the file
                long allbytes = new FileInfo(txtDonorFiles.Text).Length;

                // read entire file into buffer
                FileBytes = binaryReader.ReadBytes((Int32)allbytes);

                // close all instances
                fileStream.Close();
                fileStream.Dispose();
                binaryReader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during File Read " + ex.ToString());
            }
        }

        #endregion Private Methods

        #region Public Properties

        public int DonorDocumentId
        {
            get
            {
                return this._donor_document_id;
            }
            set
            {
                this._donor_document_id = value;
            }
        }

        #endregion Public Properties
    }
}