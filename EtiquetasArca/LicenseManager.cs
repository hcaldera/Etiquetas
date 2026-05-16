using System.Reflection;
using System.Runtime.Versioning;
using System.Text;

namespace EtiquetasArca
{
    [SupportedOSPlatform("windows")]
    public partial class LicenseManager : Form
    {
        private readonly string AppFolderPath;
        private readonly string LicenseFilePath;
        private readonly string RegisterPath;
        private readonly string RedundantRegisterPath;
        private readonly RsaHelper rsaHelper;

        private License License;
        public bool LicenseOK { get; private set; }
        public bool InstallationOK { get; private set; }

        public LicenseManager()
        {
            string appName = Assembly.GetExecutingAssembly().GetName().Name ?? string.Empty;
            string progDataDir = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

            InitializeComponent();

            rsaHelper = new RsaHelper();
            rsaHelper.LoadKey(Properties.Settings.Default.PublicKey);

            AppFolderPath = Path.Combine(progDataDir, appName);
            LicenseFilePath = Path.Combine(AppFolderPath, "license.lic");
            RegisterPath = $@"SOFTWARE\{appName}";
            RedundantRegisterPath = $@"SOFTWARE\Classes\CLSID\{{{Properties.Settings.Default.CLSID}}}";

            License = new License();
            UpdateLicenseStatus();
            UpdateInstallationStatus();
        }

        private void UpdateLicenseStatus()
        {
            LicenseOK = false;

            if (File.Exists(LicenseFilePath))
            {
                if (GetLicenseFromFile(LicenseFilePath, out License))
                {
                    LicenseOK = License.Status switch
                    {
                        LicenseStatus.License_Valid or LicenseStatus.License_Expiring => true,
                        _ => false,
                    };
                }
            }
        }

        private void UpdateInstallationStatus()
        {
            var systemKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(RegisterPath);
            object? payloadReg = systemKey?.GetValue("Payload");
            object? signatureReg = systemKey?.GetValue("Signature");

            var redundantKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(RedundantRegisterPath);
            object? redundantPayloadReg = redundantKey?.GetValue("R2");
            object? redundantSignatureReg = redundantKey?.GetValue("R3");
            object? protectedIDsCountReg = redundantKey?.GetValue("RC");

            var protectedIDsKey = redundantKey?.OpenSubKey("R");
            string[] protectedIDsRegs = protectedIDsKey?.GetValueNames() ?? [];

            LicenseStatus status = License.Status;

            InstallationOK = false;

            switch (status)
            {
                case LicenseStatus.License_NotFound:
                    if (payloadReg is not null ||
                        signatureReg is not null ||
                        redundantKey is not null)
                    {
                        MessageBox.Show("Parece que el programa ha sido manipulado o utilizado en otra máquina. Para desbloquear todas las funciones, adquiere la versión completa.",
                                        "Instalación No Válida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        InstallationOK = true;

                        MessageBox.Show("No se ha encontrado una licencia válida. Para desbloquear todas las funciones, adquiere la versión completa.",
                                        "Licencia No Encontrada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    break;
                case LicenseStatus.License_Valid:
                case LicenseStatus.License_Expiring:
                case LicenseStatus.License_Expired:
                    if (((payloadReg?.ToString() is string payloadStr) && !payloadStr.Equals("")) &&
                        ((signatureReg?.ToString() is string signatureStr) && !signatureStr.Equals("")) &&
                        ((redundantPayloadReg?.ToString() is string redPayloadStr) && !redPayloadStr.Equals("")) &&
                        ((redundantSignatureReg?.ToString() is string redSignatureStr) && !redSignatureStr.Equals("")) &&
                        ((protectedIDsCountReg?.ToString() is string protectedIDsCountStr) && !protectedIDsCountStr.Equals("")) &&
                        ((protectedIDsRegs is not null) && (protectedIDsRegs.Length > 0)) &&
                        (protectedIDsRegs.Length == int.Parse(Unprotect(protectedIDsCountStr))))
                    {
                        var licensesCount = Enumerable.Range(1, protectedIDsRegs.Length).Select(i => $"R{i}").ToList();
                        DateTime lstRunDate = DateTime.MaxValue;
                        bool idProtected = false;

                        InstallationOK =
                            $"R|{Encoding.UTF8.GetString(Unprotect(payloadStr))}".Equals(Encoding.UTF8.GetString(Unprotect(redPayloadStr))) &&
                            $"R|{Convert.ToBase64String(Unprotect(signatureStr))}".Equals(Encoding.UTF8.GetString(Unprotect(redSignatureStr))) &&
                            Encoding.UTF8.GetString(Unprotect(payloadStr)).Equals(License.Json) &&
                            Convert.ToBase64String(Unprotect(signatureStr)).Equals(Convert.ToBase64String(License.Signature));

                        foreach (string regName in protectedIDsRegs)
                        {
                            object? protectedIDReg = protectedIDsKey?.GetValue(regName);
                            string protectedIDStr = protectedIDReg?.ToString() ?? string.Empty;

                            InstallationOK = InstallationOK && licensesCount.Remove(regName);
                            InstallationOK = InstallationOK && !protectedIDStr.Equals("");
                            idProtected = idProtected || Encoding.UTF8.GetString(Unprotect(protectedIDStr)).Equals(License.Payload.LicenseId);
                        }

                        InstallationOK = InstallationOK && (idProtected && (licensesCount.Count == 0));
                    }

                    if (InstallationOK)
                    {
                        if (LicenseOK)
                        {
                            TxtType.Text = License.Payload.Type;
                            TxtVersion.Text = License.Payload.Version ?? string.Empty;
                            TxtIssuedDate.Text = License.Payload.IssuedDate.ToString("O");
                            TxtExpiryDate.Text = License.Payload.ExpiryDate?.ToString("O") ?? string.Empty;
                            TxtMachineID.Text = License.Payload.MachineId ?? string.Empty;
                        }


                        if (status == LicenseStatus.License_Expiring)
                        {
                            MessageBox.Show("Tu licencia está próxima a expirar. Para desbloquear todas las funciones, adquiere la versión completa.",
                                            "Licencia Próxima a Expirar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else if (status == LicenseStatus.License_Expired)
                        {
                            MessageBox.Show("Tu licencia ha expirado. Para desbloquear todas las funciones, adquiere la versión completa.",
                                            "Licencia Expirada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            /* License valid. Do nothing */
                        }
                    }
                    else
                    {
                        MessageBox.Show("Parece que el programa ha sido manipulado o utilizado en otra máquina. Para desbloquear todas las funciones, adquiere la versión completa.",
                                        "Instalación No Válida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    break;

                case LicenseStatus.License_VersionInvalid:
                    MessageBox.Show("La licencia no es válida para esta versión del programa. Para desbloquear todas las funciones, adquiere la versión completa.",
                                    "Licencia No Válida para esta Versión", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    break;
                case LicenseStatus.License_Error:
                    MessageBox.Show("Ocurrió un error al procesar la licencia. Para desbloquear todas las funciones, adquiere la versión completa.",
                                    "Error de Licencia", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    break;
                default:
                    MessageBox.Show("Ocurrió un error desconocido al verificar la licencia. Para desbloquear todas las funciones, adquiere la versión completa.",
                                    "Error Desconocido de Licencia", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    break;
            }
        }

        private bool GetLicenseFromFile(string filePath, out License license, bool newLicense = false)
        {
            bool isContentValid = false;
            license = new(new LicensePayload(), "ERROR", []);

            try
            {
                string content = File.ReadAllText(filePath);
                var parts = content.Split('.');

                if (newLicense && (parts.Length == 2))
                {
                    string json = Encoding.UTF8.GetString(Convert.FromBase64String(parts[0]));
                    byte[] signature = Convert.FromBase64String(parts[1]);

                    if (rsaHelper.VerifySignature(json, signature))
                    {
                        LicensePayload? payload = System.Text.Json.JsonSerializer.Deserialize<LicensePayload>(json);
                        if (payload != null)
                        {
                            license = new(payload, json, signature);
                            isContentValid = true;
                        }
                    }
                }
                else if (!newLicense && (parts.Length == 3))
                {
                    string machineName = Encoding.UTF8.GetString(Unprotect(parts[0]));
                    string json = Encoding.UTF8.GetString(Unprotect(parts[1]));
                    byte[] signature = Unprotect(parts[2]);

                    if (!machineName.Equals(Environment.MachineName))
                    {
                        throw new Exception("El archivo de licencia no es válido para esta máquina.");
                    }

                    if (rsaHelper.VerifySignature(json, signature))
                    {
                        LicensePayload? payload = System.Text.Json.JsonSerializer.Deserialize<LicensePayload>(json);
                        if (payload != null)
                        {
                            license = new(payload, json, signature);
                            isContentValid = true;
                        }
                    }
                }
                else
                {
                    throw new Exception("El formato del archivo de licencia no es válido.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al leer el archivo de licencia.\r\nPor favor, verifica tu archivo de licencia e inténtalo de nuevo.\r\n{ex.Message}",
                                "Error al Leer Licencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return isContentValid;
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnLoadLicense_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new()
            {
                DefaultExt = "lic",
                Filter = "Archivos de licencia (*.lic)|*.lic|Todos los archivos (*.*)|*.*",
                Multiselect = false,
                Title = "Selecciona un archivo de licencia"
            };

            UpdateInstallationStatus();
            if (InstallationOK && dlg.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(dlg.FileName))
                {
                    try
                    {
                        if (GetLicenseFromFile(dlg.FileName, out License license, true))
                        {
                            var redundantKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(RedundantRegisterPath, true) ??
                                               Microsoft.Win32.Registry.LocalMachine.CreateSubKey(RedundantRegisterPath, true);
                            var redundantIDsKey = redundantKey?.OpenSubKey("R", true) ?? redundantKey?.CreateSubKey("R", true);
                            string[] protectedIDRegs = redundantIDsKey?.GetValueNames() ?? [];

                            bool idProtected = false;

                            foreach (string regName in protectedIDRegs)
                            {
                                object? protectedIDReg = redundantIDsKey?.GetValue(regName);
                                string protectedIDStr = protectedIDReg?.ToString() ?? string.Empty;

                                if (Encoding.UTF8.GetString(Unprotect(protectedIDStr)).Equals(license.Payload.LicenseId))
                                {
                                    idProtected = true;
                                    break;
                                }
                            }

                            if (idProtected)
                            {
                                MessageBox.Show("La licencia proporcionada ya ha sido utilizada en esta máquina. Por favor, verifica tu archivo de licencia e inténtalo de nuevo.",
                                                "Licencia Inválida", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                DateTime now = DateTime.Now;
                                var systemKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(RegisterPath, true);

                                systemKey?.SetValue("LastRun", Protect(Encoding.UTF8.GetBytes(now.ToString("O"))));
                                systemKey?.SetValue("Payload", Protect(Encoding.UTF8.GetBytes(license.Json)));
                                systemKey?.SetValue("Signature", Protect(license.Signature));

                                /* Protect() uses DPAPI service to encrypt data, which ensures the same
                                 * data encrypted twice will produce different results. Here, adding a
                                 * prefix "R|" to each value helps in preventing copy-paste attacks. */
                                redundantKey?.SetValue("R1", Protect(Encoding.UTF8.GetBytes($"R|{now:O}")));
                                redundantKey?.SetValue("R2", Protect(Encoding.UTF8.GetBytes($"R|{license.Json}")));
                                redundantKey?.SetValue("R3", Protect(Encoding.UTF8.GetBytes($"R|{Convert.ToBase64String(license.Signature)}")));
                                
                                /* Update, protect and save number of IDs protected so far */
                                redundantKey?.SetValue("RC", Protect(Encoding.UTF8.GetBytes((protectedIDRegs.Length + 1).ToString())));
                                
                                /* Saving the new protected ID */
                                redundantIDsKey?.SetValue($"R{protectedIDRegs.Length + 1}", Protect(Encoding.UTF8.GetBytes(license.Payload.LicenseId)));

                                if (!Directory.Exists(AppFolderPath))
                                {
                                    Directory.CreateDirectory(AppFolderPath);
                                }

                                File.WriteAllText(LicenseFilePath,
                                    $"{Protect(System.Text.Encoding.UTF8.GetBytes(Environment.MachineName))}." +
                                    $"{Protect(System.Text.Encoding.UTF8.GetBytes(license.Json))}." +
                                    $"{Protect(license.Signature)}");

                                UpdateLicenseStatus();
                                UpdateInstallationStatus();

                                if (LicenseOK || InstallationOK)
                                {
                                    if (License.Payload.Type.Equals("trial"))
                                    {
                                        MessageBox.Show("Licencia de prueba válida. Gracias por probar el programa. Para desbloquear todas las funciones, adquiere la versión completa.",
                                                        "Licencia de Prueba Válida", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                    else
                                    {
                                        MessageBox.Show("Licencia válida. Gracias por adquirir la versión completa.",
                                                        "Licencia Válida", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                }
                            }
                        }
                        else
                        {
                            throw new Exception("El contenido del archivo de licencia no es válido.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ocurrió un error al guardar la licencia: {ex.Message}",
                                        "Error al Guardar Licencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("El archivo seleccionado no existe. Por favor, verifica tu archivo de licencia e inténtalo de nuevo.",
                                    "Archivo No Encontrado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private static string Protect(byte[] data)
        {
            byte[] protectedData = System.Security.Cryptography.ProtectedData.Protect(
                data, null, System.Security.Cryptography.DataProtectionScope.LocalMachine);
            return Convert.ToBase64String(protectedData);
        }

        private static byte[] Unprotect(string protectedData)
        {
            byte[] protectedBytes = Convert.FromBase64String(protectedData);
            byte[] unprotectedData = System.Security.Cryptography.ProtectedData.Unprotect(
                protectedBytes, null, System.Security.Cryptography.DataProtectionScope.LocalMachine);

            return unprotectedData;
        }
    }
}
