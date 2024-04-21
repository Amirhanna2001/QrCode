using System.ComponentModel.DataAnnotations.Schema;

namespace QrCode.DB.Models;
public class QRCode
{
    public int ID { get; set; }
    public string QRName { get; set; } 
    public string URLValue { get; set; }
    public string HashValue { get; set; }
    public string QRPath { get; set; }
    public bool IsDeleted { get; set; }
    [ForeignKey("User")]
    public string? UserId { get; set; }
    public AppUser User { get; set; }
}