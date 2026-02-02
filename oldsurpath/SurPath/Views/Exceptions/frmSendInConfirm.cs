using System.Windows.Forms;

namespace SurPath.Views.Exceptions
{
    public partial class FrmSendInConfirm : Form
    {
        public FrmSendInConfirm()
        {
            InitializeComponent();
            lblPrompt.Text = this.Prompt;
        }
    }
}