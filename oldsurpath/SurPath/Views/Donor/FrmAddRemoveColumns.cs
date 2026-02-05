using SurPath.Business;
using System;
using System.Data;
using System.Windows.Forms;

namespace SurPath
{
    public partial class FrmAddRemoveColumns : Form
    {
        #region Private Variables

        private FrmDonorSearch _frmDonorSearch;
        private DonorBL donorBL = new DonorBL();
        public bool IsValidate = true;
        public bool IsFormValidate = false;
        private string fieldList = string.Empty;
        private int count = 0;
        private int count1 = 0;
        private int count2 = 0;

        #endregion Private Variables

        #region Constructor

        public FrmAddRemoveColumns(FrmDonorSearch frmDonorSearch)
        {
            InitializeComponent();
            IsFormValidate = false;
            _frmDonorSearch = frmDonorSearch;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmAddRemoveColumns_FormClosing);
        }

        //public FrmAddRemoveColumns(FrmDonorSearch frmDonorSearch)
        //{
        //    _frmDonorSearch = frmDonorSearch;
        //    this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmAddRemoveColumns_FormClosing);
        //}

        #endregion Constructor

        #region Event Methods

        private void FrmAddRemoveColumns_Load(object sender, EventArgs e)
        {
            this.ControlBox = false;
            DataTable dtColumns = donorBL.ColumnsName();
            gvFieldList.DataSource = dtColumns;

            if (dtColumns.Rows.Count > 0)
            {
                for (int i = 0; i < dtColumns.Rows.Count; i++)
                {
                    if (dtColumns.Rows[i]["IsActive"].ToString() == "1")
                    {
                        gvFieldList.Rows[i].Cells["FieldSelect"].Value = true;
                    }
                    //if (dtColumns.Rows[i]["ColumnName"].ToString() == "First Name")
                    //{
                    //    gvFieldList.Rows[i].Cells["FieldSelect"].Value = true;
                    //}
                }
            }

            if (gvFieldList.Rows.Count > 0)
            {
                Cursor.Current = Cursors.WaitCursor;
                for (int i = 0; i < gvFieldList.Rows.Count; i++)
                {
                    DataGridViewCheckBoxCell FieldSelection = (DataGridViewCheckBoxCell)gvFieldList.Rows[i].Cells["FieldSelect"];

                    if (Convert.ToBoolean(FieldSelection.Value) == true)
                    {
                        count++;
                    }
                }
            }
        }

        private void gvFieldList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //Cursor.Current = Cursors.WaitCursor;
                //if (e.ColumnIndex == 0)
                //{
                //    if (e.RowIndex != -1)
                //    {
                //        DataGridViewCheckBoxCell fieldSelection = (DataGridViewCheckBoxCell)gvFieldList.Rows[e.RowIndex].Cells["FieldSelect"];
                //        if (Convert.ToBoolean(fieldSelection.Value))
                //        {
                //            fieldSelection.Value = false;
                //        }
                //        else
                //        {
                //            fieldSelection.Value = true;
                //        }
                //    }
                //}
                //Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                int count = 0;
                if (gvFieldList.Rows.Count > 0)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    for (int i = 0; i < gvFieldList.Rows.Count; i++)
                    {
                        DataGridViewCheckBoxCell FieldSelection = (DataGridViewCheckBoxCell)gvFieldList.Rows[i].Cells["FieldSelect"];

                        if (Convert.ToBoolean(FieldSelection.Value) == true)
                        {
                            count++;
                        }
                    }
                }

                if (count > 0)
                {
                    for (int i = 0; i < gvFieldList.Rows.Count; i++)
                    {
                        DataGridViewCheckBoxCell FieldSelection = (DataGridViewCheckBoxCell)gvFieldList.Rows[i].Cells["FieldSelect"];

                        if (Convert.ToBoolean(FieldSelection.Value))
                        {
                            string columnId = gvFieldList.Rows[i].Cells["ColumnId"].Value.ToString();
                            string columnName = gvFieldList.Rows[i].Cells["ColumnName"].Value.ToString();

                            if (columnId != string.Empty)
                            {
                                int returnVal = donorBL.ColumnsRemove(columnId, columnName);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Select a Name");
                    return;
                }
                if (count > 0)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    MessageBox.Show(count.ToString() + " columnName(s) Removed.");
                    this.Close();
                    IsValidate = true;
                    Cursor.Current = Cursors.Default;
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                int count = 0;
                if (gvFieldList.Rows.Count > 0)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    for (int i = 0; i < gvFieldList.Rows.Count; i++)
                    {
                        DataGridViewCheckBoxCell FieldSelection = (DataGridViewCheckBoxCell)gvFieldList.Rows[i].Cells["FieldSelect"];

                        if (Convert.ToBoolean(FieldSelection.Value) == true)
                        {
                            count++;
                        }
                    }
                }

                if (count > 0)
                {
                    for (int i = 0; i < gvFieldList.Rows.Count; i++)
                    {
                        DataGridViewCheckBoxCell FieldSelection = (DataGridViewCheckBoxCell)gvFieldList.Rows[i].Cells["FieldSelect"];

                        if (Convert.ToBoolean(FieldSelection.Value))
                        {
                            string columnId = gvFieldList.Rows[i].Cells["ColumnId"].Value.ToString();
                            string columnName = gvFieldList.Rows[i].Cells["ColumnName"].Value.ToString();
                            if (columnId != string.Empty)
                            {
                                int returnVal = donorBL.ColumnsAdd(columnId, columnName);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Select a Name");
                    return;
                }
                if (count > 0)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    MessageBox.Show(count.ToString() + " columnName(s) Added.");
                    this.Close();
                    IsValidate = true;
                    Cursor.Current = Cursors.Default;
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
            this.Close();
            IsValidate = false;
            IsFormValidate = true;
        }

        private void btnClose_TextChanged(object sender, EventArgs e)
        {
            btnClose.CausesValidation = false;
        }

        private void gvFieldList_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            fieldList = string.Empty;

            DataGridView.HitTestInfo info = gvFieldList.HitTest(e.X, e.Y);
            //if (info.RowIndex >= 0)
            //{
            if (gvFieldList.Rows.Count > 0 && e.ColumnIndex >= 0 && info.RowIndex == -1)
            {
                //for (int i = 0; i < gvFieldList.Rows.Count; i++)
                //{
                if (e.RowIndex != -1)
                {
                    DataGridViewCheckBoxCell fieldSelection = (DataGridViewCheckBoxCell)gvFieldList.Rows[e.RowIndex].Cells["FieldSelect"];
                    //   gvFieldList.Columns["FieldSelect"].ReadOnly = true;

                    if (Convert.ToBoolean(fieldSelection.Value))
                    {
                        fieldSelection.Value = false;
                        //count2++;
                    }
                    else
                    {
                        fieldSelection.Value = true;
                        //count1++;
                    }

                    if (Convert.ToBoolean(fieldSelection.Value) == true)
                    {
                        if (e.ColumnIndex == 0)
                        {
                            if ((bool)gvFieldList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == true)
                            {
                                fieldSelection.Value = true;
                                // count1++;
                            }
                        }
                        else if ((String)gvFieldList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != string.Empty)
                        {
                            fieldSelection.Value = true;
                            //count1++;
                            string text = (String)gvFieldList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                            if (!string.IsNullOrEmpty(text))
                            {
                                gvFieldList.DoDragDrop(text, DragDropEffects.Copy);
                            }
                        }
                    }
                    else if (Convert.ToBoolean(fieldSelection.Value) == false)
                    {
                        fieldSelection.Value = false;
                        //count2++;
                    }
                }
                //  gvFieldList.DoDragDrop(fieldList, DragDropEffects.Copy);
            }
            //}
        }

        private void gvFieldList_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void gvFieldList_DragOver(object sender, DragEventArgs e)
        {
            //  e.Effect = DragDropEffects.Copy;
            //if (e.Data.GetDataPresent(typeof(System.String)))
            //    e.Effect = DragDropEffects.Copy;
            //else
            //    e.Effect = DragDropEffects.None;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                // int count = 0;
                if (gvFieldList.Rows.Count > 0)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    for (int i = 0; i < gvFieldList.Rows.Count; i++)
                    {
                        DataGridViewCheckBoxCell FieldSelection = (DataGridViewCheckBoxCell)gvFieldList.Rows[i].Cells["FieldSelect"];

                        if (Convert.ToBoolean(FieldSelection.Value) == true)
                        {
                            count1++;
                        }
                    }
                }

                if (count1 > 0)
                {
                    for (int i = 0; i < gvFieldList.Rows.Count; i++)
                    {
                        DataGridViewCheckBoxCell FieldSelection = (DataGridViewCheckBoxCell)gvFieldList.Rows[i].Cells["FieldSelect"];

                        if (Convert.ToBoolean(FieldSelection.Value) == true)
                        {
                            string columnId = gvFieldList.Rows[i].Cells["ColumnId"].Value.ToString();
                            string columnName = gvFieldList.Rows[i].Cells["ColumnName"].Value.ToString();
                            //List<string> Columns = new List<string>();
                            //Columns.Add(columnName);
                            if (columnId != string.Empty)
                            {
                                int returnVal = donorBL.ColumnsAdd(columnId, columnName);
                            }
                        }
                        else
                        {
                            string columnId = gvFieldList.Rows[i].Cells["ColumnId"].Value.ToString();
                            string columnName = gvFieldList.Rows[i].Cells["ColumnName"].Value.ToString();
                            if (columnId != string.Empty)
                            {
                                int returnVal = donorBL.ColumnsRemove(columnId, columnName);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Select atleast one column");
                    return;
                }
                if (count1 > 0)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    int countadd;
                    if (count1 > count)
                    {
                        countadd = count1 - count;
                        MessageBox.Show(countadd.ToString() + " column(s) added.");
                    }
                    else if (count > count1)
                    {
                        countadd = count - count1;
                        MessageBox.Show(countadd.ToString() + " column(s) Removed.");
                    }
                    IsValidate = true;
                    IsFormValidate = false;
                    this.Close();
                    Cursor.Current = Cursors.Default;
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void btnOk_TextChanged(object sender, EventArgs e)
        {
            btnOk.CausesValidation = false;
        }

        private void FrmAddRemoveColumns_FormClosing(object sender, FormClosingEventArgs e)
        {
            _frmDonorSearch.AddRemoves();
        }

        #endregion Event Methods
    }
}