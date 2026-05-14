namespace EtiquetasArca
{
    partial class Etiquetas
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Etiquetas));
            BtnCreateDocument = new Button();
            BtnExit = new Button();
            LblCompany = new Label();
            LblFiscalAddress = new Label();
            LblAddress = new Label();
            LblProductName = new Label();
            LblBrand = new Label();
            LblModel = new Label();
            LblSeries = new Label();
            LblSerialNumber = new Label();
            LblSpecs = new Label();
            LblSerialStart = new Label();
            LblSerialEnd = new Label();
            TxtCompany = new TextBox();
            TxtFiscalAddress = new TextBox();
            TxtAddress = new TextBox();
            CboProduct = new ComboBox();
            CboBrand = new ComboBox();
            TxtModel = new TextBox();
            TxtSeries = new TextBox();
            TxtSpecs = new TextBox();
            ToolStripMainMenu = new ToolStrip();
            BtnEdit = new ToolStripButton();
            BtnSave = new ToolStripButton();
            BtnReset = new ToolStripButton();
            BtnClear = new ToolStripButton();
            NumSerialNumberStart = new NumericUpDown();
            NumSerialNumberEnd = new NumericUpDown();
            ToolStripMainMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)NumSerialNumberStart).BeginInit();
            ((System.ComponentModel.ISupportInitialize)NumSerialNumberEnd).BeginInit();
            SuspendLayout();
            // 
            // BtnCreateDocument
            // 
            BtnCreateDocument.Location = new Point(301, 401);
            BtnCreateDocument.Name = "BtnCreateDocument";
            BtnCreateDocument.Size = new Size(111, 23);
            BtnCreateDocument.TabIndex = 0;
            BtnCreateDocument.Text = "&Crear Documento";
            BtnCreateDocument.UseVisualStyleBackColor = true;
            BtnCreateDocument.Click += BtnCreateDocument_Click;
            // 
            // BtnExit
            // 
            BtnExit.Location = new Point(220, 401);
            BtnExit.Name = "BtnExit";
            BtnExit.Size = new Size(75, 23);
            BtnExit.TabIndex = 1;
            BtnExit.Text = "&Salir";
            BtnExit.UseVisualStyleBackColor = true;
            BtnExit.Click += BtnExit_Click;
            // 
            // LblCompany
            // 
            LblCompany.AutoSize = true;
            LblCompany.Location = new Point(12, 28);
            LblCompany.Name = "LblCompany";
            LblCompany.Size = new Size(130, 30);
            LblCompany.TabIndex = 2;
            LblCompany.Text = "Empresa Importadora y\r\nReconstructora:";
            // 
            // LblFiscalAddress
            // 
            LblFiscalAddress.AutoSize = true;
            LblFiscalAddress.Location = new Point(12, 83);
            LblFiscalAddress.Name = "LblFiscalAddress";
            LblFiscalAddress.Size = new Size(93, 15);
            LblFiscalAddress.TabIndex = 2;
            LblFiscalAddress.Text = "Domicilio Fiscal:";
            // 
            // LblAddress
            // 
            LblAddress.AutoSize = true;
            LblAddress.Location = new Point(12, 145);
            LblAddress.Name = "LblAddress";
            LblAddress.Size = new Size(94, 30);
            LblAddress.TabIndex = 2;
            LblAddress.Text = "Domicilio Planta\r\nReconstructora:";
            // 
            // LblProductName
            // 
            LblProductName.AutoSize = true;
            LblProductName.Location = new Point(12, 198);
            LblProductName.Name = "LblProductName";
            LblProductName.Size = new Size(125, 15);
            LblProductName.TabIndex = 2;
            LblProductName.Text = "Nombre del Producto:";
            // 
            // LblBrand
            // 
            LblBrand.AutoSize = true;
            LblBrand.Location = new Point(12, 236);
            LblBrand.Name = "LblBrand";
            LblBrand.Size = new Size(43, 15);
            LblBrand.TabIndex = 2;
            LblBrand.Text = "Marca:";
            // 
            // LblModel
            // 
            LblModel.AutoSize = true;
            LblModel.Location = new Point(12, 265);
            LblModel.Name = "LblModel";
            LblModel.Size = new Size(51, 15);
            LblModel.TabIndex = 2;
            LblModel.Text = "Modelo:";
            // 
            // LblSeries
            // 
            LblSeries.AutoSize = true;
            LblSeries.Location = new Point(12, 294);
            LblSeries.Name = "LblSeries";
            LblSeries.Size = new Size(35, 15);
            LblSeries.TabIndex = 2;
            LblSeries.Text = "Serie:";
            // 
            // LblSerialNumber
            // 
            LblSerialNumber.AutoSize = true;
            LblSerialNumber.Location = new Point(12, 338);
            LblSerialNumber.Name = "LblSerialNumber";
            LblSerialNumber.Size = new Size(98, 15);
            LblSerialNumber.TabIndex = 2;
            LblSerialNumber.Text = "Numero de Serie:";
            // 
            // LblSpecs
            // 
            LblSpecs.AutoSize = true;
            LblSpecs.Location = new Point(12, 367);
            LblSpecs.Name = "LblSpecs";
            LblSpecs.Size = new Size(96, 15);
            LblSpecs.TabIndex = 2;
            LblSpecs.Text = "Especificaciones:";
            // 
            // LblSerialStart
            // 
            LblSerialStart.AutoSize = true;
            LblSerialStart.Location = new Point(148, 320);
            LblSerialStart.Name = "LblSerialStart";
            LblSerialStart.Size = new Size(23, 15);
            LblSerialStart.TabIndex = 2;
            LblSerialStart.Text = "del";
            // 
            // LblSerialEnd
            // 
            LblSerialEnd.AutoSize = true;
            LblSerialEnd.Location = new Point(283, 320);
            LblSerialEnd.Name = "LblSerialEnd";
            LblSerialEnd.Size = new Size(16, 15);
            LblSerialEnd.TabIndex = 2;
            LblSerialEnd.Text = "al";
            // 
            // TxtCompany
            // 
            TxtCompany.AcceptsReturn = true;
            TxtCompany.BorderStyle = BorderStyle.FixedSingle;
            TxtCompany.Enabled = false;
            TxtCompany.Location = new Point(148, 28);
            TxtCompany.Multiline = true;
            TxtCompany.Name = "TxtCompany";
            TxtCompany.Size = new Size(264, 47);
            TxtCompany.TabIndex = 3;
            // 
            // TxtFiscalAddress
            // 
            TxtFiscalAddress.AcceptsReturn = true;
            TxtFiscalAddress.BorderStyle = BorderStyle.FixedSingle;
            TxtFiscalAddress.Enabled = false;
            TxtFiscalAddress.Location = new Point(148, 83);
            TxtFiscalAddress.Multiline = true;
            TxtFiscalAddress.Name = "TxtFiscalAddress";
            TxtFiscalAddress.Size = new Size(264, 56);
            TxtFiscalAddress.TabIndex = 4;
            // 
            // TxtAddress
            // 
            TxtAddress.AcceptsReturn = true;
            TxtAddress.BorderStyle = BorderStyle.FixedSingle;
            TxtAddress.Enabled = false;
            TxtAddress.Location = new Point(148, 145);
            TxtAddress.Multiline = true;
            TxtAddress.Name = "TxtAddress";
            TxtAddress.Size = new Size(264, 56);
            TxtAddress.TabIndex = 5;
            // 
            // CboProduct
            // 
            CboProduct.FormattingEnabled = true;
            CboProduct.Items.AddRange(new object[] { "Lavadora de Ropa", "Secadora", "Refrigerador", "Congelador", "Enfriador" });
            CboProduct.Location = new Point(148, 207);
            CboProduct.Name = "CboProduct";
            CboProduct.Size = new Size(264, 23);
            CboProduct.TabIndex = 6;
            // 
            // CboBrand
            // 
            CboBrand.FormattingEnabled = true;
            CboBrand.Items.AddRange(new object[] { "ANAMA", "FRIGIDAIRE", "GENERAL ELECTRIC", "HOTPOINT", "KENMORE", "MAGIC CHEF", "MAYTAG", "WHIRLPOOL" });
            CboBrand.Location = new Point(148, 236);
            CboBrand.Name = "CboBrand";
            CboBrand.Size = new Size(264, 23);
            CboBrand.TabIndex = 7;
            // 
            // TxtModel
            // 
            TxtModel.Location = new Point(148, 265);
            TxtModel.Name = "TxtModel";
            TxtModel.Size = new Size(264, 23);
            TxtModel.TabIndex = 8;
            // 
            // TxtSeries
            // 
            TxtSeries.Location = new Point(148, 294);
            TxtSeries.Name = "TxtSeries";
            TxtSeries.Size = new Size(264, 23);
            TxtSeries.TabIndex = 9;
            // 
            // TxtSpecs
            // 
            TxtSpecs.BorderStyle = BorderStyle.FixedSingle;
            TxtSpecs.Enabled = false;
            TxtSpecs.Location = new Point(148, 367);
            TxtSpecs.Name = "TxtSpecs";
            TxtSpecs.Size = new Size(264, 23);
            TxtSpecs.TabIndex = 12;
            // 
            // ToolStripMainMenu
            // 
            ToolStripMainMenu.Items.AddRange(new ToolStripItem[] { BtnEdit, BtnSave, BtnReset, BtnClear });
            ToolStripMainMenu.Location = new Point(0, 0);
            ToolStripMainMenu.Name = "ToolStripMainMenu";
            ToolStripMainMenu.Size = new Size(424, 25);
            ToolStripMainMenu.TabIndex = 13;
            ToolStripMainMenu.Text = "toolStrip1";
            // 
            // BtnEdit
            // 
            BtnEdit.Image = Properties.Resources.Edit;
            BtnEdit.ImageTransparentColor = Color.Magenta;
            BtnEdit.Name = "BtnEdit";
            BtnEdit.Size = new Size(57, 22);
            BtnEdit.Text = "&Editar";
            BtnEdit.Click += BtnEdit_Click;
            // 
            // BtnSave
            // 
            BtnSave.Enabled = false;
            BtnSave.Image = Properties.Resources.Save;
            BtnSave.ImageTransparentColor = Color.Magenta;
            BtnSave.Name = "BtnSave";
            BtnSave.Size = new Size(69, 22);
            BtnSave.Text = "&Guardar";
            BtnSave.Click += BtnSave_Click;
            // 
            // BtnReset
            // 
            BtnReset.Alignment = ToolStripItemAlignment.Right;
            BtnReset.DisplayStyle = ToolStripItemDisplayStyle.Image;
            BtnReset.DoubleClickEnabled = true;
            BtnReset.Image = Properties.Resources.Reset;
            BtnReset.ImageTransparentColor = Color.Magenta;
            BtnReset.Name = "BtnReset";
            BtnReset.Size = new Size(23, 22);
            BtnReset.Text = "Restaurar (Doble click)";
            BtnReset.DoubleClick += BtnReset_DoubleClick;
            // 
            // BtnClear
            // 
            BtnClear.Alignment = ToolStripItemAlignment.Right;
            BtnClear.DisplayStyle = ToolStripItemDisplayStyle.Image;
            BtnClear.DoubleClickEnabled = true;
            BtnClear.Image = Properties.Resources.Clear;
            BtnClear.ImageTransparentColor = Color.Magenta;
            BtnClear.Name = "BtnClear";
            BtnClear.Size = new Size(23, 22);
            BtnClear.Text = "Limpiar (Doble click)";
            BtnClear.DoubleClick += BtnClear_DoubleClick;
            // 
            // NumSerialNumberStart
            // 
            NumSerialNumberStart.Location = new Point(148, 338);
            NumSerialNumberStart.Name = "NumSerialNumberStart";
            NumSerialNumberStart.Size = new Size(129, 23);
            NumSerialNumberStart.TabIndex = 10;
            // 
            // NumSerialNumberEnd
            // 
            NumSerialNumberEnd.Location = new Point(283, 338);
            NumSerialNumberEnd.Name = "NumSerialNumberEnd";
            NumSerialNumberEnd.Size = new Size(129, 23);
            NumSerialNumberEnd.TabIndex = 11;
            // 
            // Etiquetas
            // 
            AcceptButton = BtnCreateDocument;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = BtnExit;
            ClientSize = new Size(424, 436);
            Controls.Add(NumSerialNumberEnd);
            Controls.Add(NumSerialNumberStart);
            Controls.Add(ToolStripMainMenu);
            Controls.Add(TxtSpecs);
            Controls.Add(TxtSeries);
            Controls.Add(TxtModel);
            Controls.Add(CboBrand);
            Controls.Add(CboProduct);
            Controls.Add(TxtAddress);
            Controls.Add(TxtFiscalAddress);
            Controls.Add(TxtCompany);
            Controls.Add(LblSpecs);
            Controls.Add(LblSerialEnd);
            Controls.Add(LblSerialStart);
            Controls.Add(LblSerialNumber);
            Controls.Add(LblSeries);
            Controls.Add(LblModel);
            Controls.Add(LblBrand);
            Controls.Add(LblProductName);
            Controls.Add(LblAddress);
            Controls.Add(LblFiscalAddress);
            Controls.Add(LblCompany);
            Controls.Add(BtnExit);
            Controls.Add(BtnCreateDocument);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MaximumSize = new Size(440, 475);
            MinimumSize = new Size(440, 475);
            Name = "Etiquetas";
            StartPosition = FormStartPosition.Manual;
            Text = "Arca de la Frontera - Etiquetas";
            FormClosing += FrmEtiquetas_FormClosing;
            ToolStripMainMenu.ResumeLayout(false);
            ToolStripMainMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)NumSerialNumberStart).EndInit();
            ((System.ComponentModel.ISupportInitialize)NumSerialNumberEnd).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button BtnCreateDocument;
        private Button BtnExit;
        private Label LblCompany;
        private Label LblFiscalAddress;
        private Label LblAddress;
        private Label LblProductName;
        private Label LblBrand;
        private Label LblModel;
        private Label LblSeries;
        private Label LblSerialNumber;
        private Label LblSpecs;
        private Label LblSerialStart;
        private Label LblSerialEnd;
        private TextBox TxtCompany;
        private TextBox TxtFiscalAddress;
        private TextBox TxtAddress;
        private ComboBox CboProduct;
        private ComboBox CboBrand;
        private TextBox TxtModel;
        private TextBox TxtSeries;
        private TextBox TxtSpecs;
        private ToolStrip ToolStripMainMenu;
        private ToolStripButton BtnEdit;
        private ToolStripButton BtnSave;
        private NumericUpDown NumSerialNumberStart;
        private NumericUpDown NumSerialNumberEnd;
        private ToolStripButton BtnReset;
        private ToolStripButton BtnClear;
    }
}
