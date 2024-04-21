using QrCode.DB.Models;

namespace QrCode.Services;
public interface IQrScanServices
{
    Dictionary<string, int> BrowserOfScans(List<QRScan> scans);
    Dictionary<DeviceType, int> DevicesOfScans(List<QRScan> scans);
    Dictionary<string, int> CountriesOfScan(List<QRScan> scans);
}
