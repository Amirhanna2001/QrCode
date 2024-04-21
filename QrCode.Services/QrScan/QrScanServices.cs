using QrCode.DB.Models;

namespace QrCode.Services;
public class QrScanServices
{
    public static Dictionary<string, int> BrowserOfScans(List<QRScan> scans)
    {
        return scans.GroupBy(s => s.Browser)
                    .ToDictionary(group => group.Key, group => group.Count());
    }

    public static Dictionary<DeviceType, int> DevicesOfScans(List<QRScan> scans)
    {
        return scans.GroupBy(s => s.DeviceType)
                    .ToDictionary(group => group.Key, group => group.Count());
    }

    public static Dictionary<string, int> CountriesOfScan(List<QRScan> scans)
    {
        return scans.GroupBy(s => s.Country)
             .ToDictionary(group => group.Key, group => group.Count());
    }
}
