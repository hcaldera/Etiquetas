using EtiquetasArca.Properties;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace EtiquetasArca
{
    public partial class Etiquetas : Form
    {
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
        private bool ConfirmedClosed = false;

        public Etiquetas()
        {
            InitializeComponent();

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

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            EnableEditing(true);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            EnableEditing(false);
            Edited = true;
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
                    GenerateDocument(filePath);
                    OpenPdf(filePath);
                    Edited = true;
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
            if (!ConfirmedClosed && !ConfirmExit())
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

        private void GenerateDocument(string filePath)
        {
            uint _totalLabels = SerialNumberEnd - SerialNumberStart + 1;

            if ((_totalLabels % 6) != 0)
            {
                _totalLabels += (6 - (_totalLabels % 6));
            }

            var serialNumbers = Enumerable
                .Range((int)SerialNumberStart, (int)_totalLabels)
                .ToList();

            try
            {
                Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.Letter);
                        page.Margin(0.5F, Unit.Inch);
                        page.DefaultTextStyle(x => x
                            .FontFamily("Times New Roman")
                            .FontSize(8));

                        page.Content().Element(content =>
                        {
                            int totalLabels = serialNumbers.Count;
                            int currentIndex = 0;

                            while (currentIndex < totalLabels)
                            {
                                content.Column(column =>
                                {
                                    column.Spacing(10);

                                    for (int row = 0; row < 3; row++)
                                    {
                                        column.Item().Row(rowContainer =>
                                        {
                                            rowContainer.Spacing(10);

                                            for (int col = 0; col < 2; col++)
                                            {
                                                if (currentIndex < totalLabels)
                                                {
                                                    int serial = serialNumbers[currentIndex++];

                                                    rowContainer.RelativeItem().Border(1).Height(233).Padding(3).Element(label =>
                                                    {
                                                        ComposeLabel(label, serial);
                                                    });
                                                }
                                                else
                                                {
                                                    rowContainer.RelativeItem();
                                                }
                                            }
                                        });
                                    }
                                });

                                if (currentIndex < totalLabels)
                                {
                                    content.PageBreak();
                                }
                            }
                        });
                    });
                })
                .GeneratePdf(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar el documento PDF: {ex.Message}", "Error al generar PDF", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ComposeLabel(IContainer container, int serial)
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
                        importer.Item().Text(CompanyLine1).Bold();
                        importer.Item().Text(CompanyLine2).Bold();
                    });

                    col.Item().Text("");
                    col.Item().Row(r =>
                    {
                        r.RelativeItem(_row1Length).Text("Domicilio Fiscal:");
                        r.RelativeItem(_row2Length).Column(addr =>
                        {
                            addr.Item().Text(FiscalAddressLine1);
                            addr.Item().Text(FiscalAddressLine2);
                            addr.Item().Text(FiscalAddressLine3);
                        });
                    });

                    col.Item().Text("Domicilio Planta Reconstructora:");

                    col.Item().PaddingLeft(_padding2).Column(addr =>
                    {
                        addr.Item().Text(AddressLine1);
                        addr.Item().Text(AddressLine2);
                        addr.Item().Text(AddressLine3);
                    });

                    col.Item().Row(r =>
                    {
                        r.RelativeItem(_row1Length).Text("Nombre del producto:").Bold();
                        r.RelativeItem(_row2Length).Text(ImpProductName).Bold();
                    });

                    col.Item().Row(r =>
                    {
                        r.RelativeItem(_row1Length).Text("Marca:").Bold();
                        r.RelativeItem(_row2Length).Text(Brand).Bold();
                    });

                    col.Item().Row(r =>
                    {
                        r.RelativeItem(_row1Length).Text("Modelo:").Bold();
                        r.RelativeItem(_row2Length).Text(Model).Bold();
                    });

                    col.Item().Row(r =>
                    {
                        r.RelativeItem(_row1Length).Text("Serie:").Bold();
                        r.RelativeItem(_row2Length).Text(Series).Bold();
                    });

                    col.Item().Row(r =>
                    {
                        r.RelativeItem(_row1Length).Text("Especificaciones:");
                        r.RelativeItem(_row2Length).Text(Specs);
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
    }
}
