using System.Reflection;

namespace EtiquetasArca
{
    internal class License
    {
        public LicenseStatus Status
        {
            get
            {
                if (Json.Equals(""))
                {
                    return LicenseStatus.License_NotFound;
                }
                else if (Json.Equals("ERROR"))
                {
                    return LicenseStatus.License_Error;
                }
                else if ((Payload.ExpiryDate ?? DateTime.MaxValue) <= DateTime.Today)
                {
                    return LicenseStatus.License_Expired;
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(Payload.Version) || IsVersionCompatible())
                    {
                        if (((Payload.ExpiryDate ?? DateTime.MaxValue) - DateTime.Today).TotalDays <= 30)
                        {
                            return LicenseStatus.License_Expiring;
                        }
                        else
                        {
                            return LicenseStatus.License_Valid;
                        }
                    }
                    else
                    {
                        return LicenseStatus.License_VersionInvalid;
                    }
                }
            }
        }

        public readonly LicensePayload Payload;
        public readonly string Json;
        public readonly byte[] Signature;

        public License()
        {
            Payload = new LicensePayload();
            Json = string.Empty;
            Signature = [];
        }

        public License(LicensePayload payload, string json, byte[] signature)
        {
            Payload = payload;
            Json = json;
            Signature = signature;
        }

        private bool IsVersionCompatible()
        {
            bool isVersionCompatible = false;

            string licenseVersion = Payload.Version ?? string.Empty;
            string appVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? string.Empty;

            if (licenseVersion == string.Empty)
            {
                isVersionCompatible = true;
            }
            else
            {
                /* License version can contain wildcards, e.g., "1.0.*" */
                string[] licenseParts = licenseVersion.Split('.');
                string[] appParts = appVersion.Split('.');

                if (licenseParts.Length == appParts.Length)
                {
                    isVersionCompatible = true;
                    for (int i = 0; i < licenseParts.Length; i++)
                    {
                        if ((licenseParts[i] != "*") && (licenseParts[i] != appParts[i]))
                        {
                            isVersionCompatible = false;
                            break;
                        }
                    }
                }
            }

            return isVersionCompatible;
        }
    }
}
