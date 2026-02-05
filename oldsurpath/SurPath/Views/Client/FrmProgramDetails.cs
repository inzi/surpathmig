using System;
using System.Windows.Forms;

namespace SurPath
{
    public partial class FrmProgramDetails : Form
    {
        public FrmProgramDetails()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmProgramDetails_Load(object sender, EventArgs e)
        {
        }
    }
}