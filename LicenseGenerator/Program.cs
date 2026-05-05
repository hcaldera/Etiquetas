using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace LicenseGenerator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool generate;
            bool generateKey;
            string type;
            string version;
            DateTime? issuedDate;
            DateTime? expiryDate;
            string machineId;
            LicensePayload payload = new LicensePayload();

            var options = CommandLineOptions.ParseArgs(args ?? Array.Empty<string>());

            if (options.ContainsKey("help") || options.ContainsKey("h") || options.Count == 0)
            {
                CommandLineOptions.PrintUsage();
                return;
            }

            generate = options.ContainsKey("generate") || options.ContainsKey("g");
            generateKey = options.ContainsKey("k") || options.ContainsKey("key");
            type = CommandLineOptions.GetOption(options, "type", "t");
            version = CommandLineOptions.GetOption(options, "version", "v");
            issuedDate = ((CommandLineOptions.GetOption(options, "issued", "i") is string issuedStr) &&
                          DateTime.TryParse(issuedStr, out var issued)) ?
                          issued : (DateTime?)null;
            expiryDate = ((CommandLineOptions.GetOption(options, "expiry", "e") is string expiryStr) &&
                          DateTime.TryParse(expiryStr, out var expiry)) ?
                          expiry : (DateTime?)null;
            machineId = CommandLineOptions.GetOption(options, "machine", "m");

            if (generateKey)
            {
                GenerateKey();
            }

            if (generate)
            {
                if (string.IsNullOrWhiteSpace(type))
                {
                    payload.Type = "Trial";
                }
                else
                {
                    switch (type)
                    {
                        case "Full":
                        case "Pro":
                        case "Enterprise":
                        case "Trial":
                            payload.Type = type;
                            break;
                        default:
                            payload.Type = "Trial";
                            break;
                    }
                }

                if (string.IsNullOrWhiteSpace(version))
                {
                    payload.Version = "0.1.0";
                }
                else
                {
                    payload.Version = version;
                }

                if (null == issuedDate)
                {
                    payload.IssuedDate = DateTime.Today;
                }
                else
                {
                    payload.IssuedDate = issuedDate.Value;
                }

                if (string.Compare(payload.Type, "Trial", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    payload.ExpiryDate = DateTime.Today.AddDays(RsaHelper.TRIAL_DAYS);
                }
                else
                {
                    payload.ExpiryDate = expiryDate.Value;
                }

                payload.MachineId = machineId;

                GenerateLicense(payload);
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
            Console.WriteLine($"Type: {payload.Type}");
            Console.WriteLine($"Version: {payload.Version}");
            Console.WriteLine($"Issued Date: {payload.IssuedDate}");
            Console.WriteLine($"Expiry Date: {(payload.ExpiryDate) ?.ToString() ?? "null"}");
            Console.WriteLine($"Machine ID: {(payload.MachineId) ?? "null"}");

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
                byte[] signature = Convert.FromBase64String(parts[1]);
                string json = Encoding.UTF8.GetString(data);
                if (RsaHelper.RsaVerify(json, signature))
                {
                    var payload = JsonConvert.DeserializeObject<LicensePayload>(json);
                    Console.WriteLine("License is valid. Payload:");
                    Console.WriteLine(JsonConvert.SerializeObject(payload, Formatting.Indented));
                    File.WriteAllText("license.lic", licenseContent);
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
    }

    internal class LicensePayload
    {
        public string Type { get; set; }
        public string Version { get; set; }
        public DateTime IssuedDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string MachineId { get; set; }
    }

    internal static class RsaHelper
    {
        public const int TRIAL_DAYS = 7;

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
