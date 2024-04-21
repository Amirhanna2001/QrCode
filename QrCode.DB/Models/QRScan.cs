using System.ComponentModel.DataAnnotations.Schema;
namespace QrCode.DB.Models;
public class QRScan
{
    public int ID { get; set; }
    public DeviceType DeviceType { get ; set; }
    public string Country { get; set; }
    public string Browser { get; set; }
    [ForeignKey("QRCode")]
    public int QRCodeID { get; set; }
    public QRCode QRCode { get; set; }
}