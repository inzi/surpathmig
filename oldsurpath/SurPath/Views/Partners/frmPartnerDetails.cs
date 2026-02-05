using Serilog;
using SurPath.Business;
using SurPath.Data;
using SurPath.Data.Backend;
using SurPath.Entity;
using SurPath.Enum;
using SurpathBackend;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SurPath
{
    public partial class FrmPartnerDetails : Form
    {
        public IntegrationPartner integrationParter { get; set; }
        private BackendLogic backendLogic = new BackendLogic(Program.currentUserName, _logger);
        static ILogger _logger = Program._logger;

        public FrmPartnerDetails(ILogger __logger = null)
        {
            if (__logger != null) _logger = __logger;

            InitializeComponent();
        }

        private void frmPartnerDetails_Load(object sender, EventArgs e)
        {
            txtName.Text = this.integrationParter.partner_name;
            txtKey.Text = this.integrationParter.partner_key;
            txtCrypto.Text = this.integrationParter.partner_crypto;
            txtPidType.Text = this.integrationParter.backend_integration_partners_pidtype.ToString();
            chkActive.Checked = this.integrationParter.active;
            chkRequireLogin.Checked = this.integrationParter.require_login;
            txtInstructions.Text = this.integrationParter.html_instructions;
            txtLoginUrl.Text = this.integrationParter.login_url;

            txtHost.Text = this.integrationParter.partner_push_host;
            int.TryParse(this.integrationParter.partner_push_port, out int _port);
            if (_port < 1) _port = 22;
            numPort.Value = _port;
            txtLogin.Text = this.integrationParter.partner_push_username;
            txtPassword.Text = this.integrationParter.partner_push_password;
            txtFolder.Text = this.integrationParter.partner_push_path;
            cbPush.Checked = this.integrationParter.partner_push;
        }

        private bool CheckForm()
        {

            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("A name is required.", "Invalid Name", MessageBoxButtons.OK);
                return false;
            }
            if (string.IsNullOrEmpty(txtCrypto.Text))
            {
                MessageBox.Show("Crypto cannot be blank.", "Invalid Crypto", MessageBoxButtons.OK);
                return false;
            }
            if (string.IsNullOrEmpty(txtKey.Text))
            {
                MessageBox.Show("Key cannot be blank.", "Invalid Key", MessageBoxButtons.OK);
                return false;
            }
            if (string.IsNullOrEmpty(txtKey.Text))
            {
                MessageBox.Show("Key cannot be blank.", "Invalid Key", MessageBoxButtons.OK);
                return false;
            }

            if (cbPush.Checked==true)
            {
                    if (string.IsNullOrEmpty(txtHost.Text) || string.IsNullOrEmpty(txtLogin.Text) || string.IsNullOrEmpty(txtPassword.Text))
                    {
                        MessageBox.Show("SFTP Push requires login, and password", "Incomplete SFTP Info", MessageBoxButtons.OK);
                        return false;
                    }
            }
           

            if (!(string.IsNullOrEmpty(txtLoginUrl.Text)))
            {
                Uri uriResult;
                bool uri_result = Uri.TryCreate(txtLoginUrl.Text, UriKind.Absolute, out uriResult)
                    && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

                if (uri_result == false)
                {
                    MessageBox.Show("The login URL isn't valid. Did you include HTTP:// or HTTPS://?\r\nEnter a valid URL or leave blank.", "Invalid URL", MessageBoxButtons.OK);
                    return false;
                }
            }
            return true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (CheckForm() == false) return;

            IntegrationPartner _integrationPartner = new IntegrationPartner()
            {
                backend_integration_partners_pidtype = this.integrationParter.backend_integration_partners_pidtype,
                active = chkActive.Checked,
                backend_integration_partner_id = this.integrationParter.backend_integration_partner_id,
                last_modified_by = Program.currentUserName,
                partner_crypto = txtCrypto.Text,
                partner_key = txtKey.Text,
                partner_name = txtName.Text,
                require_login = chkRequireLogin.Checked,
                html_instructions = txtInstructions.Text,
                login_url = txtLoginUrl.Text,
                partner_push = cbPush.Checked,
                partner_push_host = txtHost.Text,
                partner_push_port = numPort.Value.ToString(),
                partner_push_username = txtLogin.Text,
                partner_push_password = txtPassword.Text,
                partner_push_path = txtFolder.Text,

            };

            // if different, save it
            if (
                this.integrationParter.partner_name != _integrationPartner.partner_name
                ||
                this.integrationParter.partner_crypto != _integrationPartner.partner_crypto
                ||
                this.integrationParter.partner_key != _integrationPartner.partner_key
                ||
                this.integrationParter.active != _integrationPartner.active
                ||
                this.integrationParter.require_login != _integrationPartner.require_login
                ||
                this.integrationParter.html_instructions != _integrationPartner.html_instructions
                ||
                this.integrationParter.login_url != _integrationPartner.login_url
                ||
                this.integrationParter.partner_push != _integrationPartner.partner_push
                ||
                this.integrationParter.partner_push_host != _integrationPartner.partner_push_host
                ||
                this.integrationParter.partner_push_port != _integrationPartner.partner_push_port
                ||
                this.integrationParter.partner_push_username != _integrationPartner.partner_push_username
                ||
                this.integrationParter.partner_push_password != _integrationPartner.partner_push_password
                ||
                this.integrationParter.partner_push_path != _integrationPartner.partner_push_path

                )
            {
                Cursor.Current = Cursors.WaitCursor;
                backendLogic.SetIntegrationPartners(_integrationPartner);
                Cursor.Current = Cursors.Default;
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();

        }

        private void frmPartnerDetails_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void btnGenKey_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure?", "Generate New Key", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                txtKey.Text = Guid.NewGuid().ToString();
            }
        }

        private void bnGenCrypto_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure?", "Generate New Crypto", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                txtCrypto.Text = Guid.NewGuid().ToString();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
