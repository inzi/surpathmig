using SurPath.Business;
using SurPath.Entity;
using SurPath.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SurPath.Controls.TextBoxes;
using Surpath.ClearStar.BL;
using Surpath.CSTest.Models;
using SurPath.Data;
using SurpathBackend;
using Serilog;
using Newtonsoft.Json;

namespace SurPath
{
    public partial class FrmIntegrations : Form
    {
        private bool _checkboxssetup = false;
        private int ClientId;
        private int ClientDepartmentId = 0;
        private OperationMode _mode = OperationMode.None;

        private ClientBL clientBL = new ClientBL();

        private BackendLogic backendLogic; // = new BackendLogic();
        private PDFengine _PDFengine; // = new PDFengine();
        private PDFengine _PDFengine_reset; // = new PDFengine();

        private List<IntegrationPartner> _partners = new List<IntegrationPartner>();
        List<IntegrationPartnerClient> _integrations = new List<IntegrationPartnerClient>();
        private List<RadioButton> _partnerRadioButtons = new List<RadioButton>();



        static ILogger _logger = Program._logger;


        //public frmIntegrations()
        //{
        //    InitializeComponent();
        //}

        public FrmIntegrations(OperationMode mode, int clientId, int clientDepartmentId = 0)
        {
            setBackend();
            InitializeComponent();
            this._mode = mode;
            this.ClientId = clientId;
            this.ClientDepartmentId = clientDepartmentId;

        }
        private void setBackend()
        {

            _logger.Debug("FrmClientDepartmentDetails loaded");

            backendLogic = new BackendLogic(null, Program._logger);
            _PDFengine = new PDFengine(Program._logger);
            _PDFengine_reset = new PDFengine(Program._logger);
        }
        public void LoadData()
        {
            bool integrationPartner = false;
            bool requireLogin = false;
            bool require_remote_login = false;

            List<IntegrationPartnerClient> _ips = backendLogic.GetIntegrationPartnerClientByClientAndDepartmentId(this.ClientId, this.ClientDepartmentId);

            if (_ips.Exists(i => i.client_id == this.ClientId && i.client_department_id == this.ClientDepartmentId))
            {
                integrationPartner = true;
                var _ip = _ips.Where(i => i.client_id == this.ClientId && i.client_department_id == this.ClientDepartmentId).FirstOrDefault();
                requireLogin = _ip.require_login;
                require_remote_login = _ip.require_remote_login;
            }

            //ClientDepartment clientDepartment = clientBL.GetClientDepartment(this.ClientDepartmentId);

            if (integrationPartner == true && requireLogin == true)
            {
                // chkRequireLogin.Checked = true;
                rdoRequireLogin.Checked = true;
            }
            else if (integrationPartner == true && require_remote_login == true)
            {
                rdoRequireRemoteLogin.Checked = true;
            }
            else
            {
                rdoRequireNoLogin.Checked = true;
            }
        }

        public bool SaveData()
        {
            if (ValidateControls())
            {
                try
                {
                    List<ClientDepartment> clientDepartments = clientBL.GetClientDepartmentList(this.ClientId);
                    if (this.ClientDepartmentId != 0)
                    {
                        clientDepartments = clientDepartments.Where(cd => cd.ClientDepartmentId == this.ClientDepartmentId).ToList();
                    }

                    RadioButton rdo = _partnerRadioButtons.Where(pc => pc.Checked == true).First();

                    // Our selected partner id
                    var backend_integration_partner_id = (int)rdo.Tag;

                    foreach (IntegrationPartner _partner in this._partners)
                    {
                        // find the checkbox
                        //CheckBox box = _partnerCheckboxes.Where(pc => (int)pc.Tag == _partner.backend_integration_partner_id).First();

                        if (_partner.backend_integration_partner_id== backend_integration_partner_id)
                        {
                            // If no integration exists for dept 0 (for when dept is unknown), add it.
                            // if no integration exists, add it
                            if (!_integrations.Exists(_i => _i.backend_integration_partner_id == _partner.backend_integration_partner_id && _i.client_id == this.ClientId && _i.client_department_id == 0))
                            {
                                // add it
                                //RandomStringGenerator rsg = new RandomStringGenerator(true, true, true, false);
                                //string rndClientCode = rsg.Generate(10, 12);
                                //string rndClientId = rsg.Generate(10, 12);


                                IntegrationPartnerClient integrationPartnerClient = new IntegrationPartnerClient()
                                {
                                    active = false,
                                    backend_integration_partner_client_map_GUID = Guid.NewGuid(),
                                    backend_integration_partner_id = _partner.backend_integration_partner_id,
                                    client_department_id = 0,
                                    partner_client_code = Guid.NewGuid().ToString(), // txtClientCode.Text,
                                    partner_client_id = Guid.NewGuid().ToString(), //txtClientCode.Text,
                                    client_id = this.ClientId,
                                    require_login = rdoRequireLogin.Checked,
                                    require_remote_login = rdoRequireRemoteLogin.Checked,
                                    last_modified_by = Program.currentUserName,

                                };

                                _integrations.Add(integrationPartnerClient);
                            }

                            // if no integration exists for each dept, add it

                            foreach (ClientDepartment cd in clientDepartments)
                            {
                                var thisdeptid = cd.ClientDepartmentId;


                                if (!_integrations.Exists(_i => _i.backend_integration_partner_id == _partner.backend_integration_partner_id && _i.client_id == this.ClientId && _i.client_department_id == thisdeptid))
                                {
                                    // add it
                                    //RandomStringGenerator rsg = new RandomStringGenerator(true, true, true, false);
                                    //string rndClientCode = rsg.Generate(10, 12);
                                    //string rndClientId = rsg.Generate(10, 12);


                                    IntegrationPartnerClient integrationPartnerClient = new IntegrationPartnerClient()
                                    {
                                        active = false,
                                        backend_integration_partner_client_map_GUID = Guid.NewGuid(),
                                        backend_integration_partner_id = _partner.backend_integration_partner_id,
                                        client_department_id = thisdeptid,
                                        partner_client_code = Guid.NewGuid().ToString(), // txtClientCode.Text,
                                        partner_client_id = Guid.NewGuid().ToString(), //txtClientCode.Text,
                                        client_id = this.ClientId,
                                        require_login = rdoRequireLogin.Checked,
                                        require_remote_login = rdoRequireRemoteLogin.Checked,
                                        last_modified_by = Program.currentUserName,

                                    };

                                    _integrations.Add(integrationPartnerClient);
                                }

                            }

                        }

                        // if we specified a dept, set the push folder if it's set
                        if (this.ClientDepartmentId!=0 && string.IsNullOrEmpty(txtPushFolder.Text.ToString())==false)
                        {
 
                                //partner_push_folder = txtPushFolder.Text.ToString(),
                             _integrations.Where(_i => _i.client_id == this.ClientId && _i.client_department_id==this.ClientDepartmentId && _i.backend_integration_partner_id == backend_integration_partner_id).ToList().Select(_i =>
                             {
                                 _i.partner_push_folder = txtPushFolder.Text.ToString();
                                 return _i;
                             }).ToList();
                        }

                        // deactivate for all partners
                        _integrations.Where(_i => _i.client_id == this.ClientId).ToList().Select(_i => { _i.active = false; return _i; }).ToList();

                        // activate for our selected backend_integration_partner_id
                        _integrations.Where(_i => _i.client_id == this.ClientId && _i.backend_integration_partner_id == backend_integration_partner_id).ToList().Select(_i => { _i.active = true; return _i; }).ToList();


                        // Set values for our backend_integration_partner_id
                        _integrations.Where(_i => _i.client_id == this.ClientId && _i.backend_integration_partner_id == backend_integration_partner_id).ToList().Select(_i =>
                        {

                            _i.partner_client_code = txtClientCode.Text;
                            _i.partner_client_id = txtClientCode.Text;


                            _i.require_login = false;
                            _i.require_login = rdoRequireLogin.Checked;// chkRequireLogin.Checked;

                            _i.require_remote_login = false;
                            _i.require_remote_login = rdoRequireRemoteLogin.Checked;// chkRequireLogin.Checked;
                            return _i;
                        }).ToList();



                        //_integrations.Where(_i =>_i.client_id == this.ClientId && _i.client_department_id == 0).First().active = rdo.Checked;

                        //// set 0 active and each dept active for this client
                        //_integrations.Where(_i => _i.backend_integration_partner_id == _partner.backend_integration_partner_id && _i.client_id == this.ClientId && _i.client_department_id == 0).First().active = rdo.Checked;

                        //foreach (ClientDepartment cd in clientDepartments)
                        //{
                        //    _integrations.Where(_i => _i.backend_integration_partner_id == _partner.backend_integration_partner_id && _i.client_id == this.ClientId && _i.client_department_id == cd.ClientDepartmentId).First().active = rdo.Checked;

                        //}
                        //// from: https://visualstudiomagazine.com/articles/2019/07/01/updating-linq.aspx
                        //// db.Customers.Where(c => c.IsValid).ToList().Select(c => { c.CreditLimit = 1000; return c; });


                        //// deactivate all other partners for this client - there can only be on integration
                        //_integrations.Where(_i => _i.client_id != this.ClientId).ToList().Select(_i => { _i.active = false; return _i; });






                        ////// set the partner to active or not
                        ////_integrations.Where(_i => _i.backend_integration_partner_id == _partner.backend_integration_partner_id && _i.client_id == this.ClientId && _i.client_department_id == this.ClientDepartmentId).First().active = rdo.Checked;

                        //foreach (IntegrationPartnerClient _int in _integrations)
                        //{
                        //    if ((int)rdo.Tag == _partner.backend_integration_partner_id)
                        //    {
                        //        // set the client_code
                        //        _integrations.Where(_i => _i.backend_integration_partner_id == _partner.backend_integration_partner_id && _i.client_id == this.ClientId && _i.client_department_id == this.ClientDepartmentId).First().partner_client_code = txtClientCode.Text;
                        //        _integrations.Where(_i => _i.backend_integration_partner_id == _partner.backend_integration_partner_id && _i.client_id == this.ClientId && _i.client_department_id == this.ClientDepartmentId).First().partner_client_id = txtClientCode.Text;

                        //        // set login requirements
                        //        _integrations.Where(_i => _i.backend_integration_partner_id == _partner.backend_integration_partner_id && _i.client_id == this.ClientId && _i.client_department_id == this.ClientDepartmentId).First().require_login = false;
                        //        _integrations.Where(_i => _i.backend_integration_partner_id == _partner.backend_integration_partner_id && _i.client_id == this.ClientId && _i.client_department_id == this.ClientDepartmentId).First().require_login = rdoRequireLogin.Checked;// chkRequireLogin.Checked;

                        //        _integrations.Where(_i => _i.backend_integration_partner_id == _partner.backend_integration_partner_id && _i.client_id == this.ClientId && _i.client_department_id == this.ClientDepartmentId).First().require_remote_login = false;
                        //        _integrations.Where(_i => _i.backend_integration_partner_id == _partner.backend_integration_partner_id && _i.client_id == this.ClientId && _i.client_department_id == this.ClientDepartmentId).First().require_remote_login = rdoRequireRemoteLogin.Checked;// chkRequireLogin.Checked;

                        //    }
                        //}
                        //// if this client is selected, set these values
                        ////if ((int)rdo.Tag == _partner.backend_integration_partner_id)
                        ////{
                        ////    // set the client_code
                        ////    _integrations.Where(_i => _i.backend_integration_partner_id == _partner.backend_integration_partner_id && _i.client_id == this.ClientId && _i.client_department_id == this.ClientDepartmentId).First().partner_client_code = txtClientCode.Text;
                        ////    _integrations.Where(_i => _i.backend_integration_partner_id == _partner.backend_integration_partner_id && _i.client_id == this.ClientId && _i.client_department_id == this.ClientDepartmentId).First().partner_client_id = txtClientCode.Text;

                        ////    // set login requirements
                        ////    _integrations.Where(_i => _i.backend_integration_partner_id == _partner.backend_integration_partner_id && _i.client_id == this.ClientId && _i.client_department_id == this.ClientDepartmentId).First().require_login = false;
                        ////    _integrations.Where(_i => _i.backend_integration_partner_id == _partner.backend_integration_partner_id && _i.client_id == this.ClientId && _i.client_department_id == this.ClientDepartmentId).First().require_login = rdoRequireLogin.Checked;// chkRequireLogin.Checked;

                        ////    _integrations.Where(_i => _i.backend_integration_partner_id == _partner.backend_integration_partner_id && _i.client_id == this.ClientId && _i.client_department_id == this.ClientDepartmentId).First().require_remote_login = false;
                        ////    _integrations.Where(_i => _i.backend_integration_partner_id == _partner.backend_integration_partner_id && _i.client_id == this.ClientId && _i.client_department_id == this.ClientDepartmentId).First().require_remote_login = rdoRequireRemoteLogin.Checked;// chkRequireLogin.Checked;

                        ////}


                    }
                    // update the partners
                    foreach (IntegrationPartnerClient _int in _integrations)
                    {
                        backendLogic.SetIntegrationPartnerClient(_int);
                    }
                }
                catch (Exception ex)
                {
                    Program.LogError(ex);
                    throw;
                }

                return true;

            }
            return false;
        }

        /// <summary>
        /// Load integration partners and build radio buttons on form
        /// </summary>
        public void LoadIntegrationPartners()
        {
            _partners = backendLogic.GetIntegrationPartners();
            _integrations = new List<IntegrationPartnerClient>();
            _integrations = backendLogic.GetIntegrationPartnerClientByClientAndDepartmentId(this.ClientId, 0);
            var i = 0;
            var left = 23;
            var gap = 23;

            var top = 20; // gbIntegrations.Location.Y + 20;
            var _partnerId = -1;
            var _partner_client_code = string.Empty;
            var _partner_push_folder = string.Empty;
            RadioButton rdo = new RadioButton();
            rdo.Tag = _partnerId;
            rdo.Text = "None";
            rdo.Checked = false;
            rdo.Location = new Point(left, top + (i * gap));
            //tabIntegrationsClientDepartmentDetails.Controls.Add(rdo);
            gbIntegrations.Controls.Add(rdo);

            _partnerRadioButtons.Add(rdo);
            i++;
            foreach (IntegrationPartner _partner in _partners)
            {
                rdo = new RadioButton();
                rdo.Tag = _partner.backend_integration_partner_id;
                rdo.Text = _partner.partner_name;

                if (_integrations.Exists(_i => _i.active == true && _i.backend_integration_partner_id == _partner.backend_integration_partner_id && _i.client_department_id == this.ClientDepartmentId && _i.client_id == this.ClientId))
                {
                    _partnerId = _partner.backend_integration_partner_id;
                    _partner_client_code = _integrations.Where(_i => _i.active == true && _i.backend_integration_partner_id == _partner.backend_integration_partner_id && _i.client_department_id == this.ClientDepartmentId && _i.client_id == this.ClientId).FirstOrDefault().partner_client_code;
                    _partner_push_folder = _integrations.Where(_i => _i.active == true && _i.backend_integration_partner_id == _partner.backend_integration_partner_id && _i.client_department_id == this.ClientDepartmentId && _i.client_id == this.ClientId).FirstOrDefault().partner_push_folder;
                }
                rdo.Location = new Point(left, top + (i * gap));
                gbIntegrations.Controls.Add(rdo);
                _partnerRadioButtons.Add(rdo);

                i++;
            }

            if (!(string.IsNullOrEmpty(_partner_client_code)))
            {
                // set txtClientCode
                // get the mapp
                txtClientCode.Text = _partner_client_code;
            }

            if (!(string.IsNullOrEmpty(_partner_push_folder)))
            {
                // set txtPushFolder
                txtPushFolder.Text = _partner_push_folder;
            }
            if (this.ClientDepartmentId!=0)
            {
                txtPushFolder.Visible = true;
                lblPushFolder.Visible = true;
            }


            foreach (RadioButton __rdo in _partnerRadioButtons)
            {
                __rdo.Click += delegate (object sender, EventArgs e)
                {
                    var _rdo = sender as RadioButton;
                    if ((int)_rdo.Tag != -1)
                    {
                        rdoRequireNoLogin.Checked = false;
                        rdoRequireNoLogin.Enabled = false;
                    }
                    else
                    {
                        rdoRequireNoLogin.Enabled = true;
                        rdoRequireNoLogin.Checked = true;

                    }
                };
            }
            _partnerRadioButtons.Where(_rdo => (int)_rdo.Tag == _partnerId).First().Checked = true;
            _checkboxssetup = true;

        }

        private void frmIntegrations_Load(object sender, EventArgs e)
        {
            LoadData();

            LoadIntegrationPartners();
        }


        private bool ValidateControls()
        {
            if ((int)_partnerRadioButtons.Where(_r => _r.Checked == true).First().Tag > 0)
            {
                if (rdoRequireLogin.Checked == false && rdoRequireRemoteLogin.Checked == false)
                {
                    MessageBox.Show("Please select integration donor login requirements!", "Integrations require local or remote login");
                    rdoRequireLogin.Focus();
                    return false;
                }

                if (string.IsNullOrEmpty(txtClientCode.Text))
                {
                    MessageBox.Show("Please provide a partner client code", "A partner client code is required!");
                    txtClientCode.Focus();
                    return false;
                }
                else
                {

                    if (this.ClientDepartmentId>0)
                    {
                        // make sure this isn't used by another client or dept for this partner
                        if (
                            _integrations.Exists(_i => _i.active == true && _i.partner_client_code == txtClientCode.Text && _i.client_id != this.ClientId) // diff client already used this client code
                            ||
                            _integrations.Exists(_i => _i.active == true && _i.partner_client_code == txtClientCode.Text && _i.client_id == this.ClientId && _i.client_department_id != this.ClientDepartmentId)
                            )
                        {
                            IntegrationPartnerClient _assigned;
                            string _msg = "This partner client code is already assigned to another client and /or client department";
                            if (_integrations.Exists(_i => _i.partner_client_code == txtClientCode.Text && _i.client_id != this.ClientId))
                            {
                                _assigned = _integrations.Where(_i => _i.active == true && _i.partner_client_code == txtClientCode.Text && _i.client_id != this.ClientId).First();
                            }
                            else
                            {
                                _assigned = _integrations.Where(_i => _i.active == true && _i.partner_client_code == txtClientCode.Text && _i.client_id == this.ClientId && _i.client_department_id != this.ClientDepartmentId).First();
                            }
                            if (_assigned.client_department_id > 0)
                            {
                                var _d = clientBL.GetClientDepartment(_assigned.client_department_id);
                                _msg = $"This partner client code is assigned to a specific department ({_d.DepartmentName}).\r\nClient codes must be unique to either the client or a specific department";
                            }
                            else
                            {
                                var _c = clientBL.Get(_assigned.client_id);
                                _msg = $"This partner client code is assigned to a another client ({_c.ClientName}).\r\nClient codes must be unique to either a client or a specific department";
                            }


                            MessageBox.Show(_msg, "Partner client code already in use!");
                            txtClientCode.Focus();
                            return false;
                        }
                    }
                    else
                    {
                        // TODO - when we implement logic to push folders and per dept integration settings
                        // this needs to confirm to apply these settings to all integrated clients
                        // which is the default behavior at this time.

                        // the only other check would be if same client was in use by another integration partner - which should not happen

                        //if (
                        //_integrations.Exists(_i => _i.active == true && _i.partner_client_code == txtClientCode.Text && _i.client_id != this.ClientId) // diff client already used this client code
                       
                        //)
                        //{
                        //    IntegrationPartnerClient _assigned;
                        //    string _msg = "This partner client code is already assigned to another client and /or client department";

                        //    if (_integrations.Exists(_i => _i.partner_client_code == txtClientCode.Text && _i.client_id != this.ClientId))
                        //    {
                        //        _assigned = _integrations.Where(_i => _i.active == true && _i.partner_client_code == txtClientCode.Text && _i.client_id != this.ClientId).First();
                        //    }
                        //    else
                        //    {
                        //        _assigned = _integrations.Where(_i => _i.active == true && _i.partner_client_code == txtClientCode.Text && _i.client_id == this.ClientId && _i.client_department_id != this.ClientDepartmentId).First();
                        //    }
                        //    if (_assigned.client_department_id > 0)
                        //    {
                        //        var _d = clientBL.GetClientDepartment(_assigned.client_department_id);
                        //        _msg = $"This partner client code is assigned to a specific department ({_d.DepartmentName}).\r\nClient codes must be unique to either the client or a specific department";
                        //    }
                        //    else
                        //    {
                        //        var _c = clientBL.Get(_assigned.client_id);
                        //        _msg = $"This partner client code is assigned to a another client ({_c.ClientName}).\r\nClient codes must be unique to either a client or a specific department";
                        //    }


                        //    MessageBox.Show(_msg, "Partner client code already in use!");
                        //    txtClientCode.Focus();
                        //    return false;
                        //}
                    }
                   
                }
            }
            return true;
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

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void surTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
