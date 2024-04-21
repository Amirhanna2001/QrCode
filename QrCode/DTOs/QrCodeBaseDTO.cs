namespace QrCode.API.DTOs;
public class QrCodeBaseDTO
{
    public int ID { get; set; }
    public string QRName { get; set; }
    public string URLValue { get; set; }
    public string HashValue { get; set; }
    public string QRPath { get; set; }
}
