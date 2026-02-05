using SurPath.Business;
using SurPath.Entity;
using SurPath.Enum;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace SurPath
{
    public partial class FrmDashboard : Form
    {
        private ClientBL clientBL = new ClientBL();
        private bool loadFlag = false;

        public FrmDashboard()
        {
            InitializeComponent();
        }

        #region Event Methods

        private void FrmDashBoard_Load(object sender, EventArgs e)
        {
            InitializeControls();

            dgvTestPerformed.AutoGenerateColumns = false;
            LoadClientDepartmentDetails(tvTestPerformed);
            CheckAllNodes(tvTestPerformed.Nodes);

            LoadClientDepartmentDetails(tvAccounting);
            CheckAllNodes(tvAccounting.Nodes);

            LoadClientDepartmentDetails(tvCommission);
            CheckAllNodes(tvCommission.Nodes);

            LoadAccountingTabData();
            LoadPerformanceTabData();

            loadFlag = true;
        }

        private void btnCommClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAccClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnIssuesClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnTestPerformClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmDashboard_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.frmMain.frmDashboard = null;
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {
        }

        private void tvAccounting_AfterCheck(object sender, TreeViewEventArgs e)
        {
            // The code only executes if the user caused the checked state to change.
            if (e.Action != TreeViewAction.Unknown)
            {
                if (e.Node.Nodes.Count > 0)
                {
                    /* Calls the CheckAllChildNodes method, passing in the current
                    Checked value of the TreeNode whose checked state changed. */
                    this.CheckAllChildNodes(e.Node, e.Node.Checked);
                }

                if (e.Node.Parent != null && e.Node.Parent.Nodes.Count > 0)
                {
                    bool flag = true;
                    foreach (TreeNode childNode in e.Node.Parent.Nodes)
                    {
                        if (!childNode.Checked)
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (flag)
                    {
                        e.Node.Parent.Checked = true;
                    }
                    else
                    {
                        e.Node.Parent.Checked = false;
                    }
                }
            }

            if (loadFlag)
            {
                LoadAccountingTabData();
            }
        }

        private void tvAccounting_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //
        }

        private void tvTestPerformed_AfterCheck(object sender, TreeViewEventArgs e)
        {
            // The code only executes if the user caused the checked state to change.
            if (e.Action != TreeViewAction.Unknown)
            {
                if (e.Node.Nodes.Count > 0)
                {
                    /* Calls the CheckAllChildNodes method, passing in the current
                    Checked value of the TreeNode whose checked state changed. */
                    this.CheckAllChildNodes(e.Node, e.Node.Checked);
                }

                if (e.Node.Parent != null && e.Node.Parent.Nodes.Count > 0)
                {
                    bool flag = true;
                    foreach (TreeNode childNode in e.Node.Parent.Nodes)
                    {
                        if (!childNode.Checked)
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (flag)
                    {
                        e.Node.Parent.Checked = true;
                    }
                    else
                    {
                        e.Node.Parent.Checked = false;
                    }
                }
            }
            if (loadFlag)
            {
                LoadPerformanceTabData();
            }
        }

        private void tvTestPerformed_AfterSelect(object sender, TreeViewEventArgs e)
        {
        }

        private void tvCommission_AfterCheck(object sender, TreeViewEventArgs e)
        {
            // The code only executes if the user caused the checked state to change.
            if (e.Action != TreeViewAction.Unknown)
            {
                if (e.Node.Nodes.Count > 0)
                {
                    /* Calls the CheckAllChildNodes method, passing in the current
                    Checked value of the TreeNode whose checked state changed. */
                    this.CheckAllChildNodes(e.Node, e.Node.Checked);
                }

                if (e.Node.Parent != null && e.Node.Parent.Nodes.Count > 0)
                {
                    bool flag = true;
                    foreach (TreeNode childNode in e.Node.Parent.Nodes)
                    {
                        if (!childNode.Checked)
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (flag)
                    {
                        e.Node.Parent.Checked = true;
                    }
                    else
                    {
                        e.Node.Parent.Checked = false;
                    }
                }
            }
        }

        private void tvCommission_AfterSelect(object sender, TreeViewEventArgs e)
        {
        }

        #endregion Event Methods

        #region Private Methods

        private void InitializeControls()
        {
            if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
            {
                tabDashboard.TabPages.Remove(tabTestPerformed);
                tabDashboard.TabPages.Remove(tabIssues);
                tabDashboard.TabPages.Remove(tabAccounting);
                tabDashboard.TabPages.Remove(tabCommissions);

                int tabCount = 0;
                bool firstTabFlag = true;

                //DASHBOARD_VIEW_TEST_PERFORMED_TAB
                DataRow[] dashboardTestPerformedTab = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.DASHBOARD_VIEW_TEST_PERFORMED_TAB.ToDescriptionString() + "'");

                if (dashboardTestPerformedTab.Length > 0)
                {
                    tabDashboard.TabPages.Add(tabTestPerformed);
                    tabCount++;

                    if (firstTabFlag)
                    {
                        tabTestPerformed.BringToFront();
                        firstTabFlag = false;

                        tabDashboard.SelectedTab = tabTestPerformed;
                    }
                }

                //DASHBOARD_VIEW_ISSUES_TAB
                DataRow[] dashboardIssuesTab = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.DASHBOARD_VIEW_ISSUES_TAB.ToDescriptionString() + "'");

                if (dashboardIssuesTab.Length > 0)
                {
                    tabDashboard.TabPages.Add(tabIssues);
                    tabCount++;

                    if (firstTabFlag)
                    {
                        tabIssues.BringToFront();
                        firstTabFlag = false;

                        tabDashboard.SelectedTab = tabIssues;
                    }
                }

                //DASHBOARD_VIEW_ACCOUNTING_TAB
                DataRow[] dashboardAccountingTab = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.DASHBOARD_VIEW_ACCOUNTING_TAB.ToDescriptionString() + "'");

                if (dashboardAccountingTab.Length > 0)
                {
                    tabDashboard.TabPages.Add(tabAccounting);
                    tabCount++;

                    if (firstTabFlag)
                    {
                        tabAccounting.BringToFront();
                        firstTabFlag = false;

                        tabDashboard.SelectedTab = tabAccounting;
                    }
                }

                //DASHBOARD_VIEW_COMMISSIONS_TAB
                DataRow[] dashboardCommissionTab = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.DASHBOARD_VIEW_COMMISSIONS_TAB.ToDescriptionString() + "'");

                if (dashboardCommissionTab.Length > 0)
                {
                    tabDashboard.TabPages.Add(tabCommissions);
                    tabCount++;

                    if (firstTabFlag)
                    {
                        tabCommissions.BringToFront();
                        firstTabFlag = false;

                        tabDashboard.SelectedTab = tabCommissions;
                    }
                }
            }
        }

        private void LoadClientDepartmentDetails(TreeView tv)
        {
            try
            {
                DataTable dtClients = null;
                DataTable dtDepartments = null;

                if (Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper())
                {
                    dtClients = clientBL.GetListDT();
                    dtDepartments = clientBL.GetClientDepartmentList();
                }
                else
                {
                    dtClients = clientBL.GetListDT(Program.currentUserId);
                    dtDepartments = clientBL.GetClientDepartmentListByUserId(Program.currentUserId);
                }

                tv.Nodes.Clear();

                foreach (DataRow drClient in dtClients.Rows)
                {
                    List<TreeNode> clientNameNodeList = new List<TreeNode>();

                    DataRow[] departments = dtDepartments.Select("ClientId = " + drClient["ClientId"].ToString() + "");

                    if (departments.Length > 0)
                    {
                        foreach (DataRow drDepartment in departments)
                        {
                            TreeNode DeptNameNode = new TreeNode(drDepartment["DepartmentName"].ToString());

                            DeptNameNode.Text = drDepartment["DepartmentName"].ToString();
                            DeptNameNode.ToolTipText = drDepartment["DepartmentName"].ToString();
                            DeptNameNode.Tag = "Department#" + drDepartment["ClientDepartmentId"].ToString() + "#" + drDepartment["ClientId"].ToString();

                            clientNameNodeList.Add(DeptNameNode);
                        }
                    }

                    TreeNode clientNameNode = new TreeNode(drClient["ClientName"].ToString(), clientNameNodeList.ToArray<TreeNode>());

                    clientNameNode.Text = drClient["ClientName"].ToString();
                    clientNameNode.ToolTipText = drClient["ClientName"].ToString();
                    clientNameNode.Tag = "Client#" + drClient["ClientId"].ToString();

                    tv.Nodes.Add(clientNameNode);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CheckAllChildNodes(TreeNode treeNode, bool nodeChecked)
        {
            foreach (TreeNode node in treeNode.Nodes)
            {
                node.Checked = nodeChecked;
                if (node.Nodes.Count > 0)
                {
                    this.CheckAllChildNodes(node, nodeChecked);
                }
            }
        }

        private void CheckAllNodes(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                node.Checked = true;
                CheckChildren(node, true);
            }
        }

        private void UncheckAllNodes(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                node.Checked = false;
                CheckChildren(node, false);
            }
        }

        private void CheckChildren(TreeNode rootNode, bool isChecked)
        {
            foreach (TreeNode node in rootNode.Nodes)
            {
                CheckChildren(node, isChecked);
                node.Checked = isChecked;
            }
        }

        private void LoadPerformanceTabData(string param = null, string order = null)
        {
            SelectionRange dtSelected = dtTestPerformed.SelectionRange;

            //Client Departments
            string clientDepartmentList = string.Empty;
            foreach (TreeNode clientNode in tvTestPerformed.Nodes)
            {
                if (clientNode.Nodes.Count > 0)
                {
                    foreach (TreeNode deptNode in clientNode.Nodes)
                    {
                        if (deptNode.Checked)
                        {
                            if (deptNode.Tag.ToString().StartsWith("Department"))
                            {
                                string[] dept = deptNode.Tag.ToString().Split('#');

                                if (dept.Length == 3)
                                {
                                    if (clientDepartmentList.Trim() == string.Empty)
                                    {
                                        clientDepartmentList = dept[1];
                                    }
                                    else
                                    {
                                        clientDepartmentList += "," + dept[1];
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //MessageBox.Show(dtSelected.Start.ToShortDateString());
            //MessageBox.Show(dtSelected.End.ToShortDateString());
            //MessageBox.Show(clientDepartmentList);

            DonorBL donorBL = new DonorBL();
            List<DonorTestInfo> donorGetPerformanceDetailsList = null;
            if (param == null)
            {
                donorGetPerformanceDetailsList = donorBL.GetPerformanceDetails(dtSelected.Start, dtSelected.End, clientDepartmentList);
            }
            else
            {
                donorGetPerformanceDetailsList = donorBL.GetPerformanceDetailsBySorting(dtSelected.Start, dtSelected.End, clientDepartmentList, param, order);
            }
            dgvTestPerformed.DataSource = donorGetPerformanceDetailsList;
            dgvTestPerformed.Focus();
        }

        private void LoadAccountingTabData()
        {
            SelectionRange dtSelected = dtAccounting.SelectionRange;

            //Client Departments
            string clientDepartmentList = string.Empty;
            foreach (TreeNode clientNode in tvAccounting.Nodes)
            {
                if (clientNode.Nodes.Count > 0)
                {
                    foreach (TreeNode deptNode in clientNode.Nodes)
                    {
                        if (deptNode.Checked)
                        {
                            if (deptNode.Tag.ToString().StartsWith("Department"))
                            {
                                string[] dept = deptNode.Tag.ToString().Split('#');

                                if (dept.Length == 3)
                                {
                                    if (clientDepartmentList.Trim() == string.Empty)
                                    {
                                        clientDepartmentList = dept[1];
                                    }
                                    else
                                    {
                                        clientDepartmentList += "," + dept[1];
                                    }
                                }
                            }
                        }
                    }
                }
            }

            txtUARevenue.Text = "0.00";
            txtHairRevenue.Text = "0.00";
            txtDNARevenue.Text = "0.00";
            txtTotalRevenue.Text = "0.00";

            txtLabCost.Text = "0.00";
            txtMROCost.Text = "0.00";
            txtVendorCost.Text = "0.00";
            txtCommissionCost.Text = "0.00";
            txtTotalCost.Text = "0.00";

            txtShippingCost.Text = "0.00";
            txtCupCost.Text = "0.00";

            txtGrossProfit.Text = "0.00";

            DonorBL donorBL = new DonorBL();
            DonorAccounting donorAccounting = donorBL.GetAccountingDetails(dtSelected.Start, dtSelected.End, clientDepartmentList);

            txtUARevenue.Text = Convert.ToDouble(donorAccounting.UARevenue).ToString("N2");
            txtHairRevenue.Text = Convert.ToDouble(donorAccounting.HairRevenue).ToString("N2");
            txtDNARevenue.Text = Convert.ToDouble(donorAccounting.DNARevenue).ToString("N2");

            txtTotalRevenue.Text = Convert.ToDouble(donorAccounting.TotalRevenue).ToString("N2");

            txtLabCost.Text = Convert.ToDouble(donorAccounting.LaboratoryCost).ToString("N2");
            txtMROCost.Text = Convert.ToDouble(donorAccounting.MROCost).ToString("N2");
            txtCupCost.Text = Convert.ToDouble(donorAccounting.CupCost).ToString("N2");
            txtShippingCost.Text = Convert.ToDouble(donorAccounting.ShippingCost).ToString("N2");
            txtVendorCost.Text = Convert.ToDouble(donorAccounting.VendorCost).ToString("N2");
            txtTotalCost.Text = Convert.ToDouble(donorAccounting.TotalCost).ToString("N2");

            txtGrossProfit.Text = Convert.ToDouble(donorAccounting.GrossProfit).ToString("N2");
        }

        #endregion Private Methods

        private void dtAccounting_DateChanged(object sender, DateRangeEventArgs e)
        {
            LoadAccountingTabData();
        }

        private void dtTestPerformed_DateChanged(object sender, DateRangeEventArgs e)
        {
            LoadPerformanceTabData();
        }

        private void dgvTestPerformed_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DonorBL donorBL = new DonorBL();
            // List<DonorTestInfo> donorGetPerformanceDetailsList = null;
            DataGridView dgv = sender as DataGridView;
            DataGridViewColumn col = dgv.Columns[e.ColumnIndex];
            if (col.Name == "Client")
            {
                DataGridViewColumn Client = dgv.Columns["Client"];
                string client = string.Empty;
                if (Client.HeaderCell.SortGlyphDirection == SortOrder.None || Client.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    // donorGetPerformanceDetailsList = donorBL.Sorting("client", "1");
                    LoadPerformanceTabData("client", "1");
                    // dgvTestPerformed.DataSource = donorGetPerformanceDetailsList;

                    Client.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                    // col.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
                else
                {
                    // donorGetPerformanceDetailsList = donorBL.Sorting("client");
                    LoadPerformanceTabData("client");
                    // dgvTestPerformed.DataSource = donorGetPerformanceDetailsList;
                    Client.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                    // col.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
            }
            else if (col.Name == "Department")
            {
                DataGridViewColumn Department = dgv.Columns["Department"];
                string department = string.Empty;
                if (Department.HeaderCell.SortGlyphDirection == SortOrder.None || Department.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    // donorGetPerformanceDetailsList = donorBL.Sorting("department", "1");
                    // dgvTestPerformed.DataSource = donorGetPerformanceDetailsList;
                    LoadPerformanceTabData("department", "1");
                    Department.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                    //col.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
                else
                {
                    //donorGetPerformanceDetailsList = donorBL.Sorting("department");
                    // dgvTestPerformed.DataSource = donorGetPerformanceDetailsList;
                    LoadPerformanceTabData("department");
                    Department.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                    //col.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
            }

            /*   else if (col.Name == "Registered")
               {
                   DataGridViewColumn Registered = dgv.Columns["Registered"];
                   string registered = string.Empty;
                   if (Registered.HeaderCell.SortGlyphDirection == SortOrder.None || Registered.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                   {
                       //donorGetPerformanceDetailsList = donorBL.Sorting("registered", "1");
                       //dgvTestPerformed.DataSource = donorGetPerformanceDetailsList;
                       LoadPerformanceTabData("registered", "1");
                       Registered.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                       // col.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                   }
                   else
                   {
                       //donorGetPerformanceDetailsList = donorBL.Sorting("registered");
                       //dgvTestPerformed.DataSource = donorGetPerformanceDetailsList;
                       LoadPerformanceTabData("registered");
                       Registered.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                       // col.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                   }
               }
               else if (col.Name == "InQueue")
               {
                   DataGridViewColumn InQueue = dgv.Columns["InQueue"];
                   string inQueue = string.Empty;
                   if (InQueue.HeaderCell.SortGlyphDirection == SortOrder.None || InQueue.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                   {
                       //donorGetPerformanceDetailsList = donorBL.Sorting("registered", "1");
                       //dgvTestPerformed.DataSource = donorGetPerformanceDetailsList;
                       LoadPerformanceTabData("inQueue", "1");
                       InQueue.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                       // col.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                   }
                   else
                   {
                       //donorGetPerformanceDetailsList = donorBL.Sorting("registered");
                       //dgvTestPerformed.DataSource = donorGetPerformanceDetailsList;
                       LoadPerformanceTabData("inQueue");
                       InQueue.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                       // col.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                   }
               }
               else if (col.Name == "SuspensionQueue")
               {
                   DataGridViewColumn SuspensionQueue = dgv.Columns["SuspensionQueue"];
                   string suspensionQueue = string.Empty;
                   if (SuspensionQueue.HeaderCell.SortGlyphDirection == SortOrder.None || SuspensionQueue.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                   {
                       //donorGetPerformanceDetailsList = donorBL.Sorting("suspensionQueue", "1");
                       //dgvTestPerformed.DataSource = donorGetPerformanceDetailsList;
                        LoadPerformanceTabData("suspensionQueue", "1");
                       SuspensionQueue.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                       // col.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                   }
                   else
                   {
                       //donorGetPerformanceDetailsList = donorBL.Sorting("suspensionQueue");
                       //dgvTestPerformed.DataSource = donorGetPerformanceDetailsList;
                       LoadPerformanceTabData("suspensionQueue");
                       SuspensionQueue.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                       // col.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                   }
               }
               else if (col.Name == "Processing")
               {
                   DataGridViewColumn Processing = dgv.Columns["Processing"];
                   string processing = string.Empty;
                   if (Processing.HeaderCell.SortGlyphDirection == SortOrder.None || Processing.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                   {
                       //donorGetPerformanceDetailsList = donorBL.Sorting("processing", "1");
                       //dgvTestPerformed.DataSource = donorGetPerformanceDetailsList;
                        LoadPerformanceTabData("processing", "1");
                       Processing.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                       //col.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                   }
                   else
                   {
                       //donorGetPerformanceDetailsList = donorBL.Sorting("processing");
                       //dgvTestPerformed.DataSource = donorGetPerformanceDetailsList;
                        LoadPerformanceTabData("processing");
                       Processing.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                       //col.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                   }
               }
               else if (col.Name == "Completed")
               {
                   DataGridViewColumn Completed = dgv.Columns["Completed"];
                   string completed = string.Empty;
                   if (Completed.HeaderCell.SortGlyphDirection == SortOrder.None || Completed.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                   {
                       //donorGetPerformanceDetailsList = donorBL.Sorting("completed", "1");
                       //dgvTestPerformed.DataSource = donorGetPerformanceDetailsList;
                        LoadPerformanceTabData("completed", "1");
                       Completed.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                       //col.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                   }
                   else
                   {
                       //donorGetPerformanceDetailsList = donorBL.Sorting("completed");
                       //dgvTestPerformed.DataSource = donorGetPerformanceDetailsList;
                        LoadPerformanceTabData("completed");
                       Completed.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                       //col.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                   }
               }*/
        }
    }
}