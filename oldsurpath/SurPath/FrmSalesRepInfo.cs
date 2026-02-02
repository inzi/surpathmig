using System.Windows.Forms;

namespace SurPath
{
    public partial class FrmSalesRepInfo : Form
    {
        #region Constructor

        public FrmSalesRepInfo()
        {
            InitializeComponent();
        }

        #endregion

        #region Event Methods

        private void FrmSalesRepInfo_Load(object sender, System.EventArgs e)
        {
            LoadSalesRepInfo();

        }

        private void FrmSalesRepInfo_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.frmMain.frmSalesRepInfo = null;
        }

        private void tsbNew_Click(object sender, System.EventArgs e)
        {
            //

        }

        private void tsbEdit_Click(object sender, System.EventArgs e)
        {
            //

        }
        private void tsbDelete_Click(object sender, System.EventArgs e)
        {
            if (MessageBox.Show("Are you sure? Do you want to delete the selected record?") == DialogResult.Yes)
            {
                //
            }

        }

        private void btnSearch_Click(object sender, System.EventArgs e)
        {
            //

        }

        #endregion

        #region Private Methods

        private void LoadSalesRepInfo()
        {
            //
        }

        private void SearchSalesRepInfo(string searchKeyword)
        {
            //
        }

        #endregion 


    }
}
