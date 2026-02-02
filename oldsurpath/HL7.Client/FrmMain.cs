using RTF;
using SurPath.Data;
using SurPath.Entity;
using SurPath.Enum;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace HL7.Client
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void BtnGetReport_Click(object sender, EventArgs e)
        {
            ReportType reportType = ReportType.LabReport;
            string specimenId = "0095159553";
            ReportInfo reportDetails = null;
            List<OBR_Info> obrList = null;

            RTFBuilderbase crlReport = null;

            rtbFileContent.Text = string.Empty;
            rtbCRLReport.Text = string.Empty;

            HL7ParserDao hl7ParserDao = new HL7ParserDao();
            hl7ParserDao.GetReport(reportType, specimenId, reportDetails, ref obrList, ref crlReport);

            if (crlReport != null)
            {
                rtbCRLReport.Rtf = crlReport.ToString();
            }

            dataGridView1.AutoGenerateColumns = false;
            dataGridView2.AutoGenerateColumns = false;
            dataGridView3.AutoGenerateColumns = false;
            dataGridView4.AutoGenerateColumns = false;

            dataGridView1.DataSource = null;
            dataGridView2.DataSource = null;
            dataGridView3.DataSource = null;
            dataGridView4.DataSource = null;

            dataGridView1.Columns[0].HeaderText = "Test Panel";
            dataGridView2.Columns[0].HeaderText = "Test Panel";
            dataGridView3.Columns[0].HeaderText = "Test Panel";
            dataGridView4.Columns[0].HeaderText = "Test Panel";

            if (obrList != null)
            {
                if (obrList.Count <= 4)
                {
                    int i = 1;
                    foreach (OBR_Info obr in obrList)
                    {
                        if (obr.SectionHeader.Contains("CONFIRMATION"))
                        {
                            if (obrList.Count == 4)
                            {
                                dataGridView4.DataSource = obr.observatinos;
                                dataGridView4.Columns[0].HeaderText = obr.SectionHeader;
                            }
                            else if (obrList.Count == 3)
                            {
                                dataGridView3.DataSource = obr.observatinos;
                                dataGridView3.Columns[0].HeaderText = obr.SectionHeader;
                            }
                            else if (obrList.Count == 2)
                            {
                                dataGridView2.DataSource = obr.observatinos;
                                dataGridView2.Columns[0].HeaderText = obr.SectionHeader;
                            }
                        }
                        else if (obr.SectionHeader.Contains("INITIAL TEST") || obr.SectionHeader.ToUpper().Contains("SUBSTANCE ABUSE PANEL"))
                        {
                            if (obrList.Count == 4)
                            {
                                dataGridView3.DataSource = obr.observatinos;
                                dataGridView3.Columns[0].HeaderText = obr.SectionHeader;
                            }
                            else if (obrList.Count == 3)
                            {
                                dataGridView2.DataSource = obr.observatinos;
                                dataGridView2.Columns[0].HeaderText = obr.SectionHeader;
                            }
                            else if (obrList.Count == 2)
                            {
                                dataGridView1.DataSource = obr.observatinos;
                                dataGridView1.Columns[0].HeaderText = obr.SectionHeader;
                            }
                        }
                        else
                        {
                            if (i == 1)
                            {
                                dataGridView1.DataSource = obr.observatinos;
                                dataGridView1.Columns[0].HeaderText = obr.SectionHeader;
                            }
                            else if (i == 2)
                            {
                                dataGridView2.DataSource = obr.observatinos;
                                dataGridView2.Columns[0].HeaderText = obr.SectionHeader;
                            }
                            else if (i == 3)
                            {
                                dataGridView3.DataSource = obr.observatinos;
                                dataGridView3.Columns[0].HeaderText = obr.SectionHeader;
                            }
                            else if (i == 4)
                            {
                                dataGridView4.DataSource = obr.observatinos;
                                dataGridView4.Columns[0].HeaderText = obr.SectionHeader;
                            }
                            i++;
                        }
                    }
                }
            }
        }

        private void dataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;

            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (row.Cells[1].Value != null && row.Cells[1].Value.ToString() != string.Empty)
                {
                    row.Cells[0].Value = row.Cells[1].Value.ToString() + " - " + row.Cells[2].Value.ToString();
                }
                else
                {
                    row.Cells[0].Value = row.Cells[2].Value;
                }

                if (row.Cells[7].Value != null && row.Cells[7].Value.ToString() != string.Empty)
                {
                    row.Cells[5].Value = row.Cells[7].Value.ToString() + " " + row.Cells[6].Value.ToString();
                }

                if (row.Cells[4].Value != null && row.Cells[4].Value.ToString() == "POS")
                {
                    row.Cells[4].Value = "POSITIVE";
                }
                else if (row.Cells[4].Value != null && row.Cells[4].Value.ToString() == "NEG")
                {
                    row.Cells[4].Value = "NEGATIVE";
                }
            }
        }

        private void btnSaveCRLReport_Click(object sender, EventArgs e)
        {
            // Create a SaveFileDialog to request a path and file name to save to.
            SaveFileDialog saveFile1 = new SaveFileDialog();

            // Initialize the SaveFileDialog to specify the RTF extention for the file.
            saveFile1.DefaultExt = "*.txt";
            saveFile1.Filter = "Text Files|*.txt";

            // Determine whether the user selected a file name from the saveFileDialog.
            if (saveFile1.ShowDialog() == System.Windows.Forms.DialogResult.OK &&
               saveFile1.FileName.Length > 0)
            {
                // Save the contents of the RichTextBox into the file.
                rtbCRLReport.SaveFile(saveFile1.FileName, RichTextBoxStreamType.PlainText);
                System.Diagnostics.Process.Start(saveFile1.FileName);
            }
        }
    }
}