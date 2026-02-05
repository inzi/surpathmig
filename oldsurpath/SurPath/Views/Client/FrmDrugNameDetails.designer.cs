namespace SurPath
{
    partial class FrmDrugNameDetails
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDrugNameDetails));
            this.btnClose = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.chkHair = new System.Windows.Forms.CheckBox();
            this.chkUA = new System.Windows.Forms.CheckBox();
            this.lblDrugName = new System.Windows.Forms.Label();
            this.lblPageHeader = new System.Windows.Forms.Label();
            this.lblCategories = new System.Windows.Forms.Label();
            this.lblDrugsMandatory = new System.Windows.Forms.Label();
            this.lblDrugNameMan = new System.Windows.Forms.Label();
            this.lblCategoriesMan = new System.Windows.Forms.Label();
            this.lblDrugCodeMan = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblUAUnitOfMeasurement = new System.Windows.Forms.Label();
            this.grbUA = new System.Windows.Forms.GroupBox();
            this.txtUAUOM = new SurPath.Controls.TextBoxes.SurTextBox();
            this.cmbUAUOM = new System.Windows.Forms.ComboBox();
            this.lblUAConfirmMan = new System.Windows.Forms.Label();
            this.lblUAScreenMan = new System.Windows.Forms.Label();
            this.txtUAConfirmationValue = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtUAScreenValue = new SurPath.Controls.TextBoxes.SurTextBox();
            this.lblUAConfirmationValue = new System.Windows.Forms.Label();
            this.lblUAScreenValue = new System.Windows.Forms.Label();
            this.grbHair = new System.Windows.Forms.GroupBox();
            this.txtHairUOM = new SurPath.Controls.TextBoxes.SurTextBox();
            this.cmbHairUOM = new System.Windows.Forms.ComboBox();
            this.lblHairConfirmationMan = new System.Windows.Forms.Label();
            this.lblHairSceenValue = new System.Windows.Forms.Label();
            this.lblHairScreenMan = new System.Windows.Forms.Label();
            this.lblHairConfirmationValue = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtHairConfirmationValue = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtHairScreenValue = new SurPath.Controls.TextBoxes.SurTextBox();
            this.lblHairUnitOfMeasurement = new System.Windows.Forms.Label();
            this.chkActive = new System.Windows.Forms.CheckBox();
            this.txtDrugCode = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtDrugName = new SurPath.Controls.TextBoxes.SurTextBox();
            this.chkBC = new System.Windows.Forms.CheckBox();
            this.chkDNA = new System.Windows.Forms.CheckBox();
            this.grbUA.SuspendLayout();
            this.grbHair.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(280, 279);
            this.btnClose.Name = "btnClose";
            this.btnClose.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 17;
            this.btnClose.Text = "&Cancel";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(189, 280);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 16;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // chkHair
            // 
            this.chkHair.AutoSize = true;
            this.chkHair.Location = new System.Drawing.Point(202, 112);
            this.chkHair.Name = "chkHair";
            this.chkHair.Size = new System.Drawing.Size(45, 17);
            this.chkHair.TabIndex = 10;
            this.chkHair.Text = "&Hair";
            this.chkHair.UseVisualStyleBackColor = true;
            this.chkHair.CheckedChanged += new System.EventHandler(this.chkHair_CheckedChanged);
            this.chkHair.Click += new System.EventHandler(this.chkHair_Click);
            // 
            // chkUA
            // 
            this.chkUA.AutoSize = true;
            this.chkUA.Checked = true;
            this.chkUA.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUA.Location = new System.Drawing.Point(134, 112);
            this.chkUA.Name = "chkUA";
            this.chkUA.Size = new System.Drawing.Size(41, 17);
            this.chkUA.TabIndex = 9;
            this.chkUA.Text = "&UA";
            this.chkUA.UseVisualStyleBackColor = true;
            this.chkUA.CheckedChanged += new System.EventHandler(this.chkUA_CheckedChanged);
            this.chkUA.Click += new System.EventHandler(this.chkUA_Click);
            // 
            // lblDrugName
            // 
            this.lblDrugName.AutoSize = true;
            this.lblDrugName.Location = new System.Drawing.Point(14, 42);
            this.lblDrugName.Name = "lblDrugName";
            this.lblDrugName.Size = new System.Drawing.Size(61, 13);
            this.lblDrugName.TabIndex = 1;
            this.lblDrugName.Text = "&Drug Name";
            // 
            // lblPageHeader
            // 
            this.lblPageHeader.AutoSize = true;
            this.lblPageHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPageHeader.Location = new System.Drawing.Point(12, 9);
            this.lblPageHeader.Name = "lblPageHeader";
            this.lblPageHeader.Size = new System.Drawing.Size(160, 20);
            this.lblPageHeader.TabIndex = 0;
            this.lblPageHeader.Text = "Drug Name Details";
            // 
            // lblCategories
            // 
            this.lblCategories.AutoSize = true;
            this.lblCategories.Location = new System.Drawing.Point(14, 112);
            this.lblCategories.Name = "lblCategories";
            this.lblCategories.Size = new System.Drawing.Size(81, 13);
            this.lblCategories.TabIndex = 7;
            this.lblCategories.Text = "Test C&ategories";
            // 
            // lblDrugsMandatory
            // 
            this.lblDrugsMandatory.AutoSize = true;
            this.lblDrugsMandatory.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDrugsMandatory.ForeColor = System.Drawing.Color.Red;
            this.lblDrugsMandatory.Location = new System.Drawing.Point(442, 16);
            this.lblDrugsMandatory.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDrugsMandatory.Name = "lblDrugsMandatory";
            this.lblDrugsMandatory.Size = new System.Drawing.Size(94, 13);
            this.lblDrugsMandatory.TabIndex = 18;
            this.lblDrugsMandatory.Text = "* Mandatory Fields";
            // 
            // lblDrugNameMan
            // 
            this.lblDrugNameMan.AutoSize = true;
            this.lblDrugNameMan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDrugNameMan.ForeColor = System.Drawing.Color.Red;
            this.lblDrugNameMan.Location = new System.Drawing.Point(71, 40);
            this.lblDrugNameMan.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDrugNameMan.Name = "lblDrugNameMan";
            this.lblDrugNameMan.Size = new System.Drawing.Size(13, 17);
            this.lblDrugNameMan.TabIndex = 2;
            this.lblDrugNameMan.Text = "*";
            // 
            // lblCategoriesMan
            // 
            this.lblCategoriesMan.AutoSize = true;
            this.lblCategoriesMan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCategoriesMan.ForeColor = System.Drawing.Color.Red;
            this.lblCategoriesMan.Location = new System.Drawing.Point(95, 110);
            this.lblCategoriesMan.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCategoriesMan.Name = "lblCategoriesMan";
            this.lblCategoriesMan.Size = new System.Drawing.Size(13, 17);
            this.lblCategoriesMan.TabIndex = 8;
            this.lblCategoriesMan.Text = "*";
            // 
            // lblDrugCodeMan
            // 
            this.lblDrugCodeMan.AutoSize = true;
            this.lblDrugCodeMan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDrugCodeMan.ForeColor = System.Drawing.Color.Red;
            this.lblDrugCodeMan.Location = new System.Drawing.Point(71, 73);
            this.lblDrugCodeMan.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDrugCodeMan.Name = "lblDrugCodeMan";
            this.lblDrugCodeMan.Size = new System.Drawing.Size(13, 17);
            this.lblDrugCodeMan.TabIndex = 5;
            this.lblDrugCodeMan.Text = "*";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Drug C&ode";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(108, 92);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(13, 17);
            this.label1.TabIndex = 14;
            this.label1.Text = "*";
            // 
            // lblUAUnitOfMeasurement
            // 
            this.lblUAUnitOfMeasurement.AutoSize = true;
            this.lblUAUnitOfMeasurement.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUAUnitOfMeasurement.Location = new System.Drawing.Point(3, 94);
            this.lblUAUnitOfMeasurement.Name = "lblUAUnitOfMeasurement";
            this.lblUAUnitOfMeasurement.Size = new System.Drawing.Size(107, 13);
            this.lblUAUnitOfMeasurement.TabIndex = 13;
            this.lblUAUnitOfMeasurement.Text = "Unit Of &Measurement";
            // 
            // grbUA
            // 
            this.grbUA.Controls.Add(this.txtUAUOM);
            this.grbUA.Controls.Add(this.cmbUAUOM);
            this.grbUA.Controls.Add(this.lblUAConfirmMan);
            this.grbUA.Controls.Add(this.lblUAScreenMan);
            this.grbUA.Controls.Add(this.txtUAConfirmationValue);
            this.grbUA.Controls.Add(this.txtUAScreenValue);
            this.grbUA.Controls.Add(this.label1);
            this.grbUA.Controls.Add(this.lblUAConfirmationValue);
            this.grbUA.Controls.Add(this.lblUAScreenValue);
            this.grbUA.Controls.Add(this.lblUAUnitOfMeasurement);
            this.grbUA.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grbUA.Location = new System.Drawing.Point(14, 137);
            this.grbUA.Name = "grbUA";
            this.grbUA.Size = new System.Drawing.Size(255, 119);
            this.grbUA.TabIndex = 11;
            this.grbUA.TabStop = false;
            this.grbUA.Text = "UA";
            // 
            // txtUAUOM
            // 
            this.txtUAUOM.Enabled = false;
            this.txtUAUOM.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUAUOM.Location = new System.Drawing.Point(123, 90);
            this.txtUAUOM.MaxLength = 10;
            this.txtUAUOM.Name = "txtUAUOM";
            this.txtUAUOM.Size = new System.Drawing.Size(127, 20);
            this.txtUAUOM.TabIndex = 31;
            this.txtUAUOM.Visible = false;
            this.txtUAUOM.WaterMark = "Enter Confirmation Value";
            this.txtUAUOM.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtUAUOM.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUAUOM.WaterMarkForeColor = System.Drawing.Color.LightGray;
            // 
            // cmbUAUOM
            // 
            this.cmbUAUOM.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUAUOM.Enabled = false;
            this.cmbUAUOM.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbUAUOM.FormattingEnabled = true;
            this.cmbUAUOM.Items.AddRange(new object[] {
            "(Select)",
            "ng/ml"});
            this.cmbUAUOM.Location = new System.Drawing.Point(123, 90);
            this.cmbUAUOM.Name = "cmbUAUOM";
            this.cmbUAUOM.Size = new System.Drawing.Size(127, 21);
            this.cmbUAUOM.TabIndex = 30;
            this.cmbUAUOM.SelectedIndexChanged += new System.EventHandler(this.cmbUAUOM_SelectedIndexChanged);
            this.cmbUAUOM.TextChanged += new System.EventHandler(this.cmbUAUOM_TextChanged);
            // 
            // lblUAConfirmMan
            // 
            this.lblUAConfirmMan.AutoSize = true;
            this.lblUAConfirmMan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUAConfirmMan.ForeColor = System.Drawing.Color.Red;
            this.lblUAConfirmMan.Location = new System.Drawing.Point(100, 61);
            this.lblUAConfirmMan.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblUAConfirmMan.Name = "lblUAConfirmMan";
            this.lblUAConfirmMan.Size = new System.Drawing.Size(13, 17);
            this.lblUAConfirmMan.TabIndex = 4;
            this.lblUAConfirmMan.Text = "*";
            // 
            // lblUAScreenMan
            // 
            this.lblUAScreenMan.AutoSize = true;
            this.lblUAScreenMan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUAScreenMan.ForeColor = System.Drawing.Color.Red;
            this.lblUAScreenMan.Location = new System.Drawing.Point(77, 28);
            this.lblUAScreenMan.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblUAScreenMan.Name = "lblUAScreenMan";
            this.lblUAScreenMan.Size = new System.Drawing.Size(13, 17);
            this.lblUAScreenMan.TabIndex = 1;
            this.lblUAScreenMan.Text = "*";
            // 
            // txtUAConfirmationValue
            // 
            this.txtUAConfirmationValue.Enabled = false;
            this.txtUAConfirmationValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUAConfirmationValue.Location = new System.Drawing.Point(123, 59);
            this.txtUAConfirmationValue.MaxLength = 10;
            this.txtUAConfirmationValue.Name = "txtUAConfirmationValue";
            this.txtUAConfirmationValue.Size = new System.Drawing.Size(127, 20);
            this.txtUAConfirmationValue.TabIndex = 5;
            this.txtUAConfirmationValue.WaterMark = "Enter Confirmation Value";
            this.txtUAConfirmationValue.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtUAConfirmationValue.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUAConfirmationValue.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtUAConfirmationValue.TextChanged += new System.EventHandler(this.txtUAConfirmationValue_TextChanged);
            this.txtUAConfirmationValue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtUAConfirmationValue_KeyPress);
            // 
            // txtUAScreenValue
            // 
            this.txtUAScreenValue.Enabled = false;
            this.txtUAScreenValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUAScreenValue.Location = new System.Drawing.Point(121, 26);
            this.txtUAScreenValue.MaxLength = 10;
            this.txtUAScreenValue.Name = "txtUAScreenValue";
            this.txtUAScreenValue.Size = new System.Drawing.Size(129, 20);
            this.txtUAScreenValue.TabIndex = 2;
            this.txtUAScreenValue.WaterMark = "Enter Screen Value";
            this.txtUAScreenValue.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtUAScreenValue.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUAScreenValue.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtUAScreenValue.TextChanged += new System.EventHandler(this.txtUAScreenValue_TextChanged);
            this.txtUAScreenValue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtUAScreenValue_KeyPress);
            // 
            // lblUAConfirmationValue
            // 
            this.lblUAConfirmationValue.AutoSize = true;
            this.lblUAConfirmationValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUAConfirmationValue.Location = new System.Drawing.Point(3, 63);
            this.lblUAConfirmationValue.Name = "lblUAConfirmationValue";
            this.lblUAConfirmationValue.Size = new System.Drawing.Size(95, 13);
            this.lblUAConfirmationValue.TabIndex = 3;
            this.lblUAConfirmationValue.Text = "Confirmation Value";
            // 
            // lblUAScreenValue
            // 
            this.lblUAScreenValue.AutoSize = true;
            this.lblUAScreenValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUAScreenValue.Location = new System.Drawing.Point(3, 30);
            this.lblUAScreenValue.Name = "lblUAScreenValue";
            this.lblUAScreenValue.Size = new System.Drawing.Size(71, 13);
            this.lblUAScreenValue.TabIndex = 0;
            this.lblUAScreenValue.Text = "Screen Value";
            // 
            // grbHair
            // 
            this.grbHair.Controls.Add(this.txtHairUOM);
            this.grbHair.Controls.Add(this.cmbHairUOM);
            this.grbHair.Controls.Add(this.lblHairConfirmationMan);
            this.grbHair.Controls.Add(this.lblHairSceenValue);
            this.grbHair.Controls.Add(this.lblHairScreenMan);
            this.grbHair.Controls.Add(this.lblHairConfirmationValue);
            this.grbHair.Controls.Add(this.label4);
            this.grbHair.Controls.Add(this.txtHairConfirmationValue);
            this.grbHair.Controls.Add(this.txtHairScreenValue);
            this.grbHair.Controls.Add(this.lblHairUnitOfMeasurement);
            this.grbHair.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grbHair.Location = new System.Drawing.Point(280, 137);
            this.grbHair.Name = "grbHair";
            this.grbHair.Size = new System.Drawing.Size(261, 119);
            this.grbHair.TabIndex = 12;
            this.grbHair.TabStop = false;
            this.grbHair.Text = "Hair";
            this.grbHair.Enter += new System.EventHandler(this.grbHair_Enter);
            // 
            // txtHairUOM
            // 
            this.txtHairUOM.Enabled = false;
            this.txtHairUOM.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHairUOM.Location = new System.Drawing.Point(124, 90);
            this.txtHairUOM.MaxLength = 10;
            this.txtHairUOM.Name = "txtHairUOM";
            this.txtHairUOM.Size = new System.Drawing.Size(129, 20);
            this.txtHairUOM.TabIndex = 32;
            this.txtHairUOM.Visible = false;
            this.txtHairUOM.WaterMark = "Enter Confirmation Value";
            this.txtHairUOM.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtHairUOM.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHairUOM.WaterMarkForeColor = System.Drawing.Color.LightGray;
            // 
            // cmbHairUOM
            // 
            this.cmbHairUOM.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbHairUOM.Enabled = false;
            this.cmbHairUOM.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbHairUOM.FormattingEnabled = true;
            this.cmbHairUOM.Items.AddRange(new object[] {
            "(Select)",
            "pg/mg"});
            this.cmbHairUOM.Location = new System.Drawing.Point(124, 90);
            this.cmbHairUOM.Name = "cmbHairUOM";
            this.cmbHairUOM.Size = new System.Drawing.Size(129, 21);
            this.cmbHairUOM.TabIndex = 30;
            this.cmbHairUOM.SelectedIndexChanged += new System.EventHandler(this.cmbHairUOM_SelectedIndexChanged);
            // 
            // lblHairConfirmationMan
            // 
            this.lblHairConfirmationMan.AutoSize = true;
            this.lblHairConfirmationMan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHairConfirmationMan.ForeColor = System.Drawing.Color.Red;
            this.lblHairConfirmationMan.Location = new System.Drawing.Point(98, 61);
            this.lblHairConfirmationMan.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblHairConfirmationMan.Name = "lblHairConfirmationMan";
            this.lblHairConfirmationMan.Size = new System.Drawing.Size(13, 17);
            this.lblHairConfirmationMan.TabIndex = 4;
            this.lblHairConfirmationMan.Text = "*";
            // 
            // lblHairSceenValue
            // 
            this.lblHairSceenValue.AutoSize = true;
            this.lblHairSceenValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHairSceenValue.Location = new System.Drawing.Point(7, 30);
            this.lblHairSceenValue.Name = "lblHairSceenValue";
            this.lblHairSceenValue.Size = new System.Drawing.Size(71, 13);
            this.lblHairSceenValue.TabIndex = 0;
            this.lblHairSceenValue.Text = "Screen Value";
            // 
            // lblHairScreenMan
            // 
            this.lblHairScreenMan.AutoSize = true;
            this.lblHairScreenMan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHairScreenMan.ForeColor = System.Drawing.Color.Red;
            this.lblHairScreenMan.Location = new System.Drawing.Point(75, 28);
            this.lblHairScreenMan.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblHairScreenMan.Name = "lblHairScreenMan";
            this.lblHairScreenMan.Size = new System.Drawing.Size(13, 17);
            this.lblHairScreenMan.TabIndex = 1;
            this.lblHairScreenMan.Text = "*";
            // 
            // lblHairConfirmationValue
            // 
            this.lblHairConfirmationValue.AutoSize = true;
            this.lblHairConfirmationValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHairConfirmationValue.Location = new System.Drawing.Point(7, 63);
            this.lblHairConfirmationValue.Name = "lblHairConfirmationValue";
            this.lblHairConfirmationValue.Size = new System.Drawing.Size(95, 13);
            this.lblHairConfirmationValue.TabIndex = 3;
            this.lblHairConfirmationValue.Text = "Confirmation Value";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(111, 92);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(13, 17);
            this.label4.TabIndex = 14;
            this.label4.Text = "*";
            // 
            // txtHairConfirmationValue
            // 
            this.txtHairConfirmationValue.Enabled = false;
            this.txtHairConfirmationValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHairConfirmationValue.Location = new System.Drawing.Point(124, 59);
            this.txtHairConfirmationValue.MaxLength = 10;
            this.txtHairConfirmationValue.Name = "txtHairConfirmationValue";
            this.txtHairConfirmationValue.Size = new System.Drawing.Size(129, 20);
            this.txtHairConfirmationValue.TabIndex = 5;
            this.txtHairConfirmationValue.WaterMark = "Enter Confirmation Value";
            this.txtHairConfirmationValue.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtHairConfirmationValue.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHairConfirmationValue.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtHairConfirmationValue.TextChanged += new System.EventHandler(this.txtHairConfirmationValue_TextChanged);
            this.txtHairConfirmationValue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtHairConfirmationValue_KeyPress);
            // 
            // txtHairScreenValue
            // 
            this.txtHairScreenValue.Enabled = false;
            this.txtHairScreenValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHairScreenValue.Location = new System.Drawing.Point(124, 26);
            this.txtHairScreenValue.MaxLength = 10;
            this.txtHairScreenValue.Name = "txtHairScreenValue";
            this.txtHairScreenValue.Size = new System.Drawing.Size(129, 20);
            this.txtHairScreenValue.TabIndex = 2;
            this.txtHairScreenValue.WaterMark = "Enter Screen Value";
            this.txtHairScreenValue.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtHairScreenValue.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHairScreenValue.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtHairScreenValue.TextChanged += new System.EventHandler(this.txtHairScreenValue_TextChanged);
            this.txtHairScreenValue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtHairScreenValue_KeyPress);
            // 
            // lblHairUnitOfMeasurement
            // 
            this.lblHairUnitOfMeasurement.AutoSize = true;
            this.lblHairUnitOfMeasurement.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHairUnitOfMeasurement.Location = new System.Drawing.Point(6, 94);
            this.lblHairUnitOfMeasurement.Name = "lblHairUnitOfMeasurement";
            this.lblHairUnitOfMeasurement.Size = new System.Drawing.Size(107, 13);
            this.lblHairUnitOfMeasurement.TabIndex = 13;
            this.lblHairUnitOfMeasurement.Text = "Unit Of &Measurement";
            // 
            // chkActive
            // 
            this.chkActive.AutoSize = true;
            this.chkActive.Checked = true;
            this.chkActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkActive.Location = new System.Drawing.Point(332, 43);
            this.chkActive.Name = "chkActive";
            this.chkActive.Size = new System.Drawing.Size(56, 17);
            this.chkActive.TabIndex = 29;
            this.chkActive.Text = "Active";
            this.chkActive.UseVisualStyleBackColor = true;
            this.chkActive.CheckedChanged += new System.EventHandler(this.chkActive_CheckedChanged);
            // 
            // txtDrugCode
            // 
            this.txtDrugCode.Location = new System.Drawing.Point(134, 74);
            this.txtDrugCode.MaxLength = 200;
            this.txtDrugCode.Name = "txtDrugCode";
            this.txtDrugCode.Size = new System.Drawing.Size(191, 20);
            this.txtDrugCode.TabIndex = 6;
            this.txtDrugCode.WaterMark = "Enter Drug Code";
            this.txtDrugCode.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtDrugCode.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDrugCode.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtDrugCode.TextChanged += new System.EventHandler(this.txtDrugCode_TextChanged);
            this.txtDrugCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDrugCode_KeyPress);
            // 
            // txtDrugName
            // 
            this.txtDrugName.Location = new System.Drawing.Point(134, 41);
            this.txtDrugName.MaxLength = 200;
            this.txtDrugName.Name = "txtDrugName";
            this.txtDrugName.Size = new System.Drawing.Size(191, 20);
            this.txtDrugName.TabIndex = 3;
            this.txtDrugName.WaterMark = "Enter Drug Name";
            this.txtDrugName.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtDrugName.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDrugName.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtDrugName.TextChanged += new System.EventHandler(this.txtDrugName_TextChanged);
            // 
            // chkBC
            // 
            this.chkBC.AutoSize = true;
            this.chkBC.Location = new System.Drawing.Point(265, 112);
            this.chkBC.Name = "chkBC";
            this.chkBC.Size = new System.Drawing.Size(40, 17);
            this.chkBC.TabIndex = 30;
            this.chkBC.Text = "&BC";
            this.chkBC.UseVisualStyleBackColor = true;
            this.chkBC.CheckedChanged += new System.EventHandler(this.chkBC_CheckedChanged);
            // 
            // chkDNA
            // 
            this.chkDNA.AutoSize = true;
            this.chkDNA.Location = new System.Drawing.Point(329, 111);
            this.chkDNA.Name = "chkDNA";
            this.chkDNA.Size = new System.Drawing.Size(49, 17);
            this.chkDNA.TabIndex = 31;
            this.chkDNA.Text = "D&NA";
            this.chkDNA.UseVisualStyleBackColor = true;
            this.chkDNA.CheckedChanged += new System.EventHandler(this.chkDNA_CheckedChanged);
            // 
            // FrmDrugNameDetails
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(550, 326);
            this.Controls.Add(this.chkDNA);
            this.Controls.Add(this.chkBC);
            this.Controls.Add(this.chkActive);
            this.Controls.Add(this.grbHair);
            this.Controls.Add(this.grbUA);
            this.Controls.Add(this.lblDrugCodeMan);
            this.Controls.Add(this.txtDrugCode);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblCategoriesMan);
            this.Controls.Add(this.lblDrugNameMan);
            this.Controls.Add(this.lblDrugsMandatory);
            this.Controls.Add(this.txtDrugName);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.chkHair);
            this.Controls.Add(this.chkUA);
            this.Controls.Add(this.lblPageHeader);
            this.Controls.Add(this.lblCategories);
            this.Controls.Add(this.lblDrugName);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmDrugNameDetails";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Drug Name Details";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmDrugNameDetails_FormClosing);
            this.Load += new System.EventHandler(this.FrmDrugNameDetails_Load);
            this.grbUA.ResumeLayout(false);
            this.grbUA.PerformLayout();
            this.grbHair.ResumeLayout(false);
            this.grbHair.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.CheckBox chkHair;
        private System.Windows.Forms.CheckBox chkUA;
        private System.Windows.Forms.Label lblDrugName;
        private System.Windows.Forms.Label lblPageHeader;
        private Controls.TextBoxes.SurTextBox txtDrugName;
        private System.Windows.Forms.Label lblCategories;
        private System.Windows.Forms.Label lblDrugsMandatory;
        private System.Windows.Forms.Label lblDrugNameMan;
        private System.Windows.Forms.Label lblCategoriesMan;
        private System.Windows.Forms.Label lblDrugCodeMan;
        private Controls.TextBoxes.SurTextBox txtDrugCode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblUAUnitOfMeasurement;
        private System.Windows.Forms.GroupBox grbUA;
        private Controls.TextBoxes.SurTextBox txtUAConfirmationValue;
        private Controls.TextBoxes.SurTextBox txtUAScreenValue;
        private System.Windows.Forms.Label lblUAConfirmationValue;
        private System.Windows.Forms.Label lblUAScreenValue;
        private System.Windows.Forms.Label lblUAConfirmMan;
        private System.Windows.Forms.Label lblUAScreenMan;
        private System.Windows.Forms.GroupBox grbHair;
        private System.Windows.Forms.Label lblHairConfirmationMan;
        private System.Windows.Forms.Label lblHairSceenValue;
        private System.Windows.Forms.Label lblHairScreenMan;
        private System.Windows.Forms.Label lblHairConfirmationValue;
        private Controls.TextBoxes.SurTextBox txtHairConfirmationValue;
        private Controls.TextBoxes.SurTextBox txtHairScreenValue;
        private System.Windows.Forms.CheckBox chkActive;
        private System.Windows.Forms.ComboBox cmbUAUOM;
        private System.Windows.Forms.ComboBox cmbHairUOM;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblHairUnitOfMeasurement;
        private Controls.TextBoxes.SurTextBox txtUAUOM;
        private Controls.TextBoxes.SurTextBox txtHairUOM;
        private System.Windows.Forms.CheckBox chkBC;
        private System.Windows.Forms.CheckBox chkDNA;
    }
}