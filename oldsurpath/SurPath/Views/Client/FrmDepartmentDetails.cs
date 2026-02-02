using SurPath.Business.Master;
using SurPath.Entity.Master;
using SurPath.Enum;
using System;
using System.Data;
using System.Windows.Forms;

namespace SurPath
{
    public partial class FrmDepartmentDetails : Form
    {
        #region Private Variables

        private OperationMode _mode = OperationMode.None;
        private int _departmentId = 0;

        private DepartmentBL departmentBL = new DepartmentBL();

        #endregion Private Variables

        #region Constructor

        public FrmDepartmentDetails()
        {
            InitializeComponent();
        }

        public FrmDepartmentDetails(OperationMode mode, int departmentId)
        {
            InitializeComponent();

            this._mode = mode;
            this._departmentId = departmentId;
        }

        #endregion Constructor

        #region Event Methods

        private void FrmDepartmentDetails_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                InitializeControls();

                if (this._mode == OperationMode.Edit && this._departmentId != 0)
                {
                    LoadData();
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Error occurred while loading the page.");
            }
        }

        private void FrmDepartmentDetails_FormClosing(object sender, FormClosingEventArgs e)
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

        private void btnOK_Click(object sender, EventArgs e)
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

        private void txtDepartmentName_TextChanged(object sender, EventArgs e)
        {
            txtDepartmentName.CausesValidation = false;
        }

        private void chkActive_CheckedChanged(object sender, EventArgs e)
        {
            chkActive.CausesValidation = false;
        }

        #endregion Event Methods

        #region Public Methods

        public void LoadData()
        {
            try
            {
                if (this._mode == OperationMode.Edit)
                {
                    Department department = departmentBL.Get(this._departmentId);

                    if (department != null)
                    {
                        txtDepartmentName.Text = department.DepartmentNameValue;
                        chkActive.Checked = department.IsActive;
                        ResetControlsCauseValidation();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void InitializeControls()
        {
            if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
            {
                //DEPARTMENT_VIEW
                DataRow[] departmentView = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.DEPARTMENT_VIEW.ToDescriptionString() + "'");

                if (departmentView.Length > 0)
                {
                    foreach (Control ctrl in this.Controls)
                    {
                        if (ctrl is TextBox)
                        {
                            ((TextBox)ctrl).ReadOnly = true;
                        }
                        if (ctrl is Button)
                        {
                            ((Button)ctrl).Enabled = false;
                        }
                        if (ctrl is CheckBox)
                        {
                            ((CheckBox)ctrl).Enabled = false;
                        }
                        if (ctrl is MaskedTextBox)
                        {
                            ((MaskedTextBox)ctrl).ReadOnly = true;
                        }
                    }
                }
            }
            ResetControlsCauseValidation();
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
                Department department = null;

                if (this._mode == OperationMode.New)
                {
                    department = new Department();
                    department.DepartmentId = 0;
                    department.CreatedBy = Program.currentUserName;
                }
                else if (this._mode == OperationMode.Edit)
                {
                    department = departmentBL.Get(this._departmentId);
                }

                department.DepartmentNameValue = txtDepartmentName.Text.Trim();
                department.IsActive = chkActive.Checked;
                department.LastModifiedBy = Program.currentUserName;

                int returnVal = departmentBL.Save(department);

                if (returnVal > 0)
                {
                    if (this._mode == OperationMode.New)
                    {
                        department.DepartmentId = returnVal;
                        this.DepartmentId = returnVal;
                    }

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
                if (txtDepartmentName.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Department Name cannot be empty.");
                    return false;
                }
                else
                {
                    if (!ValidateDepartmentName())
                    {
                        MessageBox.Show("Department Name already exits.");
                        txtDepartmentName.Focus();
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return true;
        }

        private bool ValidateDepartmentName()
        {
            try
            {
                DataTable department = departmentBL.GetByDepartmentName(txtDepartmentName.Text.Trim());

                if (department.Rows.Count > 0 && this._mode == OperationMode.New)
                {
                    return false;
                }
                else if (department.Rows.Count > 1 && this._mode == OperationMode.Edit)
                {
                    return false;
                }
                else if (department.Rows.Count == 1 && this._mode == OperationMode.Edit)
                {
                    if ((int)department.Rows[0]["DepartmentId"] != this._departmentId)
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return true;
        }

        #endregion Private Methods

        #region Public Properties

        public OperationMode Mode
        {
            get
            {
                return this._mode;
            }
            set
            {
                this._mode = value;
            }
        }

        public int DepartmentId
        {
            get
            {
                return this._departmentId;
            }
            set
            {
                this._departmentId = value;
            }
        }

        #endregion Public Properties
    }
}