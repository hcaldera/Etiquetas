namespace EtiquetasArca
{
    partial class LicenseManager
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
            BtnExit = new Button();
            LblType = new Label();
            LblVersion = new Label();
            LblIssueDate = new Label();
            LblExpiryDate = new Label();
            LblMachinID = new Label();
            TxtType = new TextBox();
            TxtVersion = new TextBox();
            TxtIssuedDate = new TextBox();
            TxtExpiryDate = new TextBox();
            TxtMachineID = new TextBox();
            BtnLoadLicense = new Button();
            SuspendLayout();
            // 
            // BtnExit
            // 
            BtnExit.Location = new Point(250, 157);
            BtnExit.Name = "BtnExit";
            BtnExit.Size = new Size(75, 23);
            BtnExit.TabIndex = 0;
            BtnExit.Text = "&Salir";
            BtnExit.UseVisualStyleBackColor = true;
            BtnExit.Click += BtnExit_Click;
            // 
            // LblType
            // 
            LblType.AutoSize = true;
            LblType.Location = new Point(12, 12);
            LblType.Name = "LblType";
            LblType.Size = new Size(90, 15);
            LblType.TabIndex = 1;
            LblType.Text = "Tipo de licencia";
            // 
            // LblVersion
            // 
            LblVersion.AutoSize = true;
            LblVersion.Location = new Point(12, 41);
            LblVersion.Name = "LblVersion";
            LblVersion.Size = new Size(45, 15);
            LblVersion.TabIndex = 1;
            LblVersion.Text = "Versión";
            // 
            // LblIssueDate
            // 
            LblIssueDate.AutoSize = true;
            LblIssueDate.Location = new Point(12, 70);
            LblIssueDate.Name = "LblIssueDate";
            LblIssueDate.Size = new Size(99, 15);
            LblIssueDate.TabIndex = 1;
            LblIssueDate.Text = "Fecha de emisión";
            // 
            // LblExpiryDate
            // 
            LblExpiryDate.AutoSize = true;
            LblExpiryDate.Location = new Point(12, 99);
            LblExpiryDate.Name = "LblExpiryDate";
            LblExpiryDate.Size = new Size(111, 15);
            LblExpiryDate.TabIndex = 1;
            LblExpiryDate.Text = "Fecha de expiración";
            // 
            // LblMachinID
            // 
            LblMachinID.AutoSize = true;
            LblMachinID.Location = new Point(12, 128);
            LblMachinID.Name = "LblMachinID";
            LblMachinID.Size = new Size(84, 15);
            LblMachinID.TabIndex = 1;
            LblMachinID.Text = "ID de máquina";
            // 
            // TxtType
            // 
            TxtType.Enabled = false;
            TxtType.Location = new Point(129, 12);
            TxtType.Name = "TxtType";
            TxtType.Size = new Size(196, 23);
            TxtType.TabIndex = 2;
            // 
            // TxtVersion
            // 
            TxtVersion.Enabled = false;
            TxtVersion.Location = new Point(129, 41);
            TxtVersion.Name = "TxtVersion";
            TxtVersion.Size = new Size(196, 23);
            TxtVersion.TabIndex = 2;
            // 
            // TxtIssuedDate
            // 
            TxtIssuedDate.Enabled = false;
            TxtIssuedDate.Location = new Point(129, 70);
            TxtIssuedDate.Name = "TxtIssuedDate";
            TxtIssuedDate.Size = new Size(196, 23);
            TxtIssuedDate.TabIndex = 2;
            // 
            // TxtExpiryDate
            // 
            TxtExpiryDate.Enabled = false;
            TxtExpiryDate.Location = new Point(129, 99);
            TxtExpiryDate.Name = "TxtExpiryDate";
            TxtExpiryDate.Size = new Size(196, 23);
            TxtExpiryDate.TabIndex = 2;
            // 
            // TxtMachineID
            // 
            TxtMachineID.Enabled = false;
            TxtMachineID.Location = new Point(129, 128);
            TxtMachineID.Name = "TxtMachineID";
            TxtMachineID.Size = new Size(196, 23);
            TxtMachineID.TabIndex = 2;
            // 
            // BtnLoadLicense
            // 
            BtnLoadLicense.Location = new Point(145, 157);
            BtnLoadLicense.Name = "BtnLoadLicense";
            BtnLoadLicense.Size = new Size(99, 23);
            BtnLoadLicense.TabIndex = 3;
            BtnLoadLicense.Text = "Cargar Licencia";
            BtnLoadLicense.UseVisualStyleBackColor = true;
            BtnLoadLicense.Click += BtnLoadLicense_Click;
            // 
            // LicenseManager
            // 
            AcceptButton = BtnExit;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = BtnExit;
            ClientSize = new Size(337, 189);
            ControlBox = false;
            Controls.Add(BtnLoadLicense);
            Controls.Add(TxtMachineID);
            Controls.Add(TxtExpiryDate);
            Controls.Add(TxtIssuedDate);
            Controls.Add(TxtVersion);
            Controls.Add(TxtType);
            Controls.Add(LblMachinID);
            Controls.Add(LblExpiryDate);
            Controls.Add(LblIssueDate);
            Controls.Add(LblVersion);
            Controls.Add(LblType);
            Controls.Add(BtnExit);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "LicenseManager";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "License Manager";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button BtnExit;
        private Label LblType;
        private Label LblVersion;
        private Label LblIssueDate;
        private Label LblExpiryDate;
        private Label LblMachinID;
        private TextBox TxtType;
        private TextBox TxtVersion;
        private TextBox TxtIssuedDate;
        private TextBox TxtExpiryDate;
        private TextBox TxtMachineID;
        private Button BtnLoadLicense;
    }
}