namespace EtiquetasArca
{
    internal class LicensePayload
    {
        public string LicenseId { get; set; }
        public string Type { get; set; }
        public string? Version { get; set; }
        public DateTime IssuedDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string? MachineId { get; set; }

        public LicensePayload()
        {
            LicenseId = string.Empty;
            Type = string.Empty;
            Version = string.Empty;
            IssuedDate = DateTime.MaxValue;
            ExpiryDate = DateTime.MinValue;
            MachineId = string.Empty;
        }
    }
}
