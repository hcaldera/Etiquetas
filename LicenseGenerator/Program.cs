using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace LicenseGenerator
{
    internal class Program
    {
        public const int TRIAL_DAYS = 7;
        static void Main(string[] args)
        {
            LicensePayload payload = new LicensePayload();

            var options = CommandLineOptions.ParseArgs(args ?? Array.Empty<string>());

            if ((CommandLineOptions.GetOption(options, "help", "h") == "true") || options.Count == 0)
            {
                CommandLineOptions.PrintUsage();
                return;
            }

            bool generate = CommandLineOptions.GetOption(options, "generate", "g") == "true";
            bool generateKey = CommandLineOptions.GetOption(options, "key", "k") == "true";
            string type = CommandLineOptions.GetOption(options, "type", "t");
            string version = CommandLineOptions.GetOption(options, "version", "v");
            string issuedStr = CommandLineOptions.GetOption(options, "issued", "i");
            string expiryStr = CommandLineOptions.GetOption(options, "expiry", "e");
            string machineId = CommandLineOptions.GetOption(options, "machine", "m");

            if (generateKey)
            {
                GenerateKey();
            }

            if (generate)
            {
                /* Type */
                if (string.IsNullOrWhiteSpace(type))
                {
                    payload.Type = "trial";
                }
                else
                {
                    switch (type.ToLower())
                    {
                        case "full":
                        case "pro":
                        case "enterprise":
                        case "trial":
                            payload.Type = type.ToLower();
                            break;
                        default:
                            Console.WriteLine("Error: Invalid license type. Allowed values are: full, pro, enterprise, trial.");
                            CommandLineOptions.PrintUsage();
                            return;
                    }
                }

                /* Version */
                if (IsVersionValid(version))
                {
                    payload.Version = version;
                }
                else
                {
                    Console.WriteLine("Error: Invalid version format. Expected format: major.minor.patch (e.g., 1.0.0).");
                    CommandLineOptions.PrintUsage();
                    return;
                }

                /* Issued date */
                if (!string.IsNullOrWhiteSpace(issuedStr))
                {
                    if (DateTime.TryParse(issuedStr, out var issued))
                    {
                        payload.IssuedDate = issued;
                    }
                    else
                    {
                        Console.WriteLine("Error: Issued date invalid.");
                        return;
                    }
                }
                else
                {
                    payload.IssuedDate = DateTime.Today;
                }

                /* Expiry date */
                if ((string.Compare(payload.Type, "trial") == 0) && string.IsNullOrWhiteSpace(expiryStr))
                {
                    payload.ExpiryDate = payload.IssuedDate.AddDays(TRIAL_DAYS);
                }
                else if (!string.IsNullOrWhiteSpace(expiryStr))
                {
                    if (DateTime.TryParse(expiryStr, out var expiry))
                    {
                        if (expiry <= payload.IssuedDate)
                        {
                            Console.WriteLine("Error: Expiry date must be greater than issued date.");
                            return;
                        }
                        payload.ExpiryDate = expiry;
                    }
                    else
                    {
                        Console.WriteLine("Error: Expiry date invalid.");
                        return;
                    }
                }
                else
                {
                    payload.ExpiryDate = null; // No expiry
                }

                payload.MachineId = machineId;

                GenerateLicense(payload);
            }
            else
            {
                if (!generateKey)
                {
                    CommandLineOptions.PrintUsage();
                }
            }
        }

        private static void GenerateKey()
        {
            Console.WriteLine("Generating RSA key...");

            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false; // Don't persist the key in the CSP

                string privateKeyXml = rsa.ToXmlString(true); // Get the private key in XML format
                string publicKeyXml = rsa.ToXmlString(false); // Get the public key in XML format

                File.WriteAllText("rsa_private_key.xml", privateKeyXml);
                File.WriteAllText("rsa_public_key.xml", publicKeyXml);
            }

            Console.WriteLine("Keys generated successfully.");
            Console.WriteLine("Private key saved to: rsa_private_key.xml");
            Console.WriteLine("Public key saved to: rsa_public_key.xml");
        }

        private static void GenerateLicense(LicensePayload payload)
        {
            RsaHelper.LoadRsaKey(true); // Load private key for signing

            Console.WriteLine("Generating license with the following details:");
            Console.WriteLine($"License ID: {payload.LicenseId = GenerateLicenseId()}");
            Console.WriteLine($"Type: {payload.Type}");
            Console.WriteLine($"Version: {payload.Version ?? "null"}");
            Console.WriteLine($"Issued Date: {payload.IssuedDate}");
            Console.WriteLine($"Expiry Date: {payload.ExpiryDate?.ToString() ?? "null"}");
            Console.WriteLine($"Machine ID: {payload.MachineId ?? "null"}");

            string json = JsonConvert.SerializeObject(payload, Formatting.Indented);
            byte[] data = Encoding.UTF8.GetBytes(json);
            byte[] signature = RsaHelper.RsaSign(json);

            string licenseContent = Convert.ToBase64String(data) + "." + Convert.ToBase64String(signature);

            Console.WriteLine();
            Console.WriteLine("License content:");
            Console.WriteLine();
            Console.WriteLine(licenseContent);
            Console.WriteLine();

            ValidateLicense(licenseContent);
        }

        private static void ValidateLicense(string licenseContent)
        {
            RsaHelper.LoadRsaKey(false); // Load public key for verification
            var parts = licenseContent.Split('.');
            if (parts.Length != 2)
            {
                Console.WriteLine("Invalid license format.");
                return;
            }

            try
            {
                byte[] data = Convert.FromBase64String(parts[0]);
                string json = Encoding.UTF8.GetString(data);
                byte[] signature = Convert.FromBase64String(parts[1]);

                if (RsaHelper.RsaVerify(json, signature))
                {
                    File.WriteAllText("license.lic", licenseContent);
                    Console.WriteLine("License is valid.");
                    Console.WriteLine();
                    Console.WriteLine("License saved to license.lic");
                }
                else
                {
                    Console.WriteLine("Invalid license signature.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error validating license: " + ex.Message);
            }
        }

        private static string GenerateLicenseId()
        {
            byte[] bytes = new byte[6]; // 48 bits

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }

            string hex = BitConverter.ToString(bytes).Replace("-", "").ToUpper(); // 12 hex chars

            return $"{hex.Substring(0, 4)}-{hex.Substring(4, 4)}-{hex.Substring(8, 4)}"; // Format as XXXX-XXXX-XXXX
        }

        private static bool IsVersionValid(string version)
        {
            if (string.IsNullOrWhiteSpace(version))
                return true; // treat null or empty as valid
            // Simple regex for semantic versioning: major.minor.patch
            var semverRegex = new Regex(@"^(\d+|\*)\.(\d+|\*)\.(\d+|\*)$");
            return semverRegex.IsMatch(version);
        }
    }

    internal class LicensePayload
    {
        public string LicenseId { get; set; }
        public string Type { get; set; }
        public string Version { get; set; }
        public DateTime IssuedDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string MachineId { get; set; }
    }

    internal static class RsaHelper
    {
        private static RSACryptoServiceProvider _rsa;

        public static void LoadRsaKey(bool privateKey)
        {
            string xml = privateKey ? Properties.Settings.Default.PrivateKey : Properties.Settings.Default.PublicKey;

            _rsa = new RSACryptoServiceProvider
            {
                PersistKeyInCsp = false
            };

            _rsa.FromXmlString(xml);
        }

        public static byte[] RsaEncrypt(string plainText)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(plainText);
                return _rsa.Encrypt(data, false);
            }
            catch
            {
                return null;
            }
        }

        public static string RsaDecrypt(byte[] cipherText)
        {
            try
            {
                byte[] data = _rsa.Decrypt(cipherText, false);
                return Encoding.UTF8.GetString(data);
            }
            catch
            {
                return null;
            }
        }

        public static byte[] RsaSign(string message)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(message);
                return _rsa.SignData(data, "SHA256");
            }
            catch
            {
                return null;
            }
        }

        public static bool RsaVerify(string message, byte[] signature)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(message);
                return _rsa.VerifyData(data, "SHA256", signature);
            }
            catch
            {
                return false;
            }
        }
    }

    internal class CommandLineOptions
    {
        public static Dictionary<string, string> ParseArgs(string[] args)
        {
            var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            for (int i = 0; i < args.Length; i++)
            {
                var a = args[i];
                if (string.IsNullOrWhiteSpace(a))
                    continue;

                if (a == "-h" || a == "--help")
                {
                    result["help"] = "true";
                    continue;
                }

                if (a.StartsWith("--"))
                {
                    var rest = a.Substring(2);
                    var eq = rest.IndexOf('=');
                    if (eq >= 0)
                    {
                        var key = rest.Substring(0, eq);
                        var val = rest.Substring(eq + 1);
                        result[key] = val;
                    }
                    else
                    {
                        // next token as value if it doesn't start with -
                        if (i + 1 < args.Length && !args[i + 1].StartsWith("-"))
                            result[rest] = args[++i];
                        else
                            result[rest] = "true";
                    }
                }
                else if (a.StartsWith("-"))
                {
                    var rest = a.Substring(1);
                    if (rest.Length > 1)
                    {
                        // treat as combined short flags like -vf -> v=true, f=true
                        foreach (var ch in rest)
                            result[ch.ToString()] = "true";
                    }
                    else
                    {
                        if (i + 1 < args.Length && !args[i + 1].StartsWith("-"))
                            result[rest] = args[++i];
                        else
                            result[rest] = "true";
                    }
                }
                else
                {
                    // positional argument(s) collected under _
                    if (!result.ContainsKey("_"))
                        result["_"] = a;
                    else
                        result["_"] = result["_"] + " " + a;
                }
            }

            return result;
        }

        public static string GetOption(Dictionary<string, string> opts, string longName, string shortName)
        {
            if (opts.TryGetValue(longName, out var v)) return v;
            if (opts.TryGetValue(shortName, out v)) return v;
            return null;
        }

        public static void PrintUsage()
        {
            Console.WriteLine("Usage: LicenseGenerator [options] [positional]");
            Console.WriteLine("Options:");
            Console.WriteLine("  -h, --help          Show this help");
            Console.WriteLine("  -g, --generate      Generate license file");
            Console.WriteLine("  -k, --key           Generate a new RSA key");
            Console.WriteLine("  -t, --type <type>   License Type");
            Console.WriteLine("  -v, --version <ver> License Version");
            Console.WriteLine("  -i, --issued <date> Issued date (YYYY-MM-DD)");
            Console.WriteLine("  -e, --expiry <date> Expiry date (YYYY-MM-DD)");
            Console.WriteLine("  -m, --machine <id>  Machine ID");
            Console.WriteLine();
            Console.WriteLine("Examples:");
            Console.WriteLine("  LicenseGenerator -k");
            Console.WriteLine("  LicenseGenerator -g");
            Console.WriteLine("  LicenseGenerator -g -t Pro -v 1.0.0 -i 2024-01-01 -e 2025-01-01 -m MACHINEID");
            Console.WriteLine("  LicenseGenerator -g -t Pro -v 1.0.0 -i 2024-01-01 -e 2025-01-01");
            Console.WriteLine("  LicenseGenerator -g -t Pro -v 1.0.0 -i 2024-01-01");
            Console.WriteLine("  LicenseGenerator -g -t Pro -v 1.0.0 -e 2025-01-01");
            Console.WriteLine("  LicenseGenerator -g -t Pro -v 1.0.0");
            Console.WriteLine("  LicenseGenerator -g -t Pro");
        }
    }
}
