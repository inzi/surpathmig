using SurPath.Business;
using SurPath.Entity;
using SurPath.Enum;
using System;
using System.Windows.Forms;

namespace SurPath
{
    public partial class FrmHideWebInput : Form
    {
        private DonorBL _donor = null;
        private int _currenttestInfoID = 0;
        private int _currentuserId = 0;
        private int _donorid = 0;
        private bool _hide;

        public FrmHideWebInput(int CurrentTestInfoID, int DonorId, DonorBL donor, int CurrentUserID, bool hide)
        {
            _donor = donor;
            _currenttestInfoID = CurrentTestInfoID;
            _currentuserId = CurrentUserID;
            _donorid = DonorId;
            _hide = hide;

            InitializeComponent();
            if (hide)
            {
                this.Text = "Hide From Web";
                this.lblInput.Text = "What is the reason for hiding this donor?";
            }
            else
            {
                this.Text = "Show On Web";
                this.lblInput.Text = "What is the reason for enabling this donor?";
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Hide();
            CreateNoteWithText(" " + this.txtInputReason.Text + $" set to {this._hide.ToString()}", _currenttestInfoID, _donorid, _donor, _currentuserId);
            //LoadTestInfoDetails(donorTestInfo);
            //LoadAccountDetails(this.currentDonorId, this.currentTestInfoId);
            this.Close();
        }

        private void CreateNoteWithText(string NoteText, int currentTestInfoId, int DonorID, DonorBL bl, int CurrentUserId)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                DonorActivityNote donorActivityNote = new DonorActivityNote();

                donorActivityNote.DonorTestInfoId = currentTestInfoId;
                donorActivityNote.ActivityUserId = CurrentUserId;

                donorActivityNote.ActivityNote = NoteText;

                donorActivityNote.ActivityCategoryId = DonorActivityCategories.General;

                bl.AddDonorActivityNote(donorActivityNote);

                if (_hide)
                {
                    bl.UpdateWebHidden(DonorID, true);
                }
                else
                {
                    bl.UpdateWebHidden(DonorID, false);
                }

                Cursor.Current = Cursors.Default;
                MessageBox.Show("Activity / Note has been saved successfully.");

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Program._logger.Error(ex.Message);
                Program._logger.Error(ex.InnerException.ToString());

                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message + "\r\n" + ex.InnerException.ToString());
            }
        }

        private void FrmHideWebInput_Load(object sender, EventArgs e)
        {
        }
    }
}