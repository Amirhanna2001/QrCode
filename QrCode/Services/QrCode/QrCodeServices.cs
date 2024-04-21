using QrCode.DB.Models;

namespace QrCode.API.Services;

public class QrCodeServices:IQrCodeServices
{
    public Dictionary<string, int> BrowserOfScans(List<QRScan> scansOfThisQrCode)
    {
        return scansOfThisQrCode
                    .GroupBy(s => s.Browser)
                    .ToDictionary(group => group.Key, group => group.Count());
    }

    public Dictionary<DeviceType, int> DevicesOfScans(List<QRScan> scansOfThisQrCode)
    {
        return scansOfThisQrCode
                    .GroupBy(s => s.DeviceType)
                    .ToDictionary(group => group.Key, group => group.Count());
    }

    public Dictionary<string, int> CountriesOfScan(List<QRScan> scansOfThisQrCode)
    {
        return scansOfThisQrCode
             .GroupBy(s => s.Country)
             .ToDictionary(group => group.Key, group => group.Count());
    }
}
