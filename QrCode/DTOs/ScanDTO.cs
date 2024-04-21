using QrCode.DB.Models;
namespace QrCode.API.DTOs;

public class ScanDTO
{
    public string DeviceType { get; set; }
    public string HashValue { get; set; }
    public string Country { get; set; }
    public string Browser { get; set; }
}
