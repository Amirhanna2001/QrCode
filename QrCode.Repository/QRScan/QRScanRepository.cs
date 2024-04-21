using LimitlessCareDrPortal.Repository;
using Microsoft.EntityFrameworkCore;
using QrCode.DB;
using QrCode.DB.Models;

namespace QrCode.Repository;
public class QRScanRepository : GenericRepository<QRScan>, IQRScanRepository
{
    private readonly DatabaseContext context;

    public QRScanRepository(DatabaseContext context) : base(context)
    {
        this.context = context;
    }

    public async Task<List<QRScan>> AllScansInTheSameQrCode(int id)
    {
        return await context.QRScans.Where(s=>s.QRCodeID == id).ToListAsync();
    }

    public async Task<List<QRScan>> GetAll()
    {
        return await context.QRScans.ToListAsync();
    }
}
