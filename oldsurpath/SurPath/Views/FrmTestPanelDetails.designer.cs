namespace SurPath
{
    partial class FrmTestPanelDetails
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmTestPanelDetails));
            this.btnClose = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnDrugNotFound = new System.Windows.Forms.Button();
            this.lblSelectDrugs = new System.Windows.Forms.Label();
            this.lblCost = new System.Windows.Forms.Label();
            this.lblTestPanelCode = new System.Windows.Forms.Label();
            this.lblPageHeader = new System.Windows.Forms.Label();
            this.lblCategory = new System.Windows.Forms.Label();
            this.rbtnUA = new System.Windows.Forms.RadioButton();
            this.rbtnHair = new System.Windows.Forms.RadioButton();
            this.label6 = new System.Windows.Forms.Label();
            this.lblPanelMan = new System.Windows.Forms.Label();
            this.lblCategoryMan = new System.Windows.Forms.Label();
            this.lblCostMan = new System.Windows.Forms.Label();
            this.lblSelectDrugsMan = new System.Windows.Forms.Label();
            this.chkDrugNameList = new SurPath.Controls.CheckedListBoxes.SurCheckedListBox();
            this.txtCost = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtTestPanelCode = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtDescription = new SurPath.Controls.TextBoxes.SurTextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.chkActive = new System.Windows.Forms.CheckBox();
            this.rbtnBC = new System.Windows.Forms.RadioButton();
            this.rbtnDNA = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(230, 362);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 19;
            this.btnClose.Text = "&Cancel";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(147, 362);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 18;
            this.btnOk.Text = "&OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnDrugNotFound
            // 
            this.btnDrugNotFound.Location = new System.Drawing.Point(162, 327);
            this.btnDrugNotFound.Name = "btnDrugNotFound";
            this.btnDrugNotFound.Size = new System.Drawing.Size(141, 23);
            this.btnDrugNotFound.TabIndex = 17;
            this.btnDrugNotFound.Text = "&Drug not found";
            this.btnDrugNotFound.UseVisualStyleBackColor = true;
            this.btnDrugNotFound.Click += new System.EventHandler(this.btnDrugNotFound_Click);
            // 
            // lblSelectDrugs
            // 
            this.lblSelectDrugs.AutoSize = true;
            this.lblSelectDrugs.Location = new System.Drawing.Point(17, 182);
            this.lblSelectDrugs.Name = "lblSelectDrugs";
            this.lblSelectDrugs.Size = new System.Drawing.Size(134, 13);
            this.lblSelectDrugs.TabIndex = 14;
            this.lblSelectDrugs.Text = "Se&lect Drugs for Screening";
            // 
            // lblCost
            // 
            this.lblCost.AutoSize = true;
            this.lblCost.Location = new System.Drawing.Point(17, 148);
            this.lblCost.Name = "lblCost";
            this.lblCost.Size = new System.Drawing.Size(28, 13);
            this.lblCost.TabIndex = 11;
            this.lblCost.Text = "Co&st";
            // 
            // lblTestPanelCode
            // 
            this.lblTestPanelCode.AutoSize = true;
            this.lblTestPanelCode.Location = new System.Drawing.Point(17, 39);
            this.lblTestPanelCode.Name = "lblTestPanelCode";
            this.lblTestPanelCode.Size = new System.Drawing.Size(86, 13);
            this.lblTestPanelCode.TabIndex = 1;
            this.lblTestPanelCode.Text = "Test &Panel Code";
            // 
            // lblPageHeader
            // 
            this.lblPageHeader.AutoSize = true;
            this.lblPageHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPageHeader.Location = new System.Drawing.Point(16, 9);
            this.lblPageHeader.Name = "lblPageHeader";
            this.lblPageHeader.Size = new System.Drawing.Size(155, 20);
            this.lblPageHeader.TabIndex = 0;
            this.lblPageHeader.Text = "Test Panel Details";
            // 
            // lblCategory
            // 
            this.lblCategory.AutoSize = true;
            this.lblCategory.Location = new System.Drawing.Point(17, 114);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(49, 13);
            this.lblCategory.TabIndex = 7;
            this.lblCategory.Text = "Categor&y";
            // 
            // rbtnUA
            // 
            this.rbtnUA.AutoSize = true;
            this.rbtnUA.Location = new System.Drawing.Point(162, 112);
            this.rbtnUA.Name = "rbtnUA";
            this.rbtnUA.Size = new System.Drawing.Size(40, 17);
            this.rbtnUA.TabIndex = 9;
            this.rbtnUA.Text = "&UA";
            this.rbtnUA.UseVisualStyleBackColor = true;
            this.rbtnUA.CheckedChanged += new System.EventHandler(this.rbtnUA_CheckedChanged);
            this.rbtnUA.Click += new System.EventHandler(this.rbtnUA_Click);
            // 
            // rbtnHair
            // 
            this.rbtnHair.AutoSize = true;
            this.rbtnHair.Location = new System.Drawing.Point(220, 112);
            this.rbtnHair.Name = "rbtnHair";
            this.rbtnHair.Size = new System.Drawing.Size(44, 17);
            this.rbtnHair.TabIndex = 10;
            this.rbtnHair.Text = "&Hair";
            this.rbtnHair.UseVisualStyleBackColor = true;
            this.rbtnHair.CheckedChanged += new System.EventHandler(this.rbtnHair_CheckedChanged);
            this.rbtnHair.Click += new System.EventHandler(this.rbtnHair_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(280, 9);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(94, 13);
            this.label6.TabIndex = 20;
            this.label6.Text = "* Mandatory Fields";
            // 
            // lblPanelMan
            // 
            this.lblPanelMan.AutoSize = true;
            this.lblPanelMan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPanelMan.ForeColor = System.Drawing.Color.Red;
            this.lblPanelMan.Location = new System.Drawing.Point(103, 38);
            this.lblPanelMan.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPanelMan.Name = "lblPanelMan";
            this.lblPanelMan.Size = new System.Drawing.Size(13, 17);
            this.lblPanelMan.TabIndex = 2;
            this.lblPanelMan.Text = "*";
            // 
            // lblCategoryMan
            // 
            this.lblCategoryMan.AutoSize = true;
            this.lblCategoryMan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCategoryMan.ForeColor = System.Drawing.Color.Red;
            this.lblCategoryMan.Location = new System.Drawing.Point(66, 112);
            this.lblCategoryMan.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCategoryMan.Name = "lblCategoryMan";
            this.lblCategoryMan.Size = new System.Drawing.Size(13, 17);
            this.lblCategoryMan.TabIndex = 8;
            this.lblCategoryMan.Text = "*";
            // 
            // lblCostMan
            // 
            this.lblCostMan.AutoSize = true;
            this.lblCostMan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCostMan.ForeColor = System.Drawing.Color.Red;
            this.lblCostMan.Location = new System.Drawing.Point(44, 147);
            this.lblCostMan.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCostMan.Name = "lblCostMan";
            this.lblCostMan.Size = new System.Drawing.Size(13, 17);
            this.lblCostMan.TabIndex = 12;
            this.lblCostMan.Text = "*";
            // 
            // lblSelectDrugsMan
            // 
            this.lblSelectDrugsMan.AutoSize = true;
            this.lblSelectDrugsMan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelectDrugsMan.ForeColor = System.Drawing.Color.Red;
            this.lblSelectDrugsMan.Location = new System.Drawing.Point(148, 182);
            this.lblSelectDrugsMan.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSelectDrugsMan.Name = "lblSelectDrugsMan";
            this.lblSelectDrugsMan.Size = new System.Drawing.Size(13, 17);
            this.lblSelectDrugsMan.TabIndex = 15;
            this.lblSelectDrugsMan.Text = "*";
            // 
            // chkDrugNameList
            // 
            this.chkDrugNameList.CheckOnClick = true;
            this.chkDrugNameList.DataSource = null;
            this.chkDrugNameList.FormattingEnabled = true;
            this.chkDrugNameList.Location = new System.Drawing.Point(162, 182);
            this.chkDrugNameList.Name = "chkDrugNameList";
            this.chkDrugNameList.Size = new System.Drawing.Size(217, 139);
            this.chkDrugNameList.TabIndex = 16;
            this.chkDrugNameList.ValueList = ((System.Collections.Generic.List<int>)(resources.GetObject("chkDrugNameList.ValueList")));
            this.chkDrugNameList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.chkDrugNameList_ItemCheck);
            // 
            // txtCost
            // 
            this.txtCost.Location = new System.Drawing.Point(162, 145);
            this.txtCost.MaxLength = 6;
            this.txtCost.Name = "txtCost";
            this.txtCost.Size = new System.Drawing.Size(217, 20);
            this.txtCost.TabIndex = 13;
            this.txtCost.WaterMark = "Enter Cost";
            this.txtCost.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtCost.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCost.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtCost.TextChanged += new System.EventHandler(this.txtCost_TextChanged);
            this.txtCost.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCost_KeyPress);
            // 
            // txtTestPanelCode
            // 
            this.txtTestPanelCode.Location = new System.Drawing.Point(162, 36);
            this.txtTestPanelCode.MaxLength = 200;
            this.txtTestPanelCode.Name = "txtTestPanelCode";
            this.txtTestPanelCode.Size = new System.Drawing.Size(217, 20);
            this.txtTestPanelCode.TabIndex = 3;
            this.txtTestPanelCode.WaterMark = "Enter Panel Code";
            this.txtTestPanelCode.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtTestPanelCode.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTestPanelCode.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtTestPanelCode.TextChanged += new System.EventHandler(this.txtTestPanelCode_TextChanged);
            this.txtTestPanelCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTestPanelCode_KeyPress);
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(163, 73);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(217, 20);
            this.txtDescription.TabIndex = 6;
            this.txtDescription.WaterMark = "Enter Description";
            this.txtDescription.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtDescription.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDescription.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtDescription.TextChanged += new System.EventHandler(this.txtDescription_TextChanged);
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(18, 73);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(60, 13);
            this.lblDescription.TabIndex = 5;
            this.lblDescription.Text = "D&escription";
            // 
            // chkActive
            // 
            this.chkActive.AutoSize = true;
            this.chkActive.Checked = true;
            this.chkActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkActive.Location = new System.Drawing.Point(382, 38);
            this.chkActive.Name = "chkActive";
            this.chkActive.Size = new System.Drawing.Size(56, 17);
            this.chkActive.TabIndex = 4;
            this.chkActive.Text = "Active";
            this.chkActive.UseVisualStyleBackColor = true;
            this.chkActive.CheckedChanged += new System.EventHandler(this.chkActive_CheckedChanged);
            // 
            // rbtnBC
            // 
            this.rbtnBC.AutoSize = true;
            this.rbtnBC.Location = new System.Drawing.Point(270, 112);
            this.rbtnBC.Name = "rbtnBC";
            this.rbtnBC.Size = new System.Drawing.Size(39, 17);
            this.rbtnBC.TabIndex = 21;
            this.rbtnBC.Text = "&BC";
            this.rbtnBC.UseVisualStyleBackColor = true;
            this.rbtnBC.CheckedChanged += new System.EventHandler(this.rbtnBC_CheckedChanged);
            // 
            // rbtnDNA
            // 
            this.rbtnDNA.AutoSize = true;
            this.rbtnDNA.Location = new System.Drawing.Point(320, 112);
            this.rbtnDNA.Name = "rbtnDNA";
            this.rbtnDNA.Size = new System.Drawing.Size(48, 17);
            this.rbtnDNA.TabIndex = 22;
            this.rbtnDNA.Text = "D&NA";
            this.rbtnDNA.UseVisualStyleBackColor = true;
            this.rbtnDNA.CheckedChanged += new System.EventHandler(this.rbtnDNA_CheckedChanged);
            // 
            // FrmTestPanelDetails
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(453, 396);
            this.Controls.Add(this.rbtnDNA);
            this.Controls.Add(this.rbtnBC);
            this.Controls.Add(this.chkActive);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.lblSelectDrugsMan);
            this.Controls.Add(this.lblCostMan);
            this.Controls.Add(this.lblCategoryMan);
            this.Controls.Add(this.lblPanelMan);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.chkDrugNameList);
            this.Controls.Add(this.txtCost);
            this.Controls.Add(this.txtTestPanelCode);
            this.Controls.Add(this.rbtnHair);
            this.Controls.Add(this.rbtnUA);
            this.Controls.Add(this.lblPageHeader);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnDrugNotFound);
            this.Controls.Add(this.lblSelectDrugs);
            this.Controls.Add(this.lblCost);
            this.Controls.Add(this.lblCategory);
            this.Controls.Add(this.lblTestPanelCode);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmTestPanelDetails";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Test Panel Details";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmTestPanelDetails_FormClosing);
            this.Load += new System.EventHandler(this.FrmTestPanelDetails_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnDrugNotFound;
        private System.Windows.Forms.Label lblSelectDrugs;
        private System.Windows.Forms.Label lblCost;
        private System.Windows.Forms.Label lblTestPanelCode;
        private System.Windows.Forms.Label lblPageHeader;
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.RadioButton rbtnUA;
        private System.Windows.Forms.RadioButton rbtnHair;
        private Controls.TextBoxes.SurTextBox txtTestPanelCode;
        private Controls.TextBoxes.SurTextBox txtCost;
        private Controls.CheckedListBoxes.SurCheckedListBox chkDrugNameList;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblPanelMan;
        private System.Windows.Forms.Label lblCategoryMan;
        private System.Windows.Forms.Label lblCostMan;
        private System.Windows.Forms.Label lblSelectDrugsMan;
        private Controls.TextBoxes.SurTextBox txtDescription;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.CheckBox chkActive;
        private System.Windows.Forms.RadioButton rbtnBC;
        private System.Windows.Forms.RadioButton rbtnDNA;
    }
}