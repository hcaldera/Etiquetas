using EtiquetasArca.Properties;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace EtiquetasArca
{
    public partial class Etiquetas : Form
    {
        private sealed record LabelDocumentData(
            string CompanyLine1,
            string CompanyLine2,
            string FiscalAddressLine1,
            string FiscalAddressLine2,
            string FiscalAddressLine3,
            string AddressLine1,
            string AddressLine2,
            string AddressLine3,
            string ImpProductName,
            string Brand,
            string Model,
            string Series,
            uint SerialNumberStart,
            uint SerialNumberEnd,
            string Specs);

        private string CompanyLine1 = string.Empty;
        private string CompanyLine2 = string.Empty;
        private string FiscalAddressLine1 = string.Empty;
        private string FiscalAddressLine2 = string.Empty;
        private string FiscalAddressLine3 = string.Empty;
        private string AddressLine1 = string.Empty;
        private string AddressLine2 = string.Empty;
        private string AddressLine3 = string.Empty;
        private string ImpProductName = string.Empty;
        private string Brand = string.Empty;
        private string Model = string.Empty;
        private string Series = string.Empty;
        private uint SerialNumberStart = 0;
        private uint SerialNumberEnd = 0;
        private string Specs = string.Empty;

        private bool Editing = false;
        private bool Edited = false;
        private static bool Generating = false;
        private static bool ConfirmedClosed = false;

        private readonly LicenseManager licenseManager;

        public Etiquetas()
        {
            InitializeComponent();

            licenseManager = new LicenseManager();

            if (licenseManager.LicenseOK && licenseManager.InstallationOK)
            {
                CompanyLine1 = Properties.Settings.Default.CompanyNameLine1;
                CompanyLine2 = Properties.Settings.Default.CompanyNameLine2;
                FiscalAddressLine1 = Properties.Settings.Default.FiscalAddressLine1;
                FiscalAddressLine2 = Properties.Settings.Default.FiscalAddressLine2;
                FiscalAddressLine3 = Properties.Settings.Default.FiscalAddressLine3;
                AddressLine1 = Properties.Settings.Default.AddressLine1;
                AddressLine2 = Properties.Settings.Default.AddressLine2;
                AddressLine3 = Properties.Settings.Default.AddressLine3;
                ImpProductName = Properties.Settings.Default.ProductName;
                Brand = Properties.Settings.Default.Brand;
                Model = Properties.Settings.Default.Model;
                Series = Properties.Settings.Default.Series;
                SerialNumberStart = Properties.Settings.Default.SerialNumberStart;
                SerialNumberEnd = Properties.Settings.Default.SerialNumberEnd;
                Specs = Properties.Settings.Default.Specs;

                /* Initialize GUI */
                TxtCompany.Text = $"{CompanyLine1}\r\n{CompanyLine2}";
                TxtFiscalAddress.Text = $"{FiscalAddressLine1}\r\n{FiscalAddressLine2}\r\n{FiscalAddressLine3}";
                TxtAddress.Text = $"{AddressLine1}\r\n{AddressLine2}\r\n{AddressLine3}";
                CboProduct.Text = ImpProductName;
                CboBrand.Text = Brand;
                TxtModel.Text = Model;
                TxtSeries.Text = Series;
                NumSerialNumberStart.Text = SerialNumberStart.ToString();
                NumSerialNumberEnd.Text = SerialNumberEnd.ToString();
                TxtSpecs.Text = Specs;
                this.Location = Properties.Settings.Default.WindowLocation;
            }
            else
            {
                BtnEdit.Enabled = false;
                BtnSave.Enabled = false;
                BtnClear.Enabled = false;
                BtnReset.Enabled = false;
                BtnCreateDocument.Enabled = false;

                CboProduct.Enabled = false;
                CboBrand.Enabled = false;
                TxtModel.Enabled = false;
                TxtSeries.Enabled = false;
                NumSerialNumberStart.Enabled = false;
                NumSerialNumberEnd.Enabled = false;
                this.Text = "Aplicación sin licencia";

                if (!licenseManager.InstallationOK)
                {
                    BtnLicenseManager.Enabled = false;
                }
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            EnableEditing(true);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            EnableEditing(false);
            Edited = true;
        }

        private void BtnLicenseManager_DoubleClick(object sender, EventArgs e)
        {
            if (IsProcessElevated())
            {
                licenseManager.ShowDialog(this);
            }
            else
            {
                RelaunchAsAdmin();
            }
        }

        private void BtnClear_DoubleClick(object sender, EventArgs e)
        {
            CompanyLine1 = Properties.Settings.Default.CompanyNameLine1;
            CompanyLine2 = Properties.Settings.Default.CompanyNameLine2;
            FiscalAddressLine1 = Properties.Settings.Default.FiscalAddressLine1;
            FiscalAddressLine2 = Properties.Settings.Default.FiscalAddressLine2;
            FiscalAddressLine3 = Properties.Settings.Default.FiscalAddressLine3;
            AddressLine1 = Properties.Settings.Default.AddressLine1;
            AddressLine2 = Properties.Settings.Default.AddressLine2;
            AddressLine3 = Properties.Settings.Default.AddressLine3;
            ImpProductName = "";
            Brand = "";
            Model = "";
            Series = "";
            SerialNumberStart = Properties.Settings.Default.SerialNumberStart;
            SerialNumberEnd = Properties.Settings.Default.SerialNumberEnd;
            Specs = Properties.Settings.Default.Specs;

            /* Update GUI */
            TxtCompany.Text = $"{CompanyLine1}\r\n{CompanyLine2}";
            TxtFiscalAddress.Text = $"{FiscalAddressLine1}\r\n{FiscalAddressLine2}\r\n{FiscalAddressLine3}";
            TxtAddress.Text = $"{AddressLine1}\r\n{AddressLine2}\r\n{AddressLine3}";
            CboProduct.Text = ImpProductName;
            CboBrand.Text = Brand;
            TxtModel.Text = Model;
            TxtSeries.Text = Series;
            NumSerialNumberStart.Text = SerialNumberStart.ToString();
            NumSerialNumberEnd.Text = SerialNumberEnd.ToString();
            TxtSpecs.Text = Specs;
            Edited = true;
        }

        private void BtnReset_DoubleClick(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "¿Estás segur@ de que deseas restablecer los valores predeterminados? Esta acción no se puede deshacer.",
                "Confirmar restablecimiento",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Exclamation);

            if (result == DialogResult.Yes)
            {
                Properties.Settings.Default.Reset();
                Properties.Settings.Default.Save();

                CompanyLine1 = Properties.Settings.Default.CompanyNameLine1;
                CompanyLine2 = Properties.Settings.Default.CompanyNameLine2;
                FiscalAddressLine1 = Properties.Settings.Default.FiscalAddressLine1;
                FiscalAddressLine2 = Properties.Settings.Default.FiscalAddressLine2;
                FiscalAddressLine3 = Properties.Settings.Default.FiscalAddressLine3;
                AddressLine1 = Properties.Settings.Default.AddressLine1;
                AddressLine2 = Properties.Settings.Default.AddressLine2;
                AddressLine3 = Properties.Settings.Default.AddressLine3;
                ImpProductName = Properties.Settings.Default.ProductName;
                Brand = Properties.Settings.Default.Brand;
                Model = Properties.Settings.Default.Model;
                Series = Properties.Settings.Default.Series;
                SerialNumberStart = Properties.Settings.Default.SerialNumberStart;
                SerialNumberEnd = Properties.Settings.Default.SerialNumberEnd;
                Specs = Properties.Settings.Default.Specs;

                /* Update GUI */
                TxtCompany.Text = $"{CompanyLine1}\r\n{CompanyLine2}";
                TxtFiscalAddress.Text = $"{FiscalAddressLine1}\r\n{FiscalAddressLine2}\r\n{FiscalAddressLine3}";
                TxtAddress.Text = $"{AddressLine1}\r\n{AddressLine2}\r\n{AddressLine3}";
                CboProduct.Text = ImpProductName;
                CboBrand.Text = Brand;
                TxtModel.Text = Model;
                TxtSeries.Text = Series;
                NumSerialNumberStart.Text = SerialNumberStart.ToString();
                NumSerialNumberEnd.Text = SerialNumberEnd.ToString();
                TxtSpecs.Text = Specs;
                Edited = true;
            }
        }

        private void BtnCreateDocument_Click(object sender, EventArgs e)
        {
            CompanyLine1 = TxtCompany.Lines.Length >= 1 ? TxtCompany.Lines[0] : string.Empty;
            CompanyLine2 = TxtCompany.Lines.Length >= 2 ? TxtCompany.Lines[1] : string.Empty;
            FiscalAddressLine1 = TxtFiscalAddress.Lines.Length >= 1 ? TxtFiscalAddress.Lines[0] : string.Empty;
            FiscalAddressLine2 = TxtFiscalAddress.Lines.Length >= 2 ? TxtFiscalAddress.Lines[1] : string.Empty;
            FiscalAddressLine3 = TxtFiscalAddress.Lines.Length == 3 ? TxtFiscalAddress.Lines[2] : string.Empty;
            AddressLine1 = TxtAddress.Lines.Length >= 1 ? TxtAddress.Lines[0] : string.Empty;
            AddressLine2 = TxtAddress.Lines.Length >= 2 ? TxtAddress.Lines[1] : string.Empty;
            AddressLine3 = TxtAddress.Lines.Length == 3 ? TxtAddress.Lines[2] : string.Empty;
            ImpProductName = CboProduct.Text;
            Brand = CboBrand.Text;
            Model = TxtModel.Text;
            Series = TxtSeries.Text;
            SerialNumberStart = string.IsNullOrWhiteSpace(NumSerialNumberStart.Text) ? 0U : uint.Parse(NumSerialNumberStart.Text);
            SerialNumberEnd = string.IsNullOrWhiteSpace(NumSerialNumberEnd.Text) ? 0U : uint.Parse(NumSerialNumberEnd.Text);
            Specs = TxtSpecs.Text;

            if (ValidateFields())
            {
                using SaveFileDialog saveFileDialog = new()
                {
                    Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*",
                    Title = "Guardar documento"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog.FileName;
                    GenerateDocumentInBackground(filePath);
                }
            }
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            if (ConfirmExit())
            {
                ConfirmedClosed = true;
                Application.Exit();
            }
        }

        private void FrmEtiquetas_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Generating || (!ConfirmedClosed && !ConfirmExit()))
            {
                e.Cancel = true;
            }
        }

        private bool ConfirmExit()
        {
            string _companyLine1 = Properties.Settings.Default.CompanyNameLine1;
            string _companyLine2 = Properties.Settings.Default.CompanyNameLine2;
            string _fiscalAddressLine1 = Properties.Settings.Default.FiscalAddressLine1;
            string _fiscalAddressLine2 = Properties.Settings.Default.FiscalAddressLine2;
            string _fiscalAddressLine3 = Properties.Settings.Default.FiscalAddressLine3;
            string _addressLine1 = Properties.Settings.Default.AddressLine1;
            string _addressLine2 = Properties.Settings.Default.AddressLine2;
            string _addressLine3 = Properties.Settings.Default.AddressLine3;
            string _productName = Properties.Settings.Default.ProductName;
            string _brand = Properties.Settings.Default.Brand;
            string _model = Properties.Settings.Default.Model;
            string _series = Properties.Settings.Default.Series;
            string _specs = Properties.Settings.Default.Specs;

            DialogResult result;

            if (Editing)
            {
                result = MessageBox.Show(
                    "Hay cambios sin guardar. ¿Deseas salir de todos modos?",
                    "Confirmar salida",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Exclamation
                    );
            }
            else
            {
                result = DialogResult.Yes;
                if (licenseManager.LicenseOK && licenseManager.InstallationOK)
                {
                    result = MessageBox.Show(
                        "¿Segur@ que deseas salir?",
                        "Confirmar salida",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );

                    if (Edited && (result == DialogResult.Yes))
                    {
                        Properties.Settings.Default.CompanyNameLine1 = CompanyLine1 ?? _companyLine1;
                        Properties.Settings.Default.CompanyNameLine2 = CompanyLine2 ?? _companyLine2;
                        Properties.Settings.Default.FiscalAddressLine1 = FiscalAddressLine1 ?? _fiscalAddressLine1;
                        Properties.Settings.Default.FiscalAddressLine2 = FiscalAddressLine2 ?? _fiscalAddressLine2;
                        Properties.Settings.Default.FiscalAddressLine3 = FiscalAddressLine3 ?? _fiscalAddressLine3;
                        Properties.Settings.Default.AddressLine1 = AddressLine1 ?? _addressLine1;
                        Properties.Settings.Default.AddressLine2 = AddressLine2 ?? _addressLine2;
                        Properties.Settings.Default.AddressLine3 = AddressLine3 ?? _addressLine3;
                        Properties.Settings.Default.ProductName = ImpProductName ?? _productName;
                        Properties.Settings.Default.Brand = Brand ?? _brand;
                        Properties.Settings.Default.Model = Model ?? _model;
                        Properties.Settings.Default.Series = Series ?? _series;
                        Properties.Settings.Default.SerialNumberStart = SerialNumberStart;
                        Properties.Settings.Default.SerialNumberEnd = SerialNumberEnd;
                        Properties.Settings.Default.Specs = Specs ?? _specs;
                        Properties.Settings.Default.WindowLocation = this.Location;
                        Properties.Settings.Default.Save();
                    }
                }
            }

            return result == DialogResult.Yes;
        }

        private bool ValidateFields()
        {
            bool result = false;

            if (string.IsNullOrWhiteSpace(CompanyLine1) ||
                string.IsNullOrWhiteSpace(CompanyLine2) ||
                string.IsNullOrWhiteSpace(FiscalAddressLine1) ||
                string.IsNullOrWhiteSpace(FiscalAddressLine2) ||
                string.IsNullOrWhiteSpace(FiscalAddressLine3) ||
                string.IsNullOrWhiteSpace(AddressLine1) ||
                string.IsNullOrWhiteSpace(AddressLine2) ||
                string.IsNullOrWhiteSpace(AddressLine3) ||
                string.IsNullOrWhiteSpace(ImpProductName) ||
                string.IsNullOrWhiteSpace(Brand) ||
                string.IsNullOrWhiteSpace(Model) ||
                string.IsNullOrWhiteSpace(Series) ||
                (SerialNumberStart == 0) ||
                (SerialNumberEnd == 0) ||
                (SerialNumberEnd < SerialNumberStart) ||
                string.IsNullOrWhiteSpace(Specs))
            {
                MessageBox.Show(
                    "Por favor, completa todos los campos correctamente.",
                    "Campos incompletos",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                    );

                if (string.IsNullOrWhiteSpace(CompanyLine1))
                {
                    EnableEditing(true);
                    CompanyLine1 = Properties.Settings.Default.CompanyNameLine1;
                    TxtCompany.Focus();
                }
                else if (string.IsNullOrWhiteSpace(CompanyLine2))
                {
                    EnableEditing(true);
                    CompanyLine2 = Properties.Settings.Default.CompanyNameLine2;
                    TxtCompany.Focus();
                }
                else if (string.IsNullOrWhiteSpace(FiscalAddressLine1))
                {
                    EnableEditing(true);
                    FiscalAddressLine1 = Properties.Settings.Default.FiscalAddressLine1;
                    TxtFiscalAddress.Focus();
                }
                else if (string.IsNullOrWhiteSpace(FiscalAddressLine2))
                {
                    EnableEditing(true);
                    FiscalAddressLine2 = Properties.Settings.Default.FiscalAddressLine2;
                    TxtFiscalAddress.Focus();
                }
                else if (string.IsNullOrWhiteSpace(FiscalAddressLine3))
                {
                    EnableEditing(true);
                    FiscalAddressLine3 = Properties.Settings.Default.FiscalAddressLine3;
                    TxtFiscalAddress.Focus();
                }
                else if (string.IsNullOrWhiteSpace(AddressLine1))
                {
                    EnableEditing(true);
                    AddressLine1 = Properties.Settings.Default.AddressLine1;
                    TxtAddress.Focus();
                }
                else if (string.IsNullOrWhiteSpace(AddressLine2))
                {
                    EnableEditing(true);
                    AddressLine2 = Properties.Settings.Default.AddressLine2;
                    TxtAddress.Focus();
                }
                else if (string.IsNullOrWhiteSpace(AddressLine3))
                {
                    EnableEditing(true);
                    AddressLine3 = Properties.Settings.Default.AddressLine3;
                    TxtAddress.Focus();
                }
                else if (string.IsNullOrWhiteSpace(ImpProductName))
                {
                    ImpProductName = Properties.Settings.Default.ProductName;
                    CboProduct.Focus();
                }
                else if (string.IsNullOrWhiteSpace(Brand))
                {
                    Brand = Properties.Settings.Default.Brand;
                    CboBrand.Focus();
                }
                else if (string.IsNullOrWhiteSpace(Model))
                {
                    Model = Properties.Settings.Default.Model;
                    TxtModel.Focus();
                }
                else if (string.IsNullOrWhiteSpace(Series))
                {
                    Series = Properties.Settings.Default.Series;
                    TxtSeries.Focus();
                }
                else if (SerialNumberEnd == 0)
                {
                    SerialNumberStart = Properties.Settings.Default.SerialNumberStart;
                    SerialNumberEnd = Properties.Settings.Default.SerialNumberEnd;
                    NumSerialNumberEnd.Focus();
                }
                else if ((SerialNumberStart == 0) || (SerialNumberEnd < SerialNumberStart))
                {
                    SerialNumberStart = Properties.Settings.Default.SerialNumberStart;
                    SerialNumberEnd = Properties.Settings.Default.SerialNumberEnd;
                    NumSerialNumberStart.Focus();
                }
                else if (string.IsNullOrWhiteSpace(Specs))
                {
                    EnableEditing(true);
                    Specs = Properties.Settings.Default.Specs;
                    TxtSpecs.Focus();
                }
            }
            else
            {
                result = true;
            }

            return result;
        }

        private void EnableEditing(bool enable)
        {
            BtnEdit.Enabled = !enable;
            BtnSave.Enabled = enable;
            TxtCompany.Enabled = enable;
            TxtCompany.BorderStyle = enable ? BorderStyle.Fixed3D : BorderStyle.FixedSingle;
            TxtFiscalAddress.Enabled = enable;
            TxtFiscalAddress.BorderStyle = enable ? BorderStyle.Fixed3D : BorderStyle.FixedSingle;
            TxtAddress.Enabled = enable;
            TxtAddress.BorderStyle = enable ? BorderStyle.Fixed3D : BorderStyle.FixedSingle;
            TxtSpecs.Enabled = enable;
            TxtSpecs.BorderStyle = enable ? BorderStyle.Fixed3D : BorderStyle.FixedSingle;
            BtnCreateDocument.Enabled = !enable;
            Editing = enable;
        }

        private void GenerateDocumentInBackground(string filePath)
        {
            LabelDocumentData documentData = new(
                CompanyLine1,
                CompanyLine2,
                FiscalAddressLine1,
                FiscalAddressLine2,
                FiscalAddressLine3,
                AddressLine1,
                AddressLine2,
                AddressLine3,
                ImpProductName,
                Brand,
                Model,
                Series,
                SerialNumberStart,
                SerialNumberEnd,
                Specs);

            BtnCreateDocument.Enabled = false;
            BtnExit.Enabled = false;
            BtnEdit.Enabled = false;
            BtnLicenseManager.Enabled = false;
            BtnClear.Enabled = false;
            BtnReset.Enabled = false;
            UseWaitCursor = true;
            Generating = true;
            PrepareDocumentProgress(GetPaddedTotalLabels(documentData));
            /* Ensure the numeric up-down control reflects the current end serial number during generation */
            SerialNumberEnd = documentData.SerialNumberStart + (uint)GetPaddedTotalLabels(documentData) - 1U;
            NumSerialNumberEnd.Value = SerialNumberEnd;

            Thread documentThread = new(() =>
            {
                try
                {
                    GenerateDocument(filePath, documentData, ReportDocumentProgress);

                    RunOnUiThread(() =>
                    {
                        LblProgressStatus.Text = "Documento generado.";
                        ProgressDocument.Value = ProgressDocument.Maximum;
                        Edited = true;
                        OpenPdf(filePath);
                    });
                }
                catch (Exception ex)
                {
                    RunOnUiThread(() =>
                    {
                        MessageBox.Show($"Error al generar el documento PDF: {ex.Message}", "Error al generar PDF", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    });
                }
                finally
                {
                    RunOnUiThread(() =>
                    {
                        BtnCreateDocument.Enabled = true;
                        BtnExit.Enabled = true;
                        BtnEdit.Enabled = true;
                        BtnLicenseManager.Enabled = true;
                        BtnClear.Enabled = true;
                        BtnReset.Enabled = true;
                        StatusStripMain.Visible = false;
                        LblProgressStatus.Text = string.Empty;
                        LblProgressStatus.Visible = false;
                        ProgressDocument.Visible = false;
                        ProgressDocument.Value = 0;
                        UseWaitCursor = false;
                        Generating = false;
                    });
                }
            })
            {
                IsBackground = true
            };
            documentThread.Start();
        }

        private void PrepareDocumentProgress(int totalLabels)
        {
            StatusStripMain.Visible = true;
            LblProgressStatus.Text = $"Generando etiquetas: 0/{totalLabels}";
            LblProgressStatus.Visible = true;
            ProgressDocument.Minimum = 0;
            ProgressDocument.Maximum = Math.Max(totalLabels, 1);
            ProgressDocument.Value = 0;
            ProgressDocument.Visible = true;
        }

        private void ReportDocumentProgress(int labelsGenerated)
        {
            RunOnUiThread(() =>
            {
                if (labelsGenerated >= ProgressDocument.Maximum)
                {
                    LblProgressStatus.Text = "Finalizando documento...";
                }
                else
                {
                    LblProgressStatus.Text = $"Generando etiquetas: {labelsGenerated}/{ProgressDocument.Maximum}";
                }
                
                ProgressDocument.Value = Math.Min(labelsGenerated, ProgressDocument.Maximum);
            });
        }

        private void RunOnUiThread(Action action)
        {
            if (IsDisposed || !IsHandleCreated)
            {
                return;
            }

            try
            {
                BeginInvoke(action);
            }
            catch (InvalidOperationException)
            {
            }
        }

        private static void GenerateDocument(string filePath, LabelDocumentData documentData, Action<int> reportProgress)
        {
            int totalLabelsToGenerate = GetPaddedTotalLabels(documentData);

            var serialNumbers = Enumerable
                .Range((int)documentData.SerialNumberStart, totalLabelsToGenerate)
                .ToList();

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.Letter);
                    page.Margin(0.5F, Unit.Inch);
                    page.DefaultTextStyle(x => x
                        .FontFamily("Times New Roman")
                        .FontSize(8));

                    page.Content().Column(mainColumn =>
                    {
                        int totalLabels = serialNumbers.Count;
                        int currentIndex = 0;
                        bool isFirstPage = true;

                        mainColumn.Spacing(5);

                        while (currentIndex < totalLabels)
                        {
                            /* Add a page break before every page except the first one */
                            if (!isFirstPage)
                            {
                                mainColumn.Item().PageBreak();
                            }

                            isFirstPage = false;

                            /* Each page: 3 rows x 2 cols = 6 labels */
                            for (int row = 0; row < 3; row++)
                            {
                                mainColumn.Item().Row(rowContainter =>
                                {
                                    rowContainter.Spacing(10);

                                    for (int col = 0; col < 2; col++)
                                    {
                                        if (currentIndex < totalLabels)
                                        {
                                            int serial = serialNumbers[currentIndex++];

                                            rowContainter.RelativeItem().Border(0.5F).Height(233).Padding(3).Element(label => ComposeLabel(label, serial, documentData));
                                            reportProgress(currentIndex);
                                        }
                                    }
                                });
                            }
                        }
                    });
                });
            })
            .GeneratePdf(filePath);
        }

        private static int GetPaddedTotalLabels(LabelDocumentData documentData)
        {
            uint totalLabels = documentData.SerialNumberEnd - documentData.SerialNumberStart + 1;

            if ((totalLabels % 6) != 0)
            {
                totalLabels += (6 - (totalLabels % 6));
            }

            return (int)totalLabels;
        }

        private static void ComposeLabel(IContainer container, int serial, LabelDocumentData documentData)
        {
            float _padding1 = 73;
            float _padding2 = 105.2F;
            float _row1Length = 4;
            float _row2Length = 5;

            container.Layers(layers =>
            {
                /* Background layer */
                layers.Layer().AlignCenter().AlignBottom().Height(205).Image(Resources.Logo).FitArea();

                /* Foreground layer */
                layers.PrimaryLayer().Padding(5).Column(col =>
                {
                    col.Spacing(2);

                    col.Item().Text("PRODUCTO USADO").FontSize(18).AlignCenter();

                    col.Item().Text("Fabricado en Estados Unidos de América").AlignCenter();

                    col.Item().Text("Importador y Reconstructor:");
                    col.Item().PaddingLeft(_padding1).Column(importer =>
                    {
                        importer.Item().Text(documentData.CompanyLine1).Bold();
                        importer.Item().Text(documentData.CompanyLine2).Bold();
                    });

                    col.Item().Text("");
                    col.Item().Row(r =>
                    {
                        r.RelativeItem(_row1Length).Text("Domicilio Fiscal:");
                        r.RelativeItem(_row2Length).Column(addr =>
                        {
                            addr.Item().Text(documentData.FiscalAddressLine1);
                            addr.Item().Text(documentData.FiscalAddressLine2);
                            addr.Item().Text(documentData.FiscalAddressLine3);
                        });
                    });

                    col.Item().Text("Domicilio Planta Reconstructora:");

                    col.Item().PaddingLeft(_padding2).Column(addr =>
                    {
                        addr.Item().Text(documentData.AddressLine1);
                        addr.Item().Text(documentData.AddressLine2);
                        addr.Item().Text(documentData.AddressLine3);
                    });

                    col.Item().Row(r =>
                    {
                        r.RelativeItem(_row1Length).Text("Nombre del producto:").Bold();
                        r.RelativeItem(_row2Length).Text(documentData.ImpProductName).Bold();
                    });

                    col.Item().Row(r =>
                    {
                        r.RelativeItem(_row1Length).Text("Marca:").Bold();
                        r.RelativeItem(_row2Length).Text(documentData.Brand).Bold();
                    });

                    col.Item().Row(r =>
                    {
                        r.RelativeItem(_row1Length).Text("Modelo:").Bold();
                        r.RelativeItem(_row2Length).Text(documentData.Model).Bold();
                    });

                    col.Item().Row(r =>
                    {
                        r.RelativeItem(_row1Length).Text("Serie:").Bold();
                        r.RelativeItem(_row2Length).Text(documentData.Series).Bold();
                    });

                    col.Item().Row(r =>
                    {
                        r.RelativeItem(_row1Length).Text("Especificaciones:");
                        r.RelativeItem(_row2Length).Text(documentData.Specs);
                    });

                    col.Item().Row(r =>
                    {
                        r.ConstantItem(140).Text("Una pieza").AlignRight();
                        r.ConstantItem(100).Text($"# {serial:D4}").AlignRight();
                    });
                });
            });
        }

        private static void OpenPdf(string path)
        {
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                {
                    FileName = path,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo abrir el archivo PDF: {ex.Message}", "Error al abrir PDF", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static bool IsProcessElevated()
        {
            bool isElevated;

            try
            {
                using var identity = System.Security.Principal.WindowsIdentity.GetCurrent();
                var principal = new System.Security.Principal.WindowsPrincipal(identity);
                isElevated = principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
            }
            catch
            {
                isElevated = false;
            }

            return isElevated;
        }

        public static void RelaunchAsAdmin()
        {
            try
            {
                DialogResult result = MessageBox.Show(
                    "Para acceder al administrador de licencias, la aplicación debe ejecutarse con privilegios de administrador. ¿Deseas reiniciar la aplicación como administrador?",
                    "Reiniciar como administrador",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                    );

                if (result == DialogResult.Yes)
                {
                    var processInfo = new System.Diagnostics.ProcessStartInfo
                    {
                        UseShellExecute = true,
                        FileName = Application.ExecutablePath,
                        Verb = "runas"
                    };

                    System.Diagnostics.Process.Start(processInfo);
                    ConfirmedClosed = true;
                    Application.Exit(); /* Ensure the current instance exits after launching the new one with elevated privileges. */
                }
            }
            catch (System.ComponentModel.Win32Exception ex) when (ex.NativeErrorCode == 1223)
            {
                MessageBox.Show(
                    "No se pudo iniciar el programa con privilegios de administrador porque se canceló la solicitud.",
                    "Operación Cancelada",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"No se pudo iniciar el programa con privilegios de administrador.\r\n" +
                    $"Por favor, inténtalo de nuevo y acepta la solicitud.\r\n" +
                    $"Detalles del error: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
