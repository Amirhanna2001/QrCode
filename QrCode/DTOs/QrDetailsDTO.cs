using QrCode.DB.Models;

namespace QrCode.API.DTOs
{
    public class QrDetailsDTO
    {
        public string QRName { get; set; }
        public string URLValue { get; set; }
        public string qrPath { get; set; }
        public int NoOfScans { get; set; }

        public Dictionary<DeviceType, int> DeviceScans { get; set; } = new();
        public Dictionary<string, int> CountryScans { get; set; } = new();
        public Dictionary<string, int> BrowserScans { get; set; } = new();
    }
}
