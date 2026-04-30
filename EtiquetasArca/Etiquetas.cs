using EtiquetasArca.Properties;
using Microsoft.VisualBasic;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace EtiquetasArca
{
    public partial class FrmStickers : Form
    {
        private static string? companyLine1;
        private static string? companyLine2;
        private static string? fiscalAddressLine1;
        private static string? fiscalAddressLine2;
        private static string? fiscalAddressLine3;
        private static string? addressLine1;
        private static string? addressLine2;
        private static string? addressLine3;
        private static string? productName;
        private static string? brand;
        private static string? model;
        private static string? series;
        private static uint serialNumberStart;
        private static uint serialNumberEnd;
        private static string? specs;

        private static bool editing = false;
        private static bool edited = false;
        private static bool closed = false;

        public FrmStickers()
        {
            InitializeComponent();

            companyLine1 = Properties.Settings.Default.CompanyNameLine1;
            companyLine2 = Properties.Settings.Default.CompanyNameLine2;
            fiscalAddressLine1 = Properties.Settings.Default.FiscalAddressLine1;
            fiscalAddressLine2 = Properties.Settings.Default.FiscalAddressLine2;
            fiscalAddressLine3 = Properties.Settings.Default.FiscalAddressLine3;
            addressLine1 = Properties.Settings.Default.AddressLine1;
            addressLine2 = Properties.Settings.Default.AddressLine2;
            addressLine3 = Properties.Settings.Default.AddressLine3;
            productName = Properties.Settings.Default.ProductName;
            brand = Properties.Settings.Default.Brand;
            model = Properties.Settings.Default.Model;
            series = Properties.Settings.Default.Series;
            serialNumberStart = Properties.Settings.Default.SerialNumberStart;
            serialNumberEnd = Properties.Settings.Default.SerialNumberEnd;
            specs = Properties.Settings.Default.Specs;
            /* Initialize GUI */
            TxtCompany.Text = $"{companyLine1}\r\n{companyLine2}";
            TxtFiscalAddress.Text = $"{fiscalAddressLine1}\r\n{fiscalAddressLine2}\r\n{fiscalAddressLine3}";
            TxtAddress.Text = $"{addressLine1}\r\n{addressLine2}\r\n{addressLine3}";
            CboProduct.Text = productName;
            CboBrand.Text = brand;
            TxtModel.Text = model;
            TxtSeries.Text = series;
            NumSerialNumberStart.Text = serialNumberStart.ToString();
            NumSerialNumberEnd.Text = serialNumberEnd.ToString();
            TxtSpecs.Text = specs;
            this.Location = Properties.Settings.Default.WindowLocation;
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            EnableEditing(true);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            EnableEditing(false);
            edited = true;
        }

        private void BtnClear_DoubleClick(object sender, EventArgs e)
        {
            companyLine1 = Properties.Settings.Default.CompanyNameLine1;
            companyLine2 = Properties.Settings.Default.CompanyNameLine2;
            fiscalAddressLine1 = Properties.Settings.Default.FiscalAddressLine1;
            fiscalAddressLine2 = Properties.Settings.Default.FiscalAddressLine2;
            fiscalAddressLine3 = Properties.Settings.Default.FiscalAddressLine3;
            addressLine1 = Properties.Settings.Default.AddressLine1;
            addressLine2 = Properties.Settings.Default.AddressLine2;
            addressLine3 = Properties.Settings.Default.AddressLine3;
            productName = "";
            brand = "";
            model = "";
            series = "";
            serialNumberStart = Properties.Settings.Default.SerialNumberStart;
            serialNumberEnd = Properties.Settings.Default.SerialNumberEnd;
            specs = Properties.Settings.Default.Specs;
            /* Update GUI */
            TxtCompany.Text = $"{companyLine1}\r\n{companyLine2}";
            TxtFiscalAddress.Text = $"{fiscalAddressLine1}\r\n{fiscalAddressLine2}\r\n{fiscalAddressLine3}";
            TxtAddress.Text = $"{addressLine1}\r\n{addressLine2}\r\n{addressLine3}";
            CboProduct.Text = productName;
            CboBrand.Text = brand;
            TxtModel.Text = model;
            TxtSeries.Text = series;
            NumSerialNumberStart.Text = serialNumberStart.ToString();
            NumSerialNumberEnd.Text = serialNumberEnd.ToString();
            TxtSpecs.Text = specs;
            edited = true;
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

                companyLine1 = Properties.Settings.Default.CompanyNameLine1;
                companyLine2 = Properties.Settings.Default.CompanyNameLine2;
                fiscalAddressLine1 = Properties.Settings.Default.FiscalAddressLine1;
                fiscalAddressLine2 = Properties.Settings.Default.FiscalAddressLine2;
                fiscalAddressLine3 = Properties.Settings.Default.FiscalAddressLine3;
                addressLine1 = Properties.Settings.Default.AddressLine1;
                addressLine2 = Properties.Settings.Default.AddressLine2;
                addressLine3 = Properties.Settings.Default.AddressLine3;
                productName = Properties.Settings.Default.ProductName;
                brand = Properties.Settings.Default.Brand;
                model = Properties.Settings.Default.Model;
                series = Properties.Settings.Default.Series;
                serialNumberStart = Properties.Settings.Default.SerialNumberStart;
                serialNumberEnd = Properties.Settings.Default.SerialNumberEnd;
                specs = Properties.Settings.Default.Specs;
                /* Update GUI */
                TxtCompany.Text = $"{companyLine1}\r\n{companyLine2}";
                TxtFiscalAddress.Text = $"{fiscalAddressLine1}\r\n{fiscalAddressLine2}\r\n{fiscalAddressLine3}";
                TxtAddress.Text = $"{addressLine1}\r\n{addressLine2}\r\n{addressLine3}";
                CboProduct.Text = productName;
                CboBrand.Text = brand;
                TxtModel.Text = model;
                TxtSeries.Text = series;
                NumSerialNumberStart.Text = serialNumberStart.ToString();
                NumSerialNumberEnd.Text = serialNumberEnd.ToString();
                TxtSpecs.Text = specs;
                edited = true;
            }
        }

        private void BtnCreateDocument_Click(object sender, EventArgs e)
        {
            companyLine1 = TxtCompany.Lines.Length >= 1 ? TxtCompany.Lines[0] : "";
            companyLine2 = TxtCompany.Lines.Length >= 2 ? TxtCompany.Lines[1] : "";
            fiscalAddressLine1 = TxtFiscalAddress.Lines.Length >= 1 ? TxtFiscalAddress.Lines[0] : "";
            fiscalAddressLine2 = TxtFiscalAddress.Lines.Length >= 2 ? TxtFiscalAddress.Lines[1] : "";
            fiscalAddressLine3 = TxtFiscalAddress.Lines.Length == 3 ? TxtFiscalAddress.Lines[2] : "";
            addressLine1 = TxtAddress.Lines.Length >= 1 ? TxtAddress.Lines[0] : "";
            addressLine2 = TxtAddress.Lines.Length >= 2 ? TxtAddress.Lines[1] : "";
            addressLine3 = TxtAddress.Lines.Length == 3 ? TxtAddress.Lines[2] : "";
            productName = CboProduct.Text;
            brand = CboBrand.Text;
            model = TxtModel.Text;
            series = TxtSeries.Text;
            serialNumberStart = NumSerialNumberStart.Text == "" ? 0U : uint.Parse(NumSerialNumberStart.Text);
            serialNumberEnd = NumSerialNumberEnd.Text == "" ? 0U : uint.Parse(NumSerialNumberEnd.Text);
            specs = TxtSpecs.Text;

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
                    edited = true;
                }
            }
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            if (ConfirmExit())
            {
                closed = true;
                Application.Exit();
            }
        }

        private void FrmEtiquetas_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!closed && !ConfirmExit())
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
            uint _serialNumberStart = Properties.Settings.Default.SerialNumberStart;
            uint _serialNumberEnd = Properties.Settings.Default.SerialNumberEnd;
            string _specs = Properties.Settings.Default.Specs;

            DialogResult result;

            if (editing)
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
                
                if (edited && (result == DialogResult.Yes))
                {
                    Properties.Settings.Default.CompanyNameLine1 = companyLine1 ?? _companyLine1;
                    Properties.Settings.Default.CompanyNameLine2 = companyLine2 ?? _companyLine2;
                    Properties.Settings.Default.FiscalAddressLine1 = fiscalAddressLine1 ?? _fiscalAddressLine1;
                    Properties.Settings.Default.FiscalAddressLine2 = fiscalAddressLine2 ?? _fiscalAddressLine2;
                    Properties.Settings.Default.FiscalAddressLine3 = fiscalAddressLine3 ?? _fiscalAddressLine3;
                    Properties.Settings.Default.AddressLine1 = addressLine1 ?? _addressLine1;
                    Properties.Settings.Default.AddressLine2 = addressLine2 ?? _addressLine2;
                    Properties.Settings.Default.AddressLine3 = addressLine3 ?? _addressLine3;
                    Properties.Settings.Default.ProductName = productName ?? _productName;
                    Properties.Settings.Default.Brand = brand ?? _brand;
                    Properties.Settings.Default.Model = model ?? _model;
                    Properties.Settings.Default.Series = series ?? _series;
                    Properties.Settings.Default.SerialNumberStart = serialNumberStart;
                    Properties.Settings.Default.SerialNumberEnd = serialNumberEnd;
                    Properties.Settings.Default.Specs = specs ?? _specs;
                    Properties.Settings.Default.WindowLocation = this.Location;
                    Properties.Settings.Default.Save();
                }
            }

            return result == DialogResult.Yes;
        }

        private bool ValidateFields()
        {
            bool result = false;

            if (string.IsNullOrWhiteSpace(companyLine1) ||
                string.IsNullOrWhiteSpace(companyLine2) ||
                string.IsNullOrWhiteSpace(fiscalAddressLine1) ||
                string.IsNullOrWhiteSpace(fiscalAddressLine2) ||
                string.IsNullOrWhiteSpace(fiscalAddressLine3) ||
                string.IsNullOrWhiteSpace(addressLine1) ||
                string.IsNullOrWhiteSpace(addressLine2) ||
                string.IsNullOrWhiteSpace(addressLine3) ||
                string.IsNullOrWhiteSpace(productName) ||
                string.IsNullOrWhiteSpace(brand) ||
                string.IsNullOrWhiteSpace(model) ||
                string.IsNullOrWhiteSpace(series) ||
                (serialNumberStart == 0) ||
                (serialNumberEnd == 0) ||
                (serialNumberEnd < serialNumberStart) ||
                string.IsNullOrWhiteSpace(specs))
            {
                MessageBox.Show(
                    "Por favor, completa todos los campos correctamente.",
                    "Campos incompletos",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                    );

                if (string.IsNullOrWhiteSpace(companyLine1))
                {
                    EnableEditing(true);
                    companyLine1 = Properties.Settings.Default.CompanyNameLine1;
                    TxtCompany.Focus();
                }
                else if (string.IsNullOrWhiteSpace(companyLine2))
                {
                    EnableEditing(true);
                    companyLine2 = Properties.Settings.Default.CompanyNameLine2;
                    TxtCompany.Focus();
                }
                else if (string.IsNullOrWhiteSpace(fiscalAddressLine1))
                {
                    EnableEditing(true);
                    fiscalAddressLine1 = Properties.Settings.Default.FiscalAddressLine1;
                    TxtFiscalAddress.Focus();
                }
                else if (string.IsNullOrWhiteSpace(fiscalAddressLine2))
                {
                    EnableEditing(true);
                    fiscalAddressLine2 = Properties.Settings.Default.FiscalAddressLine2;
                    TxtFiscalAddress.Focus();
                }
                else if (string.IsNullOrWhiteSpace(fiscalAddressLine3))
                {
                    EnableEditing(true);
                    fiscalAddressLine3 = Properties.Settings.Default.FiscalAddressLine3;
                    TxtFiscalAddress.Focus();
                }
                else if (string.IsNullOrWhiteSpace(addressLine1))
                {
                    EnableEditing(true);
                    addressLine1 = Properties.Settings.Default.AddressLine1;
                    TxtAddress.Focus();
                }
                else if (string.IsNullOrWhiteSpace(addressLine2))
                {
                    EnableEditing(true);
                    addressLine2 = Properties.Settings.Default.AddressLine2;
                    TxtAddress.Focus();
                }
                else if (string.IsNullOrWhiteSpace(addressLine3))
                {
                    EnableEditing(true);
                    addressLine3 = Properties.Settings.Default.AddressLine3;
                    TxtAddress.Focus();
                }
                else if (string.IsNullOrWhiteSpace(productName))
                {
                    productName = Properties.Settings.Default.ProductName;
                    CboProduct.Focus();
                }
                else if (string.IsNullOrWhiteSpace(brand))
                {
                    brand = Properties.Settings.Default.Brand;
                    CboBrand.Focus();
                }
                else if (string.IsNullOrWhiteSpace(model))
                {
                    model = Properties.Settings.Default.Model;
                    TxtModel.Focus();
                }
                else if (string.IsNullOrWhiteSpace(series))
                {
                    series = Properties.Settings.Default.Series;
                    TxtSeries.Focus();
                }
                else if (serialNumberEnd == 0)
                {
                    serialNumberStart = Properties.Settings.Default.SerialNumberStart;
                    serialNumberEnd = Properties.Settings.Default.SerialNumberEnd;
                    NumSerialNumberEnd.Focus();
                }
                else if ((serialNumberStart == 0) || (serialNumberEnd < serialNumberStart))
                {
                    serialNumberStart = Properties.Settings.Default.SerialNumberStart;
                    serialNumberEnd = Properties.Settings.Default.SerialNumberEnd;
                    NumSerialNumberStart.Focus();
                }
                else if (string.IsNullOrWhiteSpace(specs))
                {
                    EnableEditing(true);
                    specs = Properties.Settings.Default.Specs;
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
            editing = enable;
        }

        private static void GenerateDocument(string filePath)
        {
            //uint _totalLabels = serialNumberEnd - serialNumberStart + 1;
            uint _totalLabels = serialNumberStart + 5;

            if ((_totalLabels % 6) != 0)
            {
                _totalLabels += (6 - (_totalLabels % 6));
            }

            var serialNumbers = Enumerable
                .Range((int)serialNumberStart, (int)_totalLabels)
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

        private static void ComposeLabel(IContainer container, int serial)
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
                        importer.Item().Text(companyLine1).Bold();
                        importer.Item().Text(companyLine2).Bold();
                    });

                    col.Item().Text("");
                    col.Item().Row(r =>
                    {
                        r.RelativeItem(_row1Length).Text("Domicilio Fiscal:");
                        r.RelativeItem(_row2Length).Column(addr =>
                        {
                            addr.Item().Text(fiscalAddressLine1);
                            addr.Item().Text(fiscalAddressLine2);
                            addr.Item().Text(fiscalAddressLine3);
                        });
                    });

                    col.Item().Text("Domicilio Planta Reconstructora:");

                    col.Item().PaddingLeft(_padding2).Column(addr =>
                    {
                        addr.Item().Text(addressLine1);
                        addr.Item().Text(addressLine2);
                        addr.Item().Text(addressLine3);
                    });

                    col.Item().Row(r =>
                    {
                        r.RelativeItem(_row1Length).Text("Nombre del producto:").Bold();
                        r.RelativeItem(_row2Length).Text(productName).Bold();
                    });

                    col.Item().Row(r =>
                    {
                        r.RelativeItem(_row1Length).Text("Marca:").Bold();
                        r.RelativeItem(_row2Length).Text(brand).Bold();
                    });

                    col.Item().Row(r =>
                    {
                        r.RelativeItem(_row1Length).Text("Modelo:").Bold();
                        r.RelativeItem(_row2Length).Text(model).Bold();
                    });

                    col.Item().Row(r =>
                    {
                        r.RelativeItem(_row1Length).Text("Serie:").Bold();
                        r.RelativeItem(_row2Length).Text(series).Bold();
                    });

                    col.Item().Row(r =>
                    {
                        r.RelativeItem(_row1Length).Text("Especificaciones:");
                        r.RelativeItem(_row2Length).Text(specs);
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
