using QrCode.DB.Models;

namespace QrCode.API.Services;

public interface IQrCodeServices
{
    Dictionary<string, int> BrowserOfScans(List<QRScan> scansOfThisQrCode);
    Dictionary<DeviceType, int> DevicesOfScans(List<QRScan> scansOfThisQrCode);
    Dictionary<string, int> CountriesOfScan(List<QRScan> scansOfThisQrCode);
}
